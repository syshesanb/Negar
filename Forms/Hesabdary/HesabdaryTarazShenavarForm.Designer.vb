Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class HesabdaryTarazShenavarForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents chkOnlyWithData As CheckBox
        Friend WithEvents lblExpand As Label
        Friend WithEvents cmbExpandToLevel As ComboBox
        Friend WithEvents btnRefresh As Button
        Friend WithEvents btnPrintTaraz As Button
        Friend WithEvents lblTrialType As Label
        Friend WithEvents cmbTrialType As ComboBox
        Friend WithEvents dgvTrial As DataGridView
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
        Friend WithEvents pnlJam As Panel
        Friend WithEvents lblJamTitle As Label
        Friend WithEvents lblSumDebitBefore As Label
        Friend WithEvents lblSumCreditBefore As Label
        Friend WithEvents lblSumDebitBegin As Label
        Friend WithEvents lblSumCreditBegin As Label
        Friend WithEvents lblSumDebitDuring As Label
        Friend WithEvents lblSumCreditDuring As Label
        Friend WithEvents lblSumDebitTotal As Label
        Friend WithEvents lblSumCreditTotal As Label
        Friend WithEvents lblSumDebitEnd As Label
        Friend WithEvents lblSumCreditEnd As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.btnRefresh = New System.Windows.Forms.Button()
            Me.btnPrintTaraz = New System.Windows.Forms.Button()
            Me.btnExportExcel = New System.Windows.Forms.Button()
            Me.lblExpand = New System.Windows.Forms.Label()
            Me.cmbExpandToLevel = New System.Windows.Forms.ComboBox()
            Me.chkOnlyWithData = New System.Windows.Forms.CheckBox()
            Me.cmbTrialType = New System.Windows.Forms.ComboBox()
            Me.lblTrialType = New System.Windows.Forms.Label()
            Me.dgvTrial = New System.Windows.Forms.DataGridView()
            Me.colToggle = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colLedger = New System.Windows.Forms.DataGridViewButtonColumn()
            Me.colCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebitBefore = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCreditBefore = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebitBegin = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCreditBegin = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebitDuring = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCreditDuring = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebitTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCreditTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebitEnd = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCreditEnd = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
            Me.pnlJam = New System.Windows.Forms.Panel()
            Me.lblJamTitle = New System.Windows.Forms.Label()
            Me.lblSumDebitBefore = New System.Windows.Forms.Label()
            Me.lblSumCreditBefore = New System.Windows.Forms.Label()
            Me.lblSumDebitBegin = New System.Windows.Forms.Label()
            Me.lblSumCreditBegin = New System.Windows.Forms.Label()
            Me.lblSumDebitDuring = New System.Windows.Forms.Label()
            Me.lblSumCreditDuring = New System.Windows.Forms.Label()
            Me.lblSumDebitTotal = New System.Windows.Forms.Label()
            Me.lblSumCreditTotal = New System.Windows.Forms.Label()
            Me.lblSumDebitEnd = New System.Windows.Forms.Label()
            Me.lblSumCreditEnd = New System.Windows.Forms.Label()
            Me.pnlGridSearch = New System.Windows.Forms.Panel()
            Me.txtSearchToggle = New System.Windows.Forms.TextBox()
            Me.txtSearchLedger = New System.Windows.Forms.TextBox()
            Me.txtSearchCode = New System.Windows.Forms.TextBox()
            Me.txtSearchName = New System.Windows.Forms.TextBox()
            Me.txtSearchDebitBefore = New System.Windows.Forms.TextBox()
            Me.txtSearchCreditBefore = New System.Windows.Forms.TextBox()
            Me.txtSearchDebitBegin = New System.Windows.Forms.TextBox()
            Me.txtSearchCreditBegin = New System.Windows.Forms.TextBox()
            Me.txtSearchDebitDuring = New System.Windows.Forms.TextBox()
            Me.txtSearchCreditDuring = New System.Windows.Forms.TextBox()
            Me.txtSearchDebitTotal = New System.Windows.Forms.TextBox()
            Me.txtSearchCreditTotal = New System.Windows.Forms.TextBox()
            Me.txtSearchDebitEnd = New System.Windows.Forms.TextBox()
            Me.txtSearchCreditEnd = New System.Windows.Forms.TextBox()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvTrial, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlFilters.SuspendLayout()
            Me.pnlJam.SuspendLayout()
            Me.pnlGridSearch.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Controls.Add(Me.btnPrintTaraz)
            Me.pnlTop.Controls.Add(Me.btnExportExcel)
            Me.pnlTop.Controls.Add(Me.lblExpand)
            Me.pnlTop.Controls.Add(Me.cmbExpandToLevel)
            Me.pnlTop.Controls.Add(Me.chkOnlyWithData)
            Me.pnlTop.Controls.Add(Me.cmbTrialType)
            Me.pnlTop.Controls.Add(Me.lblTrialType)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(1320, 44)
            Me.pnlTop.TabIndex = 2
            '
            'btnRefresh
            '
            Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnRefresh.Location = New System.Drawing.Point(6, 7)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New System.Drawing.Size(100, 28)
            Me.btnRefresh.TabIndex = 0
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = False
            '
            'btnPrintTaraz
            '
            Me.btnPrintTaraz.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.btnPrintTaraz.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnPrintTaraz.Location = New System.Drawing.Point(112, 7)
            Me.btnPrintTaraz.Name = "btnPrintTaraz"
            Me.btnPrintTaraz.Size = New System.Drawing.Size(140, 28)
            Me.btnPrintTaraz.TabIndex = 6
            Me.btnPrintTaraz.Text = "چاپ تراز شناور"
            Me.btnPrintTaraz.UseVisualStyleBackColor = False
            '
            'btnExportExcel
            '
            Me.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(174, Byte), Integer), CType(CType(96, Byte), Integer))
            Me.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnExportExcel.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnExportExcel.ForeColor = System.Drawing.Color.White
            Me.btnExportExcel.Location = New System.Drawing.Point(1120, 8)
            Me.btnExportExcel.Name = "btnExportExcel"
            Me.btnExportExcel.Size = New System.Drawing.Size(130, 28)
            Me.btnExportExcel.TabIndex = 7
            Me.btnExportExcel.Text = "خروجی اکسل"
            Me.btnExportExcel.UseVisualStyleBackColor = False
            '
            'lblExpand
            '
            Me.lblExpand.BackColor = System.Drawing.Color.Transparent
            Me.lblExpand.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblExpand.Location = New System.Drawing.Point(416, 11)
            Me.lblExpand.Name = "lblExpand"
            Me.lblExpand.Size = New System.Drawing.Size(166, 20)
            Me.lblExpand.TabIndex = 1
            Me.lblExpand.Text = "نمایش سرفصلها تا سطح :"
            Me.lblExpand.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cmbExpandToLevel
            '
            Me.cmbExpandToLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbExpandToLevel.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.cmbExpandToLevel.FormattingEnabled = True
            Me.cmbExpandToLevel.Items.AddRange(New Object() {"گروه (بستن همه)", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"})
            Me.cmbExpandToLevel.Location = New System.Drawing.Point(287, 11)
            Me.cmbExpandToLevel.Name = "cmbExpandToLevel"
            Me.cmbExpandToLevel.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.cmbExpandToLevel.Size = New System.Drawing.Size(120, 22)
            Me.cmbExpandToLevel.TabIndex = 2
            '
            'chkOnlyWithData
            '
            Me.chkOnlyWithData.Checked = True
            Me.chkOnlyWithData.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkOnlyWithData.Location = New System.Drawing.Point(926, 11)
            Me.chkOnlyWithData.Name = "chkOnlyWithData"
            Me.chkOnlyWithData.Size = New System.Drawing.Size(177, 22)
            Me.chkOnlyWithData.TabIndex = 3
            Me.chkOnlyWithData.Text = "فقط حسابهای دارای گردش"
            '
            'cmbTrialType
            '
            Me.cmbTrialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbTrialType.FormattingEnabled = True
            Me.cmbTrialType.Items.AddRange(New Object() {"2 ستونی", "4 ستونی", "6 ستونی", "8 ستونی", "10 ستونی"})
            Me.cmbTrialType.Location = New System.Drawing.Point(655, 9)
            Me.cmbTrialType.Name = "cmbTrialType"
            Me.cmbTrialType.Size = New System.Drawing.Size(100, 22)
            Me.cmbTrialType.TabIndex = 5
            '
            'lblTrialType
            '
            Me.lblTrialType.Location = New System.Drawing.Point(760, 13)
            Me.lblTrialType.Name = "lblTrialType"
            Me.lblTrialType.Size = New System.Drawing.Size(150, 18)
            Me.lblTrialType.TabIndex = 4
            Me.lblTrialType.Text = "نوع تراز از نظر تعداد ستون :"
            '
            'dgvTrial
            '
            Me.dgvTrial.AllowUserToAddRows = False
            Me.dgvTrial.AllowUserToDeleteRows = False
            Me.dgvTrial.BackgroundColor = System.Drawing.Color.White
            Me.dgvTrial.BorderStyle = System.Windows.Forms.BorderStyle.None
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(160, Byte), Integer))
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!)
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvTrial.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.dgvTrial.ColumnHeadersHeight = 30
            Me.dgvTrial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvTrial.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colToggle, Me.colLedger, Me.colCode, Me.colName, Me.colDebitBefore, Me.colCreditBefore, Me.colDebitBegin, Me.colCreditBegin, Me.colDebitDuring, Me.colCreditDuring, Me.colDebitTotal, Me.colCreditTotal, Me.colDebitEnd, Me.colCreditEnd})
            Me.dgvTrial.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvTrial.EnableHeadersVisualStyles = False
            Me.dgvTrial.GridColor = System.Drawing.Color.LightSteelBlue
            Me.dgvTrial.Location = New System.Drawing.Point(0, 112)
            Me.dgvTrial.MultiSelect = False
            Me.dgvTrial.Name = "dgvTrial"
            Me.dgvTrial.ReadOnly = True
            Me.dgvTrial.RowHeadersVisible = False
            Me.dgvTrial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvTrial.Size = New System.Drawing.Size(1320, 602)
            Me.dgvTrial.TabIndex = 0
            '
            'colToggle
            '
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.DarkBlue
            Me.colToggle.DefaultCellStyle = DataGridViewCellStyle2
            Me.colToggle.HeaderText = ""
            Me.colToggle.Name = "colToggle"
            Me.colToggle.ReadOnly = True
            Me.colToggle.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.colToggle.Width = 30
            '
            'colLedger
            '
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.colLedger.DefaultCellStyle = DataGridViewCellStyle3
            Me.colLedger.HeaderText = "دفتر شناور"
            Me.colLedger.Name = "colLedger"
            Me.colLedger.ReadOnly = True
            Me.colLedger.Text = "دفتر"
            Me.colLedger.UseColumnTextForButtonValue = True
            Me.colLedger.Width = 90
            '
            'colCode
            '
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colCode.DefaultCellStyle = DataGridViewCellStyle4
            Me.colCode.HeaderText = "کد شناور"
            Me.colCode.Name = "colCode"
            Me.colCode.ReadOnly = True
            Me.colCode.Width = 110
            '
            'colName
            '
            Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            Me.colName.DefaultCellStyle = DataGridViewCellStyle5
            Me.colName.HeaderText = "نام حساب تفصیلی شناور"
            Me.colName.Name = "colName"
            Me.colName.ReadOnly = True
            '
            'colDebitBefore
            '
            DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            Me.colDebitBefore.DefaultCellStyle = DataGridViewCellStyle6
            Me.colDebitBefore.HeaderText = "بدهکار قبل دوره"
            Me.colDebitBefore.Name = "colDebitBefore"
            Me.colDebitBefore.ReadOnly = True
            Me.colDebitBefore.Width = 110
            '
            'colCreditBefore
            '
            Me.colCreditBefore.DefaultCellStyle = DataGridViewCellStyle6
            Me.colCreditBefore.HeaderText = "بستانکار قبل دوره"
            Me.colCreditBefore.Name = "colCreditBefore"
            Me.colCreditBefore.ReadOnly = True
            Me.colCreditBefore.Width = 110
            '
            'colDebitBegin
            '
            Me.colDebitBegin.DefaultCellStyle = DataGridViewCellStyle6
            Me.colDebitBegin.HeaderText = "بدهکار اول دوره"
            Me.colDebitBegin.Name = "colDebitBegin"
            Me.colDebitBegin.ReadOnly = True
            Me.colDebitBegin.Width = 110
            '
            'colCreditBegin
            '
            Me.colCreditBegin.DefaultCellStyle = DataGridViewCellStyle6
            Me.colCreditBegin.HeaderText = "بستانکار اول دوره"
            Me.colCreditBegin.Name = "colCreditBegin"
            Me.colCreditBegin.ReadOnly = True
            Me.colCreditBegin.Width = 110
            '
            'colDebitDuring
            '
            Me.colDebitDuring.DefaultCellStyle = DataGridViewCellStyle6
            Me.colDebitDuring.HeaderText = "گردش بدهکار طی"
            Me.colDebitDuring.Name = "colDebitDuring"
            Me.colDebitDuring.ReadOnly = True
            Me.colDebitDuring.Width = 110
            '
            'colCreditDuring
            '
            Me.colCreditDuring.DefaultCellStyle = DataGridViewCellStyle6
            Me.colCreditDuring.HeaderText = "گردش بستانکار طی"
            Me.colCreditDuring.Name = "colCreditDuring"
            Me.colCreditDuring.ReadOnly = True
            Me.colCreditDuring.Width = 110
            '
            'colDebitTotal
            '
            Me.colDebitTotal.DefaultCellStyle = DataGridViewCellStyle6
            Me.colDebitTotal.HeaderText = "جمع گردش بدهکار"
            Me.colDebitTotal.Name = "colDebitTotal"
            Me.colDebitTotal.ReadOnly = True
            Me.colDebitTotal.Width = 110
            '
            'colCreditTotal
            '
            Me.colCreditTotal.DefaultCellStyle = DataGridViewCellStyle6
            Me.colCreditTotal.HeaderText = "جمع گردش بستانکار"
            Me.colCreditTotal.Name = "colCreditTotal"
            Me.colCreditTotal.ReadOnly = True
            Me.colCreditTotal.Width = 110
            '
            'colDebitEnd
            '
            Me.colDebitEnd.DefaultCellStyle = DataGridViewCellStyle6
            Me.colDebitEnd.HeaderText = "مانده بدهکار"
            Me.colDebitEnd.Name = "colDebitEnd"
            Me.colDebitEnd.ReadOnly = True
            Me.colDebitEnd.Width = 110
            '
            'colCreditEnd
            '
            Me.colCreditEnd.DefaultCellStyle = DataGridViewCellStyle6
            Me.colCreditEnd.HeaderText = "مانده بستانکار"
            Me.colCreditEnd.Name = "colCreditEnd"
            Me.colCreditEnd.ReadOnly = True
            Me.colCreditEnd.Width = 110
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
            Me.pnlFilters.Location = New System.Drawing.Point(0, 44)
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.Size = New System.Drawing.Size(1320, 40)
            Me.pnlFilters.TabIndex = 1
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
            'pnlJam
            '
            Me.pnlJam.BackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(228, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlJam.Controls.Add(Me.lblJamTitle)
            Me.pnlJam.Controls.Add(Me.lblSumDebitBefore)
            Me.pnlJam.Controls.Add(Me.lblSumCreditBefore)
            Me.pnlJam.Controls.Add(Me.lblSumDebitBegin)
            Me.pnlJam.Controls.Add(Me.lblSumCreditBegin)
            Me.pnlJam.Controls.Add(Me.lblSumDebitDuring)
            Me.pnlJam.Controls.Add(Me.lblSumCreditDuring)
            Me.pnlJam.Controls.Add(Me.lblSumDebitTotal)
            Me.pnlJam.Controls.Add(Me.lblSumCreditTotal)
            Me.pnlJam.Controls.Add(Me.lblSumDebitEnd)
            Me.pnlJam.Controls.Add(Me.lblSumCreditEnd)
            Me.pnlJam.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlJam.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.pnlJam.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(100, Byte), Integer))
            Me.pnlJam.Location = New System.Drawing.Point(0, 714)
            Me.pnlJam.Name = "pnlJam"
            Me.pnlJam.Size = New System.Drawing.Size(1320, 35)
            Me.pnlJam.TabIndex = 3
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
            'lblSumDebitBefore
            '
            Me.lblSumDebitBefore.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebitBefore.Name = "lblSumDebitBefore"
            Me.lblSumDebitBefore.Size = New System.Drawing.Size(110, 18)
            Me.lblSumDebitBefore.TabIndex = 1
            Me.lblSumDebitBefore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCreditBefore
            '
            Me.lblSumCreditBefore.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCreditBefore.Name = "lblSumCreditBefore"
            Me.lblSumCreditBefore.Size = New System.Drawing.Size(110, 18)
            Me.lblSumCreditBefore.TabIndex = 2
            Me.lblSumCreditBefore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumDebitBegin
            '
            Me.lblSumDebitBegin.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebitBegin.Name = "lblSumDebitBegin"
            Me.lblSumDebitBegin.Size = New System.Drawing.Size(110, 18)
            Me.lblSumDebitBegin.TabIndex = 3
            Me.lblSumDebitBegin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCreditBegin
            '
            Me.lblSumCreditBegin.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCreditBegin.Name = "lblSumCreditBegin"
            Me.lblSumCreditBegin.Size = New System.Drawing.Size(110, 18)
            Me.lblSumCreditBegin.TabIndex = 4
            Me.lblSumCreditBegin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumDebitDuring
            '
            Me.lblSumDebitDuring.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebitDuring.Name = "lblSumDebitDuring"
            Me.lblSumDebitDuring.Size = New System.Drawing.Size(110, 18)
            Me.lblSumDebitDuring.TabIndex = 5
            Me.lblSumDebitDuring.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCreditDuring
            '
            Me.lblSumCreditDuring.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCreditDuring.Name = "lblSumCreditDuring"
            Me.lblSumCreditDuring.Size = New System.Drawing.Size(110, 18)
            Me.lblSumCreditDuring.TabIndex = 6
            Me.lblSumCreditDuring.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumDebitTotal
            '
            Me.lblSumDebitTotal.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebitTotal.Name = "lblSumDebitTotal"
            Me.lblSumDebitTotal.Size = New System.Drawing.Size(110, 18)
            Me.lblSumDebitTotal.TabIndex = 7
            Me.lblSumDebitTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCreditTotal
            '
            Me.lblSumCreditTotal.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCreditTotal.Name = "lblSumCreditTotal"
            Me.lblSumCreditTotal.Size = New System.Drawing.Size(110, 18)
            Me.lblSumCreditTotal.TabIndex = 8
            Me.lblSumCreditTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumDebitEnd
            '
            Me.lblSumDebitEnd.Location = New System.Drawing.Point(0, 8)
            Me.lblSumDebitEnd.Name = "lblSumDebitEnd"
            Me.lblSumDebitEnd.Size = New System.Drawing.Size(110, 18)
            Me.lblSumDebitEnd.TabIndex = 9
            Me.lblSumDebitEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblSumCreditEnd
            '
            Me.lblSumCreditEnd.Location = New System.Drawing.Point(0, 8)
            Me.lblSumCreditEnd.Name = "lblSumCreditEnd"
            Me.lblSumCreditEnd.Size = New System.Drawing.Size(110, 18)
            Me.lblSumCreditEnd.TabIndex = 10
            Me.lblSumCreditEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlGridSearch
            '
            Me.pnlGridSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlGridSearch.Controls.Add(Me.txtSearchToggle)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchLedger)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCode)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchName)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchDebitBefore)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCreditBefore)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchDebitBegin)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCreditBegin)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchDebitDuring)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCreditDuring)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchDebitTotal)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCreditTotal)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchDebitEnd)
            Me.pnlGridSearch.Controls.Add(Me.txtSearchCreditEnd)
            Me.pnlGridSearch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlGridSearch.Location = New System.Drawing.Point(0, 84)
            Me.pnlGridSearch.Name = "pnlGridSearch"
            Me.pnlGridSearch.Size = New System.Drawing.Size(1320, 28)
            Me.pnlGridSearch.TabIndex = 4
            '
            'txtSearchToggle
            '
            Me.txtSearchToggle.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
            Me.txtSearchToggle.Enabled = False
            Me.txtSearchToggle.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchToggle.Location = New System.Drawing.Point(1290, 3)
            Me.txtSearchToggle.Name = "txtSearchToggle"
            Me.txtSearchToggle.Size = New System.Drawing.Size(30, 21)
            Me.txtSearchToggle.TabIndex = 0
            '
            'txtSearchLedger
            '
            Me.txtSearchLedger.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
            Me.txtSearchLedger.Enabled = False
            Me.txtSearchLedger.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchLedger.Location = New System.Drawing.Point(1200, 3)
            Me.txtSearchLedger.Name = "txtSearchLedger"
            Me.txtSearchLedger.Size = New System.Drawing.Size(90, 21)
            Me.txtSearchLedger.TabIndex = 1
            '
            'txtSearchCode
            '
            Me.txtSearchCode.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCode.Location = New System.Drawing.Point(1110, 3)
            Me.txtSearchCode.Name = "txtSearchCode"
            Me.txtSearchCode.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCode.TabIndex = 2
            '
            'txtSearchName
            '
            Me.txtSearchName.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchName.Location = New System.Drawing.Point(910, 3)
            Me.txtSearchName.Name = "txtSearchName"
            Me.txtSearchName.Size = New System.Drawing.Size(200, 21)
            Me.txtSearchName.TabIndex = 3
            '
            'txtSearchDebitBefore
            '
            Me.txtSearchDebitBefore.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchDebitBefore.Location = New System.Drawing.Point(800, 3)
            Me.txtSearchDebitBefore.Name = "txtSearchDebitBefore"
            Me.txtSearchDebitBefore.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchDebitBefore.TabIndex = 4
            '
            'txtSearchCreditBefore
            '
            Me.txtSearchCreditBefore.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCreditBefore.Location = New System.Drawing.Point(690, 3)
            Me.txtSearchCreditBefore.Name = "txtSearchCreditBefore"
            Me.txtSearchCreditBefore.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCreditBefore.TabIndex = 5
            '
            'txtSearchDebitBegin
            '
            Me.txtSearchDebitBegin.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchDebitBegin.Location = New System.Drawing.Point(580, 3)
            Me.txtSearchDebitBegin.Name = "txtSearchDebitBegin"
            Me.txtSearchDebitBegin.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchDebitBegin.TabIndex = 6
            '
            'txtSearchCreditBegin
            '
            Me.txtSearchCreditBegin.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCreditBegin.Location = New System.Drawing.Point(470, 3)
            Me.txtSearchCreditBegin.Name = "txtSearchCreditBegin"
            Me.txtSearchCreditBegin.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCreditBegin.TabIndex = 7
            '
            'txtSearchDebitDuring
            '
            Me.txtSearchDebitDuring.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchDebitDuring.Location = New System.Drawing.Point(360, 3)
            Me.txtSearchDebitDuring.Name = "txtSearchDebitDuring"
            Me.txtSearchDebitDuring.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchDebitDuring.TabIndex = 8
            '
            'txtSearchCreditDuring
            '
            Me.txtSearchCreditDuring.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCreditDuring.Location = New System.Drawing.Point(250, 3)
            Me.txtSearchCreditDuring.Name = "txtSearchCreditDuring"
            Me.txtSearchCreditDuring.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCreditDuring.TabIndex = 9
            '
            'txtSearchDebitTotal
            '
            Me.txtSearchDebitTotal.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchDebitTotal.Location = New System.Drawing.Point(140, 3)
            Me.txtSearchDebitTotal.Name = "txtSearchDebitTotal"
            Me.txtSearchDebitTotal.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchDebitTotal.TabIndex = 10
            '
            'txtSearchCreditTotal
            '
            Me.txtSearchCreditTotal.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCreditTotal.Location = New System.Drawing.Point(30, 3)
            Me.txtSearchCreditTotal.Name = "txtSearchCreditTotal"
            Me.txtSearchCreditTotal.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCreditTotal.TabIndex = 11
            '
            'txtSearchDebitEnd
            '
            Me.txtSearchDebitEnd.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchDebitEnd.Location = New System.Drawing.Point(0, 3)
            Me.txtSearchDebitEnd.Name = "txtSearchDebitEnd"
            Me.txtSearchDebitEnd.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchDebitEnd.TabIndex = 12
            '
            'txtSearchCreditEnd
            '
            Me.txtSearchCreditEnd.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.txtSearchCreditEnd.Location = New System.Drawing.Point(0, 3)
            Me.txtSearchCreditEnd.Name = "txtSearchCreditEnd"
            Me.txtSearchCreditEnd.Size = New System.Drawing.Size(110, 21)
            Me.txtSearchCreditEnd.TabIndex = 13
            '
            'HesabdaryTarazShenavarForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1320, 749)
            Me.Controls.Add(Me.dgvTrial)
            Me.Controls.Add(Me.pnlJam)
            Me.Controls.Add(Me.pnlGridSearch)
            Me.Controls.Add(Me.pnlFilters)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryTarazShenavarForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "تراز شناور"
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvTrial, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlFilters.ResumeLayout(False)
            Me.pnlFilters.PerformLayout()
            Me.pnlJam.ResumeLayout(False)
            Me.pnlGridSearch.ResumeLayout(False)
            Me.pnlGridSearch.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents colToggle As DataGridViewTextBoxColumn
        Friend WithEvents colLedger As DataGridViewButtonColumn
        Friend WithEvents colCode As DataGridViewTextBoxColumn
        Friend WithEvents colName As DataGridViewTextBoxColumn
        Friend WithEvents colDebitBefore As DataGridViewTextBoxColumn
        Friend WithEvents colCreditBefore As DataGridViewTextBoxColumn
        Friend WithEvents colDebitBegin As DataGridViewTextBoxColumn
        Friend WithEvents colCreditBegin As DataGridViewTextBoxColumn
        Friend WithEvents colDebitDuring As DataGridViewTextBoxColumn
        Friend WithEvents colCreditDuring As DataGridViewTextBoxColumn
        Friend WithEvents colDebitTotal As DataGridViewTextBoxColumn
        Friend WithEvents colCreditTotal As DataGridViewTextBoxColumn
        Friend WithEvents colDebitEnd As DataGridViewTextBoxColumn
        Friend WithEvents colCreditEnd As DataGridViewTextBoxColumn
        Friend WithEvents pnlGridSearch As Panel
        Friend WithEvents txtSearchToggle As TextBox
        Friend WithEvents txtSearchLedger As TextBox
        Friend WithEvents txtSearchCode As TextBox
        Friend WithEvents txtSearchName As TextBox
        Friend WithEvents txtSearchDebitBefore As TextBox
        Friend WithEvents txtSearchCreditBefore As TextBox
        Friend WithEvents txtSearchDebitBegin As TextBox
        Friend WithEvents txtSearchCreditBegin As TextBox
        Friend WithEvents txtSearchDebitDuring As TextBox
        Friend WithEvents txtSearchCreditDuring As TextBox
        Friend WithEvents txtSearchDebitTotal As TextBox
        Friend WithEvents txtSearchCreditTotal As TextBox
        Friend WithEvents txtSearchDebitEnd As TextBox
        Friend WithEvents txtSearchCreditEnd As TextBox
        Friend WithEvents btnExportExcel As Button
    End Class
End Namespace
