Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.Linq
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class HesabdaryPrintForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private ReadOnly compService As New CompanyFiscalYearService()

        ' داده‌های ورودی جهت چاپ
        Private ReadOnly _printableDocs As New List(Of PrintableDocument)()
        Private _fromRef As Integer? = Nothing
        Private _toRef As Integer? = Nothing
        Private _fromDate As String = String.Empty
        Private _toDate As String = String.Empty

        ' متغیرهای صفحه بندی و ترسیم
        Private _currentPageIndex As Integer = 0
        Private _totalPages As Integer = 1
        Private Const MaxRowsPerPage As Integer = 30

        ' متغیرهای کشیدن و حرکت (Panning) گزارش با کلیک راست
        Private _isPanning As Boolean = False
        Private _startMousePos As Point
        Private _startScrollPosX As Integer
        Private _startScrollPosY As Integer

        ' کش لوگوی شرکت و امضاها
        Private _logoImage As Image = Nothing
        Private _logoPosition As String = "Left"
        Private _sig1Title As String = "تهیه کننده"
        Private _sig1Name As String = ""
        Private _sig2Title As String = "تأیید کننده"
        Private _sig2Name As String = ""
        Private _sig3Title As String = "تصویب کننده"
        Private _sig3Name As String = ""

        Public Class PrintableDocument
            Public Property EntryID As Integer
            Public Property ReferenceNumber As String
            Public Property EntryDate As String
            Public Property Description As String
            Public Property RawLinesTable As DataTable
            Public Property ActiveRows As New List(Of PrintableRow)()
            Public Property TotalPages As Integer = 1
        End Class

        Public Sub New(refNo As String, dateSanad As String, description As String, linesTable As DataTable)
            InitializeComponent()
            Dim doc As New PrintableDocument() With {
                .ReferenceNumber = refNo,
                .EntryDate = dateSanad,
                .Description = description,
                .RawLinesTable = linesTable
            }
            _printableDocs.Add(doc)
        End Sub

        Public Sub New(fromRef As Integer?, toRef As Integer?, fromDate As String, toDate As String)
            InitializeComponent()
            _fromRef = fromRef
            _toRef = toRef
            _fromDate = fromDate
            _toDate = toDate
        End Sub

        Private Sub LoadBatchDocuments(Optional progress As ProgressForm = Nothing)
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
                        progress.UpdateProgress(percent, "بارگذاری جزئیات سند شماره " & refNo & " (" & current & " از " & total & ")...")
                    End If

                    ' قالب بندی تاریخ سند
                    Dim dateVal = row("EntryDate")
                    Dim dateStr As String = ""
                    If dateVal IsNot Nothing AndAlso dateVal IsNot DBNull.Value Then
                        dateStr = PersianDateHelper.FormatDateTime(Convert.ToDateTime(dateVal))
                    End If

                    Dim desc = Convert.ToString(row("Description"))
                    Dim linesTable = service.GetEntryDetails(entryId)

                    Dim doc As New PrintableDocument() With {
                        .EntryID = entryId,
                        .ReferenceNumber = refNo,
                        .EntryDate = dateStr,
                        .Description = desc,
                        .RawLinesTable = linesTable
                    }
                    _printableDocs.Add(doc)
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در واکشی اسناد جهت چاپ: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub HesabdaryPrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
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

                progress.UpdateProgress(15, "بارگذاری مشخصات لوگو و امضاها...")
                ' ۲. واکشی لوگو و مشخصات امضاها از دیتابیس
                LoadCompanyLogoAndSignatures()

                progress.UpdateProgress(18, "تنظیم سطوح و دسترسی‌های سرفصل‌ها...")
                ' ۳. فعال‌سازی چک‌باکس‌های سطوح حساب بر اساس تنظیمات شرکت
                SetupLevelCheckboxes()

                ' بارگذاری دسته‌ای اسناد در صورت لزوم
                If _printableDocs.Count = 0 Then
                    LoadBatchDocuments(progress)
                End If

                progress.UpdateProgress(70, "بازسازی و صفحه‌بندی آرتیکل‌های اسناد...")
                ' ۴. بازسازی ردیف‌های چاپی
                RebuildActiveRows(progress)

                progress.UpdateProgress(95, "آماده‌سازی نهایی پیش‌نمایش چاپ...")
                ' ۵. تنظیم اولیه سند جهت چاپ A4 عمودی
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169) ' A4 size in 100ths of inch
                printDoc.DefaultPageSettings.Margins = New Margins(50, 50, 50, 50) ' 0.5 inch margins

                ' به روز رسانی زنده پیش‌نمایش
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()

                progress.UpdateProgress(100, "پیش‌نمایش با موفقیت آماده شد")
            End Using
        End Sub

        Private Sub LoadCompanyLogoAndSignatures()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim dt = Negar.Data.Sql.ExecuteTable("SELECT LogoImage, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, LogoPosition FROM Companies WHERE CompanyID = ?", companyId)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)

                    ' لوگو
                    If Not row.IsNull("LogoImage") Then
                        Dim bytes = DirectCast(row("LogoImage"), Byte())
                        If bytes.Length > 0 Then
                            Using ms As New IO.MemoryStream(bytes)
                                _logoImage = Image.FromStream(ms)
                            End Using
                        End If
                    End If

                    ' امضاها
                    If Not row.IsNull("Signatory1Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory1Title").ToString()) Then _sig1Title = row("Signatory1Title").ToString()
                    If Not row.IsNull("Signatory1Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory1Name").ToString()) Then _sig1Name = row("Signatory1Name").ToString()

                    If Not row.IsNull("Signatory2Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory2Title").ToString()) Then _sig2Title = row("Signatory2Title").ToString()
                    If Not row.IsNull("Signatory2Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory2Name").ToString()) Then _sig2Name = row("Signatory2Name").ToString()

                    If Not row.IsNull("Signatory3Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory3Title").ToString()) Then _sig3Title = row("Signatory3Title").ToString()
                    If Not row.IsNull("Signatory3Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory3Name").ToString()) Then _sig3Name = row("Signatory3Name").ToString()

                    ' محل لوگو
                    If Not row.IsNull("LogoPosition") Then
                        _logoPosition = row("LogoPosition").ToString()
                    End If
                End If
            Catch ex As Exception
                ' نادیده گرفتن خطا و استفاده از مقادیر پیش‌فرض
            End Try
        End Sub

        Private Sub SetupLevelCheckboxes()
            Dim settings = service.GetCompanyAccountSettings()
            Dim maxLevels = settings.Item1

            ' غیرفعال کردن چک‌باکس‌های غیرمجاز
            chkGroup.Enabled = (maxLevels >= 1)
            chkGeneral.Enabled = (maxLevels >= 2)
            chkSubsidiary.Enabled = (maxLevels >= 3)
            chkDetail1.Enabled = (maxLevels >= 4)
            chkDetail2.Enabled = (maxLevels >= 5)

            ' انتخاب پیش‌فرض سطوح مجاز
            chkGroup.Checked = chkGroup.Enabled
            chkGeneral.Checked = chkGeneral.Enabled
            chkSubsidiary.Checked = chkSubsidiary.Enabled
            chkDetail1.Checked = chkDetail1.Enabled
            chkDetail2.Checked = chkDetail2.Enabled

            AddHandler btnReload.Click, Sub(s, ev)
                                            Using progress As New ProgressForm()
                                                progress.ShowAndCenter(Me)
                                                progress.UpdateProgress(10, "در حال بازخوانی سرفصل‌ها و بازسازی صفحات...")
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
                MessageBox.Show("سند حسابداری با موفقیت به چاپگر ارسال شد.", "چاپ موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در چاپ سند: " & ex.Message, "خطا در چاپ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        ' ========================
        ' رویداد ترسیم زنده صفحات چاپ سند
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

            ' پیدا کردن سند و صفحه محلی آن با استفاده از _currentPageIndex
            Dim pageCounter As Integer = 0
            Dim targetDoc As PrintableDocument = Nothing
            Dim docPageIndex As Integer = 0

            For Each doc In _printableDocs
                If _currentPageIndex >= pageCounter AndAlso _currentPageIndex < pageCounter + doc.TotalPages Then
                    targetDoc = doc
                    docPageIndex = _currentPageIndex - pageCounter
                    Exit For
                End If
                pageCounter += doc.TotalPages
            Next

            If targetDoc Is Nothing Then
                If _printableDocs.Count > 0 Then
                    targetDoc = _printableDocs(0)
                Else
                    targetDoc = New PrintableDocument() With {
                        .ReferenceNumber = "-",
                        .EntryDate = "-",
                        .Description = "-",
                        .ActiveRows = New List(Of PrintableRow)()
                    }
                End If
                docPageIndex = 0
            End If

            ' ۱. ترسیم کادر دور صفحه
            Using pBorder As New Pen(Color.Black, 2.0!)
                g.DrawRectangle(pBorder, leftX, topY, pageWidth, pageHeight)
            End Using

            ' ۲. هدر: لوگوی شرکت
            If _logoImage IsNot Nothing Then
                If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                    g.DrawImage(_logoImage, rightX - 75, topY + 5, 60, 60)
                Else
                    g.DrawImage(_logoImage, leftX + 15, topY + 5, 60, 60)
                End If
            End If

            ' هدر: عنوان وسط صفحه
            Dim fTitle As New Font("Tahoma", 15.0!, FontStyle.Bold)
            Dim fSubTitle As New Font("Tahoma", 12.0!, FontStyle.Bold)
            Dim fRegular As New Font("Tahoma", 9.0!, FontStyle.Regular)
            Dim fBold As New Font("Tahoma", 9.0!, FontStyle.Bold)

            Dim companyName = "شرکت " & SessionContext.CurrentCompanyName
            Dim titleText = "سند حسابداری"

            Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center}
            Dim sfRight As New StringFormat() With {.Alignment = System.Drawing.StringAlignment.Far}
            Dim sfLeft As New StringFormat() With {.Alignment = System.Drawing.StringAlignment.Near}

            ' محاسبه عرض صفحه و فضای یک سوم جهت شرکت و عنوان
            Dim compWidth = pageWidth \ 3
            Dim sizeCompName = g.MeasureString(companyName, fTitle, compWidth)
            Dim rectCompName As New Rectangle(leftX + compWidth, topY + 15, compWidth, CInt(sizeCompName.Height) + 5)

            Using brMaroon As New SolidBrush(Color.FromArgb(160, 0, 0))
                g.DrawString(companyName, fTitle, brMaroon, rectCompName, sfCenter)

                Dim titleY = rectCompName.Bottom + 5
                g.DrawString(titleText, fSubTitle, brMaroon, leftX + (pageWidth \ 2), titleY, sfCenter)
            End Using

            ' هدر: شماره و تاریخ (همیشه در سمت راست)
            If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                ' اگر آرم سمت راست بود، شماره و تاریخ زیر آرم قرار می‌گیرد
                g.DrawString("شماره : " & targetDoc.ReferenceNumber, fBold, Brushes.Black, rightX - 15, topY + 68, sfRight)
                g.DrawString("تاریخ : " & targetDoc.EntryDate, fBold, Brushes.Black, rightX - 15, topY + 86, sfRight)
            Else
                ' اگر آرم سمت چپ بود، شماره و تاریخ در موقعیت پیش‌فرض بالا سمت راست قرار می‌گیرد
                g.DrawString("شماره : " & targetDoc.ReferenceNumber, fBold, Brushes.Black, rightX - 15, topY + 20, sfRight)
                g.DrawString("تاریخ : " & targetDoc.EntryDate, fBold, Brushes.Black, rightX - 15, topY + 45, sfRight)
            End If

            ' ۳. ترسیم ساختار جدول آرتیکل‌ها
            Dim tableStartY = topY + 110
            Dim rowHeight = 25
            Dim headerHeight = 30
            Dim tableEndY = tableStartY + headerHeight + (MaxRowsPerPage * rowHeight)

            ' ستون‌ها از راست به چپ:
            ' کد حساب (۱۲۰ پیکسل)
            ' شرح (۲۶۷ پیکسل)
            ' مبلغ جزء (۱۰۰ پیکسل)
            ' بدهکار (۱۲۰ پیکسل)
            ' بستانکار (۱۲۰ پیکسل)
            Dim colWidths = New Integer() {120, 267, 100, 120, 120}
            Dim colX = New Integer(5) {}
            colX(0) = rightX ' شروع ستون کد حساب از راست
            colX(1) = colX(0) - colWidths(0)
            colX(2) = colX(1) - colWidths(1)
            colX(3) = colX(2) - colWidths(2)
            colX(4) = colX(3) - colWidths(3)
            colX(5) = leftX ' ستون بستانکار به لبه چپ متصل می‌شود

            ' هدر جدول (آبی فیروزه‌ای ملایم مطابق تصویر)
            Dim rectHeader As New Rectangle(leftX, tableStartY, pageWidth, headerHeight)
            Using brHeader As New SolidBrush(Color.FromArgb(210, 236, 245))
                g.FillRectangle(brHeader, rectHeader)
            End Using
            g.DrawRectangle(Pens.Black, rectHeader)

            Dim headers = New String() {"کد حساب", "شــــــــرح", "مبلغ جزء", "بدهکار", "بستانکار"}
            For i = 0 To 4
                Dim rectColHeader As New Rectangle(colX(i + 1), tableStartY, colWidths(i), headerHeight)
                g.DrawRectangle(Pens.Black, rectColHeader)

                Dim sfCol As New StringFormat() With {
                    .Alignment = StringAlignment.Center,
                    .LineAlignment = StringAlignment.Center
                }
                g.DrawString(headers(i), fBold, Brushes.Black, rectColHeader, sfCol)
            Next

            ' ۴. رسم سطرها (ردیف‌های فعال سند)
            Dim startRowIndex = docPageIndex * MaxRowsPerPage
            Dim endRowIndex = Math.Min(startRowIndex + MaxRowsPerPage, targetDoc.ActiveRows.Count)
            Dim currY = tableStartY + headerHeight

            Dim sumDebit As Decimal = 0
            Dim sumCredit As Decimal = 0

            ' محاسبه مجموع کل برای سند (بدون محدودیت صفحه) جهت درج در آخرین صفحه
            For Each r In targetDoc.ActiveRows
                If r.DebitAmount.HasValue Then sumDebit += r.DebitAmount.Value
                If r.CreditAmount.HasValue Then sumCredit += r.CreditAmount.Value
            Next

            Dim rowCount = endRowIndex - startRowIndex
            For i = startRowIndex To endRowIndex - 1
                Dim dr = targetDoc.ActiveRows(i)
                Dim rectRow = New Rectangle(leftX, currY, pageWidth, rowHeight)

                ' ترسیم متون سلول‌ها با تراز مناسب
                Dim sfTextCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                Dim sfTextRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

                ' کد حساب (تراز وسط)
                Dim rCode = New Rectangle(colX(1) + 5, currY, colWidths(0) - 10, rowHeight)
                g.DrawString(dr.AccountCode, fRegular, Brushes.Black, rCode, sfTextCenter)

                ' شرح (نام سرفصل)
                If dr.IsCredit Then
                    ' یک سوم اول خالی، دو سوم بعدی راست‌چین
                    Dim rDescCredit = New Rectangle(colX(2) + 5, currY, (colWidths(1) * 2 \ 3) - 10, rowHeight)
                    g.DrawString(dr.AccountName, fRegular, Brushes.Black, rDescCredit, sfTextRight)
                Else
                    Dim rDescDebit = New Rectangle(colX(2) + 5, currY, colWidths(1) - 10, rowHeight)
                    g.DrawString(dr.AccountName, fRegular, Brushes.Black, rDescDebit, sfTextRight)
                End If

                ' مبلغ جزء (راست‌چین)
                If dr.SubAmount.HasValue Then
                    Dim rSub = New Rectangle(colX(3) + 5, currY, colWidths(2) - 10, rowHeight)
                    g.DrawString(dr.SubAmount.Value.ToString("N0"), fRegular, Brushes.Black, rSub, sfTextRight)
                End If

                ' بدهکار (راست‌چین)
                If dr.DebitAmount.HasValue Then
                    Dim rDeb = New Rectangle(colX(4) + 5, currY, colWidths(3) - 10, rowHeight)
                    g.DrawString(dr.DebitAmount.Value.ToString("N0"), fRegular, Brushes.Black, rDeb, sfTextRight)
                End If

                ' بستانکار (راست‌چین)
                If dr.CreditAmount.HasValue Then
                    Dim rCred = New Rectangle(colX(5) + 5, currY, colWidths(4) - 10, rowHeight)
                    g.DrawString(dr.CreditAmount.Value.ToString("N0"), fRegular, Brushes.Black, rCred, sfTextRight)
                End If

                ' رسم خط نقطه‌چین پایین سطر (طرح تصویر نمونه)
                Using pDot As New Pen(Color.LightGray) With {.DashStyle = DashStyle.Dot}
                    g.DrawLine(pDot, leftX, currY + rowHeight, rightX, currY + rowHeight)
                End Using

                currY += rowHeight
            Next

            ' ۵. امتداد خطوط افقی و عمودی جدول تا انتهای فریم
            ' رسم خطوط افقی سطرها تا پایین فریم
            Dim remainingY = currY
            Do While remainingY < tableEndY
                Using pDot As New Pen(Color.LightGray) With {.DashStyle = DashStyle.Dot}
                    g.DrawLine(pDot, leftX, remainingY, rightX, remainingY)
                End Using
                remainingY += rowHeight
            Loop

            ' رسم خطوط عمودی ستون‌ها از سرستون تا انتهای جدول
            For i = 0 To 5
                g.DrawLine(Pens.Black, colX(i), tableStartY, colX(i), tableEndY)
            Next
            g.DrawLine(Pens.Black, leftX, tableEndY, rightX, tableEndY)

            ' ترسیم خط افقی و مورب مسدودکننده فضای خالی در صفحه آخر در صورت نیمه‌خالی بودن
            Dim isLastPage = (docPageIndex = targetDoc.TotalPages - 1)
            If isLastPage AndAlso currY < tableEndY Then
                g.DrawLine(Pens.Black, colX(3), currY, colX(0), currY)
                g.DrawLine(Pens.Black, colX(3), currY, colX(5), tableEndY)
            End If

            ' ۶. ترسیم باکس جمع کل، شرح سند و امضاها در صفحه آخر
            If isLastPage Then
                ' باکس جمع کل (زرد لیمویی ملایم مطابق تصویر)
                Dim totalsHeight = 30
                Dim rectTotals = New Rectangle(leftX, tableEndY, pageWidth, totalsHeight)
                Using brTotals As New SolidBrush(Color.FromArgb(254, 248, 165))
                    g.FillRectangle(brTotals, rectTotals)
                End Using
                g.DrawRectangle(Pens.Black, rectTotals)

                Dim sfTotalsRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                Dim sfTotalsWords As New StringFormat() With {
                    .Alignment = StringAlignment.Near,
                    .LineAlignment = StringAlignment.Center,
                    .FormatFlags = StringFormatFlags.DirectionRightToLeft
                }

                ' متن جمع کل به حروف
                Dim totalsTextWidth = colWidths(0) + colWidths(1) + colWidths(2)
                Dim rDescTot = New Rectangle(colX(3) + 5, tableEndY, totalsTextWidth - 10, totalsHeight)
                Dim sumWords = "جمع :"
                If sumDebit > 0 Then
                    sumWords &= " " & NumberToPersianWords(Convert.ToInt64(sumDebit)) & " ریال"
                End If
                g.DrawString(sumWords, fBold, Brushes.Black, rDescTot, sfTotalsWords)

                ' مجموع بدهکار (راست‌چین)
                Dim rDebTot = New Rectangle(colX(4) + 5, tableEndY, colWidths(3) - 10, totalsHeight)
                g.DrawString(sumDebit.ToString("N0"), fBold, Brushes.Black, rDebTot, sfTotalsRight)

                ' مجموع بستانکار (راست‌چین)
                Dim rCredTot = New Rectangle(colX(5) + 5, tableEndY, colWidths(4) - 10, totalsHeight)
                g.DrawString(sumCredit.ToString("N0"), fBold, Brushes.Black, rCredTot, sfTotalsRight)

                ' ترسیم خطوط عمودی جداکننده جمع
                g.DrawLine(Pens.Black, colX(3), tableEndY, colX(3), tableEndY + totalsHeight)
                g.DrawLine(Pens.Black, colX(4), tableEndY, colX(4), tableEndY + totalsHeight)

                ' باکس شرح سند (فیروزه‌ای ملایم مطابق تصویر)
                Dim descBoxHeight = 45
                Dim rectDesc = New Rectangle(leftX, tableEndY + totalsHeight, pageWidth, descBoxHeight)
                Using brDesc As New SolidBrush(Color.FromArgb(210, 236, 245))
                    g.FillRectangle(brDesc, rectDesc)
                End Using
                g.DrawRectangle(Pens.Black, rectDesc)

                Dim rDescText = New Rectangle(leftX + 10, tableEndY + totalsHeight + 5, pageWidth - 20, descBoxHeight - 10)
                g.DrawString("شرح: " & targetDoc.Description, fBold, Brushes.Black, rDescText, sfRight)

                ' بخش امضاها (مطابق نمونه تصویر: تهیه کننده / تأیید کننده / تصویب کننده)
                Dim sigY = tableEndY + totalsHeight + descBoxHeight + 15
                Dim sigColWidth = pageWidth \ 3

                ' تهیه کننده (راست)
                Dim rectSig1 = New Rectangle(rightX - sigColWidth, sigY, sigColWidth, 40)
                Dim sig1Text = If(String.IsNullOrWhiteSpace(_sig1Name), "تهیه کننده:", "تهیه کننده: " & _sig1Name)
                g.DrawString(sig1Text, fBold, Brushes.Black, rectSig1, sfCenter)

                ' تأیید کننده (وسط)
                Dim rectSig2 = New Rectangle(rightX - (sigColWidth * 2), sigY, sigColWidth, 40)
                Dim sig2Text = If(String.IsNullOrWhiteSpace(_sig2Name), "تأیید کننده:", "تأیید کننده: " & _sig2Name)
                g.DrawString(sig2Text, fBold, Brushes.Black, rectSig2, sfCenter)

                ' تصویب کننده (چپ)
                Dim rectSig3 = New Rectangle(leftX, sigY, sigColWidth, 40)
                Dim sig3Text = If(String.IsNullOrWhiteSpace(_sig3Name), "تصویب کننده:", "تصویب کننده: " & _sig3Name)
                g.DrawString(sig3Text, fBold, Brushes.Black, rectSig3, sfCenter)
            Else
                ' در صفحات میانی، پیامی درج می‌کنیم که ادامه در صفحه بعد است
                Dim rectNextPage = New Rectangle(leftX, tableEndY, pageWidth, 30)
                g.DrawRectangle(Pens.Black, rectNextPage)
                g.DrawString("ادامه در صفحه بعد...", fBold, Brushes.DarkGray, rectNextPage, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            End If

            ' ۷. ترسیم شماره صفحه در پایین برگ
            Dim pageNoStr = String.Format("صفحه : {0} از {1}", docPageIndex + 1, targetDoc.TotalPages)
            g.DrawString(pageNoStr, fRegular, Brushes.Black, leftX + (pageWidth \ 2), bottomY - 25, sfCenter)

            ' مدیریت فرآیند چند صفحه‌ای بودن
            _currentPageIndex += 1
            If _currentPageIndex < _totalPages Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                _currentPageIndex = 0 ' ریست برای چاپ‌های بعدی
            End If
        End Sub

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

        Private Sub RebuildActiveRows(Optional progress As ProgressForm = Nothing)
            _totalPages = 0
            Dim hierarchyCache As New Dictionary(Of Integer, List(Of Tuple(Of String, String)))()

            Dim totalDocs = _printableDocs.Count
            Dim currentDocIndex = 0

            For Each doc In _printableDocs
                currentDocIndex += 1
                If progress IsNot Nothing Then
                    Dim percent = 70 + CInt((currentDocIndex / totalDocs) * 25) ' 70% to 95%
                    progress.UpdateProgress(percent, "صفحه‌بندی و تحلیل ساختاری سند شماره " & doc.ReferenceNumber & " (" & currentDocIndex & " از " & totalDocs & ")...")
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

                Dim newActiveRows As New List(Of PrintableRow)()

                For Each root In debitForest
                    FlattenNode(root, newActiveRows, False, True)
                Next

                For Each root In creditForest
                    FlattenNode(root, newActiveRows, True, True)
                Next

                doc.ActiveRows = newActiveRows
                doc.TotalPages = Math.Max(1, CInt(Math.Ceiling(doc.ActiveRows.Count / MaxRowsPerPage)))
                _totalPages += doc.TotalPages
            Next

            If _totalPages = 0 Then _totalPages = 1
            UpdateNavigationUI()
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

        Private Sub UpdateNavigationUI()
            ' تحدیث وضعیت دکمه‌های ناوبری صفحات
            btnFirstPage.Enabled = (previewCtrl.StartPage > 0)
            btnPrevPage.Enabled = (previewCtrl.StartPage > 0)
            btnNextPage.Enabled = (previewCtrl.StartPage < _totalPages - 1)
            btnLastPage.Enabled = (previewCtrl.StartPage < _totalPages - 1)

            lblPageStatus.Text = String.Format("صفحه {0} از {1}", previewCtrl.StartPage + 1, _totalPages)

            ' تحدیث وضعیت زوم
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
                ' زوم کردن (بزرگنمایی / کوچک‌نمایی)
                previewCtrl.AutoZoom = False
                Dim zoomStep As Double = 0.05
                If e.Delta > 0 Then
                    previewCtrl.Zoom = Math.Min(3.0, previewCtrl.Zoom + zoomStep)
                Else
                    previewCtrl.Zoom = Math.Max(0.1, previewCtrl.Zoom - zoomStep)
                End If
                UpdateNavigationUI()
                ' جلوگیری از اسکرول عادی
                Dim handeledArgs As HandledMouseEventArgs = TryCast(e, HandledMouseEventArgs)
                If handeledArgs IsNot Nothing Then
                    handeledArgs.Handled = True
                End If
            Else
                ' ناوبری صفحات فقط در صورتی که کل صفحه در کادر جا شده باشد (زوم کمتر یا مساوی 0.85 یا خودکار)
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
                    ' جلوگیری از اسکرول عادی
                    Dim handeledArgs As HandledMouseEventArgs = TryCast(e, HandledMouseEventArgs)
                    If handeledArgs IsNot Nothing Then
                        handeledArgs.Handled = True
                    End If
                End If
            End If
        End Sub

        Private Sub chkGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroup.CheckedChanged

        End Sub

        ' ==========================================
        ' رویدادهای کلیک راست و حرکت (Panning) گزارش
        ' ==========================================
        Private Sub previewCtrl_MouseDown(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseDown
            If e.Button = MouseButtons.Right Then
                ' فقط در صورتی که زوم شده باشد یا صفحه بزرگتر از کادر باشد
                If Not previewCtrl.AutoZoom AndAlso previewCtrl.Zoom > 0.3 Then
                    _isPanning = True
                    _startMousePos = e.Location

                    Dim min As Integer, max As Integer, page As Integer
                    _startScrollPosX = GetScrollPos(SB_HORZ, min, max, page)
                    _startScrollPosY = GetScrollPos(SB_VERT, min, max, page)

                    ' تغییر شکل کرسر به دست (Grab) جهت حس بصری بهتر
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
                ' حد بالا اسکرول افقی maxH - pageH است
                Dim limitH = Math.Max(0, maxH - pageH)

                Dim minV As Integer, maxV As Integer, pageV As Integer
                GetScrollPos(SB_VERT, minV, maxV, pageV)
                ' حد بالا اسکرول عمودی maxV - pageV است
                Dim limitV = Math.Max(0, maxV - pageV)

                ' محاسبه موقعیت جدید (حرکت دست به راست سند را به چپ هدایت می‌کند)
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

        ' ==========================================
        ' توابع P/Invoke جهت اسکرول دستی و حرکت (Panning) گزارش
        ' ==========================================
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
            ' High-order 16 bits = position, Low-order 16 bits = SB_THUMBPOSITION
            Dim wParam As New IntPtr((SB_THUMBPOSITION And &HFFFF) Or (pos << 16))
            SendMessage(previewCtrl.Handle, msg, wParam, IntPtr.Zero)
            ' Send SB_ENDSCROLL
            SendMessage(previewCtrl.Handle, msg, CType(SB_ENDSCROLL, IntPtr), IntPtr.Zero)
        End Sub

        Private Sub chkDetail1_CheckedChanged(sender As Object, e As EventArgs) Handles chkDetail1.CheckedChanged

        End Sub
    End Class
End Namespace
