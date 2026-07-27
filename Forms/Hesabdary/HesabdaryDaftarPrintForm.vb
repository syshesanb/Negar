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
    Public Class LedgerPrintBlock
        Public Property LedgerTitle As String = String.Empty
        Public Property AccountNameTitle As String = String.Empty
        Public Property AccountHierarchyChain As List(Of Tuple(Of String, String)) = Nothing
        Public Property Rows As New List(Of HesabdaryDaftarPrintForm.LedgerRowInfo)()
        Public Property TotalDebit As Decimal = 0D
        Public Property TotalCredit As Decimal = 0D
        Public Property TotalBalance As Decimal? = Nothing
        Public Property TotalTashkhis As String = String.Empty
    End Class

    Public Structure PrintPageInfo
        Public Block As LedgerPrintBlock
        Public Rows As List(Of HesabdaryDaftarPrintForm.LedgerRowInfo)
        Public PageNumberInBlock As Integer
        Public TotalPagesInBlock As Integer
    End Structure

    Public Class HesabdaryDaftarPrintForm
        Inherits Form

        Private ReadOnly service As New AccountingService()

        ' داده‌های ورودی گزارش
        Private ReadOnly _blocks As New List(Of LedgerPrintBlock)()

        ' مدیریت صفحه‌بندی
        Private _pages As New List(Of PrintPageInfo)()
        Private _currentPageIndex As Integer = 0
        Private _totalPages As Integer = 1

        ' کش لوگوی شرکت و امضاها
        Private _logoImage As Image = Nothing
        Private _logoPosition As String = "Left"
        Private _sig1Title As String = "تهیه کننده"
        Private _sig1Name As String = ""
        Private _sig2Title As String = "تأیید کننده"
        Private _sig2Name As String = ""
        Private _sig3Title As String = "تصویب کننده"
        Private _sig3Name As String = ""

        ' متغیرهای پنینگ (Panning) با کلیک راست
        Private _isPanning As Boolean = False
        Private _startMousePos As Point
        Private _startScrollPosX As Integer
        Private _startScrollPosY As Integer

        Private Structure ColDef
            Public Key As String
            Public Title As String
            Public Ratio As Single
        End Structure

        Public Class LedgerRowInfo
            Public Property RefNo As String = String.Empty
            Public Property EntryDate As String = String.Empty
            Public Property Description As String = String.Empty
            Public Property DebitAmount As Decimal? = Nothing
            Public Property CreditAmount As Decimal? = Nothing
            Public Property Tashkhis As String = String.Empty
            Public Property BalanceAmount As Decimal? = Nothing
            Public Property IsHeader As Boolean = False
            Public Property IsSummary As Boolean = False
        End Class

        Public Sub New(ledgerTitle As String, accountNameTitle As String, rows As List(Of LedgerRowInfo), totalDebit As Decimal, totalCredit As Decimal, Optional totalBalance As Decimal? = Nothing, Optional totalTashkhis As String = "")
            InitializeComponent()
            Dim block As New LedgerPrintBlock() With {
                .LedgerTitle = If(String.IsNullOrWhiteSpace(ledgerTitle), "دفتر حساب", ledgerTitle),
                .AccountNameTitle = accountNameTitle,
                .Rows = rows,
                .TotalDebit = totalDebit,
                .TotalCredit = totalCredit,
                .TotalBalance = totalBalance,
                .TotalTashkhis = totalTashkhis
            }
            _blocks.Add(block)
        End Sub

        Public Sub New(blocks As List(Of LedgerPrintBlock))
            InitializeComponent()
            _blocks = blocks
        End Sub

        Private Sub HesabdaryDaftarPrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "بارگذاری مشخصات چاپگرها...")

                ' ۱. لود لیست چاپگرها
                cmbPrinter.Items.Clear()
                For Each prt As String In PrinterSettings.InstalledPrinters
                    cmbPrinter.Items.Add(prt)
                Next
                Dim defaultPrinter = printDoc.PrinterSettings.PrinterName
                If cmbPrinter.Items.Contains(defaultPrinter) Then
                    cmbPrinter.SelectedItem = defaultPrinter
                ElseIf cmbPrinter.Items.Count > 0 Then
                    cmbPrinter.SelectedIndex = 0
                End If

                progress.UpdateProgress(20, "بارگذاری لوگو و امضاداران...")
                LoadCompanyLogoAndSignatures()

                progress.UpdateProgress(50, "محاسبه ساختار صفحات و صفحه‌بندی...")
                ' تنظیمات اولیه A4 عمودی
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
                printDoc.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)
                printDoc.DefaultPageSettings.Landscape = False

                CalculatePages()

                progress.UpdateProgress(90, "آماده‌سازی پیش‌نمایش چاپ...")
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()

                progress.UpdateProgress(100, "پیش‌نمایش دفتر حساب آماده شد")
            End Using
        End Sub

        Private Sub LoadCompanyLogoAndSignatures()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim dt = Sql.ExecuteTable("SELECT LogoImage, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, LogoPosition FROM Companies WHERE CompanyID = ?", companyId)
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

                    If Not row.IsNull("Signatory1Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory1Title").ToString()) Then _sig1Title = row("Signatory1Title").ToString()
                    If Not row.IsNull("Signatory1Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory1Name").ToString()) Then _sig1Name = row("Signatory1Name").ToString()
                    If Not row.IsNull("Signatory2Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory2Title").ToString()) Then _sig2Title = row("Signatory2Title").ToString()
                    If Not row.IsNull("Signatory2Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory2Name").ToString()) Then _sig2Name = row("Signatory2Name").ToString()
                    If Not row.IsNull("Signatory3Title") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory3Title").ToString()) Then _sig3Title = row("Signatory3Title").ToString()
                    If Not row.IsNull("Signatory3Name") AndAlso Not String.IsNullOrWhiteSpace(row("Signatory3Name").ToString()) Then _sig3Name = row("Signatory3Name").ToString()
                    If Not row.IsNull("LogoPosition") Then
                        _logoPosition = row("LogoPosition").ToString()
                    End If
                End If
            Catch
            End Try
        End Sub

        Private Sub CalculatePages()
            _pages.Clear()
            If _blocks.Count = 0 Then
                _totalPages = 1
                Return
            End If

            Dim rowsPerPage As Integer = If(printDoc.DefaultPageSettings.Landscape, 20, 30)

            For Each block In _blocks
                Dim blockPages As New List(Of List(Of LedgerRowInfo))()
                If block.Rows.Count = 0 Then
                    Dim currentPage As New List(Of LedgerRowInfo)()
                    blockPages.Add(currentPage)
                Else
                    Dim currentPage As New List(Of LedgerRowInfo)()
                    For Each r In block.Rows
                        currentPage.Add(r)
                        If currentPage.Count >= rowsPerPage Then
                            blockPages.Add(currentPage)
                            currentPage = New List(Of LedgerRowInfo)()
                        End If
                    Next
                    If currentPage.Count > 0 Then
                        blockPages.Add(currentPage)
                    End If
                End If

                Dim pageNum = 1
                For Each pRows In blockPages
                    Dim pageInfo As New PrintPageInfo() With {
                        .Block = block,
                        .Rows = pRows,
                        .PageNumberInBlock = pageNum,
                        .TotalPagesInBlock = blockPages.Count
                    }
                    _pages.Add(pageInfo)
                    pageNum += 1
                Next
            Next

            _totalPages = _pages.Count
            If previewCtrl IsNot Nothing Then
                previewCtrl.StartPage = 0
            End If
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

        Private Sub btnOrientation_Click(sender As Object, e As EventArgs) Handles btnOrientation.Click
            printDoc.DefaultPageSettings.Landscape = Not printDoc.DefaultPageSettings.Landscape
            If printDoc.DefaultPageSettings.Landscape Then
                btnOrientation.Text = "جهت کاغذ: افقی (Landscape)"
            Else
                btnOrientation.Text = "جهت کاغذ: عمودی (Portrait)"
            End If
            CalculatePages()
            previewCtrl.InvalidatePreview()
            UpdateNavigationUI()
        End Sub

        Private Sub btnPageSetup_Click(sender As Object, e As EventArgs) Handles btnPageSetup.Click
            If dialogPageSetup.ShowDialog() = DialogResult.OK Then
                CalculatePages()
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
            Try
                _currentPageIndex = 0
                printDoc.Print()
                MessageBox.Show("گزارش دفتر حساب با موفقیت به چاپگر ارسال شد.", "چاپ موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در چاپ گزارش: " & ex.Message, "خطا در چاپ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub printDoc_BeginPrint(sender As Object, e As PrintEventArgs) Handles printDoc.BeginPrint
            _currentPageIndex = 0
        End Sub

        Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
            Dim g = e.Graphics
            g.SmoothingMode = SmoothingMode.HighQuality
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Dim pageBounds = e.MarginBounds
            Dim leftX = pageBounds.Left
            Dim rightX = pageBounds.Right
            Dim topY = pageBounds.Top
            Dim bottomY = pageBounds.Bottom
            Dim pageWidth = pageBounds.Width

            Using fMainHeader As New Font("B Nazanin", 18.0!, FontStyle.Bold),
                  fSubHeader As New Font("B Nazanin", 12.0!, FontStyle.Bold),
                  fTableHeader As New Font("Tahoma", 9.0!, FontStyle.Bold),
                  fTableRow As New Font("Tahoma", 8.5!, FontStyle.Regular),
                  fTableBold As New Font("Tahoma", 8.5!, FontStyle.Bold),
                  fFooter As New Font("Tahoma", 8.0!, FontStyle.Regular)

                Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                Dim sfLeft As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}

                Dim currentY = topY

                Dim pageInfo = If(_currentPageIndex < _pages.Count, _pages(_currentPageIndex), CType(Nothing, PrintPageInfo?))
                If Not pageInfo.HasValue Then Return

                Dim block = pageInfo.Value.Block
                Dim pageRows = pageInfo.Value.Rows

                ' ۱. لوگوی شرکت
                If _logoImage IsNot Nothing Then
                    Dim logoWidth As Integer = 65
                    Dim logoHeight As Integer = 65
                    If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                        g.DrawImage(_logoImage, rightX - logoWidth, currentY, logoWidth, logoHeight)
                    Else
                        g.DrawImage(_logoImage, leftX, currentY, logoWidth, logoHeight)
                    End If
                End If

                ' نام شرکت در بالای عنوان گزارش (مطابق نمونه تصویر: قرمز عنابی)
                Using fCompany As New Font("B Nazanin", 13.0!, FontStyle.Bold),
                      brRed As New SolidBrush(Color.FromArgb(160, 0, 0))
                    Dim rCompany = New Rectangle(leftX, currentY, pageWidth, 25)
                    g.DrawString(SessionContext.CurrentCompanyName, fCompany, brRed, rCompany, sfCenter)
                End Using

                currentY += 28

                ' عنوان سربرگ (قرمز عنابی)
                Using brRed As New SolidBrush(Color.FromArgb(160, 0, 0))
                    Dim rHeaderTitle = New Rectangle(leftX, currentY, pageWidth, 32)
                    g.DrawString(block.LedgerTitle, fMainHeader, brRed, rHeaderTitle, sfCenter)
                End Using

                currentY += 42

                ' سطر مشخصات بالای جدول: سمت راست زنجیره حساب‌ها و سمت چپ "صفحه: X"
                Dim chain = block.AccountHierarchyChain

                Dim printedCount As Integer = 0
                Dim printLevel = Sub(lblPrefix As String, idx As Integer)
                                     If chain IsNot Nothing AndAlso idx < chain.Count Then
                                         Dim rLevel = New Rectangle(leftX, currentY, pageWidth, 20)
                                         Dim levelText = String.Format("{0} : {1} - {2}", lblPrefix, chain(idx).Item1, chain(idx).Item2)
                                         g.DrawString(levelText, fSubHeader, Brushes.Black, rLevel, sfRight)

                                         Dim textHeight As Integer = 22
                                         If idx = chain.Count - 1 Then
                                             Dim pageStr = String.Format("صفحه : {0}", _currentPageIndex + 1)
                                             g.DrawString(pageStr, fSubHeader, Brushes.Black, rLevel, sfLeft)
                                         End If

                                         currentY += textHeight
                                         printedCount += 1
                                     End If
                                 End Sub

                printLevel("گروه", 0)
                printLevel("کل", 1)
                printLevel("معین", 2)
                printLevel("تفضیلی1", 3)
                printLevel("تفضیلی2", 4)
                printLevel("تفضیلی3", 5)

                If printedCount = 0 Then
                    currentY += 25
                End If

                currentY += 10

                ' ۲. ساختار ستون‌های جدول (راست به چپ) بدون ستون عطف
                Dim cols As New List(Of ColDef) From {
                    New ColDef With {.Key = "RefNo", .Title = "شماره سند", .Ratio = 1.0F},
                    New ColDef With {.Key = "Date", .Title = "تاریخ", .Ratio = 1.6F},
                    New ColDef With {.Key = "Sharh", .Title = "شـــــرح", .Ratio = 3.6F},
                    New ColDef With {.Key = "Debit", .Title = "بدهکار", .Ratio = 1.9F},
                    New ColDef With {.Key = "Credit", .Title = "بستانکار", .Ratio = 1.9F},
                    New ColDef With {.Key = "Tash", .Title = "تشخیص", .Ratio = 1.1F},
                    New ColDef With {.Key = "Balance", .Title = "مانده", .Ratio = 1.9F}
                }

                Dim colWidths As New Dictionary(Of String, Integer)()
                Dim totalRatio As Single = cols.Sum(Function(c) c.Ratio)
                For Each c In cols
                    colWidths(c.Key) = CInt((c.Ratio / totalRatio) * pageWidth)
                Next
                Dim currentSum = colWidths.Values.Sum()
                If currentSum < pageWidth Then
                    colWidths(cols(cols.Count - 1).Key) += (pageWidth - currentSum)
                End If

                ' ۳. رسم هدر جدول (آبی فیروزه‌ای ملایم مطابق تصویر)
                Dim headerHeight = 34
                Dim rectHeaderFull = New Rectangle(leftX, currentY, pageWidth, headerHeight)
                Using brHeaderBg As New SolidBrush(Color.FromArgb(210, 236, 245))
                    g.FillRectangle(brHeaderBg, rectHeaderFull)
                End Using
                g.DrawRectangle(Pens.Black, rectHeaderFull)

                Dim colX = rightX
                For Each c In cols
                    Dim w = colWidths(c.Key)
                    colX -= w
                    Dim rectColHeader = New Rectangle(colX, currentY, w, headerHeight)
                    g.DrawRectangle(Pens.Black, rectColHeader)
                    g.DrawString(c.Title, fTableHeader, Brushes.Black, rectColHeader, sfCenter)
                Next

                currentY += headerHeight

                ' ۴. رسم ردیف‌های داده
                Dim rowHeight = 24

                For Each r In pageRows
                    If r.IsHeader Then
                        Dim rectHeaderBg = New Rectangle(leftX, currentY, pageWidth, rowHeight)
                        Using brBg As New SolidBrush(Color.FromArgb(235, 243, 255))
                            g.FillRectangle(brBg, rectHeaderBg)
                        End Using
                        g.DrawRectangle(Pens.Black, rectHeaderBg)
                        Dim textPadding = New Rectangle(leftX + 5, currentY, pageWidth - 10, rowHeight)
                        g.DrawString(r.Description, fTableBold, Brushes.DarkBlue, textPadding, sfRight)
                    ElseIf r.IsSummary Then
                        ' سطر جمع (زرد لیمویی ملایم مطابق تصویر)
                        Dim rectSumBg = New Rectangle(leftX, currentY, pageWidth, rowHeight)
                        Using brBg As New SolidBrush(Color.FromArgb(254, 248, 165))
                            g.FillRectangle(brBg, rectSumBg)
                        End Using
                        g.DrawRectangle(Pens.Black, rectSumBg)

                        colX = rightX
                        For Each c In cols
                            Dim w = colWidths(c.Key)
                            colX -= w
                            Dim rectCell = New Rectangle(colX, currentY, w, rowHeight)
                            g.DrawRectangle(Pens.Black, rectCell)

                            Dim cellPadding = New Rectangle(colX + 3, currentY, w - 6, rowHeight)
                            Select Case c.Key
                                Case "Sharh"
                                    g.DrawString(r.Description, fTableBold, Brushes.Black, cellPadding, sfRight)
                                Case "Debit"
                                    If r.DebitAmount.HasValue AndAlso r.DebitAmount.Value <> 0D Then
                                        g.DrawString(FormatAmount(r.DebitAmount.Value), fTableBold, Brushes.Black, cellPadding, sfRight)
                                    End If
                                Case "Credit"
                                    If r.CreditAmount.HasValue AndAlso r.CreditAmount.Value <> 0D Then
                                        g.DrawString(FormatAmount(r.CreditAmount.Value), fTableBold, Brushes.Black, cellPadding, sfRight)
                                    End If
                                Case "Tash"
                                    g.DrawString(r.Tashkhis, fTableBold, Brushes.Black, cellPadding, sfCenter)
                                Case "Balance"
                                    If r.BalanceAmount.HasValue Then
                                        g.DrawString(FormatAmount(r.BalanceAmount.Value), fTableBold, Brushes.Black, cellPadding, sfRight)
                                    End If
                            End Select
                        Next
                    Else
                        Dim rectRowFull = New Rectangle(leftX, currentY, pageWidth, rowHeight)
                        g.DrawRectangle(Pens.LightGray, rectRowFull)

                        colX = rightX
                        For Each c In cols
                            Dim w = colWidths(c.Key)
                            colX -= w
                            Dim rectCell = New Rectangle(colX, currentY, w, rowHeight)
                            g.DrawRectangle(Pens.LightGray, rectCell)

                            Dim cellPadding = New Rectangle(colX + 3, currentY, w - 6, rowHeight)

                            Select Case c.Key
                                Case "RefNo"
                                    g.DrawString(r.RefNo, fTableRow, Brushes.Black, cellPadding, sfCenter)
                                Case "Date"
                                    g.DrawString(r.EntryDate, fTableRow, Brushes.Black, cellPadding, sfCenter)
                                Case "Sharh"
                                    g.DrawString(r.Description, fTableRow, Brushes.Black, cellPadding, sfRight)
                                Case "Debit"
                                    If r.DebitAmount.HasValue AndAlso r.DebitAmount.Value <> 0D Then
                                        g.DrawString(FormatAmount(r.DebitAmount.Value), fTableRow, Brushes.Black, cellPadding, sfRight)
                                    End If
                                Case "Credit"
                                    If r.CreditAmount.HasValue AndAlso r.CreditAmount.Value <> 0D Then
                                        g.DrawString(FormatAmount(r.CreditAmount.Value), fTableRow, Brushes.Black, cellPadding, sfRight)
                                    End If
                                Case "Tash"
                                    g.DrawString(r.Tashkhis, fTableRow, Brushes.Black, cellPadding, sfCenter)
                                Case "Balance"
                                    If r.BalanceAmount.HasValue Then
                                        g.DrawString(FormatAmount(r.BalanceAmount.Value), fTableRow, Brushes.Black, cellPadding, sfRight)
                                    End If
                            End Select
                        Next
                    End If

                    currentY += rowHeight
                Next

                ' ۵. سطر "جمع" در انتهای آخرین صفحه بلاک جاری
                If pageInfo.Value.PageNumberInBlock = pageInfo.Value.TotalPagesInBlock Then
                    Dim totalRowHeight = 26
                    Dim rectTotalFull = New Rectangle(leftX, currentY, pageWidth, totalRowHeight)
                    Using brTotalBg As New SolidBrush(Color.FromArgb(255, 255, 190))
                        g.FillRectangle(brTotalBg, rectTotalFull)
                    End Using
                    g.DrawRectangle(Pens.Black, rectTotalFull)

                    colX = rightX
                    For Each c In cols
                        Dim w = colWidths(c.Key)
                        colX -= w
                        Dim rectCell = New Rectangle(colX, currentY, w, totalRowHeight)
                        g.DrawRectangle(Pens.Black, rectCell)

                        Dim cellPadding = New Rectangle(colX + 3, currentY, w - 6, totalRowHeight)

                        Select Case c.Key
                            Case "RefNo"
                                g.DrawString("جمع", fTableHeader, Brushes.Black, cellPadding, sfCenter)
                            Case "Debit"
                                g.DrawString(FormatAmount(block.TotalDebit), fTableHeader, Brushes.Black, cellPadding, sfRight)
                            Case "Credit"
                                g.DrawString(FormatAmount(block.TotalCredit), fTableHeader, Brushes.Black, cellPadding, sfRight)
                            Case "Tash"
                                g.DrawString(block.TotalTashkhis, fTableHeader, Brushes.Black, cellPadding, sfCenter)
                            Case "Balance"
                                If block.TotalBalance.HasValue AndAlso block.TotalBalance.Value <> 0D Then
                                    g.DrawString(FormatAmount(block.TotalBalance.Value), fTableHeader, Brushes.Black, cellPadding, sfRight)
                                End If
                        End Select
                    Next

                    currentY += totalRowHeight + 25

                    ' ۶. بخش امضاداران در پایین آخرین صفحه بلاک جاری
                    Dim sigY = currentY
                    Dim sigColWidth = pageWidth \ 3

                    Dim rectSig1 = New Rectangle(rightX - sigColWidth, sigY, sigColWidth, 35)
                    g.DrawString(_sig1Title & " : " & _sig1Name, fTableBold, Brushes.Black, rectSig1, sfCenter)

                    Dim rectSig2 = New Rectangle(rightX - (sigColWidth * 2), sigY, sigColWidth, 35)
                    g.DrawString(_sig2Title & " : " & _sig2Name, fTableBold, Brushes.Black, rectSig2, sfCenter)

                    Dim rectSig3 = New Rectangle(leftX, sigY, sigColWidth, 35)
                    g.DrawString(_sig3Title & " : " & _sig3Name, fTableBold, Brushes.Black, rectSig3, sfCenter)
                End If

                ' شماره صفحه انتهای برگ
                Dim pageNoStr = String.Format("صفحه {0} از {1}", pageInfo.Value.PageNumberInBlock, pageInfo.Value.TotalPagesInBlock)
                g.DrawString(pageNoStr, fFooter, Brushes.Black, leftX + (pageWidth \ 2), bottomY - 15, sfCenter)
            End Using

            _currentPageIndex += 1
            If _currentPageIndex < _totalPages Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                _currentPageIndex = 0
            End If
        End Sub

        Private Function FormatAmount(amt As Decimal) As String
            If amt = 0D Then Return ""
            Return Math.Abs(amt).ToString("#,##0")
        End Function

        Private Sub UpdateNavigationUI()
            If previewCtrl IsNot Nothing Then
                lblPageStatus.Text = String.Format("{0} از {1}", previewCtrl.StartPage + 1, _totalPages)
                lblZoomValue.Text = If(previewCtrl.AutoZoom, "خودکار", CInt(previewCtrl.Zoom * 100).ToString() & "%")
            End If
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
                If e.Delta > 0 Then
                    previewCtrl.Zoom = Math.Min(3.0, previewCtrl.Zoom + 0.05)
                Else
                    previewCtrl.Zoom = Math.Max(0.1, previewCtrl.Zoom - 0.05)
                End If
                UpdateNavigationUI()
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
                End If
            End If
        End Sub

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

                Dim newScrollPosX = Math.Max(0, Math.Min(_startScrollPosX - deltaX, limitH))
                Dim newScrollPosY = Math.Max(0, Math.Min(_startScrollPosY - deltaY, limitV))

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
