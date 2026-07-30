Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class MainForm
        Inherits Form

        Private components As IContainer

        ' Main Top-Level Menus
        Friend WithEvents pnlMainMenuContainer As Panel
        Friend WithEvents mainMenu As MenuStrip
        Friend WithEvents mSystemMgmt As ToolStripMenuItem
        Friend WithEvents mUserMgmt As ToolStripMenuItem
        Friend WithEvents mCompanyMgmt As ToolStripMenuItem
        Friend WithEvents mAccounting As ToolStripMenuItem
        Friend WithEvents mTradeWarehouse As ToolStripMenuItem
        Friend WithEvents mPayroll As ToolStripMenuItem
        Friend WithEvents mAmval As ToolStripMenuItem
        Friend WithEvents mAutomation As ToolStripMenuItem
        Friend WithEvents mCrm As ToolStripMenuItem
        Friend WithEvents mTreasury As ToolStripMenuItem
        Friend WithEvents mBudgeting As ToolStripMenuItem
        Friend WithEvents mProduction As ToolStripMenuItem
        Friend WithEvents mProject As ToolStripMenuItem
        Friend WithEvents mKpi As ToolStripMenuItem
        Friend WithEvents mBusinessShells As ToolStripMenuItem
        Friend WithEvents mUtilities As ToolStripMenuItem

        ' Sub-items for Payroll
        Friend WithEvents miPayrollMain As ToolStripMenuItem
        Friend WithEvents miPayrollReports As ToolStripMenuItem

        ' Sub-items for Amval
        Friend WithEvents miAmvalMain As ToolStripMenuItem
        Friend WithEvents miAmvalReports As ToolStripMenuItem

        ' Sub-items for Automation
        Friend WithEvents miAutomationMain As ToolStripMenuItem
        Friend WithEvents miAutomationReports As ToolStripMenuItem

        ' Sub-items for CRM
        Friend WithEvents miCrmMain As ToolStripMenuItem
        Friend WithEvents miCrmReports As ToolStripMenuItem

        ' Sub-items for Treasury
        Friend WithEvents miTreasuryMain As ToolStripMenuItem
        Friend WithEvents miTreasuryReports As ToolStripMenuItem

        ' Sub-items for Budgeting
        Friend WithEvents miBudgetingMain As ToolStripMenuItem
        Friend WithEvents miBudgetingReports As ToolStripMenuItem

        ' Sub-items for Production
        Friend WithEvents miProductionMain As ToolStripMenuItem
        Friend WithEvents miProductionReports As ToolStripMenuItem

        ' Sub-items for Project
        Friend WithEvents miProjectMain As ToolStripMenuItem
        Friend WithEvents miProjectReports As ToolStripMenuItem

        ' Sub-items for KPI
        Friend WithEvents miKpiMain As ToolStripMenuItem
        Friend WithEvents miKpiReports As ToolStripMenuItem

        ' Sub-items for System Management
        Friend WithEvents miSettingsMessages As ToolStripMenuItem
        Friend WithEvents miSettingsThemes As ToolStripMenuItem
        Friend WithEvents miSepSys1 As ToolStripSeparator
        Friend WithEvents miBackupData As ToolStripMenuItem
        Friend WithEvents miRestoreData As ToolStripMenuItem
        Friend WithEvents miSepSys2 As ToolStripSeparator
        Friend WithEvents miCreateRelease As ToolStripMenuItem
        Friend WithEvents miCreateUpdate As ToolStripMenuItem
        Friend WithEvents miExportDecryptedDb As ToolStripMenuItem
        Friend WithEvents miSepSys3 As ToolStripSeparator
        Friend WithEvents miLock As ToolStripMenuItem
        Friend WithEvents miSepSys4 As ToolStripSeparator
        Friend WithEvents miAbout As ToolStripMenuItem
        Friend WithEvents miContact As ToolStripMenuItem
        Friend WithEvents miExit As ToolStripMenuItem

        ' Sub-items for User Management
        Friend WithEvents miUsers As ToolStripMenuItem
        Friend WithEvents miBasicUsers As ToolStripMenuItem
        Friend WithEvents miSepUser1 As ToolStripSeparator
        Friend WithEvents miChangeProfile As ToolStripMenuItem
        Friend WithEvents miSwitchUser As ToolStripMenuItem

        ' Sub-items for Company Management
        Friend WithEvents miCompanyFiscalYears As ToolStripMenuItem

        ' Sub-items for Accounting
        Friend WithEvents miAccountingMain As ToolStripMenuItem
        Friend WithEvents miReportsAccounting As ToolStripMenuItem

        ' Sub-items for Trade & Warehousing
        Friend WithEvents miTradeMini As ToolStripMenuItem
        Friend WithEvents miTradeMedium As ToolStripMenuItem
        Friend WithEvents miTradeBig As ToolStripMenuItem
        Friend WithEvents miTradeWarehouseMain As ToolStripMenuItem
        Friend WithEvents miReportsTrade As ToolStripMenuItem

        ' Sub-items for Business Shells
        Friend WithEvents miShellGeneral As ToolStripMenuItem
        Friend WithEvents miShellRetail As ToolStripMenuItem
        Friend WithEvents miShellServices As ToolStripMenuItem

        ' Sub-items for Utilities / Tools
        Friend WithEvents miUtilCalculator As ToolStripMenuItem
        Friend WithEvents miUtilNotes As ToolStripMenuItem
        Friend WithEvents miUtilCalendar As ToolStripMenuItem

        ' Toolbar (Top Level Main Menu Titles as Buttons)
        Friend WithEvents pnlToolBar As Panel
        Friend WithEvents tool As ToolStrip
        Friend WithEvents btnToolSystemMgmt As ToolStripButton
        Friend WithEvents btnToolUserMgmt As ToolStripButton
        Friend WithEvents btnToolCompanyMgmt As ToolStripButton
        Friend WithEvents btnToolAccounting As ToolStripButton
        Friend WithEvents btnToolTradeWarehouse As ToolStripButton
        Friend WithEvents btnToolPayroll As ToolStripButton
        Friend WithEvents btnToolAmval As ToolStripButton
        Friend WithEvents btnToolAutomation As ToolStripButton
        Friend WithEvents btnToolCrm As ToolStripButton
        Friend WithEvents btnToolTreasury As ToolStripButton
        Friend WithEvents btnToolBudgeting As ToolStripButton
        Friend WithEvents btnToolProduction As ToolStripButton
        Friend WithEvents btnToolProject As ToolStripButton
        Friend WithEvents btnToolKpi As ToolStripButton
        Friend WithEvents btnToolBusinessShells As ToolStripButton
        Friend WithEvents btnToolUtilities As ToolStripButton

        ' Status bar
        Friend WithEvents status As StatusStrip
        Friend WithEvents lblUser As ToolStripStatusLabel
        Friend WithEvents lblSep1 As ToolStripSeparator
        Friend WithEvents lblCompany As ToolStripStatusLabel
        Friend WithEvents lblSep2 As ToolStripSeparator
        Friend WithEvents lblFiscalYear As ToolStripStatusLabel
        Friend WithEvents lblSpring As ToolStripStatusLabel
        Friend WithEvents lblDateTime As ToolStripStatusLabel

        ' Dashboard Panel (Populated dynamically with sub-items of selected category)
        Friend WithEvents flpDashboard As FlowLayoutPanel

        ' Timer for clock
        Friend WithEvents clockTimer As Timer

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlMainMenuContainer = New Panel()
            Me.mainMenu = New MenuStrip()
            Me.mSystemMgmt = New ToolStripMenuItem()
            Me.mUserMgmt = New ToolStripMenuItem()
            Me.mCompanyMgmt = New ToolStripMenuItem()
            Me.mAccounting = New ToolStripMenuItem()
            Me.mTradeWarehouse = New ToolStripMenuItem()
            Me.mPayroll = New ToolStripMenuItem()
            Me.mAmval = New ToolStripMenuItem()
            Me.mAutomation = New ToolStripMenuItem()
            Me.mCrm = New ToolStripMenuItem()
            Me.mTreasury = New ToolStripMenuItem()
            Me.mBudgeting = New ToolStripMenuItem()
            Me.mProduction = New ToolStripMenuItem()
            Me.mProject = New ToolStripMenuItem()
            Me.mKpi = New ToolStripMenuItem()
            Me.mBusinessShells = New ToolStripMenuItem()
            Me.mUtilities = New ToolStripMenuItem()

            Me.miPayrollMain = New ToolStripMenuItem()
            Me.miPayrollReports = New ToolStripMenuItem()

            Me.miAmvalMain = New ToolStripMenuItem()
            Me.miAmvalReports = New ToolStripMenuItem()

            Me.miAutomationMain = New ToolStripMenuItem()
            Me.miAutomationReports = New ToolStripMenuItem()

            Me.miCrmMain = New ToolStripMenuItem()
            Me.miCrmReports = New ToolStripMenuItem()

            Me.miTreasuryMain = New ToolStripMenuItem()
            Me.miTreasuryReports = New ToolStripMenuItem()

            Me.miBudgetingMain = New ToolStripMenuItem()
            Me.miBudgetingReports = New ToolStripMenuItem()

            Me.miProductionMain = New ToolStripMenuItem()
            Me.miProductionReports = New ToolStripMenuItem()

            Me.miProjectMain = New ToolStripMenuItem()
            Me.miProjectReports = New ToolStripMenuItem()

            Me.miKpiMain = New ToolStripMenuItem()
            Me.miKpiReports = New ToolStripMenuItem()

            Me.miSettingsMessages = New ToolStripMenuItem()
            Me.miSettingsThemes = New ToolStripMenuItem()
            Me.miSepSys1 = New ToolStripSeparator()
            Me.miBackupData = New ToolStripMenuItem()
            Me.miRestoreData = New ToolStripMenuItem()
            Me.miSepSys2 = New ToolStripSeparator()
            Me.miCreateRelease = New ToolStripMenuItem()
            Me.miCreateUpdate = New ToolStripMenuItem()
            Me.miExportDecryptedDb = New ToolStripMenuItem()
            Me.miSepSys3 = New ToolStripSeparator()
            Me.miLock = New ToolStripMenuItem()
            Me.miSepSys4 = New ToolStripSeparator()
            Me.miAbout = New ToolStripMenuItem()
            Me.miContact = New ToolStripMenuItem()
            Me.miExit = New ToolStripMenuItem()

            Me.miUsers = New ToolStripMenuItem()
            Me.miBasicUsers = New ToolStripMenuItem()
            Me.miSepUser1 = New ToolStripSeparator()
            Me.miChangeProfile = New ToolStripMenuItem()
            Me.miSwitchUser = New ToolStripMenuItem()

            Me.miCompanyFiscalYears = New ToolStripMenuItem()

            Me.miAccountingMain = New ToolStripMenuItem()
            Me.miReportsAccounting = New ToolStripMenuItem()

            Me.miTradeMini = New ToolStripMenuItem()
            Me.miTradeMedium = New ToolStripMenuItem()
            Me.miTradeBig = New ToolStripMenuItem()
            Me.miTradeWarehouseMain = New ToolStripMenuItem()
            Me.miReportsTrade = New ToolStripMenuItem()

            Me.miShellGeneral = New ToolStripMenuItem()
            Me.miShellRetail = New ToolStripMenuItem()
            Me.miShellServices = New ToolStripMenuItem()

            Me.miUtilCalculator = New ToolStripMenuItem()
            Me.miUtilNotes = New ToolStripMenuItem()
            Me.miUtilCalendar = New ToolStripMenuItem()

            Me.pnlToolBar = New Panel()
            Me.tool = New ToolStrip()
            Me.btnToolSystemMgmt = New ToolStripButton()
            Me.btnToolUserMgmt = New ToolStripButton()
            Me.btnToolCompanyMgmt = New ToolStripButton()
            Me.btnToolAccounting = New ToolStripButton()
            Me.btnToolTradeWarehouse = New ToolStripButton()
            Me.btnToolPayroll = New ToolStripButton()
            Me.btnToolAmval = New ToolStripButton()
            Me.btnToolAutomation = New ToolStripButton()
            Me.btnToolCrm = New ToolStripButton()
            Me.btnToolTreasury = New ToolStripButton()
            Me.btnToolBudgeting = New ToolStripButton()
            Me.btnToolProduction = New ToolStripButton()
            Me.btnToolProject = New ToolStripButton()
            Me.btnToolKpi = New ToolStripButton()
            Me.btnToolBusinessShells = New ToolStripButton()
            Me.btnToolUtilities = New ToolStripButton()

            Me.flpDashboard = New FlowLayoutPanel()

            Me.status = New StatusStrip()
            Me.lblUser = New ToolStripStatusLabel()
            Me.lblSep1 = New ToolStripSeparator()
            Me.lblCompany = New ToolStripStatusLabel()
            Me.lblSep2 = New ToolStripSeparator()
            Me.lblFiscalYear = New ToolStripStatusLabel()
            Me.lblSpring = New ToolStripStatusLabel()
            Me.lblDateTime = New ToolStripStatusLabel()
            Me.clockTimer = New Timer(Me.components)

            Me.mainMenu.SuspendLayout()
            Me.tool.SuspendLayout()
            Me.status.SuspendLayout()
            Me.SuspendLayout()

            ' pnlMainMenuContainer (Scrollable Container for Top MenuStrip)
            Me.pnlMainMenuContainer.AutoScroll = True
            Me.pnlMainMenuContainer.Dock = DockStyle.Top
            Me.pnlMainMenuContainer.Height = 44
            Me.pnlMainMenuContainer.Location = New Point(0, 0)
            Me.pnlMainMenuContainer.Name = "pnlMainMenuContainer"
            Me.pnlMainMenuContainer.RightToLeft = RightToLeft.Yes
            Me.pnlMainMenuContainer.TabIndex = 1
            Me.pnlMainMenuContainer.Controls.Add(Me.mainMenu)

            ' mainMenu
            Me.mainMenu.AutoSize = True
            Me.mainMenu.CanOverflow = True
            Me.mainMenu.Dock = DockStyle.None
            Me.mainMenu.Font = New Font("Tahoma", 9.0!)
            Me.mainMenu.Items.AddRange(New ToolStripItem() {Me.mSystemMgmt, Me.mUserMgmt, Me.mCompanyMgmt, Me.mAccounting, Me.mTradeWarehouse, Me.mPayroll, Me.mAmval, Me.mAutomation, Me.mCrm, Me.mTreasury, Me.mBudgeting, Me.mProduction, Me.mProject, Me.mKpi, Me.mBusinessShells, Me.mUtilities})
            Me.mainMenu.Location = New Point(0, 0)
            Me.mainMenu.Name = "mainMenu"
            Me.mainMenu.RightToLeft = RightToLeft.Yes
            Me.mainMenu.TabIndex = 1

            ' mPayroll
            Me.mPayroll.DropDownItems.AddRange(New ToolStripItem() {Me.miPayrollMain, Me.miPayrollReports})
            Me.mPayroll.Name = "mPayroll"
            Me.mPayroll.Size = New Size(100, 20)
            Me.mPayroll.Text = "💳 حقوق و دستمزد"

            ' miPayrollMain
            Me.miPayrollMain.Name = "miPayrollMain"
            Me.miPayrollMain.Size = New Size(240, 22)
            Me.miPayrollMain.Text = "💳 سیستم جامع حقوق و دستمزد"

            ' miPayrollReports
            Me.miPayrollReports.Name = "miPayrollReports"
            Me.miPayrollReports.Size = New Size(240, 22)
            Me.miPayrollReports.Text = "📊 گزارشات جامع حقوق و دستمزد"

            ' mAmval
            Me.mAmval.DropDownItems.AddRange(New ToolStripItem() {Me.miAmvalMain, Me.miAmvalReports})
            Me.mAmval.Name = "mAmval"
            Me.mAmval.Size = New Size(90, 20)
            Me.mAmval.Text = "🏛️ اموال"

            ' miAmvalMain
            Me.miAmvalMain.Name = "miAmvalMain"
            Me.miAmvalMain.Size = New Size(220, 22)
            Me.miAmvalMain.Text = "🏛️ سیستم جامع اموال"

            ' miAmvalReports
            Me.miAmvalReports.Name = "miAmvalReports"
            Me.miAmvalReports.Size = New Size(220, 22)
            Me.miAmvalReports.Text = "📊 گزارشات جامع اموال"

            ' mAutomation
            Me.mAutomation.DropDownItems.AddRange(New ToolStripItem() {Me.miAutomationMain, Me.miAutomationReports})
            Me.mAutomation.Name = "mAutomation"
            Me.mAutomation.Size = New Size(120, 20)
            Me.mAutomation.Text = "📨 اتوماسیون اداری"

            ' miAutomationMain
            Me.miAutomationMain.Name = "miAutomationMain"
            Me.miAutomationMain.Size = New Size(240, 22)
            Me.miAutomationMain.Text = "📨 سیستم جامع اتوماسیون اداری"

            ' miAutomationReports
            Me.miAutomationReports.Name = "miAutomationReports"
            Me.miAutomationReports.Size = New Size(240, 22)
            Me.miAutomationReports.Text = "📊 گزارشات جامع اتوماسیون اداری"

            ' mCrm
            Me.mCrm.DropDownItems.AddRange(New ToolStripItem() {Me.miCrmMain, Me.miCrmReports})
            Me.mCrm.Name = "mCrm"
            Me.mCrm.Size = New Size(120, 20)
            Me.mCrm.Text = "🤝 باشگاه مشتریان"

            ' miCrmMain
            Me.miCrmMain.Name = "miCrmMain"
            Me.miCrmMain.Size = New Size(280, 22)
            Me.miCrmMain.Text = "🤝 سیستم جامع باشگاه مشتریان (CRM)"

            ' miCrmReports
            Me.miCrmReports.Name = "miCrmReports"
            Me.miCrmReports.Size = New Size(280, 22)
            Me.miCrmReports.Text = "📊 گزارشات جامع باشگاه مشتریان"

            ' mTreasury
            Me.mTreasury.DropDownItems.AddRange(New ToolStripItem() {Me.miTreasuryMain, Me.miTreasuryReports})
            Me.mTreasury.Name = "mTreasury"
            Me.mTreasury.Size = New Size(100, 20)
            Me.mTreasury.Text = "💰 خزانه‌داری"

            ' miTreasuryMain
            Me.miTreasuryMain.Name = "miTreasuryMain"
            Me.miTreasuryMain.Size = New Size(280, 22)
            Me.miTreasuryMain.Text = "💰 سیستم جامع خزانه‌داری و جریان نقدینگی"

            ' miTreasuryReports
            Me.miTreasuryReports.Name = "miTreasuryReports"
            Me.miTreasuryReports.Size = New Size(280, 22)
            Me.miTreasuryReports.Text = "📊 گزارشات جامع خزانه‌داری و Cash Flow"

            ' mBudgeting
            Me.mBudgeting.DropDownItems.AddRange(New ToolStripItem() {Me.miBudgetingMain, Me.miBudgetingReports})
            Me.mBudgeting.Name = "mBudgeting"
            Me.mBudgeting.Size = New Size(130, 20)
            Me.mBudgeting.Text = "📊 بودجه و هزینه"

            ' miBudgetingMain
            Me.miBudgetingMain.Name = "miBudgetingMain"
            Me.miBudgetingMain.Size = New Size(280, 22)
            Me.miBudgetingMain.Text = "📊 سیستم جامع بودجه و کنترل هزینه‌ها"

            ' miBudgetingReports
            Me.miBudgetingReports.Name = "miBudgetingReports"
            Me.miBudgetingReports.Size = New Size(280, 22)
            Me.miBudgetingReports.Text = "📈 گزارشات انحراف بودجه و انضباط مالی"

            ' mProduction
            Me.mProduction.DropDownItems.AddRange(New ToolStripItem() {Me.miProductionMain, Me.miProductionReports})
            Me.mProduction.Name = "mProduction"
            Me.mProduction.Size = New Size(140, 20)
            Me.mProduction.Text = "🏭 تولید و بهای تمام‌شده"

            ' miProductionMain
            Me.miProductionMain.Name = "miProductionMain"
            Me.miProductionMain.Size = New Size(300, 22)
            Me.miProductionMain.Text = "🏭 سیستم جامع بهای تمام‌شده و برنامه‌ریزی تولید"

            ' miProductionReports
            Me.miProductionReports.Name = "miProductionReports"
            Me.miProductionReports.Size = New Size(300, 22)
            Me.miProductionReports.Text = "📊 گزارشات جامع بهای تمام‌شده و آنالیز BOM"

            ' mProject
            Me.mProject.DropDownItems.AddRange(New ToolStripItem() {Me.miProjectMain, Me.miProjectReports})
            Me.mProject.Name = "mProject"
            Me.mProject.Size = New Size(150, 20)
            Me.mProject.Text = "🏗️ پروژه‌ها و پیمان‌ها"

            ' miProjectMain
            Me.miProjectMain.Name = "miProjectMain"
            Me.miProjectMain.Size = New Size(300, 22)
            Me.miProjectMain.Text = "🏗️ سیستم جامع مدیریت پروژه‌ها و پیمان‌ها"

            ' miProjectReports
            Me.miProjectReports.Name = "miProjectReports"
            Me.miProjectReports.Size = New Size(300, 22)
            Me.miProjectReports.Text = "📊 گزارشات جامع پروژه‌ها و پیمان‌ها"

            ' mKpi
            Me.mKpi.DropDownItems.AddRange(New ToolStripItem() {Me.miKpiMain, Me.miKpiReports})
            Me.mKpi.Name = "mKpi"
            Me.mKpi.Size = New Size(145, 20)
            Me.mKpi.Text = "🎯 ارزیابی عملکرد و پاداش"

            ' miKpiMain
            Me.miKpiMain.Name = "miKpiMain"
            Me.miKpiMain.Size = New Size(320, 22)
            Me.miKpiMain.Text = "🎯 سیستم جامع ارزیابی عملکرد و پاداش (KPI)"

            ' miKpiReports
            Me.miKpiReports.Name = "miKpiReports"
            Me.miKpiReports.Size = New Size(320, 22)
            Me.miKpiReports.Text = "📊 گزارشات جامع ارزیابی عملکرد و کارانه"

            ' mSystemMgmt
            Me.mSystemMgmt.DropDownItems.AddRange(New ToolStripItem() {Me.miSettingsMessages, Me.miSettingsThemes, Me.miSepSys1, Me.miBackupData, Me.miRestoreData, Me.miSepSys2, Me.miCreateRelease, Me.miCreateUpdate, Me.miExportDecryptedDb, Me.miSepSys3, Me.miLock, Me.miSepSys4, Me.miAbout, Me.miContact, Me.miExit})
            Me.mSystemMgmt.Name = "mSystemMgmt"
            Me.mSystemMgmt.Size = New Size(93, 20)
            Me.mSystemMgmt.Text = "سیستم"

            ' miSettingsMessages
            Me.miSettingsMessages.Name = "miSettingsMessages"
            Me.miSettingsMessages.Size = New Size(245, 22)
            Me.miSettingsMessages.Text = "مدیریت پیامهای : درباره... و ارتباط با ما"

            ' miSettingsThemes
            Me.miSettingsThemes.Name = "miSettingsThemes"
            Me.miSettingsThemes.Size = New Size(245, 22)
            Me.miSettingsThemes.Text = "مدیریت تمهای برنامه و فرمها"

            ' miSepSys1
            Me.miSepSys1.Name = "miSepSys1"
            Me.miSepSys1.Size = New Size(242, 6)

            ' miBackupData
            Me.miBackupData.Name = "miBackupData"
            Me.miBackupData.Size = New Size(245, 22)
            Me.miBackupData.Text = "پشتیبان‌گیری اطلاعات"

            ' miRestoreData
            Me.miRestoreData.Name = "miRestoreData"
            Me.miRestoreData.Size = New Size(245, 22)
            Me.miRestoreData.Text = "بازیابی اطلاعات"

            ' miSepSys2
            Me.miSepSys2.Name = "miSepSys2"
            Me.miSepSys2.Size = New Size(242, 6)

            ' miCreateRelease
            Me.miCreateRelease.Name = "miCreateRelease"
            Me.miCreateRelease.Size = New Size(245, 22)
            Me.miCreateRelease.Text = "ایجاد نسخه قابل انتشار"

            ' miCreateUpdate
            Me.miCreateUpdate.Name = "miCreateUpdate"
            Me.miCreateUpdate.Size = New Size(245, 22)
            Me.miCreateUpdate.Text = "ایجاد بسته به‌روزرسانی (Update)"

            ' miExportDecryptedDb
            Me.miExportDecryptedDb.Name = "miExportDecryptedDb"
            Me.miExportDecryptedDb.Size = New Size(245, 22)
            Me.miExportDecryptedDb.Text = "خروجی دیتابیس بدون رمز جهت بازرسی"

            ' miSepSys3
            Me.miSepSys3.Name = "miSepSys3"
            Me.miSepSys3.Size = New Size(242, 6)

            ' miLock
            Me.miLock.Name = "miLock"
            Me.miLock.Size = New Size(245, 22)
            Me.miLock.Text = "قفل موقت برنامه (Ctrl+Alt+L)"

            ' miSepSys4
            Me.miSepSys4.Name = "miSepSys4"
            Me.miSepSys4.Size = New Size(242, 6)

            ' miAbout
            Me.miAbout.Name = "miAbout"
            Me.miAbout.Size = New Size(245, 22)
            Me.miAbout.Text = "درباره..."

            ' miContact
            Me.miContact.Name = "miContact"
            Me.miContact.Size = New Size(245, 22)
            Me.miContact.Text = "ارتباط با ما"

            ' miExit
            Me.miExit.Name = "miExit"
            Me.miExit.Size = New Size(245, 22)
            Me.miExit.Text = "خروج"

            ' mUserMgmt
            Me.mUserMgmt.DropDownItems.AddRange(New ToolStripItem() {Me.miUsers, Me.miBasicUsers, Me.miSepUser1, Me.miChangeProfile, Me.miSwitchUser})
            Me.mUserMgmt.Name = "mUserMgmt"
            Me.mUserMgmt.Size = New Size(94, 20)
            Me.mUserMgmt.Text = "کاربران"

            ' miUsers
            Me.miUsers.Name = "miUsers"
            Me.miUsers.Size = New Size(220, 22)
            Me.miUsers.Text = "مدیریت کاربران (جامع)"

            ' miBasicUsers
            Me.miBasicUsers.Name = "miBasicUsers"
            Me.miBasicUsers.Size = New Size(220, 22)
            Me.miBasicUsers.Text = "مدیریت کاربران عادی"

            ' miSepUser1
            Me.miSepUser1.Name = "miSepUser1"
            Me.miSepUser1.Size = New Size(217, 6)

            ' miChangeProfile
            Me.miChangeProfile.Name = "miChangeProfile"
            Me.miChangeProfile.Size = New Size(220, 22)
            Me.miChangeProfile.Text = "تغییر نام کاربری و رمز عبور"

            ' miSwitchUser
            Me.miSwitchUser.Name = "miSwitchUser"
            Me.miSwitchUser.Size = New Size(220, 22)
            Me.miSwitchUser.Text = "ورود با نام کاربری دیگر"

            ' mCompanyMgmt
            Me.mCompanyMgmt.DropDownItems.AddRange(New ToolStripItem() {Me.miCompanyFiscalYears})
            Me.mCompanyMgmt.Name = "mCompanyMgmt"
            Me.mCompanyMgmt.Size = New Size(110, 20)
            Me.mCompanyMgmt.Text = "شرکت‌ها و سال‌ها"

            ' miCompanyFiscalYears
            Me.miCompanyFiscalYears.Name = "miCompanyFiscalYears"
            Me.miCompanyFiscalYears.Size = New Size(230, 22)
            Me.miCompanyFiscalYears.Text = "شرکت ها و سالهای مالی"

            ' mAccounting
            Me.mAccounting.DropDownItems.AddRange(New ToolStripItem() {Me.miAccountingMain, Me.miReportsAccounting})
            Me.mAccounting.Name = "mAccounting"
            Me.mAccounting.Size = New Size(67, 20)
            Me.mAccounting.Text = "حسابداری"

            ' miAccountingMain
            Me.miAccountingMain.Name = "miAccountingMain"
            Me.miAccountingMain.Size = New Size(260, 22)
            Me.miAccountingMain.Text = "کدینگ، ثبت اسناد و دفاتر حسابداری"

            ' miReportsAccounting
            Me.miReportsAccounting.Name = "miReportsAccounting"
            Me.miReportsAccounting.Size = New Size(260, 22)
            Me.miReportsAccounting.Text = "گزارشات و ترازهای حسابداری"

            ' mTradeWarehouse
            Me.mTradeWarehouse.DropDownItems.AddRange(New ToolStripItem() {Me.miTradeMini, Me.miTradeMedium, Me.miTradeBig, Me.miTradeWarehouseMain, Me.miReportsTrade})
            Me.mTradeWarehouse.Name = "mTradeWarehouse"
            Me.mTradeWarehouse.Size = New Size(95, 20)
            Me.mTradeWarehouse.Text = "انبار و فروش"

            ' miTradeMini
            Me.miTradeMini.Name = "miTradeMini"
            Me.miTradeMini.Size = New Size(260, 22)
            Me.miTradeMini.Text = "استفاده از انبارداری مینی"

            ' miTradeMedium
            Me.miTradeMedium.Name = "miTradeMedium"
            Me.miTradeMedium.Size = New Size(260, 22)
            Me.miTradeMedium.Text = "استفاده از انبارداری متوسط"

            ' miTradeBig
            Me.miTradeBig.Name = "miTradeBig"
            Me.miTradeBig.Size = New Size(260, 22)
            Me.miTradeBig.Text = "استفاده از انبارداری پیشرفته"

            ' miTradeWarehouseMain
            Me.miTradeWarehouseMain.Name = "miTradeWarehouseMain"
            Me.miTradeWarehouseMain.Size = New Size(260, 22)
            Me.miTradeWarehouseMain.Text = "فاکتورها، انبار و مدیریت کالاها (جامع)"

            ' miReportsTrade
            Me.miReportsTrade.Name = "miReportsTrade"
            Me.miReportsTrade.Size = New Size(250, 22)
            Me.miReportsTrade.Text = "گزارشات فاکتورها و موجودی انبار"

            ' mBusinessShells
            Me.mBusinessShells.DropDownItems.AddRange(New ToolStripItem() {Me.miShellGeneral, Me.miShellRetail, Me.miShellServices})
            Me.mBusinessShells.Name = "mBusinessShells"
            Me.mBusinessShells.Size = New Size(81, 20)
            Me.mBusinessShells.Text = "پوسته مشاغل"

            ' miShellGeneral
            Me.miShellGeneral.Name = "miShellGeneral"
            Me.miShellGeneral.Size = New Size(210, 22)
            Me.miShellGeneral.Text = "پوسته عمومی و بازرگانی"

            ' miShellRetail
            Me.miShellRetail.Name = "miShellRetail"
            Me.miShellRetail.Size = New Size(210, 22)
            Me.miShellRetail.Text = "پوسته فروشگاهی و اصناف"

            ' miShellServices
            Me.miShellServices.Name = "miShellServices"
            Me.miShellServices.Size = New Size(210, 22)
            Me.miShellServices.Text = "پوسته خدماتی و شرکتی"

            ' mUtilities
            Me.mUtilities.DropDownItems.AddRange(New ToolStripItem() {Me.miUtilCalculator, Me.miUtilNotes, Me.miUtilCalendar})
            Me.mUtilities.Name = "mUtilities"
            Me.mUtilities.Size = New Size(62, 20)
            Me.mUtilities.Text = "امکانات"

            ' miUtilCalculator
            Me.miUtilCalculator.Name = "miUtilCalculator"
            Me.miUtilCalculator.Size = New Size(200, 22)
            Me.miUtilCalculator.Text = "ماشین حساب سیستم"

            ' miUtilNotes
            Me.miUtilNotes.Name = "miUtilNotes"
            Me.miUtilNotes.Size = New Size(200, 22)
            Me.miUtilNotes.Text = "دفترچه یادداشت"

            ' miUtilCalendar
            Me.miUtilCalendar.Name = "miUtilCalendar"
            Me.miUtilCalendar.Size = New Size(200, 22)
            Me.miUtilCalendar.Text = "تقویم و مناسبت‌ها"

            ' tool (ToolStrip Toolbar for Top-Level Main Menu Titles)
            Me.tool.AutoSize = True
            Me.tool.Dock = DockStyle.None
            Me.tool.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.tool.ImageScalingSize = New Size(28, 28)
            Me.tool.Items.AddRange(New ToolStripItem() {Me.btnToolSystemMgmt, Me.btnToolUserMgmt, Me.btnToolCompanyMgmt, Me.btnToolAccounting, Me.btnToolTradeWarehouse, Me.btnToolPayroll, Me.btnToolAmval, Me.btnToolAutomation, Me.btnToolCrm, Me.btnToolTreasury, Me.btnToolBudgeting, Me.btnToolProduction, Me.btnToolProject, Me.btnToolKpi, Me.btnToolBusinessShells, Me.btnToolUtilities})

            ' pnlToolBar (Scrollable Container for Dashboard Category Buttons Toolbar)
            Me.pnlToolBar.AutoScroll = True
            Me.pnlToolBar.Dock = DockStyle.Top
            Me.pnlToolBar.Height = 60
            Me.pnlToolBar.Location = New Point(0, 24)
            Me.pnlToolBar.Name = "pnlToolBar"
            Me.pnlToolBar.RightToLeft = RightToLeft.Yes
            Me.pnlToolBar.TabIndex = 0
            Me.pnlToolBar.Controls.Add(Me.tool)

            ' btnToolPayroll
            Me.btnToolPayroll.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolPayroll.Name = "btnToolPayroll"
            Me.btnToolPayroll.Size = New Size(115, 51)
            Me.btnToolPayroll.Text = "حقوق و دستمزد"

            ' btnToolAmval
            Me.btnToolAmval.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolAmval.Name = "btnToolAmval"
            Me.btnToolAmval.Size = New Size(95, 51)
            Me.btnToolAmval.Text = "اموال"

            ' btnToolAutomation
            Me.btnToolAutomation.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolAutomation.Name = "btnToolAutomation"
            Me.btnToolAutomation.Size = New Size(125, 51)
            Me.btnToolAutomation.Text = "اتوماسیون اداری"

            ' btnToolCrm
            Me.btnToolCrm.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolCrm.Name = "btnToolCrm"
            Me.btnToolCrm.Size = New Size(130, 51)
            Me.btnToolCrm.Text = "باشگاه مشتریان"

            ' btnToolTreasury
            Me.btnToolTreasury.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolTreasury.Name = "btnToolTreasury"
            Me.btnToolTreasury.Size = New Size(105, 51)
            Me.btnToolTreasury.Text = "خزانه‌داری"

            ' btnToolBudgeting
            Me.btnToolBudgeting.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolBudgeting.Name = "btnToolBudgeting"
            Me.btnToolBudgeting.Size = New Size(130, 51)
            Me.btnToolBudgeting.Text = "بودجه و هزینه"

            ' btnToolProduction
            Me.btnToolProduction.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolProduction.Name = "btnToolProduction"
            Me.btnToolProduction.Size = New Size(160, 51)
            Me.btnToolProduction.Text = "تولید و بهای تمام‌شده"

            ' btnToolProject
            Me.btnToolProject.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolProject.Name = "btnToolProject"
            Me.btnToolProject.Size = New Size(150, 51)
            Me.btnToolProject.Text = "پروژه‌ها و پیمان‌ها"

            ' btnToolKpi
            Me.btnToolKpi.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolKpi.Name = "btnToolKpi"
            Me.btnToolKpi.Size = New Size(170, 51)
            Me.btnToolKpi.Text = "ارزیابی عملکرد و پاداش"
            Me.tool.Location = New Point(0, 0)
            Me.tool.Name = "tool"
            Me.tool.RightToLeft = RightToLeft.Yes
            Me.tool.TabIndex = 0

            ' btnToolSystemMgmt
            Me.btnToolSystemMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolSystemMgmt.Name = "btnToolSystemMgmt"
            Me.btnToolSystemMgmt.Size = New Size(105, 51)
            Me.btnToolSystemMgmt.Text = "سیستم"

            ' btnToolUserMgmt
            Me.btnToolUserMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolUserMgmt.Name = "btnToolUserMgmt"
            Me.btnToolUserMgmt.Size = New Size(106, 51)
            Me.btnToolUserMgmt.Text = "کاربران"

            ' btnToolCompanyMgmt
            Me.btnToolCompanyMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolCompanyMgmt.Name = "btnToolCompanyMgmt"
            Me.btnToolCompanyMgmt.Size = New Size(160, 51)
            Me.btnToolCompanyMgmt.Text = "شرکت ها و سالهای مالی"

            ' btnToolAccounting
            Me.btnToolAccounting.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolAccounting.Name = "btnToolAccounting"
            Me.btnToolAccounting.Size = New Size(77, 51)
            Me.btnToolAccounting.Text = "حسابداری"

            ' btnToolTradeWarehouse
            Me.btnToolTradeWarehouse.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolTradeWarehouse.Name = "btnToolTradeWarehouse"
            Me.btnToolTradeWarehouse.Size = New Size(145, 51)
            Me.btnToolTradeWarehouse.Text = "خریدو فروش و انبارداری"

            ' btnToolBusinessShells
            Me.btnToolBusinessShells.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolBusinessShells.Name = "btnToolBusinessShells"
            Me.btnToolBusinessShells.Size = New Size(91, 51)
            Me.btnToolBusinessShells.Text = "پوسته مشاغل"

            ' btnToolUtilities
            Me.btnToolUtilities.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolUtilities.Name = "btnToolUtilities"
            Me.btnToolUtilities.Size = New Size(67, 51)
            Me.btnToolUtilities.Text = "امکانات"

            ' flpDashboard
            Me.flpDashboard.AutoScroll = True
            Me.flpDashboard.Dock = DockStyle.Fill
            Me.flpDashboard.Location = New Point(0, 79)
            Me.flpDashboard.Name = "flpDashboard"
            Me.flpDashboard.Padding = New Padding(30)
            Me.flpDashboard.RightToLeft = RightToLeft.Yes
            Me.flpDashboard.Size = New Size(1200, 597)
            Me.flpDashboard.TabIndex = 3

            ' status
            Me.status.Items.AddRange(New ToolStripItem() {Me.lblUser, Me.lblSep1, Me.lblCompany, Me.lblSep2, Me.lblFiscalYear, Me.lblSpring, Me.lblDateTime})
            Me.status.Location = New Point(0, 676)
            Me.status.Name = "status"
            Me.status.RightToLeft = RightToLeft.Yes
            Me.status.Size = New Size(1200, 24)
            Me.status.TabIndex = 2

            ' lblUser
            Me.lblUser.Name = "lblUser"
            Me.lblUser.Size = New Size(77, 19)
            Me.lblUser.Text = "کاربر جاری: -"

            ' lblSep1
            Me.lblSep1.Name = "lblSep1"
            Me.lblSep1.Size = New Size(6, 24)

            ' lblCompany
            Me.lblCompany.Name = "lblCompany"
            Me.lblCompany.Size = New Size(79, 19)
            Me.lblCompany.Text = "شرکت جاری: -"

            ' lblSep2
            Me.lblSep2.Name = "lblSep2"
            Me.lblSep2.Size = New Size(6, 24)

            ' lblFiscalYear
            Me.lblFiscalYear.Name = "lblFiscalYear"
            Me.lblFiscalYear.Size = Me.lblCompany.Size

            ' lblSpring
            Me.lblSpring.Name = "lblSpring"
            Me.lblSpring.Size = New Size(800, 19)
            Me.lblSpring.Spring = True

            ' lblDateTime
            Me.lblDateTime.Name = "lblDateTime"
            Me.lblDateTime.Size = New Size(60, 19)
            Me.lblDateTime.Text = "ساعت و تاریخ"

            ' Form settings
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1200, 700)
            Me.Controls.Add(Me.flpDashboard)
            Me.Controls.Add(Me.status)
            Me.Controls.Add(Me.pnlToolBar)
            Me.Controls.Add(Me.pnlMainMenuContainer)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.MainMenuStrip = Me.mainMenu
            Me.Name = "MainForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "سیستم یکپارچه مالی، حسابداری، انبارداری و خرید و فروش"
            Me.WindowState = FormWindowState.Maximized

            Me.mainMenu.ResumeLayout(False)
            Me.mainMenu.PerformLayout()
            Me.tool.ResumeLayout(False)
            Me.tool.PerformLayout()
            Me.status.ResumeLayout(False)
            Me.status.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace

