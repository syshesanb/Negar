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
            Dim dt = Sql.ExecuteTable(query)

            If Not dt.Columns.Contains("TotalValue") Then dt.Columns.Add("TotalValue", GetType(Decimal))

            For Each row As DataRow In dt.Rows
                Dim pId = Convert.ToInt32(row("ProductID"))
                Dim kardexDt = GetKardex(pId, warehouseId)
                If kardexDt IsNot Nothing AndAlso kardexDt.Rows.Count > 0 Then
                    Dim lastRow = kardexDt.Rows(kardexDt.Rows.Count - 1)
                    Dim qty = Convert.ToDecimal(If(lastRow.IsNull("Balance"), 0, lastRow("Balance")))
                    Dim balCost = Convert.ToDecimal(If(lastRow.IsNull("BalanceCost"), 0, lastRow("BalanceCost")))

                    row("Quantity") = qty
                    row("TotalValue") = balCost
                    If qty > 0 Then
                        row("AverageCost") = Math.Round(balCost / qty, 0)
                    Else
                        row("AverageCost") = 0D
                    End If
                Else
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0, row("Quantity")))
                    Dim avg = Convert.ToDecimal(If(row.IsNull("AverageCost"), 0, row("AverageCost")))
                    row("TotalValue") = qty * avg
                End If
            Next

            Return dt
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
                    "t.CostIn AS CostIn, " &
                    "t.QtyOut AS QuantityOut, " &
                    "t.TxDesc AS Description " &
                    "FROM (" &
                    "  SELECT pi.InvoiceDate AS TxDate, pi.WarehouseID AS WarehouseID, " &
                    "  'فاکتور خرید (' || pi.InvoiceNumber || ')' AS TxType, " &
                    "  pd.Quantity AS QtyIn, (pd.Quantity * pd.UnitPrice) AS CostIn, " &
                    "  0 AS QtyOut, " &
                    "  COALESCE(pi.VendorName, '') || CASE WHEN pi.Description IS NOT NULL AND pi.Description <> '' THEN ' - ' || pi.Description ELSE '' END AS TxDesc " &
                    "  FROM PurchaseInvoiceDetails pd JOIN PurchaseInvoices pi ON pd.InvoiceID = pi.InvoiceID " &
                    "  WHERE pd.ProductID = " & productId & whPurCondition & compPurCondition &
                    "  UNION ALL " &
                    "  SELECT si.InvoiceDate AS TxDate, si.WarehouseID AS WarehouseID, " &
                    "  'فاکتور فروش (' || si.InvoiceNumber || ')' AS TxType, " &
                    "  0 AS QtyIn, 0 AS CostIn, " &
                    "  sd.Quantity AS QtyOut, " &
                    "  COALESCE(si.CustomerName, '') || CASE WHEN si.Description IS NOT NULL AND si.Description <> '' THEN ' - ' || si.Description ELSE '' END AS TxDesc " &
                    "  FROM SalesInvoiceDetails sd JOIN SalesInvoices si ON sd.InvoiceID = si.InvoiceID " &
                    "  WHERE sd.ProductID = " & productId & whSalCondition & compSalCondition &
                    ") t " &
                    "LEFT JOIN Warehouses w ON w.WarehouseID = t.WarehouseID "

                query &= " ORDER BY t.TxDate ASC"

                Dim dtRaw = Sql.ExecuteTable(query)

                If Not dtRaw.Columns.Contains("CostOut") Then dtRaw.Columns.Add("CostOut", GetType(Decimal))
                If Not dtRaw.Columns.Contains("Balance") Then dtRaw.Columns.Add("Balance", GetType(Decimal))
                If Not dtRaw.Columns.Contains("UnitPrice") Then dtRaw.Columns.Add("UnitPrice", GetType(Decimal))
                If Not dtRaw.Columns.Contains("BalanceCost") Then dtRaw.Columns.Add("BalanceCost", GetType(Decimal))

                Dim dtFiltered As DataTable = dtRaw.Clone()

                Dim cleanFrom = If(Not String.IsNullOrEmpty(fromDate), fromDate.Trim(), "")
                Dim cleanTo = If(Not String.IsNullOrEmpty(toDate), toDate.Trim(), "")

                Dim balance As Decimal = 0D
                Dim balanceCost As Decimal = 0D

                For Each row As DataRow In dtRaw.Rows
                    Dim pDate As String = ""
                    If Not row.IsNull("TransactionDate") Then
                        Try
                            Dim rawStr = Convert.ToString(row("TransactionDate"))
                            Dim d As DateTime
                            If DateTime.TryParse(rawStr, d) Then
                                pDate = PersianDateHelper.ToPersian(d)
                            Else
                                pDate = rawStr
                            End If
                        Catch
                            pDate = Convert.ToString(row("TransactionDate"))
                        End Try
                    End If

                    Dim pDate10 = If(pDate.Length >= 10, pDate.Substring(0, 10), pDate)

                    Dim qIn = Convert.ToDecimal(If(row.IsNull("QuantityIn"), 0, row("QuantityIn")))
                    Dim cIn = Convert.ToDecimal(If(row.IsNull("CostIn"), 0, row("CostIn")))
                    Dim qOut = Convert.ToDecimal(If(row.IsNull("QuantityOut"), 0, row("QuantityOut")))

                    Dim currentAvgCost As Decimal = 0D
                    If balance > 0 Then
                        currentAvgCost = balanceCost / balance
                    End If

                    Dim cOut As Decimal = 0D
                    If qOut > 0 Then
                        cOut = Math.Round(qOut * currentAvgCost, 0)
                    End If
                    row("CostOut") = cOut

                    balance += qIn - qOut
                    balanceCost += cIn - cOut
                    If balance <= 0 OrElse balanceCost < 0 Then
                        If balance <= 0 Then balanceCost = 0D
                    End If

                    row("Balance") = balance
                    row("BalanceCost") = Math.Round(balanceCost, 0)
                    If balance > 0 Then
                        row("UnitPrice") = Math.Round(balanceCost / balance, 0)
                    Else
                        row("UnitPrice") = 0D
                    End If

                    Dim keepRow As Boolean = True
                    If cleanFrom.Length = 10 AndAlso pDate10 < cleanFrom Then
                        keepRow = False
                    End If
                    If cleanTo.Length = 10 AndAlso pDate10 > cleanTo Then
                        keepRow = False
                    End If

                    If keepRow Then
                        Dim nr = dtFiltered.NewRow()
                        nr.ItemArray = row.ItemArray
                        dtFiltered.Rows.Add(nr)
                    End If
                Next

                Return dtFiltered
            Catch ex As Exception
                Dim dt As New DataTable()
                dt.Columns.Add("TransactionDate", GetType(String))
                dt.Columns.Add("WarehouseName", GetType(String))
                dt.Columns.Add("TransactionType", GetType(String))
                dt.Columns.Add("QuantityIn", GetType(Decimal))
                dt.Columns.Add("CostIn", GetType(Decimal))
                dt.Columns.Add("QuantityOut", GetType(Decimal))
                dt.Columns.Add("CostOut", GetType(Decimal))
                dt.Columns.Add("Balance", GetType(Decimal))
                dt.Columns.Add("UnitPrice", GetType(Decimal))
                dt.Columns.Add("BalanceCost", GetType(Decimal))
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
