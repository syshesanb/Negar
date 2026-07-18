Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Models

Namespace Sys_Hes_Anb.Forms
    Public Class AppLockForm
        Private ReadOnly _currentUser As UserAccount
        Public Property SwitchUserRequested As Boolean = False

        Public Sub New(currentUser As UserAccount)
            _currentUser = currentUser
            InitializeComponent()
            Me.AcceptButton = btnUnlock
        End Sub

        Private Sub AppLockForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            CenterPanel()
            If _currentUser IsNot Nothing Then
                lblUserInfo.Text = "کاربر جاری: " & _currentUser.FullName & " (" & _currentUser.Username & ")"
            Else
                lblUserInfo.Text = "کاربر جاری: نامشخص"
            End If
            lblError.Text = ""
        End Sub

        Private Sub AppLockForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            txtPassword.Focus()
        End Sub

        Private Sub AppLockForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
            CenterPanel()
        End Sub

        Private Sub CenterPanel()
            If pnlCenter IsNot Nothing Then
                pnlCenter.Location = New Point((Me.ClientSize.Width - pnlCenter.Width) \ 2, (Me.ClientSize.Height - pnlCenter.Height) \ 2)
            End If
        End Sub

        Private Sub BtnUnlock_Click(sender As Object, e As EventArgs) Handles btnUnlock.Click
            lblError.Text = ""
            Dim entered = txtPassword.Text
            If String.IsNullOrEmpty(entered) Then
                lblError.Text = "لطفاً رمز عبور را وارد کنید."
                txtPassword.Focus()
                Return
            End If

            If _currentUser IsNot Nothing Then
                Dim enteredHash = PasswordHasher.Hash(entered)
                If String.Equals(enteredHash, _currentUser.PasswordHash, StringComparison.OrdinalIgnoreCase) Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                    Return
                End If
            End If

            lblError.Text = "رمز عبور وارد شده صحیح نمی‌باشد."
            txtPassword.SelectAll()
            txtPassword.Focus()
        End Sub

        Private Sub BtnSwitchUser_Click(sender As Object, e As EventArgs) Handles btnSwitchUser.Click
            Dim confirm = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید با کاربر دیگری وارد شوید؟" & Environment.NewLine &
                "توجه: اطلاعات ذخیره‌نشده در فرم‌های باز بسته‌خواهند شد.",
                "تغییر کاربر", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If confirm = DialogResult.Yes Then
                SwitchUserRequested = True
                Me.DialogResult = DialogResult.Retry
                Me.Close()
            End If
        End Sub

        Private Sub AppLockForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            If e.CloseReason = CloseReason.UserClosing AndAlso Me.DialogResult <> DialogResult.OK AndAlso Me.DialogResult <> DialogResult.Retry Then
                e.Cancel = True
            End If
        End Sub
    End Class
End Namespace
