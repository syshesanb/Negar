Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class SelectReleaseUserForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblSelectManager As Label
        Friend WithEvents cmbManagers As ComboBox
        Friend WithEvents lblManagerInfo As Label
        Friend WithEvents lblPassword As Label
        Friend WithEvents txtPassword As TextBox
        Friend WithEvents btnGenerate As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTitle = New Label()
            Me.lblSelectManager = New Label()
            Me.cmbManagers = New ComboBox()
            Me.lblManagerInfo = New Label()
            Me.lblPassword = New Label()
            Me.txtPassword = New TextBox()
            Me.btnGenerate = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(480, 270)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "SelectReleaseUserForm"
            Me.Text = "تنظیم کاربر میانی برای نسخه قابل انتشار"

            ' lblTitle
            Me.lblTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.FromArgb(41, 128, 185)
            Me.lblTitle.Location = New Point(20, 15)
            Me.lblTitle.Size = New Size(440, 35)
            Me.lblTitle.Text = "لطفاً کاربر میانی مورد نظر را جهت ساخت نسخه قابل انتشار انتخاب کنید:"

            ' lblSelectManager
            Me.lblSelectManager.AutoSize = True
            Me.lblSelectManager.Location = New Point(360, 65)
            Me.lblSelectManager.Text = "انتخاب کاربر میانی:"

            ' cmbManagers
            Me.cmbManagers.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbManagers.Location = New Point(30, 62)
            Me.cmbManagers.Size = New Size(320, 22)

            ' lblManagerInfo
            Me.lblManagerInfo.ForeColor = Color.DarkSlateGray
            Me.lblManagerInfo.Location = New Point(30, 95)
            Me.lblManagerInfo.Size = New Size(420, 35)
            Me.lblManagerInfo.Text = "سقف شرکت‌های مجاز: - | سقف سال‌های مالی: -"

            ' lblPassword
            Me.lblPassword.AutoSize = True
            Me.lblPassword.Location = New Point(360, 145)
            Me.lblPassword.Text = "رمز عبور اولیه نسخه نصبی:"

            ' txtPassword
            Me.txtPassword.Location = New Point(30, 142)
            Me.txtPassword.Size = New Size(320, 22)
            Me.txtPassword.Text = "123456"

            ' btnGenerate
            Me.btnGenerate.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.btnGenerate.Location = New Point(160, 210)
            Me.btnGenerate.Size = New Size(170, 34)
            Me.btnGenerate.Text = "تولید نسخه انتشار"
            Me.btnGenerate.UseVisualStyleBackColor = True

            ' btnCancel
            Me.btnCancel.Location = New Point(30, 210)
            Me.btnCancel.Size = New Size(110, 34)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True

            ' Add controls
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblSelectManager)
            Me.Controls.Add(Me.cmbManagers)
            Me.Controls.Add(Me.lblManagerInfo)
            Me.Controls.Add(Me.lblPassword)
            Me.Controls.Add(Me.txtPassword)
            Me.Controls.Add(Me.btnGenerate)
            Me.Controls.Add(Me.btnCancel)

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
