Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class HesabdaryDaftarShenavarForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlHeader As Panel
        Friend WithEvents pnlTanzim As Panel
        Friend WithEvents btnSelectAccountsPopup As Button
        Friend WithEvents cmbSelectedAccounts As ComboBox
        Friend WithEvents lblAccountTitle As Label
        Friend WithEvents btnBackToTrial As Button
        Friend WithEvents chkAggregate As CheckBox
        Friend WithEvents chkRecalculateBalance As CheckBox
        Friend WithEvents dgvLedger As DataGridView
        Friend WithEvents pnlJamDaftar As Panel
        Friend WithEvents lblJamTitle As Label
        Friend WithEvents lblSumDebit As Label
        Friend WithEvents lblSumCredit As Label
        Friend WithEvents lblTash As Label
        Friend WithEvents lblSumBalance As Label

        Friend WithEvents pnlFilters As Panel
        Friend WithEvents chkFilterByDate As CheckBox
        Friend WithEvents lblFromDate As Label
        Friend WithEvents txtFromDate As MaskedTextBox
        Friend WithEvents btnFromDate As Button
        Friend WithEvents lblToDate As Label
        Friend WithEvents txtToDate As MaskedTextBox
        Friend WithEvents btnToDate As Button
        Friend WithEvents chkFilterByDoc As CheckBox
        Friend WithEvents lblFromDoc As Label
        Friend WithEvents txtFromDoc As TextBox
        Friend WithEvents lblToDoc As Label
        Friend WithEvents txtToDoc As TextBox
        Friend WithEvents chkFilterByStatus As CheckBox
        Friend WithEvents cmbStatus As ComboBox
        Friend WithEvents lblDescType As Label
        Friend WithEvents cmbDescType As ComboBox
        Friend WithEvents btnRefresh As Button
        Friend WithEvents btnPrintDaftar As Button
        Friend WithEvents btnExportExcel As Button
        Friend WithEvents pnlSerch As Panel
        Friend WithEvents txtSrcRefNo As TextBox
        Friend WithEvents txtSrcLineNo As TextBox
        Friend WithEvents txtSrcDate As TextBox
        Friend WithEvents txtSrcSharh As TextBox
        Friend WithEvents txtSrcAccountCode As TextBox
        Friend WithEvents txtSrcAccountName As TextBox
        Friend WithEvents txtSrcDebit As TextBox
        Friend WithEvents txtSrcCredit As TextBox
        Friend WithEvents txtSrcTash As TextBox
        Friend WithEvents txtSrcBalance As TextBox

        Friend WithEvents colGoToDoc As DataGridViewButtonColumn
        Friend WithEvents colRefNo As DataGridViewTextBoxColumn
        Friend WithEvents colLineNo As DataGridViewTextBoxColumn
        Friend WithEvents colDate As DataGridViewTextBoxColumn
        Friend WithEvents colSharh As DataGridViewTextBoxColumn
        Friend WithEvents colAccountCode As DataGridViewTextBoxColumn
        Friend WithEvents colAccountName As DataGridViewTextBoxColumn
        Friend WithEvents colDebit As DataGridViewTextBoxColumn
        Friend WithEvents colCredit As DataGridViewTextBoxColumn
        Friend WithEvents colTash As DataGridViewTextBoxColumn
        Friend WithEvents colBalance As DataGridViewTextBoxColumn
        Friend WithEvents btnClearSearch As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblAccountTitle = New System.Windows.Forms.Label()
            Me.btnExportExcel = New System.Windows.Forms.Button()
            Me.pnlTanzim = New System.Windows.Forms.Panel()
            Me.cmbDescType = New System.Windows.Forms.ComboBox()
            Me.btnSelectAccountsPopup = New System.Windows.Forms.Button()
            Me.cmbSelectedAccounts = New System.Windows.Forms.ComboBox()
            Me.btnRefresh = New System.Windows.Forms.Button()
            Me.btnPrintDaftar = New System.Windows.Forms.Button()
            Me.lblDescType = New System.Windows.Forms.Label()
            Me.chkAggregate = New System.Windows.Forms.CheckBox()
            Me.chkRecalculateBalance = New System.Windows.Forms.CheckBox()
            Me.btnBackToTrial = New System.Windows.Forms.Button()
            Me.btnClearSearch = New System.Windows.Forms.Button()
            Me.pnlSerch = New System.Windows.Forms.Panel()
            Me.txtSrcRefNo = New System.Windows.Forms.TextBox()
            Me.txtSrcLineNo = New System.Windows.Forms.TextBox()
            Me.txtSrcDate = New System.Windows.Forms.TextBox()
            Me.txtSrcSharh = New System.Windows.Forms.TextBox()
            Me.txtSrcAccountCode = New System.Windows.Forms.TextBox()
            Me.txtSrcAccountName = New System.Windows.Forms.TextBox()
            Me.txtSrcDebit = New System.Windows.Forms.TextBox()
            Me.txtSrcCredit = New System.Windows.Forms.TextBox()
            Me.txtSrcTash = New System.Windows.Forms.TextBox()
            Me.txtSrcBalance = New System.Windows.Forms.TextBox()
            Me.dgvLedger = New System.Windows.Forms.DataGridView()
            Me.colGoToDoc = New System.Windows.Forms.DataGridViewButtonColumn()
            Me.colRefNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colLineNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSharh = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTash = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.pnlJamDaftar = New System.Windows.Forms.Panel()
            Me.lblJamTitle = New System.Windows.Forms.Label()
            Me.lblSumDebit = New System.Windows.Forms.Label()
            Me.lblSumCredit = New System.Windows.Forms.Label()
            Me.lblTash = New System.Windows.Forms.Label()
            Me.lblSumBalance = New System.Windows.Forms.Label()
            Me.pnlFilters = New System.Windows.Forms.Panel()
            Me.txtFromDoc = New System.Windows.Forms.TextBox()
            Me.txtToDoc = New System.Windows.Forms.TextBox()
            Me.chkFilterByDate = New System.Windows.Forms.CheckBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.MaskedTextBox()
            Me.btnFromDate = New System.Windows.Forms.Button()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtToDate = New System.Windows.Forms.MaskedTextBox()
            Me.btnToDate = New System.Windows.Forms.Button()
            Me.chkFilterByDoc = New System.Windows.Forms.CheckBox()
            Me.lblFromDoc = New System.Windows.Forms.Label()
            Me.lblToDoc = New System.Windows.Forms.Label()
            Me.chkFilterByStatus = New System.Windows.Forms.CheckBox()
            Me.cmbStatus = New System.Windows.Forms.ComboBox()
            Me.pnlHeader.SuspendLayout()
            Me.pnlTanzim.SuspendLayout()
            Me.pnlSerch.SuspendLayout()
            CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlJamDaftar.SuspendLayout()
            Me.pnlFilters.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblAccountTitle)
            Me.pnlHeader.Controls.Add(Me.btnExportExcel)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(1320, 44)
            Me.pnlHeader.TabIndex = 1
            '
            'lblAccountTitle
            '
            Me.lblAccountTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblAccountTitle.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblAccountTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(120, Byte), Integer))
            Me.lblAccountTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblAccountTitle.Name = "lblAccountTitle"
            Me.lblAccountTitle.Size = New System.Drawing.Size(1320, 44)
            Me.lblAccountTitle.TabIndex = 4
            Me.lblAccountTitle.Text = "دفتر تفصیلی شناور"
            Me.lblAccountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnExportExcel
            '
            Me.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(174, Byte), Integer), CType(CType(96, Byte), Integer))
            Me.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnExportExcel.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnExportExcel.ForeColor = System.Drawing.Color.White
            Me.btnExportExcel.Location = New System.Drawing.Point(1180, 8)
            Me.btnExportExcel.Name = "btnExportExcel"
            Me.btnExportExcel.Size = New System.Drawing.Size(130, 28)
            Me.btnExportExcel.TabIndex = 5
            Me.btnExportExcel.Text = "خروجی اکسل"
            Me.btnExportExcel.UseVisualStyleBackColor = False
            Me.btnExportExcel.BringToFront()
            '
            'pnlTanzim
            '
            Me.pnlTanzim.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlTanzim.Controls.Add(Me.cmbDescType)
            Me.pnlTanzim.Controls.Add(Me.btnSelectAccountsPopup)
            Me.pnlTanzim.Controls.Add(Me.cmbSelectedAccounts)
            Me.pnlTanzim.Controls.Add(Me.btnRefresh)
            Me.pnlTanzim.Controls.Add(Me.btnPrintDaftar)
            Me.pnlTanzim.Controls.Add(Me.lblDescType)
            Me.pnlTanzim.Controls.Add(Me.chkAggregate)
            Me.pnlTanzim.Controls.Add(Me.chkRecalculateBalance)
            Me.pnlTanzim.Controls.Add(Me.btnBackToTrial)
            Me.pnlTanzim.Controls.Add(Me.btnClearSearch)
            Me.pnlTanzim.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTanzim.Location = New System.Drawing.Point(0, 44)
            Me.pnlTanzim.Name = "pnlTanzim"
            Me.pnlTanzim.Size = New System.Drawing.Size(1320, 44)
            Me.pnlTanzim.TabIndex = 8
            '
            'cmbDescType
            '
            Me.cmbDescType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbDescType.FormattingEnabled = True
            Me.cmbDescType.Items.AddRange(New Object() {"فقط شرح ردیف", "فقط شرح سند", "شرح ردیف و شرح سند"})
            Me.cmbDescType.Location = New System.Drawing.Point(718, 9)
            Me.cmbDescType.Name = "cmbDescType"
            Me.cmbDescType.Size = New System.Drawing.Size(150, 22)
            Me.cmbDescType.TabIndex = 7
            '
            'btnSelectAccountsPopup
            '
            Me.btnSelectAccountsPopup.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.btnSelectAccountsPopup.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSelectAccountsPopup.Location = New System.Drawing.Point(502, 7)
            Me.btnSelectAccountsPopup.Name = "btnSelectAccountsPopup"
            Me.btnSelectAccountsPopup.Size = New System.Drawing.Size(110, 28)
            Me.btnSelectAccountsPopup.TabIndex = 10
            Me.btnSelectAccountsPopup.Text = "انتخاب شناور"
            Me.btnSelectAccountsPopup.UseVisualStyleBackColor = False
            '
            'cmbSelectedAccounts
            '
            Me.cmbSelectedAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSelectedAccounts.DropDownWidth = 350
            Me.cmbSelectedAccounts.FormattingEnabled = True
            Me.cmbSelectedAccounts.Location = New System.Drawing.Point(618, 9)
            Me.cmbSelectedAccounts.Name = "cmbSelectedAccounts"
            Me.cmbSelectedAccounts.Size = New System.Drawing.Size(90, 22)
            Me.cmbSelectedAccounts.TabIndex = 11
            '
            'btnRefresh
            '
            Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.btnRefresh.Location = New System.Drawing.Point(132, 7)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New System.Drawing.Size(100, 28)
            Me.btnRefresh.TabIndex = 5
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = False
            '
            'btnPrintDaftar
            '
            Me.btnPrintDaftar.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.btnPrintDaftar.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnPrintDaftar.Location = New System.Drawing.Point(382, 7)
            Me.btnPrintDaftar.Name = "btnPrintDaftar"
            Me.btnPrintDaftar.Size = New System.Drawing.Size(110, 28)
            Me.btnPrintDaftar.TabIndex = 6
            Me.btnPrintDaftar.Text = "چاپ دفتر"
            Me.btnPrintDaftar.UseVisualStyleBackColor = False
            '
            'lblDescType
            '
            Me.lblDescType.Location = New System.Drawing.Point(874, 13)
            Me.lblDescType.Name = "lblDescType"
            Me.lblDescType.Size = New System.Drawing.Size(90, 18)
            Me.lblDescType.TabIndex = 6
            Me.lblDescType.Text = "نوع شرح دفتر :"
            '
            'chkAggregate
            '
            Me.chkAggregate.Location = New System.Drawing.Point(974, 19)
            Me.chkAggregate.Name = "chkAggregate"
            Me.chkAggregate.Size = New System.Drawing.Size(300, 22)
            Me.chkAggregate.TabIndex = 2
            Me.chkAggregate.Text = "تجمیع سطرهای هم سطح با کد یکسان در یک سند"
            Me.chkAggregate.UseVisualStyleBackColor = True
            '
            'chkRecalculateBalance
            '
            Me.chkRecalculateBalance.Checked = True
            Me.chkRecalculateBalance.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkRecalculateBalance.Location = New System.Drawing.Point(994, 2)
            Me.chkRecalculateBalance.Name = "chkRecalculateBalance"
            Me.chkRecalculateBalance.Size = New System.Drawing.Size(280, 22)
            Me.chkRecalculateBalance.TabIndex = 9
            Me.chkRecalculateBalance.Text = "محاسبه مجدد مانده حساب بعد از جستجو"
            Me.chkRecalculateBalance.UseVisualStyleBackColor = True
            '
            'btnBackToTrial
            '
            Me.btnBackToTrial.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.btnBackToTrial.Location = New System.Drawing.Point(6, 7)
            Me.btnBackToTrial.Name = "btnBackToTrial"
            Me.btnBackToTrial.Size = New System.Drawing.Size(120, 28)
            Me.btnBackToTrial.TabIndex = 3
            Me.btnBackToTrial.Text = "بازگشت به تراز"
            Me.btnBackToTrial.UseVisualStyleBackColor = False
            '
            'btnClearSearch
            '
            Me.btnClearSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.btnClearSearch.Location = New System.Drawing.Point(238, 7)
            Me.btnClearSearch.Name = "btnClearSearch"
            Me.btnClearSearch.Size = New System.Drawing.Size(130, 28)
            Me.btnClearSearch.TabIndex = 8
            Me.btnClearSearch.Text = "پاک کردن جستجوها"
            Me.btnClearSearch.UseVisualStyleBackColor = False
            '
            'pnlSerch
            '
            Me.pnlSerch.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.pnlSerch.Controls.Add(Me.txtSrcRefNo)
            Me.pnlSerch.Controls.Add(Me.txtSrcLineNo)
            Me.pnlSerch.Controls.Add(Me.txtSrcDate)
            Me.pnlSerch.Controls.Add(Me.txtSrcSharh)
            Me.pnlSerch.Controls.Add(Me.txtSrcAccountCode)
            Me.pnlSerch.Controls.Add(Me.txtSrcAccountName)
            Me.pnlSerch.Controls.Add(Me.txtSrcDebit)
            Me.pnlSerch.Controls.Add(Me.txtSrcCredit)
            Me.pnlSerch.Controls.Add(Me.txtSrcTash)
            Me.pnlSerch.Controls.Add(Me.txtSrcBalance)
            Me.pnlSerch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSerch.Location = New System.Drawing.Point(0, 128)
            Me.pnlSerch.Name = "pnlSerch"
            Me.pnlSerch.Size = New System.Drawing.Size(1320, 30)
            Me.pnlSerch.TabIndex = 9
            '
            'txtSrcRefNo
            '
            Me.txtSrcRefNo.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcRefNo.Name = "txtSrcRefNo"
            Me.txtSrcRefNo.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcRefNo.TabIndex = 0
            '
            'txtSrcLineNo
            '
            Me.txtSrcLineNo.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcLineNo.Name = "txtSrcLineNo"
            Me.txtSrcLineNo.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcLineNo.TabIndex = 1
            '
            'txtSrcDate
            '
            Me.txtSrcDate.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcDate.Name = "txtSrcDate"
            Me.txtSrcDate.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcDate.TabIndex = 2
            '
            'txtSrcSharh
            '
            Me.txtSrcSharh.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcSharh.Name = "txtSrcSharh"
            Me.txtSrcSharh.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcSharh.TabIndex = 3
            '
            'txtSrcAccountCode
            '
            Me.txtSrcAccountCode.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcAccountCode.Name = "txtSrcAccountCode"
            Me.txtSrcAccountCode.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcAccountCode.TabIndex = 4
            '
            'txtSrcAccountName
            '
            Me.txtSrcAccountName.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcAccountName.Name = "txtSrcAccountName"
            Me.txtSrcAccountName.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcAccountName.TabIndex = 5
            '
            'txtSrcDebit
            '
            Me.txtSrcDebit.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcDebit.Name = "txtSrcDebit"
            Me.txtSrcDebit.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcDebit.TabIndex = 6
            '
            'txtSrcCredit
            '
            Me.txtSrcCredit.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcCredit.Name = "txtSrcCredit"
            Me.txtSrcCredit.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcCredit.TabIndex = 7
            '
            'txtSrcTash
            '
            Me.txtSrcTash.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcTash.Name = "txtSrcTash"
            Me.txtSrcTash.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcTash.TabIndex = 8
            '
            'txtSrcBalance
            '
            Me.txtSrcBalance.Location = New System.Drawing.Point(0, 4)
            Me.txtSrcBalance.Name = "txtSrcBalance"
            Me.txtSrcBalance.Size = New System.Drawing.Size(80, 22)
            Me.txtSrcBalance.TabIndex = 9
            '
            'dgvLedger
            '
            Me.dgvLedger.AllowUserToAddRows = False
            Me.dgvLedger.AllowUserToDeleteRows = False
            Me.dgvLedger.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
            Me.dgvLedger.BackgroundColor = System.Drawing.Color.White
            Me.dgvLedger.BorderStyle = System.Windows.Forms.BorderStyle.None
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(160, Byte), Integer))
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!)
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvLedger.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.dgvLedger.ColumnHeadersHeight = 30
            Me.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvLedger.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colGoToDoc, Me.colRefNo, Me.colLineNo, Me.colDate, Me.colSharh, Me.colAccountCode, Me.colAccountName, Me.colDebit, Me.colCredit, Me.colTash, Me.colBalance})
            Me.dgvLedger.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvLedger.EnableHeadersVisualStyles = False
            Me.dgvLedger.GridColor = System.Drawing.Color.LightSteelBlue
            Me.dgvLedger.Location = New System.Drawing.Point(0, 158)
            Me.dgvLedger.MultiSelect = False
            Me.dgvLedger.Name = "dgvLedger"
            Me.dgvLedger.ReadOnly = True
            Me.dgvLedger.RowHeadersVisible = False
            Me.dgvLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvLedger.Size = New System.Drawing.Size(1320, 556)
            Me.dgvLedger.TabIndex = 0
            '
            'colGoToDoc
            '
            Me.colGoToDoc.HeaderText = "رفتن به سند"
            Me.colGoToDoc.Name = "colGoToDoc"
            Me.colGoToDoc.ReadOnly = True
            Me.colGoToDoc.Text = "رفتن به سند"
            Me.colGoToDoc.UseColumnTextForButtonValue = False
            Me.colGoToDoc.Width = 95
            '
            'colRefNo
            '
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colRefNo.DefaultCellStyle = DataGridViewCellStyle2
            Me.colRefNo.HeaderText = "شماره سند"
            Me.colRefNo.Name = "colRefNo"
            Me.colRefNo.ReadOnly = True
            Me.colRefNo.Width = 85
            '
            'colLineNo
            '
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colLineNo.DefaultCellStyle = DataGridViewCellStyle3
            Me.colLineNo.HeaderText = "ردیف"
            Me.colLineNo.Name = "colLineNo"
            Me.colLineNo.ReadOnly = True
            Me.colLineNo.Width = 50
            '
            'colDate
            '
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colDate.DefaultCellStyle = DataGridViewCellStyle4
            Me.colDate.HeaderText = "تاریخ"
            Me.colDate.Name = "colDate"
            Me.colDate.ReadOnly = True
            Me.colDate.Width = 85
            '
            'colSharh
            '
            Me.colSharh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.colSharh.DefaultCellStyle = DataGridViewCellStyle5
            Me.colSharh.HeaderText = "شرح تراکنش"
            Me.colSharh.Name = "colSharh"
            Me.colSharh.ReadOnly = True
            '
            'colAccountCode
            '
            DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colAccountCode.DefaultCellStyle = DataGridViewCellStyle6
            Me.colAccountCode.HeaderText = "کد حساب"
            Me.colAccountCode.Name = "colAccountCode"
            Me.colAccountCode.ReadOnly = True
            Me.colAccountCode.Width = 100
            '
            'colAccountName
            '
            DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            Me.colAccountName.DefaultCellStyle = DataGridViewCellStyle7
            Me.colAccountName.HeaderText = "حسابداری متقابل"
            Me.colAccountName.Name = "colAccountName"
            Me.colAccountName.ReadOnly = True
            Me.colAccountName.Width = 180
            '
            'colDebit
            '
            DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            Me.colDebit.DefaultCellStyle = DataGridViewCellStyle8
            Me.colDebit.HeaderText = "بدهکار (واریز)"
            Me.colDebit.Name = "colDebit"
            Me.colDebit.ReadOnly = True
            Me.colDebit.Width = 115
            '
            'colCredit
            '
            DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            Me.colCredit.DefaultCellStyle = DataGridViewCellStyle9
            Me.colCredit.HeaderText = "بستانکار (برداشت)"
            Me.colCredit.Name = "colCredit"
            Me.colCredit.ReadOnly = True
            Me.colCredit.Width = 115
            '
            'colTash
            '
            DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colTash.DefaultCellStyle = DataGridViewCellStyle10
            Me.colTash.HeaderText = "تش"
            Me.colTash.Name = "colTash"
            Me.colTash.ReadOnly = True
            Me.colTash.Width = 70
            '
            'colBalance
            '
            Me.colBalance.DefaultCellStyle = DataGridViewCellStyle8
            Me.colBalance.HeaderText = "مانده"
            Me.colBalance.Name = "colBalance"
            Me.colBalance.ReadOnly = True
            Me.colBalance.Width = 125
            '
            'pnlJamDaftar
            '
            Me.pnlJamDaftar.BackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(228, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlJamDaftar.Controls.Add(Me.lblJamTitle)
            Me.pnlJamDaftar.Controls.Add(Me.lblSumDebit)
            Me.pnlJamDaftar.Controls.Add(Me.lblSumCredit)
            Me.pnlJamDaftar.Controls.Add(Me.lblTash)
            Me.pnlJamDaftar.Controls.Add(Me.lblSumBalance)
            Me.pnlJamDaftar.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlJamDaftar.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.pnlJamDaftar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(100, Byte), Integer))
            Me.pnlJamDaftar.Location = New System.Drawing.Point(0, 714)
            Me.pnlJamDaftar.Name = "pnlJamDaftar"
            Me.pnlJamDaftar.Size = New System.Drawing.Size(1320, 35)
            Me.pnlJamDaftar.TabIndex = 3
            '
            'lblJamTitle
            '
            Me.lblJamTitle.Location = New System.Drawing.Point(0, 8)
            Me.lblJamTitle.Name = "lblJamTitle"
            Me.lblJamTitle.Size = New System.Drawing.Size(100, 18)
            Me.lblJamTitle.TabIndex = 0
            Me.lblJamTitle.Text = "جمع کل :"
            Me.lblJamTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSumDebit
            '
            Me.lblSumDebit.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebit.Name = "lblSumDebit"
            Me.lblSumDebit.Size = New System.Drawing.Size(115, 18)
            Me.lblSumDebit.TabIndex = 1
            Me.lblSumDebit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCredit
            '
            Me.lblSumCredit.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCredit.Name = "lblSumCredit"
            Me.lblSumCredit.Size = New System.Drawing.Size(115, 18)
            Me.lblSumCredit.TabIndex = 2
            Me.lblSumCredit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblTash
            '
            Me.lblTash.Location = New System.Drawing.Point(0, 8)
            Me.lblTash.Name = "lblTash"
            Me.lblTash.Size = New System.Drawing.Size(70, 18)
            Me.lblTash.TabIndex = 3
            Me.lblTash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblSumBalance
            '
            Me.lblSumBalance.Location = New System.Drawing.Point(0, 8)
            Me.lblSumBalance.Name = "lblSumBalance"
            Me.lblSumBalance.Size = New System.Drawing.Size(125, 18)
            Me.lblSumBalance.TabIndex = 4
            Me.lblSumBalance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlFilters
            '
            Me.pnlFilters.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.pnlFilters.Controls.Add(Me.txtFromDoc)
            Me.pnlFilters.Controls.Add(Me.txtToDoc)
            Me.pnlFilters.Controls.Add(Me.chkFilterByDate)
            Me.pnlFilters.Controls.Add(Me.lblFromDate)
            Me.pnlFilters.Controls.Add(Me.txtFromDate)
            Me.pnlFilters.Controls.Add(Me.btnFromDate)
            Me.pnlFilters.Controls.Add(Me.lblToDate)
            Me.pnlFilters.Controls.Add(Me.txtToDate)
            Me.pnlFilters.Controls.Add(Me.btnToDate)
            Me.pnlFilters.Controls.Add(Me.chkFilterByDoc)
            Me.pnlFilters.Controls.Add(Me.lblFromDoc)
            Me.pnlFilters.Controls.Add(Me.lblToDoc)
            Me.pnlFilters.Controls.Add(Me.chkFilterByStatus)
            Me.pnlFilters.Controls.Add(Me.cmbStatus)
            Me.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlFilters.Location = New System.Drawing.Point(0, 88)
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.Size = New System.Drawing.Size(1320, 40)
            Me.pnlFilters.TabIndex = 10
            '
            'txtFromDoc
            '
            Me.txtFromDoc.Enabled = False
            Me.txtFromDoc.Location = New System.Drawing.Point(766, 9)
            Me.txtFromDoc.Name = "txtFromDoc"
            Me.txtFromDoc.Size = New System.Drawing.Size(70, 22)
            Me.txtFromDoc.TabIndex = 9
            '
            'txtToDoc
            '
            Me.txtToDoc.Enabled = False
            Me.txtToDoc.Location = New System.Drawing.Point(668, 9)
            Me.txtToDoc.Name = "txtToDoc"
            Me.txtToDoc.Size = New System.Drawing.Size(70, 22)
            Me.txtToDoc.TabIndex = 11
            '
            'chkFilterByDate
            '
            Me.chkFilterByDate.Location = New System.Drawing.Point(480, 9)
            Me.chkFilterByDate.Name = "chkFilterByDate"
            Me.chkFilterByDate.Size = New System.Drawing.Size(125, 21)
            Me.chkFilterByDate.TabIndex = 0
            Me.chkFilterByDate.Text = "فیلتر بر اساس تاریخ"
            '
            'lblFromDate
            '
            Me.lblFromDate.Location = New System.Drawing.Point(421, 12)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(48, 14)
            Me.lblFromDate.TabIndex = 1
            Me.lblFromDate.Text = "از تاریخ:"
            '
            'txtFromDate
            '
            Me.txtFromDate.Enabled = False
            Me.txtFromDate.Location = New System.Drawing.Point(303, 9)
            Me.txtFromDate.Mask = "0000/00/00"
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(80, 22)
            Me.txtFromDate.TabIndex = 2
            '
            'btnFromDate
            '
            Me.btnFromDate.Enabled = False
            Me.btnFromDate.Location = New System.Drawing.Point(388, 9)
            Me.btnFromDate.Name = "btnFromDate"
            Me.btnFromDate.Size = New System.Drawing.Size(28, 22)
            Me.btnFromDate.TabIndex = 3
            Me.btnFromDate.Text = "..."
            '
            'lblToDate
            '
            Me.lblToDate.Location = New System.Drawing.Point(250, 12)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(48, 14)
            Me.lblToDate.TabIndex = 4
            Me.lblToDate.Text = "تا تاریخ:"
            '
            'txtToDate
            '
            Me.txtToDate.Enabled = False
            Me.txtToDate.Location = New System.Drawing.Point(133, 9)
            Me.txtToDate.Mask = "0000/00/00"
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(80, 22)
            Me.txtToDate.TabIndex = 5
            '
            'btnToDate
            '
            Me.btnToDate.Enabled = False
            Me.btnToDate.Location = New System.Drawing.Point(218, 9)
            Me.btnToDate.Name = "btnToDate"
            Me.btnToDate.Size = New System.Drawing.Size(28, 22)
            Me.btnToDate.TabIndex = 6
            Me.btnToDate.Text = "..."
            '
            'chkFilterByDoc
            '
            Me.chkFilterByDoc.Location = New System.Drawing.Point(861, 9)
            Me.chkFilterByDoc.Name = "chkFilterByDoc"
            Me.chkFilterByDoc.Size = New System.Drawing.Size(140, 21)
            Me.chkFilterByDoc.TabIndex = 7
            Me.chkFilterByDoc.Text = "فیلتر بر اساس شماره سند"
            '
            'lblFromDoc
            '
            Me.lblFromDoc.Location = New System.Drawing.Point(808, 12)
            Me.lblFromDoc.Name = "lblFromDoc"
            Me.lblFromDoc.Size = New System.Drawing.Size(50, 14)
            Me.lblFromDoc.TabIndex = 8
            Me.lblFromDoc.Text = "از شماره:"
            '
            'lblToDoc
            '
            Me.lblToDoc.Location = New System.Drawing.Point(709, 12)
            Me.lblToDoc.Name = "lblToDoc"
            Me.lblToDoc.Size = New System.Drawing.Size(50, 14)
            Me.lblToDoc.TabIndex = 10
            Me.lblToDoc.Text = "تا شماره:"
            '
            'chkFilterByStatus
            '
            Me.chkFilterByStatus.Location = New System.Drawing.Point(1193, 9)
            Me.chkFilterByStatus.Name = "chkFilterByStatus"
            Me.chkFilterByStatus.Size = New System.Drawing.Size(115, 21)
            Me.chkFilterByStatus.TabIndex = 12
            Me.chkFilterByStatus.Text = "فیلتر وضعیت سند"
            '
            'cmbStatus
            '
            Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbStatus.Enabled = False
            Me.cmbStatus.Items.AddRange(New Object() {"موقت", "دائم"})
            Me.cmbStatus.Location = New System.Drawing.Point(1086, 9)
            Me.cmbStatus.Name = "cmbStatus"
            Me.cmbStatus.Size = New System.Drawing.Size(100, 22)
            Me.cmbStatus.TabIndex = 13
            '
            'HesabdaryDaftarShenavarForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1320, 749)
            Me.Controls.Add(Me.dgvLedger)
            Me.Controls.Add(Me.pnlJamDaftar)
            Me.Controls.Add(Me.pnlSerch)
            Me.Controls.Add(Me.pnlFilters)
            Me.Controls.Add(Me.pnlTanzim)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryDaftarShenavarForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "دفتر شناور"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlTanzim.ResumeLayout(False)
            Me.pnlSerch.ResumeLayout(False)
            Me.pnlSerch.PerformLayout()
            CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlJamDaftar.ResumeLayout(False)
            Me.pnlFilters.ResumeLayout(False)
            Me.pnlFilters.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
