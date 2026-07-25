Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.IO
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryNamKala2Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private ReadOnly _uomService As New UnitOfMeasureService()
        Private ReadOnly _settingsService As New SettingsService()
        Private ReadOnly _productId As Integer?
        Private _productImagePaths As String() = New String(5) {}
        Private _locationId As Integer?

        Private _selectedId As Integer? = Nothing
        Private _selectedGroupId As Integer = 0
        Private _selectedBaseUomId As Integer = 0
        Private _selectedBaseUomName As String = ""
        Private _selectedSecondaryUomId As Integer = 0
        Private _selectedSecondaryUomName As String = ""
        Private _selectedDefaultWarehouseId As Integer? = Nothing

        ' Product Images (Slots 1 to 6)
        Private _imagePaths(6) As String
        Private _imageChanged(6) As Boolean

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(productId As Integer)
            InitializeComponent()
            _selectedId = productId
        End Sub

        Private Sub AnbardaryNamKala2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)

            ' Select defaults
            If cmbProductType.Items.Count > 0 Then cmbProductType.SelectedIndex = 0
            If cmbTrackingType.Items.Count > 0 Then cmbTrackingType.SelectedIndex = 0
            txtGroup.Text = "--- بدون گروه ---"
            txtBaseUoM.Text = "--- بدون واحد ---"
            txtSecondaryUoM.Text = "--- بدون واحد ---"

            ' Update dynamic formula labels based on PurchasePricingMethod
            Dim purchaseMethod = _settingsService.GetSettingValue("PurchasePricingMethod", "روش FIFO")
            lblConsumerFormulaProduct.Text = "قیمت مصرف‌کننده : قیمت خرید بر اساس " & purchaseMethod & " + "
            lblColleagueFormulaProduct.Text = "قیمت همکار : قیمت خرید بر اساس " & purchaseMethod & " + "
            lblWholesaleFormulaProduct.Text = "قیمت عمده‌فروشی : قیمت خرید بر اساس " & purchaseMethod & " + "

            If _selectedId.HasValue Then
                LoadProductData(_selectedId.Value)
            Else
                ' Load default markups from AppSettings for new products
                Dim legacyMarkup = _settingsService.GetSettingValue("SaleMarkupPercent", "0")
                
                Dim consumerDefault As Decimal = 0D
                Decimal.TryParse(_settingsService.GetSettingValue("SaleMarkupPercent_Consumer", legacyMarkup), consumerDefault)
                numConsumerMarkupProduct.Value = Math.Min(numConsumerMarkupProduct.Maximum, Math.Max(0D, consumerDefault))

                Dim colleagueDefault As Decimal = 0D
                Decimal.TryParse(_settingsService.GetSettingValue("SaleMarkupPercent_Colleague", "0"), colleagueDefault)
                numColleagueMarkupProduct.Value = Math.Min(numColleagueMarkupProduct.Maximum, Math.Max(0D, colleagueDefault))

                Dim wholesaleDefault As Decimal = 0D
                Decimal.TryParse(_settingsService.GetSettingValue("SaleMarkupPercent_Wholesale", "0"), wholesaleDefault)
                numWholesaleMarkupProduct.Value = Math.Min(numWholesaleMarkupProduct.Maximum, Math.Max(0D, wholesaleDefault))

                numConsumerDiscountProduct.Value = 0D
                numColleagueDiscountProduct.Value = 0D
                numWholesaleDiscountProduct.Value = 0D
                numTaxPercent.Value = 0D
                numTollPercent.Value = 0D

                ' Physical specs defaults
                numNetWeight.Value = 0D
                numGrossWeight.Value = 0D
                numLength.Value = 0D
                numWidth.Value = 0D
                numHeight.Value = 0D
                numVolume.Value = 0D
                txtColor.Text = ""
                txtMaterial.Text = ""
                txtSize.Text = ""
                txtBrand.Text = ""
                txtCountryOfOrigin.Text = ""
                txtPhysicalDescription.Text = ""

                ' Image slots reset
                For i As Integer = 1 To 6
                    _imagePaths(i) = ""
                    _imageChanged(i) = False
                Next

                ' Suggest the next product code automatically
                SuggestNextCode()
            End If
        End Sub

        Private Sub SuggestNextCode()
            Try
                txtCode.Text = _service.GetNextProductCode()
            Catch ex As Exception
                txtCode.Text = "1001"
            End Try
        End Sub

        Private Sub LoadProductData(productId As Integer)
            Try
                Dim row = _service.GetProductById(productId)
                If row IsNot Nothing Then
                    txtCode.Text = Convert.ToString(row("ProductCode"))
                    txtName.Text = Convert.ToString(row("ProductName"))
                    txtTechnicalName.Text = Convert.ToString(row("TechnicalName"))
                    txtBarcode.Text = Convert.ToString(row("Barcode"))
                    txtTaxID.Text = Convert.ToString(row("TaxID"))
                    txtPurchasePrice.Text = Convert.ToString(row("PurchasePrice"))
                    txtPrice.Text = Convert.ToString(row("DefaultPrice"))
                    If Not row.IsNull("LocationID") Then
                        _locationId = Convert.ToInt32(row("LocationID"))
                        Dim paths = _service.GetLocationPath(_locationId.Value)
                        lblPhysicalLocationName.Text = paths.Item1
                        lblPhysicalLocationCode.Text = paths.Item2
                    End If
                    chkActive.Checked = Convert.ToBoolean(row("IsActive"))

                    ' Load ProductGroup
                    Dim pgIdVal = row("ProductGroupID")
                    If pgIdVal IsNot Nothing AndAlso pgIdVal IsNot DBNull.Value Then
                        _selectedGroupId = Convert.ToInt32(pgIdVal)
                        Dim pgRow = New ProductGroupService().GetById(_selectedGroupId)
                        If pgRow IsNot Nothing Then
                            txtGroup.Text = $"{Convert.ToString(pgRow("GroupCode"))} - {Convert.ToString(pgRow("GroupName"))}"
                        Else
                            txtGroup.Text = "--- بدون گروه ---"
                            _selectedGroupId = 0
                        End If
                    Else
                        _selectedGroupId = 0
                        txtGroup.Text = "--- بدون گروه ---"
                    End If

                    ' Select ProductType
                    Dim pTypeVal = Convert.ToString(row("ProductType"))
                    If Not String.IsNullOrEmpty(pTypeVal) Then
                        If pTypeVal = "دارایی ثابت" Then pTypeVal = "دارایی شرکت"
                        cmbProductType.SelectedItem = pTypeVal
                    End If

                    ' Load BaseUoM
                    Dim bUomVal = row("BaseUoMID")
                    If bUomVal IsNot Nothing AndAlso bUomVal IsNot DBNull.Value Then
                        _selectedBaseUomId = Convert.ToInt32(bUomVal)
                        Dim uRow = _uomService.GetById(_selectedBaseUomId)
                        If uRow IsNot Nothing Then
                            _selectedBaseUomName = Convert.ToString(uRow("UoMName"))
                            txtBaseUoM.Text = $"{_selectedBaseUomName}"
                        Else
                            _selectedBaseUomId = 0
                            _selectedBaseUomName = ""
                            txtBaseUoM.Text = "--- بدون واحد ---"
                        End If
                    Else
                        _selectedBaseUomId = 0
                        _selectedBaseUomName = ""
                        txtBaseUoM.Text = "--- بدون واحد ---"
                    End If

                    ' Load SecondaryUoM
                    Dim sUomVal = row("SecondaryUoMID")
                    If sUomVal IsNot Nothing AndAlso sUomVal IsNot DBNull.Value Then
                        _selectedSecondaryUomId = Convert.ToInt32(sUomVal)
                        Dim uRow = _uomService.GetById(_selectedSecondaryUomId)
                        If uRow IsNot Nothing Then
                            _selectedSecondaryUomName = Convert.ToString(uRow("UoMName"))
                            txtSecondaryUoM.Text = $"{_selectedSecondaryUomName}"
                        Else
                            _selectedSecondaryUomId = 0
                            _selectedSecondaryUomName = ""
                            txtSecondaryUoM.Text = "--- بدون واحد ---"
                        End If
                    Else
                        _selectedSecondaryUomId = 0
                        _selectedSecondaryUomName = ""
                        txtSecondaryUoM.Text = "--- بدون واحد ---"
                    End If

                    ' Physical and Appearance attributes
                    numNetWeight.Value = If(row.Table.Columns.Contains("NetWeight") AndAlso Not row.IsNull("NetWeight"), Convert.ToDecimal(row("NetWeight")), 0D)
                    numGrossWeight.Value = If(row.Table.Columns.Contains("GrossWeight") AndAlso Not row.IsNull("GrossWeight"), Convert.ToDecimal(row("GrossWeight")), 0D)
                    numLength.Value = If(row.Table.Columns.Contains("Length") AndAlso Not row.IsNull("Length"), Convert.ToDecimal(row("Length")), 0D)
                    numWidth.Value = If(row.Table.Columns.Contains("Width") AndAlso Not row.IsNull("Width"), Convert.ToDecimal(row("Width")), 0D)
                    numHeight.Value = If(row.Table.Columns.Contains("Height") AndAlso Not row.IsNull("Height"), Convert.ToDecimal(row("Height")), 0D)
                    numVolume.Value = If(row.Table.Columns.Contains("Volume") AndAlso Not row.IsNull("Volume"), Convert.ToDecimal(row("Volume")), 0D)
                    txtColor.Text = If(row.Table.Columns.Contains("Color") AndAlso Not row.IsNull("Color"), Convert.ToString(row("Color")), "")
                    txtMaterial.Text = If(row.Table.Columns.Contains("Material") AndAlso Not row.IsNull("Material"), Convert.ToString(row("Material")), "")
                    txtSize.Text = If(row.Table.Columns.Contains("Size") AndAlso Not row.IsNull("Size"), Convert.ToString(row("Size")), "")
                    txtBrand.Text = If(row.Table.Columns.Contains("Brand") AndAlso Not row.IsNull("Brand"), Convert.ToString(row("Brand")), "")
                    txtCountryOfOrigin.Text = If(row.Table.Columns.Contains("CountryOfOrigin") AndAlso Not row.IsNull("CountryOfOrigin"), Convert.ToString(row("CountryOfOrigin")), "")
                    txtPhysicalDescription.Text = If(row.Table.Columns.Contains("PhysicalDescription") AndAlso Not row.IsNull("PhysicalDescription"), Convert.ToString(row("PhysicalDescription")), "")

                    ' Product Images (Slots 1 to 6)
                    LoadProductImageSlot(row, 1, picImage1)
                    LoadProductImageSlot(row, 2, picImage2)
                    LoadProductImageSlot(row, 3, picImage3)
                    LoadProductImageSlot(row, 4, picImage4)
                    LoadProductImageSlot(row, 5, picImage5)
                    LoadProductImageSlot(row, 6, picImage6)

                    ' Markups and Discounts
                    numConsumerMarkupProduct.Value = If(row.Table.Columns.Contains("ConsumerMarkup") AndAlso Not row.IsNull("ConsumerMarkup"), Convert.ToDecimal(row("ConsumerMarkup")), 0D)
                    numConsumerDiscountProduct.Value = If(row.Table.Columns.Contains("ConsumerDiscount") AndAlso Not row.IsNull("ConsumerDiscount"), Convert.ToDecimal(row("ConsumerDiscount")), 0D)
                    numColleagueMarkupProduct.Value = If(row.Table.Columns.Contains("ColleagueMarkup") AndAlso Not row.IsNull("ColleagueMarkup"), Convert.ToDecimal(row("ColleagueMarkup")), 0D)
                    numColleagueDiscountProduct.Value = If(row.Table.Columns.Contains("ColleagueDiscount") AndAlso Not row.IsNull("ColleagueDiscount"), Convert.ToDecimal(row("ColleagueDiscount")), 0D)
                    numWholesaleMarkupProduct.Value = If(row.Table.Columns.Contains("WholesaleMarkup") AndAlso Not row.IsNull("WholesaleMarkup"), Convert.ToDecimal(row("WholesaleMarkup")), 0D)
                    numWholesaleDiscountProduct.Value = If(row.Table.Columns.Contains("WholesaleDiscount") AndAlso Not row.IsNull("WholesaleDiscount"), Convert.ToDecimal(row("WholesaleDiscount")), 0D)

                    ' Tax and Toll
                    numTaxPercent.Value = If(row.Table.Columns.Contains("TaxPercent") AndAlso Not row.IsNull("TaxPercent"), Convert.ToDecimal(row("TaxPercent")), 0D)
                    numTollPercent.Value = If(row.Table.Columns.Contains("TollPercent") AndAlso Not row.IsNull("TollPercent"), Convert.ToDecimal(row("TollPercent")), 0D)

                    ' Numeric fields
                    numNominalFactor.Value = If(row.IsNull("NominalFactor"), 0D, Convert.ToDecimal(row("NominalFactor")))
                    numMinStock.Value = If(row.IsNull("MinStock"), 0D, Convert.ToDecimal(row("MinStock")))
                    numReorderPoint.Value = If(row.IsNull("ReorderPoint"), 0D, Convert.ToDecimal(row("ReorderPoint")))
                    numMaxStock.Value = If(row.IsNull("MaxStock"), 0D, Convert.ToDecimal(row("MaxStock")))

                    ' Default Warehouse
                    If row.Table.Columns.Contains("DefaultWarehouseID") AndAlso Not row.IsNull("DefaultWarehouseID") Then
                        _selectedDefaultWarehouseId = Convert.ToInt32(row("DefaultWarehouseID"))
                        Dim wRow = _service.GetWarehouseById(_selectedDefaultWarehouseId.Value)
                        If wRow IsNot Nothing Then
                            lblDefaultWarehouseName.Text = Convert.ToString(wRow("WarehouseName"))
                        Else
                            lblDefaultWarehouseName.Text = ""
                        End If
                    Else
                        _selectedDefaultWarehouseId = Nothing
                        lblDefaultWarehouseName.Text = ""
                    End If

                    ' Tracking type
                    Dim trackingVal = Convert.ToString(row("TrackingType"))
                    If Not String.IsNullOrEmpty(trackingVal) Then
                        cmbTrackingType.SelectedItem = trackingVal
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات کالا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadProductImageSlot(row As DataRow, slot As Integer, picBox As PictureBox)
            _imageChanged(slot) = False
            Dim colName = "Image" & slot
            If row.Table.Columns.Contains(colName) AndAlso Not row.IsNull(colName) Then
                Dim relPath = Convert.ToString(row(colName)).Trim()
                _imagePaths(slot) = relPath
                If Not String.IsNullOrEmpty(relPath) Then
                    Dim fullPath = If(Path.IsPathRooted(relPath), relPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relPath))
                    LoadImageToBox(picBox, fullPath)
                    Return
                End If
            End If
            _imagePaths(slot) = ""
            LoadImageToBox(picBox, "")
        End Sub

        Private Sub LoadImageToBox(picBox As PictureBox, filePath As String)
            Try
                If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
                    Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                        Using img = Image.FromStream(fs)
                            picBox.Image = New Bitmap(img)
                        End Using
                    End Using
                Else
                    picBox.Image = Nothing
                End If
            Catch ex As Exception
                picBox.Image = Nothing
            End Try
        End Sub

        ' Image Browse / Remove Handlers
        Private Sub BrowseImage(slot As Integer, picBox As PictureBox)
            Try
                Using ofd As New OpenFileDialog()
                    ofd.Filter = "فایل‌های تصویری (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
                    ofd.Title = $"انتخاب تصویر کالا - شماره {slot}"
                    If ofd.ShowDialog() = DialogResult.OK Then
                        LoadImageToBox(picBox, ofd.FileName)
                        _imagePaths(slot) = ofd.FileName
                        _imageChanged(slot) = True
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در انتخاب تصویر: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub RemoveImage(slot As Integer, picBox As PictureBox)
            picBox.Image = Nothing
            _imagePaths(slot) = ""
            _imageChanged(slot) = True
        End Sub

        Private Sub BtnBrowseImage1_Click(sender As Object, e As EventArgs) Handles btnBrowseImage1.Click
            BrowseImage(1, picImage1)
        End Sub

        Private Sub BtnRemoveImage1_Click(sender As Object, e As EventArgs) Handles btnRemoveImage1.Click
            RemoveImage(1, picImage1)
        End Sub

        Private Sub BtnBrowseImage2_Click(sender As Object, e As EventArgs) Handles btnBrowseImage2.Click
            BrowseImage(2, picImage2)
        End Sub

        Private Sub BtnRemoveImage2_Click(sender As Object, e As EventArgs) Handles btnRemoveImage2.Click
            RemoveImage(2, picImage2)
        End Sub

        Private Sub BtnBrowseImage3_Click(sender As Object, e As EventArgs) Handles btnBrowseImage3.Click
            BrowseImage(3, picImage3)
        End Sub

        Private Sub BtnRemoveImage3_Click(sender As Object, e As EventArgs) Handles btnRemoveImage3.Click
            RemoveImage(3, picImage3)
        End Sub

        Private Sub BtnBrowseImage4_Click(sender As Object, e As EventArgs) Handles btnBrowseImage4.Click
            BrowseImage(4, picImage4)
        End Sub

        Private Sub BtnRemoveImage4_Click(sender As Object, e As EventArgs) Handles btnRemoveImage4.Click
            RemoveImage(4, picImage4)
        End Sub

        Private Sub BtnBrowseImage5_Click(sender As Object, e As EventArgs) Handles btnBrowseImage5.Click
            BrowseImage(5, picImage5)
        End Sub

        Private Sub BtnRemoveImage5_Click(sender As Object, e As EventArgs) Handles btnRemoveImage5.Click
            RemoveImage(5, picImage5)
        End Sub

        Private Sub BtnBrowseImage6_Click(sender As Object, e As EventArgs) Handles btnBrowseImage6.Click
            BrowseImage(6, picImage6)
        End Sub

        Private Sub BtnRemoveImage6_Click(sender As Object, e As EventArgs) Handles btnRemoveImage6.Click
            RemoveImage(6, picImage6)
        End Sub

        Private Sub BtnBrowseGroup_Click(sender As Object, e As EventArgs) Handles btnBrowseGroup.Click
            Try
                Using frm As New AnbardaryGoroohKala1Form(isSelectMode:=True)
                    If frm.ShowDialog() = DialogResult.OK Then
                        _selectedGroupId = frm.SelectedGroupID
                        If _selectedGroupId > 0 Then
                            txtGroup.Text = $"{frm.SelectedGroupCode} - {frm.SelectedGroupName}"
                        Else
                            txtGroup.Text = "--- بدون گروه ---"
                            _selectedGroupId = 0
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن انتخابگر گروه‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnBrowseBaseUoM_Click(sender As Object, e As EventArgs) Handles btnBrowseBaseUoM.Click
            Try
                Using frm As New AnbardaryVahedKala1Form(isSelectMode:=True)
                    If frm.ShowDialog() = DialogResult.OK Then
                        _selectedBaseUomId = frm.SelectedUoMID
                        _selectedBaseUomName = frm.SelectedUoMName
                        If _selectedBaseUomId > 0 Then
                            txtBaseUoM.Text = $"{frm.SelectedUoMName} ({frm.SelectedCategoryName})"
                        Else
                            txtBaseUoM.Text = "--- بدون واحد ---"
                            _selectedBaseUomId = 0
                            _selectedBaseUomName = ""
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن فرم انتخاب واحد اندازه‌گیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnBrowseSecondaryUoM_Click(sender As Object, e As EventArgs) Handles btnBrowseSecondaryUoM.Click
            Try
                Using frm As New AnbardaryVahedKala1Form(isSelectMode:=True)
                    If frm.ShowDialog() = DialogResult.OK Then
                        _selectedSecondaryUomId = frm.SelectedUoMID
                        _selectedSecondaryUomName = frm.SelectedUoMName
                        If _selectedSecondaryUomId > 0 Then
                            txtSecondaryUoM.Text = $"{frm.SelectedUoMName} ({frm.SelectedCategoryName})"
                        Else
                            txtSecondaryUoM.Text = "--- بدون واحد ---"
                            _selectedSecondaryUomId = 0
                            _selectedSecondaryUomName = ""
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن فرم انتخاب واحد اندازه‌گیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("نام کالا الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If

            Try
                Dim purchasePrice As Decimal = 0
                Decimal.TryParse(txtPurchasePrice.Text.Trim(), purchasePrice)

                Dim salesPrice As Decimal = 0
                Decimal.TryParse(txtPrice.Text.Trim(), salesPrice)

                ' ProductGroup
                Dim productGroupId As Object = DBNull.Value
                Dim categoryName = ""
                If _selectedGroupId > 0 Then
                    productGroupId = _selectedGroupId
                    Dim idx = txtGroup.Text.IndexOf(" - ")
                    If idx >= 0 Then
                        categoryName = txtGroup.Text.Substring(idx + 3)
                    Else
                        categoryName = txtGroup.Text
                    End If
                End If

                ' Base UoM
                Dim baseUomId As Object = DBNull.Value
                Dim baseUnitName = ""
                If _selectedBaseUomId > 0 Then
                    baseUomId = _selectedBaseUomId
                    baseUnitName = _selectedBaseUomName
                End If

                ' Secondary UoM
                Dim secondaryUomId As Object = DBNull.Value
                If _selectedSecondaryUomId > 0 Then
                    secondaryUomId = _selectedSecondaryUomId
                End If

                Dim productType = If(cmbProductType.SelectedItem IsNot Nothing, cmbProductType.SelectedItem.ToString(), "کالا")
                Dim trackingType = If(cmbTrackingType.SelectedItem IsNot Nothing, cmbTrackingType.SelectedItem.ToString(), "عادی")

                ' Step 1: Initial Save to obtain product ID
                Dim savedId = _service.SaveProduct(
                    _selectedId,
                    txtCode.Text.Trim(),
                    txtName.Text.Trim(),
                    baseUnitName,
                    salesPrice,
                    categoryName,
                    chkActive.Checked,
                    baseUomId,
                    secondaryUomId,
                    numNominalFactor.Value,
                    productGroupId,
                    txtBarcode.Text.Trim(),
                    txtTaxID.Text.Trim(),
                    productType,
                    purchasePrice,
                    numMinStock.Value,
                    numReorderPoint.Value,
                    numMaxStock.Value,
                    trackingType,
                    _locationId,
                    txtTechnicalName.Text.Trim(),
                    numConsumerMarkupProduct.Value,
                    numConsumerDiscountProduct.Value,
                    numColleagueMarkupProduct.Value,
                    numColleagueDiscountProduct.Value,
                    numWholesaleMarkupProduct.Value,
                    numWholesaleDiscountProduct.Value,
                    numTaxPercent.Value,
                    numTollPercent.Value,
                    numNetWeight.Value,
                    numGrossWeight.Value,
                    numLength.Value,
                    numWidth.Value,
                    numHeight.Value,
                    numVolume.Value,
                    txtColor.Text.Trim(),
                    txtMaterial.Text.Trim(),
                    txtSize.Text.Trim(),
                    txtBrand.Text.Trim(),
                    txtCountryOfOrigin.Text.Trim(),
                    txtPhysicalDescription.Text.Trim(),
                    _imagePaths(1),
                    _imagePaths(2),
                    _imagePaths(3),
                    _imagePaths(4),
                    _imagePaths(5),
                    _imagePaths(6),
                    _selectedDefaultWarehouseId)

                ' Step 2: Handle Image Files Copying / Saving
                Dim imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages")
                If Not Directory.Exists(imgDir) Then
                    Directory.CreateDirectory(imgDir)
                End If

                Dim anyImageUpdated As Boolean = False
                For i As Integer = 1 To 6
                    If _imageChanged(i) Then
                        anyImageUpdated = True
                        If Not String.IsNullOrEmpty(_imagePaths(i)) AndAlso File.Exists(_imagePaths(i)) Then
                            Dim ext = Path.GetExtension(_imagePaths(i))
                            If String.IsNullOrEmpty(ext) Then ext = ".jpg"
                            Dim destFileName = $"Prod_{savedId}_{i}{ext}"
                            Dim destFullPath = Path.Combine(imgDir, destFileName)
                            If _imagePaths(i) <> destFullPath Then
                                File.Copy(_imagePaths(i), destFullPath, True)
                            End If
                            _imagePaths(i) = Path.Combine("ProductImages", destFileName)
                        Else
                            _imagePaths(i) = ""
                        End If
                    End If
                Next

                ' Step 3: If images were updated, persist image paths
                If anyImageUpdated Then
                    _service.SaveProduct(
                        savedId,
                        txtCode.Text.Trim(),
                        txtName.Text.Trim(),
                        baseUnitName,
                        salesPrice,
                        categoryName,
                        chkActive.Checked,
                        baseUomId,
                        secondaryUomId,
                        numNominalFactor.Value,
                        productGroupId,
                        txtBarcode.Text.Trim(),
                        txtTaxID.Text.Trim(),
                        productType,
                        purchasePrice,
                        numMinStock.Value,
                        numReorderPoint.Value,
                        numMaxStock.Value,
                        trackingType,
                        _locationId,
                        txtTechnicalName.Text.Trim(),
                        numConsumerMarkupProduct.Value,
                        numConsumerDiscountProduct.Value,
                        numColleagueMarkupProduct.Value,
                        numColleagueDiscountProduct.Value,
                        numWholesaleMarkupProduct.Value,
                        numWholesaleDiscountProduct.Value,
                        numTaxPercent.Value,
                        numTollPercent.Value,
                        numNetWeight.Value,
                        numGrossWeight.Value,
                        numLength.Value,
                        numWidth.Value,
                        numHeight.Value,
                        numVolume.Value,
                        txtColor.Text.Trim(),
                        txtMaterial.Text.Trim(),
                        txtSize.Text.Trim(),
                        txtBrand.Text.Trim(),
                        txtCountryOfOrigin.Text.Trim(),
                        txtPhysicalDescription.Text.Trim(),
                        _imagePaths(1),
                        _imagePaths(2),
                        _imagePaths(3),
                        _imagePaths(4),
                        _imagePaths(5),
                        _imagePaths(6),
                        _selectedDefaultWarehouseId)
                End If

                MessageBox.Show("اطلاعات کالا با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnSelectDefaultWarehouse_Click(sender As Object, e As EventArgs) Handles btnSelectDefaultWarehouse.Click
            Dim warehouses = _service.GetWarehouses()
            If warehouses Is Nothing OrElse warehouses.Rows.Count = 0 Then
                MessageBox.Show("هیچ انباری در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب انبار پیش فرض"
                dlg.Size = New Size(450, 350)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True

                Dim grid As New DataGridView()
                grid.Dock = DockStyle.Fill
                grid.DataSource = warehouses
                grid.ReadOnly = True
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                grid.MultiSelect = False
                grid.AllowUserToAddRows = False
                grid.RowHeadersVisible = False

                dlg.Controls.Add(grid)

                AddHandler grid.CellDoubleClick, Sub(s, ea)
                                                     If ea.RowIndex >= 0 Then
                                                         dlg.Tag = grid.Rows(ea.RowIndex).DataBoundItem
                                                         dlg.DialogResult = DialogResult.OK
                                                         dlg.Close()
                                                     End If
                                                 End Sub

                If dlg.ShowDialog() = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    Dim drv = DirectCast(dlg.Tag, DataRowView)
                    _selectedDefaultWarehouseId = Convert.ToInt32(drv("WarehouseID"))
                    lblDefaultWarehouseName.Text = Convert.ToString(drv("WarehouseName"))
                End If
            End Using
        End Sub

        Private Sub BtnSelectLocation_Click(sender As Object, e As EventArgs) Handles btnSelectLocation.Click
            Using frm As New LocationSelectorForm()
                If frm.ShowDialog() = DialogResult.OK Then
                    _locationId = frm.SelectedLocationID
                    lblPhysicalLocationName.Text = frm.SelectedTitlePath
                    lblPhysicalLocationCode.Text = frm.SelectedCodePath
                End If
            End Using
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
