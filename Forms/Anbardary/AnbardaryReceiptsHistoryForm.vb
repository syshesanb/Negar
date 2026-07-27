Imports System
Imports System.Windows.Forms
Imports System.Drawing

Namespace Negar.Forms
    Public Class AnbardaryReceiptsHistoryForm
        Inherits Form

        Private _invoiceId As Integer
        Private _invoiceService As New Negar.Business.InvoiceService()
        Private dgvReceipts As DataGridView

        Public Sub New(invoiceId As Integer)
            _invoiceId = invoiceId
            InitializeComponent()
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Size = New Size(950, 500)
            Me.Text = "تاریخچه رسیدهای انبار"
            Me.Font = New Font("Tahoma", 9)
            Me.BackColor = Color.White
        End Sub

        Private Sub InitializeComponent()
            Dim pnlTop As New Panel()
            pnlTop.Dock = DockStyle.Top
            pnlTop.Height = 50
            pnlTop.BackColor = Color.FromArgb(240, 240, 240)
            
            Dim lblTitle As New Label()
            lblTitle.Text = "لیست رسیدهای انبار ثبت شده برای این فاکتور"
            lblTitle.Font = New Font("Tahoma", 10, FontStyle.Bold)
            lblTitle.AutoSize = True
            lblTitle.Location = New Point(10, 15)
            pnlTop.Controls.Add(lblTitle)
            
            Dim flpButtons As New FlowLayoutPanel()
            flpButtons.Dock = DockStyle.Top
            flpButtons.Height = 45
            flpButtons.BackColor = Color.FromArgb(235, 240, 245)
            flpButtons.Padding = New Padding(5)
            
            Dim btnNewReceipt As New Button()
            btnNewReceipt.Text = "رسید انبار جدید"
            btnNewReceipt.Size = New Size(130, 32)
            btnNewReceipt.BackColor = Color.FromArgb(0, 120, 215)
            btnNewReceipt.ForeColor = Color.White
            btnNewReceipt.FlatStyle = FlatStyle.Flat
            AddHandler btnNewReceipt.Click, Sub(s, e)
                                                Using frm As New AnbardaryKharid2Form(_invoiceId, "رسید ورود به انبار")
                                                    If frm.ShowDialog() = DialogResult.OK Then
                                                        LoadData()
                                                    End If
                                                End Using
                                            End Sub
            flpButtons.Controls.Add(btnNewReceipt)

            dgvReceipts = New DataGridView()
            dgvReceipts.Dock = DockStyle.Fill
            dgvReceipts.AllowUserToAddRows = False
            dgvReceipts.AllowUserToDeleteRows = False
            dgvReceipts.ReadOnly = True
            dgvReceipts.AutoGenerateColumns = False
            dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReceipts.BackgroundColor = Color.White
            dgvReceipts.RowHeadersVisible = False
            dgvReceipts.BorderStyle = BorderStyle.None
            dgvReceipts.EnableHeadersVisualStyles = False
            dgvReceipts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 230, 240)
            dgvReceipts.ColumnHeadersHeight = 35
            dgvReceipts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)

            Dim colReceiptId As New DataGridViewTextBoxColumn()
            colReceiptId.Name = "ReceiptID"
            colReceiptId.DataPropertyName = "ReceiptID"
            colReceiptId.Visible = False
            
            Dim colNumber As New DataGridViewTextBoxColumn()
            colNumber.Name = "ReceiptNumber"
            colNumber.DataPropertyName = "ReceiptNumber"
            colNumber.HeaderText = "شماره رسید"
            colNumber.Width = 150
            
            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "ReceiptDate"
            colDate.DataPropertyName = "ReceiptDate"
            colDate.HeaderText = "تاریخ رسید"
            colDate.Width = 100
            
            Dim colUser As New DataGridViewTextBoxColumn()
            colUser.Name = "CreatedBy"
            colUser.DataPropertyName = "CreatedBy"
            colUser.HeaderText = "کاربر ثبت کننده"
            colUser.Width = 120
            
            Dim colQty As New DataGridViewTextBoxColumn()
            colQty.Name = "TotalQuantity"
            colQty.DataPropertyName = "TotalQuantity"
            colQty.HeaderText = "مجموع تعداد"
            colQty.Width = 100
            colQty.Visible = False
            
            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "colEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 60
            colEdit.FlatStyle = FlatStyle.Standard

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "colDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.Width = 60
            colDelete.FlatStyle = FlatStyle.Standard

            Dim colPrint As New DataGridViewButtonColumn()
            colPrint.Name = "colPrint"
            colPrint.HeaderText = "چاپ"
            colPrint.Text = "چاپ"
            colPrint.UseColumnTextForButtonValue = True
            colPrint.Width = 50
            colPrint.FlatStyle = FlatStyle.Standard

            dgvReceipts.Columns.AddRange(New DataGridViewColumn() {colEdit, colDelete, colPrint, colReceiptId, colNumber, colDate, colUser, colQty, colDesc})

            AddHandler dgvReceipts.CellContentClick, AddressOf dgvReceipts_CellContentClick

            AddHandler dgvReceipts.CellFormatting, AddressOf dgvReceipts_CellFormatting

            Dim pnlBottom As New Panel()
            pnlBottom.Dock = DockStyle.Bottom
            pnlBottom.Height = 50
            pnlBottom.BackColor = Color.FromArgb(240, 240, 240)
            
            Dim btnClose As New Button()
            btnClose.Text = "بستن"
            btnClose.Size = New Size(90, 32)
            btnClose.Location = New Point(10, 9)
            btnClose.BackColor = Color.White
            btnClose.FlatStyle = FlatStyle.Flat
            AddHandler btnClose.Click, Sub(s, e) Me.Close()
            pnlBottom.Controls.Add(btnClose)

            Me.Controls.Add(dgvReceipts)
            Me.Controls.Add(flpButtons)
            Me.Controls.Add(pnlTop)
            Me.Controls.Add(pnlBottom)
        End Sub

        Private Sub AnbardaryReceiptsHistoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadData()
        End Sub

        Private Sub LoadData()
            Try
                Dim dt = _invoiceService.GetWarehouseReceiptsForInvoice(_invoiceId)
                dgvReceipts.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
        
        Private Sub dgvReceipts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
            If dgvReceipts.Columns(e.ColumnIndex).Name = "ReceiptDate" AndAlso e.Value IsNot Nothing AndAlso TypeOf e.Value Is DateTime Then
                e.Value = Negar.Business.PersianDateHelper.ToPersian(DirectCast(e.Value, DateTime))
                e.FormattingApplied = True
            End If
        End Sub
        
        Private Sub dgvReceipts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                Dim colName = dgvReceipts.Columns(e.ColumnIndex).Name
                Dim receiptId = Convert.ToInt32(dgvReceipts.Rows(e.RowIndex).Cells("ReceiptID").Value)
                
                If colName = "colEdit" Then
                    Using frm As New AnbardaryKharid2Form(_invoiceId, receiptId)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadData()
                        End If
                    End Using
                ElseIf colName = "colDelete" Then
                    If MessageBox.Show("آیا از حذف این رسید انبار اطمینان دارید؟ در صورت حذف، مقادیر رسید شده در فاکتور مرجع مجدداً به عنوان دریافت نشده در نظر گرفته می‌شوند.", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            _invoiceService.DeleteWarehouseReceipt(receiptId)
                            MessageBox.Show("رسید انبار با موفقیت حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف رسید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                ElseIf colName = "colPrint" Then
                    PrintReceiptRow(e.RowIndex)
                End If
            End If
        End Sub
        Private Sub PrintReceiptRow(rowIndex As Integer)
            Try
                Dim row = dgvReceipts.Rows(rowIndex)
                Dim receiptNumber = Convert.ToString(row.Cells("ReceiptNumber").Value)
                Dim receiptDate = Convert.ToString(row.Cells("ReceiptDate").Value)
                Dim createdBy = Convert.ToString(row.Cells("CreatedBy").Value)
                Dim description = Convert.ToString(row.Cells("Description").Value)
                Dim receiptId = Convert.ToInt32(row.Cells("ReceiptID").Value)

                Dim receiptDetails = _invoiceService.GetWarehouseReceiptDetailsList(receiptId)

                Dim pd As New System.Drawing.Printing.PrintDocument()
                pd.DefaultPageSettings.Landscape = True
                pd.DefaultPageSettings.PaperSize = New System.Drawing.Printing.PaperSize("آجکیӌ 4", 827, 1169)

                AddHandler pd.PrintPage, Sub(sender As Object, ev As System.Drawing.Printing.PrintPageEventArgs)
                    Dim g = ev.Graphics
                    Dim rtlFmt As New StringFormat()
                    rtlFmt.FormatFlags = StringFormatFlags.DirectionRightToLeft
                    rtlFmt.LineAlignment = StringAlignment.Center

                    Dim titleFont As New Font("تاهوما", 14, FontStyle.Bold)
                    Dim headerFont As New Font("تاهوما", 9, FontStyle.Bold)
                    Dim bodyFont As New Font("تاهوما", 9)
                    Dim smallFont As New Font("تاهوما", 8)

                    Dim leftMargin = ev.MarginBounds.Left
                    Dim rightMargin = ev.MarginBounds.Right
                    Dim pageWidth = ev.MarginBounds.Width
                    Dim y As Integer = ev.MarginBounds.Top

                    ' عنوان رسید
                    Dim titleRect As New Rectangle(leftMargin, y, pageWidth, 35)
                    g.DrawString("رسید انبار", titleFont, Brushes.Black, titleRect, rtlFmt)
                    y += 40

                    ' اطلاعات سر صفحه
                    g.DrawString("شماره رسید: " & receiptNumber, bodyFont, Brushes.Black, New Rectangle(leftMargin, y, pageWidth \ 2, 22), rtlFmt)
                    g.DrawString("تاریخ: " & receiptDate, bodyFont, Brushes.Black, New Rectangle(leftMargin + pageWidth \ 2, y, pageWidth \ 2, 22), rtlFmt)
                    y += 25
                    g.DrawString("ثبت کننده: " & createdBy, bodyFont, Brushes.Black, New Rectangle(leftMargin, y, pageWidth \ 2, 22), rtlFmt)
                    If Not String.IsNullOrEmpty(description) Then
                        g.DrawString("توضیحات: " & description, bodyFont, Brushes.Black, New Rectangle(leftMargin + pageWidth \ 2, y, pageWidth \ 2, 22), rtlFmt)
                    End If
                    y += 30

                    ' خط جداکننده
                    g.DrawLine(Pens.Black, leftMargin, y, rightMargin, y)
                    y += 8

                    ' هدر جدول
                    Dim colWidths() As Integer = {50, 280, 90, 80, 80, 120}
                    Dim colHeaders() As String = {"ردیف", "نام کالا", "کد کالا", "واحد", "تعداد", "انبار"}

                    Dim x As Integer = rightMargin
                    For ci = 0 To colWidths.Length - 1
                        x -= colWidths(ci)
                        Dim cellRect As New Rectangle(x, y, colWidths(ci), 22)
                        g.FillRectangle(Brushes.LightSteelBlue, cellRect)
                        g.DrawRectangle(Pens.Gray, cellRect)
                        Dim cFmt As New StringFormat()
                        cFmt.FormatFlags = StringFormatFlags.DirectionRightToLeft
                        cFmt.LineAlignment = StringAlignment.Center
                        cFmt.Alignment = StringAlignment.Center
                        g.DrawString(colHeaders(ci), headerFont, Brushes.Black, cellRect, cFmt)
                    Next
                    y += 22

                    ' سطرهای دیتا
                    Dim lineNum = 1
                    For Each dr As System.Data.DataRow In receiptDetails.Rows
                        x = rightMargin
                        Dim cellData() As String = {
                            lineNum.ToString(),
                            Convert.ToString(If(dr.IsNull("ProductName"), "", dr("ProductName"))),
                            Convert.ToString(If(dr.IsNull("ProductCode"), "", dr("ProductCode"))),
                            Convert.ToString(If(dr.IsNull("Unit"), "", dr("Unit"))),
                            Convert.ToString(If(dr.IsNull("Quantity"), "", dr("Quantity"))),
                            Convert.ToString(If(dr.IsNull("WarehouseName"), "", dr("WarehouseName")))
                        }
                        Dim rowBrush = If(lineNum Mod 2 = 0, Brushes.AliceBlue, Brushes.White)
                        For ci = 0 To colWidths.Length - 1
                            x -= colWidths(ci)
                            Dim cellRect As New Rectangle(x, y, colWidths(ci), 22)
                            g.FillRectangle(rowBrush, cellRect)
                            g.DrawRectangle(Pens.LightGray, cellRect)
                            Dim dFmt As New StringFormat()
                            dFmt.FormatFlags = StringFormatFlags.DirectionRightToLeft
                            dFmt.LineAlignment = StringAlignment.Center
                            dFmt.Alignment = StringAlignment.Center
                            g.DrawString(cellData(ci), bodyFont, Brushes.Black, cellRect, dFmt)
                        Next
                        y += 22
                        lineNum += 1
                    Next

                    ' خط پایی
                    y += 10
                    g.DrawLine(Pens.Black, leftMargin, y, rightMargin, y)
                    y += 8
                    g.DrawString("امضای مسئول انبار: ____________________", smallFont, Brushes.Black, New Rectangle(leftMargin, y, pageWidth \ 2, 22), rtlFmt)
                    g.DrawString("تاریخ چاپ: " & Negar.Business.PersianDateHelper.ToPersian(DateTime.Today), smallFont, Brushes.Black, New Rectangle(leftMargin + pageWidth \ 2, y, pageWidth \ 2, 22), rtlFmt)
                End Sub

                Using dlg As New PrintPreviewDialog()
                    dlg.Document = pd
                    dlg.WindowState = FormWindowState.Maximized
                    dlg.ShowDialog(Me)
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در چاپ رسید: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class
End Namespace
