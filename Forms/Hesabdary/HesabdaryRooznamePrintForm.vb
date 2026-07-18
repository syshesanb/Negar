Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.Linq
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryRooznamePrintForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private ReadOnly compService As New CompanyFiscalYearService()

        ' داده‌های ورودی جهت چاپ
        Private ReadOnly _fromRef As Integer?
        Private ReadOnly _toRef As Integer?
        Private ReadOnly _fromDate As String
        Private ReadOnly _toDate As String
        Private ReadOnly _journalDocs As New List(Of JournalDoc)()

        ' متغیرهای صفحه بندی و ترسیم
        Private _pages As New List(Of List(Of JournalRow))()
        Private _currentPageIndex As Integer = 0
        Private _totalPages As Integer = 1
        Private Const PageCapacity As Integer = 35

        ' متغیرهای کشیدن و حرکت (Panning) گزارش با کلیک راست
        Private _isPanning As Boolean = False
        Private _startMousePos As Point
        Private _startScrollPosX As Integer
        Private _startScrollPosY As Integer

        ' کش لوگوی شرکت و مشخصات
        Private _logoImage As Image = Nothing
        Private _logoPosition As String = "Left"

        Public Class JournalDoc
            Public Property EntryID As Integer
            Public Property ReferenceNumber As String
            Public Property EntryDate As String
            Public Property Description As String
            Public Property RawLinesTable As DataTable
        End Class

        Public Class JournalRow
            Public Property ReferenceNumber As String = String.Empty
            Public Property EntryDate As String = String.Empty
            Public Property AccountCode As String = String.Empty
            Public Property AccountName As String = String.Empty
            Public Property DebitAmount As Decimal? = Nothing
            Public Property CreditAmount As Decimal? = Nothing
            Public Property IsCredit As Boolean = False
            Public Property IsDescriptionRow As Boolean = False
            Public Property IsCarryForward As Boolean = False
            Public Property IsCarryFrom As Boolean = False
            Public Property IsTotalRow As Boolean = False
        End Class

        Public Sub New(fromRef As Integer?, toRef As Integer?, fromDate As String, toDate As String)
            InitializeComponent()
            _fromRef = fromRef
            _toRef = toRef
            _fromDate = fromDate
            _toDate = toDate
        End Sub

        Private Sub LoadJournalDocuments(Optional progress As ProgressForm = Nothing)
            Try
                Dim dtEntries = service.GetEntriesForPrint(_fromRef, _toRef, _fromDate, _toDate)
                Dim total = dtEntries.Rows.Count
                Dim current = 0
                For Each row As DataRow In dtEntries.Rows
                    current += 1
                    Dim entryId = Convert.ToInt32(row("EntryID"))
                    Dim refNo = Convert.ToString(row("ReferenceNumber"))
                    
                    If progress IsNot Nothing Then
                        Dim percent = 20 + CInt((current / total) * 50) ' 20% to 70%
                        progress.UpdateProgress(percent, "بارگذاری جزئیات سند دفتر روزنامه شماره " & refNo & " (" & current & " از " & total & ")...")
                    End If

                    ' قالب بندی تاریخ
                    Dim dateVal = row("EntryDate")
                    Dim dateStr As String = ""
                    If dateVal IsNot Nothing AndAlso dateVal IsNot DBNull.Value Then
                        dateStr = PersianDateHelper.FormatDateTime(Convert.ToDateTime(dateVal))
                    End If
                    
                    Dim desc = Convert.ToString(row("Description"))
                    Dim linesTable = service.GetEntryDetails(entryId)
                    
                    Dim doc As New JournalDoc() With {
                        .EntryID = entryId,
                        .ReferenceNumber = refNo,
                        .EntryDate = dateStr,
                        .Description = desc,
                        .RawLinesTable = linesTable
                    }
                    _journalDocs.Add(doc)
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در واکشی اسناد جهت دفتر روزنامه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub HesabdaryRooznamePrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "بارگذاری مشخصات چاپگرها...")

                ' ۱. لود لیست چاپگرهای سیستم
                cmbPrinter.Items.Clear()
                For Each prt As String In PrinterSettings.InstalledPrinters
                    cmbPrinter.Items.Add(prt)
                Next
                
                ' انتخاب چاپگر پیش‌فرض
                Dim defaultPrinter = printDoc.PrinterSettings.PrinterName
                If cmbPrinter.Items.Contains(defaultPrinter) Then
                    cmbPrinter.SelectedItem = defaultPrinter
                ElseIf cmbPrinter.Items.Count > 0 Then
                    cmbPrinter.SelectedIndex = 0
                End If

                progress.UpdateProgress(15, "بارگذاری مشخصات لوگوی شرکت...")
                ' ۲. واکشی لوگو
                LoadCompanyLogo()

                progress.UpdateProgress(18, "تنظیم سطوح و دسترسی‌های سرفصل‌ها...")
                ' ۳. فعال‌سازی چک‌باکس‌های سطوح حساب بر اساس تنظیمات شرکت
                SetupLevelCheckboxes()

                ' بارگذاری دسته‌ای اسناد در صورت لزوم
                If _journalDocs.Count = 0 Then
                    LoadJournalDocuments(progress)
                End If

                progress.UpdateProgress(70, "بازسازی و صفحه‌بندی ردیف‌های دفتر روزنامه...")
                ' ۴. بازسازی ردیف‌های دفتر روزنامه و صفحه‌بندی
                RebuildActiveRows(progress)

                progress.UpdateProgress(95, "آماده‌سازی نهایی پیش‌نمایش چاپ...")
                ' ۵. تنظیم اولیه سند جهت چاپ A4 عمودی
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
                printDoc.DefaultPageSettings.Margins = New Margins(50, 50, 50, 50)

                ' به روز رسانی زنده پیش‌نمایش
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()

                progress.UpdateProgress(100, "پیش‌نمایش دفتر روزنامه آماده شد")
            End Using
        End Sub

        Private Sub LoadCompanyLogo()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim dt = Sql.ExecuteTable("SELECT LogoImage, LogoPosition FROM Companies WHERE CompanyID = ?", companyId)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    If Not row.IsNull("LogoImage") Then
                        Dim bytes = DirectCast(row("LogoImage"), Byte())
                        If bytes.Length > 0 Then
                            Using ms As New IO.MemoryStream(bytes)
                                _logoImage = Image.FromStream(ms)
                            End Using
                        End If
                    End If
                    If Not row.IsNull("LogoPosition") Then
                        _logoPosition = row("LogoPosition").ToString()
                    End If
                End If
            Catch
            End Try
        End Sub

        Private Sub SetupLevelCheckboxes()
            Dim settings = service.GetCompanyAccountSettings()
            Dim maxLevels = settings.Item1

            chkGroup.Enabled = (maxLevels >= 1)
            chkGeneral.Enabled = (maxLevels >= 2)
            chkSubsidiary.Enabled = (maxLevels >= 3)
            chkDetail1.Enabled = (maxLevels >= 4)
            chkDetail2.Enabled = (maxLevels >= 5)

            chkGroup.Checked = chkGroup.Enabled
            chkGeneral.Checked = chkGeneral.Enabled
            chkSubsidiary.Checked = chkSubsidiary.Enabled
            chkDetail1.Checked = chkDetail1.Enabled
            chkDetail2.Checked = chkDetail2.Enabled

            AddHandler btnReload.Click, Sub(s, ev)
                                            Using progress As New ProgressForm()
                                                progress.ShowAndCenter(Me)
                                                progress.UpdateProgress(10, "در حال بازخوانی سرفصل‌ها و بازسازی دفتر روزنامه...")
                                                RebuildActiveRows(progress)
                                                progress.UpdateProgress(95, "به‌روزرسانی پیش‌نمایش...")
                                                previewCtrl.InvalidatePreview()
                                                progress.UpdateProgress(100, "بازخوانی با موفقیت انجام شد")
                                            End Using
                                        End Sub
        End Sub

        Private Sub cmbPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrinter.SelectedIndexChanged
            If cmbPrinter.SelectedItem IsNot Nothing Then
                printDoc.PrinterSettings.PrinterName = cmbPrinter.SelectedItem.ToString()
                previewCtrl.InvalidatePreview()
            End If
        End Sub

        Private Sub numCopies_ValueChanged(sender As Object, e As EventArgs) Handles numCopies.ValueChanged
            printDoc.PrinterSettings.Copies = CShort(numCopies.Value)
        End Sub

        Private Sub btnPageSetup_Click(sender As Object, e As EventArgs) Handles btnPageSetup.Click
            If dialogPageSetup.ShowDialog() = DialogResult.OK Then
                previewCtrl.InvalidatePreview()
            End If
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
            Try
                _currentPageIndex = 0
                printDoc.Print()
                MessageBox.Show("دفتر روزنامه با موفقیت به چاپگر ارسال شد.", "چاپ موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در چاپ دفتر روزنامه: " & ex.Message, "خطا در چاپ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        ' ========================
        ' رویداد ترسیم صفحات چاپ دفتر روزنامه
        ' ========================

        Private Sub printDoc_BeginPrint(sender As Object, e As PrintEventArgs) Handles printDoc.BeginPrint
            _currentPageIndex = 0
        End Sub

        Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
            Dim g = e.Graphics
            Dim leftX = e.MarginBounds.Left
            Dim rightX = e.MarginBounds.Right
            Dim topY = e.MarginBounds.Top
            Dim bottomY = e.MarginBounds.Bottom
            Dim pageWidth = rightX - leftX
            Dim pageHeight = bottomY - topY

            ' ۱. ترسیم کادر دور صفحه
            Using pBorder As New Pen(Color.Black, 2.0!)
                g.DrawRectangle(pBorder, leftX, topY, pageWidth, pageHeight)
            End Using

            ' ۲. هدر: لوگوی شرکت
            If _logoImage IsNot Nothing Then
                If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                    g.DrawImage(_logoImage, rightX - 75, topY + 15, 60, 60)
                Else
                    g.DrawImage(_logoImage, leftX + 15, topY + 15, 60, 60)
                End If
            End If

            ' هدر: عنوان وسط صفحه
            Dim fTitle As New Font("Tahoma", 15.0!, FontStyle.Bold)
            Dim fSubTitle As New Font("Tahoma", 12.0!, FontStyle.Bold)
            Dim fRegular As New Font("Tahoma", 9.0!, FontStyle.Regular)
            Dim fBold As New Font("Tahoma", 9.0!, FontStyle.Bold)

            Dim companyName = "شرکت " & SessionContext.CurrentCompanyName
            Dim titleText = "دفتر روزنامه"

            Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center}
            Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far}
            Dim sfLeft As New StringFormat() With {.Alignment = StringAlignment.Near}

            ' محاسبه عرض صفحه و فضای یک سوم جهت شرکت و عنوان
            Dim compWidth = pageWidth \ 3
            Dim sizeCompName = g.MeasureString(companyName, fTitle, compWidth)
            Dim rectCompName As New Rectangle(leftX + compWidth, topY + 15, compWidth, CInt(sizeCompName.Height) + 5)

            g.DrawString(companyName, fTitle, Brushes.DarkRed, rectCompName, sfCenter)

            Dim titleY = rectCompName.Bottom + 5
            g.DrawString(titleText, fSubTitle, Brushes.DarkRed, leftX + (pageWidth \ 2), titleY, sfCenter)

            ' هدر: فیلترها (بر اساس موقعیت لوگو)
            Dim filterText As String = ""
            If _fromRef.HasValue AndAlso _toRef.HasValue Then
                filterText = "از شماره سند: " & _fromRef.Value & " تا: " & _toRef.Value
            ElseIf Not String.IsNullOrEmpty(_fromDate) AndAlso Not String.IsNullOrEmpty(_toDate) Then
                filterText = "از تاریخ: " & _fromDate & " تا: " & _toDate
            End If
            
            If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                g.DrawString(filterText, fBold, Brushes.Black, leftX + 15, topY + 20, sfLeft)
            Else
                g.DrawString(filterText, fBold, Brushes.Black, rightX - 15, topY + 20, sfRight)
            End If

            ' ۳. ترسیم ساختار جدول
            Dim tableStartY = topY + 110
            Dim rowHeight = 25
            Dim headerHeight = 30
            Dim tableEndY = tableStartY + headerHeight + (PageCapacity * rowHeight)

            ' ستون‌ها از راست به چپ:
            ' شماره سند (۷۰ پیکسل)
            ' تاریخ (۹۰ پیکسل)
            ' شرح (۳۲۷ پیکسل)
            ' بدهکار (۱۲۰ پیکسل)
            ' بستانکار (۱۲۰ پیکسل)
            Dim colWidths = New Integer() {70, 90, 327, 120, 120}
            Dim colX = New Integer(5) {}
            colX(0) = rightX
            colX(1) = colX(0) - colWidths(0)
            colX(2) = colX(1) - colWidths(1)
            colX(3) = colX(2) - colWidths(2)
            colX(4) = colX(3) - colWidths(3)
            colX(5) = leftX

            ' هدر جدول
            Dim rectHeader As New Rectangle(leftX, tableStartY, pageWidth, headerHeight)
            Using brHeader As New SolidBrush(Color.FromArgb(220, 230, 242))
                g.FillRectangle(brHeader, rectHeader)
            End Using
            g.DrawRectangle(Pens.Black, rectHeader)

            Dim headers = New String() {"شماره سند", "تاریخ", "شرح", "بدهکار", "بستانکار"}
            For i = 0 To 4
                Dim rectColHeader As New Rectangle(colX(i + 1), tableStartY, colWidths(i), headerHeight)
                g.DrawRectangle(Pens.Black, rectColHeader)
                
                Dim sfCol As New StringFormat() With {
                    .Alignment = StringAlignment.Center,
                    .LineAlignment = StringAlignment.Center
                }
                g.DrawString(headers(i), fBold, Brushes.Black, rectColHeader, sfCol)
            Next

            ' ۴. رسم ردیف‌های صفحه جاری
            Dim pageRows As New List(Of JournalRow)()
            If _currentPageIndex < _pages.Count Then
                pageRows = _pages(_currentPageIndex)
            End If

            Dim currY = tableStartY + headerHeight
            Dim sfTextCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim sfTextRight As New StringFormat() With {.Alignment = System.Drawing.StringAlignment.Far, .LineAlignment = StringAlignment.Center}
            
            For i = 0 To pageRows.Count - 1
                Dim row = pageRows(i)
                Dim nextRow = If(i < pageRows.Count - 1, pageRows(i + 1), Nothing)

                ' ۱. رنگ پس‌زمینه سطرهای جمع یا نقل/منقول
                If row.IsTotalRow Then
                    Using brTotals As New SolidBrush(Color.FromArgb(255, 255, 204))
                        g.FillRectangle(brTotals, leftX, currY, pageWidth, rowHeight)
                    End Using
                ElseIf row.IsCarryForward OrElse row.IsCarryFrom Then
                    Using brCarry As New SolidBrush(Color.FromArgb(240, 245, 255))
                        g.FillRectangle(brCarry, leftX, currY, pageWidth, rowHeight)
                    End Using
                End If

                ' ۲. ترسیم متون سلول‌ها
                ' شماره سند و تاریخ (ادغام شده برای یک سند)
                If Not row.IsCarryForward AndAlso Not row.IsCarryFrom AndAlso Not row.IsTotalRow Then
                    Dim shouldDrawRefAndDate = False
                    If i = 0 Then
                        shouldDrawRefAndDate = True
                    Else
                        Dim prevRow = pageRows(i - 1)
                        If prevRow.IsCarryFrom OrElse prevRow.ReferenceNumber <> row.ReferenceNumber Then
                            shouldDrawRefAndDate = True
                        End If
                    End If

                    If shouldDrawRefAndDate Then
                        Dim rRef = New Rectangle(colX(1) + 2, currY, colWidths(0) - 4, rowHeight)
                        g.DrawString(row.ReferenceNumber, fRegular, Brushes.Black, rRef, sfTextCenter)

                        Dim rDate = New Rectangle(colX(2) + 2, currY, colWidths(1) - 4, rowHeight)
                        g.DrawString(row.EntryDate, fRegular, Brushes.Black, rDate, sfTextCenter)
                    End If
                End If

                ' شرح
                Dim rDesc As Rectangle
                If row.IsCarryForward OrElse row.IsCarryFrom OrElse row.IsTotalRow Then
                    ' سطر نقل، منقول یا جمع کل -> در کل محدوده ستون شرح راست‌چین می‌شود
                    rDesc = New Rectangle(colX(3) + 5, currY, colWidths(2) - 10, rowHeight)
                    g.DrawString(row.AccountName, fBold, Brushes.Black, rDesc, sfTextRight)
                ElseIf row.IsDescriptionRow Then
                    ' سطر شرح سند -> تورفتگی بیشتر و فونت معمولی
                    rDesc = New Rectangle(colX(3) + 5, currY, colWidths(2) - 55, rowHeight)
                    g.DrawString(row.AccountName, fRegular, Brushes.Black, rDesc, sfTextRight)
                Else
                    ' سطر سرفصل حساب
                    If row.IsCredit Then
                        ' بستانکار -> شروع بعد از یک سوم اول سطر (دو سوم بعدی راست‌چین)
                        rDesc = New Rectangle(colX(3) + 5, currY, (colWidths(2) * 2 \ 3) - 10, rowHeight)
                    Else
                        ' بدهکار
                        rDesc = New Rectangle(colX(3) + 5, currY, colWidths(2) - 10, rowHeight)
                    End If
                    g.DrawString(row.AccountName, fRegular, Brushes.Black, rDesc, sfTextRight)
                End If

                ' بدهکار
                If row.DebitAmount.HasValue Then
                    Dim rDeb = New Rectangle(colX(4) + 5, currY, colWidths(3) - 10, rowHeight)
                    g.DrawString(row.DebitAmount.Value.ToString("N0"), If(row.IsCarryForward OrElse row.IsCarryFrom OrElse row.IsTotalRow, fBold, fRegular), Brushes.Black, rDeb, sfTextRight)
                End If

                ' بستانکار
                If row.CreditAmount.HasValue Then
                    Dim rCred = New Rectangle(colX(5) + 5, currY, colWidths(4) - 10, rowHeight)
                    g.DrawString(row.CreditAmount.Value.ToString("N0"), If(row.IsCarryForward OrElse row.IsCarryFrom OrElse row.IsTotalRow, fBold, fRegular), Brushes.Black, rCred, sfTextRight)
                End If

                ' ۳. رسم خطوط افقی
                Dim drawFullHorizontal = False
                If row.IsCarryForward OrElse row.IsCarryFrom OrElse row.IsTotalRow Then
                    drawFullHorizontal = True
                ElseIf nextRow Is Nothing OrElse nextRow.IsCarryFrom OrElse nextRow.IsTotalRow OrElse nextRow.ReferenceNumber <> row.ReferenceNumber Then
                    drawFullHorizontal = True
                End If

                If drawFullHorizontal Then
                    g.DrawLine(Pens.Black, leftX, currY + rowHeight, rightX, currY + rowHeight)
                Else
                    Using pDash As New Pen(Color.LightGray) With {.DashStyle = DashStyle.Dash}
                        g.DrawLine(pDash, colX(3), currY + rowHeight, rightX, currY + rowHeight)
                    End Using
                End If

                ' ۴. رسم خطوط عمودی داخلی فقط برای سطرهای غیر تجمعی
                If Not row.IsCarryForward AndAlso Not row.IsCarryFrom AndAlso Not row.IsTotalRow Then
                    g.DrawLine(Pens.Black, colX(1), currY, colX(1), currY + rowHeight)
                    g.DrawLine(Pens.Black, colX(2), currY, colX(2), currY + rowHeight)
                End If

                currY += rowHeight
            Next

            ' ۵. امتداد خطوط تا انتهای فریم
            While currY < tableEndY
                Dim h = Math.Min(rowHeight, tableEndY - currY)
                Using pDash As New Pen(Color.LightGray) With {.DashStyle = DashStyle.Dash}
                    g.DrawLine(pDash, colX(3), currY, rightX, currY)
                End Using
                
                ' رسم خطوط عمودی شماره سند و تاریخ در قسمت‌های خالی
                g.DrawLine(Pens.Black, colX(1), currY, colX(1), currY + h)
                g.DrawLine(Pens.Black, colX(2), currY, colX(2), currY + h)

                currY += h
            End While
            g.DrawLine(Pens.Black, leftX, tableEndY, rightX, tableEndY)

            ' ۶. رسم خطوط عمودی سراسری
            g.DrawLine(Pens.Black, colX(0), tableStartY, colX(0), tableEndY)
            g.DrawLine(Pens.Black, colX(3), tableStartY, colX(3), tableEndY)
            g.DrawLine(Pens.Black, colX(4), tableStartY, colX(4), tableEndY)
            g.DrawLine(Pens.Black, colX(5), tableStartY, colX(5), tableEndY)

            ' ۷. ترسیم شماره صفحه
            Dim pageNoStr = String.Format("صفحه : {0} از {1}", _currentPageIndex + 1, _totalPages)
            g.DrawString(pageNoStr, fRegular, Brushes.Black, leftX + (pageWidth \ 2), bottomY - 25, sfCenter)

            ' مدیریت فرآیند چند صفحه‌ای
            _currentPageIndex += 1
            If _currentPageIndex < _totalPages Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                _currentPageIndex = 0
            End If
        End Sub

        Private Sub RebuildActiveRows(Optional progress As ProgressForm = Nothing)
            _pages.Clear()
            _totalPages = 0
            Dim hierarchyCache As New Dictionary(Of Integer, List(Of Tuple(Of String, String)))()

            ' ساخت لیست خام کلیه ردیف‌های دفتر روزنامه در محدوده فیلتر شده
            Dim rawRows As New List(Of JournalRow)()

            Dim totalDocs = _journalDocs.Count
            Dim currentDocIndex = 0

            For Each doc In _journalDocs
                currentDocIndex += 1
                If progress IsNot Nothing Then
                    Dim percent = 70 + CInt((currentDocIndex / totalDocs) * 25) ' 70% to 95%
                    progress.UpdateProgress(percent, "صفحه‌بندی و تحلیل ساختاری سند دفتر روزنامه شماره " & doc.ReferenceNumber & " (" & currentDocIndex & " از " & totalDocs & ")...")
                End If

                If doc.RawLinesTable Is Nothing Then Continue For

                Dim rawLines = doc.RawLinesTable.AsEnumerable().Where(Function(r)
                    Dim accId = 0
                    If r.Table.Columns.Contains("AccountID") AndAlso Not r.IsNull("AccountID") Then
                        accId = Convert.ToInt32(r("AccountID"))
                    End If
                    Return accId > 0
                End Function).ToList()

                Dim debitForest As New List(Of VoucherAccountNode)()
                Dim creditForest As New List(Of VoucherAccountNode)()

                For Each row In rawLines
                    Dim accId = Convert.ToInt32(row("AccountID"))
                    Dim debit = If(row.Table.Columns.Contains("DebitAmount") AndAlso Not row.IsNull("DebitAmount"), Convert.ToDecimal(row("DebitAmount")), 0D)
                    Dim credit = If(row.Table.Columns.Contains("CreditAmount") AndAlso Not row.IsNull("CreditAmount"), Convert.ToDecimal(row("CreditAmount")), 0D)

                    Dim chain As List(Of Tuple(Of String, String)) = Nothing
                    If Not hierarchyCache.TryGetValue(accId, chain) Then
                        chain = service.GetAccountHierarchyChain(accId)
                        hierarchyCache(accId) = chain
                    End If
                    If chain IsNot Nothing AndAlso chain.Count > 0 Then
                        Dim path As New List(Of Tuple(Of String, String, Integer))()
                        For i = 0 To chain.Count - 1
                            Dim isChecked = False
                            If i = 0 AndAlso chkGroup.Checked AndAlso chkGroup.Enabled Then isChecked = True
                            If i = 1 AndAlso chkGeneral.Checked AndAlso chkGeneral.Enabled Then isChecked = True
                            If i = 2 AndAlso chkSubsidiary.Checked AndAlso chkSubsidiary.Enabled Then isChecked = True
                            If i = 3 AndAlso chkDetail1.Checked AndAlso chkDetail1.Enabled Then isChecked = True
                            If i = 4 AndAlso chkDetail2.Checked AndAlso chkDetail2.Enabled Then isChecked = True

                            If isChecked Then
                                path.Add(Tuple.Create(chain(i).Item1, chain(i).Item2, i))
                            End If
                        Next

                        If path.Count > 0 Then
                            If debit > 0 Then
                                InsertPath(debitForest, path, debit)
                            End If
                            If credit > 0 Then
                                InsertPath(creditForest, path, credit)
                            End If
                        End If
                    End If
                Next

                SortForest(debitForest)
                SortForest(creditForest)

                Dim voucherActiveRows As New List(Of PrintableRow)()

                For Each root In debitForest
                    FlattenNode(root, voucherActiveRows, False, True)
                Next

                For Each root In creditForest
                    FlattenNode(root, voucherActiveRows, True, True)
                Next

                ' اضافه کردن به ردیف‌های دفتر روزنامه
                For Each vr In voucherActiveRows
                    rawRows.Add(New JournalRow() With {
                        .ReferenceNumber = doc.ReferenceNumber,
                        .EntryDate = doc.EntryDate,
                        .AccountCode = vr.AccountCode,
                        .AccountName = vr.AccountName,
                        .DebitAmount = vr.DebitAmount,
                        .CreditAmount = vr.CreditAmount,
                        .IsCredit = vr.IsCredit,
                        .IsDescriptionRow = False
                    })
                Next

                ' اضافه کردن سطر شرح سند
                If Not String.IsNullOrWhiteSpace(doc.Description) Then
                    rawRows.Add(New JournalRow() With {
                        .ReferenceNumber = doc.ReferenceNumber,
                        .EntryDate = doc.EntryDate,
                        .AccountName = doc.Description,
                        .IsDescriptionRow = True
                    })
                End If
            Next

            ' اجرای فرآیند صفحه‌بندی با محاسبه دقیق ظرفیت و تولید سطرهای انتقال مانده
            Dim currentIndex As Integer = 0
            Dim pageNum As Integer = 1
            Dim runningDebit As Decimal = 0
            Dim runningCredit As Decimal = 0

            While currentIndex < rawRows.Count
                Dim currentPageRows As New List(Of JournalRow)()
                Dim overhead As Integer = 0
                
                If pageNum > 1 Then overhead += 1 ' برای سطر "منقول از صفحه"
                overhead += 1 ' برای سطر پایانی ("نقل به صفحه" یا "جمع")

                Dim remainingRawCount = rawRows.Count - currentIndex
                Dim fitsOnThisPage As Boolean = (remainingRawCount + overhead <= PageCapacity)

                ' افزودن سطر "منقول از صفحه" در ابتدای صفحه
                If pageNum > 1 Then
                    currentPageRows.Add(New JournalRow() With {
                        .AccountName = "منقول از صفحه : " & (pageNum - 1),
                        .DebitAmount = runningDebit,
                        .CreditAmount = runningCredit,
                        .IsCarryFrom = True
                    })
                End If

                Dim rawRowsToTake As Integer = If(fitsOnThisPage, remainingRawCount, PageCapacity - overhead)

                For k As Integer = 0 To rawRowsToTake - 1
                    If currentIndex >= rawRows.Count Then Exit For
                    Dim r = rawRows(currentIndex)
                    currentPageRows.Add(r)

                    If r.DebitAmount.HasValue Then runningDebit += r.DebitAmount.Value
                    If r.CreditAmount.HasValue Then runningCredit += r.CreditAmount.Value

                    currentIndex += 1
                Next

                If fitsOnThisPage Then
                    ' آخرین صفحه -> افزودن ردیف‌های خالی جهت پر کردن صفحه تا انتهای فریم قبل از سطر جمع
                    Dim numEmptyRows = PageCapacity - 1 - currentPageRows.Count
                    For k As Integer = 1 To numEmptyRows
                        currentPageRows.Add(New JournalRow() With {
                            .ReferenceNumber = String.Empty,
                            .EntryDate = String.Empty,
                            .AccountCode = String.Empty,
                            .AccountName = String.Empty,
                            .DebitAmount = Nothing,
                            .CreditAmount = Nothing,
                            .IsCredit = False,
                            .IsDescriptionRow = False
                        })
                    Next

                    ' افزودن سطر جمع نهایی
                    currentPageRows.Add(New JournalRow() With {
                        .AccountName = "جمع : " & NumberToPersianWords(Convert.ToInt64(runningDebit)) & " ریال",
                        .DebitAmount = runningDebit,
                        .CreditAmount = runningCredit,
                        .IsTotalRow = True
                    })
                Else
                    ' صفحات میانی -> افزودن سطر نقل به صفحه بعد
                    currentPageRows.Add(New JournalRow() With {
                        .AccountName = "نقل به صفحه : " & (pageNum + 1),
                        .DebitAmount = runningDebit,
                        .CreditAmount = runningCredit,
                        .IsCarryForward = True
                    })
                End If

                _pages.Add(currentPageRows)
                pageNum += 1
            End While

            If _pages.Count = 0 Then
                Dim emptyPage As New List(Of JournalRow)()
                emptyPage.Add(New JournalRow() With {
                    .AccountName = "جمع : صفر ریال",
                    .DebitAmount = 0,
                    .CreditAmount = 0,
                    .IsTotalRow = True
                })
                _pages.Add(emptyPage)
            End If

            _totalPages = _pages.Count
            UpdateNavigationUI()
        End Sub

        ' ========================
        ' کدهای مربوط به جنگل درختی حساب‌ها (کپی شده از HesabdaryPrintForm)
        ' ========================

        Public Class VoucherAccountNode
            Public AccountCode As String
            Public AccountName As String
            Public LevelIndex As Integer
            Public Amount As Decimal
            Public Children As New List(Of VoucherAccountNode)()
        End Class

        Public Class PrintableRow
            Public AccountCode As String
            Public AccountName As String
            Public SubAmount As Decimal?
            Public DebitAmount As Decimal?
            Public CreditAmount As Decimal?
            Public IsCredit As Boolean
        End Class

        Private Sub InsertPath(forest As List(Of VoucherAccountNode), path As List(Of Tuple(Of String, String, Integer)), amount As Decimal)
            If path.Count = 0 Then Return
            
            Dim firstNode = path(0)
            Dim rootNode = forest.FirstOrDefault(Function(n) n.AccountCode = firstNode.Item1)
            If rootNode Is Nothing Then
                rootNode = New VoucherAccountNode() With {
                    .AccountCode = firstNode.Item1,
                    .AccountName = firstNode.Item2,
                    .LevelIndex = firstNode.Item3,
                    .Amount = 0D
                }
                forest.Add(rootNode)
            End If
            rootNode.Amount += amount
            
            Dim currentNode = rootNode
            For i = 1 To path.Count - 1
                Dim stepNode = path(i)
                Dim childNode = currentNode.Children.FirstOrDefault(Function(n) n.AccountCode = stepNode.Item1)
                If childNode Is Nothing Then
                    childNode = New VoucherAccountNode() With {
                        .AccountCode = stepNode.Item1,
                        .AccountName = stepNode.Item2,
                        .LevelIndex = stepNode.Item3,
                        .Amount = 0D
                    }
                    currentNode.Children.Add(childNode)
                End If
                childNode.Amount += amount
                currentNode = childNode
            Next
        End Sub

        Private Sub SortForest(forest As List(Of VoucherAccountNode))
            forest.Sort(Function(a, b) String.Compare(a.AccountCode, b.AccountCode, StringComparison.Ordinal))
            For Each node In forest
                SortForest(node.Children)
            Next
        End Sub

        Private Sub FlattenNode(node As VoucherAccountNode, rows As List(Of PrintableRow), isCredit As Boolean, isRoot As Boolean)
            Dim row As New PrintableRow() With {
                .AccountCode = node.AccountCode,
                .AccountName = node.AccountName,
                .IsCredit = isCredit
            }
            
            If isRoot Then
                If isCredit Then
                    row.CreditAmount = node.Amount
                Else
                    row.DebitAmount = node.Amount
                End If
            Else
                row.SubAmount = node.Amount
            End If
            
            rows.Add(row)
            
            For Each child In node.Children
                FlattenNode(child, rows, isCredit, False)
            Next
        End Sub

        Private Shared Function NumberToPersianWords(number As Long) As String
            If number = 0 Then Return "صفر"
            
            Dim yekan = New String() {"", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه"}
            Dim dahgan10 = New String() {"ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده"}
            Dim dahgan = New String() {"", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود"}
            Dim sadgan = New String() {"", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد"}
            Dim levels = New String() {"", "هزار", "میلیون", "میلیارد", "تریلیون"}

            If number < 0 Then
                Return "منفی " & NumberToPersianWords(Math.Abs(number))
            End If

            Dim result As New List(Of String)()
            Dim temp = number
            Dim levelIndex = 0

            While temp > 0
                Dim currentTriplet = CInt(temp Mod 1000)
                temp = temp \ 1000

                If currentTriplet > 0 Then
                    Dim tripletWord = TripletToWord(currentTriplet, yekan, dahgan10, dahgan, sadgan)
                    If levelIndex > 0 Then
                        tripletWord &= " " & levels(levelIndex)
                    End If
                    result.Insert(0, tripletWord)
                End If

                levelIndex += 1
            End While

            Return String.Join(" و ", result.ToArray())
        End Function

        Private Shared Function TripletToWord(num As Integer, yekan() As String, dahgan10() As String, dahgan() As String, sadgan() As String) As String
            Dim s = num \ 100
            Dim d = (num Mod 100) \ 10
            Dim y = num Mod 10

            Dim parts As New List(Of String)()

            If s > 0 Then parts.Add(sadgan(s))

            If d = 1 Then
                parts.Add(dahgan10(y))
            Else
                If d > 1 Then parts.Add(dahgan(d))
                If y > 0 Then parts.Add(yekan(y))
            End If

            Return String.Join(" و ", parts.ToArray())
        End Function

        ' ========================
        ' کدهای مربوط به ناوبری و زوم پیش‌نمایش (کپی شده از HesabdaryPrintForm)
        ' ========================

        Private Sub UpdateNavigationUI()
            btnFirstPage.Enabled = (previewCtrl.StartPage > 0)
            btnPrevPage.Enabled = (previewCtrl.StartPage > 0)
            btnNextPage.Enabled = (previewCtrl.StartPage < _totalPages - 1)
            btnLastPage.Enabled = (previewCtrl.StartPage < _totalPages - 1)

            lblPageStatus.Text = String.Format("صفحه {0} از {1}", previewCtrl.StartPage + 1, _totalPages)

            If previewCtrl.AutoZoom Then
                lblZoomValue.Text = "خودکار"
            Else
                lblZoomValue.Text = CInt(previewCtrl.Zoom * 100) & "%"
            End If
        End Sub

        Private Sub previewCtrl_StartPageChanged(sender As Object, e As EventArgs) Handles previewCtrl.StartPageChanged
            UpdateNavigationUI()
        End Sub

        Private Sub btnZoomIn_Click(sender As Object, e As EventArgs) Handles btnZoomIn.Click
            previewCtrl.AutoZoom = False
            previewCtrl.Zoom = Math.Min(3.0, previewCtrl.Zoom + 0.1)
            UpdateNavigationUI()
        End Sub

        Private Sub btnZoomOut_Click(sender As Object, e As EventArgs) Handles btnZoomOut.Click
            previewCtrl.AutoZoom = False
            previewCtrl.Zoom = Math.Max(0.1, previewCtrl.Zoom - 0.1)
            UpdateNavigationUI()
        End Sub

        Private Sub btnZoomFit_Click(sender As Object, e As EventArgs) Handles btnZoomFit.Click
            previewCtrl.AutoZoom = True
            UpdateNavigationUI()
        End Sub

        Private Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
            If previewCtrl.StartPage > 0 Then
                previewCtrl.StartPage = 0
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
            If previewCtrl.StartPage > 0 Then
                previewCtrl.StartPage -= 1
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
            If previewCtrl.StartPage < _totalPages - 1 Then
                previewCtrl.StartPage += 1
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
            If previewCtrl.StartPage < _totalPages - 1 Then
                previewCtrl.StartPage = _totalPages - 1
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub previewCtrl_MouseEnter(sender As Object, e As EventArgs) Handles previewCtrl.MouseEnter
            previewCtrl.Focus()
        End Sub

        Private Sub previewCtrl_MouseWheel(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseWheel
            Dim isCtrlPressed As Boolean = (Control.ModifierKeys And Keys.Control) = Keys.Control
            
            If isCtrlPressed Then
                previewCtrl.AutoZoom = False
                Dim zoomStep As Double = 0.05
                If e.Delta > 0 Then
                    previewCtrl.Zoom = Math.Min(3.0, previewCtrl.Zoom + zoomStep)
                Else
                    previewCtrl.Zoom = Math.Max(0.1, previewCtrl.Zoom - zoomStep)
                End If
                UpdateNavigationUI()
                Dim handeledArgs As HandledMouseEventArgs = TryCast(e, HandledMouseEventArgs)
                If handeledArgs IsNot Nothing Then
                    handeledArgs.Handled = True
                End If
            Else
                If previewCtrl.AutoZoom OrElse previewCtrl.Zoom <= 0.85 Then
                    If e.Delta < 0 Then
                        If previewCtrl.StartPage < _totalPages - 1 Then
                            previewCtrl.StartPage += 1
                            UpdateNavigationUI()
                        End If
                    Else
                        If previewCtrl.StartPage > 0 Then
                            previewCtrl.StartPage -= 1
                            UpdateNavigationUI()
                        End If
                    End If
                    Dim handeledArgs As HandledMouseEventArgs = TryCast(e, HandledMouseEventArgs)
                    If handeledArgs IsNot Nothing Then
                        handeledArgs.Handled = True
                    End If
                End If
            End If
        End Sub

        ' ========================
        ' کدهای مربوط به Panning با دکمه راست ماوس (کپی شده از HesabdaryPrintForm)
        ' ========================

        Private Sub previewCtrl_MouseDown(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseDown
            If e.Button = MouseButtons.Right Then
                If Not previewCtrl.AutoZoom AndAlso previewCtrl.Zoom > 0.3 Then
                    _isPanning = True
                    _startMousePos = e.Location
                    
                    Dim min As Integer, max As Integer, page As Integer
                    _startScrollPosX = GetScrollPos(SB_HORZ, min, max, page)
                    _startScrollPosY = GetScrollPos(SB_VERT, min, max, page)
                    
                    previewCtrl.Cursor = Cursors.Hand
                End If
            End If
        End Sub

        Private Sub previewCtrl_MouseMove(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseMove
            If _isPanning Then
                Dim deltaX = e.X - _startMousePos.X
                Dim deltaY = e.Y - _startMousePos.Y

                Dim minH As Integer, maxH As Integer, pageH As Integer
                GetScrollPos(SB_HORZ, minH, maxH, pageH)
                Dim limitH = Math.Max(0, maxH - pageH)

                Dim minV As Integer, maxV As Integer, pageV As Integer
                GetScrollPos(SB_VERT, minV, maxV, pageV)
                Dim limitV = Math.Max(0, maxV - pageV)

                Dim newScrollPosX = _startScrollPosX - deltaX
                Dim newScrollPosY = _startScrollPosY - deltaY

                newScrollPosX = Math.Max(0, Math.Min(newScrollPosX, limitH))
                newScrollPosY = Math.Max(0, Math.Min(newScrollPosY, limitV))

                SetScrollPos(SB_HORZ, newScrollPosX)
                SetScrollPos(SB_VERT, newScrollPosY)
            End If
        End Sub

        Private Sub previewCtrl_MouseUp(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseUp
            If e.Button = MouseButtons.Right Then
                _isPanning = False
                previewCtrl.Cursor = Cursors.Default
            End If
        End Sub

        <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
        Private Structure SCROLLINFO
            Public cbSize As Integer
            Public fMask As Integer
            Public nMin As Integer
            Public nMax As Integer
            Public nPage As Integer
            Public nPos As Integer
            Public nTrackPos As Integer
        End Structure

        Private Const SIF_RANGE As Integer = &H1
        Private Const SIF_PAGE As Integer = &H2
        Private Const SIF_POS As Integer = &H4
        Private Const SIF_TRACKPOS As Integer = &H10
        Private Const SIF_ALL As Integer = SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS

        Private Declare Function GetScrollInfo Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal fnBar As Integer, ByRef lpsi As SCROLLINFO) As Boolean
        Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

        Private Const SB_HORZ As Integer = 0
        Private Const SB_VERT As Integer = 1
        Private Const WM_HSCROLL As Integer = &H114
        Private Const WM_VSCROLL As Integer = &H115
        Private Const SB_THUMBPOSITION As Integer = 4
        Private Const SB_ENDSCROLL As Integer = 8

        Private Function GetScrollPos(fnBar As Integer, ByRef min As Integer, ByRef max As Integer, ByRef page As Integer) As Integer
            Dim si As New SCROLLINFO()
            si.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(si)
            si.fMask = SIF_ALL
            If GetScrollInfo(previewCtrl.Handle, fnBar, si) Then
                min = si.nMin
                max = si.nMax
                page = si.nPage
                Return si.nPos
            End If
            min = 0
            max = 0
            page = 0
            Return 0
        End Function

        Private Sub SetScrollPos(fnBar As Integer, pos As Integer)
            Dim msg As Integer = If(fnBar = SB_HORZ, WM_HSCROLL, WM_VSCROLL)
            Dim wParam As New IntPtr((SB_THUMBPOSITION And &HFFFF) Or (pos << 16))
            SendMessage(previewCtrl.Handle, msg, wParam, IntPtr.Zero)
            SendMessage(previewCtrl.Handle, msg, CType(SB_ENDSCROLL, IntPtr), IntPtr.Zero)
        End Sub
    End Class
End Namespace
