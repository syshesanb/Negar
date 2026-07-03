Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms

    ''' تقویم شمسی کوچک جهت انتخاب تاریخ — نتیجه از طریق SelectedDate برگشت داده می‌شود
    Public Class PersianCalendarForm
        Inherits Form

        Public Property SelectedDate As String = String.Empty

        Private ReadOnly _pc As New PersianCalendar()
        Private _year As Integer
        Private _month As Integer

        Private _lblMonthYear As Label
        Private _btnPrev As Button
        Private _btnNext As Button
        Private _pnlDays As Panel

        ' ColW=34, gap=2, margin=4 → 7*(34+2)-2+2*4 = 258px داخل فرم
        Private Const ColW As Integer = 34
        Private Const ColGap As Integer = 2
        Private Const CalMargin As Integer = 4
        Private Const RowH As Integer = 26
        Private Const RowGap As Integer = 2

        Private Shared ReadOnly MonthNames As String() = {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"}

        ' ترتیب کد از 0 (شنبه/راست) تا 6 (جمعه/چپ) — با RightToLeftLayout فرم ، 0 → راست
        Private Shared ReadOnly DayHeaders As String() = {"ش", "ی", "د", "س", "چ", "پ", "ج"}

        Public Sub New(Optional initialDate As String = "")
            SetupForm()
            ParseInitialDate(initialDate)
            If _year = 0 Then
                _year = _pc.GetYear(DateTime.Today)
                _month = _pc.GetMonth(DateTime.Today)
            End If
            RenderCalendar()
        End Sub

        Private Sub ParseInitialDate(dateStr As String)
            If String.IsNullOrEmpty(dateStr) Then Return
            Dim parts = dateStr.Split("/"c)
            If parts.Length = 3 Then
                Dim y, m As Integer
                If Integer.TryParse(parts(0), y) AndAlso Integer.TryParse(parts(1), m) AndAlso
                   y > 1300 AndAlso m >= 1 AndAlso m <= 12 Then
                    _year = y
                    _month = m
                End If
            End If
        End Sub

        Private Sub SetupForm()
            Me.Text = "انتخاب تاریخ"
            Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
            Me.ClientSize = New Size(270, 228)
            Me.ShowInTaskbar = False
            Me.TopMost = True
            Me.KeyPreview = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            AddHandler Me.KeyDown, AddressOf CalForm_KeyDown

            ' ---- ردیف سرستون: ‹ ماه/سال › ----
            Dim pnlHeader = New Panel()
            pnlHeader.Dock = DockStyle.Top
            pnlHeader.Height = 30

            ' code-LEFT → visual RIGHT (RTL) → دکمه ماه قبل در سمت راست
            _btnPrev = New Button()
            _btnPrev.Text = "»"
            _btnPrev.Size = New Size(28, 24)
            _btnPrev.Location = New Point(CalMargin, 3)
            _btnPrev.FlatStyle = FlatStyle.Flat
            AddHandler _btnPrev.Click, AddressOf BtnPrev_Click

            _lblMonthYear = New Label()
            _lblMonthYear.TextAlign = ContentAlignment.MiddleCenter
            _lblMonthYear.Size = New Size(198, 24)
            _lblMonthYear.Location = New Point(36, 3)

            ' code-RIGHT → visual LEFT (RTL) → دکمه ماه بعد در سمت چپ
            _btnNext = New Button()
            _btnNext.Text = "«"
            _btnNext.Size = New Size(28, 24)
            _btnNext.Location = New Point(238, 3)
            _btnNext.FlatStyle = FlatStyle.Flat
            AddHandler _btnNext.Click, AddressOf BtnNext_Click

            pnlHeader.Controls.AddRange(New Control() {_btnPrev, _lblMonthYear, _btnNext})

            ' ---- ردیف نام روزها ----
            Dim pnlDayNames = New Panel()
            pnlDayNames.Dock = DockStyle.Top
            pnlDayNames.Height = 22
            For i = 0 To 6
                Dim lbl = New Label()
                lbl.Text = DayHeaders(i)
                lbl.TextAlign = ContentAlignment.MiddleCenter
                lbl.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)
                lbl.Size = New Size(ColW, 20)
                lbl.Location = New Point(CalMargin + i * (ColW + ColGap), 1)
                pnlDayNames.Controls.Add(lbl)
            Next

            ' ---- پنل روزهای ماه ----
            _pnlDays = New Panel()
            _pnlDays.Dock = DockStyle.Fill

            Me.Controls.Add(_pnlDays)
            Me.Controls.Add(pnlDayNames)
            Me.Controls.Add(pnlHeader)
        End Sub

        Private Sub RenderCalendar()
            _lblMonthYear.Text = MonthNames(_month - 1) & "  " & _year.ToString()
            _pnlDays.Controls.Clear()

            ' اولین روز ماه شمسی
            Dim firstDay = _pc.ToDateTime(_year, _month, 1, 0, 0, 0, 0)
            ' ستون شروع: (DayOfWeek+1) mod 7 → شنبه=0 یک‌شنبه=1 ... جمعه=6
            Dim startCol = (CInt(firstDay.DayOfWeek) + 1) Mod 7
            Dim daysInMonth = _pc.GetDaysInMonth(_year, _month)

            Dim col = startCol
            Dim row = 0
            For day = 1 To daysInMonth
                Dim btn = New Button()
                btn.Text = day.ToString()
                btn.Tag = day
                btn.Size = New Size(ColW, RowH)
                btn.Location = New Point(CalMargin + col * (ColW + ColGap), 2 + row * (RowH + RowGap))
                btn.FlatStyle = FlatStyle.Flat
                AddHandler btn.Click, AddressOf DayBtn_Click
                _pnlDays.Controls.Add(btn)

                col += 1
                If col = 7 Then
                    col = 0
                    row += 1
                End If
            Next
        End Sub

        Private Sub DayBtn_Click(sender As Object, e As EventArgs)
            Dim day = CInt(DirectCast(sender, Button).Tag)
            SelectedDate = String.Format("{0:D4}/{1:D2}/{2:D2}", _year, _month, day)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub BtnPrev_Click(sender As Object, e As EventArgs)
            _month -= 1
            If _month = 0 Then
                _month = 12
                _year -= 1
            End If
            RenderCalendar()
        End Sub

        Private Sub BtnNext_Click(sender As Object, e As EventArgs)
            _month += 1
            If _month = 13 Then
                _month = 1
                _year += 1
            End If
            RenderCalendar()
        End Sub

        Private Sub CalForm_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
            End If
        End Sub

    End Class
End Namespace
