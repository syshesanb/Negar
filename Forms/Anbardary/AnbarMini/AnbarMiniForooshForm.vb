Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniForooshForm
        Private ReadOnly catalogService As New CatalogService()
        Private ReadOnly invoiceService As New InvoiceService()
        Private ReadOnly defaultWarehouseId As Integer = 1

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniForooshForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            txtInvoiceDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            GenerateNextInvoiceNumber()
            cmbPaymentType.SelectedIndex = 0 ' کارتخوان (POS)
            txtBarcodeScan.Focus()
        End Sub

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
                ShowProductSelector(txtBarcodeScan.Text.Trim())
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
                dt = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND (Barcode = ? OR ProductCode = ? OR ProductName LIKE ?)", compId.Value, term, term, "%" & term & "%")
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
            For Each gRow As DataGridViewRow In dgvCart.Rows
                If Convert.ToInt32(gRow.Cells("colProductID").Value) = productId Then
                    existingRow = gRow
                    Exit For
                End If
            Next

            If existingRow IsNot Nothing Then
                Dim currentQty = Convert.ToDecimal(existingRow.Cells("colQuantity").Value)
                existingRow.Cells("colQuantity").Value = currentQty + 1
                RecalculateRowTotal(existingRow)
            Else
                Dim rowIndex = dgvCart.Rows.Add(productId, code, name, unit, 1, salesPrice, salesPrice)
                RecalculateRowTotal(dgvCart.Rows(rowIndex))
            End If

            RecalculateCartTotal()
            txtBarcodeScan.Clear()
            txtBarcodeScan.Focus()
        End Sub

        Private Sub ShowProductSelector(Optional initialSearch As String = "")
            Dim compId = SessionContext.CurrentCompanyID
            Dim dtAll As DataTable
            If compId.HasValue Then
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE (CompanyID = ? OR CompanyID IS NULL) AND IsActive = 1 ORDER BY ProductCode", compId.Value)
            Else
                dtAll = Sql.ExecuteTable("SELECT ProductID, ProductCode AS Code, ProductName AS Name, Unit AS PrimaryUnit, DefaultPrice AS SalesPrice, Barcode FROM Products WHERE IsActive = 1 ORDER BY ProductCode")
            End If

            If dtAll Is Nothing OrElse dtAll.Rows.Count = 0 Then
                MessageBox.Show("هیچ کالای فعالی در سیستم تعریف نشده است.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using dlg As New Form()
                dlg.Text = "جستجو و انتخاب کالا (F2)"
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
                If dgv.Columns.Contains("SalesPrice") Then dgv.Columns("SalesPrice").HeaderText = "قیمت فروش"
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
                    AddProductRowToCart(DirectCast(dlg.Tag, DataRow))
                End If
            End Using
        End Sub

        Private Sub RecalculateRowTotal(row As DataGridViewRow)
            Dim qty As Decimal = 0D
            Dim price As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
            Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value), price)

            Dim total = qty * price
            row.Cells("colTotalPrice").Value = total.ToString("N0")
        End Sub

        Private Sub RecalculateCartTotal()
            Dim grandTotal As Decimal = 0D
            For Each row As DataGridViewRow In dgvCart.Rows
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(row.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(row.Cells("colUnitPrice").Value), price)
                grandTotal += (qty * price)
            Next

            lblTotalAmountValue.Text = grandTotal.ToString("N0") & " ریال"
        End Sub

        Private Sub dgvCart_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellValueChanged
            If e.RowIndex >= 0 AndAlso (e.ColumnIndex = dgvCart.Columns("colQuantity").Index OrElse e.ColumnIndex = dgvCart.Columns("colUnitPrice").Index) Then
                RecalculateRowTotal(dgvCart.Rows(e.RowIndex))
                RecalculateCartTotal()
            End If
        End Sub

        Private Sub btnSaveAndPrint_Click(sender As Object, e As EventArgs) Handles btnSaveAndPrint.Click
            SaveInvoice()
        End Sub

        Private Sub SaveInvoice()
            If dgvCart.Rows.Count = 0 Then
                MessageBox.Show("سبد خرید خالی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)
            Dim customerName = If(String.IsNullOrWhiteSpace(txtCustomerName.Text), "مشتری نقدی", txtCustomerName.Text.Trim())

            Dim lines As New List(Of Tuple(Of Integer, Decimal, Decimal))()
            For Each gRow As DataGridViewRow In dgvCart.Rows
                Dim pId = Convert.ToInt32(gRow.Cells("colProductID").Value)
                Dim qty As Decimal = 0D
                Dim price As Decimal = 0D
                Decimal.TryParse(Convert.ToString(gRow.Cells("colQuantity").Value), qty)
                Decimal.TryParse(Convert.ToString(gRow.Cells("colUnitPrice").Value), price)
                lines.Add(New Tuple(Of Integer, Decimal, Decimal)(pId, qty, price))
            Next

            Try
                Dim invoiceId = invoiceService.SaveSalesInvoice(txtInvoiceNo.Text, DateTime.Now, customerName, defaultWarehouseId, currentUserId, lines)
                MessageBox.Show("فاکتور فروش با موفقیت ثبت شد." & Environment.NewLine & "شماره فاکتور: " & txtInvoiceNo.Text, "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ClearInvoice()
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnNewInvoice_Click(sender As Object, e As EventArgs) Handles btnNewInvoice.Click
            ClearInvoice()
        End Sub

        Private Sub ClearInvoice()
            dgvCart.Rows.Clear()
            GenerateNextInvoiceNumber()
            RecalculateCartTotal()
            txtCustomerName.Text = "مشتری نقدی"
            txtBarcodeScan.Clear()
            txtBarcodeScan.Focus()
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            If keyData = Keys.F2 Then
                SaveInvoice()
                Return True
            ElseIf keyData = Keys.F3 Then
                ClearInvoice()
                Return True
            End If
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function
    End Class
End Namespace
