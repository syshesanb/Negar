Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AppLockForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents pnlCenter As Panel
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblUserInfo As Label
        Friend WithEvents lblPasswordPrompt As Label
        Friend WithEvents txtPassword As TextBox
        Friend WithEvents btnUnlock As Button
        Friend WithEvents btnSwitchUser As Button
        Friend WithEvents lblError As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlCenter = New Panel()
            Me.lblTitle = New Label()
            Me.lblUserInfo = New Label()
            Me.lblPasswordPrompt = New Label()
            Me.txtPassword = New TextBox()
            Me.btnUnlock = New Button()
            Me.btnSwitchUser = New Button()
            Me.lblError = New Label()

            Me.pnlCenter.SuspendLayout()
            Me.SuspendLayout()
            '
            'AppLockForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.BackColor = Color.FromArgb(30, 35, 45)
            Me.ClientSize = New Size(800, 600)
            Me.Font = New Font("Tahoma", 9.5!)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AppLockForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "قفل موقت برنامه"
            Me.TopMost = True
            Me.WindowState = FormWindowState.Maximized
            '
            'pnlCenter
            '
            Me.pnlCenter.Anchor = AnchorStyles.None
            Me.pnlCenter.BackColor = Color.FromArgb(42, 48, 60)
            Me.pnlCenter.BorderStyle = BorderStyle.FixedSingle
            Me.pnlCenter.Controls.Add(Me.lblTitle)
            Me.pnlCenter.Controls.Add(Me.lblUserInfo)
            Me.pnlCenter.Controls.Add(Me.lblPasswordPrompt)
            Me.pnlCenter.Controls.Add(Me.txtPassword)
            Me.pnlCenter.Controls.Add(Me.btnUnlock)
            Me.pnlCenter.Controls.Add(Me.lblError)
            Me.pnlCenter.Controls.Add(Me.btnSwitchUser)
            Me.pnlCenter.Location = New Point(150, 100)
            Me.pnlCenter.Name = "pnlCenter"
            Me.pnlCenter.Size = New Size(500, 360)
            Me.pnlCenter.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.Font = New Font("Tahoma", 16.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.FromArgb(52, 152, 219)
            Me.lblTitle.Location = New Point(20, 25)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(460, 40)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "🔒 برنامه قفل شده است"
            Me.lblTitle.TextAlign = ContentAlignment.MiddleCenter
            '
            'lblUserInfo
            '
            Me.lblUserInfo.Font = New Font("Tahoma", 10.5!, FontStyle.Regular)
            Me.lblUserInfo.ForeColor = Color.WhiteSmoke
            Me.lblUserInfo.Location = New Point(20, 75)
            Me.lblUserInfo.Name = "lblUserInfo"
            Me.lblUserInfo.Size = New Size(460, 30)
            Me.lblUserInfo.TabIndex = 1
            Me.lblUserInfo.Text = "کاربر جاری: -"
            Me.lblUserInfo.TextAlign = ContentAlignment.MiddleCenter
            '
            'lblPasswordPrompt
            '
            Me.lblPasswordPrompt.AutoSize = True
            Me.lblPasswordPrompt.ForeColor = Color.Gainsboro
            Me.lblPasswordPrompt.Location = New Point(360, 132)
            Me.lblPasswordPrompt.Name = "lblPasswordPrompt"
            Me.lblPasswordPrompt.Size = New Size(106, 16)
            Me.lblPasswordPrompt.TabIndex = 2
            Me.lblPasswordPrompt.Text = "رمز عبور بازگشایی:"
            '
            'txtPassword
            '
            Me.txtPassword.Font = New Font("Tahoma", 11.0!)
            Me.txtPassword.Location = New Point(50, 128)
            Me.txtPassword.Name = "txtPassword"
            Me.txtPassword.Size = New Size(300, 25)
            Me.txtPassword.TabIndex = 3
            Me.txtPassword.UseSystemPasswordChar = True
            '
            'btnUnlock
            '
            Me.btnUnlock.BackColor = Color.FromArgb(41, 128, 185)
            Me.btnUnlock.Cursor = Cursors.Hand
            Me.btnUnlock.FlatAppearance.BorderSize = 0
            Me.btnUnlock.FlatStyle = FlatStyle.Flat
            Me.btnUnlock.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.btnUnlock.ForeColor = Color.White
            Me.btnUnlock.Location = New Point(50, 180)
            Me.btnUnlock.Name = "btnUnlock"
            Me.btnUnlock.Size = New Size(400, 40)
            Me.btnUnlock.TabIndex = 4
            Me.btnUnlock.Text = "بازگشایی قفل برنامه"
            Me.btnUnlock.UseVisualStyleBackColor = False
            '
            'lblError
            '
            Me.lblError.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.lblError.ForeColor = Color.FromArgb(231, 76, 60)
            Me.lblError.Location = New Point(20, 235)
            Me.lblError.Name = "lblError"
            Me.lblError.Size = New Size(460, 30)
            Me.lblError.TabIndex = 5
            Me.lblError.TextAlign = ContentAlignment.MiddleCenter
            '
            'btnSwitchUser
            '
            Me.btnSwitchUser.BackColor = Color.Transparent
            Me.btnSwitchUser.Cursor = Cursors.Hand
            Me.btnSwitchUser.FlatAppearance.BorderSize = 0
            Me.btnSwitchUser.FlatStyle = FlatStyle.Flat
            Me.btnSwitchUser.Font = New Font("Tahoma", 9.0!, FontStyle.Underline)
            Me.btnSwitchUser.ForeColor = Color.DarkGray
            Me.btnSwitchUser.Location = New Point(150, 285)
            Me.btnSwitchUser.Name = "btnSwitchUser"
            Me.btnSwitchUser.Size = New Size(200, 35)
            Me.btnSwitchUser.TabIndex = 6
            Me.btnSwitchUser.Text = "ورود با نام کاربری دیگر"
            Me.btnSwitchUser.UseVisualStyleBackColor = False
            '
            'Controls
            '
            Me.Controls.Add(Me.pnlCenter)
            Me.pnlCenter.ResumeLayout(False)
            Me.pnlCenter.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
