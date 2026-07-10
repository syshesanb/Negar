Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryReportPrintForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private ReadOnly _reportId As Integer
        
        Private _companyName As String = "مؤسسه حسابداری"
        Private _reportName As String = ""
        Private _totalPages As Integer = 1

        ' Style configurations
        Private _fontHeaderName As String = "Tahoma"
        Private _fontHeaderSize As Decimal = 12
        Private _fontMainRowName As String = "Tahoma"
        Private _fontMainRowSize As Decimal = 10
        Private _fontDetailRowName As String = "Tahoma"
        Private _fontDetailRowSize As Decimal = 9
        Private _fontFormulaName As String = "Tahoma"
        Private _fontFormulaSize As Decimal = 9
        Private _fontFormulaDetailName As String = "Tahoma"
        Private _fontFormulaDetailSize As Decimal = 9
        Private _rowCount As Integer = 50
        Private _colCount As Integer = 10
        Private _orientation As String = "عمودی"
        Private _paperSize As String = "A4"
        Private _marginTop As Decimal = 10
        Private _marginBottom As Decimal = 10
        Private _marginLeft As Decimal = 10
        Private _marginRight As Decimal = 10
        Private _pageBorder As String = "بدون کادر"

        Private ReadOnly _nodes As New List(Of ReportRowNode)()

        ' Logos and Signatures
        Private _logoImage As Image = Nothing
        Private _logoPosition As String = "Left"
        Private _sig1Title As String = "تهیه کننده"
        Private _sig1Name As String = ""
        Private _sig2Title As String = "تأیید کننده"
        Private _sig2Name As String = ""
        Private _sig3Title As String = "تصویب کننده"
        Private _sig3Name As String = ""

        ' Panning variables
        Private _isPanning As Boolean = False
        Private _startMousePos As Point
        Private _startScrollPosX As Integer
        Private _startScrollPosY As Integer

        Public Class ReportRowNode
            Public Property CategoryName As String
            Public Property IsMainRow As Boolean
            Public Property RO As String
            Public Property SO As String
            Public Property RN As String
            Public Property SN As String
            Public Property UnderlineStyle As String
            Public Property CategoryID As Integer
            Public Property AccountID As Integer
            Public Property Formula As String
            Public Property BaseValue As Decimal
            Public Property FinalValue As Decimal
