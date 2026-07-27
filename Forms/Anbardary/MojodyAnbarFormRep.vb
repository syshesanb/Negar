Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.Linq
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
            ApplyGridStyle(dgvProfitLoss)
            ApplyGridStyle(dgvInvCount)

            ConfigureInventoryGrid()
            ConfigureKardexGrid()
            ConfigureProfitLossGrid()

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

            dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvInventory.ColumnHeadersHeight = 44
            dgvInventory.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            Dim cols() As Object = {
                New With {.Name = "colCode", .Prop = "ProductCode", .Header = "کد" & vbCrLf & "کالا", .Width = 70, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "", .AutoFit = False},
                New With {.Name = "colName", .Prop = "ProductName", .Header = "نام کالا", .Width = 180, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "", .AutoFit = False},
                New With {.Name = "colWarehouse", .Prop = "WarehouseName", .Header = "انبار", .Width = 120, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "", .AutoFit = False},
                New With {.Name = "colTotalIn", .Prop = "TotalInput", .Header = "تعداد" & vbCrLf & "ورودی", .Width = 75, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colTotalOut", .Prop = "TotalOutput", .Header = "تعداد" & vbCrLf & "خروجی", .Width = 75, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colQty", .Prop = "Quantity", .Header = "تعداد" & vbCrLf & "موجودی", .Width = 80, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "N0", .AutoFit = False},
                New With {.Name = "colAvgCost", .Prop = "AverageCost", .Header = "میانگین قیمت واحد" & vbCrLf & "بهای تمام شده", .Width = 140, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "N0", .AutoFit = False},
                New With {.Name = "colTotalValue", .Prop = "TotalValue", .Header = "بهای تمام شده" & vbCrLf & "موجودی", .Width = 130, .Align = DataGridViewContentAlignment.MiddleLeft, .Format = "N0", .AutoFit = False},
                New With {.Name = "colLastUpdate", .Prop = "PersianLastUpdate", .Header = "آخرین" & vbCrLf & "به‌روزرسانی", .Width = 140, .Align = DataGridViewContentAlignment.MiddleCenter, .Format = "", .AutoFit = True}
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

            Dim colUnitPrice As New DataGridViewTextBoxColumn()
            colUnitPrice.Name = "colUnitPrice"
            colUnitPrice.DataPropertyName = "UnitPrice"
            colUnitPrice.HeaderText = "قیمت" & vbCrLf & "واحد"
            colUnitPrice.Width = 95
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colUnitPrice.DefaultCellStyle.Format = "N0"

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
                colUnitPrice, colCostIn, colCostOut, colBalanceCost, colDesc
            })

            dgvKardex.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub ConfigureProfitLossGrid()
            dgvProfitLoss.AutoGenerateColumns = False
            dgvProfitLoss.Columns.Clear()

            dgvProfitLoss.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvProfitLoss.ColumnHeadersHeight = 44
            dgvProfitLoss.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            Dim rowNum As New DataGridViewTextBoxColumn()
            rowNum.Name = "rowNum"
            rowNum.HeaderText = "ردیف"
            rowNum.Width = 45
            rowNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colDate"
            colDate.DataPropertyName = "TransactionDate"
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

            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.Name = "colQty"
            colQty.DataPropertyName = "Quantity"
            colQty.HeaderText = "تعداد" & vbCrLf & "فروخته شده"
            colQty.Width = 80
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colQty.DefaultCellStyle.Format = "N0"

            Dim colSalesAmt As New DataGridViewTextBoxColumn()
            colSalesAmt.Name = "colSalesAmt"
            colSalesAmt.DataPropertyName = "SalesAmount"
            colSalesAmt.HeaderText = "مبلغ" & vbCrLf & "فروش"
            colSalesAmt.Width = 120
            colSalesAmt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colSalesAmt.DefaultCellStyle.Format = "N0"

            Dim colCOGS As New DataGridViewTextBoxColumn()
            colCOGS.Name = "colCOGS"
            colCOGS.DataPropertyName = "CostOfGoodsSold"
            colCOGS.HeaderText = "بهای تمام شده" & vbCrLf & "کالای فروش رفته"
            colCOGS.Width = 135
            colCOGS.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colCOGS.DefaultCellStyle.Format = "N0"

            Dim colGrossProfit As New DataGridViewTextBoxColumn()
            colGrossProfit.Name = "colGrossProfit"
            colGrossProfit.DataPropertyName = "GrossProfit"
            colGrossProfit.HeaderText = "سود" & vbCrLf & "ناخالص"
            colGrossProfit.Width = 120
            colGrossProfit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colGrossProfit.DefaultCellStyle.Format = "N0"

            Dim colProfitMargin As New DataGridViewTextBoxColumn()
            colProfitMargin.Name = "colProfitMargin"
            colProfitMargin.DataPropertyName = "ProfitMargin"
            colProfitMargin.HeaderText = "درصد" & vbCrLf & "سود"
            colProfitMargin.Width = 75
            colProfitMargin.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colProfitMargin.DefaultCellStyle.Format = "0.0'%'"

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            dgvProfitLoss.Columns.AddRange(New DataGridViewColumn() {
                rowNum, colDate, colWarehouse, colType, colQty, colSalesAmt,
                colCOGS, colGrossProfit, colProfitMargin, colDesc
            })

            dgvProfitLoss.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub LoadProfitLossDropdowns()
            Try
                ' Load Products
                Dim dtProd = catalogService.GetProducts()
                Dim dtProdWithAll = dtProd.Clone()
                Dim allProdRow = dtProdWithAll.NewRow()
                allProdRow("ProductID") = 0
                allProdRow("ProductName") = "همه کالاها"
                dtProdWithAll.Rows.Add(allProdRow)
                For Each r As DataRow In dtProd.Rows
                    dtProdWithAll.ImportRow(r)
                Next
                cmbProfitLossProduct.DataSource = dtProdWithAll
                cmbProfitLossProduct.DisplayMember = "ProductName"
                cmbProfitLossProduct.ValueMember = "ProductID"
                cmbProfitLossProduct.SelectedIndex = 0

                ' Load Warehouses
                Dim dtWh = catalogService.GetWarehouses()
                Dim dtWhWithAll = dtWh.Clone()
                Dim allWhRow = dtWhWithAll.NewRow()
                allWhRow("WarehouseID") = 0
                allWhRow("WarehouseName") = "همه انبارها"
                dtWhWithAll.Rows.Add(allWhRow)
                For Each r As DataRow In dtWh.Rows
                    dtWhWithAll.ImportRow(r)
                Next
                cmbProfitLossWarehouse.DataSource = dtWhWithAll
                cmbProfitLossWarehouse.DisplayMember = "WarehouseName"
                cmbProfitLossWarehouse.ValueMember = "WarehouseID"
                cmbProfitLossWarehouse.SelectedIndex = 0
            Catch
            End Try
        End Sub

        Private Sub LoadProfitLossData()
            Try
                Dim productId As Integer? = Nothing
                If cmbProfitLossProduct.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbProfitLossProduct.SelectedValue) Then
                    Dim pId = Convert.ToInt32(cmbProfitLossProduct.SelectedValue)
                    If pId > 0 Then productId = pId
                End If

                Dim warehouseId As Integer? = Nothing
                If cmbProfitLossWarehouse.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbProfitLossWarehouse.SelectedValue) Then
                    Dim wId = Convert.ToInt32(cmbProfitLossWarehouse.SelectedValue)
                    If wId > 0 Then warehouseId = wId
                End If

                Dim fromDate = txtProfitLossFrom.Text.Trim()
                Dim toDate = txtProfitLossTo.Text.Trim()

                Dim prodName As String = "همه کالاها"
                If productId.HasValue AndAlso cmbProfitLossProduct.SelectedItem IsNot Nothing Then
                    prodName = cmbProfitLossProduct.Text
                End If
                lblProfitLossTitle.Text = "گزارش سود و زیان کالا: " & prodName

                Dim dt = inventoryService.GetProductProfitLoss(productId, warehouseId, fromDate, toDate)
                dgvProfitLoss.DataSource = dt

                Dim grandTotalProfit As Decimal = 0D
                For idx As Integer = 0 To dgvProfitLoss.Rows.Count - 1
                    Dim row = dgvProfitLoss.Rows(idx)
                    row.Cells("rowNum").Value = (idx + 1).ToString()
                    If Not row.IsNewRow Then
                        Dim profitVal As Decimal = 0D
                        If Not row.Cells("colGrossProfit").Value Is DBNull.Value AndAlso row.Cells("colGrossProfit").Value IsNot Nothing Then
                            Decimal.TryParse(Convert.ToString(row.Cells("colGrossProfit").Value), profitVal)
                        End If

                        grandTotalProfit += profitVal

                        Dim cellProfit = row.Cells("colGrossProfit")
                        Dim cellMargin = row.Cells("colProfitMargin")

                        If profitVal > 0 Then
                            Dim bgProfit = Color.FromArgb(220, 248, 225)
                            Dim fgProfit = Color.FromArgb(21, 87, 36)
                            Dim selBgProfit = Color.FromArgb(195, 235, 202)

                            cellProfit.Style.BackColor = bgProfit
                            cellProfit.Style.ForeColor = fgProfit
                            cellProfit.Style.SelectionBackColor = selBgProfit
                            cellProfit.Style.SelectionForeColor = fgProfit

                            cellMargin.Style.BackColor = bgProfit
                            cellMargin.Style.ForeColor = fgProfit
                            cellMargin.Style.SelectionBackColor = selBgProfit
                            cellMargin.Style.SelectionForeColor = fgProfit
                        ElseIf profitVal < 0 Then
                            Dim bgLoss = Color.FromArgb(253, 227, 227)
                            Dim fgLoss = Color.FromArgb(114, 28, 36)
                            Dim selBgLoss = Color.FromArgb(245, 200, 200)

                            cellProfit.Style.BackColor = bgLoss
                            cellProfit.Style.ForeColor = fgLoss
                            cellProfit.Style.SelectionBackColor = selBgLoss
                            cellProfit.Style.SelectionForeColor = fgLoss

                            cellMargin.Style.BackColor = bgLoss
                            cellMargin.Style.ForeColor = fgLoss
                            cellMargin.Style.SelectionBackColor = selBgLoss
                            cellMargin.Style.SelectionForeColor = fgLoss
                        Else
                            Dim bgZero = Color.FromArgb(245, 245, 245)
                            Dim fgZero = Color.FromArgb(80, 80, 80)

                            cellProfit.Style.BackColor = bgZero
                            cellProfit.Style.ForeColor = fgZero
                            cellMargin.Style.BackColor = bgZero
                            cellMargin.Style.ForeColor = fgZero
                        End If
                    End If
                Next

                lblProfitLossGrandTotalValue.Text = String.Format("{0:N0} ریال", grandTotalProfit)
                If grandTotalProfit >= 0 Then
                    lblProfitLossGrandTotalValue.ForeColor = Color.FromArgb(46, 204, 113)
                Else
                    lblProfitLossGrandTotalValue.ForeColor = Color.FromArgb(231, 76, 60)
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری گزارش سود و زیان: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
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
            PrintGridReport("گزارش موجودی انبار", dgvInventory, "جمع کل بهای تمام شده موجودی:", lblGrandTotalValue.Text)
        End Sub

        Private Sub BtnKardexLoad_Click(sender As Object, e As EventArgs) Handles btnKardexLoad.Click
            LoadKardex()
        End Sub

        Private Sub BtnPrintKardex_Click(sender As Object, e As EventArgs) Handles btnPrintKardex.Click
            Dim title = If(String.IsNullOrWhiteSpace(lblKardexTitle.Text), "گزارش کاردکس کالا", lblKardexTitle.Text)
            PrintGridReport(title, dgvKardex)
        End Sub

        Private Sub BtnPrintProfitLoss_Click(sender As Object, e As EventArgs) Handles btnPrintProfitLoss.Click
            Dim title = If(String.IsNullOrWhiteSpace(lblProfitLossTitle.Text), "گزارش سود و زیان کالا", lblProfitLossTitle.Text)
            PrintGridReport(title, dgvProfitLoss, "جمع کل سود ناخالص:", lblProfitLossGrandTotalValue.Text)
        End Sub

        Private Sub BtnGenerateInvCount_Click(sender As Object, e As EventArgs) Handles btnGenerateInvCount.Click
            GenerateInventoryCount()
        End Sub

        Private Sub BtnPrintInvCount_Click(sender As Object, e As EventArgs) Handles btnPrintInvCount.Click
            PrintGridReport("لیست انبار گردانی", dgvInvCount)
        End Sub

        ' ===== موتور چاپ گرافیکی پیشرفته (طرح و رنگ‌بندی استاندارد مطابق تصویر نمونه) =====

        Private Sub PrintGridReport(reportTitle As String, grid As DataGridView, Optional totalLabel As String = Nothing, Optional totalValue As String = Nothing)
            If grid Is Nothing OrElse grid.Rows.Count = 0 Then
                MessageBox.Show("هیچ داده‌ای برای چاپ وجود ندارد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim doc As New PrintDocument()
            doc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            doc.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            Dim rowIndex As Integer = 0

            AddHandler doc.PrintPage, Sub(sender As Object, e As PrintPageEventArgs)
                Dim g = e.Graphics
                g.SmoothingMode = SmoothingMode.HighQuality
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                Dim leftX = e.MarginBounds.Left
                Dim rightX = e.MarginBounds.Right
                Dim topY = e.MarginBounds.Top
                Dim bottomY = e.MarginBounds.Bottom
                Dim pageWidth = rightX - leftX
                Dim pageHeight = bottomY - topY

                ' ۱. کادر پررنگ دور صفحه
                Using pBorder As New Pen(Color.Black, 2.0!)
                    g.DrawRectangle(pBorder, leftX, topY, pageWidth, pageHeight)
                End Using

                ' ۲. سربرگ: نام شرکت و عنوان گزارش با رنگ قرمز عنابی (مطابق تصویر)
                Dim companyName = "شرکت " & SessionContext.CurrentCompanyName
                Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                Dim sfLeft As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}

                Using brMaroon As New SolidBrush(Color.FromArgb(160, 0, 0))
                    Dim compRect As New Rectangle(leftX, topY + 12, pageWidth, 25)
                    Using fComp As New Font("Tahoma", 13.0!, FontStyle.Bold)
                        g.DrawString(companyName, fComp, brMaroon, compRect, sfCenter)
                    End Using

                    Dim titleRect As New Rectangle(leftX, topY + 38, pageWidth, 26)
                    Using fTitle As New Font("Tahoma", 11.5!, FontStyle.Bold)
                        g.DrawString(reportTitle, fTitle, brMaroon, titleRect, sfCenter)
                    End Using
                End Using

                ' تاریخ و مشخصات در سمت راست
                Using fBold As New Font("Tahoma", 9.0!, FontStyle.Bold)
                    Dim printDateStr = "تاریخ: " & PersianDateHelper.ToPersian(DateTime.Now)
                    g.DrawString(printDateStr, fBold, Brushes.Black, rightX - 15, topY + 22, sfRight)
                End Using

                ' ۳. استخراج ستون‌های مرئی جدول
                Dim visibleCols As New List(Of DataGridViewColumn)()
                For Each col As DataGridViewColumn In grid.Columns
                    If col.Visible Then visibleCols.Add(col)
                Next

                If visibleCols.Count = 0 Then Return

                ' محاسبه عرض نسبی ستون‌ها برای برازش کامل در عرض صفحه
                Dim totalGridWidth As Integer = visibleCols.Sum(Function(c) Math.Max(c.Width, 40))
                Dim colWidths As New List(Of Integer)()
                For Each c In visibleCols
                    Dim w = CInt((Math.Max(c.Width, 40) / CSng(totalGridWidth)) * pageWidth)
                    colWidths.Add(w)
                Next
                Dim currentSum = colWidths.Sum()
                If currentSum < pageWidth Then
                    colWidths(colWidths.Count - 1) += (pageWidth - currentSum)
                End If

                ' مختصات افقی ستون‌ها از راست به چپ
                Dim colX = New Integer(visibleCols.Count) {}
                colX(0) = rightX
                For i As Integer = 0 To visibleCols.Count - 1
                    colX(i + 1) = colX(i) - colWidths(i)
                Next

                Dim tableStartY = topY + 80
                Dim headerHeight = 32
                Dim rowHeight = 24
                Dim footerHeight = 40 ' ارتفاع امضاها
                Dim totalsHeight = If(Not String.IsNullOrEmpty(totalValue), 28, 0)
                Dim maxY = bottomY - footerHeight - totalsHeight - 10

                ' رسم هدر جدول (آبی فیروزه‌ای ملایم مطابق تصویر نمونه)
                Dim rectHeaderFull = New Rectangle(leftX, tableStartY, pageWidth, headerHeight)
                Using brHeaderBg As New SolidBrush(Color.FromArgb(210, 236, 245))
                    g.FillRectangle(brHeaderBg, rectHeaderFull)
                End Using
                g.DrawRectangle(Pens.Black, rectHeaderFull)

                Using fTableHeader As New Font("Tahoma", 8.5!, FontStyle.Bold)
                    For i As Integer = 0 To visibleCols.Count - 1
                        Dim rectColHeader = New Rectangle(colX(i + 1), tableStartY, colWidths(i), headerHeight)
                        g.DrawRectangle(Pens.Black, rectColHeader)

                        Dim cleanHeader = visibleCols(i).HeaderText.Replace(vbCrLf, " ")
                        g.DrawString(cleanHeader, fTableHeader, Brushes.Black, rectColHeader, sfCenter)
                    Next
                End Using

                ' ۴. رسم ردیف‌های داده
                Dim currY = tableStartY + headerHeight
                Using fRow As New Font("Tahoma", 8.5!, FontStyle.Regular)
                    While rowIndex < grid.Rows.Count AndAlso currY + rowHeight <= maxY
                        Dim row = grid.Rows(rowIndex)
                        If Not row.IsNewRow Then
                            For i As Integer = 0 To visibleCols.Count - 1
                                Dim col = visibleCols(i)
                                Dim cellRect = New Rectangle(colX(i + 1), currY, colWidths(i), rowHeight)

                                ' استخراج و قالب‌بندی مقدار سلول
                                Dim cellText As String = ""
                                Dim cellVal = row.Cells(col.Index).Value
                                If cellVal IsNot Nothing AndAlso Not Convert.IsDBNull(cellVal) Then
                                    If Not String.IsNullOrEmpty(col.DefaultCellStyle.Format) AndAlso IsNumeric(cellVal) Then
                                        Dim dVal As Decimal = 0D
                                        Decimal.TryParse(Convert.ToString(cellVal), dVal)
                                        cellText = dVal.ToString(col.DefaultCellStyle.Format)
                                    Else
                                        cellText = Convert.ToString(cellVal)
                                    End If
                                End If

                                ' رنگ پس‌زمینه اختصاصی سلول (مانند سبز و قرمز ملایم در سود و زیان)
                                Dim cellBg = row.Cells(col.Index).Style.BackColor
                                If cellBg.IsEmpty OrElse cellBg = Color.Empty Then cellBg = Color.White
                                Using brCellBg As New SolidBrush(cellBg)
                                    g.FillRectangle(brCellBg, cellRect)
                                End Using

                                ' تراز متن سلول
                                Dim align = col.DefaultCellStyle.Alignment
                                Dim sfCell As StringFormat = sfRight
                                If align = DataGridViewContentAlignment.MiddleCenter Then
                                    sfCell = sfCenter
                                ElseIf align = DataGridViewContentAlignment.MiddleLeft OrElse align = DataGridViewContentAlignment.BottomLeft OrElse align = DataGridViewContentAlignment.TopLeft Then
                                    sfCell = sfRight
                                End If

                                Dim textPaddingRect = New Rectangle(colX(i + 1) + 4, currY, colWidths(i) - 8, rowHeight)
                                g.DrawString(cellText, fRow, Brushes.Black, textPaddingRect, sfCell)
                                g.DrawRectangle(Pens.Black, cellRect)
                            Next

                            ' خط نقطه‌چین افقی
                            Using pDot As New Pen(Color.LightGray) With {.DashStyle = DashStyle.Dot}
                                g.DrawLine(pDot, leftX, currY + rowHeight, rightX, currY + rowHeight)
                            End Using

                            currY += rowHeight
                        End If
                        rowIndex += 1
                    End While
                End Using

                ' کادر مشکی انتهای جدول
                g.DrawRectangle(Pens.Black, leftX, tableStartY, pageWidth, currY - tableStartY)

                ' ۵. سطر جمع کل (زرد لیمویی ملایم مطابق تصویر نمونه)
                Dim isLastPage = (rowIndex >= grid.Rows.Count)
                If isLastPage AndAlso Not String.IsNullOrEmpty(totalValue) Then
                    Dim rectTotals = New Rectangle(leftX, currY, pageWidth, 28)
                    Using brTotals As New SolidBrush(Color.FromArgb(254, 248, 165))
                        g.FillRectangle(brTotals, rectTotals)
                    End Using
                    g.DrawRectangle(Pens.Black, rectTotals)

                    Using fTotals As New Font("Tahoma", 9.0!, FontStyle.Bold)
                        Dim rLabel = New Rectangle(leftX + (pageWidth \ 2), currY, (pageWidth \ 2) - 10, 28)
                        Dim lblStr = If(Not String.IsNullOrEmpty(totalLabel), totalLabel, "جمع کل:")
                        g.DrawString(lblStr, fTotals, Brushes.Black, rLabel, sfRight)

                        Dim rValue = New Rectangle(leftX + 10, currY, (pageWidth \ 2) - 10, 28)
                        g.DrawString(totalValue, fTotals, Brushes.Black, rValue, sfLeft)
                    End Using

                    currY += 28
                End If

                ' ۶. امضاداران پایین صفحه (تهیه کننده / تأیید کننده / تصویب کننده)
                If isLastPage Then
                    Dim sigY = bottomY - 35
                    Dim sigColWidth = pageWidth \ 3
                    Using fSig As New Font("Tahoma", 9.0!, FontStyle.Bold)
                        Dim rectSig1 = New Rectangle(rightX - sigColWidth, sigY, sigColWidth, 30)
                        g.DrawString("تهیه کننده:", fSig, Brushes.Black, rectSig1, sfCenter)

                        Dim rectSig2 = New Rectangle(rightX - (sigColWidth * 2), sigY, sigColWidth, 30)
                        g.DrawString("تأیید کننده:", fSig, Brushes.Black, rectSig2, sfCenter)

                        Dim rectSig3 = New Rectangle(leftX, sigY, sigColWidth, 30)
                        g.DrawString("تصویب کننده:", fSig, Brushes.Black, rectSig3, sfCenter)
                    End Using
                End If

                e.HasMorePages = Not isLastPage
                If isLastPage Then rowIndex = 0
            End Sub

            Using dlg As New PrintPreviewDialog()
                dlg.Document = doc
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

        Private Function TruncateStr(s As String, maxLen As Integer) As String
            If String.IsNullOrEmpty(s) Then Return ""
            If s.Length <= maxLen Then Return s
            Return s.Substring(0, maxLen - 1) & "..."
        End Function

        Private Sub btnProfitLossLoad_Click(sender As Object, e As EventArgs) Handles btnProfitLossLoad.Click
            LoadProfitLossData()
        End Sub

        Private isFormattingProfitLossDate As Boolean = False
        Private Sub FormatProfitLossDateTextBox(txt As TextBox)
            If isFormattingProfitLossDate Then Return
            Dim digitsOnly = System.Text.RegularExpressions.Regex.Replace(txt.Text, "[^\d]", "")
            If digitsOnly.Length = 8 Then
                isFormattingProfitLossDate = True
                txt.Text = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4, 2) & "/" & digitsOnly.Substring(6, 2)
                txt.SelectionStart = txt.Text.Length
                isFormattingProfitLossDate = False
            End If
        End Sub

        Private Sub txtProfitLossFrom_TextChanged(sender As Object, e As EventArgs) Handles txtProfitLossFrom.TextChanged
            FormatProfitLossDateTextBox(txtProfitLossFrom)
        End Sub

        Private Sub txtProfitLossTo_TextChanged(sender As Object, e As EventArgs) Handles txtProfitLossTo.TextChanged
            FormatProfitLossDateTextBox(txtProfitLossTo)
        End Sub

        Private Sub btnPickProfitLossFrom_Click(sender As Object, e As EventArgs) Handles btnPickProfitLossFrom.Click
            Using calendar As New PersianCalendarForm()
                If calendar.ShowDialog(Me) = DialogResult.OK Then
                    txtProfitLossFrom.Text = calendar.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnPickProfitLossTo_Click(sender As Object, e As EventArgs) Handles btnPickProfitLossTo.Click
            Using calendar As New PersianCalendarForm()
                If calendar.ShowDialog(Me) = DialogResult.OK Then
                    txtProfitLossTo.Text = calendar.SelectedDate
                End If
            End Using
        End Sub

        Private Sub tabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
            If tabMain.SelectedTab Is tabProfitLoss Then
                If cmbProfitLossProduct.Items.Count = 0 Then
                    LoadProfitLossDropdowns()
                End If
                If dgvProfitLoss.Rows.Count = 0 Then
                    LoadProfitLossData()
                End If
            End If
        End Sub

    End Class
End Namespace
