Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class SelectAccountsRangeForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        
        Public Property SelectedLevel As Integer ' 1-indexed (1: Group, 2: General, 3: Subsidiary, etc.)
        Public Property SelectedFromCode As String = String.Empty
        Public Property SelectedToCode As String = String.Empty
        Public Property SelectedAccounts As New List(Of Tuple(Of Integer, String, String))() ' List of (AccountID, Code, Name)

        Private cmbLevelSelector As ComboBox
        Private cmbFromAccount As ComboBox
        Private cmbToAccount As ComboBox
        Private btnOk As Button
        Private btnCancel As Button

        Private _allAccounts As New List(Of Tuple(Of Integer, String, String, Integer))() ' (ID, Code, Name, Level)

        Public Sub New()
            InitializeComponent()
            LoadCompanyLevels()
            LoadAllAccounts()
            PopulateLevels()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "انتخاب محدوده سرفصل‌ها"
            Me.Size = New Size(400, 250)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.StartPosition = FormStartPosition.CenterParent
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)

            Dim lblLevel As New Label() With {.Text = "سطح سرفصل:", .Location = New Point(280, 20), .Size = New Size(90, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbLevelSelector = New ComboBox() With {.Location = New Point(30, 20), .Size = New Size(240, 22), .DropDownStyle = ComboBoxStyle.DropDownList}
            AddHandler cmbLevelSelector.SelectedIndexChanged, AddressOf CmbLevelSelector_SelectedIndexChanged

            Dim lblFrom As New Label() With {.Text = "از سرفصل:", .Location = New Point(280, 60), .Size = New Size(90, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbFromAccount = New ComboBox() With {.Location = New Point(30, 60), .Size = New Size(240, 22), .DropDownStyle = ComboBoxStyle.DropDownList, .DropDownWidth = 350}

            Dim lblTo As New Label() With {.Text = "تا سرفصل:", .Location = New Point(280, 100), .Size = New Size(90, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbToAccount = New ComboBox() With {.Location = New Point(30, 100), .Size = New Size(240, 22), .DropDownStyle = ComboBoxStyle.DropDownList, .DropDownWidth = 350}

            btnOk = New Button() With {.Text = "تأیید", .Location = New Point(150, 150), .Size = New Size(100, 32), .DialogResult = DialogResult.OK}
            AddHandler btnOk.Click, AddressOf BtnOk_Click

            btnCancel = New Button() With {.Text = "انصراف", .Location = New Point(30, 150), .Size = New Size(100, 32), .DialogResult = DialogResult.Cancel}

            Me.Controls.Add(lblLevel)
            Me.Controls.Add(cmbLevelSelector)
            Me.Controls.Add(lblFrom)
            Me.Controls.Add(cmbFromAccount)
            Me.Controls.Add(lblTo)
            Me.Controls.Add(cmbToAccount)
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
                Dim dt = Sys_Hes_Anb.Data.Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountName, ParentAccountID FROM ChartOfAccounts WHERE CompanyID = ? AND IsActive = 1 ORDER BY AccountCode", SessionContext.CurrentCompanyID.Value)
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

            cmbFromAccount.Items.Clear()
            cmbToAccount.Items.Clear()

            For Each acc In _allAccounts
                If acc.Item4 = selectedLvl Then
                    Dim itemText = acc.Item2 & " - " & acc.Item3
                    cmbFromAccount.Items.Add(New ComboItem(acc.Item1, acc.Item2, itemText))
                    cmbToAccount.Items.Add(New ComboItem(acc.Item1, acc.Item2, itemText))
                End If
            Next

            If cmbFromAccount.Items.Count > 0 Then cmbFromAccount.SelectedIndex = 0
            If cmbToAccount.Items.Count > 0 Then cmbToAccount.SelectedIndex = cmbToAccount.Items.Count - 1
        End Sub

        Private Sub CmbLevelSelector_SelectedIndexChanged(sender As Object, e As EventArgs)
            PopulateLevels()
        End Sub

        Private Sub BtnOk_Click(sender As Object, e As EventArgs)
            If cmbLevelSelector.SelectedIndex < 0 OrElse cmbFromAccount.SelectedItem Is Nothing OrElse cmbToAccount.SelectedItem Is Nothing Then
                MessageBox.Show("لطفاً سطح و محدوده حساب‌ها را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.DialogResult = DialogResult.None
                Return
            End If

            Dim fromItem = DirectCast(cmbFromAccount.SelectedItem, ComboItem)
            Dim toItem = DirectCast(cmbToAccount.SelectedItem, ComboItem)

            If String.Compare(fromItem.Code, toItem.Code, StringComparison.OrdinalIgnoreCase) > 0 Then
                MessageBox.Show("کد شروع نمی‌تواند بزرگتر از کد پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.DialogResult = DialogResult.None
                Return
            End If

            Me.SelectedLevel = cmbLevelSelector.SelectedIndex + 1
            Me.SelectedFromCode = fromItem.Code
            Me.SelectedToCode = toItem.Code

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

        Private Class ComboItem
            Public Property ID As Integer
            Public Property Code As String
            Public Property Text As String

            Public Sub New(id As Integer, code As String, text As String)
                Me.ID = id
                Me.Code = code
                Me.Text = text
            End Sub

            Public Overrides Function ToString() As String
                Return Me.Text
            End Function
        End Class
    End Class
End Namespace
