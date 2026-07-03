Option Strict Off
Option Explicit On

Namespace Sys_Hes_Anb.Business
    Public Class AccountingEntryLine
        Public Property AccountID As Integer
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
        Public Property LineNumber As Integer
        Public Property ShenavarID As Integer
        Public Property SharhRadif As String
        Public Property TransactionNumber As String
        Public Property TransactionDate As String

        Public Sub New(accountID As Integer, debit As Decimal, credit As Decimal,
                       lineNumber As Integer, shenavarID As Integer,
                       sharhRadif As String, transactionNumber As String, transactionDate As String)
            Me.AccountID = accountID
            Me.DebitAmount = debit
            Me.CreditAmount = credit
            Me.LineNumber = lineNumber
            Me.ShenavarID = shenavarID
            Me.SharhRadif = If(sharhRadif, "")
            Me.TransactionNumber = If(transactionNumber, "")
            Me.TransactionDate = If(transactionDate, "")
        End Sub
    End Class
End Namespace
