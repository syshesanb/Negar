Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class InvoiceService
        Private ReadOnly logService As New ActivityLogService()

        Public Function GetPurchaseInvoices() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable("SELECT InvoiceID, InvoiceNumber, InvoiceDate, VendorName, TotalAmount, CreatedBy, WarehouseID FROM PurchaseInvoices ORDER BY InvoiceDate DESC")
            End If
            Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
            Return Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, InvoiceDate, VendorName, TotalAmount, CreatedBy, WarehouseID FROM PurchaseInvoices " &
                "WHERE CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ") ORDER BY InvoiceDate DESC")
        End Function

        Public Function GetSalesInvoices() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable("SELECT InvoiceID, InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, CreatedBy, WarehouseID FROM SalesInvoices ORDER BY InvoiceDate DESC")
            End If
            Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
            Return Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, CreatedBy, WarehouseID FROM SalesInvoices " &
                "WHERE CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ") ORDER BY InvoiceDate DESC")
        End Function

        Public Function SavePurchaseInvoice(invoiceNumber As String, invoiceDate As DateTime, vendorName As String, warehouseId As Integer, createdBy As Integer, lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim total As Decimal = 0D
            For Each line In lines
                total += line.Item2 * line.Item3
            Next
            Dim invoiceId = Sql.ExecuteIdentity("INSERT INTO PurchaseInvoices (InvoiceNumber, InvoiceDate, VendorName, TotalAmount, CreatedBy, WarehouseID) VALUES (?, ?, ?, ?, ?, ?)",
                                                invoiceNumber, invoiceDate, vendorName, total, createdBy, warehouseId)

            Dim inventoryService As New InventoryService()
            For Each line In lines
                Dim lineTotal = line.Item2 * line.Item3
                Sql.ExecuteNonQuery("INSERT INTO PurchaseInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice) VALUES (?, ?, ?, ?, ?)",
                                    invoiceId, line.Item1, line.Item2, line.Item3, lineTotal)

                Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", line.Item1, warehouseId)
                Dim current As Decimal = 0D
                If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                    current = Convert.ToDecimal(quantityValue)
                End If
                inventoryService.UpsertInventory(line.Item1, warehouseId, current + line.Item2, line.Item3)
            Next

            logService.LogActivity(createdBy, "CreatePurchase", "PurchaseInvoice", invoiceId,
                                   "ثبت فاکتور خرید: " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Function SaveSalesInvoice(invoiceNumber As String, invoiceDate As DateTime, customerName As String, warehouseId As Integer, createdBy As Integer, lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim total As Decimal = 0D
            For Each line In lines
                total += line.Item2 * line.Item3
            Next
            Dim invoiceId = Sql.ExecuteIdentity("INSERT INTO SalesInvoices (InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, CreatedBy, WarehouseID) VALUES (?, ?, ?, ?, ?, ?)",
                                                invoiceNumber, invoiceDate, customerName, total, createdBy, warehouseId)

            For Each line In lines
                Dim lineTotal = line.Item2 * line.Item3
                Dim costAtSale = GetAverageCost(line.Item1, warehouseId)
                Sql.ExecuteNonQuery("INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice, CostAtSaleTime) VALUES (?, ?, ?, ?, ?, ?)",
                                    invoiceId, line.Item1, line.Item2, lineTotal, lineTotal, costAtSale)

                Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", line.Item1, warehouseId)
                Dim current As Decimal = 0D
                If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                    current = Convert.ToDecimal(quantityValue)
                End If
                Dim inventoryService As New InventoryService()
                inventoryService.UpsertInventory(line.Item1, warehouseId, current - line.Item2, costAtSale)
            Next

            logService.LogActivity(createdBy, "CreateSale", "SalesInvoice", invoiceId,
                                   "ثبت فاکتور فروش: " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Function GetAverageCost(productId As Integer, warehouseId As Integer) As Decimal
            Dim value = Sql.ExecuteScalar("SELECT AverageCost FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", productId, warehouseId)
            If value Is Nothing OrElse Convert.IsDBNull(value) Then Return 0D
            Return Convert.ToDecimal(value)
        End Function
        ' ══════════════════════════════════════════════════════
        '  دریافت اطلاعات پایه یک فاکتور (حالت ویرایش)
        ' ══════════════════════════════════════════════════════
        Public Function GetPurchaseInvoiceById(invoiceId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, InvoiceDate, VendorName, TotalAmount, WarehouseID " &
                "FROM PurchaseInvoices WHERE InvoiceID = ?", invoiceId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function GetSalesInvoiceById(invoiceId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, WarehouseID " &
                "FROM SalesInvoices WHERE InvoiceID = ?", invoiceId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        ' ══════════════════════════════════════════════════════
        '  دریافت ردیف‌های جزییات فاکتور
        ' ══════════════════════════════════════════════════════
        Public Function GetPurchaseInvoiceDetails(invoiceId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT d.DetailID, d.ProductID, " &
                "COALESCE(p.ProductName, '(کالای حذف شده)') AS ProductName, " &
                "d.Quantity, d.UnitPrice, d.TotalPrice " &
                "FROM PurchaseInvoiceDetails d " &
                "LEFT JOIN Products p ON p.ProductID = d.ProductID " &
                "WHERE d.InvoiceID = ?", invoiceId)
        End Function

        Public Function GetSalesInvoiceDetails(invoiceId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT d.DetailID, d.ProductID, " &
                "COALESCE(p.ProductName, '(کالای حذف شده)') AS ProductName, " &
                "d.Quantity, d.UnitPrice, d.TotalPrice " &
                "FROM SalesInvoiceDetails d " &
                "LEFT JOIN Products p ON p.ProductID = d.ProductID " &
                "WHERE d.InvoiceID = ?", invoiceId)
        End Function

        ' ══════════════════════════════════════════════════════
        '  حذف فاکتور (header + details + برگشت موجودی)
        ' ══════════════════════════════════════════════════════
        Public Sub DeletePurchaseInvoice(invoiceId As Integer)
            Dim hdr = GetPurchaseInvoiceById(invoiceId)
            If hdr Is Nothing Then Throw New InvalidOperationException("فاکتور یافت نشد.")
            Dim warehouseId = Convert.ToInt32(hdr("WarehouseID"))
            Dim details = GetPurchaseInvoiceDetails(invoiceId)
            Dim invSvc As New InventoryService()
            For Each row As DataRow In details.Rows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim qty = Convert.ToDecimal(row("Quantity"))
                Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, warehouseId), 0D))
                invSvc.UpsertInventory(pid, warehouseId, cur - qty, 0D)
            Next
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoiceDetails WHERE InvoiceID=?", invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoices WHERE InvoiceID=?", invoiceId)
            logService.LogActivity(0, "DeletePurchase", "PurchaseInvoice", invoiceId,
                                   "حذف فاکتور خرید: " & Convert.ToString(hdr("InvoiceNumber")), SessionContext.CurrentIP)
        End Sub

        Public Sub DeleteSalesInvoice(invoiceId As Integer)
            Dim hdr = GetSalesInvoiceById(invoiceId)
            If hdr Is Nothing Then Throw New InvalidOperationException("فاکتور یافت نشد.")
            Dim warehouseId = Convert.ToInt32(hdr("WarehouseID"))
            Dim details = GetSalesInvoiceDetails(invoiceId)
            Dim invSvc As New InventoryService()
            For Each row As DataRow In details.Rows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim qty = Convert.ToDecimal(row("Quantity"))
                Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, warehouseId), 0D))
                invSvc.UpsertInventory(pid, warehouseId, cur + qty, GetAverageCost(pid, warehouseId))
            Next
            Sql.ExecuteNonQuery("DELETE FROM SalesInvoiceDetails WHERE InvoiceID=?", invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM SalesInvoices WHERE InvoiceID=?", invoiceId)
            logService.LogActivity(0, "DeleteSale", "SalesInvoice", invoiceId,
                                   "حذف فاکتور فروش: " & Convert.ToString(hdr("InvoiceNumber")), SessionContext.CurrentIP)
        End Sub

        ' ══════════════════════════════════════════════════════
        '  ویرایش فاکتور (برگشت موجودی قدیم + ثبت موجودی جدید)
        ' ══════════════════════════════════════════════════════
        Public Function UpdatePurchaseInvoice(invoiceId As Integer, invoiceNumber As String,
                                              invoiceDate As DateTime, vendorName As String,
                                              warehouseId As Integer, createdBy As Integer,
                                              lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            ' ← برگشت موجودی قدیمی
            Dim oldHdr = GetPurchaseInvoiceById(invoiceId)
            Dim oldWarehouseId = Convert.ToInt32(oldHdr("WarehouseID"))
            Dim oldDetails = GetPurchaseInvoiceDetails(invoiceId)
            Dim invSvc As New InventoryService()
            For Each row As DataRow In oldDetails.Rows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim qty = Convert.ToDecimal(row("Quantity"))
                Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, oldWarehouseId), 0D))
                invSvc.UpsertInventory(pid, oldWarehouseId, cur - qty, 0D)
            Next
            ' ← ثبت جدید
            Dim total As Decimal = 0D
            For Each line In lines : total += line.Item2 * line.Item3 : Next
            Sql.ExecuteNonQuery(
                "UPDATE PurchaseInvoices SET InvoiceNumber=?, InvoiceDate=?, VendorName=?, TotalAmount=?, WarehouseID=? WHERE InvoiceID=?",
                invoiceNumber, invoiceDate, vendorName, total, warehouseId, invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoiceDetails WHERE InvoiceID=?", invoiceId)
            For Each line In lines
                Dim lineTotal = line.Item2 * line.Item3
                Sql.ExecuteNonQuery(
                    "INSERT INTO PurchaseInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice) VALUES (?,?,?,?,?)",
                    invoiceId, line.Item1, line.Item2, line.Item3, lineTotal)
                Dim cur2 = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", line.Item1, warehouseId), 0D))
                invSvc.UpsertInventory(line.Item1, warehouseId, cur2 + line.Item2, line.Item3)
            Next
            logService.LogActivity(createdBy, "UpdatePurchase", "PurchaseInvoice", invoiceId,
                                   "ویرایش فاکتور خرید: " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Function UpdateSalesInvoice(invoiceId As Integer, invoiceNumber As String,
                                           invoiceDate As DateTime, customerName As String,
                                           warehouseId As Integer, createdBy As Integer,
                                           lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim oldHdr = GetSalesInvoiceById(invoiceId)
            Dim oldWarehouseId = Convert.ToInt32(oldHdr("WarehouseID"))
            Dim oldDetails = GetSalesInvoiceDetails(invoiceId)
            Dim invSvc As New InventoryService()
            For Each row As DataRow In oldDetails.Rows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim qty = Convert.ToDecimal(row("Quantity"))
                Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, oldWarehouseId), 0D))
                invSvc.UpsertInventory(pid, oldWarehouseId, cur + qty, GetAverageCost(pid, oldWarehouseId))
            Next
            Dim total As Decimal = 0D
            For Each line In lines : total += line.Item2 * line.Item3 : Next
            Sql.ExecuteNonQuery(
                "UPDATE SalesInvoices SET InvoiceNumber=?, InvoiceDate=?, CustomerName=?, TotalAmount=?, WarehouseID=? WHERE InvoiceID=?",
                invoiceNumber, invoiceDate, customerName, total, warehouseId, invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM SalesInvoiceDetails WHERE InvoiceID=?", invoiceId)
            For Each line In lines
                Dim lineTotal = line.Item2 * line.Item3
                Dim costAtSale = GetAverageCost(line.Item1, warehouseId)
                Sql.ExecuteNonQuery(
                    "INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice, CostAtSaleTime) VALUES (?,?,?,?,?,?)",
                    invoiceId, line.Item1, line.Item2, lineTotal, lineTotal, costAtSale)
                Dim cur2 = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", line.Item1, warehouseId), 0D))
                invSvc.UpsertInventory(line.Item1, warehouseId, cur2 - line.Item2, costAtSale)
            Next
            logService.LogActivity(createdBy, "UpdateSale", "SalesInvoice", invoiceId,
                                   "ویرایش فاکتور فروش: " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

    End Class

End Namespace
