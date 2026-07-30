Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class ProductionService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. ProductionBOM
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProductionBOM (" &
                    "BomID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProductCode TEXT, " &
                    "ProductName TEXT, " &
                    "RawMaterialName TEXT, " &
                    "QuantityRequired REAL DEFAULT 1, " &
                    "WastePercent REAL DEFAULT 0, " &
                    "UnitName TEXT DEFAULT 'عدد', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim bomCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProductionBOM"), 0))
                If bomCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProductionBOM (CompanyID, ProductCode, ProductName, RawMaterialName, QuantityRequired, WastePercent, UnitName, Notes) " &
                        "VALUES (1, 'PRD-501', 'دستگاه تصفیه هوای صنعتی P-100', 'ورق استیل ۳۰۴ کارخانه‌ای', 2.5, 3.0, 'مترمربع', 'فرمول ساخت مصوب استاندارد')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProductionBOM (CompanyID, ProductCode, ProductName, RawMaterialName, QuantityRequired, WastePercent, UnitName, Notes) " &
                        "VALUES (1, 'PRD-501', 'دستگاه تصفیه هوای صنعتی P-100', 'موتور فن سانترفیوژ ۲۲۰ ولت', 1.0, 0.0, 'دستگاه', 'موتور اصلی خط تولید')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProductionBOM (CompanyID, ProductCode, ProductName, RawMaterialName, QuantityRequired, WastePercent, UnitName, Notes) " &
                        "VALUES (1, 'PRD-501', 'دستگاه تصفیه هوای صنعتی P-100', 'فیلتر هپا (HEPA) کلاس H14', 2.0, 1.0, 'عدد', 'فیلترهای تصفیه هوا')"
                    )
                End If

                ' 2. ProductionOrders
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProductionOrders (" &
                    "OrderID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "OrderNo TEXT, " &
                    "ProductCode TEXT, " &
                    "ProductName TEXT, " &
                    "TargetQuantity REAL DEFAULT 1, " &
                    "ProducedQuantity REAL DEFAULT 0, " &
                    "DirectMaterialCost REAL DEFAULT 0, " &
                    "DirectLaborCost REAL DEFAULT 0, " &
                    "OverheadCost REAL DEFAULT 0, " &
                    "UnitCost REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال تولید', " & ' 'در حال برنامه‌ریزی', 'در حال تولید', 'تکمیل شده'
                    "StartDate TEXT, " &
                    "EndDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim ordCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProductionOrders"), 0))
                If ordCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ProductionOrders (CompanyID, OrderNo, ProductCode, ProductName, TargetQuantity, ProducedQuantity, DirectMaterialCost, DirectLaborCost, OverheadCost, UnitCost, Status, StartDate, Notes) " &
                        "VALUES (1, 'ORD-9901', 'PRD-501', 'دستگاه تصفیه هوای صنعتی P-100', 10, 10, 180000000, 35000000, 25000000, 24000000, 'در حال تولید', ?, 'کارت تولید سری زمستانه')",
                        dateStr
                    )
                End If

                ' 3. ProductionLogs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProductionLogs (" &
                    "LogID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "OrderID INTEGER, " &
                    "StageName TEXT DEFAULT 'مونتاژ اولیه', " &
                    "OperatorName TEXT DEFAULT 'سرپرست خط', " &
                    "SpentHours REAL DEFAULT 0, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetBOMList(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM ProductionBOM WHERE CompanyID = ? ORDER BY BomID DESC", companyID)
        End Function

        Public Function GetProductionOrders(companyID As Integer) As DataTable
            Dim query = "SELECT OrderID, OrderNo, ProductCode, ProductName, TargetQuantity, ProducedQuantity, " &
                        "DirectMaterialCost, DirectLaborCost, OverheadCost, " &
                        "(DirectMaterialCost + DirectLaborCost + OverheadCost) AS TotalProductionCost, " &
                        "UnitCost, Status, StartDate, EndDate, Notes " &
                        "FROM ProductionOrders WHERE CompanyID = ? ORDER BY OrderID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetProductionOrderById(id As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM ProductionOrders WHERE OrderID = ?", id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveBOM(id As Integer, companyID As Integer, prodCode As String, prodName As String, rawMat As String, qty As Double, waste As Double, unit As String, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO ProductionBOM (CompanyID, ProductCode, ProductName, RawMaterialName, QuantityRequired, WastePercent, UnitName, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, prodCode, prodName, rawMat, qty, waste, unit, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE ProductionBOM SET ProductCode = ?, ProductName = ?, RawMaterialName = ?, QuantityRequired = ?, WastePercent = ?, UnitName = ?, Notes = ? " &
                    "WHERE BomID = ? AND CompanyID = ?",
                    prodCode, prodName, rawMat, qty, waste, unit, notes, id, companyID
                )
            End If
        End Sub

        Public Sub SaveProductionOrder(id As Integer, companyID As Integer, orderNo As String, prodCode As String, prodName As String, targetQty As Double, matCost As Double, laborCost As Double, overheadCost As Double, notes As String)
            Dim totalCost = matCost + laborCost + overheadCost
            Dim unitCost = If(targetQty > 0, totalCost / targetQty, 0)
            Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO ProductionOrders (CompanyID, OrderNo, ProductCode, ProductName, TargetQuantity, ProducedQuantity, DirectMaterialCost, DirectLaborCost, OverheadCost, UnitCost, Status, StartDate, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, 0, ?, ?, ?, ?, 'در حال تولید', ?, ?)",
                    companyID, orderNo, prodCode, prodName, targetQty, matCost, laborCost, overheadCost, unitCost, dateStr, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE ProductionOrders SET OrderNo = ?, ProductCode = ?, ProductName = ?, TargetQuantity = ?, DirectMaterialCost = ?, DirectLaborCost = ?, OverheadCost = ?, UnitCost = ?, Notes = ? " &
                    "WHERE OrderID = ? AND CompanyID = ?",
                    orderNo, prodCode, prodName, targetQty, matCost, laborCost, overheadCost, unitCost, notes, id, companyID
                )
            End If
        End Sub

        Public Function CompleteProductionOrder(orderID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM ProductionOrders WHERE OrderID = ? AND CompanyID = ?", orderID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim targetQty = Convert.ToDouble(If(IsDBNull(row("TargetQuantity")), 1, row("TargetQuantity")))
                Dim matCost = Convert.ToDouble(If(IsDBNull(row("DirectMaterialCost")), 0, row("DirectMaterialCost")))
                Dim laborCost = Convert.ToDouble(If(IsDBNull(row("DirectLaborCost")), 0, row("DirectLaborCost")))
                Dim overheadCost = Convert.ToDouble(If(IsDBNull(row("OverheadCost")), 0, row("OverheadCost")))
                Dim totalCost = matCost + laborCost + overheadCost
                Dim unitCost = If(targetQty > 0, totalCost / targetQty, 0)
                Dim prodName = Convert.ToString(row("ProductName"))
                Dim orderNo = Convert.ToString(row("OrderNo"))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE ProductionOrders SET ProducedQuantity = ?, UnitCost = ?, Status = 'تکمیل شده', EndDate = ? WHERE OrderID = ?", targetQty, unitCost, dateStr, orderID)

                ' Issue Background Double-Entry Industrial Accounting Voucher in Sanad1 & Sanad2
                ' Even if current user has NO permission for Accounting, background business engine executes it!
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری صنعتی تکمیل تولید کارت " & orderNo & " - " & prodName & " (تعداد: " & targetQty & " عدد)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم تولید', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, totalCost, totalCost
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: انبار کالای ساخته شده (کد کل 14)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '14', '01', ?, ?, 0)", entryID, "رسید کالای ساخته شده - " & prodName, totalCost)

                ' Bestankar: کالای در جریان ساخت / مصرف مواد و دستمزد (کد کل 15)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '15', '01', ?, 0, ?)", entryID, "تسویه کالای در جریان ساخت (WIP)", totalCost)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetCostBreakdownReport(companyID As Integer) As DataTable
            Dim query = "SELECT ProductCode, ProductName, SUM(TargetQuantity) AS TotalQuantity, " &
                        "SUM(DirectMaterialCost) AS TotalMaterials, " &
                        "SUM(DirectLaborCost) AS TotalLabor, " &
                        "SUM(OverheadCost) AS TotalOverhead, " &
                        "SUM(DirectMaterialCost + DirectLaborCost + OverheadCost) AS TotalCost, " &
                        "AVG(UnitCost) AS AvgUnitCost " &
                        "FROM ProductionOrders WHERE CompanyID = ? GROUP BY ProductCode ORDER BY ProductCode"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
