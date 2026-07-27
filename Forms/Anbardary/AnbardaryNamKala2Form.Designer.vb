Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryNamKala2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabsProduct As TabControl
        Friend WithEvents tabGeneral As TabPage
        Friend WithEvents tabPhysical As TabPage
        Friend WithEvents tabPricing As TabPage
        Friend WithEvents tabInventory As TabPage

        ' Tab General Controls
        Friend WithEvents lblCode As Label
        Friend WithEvents txtCode As TextBox
        Friend WithEvents lblName As Label
        Friend WithEvents txtName As TextBox
        Friend WithEvents lblTechnicalName As Label
        Friend WithEvents txtTechnicalName As TextBox
        Friend WithEvents lblGroup As Label
        Friend WithEvents txtGroup As TextBox
        Friend WithEvents btnBrowseGroup As Button
        Friend WithEvents lblBarcode As Label
        Friend WithEvents txtBarcode As TextBox
        Friend WithEvents lblTaxID As Label
        Friend WithEvents txtTaxID As TextBox
        Friend WithEvents lblProductType As Label
        Friend WithEvents cmbProductType As ComboBox

        ' Tab Physical Controls
        Friend WithEvents grpProductImages As GroupBox
        Friend WithEvents picImage1 As PictureBox
        Friend WithEvents btnBrowseImage1 As Button
        Friend WithEvents btnRemoveImage1 As Button
        Friend WithEvents picImage2 As PictureBox
        Friend WithEvents btnBrowseImage2 As Button
        Friend WithEvents btnRemoveImage2 As Button
        Friend WithEvents picImage3 As PictureBox
        Friend WithEvents btnBrowseImage3 As Button
        Friend WithEvents btnRemoveImage3 As Button
        Friend WithEvents picImage4 As PictureBox
        Friend WithEvents btnBrowseImage4 As Button
        Friend WithEvents btnRemoveImage4 As Button
        Friend WithEvents picImage5 As PictureBox
        Friend WithEvents btnBrowseImage5 As Button
        Friend WithEvents btnRemoveImage5 As Button
        Friend WithEvents picImage6 As PictureBox
        Friend WithEvents btnBrowseImage6 As Button
        Friend WithEvents btnRemoveImage6 As Button

        Friend WithEvents lblNetWeight As Label
        Friend WithEvents numNetWeight As NumericUpDown
        Friend WithEvents lblGrossWeight As Label
        Friend WithEvents numGrossWeight As NumericUpDown
        Friend WithEvents lblDimensions As Label
        Friend WithEvents numLength As NumericUpDown
        Friend WithEvents lblDimX1 As Label
        Friend WithEvents numWidth As NumericUpDown
        Friend WithEvents lblDimX2 As Label
        Friend WithEvents numHeight As NumericUpDown
        Friend WithEvents lblVolume As Label
        Friend WithEvents numVolume As NumericUpDown
        Friend WithEvents lblColor As Label
        Friend WithEvents txtColor As TextBox
        Friend WithEvents lblMaterial As Label
        Friend WithEvents txtMaterial As TextBox
        Friend WithEvents lblSize As Label
        Friend WithEvents txtSize As TextBox
        Friend WithEvents lblBrand As Label
        Friend WithEvents txtBrand As TextBox
        Friend WithEvents lblCountryOfOrigin As Label
        Friend WithEvents txtCountryOfOrigin As TextBox
        Friend WithEvents lblPhysicalDescription As Label
        Friend WithEvents txtPhysicalDescription As TextBox

        ' Tab Pricing Controls
        Friend WithEvents lblBaseUoM As Label
        Friend WithEvents txtBaseUoM As TextBox
        Friend WithEvents btnBrowseBaseUoM As Button
        Friend WithEvents lblSecondaryUoM As Label
        Friend WithEvents txtSecondaryUoM As TextBox
        Friend WithEvents btnBrowseSecondaryUoM As Button
        Friend WithEvents lblNominalFactor As Label
        Friend WithEvents numNominalFactor As NumericUpDown
        Friend WithEvents lblPurchasePrice As Label
        Friend WithEvents txtPurchasePrice As TextBox
        Friend WithEvents lblPrice As Label
        Friend WithEvents txtPrice As TextBox

        ' Pricing & Discount Panel Controls
        Friend WithEvents grpSalePricingProduct As GroupBox
        Friend WithEvents lblConsumerFormulaProduct As Label
        Friend WithEvents numConsumerMarkupProduct As NumericUpDown
        Friend WithEvents lblConsumerMarkupText As Label
        Friend WithEvents lblConsumerDiscountText As Label
        Friend WithEvents numConsumerDiscountProduct As NumericUpDown

        Friend WithEvents lblColleagueFormulaProduct As Label
        Friend WithEvents numColleagueMarkupProduct As NumericUpDown
        Friend WithEvents lblColleagueMarkupText As Label
        Friend WithEvents lblColleagueDiscountText As Label
        Friend WithEvents numColleagueDiscountProduct As NumericUpDown

        Friend WithEvents lblWholesaleFormulaProduct As Label
        Friend WithEvents numWholesaleMarkupProduct As NumericUpDown
        Friend WithEvents lblWholesaleMarkupText As Label
        Friend WithEvents lblWholesaleDiscountText As Label
        Friend WithEvents numWholesaleDiscountProduct As NumericUpDown

        ' Tax and Toll Controls
        Friend WithEvents lblTaxPercent As Label
        Friend WithEvents numTaxPercent As NumericUpDown
        Friend WithEvents lblTollPercent As Label
        Friend WithEvents numTollPercent As NumericUpDown

        ' Tab Inventory Controls
        Friend WithEvents lblMinStock As Label
        Friend WithEvents numMinStock As NumericUpDown
        Friend WithEvents lblReorderPoint As Label
        Friend WithEvents numReorderPoint As NumericUpDown
        Friend WithEvents lblMaxStock As Label
        Friend WithEvents numMaxStock As NumericUpDown
        Friend WithEvents lblTrackingType As Label
        Friend WithEvents cmbTrackingType As ComboBox
        Friend WithEvents lblPhysicalLocation As Label
        Friend WithEvents btnSelectLocation As Button
        Friend WithEvents lblPhysicalLocationName As Label
        Friend WithEvents lblPhysicalLocationCode As Label
        Friend WithEvents lblDefaultWarehouseTitle As Label
        Friend WithEvents btnSelectDefaultWarehouse As Button
        Friend WithEvents lblDefaultWarehouseName As Label
        Friend WithEvents chkActive As CheckBox

        ' Main Buttons
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabsProduct = New TabControl()
            Me.tabGeneral = New TabPage()
            Me.tabPhysical = New TabPage()
            Me.tabPricing = New TabPage()
            Me.tabInventory = New TabPage()

            ' General Tab
            Me.lblCode = New Label()
            Me.txtCode = New TextBox()
            Me.lblName = New Label()
            Me.txtName = New TextBox()
            Me.lblTechnicalName = New Label()
            Me.txtTechnicalName = New TextBox()
            Me.lblGroup = New Label()
            Me.txtGroup = New TextBox()
            Me.btnBrowseGroup = New Button()
            Me.lblBarcode = New Label()
            Me.txtBarcode = New TextBox()
            Me.lblTaxID = New Label()
            Me.txtTaxID = New TextBox()
            Me.lblProductType = New Label()
            Me.cmbProductType = New ComboBox()

            ' Physical Tab Controls
            Me.grpProductImages = New GroupBox()
            Me.picImage1 = New PictureBox()
            Me.btnBrowseImage1 = New Button()
            Me.btnRemoveImage1 = New Button()
            Me.picImage2 = New PictureBox()
            Me.btnBrowseImage2 = New Button()
            Me.btnRemoveImage2 = New Button()
            Me.picImage3 = New PictureBox()
            Me.btnBrowseImage3 = New Button()
            Me.btnRemoveImage3 = New Button()
            Me.picImage4 = New PictureBox()
            Me.btnBrowseImage4 = New Button()
            Me.btnRemoveImage4 = New Button()
            Me.picImage5 = New PictureBox()
            Me.btnBrowseImage5 = New Button()
            Me.btnRemoveImage5 = New Button()
            Me.picImage6 = New PictureBox()
            Me.btnBrowseImage6 = New Button()
            Me.btnRemoveImage6 = New Button()

            Me.lblNetWeight = New Label()
            Me.numNetWeight = New NumericUpDown()
            Me.lblGrossWeight = New Label()
            Me.numGrossWeight = New NumericUpDown()
            Me.lblDimensions = New Label()
            Me.numLength = New NumericUpDown()
            Me.lblDimX1 = New Label()
            Me.numWidth = New NumericUpDown()
            Me.lblDimX2 = New Label()
            Me.numHeight = New NumericUpDown()
            Me.lblVolume = New Label()
            Me.numVolume = New NumericUpDown()
            Me.lblColor = New Label()
            Me.txtColor = New TextBox()
            Me.lblMaterial = New Label()
            Me.txtMaterial = New TextBox()
            Me.lblSize = New Label()
            Me.txtSize = New TextBox()
            Me.lblBrand = New Label()
            Me.txtBrand = New TextBox()
            Me.lblCountryOfOrigin = New Label()
            Me.txtCountryOfOrigin = New TextBox()
            Me.lblPhysicalDescription = New Label()
            Me.txtPhysicalDescription = New TextBox()

            ' Pricing Tab
            Me.lblBaseUoM = New Label()
            Me.txtBaseUoM = New TextBox()
            Me.btnBrowseBaseUoM = New Button()
            Me.lblSecondaryUoM = New Label()
            Me.txtSecondaryUoM = New TextBox()
            Me.btnBrowseSecondaryUoM = New Button()
            Me.lblNominalFactor = New Label()
            Me.numNominalFactor = New NumericUpDown()
            Me.lblPurchasePrice = New Label()
            Me.txtPurchasePrice = New TextBox()
            Me.lblPrice = New Label()
            Me.txtPrice = New TextBox()

            ' Pricing & Discount GroupBox
            Me.grpSalePricingProduct = New GroupBox()
            Me.lblConsumerFormulaProduct = New Label()
            Me.numConsumerMarkupProduct = New NumericUpDown()
            Me.lblConsumerMarkupText = New Label()
            Me.lblConsumerDiscountText = New Label()
            Me.numConsumerDiscountProduct = New NumericUpDown()

            Me.lblColleagueFormulaProduct = New Label()
            Me.numColleagueMarkupProduct = New NumericUpDown()
            Me.lblColleagueMarkupText = New Label()
            Me.lblColleagueDiscountText = New Label()
            Me.numColleagueDiscountProduct = New NumericUpDown()

            Me.lblWholesaleFormulaProduct = New Label()
            Me.numWholesaleMarkupProduct = New NumericUpDown()
            Me.lblWholesaleMarkupText = New Label()
            Me.lblWholesaleDiscountText = New Label()
            Me.numWholesaleDiscountProduct = New NumericUpDown()

            ' Tax and Toll
            Me.lblTaxPercent = New Label()
            Me.numTaxPercent = New NumericUpDown()
            Me.lblTollPercent = New Label()
            Me.numTollPercent = New NumericUpDown()

            ' Inventory Tab
            Me.lblMinStock = New Label()
            Me.numMinStock = New NumericUpDown()
            Me.lblReorderPoint = New Label()
            Me.numReorderPoint = New NumericUpDown()
            Me.lblMaxStock = New Label()
            Me.numMaxStock = New NumericUpDown()
            Me.lblTrackingType = New Label()
            Me.cmbTrackingType = New ComboBox()
            Me.lblPhysicalLocation = New Label()
            Me.btnSelectLocation = New Button()
            Me.lblPhysicalLocationName = New Label()
            Me.lblPhysicalLocationCode = New Label()
            Me.lblDefaultWarehouseTitle = New Label()
            Me.btnSelectDefaultWarehouse = New Button()
            Me.lblDefaultWarehouseName = New Label()
            Me.chkActive = New CheckBox()

            ' Main Buttons
            Me.btnSave = New Button()
            Me.btnCancel = New Button()

            Me.tabsProduct.SuspendLayout()
            Me.tabGeneral.SuspendLayout()
            Me.tabPhysical.SuspendLayout()
            Me.grpProductImages.SuspendLayout()
            CType(Me.picImage1, ISupportInitialize).BeginInit()
            CType(Me.picImage2, ISupportInitialize).BeginInit()
            CType(Me.picImage3, ISupportInitialize).BeginInit()
            CType(Me.picImage4, ISupportInitialize).BeginInit()
            CType(Me.picImage5, ISupportInitialize).BeginInit()
            CType(Me.picImage6, ISupportInitialize).BeginInit()
            Me.tabPricing.SuspendLayout()
            Me.grpSalePricingProduct.SuspendLayout()
            Me.tabInventory.SuspendLayout()
            CType(Me.numNetWeight, ISupportInitialize).BeginInit()
            CType(Me.numGrossWeight, ISupportInitialize).BeginInit()
            CType(Me.numLength, ISupportInitialize).BeginInit()
            CType(Me.numWidth, ISupportInitialize).BeginInit()
            CType(Me.numHeight, ISupportInitialize).BeginInit()
            CType(Me.numVolume, ISupportInitialize).BeginInit()
            CType(Me.numNominalFactor, ISupportInitialize).BeginInit()
            CType(Me.numConsumerMarkupProduct, ISupportInitialize).BeginInit()
            CType(Me.numConsumerDiscountProduct, ISupportInitialize).BeginInit()
            CType(Me.numColleagueMarkupProduct, ISupportInitialize).BeginInit()
            CType(Me.numColleagueDiscountProduct, ISupportInitialize).BeginInit()
            CType(Me.numWholesaleMarkupProduct, ISupportInitialize).BeginInit()
            CType(Me.numWholesaleDiscountProduct, ISupportInitialize).BeginInit()
            CType(Me.numTaxPercent, ISupportInitialize).BeginInit()
            CType(Me.numTollPercent, ISupportInitialize).BeginInit()
            CType(Me.numMinStock, ISupportInitialize).BeginInit()
            CType(Me.numReorderPoint, ISupportInitialize).BeginInit()
            CType(Me.numMaxStock, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            'tabsProduct
            '
            Me.tabsProduct.Controls.Add(Me.tabGeneral)
            Me.tabsProduct.Controls.Add(Me.tabPhysical)
            Me.tabsProduct.Controls.Add(Me.tabPricing)
            Me.tabsProduct.Controls.Add(Me.tabInventory)
            Me.tabsProduct.Location = New Point(10, 10)
            Me.tabsProduct.Name = "tabsProduct"
            Me.tabsProduct.SelectedIndex = 0
            Me.tabsProduct.Size = New Size(760, 450)
            Me.tabsProduct.TabIndex = 0

            '
            'tabGeneral
            '
            Me.tabGeneral.Controls.Add(Me.lblCode)
            Me.tabGeneral.Controls.Add(Me.txtCode)
            Me.tabGeneral.Controls.Add(Me.lblName)
            Me.tabGeneral.Controls.Add(Me.txtName)
            Me.tabGeneral.Controls.Add(Me.lblTechnicalName)
            Me.tabGeneral.Controls.Add(Me.txtTechnicalName)
            Me.tabGeneral.Controls.Add(Me.lblGroup)
            Me.tabGeneral.Controls.Add(Me.txtGroup)
            Me.tabGeneral.Controls.Add(Me.btnBrowseGroup)
            Me.tabGeneral.Controls.Add(Me.lblBarcode)
            Me.tabGeneral.Controls.Add(Me.txtBarcode)
            Me.tabGeneral.Controls.Add(Me.lblTaxID)
            Me.tabGeneral.Controls.Add(Me.txtTaxID)
            Me.tabGeneral.Controls.Add(Me.lblProductType)
            Me.tabGeneral.Controls.Add(Me.cmbProductType)
            Me.tabGeneral.Location = New Point(4, 23)
            Me.tabGeneral.Name = "tabGeneral"
            Me.tabGeneral.Padding = New Padding(10)
            Me.tabGeneral.Size = New Size(752, 423)
            Me.tabGeneral.TabIndex = 0
            Me.tabGeneral.Text = "مشخصات عمومی"
            Me.tabGeneral.UseVisualStyleBackColor = True

            '
            'lblCode
            '
            Me.lblCode.Location = New Point(600, 15)
            Me.lblCode.Name = "lblCode"
            Me.lblCode.Size = New Size(140, 20)
            Me.lblCode.Text = "کد کالا:"
            Me.lblCode.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtCode
            '
            Me.txtCode.Location = New Point(20, 12)
            Me.txtCode.MaxLength = 20
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New Size(570, 22)
            Me.txtCode.TabIndex = 0

            '
            'lblName
            '
            Me.lblName.Location = New Point(600, 50)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New Size(140, 20)
            Me.lblName.Text = "نام کالا (خدمت): *"
            Me.lblName.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtName
            '
            Me.txtName.Location = New Point(20, 47)
            Me.txtName.MaxLength = 100
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New Size(570, 22)
            Me.txtName.TabIndex = 1

            '
            'lblTechnicalName
            '
            Me.lblTechnicalName.Location = New Point(600, 85)
            Me.lblTechnicalName.Name = "lblTechnicalName"
            Me.lblTechnicalName.Size = New Size(140, 20)
            Me.lblTechnicalName.Text = "نام فنی / لاتین:"
            Me.lblTechnicalName.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtTechnicalName
            '
            Me.txtTechnicalName.Location = New Point(20, 82)
            Me.txtTechnicalName.MaxLength = 100
            Me.txtTechnicalName.Name = "txtTechnicalName"
            Me.txtTechnicalName.Size = New Size(570, 22)
            Me.txtTechnicalName.TabIndex = 2

            '
            'lblGroup
            '
            Me.lblGroup.Location = New Point(600, 120)
            Me.lblGroup.Name = "lblGroup"
            Me.lblGroup.Size = New Size(140, 20)
            Me.lblGroup.Text = "گروه‌بندی کالا:"
            Me.lblGroup.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtGroup
            '
            Me.txtGroup.Location = New Point(60, 117)
            Me.txtGroup.Name = "txtGroup"
            Me.txtGroup.ReadOnly = True
            Me.txtGroup.Size = New Size(530, 22)
            Me.txtGroup.TabIndex = 3
            '
            'btnBrowseGroup
            '
            Me.btnBrowseGroup.FlatStyle = FlatStyle.System
            Me.btnBrowseGroup.Location = New Point(20, 117)
            Me.btnBrowseGroup.Name = "btnBrowseGroup"
            Me.btnBrowseGroup.Size = New Size(35, 22)
            Me.btnBrowseGroup.TabIndex = 4
            Me.btnBrowseGroup.Text = "..."
            Me.btnBrowseGroup.UseVisualStyleBackColor = True

            '
            'lblBarcode
            '
            Me.lblBarcode.Location = New Point(600, 155)
            Me.lblBarcode.Name = "lblBarcode"
            Me.lblBarcode.Size = New Size(140, 20)
            Me.lblBarcode.Text = "بارکد کالا:"
            Me.lblBarcode.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtBarcode
            '
            Me.txtBarcode.Location = New Point(20, 152)
            Me.txtBarcode.MaxLength = 30
            Me.txtBarcode.Name = "txtBarcode"
            Me.txtBarcode.Size = New Size(570, 22)
            Me.txtBarcode.TabIndex = 5

            '
            'lblTaxID
            '
            Me.lblTaxID.Location = New Point(600, 190)
            Me.lblTaxID.Name = "lblTaxID"
            Me.lblTaxID.Size = New Size(140, 20)
            Me.lblTaxID.Text = "شناسه مالیاتی (مودیان):"
            Me.lblTaxID.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtTaxID
            '
            Me.txtTaxID.Location = New Point(20, 187)
            Me.txtTaxID.MaxLength = 30
            Me.txtTaxID.Name = "txtTaxID"
            Me.txtTaxID.Size = New Size(570, 22)
            Me.txtTaxID.TabIndex = 6

            '
            'lblProductType
            '
            Me.lblProductType.Location = New Point(600, 225)
            Me.lblProductType.Name = "lblProductType"
            Me.lblProductType.Size = New Size(140, 20)
            Me.lblProductType.Text = "نوع ماهیت:"
            Me.lblProductType.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbProductType
            '
            Me.cmbProductType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbProductType.FormattingEnabled = True
            Me.cmbProductType.Items.AddRange(New Object() {"کالا", "خدمت", "دارایی شرکت", "امانی"})
            Me.cmbProductType.Location = New Point(20, 222)
            Me.cmbProductType.Name = "cmbProductType"
            Me.cmbProductType.Size = New Size(570, 22)
            Me.cmbProductType.TabIndex = 7

            '
            'tabPhysical
            '
            Me.tabPhysical.Controls.Add(Me.grpProductImages)
            Me.tabPhysical.Controls.Add(Me.lblNetWeight)
            Me.tabPhysical.Controls.Add(Me.numNetWeight)
            Me.tabPhysical.Controls.Add(Me.lblGrossWeight)
            Me.tabPhysical.Controls.Add(Me.numGrossWeight)
            Me.tabPhysical.Controls.Add(Me.lblDimensions)
            Me.tabPhysical.Controls.Add(Me.numLength)
            Me.tabPhysical.Controls.Add(Me.lblDimX1)
            Me.tabPhysical.Controls.Add(Me.numWidth)
            Me.tabPhysical.Controls.Add(Me.lblDimX2)
            Me.tabPhysical.Controls.Add(Me.numHeight)
            Me.tabPhysical.Controls.Add(Me.lblVolume)
            Me.tabPhysical.Controls.Add(Me.numVolume)
            Me.tabPhysical.Controls.Add(Me.lblColor)
            Me.tabPhysical.Controls.Add(Me.txtColor)
            Me.tabPhysical.Controls.Add(Me.lblMaterial)
            Me.tabPhysical.Controls.Add(Me.txtMaterial)
            Me.tabPhysical.Controls.Add(Me.lblSize)
            Me.tabPhysical.Controls.Add(Me.txtSize)
            Me.tabPhysical.Controls.Add(Me.lblBrand)
            Me.tabPhysical.Controls.Add(Me.txtBrand)
            Me.tabPhysical.Controls.Add(Me.lblCountryOfOrigin)
            Me.tabPhysical.Controls.Add(Me.txtCountryOfOrigin)
            Me.tabPhysical.Controls.Add(Me.lblPhysicalDescription)
            Me.tabPhysical.Controls.Add(Me.txtPhysicalDescription)
            Me.tabPhysical.Location = New Point(4, 23)
            Me.tabPhysical.Name = "tabPhysical"
            Me.tabPhysical.Padding = New Padding(10)
            Me.tabPhysical.Size = New Size(752, 423)
            Me.tabPhysical.TabIndex = 1
            Me.tabPhysical.Text = "مشخصات فیزیکی و ظاهری"
            Me.tabPhysical.UseVisualStyleBackColor = True

            '
            'grpProductImages
            '
            Me.grpProductImages.Controls.Add(Me.picImage1)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage1)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage1)
            Me.grpProductImages.Controls.Add(Me.picImage2)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage2)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage2)
            Me.grpProductImages.Controls.Add(Me.picImage3)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage3)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage3)
            Me.grpProductImages.Controls.Add(Me.picImage4)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage4)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage4)
            Me.grpProductImages.Controls.Add(Me.picImage5)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage5)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage5)
            Me.grpProductImages.Controls.Add(Me.picImage6)
            Me.grpProductImages.Controls.Add(Me.btnBrowseImage6)
            Me.grpProductImages.Controls.Add(Me.btnRemoveImage6)
            Me.grpProductImages.Location = New Point(10, 10)
            Me.grpProductImages.Name = "grpProductImages"
            Me.grpProductImages.Size = New Size(265, 400)
            Me.grpProductImages.TabIndex = 0
            Me.grpProductImages.TabStop = False
            Me.grpProductImages.Text = "تصاویر کالا (۱ تا ۶ تصویر)"

            '
            'picImage1 & buttons (Row 1 Right)
            '
            Me.picImage1.BorderStyle = BorderStyle.FixedSingle
            Me.picImage1.Location = New Point(135, 25)
            Me.picImage1.Name = "picImage1"
            Me.picImage1.Size = New Size(110, 80)
            Me.picImage1.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage1.TabIndex = 0
            Me.picImage1.TabStop = False

            Me.btnBrowseImage1.FlatStyle = FlatStyle.System
            Me.btnBrowseImage1.Location = New Point(192, 108)
            Me.btnBrowseImage1.Name = "btnBrowseImage1"
            Me.btnBrowseImage1.Size = New Size(53, 22)
            Me.btnBrowseImage1.TabIndex = 1
            Me.btnBrowseImage1.Text = "انتخاب..."
            Me.btnBrowseImage1.UseVisualStyleBackColor = True

            Me.btnRemoveImage1.FlatStyle = FlatStyle.System
            Me.btnRemoveImage1.Location = New Point(135, 108)
            Me.btnRemoveImage1.Name = "btnRemoveImage1"
            Me.btnRemoveImage1.Size = New Size(53, 22)
            Me.btnRemoveImage1.TabIndex = 2
            Me.btnRemoveImage1.Text = "حذف"
            Me.btnRemoveImage1.UseVisualStyleBackColor = True

            '
            'picImage2 & buttons (Row 1 Left)
            '
            Me.picImage2.BorderStyle = BorderStyle.FixedSingle
            Me.picImage2.Location = New Point(15, 25)
            Me.picImage2.Name = "picImage2"
            Me.picImage2.Size = New Size(110, 80)
            Me.picImage2.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage2.TabIndex = 3
            Me.picImage2.TabStop = False

            Me.btnBrowseImage2.FlatStyle = FlatStyle.System
            Me.btnBrowseImage2.Location = New Point(72, 108)
            Me.btnBrowseImage2.Name = "btnBrowseImage2"
            Me.btnBrowseImage2.Size = New Size(53, 22)
            Me.btnBrowseImage2.TabIndex = 4
            Me.btnBrowseImage2.Text = "انتخاب..."
            Me.btnBrowseImage2.UseVisualStyleBackColor = True

            Me.btnRemoveImage2.FlatStyle = FlatStyle.System
            Me.btnRemoveImage2.Location = New Point(15, 108)
            Me.btnRemoveImage2.Name = "btnRemoveImage2"
            Me.btnRemoveImage2.Size = New Size(53, 22)
            Me.btnRemoveImage2.TabIndex = 5
            Me.btnRemoveImage2.Text = "حذف"
            Me.btnRemoveImage2.UseVisualStyleBackColor = True

            '
            'picImage3 & buttons (Row 2 Right)
            '
            Me.picImage3.BorderStyle = BorderStyle.FixedSingle
            Me.picImage3.Location = New Point(135, 140)
            Me.picImage3.Name = "picImage3"
            Me.picImage3.Size = New Size(110, 80)
            Me.picImage3.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage3.TabIndex = 6
            Me.picImage3.TabStop = False

            Me.btnBrowseImage3.FlatStyle = FlatStyle.System
            Me.btnBrowseImage3.Location = New Point(192, 223)
            Me.btnBrowseImage3.Name = "btnBrowseImage3"
            Me.btnBrowseImage3.Size = New Size(53, 22)
            Me.btnBrowseImage3.TabIndex = 7
            Me.btnBrowseImage3.Text = "انتخاب..."
            Me.btnBrowseImage3.UseVisualStyleBackColor = True

            Me.btnRemoveImage3.FlatStyle = FlatStyle.System
            Me.btnRemoveImage3.Location = New Point(135, 223)
            Me.btnRemoveImage3.Name = "btnRemoveImage3"
            Me.btnRemoveImage3.Size = New Size(53, 22)
            Me.btnRemoveImage3.TabIndex = 8
            Me.btnRemoveImage3.Text = "حذف"
            Me.btnRemoveImage3.UseVisualStyleBackColor = True

            '
            'picImage4 & buttons (Row 2 Left)
            '
            Me.picImage4.BorderStyle = BorderStyle.FixedSingle
            Me.picImage4.Location = New Point(15, 140)
            Me.picImage4.Name = "picImage4"
            Me.picImage4.Size = New Size(110, 80)
            Me.picImage4.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage4.TabIndex = 9
            Me.picImage4.TabStop = False

            Me.btnBrowseImage4.FlatStyle = FlatStyle.System
            Me.btnBrowseImage4.Location = New Point(72, 223)
            Me.btnBrowseImage4.Name = "btnBrowseImage4"
            Me.btnBrowseImage4.Size = New Size(53, 22)
            Me.btnBrowseImage4.TabIndex = 10
            Me.btnBrowseImage4.Text = "انتخاب..."
            Me.btnBrowseImage4.UseVisualStyleBackColor = True

            Me.btnRemoveImage4.FlatStyle = FlatStyle.System
            Me.btnRemoveImage4.Location = New Point(15, 223)
            Me.btnRemoveImage4.Name = "btnRemoveImage4"
            Me.btnRemoveImage4.Size = New Size(53, 22)
            Me.btnRemoveImage4.TabIndex = 11
            Me.btnRemoveImage4.Text = "حذف"
            Me.btnRemoveImage4.UseVisualStyleBackColor = True

            '
            'picImage5 & buttons (Row 3 Right)
            '
            Me.picImage5.BorderStyle = BorderStyle.FixedSingle
            Me.picImage5.Location = New Point(135, 255)
            Me.picImage5.Name = "picImage5"
            Me.picImage5.Size = New Size(110, 80)
            Me.picImage5.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage5.TabIndex = 12
            Me.picImage5.TabStop = False

            Me.btnBrowseImage5.FlatStyle = FlatStyle.System
            Me.btnBrowseImage5.Location = New Point(192, 338)
            Me.btnBrowseImage5.Name = "btnBrowseImage5"
            Me.btnBrowseImage5.Size = New Size(53, 22)
            Me.btnBrowseImage5.TabIndex = 13
            Me.btnBrowseImage5.Text = "انتخاب..."
            Me.btnBrowseImage5.UseVisualStyleBackColor = True

            Me.btnRemoveImage5.FlatStyle = FlatStyle.System
            Me.btnRemoveImage5.Location = New Point(135, 338)
            Me.btnRemoveImage5.Name = "btnRemoveImage5"
            Me.btnRemoveImage5.Size = New Size(53, 22)
            Me.btnRemoveImage5.TabIndex = 14
            Me.btnRemoveImage5.Text = "حذف"
            Me.btnRemoveImage5.UseVisualStyleBackColor = True

            '
            'picImage6 & buttons (Row 3 Left)
            '
            Me.picImage6.BorderStyle = BorderStyle.FixedSingle
            Me.picImage6.Location = New Point(15, 255)
            Me.picImage6.Name = "picImage6"
            Me.picImage6.Size = New Size(110, 80)
            Me.picImage6.SizeMode = PictureBoxSizeMode.Zoom
            Me.picImage6.TabIndex = 15
            Me.picImage6.TabStop = False

            Me.btnBrowseImage6.FlatStyle = FlatStyle.System
            Me.btnBrowseImage6.Location = New Point(72, 338)
            Me.btnBrowseImage6.Name = "btnBrowseImage6"
            Me.btnBrowseImage6.Size = New Size(53, 22)
            Me.btnBrowseImage6.TabIndex = 16
            Me.btnBrowseImage6.Text = "انتخاب..."
            Me.btnBrowseImage6.UseVisualStyleBackColor = True

            Me.btnRemoveImage6.FlatStyle = FlatStyle.System
            Me.btnRemoveImage6.Location = New Point(15, 338)
            Me.btnRemoveImage6.Name = "btnRemoveImage6"
            Me.btnRemoveImage6.Size = New Size(53, 22)
            Me.btnRemoveImage6.TabIndex = 17
            Me.btnRemoveImage6.Text = "حذف"
            Me.btnRemoveImage6.UseVisualStyleBackColor = True

            '
            'lblNetWeight
            '
            Me.lblNetWeight.Location = New Point(610, 15)
            Me.lblNetWeight.Name = "lblNetWeight"
            Me.lblNetWeight.Size = New Size(130, 20)
            Me.lblNetWeight.Text = "وزن خالص (کیلوگرم):"
            Me.lblNetWeight.TextAlign = ContentAlignment.MiddleLeft
            '
            'numNetWeight
            '
            Me.numNetWeight.DecimalPlaces = 3
            Me.numNetWeight.Location = New Point(480, 12)
            Me.numNetWeight.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
            Me.numNetWeight.Name = "numNetWeight"
            Me.numNetWeight.Size = New Size(125, 22)
            Me.numNetWeight.TabIndex = 1
            Me.numNetWeight.TextAlign = HorizontalAlignment.Center

            '
            'lblGrossWeight
            '
            Me.lblGrossWeight.Location = New Point(390, 15)
            Me.lblGrossWeight.Name = "lblGrossWeight"
            Me.lblGrossWeight.Size = New Size(85, 20)
            Me.lblGrossWeight.Text = "وزن ناخالص:"
            Me.lblGrossWeight.TextAlign = ContentAlignment.MiddleLeft
            '
            'numGrossWeight
            '
            Me.numGrossWeight.DecimalPlaces = 3
            Me.numGrossWeight.Location = New Point(285, 12)
            Me.numGrossWeight.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
            Me.numGrossWeight.Name = "numGrossWeight"
            Me.numGrossWeight.Size = New Size(100, 22)
            Me.numGrossWeight.TabIndex = 2
            Me.numGrossWeight.TextAlign = HorizontalAlignment.Center

            '
            'lblDimensions
            '
            Me.lblDimensions.Location = New Point(610, 50)
            Me.lblDimensions.Name = "lblDimensions"
            Me.lblDimensions.Size = New Size(130, 20)
            Me.lblDimensions.Text = "ابعاد (طول×عرض×ارتفاع):"
            Me.lblDimensions.TextAlign = ContentAlignment.MiddleLeft
            '
            'numLength
            '
            Me.numLength.DecimalPlaces = 2
            Me.numLength.Location = New Point(535, 47)
            Me.numLength.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
            Me.numLength.Name = "numLength"
            Me.numLength.Size = New Size(70, 22)
            Me.numLength.TabIndex = 3
            Me.numLength.TextAlign = HorizontalAlignment.Center
            '
            'lblDimX1
            '
            Me.lblDimX1.Location = New Point(515, 50)
            Me.lblDimX1.Name = "lblDimX1"
            Me.lblDimX1.Size = New Size(15, 20)
            Me.lblDimX1.Text = "×"
            Me.lblDimX1.TextAlign = ContentAlignment.MiddleCenter
            '
            'numWidth
            '
            Me.numWidth.DecimalPlaces = 2
            Me.numWidth.Location = New Point(440, 47)
            Me.numWidth.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
            Me.numWidth.Name = "numWidth"
            Me.numWidth.Size = New Size(70, 22)
            Me.numWidth.TabIndex = 4
            Me.numWidth.TextAlign = HorizontalAlignment.Center
            '
            'lblDimX2
            '
            Me.lblDimX2.Location = New Point(420, 50)
            Me.lblDimX2.Name = "lblDimX2"
            Me.lblDimX2.Size = New Size(15, 20)
            Me.lblDimX2.Text = "×"
            Me.lblDimX2.TextAlign = ContentAlignment.MiddleCenter
            '
            'numHeight
            '
            Me.numHeight.DecimalPlaces = 2
            Me.numHeight.Location = New Point(345, 47)
            Me.numHeight.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
            Me.numHeight.Name = "numHeight"
            Me.numHeight.Size = New Size(70, 22)
            Me.numHeight.TabIndex = 5
            Me.numHeight.TextAlign = HorizontalAlignment.Center

            '
            'lblVolume
            '
            Me.lblVolume.Location = New Point(610, 85)
            Me.lblVolume.Name = "lblVolume"
            Me.lblVolume.Size = New Size(130, 20)
            Me.lblVolume.Text = "حجم (سم مکعب):"
            Me.lblVolume.TextAlign = ContentAlignment.MiddleLeft
            '
            'numVolume
            '
            Me.numVolume.DecimalPlaces = 2
            Me.numVolume.Location = New Point(480, 82)
            Me.numVolume.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
            Me.numVolume.Name = "numVolume"
            Me.numVolume.Size = New Size(125, 22)
            Me.numVolume.TabIndex = 6
            Me.numVolume.TextAlign = HorizontalAlignment.Center

            '
            'lblColor
            '
            Me.lblColor.Location = New Point(610, 120)
            Me.lblColor.Name = "lblColor"
            Me.lblColor.Size = New Size(130, 20)
            Me.lblColor.Text = "رنگ / ظاهری:"
            Me.lblColor.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtColor
            '
            Me.txtColor.Location = New Point(480, 117)
            Me.txtColor.MaxLength = 50
            Me.txtColor.Name = "txtColor"
            Me.txtColor.Size = New Size(125, 22)
            Me.txtColor.TabIndex = 7

            '
            'lblMaterial
            '
            Me.lblMaterial.Location = New Point(390, 120)
            Me.lblMaterial.Name = "lblMaterial"
            Me.lblMaterial.Size = New Size(85, 20)
            Me.lblMaterial.Text = "جنس / متریال:"
            Me.lblMaterial.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtMaterial
            '
            Me.txtMaterial.Location = New Point(285, 117)
            Me.txtMaterial.MaxLength = 50
            Me.txtMaterial.Name = "txtMaterial"
            Me.txtMaterial.Size = New Size(100, 22)
            Me.txtMaterial.TabIndex = 8

            '
            'lblSize
            '
            Me.lblSize.Location = New Point(610, 155)
            Me.lblSize.Name = "lblSize"
            Me.lblSize.Size = New Size(130, 20)
            Me.lblSize.Text = "سایز / ابعاد ظاهری:"
            Me.lblSize.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtSize
            '
            Me.txtSize.Location = New Point(480, 152)
            Me.txtSize.MaxLength = 50
            Me.txtSize.Name = "txtSize"
            Me.txtSize.Size = New Size(125, 22)
            Me.txtSize.TabIndex = 9

            '
            'lblBrand
            '
            Me.lblBrand.Location = New Point(390, 155)
            Me.lblBrand.Name = "lblBrand"
            Me.lblBrand.Size = New Size(85, 20)
            Me.lblBrand.Text = "برند / سازنده:"
            Me.lblBrand.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtBrand
            '
            Me.txtBrand.Location = New Point(285, 152)
            Me.txtBrand.MaxLength = 50
            Me.txtBrand.Name = "txtBrand"
            Me.txtBrand.Size = New Size(100, 22)
            Me.txtBrand.TabIndex = 10

            '
            'lblCountryOfOrigin
            '
            Me.lblCountryOfOrigin.Location = New Point(610, 190)
            Me.lblCountryOfOrigin.Name = "lblCountryOfOrigin"
            Me.lblCountryOfOrigin.Size = New Size(130, 20)
            Me.lblCountryOfOrigin.Text = "کشور سازنده:"
            Me.lblCountryOfOrigin.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtCountryOfOrigin
            '
            Me.txtCountryOfOrigin.Location = New Point(285, 187)
            Me.txtCountryOfOrigin.MaxLength = 50
            Me.txtCountryOfOrigin.Name = "txtCountryOfOrigin"
            Me.txtCountryOfOrigin.Size = New Size(320, 22)
            Me.txtCountryOfOrigin.TabIndex = 11

            '
            'lblPhysicalDescription
            '
            Me.lblPhysicalDescription.Location = New Point(610, 225)
            Me.lblPhysicalDescription.Name = "lblPhysicalDescription"
            Me.lblPhysicalDescription.Size = New Size(130, 20)
            Me.lblPhysicalDescription.Text = "توضیحات ظاهری:"
            Me.lblPhysicalDescription.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtPhysicalDescription
            '
            Me.txtPhysicalDescription.Location = New Point(285, 222)
            Me.txtPhysicalDescription.Multiline = True
            Me.txtPhysicalDescription.Name = "txtPhysicalDescription"
            Me.txtPhysicalDescription.ScrollBars = ScrollBars.Vertical
            Me.txtPhysicalDescription.Size = New Size(320, 180)
            Me.txtPhysicalDescription.TabIndex = 12

            '
            'tabPricing
            '
            Me.tabPricing.Controls.Add(Me.lblBaseUoM)
            Me.tabPricing.Controls.Add(Me.txtBaseUoM)
            Me.tabPricing.Controls.Add(Me.btnBrowseBaseUoM)
            Me.tabPricing.Controls.Add(Me.lblSecondaryUoM)
            Me.tabPricing.Controls.Add(Me.txtSecondaryUoM)
            Me.tabPricing.Controls.Add(Me.btnBrowseSecondaryUoM)
            Me.tabPricing.Controls.Add(Me.lblNominalFactor)
            Me.tabPricing.Controls.Add(Me.numNominalFactor)
            Me.tabPricing.Controls.Add(Me.lblPurchasePrice)
            Me.tabPricing.Controls.Add(Me.txtPurchasePrice)
            Me.tabPricing.Controls.Add(Me.lblPrice)
            Me.tabPricing.Controls.Add(Me.txtPrice)
            Me.tabPricing.Controls.Add(Me.grpSalePricingProduct)
            Me.tabPricing.Controls.Add(Me.lblTaxPercent)
            Me.tabPricing.Controls.Add(Me.numTaxPercent)
            Me.tabPricing.Location = New Point(4, 23)
            Me.tabPricing.Name = "tabPricing"
            Me.tabPricing.Padding = New Padding(10)
            Me.tabPricing.Size = New Size(752, 423)
            Me.tabPricing.TabIndex = 2
            Me.tabPricing.Text = "واحد و قیمت‌گذاری"
            Me.tabPricing.UseVisualStyleBackColor = True

            '
            'lblBaseUoM
            '
            Me.lblBaseUoM.Location = New Point(580, 15)
            Me.lblBaseUoM.Name = "lblBaseUoM"
            Me.lblBaseUoM.Size = New Size(160, 20)
            Me.lblBaseUoM.Text = "واحد اندازه گیری اصلی"
            Me.lblBaseUoM.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtBaseUoM
            '
            Me.txtBaseUoM.Location = New Point(60, 12)
            Me.txtBaseUoM.Name = "txtBaseUoM"
            Me.txtBaseUoM.ReadOnly = True
            Me.txtBaseUoM.Size = New Size(510, 22)
            Me.txtBaseUoM.TabIndex = 0
            '
            'btnBrowseBaseUoM
            '
            Me.btnBrowseBaseUoM.FlatStyle = FlatStyle.System
            Me.btnBrowseBaseUoM.Location = New Point(20, 12)
            Me.btnBrowseBaseUoM.Name = "btnBrowseBaseUoM"
            Me.btnBrowseBaseUoM.Size = New Size(35, 22)
            Me.btnBrowseBaseUoM.TabIndex = 1
            Me.btnBrowseBaseUoM.Text = "..."
            Me.btnBrowseBaseUoM.UseVisualStyleBackColor = True

            '
            'lblSecondaryUoM
            '
            Me.lblSecondaryUoM.Location = New Point(580, 50)
            Me.lblSecondaryUoM.Name = "lblSecondaryUoM"
            Me.lblSecondaryUoM.Size = New Size(160, 20)
            Me.lblSecondaryUoM.Text = "واحد اندازه گیری فرعی "
            Me.lblSecondaryUoM.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtSecondaryUoM
            '
            Me.txtSecondaryUoM.Location = New Point(60, 47)
            Me.txtSecondaryUoM.Name = "txtSecondaryUoM"
            Me.txtSecondaryUoM.ReadOnly = True
            Me.txtSecondaryUoM.Size = New Size(510, 22)
            Me.txtSecondaryUoM.TabIndex = 2
            '
            'btnBrowseSecondaryUoM
            '
            Me.btnBrowseSecondaryUoM.FlatStyle = FlatStyle.System
            Me.btnBrowseSecondaryUoM.Location = New Point(20, 47)
            Me.btnBrowseSecondaryUoM.Name = "btnBrowseSecondaryUoM"
            Me.btnBrowseSecondaryUoM.Size = New Size(35, 22)
            Me.btnBrowseSecondaryUoM.TabIndex = 3
            Me.btnBrowseSecondaryUoM.Text = "..."
            Me.btnBrowseSecondaryUoM.UseVisualStyleBackColor = True

            '
            'lblNominalFactor
            '
            Me.lblNominalFactor.Location = New Point(580, 85)
            Me.lblNominalFactor.Name = "lblNominalFactor"
            Me.lblNominalFactor.Size = New Size(160, 20)
            Me.lblNominalFactor.Text = "ضریب تبدیل به فرعی:"
            Me.lblNominalFactor.TextAlign = ContentAlignment.MiddleLeft
            '
            'numNominalFactor
            '
            Me.numNominalFactor.DecimalPlaces = 4
            Me.numNominalFactor.Location = New Point(450, 82)
            Me.numNominalFactor.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
            Me.numNominalFactor.Name = "numNominalFactor"
            Me.numNominalFactor.Size = New Size(120, 22)
            Me.numNominalFactor.TabIndex = 4
            Me.numNominalFactor.TextAlign = HorizontalAlignment.Center

            '
            'lblPurchasePrice
            '
            Me.lblPurchasePrice.Location = New Point(580, 120)
            Me.lblPurchasePrice.Name = "lblPurchasePrice"
            Me.lblPurchasePrice.Size = New Size(160, 20)
            Me.lblPurchasePrice.Text = "قیمت خرید پیش‌فرض:"
            Me.lblPurchasePrice.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtPurchasePrice
            '
            Me.txtPurchasePrice.Location = New Point(20, 117)
            Me.txtPurchasePrice.MaxLength = 20
            Me.txtPurchasePrice.Name = "txtPurchasePrice"
            Me.txtPurchasePrice.Size = New Size(550, 22)
            Me.txtPurchasePrice.TabIndex = 5

            '
            'lblPrice
            '
            Me.lblPrice.Location = New Point(580, 155)
            Me.lblPrice.Name = "lblPrice"
            Me.lblPrice.Size = New Size(160, 20)
            Me.lblPrice.Text = "قیمت فروش پیش‌فرض:"
            Me.lblPrice.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtPrice
            '
            Me.txtPrice.Location = New Point(20, 152)
            Me.txtPrice.MaxLength = 20
            Me.txtPrice.Name = "txtPrice"
            Me.txtPrice.Size = New Size(550, 22)
            Me.txtPrice.TabIndex = 6

            '
            'grpSalePricingProduct
            '
            Me.grpSalePricingProduct.Controls.Add(Me.lblConsumerFormulaProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.numConsumerMarkupProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.lblConsumerMarkupText)
            Me.grpSalePricingProduct.Controls.Add(Me.lblConsumerDiscountText)
            Me.grpSalePricingProduct.Controls.Add(Me.numConsumerDiscountProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.lblColleagueFormulaProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.numColleagueMarkupProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.lblColleagueMarkupText)
            Me.grpSalePricingProduct.Controls.Add(Me.lblColleagueDiscountText)
            Me.grpSalePricingProduct.Controls.Add(Me.numColleagueDiscountProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.lblWholesaleFormulaProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.numWholesaleMarkupProduct)
            Me.grpSalePricingProduct.Controls.Add(Me.lblWholesaleMarkupText)
            Me.grpSalePricingProduct.Controls.Add(Me.lblWholesaleDiscountText)
            Me.grpSalePricingProduct.Controls.Add(Me.numWholesaleDiscountProduct)
            Me.grpSalePricingProduct.Location = New Point(10, 185)
            Me.grpSalePricingProduct.Name = "grpSalePricingProduct"
            Me.grpSalePricingProduct.Size = New Size(730, 142)
            Me.grpSalePricingProduct.TabIndex = 7
            Me.grpSalePricingProduct.TabStop = False
            Me.grpSalePricingProduct.Text = "قیمت فروش و تخفیفات پیش فرض درصدی برای این کالا"

            '
            'lblConsumerFormulaProduct
            '
            Me.lblConsumerFormulaProduct.Location = New Point(380, 28)
            Me.lblConsumerFormulaProduct.Name = "lblConsumerFormulaProduct"
            Me.lblConsumerFormulaProduct.Size = New Size(340, 20)
            Me.lblConsumerFormulaProduct.TabIndex = 0
            Me.lblConsumerFormulaProduct.Text = "قیمت مصرف‌کننده : قیمت خرید بر اساس روش انتخابی + "
            Me.lblConsumerFormulaProduct.TextAlign = ContentAlignment.MiddleLeft
            '
            'numConsumerMarkupProduct
            '
            Me.numConsumerMarkupProduct.DecimalPlaces = 2
            Me.numConsumerMarkupProduct.Location = New Point(310, 25)
            Me.numConsumerMarkupProduct.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numConsumerMarkupProduct.Name = "numConsumerMarkupProduct"
            Me.numConsumerMarkupProduct.Size = New Size(65, 22)
            Me.numConsumerMarkupProduct.TabIndex = 1
            Me.numConsumerMarkupProduct.TextAlign = HorizontalAlignment.Center
            '
            'lblConsumerMarkupText
            '
            Me.lblConsumerMarkupText.Location = New Point(185, 28)
            Me.lblConsumerMarkupText.Name = "lblConsumerMarkupText"
            Me.lblConsumerMarkupText.Size = New Size(125, 20)
            Me.lblConsumerMarkupText.TabIndex = 2
            Me.lblConsumerMarkupText.Text = "درصد قیمت خرید / "
            Me.lblConsumerMarkupText.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblConsumerDiscountText
            '
            Me.lblConsumerDiscountText.Location = New Point(105, 28)
            Me.lblConsumerDiscountText.Name = "lblConsumerDiscountText"
            Me.lblConsumerDiscountText.Size = New Size(75, 20)
            Me.lblConsumerDiscountText.TabIndex = 3
            Me.lblConsumerDiscountText.Text = "درصد تخفیف"
            Me.lblConsumerDiscountText.TextAlign = ContentAlignment.MiddleLeft
            '
            'numConsumerDiscountProduct
            '
            Me.numConsumerDiscountProduct.DecimalPlaces = 2
            Me.numConsumerDiscountProduct.Location = New Point(35, 25)
            Me.numConsumerDiscountProduct.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
            Me.numConsumerDiscountProduct.Name = "numConsumerDiscountProduct"
            Me.numConsumerDiscountProduct.Size = New Size(65, 22)
            Me.numConsumerDiscountProduct.TabIndex = 4
            Me.numConsumerDiscountProduct.TextAlign = HorizontalAlignment.Center

            '
            'lblColleagueFormulaProduct
            '
            Me.lblColleagueFormulaProduct.Location = New Point(380, 65)
            Me.lblColleagueFormulaProduct.Name = "lblColleagueFormulaProduct"
            Me.lblColleagueFormulaProduct.Size = New Size(340, 20)
            Me.lblColleagueFormulaProduct.TabIndex = 5
            Me.lblColleagueFormulaProduct.Text = "قیمت همکار : قیمت خرید بر اساس روش انتخابی + "
            Me.lblColleagueFormulaProduct.TextAlign = ContentAlignment.MiddleLeft
            '
            'numColleagueMarkupProduct
            '
            Me.numColleagueMarkupProduct.DecimalPlaces = 2
            Me.numColleagueMarkupProduct.Location = New Point(310, 62)
            Me.numColleagueMarkupProduct.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numColleagueMarkupProduct.Name = "numColleagueMarkupProduct"
            Me.numColleagueMarkupProduct.Size = New Size(65, 22)
            Me.numColleagueMarkupProduct.TabIndex = 6
            Me.numColleagueMarkupProduct.TextAlign = HorizontalAlignment.Center
            '
            'lblColleagueMarkupText
            '
            Me.lblColleagueMarkupText.Location = New Point(185, 65)
            Me.lblColleagueMarkupText.Name = "lblColleagueMarkupText"
            Me.lblColleagueMarkupText.Size = New Size(125, 20)
            Me.lblColleagueMarkupText.TabIndex = 7
            Me.lblColleagueMarkupText.Text = "درصد قیمت خرید / "
            Me.lblColleagueMarkupText.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblColleagueDiscountText
            '
            Me.lblColleagueDiscountText.Location = New Point(105, 65)
            Me.lblColleagueDiscountText.Name = "lblColleagueDiscountText"
            Me.lblColleagueDiscountText.Size = New Size(75, 20)
            Me.lblColleagueDiscountText.TabIndex = 8
            Me.lblColleagueDiscountText.Text = "درصد تخفیف"
            Me.lblColleagueDiscountText.TextAlign = ContentAlignment.MiddleLeft
            '
            'numColleagueDiscountProduct
            '
            Me.numColleagueDiscountProduct.DecimalPlaces = 2
            Me.numColleagueDiscountProduct.Location = New Point(35, 62)
            Me.numColleagueDiscountProduct.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
            Me.numColleagueDiscountProduct.Name = "numColleagueDiscountProduct"
            Me.numColleagueDiscountProduct.Size = New Size(65, 22)
            Me.numColleagueDiscountProduct.TabIndex = 9
            Me.numColleagueDiscountProduct.TextAlign = HorizontalAlignment.Center

            '
            'lblWholesaleFormulaProduct
            '
            Me.lblWholesaleFormulaProduct.Location = New Point(380, 102)
            Me.lblWholesaleFormulaProduct.Name = "lblWholesaleFormulaProduct"
            Me.lblWholesaleFormulaProduct.Size = New Size(340, 20)
            Me.lblWholesaleFormulaProduct.TabIndex = 10
            Me.lblWholesaleFormulaProduct.Text = "قیمت عمده‌فروشی : قیمت خرید بر اساس روش انتخابی + "
            Me.lblWholesaleFormulaProduct.TextAlign = ContentAlignment.MiddleLeft
            '
            'numWholesaleMarkupProduct
            '
            Me.numWholesaleMarkupProduct.DecimalPlaces = 2
            Me.numWholesaleMarkupProduct.Location = New Point(310, 99)
            Me.numWholesaleMarkupProduct.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numWholesaleMarkupProduct.Name = "numWholesaleMarkupProduct"
            Me.numWholesaleMarkupProduct.Size = New Size(65, 22)
            Me.numWholesaleMarkupProduct.TabIndex = 11
            Me.numWholesaleMarkupProduct.TextAlign = HorizontalAlignment.Center
            '
            'lblWholesaleMarkupText
            '
            Me.lblWholesaleMarkupText.Location = New Point(185, 102)
            Me.lblWholesaleMarkupText.Name = "lblWholesaleMarkupText"
            Me.lblWholesaleMarkupText.Size = New Size(125, 20)
            Me.lblWholesaleMarkupText.TabIndex = 12
            Me.lblWholesaleMarkupText.Text = "درصد قیمت خرید / "
            Me.lblWholesaleMarkupText.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblWholesaleDiscountText
            '
            Me.lblWholesaleDiscountText.Location = New Point(105, 102)
            Me.lblWholesaleDiscountText.Name = "lblWholesaleDiscountText"
            Me.lblWholesaleDiscountText.Size = New Size(75, 20)
            Me.lblWholesaleDiscountText.TabIndex = 13
            Me.lblWholesaleDiscountText.Text = "درصد تخفیف"
            Me.lblWholesaleDiscountText.TextAlign = ContentAlignment.MiddleLeft
            '
            'numWholesaleDiscountProduct
            '
            Me.numWholesaleDiscountProduct.DecimalPlaces = 2
            Me.numWholesaleDiscountProduct.Location = New Point(35, 99)
            Me.numWholesaleDiscountProduct.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
            Me.numWholesaleDiscountProduct.Name = "numWholesaleDiscountProduct"
            Me.numWholesaleDiscountProduct.Size = New Size(65, 22)
            Me.numWholesaleDiscountProduct.TabIndex = 14
            Me.numWholesaleDiscountProduct.TextAlign = HorizontalAlignment.Center

            '
            'lblTaxPercent
            '
            Me.lblTaxPercent.Location = New Point(580, 343)
            Me.lblTaxPercent.Name = "lblTaxPercent"
            Me.lblTaxPercent.Size = New Size(160, 20)
            Me.lblTaxPercent.TabIndex = 8
            Me.lblTaxPercent.Text = "درصد مالیات و عوارض:"
            Me.lblTaxPercent.TextAlign = ContentAlignment.MiddleLeft
            '
            'numTaxPercent
            '
            Me.numTaxPercent.DecimalPlaces = 2
            Me.numTaxPercent.Location = New Point(450, 340)
            Me.numTaxPercent.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
            Me.numTaxPercent.Name = "numTaxPercent"
            Me.numTaxPercent.Size = New Size(120, 22)
            Me.numTaxPercent.TabIndex = 9
            Me.numTaxPercent.TextAlign = HorizontalAlignment.Center

            '
            'lblTollPercent
            '
            Me.lblTollPercent.Location = New Point(580, 378)
            Me.lblTollPercent.Name = "lblTollPercent"
            Me.lblTollPercent.Size = New Size(160, 20)
            Me.lblTollPercent.TabIndex = 10
            Me.lblTollPercent.Text = "درصد عوارض:"
            Me.lblTollPercent.TextAlign = ContentAlignment.MiddleLeft
            Me.lblTollPercent.Visible = False
            '
            'numTollPercent
            '
            Me.numTollPercent.DecimalPlaces = 2
            Me.numTollPercent.Location = New Point(450, 375)
            Me.numTollPercent.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
            Me.numTollPercent.Name = "numTollPercent"
            Me.numTollPercent.Size = New Size(120, 22)
            Me.numTollPercent.TabIndex = 11
            Me.numTollPercent.TextAlign = HorizontalAlignment.Center
            Me.numTollPercent.Visible = False

            '
            'tabInventory
            '
            Me.tabInventory.Controls.Add(Me.lblMinStock)
            Me.tabInventory.Controls.Add(Me.numMinStock)
            Me.tabInventory.Controls.Add(Me.lblReorderPoint)
            Me.tabInventory.Controls.Add(Me.numReorderPoint)
            Me.tabInventory.Controls.Add(Me.lblMaxStock)
            Me.tabInventory.Controls.Add(Me.numMaxStock)
            Me.tabInventory.Controls.Add(Me.lblTrackingType)
            Me.tabInventory.Controls.Add(Me.cmbTrackingType)
            Me.tabInventory.Controls.Add(Me.lblDefaultWarehouseTitle)
            Me.tabInventory.Controls.Add(Me.btnSelectDefaultWarehouse)
            Me.tabInventory.Controls.Add(Me.lblDefaultWarehouseName)
            Me.tabInventory.Controls.Add(Me.lblPhysicalLocation)
            Me.tabInventory.Controls.Add(Me.btnSelectLocation)
            Me.tabInventory.Controls.Add(Me.lblPhysicalLocationName)
            Me.tabInventory.Controls.Add(Me.lblPhysicalLocationCode)
            Me.tabInventory.Controls.Add(Me.chkActive)
            Me.tabInventory.Location = New Point(4, 23)
            Me.tabInventory.Name = "tabInventory"
            Me.tabInventory.Padding = New Padding(10)
            Me.tabInventory.Size = New Size(752, 423)
            Me.tabInventory.TabIndex = 3
            Me.tabInventory.Text = "کنترل موجودی و انبار"
            Me.tabInventory.UseVisualStyleBackColor = True

            '
            'lblMinStock
            '
            Me.lblMinStock.Location = New Point(600, 15)
            Me.lblMinStock.Name = "lblMinStock"
            Me.lblMinStock.Size = New Size(140, 20)
            Me.lblMinStock.Text = "حداقل موجودی:"
            Me.lblMinStock.TextAlign = ContentAlignment.MiddleLeft
            '
            'numMinStock
            '
            Me.numMinStock.DecimalPlaces = 2
            Me.numMinStock.Location = New Point(470, 12)
            Me.numMinStock.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
            Me.numMinStock.Name = "numMinStock"
            Me.numMinStock.Size = New Size(120, 22)
            Me.numMinStock.TabIndex = 0
            Me.numMinStock.TextAlign = HorizontalAlignment.Center

            '
            'lblReorderPoint
            '
            Me.lblReorderPoint.Location = New Point(600, 50)
            Me.lblReorderPoint.Name = "lblReorderPoint"
            Me.lblReorderPoint.Size = New Size(140, 20)
            Me.lblReorderPoint.Text = "نقطه سفارش:"
            Me.lblReorderPoint.TextAlign = ContentAlignment.MiddleLeft
            '
            'numReorderPoint
            '
            Me.numReorderPoint.DecimalPlaces = 2
            Me.numReorderPoint.Location = New Point(470, 47)
            Me.numReorderPoint.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
            Me.numReorderPoint.Name = "numReorderPoint"
            Me.numReorderPoint.Size = New Size(120, 22)
            Me.numReorderPoint.TabIndex = 1
            Me.numReorderPoint.TextAlign = HorizontalAlignment.Center

            '
            'lblMaxStock
            '
            Me.lblMaxStock.Location = New Point(600, 85)
            Me.lblMaxStock.Name = "lblMaxStock"
            Me.lblMaxStock.Size = New Size(140, 20)
            Me.lblMaxStock.Text = "حداکثر موجودی:"
            Me.lblMaxStock.TextAlign = ContentAlignment.MiddleLeft
            '
            'numMaxStock
            '
            Me.numMaxStock.DecimalPlaces = 2
            Me.numMaxStock.Location = New Point(470, 82)
            Me.numMaxStock.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
            Me.numMaxStock.Name = "numMaxStock"
            Me.numMaxStock.Size = New Size(120, 22)
            Me.numMaxStock.TabIndex = 2
            Me.numMaxStock.TextAlign = HorizontalAlignment.Center

            '
            'lblTrackingType
            '
            Me.lblTrackingType.Location = New Point(600, 120)
            Me.lblTrackingType.Name = "lblTrackingType"
            Me.lblTrackingType.Size = New Size(140, 20)
            Me.lblTrackingType.Text = "نحوه ردیابی کالا:"
            Me.lblTrackingType.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbTrackingType
            '
            Me.cmbTrackingType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbTrackingType.FormattingEnabled = True
            Me.cmbTrackingType.Items.AddRange(New Object() {"عادی", "سریال‌دار", "سری ساخت (بچ)", "تاریخ انقضا"})
            Me.cmbTrackingType.Location = New Point(20, 117)
            Me.cmbTrackingType.Name = "cmbTrackingType"
            Me.cmbTrackingType.Size = New Size(570, 22)
            Me.cmbTrackingType.TabIndex = 3

            '
            'lblDefaultWarehouseTitle
            '
            Me.lblDefaultWarehouseTitle.Location = New Point(600, 155)
            Me.lblDefaultWarehouseTitle.Name = "lblDefaultWarehouseTitle"
            Me.lblDefaultWarehouseTitle.Size = New Size(140, 20)
            Me.lblDefaultWarehouseTitle.Text = "نام انبار پیش فرض:"
            Me.lblDefaultWarehouseTitle.TextAlign = ContentAlignment.MiddleLeft
            '
            'btnSelectDefaultWarehouse
            '
            Me.btnSelectDefaultWarehouse.Location = New Point(555, 149)
            Me.btnSelectDefaultWarehouse.Name = "btnSelectDefaultWarehouse"
            Me.btnSelectDefaultWarehouse.Size = New Size(35, 26)
            Me.btnSelectDefaultWarehouse.TabIndex = 4
            Me.btnSelectDefaultWarehouse.Text = "..."
            Me.btnSelectDefaultWarehouse.UseVisualStyleBackColor = True
            '
            'lblDefaultWarehouseName
            '
            Me.lblDefaultWarehouseName.Location = New Point(20, 149)
            Me.lblDefaultWarehouseName.Name = "lblDefaultWarehouseName"
            Me.lblDefaultWarehouseName.Size = New Size(525, 22)
            Me.lblDefaultWarehouseName.BorderStyle = BorderStyle.FixedSingle
            Me.lblDefaultWarehouseName.TextAlign = ContentAlignment.MiddleLeft

            '
            'lblPhysicalLocation
            '
            Me.lblPhysicalLocation.Location = New Point(600, 190)
            Me.lblPhysicalLocation.Name = "lblPhysicalLocation"
            Me.lblPhysicalLocation.Size = New Size(140, 20)
            Me.lblPhysicalLocation.Text = "محل فیزیکی در انبار:"
            Me.lblPhysicalLocation.TextAlign = ContentAlignment.MiddleLeft
            '
            'btnSelectLocation
            '
            Me.btnSelectLocation.Location = New Point(555, 184)
            Me.btnSelectLocation.Name = "btnSelectLocation"
            Me.btnSelectLocation.Size = New Size(35, 26)
            Me.btnSelectLocation.TabIndex = 5
            Me.btnSelectLocation.Text = "..."
            Me.btnSelectLocation.UseVisualStyleBackColor = True
            '
            'lblPhysicalLocationName
            '
            Me.lblPhysicalLocationName.Location = New Point(20, 184)
            Me.lblPhysicalLocationName.Name = "lblPhysicalLocationName"
            Me.lblPhysicalLocationName.Size = New Size(525, 22)
            Me.lblPhysicalLocationName.BorderStyle = BorderStyle.FixedSingle
            Me.lblPhysicalLocationName.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblPhysicalLocationCode
            '
            Me.lblPhysicalLocationCode.Location = New Point(20, 210)
            Me.lblPhysicalLocationCode.Name = "lblPhysicalLocationCode"
            Me.lblPhysicalLocationCode.Size = New Size(525, 22)
            Me.lblPhysicalLocationCode.BorderStyle = BorderStyle.FixedSingle
            Me.lblPhysicalLocationCode.TextAlign = ContentAlignment.MiddleLeft

            '
            'chkActive
            '
            Me.chkActive.AutoSize = True
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = CheckState.Checked
            Me.chkActive.Location = New Point(470, 245)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New Size(120, 18)
            Me.chkActive.TabIndex = 6
            Me.chkActive.Text = "کالا فعال است"
            Me.chkActive.UseVisualStyleBackColor = True

            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(30, 120, 60)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(410, 468)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(110, 30)
            Me.btnSave.TabIndex = 1
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False

            '
            'btnCancel
            '
            Me.btnCancel.BackColor = Color.FromArgb(120, 120, 120)
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(280, 468)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(110, 30)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False

            '
            'AnbardaryNamKala2Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(780, 510)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.tabsProduct)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbardaryNamKala2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش اطلاعات کالا و خدمات"
            Me.tabsProduct.ResumeLayout(False)
            Me.tabGeneral.ResumeLayout(False)
            Me.tabGeneral.PerformLayout()
            Me.tabPhysical.ResumeLayout(False)
            Me.tabPhysical.PerformLayout()
            Me.grpProductImages.ResumeLayout(False)
            CType(Me.picImage1, ISupportInitialize).EndInit()
            CType(Me.picImage2, ISupportInitialize).EndInit()
            CType(Me.picImage3, ISupportInitialize).EndInit()
            CType(Me.picImage4, ISupportInitialize).EndInit()
            CType(Me.picImage5, ISupportInitialize).EndInit()
            CType(Me.picImage6, ISupportInitialize).EndInit()
            Me.tabPricing.ResumeLayout(False)
            Me.tabPricing.PerformLayout()
            Me.grpSalePricingProduct.ResumeLayout(False)
            Me.grpSalePricingProduct.PerformLayout()
            Me.tabInventory.ResumeLayout(False)
            Me.tabInventory.PerformLayout()
            CType(Me.numNetWeight, ISupportInitialize).EndInit()
            CType(Me.numGrossWeight, ISupportInitialize).EndInit()
            CType(Me.numLength, ISupportInitialize).EndInit()
            CType(Me.numWidth, ISupportInitialize).EndInit()
            CType(Me.numHeight, ISupportInitialize).EndInit()
            CType(Me.numVolume, ISupportInitialize).EndInit()
            CType(Me.numNominalFactor, ISupportInitialize).EndInit()
            CType(Me.numConsumerMarkupProduct, ISupportInitialize).EndInit()
            CType(Me.numConsumerDiscountProduct, ISupportInitialize).EndInit()
            CType(Me.numColleagueMarkupProduct, ISupportInitialize).EndInit()
            CType(Me.numColleagueDiscountProduct, ISupportInitialize).EndInit()
            CType(Me.numWholesaleMarkupProduct, ISupportInitialize).EndInit()
            CType(Me.numWholesaleDiscountProduct, ISupportInitialize).EndInit()
            CType(Me.numTaxPercent, ISupportInitialize).EndInit()
            CType(Me.numTollPercent, ISupportInitialize).EndInit()
            CType(Me.numMinStock, ISupportInitialize).EndInit()
            CType(Me.numReorderPoint, ISupportInitialize).EndInit()
            CType(Me.numMaxStock, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
