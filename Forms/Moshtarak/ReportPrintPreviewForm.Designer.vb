Namespace Negar.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ReportPrintPreviewForm
        Inherits System.Windows.Forms.Form

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlLeft = New System.Windows.Forms.Panel()
            Me.grpSign = New System.Windows.Forms.GroupBox()
            Me.btnPrint = New System.Windows.Forms.Button()
            Me.btnClose = New System.Windows.Forms.Button()
            Me.grpPage = New System.Windows.Forms.GroupBox()
            Me.btnPageSetup = New System.Windows.Forms.Button()
            Me.grpPrinter = New System.Windows.Forms.GroupBox()
            Me.btnPrinterProperties = New System.Windows.Forms.Button()
            Me.numCopies = New System.Windows.Forms.NumericUpDown()
            Me.lblCopies = New System.Windows.Forms.Label()
            Me.cmbPrinter = New System.Windows.Forms.ComboBox()
            Me.lblPrinter = New System.Windows.Forms.Label()
            Me.grpZoomNav = New System.Windows.Forms.GroupBox()
            Me.btnNextPage = New System.Windows.Forms.Button()
            Me.lblPageStatus = New System.Windows.Forms.Label()
            Me.btnPrevPage = New System.Windows.Forms.Button()
            Me.btnZoomFit = New System.Windows.Forms.Button()
            Me.btnZoomOut = New System.Windows.Forms.Button()
            Me.btnZoomIn = New System.Windows.Forms.Button()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.previewCtrl = New System.Windows.Forms.PrintPreviewControl()
            Me.dialogPageSetup = New System.Windows.Forms.PageSetupDialog()
            Me.dialogPrint = New System.Windows.Forms.PrintDialog()
            Me.pnlLeft.SuspendLayout()
            Me.grpPage.SuspendLayout()
            Me.grpPrinter.SuspendLayout()
            CType(Me.numCopies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpZoomNav.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlLeft
            '
            Me.pnlLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlLeft.Controls.Add(Me.lblTitle)
            Me.pnlLeft.Controls.Add(Me.btnPrint)
            Me.pnlLeft.Controls.Add(Me.grpPrinter)
            Me.pnlLeft.Controls.Add(Me.grpPage)
            Me.pnlLeft.Controls.Add(Me.grpZoomNav)
            Me.pnlLeft.Controls.Add(Me.btnClose)
            Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlLeft.Location = New System.Drawing.Point(740, 0)
            Me.pnlLeft.Name = "pnlLeft"
            Me.pnlLeft.Padding = New System.Windows.Forms.Padding(10)
            Me.pnlLeft.Size = New System.Drawing.Size(280, 680)
            Me.pnlLeft.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(100, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(10, 10)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(260, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "پیش‌نمایش و تنظیمات چاپ"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnPrint
            '
            Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPrint.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnPrint.ForeColor = System.Drawing.Color.White
            Me.btnPrint.Location = New System.Drawing.Point(10, 48)
            Me.btnPrint.Name = "btnPrint"
            Me.btnPrint.Size = New System.Drawing.Size(260, 42)
            Me.btnPrint.TabIndex = 1
            Me.btnPrint.Text = "🖨  ارسال به چاپگر (Print)"
            Me.btnPrint.UseVisualStyleBackColor = False
            '
            'grpPrinter
            '
            Me.grpPrinter.Controls.Add(Me.btnPrinterProperties)
            Me.grpPrinter.Controls.Add(Me.numCopies)
            Me.grpPrinter.Controls.Add(Me.lblCopies)
            Me.grpPrinter.Controls.Add(Me.cmbPrinter)
            Me.grpPrinter.Controls.Add(Me.lblPrinter)
            Me.grpPrinter.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpPrinter.Location = New System.Drawing.Point(10, 98)
            Me.grpPrinter.Name = "grpPrinter"
            Me.grpPrinter.Size = New System.Drawing.Size(260, 155)
            Me.grpPrinter.TabIndex = 2
            Me.grpPrinter.TabStop = False
            Me.grpPrinter.Text = "تنظیمات چاپگر"
            '
            'lblPrinter
            '
            Me.lblPrinter.AutoSize = True
            Me.lblPrinter.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Regular)
            Me.lblPrinter.Location = New System.Drawing.Point(170, 25)
            Me.lblPrinter.Name = "lblPrinter"
            Me.lblPrinter.Size = New System.Drawing.Size(78, 14)
            Me.lblPrinter.TabIndex = 0
            Me.lblPrinter.Text = "انتخاب چاپگر:"
            '
            'cmbPrinter
            '
            Me.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPrinter.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbPrinter.FormattingEnabled = True
            Me.cmbPrinter.Location = New System.Drawing.Point(10, 44)
            Me.cmbPrinter.Name = "cmbPrinter"
            Me.cmbPrinter.Size = New System.Drawing.Size(240, 21)
            Me.cmbPrinter.TabIndex = 1
            '
            'btnPrinterProperties
            '
            Me.btnPrinterProperties.BackColor = System.Drawing.Color.White
            Me.btnPrinterProperties.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnPrinterProperties.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnPrinterProperties.Location = New System.Drawing.Point(10, 72)
            Me.btnPrinterProperties.Name = "btnPrinterProperties"
            Me.btnPrinterProperties.Size = New System.Drawing.Size(240, 28)
            Me.btnPrinterProperties.TabIndex = 2
            Me.btnPrinterProperties.Text = "⚙  تنظیمات درایور چاپگر..."
            Me.btnPrinterProperties.UseVisualStyleBackColor = True
            '
            'lblCopies
            '
            Me.lblCopies.AutoSize = True
            Me.lblCopies.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Regular)
            Me.lblCopies.Location = New System.Drawing.Point(150, 115)
            Me.lblCopies.Name = "lblCopies"
            Me.lblCopies.Size = New System.Drawing.Size(97, 14)
            Me.lblCopies.TabIndex = 3
            Me.lblCopies.Text = "تعداد نسخه (کپی):"
            '
            'numCopies
            '
            Me.numCopies.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.numCopies.Location = New System.Drawing.Point(10, 112)
            Me.numCopies.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numCopies.Name = "numCopies"
            Me.numCopies.Size = New System.Drawing.Size(130, 22)
            Me.numCopies.TabIndex = 4
            Me.numCopies.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'grpPage
            '
            Me.grpPage.Controls.Add(Me.btnPageSetup)
            Me.grpPage.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpPage.Location = New System.Drawing.Point(10, 260)
            Me.grpPage.Name = "grpPage"
            Me.grpPage.Size = New System.Drawing.Size(260, 75)
            Me.grpPage.TabIndex = 3
            Me.grpPage.TabStop = False
            Me.grpPage.Text = "تنظیمات کاغذ و حاشیه"
            '
            'btnPageSetup
            '
            Me.btnPageSetup.BackColor = System.Drawing.Color.White
            Me.btnPageSetup.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnPageSetup.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnPageSetup.Location = New System.Drawing.Point(10, 26)
            Me.btnPageSetup.Name = "btnPageSetup"
            Me.btnPageSetup.Size = New System.Drawing.Size(240, 35)
            Me.btnPageSetup.TabIndex = 0
            Me.btnPageSetup.Text = "📄  تنظیمات ابعاد کاغذ و حاشیه‌ها..."
            Me.btnPageSetup.UseVisualStyleBackColor = True
            '
            'grpZoomNav
            '
            Me.grpZoomNav.Controls.Add(Me.btnNextPage)
            Me.grpZoomNav.Controls.Add(Me.lblPageStatus)
            Me.grpZoomNav.Controls.Add(Me.btnPrevPage)
            Me.grpZoomNav.Controls.Add(Me.btnZoomFit)
            Me.grpZoomNav.Controls.Add(Me.btnZoomOut)
            Me.grpZoomNav.Controls.Add(Me.btnZoomIn)
            Me.grpZoomNav.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpZoomNav.Location = New System.Drawing.Point(10, 342)
            Me.grpZoomNav.Name = "grpZoomNav"
            Me.grpZoomNav.Size = New System.Drawing.Size(260, 115)
            Me.grpZoomNav.TabIndex = 4
            Me.grpZoomNav.TabStop = False
            Me.grpZoomNav.Text = "نمایش و بزرگنمایی"
            '
            'btnZoomIn
            '
            Me.btnZoomIn.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnZoomIn.Location = New System.Drawing.Point(175, 25)
            Me.btnZoomIn.Name = "btnZoomIn"
            Me.btnZoomIn.Size = New System.Drawing.Size(75, 28)
            Me.btnZoomIn.TabIndex = 0
            Me.btnZoomIn.Text = "🔍 +  بزرگ"
            Me.btnZoomIn.UseVisualStyleBackColor = True
            '
            'btnZoomOut
            '
            Me.btnZoomOut.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnZoomOut.Location = New System.Drawing.Point(92, 25)
            Me.btnZoomOut.Name = "btnZoomOut"
            Me.btnZoomOut.Size = New System.Drawing.Size(75, 28)
            Me.btnZoomOut.TabIndex = 1
            Me.btnZoomOut.Text = "🔍 -  کوچک"
            Me.btnZoomOut.UseVisualStyleBackColor = True
            '
            'btnZoomFit
            '
            Me.btnZoomFit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnZoomFit.Location = New System.Drawing.Point(10, 25)
            Me.btnZoomFit.Name = "btnZoomFit"
            Me.btnZoomFit.Size = New System.Drawing.Size(75, 28)
            Me.btnZoomFit.TabIndex = 2
            Me.btnZoomFit.Text = "اندازه صفحه"
            Me.btnZoomFit.UseVisualStyleBackColor = True
            '
            'btnPrevPage
            '
            Me.btnPrevPage.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnPrevPage.Location = New System.Drawing.Point(175, 68)
            Me.btnPrevPage.Name = "btnPrevPage"
            Me.btnPrevPage.Size = New System.Drawing.Size(75, 28)
            Me.btnPrevPage.TabIndex = 3
            Me.btnPrevPage.Text = "صفحه قبلی"
            Me.btnPrevPage.UseVisualStyleBackColor = True
            '
            'lblPageStatus
            '
            Me.lblPageStatus.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblPageStatus.Location = New System.Drawing.Point(90, 68)
            Me.lblPageStatus.Name = "lblPageStatus"
            Me.lblPageStatus.Size = New System.Drawing.Size(80, 28)
            Me.lblPageStatus.TabIndex = 4
            Me.lblPageStatus.Text = "صفحه ۱"
            Me.lblPageStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnNextPage
            '
            Me.btnNextPage.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnNextPage.Location = New System.Drawing.Point(10, 68)
            Me.btnNextPage.Name = "btnNextPage"
            Me.btnNextPage.Size = New System.Drawing.Size(75, 28)
            Me.btnNextPage.TabIndex = 5
            Me.btnNextPage.Text = "صفحه بعدی"
            Me.btnNextPage.UseVisualStyleBackColor = True
            '
            'btnClose
            '
            Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClose.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnClose.ForeColor = System.Drawing.Color.White
            Me.btnClose.Location = New System.Drawing.Point(10, 630)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(260, 38)
            Me.btnClose.TabIndex = 5
            Me.btnClose.Text = "❌  خروج (Close)"
            Me.btnClose.UseVisualStyleBackColor = False
            '
            'previewCtrl
            '
            Me.previewCtrl.AutoZoom = False
            Me.previewCtrl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.previewCtrl.Location = New System.Drawing.Point(0, 0)
            Me.previewCtrl.Name = "previewCtrl"
            Me.previewCtrl.Size = New System.Drawing.Size(740, 680)
            Me.previewCtrl.TabIndex = 1
            Me.previewCtrl.Zoom = 0.75D
            '
            'ReportPrintPreviewForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1020, 680)
            Me.Controls.Add(Me.previewCtrl)
            Me.Controls.Add(Me.pnlLeft)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.Name = "ReportPrintPreviewForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "پیش‌نمایش و تنظیمات چاپ گزارش"
            Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
            Me.pnlLeft.ResumeLayout(False)
            Me.grpPage.ResumeLayout(False)
            Me.grpPrinter.ResumeLayout(False)
            Me.grpPrinter.PerformLayout()
            CType(Me.numCopies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpZoomNav.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents pnlLeft As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents btnPrint As System.Windows.Forms.Button
        Friend WithEvents grpPrinter As System.Windows.Forms.GroupBox
        Friend WithEvents lblPrinter As System.Windows.Forms.Label
        Friend WithEvents cmbPrinter As System.Windows.Forms.ComboBox
        Friend WithEvents btnPrinterProperties As System.Windows.Forms.Button
        Friend WithEvents lblCopies As System.Windows.Forms.Label
        Friend WithEvents numCopies As System.Windows.Forms.NumericUpDown
        Friend WithEvents grpPage As System.Windows.Forms.GroupBox
        Friend WithEvents btnPageSetup As System.Windows.Forms.Button
        Friend WithEvents grpZoomNav As System.Windows.Forms.GroupBox
        Friend WithEvents btnZoomIn As System.Windows.Forms.Button
        Friend WithEvents btnZoomOut As System.Windows.Forms.Button
        Friend WithEvents btnZoomFit As System.Windows.Forms.Button
        Friend WithEvents btnPrevPage As System.Windows.Forms.Button
        Friend WithEvents lblPageStatus As System.Windows.Forms.Label
        Friend WithEvents btnNextPage As System.Windows.Forms.Button
        Friend WithEvents grpSign As System.Windows.Forms.GroupBox
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents previewCtrl As System.Windows.Forms.PrintPreviewControl
        Friend WithEvents dialogPageSetup As System.Windows.Forms.PageSetupDialog
        Friend WithEvents dialogPrint As System.Windows.Forms.PrintDialog
    End Class
End Namespace
