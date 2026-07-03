Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class LoginForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents txtUsername As TextBox
        Friend WithEvents txtPassword As TextBox
        Friend WithEvents btnLogin As Button
        Friend WithEvents lblStatus As Label
        Private lblTitle As Label
        Private lblUser As Label
        Private lblPass As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.lblTitle = New Label()
            Me.lblUser = New Label()
            Me.txtUsername = New TextBox()
            Me.lblPass = New Label()
            Me.txtPassword = New TextBox()
            Me.btnLogin = New Button()
            Me.lblStatus = New Label()
            Me.SuspendLayout()
            '
            'LoginForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(420, 240)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Text = "ورود به سیستم"
            Me.AcceptButton = Me.btnLogin
            '
            'lblTitle
            '
            Me.lblTitle.Dock = DockStyle.Top
            Me.lblTitle.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblTitle.Height = 50
            Me.lblTitle.Text = "سیستم یکپارچه Sys_Hes_Anb"
            Me.lblTitle.TextAlign = ContentAlignment.MiddleCenter
            '
            'lblUser
            '
            Me.lblUser.AutoSize = True
            Me.lblUser.Location = New Point(32, 75)
            Me.lblUser.Text = "نام کاربری"
            '
            'txtUsername
            '
            Me.txtUsername.Location = New Point(140, 72)
            Me.txtUsername.Width = 220
            '
            'lblPass
            '
            Me.lblPass.AutoSize = True
            Me.lblPass.Location = New Point(32, 112)
            Me.lblPass.Text = "رمز عبور"
            '
            'txtPassword
            '
            Me.txtPassword.Location = New Point(140, 109)
            Me.txtPassword.UseSystemPasswordChar = True
            Me.txtPassword.Width = 220
            '
            'btnLogin
            '
            Me.btnLogin.Location = New Point(285, 148)
            Me.btnLogin.Text = "ورود"
            Me.btnLogin.Width = 75
            '
            'lblStatus
            '
            Me.lblStatus.AutoSize = False
            Me.lblStatus.ForeColor = Color.DarkRed
            Me.lblStatus.Location = New Point(32, 185)
            Me.lblStatus.Size = New Size(360, 28)
            '
            'Controls
            '
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblUser)
            Me.Controls.Add(Me.txtUsername)
            Me.Controls.Add(Me.lblPass)
            Me.Controls.Add(Me.txtPassword)
            Me.Controls.Add(Me.btnLogin)
            Me.Controls.Add(Me.lblStatus)
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