End Class

        Public Sub New(reportId As Integer)
            InitializeComponent()
            _reportId = reportId
        End Sub

        Private Sub HesabdaryReportPrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "بارگذاری مشخصات چاپگرها...")

                ' 1. Load printers
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

                progress.UpdateProgress(30, "بارگذاری تنظیمات گزارش و امضاداران...")
                LoadReportSettings()
                LoadCompanyLogoAndSignatures()
                CalculateNodeValues()

                progress.UpdateProgress(60, "پیکربندی صفحه...")
                SetupPageSettings()

                progress.UpdateProgress(90, "آماده‌سازی پیش‌نمایش...")
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()

                progress.UpdateProgress(100, "پیش‌نمایش گزارش آماده شد")
            End Using
        End Sub

        Private Sub LoadReportSettings()
            Try
                ' Load Report1 layout parameters
                Dim dtRep = Sql.ExecuteTable("SELECT ReportName, FontHeaderName, FontHeaderSize, FontMainRowName, FontMainRowSize, FontDetailRowName, FontDetailRowSize, FontFormulaName, FontFormulaSize, FontFormulaDetailName, FontFormulaDetailSize, RowCount, ColCount, Orientation, MarginTop, MarginBottom, MarginLeft, MarginRight, PageBorder FROM Report1 WHERE ReportID = ?", _reportId)
                If dtRep.Rows.Count > 0 Then
                    Dim r = dtRep.Rows(0)
                    _reportName = Convert.ToString(r("ReportName"))
                    
                    _fontHeaderName = If(Convert.IsDBNull(r("FontHeaderName")), "Tahoma", Convert.ToString(r("FontHeaderName")))
                    _fontHeaderSize = If(Convert.IsDBNull(r("FontHeaderSize")), 12, Convert.ToDecimal(r("FontHeaderSize")))
                    
                    _fontMainRowName = If(Convert.IsDBNull(r("FontMainRowName")), "Tahoma", Convert.ToString(r("FontMainRowName")))
                    _fontMainRowSize = If(Convert.IsDBNull(r("FontMainRowSize")), 10, Convert.ToDecimal(r("FontMainRowSize")))
                    
                    _fontDetailRowName = If(Convert.IsDBNull(r("FontDetailRowName")), "Tahoma", Convert.ToString(r("FontDetailRowName")))
                    _fontDetailRowSize = If(Convert.IsDBNull(r("FontDetailRowSize")), 9, Convert.ToDecimal(r("FontDetailRowSize")))
                    
                    _fontFormulaName = If(Convert.IsDBNull(r("FontFormulaName")), "Tahoma", Convert.ToString(r("FontFormulaName")))
                    _fontFormulaSize = If(Convert.IsDBNull(r("FontFormulaSize")), 9, Convert.ToDecimal(r("FontFormulaSize")))
                    _fontFormulaDetailName = If(Convert.IsDBNull(r("FontFormulaDetailName")), "Tahoma", Convert.ToString(r("FontFormulaDetailName")))
                    _fontFormulaDetailSize = If(Convert.IsDBNull(r("FontFormulaDetailSize")), 9, Convert.ToDecimal(r("FontFormulaDetailSize")))
                    
                    _rowCount = If(Convert.IsDBNull(r("RowCount")), 50, Convert.ToInt32(r("RowCount")))
                    _colCount = If(Convert.IsDBNull(r("ColCount")), 10, Convert.ToInt32(r("ColCount")))
                    
                    _orientation = If(Convert.IsDBNull(r("Orientation")), "عمودی", Convert.ToString(r("Orientation")))
                    
                    _marginTop = If(Convert.IsDBNull(r("MarginTop")), 10, Convert.ToDecimal(r("MarginTop")))
                    _marginBottom = If(Convert.IsDBNull(r("MarginBottom")), 10, Convert.ToDecimal(r("MarginBottom")))
                    _marginLeft = If(Convert.IsDBNull(r("MarginLeft")), 10, Convert.ToDecimal(r("MarginLeft")))
                    _marginRight = If(Convert.IsDBNull(r("MarginRight")), 10, Convert.ToDecimal(r("MarginRight")))
                    
                                        _pageBorder = If(Convert.IsDBNull(r("PageBorder")), "بدون کادر", Convert.ToString(r("PageBorder")))
                    If dtRep.Columns.Contains("PaperSize") AndAlso Not Convert.IsDBNull(r("PaperSize")) Then _paperSize = Convert.ToString(r("PaperSize"))
                End If

                ' Load rows categories from Report2
                _nodes.Clear()
                Dim dtCats = service.GetProfitLossCategories(_reportId)
                For Each row As DataRow In dtCats.Rows
                    Dim isMainRow = If(dtCats.Columns.Contains("IsMainRow") AndAlso Not Convert.IsDBNull(row("IsMainRow")), Convert.ToInt32(row("IsMainRow")) = 1, False)
                    Dim roVal = If(dtCats.Columns.Contains("RO"), Convert.ToString(row("RO")), "")
                    Dim soVal = If(dtCats.Columns.Contains("SO"), Convert.ToString(row("SO")), "")
                    Dim rnVal = If(dtCats.Columns.Contains("RN"), Convert.ToString(row("RN")), "")
                    Dim snVal = If(dtCats.Columns.Contains("SN"), Convert.ToString(row("SN")), "")
                    Dim underline = If(dtCats.Columns.Contains("UnderlineStyle"), Convert.ToString(row("UnderlineStyle")), "بدون خط")
                    
                    Dim catId = Convert.ToInt32(row("CategoryID"))
                    Dim formula = If(dtCats.Columns.Contains("Formula"), Convert.ToString(row("Formula")), "")
                    Dim accId = If(dtCats.Columns.Contains("AccountID") AndAlso Not Convert.IsDBNull(row("AccountID")), Convert.ToInt32(row("AccountID")), 0)
                    
                    Dim node As New ReportRowNode() With {
                        .CategoryName = Convert.ToString(row("CategoryName")),
                        .IsMainRow = isMainRow,
                        .RO = roVal,
                        .SO = soVal,
                        .RN = rnVal,
                        .SN = snVal,
                        .UnderlineStyle = underline,
                        .CategoryID = catId,
                        .Formula = formula,
                        .AccountID = accId
                    }
                    _nodes.Add(node)
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری پیکربندی گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadCompanyLogoAndSignatures()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim dt = Sql.ExecuteTable("SELECT CompanyName, LogoImage, Signatory1Title, Signatory1Name, Signatory2Title, Signatory2Name, Signatory3Title, Signatory3Name, LogoPosition FROM Companies WHERE CompanyID = ?", companyId)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    _companyName = Convert.ToString(row("CompanyName"))
                    
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

        Private Sub SetupPageSettings()
            ' Convert Margins from mm to hundredths of an inch (1 inch = 25.4 mm)
            Dim marginL = CInt(_marginLeft / 25.4 * 100)
            Dim marginR = CInt(_marginRight / 25.4 * 100)
            Dim marginT = CInt(_marginTop / 25.4 * 100)
            Dim marginB = CInt(_marginBottom / 25.4 * 100)

            Dim w As Integer = 827 ' A4 default
            Dim h As Integer = 1169
            Select Case _paperSize
                Case "A3"
                    w = 1169
                    h = 1654
                Case "A5"
                    w = 583
                    h = 827
                Case "Letter"
                    w = 850
                    h = 1100
                Case "A4"
                    w = 827
                    h = 1169
            End Select
            printDoc.DefaultPageSettings.PaperSize = New PaperSize(_paperSize, w, h)

            printDoc.DefaultPageSettings.Margins = New Margins(marginL, marginR, marginT, marginB)
            printDoc.DefaultPageSettings.Landscape = (_orientation = "افقی")
            
            btnOrientation.Text = "جهت کاغذ: " & If(printDoc.DefaultPageSettings.Landscape, "افقی (Landscape)", "عمودی (Portrait)")
        End Sub

        Private Function ParseMmToHundredths(val As String) As Single
            Dim mm As Single = 0
            If Single.TryParse(val, mm) Then
                Return CSng(mm * (100.0 / 25.4))
            End If
            Return 0
        End Function

        Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
            Dim g = e.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias
            
            Dim margins = e.PageSettings.Margins
            Dim bounds = e.PageSettings.Bounds
            
            Dim printableWidth = bounds.Width - margins.Left - margins.Right
            Dim printableHeight = bounds.Height - margins.Top - margins.Bottom
            
            ' 1. Draw Page Border
            DrawPageBorder(g, margins.Left, margins.Top, printableWidth, printableHeight)
            
            ' 2. Draw Header Area
            Dim headerY = margins.Top + 10
            Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            
            ' Company Name Center (Above Report Name)
            Dim compRect As New RectangleF(margins.Left, headerY, printableWidth, 25)
            Using compBrush As New SolidBrush(Color.Black)
                g.DrawString(_companyName, New Font("Tahoma", 11.0!, FontStyle.Bold), compBrush, compRect, sfCenter)
            End Using
            
            ' Title Center (Below Company Name)
            Dim headerFont As New Font(_fontHeaderName, CSng(_fontHeaderSize), FontStyle.Bold)
            Dim titleRect As New RectangleF(margins.Left, headerY + 25, printableWidth, 35)
            g.DrawString(_reportName, headerFont, Brushes.Black, titleRect, sfCenter)
            
            ' Logo image
            If _logoImage IsNot Nothing Then
                Dim logoSize = 45
                Dim logoX = margins.Left + 20
                If _logoPosition = "Right" Then
                    logoX = margins.Left + printableWidth - 20 - logoSize
                End If
                Dim logoRect As New Rectangle(logoX, CInt(headerY), logoSize, logoSize)
                g.DrawImage(_logoImage, logoRect)
            End If

            ' 3. Draw Rows Content
            Dim sfCell As New StringFormat() With {.FormatFlags = StringFormatFlags.DirectionRightToLeft, .Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near}
            
            For Each node In _nodes
                ' Draw Category Label at Category Coordinates (RO, SO)
                Dim roY = ParseMmToHundredths(node.RO)
                Dim soX = ParseMmToHundredths(node.SO)
                If roY > 0 AndAlso soX > 0 Then
                    Dim cX = bounds.Right - margins.Right - soX
                    Dim cY = margins.Top + roY
                    
                    Dim fName = If(node.IsMainRow, _fontMainRowName, _fontDetailRowName)
                    Dim fSize = CSng(If(node.IsMainRow, _fontMainRowSize, _fontDetailRowSize))
                    Dim fStyle = If(node.IsMainRow, FontStyle.Bold, FontStyle.Regular)
                    
                    Dim font As New Font(fName, fSize, fStyle)
                    g.DrawString(node.CategoryName, font, Brushes.Black, cX, cY, sfCell)
                End If
                
                ' Draw Result Placeholder at Formula Coordinates (RN, SN)
                Dim rnY = ParseMmToHundredths(node.RN)
                Dim snX = ParseMmToHundredths(node.SN)
                If rnY > 0 AndAlso snX > 0 Then
                    Dim cX = bounds.Right - margins.Right - snX
                    Dim cY = margins.Top + rnY
                    
                    Dim fontName = If(node.IsMainRow, _fontFormulaName, _fontFormulaDetailName)
                    Dim fontSize = If(node.IsMainRow, _fontFormulaSize, _fontFormulaDetailSize)
                    Dim font As New Font(fontName, CSng(fontSize), FontStyle.Bold)
                    
                    Dim valToPrint = node.FinalValue.ToString("N0")
                    ' Replace English digits with Persian digits
                    valToPrint = valToPrint.Replace("0"c, "۰"c).Replace("1"c, "۱"c).Replace("2"c, "۲"c).Replace("3"c, "۳"c).Replace("4"c, "۴"c).Replace("5"c, "۵"c).Replace("6"c, "۶"c).Replace("7"c, "۷"c).Replace("8"c, "۸"c).Replace("9"c, "۹"c)
                    g.DrawString(valToPrint, font, Brushes.Black, cX, cY, sfCell)
                    
                    Dim textSize = g.MeasureString(valToPrint, font)
                    Dim actualLeftEdge = cX - textSize.Width
                    DrawUnderline(g, actualLeftEdge, cY, textSize.Width, textSize.Height, node.UnderlineStyle)
                End If
            Next
            
            ' 5. Draw Signatures
            DrawSignatures(g, margins.Top + printableHeight - 60, margins.Left, printableWidth)
            
            e.HasMorePages = False
        End Sub

        Private Sub DrawPageBorder(g As Graphics, left As Single, top As Single, width As Single, height As Single)
            If _pageBorder = "بدون کادر" Then Return
            
            Dim penThin As New Pen(Color.FromArgb(80, 80, 80), 1.0!)
            Dim penThick As New Pen(Color.FromArgb(50, 50, 50), 2.5!)
            
            If _pageBorder = "خط تکی نازک" Then
                g.DrawRectangle(penThin, left, top, width, height)
            ElseIf _pageBorder = "خط تکی ضخیم" Then
                g.DrawRectangle(penThick, left, top, width, height)
            ElseIf _pageBorder = "خط دوتایی" Then
                g.DrawRectangle(penThin, left, top, width, height)
                g.DrawRectangle(penThin, left + 3, top + 3, width - 6, height - 6)
            ElseIf _pageBorder = "خط دوتایی نازک، ضخیم" Then
                g.DrawRectangle(penThick, left, top, width, height)
                g.DrawRectangle(penThin, left + 4, top + 4, width - 8, height - 8)
            End If
            
            penThin.Dispose()
            penThick.Dispose()
        End Sub

        Private Sub DrawUnderline(g As Graphics, x As Single, y As Single, width As Single, height As Single, style As String)
            If String.IsNullOrEmpty(style) OrElse style = "بدون خط" Then Return
            
            Dim penThin As New Pen(Color.Black, 1.0!)
            Dim penThick As New Pen(Color.Black, 2.0!)
            
            Dim lineY = y + height - 2
            
            If style = "خط تکی نازک" Then
                g.DrawLine(penThin, x, lineY, x + width, lineY)
            ElseIf style = "خط تکی ضخیم" Then
                g.DrawLine(penThick, x, lineY, x + width, lineY)
            ElseIf style = "خط دوتایی" Then
                g.DrawLine(penThin, x, lineY - 2, x + width, lineY - 2)
                g.DrawLine(penThin, x, lineY, x + width, lineY)
            ElseIf style = "خط دوتایی نازک، ضخیم" Then
                g.DrawLine(penThin, x, lineY - 3, x + width, lineY - 3)
                g.DrawLine(penThick, x, lineY, x + width, lineY)
            End If
            
            penThin.Dispose()
            penThick.Dispose()
        End Sub

        Private Sub DrawSignatures(g As Graphics, y As Single, left As Single, width As Single)
            Dim fSig As New Font("Tahoma", 9.0!, FontStyle.Bold)
            Dim colWidth = width / 3
            Dim sfCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Near}
            
            If Not String.IsNullOrEmpty(_sig1Title) Then
                g.DrawString(_sig1Title, fSig, Brushes.Black, New RectangleF(left, y, colWidth, 40), sfCenter)
                If Not String.IsNullOrEmpty(_sig1Name) Then
                    g.DrawString(_sig1Name, fSig, Brushes.Black, New RectangleF(left, y + 20, colWidth, 20), sfCenter)
                End If
            End If
            
            If Not String.IsNullOrEmpty(_sig2Title) Then
                g.DrawString(_sig2Title, fSig, Brushes.Black, New RectangleF(left + colWidth, y, colWidth, 40), sfCenter)
                If Not String.IsNullOrEmpty(_sig2Name) Then
                    g.DrawString(_sig2Name, fSig, Brushes.Black, New RectangleF(left + colWidth, y + 20, colWidth, 20), sfCenter)
                End If
            End If
            
            If Not String.IsNullOrEmpty(_sig3Title) Then
                g.DrawString(_sig3Title, fSig, Brushes.Black, New RectangleF(left + colWidth * 2, y, colWidth, 40), sfCenter)
                If Not String.IsNullOrEmpty(_sig3Name) Then
                    g.DrawString(_sig3Name, fSig, Brushes.Black, New RectangleF(left + colWidth * 2, y + 20, colWidth, 20), sfCenter)
                End If
            End If
        End Sub

        ' Navigation & Zoom handlers
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

        Private Sub cmbPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrinter.SelectedIndexChanged
            If cmbPrinter.SelectedItem IsNot Nothing Then
                printDoc.PrinterSettings.PrinterName = cmbPrinter.SelectedItem.ToString()
            End If
        End Sub

        Private Sub numCopies_ValueChanged(sender As Object, e As EventArgs) Handles numCopies.ValueChanged
            printDoc.PrinterSettings.Copies = CShort(numCopies.Value)
        End Sub

        Private Sub btnOrientation_Click(sender As Object, e As EventArgs) Handles btnOrientation.Click
            printDoc.DefaultPageSettings.Landscape = Not printDoc.DefaultPageSettings.Landscape
            btnOrientation.Text = "جهت کاغذ: " & If(printDoc.DefaultPageSettings.Landscape, "افقی (Landscape)", "عمودی (Portrait)")
            previewCtrl.InvalidatePreview()
            UpdateNavigationUI()
        End Sub

        Private Sub btnPageSetup_Click(sender As Object, e As EventArgs) Handles btnPageSetup.Click
            If dialogPageSetup.ShowDialog(Me) = DialogResult.OK Then
                previewCtrl.InvalidatePreview()
                UpdateNavigationUI()
            End If
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
            Try
                printDoc.PrinterSettings.Copies = CShort(numCopies.Value)
                printDoc.Print()
                MessageBox.Show("سند با موفقیت به چاپگر ارسال شد.", "چاپ موفقیت‌آمیز", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در چاپ سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub previewCtrl_MouseEnter(sender As Object, e As EventArgs) Handles previewCtrl.MouseEnter
            previewCtrl.Focus()
        End Sub

        Private Sub previewCtrl_MouseWheel(sender As Object, e As MouseEventArgs) Handles previewCtrl.MouseWheel
            Dim isCtrlPressed As Boolean = (ModifierKeys And Keys.Control) = Keys.Control
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

        <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
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
            Private Sub CalculateNodeValues()
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return
            Dim balances = service.GetAllAccountBalances(SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value)
            Dim allMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)

            ' Pass 1: Calculate Base Values
            For Each node In _nodes
                Dim sum As Decimal = 0
                If node.AccountID > 0 Then
                    Dim targetCode As String = ""
                    If balances.ContainsKey(node.AccountID) Then
                        targetCode = balances(node.AccountID).Item1
                    Else
                        Try
                            Dim info = service.GetAccountInfo(node.AccountID)
                            targetCode = info.Item1
                        Catch
                        End Try
                    End If
                    
                    If Not String.IsNullOrEmpty(targetCode) Then
                        For Each kvp In balances.Values
                            If kvp.Item1.StartsWith(targetCode) Then
                                sum += kvp.Item2
                            End If
                        Next
                    End If
                ElseIf node.CategoryID > 0 Then
                    Dim dv As New DataView(allMappings)
                    dv.RowFilter = "CategoryID = " & node.CategoryID
                    For Each mapRow As DataRowView In dv
                        Dim cId = Convert.ToInt32(mapRow("AccountID"))
                        Dim targetCode As String = ""
                        If balances.ContainsKey(cId) Then
                            targetCode = balances(cId).Item1
                        Else
                            Try
                                Dim info = service.GetAccountInfo(cId)
                                targetCode = info.Item1
                            Catch
                            End Try
                        End If
                        If Not String.IsNullOrEmpty(targetCode) Then
                            For Each kvp In balances.Values
                                If kvp.Item1.StartsWith(targetCode) Then
                                    sum += kvp.Item2
                                End If
                            Next
                        End If
                    Next
                End If
                node.BaseValue = sum
            Next

            ' Pass 2: Evaluate Formulas
            Dim dtMath As New DataTable()
            For i As Integer = 0 To _nodes.Count - 1
                Dim node = _nodes(i)
                If String.IsNullOrWhiteSpace(node.Formula) Then
                    node.FinalValue = node.BaseValue
                Else
                    Dim expr = node.Formula
                    ' Remove = from the expression to allow compute to work
                    expr = expr.Replace("=", "").Trim()
                    ' Replace [n] with the base value of row n. Note: row 1 is _nodes(0)
                    For j As Integer = 0 To _nodes.Count - 1
                        Dim pattern = "[" & (j + 1).ToString() & "]"
                        expr = expr.Replace(pattern, _nodes(j).BaseValue.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    Next
                    Try
                        Dim resultObj = dtMath.Compute(expr, "")
                        node.FinalValue = Convert.ToDecimal(resultObj)
                    Catch ex As Exception
                        ' If formula evaluation fails, fallback to base value
                        node.FinalValue = node.BaseValue
                    End Try
                End If
            Next
        End Sub


End Class
End Namespace
