Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Negar.Data
Imports Negar.Forms.Moshtarak

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniExpenseLedgerDialog

        Private _reportRows As List(Of DataRow)
        Private _currentPageIndex As Integer = 0
        Private _ledgerTitleHeader As String = ""
        Private _dateRangeHeader As String = ""
        Private _totalExpenseAmount As Long = 0

        Public Sub New(Optional defaultFromDate As String = "", Optional defaultToDate As String = "")
            InitializeComponent()
            txtFromDate.Text = defaultFromDate
            txtToDate.Text = defaultToDate
        End Sub

        Private Sub AnbarMiniExpenseLedgerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            PopulateCategories()
            PopulateTitles()
            UpdateControlStates()
        End Sub

        Private Sub PopulateCategories()
            cmbCategory.Items.Clear()
            cmbCategory.Items.Add("همه سرفصل‌ها")

            ' Default categories
            Dim defaults = New String() {"هزینه‌های جاری", "اجاره محل", "حقوق و دستمزد", "حمل و نقل", "بازاریابی و تبلیغات", "تعمیرات و نگهداری", "ملزومات و اداری", "مالیات و عوارض", "سایر هزینه‌ها"}
            For Each c In defaults
                If Not cmbCategory.Items.Contains(c) Then cmbCategory.Items.Add(c)
            Next

            ' Fetch distinct from DB
            Try
                Dim dt = Sql.ExecuteTable("SELECT DISTINCT Category FROM Expenses WHERE Category IS NOT NULL AND Category <> '' ORDER BY Category")
                For Each row As DataRow In dt.Rows
                    Dim catName = Convert.ToString(row("Category"))
                    If Not cmbCategory.Items.Contains(catName) Then
                        cmbCategory.Items.Add(catName)
                    End If
                Next
            Catch ex As Exception
            End Try

            cmbCategory.SelectedIndex = 0
        End Sub

        Private Sub PopulateTitles()
            cmbTitle.Items.Clear()
            cmbTitle.Items.Add("همه عناوین هزینه")

            Try
                Dim dt = Sql.ExecuteTable("SELECT DISTINCT ExpenseTitle FROM Expenses WHERE ExpenseTitle IS NOT NULL AND ExpenseTitle <> '' ORDER BY ExpenseTitle")
                For Each row As DataRow In dt.Rows
                    Dim titleName = Convert.ToString(row("ExpenseTitle"))
                    If Not cmbTitle.Items.Contains(titleName) Then
                        cmbTitle.Items.Add(titleName)
                    End If
                Next
            Catch ex As Exception
            End Try

            cmbTitle.SelectedIndex = 0
        End Sub

        Private Sub rbCategoryLevel_CheckedChanged(sender As Object, e As EventArgs) Handles rbCategoryLevel.CheckedChanged, rbTitleLevel.CheckedChanged
            UpdateControlStates()
        End Sub

        Private Sub UpdateControlStates()
            If rbCategoryLevel.Checked Then
                cmbCategory.Enabled = True
                lblSelectCategory.Enabled = True
                cmbTitle.Enabled = False
                lblSelectTitle.Enabled = False
            Else
                cmbCategory.Enabled = False
                lblSelectCategory.Enabled = False
                cmbTitle.Enabled = True
                lblSelectTitle.Enabled = True
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

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub

        Private Sub btnPreviewPrint_Click(sender As Object, e As EventArgs) Handles btnPreviewPrint.Click
            ' 1. Build Query & Fetch Data
            Dim sqlQuery As String = "SELECT ExpenseID, ExpenseDate, ExpenseTitle, Category, Amount, PaidTo, PaymentMethod, ReferenceNo, Description FROM Expenses WHERE 1=1"
            Dim params As New List(Of Object)()

            If rbCategoryLevel.Checked Then
                Dim selCat = cmbCategory.Text.Trim()
                If cmbCategory.SelectedIndex > 0 AndAlso Not String.IsNullOrEmpty(selCat) Then
                    sqlQuery &= " AND Category = ?"
                    params.Add(selCat)
                    _ledgerTitleHeader = "دفتر سرفصل هزینه: " & selCat
                Else
                    _ledgerTitleHeader = "دفتر کلی سرفصل‌های هزینه (تمام دسته‌بندی‌ها)"
                End If
            Else
                Dim selTitle = cmbTitle.Text.Trim()
                If cmbTitle.SelectedIndex > 0 AndAlso Not String.IsNullOrEmpty(selTitle) Then
                    sqlQuery &= " AND ExpenseTitle LIKE ?"
                    params.Add("%" & selTitle & "%")
                    _ledgerTitleHeader = "دفتر عنوان هزینه: " & selTitle
                Else
                    _ledgerTitleHeader = "دفتر عناوین هزینه (تمام شرح‌ها)"
                End If
            End If

            ' Date filters
            Dim fDate = txtFromDate.Text.Trim()
            Dim tDate = txtToDate.Text.Trim()
            If Not String.IsNullOrEmpty(fDate) Then
                sqlQuery &= " AND ExpenseDate >= ?"
                params.Add(fDate)
            End If
            If Not String.IsNullOrEmpty(tDate) Then
                sqlQuery &= " AND ExpenseDate <= ?"
                params.Add(tDate)
            End If

            sqlQuery &= " ORDER BY ExpenseDate ASC, ExpenseID ASC"

            ' Date Range Header Text
            If Not String.IsNullOrEmpty(fDate) AndAlso Not String.IsNullOrEmpty(tDate) Then
                _dateRangeHeader = "از تاریخ: " & fDate & " تا تاریخ: " & tDate
            ElseIf Not String.IsNullOrEmpty(fDate) Then
                _dateRangeHeader = "از تاریخ: " & fDate
            ElseIf Not String.IsNullOrEmpty(tDate) Then
                _dateRangeHeader = "تا تاریخ: " & tDate
            Else
                _dateRangeHeader = "دوره مالی جاری (بدون محدودیت تاریخ)"
            End If

            Try
                Dim dt As DataTable = Sql.ExecuteTable(sqlQuery, params.ToArray())
                _reportRows = New List(Of DataRow)()
                _totalExpenseAmount = 0

                For Each r As DataRow In dt.Rows
                    _reportRows.Add(r)
                    If Not r("Amount") Is DBNull.Value Then
                        _totalExpenseAmount += Convert.ToInt64(r("Amount"))
                    End If
                Next

                If _reportRows.Count = 0 Then
                    MessageBox.Show("هیچ سندی با مشخصات انتخابی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                ' 2. Launch Print Preview
                _currentPageIndex = 0
                Dim doc As New PrintDocument()
                doc.DefaultPageSettings.Landscape = True
                AddHandler doc.PrintPage, AddressOf PrintDoc_PrintPage

                Using previewForm As New ReportPrintPreviewForm(doc)
                    previewForm.ShowDialog(Me)
                End Using

            Catch ex As Exception
                MessageBox.Show("خطا در گزارش‌گیری دفتر هزینه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub PrintDoc_PrintPage(sender As Object, e As PrintPageEventArgs)
            Dim g = e.Graphics
            Dim bounds = e.MarginBounds
            If bounds.Width < 100 Then bounds = New Rectangle(20, 20, e.PageBounds.Width - 40, e.PageBounds.Height - 40)

            ' Fonts
            Dim fnTitle As New Font("Tahoma", 13.0!, FontStyle.Bold)
            Dim fnSubTitle As New Font("Tahoma", 10.0!, FontStyle.Bold)
            Dim fnHeader As New Font("Tahoma", 9.0!, FontStyle.Bold)
            Dim fnBody As New Font("Tahoma", 8.5!, FontStyle.Regular)
            Dim fnBold As New Font("Tahoma", 9.0!, FontStyle.Bold)

            ' Pens & Brushes
            Dim penBorder As New Pen(Color.FromArgb(0, 100, 140), 1.5!)
            Dim penGrid As New Pen(Color.LightGray, 1)
            Dim brBgHeader As New SolidBrush(Color.FromArgb(230, 242, 250))
            Dim brBgAlt As New SolidBrush(Color.FromArgb(248, 250, 252))
            Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim sfFar As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
            Dim sfNear As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}

            Dim yPosition As Integer = bounds.Top

            ' --- Header Box ---
            Dim headerRect As New Rectangle(bounds.Left, yPosition, bounds.Width, 65)
            g.FillRectangle(New SolidBrush(Color.FromArgb(240, 248, 255)), headerRect)
            g.DrawRectangle(penBorder, headerRect)

            Dim strCompanyName = "مدیریت فروشگاه و انبار - نسخه مینی"
            Try
                If String.IsNullOrEmpty(strCompanyName) Then strCompanyName = "سیستم حسابداری و انبارداری نگار"
            Catch
            End Try

            g.DrawString(strCompanyName, fnSubTitle, Brushes.DarkSlateGray, New Rectangle(bounds.Left + 15, yPosition + 10, bounds.Width - 30, 22), sfNear)
            g.DrawString("📒 " & _ledgerTitleHeader, fnTitle, Brushes.Navy, New Rectangle(bounds.Left + 15, yPosition + 8, bounds.Width - 30, 28), sfCenter)
            g.DrawString(_dateRangeHeader, fnSubTitle, Brushes.DarkBlue, New Rectangle(bounds.Left + 15, yPosition + 38, bounds.Width - 30, 22), sfCenter)
            g.DrawString("تاریخ چاپ: " & DateTime.Now.ToString("yyyy/MM/dd"), fnBody, Brushes.Black, New Rectangle(bounds.Left + 15, yPosition + 38, bounds.Width - 30, 22), sfFar)

            yPosition += 75

            ' --- Table Layout ---
            ' Columns: ردیف (40), تاریخ (80), عنوان/شرح هزینه (180), سرفصل (140), دریافت کننده (140), نحوه پرداخت / ش پیگیری (150), مبلغ به ریال (Rest)
            Dim colWidths = New Integer() {45, 85, 190, 140, 140, 150, bounds.Width - (45 + 85 + 190 + 140 + 140 + 150)}
            Dim colHeaders = New String() {"ردیف", "تاریخ", "عنوان / شرح هزینه", "سرفصل / دسته", "پرداخت شده به", "نحوه پرداخت / پیگیری", "مبلغ (ریال)"}

            Dim rowHeight As Integer = 25

            ' Header Row
            Dim curX As Integer = bounds.Right
            For i As Integer = 0 To colHeaders.Length - 1
                curX -= colWidths(i)
                Dim cellRect As New Rectangle(curX, yPosition, colWidths(i), rowHeight)
                g.FillRectangle(brBgHeader, cellRect)
                g.DrawRectangle(penBorder, cellRect)
                g.DrawString(colHeaders(i), fnHeader, Brushes.DarkBlue, cellRect, sfCenter)
            Next

            yPosition += rowHeight

            ' Data Rows
            Dim maxRowsPerPage As Integer = CInt(Math.Floor((bounds.Bottom - yPosition - 60) / rowHeight))
            Dim rowCountOnThisPage As Integer = 0

            While _currentPageIndex < _reportRows.Count AndAlso rowCountOnThisPage < maxRowsPerPage
                Dim row = _reportRows(_currentPageIndex)
                curX = bounds.Right

                Dim isAlt = (_currentPageIndex Mod 2 = 1)
                Dim rowBg = If(isAlt, brBgAlt, Brushes.White)

                ' Row Data Values
                Dim strRowNo = (_currentPageIndex + 1).ToString()
                Dim strDate = Convert.ToString(row("ExpenseDate"))
                Dim strTitle = Convert.ToString(row("ExpenseTitle"))
                Dim strCat = Convert.ToString(row("Category"))
                Dim strPayee = Convert.ToString(row("PaidTo"))
                Dim strPayMethod = Convert.ToString(row("PaymentMethod"))
                Dim strRefNo = Convert.ToString(row("ReferenceNo"))
                Dim strPayInfo = strPayMethod & If(Not String.IsNullOrEmpty(strRefNo), " (" & strRefNo & ")", "")
                Dim amt As Long = If(row("Amount") Is DBNull.Value, 0L, Convert.ToInt64(row("Amount")))
                Dim strAmount = amt.ToString("N0")

                Dim cellValues = New String() {strRowNo, strDate, strTitle, strCat, strPayee, strPayInfo, strAmount}

                For i As Integer = 0 To cellValues.Length - 1
                    curX -= colWidths(i)
                    Dim cellRect As New Rectangle(curX, yPosition, colWidths(i), rowHeight)
                    g.FillRectangle(rowBg, cellRect)
                    g.DrawRectangle(penGrid, cellRect)

                    Dim fmt = If(i = 6, sfFar, If(i = 2 Or i = 3 Or i = 4, sfNear, sfCenter))
                    ' Pad inner text
                    Dim txtRect = cellRect
                    txtRect.Inflate(-4, 0)
                    g.DrawString(cellValues(i), fnBody, Brushes.Black, txtRect, fmt)
                Next

                yPosition += rowHeight
                _currentPageIndex += 1
                rowCountOnThisPage += 1
            End While

            ' Total Box (if last page)
            If _currentPageIndex >= _reportRows.Count Then
                ' Total Row
                Dim totalRect As New Rectangle(bounds.Left, yPosition, bounds.Width, 30)
                g.FillRectangle(New SolidBrush(Color.FromArgb(235, 245, 235)), totalRect)
                g.DrawRectangle(penBorder, totalRect)

                Dim strTotalText = "جمع کل هزینه‌های دفتر (" & _reportRows.Count & " سند):   " & _totalExpenseAmount.ToString("N0") & " ریال"
                g.DrawString(strTotalText, fnBold, Brushes.DarkGreen, totalRect, sfCenter)

                yPosition += 45

                ' Signature block
                Dim sigBoxWidth = CInt(bounds.Width / 3)
                Dim sigY = yPosition
                g.DrawString("امضاء تنظیم کننده / حسابدار", fnHeader, Brushes.DarkGray, New Rectangle(bounds.Right - sigBoxWidth, sigY, sigBoxWidth, 20), sfCenter)
                g.DrawString("امضاء و تأیید مدیر مالی", fnHeader, Brushes.DarkGray, New Rectangle(bounds.Right - (sigBoxWidth * 2), sigY, sigBoxWidth, 20), sfCenter)
                g.DrawString("امضاء و تأیید مدیر عامل", fnHeader, Brushes.DarkGray, New Rectangle(bounds.Left, sigY, sigBoxWidth, 20), sfCenter)
            End If

            ' Page Numbers
            g.DrawString("صفحه " & (_currentPageIndex \ maxRowsPerPage + 1).ToString(), fnBody, Brushes.Gray, New Rectangle(bounds.Left, bounds.Bottom + 5, bounds.Width, 20), sfCenter)

            ' More pages?
            e.HasMorePages = (_currentPageIndex < _reportRows.Count)
        End Sub
    End Class
End Namespace
