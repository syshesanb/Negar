Namespace Negar.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class HesabdaryTarazPrintForm
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
            Me.splitPrint = New System.Windows.Forms.SplitContainer()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.btnPrint = New System.Windows.Forms.Button()
            Me.lblPrinter = New System.Windows.Forms.Label()
            Me.cmbPrinter = New System.Windows.Forms.ComboBox()
            Me.lblCopies = New System.Windows.Forms.Label()
            Me.numCopies = New System.Windows.Forms.NumericUpDown()
            Me.btnOrientation = New System.Windows.Forms.Button()
            Me.btnPageSetup = New System.Windows.Forms.Button()
            Me.grpZoomNav = New System.Windows.Forms.GroupBox()
            Me.btnZoomOut = New System.Windows.Forms.Button()
            Me.lblZoomValue = New System.Windows.Forms.Label()
            Me.btnZoomIn = New System.Windows.Forms.Button()
            Me.btnZoomFit = New System.Windows.Forms.Button()
            Me.btnFirstPage = New System.Windows.Forms.Button()
            Me.btnPrevPage = New System.Windows.Forms.Button()
            Me.lblPageStatus = New System.Windows.Forms.Label()
            Me.btnNextPage = New System.Windows.Forms.Button()
            Me.btnLastPage = New System.Windows.Forms.Button()
            Me.btnExit = New System.Windows.Forms.Button()
            Me.previewCtrl = New System.Windows.Forms.PrintPreviewControl()
            Me.printDoc = New System.Drawing.Printing.PrintDocument()
            Me.dialogPageSetup = New System.Windows.Forms.PageSetupDialog()
            CType(Me.splitPrint, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitPrint.Panel1.SuspendLayout()
            Me.splitPrint.Panel2.SuspendLayout()
            Me.splitPrint.SuspendLayout()
            CType(Me.numCopies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpZoomNav.SuspendLayout()
            Me.SuspendLayout()
            '
            'splitPrint
            '
            Me.splitPrint.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitPrint.Location = New System.Drawing.Point(0, 0)
            Me.splitPrint.Name = "splitPrint"
            '
            'splitPrint.Panel1
            '
            Me.splitPrint.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.splitPrint.Panel1.Controls.Add(Me.lblTitle)
            Me.splitPrint.Panel1.Controls.Add(Me.btnPrint)
            Me.splitPrint.Panel1.Controls.Add(Me.lblPrinter)
            Me.splitPrint.Panel1.Controls.Add(Me.cmbPrinter)
            Me.splitPrint.Panel1.Controls.Add(Me.lblCopies)
            Me.splitPrint.Panel1.Controls.Add(Me.numCopies)
            Me.splitPrint.Panel1.Controls.Add(Me.btnOrientation)
            Me.splitPrint.Panel1.Controls.Add(Me.btnPageSetup)
            Me.splitPrint.Panel1.Controls.Add(Me.grpZoomNav)
            Me.splitPrint.Panel1.Controls.Add(Me.btnExit)
            Me.splitPrint.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'splitPrint.Panel2
            '
            Me.splitPrint.Panel2.Controls.Add(Me.previewCtrl)
            Me.splitPrint.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.splitPrint.Size = New System.Drawing.Size(1050, 720)
            Me.splitPrint.SplitterDistance = 280
            Me.splitPrint.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(10, 15)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(260, 25)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "تنظیمات چاپ تراز آزمایشی"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnPrint
            '
            Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
            Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPrint.Font = New System.Drawing.Font("Tahoma", 10.5!, System.Drawing.FontStyle.Bold)
            Me.btnPrint.ForeColor = System.Drawing.Color.White
            Me.btnPrint.Location = New System.Drawing.Point(10, 50)
            Me.btnPrint.Name = "btnPrint"
            Me.btnPrint.Size = New System.Drawing.Size(260, 42)
            Me.btnPrint.TabIndex = 1
            Me.btnPrint.Text = "🖨  چاپ تراز آزمایشی"
            Me.btnPrint.UseVisualStyleBackColor = False
            '
            'lblPrinter
            '
            Me.lblPrinter.AutoSize = True
            Me.lblPrinter.Location = New System.Drawing.Point(214, 108)
            Me.lblPrinter.Name = "lblPrinter"
            Me.lblPrinter.Size = New System.Drawing.Size(56, 14)
            Me.lblPrinter.TabIndex = 2
            Me.lblPrinter.Text = "چاپگر:"
            '
            'cmbPrinter
            '
            Me.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPrinter.FormattingEnabled = True
            Me.cmbPrinter.Location = New System.Drawing.Point(10, 126)
            Me.cmbPrinter.Name = "cmbPrinter"
            Me.cmbPrinter.Size = New System.Drawing.Size(260, 22)
            Me.cmbPrinter.TabIndex = 3
            '
            'lblCopies
            '
            Me.lblCopies.AutoSize = True
            Me.lblCopies.Location = New System.Drawing.Point(204, 162)
            Me.lblCopies.Name = "lblCopies"
            Me.lblCopies.Size = New System.Drawing.Size(66, 14)
            Me.lblCopies.TabIndex = 4
            Me.lblCopies.Text = "تعداد کپی:"
            '
            'numCopies
            '
            Me.numCopies.Location = New System.Drawing.Point(10, 180)
            Me.numCopies.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numCopies.Name = "numCopies"
            Me.numCopies.Size = New System.Drawing.Size(260, 22)
            Me.numCopies.TabIndex = 5
            Me.numCopies.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'btnOrientation
            '
            Me.btnOrientation.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.btnOrientation.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnOrientation.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnOrientation.Location = New System.Drawing.Point(10, 215)
            Me.btnOrientation.Name = "btnOrientation"
            Me.btnOrientation.Size = New System.Drawing.Size(260, 32)
            Me.btnOrientation.TabIndex = 6
            Me.btnOrientation.Text = "جهت کاغذ: عمودی (Portrait)"
            Me.btnOrientation.UseVisualStyleBackColor = False
            '
            'btnPageSetup
            '
            Me.btnPageSetup.Location = New System.Drawing.Point(10, 253)
            Me.btnPageSetup.Name = "btnPageSetup"
            Me.btnPageSetup.Size = New System.Drawing.Size(260, 30)
            Me.btnPageSetup.TabIndex = 7
            Me.btnPageSetup.Text = "تنظیمات صفحه (Page Setup)..."
            Me.btnPageSetup.UseVisualStyleBackColor = True
            '
            'grpZoomNav
            '
            Me.grpZoomNav.Controls.Add(Me.btnZoomOut)
            Me.grpZoomNav.Controls.Add(Me.lblZoomValue)
            Me.grpZoomNav.Controls.Add(Me.btnZoomIn)
            Me.grpZoomNav.Controls.Add(Me.btnZoomFit)
            Me.grpZoomNav.Controls.Add(Me.btnFirstPage)
            Me.grpZoomNav.Controls.Add(Me.btnPrevPage)
            Me.grpZoomNav.Controls.Add(Me.lblPageStatus)
            Me.grpZoomNav.Controls.Add(Me.btnNextPage)
            Me.grpZoomNav.Controls.Add(Me.btnLastPage)
            Me.grpZoomNav.Location = New System.Drawing.Point(10, 295)
            Me.grpZoomNav.Name = "grpZoomNav"
            Me.grpZoomNav.Size = New System.Drawing.Size(260, 160)
            Me.grpZoomNav.TabIndex = 8
            Me.grpZoomNav.TabStop = False
            Me.grpZoomNav.Text = "بزرگ‌نمایی و پیمایش"
            '
            'btnZoomOut
            '
            Me.btnZoomOut.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnZoomOut.Location = New System.Drawing.Point(10, 25)
            Me.btnZoomOut.Name = "btnZoomOut"
            Me.btnZoomOut.Size = New System.Drawing.Size(40, 30)
            Me.btnZoomOut.TabIndex = 0
            Me.btnZoomOut.Text = "-"
            Me.btnZoomOut.UseVisualStyleBackColor = True
            '
            'lblZoomValue
            '
            Me.lblZoomValue.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblZoomValue.Location = New System.Drawing.Point(56, 25)
            Me.lblZoomValue.Name = "lblZoomValue"
            Me.lblZoomValue.Size = New System.Drawing.Size(68, 30)
            Me.lblZoomValue.TabIndex = 1
            Me.lblZoomValue.Text = "خودکار"
            Me.lblZoomValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnZoomIn
            '
            Me.btnZoomIn.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnZoomIn.Location = New System.Drawing.Point(130, 25)
            Me.btnZoomIn.Name = "btnZoomIn"
            Me.btnZoomIn.Size = New System.Drawing.Size(40, 30)
            Me.btnZoomIn.TabIndex = 2
            Me.btnZoomIn.Text = "+"
            Me.btnZoomIn.UseVisualStyleBackColor = True
            '
            'btnZoomFit
            '
            Me.btnZoomFit.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.btnZoomFit.Location = New System.Drawing.Point(176, 25)
            Me.btnZoomFit.Name = "btnZoomFit"
            Me.btnZoomFit.Size = New System.Drawing.Size(74, 30)
            Me.btnZoomFit.TabIndex = 3
            Me.btnZoomFit.Text = "اندازه صفحه"
            Me.btnZoomFit.UseVisualStyleBackColor = True
            '
            'btnFirstPage
            '
            Me.btnFirstPage.Location = New System.Drawing.Point(10, 110)
            Me.btnFirstPage.Name = "btnFirstPage"
            Me.btnFirstPage.Size = New System.Drawing.Size(40, 30)
            Me.btnFirstPage.TabIndex = 4
            Me.btnFirstPage.Text = "▶▶"
            Me.btnFirstPage.UseVisualStyleBackColor = True
            '
            'btnPrevPage
            '
            Me.btnPrevPage.Location = New System.Drawing.Point(56, 110)
            Me.btnPrevPage.Name = "btnPrevPage"
            Me.btnPrevPage.Size = New System.Drawing.Size(40, 30)
            Me.btnPrevPage.TabIndex = 5
            Me.btnPrevPage.Text = "▶"
            Me.btnPrevPage.UseVisualStyleBackColor = True
            '
            'lblPageStatus
            '
            Me.lblPageStatus.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblPageStatus.Location = New System.Drawing.Point(102, 110)
            Me.lblPageStatus.Name = "lblPageStatus"
            Me.lblPageStatus.Size = New System.Drawing.Size(56, 30)
            Me.lblPageStatus.TabIndex = 6
            Me.lblPageStatus.Text = "۱ از ۱"
            Me.lblPageStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnNextPage
            '
            Me.btnNextPage.Location = New System.Drawing.Point(164, 110)
            Me.btnNextPage.Name = "btnNextPage"
            Me.btnNextPage.Size = New System.Drawing.Size(40, 30)
            Me.btnNextPage.TabIndex = 7
            Me.btnNextPage.Text = "◀"
            Me.btnNextPage.UseVisualStyleBackColor = True
            '
            'btnLastPage
            '
            Me.btnLastPage.Location = New System.Drawing.Point(210, 110)
            Me.btnLastPage.Name = "btnLastPage"
            Me.btnLastPage.Size = New System.Drawing.Size(40, 30)
            Me.btnLastPage.TabIndex = 8
            Me.btnLastPage.Text = "◀◀"
            Me.btnLastPage.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnExit.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnExit.ForeColor = System.Drawing.Color.White
            Me.btnExit.Location = New System.Drawing.Point(10, 670)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(260, 38)
            Me.btnExit.TabIndex = 9
            Me.btnExit.Text = "خروج"
            Me.btnExit.UseVisualStyleBackColor = False
            '
            'previewCtrl
            '
            Me.previewCtrl.AutoZoom = True
            Me.previewCtrl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.previewCtrl.Document = Me.printDoc
            Me.previewCtrl.Location = New System.Drawing.Point(0, 0)
            Me.previewCtrl.Name = "previewCtrl"
            Me.previewCtrl.Size = New System.Drawing.Size(766, 720)
            Me.previewCtrl.TabIndex = 0
            '
            'dialogPageSetup
            '
            Me.dialogPageSetup.Document = Me.printDoc
            '
            'HesabdaryTarazPrintForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1050, 720)
            Me.Controls.Add(Me.splitPrint)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryTarazPrintForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "پیش‌نمایش و چاپ تراز آزمایشی"
            Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
            Me.splitPrint.Panel1.ResumeLayout(False)
            Me.splitPrint.Panel1.PerformLayout()
            Me.splitPrint.Panel2.ResumeLayout(False)
            CType(Me.splitPrint, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitPrint.ResumeLayout(False)
            CType(Me.numCopies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpZoomNav.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents splitPrint As System.Windows.Forms.SplitContainer
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents btnPrint As System.Windows.Forms.Button
        Friend WithEvents lblPrinter As System.Windows.Forms.Label
        Friend WithEvents cmbPrinter As System.Windows.Forms.ComboBox
        Friend WithEvents lblCopies As System.Windows.Forms.Label
        Friend WithEvents numCopies As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnOrientation As System.Windows.Forms.Button
        Friend WithEvents btnPageSetup As System.Windows.Forms.Button
        Friend WithEvents grpZoomNav As System.Windows.Forms.GroupBox
        Friend WithEvents btnZoomOut As System.Windows.Forms.Button
        Friend WithEvents lblZoomValue As System.Windows.Forms.Label
        Friend WithEvents btnZoomIn As System.Windows.Forms.Button
        Friend WithEvents btnZoomFit As System.Windows.Forms.Button
        Friend WithEvents btnFirstPage As System.Windows.Forms.Button
        Friend WithEvents btnPrevPage As System.Windows.Forms.Button
        Friend WithEvents lblPageStatus As System.Windows.Forms.Label
        Friend WithEvents btnNextPage As System.Windows.Forms.Button
        Friend WithEvents btnLastPage As System.Windows.Forms.Button
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents previewCtrl As System.Windows.Forms.PrintPreviewControl
        Friend WithEvents printDoc As System.Drawing.Printing.PrintDocument
        Friend WithEvents dialogPageSetup As System.Windows.Forms.PageSetupDialog
    End Class
End Namespace
