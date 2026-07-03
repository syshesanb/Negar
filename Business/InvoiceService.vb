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
    End Class
End Namespace
