Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class PersonService

        Public Function GetAll(Optional filterRole As String = "همه", Optional filterType As String = "همه", Optional searchKeyword As String = "") As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Dim companyId = SessionContext.CurrentCompanyID.Value

            Dim query As String = "SELECT PersonID, PersonType, RoleType, PersonCode, " &
                                 "CASE WHEN PersonType = 'حقوقی' THEN CompanyName ELSE (COALESCE(FirstName, '') || ' ' || COALESCE(LastName, '')) END AS DisplayName, " &
                                 "FirstName, LastName, CompanyName, NationalCode, EconomicCode, RegistrationNumber, " &
                                 "Phone, Mobile, Address, PostalCode, ShenavarID, IsActive " &
                                 "FROM Persons WHERE CompanyID = ? "

            Dim args As New List(Of Object)()
            args.Add(companyId)

            If Not String.IsNullOrEmpty(filterRole) AndAlso filterRole <> "همه" Then
                query &= "AND (RoleType = ? OR RoleType = 'هر دو') "
                args.Add(filterRole)
            End If

            If Not String.IsNullOrEmpty(filterType) AndAlso filterType <> "همه" Then
                query &= "AND PersonType = ? "
                args.Add(filterType)
            End If

            If Not String.IsNullOrWhiteSpace(searchKeyword) Then
                Dim kw = "%" & searchKeyword.Trim() & "%"
                query &= "AND (PersonCode LIKE ? OR FirstName LIKE ? OR LastName LIKE ? OR CompanyName LIKE ? OR NationalCode LIKE ? OR Mobile LIKE ?) "
                args.Add(kw)
                args.Add(kw)
                args.Add(kw)
                args.Add(kw)
                args.Add(kw)
                args.Add(kw)
            End If

            query &= "ORDER BY PersonCode"

            Return Sql.ExecuteTable(query, args.ToArray())
        End Function

        Public Function GetNextCode() As String
            If Not SessionContext.CurrentCompanyID.HasValue Then Return "1001"
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim maxVal = Sql.ExecuteScalar("SELECT MAX(CAST(PersonCode AS INTEGER)) FROM Persons WHERE CompanyID = ? AND PersonCode GLOB '[0-9]*'", companyId)
            If maxVal Is Nothing OrElse maxVal Is DBNull.Value Then
                Return "1001"
            End If
            Dim maxInt As Integer
            If Integer.TryParse(Convert.ToString(maxVal), maxInt) Then
                Return (maxInt + 1).ToString()
            End If
            Return "1001"
        End Function

        Public Function Save(personId As Integer?, personType As String, roleType As String, personCode As String,
                             firstName As String, lastName As String, companyName As String, nationalCode As String,
                             economicCode As String, registrationNumber As String, phone As String, mobile As String,
                             address As String, postalCode As String, isActive As Boolean) As Integer

            If Not SessionContext.CurrentCompanyID.HasValue Then
                Throw New InvalidOperationException("ابتدا باید شرکت جاری را انتخاب کنید.")
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, CType(Nothing, Integer?))

            ' بررسی تکراری نبودن کد شخص
            Dim excludeId = If(personId.HasValue AndAlso personId.Value > 0, personId.Value, 0)
            Dim dup = Sql.ExecuteScalar("SELECT COUNT(*) FROM Persons WHERE CompanyID = ? AND PersonCode = ? AND PersonID <> ?",
                                       companyId, personCode.Trim(), excludeId)
            If Convert.ToInt32(dup) > 0 Then
                Throw New InvalidOperationException("کد «" & personCode & "» قبلاً به شخص دیگری اختصاص یافته است.")
            End If

            ' نام جهت درج در حساب شناور
            Dim displayName As String = If(personType = "حقوقی", companyName.Trim(), (firstName.Trim() & " " & lastName.Trim()).Trim())

            Dim shenavarId As Integer? = Nothing

            If personId.HasValue AndAlso personId.Value > 0 Then
                ' ویرایش شخص موجود
                Dim oldShenavarId = Sql.ExecuteScalar("SELECT ShenavarID FROM Persons WHERE PersonID = ?", personId.Value)
                If oldShenavarId IsNot Nothing AndAlso oldShenavarId IsNot DBNull.Value Then
                    shenavarId = Convert.ToInt32(oldShenavarId)
                    ' به‌روزرسانی حساب شناور متناظر
                    Sql.ExecuteNonQuery("UPDATE SarfaslShenavar SET AccountCode = ?, AccountName = ?, IsActive = ? WHERE ShenavarID = ?",
                                       personCode.Trim(), displayName, isActive, shenavarId.Value)
                Else
                    ' ایجاد حساب شناور جدید در صورت عدم وجود
                    shenavarId = CreateShenavarAccount(companyId, personCode.Trim(), displayName, isActive, currentUserId)
                End If

                Sql.ExecuteNonQuery(
                    "UPDATE Persons SET PersonType=?, RoleType=?, PersonCode=?, FirstName=?, LastName=?, CompanyName=?, " &
                    "NationalCode=?, EconomicCode=?, RegistrationNumber=?, Phone=?, Mobile=?, Address=?, PostalCode=?, ShenavarID=?, IsActive=? " &
                    "WHERE PersonID=?",
                    personType, roleType, personCode.Trim(), firstName.Trim(), lastName.Trim(), companyName.Trim(),
                    nationalCode.Trim(), economicCode.Trim(), registrationNumber.Trim(), phone.Trim(), mobile.Trim(),
                    address.Trim(), postalCode.Trim(), shenavarId, isActive, personId.Value)

                ' پیوند معکوس در SarfaslShenavar
                If shenavarId.HasValue Then
                    Sql.ExecuteNonQuery("UPDATE SarfaslShenavar SET PersonID = ? WHERE ShenavarID = ?", personId.Value, shenavarId.Value)
                End If

                Return personId.Value
            Else
                ' ثبت شخص جدید + حساب شناور خودکار
                shenavarId = CreateShenavarAccount(companyId, personCode.Trim(), displayName, isActive, currentUserId)

                Dim newPersonId = Convert.ToInt32(Sql.ExecuteIdentity(
                    "INSERT INTO Persons (CompanyID, PersonType, RoleType, PersonCode, FirstName, LastName, CompanyName, " &
                    "NationalCode, EconomicCode, RegistrationNumber, Phone, Mobile, Address, PostalCode, ShenavarID, IsActive, CreatedBy) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    companyId, personType, roleType, personCode.Trim(), firstName.Trim(), lastName.Trim(), companyName.Trim(),
                    nationalCode.Trim(), economicCode.Trim(), registrationNumber.Trim(), phone.Trim(), mobile.Trim(),
                    address.Trim(), postalCode.Trim(), shenavarId, isActive, currentUserId))

                ' پیوند معکوس در SarfaslShenavar
                If shenavarId.HasValue Then
                    Sql.ExecuteNonQuery("UPDATE SarfaslShenavar SET PersonID = ? WHERE ShenavarID = ?", newPersonId, shenavarId.Value)
                End If

                Return newPersonId
            End If
        End Function

        Private Function CreateShenavarAccount(companyId As Integer, code As String, name As String, isActive As Boolean, userId As Integer?) As Integer
            Return Convert.ToInt32(Sql.ExecuteIdentity(
                "INSERT INTO SarfaslShenavar (CompanyID, AccountCode, AccountName, ParentShenavarID, IsActive, CreatedBy) " &
                "VALUES (?, ?, ?, NULL, ?, ?)",
                companyId, code, name, isActive, userId))
        End Function

        Public Sub Delete(personId As Integer)
            Dim companyId = SessionContext.CurrentCompanyID.Value

            ' بررسی وابستگی در فاکتورهای خرید یا فروش
            Dim checkPurchases = Sql.ExecuteScalar("SELECT COUNT(*) FROM PurchaseInvoices WHERE VendorID = ?", personId)
            If Convert.ToInt32(checkPurchases) > 0 Then
                Throw New InvalidOperationException("این شخص دارای فاکتور خرید ثبت‌شده می‌باشد و امکان حذف آن وجود ندارد.")
            End If

            Dim checkSales = Sql.ExecuteScalar("SELECT COUNT(*) FROM SalesInvoices WHERE CustomerID = ?", personId)
            If Convert.ToInt32(checkSales) > 0 Then
                Throw New InvalidOperationException("این شخص دارای فاکتور فروش ثبت‌شده می‌باشد و امکان حذف آن وجود ندارد.")
            End If

            ' حذف حساب شناور متناظر
            Dim shenavarId = Sql.ExecuteScalar("SELECT ShenavarID FROM Persons WHERE PersonID = ?", personId)
            If shenavarId IsNot Nothing AndAlso shenavarId IsNot DBNull.Value Then
                Dim sId = Convert.ToInt32(shenavarId)
                ' بررسی وابستگی سند حسابداری شناور
                Dim sanadCount = Sql.ExecuteScalar("SELECT COUNT(*) FROM Sanad2 WHERE ShenavarID = ?", sId)
                If Convert.ToInt32(sanadCount) > 0 Then
                    Throw New InvalidOperationException("برای این شخص اسناد حسابداری شناور صادر شده است و امکان حذف وجود ندارد.")
                End If
                Sql.ExecuteNonQuery("DELETE FROM SarfaslShenavar WHERE ShenavarID = ?", sId)
            End If

            Sql.ExecuteNonQuery("DELETE FROM Persons WHERE PersonID = ?", personId)
        End Sub

    End Class
End Namespace
