Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class SrmService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. SrmSuppliers
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SrmSuppliers (" &
                    "SupplierID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "SupplierCode TEXT, " &
                    "SupplierName TEXT, " &
                    "Category TEXT DEFAULT 'مواد اولیه', " &
                    "Grade TEXT DEFAULT 'گرید A', " &
                    "EconomicCode TEXT, " &
                    "Phone TEXT, " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim suppCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SrmSuppliers"), 0))
                If suppCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SrmSuppliers (CompanyID, SupplierCode, SupplierName, Category, Grade, EconomicCode, Phone, Status, Notes) " &
                        "VALUES (1, 'SUP-101', 'شرکت فولاد مبارکه اصفهان', 'مواد اولیه فلزی', 'گرید A', '411235678912', '031-33334444', 'فعال', 'تامین‌کننده اصلی ورق‌های فولادی')"
                    )
                End If

                ' 2. SrmRfqs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SrmRfqs (" &
                    "RfqID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "RfqNumber TEXT, " &
                    "ItemName TEXT, " &
                    "Quantity REAL DEFAULT 0, " &
                    "WinnerSupplierName TEXT, " &
                    "WinnerPrice REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'تکمیل شده', " &
                    "CreationDate TEXT, " &
                    "ClosingDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim rfqCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SrmRfqs"), 0))
                If rfqCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SrmRfqs (CompanyID, RfqNumber, ItemName, Quantity, WinnerSupplierName, WinnerPrice, Status, CreationDate, ClosingDate, Notes) " &
                        "VALUES (1, 'RFQ-501', 'ورق روغنی ۲ میلی‌متر', 25000, 'شرکت فولاد مبارکه اصفهان', 42500000000, 'تکمیل شده', ?, ?, 'استعلام قیمت مناقصه خرید مواد اولیه سالن ۱')",
                        dateStr, dateStr
                    )
                End If

                ' 3. SrmEvaluations
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SrmEvaluations (" &
                    "EvalID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "SupplierID INTEGER, " &
                    "EvaluationPeriod TEXT DEFAULT 'سه ماهه اول ۱۴۰۵', " &
                    "QualityScore REAL DEFAULT 95, " &
                    "DeliveryScore REAL DEFAULT 90, " &
                    "PriceScore REAL DEFAULT 88, " &
                    "FinalScore REAL DEFAULT 91, " &
                    "AssignedGrade TEXT DEFAULT 'گرید A', " &
                    "EvaluatorName TEXT DEFAULT 'مدیر بازرگانی و خرید', " &
                    "Status TEXT DEFAULT 'ثبت اولیه', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim evalCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SrmEvaluations"), 0))
                If evalCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SrmEvaluations (CompanyID, SupplierID, EvaluationPeriod, QualityScore, DeliveryScore, PriceScore, FinalScore, AssignedGrade, EvaluatorName, Status, Notes) " &
                        "VALUES (1, 1, 'سه ماهه اول ۱۴۰۵', 96, 92, 90, 92.6, 'گرید A', 'مهندس طاهری', 'ثبت اولیه', 'ارزیابی دوره سه ماهه تحویل بدون ضایعات')"
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetSuppliers(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, SupplierID, SupplierCode, SupplierName, Category, Grade, EconomicCode, Phone, Status, Notes FROM SrmSuppliers WHERE CompanyID = ? ORDER BY SupplierID DESC", companyID)
        End Function

        Public Function GetRfqs(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, RfqID, RfqNumber, ItemName, Quantity, WinnerSupplierName, WinnerPrice, Status, CreationDate, ClosingDate, Notes FROM SrmRfqs WHERE CompanyID = ? ORDER BY RfqID DESC", companyID)
        End Function

        Public Function GetEvaluations(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, E.EvalID, S.SupplierCode, S.SupplierName, E.EvaluationPeriod, " &
                        "E.QualityScore, E.DeliveryScore, E.PriceScore, E.FinalScore, E.AssignedGrade, E.EvaluatorName, E.Status, E.Notes " &
                        "FROM SrmEvaluations E INNER JOIN SrmSuppliers S ON E.SupplierID = S.SupplierID " &
                        "WHERE E.CompanyID = ? ORDER BY E.EvalID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Sub SaveSupplier(id As Integer, companyID As Integer, code As String, name As String, category As String, grade As String, econCode As String, phone As String, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO SrmSuppliers (CompanyID, SupplierCode, SupplierName, Category, Grade, EconomicCode, Phone, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, 'فعال', ?)",
                    companyID, code, name, category, grade, econCode, phone, notes
                )
            End If
        End Sub

        Public Function ApproveEvaluationAndIssueSanad(evalID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT E.*, S.SupplierName FROM SrmEvaluations E INNER JOIN SrmSuppliers S ON E.SupplierID = S.SupplierID WHERE E.EvalID = ? AND E.CompanyID = ?", evalID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim suppName = Convert.ToString(row("SupplierName"))
                Dim finalScore = Convert.ToDouble(If(IsDBNull(row("FinalScore")), 90, row("FinalScore")))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                ' Update status to APPROVED
                Sql.ExecuteNonQuery("UPDATE SrmEvaluations SET Status = 'تایید نهایی', AssignedGrade = 'گرید A' WHERE EvalID = ?", evalID)
                Sql.ExecuteNonQuery("UPDATE SrmSuppliers SET Grade = 'گرید A' WHERE SupplierID = ?", row("SupplierID"))

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2 for Quality Incentive Bonus (25,000,000 Rls)
                Dim bonusAmount As Double = 25000000
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری پاداش کیفی و تخفیف SRM تامین‌کننده " & suppName & " (امتیاز کیفی: " & finalScore.ToString("N1") & ")"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم مدیریت تامین‌کنندگان (SRM)', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, bonusAmount, bonusAmount
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: تخفیفات و پاداش کیفی خرید (کد کل 51 - تخفیفات خرید)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '51', '02', ?, ?, 0)", entryID, "پاداش و اعتبار کیفی تامین‌کننده - " & suppName, bonusAmount)

                ' Bestankar: بستانکاران تجاری - تامین‌کنندگان (کد کل 21)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '21', '01', ?, 0, ?)", entryID, "بستانکاری اعتبار کیفی تامین‌کننده - " & suppName, bonusAmount)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetSrmReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, S.SupplierCode, S.SupplierName, S.Category, S.Grade, " &
                        "COUNT(E.EvalID) AS TotalEvaluations, " &
                        "COALESCE(AVG(E.QualityScore), 90) AS AvgQualityScore, " &
                        "COALESCE(AVG(E.DeliveryScore), 90) AS AvgDeliveryScore, " &
                        "COALESCE(AVG(E.FinalScore), 90) AS OverallScore " &
                        "FROM SrmSuppliers S LEFT JOIN SrmEvaluations E ON S.SupplierID = E.SupplierID " &
                        "WHERE S.CompanyID = ? GROUP BY S.SupplierID ORDER BY S.SupplierID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
