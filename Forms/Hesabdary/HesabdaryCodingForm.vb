Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class HesabdaryCodingForm
        Inherits Form

        Private ReadOnly service As New AccountingService()

        Private Declare Auto Function SendMessage Lib "user32" (hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
        Private Const EM_SETCUEBANNER As Integer = &H1501

        ' Tree state variables
        Private _rootNodes As New List(Of CodingNode)()
        Private _nodeDict As New Dictionary(Of Integer, CodingNode)()



        ' Edit state variables
        Private _editAccountId As Integer? = Nothing     ' Nothing = new record
        Private _editParentId As Integer? = Nothing      ' Parent of new/edited record

        Public Property SelectMode As Boolean = False
        Public Property ReportSelectionMode As Boolean = False
        Public Property SelectedAccountID As Integer? = Nothing

        Public Property LevelMode As Boolean = False
        Public Property StartParentId As Integer? = Nothing
        Public Event LevelAccountSelected(accountId As Integer, accountCode As String)
        Public Event LevelPickerCloseRequested()

        ' Const button column names
        Private Const ColBtnSelect As String = "colBtnSelect"
        Private Const ColBtnEdit As String = "colBtnEdit"
        Private Const ColBtnDelete As String = "colBtnDelete"

        ' Tree node class
        Private NotInheritable Class CodingNode
            Public AccountID As Integer
            Public ParentAccountID As Integer?
            Public Level As Integer
            Public AccountCode As String
            Public AccountName As String
            Public AccountType As String
            Public IsActive As Boolean
            Public IsExpanded As Boolean
            Public AccountNature As String
            Public ReadOnly Children As New List(Of CodingNode)()

            Public ReadOnly Property HasChildren As Boolean
                Get
                    Return Children.Count > 0
                End Get
            End Property
        End Class

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryCodingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            Sys_Hes_Anb.Business.ThemeHelper.AppendStatusBar(Me)
            Try
                System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, "debug_load.txt"), "Form_Load started")
                LoadAccountTypes()
                LoadAccountNatures()
                SetupGrid()
                cmbSearchLevel.Visible = False 
                lblSearchLevel.Visible = False



                ' Align Docking order deterministically: Top -> Sath -> Search -> Data -> Grid
                Me.Controls.SetChildIndex(dgvAccounts, 0)
                Me.Controls.SetChildIndex(pnlData, 1)
                Me.Controls.SetChildIndex(pnlSearch, 2)
                Me.Controls.SetChildIndex(pnlSath, 3)
                Me.Controls.SetChildIndex(pnlTop, 4)
                
                SendMessage(txtSearchCode.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی کد...")
                SendMessage(txtSearchName.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی نام...")

                LoadData()
                
                Dim levelsObj = Sys_Hes_Anb.Data.Sql.ExecuteScalar("SELECT AccountLevels FROM Companies WHERE CompanyID = ?", SessionContext.CurrentCompanyID)
                Dim levels As Integer = If(levelsObj IsNot Nothing AndAlso Not Convert.IsDBNull(levelsObj), Convert.ToInt32(levelsObj), 4)
                If levels < 2 Then levels = 2
                If levels > 5 Then levels = 5

                cmbExpandToLevel.Items.Clear()
                Dim allItems As String() = {"گروه (بستن همه)", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"}
                For i As Integer = 0 To levels - 1
                    cmbExpandToLevel.Items.Add(allItems(i))
                Next

                ' Default to "Group" (Collapsed All)
                cmbExpandToLevel.SelectedIndex = 0
                ExpandTreeToLevel(0)

                If SelectMode Then
                    dgvAccounts.Columns(ColBtnSelect).Visible = True
                End If
                If ReportSelectionMode Then
                    dgvAccounts.Columns(ColBtnEdit).Visible = False
                    dgvAccounts.Columns(ColBtnDelete).Visible = False
                End If
                ApplySecurity()
                System.IO.File.AppendAllText(System.IO.Path.Combine(Application.StartupPath, "debug_load.txt"), Environment.NewLine & "Form_Load finished successfully")
            Catch ex As Exception
                System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, "debug_error.txt"), "Error in Form_Load: " & ex.Message & Environment.NewLine & ex.StackTrace)
            End Try
        End Sub

        Private Sub CmbExpandToLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExpandToLevel.SelectedIndexChanged
            If cmbExpandToLevel.SelectedIndex >= 0 Then
                ExpandTreeToLevel(cmbExpandToLevel.SelectedIndex)
            End If
        End Sub

        Private Sub ExpandTreeToLevel(maxLevel As Integer)
            For Each node In _nodeDict.Values
                If node.Level < maxLevel Then
                    node.IsExpanded = True
                Else
                    node.IsExpanded = False
                End If
            Next
            RefreshGrid()
        End Sub

        Public Sub RefreshData()
            Dim oldExpandedNodes As New HashSet(Of Integer)()
            If _nodeDict IsNot Nothing Then
                For Each node In _nodeDict.Values
                    If node.IsExpanded Then
                        oldExpandedNodes.Add(node.AccountID)
                    End If
                Next
            End If

            LoadData()

            If oldExpandedNodes.Count > 0 Then
                For Each node In _nodeDict.Values
                    If oldExpandedNodes.Contains(node.AccountID) Then
                        node.IsExpanded = True
                    Else
                        node.IsExpanded = False
                    End If
                Next
            ElseIf cmbExpandToLevel IsNot Nothing AndAlso cmbExpandToLevel.SelectedIndex >= 0 Then
                ExpandTreeToLevel(cmbExpandToLevel.SelectedIndex)
            End If

            RefreshGrid()
            HideDataPanel()
        End Sub

        Public Function IsGridInEditMode() As Boolean
            Return dgvAccounts.IsCurrentCellInEditMode
        End Function

        Private Sub SetupGrid()
            dgvAccounts.Columns.Clear()

            ' Toggle button (+ / -)
            Dim colToggle As New DataGridViewTextBoxColumn()
            colToggle.Name = "colToggle"
            colToggle.HeaderText = ""
            colToggle.Width = 35
            colToggle.ReadOnly = True
            colToggle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colSelect As New DataGridViewButtonColumn()
            colSelect.Name = ColBtnSelect
            colSelect.HeaderText = "انتخاب"
            colSelect.Text = "انتخاب"
            colSelect.UseColumnTextForButtonValue = True
            colSelect.Width = 64
            colSelect.FlatStyle = FlatStyle.Standard
            colSelect.Visible = False

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColBtnEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 64
            colEdit.FlatStyle = FlatStyle.Standard

            Dim colDel As New DataGridViewButtonColumn()
            colDel.Name = ColBtnDelete
            colDel.HeaderText = "حذف"
            colDel.Text = "حذف"
            colDel.UseColumnTextForButtonValue = True
            colDel.Width = 52
            colDel.FlatStyle = FlatStyle.Standard

            ' Hidden data columns
            Dim colAccountId As New DataGridViewTextBoxColumn()
            colAccountId.Name = "colAccountID"
            colAccountId.Visible = False

            Dim colParentId As New DataGridViewTextBoxColumn()
            colParentId.Name = "colParentAccountID"
            colParentId.Visible = False

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "colAccountType"
            colType.Visible = False

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colAccountCode"
            colCode.HeaderText = "کد حساب"
            colCode.Width = 120
            colCode.ReadOnly = True

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colAccountName"
            colName.HeaderText = "نام حساب"
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colName.ReadOnly = True

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "colIsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 52
            colActive.ReadOnly = True

            dgvAccounts.Columns.AddRange(New DataGridViewColumn() {
                colToggle, colSelect, colEdit, colDel,
                colAccountId, colParentId, colType,
                colCode, colName, colActive})
        End Sub

        Private Sub LoadData()
            Dim dt As DataTable
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(20, "دریافت اطلاعات سرفصل‌ها از پایگاه داده...")

                Try
                    dt = service.GetAccounts()
                Catch ex As Exception
                    MessageBox.Show("خطا در بارگذاری داده‌ها: " & ex.Message, "خطا",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                progress.UpdateProgress(60, "تحلیل ساختار درختی حساب‌ها...")

                _nodeDict.Clear()
                _rootNodes.Clear()

                For Each row As DataRow In dt.Rows
                    Dim node As New CodingNode()
                    node.AccountID = Convert.ToInt32(row("AccountID"))
                    node.AccountCode = Convert.ToString(row("AccountCode"))
                    node.AccountName = Convert.ToString(row("AccountName"))
                    node.AccountType = Convert.ToString(row("AccountType"))
                    node.IsActive = If(row.IsNull("IsActive"), True, Convert.ToBoolean(row("IsActive")))
                    node.AccountNature = Convert.ToString(row("AccountNature"))
                    node.ParentAccountID = If(row.IsNull("ParentAccountID"),
                                              CType(Nothing, Integer?),
                                              CType(Convert.ToInt32(row("ParentAccountID")), Integer?))
                    node.IsExpanded = True
                    _nodeDict(node.AccountID) = node
                Next

                For Each node In _nodeDict.Values
                    If node.ParentAccountID.HasValue AndAlso _nodeDict.ContainsKey(node.ParentAccountID.Value) Then
                        _nodeDict(node.ParentAccountID.Value).Children.Add(node)
                    Else
                        _rootNodes.Add(node)
                    End If
                Next

                SetLevels(_rootNodes, 0)
                progress.UpdateProgress(100, "بارگذاری درختی کامل شد")
            End Using
        End Sub

        Private Sub SetLevels(nodes As List(Of CodingNode), level As Integer)
            If level > 20 Then Return
            For Each node In nodes
                node.Level = level
                SetLevels(node.Children, level + 1)
            Next
        End Sub

        Private Sub RefreshGrid()
            Try
                If dgvAccounts Is Nothing OrElse dgvAccounts.Columns.Count = 0 Then Return
                dgvAccounts.SuspendLayout()
                dgvAccounts.Rows.Clear()

                Dim displayList As New List(Of CodingNode)()
                BuildDisplayList(_rootNodes, displayList)

                ' Write debug log
                Try
                    Dim logLines As New List(Of String)()
                    logLines.Add("=== RefreshGrid Debug ===")
                    logLines.Add("displayList count: " & displayList.Count)
                    logLines.Add("dgvAccounts columns count: " & dgvAccounts.Columns.Count)
                    For Each col As DataGridViewColumn In dgvAccounts.Columns
                        logLines.Add(String.Format("Col Name: {0}, Header: {1}, Type: {2}, Visible: {3}",
                                                   col.Name, col.HeaderText, col.GetType().Name, col.Visible))
                    Next
                    For Each n In displayList
                        logLines.Add(String.Format("ID: {0}, Code: {1}, Name: {2}, Level: {3}, Expanded: {4}, HasChildren: {5}",
                                                   n.AccountID, n.AccountCode, n.AccountName, n.Level, n.IsExpanded, n.HasChildren))
                    Next
                    System.IO.File.WriteAllLines(System.IO.Path.Combine(Application.StartupPath, "debug_grid.txt"), logLines.ToArray())
                Catch ex As Exception
                    ' Ignore logging errors
                End Try

                For Each node In displayList
                    Dim rowIdx = dgvAccounts.Rows.Add()
                    Dim row = dgvAccounts.Rows(rowIdx)
                    row.Tag = node

                    row.Cells("colToggle").Value = GetToggleText(node)
                    row.Cells("colAccountID").Value = node.AccountID
                    row.Cells("colParentAccountID").Value = node.ParentAccountID
                    row.Cells("colAccountType").Value = node.AccountType
                    row.Cells("colAccountCode").Value = node.AccountCode
                    
                    Dim indentSpaces As String = New String(Convert.ToChar(160), node.Level * 6)
                    row.Cells("colAccountName").Value = indentSpaces & node.AccountName
                    row.Cells("colIsActive").Value = node.IsActive

                    ApplyRowStyle(row, node)
                Next

                dgvAccounts.ResumeLayout()
                UpdateLevelLabel()
                UpdateSathPanel()
            Catch ex As Exception
                MessageBox.Show("خطا در به‌روزرسانی جدول: " & ex.Message & Environment.NewLine & ex.StackTrace, "خطای جدول", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BuildDisplayList(nodes As List(Of CodingNode), result As List(Of CodingNode))
            Dim codeF = txtSearchCode.Text.Trim()
            Dim nameF = txtSearchName.Text.Trim()
            Dim hasFilter = (codeF.Length > 0 OrElse nameF.Length > 0)

            For Each node In nodes
                Dim matchesSearch = True
                If hasFilter Then
                    If codeF.Length > 0 AndAlso Not node.AccountCode.Contains(codeF) Then matchesSearch = False
                    If nameF.Length > 0 AndAlso Not node.AccountName.Contains(nameF) Then matchesSearch = False
                End If

                Dim showNode = True
                If hasFilter Then
                    showNode = matchesSearch OrElse HasAnyMatchingChild(node, codeF, nameF)
                End If

                If showNode Then
                    result.Add(node)
                    If hasFilter Then
                        node.IsExpanded = True
                    End If

                    If node.IsExpanded AndAlso node.HasChildren Then
                        BuildDisplayList(node.Children, result)
                    End If
                End If
            Next
        End Sub

        Private Function HasAnyMatchingChild(node As CodingNode, codeF As String, nameF As String) As Boolean
            For Each child In node.Children
                Dim matches = True
                If codeF.Length > 0 AndAlso Not child.AccountCode.Contains(codeF) Then matches = False
                If nameF.Length > 0 AndAlso Not child.AccountName.Contains(nameF) Then matches = False
                If matches Then Return True
                If HasAnyMatchingChild(child, codeF, nameF) Then Return True
            Next
            Return False
        End Function

        Private Function GetToggleText(node As CodingNode) As String
            If Not node.HasChildren Then Return ""
            Return If(node.IsExpanded, "−", "+")
        End Function

        Private Sub ApplyRowStyle(row As DataGridViewRow, node As CodingNode)
            Select Case node.Level
                Case 0
                    row.DefaultCellStyle.Font = New Font(dgvAccounts.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(195, 218, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(15, 30, 80)
                Case 1
                    row.DefaultCellStyle.Font = New Font(dgvAccounts.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(215, 232, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 100)
                Case 2
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 244, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 60, 120)
                Case 3
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case 4
                    row.DefaultCellStyle.BackColor = Color.FromArgb(252, 254, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case Else
                    row.DefaultCellStyle.BackColor = Color.White
                    row.DefaultCellStyle.ForeColor = Color.Black
            End Select
        End Sub

        Private Sub TxtSearchCode_TextChanged(sender As Object, e As EventArgs) Handles txtSearchCode.TextChanged
            RefreshGrid()
        End Sub

        Private Sub TxtSearchName_TextChanged(sender As Object, e As EventArgs) Handles txtSearchName.TextChanged
            RefreshGrid()
        End Sub

        Private Sub UpdateLevelLabel()
            lblCurrentLevel.Text = "نمایش درختی و ساختار سلسله‌مراتبی سرفصل‌های حساب"
        End Sub

        Private Sub UpdateSathPanel()
            If lblSathInfo Is Nothing Then Return
            If dgvAccounts.CurrentRow Is Nothing Then
                lblSathInfo.Text = "سطح سرفصل جاری: -"
                Return
            End If

            Dim node = TryCast(dgvAccounts.CurrentRow.Tag, CodingNode)
            If node Is Nothing Then
                lblSathInfo.Text = "سطح سرفصل جاری: -"
                Return
            End If

            Dim levelNames As String() = {"گروه", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"}
            Dim currentLevelName = If(node.Level < levelNames.Length, levelNames(node.Level), "تفضیلی")

            Dim chain As New List(Of CodingNode)()
            Dim curr = node
            While curr IsNot Nothing
                chain.Insert(0, curr)
                curr = If(curr.ParentAccountID.HasValue AndAlso _nodeDict.ContainsKey(curr.ParentAccountID.Value), _nodeDict(curr.ParentAccountID.Value), Nothing)
            End While

            Dim chainParts As New List(Of String)()
            For Each item In chain
                Dim lvlName = If(item.Level < levelNames.Length, levelNames(item.Level), "تفضیلی")
                chainParts.Add(String.Format("{0}: {1} ({2})", lvlName, item.AccountCode, item.AccountName))
            Next

            Dim chainStr = String.Join(" 🡨 ", chainParts.ToArray())
            lblSathInfo.Text = String.Format("سطح سرفصل جاری: {0}  /  زنجیره: {1}", currentLevelName, chainStr)
        End Sub

        Private Sub LoadAccountTypes()
            cmbAccountType.Items.Clear()
            cmbAccountType.Items.AddRange(New Object() {
                "دارایی",
                "بدهی",
                "حقوق صاحبان سرمایه",
                "درآمد",
                "هزینه",
                "حسابهای انتظامی",
                "سایر"})
            If cmbAccountType.Items.Count > 0 Then cmbAccountType.SelectedIndex = 0
        End Sub

        Private Sub DgvAccounts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvAccounts.Columns(e.ColumnIndex).Name = "colToggle" Then
                ToggleNode(e.RowIndex)
            End If
        End Sub

        Private Sub ToggleNode(rowIndex As Integer)
            Dim node = TryCast(dgvAccounts.Rows(rowIndex).Tag, CodingNode)
            If node Is Nothing OrElse Not node.HasChildren Then Return
            node.IsExpanded = Not node.IsExpanded
            RefreshGrid()
        End Sub

        Private Sub DgvAccounts_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAccounts.SelectionChanged
            UpdateSathPanel()
        End Sub

        Private Sub DgvAccounts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim row = dgvAccounts.Rows(e.RowIndex)
            Dim accountIdVal = row.Cells("colAccountID").Value
            If accountIdVal Is Nothing OrElse accountIdVal Is DBNull.Value Then Return
            Dim accountId = Convert.ToInt32(accountIdVal)
            Dim accountName = Convert.ToString(row.Cells("colAccountName").Value)
            Dim colName = dgvAccounts.Columns(e.ColumnIndex).Name

            Select Case colName
                Case ColBtnEdit
                    Dim parentVal = row.Cells("colParentAccountID").Value
                    Dim parentId As Integer? = Nothing
                    If parentVal IsNot Nothing AndAlso parentVal IsNot DBNull.Value Then
                        parentId = Convert.ToInt32(parentVal)
                    End If
                    ShowDataPanel(accountId, parentId)

                Case ColBtnSelect
                    Dim selectedNode = TryCast(row.Tag, CodingNode)
                    If LevelMode Then
                        SelectedAccountID = accountId
                        RaiseEvent LevelAccountSelected(accountId, Convert.ToString(row.Cells("colAccountCode").Value))
                    ElseIf selectedNode IsNot Nothing AndAlso selectedNode.HasChildren AndAlso Not ReportSelectionMode Then
                        MessageBox.Show("این سرفصل حساب ، دارای زیر سطح می باشد",
                                        "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        SelectedAccountID = accountId
                        Me.Close()
                    End If

                Case ColBtnDelete
                    if service.AccountHasChildren(accountId) Then
                        MessageBox.Show("این سرفصل دارای زیرمجموعه است و قابل حذف نیست." & Environment.NewLine &
                                        "ابتدا زیرمجموعه‌های آن را حذف کنید.",
                                        "امکان حذف وجود ندارد", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    If MessageBox.Show("سرفصل «" & accountName & "» حذف شود؟",
                                       "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            service.DeleteAccount(accountId)
                            RefreshData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف: " & ex.Message, "خطا",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
            End Select
        End Sub

        Private Sub dgvAccounts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAccounts.CellFormatting
            If e.RowIndex < 0 Then Return
            Dim colName = dgvAccounts.Columns(e.ColumnIndex).Name

            If colName = "colAccountName" Then
                Dim node = TryCast(dgvAccounts.Rows(e.RowIndex).Tag, CodingNode)
                If node IsNot Nothing Then
                    dgvAccounts.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.Padding =
                        New Padding(0, 0, node.Level * 20, 0)
                End If
            ElseIf colName = "colToggle" Then
                Dim txt = Convert.ToString(e.Value)
                If txt = "+" OrElse txt = "−" Then
                    e.CellStyle.BackColor = Color.FromArgb(225, 235, 255)
                    e.CellStyle.SelectionBackColor = Color.FromArgb(100, 130, 200)
                End If
            End If
        End Sub

        Private Sub dgvAccounts_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellMouseEnter
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvAccounts.Columns(e.ColumnIndex).Name = "colToggle" Then
                Dim node = TryCast(dgvAccounts.Rows(e.RowIndex).Tag, CodingNode)
                If node IsNot Nothing AndAlso node.HasChildren Then
                    dgvAccounts.Cursor = Cursors.Hand
                End If
            End If
        End Sub

        Private Sub dgvAccounts_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellMouseLeave
            dgvAccounts.Cursor = Cursors.Default
        End Sub

        Private Sub ShowDataPanel(accountId As Integer?, parentId As Integer?)
            If Not accountId.HasValue Then
                Dim settings = service.GetCompanyAccountSettings()
                Dim maxLevels = settings.Item1
                Dim newLevel = 1
                If parentId.HasValue Then
                    newLevel = service.GetAccountHierarchyChain(parentId.Value).Count + 1
                End If
                If newLevel > maxLevels Then
                    MessageBox.Show(String.Format("با توجه به تنظیمات شرکت، امکان ایجاد سرفصل در سطح {0} وجود ندارد (حداکثر {1} سطح مجاز است).", newLevel, maxLevels),
                                    "خطای سطح حساب", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            _editAccountId = accountId
            _editParentId = parentId

            Dim isRootLevel = Not parentId.HasValue
            lblDataType.Visible = isRootLevel
            cmbAccountType.Visible = isRootLevel
            lblAccountNature.Visible = isRootLevel
            cmbAccountNature.Visible = isRootLevel

            Dim compSettings = service.GetCompanyAccountSettings()
            Dim currentLevel = 1
            If parentId.HasValue Then
                currentLevel = service.GetAccountHierarchyChain(parentId.Value).Count + 1
            ElseIf accountId.HasValue Then
                currentLevel = service.GetAccountHierarchyChain(accountId.Value).Count
            End If

            Dim codeLength = 2
            Select Case currentLevel
                Case 1: codeLength = compSettings.Item2
                Case 2: codeLength = compSettings.Item3
                Case 3: codeLength = compSettings.Item4
                Case 4: codeLength = compSettings.Item5
                Case 5: codeLength = compSettings.Item6
                Case 6: codeLength = compSettings.Item7
            End Select

            txtAccountCode.MaxLength = codeLength

            If accountId.HasValue Then
                Dim foundNode As CodingNode = Nothing
                For Each row As DataGridViewRow In dgvAccounts.Rows
                    Dim rowId = row.Cells("colAccountID").Value
                    If rowId IsNot Nothing AndAlso Not Convert.IsDBNull(rowId) AndAlso Convert.ToInt32(rowId) = accountId.Value Then
                        foundNode = TryCast(row.Tag, CodingNode)
                        Exit For
                    End If
                Next
                
                If foundNode IsNot Nothing Then
                    txtAccountCode.Text = foundNode.AccountCode
                    txtAccountName.Text = foundNode.AccountName
                    If isRootLevel Then
                        cmbAccountType.Text = foundNode.AccountType
                        If String.IsNullOrEmpty(cmbAccountType.Text) AndAlso cmbAccountType.Items.Count > 0 Then
                            cmbAccountType.SelectedIndex = 0
                        End If
                        cmbAccountNature.Text = GetNatureText(foundNode.AccountNature)
                    End If
                    chkActive.Checked = foundNode.IsActive
                End If
            Else
                Dim suggestedCode = service.GetNextSuggestedCode(parentId)
                If suggestedCode.Length < codeLength AndAlso Long.TryParse(suggestedCode, New Long) Then
                    suggestedCode = suggestedCode.PadLeft(codeLength, "0"c)
                End If
                txtAccountCode.Text = suggestedCode
                txtAccountName.Clear()
                If isRootLevel Then
                    If cmbAccountType.Items.Count > 0 Then cmbAccountType.SelectedIndex = 0
                    If cmbAccountNature.Items.Count > 0 Then cmbAccountNature.SelectedIndex = 2
                End If
                chkActive.Checked = True
            End If

            pnlData.Visible = True
            txtAccountCode.Focus()
            txtAccountCode.SelectAll()
        End Sub

        Private Sub HideDataPanel()
            pnlData.Visible = False
            _editAccountId = Nothing
            _editParentId = Nothing
        End Sub

        Private Sub txtAccountCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccountCode.KeyPress
            If Not (e.KeyChar >= "0"c AndAlso e.KeyChar <= "9"c) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Dim parentId As Integer? = Nothing
            If dgvAccounts.CurrentRow IsNot Nothing Then
                Dim selectedNode = TryCast(dgvAccounts.CurrentRow.Tag, CodingNode)
                If selectedNode IsNot Nothing Then
                    Dim levelNames As String() = {"گروه", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"}
                    Dim currentLevelName = If(selectedNode.Level < levelNames.Length, levelNames(selectedNode.Level), "تفضیلی")
                    Dim childLevelName = If(selectedNode.Level + 1 < levelNames.Length, levelNames(selectedNode.Level + 1), "تفضیلی")

                    Dim res = MessageBox.Show("آیا می‌خواهید حساب جدید را به عنوان زیرمجموعه «" & childLevelName & "» برای «" & currentLevelName & "» «" & selectedNode.AccountName & "» تعریف کنید؟" & Environment.NewLine & "در غیر این صورت، حساب به عنوان یک سرفصل اصلی (گروه) ثبت خواهد شد.", "تعیین حساب والد", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                    If res = DialogResult.Cancel Then Return
                    If res = DialogResult.Yes Then
                        parentId = selectedNode.AccountID
                    End If
                End If
            End If
            ShowDataPanel(Nothing, parentId)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtAccountCode.Text) Then
                MessageBox.Show("کد حساب نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountCode.Focus()
                Return
            End If

            Dim settings = service.GetCompanyAccountSettings()
            Dim level = 1
            If _editParentId.HasValue Then
                level = service.GetAccountHierarchyChain(_editParentId.Value).Count + 1
            ElseIf _editAccountId.HasValue Then
                level = service.GetAccountHierarchyChain(_editAccountId.Value).Count
            End If

            Dim codeLength = 2
            Select Case level
                Case 1: codeLength = settings.Item2
                Case 2: codeLength = settings.Item3
                Case 3: codeLength = settings.Item4
                Case 4: codeLength = settings.Item5
                Case 5: codeLength = settings.Item6
                Case 6: codeLength = settings.Item7
            End Select

            Dim enteredCode = txtAccountCode.Text.Trim()
            If enteredCode.Length < codeLength Then
                enteredCode = enteredCode.PadLeft(codeLength, "0"c)
                txtAccountCode.Text = enteredCode
            End If

            If enteredCode.Length > codeLength Then
                MessageBox.Show(String.Format("طول کد حساب نمی‌تواند بیشتر از {0} کاراکتر باشد.", codeLength), "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountCode.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(txtAccountName.Text) Then
                MessageBox.Show("نام حساب نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountName.Focus()
                Return
            End If

            Dim accountType As String
            Dim accountNature As String
            If _editParentId.HasValue Then
                accountType = service.GetRootAncestorAccountType(_editParentId.Value)
                accountNature = service.GetRootAncestorAccountNature(_editParentId.Value)
            Else
                accountType = cmbAccountType.Text
                accountNature = GetNatureValue(cmbAccountNature.Text)
            End If

            Try
                service.SaveAccount(
                    _editAccountId,
                    txtAccountCode.Text.Trim(),
                    txtAccountName.Text.Trim(),
                    accountType,
                    _editParentId,
                    chkActive.Checked,
                    accountNature)
                HideDataPanel()
                RefreshData()
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "کد حساب تکراری",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountCode.Focus()
                txtAccountCode.SelectAll()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره سرفصل: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadAccountNatures()
            cmbAccountNature.Items.Clear()
            cmbAccountNature.Items.AddRange(New Object() {
                "مانده حساب فقط بدهکار",
                "مانده حساب فقط بستانکار",
                "مانده هم بدهکار ، هم بستانکار"
            })
            If cmbAccountNature.Items.Count > 0 Then cmbAccountNature.SelectedIndex = 2
        End Sub

        Private Function GetNatureText(natureVal As String) As String
            Select Case natureVal
                Case "Bedehkar"
                    Return "مانده حساب فقط بدهکار"
                Case "Bestankar"
                    Return "مانده حساب فقط بستانکار"
                Case "Both", ""
                    Return "مانده هم بدهکار ، هم بستانکار"
                Case Else
                    Return "مانده هم بدهکار ، هم بستانکار"
            End Select
        End Function

        Private Function GetNatureValue(natureText As String) As String
            Select Case natureText
                Case "مانده حساب فقط بدهکار"
                    Return "Bedehkar"
                Case "مانده حساب فقط بستانکار"
                    Return "Bestankar"
                Case "مانده هم بدهکار ، هم بستانکار"
                    Return "Both"
                Case Else
                    Return "Both"
            End Select
        End Function

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            HideDataPanel()
        End Sub

        Private Sub HesabdaryCodingForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.Escape Then
                If Not dgvAccounts.IsCurrentCellInEditMode Then
                    If LevelMode Then
                        RaiseEvent LevelPickerCloseRequested()
                    End If
                    If Me.TopLevel Then
                        Me.Close()
                        e.Handled = True
                    End If
                End If
            End If
        End Sub

        Private Sub dgvAccounts_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellDoubleClick
            If Not LevelMode Then Return
            If e.RowIndex < 0 Then Return
            Dim row = dgvAccounts.Rows(e.RowIndex)
            Dim accountIdVal = row.Cells("colAccountID").Value
            If accountIdVal Is Nothing OrElse accountIdVal Is DBNull.Value Then Return
            Dim accountId = Convert.ToInt32(accountIdVal)
            SelectedAccountID = accountId
            RaiseEvent LevelAccountSelected(accountId, Convert.ToString(row.Cells("colAccountCode").Value))
        End Sub

        Private Sub AlignSearchControls()
            Try
                If dgvAccounts Is Nothing OrElse txtSearchCode Is Nothing OrElse txtSearchName Is Nothing Then Return
                If dgvAccounts.Columns.Count = 0 Then Return

                Dim codeColIdx As Integer = -1
                Dim nameColIdx As Integer = -1
                For i As Integer = 0 To dgvAccounts.Columns.Count - 1
                    If dgvAccounts.Columns(i).Name = "colAccountCode" Then
                        codeColIdx = i
                    ElseIf dgvAccounts.Columns(i).Name = "colAccountName" Then
                        nameColIdx = i
                    End If
                Next

                If codeColIdx = -1 OrElse nameColIdx = -1 Then Return

                ' Align txtSearchCode
                Dim codeRect = dgvAccounts.GetCellDisplayRectangle(codeColIdx, -1, True)
                If codeRect.Width > 0 Then
                    Dim screenPt = dgvAccounts.PointToScreen(New Point(codeRect.Left, 0))
                    Dim clientPt = pnlSearch.PointToClient(screenPt)
                    txtSearchCode.Left = clientPt.X
                    txtSearchCode.Width = codeRect.Width
                    txtSearchCode.Visible = True
                Else
                    txtSearchCode.Visible = False
                End If

                ' Align txtSearchName
                Dim nameRect = dgvAccounts.GetCellDisplayRectangle(nameColIdx, -1, True)
                If nameRect.Width > 0 Then
                    Dim screenPt = dgvAccounts.PointToScreen(New Point(nameRect.Left, 0))
                    Dim clientPt = pnlSearch.PointToClient(screenPt)
                    txtSearchName.Left = clientPt.X
                    txtSearchName.Width = nameRect.Width
                    txtSearchName.Visible = True
                Else
                    txtSearchName.Visible = False
                End If
            Catch ex As Exception
                ' Prevent crash
            End Try
        End Sub

        Private Sub dgvAccounts_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvAccounts.ColumnWidthChanged
            AlignSearchControls()
        End Sub

        Private Sub dgvAccounts_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvAccounts.Scroll
            AlignSearchControls()
        End Sub

        Private Sub dgvAccounts_Resize(sender As Object, e As EventArgs) Handles dgvAccounts.Resize
            AlignSearchControls()
        End Sub

        Protected Overrides Sub OnShown(e As EventArgs)
            MyBase.OnShown(e)
            AlignSearchControls()
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim userType = SessionContext.CurrentUser.UserType
            Dim isSuperAdmin = String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim hasGlobalAccounting = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAccounting)

            Dim canCreate = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingHeader & PermissionKeys.CanCreate)
            Dim canEdit = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingHeader & PermissionKeys.CanEdit)
            Dim canDelete = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingHeader & PermissionKeys.CanDelete)

            btnNew.Visible = canCreate
            btnSave.Visible = canCreate OrElse canEdit

            If dgvAccounts.Columns.Contains(ColBtnEdit) Then
                dgvAccounts.Columns(ColBtnEdit).Visible = canEdit AndAlso Not ReportSelectionMode
            End If
            If dgvAccounts.Columns.Contains(ColBtnDelete) Then
                dgvAccounts.Columns(ColBtnDelete).Visible = canDelete AndAlso Not ReportSelectionMode
            End If
        End Sub

    End Class
End Namespace
