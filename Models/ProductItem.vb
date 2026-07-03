Option Strict Off
Option Explicit On

Imports System
Namespace Sys_Hes_Anb.Models
    Public Class ProductItem
        Public Property ProductID As Integer
        Public Property ProductCode As String
        Public Property ProductName As String
        Public Property Unit As String
        Public Property DefaultPrice As Decimal
        Public Property Category As String
        Public Property IsActive As Boolean
    End Class
End Namespace
