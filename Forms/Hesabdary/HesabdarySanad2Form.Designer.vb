Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class HesabdarySanad2Form
        Inherits Form

        Private components As IContainer

        ' تب کنترل اصلی
        Friend WithEvents tabMain As TabControl
        Friend WithEvents tabPageSanad As TabPage
        Friend WithEvents tabPageZamayem As TabPage
        Friend WithEvents tabPageYaddasht As TabPage

        ' کنترل‌های تب یادداشت سند
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

        ' کنترل‌های تب ضمائم سند
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

        ' لیست ردیف‌های دارای ضمیمه
        Friend WithEvents pnlZamLineSelector As Panel
        Friend WithEvents lblZamLineSelectorTitle As Label
        Friend WithEvents dgvZamLineSelector As DataGridView

        ' لیست ردیف‌های دارای یادداشت
        Friend WithEvents pnlNoteLineSelector As Panel
        Friend WithEvents lblNoteLineSelectorTitle As Label
        Friend WithEvents dgvNoteLineSelector As DataGridView

        ' پنل شماره سند (بالا)
        Friend WithEvents pnlNoSanad As Panel
        Friend WithEvents lblEntryRef As Label
        Friend WithEvents txtEntryReference As TextBox
        Friend WithEvents lblDateSanad As Label
        Friend WithEvents txtDateSanad As TextBox
        Friend WithEvents btnCalDate As Button

        ' پنل نمایش سرفصل ردیف جاری
        Friend WithEvents pnlViewSarfasl As Panel
        Friend WithEvents lblSarfaslTitle As Label
        Friend WithEvents lblSarfaslValue As Label

        ' پنل نمایش شناور ردیف جاری
        Friend WithEvents pnlViewShenavar As Panel
        Friend WithEvents lblShenavarTitle As Label
        Friend WithEvents lblShenavarValue As Label

        ' پنل جستجو — یک ردیف TextBox بالای هر ستون داده در گرید (موقعیت در runtime تنظیم می‌شود)
        Friend WithEvents pnlSerch As Panel
        Friend WithEvents txtSrcLineNo As TextBox
        Friend WithEvents txtSrcSF As TextBox
        Friend WithEvents txtSrcSH As TextBox
        Friend WithEvents txtSrcSharh As TextBox
        Friend WithEvents txtSrcDebit As TextBox
        Friend WithEvents txtSrcCredit As TextBox
        Friend WithEvents txtSrcTransNo As TextBox
        Friend WithEvents txtSrcTransDate As TextBox

        ' پنل داتاگریدویو (Fill)
        Friend WithEvents pnlDgv As Panel
        Friend WithEvents dgvEntryLines As DataGridView

        ' ردیف پایین: شامل pnlSharhSanad و pnlJamSanad کنار هم
        Friend WithEvents pnlBottomRow As Panel

        ' پنل شرح سند (سمت راست ردیف پایین)
        Friend WithEvents pnlSharhSanad As Panel
        Friend WithEvents lblEntryDesc As Label
        Friend WithEvents txtEntryDescription As TextBox

        ' پنل جمع سند (سمت چپ ردیف پایین)
        Friend WithEvents pnlJamSanad As Panel
        Friend WithEvents lblJamTitle As Label
        Friend WithEvents txtJamBedehkar As TextBox
        Friend WithEvents txtJamBestankar As TextBox
        Friend WithEvents lblTaeazLabel As Label
        Friend WithEvents lblSanadLabel As Label
        Friend WithEvents lblSanadStatus As Label
        Friend WithEvents lblKasriTitle As Label
        Friend WithEvents txtKasriDebit As TextBox
        Friend WithEvents txtKasriCredit As TextBox

        ' پنل دکمه‌ها (پایین)
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
        Friend WithEvents btnPrintVoucher As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlNoteHeader = New System.Windows.Forms.Panel()
            Me.lblNoteHeader = New System.Windows.Forms.Label()
            Me.pnlEntryNote = New System.Windows.Forms.Panel()
            Me.dgvEntryNotes = New System.Windows.Forms.DataGridView()
            Me.pnlEntryNoteInput = New System.Windows.Forms.Panel()
            Me.txtEntryNote = New System.Windows.Forms.TextBox()
            Me.pnlEntryNoteAction = New System.Windows.Forms.Panel()
            Me.btnSaveEntryNote = New System.Windows.Forms.Button()
            Me.lblEntryNoteInfo = New System.Windows.Forms.Label()
            Me.lblEntryNoteInputTitle = New System.Windows.Forms.Label()
            Me.lblEntryNoteTitle = New System.Windows.Forms.Label()
            Me.pnlLineNote = New System.Windows.Forms.Panel()
            Me.dgvLineNotes = New System.Windows.Forms.DataGridView()
            Me.pnlLineNoteInput = New System.Windows.Forms.Panel()
            Me.txtLineNote = New System.Windows.Forms.TextBox()
            Me.pnlLineNoteAction = New System.Windows.Forms.Panel()
            Me.btnSaveLineNote = New System.Windows.Forms.Button()
            Me.lblLineNoteInfo = New System.Windows.Forms.Label()
            Me.lblLineNoteInputTitle = New System.Windows.Forms.Label()
            Me.lblLineNoteTitle = New System.Windows.Forms.Label()
            Me.tabMain = New System.Windows.Forms.TabControl()
            Me.tabPageSanad = New System.Windows.Forms.TabPage()
            Me.pnlDgv = New System.Windows.Forms.Panel()
            Me.dgvEntryLines = New System.Windows.Forms.DataGridView()
            Me.pnlBottomRow = New System.Windows.Forms.Panel()
            Me.pnlJamSanad = New System.Windows.Forms.Panel()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblJamTitle = New System.Windows.Forms.Label()
            Me.txtJamBedehkar = New System.Windows.Forms.TextBox()
            Me.txtJamBestankar = New System.Windows.Forms.TextBox()
            Me.lblTaeazLabel = New System.Windows.Forms.Label()
            Me.lblSanadLabel = New System.Windows.Forms.Label()
            Me.lblSanadStatus = New System.Windows.Forms.Label()
            Me.lblKasriTitle = New System.Windows.Forms.Label()
            Me.txtKasriDebit = New System.Windows.Forms.TextBox()
            Me.txtKasriCredit = New System.Windows.Forms.TextBox()
            Me.pnlSharhSanad = New System.Windows.Forms.Panel()
            Me.lblEntryDesc = New System.Windows.Forms.Label()
            Me.txtEntryDescription = New System.Windows.Forms.TextBox()
            Me.pnlButton = New System.Windows.Forms.Panel()
            Me.btnSaveAndContinue = New System.Windows.Forms.Button()
            Me.btnSaveEntry = New System.Windows.Forms.Button()
            Me.btnAddLine = New System.Windows.Forms.Button()
            Me.btnDeleteRow = New System.Windows.Forms.Button()
            Me.btnCopyBelow = New System.Windows.Forms.Button()
            Me.btnCopyAbove = New System.Windows.Forms.Button()
            Me.btnCopyToPos = New System.Windows.Forms.Button()
            Me.btnClearSearch = New System.Windows.Forms.Button()
            Me.btnExit = New System.Windows.Forms.Button()
            Me.btnPrintVoucher = New System.Windows.Forms.Button()
            Me.pnlSerch = New System.Windows.Forms.Panel()
            Me.txtSrcLineNo = New System.Windows.Forms.TextBox()
            Me.txtSrcSF = New System.Windows.Forms.TextBox()
            Me.txtSrcSH = New System.Windows.Forms.TextBox()
            Me.txtSrcSharh = New System.Windows.Forms.TextBox()
            Me.txtSrcDebit = New System.Windows.Forms.TextBox()
            Me.txtSrcCredit = New System.Windows.Forms.TextBox()
            Me.txtSrcTransNo = New System.Windows.Forms.TextBox()
            Me.txtSrcTransDate = New System.Windows.Forms.TextBox()
            Me.pnlViewShenavar = New System.Windows.Forms.Panel()
            Me.lblShenavarValue = New System.Windows.Forms.Label()
            Me.lblShenavarTitle = New System.Windows.Forms.Label()
            Me.pnlViewSarfasl = New System.Windows.Forms.Panel()
            Me.lblSarfaslValue = New System.Windows.Forms.Label()
            Me.lblSarfaslTitle = New System.Windows.Forms.Label()
            Me.pnlNoSanad = New System.Windows.Forms.Panel()
            Me.lblEntryRef = New System.Windows.Forms.Label()
            Me.txtEntryReference = New System.Windows.Forms.TextBox()
            Me.lblDateSanad = New System.Windows.Forms.Label()
            Me.txtDateSanad = New System.Windows.Forms.TextBox()
            Me.btnCalDate = New System.Windows.Forms.Button()
            Me.tabPageZamayem = New System.Windows.Forms.TabPage()
            Me.pnlZamPreview = New System.Windows.Forms.Panel()
            Me.picAttachment = New System.Windows.Forms.PictureBox()
            Me.pnlZamList = New System.Windows.Forms.Panel()
            Me.dgvAttachments = New System.Windows.Forms.DataGridView()
            Me.pnlZamLineSelector = New System.Windows.Forms.Panel()
            Me.dgvZamLineSelector = New System.Windows.Forms.DataGridView()
            Me.lblZamLineSelectorTitle = New System.Windows.Forms.Label()
            Me.pnlZamButtons = New System.Windows.Forms.Panel()
            Me.btnZamAddFile = New System.Windows.Forms.Button()
            Me.btnZamScan = New System.Windows.Forms.Button()
            Me.btnZamView = New System.Windows.Forms.Button()
            Me.btnZamPrint = New System.Windows.Forms.Button()
            Me.btnZamDelete = New System.Windows.Forms.Button()
            Me.pnlZamHeader = New System.Windows.Forms.Panel()
            Me.lblSelectedLine = New System.Windows.Forms.Label()
            Me.tabPageYaddasht = New System.Windows.Forms.TabPage()
            Me.pnlNoteLineSelector = New System.Windows.Forms.Panel()
            Me.dgvNoteLineSelector = New System.Windows.Forms.DataGridView()
            Me.lblNoteLineSelectorTitle = New System.Windows.Forms.Label()
            Me.pnlNoteHeader.SuspendLayout()
            Me.pnlEntryNote.SuspendLayout()
            CType(Me.dgvEntryNotes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlEntryNoteInput.SuspendLayout()
            Me.pnlEntryNoteAction.SuspendLayout()
            Me.pnlLineNote.SuspendLayout()
            CType(Me.dgvLineNotes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlLineNoteInput.SuspendLayout()
            Me.pnlLineNoteAction.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.tabPageSanad.SuspendLayout()
            Me.pnlDgv.SuspendLayout()
            CType(Me.dgvEntryLines, System.ComponentModel.ISupportInitialize).BeginInit()
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
            CType(Me.picAttachment, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlZamList.SuspendLayout()
            CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlZamLineSelector.SuspendLayout()
            CType(Me.dgvZamLineSelector, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlZamButtons.SuspendLayout()
            Me.pnlZamHeader.SuspendLayout()
            Me.tabPageYaddasht.SuspendLayout()
            Me.pnlNoteLineSelector.SuspendLayout()
            CType(Me.dgvNoteLineSelector, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlNoteHeader
            '
            Me.pnlNoteHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(220, Byte), Integer))
            Me.pnlNoteHeader.Controls.Add(Me.lblNoteHeader)
            Me.pnlNoteHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlNoteHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlNoteHeader.Name = "pnlNoteHeader"
            Me.pnlNoteHeader.Size = New System.Drawing.Size(1312, 36)
            Me.pnlNoteHeader.TabIndex = 0
            '
            'lblNoteHeader
            '
            Me.lblNoteHeader.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblNoteHeader.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblNoteHeader.ForeColor = System.Drawing.Color.DarkGoldenrod
            Me.lblNoteHeader.Location = New System.Drawing.Point(0, 0)
            Me.lblNoteHeader.Name = "lblNoteHeader"
            Me.lblNoteHeader.Size = New System.Drawing.Size(1312, 36)
            Me.lblNoteHeader.TabIndex = 0
            Me.lblNoteHeader.Text = "لطفاً در تب «سطرهای سند» روی یک ردیف کلیک کنید"
            Me.lblNoteHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlEntryNote
            '
            Me.pnlEntryNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlEntryNote.Controls.Add(Me.dgvEntryNotes)
            Me.pnlEntryNote.Controls.Add(Me.pnlEntryNoteInput)
            Me.pnlEntryNote.Controls.Add(Me.lblEntryNoteTitle)
            Me.pnlEntryNote.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlEntryNote.Location = New System.Drawing.Point(786, 36)
            Me.pnlEntryNote.Name = "pnlEntryNote"
            Me.pnlEntryNote.Size = New System.Drawing.Size(526, 686)
            Me.pnlEntryNote.TabIndex = 1
            '
            'dgvEntryNotes
            '
            Me.dgvEntryNotes.AllowUserToAddRows = False
            Me.dgvEntryNotes.AllowUserToDeleteRows = False
            Me.dgvEntryNotes.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvEntryNotes.Location = New System.Drawing.Point(0, 30)
            Me.dgvEntryNotes.MultiSelect = False
            Me.dgvEntryNotes.Name = "dgvEntryNotes"
            Me.dgvEntryNotes.ReadOnly = True
            Me.dgvEntryNotes.RowHeadersVisible = False
            Me.dgvEntryNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvEntryNotes.Size = New System.Drawing.Size(524, 481)
            Me.dgvEntryNotes.TabIndex = 1
            '
            'pnlEntryNoteInput
            '
            Me.pnlEntryNoteInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlEntryNoteInput.Controls.Add(Me.txtEntryNote)
            Me.pnlEntryNoteInput.Controls.Add(Me.pnlEntryNoteAction)
            Me.pnlEntryNoteInput.Controls.Add(Me.lblEntryNoteInputTitle)
            Me.pnlEntryNoteInput.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlEntryNoteInput.Location = New System.Drawing.Point(0, 511)
            Me.pnlEntryNoteInput.Name = "pnlEntryNoteInput"
            Me.pnlEntryNoteInput.Size = New System.Drawing.Size(524, 173)
            Me.pnlEntryNoteInput.TabIndex = 2
            '
            'txtEntryNote
            '
            Me.txtEntryNote.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtEntryNote.Location = New System.Drawing.Point(0, 22)
            Me.txtEntryNote.Multiline = True
            Me.txtEntryNote.Name = "txtEntryNote"
            Me.txtEntryNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtEntryNote.Size = New System.Drawing.Size(524, 113)
            Me.txtEntryNote.TabIndex = 1
            '
            'pnlEntryNoteAction
            '
            Me.pnlEntryNoteAction.Controls.Add(Me.btnSaveEntryNote)
            Me.pnlEntryNoteAction.Controls.Add(Me.lblEntryNoteInfo)
            Me.pnlEntryNoteAction.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlEntryNoteAction.Location = New System.Drawing.Point(0, 135)
            Me.pnlEntryNoteAction.Name = "pnlEntryNoteAction"
            Me.pnlEntryNoteAction.Size = New System.Drawing.Size(524, 38)
            Me.pnlEntryNoteAction.TabIndex = 2
            '
            'btnSaveEntryNote
            '
            Me.btnSaveEntryNote.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnSaveEntryNote.Location = New System.Drawing.Point(490, 6)
            Me.btnSaveEntryNote.Name = "btnSaveEntryNote"
            Me.btnSaveEntryNote.Size = New System.Drawing.Size(155, 26)
            Me.btnSaveEntryNote.TabIndex = 0
            Me.btnSaveEntryNote.Text = "ذخیره یادداشت سند"
            Me.btnSaveEntryNote.UseVisualStyleBackColor = False
            '
            'lblEntryNoteInfo
            '
            Me.lblEntryNoteInfo.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblEntryNoteInfo.ForeColor = System.Drawing.Color.Gray
            Me.lblEntryNoteInfo.Location = New System.Drawing.Point(5, 9)
            Me.lblEntryNoteInfo.Name = "lblEntryNoteInfo"
            Me.lblEntryNoteInfo.Size = New System.Drawing.Size(480, 20)
            Me.lblEntryNoteInfo.TabIndex = 1
            '
            'lblEntryNoteInputTitle
            '
            Me.lblEntryNoteInputTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblEntryNoteInputTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblEntryNoteInputTitle.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblEntryNoteInputTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblEntryNoteInputTitle.Name = "lblEntryNoteInputTitle"
            Me.lblEntryNoteInputTitle.Size = New System.Drawing.Size(524, 22)
            Me.lblEntryNoteInputTitle.TabIndex = 0
            Me.lblEntryNoteInputTitle.Text = "یادداشت من برای سند :"
            Me.lblEntryNoteInputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblEntryNoteTitle
            '
            Me.lblEntryNoteTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblEntryNoteTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblEntryNoteTitle.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblEntryNoteTitle.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblEntryNoteTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblEntryNoteTitle.Name = "lblEntryNoteTitle"
            Me.lblEntryNoteTitle.Size = New System.Drawing.Size(524, 30)
            Me.lblEntryNoteTitle.TabIndex = 0
            Me.lblEntryNoteTitle.Text = "یادداشت کلی سند"
            Me.lblEntryNoteTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlLineNote
            '
            Me.pnlLineNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlLineNote.Controls.Add(Me.dgvLineNotes)
            Me.pnlLineNote.Controls.Add(Me.pnlLineNoteInput)
            Me.pnlLineNote.Controls.Add(Me.lblLineNoteTitle)
            Me.pnlLineNote.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlLineNote.Location = New System.Drawing.Point(0, 36)
            Me.pnlLineNote.Name = "pnlLineNote"
            Me.pnlLineNote.Size = New System.Drawing.Size(656, 686)
            Me.pnlLineNote.TabIndex = 2
            '
            'dgvLineNotes
            '
            Me.dgvLineNotes.AllowUserToAddRows = False
            Me.dgvLineNotes.AllowUserToDeleteRows = False
            Me.dgvLineNotes.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvLineNotes.Location = New System.Drawing.Point(0, 30)
            Me.dgvLineNotes.MultiSelect = False
            Me.dgvLineNotes.Name = "dgvLineNotes"
            Me.dgvLineNotes.ReadOnly = True
            Me.dgvLineNotes.RowHeadersVisible = False
            Me.dgvLineNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvLineNotes.Size = New System.Drawing.Size(654, 481)
            Me.dgvLineNotes.TabIndex = 1
            '
            'pnlLineNoteInput
            '
            Me.pnlLineNoteInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlLineNoteInput.Controls.Add(Me.txtLineNote)
            Me.pnlLineNoteInput.Controls.Add(Me.pnlLineNoteAction)
            Me.pnlLineNoteInput.Controls.Add(Me.lblLineNoteInputTitle)
            Me.pnlLineNoteInput.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlLineNoteInput.Location = New System.Drawing.Point(0, 511)
            Me.pnlLineNoteInput.Name = "pnlLineNoteInput"
            Me.pnlLineNoteInput.Size = New System.Drawing.Size(654, 173)
            Me.pnlLineNoteInput.TabIndex = 2
            '
            'txtLineNote
            '
            Me.txtLineNote.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtLineNote.Location = New System.Drawing.Point(0, 22)
            Me.txtLineNote.Multiline = True
            Me.txtLineNote.Name = "txtLineNote"
            Me.txtLineNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtLineNote.Size = New System.Drawing.Size(654, 113)
            Me.txtLineNote.TabIndex = 1
            '
            'pnlLineNoteAction
            '
            Me.pnlLineNoteAction.Controls.Add(Me.btnSaveLineNote)
            Me.pnlLineNoteAction.Controls.Add(Me.lblLineNoteInfo)
            Me.pnlLineNoteAction.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlLineNoteAction.Location = New System.Drawing.Point(0, 135)
            Me.pnlLineNoteAction.Name = "pnlLineNoteAction"
            Me.pnlLineNoteAction.Size = New System.Drawing.Size(654, 38)
            Me.pnlLineNoteAction.TabIndex = 2
            '
            'btnSaveLineNote
            '
            Me.btnSaveLineNote.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnSaveLineNote.Location = New System.Drawing.Point(490, 6)
            Me.btnSaveLineNote.Name = "btnSaveLineNote"
            Me.btnSaveLineNote.Size = New System.Drawing.Size(155, 26)
            Me.btnSaveLineNote.TabIndex = 0
            Me.btnSaveLineNote.Text = "ذخیره یادداشت ردیف"
            Me.btnSaveLineNote.UseVisualStyleBackColor = False
            '
            'lblLineNoteInfo
            '
            Me.lblLineNoteInfo.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblLineNoteInfo.ForeColor = System.Drawing.Color.Gray
            Me.lblLineNoteInfo.Location = New System.Drawing.Point(5, 9)
            Me.lblLineNoteInfo.Name = "lblLineNoteInfo"
            Me.lblLineNoteInfo.Size = New System.Drawing.Size(480, 20)
            Me.lblLineNoteInfo.TabIndex = 1
            '
            'lblLineNoteInputTitle
            '
            Me.lblLineNoteInputTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblLineNoteInputTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblLineNoteInputTitle.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblLineNoteInputTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblLineNoteInputTitle.Name = "lblLineNoteInputTitle"
            Me.lblLineNoteInputTitle.Size = New System.Drawing.Size(654, 22)
            Me.lblLineNoteInputTitle.TabIndex = 0
            Me.lblLineNoteInputTitle.Text = "یادداشت من برای ردیف :"
            Me.lblLineNoteInputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblLineNoteTitle
            '
            Me.lblLineNoteTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(220, Byte), Integer))
            Me.lblLineNoteTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblLineNoteTitle.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblLineNoteTitle.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblLineNoteTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblLineNoteTitle.Name = "lblLineNoteTitle"
            Me.lblLineNoteTitle.Size = New System.Drawing.Size(654, 30)
            Me.lblLineNoteTitle.TabIndex = 0
            Me.lblLineNoteTitle.Text = "یادداشت ردیف"
            Me.lblLineNoteTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'tabMain
            '
            Me.tabMain.Controls.Add(Me.tabPageSanad)
            Me.tabMain.Controls.Add(Me.tabPageZamayem)
            Me.tabMain.Controls.Add(Me.tabPageYaddasht)
            Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabMain.Location = New System.Drawing.Point(0, 0)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New System.Drawing.Size(1320, 749)
            Me.tabMain.TabIndex = 0
            '
            'tabPageSanad
            '
            Me.tabPageSanad.Controls.Add(Me.pnlDgv)
            Me.tabPageSanad.Controls.Add(Me.pnlBottomRow)
            Me.tabPageSanad.Controls.Add(Me.pnlButton)
            Me.tabPageSanad.Controls.Add(Me.pnlSerch)
            Me.tabPageSanad.Controls.Add(Me.pnlViewShenavar)
            Me.tabPageSanad.Controls.Add(Me.pnlViewSarfasl)
            Me.tabPageSanad.Controls.Add(Me.pnlNoSanad)
            Me.tabPageSanad.Location = New System.Drawing.Point(4, 23)
            Me.tabPageSanad.Name = "tabPageSanad"
            Me.tabPageSanad.Size = New System.Drawing.Size(1312, 722)
            Me.tabPageSanad.TabIndex = 0
            Me.tabPageSanad.Text = "سطرهای سند"
            Me.tabPageSanad.UseVisualStyleBackColor = True
            '
            'pnlDgv
            '
            Me.pnlDgv.Controls.Add(Me.dgvEntryLines)
            Me.pnlDgv.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlDgv.Location = New System.Drawing.Point(0, 126)
            Me.pnlDgv.Name = "pnlDgv"
            Me.pnlDgv.Size = New System.Drawing.Size(1312, 477)
            Me.pnlDgv.TabIndex = 0
            '
            'dgvEntryLines
            '
            Me.dgvEntryLines.AllowUserToAddRows = False
            Me.dgvEntryLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvEntryLines.Location = New System.Drawing.Point(0, 0)
            Me.dgvEntryLines.Name = "dgvEntryLines"
            Me.dgvEntryLines.RowHeadersVisible = False
            Me.dgvEntryLines.Size = New System.Drawing.Size(1312, 477)
            Me.dgvEntryLines.TabIndex = 0
            '
            'pnlBottomRow
            '
            Me.pnlBottomRow.Controls.Add(Me.pnlJamSanad)
            Me.pnlBottomRow.Controls.Add(Me.pnlSharhSanad)
            Me.pnlBottomRow.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottomRow.Location = New System.Drawing.Point(0, 603)
            Me.pnlBottomRow.Name = "pnlBottomRow"
            Me.pnlBottomRow.Size = New System.Drawing.Size(1312, 75)
            Me.pnlBottomRow.TabIndex = 1
            '
            'pnlJamSanad
            '
            Me.pnlJamSanad.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlJamSanad.Controls.Add(Me.Label1)
            Me.pnlJamSanad.Controls.Add(Me.lblJamTitle)
            Me.pnlJamSanad.Controls.Add(Me.txtJamBedehkar)
            Me.pnlJamSanad.Controls.Add(Me.txtJamBestankar)
            Me.pnlJamSanad.Controls.Add(Me.lblTaeazLabel)
            Me.pnlJamSanad.Controls.Add(Me.lblSanadLabel)
            Me.pnlJamSanad.Controls.Add(Me.lblSanadStatus)
            Me.pnlJamSanad.Controls.Add(Me.lblKasriTitle)
            Me.pnlJamSanad.Controls.Add(Me.txtKasriDebit)
            Me.pnlJamSanad.Controls.Add(Me.txtKasriCredit)
            Me.pnlJamSanad.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlJamSanad.Location = New System.Drawing.Point(0, 0)
            Me.pnlJamSanad.Name = "pnlJamSanad"
            Me.pnlJamSanad.Size = New System.Drawing.Size(862, 75)
            Me.pnlJamSanad.TabIndex = 0
            '
            'Label1
            '
            Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Label1.Location = New System.Drawing.Point(12, 9)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 22)
            Me.Label1.TabIndex = 10
            Me.Label1.Text = "است"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblJamTitle
            '
            Me.lblJamTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblJamTitle.Location = New System.Drawing.Point(715, 9)
            Me.lblJamTitle.Name = "lblJamTitle"
            Me.lblJamTitle.Size = New System.Drawing.Size(150, 22)
            Me.lblJamTitle.TabIndex = 0
            Me.lblJamTitle.Text = "جمـــع :"
            Me.lblJamTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtJamBedehkar
            '
            Me.txtJamBedehkar.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.txtJamBedehkar.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtJamBedehkar.Location = New System.Drawing.Point(510, 6)
            Me.txtJamBedehkar.Name = "txtJamBedehkar"
            Me.txtJamBedehkar.ReadOnly = True
            Me.txtJamBedehkar.Size = New System.Drawing.Size(200, 22)
            Me.txtJamBedehkar.TabIndex = 1
            Me.txtJamBedehkar.Text = "0"
            Me.txtJamBedehkar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'txtJamBestankar
            '
            Me.txtJamBestankar.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.txtJamBestankar.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtJamBestankar.Location = New System.Drawing.Point(305, 6)
            Me.txtJamBestankar.Name = "txtJamBestankar"
            Me.txtJamBestankar.ReadOnly = True
            Me.txtJamBestankar.Size = New System.Drawing.Size(200, 22)
            Me.txtJamBestankar.TabIndex = 2
            Me.txtJamBestankar.Text = "0"
            Me.txtJamBestankar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblTaeazLabel
            '
            Me.lblTaeazLabel.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblTaeazLabel.Location = New System.Drawing.Point(195, 9)
            Me.lblTaeazLabel.Name = "lblTaeazLabel"
            Me.lblTaeazLabel.Size = New System.Drawing.Size(70, 22)
            Me.lblTaeazLabel.TabIndex = 3
            Me.lblTaeazLabel.Text = "تعادل سند :"
            Me.lblTaeazLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSanadLabel
            '
            Me.lblSanadLabel.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblSanadLabel.Location = New System.Drawing.Point(152, 9)
            Me.lblSanadLabel.Name = "lblSanadLabel"
            Me.lblSanadLabel.Size = New System.Drawing.Size(35, 22)
            Me.lblSanadLabel.TabIndex = 4
            Me.lblSanadLabel.Text = "سند"
            Me.lblSanadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSanadStatus
            '
            Me.lblSanadStatus.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblSanadStatus.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblSanadStatus.Location = New System.Drawing.Point(59, 6)
            Me.lblSanadStatus.Name = "lblSanadStatus"
            Me.lblSanadStatus.Size = New System.Drawing.Size(80, 28)
            Me.lblSanadStatus.TabIndex = 5
            Me.lblSanadStatus.Text = "تراز"
            Me.lblSanadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblKasriTitle
            '
            Me.lblKasriTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblKasriTitle.Location = New System.Drawing.Point(715, 46)
            Me.lblKasriTitle.Name = "lblKasriTitle"
            Me.lblKasriTitle.Size = New System.Drawing.Size(150, 22)
            Me.lblKasriTitle.TabIndex = 7
            Me.lblKasriTitle.Text = "کسری تا تراز :"
            Me.lblKasriTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtKasriDebit
            '
            Me.txtKasriDebit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(193, Byte), Integer))
            Me.txtKasriDebit.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtKasriDebit.Location = New System.Drawing.Point(510, 43)
            Me.txtKasriDebit.Name = "txtKasriDebit"
            Me.txtKasriDebit.ReadOnly = True
            Me.txtKasriDebit.Size = New System.Drawing.Size(200, 22)
            Me.txtKasriDebit.TabIndex = 8
            Me.txtKasriDebit.Text = "0"
            Me.txtKasriDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'txtKasriCredit
            '
            Me.txtKasriCredit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(150, Byte), Integer))
            Me.txtKasriCredit.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtKasriCredit.Location = New System.Drawing.Point(305, 43)
            Me.txtKasriCredit.Name = "txtKasriCredit"
            Me.txtKasriCredit.ReadOnly = True
            Me.txtKasriCredit.Size = New System.Drawing.Size(200, 22)
            Me.txtKasriCredit.TabIndex = 9
            Me.txtKasriCredit.Text = "0"
            Me.txtKasriCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'pnlSharhSanad
            '
            Me.pnlSharhSanad.Controls.Add(Me.lblEntryDesc)
            Me.pnlSharhSanad.Controls.Add(Me.txtEntryDescription)
            Me.pnlSharhSanad.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlSharhSanad.Location = New System.Drawing.Point(862, 0)
            Me.pnlSharhSanad.Name = "pnlSharhSanad"
            Me.pnlSharhSanad.Size = New System.Drawing.Size(450, 75)
            Me.pnlSharhSanad.TabIndex = 1
            '
            'lblEntryDesc
            '
            Me.lblEntryDesc.Location = New System.Drawing.Point(375, 11)
            Me.lblEntryDesc.Name = "lblEntryDesc"
            Me.lblEntryDesc.Size = New System.Drawing.Size(70, 20)
            Me.lblEntryDesc.TabIndex = 0
            Me.lblEntryDesc.Text = "شرح سند:"
            '
            'txtEntryDescription
            '
            Me.txtEntryDescription.Location = New System.Drawing.Point(5, 9)
            Me.txtEntryDescription.Multiline = True
            Me.txtEntryDescription.Name = "txtEntryDescription"
            Me.txtEntryDescription.Size = New System.Drawing.Size(365, 58)
            Me.txtEntryDescription.TabIndex = 1
            '
            'pnlButton
            '
            Me.pnlButton.Controls.Add(Me.btnSaveAndContinue)
            Me.pnlButton.Controls.Add(Me.btnSaveEntry)
            Me.pnlButton.Controls.Add(Me.btnAddLine)
            Me.pnlButton.Controls.Add(Me.btnDeleteRow)
            Me.pnlButton.Controls.Add(Me.btnCopyBelow)
            Me.pnlButton.Controls.Add(Me.btnCopyAbove)
            Me.pnlButton.Controls.Add(Me.btnCopyToPos)
            Me.pnlButton.Controls.Add(Me.btnClearSearch)
            Me.pnlButton.Controls.Add(Me.btnExit)
            Me.pnlButton.Controls.Add(Me.btnPrintVoucher)
            Me.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlButton.Location = New System.Drawing.Point(0, 678)
            Me.pnlButton.Name = "pnlButton"
            Me.pnlButton.Size = New System.Drawing.Size(1312, 44)
            Me.pnlButton.TabIndex = 2
            '
            'btnSaveAndContinue
            '
            Me.btnSaveAndContinue.Location = New System.Drawing.Point(826, 8)
            Me.btnSaveAndContinue.Name = "btnSaveAndContinue"
            Me.btnSaveAndContinue.Size = New System.Drawing.Size(140, 28)
            Me.btnSaveAndContinue.TabIndex = 7
            Me.btnSaveAndContinue.Text = "ثبت سند و ادامه"
            '
            'btnSaveEntry
            '
            Me.btnSaveEntry.Location = New System.Drawing.Point(974, 8)
            Me.btnSaveEntry.Name = "btnSaveEntry"
            Me.btnSaveEntry.Size = New System.Drawing.Size(137, 28)
            Me.btnSaveEntry.TabIndex = 8
            Me.btnSaveEntry.Text = "ثبت سند و خروج"
            '
            'btnAddLine
            '
            Me.btnAddLine.Location = New System.Drawing.Point(693, 8)
            Me.btnAddLine.Name = "btnAddLine"
            Me.btnAddLine.Size = New System.Drawing.Size(125, 28)
            Me.btnAddLine.TabIndex = 6
            Me.btnAddLine.Text = "افزودن سطر خالی"
            '
            'btnDeleteRow
            '
            Me.btnDeleteRow.Location = New System.Drawing.Point(570, 8)
            Me.btnDeleteRow.Name = "btnDeleteRow"
            Me.btnDeleteRow.Size = New System.Drawing.Size(115, 28)
            Me.btnDeleteRow.TabIndex = 5
            Me.btnDeleteRow.Text = "حذف سطر جاری"
            '
            'btnCopyBelow
            '
            Me.btnCopyBelow.Location = New System.Drawing.Point(467, 8)
            Me.btnCopyBelow.Name = "btnCopyBelow"
            Me.btnCopyBelow.Size = New System.Drawing.Size(95, 28)
            Me.btnCopyBelow.TabIndex = 4
            Me.btnCopyBelow.Text = "کپی در زیر"
            '
            'btnCopyAbove
            '
            Me.btnCopyAbove.Location = New System.Drawing.Point(364, 8)
            Me.btnCopyAbove.Name = "btnCopyAbove"
            Me.btnCopyAbove.Size = New System.Drawing.Size(95, 28)
            Me.btnCopyAbove.TabIndex = 3
            Me.btnCopyAbove.Text = "کپی در بالا"
            '
            'btnCopyToPos
            '
            Me.btnCopyToPos.Location = New System.Drawing.Point(226, 8)
            Me.btnCopyToPos.Name = "btnCopyToPos"
            Me.btnCopyToPos.Size = New System.Drawing.Size(130, 28)
            Me.btnCopyToPos.TabIndex = 2
            Me.btnCopyToPos.Text = "کپی در محل دلخواه"
            '
            'btnClearSearch
            '
            Me.btnClearSearch.Location = New System.Drawing.Point(88, 8)
            Me.btnClearSearch.Name = "btnClearSearch"
            Me.btnClearSearch.Size = New System.Drawing.Size(130, 28)
            Me.btnClearSearch.TabIndex = 1
            Me.btnClearSearch.Text = "پاک کردن جستجوها"
            '
            'btnExit
            '
            Me.btnExit.Location = New System.Drawing.Point(10, 8)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(70, 28)
            Me.btnExit.TabIndex = 0
            Me.btnExit.Text = "خروج"
            '
            'btnPrintVoucher
            '
            Me.btnPrintVoucher.Location = New System.Drawing.Point(1118, 8)
            Me.btnPrintVoucher.Name = "btnPrintVoucher"
            Me.btnPrintVoucher.Size = New System.Drawing.Size(120, 28)
            Me.btnPrintVoucher.TabIndex = 9
            Me.btnPrintVoucher.Text = "چاپ سند (Ctrl+P)"
            Me.btnPrintVoucher.UseVisualStyleBackColor = True
            '
            'pnlSerch
            '
            Me.pnlSerch.Controls.Add(Me.txtSrcLineNo)
            Me.pnlSerch.Controls.Add(Me.txtSrcSF)
            Me.pnlSerch.Controls.Add(Me.txtSrcSH)
            Me.pnlSerch.Controls.Add(Me.txtSrcSharh)
            Me.pnlSerch.Controls.Add(Me.txtSrcDebit)
            Me.pnlSerch.Controls.Add(Me.txtSrcCredit)
            Me.pnlSerch.Controls.Add(Me.txtSrcTransNo)
            Me.pnlSerch.Controls.Add(Me.txtSrcTransDate)
            Me.pnlSerch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSerch.Location = New System.Drawing.Point(0, 96)
            Me.pnlSerch.Name = "pnlSerch"
            Me.pnlSerch.Size = New System.Drawing.Size(1312, 30)
            Me.pnlSerch.TabIndex = 3
            '
            'txtSrcLineNo
            '
            Me.txtSrcLineNo.Location = New System.Drawing.Point(1245, 4)
            Me.txtSrcLineNo.Name = "txtSrcLineNo"
            Me.txtSrcLineNo.Size = New System.Drawing.Size(45, 22)
            Me.txtSrcLineNo.TabIndex = 0
            '
            'txtSrcSF
            '
            Me.txtSrcSF.Location = New System.Drawing.Point(1198, 4)
            Me.txtSrcSF.Name = "txtSrcSF"
            Me.txtSrcSF.Size = New System.Drawing.Size(35, 22)
            Me.txtSrcSF.TabIndex = 1
            '
            'txtSrcSH
            '
            Me.txtSrcSH.Location = New System.Drawing.Point(1145, 4)
            Me.txtSrcSH.Name = "txtSrcSH"
            Me.txtSrcSH.Size = New System.Drawing.Size(35, 22)
            Me.txtSrcSH.TabIndex = 2
            '
            'txtSrcSharh
            '
            Me.txtSrcSharh.Location = New System.Drawing.Point(775, 5)
            Me.txtSrcSharh.Name = "txtSrcSharh"
            Me.txtSrcSharh.Size = New System.Drawing.Size(231, 22)
            Me.txtSrcSharh.TabIndex = 3
            '
            'txtSrcDebit
            '
            Me.txtSrcDebit.Location = New System.Drawing.Point(352, 6)
            Me.txtSrcDebit.Name = "txtSrcDebit"
            Me.txtSrcDebit.Size = New System.Drawing.Size(200, 22)
            Me.txtSrcDebit.TabIndex = 4
            '
            'txtSrcCredit
            '
            Me.txtSrcCredit.Location = New System.Drawing.Point(558, 6)
            Me.txtSrcCredit.Name = "txtSrcCredit"
            Me.txtSrcCredit.Size = New System.Drawing.Size(200, 22)
            Me.txtSrcCredit.TabIndex = 5
            '
            'txtSrcTransNo
            '
            Me.txtSrcTransNo.Location = New System.Drawing.Point(226, 5)
            Me.txtSrcTransNo.Name = "txtSrcTransNo"
            Me.txtSrcTransNo.Size = New System.Drawing.Size(120, 22)
            Me.txtSrcTransNo.TabIndex = 6
            '
            'txtSrcTransDate
            '
            Me.txtSrcTransDate.Location = New System.Drawing.Point(106, 4)
            Me.txtSrcTransDate.Name = "txtSrcTransDate"
            Me.txtSrcTransDate.Size = New System.Drawing.Size(95, 22)
            Me.txtSrcTransDate.TabIndex = 7
            '
            'pnlViewShenavar
            '
            Me.pnlViewShenavar.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.pnlViewShenavar.Controls.Add(Me.lblShenavarValue)
            Me.pnlViewShenavar.Controls.Add(Me.lblShenavarTitle)
            Me.pnlViewShenavar.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlViewShenavar.Location = New System.Drawing.Point(0, 68)
            Me.pnlViewShenavar.Name = "pnlViewShenavar"
            Me.pnlViewShenavar.Size = New System.Drawing.Size(1312, 28)
            Me.pnlViewShenavar.TabIndex = 4
            '
            'lblShenavarValue
            '
            Me.lblShenavarValue.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblShenavarValue.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblShenavarValue.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblShenavarValue.Location = New System.Drawing.Point(0, 0)
            Me.lblShenavarValue.Name = "lblShenavarValue"
            Me.lblShenavarValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.lblShenavarValue.Size = New System.Drawing.Size(1044, 28)
            Me.lblShenavarValue.TabIndex = 0
            Me.lblShenavarValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblShenavarTitle
            '
            Me.lblShenavarTitle.Dock = System.Windows.Forms.DockStyle.Right
            Me.lblShenavarTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblShenavarTitle.Location = New System.Drawing.Point(1044, 0)
            Me.lblShenavarTitle.Name = "lblShenavarTitle"
            Me.lblShenavarTitle.Size = New System.Drawing.Size(268, 28)
            Me.lblShenavarTitle.TabIndex = 1
            Me.lblShenavarTitle.Text = "کد و نام حساب شنـــــاور ردیف جاری :"
            Me.lblShenavarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pnlViewSarfasl
            '
            Me.pnlViewSarfasl.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlViewSarfasl.Controls.Add(Me.lblSarfaslValue)
            Me.pnlViewSarfasl.Controls.Add(Me.lblSarfaslTitle)
            Me.pnlViewSarfasl.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlViewSarfasl.Location = New System.Drawing.Point(0, 40)
            Me.pnlViewSarfasl.Name = "pnlViewSarfasl"
            Me.pnlViewSarfasl.Size = New System.Drawing.Size(1312, 28)
            Me.pnlViewSarfasl.TabIndex = 5
            '
            'lblSarfaslValue
            '
            Me.lblSarfaslValue.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSarfaslValue.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblSarfaslValue.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblSarfaslValue.Location = New System.Drawing.Point(0, 0)
            Me.lblSarfaslValue.Name = "lblSarfaslValue"
            Me.lblSarfaslValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.lblSarfaslValue.Size = New System.Drawing.Size(1044, 28)
            Me.lblSarfaslValue.TabIndex = 0
            Me.lblSarfaslValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSarfaslTitle
            '
            Me.lblSarfaslTitle.Dock = System.Windows.Forms.DockStyle.Right
            Me.lblSarfaslTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblSarfaslTitle.Location = New System.Drawing.Point(1044, 0)
            Me.lblSarfaslTitle.Name = "lblSarfaslTitle"
            Me.lblSarfaslTitle.Size = New System.Drawing.Size(268, 28)
            Me.lblSarfaslTitle.TabIndex = 1
            Me.lblSarfaslTitle.Text = "کد و نام حساب سرفصل ردیف جاری :"
            Me.lblSarfaslTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pnlNoSanad
            '
            Me.pnlNoSanad.Controls.Add(Me.lblEntryRef)
            Me.pnlNoSanad.Controls.Add(Me.txtEntryReference)
            Me.pnlNoSanad.Controls.Add(Me.lblDateSanad)
            Me.pnlNoSanad.Controls.Add(Me.txtDateSanad)
            Me.pnlNoSanad.Controls.Add(Me.btnCalDate)
            Me.pnlNoSanad.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlNoSanad.Location = New System.Drawing.Point(0, 0)
            Me.pnlNoSanad.Name = "pnlNoSanad"
            Me.pnlNoSanad.Size = New System.Drawing.Size(1312, 40)
            Me.pnlNoSanad.TabIndex = 6
            '
            'lblEntryRef
            '
            Me.lblEntryRef.Location = New System.Drawing.Point(1198, 12)
            Me.lblEntryRef.Name = "lblEntryRef"
            Me.lblEntryRef.Size = New System.Drawing.Size(75, 20)
            Me.lblEntryRef.TabIndex = 0
            Me.lblEntryRef.Text = "شماره سند:"
            '
            'txtEntryReference
            '
            Me.txtEntryReference.Location = New System.Drawing.Point(1052, 10)
            Me.txtEntryReference.Name = "txtEntryReference"
            Me.txtEntryReference.Size = New System.Drawing.Size(140, 22)
            Me.txtEntryReference.TabIndex = 1
            '
            'lblDateSanad
            '
            Me.lblDateSanad.Location = New System.Drawing.Point(948, 12)
            Me.lblDateSanad.Name = "lblDateSanad"
            Me.lblDateSanad.Size = New System.Drawing.Size(72, 20)
            Me.lblDateSanad.TabIndex = 2
            Me.lblDateSanad.Text = "تاریخ سند:"
            '
            'txtDateSanad
            '
            Me.txtDateSanad.Location = New System.Drawing.Point(790, 9)
            Me.txtDateSanad.MaxLength = 10
            Me.txtDateSanad.Name = "txtDateSanad"
            Me.txtDateSanad.Size = New System.Drawing.Size(110, 22)
            Me.txtDateSanad.TabIndex = 3
            '
            'btnCalDate
            '
            Me.btnCalDate.Location = New System.Drawing.Point(905, 8)
            Me.btnCalDate.Name = "btnCalDate"
            Me.btnCalDate.Size = New System.Drawing.Size(26, 24)
            Me.btnCalDate.TabIndex = 4
            Me.btnCalDate.Text = "..."
            '
            'tabPageZamayem
            '
            Me.tabPageZamayem.Controls.Add(Me.pnlZamPreview)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamList)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamLineSelector)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamButtons)
            Me.tabPageZamayem.Controls.Add(Me.pnlZamHeader)
            Me.tabPageZamayem.Location = New System.Drawing.Point(4, 23)
            Me.tabPageZamayem.Name = "tabPageZamayem"
            Me.tabPageZamayem.Size = New System.Drawing.Size(1312, 722)
            Me.tabPageZamayem.TabIndex = 1
            Me.tabPageZamayem.Text = "ضمائم سند"
            Me.tabPageZamayem.UseVisualStyleBackColor = True
            '
            'pnlZamPreview
            '
            Me.pnlZamPreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
            Me.pnlZamPreview.Controls.Add(Me.picAttachment)
            Me.pnlZamPreview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlZamPreview.Location = New System.Drawing.Point(0, 36)
            Me.pnlZamPreview.Name = "pnlZamPreview"
            Me.pnlZamPreview.Size = New System.Drawing.Size(782, 642)
            Me.pnlZamPreview.TabIndex = 3
            '
            'picAttachment
            '
            Me.picAttachment.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
            Me.picAttachment.Dock = System.Windows.Forms.DockStyle.Fill
            Me.picAttachment.Location = New System.Drawing.Point(0, 0)
            Me.picAttachment.Name = "picAttachment"
            Me.picAttachment.Size = New System.Drawing.Size(782, 642)
            Me.picAttachment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picAttachment.TabIndex = 0
            Me.picAttachment.TabStop = False
            '
            'pnlZamList
            '
            Me.pnlZamList.Controls.Add(Me.dgvAttachments)
            Me.pnlZamList.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlZamList.Location = New System.Drawing.Point(782, 36)
            Me.pnlZamList.Name = "pnlZamList"
            Me.pnlZamList.Size = New System.Drawing.Size(400, 642)
            Me.pnlZamList.TabIndex = 2
            '
            'dgvAttachments
            '
            Me.dgvAttachments.AllowUserToAddRows = False
            Me.dgvAttachments.AllowUserToDeleteRows = False
            Me.dgvAttachments.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAttachments.Location = New System.Drawing.Point(0, 0)
            Me.dgvAttachments.MultiSelect = False
            Me.dgvAttachments.Name = "dgvAttachments"
            Me.dgvAttachments.ReadOnly = True
            Me.dgvAttachments.RowHeadersVisible = False
            Me.dgvAttachments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvAttachments.Size = New System.Drawing.Size(400, 642)
            Me.dgvAttachments.TabIndex = 0
            '
            'pnlZamLineSelector
            '
            Me.pnlZamLineSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlZamLineSelector.Controls.Add(Me.dgvZamLineSelector)
            Me.pnlZamLineSelector.Controls.Add(Me.lblZamLineSelectorTitle)
            Me.pnlZamLineSelector.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlZamLineSelector.Location = New System.Drawing.Point(1182, 36)
            Me.pnlZamLineSelector.Name = "pnlZamLineSelector"
            Me.pnlZamLineSelector.Size = New System.Drawing.Size(130, 642)
            Me.pnlZamLineSelector.TabIndex = 4
            '
            'dgvZamLineSelector
            '
            Me.dgvZamLineSelector.AllowUserToAddRows = False
            Me.dgvZamLineSelector.AllowUserToDeleteRows = False
            Me.dgvZamLineSelector.BackgroundColor = System.Drawing.SystemColors.Window
            Me.dgvZamLineSelector.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvZamLineSelector.Location = New System.Drawing.Point(0, 30)
            Me.dgvZamLineSelector.MultiSelect = False
            Me.dgvZamLineSelector.Name = "dgvZamLineSelector"
            Me.dgvZamLineSelector.ReadOnly = True
            Me.dgvZamLineSelector.RowHeadersVisible = False
            Me.dgvZamLineSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvZamLineSelector.Size = New System.Drawing.Size(128, 610)
            Me.dgvZamLineSelector.TabIndex = 1
            '
            'lblZamLineSelectorTitle
            '
            Me.lblZamLineSelectorTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblZamLineSelectorTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblZamLineSelectorTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.lblZamLineSelectorTitle.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblZamLineSelectorTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblZamLineSelectorTitle.Name = "lblZamLineSelectorTitle"
            Me.lblZamLineSelectorTitle.Size = New System.Drawing.Size(128, 30)
            Me.lblZamLineSelectorTitle.TabIndex = 0
            Me.lblZamLineSelectorTitle.Text = "ردیف‌های دارای ضمیمه"
            Me.lblZamLineSelectorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlZamButtons
            '
            Me.pnlZamButtons.Controls.Add(Me.btnZamAddFile)
            Me.pnlZamButtons.Controls.Add(Me.btnZamScan)
            Me.pnlZamButtons.Controls.Add(Me.btnZamView)
            Me.pnlZamButtons.Controls.Add(Me.btnZamPrint)
            Me.pnlZamButtons.Controls.Add(Me.btnZamDelete)
            Me.pnlZamButtons.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlZamButtons.Location = New System.Drawing.Point(0, 678)
            Me.pnlZamButtons.Name = "pnlZamButtons"
            Me.pnlZamButtons.Size = New System.Drawing.Size(1312, 44)
            Me.pnlZamButtons.TabIndex = 1
            '
            'btnZamAddFile
            '
            Me.btnZamAddFile.Location = New System.Drawing.Point(1112, 8)
            Me.btnZamAddFile.Name = "btnZamAddFile"
            Me.btnZamAddFile.Size = New System.Drawing.Size(140, 28)
            Me.btnZamAddFile.TabIndex = 0
            Me.btnZamAddFile.Text = "انتخاب از فایل"
            '
            'btnZamScan
            '
            Me.btnZamScan.Location = New System.Drawing.Point(962, 8)
            Me.btnZamScan.Name = "btnZamScan"
            Me.btnZamScan.Size = New System.Drawing.Size(140, 28)
            Me.btnZamScan.TabIndex = 1
            Me.btnZamScan.Text = "اسکن تصویر"
            '
            'btnZamView
            '
            Me.btnZamView.Location = New System.Drawing.Point(812, 8)
            Me.btnZamView.Name = "btnZamView"
            Me.btnZamView.Size = New System.Drawing.Size(140, 28)
            Me.btnZamView.TabIndex = 2
            Me.btnZamView.Text = "مشاهده کامل"
            '
            'btnZamPrint
            '
            Me.btnZamPrint.Location = New System.Drawing.Point(662, 8)
            Me.btnZamPrint.Name = "btnZamPrint"
            Me.btnZamPrint.Size = New System.Drawing.Size(140, 28)
            Me.btnZamPrint.TabIndex = 3
            Me.btnZamPrint.Text = "چاپ ضمیمه"
            '
            'btnZamDelete
            '
            Me.btnZamDelete.ForeColor = System.Drawing.Color.DarkRed
            Me.btnZamDelete.Location = New System.Drawing.Point(512, 8)
            Me.btnZamDelete.Name = "btnZamDelete"
            Me.btnZamDelete.Size = New System.Drawing.Size(140, 28)
            Me.btnZamDelete.TabIndex = 4
            Me.btnZamDelete.Text = "حذف ضمیمه"
            '
            'pnlZamHeader
            '
            Me.pnlZamHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlZamHeader.Controls.Add(Me.lblSelectedLine)
            Me.pnlZamHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlZamHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlZamHeader.Name = "pnlZamHeader"
            Me.pnlZamHeader.Size = New System.Drawing.Size(1312, 36)
            Me.pnlZamHeader.TabIndex = 0
            '
            'lblSelectedLine
            '
            Me.lblSelectedLine.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSelectedLine.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblSelectedLine.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblSelectedLine.Location = New System.Drawing.Point(0, 0)
            Me.lblSelectedLine.Name = "lblSelectedLine"
            Me.lblSelectedLine.Size = New System.Drawing.Size(1312, 36)
            Me.lblSelectedLine.TabIndex = 0
            Me.lblSelectedLine.Text = "لطفاً در تب «سطرهای سند» روی یک ردیف کلیک کنید تا ضمائم آن نمایش داده شود"
            Me.lblSelectedLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'tabPageYaddasht
            '
            Me.tabPageYaddasht.Controls.Add(Me.pnlLineNote)
            Me.tabPageYaddasht.Controls.Add(Me.pnlNoteLineSelector)
            Me.tabPageYaddasht.Controls.Add(Me.pnlEntryNote)
            Me.tabPageYaddasht.Controls.Add(Me.pnlNoteHeader)
            Me.tabPageYaddasht.Location = New System.Drawing.Point(4, 23)
            Me.tabPageYaddasht.Name = "tabPageYaddasht"
            Me.tabPageYaddasht.Size = New System.Drawing.Size(1312, 722)
            Me.tabPageYaddasht.TabIndex = 2
            Me.tabPageYaddasht.Text = "یادداشت برای سند"
            Me.tabPageYaddasht.UseVisualStyleBackColor = True
            '
            'pnlNoteLineSelector
            '
            Me.pnlNoteLineSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlNoteLineSelector.Controls.Add(Me.dgvNoteLineSelector)
            Me.pnlNoteLineSelector.Controls.Add(Me.lblNoteLineSelectorTitle)
            Me.pnlNoteLineSelector.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlNoteLineSelector.Location = New System.Drawing.Point(656, 36)
            Me.pnlNoteLineSelector.Name = "pnlNoteLineSelector"
            Me.pnlNoteLineSelector.Size = New System.Drawing.Size(130, 686)
            Me.pnlNoteLineSelector.TabIndex = 3
            '
            'dgvNoteLineSelector
            '
            Me.dgvNoteLineSelector.AllowUserToAddRows = False
            Me.dgvNoteLineSelector.AllowUserToDeleteRows = False
            Me.dgvNoteLineSelector.BackgroundColor = System.Drawing.SystemColors.Window
            Me.dgvNoteLineSelector.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvNoteLineSelector.Location = New System.Drawing.Point(0, 30)
            Me.dgvNoteLineSelector.MultiSelect = False
            Me.dgvNoteLineSelector.Name = "dgvNoteLineSelector"
            Me.dgvNoteLineSelector.ReadOnly = True
            Me.dgvNoteLineSelector.RowHeadersVisible = False
            Me.dgvNoteLineSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvNoteLineSelector.Size = New System.Drawing.Size(128, 654)
            Me.dgvNoteLineSelector.TabIndex = 1
            '
            'lblNoteLineSelectorTitle
            '
            Me.lblNoteLineSelectorTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.lblNoteLineSelectorTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblNoteLineSelectorTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.lblNoteLineSelectorTitle.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblNoteLineSelectorTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblNoteLineSelectorTitle.Name = "lblNoteLineSelectorTitle"
            Me.lblNoteLineSelectorTitle.Size = New System.Drawing.Size(128, 30)
            Me.lblNoteLineSelectorTitle.TabIndex = 0
            Me.lblNoteLineSelectorTitle.Text = "ردیف‌های دارای یادداشت"
            Me.lblNoteLineSelectorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'HesabdarySanad2Form
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1320, 749)
            Me.Controls.Add(Me.tabMain)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdarySanad2Form"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ثبت ردیف‌های سند"
            Me.pnlNoteHeader.ResumeLayout(False)
            Me.pnlEntryNote.ResumeLayout(False)
            CType(Me.dgvEntryNotes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlEntryNoteInput.ResumeLayout(False)
            Me.pnlEntryNoteInput.PerformLayout()
            Me.pnlEntryNoteAction.ResumeLayout(False)
            Me.pnlLineNote.ResumeLayout(False)
            CType(Me.dgvLineNotes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlLineNoteInput.ResumeLayout(False)
            Me.pnlLineNoteInput.PerformLayout()
            Me.pnlLineNoteAction.ResumeLayout(False)
            Me.tabMain.ResumeLayout(False)
            Me.tabPageSanad.ResumeLayout(False)
            Me.pnlDgv.ResumeLayout(False)
            CType(Me.dgvEntryLines, System.ComponentModel.ISupportInitialize).EndInit()
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
            CType(Me.picAttachment, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlZamList.ResumeLayout(False)
            CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlZamLineSelector.ResumeLayout(False)
            CType(Me.dgvZamLineSelector, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlZamButtons.ResumeLayout(False)
            Me.pnlZamHeader.ResumeLayout(False)
            Me.tabPageYaddasht.ResumeLayout(False)
            Me.pnlNoteLineSelector.ResumeLayout(False)
            CType(Me.dgvNoteLineSelector, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents Label1 As Label
    End Class
End Namespace
