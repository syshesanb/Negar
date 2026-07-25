Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryKharid2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabMain As TabControl
        Friend WithEvents tabPageSanad As TabPage
        Friend WithEvents tabPageZamayem As TabPage
        Friend WithEvents tabPageYaddasht As TabPage
        Friend WithEvents tabPageTasvieh As TabPage

        ' کنترل‌های تب یادداشت فاکتور
        Friend WithEvents pnlNoteHeader As Panel
        Friend WithEvents lblNoteHeader As Label
        Friend WithEvents pnlEntryNote As Panel
        Friend WithEvents lblEntryNoteTitle As Label
        Friend WithEvents dgvEntryNotes As DataGridView
        Friend WithEvents pnlEntryNoteInput As Panel
        Friend WithEvents lblEntryNoteInputTitle As Label
        Friend WithEvents txtEntryNote As TextBox
        Friend WithEvents pnlEntryNoteAction As Panel
        Friend WithEvents btnSaveEntryNote As Button
        Friend WithEvents lblEntryNoteInfo As Label
        Friend WithEvents pnlLineNote As Panel
        Friend WithEvents lblLineNoteTitle As Label
        Friend WithEvents dgvLineNotes As DataGridView
        Friend WithEvents pnlLineNoteInput As Panel
        Friend WithEvents lblLineNoteInputTitle As Label
        Friend WithEvents txtLineNote As TextBox
        Friend WithEvents pnlLineNoteAction As Panel
        Friend WithEvents btnSaveLineNote As Button
        Friend WithEvents lblLineNoteInfo As Label

        ' کنترل‌های تب ضمائم فاکتور
        Friend WithEvents pnlZamHeader As Panel
        Friend WithEvents lblSelectedLine As Label
        Friend WithEvents pnlZamButtons As Panel
        Friend WithEvents btnZamAddFile As Button
        Friend WithEvents btnZamScan As Button
        Friend WithEvents btnZamView As Button
        Friend WithEvents btnZamPrint As Button
        Friend WithEvents btnZamDelete As Button
        Friend WithEvents pnlZamList As Panel
        Friend WithEvents dgvAttachments As DataGridView
        Friend WithEvents pnlZamPreview As Panel
        Friend WithEvents picAttachment As PictureBox

        Friend WithEvents pnlZamLineSelector As Panel
        Friend WithEvents lblZamLineSelectorTitle As Label
        Friend WithEvents dgvZamLineSelector As DataGridView

        Friend WithEvents pnlNoteLineSelector As Panel
        Friend WithEvents lblNoteLineSelectorTitle As Label
        Friend WithEvents dgvNoteLineSelector As DataGridView

        ' سربرگ سند فاکتور خرید
        Friend WithEvents pnlNoSanad As Panel
        Friend WithEvents lblEntryRef As Label
        Friend WithEvents txtEntryReference As TextBox
        Friend WithEvents lblVendorInvoiceNo As Label
        Friend WithEvents txtVendorInvoiceNumber As TextBox
        Friend WithEvents lblDateSanad As Label
        Friend WithEvents txtDateSanad As TextBox
        Friend WithEvents btnCalDate As Button
        Friend WithEvents lblSystemDate As Label
        Friend WithEvents txtSystemDate As TextBox
        Friend WithEvents btnCalSystemDate As Button
        Friend WithEvents lblTaxEntryMode As Label
        Friend WithEvents cmbTaxEntryMode As ComboBox

        ' نمایش فروشنده و انبار
        Friend WithEvents pnlViewSarfasl As Panel
        Friend WithEvents lblSarfaslTitle As Label
        Friend WithEvents btnSelectVendor As Button
        Friend WithEvents lblSarfaslValue As Label

        Friend WithEvents pnlViewShenavar As Panel
        Friend WithEvents lblShenavarTitle As Label
        Friend WithEvents lblShenavarValue As Label

        ' فیلتر بالای ستون‌های گرید
        Friend WithEvents pnlSerch As Panel
        Friend WithEvents txtSrcLineNo As TextBox
        Friend WithEvents txtSrcKala As TextBox
        Friend WithEvents txtSrcCode As TextBox
        Friend WithEvents txtSrcName As TextBox
        Friend WithEvents txtSrcBtnWarehouse As TextBox
        Friend WithEvents txtSrcWarehouse As TextBox
        Friend WithEvents txtSrcBtnUnit As TextBox
        Friend WithEvents txtSrcUnit As TextBox
        Friend WithEvents txtSrcQty As TextBox
        Friend WithEvents txtSrcUnitPrice As TextBox
        Friend WithEvents txtSrcLineTotal As TextBox
        Friend WithEvents txtSrcDiscount As TextBox
        Friend WithEvents txtSrcLineTotalAfterDiscount As TextBox
        Friend WithEvents txtSrcVat As TextBox
        Friend WithEvents txtSrcTotalPrice As TextBox

        ' گرید اصلی فاکتور خرید
        Friend WithEvents pnlDgv As Panel
        Friend WithEvents dgvEntryLines As DataGridView

        ' پانل مجموع زنده زیر گرید
        Friend WithEvents pnlTotalsRow As Panel
        Friend WithEvents txtSumLineTotal As TextBox
        Friend WithEvents txtSumDiscount As TextBox
        Friend WithEvents txtSumLineTotalAfterDiscount As TextBox
        Friend WithEvents txtSumVat As TextBox
        Friend WithEvents txtSumTotalPrice As TextBox

        ' خلاصه پایین و شرح سند
        Friend WithEvents pnlBottomRow As Panel
        Friend WithEvents pnlSharhSanad As Panel
        Friend WithEvents lblEntryDesc As Label
        Friend WithEvents txtEntryDescription As TextBox
        Friend WithEvents pnlJamSanad As Panel
        Friend WithEvents lblJamTitle As Label
        Friend WithEvents txtJamBedehkar As TextBox
        Friend WithEvents txtJamBestankar As TextBox
        Friend WithEvents lblTaeazLabel As Label
        Friend WithEvents lblSanadLabel As Label
        Friend WithEvents lblSanadStatus As Label
        Friend WithEvents lblKasriTitle As Label
        Friend WithEvents txtKasriDebit As TextBox
        Friend WithEvents lblTotalVatInput As Label
        Friend WithEvents txtTotalVatInput As TextBox

        ' نوار دکمه‌های پایین
        Friend WithEvents pnlButton As Panel
        Friend WithEvents btnSaveAndContinue As Button
        Friend WithEvents btnSaveEntry As Button
        Friend WithEvents btnAddLine As Button
        Friend WithEvents btnDeleteRow As Button
        Friend WithEvents btnCopyBelow As Button
        Friend WithEvents btnCopyAbove As Button
        Friend WithEvents btnCopyToPos As Button
        Friend WithEvents btnClearSearch As Button
        Friend WithEvents btnExit As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlNoteHeader = New Panel()
            Me.lblNoteHeader = New Label()
            Me.pnlEntryNote = New Panel()
            Me.dgvEntryNotes = New DataGridView()
            Me.pnlEntryNoteInput = New Panel()
            Me.txtEntryNote = New TextBox()
            Me.pnlEntryNoteAction = New Panel()
            Me.btnSaveEntryNote = New Button()
            Me.lblEntryNoteInfo = New Label()
            Me.lblEntryNoteInputTitle = New Label()
            Me.lblEntryNoteTitle = New Label()
            Me.pnlLineNote = New Panel()
            Me.dgvLineNotes = New DataGridView()
            Me.pnlLineNoteInput = New Panel()
            Me.txtLineNote = New TextBox()
            Me.pnlLineNoteAction = New Panel()
            Me.btnSaveLineNote = New Button()
            Me.lblLineNoteInfo = New Label()
            Me.lblLineNoteInputTitle = New Label()
            Me.lblLineNoteTitle = New Label()
            Me.tabMain = New TabControl()
            Me.tabPageSanad = New TabPage()
            Me.pnlDgv = New Panel()
            Me.dgvEntryLines = New DataGridView()
            Me.pnlTotalsRow = New Panel()
            Me.txtSumLineTotal = New TextBox()
            Me.txtSumDiscount = New TextBox()
            Me.txtSumLineTotalAfterDiscount = New TextBox()
            Me.txtSumVat = New TextBox()
            Me.txtSumTotalPrice = New TextBox()
            Me.pnlBottomRow = New Panel()
            Me.pnlJamSanad = New Panel()
            Me.lblJamTitle = New Label()
            Me.txtJamBedehkar = New TextBox()
            Me.txtJamBestankar = New TextBox()
            Me.lblTaeazLabel = New Label()
            Me.lblSanadLabel = New Label()
            Me.lblSanadStatus = New Label()
            Me.lblKasriTitle = New Label()
            Me.txtKasriDebit = New TextBox()
            Me.lblTotalVatInput = New Label()
            Me.txtTotalVatInput = New TextBox()
            Me.pnlSharhSanad = New Panel()
            Me.lblEntryDesc = New Label()
            Me.txtEntryDescription = New TextBox()
            Me.pnlButton = New Panel()
            Me.btnSaveAndContinue = New Button()
            Me.btnSaveEntry = New Button()
            Me.btnAddLine = New Button()
            Me.btnDeleteRow = New Button()
            Me.btnCopyBelow = New Button()
            Me.btnCopyAbove = New Button()
            Me.btnCopyToPos = New Button()
            Me.btnClearSearch = New Button()
            Me.btnExit = New Button()
            Me.pnlSerch = New Panel()
            Me.txtSrcLineNo = New TextBox()
            Me.txtSrcKala = New TextBox()
            Me.txtSrcCode = New TextBox()
            Me.txtSrcName = New TextBox()
            Me.txtSrcBtnWarehouse = New TextBox()
            Me.txtSrcWarehouse = New TextBox()
            Me.txtSrcBtnUnit = New TextBox()
            Me.txtSrcUnit = New TextBox()
            Me.txtSrcQty = New TextBox()
            Me.txtSrcUnitPrice = New TextBox()
            Me.txtSrcLineTotal = New TextBox()
            Me.txtSrcDiscount = New TextBox()
            Me.txtSrcLineTotalAfterDiscount = New TextBox()
            Me.txtSrcVat = New TextBox()
            Me.txtSrcTotalPrice = New TextBox()
            Me.pnlViewShenavar = New Panel()
            Me.lblShenavarValue = New Label()
            Me.lblShenavarTitle = New Label()
            Me.pnlViewSarfasl = New Panel()
            Me.btnSelectVendor = New Button()
            Me.lblSarfaslValue = New Label()
            Me.lblSarfaslTitle = New Label()
            Me.pnlNoSanad = New Panel()
            Me.lblEntryRef = New Label()
            Me.txtEntryReference = New TextBox()
            Me.lblVendorInvoiceNo = New Label()
            Me.txtVendorInvoiceNumber = New TextBox()
            Me.lblDateSanad = New Label()
            Me.txtDateSanad = New TextBox()
            Me.btnCalDate = New Button()
            Me.lblSystemDate = New Label()
            Me.txtSystemDate = New TextBox()
            Me.btnCalSystemDate = New Button()
            Me.lblTaxEntryMode = New Label()
            Me.cmbTaxEntryMode = New ComboBox()
            Me.tabPageZamayem = New TabPage()
            Me.pnlZamPreview = New Panel()
            Me.picAttachment = New PictureBox()
            Me.pnlZamList = New Panel()
            Me.dgvAttachments = New DataGridView()
            Me.pnlZamLineSelector = New Panel()
            Me.dgvZamLineSelector = New DataGridView()
            Me.lblZamLineSelectorTitle = New Label()
            Me.pnlZamButtons = New Panel()
            Me.btnZamAddFile = New Button()
            Me.btnZamScan = New Button()
            Me.btnZamView = New Button()
            Me.btnZamPrint = New Button()
            Me.btnZamDelete = New Button()
            Me.pnlZamHeader = New Panel()
            Me.lblSelectedLine = New Label()
            Me.tabPageYaddasht = New TabPage()
            Me.tabPageTasvieh = New TabPage()
            Me.pnlNoteLineSelector = New Panel()
            Me.dgvNoteLineSelector = New DataGridView()
            Me.lblNoteLineSelectorTitle = New Label()

            Me.pnlNoteHeader.SuspendLayout()
            Me.pnlEntryNote.SuspendLayout()
            CType(Me.dgvEntryNotes, ISupportInitialize).BeginInit()
            Me.pnlEntryNoteInput.SuspendLayout()
            Me.pnlEntryNoteAction.SuspendLayout()
            Me.pnlLineNote.SuspendLayout()
            CType(Me.dgvLineNotes, ISupportInitialize).BeginInit()
            Me.pnlLineNoteInput.SuspendLayout()
            Me.pnlLineNoteAction.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.tabPageSanad.SuspendLayout()
            Me.pnlDgv.SuspendLayout()
            CType(Me.dgvEntryLines, ISupportInitialize).BeginInit()
            Me.pnlTotalsRow.SuspendLayout()
            Me.pnlBottomRow.SuspendLayout()
            Me.pnlJamSanad.SuspendLayout()
            Me.pnlSharhSanad.SuspendLayout()
            Me.pnlButton.SuspendLayout()
            Me.pnlSerch.SuspendLayout()
            Me.pnlViewShenavar.SuspendLayout()
            Me.pnlViewSarfasl.SuspendLayout()
            Me.pnlNoSanad.SuspendLayout()
            Me.tabPageZamayem.SuspendLayout()
            Me.pnlZamPreview.SuspendLayout()
            CType(Me.picAttachment, ISupportInitialize).BeginInit()
            Me.pnlZamList.SuspendLayout()
            CType(Me.dgvAttachments, ISupportInitialize).BeginInit()
            Me.pnlZamLineSelector.SuspendLayout()
            CType(Me.dgvZamLineSelector, ISupportInitialize).BeginInit()
            Me.pnlZamButtons.SuspendLayout()
            Me.pnlZamHeader.SuspendLayout()
            Me.tabPageYaddasht.SuspendLayout()
            Me.pnlNoteLineSelector.SuspendLayout()
            CType(Me.dgvNoteLineSelector, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            ' tabMain
            Me.tabMain.Controls.Add(Me.tabPageSanad)
            Me.tabMain.Controls.Add(Me.tabPageTasvieh)
            Me.tabMain.Controls.Add(Me.tabPageZamayem)
            Me.tabMain.Controls.Add(Me.tabPageYaddasht)
            Me.tabMain.Dock = DockStyle.Fill
            Me.tabMain.Location = New Point(0, 0)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.RightToLeft = RightToLeft.Yes
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New Size(1320, 749)
            Me.tabMain.TabIndex = 0

            ' tabPageSanad
            Me.tabPageSanad.Controls.Add(Me.pnlDgv)
            Me.tabPageSanad.Controls.Add(Me.pnlTotalsRow)
            Me.tabPageSanad.Controls.Add(Me.pnlBottomRow)
            Me.tabPageSanad.Controls.Add(Me.pnlButton)
            Me.tabPageSanad.Controls.Add(Me.pnlSerch)
            Me.tabPageSanad.Controls.Add(Me.pnlViewShenavar)
            Me.tabPageSanad.Controls.Add(Me.pnlViewSarfasl)
            Me.tabPageSanad.Controls.Add(Me.pnlNoSanad)
            Me.tabPageSanad.Location = New Point(4, 23)
            Me.tabPageSanad.Name = "tabPageSanad"
            Me.tabPageSanad.Size = New Size(1312, 722)
            Me.tabPageSanad.TabIndex = 0
            Me.tabPageSanad.Text = "سطرهای فاکتور خرید"
            Me.tabPageSanad.UseVisualStyleBackColor = True

            ' pnlNoSanad
            Me.pnlNoSanad.Controls.Add(Me.lblEntryRef)
            Me.pnlNoSanad.Controls.Add(Me.txtEntryReference)
            Me.pnlNoSanad.Controls.Add(Me.lblVendorInvoiceNo)
            Me.pnlNoSanad.Controls.Add(Me.txtVendorInvoiceNumber)
            Me.pnlNoSanad.Controls.Add(Me.lblDateSanad)
            Me.pnlNoSanad.Controls.Add(Me.txtDateSanad)
            Me.pnlNoSanad.Controls.Add(Me.btnCalDate)
            Me.pnlNoSanad.Controls.Add(Me.lblSystemDate)
            Me.pnlNoSanad.Controls.Add(Me.txtSystemDate)
            Me.pnlNoSanad.Controls.Add(Me.btnCalSystemDate)
            Me.pnlNoSanad.Controls.Add(Me.lblTaxEntryMode)
            Me.pnlNoSanad.Controls.Add(Me.cmbTaxEntryMode)
            Me.pnlNoSanad.Dock = DockStyle.Top
            Me.pnlNoSanad.Location = New Point(0, 0)
            Me.pnlNoSanad.Name = "pnlNoSanad"
            Me.pnlNoSanad.Size = New Size(1312, 34)
            Me.pnlNoSanad.TabIndex = 0

            Me.lblEntryRef.Font = New Font("Tahoma", 9.0!)
            Me.lblEntryRef.Location = New Point(1140, 6)
            Me.lblEntryRef.Name = "lblEntryRef"
            Me.lblEntryRef.Size = New Size(165, 22)
            Me.lblEntryRef.Text = "شماره فاکتور خرید در سیستم:"
            Me.lblEntryRef.TextAlign = ContentAlignment.MiddleLeft
            Me.lblEntryRef.Visible = False

            Me.txtEntryReference.Location = New Point(1015, 6)
            Me.txtEntryReference.Name = "txtEntryReference"
            Me.txtEntryReference.Size = New Size(120, 22)
            Me.txtEntryReference.TabIndex = 0
            Me.txtEntryReference.Visible = False

            Me.lblVendorInvoiceNo.Font = New Font("Tahoma", 9.0!)
            Me.lblVendorInvoiceNo.Location = New Point(1145, 6)
            Me.lblVendorInvoiceNo.Name = "lblVendorInvoiceNo"
            Me.lblVendorInvoiceNo.Size = New Size(160, 22)
            Me.lblVendorInvoiceNo.Text = "شماره فاکتور خرید:"
            Me.lblVendorInvoiceNo.TextAlign = ContentAlignment.MiddleLeft

            Me.txtVendorInvoiceNumber.Location = New Point(880, 6)
            Me.txtVendorInvoiceNumber.Name = "txtVendorInvoiceNumber"
            Me.txtVendorInvoiceNumber.Size = New Size(260, 22)
            Me.txtVendorInvoiceNumber.TabIndex = 1

            Me.lblDateSanad.Font = New Font("Tahoma", 9.0!)
            Me.lblDateSanad.Location = New Point(780, 6)
            Me.lblDateSanad.Name = "lblDateSanad"
            Me.lblDateSanad.Size = New Size(95, 22)
            Me.lblDateSanad.Text = "تاریخ فاکتور خرید:"
            Me.lblDateSanad.TextAlign = ContentAlignment.MiddleLeft

            Me.txtDateSanad.Location = New Point(690, 6)
            Me.txtDateSanad.Name = "txtDateSanad"
            Me.txtDateSanad.Size = New Size(85, 22)
            Me.txtDateSanad.TabIndex = 2

            Me.btnCalDate.Location = New Point(662, 5)
            Me.btnCalDate.Name = "btnCalDate"
            Me.btnCalDate.Size = New Size(26, 24)
            Me.btnCalDate.TabIndex = 3
            Me.btnCalDate.Text = ".."
            Me.btnCalDate.UseVisualStyleBackColor = True

            '
            'lblSystemDate & txtSystemDate
            '
            Me.lblSystemDate.Font = New Font("Tahoma", 9.0!)
            Me.lblSystemDate.Location = New Point(560, 6)
            Me.lblSystemDate.Name = "lblSystemDate"
            Me.lblSystemDate.Size = New Size(125, 22)
            Me.lblSystemDate.Text = "تاریخ ثبت در سیستم:"
            Me.lblSystemDate.TextAlign = ContentAlignment.MiddleLeft

            Me.txtSystemDate.Location = New Point(475, 6)
            Me.txtSystemDate.Name = "txtSystemDate"
            Me.txtSystemDate.Size = New Size(80, 22)
            Me.txtSystemDate.TabIndex = 4

            Me.btnCalSystemDate.Location = New Point(447, 5)
            Me.btnCalSystemDate.Name = "btnCalSystemDate"
            Me.btnCalSystemDate.Size = New Size(26, 24)
            Me.btnCalSystemDate.TabIndex = 5
            Me.btnCalSystemDate.Text = ".."
            Me.btnCalSystemDate.UseVisualStyleBackColor = True

            '
            'lblTaxEntryMode & cmbTaxEntryMode
            '
            Me.lblTaxEntryMode.Font = New Font("Tahoma", 9.0!)
            Me.lblTaxEntryMode.Location = New Point(215, 6)
            Me.lblTaxEntryMode.Name = "lblTaxEntryMode"
            Me.lblTaxEntryMode.Size = New Size(225, 22)
            Me.lblTaxEntryMode.Text = "نحوه محاسبه مالیات و عوارض:"
            Me.lblTaxEntryMode.TextAlign = ContentAlignment.MiddleLeft

            Me.cmbTaxEntryMode.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbTaxEntryMode.Font = New Font("Tahoma", 9.0!)
            Me.cmbTaxEntryMode.FormattingEnabled = True
            Me.cmbTaxEntryMode.Items.AddRange(New Object() {"--- انتخاب نشده ---", "ورود مالیات و عوارض برای هر سطر", "ورود مالیات و عوارض برای کل فاکتور"})
            Me.cmbTaxEntryMode.Location = New Point(10, 6)
            Me.cmbTaxEntryMode.Name = "cmbTaxEntryMode"
            Me.cmbTaxEntryMode.Size = New Size(200, 22)
            Me.cmbTaxEntryMode.TabIndex = 6

            Me.lblTaeazLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblTaeazLabel.Location = New Point(200, 4)
            Me.lblTaeazLabel.Name = "lblTaeazLabel"
            Me.lblTaeazLabel.Size = New Size(90, 22)
            Me.lblTaeazLabel.Text = "وضعیت تسویه:"
            Me.lblTaeazLabel.TextAlign = ContentAlignment.MiddleLeft

            Me.lblSanadLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblSanadLabel.Location = New Point(185, 4)
            Me.lblSanadLabel.Name = "lblSanadLabel"
            Me.lblSanadLabel.Size = New Size(60, 22)
            Me.lblSanadLabel.Text = "فاکتور"
            Me.lblSanadLabel.TextAlign = ContentAlignment.MiddleLeft
            Me.lblSanadLabel.Visible = False

            Me.lblSanadStatus.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.lblSanadStatus.ForeColor = Color.Red
            Me.lblSanadStatus.Location = New Point(80, 4)
            Me.lblSanadStatus.Name = "lblSanadStatus"
            Me.lblSanadStatus.Size = New Size(115, 22)
            Me.lblSanadStatus.Text = "تسویه نشده"
            Me.lblSanadStatus.TextAlign = ContentAlignment.MiddleLeft

            ' pnlViewSarfasl
            Me.pnlViewSarfasl.Controls.Add(Me.lblSarfaslTitle)
            Me.pnlViewSarfasl.Controls.Add(Me.btnSelectVendor)
            Me.pnlViewSarfasl.Controls.Add(Me.lblSarfaslValue)
            Me.pnlViewSarfasl.Controls.Add(Me.lblTaeazLabel)
            Me.pnlViewSarfasl.Controls.Add(Me.lblSanadLabel)
            Me.pnlViewSarfasl.Controls.Add(Me.lblSanadStatus)
            Me.pnlViewSarfasl.Dock = DockStyle.Top
            Me.pnlViewSarfasl.Location = New Point(0, 34)
            Me.pnlViewSarfasl.Name = "pnlViewSarfasl"
            Me.pnlViewSarfasl.Size = New Size(1312, 30)
            Me.pnlViewSarfasl.TabIndex = 1

            Me.lblSarfaslTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblSarfaslTitle.ForeColor = Color.DarkGreen
            Me.lblSarfaslTitle.Location = New Point(1100, 4)
            Me.lblSarfaslTitle.Name = "lblSarfaslTitle"
            Me.lblSarfaslTitle.Size = New Size(200, 22)
            Me.lblSarfaslTitle.Text = "کد و نام فروشنده / تامین کننده :"
            Me.lblSarfaslTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.btnSelectVendor.Location = New Point(1060, 4)
            Me.btnSelectVendor.Name = "btnSelectVendor"
            Me.btnSelectVendor.Size = New Size(35, 24)
            Me.btnSelectVendor.TabIndex = 0
            Me.btnSelectVendor.Text = "..."
            Me.btnSelectVendor.UseVisualStyleBackColor = True

            Me.lblSarfaslValue.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblSarfaslValue.ForeColor = Color.Blue
            Me.lblSarfaslValue.Location = New Point(400, 4)
            Me.lblSarfaslValue.Name = "lblSarfaslValue"
            Me.lblSarfaslValue.Size = New Size(650, 22)
            Me.lblSarfaslValue.Text = "(انتخاب نشده)"
            Me.lblSarfaslValue.TextAlign = ContentAlignment.MiddleLeft

            ' pnlViewShenavar
            Me.pnlViewShenavar.Controls.Add(Me.lblShenavarValue)
            Me.pnlViewShenavar.Controls.Add(Me.lblShenavarTitle)
            Me.pnlViewShenavar.Dock = DockStyle.Top
            Me.pnlViewShenavar.Location = New Point(0, 64)
            Me.pnlViewShenavar.Name = "pnlViewShenavar"
            Me.pnlViewShenavar.Size = New Size(1312, 30)
            Me.pnlViewShenavar.TabIndex = 2

            Me.lblShenavarTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblShenavarTitle.ForeColor = Color.DarkBlue
            Me.lblShenavarTitle.Location = New Point(1100, 4)
            Me.lblShenavarTitle.Name = "lblShenavarTitle"
            Me.lblShenavarTitle.Size = New Size(200, 22)
            Me.lblShenavarTitle.Text = "کد و نام انبار مقصد ردیف جاری :"
            Me.lblShenavarTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.lblShenavarValue.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblShenavarValue.ForeColor = Color.Blue
            Me.lblShenavarValue.Location = New Point(400, 4)
            Me.lblShenavarValue.Name = "lblShenavarValue"
            Me.lblShenavarValue.Size = New Size(650, 22)
            Me.lblShenavarValue.Text = "(انتخاب نشده)"
            Me.lblShenavarValue.TextAlign = ContentAlignment.MiddleLeft

            ' pnlSerch
            Me.pnlSerch.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlSerch.Controls.Add(Me.txtSrcLineNo)
            Me.pnlSerch.Controls.Add(Me.txtSrcKala)
            Me.pnlSerch.Controls.Add(Me.txtSrcCode)
            Me.pnlSerch.Controls.Add(Me.txtSrcName)
            Me.pnlSerch.Controls.Add(Me.txtSrcBtnWarehouse)
            Me.pnlSerch.Controls.Add(Me.txtSrcWarehouse)
            Me.pnlSerch.Controls.Add(Me.txtSrcBtnUnit)
            Me.pnlSerch.Controls.Add(Me.txtSrcUnit)
            Me.pnlSerch.Controls.Add(Me.txtSrcQty)
            Me.pnlSerch.Controls.Add(Me.txtSrcUnitPrice)
            Me.pnlSerch.Controls.Add(Me.txtSrcLineTotal)
            Me.pnlSerch.Controls.Add(Me.txtSrcDiscount)
            Me.pnlSerch.Controls.Add(Me.txtSrcLineTotalAfterDiscount)
            Me.pnlSerch.Controls.Add(Me.txtSrcVat)
            Me.pnlSerch.Controls.Add(Me.txtSrcTotalPrice)
            Me.pnlSerch.Dock = DockStyle.Top
            Me.pnlSerch.Location = New Point(0, 94)
            Me.pnlSerch.Name = "pnlSerch"
            Me.pnlSerch.Size = New Size(1312, 32)
            Me.pnlSerch.TabIndex = 3

            ' pnlDgv
            Me.pnlDgv.Controls.Add(Me.dgvEntryLines)
            Me.pnlDgv.Dock = DockStyle.Fill
            Me.pnlDgv.Location = New Point(0, 126)
            Me.pnlDgv.Name = "pnlDgv"
            Me.pnlDgv.Size = New Size(1312, 445)
            Me.pnlDgv.TabIndex = 4

            Me.dgvEntryLines.AllowUserToAddRows = False
            Me.dgvEntryLines.Dock = DockStyle.Fill
            Me.dgvEntryLines.Location = New Point(0, 0)
            Me.dgvEntryLines.Name = "dgvEntryLines"
            Me.dgvEntryLines.RowHeadersVisible = False
            Me.dgvEntryLines.Size = New Size(1312, 445)
            Me.dgvEntryLines.TabIndex = 0

            ' pnlTotalsRow (پانل مجموع زنده تراز شده با ستون‌های گرید)
            Me.pnlTotalsRow.BackColor = Color.FromArgb(235, 240, 248)
            Me.pnlTotalsRow.Controls.Add(Me.txtSumLineTotal)
            Me.pnlTotalsRow.Controls.Add(Me.txtSumDiscount)
            Me.pnlTotalsRow.Controls.Add(Me.txtSumLineTotalAfterDiscount)
            Me.pnlTotalsRow.Controls.Add(Me.txtSumVat)
            Me.pnlTotalsRow.Controls.Add(Me.txtSumTotalPrice)
            Me.pnlTotalsRow.Dock = DockStyle.Bottom
            Me.pnlTotalsRow.Location = New Point(0, 571)
            Me.pnlTotalsRow.Name = "pnlTotalsRow"
            Me.pnlTotalsRow.Size = New Size(1312, 32)
            Me.pnlTotalsRow.TabIndex = 5

            ' pnlBottomRow
            Me.pnlBottomRow.Controls.Add(Me.pnlJamSanad)
            Me.pnlBottomRow.Controls.Add(Me.pnlSharhSanad)
            Me.pnlBottomRow.Dock = DockStyle.Bottom
            Me.pnlBottomRow.Location = New Point(0, 603)
            Me.pnlBottomRow.Name = "pnlBottomRow"
            Me.pnlBottomRow.Size = New Size(1312, 75)
            Me.pnlBottomRow.TabIndex = 6

            ' pnlSharhSanad
            Me.pnlSharhSanad.Controls.Add(Me.lblEntryDesc)
            Me.pnlSharhSanad.Controls.Add(Me.txtEntryDescription)
            Me.pnlSharhSanad.Dock = DockStyle.Right
            Me.pnlSharhSanad.Location = New Point(862, 0)
            Me.pnlSharhSanad.Name = "pnlSharhSanad"
            Me.pnlSharhSanad.Size = New Size(450, 75)
            Me.pnlSharhSanad.TabIndex = 0

            Me.lblEntryDesc.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblEntryDesc.Location = New Point(330, 8)
            Me.lblEntryDesc.Name = "lblEntryDesc"
            Me.lblEntryDesc.Size = New Size(115, 22)
            Me.lblEntryDesc.Text = "شرح فاکتور خرید:"
            Me.lblEntryDesc.TextAlign = ContentAlignment.MiddleLeft

            Me.txtEntryDescription.Location = New Point(10, 8)
            Me.txtEntryDescription.Multiline = True
            Me.txtEntryDescription.Name = "txtEntryDescription"
            Me.txtEntryDescription.Size = New Size(315, 58)
            Me.txtEntryDescription.TabIndex = 0

            ' pnlJamSanad
            Me.pnlJamSanad.BackColor = Color.FromArgb(250, 250, 245)
            Me.pnlJamSanad.Controls.Add(Me.lblTotalVatInput)
            Me.pnlJamSanad.Controls.Add(Me.txtTotalVatInput)
            Me.pnlJamSanad.Controls.Add(Me.lblJamTitle)
            Me.pnlJamSanad.Controls.Add(Me.txtJamBedehkar)
            Me.pnlJamSanad.Controls.Add(Me.lblKasriTitle)
            Me.pnlJamSanad.Controls.Add(Me.txtKasriDebit)
            Me.pnlJamSanad.Dock = DockStyle.Fill
            Me.pnlJamSanad.Location = New Point(0, 0)
            Me.pnlJamSanad.Name = "pnlJamSanad"
            Me.pnlJamSanad.Size = New Size(862, 75)
            Me.pnlJamSanad.TabIndex = 1

            ' مالیات و عوارض برای کل فاکتور
            Me.lblTotalVatInput.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblTotalVatInput.ForeColor = Color.DarkRed
            Me.lblTotalVatInput.Location = New Point(660, 9)
            Me.lblTotalVatInput.Name = "lblTotalVatInput"
            Me.lblTotalVatInput.Size = New Size(195, 22)
            Me.lblTotalVatInput.Text = "مالیات و عوارض برای کل فاکتور:"
            Me.lblTotalVatInput.TextAlign = ContentAlignment.MiddleLeft

            Me.txtTotalVatInput.Enabled = False
            Me.txtTotalVatInput.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.txtTotalVatInput.Location = New Point(510, 8)
            Me.txtTotalVatInput.Name = "txtTotalVatInput"
            Me.txtTotalVatInput.Size = New Size(145, 23)
            Me.txtTotalVatInput.TabIndex = 0
            Me.txtTotalVatInput.Text = "0"
            Me.txtTotalVatInput.TextAlign = HorizontalAlignment.Center

            Me.lblJamTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblJamTitle.Location = New Point(380, 9)
            Me.lblJamTitle.Name = "lblJamTitle"
            Me.lblJamTitle.Size = New Size(120, 22)
            Me.lblJamTitle.Text = "جمع کل اقلام :"
            Me.lblJamTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.txtJamBedehkar.BackColor = Color.FromArgb(200, 240, 255)
            Me.txtJamBedehkar.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.txtJamBedehkar.Location = New Point(230, 8)
            Me.txtJamBedehkar.Name = "txtJamBedehkar"
            Me.txtJamBedehkar.ReadOnly = True
            Me.txtJamBedehkar.Size = New Size(145, 22)
            Me.txtJamBedehkar.TabIndex = 1
            Me.txtJamBedehkar.Text = "0"
            Me.txtJamBedehkar.TextAlign = HorizontalAlignment.Center

            Me.lblKasriTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblKasriTitle.Location = New Point(660, 44)
            Me.lblKasriTitle.Name = "lblKasriTitle"
            Me.lblKasriTitle.Size = New Size(195, 22)
            Me.lblKasriTitle.Text = "خالص قابل پرداخت :"
            Me.lblKasriTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.txtKasriDebit.BackColor = Color.FromArgb(255, 240, 200)
            Me.txtKasriDebit.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.txtKasriDebit.ForeColor = Color.DarkBlue
            Me.txtKasriDebit.Location = New Point(510, 42)
            Me.txtKasriDebit.Name = "txtKasriDebit"
            Me.txtKasriDebit.ReadOnly = True
            Me.txtKasriDebit.Size = New Size(145, 23)
            Me.txtKasriDebit.TabIndex = 2
            Me.txtKasriDebit.Text = "0"
            Me.txtKasriDebit.TextAlign = HorizontalAlignment.Center

            ' pnlButton
            Me.pnlButton.BackColor = Color.FromArgb(235, 240, 250)
            Me.pnlButton.Controls.Add(Me.btnSaveAndContinue)
            Me.pnlButton.Controls.Add(Me.btnSaveEntry)
            Me.pnlButton.Controls.Add(Me.btnAddLine)
            Me.pnlButton.Controls.Add(Me.btnDeleteRow)
            Me.pnlButton.Controls.Add(Me.btnCopyBelow)
            Me.pnlButton.Controls.Add(Me.btnCopyAbove)
            Me.pnlButton.Controls.Add(Me.btnCopyToPos)
            Me.pnlButton.Controls.Add(Me.btnClearSearch)
            Me.pnlButton.Controls.Add(Me.btnExit)
            Me.pnlButton.Dock = DockStyle.Bottom
            Me.pnlButton.Location = New Point(0, 678)
            Me.pnlButton.Name = "pnlButton"
            Me.pnlButton.Size = New Size(1312, 44)
            Me.pnlButton.TabIndex = 7

            Me.btnSaveEntry.Location = New Point(1030, 8)
            Me.btnSaveEntry.Name = "btnSaveEntry"
            Me.btnSaveEntry.Size = New Size(150, 28)
            Me.btnSaveEntry.TabIndex = 0
            Me.btnSaveEntry.Text = "ثبت فاکتور خرید و خروج"
            Me.btnSaveEntry.UseVisualStyleBackColor = True

            Me.btnSaveAndContinue.Location = New Point(870, 8)
            Me.btnSaveAndContinue.Name = "btnSaveAndContinue"
            Me.btnSaveAndContinue.Size = New Size(150, 28)
            Me.btnSaveAndContinue.TabIndex = 1
            Me.btnSaveAndContinue.Text = "ثبت فاکتور خرید و ادامه"
            Me.btnSaveAndContinue.UseVisualStyleBackColor = True

            Me.btnAddLine.Location = New Point(740, 8)
            Me.btnAddLine.Name = "btnAddLine"
            Me.btnAddLine.Size = New Size(120, 28)
            Me.btnAddLine.TabIndex = 2
            Me.btnAddLine.Text = "افزودن سطر خالی"
            Me.btnAddLine.UseVisualStyleBackColor = True

            Me.btnDeleteRow.Location = New Point(610, 8)
            Me.btnDeleteRow.Name = "btnDeleteRow"
            Me.btnDeleteRow.Size = New Size(120, 28)
            Me.btnDeleteRow.TabIndex = 3
            Me.btnDeleteRow.Text = "حذف سطر جاری"
            Me.btnDeleteRow.UseVisualStyleBackColor = True

            Me.btnCopyBelow.Location = New Point(490, 8)
            Me.btnCopyBelow.Name = "btnCopyBelow"
            Me.btnCopyBelow.Size = New Size(110, 28)
            Me.btnCopyBelow.TabIndex = 4
            Me.btnCopyBelow.Text = "کپی در زیر"
            Me.btnCopyBelow.UseVisualStyleBackColor = True

            Me.btnCopyAbove.Location = New Point(370, 8)
            Me.btnCopyAbove.Name = "btnCopyAbove"
            Me.btnCopyAbove.Size = New Size(110, 28)
            Me.btnCopyAbove.TabIndex = 5
            Me.btnCopyAbove.Text = "کپی در بالا"
            Me.btnCopyAbove.UseVisualStyleBackColor = True

            Me.btnCopyToPos.Location = New Point(240, 8)
            Me.btnCopyToPos.Name = "btnCopyToPos"
            Me.btnCopyToPos.Size = New Size(120, 28)
            Me.btnCopyToPos.TabIndex = 6
            Me.btnCopyToPos.Text = "کپی در محل دلخواه"
            Me.btnCopyToPos.UseVisualStyleBackColor = True

            Me.btnClearSearch.Location = New Point(110, 8)
            Me.btnClearSearch.Name = "btnClearSearch"
            Me.btnClearSearch.Size = New Size(120, 28)
            Me.btnClearSearch.TabIndex = 7
            Me.btnClearSearch.Text = "پاک کردن جستجوها"
            Me.btnClearSearch.UseVisualStyleBackColor = True

            Me.btnExit.Location = New Point(10, 8)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New Size(90, 28)
            Me.btnExit.TabIndex = 8
            Me.btnExit.Text = "خروج"
            Me.btnExit.UseVisualStyleBackColor = True

            ' tabPageZamayem
            Me.tabPageZamayem.Controls.Add(Me.pnlZamPreview)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamList)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamLineSelector)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamButtons)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamHeader)
            Me.tabPageZamayem.Location = New Point(4, 23)
            Me.tabPageZamayem.Name = "tabPageZamayem"
            Me.tabPageZamayem.Size = New Size(1312, 722)
            Me.tabPageZamayem.TabIndex = 1
            Me.tabPageZamayem.Text = "ضمائم فاکتور خرید"
            Me.tabPageZamayem.UseVisualStyleBackColor = True

            ' tabPageTasvieh
            Me.tabPageTasvieh.Location = New Point(4, 23)
            Me.tabPageTasvieh.Name = "tabPageTasvieh"
            Me.tabPageTasvieh.Size = New Size(1312, 722)
            Me.tabPageTasvieh.TabIndex = 3
            Me.tabPageTasvieh.Text = "تسویه فاکتور خرید"
            Me.tabPageTasvieh.UseVisualStyleBackColor = True

            ' pnlZamPreview
            Me.pnlZamPreview.Controls.Add(Me.picAttachment)
            Me.pnlZamPreview.Dock = DockStyle.Fill
            Me.pnlZamPreview.Location = New Point(350, 60)
            Me.pnlZamPreview.Name = "pnlZamPreview"
            Me.pnlZamPreview.Size = New Size(612, 662)
            Me.pnlZamPreview.TabIndex = 4

            Me.picAttachment.Dock = DockStyle.Fill
            Me.picAttachment.Location = New Point(0, 0)
            Me.picAttachment.Name = "picAttachment"
            Me.picAttachment.Size = New Size(612, 662)
            Me.picAttachment.SizeMode = PictureBoxSizeMode.Zoom
            Me.picAttachment.TabIndex = 0

            ' pnlZamList
            Me.pnlZamList.Controls.Add(Me.dgvAttachments)
            Me.pnlZamList.Dock = DockStyle.Right
            Me.pnlZamList.Location = New Point(962, 60)
            Me.pnlZamList.Name = "pnlZamList"
            Me.pnlZamList.Size = New Size(350, 662)
            Me.pnlZamList.TabIndex = 3

            Me.dgvAttachments.AllowUserToAddRows = False
            Me.dgvAttachments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAttachments.Dock = DockStyle.Fill
            Me.dgvAttachments.Location = New Point(0, 0)
            Me.dgvAttachments.Name = "dgvAttachments"
            Me.dgvAttachments.Size = New Size(350, 662)
            Me.dgvAttachments.TabIndex = 0

            ' pnlZamLineSelector
            Me.pnlZamLineSelector.Controls.Add(Me.dgvZamLineSelector)
            Me.pnlZamLineSelector.Controls.Add(Me.lblZamLineSelectorTitle)
            Me.pnlZamLineSelector.Dock = DockStyle.Left
            Me.pnlZamLineSelector.Location = New Point(0, 60)
            Me.pnlZamLineSelector.Name = "pnlZamLineSelector"
            Me.pnlZamLineSelector.Size = New Size(350, 662)
            Me.pnlZamLineSelector.TabIndex = 2

            Me.dgvZamLineSelector.AllowUserToAddRows = False
            Me.dgvZamLineSelector.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvZamLineSelector.Dock = DockStyle.Fill
            Me.dgvZamLineSelector.Location = New Point(0, 24)
            Me.dgvZamLineSelector.Name = "dgvZamLineSelector"
            Me.dgvZamLineSelector.Size = New Size(350, 638)
            Me.dgvZamLineSelector.TabIndex = 0

            Me.lblZamLineSelectorTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblZamLineSelectorTitle.Location = New Point(0, 0)
            Me.lblZamLineSelectorTitle.Name = "lblZamLineSelectorTitle"
            Me.lblZamLineSelectorTitle.Size = New Size(350, 24)
            Me.lblZamLineSelectorTitle.Text = "انتخاب ردیف کالا برای ضمیمه:"
            Me.lblZamLineSelectorTitle.TextAlign = ContentAlignment.MiddleCenter

            ' pnlZamButtons
            Me.pnlZamButtons.Controls.Add(Me.btnZamAddFile)
            Me.pnlZamButtons.Controls.Add(Me.btnZamScan)
            Me.pnlZamButtons.Controls.Add(Me.btnZamView)
            Me.pnlZamButtons.Controls.Add(Me.btnZamPrint)
            Me.pnlZamButtons.Controls.Add(Me.btnZamDelete)
            Me.pnlZamButtons.Dock = DockStyle.Top
            Me.pnlZamButtons.Location = New Point(0, 30)
            Me.pnlZamButtons.Name = "pnlZamButtons"
            Me.pnlZamButtons.Size = New Size(1312, 30)
            Me.pnlZamButtons.TabIndex = 1

            Me.btnZamAddFile.Location = New Point(1220, 2)
            Me.btnZamAddFile.Name = "btnZamAddFile"
            Me.btnZamAddFile.Size = New Size(80, 26)
            Me.btnZamAddFile.TabIndex = 0
            Me.btnZamAddFile.Text = "افزودن فایل"

            Me.btnZamScan.Location = New Point(1130, 2)
            Me.btnZamScan.Name = "btnZamScan"
            Me.btnZamScan.Size = New Size(80, 26)
            Me.btnZamScan.TabIndex = 1
            Me.btnZamScan.Text = "اسکن سند"

            Me.btnZamView.Location = New Point(1040, 2)
            Me.btnZamView.Name = "btnZamView"
            Me.btnZamView.Size = New Size(80, 26)
            Me.btnZamView.TabIndex = 2
            Me.btnZamView.Text = "نمایش بزرگ"

            Me.btnZamPrint.Location = New Point(950, 2)
            Me.btnZamPrint.Name = "btnZamPrint"
            Me.btnZamPrint.Size = New Size(80, 26)
            Me.btnZamPrint.TabIndex = 3
            Me.btnZamPrint.Text = "چاپ تصویر"

            Me.btnZamDelete.Location = New Point(860, 2)
            Me.btnZamDelete.Name = "btnZamDelete"
            Me.btnZamDelete.Size = New Size(80, 26)
            Me.btnZamDelete.TabIndex = 4
            Me.btnZamDelete.Text = "حذف ضمیمه"

            ' pnlZamHeader
            Me.pnlZamHeader.Controls.Add(Me.lblSelectedLine)
            Me.pnlZamHeader.Dock = DockStyle.Top
            Me.pnlZamHeader.Location = New Point(0, 0)
            Me.pnlZamHeader.Name = "pnlZamHeader"
            Me.pnlZamHeader.Size = New Size(1312, 30)
            Me.pnlZamHeader.TabIndex = 0

            Me.lblSelectedLine.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.lblSelectedLine.ForeColor = Color.DarkBlue
            Me.lblSelectedLine.Location = New Point(500, 4)
            Me.lblSelectedLine.Name = "lblSelectedLine"
            Me.lblSelectedLine.Size = New Size(400, 22)
            Me.lblSelectedLine.Text = "ردیف کالا انتخاب نشده است."
            Me.lblSelectedLine.TextAlign = ContentAlignment.MiddleLeft

            ' tabPageYaddasht
            Me.tabPageYaddasht.Controls.Add(Me.pnlNoteLineSelector)
            Me.tabPageYaddasht.Controls.Add(Me.pnlEntryNote)
            Me.tabPageYaddasht.Controls.Add(Me.pnlLineNote)
            Me.tabPageYaddasht.Controls.Add(Me.pnlNoteHeader)
            Me.tabPageYaddasht.Location = New Point(4, 23)
            Me.tabPageYaddasht.Name = "tabPageYaddasht"
            Me.tabPageYaddasht.Size = New Size(1312, 722)
            Me.tabPageYaddasht.TabIndex = 2
            Me.tabPageYaddasht.Text = "یادداشت برای فاکتور خرید"
            Me.tabPageYaddasht.UseVisualStyleBackColor = True

            ' pnlNoteHeader
            Me.pnlNoteHeader.Controls.Add(Me.lblNoteHeader)
            Me.pnlNoteHeader.Dock = DockStyle.Top
            Me.pnlNoteHeader.Location = New Point(0, 0)
            Me.pnlNoteHeader.Name = "pnlNoteHeader"
            Me.pnlNoteHeader.Size = New Size(1312, 30)
            Me.pnlNoteHeader.TabIndex = 0

            Me.lblNoteHeader.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.lblNoteHeader.ForeColor = Color.DarkMagenta
            Me.lblNoteHeader.Location = New Point(400, 4)
            Me.lblNoteHeader.Name = "lblNoteHeader"
            Me.lblNoteHeader.Size = New Size(600, 22)
            Me.lblNoteHeader.Text = "مدیریت یادداشت‌های فاکتور خرید جاری"
            Me.lblNoteHeader.TextAlign = ContentAlignment.MiddleCenter

            ' pnlNoteLineSelector
            Me.pnlNoteLineSelector.Controls.Add(Me.dgvNoteLineSelector)
            Me.pnlNoteLineSelector.Controls.Add(Me.lblNoteLineSelectorTitle)
            Me.pnlNoteLineSelector.Dock = DockStyle.Left
            Me.pnlNoteLineSelector.Location = New Point(0, 30)
            Me.pnlNoteLineSelector.Name = "pnlNoteLineSelector"
            Me.pnlNoteLineSelector.Size = New Size(300, 692)
            Me.pnlNoteLineSelector.TabIndex = 1

            Me.dgvNoteLineSelector.AllowUserToAddRows = False
            Me.dgvNoteLineSelector.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvNoteLineSelector.Dock = DockStyle.Fill
            Me.dgvNoteLineSelector.Location = New Point(0, 24)
            Me.dgvNoteLineSelector.Name = "dgvNoteLineSelector"
            Me.dgvNoteLineSelector.Size = New Size(300, 668)
            Me.dgvNoteLineSelector.TabIndex = 0

            Me.lblNoteLineSelectorTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblNoteLineSelectorTitle.Location = New Point(0, 0)
            Me.lblNoteLineSelectorTitle.Name = "lblNoteLineSelectorTitle"
            Me.lblNoteLineSelectorTitle.Size = New Size(300, 24)
            Me.lblNoteLineSelectorTitle.Text = "انتخاب ردیف کالا برای یادداشت:"
            Me.lblNoteLineSelectorTitle.TextAlign = ContentAlignment.MiddleCenter

            ' pnlEntryNote
            Me.pnlEntryNote.Controls.Add(Me.dgvEntryNotes)
            Me.pnlEntryNote.Controls.Add(Me.pnlEntryNoteInput)
            Me.pnlEntryNote.Controls.Add(Me.lblEntryNoteTitle)
            Me.pnlEntryNote.Dock = DockStyle.Right
            Me.pnlEntryNote.Location = New Point(812, 30)
            Me.pnlEntryNote.Name = "pnlEntryNote"
            Me.pnlEntryNote.Size = New Size(500, 692)
            Me.pnlEntryNote.TabIndex = 3

            Me.lblEntryNoteTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblEntryNoteTitle.Location = New Point(0, 0)
            Me.lblEntryNoteTitle.Name = "lblEntryNoteTitle"
            Me.lblEntryNoteTitle.Size = New Size(500, 24)
            Me.lblEntryNoteTitle.Text = "یادداشت‌های عمومی کل فاکتور"
            Me.lblEntryNoteTitle.TextAlign = ContentAlignment.MiddleCenter

            Me.dgvEntryNotes.AllowUserToAddRows = False
            Me.dgvEntryNotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvEntryNotes.Dock = DockStyle.Fill
            Me.dgvEntryNotes.Location = New Point(0, 24)
            Me.dgvEntryNotes.Name = "dgvEntryNotes"
            Me.dgvEntryNotes.Size = New Size(500, 528)
            Me.dgvEntryNotes.TabIndex = 0

            Me.pnlEntryNoteInput.Controls.Add(Me.txtEntryNote)
            Me.pnlEntryNoteInput.Controls.Add(Me.pnlEntryNoteAction)
            Me.pnlEntryNoteInput.Controls.Add(Me.lblEntryNoteInputTitle)
            Me.pnlEntryNoteInput.Dock = DockStyle.Bottom
            Me.pnlEntryNoteInput.Location = New Point(0, 552)
            Me.pnlEntryNoteInput.Name = "pnlEntryNoteInput"
            Me.pnlEntryNoteInput.Size = New Size(500, 140)
            Me.pnlEntryNoteInput.TabIndex = 1

            Me.lblEntryNoteInputTitle.Location = New Point(400, 4)
            Me.lblEntryNoteInputTitle.Name = "lblEntryNoteInputTitle"
            Me.lblEntryNoteInputTitle.Size = New Size(90, 22)
            Me.lblEntryNoteInputTitle.Text = "متن یادداشت:"
            Me.lblEntryNoteInputTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.txtEntryNote.Location = New Point(10, 30)
            Me.txtEntryNote.Multiline = True
            Me.txtEntryNote.Name = "txtEntryNote"
            Me.txtEntryNote.Size = New Size(480, 70)
            Me.txtEntryNote.TabIndex = 0

            Me.pnlEntryNoteAction.Controls.Add(Me.btnSaveEntryNote)
            Me.pnlEntryNoteAction.Controls.Add(Me.lblEntryNoteInfo)
            Me.pnlEntryNoteAction.Dock = DockStyle.Bottom
            Me.pnlEntryNoteAction.Location = New Point(0, 106)
            Me.pnlEntryNoteAction.Name = "pnlEntryNoteAction"
            Me.pnlEntryNoteAction.Size = New Size(500, 34)
            Me.pnlEntryNoteAction.TabIndex = 1

            Me.btnSaveEntryNote.Location = New Point(10, 4)
            Me.btnSaveEntryNote.Name = "btnSaveEntryNote"
            Me.btnSaveEntryNote.Size = New Size(80, 26)
            Me.btnSaveEntryNote.TabIndex = 0
            Me.btnSaveEntryNote.Text = "ثبت یادداشت"

            Me.lblEntryNoteInfo.Location = New Point(100, 4)
            Me.lblEntryNoteInfo.Name = "lblEntryNoteInfo"
            Me.lblEntryNoteInfo.Size = New Size(390, 22)
            Me.lblEntryNoteInfo.Text = "یادداشت جدید با کلید ثبت اضافه خواهد شد."
            Me.lblEntryNoteInfo.TextAlign = ContentAlignment.MiddleLeft

            ' pnlLineNote
            Me.pnlLineNote.Controls.Add(Me.dgvLineNotes)
            Me.pnlLineNote.Controls.Add(Me.pnlLineNoteInput)
            Me.pnlLineNote.Controls.Add(Me.lblLineNoteTitle)
            Me.pnlLineNote.Dock = DockStyle.Fill
            Me.pnlLineNote.Location = New Point(300, 30)
            Me.pnlLineNote.Name = "pnlLineNote"
            Me.pnlLineNote.Size = New Size(512, 692)
            Me.pnlLineNote.TabIndex = 2

            Me.lblLineNoteTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblLineNoteTitle.Location = New Point(0, 0)
            Me.lblLineNoteTitle.Name = "lblLineNoteTitle"
            Me.lblLineNoteTitle.Size = New Size(512, 24)
            Me.lblLineNoteTitle.Text = "یادداشت‌های ردیف کالا انتخاب شده"
            Me.lblLineNoteTitle.TextAlign = ContentAlignment.MiddleCenter

            Me.dgvLineNotes.AllowUserToAddRows = False
            Me.dgvLineNotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvLineNotes.Dock = DockStyle.Fill
            Me.dgvLineNotes.Location = New Point(0, 24)
            Me.dgvLineNotes.Name = "dgvLineNotes"
            Me.dgvLineNotes.Size = New Size(512, 528)
            Me.dgvLineNotes.TabIndex = 0

            Me.pnlLineNoteInput.Controls.Add(Me.txtLineNote)
            Me.pnlLineNoteInput.Controls.Add(Me.pnlLineNoteAction)
            Me.pnlLineNoteInput.Controls.Add(Me.lblLineNoteInputTitle)
            Me.pnlLineNoteInput.Dock = DockStyle.Bottom
            Me.pnlLineNoteInput.Location = New Point(0, 552)
            Me.pnlLineNoteInput.Name = "pnlLineNoteInput"
            Me.pnlLineNoteInput.Size = New Size(512, 140)
            Me.pnlLineNoteInput.TabIndex = 1

            Me.lblLineNoteInputTitle.Location = New Point(410, 4)
            Me.lblLineNoteInputTitle.Name = "lblLineNoteInputTitle"
            Me.lblLineNoteInputTitle.Size = New Size(90, 22)
            Me.lblLineNoteInputTitle.Text = "متن یادداشت ردیف:"
            Me.lblLineNoteInputTitle.TextAlign = ContentAlignment.MiddleLeft

            Me.txtLineNote.Location = New Point(10, 30)
            Me.txtLineNote.Multiline = True
            Me.txtLineNote.Name = "txtLineNote"
            Me.txtLineNote.Size = New Size(490, 70)
            Me.txtLineNote.TabIndex = 0

            Me.pnlLineNoteAction.Controls.Add(Me.btnSaveLineNote)
            Me.pnlLineNoteAction.Controls.Add(Me.lblLineNoteInfo)
            Me.pnlLineNoteAction.Dock = DockStyle.Bottom
            Me.pnlLineNoteAction.Location = New Point(0, 106)
            Me.pnlLineNoteAction.Name = "pnlLineNoteAction"
            Me.pnlLineNoteAction.Size = New Size(512, 34)
            Me.pnlLineNoteAction.TabIndex = 1

            Me.btnSaveLineNote.Location = New Point(10, 4)
            Me.btnSaveLineNote.Name = "btnSaveLineNote"
            Me.btnSaveLineNote.Size = New Size(80, 26)
            Me.btnSaveLineNote.TabIndex = 0
            Me.btnSaveLineNote.Text = "ثبت یادداشت"

            Me.lblLineNoteInfo.Location = New Point(100, 4)
            Me.lblLineNoteInfo.Name = "lblLineNoteInfo"
            Me.lblLineNoteInfo.Size = New Size(400, 22)
            Me.lblLineNoteInfo.Text = "یادداشت جدید با کلید ثبت اضافه خواهد شد."
            Me.lblLineNoteInfo.TextAlign = ContentAlignment.MiddleLeft

            ' Form Configs
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1320, 749)
            Me.Controls.Add(Me.tabMain)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryKharid2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت فاکتور خرید جدید"

            Me.pnlNoteHeader.ResumeLayout(False)
            Me.pnlEntryNote.ResumeLayout(False)
            CType(Me.dgvEntryNotes, ISupportInitialize).EndInit()
            Me.pnlEntryNoteInput.ResumeLayout(False)
            Me.pnlEntryNoteInput.PerformLayout()
            Me.pnlEntryNoteAction.ResumeLayout(False)
            Me.pnlLineNote.ResumeLayout(False)
            CType(Me.dgvLineNotes, ISupportInitialize).EndInit()
            Me.pnlLineNoteInput.ResumeLayout(False)
            Me.pnlLineNoteInput.PerformLayout()
            Me.pnlLineNoteAction.ResumeLayout(False)
            Me.tabMain.ResumeLayout(False)
            Me.tabPageSanad.ResumeLayout(False)
            Me.pnlDgv.ResumeLayout(False)
            CType(Me.dgvEntryLines, ISupportInitialize).EndInit()
            Me.pnlTotalsRow.ResumeLayout(False)
            Me.pnlTotalsRow.PerformLayout()
            Me.pnlBottomRow.ResumeLayout(False)
            Me.pnlJamSanad.ResumeLayout(False)
            Me.pnlJamSanad.PerformLayout()
            Me.pnlSharhSanad.ResumeLayout(False)
            Me.pnlSharhSanad.PerformLayout()
            Me.pnlButton.ResumeLayout(False)
            Me.pnlSerch.ResumeLayout(False)
            Me.pnlSerch.PerformLayout()
            Me.pnlViewShenavar.ResumeLayout(False)
            Me.pnlViewSarfasl.ResumeLayout(False)
            Me.pnlNoSanad.ResumeLayout(False)
            Me.pnlNoSanad.PerformLayout()
            Me.tabPageZamayem.ResumeLayout(False)
            Me.pnlZamPreview.ResumeLayout(False)
            CType(Me.picAttachment, ISupportInitialize).EndInit()
            Me.pnlZamList.ResumeLayout(False)
            CType(Me.dgvAttachments, ISupportInitialize).EndInit()
            Me.pnlZamLineSelector.ResumeLayout(False)
            CType(Me.dgvZamLineSelector, ISupportInitialize).EndInit()
            Me.pnlZamButtons.ResumeLayout(False)
            Me.pnlZamHeader.ResumeLayout(False)
            Me.tabPageYaddasht.ResumeLayout(False)
            Me.pnlNoteLineSelector.ResumeLayout(False)
            CType(Me.dgvNoteLineSelector, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
