Option Strict Off
Option Explicit On

Imports System
Namespace Negar.Models
    Public Class InventoryRecord
        Public Property InventoryID As Integer
        Public Property ProductID As Integer
        Public Property WarehouseID As Integer
        Public Property Quantity As Decimal
        Public Property AverageCost As Decimal
        Public Property LastUpdate As DateTime
    End Class
End Namespace
