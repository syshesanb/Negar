Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Models

Namespace Sys_Hes_Anb.Business
    Public Module SessionContext
        Public Property CurrentUser As UserAccount
        Public Property CurrentPermissions As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Public Property CurrentTheme As String = "Light"
        Public Property CurrentIP As String = "127.0.0.1"
        Public Property CurrentCompanyID As Integer? = Nothing
        Public Property CurrentCompanyName As String = "-"
        Public Property CurrentFiscalYearID As Integer? = Nothing
        Public Property CurrentFiscalYearName As String = "-"

        Public Function HasPermission(key As String) As Boolean
            If CurrentUser Is Nothing Then Return False
            If String.Equals(CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then Return True
            Return CurrentPermissions.Contains(key)
        End Function
    End Module
End Namespace
