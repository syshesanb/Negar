Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Business.PersianDateHelper

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryKharid1Form
        Inherits Form

        Private ReadOnly _invoiceService As New InvoiceService()
        Private _invoicesTable As DataTable

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryKharid1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            If Me.dgvInvoices IsNot Nothing Then
                Me.dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
        End Sub

        Private Sub ConfigureGrid()
            dgvInvoices.AutoGenerateColumns = False
            dgvInvoices.Columns.Clear()

            Dim colId As New DataGridViewTextBoxColumn()
            colId.DataPropertyName = "InvoiceID"
            colId.Name = "InvoiceID"
            colId.Visible = False

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.DataPropertyName = "InvoiceNumber"
            colNum.HeaderText = "شماره فاکتور"
            colNum.FillWeight = 20

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.DataPropertyName = "InvoiceDate"
            colDate.HeaderText = "تاریخ فاکتور"
            colDate.FillWeight = 20

            Dim colVendor As New DataGridViewTextBoxColumn()
            colVendor.DataPropertyName = "VendorName"
            colVendor.HeaderText = "فروشنده"
            colVendor.FillWeight = 30

            Dim colTotal As New DataGridViewTextBoxColumn()
            colTotal.DataPropertyName = "TotalAmount"
            colTotal.HeaderText = "جمع کل (ریال)"
            colTotal.FillWeight = 30
            colTotal.DefaultCellStyle.Format = "N0"

            dgvInvoices.Columns.AddRange(New DataGridViewColumn() {colId, colNum, colDate, colVendor, colTotal})
        End Sub

        Private Sub LoadData()
            Try
                _invoicesTable = _invoiceService.GetPurchaseInvoices()
                
                ' تبدیل ستون تاریخ میلادی به تاریخ شمسی در نمایش
                If _invoicesTable IsNot Nothing AndAlso Not _invoicesTable.Columns.Contains("PersianDate") Then
                    _invoicesTable.Columns.Add("PersianDate", GetType(String))
                End If

                For Each row As DataRow In _invoicesTable.Rows
                    If Not row.IsNull("InvoiceDate") Then
                        row("PersianDate") = ToPersian(Convert.ToDateTime(row("InvoiceDate")))
                    End If
                Next

                dgvInvoices.DataSource = _invoicesTable
                
                ' ست کردن ستون با تاریخ شمسی
                If dgvInvoices.Columns.Contains("InvoiceDate") Then
                    dgvInvoices.Columns("InvoiceDate").DataPropertyName = "PersianDate"
                End If

            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست فاکتورها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryKharid2Form()
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            OpenSelectedForEdit()
        End Sub

        Private Sub DgvInvoices_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoices.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit()
            End If
        End Sub

        Private Sub OpenSelectedForEdit()
            If dgvInvoices.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک فاکتور را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(dgvInvoices.CurrentRow.Cells("InvoiceID").Value)
            Using frm As New AnbardaryKharid2Form(invoiceId)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If dgvInvoices.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک فاکتور را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(dgvInvoices.CurrentRow.Cells("InvoiceID").Value)
            Dim invoiceNum = Convert.ToString(dgvInvoices.CurrentRow.Cells(1).Value)

            Dim confirm = MessageBox.Show("آیا از حذف فاکتور خرید شماره «" & invoiceNum & "» و بازگرداندن موجودی انبار اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _invoiceService.DeletePurchaseInvoice(invoiceId)
                    MessageBox.Show("فاکتور خرید با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            txtSearch.Clear()
            LoadData()
        End Sub

        Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
            If _invoicesTable Is Nothing Then Return
            Dim kw = txtSearch.Text.Trim().Replace("'", "''")
            If String.IsNullOrEmpty(kw) Then
                _invoicesTable.DefaultView.RowFilter = ""
            Else
                _invoicesTable.DefaultView.RowFilter = String.Format(
                    "InvoiceNumber LIKE '%{0}%' OR VendorName LIKE '%{0}%' OR PersianDate LIKE '%{0}%'", kw)
            End If
        End Sub
    End Class
End Namespace
