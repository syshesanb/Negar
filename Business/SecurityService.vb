Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Net
Imports System.Net.Sockets
Imports Negar.Data
Imports Negar.Models

Namespace Negar.Business
    Public Class SecurityService
        Public Function Authenticate(username As String, password As String) As UserAccount
            Dim hashed = PasswordHasher.Hash(password)
            Dim query = "SELECT UserID, Username, [Password], UserType, CreatedBy, CreatedDate, IsActive, FullName, MaxCompaniesAllowed, MaxFiscalYearsPerCompany " &
                        "FROM Users WHERE Username = ? AND [Password] = ? AND (IsActive = 1 OR IsActive = 'True' OR IsActive = true)"
            Dim table = Sql.ExecuteTable(query, username, hashed)
            If table.Rows.Count = 0 Then Return Nothing
            Return MapUser(table.Rows(0))
        End Function

        Public Function LoadPermissions(userId As Integer) As HashSet(Of String)
            Dim query = "SELECT p.PermissionKey, rp.CanView, rp.CanCreate, rp.CanEdit, rp.CanDelete, rp.CanPrint, rp.CanExport " &
                      "FROM Permissions AS p " &
                      "INNER JOIN RolePermissions AS rp ON p.PermissionID = rp.PermissionID " &
                      "WHERE rp.UserID = ?"
            Dim table = Sql.ExecuteTable(query, userId)
            Dim result As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each row As DataRow In table.Rows
                Dim key = Convert.ToString(row("PermissionKey"))
                If Not row.IsNull("CanView") AndAlso Convert.ToBoolean(row("CanView")) Then result.Add(key)
                If Not row.IsNull("CanCreate") AndAlso Convert.ToBoolean(row("CanCreate")) Then result.Add(key & ".CanCreate")
                If Not row.IsNull("CanEdit") AndAlso Convert.ToBoolean(row("CanEdit")) Then result.Add(key & ".CanEdit")
                If Not row.IsNull("CanDelete") AndAlso Convert.ToBoolean(row("CanDelete")) Then result.Add(key & ".CanDelete")
                If Not row.IsNull("CanPrint") AndAlso Convert.ToBoolean(row("CanPrint")) Then result.Add(key & ".CanPrint")
                If Not row.IsNull("CanExport") AndAlso Convert.ToBoolean(row("CanExport")) Then result.Add(key & ".CanExport")
            Next
            Return result
        End Function

        Public Sub SignIn(user As UserAccount)
            SessionContext.CurrentUser = user
            SessionContext.CurrentPermissions = LoadPermissions(user.UserID)
            SessionContext.CurrentIP = GetLocalIP()
            Dim logService As New ActivityLogService()
            logService.LogActivity(user.UserID, "Login", "Session", Nothing, "ورود به سیستم", SessionContext.CurrentIP)
        End Sub

        Public Sub SignOut()
            If SessionContext.CurrentUser IsNot Nothing Then
                Dim logService As New ActivityLogService()
                logService.LogActivity(SessionContext.CurrentUser.UserID, "Logout", "Session", Nothing, "خروج از سیستم", SessionContext.CurrentIP)
            End If
            SessionContext.CurrentUser = Nothing
            SessionContext.CurrentPermissions = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            SessionContext.CurrentIP = "127.0.0.1"
        End Sub

        Public Function GetLocalIP() As String
            Try
                Dim host = Dns.GetHostEntry(Dns.GetHostName())
                For Each ip In host.AddressList
                    If ip.AddressFamily = AddressFamily.InterNetwork Then
                        Return ip.ToString()
                    End If
                Next
            Catch
            End Try
            Return "127.0.0.1"
        End Function

        Private Function MapUser(row As DataRow) As UserAccount
            Dim user As New UserAccount()
            user.UserID = Convert.ToInt32(row("UserID"))
            user.Username = Convert.ToString(row("Username"))
            user.PasswordHash = Convert.ToString(row("Password"))
            user.UserType = Convert.ToString(row("UserType"))
            If row.IsNull("CreatedBy") Then
                user.CreatedBy = Nothing
            Else
                user.CreatedBy = Convert.ToInt32(row("CreatedBy"))
            End If
            user.CreatedDate = If(row.IsNull("CreatedDate"), DateTime.Now, Convert.ToDateTime(row("CreatedDate")))
            user.IsActive = If(row.IsNull("IsActive"), True, Convert.ToBoolean(row("IsActive")))
            user.FullName = Convert.ToString(row("FullName"))
            user.MaxCompaniesAllowed = If(row.Table.Columns.Contains("MaxCompaniesAllowed") AndAlso Not row.IsNull("MaxCompaniesAllowed"), Convert.ToInt32(row("MaxCompaniesAllowed")), 0)
            user.MaxFiscalYearsPerCompany = If(row.Table.Columns.Contains("MaxFiscalYearsPerCompany") AndAlso Not row.IsNull("MaxFiscalYearsPerCompany"), Convert.ToInt32(row("MaxFiscalYearsPerCompany")), 0)
            Return user
        End Function
    End Class
End Namespace
