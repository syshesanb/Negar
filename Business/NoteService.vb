Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business

    Public Class NoteService

        Public Function GetLinesWithNotes(entryId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT DISTINCT LineNumber FROM EntryNotes " &
                "WHERE EntryID = ? AND LineNumber IS NOT NULL " &
                "ORDER BY LineNumber",
                entryId)
        End Function

        ' یادداشت‌های همه کاربران برای یک سند یا ردیف
        Public Function GetNotes(entryId As Integer, lineNumber As Integer?) As DataTable
            If lineNumber.HasValue Then
                Return Sql.ExecuteTable(
                    "SELECT n.NoteID, IFNULL(u.FullName, u.Username) AS UserDisplayName, " &
                    "n.NoteText, n.UserID, n.ModifiedDate " &
                    "FROM EntryNotes n INNER JOIN Users u ON n.UserID = u.UserID " &
                    "WHERE n.EntryID = ? AND n.LineNumber = ? " &
                    "ORDER BY n.ModifiedDate DESC",
                    entryId, lineNumber.Value)
            Else
                Return Sql.ExecuteTable(
                    "SELECT n.NoteID, IFNULL(u.FullName, u.Username) AS UserDisplayName, " &
                    "n.NoteText, n.UserID, n.ModifiedDate " &
                    "FROM EntryNotes n INNER JOIN Users u ON n.UserID = u.UserID " &
                    "WHERE n.EntryID = ? AND n.LineNumber IS NULL " &
                    "ORDER BY n.ModifiedDate DESC",
                    entryId)
            End If
        End Function

        ' یادداشت کاربر جاری برای یک سند یا ردیف
        Public Function GetCurrentUserNote(entryId As Integer, lineNumber As Integer?,
                                           userId As Integer) As String
            Dim dt As DataTable
            If lineNumber.HasValue Then
                dt = Sql.ExecuteTable(
                    "SELECT NoteText FROM EntryNotes WHERE EntryID = ? AND LineNumber = ? AND UserID = ?",
                    entryId, lineNumber.Value, userId)
            Else
                dt = Sql.ExecuteTable(
                    "SELECT NoteText FROM EntryNotes WHERE EntryID = ? AND LineNumber IS NULL AND UserID = ?",
                    entryId, userId)
            End If
            If dt.Rows.Count = 0 Then Return ""
            Dim val = dt.Rows(0)("NoteText")
            Return Convert.ToString(If(val Is Nothing OrElse val Is DBNull.Value, "", val))
        End Function

        ' ذخیره یا آپدیت یادداشت کاربر جاری
        Public Sub SaveNote(entryId As Integer, lineNumber As Integer?,
                            userId As Integer, noteText As String)
            Dim count As Integer
            If lineNumber.HasValue Then
                count = Convert.ToInt32(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM EntryNotes WHERE EntryID = ? AND LineNumber = ? AND UserID = ?",
                    entryId, lineNumber.Value, userId))
            Else
                count = Convert.ToInt32(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM EntryNotes WHERE EntryID = ? AND LineNumber IS NULL AND UserID = ?",
                    entryId, userId))
            End If

            If count > 0 Then
                If lineNumber.HasValue Then
                    Sql.ExecuteNonQuery(
                        "UPDATE EntryNotes SET NoteText = ?, ModifiedDate = ? " &
                        "WHERE EntryID = ? AND LineNumber = ? AND UserID = ?",
                        noteText, Date.Now, entryId, lineNumber.Value, userId)
                Else
                    Sql.ExecuteNonQuery(
                        "UPDATE EntryNotes SET NoteText = ?, ModifiedDate = ? " &
                        "WHERE EntryID = ? AND LineNumber IS NULL AND UserID = ?",
                        noteText, Date.Now, entryId, userId)
                End If
            Else
                If lineNumber.HasValue Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO EntryNotes (EntryID, LineNumber, NoteText, UserID, CreatedDate, ModifiedDate) " &
                        "VALUES (?, ?, ?, ?, ?, ?)",
                        entryId, lineNumber.Value, noteText, userId, Date.Now, Date.Now)
                Else
                    Sql.ExecuteNonQuery(
                        "INSERT INTO EntryNotes (EntryID, LineNumber, NoteText, UserID, CreatedDate, ModifiedDate) " &
                        "VALUES (?, NULL, ?, ?, ?, ?)",
                        entryId, noteText, userId, Date.Now, Date.Now)
                End If
            End If
        End Sub

    End Class

End Namespace
