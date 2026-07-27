Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniForooshForm
        Private ReadOnly catalogService As New CatalogService()
        Private ReadOnly invoiceService As New InvoiceService()
        Private ReadOnly defaultWarehouseId As Integer = 1

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniForooshForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            txtInvoiceDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            GenerateNextInvoiceNumber()
            cmbPaymentType.SelectedIndex = 0 ' کارتخوان (POS)
            txtBarcodeScan.Focus()
        End Sub

        Private Sub GenerateNextInvoiceNumber()
            Try
                Dim val = Sql.ExecuteScalar("SELECT MAX(InvoiceID) FROM SalesInvoices")
                Dim nextId As Integer = 1
                If val IsNot Nothing AndAlso Not Convert.IsDBNull(val) Then
                    nextId = Convert.ToInt32(val) + 1
                End If
                txtInvoiceNo.Text = "INV-" & nextId.ToString("D5")
            Catch
                txtInvoiceNo.Text = "INV-00001"
            End Try
        End Sub

        Private Sub txtBarcodeScan_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarcodeScan.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                AddScannedProduct()
            End If
        End Sub

        Private Sub btnAddProduct_Click(sender As Object, e As EventArgs) Handles btnAddProduct.Click
            AddScannedProduct()
        End Sub

        Private Sub AddScannedProduct()
            Dim term = txtBarcodeScan.Text.Trim()
            If String.IsNullOrEmpty(term) Then Return

            Dim compId = SessionContext.CurrentCompanyID
            Dim dt As DataTable
            If compId.HasValue Then
                dt = Sql.ExecuteTable("SELECT ProductID, Code, Name, PrimaryUnit, SalesPrice, Barcode FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND (Barcode = ? OR Code = ? OR Name LIKE ?)", compId.Value, term, term, "%" & term & "%")
            Else
                dt = Sql.ExecuteTable("SELECT ProductID, Code, Name, PrimaryUnit, SalesPrice, Barcode FROM Products WHERE Barcode = ? OR Code = ? OR Name LIKE ?", term, term, "%" & term & "%")
            End If
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("کالایی با این مشخصات یا بارکد یافت نشد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBarcodeScan.SelectAll()
                Return
            End If

            Dim row = dt.Rows(0)
            Dim productId = Convert.ToInt32(row("ProductID"))
            Dim code = Convert.ToString(row("Code"))
            Dim name = Convert.ToString(row("Name"))
            Dim unit = If(row.IsNull("PrimaryUnit"), "عدد", Convert.ToString(row("PrimaryUnit")))
            Dim salesPrice = If(row.IsNull("SalesPrice"), 0D, Convert.ToDecimal(row("SalesPrice")))

            ' بررسی آیا کالا قبلاً در سبد موجود است یا خیر
            Dim existingRow As DataGridViewRow = Nothing
            For Each gRow As DataGridViewRow In dgvCart.Rows
                If Convert.ToInt32(gRow.Cells("colProductID").Value) = productId Then
                    existingRow = gRow
                    Exit For
                End If
            Next

            If existingRow IsNot Nothing Then
                Dim currentQty = Convert.ToDecimal(existingRow.Cells("colQuantity").Value)
                existingRow.Cells("colQuantity").Value = currentQty + 1
                RecalculateRowTotal(existingRow)
            Else
                Dim rowIndex = dgvCart.Rows.Add(productId, code, name, unit, 1, salesPrice, salesPrice)
                RecalculateRowTotal(dgvCart.Rows(rowIndex))
            End If

            RecalculateCartTotal()
            txtBarcodeScan.Clear()
            txtBarcodeScan.Focus()
        End Sub

        Private Sub RecalculateRowTotal(row As DataGridViewRow)
            Dim qty As Decimal = 0D
            Dim price As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
            Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value), price)

            Dim total = qty * price
            row.Cells("colTotalPrice").Value = total.ToString("N0")
        End Sub

        Private Sub RecalculateCartTotal()
            Dim grandTotal As Decimal = 0D
            For Each row As DataGridViewRow In dgvCart.Rows
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value), price)
                grandTotal += (qty * price)
            Next

            lblTotalAmountValue.Text = grandTotal.ToString("N0") & " ریال"
        End Sub

        Private Sub dgvCart_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellValueChanged
            If e.RowIndex >= 0 AndAlso (e.ColumnIndex = dgvCart.Columns("colQuantity").Index OrElse e.ColumnIndex = dgvCart.Columns("colUnitPrice").Index) Then
                RecalculateRowTotal(dgvCart.Rows(e.RowIndex))
                RecalculateCartTotal()
            End If
        End Sub

        Private Sub btnSaveAndPrint_Click(sender As Object, e As EventArgs) Handles btnSaveAndPrint.Click
            SaveInvoice()
        End Sub

        Private Sub SaveInvoice()
            If dgvCart.Rows.Count = 0 Then
                MessageBox.Show("سبد خرید خالی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)
            Dim customerName = If(String.IsNullOrWhiteSpace(txtCustomerName.Text), "مشتری نقدی", txtCustomerName.Text.Trim())

            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()
            For Each gRow As DataGridViewRow In dgvCart.Rows
                Dim pId = Convert.ToInt32(gRow.Cells("colProductID").Value)
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(gRow.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(gRow.Cells("colUnitPrice").Value), price)
                lines.Add(New Tuple(Of Integer, Decimal, Decimal)(pId, qty, price))
            Next

            Try
                Dim invoiceId = invoiceService.SaveSalesInvoice(txtInvoiceNo.Text, DateTime.Now, customerName, defaultWarehouseId, currentUserId, lines)
                MessageBox.Show("فاکتور فروش با موفقیت ثبت شد." & Environment.NewLine & "شماره فاکتور: " & txtInvoiceNo.Text, "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ClearInvoice()
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnNewInvoice_Click(sender As Object, e As EventArgs) Handles btnNewInvoice.Click
            ClearInvoice()
        End Sub

        Private Sub ClearInvoice()
            dgvCart.Rows.Clear()
            GenerateNextInvoiceNumber()
            RecalculateCartTotal()
            txtCustomerName.Text = "مشتری نقدی"
            txtBarcodeScan.Clear()
            txtBarcodeScan.Focus()
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            If keyData = Keys.F2 Then
                SaveInvoice()
                Return True
            ElseIf keyData = Keys.F3 Then
                ClearInvoice()
                Return True
            End If
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function
    End Class
End Namespace
