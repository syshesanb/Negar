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

            cmbKardexWarehouse.DataSource = warehouses.Copy()
            cmbKardexWarehouse.DisplayMember = "WarehouseName"
            cmbKardexWarehouse.ValueMember = "WarehouseID"
            cmbKardexWarehouse.SelectedIndex = -1

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
                New With {.Name = "colCode", .Prop = "ProductCode", .Header = "کد کالا", .Width = 100, .Align = DataGridViewContentAlignment.MiddleCenter},
                New With {.Name = "colName", .Prop = "ProductName", .Header = "نام کالا", .Width = 220, .Align = DataGridViewContentAlignment.MiddleRight},
                New With {.Name = "colWarehouse", .Prop = "WarehouseName", .Header = "انبار", .Width = 140, .Align = DataGridViewContentAlignment.MiddleRight},
                New With {.Name = "colTotalIn", .Prop = "TotalInput", .Header = "ورودی", .Width = 100, .Align = DataGridViewContentAlignment.MiddleCenter},
                New With {.Name = "colTotalOut", .Prop = "TotalOutput", .Header = "خروجی", .Width = 100, .Align = DataGridViewContentAlignment.MiddleCenter},
                New With {.Name = "colQty", .Prop = "Quantity", .Header = "موجودی", .Width = 100, .Align = DataGridViewContentAlignment.MiddleCenter},
                New With {.Name = "colAvgCost", .Prop = "AverageCost", .Header = "میانگین بهای تمام‌شده", .Width = 140, .Align = DataGridViewContentAlignment.MiddleCenter},
                New With {.Name = "colLastUpdate", .Prop = "LastUpdate", .Header = "آخرین به‌روزرسانی", .Width = 150, .Align = DataGridViewContentAlignment.MiddleCenter}
            }

            For Each c In cols
                Dim col As New DataGridViewTextBoxColumn()
                col.Name = c.Name
                col.DataPropertyName = c.Prop
                col.HeaderText = c.Header
                col.Width = c.Width
                col.DefaultCellStyle.Alignment = c.Align
                col.ReadOnly = True
                dgvInventory.Columns.Add(col)
            Next

            dgvInventory.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub ConfigureKardexGrid()
            dgvKardex.AutoGenerateColumns = False
            dgvKardex.Columns.Clear()

            Dim rowNum As New DataGridViewTextBoxColumn()
            rowNum.Name = "colRowNum"
            rowNum.HeaderText = "ردیف"
            rowNum.Width = 50
            rowNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            rowNum.ReadOnly = True

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110
            colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "colWarehouse"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار"
            colWarehouse.Width = 150
            colWarehouse.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "colType"
            colType.DataPropertyName = "TransactionType"
            colType.HeaderText = "نوع عملیات"
            colType.Width = 150
            colType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colIn As New DataGridViewTextBoxColumn()
            colIn.Name = "colIn"
            colIn.DataPropertyName = "QuantityIn"
            colIn.HeaderText = "ورود"
            colIn.Width = 90
            colIn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colOut As New DataGridViewTextBoxColumn()
            colOut.Name = "colOut"
            colOut.DataPropertyName = "QuantityOut"
            colOut.HeaderText = "خروج"
            colOut.Width = 90
            colOut.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colBalance As New DataGridViewTextBoxColumn()
            colBalance.Name = "colBalance"
            colBalance.DataPropertyName = "Balance"
            colBalance.HeaderText = "موجودی"
            colBalance.Width = 90
            colBalance.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dgvKardex.Columns.AddRange(New DataGridViewColumn() {
                rowNum, colDate, colWarehouse, colType, colIn, colOut, colBalance, colDesc
            })

            dgvKardex.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub LoadKardexProducts()
            Try
                Dim dt = catalogService.GetProducts()
                cmbKardexProduct.DataSource = dt
                cmbKardexProduct.DisplayMember = "ProductName"
                cmbKardexProduct.ValueMember = "ProductID"
                cmbKardexProduct.SelectedIndex = -1
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
                dgvInventory.DataSource = _inventoryTable
                lblInventoryCount.Text = String.Format("تعداد اقلام: {0}", If(_inventoryTable IsNot Nothing, _inventoryTable.Rows.Count, 0))
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری موجودی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
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
