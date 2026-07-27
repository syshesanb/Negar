Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class InventoryService
        Public Function GetInventory(Optional warehouseId As Integer? = Nothing) As DataTable
            Dim compId = SessionContext.CurrentCompanyID

            Dim query As String =
                "SELECT p.ProductID, p.ProductCode, p.ProductName, " &
                "COALESCE(w.WarehouseName, 'فروشگاه') AS WarehouseName, " &
                "COALESCE((SELECT SUM(pd.Quantity) FROM PurchaseInvoiceDetails pd JOIN PurchaseInvoices pi ON pd.InvoiceID = pi.InvoiceID WHERE pd.ProductID = p.ProductID " &
                If(warehouseId.HasValue AndAlso warehouseId.Value > 0, "AND pi.WarehouseID = " & warehouseId.Value & " ", "") &
                If(compId.HasValue, "AND pi.CompanyID = " & compId.Value & " ", "") & "), 0) AS TotalInput, " &
                "COALESCE((SELECT SUM(sd.Quantity) FROM SalesInvoiceDetails sd JOIN SalesInvoices si ON sd.InvoiceID = si.InvoiceID WHERE sd.ProductID = p.ProductID " &
                If(warehouseId.HasValue AndAlso warehouseId.Value > 0, "AND si.WarehouseID = " & warehouseId.Value & " ", "") &
                If(compId.HasValue, "AND si.CompanyID = " & compId.Value & " ", "") & "), 0) AS TotalOutput, " &
                "COALESCE(i.Quantity, " &
                "(COALESCE((SELECT SUM(pd.Quantity) FROM PurchaseInvoiceDetails pd JOIN PurchaseInvoices pi ON pd.InvoiceID = pi.InvoiceID WHERE pd.ProductID = p.ProductID " &
                If(warehouseId.HasValue AndAlso warehouseId.Value > 0, "AND pi.WarehouseID = " & warehouseId.Value & " ", "") &
                If(compId.HasValue, "AND pi.CompanyID = " & compId.Value & " ", "") & "), 0) - " &
                "COALESCE((SELECT SUM(sd.Quantity) FROM SalesInvoiceDetails sd JOIN SalesInvoices si ON sd.InvoiceID = si.InvoiceID WHERE sd.ProductID = p.ProductID " &
                If(warehouseId.HasValue AndAlso warehouseId.Value > 0, "AND si.WarehouseID = " & warehouseId.Value & " ", "") &
                If(compId.HasValue, "AND si.CompanyID = " & compId.Value & " ", "") & "), 0)) " &
                ") AS Quantity, " &
                "COALESCE(i.AverageCost, p.PurchasePrice, 0) AS AverageCost, " &
                "COALESCE(i.LastUpdate, strftime('%Y-%m-%d %H:%M:%S', 'now', 'localtime')) AS LastUpdate " &
                "FROM Products p " &
                "LEFT JOIN Inventory i ON i.ProductID = p.ProductID " &
                "LEFT JOIN Warehouses w ON w.WarehouseID = COALESCE(i.WarehouseID, p.DefaultWarehouseID, 1) "

            Dim conditions As New System.Collections.Generic.List(Of String)()
            If compId.HasValue Then
                conditions.Add("p.CompanyID = " & compId.Value)
            End If
            If warehouseId.HasValue AndAlso warehouseId.Value > 0 Then
                conditions.Add("(i.WarehouseID = " & warehouseId.Value & " OR (i.WarehouseID IS NULL AND (p.DefaultWarehouseID = " & warehouseId.Value & " OR w.WarehouseID = " & warehouseId.Value & ")))")
            End If

            If conditions.Count > 0 Then
                query &= " WHERE " & String.Join(" AND ", conditions.ToArray())
            End If

            query &= " ORDER BY p.ProductCode, p.ProductName"
            Return Sql.ExecuteTable(query)
        End Function

        Public Function GetKardex(productId As Integer, Optional warehouseId As Integer? = Nothing,
                                  Optional fromDate As String = Nothing, Optional toDate As String = Nothing) As DataTable
            Try
                Dim compId = SessionContext.CurrentCompanyID

                Dim whPurCondition As String = ""
                Dim whSalCondition As String = ""

                If warehouseId.HasValue AndAlso warehouseId.Value > 0 Then
                    whPurCondition = " AND pi.WarehouseID = " & warehouseId.Value & " "
                    whSalCondition = " AND si.WarehouseID = " & warehouseId.Value & " "
                End If

                Dim compPurCondition As String = If(compId.HasValue, " AND pi.CompanyID = " & compId.Value & " ", "")
                Dim compSalCondition As String = If(compId.HasValue, " AND si.CompanyID = " & compId.Value & " ", "")

                Dim query As String =
                    "SELECT t.TxDate AS TransactionDate, " &
                    "COALESCE(w.WarehouseName, 'فروشگاه') AS WarehouseName, " &
                    "t.TxType AS TransactionType, " &
                    "t.QtyIn AS QuantityIn, " &
                    "t.QtyOut AS QuantityOut, " &
                    "t.TxDesc AS Description " &
                    "FROM (" &
                    "  SELECT pi.InvoiceDate AS TxDate, pi.WarehouseID AS WarehouseID, " &
                    "  'فاکتور خرید (' || pi.InvoiceNumber || ')' AS TxType, " &
                    "  pd.Quantity AS QtyIn, 0 AS QtyOut, " &
                    "  COALESCE(pi.VendorName, '') || CASE WHEN pi.Description IS NOT NULL AND pi.Description <> '' THEN ' - ' || pi.Description ELSE '' END AS TxDesc " &
                    "  FROM PurchaseInvoiceDetails pd JOIN PurchaseInvoices pi ON pd.InvoiceID = pi.InvoiceID " &
                    "  WHERE pd.ProductID = " & productId & whPurCondition & compPurCondition &
                    "  UNION ALL " &
                    "  SELECT si.InvoiceDate AS TxDate, si.WarehouseID AS WarehouseID, " &
                    "  'فاکتور فروش (' || si.InvoiceNumber || ')' AS TxType, " &
                    "  0 AS QtyIn, sd.Quantity AS QtyOut, " &
                    "  COALESCE(si.CustomerName, '') || CASE WHEN si.Description IS NOT NULL AND si.Description <> '' THEN ' - ' || si.Description ELSE '' END AS TxDesc " &
                    "  FROM SalesInvoiceDetails sd JOIN SalesInvoices si ON sd.InvoiceID = si.InvoiceID " &
                    "  WHERE sd.ProductID = " & productId & whSalCondition & compSalCondition &
                    ") t " &
                    "LEFT JOIN Warehouses w ON w.WarehouseID = t.WarehouseID "

                Dim dateConditions As New System.Collections.Generic.List(Of String)()
                If Not String.IsNullOrEmpty(fromDate) AndAlso fromDate.Trim().Length = 10 Then
                    dateConditions.Add("t.TxDate >= '" & fromDate.Trim().Replace("'", "''") & "'")
                End If
                If Not String.IsNullOrEmpty(toDate) AndAlso toDate.Trim().Length = 10 Then
                    dateConditions.Add("t.TxDate <= '" & toDate.Trim().Replace("'", "''") & " 23:59:59'")
                End If

                If dateConditions.Count > 0 Then
                    query &= " WHERE " & String.Join(" AND ", dateConditions.ToArray())
                End If

                query &= " ORDER BY t.TxDate ASC"

                Dim dt = Sql.ExecuteTable(query)

                If Not dt.Columns.Contains("Balance") Then dt.Columns.Add("Balance", GetType(Decimal))
                Dim balance As Decimal = 0D
                For Each row As DataRow In dt.Rows
                    Dim qIn = Convert.ToDecimal(If(row.IsNull("QuantityIn"), 0, row("QuantityIn")))
                    Dim qOut = Convert.ToDecimal(If(row.IsNull("QuantityOut"), 0, row("QuantityOut")))
                    balance += qIn - qOut
                    row("Balance") = balance
                Next
                Return dt
            Catch ex As Exception
                Dim dt As New DataTable()
                dt.Columns.Add("TransactionDate", GetType(String))
                dt.Columns.Add("WarehouseName", GetType(String))
                dt.Columns.Add("TransactionType", GetType(String))
                dt.Columns.Add("QuantityIn", GetType(Decimal))
                dt.Columns.Add("QuantityOut", GetType(Decimal))
                dt.Columns.Add("Balance", GetType(Decimal))
                dt.Columns.Add("Description", GetType(String))
                Return dt
            End Try
        End Function

        Public Sub UpsertInventory(productId As Integer, warehouseId As Integer, quantity As Decimal, averageCost As Decimal)
            Dim exists = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", productId, warehouseId), 0))
            If exists > 0 Then
                Sql.ExecuteNonQuery("UPDATE Inventory SET Quantity = ?, AverageCost = ?, LastUpdate = ? WHERE ProductID = ? AND WarehouseID = ?",
                                    quantity, averageCost, DateTime.Now, productId, warehouseId)
            Else
                Sql.ExecuteNonQuery("INSERT INTO Inventory (ProductID, WarehouseID, Quantity, AverageCost, LastUpdate) VALUES (?, ?, ?, ?, ?)",
                                    productId, warehouseId, quantity, averageCost, DateTime.Now)
            End If
        End Sub
    End Class
End Namespace
