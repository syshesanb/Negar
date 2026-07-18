Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class UserManagementUsersForm
        Inherits Form

        Private ReadOnly service As New UserService()
        Private ReadOnly _ordinaryOnly As Boolean
        Private _selectedUserId As Integer?

        Private NotInheritable Class ComboOption
            Private _text As String
            Private _value As String

            Public ReadOnly Property Text As String
                Get
                    Return _text
                End Get
            End Property

            Public ReadOnly Property Value As String
                Get
                    Return _value
                End Get
            End Property

            Public Sub New(displayText As String, value As String)
                _text = displayText
                _value = value
            End Sub

            Public Overrides Function ToString() As String
                Return Text
            End Function
        End Class

        Public Sub New(ordinaryOnly As Boolean)
            _ordinaryOnly = ordinaryOnly
            InitializeComponent()
        End Sub

        Private Sub UserManagementUsersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvUsers IsNot Nothing Then Me.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            LoadUserTypes()
            LoadUsers()
            ClearEditorForNewUser()
            If _ordinaryOnly Then
                Text = "کاربران عادی"
            Else
                Text = "مدیریت کاربران"
            End If
        End Sub

        Private Sub LoadUserTypes()
            cmbUserType.DataSource = Nothing
            cmbUserType.Items.Clear()
            If Not _ordinaryOnly Then
                cmbUserType.Items.Add(New ComboOption("ابر مدیر", "SuperAdmin"))
                cmbUserType.Items.Add(New ComboOption("مدیر میانی", "Manager"))
            End If
            cmbUserType.Items.Add(New ComboOption("کاربر عادی", "User"))
            If cmbUserType.Items.Count > 0 Then
                cmbUserType.SelectedIndex = 0
            End If
        End Sub

        Private Sub LoadUsers()
            If _ordinaryOnly Then
                dgvUsers.DataSource = service.GetUsersByTypes("User")
            Else
                dgvUsers.DataSource = service.GetUsers()
            End If
            dgvUsers.ClearSelection()
        End Sub

        Private Sub ClearEditorForNewUser()
            _selectedUserId = Nothing
            txtUsername.Clear()
            txtPassword.Clear()
            txtFullName.Clear()
            chkActive.Checked = True
            numMaxCompanies.Value = 0
            numMaxFiscalYears.Value = 0
            If cmbUserType.Items.Count > 0 Then
                cmbUserType.SelectedIndex = 0
            End If
            dgvUsers.ClearSelection()
        End Sub

        Private Function GetSelectedUserId() As Integer?
            If dgvUsers.CurrentRow Is Nothing OrElse dgvUsers.CurrentRow.IsNewRow Then
                Return Nothing
            End If

            Dim value = dgvUsers.CurrentRow.Cells("UserID").Value
            If value Is Nothing OrElse value Is DBNull.Value Then
                Return Nothing
            End If

            Return Convert.ToInt32(value)
        End Function

        Private Sub LoadUserToEditor(userId As Integer)
            Dim table = TryCast(dgvUsers.DataSource, DataTable)
            If table Is Nothing Then
                Return
            End If

            Dim matches = table.Select("UserID=" & userId.ToString())
            If matches.Length = 0 Then
                Return
            End If

            Dim row = matches(0)
            _selectedUserId = userId
            txtUsername.Text = Convert.ToString(row("Username"))
            txtPassword.Clear()
            txtFullName.Text = Convert.ToString(row("FullName"))
            SelectUserType(Convert.ToString(row("UserType")))
            chkActive.Checked = If(row.IsNull("IsActive"), True, Convert.ToBoolean(row("IsActive")))
            numMaxCompanies.Value = If(row.Table.Columns.Contains("MaxCompaniesAllowed") AndAlso Not row.IsNull("MaxCompaniesAllowed"), Convert.ToDecimal(row("MaxCompaniesAllowed")), CDec(0))
            numMaxFiscalYears.Value = If(row.Table.Columns.Contains("MaxFiscalYearsPerCompany") AndAlso Not row.IsNull("MaxFiscalYearsPerCompany"), Convert.ToDecimal(row("MaxFiscalYearsPerCompany")), CDec(0))
        End Sub

        Private Sub SelectUserType(userType As String)
            For Each item As Object In cmbUserType.Items
                Dim optionItem = TryCast(item, ComboOption)
                If optionItem IsNot Nothing AndAlso String.Equals(optionItem.Value, userType, StringComparison.OrdinalIgnoreCase) Then
                    cmbUserType.SelectedItem = optionItem
                    Exit Sub
                End If
            Next
        End Sub

        Private Function GetSelectedUserType() As String
            Dim optionItem = TryCast(cmbUserType.SelectedItem, ComboOption)
            If optionItem IsNot Nothing Then
                Return optionItem.Value
            End If
            Return Convert.ToString(cmbUserType.Text)
        End Function

        Private Sub DgvUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUsers.CellFormatting
            PersianDateHelper.ApplyToGrid(sender, e)
        End Sub

        Private Sub DgvUsers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUsers.SelectionChanged
            Dim userId = GetSelectedUserId()
            If userId.HasValue Then
                LoadUserToEditor(userId.Value)
            End If
        End Sub

        Private Sub BtnNewUser_Click(sender As Object, e As EventArgs) Handles btnNewUser.Click
            ClearEditorForNewUser()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadUserTypes()
            LoadUsers()
            If _selectedUserId.HasValue Then
                LoadUserToEditor(_selectedUserId.Value)
            End If
        End Sub

        Private Sub BtnSaveUser_Click(sender As Object, e As EventArgs) Handles btnSaveUser.Click
            If String.IsNullOrWhiteSpace(txtUsername.Text) Then
                MessageBox.Show("نام کاربری نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim createdBy As Integer? = Nothing
            If SessionContext.CurrentUser IsNot Nothing Then
                createdBy = SessionContext.CurrentUser.UserID
            End If

            If _ordinaryOnly Then
                If Not String.Equals(GetSelectedUserType(), "User", StringComparison.OrdinalIgnoreCase) Then
                    MessageBox.Show("در این بخش فقط امکان ایجاد کاربر عادی وجود دارد.")
                    Return
                End If
            End If

            Try
                Dim userId = service.SaveUser(
                    _selectedUserId,
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    GetSelectedUserType(),
                    txtFullName.Text.Trim(),
                    chkActive.Checked,
                    createdBy,
                    SessionContext.CurrentIP,
                    Convert.ToInt32(numMaxCompanies.Value),
                    Convert.ToInt32(numMaxFiscalYears.Value))

                _selectedUserId = userId
                LoadUsers()
                LoadUserToEditor(userId)
                MessageBox.Show("کاربر با موفقیت ذخیره شد.")
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره کاربر: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnDeleteUser_Click(sender As Object, e As EventArgs) Handles btnDeleteUser.Click
            If Not _selectedUserId.HasValue Then
                MessageBox.Show("ابتدا یک کاربر را انتخاب کنید.")
                Return
            End If

            If MessageBox.Show("کاربر انتخاب شده حذف شود؟", "تایید", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            service.DeleteUser(_selectedUserId.Value)
            ClearEditorForNewUser()
            LoadUsers()
            MessageBox.Show("کاربر حذف شد.")
        End Sub
    End Class
End Namespace
