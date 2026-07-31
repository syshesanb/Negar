Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class SahamService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. SahamShareholders
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SahamShareholders (" &
                    "ShareholderID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ShareholderCode TEXT, " &
                    "FullName TEXT, " &
                    "NationalID TEXT, " &
                    "ShareType TEXT DEFAULT 'حقیقی', " &
                    "ShareCount REAL DEFAULT 10000, " &
                    "NominalValue REAL DEFAULT 1000, " &
                    "TotalValue REAL DEFAULT 10000000, " &
                    "OwnershipPercent REAL DEFAULT 1.0, " &
                    "BankAccount TEXT DEFAULT 'IR120190000000123456789012', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim shCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SahamShareholders"), 0))
                If shCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SahamShareholders (CompanyID, ShareholderCode, FullName, NationalID, ShareType, ShareCount, NominalValue, TotalValue, OwnershipPercent, BankAccount, Notes) " &
                        "VALUES (1, 'SH-1001', 'شرکت سرمایه‌گذاری توسعه صنعت نگار (سهامی عام)', '10102548963', 'حقوقی', 650000, 1000, 650000000, 65.0, 'IR880170000000987654321012', 'سهامدار عمده و دارنده کرسی هیات مدیره')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SahamShareholders (CompanyID, ShareholderCode, FullName, NationalID, ShareType, ShareCount, NominalValue, TotalValue, OwnershipPercent, BankAccount, Notes) " &
                        "VALUES (1, 'SH-1002', 'دکتر علی‌رضا شریفی', '0065489321', 'حقیقی', 350000, 1000, 350000000, 35.0, 'IR120190000000123456789012', 'عضو هیات مدیره و سهامدار اصلی')"
                    )
                End If

                ' 2. SahamDividends
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SahamDividends (" &
                    "DividendID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "FiscalYearName TEXT DEFAULT '1404', " &
                    "TotalNetProfit REAL DEFAULT 5000000000, " &
                    "DividendPerShare REAL DEFAULT 450, " &
                    "TotalDividends REAL DEFAULT 450000000, " &
                    "ApprovedDate TEXT, " &
                    "Status TEXT DEFAULT 'مصوب مجمع', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim divCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SahamDividends"), 0))
                If divCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SahamDividends (CompanyID, FiscalYearName, TotalNetProfit, DividendPerShare, TotalDividends, ApprovedDate, Status) " &
                        "VALUES (1, '1404', 5000000000, 450, 450000000, ?, 'مصوب مجمع')",
                        dateStr
                    )
                End If

                ' 3. SahamTransfers
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SahamTransfers (" &
                    "TransferID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "SellerName TEXT, " &
                    "BuyerName TEXT, " &
                    "ShareCount REAL DEFAULT 10000, " &
                    "PricePerShare REAL DEFAULT 2500, " &
                    "TotalAmount REAL DEFAULT 25000000, " &
                    "TransferDate TEXT, " &
                    "Notes TEXT);"
                )

                Dim trCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM SahamTransfers"), 0))
                If trCount = 0 Then
                    Dim nowStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SahamTransfers (CompanyID, SellerName, BuyerName, ShareCount, PricePerShare, TotalAmount, TransferDate, Notes) " &
                        "VALUES (1, 'دکتر علی‌رضا شریفی', 'مهندس محمدحسین رضایی', 50000, 2800, 140000000, ?, 'نقل و انتقال رسمی سهام بر اساس مبایعه‌نامه مجمع')",
                        nowStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetShareholders(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, ShareholderID, ShareholderCode, FullName, NationalID, ShareType, ShareCount, NominalValue, TotalValue, OwnershipPercent, BankAccount, Notes FROM SahamShareholders WHERE CompanyID = ? ORDER BY ShareholderID ASC", companyID)
        End Function

        Public Function GetDividends(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, DividendID, FiscalYearName, TotalNetProfit, DividendPerShare, TotalDividends, ApprovedDate, Status FROM SahamDividends WHERE CompanyID = ? ORDER BY DividendID DESC", companyID)
        End Function

        Public Function GetTransfers(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, TransferID, SellerName, BuyerName, ShareCount, PricePerShare, TotalAmount, TransferDate, Notes FROM SahamTransfers WHERE CompanyID = ? ORDER BY TransferID DESC", companyID)
        End Function

        Public Sub SaveShareholder(id As Integer, companyID As Integer, fullName As String, nationalID As String, shareType As String, shareCount As Double, bankAccount As String, notes As String)
            If id <= 0 Then
                Dim code = "SH-" & (Environment.TickCount Mod 10000).ToString()
                Dim nominalVal As Double = 1000
                Dim totalVal = shareCount * nominalVal
                Dim totalCompanyShares As Double = Convert.ToDouble(If(Sql.ExecuteScalar("SELECT COALESCE(SUM(ShareCount), 0) FROM SahamShareholders WHERE CompanyID = ?", companyID), 1000000))
                If totalCompanyShares <= 0 Then totalCompanyShares = 1000000
                Dim ownerPercent = Math.Round((shareCount * 100.0) / (totalCompanyShares + shareCount), 2)

                Sql.ExecuteNonQuery(
                    "INSERT INTO SahamShareholders (CompanyID, ShareholderCode, FullName, NationalID, ShareType, ShareCount, NominalValue, TotalValue, OwnershipPercent, BankAccount, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, code, fullName, nationalID, shareType, shareCount, nominalVal, totalVal, ownerPercent, bankAccount, notes
                )
            End If
        End Sub

        Public Function ApproveDividendAndIssueSanad(dividendID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM SahamDividends WHERE DividendID = ? AND CompanyID = ?", dividendID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim divDps = Convert.ToDouble(If(IsDBNull(row("DividendPerShare")), 0, row("DividendPerShare")))
                Dim totalDiv = Convert.ToDouble(If(IsDBNull(row("TotalDividends")), 0, row("TotalDividends")))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE SahamDividends SET Status = 'واریز شده و ثبت نهایی' WHERE DividendID = ?", dividendID)

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2 for Dividend Payout & Shareholders Liability
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری توزیع سود مصوب مجمع سال مالی " & salMaly & " (سود هر سهم: " & divDps.ToString("N0") & " ریال، مبلغ کل: " & totalDiv.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم امور سهامداران (Saham)', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalDiv, totalDiv
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: سود انباشته / سود مصوب مجمع (کد کل 33 - سود/زیان انباشته)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '33', '01', ?, ?, 0)", entryID, "تخصیص سود مصوب مجمع سالانه - سال " & salMaly, totalDiv)

                ' Bestankar: سود سهام پیشنهادی و پرداختنی سهامداران (کد کل 24 - سایر حساب‌های پرداختنی)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '24', '05', ?, 0, ?)", entryID, "بدهی سود سهام پرداختی به سهامداران", totalDiv)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace
