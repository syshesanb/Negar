Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class ActivityLogService

        ''' <summary>
        ''' Returns a list of UserIDs that the specified user is allowed to see.
        ''' SuperAdmin = Nothing (means all). Manager = self + their created users. User = self + their manager.
        ''' </summary>
        Public Shared Function GetVisibleUserIDs(userId As Integer, userType As String) As List(Of Integer)
            Dim result As New List(Of Integer)()
            result.Add(userId)
            If String.Equals(userType, "Manager", StringComparison.OrdinalIgnoreCase) Then
                ' مدیر: خودش + کاربران عادی که ایجاد کرده
                Dim dt = Sql.ExecuteTable("SELECT UserID FROM Users WHERE CreatedBy = ?", userId)
                For Each row As DataRow In dt.Rows
                    result.Add(Convert.ToInt32(row("UserID")))
                Next
            Else
                ' کاربر عادی: خودش + مدیری که او را ایجاد کرده
                Dim managerIdObj = Sql.ExecuteScalar("SELECT CreatedBy FROM Users WHERE UserID = ?", userId)
                If managerIdObj IsNot Nothing AndAlso managerIdObj IsNot DBNull.Value Then
                    Dim managerId = Convert.ToInt32(managerIdObj)
                    If Not result.Contains(managerId) Then result.Add(managerId)
                End If
            End If
            Return result
        End Function

        Public Shared Function BuildIDInClause(ids As List(Of Integer)) As String
            Dim parts As New List(Of String)()
            For Each id As Integer In ids
                parts.Add(id.ToString())
            Next
            Return String.Join(",", parts.ToArray())
        End Function

        Public Sub LogActivity(userId As Integer, activityType As String, entityType As String, entityId As Integer?, description As String, ipAddress As String)
            Try
                Sql.ExecuteNonQuery(
                    "INSERT INTO ActivityLog (UserID, ActivityType, EntityType, EntityID, Description, IPAddress, ActivityDate) VALUES (?, ?, ?, ?, ?, ?, ?)",
                    userId,
                    activityType,
                    If(String.IsNullOrWhiteSpace(entityType), CType(DBNull.Value, Object), entityType),
                    If(entityId.HasValue, CType(entityId.Value, Object), DBNull.Value),
                    If(String.IsNullOrWhiteSpace(description), CType(DBNull.Value, Object), description),
                    If(String.IsNullOrWhiteSpace(ipAddress), CType(DBNull.Value, Object), ipAddress),
                    DateTime.Now)
            Catch
                ' logging must never break the application
            End Try
        End Sub

        Public Function GetActivityLog() As DataTable
            Dim currentUser = SessionContext.CurrentUser
            If currentUser Is Nothing Then Return New DataTable()

            Dim baseQuery As String =
                "SELECT al.LogID, u.Username, u.FullName, al.ActivityType, al.EntityType, al.EntityID, al.Description, al.IPAddress, al.ActivityDate " &
                "FROM ActivityLog AS al INNER JOIN Users AS u ON al.UserID = u.UserID"

            If String.Equals(currentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable(baseQuery & " ORDER BY al.ActivityDate DESC")
            End If

            Dim visibleIds = GetVisibleUserIDs(currentUser.UserID, currentUser.UserType)
            If visibleIds.Count = 0 Then Return New DataTable()

            Return Sql.ExecuteTable(baseQuery & " WHERE al.UserID IN (" & BuildIDInClause(visibleIds) & ") ORDER BY al.ActivityDate DESC")
        End Function
    End Class
End Namespace
