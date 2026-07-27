Option Strict Off
Option Explicit On

Imports System
Namespace Negar.Models
    Public Class UserAccount
        Public Property UserID As Integer
        Public Property Username As String
        Public Property PasswordHash As String
        Public Property UserType As String
        Public Property CreatedBy As Integer?
        Public Property CreatedDate As DateTime
        Public Property IsActive As Boolean
        Public Property FullName As String
        Public Property MaxCompaniesAllowed As Integer = 0
        Public Property MaxFiscalYearsPerCompany As Integer = 0
    End Class
End Namespace
