Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class HesabdaryReport2Form
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
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
        <System.Diagnostics.DebuggerStepThrough()> _
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
            Me.txtCode = New System.Windows.Forms.TextBox()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.btnAddToCategories = New System.Windows.Forms.Button()
            Me.btnDeleteRow = New System.Windows.Forms.Button()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnPrintReport = New System.Windows.Forms.Button()
            Me.btnEditHelp = New System.Windows.Forms.Button()
            Me.btnExit = New System.Windows.Forms.Button()
            Me.dgvReports = New System.Windows.Forms.DataGridView()
            Me.colToggle = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colRowNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colIsMainRow = New System.Windows.Forms.DataGridViewComboBoxColumn()
            Me.colRO = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSO = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colResult = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colUnderlineStyle = New System.Windows.Forms.DataGridViewComboBoxColumn()
            Me.colRN = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSN = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAdd = New System.Windows.Forms.DataGridViewButtonColumn()
            Me.colCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colID = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colEditFormula = New System.Windows.Forms.DataGridViewButtonColumn()
            Me.colFormula = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.lblAccountTitle = New System.Windows.Forms.Label()
            Me.pnlConfig = New System.Windows.Forms.Panel()
            Me.gbFonts = New System.Windows.Forms.GroupBox()
            Me.numSizeFormulaDetail = New System.Windows.Forms.NumericUpDown()
            Me.cmbFontFormulaDetail = New System.Windows.Forms.ComboBox()
            Me.lblF5 = New System.Windows.Forms.Label()
            Me.numSizeFormula = New System.Windows.Forms.NumericUpDown()
            Me.cmbFontFormula = New System.Windows.Forms.ComboBox()
            Me.lblF4 = New System.Windows.Forms.Label()
            Me.numSizeDetailRow = New System.Windows.Forms.NumericUpDown()
            Me.cmbFontDetailRow = New System.Windows.Forms.ComboBox()
            Me.lblF3 = New System.Windows.Forms.Label()
            Me.numSizeMainRow = New System.Windows.Forms.NumericUpDown()
            Me.cmbFontMainRow = New System.Windows.Forms.ComboBox()
            Me.lblF2 = New System.Windows.Forms.Label()
            Me.numSizeHeader = New System.Windows.Forms.NumericUpDown()
            Me.cmbFontHeader = New System.Windows.Forms.ComboBox()
            Me.lblF1 = New System.Windows.Forms.Label()
            Me.gbLayout = New System.Windows.Forms.GroupBox()
            Me.lblPaperSize = New System.Windows.Forms.Label()
            Me.cmbPaperSize = New System.Windows.Forms.ComboBox()
            Me.cmbOrientation = New System.Windows.Forms.ComboBox()
            Me.lblL3 = New System.Windows.Forms.Label()
            Me.lblL2 = New System.Windows.Forms.Label()
            Me.lblL1 = New System.Windows.Forms.Label()
            Me.gbMargins = New System.Windows.Forms.GroupBox()
            Me.numMarginRight = New System.Windows.Forms.NumericUpDown()
            Me.lblM4 = New System.Windows.Forms.Label()
            Me.numMarginLeft = New System.Windows.Forms.NumericUpDown()
            Me.lblM3 = New System.Windows.Forms.Label()
            Me.numMarginBottom = New System.Windows.Forms.NumericUpDown()
            Me.lblM2 = New System.Windows.Forms.Label()
            Me.numMarginTop = New System.Windows.Forms.NumericUpDown()
            Me.lblM1 = New System.Windows.Forms.Label()
            Me.gbBorder = New System.Windows.Forms.GroupBox()
            Me.cmbPageBorder = New System.Windows.Forms.ComboBox()
            Me.lblB1 = New System.Windows.Forms.Label()
            Me.numColCount = New System.Windows.Forms.NumericUpDown()
            Me.numRowCount = New System.Windows.Forms.NumericUpDown()
            Me.pnlColumnLetters = New System.Windows.Forms.Panel()
            Me.lblColN = New System.Windows.Forms.Label()
            Me.lblColO = New System.Windows.Forms.Label()
            Me.lblColM = New System.Windows.Forms.Label()
            Me.lblColL = New System.Windows.Forms.Label()
            Me.lblColK = New System.Windows.Forms.Label()
            Me.lblColJ = New System.Windows.Forms.Label()
            Me.lblColI = New System.Windows.Forms.Label()
            Me.lblColH = New System.Windows.Forms.Label()
            Me.lblColG = New System.Windows.Forms.Label()
            Me.lblColF = New System.Windows.Forms.Label()
            Me.lblColE = New System.Windows.Forms.Label()
            Me.lblColD = New System.Windows.Forms.Label()
            Me.lblColC = New System.Windows.Forms.Label()
            Me.lblColB = New System.Windows.Forms.Label()
            Me.lblColA = New System.Windows.Forms.Label()
            Me.pnlTitleHeader = New System.Windows.Forms.Panel()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblName = New System.Windows.Forms.Label()
            Me.pnlHeaderSpacer = New System.Windows.Forms.Panel()
            Me.lblCode = New System.Windows.Forms.Label()
            Me.pnlActions = New System.Windows.Forms.Panel()
            Me.pnlSpacing2 = New System.Windows.Forms.Panel()
            Me.pnlSpacing1 = New System.Windows.Forms.Panel()
            Me.pnlSpacingEditHelp = New System.Windows.Forms.Panel()
            Me.pnlSpacingPrint = New System.Windows.Forms.Panel()
            Me.pnlSpacing0 = New System.Windows.Forms.Panel()
            CType(Me.dgvReports, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlConfig.SuspendLayout()
            Me.gbFonts.SuspendLayout()
            CType(Me.numSizeFormulaDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numSizeFormula, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numSizeDetailRow, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numSizeMainRow, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numSizeHeader, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbLayout.SuspendLayout()
            Me.gbMargins.SuspendLayout()
            CType(Me.numMarginRight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numMarginLeft, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numMarginBottom, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numMarginTop, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbBorder.SuspendLayout()
            CType(Me.numColCount, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numRowCount, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlColumnLetters.SuspendLayout()
            Me.pnlTitleHeader.SuspendLayout()
            Me.pnlHeader.SuspendLayout()
            Me.pnlActions.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtCode
            '
            Me.txtCode.Dock = System.Windows.Forms.DockStyle.Right
            Me.txtCode.Location = New System.Drawing.Point(1085, 10)
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New System.Drawing.Size(100, 22)
            Me.txtCode.TabIndex = 1
            '
            'txtName
            '
            Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtName.Location = New System.Drawing.Point(10, 10)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(985, 22)
            Me.txtName.TabIndex = 4
            '
            'btnAddToCategories
            '
            Me.btnAddToCategories.BackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.btnAddToCategories.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnAddToCategories.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnAddToCategories.Location = New System.Drawing.Point(550, 8)
            Me.btnAddToCategories.Name = "btnAddToCategories"
            Me.btnAddToCategories.Size = New System.Drawing.Size(100, 29)
            Me.btnAddToCategories.TabIndex = 6
            Me.btnAddToCategories.Text = "افزودن سطر"
            Me.btnAddToCategories.UseVisualStyleBackColor = False
            '
            'btnDeleteRow
            '
            Me.btnDeleteRow.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
            Me.btnDeleteRow.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnDeleteRow.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnDeleteRow.Location = New System.Drawing.Point(440, 8)
            Me.btnDeleteRow.Name = "btnDeleteRow"
            Me.btnDeleteRow.Size = New System.Drawing.Size(100, 29)
            Me.btnDeleteRow.TabIndex = 4
            Me.btnDeleteRow.Text = "حذف سطر"
            Me.btnDeleteRow.UseVisualStyleBackColor = False
            '
            'btnSave
            '
            Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnSave.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnSave.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSave.Location = New System.Drawing.Point(80, 8)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(100, 29)
            Me.btnSave.TabIndex = 2
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnPrintReport
            '
            Me.btnPrintReport.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(170, Byte), Integer))
            Me.btnPrintReport.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnPrintReport.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnPrintReport.Location = New System.Drawing.Point(190, 8)
            Me.btnPrintReport.Name = "btnPrintReport"
            Me.btnPrintReport.Size = New System.Drawing.Size(100, 29)
            Me.btnPrintReport.TabIndex = 7
            Me.btnPrintReport.Text = "نمایش و چاپ"
            Me.btnPrintReport.UseVisualStyleBackColor = False
            '
            'btnEditHelp
            '
            Me.btnEditHelp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
            Me.btnEditHelp.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnEditHelp.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnEditHelp.Location = New System.Drawing.Point(300, 8)
            Me.btnEditHelp.Name = "btnEditHelp"
            Me.btnEditHelp.Size = New System.Drawing.Size(130, 29)
            Me.btnEditHelp.TabIndex = 8
            Me.btnEditHelp.Text = "ویرایش متن راهنما"
            Me.btnEditHelp.UseVisualStyleBackColor = False
            '
            'btnExit
            '
            Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.btnExit.Dock = System.Windows.Forms.DockStyle.Left
            Me.btnExit.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnExit.Location = New System.Drawing.Point(10, 8)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(60, 29)
            Me.btnExit.TabIndex = 0
            Me.btnExit.Text = "خروج"
            Me.btnExit.UseVisualStyleBackColor = False
            '
            'dgvReports
            '
            Me.dgvReports.AllowUserToAddRows = False
            Me.dgvReports.AllowUserToDeleteRows = False
            Me.dgvReports.BackgroundColor = System.Drawing.Color.White
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(160, Byte), Integer))
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!)
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvReports.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.dgvReports.ColumnHeadersHeight = 30
            Me.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvReports.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colToggle, Me.colRowNo, Me.colCategory, Me.colIsMainRow, Me.colRO, Me.colSO, Me.colResult, Me.colUnderlineStyle, Me.colRN, Me.colSN, Me.colAdd, Me.colCode, Me.colName, Me.colID, Me.colEditFormula, Me.colFormula})
            Me.dgvReports.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvReports.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
            Me.dgvReports.EnableHeadersVisualStyles = False
            Me.dgvReports.Location = New System.Drawing.Point(0, 319)
            Me.dgvReports.MultiSelect = False
            Me.dgvReports.Name = "dgvReports"
            Me.dgvReports.RowHeadersVisible = False
            Me.dgvReports.RowTemplate.Height = 26
            Me.dgvReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvReports.Size = New System.Drawing.Size(1250, 430)
            Me.dgvReports.TabIndex = 5
            '
            'colToggle
            '
            Me.colToggle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colToggle.DefaultCellStyle = DataGridViewCellStyle2
            Me.colToggle.HeaderText = "+ / -"
            Me.colToggle.Name = "colToggle"
            Me.colToggle.ReadOnly = True
            Me.colToggle.Width = 35
            '
            'colRowNo
            '
            Me.colRowNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colRowNo.DefaultCellStyle = DataGridViewCellStyle3
            Me.colRowNo.HeaderText = "ردیف"
            Me.colRowNo.Name = "colRowNo"
            Me.colRowNo.ReadOnly = True
            Me.colRowNo.Width = 35
            '
            'colCategory
            '
            Me.colCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colCategory.HeaderText = "عنوان ردیف گزارش"
            Me.colCategory.Name = "colCategory"
            '
            'colIsMainRow
            '
            Me.colIsMainRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle4.NullValue = "جزئی"
            Me.colIsMainRow.DefaultCellStyle = DataGridViewCellStyle4
            Me.colIsMainRow.HeaderText = "نوع ردیف"
            Me.colIsMainRow.Items.AddRange(New Object() {"اصلی", "جزئی"})
            Me.colIsMainRow.Name = "colIsMainRow"
            Me.colIsMainRow.Width = 65
            '
            'colRO
            '
            Me.colRO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle5.Padding = New System.Windows.Forms.Padding(0, 0, 18, 0)
            Me.colRO.DefaultCellStyle = DataGridViewCellStyle5
            Me.colRO.HeaderText = "R_O"
            Me.colRO.Name = "colRO"
            Me.colRO.Width = 40
            '
            'colSO
            '
            Me.colSO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle6.Padding = New System.Windows.Forms.Padding(0, 0, 18, 0)
            Me.colSO.DefaultCellStyle = DataGridViewCellStyle6
            Me.colSO.HeaderText = "S_O"
            Me.colSO.Name = "colSO"
            Me.colSO.Width = 40
            '
            'colResult
            '
            Me.colResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colResult.HeaderText = "نتیجه فرمول"
            Me.colResult.Name = "colResult"
            Me.colResult.ReadOnly = True
            Me.colResult.Width = 75
            '
            'colUnderlineStyle
            '
            Me.colUnderlineStyle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle7.NullValue = "بدون خط"
            Me.colUnderlineStyle.DefaultCellStyle = DataGridViewCellStyle7
            Me.colUnderlineStyle.HeaderText = "خط زیر نتیجه"
            Me.colUnderlineStyle.Items.AddRange(New Object() {"بدون خط", "خط تکی نازک", "خط تکی ضخیم", "خط دوتایی", "خط دوتایی نازک، ضخیم"})
            Me.colUnderlineStyle.Name = "colUnderlineStyle"
            '
            'colRN
            '
            Me.colRN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle8.Padding = New System.Windows.Forms.Padding(0, 0, 18, 0)
            Me.colRN.DefaultCellStyle = DataGridViewCellStyle8
            Me.colRN.HeaderText = "R_N"
            Me.colRN.Name = "colRN"
            Me.colRN.Width = 40
            '
            'colSN
            '
            Me.colSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle9.Padding = New System.Windows.Forms.Padding(0, 0, 18, 0)
            Me.colSN.DefaultCellStyle = DataGridViewCellStyle9
            Me.colSN.HeaderText = "S_N"
            Me.colSN.Name = "colSN"
            Me.colSN.Width = 40
            '
            'colAdd
            '
            Me.colAdd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAdd.HeaderText = "کد سرفصل"
            Me.colAdd.Name = "colAdd"
            Me.colAdd.ReadOnly = True
            Me.colAdd.Width = 70
            '
            'colCode
            '
            Me.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.colCode.DefaultCellStyle = DataGridViewCellStyle10
            Me.colCode.HeaderText = "کد سرفصل"
            Me.colCode.Name = "colCode"
            Me.colCode.ReadOnly = True
            Me.colCode.Width = 60
            '
            'colName
            '
            Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colName.HeaderText = "نام سرفصل"
            Me.colName.Name = "colName"
            Me.colName.ReadOnly = True
            '
            'colID
            '
            Me.colID.HeaderText = "آی دی"
            Me.colID.Name = "colID"
            Me.colID.ReadOnly = True
            Me.colID.Width = 60
            '
            'colEditFormula
            '
            Me.colEditFormula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colEditFormula.HeaderText = ""
            Me.colEditFormula.Name = "colEditFormula"
            Me.colEditFormula.Text = "..."
            Me.colEditFormula.UseColumnTextForButtonValue = True
            Me.colEditFormula.Width = 30
            '
            'colFormula
            '
            Me.colFormula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colFormula.HeaderText = "فرمول محاسبه این ردیف"
            Me.colFormula.Name = "colFormula"
            Me.colFormula.Width = 160
            '
            'lblAccountTitle
            '
            Me.lblAccountTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblAccountTitle.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblAccountTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(120, Byte), Integer))
            Me.lblAccountTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblAccountTitle.Name = "lblAccountTitle"
            Me.lblAccountTitle.Size = New System.Drawing.Size(1250, 44)
            Me.lblAccountTitle.TabIndex = 0
            Me.lblAccountTitle.Text = "طراحی گزارشات دلخواه"
            Me.lblAccountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlConfig
            '
            Me.pnlConfig.BackColor = System.Drawing.Color.White
            Me.pnlConfig.Controls.Add(Me.gbFonts)
            Me.pnlConfig.Controls.Add(Me.gbLayout)
            Me.pnlConfig.Controls.Add(Me.gbMargins)
            Me.pnlConfig.Controls.Add(Me.gbBorder)
            Me.pnlConfig.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlConfig.Location = New System.Drawing.Point(0, 99)
            Me.pnlConfig.Name = "pnlConfig"
            Me.pnlConfig.Size = New System.Drawing.Size(1250, 150)
            Me.pnlConfig.TabIndex = 2
            '
            'gbFonts
            '
            Me.gbFonts.Controls.Add(Me.numSizeFormulaDetail)
            Me.gbFonts.Controls.Add(Me.cmbFontFormulaDetail)
            Me.gbFonts.Controls.Add(Me.lblF5)
            Me.gbFonts.Controls.Add(Me.numSizeFormula)
            Me.gbFonts.Controls.Add(Me.cmbFontFormula)
            Me.gbFonts.Controls.Add(Me.lblF4)
            Me.gbFonts.Controls.Add(Me.numSizeDetailRow)
            Me.gbFonts.Controls.Add(Me.cmbFontDetailRow)
            Me.gbFonts.Controls.Add(Me.lblF3)
            Me.gbFonts.Controls.Add(Me.numSizeMainRow)
            Me.gbFonts.Controls.Add(Me.cmbFontMainRow)
            Me.gbFonts.Controls.Add(Me.lblF2)
            Me.gbFonts.Controls.Add(Me.numSizeHeader)
            Me.gbFonts.Controls.Add(Me.cmbFontHeader)
            Me.gbFonts.Controls.Add(Me.lblF1)
            Me.gbFonts.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.gbFonts.Location = New System.Drawing.Point(820, 3)
            Me.gbFonts.Name = "gbFonts"
            Me.gbFonts.Size = New System.Drawing.Size(420, 145)
            Me.gbFonts.TabIndex = 0
            Me.gbFonts.TabStop = False
            Me.gbFonts.Text = "تنظیمات فونت گزارش"
            '
            'numSizeFormulaDetail
            '
            Me.numSizeFormulaDetail.DecimalPlaces = 1
            Me.numSizeFormulaDetail.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numSizeFormulaDetail.Location = New System.Drawing.Point(10, 119)
            Me.numSizeFormulaDetail.Maximum = New Decimal(New Integer() {36, 0, 0, 0})
            Me.numSizeFormulaDetail.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numSizeFormulaDetail.Name = "numSizeFormulaDetail"
            Me.numSizeFormulaDetail.Size = New System.Drawing.Size(60, 20)
            Me.numSizeFormulaDetail.TabIndex = 14
            Me.numSizeFormulaDetail.Value = New Decimal(New Integer() {9, 0, 0, 0})
            '
            'cmbFontFormulaDetail
            '
            Me.cmbFontFormulaDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFontFormulaDetail.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbFontFormulaDetail.Items.AddRange(New Object() {"Tahoma", "Arial", "B Nazanin", "B Zar", "Segoe UI"})
            Me.cmbFontFormulaDetail.Location = New System.Drawing.Point(75, 119)
            Me.cmbFontFormulaDetail.Name = "cmbFontFormulaDetail"
            Me.cmbFontFormulaDetail.Size = New System.Drawing.Size(200, 21)
            Me.cmbFontFormulaDetail.TabIndex = 13
            '
            'lblF5
            '
            Me.lblF5.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblF5.Location = New System.Drawing.Point(275, 119)
            Me.lblF5.Name = "lblF5"
            Me.lblF5.Size = New System.Drawing.Size(140, 20)
            Me.lblF5.TabIndex = 12
            Me.lblF5.Text = "نتیجه فرمول ردیفهای فرعی:"
            Me.lblF5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numSizeFormula
            '
            Me.numSizeFormula.DecimalPlaces = 1
            Me.numSizeFormula.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numSizeFormula.Location = New System.Drawing.Point(10, 93)
            Me.numSizeFormula.Maximum = New Decimal(New Integer() {36, 0, 0, 0})
            Me.numSizeFormula.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numSizeFormula.Name = "numSizeFormula"
            Me.numSizeFormula.Size = New System.Drawing.Size(60, 20)
            Me.numSizeFormula.TabIndex = 11
            Me.numSizeFormula.Value = New Decimal(New Integer() {9, 0, 0, 0})
            '
            'cmbFontFormula
            '
            Me.cmbFontFormula.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFontFormula.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbFontFormula.Items.AddRange(New Object() {"Tahoma", "Arial", "B Nazanin", "B Zar", "Segoe UI"})
            Me.cmbFontFormula.Location = New System.Drawing.Point(75, 93)
            Me.cmbFontFormula.Name = "cmbFontFormula"
            Me.cmbFontFormula.Size = New System.Drawing.Size(200, 21)
            Me.cmbFontFormula.TabIndex = 10
            '
            'lblF4
            '
            Me.lblF4.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblF4.Location = New System.Drawing.Point(275, 93)
            Me.lblF4.Name = "lblF4"
            Me.lblF4.Size = New System.Drawing.Size(140, 20)
            Me.lblF4.TabIndex = 9
            Me.lblF4.Text = "نتیجه فرمول ردیفهای اصلی:"
            Me.lblF4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numSizeDetailRow
            '
            Me.numSizeDetailRow.DecimalPlaces = 1
            Me.numSizeDetailRow.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numSizeDetailRow.Location = New System.Drawing.Point(10, 68)
            Me.numSizeDetailRow.Maximum = New Decimal(New Integer() {36, 0, 0, 0})
            Me.numSizeDetailRow.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numSizeDetailRow.Name = "numSizeDetailRow"
            Me.numSizeDetailRow.Size = New System.Drawing.Size(60, 20)
            Me.numSizeDetailRow.TabIndex = 8
            Me.numSizeDetailRow.Value = New Decimal(New Integer() {9, 0, 0, 0})
            '
            'cmbFontDetailRow
            '
            Me.cmbFontDetailRow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFontDetailRow.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbFontDetailRow.Items.AddRange(New Object() {"Tahoma", "Arial", "B Nazanin", "B Zar", "Segoe UI"})
            Me.cmbFontDetailRow.Location = New System.Drawing.Point(75, 68)
            Me.cmbFontDetailRow.Name = "cmbFontDetailRow"
            Me.cmbFontDetailRow.Size = New System.Drawing.Size(200, 21)
            Me.cmbFontDetailRow.TabIndex = 7
            '
            'lblF3
            '
            Me.lblF3.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblF3.Location = New System.Drawing.Point(315, 68)
            Me.lblF3.Name = "lblF3"
            Me.lblF3.Size = New System.Drawing.Size(140, 20)
            Me.lblF3.TabIndex = 6
            Me.lblF3.Text = "ردیف‌های جزئی:"
            Me.lblF3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numSizeMainRow
            '
            Me.numSizeMainRow.DecimalPlaces = 1
            Me.numSizeMainRow.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numSizeMainRow.Location = New System.Drawing.Point(10, 43)
            Me.numSizeMainRow.Maximum = New Decimal(New Integer() {36, 0, 0, 0})
            Me.numSizeMainRow.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numSizeMainRow.Name = "numSizeMainRow"
            Me.numSizeMainRow.Size = New System.Drawing.Size(60, 20)
            Me.numSizeMainRow.TabIndex = 5
            Me.numSizeMainRow.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'cmbFontMainRow
            '
            Me.cmbFontMainRow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFontMainRow.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbFontMainRow.Items.AddRange(New Object() {"Tahoma", "Arial", "B Nazanin", "B Zar", "Segoe UI"})
            Me.cmbFontMainRow.Location = New System.Drawing.Point(75, 43)
            Me.cmbFontMainRow.Name = "cmbFontMainRow"
            Me.cmbFontMainRow.Size = New System.Drawing.Size(200, 21)
            Me.cmbFontMainRow.TabIndex = 4
            '
            'lblF2
            '
            Me.lblF2.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblF2.Location = New System.Drawing.Point(315, 43)
            Me.lblF2.Name = "lblF2"
            Me.lblF2.Size = New System.Drawing.Size(140, 20)
            Me.lblF2.TabIndex = 3
            Me.lblF2.Text = "ردیف‌های اصلی:"
            Me.lblF2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numSizeHeader
            '
            Me.numSizeHeader.DecimalPlaces = 1
            Me.numSizeHeader.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numSizeHeader.Location = New System.Drawing.Point(10, 18)
            Me.numSizeHeader.Maximum = New Decimal(New Integer() {36, 0, 0, 0})
            Me.numSizeHeader.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
            Me.numSizeHeader.Name = "numSizeHeader"
            Me.numSizeHeader.Size = New System.Drawing.Size(60, 20)
            Me.numSizeHeader.TabIndex = 2
            Me.numSizeHeader.Value = New Decimal(New Integer() {12, 0, 0, 0})
            '
            'cmbFontHeader
            '
            Me.cmbFontHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFontHeader.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbFontHeader.Items.AddRange(New Object() {"Tahoma", "Arial", "B Nazanin", "B Zar", "B Titr", "Segoe UI"})
            Me.cmbFontHeader.Location = New System.Drawing.Point(75, 18)
            Me.cmbFontHeader.Name = "cmbFontHeader"
            Me.cmbFontHeader.Size = New System.Drawing.Size(200, 21)
            Me.cmbFontHeader.TabIndex = 1
            '
            'lblF1
            '
            Me.lblF1.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblF1.Location = New System.Drawing.Point(315, 18)
            Me.lblF1.Name = "lblF1"
            Me.lblF1.Size = New System.Drawing.Size(140, 20)
            Me.lblF1.TabIndex = 0
            Me.lblF1.Text = "عنوان هدر/اصلی:"
            Me.lblF1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'gbLayout
            '
            Me.gbLayout.Controls.Add(Me.lblPaperSize)
            Me.gbLayout.Controls.Add(Me.cmbPaperSize)
            Me.gbLayout.Controls.Add(Me.cmbOrientation)
            Me.gbLayout.Controls.Add(Me.lblL3)
            Me.gbLayout.Controls.Add(Me.lblL2)
            Me.gbLayout.Controls.Add(Me.lblL1)
            Me.gbLayout.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.gbLayout.Location = New System.Drawing.Point(215, 3)
            Me.gbLayout.Name = "gbLayout"
            Me.gbLayout.Size = New System.Drawing.Size(270, 130)
            Me.gbLayout.TabIndex = 1
            Me.gbLayout.TabStop = False
            Me.gbLayout.Text = "ابعاد و جهت گزارش"
            '
            'lblPaperSize
            '
            Me.lblPaperSize.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblPaperSize.Location = New System.Drawing.Point(135, 23)
            Me.lblPaperSize.Name = "lblPaperSize"
            Me.lblPaperSize.Size = New System.Drawing.Size(120, 20)
            Me.lblPaperSize.TabIndex = 7
            Me.lblPaperSize.Text = "نوع کاغذ:"
            Me.lblPaperSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cmbPaperSize
            '
            Me.cmbPaperSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPaperSize.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbPaperSize.Items.AddRange(New Object() {"A4", "A3", "A5", "Letter"})
            Me.cmbPaperSize.Location = New System.Drawing.Point(15, 23)
            Me.cmbPaperSize.Name = "cmbPaperSize"
            Me.cmbPaperSize.Size = New System.Drawing.Size(110, 21)
            Me.cmbPaperSize.TabIndex = 6
            '
            'cmbOrientation
            '
            Me.cmbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbOrientation.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbOrientation.Items.AddRange(New Object() {"عمودی", "افقی"})
            Me.cmbOrientation.Location = New System.Drawing.Point(15, 53)
            Me.cmbOrientation.Name = "cmbOrientation"
            Me.cmbOrientation.Size = New System.Drawing.Size(110, 21)
            Me.cmbOrientation.TabIndex = 5
            '
            'lblL3
            '
            Me.lblL3.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblL3.Location = New System.Drawing.Point(135, 53)
            Me.lblL3.Name = "lblL3"
            Me.lblL3.Size = New System.Drawing.Size(120, 20)
            Me.lblL3.TabIndex = 4
            Me.lblL3.Text = "جهت صفحات گزارش:"
            Me.lblL3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblL2
            '
            Me.lblL2.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblL2.Location = New System.Drawing.Point(5, 103)
            Me.lblL2.Name = "lblL2"
            Me.lblL2.Size = New System.Drawing.Size(260, 20)
            Me.lblL2.TabIndex = 2
            Me.lblL2.Text = "ارتفاع متن گزارش به جز حاشیه :"
            Me.lblL2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblL1
            '
            Me.lblL1.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblL1.Location = New System.Drawing.Point(5, 83)
            Me.lblL1.Name = "lblL1"
            Me.lblL1.Size = New System.Drawing.Size(260, 20)
            Me.lblL1.TabIndex = 0
            Me.lblL1.Text = "عرض متن گزارش به جز حاشیه :"
            Me.lblL1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'gbMargins
            '
            Me.gbMargins.Controls.Add(Me.numMarginRight)
            Me.gbMargins.Controls.Add(Me.lblM4)
            Me.gbMargins.Controls.Add(Me.numMarginLeft)
            Me.gbMargins.Controls.Add(Me.lblM3)
            Me.gbMargins.Controls.Add(Me.numMarginBottom)
            Me.gbMargins.Controls.Add(Me.lblM2)
            Me.gbMargins.Controls.Add(Me.numMarginTop)
            Me.gbMargins.Controls.Add(Me.lblM1)
            Me.gbMargins.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.gbMargins.Location = New System.Drawing.Point(490, 3)
            Me.gbMargins.Name = "gbMargins"
            Me.gbMargins.Size = New System.Drawing.Size(320, 120)
            Me.gbMargins.TabIndex = 2
            Me.gbMargins.TabStop = False
            Me.gbMargins.Text = "حاشیه صفحه (میلی‌متر)"
            '
            'numMarginRight
            '
            Me.numMarginRight.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numMarginRight.Location = New System.Drawing.Point(15, 53)
            Me.numMarginRight.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
            Me.numMarginRight.Name = "numMarginRight"
            Me.numMarginRight.Size = New System.Drawing.Size(60, 20)
            Me.numMarginRight.TabIndex = 7
            Me.numMarginRight.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'lblM4
            '
            Me.lblM4.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblM4.Location = New System.Drawing.Point(80, 53)
            Me.lblM4.Name = "lblM4"
            Me.lblM4.Size = New System.Drawing.Size(70, 20)
            Me.lblM4.TabIndex = 6
            Me.lblM4.Text = "راست:"
            Me.lblM4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numMarginLeft
            '
            Me.numMarginLeft.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numMarginLeft.Location = New System.Drawing.Point(15, 23)
            Me.numMarginLeft.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
            Me.numMarginLeft.Name = "numMarginLeft"
            Me.numMarginLeft.Size = New System.Drawing.Size(60, 20)
            Me.numMarginLeft.TabIndex = 5
            Me.numMarginLeft.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'lblM3
            '
            Me.lblM3.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblM3.Location = New System.Drawing.Point(80, 23)
            Me.lblM3.Name = "lblM3"
            Me.lblM3.Size = New System.Drawing.Size(70, 20)
            Me.lblM3.TabIndex = 4
            Me.lblM3.Text = "چپ:"
            Me.lblM3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numMarginBottom
            '
            Me.numMarginBottom.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numMarginBottom.Location = New System.Drawing.Point(170, 53)
            Me.numMarginBottom.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
            Me.numMarginBottom.Name = "numMarginBottom"
            Me.numMarginBottom.Size = New System.Drawing.Size(60, 20)
            Me.numMarginBottom.TabIndex = 3
            Me.numMarginBottom.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'lblM2
            '
            Me.lblM2.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblM2.Location = New System.Drawing.Point(235, 53)
            Me.lblM2.Name = "lblM2"
            Me.lblM2.Size = New System.Drawing.Size(70, 20)
            Me.lblM2.TabIndex = 2
            Me.lblM2.Text = "پایین:"
            Me.lblM2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numMarginTop
            '
            Me.numMarginTop.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numMarginTop.Location = New System.Drawing.Point(170, 23)
            Me.numMarginTop.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
            Me.numMarginTop.Name = "numMarginTop"
            Me.numMarginTop.Size = New System.Drawing.Size(60, 20)
            Me.numMarginTop.TabIndex = 1
            Me.numMarginTop.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'lblM1
            '
            Me.lblM1.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblM1.Location = New System.Drawing.Point(235, 23)
            Me.lblM1.Name = "lblM1"
            Me.lblM1.Size = New System.Drawing.Size(70, 20)
            Me.lblM1.TabIndex = 0
            Me.lblM1.Text = "بالا:"
            Me.lblM1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'gbBorder
            '
            Me.gbBorder.Controls.Add(Me.cmbPageBorder)
            Me.gbBorder.Controls.Add(Me.lblB1)
            Me.gbBorder.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Me.gbBorder.Location = New System.Drawing.Point(5, 3)
            Me.gbBorder.Name = "gbBorder"
            Me.gbBorder.Size = New System.Drawing.Size(200, 120)
            Me.gbBorder.TabIndex = 3
            Me.gbBorder.TabStop = False
            Me.gbBorder.Text = "کادر صفحه"
            '
            'cmbPageBorder
            '
            Me.cmbPageBorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPageBorder.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.cmbPageBorder.Items.AddRange(New Object() {"بدون کادر", "خط تکی نازک", "خط تکی ضخیم", "خط دوتایی"})
            Me.cmbPageBorder.Location = New System.Drawing.Point(10, 53)
            Me.cmbPageBorder.Name = "cmbPageBorder"
            Me.cmbPageBorder.Size = New System.Drawing.Size(180, 21)
            Me.cmbPageBorder.TabIndex = 1
            '
            'lblB1
            '
            Me.lblB1.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.lblB1.Location = New System.Drawing.Point(10, 23)
            Me.lblB1.Name = "lblB1"
            Me.lblB1.Size = New System.Drawing.Size(180, 20)
            Me.lblB1.TabIndex = 0
            Me.lblB1.Text = "نوع خطوط کادر دور صفحه:"
            Me.lblB1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'numColCount
            '
            Me.numColCount.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numColCount.Location = New System.Drawing.Point(15, 53)
            Me.numColCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numColCount.Name = "numColCount"
            Me.numColCount.Size = New System.Drawing.Size(110, 20)
            Me.numColCount.TabIndex = 3
            Me.numColCount.Value = New Decimal(New Integer() {10, 0, 0, 0})
            '
            'numRowCount
            '
            Me.numRowCount.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.numRowCount.Location = New System.Drawing.Point(15, 23)
            Me.numRowCount.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numRowCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numRowCount.Name = "numRowCount"
            Me.numRowCount.Size = New System.Drawing.Size(110, 20)
            Me.numRowCount.TabIndex = 1
            Me.numRowCount.Value = New Decimal(New Integer() {50, 0, 0, 0})
            '
            'pnlColumnLetters
            '
            Me.pnlColumnLetters.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlColumnLetters.Controls.Add(Me.lblColN)
            Me.pnlColumnLetters.Controls.Add(Me.lblColO)
            Me.pnlColumnLetters.Controls.Add(Me.lblColM)
            Me.pnlColumnLetters.Controls.Add(Me.lblColL)
            Me.pnlColumnLetters.Controls.Add(Me.lblColK)
            Me.pnlColumnLetters.Controls.Add(Me.lblColJ)
            Me.pnlColumnLetters.Controls.Add(Me.lblColI)
            Me.pnlColumnLetters.Controls.Add(Me.lblColH)
            Me.pnlColumnLetters.Controls.Add(Me.lblColG)
            Me.pnlColumnLetters.Controls.Add(Me.lblColF)
            Me.pnlColumnLetters.Controls.Add(Me.lblColE)
            Me.pnlColumnLetters.Controls.Add(Me.lblColD)
            Me.pnlColumnLetters.Controls.Add(Me.lblColC)
            Me.pnlColumnLetters.Controls.Add(Me.lblColB)
            Me.pnlColumnLetters.Controls.Add(Me.lblColA)
            Me.pnlColumnLetters.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlColumnLetters.Location = New System.Drawing.Point(0, 294)
            Me.pnlColumnLetters.Name = "pnlColumnLetters"
            Me.pnlColumnLetters.Size = New System.Drawing.Size(1250, 25)
            Me.pnlColumnLetters.TabIndex = 4
            '
            'lblColN
            '
            Me.lblColN.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColN.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColN.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColN.Location = New System.Drawing.Point(1100, 0)
            Me.lblColN.Name = "lblColN"
            Me.lblColN.Size = New System.Drawing.Size(100, 25)
            Me.lblColN.TabIndex = 13
            Me.lblColN.Text = "N"
            Me.lblColN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColO
            '
            Me.lblColO.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColO.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColO.Location = New System.Drawing.Point(1200, 0)
            Me.lblColO.Name = "lblColO"
            Me.lblColO.Size = New System.Drawing.Size(100, 25)
            Me.lblColO.TabIndex = 14
            Me.lblColO.Text = "O"
            Me.lblColO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColM
            '
            Me.lblColM.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColM.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColM.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColM.Location = New System.Drawing.Point(1000, 0)
            Me.lblColM.Name = "lblColM"
            Me.lblColM.Size = New System.Drawing.Size(100, 25)
            Me.lblColM.TabIndex = 12
            Me.lblColM.Text = "M"
            Me.lblColM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColL
            '
            Me.lblColL.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColL.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColL.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColL.Location = New System.Drawing.Point(900, 0)
            Me.lblColL.Name = "lblColL"
            Me.lblColL.Size = New System.Drawing.Size(100, 25)
            Me.lblColL.TabIndex = 11
            Me.lblColL.Text = "L"
            Me.lblColL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColK
            '
            Me.lblColK.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColK.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColK.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColK.Location = New System.Drawing.Point(800, 0)
            Me.lblColK.Name = "lblColK"
            Me.lblColK.Size = New System.Drawing.Size(100, 25)
            Me.lblColK.TabIndex = 10
            Me.lblColK.Text = "K"
            Me.lblColK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColJ
            '
            Me.lblColJ.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColJ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColJ.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColJ.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColJ.Location = New System.Drawing.Point(700, 0)
            Me.lblColJ.Name = "lblColJ"
            Me.lblColJ.Size = New System.Drawing.Size(100, 25)
            Me.lblColJ.TabIndex = 9
            Me.lblColJ.Text = "J"
            Me.lblColJ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColI
            '
            Me.lblColI.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColI.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColI.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColI.Location = New System.Drawing.Point(600, 0)
            Me.lblColI.Name = "lblColI"
            Me.lblColI.Size = New System.Drawing.Size(100, 25)
            Me.lblColI.TabIndex = 8
            Me.lblColI.Text = "I"
            Me.lblColI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColH
            '
            Me.lblColH.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColH.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColH.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColH.Location = New System.Drawing.Point(500, 0)
            Me.lblColH.Name = "lblColH"
            Me.lblColH.Size = New System.Drawing.Size(100, 25)
            Me.lblColH.TabIndex = 7
            Me.lblColH.Text = "H"
            Me.lblColH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColG
            '
            Me.lblColG.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColG.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColG.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColG.Location = New System.Drawing.Point(400, 0)
            Me.lblColG.Name = "lblColG"
            Me.lblColG.Size = New System.Drawing.Size(100, 25)
            Me.lblColG.TabIndex = 6
            Me.lblColG.Text = "G"
            Me.lblColG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColF
            '
            Me.lblColF.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColF.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColF.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColF.Location = New System.Drawing.Point(300, 0)
            Me.lblColF.Name = "lblColF"
            Me.lblColF.Size = New System.Drawing.Size(100, 25)
            Me.lblColF.TabIndex = 5
            Me.lblColF.Text = "F"
            Me.lblColF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColE
            '
            Me.lblColE.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColE.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColE.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColE.Location = New System.Drawing.Point(200, 0)
            Me.lblColE.Name = "lblColE"
            Me.lblColE.Size = New System.Drawing.Size(100, 25)
            Me.lblColE.TabIndex = 4
            Me.lblColE.Text = "E"
            Me.lblColE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColD
            '
            Me.lblColD.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColD.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColD.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColD.Location = New System.Drawing.Point(100, 0)
            Me.lblColD.Name = "lblColD"
            Me.lblColD.Size = New System.Drawing.Size(100, 25)
            Me.lblColD.TabIndex = 3
            Me.lblColD.Text = "D"
            Me.lblColD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColC
            '
            Me.lblColC.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColC.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColC.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColC.Location = New System.Drawing.Point(0, 0)
            Me.lblColC.Name = "lblColC"
            Me.lblColC.Size = New System.Drawing.Size(100, 25)
            Me.lblColC.TabIndex = 2
            Me.lblColC.Text = "C"
            Me.lblColC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColB
            '
            Me.lblColB.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColB.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColB.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColB.Location = New System.Drawing.Point(0, 0)
            Me.lblColB.Name = "lblColB"
            Me.lblColB.Size = New System.Drawing.Size(100, 25)
            Me.lblColB.TabIndex = 1
            Me.lblColB.Text = "B"
            Me.lblColB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColA
            '
            Me.lblColA.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblColA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblColA.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblColA.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(110, Byte), Integer))
            Me.lblColA.Location = New System.Drawing.Point(0, 0)
            Me.lblColA.Name = "lblColA"
            Me.lblColA.Size = New System.Drawing.Size(100, 25)
            Me.lblColA.TabIndex = 0
            Me.lblColA.Text = "A"
            Me.lblColA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlTitleHeader
            '
            Me.pnlTitleHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlTitleHeader.Controls.Add(Me.lblAccountTitle)
            Me.pnlTitleHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTitleHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlTitleHeader.Name = "pnlTitleHeader"
            Me.pnlTitleHeader.Size = New System.Drawing.Size(1250, 44)
            Me.pnlTitleHeader.TabIndex = 0
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.txtName)
            Me.pnlHeader.Controls.Add(Me.lblName)
            Me.pnlHeader.Controls.Add(Me.pnlHeaderSpacer)
            Me.pnlHeader.Controls.Add(Me.txtCode)
            Me.pnlHeader.Controls.Add(Me.lblCode)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 44)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(10)
            Me.pnlHeader.Size = New System.Drawing.Size(1250, 55)
            Me.pnlHeader.TabIndex = 1
            '
            'lblName
            '
            Me.lblName.Dock = System.Windows.Forms.DockStyle.Right
            Me.lblName.Location = New System.Drawing.Point(995, 10)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New System.Drawing.Size(70, 35)
            Me.lblName.TabIndex = 3
            Me.lblName.Text = "نام گزارش:"
            Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pnlHeaderSpacer
            '
            Me.pnlHeaderSpacer.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlHeaderSpacer.Location = New System.Drawing.Point(1065, 10)
            Me.pnlHeaderSpacer.Name = "pnlHeaderSpacer"
            Me.pnlHeaderSpacer.Size = New System.Drawing.Size(20, 35)
            Me.pnlHeaderSpacer.TabIndex = 2
            '
            'lblCode
            '
            Me.lblCode.Dock = System.Windows.Forms.DockStyle.Right
            Me.lblCode.Location = New System.Drawing.Point(1185, 10)
            Me.lblCode.Name = "lblCode"
            Me.lblCode.Size = New System.Drawing.Size(55, 35)
            Me.lblCode.TabIndex = 0
            Me.lblCode.Text = "کد گزارش:"
            Me.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pnlActions
            '
            Me.pnlActions.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlActions.Controls.Add(Me.btnAddToCategories)
            Me.pnlActions.Controls.Add(Me.pnlSpacing2)
            Me.pnlActions.Controls.Add(Me.btnDeleteRow)
            Me.pnlActions.Controls.Add(Me.pnlSpacing1)
            Me.pnlActions.Controls.Add(Me.btnEditHelp)
            Me.pnlActions.Controls.Add(Me.pnlSpacingEditHelp)
            Me.pnlActions.Controls.Add(Me.btnPrintReport)
            Me.pnlActions.Controls.Add(Me.pnlSpacingPrint)
            Me.pnlActions.Controls.Add(Me.btnSave)
            Me.pnlActions.Controls.Add(Me.pnlSpacing0)
            Me.pnlActions.Controls.Add(Me.btnExit)
            Me.pnlActions.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlActions.Location = New System.Drawing.Point(0, 249)
            Me.pnlActions.Name = "pnlActions"
            Me.pnlActions.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlActions.Size = New System.Drawing.Size(1250, 45)
            Me.pnlActions.TabIndex = 3
            '
            'pnlSpacing2
            '
            Me.pnlSpacing2.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSpacing2.Location = New System.Drawing.Point(540, 8)
            Me.pnlSpacing2.Name = "pnlSpacing2"
            Me.pnlSpacing2.Size = New System.Drawing.Size(10, 29)
            Me.pnlSpacing2.TabIndex = 5
            '
            'pnlSpacing1
            '
            Me.pnlSpacing1.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSpacing1.Location = New System.Drawing.Point(430, 8)
            Me.pnlSpacing1.Name = "pnlSpacing1"
            Me.pnlSpacing1.Size = New System.Drawing.Size(10, 29)
            Me.pnlSpacing1.TabIndex = 3
            '
            'pnlSpacingEditHelp
            '
            Me.pnlSpacingEditHelp.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSpacingEditHelp.Location = New System.Drawing.Point(290, 8)
            Me.pnlSpacingEditHelp.Name = "pnlSpacingEditHelp"
            Me.pnlSpacingEditHelp.Size = New System.Drawing.Size(10, 29)
            Me.pnlSpacingEditHelp.TabIndex = 9
            '
            'pnlSpacingPrint
            '
            Me.pnlSpacingPrint.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSpacingPrint.Location = New System.Drawing.Point(180, 8)
            Me.pnlSpacingPrint.Name = "pnlSpacingPrint"
            Me.pnlSpacingPrint.Size = New System.Drawing.Size(10, 29)
            Me.pnlSpacingPrint.TabIndex = 10
            '
            'pnlSpacing0
            '
            Me.pnlSpacing0.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSpacing0.Location = New System.Drawing.Point(70, 8)
            Me.pnlSpacing0.Name = "pnlSpacing0"
            Me.pnlSpacing0.Size = New System.Drawing.Size(10, 29)
            Me.pnlSpacing0.TabIndex = 1
            '
            'HesabdaryReport2Form
            '
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1250, 749)
            Me.Controls.Add(Me.dgvReports)
            Me.Controls.Add(Me.pnlColumnLetters)
            Me.Controls.Add(Me.pnlActions)
            Me.Controls.Add(Me.pnlConfig)
            Me.Controls.Add(Me.pnlHeader)
            Me.Controls.Add(Me.pnlTitleHeader)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryReport2Form"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            CType(Me.dgvReports, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlConfig.ResumeLayout(False)
            Me.gbFonts.ResumeLayout(False)
            CType(Me.numSizeFormulaDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numSizeFormula, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numSizeDetailRow, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numSizeMainRow, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numSizeHeader, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbLayout.ResumeLayout(False)
            Me.gbMargins.ResumeLayout(False)
            CType(Me.numMarginRight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numMarginLeft, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numMarginBottom, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numMarginTop, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbBorder.ResumeLayout(False)
            CType(Me.numColCount, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numRowCount, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlColumnLetters.ResumeLayout(False)
            Me.pnlTitleHeader.ResumeLayout(False)
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlActions.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents txtCode As System.Windows.Forms.TextBox
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents btnAddToCategories As System.Windows.Forms.Button
        Friend WithEvents btnDeleteRow As System.Windows.Forms.Button
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents btnPrintReport As System.Windows.Forms.Button
        Friend WithEvents btnEditHelp As System.Windows.Forms.Button
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents dgvReports As System.Windows.Forms.DataGridView
        Friend WithEvents lblAccountTitle As System.Windows.Forms.Label
        
        Friend WithEvents pnlConfig As System.Windows.Forms.Panel
        Friend WithEvents cmbFontHeader As System.Windows.Forms.ComboBox
        Friend WithEvents numSizeHeader As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbFontMainRow As System.Windows.Forms.ComboBox
        Friend WithEvents numSizeMainRow As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbFontDetailRow As System.Windows.Forms.ComboBox
        Friend WithEvents numSizeDetailRow As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbFontFormula As System.Windows.Forms.ComboBox
        Friend WithEvents numSizeFormula As System.Windows.Forms.NumericUpDown
        Friend WithEvents numRowCount As System.Windows.Forms.NumericUpDown
        Friend WithEvents numColCount As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbOrientation As System.Windows.Forms.ComboBox
        Friend WithEvents cmbPaperSize As System.Windows.Forms.ComboBox
        Friend WithEvents lblPaperSize As System.Windows.Forms.Label
        Friend WithEvents numMarginTop As System.Windows.Forms.NumericUpDown
        Friend WithEvents numMarginBottom As System.Windows.Forms.NumericUpDown
        Friend WithEvents numMarginLeft As System.Windows.Forms.NumericUpDown
        Friend WithEvents numMarginRight As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbPageBorder As System.Windows.Forms.ComboBox
        
        Friend WithEvents pnlColumnLetters As System.Windows.Forms.Panel
        Friend WithEvents lblColA As System.Windows.Forms.Label
        Friend WithEvents lblColB As System.Windows.Forms.Label
        Friend WithEvents lblColC As System.Windows.Forms.Label
        Friend WithEvents lblColD As System.Windows.Forms.Label
        Friend WithEvents lblColE As System.Windows.Forms.Label
        Friend WithEvents lblColF As System.Windows.Forms.Label
        Friend WithEvents lblColG As System.Windows.Forms.Label
        Friend WithEvents lblColH As System.Windows.Forms.Label
        Friend WithEvents lblColI As System.Windows.Forms.Label
        Friend WithEvents lblColJ As System.Windows.Forms.Label
        Friend WithEvents lblColK As System.Windows.Forms.Label
        Friend WithEvents lblColL As System.Windows.Forms.Label
        Friend WithEvents lblColM As System.Windows.Forms.Label
        Friend WithEvents lblColN As System.Windows.Forms.Label
        Friend WithEvents lblColO As System.Windows.Forms.Label
        Friend WithEvents colToggle As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colRowNo As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colCategory As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colIsMainRow As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents colRO As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSO As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colResult As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colUnderlineStyle As System.Windows.Forms.DataGridViewComboBoxColumn
        Friend WithEvents colRN As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSN As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAdd As System.Windows.Forms.DataGridViewButtonColumn
        Friend WithEvents colCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colEditFormula As System.Windows.Forms.DataGridViewButtonColumn
        Friend WithEvents colFormula As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colID As System.Windows.Forms.DataGridViewTextBoxColumn
                Friend WithEvents numSizeFormulaDetail As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmbFontFormulaDetail As System.Windows.Forms.ComboBox
        Friend WithEvents lblF5 As System.Windows.Forms.Label
        Friend WithEvents gbFonts As System.Windows.Forms.GroupBox
        Friend WithEvents lblF4 As System.Windows.Forms.Label
        Friend WithEvents lblF3 As System.Windows.Forms.Label
        Friend WithEvents lblF2 As System.Windows.Forms.Label
        Friend WithEvents lblF1 As System.Windows.Forms.Label
        Friend WithEvents gbLayout As System.Windows.Forms.GroupBox
        Friend WithEvents lblL3 As System.Windows.Forms.Label
        Friend WithEvents lblL2 As System.Windows.Forms.Label
        Friend WithEvents lblL1 As System.Windows.Forms.Label
        Friend WithEvents gbMargins As System.Windows.Forms.GroupBox
        Friend WithEvents lblM4 As System.Windows.Forms.Label
        Friend WithEvents lblM3 As System.Windows.Forms.Label
        Friend WithEvents lblM2 As System.Windows.Forms.Label
        Friend WithEvents lblM1 As System.Windows.Forms.Label
        Friend WithEvents gbBorder As System.Windows.Forms.GroupBox
        Friend WithEvents lblB1 As System.Windows.Forms.Label
        Friend WithEvents pnlTitleHeader As System.Windows.Forms.Panel
        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblName As System.Windows.Forms.Label
        Friend WithEvents pnlHeaderSpacer As System.Windows.Forms.Panel
        Friend WithEvents lblCode As System.Windows.Forms.Label
        Friend WithEvents pnlActions As System.Windows.Forms.Panel
        Friend WithEvents pnlSpacing2 As System.Windows.Forms.Panel
        Friend WithEvents pnlSpacing1 As System.Windows.Forms.Panel
        Friend WithEvents pnlSpacingEditHelp As System.Windows.Forms.Panel
        Friend WithEvents pnlSpacingPrint As System.Windows.Forms.Panel
        Friend WithEvents pnlSpacing0 As System.Windows.Forms.Panel
    End Class
End Namespace