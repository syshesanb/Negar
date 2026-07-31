Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class QcService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. QcInspections
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS QcInspections (" &
                    "InspectionID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "InspectionType TEXT DEFAULT 'حین تولید IPQC', " &
                    "BatchNumber TEXT, " &
                    "ItemName TEXT, " &
                    "SampleQuantity REAL DEFAULT 100, " &
                    "PassedQuantity REAL DEFAULT 95, " &
                    "RejectedQuantity REAL DEFAULT 5, " &
                    "InspectorName TEXT DEFAULT 'مهندس حسینی (QC)', " &
                    "Result TEXT DEFAULT 'تایید شده Pass', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim inspCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM QcInspections"), 0))
                If inspCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO QcInspections (CompanyID, InspectionType, BatchNumber, ItemName, SampleQuantity, PassedQuantity, RejectedQuantity, InspectorName, Result, Notes) " &
                        "VALUES (1, 'حین تولید IPQC', 'BATCH-8801', 'قطعه پرس‌شده بدنه جانبی', 500, 485, 15, 'مهندس حسینی (QC)', 'تایید شده Pass', 'آزمون ابعادی و تست کشش سالن ۱')"
                    )
                End If

                ' 2. QcNcrCapas
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS QcNcrCapas (" &
                    "NcrID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "NcrNumber TEXT, " &
                    "IssueTitle TEXT, " &
                    "Department TEXT DEFAULT 'سالن پرس‌کاری', " &
                    "RootCause TEXT, " &
                    "CorrectiveAction TEXT, " &
                    "Status TEXT DEFAULT 'در حال بررسی', " &
                    "IssueDate TEXT, " &
                    "ClosureDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim ncrCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM QcNcrCapas"), 0))
                If ncrCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO QcNcrCapas (CompanyID, NcrNumber, IssueTitle, Department, RootCause, CorrectiveAction, Status, IssueDate, Notes) " &
                        "VALUES (1, 'NCR-104', 'انحراف ضخامت ورق‌های روغنی ورودی', 'انبار مواد اولیه', 'مغایرت در کیفیت سفارش خریدار با پارت ورودی', 'تنظیم مجدد قالب پرس و درخواست مرجوعی محموله', 'در حال بررسی', ?, 'عدم انطباق کیفی ثبت‌شده توسط واحد کنترل کیفیت')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetInspections(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, InspectionID, InspectionType, BatchNumber, ItemName, SampleQuantity, PassedQuantity, RejectedQuantity, InspectorName, Result, Notes FROM QcInspections WHERE CompanyID = ? ORDER BY InspectionID DESC", companyID)
        End Function

        Public Function GetNcrCapas(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, NcrID, NcrNumber, IssueTitle, Department, RootCause, CorrectiveAction, Status, IssueDate, ClosureDate, Notes FROM QcNcrCapas WHERE CompanyID = ? ORDER BY NcrID DESC", companyID)
        End Function

        Public Sub SaveInspection(id As Integer, companyID As Integer, typeName As String, batch As String, item As String, sampleQty As Double, passQty As Double, rejectQty As Double, inspector As String, notes As String)
            If id <= 0 Then
                Dim resStr = If(rejectQty > 0, "مردود شده Reject", "تایید شده Pass")
                Sql.ExecuteNonQuery(
                    "INSERT INTO QcInspections (CompanyID, InspectionType, BatchNumber, ItemName, SampleQuantity, PassedQuantity, RejectedQuantity, InspectorName, Result, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, typeName, batch, item, sampleQty, passQty, rejectQty, inspector, resStr, notes
                )
            End If
        End Sub

        Public Function ApproveInspectionAndIssueSanad(inspectionID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM QcInspections WHERE InspectionID = ? AND CompanyID = ?", inspectionID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim batchNum = Convert.ToString(row("BatchNumber"))
                Dim itemName = Convert.ToString(row("ItemName"))
                Dim rejectQty = Convert.ToDouble(If(IsDBNull(row("RejectedQuantity")), 0, row("RejectedQuantity")))
                Dim unitScrapVal As Double = 1250000 ' 1.25M Rls per scrap unit
                Dim scrapCost = rejectQty * unitScrapVal
                If scrapCost <= 0 Then scrapCost = 15000000
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE QcInspections SET Result = 'تکمیل و ثبت ضایعات' WHERE InspectionID = ?", inspectionID)

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2 for Quality Scrap / Cost of Quality (COQ)
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری ضایعات کیفی محصول/کالا " & itemName & " محموله " & batchNum & " (مبلغ ضایعات: " & scrapCost.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم کنترل کیفیت (QC/QA)', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, scrapCost, scrapCost
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: هزینه ضایعات غیرعادی کیفی (کد کل 61 - سربار و ضایعات تولید)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '61', '08', ?, ?, 0)", entryID, "هزینه ضایعات عدم انطباق کیفی - " & itemName, scrapCost)

                ' Bestankar: کالای در جریان ساخت / مواد اولیه (کد کل 14)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '14', '01', ?, 0, ?)", entryID, "کسر ضایعات کیفی از خط تولید - " & itemName, scrapCost)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetQcReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, InspectionType, " &
                        "COUNT(InspectionID) AS TotalInspections, " &
                        "COALESCE(SUM(SampleQuantity), 0) AS TotalSampleQty, " &
                        "COALESCE(SUM(PassedQuantity), 0) AS TotalPassedQty, " &
                        "COALESCE(SUM(RejectedQuantity), 0) AS TotalRejectedQty, " &
                        "CASE WHEN SUM(SampleQuantity) = 0 THEN 100.0 ELSE ROUND((SUM(PassedQuantity) * 100.0) / SUM(SampleQuantity), 1) END AS FpyPercentage " &
                        "FROM QcInspections WHERE CompanyID = ? GROUP BY InspectionType ORDER BY InspectionID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
