Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class PmService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. PmAssets
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PmAssets (" &
                    "AssetID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "AssetCode TEXT, " &
                    "AssetName TEXT, " &
                    "Category TEXT DEFAULT 'ماشین‌آلات اصلی', " &
                    "LocationName TEXT, " &
                    "CostCenter TEXT, " &
                    "Status TEXT DEFAULT 'فعال در خط', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim assetCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM PmAssets"), 0))
                If assetCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO PmAssets (CompanyID, AssetCode, AssetName, Category, LocationName, CostCenter, Status, Notes) " &
                        "VALUES (1, 'EQ-101', 'پرس هیدرولیک ۲۰۰ تن کوماتسو', 'ماشین‌آلات اصلی', 'سالن تولید شماره ۱', 'مرکز هزینه پرس‌کاری', 'فعال در خط', 'دستگاه اصلی خط تزریق و پرس')"
                    )
                End If

                ' 2. PmSchedules
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PmSchedules (" &
                    "ScheduleID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "AssetID INTEGER, " &
                    "TaskTitle TEXT, " &
                    "IntervalType TEXT DEFAULT 'ماهانه', " &
                    "IntervalValue INTEGER DEFAULT 1, " &
                    "LastDoneDate TEXT, " &
                    "NextDueDate TEXT, " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim schedCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM PmSchedules"), 0))
                If schedCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO PmSchedules (CompanyID, AssetID, TaskTitle, IntervalType, IntervalValue, LastDoneDate, NextDueDate, Status, Notes) " &
                        "VALUES (1, 1, 'تعویض روغن هیدرولیک و فیلترهای روغن', 'ساعتی', 500, ?, ?, 'فعال', 'سرویس دوره ۵۰۰ ساعته')",
                        dateStr, dateStr
                    )
                End If

                ' 3. PmWorkOrders
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PmWorkOrders (" &
                    "WorkOrderID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "AssetID INTEGER, " &
                    "OrderType TEXT DEFAULT 'پیشگیرانه PM', " &
                    "Title TEXT, " &
                    "TechnicianName TEXT, " &
                    "DowntimeHours REAL DEFAULT 0, " &
                    "PartsCost REAL DEFAULT 0, " &
                    "LaborCost REAL DEFAULT 0, " &
                    "TotalCost REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'درحال انجام', " &
                    "StartDate TEXT, " &
                    "CompletionDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim woCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM PmWorkOrders"), 0))
                If woCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO PmWorkOrders (CompanyID, AssetID, OrderType, Title, TechnicianName, DowntimeHours, PartsCost, LaborCost, TotalCost, Status, StartDate, Notes) " &
                        "VALUES (1, 1, 'پیشگیرانه PM', 'سرویس دوره ۵۰۰ ساعته فیلتر و روغن', 'مهندس علوی (واحد نت)', 2.5, 45000000, 15000000, 60000000, 'درحال انجام', ?, 'تعویض فیلتر روغن هیدرولیک اصلی')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetAssets(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, AssetID, AssetCode, AssetName, Category, LocationName, CostCenter, Status, Notes FROM PmAssets WHERE CompanyID = ? ORDER BY AssetID DESC", companyID)
        End Function

        Public Function GetSchedules(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, S.ScheduleID, A.AssetCode, A.AssetName, S.TaskTitle, S.IntervalType, S.IntervalValue, S.LastDoneDate, S.NextDueDate, S.Status, S.Notes " &
                        "FROM PmSchedules S INNER JOIN PmAssets A ON S.AssetID = A.AssetID " &
                        "WHERE S.CompanyID = ? ORDER BY S.ScheduleID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetWorkOrders(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, W.WorkOrderID, A.AssetCode, A.AssetName, W.OrderType, W.Title, W.TechnicianName, " &
                        "W.DowntimeHours, W.PartsCost, W.LaborCost, W.TotalCost, W.Status, W.StartDate, W.CompletionDate, W.Notes " &
                        "FROM PmWorkOrders W INNER JOIN PmAssets A ON W.AssetID = A.AssetID " &
                        "WHERE W.CompanyID = ? ORDER BY W.WorkOrderID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Sub SaveAsset(id As Integer, companyID As Integer, code As String, name As String, category As String, location As String, costCenter As String, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO PmAssets (CompanyID, AssetCode, AssetName, Category, LocationName, CostCenter, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, 'فعال در خط', ?)",
                    companyID, code, name, category, location, costCenter, notes
                )
            End If
        End Sub

        Public Function CompleteWorkOrderAndIssueSanad(workOrderID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT W.*, A.AssetCode, A.AssetName FROM PmWorkOrders W INNER JOIN PmAssets A ON W.AssetID = A.AssetID WHERE W.WorkOrderID = ? AND W.CompanyID = ?", workOrderID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim assetName = Convert.ToString(row("AssetName"))
                Dim partsCost = Convert.ToDouble(If(IsDBNull(row("PartsCost")), 0, row("PartsCost")))
                Dim laborCost = Convert.ToDouble(If(IsDBNull(row("LaborCost")), 0, row("LaborCost")))
                Dim totalCost = partsCost + laborCost
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE PmWorkOrders SET Status = 'تکمیل شده', TotalCost = ?, CompletionDate = ? WHERE WorkOrderID = ?", totalCost, dateStr, workOrderID)

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2 for Maintenance Expense
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری نگهداری و تعمیرات (PM/EM) تجهیز " & assetName & " (مجموع هزینه: " & totalCost.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم نت (PM)', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalCost, totalCost
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: هزینه تعمیر و نگهداری ماشین‌آلات (کد کل 61 - سربار تولید)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '61', '05', ?, ?, 0)", entryID, "هزینه تعمیرات و نگهداری - " & assetName, totalCost)

                ' Bestankar: انبار قطعات یدکی و حساب‌های پرداختنی نت (کد کل 13 / 21)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '13', '02', ?, 0, ?)", entryID, "مصرف قطعات یدکی و دستمزد نت - " & assetName, totalCost)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetOeeReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, A.AssetCode, A.AssetName, A.Category, A.LocationName, " &
                        "COALESCE(SUM(W.DowntimeHours), 0) AS TotalDowntimeHours, " &
                        "COUNT(W.WorkOrderID) AS TotalMaintenanceOrders, " &
                        "COALESCE(SUM(W.TotalCost), 0) AS TotalMaintenanceCost, " &
                        "CASE WHEN COUNT(W.WorkOrderID) = 0 THEN 99.5 ELSE ROUND(100.0 - (COALESCE(SUM(W.DowntimeHours), 0) * 1.5), 1) END AS OeePercentage " &
                        "FROM PmAssets A LEFT JOIN PmWorkOrders W ON A.AssetID = W.AssetID " &
                        "WHERE A.CompanyID = ? GROUP BY A.AssetID ORDER BY A.AssetID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
