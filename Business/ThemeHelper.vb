Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Business
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
