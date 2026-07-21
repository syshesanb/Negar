Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryVahedKala2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents lblCategory As Label
        Friend WithEvents cmbCategory As ComboBox
        Friend WithEvents btnAddCategory As Button
        Friend WithEvents lblName As Label
        Friend WithEvents txtName As TextBox
        Friend WithEvents lblAbbreviation As Label
        Friend WithEvents txtAbbreviation As TextBox
        Friend WithEvents chkIsReferenceUoM As CheckBox
        Friend WithEvents gbConversion As GroupBox
        Friend WithEvents lblNumerator As Label
        Friend WithEvents txtNumerator As TextBox
        Friend WithEvents lblDenominator As Label
        Friend WithEvents txtDenominator As TextBox
        Friend WithEvents lblExplanation As Label
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblCategory = New Label()
            Me.cmbCategory = New ComboBox()
            Me.btnAddCategory = New Button()
            Me.lblName = New Label()
            Me.txtName = New TextBox()
            Me.lblAbbreviation = New Label()
            Me.txtAbbreviation = New TextBox()
            Me.chkIsReferenceUoM = New CheckBox()
            Me.gbConversion = New GroupBox()
            Me.lblNumerator = New Label()
            Me.txtNumerator = New TextBox()
            Me.lblDenominator = New Label()
            Me.txtDenominator = New TextBox()
            Me.lblExplanation = New Label()
            Me.chkActive = New CheckBox()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.gbConversion.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblCategory
            '
            Me.lblCategory.Location = New Point(320, 25)
            Me.lblCategory.Name = "lblCategory"
            Me.lblCategory.Size = New Size(100, 20)
            Me.lblCategory.Text = "دسته‌بندی واحد: *"
            Me.lblCategory.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbCategory
            '
            Me.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbCategory.Location = New Point(120, 22)
            Me.cmbCategory.Name = "cmbCategory"
            Me.cmbCategory.Size = New Size(190, 22)
            Me.cmbCategory.TabIndex = 0
            '
            'btnAddCategory
            '
            Me.btnAddCategory.Location = New Point(80, 22)
            Me.btnAddCategory.Name = "btnAddCategory"
            Me.btnAddCategory.Size = New Size(34, 22)
            Me.btnAddCategory.Text = "+"
            Me.btnAddCategory.TabIndex = 1
            Me.btnAddCategory.UseVisualStyleBackColor = True
            '
            'lblName
            '
            Me.lblName.Location = New Point(320, 65)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New Size(100, 20)
            Me.lblName.Text = "نام واحد: *"
            Me.lblName.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtName
            '
            Me.txtName.Location = New Point(80, 62)
            Me.txtName.MaxLength = 50
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New Size(230, 22)
            Me.txtName.TabIndex = 2
            '
            'lblAbbreviation
            '
            Me.lblAbbreviation.Location = New Point(320, 105)
            Me.lblAbbreviation.Name = "lblAbbreviation"
            Me.lblAbbreviation.Size = New Size(100, 20)
            Me.lblAbbreviation.Text = "علامت اختصاری:"
            Me.lblAbbreviation.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtAbbreviation
            '
            Me.txtAbbreviation.Location = New Point(80, 102)
            Me.txtAbbreviation.MaxLength = 10
            Me.txtAbbreviation.Name = "txtAbbreviation"
            Me.txtAbbreviation.Size = New Size(230, 22)
            Me.txtAbbreviation.TabIndex = 3
            '
            'chkIsReferenceUoM
            '
            Me.chkIsReferenceUoM.Location = New Point(80, 140)
            Me.chkIsReferenceUoM.Name = "chkIsReferenceUoM"
            Me.chkIsReferenceUoM.RightToLeft = RightToLeft.Yes
            Me.chkIsReferenceUoM.Size = New Size(230, 24)
            Me.chkIsReferenceUoM.TabIndex = 4
            Me.chkIsReferenceUoM.Text = "این واحد، واحد مرجع (مبنا) گروه است"
            Me.chkIsReferenceUoM.UseVisualStyleBackColor = True
            '
            'gbConversion
            '
            Me.gbConversion.Controls.Add(Me.lblNumerator)
            Me.gbConversion.Controls.Add(Me.txtNumerator)
            Me.gbConversion.Controls.Add(Me.lblDenominator)
            Me.gbConversion.Controls.Add(Me.txtDenominator)
            Me.gbConversion.Controls.Add(Me.lblExplanation)
            Me.gbConversion.Location = New Point(20, 180)
            Me.gbConversion.Name = "gbConversion"
            Me.gbConversion.RightToLeft = RightToLeft.Yes
            Me.gbConversion.Size = New Size(400, 140)
            Me.gbConversion.TabIndex = 5
            Me.gbConversion.TabStop = False
            Me.gbConversion.Text = "فرمول تبدیل به واحد مرجع"
            '
            'lblNumerator
            '
            Me.lblNumerator.Location = New Point(280, 32)
            Me.lblNumerator.Name = "lblNumerator"
            Me.lblNumerator.Size = New Size(100, 20)
            Me.lblNumerator.Text = "صورت کسر (ضریب):"
            Me.lblNumerator.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtNumerator
            '
            Me.txtNumerator.Location = New Point(180, 30)
            Me.txtNumerator.MaxLength = 9
            Me.txtNumerator.Name = "txtNumerator"
            Me.txtNumerator.Size = New Size(90, 22)
            Me.txtNumerator.TabIndex = 0
            Me.txtNumerator.Text = "1"
            Me.txtNumerator.TextAlign = HorizontalAlignment.Center
            '
            'lblDenominator
            '
            Me.lblDenominator.Location = New Point(100, 32)
            Me.lblDenominator.Name = "lblDenominator"
            Me.lblDenominator.Size = New Size(70, 20)
            Me.lblDenominator.Text = "مخرج کسر:"
            Me.lblDenominator.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtDenominator
            '
            Me.txtDenominator.Location = New Point(10, 30)
            Me.txtDenominator.MaxLength = 9
            Me.txtDenominator.Name = "txtDenominator"
            Me.txtDenominator.Size = New Size(80, 22)
            Me.txtDenominator.TabIndex = 1
            Me.txtDenominator.Text = "1"
            Me.txtDenominator.TextAlign = HorizontalAlignment.Center
            '
            'lblExplanation
            '
            Me.lblExplanation.ForeColor = Color.DarkBlue
            Me.lblExplanation.Location = New Point(10, 75)
            Me.lblExplanation.Name = "lblExplanation"
            Me.lblExplanation.Size = New Size(370, 50)
            Me.lblExplanation.Text = "فرمول تبدیل..."
            Me.lblExplanation.TextAlign = ContentAlignment.MiddleCenter
            '
            'chkActive
            '
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = CheckState.Checked
            Me.chkActive.Location = New Point(80, 335)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.RightToLeft = RightToLeft.Yes
            Me.chkActive.Size = New Size(230, 24)
            Me.chkActive.TabIndex = 6
            Me.chkActive.Text = "فعال"
            Me.chkActive.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Location = New Point(125, 380)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(90, 30)
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.TabIndex = 7
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New Point(20, 380)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(90, 30)
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.TabIndex = 8
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'AnbardaryVahedKala2Form
            '
            Me.ClientSize = New Size(440, 430)
            Me.Controls.Add(Me.lblCategory)
            Me.Controls.Add(Me.cmbCategory)
            Me.Controls.Add(Me.btnAddCategory)
            Me.Controls.Add(Me.lblName)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.lblAbbreviation)
            Me.Controls.Add(Me.txtAbbreviation)
            Me.Controls.Add(Me.chkIsReferenceUoM)
            Me.Controls.Add(Me.gbConversion)
            Me.Controls.Add(Me.chkActive)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.btnCancel)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش واحد اندازه‌گیری"
            Me.gbConversion.ResumeLayout(False)
            Me.gbConversion.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
