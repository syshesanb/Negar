Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class ImportExportService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. ImportProformas
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ImportProformas (" &
                    "ProformaID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PINumber TEXT, " &
                    "SupplierName TEXT, " &
                    "CurrencyCode TEXT DEFAULT 'EUR', " &
                    "CurrencyRate REAL DEFAULT 600000, " &
                    "CurrencyAmount REAL DEFAULT 0, " &
                    "IrrAmount REAL DEFAULT 0, " &
                    "Incoterms TEXT DEFAULT 'FOB', " &
                    "Status TEXT DEFAULT 'ثبت اولیه', " &
                    "PIDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim piCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ImportProformas"), 0))
                If piCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ImportProformas (CompanyID, PINumber, SupplierName, CurrencyCode, CurrencyRate, CurrencyAmount, IrrAmount, Incoterms, Status, PIDate, Notes) " &
                        "VALUES (1, 'PI-8802', 'Siemens Germany GMBH', 'EUR', 650000, 50000, 32500000000, 'FOB', 'ثبت سفارش شده', ?, 'خرید قطعات هیدرولیک خط تولید')",
                        dateStr
                    )
                End If

                ' 2. ImportLCs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ImportLCs (" &
                    "LcID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProformaID INTEGER, " &
                    "LcNumber TEXT, " &
                    "BankName TEXT, " &
                    "CurrencyCode TEXT DEFAULT 'EUR', " &
                    "CurrencyAmount REAL DEFAULT 0, " &
                    "IrrAmount REAL DEFAULT 0, " &
                    "AdvancePayment REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'گشایش شده', " &
                    "IssueDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim lcCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ImportLCs"), 0))
                If lcCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ImportLCs (CompanyID, ProformaID, LcNumber, BankName, CurrencyCode, CurrencyAmount, IrrAmount, AdvancePayment, Status, IssueDate, Notes) " &
                        "VALUES (1, 1, 'LC-99014', 'بانک تجارت شعبه میرداماد', 'EUR', 50000, 32500000000, 3250000000, 'گشایش شده', ?, 'گشایش اعتبار اسنادی دیداری')",
                        dateStr
                    )
                End If

                ' 3. ImportCustoms
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ImportCustoms (" &
                    "CustomsID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProformaID INTEGER, " &
                    "DeclarationNo TEXT, " &
                    "CustomsName TEXT, " &
                    "DutyAmount REAL DEFAULT 0, " &
                    "VatAmount REAL DEFAULT 0, " &
                    "ShippingCost REAL DEFAULT 0, " &
                    "ClearanceCost REAL DEFAULT 0, " &
                    "TotalExtraCosts REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال ترخیص', " &
                    "ClearanceDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim custCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ImportCustoms"), 0))
                If custCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ImportCustoms (CompanyID, ProformaID, DeclarationNo, CustomsName, DutyAmount, VatAmount, ShippingCost, ClearanceCost, TotalExtraCosts, Status, ClearanceDate, Notes) " &
                        "VALUES (1, 1, 'DEC-1049', 'گمرک شهید رجایی بندرعباس', 1625000000, 3250000000, 850000000, 350000000, 6075000000, 'در حال ترخیص', ?, 'اظهارنامه ترخیص قطعات صنعتی')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetProformas(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, ProformaID, PINumber, SupplierName, CurrencyCode, CurrencyRate, CurrencyAmount, IrrAmount, Incoterms, Status, PIDate, Notes FROM ImportProformas WHERE CompanyID = ? ORDER BY ProformaID DESC", companyID)
        End Function

        Public Function GetLCs(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, L.LcID, P.PINumber, L.LcNumber, L.BankName, L.CurrencyCode, L.CurrencyAmount, L.IrrAmount, L.AdvancePayment, L.Status, L.IssueDate, L.Notes " &
                        "FROM ImportLCs L INNER JOIN ImportProformas P ON L.ProformaID = P.ProformaID " &
                        "WHERE L.CompanyID = ? ORDER BY L.LcID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetCustoms(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, C.CustomsID, P.PINumber, C.DeclarationNo, C.CustomsName, C.DutyAmount, " &
                        "C.VatAmount, C.ShippingCost, C.ClearanceCost, C.TotalExtraCosts, C.Status, C.ClearanceDate, C.Notes " &
                        "FROM ImportCustoms C INNER JOIN ImportProformas P ON C.ProformaID = P.ProformaID " &
                        "WHERE C.CompanyID = ? ORDER BY C.CustomsID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Sub SaveProforma(id As Integer, companyID As Integer, piNumber As String, supplier As String, currCode As String, currRate As Double, currAmt As Double, incoterms As String, notes As String)
            Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
            Dim irrAmt = currAmt * currRate

            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO ImportProformas (CompanyID, PINumber, SupplierName, CurrencyCode, CurrencyRate, CurrencyAmount, IrrAmount, Incoterms, Status, PIDate, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'ثبت سفارش شده', ?, ?)",
                    companyID, piNumber, supplier, currCode, currRate, currAmt, irrAmt, incoterms, dateStr, notes
                )
            End If
        End Sub

        Public Function CalculateLandedCostAndConfirm(customsID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT C.*, P.PINumber, P.IrrAmount FROM ImportCustoms C INNER JOIN ImportProformas P ON C.ProformaID = P.ProformaID WHERE C.CustomsID = ? AND C.CompanyID = ?", customsID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim piNum = Convert.ToString(row("PINumber"))
                Dim baseIrr = Convert.ToDouble(If(IsDBNull(row("IrrAmount")), 0, row("IrrAmount")))
                Dim duty = Convert.ToDouble(If(IsDBNull(row("DutyAmount")), 0, row("DutyAmount")))
                Dim vat = Convert.ToDouble(If(IsDBNull(row("VatAmount")), 0, row("VatAmount")))
                Dim shipping = Convert.ToDouble(If(IsDBNull(row("ShippingCost")), 0, row("ShippingCost")))
                Dim clearance = Convert.ToDouble(If(IsDBNull(row("ClearanceCost")), 0, row("ClearanceCost")))
                Dim totalLanded = baseIrr + duty + vat + shipping + clearance
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE ImportCustoms SET Status = 'ترخیص قطعی', TotalExtraCosts = ? WHERE CustomsID = ?", (duty + vat + shipping + clearance), customsID)

                ' Issue Background Double-Entry Accounting Voucher for Landed Cost & Warehouse Arrival in Sanad1 & Sanad2
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری بهای تمام‌شده واقعی (Landed Cost) و ترخیص کالای پروفرما " & piNum & " (مجموع: " & totalLanded.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم بازرگانی خارجی', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalLanded, totalLanded
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: موجودی کالای ترخیص شده / انبار کالا (کد کل 14)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '14', '01', ?, ?, 0)", entryID, "بهای تمام‌شده واقعی کالای وارداتی - PI " & piNum, totalLanded)

                ' Bestankar: سفارشات و اعتبارات اسنادی در جریان (کد کل 15)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '15', '01', ?, 0, ?)", entryID, "تسویه پرونده خرید خارجی و گمرک - PI " & piNum, totalLanded)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetLandedCostReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, P.PINumber, P.SupplierName, P.CurrencyCode, P.CurrencyAmount, P.IrrAmount AS BaseCost, " &
                        "COALESCE(C.DutyAmount, 0) AS CustomsDuty, " &
                        "COALESCE(C.ShippingCost, 0) AS FreightCost, " &
                        "COALESCE(C.ClearanceCost, 0) AS ClearanceCost, " &
                        "(P.IrrAmount + COALESCE(C.DutyAmount, 0) + COALESCE(C.ShippingCost, 0) + COALESCE(C.ClearanceCost, 0)) AS TotalLandedCost, " &
                        "ROUND(((COALESCE(C.DutyAmount, 0) + COALESCE(C.ShippingCost, 0) + COALESCE(C.ClearanceCost, 0)) / CASE WHEN P.IrrAmount = 0 THEN 1 ELSE P.IrrAmount END) * 100, 1) AS ExpensePercentage " &
                        "FROM ImportProformas P LEFT JOIN ImportCustoms C ON P.ProformaID = C.ProformaID " &
                        "WHERE P.CompanyID = ? GROUP BY P.ProformaID ORDER BY P.ProformaID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
