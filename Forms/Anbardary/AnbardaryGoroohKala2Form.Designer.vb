Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryGoroohKala2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents lblParent As Label
        Friend WithEvents txtParent As TextBox
        Friend WithEvents lblCode As Label
        Friend WithEvents txtGroupCode As TextBox
        Friend WithEvents lblName As Label
        Friend WithEvents txtGroupName As TextBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblParent = New Label()
            Me.txtParent = New TextBox()
            Me.lblCode = New Label()
            Me.txtGroupCode = New TextBox()
            Me.lblName = New Label()
            Me.txtGroupName = New TextBox()
            Me.chkActive = New CheckBox()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()
            '
            'lblParent
            '
            Me.lblParent.Location = New Point(320, 25)
            Me.lblParent.Name = "lblParent"
            Me.lblParent.Size = New Size(100, 20)
            Me.lblParent.Text = "گروه بالا دستی:"
            Me.lblParent.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtParent
            '
            Me.txtParent.BackColor = SystemColors.Control
            Me.txtParent.Location = New Point(30, 22)
            Me.txtParent.Name = "txtParent"
            Me.txtParent.ReadOnly = True
            Me.txtParent.Size = New Size(280, 22)
            Me.txtParent.TabStop = False
            '
            'lblCode
            '
            Me.lblCode.Location = New Point(320, 65)
            Me.lblCode.Name = "lblCode"
            Me.lblCode.Size = New Size(100, 20)
            Me.lblCode.Text = "کد گروه:"
            Me.lblCode.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtGroupCode
            '
            Me.txtGroupCode.BackColor = SystemColors.Control
            Me.txtGroupCode.Location = New Point(30, 62)
            Me.txtGroupCode.Name = "txtGroupCode"
            Me.txtGroupCode.ReadOnly = True
            Me.txtGroupCode.Size = New Size(280, 22)
            Me.txtGroupCode.TabStop = False
            Me.txtGroupCode.TextAlign = HorizontalAlignment.Center
            '
            'lblName
            '
            Me.lblName.Location = New Point(320, 105)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New Size(100, 20)
            Me.lblName.Text = "نام گروه کالا:"
            Me.lblName.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtGroupName
            '
            Me.txtGroupName.Location = New Point(30, 102)
            Me.txtGroupName.Name = "txtGroupName"
            Me.txtGroupName.Size = New Size(280, 22)
            '
            'chkActive
            '
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = CheckState.Checked
            Me.chkActive.Location = New Point(210, 142)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New Size(100, 20)
            Me.chkActive.Text = "فعال"
            Me.chkActive.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(40, 167, 69)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(135, 185)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(100, 30)
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = Color.FromArgb(108, 117, 125)
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(30, 185)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(100, 30)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'AnbardaryGoroohKala2Form
            '
            Me.ClientSize = New Size(450, 240)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.chkActive)
            Me.Controls.Add(Me.txtGroupName)
            Me.Controls.Add(Me.lblName)
            Me.Controls.Add(Me.txtGroupCode)
            Me.Controls.Add(Me.lblCode)
            Me.Controls.Add(Me.txtParent)
            Me.Controls.Add(Me.lblParent)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbardaryGoroohKala2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "تعریف گروه کالا"
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
