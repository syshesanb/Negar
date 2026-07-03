Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class CompanyFiscalYearForm
        Inherits Form

        Private ReadOnly service As New CompanyFiscalYearService()
        Private ReadOnly _mainForm As MainForm
        Private ReadOnly _openOnSelectTab As Boolean
        Private _selectedCompanyId As Integer?
        Private _selectedFiscalYearId As Integer?
        Private _selectCompanyId As Integer?
        Private _selectFiscalYearId As Integer?
        Private _selectCompanyName As String = String.Empty
        Private _selectFiscalYearName As String = String.Empty

        Private Property StartDateValue As DateTime
            Get
                Dim gDate = PersianDateHelper.ParsePersianDate(lblDtpStartPersian.Text)
                Return If(gDate.HasValue, gDate.Value, Date.Today)
            End Get
            Set(value As DateTime)
                lblDtpStartPersian.Text = PersianDateHelper.ToPersian(value)
            End Set
        End Property

        Private Property EndDateValue As DateTime
            Get
                Dim gDate = PersianDateHelper.ParsePersianDate(lblDtpEndPersian.Text)
                Return If(gDate.HasValue, gDate.Value, Date.Today)
            End Get
            Set(value As DateTime)
                lblDtpEndPersian.Text = PersianDateHelper.ToPersian(value)
            End Set
        End Property

        Private Sub BtnSelectLogo_Click(sender As Object, e As EventArgs) Handles btnSelectLogo.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
                If ofd.ShowDialog() = DialogResult.OK Then
                    picLogoImage.Image = System.Drawing.Image.FromFile(ofd.FileName)
                End If
            End Using
        End Sub

        Private Sub BtnRemoveLogo_Click(sender As Object, e As EventArgs) Handles btnRemoveLogo.Click
            picLogoImage.Image = Nothing
        End Sub

        Private Sub BtnCalReg_Click(sender As Object, e As EventArgs) Handles btnCalReg.Click
            Using frm As New PersianCalendarForm(txtRegistrationDate.Text)
                If frm.ShowDialog() = DialogResult.OK Then
                    txtRegistrationDate.Text = frm.SelectedDate
                End If
            End Using
        End Sub

        Private Function ImageToByteArray(img As System.Drawing.Image) As Byte()
            If img Is Nothing Then Return Nothing
            Using ms As New IO.MemoryStream()
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                Return ms.ToArray()
            End Using
        End Function

        Private Function ByteArrayToImage(byteArray As Object) As System.Drawing.Image
            If byteArray Is Nothing OrElse Convert.IsDBNull(byteArray) Then Return Nothing
            Try
                Dim bytes As Byte() = DirectCast(byteArray, Byte())
                If bytes.Length = 0 Then Return Nothing
                Using ms As New IO.MemoryStream(bytes)
                    Return System.Drawing.Image.FromStream(ms)
                End Using
            Catch
                Return Nothing
            End Try
        End Function

        Public Sub New(Optional mainForm As MainForm = Nothing, Optional openOnSelectTab As Boolean = False)
            _mainForm = mainForm
            _openOnSelectTab = openOnSelectTab
            InitializeComponent()
        End Sub

        Private Sub CompanyFiscalYearForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ApplySecurity()
            cmbLogoPosition.SelectedItem = "سمت چپ"
            LoadCompanies()
            LoadFiscalYears()
            LoadSelectCompanies()
            UpdateCurrentSelectionLabel()
            UpdateLevelsControlsState()
            If _openOnSelectTab Then tabs.SelectedTab = tabSelectActive
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim userType = SessionContext.CurrentUser.UserType
            Dim isSuperAdmin = String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            ' اگر کاربر ادمین یا دارای دسترسی جامع باشد، به کلیه عملیات دسترسی دارد
            Dim hasGlobalCompaniesYears = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageCompaniesYears)

            ' مجوزهای مربوط به شرکت
            Dim canCreateCompany = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageCompanies & PermissionKeys.CanCreate)
            Dim canEditCompany = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageCompanies & PermissionKeys.CanEdit)
            Dim canDeleteCompany = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageCompanies & PermissionKeys.CanDelete)

            ' مجوزهای مربوط به سال مالی
            Dim canCreateFiscalYear = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageFiscalYears & PermissionKeys.CanCreate)
            Dim canEditFiscalYear = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageFiscalYears & PermissionKeys.CanEdit)
            Dim canDeleteFiscalYear = hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageFiscalYears & PermissionKeys.CanDelete)

            ' دکمه‌های شرکت
            btnNewCompany.Visible = canCreateCompany
            btnSaveCompany.Visible = canCreateCompany OrElse canEditCompany
            btnDeleteCompany.Visible = canDeleteCompany

            ' دکمه‌های سال مالی
            btnNewFiscalYear.Visible = canCreateFiscalYear
            btnSaveFiscalYear.Visible = canCreateFiscalYear OrElse canEditFiscalYear
            btnDeleteFiscalYear.Visible = canDeleteFiscalYear

            ' تب انتخاب شرکت و سال مالی جاری
            If Not (hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.SelectCompanyFiscalYear)) Then
                tabs.TabPages.Remove(tabSelectActive)
            End If

            ' تب مدیریت شرکت‌ها
            If Not (hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageCompanies)) Then
                tabs.TabPages.Remove(tabCompanies)
            End If

            ' تب مدیریت سال‌های مالی
            If Not (hasGlobalCompaniesYears OrElse SessionContext.HasPermission(PermissionKeys.ManageFiscalYears)) Then
                tabs.TabPages.Remove(tabFiscalYears)
            End If
        End Sub

        ' ========================
        ' تب شرکتها
        ' ========================

        Private Sub LoadCompanies()
            dgvCompanies.DataSource = service.GetCompanies()
            For Each col As DataGridViewColumn In dgvCompanies.Columns
                col.Visible = False
            Next
            If dgvCompanies.Columns.Contains("CompanyCode") Then dgvCompanies.Columns("CompanyCode").Visible = True : dgvCompanies.Columns("CompanyCode").HeaderText = "کد شرکت"
            If dgvCompanies.Columns.Contains("CompanyName") Then dgvCompanies.Columns("CompanyName").Visible = True : dgvCompanies.Columns("CompanyName").HeaderText = "نام شرکت"
            If dgvCompanies.Columns.Contains("Signatory1Name") Then dgvCompanies.Columns("Signatory1Name").Visible = True : dgvCompanies.Columns("Signatory1Name").HeaderText = "نام امضادار 1"
            If dgvCompanies.Columns.Contains("Signatory2Name") Then dgvCompanies.Columns("Signatory2Name").Visible = True : dgvCompanies.Columns("Signatory2Name").HeaderText = "نام امضادار 2"
            If dgvCompanies.Columns.Contains("Signatory3Name") Then dgvCompanies.Columns("Signatory3Name").Visible = True : dgvCompanies.Columns("Signatory3Name").HeaderText = "نام امضادار 3"
            If dgvCompanies.Columns.Contains("Signatory4Name") Then dgvCompanies.Columns("Signatory4Name").Visible = True : dgvCompanies.Columns("Signatory4Name").HeaderText = "نام امضادار 4"

            cmbCompany.DataSource = service.GetCompanies()
            cmbCompany.DisplayMember = "CompanyName"
            cmbCompany.ValueMember = "CompanyID"
        End Sub

        Private Sub LoadFiscalYears()
            If cmbCompany.SelectedValue Is Nothing OrElse Convert.IsDBNull(cmbCompany.SelectedValue) Then
                dgvFiscalYears.DataSource = Nothing
                Return
            End If
            
            Dim companyId As Integer
            If TypeOf cmbCompany.SelectedValue Is DataRowView Then
                Dim drv = DirectCast(cmbCompany.SelectedValue, DataRowView)
                If drv.Row.Table.Columns.Contains("CompanyID") AndAlso Not drv.Row.IsNull("CompanyID") Then
                    companyId = Convert.ToInt32(drv("CompanyID"))
                Else
                    dgvFiscalYears.DataSource = Nothing
                    Return
                End If
            ElseIf TypeOf cmbCompany.SelectedValue Is Integer OrElse TypeOf cmbCompany.SelectedValue Is Decimal OrElse TypeOf cmbCompany.SelectedValue Is Double OrElse TypeOf cmbCompany.SelectedValue Is Long Then
                companyId = Convert.ToInt32(cmbCompany.SelectedValue)
            Else
                Dim valStr = Convert.ToString(cmbCompany.SelectedValue)
                If Not Integer.TryParse(valStr, companyId) Then
                    dgvFiscalYears.DataSource = Nothing
                    Return
                End If
            End If
            
            dgvFiscalYears.DataSource = service.GetFiscalYearsByCompany(companyId)
            
            If dgvFiscalYears.Columns.Contains("FiscalYearID") Then dgvFiscalYears.Columns("FiscalYearID").Visible = False
            If dgvFiscalYears.Columns.Contains("CompanyID") Then dgvFiscalYears.Columns("CompanyID").Visible = False
            If dgvFiscalYears.Columns.Contains("FiscalYearName") Then dgvFiscalYears.Columns("FiscalYearName").HeaderText = "نام سال مالی"
            If dgvFiscalYears.Columns.Contains("StartDate") Then dgvFiscalYears.Columns("StartDate").HeaderText = "تاریخ شروع"
            If dgvFiscalYears.Columns.Contains("EndDate") Then dgvFiscalYears.Columns("EndDate").HeaderText = "تاریخ پایان"
            If dgvFiscalYears.Columns.Contains("IsActive") Then dgvFiscalYears.Columns("IsActive").HeaderText = "فعال"
        End Sub

        Private Sub CmbCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCompany.SelectedIndexChanged
            LoadFiscalYears()
        End Sub

        Private Sub DgvCompanies_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCompanies.SelectionChanged
            If dgvCompanies.CurrentRow Is Nothing Then Return
            Dim row = dgvCompanies.CurrentRow
            If row.Cells("CompanyID").Value Is Nothing Then Return
            _selectedCompanyId = Convert.ToInt32(row.Cells("CompanyID").Value)
            txtCompanyName.Text = Convert.ToString(row.Cells("CompanyName").Value)
            txtCompanyCode.Text = Convert.ToString(row.Cells("CompanyCode").Value)
            txtAddress.Text = Convert.ToString(row.Cells("Address").Value)
            txtPhone.Text = Convert.ToString(row.Cells("Phone").Value)
            txtTaxId.Text = Convert.ToString(row.Cells("TaxID").Value)
            txtBrandName.Text = Convert.ToString(row.Cells("BrandName").Value)
            txtEconomicCode.Text = Convert.ToString(row.Cells("EconomicCode").Value)
            txtPostalCode.Text = Convert.ToString(row.Cells("PostalCode").Value)
            If row.Cells("RegistrationDate").Value IsNot Nothing AndAlso Not Convert.IsDBNull(row.Cells("RegistrationDate").Value) Then txtRegistrationDate.Text = PersianDateHelper.ToPersian(Convert.ToDateTime(row.Cells("RegistrationDate").Value)) Else txtRegistrationDate.Clear()
            txtRegistrationNumber.Text = Convert.ToString(row.Cells("RegistrationNumber").Value)
            txtActivityField.Text = Convert.ToString(row.Cells("ActivityField").Value)
            txtPhone2.Text = Convert.ToString(row.Cells("Phone2").Value)
            txtEmail.Text = Convert.ToString(row.Cells("Email").Value)
            picLogoImage.Image = ByteArrayToImage(row.Cells("LogoImage").Value)
            txtChairmanName.Text = Convert.ToString(row.Cells("ChairmanName").Value)
            txtInspectorName.Text = Convert.ToString(row.Cells("InspectorName").Value)
            txtCEOName.Text = Convert.ToString(row.Cells("CEOName").Value)
            txtSignatory1Title.Text = Convert.ToString(row.Cells("Signatory1Title").Value)
            txtSignatory1Name.Text = Convert.ToString(row.Cells("Signatory1Name").Value)
            txtSignatory2Title.Text = Convert.ToString(row.Cells("Signatory2Title").Value)
            txtSignatory2Name.Text = Convert.ToString(row.Cells("Signatory2Name").Value)
            txtSignatory3Title.Text = Convert.ToString(row.Cells("Signatory3Title").Value)
            txtSignatory3Name.Text = Convert.ToString(row.Cells("Signatory3Name").Value)
            txtSignatory4Title.Text = Convert.ToString(row.Cells("Signatory4Title").Value)
            txtSignatory4Name.Text = Convert.ToString(row.Cells("Signatory4Name").Value)
            chkCompanyActive.Checked = If(row.Cells("IsActive").Value Is Nothing OrElse row.Cells("IsActive").Value Is DBNull.Value, True, Convert.ToBoolean(row.Cells("IsActive").Value))
            
            Dim logoPosVal = Convert.ToString(row.Cells("LogoPosition").Value)
            If String.Equals(logoPosVal, "Right", StringComparison.OrdinalIgnoreCase) Then
                cmbLogoPosition.SelectedItem = "سمت راست"
            Else
                cmbLogoPosition.SelectedItem = "سمت چپ"
            End If

            numAccountLevels.Value = If(row.Cells("AccountLevels").Value Is Nothing OrElse row.Cells("AccountLevels").Value Is DBNull.Value, 4D, Convert.ToDecimal(row.Cells("AccountLevels").Value))
            numLevel1Length.Value = If(row.Cells("Level1Length").Value Is Nothing OrElse row.Cells("Level1Length").Value Is DBNull.Value, 2D, Convert.ToDecimal(row.Cells("Level1Length").Value))
            numLevel2Length.Value = If(row.Cells("Level2Length").Value Is Nothing OrElse row.Cells("Level2Length").Value Is DBNull.Value, 2D, Convert.ToDecimal(row.Cells("Level2Length").Value))
            numLevel3Length.Value = If(row.Cells("Level3Length").Value Is Nothing OrElse row.Cells("Level3Length").Value Is DBNull.Value, 2D, Convert.ToDecimal(row.Cells("Level3Length").Value))
            numLevel4Length.Value = If(row.Cells("Level4Length").Value Is Nothing OrElse row.Cells("Level4Length").Value Is DBNull.Value, 2D, Convert.ToDecimal(row.Cells("Level4Length").Value))
            numLevel5Length.Value = If(row.Cells("Level5Length").Value Is Nothing OrElse row.Cells("Level5Length").Value Is DBNull.Value, 2D, Convert.ToDecimal(row.Cells("Level5Length").Value))
            UpdateLevelsControlsState()
        End Sub

        Private Sub DgvFiscalYears_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) _
            Handles dgvFiscalYears.CellFormatting, dgvSelectFiscalYears.CellFormatting
            PersianDateHelper.ApplyToGrid(sender, e)
        End Sub

        Private Sub DgvFiscalYears_SelectionChanged(sender As Object, e As EventArgs) Handles dgvFiscalYears.SelectionChanged
            If dgvFiscalYears.CurrentRow Is Nothing Then Return
            Dim row = dgvFiscalYears.CurrentRow
            If row.Cells("FiscalYearID").Value Is Nothing Then Return
            _selectedFiscalYearId = Convert.ToInt32(row.Cells("FiscalYearID").Value)
            If Not row.IsNewRow AndAlso dgvFiscalYears.Columns.Contains("CompanyID") AndAlso row.Cells("CompanyID").Value IsNot Nothing AndAlso Not Convert.IsDBNull(row.Cells("CompanyID").Value) Then
                If Not String.IsNullOrEmpty(cmbCompany.ValueMember) Then
                    Try
                        cmbCompany.SelectedValue = Convert.ToInt32(row.Cells("CompanyID").Value)
                    Catch
                    End Try
                End If
            End If
            txtFiscalYearName.Text = Convert.ToString(row.Cells("FiscalYearName").Value)
            If row.Cells("StartDate").Value IsNot Nothing AndAlso row.Cells("StartDate").Value IsNot DBNull.Value Then StartDateValue = Convert.ToDateTime(row.Cells("StartDate").Value)
            If row.Cells("EndDate").Value IsNot Nothing AndAlso row.Cells("EndDate").Value IsNot DBNull.Value Then EndDateValue = Convert.ToDateTime(row.Cells("EndDate").Value)
            chkFiscalYearActive.Checked = If(row.Cells("IsActive").Value Is Nothing OrElse row.Cells("IsActive").Value Is DBNull.Value, True, Convert.ToBoolean(row.Cells("IsActive").Value))
        End Sub

        Private Sub BtnNewCompany_Click(sender As Object, e As EventArgs) Handles btnNewCompany.Click
            _selectedCompanyId = Nothing
            txtCompanyName.Clear()
            txtCompanyCode.Clear()
            txtAddress.Clear()
            txtPhone.Clear()
            txtTaxId.Clear()
            txtBrandName.Clear()
            txtEconomicCode.Clear()
            txtPostalCode.Clear()
            txtRegistrationDate.Clear()
            txtRegistrationNumber.Clear()
            txtActivityField.Clear()
            txtPhone2.Clear()
            txtEmail.Clear()
            picLogoImage.Image = Nothing
            cmbLogoPosition.SelectedItem = "سمت چپ"
            txtChairmanName.Clear()
            txtInspectorName.Clear()
            txtCEOName.Clear()
            txtSignatory1Title.Clear()
            txtSignatory1Name.Clear()
            txtSignatory2Title.Clear()
            txtSignatory2Name.Clear()
            txtSignatory3Title.Clear()
            txtSignatory3Name.Clear()
            txtSignatory4Title.Clear()
            txtSignatory4Name.Clear()
            chkCompanyActive.Checked = True
            numAccountLevels.Value = 4D
            numLevel1Length.Value = 2D
            numLevel2Length.Value = 2D
            numLevel3Length.Value = 2D
            numLevel4Length.Value = 2D
            numLevel5Length.Value = 2D
            UpdateLevelsControlsState()
        End Sub

        Private Sub BtnSaveCompany_Click(sender As Object, e As EventArgs) Handles btnSaveCompany.Click
            ' در صورت ویرایش شرکت موجود، ابتدا اعتبارسنجی ساختار کدینگ را انجام می‌دهیم
            If _selectedCompanyId.HasValue AndAlso _selectedCompanyId.Value > 0 Then
                Dim proposedLevels = CInt(numAccountLevels.Value)
                Dim proposedLengths = New Integer() {
                    CInt(numLevel1Length.Value),
                    CInt(numLevel2Length.Value),
                    CInt(numLevel3Length.Value),
                    CInt(numLevel4Length.Value),
                    CInt(numLevel5Length.Value)
                }
                Dim errMsg = service.ValidateCompanySettingsChange(_selectedCompanyId.Value, proposedLevels, proposedLengths)
                If Not String.IsNullOrEmpty(errMsg) Then
                    MessageBox.Show(errMsg, "خطای اعتبارسنجی ساختار حساب‌ها", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            Dim regDt = PersianDateHelper.ParsePersianDate(txtRegistrationDate.Text)
            Dim logoPosition = If(cmbLogoPosition.SelectedItem IsNot Nothing, cmbLogoPosition.SelectedItem.ToString(), "سمت چپ")
            Dim logoPosDb = If(logoPosition = "سمت راست", "Right", "Left")
            Try
                service.SaveCompany(_selectedCompanyId, txtCompanyName.Text.Trim(), txtCompanyCode.Text.Trim(), txtBrandName.Text.Trim(), txtEconomicCode.Text.Trim(), DBNull.Value, DBNull.Value, txtPostalCode.Text.Trim(), If(regDt.HasValue, CObj(regDt.Value), DBNull.Value), txtRegistrationNumber.Text.Trim(), txtActivityField.Text.Trim(), txtAddress.Text.Trim(), txtPhone.Text.Trim(), txtPhone2.Text.Trim(), txtEmail.Text.Trim(), txtTaxId.Text.Trim(), ImageToByteArray(picLogoImage.Image), txtChairmanName.Text.Trim(), txtInspectorName.Text.Trim(), txtCEOName.Text.Trim(), txtSignatory1Title.Text.Trim(), txtSignatory1Name.Text.Trim(), txtSignatory2Title.Text.Trim(), txtSignatory2Name.Text.Trim(), txtSignatory3Title.Text.Trim(), txtSignatory3Name.Text.Trim(), txtSignatory4Title.Text.Trim(), txtSignatory4Name.Text.Trim(), CInt(numAccountLevels.Value), CInt(numLevel1Length.Value), CInt(numLevel2Length.Value), CInt(numLevel3Length.Value), CInt(numLevel4Length.Value), CInt(numLevel5Length.Value), chkCompanyActive.Checked, logoPosDb)
                LoadCompanies()
                LoadFiscalYears()
                LoadSelectCompanies()
                MessageBox.Show("اطلاعات شرکت با موفقیت ذخیره شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "هشدار محدودیت سیستم", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی شرکت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnDeleteCompany_Click(sender As Object, e As EventArgs) Handles btnDeleteCompany.Click
            If Not _selectedCompanyId.HasValue Then Return
            service.DeleteCompany(_selectedCompanyId.Value)
            _selectedCompanyId = Nothing
            LoadCompanies()
            LoadFiscalYears()
            LoadSelectCompanies()
        End Sub

        Private Sub BtnRefreshCompanies_Click(sender As Object, e As EventArgs) Handles btnRefreshCompanies.Click
            LoadCompanies()
            LoadFiscalYears()
            LoadSelectCompanies()
        End Sub



        Private Sub DtpStartDate_Click(sender As Object, e As EventArgs) Handles dtpStartDate.Click
            Using frm As New PersianCalendarForm(lblDtpStartPersian.Text)
                If frm.ShowDialog() = DialogResult.OK Then
                    lblDtpStartPersian.Text = frm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub DtpEndDate_Click(sender As Object, e As EventArgs) Handles dtpEndDate.Click
            Using frm As New PersianCalendarForm(lblDtpEndPersian.Text)
                If frm.ShowDialog() = DialogResult.OK Then
                    lblDtpEndPersian.Text = frm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub BtnNewFiscalYear_Click(sender As Object, e As EventArgs) Handles btnNewFiscalYear.Click
            _selectedFiscalYearId = Nothing
            txtFiscalYearName.Clear()
            chkFiscalYearActive.Checked = True
            If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
            StartDateValue = Date.Today
            EndDateValue = Date.Today
        End Sub

        Private Sub BtnSaveFiscalYear_Click(sender As Object, e As EventArgs) Handles btnSaveFiscalYear.Click
            If cmbCompany.SelectedValue Is Nothing Then Return
            Try
                service.SaveFiscalYear(_selectedFiscalYearId, Convert.ToInt32(cmbCompany.SelectedValue), txtFiscalYearName.Text.Trim(), StartDateValue, EndDateValue, chkFiscalYearActive.Checked)
                LoadFiscalYears()
                If _selectCompanyId.HasValue Then
                    LoadSelectFiscalYears(_selectCompanyId.Value)
                End If
                MessageBox.Show("اطلاعات سال مالی با موفقیت ذخیره شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "هشدار محدودیت سیستم", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی سال مالی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnDeleteFiscalYear_Click(sender As Object, e As EventArgs) Handles btnDeleteFiscalYear.Click
            If Not _selectedFiscalYearId.HasValue Then Return
            service.DeleteFiscalYear(_selectedFiscalYearId.Value)
            _selectedFiscalYearId = Nothing
            LoadFiscalYears()
            If _selectCompanyId.HasValue Then
                LoadSelectFiscalYears(_selectCompanyId.Value)
            End If
        End Sub

        Private Sub BtnRefreshFiscalYears_Click(sender As Object, e As EventArgs) Handles btnRefreshFiscalYears.Click
            LoadFiscalYears()
        End Sub

        ' ========================
        ' تب انتخاب شرکت و سال مالی جاری
        ' ========================

        Private Sub LoadSelectCompanies()
            _selectCompanyId = Nothing
            _selectFiscalYearId = Nothing
            _selectCompanyName = String.Empty
            _selectFiscalYearName = String.Empty
            dgvSelectFiscalYears.DataSource = Nothing
            dgvSelectCompanies.DataSource = service.GetCompanies()
            For Each col As DataGridViewColumn In dgvSelectCompanies.Columns
                col.Visible = False
            Next
            If dgvSelectCompanies.Columns.Contains("CompanyCode") Then dgvSelectCompanies.Columns("CompanyCode").Visible = True : dgvSelectCompanies.Columns("CompanyCode").HeaderText = "کد شرکت"
            If dgvSelectCompanies.Columns.Contains("CompanyName") Then dgvSelectCompanies.Columns("CompanyName").Visible = True : dgvSelectCompanies.Columns("CompanyName").HeaderText = "نام شرکت"
            If dgvSelectCompanies.Columns.Contains("Signatory1Name") Then dgvSelectCompanies.Columns("Signatory1Name").Visible = True : dgvSelectCompanies.Columns("Signatory1Name").HeaderText = "نام امضادار 1"
            If dgvSelectCompanies.Columns.Contains("Signatory2Name") Then dgvSelectCompanies.Columns("Signatory2Name").Visible = True : dgvSelectCompanies.Columns("Signatory2Name").HeaderText = "نام امضادار 2"
            If dgvSelectCompanies.Columns.Contains("Signatory3Name") Then dgvSelectCompanies.Columns("Signatory3Name").Visible = True : dgvSelectCompanies.Columns("Signatory3Name").HeaderText = "نام امضادار 3"
            If dgvSelectCompanies.Columns.Contains("Signatory4Name") Then dgvSelectCompanies.Columns("Signatory4Name").Visible = True : dgvSelectCompanies.Columns("Signatory4Name").HeaderText = "نام امضادار 4"
        End Sub

        Private Sub LoadSelectFiscalYears(companyId As Integer)
            _selectFiscalYearId = Nothing
            _selectFiscalYearName = String.Empty
            dgvSelectFiscalYears.DataSource = service.GetFiscalYearsByCompany(companyId)
            If dgvSelectFiscalYears.Columns.Contains("FiscalYearID") Then dgvSelectFiscalYears.Columns("FiscalYearID").Visible = False
            If dgvSelectFiscalYears.Columns.Contains("FiscalYearName") Then dgvSelectFiscalYears.Columns("FiscalYearName").HeaderText = "نام سال مالی"
            If dgvSelectFiscalYears.Columns.Contains("StartDate") Then dgvSelectFiscalYears.Columns("StartDate").HeaderText = "تاریخ شروع"
            If dgvSelectFiscalYears.Columns.Contains("EndDate") Then dgvSelectFiscalYears.Columns("EndDate").HeaderText = "تاریخ پایان"
            If dgvSelectFiscalYears.Columns.Contains("IsActive") Then dgvSelectFiscalYears.Columns("IsActive").HeaderText = "فعال"
        End Sub

        Private Sub DgvSelectCompanies_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSelectCompanies.SelectionChanged
            If dgvSelectCompanies.CurrentRow Is Nothing OrElse dgvSelectCompanies.CurrentRow.IsNewRow Then Return
            Dim val = dgvSelectCompanies.CurrentRow.Cells("CompanyID").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return
            _selectCompanyId = Convert.ToInt32(val)
            _selectCompanyName = Convert.ToString(dgvSelectCompanies.CurrentRow.Cells("CompanyName").Value)
            _selectFiscalYearId = Nothing
            _selectFiscalYearName = String.Empty
            LoadSelectFiscalYears(_selectCompanyId.Value)
        End Sub

        Private Sub DgvSelectFiscalYears_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSelectFiscalYears.SelectionChanged
            If dgvSelectFiscalYears.CurrentRow Is Nothing OrElse dgvSelectFiscalYears.CurrentRow.IsNewRow Then Return
            Dim val = dgvSelectFiscalYears.CurrentRow.Cells("FiscalYearID").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return
            _selectFiscalYearId = Convert.ToInt32(val)
            _selectFiscalYearName = Convert.ToString(dgvSelectFiscalYears.CurrentRow.Cells("FiscalYearName").Value)
        End Sub

        Private Sub BtnSetActive_Click(sender As Object, e As EventArgs) Handles btnSetActive.Click
            If Not _selectCompanyId.HasValue Then
                MessageBox.Show("ابتدا یک شرکت را از لیست بالا انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If Not _selectFiscalYearId.HasValue Then
                MessageBox.Show("ابتدا یک سال مالی را از لیست پایین انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            SessionContext.CurrentCompanyID = _selectCompanyId.Value
            SessionContext.CurrentCompanyName = _selectCompanyName
            SessionContext.CurrentFiscalYearID = _selectFiscalYearId.Value
            SessionContext.CurrentFiscalYearName = _selectFiscalYearName

            UpdateCurrentSelectionLabel()

            If _mainForm IsNot Nothing Then
                _mainForm.UpdateStatusBar()
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub UpdateCurrentSelectionLabel()
            If SessionContext.CurrentCompanyID.HasValue AndAlso SessionContext.CurrentFiscalYearID.HasValue Then
                lblCurrentSelection.Text = "جاری:  شرکت: " & SessionContext.CurrentCompanyName &
                                           "   |   سال مالی: " & SessionContext.CurrentFiscalYearName
                lblCurrentSelection.ForeColor = Drawing.Color.DarkGreen
            ElseIf SessionContext.CurrentCompanyID.HasValue Then
                lblCurrentSelection.Text = "شرکت جاری: " & SessionContext.CurrentCompanyName & "   |   سال مالی: انتخاب نشده"
                lblCurrentSelection.ForeColor = Drawing.Color.DarkOrange
            Else
                lblCurrentSelection.Text = "هنوز شرکت و سال مالی جاری انتخاب نشده است."
                lblCurrentSelection.ForeColor = Drawing.Color.Gray
            End If
        End Sub

        Private Sub numAccountLevels_ValueChanged(sender As Object, e As EventArgs) Handles numAccountLevels.ValueChanged
            UpdateLevelsControlsState()
        End Sub

        Private Sub UpdateLevelsControlsState()
            Dim lvls = CInt(numAccountLevels.Value)

            Dim UpdateControl = Sub(num As NumericUpDown, enabled As Boolean)
                                    If enabled Then
                                        num.Enabled = True
                                        num.Minimum = 2
                                        If num.Value = 0 Then num.Value = 2
                                    Else
                                        num.Minimum = 0
                                        num.Value = 0
                                        num.Enabled = False
                                    End If
                                End Sub

            UpdateControl(numLevel3Length, lvls >= 3)
            UpdateControl(numLevel4Length, lvls >= 4)
            UpdateControl(numLevel5Length, lvls >= 5)
        End Sub
    End Class
End Namespace
