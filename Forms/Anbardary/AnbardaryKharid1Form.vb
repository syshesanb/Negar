Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Business.PersianDateHelper

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryKharid1Form
        Inherits Form

        Private ReadOnly _invoiceService As New InvoiceService()
        Private _invoicesTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()
        Private filterTextBoxesResid As New Dictionary(Of String, TextBox)()

        Private Const ColNameReceipt As String = "colReceipt"
        Private Const ColNameEdit As String = "colEdit"
        Private Const ColNameDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryKharid1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            ' Style DataGridViews
            If Me.dgvInvoices IsNot Nothing Then ApplyGridStyling(Me.dgvInvoices)
            If Me.dgvInvoicesResid IsNot Nothing Then ApplyGridStyling(Me.dgvInvoicesResid)

            ConfigureGridKharid(dgvInvoices)
            ConfigureGridResid(dgvInvoicesResid)

            LoadData()

            CreateFilterTextBoxes(dgvInvoices, pnlFilters, filterTextBoxes, AddressOf FilterTextBox_TextChanged)
            CreateFilterTextBoxes(dgvInvoicesResid, pnlFiltersResid, filterTextBoxesResid, AddressOf FilterTextBoxResid_TextChanged)

            AddHandler dgvInvoices.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvInvoices.Scroll, AddressOf DgvInvoices_Scroll
            AddHandler dgvInvoicesResid.ColumnWidthChanged, AddressOf AlignSearchBoxesResid
            AddHandler dgvInvoicesResid.Scroll, AddressOf DgvInvoicesResid_Scroll
            AddHandler Me.Resize, AddressOf AlignAllSearchBoxes

            AlignAllSearchBoxes()
        End Sub

        Private Sub ApplyGridStyling(grid As DataGridView)
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single
            grid.GridColor = Color.FromArgb(200, 210, 225)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
            grid.DefaultCellStyle.SelectionForeColor = Color.White
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
        End Sub

        Private Sub ConfigureGridKharid(grid As DataGridView)
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

            Dim colVendor As New DataGridViewTextBoxColumn()
            colVendor.Name = "VendorName"
            colVendor.DataPropertyName = "VendorName"
            colVendor.HeaderText = "فروشنده / تامین‌کننده"
            colVendor.Width = 180

            Dim colWarehouse As New DataGridViewTextBoxColumn()
            colWarehouse.Name = "WarehouseName"
            colWarehouse.DataPropertyName = "WarehouseName"
            colWarehouse.HeaderText = "انبار مقصد"
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
                colVendor, colWarehouse, colTotal, colPayment, colDesc
            })
        End Sub

        Private Sub ConfigureGridResid(grid As DataGridView)
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

            Dim colVendor As New DataGridViewTextBoxColumn()
            colVendor.Name = "VendorName"
            colVendor.DataPropertyName = "VendorName"
            colVendor.HeaderText = "فروشنده / تامین‌کننده"
            colVendor.Width = 180

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
                colVendor, colWarehouse, colReceiptStatus, colDesc
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

        Private Sub DgvInvoices_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvInvoices.CellFormatting
            If e.RowIndex < 0 Then Return
            If dgvInvoices.Columns(e.ColumnIndex).Name = "PaymentType" Then
                Dim val = Convert.ToString(e.Value)
                Select Case val
                    Case "تسویه نشده"
                        e.CellStyle.ForeColor = Color.Red
                        e.CellStyle.BackColor = Color.FromArgb(255, 235, 235)
                        e.CellStyle.Font = New Font(dgvInvoices.Font, FontStyle.Bold)
                    Case "تسویه ناقص"
                        e.CellStyle.ForeColor = Color.DarkOrange
                        e.CellStyle.BackColor = Color.FromArgb(255, 250, 220)
                        e.CellStyle.Font = New Font(dgvInvoices.Font, FontStyle.Bold)
                    Case "تسویه کامل"
                        e.CellStyle.ForeColor = Color.DarkGreen
                        e.CellStyle.BackColor = Color.FromArgb(235, 255, 235)
                        e.CellStyle.Font = New Font(dgvInvoices.Font, FontStyle.Bold)
                End Select
            End If
        End Sub

        Private Sub DgvInvoicesResid_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxesResid()
        End Sub

        Private Sub AlignAllSearchBoxes()
            AlignSearchBoxes()
            AlignSearchBoxesResid()
        End Sub

        Private Sub AlignSearchBoxes()
            AlignSearchBoxesForGrid(dgvInvoices, pnlFilters, filterTextBoxes)
        End Sub

        Private Sub AlignSearchBoxesResid()
            AlignSearchBoxesForGrid(dgvInvoicesResid, pnlFiltersResid, filterTextBoxesResid)
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

        Private Sub FilterTextBoxResid_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvInvoicesResid, filterTextBoxesResid)
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
                _invoicesTable = _invoiceService.GetPurchaseInvoices()

                If _invoicesTable IsNot Nothing Then
                    If Not _invoicesTable.Columns.Contains("PersianDate") Then
                        _invoicesTable.Columns.Add("PersianDate", GetType(String))
                    End If

                    Dim paySvc As New PaymentService()
                    For Each row As DataRow In _invoicesTable.Rows
                        If Not row.IsNull("InvoiceDate") Then
                            row("PersianDate") = ToPersian(Convert.ToDateTime(row("InvoiceDate")))
                        End If
                        If row.IsNull("ReceiptStatus") OrElse String.IsNullOrEmpty(Convert.ToString(row("ReceiptStatus"))) Then
                            row("ReceiptStatus") = "رسید نشده"
                        End If

                        Dim invId = Convert.ToInt32(row("InvoiceID"))
                        Dim tot = Convert.ToDecimal(If(row.IsNull("TotalAmount"), 0D, row("TotalAmount")))
                        Dim statusInfo = paySvc.GetSettlementStatus(invId, tot)
                        row("PaymentType") = statusInfo.StatusText
                    Next
                End If

                Dim dvKharid As New DataView(_invoicesTable)
                Dim dvResid As New DataView(_invoicesTable)

                dgvInvoices.DataSource = dvKharid
                dgvInvoicesResid.DataSource = dvResid

                ApplyFilters(dgvInvoices, filterTextBoxes)
                ApplyFilters(dgvInvoicesResid, filterTextBoxesResid)
                AlignAllSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست اسناد خرید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, btnNewResid.Click
            Using frm As New AnbardaryKharid2Form("فاکتور خرید")
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnNewReceipt_Click(sender As Object, e As EventArgs) Handles btnNewReceipt.Click, btnNewReceiptResid.Click
            Using frm As New AnbardaryKharid2Form("رسید ورود به انبار")
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

        Private Sub DgvInvoicesResid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesResid.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit(dgvInvoicesResid)
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

        Private Sub DgvInvoicesResid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoicesResid.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoicesResid.Columns(e.ColumnIndex).Name
                If colName = "colBtnViewReceipts" Then
                    Dim invoiceId = Convert.ToInt32(dgvInvoicesResid.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Using frm As New AnbardaryReceiptsHistoryForm(invoiceId)
                        frm.ShowDialog()
                    End Using
                End If
            End If
        End Sub

        Private Sub OpenSelectedForEdit(grid As DataGridView)
            If grid.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک سند خرید را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(grid.CurrentRow.Cells("InvoiceID").Value)
            Dim docType = If(grid Is dgvInvoicesResid, "رسید ورود به انبار", "فاکتور خرید")
            Using frm As New AnbardaryKharid2Form(invoiceId, docType)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DeleteSelected(grid As DataGridView)
            If grid.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک سند خرید را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim invoiceId = Convert.ToInt32(grid.CurrentRow.Cells("InvoiceID").Value)
            Dim invoiceNum = Convert.ToString(grid.CurrentRow.Cells("InvoiceNumber").Value)
            Dim docType = Convert.ToString(grid.CurrentRow.Cells("InvoiceType").Value)

            Dim confirm = MessageBox.Show("آیا از حذف " & docType & " شماره «" & invoiceNum & "» و بازگرداندن موجودی انبار اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _invoiceService.DeletePurchaseInvoice(invoiceId)
                    MessageBox.Show("سند با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click, btnRefreshResid.Click
            For Each txt In filterTextBoxes.Values
                txt.Clear()
            Next
            For Each txt In filterTextBoxesResid.Values
                txt.Clear()
            Next
            LoadData()
        End Sub
    End Class
End Namespace
