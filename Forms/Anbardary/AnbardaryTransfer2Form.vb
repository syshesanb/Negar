Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Business.PersianDateHelper
Imports Negar.Data
Imports Negar.Forms.Moshtarak

Namespace Negar.Forms
    Public Class AnbardaryTransfer2Form
        Inherits Form

        Private _editTransferId As Integer? = Nothing
        Private _isLoading As Boolean = False
        Private _isDirty As Boolean = False
        Private _fromWarehouseId As Integer = 0
        Private _toWarehouseId As Integer = 0

        Private Const TotalPreloadedRows As Integer = 20

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(transferId As Integer)
            InitializeComponent()
            _editTransferId = transferId
        End Sub

        Private Sub AnbardaryTransfer2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            EnsureTablesExist()
            InitializeEntryGrid()
            LoadWarehouses()

            txtTransferDate.Text = ToPersian(DateTime.Today)
            GenerateTransferNumber()

            If _editTransferId.HasValue Then
                Me.Text = "ویرایش حواله بین انبارها"
                LoadTransferForEdit(_editTransferId.Value)
            Else
                Me.Text = "ثبت حواله بین انبارها (جدید)"
            End If

            _isDirty = False
        End Sub

        Private Sub EnsureTablesExist()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseTransfers (" &
                    "TransferID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "TransferNumber TEXT NOT NULL, " &
                    "TransferDate TEXT NOT NULL, " &
                    "FromWarehouseID INTEGER NOT NULL, " &
                    "ToWarehouseID INTEGER NOT NULL, " &
                    "Status TEXT DEFAULT 'ثبت شده', " &
                    "Description TEXT, " &
                    "CreatedAt TEXT DEFAULT (datetime('now')), " &
                    "FOREIGN KEY (FromWarehouseID) REFERENCES Warehouses(WarehouseID), " &
                    "FOREIGN KEY (ToWarehouseID) REFERENCES Warehouses(WarehouseID))")

                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseTransferDetails (" &
                    "DetailID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "TransferID INTEGER NOT NULL, " &
                    "ProductID INTEGER NOT NULL, " &
                    "Quantity REAL NOT NULL DEFAULT 0, " &
                    "Unit TEXT, " &
                    "Description TEXT, " &
                    "FOREIGN KEY (TransferID) REFERENCES WarehouseTransfers(TransferID) ON DELETE CASCADE, " &
                    "FOREIGN KEY (ProductID) REFERENCES Products(ProductID))")
            Catch ex As Exception
                ' جداول ممکن است قبلا ساخته شده باشند
            End Try
        End Sub

        Private Sub InitializeEntryGrid()
            If dgvEntryLines.Columns.Count > 0 Then Return

            Dim table As New DataTable()
            table.Columns.Add("LineNumber", GetType(Integer))
            table.Columns.Add("ProductID", GetType(Integer))
            table.Columns.Add("ProductCode", GetType(String))
            table.Columns.Add("ProductName", GetType(String))
            table.Columns.Add("Unit", GetType(String))
            table.Columns.Add("StockFrom", GetType(Decimal))
            table.Columns.Add("Quantity", GetType(Decimal))
            table.Columns.Add("LineDescription", GetType(String))
            table.Columns.Add("DetailID", GetType(Integer))

            For i = 1 To TotalPreloadedRows
                table.Rows.Add(i, 0, "", "", "", 0D, 0D, "", 0)
            Next

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
            colBtnKala.HeaderText = "..."
            colBtnKala.Text = "..."
            colBtnKala.UseColumnTextForButtonValue = True
            colBtnKala.Width = 35
            colBtnKala.FlatStyle = FlatStyle.Standard

            Dim colKalaCode As New DataGridViewTextBoxColumn()
            colKalaCode.Name = "colKalaCode"
            colKalaCode.DataPropertyName = "ProductCode"
            colKalaCode.HeaderText = "کد / بارکد"
            colKalaCode.Width = 100

            Dim colKalaName As New DataGridViewTextBoxColumn()
            colKalaName.Name = "colKalaName"
            colKalaName.DataPropertyName = "ProductName"
            colKalaName.HeaderText = "نام کالا"
            colKalaName.Width = 250

            Dim colUnit As New DataGridViewTextBoxColumn()
            colUnit.Name = "colUnit"
            colUnit.DataPropertyName = "Unit"
            colUnit.HeaderText = "واحد"
            colUnit.Width = 70
            colUnit.ReadOnly = True

            Dim colStockFrom As New DataGridViewTextBoxColumn()
            colStockFrom.Name = "colStockFrom"
            colStockFrom.DataPropertyName = "StockFrom"
            colStockFrom.HeaderText = "موجودی مبدا"
            colStockFrom.Width = 100
            colStockFrom.ReadOnly = True
            colStockFrom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.Name = "colQty"
            colQty.DataPropertyName = "Quantity"
            colQty.HeaderText = "مقدار انتقال"
            colQty.Width = 100
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colLineDesc As New DataGridViewTextBoxColumn()
            colLineDesc.Name = "colLineDesc"
            colLineDesc.DataPropertyName = "LineDescription"
            colLineDesc.HeaderText = "توضیحات ردیف"
            colLineDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvEntryLines.Columns.AddRange(New DataGridViewColumn() {
                colLineNo, colBtnKala, colKalaCode, colKalaName,
                colUnit, colStockFrom, colQty, colLineDesc
            })

            dgvEntryLines.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub LoadWarehouses()
            Try
                Dim dt = Sql.ExecuteTable("SELECT WarehouseID, WarehouseName FROM Warehouses ORDER BY WarehouseName")
                cmbFromWarehouse.DataSource = dt.Copy()
                cmbFromWarehouse.DisplayMember = "WarehouseName"
                cmbFromWarehouse.ValueMember = "WarehouseID"
                cmbFromWarehouse.SelectedIndex = -1

                cmbToWarehouse.DataSource = dt.Copy()
                cmbToWarehouse.DisplayMember = "WarehouseName"
                cmbToWarehouse.ValueMember = "WarehouseID"
                cmbToWarehouse.SelectedIndex = -1
            Catch ex As Exception
                ' انبارها هنوز تعریف نشدهاند
            End Try
        End Sub

        Private Sub GenerateTransferNumber()
            Try
                Dim maxNum = Sql.ExecuteScalar("SELECT MAX(CAST(SUBSTR(TransferNumber, 4) AS INTEGER)) FROM WarehouseTransfers WHERE TransferNumber LIKE 'HAN%'")
                Dim nextNum = 1
                If maxNum IsNot Nothing AndAlso Not Convert.IsDBNull(maxNum) Then
                    nextNum = Convert.ToInt32(maxNum) + 1
                End If
                txtTransferNumber.Text = "HAN" & nextNum.ToString("D5")
            Catch
                txtTransferNumber.Text = "HAN00001"
            End Try
        End Sub

        Private Sub LoadTransferForEdit(transferId As Integer)
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM WarehouseTransfers WHERE TransferID = ?", transferId)
                If dt.Rows.Count = 0 Then
                    MessageBox.Show("حواله مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Return
                End If

                Dim hdr = dt.Rows(0)
                txtTransferNumber.Text = Convert.ToString(hdr("TransferNumber"))
                If Not Convert.IsDBNull(hdr("TransferDate")) Then
                    Try
                        txtTransferDate.Text = ToPersian(Convert.ToDateTime(hdr("TransferDate")))
                    Catch
                        txtTransferDate.Text = Convert.ToString(hdr("TransferDate"))
                    End Try
                End If
                txtDescription.Text = If(hdr.Table.Columns.Contains("Description"), Convert.ToString(hdr("Description")), "")

                Dim fromWid = Convert.ToInt32(hdr("FromWarehouseID"))
                Dim toWid = Convert.ToInt32(hdr("ToWarehouseID"))
                _fromWarehouseId = fromWid
                _toWarehouseId = toWid

                For Each item As DataRowView In cmbFromWarehouse.Items
                    If Convert.ToInt32(item("WarehouseID")) = fromWid Then
                        cmbFromWarehouse.SelectedItem = item
                        Exit For
                    End If
                Next
                For Each item As DataRowView In cmbToWarehouse.Items
                    If Convert.ToInt32(item("WarehouseID")) = toWid Then
                        cmbToWarehouse.SelectedItem = item
                        Exit For
                    End If
                Next

                ' بارگذاری ردیفها
                Dim dtDetails = Sql.ExecuteTable(
                    "SELECT d.DetailID, d.ProductID, " &
                    "COALESCE(p.ProductCode, '') AS ProductCode, " &
                    "COALESCE(p.ProductName, '(کالای حذف شده)') AS ProductName, " &
                    "COALESCE(p.Unit, 'عدد') AS Unit, " &
                    "d.Quantity, COALESCE(d.Description, '') AS LineDescription " &
                    "FROM WarehouseTransferDetails d " &
                    "LEFT JOIN Products p ON p.ProductID = d.ProductID " &
                    "WHERE d.TransferID = ?", transferId)

                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.Clear()
                Dim lineNo As Integer = 1
                For Each dRow As DataRow In dtDetails.Rows
                    Dim row = table.NewRow()
                    row("LineNumber") = lineNo
                    row("ProductID") = dRow("ProductID")
                    row("ProductCode") = dRow("ProductCode")
                    row("ProductName") = dRow("ProductName")
                    row("Unit") = dRow("Unit")
                    row("Quantity") = dRow("Quantity")
                    row("LineDescription") = dRow("LineDescription")
                    row("StockFrom") = 0D
                    row("DetailID") = dRow("DetailID")
                    table.Rows.Add(row)
                    lineNo += 1
                Next

                ' پر کردن ردیفهای خالی باقیمانده
                While lineNo <= TotalPreloadedRows
                    Dim row = table.NewRow()
                    row("LineNumber") = lineNo
                    row("ProductID") = 0
                    row("ProductCode") = ""
                    row("ProductName") = ""
                    row("Unit") = ""
                    row("Quantity") = 0D
                    row("LineDescription") = ""
                    row("StockFrom") = 0D
                    row("DetailID") = 0
                    table.Rows.Add(row)
                    lineNo += 1
                End While
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری حواله: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DgvEntryLines_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellContentClick
            If e.RowIndex < 0 Then Return
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
            If colName = "colBtnKala" Then
                SelectProductForLine(e.RowIndex)
            End If
        End Sub

        Private Sub SelectProductForLine(rowIndex As Integer)
            Dim catalogSvc As New CatalogService()
            Dim products = catalogSvc.GetProducts()
            If products Is Nothing OrElse products.Rows.Count = 0 Then
                MessageBox.Show("هیچ کالایی در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب کالا"
                dlg.Size = New Size(600, 420)
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

                If dlg.ShowDialog(Me) = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    Dim drv = DirectCast(dlg.Tag, DataRowView)
                    Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                    Dim row = table.Rows(rowIndex)

                    row("ProductID") = drv("ProductID")
                    row("ProductCode") = If(drv.Row.Table.Columns.Contains("ProductCode"), Convert.ToString(drv("ProductCode")), "")
                    row("ProductName") = Convert.ToString(drv("ProductName"))
                    row("Unit") = If(drv.Row.Table.Columns.Contains("Unit"), Convert.ToString(drv("Unit")), "عدد")

                    ' نمایش موجودی از انبار مبدا
                    If _fromWarehouseId > 0 Then
                        Try
                            Dim pid = Convert.ToInt32(drv("ProductID"))
                            Dim stock = Sql.ExecuteScalar(
                                "SELECT COALESCE(SUM(Quantity), 0) FROM InventoryLedger WHERE ProductID = ? AND WarehouseID = ?",
                                pid, _fromWarehouseId)
                            row("StockFrom") = If(stock Is Nothing OrElse Convert.IsDBNull(stock), 0D, Convert.ToDecimal(stock))
                        Catch
                            row("StockFrom") = 0D
                        End Try
                    End If
                End If
            End Using
        End Sub

        Private Sub DgvEntryLines_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvEntryLines.CellValidating
            If dgvEntryLines.Columns(e.ColumnIndex).Name = "colQty" Then
                Dim val As Decimal
                If Not String.IsNullOrWhiteSpace(Convert.ToString(e.FormattedValue)) Then
                    If Not Decimal.TryParse(Convert.ToString(e.FormattedValue), val) Then
                        e.Cancel = True
                        MessageBox.Show("لطفا یک عدد وارد نمایید.", "خطا در ورودی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            End If
        End Sub

        Private Sub CmbFromWarehouse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFromWarehouse.SelectedIndexChanged
            Try
                Dim drv = TryCast(cmbFromWarehouse.SelectedItem, DataRowView)
                If drv IsNot Nothing Then
                    _fromWarehouseId = Convert.ToInt32(drv("WarehouseID"))
                ElseIf cmbFromWarehouse.SelectedValue IsNot Nothing AndAlso Not Convert.IsDBNull(cmbFromWarehouse.SelectedValue) Then
                    _fromWarehouseId = Convert.ToInt32(cmbFromWarehouse.SelectedValue)
                End If
            Catch
                _fromWarehouseId = 0
            End Try
        End Sub

        Private Sub CmbToWarehouse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbToWarehouse.SelectedIndexChanged
            Try
                Dim drv = TryCast(cmbToWarehouse.SelectedItem, DataRowView)
                If drv IsNot Nothing Then
                    _toWarehouseId = Convert.ToInt32(drv("WarehouseID"))
                ElseIf cmbToWarehouse.SelectedValue IsNot Nothing AndAlso Not Convert.IsDBNull(cmbToWarehouse.SelectedValue) Then
                    _toWarehouseId = Convert.ToInt32(cmbToWarehouse.SelectedValue)
                End If
            Catch
                _toWarehouseId = 0
            End Try
        End Sub

        Private Sub BtnAddRow_Click(sender As Object, e As EventArgs) Handles btnAddRow.Click
            Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
            Dim nextLine = table.Rows.Count + 1
            Dim newRow = table.NewRow()
            newRow("LineNumber") = nextLine
            newRow("ProductID") = 0
            newRow("ProductCode") = ""
            newRow("ProductName") = ""
            newRow("Unit") = ""
            newRow("StockFrom") = 0D
            newRow("Quantity") = 0D
            newRow("LineDescription") = ""
            newRow("DetailID") = 0
            table.Rows.Add(newRow)
            dgvEntryLines.FirstDisplayedScrollingRowIndex = table.Rows.Count - 1
        End Sub

        Private Sub BtnDeleteRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
            If dgvEntryLines.CurrentRow IsNot Nothing AndAlso Not dgvEntryLines.CurrentRow.IsNewRow Then
                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                table.Rows.RemoveAt(dgvEntryLines.CurrentRow.Index)
                ' شمارهگذاری مجدد
                Dim i = 1
                For Each row As DataRow In table.Rows
                    row("LineNumber") = i
                    i += 1
                Next
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            SaveTransfer()
        End Sub

        Private Sub BtnSaveExit_Click(sender As Object, e As EventArgs) Handles btnSaveExit.Click
            If SaveTransfer() Then Me.DialogResult = DialogResult.OK : Me.Close()
        End Sub

        Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Function SaveTransfer() As Boolean
            Try
                ' اعتبارسنجی
                If String.IsNullOrWhiteSpace(txtTransferNumber.Text) Then
                    MessageBox.Show("شماره حواله را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                If String.IsNullOrWhiteSpace(txtTransferDate.Text) Then
                    MessageBox.Show("تاریخ حواله را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                If _fromWarehouseId <= 0 Then
                    MessageBox.Show("انبار مبدا را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                If _toWarehouseId <= 0 Then
                    MessageBox.Show("انبار مقصد را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                If _fromWarehouseId = _toWarehouseId Then
                    MessageBox.Show("انبار مبدا و مقصد نباید یکسان باشند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If

                ' بررسی ردیفهای معتبر
                Dim table = DirectCast(dgvEntryLines.DataSource, DataTable)
                Dim validRows As New List(Of DataRow)()
                For Each row As DataRow In table.Rows
                    Dim pid = Convert.ToInt32(row("ProductID"))
                    Dim qty = Convert.ToDecimal(row("Quantity"))
                    If pid > 0 AndAlso qty > 0 Then
                        validRows.Add(row)
                    End If
                Next

                If validRows.Count = 0 Then
                    MessageBox.Show("حداقل یک ردیف کالا با مقدار بزرگتر از صفر وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If

                Dim transferDate As String = txtTransferDate.Text

                If _editTransferId.HasValue Then
                    ' ویرایش
                    Sql.ExecuteNonQuery(
                        "UPDATE WarehouseTransfers SET TransferNumber=?, TransferDate=?, FromWarehouseID=?, ToWarehouseID=?, Description=? WHERE TransferID=?",
                        txtTransferNumber.Text, transferDate, _fromWarehouseId, _toWarehouseId,
                        txtDescription.Text, _editTransferId.Value)
                    Sql.ExecuteNonQuery("DELETE FROM WarehouseTransferDetails WHERE TransferID=?", _editTransferId.Value)

                    For Each row As DataRow In validRows
                        Sql.ExecuteNonQuery(
                            "INSERT INTO WarehouseTransferDetails (TransferID, ProductID, Quantity, Unit, Description) VALUES (?,?,?,?,?)",
                            _editTransferId.Value, Convert.ToInt32(row("ProductID")),
                            Convert.ToDecimal(row("Quantity")), Convert.ToString(row("Unit")),
                            Convert.ToString(row("LineDescription")))
                    Next

                    ' بروزرسانی موجودی انبار (ابتدا برگشت قبلی)
                    UpdateInventory(validRows)

                    MessageBox.Show("حواله با موفقیت ویرایش شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' ثبت جدید
                    Sql.ExecuteNonQuery(
                        "INSERT INTO WarehouseTransfers (TransferNumber, TransferDate, FromWarehouseID, ToWarehouseID, Status, Description) VALUES (?,?,?,?,?,?)",
                        txtTransferNumber.Text, transferDate, _fromWarehouseId, _toWarehouseId,
                        "ثبت شده", txtDescription.Text)

                    Dim newId = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                    For Each row As DataRow In validRows
                        Sql.ExecuteNonQuery(
                            "INSERT INTO WarehouseTransferDetails (TransferID, ProductID, Quantity, Unit, Description) VALUES (?,?,?,?,?)",
                            newId, Convert.ToInt32(row("ProductID")),
                            Convert.ToDecimal(row("Quantity")), Convert.ToString(row("Unit")),
                            Convert.ToString(row("LineDescription")))
                    Next

                    ' بروزرسانی موجودی انبار
                    UpdateInventory(validRows)

                    MessageBox.Show("حواله با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    _editTransferId = Nothing
                    GenerateTransferNumber()
                End If

                _isDirty = False
                Return True
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره حواله: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Private Sub UpdateInventory(validRows As List(Of DataRow))
            For Each row As DataRow In validRows
                Dim pid = Convert.ToInt32(row("ProductID"))
                Dim qty = Convert.ToDecimal(row("Quantity"))

                ' کاهش موجودی انبار مبدا
                Try
                    Sql.ExecuteNonQuery(
                        "INSERT INTO InventoryLedger (ProductID, WarehouseID, Quantity, TransactionDate, TransactionType, Description) " &
                        "VALUES (?, ?, ?, datetime('now'), 'حواله انتقال (خروج)', 'انتقال به انبار مقصد')",
                        pid, _fromWarehouseId, -qty)
                Catch
                    ' اگر جدول InventoryLedger وجود نداشت
                End Try

                ' افزایش موجودی انبار مقصد
                Try
                    Sql.ExecuteNonQuery(
                        "INSERT INTO InventoryLedger (ProductID, WarehouseID, Quantity, TransactionDate, TransactionType, Description) " &
                        "VALUES (?, ?, ?, datetime('now'), 'رسید انتقال (ورود)', 'انتقال از انبار مبدا')",
                        pid, _toWarehouseId, qty)
                Catch
                End Try
            Next
        End Sub

    End Class
End Namespace
