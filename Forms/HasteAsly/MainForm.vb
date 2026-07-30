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
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            AppIconHelper.ApplyAppIcon(Me)
            UpdateStatusBar()
            clockTimer.Start()
            _shortcutFilter = New GlobalShortcutFilter(Me)
            Application.AddMessageFilter(_shortcutFilter)
            LoadRandomBackgroundImage()
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
            mBusinessShells.Visible = canBusinessShells OrElse isSuperAdmin
            mUtilities.Visible = canUtilities OrElse isSuperAdmin

            btnToolSystemMgmt.Visible = True
            btnToolUserMgmt.Visible = canUsers OrElse canBasicUsers OrElse canSwitchUser OrElse canChangePassword
            btnToolCompanyMgmt.Visible = canCompanyYears
            btnToolAccounting.Visible = canAccounting OrElse canReports
            btnToolTradeWarehouse.Visible = canTrade OrElse canReports
            btnToolBusinessShells.Visible = canBusinessShells OrElse isSuperAdmin
            btnToolUtilities.Visible = canUtilities OrElse isSuperAdmin

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

        Private Sub OpenChild(child As Form)
            child.StartPosition = FormStartPosition.CenterParent
            child.Show(Me)
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
            Dim form As New CompanyFiscalYearForm(Me)
            form.StartPosition = FormStartPosition.CenterParent
            form.ShowDialog(Me)
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
                    AddDashButton("سیستم جامع مدیریت ارتباط با مشتریان (CRM)", AddressOf OpenCrmMainForm, "Users")
                    AddDashButton("گزارشات جامع CRM و فروش", AddressOf OpenCrmMainForm, "Reports")

                Case "Treasury"
                    AddDashButton("سیستم جامع خزانه‌داری و مدیریت نقدینگی", AddressOf OpenTreasuryMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات جامع خزانه‌داری و Cash Flow", AddressOf OpenTreasuryMainForm, "Reports")

                Case "Budgeting"
                    AddDashButton("سیستم جامع بودجه و کنترل هزینه‌ها", AddressOf OpenBudgetingMainForm, "CompanyFiscalYears")
                    AddDashButton("گزارشات انحراف بودجه و انضباط مالی", AddressOf OpenBudgetingMainForm, "Reports")

                Case "Production"
                    AddDashButton("سیستم جامع بهای تمام‌شده و برنامه‌ریزی تولید", AddressOf OpenProductionMainForm, "TradeWarehouse")
                    AddDashButton("گزارشات جامع بهای تمام‌شده و آنالیز BOM", AddressOf OpenProductionMainForm, "Reports")

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

            flpDashboard.ResumeLayout()
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

        Private Sub BtnToolSystemMgmt_Click(sender As Object, e As EventArgs) Handles btnToolSystemMgmt.Click
            ShowDashboardCategory("SystemMgmt")
        End Sub

        Private Sub BtnToolUserMgmt_Click(sender As Object, e As EventArgs) Handles btnToolUserMgmt.Click
            ShowDashboardCategory("UserMgmt")
        End Sub

        Private Sub BtnToolCompanyMgmt_Click(sender As Object, e As EventArgs) Handles btnToolCompanyMgmt.Click
            ShowDashboardCategory("CompanyMgmt")
        End Sub

        Private Sub BtnToolAccounting_Click(sender As Object, e As EventArgs) Handles btnToolAccounting.Click
            ShowDashboardCategory("Accounting")
        End Sub

        Private Sub BtnToolTradeWarehouse_Click(sender As Object, e As EventArgs) Handles btnToolTradeWarehouse.Click
            ShowDashboardCategory("TradeWarehouse")
        End Sub

        Private Sub BtnToolPayroll_Click(sender As Object, e As EventArgs) Handles btnToolPayroll.Click
            ShowDashboardCategory("Payroll")
        End Sub

        Private Sub OpenPayrollMainForm(sender As Object, e As EventArgs) Handles mPayroll.Click, miPayrollMain.Click, miPayrollReports.Click
            Try
                If Not EnsureCompanyAndFiscalYearSelected() Then Return
                Using dlg As New Negar.Forms.Payroll.PayrollMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.Amval.AmvalMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.Automation.AutomationMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.CRM.CrmMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.Treasury.TreasuryMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.Budgeting.BudgetMainForm()
                    dlg.ShowDialog(Me)
                End Using
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
                Using dlg As New Negar.Forms.Production.ProductionMainForm()
                    dlg.ShowDialog(Me)
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در باز کردن ماژول بهای تمام‌شده و تولید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnToolBusinessShells_Click(sender As Object, e As EventArgs) Handles btnToolBusinessShells.Click
            ShowDashboardCategory("BusinessShells")
        End Sub

        Private Sub BtnToolUtilities_Click(sender As Object, e As EventArgs) Handles btnToolUtilities.Click
            ShowDashboardCategory("Utilities")
        End Sub

        Private Sub LblCompany_DoubleClick(sender As Object, e As EventArgs) Handles lblCompany.DoubleClick
            Dim form As New CompanyFiscalYearForm(Me)
            form.StartPosition = FormStartPosition.CenterParent
            form.ShowDialog(Me)
            UpdateStatusBar()
        End Sub

        Private Sub LblFiscalYear_DoubleClick(sender As Object, e As EventArgs) Handles lblFiscalYear.DoubleClick
            Dim form As New CompanyFiscalYearForm(Me)
            form.StartPosition = FormStartPosition.CenterParent
            form.ShowDialog(Me)
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


