Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryModyanCodes2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents lblModyanCode As Label
        Friend WithEvents txtModyanCode As TextBox
        Friend WithEvents lblDescription As Label
        Friend WithEvents txtDescription As TextBox
        Friend WithEvents lblCategoryName As Label
        Friend WithEvents txtCategoryName As TextBox
        Friend WithEvents lblTaxRate As Label
        Friend WithEvents numTaxRate As NumericUpDown
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblModyanCode = New Label()
            Me.txtModyanCode = New TextBox()
            Me.lblDescription = New Label()
            Me.txtDescription = New TextBox()
            Me.lblCategoryName = New Label()
            Me.txtCategoryName = New TextBox()
            Me.lblTaxRate = New Label()
            Me.numTaxRate = New NumericUpDown()
            Me.chkActive = New CheckBox()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            CType(Me.numTaxRate, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblModyanCode
            '
            Me.lblModyanCode.Location = New Point(310, 20)
            Me.lblModyanCode.Name = "lblModyanCode"
            Me.lblModyanCode.Size = New Size(120, 20)
            Me.lblModyanCode.Text = "کد عمومی مودیان:"
            Me.lblModyanCode.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtModyanCode
            '
            Me.txtModyanCode.Location = New Point(20, 18)
            Me.txtModyanCode.Name = "txtModyanCode"
            Me.txtModyanCode.Size = New Size(280, 22)
            Me.txtModyanCode.TabIndex = 0
            '
            'lblDescription
            '
            Me.lblDescription.Location = New Point(310, 60)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New Size(120, 20)
            Me.lblDescription.Text = "شرح کالا / خدمت:"
            Me.lblDescription.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtDescription
            '
            Me.txtDescription.Location = New Point(20, 58)
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New Size(280, 22)
            Me.txtDescription.TabIndex = 1
            '
            'lblCategoryName
            '
            Me.lblCategoryName.Location = New Point(310, 100)
            Me.lblCategoryName.Name = "lblCategoryName"
            Me.lblCategoryName.Size = New Size(120, 20)
            Me.lblCategoryName.Text = "نام دسته بندی:"
            Me.lblCategoryName.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtCategoryName
            '
            Me.txtCategoryName.Location = New Point(20, 98)
            Me.txtCategoryName.Name = "txtCategoryName"
            Me.txtCategoryName.Size = New Size(280, 22)
            Me.txtCategoryName.TabIndex = 2
            '
            'lblTaxRate
            '
            Me.lblTaxRate.Location = New Point(310, 140)
            Me.lblTaxRate.Name = "lblTaxRate"
            Me.lblTaxRate.Size = New Size(120, 20)
            Me.lblTaxRate.Text = "نرخ مالیات (عوارض) %:"
            Me.lblTaxRate.TextAlign = ContentAlignment.MiddleLeft
            '
            'numTaxRate
            '
            Me.numTaxRate.DecimalPlaces = 2
            Me.numTaxRate.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.numTaxRate.Location = New Point(180, 138)
            Me.numTaxRate.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numTaxRate.Name = "numTaxRate"
            Me.numTaxRate.Size = New Size(120, 22)
            Me.numTaxRate.TabIndex = 3
            Me.numTaxRate.TextAlign = HorizontalAlignment.Center
            '
            'chkActive
            '
            Me.chkActive.AutoSize = True
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = CheckState.Checked
            Me.chkActive.Location = New Point(210, 180)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New Size(90, 18)
            Me.chkActive.TabIndex = 4
            Me.chkActive.Text = "کد فعال است"
            Me.chkActive.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(30, 120, 60)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(150, 220)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(110, 30)
            Me.btnSave.TabIndex = 5
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = Color.FromArgb(120, 120, 120)
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(20, 220)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(110, 30)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'AnbardaryModyanCodes2Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(450, 270)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.chkActive)
            Me.Controls.Add(Me.numTaxRate)
            Me.Controls.Add(Me.lblTaxRate)
            Me.Controls.Add(Me.txtCategoryName)
            Me.Controls.Add(Me.lblCategoryName)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.txtModyanCode)
            Me.Controls.Add(Me.lblModyanCode)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbardaryModyanCodes2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش کد مودیان"
            CType(Me.numTaxRate, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
