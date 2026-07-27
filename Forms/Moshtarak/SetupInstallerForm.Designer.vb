Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class SetupInstallerForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblDesc As Label
        Friend WithEvents lblPath As Label
        Friend WithEvents txtInstallPath As TextBox
        Friend WithEvents btnBrowse As Button
        Friend WithEvents chkCreateShortcut As CheckBox
        Friend WithEvents btnInstall As Button
        Friend WithEvents btnCancel As Button
        Friend WithEvents progressBar As ProgressBar
        Friend WithEvents lblStatus As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTitle = New Label()
            Me.lblDesc = New Label()
            Me.lblPath = New Label()
            Me.txtInstallPath = New TextBox()
            Me.btnBrowse = New Button()
            Me.chkCreateShortcut = New CheckBox()
            Me.btnInstall = New Button()
            Me.btnCancel = New Button()
            Me.progressBar = New ProgressBar()
            Me.lblStatus = New Label()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(540, 320)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Name = "SetupInstallerForm"
            Me.Text = "نصب‌کننده سیستم حسابداری و انبارداری"

            ' lblTitle
            Me.lblTitle.Font = New Font("Tahoma", 11.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.FromArgb(41, 128, 185)
            Me.lblTitle.Location = New Point(20, 20)
            Me.lblTitle.Size = New Size(500, 30)
            Me.lblTitle.Text = "خوش آمدید - دستیار نصب سیستم حسابداری و انبارداری"

            ' lblDesc
            Me.lblDesc.ForeColor = Color.FromArgb(70, 80, 95)
            Me.lblDesc.Location = New Point(20, 55)
            Me.lblDesc.Size = New Size(500, 40)
            Me.lblDesc.Text = "این برنامه، سیستم را به همراه دیتابیس خام و آماده ورود اطلاعات در کامپیوتر شما نصب می‌نماید. لطفاً مسیر نصب مورد نظر خود را انتخاب کنید:"

            ' lblPath
            Me.lblPath.AutoSize = True
            Me.lblPath.Location = New Point(440, 110)
            Me.lblPath.Text = "مسیر نصب:"

            ' txtInstallPath
            Me.txtInstallPath.Location = New Point(110, 107)
            Me.txtInstallPath.Size = New Size(320, 22)
            Me.txtInstallPath.Text = "C:\Negar"

            ' btnBrowse
            Me.btnBrowse.Location = New Point(20, 105)
            Me.btnBrowse.Size = New Size(80, 26)
            Me.btnBrowse.Text = "انتخاب..."
            Me.btnBrowse.UseVisualStyleBackColor = True

            ' chkCreateShortcut
            Me.chkCreateShortcut.AutoSize = True
            Me.chkCreateShortcut.Checked = True
            Me.chkCreateShortcut.CheckState = CheckState.Checked
            Me.chkCreateShortcut.Location = New Point(280, 145)
            Me.chkCreateShortcut.Text = "ایجاد میانبر (Shortcut) روی دسکتاپ"

            ' progressBar
            Me.progressBar.Location = New Point(20, 180)
            Me.progressBar.Size = New Size(500, 22)
            Me.progressBar.Visible = False

            ' lblStatus
            Me.lblStatus.AutoSize = True
            Me.lblStatus.ForeColor = Color.DarkGreen
            Me.lblStatus.Location = New Point(20, 210)
            Me.lblStatus.Text = ""

            ' btnInstall
            Me.btnInstall.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.btnInstall.Location = New Point(140, 260)
            Me.btnInstall.Size = New Size(120, 34)
            Me.btnInstall.Text = "شروع نصب"
            Me.btnInstall.UseVisualStyleBackColor = True

            ' btnCancel
            Me.btnCancel.Location = New Point(20, 260)
            Me.btnCancel.Size = New Size(100, 34)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True

            ' Add controls
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblDesc)
            Me.Controls.Add(Me.lblPath)
            Me.Controls.Add(Me.txtInstallPath)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.chkCreateShortcut)
            Me.Controls.Add(Me.progressBar)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.btnInstall)
            Me.Controls.Add(Me.btnCancel)

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
