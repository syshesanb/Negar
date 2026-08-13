Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data
Imports Negar.Models

Namespace Negar.Forms
    Partial Class MainForm
        Inherits Form

        Private _currentUser As UserAccount
        Private _isLocked As Boolean = False
        Private _shortcutFilter As GlobalShortcutFilter
        Private ReadOnly _spawnedCalcProcesses As New List(Of Process)()

        ' Collapsible Right Sidebar Menu Logic with Handle Knob
        Private WithEvents menuTransitionTimer As New Timer() With {.Interval = 15}
        Private _isMenuExpanded As Boolean = True
        Private Const ExpandedWidth As Integer = 268
        Private Const CollapsedWidth As Integer = 28
        Private _targetWidth As Integer = ExpandedWidth

        ' Open Forms Tab Manager
        Private ReadOnly _openFormTabs As New Dictionary(Of Form, Button)()
        Private _btnHomeTab As Button

        Private Class GlobalShortcutFilter
            Implements IMessageFilter

            Private ReadOnly _mainForm As MainForm
            Private Const WM_KEYDOWN As Integer = &H100
            Private Const WM_SYSKEYDOWN As Integer = &H104

            Public Sub New(mainForm As MainForm)
                _mainForm = mainForm
            End Sub

            Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
                If m.Msg = WM_KEYDOWN OrElse m.Msg = WM_SYSKEYDOWN Then
                    Dim keyCode As Keys = CType(CInt(m.WParam), Keys)
                    Dim modifiers = Control.ModifierKeys
                    Dim isAlt = (modifiers And Keys.Alt) = Keys.Alt
                    Dim isCtrl = (modifiers And Keys.Control) = Keys.Control
                    Dim isShift = (modifiers And Keys.Shift) = Keys.Shift

                    ' Alt+S  =>  نمایش انتخابگر سریع سال مالی
                    If keyCode = Keys.S AndAlso isAlt AndAlso Not isCtrl AndAlso Not isShift Then
                        If Not _mainForm._isLocked Then
                            _mainForm.BeginInvoke(New Action(AddressOf _mainForm.ShowFiscalYearSelector))
                        End If
                        Return True
                    End If

                    ' Ctrl+Alt+L  یا  Ctrl+Shift+L  =>  قفل برنامه
                    If keyCode = Keys.L AndAlso isCtrl AndAlso (isAlt OrElse isShift) Then
                        If Not _mainForm._isLocked Then
                            _mainForm.BeginInvoke(New Action(AddressOf _mainForm.LockApplication))
                        End If
                        Return True
                    End If
                End If
                Return False
            End Function
        End Class

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(currentUser As UserAccount)
            _currentUser = currentUser
            InitializeComponent()
            SetupToolIcons()
            ApplySecurity()
            ApplyTheme()
        End Sub

        Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.IradLogger.Clear()
            Negar.Business.IradLogger.Log("MainForm_Load", $"MainForm loaded. Bounds: {Me.Bounds.Width}x{Me.Bounds.Height}, WindowState: {Me.WindowState}")
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            AppIconHelper.ApplyAppIcon(Me)
            UpdateStatusBar()
            clockTimer.Start()
            _shortcutFilter = New GlobalShortcutFilter(Me)
            Application.AddMessageFilter(_shortcutFilter)
            LoadRandomBackgroundImage()
            InitOpenFormsTabBar()
            BuildAccordionTree()
            UpdateSidebarBounds()
        End Sub

        Private Sub UpdateSidebarBounds()
            If pnlSidebarContainer IsNot Nothing AndAlso status IsNot Nothing Then
                pnlSidebarContainer.Location = New Point(0, 0)
                pnlSidebarContainer.Height = Math.Max(100, Me.ClientSize.Height - status.Height)
                pnlSidebarContainer.BringToFront()
            End If
        End Sub

        Private Sub MainForm_ResizeOrLayout(sender As Object, e As EventArgs) Handles MyBase.Resize, MyBase.Layout
            UpdateSidebarBounds()
        End Sub

        Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
            If _shortcutFilter IsNot Nothing Then
                Application.RemoveMessageFilter(_shortcutFilter)
            End If
            CloseSpawnedCalculators()
        End Sub

        Private Sub CloseSpawnedCalculators()
            For Each proc In _spawnedCalcProcesses
                Try
                    If Not proc.HasExited Then
                        proc.Kill()
                    End If
                Catch
                End Try
            Next
            _spawnedCalcProcesses.Clear()
        End Sub

        Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        End Sub

        Private Sub SetupToolIcons()
            btnToolSystemMgmt.Image = CreateModuleIcon("Settings", 24)
            btnToolUserMgmt.Image = CreateModuleIcon("Users", 24)
            btnToolCompanyMgmt.Image = CreateModuleIcon("CompanyFiscalYears", 24)
            btnToolAccounting.Image = CreateModuleIcon("Accounting", 24)
            btnToolTradeWarehouse.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolImportExport.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolPm.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolLogistics.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolSrm.Image = CreateModuleIcon("CompanyFiscalYears", 24)
            btnToolQc.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolBi.Image = CreateModuleIcon("Reports", 24)
            btnToolDms.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolSaham.Image = CreateModuleIcon("CompanyFiscalYears", 24)
            btnToolApi.Image = CreateModuleIcon("TradeWarehouse", 24)
            btnToolLegal.Image = CreateModuleIcon("CompanyFiscalYears", 24)
            btnToolRd.Image = CreateModuleIcon("Reports", 24)
            btnToolVoip.Image = CreateModuleIcon("Crm", 24)
            btnToolBusinessShells.Image = CreateModuleIcon("Home", 24)
            btnToolUtilities.Image = CreateModuleIcon("Reports", 24)
        End Sub

        Private Function CreateModuleIcon(iconType As String, Optional iconSize As Integer = 32) As Bitmap
            Dim bmp As New Bitmap(iconSize, iconSize)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                Dim scale As Single = CSng(iconSize) / 32.0F
                g.ScaleTransform(scale, scale)

                Select Case iconType
                    Case "Home"
                        Using b As New SolidBrush(Color.FromArgb(41, 128, 185))
                            Dim pts As Point() = {New Point(16, 4), New Point(4, 16), New Point(28, 16)}
                            g.FillPolygon(b, pts)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(52, 152, 219))
                            g.FillRectangle(b, 7, 16, 18, 12)
                        End Using
                        Using b As New SolidBrush(Color.White)
                            g.FillRectangle(b, 13, 20, 6, 8)
                        End Using

                    Case "Users"
                        Using b As New SolidBrush(Color.FromArgb(142, 68, 173))
                            g.FillEllipse(b, 6, 4, 10, 10)
                            g.FillPie(b, 2, 14, 18, 16, 180, 180)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(41, 128, 185))
                            g.FillEllipse(b, 17, 6, 10, 10)
                            g.FillPie(b, 13, 16, 18, 16, 180, 180)
                        End Using

                    Case "BasicUsers"
                        Using b As New SolidBrush(Color.FromArgb(22, 160, 133))
                            g.FillEllipse(b, 11, 4, 10, 10)
                            g.FillPie(b, 7, 14, 18, 16, 180, 180)
                        End Using
                        Using p As New Pen(Color.FromArgb(39, 174, 96), 2)
                            g.DrawEllipse(p, 4, 2, 24, 28)
                        End Using

                    Case "TradeWarehouse"
                        Using b As New SolidBrush(Color.FromArgb(230, 126, 34))
                            Dim pts As Point() = {New Point(16, 4), New Point(28, 10), New Point(16, 16), New Point(4, 10)}
                            g.FillPolygon(b, pts)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(211, 84, 0))
                            Dim pts1 As Point() = {New Point(4, 10), New Point(16, 16), New Point(16, 28), New Point(4, 22)}
                            g.FillPolygon(b, pts1)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(243, 156, 18))
                            Dim pts2 As Point() = {New Point(16, 16), New Point(28, 10), New Point(28, 22), New Point(16, 28)}
                            g.FillPolygon(b, pts2)
                        End Using

                    Case "Accounting"
                        Using b As New SolidBrush(Color.FromArgb(39, 174, 96))
                            g.FillRectangle(b, 5, 3, 22, 26)
                        End Using
                        Using b As New SolidBrush(Color.White)
                            g.FillRectangle(b, 8, 6, 16, 5)
                            g.FillRectangle(b, 8, 13, 4, 4)
                            g.FillRectangle(b, 14, 13, 4, 4)
                            g.FillRectangle(b, 20, 13, 4, 4)
                            g.FillRectangle(b, 8, 19, 4, 4)
                            g.FillRectangle(b, 14, 19, 4, 4)
                            g.FillRectangle(b, 20, 19, 4, 4)
                            g.FillRectangle(b, 8, 24, 10, 3)
                        End Using

                    Case "CompanyFiscalYears"
                        Using b As New SolidBrush(Color.FromArgb(192, 57, 43))
                            g.FillRectangle(b, 6, 6, 20, 23)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(241, 196, 15))
                            g.FillRectangle(b, 9, 9, 4, 4)
                            g.FillRectangle(b, 19, 9, 4, 4)
                            g.FillRectangle(b, 9, 15, 4, 4)
                            g.FillRectangle(b, 19, 15, 4, 4)
                            g.FillRectangle(b, 13, 21, 6, 8)
                        End Using

                    Case "Reports"
                        Using b As New SolidBrush(Color.FromArgb(52, 152, 219))
                            g.FillRectangle(b, 4, 18, 6, 10)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(155, 89, 182))
                            g.FillRectangle(b, 13, 10, 6, 18)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(46, 204, 113))
                            g.FillRectangle(b, 22, 4, 6, 24)
                        End Using

                    Case "Settings"
                        Using b As New SolidBrush(Color.FromArgb(127, 140, 141))
                            g.FillEllipse(b, 4, 4, 24, 24)
                        End Using
                        Using b As New SolidBrush(Color.White)
                            g.FillEllipse(b, 11, 11, 10, 10)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(52, 73, 94))
                            g.FillEllipse(b, 13, 13, 6, 6)
                        End Using

                    Case "ChangeProfile"
                        Using b As New SolidBrush(Color.FromArgb(52, 152, 219))
                            g.FillEllipse(b, 10, 4, 12, 12)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(41, 128, 185))
                            g.FillRectangle(b, 6, 18, 20, 10)
                        End Using

                    Case "CreateRelease"
                        Using b As New SolidBrush(Color.FromArgb(46, 204, 113))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using p As New Pen(Color.White, 3)
                            g.DrawLine(p, 16, 8, 16, 20)
                            g.DrawLine(p, 10, 14, 16, 20)
                            g.DrawLine(p, 22, 14, 16, 20)
                        End Using

                    Case "CreateUpdate"
                        Using b As New SolidBrush(Color.FromArgb(155, 89, 182))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using p As New Pen(Color.White, 3)
                            g.DrawLine(p, 16, 20, 16, 8)
                            g.DrawLine(p, 10, 14, 16, 8)
                            g.DrawLine(p, 22, 14, 16, 8)
                        End Using

                    Case "ExportDecryptedDb"
                        Using b As New SolidBrush(Color.FromArgb(52, 73, 94))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using p As New Pen(Color.White, 3)
                            g.DrawRectangle(p, 8, 10, 16, 12)
                            g.DrawLine(p, 12, 16, 20, 16)
                        End Using

                    Case "BackupData"
                        Using b As New SolidBrush(Color.FromArgb(39, 174, 96))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using p As New Pen(Color.White, 3)
                            g.DrawRectangle(p, 8, 8, 16, 16)
                            g.DrawLine(p, 12, 16, 20, 16)
                        End Using

                    Case "RestoreData"
                        Using b As New SolidBrush(Color.FromArgb(211, 84, 0))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using p As New Pen(Color.White, 3)
                            g.DrawRectangle(p, 8, 8, 16, 16)
                            g.DrawLine(p, 16, 12, 16, 20)
                        End Using

                    Case "SwitchUser"
                        Using b As New SolidBrush(Color.FromArgb(22, 160, 133))
                            g.FillRectangle(b, 4, 8, 16, 6)
                            Dim pts1 As Point() = {New Point(20, 4), New Point(28, 11), New Point(20, 18)}
                            g.FillPolygon(b, pts1)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(230, 126, 34))
                            g.FillRectangle(b, 12, 18, 16, 6)
                            Dim pts2 As Point() = {New Point(12, 14), New Point(4, 21), New Point(12, 28)}
                            g.FillPolygon(b, pts2)
                        End Using

                    Case "Lock"
                        Using b As New SolidBrush(Color.FromArgb(241, 196, 15))
                            g.FillRectangle(b, 8, 14, 16, 14)
                        End Using
                        Using p As New Pen(Color.FromArgb(241, 196, 15), 3)
                            g.DrawArc(p, 10, 6, 12, 14, 180, 180)
                        End Using
                        Using b As New SolidBrush(Color.FromArgb(44, 62, 80))
                            g.FillEllipse(b, 14, 18, 4, 4)
                            g.FillRectangle(b, 15, 20, 2, 4)
                        End Using

                    Case "Exit"
                        Using b As New SolidBrush(Color.FromArgb(231, 76, 60))
                            g.FillRectangle(b, 4, 4, 24, 24)
                        End Using
                        Using b As New SolidBrush(Color.White)
                            g.FillRectangle(b, 10, 8, 12, 16)
                        End Using
                End Select
            End Using
            Return bmp
        End Function

        Private Sub ApplySecurity()
            Dim isSuperAdmin = _currentUser IsNot Nothing AndAlso String.Equals(_currentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            Dim canUsers = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageUsers)
            Dim canBasicUsers = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageBasicUsers)
            Dim canTrade = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageTradeWarehouse) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageProducts) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageWarehouses) OrElse
                SessionContext.HasPermission(PermissionKeys.ManagePurchases) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageSales) OrElse
                SessionContext.HasPermission(PermissionKeys.ViewInventory) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeProducts) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeWarehouses) OrElse
                SessionContext.HasPermission(PermissionKeys.TradePurchase) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeSales) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeRemittance) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeReports) OrElse
                SessionContext.HasPermission(PermissionKeys.AnbarMiniModule) OrElse
                SessionContext.HasPermission(PermissionKeys.AnbarMediumModule) OrElse
                SessionContext.HasPermission(PermissionKeys.AnbarBigModule)

            Dim canAnbarMini = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule) OrElse (canTrade AndAlso Not (SessionContext.HasPermission(PermissionKeys.AnbarMediumModule) OrElse SessionContext.HasPermission(PermissionKeys.AnbarBigModule)))
            Dim canAnbarMedium = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMediumModule)
            Dim canAnbarBig = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarBigModule)

            Dim canAccounting = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageAccounting) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingHeader) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingShenavar) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingEntry) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingBank) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingBalance) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingLedger) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingReports)

            Dim canCompanyYears = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageCompaniesYears) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageCompanies) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageFiscalYears) OrElse
                SessionContext.HasPermission(PermissionKeys.SelectCompanyFiscalYear)

            Dim canReports = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ViewReports)
            Dim canManageThemes = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAppThemes)
            Dim canBackup = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.BackupData)
            Dim canRestore = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.RestoreData)
            Dim canBusinessShells = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageBusinessShells)
            Dim canUtilities = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageUtilities)
            Dim canSwitchUser = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.SwitchUser)
            Dim canChangePassword = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ChangePassword)

            Dim canManageMessages = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAppMessages)

            Dim canPayroll = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.PayrollModule)
            Dim canAmval = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AmvalModule)
            Dim canAutomation = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AutomationModule)
            Dim canCrm = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.CrmModule)
            Dim canTreasury = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TreasuryModule)
            Dim canBudgeting = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.BudgetingModule)
            Dim canProduction = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ProductionModule)
            Dim canProject = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ProjectModule)
            Dim canKpi = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.KpiModule)
            Dim canImportExport = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ImportExportModule)
            Dim canPm = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.PmModule)
            Dim canLogistics = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.LogisticsModule)
            Dim canSrm = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.SrmModule)
            Dim canQc = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.QcModule)
            Dim canBi = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.BiModule)
            Dim canDms = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.DmsModule)
            Dim canSaham = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.SahamModule)
            Dim canApi = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ApiModule)
            Dim canLegal = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.LegalModule)
            Dim canRd = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.RdModule)
            Dim canVoip = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.VoipModule)

            miUsers.Available = canUsers
            miBasicUsers.Available = canBasicUsers
            miTradeMini.Available = canAnbarMini
            miTradeMedium.Available = canAnbarMedium
            miTradeBig.Available = canAnbarBig
            miTradeWarehouseMain.Available = canTrade
            miReportsTrade.Available = canTrade OrElse canReports
            miAccountingMain.Available = canAccounting
            miReportsAccounting.Available = canAccounting OrElse canReports
            miCompanyFiscalYears.Available = canCompanyYears
            miSettingsMessages.Available = canManageMessages
            miSettingsThemes.Available = canManageThemes
            miBackupData.Available = canBackup
            miRestoreData.Available = canRestore
            miChangeProfile.Available = canChangePassword
            miSwitchUser.Available = canSwitchUser
            miCreateRelease.Available = isSuperAdmin
            miCreateUpdate.Available = isSuperAdmin
            miExportDecryptedDb.Available = isSuperAdmin
            miAbout.Available = True
            miContact.Available = True

            miUsers.Visible = canUsers
            miBasicUsers.Visible = canBasicUsers
            miTradeMini.Visible = canAnbarMini
            miTradeMedium.Visible = canAnbarMedium
            miTradeBig.Visible = canAnbarBig
            miTradeWarehouseMain.Visible = canTrade
            miReportsTrade.Visible = canTrade OrElse canReports
            miAccountingMain.Visible = canAccounting
            miReportsAccounting.Visible = canAccounting OrElse canReports
            miCompanyFiscalYears.Visible = canCompanyYears
            miSettingsMessages.Visible = canManageMessages
            miSettingsThemes.Visible = canManageThemes
            miBackupData.Visible = canBackup
            miRestoreData.Visible = canRestore
            miChangeProfile.Visible = canChangePassword
            miSwitchUser.Visible = canSwitchUser
            miCreateRelease.Visible = isSuperAdmin
            miCreateUpdate.Visible = isSuperAdmin
            miExportDecryptedDb.Visible = isSuperAdmin
            miAbout.Visible = True
            miContact.Visible = True

            mSystemMgmt.Visible = True
            mUserMgmt.Visible = canUsers OrElse canBasicUsers OrElse canSwitchUser OrElse canChangePassword
            mCompanyMgmt.Visible = canCompanyYears
            mAccounting.Visible = canAccounting OrElse canReports
            mTradeWarehouse.Visible = canTrade OrElse canReports
            mPayroll.Visible = canPayroll
            mAmval.Visible = canAmval
            mAutomation.Visible = canAutomation
            mCrm.Visible = canCrm
            mTreasury.Visible = canTreasury
            mBudgeting.Visible = canBudgeting
            mProduction.Visible = canProduction
            mProject.Visible = canProject
            mKpi.Visible = canKpi
            mImportExport.Visible = canImportExport
            mPm.Visible = canPm
            mLogistics.Visible = canLogistics
            mSrm.Visible = canSrm
            mQc.Visible = canQc
            mBi.Visible = canBi
            mDms.Visible = canDms
            mSaham.Visible = canSaham
            mApi.Visible = canApi
            mLegal.Visible = canLegal
            mRd.Visible = canRd
            mVoip.Visible = canVoip
            mBusinessShells.Visible = canBusinessShells OrElse isSuperAdmin
            mUtilities.Visible = canUtilities OrElse isSuperAdmin

            btnToolSystemMgmt.Visible = True
            btnToolUserMgmt.Visible = canUsers OrElse canBasicUsers OrElse canSwitchUser OrElse canChangePassword
            btnToolCompanyMgmt.Visible = canCompanyYears
            btnToolAccounting.Visible = canAccounting OrElse canReports
            btnToolTradeWarehouse.Visible = canTrade OrElse canReports
            btnToolPayroll.Visible = canPayroll
            btnToolAmval.Visible = canAmval
            btnToolAutomation.Visible = canAutomation
            btnToolCrm.Visible = canCrm
            btnToolTreasury.Visible = canTreasury
            btnToolBudgeting.Visible = canBudgeting
            btnToolProduction.Visible = canProduction
            btnToolProject.Visible = canProject
            btnToolKpi.Visible = canKpi
            btnToolImportExport.Visible = canImportExport
            btnToolPm.Visible = canPm
            btnToolLogistics.Visible = canLogistics
            btnToolSrm.Visible = canSrm
            btnToolQc.Visible = canQc
            btnToolBi.Visible = canBi
            btnToolDms.Visible = canDms
            btnToolSaham.Visible = canSaham
            btnToolApi.Visible = canApi
            btnToolLegal.Visible = canLegal
            btnToolRd.Visible = canRd
            btnToolVoip.Visible = canVoip
            btnToolBusinessShells.Visible = canBusinessShells OrElse isSuperAdmin
            btnToolUtilities.Visible = canUtilities OrElse isSuperAdmin

            ShowDashboardCategory("SystemMgmt")

            ShowDashboardCategory("SystemMgmt")
        End Sub

        Private Sub ApplyTheme()
            Dim theme = SessionContext.CurrentTheme
            If String.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase) Then
                BackColor = Color.FromArgb(36, 39, 46)
                ForeColor = Color.WhiteSmoke
            ElseIf String.Equals(theme, "Blue", StringComparison.OrdinalIgnoreCase) Then
                BackColor = Color.FromArgb(227, 238, 247)
                ForeColor = Color.Black
            Else
                BackColor = Color.WhiteSmoke
                ForeColor = Color.Black
            End If
        End Sub

        Public Sub LoadRandomBackgroundImage()
            Try
                Dim bgImg = BackgroundImageService.GetRandomBackgroundImage()
                If bgImg IsNot Nothing Then
                    Me.BackgroundImage = bgImg
                    Me.BackgroundImageLayout = ImageLayout.Stretch
                    flpDashboard.BackgroundImage = bgImg
                    flpDashboard.BackgroundImageLayout = ImageLayout.Stretch
                    flpDashboard.BackColor = Color.Transparent
                    For Each ctrl As Control In flpDashboard.Controls
                        Dim btn = TryCast(ctrl, Button)
                        If btn IsNot Nothing Then
                            btn.BackColor = Color.FromArgb(230, 255, 255, 255)
                            btn.ForeColor = Color.FromArgb(25, 30, 40)
                            btn.FlatAppearance.BorderColor = Color.FromArgb(200, 255, 255, 255)
                            btn.FlatAppearance.BorderSize = 1
                        End If
                    Next
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Sub UpdateStatusBar()
            If _currentUser IsNot Nothing Then
                lblUser.Text = "  کاربر: " & _currentUser.FullName & "  "
            Else
                lblUser.Text = "  کاربر: -  "
            End If

            Dim compName = If(SessionContext.CurrentCompanyID.HasValue AndAlso
                              Not String.IsNullOrWhiteSpace(SessionContext.CurrentCompanyName),
                              SessionContext.CurrentCompanyName, "-")
            lblCompany.Text = "  شرکت: " & compName & "  "

            Dim fyName = If(SessionContext.CurrentFiscalYearID.HasValue AndAlso
                            Not String.IsNullOrWhiteSpace(SessionContext.CurrentFiscalYearName),
                            SessionContext.CurrentFiscalYearName, "-")
            lblFiscalYear.Text = "  سال مالی: " & fyName & "  "

            UpdateClock()
        End Sub

        Private Function ToPersianDate(dt As DateTime) As String
            Dim pc As New PersianCalendar()
            Return String.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt))
        End Function

        Private Sub UpdateClock()
            lblDateTime.Text = "  " & ToPersianDate(DateTime.Now) & "   " & DateTime.Now.ToString("HH:mm:ss") & "  "
        End Sub

        Private Sub ClockTimer_Tick(sender As Object, e As EventArgs) Handles clockTimer.Tick
            UpdateClock()
        End Sub

        Private Sub InitOpenFormsTabBar()
            If flpFormTabs Is Nothing Then Return
            flpFormTabs.SuspendLayout()
            flpFormTabs.Controls.Clear()
            _openFormTabs.Clear()

            _btnHomeTab = New Button()
            _btnHomeTab.Text = "🏠 داشبورد اصلی"
            _btnHomeTab.AutoSize = True
            _btnHomeTab.Height = 30
            _btnHomeTab.FlatStyle = FlatStyle.Flat
            _btnHomeTab.FlatAppearance.BorderSize = 1
            _btnHomeTab.FlatAppearance.BorderColor = Color.FromArgb(180, 200, 225)
            _btnHomeTab.BackColor = Color.FromArgb(41, 128, 185)
            _btnHomeTab.ForeColor = Color.White
            _btnHomeTab.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            _btnHomeTab.Cursor = Cursors.Hand
            _btnHomeTab.Margin = New Padding(2, 1, 2, 1)

            AddHandler _btnHomeTab.Click, AddressOf BtnHomeTab_Click
            flpFormTabs.Controls.Add(_btnHomeTab)
            flpFormTabs.ResumeLayout()
        End Sub

        Private Sub BtnHomeTab_Click(sender As Object, e As EventArgs)
            Negar.Business.IradLogger.Log("BtnHomeTab_Click", "Home Dashboard tab clicked")
            SetActiveTabVisual(Nothing)
        End Sub

        Private Sub AddFormTab(child As Form)
            If child Is Nothing OrElse _openFormTabs.ContainsKey(child) Then Return

            Dim btnTab As New Button()
            btnTab.Text = child.Text & "   ✕"
            btnTab.Tag = child
            btnTab.AutoSize = True
            btnTab.Height = 30
            btnTab.FlatStyle = FlatStyle.Flat
            btnTab.FlatAppearance.BorderSize = 1
            btnTab.FlatAppearance.BorderColor = Color.FromArgb(180, 200, 225)
            btnTab.BackColor = Color.FromArgb(240, 244, 250)
            btnTab.ForeColor = Color.FromArgb(40, 50, 70)
            btnTab.Font = New Font("Tahoma", 9.0!, FontStyle.Regular)
            btnTab.Cursor = Cursors.Hand
            btnTab.Margin = New Padding(2, 1, 2, 1)

            AddHandler btnTab.Click, AddressOf FormTab_Click

            _openFormTabs(child) = btnTab
            flpFormTabs.Controls.Add(btnTab)
            Negar.Business.IradLogger.Log("AddFormTab", $"Tab created for form: {child.GetType().Name}, Title: '{child.Text}', TotalOpenTabs: {_openFormTabs.Count}")
        End Sub

        Private Sub FormTab_Click(sender As Object, e As EventArgs)
            Dim btn = TryCast(sender, Button)
            If btn Is Nothing Then Return
            Dim child = TryCast(btn.Tag, Form)
            If child Is Nothing Then Return

            Negar.Business.IradLogger.Log("FormTab_Click", $"Tab clicked for form: {child.GetType().Name}, Title: '{child.Text}'")

            ' Check if clicked on close icon area (in RTL, ✕ is at the left edge X < 25)
            Dim mousePos = btn.PointToClient(Cursor.Position)
            If mousePos.X < 25 Then
                Negar.Business.IradLogger.Log("FormTab_Click", $"Close icon (X) clicked for form: {child.GetType().Name}. Closing form.")
                child.Close()
                Return
            End If

            SetActiveTabVisual(child)
        End Sub

        Private Sub RemoveFormTab(child As Form)
            If child Is Nothing Then Return
            Negar.Business.IradLogger.Log("RemoveFormTab", $"Removing tab for form: {child.GetType().Name}")

            If _openFormTabs.ContainsKey(child) Then
                Dim btn = _openFormTabs(child)
                flpFormTabs.Controls.Remove(btn)
                btn.Dispose()
                _openFormTabs.Remove(child)
            End If

            If Me.MdiChildren Is Nothing OrElse Me.MdiChildren.Length <= 1 Then
                SetActiveTabVisual(Nothing)
            Else
                Dim activatedAny As Boolean = False
                For Each f As Form In Me.MdiChildren
                    If f IsNot child AndAlso Not f.IsDisposed Then
                        SetActiveTabVisual(f)
                        activatedAny = True
                        Exit For
                    End If
                Next
                If Not activatedAny Then
                    SetActiveTabVisual(Nothing)
                End If
            End If
        End Sub

        Private Sub SetActiveTabVisual(activeChild As Form)
            Dim activeName = If(activeChild IsNot Nothing, activeChild.GetType().Name, "HOME_DASHBOARD")
            Negar.Business.IradLogger.Log("SetActiveTabVisual", $"Setting Active Tab: {activeName}, MdiChildrenCount: {If(Me.MdiChildren IsNot Nothing, Me.MdiChildren.Length, 0)}")

            ' 1. Dashboard Visibility
            If activeChild Is Nothing Then
                flpDashboard.Visible = True
                flpDashboard.BringToFront()
            Else
                flpDashboard.Visible = False
            End If

            ' 2. Home Tab Button Visuals
            If _btnHomeTab IsNot Nothing Then
                If activeChild Is Nothing Then
                    _btnHomeTab.BackColor = Color.FromArgb(41, 128, 185)
                    _btnHomeTab.ForeColor = Color.White
                    _btnHomeTab.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Else
                    _btnHomeTab.BackColor = Color.FromArgb(240, 244, 250)
                    _btnHomeTab.ForeColor = Color.FromArgb(40, 50, 70)
                    _btnHomeTab.Font = New Font("Tahoma", 9.0!, FontStyle.Regular)
                End If
            End If

            ' 3. Strict MDI Child Form Visibility & Maximized Window Management
            ' Hide all inactive MDI forms so they cannot leak into other tabs as floating windows or gray boxes!
            If Me.MdiChildren IsNot Nothing Then
                For Each f As Form In Me.MdiChildren
                    If f IsNot Nothing AndAlso Not f.IsDisposed Then
                        If f Is activeChild Then
                            f.Visible = True
                            If f.WindowState <> FormWindowState.Maximized Then
                                Negar.Business.IradLogger.Log("SetActiveTabVisual", $"Re-maximizing active child: {f.GetType().Name} (was {f.WindowState})")
                                f.WindowState = FormWindowState.Normal
                                f.WindowState = FormWindowState.Maximized
                            End If
                            f.BringToFront()
                            f.Activate()
                            Negar.Business.IradLogger.Log("SetActiveTabVisual", $"Active child state: {f.GetType().Name}, Visible={f.Visible}, WindowState={f.WindowState}, Size={f.Size.Width}x{f.Size.Height}")
                        Else
                            f.Visible = False
                            Negar.Business.IradLogger.Log("SetActiveTabVisual", $"Hiding inactive child: {f.GetType().Name}")
                        End If
                    End If
                Next
            End If

            ' 4. Form Tab Buttons Visuals
            For Each kvp In _openFormTabs
                Dim f = kvp.Key
                Dim btn = kvp.Value
                If f Is activeChild Then
                    btn.BackColor = Color.FromArgb(41, 128, 185)
                    btn.ForeColor = Color.White
                    btn.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Else
                    btn.BackColor = Color.FromArgb(240, 244, 250)
                    btn.ForeColor = Color.FromArgb(40, 50, 70)
                    btn.Font = New Font("Tahoma", 9.0!, FontStyle.Regular)
                End If
            Next

            UpdateSidebarBounds()
        End Sub

        Private Sub OpenChild(child As Form)
            If child Is Nothing Then Return
            Negar.Business.IradLogger.Log("OpenChild", $"OpenChild requested for form: {child.GetType().Name}, Text='{child.Text}'")

            Try
                ' If form of same type is already open, activate its existing window and tab!
                For Each existingForm As Form In Me.MdiChildren
                    If existingForm.GetType() Is child.GetType() Then
                        Negar.Business.IradLogger.Log("OpenChild", $"Form {child.GetType().Name} already open. Activating existing instance.")
                        SetActiveTabVisual(existingForm)
                        child.Dispose()
                        Return
                    End If
                Next

                child.MdiParent = Me
                child.WindowState = FormWindowState.Maximized
                AddHandler child.FormClosed, AddressOf ChildForm_FormClosed
                child.Show()
                Negar.Business.IradLogger.Log("OpenChild", $"Form {child.GetType().Name} shown inside MDI Parent. WindowState={child.WindowState}, Visible={child.Visible}, Size={child.Size.Width}x{child.Size.Height}")
                AddFormTab(child)
                SetActiveTabVisual(child)
            Catch ex As Exception
                Negar.Business.IradLogger.Log("OpenChild", $"Exception opening form {child.GetType().Name}: {ex.Message}")
                child.StartPosition = FormStartPosition.CenterParent
                child.Show(Me)
            End Try
        End Sub

        Private Sub ChildForm_FormClosed(sender As Object, e As FormClosedEventArgs)
            Dim child = TryCast(sender, Form)
            If child IsNot Nothing Then
                Negar.Business.IradLogger.Log("ChildForm_FormClosed", $"FormClosed event for form: {child.GetType().Name}")
                RemoveFormTab(child)
            End If
        End Sub

        Private Sub MiUsers_Click(sender As Object, e As EventArgs) Handles miUsers.Click
            OpenChild(New UserManagementForm())
        End Sub

        Private Sub MiBasicUsers_Click(sender As Object, e As EventArgs) Handles miBasicUsers.Click
            OpenChild(New UserManagementForm(True))
        End Sub

        Public Function EnsureCompanyAndFiscalYearSelected() As Boolean
            If SessionContext.CurrentCompanyID.HasValue AndAlso SessionContext.CurrentFiscalYearID.HasValue Then
                Return True
            End If

            MessageBox.Show(
                "برای استفاده از بخش‌های سیستم، ابتدا باید شرکت و سال مالی جاری را انتخاب کنید." & Environment.NewLine &
                "فرم انتخاب شرکت و سال مالی جاری باز می‌شود.",
                "شرکت انتخاب نشده", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Dim cfForm As New CompanyFiscalYearForm(Me, openOnSelectTab:=True)
            cfForm.StartPosition = FormStartPosition.CenterParent
            cfForm.ShowDialog(Me)
            UpdateStatusBar()

            Return SessionContext.CurrentCompanyID.HasValue AndAlso SessionContext.CurrentFiscalYearID.HasValue
        End Function

        Private Sub MiTradeMini_Click(sender As Object, e As EventArgs) Handles miTradeMini.Click
            If Not EnsureCompanyAndFiscalYearSelected() Then Return
            SessionContext.CurrentEdition = AppEdition.Mini
            OpenChild(New Anbardary.AnbarMini.AnbarMiniMainForm())
        End Sub

        Private Sub MiTradeMedium_Click(sender As Object, e As EventArgs) Handles miTradeMedium.Click
            If Not EnsureCompanyAndFiscalYearSelected() Then Return
            SessionContext.CurrentEdition = AppEdition.Medium
            OpenChild(New AnbardaryMainForm())
        End Sub

        Private Sub MiTradeBig_Click(sender As Object, e As EventArgs) Handles miTradeBig.Click
            If Not EnsureCompanyAndFiscalYearSelected() Then Return
            SessionContext.CurrentEdition = AppEdition.Big
            OpenChild(New AnbardaryMainForm())
        End Sub

        Private Sub MiTradeWarehouseMain_Click(sender As Object, e As EventArgs) Handles miTradeWarehouseMain.Click
            If Not EnsureCompanyAndFiscalYearSelected() Then Return
            Select Case SessionContext.CurrentEdition
                Case AppEdition.Mini
                    OpenChild(New Anbardary.AnbarMini.AnbarMiniMainForm())
                Case Else
                    OpenChild(New AnbardaryMainForm())
            End Select
        End Sub

        Private Sub MiReportsTrade_Click(sender As Object, e As EventArgs) Handles miReportsTrade.Click
            OpenChild(New ReportCenterForm())
        End Sub

        Private Sub MiAccountingMain_Click(sender As Object, e As EventArgs) Handles miAccountingMain.Click
            If Not EnsureCompanyAndFiscalYearSelected() Then Return
            OpenChild(New HesabdaryMainForm())
        End Sub

        Private Sub MiReportsAccounting_Click(sender As Object, e As EventArgs) Handles miReportsAccounting.Click
            OpenChild(New HesabdaryMainForm(True))
        End Sub

        Private Sub MiCompanyFiscalYears_Click(sender As Object, e As EventArgs) Handles miCompanyFiscalYears.Click
            OpenChild(New CompanyFiscalYearForm(Me, openOnSelectTab:=True))
            UpdateStatusBar()
        End Sub

        Private Sub MiShellGeneral_Click(sender As Object, e As EventArgs) Handles miShellGeneral.Click
            MessageBox.Show("پوسته عمومی و بازرگانی فعال می‌باشد.", "پوسته مشاغل", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub MiShellRetail_Click(sender As Object, e As EventArgs) Handles miShellRetail.Click
            MessageBox.Show("پوسته فروشگاهی و اصناف انتخاب گردید.", "پوسته مشاغل", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub MiShellServices_Click(sender As Object, e As EventArgs) Handles miShellServices.Click
            MessageBox.Show("پوسته خدماتی و شرکتی انتخاب گردید.", "پوسته مشاغل", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub MiUtilCalculator_Click(sender As Object, e As EventArgs) Handles miUtilCalculator.Click
            Try
                Dim proc = Process.Start("calc.exe")
                If proc IsNot Nothing Then
                    _spawnedCalcProcesses.Add(proc)
                End If
            Catch
                MessageBox.Show("ماشین حساب سیستم یافت نشد.", "ماشین حساب", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub

        Private Sub MiUtilNotes_Click(sender As Object, e As EventArgs) Handles miUtilNotes.Click
            OpenChild(New UserNotebookForm())
        End Sub

        Private Sub MiUtilCalendar_Click(sender As Object, e As EventArgs) Handles miUtilCalendar.Click
            OpenChild(New PersianCalendarViewForm())
        End Sub

        Private Sub MiSettingsMessages_Click(sender As Object, e As EventArgs) Handles miSettingsMessages.Click
            OpenChild(New SystemMessagesForm())
        End Sub

        Private Sub MiSettingsThemes_Click(sender As Object, e As EventArgs) Handles miSettingsThemes.Click
            OpenChild(New SettingsForm())
        End Sub

        Private Sub MiDataMigration_Click(sender As Object, e As EventArgs)
            OpenChild(New DataMigrationForm())
        End Sub

        Private Sub MiBackupData_Click(sender As Object, e As EventArgs) Handles miBackupData.Click
            Dim form As New BackupRestoreForm(BackupRestoreForm.OperationMode.Backup)
            form.ShowDialog(Me)
        End Sub

        Private Sub MiRestoreData_Click(sender As Object, e As EventArgs) Handles miRestoreData.Click
            Dim form As New BackupRestoreForm(BackupRestoreForm.OperationMode.Restore)
            form.ShowDialog(Me)
        End Sub

        Private Sub MiChangeProfile_Click(sender As Object, e As EventArgs) Handles miChangeProfile.Click
            Dim form As New ChangeProfileForm()
            If form.ShowDialog(Me) = DialogResult.OK Then
                UpdateStatusBar()
            End If
        End Sub

        Private Sub MiCreateRelease_Click(sender As Object, e As EventArgs) Handles miCreateRelease.Click
            Dim selectForm As New SelectReleaseUserForm()
            If selectForm.ShowDialog(Me) = DialogResult.OK Then
                ReleaseBuilderService.CreateReleasePackage(selectForm.SelectedManagerID, selectForm.ManagerPassword)
            End If
        End Sub

        Private Sub MiCreateUpdate_Click(sender As Object, e As EventArgs) Handles miCreateUpdate.Click
            UpdateBuilderService.CreateUpdatePackage()
        End Sub

        Private Sub MiExportDecryptedDb_Click(sender As Object, e As EventArgs) Handles miExportDecryptedDb.Click
            Using ofd As New OpenFileDialog()
                ofd.Title = "انتخاب فایل دیتابیس رمزنگاری‌شده جهت بازرسی"
                ofd.Filter = "فایل‌های دیتابیس سیستمی (*.dat;*.db)|*.dat;*.db|کلیه فایل‌ها (*.*)|*.*"
                Dim defaultEncPath As String = AesDbService.GetEncryptedFilePath()
                If File.Exists(defaultEncPath) Then
                    ofd.InitialDirectory = Path.GetDirectoryName(defaultEncPath)
                    ofd.FileName = Path.GetFileName(defaultEncPath)
                End If

                If ofd.ShowDialog(Me) = DialogResult.OK Then
                    Using sfd As New SaveFileDialog()
                        sfd.Title = "مسیر ذخیره خروجی دیتابیس بدون رمز جهت بازرسی"
                        sfd.Filter = "فایل دیتابیس اسکیوال‌لایت (*.db)|*.db"
                        sfd.FileName = "Negar_unlocked.db"
                        If sfd.ShowDialog(Me) = DialogResult.OK Then
                            Try
                                AesDbService.ExportDecryptedDatabase(ofd.FileName, sfd.FileName)
                                MessageBox.Show("نسخه بدون رمز دیتابیس با موفقیت ایجاد شد و در مسیر زیر قرار گرفت:" & Environment.NewLine & Environment.NewLine & sfd.FileName & Environment.NewLine & Environment.NewLine & "اکنون می‌توانید آن را بدون کلمه عبور در DB Browser یا هر ابزار دیگری بازرسی فرمایید.", "موفقیت در بازرسی دیتابیس", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Catch ex As Exception
                                MessageBox.Show("خطا در ایجاد خروجی دیتابیس: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End If
                    End Using
                End If
            End Using
        End Sub

        Private Sub MiAbout_Click(sender As Object, e As EventArgs) Handles miAbout.Click
            Dim settingsSvc As New SettingsService()
            Dim aboutText = settingsSvc.GetSettingValue("AboutText", SettingsService.DefaultAboutText)
            Dim form As New InfoWindowForm("درباره نرم‌افزار", aboutText, Me)
            form.ShowDialog(Me)
        End Sub

        Private Sub MiContact_Click(sender As Object, e As EventArgs) Handles miContact.Click
            Dim settingsSvc As New SettingsService()
            Dim contactText = settingsSvc.GetSettingValue("ContactText", SettingsService.DefaultContactText)
            Dim form As New InfoWindowForm("ارتباط با ما", contactText, Me)
            form.ShowDialog(Me)
        End Sub

        Public Sub LockApplication()
            If _isLocked Then Return
            _isLocked = True
            Try
                Dim lockForm As New AppLockForm(_currentUser)
                Dim result = lockForm.ShowDialog(Me)
                If result = DialogResult.Retry AndAlso lockForm.SwitchUserRequested Then
                    MiSwitchUser_Click(Me, EventArgs.Empty)
                End If
            Finally
                _isLocked = False
            End Try
        End Sub

        Private Sub MiLock_Click(sender As Object, e As EventArgs) Handles miLock.Click
            LockApplication()
        End Sub

        Private Sub MiSwitchUser_Click(sender As Object, e As EventArgs) Handles miSwitchUser.Click
            Dim loginForm As New LoginForm()
            loginForm.StartPosition = FormStartPosition.CenterParent
            Dim result = loginForm.ShowDialog(Me)
            If result <> DialogResult.OK Then Return

            ' بستن همه فرم‌های باز شده توسط این پنجره
            For Each f As Form In Me.OwnedForms.Clone()
                f.Close()
            Next

            ' به‌روزرسانی کاربر جاری و بازسازی دسترسی‌ها
            _currentUser = loginForm.AuthenticatedUser
            ApplySecurity()
            ApplyTheme()
            LoadRandomBackgroundImage()
            UpdateStatusBar()
        End Sub

        Private Sub MiExit_Click(sender As Object, e As EventArgs) Handles miExit.Click
            Close()
        End Sub

        Public Sub ShowDashboardCategory(category As String)
            flpDashboard.SuspendLayout()
            flpDashboard.Controls.Clear()

            Dim isSuperAdmin = _currentUser IsNot Nothing AndAlso String.Equals(_currentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim canUsers = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageUsers)
            Dim canBasicUsers = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageBasicUsers)
            Dim canTrade = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageTradeWarehouse) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageProducts) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageWarehouses) OrElse
                SessionContext.HasPermission(PermissionKeys.ManagePurchases) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageSales) OrElse
                SessionContext.HasPermission(PermissionKeys.ViewInventory) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeProducts) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeWarehouses) OrElse
                SessionContext.HasPermission(PermissionKeys.TradePurchase) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeSales) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeRemittance) OrElse
                SessionContext.HasPermission(PermissionKeys.TradeReports)

            Dim canAccounting = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageAccounting) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingHeader) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingShenavar) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingEntry) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingBank) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingBalance) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingLedger) OrElse
                SessionContext.HasPermission(PermissionKeys.AccountingReports)

            Dim canCompanyYears = isSuperAdmin OrElse
                SessionContext.HasPermission(PermissionKeys.ManageCompaniesYears) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageCompanies) OrElse
                SessionContext.HasPermission(PermissionKeys.ManageFiscalYears) OrElse
                SessionContext.HasPermission(PermissionKeys.SelectCompanyFiscalYear)

            Dim canReports = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ViewReports)
            Dim canManageThemes = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAppThemes)
            Dim canBackup = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.BackupData)
            Dim canRestore = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.RestoreData)
            Dim canBusinessShells = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageBusinessShells)
            Dim canUtilities = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageUtilities)
            Dim canSwitchUser = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.SwitchUser)
            Dim canChangePassword = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ChangePassword)

            Select Case category
                Case "SystemMgmt"
                    If isSuperAdmin Then AddDashButton("تبدیل دیتا از سایر نرم افزارها", AddressOf MiDataMigration_Click, "RestoreData")
                    If isSuperAdmin Then AddDashButton("مدیریت پیامهای : درباره... و ارتباط با ما", AddressOf MiSettingsMessages_Click, "Settings")
                    If canManageThemes Then AddDashButton("مدیریت تمهای برنامه و فرمها", AddressOf MiSettingsThemes_Click, "Settings")
                    If canBackup Then AddDashButton("پشتیبان‌گیری اطلاعات", AddressOf MiBackupData_Click, "BackupData")
                    If canRestore Then AddDashButton("بازیابی اطلاعات", AddressOf MiRestoreData_Click, "RestoreData")
                    If isSuperAdmin Then AddDashButton("ایجاد نسخه قابل انتشار", AddressOf MiCreateRelease_Click, "CreateRelease")
                    If isSuperAdmin Then AddDashButton("ایجاد بسته به‌روزرسانی", AddressOf MiCreateUpdate_Click, "CreateUpdate")
                    If isSuperAdmin Then AddDashButton("خروجی دیتابیس (بازرسی)", AddressOf MiExportDecryptedDb_Click, "ExportDecryptedDb")
                    AddDashButton("قفل موقت برنامه", Sub(s, e) LockApplication(), "Lock")
                    AddDashButton("درباره...", AddressOf MiAbout_Click, "Home")
                    AddDashButton("ارتباط با ما", AddressOf MiContact_Click, "ChangeProfile")
                    AddDashButton("خروج", Sub(s, e) Close(), "Exit")

                Case "UserMgmt"
                    If canUsers Then AddDashButton("مدیریت کاربران (جامع)", AddressOf MiUsers_Click, "Users")
                    If canBasicUsers Then AddDashButton("مدیریت کاربران – مدیریت کاربران عادی", AddressOf MiBasicUsers_Click, "BasicUsers")
                    If canChangePassword Then AddDashButton("تغییر کلمه عبور", AddressOf MiChangeProfile_Click, "ChangeProfile")
                    If canSwitchUser Then AddDashButton("ورود با کاربر دیگر", AddressOf MiSwitchUser_Click, "SwitchUser")

                Case "CompanyMgmt"
                    If canCompanyYears Then AddDashButton("شرکت ها و سالهای مالی", AddressOf MiCompanyFiscalYears_Click, "CompanyFiscalYears")

                Case "Accounting"
                    If canAccounting Then AddDashButton("کدینگ، ثبت اسناد و دفاتر", AddressOf MiAccountingMain_Click, "Accounting")
                    If canAccounting OrElse canReports Then AddDashButton("گزارشات و ترازهای حسابداری", AddressOf MiReportsAccounting_Click, "Reports")

                Case "TradeWarehouse"
                    Dim canAnbarMini = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule) OrElse (canTrade AndAlso Not (SessionContext.HasPermission(PermissionKeys.AnbarMediumModule) OrElse SessionContext.HasPermission(PermissionKeys.AnbarBigModule)))
                    Dim canAnbarMedium = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMediumModule)
                    Dim canAnbarBig = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarBigModule)

                    If canAnbarMini Then AddDashButton("استفاده از انبارداری مینی", AddressOf MiTradeMini_Click, "TradeWarehouse")
                    If canAnbarMedium Then AddDashButton("استفاده از انبارداری متوسط", AddressOf MiTradeMedium_Click, "TradeWarehouse")
                    If canAnbarBig Then AddDashButton("استفاده از انبارداری پیشرفته", AddressOf MiTradeBig_Click, "TradeWarehouse")
                    If canTrade OrElse canReports Then AddDashButton("گزارشات فاکتورها و موجودی انبار", AddressOf MiReportsTrade_Click, "Reports")

                Case "Payroll"
                    AddDashButton("سیستم جامع حقوق و دستمزد", AddressOf OpenPayrollMainForm, "Users")
                    AddDashButton("گزارشات جامع حقوق و دستمزد", AddressOf OpenPayrollMainForm, "Reports")

                Case "Amval"
                    AddDashButton("سیستم جامع اموال", AddressOf OpenAmvalMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع اموال", AddressOf OpenAmvalMainForm, "Reports")

                Case "Automation"
                    AddDashButton("سیستم جامع اتوماسیون اداری", AddressOf OpenAutomationMainForm, "Users")
                    AddDashButton("گزارشات جامع اتوماسیون اداری", AddressOf OpenAutomationMainForm, "Reports")

                Case "Crm"
                    AddDashButton("سیستم جامع باشگاه مشتریان (CRM)", AddressOf OpenCrmMainForm, "Users")
                    AddDashButton("گزارشات جامع باشگاه مشتریان و فروش", AddressOf OpenCrmMainForm, "Reports")

                Case "Treasury"
                    AddDashButton("سیستم جامع خزانه‌داری و مدیریت نقدینگی", AddressOf OpenTreasuryMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع خزانه‌داری و Cash Flow", AddressOf OpenTreasuryMainForm, "Reports")

                Case "Budgeting"
                    AddDashButton("سیستم جامع بودجه و کنترل هزینه‌ها", AddressOf OpenBudgetingMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات انحراف بودجه و انضباط مالی", AddressOf OpenBudgetingMainForm, "Reports")

                Case "Production"
                    AddDashButton("سیستم جامع بهای تمام‌شده و برنامه‌ریزی تولید", AddressOf OpenProductionMainForm, "TradeWarehouse")
                    AddDashButton("گزارشات جامع بهای تمام‌شده و آنالیز BOM", AddressOf OpenProductionMainForm, "Reports")

                Case "Project"
                    AddDashButton("سیستم جامع مدیریت پروژه‌ها و پیمان‌ها", AddressOf OpenProjectMainForm, "TradeWarehouse")
                    AddDashButton("گزارشات جامع پروژه‌ها و پیمان‌ها", AddressOf OpenProjectMainForm, "Reports")

                Case "Kpi"
                    AddDashButton("سیستم جامع ارزیابی عملکرد و پاداش (KPI)", AddressOf OpenKpiMainForm, "Users")
                    AddDashButton("گزارشات جامع ارزیابی عملکرد و کارانه", AddressOf OpenKpiMainForm, "Reports")

                Case "ImportExport"
                    AddDashButton("سیستم جامع بازرگانی خارجی و واردات/صادرات", AddressOf OpenImportExportMainForm, "TradeWarehouse")
                    AddDashButton("گزارشات جامع بهای تمام‌شده واردات (Landed Cost)", AddressOf OpenImportExportMainForm, "Reports")

                Case "Pm"
                    AddDashButton("سیستم جامع مدیریت نت، نگهداری و تعمیرات (PM)", AddressOf OpenPmMainForm, "Production")
                    AddDashButton("گزارشات جامع شاخص‌های OEE، MTBF و هزینه نت", AddressOf OpenPmMainForm, "Reports")

                Case "Logistics"
                    AddDashButton("سیستم جامع مدیریت ناوگان حمل و پخش مویرگی", AddressOf OpenLogisticsMainForm, "TradeWarehouse")
                    AddDashButton("گزارشات جامع بارنامه‌ها، کرایه حمل و پورسانت توزیع", AddressOf OpenLogisticsMainForm, "Reports")

                Case "Srm"
                    AddDashButton("سیستم جامع ارزیابی و مدیریت ارتباط با تامین‌کنندگان (SRM)", AddressOf OpenSrmMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع استعلام‌ها (RFQ)، کارت امتیازی و انحراف قیمت خرید", AddressOf OpenSrmMainForm, "Reports")

                Case "Qc"
                    AddDashButton("سیستم جامع کنترل کیفیت و تضمین کیفیت (QC/QA)", AddressOf OpenQcMainForm, "Production")
                    AddDashButton("گزارشات جامع بازرسی‌های IQC/IPQC، ضایعات و FPY", AddressOf OpenQcMainForm, "Reports")

                Case "Bi"
                    AddDashButton("سیستم جامع هوش تجاری و داشبورد مدیریتی پیشرفته (BI)", AddressOf OpenBiMainForm, "Reports")
                    AddDashButton("گزارشات جامع تحلیلی OLAP، پیش‌بینی هوشمند و P&L", AddressOf OpenBiMainForm, "Accounting")

                Case "Dms"
                    AddDashButton("سیستم جامع مدیریت بایگانی دیجیتال و آرشیو اسناد (DMS)", AddressOf OpenDmsMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع پرونده‌های آرشیو، سررسید انقضا و لاگ امنیتی", AddressOf OpenDmsMainForm, "Reports")

                Case "Saham"
                    AddDashButton("سیستم جامع امور سهام و سهامداران", AddressOf OpenSahamMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع ترکیب سهامداران، مجمع عمومی و پرداخت سود", AddressOf OpenSahamMainForm, "Reports")

                Case "Api"
                    AddDashButton("سیستم جامع وب‌سرویس، API فروشگاه اینترنتی و پوز سیار", AddressOf OpenApiMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع لاگ‌های API، مانیتورینگ ترافیک و فروش Omnichannel", AddressOf OpenApiMainForm, "Reports")

                Case "Legal"
                    AddDashButton("سیستم جامع مدیریت امور حقوقی، قراردادها و دعاوی", AddressOf OpenLegalMainForm, "CompanyFiscalYears")
                    AddDashButton("📅 تقویم جلسات دادگاه و مهلت‌های تجدیدنظرخواهی", AddressOf OpenLegalMainForm, "Accounting")
                    AddDashButton("👨‍⚖️ مدیریت وکلا، کارشناسان رسمی و حق‌الوکاله‌ها", AddressOf OpenLegalMainForm, "TradeWarehouse")
                    AddDashButton("📊 گزارشات جامع ریسک مالی پرونده‌های حقوقی در جریان", AddressOf OpenLegalMainForm, "Reports")

                Case "Rd"
                    AddDashButton("🔬 پروژه‌های NPD با Stage-Gate — از ایده تا تجاری‌سازی", AddressOf OpenRdMainForm, "Production")
                    AddDashButton("🧪 فرمولاسیون محصول، BOM پژوهشی و Version Control فرمول", AddressOf OpenRdMainForm, "TradeWarehouse")
                    AddDashButton("🧫 لاگ آزمایشگاهی، Pilot Test و مقایسه با Target Specs", AddressOf OpenRdMainForm, "QcModule")
                    AddDashButton("🏛️ مدیریت پتنت‌ها، اختراعات و مالکیت فکری (IPR)", AddressOf OpenRdMainForm, "CompanyFiscalYears")
                    AddDashButton("📊 گزارشات Innovation Funnel، ROI تحقیقات و Time-to-Market", AddressOf OpenRdMainForm, "Reports")

                Case "Voip"
                    AddDashButton("📞 سیستم جامع مرکز تلفن هوشمند، CRM صوتی و صف ACD", AddressOf OpenVoipMainForm, "Crm")
                    AddDashButton("🖥️ Screen Pop-Up هوشمند مشتری و پایش زنده صف تماس", AddressOf OpenVoipMainForm, "Automation")
                    AddDashButton("🎙️ آرشیو صوتی مکالمات، پیوند به DMS و جستجوی متنی (STT)", AddressOf OpenVoipMainForm, "TradeWarehouse")
                    AddDashButton("📲 کمپین‌های تماس خروجی با Preview Dial و Click-to-Call", AddressOf OpenVoipMainForm, "CompanyFiscalYears")
                    AddDashButton("📊 گزارشات KPI مرکز تماس — Answer Rate، ASA، CSAT و نرخ تبدیل", AddressOf OpenVoipMainForm, "Reports")

                Case "BusinessShells"
                    If canBusinessShells OrElse isSuperAdmin Then
                        AddDashButton("پوسته عمومی و بازرگانی", AddressOf MiShellGeneral_Click, "Home")
                        AddDashButton("پوسته فروشگاهی و اصناف", AddressOf MiShellRetail_Click, "TradeWarehouse")
                        AddDashButton("پوسته خدماتی و شرکتی", AddressOf MiShellServices_Click, "CompanyFiscalYears")
                    End If

                Case "Utilities"
                    If canUtilities OrElse isSuperAdmin Then
                        AddDashButton("ماشین حساب سیستم", AddressOf MiUtilCalculator_Click, "Accounting")
                        AddDashButton("دفترچه یادداشت", AddressOf MiUtilNotes_Click, "Settings")
                        AddDashButton("تقویم و مناسبت‌ها", AddressOf MiUtilCalendar_Click, "CompanyFiscalYears")
                    End If
            End Select

            flpDashboard.BringToFront()
            flpDashboard.ResumeLayout()
            UpdateSidebarBounds()
        End Sub

        Private Sub AddDashButton(text As String, onClick As EventHandler, Optional iconType As String = "")
            Dim btn As New Button()
            btn.Text = text
            btn.Width = 220
            btn.Height = 140
            btn.BackColor = Color.FromArgb(230, 255, 255, 255)
            btn.ForeColor = Color.FromArgb(25, 30, 40)
            btn.Cursor = Cursors.Hand
            btn.FlatAppearance.BorderColor = Color.FromArgb(200, 255, 255, 255)
            btn.FlatAppearance.BorderSize = 1
            btn.FlatStyle = FlatStyle.Flat
            btn.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            btn.Margin = New Padding(15)
            btn.TextImageRelation = TextImageRelation.ImageAboveText
            If Not String.IsNullOrEmpty(iconType) Then
                btn.Image = CreateModuleIcon(iconType, 48)
            End If
            AddHandler btn.Click, onClick
            flpDashboard.Controls.Add(btn)
        End Sub

        Private Sub OpenPayrollMainForm(sender As Object, e As EventArgs) Handles mPayroll.Click, miPayrollMain.Click, miPayrollReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Payroll.PayrollMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول حقوق و دستمزد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolAmval_Click(sender As Object, e As EventArgs) Handles btnToolAmval.Click
            ShowDashboardCategory("Amval")
        End Sub

        Private Sub OpenAmvalMainForm(sender As Object, e As EventArgs) Handles mAmval.Click, miAmvalMain.Click, miAmvalReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Amval.AmvalMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول اموال: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolAutomation_Click(sender As Object, e As EventArgs) Handles btnToolAutomation.Click
            ShowDashboardCategory("Automation")
        End Sub

        Private Sub OpenAutomationMainForm(sender As Object, e As EventArgs) Handles mAutomation.Click, miAutomationMain.Click, miAutomationReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Automation.AutomationMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول اتوماسیون اداری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolCrm_Click(sender As Object, e As EventArgs) Handles btnToolCrm.Click
            ShowDashboardCategory("Crm")
        End Sub

        Private Sub OpenCrmMainForm(sender As Object, e As EventArgs) Handles mCrm.Click, miCrmMain.Click, miCrmReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.CRM.CrmMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول CRM: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolTreasury_Click(sender As Object, e As EventArgs) Handles btnToolTreasury.Click
            ShowDashboardCategory("Treasury")
        End Sub

        Private Sub OpenTreasuryMainForm(sender As Object, e As EventArgs) Handles mTreasury.Click, miTreasuryMain.Click, miTreasuryReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Treasury.TreasuryMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول خزانه‌داری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolBudgeting_Click(sender As Object, e As EventArgs) Handles btnToolBudgeting.Click
            ShowDashboardCategory("Budgeting")
        End Sub

        Private Sub OpenBudgetingMainForm(sender As Object, e As EventArgs) Handles mBudgeting.Click, miBudgetingMain.Click, miBudgetingReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Budgeting.BudgetMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول بودجه و کنترل هزینه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolProduction_Click(sender As Object, e As EventArgs) Handles btnToolProduction.Click
            ShowDashboardCategory("Production")
        End Sub

        Private Sub OpenProductionMainForm(sender As Object, e As EventArgs) Handles mProduction.Click, miProductionMain.Click, miProductionReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Production.ProductionMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول بهای تمام‌شده و تولید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolProject_Click(sender As Object, e As EventArgs) Handles btnToolProject.Click
            ShowDashboardCategory("Project")
        End Sub

        Private Sub OpenProjectMainForm(sender As Object, e As EventArgs) Handles mProject.Click, miProjectMain.Click, miProjectReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Project.ProjectMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول مدیریت پروژه‌ها و پیمان‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolKpi_Click(sender As Object, e As EventArgs) Handles btnToolKpi.Click
            ShowDashboardCategory("Kpi")
        End Sub

        Private Sub OpenKpiMainForm(sender As Object, e As EventArgs) Handles mKpi.Click, miKpiMain.Click, miKpiReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.KPI.KpiMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول ارزیابی عملکرد و پاداش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolImportExport_Click(sender As Object, e As EventArgs) Handles btnToolImportExport.Click
            ShowDashboardCategory("ImportExport")
        End Sub

        Private Sub OpenImportExportMainForm(sender As Object, e As EventArgs) Handles mImportExport.Click, miImportExportMain.Click, miImportExportReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.ImportExport.ImportExportMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم بازرگانی خارجی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolPm_Click(sender As Object, e As EventArgs) Handles btnToolPm.Click
            ShowDashboardCategory("Pm")
        End Sub

        Private Sub OpenPmMainForm(sender As Object, e As EventArgs) Handles mPm.Click, miPmMain.Click, miPmReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.PM.PmMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم مدیریت نگهداری و تعمیرات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolLogistics_Click(sender As Object, e As EventArgs) Handles btnToolLogistics.Click
            ShowDashboardCategory("Logistics")
        End Sub

        Private Sub OpenLogisticsMainForm(sender As Object, e As EventArgs) Handles mLogistics.Click, miLogisticsMain.Click, miLogisticsReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Logistics.LogisticsMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم لوجستیک و پخش مویرگی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolSrm_Click(sender As Object, e As EventArgs) Handles btnToolSrm.Click
            ShowDashboardCategory("Srm")
        End Sub

        Private Sub OpenSrmMainForm(sender As Object, e As EventArgs) Handles mSrm.Click, miSrmMain.Click, miSrmReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.SRM.SrmMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم مدیریت تامین‌کنندگان (SRM): " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolQc_Click(sender As Object, e As EventArgs) Handles btnToolQc.Click
            ShowDashboardCategory("Qc")
        End Sub

        Private Sub OpenQcMainForm(sender As Object, e As EventArgs) Handles mQc.Click, miQcMain.Click, miQcReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.QC.QcMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم کنترل کیفیت و تضمین کیفیت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolBi_Click(sender As Object, e As EventArgs) Handles btnToolBi.Click
            ShowDashboardCategory("Bi")
        End Sub

        Private Sub OpenBiMainForm(sender As Object, e As EventArgs) Handles mBi.Click, miBiMain.Click, miBiReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.BI.BiMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم هوش تجاری و داشبورد مدیریتی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolDms_Click(sender As Object, e As EventArgs) Handles btnToolDms.Click
            ShowDashboardCategory("Dms")
        End Sub

        Private Sub OpenDmsMainForm(sender As Object, e As EventArgs) Handles mDms.Click, miDmsMain.Click, miDmsReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.DMS.DmsMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم مدیریت بایگانی دیجیتال: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolSaham_Click(sender As Object, e As EventArgs) Handles btnToolSaham.Click
            ShowDashboardCategory("Saham")
        End Sub

        Private Sub OpenSahamMainForm(sender As Object, e As EventArgs) Handles mSaham.Click, miSahamMain.Click, miSahamReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Saham.SahamMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم امور سهام و سهامداران: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolApi_Click(sender As Object, e As EventArgs) Handles btnToolApi.Click
            ShowDashboardCategory("Api")
        End Sub

        Private Sub OpenApiMainForm(sender As Object, e As EventArgs) Handles mApi.Click, miApiMain.Click, miApiReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.API.ApiMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم وب‌سرویس و API: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolLegal_Click(sender As Object, e As EventArgs) Handles btnToolLegal.Click
            ShowDashboardCategory("Legal")
        End Sub

        Private Sub OpenLegalMainForm(sender As Object, e As EventArgs) Handles mLegal.Click, miLegalMain.Click, miLegalReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.Legal.LegalMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم امور حقوقی و دعاوی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolRd_Click(sender As Object, e As EventArgs) Handles btnToolRd.Click
            ShowDashboardCategory("Rd")
        End Sub

        Private Sub OpenRdMainForm(sender As Object, e As EventArgs) Handles mRd.Click, miRdMain.Click, miRdReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.RD.RdMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم تحقیق و توسعه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolVoip_Click(sender As Object, e As EventArgs) Handles btnToolVoip.Click
            ShowDashboardCategory("Voip")
        End Sub

        Private Sub OpenVoipMainForm(sender As Object, e As EventArgs) Handles mVoip.Click, miVoipMain.Click, miVoipReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                OpenChild(New Negar.Forms.VoIP.VoipMainForm())
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن سیستم مرکز تلفن هوشمند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolBusinessShells_Click(sender As Object, e As EventArgs) Handles btnToolBusinessShells.Click
            ShowDashboardCategory("BusinessShells")
        End Sub

        Private Sub BtnToolUtilities_Click(sender As Object, e As EventArgs) Handles btnToolUtilities.Click
            ShowDashboardCategory("Utilities")
        End Sub

        Private Sub BtnHandle_Click(sender As Object, e As EventArgs) Handles btnHandle.Click
            _isMenuExpanded = Not _isMenuExpanded
            _targetWidth = If(_isMenuExpanded, ExpandedWidth, CollapsedWidth)
            menuTransitionTimer.Start()
        End Sub

        Private Sub MenuTransitionTimer_Tick(sender As Object, e As EventArgs) Handles menuTransitionTimer.Tick
            Dim currentWidth = pnlSidebarContainer.Width
            Dim stepVal = 30
            If currentWidth < _targetWidth Then
                currentWidth = Math.Min(_targetWidth, currentWidth + stepVal)
            ElseIf currentWidth > _targetWidth Then
                currentWidth = Math.Max(_targetWidth, currentWidth - stepVal)
            End If
            pnlSidebarContainer.Width = currentWidth
            UpdateSidebarBounds()

            If currentWidth = _targetWidth Then
                menuTransitionTimer.Stop()
                ApplyMenuStateVisuals(_isMenuExpanded)
            End If
        End Sub

        Private Sub ApplyMenuStateVisuals(expanded As Boolean)
            If expanded Then
                btnHandle.Text = "❯"
            Else
                btnHandle.Text = "❮"
            End If
        End Sub

        Private Sub BuildAccordionTree()
            tvMenu.Nodes.Clear()

            ' 1. سیستم
            Dim nSys = tvMenu.Nodes.Add("nSys", "⚙️ سیستم")
            nSys.Nodes.Add("miSettingsMessages", "📩 مدیریت پیام‌ها")
            nSys.Nodes.Add("miSettingsThemes", "🎨 مدیریت تم‌های برنامه")
            nSys.Nodes.Add("miBackupData", "💾 پشتیبان‌گیری اطلاعات")
            nSys.Nodes.Add("miRestoreData", "🔄 بازیابی اطلاعات")
            nSys.Nodes.Add("miCreateRelease", "📦 ایجاد نسخه قابل انتشار")
            nSys.Nodes.Add("miCreateUpdate", "🆙 ایجاد بسته به‌روزرسانی")
            nSys.Nodes.Add("miExportDecryptedDb", "🔍 خروجی دیتابیس (بازرسی)")
            nSys.Nodes.Add("miLock", "🔒 قفل موقت برنامه")
            nSys.Nodes.Add("miAbout", "ℹ️ درباره...")
            nSys.Nodes.Add("miContact", "📞 ارتباط با ما")
            nSys.Nodes.Add("miExit", "🚪 خروج")

            ' 2. کاربران
            Dim nUsers = tvMenu.Nodes.Add("nUsers", "👥 کاربران")
            nUsers.Nodes.Add("miUsers", "👤 مدیریت کاربران سیستم")
            nUsers.Nodes.Add("miBasicUsers", "👥 مدیریت کاربران پایه")
            nUsers.Nodes.Add("miChangeProfile", "🔑 تغییر مشخصات کاربر جاری")
            nUsers.Nodes.Add("miSwitchUser", "🔄 تغییر کاربر سیستم")

            ' 3. شرکت‌ها و سال‌های مالی
            Dim nComp = tvMenu.Nodes.Add("nComp", "🏢 شرکت‌ها و سال‌های مالی")
            nComp.Nodes.Add("miCompanyFiscalYears", "🏢 مدیریت شرکت‌ها و سال‌های مالی")

            ' 4. حسابداری
            Dim nAcc = tvMenu.Nodes.Add("nAcc", "🧮 حسابداری")
            nAcc.Nodes.Add("miAccountingMain", "🧮 سیستم جامع حسابداری مالی")
            nAcc.Nodes.Add("miReportsAccounting", "📑 دفاتر و گزارشات حسابداری")

            ' 5. خرید و فروش و انبارداری
            Dim nTrade = tvMenu.Nodes.Add("nTrade", "📦 خرید و فروش و انبارداری")
            nTrade.Nodes.Add("miTradeMini", "📦 سیستم انبارداری همراه (اصناف)")
            nTrade.Nodes.Add("miTradeMedium", "📦 سیستم انبارداری شرکتی (متوسط)")
            nTrade.Nodes.Add("miTradeBig", "📦 سیستم انبارداری سازمان‌های بزرگ")
            nTrade.Nodes.Add("miTradeWarehouseMain", "🛒 سیستم جامع خرید و فروش و انبارداری")
            nTrade.Nodes.Add("miReportsTrade", "📊 گزارشات انبارداری و خریدوفروش")

            ' 6. حقوق و دستمزد
            Dim nPay = tvMenu.Nodes.Add("nPay", "💳 حقوق و دستمزد")
            nPay.Nodes.Add("miPayrollMain", "💳 سیستم جامع حقوق و دستمزد")
            nPay.Nodes.Add("miPayrollReports", "📊 گزارشات حقوق و دستمزد")

            ' 7. اموال و دارایی ثابت
            Dim nAmval = tvMenu.Nodes.Add("nAmval", "🏛️ اموال و دارایی‌های ثابت")
            nAmval.Nodes.Add("miAmvalMain", "🏛️ سیستم مدیریت اموال و دارایی ثابت")
            nAmval.Nodes.Add("miAmvalReports", "📊 گزارشات اموال و استهلاک")

            ' 8. اتوماسیون اداری
            Dim nAuto = tvMenu.Nodes.Add("nAuto", "📨 اتوماسیون اداری")
            nAuto.Nodes.Add("miAutomationMain", "📨 سیستم اتوماسیون اداری")
            nAuto.Nodes.Add("miAutomationReports", "📊 گزارشات اتوماسیون اداری")

            ' 9. باشگاه مشتریان (CRM)
            Dim nCrm = tvMenu.Nodes.Add("nCrm", "🤝 باشگاه مشتریان (CRM)")
            nCrm.Nodes.Add("miCrmMain", "🤝 سیستم مدیریت ارتباط با مشتریان")
            nCrm.Nodes.Add("miCrmReports", "📊 گزارشات باشگاه مشتریان")

            ' 10. خزانه‌داری
            Dim nTreasury = tvMenu.Nodes.Add("nTreasury", "💰 خزانه‌داری")
            nTreasury.Nodes.Add("miTreasuryMain", "💰 سیستم خزانه‌داری و صندوق/بانک")
            nTreasury.Nodes.Add("miTreasuryReports", "📊 گزارشات خزانه‌داری")

            ' 11. بودجه و هزینه
            Dim nBudget = tvMenu.Nodes.Add("nBudget", "📈 بودجه و هزینه")
            nBudget.Nodes.Add("miBudgetingMain", "📈 سیستم بودجه‌ریزی و کنترل هزینه‌ها")
            nBudget.Nodes.Add("miBudgetingReports", "📊 گزارشات بودجه و انحرافات")

            ' 12. تولید و بهای تمام شده
            Dim nProd = tvMenu.Nodes.Add("nProd", "🏭 تولید و بهای تمام‌شده")
            nProd.Nodes.Add("miProductionMain", "🏭 سیستم بهای تمام‌شده و تولید")
            nProd.Nodes.Add("miProductionReports", "📊 گزارشات تولید و بهای تمام‌شده")

            ' 13. پروژه‌ها و پیمان‌ها
            Dim nProj = tvMenu.Nodes.Add("nProj", "🏗️ پروژه‌ها و پیمان‌ها")
            nProj.Nodes.Add("miProjectMain", "🏗️ سیستم مدیریت پروژه‌ها و پیمان‌ها")
            nProj.Nodes.Add("miProjectReports", "📊 گزارشات پروژه‌ها")

            ' 14. ارزیابی عملکرد و پاداش (KPI)
            Dim nKpi = tvMenu.Nodes.Add("nKpi", "🎯 ارزیابی عملکرد و پاداش")
            nKpi.Nodes.Add("miKpiMain", "🎯 سیستم ارزیابی عملکرد و شایستگی")
            nKpi.Nodes.Add("miKpiReports", "📊 گزارشات شاخص‌های کلیدی عملکرد")

            ' 15. بازرگانی خارجی
            Dim nImp = tvMenu.Nodes.Add("nImp", "🌐 بازرگانی خارجی")
            nImp.Nodes.Add("miImportExportMain", "🌐 سیستم بازرگانی خارجی")
            nImp.Nodes.Add("miImportExportReports", "📊 گزارشات بازرگانی خارجی")

            ' 16. نگهداری و تعمیرات (PM)
            Dim nPm = tvMenu.Nodes.Add("nPm", "🛠️ نگهداری و تعمیرات")
            nPm.Nodes.Add("miPmMain", "🛠️ سیستم نگهداری و تعمیرات پیشگیرانه")
            nPm.Nodes.Add("miPmReports", "📊 گزارشات نت و توقفات")

            ' 17. لوجستیک و پخش
            Dim nLog = tvMenu.Nodes.Add("nLog", "🚛 لوجستیک و پخش")
            nLog.Nodes.Add("miLogisticsMain", "🚛 سیستم لوجستیک و پخش مویرگی")
            nLog.Nodes.Add("miLogisticsReports", "📊 گزارشات لوجستیک")

            ' 18. مدیریت تامین‌کنندگان (SRM)
            Dim nSrm = tvMenu.Nodes.Add("nSrm", "🤝 مدیریت تامین‌کنندگان")
            nSrm.Nodes.Add("miSrmMain", "🤝 سیستم ارزیابی تامین‌کنندگان")
            nSrm.Nodes.Add("miSrmReports", "📊 گزارشات تامین‌کنندگان")

            ' 19. کنترل و تضمین کیفیت (QC)
            Dim nQc = tvMenu.Nodes.Add("nQc", "🛡️ کنترل و تضمین کیفیت")
            nQc.Nodes.Add("miQcMain", "🛡️ سیستم کنترل کیفیت آزمایشگاهی")
            nQc.Nodes.Add("miQcReports", "📊 گزارشات کیفیت")

            ' 20. هوش تجاری و داشبورد (BI)
            Dim nBi = tvMenu.Nodes.Add("nBi", "📊 هوش تجاری و داشبورد")
            nBi.Nodes.Add("miBiMain", "📊 سیستم هوش تجاری و تحلیل‌های مدیریتی")
            nBi.Nodes.Add("miBiReports", "📈 گزارشات و تحلیل‌های BI")

            ' 21. بایگانی و آرشیو اسناد (DMS)
            Dim nDms = tvMenu.Nodes.Add("nDms", "📂 بایگانی و آرشیو اسناد")
            nDms.Nodes.Add("miDmsMain", "📂 سیستم مدیریت بایگانی اسناد")
            nDms.Nodes.Add("miDmsReports", "📊 گزارشات بایگانی اسناد")

            ' 22. امور سهام و سهامداران
            Dim nSaham = tvMenu.Nodes.Add("nSaham", "📜 امور سهام و سهامداران")
            nSaham.Nodes.Add("miSahamMain", "📜 سیستم مدیریت امور سهام")
            nSaham.Nodes.Add("miSahamReports", "📊 گزارشات سهامداران")

            ' 23. وب‌سرویس و API
            Dim nApi = tvMenu.Nodes.Add("nApi", "🔌 وب‌سرویس و API")
            nApi.Nodes.Add("miApiMain", "🔌 سیستم مدیریت وب‌سرویس و API")
            nApi.Nodes.Add("miApiReports", "📊 لاگ‌ها و گزارشات وب‌سرویس")

            ' 24. امور حقوقی و دعاوی
            Dim nLegal = tvMenu.Nodes.Add("nLegal", "⚖️ امور حقوقی و دعاوی")
            nLegal.Nodes.Add("miLegalMain", "⚖️ سیستم مدیریت امور حقوقی")
            nLegal.Nodes.Add("miLegalReports", "📊 گزارشات حقوقی و قراردادها")

            ' 25. تحقیق و توسعه (R&D)
            Dim nRd = tvMenu.Nodes.Add("nRd", "🧪 تحقیق و توسعه")
            nRd.Nodes.Add("miRdMain", "🧪 سیستم جامع تحقیق و توسعه")
            nRd.Nodes.Add("miRdReports", "📊 گزارشات R&D")

            ' 26. مرکز تلفن هوشمند (VoIP)
            Dim nVoip = tvMenu.Nodes.Add("nVoip", "📞 مرکز تلفن هوشمند")
            nVoip.Nodes.Add("miVoipMain", "📞 سیستم مدیریت مرکز تلفن")
            nVoip.Nodes.Add("miVoipReports", "📊 لاگ‌ها و گزارشات VoIP")

            ' 27. پوسته‌های مشاغل
            Dim nShell = tvMenu.Nodes.Add("nShell", "🏠 پوسته‌های مشاغل")
            nShell.Nodes.Add("miShellGeneral", "🏬 پوسته عمومی و بازرگانی")
            nShell.Nodes.Add("miShellRetail", "🏪 پوسته فروشگاهی و اصناف")
            nShell.Nodes.Add("miShellServices", "🏢 پوسته خدماتی و شرکتی")

            ' 28. امکانات و ابزارها
            Dim nUtil = tvMenu.Nodes.Add("nUtil", "🧰 امکانات و ابزارها")
            nUtil.Nodes.Add("miUtilCalculator", "🧮 ماشین حساب سیستم")
            nUtil.Nodes.Add("miUtilNotes", "📝 یادداشت‌های من")
            nUtil.Nodes.Add("miUtilCalendar", "📅 تقویم و رویدادنما")
        End Sub

        Private Sub TvMenu_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvMenu.NodeMouseClick
            If e.Node Is Nothing Then Return

            ' Parent category node -> expand / collapse top-to-bottom AND show buttons on Dashboard Panel (flpDashboard)
            If e.Node.Nodes.Count > 0 Then
                If e.Node.IsExpanded Then
                    e.Node.Collapse()
                Else
                    e.Node.Expand()
                End If

                ' Display category buttons in Dashboard Panel (flpDashboard) on Parent Form
                Select Case e.Node.Name
                    Case "nSys" : ShowDashboardCategory("SystemMgmt")
                    Case "nUsers" : ShowDashboardCategory("UserMgmt")
                    Case "nComp" : ShowDashboardCategory("CompanyMgmt")
                    Case "nAcc" : ShowDashboardCategory("Accounting")
                    Case "nTrade" : ShowDashboardCategory("TradeWarehouse")
                    Case "nPay" : ShowDashboardCategory("Payroll")
                    Case "nAmval" : ShowDashboardCategory("Amval")
                    Case "nAuto" : ShowDashboardCategory("Automation")
                    Case "nCrm" : ShowDashboardCategory("Crm")
                    Case "nTreasury" : ShowDashboardCategory("Treasury")
                    Case "nBudget" : ShowDashboardCategory("Budgeting")
                    Case "nProd" : ShowDashboardCategory("Production")
                    Case "nProj" : ShowDashboardCategory("Project")
                    Case "nKpi" : ShowDashboardCategory("Kpi")
                    Case "nImp" : ShowDashboardCategory("ImportExport")
                    Case "nPm" : ShowDashboardCategory("Pm")
                    Case "nLog" : ShowDashboardCategory("Logistics")
                    Case "nSrm" : ShowDashboardCategory("Srm")
                    Case "nQc" : ShowDashboardCategory("Qc")
                    Case "nBi" : ShowDashboardCategory("Bi")
                    Case "nDms" : ShowDashboardCategory("Dms")
                    Case "nSaham" : ShowDashboardCategory("Saham")
                    Case "nApi" : ShowDashboardCategory("Api")
                    Case "nLegal" : ShowDashboardCategory("Legal")
                    Case "nRd" : ShowDashboardCategory("Rd")
                    Case "nVoip" : ShowDashboardCategory("Voip")
                    Case "nShell" : ShowDashboardCategory("BusinessShells")
                    Case "nUtil" : ShowDashboardCategory("Utilities")
                End Select

                UpdateSidebarBounds()
                Return
            End If

            ' Leaf menu node -> execute module action
            Select Case e.Node.Name
                Case "miSettingsMessages" : MiSettingsMessages_Click(sender, e)
                Case "miSettingsThemes" : MiSettingsThemes_Click(sender, e)
                Case "miBackupData" : MiBackupData_Click(sender, e)
                Case "miRestoreData" : MiRestoreData_Click(sender, e)
                Case "miCreateRelease" : MiCreateRelease_Click(sender, e)
                Case "miCreateUpdate" : MiCreateUpdate_Click(sender, e)
                Case "miExportDecryptedDb" : MiExportDecryptedDb_Click(sender, e)
                Case "miLock" : LockApplication()
                Case "miAbout" : MiAbout_Click(sender, e)
                Case "miContact" : MiContact_Click(sender, e)
                Case "miExit" : MiExit_Click(sender, e)

                Case "miUsers" : MiUsers_Click(sender, e)
                Case "miBasicUsers" : MiBasicUsers_Click(sender, e)
                Case "miChangeProfile" : MiChangeProfile_Click(sender, e)
                Case "miSwitchUser" : MiSwitchUser_Click(sender, e)

                Case "miCompanyFiscalYears" : LblCompany_DoubleClick(sender, e)

                Case "miAccountingMain" : MiAccountingMain_Click(sender, e)
                Case "miReportsAccounting" : MiReportsAccounting_Click(sender, e)

                Case "miTradeMini" : MiTradeMini_Click(sender, e)
                Case "miTradeMedium" : MiTradeMedium_Click(sender, e)
                Case "miTradeBig" : MiTradeBig_Click(sender, e)
                Case "miTradeWarehouseMain" : MiTradeWarehouseMain_Click(sender, e)
                Case "miReportsTrade" : MiReportsTrade_Click(sender, e)

                Case "miPayrollMain", "miPayrollReports" : OpenPayrollMainForm(sender, e)
                Case "miAmvalMain", "miAmvalReports" : OpenAmvalMainForm(sender, e)
                Case "miAutomationMain", "miAutomationReports" : OpenAutomationMainForm(sender, e)
                Case "miCrmMain", "miCrmReports" : OpenCrmMainForm(sender, e)
                Case "miTreasuryMain", "miTreasuryReports" : OpenTreasuryMainForm(sender, e)
                Case "miBudgetingMain", "miBudgetingReports" : OpenBudgetingMainForm(sender, e)
                Case "miProductionMain", "miProductionReports" : OpenProductionMainForm(sender, e)
                Case "miProjectMain", "miProjectReports" : OpenProjectMainForm(sender, e)
                Case "miKpiMain", "miKpiReports" : OpenKpiMainForm(sender, e)
                Case "miImportExportMain", "miImportExportReports" : OpenImportExportMainForm(sender, e)
                Case "miPmMain", "miPmReports" : OpenPmMainForm(sender, e)
                Case "miLogisticsMain", "miLogisticsReports" : OpenLogisticsMainForm(sender, e)
                Case "miSrmMain", "miSrmReports" : OpenSrmMainForm(sender, e)
                Case "miQcMain", "miQcReports" : OpenQcMainForm(sender, e)
                Case "miBiMain", "miBiReports" : OpenBiMainForm(sender, e)
                Case "miDmsMain", "miDmsReports" : OpenDmsMainForm(sender, e)
                Case "miSahamMain", "miSahamReports" : OpenSahamMainForm(sender, e)
                Case "miApiMain", "miApiReports" : OpenApiMainForm(sender, e)
                Case "miLegalMain", "miLegalReports" : OpenLegalMainForm(sender, e)
                Case "miRdMain", "miRdReports" : OpenRdMainForm(sender, e)
                Case "miVoipMain", "miVoipReports" : OpenVoipMainForm(sender, e)

                Case "miShellGeneral" : MiShellGeneral_Click(sender, e)
                Case "miShellRetail" : MiShellRetail_Click(sender, e)
                Case "miShellServices" : MiShellServices_Click(sender, e)

                Case "miUtilCalculator" : MiUtilCalculator_Click(sender, e)
                Case "miUtilNotes" : MiUtilNotes_Click(sender, e)
                Case "miUtilCalendar" : MiUtilCalendar_Click(sender, e)
            End Select
        End Sub

        Private Sub PnlRightMenuContent_MouseWheel(sender As Object, e As MouseEventArgs) Handles pnlRightMenuContent.MouseWheel, mainMenu.MouseWheel, tool.MouseWheel
            Try
                Dim newY = pnlRightMenuContent.VerticalScroll.Value - e.Delta
                If newY < 0 Then newY = 0
                If newY > pnlRightMenuContent.VerticalScroll.Maximum Then newY = pnlRightMenuContent.VerticalScroll.Maximum
                pnlRightMenuContent.VerticalScroll.Value = newY
            Catch ex As Exception
            End Try
        End Sub

        Private Sub LblCompany_DoubleClick(sender As Object, e As EventArgs) Handles lblCompany.DoubleClick
            OpenChild(New CompanyFiscalYearForm(Me, openOnSelectTab:=True))
            UpdateStatusBar()
        End Sub

        Private Sub LblFiscalYear_DoubleClick(sender As Object, e As EventArgs) Handles lblFiscalYear.DoubleClick
            OpenChild(New CompanyFiscalYearForm(Me, openOnSelectTab:=True))
            UpdateStatusBar()
        End Sub

        ''' <summary>
        ''' نمایش پنجره انتخاب سریع سال مالی (Alt+S).
        ''' بررسی می‌کند که آیا فرم سند باز و دارای تغییرات ذخیره‌نشده است یا خیر.
        ''' </summary>
        Public Sub ShowFiscalYearSelector()
            If Not SessionContext.CurrentCompanyID.HasValue Then
                MessageBox.Show("ابتدا یک شرکت انتخاب کنید.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' بررسی فرم‌های سند باز با تغییرات ذخیره‌نشده
            Dim openSanadForms As New List(Of HesabdarySanad2Form)()
            For Each frm As Form In Application.OpenForms
                Dim s2 = TryCast(frm, HesabdarySanad2Form)
                If s2 IsNot Nothing AndAlso s2.HasUnsavedChanges Then
                    openSanadForms.Add(s2)
                End If
            Next

            If openSanadForms.Count > 0 Then
                Dim ans = MessageBox.Show(
                    "پنجره سند حسابداری باز است و دارای اطلاعات ذخیره‌نشده می‌باشد." & Environment.NewLine &
                    "با تغییر سال مالی، اطلاعات ذخیره‌نشده سند از بین خواهد رفت." & Environment.NewLine & Environment.NewLine &
                    "آیا مایل به تغییر سال مالی هستید؟",
                    "تأیید تغییر سال مالی",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2)

                If ans <> DialogResult.Yes Then Return

                ' بستن فرم‌های سند قبل از تغییر سال مالی
                For Each s2 In openSanadForms
                    Try
                        s2.SuppressCloseConfirmation()
                        s2.Close()
                    Catch
                    End Try
                Next
            End If

            ' نمایش پنجره انتخاب سال مالی
            Using selector As New FiscalYearSelectorForm()
                selector.ShowCentered(Me)
                Dim result = selector.ShowDialog(Me)

                If result = DialogResult.OK AndAlso selector.SelectedFiscalYearID.HasValue Then
                    SessionContext.CurrentFiscalYearID = selector.SelectedFiscalYearID.Value
                    SessionContext.CurrentFiscalYearName = selector.SelectedFiscalYearName
                    UpdateStatusBar()
                    ThemeHelper.RefreshAllStatusBars()

                    ' رفرش فوری تمام فرم‌های سند 1 که در حال حاضر باز هستند
                    For Each frm As Form In Application.OpenForms
                        Dim s1 = TryCast(frm, HesabdarySanad1Form)
                        If s1 IsNot Nothing AndAlso Not s1.IsDisposed Then
                            Try
                                s1.RefreshData()
                            Catch
                            End Try
                        End If
                    Next

                    MessageBox.Show(
                        "سال مالی به «" & selector.SelectedFiscalYearName & "» تغییر یافت.",
                        "سال مالی تغییر کرد",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
                End If
            End Using
        End Sub
    End Class
End Namespace


