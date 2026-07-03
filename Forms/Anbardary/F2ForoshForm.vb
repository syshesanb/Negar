Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class F2ForoshForm
        Inherits F1KharidForoshFormBase

        Private ReadOnly invoiceService As New InvoiceService()

        Public Sub New()
            MyBase.New()
            InitializeComponent()
            SetupInvoice("مشتری", "ثبت فاکتور فروش")
        End Sub

        Protected Overrides ReadOnly Property InvoicePrefix As String
            Get
                Return "SINV-"
            End Get
        End Property

        Protected Overrides Function SaveInvoice(lines As List(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Dim createdBy = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)
            Dim warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue)
            Return invoiceService.SaveSalesInvoice(txtInvoiceNumber.Text.Trim(), dtpInvoiceDate.Value, txtPartyName.Text.Trim(), warehouseId, createdBy, lines)
        End Function
    End Class
End Namespace
