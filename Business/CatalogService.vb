Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class CatalogService
        Public Function GetProducts() As DataTable
            Return Sql.ExecuteTable("SELECT ProductID, ProductCode, ProductName, Unit, DefaultPrice, Category, IsActive FROM Products ORDER BY ProductName")
        End Function

        Public Function SaveProduct(productId As Integer?, code As String, name As String, unit As String, defaultPrice As Decimal, category As String, isActive As Boolean) As Integer
            If productId.HasValue AndAlso productId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Products SET ProductCode = ?, ProductName = ?, Unit = ?, DefaultPrice = ?, Category = ?, IsActive = ? WHERE ProductID = ?",
                                    code, name, unit, defaultPrice, category, isActive, productId.Value)
                Return productId.Value
            End If

            Return Sql.ExecuteIdentity("INSERT INTO Products (ProductCode, ProductName, Unit, DefaultPrice, Category, IsActive) VALUES (?, ?, ?, ?, ?, ?)",
                                       code, name, unit, defaultPrice, category, isActive)
        End Function

        Public Sub DeleteProduct(productId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM Products WHERE ProductID = ?", productId)
        End Sub

        Public Function GetWarehouses() As DataTable
            Return Sql.ExecuteTable("SELECT WarehouseID, WarehouseName, Location, IsActive FROM Warehouses ORDER BY WarehouseName")
        End Function

        Public Function SaveWarehouse(warehouseId As Integer?, warehouseName As String, location As String, isActive As Boolean) As Integer
            If warehouseId.HasValue AndAlso warehouseId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Warehouses SET WarehouseName = ?, Location = ?, IsActive = ? WHERE WarehouseID = ?",
                                    warehouseName, location, isActive, warehouseId.Value)
                Return warehouseId.Value
            End If

            Return Sql.ExecuteIdentity("INSERT INTO Warehouses (WarehouseName, Location, IsActive) VALUES (?, ?, ?)",
                                       warehouseName, location, isActive)
        End Function

        Public Sub DeleteWarehouse(warehouseId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM Warehouses WHERE WarehouseID = ?", warehouseId)
        End Sub
    End Class
End Namespace
