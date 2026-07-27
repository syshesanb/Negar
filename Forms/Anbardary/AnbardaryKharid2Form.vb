Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Business.PersianDateHelper
Imports Negar.Forms.Moshtarak
Imports Negar.Data

Namespace Negar.Forms
    Public Class AnbardaryKharid2Form
        Inherits Form

        Private ReadOnly _catalogService As New CatalogService()
        Private ReadOnly _invoiceService As New InvoiceService()
        Private ReadOnly _uomService As New UnitOfMeasureService()
        Private ReadOnly _paymentService As New PaymentService()
        Private _editInvoiceId As Integer? = Nothing
        Private _defaultDocType As String = "فاکتور خرید"
        Private _isLoading As Boolean = False
        Private _isDirty As Boolean = False
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()
        Private totalsTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const TotalPreloadedRows As Integer = 30

        Public Property SelectedVendorID As Integer?
        Public Property SelectedVendorCode As String
        Public Property SelectedVendorName As String

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(docType As String)
            InitializeComponent()
            _defaultDocType = docType
        End Sub

        Public Sub New(invoiceId As Integer)
            InitializeComponent()
            _editInvoiceId = invoiceId
        End Sub

        Public Sub New(invoiceId As Integer, docType As String)
            InitializeComponent()
            _editInvoiceId = invoiceId
            _defaultDocType = docType
        End Sub

        Private _editReceiptId As Integer? = Nothing
        Private _parentInvoiceId As Integer? = Nothing

        Public Sub New(invoiceId As Integer, docType As String, isNewBargasht As Boolean)
            InitializeComponent()
            _parentInvoiceId = invoiceId
            _defaultDocType = docType
        End Sub

        Public Sub New(invoiceId As Integer, receiptId As Integer)
            InitializeComponent()
            _editInvoiceId = invoiceId
            _editReceiptId = receiptId
            _defaultDocType = "رسید ورود به انبار"
        End Sub

        Private Sub AnbardaryKharid2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            Me.WindowState = FormWindowState.Maximized
            _isLoading = True

            If _defaultDocType = "رسید ورود به انبار" Then
                tabPageSanad.Text = "سطرهای رسید انبار"
                tabPageZamayem.Text = "ضمائم رسید انبار"
                tabPageYaddasht.Text = "یادداشت برای رسید انبار"
            Else
                tabPageSanad.Text = "سطرهای فاکتور خرید"
                tabPageZamayem.Text = "ضمائم فاکتور خرید"
                tabPageYaddasht.Text = "یادداشت برای فاکتور خرید"
            End If

            If Me.dgvEntryLines IsNot Nothing Then
                Me.dgvEntryLines.CellBorderStyle = DataGridViewCellBorderStyle.Single
                Me.dgvEntryLines.GridColor = Color.FromArgb(200, 210, 225)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.dgvEntryLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
                Me.dgvEntryLines.DefaultCellStyle.SelectionForeColor = Color.White
                Me.dgvEntryLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            InitializeEntryGrid()
            CreateFilterTextBoxes()
            InitializeTotalsBoxes()

            AddHandler dgvEntryLines.ColumnWidthChanged, AddressOf AlignSearchAndTotalBoxes
            AddHandler dgvEntryLines.Scroll, AddressOf DgvEntryLines_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchAndTotalBoxes
            AddHandler cmbTaxEntryMode.SelectedIndexChanged, AddressOf CmbTaxEntryMode_SelectedIndexChanged
            AddHandler txtTotalVatInput.TextChanged, AddressOf TxtTotalVatInput_TextChanged

            ' مقداردهی پیشفرض تاریخ ثبت سیستم و کامبوباکس مالیات
            txtSystemDate.Text = ToPersian(DateTime.Today)
            If cmbTaxEntryMode.Items.Count > 0 Then
                cmbTaxEntryMode.SelectedIndex = 0
            End If

            If _editInvoiceId.HasValue Then
                Me.Text = "ویرایش " & _defaultDocType
                LoadInvoiceForEdit(_editInvoiceId.Value)
                btnSaveAndContinue.Visible = False
            ElseIf _parentInvoiceId.HasValue Then
                Me.Text = "ثبت " & _defaultDocType & " جدید"
                LoadInvoiceForEdit(_parentInvoiceId.Value)
                txtEntryReference.Text = "PINV-RETURN-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                btnSaveAndContinue.Visible = False
            Else
                Me.Text = "ثبت " & _defaultDocType & " جدید"
                Dim prefix = If(_defaultDocType = "رسید ورود به انبار", "REC-", "PINV-")
                txtEntryReference.Text = prefix & DateTime.Now.ToString("yyyyMMddHHmmss")
                btnSaveAndContinue.Visible = True
            End If

            If _defaultDocType = "فاکتور خرید" Then
                If dgvEntryLines.Columns.Contains("colBtnWarehouse") Then dgvEntryLines.Columns("colBtnWarehouse").Visible = False
                If dgvEntryLines.Columns.Contains("colWarehouse") Then dgvEntryLines.Columns("colWarehouse").Visible = False
                If pnlViewShenavar IsNot Nothing Then pnlViewShenavar.Visible = False

                dgvEntryLines.Columns("colReceivedQty").Visible = False
                dgvEntryLines.Columns("colReceiptQty").Visible = False
                dgvEntryLines.Columns("colRemainingQty").Visible = False
                dgvEntryLines.Columns("colReturnQty").Visible = False
            ElseIf _defaultDocType = "برگشت از خرید" Then
                If dgvEntryLines.Columns.Contains("colBtnWarehouse") Then dgvEntryLines.Columns("colBtnWarehouse").Visible = False
                If dgvEntryLines.Columns.Contains("colWarehouse") Then dgvEntryLines.Columns("colWarehouse").Visible = False
                If pnlViewShenavar IsNot Nothing Then pnlViewShenavar.Visible = False

                If dgvEntryLines.Columns.Contains("colBtnKala") Then dgvEntryLines.Columns("colBtnKala").Visible = False
                If dgvEntryLines.Columns.Contains("colBtnUnit") Then dgvEntryLines.Columns("colBtnUnit").Visible = False

                If dgvEntryLines.Columns.Contains("colKalaCode") Then dgvEntryLines.Columns("colKalaCode").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colKalaName") Then dgvEntryLines.Columns("colKalaName").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colUnit") Then dgvEntryLines.Columns("colUnit").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colQty") Then dgvEntryLines.Columns("colQty").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colUnitPrice") Then dgvEntryLines.Columns("colUnitPrice").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colLineTotal") Then dgvEntryLines.Columns("colLineTotal").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colDiscount") Then dgvEntryLines.Columns("colDiscount").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colLineTotalAfterDiscount") Then dgvEntryLines.Columns("colLineTotalAfterDiscount").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colVat") Then dgvEntryLines.Columns("colVat").ReadOnly = True
                If dgvEntryLines.Columns.Contains("colTotalPrice") Then dgvEntryLines.Columns("colTotalPrice").ReadOnly = True

                dgvEntryLines.Columns("colReceivedQty").Visible = False
                dgvEntryLines.Columns("colReceiptQty").Visible = False
                dgvEntryLines.Columns("colRemainingQty").Visible = False
                dgvEntryLines.Columns("colReturnQty").Visible = True
                dgvEntryLines.Columns("colReturnQty").ReadOnly = False

                ' فیلدهای سرصفحه ریداونلی شوند
                If btnSelectVendor IsNot Nothing Then btnSelectVendor.Visible = False
                If txtEntryReference IsNot Nothing Then txtEntryReference.ReadOnly = True
                If txtVendorInvoiceNumber IsNot Nothing Then txtVendorInvoiceNumber.ReadOnly = True
                If txtDateSanad IsNot Nothing Then txtDateSanad.ReadOnly = True
                If btnCalDate IsNot Nothing Then btnCalDate.Visible = False
                If txtSystemDate IsNot Nothing Then txtSystemDate.ReadOnly = True
                If btnCalSystemDate IsNot Nothing Then btnCalSystemDate.Visible = False
                If cmbTaxEntryMode IsNot Nothing Then cmbTaxEntryMode.Enabled = False
                If txtEntryDescription IsNot Nothing Then txtEntryDescription.ReadOnly = True
                If txtTotalVatInput IsNot Nothing Then txtTotalVatInput.ReadOnly = True

                ' پنل دکمهها: فقط "ثبت برگشت از خرید و خروج"، "پاک کردن جستجوها" و "خروج" نمایش داده شوند
                btnSaveEntry.Text = "ثبت برگشت از خرید و خروج"
                If btnSaveAndContinue IsNot Nothing Then btnSaveAndContinue.Visible = False
                If btnAddLine IsNot Nothing Then btnAddLine.Visible = False
                If btnDeleteRow IsNot Nothing Then btnDeleteRow.Visible = False
                If btnCopyBelow IsNot Nothing Then btnCopyBelow.Visible = False
                If btnCopyAbove IsNot Nothing Then btnCopyAbove.Visible = False
                If btnCopyToPos IsNot Nothing Then btnCopyToPos.Visible = False
            Else
                If dgvEntryLines.Columns.Contains("colBtnWarehouse") Then dgvEntryLines.Columns("colBtnWarehouse").Visible = True
                If dgvEntryLines.Columns.Contains("colWarehouse") Then dgvEntryLines.Columns("colWarehouse").Visible = True
                If pnlViewShenavar IsNot Nothing Then pnlViewShenavar.Visible = True

                dgvEntryLines.Columns("colUnitPrice").Visible = False
                dgvEntryLines.Columns("colLineTotal").Visible = False
                dgvEntryLines.Columns("colDiscount").Visible = False
                dgvEntryLines.Columns("colLineTotalAfterDiscount").Visible = False
                dgvEntryLines.Columns("colVat").Visible = False
                dgvEntryLines.Columns("colTotalPrice").Visible = False
                dgvEntryLines.Columns("colQty").ReadOnly = True

                dgvEntryLines.Columns("colReceivedQty").Visible = True
                dgvEntryLines.Columns("colReceiptQty").Visible = True
                dgvEntryLines.Columns("colRemainingQty").Visible = True
                dgvEntryLines.Columns("colReturnQty").Visible = False

                ' اگر رسید انبار به صورت مستقل ثبت میشود (بدون فاکتور مرجع)، امکان انتخاب کالا و واحد فعال باشد
                Dim isIndependentReceipt As Boolean = Not _editInvoiceId.HasValue

                If dgvEntryLines.Columns.Contains("colBtnKala") Then dgvEntryLines.Columns("colBtnKala").Visible = isIndependentReceipt
                If dgvEntryLines.Columns.Contains("colBtnUnit") Then dgvEntryLines.Columns("colBtnUnit").Visible = isIndependentReceipt
                If dgvEntryLines.Columns.Contains("colKalaCode") Then dgvEntryLines.Columns("colKalaCode").ReadOnly = Not isIndependentReceipt
                If dgvEntryLines.Columns.Contains("colKalaName") Then dgvEntryLines.Columns("colKalaName").ReadOnly = Not isIndependentReceipt
                If dgvEntryLines.Columns.Contains("colUnit") Then dgvEntryLines.Columns("colUnit").ReadOnly = Not isIndependentReceipt

                ' در حالت رسید انبار مستقل، دکمه انتخاب فروشنده فعال باشد
                If btnSelectVendor IsNot Nothing Then btnSelectVendor.Visible = isIndependentReceipt
                If txtVendorInvoiceNumber IsNot Nothing Then txtVendorInvoiceNumber.ReadOnly = Not isIndependentReceipt
                If txtDateSanad IsNot Nothing Then txtDateSanad.ReadOnly = False
                If btnCalDate IsNot Nothing Then btnCalDate.Visible = True
                If txtSystemDate IsNot Nothing Then txtSystemDate.ReadOnly = True
                If btnCalSystemDate IsNot Nothing Then btnCalSystemDate.Visible = False

                ' شرح رسید انبار
                If txtEntryDescription IsNot Nothing Then txtEntryDescription.ReadOnly = False

                ' در حالت رسید انبار تب تسویه معنی ندارد
                If tabPageTasvieh IsNot Nothing AndAlso tabMain.TabPages.Contains(tabPageTasvieh) Then
                    tabMain.TabPages.Remove(tabPageTasvieh)
                End If
            End If

            If _defaultDocType = "فاکتور خرید" Then
                InitTasviehTabControls()
            End If

            UpdateTotals()
            AlignSearchAndTotalBoxes()
            _isLoading = False
        End Sub

        Private Sub InitializeEntryGrid()
            If dgvEntryLines.Columns.Count > 0 Then Return

            Dim table As New DataTable()
            table.Columns.Add("LineNumber", GetType(Integer))
            table.Columns.Add("ProductID", GetType(Integer))
            table.Columns.Add("ProductCode", GetType(String))
            table.Columns.Add("ProductName", GetType(String))
            table.Columns.Add("WarehouseID", GetType(Integer))
            table.Columns.Add("WarehouseName", GetType(String))
            table.Columns.Add("Unit", GetType(String))
            table.Columns.Add("Quantity", GetType(Decimal))
            table.Columns.Add("UnitPrice", GetType(Decimal))
            table.Columns.Add("LineTotal", GetType(Decimal))
            table.Columns.Add("Discount", GetType(Decimal))
            table.Columns.Add("LineTotalAfterDiscount", GetType(Decimal))
            table.Columns.Add("Vat", GetType(Decimal))
            table.Columns.Add("TotalPrice", GetType(Decimal))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("DetailID", GetType(Integer))
            table.Columns.Add("ReceivedQuantity", GetType(Decimal))
            table.Columns.Add("ReceiptQuantity", GetType(Decimal))
            table.Columns.Add("RemainingQuantity", GetType(Decimal))
            table.Columns.Add("ReturnQuantity", GetType(Decimal))

            For i = 1 To TotalPreloadedRows
                table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, 0D, 0D, "", 0, 0D, 0D, 0D, 0D)
            Next

            AddHandler table.ColumnChanged, AddressOf Table_ColumnChanged

            dgvEntryLines.AutoGenerateColumns = False
            dgvEntryLines.ReadOnly = False
            dgvEntryLines.DataSource = table
            dgvEntryLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvEntryLines.ColumnHeadersHeight = 38
            dgvEntryLines.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvEntryLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colLineNo As New DataGridViewTextBoxColumn()
            colLineNo.Name = "colLineNo"
            colLineNo.DataPropertyName = "LineNumber"
            colLineNo.HeaderText = "ردیف"
            colLineNo.Width = 45
            colLineNo.ReadOnly = True

            Dim colBtnKala As New DataGridViewButtonColumn()
            colBtnKala.Name = "colBtnKala"
            colBtnKala.HeaderText = "کد کالا"
            colBtnKala.Text = "کد کالا"
            colBtnKala.UseColumnTextForButtonValue = True
            colBtnKala.Width = 75
            colBtnKala.FlatStyle = FlatStyle.Standard

            Dim colKalaCode As New DataGridViewTextBoxColumn()
            colKalaCode.Name = "colKalaCode"
            colKalaCode.DataPropertyName = "ProductCode"
            colKalaCode.HeaderText = "کد / بارکد"
            colKalaCode.Width = 100

            Dim colKalaName As New DataGridViewTextBoxColumn()
            colKalaName.Name = "colKalaName"
            colKalaName.DataPropertyName = "ProductName"
            colKalaName.HeaderText = "نام کالا / خدمات"
            colKalaName.Width = 220

            Dim colBtnWarehouse As New DataGridViewButtonColumn()
            colBtnWarehouse.Name = "colBtnWarehouse"
            colBtnWarehouse.HeaderText = "انبار"
            colBtnWarehouse.Text = "..."
            colBtnWarehouse.UseColumnTextForButtonValue = True
            colBtnWarehouse.Width = 40
            colBtnWarehouse.FlatStyle = FlatStyle.Standard

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "colWarehouse"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار مقصد"
            colWarehouse.Width = 120

            Dim colBtnUnit As New DataGridViewButtonColumn()
            colBtnUnit.Name = "colBtnUnit"
            colBtnUnit.HeaderText = "واحد"
            colBtnUnit.Text = "..."
            colBtnUnit.UseColumnTextForButtonValue = True
            colBtnUnit.Width = 40
            colBtnUnit.FlatStyle = FlatStyle.Standard

            Dim colUnit As New DataGridViewTextBoxColumn()
            colUnit.Name = "colUnit"
            colUnit.DataPropertyName = "Unit"
            colUnit.HeaderText = "نام واحد"
            colUnit.Width = 75

            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.Name = "colQty"
            colQty.DataPropertyName = "Quantity"
            colQty.HeaderText = "تعداد /" & Environment.NewLine & "مقدار"
            colQty.Width = 95
            colQty.DefaultCellStyle.Format = "N2"
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colReceivedQty As New DataGridViewTextBoxColumn()
            colReceivedQty.Name = "colReceivedQty"
            colReceivedQty.DataPropertyName = "ReceivedQuantity"
            colReceivedQty.HeaderText = "رسید شده قبلی"
            colReceivedQty.Width = 95
            colReceivedQty.ReadOnly = True
            colReceivedQty.DefaultCellStyle.Format = "N2"
            colReceivedQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colReceivedQty.DefaultCellStyle.BackColor = Color.FromArgb(235, 235, 235)

            Dim colReceiptQty As New DataGridViewTextBoxColumn()
            colReceiptQty.Name = "colReceiptQty"
            colReceiptQty.DataPropertyName = "ReceiptQuantity"
            colReceiptQty.HeaderText = "تعداد رسید جاری"
            colReceiptQty.Width = 100
            colReceiptQty.DefaultCellStyle.Format = "N2"
            colReceiptQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colReceiptQty.DefaultCellStyle.BackColor = Color.LightYellow

            Dim colRemainingQty As New DataGridViewTextBoxColumn()
            colRemainingQty.Name = "colRemainingQty"
            colRemainingQty.DataPropertyName = "RemainingQuantity"
            colRemainingQty.HeaderText = "تعداد مانده"
            colRemainingQty.Width = 95
            colRemainingQty.ReadOnly = True
            colRemainingQty.DefaultCellStyle.Format = "N2"
            colRemainingQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colRemainingQty.DefaultCellStyle.BackColor = Color.FromArgb(235, 235, 235)

            Dim colReturnQty As New DataGridViewTextBoxColumn()
            colReturnQty.Name = "colReturnQty"
            colReturnQty.DataPropertyName = "ReturnQuantity"
            colReturnQty.HeaderText = "تعداد /" & Environment.NewLine & "مقدار برگشتی"
            colReturnQty.Width = 100
            colReturnQty.DefaultCellStyle.Format = "N2"
            colReturnQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colReturnQty.DefaultCellStyle.BackColor = Color.LightPink

            Dim colUnitPrice As New DataGridViewTextBoxColumn()
            colUnitPrice.Name = "colUnitPrice"
            colUnitPrice.DataPropertyName = "UnitPrice"
            colUnitPrice.HeaderText = "فی (قیمت واحد)"
            colUnitPrice.Width = 110
            colUnitPrice.DefaultCellStyle.Format = "N0"
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colLineTotal As New DataGridViewTextBoxColumn()
            colLineTotal.Name = "colLineTotal"
            colLineTotal.DataPropertyName = "LineTotal"
            colLineTotal.HeaderText = "مبلغ کل سطر"
            colLineTotal.Width = 120
            colLineTotal.ReadOnly = True
            colLineTotal.DefaultCellStyle.Format = "N0"
            colLineTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colDiscount As New DataGridViewTextBoxColumn()
            colDiscount.Name = "colDiscount"
            colDiscount.DataPropertyName = "Discount"
            colDiscount.HeaderText = "تخفیف سطر"
            colDiscount.Width = 100
            colDiscount.DefaultCellStyle.Format = "N0"
            colDiscount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colLineTotalAfterDiscount As New DataGridViewTextBoxColumn()
            colLineTotalAfterDiscount.Name = "colLineTotalAfterDiscount"
            colLineTotalAfterDiscount.DataPropertyName = "LineTotalAfterDiscount"
            colLineTotalAfterDiscount.HeaderText = "مبلغ کل سطر" & Environment.NewLine & "پس از تخفیف"
            colLineTotalAfterDiscount.Width = 135
            colLineTotalAfterDiscount.ReadOnly = True
            colLineTotalAfterDiscount.DefaultCellStyle.Format = "N0"
            colLineTotalAfterDiscount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colVat As New DataGridViewTextBoxColumn()
            colVat.Name = "colVat"
            colVat.DataPropertyName = "Vat"
            colVat.HeaderText = "مالیات و عوارض"
            colVat.Width = 110
            colVat.DefaultCellStyle.Format = "N0"
            colVat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colTotalPrice As New DataGridViewTextBoxColumn()
            colTotalPrice.Name = "colTotalPrice"
            colTotalPrice.DataPropertyName = "TotalPrice"
            colTotalPrice.HeaderText = "مبلغ خالص"
            colTotalPrice.Width = 130
            colTotalPrice.ReadOnly = True
            colTotalPrice.DefaultCellStyle.Format = "N0"
            colTotalPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.Width = 150

            dgvEntryLines.Columns.AddRange(New DataGridViewColumn() {
                colLineNo, colBtnKala, colKalaCode, colKalaName, colBtnWarehouse, colWarehouse,
                colBtnUnit, colUnit, colQty, colReturnQty, colReceivedQty, colReceiptQty, colRemainingQty, colUnitPrice, colLineTotal, colDiscount, colLineTotalAfterDiscount, colVat, colTotalPrice, colDesc
            })
        End Sub

        Private Sub Table_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
            If _isLoading Then Return
            _isDirty = True

            If e.Column.ColumnName = "ProductCode" Then
                Dim codeStr = Convert.ToString(If(e.Row.IsNull("ProductCode"), "", e.Row("ProductCode"))).Trim()
                If Not String.IsNullOrEmpty(codeStr) Then
                    Dim query = "SELECT p.ProductID, p.ProductCode, p.ProductName, p.Unit, p.DefaultPrice, p.DefaultWarehouseID, w.WarehouseName AS DefaultWarehouseName, p.TaxPercent FROM Products p LEFT JOIN Warehouses w ON p.DefaultWarehouseID = w.WarehouseID WHERE p.ProductCode = ? OR p.Barcode = ?"
                    Dim lookupDt = Sql.ExecuteTable(query, codeStr, codeStr)
                    If lookupDt.Rows.Count > 0 Then
                        Dim dr = lookupDt.Rows(0)

                        _isLoading = True
                        e.Row("ProductID") = dr("ProductID")
                        e.Row("ProductName") = dr("ProductName")
                        e.Row("Unit") = If(dr.IsNull("Unit"), "عدد", dr("Unit"))
                        e.Row("UnitPrice") = If(dr.IsNull("DefaultPrice"), 0D, dr("DefaultPrice"))

                        If Not dr.IsNull("DefaultWarehouseID") Then
                            e.Row("WarehouseID") = dr("DefaultWarehouseID")
                            e.Row("WarehouseName") = If(dr.IsNull("DefaultWarehouseName"), "", dr("DefaultWarehouseName"))
                        End If

                        Dim qty = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                        Dim price = Convert.ToDecimal(If(e.Row.IsNull("UnitPrice"), 0D, e.Row("UnitPrice")))
                        Dim disc = Convert.ToDecimal(If(e.Row.IsNull("Discount"), 0D, e.Row("Discount")))
                        Dim lineTot = qty * price
                        Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)

                        e.Row("LineTotal") = lineTot
                        e.Row("LineTotalAfterDiscount") = lineTotAfterDisc

                        If cmbTaxEntryMode.SelectedIndex <> 2 Then
                            Dim taxPct = If(dr.IsNull("TaxPercent"), 0D, Convert.ToDecimal(dr("TaxPercent")))
                            Dim vat = Math.Round(lineTotAfterDisc * taxPct / 100D, 0)
                            If vat < 0 Then vat = 0D
                            e.Row("Vat") = vat
                            e.Row("TotalPrice") = lineTotAfterDisc + vat
                        End If
                        _isLoading = False

                        If cmbTaxEntryMode.SelectedIndex = 2 Then
                            RecalculateProratedTax()
                        Else
                            UpdateTotals()
                        End If
                    Else
                        _isLoading = True
                        e.Row("ProductID") = 0
                        e.Row("ProductName") = "کالای نامشخص"
                        _isLoading = False
                    End If
                End If
            End If

            If e.Column.ColumnName = "Quantity" OrElse e.Column.ColumnName = "UnitPrice" OrElse e.Column.ColumnName = "Discount" OrElse e.Column.ColumnName = "Vat" Then
                _isLoading = True
                Dim qty = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                Dim price = Convert.ToDecimal(If(e.Row.IsNull("UnitPrice"), 0D, e.Row("UnitPrice")))
                Dim disc = Convert.ToDecimal(If(e.Row.IsNull("Discount"), 0D, e.Row("Discount")))

                Dim lineTot = qty * price
                Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)

                e.Row("LineTotal") = lineTot
                e.Row("LineTotalAfterDiscount") = lineTotAfterDisc

                If cmbTaxEntryMode.SelectedIndex <> 2 Then
                    Dim vat = Convert.ToDecimal(If(e.Row.IsNull("Vat"), 0D, e.Row("Vat")))
                    e.Row("TotalPrice") = lineTotAfterDisc + vat
                End If
                _isLoading = False

                If cmbTaxEntryMode.SelectedIndex = 2 Then
                    RecalculateProratedTax()
                Else
                    UpdateTotals()
                End If
            End If

            If e.Column.ColumnName = "ReceiptQuantity" Then
                _isLoading = True
                Dim qty = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                Dim received = Convert.ToDecimal(If(e.Row.IsNull("ReceivedQuantity"), 0D, e.Row("ReceivedQuantity")))
                Dim maxAllowed = Math.Max(0D, qty - received)
                Dim receiptQty = Convert.ToDecimal(If(e.Row.IsNull("ReceiptQuantity"), 0D, e.Row("ReceiptQuantity")))
                
                If receiptQty < 0 Then
                    receiptQty = 0
                    e.Row("ReceiptQuantity") = receiptQty
                End If
                
                e.Row("RemainingQuantity") = maxAllowed - receiptQty
                _isLoading = False
            End If
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlSerch.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvEntryLines.Columns
                Dim txt As New TextBox()
                txt.Name = "txtFilter_" & col.Name
                txt.Tag = col.DataPropertyName
                txt.BorderStyle = BorderStyle.FixedSingle

                If TypeOf col Is DataGridViewButtonColumn Then
                    txt.Enabled = False
                    txt.ReadOnly = True
                Else
                    AddHandler txt.TextChanged, AddressOf FilterTextBox_TextChanged
                End If

                pnlSerch.Controls.Add(txt)
                filterTextBoxes.Add(col.Name, txt)
            Next
        End Sub

        Private Sub InitializeTotalsBoxes()
            totalsTextBoxes.Clear()
            pnlTotalsRow.Controls.Clear()

            ' تکستباکسهای جمع زنده برای ستونهای خاص
            Dim targetCols = New String() {"colLineTotal", "colDiscount", "colLineTotalAfterDiscount", "colVat", "colTotalPrice"}
            For Each colName In targetCols
                Dim txt As New TextBox()
                txt.Name = "txtSum_" & colName
                txt.ReadOnly = True
                txt.BackColor = Color.FromArgb(235, 243, 255)
                txt.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                txt.TextAlign = HorizontalAlignment.Center
                txt.BorderStyle = BorderStyle.FixedSingle
                txt.Text = "0"

                pnlTotalsRow.Controls.Add(txt)
                totalsTextBoxes.Add(colName, txt)
            Next
        End Sub

        Private Sub DgvEntryLines_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchAndTotalBoxes()
        End Sub

        Private Sub AlignSearchAndTotalBoxes()
            If dgvEntryLines Is Nothing OrElse dgvEntryLines.Columns.Count = 0 Then Return

            ' ۱. تراز فیلترهای جستجو
            If pnlSerch IsNot Nothing Then
                pnlSerch.SuspendLayout()
                For Each kvp In filterTextBoxes
                    Dim colName = kvp.Key
                    Dim txt = kvp.Value
                    Dim col = dgvEntryLines.Columns(colName)

                    If col IsNot Nothing AndAlso col.Visible Then
                        Dim rect = dgvEntryLines.GetColumnDisplayRectangle(col.Index, True)
                        If rect.IsEmpty OrElse rect.Width = 0 Then
                            txt.Visible = False
                        Else
                            Dim screenPt = dgvEntryLines.PointToScreen(New Point(rect.X, 0))
                            Dim panelPt = pnlSerch.PointToClient(screenPt)
                            txt.Location = New Point(panelPt.X, 4)
                            txt.Width = rect.Width
                            txt.Visible = True
                        End If
                    Else
                        txt.Visible = False
                    End If
                Next
                pnlSerch.ResumeLayout()
            End If

            ' ۲. تراز تکستباکسهای جمع زنده زیر گرید
            If pnlTotalsRow IsNot Nothing Then
                pnlTotalsRow.SuspendLayout()
                For Each kvp In totalsTextBoxes
                    Dim colName = kvp.Key
                    Dim txt = kvp.Value
                    Dim col = dgvEntryLines.Columns(colName)

                    If col IsNot Nothing AndAlso col.Visible Then
                        Dim rect = dgvEntryLines.GetColumnDisplayRectangle(col.Index, True)
                        If rect.IsEmpty OrElse rect.Width = 0 Then
                            txt.Visible = False
                        Else
                            Dim screenPt = dgvEntryLines.PointToScreen(New Point(rect.X, 0))
                            Dim panelPt = pnlTotalsRow.PointToClient(screenPt)
                            txt.Location = New Point(panelPt.X, 4)
                            txt.Width = rect.Width
                            txt.Visible = True
                        End If
                    Else
                        txt.Visible = False
                    End If
                Next
                pnlTotalsRow.ResumeLayout()
            End If
        End Sub

        Private Sub FilterTextBox_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters()
        End Sub

        Private Sub ApplyFilters()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            Dim filters As New List(Of String)()
            For Each kvp In filterTextBoxes
                Dim txt = kvp.Value
                Dim propertyName = Convert.ToString(txt.Tag)
                If String.IsNullOrEmpty(propertyName) OrElse Not txt.Enabled Then Continue For

                Dim val = txt.Text.Trim().Replace("'", "''")
                If Not String.IsNullOrEmpty(val) Then
                    filters.Add(String.Format("Convert({0}, 'System.String') LIKE '%{1}%'", propertyName, val))
                End If
            Next

            If filters.Count > 0 Then
                table.DefaultView.RowFilter = String.Join(" AND ", filters)
            Else
                table.DefaultView.RowFilter = ""
            End If
            UpdateTotals()
        End Sub

        Private Sub CmbTaxEntryMode_SelectedIndexChanged(sender As Object, e As EventArgs)
            If _isLoading Then Return

            If cmbTaxEntryMode.SelectedIndex = 2 Then
                ' ورود مالیات و عوارض برای کل فاکتور
                txtTotalVatInput.Enabled = True
                If dgvEntryLines.Columns.Contains("colVat") Then
                    dgvEntryLines.Columns("colVat").ReadOnly = True
                End If
                RecalculateProratedTax()
            Else
                ' غیرفعالسازی مالیات کل فاکتور و فعالسازی ویرایش دستی سطر
                txtTotalVatInput.Enabled = False
                txtTotalVatInput.Text = "0"
                If dgvEntryLines.Columns.Contains("colVat") Then
                    dgvEntryLines.Columns("colVat").ReadOnly = False
                End If

                Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
                If table IsNot Nothing Then
                    _isLoading = True
                    For Each row As DataRow In table.Rows
                        If row.RowState <> DataRowState.Deleted Then
                            Dim lineTotAfterDisc = Convert.ToDecimal(If(row.IsNull("LineTotalAfterDiscount"), 0D, row("LineTotalAfterDiscount")))
                            Dim vat = Convert.ToDecimal(If(row.IsNull("Vat"), 0D, row("Vat")))
                            row("TotalPrice") = lineTotAfterDisc + vat
                        End If
                    Next
                    _isLoading = False
                End If
                UpdateTotals()
            End If
        End Sub

        Private Sub TxtTotalVatInput_TextChanged(sender As Object, e As EventArgs)
            If _isLoading Then Return
            If cmbTaxEntryMode.SelectedIndex = 2 Then
                RecalculateProratedTax()
            End If
        End Sub

        Private Sub RecalculateProratedTax()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            Dim totalVatVal As Decimal = 0D
            Decimal.TryParse(txtTotalVatInput.Text.Trim().Replace(",", ""), totalVatVal)

            ' محاسبه مجموع مبلغ خالص سطرها پس از تخفیف برای سرشکن وزنی
            Dim sumNetAfterDisc As Decimal = 0D
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted Then
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                    Dim pid = Convert.ToInt32(If(row.IsNull("ProductID"), 0, row("ProductID")))
                    If (pid > 0 OrElse qty > 0) AndAlso Not row.IsNull("LineTotalAfterDiscount") Then
                        sumNetAfterDisc += Math.Max(0D, Convert.ToDecimal(row("LineTotalAfterDiscount")))
                    End If
                End If
            Next

            _isLoading = True
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted Then
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    Dim pid = Convert.ToInt32(If(row.IsNull("ProductID"), 0, row("ProductID")))
                    Dim lineTotAfterDisc = Convert.ToDecimal(If(row.IsNull("LineTotalAfterDiscount"), 0D, row("LineTotalAfterDiscount")))

                    If (pid > 0 OrElse qty > 0) AndAlso sumNetAfterDisc > 0D AndAlso lineTotAfterDisc > 0D Then
                        Dim proratedVat = Math.Round((totalVatVal * lineTotAfterDisc) / sumNetAfterDisc, 0)
                        row("Vat") = Math.Max(0D, proratedVat)
                    Else
                        row("Vat") = 0D
                    End If
                    row("TotalPrice") = lineTotAfterDisc + Convert.ToDecimal(row("Vat"))
                End If
            Next
            _isLoading = False

            UpdateTotals()
        End Sub

        Private Sub UpdateTotals()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            Dim sumLineTotal As Decimal = 0D
            Dim sumDiscount As Decimal = 0D
            Dim sumLineTotalAfterDiscount As Decimal = 0D
            Dim sumVat As Decimal = 0D
            Dim sumTotalPrice As Decimal = 0D

            Dim activeRows = table.Select(table.DefaultView.RowFilter)
            For Each row In activeRows
                If row.RowState <> DataRowState.Deleted Then
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                    Dim disc = Convert.ToDecimal(If(row.IsNull("Discount"), 0D, row("Discount")))
                    Dim vat = Convert.ToDecimal(If(row.IsNull("Vat"), 0D, row("Vat")))

                    Dim lineTot = qty * price
                    Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)
                    Dim totalPr = lineTotAfterDisc + vat

                    sumLineTotal += lineTot
                    sumDiscount += disc
                    sumLineTotalAfterDiscount += lineTotAfterDisc
                    sumVat += vat
                    sumTotalPrice += totalPr
                End If
            Next

            If totalsTextBoxes.ContainsKey("colLineTotal") Then totalsTextBoxes("colLineTotal").Text = sumLineTotal.ToString("N0")
            If totalsTextBoxes.ContainsKey("colDiscount") Then totalsTextBoxes("colDiscount").Text = sumDiscount.ToString("N0")
            If totalsTextBoxes.ContainsKey("colLineTotalAfterDiscount") Then totalsTextBoxes("colLineTotalAfterDiscount").Text = sumLineTotalAfterDiscount.ToString("N0")
            If totalsTextBoxes.ContainsKey("colVat") Then totalsTextBoxes("colVat").Text = sumVat.ToString("N0")
            If totalsTextBoxes.ContainsKey("colTotalPrice") Then totalsTextBoxes("colTotalPrice").Text = sumTotalPrice.ToString("N0")

            txtJamBedehkar.Text = sumLineTotal.ToString("N0")
            txtKasriDebit.Text = sumTotalPrice.ToString("N0")

            UpdateHeaderSettlementStatus()
        End Sub

        Public Function CalculateGrandTotalSum() As Decimal
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return 0D
            Dim sumTotalPrice As Decimal = 0D
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted Then
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                    Dim disc = Convert.ToDecimal(If(row.IsNull("Discount"), 0D, row("Discount")))
                    Dim vat = Convert.ToDecimal(If(row.IsNull("Vat"), 0D, row("Vat")))
                    Dim lineTot = qty * price
                    Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)
                    sumTotalPrice += (lineTotAfterDisc + vat)
                End If
            Next
            Return sumTotalPrice
        End Function

        Private Sub LoadInvoiceForEdit(invoiceId As Integer)
            Try
                Dim hdr = _invoiceService.GetPurchaseInvoiceById(invoiceId)
                If hdr Is Nothing Then
                    MessageBox.Show("سند مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Return
                End If

                _isLoading = True

                If _defaultDocType <> "رسید ورود به انبار" AndAlso _defaultDocType <> "برگشت از خرید" Then
                    _defaultDocType = Convert.ToString(hdr("InvoiceType"))
                End If
                
                If _defaultDocType = "رسید ورود به انبار" Then
                    txtEntryReference.Text = "REC-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                    txtVendorInvoiceNumber.Text = "عطف به فاکتور: " & Convert.ToString(hdr("InvoiceNumber"))
                    txtDateSanad.Text = ToPersian(DateTime.Today)
                    txtEntryDescription.Text = "رسید انبار برای فاکتور خرید " & Convert.ToString(hdr("InvoiceNumber"))
                ElseIf _defaultDocType = "برگشت از خرید" Then
                    txtEntryReference.Text = Convert.ToString(hdr("InvoiceNumber"))
                    txtVendorInvoiceNumber.Text = If(hdr.Table.Columns.Contains("VendorInvoiceNumber"), Convert.ToString(hdr("VendorInvoiceNumber")), "")
                    txtDateSanad.Text = ToPersian(DateTime.Today)
                    txtEntryDescription.Text = "برگشت از خرید برای فاکتور خرید " & Convert.ToString(hdr("InvoiceNumber"))
                Else
                    txtEntryReference.Text = Convert.ToString(hdr("InvoiceNumber"))
                    txtVendorInvoiceNumber.Text = If(hdr.Table.Columns.Contains("VendorInvoiceNumber"), Convert.ToString(hdr("VendorInvoiceNumber")), "")
                    If Not Convert.IsDBNull(hdr("InvoiceDate")) Then
                        txtDateSanad.Text = ToPersian(Convert.ToDateTime(hdr("InvoiceDate")))
                    End If
                    txtEntryDescription.Text = Convert.ToString(hdr("Description"))
                End If

                lblSarfaslValue.Text = Convert.ToString(hdr("VendorName"))

                If hdr.Table.Columns.Contains("TaxEntryMode") AndAlso Not hdr.IsNull("TaxEntryMode") Then
                    Dim modeIdx = Convert.ToInt32(hdr("TaxEntryMode"))
                    If modeIdx >= 0 AndAlso modeIdx < cmbTaxEntryMode.Items.Count Then
                        cmbTaxEntryMode.SelectedIndex = modeIdx
                    End If
                End If

                If hdr.Table.Columns.Contains("TotalVat") AndAlso Not hdr.IsNull("TotalVat") Then
                    Dim totVat = Convert.ToDecimal(hdr("TotalVat"))
                    txtTotalVatInput.Text = totVat.ToString("N0")
                End If

                If cmbTaxEntryMode.SelectedIndex = 2 Then
                    txtTotalVatInput.Enabled = True
                    If dgvEntryLines.Columns.Contains("colVat") Then
                        dgvEntryLines.Columns("colVat").ReadOnly = True
                    End If
                Else
                    txtTotalVatInput.Enabled = False
                    If dgvEntryLines.Columns.Contains("colVat") Then
                        dgvEntryLines.Columns("colVat").ReadOnly = False
                    End If
                End If

                Dim dtDetails = _invoiceService.GetPurchaseInvoiceDetails(invoiceId)

                Dim dtProducts = Sql.ExecuteTable("SELECT p.ProductID, p.ProductCode, p.DefaultWarehouseID, w.WarehouseName AS DefaultWarehouseName, p.TaxPercent FROM Products p LEFT JOIN Warehouses w ON p.DefaultWarehouseID = w.WarehouseID")
                Dim prodInfo As New Dictionary(Of Integer, DataRow)()
                For Each pRow As DataRow In dtProducts.Rows
                    prodInfo(Convert.ToInt32(pRow("ProductID"))) = pRow
                Next

                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.Clear()

                Dim receiptDetails As New Dictionary(Of Integer, Decimal)()
                If _editReceiptId.HasValue Then
                    Dim dtRecDetails = _invoiceService.GetWarehouseReceiptDetailsList(_editReceiptId.Value)
                    For Each rRow As DataRow In dtRecDetails.Rows
                        Dim pdId = Convert.ToInt32(rRow("PurchaseInvoiceDetailID"))
                        Dim rQty = Convert.ToDecimal(rRow("Quantity"))
                        receiptDetails(pdId) = rQty
                    Next
                End If

                Dim lineNo As Integer = 1
                For Each dRow As DataRow In dtDetails.Rows
                    Dim row = table.NewRow()
                    row("LineNumber") = lineNo
                    row("ProductID") = dRow("ProductID")
                    row("Quantity") = dRow("Quantity")
                    row("UnitPrice") = dRow("UnitPrice")
                    row("Discount") = dRow("Discount")

                    Dim qty = Convert.ToDecimal(dRow("Quantity"))
                    Dim price = Convert.ToDecimal(dRow("UnitPrice"))
                    Dim disc = Convert.ToDecimal(dRow("Discount"))
                    Dim lineTot = qty * price
                    Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)

                    row("LineTotal") = lineTot
                    row("LineTotalAfterDiscount") = lineTotAfterDisc

                    Dim pid = Convert.ToInt32(dRow("ProductID"))
                    If prodInfo.ContainsKey(pid) Then
                        Dim pRow = prodInfo(pid)
                        row("ProductCode") = If(pRow.IsNull("ProductCode"), "", pRow("ProductCode"))
                        row("ProductName") = dRow("ProductName")
                        row("Unit") = dRow("Unit")
                        row("WarehouseID") = If(pRow.IsNull("DefaultWarehouseID"), 0, pRow("DefaultWarehouseID"))
                        row("WarehouseName") = If(pRow.IsNull("DefaultWarehouseName"), "", pRow("DefaultWarehouseName"))
                    Else
                        row("ProductCode") = ""
                        row("ProductName") = dRow("ProductName")
                        row("Unit") = dRow("Unit")
                        row("WarehouseID") = 0
                        row("WarehouseName") = ""
                    End If

                    Dim vatVal = If(dRow.Table.Columns.Contains("Vat") AndAlso Not dRow.IsNull("Vat"), Convert.ToDecimal(dRow("Vat")), 0D)
                    row("Vat") = vatVal
                    row("TotalPrice") = lineTotAfterDisc + vatVal
                    row("DetailID") = If(dRow.Table.Columns.Contains("DetailID") AndAlso Not dRow.IsNull("DetailID"), dRow("DetailID"), 0)
                    
                    Dim detailIdInt = Convert.ToInt32(row("DetailID"))
                    Dim receivedQty = If(dRow.Table.Columns.Contains("ReceivedQuantity") AndAlso Not dRow.IsNull("ReceivedQuantity"), Convert.ToDecimal(dRow("ReceivedQuantity")), 0D)
                    
                    If _editReceiptId.HasValue AndAlso receiptDetails.ContainsKey(detailIdInt) Then
                        Dim currentRecQty = receiptDetails(detailIdInt)
                        receivedQty -= currentRecQty
                        row("ReceivedQuantity") = Math.Max(0D, receivedQty)
                        row("RemainingQuantity") = Math.Max(0D, qty - Math.Max(0D, receivedQty))
                        row("ReceiptQuantity") = currentRecQty
                    Else
                        row("ReceivedQuantity") = receivedQty
                        row("RemainingQuantity") = Math.Max(0D, qty - receivedQty)
                        row("ReceiptQuantity") = row("RemainingQuantity")
                    End If
                    
                    table.Rows.Add(row)
                    lineNo += 1
                Next

                _isLoading = False
                UpdateTotals()
            Catch ex As Exception
                _isLoading = False
                MessageBox.Show("خطا در بارگذاری سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnAddLine_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
            Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
            Dim lineNo = table.Rows.Count + 1
            table.Rows.Add(lineNo, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, 0D, 0D, "", 0, 0D, 0D, 0D)
        End Sub

        Private Sub BtnDeleteRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
            If dgvEntryLines.CurrentRow IsNot Nothing Then
                dgvEntryLines.Rows.Remove(dgvEntryLines.CurrentRow)
                If cmbTaxEntryMode.SelectedIndex = 2 Then
                    RecalculateProratedTax()
                Else
                    UpdateTotals()
                End If
            End If
        End Sub

        Private Sub BtnClearSearch_Click(sender As Object, e As EventArgs) Handles btnClearSearch.Click
            For Each txt In filterTextBoxes.Values
                txt.Clear()
            Next
            ApplyFilters()
        End Sub

        Private Sub BtnSaveEntry_Click(sender As Object, e As EventArgs) Handles btnSaveEntry.Click
            If SaveCurrentInvoice() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End Sub

        Private Sub BtnSaveAndContinue_Click(sender As Object, e As EventArgs) Handles btnSaveAndContinue.Click
            If SaveCurrentInvoice() Then
                _editInvoiceId = Nothing
                txtEntryReference.Text = "PINV-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                txtVendorInvoiceNumber.Clear()
                txtEntryDescription.Clear()
                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.Clear()
                For i = 1 To TotalPreloadedRows
                    table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, 0D, 0D, "", 0, 0D, 0D, 0D)
                Next
                UpdateTotals()
            End If
        End Sub

        Private Function SaveCurrentInvoice() As Boolean
            Dim num = txtEntryReference.Text.Trim()
            If String.IsNullOrEmpty(num) Then
                MessageBox.Show("لطفاً شماره فاکتور خرید در سیستم را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Dim userId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
            Dim docDate = ParsePersianDate(txtDateSanad.Text.Trim())
            Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)

            If _defaultDocType = "رسید ورود به انبار" Then
                If Not _editInvoiceId.HasValue Then
                    MessageBox.Show("سند خرید مرجع یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
                Dim receiptLines As New List(Of Tuple(Of Integer, Integer, Decimal, Integer))()
                
                For i As Integer = 0 To table.Rows.Count - 1
                    Dim row = table.Rows(i)
                    If row.RowState <> DataRowState.Deleted Then
                        Dim rQty = Convert.ToDecimal(If(row.IsNull("ReceiptQuantity"), 0D, row("ReceiptQuantity")))
                        Dim remQty = Convert.ToDecimal(If(row.IsNull("RemainingQuantity"), 0D, row("RemainingQuantity")))
                        Dim pName = Convert.ToString(If(row.IsNull("ProductName"), "", row("ProductName")))
                        
                        If rQty > 0 AndAlso remQty < 0 Then
                            Dim msg = "با ذخیره این رسید ، مجموع تعداد رسید شده برای کالای " & pName & " ، بیشتر از تعداد خرید این کالا در فاکتور خرید می شود"
                            MessageBox.Show(msg, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Try
                                dgvEntryLines.CurrentCell = dgvEntryLines.Rows(i).Cells("colReceiptQty")
                                dgvEntryLines.BeginEdit(True)
                            Catch
                            End Try
                            Return False
                        End If
                    End If
                Next

                For Each row As DataRow In table.Rows
                    If row.RowState <> DataRowState.Deleted Then
                        Dim detailId = Convert.ToInt32(If(row.IsNull("DetailID"), 0, row("DetailID")))
                        Dim pid = Convert.ToInt32(If(row.IsNull("ProductID"), 0, row("ProductID")))
                        Dim rQty = Convert.ToDecimal(If(row.IsNull("ReceiptQuantity"), 0D, row("ReceiptQuantity")))
                        Dim wId = Convert.ToInt32(If(row.IsNull("WarehouseID") OrElse Convert.ToInt32(row("WarehouseID")) = 0, 1, row("WarehouseID")))
                        If pid > 0 AndAlso detailId > 0 AndAlso rQty > 0 Then
                            receiptLines.Add(Tuple.Create(detailId, pid, rQty, wId))
                        End If
                    End If
                Next
                If receiptLines.Count = 0 Then
                    MessageBox.Show("هیچ تعدادی جهت رسید وارد نشده است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                Try
                    If _editReceiptId.HasValue Then
                        _invoiceService.UpdateIndependentWarehouseReceipt(_editReceiptId.Value, _editInvoiceId.Value, num, docDate, userId, 1, txtEntryDescription.Text.Trim(), receiptLines)
                        MessageBox.Show("رسید انبار با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        _invoiceService.SaveIndependentWarehouseReceipt(_editInvoiceId.Value, num, docDate, userId, 1, txtEntryDescription.Text.Trim(), receiptLines)
                        MessageBox.Show("رسید انبار با موفقیت ثبت شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    MessageBox.Show("خطا در ثبت رسید انبار: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            End If

            If _defaultDocType = "برگشت از خرید" Then
                If Not _editInvoiceId.HasValue AndAlso Not _parentInvoiceId.HasValue Then
                    MessageBox.Show("سند خرید مرجع یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If

                For i As Integer = 0 To table.Rows.Count - 1
                    Dim row = table.Rows(i)
                    If row.RowState <> DataRowState.Deleted Then
                        Dim returnQty = Convert.ToDecimal(If(row.IsNull("ReturnQuantity"), 0D, row("ReturnQuantity")))
                        Dim invQty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                        Dim pName = Convert.ToString(If(row.IsNull("ProductName"), "", row("ProductName")))

                        If returnQty > invQty Then
                            Dim msg = "مجموع تعداد برگشت از خرید برای کالای «" & pName & "» بیشتر از تعداد این کالا در فاکتور خرید میباشند."
                            MessageBox.Show(msg, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Try
                                dgvEntryLines.CurrentCell = dgvEntryLines.Rows(i).Cells("colReturnQty")
                                dgvEntryLines.BeginEdit(True)
                            Catch
                            End Try
                            Return False
                        End If
                    End If
                Next
            End If

            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal, Decimal, Decimal))()
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted Then
                    Dim pid = Convert.ToInt32(If(row.IsNull("ProductID"), 0, row("ProductID")))
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    If pid > 0 AndAlso qty > 0 Then
                        Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                        Dim disc = Convert.ToDecimal(If(row.IsNull("Discount"), 0D, row("Discount")))
                        Dim vat = Convert.ToDecimal(If(row.IsNull("Vat"), 0D, row("Vat")))
                        lines.Add(Tuple.Create(pid, qty, price, disc, vat))
                    End If
                End If
            Next

            If lines.Count = 0 Then
                MessageBox.Show("حداقل یک سطر کالا با مقدار معتبر باید وارد شود.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Dim warehouseId = 1 ' انبار پیشفرض

            Dim taxModeIdx = cmbTaxEntryMode.SelectedIndex
            Dim totalVatVal As Decimal = 0D
            Decimal.TryParse(txtTotalVatInput.Text.Trim().Replace(",", ""), totalVatVal)

            Try
                If _editInvoiceId.HasValue Then
                    _invoiceService.UpdatePurchaseInvoice(_editInvoiceId.Value, num, docDate, lblSarfaslValue.Text, warehouseId, userId, lines, _defaultDocType, 0D, "نسیه", txtEntryDescription.Text.Trim(), taxModeIdx, totalVatVal)
                    ' بررسی بدهی خودکار
                    Dim totalPayable = CalculateGrandTotalSum()
                    _paymentService.EnsureAutoDebtIfNeeded(_editInvoiceId.Value, totalPayable)
                    MessageBox.Show("سند خرید با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim savedId = _invoiceService.SavePurchaseInvoice(num, docDate, lblSarfaslValue.Text, warehouseId, userId, lines, _defaultDocType, 0D, "نسیه", txtEntryDescription.Text.Trim(), taxModeIdx, totalVatVal)
                    ' بررسی بدهی خودکار
                    Dim totalPayable = CalculateGrandTotalSum()
                    _paymentService.EnsureAutoDebtIfNeeded(savedId, totalPayable)
                    MessageBox.Show("سند خرید با موفقیت ثبت شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return True
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت سند خرید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub DgvEntryLines_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
                If colName = "colBtnKala" Then
                    SelectProductForLine(e.RowIndex)
                ElseIf colName = "colBtnWarehouse" Then
                    SelectWarehouseForLine(e.RowIndex)
                ElseIf colName = "colBtnUnit" Then
                    SelectUnitForLine(e.RowIndex)
                End If
            End If
        End Sub

        Private Sub DgvEntryLines_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEntryLines.SelectionChanged
            If dgvEntryLines.CurrentRow IsNot Nothing Then
                Dim row = dgvEntryLines.CurrentRow
                Dim warehouseName = Convert.ToString(row.Cells("colWarehouse").Value)
                lblShenavarValue.Text = If(String.IsNullOrEmpty(warehouseName), "(انتخاب نشده)", warehouseName)
            End If
        End Sub

        Private Sub SelectProductForLine(rowIndex As Integer)
            Dim products = _catalogService.GetProducts()
            If products Is Nothing OrElse products.Rows.Count = 0 Then
                MessageBox.Show("هیچ کالایی در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب کالا"
                dlg.Size = New Size(550, 400)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True

                Dim grid As New DataGridView()
                grid.Dock = DockStyle.Fill
                grid.DataSource = products
                grid.ReadOnly = True
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                grid.MultiSelect = False
                grid.AllowUserToAddRows = False
                grid.RowHeadersVisible = False

                dlg.Controls.Add(grid)

                AddHandler grid.CellDoubleClick, Sub(s, ea)
                                                     If ea.RowIndex >= 0 Then
                                                         dlg.Tag = grid.Rows(ea.RowIndex).DataBoundItem
                                                         dlg.DialogResult = DialogResult.OK
                                                         dlg.Close()
                                                     End If
                                                 End Sub

                If dlg.ShowDialog() = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    Dim drv = DirectCast(dlg.Tag, DataRowView)
                    Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                    Dim r = table.Rows(rowIndex)

                    _isLoading = True
                    r("ProductID") = drv("ProductID")
                    r("ProductCode") = If(drv.Row.Table.Columns.Contains("ProductCode"), Convert.ToString(drv("ProductCode")), "")
                    r("ProductName") = drv("ProductName")
                    r("Unit") = If(drv.Row.Table.Columns.Contains("Unit"), Convert.ToString(drv("Unit")), "عدد")
                    r("UnitPrice") = If(drv.Row.Table.Columns.Contains("DefaultPrice"), Convert.ToDecimal(drv("DefaultPrice")), 0D)

                    If drv.Row.Table.Columns.Contains("DefaultWarehouseID") AndAlso Not drv.Row.IsNull("DefaultWarehouseID") Then
                        r("WarehouseID") = drv("DefaultWarehouseID")
                        r("WarehouseName") = If(drv.Row.Table.Columns.Contains("DefaultWarehouseName"), Convert.ToString(drv("DefaultWarehouseName")), "")
                    End If

                    Dim qty = Convert.ToDecimal(If(r.IsNull("Quantity"), 0D, r("Quantity")))
                    Dim price = Convert.ToDecimal(If(r.IsNull("UnitPrice"), 0D, r("UnitPrice")))
                    Dim disc = Convert.ToDecimal(If(r.IsNull("Discount"), 0D, r("Discount")))
                    Dim lineTot = qty * price
                    Dim lineTotAfterDisc = Math.Max(0D, lineTot - disc)

                    r("LineTotal") = lineTot
                    r("LineTotalAfterDiscount") = lineTotAfterDisc

                    If cmbTaxEntryMode.SelectedIndex <> 2 Then
                        Dim taxPct = If(drv.Row.Table.Columns.Contains("TaxPercent") AndAlso Not drv.Row.IsNull("TaxPercent"), Convert.ToDecimal(drv("TaxPercent")), 0D)
                        Dim vat = Math.Round(lineTotAfterDisc * taxPct / 100D, 0)
                        r("Vat") = If(vat < 0, 0D, vat)
                        r("TotalPrice") = lineTotAfterDisc + Convert.ToDecimal(r("Vat"))
                    End If

                    _isLoading = False

                    If cmbTaxEntryMode.SelectedIndex = 2 Then
                        RecalculateProratedTax()
                    Else
                        UpdateTotals()
                    End If
                End If
            End Using
        End Sub

        Private Sub SelectWarehouseForLine(rowIndex As Integer)
            Dim warehouses = _catalogService.GetWarehouses()
            If warehouses Is Nothing OrElse warehouses.Rows.Count = 0 Then
                MessageBox.Show("هیچ انباری برای شرکت جاری تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب انبار مقصد"
                dlg.Size = New Size(450, 350)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True

                Dim grid As New DataGridView()
                grid.Dock = DockStyle.Fill
                grid.DataSource = warehouses
                grid.ReadOnly = True
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                grid.MultiSelect = False
                grid.AllowUserToAddRows = False
                grid.RowHeadersVisible = False

                dlg.Controls.Add(grid)

                AddHandler grid.CellDoubleClick, Sub(s, ea)
                                                     If ea.RowIndex >= 0 Then
                                                         dlg.Tag = grid.Rows(ea.RowIndex).DataBoundItem
                                                         dlg.DialogResult = DialogResult.OK
                                                         dlg.Close()
                                                     End If
                                                 End Sub

                If dlg.ShowDialog() = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    Dim drv = DirectCast(dlg.Tag, DataRowView)
                    Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                    Dim r = table.Rows(rowIndex)
                    r("WarehouseID") = drv("WarehouseID")
                    r("WarehouseName") = drv("WarehouseName")
                    UpdateTotals()
                End If
            End Using
        End Sub

        Private Sub SelectUnitForLine(rowIndex As Integer)
            Dim uoms = _uomService.GetActive()
            If uoms Is Nothing OrElse uoms.Rows.Count = 0 Then
                MessageBox.Show("هیچ واحد اندازهگیری تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب واحد کالا"
                dlg.Size = New Size(400, 300)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True

                Dim grid As New DataGridView()
                grid.Dock = DockStyle.Fill
                grid.DataSource = uoms
                grid.ReadOnly = True
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                grid.MultiSelect = False
                grid.AllowUserToAddRows = False
                grid.RowHeadersVisible = False

                dlg.Controls.Add(grid)

                AddHandler grid.CellDoubleClick, Sub(s, ea)
                                                     If ea.RowIndex >= 0 Then
                                                         dlg.Tag = grid.Rows(ea.RowIndex).DataBoundItem
                                                         dlg.DialogResult = DialogResult.OK
                                                         dlg.Close()
                                                     End If
                                                 End Sub

                If dlg.ShowDialog() = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    Dim drv = DirectCast(dlg.Tag, DataRowView)
                    Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                    Dim r = table.Rows(rowIndex)
                    r("Unit") = drv("UoMName")
                End If
            End Using
        End Sub

        Private Sub BtnSelectVendor_Click(sender As Object, e As EventArgs) Handles btnSelectVendor.Click
            Using dlg As New Negar.Forms.Moshtarak.ShenavarTreePickerForm()
                If dlg.ShowDialog() = DialogResult.OK Then
                    SelectedVendorID = dlg.SelectedShenavarID
                    SelectedVendorCode = dlg.SelectedAccountCode
                    SelectedVendorName = dlg.SelectedAccountName
                    lblSarfaslValue.Text = SelectedVendorCode & " - " & SelectedVendorName
                End If
            End Using
        End Sub

        ' ─────────────────────────────────────────────
        '  کنترلها و منطق تب تسویه فاکتور خرید
        ' ─────────────────────────────────────────────
        Private dgvPayments As DataGridView
        Private lblTasviehSummary As Label
        Private btnAddPayment As Button

        Private Sub InitTasviehTabControls()
            If tabPageTasvieh Is Nothing OrElse dgvPayments IsNot Nothing Then Return

            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 45, .BackColor = Color.FromArgb(245, 248, 252)}
            lblTasviehSummary = New Label() With {.Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft,
                                                  .Font = New Font("Tahoma", 9, FontStyle.Bold), .ForeColor = Color.DarkBlue,
                                                  .Padding = New Padding(10, 0, 0, 0)}
            btnAddPayment = New Button() With {.Text = "+ ثبت پرداخت جدید", .Dock = DockStyle.Right, .Width = 140,
                                               .BackColor = Color.FromArgb(0, 120, 215), .ForeColor = Color.White,
                                               .FlatStyle = FlatStyle.Flat, .Font = New Font("Tahoma", 9, FontStyle.Bold)}
            AddHandler btnAddPayment.Click, AddressOf BtnAddPayment_Click
            pnlTop.Controls.Add(lblTasviehSummary)
            pnlTop.Controls.Add(btnAddPayment)

            dgvPayments = New DataGridView() With {.Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
                                                   .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                                   .BackgroundColor = Color.White, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                                                   .RightToLeft = RightToLeft.Yes}
            
            ' ستونهای دکمهای حذف و ویرایش پرداخت
            If Not dgvPayments.Columns.Contains("colEditPay") Then
                Dim colEdit As New DataGridViewButtonColumn() With {.Name = "colEditPay", .HeaderText = "ویرایش", .Text = "ویرایش", .UseColumnTextForButtonValue = True, .Width = 65}
                dgvPayments.Columns.Add(colEdit)
            End If
            If Not dgvPayments.Columns.Contains("colDeletePay") Then
                Dim colDelete As New DataGridViewButtonColumn() With {.Name = "colDeletePay", .HeaderText = "حذف", .Text = "حذف", .UseColumnTextForButtonValue = True, .Width = 60}
                dgvPayments.Columns.Add(colDelete)
            End If

            AddHandler dgvPayments.CellContentClick, AddressOf DgvPayments_CellContentClick

            tabPageTasvieh.Controls.Add(dgvPayments)
            tabPageTasvieh.Controls.Add(pnlTop)

            LoadInvoicePayments()
        End Sub

        Private Sub LoadInvoicePayments()
            If Not _editInvoiceId.HasValue OrElse dgvPayments Is Nothing Then
                lblTasviehSummary.Text = "برای ثبت و مشاهده پرداختها، ابتدا فاکتور خرید را ذخیره کنید."
                btnAddPayment.Enabled = _editInvoiceId.HasValue
                Return
            End If

            btnAddPayment.Enabled = True
            Dim dt = _paymentService.GetPaymentsForInvoice(_editInvoiceId.Value)
            
            ' پر کردن گرید (نگهداشتن ستونهای دکمهای در ابتدای گرید)
            Dim colEdit = dgvPayments.Columns("colEditPay")
            Dim colDel = dgvPayments.Columns("colDeletePay")
            dgvPayments.DataSource = dt
            If Not dgvPayments.Columns.Contains("colEditPay") AndAlso colEdit IsNot Nothing Then
                dgvPayments.Columns.Insert(0, colEdit)
            End If
            If Not dgvPayments.Columns.Contains("colDeletePay") AndAlso colDel IsNot Nothing Then
                dgvPayments.Columns.Insert(1, colDel)
            End If

            ' نام ستونهای فارسی
            Dim colNames = New Dictionary(Of String, String) From {
                {"PaymentID", "شناسه"}, {"PaymentDate", "تاریخ پرداخت"}, {"PaymentType", "نوع پرداخت"},
                {"Amount", "مبلغ (ریال)"}, {"DueDate", "سررسید بدهی"}, {"Description", "شرح"},
                {"CheckNumber", "شماره چک"}, {"BankName", "بانک"}, {"CheckStatus", "وضعیت چک"},
                {"CreatedBy", "ثبت کننده"}, {"CreatedAt", "زمان ثبت"}}
            For Each col As DataGridViewColumn In dgvPayments.Columns
                If colNames.ContainsKey(col.Name) Then col.HeaderText = colNames(col.Name)
                If col.Name = "Amount" Then col.DefaultCellStyle.Format = "N0"
            Next

            ' بهروزرسانی خلاصه
            Dim grandTotal = CalculateGrandTotalSum()
            Dim totalPaid = _paymentService.GetTotalPaid(_editInvoiceId.Value)
            Dim remaining = grandTotal - totalPaid
            lblTasviehSummary.Text = $"جمع فاکتور: {grandTotal:N0} ریال   |   مجموع پرداختی: {totalPaid:N0} ریال   |   مانده قابل پرداخت: {remaining:N0} ریال"

            UpdateHeaderSettlementStatus()
        End Sub

        Private Sub UpdateHeaderSettlementStatus()
            If lblSanadStatus Is Nothing Then Return
            Dim grandTotal = CalculateGrandTotalSum()
            Dim invId = If(_editInvoiceId.HasValue, _editInvoiceId.Value, 0)
            Dim statusInfo = _paymentService.GetSettlementStatus(invId, grandTotal)

            lblSanadStatus.Text = statusInfo.StatusText
            lblSanadStatus.ForeColor = statusInfo.TextColor
        End Sub

        Private Sub BtnAddPayment_Click(sender As Object, e As EventArgs)
            If Not _editInvoiceId.HasValue Then Return
            Dim grandTotal = CalculateGrandTotalSum()
            Dim totalPaid = _paymentService.GetTotalPaid(_editInvoiceId.Value)
            Using dlg As New PaymentEntryForm(_editInvoiceId.Value, grandTotal, totalPaid)
                If dlg.ShowDialog() = DialogResult.OK Then
                    LoadInvoicePayments()
                End If
            End Using
        End Sub

        Private Sub DgvPayments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                Dim colName = dgvPayments.Columns(e.ColumnIndex).Name
                Dim payId = Convert.ToInt32(dgvPayments.Rows(e.RowIndex).Cells("PaymentID").Value)

                If colName = "colEditPay" Then
                    Dim grandTotal = CalculateGrandTotalSum()
                    Dim totalPaid = _paymentService.GetTotalPaid(_editInvoiceId.Value)
                    Using dlg As New PaymentEntryForm(_editInvoiceId.Value, grandTotal, totalPaid, payId)
                        If dlg.ShowDialog() = DialogResult.OK Then
                            LoadInvoicePayments()
                        End If
                    End Using
                ElseIf colName = "colDeletePay" Then
                    If MessageBox.Show("آیا از حذف این پرداخت اطمینان دارید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        _paymentService.DeletePayment(payId)
                        LoadInvoicePayments()
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
