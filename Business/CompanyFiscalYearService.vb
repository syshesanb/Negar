Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class CompanyFiscalYearService
        Private ReadOnly logService As New ActivityLogService()

        ' برای کاربر عادی، ID مدیر سازنده‌اش را برمی‌گرداند؛ برای مدیر، ID خودش را
        Private Function GetEffectiveOwnerID() As Integer?
            If SessionContext.CurrentUser Is Nothing Then Return Nothing
            Dim userType = SessionContext.CurrentUser.UserType
            If String.Equals(userType, "Manager", StringComparison.OrdinalIgnoreCase) Then
                Return SessionContext.CurrentUser.UserID
            End If
            ' کاربر عادی: ID مدیری که او را ایجاد کرده
            Dim val = Sql.ExecuteScalar("SELECT CreatedBy FROM Users WHERE UserID = ?", SessionContext.CurrentUser.UserID)
            If val Is Nothing OrElse val Is DBNull.Value Then Return Nothing
            Return Convert.ToInt32(val)
        End Function

        Public Function GetCompanies() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()
            Dim userType = SessionContext.CurrentUser.UserType
            If String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable("SELECT CompanyID, CompanyName, CompanyCode, BrandName, EconomicCode, FiscalYearStartDate, FiscalYearEndDate, PostalCode, RegistrationDate, RegistrationNumber, ActivityField, Address, Phone, Phone2, Email, TaxID, LogoImage, ChairmanName, InspectorName, CEOName, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, Signatory4Title, Signatory4Name, AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length, Level6Length, IsActive, LogoPosition, ProductGroupLevels FROM Companies ORDER BY CompanyName")
            End If
            Dim ownerId = GetEffectiveOwnerID()
            If Not ownerId.HasValue Then Return New DataTable()
            Return Sql.ExecuteTable(
                "SELECT CompanyID, CompanyName, CompanyCode, BrandName, EconomicCode, FiscalYearStartDate, FiscalYearEndDate, PostalCode, RegistrationDate, RegistrationNumber, ActivityField, Address, Phone, Phone2, Email, TaxID, LogoImage, ChairmanName, InspectorName, CEOName, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, Signatory4Title, Signatory4Name, AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length, Level6Length, IsActive, LogoPosition, ProductGroupLevels FROM Companies WHERE OwnerUserID = ? ORDER BY CompanyName",
                ownerId.Value)
        End Function

        Public Function SaveCompany(companyId As Integer?, companyName As String, companyCode As String, brandName As String, economicCode As String, fiscalYearStartDate As Object, fiscalYearEndDate As Object, postalCode As String, registrationDate As Object, registrationNumber As String, activityField As String, address As String, phone As String, phone2 As String, email As String, taxId As String, logoImage() As Byte, chairmanName As String, inspectorName As String, ceoName As String, sig1Title As String, sig1Name As String, sig2Title As String, sig2Name As String, sig3Title As String, sig3Name As String, sig4Title As String, sig4Name As String, accountLevels As Integer, l1 As Integer, l2 As Integer, l3 As Integer, l4 As Integer, l5 As Integer, l6 As Integer, isActive As Boolean, logoPosition As String, productGroupLevels As Integer, Optional ownerUserIdOverride As Integer? = Nothing) As Integer
            Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
            ' If caller supplies an explicit owner, use it; otherwise use the session user
            Dim effectiveOwnerUserId As Integer = If(ownerUserIdOverride.HasValue, ownerUserIdOverride.Value, currentUserId)

            If (Not companyId.HasValue OrElse companyId.Value <= 0) AndAlso SessionContext.CurrentUser IsNot Nothing AndAlso Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim ownerId = GetEffectiveOwnerID()
                If ownerId.HasValue Then
                    Dim maxCompObj = Sql.ExecuteScalar("SELECT MaxCompaniesAllowed FROM Users WHERE UserID = ?", ownerId.Value)
                    Dim maxCompanies As Integer = If(maxCompObj IsNot Nothing AndAlso Not Convert.IsDBNull(maxCompObj), Convert.ToInt32(maxCompObj), 0)
                    If maxCompanies > 0 Then
                        Dim currentCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Companies WHERE OwnerUserID = ?", ownerId.Value), 0))
                        If currentCount >= maxCompanies Then
                            Throw New InvalidOperationException("حداکثر تعداد مجاز برای ثبت شرکت (" & maxCompanies & " شرکت) به اتمام رسیده است و امکان ثبت شرکت جدید وجود ندارد.")
                        End If
                    End If
                End If
            End If

            If companyId.HasValue AndAlso companyId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Companies SET CompanyName = ?, CompanyCode = ?, BrandName = ?, EconomicCode = ?, FiscalYearStartDate = ?, FiscalYearEndDate = ?, PostalCode = ?, RegistrationDate = ?, RegistrationNumber = ?, ActivityField = ?, Address = ?, Phone = ?, Phone2 = ?, Email = ?, TaxID = ?, LogoImage = ?, ChairmanName = ?, InspectorName = ?, CEOName = ?, Signatory1Title = ?, Signatory1Name = ?, Signatory2Title = ?, Signatory2Name = ?, Signatory3Title = ?, Signatory3Name = ?, Signatory4Title = ?, Signatory4Name = ?, AccountLevels = ?, Level1Length = ?, Level2Length = ?, Level3Length = ?, Level4Length = ?, Level5Length = ?, Level6Length = ?, IsActive = ?, LogoPosition = ?, ProductGroupLevels = ? WHERE CompanyID = ?",
                                    companyName, companyCode, brandName, economicCode, fiscalYearStartDate, fiscalYearEndDate, postalCode, registrationDate, registrationNumber, activityField, address, phone, phone2, email, taxId, logoImage, chairmanName, inspectorName, ceoName, sig1Title, sig1Name, sig2Title, sig2Name, sig3Title, sig3Name, sig4Title, sig4Name, accountLevels, l1, l2, l3, l4, l5, l6, isActive, logoPosition, productGroupLevels, companyId.Value)
                logService.LogActivity(currentUserId, "EditCompany", "Company", companyId.Value,
                                       "ویرایش شرکت: " & companyName, SessionContext.CurrentIP)
                Return companyId.Value
            End If

            Dim newId = Sql.ExecuteIdentity(
                "INSERT INTO Companies (CompanyName, CompanyCode, BrandName, EconomicCode, FiscalYearStartDate, FiscalYearEndDate, PostalCode, RegistrationDate, RegistrationNumber, ActivityField, Address, Phone, Phone2, Email, TaxID, LogoImage, ChairmanName, InspectorName, CEOName, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, Signatory4Title, Signatory4Name, AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length, Level6Length, IsActive, LogoPosition, ProductGroupLevels, OwnerUserID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                companyName, companyCode, brandName, economicCode, fiscalYearStartDate, fiscalYearEndDate, postalCode, registrationDate, registrationNumber, activityField, address, phone, phone2, email, taxId, logoImage, chairmanName, inspectorName, ceoName, sig1Title, sig1Name, sig2Title, sig2Name, sig3Title, sig3Name, sig4Title, sig4Name, accountLevels, l1, l2, l3, l4, l5, l6, isActive, logoPosition, productGroupLevels, effectiveOwnerUserId)

            logService.LogActivity(currentUserId, "CreateCompany", "Company", newId,
                                   "ایجاد شرکت: " & companyName, SessionContext.CurrentIP)
            Return newId
        End Function

        Public Sub DeleteCompany(companyId As Integer)
            Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
            Dim nameObj = Sql.ExecuteScalar("SELECT CompanyName FROM Companies WHERE CompanyID = ?", companyId)
            Dim companyName = If(nameObj Is Nothing OrElse Convert.IsDBNull(nameObj), "?", Convert.ToString(nameObj))

            Sql.ExecuteNonQuery("DELETE FROM Companies WHERE CompanyID = ?", companyId)

            logService.LogActivity(currentUserId, "DeleteCompany", "Company", companyId,
                                   "حذف شرکت: " & companyName, SessionContext.CurrentIP)
        End Sub

        Public Function GetFiscalYearsByCompany(companyId As Integer) As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()
            Dim userType = SessionContext.CurrentUser.UserType
            If String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable(
                    "SELECT FiscalYearID, CompanyID, FiscalYearName, StartDate, EndDate, IsActive FROM FiscalYears WHERE CompanyID = ? ORDER BY StartDate DESC",
                    companyId)
            End If
            Dim ownerId = GetEffectiveOwnerID()
            If Not ownerId.HasValue Then Return New DataTable()
            Return Sql.ExecuteTable(
                "SELECT fy.FiscalYearID, fy.CompanyID, fy.FiscalYearName, fy.StartDate, fy.EndDate, fy.IsActive " &
                "FROM FiscalYears AS fy INNER JOIN Companies AS c ON fy.CompanyID = c.CompanyID " &
                "WHERE fy.CompanyID = ? AND c.OwnerUserID = ? ORDER BY fy.StartDate DESC",
                companyId, ownerId.Value)
        End Function

        Public Function GetFiscalYears() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()
            Dim userType = SessionContext.CurrentUser.UserType
            If String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable("SELECT FiscalYearID, CompanyID, FiscalYearName, StartDate, EndDate, IsActive FROM FiscalYears ORDER BY StartDate DESC")
            End If
            Dim ownerId = GetEffectiveOwnerID()
            If Not ownerId.HasValue Then Return New DataTable()
            Return Sql.ExecuteTable(
                "SELECT fy.FiscalYearID, fy.CompanyID, fy.FiscalYearName, fy.StartDate, fy.EndDate, fy.IsActive " &
                "FROM FiscalYears AS fy INNER JOIN Companies AS c ON fy.CompanyID = c.CompanyID " &
                "WHERE c.OwnerUserID = ? ORDER BY fy.StartDate DESC",
                ownerId.Value)
        End Function

        Public Function SaveFiscalYear(fiscalYearId As Integer?, companyId As Integer, fiscalYearName As String, startDate As DateTime, endDate As DateTime, isActive As Boolean) As Integer
            Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)

            If (Not fiscalYearId.HasValue OrElse fiscalYearId.Value <= 0) AndAlso SessionContext.CurrentUser IsNot Nothing AndAlso Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim ownerId = GetEffectiveOwnerID()
                If ownerId.HasValue Then
                    Dim maxFYObj = Sql.ExecuteScalar("SELECT MaxFiscalYearsPerCompany FROM Users WHERE UserID = ?", ownerId.Value)
                    Dim maxFY As Integer = If(maxFYObj IsNot Nothing AndAlso Not Convert.IsDBNull(maxFYObj), Convert.ToInt32(maxFYObj), 0)
                    If maxFY > 0 Then
                        Dim currentCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM FiscalYears WHERE CompanyID = ?", companyId), 0))
                        If currentCount >= maxFY Then
                            Throw New InvalidOperationException("حداکثر تعداد مجاز سال مالی برای این شرکت (" & maxFY & " سال مالی) به اتمام رسیده است و امکان ثبت سال مالی جدید وجود ندارد.")
                        End If
                    End If
                End If
            End If

            If fiscalYearId.HasValue AndAlso fiscalYearId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE FiscalYears SET CompanyID = ?, FiscalYearName = ?, StartDate = ?, EndDate = ?, IsActive = ? WHERE FiscalYearID = ?",
                                    companyId, fiscalYearName, startDate, endDate, isActive, fiscalYearId.Value)
                logService.LogActivity(currentUserId, "EditFiscalYear", "FiscalYear", fiscalYearId.Value,
                                       "ویرایش سال مالی: " & fiscalYearName, SessionContext.CurrentIP)
                Return fiscalYearId.Value
            End If

            Dim newId = Sql.ExecuteIdentity(
                "INSERT INTO FiscalYears (CompanyID, FiscalYearName, StartDate, EndDate, IsActive) VALUES (?, ?, ?, ?, ?)",
                companyId, fiscalYearName, startDate, endDate, isActive)
            logService.LogActivity(currentUserId, "CreateFiscalYear", "FiscalYear", newId,
                                   "ایجاد سال مالی: " & fiscalYearName, SessionContext.CurrentIP)
            Return newId
        End Function

        Public Sub DeleteFiscalYear(fiscalYearId As Integer)
            Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
            Dim nameObj = Sql.ExecuteScalar("SELECT FiscalYearName FROM FiscalYears WHERE FiscalYearID = ?", fiscalYearId)
            Dim fyName = If(nameObj Is Nothing OrElse Convert.IsDBNull(nameObj), "?", Convert.ToString(nameObj))

            Sql.ExecuteNonQuery("DELETE FROM FiscalYears WHERE FiscalYearID = ?", fiscalYearId)

            logService.LogActivity(currentUserId, "DeleteFiscalYear", "FiscalYear", fiscalYearId,
                                   "حذف سال مالی: " & fyName, SessionContext.CurrentIP)
        End Sub

        Public Function ValidateCompanySettingsChange(companyId As Integer, proposedLevels As Integer, proposedLengths As Integer()) As String
            ' proposedLengths contains [Level1Length, Level2Length, Level3Length, Level4Length, Level5Length]
            ' واکشی تمامی سرفصل‌های شرکت
            Dim dt = Sql.ExecuteTable("SELECT AccountID, AccountCode, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", companyId)
            If dt.Rows.Count = 0 Then Return Nothing ' هیچ سرفصلی وجود ندارد

            ' ساخت دیکشنری برای پیمایش والدها و تعیین سطح هر حساب
            Dim parentDict As New System.Collections.Generic.Dictionary(Of Integer, Integer?)()
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("AccountID"))
                Dim pId As Integer? = Nothing
                If row("ParentAccountID") IsNot DBNull.Value AndAlso row("ParentAccountID") IsNot Nothing Then
                    pId = Convert.ToInt32(row("ParentAccountID"))
                End If
                parentDict(id) = pId
            Next

            ' محاسبه سطح هر حساب در حافظه
            Dim accountLevels As New System.Collections.Generic.Dictionary(Of Integer, Integer)()
            For Each id In parentDict.Keys
                Dim level = 1
                Dim currParent = parentDict(id)
                Dim guard = 0
                Do While currParent.HasValue AndAlso guard < 100
                    guard += 1
                    level += 1
                    If parentDict.ContainsKey(currParent.Value) Then
                        currParent = parentDict(currParent.Value)
                    Else
                        Exit Do
                    End If
                Loop
                accountLevels(id) = level
            Next

            ' ۱. بررسی کاهش تعداد سطوح حساب
            For Each kvp In accountLevels
                Dim id = kvp.Key
                Dim lvl = kvp.Value
                If lvl > proposedLevels Then
                    Return String.Format("امکان کاهش تعداد سطوح حساب به {0} وجود ندارد؛ زیرا در حال حاضر سرفصل‌هایی در سطح {1} تعریف شده‌اند که با این کار غیرفعال یا نامعتبر می‌شوند.", proposedLevels, lvl)
                End If
            Next

            ' ۲. بررسی کاهش طول کدهای سطوح مختلف
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("AccountID"))
                Dim code = Convert.ToString(row("AccountCode")).Trim()
                Dim lvl = accountLevels(id)
                
                If lvl <= proposedLengths.Length Then
                    Dim proposedLen = proposedLengths(lvl - 1)
                    If code.Length > proposedLen Then
                        Return String.Format("امکان کاهش طول کد سطح {0} به {1} کاراکتر وجود ندارد؛ زیرا سرفصلی با کد '{2}' (به طول {3} کاراکتر) در این سطح تعریف شده است.", lvl, proposedLen, code, code.Length)
                    End If
                End If
            Next

            Return Nothing
        End Function
    End Class
End Namespace
