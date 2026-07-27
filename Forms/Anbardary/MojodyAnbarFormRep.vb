Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Business.PersianDateHelper
Imports Negar.Data

Namespace Negar.Forms
    Partial Class MojodyAnbarFormRep
        Inherits Form

        Private ReadOnly inventoryService As New InventoryService()
        Private ReadOnly catalogService As New CatalogService()
        Private _inventoryTable As DataTable
        Private _kardexTable As DataTable
        Private _invCountTable As DataTable
        Private _printDoc As New PrintDocument()
        Private _printLines As New System.Collections.Generic.List(Of String)()
        Private _printLineIndex As Integer = 0
        Private Const LinesPerPage As Integer = 45

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MojodyAnbarFormRep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)

            ApplyGridStyle(dgvInventory)
            ApplyGridStyle(dgvKardex)
            ApplyGridStyle(dgvInvCount)

            ConfigureInventoryGrid()
            ConfigureKardexGrid()

            ' بارگذاری انبارها برای کمبو باکس‌ها
            Dim warehouses = catalogService.GetWarehouses()

            Dim warehousesWithAll = warehouses.Copy()
            Dim allRow = warehousesWithAll.NewRow()
            allRow("WarehouseID") = 0
            allRow("WarehouseName") = "همه انبارها"
            warehousesWithAll.Rows.InsertAt(allRow, 0)

            cmbWarehouse.DataSource = warehousesWithAll
            cmbWarehouse.DisplayMember = "WarehouseName"
            cmbWarehouse.ValueMember = "WarehouseID"
            cmbWarehouse.SelectedIndex = 0

            cmbKardexWarehouse.DataSource = warehousesWithAll.Copy()
            cmbKardexWarehouse.DisplayMember = "WarehouseName"
            cmbKardexWarehouse.ValueMember = "WarehouseID"
            cmbKardexWarehouse.SelectedIndex = 0

            cmbInvCountWarehouse.DataSource = warehousesWithAll.Copy()
            cmbInvCountWarehouse.DisplayMember = "WarehouseName"
            cmbInvCountWarehouse.ValueMember = "WarehouseID"
            cmbInvCountWarehouse.SelectedIndex = 0

            ' بارگذاری کالاها برای کاردکس
            LoadKardexProducts()

            ' تاریخ پیشفرض فیلتر کاردکس
            txtKardexFrom.Text = ""
            txtKardexTo.Text = ""

            LoadInventory()

            AddHandler _printDoc.PrintPage, AddressOf PrintDoc_PrintPage
        End Sub

        Private Sub ApplyGridStyle(grid As DataGridView)
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single
            grid.GridColor = Color.FromArgb(200, 220, 235)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 100, 160)
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            grid.EnableHeadersVisualStyles = False
            grid.RowHeadersVisible = False
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255)
            grid.DefaultCellStyle.Font = New Font("Tahoma", 9.0!)
            grid.AllowUserToAddRows = False
            grid.ReadOnly = True
        End Sub

        Private Sub ConfigureInventoryGrid()
            dgvInventory.AutoGenerateColumns = False
            dgvInventory.Columns.Clear()

            Dim cols() As Object = {
                New With {.Name = "colCode", .Prop = "ProductCode", .Header = "کد کالا", .Width = 90, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "", .AutoFit = False},
                New With {.Name = "colName", .Prop = "ProductName", .Header = "نام کالا", .Width = 200, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "", .AutoFit = False},
                New With {.Name = "colWarehouse", .Prop = "WarehouseName", .Header = "انبار", .Width = 130, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "", .AutoFit = False},
                New With {.Name = "colTotalIn", .Prop = "TotalInput", .Header = "ورودی", .Width = 80, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colTotalOut", .Prop = "TotalOutput", .Header = "خروجی", .Width = 80, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colQty", .Prop = "Quantity", .Header = "موجودی", .Width = 80, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colAvgCost", .Prop = "AverageCost", .Header = "میانگین بهای تمام‌شده", .Width = 140, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "N0", .AutoFit = False},
                New With {.Name = "colTotalValue", .Prop = "TotalValue", .Header = "بهای تمام شده موجودی", .Width = 160, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "N0", .AutoFit = False},
                New With {.Name = "colLastUpdate", .Prop = "PersianLastUpdate", .Header = "آخرین به‌روزرسانی", .Width = 140, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "", .AutoFit = True}
            }

            For Each c In cols
                Dim col As New DataGridViewTextBoxColumn()
                col.Name = c.Name
                col.DataPropertyName = c.Prop
                col.HeaderText = c.Header
                col.Width = c.Width
                col.DefaultCellStyle.Alignment = c.Align
                If Not String.IsNullOrEmpty(c.Format) Then
                    col.DefaultCellStyle.Format = c.Format
                End If
                If c.AutoFit Then
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                End If
                col.ReadOnly = True
                dgvInventory.Columns.Add(col)
            Next

            dgvInventory.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub ConfigureKardexGrid()
            dgvKardex.AutoGenerateColumns = False
            dgvKardex.Columns.Clear()

            dgvKardex.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvKardex.ColumnHeadersHeight = 44
            dgvKardex.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            Dim rowNum As New DataGridViewTextBoxColumn()
            rowNum.Name = "colRowNum"
            rowNum.HeaderText = "ردیف"
            rowNum.Width = 45
            rowNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            rowNum.ReadOnly = True

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 105
            colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "colWarehouse"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار"
            colWarehouse.Width = 120
            colWarehouse.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "colType"
            colType.DataPropertyName = "TransactionType"
            colType.HeaderText = "نوع عملیات"
            colType.Width = 140
            colType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colIn As New DataGridViewTextBoxColumn()
            colIn.Name = "colIn"
            colIn.DataPropertyName = "QuantityIn"
            colIn.HeaderText = "تعداد" & vbCrLf & "وارده"
            colIn.Width = 75
            colIn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colIn.DefaultCellStyle.Format = "N0"

            Dim colOut As New DataGridViewTextBoxColumn()
            colOut.Name = "colOut"
            colOut.DataPropertyName = "QuantityOut"
            colOut.HeaderText = "تعداد" & vbCrLf & "صادره"
            colOut.Width = 75
            colOut.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colOut.DefaultCellStyle.Format = "N0"

            Dim colBalance As New DataGridViewTextBoxColumn()
            colBalance.Name = "colBalance"
            colBalance.DataPropertyName = "Balance"
            colBalance.HeaderText = "تعداد" & vbCrLf & "موجودی"
            colBalance.Width = 80
            colBalance.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colBalance.DefaultCellStyle.Format = "N0"

            Dim colCostIn As New DataGridViewTextBoxColumn()
            colCostIn.Name = "colCostIn"
            colCostIn.DataPropertyName = "CostIn"
            colCostIn.HeaderText = "بهای تمام شده" & vbCrLf & "وارده"
            colCostIn.Width = 120
            colCostIn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colCostIn.DefaultCellStyle.Format = "N0"

            Dim colCostOut As New DataGridViewTextBoxColumn()
            colCostOut.Name = "colCostOut"
            colCostOut.DataPropertyName = "CostOut"
            colCostOut.HeaderText = "بهای تمام شده" & vbCrLf & "صادره"
            colCostOut.Width = 120
            colCostOut.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colCostOut.DefaultCellStyle.Format = "N0"

            Dim colBalanceCost As New DataGridViewTextBoxColumn()
            colBalanceCost.Name = "colBalanceCost"
            colBalanceCost.DataPropertyName = "BalanceCost"
            colBalanceCost.HeaderText = "بهای تمام شده" & vbCrLf & "موجودی"
            colBalanceCost.Width = 130
            colBalanceCost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colBalanceCost.DefaultCellStyle.Format = "N0"

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            dgvKardex.Columns.AddRange(New DataGridViewColumn() {
                rowNum, colDate, colWarehouse, colType, colIn, colOut, colBalance,
                colCostIn, colCostOut, colBalanceCost, colDesc
            })

            dgvKardex.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub LoadKardexProducts()
            Try
                Dim dt = catalogService.GetProducts()
                cmbKardexProduct.DataSource = dt
                cmbKardexProduct.DisplayMember = "ProductName"
                cmbKardexProduct.ValueMember = "ProductID"
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    cmbKardexProduct.SelectedIndex = 0
                Else
                    cmbKardexProduct.SelectedIndex = -1
                End If
            Catch
            End Try
        End Sub

        Private Sub LoadInventory()
            Try
                Dim warehouseId As Integer? = Nothing
                If cmbWarehouse.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbWarehouse.SelectedValue) Then
                    Dim wId = Convert.ToInt32(cmbWarehouse.SelectedValue)
                    If wId > 0 Then warehouseId = wId
                Else
                    Dim drv = TryCast(cmbWarehouse.SelectedItem, DataRowView)
                    If drv IsNot Nothing AndAlso Not drv.Row.IsNull("WarehouseID") Then
                        Dim wId = Convert.ToInt32(drv("WarehouseID"))
                        If wId > 0 Then warehouseId = wId
                    End If
                End If

                _inventoryTable = inventoryService.GetInventory(warehouseId)
                If _inventoryTable IsNot Nothing Then
                    If Not _inventoryTable.Columns.Contains("TotalValue") Then
                        _inventoryTable.Columns.Add("TotalValue", GetType(Decimal), "Quantity * AverageCost")
                    End If
                    If Not _inventoryTable.Columns.Contains("PersianLastUpdate") Then
                        _inventoryTable.Columns.Add("PersianLastUpdate", GetType(String))
                    End If

                    For Each row As DataRow In _inventoryTable.Rows
                        If Not row.IsNull("LastUpdate") Then
                            Try
                                Dim rawVal = Convert.ToString(row("LastUpdate"))
                                Dim dtVal As DateTime
                                If DateTime.TryParse(rawVal, dtVal) Then
                                    row("PersianLastUpdate") = PersianDateHelper.ToPersian(dtVal) & "  " & dtVal.ToString("HH:mm")
                                Else
                                    row("PersianLastUpdate") = rawVal
                                End If
                            Catch
                                row("PersianLastUpdate") = Convert.ToString(row("LastUpdate"))
                            End Try
                        Else
                            row("PersianLastUpdate") = ""
                        End If
                    Next
                End If

                dgvInventory.DataSource = _inventoryTable
                ApplyInventoryFilter()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری موجودی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub txtSearchInventory_TextChanged(sender As Object, e As EventArgs) Handles txtSearchInventory.TextChanged
            ApplyInventoryFilter()
        End Sub

        Private Sub ApplyInventoryFilter()
            If _inventoryTable Is Nothing Then Return
            Dim f = txtSearchInventory.Text.Trim().Replace("'", "''")
            If String.IsNullOrEmpty(f) Then
                _inventoryTable.DefaultView.RowFilter = ""
            Else
                _inventoryTable.DefaultView.RowFilter = $"ProductCode LIKE '%{f}%' OR ProductName LIKE '%{f}%' OR WarehouseName LIKE '%{f}%'"
            End If

            RecalculateInventoryTotals()
        End Sub

        Private Sub RecalculateInventoryTotals()
            If _inventoryTable Is Nothing Then
                lblInventoryCount.Text = "تعداد اقلام: 0"
                lblGrandTotalValue.Text = "۰ ریال"
                Return
            End If

            Dim dv = _inventoryTable.DefaultView
            lblInventoryCount.Text = String.Format("تعداد اقلام: {0}", dv.Count)

            Dim grandTotal As Decimal = 0D
            For Each drv As DataRowView In dv
                If Not drv.Row.IsNull("TotalValue") Then
                    Dim val As Decimal = 0D
                    Decimal.TryParse(Convert.ToString(drv("TotalValue")), val)
                    grandTotal += val
                End If
            Next

            lblGrandTotalValue.Text = grandTotal.ToString("N0") & " ریال"
        End Sub

        Private isFormattingKardexDate As Boolean = False
        Private Sub FormatKardexDateTextBox(txt As TextBox)
            If isFormattingKardexDate Then Return
            Dim digitsOnly = System.Text.RegularExpressions.Regex.Replace(txt.Text, "[^\d]", "")
            If digitsOnly.Length = 8 Then
                isFormattingKardexDate = True
                txt.Text = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4, 2) & "/" & digitsOnly.Substring(6, 2)
                txt.SelectionStart = txt.Text.Length
                isFormattingKardexDate = False
            End If
        End Sub

        Private Sub txtKardexFrom_TextChanged(sender As Object, e As EventArgs) Handles txtKardexFrom.TextChanged
            FormatKardexDateTextBox(txtKardexFrom)
        End Sub

        Private Sub txtKardexTo_TextChanged(sender As Object, e As EventArgs) Handles txtKardexTo.TextChanged
            FormatKardexDateTextBox(txtKardexTo)
        End Sub

        Private Sub btnPickKardexFrom_Click(sender As Object, e As EventArgs) Handles btnPickKardexFrom.Click
            Using calForm As New PersianCalendarForm()
                If calForm.ShowDialog(Me) = DialogResult.OK Then
                    txtKardexFrom.Text = calForm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnPickKardexTo_Click(sender As Object, e As EventArgs) Handles btnPickKardexTo.Click
            Using calForm As New PersianCalendarForm()
                If calForm.ShowDialog(Me) = DialogResult.OK Then
                    txtKardexTo.Text = calForm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub LoadKardex()
            Try
                Dim drv = TryCast(cmbKardexProduct.SelectedItem, DataRowView)
                If drv Is Nothing Then
                    MessageBox.Show("لطفا کالا را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                Dim productId = Convert.ToInt32(drv("ProductID"))

                Dim warehouseId As Integer? = Nothing
                Dim wDrv = TryCast(cmbKardexWarehouse.SelectedItem, DataRowView)
                If wDrv IsNot Nothing Then warehouseId = Convert.ToInt32(wDrv("WarehouseID"))

                _kardexTable = inventoryService.GetKardex(productId, warehouseId, txtKardexFrom.Text, txtKardexTo.Text)

                ' اضافه کردن ستون تاریخ شمسی
                If Not _kardexTable.Columns.Contains("PersianDate") Then
                    _kardexTable.Columns.Add("PersianDate", GetType(String))
                End If
                For Each row As DataRow In _kardexTable.Rows
                    If Not row.IsNull("TransactionDate") Then
                        Try
                            Dim d = Convert.ToDateTime(row("TransactionDate"))
                            row("PersianDate") = ToPersian(d)
                        Catch
                            row("PersianDate") = Convert.ToString(row("TransactionDate"))
                        End Try
                    End If
                Next

                dgvKardex.DataSource = _kardexTable

                ' شماره ردیف خودکار
                For i = 0 To dgvKardex.Rows.Count - 1
                    If dgvKardex.Rows(i).Cells("colRowNum") IsNot Nothing Then
                        dgvKardex.Rows(i).Cells("colRowNum").Value = i + 1
                    End If
                Next

                lblKardexCount.Text = String.Format("تعداد تراکنشها: {0}", If(_kardexTable IsNot Nothing, _kardexTable.Rows.Count, 0))

                ' عنوان کاردکس
                Dim productName = Convert.ToString(drv("ProductName"))
                lblKardexTitle.Text = String.Format("کاردکس کالا:  {0}", productName)
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری کاردکس: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' ===== رویدادها =====

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadInventory()
        End Sub

        Private Sub CmbWarehouse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbWarehouse.SelectedIndexChanged
            LoadInventory()
        End Sub

        Private Sub BtnPrintInventory_Click(sender As Object, e As EventArgs) Handles btnPrintInventory.Click
            PrintInventory()
        End Sub

        Private Sub BtnKardexLoad_Click(sender As Object, e As EventArgs) Handles btnKardexLoad.Click
            LoadKardex()
        End Sub

        Private Sub BtnPrintKardex_Click(sender As Object, e As EventArgs) Handles btnPrintKardex.Click
            PrintKardex()
        End Sub

        Private Sub BtnGenerateInvCount_Click(sender As Object, e As EventArgs) Handles btnGenerateInvCount.Click
            GenerateInventoryCount()
        End Sub

        Private Sub BtnPrintInvCount_Click(sender As Object, e As EventArgs) Handles btnPrintInvCount.Click
            PrintInvCount()
        End Sub

        ' ===== چاپ موجودی انبار =====

        Private Sub PrintInventory()
            If _inventoryTable Is Nothing OrElse _inventoryTable.Rows.Count = 0 Then
                MessageBox.Show("دادهای برای چاپ وجود ندارد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            _printLines.Clear()
            _printLines.Add("گزارش موجودی انبار")
            _printLines.Add(String.Format("تاریخ چاپ: {0}", ToPersian(DateTime.Now)))
            _printLines.Add(String.Concat(New String("-"c, 90)))
            _printLines.Add(String.Format("{0,-20}{1,-35}{2,-25}{3,-12}", "کد کالا", "نام کالا", "انبار", "موجودی"))
            _printLines.Add(String.Concat(New String("-"c, 90)))

            For Each row As DataRow In _inventoryTable.Rows
                _printLines.Add(String.Format("{0,-20}{1,-35}{2,-25}{3,-12}",
                    Convert.ToString(row("ProductCode")),
                    TruncateStr(Convert.ToString(row("ProductName")), 33),
                    TruncateStr(Convert.ToString(row("WarehouseName")), 23),
                    Convert.ToString(row("Quantity"))))
            Next
            _printLines.Add(String.Concat(New String("-"c, 90)))

            _printDoc.DocumentName = "موجودی انبار"
            _printLineIndex = 0
            Using dlg As New PrintPreviewDialog()
                dlg.Document = _printDoc
                dlg.WindowState = FormWindowState.Maximized
                dlg.ShowDialog(Me)
            End Using
        End Sub

        ' ===== چاپ کاردکس =====

        Private Sub PrintKardex()
            If _kardexTable Is Nothing OrElse _kardexTable.Rows.Count = 0 Then
                MessageBox.Show("دادهای برای چاپ وجود ندارد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            _printLines.Clear()
            _printLines.Add(lblKardexTitle.Text)
            _printLines.Add(String.Format("تاریخ چاپ: {0}", ToPersian(DateTime.Now)))
            _printLines.Add(String.Concat(New String("-"c, 100)))
            _printLines.Add(String.Format("{0,-5}{1,-14}{2,-22}{3,-22}{4,-10}{5,-10}{6,-10}", "ردیف", "تاریخ", "انبار", "نوع عملیات", "ورود", "خروج", "موجودی"))
            _printLines.Add(String.Concat(New String("-"c, 100)))

            Dim i As Integer = 1
            For Each row As DataRow In _kardexTable.Rows
                _printLines.Add(String.Format("{0,-5}{1,-14}{2,-22}{3,-22}{4,-10}{5,-10}{6,-10}",
                    i,
                    Convert.ToString(row("PersianDate")),
                    TruncateStr(Convert.ToString(row("WarehouseName")), 20),
                    TruncateStr(Convert.ToString(row("TransactionType")), 20),
                    If(Convert.ToDecimal(row("QuantityIn")) > 0, Convert.ToString(row("QuantityIn")), "-"),
                    If(Convert.ToDecimal(row("QuantityOut")) > 0, Convert.ToString(row("QuantityOut")), "-"),
                    Convert.ToString(row("Balance"))))
                i += 1
            Next
            _printLines.Add(String.Concat(New String("-"c, 100)))

            _printDoc.DocumentName = "کاردکس کالا"
            _printLineIndex = 0
            Using dlg As New PrintPreviewDialog()
                dlg.Document = _printDoc
                dlg.WindowState = FormWindowState.Maximized
                dlg.ShowDialog(Me)
            End Using
        End Sub

        ' ===== انبارگردانی =====

        Private Sub ConfigureInvCountGrid(showQty As Boolean, showLocation As Boolean)
            dgvInvCount.Columns.Clear()
            dgvInvCount.AutoGenerateColumns = False

            Dim colNo As New DataGridViewTextBoxColumn()
            colNo.Name = "colInvNo"
            colNo.HeaderText = "ردیف"
            colNo.Width = 55
            colNo.ReadOnly = True
            colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colInvCode"
            colCode.DataPropertyName = "ProductCode"
            colCode.HeaderText = "کد کالا"
            colCode.Width = 100
            colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colInvName"
            colName.DataPropertyName = "ProductName"
            colName.HeaderText = "نام کالا"
            colName.Width = 250
            colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colUnit As New DataGridViewTextBoxColumn()
            colUnit.Name = "colInvUnit"
            colUnit.DataPropertyName = "Unit"
            colUnit.HeaderText = "واحد"
            colUnit.Width = 70
            colUnit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "colInvWarehouse"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار"
            colWarehouse.Width = 160
            colWarehouse.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colLocation As New DataGridViewTextBoxColumn()
            colLocation.Name = "colInvLocation"
            colLocation.DataPropertyName = "LocationPath"
            colLocation.HeaderText = "محل کالا در انبار"
            colLocation.Width = 180
            colLocation.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            colLocation.Visible = showLocation

            Dim colSysQty As New DataGridViewTextBoxColumn()
            colSysQty.Name = "colInvSysQty"
            colSysQty.DataPropertyName = "SystemQty"
            colSysQty.HeaderText = "موجودی سیستم"
            colSysQty.Width = 110
            colSysQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colSysQty.Visible = showQty

            Dim colActualQty As New DataGridViewTextBoxColumn()
            colActualQty.Name = "colInvActualQty"
            colActualQty.HeaderText = "تعداد شمارش شده"
            colActualQty.Width = 130
            colActualQty.ReadOnly = False
            colActualQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colActualQty.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 220)

            Dim colNote As New DataGridViewTextBoxColumn()
            colNote.Name = "colInvNote"
            colNote.HeaderText = "توضیحات"
            colNote.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colNote.ReadOnly = False
            colNote.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dgvInvCount.Columns.AddRange(New DataGridViewColumn() {
                colNo, colCode, colName, colUnit, colWarehouse, colLocation, colSysQty, colActualQty, colNote
            })
            dgvInvCount.ReadOnly = False
        End Sub

        Private Sub GenerateInventoryCount()
            Try
                Dim warehouseId As Integer? = Nothing
                Dim drv = TryCast(cmbInvCountWarehouse.SelectedItem, DataRowView)
                If drv IsNot Nothing Then
                    Dim wId = Convert.ToInt32(drv("WarehouseID"))
                    If wId > 0 Then warehouseId = wId
                End If

                Dim showQty = chkShowQty.Checked
                Dim showLoc = chkShowLocation.Checked

                ConfigureInvCountGrid(showQty, showLoc)

                ' کوئری موجودی با join به Products و Warehouses
                Dim query As String
                Dim dt As DataTable

                If warehouseId.HasValue Then
                    query = "SELECT p.ProductCode, p.ProductName, " &
                            "COALESCE(p.Unit, 'عدد') AS Unit, " &
                            "w.WarehouseName, " &
                            "COALESCE(i.Quantity, 0) AS SystemQty, " &
                            "COALESCE(p.LocationID, 0) AS LocationID " &
                            "FROM Products p " &
                            "LEFT JOIN Inventory i ON i.ProductID = p.ProductID AND i.WarehouseID = ? " &
                            "LEFT JOIN Warehouses w ON w.WarehouseID = ? " &
                            "WHERE p.IsActive = 1 OR p.IsActive IS NULL " &
                            "ORDER BY p.ProductName"
                    dt = Sql.ExecuteTable(query, warehouseId.Value, warehouseId.Value)
                Else
                    query = "SELECT p.ProductCode, p.ProductName, " &
                            "COALESCE(p.Unit, 'عدد') AS Unit, " &
                            "COALESCE(w.WarehouseName, '---') AS WarehouseName, " &
                            "COALESCE(i.Quantity, 0) AS SystemQty, " &
                            "COALESCE(p.LocationID, 0) AS LocationID " &
                            "FROM Products p " &
                            "LEFT JOIN Inventory i ON i.ProductID = p.ProductID " &
                            "LEFT JOIN Warehouses w ON w.WarehouseID = i.WarehouseID " &
                            "WHERE p.IsActive = 1 OR p.IsActive IS NULL " &
                            "ORDER BY p.ProductName, w.WarehouseName"
                    dt = Sql.ExecuteTable(query)
                End If

                ' اضافه کردن ستون مسیر محل کالا
                If Not dt.Columns.Contains("LocationPath") Then
                    dt.Columns.Add("LocationPath", GetType(String))
                End If

                If showLoc Then
                    For Each row As DataRow In dt.Rows
                        Dim locId = Convert.ToInt32(row("LocationID"))
                        If locId > 0 Then
                            Try
                                Dim locPath = catalogService.GetLocationPath(locId)
                                row("LocationPath") = If(locPath IsNot Nothing, locPath.Item1, "")
                            Catch
                                row("LocationPath") = ""
                            End Try
                        Else
                            row("LocationPath") = ""
                        End If
                    Next
                End If

                _invCountTable = dt
                dgvInvCount.DataSource = dt

                ' شماره ردیف
                For i = 0 To dgvInvCount.Rows.Count - 1
                    If dgvInvCount.Columns.Contains("colInvNo") Then
                        dgvInvCount.Rows(i).Cells("colInvNo").Value = i + 1
                    End If
                Next

                lblInvCountStatus.Text = String.Format("تعداد اقلام: {0}", dt.Rows.Count)
            Catch ex As Exception
                MessageBox.Show("خطا در تهیه لیست انبارگردانی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub PrintInvCount()
            If _invCountTable Is Nothing OrElse _invCountTable.Rows.Count = 0 Then
                MessageBox.Show("ابتدا لیست انبارگردانی را تهیه کنید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim showQty = chkShowQty.Checked
            Dim showLoc = chkShowLocation.Checked

            _printLines.Clear()
            _printLines.Add("لیست انبارگردانی")
            _printLines.Add(String.Format("تاریخ چاپ: {0}", ToPersian(DateTime.Now)))

            Dim drv = TryCast(cmbInvCountWarehouse.SelectedItem, DataRowView)
            If drv IsNot Nothing Then
                _printLines.Add(String.Format("انبار: {0}", Convert.ToString(drv("WarehouseName"))))
            End If
            _printLines.Add(String.Concat(New String("-"c, 110)))

            ' سرستون
            Dim header As String
            If showLoc AndAlso showQty Then
                header = String.Format("{0,-5}{1,-14}{2,-32}{3,-10}{4,-22}{5,-18}{6,-10}", "ردیف", "کد", "نام کالا", "واحد", "محل کالا", "موجودی سیستم", "شمارش")
            ElseIf showLoc Then
                header = String.Format("{0,-5}{1,-14}{2,-32}{3,-10}{4,-22}{5,-14}", "ردیف", "کد", "نام کالا", "واحد", "محل کالا", "شمارش")
            ElseIf showQty Then
                header = String.Format("{0,-5}{1,-14}{2,-38}{3,-10}{4,-18}{5,-14}", "ردیف", "کد", "نام کالا", "واحد", "موجودی سیستم", "شمارش")
            Else
                header = String.Format("{0,-5}{1,-14}{2,-45}{3,-10}{4,-18}", "ردیف", "کد", "نام کالا", "واحد", "شمارش")
            End If
            _printLines.Add(header)
            _printLines.Add(String.Concat(New String("-"c, 110)))

            Dim i As Integer = 1
            For Each row As DataRow In _invCountTable.Rows
                Dim line As String
                If showLoc AndAlso showQty Then
                    line = String.Format("{0,-5}{1,-14}{2,-32}{3,-10}{4,-22}{5,-18}{6,-10}",
                        i, Convert.ToString(row("ProductCode")),
                        TruncateStr(Convert.ToString(row("ProductName")), 30),
                        Convert.ToString(row("Unit")),
                        TruncateStr(Convert.ToString(row("LocationPath")), 20),
                        Convert.ToString(row("SystemQty")), "")
                ElseIf showLoc Then
                    line = String.Format("{0,-5}{1,-14}{2,-32}{3,-10}{4,-22}{5,-14}",
                        i, Convert.ToString(row("ProductCode")),
                        TruncateStr(Convert.ToString(row("ProductName")), 30),
                        Convert.ToString(row("Unit")),
                        TruncateStr(Convert.ToString(row("LocationPath")), 20), "")
                ElseIf showQty Then
                    line = String.Format("{0,-5}{1,-14}{2,-38}{3,-10}{4,-18}{5,-14}",
                        i, Convert.ToString(row("ProductCode")),
                        TruncateStr(Convert.ToString(row("ProductName")), 36),
                        Convert.ToString(row("Unit")),
                        Convert.ToString(row("SystemQty")), "")
                Else
                    line = String.Format("{0,-5}{1,-14}{2,-45}{3,-10}{4,-18}",
                        i, Convert.ToString(row("ProductCode")),
                        TruncateStr(Convert.ToString(row("ProductName")), 43),
                        Convert.ToString(row("Unit")), "")
                End If
                _printLines.Add(line)
                i += 1
            Next
            _printLines.Add(String.Concat(New String("-"c, 110)))

            _printDoc.DocumentName = "لیست انبارگردانی"
            _printLineIndex = 0
            Using dlg As New PrintPreviewDialog()
                dlg.Document = _printDoc
                dlg.WindowState = FormWindowState.Maximized
                dlg.ShowDialog(Me)
            End Using
        End Sub

        Private Sub PrintDoc_PrintPage(sender As Object, e As PrintPageEventArgs)
            Dim font As New Font("Tahoma", 9.0!)
            Dim lineHeight As Single = font.GetHeight(e.Graphics) + 2
            Dim y As Single = e.MarginBounds.Top
            Dim linesOnPage As Integer = 0

            While _printLineIndex < _printLines.Count AndAlso linesOnPage < LinesPerPage
                Dim line = _printLines(_printLineIndex)
                e.Graphics.DrawString(line, font, Brushes.Black, e.MarginBounds.Left, y, StringFormat.GenericTypographic)
                y += lineHeight
                _printLineIndex += 1
                linesOnPage += 1
            End While

            e.HasMorePages = (_printLineIndex < _printLines.Count)
            font.Dispose()
        End Sub

        Private Function TruncateStr(s As String, maxLen As Integer) As String
            If String.IsNullOrEmpty(s) Then Return ""
            If s.Length <= maxLen Then Return s
            Return s.Substring(0, maxLen - 1) & "..."
        End Function

    End Class
End Namespace
