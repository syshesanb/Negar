Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class ChangeProfileForm
        Inherits Form

        Private components As IContainer
        Private lblFullName As Label
        Private txtFullName As TextBox
        Private lblUsername As Label
        Private txtUsername As TextBox
        Private lblCurrentPassword As Label
        Private txtCurrentPassword As TextBox
        Private lblNewPassword As Label
        Private txtNewPassword As TextBox
        Private lblConfirmPassword As Label
        Private txtConfirmPassword As TextBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblFullName = New Label()
            Me.txtFullName = New TextBox()
            Me.lblUsername = New Label()
            Me.txtUsername = New TextBox()
            Me.lblCurrentPassword = New Label()
            Me.txtCurrentPassword = New TextBox()
            Me.lblNewPassword = New Label()
            Me.txtNewPassword = New TextBox()
            Me.lblConfirmPassword = New Label()
            Me.txtConfirmPassword = New TextBox()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(420, 290)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "ChangeProfileForm"
            Me.Text = "تغییر نام کاربری و رمز عبور"

            ' lblFullName
            Me.lblFullName.AutoSize = True
            Me.lblFullName.Location = New Point(280, 25)
            Me.lblFullName.Text = "نام و نام خانوادگی:"

            ' txtFullName
            Me.txtFullName.Location = New Point(30, 22)
            Me.txtFullName.Size = New Size(240, 22)

            ' lblUsername
            Me.lblUsername.AutoSize = True
            Me.lblUsername.Location = New Point(280, 65)
            Me.lblUsername.Text = "نام کاربری:"

            ' txtUsername
            Me.txtUsername.Location = New Point(30, 62)
            Me.txtUsername.Size = New Size(240, 22)

            ' lblCurrentPassword
            Me.lblCurrentPassword.AutoSize = True
            Me.lblCurrentPassword.Location = New Point(280, 105)
            Me.lblCurrentPassword.Text = "رمز عبور فعلی:"

            ' txtCurrentPassword
            Me.txtCurrentPassword.Location = New Point(30, 102)
            Me.txtCurrentPassword.Size = New Size(240, 22)
            Me.txtCurrentPassword.UseSystemPasswordChar = True

            ' lblNewPassword
            Me.lblNewPassword.AutoSize = True
            Me.lblNewPassword.Location = New Point(280, 145)
            Me.lblNewPassword.Text = "رمز عبور جدید (اختیاری):"

            ' txtNewPassword
            Me.txtNewPassword.Location = New Point(30, 142)
            Me.txtNewPassword.Size = New Size(240, 22)
            Me.txtNewPassword.UseSystemPasswordChar = True

            ' lblConfirmPassword
            Me.lblConfirmPassword.AutoSize = True
            Me.lblConfirmPassword.Location = New Point(280, 185)
            Me.lblConfirmPassword.Text = "تکرار رمز عبور جدید:"

            ' txtConfirmPassword
            Me.txtConfirmPassword.Location = New Point(30, 182)
            Me.txtConfirmPassword.Size = New Size(240, 22)
            Me.txtConfirmPassword.UseSystemPasswordChar = True

            ' btnSave
            Me.btnSave.Location = New Point(160, 235)
            Me.btnSave.Size = New Size(110, 32)
            Me.btnSave.Text = "ثبت تغییرات"
            Me.btnSave.UseVisualStyleBackColor = True

            ' btnCancel
            Me.btnCancel.Location = New Point(30, 235)
            Me.btnCancel.Size = New Size(110, 32)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True

            ' Add controls
            Me.Controls.Add(Me.lblFullName)
            Me.Controls.Add(Me.txtFullName)
            Me.Controls.Add(Me.lblUsername)
            Me.Controls.Add(Me.txtUsername)
            Me.Controls.Add(Me.lblCurrentPassword)
            Me.Controls.Add(Me.txtCurrentPassword)
            Me.Controls.Add(Me.lblNewPassword)
            Me.Controls.Add(Me.txtNewPassword)
            Me.Controls.Add(Me.lblConfirmPassword)
            Me.Controls.Add(Me.txtConfirmPassword)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.btnCancel)

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
