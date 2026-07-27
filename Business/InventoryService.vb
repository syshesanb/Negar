Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class InventoryService
        Public Function GetInventory(Optional warehouseId As Integer? = Nothing) As DataTable
            Dim query As String =
                "SELECT i.InventoryID, p.ProductCode, p.ProductName, w.WarehouseName, i.Quantity, i.AverageCost, i.LastUpdate " &
                "FROM (Inventory AS i INNER JOIN Products AS p ON i.ProductID = p.ProductID) " &
                "INNER JOIN Warehouses AS w ON i.WarehouseID = w.WarehouseID"

            If warehouseId.HasValue Then
                query &= " WHERE i.WarehouseID = ? ORDER BY p.ProductName"
                Return Sql.ExecuteTable(query, warehouseId.Value)
            End If

            Return Sql.ExecuteTable(query & " ORDER BY p.ProductName")
        End Function

        Public Function GetKardex(productId As Integer, Optional warehouseId As Integer? = Nothing,
                                  Optional fromDate As String = Nothing, Optional toDate As String = Nothing) As DataTable
            Try
                Dim conditions As New System.Collections.Generic.List(Of String)()
                Dim parms As New System.Collections.Generic.List(Of Object)()

                conditions.Add("il.ProductID = ?")
                parms.Add(productId)

                If warehouseId.HasValue Then
                    conditions.Add("il.WarehouseID = ?")
                    parms.Add(warehouseId.Value)
                End If
                If Not String.IsNullOrEmpty(fromDate) Then
                    conditions.Add("DATE(il.TransactionDate) >= ?")
                    parms.Add(fromDate)
                End If
                If Not String.IsNullOrEmpty(toDate) Then
                    conditions.Add("DATE(il.TransactionDate) <= ?")
                    parms.Add(toDate)
                End If

                Dim whereClause = If(conditions.Count > 0, " WHERE " & String.Join(" AND ", conditions.ToArray()), "")
                Dim query =
                    "SELECT il.LedgerID, il.TransactionDate, " &
                    "COALESCE(w.WarehouseName, '---') AS WarehouseName, " &
                    "COALESCE(il.TransactionType, '') AS TransactionType, " &
                    "CASE WHEN il.Quantity > 0 THEN il.Quantity ELSE 0 END AS QuantityIn, " &
                    "CASE WHEN il.Quantity < 0 THEN ABS(il.Quantity) ELSE 0 END AS QuantityOut, " &
                    "COALESCE(il.Description, '') AS Description " &
                    "FROM InventoryLedger il " &
                    "LEFT JOIN Warehouses w ON w.WarehouseID = il.WarehouseID" &
                    whereClause &
                    " ORDER BY il.TransactionDate, il.LedgerID"

                Dim dt = Sql.ExecuteTable(query, parms.ToArray())

                ' محاسبه موجودی تجمعی
                If Not dt.Columns.Contains("Balance") Then dt.Columns.Add("Balance", GetType(Decimal))
                Dim balance As Decimal = 0
                For Each row As DataRow In dt.Rows
                    Dim qIn = Convert.ToDecimal(row("QuantityIn"))
                    Dim qOut = Convert.ToDecimal(row("QuantityOut"))
                    balance += qIn - qOut
                    row("Balance") = balance
                Next
                Return dt
            Catch ex As Exception
                Dim dt As New DataTable()
                dt.Columns.Add("LedgerID", GetType(Integer))
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
