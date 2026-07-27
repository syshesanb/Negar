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
    Public Class HesabdaryTarazPrintForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        
        ' داده‌های گزارش
        Private ReadOnly _companyName As String = "مؤسسه حسابداری"
        Private ReadOnly _reportTitle As String = "تراز آزمایشی"
        Private ReadOnly _dateTitle As String = String.Empty
        Private ReadOnly _columns As New List(Of PrintColumnInfo)()
        Private ReadOnly _rows As New List(Of PrintRowInfo)()
        Private ReadOnly _totals As New Dictionary(Of String, Decimal)()

        ' مدیریت صفحه‌بندی
        Private _pages As New List(Of List(Of PrintRowInfo))()
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

        Public Class PrintColumnInfo
            Public Property Key As String
            Public Property Title As String
            Public Property WidthRatio As Single
        End Class

        Public Class PrintRowInfo
            Public Property AccountCode As String
            Public Property AccountName As String
            Public Property Values As New Dictionary(Of String, Decimal)()
            Public Property IsHeader As Boolean = False
            Public Property Level As Integer = 0
        End Class

        Public Sub New(companyName As String, dateTitle As String, columns As List(Of PrintColumnInfo), rows As List(Of PrintRowInfo), totals As Dictionary(Of String, Decimal), Optional reportTitle As String = "تراز آزمایشی")
            InitializeComponent()
            _companyName = If(String.IsNullOrWhiteSpace(companyName), "مؤسسه", companyName)
            _dateTitle = dateTitle
            _reportTitle = reportTitle
            _columns = columns
            _rows = rows
            _totals = totals
        End Sub

        Private Sub HesabdaryTarazPrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

                progress.UpdateProgress(100, "پیش‌نمایش تراز آزمایشی آماده شد")
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
            If _rows.Count = 0 Then
                _pages.Add(New List(Of PrintRowInfo)())
                _totalPages = 1
                Return
            End If

            ' تعداد ردیف در هر صفحه بر اساس جهت کاغذ
            Dim rowsPerPage As Integer = If(printDoc.DefaultPageSettings.Landscape, 22, 32)
            
            Dim currentPage As New List(Of PrintRowInfo)()
            For Each r In _rows
                currentPage.Add(r)
                If currentPage.Count >= rowsPerPage Then
                    _pages.Add(currentPage)
                    currentPage = New List(Of PrintRowInfo)()
                End If
            Next
            If currentPage.Count > 0 OrElse _pages.Count = 0 Then
                _pages.Add(currentPage)
            End If

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
                MessageBox.Show("گزارش تراز آزمایشی با موفقیت به چاپگر ارسال شد.", "چاپ موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

            ' فونت‌ها
            Using fTitleHeader As New Font("B Nazanin", 16.0!, FontStyle.Bold),
                  fSubHeader As New Font("B Nazanin", 13.0!, FontStyle.Bold),
                  fDateHeader As New Font("B Nazanin", 12.0!, FontStyle.Bold),
                  fTableHeader As New Font("Tahoma", 9.0!, FontStyle.Bold),
                  fTableRow As New Font("Tahoma", 8.5!, FontStyle.Regular),
                  fTableBold As New Font("Tahoma", 8.5!, FontStyle.Bold),
                  fFooter As New Font("Tahoma", 8.0!, FontStyle.Regular)

                Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                Dim sfLeft As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}

                ' ۱. رسم سربرگ (لوگو، نام مؤسسه، عنوان تراز، تاریخ)
                Dim currentY = topY

                ' لوگوی شرکت
                If _logoImage IsNot Nothing Then
                    Dim logoWidth As Integer = 70
                    Dim logoHeight As Integer = 70
                    If String.Equals(_logoPosition, "Right", StringComparison.OrdinalIgnoreCase) Then
                        g.DrawImage(_logoImage, rightX - logoWidth, currentY, logoWidth, logoHeight)
                    Else
                        g.DrawImage(_logoImage, leftX, currentY, logoWidth, logoHeight)
                    End If
                End If

                ' عناوین مرکزی با رنگ قرمز خرمایی (مطابق تصویر نمونه)
                Using brRed As New SolidBrush(Color.FromArgb(180, 0, 0))
                    Dim rHeaderComp = New Rectangle(leftX, currentY, pageWidth, 28)
                    g.DrawString(_companyName, fTitleHeader, brRed, rHeaderComp, sfCenter)

                    Dim rHeaderTitle = New Rectangle(leftX, currentY + 28, pageWidth, 26)
                    g.DrawString(_reportTitle, fSubHeader, brRed, rHeaderTitle, sfCenter)

                    If Not String.IsNullOrEmpty(_dateTitle) Then
                        Dim rHeaderDate = New Rectangle(leftX, currentY + 54, pageWidth, 24)
                        g.DrawString(_dateTitle, fDateHeader, brRed, rHeaderDate, sfCenter)
                        currentY += 82
                    Else
                        currentY += 60
                    End If
                End Using

                currentY += 10

                ' ۲. محاسبه پهنای ستون‌های جدول
                Dim colWidths As New Dictionary(Of String, Integer)()
                Dim totalRatio As Single = _columns.Sum(Function(c) c.WidthRatio)
                For Each col In _columns
                    colWidths(col.Key) = CInt((col.WidthRatio / totalRatio) * pageWidth)
                Next
                ' تنظیم مابقی عرض برای جبران گرد کردن
                Dim currentSum = colWidths.Values.Sum()
                If currentSum < pageWidth AndAlso _columns.Count > 0 Then
                    colWidths(_columns(_columns.Count - 1).Key) += (pageWidth - currentSum)
                End If

                ' ۳. رسم هدر جدول (با رنگ آبی ملایم مطابق تصویر نمونه)
                Dim headerHeight = 28
                Dim rectHeaderFull = New Rectangle(leftX, currentY, pageWidth, headerHeight)
                Using brHeaderBg As New SolidBrush(Color.FromArgb(200, 230, 245))
                    g.FillRectangle(brHeaderBg, rectHeaderFull)
                End Using
                g.DrawRectangle(Pens.Black, rectHeaderFull)

                Dim colX = rightX
                For Each col In _columns
                    Dim w = colWidths(col.Key)
                    colX -= w
                    Dim rectColHeader = New Rectangle(colX, currentY, w, headerHeight)
                    g.DrawRectangle(Pens.Black, rectColHeader)
                    g.DrawString(col.Title, fTableHeader, Brushes.Black, rectColHeader, sfCenter)
                Next

                currentY += headerHeight

                ' ۴. رسم ردیف‌های داده مربوط به صفحه جاری
                Dim pageRows = If(_currentPageIndex < _pages.Count, _pages(_currentPageIndex), New List(Of PrintRowInfo)())
                Dim rowHeight = 24

                For Each r In pageRows
                    Dim rectRowFull = New Rectangle(leftX, currentY, pageWidth, rowHeight)
                    
                    ' هایلایت ردیف‌های اصلی/والد
                    If r.IsHeader Then
                        Using brGroupBg As New SolidBrush(Color.FromArgb(245, 248, 255))
                            g.FillRectangle(brGroupBg, rectRowFull)
                        End Using
                    End If
                    g.DrawRectangle(Pens.LightGray, rectRowFull)

                    colX = rightX
                    For Each col In _columns
                        Dim w = colWidths(col.Key)
                        colX -= w
                        Dim rectCell = New Rectangle(colX, currentY, w, rowHeight)
                        g.DrawRectangle(Pens.LightGray, rectCell)

                        Dim cellRectPadding = New Rectangle(colX + 4, currentY, w - 8, rowHeight)

                        If col.Key = "AccountName" Then
                            ' نام حساب به صورت راست‌چین
                            Dim textFont = If(r.IsHeader, fTableBold, fTableRow)
                            ' اعمال فاصله بر اساس سطح حساب
                            Dim indent = r.Level * 10
                            Dim rIndent = New Rectangle(colX + 4, currentY, w - 8 - indent, rowHeight)
                            g.DrawString(r.AccountName, textFont, Brushes.Black, rIndent, sfRight)
                        ElseIf col.Key = "AccountCode" Then
                            g.DrawString(r.AccountCode, fTableRow, Brushes.Black, cellRectPadding, sfCenter)
                        Else
                            ' مقادیر عددی (بدهکار / بستانکار و ...)
                            If r.Values.ContainsKey(col.Key) AndAlso r.Values(col.Key) <> 0D Then
                                Dim valStr = FormatAmount(r.Values(col.Key))
                                Dim textFont = If(r.IsHeader, fTableBold, fTableRow)
                                g.DrawString(valStr, textFont, Brushes.Black, cellRectPadding, sfRight)
                            End If
                        End If
                    Next

                    currentY += rowHeight
                Next

                ' ۵. اگر صفحه آخر است، ردیف "جمع" به انتهای جدول اضافه می‌شود (با پس‌زمینه زرد لیمویی مطابق تصویر نمونه)
                If _currentPageIndex = _pages.Count - 1 Then
                    Dim totalRowHeight = 26
                    Dim rectTotalFull = New Rectangle(leftX, currentY, pageWidth, totalRowHeight)
                    Using brTotalBg As New SolidBrush(Color.FromArgb(255, 255, 190)) ' زرد لیمویی ملایم
                        g.FillRectangle(brTotalBg, rectTotalFull)
                    End Using
                    g.DrawRectangle(Pens.Black, rectTotalFull)

                    colX = rightX
                    For Each col In _columns
                        Dim w = colWidths(col.Key)
                        colX -= w
                        Dim rectCell = New Rectangle(colX, currentY, w, totalRowHeight)
                        g.DrawRectangle(Pens.Black, rectCell)

                        Dim cellRectPadding = New Rectangle(colX + 4, currentY, w - 8, totalRowHeight)

                        If col.Key = "AccountName" Then
                            g.DrawString("جمع", fTableHeader, Brushes.Black, cellRectPadding, sfRight)
                        ElseIf col.Key <> "AccountCode" AndAlso _totals.ContainsKey(col.Key) Then
                            Dim valStr = FormatAmount(_totals(col.Key))
                            g.DrawString(valStr, fTableHeader, Brushes.Black, cellRectPadding, sfRight)
                        End If
                    Next

                    currentY += totalRowHeight + 20

                    ' ۶. بخش امضاداران در پایین آخرین صفحه
                    Dim sigY = currentY
                    Dim sigColWidth = pageWidth \ 3

                    Dim rectSig1 = New Rectangle(rightX - sigColWidth, sigY, sigColWidth, 35)
                    g.DrawString(_sig1Title & " : " & _sig1Name, fTableBold, Brushes.Black, rectSig1, sfCenter)

                    Dim rectSig2 = New Rectangle(rightX - (sigColWidth * 2), sigY, sigColWidth, 35)
                    g.DrawString(_sig2Title & " : " & _sig2Name, fTableBold, Brushes.Black, rectSig2, sfCenter)

                    Dim rectSig3 = New Rectangle(leftX, sigY, sigColWidth, 35)
                    g.DrawString(_sig3Title & " : " & _sig3Name, fTableBold, Brushes.Black, rectSig3, sfCenter)
                End If

                ' ۷. شماره صفحه در پایین برگ
                Dim pageNoStr = String.Format("صفحه {0} از {1}", _currentPageIndex + 1, _totalPages)
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
            If previewCtrl Is Nothing Then Return
            lblPageStatus.Text = String.Format("{0} از {1}", previewCtrl.StartPage + 1, _totalPages)
            lblZoomValue.Text = If(previewCtrl.AutoZoom, "خودکار", CInt(previewCtrl.Zoom * 100).ToString() & "%")
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
