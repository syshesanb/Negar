Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Models

Namespace Negar.Forms
    Partial Class LoginForm
        Inherits Form

        Private ReadOnly securityService As New SecurityService()

        Public Property AuthenticatedUser As UserAccount

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            AppIconHelper.ApplyAppIcon(Me)
        End Sub

        Protected Overrides Sub OnShown(e As EventArgs)
            MyBase.OnShown(e)
            txtUsername.Focus()
        End Sub

        Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
            Try
                lblStatus.Text = ""
                Dim user = securityService.Authenticate(txtUsername.Text.Trim(), txtPassword.Text)
                If user Is Nothing Then
                    lblStatus.Text = "نام کاربری یا رمز عبور صحیح نیست."
                    Return
                End If

                securityService.SignIn(user)
                AuthenticatedUser = user
                DialogResult = DialogResult.OK
                Close()
            Catch ex As Exception
                lblStatus.Text = ex.Message
            End Try
        End Sub
    End Class
End Namespace
