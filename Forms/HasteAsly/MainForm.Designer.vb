Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class MainForm
        Inherits Form

        Private components As IContainer

        ' Main Top-Level Menus
        Friend WithEvents mainMenu As MenuStrip
        Friend WithEvents mSystemMgmt As ToolStripMenuItem
        Friend WithEvents mUserMgmt As ToolStripMenuItem
        Friend WithEvents mCompanyMgmt As ToolStripMenuItem
        Friend WithEvents mAccounting As ToolStripMenuItem
        Friend WithEvents mTradeWarehouse As ToolStripMenuItem
        Friend WithEvents mBusinessShells As ToolStripMenuItem
        Friend WithEvents mUtilities As ToolStripMenuItem

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
        Friend WithEvents tool As ToolStrip
        Friend WithEvents btnToolSystemMgmt As ToolStripButton
        Friend WithEvents btnToolUserMgmt As ToolStripButton
        Friend WithEvents btnToolCompanyMgmt As ToolStripButton
        Friend WithEvents btnToolAccounting As ToolStripButton
        Friend WithEvents btnToolTradeWarehouse As ToolStripButton
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
            Me.mainMenu = New MenuStrip()
            Me.mSystemMgmt = New ToolStripMenuItem()
            Me.mUserMgmt = New ToolStripMenuItem()
            Me.mCompanyMgmt = New ToolStripMenuItem()
            Me.mAccounting = New ToolStripMenuItem()
            Me.mTradeWarehouse = New ToolStripMenuItem()
            Me.mBusinessShells = New ToolStripMenuItem()
            Me.mUtilities = New ToolStripMenuItem()

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

            Me.miTradeWarehouseMain = New ToolStripMenuItem()
            Me.miReportsTrade = New ToolStripMenuItem()

            Me.miShellGeneral = New ToolStripMenuItem()
            Me.miShellRetail = New ToolStripMenuItem()
            Me.miShellServices = New ToolStripMenuItem()

            Me.miUtilCalculator = New ToolStripMenuItem()
            Me.miUtilNotes = New ToolStripMenuItem()
            Me.miUtilCalendar = New ToolStripMenuItem()

            Me.tool = New ToolStrip()
            Me.btnToolSystemMgmt = New ToolStripButton()
            Me.btnToolUserMgmt = New ToolStripButton()
            Me.btnToolCompanyMgmt = New ToolStripButton()
            Me.btnToolAccounting = New ToolStripButton()
            Me.btnToolTradeWarehouse = New ToolStripButton()
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

            ' mainMenu
            Me.mainMenu.Items.AddRange(New ToolStripItem() {Me.mSystemMgmt, Me.mUserMgmt, Me.mCompanyMgmt, Me.mAccounting, Me.mTradeWarehouse, Me.mBusinessShells, Me.mUtilities})
            Me.mainMenu.Location = New Point(0, 0)
            Me.mainMenu.Name = "mainMenu"
            Me.mainMenu.RightToLeft = RightToLeft.Yes
            Me.mainMenu.Size = New Size(1200, 24)
            Me.mainMenu.TabIndex = 1

            ' mSystemMgmt
            Me.mSystemMgmt.DropDownItems.AddRange(New ToolStripItem() {Me.miSettingsMessages, Me.miSettingsThemes, Me.miSepSys1, Me.miBackupData, Me.miRestoreData, Me.miSepSys2, Me.miCreateRelease, Me.miCreateUpdate, Me.miExportDecryptedDb, Me.miSepSys3, Me.miLock, Me.miSepSys4, Me.miAbout, Me.miContact, Me.miExit})
            Me.mSystemMgmt.Name = "mSystemMgmt"
            Me.mSystemMgmt.Size = New Size(93, 20)
            Me.mSystemMgmt.Text = "مدیریت سیستم"

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
            Me.mUserMgmt.Text = "مدیریت کاربران"

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
            Me.mCompanyMgmt.Size = New Size(140, 20)
            Me.mCompanyMgmt.Text = "مدیریت شرکت و سال مالی"

            ' miCompanyFiscalYears
            Me.miCompanyFiscalYears.Name = "miCompanyFiscalYears"
            Me.miCompanyFiscalYears.Size = New Size(230, 22)
            Me.miCompanyFiscalYears.Text = "مدیریت شرکت‌ها و سال‌های مالی"

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
            Me.mTradeWarehouse.DropDownItems.AddRange(New ToolStripItem() {Me.miTradeWarehouseMain, Me.miReportsTrade})
            Me.mTradeWarehouse.Name = "mTradeWarehouse"
            Me.mTradeWarehouse.Size = New Size(136, 20)
            Me.mTradeWarehouse.Text = "خریدو فروش و انبارداری"

            ' miTradeWarehouseMain
            Me.miTradeWarehouseMain.Name = "miTradeWarehouseMain"
            Me.miTradeWarehouseMain.Size = New Size(250, 22)
            Me.miTradeWarehouseMain.Text = "فاکتورها، انبار و مدیریت کالاها"

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
            Me.tool.AutoSize = False
            Me.tool.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.tool.ImageScalingSize = New Size(28, 28)
            Me.tool.Items.AddRange(New ToolStripItem() {Me.btnToolSystemMgmt, Me.btnToolUserMgmt, Me.btnToolCompanyMgmt, Me.btnToolAccounting, Me.btnToolTradeWarehouse, Me.btnToolBusinessShells, Me.btnToolUtilities})
            Me.tool.Location = New Point(0, 24)
            Me.tool.Name = "tool"
            Me.tool.RightToLeft = RightToLeft.Yes
            Me.tool.Size = New Size(1200, 55)
            Me.tool.TabIndex = 0

            ' btnToolSystemMgmt
            Me.btnToolSystemMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolSystemMgmt.Name = "btnToolSystemMgmt"
            Me.btnToolSystemMgmt.Size = New Size(105, 51)
            Me.btnToolSystemMgmt.Text = "مدیریت سیستم"

            ' btnToolUserMgmt
            Me.btnToolUserMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolUserMgmt.Name = "btnToolUserMgmt"
            Me.btnToolUserMgmt.Size = New Size(106, 51)
            Me.btnToolUserMgmt.Text = "مدیریت کاربران"

            ' btnToolCompanyMgmt
            Me.btnToolCompanyMgmt.Margin = New Padding(4, 2, 4, 2)
            Me.btnToolCompanyMgmt.Name = "btnToolCompanyMgmt"
            Me.btnToolCompanyMgmt.Size = New Size(160, 51)
            Me.btnToolCompanyMgmt.Text = "مدیریت شرکت و سال مالی"

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
            Me.Controls.Add(Me.tool)
            Me.Controls.Add(Me.mainMenu)
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
