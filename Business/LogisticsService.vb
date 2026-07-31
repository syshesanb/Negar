Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class LogisticsService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. LogisticsFleet
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LogisticsFleet (" &
                    "VehicleID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PlateNumber TEXT, " &
                    "VehicleType TEXT DEFAULT 'کامیونت', " &
                    "DriverName TEXT, " &
                    "CapacityKg REAL DEFAULT 3500, " &
                    "Ownership TEXT DEFAULT 'شرکتی', " &
                    "Status TEXT DEFAULT 'آماده به کار', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim fleetCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LogisticsFleet"), 0))
                If fleetCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LogisticsFleet (CompanyID, PlateNumber, VehicleType, DriverName, CapacityKg, Ownership, Status, Notes) " &
                        "VALUES (1, '77-ب-415-ایران 44', 'کامیونت ایسوزو ۶ تن', 'جناب آقای حیدری', 6000, 'شرکتی', 'آماده به کار', 'کامیونت یخچال‌دار توزیع مویرگی شهری')"
                    )
                End If

                ' 2. LogisticsRoutes
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LogisticsRoutes (" &
                    "RouteID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "RouteCode TEXT, " &
                    "RouteName TEXT, " &
                    "CityZone TEXT DEFAULT 'تهران - منطقه غرب', " &
                    "EstimatedHours REAL DEFAULT 6, " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim routeCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LogisticsRoutes"), 0))
                If routeCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LogisticsRoutes (CompanyID, RouteCode, RouteName, CityZone, EstimatedHours, Status, Notes) " &
                        "VALUES (1, 'R-101', 'مسیر شماره ۱ - صادقیه و آزادی', 'تهران - منطقه غرب', 6.5, 'فعال', 'مسیر توزیع مویرگی فروشگاه‌های غرب تهران')"
                    )
                End If

                ' 3. LogisticsManifests
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LogisticsManifests (" &
                    "ManifestID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "VehicleID INTEGER, " &
                    "RouteID INTEGER, " &
                    "ManifestNumber TEXT, " &
                    "DriverName TEXT, " &
                    "InvoiceCount INTEGER DEFAULT 0, " &
                    "TotalWeightKg REAL DEFAULT 0, " &
                    "FreightCost REAL DEFAULT 0, " &
                    "DistributorCommission REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'درحال توزیع', " &
                    "DispatchDate TEXT, " &
                    "SettlementDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim manCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LogisticsManifests"), 0))
                If manCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LogisticsManifests (CompanyID, VehicleID, RouteID, ManifestNumber, DriverName, InvoiceCount, TotalWeightKg, FreightCost, DistributorCommission, Status, DispatchDate, Notes) " &
                        "VALUES (1, 1, 1, 'MNF-9921', 'جناب آقای حیدری', 18, 3850, 18500000, 7500000, 'درحال توزیع', ?, 'بارنامه توزیع مویرگی ۱۸ فاکتور فروش')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetFleet(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, VehicleID, PlateNumber, VehicleType, DriverName, CapacityKg, Ownership, Status, Notes FROM LogisticsFleet WHERE CompanyID = ? ORDER BY VehicleID DESC", companyID)
        End Function

        Public Function GetRoutes(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, RouteID, RouteCode, RouteName, CityZone, EstimatedHours, Status, Notes FROM LogisticsRoutes WHERE CompanyID = ? ORDER BY RouteID DESC", companyID)
        End Function

        Public Function GetManifests(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, M.ManifestID, M.ManifestNumber, F.PlateNumber, R.RouteName, M.DriverName, " &
                        "M.InvoiceCount, M.TotalWeightKg, M.FreightCost, M.DistributorCommission, M.Status, M.DispatchDate, M.SettlementDate, M.Notes " &
                        "FROM LogisticsManifests M " &
                        "INNER JOIN LogisticsFleet F ON M.VehicleID = F.VehicleID " &
                        "INNER JOIN LogisticsRoutes R ON M.RouteID = R.RouteID " &
                        "WHERE M.CompanyID = ? ORDER BY M.ManifestID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Sub SaveVehicle(id As Integer, companyID As Integer, plate As String, typeName As String, driver As String, capKg As Double, ownership As String, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO LogisticsFleet (CompanyID, PlateNumber, VehicleType, DriverName, CapacityKg, Ownership, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, 'آماده به کار', ?)",
                    companyID, plate, typeName, driver, capKg, ownership, notes
                )
            End If
        End Sub

        Public Function SettleManifestAndIssueSanad(manifestID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT M.*, F.PlateNumber FROM LogisticsManifests M INNER JOIN LogisticsFleet F ON M.VehicleID = F.VehicleID WHERE M.ManifestID = ? AND M.CompanyID = ?", manifestID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim manifestNum = Convert.ToString(row("ManifestNumber"))
                Dim freight = Convert.ToDouble(If(IsDBNull(row("FreightCost")), 0, row("FreightCost")))
                Dim comm = Convert.ToDouble(If(IsDBNull(row("DistributorCommission")), 0, row("DistributorCommission")))
                Dim totalCost = freight + comm
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE LogisticsManifests SET Status = 'تسویه شده', SettlementDate = ? WHERE ManifestID = ?", dateStr, manifestID)

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2 for Freight & Logistics Expense
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری کرایه حمل و پخش مویرگی بارنامه " & manifestNum & " (مجموع: " & totalCost.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم لوجستیک و پخش', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalCost, totalCost
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: هزینه حمل و نقل و توزیع فروش (کد کل 52 - هزینه توزیع و فروش)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '52', '03', ?, ?, 0)", entryID, "هزینه حمل و پورسانت توزیع - بارنامه " & manifestNum, totalCost)

                ' Bestankar: حساب‌های پرداختنی رانندگان و موزعان (کد کل 21)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '21', '08', ?, 0, ?)", entryID, "تسویه بارنامه توزیع - بارنامه " & manifestNum, totalCost)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetLogisticsReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, F.PlateNumber, F.VehicleType, F.DriverName, " &
                        "COUNT(M.ManifestID) AS TotalManifests, " &
                        "COALESCE(SUM(M.InvoiceCount), 0) AS TotalDeliveredInvoices, " &
                        "COALESCE(SUM(M.TotalWeightKg), 0) AS TotalTonnageKg, " &
                        "COALESCE(SUM(M.FreightCost), 0) AS TotalFreightCost, " &
                        "COALESCE(SUM(M.DistributorCommission), 0) AS TotalCommission " &
                        "FROM LogisticsFleet F LEFT JOIN LogisticsManifests M ON F.VehicleID = M.VehicleID " &
                        "WHERE F.CompanyID = ? GROUP BY F.VehicleID ORDER BY F.VehicleID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
