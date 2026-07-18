Imports System
Imports System.Data
Imports System.Data.SQLite
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data
Imports Sys_Hes_Anb.Models
Imports System.Collections.Generic

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryBalanceSheetMappingForm
        Inherits Form

        Private tvAccounts As TreeView
        Private lstCategories As ListBox
        Private btnSave As Button
        Private btnCancel As Button
        Private lblTitle As Label
        Private pnlBottom As Panel
        Private splitContainer As SplitContainer

        Private _mappings As New Dictionary(Of String, HashSet(Of Integer))()
        Private _isUpdatingChecks As Boolean = False
        Private _currentCategory As String = ""

        ' Standard Balance Sheet Categories
        Private _categories As New Dictionary(Of String, String) From {
            {"CURR_ASSETS", "دارایی‌های جاری"},
            {"NON_CURR_ASSETS", "دارایی‌های غیرجاری (ثابت)"},
            {"CURR_LIABILITIES", "بدهی‌های جاری"},
            {"NON_CURR_LIABILITIES", "بدهی‌های غیرجاری"},
            {"EQUITY_CAPITAL", "سرمایه ثبت شده"},
            {"EQUITY_RESERVES", "اندوخته‌ها و سود/زیان انباشته"}
        }

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)
            LoadCategories()
            LoadAccountsTree()
            LoadExistingMappings()
            
            ' Ensure the event is fired or checks are updated explicitly
            If lstCategories.Items.Count > 0 Then
                lstCategories.SelectedIndex = -1 ' Reset to force change
                lstCategories.SelectedIndex = 0
            End If
            
            _isUpdatingChecks = True
            UpdateTreeChecks(tvAccounts.Nodes)
            _isUpdatingChecks = False
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "معرفی حساب‌های ترازنامه‌ای"
            Me.Size = New Size(900, 600)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True

            lblTitle = New Label()
            lblTitle.Text = "لطفاً ابتدا از لیست سمت راست یک گروه را انتخاب کرده، سپس در سمت چپ حساب‌های مربوط به آن را تیک بزنید:"
            lblTitle.Dock = DockStyle.Top
            lblTitle.Height = 40
            lblTitle.TextAlign = ContentAlignment.MiddleLeft
            lblTitle.Padding = New Padding(10, 0, 10, 0)

            pnlBottom = New Panel()
            pnlBottom.Dock = DockStyle.Bottom
            pnlBottom.Height = 50

            btnSave = New Button()
            btnSave.Text = "ذخیره تغییرات"
            btnSave.Size = New Size(120, 30)
            btnSave.Location = New Point(20, 10)
            btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            btnSave.BackColor = Color.FromArgb(46, 204, 113)
            btnSave.ForeColor = Color.White
            btnSave.FlatStyle = FlatStyle.Flat
            AddHandler btnSave.Click, AddressOf btnSave_Click

            btnCancel = New Button()
            btnCancel.Text = "انصراف"
            btnCancel.Size = New Size(100, 30)
            btnCancel.Location = New Point(150, 10)
            btnCancel.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            AddHandler btnCancel.Click, AddressOf btnCancel_Click

            pnlBottom.Controls.Add(btnSave)
            pnlBottom.Controls.Add(btnCancel)

            splitContainer = New SplitContainer()
            splitContainer.Dock = DockStyle.Fill
            splitContainer.SplitterDistance = 430

            ' Right Panel (Categories)
            lstCategories = New ListBox()
            lstCategories.Dock = DockStyle.Fill
            lstCategories.Font = New Font("Tahoma", 10.0!)
            AddHandler lstCategories.SelectedIndexChanged, AddressOf lstCategories_SelectedIndexChanged

            ' Left Panel (Tree)
            tvAccounts = New TreeView()
            tvAccounts.Dock = DockStyle.Fill
            tvAccounts.CheckBoxes = True
            tvAccounts.Font = New Font("Tahoma", 9.0!)
            AddHandler tvAccounts.AfterCheck, AddressOf tvAccounts_AfterCheck

            splitContainer.Panel1.Controls.Add(lstCategories)
            splitContainer.Panel2.Controls.Add(tvAccounts)

            Me.Controls.Add(splitContainer)
            Me.Controls.Add(lblTitle)
            Me.Controls.Add(pnlBottom)
        End Sub

        Private Sub LoadCategories()
            lstCategories.DisplayMember = "Value"
            lstCategories.ValueMember = "Key"
            For Each kvp In _categories
                lstCategories.Items.Add(New KeyValuePair(Of String, String)(kvp.Key, kvp.Value))
                _mappings(kvp.Key) = New HashSet(Of Integer)()
            Next
        End Sub

        Private Sub LoadAccountsTree()
            tvAccounts.Nodes.Clear()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim dt As DataTable = Sql.ExecuteTable("SELECT * FROM SarfaslHesab WHERE CompanyID = " & companyId & " ORDER BY AccountCode")
            
            ' 1. Load P&L mapped accounts to exclude
            Dim dtExcluded As DataTable = Sql.ExecuteTable("SELECT AccountID FROM PnLAccountMappings WHERE CompanyID = " & companyId)
            Dim excludedSet As New HashSet(Of Integer)()
            For Each row As DataRow In dtExcluded.Rows
                excludedSet.Add(Convert.ToInt32(row("AccountID")))
            Next

            ' 2. Build parent map for all accounts to trace ancestors
            Dim parentMap As New Dictionary(Of Integer, Integer)()
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("AccountID"))
                Dim parentId = If(Convert.IsDBNull(row("ParentAccountID")), 0, Convert.ToInt32(row("ParentAccountID")))
                parentMap(id) = parentId
            Next

            ' Helper to check if an account or any of its ancestors is excluded
            Dim isExcluded As Func(Of Integer, Boolean) =
                Function(id As Integer)
                    Dim current = id
                    Dim guard = 0
                    Do While current > 0 AndAlso guard < 100
                        guard += 1
                        If excludedSet.Contains(current) Then Return True
                        If parentMap.ContainsKey(current) Then
                            current = parentMap(current)
                        Else
                            current = 0
                        End If
                    Loop
                    Return False
                End Function

            Dim allNodes As New Dictionary(Of Integer, TreeNode)()
            Dim roots As New List(Of TreeNode)()

            ' Pass 1: Create all nodes
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("AccountID"))
                If isExcluded(id) Then Continue For

                Dim code = row("AccountCode").ToString()
                Dim name = row("AccountName").ToString()

                Dim node As New TreeNode(code & " - " & name)
                node.Tag = id
                allNodes(id) = node
            Next

            ' Pass 2: Link parents to children
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("AccountID"))
                If Not allNodes.ContainsKey(id) Then Continue For

                Dim parentId = If(Convert.IsDBNull(row("ParentAccountID")), 0, Convert.ToInt32(row("ParentAccountID")))
                
                Dim node = allNodes(id)
                If parentId = 0 OrElse Not allNodes.ContainsKey(parentId) Then
                    roots.Add(node)
                Else
                    allNodes(parentId).Nodes.Add(node)
                End If
            Next

            tvAccounts.Nodes.AddRange(roots.ToArray())
            tvAccounts.CollapseAll()
        End Sub

        Private Sub LoadExistingMappings()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Dim dt As DataTable = Sql.ExecuteTable("SELECT * FROM BalanceSheetAccountMappings WHERE CompanyID = " & SessionContext.CurrentCompanyID.Value)
            For Each row As DataRow In dt.Rows
                Dim catKey = row("CategoryKey").ToString()
                Dim accId = Convert.ToInt32(row("AccountID"))
                If _mappings.ContainsKey(catKey) Then
                    _mappings(catKey).Add(accId)
                End If
            Next
        End Sub

        Private Sub lstCategories_SelectedIndexChanged(sender As Object, e As EventArgs)
            If lstCategories.SelectedItem Is Nothing Then Return
            Dim item = DirectCast(lstCategories.SelectedItem, KeyValuePair(Of String, String))
            _currentCategory = item.Key

            _isUpdatingChecks = True
            UpdateTreeChecks(tvAccounts.Nodes)
            _isUpdatingChecks = False
        End Sub

        Private Sub UpdateTreeChecks(nodes As TreeNodeCollection)
            If String.IsNullOrEmpty(_currentCategory) Then Return
            Dim currentSet = _mappings(_currentCategory)

            For Each node As TreeNode In nodes
                Dim accId = CInt(node.Tag)
                node.Checked = currentSet.Contains(accId)
                UpdateTreeChecks(node.Nodes)
            Next
        End Sub

        Private Sub tvAccounts_AfterCheck(sender As Object, e As TreeViewEventArgs)
            If _isUpdatingChecks OrElse String.IsNullOrEmpty(_currentCategory) Then Return

            Dim accId = CInt(e.Node.Tag)
            Dim currentSet = _mappings(_currentCategory)

            If e.Node.Checked Then
                currentSet.Add(accId)
            Else
                currentSet.Remove(accId)
            End If

            _isUpdatingChecks = True
            CheckAllChildren(e.Node, e.Node.Checked, currentSet)
            _isUpdatingChecks = False
        End Sub

        Private Sub CheckAllChildren(parent As TreeNode, isChecked As Boolean, currentSet As HashSet(Of Integer))
            For Each child As TreeNode In parent.Nodes
                child.Checked = isChecked
                Dim accId = CInt(child.Tag)
                If isChecked Then
                    currentSet.Add(accId)
                Else
                    currentSet.Remove(accId)
                End If
                CheckAllChildren(child, isChecked, currentSet)
            Next
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Dim compId = SessionContext.CurrentCompanyID.Value
            Dim insertCount As Integer = 0

            Try
                Using conn As New SQLiteConnection(Db.ConnectionString)
                    conn.Open()
                    Using tr = conn.BeginTransaction()
                        ' Clear existing for this company
                        Using cmdDelete As New SQLiteCommand("DELETE FROM BalanceSheetAccountMappings WHERE CompanyID = @cid", conn, tr)
                            cmdDelete.Parameters.AddWithValue("@cid", compId)
                            cmdDelete.ExecuteNonQuery()
                        End Using

                        ' Insert new
                        Using cmdInsert As New SQLiteCommand("INSERT INTO BalanceSheetAccountMappings (CompanyID, CategoryKey, AccountID) VALUES (@cid, @key, @accid)", conn, tr)
                            For Each kvp In _mappings
                                For Each accId In kvp.Value
                                    cmdInsert.Parameters.Clear()
                                    cmdInsert.Parameters.AddWithValue("@cid", compId)
                                    cmdInsert.Parameters.AddWithValue("@key", kvp.Key)
                                    cmdInsert.Parameters.AddWithValue("@accid", accId)
                                    cmdInsert.ExecuteNonQuery()
                                    insertCount += 1
                                Next
                            Next
                        End Using
                        tr.Commit()
                    End Using
                End Using

                MessageBox.Show("تنظیمات با موفقیت ذخیره شد. تعداد رکوردهای ثبت شده: " & insertCount, "عملیات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
