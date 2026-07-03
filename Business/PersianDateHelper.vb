Option Strict Off
Option Explicit On

Imports System
Imports System.Globalization
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Business
    Public Module PersianDateHelper
        Private ReadOnly _pc As New PersianCalendar()

        Public Function ToPersian(dt As DateTime) As String
            Return String.Format("{0:0000}/{1:00}/{2:00}", _pc.GetYear(dt), _pc.GetMonth(dt), _pc.GetDayOfMonth(dt))
        End Function

        Public Function ToPersianFull(dt As DateTime) As String
            Return ToPersian(dt) & "  " & dt.ToString("HH:mm:ss")
        End Function

        ' تاریخ را هوشمندانه فرمت می‌کند: اگر زمان داشت کامل، وگرنه فقط تاریخ
        Public Function FormatDateTime(dt As DateTime) As String
            If dt.TimeOfDay = TimeSpan.Zero Then
                Return ToPersian(dt)
            Else
                Return ToPersianFull(dt)
            End If
        End Function

        Public Function ParsePersianDate(persianDate As String) As DateTime?
            If String.IsNullOrWhiteSpace(persianDate) Then Return Nothing
            Dim cleanDate = persianDate.Replace("/", "").Replace(" ", "")
            If cleanDate.Length = 0 Then Return Nothing
            Try
                Dim parts = persianDate.Split("/"c)
                If parts.Length = 3 Then
                    Dim year = Integer.Parse(parts(0))
                    Dim month = Integer.Parse(parts(1))
                    Dim day = Integer.Parse(parts(2))
                    Return _pc.ToDateTime(year, month, day, 0, 0, 0, 0)
                End If
            Catch
            End Try
            Return Nothing
        End Function

        ' برای استفاده در رویداد CellFormatting هر DataGridView
        Public Sub ApplyToGrid(sender As Object, e As DataGridViewCellFormattingEventArgs)
            If e.Value Is Nothing OrElse e.Value Is DBNull.Value Then Return
            If TypeOf e.Value Is DateTime Then
                e.Value = FormatDateTime(CType(e.Value, DateTime))
                e.FormattingApplied = True
            End If
        End Sub

        ' نمایش تاریخ شمسی در Label کنار DateTimePicker
        Public Sub UpdatePersianLabel(lbl As Label, dtp As DateTimePicker)
            If lbl Is Nothing OrElse dtp Is Nothing Then Return
            Try
                If dtp.ShowCheckBox AndAlso Not dtp.Checked Then
                    lbl.Text = "-"
                Else
                    lbl.Text = FormatDateTime(dtp.Value)
                End If
            Catch
                lbl.Text = ""
            End Try
        End Sub
    End Module
End Namespace
