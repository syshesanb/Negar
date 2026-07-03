Option Strict Off
Option Explicit On

Imports System
Namespace Sys_Hes_Anb.Models
    Public Class AccountingEntry
        Public Property EntryID As Integer
        Public Property EntryDate As DateTime
        Public Property Description As String
        Public Property ReferenceNumber As String
        Public Property CreatedBy As Integer
    End Class
End Namespace
