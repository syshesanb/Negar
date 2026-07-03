Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Sys_Hes_Anb.Data
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Business
    Public Class UserService
        Private ReadOnly logService As New ActivityLogService()

        Public Function GetUsers() As DataTable
            If SessionContext.CurrentUser Is Nothing Then
                Return Sql.ExecuteTable("SELECT UserID, Username, UserType, FullName, CreatedDate, IsActive, MaxCompaniesAllowed, MaxFiscalYearsPerCompany FROM Users ORDER BY UserID DESC")
            End If
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable("SELECT UserID, Username, UserType, FullName, CreatedDate, IsActive, MaxCompaniesAllowed, MaxFiscalYearsPerCompany FROM Users ORDER BY UserID DESC")
            End If
            Return Sql.ExecuteTable(
                "SELECT UserID, Username, UserType, FullName, CreatedDate, IsActive, MaxCompaniesAllowed, MaxFiscalYearsPerCompany FROM Users WHERE CreatedBy = ? ORDER BY UserID DESC",
                SessionContext.CurrentUser.UserID)
        End Function

        Public Function GetUsersByTypes(ParamArray userTypes() As String) As DataTable
            If userTypes Is Nothing OrElse userTypes.Length = 0 Then
                Return GetUsers()
            End If

            Dim placeholders As New List(Of String)()
            Dim args As New List(Of Object)()
            For Each userType In userTypes
                placeholders.Add("?")
                args.Add(userType)
            Next

            Dim query = "SELECT UserID, Username, UserType, FullName, CreatedDate, IsActive, MaxCompaniesAllowed, MaxFiscalYearsPerCompany FROM Users WHERE UserType IN (" & String.Join(",", placeholders.ToArray()) & ")"

            ' Non-SuperAdmin can only see users they created
            If SessionContext.CurrentUser IsNot Nothing AndAlso
               Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                query &= " AND CreatedBy = ?"
                args.Add(SessionContext.CurrentUser.UserID)
            End If

            query &= " ORDER BY UserID DESC"
            Return Sql.ExecuteTable(query, args.ToArray())
        End Function

        Public Function GetRoles() As DataTable
            Return Sql.ExecuteTable("SELECT DISTINCT UserType FROM Users ORDER BY UserType")
        End Function

        Public Function SaveUser(userId As Integer?, username As String, password As String, userType As String, fullName As String, isActive As Boolean, createdBy As Integer?, creatorIP As String, Optional maxCompanies As Integer = 0, Optional maxFiscalYears As Integer = 0) As Integer
            Dim hashed = If(String.IsNullOrWhiteSpace(password), Nothing, PasswordHasher.Hash(password))
            If (Not userId.HasValue OrElse userId.Value <= 0) AndAlso String.IsNullOrWhiteSpace(password) Then
                Throw New InvalidOperationException("رمز عبور برای کاربر جدید الزامی است.")
            End If
            If userId.HasValue AndAlso userId.Value > 0 Then
                If String.IsNullOrWhiteSpace(password) Then
                    Sql.ExecuteNonQuery("UPDATE Users SET Username = ?, UserType = ?, FullName = ?, IsActive = ?, MaxCompaniesAllowed = ?, MaxFiscalYearsPerCompany = ? WHERE UserID = ?",
                                        username, userType, fullName, isActive, maxCompanies, maxFiscalYears, userId.Value)
                Else
                    Sql.ExecuteNonQuery("UPDATE Users SET Username = ?, [Password] = ?, UserType = ?, FullName = ?, IsActive = ?, MaxCompaniesAllowed = ?, MaxFiscalYearsPerCompany = ? WHERE UserID = ?",
                                        username, hashed, userType, fullName, isActive, maxCompanies, maxFiscalYears, userId.Value)
                End If
                If SessionContext.CurrentUser IsNot Nothing Then
                    logService.LogActivity(SessionContext.CurrentUser.UserID, "EditUser", "User", userId.Value,
                                           "ویرایش کاربر: " & username, SessionContext.CurrentIP)
                End If
                Return userId.Value
            End If

            Dim duplicate = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE Username = ?", username), 0))
            If duplicate > 0 Then
                Throw New InvalidOperationException("نام کاربری '" & username & "' از قبل در سیستم وجود دارد. لطفاً نام کاربری دیگری انتخاب کنید.")
            End If

            Dim newId = Sql.ExecuteIdentity(
                "INSERT INTO Users (Username, [Password], UserType, CreatedBy, CreatedDate, IsActive, FullName, CreatorIP, MaxCompaniesAllowed, MaxFiscalYearsPerCompany) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                username, hashed, userType,
                If(createdBy.HasValue, CType(createdBy.Value, Object), DBNull.Value),
                DateTime.Now, isActive, fullName,
                If(String.IsNullOrWhiteSpace(creatorIP), CType(DBNull.Value, Object), creatorIP),
                maxCompanies, maxFiscalYears)

            If SessionContext.CurrentUser IsNot Nothing Then
                logService.LogActivity(SessionContext.CurrentUser.UserID, "CreateUser", "User", newId,
                                       "ایجاد کاربر: " & username & " (نوع: " & userType & ")", SessionContext.CurrentIP)
            End If
            Return newId
        End Function

        Public Sub DeleteUser(userId As Integer)
            Dim usernameObj = Sql.ExecuteScalar("SELECT Username FROM Users WHERE UserID = ?", userId)
            Dim username = If(usernameObj Is Nothing OrElse Convert.IsDBNull(usernameObj), "?", Convert.ToString(usernameObj))

            Sql.ExecuteNonQuery("DELETE FROM RolePermissions WHERE UserID = ?", userId)
            Sql.ExecuteNonQuery("DELETE FROM Users WHERE UserID = ?", userId)

            If SessionContext.CurrentUser IsNot Nothing Then
                logService.LogActivity(SessionContext.CurrentUser.UserID, "DeleteUser", "User", userId,
                                       "حذف کاربر: " & username, SessionContext.CurrentIP)
            End If
        End Sub

        Public Function GetPermissions(Optional allowedPermissionKeys As IEnumerable(Of String) = Nothing) As DataTable
            Dim permissions = Sql.ExecuteTable("SELECT PermissionID, PermissionName, PermissionKey, SectionName FROM Permissions ORDER BY PermissionID")
            If allowedPermissionKeys Is Nothing Then
                Return permissions
            End If

            Dim allowed As New HashSet(Of String)(allowedPermissionKeys, StringComparer.OrdinalIgnoreCase)
            Dim filtered As DataTable = permissions.Clone()
            For Each row As DataRow In permissions.Rows
                Dim key = Convert.ToString(row("PermissionKey"))
                If allowed.Contains(key) Then
                    filtered.ImportRow(row)
                End If
            Next
            Return filtered
        End Function

        Public Function GetPermissionMatrix(userId As Integer, Optional allowedPermissionKeys As IEnumerable(Of String) = Nothing) As DataTable
            Dim permissions = GetPermissions(allowedPermissionKeys)
            Dim rolePermissions = Sql.ExecuteTable(
                "SELECT PermissionID, CanView, CanCreate, CanEdit, CanDelete, CanPrint, CanExport " &
                "FROM RolePermissions WHERE UserID = ?",
                userId)

            Dim matrix As New DataTable()
            matrix.Columns.Add("PermissionID", GetType(Integer))
            matrix.Columns.Add("SectionName", GetType(String))
            matrix.Columns.Add("PermissionName", GetType(String))
            matrix.Columns.Add("PermissionKey", GetType(String))
            matrix.Columns.Add("CanView", GetType(Boolean))
            matrix.Columns.Add("CanCreate", GetType(Boolean))
            matrix.Columns.Add("CanEdit", GetType(Boolean))
            matrix.Columns.Add("CanDelete", GetType(Boolean))
            matrix.Columns.Add("CanPrint", GetType(Boolean))
            matrix.Columns.Add("CanExport", GetType(Boolean))

            For Each permRow As DataRow In permissions.Rows
                Dim matchRows = rolePermissions.Select("PermissionID=" & Convert.ToInt32(permRow("PermissionID")).ToString())
                Dim roleRow As DataRow = Nothing
                If matchRows.Length > 0 Then
                    roleRow = matchRows(0)
                End If

                Dim key = Convert.ToString(permRow("PermissionKey"))
                Dim sectionName = If(permRow.IsNull("SectionName"), "", Convert.ToString(permRow("SectionName")))
                matrix.Rows.Add(
                    Convert.ToInt32(permRow("PermissionID")),
                    sectionName,
                    Convert.ToString(permRow("PermissionName")),
                    key,
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanView"), False, Convert.ToBoolean(roleRow("CanView"))),
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanCreate"), False, Convert.ToBoolean(roleRow("CanCreate"))),
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanEdit"), False, Convert.ToBoolean(roleRow("CanEdit"))),
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanDelete"), False, Convert.ToBoolean(roleRow("CanDelete"))),
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanPrint"), False, Convert.ToBoolean(roleRow("CanPrint"))),
                    If(roleRow Is Nothing OrElse roleRow.IsNull("CanExport"), False, Convert.ToBoolean(roleRow("CanExport"))))
            Next

            Return matrix
        End Function

        Public Sub UpdatePermissionSection(permissionId As Integer, sectionName As String)
            Sql.ExecuteNonQuery("UPDATE Permissions SET SectionName = ? WHERE PermissionID = ?", sectionName, permissionId)
        End Sub

        Public Function CanAssignPermission(permissionKey As String, Optional allowedPermissionKeys As IEnumerable(Of String) = Nothing) As Boolean
            If SessionContext.CurrentUser Is Nothing Then Return False
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then Return True

            Dim allowedKeys As IEnumerable(Of String) = allowedPermissionKeys
            If allowedKeys Is Nothing Then
                allowedKeys = SessionContext.CurrentPermissions
            End If

            If allowedKeys Is Nothing Then Return False

            For Each key In allowedKeys
                If String.Equals(key, permissionKey, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Sub SetUserPermission(userId As Integer, permissionId As Integer, canView As Boolean, canCreate As Boolean, canEdit As Boolean, canDelete As Boolean, canPrint As Boolean, canExport As Boolean, Optional allowedPermissionKeys As IEnumerable(Of String) = Nothing)
            Dim permissionKeyObj = Sql.ExecuteScalar("SELECT PermissionKey FROM Permissions WHERE PermissionID = ?", permissionId)
            Dim permissionKey = If(permissionKeyObj Is Nothing OrElse Convert.IsDBNull(permissionKeyObj), "", Convert.ToString(permissionKeyObj))
            If Not CanAssignPermission(permissionKey, allowedPermissionKeys) Then
                Throw New InvalidOperationException("شما اجازه ثبت این مجوز را ندارید.")
            End If

            Dim exists = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM RolePermissions WHERE UserID = ? AND PermissionID = ?", userId, permissionId), 0))
            If exists > 0 Then
                Sql.ExecuteNonQuery("UPDATE RolePermissions SET CanView = ?, CanCreate = ?, CanEdit = ?, CanDelete = ?, CanPrint = ?, CanExport = ? WHERE UserID = ? AND PermissionID = ?",
                                    canView, canCreate, canEdit, canDelete, canPrint, canExport, userId, permissionId)
            Else
                Sql.ExecuteNonQuery("INSERT INTO RolePermissions (UserID, PermissionID, CanView, CanCreate, CanEdit, CanDelete, CanPrint, CanExport) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                                    userId, permissionId, canView, canCreate, canEdit, canDelete, canPrint, canExport)
            End If
        End Sub
    End Class
End Namespace
