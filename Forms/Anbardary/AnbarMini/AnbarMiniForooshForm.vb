Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniForooshForm
        Public Event InvoiceSaved()
        Public Event InvoiceCancelled()

        Private ReadOnly catalogService As New CatalogService()
        Private ReadOnly invoiceService As New InvoiceService()
        Private ReadOnly defaultWarehouseId As Integer = 1
        Private _editingInvoiceId As Integer? = Nothing
        Private isFormattingDate As Boolean = False

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniForooshForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ResetForm()
        End Sub

        Public Sub ResetForm()
            _editingInvoiceId = Nothing
            lblTitle.Text = "فروش سریع (POS)"
            btnSaveAndPrint.Text = "ثبت و چاپ (F2)"
            txtInvoiceDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            txtCustomerName.Text = "مشتری نقدی"
            txtBarcodeScan.Clear()
            txtDescription.Clear()
            dgvCart.Rows.Clear()
            GenerateNextInvoiceNumber()
            LoadWarehouses()
            If cmbPaymentType.Items.Count > 0 Then cmbPaymentType.SelectedIndex = 0 ' کارتخوان (POS)
            RecalculateTotal()
            txtBarcodeScan.Focus()
        End Sub

        Private Sub cmbWarehouse_DropDown(sender As Object, e As EventArgs) Handles cmbWarehouse.DropDown
            LoadWarehouses()
        End Sub

        Public Sub LoadWarehouses()
            Try
                Dim compId = SessionContext.CurrentCompanyID
                Dim dt As DataTable
                If compId.HasValue Then
                    dt = Sql.ExecuteTable("SELECT WarehouseID, WarehouseName FROM Warehouses WHERE CompanyID = ? AND IsActive = 1 ORDER BY WarehouseID", compId.Value)
                Else
                    dt = Sql.ExecuteTable("SELECT WarehouseID, WarehouseName FROM Warehouses WHERE IsActive = 1 ORDER BY WarehouseID")
                End If

                cmbWarehouse.DataSource = Nothing
                cmbWarehouse.Items.Clear()

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    cmbWarehouse.DataSource = dt
                    cmbWarehouse.DisplayMember = "WarehouseName"
                    cmbWarehouse.ValueMember = "WarehouseID"
                    cmbWarehouse.SelectedIndex = 0
                End If
            Catch ex As Exception
                Console.WriteLine("Error loading warehouses: " & ex.Message)
            End Try
        End Sub

        Public Sub LoadInvoiceForEdit(invoiceId As Integer)
            Try
                Dim hdr = invoiceService.GetSalesInvoiceById(invoiceId)
                If hdr Is Nothing Then
                    MessageBox.Show("فاکتور مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                _editingInvoiceId = invoiceId
                txtInvoiceNo.Text = Convert.ToString(hdr("InvoiceNumber"))

                If Not hdr.IsNull("InvoiceDate") Then
                    Try
                        Dim dtVal = Convert.ToDateTime(hdr("InvoiceDate"))
                        txtInvoiceDate.Text = PersianDateHelper.ToPersian(dtVal)
                    Catch
                        txtInvoiceDate.Text = Convert.ToString(hdr("InvoiceDate"))
                    End Try
                End If

                txtCustomerName.Text = If(hdr.IsNull("CustomerName"), "مشتری نقدی", Convert.ToString(hdr("CustomerName")))
                txtDescription.Text = If(hdr.IsNull("Description"), "", Convert.ToString(hdr("Description")))

                If Not hdr.IsNull("PaymentType") Then
                    Dim pType = Convert.ToString(hdr("PaymentType"))
                    Dim idx = cmbPaymentType.FindStringExact(pType)
                    If idx >= 0 Then cmbPaymentType.SelectedIndex = idx
                End If

                LoadWarehouses()
                If Not hdr.IsNull("WarehouseID") Then
                    Try
                        cmbWarehouse.SelectedValue = Convert.ToInt32(hdr("WarehouseID"))
                    Catch
                    End Try
                End If

                lblTitle.Text = "ویرایش فاکتور فروش (" & txtInvoiceNo.Text & ")"
                btnSaveAndPrint.Text = "ثبت تغییرات"

                ' Load Invoice Details Items into DataGridView
                dgvCart.Rows.Clear()
                Dim dtDetails = invoiceService.GetSalesInvoiceDetails(invoiceId)
                If dtDetails IsNot Nothing Then
                    For Each dRow As DataRow In dtDetails.Rows
                        Dim pid = Convert.ToInt32(dRow("ProductID"))
                        Dim pCode = If(dRow.Table.Columns.Contains("ProductCode") AndAlso Not dRow.IsNull("ProductCode"), Convert.ToString(dRow("ProductCode")), "")
                        Dim pName = Convert.ToString(dRow("ProductName"))
                        Dim pUnit = If(dRow.Table.Columns.Contains("Unit") AndAlso Not dRow.IsNull("Unit"), Convert.ToString(dRow("Unit")), "عدد")
                        Dim qty = Convert.ToDecimal(dRow("Quantity"))
                        Dim unitPrice = Convert.ToDecimal(dRow("UnitPrice"))
                        Dim totalPrice = Convert.ToDecimal(dRow("TotalPrice"))

                        dgvCart.Rows.Add(pid, pCode, pName, pUnit, qty, unitPrice.ToString("N0"), totalPrice.ToString("N0"))
                    Next
                End If

                RecalculateTotal()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری فاکتور فروش جهت ویرایش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub txtInvoiceDate_TextChanged(sender As Object, e As EventArgs) Handles txtInvoiceDate.TextChanged
            If isFormattingDate Then Return
            Dim digitsOnly As String = System.Text.RegularExpressions.Regex.Replace(txtInvoiceDate.Text, "[^\d]", "")

            If digitsOnly.Length = 8 Then
                isFormattingDate = True
                txtInvoiceDate.Text = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4, 2) & "/" & digitsOnly.Substring(6, 2)
                txtInvoiceDate.SelectionStart = txtInvoiceDate.Text.Length
                isFormattingDate = False
            End If
        End Sub

        Private Sub btnPickDate_Click(sender As Object, e As EventArgs) Handles btnPickDate.Click
            Using calForm As New PersianCalendarForm()
                If calForm.ShowDialog(Me) = DialogResult.OK Then
                    txtInvoiceDate.Text = calForm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnPickCustomer_Click(sender As Object, e As EventArgs) Handles btnPickCustomer.Click
            Using dialog As New Form()
                dialog.Text = "انتخاب خریدار / مشتری"
                dialog.Size = New Size(450, 400)
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.RightToLeft = RightToLeft.Yes
                dialog.RightToLeftLayout = True
                dialog.Font = Me.Font

                Dim txtSearchBox As New TextBox() With {.Dock = DockStyle.Top, .Margin = New Padding(10)}
                Dim grid As New DataGridView() With {
                    .Dock = DockStyle.Fill,
                    .ReadOnly = True,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .MultiSelect = False,
                    .AllowUserToAddRows = False,
                    .RowHeadersVisible = False,
                    .AutoGenerateColumns = True
                }

                dialog.Controls.Add(grid)
                dialog.Controls.Add(txtSearchBox)

                Dim dtPersons = LoadCustomersDataTable()
                grid.DataSource = dtPersons

                AddHandler txtSearchBox.TextChanged, Sub()
                                                         If dtPersons IsNot Nothing Then
                                                             Dim filter = txtSearchBox.Text.Trim().Replace("'", "''")
                                                             dtPersons.DefaultView.RowFilter = $"FullName LIKE '%{filter}%' OR Mobile LIKE '%{filter}%'"
                                                         End If
                                                     End Sub

                AddHandler grid.CellDoubleClick, Sub()
                                                     If grid.CurrentRow IsNot Nothing Then
                                                         txtCustomerName.Text = Convert.ToString(grid.CurrentRow.Cells("FullName").Value)
                                                         dialog.DialogResult = DialogResult.OK
                                                         dialog.Close()
                                                     End If
                                                 End Sub

                dialog.ShowDialog(Me)
            End Using
        End Sub

        Private Function LoadCustomersDataTable() As DataTable
            Try
                Dim compId = SessionContext.CurrentCompanyID
                Dim sqlText = "SELECT PersonID, FullName, Mobile, RoleType FROM Persons WHERE (RoleType = 'خریدار' OR RoleType = 'هر دو' OR RoleType = 'مشتری')"
                If compId.HasValue Then
                    sqlText &= " AND CompanyID = " & compId.Value
                End If
                sqlText &= " ORDER BY FullName"
                Return Sql.ExecuteTable(sqlText)
            Catch ex As Exception
                Return New DataTable()
            End Try
        End Function

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

        Private Sub AnbarMiniForooshForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.F2 Then
                If _editingInvoiceId.HasValue Then
                    btnSaveAndPrint_Click(Nothing, Nothing)
                Else
                    ShowProductSelector(txtBarcodeScan.Text.Trim())
                End If
            ElseIf e.KeyCode = Keys.F3 Then
                ResetForm()
            End If
        End Sub

        Private Sub btnBrowseProduct_Click(sender As Object, e As EventArgs) Handles btnBrowseProduct.Click
            ShowProductSelector(txtBarcodeScan.Text.Trim())
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
            If String.IsNullOrEmpty(term) Then
                ShowProductSelector("")
                Return
            End If

            Dim compId = SessionContext.CurrentCompanyID
            Dim dt As DataTable
            If compId.HasValue Then
                dt = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE CompanyID = ? AND (Barcode = ? OR ProductCode = ? OR ProductName LIKE ?)", compId.Value, term, term, "%" & term & "%")
            Else
                dt = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE Barcode = ? OR ProductCode = ? OR ProductName LIKE ?", term, term, "%" & term & "%")
            End If

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                If MessageBox.Show("کالایی با این مشخصات یافت نشد. آیا مایلید لیست کالاها را باز کنید؟", "کالا یافت نشد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    ShowProductSelector(term)
                Else
                    txtBarcodeScan.SelectAll()
                End If
                Return
            ElseIf dt.Rows.Count > 1 Then
                ShowProductSelector(term)
                Return
            End If

            AddProductRowToCart(dt.Rows(0))
        End Sub

        Private Sub AddProductRowToCart(row As DataRow)
            Dim productId = Convert.ToInt32(row("ProductID"))
            Dim code = Convert.ToString(row("Code"))
            Dim name = Convert.ToString(row("Name"))
            Dim unit = If(row.Table.Columns.Contains("PrimaryUnit") AndAlso Not row.IsNull("PrimaryUnit"), Convert.ToString(row("PrimaryUnit")), "عدد")
            Dim salesPrice = If(row.Table.Columns.Contains("SalesPrice") AndAlso Not row.IsNull("SalesPrice"), Convert.ToDecimal(row("SalesPrice")), 0D)

            Dim existingRow As DataGridViewRow = Nothing
            For Each r As DataGridViewRow In dgvCart.Rows
                If Convert.ToInt32(r.Cells("colProductID").Value) = productId Then
                    existingRow = r
                    Exit For
                End If
            Next

            If existingRow IsNot Nothing Then
                Dim currentQty = Convert.ToDecimal(existingRow.Cells("colQuantity").Value)
                existingRow.Cells("colQuantity").Value = currentQty + 1
                Dim price = Convert.ToDecimal(existingRow.Cells("colUnitPrice").Value.ToString().Replace(",", ""))
                existingRow.Cells("colTotalPrice").Value = ((currentQty + 1) * price).ToString("N0")
            Else
                dgvCart.Rows.Add(productId, code, name, unit, 1, salesPrice.ToString("N0"), salesPrice.ToString("N0"))
            End If

            RecalculateTotal()
            txtBarcodeScan.Clear()
            txtBarcodeScan.Focus()
        End Sub

        Private Sub ShowProductSelector(Optional initialSearch As String = "")
            Dim compId = SessionContext.CurrentCompanyID
            Dim dtAll As DataTable
            If compId.HasValue Then
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE CompanyID = ? AND IsActive = 1 ORDER BY ProductCode", compId.Value)
            Else
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE IsActive = 1 ORDER BY ProductCode")
            End If

            If dtAll Is Nothing OrElse dtAll.Rows.Count = 0 Then
                MessageBox.Show("هیچ کالای فعالی در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "جستجو و انتخاب کالا جهت فروش (F2)"
                dlg.Size = New Size(750, 480)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True
                dlg.Font = New Font("Tahoma", 9.0!)

                Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 45}
                Dim lblFilter As New Label() With {.Text = "جستجو:", .AutoSize = True, .Location = New Point(680, 12)}
                Dim txtFilter As New TextBox() With {.Location = New Point(230, 9), .Size = New Size(440, 26), .Text = initialSearch}
                pnlTop.Controls.Add(lblFilter)
                pnlTop.Controls.Add(txtFilter)

                Dim dgv As New DataGridView() With {
                    .Dock = DockStyle.Fill,
                    .ReadOnly = True,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .MultiSelect = False,
                    .AllowUserToAddRows = False,
                    .RowHeadersVisible = False,
                    .AutoGenerateColumns = True,
                    .DataSource = dtAll
                }

                dlg.Controls.Add(dgv)
                dlg.Controls.Add(pnlTop)

                AddHandler txtFilter.TextChanged, Sub()
                                                      Dim f = txtFilter.Text.Trim().Replace("'", "''")
                                                      dtAll.DefaultView.RowFilter = $"Code LIKE '%{f}%' OR Name LIKE '%{f}%' OR Barcode LIKE '%{f}%'"
                                                  End Sub

                AddHandler dgv.CellDoubleClick, Sub(s, e)
                                                    If e.RowIndex >= 0 Then
                                                        Dim selectedRow = CType(dgv.Rows(e.RowIndex).DataBoundItem, DataRowView).Row
                                                        AddProductRowToCart(selectedRow)
                                                        dlg.DialogResult = DialogResult.OK
                                                        dlg.Close()
                                                    End If
                                                End Sub

                If Not String.IsNullOrEmpty(initialSearch) Then
                    dtAll.DefaultView.RowFilter = $"Code LIKE '%{initialSearch}%' OR Name LIKE '%{initialSearch}%' OR Barcode LIKE '%{initialSearch}%'"
                End If

                dlg.ShowDialog(Me)
            End Using
        End Sub

        Private Sub dgvCart_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvCart.CurrentCellDirtyStateChanged
            If dgvCart.IsCurrentCellDirty Then
                dgvCart.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End Sub

        Private Sub dgvCart_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellValueChanged
            If e.RowIndex >= 0 Then
                Dim colName = dgvCart.Columns(e.ColumnIndex).Name
                If colName = "colQuantity" OrElse colName = "colUnitPrice" Then
                    RecalculateRowAndTotal(e.RowIndex)
                End If
            End If
        End Sub

        Private Sub dgvCart_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellEndEdit
            If e.RowIndex >= 0 Then
                RecalculateRowAndTotal(e.RowIndex)
            End If
        End Sub

        Private Sub RecalculateRowAndTotal(rowIndex As Integer)
            Try
                Dim row = dgvCart.Rows(rowIndex)
                Dim qty As Decimal = 0D
                Dim unitPrice As Decimal = 0D

                Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value).Replace(",", ""), unitPrice)

                row.Cells("colUnitPrice").Value = unitPrice.ToString("N0")
                row.Cells("colTotalPrice").Value = (qty * unitPrice).ToString("N0")

                RecalculateTotal()
            Catch
            End Try
        End Sub

        Private Sub RecalculateTotal()
            Dim total As Decimal = 0D
            For Each row As DataGridViewRow In dgvCart.Rows
                Dim rowTotal As Decimal = 0D
                If row.Cells("colTotalPrice").Value IsNot Nothing Then
                    Decimal.TryParse(row.Cells("colTotalPrice").Value.ToString().Replace(",", ""), rowTotal)
                End If
                total += rowTotal
            Next

            lblTotalAmountValue.Text = total.ToString("N0") & " ریال"
        End Sub

        Private Sub btnSaveAndPrint_Click(sender As Object, e As EventArgs) Handles btnSaveAndPrint.Click
            If dgvCart.Rows.Count = 0 Then
                MessageBox.Show("سبد خرید خالی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim custName = If(String.IsNullOrWhiteSpace(txtCustomerName.Text), "مشتری نقدی", txtCustomerName.Text.Trim())
            Dim currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()
            For Each gRow As DataGridViewRow In dgvCart.Rows
                Dim pId = Convert.ToInt32(gRow.Cells("colProductID").Value)
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(gRow.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(gRow.Cells("colUnitPrice").Value).Replace(",", ""), price)
                lines.Add(New Tuple(Of Integer, Decimal, Decimal)(pId, qty, price))
            Next

            Dim targetWarehouseId As Integer = defaultWarehouseId
            If cmbWarehouse.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbWarehouse.SelectedValue) Then
                targetWarehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue)
            End If

            Dim pType = If(cmbPaymentType.SelectedItem IsNot Nothing, cmbPaymentType.SelectedItem.ToString(), "کارتخوان (POS)")
            Dim descText = If(String.IsNullOrWhiteSpace(txtDescription.Text), "فاکتور فروش نسخه مینی", txtDescription.Text.Trim())

            Try
                If _editingInvoiceId.HasValue AndAlso _editingInvoiceId.Value > 0 Then
                    invoiceService.UpdateSalesInvoice(_editingInvoiceId.Value, txtInvoiceNo.Text, DateTime.Now, custName, targetWarehouseId, currentUserId, lines, pType, descText)
                    MessageBox.Show("تغییرات فاکتور فروش با موفقیت ثبت گردید." & Environment.NewLine & "موجودی انبار به‌روزرسانی شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim invoiceId = invoiceService.SaveSalesInvoice(txtInvoiceNo.Text, DateTime.Now, custName, targetWarehouseId, currentUserId, lines, pType, descText)
                    MessageBox.Show("فاکتور فروش با موفقیت ثبت گردید." & Environment.NewLine & "موجودی انبار به‌روزرسانی شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ResetForm()
                RaiseEvent InvoiceSaved()
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور فروش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnNewInvoice_Click(sender As Object, e As EventArgs) Handles btnNewInvoice.Click
            ResetForm()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            RaiseEvent InvoiceCancelled()
        End Sub
    End Class
End Namespace
