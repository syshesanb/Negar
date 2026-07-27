Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Business.PersianDateHelper

Namespace Negar.Forms
    Public Class AnbardaryForoosh1Form
        Inherits Form

        Private ReadOnly _invoiceService As New InvoiceService()
        Private _invoicesTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()
        Private filterTextBoxesHavaleh As New Dictionary(Of String, TextBox)()
        Private filterTextBoxesBargasht As New Dictionary(Of String, TextBox)()
        Private filterTextBoxesHavalehBargasht As New Dictionary(Of String, TextBox)()

        Private Const ColNameReceipt As String = "colReceipt"
        Private Const ColNameEdit As String = "colEdit"
        Private Const ColNameDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryForoosh1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            ' Style DataGridViews
            If Me.dgvInvoices IsNot Nothing Then ApplyGridStyling(Me.dgvInvoices)
            If Me.dgvInvoicesHavaleh IsNot Nothing Then ApplyGridStyling(Me.dgvInvoicesHavaleh)
            If Me.dgvInvoicesBargasht IsNot Nothing Then ApplyGridStyling(Me.dgvInvoicesBargasht)
            If Me.dgvInvoicesHavalehBargasht IsNot Nothing Then ApplyGridStyling(Me.dgvInvoicesHavalehBargasht)

            ConfigureGridForoosh(dgvInvoices)
            ConfigureGridHavaleh(dgvInvoicesHavaleh)
            ConfigureGridBargasht(dgvInvoicesBargasht)
            ConfigureGridHavalehBargasht(dgvInvoicesHavalehBargasht)

            LoadData()

            CreateFilterTextBoxes(dgvInvoices, pnlFilters, filterTextBoxes, AddressOf FilterTextBox_TextChanged)
            CreateFilterTextBoxes(dgvInvoicesHavaleh, pnlFiltersHavaleh, filterTextBoxesHavaleh, AddressOf FilterTextBoxHavaleh_TextChanged)
            CreateFilterTextBoxes(dgvInvoicesBargasht, pnlFiltersBargasht, filterTextBoxesBargasht, AddressOf FilterTextBoxBargasht_TextChanged)
            CreateFilterTextBoxes(dgvInvoicesHavalehBargasht, pnlFiltersHavalehBargasht, filterTextBoxesHavalehBargasht, AddressOf FilterTextBoxHavalehBargasht_TextChanged)

            AddHandler dgvInvoices.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvInvoices.Scroll, AddressOf DgvInvoices_Scroll
            AddHandler dgvInvoicesHavaleh.ColumnWidthChanged, AddressOf AlignSearchBoxesHavaleh
            AddHandler dgvInvoicesHavaleh.Scroll, AddressOf DgvInvoicesHavaleh_Scroll
            AddHandler dgvInvoicesBargasht.ColumnWidthChanged, AddressOf AlignSearchBoxesBargasht
            AddHandler dgvInvoicesBargasht.Scroll, AddressOf DgvInvoicesBargasht_Scroll
            AddHandler dgvInvoicesHavalehBargasht.ColumnWidthChanged, AddressOf AlignSearchBoxesHavalehBargasht
            AddHandler dgvInvoicesHavalehBargasht.Scroll, AddressOf DgvInvoicesHavalehBargasht_Scroll
            AddHandler Me.Resize, AddressOf AlignAllSearchBoxes

            AlignAllSearchBoxes()
        End Sub

        Private Sub ApplyGridStyling(grid As DataGridView)
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single
            grid.GridColor = Color.FromArgb(225, 200, 210)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 235, 242)
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(160, 30, 80)
            grid.DefaultCellStyle.SelectionForeColor = Color.White
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 242, 248)
        End Sub

        Private Sub ConfigureGridForoosh(grid As DataGridView)
            grid.AutoGenerateColumns = False
            grid.Columns.Clear()
            grid.AllowUserToResizeColumns = True

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColNameEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 60
            colEdit.FlatStyle = FlatStyle.Standard
            colEdit.ReadOnly = True

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = ColNameDelete
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.Width = 56
            colDelete.FlatStyle = FlatStyle.Standard
            colDelete.ReadOnly = True

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "InvoiceID"
            colId.DataPropertyName = "InvoiceID"
            colId.Visible = False

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.Name = "InvoiceNumber"
            colNum.DataPropertyName = "InvoiceNumber"
            colNum.HeaderText = "شماره سند / فاکتور"
            colNum.Width = 140

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "InvoiceType"
            colType.DataPropertyName = "InvoiceType"
            colType.HeaderText = "نوع سند"
            colType.Width = 130

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "PersianDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110

            Dim colCustomer As New DataGridViewTextBoxColumn()
            colCustomer.Name = "CustomerName"
            colCustomer.DataPropertyName = "CustomerName"
            colCustomer.HeaderText = "خریدار / مشتری"
            colCustomer.Width = 180

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "WarehouseName"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار مبدأ"
            colWarehouse.Width = 140
            colWarehouse.Visible = False

            Dim colTotal As New DataGridViewTextBoxColumn()
            colTotal.Name = "TotalAmount"
            colTotal.DataPropertyName = "TotalAmount"
            colTotal.HeaderText = "مبلغ کل (ریال)"
            colTotal.Width = 140
            colTotal.DefaultCellStyle.Format = "N0"
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colPayment As New DataGridViewTextBoxColumn()
            colPayment.Name = "PaymentType"
            colPayment.DataPropertyName = "PaymentType"
            colPayment.HeaderText = "تسویه"
            colPayment.Width = 100

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.Width = 200

            grid.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colNum, colType, colDate,
                colCustomer, colWarehouse, colTotal, colPayment, colDesc
            })
        End Sub

        Private Sub ConfigureGridBargasht(grid As DataGridView)
            grid.AutoGenerateColumns = False
            grid.Columns.Clear()
            grid.AllowUserToResizeColumns = True

            Dim colBtnViewBargasht As New DataGridViewButtonColumn()
            colBtnViewBargasht.Name = "colBtnViewBargasht"
            colBtnViewBargasht.HeaderText = "مشاهده"
            colBtnViewBargasht.Text = "مشاهده برگشت از فروشها"
            colBtnViewBargasht.UseColumnTextForButtonValue = True
            colBtnViewBargasht.Width = 140
            colBtnViewBargasht.FlatStyle = FlatStyle.Standard
            colBtnViewBargasht.ReadOnly = True

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "InvoiceID"
            colId.DataPropertyName = "InvoiceID"
            colId.Visible = False

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.Name = "InvoiceNumber"
            colNum.DataPropertyName = "InvoiceNumber"
            colNum.HeaderText = "شماره سند / فاکتور"
            colNum.Width = 140

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "InvoiceType"
            colType.DataPropertyName = "InvoiceType"
            colType.HeaderText = "نوع سند"
            colType.Width = 130

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "PersianDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110

            Dim colCustomer As New DataGridViewTextBoxColumn()
            colCustomer.Name = "CustomerName"
            colCustomer.DataPropertyName = "CustomerName"
            colCustomer.HeaderText = "خریدار / مشتری"
            colCustomer.Width = 180

            Dim colTotal As New DataGridViewTextBoxColumn()
            colTotal.Name = "TotalAmount"
            colTotal.DataPropertyName = "TotalAmount"
            colTotal.HeaderText = "مبلغ کل (ریال)"
            colTotal.Width = 140
            colTotal.DefaultCellStyle.Format = "N0"
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colPayment As New DataGridViewTextBoxColumn()
            colPayment.Name = "PaymentType"
            colPayment.DataPropertyName = "PaymentType"
            colPayment.HeaderText = "تسویه"
            colPayment.Width = 100

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.Width = 200

            grid.Columns.AddRange(New DataGridViewColumn() {
                colBtnViewBargasht, colId, colNum, colType, colDate,
                colCustomer, colTotal, colPayment, colDesc
            })
        End Sub

        Private Sub ConfigureGridHavaleh(grid As DataGridView)
            grid.AutoGenerateColumns = False
            grid.Columns.Clear()
            grid.AllowUserToResizeColumns = True

            Dim colBtnViewReceipts As New DataGridViewButtonColumn()
            colBtnViewReceipts.Name = "colBtnViewReceipts"
            colBtnViewReceipts.HeaderText = "مشاهده حوالهها"
            colBtnViewReceipts.Text = "مشاهده حوالهها"
            colBtnViewReceipts.UseColumnTextForButtonValue = True
            colBtnViewReceipts.Width = 100
            colBtnViewReceipts.FlatStyle = FlatStyle.Standard
            colBtnViewReceipts.ReadOnly = True

            Dim colBtnCreateInvoice As New DataGridViewButtonColumn()
            colBtnCreateInvoice.Name = "colBtnCreateInvoice"
            colBtnCreateInvoice.HeaderText = "صدور فاکتور"
            colBtnCreateInvoice.Text = "صدور فاکتور فروش"
            colBtnCreateInvoice.UseColumnTextForButtonValue = True
            colBtnCreateInvoice.Width = 110
            colBtnCreateInvoice.FlatStyle = FlatStyle.Standard
            colBtnCreateInvoice.ReadOnly = True

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "InvoiceID"
            colId.DataPropertyName = "InvoiceID"
            colId.Visible = False

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.Name = "InvoiceNumber"
            colNum.DataPropertyName = "InvoiceNumber"
            colNum.HeaderText = "شماره سند / فاکتور"
            colNum.Width = 140

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "InvoiceType"
            colType.DataPropertyName = "InvoiceType"
            colType.HeaderText = "نوع سند"
            colType.Width = 130

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "PersianDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110

            Dim colCustomer As New DataGridViewTextBoxColumn()
            colCustomer.Name = "CustomerName"
            colCustomer.DataPropertyName = "CustomerName"
            colCustomer.HeaderText = "خریدار / مشتری"
            colCustomer.Width = 180

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "WarehouseName"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار مبدأ"
            colWarehouse.Width = 140
            colWarehouse.Visible = True

            Dim colReceiptStatus As New DataGridViewTextBoxColumn()
            colReceiptStatus.Name = "ReceiptStatus"
            colReceiptStatus.DataPropertyName = "ReceiptStatus"
            colReceiptStatus.HeaderText = "وضعیت حواله"
            colReceiptStatus.Width = 110
            colReceiptStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات حواله انبار"
            colDesc.Width = 200

            grid.Columns.AddRange(New DataGridViewColumn() {
                colBtnViewReceipts, colBtnCreateInvoice, colId, colNum, colType, colDate,
                colCustomer, colWarehouse, colReceiptStatus, colDesc
            })
        End Sub

        Private Sub ConfigureGridHavalehBargasht(grid As DataGridView)
            grid.AutoGenerateColumns = False
            grid.Columns.Clear()
            grid.AllowUserToResizeColumns = True

            Dim colBtnViewReceipts As New DataGridViewButtonColumn()
            colBtnViewReceipts.Name = "colBtnViewReceipts"
            colBtnViewReceipts.HeaderText = "مشاهده رسیدها"
            colBtnViewReceipts.Text = "مشاهده رسیدها"
            colBtnViewReceipts.UseColumnTextForButtonValue = True
            colBtnViewReceipts.Width = 100
            colBtnViewReceipts.FlatStyle = FlatStyle.Standard
            colBtnViewReceipts.ReadOnly = True

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "InvoiceID"
            colId.DataPropertyName = "InvoiceID"
            colId.Visible = False

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.Name = "InvoiceNumber"
            colNum.DataPropertyName = "InvoiceNumber"
            colNum.HeaderText = "شماره سند / فاکتور"
            colNum.Width = 140

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "InvoiceType"
            colType.DataPropertyName = "InvoiceType"
            colType.HeaderText = "نوع سند"
            colType.Width = 130

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "PersianDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110

            Dim colCustomer As New DataGridViewTextBoxColumn()
            colCustomer.Name = "CustomerName"
            colCustomer.DataPropertyName = "CustomerName"
            colCustomer.HeaderText = "خریدار / مشتری"
            colCustomer.Width = 180

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "WarehouseName"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار مقصد"
            colWarehouse.Width = 140
            colWarehouse.Visible = True

            Dim colReceiptStatus As New DataGridViewTextBoxColumn()
            colReceiptStatus.Name = "ReceiptStatus"
            colReceiptStatus.DataPropertyName = "ReceiptStatus"
            colReceiptStatus.HeaderText = "وضعیت رسید"
            colReceiptStatus.Width = 110
            colReceiptStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات رسید انبار"
            colDesc.Width = 200

            grid.Columns.AddRange(New DataGridViewColumn() {
                colBtnViewReceipts, colId, colNum, colType, colDate,
                colCustomer, colWarehouse, colReceiptStatus, colDesc
            })
        End Sub

        Private Sub CreateFilterTextBoxes(grid As DataGridView, panel As Panel, dict As Dictionary(Of String, TextBox), textHandler As EventHandler)
            panel.Controls.Clear()
            dict.Clear()

            For Each col As DataGridViewColumn In grid.Columns
                Dim txt As New TextBox()
                txt.Name = "txtFilter_" & grid.Name & "_" & col.Name
                txt.Tag = col.DataPropertyName
                txt.BorderStyle = BorderStyle.FixedSingle

                If TypeOf col Is DataGridViewButtonColumn OrElse TypeOf col Is DataGridViewCheckBoxColumn Then
                    txt.Enabled = False
                    txt.ReadOnly = True
                Else
                    AddHandler txt.TextChanged, textHandler
                End If

                panel.Controls.Add(txt)
                dict.Add(col.Name, txt)
            Next
        End Sub

        Private Sub DgvInvoices_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxes()
        End Sub

        Private Sub DgvInvoicesHavaleh_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxesHavaleh()
        End Sub

        Private Sub DgvInvoicesBargasht_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxesBargasht()
        End Sub

        Private Sub DgvInvoicesHavalehBargasht_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxesHavalehBargasht()
        End Sub

        Private Sub AlignAllSearchBoxes()
            AlignSearchBoxes()
            AlignSearchBoxesHavaleh()
            AlignSearchBoxesBargasht()
            AlignSearchBoxesHavalehBargasht()
        End Sub

        Private Sub AlignSearchBoxes()
            AlignSearchBoxesForGrid(dgvInvoices, pnlFilters, filterTextBoxes)
        End Sub

        Private Sub AlignSearchBoxesHavaleh()
            AlignSearchBoxesForGrid(dgvInvoicesHavaleh, pnlFiltersHavaleh, filterTextBoxesHavaleh)
        End Sub

        Private Sub AlignSearchBoxesBargasht()
            AlignSearchBoxesForGrid(dgvInvoicesBargasht, pnlFiltersBargasht, filterTextBoxesBargasht)
        End Sub

        Private Sub AlignSearchBoxesHavalehBargasht()
            AlignSearchBoxesForGrid(dgvInvoicesHavalehBargasht, pnlFiltersHavalehBargasht, filterTextBoxesHavalehBargasht)
        End Sub

        Private Sub AlignSearchBoxesForGrid(grid As DataGridView, panel As Panel, dict As Dictionary(Of String, TextBox))
            If grid Is Nothing OrElse grid.Columns.Count = 0 OrElse panel Is Nothing Then Return

            panel.SuspendLayout()
            For Each kvp In dict
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = grid.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = grid.GetColumnDisplayRectangle(col.Index, True)
                    If rect.IsEmpty OrElse rect.Width = 0 Then
                        txt.Visible = False
                    Else
                        Dim screenPt = grid.PointToScreen(New Point(rect.X, 0))
                        Dim panelPt = panel.PointToClient(screenPt)
                        txt.Location = New Point(panelPt.X, 4)
                        txt.Width = rect.Width
                        txt.Visible = True
                    End If
                Else
                    txt.Visible = False
                End If
            Next
            panel.ResumeLayout()
        End Sub

        Private Sub FilterTextBox_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvInvoices, filterTextBoxes)
        End Sub

        Private Sub FilterTextBoxHavaleh_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvInvoicesHavaleh, filterTextBoxesHavaleh)
        End Sub

        Private Sub FilterTextBoxBargasht_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvInvoicesBargasht, filterTextBoxesBargasht)
        End Sub

        Private Sub FilterTextBoxHavalehBargasht_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvInvoicesHavalehBargasht, filterTextBoxesHavalehBargasht)
        End Sub

        Private Sub ApplyFilters(grid As DataGridView, dict As Dictionary(Of String, TextBox))
            If _invoicesTable Is Nothing Then Return

            Dim filters As New List(Of String)()

            For Each kvp In dict
                Dim txt = kvp.Value
                Dim propertyName = Convert.ToString(txt.Tag)
                If String.IsNullOrEmpty(propertyName) OrElse Not txt.Enabled Then Continue For

                Dim val = txt.Text.Trim().Replace("'", "''")
                If Not String.IsNullOrEmpty(val) Then
                    filters.Add(String.Format("Convert({0}, 'System.String') LIKE '%{1}%'", propertyName, val))
                End If
            Next

            Dim dv As DataView = TryCast(grid.DataSource, DataView)
            If dv IsNot Nothing Then
                If filters.Count > 0 Then
                    dv.RowFilter = String.Join(" AND ", filters)
                Else
                    dv.RowFilter = ""
                End If
            ElseIf _invoicesTable IsNot Nothing Then
                If filters.Count > 0 Then
                    _invoicesTable.DefaultView.RowFilter = String.Join(" AND ", filters)
                Else
                    _invoicesTable.DefaultView.RowFilter = ""
                End If
            End If
        End Sub

        Private Sub LoadData()
            Try
                _invoicesTable = _invoiceService.GetSalesInvoices()

                If _invoicesTable IsNot Nothing Then
                    If Not _invoicesTable.Columns.Contains("PersianDate") Then
                        _invoicesTable.Columns.Add("PersianDate", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("InvoiceType") Then
                        _invoicesTable.Columns.Add("InvoiceType", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("ReceiptStatus") Then
                        _invoicesTable.Columns.Add("ReceiptStatus", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("PaymentType") Then
                        _invoicesTable.Columns.Add("PaymentType", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("WarehouseName") Then
                        _invoicesTable.Columns.Add("WarehouseName", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("CustomerName") Then
                        _invoicesTable.Columns.Add("CustomerName", GetType(String))
                    End If
                    If Not _invoicesTable.Columns.Contains("Description") Then
                        _invoicesTable.Columns.Add("Description", GetType(String))
                    End If

                    Dim paySvc As New PaymentService()
                    For Each row As DataRow In _invoicesTable.Rows
                        If Not row.IsNull("InvoiceDate") Then
                            row("PersianDate") = ToPersian(Convert.ToDateTime(row("InvoiceDate")))
                        End If
                        If row.IsNull("InvoiceType") OrElse String.IsNullOrEmpty(Convert.ToString(row("InvoiceType"))) Then
                            row("InvoiceType") = "فاکتور فروش"
                        End If
                        If row.IsNull("ReceiptStatus") OrElse String.IsNullOrEmpty(Convert.ToString(row("ReceiptStatus"))) Then
                            row("ReceiptStatus") = "حواله نشده"
                        End If

                        Try
                            Dim invId = Convert.ToInt32(row("InvoiceID"))
                            Dim tot = Convert.ToDecimal(If(row.IsNull("TotalAmount"), 0D, row("TotalAmount")))
                            Dim statusInfo = paySvc.GetSettlementStatus(invId, tot)
                            row("PaymentType") = statusInfo.StatusText
                        Catch
                            row("PaymentType") = ""
                        End Try
                    Next
                End If

                Dim dvForoosh As New DataView(_invoicesTable)
                dvForoosh.RowFilter = "InvoiceType = 'فاکتور فروش'"

                Dim dvHavaleh As New DataView(_invoicesTable)
                dvHavaleh.RowFilter = "InvoiceType = 'فاکتور فروش' OR InvoiceType = 'حواله خروج از انبار'"

                Dim dvBargasht As New DataView(_invoicesTable)
                dvBargasht.RowFilter = "InvoiceType = 'فاکتور فروش'"

                Dim dvHavalehBargasht As New DataView(_invoicesTable)
                dvHavalehBargasht.RowFilter = "InvoiceType = 'برگشت از فروش'"

                dgvInvoices.DataSource = dvForoosh
                dgvInvoicesHavaleh.DataSource = dvHavaleh
                dgvInvoicesBargasht.DataSource = dvBargasht
                dgvInvoicesHavalehBargasht.DataSource = dvHavalehBargasht

                ApplyFilters(dgvInvoices, filterTextBoxes)
                ApplyFilters(dgvInvoicesHavaleh, filterTextBoxesHavaleh)
                ApplyFilters(dgvInvoicesBargasht, filterTextBoxesBargasht)
                ApplyFilters(dgvInvoicesHavalehBargasht, filterTextBoxesHavalehBargasht)
                AlignAllSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست اسناد فروش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryForoosh2Form("فاکتور فروش")
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnNewHavaleh_Click(sender As Object, e As EventArgs) Handles btnNewHavalehHavaleh.Click
            Using frm As New AnbardaryForoosh2Form("حواله خروج از انبار")
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DgvInvoices_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoices.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit(dgvInvoices)
            End If
        End Sub

        Private Sub DgvInvoicesHavaleh_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesHavaleh.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit(dgvInvoicesHavaleh)
            End If
        End Sub

        Private Sub DgvInvoicesBargasht_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesBargasht.CellDoubleClick
            If e.RowIndex >= 0 Then
                Dim invoiceId = Convert.ToInt32(dgvInvoicesBargasht.Rows(e.RowIndex).Cells("InvoiceID").Value)
                Using frm As New AnbardaryBargashtHistoryForm(invoiceId)
                    frm.ShowDialog()
                    LoadData()
                End Using
            End If
        End Sub

        Private Sub DgvInvoicesHavalehBargasht_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesHavalehBargasht.CellDoubleClick
            If e.RowIndex >= 0 Then
                Dim invoiceId = Convert.ToInt32(dgvInvoicesHavalehBargasht.Rows(e.RowIndex).Cells("InvoiceID").Value)
                Using frm As New AnbardaryReceiptsHistoryForm(invoiceId)
                    frm.ShowDialog()
                End Using
            End If
        End Sub

        Private Sub DgvInvoices_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoices.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoices.Columns(e.ColumnIndex).Name
                If colName = ColNameEdit Then
                    OpenSelectedForEdit(dgvInvoices)
                ElseIf colName = ColNameDelete Then
                    DeleteSelected(dgvInvoices)
                End If
            End If
        End Sub

        Private Sub DgvInvoicesBargasht_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesBargasht.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoicesBargasht.Columns(e.ColumnIndex).Name
                If colName = "colBtnViewBargasht" Then
                    Dim invoiceId = Convert.ToInt32(dgvInvoicesBargasht.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Using frm As New AnbardaryBargashtHistoryForm(invoiceId)
                        frm.ShowDialog()
                        LoadData()
                    End Using
                End If
            End If
        End Sub

        Private Sub DgvInvoicesHavaleh_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesHavaleh.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoicesHavaleh.Columns(e.ColumnIndex).Name
                If colName = "colBtnViewReceipts" Then
                    Dim invoiceId = Convert.ToInt32(dgvInvoicesHavaleh.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Using frm As New AnbardaryReceiptsHistoryForm(invoiceId)
                        frm.ShowDialog()
                    End Using
                ElseIf colName = "colBtnCreateInvoice" Then
                    Dim receiptId = Convert.ToInt32(dgvInvoicesHavaleh.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Dim docType = Convert.ToString(dgvInvoicesHavaleh.Rows(e.RowIndex).Cells("InvoiceType").Value)
                    If docType = "حواله خروج از انبار" Then
                        Using frm As New AnbardaryForoosh2Form(receiptId, "فاکتور فروش", True)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadData()
                            End If
                        End Using
                    Else
                        MessageBox.Show("این سند یک فاکتور فروش مرجع است. جهت مشاهده حوالههای انبار مربوطه دکمه «مشاهده حوالهها» را کلیک کنید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If
        End Sub

        Private Sub DgvInvoicesHavalehBargasht_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesHavalehBargasht.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoicesHavalehBargasht.Columns(e.ColumnIndex).Name
                If colName = "colBtnViewReceipts" Then
                    Dim invoiceId = Convert.ToInt32(dgvInvoicesHavalehBargasht.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Using frm As New AnbardaryReceiptsHistoryForm(invoiceId)
                        frm.ShowDialog()
                    End Using
                End If
            End If
        End Sub

        Private Sub OpenSelectedForEdit(grid As DataGridView)
            If grid.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک سند فروش را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(grid.CurrentRow.Cells("InvoiceID").Value)
            Dim docType = If(grid Is dgvInvoicesHavaleh, "حواله خروج از انبار", Convert.ToString(grid.CurrentRow.Cells("InvoiceType").Value))
            Using frm As New AnbardaryForoosh2Form(invoiceId, docType)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DeleteSelected(grid As DataGridView)
            If grid.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک سند فروش را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(grid.CurrentRow.Cells("InvoiceID").Value)
            Dim invoiceNum = Convert.ToString(grid.CurrentRow.Cells("InvoiceNumber").Value)
            Dim docType = Convert.ToString(grid.CurrentRow.Cells("InvoiceType").Value)

            Dim confirm = MessageBox.Show("آیا از حذف " & docType & " شماره «" & invoiceNum & "» و بازگرداندن موجودی انبار اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _invoiceService.DeleteSalesInvoice(invoiceId)
                    MessageBox.Show("سند با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click, btnRefreshHavaleh.Click, btnRefreshBargasht.Click, btnRefreshHavalehBargasht.Click
            For Each txt In filterTextBoxes.Values
                txt.Clear()
            Next
            For Each txt In filterTextBoxesHavaleh.Values
                txt.Clear()
            Next
            For Each txt In filterTextBoxesBargasht.Values
                txt.Clear()
            Next
            For Each txt In filterTextBoxesHavalehBargasht.Values
                txt.Clear()
            Next
            LoadData()
        End Sub
    End Class
End Namespace
