Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class DmsService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. DmsCategories
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS DmsCategories (" &
                    "CategoryID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CategoryCode TEXT, " &
                    "CategoryTitle TEXT, " &
                    "ParentID INTEGER DEFAULT 0, " &
                    "Notes TEXT);"
                )

                Dim catCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM DmsCategories"), 0))
                If catCount = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO DmsCategories (CategoryCode, CategoryTitle, Notes) VALUES ('CAT-101', 'زون مالی و اسناد حسابداری', 'پرونده‌های فاکتورها، قبض‌ها و ضمائم اسناد')")
                    Sql.ExecuteNonQuery("INSERT INTO DmsCategories (CategoryCode, CategoryTitle, Notes) VALUES ('CAT-102', 'زون قراردادها و تضامین', 'قراردادهای پیمانکاری، خرید و چک‌های ضمانت')")
                    Sql.ExecuteNonQuery("INSERT INTO DmsCategories (CategoryCode, CategoryTitle, Notes) VALUES ('CAT-103', 'زون پرسنلی و منابع انسانی', 'مدارک شناسایی، ضمانت‌نامه‌ها و احکام کارگزینی')")
                    Sql.ExecuteNonQuery("INSERT INTO DmsCategories (CategoryCode, CategoryTitle, Notes) VALUES ('CAT-104', 'زون نقشه و مدارک فنی', 'نقشه‌های ساخت صنعتی و کاتالوگ تجهیزات')")
                End If

                ' 2. DmsDocuments
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS DmsDocuments (" &
                    "DocumentID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "DocumentCode TEXT, " &
                    "DocumentTitle TEXT, " &
                    "CategoryName TEXT DEFAULT 'زون مالی و اسناد حسابداری', " &
                    "FileName TEXT, " &
                    "FileSize TEXT DEFAULT '1.4 MB', " &
                    "FileType TEXT DEFAULT 'PDF', " &
                    "VersionNumber TEXT DEFAULT '1.0', " &
                    "Keywords TEXT, " &
                    "SecurityLevel TEXT DEFAULT 'محرمانه', " &
                    "ExpirationDate TEXT, " &
                    "CreatedBy TEXT DEFAULT 'مدیر بایگانی', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim docCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM DmsDocuments"), 0))
                If docCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now.AddDays(180))
                    Sql.ExecuteNonQuery(
                        "INSERT INTO DmsDocuments (CompanyID, DocumentCode, DocumentTitle, CategoryName, FileName, FileSize, FileType, VersionNumber, Keywords, SecurityLevel, ExpirationDate, CreatedBy) " &
                        "VALUES (1, 'DOC-9901', 'قرارداد خرید مواد اولیه پتروشیمی', 'زون قراردادها و تضامین', 'Contract_Petro_1405.pdf', '2.8 MB', 'PDF', '1.0', 'قرارداد، تامین، خرید، فاکتور', 'محرمانه', ?, 'مهندس امینی')",
                        dateStr
                    )
                End If

                ' 3. DmsAuditLogs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS DmsAuditLogs (" &
                    "LogID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "DocumentCode TEXT, " &
                    "DocumentTitle TEXT, " &
                    "ActionType TEXT DEFAULT 'مشاهده و اسکن', " &
                    "UserName TEXT DEFAULT 'کاربر سیستم', " &
                    "AccessDate TEXT, " &
                    "Notes TEXT);"
                )

                Dim logCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM DmsAuditLogs"), 0))
                If logCount = 0 Then
                    Dim nowStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO DmsAuditLogs (CompanyID, DocumentCode, DocumentTitle, ActionType, UserName, AccessDate, Notes) " &
                        "VALUES (1, 'DOC-9901', 'قرارداد خرید مواد اولیه پتروشیمی', 'مشاهده تصویری', 'کاربر مدیرعامل', ?, 'بازبینی متن قرارداد توسط مدیریت')",
                        nowStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetDocuments(companyID As Integer, Optional categoryFilter As String = "", Optional searchKeyword As String = "") As DataTable
            Dim query = "SELECT '' AS colRowIndex, DocumentID, DocumentCode, DocumentTitle, CategoryName, FileName, FileSize, FileType, VersionNumber, Keywords, SecurityLevel, ExpirationDate, CreatedBy FROM DmsDocuments WHERE CompanyID = ?"
            Dim params As New List(Of Object)()
            params.Add(companyID)

            If Not String.IsNullOrWhiteSpace(categoryFilter) Then
                query &= " AND CategoryName = ?"
                params.Add(categoryFilter)
            End If

            If Not String.IsNullOrWhiteSpace(searchKeyword) Then
                query &= " AND (DocumentTitle LIKE ? OR Keywords LIKE ? OR DocumentCode LIKE ?)"
                params.Add("%" & searchKeyword & "%")
                params.Add("%" & searchKeyword & "%")
                params.Add("%" & searchKeyword & "%")
            End If

            query &= " ORDER BY DocumentID DESC"
            Return Sql.ExecuteTable(query, params.ToArray())
        End Function

        Public Function GetCategories() As DataTable
            Return Sql.ExecuteTable("SELECT CategoryID, CategoryCode, CategoryTitle, Notes FROM DmsCategories ORDER BY CategoryID ASC")
        End Function

        Public Function GetExpiringDocuments(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, DocumentID, DocumentCode, DocumentTitle, CategoryName, ExpirationDate, SecurityLevel, CreatedBy FROM DmsDocuments WHERE CompanyID = ? AND ExpirationDate IS NOT NULL AND ExpirationDate != '' ORDER BY ExpirationDate ASC", companyID)
        End Function

        Public Function GetAuditLogs(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, LogID, DocumentCode, DocumentTitle, ActionType, UserName, AccessDate, Notes FROM DmsAuditLogs WHERE CompanyID = ? ORDER BY LogID DESC", companyID)
        End Function

        Public Sub SaveDocument(id As Integer, companyID As Integer, title As String, catName As String, fileName As String, keywords As String, securityLvl As String, expDate As String, user As String)
            If id <= 0 Then
                Dim docCode = "DOC-" & (Environment.TickCount Mod 10000).ToString()
                Sql.ExecuteNonQuery(
                    "INSERT INTO DmsDocuments (CompanyID, DocumentCode, DocumentTitle, CategoryName, FileName, FileSize, FileType, VersionNumber, Keywords, SecurityLevel, ExpirationDate, CreatedBy) " &
                    "VALUES (?, ?, ?, ?, ?, '1.5 MB', 'PDF', '1.0', ?, ?, ?, ?)",
                    companyID, docCode, title, catName, fileName, keywords, securityLvl, expDate, user
                )

                ' Log audit trail
                Dim nowStr = PersianDateHelper.ToPersian(DateTime.Now)
                Sql.ExecuteNonQuery(
                    "INSERT INTO DmsAuditLogs (CompanyID, DocumentCode, DocumentTitle, ActionType, UserName, AccessDate, Notes) " &
                    "VALUES (?, ?, ?, 'ثبت و اسکن جدید', ?, ?, 'ثبت سند جدید در بایگانی دیجیتال')",
                    companyID, docCode, title, user, nowStr
                )
            End If
        End Sub
    End Class
End Namespace
