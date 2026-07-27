Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Negar.Data

Namespace Negar.Business
    Public Class InvoiceService
        Private ReadOnly logService As New ActivityLogService()

        Public Function GetPurchaseInvoices() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim query = "SELECT i.InvoiceID, i.InvoiceNumber, COALESCE(i.InvoiceType, 'فاکتور خرید') AS InvoiceType, " &
                        "i.InvoiceDate, i.VendorName, i.TotalAmount, COALESCE(i.DiscountAmount, 0) AS DiscountAmount, " &
                        "i.PaymentType, i.Description, i.CreatedBy, i.WarehouseID, COALESCE(w.WarehouseName, '---') AS WarehouseName, " &
                        "COALESCE(i.ReceiptStatus, 'رسید نشده') AS ReceiptStatus " &
                        "FROM PurchaseInvoices i LEFT JOIN Warehouses w ON i.WarehouseID = w.WarehouseID "

            Dim conditions As New List(Of String)()
            If SessionContext.CurrentCompanyID.HasValue Then
                conditions.Add("(i.CompanyID = " & SessionContext.CurrentCompanyID.Value & " OR i.CompanyID IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                conditions.Add("i.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            If conditions.Count > 0 Then
                query &= "WHERE " & String.Join(" AND ", conditions.ToArray()) & " "
            End If

            query &= "ORDER BY i.InvoiceDate DESC, i.InvoiceID DESC"
            Return Sql.ExecuteTable(query)
        End Function

        Public Function GetSalesInvoices() As DataTable
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim conditions As New List(Of String)()
            If SessionContext.CurrentCompanyID.HasValue Then
                conditions.Add("(CompanyID = " & SessionContext.CurrentCompanyID.Value & " OR CompanyID IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                conditions.Add("CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            Dim query = "SELECT InvoiceID, InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, CreatedBy, WarehouseID FROM SalesInvoices "
            If conditions.Count > 0 Then
                query &= "WHERE " & String.Join(" AND ", conditions.ToArray()) & " "
            End If
            query &= "ORDER BY InvoiceDate DESC"

            Return Sql.ExecuteTable(query)
        End Function

        Public Function SavePurchaseInvoice(invoiceNumber As String, invoiceDate As DateTime, vendorName As String, warehouseId As Integer, createdBy As Integer, lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal, Decimal, Decimal)), Optional invoiceType As String = "فاکتور خرید", Optional discountAmount As Decimal = 0D, Optional paymentType As String = "نسیه", Optional description As String = "", Optional taxEntryMode As Integer = 0, Optional totalVat As Decimal = 0D) As Integer
            Dim total As Decimal = 0D
            For Each line In lines
                total += (line.Item2 * line.Item3) - line.Item4 + line.Item5
            Next
            If total > discountAmount Then
                total -= discountAmount
            Else
                total = 0D
            End If

            Dim compIdVal As Object = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, DBNull.Value)

            Dim invoiceId = Sql.ExecuteIdentity("INSERT INTO PurchaseInvoices (InvoiceNumber, InvoiceDate, VendorName, TotalAmount, CreatedBy, WarehouseID, InvoiceType, DiscountAmount, PaymentType, Description, TaxEntryMode, TotalVat, CompanyID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                                                invoiceNumber, invoiceDate, vendorName, total, createdBy, warehouseId, invoiceType, discountAmount, paymentType, description, taxEntryMode, totalVat, compIdVal)

            For Each line In lines
                Dim lineTotal = (line.Item2 * line.Item3) - line.Item4 + line.Item5
                Sql.ExecuteNonQuery("INSERT INTO PurchaseInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice, Discount, Vat, ReceivedQuantity) VALUES (?, ?, ?, ?, ?, ?, ?, 0)",
                                    invoiceId, line.Item1, line.Item2, line.Item3, lineTotal, line.Item4, line.Item5)
            Next

            logService.LogActivity(createdBy, "CreatePurchase", "PurchaseInvoice", invoiceId,
                                   "ثبت " & invoiceType & ": " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Function SaveSalesInvoice(invoiceNumber As String, invoiceDate As DateTime, customerName As String, warehouseId As Integer, createdBy As Integer, lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim total As Decimal = 0D
            For Each line In lines
                total += line.Item2 * line.Item3
            Next

            Dim compIdVal As Object = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, DBNull.Value)

            Dim invoiceId = Sql.ExecuteIdentity("INSERT INTO SalesInvoices (InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, CreatedBy, WarehouseID, CompanyID) VALUES (?, ?, ?, ?, ?, ?, ?)",
                                                invoiceNumber, invoiceDate, customerName, total, createdBy, warehouseId, compIdVal)

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

        Public Function GetPurchaseInvoiceById(invoiceId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, COALESCE(InvoiceType, 'فاکتور خرید') AS InvoiceType, " &
                "InvoiceDate, VendorName, TotalAmount, COALESCE(DiscountAmount, 0) AS DiscountAmount, " &
                "PaymentType, Description, WarehouseID, COALESCE(TaxEntryMode, 0) AS TaxEntryMode, COALESCE(TotalVat, 0) AS TotalVat FROM PurchaseInvoices WHERE InvoiceID = ?", invoiceId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function GetSalesInvoiceById(invoiceId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT InvoiceID, InvoiceNumber, InvoiceDate, CustomerName, TotalAmount, WarehouseID " &
                "FROM SalesInvoices WHERE InvoiceID = ?", invoiceId)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                If Not dt.Columns.Contains("Description") Then dt.Columns.Add("Description", GetType(String))
                If Not dt.Columns.Contains("VendorInvoiceNumber") Then dt.Columns.Add("VendorInvoiceNumber", GetType(String))
                Return dt.Rows(0)
            End If
            Return Nothing
        End Function

        Public Function GetPurchaseInvoiceDetails(invoiceId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT d.DetailID, d.ProductID, " &
                "COALESCE(p.ProductName, '(کالای حذف شده)') AS ProductName, " &
                "COALESCE(p.Unit, 'عدد') AS Unit, " &
                "d.Quantity, d.UnitPrice, d.TotalPrice, COALESCE(d.Discount, 0) AS Discount, COALESCE(d.Vat, 0) AS Vat, " &
                "COALESCE(d.ReceivedQuantity, 0) AS ReceivedQuantity " &
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

        Public Sub DeletePurchaseInvoice(invoiceId As Integer)
            Dim hdr = GetPurchaseInvoiceById(invoiceId)
            If hdr Is Nothing Then Throw New InvalidOperationException("فاکتور یافت نشد.")
            If Not hdr.IsNull("WarehouseID") Then
                Dim warehouseId = Convert.ToInt32(hdr("WarehouseID"))
                Dim details = GetPurchaseInvoiceDetails(invoiceId)
                Dim invSvc As New InventoryService()
                For Each row As DataRow In details.Rows
                    Dim pid = Convert.ToInt32(row("ProductID"))
                    Dim qty = Convert.ToDecimal(row("Quantity"))
                    Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, warehouseId), 0D))
                    invSvc.UpsertInventory(pid, warehouseId, cur - qty, 0D)
                Next
            End If
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoiceDetails WHERE InvoiceID=?", invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoices WHERE InvoiceID=?", invoiceId)
            logService.LogActivity(0, "DeletePurchase", "PurchaseInvoice", invoiceId,
                                   "حذف سند خرید: " & Convert.ToString(hdr("InvoiceNumber")), SessionContext.CurrentIP)
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

        Public Function UpdatePurchaseInvoice(invoiceId As Integer, invoiceNumber As String,
                                              invoiceDate As DateTime, vendorName As String,
                                              warehouseId As Integer, createdBy As Integer,
                                              lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal, Decimal, Decimal)),
                                              Optional invoiceType As String = "فاکتور خرید",
                                              Optional discountAmount As Decimal = 0D,
                                              Optional paymentType As String = "نسیه",
                                              Optional description As String = "",
                                              Optional taxEntryMode As Integer = 0,
                                              Optional totalVat As Decimal = 0D) As Integer
            ' ← برگشت موجودی قدیمی بر اساس مقدار رسید شده
            Dim oldHdr = GetPurchaseInvoiceById(invoiceId)
            Dim oldReceived As New Dictionary(Of Integer, Decimal)
            If oldHdr IsNot Nothing AndAlso Not oldHdr.IsNull("WarehouseID") Then
                Dim oldWarehouseId = Convert.ToInt32(oldHdr("WarehouseID"))
                Dim oldDetails = GetPurchaseInvoiceDetails(invoiceId)
                Dim invSvc As New InventoryService()
                For Each row As DataRow In oldDetails.Rows
                    Dim pid = Convert.ToInt32(row("ProductID"))
                    Dim rQty = Convert.ToDecimal(If(row.IsNull("ReceivedQuantity"), 0D, row("ReceivedQuantity")))
                    If Not oldReceived.ContainsKey(pid) Then oldReceived.Add(pid, rQty)
                    If rQty > 0 Then
                        Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, oldWarehouseId), 0D))
                        invSvc.UpsertInventory(pid, oldWarehouseId, cur - rQty, 0D)
                    End If
                Next
            End If

            ' ← ثبت جدید
            Dim total As Decimal = 0D
            For Each line In lines : total += (line.Item2 * line.Item3) - line.Item4 + line.Item5 : Next
            If total > discountAmount Then total -= discountAmount Else total = 0D

            Sql.ExecuteNonQuery(
                "UPDATE PurchaseInvoices SET InvoiceNumber=?, InvoiceDate=?, VendorName=?, TotalAmount=?, WarehouseID=?, InvoiceType=?, DiscountAmount=?, PaymentType=?, Description=?, TaxEntryMode=?, TotalVat=? WHERE InvoiceID=?",
                invoiceNumber, invoiceDate, vendorName, total, warehouseId, invoiceType, discountAmount, paymentType, description, taxEntryMode, totalVat, invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoiceDetails WHERE InvoiceID=?", invoiceId)

            Dim inventoryService As New InventoryService()
            For Each line In lines
                Dim pid = line.Item1
                Dim qty = line.Item2
                Dim rQty = If(oldReceived.ContainsKey(pid), oldReceived(pid), 0D)
                If rQty > qty Then rQty = qty ' سقف مقدار رسید شده برابر مقدار کل سطر است

                Dim lineTotal = (qty * line.Item3) - line.Item4 + line.Item5
                Sql.ExecuteNonQuery("INSERT INTO PurchaseInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, TotalPrice, Discount, Vat, ReceivedQuantity) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                                    invoiceId, pid, qty, line.Item3, lineTotal, line.Item4, line.Item5, rQty)

                If rQty > 0 Then
                    Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", pid, warehouseId)
                    Dim current As Decimal = 0D
                    If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                        current = Convert.ToDecimal(quantityValue)
                    End If
                    inventoryService.UpsertInventory(pid, warehouseId, current + rQty, line.Item3)
                End If
            Next

            logService.LogActivity(createdBy, "UpdatePurchase", "PurchaseInvoice", invoiceId,
                                   "ویرایش " & invoiceType & ": " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Function UpdateSalesInvoice(invoiceId As Integer, invoiceNumber As String,
                                           invoiceDate As DateTime, customerName As String,
                                           warehouseId As Integer, createdBy As Integer,
                                           lines As IEnumerable(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim oldHdr = GetSalesInvoiceById(invoiceId)
            If oldHdr IsNot Nothing AndAlso Not oldHdr.IsNull("WarehouseID") Then
                Dim oldWarehouseId = Convert.ToInt32(oldHdr("WarehouseID"))
                Dim oldDetails = GetSalesInvoiceDetails(invoiceId)
                Dim invSvc As New InventoryService()
                For Each row As DataRow In oldDetails.Rows
                    Dim pid = Convert.ToInt32(row("ProductID"))
                    Dim qty = Convert.ToDecimal(row("Quantity"))
                    Dim cur = Convert.ToDecimal(If(Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID=? AND WarehouseID=?", pid, oldWarehouseId), 0D))
                    invSvc.UpsertInventory(pid, oldWarehouseId, cur + qty, GetAverageCost(pid, oldWarehouseId))
                Next
            End If

            Dim total As Decimal = 0D
            For Each line In lines : total += line.Item2 * line.Item3 : Next

            Sql.ExecuteNonQuery(
                "UPDATE SalesInvoices SET InvoiceNumber=?, InvoiceDate=?, CustomerName=?, TotalAmount=?, WarehouseID=? WHERE InvoiceID=?",
                invoiceNumber, invoiceDate, customerName, total, warehouseId, invoiceId)
            Sql.ExecuteNonQuery("DELETE FROM SalesInvoiceDetails WHERE InvoiceID=?", invoiceId)

            Dim inventoryService As New InventoryService()
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
                inventoryService.UpsertInventory(line.Item1, warehouseId, current - line.Item2, costAtSale)
            Next

            logService.LogActivity(createdBy, "UpdateSale", "SalesInvoice", invoiceId,
                                   "ویرایش فاکتور فروش: " & invoiceNumber, SessionContext.CurrentIP)
            Return invoiceId
        End Function

        Public Sub SaveIndependentWarehouseReceipt(invoiceId As Integer, receiptNum As String, receiptDate As DateTime, createdBy As Integer, warehouseId As Integer, description As String, lines As List(Of Tuple(Of Integer, Integer, Decimal, Integer)))
            Dim inventoryService As New InventoryService()
            
            Dim receiptId = Sql.ExecuteIdentity("INSERT INTO WarehouseReceipts (ReceiptNumber, ReceiptDate, PurchaseInvoiceID, CreatedBy, WarehouseID, Description) VALUES (?, ?, ?, ?, ?, ?)",
                                                receiptNum, receiptDate, invoiceId, createdBy, warehouseId, description)

            For Each line In lines
                Dim detailId = line.Item1
                Dim pid = line.Item2
                Dim rQty = line.Item3
                Dim wId = line.Item4

                Sql.ExecuteNonQuery("INSERT INTO WarehouseReceiptDetails (ReceiptID, PurchaseInvoiceDetailID, ProductID, Quantity) VALUES (?, ?, ?, ?)",
                                    receiptId, detailId, pid, rQty)

                ' Update ReceivedQuantity in details (as cache)
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoiceDetails SET ReceivedQuantity = COALESCE(ReceivedQuantity, 0) + ? WHERE DetailID = ?", rQty, detailId)

                ' Update Inventory
                Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", pid, wId)
                Dim current As Decimal = 0D
                If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                    current = Convert.ToDecimal(quantityValue)
                End If
                inventoryService.UpsertInventory(pid, wId, current + rQty, 0D)
            Next
            
            UpdateReceiptStatus(invoiceId)
            
            Dim hdr = GetPurchaseInvoiceById(invoiceId)
            Dim invNum = If(hdr IsNot Nothing, Convert.ToString(hdr("InvoiceNumber")), invoiceId.ToString())
            logService.LogActivity(createdBy, "SaveWarehouseReceipt", "WarehouseReceipt", receiptId,
                                   "ثبت رسید انبار " & receiptNum & " برای فاکتور: " & invNum, SessionContext.CurrentIP)
        End Sub

        Public Function GetWarehouseReceiptsForInvoice(invoiceId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT r.ReceiptID, r.ReceiptNumber, r.ReceiptDate, r.Description, u.FullName AS CreatedBy, " &
                "(SELECT SUM(Quantity) FROM WarehouseReceiptDetails WHERE ReceiptID = r.ReceiptID) AS TotalQuantity " &
                "FROM WarehouseReceipts r " &
                "LEFT JOIN Users u ON r.CreatedBy = u.UserID " &
                "WHERE r.PurchaseInvoiceID = ? ORDER BY r.ReceiptDate DESC, r.ReceiptID DESC", invoiceId)
        End Function

        Public Sub UpdateReceiptStatus(invoiceId As Integer)
            Dim newDetails = GetPurchaseInvoiceDetails(invoiceId)
            Dim allFull = True
            Dim anyPartial = False
            For Each row As DataRow In newDetails.Rows
                Dim qty = Convert.ToDecimal(row("Quantity"))
                Dim rcv = Convert.ToDecimal(If(row.IsNull("ReceivedQuantity"), 0D, row("ReceivedQuantity")))
                If rcv < qty Then allFull = False
                If rcv > 0 Then anyPartial = True
            Next
            
            Dim status As String = "رسید نشده"
            If allFull Then
                status = "رسید کامل"
            ElseIf anyPartial Then
                status = "رسید ناقص"
            End If
            
            Sql.ExecuteNonQuery("UPDATE PurchaseInvoices SET ReceiptStatus = ? WHERE InvoiceID = ?", status, invoiceId)
        End Sub

        Public Function GetWarehouseReceiptById(receiptId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM WarehouseReceipts WHERE ReceiptID = ?", receiptId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function GetWarehouseReceiptDetailsList(receiptId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT rd.ReceiptDetailID, rd.PurchaseInvoiceDetailID, rd.ProductID, rd.Quantity, " &
                "pd.Quantity AS InvoiceQuantity, pd.ReceivedQuantity AS OldReceivedQuantity, " &
                "p.ProductCode, p.ProductName, p.Unit, " &
                "w.WarehouseName " &
                "FROM WarehouseReceiptDetails rd " &
                "JOIN PurchaseInvoiceDetails pd ON rd.PurchaseInvoiceDetailID = pd.DetailID " &
                "LEFT JOIN Products p ON rd.ProductID = p.ProductID " &
                "LEFT JOIN Warehouses w ON w.WarehouseID = (SELECT WarehouseID FROM WarehouseReceipts WHERE ReceiptID = rd.ReceiptID) " &
                "WHERE rd.ReceiptID = ?", receiptId)
        End Function

        Public Sub DeleteWarehouseReceipt(receiptId As Integer)
            Dim hdr = GetWarehouseReceiptById(receiptId)
            If hdr Is Nothing Then Return
            
            Dim invoiceId = Convert.ToInt32(hdr("PurchaseInvoiceID"))
            Dim wId = Convert.ToInt32(hdr("WarehouseID"))
            Dim details = GetWarehouseReceiptDetailsList(receiptId)
            Dim inventoryService As New InventoryService()
            
            For Each row As DataRow In details.Rows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim rQty = Convert.ToDecimal(row("Quantity"))
                Dim pdId = Convert.ToInt32(row("PurchaseInvoiceDetailID"))
                
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoiceDetails SET ReceivedQuantity = COALESCE(ReceivedQuantity, 0) - ? WHERE DetailID = ?", rQty, pdId)
                
                Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", pid, wId)
                Dim current As Decimal = 0D
                If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                    current = Convert.ToDecimal(quantityValue)
                End If
                inventoryService.UpsertInventory(pid, wId, current - rQty, 0D)
            Next
            
            Sql.ExecuteNonQuery("DELETE FROM WarehouseReceiptDetails WHERE ReceiptID = ?", receiptId)
            Sql.ExecuteNonQuery("DELETE FROM WarehouseReceipts WHERE ReceiptID = ?", receiptId)
            
            UpdateReceiptStatus(invoiceId)
            logService.LogActivity(0, "DeleteWarehouseReceipt", "WarehouseReceipt", receiptId, "حذف رسید انبار: " & Convert.ToString(hdr("ReceiptNumber")), SessionContext.CurrentIP)
        End Sub

        Public Sub UpdateIndependentWarehouseReceipt(receiptId As Integer, invoiceId As Integer, receiptNum As String, receiptDate As DateTime, createdBy As Integer, warehouseId As Integer, description As String, lines As List(Of Tuple(Of Integer, Integer, Decimal, Integer)))
            DeleteWarehouseReceipt(receiptId)
            
            Sql.ExecuteNonQuery("INSERT INTO WarehouseReceipts (ReceiptID, ReceiptNumber, ReceiptDate, PurchaseInvoiceID, CreatedBy, WarehouseID, Description) VALUES (?, ?, ?, ?, ?, ?, ?)",
                                receiptId, receiptNum, receiptDate, invoiceId, createdBy, warehouseId, description)
            
            Dim inventoryService As New InventoryService()
            For Each line In lines
                Dim pid = line.Item1
                Dim rQty = line.Item3
                Dim detailId = line.Item4
                If rQty <= 0 Then Continue For
                
                Sql.ExecuteNonQuery("INSERT INTO WarehouseReceiptDetails (ReceiptID, PurchaseInvoiceDetailID, ProductID, Quantity) VALUES (?, ?, ?, ?)",
                                    receiptId, detailId, pid, rQty)
                
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoiceDetails SET ReceivedQuantity = COALESCE(ReceivedQuantity, 0) + ? WHERE DetailID = ?", rQty, detailId)
                
                Dim quantityValue = Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", pid, warehouseId)
                Dim current As Decimal = 0D
                If quantityValue IsNot Nothing AndAlso Not Convert.IsDBNull(quantityValue) Then
                    current = Convert.ToDecimal(quantityValue)
                End If
                inventoryService.UpsertInventory(pid, warehouseId, current + rQty, 0D)
            Next
            
            UpdateReceiptStatus(invoiceId)
            logService.LogActivity(createdBy, "UpdateWarehouseReceipt", "WarehouseReceipt", receiptId, "ویرایش رسید انبار " & receiptNum, SessionContext.CurrentIP)
        End Sub

    End Class
End Namespace
