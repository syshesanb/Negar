Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class SelectAccountsRangeForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        
        Public Property SelectedLevel As Integer ' 1-indexed (1: Group, 2: General, 3: Subsidiary, etc.)
        Public Property SelectedFromCode As String = String.Empty
        Public Property SelectedToCode As String = String.Empty
        Public Property SelectedFromChain As String = String.Empty
        Public Property SelectedToChain As String = String.Empty
        Public Property SelectedAccounts As New List(Of Tuple(Of Integer, String, String))() ' List of (AccountID, Code, Name)

        Private cmbLevelSelector As ComboBox
        Private lblFromCode As Label
        Private btnSelectFrom As Button
        Private lblToCode As Label
        Private btnSelectTo As Button
        Private btnOk As Button
        Private btnCancel As Button

        Private _allAccounts As New List(Of Tuple(Of Integer, String, String, Integer))() ' (ID, Code, Name, Level)

        Private _fromAccountId As Integer = 0
        Private _fromAccountCode As String = String.Empty
        Private _fromAccountName As String = String.Empty

        Private _toAccountId As Integer = 0
        Private _toAccountCode As String = String.Empty
        Private _toAccountName As String = String.Empty

        Public Sub New()
            InitializeComponent()
            LoadCompanyLevels()
            LoadAllAccounts()
            PopulateLevels()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "انتخاب محدوده سرفصل‌ها"
            Me.Size = New Size(540, 260)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.StartPosition = FormStartPosition.CenterParent
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)

            Dim lblLevel As New Label() With {.Text = "سطح سرفصل:", .Location = New Point(420, 20), .Size = New Size(90, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbLevelSelector = New ComboBox() With {.Location = New Point(150, 20), .Size = New Size(260, 22), .DropDownStyle = ComboBoxStyle.DropDownList}
            AddHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged

            lblFromCode = New Label() With {
                .Text = "از کد: (انتخاب نشده)",
                .Location = New Point(150, 60),
                .Size = New Size(360, 36),
                .TextAlign = ContentAlignment.MiddleLeft,
                .BorderStyle = BorderStyle.FixedSingle,
                .BackColor = Color.White
            }
            btnSelectFrom = New Button() With {.Text = "...", .Location = New Point(30, 60), .Size = New Size(100, 36)}
            AddHandler btnSelectFrom.Click, AddressOf BtnSelectFrom_Click

            lblToCode = New Label() With {
                .Text = "تا کد: (انتخاب نشده)",
                .Location = New Point(150, 110),
                .Size = New Size(360, 36),
                .TextAlign = ContentAlignment.MiddleLeft,
                .BorderStyle = BorderStyle.FixedSingle,
                .BackColor = Color.White
            }
            btnSelectTo = New Button() With {.Text = "...", .Location = New Point(30, 110), .Size = New Size(100, 36)}
            AddHandler btnSelectTo.Click, AddressOf BtnSelectTo_Click

            btnOk = New Button() With {.Text = "تأیید", .Location = New Point(150, 170), .Size = New Size(100, 32), .DialogResult = DialogResult.OK}
            AddHandler btnOk.Click, AddressOf BtnOk_Click

            btnCancel = New Button() With {.Text = "انصراف", .Location = New Point(30, 170), .Size = New Size(100, 32), .DialogResult = DialogResult.Cancel}

            Me.Controls.Add(lblLevel)
            Me.Controls.Add(cmbLevelSelector)
            Me.Controls.Add(lblFromCode)
            Me.Controls.Add(btnSelectFrom)
            Me.Controls.Add(lblToCode)
            Me.Controls.Add(btnSelectTo)
            Me.Controls.Add(btnOk)
            Me.Controls.Add(btnCancel)

            Me.AcceptButton = btnOk
            Me.CancelButton = btnCancel
        End Sub

        Private Sub LoadCompanyLevels()
            Try
                Dim settings = service.GetCompanyAccountSettings()
                Dim maxLevels = settings.Item1
                Dim levelNames = New String() {"گروه", "کل", "معین", "سطح ۴", "سطح ۵"}
                cmbLevelSelector.Items.Clear()
                For i = 0 To maxLevels - 1
                    Dim name = If(i < levelNames.Length, levelNames(i), "سطح " & (i + 1))
                    cmbLevelSelector.Items.Add(name)
                Next
                If cmbLevelSelector.Items.Count > 0 Then
                    cmbLevelSelector.SelectedIndex = 0
                End If
            Catch
            End Try
        End Sub

        Private Sub LoadAllAccounts()
            _allAccounts.Clear()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim dt = Negar.Data.Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountName, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ? AND IsActive = 1 ORDER BY AccountCode", SessionContext.CurrentCompanyID.Value)
                Dim parentMap As New Dictionary(Of Integer, Integer?)()
                For Each row As DataRow In dt.Rows
                    Dim id = Convert.ToInt32(row("AccountID"))
                    Dim pVal = row("ParentAccountID")
                    parentMap(id) = If(pVal Is Nothing OrElse Convert.IsDBNull(pVal), CType(Nothing, Integer?), Convert.ToInt32(pVal))
                Next

                Dim getLevel = Function(id As Integer) As Integer
                                   Dim lvl = 1
                                   Dim curr As Integer? = id
                                   Dim guard = 0
                                   Do While parentMap.ContainsKey(curr.Value) AndAlso parentMap(curr.Value).HasValue AndAlso guard < 50
                                       guard += 1
                                       curr = parentMap(curr.Value)
                                       lvl += 1
                                   Loop
                                   Return lvl
                               End Function

                For Each row As DataRow In dt.Rows
                    Dim id = Convert.ToInt32(row("AccountID"))
                    Dim code = Convert.ToString(row("AccountCode"))
                    Dim name = Convert.ToString(row("AccountName"))
                    Dim lvl = getLevel(id)
                    _allAccounts.Add(Tuple.Create(id, code, name, lvl))
                Next
            Catch
            End Try
        End Sub

        Private Sub PopulateLevels()
            If cmbLevelSelector.SelectedIndex < 0 Then Return
            Dim selectedLvl = cmbLevelSelector.SelectedIndex + 1

            Dim levelAccounts = _allAccounts.FindAll(Function(x) x.Item4 = selectedLvl)
            If levelAccounts.Count > 0 Then
                Dim firstAcc = levelAccounts(0)
                Dim lastAcc = levelAccounts(levelAccounts.Count - 1)

                _fromAccountId = firstAcc.Item1
                _fromAccountCode = firstAcc.Item2
                _fromAccountName = firstAcc.Item3

                _toAccountId = lastAcc.Item1
                _toAccountCode = lastAcc.Item2
                _toAccountName = lastAcc.Item3

                lblFromCode.Text = "از کد: " & GetFormattedAccountChain(_fromAccountId)
                lblToCode.Text = "تا کد: " & GetFormattedAccountChain(_toAccountId)
            Else
                _fromAccountId = 0
                _fromAccountCode = String.Empty
                _fromAccountName = String.Empty

                _toAccountId = 0
                _toAccountCode = String.Empty
                _toAccountName = String.Empty

                lblFromCode.Text = "از کد: (سرفصلی در این سطح وجود ندارد)"
                lblToCode.Text = "تا کد: (سرفصلی در این سطح وجود ندارد)"
            End If
        End Sub

        Private Function GetFormattedAccountChain(accountId As Integer) As String
            Try
                If accountId <= 0 Then Return String.Empty
                Dim chain = service.GetAccountHierarchyChain(accountId)
                Dim parts As New List(Of String)()
                For Each item In chain
                    parts.Add(item.Item1 & " — " & item.Item2)
                Next
                Return String.Join(" / ", parts.ToArray())
            Catch
                Return String.Empty
            End Try
        End Function

        Private Sub CmbLevelSelector_SelectedIndexChanged(sender As Object, e As EventArgs)
            PopulateLevels()
        End Sub

        Private Sub BtnSelectFrom_Click(sender As Object, e As EventArgs)
            Using frm As New HesabdaryCodingForm()
                frm.SelectMode = True
                frm.ReportSelectionMode = True
                frm.ShowDialog(Me)
                If frm.SelectedAccountID.HasValue Then
                    Dim accId = frm.SelectedAccountID.Value
                    Dim acc = _allAccounts.Find(Function(x) x.Item1 = accId)
                    If acc IsNot Nothing Then
                        _fromAccountId = acc.Item1
                        _fromAccountCode = acc.Item2
                        _fromAccountName = acc.Item3
                        
                        ' Update Level selector automatically to match selected account level
                        RemoveHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged
                        cmbLevelSelector.SelectedIndex = acc.Item4 - 1
                        AddHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged

                        lblFromCode.Text = "از کد: " & GetFormattedAccountChain(_fromAccountId)
                    End If
                End If
            End Using
        End Sub

        Private Sub BtnSelectTo_Click(sender As Object, e As EventArgs)
            Using frm As New HesabdaryCodingForm()
                frm.SelectMode = True
                frm.ReportSelectionMode = True
                frm.ShowDialog(Me)
                If frm.SelectedAccountID.HasValue Then
                    Dim accId = frm.SelectedAccountID.Value
                    Dim acc = _allAccounts.Find(Function(x) x.Item1 = accId)
                    If acc IsNot Nothing Then
                        _toAccountId = acc.Item1
                        _toAccountCode = acc.Item2
                        _toAccountName = acc.Item3
                        
                        ' Update Level selector automatically to match selected account level
                        RemoveHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged
                        cmbLevelSelector.SelectedIndex = acc.Item4 - 1
                        AddHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged

                        lblToCode.Text = "تا کد: " & GetFormattedAccountChain(_toAccountId)
                    End If
                End If
            End Using
        End Sub

        Private Sub BtnOk_Click(sender As Object, e As EventArgs)
            If _fromAccountId <= 0 OrElse _toAccountId <= 0 Then
                MessageBox.Show("لطفاً محدوده سرفصل‌ها را مشخص کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.DialogResult = DialogResult.None
                Return
            End If

            If String.Compare(_fromAccountCode, _toAccountCode, StringComparison.OrdinalIgnoreCase) > 0 Then
                MessageBox.Show("کد شروع نمی‌تواند بزرگتر از کد پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.DialogResult = DialogResult.None
                Return
            End If

            Me.SelectedLevel = cmbLevelSelector.SelectedIndex + 1
            Me.SelectedFromCode = _fromAccountCode
            Me.SelectedToCode = _toAccountCode
            Me.SelectedFromChain = GetFormattedAccountChain(_fromAccountId)
            Me.SelectedToChain = GetFormattedAccountChain(_toAccountId)

            ' Populate selected accounts
            Me.SelectedAccounts.Clear()
            For Each acc In _allAccounts
                If acc.Item4 = Me.SelectedLevel AndAlso
                   String.Compare(acc.Item2, Me.SelectedFromCode, StringComparison.OrdinalIgnoreCase) >= 0 AndAlso
                   String.Compare(acc.Item2, Me.SelectedToCode, StringComparison.OrdinalIgnoreCase) <= 0 Then
                    Me.SelectedAccounts.Add(Tuple.Create(acc.Item1, acc.Item2, acc.Item3))
                End If
            Next
        End Sub
    End Class
End Namespace
