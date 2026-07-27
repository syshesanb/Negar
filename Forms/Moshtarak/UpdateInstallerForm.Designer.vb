Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class UpdateInstallerForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblDesc As Label
        Friend WithEvents lblPath As Label
        Friend WithEvents txtTargetDir As TextBox
        Friend WithEvents btnBrowse As Button
        Friend WithEvents progressBar As ProgressBar
        Friend WithEvents lblStatus As Label
        Friend WithEvents btnStartUpdate As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTitle = New Label()
            Me.lblDesc = New Label()
            Me.lblPath = New Label()
            Me.txtTargetDir = New TextBox()
            Me.btnBrowse = New Button()
            Me.progressBar = New ProgressBar()
            Me.lblStatus = New Label()
            Me.btnStartUpdate = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(540, 300)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Name = "UpdateInstallerForm"
            Me.Text = "دستیار به‌روزرسانی سیستم حسابداری و انبارداری"

            ' lblTitle
            Me.lblTitle.Font = New Font("Tahoma", 11.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.FromArgb(41, 128, 185)
            Me.lblTitle.Location = New Point(20, 20)
            Me.lblTitle.Size = New Size(500, 30)
            Me.lblTitle.Text = "دستیار ارتقا و به‌روزرسانی سیستم"

            ' lblDesc
            Me.lblDesc.ForeColor = Color.FromArgb(70, 80, 95)
            Me.lblDesc.Location = New Point(20, 55)
            Me.lblDesc.Size = New Size(500, 45)
            Me.lblDesc.Text = "این ابزار برنامه شما را به نسخه جدید ارتقا می‌دهد. قبل از اعمال تغییرات، یک نسخه پشتیبان (بک‌آپ) از دیتابیس فعلی شما گرفته شده و تمام اطلاعات قبلی شما ۱۰۰٪ محفوظ می‌ماند."

            ' lblPath
            Me.lblPath.AutoSize = True
            Me.lblPath.Location = New Point(420, 110)
            Me.lblPath.Text = "مسیر نصب فعلی:"

            ' txtTargetDir
            Me.txtTargetDir.Location = New Point(110, 107)
            Me.txtTargetDir.Size = New Size(300, 22)
            Me.txtTargetDir.Text = "C:\Negar"

            ' btnBrowse
            Me.btnBrowse.Location = New Point(20, 105)
            Me.btnBrowse.Size = New Size(80, 26)
            Me.btnBrowse.Text = "انتخاب..."
            Me.btnBrowse.UseVisualStyleBackColor = True

            ' progressBar
            Me.progressBar.Location = New Point(20, 160)
            Me.progressBar.Size = New Size(500, 22)
            Me.progressBar.Visible = False

            ' lblStatus
            Me.lblStatus.AutoSize = True
            Me.lblStatus.ForeColor = Color.DarkGreen
            Me.lblStatus.Location = New Point(20, 190)
            Me.lblStatus.Text = ""

            ' btnStartUpdate
            Me.btnStartUpdate.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.btnStartUpdate.Location = New Point(140, 240)
            Me.btnStartUpdate.Size = New Size(140, 34)
            Me.btnStartUpdate.Text = "شروع به‌روزرسانی"
            Me.btnStartUpdate.UseVisualStyleBackColor = True

            ' btnCancel
            Me.btnCancel.Location = New Point(20, 240)
            Me.btnCancel.Size = New Size(100, 34)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True

            ' Add controls
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblDesc)
            Me.Controls.Add(Me.lblPath)
            Me.Controls.Add(Me.txtTargetDir)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.progressBar)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.btnStartUpdate)
            Me.Controls.Add(Me.btnCancel)

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
