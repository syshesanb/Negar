Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class SelectReleaseUserForm
        Public Property SelectedManagerID As Integer
        Public Property ManagerPassword As String

        Private ReadOnly userService As New UserService()

        Private Sub SelectReleaseUserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadManagers()
        End Sub

        Private Sub LoadManagers()
            Dim managersTable As DataTable = userService.GetUsersByTypes("Manager")
            If managersTable Is Nothing OrElse managersTable.Rows.Count = 0 Then
                MessageBox.Show("هیچ کاربر میانی در سیستم یافت نشد." & Environment.NewLine & "لطفاً ابتدا از بخش مدیریت کاربران یک کاربر میانی با دسترسی‌ها و سقف شرکت/سال مالی دلخواه تعریف کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
                Return
            End If

            cmbManagers.DisplayMember = "FullName"
            cmbManagers.ValueMember = "UserID"
            cmbManagers.DataSource = managersTable
            If cmbManagers.Items.Count > 0 Then
                cmbManagers.SelectedIndex = 0
            End If
        End Sub

        Private Sub CmbManagers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbManagers.SelectedIndexChanged
            If cmbManagers.SelectedItem IsNot Nothing AndAlso TypeOf cmbManagers.SelectedItem Is DataRowView Then
                Dim drv = CType(cmbManagers.SelectedItem, DataRowView)
                Dim username = Convert.ToString(drv("Username"))
                Dim maxComp = If(drv.Row.Table.Columns.Contains("MaxCompaniesAllowed") AndAlso Not drv.Row.IsNull("MaxCompaniesAllowed"), Convert.ToInt32(drv("MaxCompaniesAllowed")), 0)
                Dim maxFY = If(drv.Row.Table.Columns.Contains("MaxFiscalYearsPerCompany") AndAlso Not drv.Row.IsNull("MaxFiscalYearsPerCompany"), Convert.ToInt32(drv("MaxFiscalYearsPerCompany")), 0)

                Dim strComp = If(maxComp = 0, "نامحدود", maxComp.ToString() & " شرکت")
                Dim strFY = If(maxFY = 0, "نامحدود", maxFY.ToString() & " سال")

                lblManagerInfo.Text = "نام کاربری: " & username & "  |  سقف شرکت: " & strComp & "  |  سقف سال مالی: " & strFY
            End If
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
            If cmbManagers.SelectedValue Is Nothing Then
                MessageBox.Show("لطفاً یک کاربر میانی انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If String.IsNullOrWhiteSpace(txtPassword.Text) Then
                MessageBox.Show("لطفاً رمز عبور اولیه برای نسخه نصبی را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Focus()
                Return
            End If

            SelectedManagerID = Convert.ToInt32(cmbManagers.SelectedValue)
            ManagerPassword = txtPassword.Text.Trim()

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
