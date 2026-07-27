Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class BackupRestoreForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblDesc As Label
        Friend WithEvents lblPath As Label
        Friend WithEvents txtPath As TextBox
        Friend WithEvents btnBrowse As Button
        Friend WithEvents progressBar As ProgressBar
        Friend WithEvents lblStatus As Label
        Friend WithEvents btnExecute As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTitle = New Label()
            Me.lblDesc = New Label()
            Me.lblPath = New Label()
            Me.txtPath = New TextBox()
            Me.btnBrowse = New Button()
            Me.progressBar = New ProgressBar()
            Me.lblStatus = New Label()
            Me.btnExecute = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(520, 270)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "BackupRestoreForm"
            Me.Text = "پشتیبان‌گیری و بازیابی اطلاعات"

            ' lblTitle
            Me.lblTitle.Font = New Font("Tahoma", 11.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.FromArgb(41, 128, 185)
            Me.lblTitle.Location = New Point(20, 20)
            Me.lblTitle.Size = New Size(480, 30)
            Me.lblTitle.Text = "پشتیبان‌گیری / بازیابی اطلاعات"

            ' lblDesc
            Me.lblDesc.ForeColor = Color.FromArgb(70, 80, 95)
            Me.lblDesc.Location = New Point(20, 55)
            Me.lblDesc.Size = New Size(480, 40)
            Me.lblDesc.Text = "لطفاً مسیر مورد نظر جهت ذخیره‌سازی یا انتخاب فایل پشتیبان را تعیین کنید."

            ' lblPath
            Me.lblPath.AutoSize = True
            Me.lblPath.Location = New Point(420, 105)
            Me.lblPath.Text = "مسیر فایل:"

            ' txtPath
            Me.txtPath.Location = New Point(110, 102)
            Me.txtPath.Size = New Size(300, 22)

            ' btnBrowse
            Me.btnBrowse.Location = New Point(20, 100)
            Me.btnBrowse.Size = New Size(80, 26)
            Me.btnBrowse.Text = "انتخاب..."
            Me.btnBrowse.UseVisualStyleBackColor = True

            ' progressBar
            Me.progressBar.Location = New Point(20, 145)
            Me.progressBar.Size = New Size(480, 20)
            Me.progressBar.Visible = False

            ' lblStatus
            Me.lblStatus.AutoSize = True
            Me.lblStatus.ForeColor = Color.DarkGreen
            Me.lblStatus.Location = New Point(20, 172)
            Me.lblStatus.Text = ""

            ' btnExecute
            Me.btnExecute.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.btnExecute.Location = New Point(150, 210)
            Me.btnExecute.Size = New Size(140, 34)
            Me.btnExecute.Text = "تایید و اجرا"
            Me.btnExecute.UseVisualStyleBackColor = True

            ' btnCancel
            Me.btnCancel.Location = New Point(20, 210)
            Me.btnCancel.Size = New Size(100, 34)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True

            ' Add controls
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblDesc)
            Me.Controls.Add(Me.lblPath)
            Me.Controls.Add(Me.txtPath)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.progressBar)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.btnExecute)
            Me.Controls.Add(Me.btnCancel)

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
