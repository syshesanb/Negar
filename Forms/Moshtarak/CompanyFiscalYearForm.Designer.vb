Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class CompanyFiscalYearForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents tabs As TabControl
        Friend WithEvents tabCompanies As TabPage
        Friend WithEvents tabFiscalYears As TabPage
        Friend WithEvents tabSelectActive As TabPage

        ' --- شرکتها ---
        Friend WithEvents companiesSplit As SplitContainer
        Friend WithEvents dgvCompanies As DataGridView
        Friend WithEvents companyEditor As Panel
        
        Friend WithEvents grpGeneral As GroupBox
        Friend WithEvents lblCompanyName As Label
        Friend WithEvents txtCompanyName As TextBox
        Friend WithEvents lblCompanyCode As Label
        Friend WithEvents txtCompanyCode As TextBox
        Friend WithEvents lblBrandName As Label
        Friend WithEvents txtBrandName As TextBox
        Friend WithEvents lblRegistrationNumber As Label
        Friend WithEvents txtRegistrationNumber As TextBox
        Friend WithEvents lblRegistrationDate As Label
        Friend WithEvents txtRegistrationDate As MaskedTextBox
        Friend WithEvents btnCalReg As Button
        Friend WithEvents lblActivityField As Label
        Friend WithEvents txtActivityField As TextBox
        Friend WithEvents lblLogo As Label
        Friend WithEvents picLogoImage As PictureBox
        Friend WithEvents btnSelectLogo As Button
        Friend WithEvents btnRemoveLogo As Button
        Friend WithEvents lblLogoPosition As Label
        Friend WithEvents cmbLogoPosition As ComboBox

        Friend WithEvents grpPostal As GroupBox
        Friend WithEvents lblAddress As Label
        Friend WithEvents txtAddress As TextBox
        Friend WithEvents lblPostalCode As Label
        Friend WithEvents txtPostalCode As TextBox
        Friend WithEvents lblPhone As Label
        Friend WithEvents txtPhone As TextBox
        Friend WithEvents lblPhone2 As Label
        Friend WithEvents txtPhone2 As TextBox
        Friend WithEvents lblEmail As Label
        Friend WithEvents txtEmail As TextBox


        Friend WithEvents grpSignatories As GroupBox
        Friend WithEvents lblChairmanName As Label
        Friend WithEvents txtChairmanName As TextBox
        Friend WithEvents lblCEOName As Label
        Friend WithEvents txtCEOName As TextBox
        Friend WithEvents lblInspectorName As Label
        Friend WithEvents txtInspectorName As TextBox
        Friend WithEvents lblSignatory1 As Label
        Friend WithEvents txtSignatory1Title As TextBox
        Friend WithEvents txtSignatory1Name As TextBox
        Friend WithEvents lblSignatory2 As Label
        Friend WithEvents txtSignatory2Title As TextBox
        Friend WithEvents txtSignatory2Name As TextBox
        Friend WithEvents lblSignatory3 As Label
        Friend WithEvents txtSignatory3Title As TextBox
        Friend WithEvents txtSignatory3Name As TextBox
        Friend WithEvents lblSignatory4 As Label
        Friend WithEvents txtSignatory4Title As TextBox
        Friend WithEvents txtSignatory4Name As TextBox


        Friend WithEvents chkCompanyActive As CheckBox
        Friend WithEvents btnNewCompany As Button
        Friend WithEvents btnSaveCompany As Button
        Friend WithEvents btnDeleteCompany As Button
        Friend WithEvents btnRefreshCompanies As Button

        ' --- سالهای مالی ---
        Friend WithEvents fySplit As SplitContainer
        Friend WithEvents dgvFiscalYears As DataGridView
        Friend WithEvents fyEditor As Panel
        Friend WithEvents lblCompany As Label
        Friend WithEvents cmbCompany As ComboBox
        Friend WithEvents lblYearName As Label
        Friend WithEvents txtFiscalYearName As TextBox
        Friend WithEvents lblStart As Label
        Friend WithEvents dtpStartDate As Button
        Friend WithEvents lblEnd As Label
        Friend WithEvents dtpEndDate As Button
        Friend WithEvents lblDtpStartPersian As Label
        Friend WithEvents lblDtpEndPersian As Label
        Friend WithEvents chkFiscalYearActive As CheckBox
        Friend WithEvents btnNewFiscalYear As Button
        Friend WithEvents btnSaveFiscalYear As Button
        Friend WithEvents btnDeleteFiscalYear As Button
        Friend WithEvents btnRefreshFiscalYears As Button

        ' --- تب انتخاب جاری ---
        Friend WithEvents selectSplit As SplitContainer
        Friend WithEvents pnlSelectCompaniesHeader As Panel
        Friend WithEvents lblSelectCompaniesTitle As Label
        Friend WithEvents dgvSelectCompanies As DataGridView
        Friend WithEvents pnlSelectFiscalYearsHeader As Panel
        Friend WithEvents lblSelectFiscalYearsTitle As Label
        Friend WithEvents dgvSelectFiscalYears As DataGridView
        Friend WithEvents pnlSelectBottom As Panel
        Friend WithEvents btnSetActive As Button
        Friend WithEvents lblCurrentSelection As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabs = New System.Windows.Forms.TabControl()
            Me.tabSelectActive = New System.Windows.Forms.TabPage()
            Me.selectSplit = New System.Windows.Forms.SplitContainer()
            Me.dgvSelectCompanies = New System.Windows.Forms.DataGridView()
            Me.pnlSelectCompaniesHeader = New System.Windows.Forms.Panel()
            Me.lblSelectCompaniesTitle = New System.Windows.Forms.Label()
            Me.dgvSelectFiscalYears = New System.Windows.Forms.DataGridView()
            Me.pnlSelectFiscalYearsHeader = New System.Windows.Forms.Panel()
            Me.lblSelectFiscalYearsTitle = New System.Windows.Forms.Label()
            Me.pnlSelectBottom = New System.Windows.Forms.Panel()
            Me.btnSetActive = New System.Windows.Forms.Button()
            Me.lblCurrentSelection = New System.Windows.Forms.Label()
            Me.tabCompanies = New System.Windows.Forms.TabPage()
            Me.companiesSplit = New System.Windows.Forms.SplitContainer()
            Me.companyEditor = New System.Windows.Forms.Panel()
            Me.grpGeneral = New System.Windows.Forms.GroupBox()
            Me.lblCompanyCode = New System.Windows.Forms.Label()
            Me.txtCompanyCode = New System.Windows.Forms.TextBox()
            Me.lblCompanyName = New System.Windows.Forms.Label()
            Me.txtCompanyName = New System.Windows.Forms.TextBox()
            Me.lblBrandName = New System.Windows.Forms.Label()
            Me.txtBrandName = New System.Windows.Forms.TextBox()
            Me.lblRegistrationNumber = New System.Windows.Forms.Label()
            Me.txtRegistrationNumber = New System.Windows.Forms.TextBox()
            Me.lblRegistrationDate = New System.Windows.Forms.Label()
            Me.txtRegistrationDate = New System.Windows.Forms.MaskedTextBox()
            Me.btnCalReg = New System.Windows.Forms.Button()
            Me.lblActivityField = New System.Windows.Forms.Label()
            Me.txtActivityField = New System.Windows.Forms.TextBox()
            Me.lblLogo = New System.Windows.Forms.Label()
            Me.picLogoImage = New System.Windows.Forms.PictureBox()
            Me.btnSelectLogo = New System.Windows.Forms.Button()
            Me.btnRemoveLogo = New System.Windows.Forms.Button()
            Me.lblLogoPosition = New System.Windows.Forms.Label()
            Me.cmbLogoPosition = New System.Windows.Forms.ComboBox()
            Me.grpPostal = New System.Windows.Forms.GroupBox()
            Me.lblAddress = New System.Windows.Forms.Label()
            Me.txtAddress = New System.Windows.Forms.TextBox()
            Me.lblPostalCode = New System.Windows.Forms.Label()
            Me.txtPostalCode = New System.Windows.Forms.TextBox()
            Me.lblPhone = New System.Windows.Forms.Label()
            Me.txtPhone = New System.Windows.Forms.TextBox()
            Me.lblPhone2 = New System.Windows.Forms.Label()
            Me.txtPhone2 = New System.Windows.Forms.TextBox()
            Me.lblEmail = New System.Windows.Forms.Label()
            Me.txtEmail = New System.Windows.Forms.TextBox()
            Me.grpSignatories = New System.Windows.Forms.GroupBox()
            Me.lblChairmanName = New System.Windows.Forms.Label()
            Me.txtChairmanName = New System.Windows.Forms.TextBox()
            Me.lblCEOName = New System.Windows.Forms.Label()
            Me.txtCEOName = New System.Windows.Forms.TextBox()
            Me.lblInspectorName = New System.Windows.Forms.Label()
            Me.txtInspectorName = New System.Windows.Forms.TextBox()
            Me.lblSignatory1 = New System.Windows.Forms.Label()
            Me.txtSignatory1Title = New System.Windows.Forms.TextBox()
            Me.txtSignatory1Name = New System.Windows.Forms.TextBox()
            Me.lblSignatory2 = New System.Windows.Forms.Label()
            Me.txtSignatory2Title = New System.Windows.Forms.TextBox()
            Me.txtSignatory2Name = New System.Windows.Forms.TextBox()
            Me.lblSignatory3 = New System.Windows.Forms.Label()
            Me.txtSignatory3Title = New System.Windows.Forms.TextBox()
            Me.txtSignatory3Name = New System.Windows.Forms.TextBox()
            Me.lblSignatory4 = New System.Windows.Forms.Label()
            Me.txtSignatory4Title = New System.Windows.Forms.TextBox()
            Me.txtSignatory4Name = New System.Windows.Forms.TextBox()
            Me.chkCompanyActive = New System.Windows.Forms.CheckBox()
            Me.btnNewCompany = New System.Windows.Forms.Button()
            Me.btnSaveCompany = New System.Windows.Forms.Button()
            Me.btnDeleteCompany = New System.Windows.Forms.Button()
            Me.btnRefreshCompanies = New System.Windows.Forms.Button()
            Me.dgvCompanies = New System.Windows.Forms.DataGridView()
            Me.tabFiscalYears = New System.Windows.Forms.TabPage()
            Me.fySplit = New System.Windows.Forms.SplitContainer()
            Me.fyEditor = New System.Windows.Forms.Panel()
            Me.lblCompany = New System.Windows.Forms.Label()
            Me.cmbCompany = New System.Windows.Forms.ComboBox()
            Me.lblYearName = New System.Windows.Forms.Label()
            Me.txtFiscalYearName = New System.Windows.Forms.TextBox()
            Me.lblStart = New System.Windows.Forms.Label()
            Me.lblDtpStartPersian = New System.Windows.Forms.Label()
            Me.dtpStartDate = New System.Windows.Forms.Button()
            Me.lblEnd = New System.Windows.Forms.Label()
            Me.lblDtpEndPersian = New System.Windows.Forms.Label()
            Me.dtpEndDate = New System.Windows.Forms.Button()
            Me.chkFiscalYearActive = New System.Windows.Forms.CheckBox()
            Me.btnNewFiscalYear = New System.Windows.Forms.Button()
            Me.btnSaveFiscalYear = New System.Windows.Forms.Button()
            Me.btnDeleteFiscalYear = New System.Windows.Forms.Button()
            Me.btnRefreshFiscalYears = New System.Windows.Forms.Button()
            Me.dgvFiscalYears = New System.Windows.Forms.DataGridView()
            Me.tabs.SuspendLayout()
            Me.tabSelectActive.SuspendLayout()
            CType(Me.selectSplit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.selectSplit.Panel1.SuspendLayout()
            Me.selectSplit.Panel2.SuspendLayout()
            Me.selectSplit.SuspendLayout()
            CType(Me.dgvSelectCompanies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSelectCompaniesHeader.SuspendLayout()
            CType(Me.dgvSelectFiscalYears, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSelectFiscalYearsHeader.SuspendLayout()
            Me.pnlSelectBottom.SuspendLayout()
            Me.tabCompanies.SuspendLayout()
            CType(Me.companiesSplit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.companiesSplit.Panel1.SuspendLayout()
            Me.companiesSplit.Panel2.SuspendLayout()
            Me.companiesSplit.SuspendLayout()
            Me.companyEditor.SuspendLayout()
            Me.grpGeneral.SuspendLayout()
            CType(Me.picLogoImage, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpPostal.SuspendLayout()
            Me.grpSignatories.SuspendLayout()
            CType(Me.dgvCompanies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabFiscalYears.SuspendLayout()
            CType(Me.fySplit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.fySplit.Panel1.SuspendLayout()
            Me.fySplit.Panel2.SuspendLayout()
            Me.fySplit.SuspendLayout()
            Me.fyEditor.SuspendLayout()
            CType(Me.dgvFiscalYears, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'tabs
            '
            Me.tabs.Controls.Add(Me.tabSelectActive)
            Me.tabs.Controls.Add(Me.tabCompanies)
            Me.tabs.Controls.Add(Me.tabFiscalYears)
            Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabs.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.tabs.Location = New System.Drawing.Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New System.Drawing.Size(1300, 700)
            Me.tabs.TabIndex = 0
            '
            'tabSelectActive
            '
            Me.tabSelectActive.Controls.Add(Me.selectSplit)
            Me.tabSelectActive.Controls.Add(Me.pnlSelectBottom)
            Me.tabSelectActive.Location = New System.Drawing.Point(4, 23)
            Me.tabSelectActive.Name = "tabSelectActive"
            Me.tabSelectActive.Size = New System.Drawing.Size(1292, 673)
            Me.tabSelectActive.TabIndex = 0
            Me.tabSelectActive.Text = "انتخاب شرکت و سال مالی جاری"
            '
            'selectSplit
            '
            Me.selectSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.selectSplit.Location = New System.Drawing.Point(0, 0)
            Me.selectSplit.Name = "selectSplit"
            Me.selectSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'selectSplit.Panel1
            '
            Me.selectSplit.Panel1.Controls.Add(Me.dgvSelectCompanies)
            Me.selectSplit.Panel1.Controls.Add(Me.pnlSelectCompaniesHeader)
            Me.selectSplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'selectSplit.Panel2
            '
            Me.selectSplit.Panel2.Controls.Add(Me.dgvSelectFiscalYears)
            Me.selectSplit.Panel2.Controls.Add(Me.pnlSelectFiscalYearsHeader)
            Me.selectSplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.selectSplit.Size = New System.Drawing.Size(1292, 617)
            Me.selectSplit.SplitterDistance = 300
            Me.selectSplit.TabIndex = 0
            '
            'dgvSelectCompanies
            '
            Me.dgvSelectCompanies.AllowUserToAddRows = False
            Me.dgvSelectCompanies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvSelectCompanies.BackgroundColor = System.Drawing.Color.White
            Me.dgvSelectCompanies.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSelectCompanies.Location = New System.Drawing.Point(0, 30)
            Me.dgvSelectCompanies.MultiSelect = False
            Me.dgvSelectCompanies.Name = "dgvSelectCompanies"
            Me.dgvSelectCompanies.ReadOnly = True
            Me.dgvSelectCompanies.RowHeadersVisible = False
            Me.dgvSelectCompanies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSelectCompanies.Size = New System.Drawing.Size(1292, 270)
            Me.dgvSelectCompanies.TabIndex = 0
            '
            'pnlSelectCompaniesHeader
            '
            Me.pnlSelectCompaniesHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlSelectCompaniesHeader.Controls.Add(Me.lblSelectCompaniesTitle)
            Me.pnlSelectCompaniesHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSelectCompaniesHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlSelectCompaniesHeader.Name = "pnlSelectCompaniesHeader"
            Me.pnlSelectCompaniesHeader.Size = New System.Drawing.Size(1292, 30)
            Me.pnlSelectCompaniesHeader.TabIndex = 1
            '
            'lblSelectCompaniesTitle
            '
            Me.lblSelectCompaniesTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSelectCompaniesTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSelectCompaniesTitle.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblSelectCompaniesTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblSelectCompaniesTitle.Name = "lblSelectCompaniesTitle"
            Me.lblSelectCompaniesTitle.Size = New System.Drawing.Size(1292, 30)
            Me.lblSelectCompaniesTitle.TabIndex = 0
            Me.lblSelectCompaniesTitle.Text = "  لیست شرکتها  (یک شرکت را انتخاب کنید)"
            Me.lblSelectCompaniesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'dgvSelectFiscalYears
            '
            Me.dgvSelectFiscalYears.AllowUserToAddRows = False
            Me.dgvSelectFiscalYears.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvSelectFiscalYears.BackgroundColor = System.Drawing.Color.White
            Me.dgvSelectFiscalYears.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSelectFiscalYears.Location = New System.Drawing.Point(0, 30)
            Me.dgvSelectFiscalYears.MultiSelect = False
            Me.dgvSelectFiscalYears.Name = "dgvSelectFiscalYears"
            Me.dgvSelectFiscalYears.ReadOnly = True
            Me.dgvSelectFiscalYears.RowHeadersVisible = False
            Me.dgvSelectFiscalYears.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSelectFiscalYears.Size = New System.Drawing.Size(1292, 283)
            Me.dgvSelectFiscalYears.TabIndex = 0
            '
            'pnlSelectFiscalYearsHeader
            '
            Me.pnlSelectFiscalYearsHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(230, Byte), Integer))
            Me.pnlSelectFiscalYearsHeader.Controls.Add(Me.lblSelectFiscalYearsTitle)
            Me.pnlSelectFiscalYearsHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSelectFiscalYearsHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlSelectFiscalYearsHeader.Name = "pnlSelectFiscalYearsHeader"
            Me.pnlSelectFiscalYearsHeader.Size = New System.Drawing.Size(1292, 30)
            Me.pnlSelectFiscalYearsHeader.TabIndex = 1
            '
            'lblSelectFiscalYearsTitle
            '
            Me.lblSelectFiscalYearsTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSelectFiscalYearsTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSelectFiscalYearsTitle.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblSelectFiscalYearsTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblSelectFiscalYearsTitle.Name = "lblSelectFiscalYearsTitle"
            Me.lblSelectFiscalYearsTitle.Size = New System.Drawing.Size(1292, 30)
            Me.lblSelectFiscalYearsTitle.TabIndex = 0
            Me.lblSelectFiscalYearsTitle.Text = "  لیست سالهای مالی  (یک سال مالی را انتخاب کنید)"
            Me.lblSelectFiscalYearsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pnlSelectBottom
            '
            Me.pnlSelectBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlSelectBottom.Controls.Add(Me.btnSetActive)
            Me.pnlSelectBottom.Controls.Add(Me.lblCurrentSelection)
            Me.pnlSelectBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlSelectBottom.Location = New System.Drawing.Point(0, 617)
            Me.pnlSelectBottom.Name = "pnlSelectBottom"
            Me.pnlSelectBottom.Padding = New System.Windows.Forms.Padding(8)
            Me.pnlSelectBottom.Size = New System.Drawing.Size(1292, 56)
            Me.pnlSelectBottom.TabIndex = 1
            '
            'btnSetActive
            '
            Me.btnSetActive.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
            Me.btnSetActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSetActive.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnSetActive.ForeColor = System.Drawing.Color.White
            Me.btnSetActive.Location = New System.Drawing.Point(8, 9)
            Me.btnSetActive.Name = "btnSetActive"
            Me.btnSetActive.Size = New System.Drawing.Size(260, 38)
            Me.btnSetActive.TabIndex = 0
            Me.btnSetActive.Text = "✔  انتخاب شرکت و سال مالی جاری"
            Me.btnSetActive.UseVisualStyleBackColor = False
            '
            'lblCurrentSelection
            '
            Me.lblCurrentSelection.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblCurrentSelection.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblCurrentSelection.Location = New System.Drawing.Point(280, 9)
            Me.lblCurrentSelection.Name = "lblCurrentSelection"
            Me.lblCurrentSelection.Size = New System.Drawing.Size(950, 38)
            Me.lblCurrentSelection.TabIndex = 1
            Me.lblCurrentSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'tabCompanies
            '
            Me.tabCompanies.Controls.Add(Me.companiesSplit)
            Me.tabCompanies.Location = New System.Drawing.Point(4, 23)
            Me.tabCompanies.Name = "tabCompanies"
            Me.tabCompanies.Size = New System.Drawing.Size(1292, 673)
            Me.tabCompanies.TabIndex = 1
            Me.tabCompanies.Text = "مدیریت شرکتها"
            '
            'companiesSplit
            '
            Me.companiesSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.companiesSplit.Location = New System.Drawing.Point(0, 0)
            Me.companiesSplit.Name = "companiesSplit"
            Me.companiesSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'companiesSplit.Panel1
            '
            Me.companiesSplit.Panel1.Controls.Add(Me.companyEditor)
            Me.companiesSplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'companiesSplit.Panel2
            '
            Me.companiesSplit.Panel2.Controls.Add(Me.dgvCompanies)
            Me.companiesSplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.companiesSplit.Size = New System.Drawing.Size(1292, 673)
            Me.companiesSplit.SplitterDistance = 420
            Me.companiesSplit.TabIndex = 0
            '
            'companyEditor
            '
            Me.companyEditor.Controls.Add(Me.grpGeneral)
            Me.companyEditor.Controls.Add(Me.grpPostal)
            Me.companyEditor.Controls.Add(Me.grpSignatories)
            Me.companyEditor.Controls.Add(Me.chkCompanyActive)
            Me.companyEditor.Controls.Add(Me.btnNewCompany)
            Me.companyEditor.Controls.Add(Me.btnSaveCompany)
            Me.companyEditor.Controls.Add(Me.btnDeleteCompany)
            Me.companyEditor.Controls.Add(Me.btnRefreshCompanies)
            Me.companyEditor.Dock = System.Windows.Forms.DockStyle.Fill
            Me.companyEditor.Location = New System.Drawing.Point(0, 0)
            Me.companyEditor.Name = "companyEditor"
            Me.companyEditor.Padding = New System.Windows.Forms.Padding(10)
            Me.companyEditor.Size = New System.Drawing.Size(1292, 420)
            Me.companyEditor.TabIndex = 0
            '
            'grpGeneral
            '
            Me.grpGeneral.Controls.Add(Me.lblCompanyCode)
            Me.grpGeneral.Controls.Add(Me.txtCompanyCode)
            Me.grpGeneral.Controls.Add(Me.lblCompanyName)
            Me.grpGeneral.Controls.Add(Me.txtCompanyName)
            Me.grpGeneral.Controls.Add(Me.lblBrandName)
            Me.grpGeneral.Controls.Add(Me.txtBrandName)
            Me.grpGeneral.Controls.Add(Me.lblRegistrationNumber)
            Me.grpGeneral.Controls.Add(Me.txtRegistrationNumber)
            Me.grpGeneral.Controls.Add(Me.lblRegistrationDate)
            Me.grpGeneral.Controls.Add(Me.txtRegistrationDate)
            Me.grpGeneral.Controls.Add(Me.btnCalReg)
            Me.grpGeneral.Controls.Add(Me.lblActivityField)
            Me.grpGeneral.Controls.Add(Me.txtActivityField)
            Me.grpGeneral.Controls.Add(Me.lblLogo)
            Me.grpGeneral.Controls.Add(Me.picLogoImage)
            Me.grpGeneral.Controls.Add(Me.btnSelectLogo)
            Me.grpGeneral.Controls.Add(Me.btnRemoveLogo)
            Me.grpGeneral.Controls.Add(Me.lblLogoPosition)
            Me.grpGeneral.Controls.Add(Me.cmbLogoPosition)
            Me.grpGeneral.Location = New System.Drawing.Point(970, 10)
            Me.grpGeneral.Name = "grpGeneral"
            Me.grpGeneral.Size = New System.Drawing.Size(310, 361)
            Me.grpGeneral.TabIndex = 0
            Me.grpGeneral.TabStop = False
            Me.grpGeneral.Text = "مشخصات عمومی"
            '
            'lblCompanyCode
            '
            Me.lblCompanyCode.Location = New System.Drawing.Point(190, 23)
            Me.lblCompanyCode.Name = "lblCompanyCode"
            Me.lblCompanyCode.Size = New System.Drawing.Size(110, 20)
            Me.lblCompanyCode.TabIndex = 0
            Me.lblCompanyCode.Text = "کد شرکت:"
            Me.lblCompanyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtCompanyCode
            '
            Me.txtCompanyCode.Location = New System.Drawing.Point(15, 20)
            Me.txtCompanyCode.Name = "txtCompanyCode"
            Me.txtCompanyCode.Size = New System.Drawing.Size(170, 22)
            Me.txtCompanyCode.TabIndex = 1
            '
            'lblCompanyName
            '
            Me.lblCompanyName.Location = New System.Drawing.Point(190, 53)
            Me.lblCompanyName.Name = "lblCompanyName"
            Me.lblCompanyName.Size = New System.Drawing.Size(110, 20)
            Me.lblCompanyName.TabIndex = 2
            Me.lblCompanyName.Text = "نام شرکت:"
            Me.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtCompanyName
            '
            Me.txtCompanyName.Location = New System.Drawing.Point(15, 50)
            Me.txtCompanyName.Name = "txtCompanyName"
            Me.txtCompanyName.Size = New System.Drawing.Size(170, 22)
            Me.txtCompanyName.TabIndex = 3
            '
            'lblBrandName
            '
            Me.lblBrandName.Location = New System.Drawing.Point(190, 83)
            Me.lblBrandName.Name = "lblBrandName"
            Me.lblBrandName.Size = New System.Drawing.Size(110, 20)
            Me.lblBrandName.TabIndex = 4
            Me.lblBrandName.Text = "عنوان تجاری:"
            Me.lblBrandName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtBrandName
            '
            Me.txtBrandName.Location = New System.Drawing.Point(15, 80)
            Me.txtBrandName.Name = "txtBrandName"
            Me.txtBrandName.Size = New System.Drawing.Size(170, 22)
            Me.txtBrandName.TabIndex = 5
            '
            'lblRegistrationNumber
            '
            Me.lblRegistrationNumber.Location = New System.Drawing.Point(190, 113)
            Me.lblRegistrationNumber.Name = "lblRegistrationNumber"
            Me.lblRegistrationNumber.Size = New System.Drawing.Size(110, 20)
            Me.lblRegistrationNumber.TabIndex = 6
            Me.lblRegistrationNumber.Text = "شماره ثبت:"
            Me.lblRegistrationNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtRegistrationNumber
            '
            Me.txtRegistrationNumber.Location = New System.Drawing.Point(15, 110)
            Me.txtRegistrationNumber.Name = "txtRegistrationNumber"
            Me.txtRegistrationNumber.Size = New System.Drawing.Size(170, 22)
            Me.txtRegistrationNumber.TabIndex = 7
            '
            'lblRegistrationDate
            '
            Me.lblRegistrationDate.Location = New System.Drawing.Point(190, 143)
            Me.lblRegistrationDate.Name = "lblRegistrationDate"
            Me.lblRegistrationDate.Size = New System.Drawing.Size(110, 20)
            Me.lblRegistrationDate.TabIndex = 8
            Me.lblRegistrationDate.Text = "تاریخ ثبت:"
            Me.lblRegistrationDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtRegistrationDate
            '
            Me.txtRegistrationDate.Location = New System.Drawing.Point(45, 140)
            Me.txtRegistrationDate.Mask = "0000/00/00"
            Me.txtRegistrationDate.Name = "txtRegistrationDate"
            Me.txtRegistrationDate.Size = New System.Drawing.Size(140, 22)
            Me.txtRegistrationDate.TabIndex = 9
            '
            'btnCalReg
            '
            Me.btnCalReg.Location = New System.Drawing.Point(15, 140)
            Me.btnCalReg.Name = "btnCalReg"
            Me.btnCalReg.Size = New System.Drawing.Size(25, 22)
            Me.btnCalReg.TabIndex = 10
            Me.btnCalReg.Text = "..."
            '
            'lblActivityField
            '
            Me.lblActivityField.Location = New System.Drawing.Point(190, 173)
            Me.lblActivityField.Name = "lblActivityField"
            Me.lblActivityField.Size = New System.Drawing.Size(110, 20)
            Me.lblActivityField.TabIndex = 11
            Me.lblActivityField.Text = "زمینه فعالیت:"
            Me.lblActivityField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtActivityField
            '
            Me.txtActivityField.Location = New System.Drawing.Point(15, 170)
            Me.txtActivityField.Multiline = True
            Me.txtActivityField.Name = "txtActivityField"
            Me.txtActivityField.Size = New System.Drawing.Size(170, 50)
            Me.txtActivityField.TabIndex = 12
            '
            'lblLogo
            '
            Me.lblLogo.Location = New System.Drawing.Point(190, 233)
            Me.lblLogo.Name = "lblLogo"
            Me.lblLogo.Size = New System.Drawing.Size(110, 20)
            Me.lblLogo.TabIndex = 13
            Me.lblLogo.Text = "آرم شرکت:"
            Me.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'picLogoImage
            '
            Me.picLogoImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.picLogoImage.Location = New System.Drawing.Point(100, 230)
            Me.picLogoImage.Name = "picLogoImage"
            Me.picLogoImage.Size = New System.Drawing.Size(85, 85)
            Me.picLogoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picLogoImage.TabIndex = 14
            Me.picLogoImage.TabStop = False
            '
            'btnSelectLogo
            '
            Me.btnSelectLogo.Location = New System.Drawing.Point(15, 230)
            Me.btnSelectLogo.Name = "btnSelectLogo"
            Me.btnSelectLogo.Size = New System.Drawing.Size(80, 28)
            Me.btnSelectLogo.TabIndex = 15
            Me.btnSelectLogo.Text = "انتخاب..."
            '
            'btnRemoveLogo
            '
            Me.btnRemoveLogo.Location = New System.Drawing.Point(15, 264)
            Me.btnRemoveLogo.Name = "btnRemoveLogo"
            Me.btnRemoveLogo.Size = New System.Drawing.Size(80, 28)
            Me.btnRemoveLogo.TabIndex = 16
            Me.btnRemoveLogo.Text = "حذف"
            '
            'lblLogoPosition
            '
            Me.lblLogoPosition.Location = New System.Drawing.Point(180, 325)
            Me.lblLogoPosition.Name = "lblLogoPosition"
            Me.lblLogoPosition.Size = New System.Drawing.Size(120, 20)
            Me.lblLogoPosition.TabIndex = 17
            Me.lblLogoPosition.Text = "محل آرم در گزارشات چاپی :"
            Me.lblLogoPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'cmbLogoPosition
            '
            Me.cmbLogoPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbLogoPosition.FormattingEnabled = True
            Me.cmbLogoPosition.Items.AddRange(New Object() {"سمت راست", "سمت چپ"})
            Me.cmbLogoPosition.Location = New System.Drawing.Point(15, 322)
            Me.cmbLogoPosition.Name = "cmbLogoPosition"
            Me.cmbLogoPosition.Size = New System.Drawing.Size(160, 22)
            Me.cmbLogoPosition.TabIndex = 18
            '
            'grpPostal
            '
            Me.grpPostal.Controls.Add(Me.lblAddress)
            Me.grpPostal.Controls.Add(Me.txtAddress)
            Me.grpPostal.Controls.Add(Me.lblPostalCode)
            Me.grpPostal.Controls.Add(Me.txtPostalCode)
            Me.grpPostal.Controls.Add(Me.lblPhone)
            Me.grpPostal.Controls.Add(Me.txtPhone)
            Me.grpPostal.Controls.Add(Me.lblPhone2)
            Me.grpPostal.Controls.Add(Me.txtPhone2)
            Me.grpPostal.Controls.Add(Me.lblEmail)
            Me.grpPostal.Controls.Add(Me.txtEmail)
            Me.grpPostal.Location = New System.Drawing.Point(650, 10)
            Me.grpPostal.Name = "grpPostal"
            Me.grpPostal.Size = New System.Drawing.Size(300, 175)
            Me.grpPostal.TabIndex = 1
            Me.grpPostal.TabStop = False
            Me.grpPostal.Text = "مشخصات پستی"
            '
            'lblAddress
            '
            Me.lblAddress.Location = New System.Drawing.Point(180, 23)
            Me.lblAddress.Name = "lblAddress"
            Me.lblAddress.Size = New System.Drawing.Size(110, 20)
            Me.lblAddress.TabIndex = 0
            Me.lblAddress.Text = "آدرس:"
            Me.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtAddress
            '
            Me.txtAddress.Location = New System.Drawing.Point(15, 20)
            Me.txtAddress.Name = "txtAddress"
            Me.txtAddress.Size = New System.Drawing.Size(160, 22)
            Me.txtAddress.TabIndex = 1
            '
            'lblPostalCode
            '
            Me.lblPostalCode.Location = New System.Drawing.Point(180, 53)
            Me.lblPostalCode.Name = "lblPostalCode"
            Me.lblPostalCode.Size = New System.Drawing.Size(110, 20)
            Me.lblPostalCode.TabIndex = 2
            Me.lblPostalCode.Text = "کد پستی:"
            Me.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtPostalCode
            '
            Me.txtPostalCode.Location = New System.Drawing.Point(15, 50)
            Me.txtPostalCode.Name = "txtPostalCode"
            Me.txtPostalCode.Size = New System.Drawing.Size(160, 22)
            Me.txtPostalCode.TabIndex = 3
            '
            'lblPhone
            '
            Me.lblPhone.Location = New System.Drawing.Point(180, 83)
            Me.lblPhone.Name = "lblPhone"
            Me.lblPhone.Size = New System.Drawing.Size(110, 20)
            Me.lblPhone.TabIndex = 4
            Me.lblPhone.Text = "تلفن:"
            Me.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtPhone
            '
            Me.txtPhone.Location = New System.Drawing.Point(15, 80)
            Me.txtPhone.Name = "txtPhone"
            Me.txtPhone.Size = New System.Drawing.Size(160, 22)
            Me.txtPhone.TabIndex = 5
            '
            'lblPhone2
            '
            Me.lblPhone2.Location = New System.Drawing.Point(180, 113)
            Me.lblPhone2.Name = "lblPhone2"
            Me.lblPhone2.Size = New System.Drawing.Size(110, 20)
            Me.lblPhone2.TabIndex = 6
            Me.lblPhone2.Text = "تلفن 2:"
            Me.lblPhone2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtPhone2
            '
            Me.txtPhone2.Location = New System.Drawing.Point(15, 110)
            Me.txtPhone2.Name = "txtPhone2"
            Me.txtPhone2.Size = New System.Drawing.Size(160, 22)
            Me.txtPhone2.TabIndex = 7
            '
            'lblEmail
            '
            Me.lblEmail.Location = New System.Drawing.Point(180, 143)
            Me.lblEmail.Name = "lblEmail"
            Me.lblEmail.Size = New System.Drawing.Size(110, 20)
            Me.lblEmail.TabIndex = 8
            Me.lblEmail.Text = "ایمیل:"
            Me.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtEmail
            '
            Me.txtEmail.Location = New System.Drawing.Point(15, 140)
            Me.txtEmail.Name = "txtEmail"
            Me.txtEmail.Size = New System.Drawing.Size(160, 22)
            Me.txtEmail.TabIndex = 9
            '

            '
            'grpSignatories
            '
            Me.grpSignatories.Controls.Add(Me.lblChairmanName)
            Me.grpSignatories.Controls.Add(Me.txtChairmanName)
            Me.grpSignatories.Controls.Add(Me.lblCEOName)
            Me.grpSignatories.Controls.Add(Me.txtCEOName)
            Me.grpSignatories.Controls.Add(Me.lblInspectorName)
            Me.grpSignatories.Controls.Add(Me.txtInspectorName)
            Me.grpSignatories.Controls.Add(Me.lblSignatory1)
            Me.grpSignatories.Controls.Add(Me.txtSignatory1Title)
            Me.grpSignatories.Controls.Add(Me.txtSignatory1Name)
            Me.grpSignatories.Controls.Add(Me.lblSignatory2)
            Me.grpSignatories.Controls.Add(Me.txtSignatory2Title)
            Me.grpSignatories.Controls.Add(Me.txtSignatory2Name)
            Me.grpSignatories.Controls.Add(Me.lblSignatory3)
            Me.grpSignatories.Controls.Add(Me.txtSignatory3Title)
            Me.grpSignatories.Controls.Add(Me.txtSignatory3Name)
            Me.grpSignatories.Controls.Add(Me.lblSignatory4)
            Me.grpSignatories.Controls.Add(Me.txtSignatory4Title)
            Me.grpSignatories.Controls.Add(Me.txtSignatory4Name)
            Me.grpSignatories.Location = New System.Drawing.Point(241, 10)
            Me.grpSignatories.Name = "grpSignatories"
            Me.grpSignatories.Size = New System.Drawing.Size(399, 280)
            Me.grpSignatories.TabIndex = 3
            Me.grpSignatories.TabStop = False
            Me.grpSignatories.Text = "صاحبان امضاء و ارکان"
            '
            'lblChairmanName
            '
            Me.lblChairmanName.Location = New System.Drawing.Point(282, 23)
            Me.lblChairmanName.Name = "lblChairmanName"
            Me.lblChairmanName.Size = New System.Drawing.Size(100, 20)
            Me.lblChairmanName.TabIndex = 0
            Me.lblChairmanName.Text = "رئیس هیات مدیره:"
            Me.lblChairmanName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtChairmanName
            '
            Me.txtChairmanName.Location = New System.Drawing.Point(15, 20)
            Me.txtChairmanName.Name = "txtChairmanName"
            Me.txtChairmanName.Size = New System.Drawing.Size(260, 22)
            Me.txtChairmanName.TabIndex = 1
            '
            'lblCEOName
            '
            Me.lblCEOName.Location = New System.Drawing.Point(282, 53)
            Me.lblCEOName.Name = "lblCEOName"
            Me.lblCEOName.Size = New System.Drawing.Size(100, 20)
            Me.lblCEOName.TabIndex = 2
            Me.lblCEOName.Text = "مدیر عامل:"
            Me.lblCEOName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtCEOName
            '
            Me.txtCEOName.Location = New System.Drawing.Point(15, 50)
            Me.txtCEOName.Name = "txtCEOName"
            Me.txtCEOName.Size = New System.Drawing.Size(260, 22)
            Me.txtCEOName.TabIndex = 3
            '
            'lblInspectorName
            '
            Me.lblInspectorName.Location = New System.Drawing.Point(282, 83)
            Me.lblInspectorName.Name = "lblInspectorName"
            Me.lblInspectorName.Size = New System.Drawing.Size(100, 20)
            Me.lblInspectorName.TabIndex = 4
            Me.lblInspectorName.Text = "بازرس:"
            Me.lblInspectorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtInspectorName
            '
            Me.txtInspectorName.Location = New System.Drawing.Point(15, 80)
            Me.txtInspectorName.Name = "txtInspectorName"
            Me.txtInspectorName.Size = New System.Drawing.Size(260, 22)
            Me.txtInspectorName.TabIndex = 5
            '
            'lblSignatory1
            '
            Me.lblSignatory1.Location = New System.Drawing.Point(282, 113)
            Me.lblSignatory1.Name = "lblSignatory1"
            Me.lblSignatory1.Size = New System.Drawing.Size(100, 20)
            Me.lblSignatory1.TabIndex = 6
            Me.lblSignatory1.Text = "امضا دار 1 (سمت/نام):"
            Me.lblSignatory1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtSignatory1Title
            '
            Me.txtSignatory1Title.Location = New System.Drawing.Point(149, 111)
            Me.txtSignatory1Title.Name = "txtSignatory1Title"
            Me.txtSignatory1Title.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory1Title.TabIndex = 7
            '
            'txtSignatory1Name
            '
            Me.txtSignatory1Name.Location = New System.Drawing.Point(15, 110)
            Me.txtSignatory1Name.Name = "txtSignatory1Name"
            Me.txtSignatory1Name.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory1Name.TabIndex = 8
            '
            'lblSignatory2
            '
            Me.lblSignatory2.Location = New System.Drawing.Point(282, 143)
            Me.lblSignatory2.Name = "lblSignatory2"
            Me.lblSignatory2.Size = New System.Drawing.Size(100, 20)
            Me.lblSignatory2.TabIndex = 9
            Me.lblSignatory2.Text = "امضا دار 2 (سمت/نام):"
            Me.lblSignatory2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtSignatory2Title
            '
            Me.txtSignatory2Title.Location = New System.Drawing.Point(149, 140)
            Me.txtSignatory2Title.Name = "txtSignatory2Title"
            Me.txtSignatory2Title.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory2Title.TabIndex = 10
            '
            'txtSignatory2Name
            '
            Me.txtSignatory2Name.Location = New System.Drawing.Point(15, 140)
            Me.txtSignatory2Name.Name = "txtSignatory2Name"
            Me.txtSignatory2Name.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory2Name.TabIndex = 11
            '
            'lblSignatory3
            '
            Me.lblSignatory3.Location = New System.Drawing.Point(282, 173)
            Me.lblSignatory3.Name = "lblSignatory3"
            Me.lblSignatory3.Size = New System.Drawing.Size(100, 20)
            Me.lblSignatory3.TabIndex = 12
            Me.lblSignatory3.Text = "امضا دار 3 (سمت/نام):"
            Me.lblSignatory3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtSignatory3Title
            '
            Me.txtSignatory3Title.Location = New System.Drawing.Point(149, 170)
            Me.txtSignatory3Title.Name = "txtSignatory3Title"
            Me.txtSignatory3Title.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory3Title.TabIndex = 13
            '
            'txtSignatory3Name
            '
            Me.txtSignatory3Name.Location = New System.Drawing.Point(15, 170)
            Me.txtSignatory3Name.Name = "txtSignatory3Name"
            Me.txtSignatory3Name.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory3Name.TabIndex = 14
            '
            'lblSignatory4
            '
            Me.lblSignatory4.Location = New System.Drawing.Point(282, 203)
            Me.lblSignatory4.Name = "lblSignatory4"
            Me.lblSignatory4.Size = New System.Drawing.Size(100, 20)
            Me.lblSignatory4.TabIndex = 15
            Me.lblSignatory4.Text = "امضا دار 4 (سمت/نام):"
            Me.lblSignatory4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtSignatory4Title
            '
            Me.txtSignatory4Title.Location = New System.Drawing.Point(149, 200)
            Me.txtSignatory4Title.Name = "txtSignatory4Title"
            Me.txtSignatory4Title.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory4Title.TabIndex = 16
            '
            'txtSignatory4Name
            '
            Me.txtSignatory4Name.Location = New System.Drawing.Point(15, 200)
            Me.txtSignatory4Name.Name = "txtSignatory4Name"
            Me.txtSignatory4Name.Size = New System.Drawing.Size(126, 22)
            Me.txtSignatory4Name.TabIndex = 17
            '

            '
            'chkCompanyActive
            '
            Me.chkCompanyActive.Checked = True
            Me.chkCompanyActive.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkCompanyActive.Location = New System.Drawing.Point(650, 372)
            Me.chkCompanyActive.Name = "chkCompanyActive"
            Me.chkCompanyActive.Size = New System.Drawing.Size(80, 24)
            Me.chkCompanyActive.TabIndex = 5
            Me.chkCompanyActive.Text = "فعال"
            '
            'btnNewCompany
            '
            Me.btnNewCompany.Location = New System.Drawing.Point(500, 370)
            Me.btnNewCompany.Name = "btnNewCompany"
            Me.btnNewCompany.Size = New System.Drawing.Size(110, 30)
            Me.btnNewCompany.TabIndex = 6
            Me.btnNewCompany.Text = "جدید"
            '
            'btnSaveCompany
            '
            Me.btnSaveCompany.Location = New System.Drawing.Point(380, 370)
            Me.btnSaveCompany.Name = "btnSaveCompany"
            Me.btnSaveCompany.Size = New System.Drawing.Size(110, 30)
            Me.btnSaveCompany.TabIndex = 7
            Me.btnSaveCompany.Text = "ذخیره"
            '
            'btnDeleteCompany
            '
            Me.btnDeleteCompany.Location = New System.Drawing.Point(260, 370)
            Me.btnDeleteCompany.Name = "btnDeleteCompany"
            Me.btnDeleteCompany.Size = New System.Drawing.Size(110, 30)
            Me.btnDeleteCompany.TabIndex = 8
            Me.btnDeleteCompany.Text = "حذف"
            '
            'btnRefreshCompanies
            '
            Me.btnRefreshCompanies.Location = New System.Drawing.Point(140, 370)
            Me.btnRefreshCompanies.Name = "btnRefreshCompanies"
            Me.btnRefreshCompanies.Size = New System.Drawing.Size(110, 30)
            Me.btnRefreshCompanies.TabIndex = 9
            Me.btnRefreshCompanies.Text = "بازخوانی"
            '
            'dgvCompanies
            '
            Me.dgvCompanies.AllowUserToAddRows = False
            Me.dgvCompanies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvCompanies.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvCompanies.Location = New System.Drawing.Point(0, 0)
            Me.dgvCompanies.MultiSelect = False
            Me.dgvCompanies.Name = "dgvCompanies"
            Me.dgvCompanies.ReadOnly = True
            Me.dgvCompanies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvCompanies.Size = New System.Drawing.Size(1292, 249)
            Me.dgvCompanies.TabIndex = 0
            '
            'tabFiscalYears
            '
            Me.tabFiscalYears.Controls.Add(Me.fySplit)
            Me.tabFiscalYears.Location = New System.Drawing.Point(4, 23)
            Me.tabFiscalYears.Name = "tabFiscalYears"
            Me.tabFiscalYears.Size = New System.Drawing.Size(1292, 673)
            Me.tabFiscalYears.TabIndex = 2
            Me.tabFiscalYears.Text = "مدیریت سالهای مالی"
            '
            'fySplit
            '
            Me.fySplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.fySplit.Location = New System.Drawing.Point(0, 0)
            Me.fySplit.Name = "fySplit"
            Me.fySplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'fySplit.Panel1
            '
            Me.fySplit.Panel1.Controls.Add(Me.fyEditor)
            Me.fySplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'fySplit.Panel2
            '
            Me.fySplit.Panel2.Controls.Add(Me.dgvFiscalYears)
            Me.fySplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.fySplit.Size = New System.Drawing.Size(1292, 673)
            Me.fySplit.SplitterDistance = 420
            Me.fySplit.TabIndex = 0
            '
            'fyEditor
            '
            Me.fyEditor.AutoScroll = True
            Me.fyEditor.Controls.Add(Me.lblCompany)
            Me.fyEditor.Controls.Add(Me.cmbCompany)
            Me.fyEditor.Controls.Add(Me.lblYearName)
            Me.fyEditor.Controls.Add(Me.txtFiscalYearName)
            Me.fyEditor.Controls.Add(Me.lblStart)
            Me.fyEditor.Controls.Add(Me.lblDtpStartPersian)
            Me.fyEditor.Controls.Add(Me.dtpStartDate)
            Me.fyEditor.Controls.Add(Me.lblEnd)
            Me.fyEditor.Controls.Add(Me.lblDtpEndPersian)
            Me.fyEditor.Controls.Add(Me.dtpEndDate)
            Me.fyEditor.Controls.Add(Me.chkFiscalYearActive)
            Me.fyEditor.Controls.Add(Me.btnNewFiscalYear)
            Me.fyEditor.Controls.Add(Me.btnSaveFiscalYear)
            Me.fyEditor.Controls.Add(Me.btnDeleteFiscalYear)
            Me.fyEditor.Controls.Add(Me.btnRefreshFiscalYears)
            Me.fyEditor.Dock = System.Windows.Forms.DockStyle.Fill
            Me.fyEditor.Location = New System.Drawing.Point(0, 0)
            Me.fyEditor.Name = "fyEditor"
            Me.fyEditor.Padding = New System.Windows.Forms.Padding(10)
            Me.fyEditor.Size = New System.Drawing.Size(1292, 420)
            Me.fyEditor.TabIndex = 0
            '
            'lblCompany
            '
            Me.lblCompany.Location = New System.Drawing.Point(430, 13)
            Me.lblCompany.Name = "lblCompany"
            Me.lblCompany.Size = New System.Drawing.Size(180, 22)
            Me.lblCompany.TabIndex = 0
            Me.lblCompany.Text = "شرکت:"
            '
            'cmbCompany
            '
            Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompany.Location = New System.Drawing.Point(120, 10)
            Me.cmbCompany.Name = "cmbCompany"
            Me.cmbCompany.Size = New System.Drawing.Size(300, 22)
            Me.cmbCompany.TabIndex = 1
            '
            'lblYearName
            '
            Me.lblYearName.Location = New System.Drawing.Point(430, 49)
            Me.lblYearName.Name = "lblYearName"
            Me.lblYearName.Size = New System.Drawing.Size(180, 22)
            Me.lblYearName.TabIndex = 2
            Me.lblYearName.Text = "نام سال مالی:"
            '
            'txtFiscalYearName
            '
            Me.txtFiscalYearName.Location = New System.Drawing.Point(120, 46)
            Me.txtFiscalYearName.Name = "txtFiscalYearName"
            Me.txtFiscalYearName.Size = New System.Drawing.Size(300, 22)
            Me.txtFiscalYearName.TabIndex = 3
            '
            'lblStart
            '
            Me.lblStart.Location = New System.Drawing.Point(430, 85)
            Me.lblStart.Name = "lblStart"
            Me.lblStart.Size = New System.Drawing.Size(180, 22)
            Me.lblStart.TabIndex = 4
            Me.lblStart.Text = "تاریخ شروع:"
            '
            'lblDtpStartPersian
            '
            Me.lblDtpStartPersian.Font = New System.Drawing.Font("Courier New", 9.0!)
            Me.lblDtpStartPersian.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblDtpStartPersian.Location = New System.Drawing.Point(120, 85)
            Me.lblDtpStartPersian.Name = "lblDtpStartPersian"
            Me.lblDtpStartPersian.Size = New System.Drawing.Size(130, 22)
            Me.lblDtpStartPersian.TabIndex = 5
            '
            'dtpStartDate
            '
            Me.dtpStartDate.Location = New System.Drawing.Point(255, 82)
            Me.dtpStartDate.Name = "dtpStartDate"
            Me.dtpStartDate.Size = New System.Drawing.Size(26, 22)
            Me.dtpStartDate.TabIndex = 6
            Me.dtpStartDate.Text = "..."
            Me.dtpStartDate.UseVisualStyleBackColor = True
            '
            'lblEnd
            '
            Me.lblEnd.Location = New System.Drawing.Point(430, 121)
            Me.lblEnd.Name = "lblEnd"
            Me.lblEnd.Size = New System.Drawing.Size(180, 22)
            Me.lblEnd.TabIndex = 7
            Me.lblEnd.Text = "تاریخ پایان:"
            '
            'lblDtpEndPersian
            '
            Me.lblDtpEndPersian.Font = New System.Drawing.Font("Courier New", 9.0!)
            Me.lblDtpEndPersian.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblDtpEndPersian.Location = New System.Drawing.Point(120, 121)
            Me.lblDtpEndPersian.Name = "lblDtpEndPersian"
            Me.lblDtpEndPersian.Size = New System.Drawing.Size(130, 22)
            Me.lblDtpEndPersian.TabIndex = 8
            '
            'dtpEndDate
            '
            Me.dtpEndDate.Location = New System.Drawing.Point(255, 118)
            Me.dtpEndDate.Name = "dtpEndDate"
            Me.dtpEndDate.Size = New System.Drawing.Size(26, 22)
            Me.dtpEndDate.TabIndex = 9
            Me.dtpEndDate.Text = "..."
            Me.dtpEndDate.UseVisualStyleBackColor = True
            '
            'chkFiscalYearActive
            '
            Me.chkFiscalYearActive.Checked = True
            Me.chkFiscalYearActive.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkFiscalYearActive.Location = New System.Drawing.Point(120, 154)
            Me.chkFiscalYearActive.Name = "chkFiscalYearActive"
            Me.chkFiscalYearActive.Size = New System.Drawing.Size(80, 24)
            Me.chkFiscalYearActive.TabIndex = 10
            Me.chkFiscalYearActive.Text = "فعال"
            '
            'btnNewFiscalYear
            '
            Me.btnNewFiscalYear.Location = New System.Drawing.Point(10, 190)
            Me.btnNewFiscalYear.Name = "btnNewFiscalYear"
            Me.btnNewFiscalYear.Size = New System.Drawing.Size(80, 28)
            Me.btnNewFiscalYear.TabIndex = 11
            Me.btnNewFiscalYear.Text = "جدید"
            '
            'btnSaveFiscalYear
            '
            Me.btnSaveFiscalYear.Location = New System.Drawing.Point(100, 190)
            Me.btnSaveFiscalYear.Name = "btnSaveFiscalYear"
            Me.btnSaveFiscalYear.Size = New System.Drawing.Size(80, 28)
            Me.btnSaveFiscalYear.TabIndex = 12
            Me.btnSaveFiscalYear.Text = "ذخیره"
            '
            'btnDeleteFiscalYear
            '
            Me.btnDeleteFiscalYear.Location = New System.Drawing.Point(190, 190)
            Me.btnDeleteFiscalYear.Name = "btnDeleteFiscalYear"
            Me.btnDeleteFiscalYear.Size = New System.Drawing.Size(80, 28)
            Me.btnDeleteFiscalYear.TabIndex = 13
            Me.btnDeleteFiscalYear.Text = "حذف"
            '
            'btnRefreshFiscalYears
            '
            Me.btnRefreshFiscalYears.Location = New System.Drawing.Point(280, 190)
            Me.btnRefreshFiscalYears.Name = "btnRefreshFiscalYears"
            Me.btnRefreshFiscalYears.Size = New System.Drawing.Size(80, 28)
            Me.btnRefreshFiscalYears.TabIndex = 14
            Me.btnRefreshFiscalYears.Text = "بازخوانی"
            '
            'dgvFiscalYears
            '
            Me.dgvFiscalYears.AllowUserToAddRows = False
            Me.dgvFiscalYears.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvFiscalYears.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvFiscalYears.Location = New System.Drawing.Point(0, 0)
            Me.dgvFiscalYears.MultiSelect = False
            Me.dgvFiscalYears.Name = "dgvFiscalYears"
            Me.dgvFiscalYears.ReadOnly = True
            Me.dgvFiscalYears.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvFiscalYears.Size = New System.Drawing.Size(1292, 249)
            Me.dgvFiscalYears.TabIndex = 0
            '
            'CompanyFiscalYearForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1300, 700)
            Me.Controls.Add(Me.tabs)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "CompanyFiscalYearForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "مدیریت شرکتها و سالهای مالی"
            Me.tabs.ResumeLayout(False)
            Me.tabSelectActive.ResumeLayout(False)
            Me.selectSplit.Panel1.ResumeLayout(False)
            Me.selectSplit.Panel2.ResumeLayout(False)
            CType(Me.selectSplit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.selectSplit.ResumeLayout(False)
            CType(Me.dgvSelectCompanies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSelectCompaniesHeader.ResumeLayout(False)
            CType(Me.dgvSelectFiscalYears, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSelectFiscalYearsHeader.ResumeLayout(False)
            Me.pnlSelectBottom.ResumeLayout(False)
            Me.tabCompanies.ResumeLayout(False)
            Me.companiesSplit.Panel1.ResumeLayout(False)
            Me.companiesSplit.Panel2.ResumeLayout(False)
            CType(Me.companiesSplit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.companiesSplit.ResumeLayout(False)
            Me.companyEditor.ResumeLayout(False)
            Me.grpGeneral.ResumeLayout(False)
            Me.grpGeneral.PerformLayout()
            CType(Me.picLogoImage, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpPostal.ResumeLayout(False)
            Me.grpPostal.PerformLayout()
            Me.grpSignatories.ResumeLayout(False)
            Me.grpSignatories.PerformLayout()
            CType(Me.dgvCompanies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabFiscalYears.ResumeLayout(False)
            Me.fySplit.Panel1.ResumeLayout(False)
            Me.fySplit.Panel2.ResumeLayout(False)
            CType(Me.fySplit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.fySplit.ResumeLayout(False)
            Me.fyEditor.ResumeLayout(False)
            Me.fyEditor.PerformLayout()
            CType(Me.dgvFiscalYears, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
