Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class CalendarNoteService
        Public Function GetNote(userId As Integer, persianDate As String) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT CalendarNoteID, UserID, PersianDate, NoteText, ReminderTime, IsReminder FROM CalendarNotes WHERE UserID = ? AND PersianDate = ?", userId, persianDate)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function GetMonthNoteDates(userId As Integer, yearMonthPrefix As String) As HashSet(Of String)
            Dim hash As New HashSet(Of String)()
            Dim dt = Sql.ExecuteTable("SELECT DISTINCT PersianDate FROM CalendarNotes WHERE UserID = ? AND PersianDate LIKE ?", userId, yearMonthPrefix & "%")
            For Each row As DataRow In dt.Rows
                If Not row.IsNull("PersianDate") Then hash.Add(Convert.ToString(row("PersianDate")))
            Next
            Return hash
        End Function

        Public Sub SaveNote(userId As Integer, persianDate As String, noteText As String, isReminder As Boolean, reminderTime As String)
            Dim now = DateTime.Now
            Dim existing = GetNote(userId, persianDate)
            If existing IsNot Nothing Then
                Sql.ExecuteNonQuery(
                    "UPDATE CalendarNotes SET NoteText = ?, IsReminder = ?, ReminderTime = ?, UpdatedDate = ? WHERE UserID = ? AND PersianDate = ?",
                    noteText, If(isReminder, 1, 0), reminderTime, now, userId, persianDate)
            Else
                Sql.ExecuteNonQuery(
                    "INSERT INTO CalendarNotes (UserID, PersianDate, NoteText, IsReminder, ReminderTime, CreatedDate, UpdatedDate) VALUES (?, ?, ?, ?, ?, ?, ?)",
                    userId, persianDate, noteText, If(isReminder, 1, 0), reminderTime, now, now)
            End If
        End Sub

        Public Sub DeleteNote(userId As Integer, persianDate As String)
            Sql.ExecuteNonQuery("DELETE FROM CalendarNotes WHERE UserID = ? AND PersianDate = ?", userId, persianDate)
        End Sub
    End Class
End Namespace
