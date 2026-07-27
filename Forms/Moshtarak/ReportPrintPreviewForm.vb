Imports System
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class ReportPrintPreviewForm
        Inherits Form

        Private ReadOnly _printDoc As PrintDocument

        Public Sub New(doc As PrintDocument, Optional title As String = "پیش‌نمایش و تنظیمات چاپ گزارش")
            _printDoc = doc
            InitializeComponent()
            If Not String.IsNullOrEmpty(title) Then
                Me.Text = title
                lblTitle.Text = title
            End If
        End Sub

        Private Sub ReportPrintPreviewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            If _printDoc Is Nothing Then Return

            ' ۱. بارگذاری چاپگرهای سیستم
            cmbPrinter.Items.Clear()
            For Each prt As String In PrinterSettings.InstalledPrinters
                cmbPrinter.Items.Add(prt)
            Next

            ' انتخاب چاپگر پیش‌فرض
            If _printDoc.PrinterSettings IsNot Nothing Then
                Dim defaultPrinter = _printDoc.PrinterSettings.PrinterName
                If cmbPrinter.Items.Contains(defaultPrinter) Then
                    cmbPrinter.SelectedItem = defaultPrinter
                ElseIf cmbPrinter.Items.Count > 0 Then
                    cmbPrinter.SelectedIndex = 0
                End If

                ' ۲. مقداردهی تعداد نسخه‌ها
                numCopies.Value = Math.Max(1, _printDoc.PrinterSettings.Copies)
            End If

            ' ۳. تنظیم سند برای پیش‌نمایش
            dialogPageSetup.Document = _printDoc
            dialogPrint.Document = _printDoc

            previewCtrl.Document = _printDoc
            UpdatePageStatus()
        End Sub

        Private Sub cmbPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrinter.SelectedIndexChanged
            If _printDoc IsNot Nothing AndAlso _printDoc.PrinterSettings IsNot Nothing AndAlso cmbPrinter.SelectedItem IsNot Nothing Then
                _printDoc.PrinterSettings.PrinterName = cmbPrinter.SelectedItem.ToString()
                If previewCtrl IsNot Nothing Then previewCtrl.InvalidatePreview()
            End If
        End Sub

        Private Sub numCopies_ValueChanged(sender As Object, e As EventArgs) Handles numCopies.ValueChanged
            If _printDoc IsNot Nothing AndAlso _printDoc.PrinterSettings IsNot Nothing Then
                _printDoc.PrinterSettings.Copies = CShort(numCopies.Value)
            End If
        End Sub

        ' دکمه تنظیمات درایور چاپگر (Printer Properties)
        Private Sub btnPrinterProperties_Click(sender As Object, e As EventArgs) Handles btnPrinterProperties.Click
            dialogPrint.AllowSomePages = False
            dialogPrint.ShowHelp = False
            dialogPrint.UseEXDialog = True
            If dialogPrint.ShowDialog(Me) = DialogResult.OK Then
                If cmbPrinter.Items.Contains(_printDoc.PrinterSettings.PrinterName) Then
                    cmbPrinter.SelectedItem = _printDoc.PrinterSettings.PrinterName
                End If
                previewCtrl.InvalidatePreview()
            End If
        End Sub

        ' دکمه تنظیمات کاغذ و حاشیه (Page Setup / Paper Size / Orientation / Margins)
        Private Sub btnPageSetup_Click(sender As Object, e As EventArgs) Handles btnPageSetup.Click
            dialogPageSetup.EnableMetric = True
            If dialogPageSetup.ShowDialog(Me) = DialogResult.OK Then
                previewCtrl.InvalidatePreview()
                UpdatePageStatus()
            End If
        End Sub

        ' کلیدهای بزرگنمایی
        Private Sub btnZoomIn_Click(sender As Object, e As EventArgs) Handles btnZoomIn.Click
            previewCtrl.AutoZoom = False
            previewCtrl.Zoom = Math.Min(3.0D, previewCtrl.Zoom + 0.15D)
        End Sub

        Private Sub btnZoomOut_Click(sender As Object, e As EventArgs) Handles btnZoomOut.Click
            previewCtrl.AutoZoom = False
            previewCtrl.Zoom = Math.Max(0.2D, previewCtrl.Zoom - 0.15D)
        End Sub

        Private Sub btnZoomFit_Click(sender As Object, e As EventArgs) Handles btnZoomFit.Click
            previewCtrl.AutoZoom = True
        End Sub

        ' ناوبری صفحات
        Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
            If previewCtrl.StartPage > 0 Then
                previewCtrl.StartPage -= 1
                UpdatePageStatus()
            End If
        End Sub

        Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
            previewCtrl.StartPage += 1
            UpdatePageStatus()
        End Sub

        Private Sub UpdatePageStatus()
            lblPageStatus.Text = String.Format("صفحه {0}", previewCtrl.StartPage + 1)
        End Sub

        ' دکمه ارسال به چاپگر
        Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
            Try
                _printDoc.Print()
                MessageBox.Show("گزارش با موفقیت به چاپگر ارسال شد.", "چاپ موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ارسال به چاپگر: " & ex.Message, "خطا در چاپ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub
    End Class
End Namespace
