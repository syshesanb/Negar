Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class ProjectService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. ProjectContracts
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProjectContracts (" &
                    "ProjectID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectCode TEXT, " &
                    "ProjectTitle TEXT, " &
                    "EmployerName TEXT, " &
                    "ContractorName TEXT DEFAULT 'شرکت نگار', " &
                    "ContractAmount REAL DEFAULT 0, " &
                    "AdvancePercent REAL DEFAULT 10, " &
                    "RetentionPercent REAL DEFAULT 10, " &
                    "InsurancePercent REAL DEFAULT 5, " &
                    "TaxPercent REAL DEFAULT 3, " &
                    "VatPercent REAL DEFAULT 10, " &
                    "StartDate TEXT, " &
                    "EndDate TEXT, " &
                    "Status TEXT DEFAULT 'در حال اجرا', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim projCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProjectContracts"), 0))
                If projCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProjectContracts (CompanyID, ProjectCode, ProjectTitle, EmployerName, ContractAmount, StartDate, EndDate, Status, Notes) " &
                        "VALUES (1, 'PRJ-101', 'پروژه احداث مجتمع تجاری - اداری مروارید', 'سازمان توسعه عمران شهری', 15000000000, ?, ?, 'در حال اجرا', 'پیمانکاری ساخت و تجهیز مجتمع')",
                        dateStr, dateStr
                    )
                End If

                ' 2. ProjectWBS
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProjectWBS (" &
                    "WbsID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectID INTEGER, " &
                    "TaskCode TEXT, " &
                    "TaskName TEXT, " &
                    "PlannedWeight REAL DEFAULT 0, " &
                    "ProgressPercent REAL DEFAULT 0, " &
                    "EstimatedCost REAL DEFAULT 0, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim wbsCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProjectWBS"), 0))
                If wbsCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProjectWBS (CompanyID, ProjectID, TaskCode, TaskName, PlannedWeight, ProgressPercent, EstimatedCost, Notes) " &
                        "VALUES (1, 1, 'WBS-1.1', 'گودبرداری و اجرای فونداسیون', 20.0, 100.0, 3000000000, 'فاز اول اسکلت و فونداسیون')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProjectWBS (CompanyID, ProjectID, TaskCode, TaskName, PlannedWeight, ProgressPercent, EstimatedCost, Notes) " &
                        "VALUES (1, 1, 'WBS-1.2', 'اجرای اسکلت فلزی و طبقات', 45.0, 60.0, 6750000000, 'فاز دوم اسکلت‌بندی')"
                    )
                End If

                ' 3. ProjectClaims
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProjectClaims (" &
                    "ClaimID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectID INTEGER, " &
                    "ClaimNo TEXT, " &
                    "GrossAmount REAL DEFAULT 0, " &
                    "AdvanceDeduction REAL DEFAULT 0, " &
                    "RetentionDeduction REAL DEFAULT 0, " &
                    "InsuranceDeduction REAL DEFAULT 0, " &
                    "TaxDeduction REAL DEFAULT 0, " &
                    "VatAmount REAL DEFAULT 0, " &
                    "NetAmount REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال بررسی', " &
                    "ClaimDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim claimCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProjectClaims"), 0))
                If claimCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProjectClaims (CompanyID, ProjectID, ClaimNo, GrossAmount, AdvanceDeduction, RetentionDeduction, InsuranceDeduction, TaxDeduction, VatAmount, NetAmount, Status, ClaimDate, Notes) " &
                        "VALUES (1, 1, 'CLM-01', 3000000000, 300000000, 300000000, 150000000, 90000000, 300000000, 2360000000, 'در حال بررسی', ?, 'صورت‌وضعیت موقت شماره ۱')",
                        dateStr
                    )
                End If

                ' 4. ProjectGuarantees
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProjectGuarantees (" &
                    "GuaranteeID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectID INTEGER, " &
                    "GuaranteeNo TEXT, " &
                    "BankName TEXT, " &
                    "GuaranteeType TEXT DEFAULT 'پیش‌پرداخت', " &
                    "Amount REAL DEFAULT 0, " &
                    "DueDate TEXT, " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim guarCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProjectGuarantees"), 0))
                If guarCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now.AddMonths(6))
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProjectGuarantees (CompanyID, ProjectID, GuaranteeNo, BankName, GuaranteeType, Amount, DueDate, Status, Notes) " &
                        "VALUES (1, 1, 'GNT-7701', 'بانک ملی شعبه مرکزی', 'پیش‌پرداخت', 1500000000, ?, 'فعال', 'ضمانت‌نامه پیش‌پرداخت اولیه')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetProjects(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, ProjectID, ProjectCode, ProjectTitle, EmployerName, ContractAmount, AdvancePercent, RetentionPercent, InsurancePercent, Status, StartDate, EndDate, Notes FROM ProjectContracts WHERE CompanyID = ? ORDER BY ProjectID DESC", companyID)
        End Function

        Public Function GetProjectWBS(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, W.WbsID, P.ProjectTitle, W.TaskCode, W.TaskName, W.PlannedWeight, W.ProgressPercent, W.EstimatedCost, W.Notes " &
                        "FROM ProjectWBS W INNER JOIN ProjectContracts P ON W.ProjectID = P.ProjectID " &
                        "WHERE W.CompanyID = ? ORDER BY W.WbsID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetProjectClaims(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, C.ClaimID, P.ProjectTitle, C.ClaimNo, C.GrossAmount, " &
                        "C.AdvanceDeduction, C.RetentionDeduction, C.InsuranceDeduction, C.TaxDeduction, C.VatAmount, C.NetAmount, " &
                        "C.Status, C.ClaimDate, C.Notes " &
                        "FROM ProjectClaims C INNER JOIN ProjectContracts P ON C.ProjectID = P.ProjectID " &
                        "WHERE C.CompanyID = ? ORDER BY C.ClaimID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetProjectGuarantees(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, G.GuaranteeID, P.ProjectTitle, G.GuaranteeNo, G.BankName, G.GuaranteeType, G.Amount, G.DueDate, G.Status, G.Notes " &
                        "FROM ProjectGuarantees G INNER JOIN ProjectContracts P ON G.ProjectID = P.ProjectID " &
                        "WHERE G.CompanyID = ? ORDER BY G.GuaranteeID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetProjectById(id As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM ProjectContracts WHERE ProjectID = ?", id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveProject(id As Integer, companyID As Integer, code As String, title As String, employer As String, amount As Double, advance As Double, retention As Double, insurance As Double, notes As String)
            Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO ProjectContracts (CompanyID, ProjectCode, ProjectTitle, EmployerName, ContractAmount, AdvancePercent, RetentionPercent, InsurancePercent, StartDate, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, 'در حال اجرا', ?)",
                    companyID, code, title, employer, amount, advance, retention, insurance, dateStr, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE ProjectContracts SET ProjectCode = ?, ProjectTitle = ?, EmployerName = ?, ContractAmount = ?, AdvancePercent = ?, RetentionPercent = ?, InsurancePercent = ?, Notes = ? " &
                    "WHERE ProjectID = ? AND CompanyID = ?",
                    code, title, employer, amount, advance, retention, insurance, notes, id, companyID
                )
            End If
        End Sub

        Public Sub SaveClaim(id As Integer, companyID As Integer, projectID As Integer, claimNo As String, grossAmount As Double, notes As String)
            Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
            Dim adv = grossAmount * 0.1
            Dim ret = grossAmount * 0.1
            Dim ins = grossAmount * 0.05
            Dim tax = grossAmount * 0.03
            Dim vat = grossAmount * 0.1
            Dim net = grossAmount - adv - ret - ins - tax + vat

            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO ProjectClaims (CompanyID, ProjectID, ClaimNo, GrossAmount, AdvanceDeduction, RetentionDeduction, InsuranceDeduction, TaxDeduction, VatAmount, NetAmount, Status, ClaimDate, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 'در حال بررسی', ?, ?)",
                    companyID, projectID, claimNo, grossAmount, adv, ret, ins, tax, vat, net, dateStr, notes
                )
            End If
        End Sub

        Public Function ConfirmClaim(claimID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT C.*, P.ProjectTitle FROM ProjectClaims C INNER JOIN ProjectContracts P ON C.ProjectID = P.ProjectID WHERE C.ClaimID = ? AND C.CompanyID = ?", claimID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim gross = Convert.ToDouble(If(IsDBNull(row("GrossAmount")), 0, row("GrossAmount")))
                Dim net = Convert.ToDouble(If(IsDBNull(row("NetAmount")), 0, row("NetAmount")))
                Dim claimNo = Convert.ToString(row("ClaimNo"))
                Dim projTitle = Convert.ToString(row("ProjectTitle"))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE ProjectClaims SET Status = 'تایید قطعی' WHERE ClaimID = ?", claimID)

                ' Issue Background Double-Entry Contract Accounting Voucher in Sanad1 & Sanad2
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری صورت‌وضعیت تایید شده " & claimNo & " پروژه " & projTitle & " (ناخالص: " & gross.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم پروژه‌ها', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, gross, gross
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: حساب‌های دریافتنی کارفرما (کد کل 11)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '11', '01', ?, ?, 0)", entryID, "مانده قابل دریافت صورت‌وضعیت - " & projTitle, net)

                ' Bestankar: درآمد پیمانکاری (کد کل 41)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '41', '01', ?, 0, ?)", entryID, "درآمد کارکرد پیمانکاری - " & projTitle, net)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetProjectPLReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, P.ProjectCode, P.ProjectTitle, P.EmployerName, P.ContractAmount, " &
                        "COALESCE(SUM(C.GrossAmount), 0) AS TotalBilled, " &
                        "COALESCE(SUM(C.NetAmount), 0) AS TotalNetCollected, " &
                        "(P.ContractAmount - COALESCE(SUM(C.GrossAmount), 0)) AS RemainingContract " &
                        "FROM ProjectContracts P LEFT JOIN ProjectClaims C ON P.ProjectID = C.ProjectID AND C.Status = 'تایید قطعی' " &
                        "WHERE P.CompanyID = ? GROUP BY P.ProjectID ORDER BY P.ProjectID"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
