Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Linq
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniExpensesForm
        Inherits Form

        Private _expensesTable As DataTable
        Private _isLoading As Boolean = True

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniExpensesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            _isLoading = True
            ThemeHelper.ApplyFormTheme(Me)

            ' ۱. پیکربندی دیتاگرید (باید قبل از تنظیم SelectedIndex باشد)
            ConfigureGrid()

            ' ۲. بارگذاری دسته‌بندی‌ها
            cmbCategory.Items.Clear()
            cmbCategory.Items.Add("همه سرفصل‌ها")
            cmbCategory.Items.AddRange(New Object() {
                "هزینه‌های جاری",
                "هزینه‌های اداری و عمومی",
                "هزینه اجاره",
                "حقوق و دستمزد",
                "هزینه حمل و نقل",
                "پذیرایی و ملزومات",
                "استهلاک و تعمیرات",
                "سایر هزینه‌ها"
            })
            cmbCategory.SelectedIndex = 0

            ' ۳. بارگذاری اطلاعات
            _isLoading = False
            LoadExpenses()
            ApplySecurity()
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim isSuperAdmin = String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            btnAdd.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniExpensesSave)
            btnEdit.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniExpensesEdit)
            btnDelete.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniExpensesDelete)
            btnExpenseLedger.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniExpenseLedger)
            btnPrint.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniProfitLoss)
        End Sub

        Private Sub ConfigureGrid()
            dgvExpenses.AutoGenerateColumns = False
            dgvExpenses.Columns.Clear()

            dgvExpenses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvExpenses.ColumnHeadersHeight = 40
            dgvExpenses.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            Dim colNo As New DataGridViewTextBoxColumn()
            colNo.Name = "colNo"
            colNo.HeaderText = "ردیف"
            colNo.Width = 55
            colNo.ReadOnly = True
            colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "colExpenseID"
            colId.DataPropertyName = "ExpenseID"
            colId.HeaderText = "آیدی"
            colId.Visible = False

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colExpenseDate"
            colDate.DataPropertyName = "ExpenseDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 100
            colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colTitle As New DataGridViewTextBoxColumn()
            colTitle.Name = "colExpenseTitle"
            colTitle.DataPropertyName = "ExpenseTitle"
            colTitle.HeaderText = "عنوان / شرح هزینه"
            colTitle.Width = 220
            colTitle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colCat As New DataGridViewTextBoxColumn()
            colCat.Name = "colCategory"
            colCat.DataPropertyName = "Category"
            colCat.HeaderText = "سرفصل / دسته‌بندی"
            colCat.Width = 150
            colCat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colAmt As New DataGridViewTextBoxColumn()
            colAmt.Name = "colAmount"
            colAmt.DataPropertyName = "Amount"
            colAmt.HeaderText = "مبلغ (ریال)"
            colAmt.Width = 135
            colAmt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colAmt.DefaultCellStyle.Format = "N0"
            colAmt.DefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)

            Dim colPaidTo As New DataGridViewTextBoxColumn()
            colPaidTo.Name = "colPaidTo"
            colPaidTo.DataPropertyName = "PaidTo"
            colPaidTo.HeaderText = "پرداخت شده به"
            colPaidTo.Width = 140
            colPaidTo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colMethod As New DataGridViewTextBoxColumn()
            colMethod.Name = "colPaymentMethod"
            colMethod.DataPropertyName = "PaymentMethod"
            colMethod.HeaderText = "نحوه پرداخت"
            colMethod.Width = 110
            colMethod.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colRef As New DataGridViewTextBoxColumn()
            colRef.Name = "colReferenceNo"
            colRef.DataPropertyName = "ReferenceNo"
            colRef.HeaderText = "شماره پیگیری"
            colRef.Width = 110
            colRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colSanadRef As New DataGridViewTextBoxColumn()
            colSanadRef.Name = "colSanadRef"
            colSanadRef.DataPropertyName = "SanadRef"
            colSanadRef.HeaderText = "سند حسابداری (سال مالی)"
            colSanadRef.Width = 160
            colSanadRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colSanadRef.DefaultCellStyle.ForeColor = Color.FromArgb(13, 71, 161)

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDescription"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dgvExpenses.Columns.AddRange(New DataGridViewColumn() {
                colNo, colId, colDate, colTitle, colCat, colAmt, colPaidTo, colMethod, colRef, colSanadRef, colDesc
            })
        End Sub

        Public Sub LoadExpenses()
            Try
                Dim sqlQuery As String = "SELECT * FROM Expenses WHERE 1=1"
                Dim params As New List(Of Object)()

                ' فیلتر متنی
                Dim fText = txtSearch.Text.Trim()
                If Not String.IsNullOrEmpty(fText) Then
                    sqlQuery &= " AND (ExpenseTitle LIKE ? OR PaidTo LIKE ? OR Category LIKE ? OR ReferenceNo LIKE ? OR Description LIKE ?)"
                    Dim p = "%" & fText & "%"
                    params.AddRange(New Object() {p, p, p, p, p})
                End If

                ' فیلتر سرفصل
                If cmbCategory.SelectedIndex > 0 Then
                    sqlQuery &= " AND Category = ?"
                    params.Add(cmbCategory.SelectedItem.ToString())
                End If

                ' فیلتر تاریخ از
                Dim fDate = txtFromDate.Text.Trim()
                If Not String.IsNullOrEmpty(fDate) Then
                    sqlQuery &= " AND ExpenseDate >= ?"
                    params.Add(fDate)
                End If

                ' فیلتر تاریخ تا
                Dim tDate = txtToDate.Text.Trim()
                If Not String.IsNullOrEmpty(tDate) Then
                    sqlQuery &= " AND ExpenseDate <= ?"
                    params.Add(tDate)
                End If

                sqlQuery &= " ORDER BY ExpenseDate DESC, ExpenseID DESC"

                _expensesTable = Sql.ExecuteTable(sqlQuery, params.ToArray())
                If _expensesTable IsNot Nothing Then
                    If Not _expensesTable.Columns.Contains("SanadRef") Then _expensesTable.Columns.Add("SanadRef", GetType(String))
                    For Each r As DataRow In _expensesTable.Rows
                        Dim expId = Convert.ToInt32(r("ExpenseID"))
                        r("SanadRef") = InvoiceService.GetSanadRefAndFiscalYearForExpense(expId)
                    Next
                End If

                dgvExpenses.DataSource = _expensesTable

                ' ردیف‌گذاری
                For i As Integer = 0 To dgvExpenses.Rows.Count - 1
                    dgvExpenses.Rows(i).Cells("colNo").Value = i + 1
                Next

                RecalculateTotals()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست هزینه‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub RecalculateTotals()
            If _expensesTable Is Nothing Then
                lblCount.Text = "تعداد اسناد هزینه: 0"
                lblGrandTotal.Text = "جمع کل هزینه‌ها: ۰ ریال"
                Return
            End If

            Dim totalCount = _expensesTable.Rows.Count
            lblCount.Text = String.Format("تعداد اسناد هزینه: {0}", totalCount)

            Dim grandTotal As Decimal = 0D
            For Each row As DataRow In _expensesTable.Rows
                If Not row.IsNull("Amount") Then
                    Dim val As Decimal = 0D
                    Decimal.TryParse(Convert.ToString(row("Amount")), val)
                    grandTotal += val
                End If
            Next

            lblGrandTotal.Text = String.Format("جمع کل هزینه‌ها: {0} ریال", grandTotal.ToString("N0"))
        End Sub

        Private isFormattingFromDate As Boolean = False
        Private Sub txtFromDate_TextChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged
            If isFormattingFromDate Then Return
            Dim digits = System.Text.RegularExpressions.Regex.Replace(txtFromDate.Text, "[^\d]", "")
            If digits.Length = 8 Then
                isFormattingFromDate = True
                txtFromDate.Text = digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6, 2)
                txtFromDate.SelectionStart = txtFromDate.Text.Length
                isFormattingFromDate = False
            End If
        End Sub

        Private isFormattingToDate As Boolean = False
        Private Sub txtToDate_TextChanged(sender As Object, e As EventArgs) Handles txtToDate.TextChanged
            If isFormattingToDate Then Return
            Dim digits = System.Text.RegularExpressions.Regex.Replace(txtToDate.Text, "[^\d]", "")
            If digits.Length = 8 Then
                isFormattingToDate = True
                txtToDate.Text = digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6, 2)
                txtToDate.SelectionStart = txtToDate.Text.Length
                isFormattingToDate = False
            End If
        End Sub

        Private Sub btnPickFromDate_Click(sender As Object, e As EventArgs) Handles btnPickFromDate.Click
            Using cal As New PersianCalendarForm()
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtFromDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnPickToDate_Click(sender As Object, e As EventArgs) Handles btnPickToDate.Click
            Using cal As New PersianCalendarForm()
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtToDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
            If _isLoading Then Return
            LoadExpenses()
        End Sub

        Private Sub cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategory.SelectedIndexChanged
            If _isLoading Then Return
            LoadExpenses()
        End Sub

        Private Sub btnExpenseLedger_Click(sender As Object, e As EventArgs) Handles btnExpenseLedger.Click
            Using dlg As New AnbarMiniExpenseLedgerDialog(txtFromDate.Text, txtToDate.Text)
                dlg.ShowDialog(Me)
            End Using
        End Sub

        Private Sub btnClearFilter_Click(sender As Object, e As EventArgs) Handles btnClearFilter.Click
            txtSearch.Text = ""
            cmbCategory.SelectedIndex = 0
            txtFromDate.Text = ""
            txtToDate.Text = ""
            LoadExpenses()
        End Sub

        Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            Using dlg As New AnbarMiniExpenseEditDialog()
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    LoadExpenses()
                End If
            End Using
        End Sub

        Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            Dim selectedId = GetSelectedExpenseId()
            If Not selectedId.HasValue Then
                MessageBox.Show("لطفاً یک ردیف هزینه برای ویرایش انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using dlg As New AnbarMiniExpenseEditDialog(selectedId.Value)
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    LoadExpenses()
                End If
            End Using
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            Dim selectedId = GetSelectedExpenseId()
            If Not selectedId.HasValue Then
                MessageBox.Show("لطفاً یک ردیف هزینه برای حذف انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If MessageBox.Show("آیا از حذف سند هزینه انتخاب شده اطمینان دارید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    Sql.ExecuteNonQuery("DELETE FROM Expenses WHERE ExpenseID = ?", selectedId.Value)
                    InvoiceService.DeleteAutoVoucherForExpense(selectedId.Value)
                    LoadExpenses()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف هزینه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Function GetSelectedExpenseId() As Integer?
            If dgvExpenses.CurrentRow Is Nothing Then Return Nothing
            Dim val = dgvExpenses.CurrentRow.Cells("colExpenseID").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return Nothing
            Dim id As Integer = 0
            If Integer.TryParse(Convert.ToString(val), id) AndAlso id > 0 Then Return id
            Return Nothing
        End Function

        Private Sub dgvExpenses_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpenses.CellDoubleClick
            If e.RowIndex >= 0 Then
                btnEdit.PerformClick()
            End If
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
            If dgvExpenses.Rows.Count = 0 Then
                MessageBox.Show("هیچ داده‌ای برای چاپ وجود ندارد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim doc As New PrintDocument()
            doc.DefaultPageSettings.Landscape = True
            doc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            doc.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            Dim rowIndex As Integer = 0

            AddHandler doc.PrintPage, Sub(s, ev)
                Dim g = ev.Graphics
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                Dim leftX = ev.MarginBounds.Left
                Dim rightX = ev.MarginBounds.Right
                Dim topY = ev.MarginBounds.Top
                Dim bottomY = ev.MarginBounds.Bottom
                Dim pageWidth = rightX - leftX
                Dim pageHeight = bottomY - topY

                ' ۱. کادر دور صفحه
                Using pBorder As New Pen(Color.Black, 2.0!)
                    g.DrawRectangle(pBorder, leftX, topY, pageWidth, pageHeight)
                End Using

                ' ۲. سربرگ قرمز عنابی
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
                        g.DrawString("گزارش هزینه‌ها", fTitle, brMaroon, titleRect, sfCenter)
                    End Using
                End Using

                Using fBold As New Font("Tahoma", 9.0!, FontStyle.Bold)
                    Dim printDateStr = "تاریخ: " & PersianDateHelper.ToPersian(DateTime.Now)
                    g.DrawString(printDateStr, fBold, Brushes.Black, rightX - 15, topY + 22, sfRight)
                End Using

                ' ۳. ستون‌ها
                Dim visibleCols As New List(Of DataGridViewColumn)()
                For Each col As DataGridViewColumn In dgvExpenses.Columns
                    If col.Visible Then visibleCols.Add(col)
                Next

                Dim totalGridWidth As Integer = visibleCols.Sum(Function(c) Math.Max(c.Width, 40))
                Dim colWidths As New List(Of Integer)()
                For Each c In visibleCols
                    Dim w = CInt((Math.Max(c.Width, 40) / CSng(totalGridWidth)) * pageWidth)
                    colWidths.Add(w)
                Next
                Dim currentSum = colWidths.Sum()
                If currentSum < pageWidth Then colWidths(colWidths.Count - 1) += (pageWidth - currentSum)

                Dim colX = New Integer(visibleCols.Count) {}
                colX(0) = rightX
                For i As Integer = 0 To visibleCols.Count - 1
                    colX(i + 1) = colX(i) - colWidths(i)
                Next

                Dim tableStartY = topY + 80
                Dim headerHeight = 32
                Dim rowHeight = 24
                Dim footerHeight = 40
                Dim totalsHeight = 28
                Dim maxY = bottomY - footerHeight - totalsHeight - 10

                ' سرستون‌ها (آبی فیروزه‌ای ملایم)
                Dim rectHeaderFull = New Rectangle(leftX, tableStartY, pageWidth, headerHeight)
                Using brHeaderBg As New SolidBrush(Color.FromArgb(210, 236, 245))
                    g.FillRectangle(brHeaderBg, rectHeaderFull)
                End Using
                g.DrawRectangle(Pens.Black, rectHeaderFull)

                Using fTableHeader As New Font("Tahoma", 8.5!, FontStyle.Bold)
                    For i As Integer = 0 To visibleCols.Count - 1
                        Dim rectColHeader = New Rectangle(colX(i + 1), tableStartY, colWidths(i), headerHeight)
                        g.DrawRectangle(Pens.Black, rectColHeader)
                        g.DrawString(visibleCols(i).HeaderText, fTableHeader, Brushes.Black, rectColHeader, sfCenter)
                    Next
                End Using

                ' ۴. داده‌ها
                Dim currY = tableStartY + headerHeight
                Using fRow As New Font("Tahoma", 8.5!, FontStyle.Regular)
                    While rowIndex < dgvExpenses.Rows.Count AndAlso currY + rowHeight <= maxY
                        Dim row = dgvExpenses.Rows(rowIndex)
                        If Not row.IsNewRow Then
                            For i As Integer = 0 To visibleCols.Count - 1
                                Dim col = visibleCols(i)
                                Dim cellRect = New Rectangle(colX(i + 1), currY, colWidths(i), rowHeight)

                                Dim cellText As String = ""
                                Dim cellVal = row.Cells(col.Index).Value
                                If cellVal IsNot Nothing AndAlso Not Convert.IsDBNull(cellVal) Then
                                    If Not String.IsNullOrEmpty(col.DefaultCellStyle.Format) AndAlso Information.IsNumeric(cellVal) Then
                                        Dim dVal As Decimal = 0D
                                        Decimal.TryParse(Convert.ToString(cellVal), dVal)
                                        cellText = dVal.ToString(col.DefaultCellStyle.Format)
                                    Else
                                        cellText = Convert.ToString(cellVal)
                                    End If
                                End If

                                Dim sfCell As StringFormat = sfRight
                                If col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Then
                                    sfCell = sfCenter
                                End If

                                Dim textPaddingRect = New Rectangle(colX(i + 1) + 4, currY, colWidths(i) - 8, rowHeight)
                                g.DrawString(cellText, fRow, Brushes.Black, textPaddingRect, sfCell)
                                g.DrawRectangle(Pens.Black, cellRect)
                            Next

                            Using pDot As New Pen(Color.LightGray) With {.DashStyle = Drawing2D.DashStyle.Dot}
                                g.DrawLine(pDot, leftX, currY + rowHeight, rightX, currY + rowHeight)
                            End Using

                            currY += rowHeight
                        End If
                        rowIndex += 1
                    End While
                End Using

                g.DrawRectangle(Pens.Black, leftX, tableStartY, pageWidth, currY - tableStartY)

                ' ۵. سطر جمع (زرد لیمویی)
                Dim isLastPage = (rowIndex >= dgvExpenses.Rows.Count)
                If isLastPage Then
                    Dim rectTotals = New Rectangle(leftX, currY, pageWidth, 28)
                    Using brTotals As New SolidBrush(Color.FromArgb(254, 248, 165))
                        g.FillRectangle(brTotals, rectTotals)
                    End Using
                    g.DrawRectangle(Pens.Black, rectTotals)

                    Using fTotals As New Font("Tahoma", 9.0!, FontStyle.Bold)
                        Dim rLabel = New Rectangle(leftX + (pageWidth \ 2), currY, (pageWidth \ 2) - 10, 28)
                        g.DrawString("جمع کل هزینه‌ها:", fTotals, Brushes.Black, rLabel, sfRight)

                        Dim rValue = New Rectangle(leftX + 10, currY, (pageWidth \ 2) - 10, 28)
                        g.DrawString(lblGrandTotal.Text.Replace("جمع کل هزینه‌ها: ", ""), fTotals, Brushes.Black, rValue, sfLeft)
                    End Using

                    currY += 28

                    ' ۶. امضاداران
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

                ev.HasMorePages = Not isLastPage
                If isLastPage Then rowIndex = 0
            End Sub

            Using dlg As New ReportPrintPreviewForm(doc, "پیش‌نمایش و تنظیمات چاپ - گزارش هزینه‌ها")
                dlg.ShowDialog(Me)
            End Using
        End Sub
    End Class
End Namespace
