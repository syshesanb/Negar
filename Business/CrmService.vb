Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class CrmService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. CrmCustomers
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CrmCustomers (" &
                    "CrmCustomerID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CustomerCode TEXT, " &
                    "FullName TEXT, " &
                    "Phone TEXT, " &
                    "Mobile TEXT, " &
                    "Email TEXT, " &
                    "Address TEXT, " &
                    "Category TEXT DEFAULT 'مشتری حقوقی', " &
                    "LeadSource TEXT DEFAULT 'وب‌سایت', " &
                    "Status TEXT DEFAULT 'مشتری احتمالی', " & ' 'سرنخ اولیه', 'مشتری احتمالی', 'مشتری قطعی', 'ناموفق'
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' Seed sample data if empty
                Dim custCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM CrmCustomers"), 0))
                If custCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO CrmCustomers (CompanyID, CustomerCode, FullName, Phone, Mobile, Email, Category, LeadSource, Status, Notes) " &
                        "VALUES (1, 'CRM-1001', 'شرکت مهندسی آرمان پارس', '02188889999', '09121112233', 'info@armanpars.com', 'حقوقی', 'نمایشگاه', 'مشتری احتمالی', 'متقاضی خرید نرم‌افزار و تجهیزات')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO CrmCustomers (CompanyID, CustomerCode, FullName, Phone, Mobile, Email, Category, LeadSource, Status, Notes) " &
                        "VALUES (1, 'CRM-1002', 'بازرگانی کیهان تجارت', '02166667777', '09123334455', 'sales@keyhan.ir', 'حقوقی', 'وب‌سایت', 'مشتری قطعی', 'خریدار قطعی سیستم‌های مالی')"
                    )
                End If

                ' 2. CrmOpportunities
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CrmOpportunities (" &
                    "OpportunityID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CrmCustomerID INTEGER, " &
                    "Title TEXT, " &
                    "EstimatedValue REAL DEFAULT 0, " &
                    "WinProbability INTEGER DEFAULT 50, " &
                    "Stage TEXT DEFAULT 'شناسایی نیاز', " & ' 'ارتباط اولیه', 'شناسایی نیاز', 'پیش‌فاکتور', 'مذاکره', 'برنده شده', 'باخته شده'
                    "ExpectedCloseDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim oppCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM CrmOpportunities"), 0))
                If oppCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO CrmOpportunities (CompanyID, CrmCustomerID, Title, EstimatedValue, WinProbability, Stage, ExpectedCloseDate, Notes) " &
                        "VALUES (1, 1, 'پروژه تجهیز سیستم‌های اداری آرمان پارس', 150000000, 75, 'پیش‌فاکتور', ?, 'پیش‌فاکتور صادر شده و منتظر تایید است')",
                        dateStr
                    )
                End If

                ' 3. CrmActivities
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CrmActivities (" &
                    "ActivityID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CrmCustomerID INTEGER, " &
                    "OpportunityID INTEGER DEFAULT 0, " &
                    "ActivityType TEXT DEFAULT 'تماس', " & ' 'تماس', 'جلسه', 'یادداشت', 'وظیفه'
                    "ActivityDate TEXT, " &
                    "Subject TEXT, " &
                    "Details TEXT, " &
                    "Status TEXT DEFAULT 'انجام شده', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' 4. CrmTickets
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CrmTickets (" &
                    "TicketID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CrmCustomerID INTEGER, " &
                    "TicketNo TEXT, " &
                    "Subject TEXT, " &
                    "Priority TEXT DEFAULT 'عادی', " &
                    "Status TEXT DEFAULT 'جدید', " &
                    "ContentBody TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetCustomers(companyID As Integer) As DataTable
            Dim query = "SELECT CrmCustomerID, CustomerCode, FullName, Phone, Mobile, Email, Category, LeadSource, Status, Notes " &
                        "FROM CrmCustomers WHERE CompanyID = ? ORDER BY CrmCustomerID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetCustomerById(customerID As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM CrmCustomers WHERE CrmCustomerID = ?", customerID)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveCustomer(customerID As Integer, companyID As Integer, code As String, name As String, phone As String, mobile As String, email As String, category As String, source As String, status As String, notes As String)
            If customerID <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO CrmCustomers (CompanyID, CustomerCode, FullName, Phone, Mobile, Email, Category, LeadSource, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, code, name, phone, mobile, email, category, source, status, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE CrmCustomers SET CustomerCode = ?, FullName = ?, Phone = ?, Mobile = ?, Email = ?, Category = ?, LeadSource = ?, Status = ?, Notes = ? " &
                    "WHERE CrmCustomerID = ? AND CompanyID = ?",
                    code, name, phone, mobile, email, category, source, status, notes, customerID, companyID
                )
            End If
        End Sub

        Public Sub DeleteCustomer(customerID As Integer, companyID As Integer)
            Sql.ExecuteNonQuery("DELETE FROM CrmCustomers WHERE CrmCustomerID = ? AND CompanyID = ?", customerID, companyID)
            Sql.ExecuteNonQuery("DELETE FROM CrmOpportunities WHERE CrmCustomerID = ? AND CompanyID = ?", customerID, companyID)
        End Sub

        Public Function GetOpportunities(companyID As Integer) As DataTable
            Dim query = "SELECT o.OpportunityID, c.FullName AS CustomerName, o.Title, o.EstimatedValue, " &
                        "o.WinProbability || '%' AS WinProbabilityTitle, o.Stage, o.ExpectedCloseDate, o.Notes " &
                        "FROM CrmOpportunities o " &
                        "LEFT JOIN CrmCustomers c ON o.CrmCustomerID = c.CrmCustomerID " &
                        "WHERE o.CompanyID = ? ORDER BY o.OpportunityID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function ConvertOpportunityToInvoice(opportunityID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                ' 1. Get Opportunity Details
                Dim dtOpp = Sql.ExecuteTable("SELECT o.*, c.FullName FROM CrmOpportunities o LEFT JOIN CrmCustomers c ON o.CrmCustomerID = c.CrmCustomerID WHERE o.OpportunityID = ?", opportunityID)
                If dtOpp Is Nothing OrElse dtOpp.Rows.Count = 0 Then Return False

                Dim row = dtOpp.Rows(0)
                Dim amount = Convert.ToDouble(If(IsDBNull(row("EstimatedValue")), 0, row("EstimatedValue")))
                Dim custName = Convert.ToString(row("FullName"))
                Dim title = Convert.ToString(row("Title"))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                ' Update Stage in CRM to 'برنده شده'
                Sql.ExecuteNonQuery("UPDATE CrmOpportunities SET Stage = 'برنده شده' WHERE OpportunityID = ?", opportunityID)
                Sql.ExecuteNonQuery("UPDATE CrmCustomers SET Status = 'مشتری قطعی' WHERE CrmCustomerID = ?", Convert.ToInt32(row("CrmCustomerID")))

                ' 2. Issue Background Accounting Voucher directly in Sanad1 & Sanad2
                ' Even if current user has NO permission for Accounting, background business logic performs it cleanly!
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند فروش اتوماتیک حاصل از معامله CRM: " & title & " - " & custName

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم CRM', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, amount, amount
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: بدهکاران تجاری (کد کل 10)
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) " &
                    "VALUES (?, '10', '01', ?, ?, 0)",
                    entryID, "بدهکاران تجاری - " & custName, amount
                )

                ' Bestankar: فروش محصولات/خدمات (کد کل 40)
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) " &
                    "VALUES (?, '40', '01', ?, 0, ?)",
                    entryID, "فروش حاصل از CRM - " & title, amount
                )

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetActivities(companyID As Integer) As DataTable
            Dim query = "SELECT a.ActivityID, c.FullName AS CustomerName, a.ActivityType, a.ActivityDate, a.Subject, a.Details, a.Status " &
                        "FROM CrmActivities a " &
                        "LEFT JOIN CrmCustomers c ON a.CrmCustomerID = c.CrmCustomerID " &
                        "WHERE a.CompanyID = ? ORDER BY a.ActivityID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetTickets(companyID As Integer) As DataTable
            Dim query = "SELECT t.TicketID, t.TicketNo, c.FullName AS CustomerName, t.Subject, t.Priority, t.Status, t.ContentBody " &
                        "FROM CrmTickets t " &
                        "LEFT JOIN CrmCustomers c ON t.CrmCustomerID = c.CrmCustomerID " &
                        "WHERE t.CompanyID = ? ORDER BY t.TicketID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetCrmReports(companyID As Integer) As DataTable
            Dim query = "SELECT c.CustomerCode, c.FullName, c.LeadSource, c.Status AS CustomerStatus, " &
                        "COUNT(o.OpportunityID) AS TotalOpportunities, " &
                        "COALESCE(SUM(o.EstimatedValue), 0) AS TotalPipelineValue " &
                        "FROM CrmCustomers c " &
                        "LEFT JOIN CrmOpportunities o ON c.CrmCustomerID = o.CrmCustomerID " &
                        "WHERE c.CompanyID = ? GROUP BY c.CrmCustomerID ORDER BY c.CrmCustomerID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
