Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class AmvalService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. Categories
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS FixedAssetCategories (" &
                    "CategoryID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CategoryCode TEXT, " &
                    "CategoryName TEXT, " &
                    "DepreciationMethod INTEGER, " & ' 1=Straight-Line, 2=Declining
                    "DepreciationRate REAL, " & ' Rate (%) or Years
                    "AccountAssetCode TEXT, " &
                    "AccountDepreciationCode TEXT, " &
                    "AccountExpenseCode TEXT);"
                )

                ' Seed default categories if empty
                Dim catCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM FixedAssetCategories"), 0))
                If catCount = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO FixedAssetCategories (CategoryCode, CategoryName, DepreciationMethod, DepreciationRate, AccountAssetCode, AccountDepreciationCode, AccountExpenseCode) VALUES ('101', 'ساختمان‌ها و تأسیسات', 1, 25.0, '1101', '1102', '6101')")
                    Sql.ExecuteNonQuery("INSERT INTO FixedAssetCategories (CategoryCode, CategoryName, DepreciationMethod, DepreciationRate, AccountAssetCode, AccountDepreciationCode, AccountExpenseCode) VALUES ('102', 'ماشین‌آلات و تجهیزات تولیدی', 2, 15.0, '1103', '1104', '6102')")
                    Sql.ExecuteNonQuery("INSERT INTO FixedAssetCategories (CategoryCode, CategoryName, DepreciationMethod, DepreciationRate, AccountAssetCode, AccountDepreciationCode, AccountExpenseCode) VALUES ('103', 'وسایل نقلیه', 2, 20.0, '1105', '1106', '6103')")
                    Sql.ExecuteNonQuery("INSERT INTO FixedAssetCategories (CategoryCode, CategoryName, DepreciationMethod, DepreciationRate, AccountAssetCode, AccountDepreciationCode, AccountExpenseCode) VALUES ('104', 'اثاثیه و منصوبات اداری', 1, 10.0, '1107', '1108', '6104')")
                    Sql.ExecuteNonQuery("INSERT INTO FixedAssetCategories (CategoryCode, CategoryName, DepreciationMethod, DepreciationRate, AccountAssetCode, AccountDepreciationCode, AccountExpenseCode) VALUES ('105', 'تجهیزات رایانه‌ای و سخت‌افزار', 1, 3.0, '1109', '1110', '6105')")
                End If

                ' 2. Assets
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS FixedAssets (" &
                    "AssetID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "AssetCode TEXT, " &
                    "PlakNo TEXT, " &
                    "AssetName TEXT, " &
                    "CategoryID INTEGER, " &
                    "PurchaseDate TEXT, " &
                    "PurchasePrice REAL DEFAULT 0, " &
                    "SalvageValue REAL DEFAULT 0, " &
                    "Location TEXT, " &
                    "PersonnelID INTEGER DEFAULT 0, " &
                    "Status INTEGER DEFAULT 1, " & ' 1=Active, 2=Disposed, 3=Transferred, 4=Written-off
                    "OverhaulAmount REAL DEFAULT 0, " &
                    "AccumulatedDepreciation REAL DEFAULT 0, " &
                    "BookValue REAL DEFAULT 0, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' 3. Depreciations
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS AssetDepreciations (" &
                    "DepreciationID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "AssetID INTEGER, " &
                    "CompanyID INTEGER, " &
                    "SalMaly TEXT, " &
                    "MahMaly INTEGER, " &
                    "DepreciationAmount REAL DEFAULT 0, " &
                    "AccumulatedDepreciation REAL DEFAULT 0, " &
                    "BookValue REAL DEFAULT 0, " &
                    "SanadNo INTEGER DEFAULT 0, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' 4. Transfers
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS AssetTransfers (" &
                    "TransferID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "AssetID INTEGER, " &
                    "CompanyID INTEGER, " &
                    "TransferDate TEXT, " &
                    "FromLocation TEXT, " &
                    "ToLocation TEXT, " &
                    "FromPersonnelID INTEGER DEFAULT 0, " &
                    "ToPersonnelID INTEGER DEFAULT 0, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetCategories() As DataTable
            Return Sql.ExecuteTable("SELECT CategoryID, CategoryCode, CategoryName, DepreciationMethod, DepreciationRate FROM FixedAssetCategories ORDER BY CategoryCode")
        End Function

        Public Function GetAssets(companyID As Integer) As DataTable
            Dim query = "SELECT a.AssetID, a.AssetCode, a.PlakNo, a.AssetName, c.CategoryName, a.PurchaseDate, " &
                        "a.PurchasePrice, a.SalvageValue, a.Location, COALESCE(p.FullName, '-') AS CustodianName, " &
                        "CASE a.Status WHEN 1 THEN 'فعال' WHEN 2 THEN 'واگذار شده' WHEN 3 THEN 'منتقل شده' ELSE 'از رده خارج' END AS StatusTitle, " &
                        "a.AccumulatedDepreciation, a.BookValue, a.Notes " &
                        "FROM FixedAssets a " &
                        "LEFT JOIN FixedAssetCategories c ON a.CategoryID = c.CategoryID " &
                        "LEFT JOIN PayrollPersonnel p ON a.PersonnelID = p.PersonnelID " &
                        "WHERE a.CompanyID = ? ORDER BY a.AssetID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetAssetById(assetID As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM FixedAssets WHERE AssetID = ?", assetID)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveAsset(assetID As Integer, companyID As Integer, assetCode As String, plakNo As String, assetName As String, categoryID As Integer, purchaseDate As String, purchasePrice As Double, salvageValue As Double, location As String, personnelID As Integer, notes As String)
            Dim bookVal = Math.Max(0, purchasePrice - salvageValue)
            If assetID <= 0 Then
                ' New asset
                Sql.ExecuteNonQuery(
                    "INSERT INTO FixedAssets (CompanyID, AssetCode, PlakNo, AssetName, CategoryID, PurchaseDate, PurchasePrice, SalvageValue, Location, PersonnelID, Status, BookValue, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 1, ?, ?)",
                    companyID, assetCode, plakNo, assetName, categoryID, purchaseDate, purchasePrice, salvageValue, location, personnelID, bookVal, notes
                )
            Else
                ' Edit asset
                Sql.ExecuteNonQuery(
                    "UPDATE FixedAssets SET AssetCode = ?, PlakNo = ?, AssetName = ?, CategoryID = ?, PurchaseDate = ?, PurchasePrice = ?, SalvageValue = ?, Location = ?, PersonnelID = ?, Notes = ? " &
                    "WHERE AssetID = ? AND CompanyID = ?",
                    assetCode, plakNo, assetName, categoryID, purchaseDate, purchasePrice, salvageValue, location, personnelID, notes, assetID, companyID
                )
            End If
        End Sub

        Public Sub DeleteAsset(assetID As Integer, companyID As Integer)
            Sql.ExecuteNonQuery("DELETE FROM FixedAssets WHERE AssetID = ? AND CompanyID = ?", assetID, companyID)
            Sql.ExecuteNonQuery("DELETE FROM AssetDepreciations WHERE AssetID = ? AND CompanyID = ?", assetID, companyID)
            Sql.ExecuteNonQuery("DELETE FROM AssetTransfers WHERE AssetID = ? AND CompanyID = ?", assetID, companyID)
        End Sub

        Public Function GetDepreciationsForPeriod(companyID As Integer, salMaly As String, mahMaly As Integer) As DataTable
            Dim query = "SELECT d.DepreciationID, a.AssetCode, a.PlakNo, a.AssetName, c.CategoryName, " &
                        "d.SalMaly, d.MahMaly, d.DepreciationAmount, d.AccumulatedDepreciation, d.BookValue, d.SanadNo " &
                        "FROM AssetDepreciations d " &
                        "INNER JOIN FixedAssets a ON d.AssetID = a.AssetID " &
                        "LEFT JOIN FixedAssetCategories c ON a.CategoryID = c.CategoryID " &
                        "WHERE d.CompanyID = ? AND d.SalMaly = ? AND d.MahMaly = ? ORDER BY d.DepreciationID DESC"
            Return Sql.ExecuteTable(query, companyID, salMaly, mahMaly)
        End Function

        Public Function CalculateDepreciationForPeriod(companyID As Integer, salMaly As String, mahMaly As Integer) As Boolean
            ' Delete previous calculations for this period if any
            Sql.ExecuteNonQuery("DELETE FROM AssetDepreciations WHERE CompanyID = ? AND SalMaly = ? AND MahMaly = ?", companyID, salMaly, mahMaly)

            Dim dtAssets = Sql.ExecuteTable("SELECT a.*, c.DepreciationMethod, c.DepreciationRate FROM FixedAssets a INNER JOIN FixedAssetCategories c ON a.CategoryID = c.CategoryID WHERE a.CompanyID = ? AND a.Status = 1", companyID)
            If dtAssets Is Nothing OrElse dtAssets.Rows.Count = 0 Then Return False

            Dim totalDepreciationAmount As Double = 0

            For Each row As DataRow In dtAssets.Rows
                Dim assetID = Convert.ToInt32(row("AssetID"))
                Dim purchasePrice = Convert.ToDouble(If(IsDBNull(row("PurchasePrice")), 0, row("PurchasePrice")))
                Dim salvageValue = Convert.ToDouble(If(IsDBNull(row("SalvageValue")), 0, row("SalvageValue")))
                Dim accumulatedDep = Convert.ToDouble(If(IsDBNull(row("AccumulatedDepreciation")), 0, row("AccumulatedDepreciation")))
                Dim method = Convert.ToInt32(If(IsDBNull(row("DepreciationMethod")), 1, row("DepreciationMethod")))
                Dim rate = Convert.ToDouble(If(IsDBNull(row("DepreciationRate")), 10, row("DepreciationRate")))

                Dim basePrice = purchasePrice - salvageValue
                Dim currentBookValue = Math.Max(0, basePrice - accumulatedDep)

                If currentBookValue <= 0 Then Continue For

                Dim monthlyDep As Double = 0
                If method = 1 Then
                    ' Straight Line: rate is useful life in years
                    Dim years = If(rate > 0, rate, 10)
                    monthlyDep = Math.Round(basePrice / (years * 12.0), 0)
                Else
                    ' Declining Balance: rate is percentage
                    monthlyDep = Math.Round((currentBookValue * (rate / 100.0)) / 12.0, 0)
                End If

                monthlyDep = Math.Min(monthlyDep, currentBookValue)
                Dim newAccumulated = accumulatedDep + monthlyDep
                Dim newBookValue = Math.Max(0, basePrice - newAccumulated)

                totalDepreciationAmount += monthlyDep

                ' Update Asset
                Sql.ExecuteNonQuery("UPDATE FixedAssets SET AccumulatedDepreciation = ?, BookValue = ? WHERE AssetID = ?", newAccumulated, newBookValue, assetID)

                ' Insert Depreciation Record
                Sql.ExecuteNonQuery(
                    "INSERT INTO AssetDepreciations (AssetID, CompanyID, SalMaly, MahMaly, DepreciationAmount, AccumulatedDepreciation, BookValue) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?)",
                    assetID, companyID, salMaly, mahMaly, monthlyDep, newAccumulated, newBookValue
                )
            Next

            ' Issue Accounting Voucher in Sanad1 and Sanad2 if totalDepreciationAmount > 0
            If totalDepreciationAmount > 0 Then
                IssueDepreciationVoucher(companyID, salMaly, mahMaly, totalDepreciationAmount)
            End If

            Return True
        End Function

        Private Sub IssueDepreciationVoucher(companyID As Integer, salMaly As String, mahMaly As Integer, totalAmount As Double)
            Try
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                Dim desc = "سند محاسبه استهلاک دارایی‌های ثابت ماه " & mahMaly.ToString() & " سال " & salMaly

                ' Find next reference number
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1

                ' Insert Sanad1 with AdamVirayesh = 1
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم اموال', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalAmount, totalAmount
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Sanad2 Bedehkar: هزینه استهلاک دارایی‌ها (کد کل 61)
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) " &
                    "VALUES (?, '61', '01', ?, ?, 0)",
                    entryID, "هزینه استهلاک دارایی‌های ثابت ماه " & mahMaly.ToString(), totalAmount
                )

                ' Sanad2 Bestankar: استهلاک انباشته (کد کل 11)
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) " &
                    "VALUES (?, '11', '02', ?, 0, ?)",
                    entryID, "استهلاک انباشته دارایی‌های ثابت ماه " & mahMaly.ToString(), totalAmount
                )

                ' Update SanadNo in AssetDepreciations
                Sql.ExecuteNonQuery("UPDATE AssetDepreciations SET SanadNo = ? WHERE CompanyID = ? AND SalMaly = ? AND MahMaly = ?", nextRef, companyID, salMaly, mahMaly)
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetTaxDepreciationReport(companyID As Integer, salMaly As String) As DataTable
            Dim query = "SELECT a.AssetCode, a.PlakNo, a.AssetName, c.CategoryName, " &
                        "CASE c.DepreciationMethod WHEN 1 THEN 'مستقیم (' || c.DepreciationRate || ' سال)' ELSE 'نزولی (' || c.DepreciationRate || '%)' END AS MethodTitle, " &
                        "a.PurchaseDate, a.PurchasePrice, a.SalvageValue, " &
                        "COALESCE(SUM(d.DepreciationAmount), 0) AS PeriodDepreciation, " &
                        "a.AccumulatedDepreciation, a.BookValue " &
                        "FROM FixedAssets a " &
                        "LEFT JOIN FixedAssetCategories c ON a.CategoryID = c.CategoryID " &
                        "LEFT JOIN AssetDepreciations d ON a.AssetID = d.AssetID AND d.SalMaly = ? " &
                        "WHERE a.CompanyID = ? " &
                        "GROUP BY a.AssetID ORDER BY a.AssetID DESC"
            Return Sql.ExecuteTable(query, salMaly, companyID)
        End Function
    End Class
End Namespace
