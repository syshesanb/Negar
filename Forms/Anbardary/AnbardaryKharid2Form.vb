Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Business.PersianDateHelper

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryKharid2Form
        Inherits Form

        Private ReadOnly _catalogService As New CatalogService()
        Private ReadOnly _invoiceService As New InvoiceService()
        Private _editInvoiceId As Integer? = Nothing
        Private _linesTable As DataTable

        ' حالت فاکتور جدید
        Public Sub New()
            InitializeComponent()
        End Sub

        ' حالت ویرایش فاکتور
        Public Sub New(invoiceId As Integer)
            InitializeComponent()
            _editInvoiceId = invoiceId
        End Sub

        Private Sub AnbardaryKharid2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvLines IsNot Nothing Then
                Me.dgvLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadLookups()

            If _editInvoiceId.HasValue Then
                LoadInvoiceForEdit(_editInvoiceId.Value)
            Else
                ' هدر پیش‌فرض جدید
                txtInvoiceNumber.Text = "PINV-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                dtpInvoiceDate.Value = DateTime.Now
                UpdatePersianLabel(lblInvoiceDatePersian, dtpInvoiceDate)
                _linesTable = CreateLinesTable()
                dgvLines.DataSource = _linesTable
                RefreshTotal()
            End If
        End Sub

        Private Sub ConfigureGrid()
            dgvLines.AutoGenerateColumns = False
            dgvLines.Columns.Clear()

            Dim colProduct As New DataGridViewTextBoxColumn()
            colProduct.DataPropertyName = "ProductName"
            colProduct.HeaderText = "نام کالا"
            colProduct.FillWeight = 40

            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.DataPropertyName = "Quantity"
            colQty.HeaderText = "مقدار"
            colQty.FillWeight = 15
            colQty.DefaultCellStyle.Format = "N2"

            Dim colPrice As New DataGridViewTextBoxColumn()
            colPrice.DataPropertyName = "UnitPrice"
            colPrice.HeaderText = "فی"
            colPrice.FillWeight = 20
            colPrice.DefaultCellStyle.Format = "N2"

            Dim colTotal As New DataGridViewTextBoxColumn()
            colTotal.DataPropertyName = "TotalPrice"
            colTotal.HeaderText = "جمع ردیف"
            colTotal.FillWeight = 25
            colTotal.DefaultCellStyle.Format = "N2"

            dgvLines.Columns.AddRange(New DataGridViewColumn() {colProduct, colQty, colPrice, colTotal})
        End Sub

        Private Sub LoadLookups()
            cmbWarehouse.DataSource = _catalogService.GetWarehouses()
            cmbWarehouse.DisplayMember = "WarehouseName"
            cmbWarehouse.ValueMember = "WarehouseID"

            cmbProduct.DataSource = _catalogService.GetProducts()
            cmbProduct.DisplayMember = "ProductName"
            cmbProduct.ValueMember = "ProductID"
        End Sub

        Private Sub LoadInvoiceForEdit(invoiceId As Integer)
            Try
                Dim hdr = _invoiceService.GetPurchaseInvoiceById(invoiceId)
                If hdr Is Nothing Then
                    MessageBox.Show("فاکتور مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Return
                End If

                txtInvoiceNumber.Text = Convert.ToString(hdr("InvoiceNumber"))
                If Not Convert.IsDBNull(hdr("InvoiceDate")) Then
                    dtpInvoiceDate.Value = Convert.ToDateTime(hdr("InvoiceDate"))
                End If
                UpdatePersianLabel(lblInvoiceDatePersian, dtpInvoiceDate)

                txtPartyName.Text = Convert.ToString(hdr("VendorName"))
                If Not Convert.IsDBNull(hdr("WarehouseID")) Then
                    cmbWarehouse.SelectedValue = Convert.ToInt32(hdr("WarehouseID"))
                End If

                ' بارگذاری جزئیات
                _linesTable = _invoiceService.GetPurchaseInvoiceDetails(invoiceId)
                dgvLines.DataSource = _linesTable
                RefreshTotal()

            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End Try
        End Sub

        Private Sub DtpInvoiceDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpInvoiceDate.ValueChanged
            UpdatePersianLabel(lblInvoiceDatePersian, dtpInvoiceDate)
        End Sub

        Private Sub BtnAddLine_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
            Dim quantity As Decimal
            Dim unitPrice As Decimal

            If cmbProduct.SelectedValue Is Nothing Then
                MessageBox.Show("لطفاً کالا را انتخاب کنید.")
                Return
            End If

            If Not Decimal.TryParse(txtQuantity.Text, quantity) OrElse quantity <= 0 Then
                MessageBox.Show("مقدار معتبر و بزرگتر از صفر نیست.")
                txtQuantity.Focus()
                Return
            End If

            If Not Decimal.TryParse(txtUnitPrice.Text, unitPrice) OrElse unitPrice < 0 Then
                MessageBox.Show("فی معتبر نیست.")
                txtUnitPrice.Focus()
                Return
            End If

            Dim productId = Convert.ToInt32(cmbProduct.SelectedValue)
            Dim productName = Convert.ToString(cmbProduct.Text)
            Dim lineTotal = quantity * unitPrice

            ' بررسی تکراری نبودن کالا در لیست
            For Each row As DataRow In _linesTable.Rows
                If row.RowState <> DataRowState.Deleted AndAlso Convert.ToInt32(row("ProductID")) = productId Then
                    MessageBox.Show("این کالا قبلاً به فاکتور اضافه شده است. در صورت نیاز ردیف قبلی را حذف و مجدداً اضافه کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            _linesTable.Rows.Add(Nothing, productId, productName, quantity, unitPrice, lineTotal)
            txtQuantity.Clear()
            txtUnitPrice.Clear()
            cmbProduct.Focus()
            RefreshTotal()
        End Sub

        Private Sub BtnRemoveLine_Click(sender As Object, e As EventArgs) Handles btnRemoveLine.Click
            If dgvLines.CurrentRow Is Nothing Then Return

            Dim rowView = TryCast(dgvLines.CurrentRow.DataBoundItem, DataRowView)
            If rowView IsNot Nothing Then
                rowView.Row.Delete()
            End If
            RefreshTotal()
        End Sub

        Private Sub RefreshTotal()
            Dim total As Decimal = 0D
            For Each row As DataRow In _linesTable.Rows
                If row.RowState <> DataRowState.Deleted AndAlso Not row.IsNull("TotalPrice") Then
                    total += Convert.ToDecimal(row("TotalPrice"))
                End If
            Next
            lblTotal.Text = "جمع کل فاکتور: " & total.ToString("N0") & " ریال"
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtInvoiceNumber.Text) Then
                MessageBox.Show("شماره فاکتور الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtInvoiceNumber.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(txtPartyName.Text) Then
                MessageBox.Show("نام فروشنده الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPartyName.Focus()
                Return
            End If

            If cmbWarehouse.SelectedValue Is Nothing Then
                MessageBox.Show("انتخاب انبار الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            dgvLines.EndEdit()
            Dim activeRows = _linesTable.Select("", "", DataViewRowState.CurrentRows)
            If activeRows.Length = 0 Then
                MessageBox.Show("حداقل یک ردیف کالا در فاکتور باید وجود داشته باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim createdBy = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)
                Dim warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue)
                
                Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()
                For Each row In activeRows
                    lines.Add(Tuple.Create(Convert.ToInt32(row("ProductID")), Convert.ToDecimal(row("Quantity")), Convert.ToDecimal(row("UnitPrice"))))
                Next

                If _editInvoiceId.HasValue Then
                    _invoiceService.UpdatePurchaseInvoice(_editInvoiceId.Value, txtInvoiceNumber.Text.Trim(), dtpInvoiceDate.Value, txtPartyName.Text.Trim(), warehouseId, createdBy, lines)
                    MessageBox.Show("فاکتور خرید با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim newId = _invoiceService.SavePurchaseInvoice(txtInvoiceNumber.Text.Trim(), dtpInvoiceDate.Value, txtPartyName.Text.Trim(), warehouseId, createdBy, lines)
                    MessageBox.Show("فاکتور خرید جدید با شماره " & txtInvoiceNumber.Text.Trim() & " ثبت شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Function CreateLinesTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("DetailID", GetType(Integer))
            table.Columns.Add("ProductID", GetType(Integer))
            table.Columns.Add("ProductName", GetType(String))
            table.Columns.Add("Quantity", GetType(Decimal))
            table.Columns.Add("UnitPrice", GetType(Decimal))
            table.Columns.Add("TotalPrice", GetType(Decimal))
            Return table
        End Function
    End Class
End Namespace
