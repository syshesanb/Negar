Option Strict Off
Option Explicit On

Imports System
Namespace Negar.Models
    Public Class AccountingAccount
        Public Property AccountID As Integer
        Public Property AccountCode As String
        Public Property AccountName As String
        Public Property AccountType As String
        Public Property ParentAccountID As Integer?
        Public Property IsActive As Boolean
    End Class
End Namespace
