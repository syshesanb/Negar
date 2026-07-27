Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class UserNotebookService
        Public Function GetUserNotes(userId As Integer) As DataTable
            Dim query As String = "SELECT NoteID, UserID, NoteDate, MainSubject, SubSubject1, SubSubject2, NoteContent, EditHistory FROM UserNotes WHERE UserID = ? ORDER BY NoteDate DESC, NoteID DESC"
            Return Sql.ExecuteTable(query, userId)
        End Function

        Public Function GetNoteById(noteId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT NoteID, UserID, NoteDate, MainSubject, SubSubject1, SubSubject2, NoteContent, EditHistory FROM UserNotes WHERE NoteID = ?", noteId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function SaveNote(noteId As Integer?, userId As Integer, mainSubject As String, subSubject1 As String, subSubject2 As String, noteContent As String) As Integer
            Dim now = DateTime.Now
            If noteId.HasValue AndAlso noteId.Value > 0 Then
                Dim oldRow = GetNoteById(noteId.Value)
                Dim oldHistory As String = If(oldRow IsNot Nothing AndAlso Not oldRow.IsNull("EditHistory"), Convert.ToString(oldRow("EditHistory")), "")
                Dim timestamp = PersianDateHelper.FormatDateTime(now)
                Dim logEntry = String.Format("[تاریخ ویرایش: {0}]: ویرایش ثبت شد.", timestamp)
                Dim newHistory = If(String.IsNullOrWhiteSpace(oldHistory), logEntry, oldHistory & Environment.NewLine & logEntry)

                Sql.ExecuteNonQuery(
                    "UPDATE UserNotes SET MainSubject = ?, SubSubject1 = ?, SubSubject2 = ?, NoteContent = ?, EditHistory = ?, UpdatedDate = ? WHERE NoteID = ?",
                    mainSubject, subSubject1, subSubject2, noteContent, newHistory, now, noteId.Value)
                Return noteId.Value
            Else
                Dim newId = Sql.ExecuteIdentity(
                    "INSERT INTO UserNotes (UserID, NoteDate, MainSubject, SubSubject1, SubSubject2, NoteContent, EditHistory, CreatedDate, UpdatedDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    userId, now, mainSubject, subSubject1, subSubject2, noteContent, "", now, now)
                Return newId
            End If
        End Function

        Public Sub DeleteNote(noteId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM UserNotes WHERE NoteID = ?", noteId)
        End Sub
    End Class
End Namespace
