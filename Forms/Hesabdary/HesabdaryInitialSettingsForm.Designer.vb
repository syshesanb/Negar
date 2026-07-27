Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class HesabdaryInitialSettingsForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents grpCodingType As GroupBox
        Friend WithEvents lblCodingType As Label
        Friend WithEvents cmbCodingType As ComboBox

        Friend WithEvents grpTaxEconomic As GroupBox
        Friend WithEvents lblEconomicCode As Label
        Friend WithEvents txtEconomicCode As TextBox
        Friend WithEvents lblTaxId As Label
        Friend WithEvents txtTaxId As TextBox

        Friend WithEvents grpCoding As GroupBox
        Friend WithEvents lblAccountLevels As Label
        Friend WithEvents numAccountLevels As NumericUpDown
        Friend WithEvents lblLevel1Length As Label
        Friend WithEvents numLevel1Length As NumericUpDown
        Friend WithEvents lblLevel2Length As Label
        Friend WithEvents numLevel2Length As NumericUpDown
        Friend WithEvents lblLevel3Length As Label
        Friend WithEvents numLevel3Length As NumericUpDown
        Friend WithEvents lblLevel4Length As Label
        Friend WithEvents numLevel4Length As NumericUpDown
        Friend WithEvents lblLevel5Length As Label
        Friend WithEvents numLevel5Length As NumericUpDown
        Friend WithEvents lblLevel6Length As Label
        Friend WithEvents numLevel6Length As NumericUpDown

        Friend WithEvents btnSave As Button

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.grpCodingType = New System.Windows.Forms.GroupBox()
            Me.lblCodingType = New System.Windows.Forms.Label()
            Me.cmbCodingType = New System.Windows.Forms.ComboBox()

            Me.grpTaxEconomic = New System.Windows.Forms.GroupBox()
            Me.lblEconomicCode = New System.Windows.Forms.Label()
            Me.txtEconomicCode = New System.Windows.Forms.TextBox()
            Me.lblTaxId = New System.Windows.Forms.Label()
            Me.txtTaxId = New System.Windows.Forms.TextBox()

            Me.grpCoding = New System.Windows.Forms.GroupBox()
            Me.lblAccountLevels = New System.Windows.Forms.Label()
            Me.numAccountLevels = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel1Length = New System.Windows.Forms.Label()
            Me.numLevel1Length = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel2Length = New System.Windows.Forms.Label()
            Me.numLevel2Length = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel3Length = New System.Windows.Forms.Label()
            Me.numLevel3Length = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel4Length = New System.Windows.Forms.Label()
            Me.numLevel4Length = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel5Length = New System.Windows.Forms.Label()
            Me.numLevel5Length = New System.Windows.Forms.NumericUpDown()
            Me.lblLevel6Length = New System.Windows.Forms.Label()
            Me.numLevel6Length = New System.Windows.Forms.NumericUpDown()

            Me.btnSave = New System.Windows.Forms.Button()

            Me.grpCodingType.SuspendLayout()
            Me.grpTaxEconomic.SuspendLayout()
            Me.grpCoding.SuspendLayout()
            CType(Me.numAccountLevels, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel1Length, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel2Length, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel3Length, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel4Length, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel5Length, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numLevel6Length, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            ' grpCodingType
            '
            Me.grpCodingType.Controls.Add(Me.lblCodingType)
            Me.grpCodingType.Controls.Add(Me.cmbCodingType)
            Me.grpCodingType.Location = New System.Drawing.Point(340, 20)
            Me.grpCodingType.Name = "grpCodingType"
            Me.grpCodingType.Size = New System.Drawing.Size(300, 70)
            Me.grpCodingType.TabIndex = 0
            Me.grpCodingType.TabStop = False
            Me.grpCodingType.Text = "نوع کد سرفصل مورد استفاده"

            '
            ' lblCodingType
            '
            Me.lblCodingType.Location = New System.Drawing.Point(200, 28)
            Me.lblCodingType.Name = "lblCodingType"
            Me.lblCodingType.Size = New System.Drawing.Size(90, 20)
            Me.lblCodingType.TabIndex = 0
            Me.lblCodingType.Text = "نوع سرفصل:"
            Me.lblCodingType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' cmbCodingType
            '
            Me.cmbCodingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCodingType.FormattingEnabled = True
            Me.cmbCodingType.Items.AddRange(New Object() {
                "--- انتخاب نشده ---",
                "سرفصل های پیش فرض برنامه",
                "ایجاد سرفصل بصورت دستی"
            })
            Me.cmbCodingType.Location = New System.Drawing.Point(15, 25)
            Me.cmbCodingType.Name = "cmbCodingType"
            Me.cmbCodingType.Size = New System.Drawing.Size(180, 22)
            Me.cmbCodingType.TabIndex = 1

            '
            ' grpTaxEconomic
            '
            Me.grpTaxEconomic.Controls.Add(Me.lblEconomicCode)
            Me.grpTaxEconomic.Controls.Add(Me.txtEconomicCode)
            Me.grpTaxEconomic.Controls.Add(Me.lblTaxId)
            Me.grpTaxEconomic.Controls.Add(Me.txtTaxId)
            Me.grpTaxEconomic.Location = New System.Drawing.Point(340, 100)
            Me.grpTaxEconomic.Name = "grpTaxEconomic"
            Me.grpTaxEconomic.Size = New System.Drawing.Size(300, 95)
            Me.grpTaxEconomic.TabIndex = 1
            Me.grpTaxEconomic.TabStop = False
            Me.grpTaxEconomic.Text = "مشخصات مالیاتی و اقتصادی"

            '
            ' lblEconomicCode
            '
            Me.lblEconomicCode.Location = New System.Drawing.Point(180, 23)
            Me.lblEconomicCode.Name = "lblEconomicCode"
            Me.lblEconomicCode.Size = New System.Drawing.Size(110, 20)
            Me.lblEconomicCode.TabIndex = 0
            Me.lblEconomicCode.Text = "کد اقتصادی:"
            Me.lblEconomicCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' txtEconomicCode
            '
            Me.txtEconomicCode.Location = New System.Drawing.Point(15, 20)
            Me.txtEconomicCode.Name = "txtEconomicCode"
            Me.txtEconomicCode.Size = New System.Drawing.Size(160, 22)
            Me.txtEconomicCode.TabIndex = 1

            '
            ' lblTaxId
            '
            Me.lblTaxId.Location = New System.Drawing.Point(180, 53)
            Me.lblTaxId.Name = "lblTaxId"
            Me.lblTaxId.Size = New System.Drawing.Size(110, 20)
            Me.lblTaxId.TabIndex = 2
            Me.lblTaxId.Text = "شناسه ملی:"
            Me.lblTaxId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' txtTaxId
            '
            Me.txtTaxId.Location = New System.Drawing.Point(15, 50)
            Me.txtTaxId.Name = "txtTaxId"
            Me.txtTaxId.Size = New System.Drawing.Size(160, 22)
            Me.txtTaxId.TabIndex = 3

            '
            ' grpCoding
            '
            Me.grpCoding.Controls.Add(Me.lblAccountLevels)
            Me.grpCoding.Controls.Add(Me.numAccountLevels)
            Me.grpCoding.Controls.Add(Me.lblLevel1Length)
            Me.grpCoding.Controls.Add(Me.numLevel1Length)
            Me.grpCoding.Controls.Add(Me.lblLevel2Length)
            Me.grpCoding.Controls.Add(Me.numLevel2Length)
            Me.grpCoding.Controls.Add(Me.lblLevel3Length)
            Me.grpCoding.Controls.Add(Me.numLevel3Length)
            Me.grpCoding.Controls.Add(Me.lblLevel4Length)
            Me.grpCoding.Controls.Add(Me.numLevel4Length)
            Me.grpCoding.Controls.Add(Me.lblLevel5Length)
            Me.grpCoding.Controls.Add(Me.numLevel5Length)
            Me.grpCoding.Controls.Add(Me.lblLevel6Length)
            Me.grpCoding.Controls.Add(Me.numLevel6Length)
            Me.grpCoding.Location = New System.Drawing.Point(20, 20)
            Me.grpCoding.Name = "grpCoding"
            Me.grpCoding.Size = New System.Drawing.Size(300, 280)
            Me.grpCoding.TabIndex = 2
            Me.grpCoding.TabStop = False
            Me.grpCoding.Text = "تنظیمات کدینگ حسابها"

            '
            ' lblAccountLevels
            '
            Me.lblAccountLevels.Location = New System.Drawing.Point(180, 23)
            Me.lblAccountLevels.Name = "lblAccountLevels"
            Me.lblAccountLevels.Size = New System.Drawing.Size(110, 20)
            Me.lblAccountLevels.TabIndex = 0
            Me.lblAccountLevels.Text = "تعداد سطوح حساب:"
            Me.lblAccountLevels.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numAccountLevels
            '
            Me.numAccountLevels.Location = New System.Drawing.Point(15, 20)
            Me.numAccountLevels.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numAccountLevels.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numAccountLevels.Name = "numAccountLevels"
            Me.numAccountLevels.Size = New System.Drawing.Size(160, 22)
            Me.numAccountLevels.TabIndex = 1
            Me.numAccountLevels.Value = New Decimal(New Integer() {4, 0, 0, 0})

            '
            ' lblLevel1Length
            '
            Me.lblLevel1Length.Location = New System.Drawing.Point(180, 53)
            Me.lblLevel1Length.Name = "lblLevel1Length"
            Me.lblLevel1Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel1Length.TabIndex = 2
            Me.lblLevel1Length.Text = "طول کد سطح گروه:"
            Me.lblLevel1Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel1Length
            '
            Me.numLevel1Length.Location = New System.Drawing.Point(15, 50)
            Me.numLevel1Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel1Length.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numLevel1Length.Name = "numLevel1Length"
            Me.numLevel1Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel1Length.TabIndex = 3
            Me.numLevel1Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' lblLevel2Length
            '
            Me.lblLevel2Length.Location = New System.Drawing.Point(180, 83)
            Me.lblLevel2Length.Name = "lblLevel2Length"
            Me.lblLevel2Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel2Length.TabIndex = 4
            Me.lblLevel2Length.Text = "طول کد سطح کل:"
            Me.lblLevel2Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel2Length
            '
            Me.numLevel2Length.Location = New System.Drawing.Point(15, 80)
            Me.numLevel2Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel2Length.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numLevel2Length.Name = "numLevel2Length"
            Me.numLevel2Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel2Length.TabIndex = 5
            Me.numLevel2Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' lblLevel3Length
            '
            Me.lblLevel3Length.Location = New System.Drawing.Point(180, 113)
            Me.lblLevel3Length.Name = "lblLevel3Length"
            Me.lblLevel3Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel3Length.TabIndex = 6
            Me.lblLevel3Length.Text = "طول کد سطح معین:"
            Me.lblLevel3Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel3Length
            '
            Me.numLevel3Length.Location = New System.Drawing.Point(15, 110)
            Me.numLevel3Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel3Length.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numLevel3Length.Name = "numLevel3Length"
            Me.numLevel3Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel3Length.TabIndex = 7
            Me.numLevel3Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' lblLevel4Length
            '
            Me.lblLevel4Length.Location = New System.Drawing.Point(180, 143)
            Me.lblLevel4Length.Name = "lblLevel4Length"
            Me.lblLevel4Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel4Length.TabIndex = 8
            Me.lblLevel4Length.Text = "طول کد تفصیلی ۱:"
            Me.lblLevel4Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel4Length
            '
            Me.numLevel4Length.Location = New System.Drawing.Point(15, 140)
            Me.numLevel4Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel4Length.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numLevel4Length.Name = "numLevel4Length"
            Me.numLevel4Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel4Length.TabIndex = 9
            Me.numLevel4Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' lblLevel5Length
            '
            Me.lblLevel5Length.Location = New System.Drawing.Point(180, 173)
            Me.lblLevel5Length.Name = "lblLevel5Length"
            Me.lblLevel5Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel5Length.TabIndex = 10
            Me.lblLevel5Length.Text = "طول کد تفصیلی ۲:"
            Me.lblLevel5Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel5Length
            '
            Me.numLevel5Length.Location = New System.Drawing.Point(15, 170)
            Me.numLevel5Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel5Length.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numLevel5Length.Name = "numLevel5Length"
            Me.numLevel5Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel5Length.TabIndex = 11
            Me.numLevel5Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' lblLevel6Length
            '
            Me.lblLevel6Length.Location = New System.Drawing.Point(180, 203)
            Me.lblLevel6Length.Name = "lblLevel6Length"
            Me.lblLevel6Length.Size = New System.Drawing.Size(110, 20)
            Me.lblLevel6Length.TabIndex = 12
            Me.lblLevel6Length.Text = "طول کد تفصیلی ۳:"
            Me.lblLevel6Length.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            '
            ' numLevel6Length
            '
            Me.numLevel6Length.Location = New System.Drawing.Point(15, 200)
            Me.numLevel6Length.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numLevel6Length.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numLevel6Length.Name = "numLevel6Length"
            Me.numLevel6Length.Size = New System.Drawing.Size(160, 22)
            Me.numLevel6Length.TabIndex = 13
            Me.numLevel6Length.Value = New Decimal(New Integer() {2, 0, 0, 0})

            '
            ' btnSave
            '
            Me.btnSave.Location = New System.Drawing.Point(340, 210)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(120, 32)
            Me.btnSave.TabIndex = 3
            Me.btnSave.Text = "ذخیره تنظیمات"
            Me.btnSave.UseVisualStyleBackColor = True

            '
            ' HesabdaryInitialSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(800, 600)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.grpCoding)
            Me.Controls.Add(Me.grpTaxEconomic)
            Me.Controls.Add(Me.grpCodingType)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryInitialSettingsForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Text = "تنظیمات اولیه حسابداری"
            Me.grpCodingType.ResumeLayout(False)
            Me.grpTaxEconomic.ResumeLayout(False)
            Me.grpTaxEconomic.PerformLayout()
            Me.grpCoding.ResumeLayout(False)
            Me.grpCoding.PerformLayout()
            CType(Me.numAccountLevels, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel1Length, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel2Length, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel3Length, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel4Length, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel5Length, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numLevel6Length, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
