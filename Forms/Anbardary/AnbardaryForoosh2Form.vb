Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Business.PersianDateHelper
Imports Sys_Hes_Anb.Forms.Moshtarak
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryForoosh2Form
        Inherits Form

        Private ReadOnly _catalogService As New CatalogService()
        Private ReadOnly _invoiceService As New InvoiceService()
        Private ReadOnly _uomService As New UnitOfMeasureService()
        Private _editInvoiceId As Integer? = Nothing
        Private _defaultDocType As String = "فاکتور فروش"
        Private _isLoading As Boolean = False
        Private _isDirty As Boolean = False
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const TotalPreloadedRows As Integer = 30

        Public Property SelectedCustomerID As Integer?
        Public Property SelectedCustomerCode As String
        Public Property SelectedCustomerName As String

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

        Private Sub AnbardaryForoosh2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            Me.WindowState = FormWindowState.Maximized
            _isLoading = True

            If Me.dgvEntryLines IsNot Nothing Then
                Me.dgvEntryLines.CellBorderStyle = DataGridViewCellBorderStyle.Single
                Me.dgvEntryLines.GridColor = Color.FromArgb(200, 210, 225)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Me.dgvEntryLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.dgvEntryLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
                Me.dgvEntryLines.DefaultCellStyle.SelectionForeColor = Color.White
                Me.dgvEntryLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 242, 248)
            End If

            InitializeEntryGrid()
            CreateFilterTextBoxes()

            AddHandler dgvEntryLines.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvEntryLines.Scroll, AddressOf DgvEntryLines_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes
            AddHandler cmbTaxEntryMode.SelectedIndexChanged, AddressOf cmbTaxEntryMode_SelectedIndexChanged

            If _editInvoiceId.HasValue Then
                Me.Text = "ویرایش " & _defaultDocType
                LoadInvoiceForEdit(_editInvoiceId.Value)
                btnSaveAndContinue.Visible = False
            Else
                Me.Text = "ثبت " & _defaultDocType & " جدید"
                Dim prefix = "SINV-"
                txtEntryReference.Text = prefix & DateTime.Now.ToString("yyyyMMddHHmmss")
                txtDateSanad.Text = ToPersian(DateTime.Today)
                btnSaveAndContinue.Visible = True
            End If

            cmbTaxEntryMode.SelectedIndex = 0

            UpdateTotals()
            AlignSearchBoxes()
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
            table.Columns.Add("Discount", GetType(Decimal))
            table.Columns.Add("Vat", GetType(Decimal))
            table.Columns.Add("TotalPrice", GetType(Decimal))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("DetailID", GetType(Integer))
            table.Columns.Add("TaxPercent", GetType(Decimal))

            For i = 1 To TotalPreloadedRows
                table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, "", 0, 0D)
            Next

            AddHandler table.ColumnChanged, AddressOf Table_ColumnChanged

            dgvEntryLines.AutoGenerateColumns = False
            dgvEntryLines.ReadOnly = False
            dgvEntryLines.DataSource = table
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
            colBtnKala.Width = 80
            colBtnKala.FlatStyle = FlatStyle.Standard

            Dim colKalaCode As New DataGridViewTextBoxColumn()
            colKalaCode.Name = "colKalaCode"
            colKalaCode.DataPropertyName = "ProductCode"
            colKalaCode.HeaderText = "کد / بارکد"
            colKalaCode.Width = 100

            Dim colKalaName As New DataGridViewTextBoxColumn()
            colKalaName.Name = "colKalaName"
            colKalaName.DataPropertyName = "ProductName"
            colKalaName.HeaderText = "نام کالا / شرح ردیف"
            colKalaName.Width = 240

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
            colWarehouse.HeaderText = "انبار مبدا"
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
            colUnit.Width = 80

            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.Name = "colQty"
            colQty.DataPropertyName = "Quantity"
            colQty.HeaderText = "تعداد / مقدار"
            colQty.Width = 100
            colQty.DefaultCellStyle.Format = "N2"
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colUnitPrice As New DataGridViewTextBoxColumn()
            colUnitPrice.Name = "colUnitPrice"
            colUnitPrice.DataPropertyName = "UnitPrice"
            colUnitPrice.HeaderText = "فی (قیمت واحد)"
            colUnitPrice.Width = 120
            colUnitPrice.DefaultCellStyle.Format = "N0"
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colDiscount As New DataGridViewTextBoxColumn()
            colDiscount.Name = "colDiscount"
            colDiscount.DataPropertyName = "Discount"
            colDiscount.HeaderText = "تخفیف سطر"
            colDiscount.Width = 110
            colDiscount.DefaultCellStyle.Format = "N0"
            colDiscount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colTaxPercent As New DataGridViewTextBoxColumn()
            colTaxPercent.Name = "colTaxPercent"
            colTaxPercent.DataPropertyName = "TaxPercent"
            colTaxPercent.HeaderText = "% مالیات و عوارض"
            colTaxPercent.Width = 100
            colTaxPercent.DefaultCellStyle.Format = "N2"
            colTaxPercent.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colVat As New DataGridViewTextBoxColumn()
            colVat.Name = "colVat"
            colVat.DataPropertyName = "Vat"
            colVat.HeaderText = "مالیات و عوارض"
            colVat.Width = 120
            colVat.DefaultCellStyle.Format = "N0"
            colVat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colTotalPrice As New DataGridViewTextBoxColumn()
            colTotalPrice.Name = "colTotalPrice"
            colTotalPrice.DataPropertyName = "TotalPrice"
            colTotalPrice.HeaderText = "مبلغ خالص"
            colTotalPrice.Width = 140
            colTotalPrice.DefaultCellStyle.Format = "N0"
            colTotalPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.Width = 160

            dgvEntryLines.Columns.AddRange(New DataGridViewColumn() {
                colLineNo, colBtnKala, colKalaCode, colKalaName, colBtnWarehouse, colWarehouse,
                colBtnUnit, colUnit, colQty, colUnitPrice, colDiscount, colTaxPercent, colVat, colTotalPrice, colDesc
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
                        
                        Dim taxPct = If(dr.IsNull("TaxPercent"), 0D, Convert.ToDecimal(dr("TaxPercent")))
                        e.Row("TaxPercent") = taxPct

                        If cmbTaxEntryMode.SelectedItem IsNot Nothing AndAlso cmbTaxEntryMode.SelectedItem.ToString() = "ورود بصورت سیستمی" Then
                            Dim qty = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                            Dim price = Convert.ToDecimal(If(e.Row.IsNull("UnitPrice"), 0D, e.Row("UnitPrice")))
                            Dim disc = Convert.ToDecimal(If(e.Row.IsNull("Discount"), 0D, e.Row("Discount")))
                            Dim vat = Math.Round(((qty * price) - disc) * taxPct / 100D, 0)
                            If vat < 0 Then vat = 0D
                            e.Row("Vat") = vat
                        End If
                        
                        _isLoading = False
                        
                        Dim qtyTrigger = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                        Dim priceTrigger = Convert.ToDecimal(If(e.Row.IsNull("UnitPrice"), 0D, e.Row("UnitPrice")))
                        Dim discTrigger = Convert.ToDecimal(If(e.Row.IsNull("Discount"), 0D, e.Row("Discount")))
                        Dim vatTrigger = Convert.ToDecimal(If(e.Row.IsNull("Vat"), 0D, e.Row("Vat")))
                        Dim net = (qtyTrigger * priceTrigger) - discTrigger + vatTrigger
                        If net < 0 Then net = 0D
                        e.Row("TotalPrice") = net
                        UpdateTotals()
                    Else
                        _isLoading = True
                        e.Row("ProductID") = 0
                        e.Row("ProductName") = "کالای نامشخص"
                        _isLoading = False
                    End If
                End If
            End If

            If e.Column.ColumnName = "Quantity" OrElse e.Column.ColumnName = "UnitPrice" OrElse e.Column.ColumnName = "Discount" Then
                Dim qty = Convert.ToDecimal(If(e.Row.IsNull("Quantity"), 0D, e.Row("Quantity")))
                Dim price = Convert.ToDecimal(If(e.Row.IsNull("UnitPrice"), 0D, e.Row("UnitPrice")))
                Dim disc = Convert.ToDecimal(If(e.Row.IsNull("Discount"), 0D, e.Row("Discount")))
                Dim lineTotal = (qty * price) - disc
                
                If cmbTaxEntryMode.SelectedItem IsNot Nothing AndAlso cmbTaxEntryMode.SelectedItem.ToString() = "ورود بصورت سیستمی" Then
                    Dim taxPct = Convert.ToDecimal(If(e.Row.IsNull("TaxPercent"), 0D, e.Row("TaxPercent")))
                    Dim vat = Math.Round(lineTotal * taxPct / 100D, 0)
                    If vat < 0 Then vat = 0D
                    
                    _isLoading = True
                    e.Row("Vat") = vat
                    Dim net = lineTotal + vat
                    If net < 0 Then net = 0D
                    e.Row("TotalPrice") = net
                    _isLoading = False
                    UpdateTotals()
                Else
                    Dim vat = Convert.ToDecimal(If(e.Row.IsNull("Vat"), 0D, e.Row("Vat")))
                    Dim net = lineTotal + vat
                    If net < 0 Then net = 0D
                    e.Row("TotalPrice") = net
                    UpdateTotals()
                End If
            End If
        End Sub

        Private Sub cmbTaxEntryMode_SelectedIndexChanged(sender As Object, e As EventArgs)
            If _isLoading Then Return
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return
            
            If cmbTaxEntryMode.SelectedItem.ToString() = "ورود بصورت دستی" Then
                dgvEntryLines.Columns("colTaxPercent").ReadOnly = False
                dgvEntryLines.Columns("colVat").ReadOnly = False
            Else
                dgvEntryLines.Columns("colTaxPercent").ReadOnly = True
                dgvEntryLines.Columns("colVat").ReadOnly = True
                
                For Each row As DataRow In table.Rows
                    If Not row.IsNull("Quantity") Then
                        Dim taxPct = Convert.ToDecimal(If(row.IsNull("TaxPercent"), 0D, row("TaxPercent")))
                        Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                        Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                        Dim disc = Convert.ToDecimal(If(row.IsNull("Discount"), 0D, row("Discount")))
                        Dim vat = Math.Round(((qty * price) - disc) * taxPct / 100D, 0)
                        row("Vat") = If(vat < 0, 0D, vat)
                    End If
                Next
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

        Private Sub DgvEntryLines_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxes()
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvEntryLines Is Nothing OrElse dgvEntryLines.Columns.Count = 0 OrElse pnlSerch Is Nothing Then Return

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

            if filters.Count > 0 Then
                table.DefaultView.RowFilter = String.Join(" AND ", filters)
            Else
                table.DefaultView.RowFilter = ""
            End If
            UpdateTotals()
        End Sub

        Private Sub UpdateTotals()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            Dim subTotal As Decimal = 0D
            Dim totalDiscount As Decimal = 0D
            Dim totalVat As Decimal = 0D
            Dim netTotal As Decimal = 0D

            Dim activeRows = table.Select(table.DefaultView.RowFilter)
            For Each row In activeRows
                If row.RowState <> DataRowState.Deleted Then
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                    Dim disc = Convert.ToDecimal(If(row.IsNull("Discount"), 0D, row("Discount")))
                    Dim vat = Convert.ToDecimal(If(row.IsNull("Vat"), 0D, row("Vat")))

                    subTotal += (qty * price)
                    totalDiscount += disc
                    totalVat += vat
                    netTotal += (qty * price) - disc + vat
                End If
            Next

            txtJamBedehkar.Text = subTotal.ToString("N0")
            txtJamBestankar.Text = totalDiscount.ToString("N0")
            txtKasriDebit.Text = netTotal.ToString("N0")
        End Sub

        Private Sub LoadInvoiceForEdit(invoiceId As Integer)
            Try
                Dim hdr = _invoiceService.GetSalesInvoiceById(invoiceId)
                If hdr Is Nothing Then
                    MessageBox.Show("سند مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Return
                End If

                txtEntryReference.Text = Convert.ToString(hdr("InvoiceNumber"))
                txtVendorInvoiceNumber.Text = If(hdr.Table.Columns.Contains("VendorInvoiceNumber"), Convert.ToString(hdr("VendorInvoiceNumber")), "")

                If Not Convert.IsDBNull(hdr("InvoiceDate")) Then
                    txtDateSanad.Text = ToPersian(Convert.ToDateTime(hdr("InvoiceDate")))
                End If

                lblSarfaslValue.Text = Convert.ToString(hdr("CustomerName"))
                txtEntryDescription.Text = Convert.ToString(hdr("Description"))

                Dim dtDetails = _invoiceService.GetSalesInvoiceDetails(invoiceId)
                
                Dim dtProducts = Sql.ExecuteTable("SELECT p.ProductID, p.ProductCode, p.DefaultWarehouseID, w.WarehouseName AS DefaultWarehouseName, p.TaxPercent FROM Products p LEFT JOIN Warehouses w ON p.DefaultWarehouseID = w.WarehouseID")
                Dim prodInfo As New Dictionary(Of Integer, DataRow)()
                For Each pRow As DataRow In dtProducts.Rows
                    prodInfo(Convert.ToInt32(pRow("ProductID"))) = pRow
                Next

                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.Clear()

                Dim lineNo As Integer = 1
                For Each dRow As DataRow In dtDetails.Rows
                    Dim row = table.NewRow()
                    row("LineNumber") = lineNo
                    row("ProductID") = dRow("ProductID")
                    row("Quantity") = dRow("Quantity")
                    row("UnitPrice") = dRow("UnitPrice")
                    row("Discount") = If(dRow.Table.Columns.Contains("Discount"), dRow("Discount"), 0D)
                    
                    Dim pid = Convert.ToInt32(dRow("ProductID"))
                    If prodInfo.ContainsKey(pid) Then
                        Dim pRow = prodInfo(pid)
                        row("ProductCode") = If(pRow.IsNull("ProductCode"), "", pRow("ProductCode"))
                        row("ProductName") = dRow("ProductName")
                        row("Unit") = If(dRow.Table.Columns.Contains("Unit"), dRow("Unit"), "عدد")
                        row("WarehouseID") = If(pRow.IsNull("DefaultWarehouseID"), 0, pRow("DefaultWarehouseID"))
                        row("WarehouseName") = If(pRow.IsNull("DefaultWarehouseName"), "", pRow("DefaultWarehouseName"))
                        row("TaxPercent") = If(pRow.IsNull("TaxPercent"), 0D, pRow("TaxPercent"))
                    Else
                        row("ProductCode") = ""
                        row("ProductName") = dRow("ProductName")
                        row("Unit") = "عدد"
                        row("WarehouseID") = 0
                        row("WarehouseName") = ""
                        row("TaxPercent") = 0D
                    End If

                    Dim qty = Convert.ToDecimal(row("Quantity"))
                    Dim price = Convert.ToDecimal(row("UnitPrice"))
                    Dim disc = Convert.ToDecimal(row("Discount"))
                    Dim taxPct = Convert.ToDecimal(row("TaxPercent"))
                    Dim vat = Math.Round(((qty * price) - disc) * taxPct / 100D, 0)
                    row("Vat") = If(vat < 0, 0D, vat)
                    row("TotalPrice") = (qty * price) - disc + vat
                    
                    row("DetailID") = dRow("DetailID")
                    table.Rows.Add(row)
                    lineNo += 1
                Next

                UpdateTotals()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnAddLine_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
            Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
            Dim lineNo = table.Rows.Count + 1
            table.Rows.Add(lineNo, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, "", 0, 0D)
        End Sub

        Private Sub BtnDeleteRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
            If dgvEntryLines.CurrentRow IsNot Nothing Then
                dgvEntryLines.Rows.Remove(dgvEntryLines.CurrentRow)
                UpdateTotals()
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
                txtEntryReference.Text = "SINV-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                txtVendorInvoiceNumber.Clear()
                txtEntryDescription.Clear()
                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.Clear()
                For i = 1 To TotalPreloadedRows
                    table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, 0D, 0D, 0D, "", 0, 0D)
                Next
                UpdateTotals()
            End If
        End Sub

        Private Function SaveCurrentInvoice() As Boolean
            Dim num = txtEntryReference.Text.Trim()
            If String.IsNullOrEmpty(num) Then
                MessageBox.Show("لطفاً شماره فاکتور فروش را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()

            ' 1. اعتبارسنجی انبار و موجودی برای هر سطر پیش از ثبت
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted Then
                    Dim pid = Convert.ToInt32(If(row.IsNull("ProductID"), 0, row("ProductID")))
                    Dim qty = Convert.ToDecimal(If(row.IsNull("Quantity"), 0D, row("Quantity")))
                    
                    If pid > 0 AndAlso qty > 0 Then
                        Dim warehouseId = Convert.ToInt32(If(row.IsNull("WarehouseID"), 1, row("WarehouseID")))
                        If warehouseId <= 0 Then warehouseId = 1
                        
                        Dim availableQtyVal = Sys_Hes_Anb.Data.Sql.ExecuteScalar("SELECT Quantity FROM Inventory WHERE ProductID = ? AND WarehouseID = ?", pid, warehouseId)
                        Dim availableQty As Decimal = 0D
                        If availableQtyVal IsNot Nothing AndAlso Not Convert.IsDBNull(availableQtyVal) Then
                            availableQty = Convert.ToDecimal(availableQtyVal)
                        End If

                        If _editInvoiceId.HasValue Then
                            Dim oldQtyVal = Sys_Hes_Anb.Data.Sql.ExecuteScalar("SELECT Quantity FROM SalesInvoiceDetails WHERE InvoiceID = ? AND ProductID = ?", _editInvoiceId.Value, pid)
                            If oldQtyVal IsNot Nothing AndAlso Not Convert.IsDBNull(oldQtyVal) Then
                                availableQty += Convert.ToDecimal(oldQtyVal)
                            End If
                        End If

                        If qty > availableQty Then
                            Dim prodName = Convert.ToString(row("ProductName"))
                            MessageBox.Show(String.Format("موجودی کالا '{0}' در انبار کافی نیست. موجودی فعلی: {1}", prodName, availableQty.ToString("N2")), "کسری موجودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return False
                        End If

                        Dim price = Convert.ToDecimal(If(row.IsNull("UnitPrice"), 0D, row("UnitPrice")))
                        lines.Add(Tuple.Create(pid, qty, price))
                    End If
                End If
            Next

            If lines.Count = 0 Then
                MessageBox.Show("حداقل یک سطر کالا با مقدار معتبر باید وارد شود.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Dim userId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
            Dim defaultWarehouseId = 1
            Dim docDate = ParsePersianDate(txtDateSanad.Text.Trim())

            Try
                If _editInvoiceId.HasValue Then
                    _invoiceService.UpdateSalesInvoice(_editInvoiceId.Value, num, docDate, lblSarfaslValue.Text, defaultWarehouseId, userId, lines)
                    MessageBox.Show("فاکتور فروش با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _invoiceService.SaveSalesInvoice(num, docDate, lblSarfaslValue.Text, defaultWarehouseId, userId, lines)
                    MessageBox.Show("فاکتور فروش با موفقیت ثبت شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return True
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور فروش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

                    Dim taxPct = If(drv.Row.Table.Columns.Contains("TaxPercent") AndAlso Not drv.Row.IsNull("TaxPercent"), Convert.ToDecimal(drv("TaxPercent")), 0D)
                    r("TaxPercent") = taxPct

                    If cmbTaxEntryMode.SelectedItem IsNot Nothing AndAlso cmbTaxEntryMode.SelectedItem.ToString() = "ورود بصورت سیستمی" Then
                        Dim qty = Convert.ToDecimal(If(r.IsNull("Quantity"), 0D, r("Quantity")))
                        Dim price = Convert.ToDecimal(If(r.IsNull("UnitPrice"), 0D, r("UnitPrice")))
                        Dim disc = Convert.ToDecimal(If(r.IsNull("Discount"), 0D, r("Discount")))
                        Dim vat = Math.Round(((qty * price) - disc) * taxPct / 100D, 0)
                        If vat < 0 Then vat = 0D
                        r("Vat") = vat
                    End If
                    
                    _isLoading = False
                    
                    Dim qtyVal = Convert.ToDecimal(If(r.IsNull("Quantity"), 0D, r("Quantity")))
                    Dim priceVal = Convert.ToDecimal(If(r.IsNull("UnitPrice"), 0D, r("UnitPrice")))
                    Dim discVal = Convert.ToDecimal(If(r.IsNull("Discount"), 0D, r("Discount")))
                    Dim vatVal = Convert.ToDecimal(If(r.IsNull("Vat"), 0D, r("Vat")))
                    Dim net = (qtyVal * priceVal) - discVal + vatVal
                    If net < 0 Then net = 0D
                    r("TotalPrice") = net

                    UpdateTotals()
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
                dlg.Text = "انتخاب انبار مبدا"
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
                MessageBox.Show("هیچ واحد اندازه‌گیری تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            Using dlg As New Sys_Hes_Anb.Forms.Moshtarak.ShenavarTreePickerForm()
                If dlg.ShowDialog() = DialogResult.OK Then
                    SelectedCustomerID = dlg.SelectedShenavarID
                    SelectedCustomerCode = dlg.SelectedAccountCode
                    SelectedCustomerName = dlg.SelectedAccountName
                    lblSarfaslValue.Text = SelectedCustomerCode & " - " & SelectedCustomerName
                End If
            End Using
        End Sub

        Private Sub DgvEntryLines_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellEndEdit
            If e.RowIndex < 0 Then Return
            
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
            Dim row = dgvEntryLines.Rows(e.RowIndex)
            Dim drv = TryCast(row.DataBoundItem, DataRowView)
            If drv Is Nothing Then Return
            
            Dim r = drv.Row
            Dim qty = Convert.ToDecimal(If(r.IsNull("Quantity"), 0D, r("Quantity")))
            Dim price = Convert.ToDecimal(If(r.IsNull("UnitPrice"), 0D, r("UnitPrice")))
            Dim disc = Convert.ToDecimal(If(r.IsNull("Discount"), 0D, r("Discount")))
            Dim lineTotal = (qty * price) - disc
            
            If cmbTaxEntryMode.SelectedItem IsNot Nothing AndAlso cmbTaxEntryMode.SelectedItem.ToString() = "ورود بصورت دستی" Then
                If colName = "colTaxPercent" Then
                    Dim taxPct = Convert.ToDecimal(If(r.IsNull("TaxPercent"), 0D, r("TaxPercent")))
                    Dim vat = Math.Round(lineTotal * taxPct / 100D, 0)
                    If vat < 0 Then vat = 0D
                    
                    _isLoading = True
                    r("Vat") = vat
                    Dim net = lineTotal + vat
                    If net < 0 Then net = 0D
                    r("TotalPrice") = net
                    _isLoading = False
                    
                    UpdateTotals()
                    
                ElseIf colName = "colVat" Then
                    Dim vat = Convert.ToDecimal(If(r.IsNull("Vat"), 0D, r("Vat")))
                    Dim taxPct = 0D
                    If lineTotal > 0 Then
                        taxPct = Math.Round((vat / lineTotal) * 100D, 2)
                    End If
                    
                    _isLoading = True
                    r("TaxPercent") = taxPct
                    Dim net = lineTotal + vat
                    If net < 0 Then net = 0D
                    r("TotalPrice") = net
                    _isLoading = False
                    
                    UpdateTotals()
                End If
            End If
        End Sub
    End Class
End Namespace
