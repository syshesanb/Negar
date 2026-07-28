Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Models

Namespace Negar.Business
    Public Module ThemeHelper
        Public Sub ApplyFormTheme(frm As Form)
            If String.IsNullOrEmpty(SessionContext.CurrentFormThemeColorHex) Then
                Dim svc As New SettingsService()
                SessionContext.CurrentFormThemeColorHex = svc.GetSettingValue("AdvancedFormThemeColor", "")
            End If

            If Not String.IsNullOrEmpty(SessionContext.CurrentFormThemeColorHex) Then
                Try
                    Dim col = ColorTranslator.FromHtml(SessionContext.CurrentFormThemeColorHex)
                    frm.BackColor = col

                    Dim tintR = CInt(255 - ((255 - col.R) * 0.15))
                    Dim tintG = CInt(255 - ((255 - col.G) * 0.15))
                    Dim tintB = CInt(255 - ((255 - col.B) * 0.15))
                    Dim altColor = Color.FromArgb(255, tintR, tintG, tintB)

                    ApplyGridTheme(frm, altColor)
                Catch
                End Try
            End If
        End Sub

        Private Sub ApplyGridTheme(parent As Control, altColor As Color)
            For Each ctrl As Control In parent.Controls
                ' اگر کنترل یک فرم است (فرم‌های هدردهی شده فرعی)، آن را رد می‌کنیم زیرا خود تم اختصاصی‌اش را اعمال می‌کند
                If TypeOf ctrl Is Form Then Continue For

                If TypeOf ctrl Is DataGridView Then
                    Dim dgv = DirectCast(ctrl, DataGridView)
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = altColor
                End If
                If ctrl.HasChildren Then
                    ApplyGridTheme(ctrl, altColor)
                End If
            Next
        End Sub

        ' ─── تم‌های سه گانه انبارداری (مینی، متوسط، بزرگ) ──────────────────────
        Public Structure EditionThemePalette
            Public HeaderBg As Color
            Public HeaderFg As Color
            Public ActiveTabBg As Color
            Public ActiveTabFg As Color
            Public InactiveTabBg As Color
            Public InactiveTabFg As Color
            Public GridHeaderBg As Color
            Public GridHeaderFg As Color
            Public GridAltRowBg As Color
            Public BorderAccent As Color
        End Structure

        Public Function GetEditionPalette(edition As AppEdition) As EditionThemePalette
            Dim p As New EditionThemePalette()
            Select Case edition
                Case AppEdition.Mini
                    ' سبز زمرّدی فروشگاهی (POS Store Vibe)
                    p.HeaderBg = Color.FromArgb(30, 126, 52)      ' Emerald Green #1E7E34
                    p.HeaderFg = Color.White
                    p.ActiveTabBg = Color.FromArgb(46, 125, 50)   ' Forest Green #2E7D32
                    p.ActiveTabFg = Color.White
                    p.InactiveTabBg = Color.FromArgb(232, 245, 233) ' Soft Mint #E8F5E9
                    p.InactiveTabFg = Color.FromArgb(33, 33, 33)
                    p.GridHeaderBg = Color.FromArgb(30, 126, 52)
                    p.GridHeaderFg = Color.White
                    p.GridAltRowBg = Color.FromArgb(241, 248, 233)
                    p.BorderAccent = Color.FromArgb(76, 175, 80)

                Case AppEdition.Medium
                    ' کهربایی/مسی گرم و سرمه‌ای اقیانوسی (Warm Amber Bronze Vibe)
                    p.HeaderBg = Color.FromArgb(216, 67, 21)      ' Deep Copper #D84315
                    p.HeaderFg = Color.White
                    p.ActiveTabBg = Color.FromArgb(198, 40, 40)   ' Crimson Amber #C62828
                    p.ActiveTabFg = Color.White
                    p.InactiveTabBg = Color.FromArgb(255, 243, 224) ' Soft Amber Gold #FFF3E0
                    p.InactiveTabFg = Color.FromArgb(33, 33, 33)
                    p.GridHeaderBg = Color.FromArgb(216, 67, 21)
                    p.GridHeaderFg = Color.White
                    p.GridAltRowBg = Color.FromArgb(253, 242, 233)
                    p.BorderAccent = Color.FromArgb(255, 112, 67)

                Case AppEdition.Big
                    ' سرمه‌ای سلطنتی و عنابی با آکسان طلایی (Enterprise Midnight Royal Vibe)
                    p.HeaderBg = Color.FromArgb(26, 35, 126)      ' Midnight Blue #1A237E
                    p.HeaderFg = Color.White
                    p.ActiveTabBg = Color.FromArgb(40, 53, 147)   ' Royal Blue #283593
                    p.ActiveTabFg = Color.FromArgb(255, 215, 0)   ' Gold Accented Text #FFD700
                    p.InactiveTabBg = Color.FromArgb(232, 234, 246) ' Soft Slate Royal #E8EAF6
                    p.InactiveTabFg = Color.FromArgb(33, 33, 33)
                    p.GridHeaderBg = Color.FromArgb(26, 35, 126)
                    p.GridHeaderFg = Color.White
                    p.GridAltRowBg = Color.FromArgb(238, 242, 250)
                    p.BorderAccent = Color.FromArgb(255, 215, 0)   ' Gold Accent
            End Select
            Return p
        End Function

        Public Sub ApplyEditionTheme(frm As Form, edition As AppEdition)
            If frm Is Nothing Then Return
            Dim palette = GetEditionPalette(edition)

            ' افزودن بنر بالای فرم در صورت اصلی بودن فرم‌های انبارداری
            If frm.Name = "AnbarMiniMainForm" OrElse frm.Name = "AnbardaryMainForm" Then
                AttachEditionHeaderBanner(frm, edition, palette)
            End If

            ApplyEditionToControls(frm, palette)
        End Sub

        Private Sub AttachEditionHeaderBanner(frm As Form, edition As AppEdition, palette As EditionThemePalette)
            Dim bannerTag = "AppEditionHeaderBanner"
            For Each ctrl As Control In frm.Controls
                If Convert.ToString(ctrl.Tag) = bannerTag Then Return
            Next

            Dim pnlBanner As New Panel()
            pnlBanner.Tag = bannerTag
            pnlBanner.Dock = DockStyle.Top
            pnlBanner.Height = 42
            pnlBanner.BackColor = palette.HeaderBg
            pnlBanner.RightToLeft = RightToLeft.Yes

            Dim lblTitle As New Label()
            lblTitle.Dock = DockStyle.Fill
            lblTitle.ForeColor = palette.HeaderFg
            lblTitle.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            lblTitle.TextAlign = ContentAlignment.MiddleRight
            lblTitle.Padding = New Padding(15, 0, 15, 0)

            Select Case edition
                Case AppEdition.Mini
                    lblTitle.Text = "🛒 ماژول انبارداری و فروشگاهی - نسخه مینی (POS Mini Edition)"
                Case AppEdition.Medium
                    lblTitle.Text = "🏢 ماژول انبارداری و کالا - نسخه متوسط (Medium Warehouse Edition)"
                Case AppEdition.Big
                    lblTitle.Text = "🏭 ماژول انبارداری و کالا - نسخه بزرگ و پیشرفته (Big Enterprise Edition)"
            End Select

            pnlBanner.Controls.Add(lblTitle)
            frm.Controls.Add(pnlBanner)
            pnlBanner.SendToBack()
        End Sub

        Private Sub ApplyEditionToControls(parent As Control, palette As EditionThemePalette)
            For Each ctrl As Control In parent.Controls
                If TypeOf ctrl Is Form Then Continue For

                If TypeOf ctrl Is DataGridView Then
                    Dim dgv = DirectCast(ctrl, DataGridView)
                    dgv.EnableHeadersVisualStyles = False
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = palette.GridHeaderBg
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = palette.GridHeaderFg
                    dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font.FontFamily, dgv.Font.Size, FontStyle.Bold)
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = palette.GridAltRowBg
                ElseIf TypeOf ctrl Is TabControl Then
                    Dim tc = DirectCast(ctrl, TabControl)
                    StylingTabControl(tc, palette)
                End If

                If ctrl.HasChildren Then
                    ApplyEditionToControls(ctrl, palette)
                End If
            Next
        End Sub

        Private Sub StylingTabControl(tc As TabControl, palette As EditionThemePalette)
            tc.DrawMode = TabDrawMode.OwnerDrawFixed
            AddHandler tc.DrawItem, Sub(sender As Object, e As DrawItemEventArgs) OnTabControlDrawItem(sender, e, palette)
        End Sub

        Private Sub OnTabControlDrawItem(sender As Object, e As DrawItemEventArgs, palette As EditionThemePalette)
            Dim tc = TryCast(sender, TabControl)
            If tc Is Nothing OrElse e.Index < 0 OrElse e.Index >= tc.TabCount Then Return

            Dim tabPage = tc.TabPages(e.Index)
            Dim tabRect = tc.GetTabRect(e.Index)
            Dim isSelected = (tc.SelectedIndex = e.Index)

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias

            Dim bgPenColor = If(isSelected, palette.ActiveTabBg, palette.InactiveTabBg)
            Dim fgColor = If(isSelected, palette.ActiveTabFg, palette.InactiveTabFg)

            Using bgBrush As New SolidBrush(bgPenColor)
                e.Graphics.FillRectangle(bgBrush, tabRect)
            End Using

            ' Draw active tab accent line at top/bottom
            If isSelected Then
                Using accentPen As New Pen(palette.BorderAccent, 3.0!)
                    e.Graphics.DrawLine(accentPen, tabRect.Left, tabRect.Top + 1, tabRect.Right, tabRect.Top + 1)
                End Using
            End If

            ' Draw text using TextRenderer (GDI) for native Emoji fallback & crisp RTL text
            Dim fStyle As FontStyle = If(isSelected, FontStyle.Bold, FontStyle.Regular)
            Using textFont As New Font("Tahoma", 9.5!, fStyle)
                Dim flags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.RightToLeft Or TextFormatFlags.SingleLine
                TextRenderer.DrawText(e.Graphics, tabPage.Text, textFont, tabRect, fgColor, flags)
            End Using
        End Sub

        ' ─── نوار وضعیت مشترک برای همه فرم‌ها ─────────────────────────────────
        Private Const AppStatusTag As String = "AppSharedStatusBar"

        ''' <summary>
        ''' یک نوار وضعیت مشترک (کاربر / شرکت / سال مالی / ساعت) را در زمان اجرا
        ''' به هر فرم اضافه می‌کند. در رویداد Load هر فرم یک‌بار فراخوانی شود.
        ''' </summary>
        Public Sub AppendStatusBar(frm As Form)
            ' فقط برای فرم‌های TopLevel (مستقل) نوار وضعیت ایجاد می‌کنیم
            ' فرم‌های Hosted (که TopLevel = False هستند) نباید نوار وضعیت داشته باشند
            If frm Is Nothing OrElse Not frm.TopLevel Then Return

            ' خود MainForm نیاز به نوار وضعیت داینامیک ندارد چون در طراحی خود نوار وضعیت دارد
            If frm.Name = "MainForm" Then Return

            ' جلوگیری از افزودن مجدد
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is StatusStrip Then
                    Dim existing = DirectCast(ctrl, StatusStrip)
                    If Convert.ToString(existing.Tag) = AppStatusTag Then Return
                End If
            Next

            AttachStatusBarNow(frm)
        End Sub

        ''' <summary>واقعاً StatusStrip را می‌سازد و به فرم می‌چسباند.</summary>
        Private Sub AttachStatusBarNow(frm As Form)
            ' جلوگیری از افزودن مجدد (دوباره بررسی می‌شود)
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is StatusStrip Then
                    Dim existing = DirectCast(ctrl, StatusStrip)
                    If Convert.ToString(existing.Tag) = AppStatusTag Then Return
                End If
            Next

            ' ── ساخت StatusStrip ─────────────────────────────────────────────
            Dim strip As New StatusStrip()
            strip.Tag = AppStatusTag
            strip.RightToLeft = RightToLeft.Yes
            strip.SizingGrip = False
            strip.BackColor = Color.FromArgb(41, 98, 180)
            strip.ForeColor = Color.White
            strip.Font = New Font("Tahoma", 8.5!)
            strip.Height = 24

            Dim lblUser As New ToolStripStatusLabel()
            lblUser.Name = "ssLblUser"
            lblUser.ForeColor = Color.White
            lblUser.Text = BuildUserText()

            Dim sep1 As New ToolStripSeparator()

            Dim lblCompany As New ToolStripStatusLabel()
            lblCompany.Name = "ssLblCompany"
            lblCompany.ForeColor = Color.White
            lblCompany.Text = BuildCompanyText()

            Dim sep2 As New ToolStripSeparator()

            Dim lblFY As New ToolStripStatusLabel()
            lblFY.Name = "ssLblFY"
            lblFY.ForeColor = Color.FromArgb(255, 220, 120)
            lblFY.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)
            lblFY.Text = BuildFYText()
            lblFY.ToolTipText = "برای تغییر سریع سال مالی کلیدهای Alt+S را فشار دهید"

            Dim spring As New ToolStripStatusLabel()
            spring.Name = "ssSpring"
            spring.Spring = True

            Dim lblHint As New ToolStripStatusLabel()
            lblHint.Name = "ssLblHint"
            lblHint.ForeColor = Color.FromArgb(180, 220, 255)
            lblHint.Font = New Font("Tahoma", 7.5!)
            lblHint.Text = "   Alt+S : تغییر سریع سال مالی   "

            Dim sep3 As New ToolStripSeparator()

            Dim lblClock As New ToolStripStatusLabel()
            lblClock.Name = "ssLblClock"
            lblClock.ForeColor = Color.White
            lblClock.Text = BuildClockText()

            strip.Items.AddRange(New ToolStripItem() {
                lblUser, sep1, lblCompany, sep2, lblFY,
                spring, lblHint, sep3, lblClock})

            frm.Controls.Add(strip)

            ' ── تایمر ۱ ثانیه‌ای برای به‌روز نگه داشتن ساعت ────────────────
            Dim tmr As New Timer()
            tmr.Interval = 1000
            tmr.Tag = strip
            AddHandler tmr.Tick, AddressOf OnStatusClockTick
            tmr.Start()

            AddHandler frm.FormClosed,
                Sub(s, ev)
                    Try
                        tmr.Stop()
                        tmr.Dispose()
                    Catch
                    End Try
                End Sub
        End Sub

        ''' <summary>نوار وضعیت تمام فرم‌های باز را به‌روز می‌کند.</summary>
        Public Sub RefreshAllStatusBars()
            For Each frm As Form In Application.OpenForms
                RefreshStatusBar(frm)
            Next
        End Sub

        ''' <summary>نوار وضعیت یک فرم خاص را به‌روز می‌کند.</summary>
        Public Sub RefreshStatusBar(frm As Form)
            If frm Is Nothing OrElse frm.IsDisposed Then Return
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is StatusStrip Then
                    Dim ss = DirectCast(ctrl, StatusStrip)
                    If Convert.ToString(ss.Tag) <> AppStatusTag Then Continue For
                    For Each item As ToolStripItem In ss.Items
                        Select Case item.Name
                            Case "ssLblUser"    : item.Text = BuildUserText()
                            Case "ssLblCompany" : item.Text = BuildCompanyText()
                            Case "ssLblFY"      : item.Text = BuildFYText()
                            Case "ssLblClock"   : item.Text = BuildClockText()
                        End Select
                    Next
                End If
            Next
        End Sub

        Private Sub OnStatusClockTick(sender As Object, e As EventArgs)
            Dim tmr = TryCast(sender, Timer)
            If tmr Is Nothing Then Return
            Dim strip = TryCast(tmr.Tag, StatusStrip)
            If strip Is Nothing OrElse strip.IsDisposed Then Return
            For Each item As ToolStripItem In strip.Items
                If item.Name = "ssLblClock" Then
                    item.Text = BuildClockText()
                    Exit For
                End If
            Next
        End Sub

        ' ─── رشته‌سازهای متن ──────────────────────────────────────────────────
        Private Function BuildUserText() As String
            Dim name = If(SessionContext.CurrentUser IsNot Nothing AndAlso
                          Not String.IsNullOrWhiteSpace(SessionContext.CurrentUser.FullName),
                          SessionContext.CurrentUser.FullName, "-")
            Return "  " & name & "  "
        End Function

        Private Function BuildCompanyText() As String
            Dim name = If(SessionContext.CurrentCompanyID.HasValue AndAlso
                          Not String.IsNullOrWhiteSpace(SessionContext.CurrentCompanyName),
                          SessionContext.CurrentCompanyName, "-")
            Return "  " & name & "  "
        End Function

        Private Function BuildFYText() As String
            Dim fy = If(SessionContext.CurrentFiscalYearID.HasValue AndAlso
                        Not String.IsNullOrWhiteSpace(SessionContext.CurrentFiscalYearName),
                        SessionContext.CurrentFiscalYearName, "-")
            Return "  سال مالی: " & fy & "  "
        End Function

        Private Function BuildClockText() As String
            Dim pc As New PersianCalendar()
            Dim now = DateTime.Now
            Return String.Format("  {0:0000}/{1:00}/{2:00}   {3:HH:mm:ss}  ",
                                 pc.GetYear(now), pc.GetMonth(now), pc.GetDayOfMonth(now), now)
        End Function
    End Module
End Namespace
