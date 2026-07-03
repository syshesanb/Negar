Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
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
