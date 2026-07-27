Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class ChangeProfileForm
        Private Sub ChangeProfileForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            If SessionContext.CurrentUser IsNot Nothing Then
                txtFullName.Text = SessionContext.CurrentUser.FullName
                txtUsername.Text = SessionContext.CurrentUser.Username
            End If
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If SessionContext.CurrentUser Is Nothing Then
                MessageBox.Show("کاربری به عنوان کاربر جاری تعریف نشده است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim newFullName = txtFullName.Text.Trim()
            Dim newUsername = txtUsername.Text.Trim()
            Dim currentPassword = txtCurrentPassword.Text
            Dim newPassword = txtNewPassword.Text
            Dim confirmPassword = txtConfirmPassword.Text

            If String.IsNullOrWhiteSpace(newUsername) Then
                MessageBox.Show("نام کاربری نمی‌تواند خالی باشد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUsername.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(currentPassword) Then
                MessageBox.Show("لطفاً جهت تایید تغییرات، رمز عبور فعلی خود را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCurrentPassword.Focus()
                Return
            End If

            ' بررسی صحت رمز عبور فعلی
            Dim hashedCurrent = PasswordHasher.Hash(currentPassword)
            If Not String.Equals(hashedCurrent, SessionContext.CurrentUser.PasswordHash, StringComparison.Ordinal) Then
                MessageBox.Show("رمز عبور فعلی وارد شده صحیح نمی‌باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCurrentPassword.SelectAll()
                txtCurrentPassword.Focus()
                Return
            End If

            ' بررسی تکراری نبودن نام کاربری جدید
            If Not String.Equals(newUsername, SessionContext.CurrentUser.Username, StringComparison.OrdinalIgnoreCase) Then
                Dim duplicateCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE Username = ? AND UserID <> ?", newUsername, SessionContext.CurrentUser.UserID), 0))
                If duplicateCount > 0 Then
                    MessageBox.Show("نام کاربری انتخاب شده تکراری است. لطفاً نام کاربری دیگری انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtUsername.SelectAll()
                    txtUsername.Focus()
                    Return
                End If
            End If

            ' بررسی رمز عبور جدید در صورت ورود
            Dim updatePassword As Boolean = False
            Dim hashedNew As String = String.Empty
            If Not String.IsNullOrEmpty(newPassword) Then
                If Not String.Equals(newPassword, confirmPassword, StringComparison.Ordinal) Then
                    MessageBox.Show("رمز عبور جدید و تکرار آن با یکدیگر مطابقت ندارند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtConfirmPassword.SelectAll()
                    txtConfirmPassword.Focus()
                    Return
                End If
                hashedNew = PasswordHasher.Hash(newPassword)
                updatePassword = True
            End If

            Try
                If updatePassword Then
                    Sql.ExecuteNonQuery("UPDATE Users SET Username = ?, FullName = ?, [Password] = ? WHERE UserID = ?", newUsername, newFullName, hashedNew, SessionContext.CurrentUser.UserID)
                    SessionContext.CurrentUser.PasswordHash = hashedNew
                Else
                    Sql.ExecuteNonQuery("UPDATE Users SET Username = ?, FullName = ? WHERE UserID = ?", newUsername, newFullName, SessionContext.CurrentUser.UserID)
                End If

                SessionContext.CurrentUser.Username = newUsername
                SessionContext.CurrentUser.FullName = newFullName

                MessageBox.Show("اطلاعات حساب کاربری شما با موفقیت به روزرسانی شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در به روزرسانی پروفایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
