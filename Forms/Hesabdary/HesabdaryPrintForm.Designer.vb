Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class HesabdaryPrintForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.splitPrint = New System.Windows.Forms.SplitContainer()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.btnPrint = New System.Windows.Forms.Button()
            Me.lblPrinter = New System.Windows.Forms.Label()
            Me.cmbPrinter = New System.Windows.Forms.ComboBox()
            Me.lblCopies = New System.Windows.Forms.Label()
            Me.numCopies = New System.Windows.Forms.NumericUpDown()
            Me.btnPageSetup = New System.Windows.Forms.Button()
            Me.grpLevels = New System.Windows.Forms.GroupBox()
            Me.btnReload = New System.Windows.Forms.Button()
            Me.chkDetail2 = New System.Windows.Forms.CheckBox()
            Me.chkDetail1 = New System.Windows.Forms.CheckBox()
            Me.chkSubsidiary = New System.Windows.Forms.CheckBox()
            Me.chkGeneral = New System.Windows.Forms.CheckBox()
            Me.chkGroup = New System.Windows.Forms.CheckBox()
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
            Me.grpLevels.SuspendLayout()
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
            Me.splitPrint.Panel1.Controls.Add(Me.btnPageSetup)
            Me.splitPrint.Panel1.Controls.Add(Me.grpLevels)
            Me.splitPrint.Panel1.Controls.Add(Me.grpZoomNav)
            Me.splitPrint.Panel1.Controls.Add(Me.btnExit)
            Me.splitPrint.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'splitPrint.Panel2
            '
            Me.splitPrint.Panel2.Controls.Add(Me.previewCtrl)
            Me.splitPrint.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.splitPrint.Size = New System.Drawing.Size(1000, 700)
            Me.splitPrint.SplitterDistance = 300
            Me.splitPrint.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(12, 15)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(276, 25)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "تنظیمات چاپ سند"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnPrint
            '
            Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
            Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPrint.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
            Me.btnPrint.ForeColor = System.Drawing.Color.White
            Me.btnPrint.Location = New System.Drawing.Point(12, 55)
            Me.btnPrint.Name = "btnPrint"
            Me.btnPrint.Size = New System.Drawing.Size(276, 45)
            Me.btnPrint.TabIndex = 1
            Me.btnPrint.Text = "🖨  چاپ سند (Print)"
            Me.btnPrint.UseVisualStyleBackColor = False
            '
            'lblPrinter
            '
            Me.lblPrinter.AutoSize = True
            Me.lblPrinter.Location = New System.Drawing.Point(230, 120)
            Me.lblPrinter.Name = "lblPrinter"
            Me.lblPrinter.Size = New System.Drawing.Size(37, 14)
            Me.lblPrinter.TabIndex = 2
            Me.lblPrinter.Text = "چاپگر:"
            '
            'cmbPrinter
            '
            Me.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPrinter.FormattingEnabled = True
            Me.cmbPrinter.Location = New System.Drawing.Point(12, 140)
            Me.cmbPrinter.Name = "cmbPrinter"
            Me.cmbPrinter.Size = New System.Drawing.Size(276, 22)
            Me.cmbPrinter.TabIndex = 3
            '
            'lblCopies
            '
            Me.lblCopies.AutoSize = True
            Me.lblCopies.Location = New System.Drawing.Point(220, 180)
            Me.lblCopies.Name = "lblCopies"
            Me.lblCopies.Size = New System.Drawing.Size(62, 14)
            Me.lblCopies.TabIndex = 4
            Me.lblCopies.Text = "تعداد کپی:"
            '
            'numCopies
            '
            Me.numCopies.Location = New System.Drawing.Point(12, 200)
            Me.numCopies.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numCopies.Name = "numCopies"
            Me.numCopies.Size = New System.Drawing.Size(276, 22)
            Me.numCopies.TabIndex = 5
            Me.numCopies.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'btnPageSetup
            '
            Me.btnPageSetup.Location = New System.Drawing.Point(12, 240)
            Me.btnPageSetup.Name = "btnPageSetup"
            Me.btnPageSetup.Size = New System.Drawing.Size(276, 30)
            Me.btnPageSetup.TabIndex = 6
            Me.btnPageSetup.Text = "⚙  تنظیمات صفحه (Page Setup)"
            Me.btnPageSetup.UseVisualStyleBackColor = True
            '
            'grpLevels
            '
            Me.grpLevels.Controls.Add(Me.btnReload)
            Me.grpLevels.Controls.Add(Me.chkDetail2)
            Me.grpLevels.Controls.Add(Me.chkDetail1)
            Me.grpLevels.Controls.Add(Me.chkSubsidiary)
            Me.grpLevels.Controls.Add(Me.chkGeneral)
            Me.grpLevels.Controls.Add(Me.chkGroup)
            Me.grpLevels.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.grpLevels.Location = New System.Drawing.Point(12, 290)
            Me.grpLevels.Name = "grpLevels"
            Me.grpLevels.Size = New System.Drawing.Size(276, 180)
            Me.grpLevels.TabIndex = 7
            Me.grpLevels.TabStop = False
            Me.grpLevels.Text = "سطوح سرفصل جهت نمایش"
            '
            'btnReload
            '
            Me.btnReload.Location = New System.Drawing.Point(15, 70)
            Me.btnReload.Name = "btnReload"
            Me.btnReload.Size = New System.Drawing.Size(85, 40)
            Me.btnReload.TabIndex = 5
            Me.btnReload.Text = "بازخوانی"
            Me.btnReload.UseVisualStyleBackColor = True
            '
            'chkDetail2
            '
            Me.chkDetail2.AutoSize = True
            Me.chkDetail2.Location = New System.Drawing.Point(161, 140)
            Me.chkDetail2.Name = "chkDetail2"
            Me.chkDetail2.Size = New System.Drawing.Size(75, 18)
            Me.chkDetail2.TabIndex = 4
            Me.chkDetail2.Text = "تفضیلی 2"
            Me.chkDetail2.UseVisualStyleBackColor = True
            '
            'chkDetail1
            '
            Me.chkDetail1.AutoSize = True
            Me.chkDetail1.Location = New System.Drawing.Point(161, 112)
            Me.chkDetail1.Name = "chkDetail1"
            Me.chkDetail1.Size = New System.Drawing.Size(75, 18)
            Me.chkDetail1.TabIndex = 3
            Me.chkDetail1.Text = "تفضیلی 1"
            Me.chkDetail1.UseVisualStyleBackColor = True
            '
            'chkSubsidiary
            '
            Me.chkSubsidiary.AutoSize = True
            Me.chkSubsidiary.Location = New System.Drawing.Point(153, 83)
            Me.chkSubsidiary.Name = "chkSubsidiary"
            Me.chkSubsidiary.Size = New System.Drawing.Size(83, 18)
            Me.chkSubsidiary.TabIndex = 2
            Me.chkSubsidiary.Text = "سطح معین"
            Me.chkSubsidiary.UseVisualStyleBackColor = True
            '
            'chkGeneral
            '
            Me.chkGeneral.AutoSize = True
            Me.chkGeneral.Location = New System.Drawing.Point(163, 55)
            Me.chkGeneral.Name = "chkGeneral"
            Me.chkGeneral.Size = New System.Drawing.Size(73, 18)
            Me.chkGeneral.TabIndex = 1
            Me.chkGeneral.Text = "سطح کل"
            Me.chkGeneral.UseVisualStyleBackColor = True
            '
            'chkGroup
            '
            Me.chkGroup.AutoSize = True
            Me.chkGroup.Location = New System.Drawing.Point(156, 28)
            Me.chkGroup.Name = "chkGroup"
            Me.chkGroup.Size = New System.Drawing.Size(80, 18)
            Me.chkGroup.TabIndex = 0
            Me.chkGroup.Text = "سطح گروه"
            Me.chkGroup.UseVisualStyleBackColor = True
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
            Me.grpZoomNav.Location = New System.Drawing.Point(12, 480)
            Me.grpZoomNav.Name = "grpZoomNav"
            Me.grpZoomNav.Size = New System.Drawing.Size(276, 140)
            Me.grpZoomNav.TabIndex = 8
            Me.grpZoomNav.TabStop = False
            Me.grpZoomNav.Text = "بزرگنمایی و ناوبری صفحات"
            '
            'btnZoomOut
            '
            Me.btnZoomOut.Location = New System.Drawing.Point(15, 25)
            Me.btnZoomOut.Name = "btnZoomOut"
            Me.btnZoomOut.Size = New System.Drawing.Size(40, 25)
            Me.btnZoomOut.TabIndex = 0
            Me.btnZoomOut.Text = "🔍-"
            Me.btnZoomOut.UseVisualStyleBackColor = True
            '
            'lblZoomValue
            '
            Me.lblZoomValue.Location = New System.Drawing.Point(60, 27)
            Me.lblZoomValue.Name = "lblZoomValue"
            Me.lblZoomValue.Size = New System.Drawing.Size(60, 20)
            Me.lblZoomValue.TabIndex = 1
            Me.lblZoomValue.Text = "100%"
            Me.lblZoomValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnZoomIn
            '
            Me.btnZoomIn.Location = New System.Drawing.Point(125, 25)
            Me.btnZoomIn.Name = "btnZoomIn"
            Me.btnZoomIn.Size = New System.Drawing.Size(40, 25)
            Me.btnZoomIn.TabIndex = 2
            Me.btnZoomIn.Text = "🔍+"
            Me.btnZoomIn.UseVisualStyleBackColor = True
            '
            'btnZoomFit
            '
            Me.btnZoomFit.Location = New System.Drawing.Point(170, 25)
            Me.btnZoomFit.Name = "btnZoomFit"
            Me.btnZoomFit.Size = New System.Drawing.Size(90, 25)
            Me.btnZoomFit.TabIndex = 3
            Me.btnZoomFit.Text = "اندازه خودکار"
            Me.btnZoomFit.UseVisualStyleBackColor = True
            '
            'btnFirstPage
            '
            Me.btnFirstPage.Location = New System.Drawing.Point(14, 75)
            Me.btnFirstPage.Name = "btnFirstPage"
            Me.btnFirstPage.Size = New System.Drawing.Size(38, 28)
            Me.btnFirstPage.TabIndex = 4
            Me.btnFirstPage.Text = "▶▶"
            Me.btnFirstPage.UseVisualStyleBackColor = True
            '
            'btnPrevPage
            '
            Me.btnPrevPage.Location = New System.Drawing.Point(56, 75)
            Me.btnPrevPage.Name = "btnPrevPage"
            Me.btnPrevPage.Size = New System.Drawing.Size(35, 28)
            Me.btnPrevPage.TabIndex = 5
            Me.btnPrevPage.Text = "▶"
            Me.btnPrevPage.UseVisualStyleBackColor = True
            '
            'lblPageStatus
            '
            Me.lblPageStatus.Location = New System.Drawing.Point(90, 79)
            Me.lblPageStatus.Name = "lblPageStatus"
            Me.lblPageStatus.Size = New System.Drawing.Size(95, 20)
            Me.lblPageStatus.TabIndex = 6
            Me.lblPageStatus.Text = "صفحه ۱ از ۱"
            Me.lblPageStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnNextPage
            '
            Me.btnNextPage.Location = New System.Drawing.Point(185, 75)
            Me.btnNextPage.Name = "btnNextPage"
            Me.btnNextPage.Size = New System.Drawing.Size(35, 28)
            Me.btnNextPage.TabIndex = 7
            Me.btnNextPage.Text = "◀"
            Me.btnNextPage.UseVisualStyleBackColor = True
            '
            'btnLastPage
            '
            Me.btnLastPage.Location = New System.Drawing.Point(224, 75)
            Me.btnLastPage.Name = "btnLastPage"
            Me.btnLastPage.Size = New System.Drawing.Size(38, 28)
            Me.btnLastPage.TabIndex = 8
            Me.btnLastPage.Text = "◀◀"
            Me.btnLastPage.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnExit.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnExit.Location = New System.Drawing.Point(12, 640)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(276, 35)
            Me.btnExit.TabIndex = 9
            Me.btnExit.Text = "↩  بازگشت (Back)"
            Me.btnExit.UseVisualStyleBackColor = False
            '
            'previewCtrl
            '
            Me.previewCtrl.AutoZoom = False
            Me.previewCtrl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.previewCtrl.Document = Me.printDoc
            Me.previewCtrl.Location = New System.Drawing.Point(0, 0)
            Me.previewCtrl.Name = "previewCtrl"
            Me.previewCtrl.Size = New System.Drawing.Size(696, 700)
            Me.previewCtrl.TabIndex = 0
            Me.previewCtrl.Zoom = 0.60650128314799R
            '
            'printDoc
            '
            '
            'dialogPageSetup
            '
            Me.dialogPageSetup.Document = Me.printDoc
            '
            'HesabdaryPrintForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1000, 700)
            Me.Controls.Add(Me.splitPrint)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryPrintForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "پیش‌نمایش چاپ سند حسابداری"
            Me.splitPrint.Panel1.ResumeLayout(False)
            Me.splitPrint.Panel1.PerformLayout()
            Me.splitPrint.Panel2.ResumeLayout(False)
            CType(Me.splitPrint, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitPrint.ResumeLayout(False)
            CType(Me.numCopies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpLevels.ResumeLayout(False)
            Me.grpLevels.PerformLayout()
            Me.grpZoomNav.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents splitPrint As System.Windows.Forms.SplitContainer
        Private WithEvents lblTitle As System.Windows.Forms.Label
        Private WithEvents btnPrint As System.Windows.Forms.Button
        Private WithEvents lblPrinter As System.Windows.Forms.Label
        Private WithEvents cmbPrinter As System.Windows.Forms.ComboBox
        Private WithEvents lblCopies As System.Windows.Forms.Label
        Private WithEvents numCopies As System.Windows.Forms.NumericUpDown
        Private WithEvents btnPageSetup As System.Windows.Forms.Button
        Private WithEvents grpLevels As System.Windows.Forms.GroupBox
        Private WithEvents chkDetail2 As System.Windows.Forms.CheckBox
        Private WithEvents chkDetail1 As System.Windows.Forms.CheckBox
        Private WithEvents chkSubsidiary As System.Windows.Forms.CheckBox
        Private WithEvents chkGeneral As System.Windows.Forms.CheckBox
        Private WithEvents chkGroup As System.Windows.Forms.CheckBox
        Private WithEvents grpZoomNav As System.Windows.Forms.GroupBox
        Private WithEvents btnZoomOut As System.Windows.Forms.Button
        Private WithEvents lblZoomValue As System.Windows.Forms.Label
        Private WithEvents btnZoomIn As System.Windows.Forms.Button
        Private WithEvents btnZoomFit As System.Windows.Forms.Button
        Private WithEvents btnFirstPage As System.Windows.Forms.Button
        Private WithEvents btnPrevPage As System.Windows.Forms.Button
        Private WithEvents lblPageStatus As System.Windows.Forms.Label
        Private WithEvents btnNextPage As System.Windows.Forms.Button
        Private WithEvents btnLastPage As System.Windows.Forms.Button
        Private WithEvents btnExit As System.Windows.Forms.Button
        Private WithEvents previewCtrl As System.Windows.Forms.PrintPreviewControl
        Private WithEvents printDoc As System.Drawing.Printing.PrintDocument
        Private WithEvents dialogPageSetup As System.Windows.Forms.PageSetupDialog
        Private WithEvents btnReload As System.Windows.Forms.Button
    End Class
End Namespace
