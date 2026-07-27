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
    Public Class AnbarMiniKharidForm
        Private ReadOnly invoiceService As New InvoiceService()
        Private ReadOnly defaultWarehouseId As Integer = 1

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniKharidForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            txtInvoiceDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            GenerateNextInvoiceNumber()
        End Sub

        Private Sub GenerateNextInvoiceNumber()
            Try
                Dim val = Sql.ExecuteScalar("SELECT MAX(InvoiceID) FROM PurchaseInvoices")
                Dim nextId As Integer = 1
                If val IsNot Nothing AndAlso Not Convert.IsDBNull(val) Then
                    nextId = Convert.ToInt32(val) + 1
                End If
                txtInvoiceNo.Text = "PUR-" & nextId.ToString("D5")
            Catch
                txtInvoiceNo.Text = "PUR-00001"
            End Try
        End Sub

        Private Sub txtProductSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtProductSearch.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                AddProductToGrid()
            End If
        End Sub

        Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            AddProductToGrid()
        End Sub

        Private Sub AddProductToGrid()
            Dim term = txtProductSearch.Text.Trim()
            If String.IsNullOrEmpty(term) Then Return

            Dim compId = SessionContext.CurrentCompanyID
            Dim dt As DataTable
            If compId.HasValue Then
                dt = Sql.ExecuteTable("SELECT ProductID, Code, Name, PurchasePrice FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND (Barcode = ? OR Code = ? OR Name LIKE ?)", compId.Value, term, term, "%" & term & "%")
            Else
                dt = Sql.ExecuteTable("SELECT ProductID, Code, Name, PurchasePrice FROM Products WHERE Barcode = ? OR Code = ? OR Name LIKE ?", term, term, "%" & term & "%")
            End If
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("کالای مورد نظر یافت نشد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim row = dt.Rows(0)
            Dim productId = Convert.ToInt32(row("ProductID"))
            Dim code = Convert.ToString(row("Code"))
            Dim name = Convert.ToString(row("Name"))

            Dim unitPrice As Decimal = 0D
            If Not String.IsNullOrWhiteSpace(txtUnitPrice.Text) Then
                Decimal.TryParse(txtUnitPrice.Text.Replace(",", ""), unitPrice)
            ElseIf Not row.IsNull("PurchasePrice") Then
                unitPrice = Convert.ToDecimal(row("PurchasePrice"))
            End If

            Dim qty = numQuantity.Value
            Dim totalPrice = qty * unitPrice

            dgvItems.Rows.Add(productId, code, name, qty, unitPrice.ToString("N0"), totalPrice.ToString("N0"))
            RecalculateTotal()

            txtProductSearch.Clear()
            txtUnitPrice.Clear()
            numQuantity.Value = 1
            txtProductSearch.Focus()
        End Sub

        Private Sub RecalculateTotal()
            Dim grandTotal As Decimal = 0D
            For Each row As DataGridViewRow In dgvItems.Rows
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value).Replace(",", ""), price)
                grandTotal += (qty * price)
            Next

            lblTotalAmount.Text = grandTotal.ToString("N0") & " ریال"
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If dgvItems.Rows.Count = 0 Then
                MessageBox.Show("فاکتور خرید خالی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim vendorName = If(String.IsNullOrWhiteSpace(txtVendorName.Text), "فروشنده عمومی", txtVendorName.Text.Trim())
            Dim currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal, Decimal, Decimal))()
            For Each gRow As DataGridViewRow In dgvItems.Rows
                Dim pId = Convert.ToInt32(gRow.Cells("colProductID").Value)
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(gRow.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(gRow.Cells("colUnitPrice").Value).Replace(",", ""), price)
                lines.Add(New Tuple(Of Integer, Decimal, Decimal, Decimal, Decimal)(pId, qty, price, 0D, 0D))
            Next

            Try
                Dim invoiceId = invoiceService.SavePurchaseInvoice(txtInvoiceNo.Text, DateTime.Now, vendorName, defaultWarehouseId, currentUserId, lines, "فاکتور خرید", 0D, "نقدی", "فاکتور خرید نسخه مینی")
                MessageBox.Show("فاکتور خرید با موفقیت ثبت شد." & Environment.NewLine & "موجودی انبار به‌روزرسانی گردید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)

                dgvItems.Rows.Clear()
                GenerateNextInvoiceNumber()
                RecalculateTotal()
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور خرید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
