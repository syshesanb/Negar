Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Business.PersianDateHelper

Namespace Sys_Hes_Anb.Forms
    Partial Class F1KharidForoshFormBase
        Inherits Form

        Private ReadOnly _catalogService As New CatalogService()
        Private _lineTotal As Decimal

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Sub SetupInvoice(partyLabel As String, titleText As String)
            Me.Text = titleText
            lblParty.Text = partyLabel
        End Sub

        Protected Overridable ReadOnly Property InvoicePrefix As String
            Get
                Return "INV-"
            End Get
        End Property

        Protected Overridable Function SaveInvoice(lines As List(Of Tuple(Of Integer, Decimal, Decimal))) As Integer
            Throw New NotImplementedException()
        End Function

        Private Sub F1KharidForoshFormBase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvLines IsNot Nothing Then Me.dgvLines.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If
            LoadLookups()
            If String.IsNullOrWhiteSpace(txtInvoiceNumber.Text) Then
                txtInvoiceNumber.Text = InvoicePrefix & DateTime.Now.ToString("yyyyMMddHHmmss")
            End If
            dtpInvoiceDate.Value = DateTime.Now
            UpdatePersianLabel(lblInvoiceDatePersian, dtpInvoiceDate)
            RefreshTotal()
        End Sub

        Private Sub DtpInvoiceDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpInvoiceDate.ValueChanged
            UpdatePersianLabel(lblInvoiceDatePersian, dtpInvoiceDate)
        End Sub

        Protected Overridable Sub LoadLookups()
            cmbWarehouse.DataSource = _catalogService.GetWarehouses()
            cmbWarehouse.DisplayMember = "WarehouseName"
            cmbWarehouse.ValueMember = "WarehouseID"

            cmbProduct.DataSource = _catalogService.GetProducts()
            cmbProduct.DisplayMember = "ProductName"
            cmbProduct.ValueMember = "ProductID"
        End Sub

        Private Sub BtnAddLine_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
            Dim quantity As Decimal
            Dim unitPrice As Decimal

            If Not Decimal.TryParse(txtQuantity.Text, quantity) Then
                MessageBox.Show("مقدار معتبر نیست.")
                Return
            End If

            If Not Decimal.TryParse(txtUnitPrice.Text, unitPrice) Then
                MessageBox.Show("فی معتبر نیست.")
                Return
            End If

            Dim productId = Convert.ToInt32(cmbProduct.SelectedValue)
            Dim productName = Convert.ToString(cmbProduct.Text)
            Dim lineTotal = quantity * unitPrice
            Dim table = TryCast(dgvLines.DataSource, DataTable)
            If table Is Nothing Then
                table = CreateLinesTable()
                dgvLines.DataSource = table
            End If

            table.Rows.Add(productId, productName, quantity, unitPrice, lineTotal)
            txtQuantity.Clear()
            txtUnitPrice.Clear()
            RefreshTotal()
        End Sub

        Private Sub BtnRemoveLine_Click(sender As Object, e As EventArgs) Handles btnRemoveLine.Click
            If dgvLines.CurrentRow Is Nothing Then
                Return
            End If

            Dim table = TryCast(dgvLines.DataSource, DataTable)
            If table IsNot Nothing AndAlso dgvLines.CurrentRow.DataBoundItem IsNot Nothing Then
                Dim view = TryCast(dgvLines.CurrentRow.DataBoundItem, DataRowView)
                If view IsNot Nothing Then
                    view.Row.Delete()
                End If
            Else
                dgvLines.Rows.Remove(dgvLines.CurrentRow)
            End If

            RefreshTotal()
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                dgvLines.EndEdit()
                Dim table = TryCast(dgvLines.DataSource, DataTable)
                If table Is Nothing OrElse table.Rows.Count = 0 Then
                    MessageBox.Show("حداقل یک ردیف فاکتور وارد کنید.")
                    Return
                End If

                Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()
                For Each row As DataRow In table.Rows
                    lines.Add(Tuple.Create(Convert.ToInt32(row("ProductID")), Convert.ToDecimal(row("Quantity")), Convert.ToDecimal(row("UnitPrice"))))
                Next

                Dim newId = SaveInvoice(lines)
                MessageBox.Show("فاکتور با موفقیت ثبت شد. شناسه: " & newId.ToString())
                table.Rows.Clear()
                RefreshTotal()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function CreateLinesTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("ProductID", GetType(Integer))
            table.Columns.Add("ProductName", GetType(String))
            table.Columns.Add("Quantity", GetType(Decimal))
            table.Columns.Add("UnitPrice", GetType(Decimal))
            table.Columns.Add("TotalPrice", GetType(Decimal))
            Return table
        End Function

        Protected Sub RefreshTotal()
            _lineTotal = 0D
            Dim table = TryCast(dgvLines.DataSource, DataTable)
            If table IsNot Nothing Then
                For Each row As DataRow In table.Rows
                    If Not row.IsNull("TotalPrice") Then
                        _lineTotal += Convert.ToDecimal(row("TotalPrice"))
                    End If
                Next
            End If
            lblTotal.Text = "جمع کل: " & _lineTotal.ToString("N2")
        End Sub
    End Class
End Namespace
