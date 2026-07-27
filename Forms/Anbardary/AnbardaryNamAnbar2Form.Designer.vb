Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryNamAnbar2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabMain As TabControl
        Friend WithEvents tabBasic As TabPage
        Friend WithEvents tabContact As TabPage
        Friend WithEvents tabPhysical As TabPage
        Friend WithEvents tabManagement As TabPage
        Friend WithEvents tabRules As TabPage

        Friend WithEvents lblName As Label
        Friend WithEvents txtName As TextBox
        Friend WithEvents lblType As Label
        Friend WithEvents cmbType As ComboBox
        Friend WithEvents btnManageTypes As Button
        Friend WithEvents lblDesc As Label
        Friend WithEvents txtDescription As TextBox

        Friend WithEvents lblLocation As Label
        Friend WithEvents txtLocation As TextBox
        Friend WithEvents lblPhone As Label
        Friend WithEvents txtPhone As TextBox
        Friend WithEvents lblPhone2 As Label
        Friend WithEvents txtPhone2 As TextBox
        Friend WithEvents lblPhone3 As Label
        Friend WithEvents txtPhone3 As TextBox
        Friend WithEvents lblPostal As Label
        Friend WithEvents txtPostalCode As TextBox

        Friend WithEvents lblCapacity As Label
        Friend WithEvents numCapacity As NumericUpDown
        Friend WithEvents tvLayout As TreeView
        Friend WithEvents ctxLayout As ContextMenuStrip
        
        Friend WithEvents lblKeeper As Label
        Friend WithEvents txtKeeper As TextBox
        Friend WithEvents btnSelectKeeper As Button
        Friend WithEvents lblCostCenter As Label
        Friend WithEvents txtCostCenter As TextBox

        Friend WithEvents chkActive As CheckBox
        Friend WithEvents chkAllowNegative As CheckBox

        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button
        Friend WithEvents pnlBottom As Panel

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabMain = New TabControl()
            Me.tabBasic = New TabPage()
            Me.tabContact = New TabPage()
            Me.tabPhysical = New TabPage()
            Me.tabManagement = New TabPage()
            Me.tabRules = New TabPage()

            Me.lblName = New Label()
            Me.txtName = New TextBox()
            Me.lblType = New Label()
            Me.cmbType = New ComboBox()
            Me.btnManageTypes = New Button()
            Me.lblDesc = New Label()
            Me.txtDescription = New TextBox()

            Me.lblLocation = New Label()
            Me.txtLocation = New TextBox()
            Me.lblPhone = New Label()
            Me.txtPhone = New TextBox()
            Me.lblPhone2 = New Label()
            Me.txtPhone2 = New TextBox()
            Me.lblPhone3 = New Label()
            Me.txtPhone3 = New TextBox()
            Me.lblPostal = New Label()
            Me.txtPostalCode = New TextBox()

            Me.lblCapacity = New Label()
            Me.numCapacity = New NumericUpDown()
            Me.tvLayout = New TreeView()
            Me.ctxLayout = New ContextMenuStrip()

            Me.lblKeeper = New Label()
            Me.txtKeeper = New TextBox()
            Me.btnSelectKeeper = New Button()
            Me.lblCostCenter = New Label()
            Me.txtCostCenter = New TextBox()

            Me.chkActive = New CheckBox()
            Me.chkAllowNegative = New CheckBox()

            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.pnlBottom = New Panel()

            Me.tabMain.SuspendLayout()
            Me.tabBasic.SuspendLayout()
            Me.tabContact.SuspendLayout()
            Me.tabPhysical.SuspendLayout()
            Me.tabManagement.SuspendLayout()
            Me.tabRules.SuspendLayout()
            CType(Me.numCapacity, ISupportInitialize).BeginInit()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()

            '
            'tabMain
            '
            Me.tabMain.Controls.Add(Me.tabBasic)
            Me.tabMain.Controls.Add(Me.tabContact)
            Me.tabMain.Controls.Add(Me.tabPhysical)
            Me.tabMain.Controls.Add(Me.tabManagement)
            Me.tabMain.Controls.Add(Me.tabRules)
            Me.tabMain.Dock = DockStyle.Fill
            Me.tabMain.Location = New Point(0, 0)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.RightToLeftLayout = True
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New Size(850, 320)
            Me.tabMain.TabIndex = 0

            '
            'tabBasic
            '
            Me.tabBasic.Controls.Add(Me.lblName)
            Me.tabBasic.Controls.Add(Me.txtName)
            Me.tabBasic.Controls.Add(Me.lblType)
            Me.tabBasic.Controls.Add(Me.cmbType)
            Me.tabBasic.Controls.Add(Me.btnManageTypes)
            Me.tabBasic.Controls.Add(Me.lblDesc)
            Me.tabBasic.Controls.Add(Me.txtDescription)
            Me.tabBasic.Location = New Point(4, 23)
            Me.tabBasic.Name = "tabBasic"
            Me.tabBasic.Padding = New Padding(3)
            Me.tabBasic.Size = New Size(642, 293)
            Me.tabBasic.TabIndex = 0
            Me.tabBasic.Text = "اطلاعات پایه و هویتی"
            Me.tabBasic.UseVisualStyleBackColor = True

            Me.lblName.Location = New Point(520, 30)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New Size(100, 20)
            Me.lblName.Text = "نام انبار: *"
            
            Me.txtName.Location = New Point(120, 27)
            Me.txtName.MaxLength = 100
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New Size(390, 22)
            Me.txtName.TabIndex = 0

            Me.lblType.Location = New Point(520, 70)
            Me.lblType.Name = "lblType"
            Me.lblType.Size = New Size(100, 20)
            Me.lblType.Text = "نوع انبار:"
            
            Me.cmbType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbType.Location = New Point(160, 67)
            Me.cmbType.Name = "cmbType"
            Me.cmbType.Size = New Size(350, 22)
            Me.cmbType.TabIndex = 1
            Me.cmbType.Items.Add("--- انتخاب نشده ---")

            Me.btnManageTypes.Location = New Point(120, 66)
            Me.btnManageTypes.Name = "btnManageTypes"
            Me.btnManageTypes.Size = New Size(35, 24)
            Me.btnManageTypes.TabIndex = 2
            Me.btnManageTypes.Text = "..."
            Me.btnManageTypes.UseVisualStyleBackColor = True

            Me.lblDesc.Location = New Point(520, 110)
            Me.lblDesc.Name = "lblDesc"
            Me.lblDesc.Size = New Size(100, 20)
            Me.lblDesc.Text = "توضیحات:"
            
            Me.txtDescription.Location = New Point(120, 107)
            Me.txtDescription.MaxLength = 500
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New Size(390, 22)
            Me.txtDescription.TabIndex = 2

            '
            'tabContact
            '
            Me.tabContact.Controls.Add(Me.lblLocation)
            Me.tabContact.Controls.Add(Me.txtLocation)
            Me.tabContact.Controls.Add(Me.lblPhone)
            Me.tabContact.Controls.Add(Me.txtPhone)
            Me.tabContact.Controls.Add(Me.lblPhone2)
            Me.tabContact.Controls.Add(Me.txtPhone2)
            Me.tabContact.Controls.Add(Me.lblPhone3)
            Me.tabContact.Controls.Add(Me.txtPhone3)
            Me.tabContact.Controls.Add(Me.lblPostal)
            Me.tabContact.Controls.Add(Me.txtPostalCode)
            Me.tabContact.Location = New Point(4, 23)
            Me.tabContact.Name = "tabContact"
            Me.tabContact.Padding = New Padding(3)
            Me.tabContact.Size = New Size(642, 293)
            Me.tabContact.TabIndex = 1
            Me.tabContact.Text = "اطلاعات مکانی و ارتباطی"
            Me.tabContact.UseVisualStyleBackColor = True

            Me.lblLocation.Location = New Point(520, 30)
            Me.lblLocation.Name = "lblLocation"
            Me.lblLocation.Size = New Size(100, 20)
            Me.lblLocation.Text = "آدرس:"
            
            Me.txtLocation.Location = New Point(120, 27)
            Me.txtLocation.MaxLength = 200
            Me.txtLocation.Name = "txtLocation"
            Me.txtLocation.Size = New Size(390, 22)
            Me.txtLocation.TabIndex = 0

            Me.lblPhone.Location = New Point(520, 70)
            Me.lblPhone.Name = "lblPhone"
            Me.lblPhone.Size = New Size(100, 20)
            Me.lblPhone.Text = "تلفن 1:"
            
            Me.txtPhone.Location = New Point(120, 67)
            Me.txtPhone.MaxLength = 50
            Me.txtPhone.Name = "txtPhone"
            Me.txtPhone.Size = New Size(390, 22)
            Me.txtPhone.TabIndex = 1

            Me.lblPhone2.Location = New Point(520, 110)
            Me.lblPhone2.Name = "lblPhone2"
            Me.lblPhone2.Size = New Size(100, 20)
            Me.lblPhone2.Text = "تلفن 2:"
            
            Me.txtPhone2.Location = New Point(120, 107)
            Me.txtPhone2.MaxLength = 50
            Me.txtPhone2.Name = "txtPhone2"
            Me.txtPhone2.Size = New Size(390, 22)
            Me.txtPhone2.TabIndex = 2

            Me.lblPhone3.Location = New Point(520, 150)
            Me.lblPhone3.Name = "lblPhone3"
            Me.lblPhone3.Size = New Size(100, 20)
            Me.lblPhone3.Text = "تلفن 3:"
            
            Me.txtPhone3.Location = New Point(120, 147)
            Me.txtPhone3.MaxLength = 50
            Me.txtPhone3.Name = "txtPhone3"
            Me.txtPhone3.Size = New Size(390, 22)
            Me.txtPhone3.TabIndex = 3

            Me.lblPostal.Location = New Point(520, 190)
            Me.lblPostal.Name = "lblPostal"
            Me.lblPostal.Size = New Size(100, 20)
            Me.lblPostal.Text = "کد پستی:"
            
            Me.txtPostalCode.Location = New Point(120, 187)
            Me.txtPostalCode.MaxLength = 50
            Me.txtPostalCode.Name = "txtPostalCode"
            Me.txtPostalCode.Size = New Size(390, 22)
            Me.txtPostalCode.TabIndex = 4

            '
            'tabPhysical
            '
            Me.tabPhysical.Controls.Add(Me.lblCapacity)
            Me.tabPhysical.Controls.Add(Me.numCapacity)
            Me.tabPhysical.Controls.Add(Me.tvLayout)
            Me.tabPhysical.Location = New Point(4, 23)
            Me.tabPhysical.Name = "tabPhysical"
            Me.tabPhysical.Padding = New Padding(3)
            Me.tabPhysical.Size = New Size(642, 293)
            Me.tabPhysical.TabIndex = 2
            Me.tabPhysical.Text = "ساختار فیزیکی و جانمایی"
            Me.tabPhysical.UseVisualStyleBackColor = True

            Me.lblCapacity.Location = New Point(520, 15)
            Me.lblCapacity.Name = "lblCapacity"
            Me.lblCapacity.Size = New Size(100, 20)
            Me.lblCapacity.Text = "ظرفیت / گنجایش:"
            
            Me.numCapacity.Location = New Point(120, 12)
            Me.numCapacity.Maximum = New Decimal(New Integer() {999999999, 0, 0, 0})
            Me.numCapacity.Name = "numCapacity"
            Me.numCapacity.Size = New Size(390, 22)
            Me.numCapacity.TabIndex = 0

            Me.tvLayout.Location = New Point(120, 45)
            Me.tvLayout.Name = "tvLayout"
            Me.tvLayout.Size = New Size(390, 235)
            Me.tvLayout.TabIndex = 1
            Me.tvLayout.RightToLeftLayout = True
            Me.tvLayout.ContextMenuStrip = Me.ctxLayout

            Me.ctxLayout.Name = "ctxLayout"
            Me.ctxLayout.Size = New Size(153, 26)
            Me.numCapacity.TabIndex = 0

            '
            'tabManagement
            '
            Me.tabManagement.Controls.Add(Me.lblKeeper)
            Me.tabManagement.Controls.Add(Me.txtKeeper)
            Me.tabManagement.Controls.Add(Me.btnSelectKeeper)
            Me.tabManagement.Controls.Add(Me.lblCostCenter)
            Me.tabManagement.Controls.Add(Me.txtCostCenter)
            Me.tabManagement.Location = New Point(4, 23)
            Me.tabManagement.Name = "tabManagement"
            Me.tabManagement.Padding = New Padding(3)
            Me.tabManagement.Size = New Size(642, 293)
            Me.tabManagement.TabIndex = 3
            Me.tabManagement.Text = "اطلاعات مدیریتی و پرسنلی"
            Me.tabManagement.UseVisualStyleBackColor = True

            Me.lblKeeper.Location = New Point(520, 30)
            Me.lblKeeper.Name = "lblKeeper"
            Me.lblKeeper.Size = New Size(100, 20)
            Me.lblKeeper.Text = "انباردار:"
            
            Me.txtKeeper.Location = New Point(155, 27)
            Me.txtKeeper.MaxLength = 100
            Me.txtKeeper.Name = "txtKeeper"
            Me.txtKeeper.Size = New Size(355, 22)
            Me.txtKeeper.TabIndex = 0

            Me.btnSelectKeeper.Location = New Point(120, 26)
            Me.btnSelectKeeper.Name = "btnSelectKeeper"
            Me.btnSelectKeeper.Size = New Size(30, 24)
            Me.btnSelectKeeper.TabIndex = 10
            Me.btnSelectKeeper.Text = "..."
            Me.btnSelectKeeper.UseVisualStyleBackColor = True

            Me.lblCostCenter.Location = New Point(520, 70)
            Me.lblCostCenter.Name = "lblCostCenter"
            Me.lblCostCenter.Size = New Size(100, 20)
            Me.lblCostCenter.Text = "مرکز هزینه:"
            
            Me.txtCostCenter.Location = New Point(120, 67)
            Me.txtCostCenter.MaxLength = 100
            Me.txtCostCenter.Name = "txtCostCenter"
            Me.txtCostCenter.Size = New Size(390, 22)
            Me.txtCostCenter.TabIndex = 1

            '
            'tabRules
            '
            Me.tabRules.Controls.Add(Me.chkActive)
            Me.tabRules.Controls.Add(Me.chkAllowNegative)
            Me.tabRules.Location = New Point(4, 23)
            Me.tabRules.Name = "tabRules"
            Me.tabRules.Padding = New Padding(3)
            Me.tabRules.Size = New Size(642, 293)
            Me.tabRules.TabIndex = 4
            Me.tabRules.Text = "قوانین و محدودیت‌های عملیاتی"
            Me.tabRules.UseVisualStyleBackColor = True

            Me.chkActive.AutoSize = True
            Me.chkActive.Checked = True
            Me.chkActive.Location = New Point(390, 30)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New Size(120, 18)
            Me.chkActive.TabIndex = 0
            Me.chkActive.Text = "انبار فعال است"

            Me.chkAllowNegative.AutoSize = True
            Me.chkAllowNegative.Location = New Point(360, 70)
            Me.chkAllowNegative.Name = "chkAllowNegative"
            Me.chkAllowNegative.Size = New Size(150, 18)
            Me.chkAllowNegative.TabIndex = 1
            Me.chkAllowNegative.Text = "اجازه موجودی منفی"

            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = Color.FromArgb(240, 240, 240)
            Me.pnlBottom.Controls.Add(Me.btnCancel)
            Me.pnlBottom.Controls.Add(Me.btnSave)
            Me.pnlBottom.Dock = DockStyle.Bottom
            Me.pnlBottom.Location = New Point(0, 320)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Size = New Size(650, 60)
            Me.pnlBottom.TabIndex = 1

            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(30, 120, 60)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(300, 15)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(110, 30)
            Me.btnSave.TabIndex = 0
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = Color.Gray
            Me.btnCancel.FlatAppearance.BorderSize = 0
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(440, 15)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(110, 30)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False

            '
            'AnbardaryNamAnbar2Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(850, 380)
            Me.Controls.Add(Me.tabMain)
            Me.Controls.Add(Me.pnlBottom)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbardaryNamAnbar2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش اطلاعات انبار"
            
            Me.tabMain.ResumeLayout(False)
            Me.tabBasic.ResumeLayout(False)
            Me.tabBasic.PerformLayout()
            Me.tabContact.ResumeLayout(False)
            Me.tabContact.PerformLayout()
            Me.tabPhysical.ResumeLayout(False)
            Me.tabPhysical.PerformLayout()
            Me.tabManagement.ResumeLayout(False)
            Me.tabManagement.PerformLayout()
            Me.tabRules.ResumeLayout(False)
            Me.tabRules.PerformLayout()
            CType(Me.numCapacity, ISupportInitialize).EndInit()
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
