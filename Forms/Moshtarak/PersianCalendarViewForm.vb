Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class PersianCalendarViewForm
        Inherits Form

        Private ReadOnly _pc As New PersianCalendar()
        Private ReadOnly _hc As New HijriCalendar()
        Private ReadOnly _monthNames As String() = {"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"}

        Private ReadOnly calendarNoteService As New CalendarNoteService()
        Private _currentUserId As Integer

        Private _currentYear As Integer
        Private _currentMonth As Integer
        Private _selectedDay As Integer

        Private lblMonthYear As Label
        Private pnlGrid As TableLayoutPanel
        Private lblSelectedPersian As Label
        Private lblSelectedGregorian As Label
        Private lblSelectedHijri As Label
        Private lblDayOfWeek As Label
        Private txtOccasions As TextBox
        Private tmrClock As Timer
        Private lblLiveClock As Label

        Private txtDayNote As TextBox
        Private chkIsReminder As CheckBox
        Private cboReminderHour As ComboBox
        Private cboReminderMinute As ComboBox
        Private btnSaveDayNote As Button
        Private btnDeleteDayNote As Button

        Private _dayButtons As New List(Of Button)()
        Private _notesInMonth As New HashSet(Of String)()

        Public Sub New()
            InitializeComponentCustom()
            AppIconHelper.ApplyAppIcon(Me)
            _currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)
            Dim now = DateTime.Now
            _currentYear = _pc.GetYear(now)
            _currentMonth = _pc.GetMonth(now)
            _selectedDay = _pc.GetDayOfMonth(now)
            RenderCalendar()
        End Sub

        Private Sub InitializeComponentCustom()
            Me.Size = New Size(980, 700)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Text = "تقویم کامل، مناسبت‌ها و سیستم یادآوری و یادداشت‌گذاری"
            Me.BackColor = Color.FromArgb(245, 247, 250)

            ' Header Panel
            Dim pnlHeader As New Panel() With {.Dock = DockStyle.Top, .Height = 65, .BackColor = Color.FromArgb(41, 128, 185)}
            AddHandler pnlHeader.Paint, Sub(s, e)
                                            Using b As New LinearGradientBrush(pnlHeader.ClientRectangle, Color.FromArgb(41, 128, 185), Color.FromArgb(44, 62, 80), LinearGradientMode.Horizontal)
                                                e.Graphics.FillRectangle(b, pnlHeader.ClientRectangle)
                                            End Using
                                        End Sub

            lblMonthYear = New Label() With {
                .AutoSize = False,
                .Size = New Size(220, 40),
                .Location = New Point(380, 12),
                .Font = New Font("Tahoma", 14.0!, FontStyle.Bold),
                .ForeColor = Color.White,
                .TextAlign = ContentAlignment.MiddleCenter,
                .BackColor = Color.Transparent
            }
            pnlHeader.Controls.Add(lblMonthYear)

            ' Year & Month Navigation Buttons
            Dim btnPrevYear As New Button() With {.Text = "<< سال قبل", .Size = New Size(85, 35), .Location = New Point(180, 15), .BackColor = Color.FromArgb(52, 73, 94), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .Font = New Font("Tahoma", 8.5!)}
            btnPrevYear.FlatAppearance.BorderSize = 0
            AddHandler btnPrevYear.Click, Sub(s, e) NavigateYear(-1)
            pnlHeader.Controls.Add(btnPrevYear)

            Dim btnPrevMonth As New Button() With {.Text = "< ماه قبل", .Size = New Size(80, 35), .Location = New Point(285, 15), .BackColor = Color.FromArgb(52, 152, 219), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
            btnPrevMonth.FlatAppearance.BorderSize = 0
            AddHandler btnPrevMonth.Click, Sub(s, e) NavigateMonth(-1)
            pnlHeader.Controls.Add(btnPrevMonth)

            Dim btnNextMonth As New Button() With {.Text = "ماه بعد >", .Size = New Size(80, 35), .Location = New Point(615, 15), .BackColor = Color.FromArgb(52, 152, 219), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
            btnNextMonth.FlatAppearance.BorderSize = 0
            AddHandler btnNextMonth.Click, Sub(s, e) NavigateMonth(1)
            pnlHeader.Controls.Add(btnNextMonth)

            Dim btnNextYear As New Button() With {.Text = "سال بعد >>", .Size = New Size(85, 35), .Location = New Point(715, 15), .BackColor = Color.FromArgb(52, 73, 94), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .Font = New Font("Tahoma", 8.5!)}
            btnNextYear.FlatAppearance.BorderSize = 0
            AddHandler btnNextYear.Click, Sub(s, e) NavigateYear(1)
            pnlHeader.Controls.Add(btnNextYear)

            Dim btnToday As New Button() With {.Text = "امروز", .Size = New Size(70, 35), .Location = New Point(820, 15), .BackColor = Color.FromArgb(46, 204, 113), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
            btnToday.FlatAppearance.BorderSize = 0
            AddHandler btnToday.Click, Sub(s, e) GoToToday()
            pnlHeader.Controls.Add(btnToday)

            lblLiveClock = New Label() With {.AutoSize = True, .Location = New Point(15, 22), .Font = New Font("Tahoma", 11.0!, FontStyle.Bold), .ForeColor = Color.FromArgb(241, 196, 15), .BackColor = Color.Transparent}
            pnlHeader.Controls.Add(lblLiveClock)

            tmrClock = New Timer() With {.Interval = 1000}
            AddHandler tmrClock.Tick, Sub(s, e) lblLiveClock.Text = DateTime.Now.ToString("HH:mm:ss")
            tmrClock.Start()
            lblLiveClock.Text = DateTime.Now.ToString("HH:mm:ss")

            Me.Controls.Add(pnlHeader)

            ' Main Content Split (Left: Details & Notes, Right: Month Grid)
            Dim pnlContent As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .Padding = New Padding(10)}
            pnlContent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0!)) ' Right: Grid
            pnlContent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0!)) ' Left: Details & Notes

            ' Calendar Grid Panel
            pnlGrid = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 7, .RowCount = 7, .BackColor = Color.White, .Padding = New Padding(5)}
            For i As Integer = 0 To 6
                pnlGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 14.28!))
            Next
            pnlGrid.RowStyles.Add(New RowStyle(SizeType.Absolute, 35.0!))
            For i As Integer = 1 To 6
                pnlGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 16.66!))
            Next

            ' Weekday headers
            Dim weekDays As String() = {"شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه"}
            For i As Integer = 0 To 6
                Dim lblW As New Label() With {
                    .Text = weekDays(i),
                    .Dock = DockStyle.Fill,
                    .TextAlign = ContentAlignment.MiddleCenter,
                    .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                    .BackColor = If(i = 6, Color.FromArgb(231, 76, 60), Color.FromArgb(236, 240, 241)),
                    .ForeColor = If(i = 6, Color.White, Color.FromArgb(44, 62, 80))
                }
                pnlGrid.Controls.Add(lblW, i, 0)
            Next

            pnlContent.Controls.Add(pnlGrid, 0, 0)

            Dim pnlDetails As New TableLayoutPanel() With {
                .Dock = DockStyle.Fill,
                .BackColor = Color.White,
                .Padding = New Padding(10),
                .AutoScroll = True,
                .ColumnCount = 1,
                .RowCount = 12
            }
            pnlDetails.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            pnlDetails.BorderStyle = BorderStyle.FixedSingle

            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0!)) ' 0: DetHeader
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0!)) ' 1: DayOfWeek
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0!)) ' 2: Persian
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0!)) ' 3: Gregorian
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0!)) ' 4: Hijri
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0!)) ' 5: OccHeader
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 60.0!)) ' 6: Occasions
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0!)) ' 7: NoteHeader
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 85.0!)) ' 8: DayNote
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0!)) ' 9: RemChk
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 36.0!)) ' 10: TimeSel
            pnlDetails.RowStyles.Add(New RowStyle(SizeType.Absolute, 40.0!)) ' 11: NoteBtns

            Dim lblDetHeader As New Label() With {.Text = "مشخصات و یادداشت روز انتخابی", .Dock = DockStyle.Fill, .Font = New Font("Tahoma", 10.5!, FontStyle.Bold), .ForeColor = Color.FromArgb(41, 128, 185), .TextAlign = ContentAlignment.MiddleCenter}
            pnlDetails.Controls.Add(lblDetHeader, 0, 0)

            lblDayOfWeek = New Label() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 12.0!, FontStyle.Bold), .ForeColor = Color.FromArgb(39, 174, 96), .TextAlign = ContentAlignment.MiddleCenter}
            pnlDetails.Controls.Add(lblDayOfWeek, 0, 1)

            lblSelectedPersian = New Label() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.5!, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleLeft}
            pnlDetails.Controls.Add(lblSelectedPersian, 0, 2)

            lblSelectedGregorian = New Label() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.0!), .TextAlign = ContentAlignment.MiddleLeft}
            pnlDetails.Controls.Add(lblSelectedGregorian, 0, 3)

            lblSelectedHijri = New Label() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.0!), .TextAlign = ContentAlignment.MiddleLeft}
            pnlDetails.Controls.Add(lblSelectedHijri, 0, 4)

            Dim lblOccHeader As New Label() With {.Text = "مناسبت‌های رسمی:", .Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.0!, FontStyle.Bold), .ForeColor = Color.FromArgb(44, 62, 80), .TextAlign = ContentAlignment.BottomLeft}
            pnlDetails.Controls.Add(lblOccHeader, 0, 5)

            txtOccasions = New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .ReadOnly = True, .ScrollBars = ScrollBars.Vertical, .BackColor = Color.FromArgb(250, 250, 250), .Font = New Font("Tahoma", 8.5!)}
            pnlDetails.Controls.Add(txtOccasions, 0, 6)

            ' Notes & Reminder Section
            Dim lblNoteHeader As New Label() With {.Text = "📝 یادداشت و یادآوری شخصی این روز:", .Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.5!, FontStyle.Bold), .ForeColor = Color.FromArgb(142, 68, 173), .TextAlign = ContentAlignment.BottomLeft}
            pnlDetails.Controls.Add(lblNoteHeader, 0, 7)

            txtDayNote = New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical, .Font = New Font("Tahoma", 9.0!)}
            pnlDetails.Controls.Add(txtDayNote, 0, 8)

            ' Reminder Checkbox Row
            chkIsReminder = New CheckBox() With {.Text = "🔔 یادآوری فعال باشد", .Dock = DockStyle.Fill, .Font = New Font("Tahoma", 8.5!, FontStyle.Bold), .ForeColor = Color.FromArgb(44, 62, 80)}
            pnlDetails.Controls.Add(chkIsReminder, 0, 9)

            ' Dedicated Time Selector Row
            Dim pnlTimeSel As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Margin = New Padding(0)}
            Dim lblHourPrompt As New Label() With {.Text = "ساعت:", .AutoSize = True, .Margin = New Padding(0, 6, 2, 0), .Font = New Font("Tahoma", 8.5!, FontStyle.Bold)}
            cboReminderHour = New ComboBox() With {.Width = 55, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Tahoma", 8.5!)}
            For i As Integer = 0 To 23
                cboReminderHour.Items.Add(i.ToString("00"))
            Next

            Dim lblColon As New Label() With {.Text = ":", .AutoSize = True, .Margin = New Padding(3, 4, 3, 0), .Font = New Font("Tahoma", 10.0!, FontStyle.Bold)}

            Dim lblMinPrompt As New Label() With {.Text = "دقیقه:", .AutoSize = True, .Margin = New Padding(10, 6, 2, 0), .Font = New Font("Tahoma", 8.5!, FontStyle.Bold)}
            cboReminderMinute = New ComboBox() With {.Width = 55, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Tahoma", 8.5!)}
            For i As Integer = 0 To 59
                cboReminderMinute.Items.Add(i.ToString("00"))
            Next

            pnlTimeSel.Controls.Add(lblHourPrompt)
            pnlTimeSel.Controls.Add(cboReminderHour)
            pnlTimeSel.Controls.Add(lblColon)
            pnlTimeSel.Controls.Add(lblMinPrompt)
            pnlTimeSel.Controls.Add(cboReminderMinute)
            pnlDetails.Controls.Add(pnlTimeSel, 0, 10)

            Dim pnlNoteBtns As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(0, 4, 0, 0)}
            btnSaveDayNote = New Button() With {.Text = "💾 ذخیره یادداشت", .Size = New Size(120, 32), .BackColor = Color.FromArgb(39, 174, 96), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .Font = New Font("Tahoma", 8.5!, FontStyle.Bold)}
            btnSaveDayNote.FlatAppearance.BorderSize = 0
            AddHandler btnSaveDayNote.Click, AddressOf BtnSaveDayNote_Click

            btnDeleteDayNote = New Button() With {.Text = "🗑 حذف", .Size = New Size(80, 32), .BackColor = Color.FromArgb(231, 76, 60), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .Font = New Font("Tahoma", 8.5!)}
            btnDeleteDayNote.FlatAppearance.BorderSize = 0
            AddHandler btnDeleteDayNote.Click, AddressOf BtnDeleteDayNote_Click

            pnlNoteBtns.Controls.Add(btnSaveDayNote)
            pnlNoteBtns.Controls.Add(btnDeleteDayNote)
            pnlDetails.Controls.Add(pnlNoteBtns, 0, 11)

            pnlContent.Controls.Add(pnlDetails, 1, 0)
            Me.Controls.Add(pnlContent)
            pnlContent.BringToFront()
        End Sub

        Private Sub GoToToday()
            Dim now = DateTime.Now
            _currentYear = _pc.GetYear(now)
            _currentMonth = _pc.GetMonth(now)
            _selectedDay = _pc.GetDayOfMonth(now)
            RenderCalendar()
        End Sub

        Private Sub NavigateYear(delta As Integer)
            _currentYear += delta
            RenderCalendar()
        End Sub

        Private Sub NavigateMonth(delta As Integer)
            _currentMonth += delta
            If _currentMonth > 12 Then
                _currentMonth = 1
                _currentYear += 1
            ElseIf _currentMonth < 1 Then
                _currentMonth = 12
                _currentYear -= 1
            End If
            _selectedDay = 1
            RenderCalendar()
        End Sub

        Private Sub RenderCalendar()
            lblMonthYear.Text = _monthNames(_currentMonth - 1) & " " & _currentYear

            ' Load note indicators for current month
            Dim prefix = String.Format("{0:0000}/{1:00}", _currentYear, _currentMonth)
            _notesInMonth = calendarNoteService.GetMonthNoteDates(_currentUserId, prefix)

            ' Clear day buttons from grid
            For Each btn In _dayButtons
                pnlGrid.Controls.Remove(btn)
                btn.Dispose()
            Next
            _dayButtons.Clear()

            Dim dtFirst As DateTime
            Try
                dtFirst = _pc.ToDateTime(_currentYear, _currentMonth, 1, 0, 0, 0, 0)
            Catch
                Return
            End Try

            Dim startCol As Integer = (CInt(dtFirst.DayOfWeek) + 1) Mod 7
            Dim daysInMonth As Integer = _pc.GetDaysInMonth(_currentYear, _currentMonth)

            Dim now = DateTime.Now
            Dim isCurrentMonthToday = (_currentYear = _pc.GetYear(now) AndAlso _currentMonth = _pc.GetMonth(now))
            Dim todayDay = _pc.GetDayOfMonth(now)

            Dim currentRow As Integer = 1
            Dim currentCol As Integer = startCol

            For day As Integer = 1 To daysInMonth
                Dim d = day
                Dim dateStr = String.Format("{0:0000}/{1:00}/{2:00}", _currentYear, _currentMonth, d)
                Dim hasNote = _notesInMonth.Contains(dateStr)

                Dim btnDay As New Button() With {
                    .Text = If(hasNote, d.ToString() & " 📝", d.ToString()),
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(2),
                    .FlatStyle = FlatStyle.Flat,
                    .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                    .Cursor = Cursors.Hand
                }
                btnDay.FlatAppearance.BorderSize = 1

                Dim isFriday = (currentCol = 6)
                Dim isToday = (isCurrentMonthToday AndAlso d = todayDay)
                Dim isSelected = (d = _selectedDay)

                If isSelected Then
                    btnDay.BackColor = Color.FromArgb(41, 128, 185)
                    btnDay.ForeColor = Color.White
                ElseIf isToday Then
                    btnDay.BackColor = Color.FromArgb(46, 204, 113)
                    btnDay.ForeColor = Color.White
                ElseIf isFriday Then
                    btnDay.BackColor = Color.FromArgb(253, 237, 236)
                    btnDay.ForeColor = Color.FromArgb(231, 76, 60)
                Else
                    btnDay.BackColor = Color.FromArgb(248, 249, 250)
                    btnDay.ForeColor = Color.FromArgb(44, 62, 80)
                End If

                AddHandler btnDay.Click, Sub(s, e)
                                             _selectedDay = d
                                             RenderCalendar()
                                         End Sub

                pnlGrid.Controls.Add(btnDay, currentCol, currentRow)
                _dayButtons.Add(btnDay)

                currentCol += 1
                If currentCol > 6 Then
                    currentCol = 0
                    currentRow += 1
                End If
            Next

            UpdateDetailsPanel()
        End Sub

        Private Sub UpdateDetailsPanel()
            Try
                Dim dt = _pc.ToDateTime(_currentYear, _currentMonth, _selectedDay, 0, 0, 0, 0)
                Dim dayOfWeekStr = dt.ToString("dddd", New CultureInfo("fa-IR"))
                lblDayOfWeek.Text = dayOfWeekStr

                Dim selectedPersianDateStr = String.Format("{0:0000}/{1:00}/{2:00}", _currentYear, _currentMonth, _selectedDay)
                lblSelectedPersian.Text = String.Format("شمسی: {0} {1} {2}", _selectedDay, _monthNames(_currentMonth - 1), _currentYear)
                lblSelectedGregorian.Text = String.Format("میلادی: {0}", dt.ToString("yyyy/MM/dd (dddd)"))

                Dim hYear = _hc.GetYear(dt)
                Dim hMonth = _hc.GetMonth(dt)
                Dim hDay = _hc.GetDayOfMonth(dt)
                lblSelectedHijri.Text = String.Format("قمری: {0}/{1:00}/{2:00}", hYear, hMonth, hDay)

                txtOccasions.Text = GetOccasionText(_currentMonth, _selectedDay)

                ' Load User Note for selected day
                Dim noteRow = calendarNoteService.GetNote(_currentUserId, selectedPersianDateStr)
                If noteRow IsNot Nothing Then
                    txtDayNote.Text = If(noteRow.IsNull("NoteText"), "", Convert.ToString(noteRow("NoteText")))
                    chkIsReminder.Checked = (Not noteRow.IsNull("IsReminder") AndAlso Convert.ToInt32(noteRow("IsReminder")) = 1)
                    SetReminderTime(If(noteRow.IsNull("ReminderTime"), "", Convert.ToString(noteRow("ReminderTime"))))
                Else
                    txtDayNote.Text = ""
                    chkIsReminder.Checked = False
                    SetReminderTime("09:00")
                End If
            Catch
            End Try
        End Sub

        Private Sub SetReminderTime(timeStr As String)
            Dim hStr As String = "09"
            Dim mStr As String = "00"
            If Not String.IsNullOrWhiteSpace(timeStr) AndAlso timeStr.Contains(":") Then
                Dim parts = timeStr.Split(":"c)
                If parts.Length >= 2 Then
                    hStr = parts(0).Trim().PadLeft(2, "0"c)
                    mStr = parts(1).Trim().PadLeft(2, "0"c)
                End If
            End If
            If Not cboReminderHour.Items.Contains(hStr) Then hStr = "09"
            If Not cboReminderMinute.Items.Contains(mStr) Then mStr = "00"
            cboReminderHour.SelectedItem = hStr
            cboReminderMinute.SelectedItem = mStr
        End Sub

        Private Function GetReminderTimeStr() As String
            Dim h = If(cboReminderHour.SelectedItem IsNot Nothing, cboReminderHour.SelectedItem.ToString(), "09")
            Dim m = If(cboReminderMinute.SelectedItem IsNot Nothing, cboReminderMinute.SelectedItem.ToString(), "00")
            Return h & ":" & m
        End Function

        Private Sub BtnSaveDayNote_Click(sender As Object, e As EventArgs)
            Try
                Dim selectedPersianDateStr = String.Format("{0:0000}/{1:00}/{2:00}", _currentYear, _currentMonth, _selectedDay)
                If String.IsNullOrWhiteSpace(txtDayNote.Text) AndAlso Not chkIsReminder.Checked Then
                    calendarNoteService.DeleteNote(_currentUserId, selectedPersianDateStr)
                Else
                    calendarNoteService.SaveNote(_currentUserId, selectedPersianDateStr, txtDayNote.Text.Trim(), chkIsReminder.Checked, GetReminderTimeStr())
                End If
                MessageBox.Show("یادداشت / یادآوری این روز با موفقیت ذخیره شد.", "ذخیره شد", MessageBoxButtons.OK, MessageBoxIcon.Information)
                RenderCalendar()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی یادداشت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnDeleteDayNote_Click(sender As Object, e As EventArgs)
            Try
                Dim selectedPersianDateStr = String.Format("{0:0000}/{1:00}/{2:00}", _currentYear, _currentMonth, _selectedDay)
                calendarNoteService.DeleteNote(_currentUserId, selectedPersianDateStr)
                txtDayNote.Text = ""
                chkIsReminder.Checked = False
                RenderCalendar()
            Catch ex As Exception
                MessageBox.Show("خطا در حذف یادداشت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetOccasionText(month As Integer, day As Integer) As String
            Dim list As New List(Of String)()

            If month = 1 AndAlso day = 1 Then list.Add("جشن نوروز / آغاز سال نو هجری شمسی")
            If month = 1 AndAlso day = 2 Then list.Add("عید نوروز")
            If month = 1 AndAlso day = 3 Then list.Add("عید نوروز")
            If month = 1 AndAlso day = 4 Then list.Add("عید نوروز")
            If month = 1 AndAlso day = 12 Then list.Add("روز جمهوری اسلامی ایران (تعطیل)")
            If month = 1 AndAlso day = 13 Then list.Add("روز طبیعت (سیزده بدر - تعطیل)")
            If month = 1 AndAlso day = 18 Then list.Add("روز سلامتی (روز جهانی بهداشت)")
            If month = 1 AndAlso day = 29 Then list.Add("روز ارتش جمهوری اسلامی ایران")

            If month = 2 AndAlso day = 1 Then list.Add("روز بزرگداشت سعدی")
            If month = 2 AndAlso day = 3 Then list.Add("روز بزرگداشت شیخ بهایی - روز معماری")
            If month = 2 AndAlso day = 9 Then list.Add("روز شوراها")
            If month = 2 AndAlso day = 12 Then list.Add("روز معلم / شهادت آیت‌الله مطهری")
            If month = 2 AndAlso day = 25 Then list.Add("روز بزرگداشت فردوسی")
            If month = 2 AndAlso day = 28 Then list.Add("روز بزرگداشت حکیم عمر خیام")

            If month = 3 AndAlso day = 1 Then list.Add("روز بهره‌وری و بهینه‌سازی مصرف")
            If month = 3 AndAlso day = 3 Then list.Add("فتح خرمشهر در عملیات بیت‌المقدس / روز مقاومت و پیروزی")
            If month = 3 AndAlso day = 14 Then list.Add("رحلت حضرت امام خمینی (ره) (تعطیل)")
            If month = 3 AndAlso day = 15 Then list.Add("قیام خونین ۱۵ خرداد (تعطیل)")

            If month = 4 AndAlso day = 1 Then list.Add("روز اصناف")
            If month = 4 AndAlso day = 10 Then list.Add("روز صنعت و معدن")
            If month = 4 AndAlso day = 14 Then list.Add("روز قلم")
            If month = 4 AndAlso day = 25 Then list.Add("روز بهزیستی و تامین اجتماعی")

            If month = 5 AndAlso day = 8 Then list.Add("روز بزرگداشت شیخ شهاب‌الدین سهروردی")
            If month = 5 AndAlso day = 17 Then list.Add("روز خبرنگار")
            If month = 5 AndAlso day = 28 Then list.Add("سالروز کودتای ۲۸ مرداد")

            If month = 6 AndAlso day = 1 Then list.Add("روز پزشک / بزرگداشت ابوعلی سینا")
            If month = 6 AndAlso day = 2 Then list.Add("آغاز هفته دولت")
            If month = 6 AndAlso day = 4 Then list.Add("روز کارمند")
            If month = 6 AndAlso day = 13 Then list.Add("روز بزرگداشت ابوریحان بیرونی")

            If month = 7 AndAlso day = 8 Then list.Add("روز بزرگداشت مولوی")
            If month = 7 AndAlso day = 20 Then list.Add("روز بزرگداشت حافظ")

            If month = 8 AndAlso day = 7 Then list.Add("روز بزرگداشت کوروش بزرگ")
            If month = 8 AndAlso day = 24 Then list.Add("روز کتاب و کتابخوانی")

            If month = 9 AndAlso day = 16 Then list.Add("روز دانشجو")
            If month = 9 AndAlso day = 30 Then list.Add("جشن شب یلدا (طولانی‌ترین شب سال)")

            If month = 10 AndAlso day = 20 Then list.Add("شهادت امیرکبیر")
            If month = 10 AndAlso day = 30 Then list.Add("فاجعه آتش‌سوزی ساختمان پلاسکو و روز آتش‌نشان")

            If month = 11 AndAlso day = 12 Then list.Add("بازگشت امام خمینی (ره) به ایران و آغاز دهه فجر")
            If month = 11 AndAlso day = 22 Then list.Add("پیروزی انقلاب اسلامی (تعطیل)")
            If month = 11 AndAlso day = 29 Then list.Add("روز عشق ایرانی (سپندارمذگان)")

            If month = 12 AndAlso day = 5 Then list.Add("روز مهندس / بزرگداشت خواجه نصیرالدین طوسی")
            If month = 12 AndAlso day = 15 Then list.Add("روز درختکاری")
            If month = 12 AndAlso day = 29 Then list.Add("روز ملی شدن صنعت نفت ایران (تعطیل)")

            If list.Count = 0 Then Return "مناسبت خاصی برای این روز ثبت نشده است."
            Return String.Join(Environment.NewLine, list.ToArray())
        End Function
    End Class
End Namespace
