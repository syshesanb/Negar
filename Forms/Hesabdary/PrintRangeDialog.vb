Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class PrintRangeDialog
        Inherits Form

        Public Property PrintByRef As Boolean = True
        Public Property FromRef As Integer? = Nothing
        Public Property ToRef As Integer? = Nothing
        Public Property FromDate As String = String.Empty
        Public Property ToDate As String = String.Empty

        Private _suppressDateChange As Boolean = False

        Public Sub New()
            MyBase.New()
            InitializeComponent()
        End Sub

        Private Sub PrintRangeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            ' تاریخ‌ها به طور پیش‌فرض خالی می‌مانند تا کاربر بتواند کل بازه را فیلتر کند
            txtFromDate.Text = String.Empty
            txtToDate.Text = String.Empty

            ' به‌روزرسانی وضعیت فعال بودن گروه‌ها
            UpdateGroupsEnabledState()
        End Sub

        Private Sub rdoByRef_CheckedChanged(sender As Object, e As EventArgs) Handles rdoByRef.CheckedChanged
            UpdateGroupsEnabledState()
        End Sub

        Private Sub rdoByDate_CheckedChanged(sender As Object, e As EventArgs) Handles rdoByDate.CheckedChanged
            UpdateGroupsEnabledState()
        End Sub

        Private Sub UpdateGroupsEnabledState()
            grpByRef.Enabled = rdoByRef.Checked
            grpByDate.Enabled = rdoByDate.Checked

            If rdoByRef.Checked Then
                txtFromRef.Focus()
            Else
                txtFromDate.Focus()
            End If
        End Sub

        ' ---- اعتبارسنجی ورودی‌های شماره سند ----
        Private Sub txtRef_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFromRef.KeyPress, txtToRef.KeyPress
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        ' ---- تاریخ شمسی: ورودی با قالب‌بندی خودکار ----
        Private Sub txtDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFromDate.KeyPress, txtToDate.KeyPress
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub txtFromDate_TextChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged
            FormatDateTextBox(txtFromDate)
        End Sub

        Private Sub txtToDate_TextChanged(sender As Object, e As EventArgs) Handles txtToDate.TextChanged
            FormatDateTextBox(txtToDate)
        End Sub

        Private Sub FormatDateTextBox(txtBox As TextBox)
            If _suppressDateChange Then Return
            Dim txt = txtBox.Text
            Dim digits = New String(txt.Where(Function(c) Char.IsDigit(c)).ToArray())
            If digits.Length > 8 Then digits = digits.Substring(0, 8)
            Dim formatted = FormatPersianDigits(digits)
            If formatted = txt Then Return
            _suppressDateChange = True
            txtBox.Text = formatted
            txtBox.SelectionStart = formatted.Length
            _suppressDateChange = False
        End Sub

        Private Shared Function FormatPersianDigits(digits As String) As String
            Select Case digits.Length
                Case <= 4 : Return digits
                Case <= 6 : Return digits.Substring(0, 4) & "/" & digits.Substring(4)
                Case Else : Return digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6)
            End Select
        End Function

        ' ---- دکمه‌های باز کردن تقویم شمسی ----
        Private Sub btnCalFromDate_Click(sender As Object, e As EventArgs) Handles btnCalFromDate.Click
            ShowCalendarForTextBox(txtFromDate)
        End Sub

        Private Sub btnCalToDate_Click(sender As Object, e As EventArgs) Handles btnCalToDate.Click
            ShowCalendarForTextBox(txtToDate)
        End Sub

        Private Sub ShowCalendarForTextBox(txtBox As TextBox)
            Dim anchor = EnsureOnScreen(
                txtBox.PointToScreen(New Point(0, txtBox.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtBox.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtBox.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, formSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - formSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - formSize.Height)))
        End Function

        ' ---- دکمه‌های تایید و انصراف ----
        Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
            If rdoByRef.Checked Then
                ' بازه شماره سند (هر فیلدی که خالی باشد نادیده گرفته می‌شود)
                Dim fromNum As Integer? = Nothing
                Dim toNum As Integer? = Nothing
                
                If Not String.IsNullOrWhiteSpace(txtFromRef.Text) Then
                    Dim parsed As Integer
                    If Integer.TryParse(txtFromRef.Text.Trim(), parsed) Then
                        fromNum = parsed
                    Else
                        MessageBox.Show("شماره سند شروع وارد شده معتبر نمی‌باشد.", "خطای ورودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
                
                If Not String.IsNullOrWhiteSpace(txtToRef.Text) Then
                    Dim parsed As Integer
                    If Integer.TryParse(txtToRef.Text.Trim(), parsed) Then
                        toNum = parsed
                    Else
                        MessageBox.Show("شماره سند پایان وارد شده معتبر نمی‌باشد.", "خطای ورودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
                
                Me.PrintByRef = True
                Me.FromRef = fromNum
                Me.ToRef = toNum
                Me.FromDate = String.Empty
                Me.ToDate = String.Empty
            Else
                ' بازه تاریخ (هر فیلدی که خالی باشد نادیده گرفته می‌شود)
                Dim fDateStr = txtFromDate.Text.Trim()
                Dim tDateStr = txtToDate.Text.Trim()
                
                If fDateStr = "//" OrElse fDateStr = "" Then fDateStr = String.Empty
                If tDateStr = "//" OrElse tDateStr = "" Then tDateStr = String.Empty
                
                If Not String.IsNullOrEmpty(fDateStr) Then
                    Dim parsed = PersianDateHelper.ParsePersianDate(fDateStr)
                    If Not parsed.HasValue Then
                        MessageBox.Show("تاریخ شروع وارد شده معتبر نمی‌باشد.", "خطای ورودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
                
                If Not String.IsNullOrEmpty(tDateStr) Then
                    Dim parsed = PersianDateHelper.ParsePersianDate(tDateStr)
                    If Not parsed.HasValue Then
                        MessageBox.Show("تاریخ پایان وارد شده معتبر نمی‌باشد.", "خطای ورودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
                
                Me.PrintByRef = False
                Me.FromRef = Nothing
                Me.ToRef = Nothing
                Me.FromDate = fDateStr
                Me.ToDate = tDateStr
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
