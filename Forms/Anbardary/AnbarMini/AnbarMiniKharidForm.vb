Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniKharidForm
        Public Event InvoiceSaved()
        Public Event InvoiceCancelled()

        Private ReadOnly invoiceService As New InvoiceService()
        Private ReadOnly defaultWarehouseId As Integer = 1

        Public Sub New()
            InitializeComponent()
        End Sub

        Private isFormattingDate As Boolean = False

        Private Sub AnbarMiniKharidForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ResetForm()
        End Sub

        Public Sub ResetForm()
            txtInvoiceDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            txtVendorName.Clear()
            txtProductSearch.Clear()
            txtUnitPrice.Clear()
            numQuantity.Value = 1
            dgvItems.Rows.Clear()
            GenerateNextInvoiceNumber()
            LoadWarehouses()
            RecalculateTotal()
        End Sub

        Private Sub cmbWarehouse_DropDown(sender As Object, e As EventArgs) Handles cmbWarehouse.DropDown
            LoadWarehouses()
        End Sub

        Public Sub LoadWarehouses()
            Try
                Dim selectedVal = cmbWarehouse.SelectedValue
                Dim compId = SessionContext.CurrentCompanyID
                Dim dt As DataTable
                If compId.HasValue Then
                    dt = Sql.ExecuteTable("SELECT WarehouseID, WarehouseName FROM Warehouses WHERE (CompanyID = ? OR CompanyID IS NULL) AND IsActive = 1 ORDER BY WarehouseID", compId.Value)
                Else
                    dt = Sql.ExecuteTable("SELECT WarehouseID, WarehouseName FROM Warehouses WHERE IsActive = 1 ORDER BY WarehouseID")
                End If

                cmbWarehouse.DataSource = Nothing
                cmbWarehouse.Items.Clear()

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    cmbWarehouse.DisplayMember = "WarehouseName"
                    cmbWarehouse.ValueMember = "WarehouseID"
                    cmbWarehouse.DataSource = dt

                    If selectedVal IsNot Nothing Then
                        Try
                            cmbWarehouse.SelectedValue = selectedVal
                        Catch
                        End Try
                    End If

                    If cmbWarehouse.SelectedIndex < 0 AndAlso cmbWarehouse.Items.Count > 0 Then
                        cmbWarehouse.SelectedIndex = 0
                    End If
                Else
                    cmbWarehouse.Items.Add("انبار اصلی")
                    cmbWarehouse.SelectedIndex = 0
                End If
            Catch ex As Exception
                Console.WriteLine("Error loading warehouses: " & ex.Message)
            End Try
        End Sub

        Private Sub btnPickDate_Click(sender As Object, e As EventArgs) Handles btnPickDate.Click
            Using cal As New PersianCalendarForm(txtInvoiceDate.Text)
                If cal.ShowDialog() = DialogResult.OK AndAlso Not String.IsNullOrEmpty(cal.SelectedDate) Then
                    txtInvoiceDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub txtInvoiceDate_TextChanged(sender As Object, e As EventArgs) Handles txtInvoiceDate.TextChanged
            If isFormattingDate Then Return
            isFormattingDate = True
            Try
                Dim textVal = txtInvoiceDate.Text
                Dim digitsOnly As String = New String(textVal.Where(Function(c) Char.IsDigit(c)).ToArray())
                If digitsOnly.Length > 8 Then
                    digitsOnly = digitsOnly.Substring(0, 8)
                End If

                Dim formatted As String = digitsOnly
                If digitsOnly.Length >= 7 Then
                    formatted = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4, 2) & "/" & digitsOnly.Substring(6)
                ElseIf digitsOnly.Length >= 5 Then
                    formatted = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4)
                End If

                Dim selStart As Integer = txtInvoiceDate.SelectionStart
                Dim oldLen As Integer = txtInvoiceDate.Text.Length
                txtInvoiceDate.Text = formatted
                Dim newLen As Integer = txtInvoiceDate.Text.Length
                txtInvoiceDate.SelectionStart = Math.Max(0, Math.Min(newLen, selStart + (newLen - oldLen)))
            Finally
                isFormattingDate = False
            End Try
        End Sub

        Private Sub btnPickVendor_Click(sender As Object, e As EventArgs) Handles btnPickVendor.Click
            ShowVendorSelector()
        End Sub

        Private Sub ShowVendorSelector()
            Dim compId = SessionContext.CurrentCompanyID
            Dim dtVendors As DataTable
            If compId.HasValue Then
                dtVendors = Sql.ExecuteTable("SELECT PersonID, PersonCode, (CASE WHEN PersonType='حقوقی' THEN CompanyName ELSE (FirstName || ' ' || LastName) END) AS DisplayName, RoleType, Mobile, Phone FROM Persons WHERE (CompanyID = ? OR CompanyID IS NULL) AND IsActive = 1 AND RoleType IN ('فروشنده', 'هر دو') ORDER BY PersonCode", compId.Value)
            Else
                dtVendors = Sql.ExecuteTable("SELECT PersonID, PersonCode, (CASE WHEN PersonType='حقوقی' THEN CompanyName ELSE (FirstName || ' ' || LastName) END) AS DisplayName, RoleType, Mobile, Phone FROM Persons WHERE IsActive = 1 AND RoleType IN ('فروشنده', 'هر دو') ORDER BY PersonCode")
            End If

            If dtVendors Is Nothing OrElse dtVendors.Rows.Count = 0 Then
                MessageBox.Show("هیچ فروشنده/تامین‌کننده‌ای در سیستم ثبت نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "انتخاب فروشنده / تامین‌کننده"
                dlg.Size = New Size(650, 420)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True
                dlg.Font = New Font("Tahoma", 9.0!)

                Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 45}
                Dim lblFilter As New Label() With {.Text = "جستجو:", .AutoSize = True, .Location = New Point(580, 12)}
                Dim txtFilter As New TextBox() With {.Location = New Point(180, 9), .Size = New Size(390, 26)}
                pnlTop.Controls.Add(lblFilter)
                pnlTop.Controls.Add(txtFilter)

                Dim dgv As New DataGridView() With {
                    .Dock = DockStyle.Fill,
                    .ReadOnly = True,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .MultiSelect = False,
                    .AllowUserToAddRows = False,
                    .RowHeadersVisible = False,
                    .DataSource = dtVendors
                }
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)

                If dgv.Columns.Contains("PersonID") Then dgv.Columns("PersonID").Visible = False
                If dgv.Columns.Contains("PersonCode") Then dgv.Columns("PersonCode").HeaderText = "کد"
                If dgv.Columns.Contains("DisplayName") Then dgv.Columns("DisplayName").HeaderText = "نام فروشنده / شرکت"
                If dgv.Columns.Contains("RoleType") Then dgv.Columns("RoleType").HeaderText = "نقش"
                If dgv.Columns.Contains("Mobile") Then dgv.Columns("Mobile").HeaderText = "همراه"
                If dgv.Columns.Contains("Phone") Then dgv.Columns("Phone").HeaderText = "تلفن"

                Dim pnlBottom As New Panel() With {.Dock = DockStyle.Bottom, .Height = 45}
                Dim btnSelect As New Button() With {.Text = "انتخاب", .BackColor = Color.FromArgb(46, 204, 113), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Location = New Point(15, 8), .Size = New Size(100, 30)}
                Dim btnCancel As New Button() With {.Text = "انصراف", .DialogResult = DialogResult.Cancel, .Location = New Point(125, 8), .Size = New Size(90, 30)}
                pnlBottom.Controls.Add(btnSelect)
                pnlBottom.Controls.Add(btnCancel)

                dlg.Controls.Add(dgv)
                dlg.Controls.Add(pnlTop)
                dlg.Controls.Add(pnlBottom)

                Dim applyFilter = Sub()
                                      Dim f = txtFilter.Text.Trim().Replace("'", "''")
                                      If String.IsNullOrEmpty(f) Then
                                          dtVendors.DefaultView.RowFilter = ""
                                      Else
                                          dtVendors.DefaultView.RowFilter = $"PersonCode LIKE '%{f}%' OR DisplayName LIKE '%{f}%' OR Mobile LIKE '%{f}%'"
                                      End If
                                  End Sub

                AddHandler txtFilter.TextChanged, Sub(s, ev) applyFilter()
                AddHandler btnSelect.Click, Sub(s, ev)
                                                If dgv.CurrentRow IsNot Nothing Then
                                                    Dim drv = DirectCast(dgv.CurrentRow.DataBoundItem, DataRowView)
                                                    txtVendorName.Text = Convert.ToString(drv.Row("DisplayName"))
                                                    dlg.DialogResult = DialogResult.OK
                                                    dlg.Close()
                                                End If
                                            End Sub
                AddHandler dgv.CellDoubleClick, Sub(s, ev)
                                                    If ev.RowIndex >= 0 Then
                                                        Dim drv = DirectCast(dgv.Rows(ev.RowIndex).DataBoundItem, DataRowView)
                                                        txtVendorName.Text = Convert.ToString(drv.Row("DisplayName"))
                                                        dlg.DialogResult = DialogResult.OK
                                                        dlg.Close()
                                                    End If
                                                End Sub

                dlg.ShowDialog()
            End Using
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

        Private Sub AnbarMiniKharidForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.F2 Then
                ShowProductSelector(txtProductSearch.Text.Trim())
            End If
        End Sub

        Private Sub btnBrowseProduct_Click(sender As Object, e As EventArgs) Handles btnBrowseProduct.Click
            ShowProductSelector(txtProductSearch.Text.Trim())
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
            If String.IsNullOrEmpty(term) Then
                ShowProductSelector("")
                Return
            End If

            Dim compId = SessionContext.CurrentCompanyID
            Dim dt As DataTable
            If compId.HasValue Then
                dt = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, PurchasePrice, Barcode FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND (Barcode = ? OR ProductCode = ? OR ProductName LIKE ?)", compId.Value, term, term, "%" & term & "%")
            Else
                dt = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, PurchasePrice, Barcode FROM Products WHERE Barcode = ? OR ProductCode = ? OR ProductName LIKE ?", term, term, "%" & term & "%")
            End If

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                If MessageBox.Show("کالای مورد نظر یافت نشد. آیا مایلید لیست کالاها را باز کنید؟", "کالا یافت نشد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    ShowProductSelector(term)
                Else
                    txtProductSearch.SelectAll()
                End If
                Return
            ElseIf dt.Rows.Count > 1 Then
                ShowProductSelector(term)
                Return
            End If

            AddProductRowToInvoice(dt.Rows(0))
        End Sub

        Private Sub AddProductRowToInvoice(row As DataRow)
            Dim productId = Convert.ToInt32(row("ProductID"))
            Dim code = Convert.ToString(row("Code"))
            Dim name = Convert.ToString(row("Name"))

            Dim unitPrice As Decimal = 0D
            If Not String.IsNullOrWhiteSpace(txtUnitPrice.Text) Then
                Decimal.TryParse(txtUnitPrice.Text.Replace(",", ""), unitPrice)
            ElseIf row.Table.Columns.Contains("PurchasePrice") AndAlso Not row.IsNull("PurchasePrice") Then
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

        Private Sub ShowProductSelector(Optional initialSearch As String = "")
            Dim compId = SessionContext.CurrentCompanyID
            Dim dtAll As DataTable
            If compId.HasValue Then
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, PurchasePrice, Barcode FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND IsActive = 1 ORDER BY ProductCode", compId.Value)
            Else
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, PurchasePrice, Barcode FROM Products WHERE IsActive = 1 ORDER BY ProductCode")
            End If

            If dtAll Is Nothing OrElse dtAll.Rows.Count = 0 Then
                MessageBox.Show("هیچ کالای فعالی در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "جستجو و انتخاب کالا جهت خرید (F2)"
                dlg.Size = New Size(750, 480)
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.RightToLeftLayout = True
                dlg.Font = New Font("Tahoma", 9.0!)

                Dim pnlTop As New Panel()
                pnlTop.Dock = DockStyle.Top
                pnlTop.Height = 45

                Dim lblFilter As New Label()
                lblFilter.Text = "جستجو:"
                lblFilter.AutoSize = True
                lblFilter.Location = New Point(680, 12)

                Dim txtFilter As New TextBox()
                txtFilter.Location = New Point(230, 9)
                txtFilter.Size = New Size(440, 26)
                txtFilter.Text = initialSearch

                pnlTop.Controls.Add(lblFilter)
                pnlTop.Controls.Add(txtFilter)

                Dim dgv As New DataGridView()
                dgv.Dock = DockStyle.Fill
                dgv.ReadOnly = True
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                dgv.MultiSelect = False
                dgv.AllowUserToAddRows = False
                dgv.RowHeadersVisible = False
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
                dgv.DataSource = dtAll

                If dgv.Columns.Contains("ProductID") Then dgv.Columns("ProductID").Visible = False
                If dgv.Columns.Contains("Code") Then dgv.Columns("Code").HeaderText = "کد کالا"
                If dgv.Columns.Contains("Name") Then dgv.Columns("Name").HeaderText = "نام کالا"
                If dgv.Columns.Contains("PrimaryUnit") Then dgv.Columns("PrimaryUnit").HeaderText = "واحد"
                If dgv.Columns.Contains("PurchasePrice") Then dgv.Columns("PurchasePrice").HeaderText = "قیمت خرید"
                If dgv.Columns.Contains("Barcode") Then dgv.Columns("Barcode").HeaderText = "بارکد"

                Dim pnlBottom As New Panel()
                pnlBottom.Dock = DockStyle.Bottom
                pnlBottom.Height = 45

                Dim btnSelect As New Button()
                btnSelect.Text = "انتخاب و افزودن"
                btnSelect.BackColor = Color.FromArgb(39, 174, 96)
                btnSelect.ForeColor = Color.White
                btnSelect.FlatStyle = FlatStyle.Flat
                btnSelect.Location = New Point(15, 8)
                btnSelect.Size = New Size(130, 30)

                Dim btnCancel As New Button()
                btnCancel.Text = "انصراف"
                btnCancel.DialogResult = DialogResult.Cancel
                btnCancel.Location = New Point(155, 8)
                btnCancel.Size = New Size(90, 30)

                pnlBottom.Controls.Add(btnSelect)
                pnlBottom.Controls.Add(btnCancel)

                dlg.Controls.Add(dgv)
                dlg.Controls.Add(pnlTop)
                dlg.Controls.Add(pnlBottom)

                Dim applyFilter = Sub()
                                      Dim filterTerm = txtFilter.Text.Trim().Replace("'", "''")
                                      If String.IsNullOrEmpty(filterTerm) Then
                                          dtAll.DefaultView.RowFilter = ""
                                      Else
                                          dtAll.DefaultView.RowFilter = $"Code LIKE '%{filterTerm}%' OR Name LIKE '%{filterTerm}%' OR Barcode LIKE '%{filterTerm}%'"
                                      End If
                                  End Sub

                AddHandler txtFilter.TextChanged, Sub(s, ev) applyFilter()
                applyFilter()

                AddHandler btnSelect.Click, Sub(s, ev)
                                                If dgv.CurrentRow IsNot Nothing Then
                                                    dlg.Tag = DirectCast(dgv.CurrentRow.DataBoundItem, DataRowView).Row
                                                    dlg.DialogResult = DialogResult.OK
                                                    dlg.Close()
                                                End If
                                            End Sub

                AddHandler dgv.CellDoubleClick, Sub(s, ev)
                                                    If ev.RowIndex >= 0 Then
                                                        dlg.Tag = DirectCast(dgv.Rows(ev.RowIndex).DataBoundItem, DataRowView).Row
                                                        dlg.DialogResult = DialogResult.OK
                                                        dlg.Close()
                                                    End If
                                                End Sub

                If dlg.ShowDialog() = DialogResult.OK AndAlso dlg.Tag IsNot Nothing Then
                    AddProductRowToInvoice(DirectCast(dlg.Tag, DataRow))
                End If
            End Using
        End Sub

        Private isRecalculating As Boolean = False

        Private Sub RecalculateRowAndGrandTotal(row As DataGridViewRow)
            If isRecalculating OrElse row Is Nothing OrElse row.IsNewRow Then Return
            isRecalculating = True
            Try
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Dim qtyStr = Convert.ToString(row.Cells("colQuantity").Value)
                Dim priceStr = Convert.ToString(row.Cells("colUnitPrice").Value)

                If Not String.IsNullOrWhiteSpace(qtyStr) Then
                    Decimal.TryParse(qtyStr.Replace(",", ""), qty)
                End If
                If Not String.IsNullOrWhiteSpace(priceStr) Then
                    Decimal.TryParse(priceStr.Replace(",", ""), price)
                End If

                Dim rowTotal = qty * price
                row.Cells("colTotalPrice").Value = rowTotal.ToString("N0")

                RecalculateTotal()
            Finally
                isRecalculating = False
            End Try
        End Sub

        Private Sub dgvItems_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvItems.CellValueChanged
            If e.RowIndex >= 0 AndAlso e.RowIndex < dgvItems.Rows.Count Then
                Dim colName = dgvItems.Columns(e.ColumnIndex).Name
                If colName = "colQuantity" OrElse colName = "colUnitPrice" Then
                    RecalculateRowAndGrandTotal(dgvItems.Rows(e.RowIndex))
                End If
            End If
        End Sub

        Private Sub dgvItems_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvItems.CurrentCellDirtyStateChanged
            If dgvItems.IsCurrentCellDirty Then
                dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End Sub

        Private Sub dgvItems_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvItems.CellEndEdit
            If e.RowIndex >= 0 AndAlso e.RowIndex < dgvItems.Rows.Count Then
                Dim row = dgvItems.Rows(e.RowIndex)
                Dim colName = dgvItems.Columns(e.ColumnIndex).Name
                If colName = "colUnitPrice" Then
                    Dim price As Decimal = 0D
                    If Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value).Replace(",", ""), price) Then
                        row.Cells("colUnitPrice").Value = price.ToString("N0")
                    End If
                End If
            End If
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

            Dim targetWarehouseId As Integer = defaultWarehouseId
            If cmbWarehouse.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbWarehouse.SelectedValue) Then
                targetWarehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue)
            End If

            Try
                Dim invoiceId = invoiceService.SavePurchaseInvoice(txtInvoiceNo.Text, DateTime.Now, vendorName, targetWarehouseId, currentUserId, lines, "فاکتور خرید", 0D, "نقدی", "فاکتور خرید نسخه مینی")
                MessageBox.Show("فاکتور خرید با موفقیت ثبت شد." & Environment.NewLine & "موجودی انبار به‌روزرسانی گردید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ResetForm()
                RaiseEvent InvoiceSaved()
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور خرید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            RaiseEvent InvoiceCancelled()
        End Sub
    End Class
End Namespace
