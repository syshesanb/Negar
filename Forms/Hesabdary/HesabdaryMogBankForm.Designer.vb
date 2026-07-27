Namespace Negar.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class HesabdaryMogBankForm
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
            Me.tcMain = New System.Windows.Forms.TabControl()
            Me.tpIntroBanks = New System.Windows.Forms.TabPage()
            Me.dgvBanks = New System.Windows.Forms.DataGridView()
            Me.lblBankStatementRange = New System.Windows.Forms.Label()
            Me.pnlBankInput = New System.Windows.Forms.Panel()
            Me.btnNewBank = New System.Windows.Forms.Button()
            Me.btnDeleteBank = New System.Windows.Forms.Button()
            Me.btnSaveBank = New System.Windows.Forms.Button()
            Me.btnSelectAccount = New System.Windows.Forms.Button()
            Me.lblAccountID = New System.Windows.Forms.Label()
            Me.lblAccountCodeChain = New System.Windows.Forms.Label()
            Me.lblAccountCoding = New System.Windows.Forms.Label()
            Me.txtAccountNumber = New System.Windows.Forms.TextBox()
            Me.lblAccountNumber = New System.Windows.Forms.Label()
            Me.txtAccountType = New System.Windows.Forms.TextBox()
            Me.lblAccountType = New System.Windows.Forms.Label()
            Me.txtBranchAddress = New System.Windows.Forms.TextBox()
            Me.lblBranchAddress = New System.Windows.Forms.Label()
            Me.txtBranchCode = New System.Windows.Forms.TextBox()
            Me.lblBranchCode = New System.Windows.Forms.Label()
            Me.txtBranchName = New System.Windows.Forms.TextBox()
            Me.lblBranchName = New System.Windows.Forms.Label()
            Me.txtBankName = New System.Windows.Forms.TextBox()
            Me.lblBankName = New System.Windows.Forms.Label()
            Me.tpImportStatement = New System.Windows.Forms.TabPage()
            Me.dgvImportPreview = New System.Windows.Forms.DataGridView()
            Me.pnlSearchFilters = New System.Windows.Forms.Panel()
            Me.pnlImportTop = New System.Windows.Forms.Panel()
            Me.btnSaveImport = New System.Windows.Forms.Button()
            Me.cmbColPayee = New System.Windows.Forms.ComboBox()
            Me.lblColPayee = New System.Windows.Forms.Label()
            Me.cmbColDesc = New System.Windows.Forms.ComboBox()
            Me.lblColDesc = New System.Windows.Forms.Label()
            Me.cmbColCredit = New System.Windows.Forms.ComboBox()
            Me.lblColCredit = New System.Windows.Forms.Label()
            Me.cmbColDebit = New System.Windows.Forms.ComboBox()
            Me.lblColDebit = New System.Windows.Forms.Label()
            Me.cmbColRef = New System.Windows.Forms.ComboBox()
            Me.lblColRef = New System.Windows.Forms.Label()
            Me.cmbColDate = New System.Windows.Forms.ComboBox()
            Me.lblColDate = New System.Windows.Forms.Label()
            Me.nudHeaderRow = New System.Windows.Forms.NumericUpDown()
            Me.lblHeaderRow = New System.Windows.Forms.Label()
            Me.btnBrowseFile = New System.Windows.Forms.Button()
            Me.lblImportFilePath = New System.Windows.Forms.Label()
            Me.cmbImportBank = New System.Windows.Forms.ComboBox()
            Me.lblImportBank = New System.Windows.Forms.Label()
            Me.tpReconciliation = New System.Windows.Forms.TabPage()
            Me.splitRec = New System.Windows.Forms.SplitContainer()
            Me.pnlBank = New System.Windows.Forms.Panel()
            Me.tcBank = New System.Windows.Forms.TabControl()
            Me.tpBank_All = New System.Windows.Forms.TabPage()
            Me.dgvBank_All = New System.Windows.Forms.DataGridView()
            Me.tpBank_Open = New System.Windows.Forms.TabPage()
            Me.dgvBank_Open = New System.Windows.Forms.DataGridView()
            Me.tpBank_OpenDebit = New System.Windows.Forms.TabPage()
            Me.dgvBank_OpenDebit = New System.Windows.Forms.DataGridView()
            Me.tpBank_OpenCredit = New System.Windows.Forms.TabPage()
            Me.dgvBank_OpenCredit = New System.Windows.Forms.DataGridView()
            Me.tpBank_Closed = New System.Windows.Forms.TabPage()
            Me.dgvBank_Closed = New System.Windows.Forms.DataGridView()
            Me.tpBank_ClosedDebit = New System.Windows.Forms.TabPage()
            Me.dgvBank_ClosedDebit = New System.Windows.Forms.DataGridView()
            Me.tpBank_ClosedCredit = New System.Windows.Forms.TabPage()
            Me.dgvBank_ClosedCredit = New System.Windows.Forms.DataGridView()
            Me.tpBank_Dup = New System.Windows.Forms.TabPage()
            Me.dgvBank_Dup = New System.Windows.Forms.DataGridView()
            Me.tpBank_Suggestions = New System.Windows.Forms.TabPage()
            Me.dgvBank_Suggestions = New System.Windows.Forms.DataGridView()
            Me.lblBankTitle = New System.Windows.Forms.Label()
            Me.pnlAsnad = New System.Windows.Forms.Panel()
            Me.tcAsnad = New System.Windows.Forms.TabControl()
            Me.tpAsnad_All = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_All = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_Open = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_Open = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_OpenDebit = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_OpenDebit = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_OpenCredit = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_OpenCredit = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_Closed = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_Closed = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_ClosedDebit = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_ClosedDebit = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_ClosedCredit = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_ClosedCredit = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_Dup = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_Dup = New System.Windows.Forms.DataGridView()
            Me.tpAsnad_Suggestions = New System.Windows.Forms.TabPage()
            Me.dgvAsnad_Suggestions = New System.Windows.Forms.DataGridView()
            Me.lblAsnadTitle = New System.Windows.Forms.Label()
            Me.pnlRecTop = New System.Windows.Forms.Panel()
            Me.btnRunReconciliation = New System.Windows.Forms.Button()
            Me.grpDateOptions = New System.Windows.Forms.GroupBox()
            Me.txtToDate = New System.Windows.Forms.MaskedTextBox()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.MaskedTextBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.btnFromDate = New System.Windows.Forms.Button()
            Me.btnToDate = New System.Windows.Forms.Button()
            Me.rbCustomRange = New System.Windows.Forms.RadioButton()
            Me.rbCurrentYear = New System.Windows.Forms.RadioButton()
            Me.rbAllYears = New System.Windows.Forms.RadioButton()
            Me.cmbRecBank = New System.Windows.Forms.ComboBox()
            Me.lblRecBank = New System.Windows.Forms.Label()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.lblSummary = New System.Windows.Forms.Label()
            Me.btnExport = New System.Windows.Forms.Button()
            Me.btnBankStatementReport = New System.Windows.Forms.Button()
            Me.btnTransferDesc = New System.Windows.Forms.Button()
            Me.tcMain.SuspendLayout()
            Me.tpIntroBanks.SuspendLayout()
            CType(Me.dgvBanks, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBankInput.SuspendLayout()
            Me.tpImportStatement.SuspendLayout()
            CType(Me.dgvImportPreview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlImportTop.SuspendLayout()
            CType(Me.nudHeaderRow, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpReconciliation.SuspendLayout()
            CType(Me.splitRec, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitRec.Panel1.SuspendLayout()
            Me.splitRec.Panel2.SuspendLayout()
            Me.splitRec.SuspendLayout()
            Me.pnlBank.SuspendLayout()
            Me.tcBank.SuspendLayout()
            Me.tpBank_All.SuspendLayout()
            CType(Me.dgvBank_All, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_Open.SuspendLayout()
            CType(Me.dgvBank_Open, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_OpenDebit.SuspendLayout()
            CType(Me.dgvBank_OpenDebit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_OpenCredit.SuspendLayout()
            CType(Me.dgvBank_OpenCredit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_Closed.SuspendLayout()
            CType(Me.dgvBank_Closed, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_ClosedDebit.SuspendLayout()
            CType(Me.dgvBank_ClosedDebit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_ClosedCredit.SuspendLayout()
            CType(Me.dgvBank_ClosedCredit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_Dup.SuspendLayout()
            CType(Me.dgvBank_Dup, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpBank_Suggestions.SuspendLayout()
            CType(Me.dgvBank_Suggestions, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlAsnad.SuspendLayout()
            Me.tcAsnad.SuspendLayout()
            Me.tpAsnad_All.SuspendLayout()
            CType(Me.dgvAsnad_All, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_Open.SuspendLayout()
            CType(Me.dgvAsnad_Open, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_OpenDebit.SuspendLayout()
            CType(Me.dgvAsnad_OpenDebit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_OpenCredit.SuspendLayout()
            CType(Me.dgvAsnad_OpenCredit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_Closed.SuspendLayout()
            CType(Me.dgvAsnad_Closed, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_ClosedDebit.SuspendLayout()
            CType(Me.dgvAsnad_ClosedDebit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_ClosedCredit.SuspendLayout()
            CType(Me.dgvAsnad_ClosedCredit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_Dup.SuspendLayout()
            CType(Me.dgvAsnad_Dup, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAsnad_Suggestions.SuspendLayout()
            CType(Me.dgvAsnad_Suggestions, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlRecTop.SuspendLayout()
            Me.grpDateOptions.SuspendLayout()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            'tcMain
            '
            Me.tcMain.Controls.Add(Me.tpIntroBanks)
            Me.tcMain.Controls.Add(Me.tpImportStatement)
            Me.tcMain.Controls.Add(Me.tpReconciliation)
            Me.tcMain.Controls.Add(Me.tpAsnad_Suggestions)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.RightToLeftLayout = True
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(1100, 650)
            Me.tcMain.TabIndex = 0
            '
            'tpIntroBanks
            '
            Me.tpIntroBanks.Controls.Add(Me.dgvBanks)
            Me.tpIntroBanks.Controls.Add(Me.lblBankStatementRange)
            Me.tpIntroBanks.Controls.Add(Me.pnlBankInput)
            Me.tpIntroBanks.Location = New System.Drawing.Point(4, 23)
            Me.tpIntroBanks.Name = "tpIntroBanks"
            Me.tpIntroBanks.Padding = New System.Windows.Forms.Padding(3)
            Me.tpIntroBanks.Size = New System.Drawing.Size(1092, 623)
            Me.tpIntroBanks.TabIndex = 0
            Me.tpIntroBanks.Text = "معرفی بانک‌ها"
            Me.tpIntroBanks.UseVisualStyleBackColor = True
            '
            'dgvBanks
            '
            Me.dgvBanks.AllowUserToAddRows = False
            Me.dgvBanks.AllowUserToDeleteRows = False
            Me.dgvBanks.BackgroundColor = System.Drawing.Color.White
            Me.dgvBanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBanks.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBanks.Location = New System.Drawing.Point(3, 3)
            Me.dgvBanks.MultiSelect = False
            Me.dgvBanks.Name = "dgvBanks"
            Me.dgvBanks.ReadOnly = True
            Me.dgvBanks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvBanks.Size = New System.Drawing.Size(766, 617)
            Me.dgvBanks.TabIndex = 1
            '
            'pnlBankInput
            '
            Me.pnlBankInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlBankInput.Controls.Add(Me.btnNewBank)
            Me.pnlBankInput.Controls.Add(Me.btnDeleteBank)
            Me.pnlBankInput.Controls.Add(Me.btnSaveBank)
            Me.pnlBankInput.Controls.Add(Me.btnSelectAccount)
            Me.pnlBankInput.Controls.Add(Me.lblAccountID)
            Me.pnlBankInput.Controls.Add(Me.lblAccountCodeChain)
            Me.pnlBankInput.Controls.Add(Me.lblAccountCoding)
            Me.pnlBankInput.Controls.Add(Me.txtAccountNumber)
            Me.pnlBankInput.Controls.Add(Me.lblAccountNumber)
            Me.pnlBankInput.Controls.Add(Me.txtAccountType)
            Me.pnlBankInput.Controls.Add(Me.lblAccountType)
            Me.pnlBankInput.Controls.Add(Me.txtBranchAddress)
            Me.pnlBankInput.Controls.Add(Me.lblBranchAddress)
            Me.pnlBankInput.Controls.Add(Me.txtBranchCode)
            Me.pnlBankInput.Controls.Add(Me.lblBranchCode)
            Me.pnlBankInput.Controls.Add(Me.txtBranchName)
            Me.pnlBankInput.Controls.Add(Me.lblBranchName)
            Me.pnlBankInput.Controls.Add(Me.txtBankName)
            Me.pnlBankInput.Controls.Add(Me.lblBankName)
            Me.pnlBankInput.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlBankInput.Location = New System.Drawing.Point(769, 3)
            Me.pnlBankInput.Name = "pnlBankInput"
            Me.pnlBankInput.Size = New System.Drawing.Size(320, 617)
            Me.pnlBankInput.TabIndex = 0
            '
            'btnNewBank
            '
            Me.btnNewBank.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(219, Byte), Integer))
            Me.btnNewBank.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNewBank.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnNewBank.ForeColor = System.Drawing.Color.White
            Me.btnNewBank.Location = New System.Drawing.Point(15, 487)
            Me.btnNewBank.Name = "btnNewBank"
            Me.btnNewBank.Size = New System.Drawing.Size(90, 35)
            Me.btnNewBank.TabIndex = 16
            Me.btnNewBank.Text = "جدید"
            Me.btnNewBank.UseVisualStyleBackColor = False
            '
            'btnDeleteBank
            '
            Me.btnDeleteBank.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.btnDeleteBank.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDeleteBank.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnDeleteBank.ForeColor = System.Drawing.Color.White
            Me.btnDeleteBank.Location = New System.Drawing.Point(115, 487)
            Me.btnDeleteBank.Name = "btnDeleteBank"
            Me.btnDeleteBank.Size = New System.Drawing.Size(90, 35)
            Me.btnDeleteBank.TabIndex = 15
            Me.btnDeleteBank.Text = "حذف"
            Me.btnDeleteBank.UseVisualStyleBackColor = False
            '
            'btnSaveBank
            '
            Me.btnSaveBank.BackColor = System.Drawing.Color.FromArgb(CType(CType(46, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(113, Byte), Integer))
            Me.btnSaveBank.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSaveBank.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSaveBank.ForeColor = System.Drawing.Color.White
            Me.btnSaveBank.Location = New System.Drawing.Point(215, 487)
            Me.btnSaveBank.Name = "btnSaveBank"
            Me.btnSaveBank.Size = New System.Drawing.Size(90, 35)
            Me.btnSaveBank.TabIndex = 14
            Me.btnSaveBank.Text = "ذخیره"
            Me.btnSaveBank.UseVisualStyleBackColor = False
            '
            'btnSelectAccount
            '
            Me.btnSelectAccount.BackColor = System.Drawing.Color.LightGray
            Me.btnSelectAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSelectAccount.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.btnSelectAccount.Location = New System.Drawing.Point(180, 420)
            Me.btnSelectAccount.Name = "btnSelectAccount"
            Me.btnSelectAccount.Size = New System.Drawing.Size(125, 26)
            Me.btnSelectAccount.TabIndex = 13
            Me.btnSelectAccount.Text = "انتخاب سرفصل"
            Me.btnSelectAccount.UseVisualStyleBackColor = True
            '
            'lblAccountID
            '
            Me.lblAccountID.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.lblAccountID.Location = New System.Drawing.Point(15, 424)
            Me.lblAccountID.Name = "lblAccountID"
            Me.lblAccountID.Size = New System.Drawing.Size(155, 18)
            Me.lblAccountID.TabIndex = 15
            Me.lblAccountID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lblAccountID.Visible = False
            '
            'lblAccountCodeChain
            '
            Me.lblAccountCodeChain.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblAccountCodeChain.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblAccountCodeChain.Location = New System.Drawing.Point(15, 450)
            Me.lblAccountCodeChain.Name = "lblAccountCodeChain"
            Me.lblAccountCodeChain.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.lblAccountCodeChain.Size = New System.Drawing.Size(290, 30)
            Me.lblAccountCodeChain.TabIndex = 16
            Me.lblAccountCodeChain.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblBankStatementRange
            '
            Me.lblBankStatementRange.BackColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblBankStatementRange.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblBankStatementRange.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBankStatementRange.ForeColor = System.Drawing.Color.DarkSlateBlue
            Me.lblBankStatementRange.Location = New System.Drawing.Point(3, 3)
            Me.lblBankStatementRange.Name = "lblBankStatementRange"
            Me.lblBankStatementRange.Size = New System.Drawing.Size(766, 30)
            Me.lblBankStatementRange.TabIndex = 2
            Me.lblBankStatementRange.Text = "بازه تاریخی صورت حساب وارد شده: فاقد صورت حساب وارد شده"
            Me.lblBankStatementRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.lblBankStatementRange.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'lblAccountCoding
            '
            Me.lblAccountCoding.AutoSize = True
            Me.lblAccountCoding.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblAccountCoding.Location = New System.Drawing.Point(216, 400)
            Me.lblAccountCoding.Name = "lblAccountCoding"
            Me.lblAccountCoding.Size = New System.Drawing.Size(101, 14)
            Me.lblAccountCoding.TabIndex = 12
            Me.lblAccountCoding.Text = "سرفصل حساب:"
            '
            'txtAccountNumber
            '
            Me.txtAccountNumber.Location = New System.Drawing.Point(15, 359)
            Me.txtAccountNumber.Name = "txtAccountNumber"
            Me.txtAccountNumber.Size = New System.Drawing.Size(290, 22)
            Me.txtAccountNumber.TabIndex = 11
            '
            'lblAccountNumber
            '
            Me.lblAccountNumber.AutoSize = True
            Me.lblAccountNumber.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblAccountNumber.Location = New System.Drawing.Point(215, 339)
            Me.lblAccountNumber.Name = "lblAccountNumber"
            Me.lblAccountNumber.Size = New System.Drawing.Size(91, 14)
            Me.lblAccountNumber.TabIndex = 10
            Me.lblAccountNumber.Text = "شماره حساب:"
            '
            'txtAccountType
            '
            Me.txtAccountType.Location = New System.Drawing.Point(15, 298)
            Me.txtAccountType.Name = "txtAccountType"
            Me.txtAccountType.Size = New System.Drawing.Size(290, 22)
            Me.txtAccountType.TabIndex = 9
            '
            'lblAccountType
            '
            Me.lblAccountType.AutoSize = True
            Me.lblAccountType.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblAccountType.Location = New System.Drawing.Point(234, 278)
            Me.lblAccountType.Name = "lblAccountType"
            Me.lblAccountType.Size = New System.Drawing.Size(72, 14)
            Me.lblAccountType.TabIndex = 8
            Me.lblAccountType.Text = "نوع حساب:"
            '
            'txtBranchAddress
            '
            Me.txtBranchAddress.Location = New System.Drawing.Point(15, 198)
            Me.txtBranchAddress.Multiline = True
            Me.txtBranchAddress.Name = "txtBranchAddress"
            Me.txtBranchAddress.Size = New System.Drawing.Size(290, 60)
            Me.txtBranchAddress.TabIndex = 7
            '
            'lblBranchAddress
            '
            Me.lblBranchAddress.AutoSize = True
            Me.lblBranchAddress.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBranchAddress.Location = New System.Drawing.Point(220, 178)
            Me.lblBranchAddress.Name = "lblBranchAddress"
            Me.lblBranchAddress.Size = New System.Drawing.Size(82, 14)
            Me.lblBranchAddress.TabIndex = 6
            Me.lblBranchAddress.Text = "آدرس شعبه:"
            '
            'txtBranchCode
            '
            Me.txtBranchCode.Location = New System.Drawing.Point(15, 137)
            Me.txtBranchCode.Name = "txtBranchCode"
            Me.txtBranchCode.Size = New System.Drawing.Size(290, 22)
            Me.txtBranchCode.TabIndex = 5
            '
            'lblBranchCode
            '
            Me.lblBranchCode.AutoSize = True
            Me.lblBranchCode.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBranchCode.Location = New System.Drawing.Point(239, 117)
            Me.lblBranchCode.Name = "lblBranchCode"
            Me.lblBranchCode.Size = New System.Drawing.Size(62, 14)
            Me.lblBranchCode.TabIndex = 4
            Me.lblBranchCode.Text = "کد شعبه:"
            '
            'txtBranchName
            '
            Me.txtBranchName.Location = New System.Drawing.Point(15, 82)
            Me.txtBranchName.Name = "txtBranchName"
            Me.txtBranchName.Size = New System.Drawing.Size(290, 22)
            Me.txtBranchName.TabIndex = 3
            '
            'lblBranchName
            '
            Me.lblBranchName.AutoSize = True
            Me.lblBranchName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBranchName.Location = New System.Drawing.Point(239, 62)
            Me.lblBranchName.Name = "lblBranchName"
            Me.lblBranchName.Size = New System.Drawing.Size(64, 14)
            Me.lblBranchName.TabIndex = 2
            Me.lblBranchName.Text = "نام شعبه:"
            '
            'txtBankName
            '
            Me.txtBankName.Location = New System.Drawing.Point(15, 30)
            Me.txtBankName.Name = "txtBankName"
            Me.txtBankName.Size = New System.Drawing.Size(290, 22)
            Me.txtBankName.TabIndex = 1
            '
            'lblBankName
            '
            Me.lblBankName.AutoSize = True
            Me.lblBankName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBankName.Location = New System.Drawing.Point(248, 10)
            Me.lblBankName.Name = "lblBankName"
            Me.lblBankName.Size = New System.Drawing.Size(56, 14)
            Me.lblBankName.TabIndex = 0
            Me.lblBankName.Text = "نام بانک:"
            '
            'tpImportStatement
            '
            Me.tpImportStatement.Controls.Add(Me.dgvImportPreview)
            Me.tpImportStatement.Controls.Add(Me.pnlSearchFilters)
            Me.tpImportStatement.Controls.Add(Me.pnlImportTop)
            Me.tpImportStatement.Location = New System.Drawing.Point(4, 23)
            Me.tpImportStatement.Name = "tpImportStatement"
            Me.tpImportStatement.Padding = New System.Windows.Forms.Padding(3)
            Me.tpImportStatement.Size = New System.Drawing.Size(1092, 623)
            Me.tpImportStatement.TabIndex = 1
            Me.tpImportStatement.Text = "ورود صورتحساب بانک"
            Me.tpImportStatement.UseVisualStyleBackColor = True
            '
            'dgvImportPreview
            '
            Me.dgvImportPreview.AllowUserToAddRows = False
            Me.dgvImportPreview.AllowUserToDeleteRows = False
            Me.dgvImportPreview.BackgroundColor = System.Drawing.Color.White
            Me.dgvImportPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvImportPreview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvImportPreview.Location = New System.Drawing.Point(3, 171)
            Me.dgvImportPreview.Name = "dgvImportPreview"
            Me.dgvImportPreview.ReadOnly = True
            Me.dgvImportPreview.Size = New System.Drawing.Size(1086, 449)
            Me.dgvImportPreview.TabIndex = 1
            '
            'pnlSearchFilters
            '
            Me.pnlSearchFilters.BackColor = System.Drawing.Color.White
            Me.pnlSearchFilters.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSearchFilters.Location = New System.Drawing.Point(3, 143)
            Me.pnlSearchFilters.Name = "pnlSearchFilters"
            Me.pnlSearchFilters.Size = New System.Drawing.Size(1086, 28)
            Me.pnlSearchFilters.TabIndex = 2
            '
            'pnlImportTop
            '
            Me.pnlImportTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlImportTop.Controls.Add(Me.btnSaveImport)
            Me.pnlImportTop.Controls.Add(Me.cmbColPayee)
            Me.pnlImportTop.Controls.Add(Me.lblColPayee)
            Me.pnlImportTop.Controls.Add(Me.cmbColDesc)
            Me.pnlImportTop.Controls.Add(Me.lblColDesc)
            Me.pnlImportTop.Controls.Add(Me.cmbColCredit)
            Me.pnlImportTop.Controls.Add(Me.lblColCredit)
            Me.pnlImportTop.Controls.Add(Me.cmbColDebit)
            Me.pnlImportTop.Controls.Add(Me.lblColDebit)
            Me.pnlImportTop.Controls.Add(Me.cmbColRef)
            Me.pnlImportTop.Controls.Add(Me.lblColRef)
            Me.pnlImportTop.Controls.Add(Me.cmbColDate)
            Me.pnlImportTop.Controls.Add(Me.lblColDate)
            Me.pnlImportTop.Controls.Add(Me.nudHeaderRow)
            Me.pnlImportTop.Controls.Add(Me.lblHeaderRow)
            Me.pnlImportTop.Controls.Add(Me.btnBrowseFile)
            Me.pnlImportTop.Controls.Add(Me.lblImportFilePath)
            Me.pnlImportTop.Controls.Add(Me.cmbImportBank)
            Me.pnlImportTop.Controls.Add(Me.lblImportBank)
            Me.pnlImportTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlImportTop.Location = New System.Drawing.Point(3, 3)
            Me.pnlImportTop.Name = "pnlImportTop"
            Me.pnlImportTop.Size = New System.Drawing.Size(1086, 140)
            Me.pnlImportTop.TabIndex = 0
            '
            'btnSaveImport
            '
            Me.btnSaveImport.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(174, Byte), Integer), CType(CType(96, Byte), Integer))
            Me.btnSaveImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSaveImport.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSaveImport.ForeColor = System.Drawing.Color.White
            Me.btnSaveImport.Location = New System.Drawing.Point(15, 80)
            Me.btnSaveImport.Name = "btnSaveImport"
            Me.btnSaveImport.Size = New System.Drawing.Size(120, 45)
            Me.btnSaveImport.TabIndex = 16
            Me.btnSaveImport.Text = "ذخیره اطلاعات"
            Me.btnSaveImport.UseVisualStyleBackColor = False
            '
            'cmbColPayee
            '
            Me.cmbColPayee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColPayee.FormattingEnabled = True
            Me.cmbColPayee.Location = New System.Drawing.Point(165, 95)
            Me.cmbColPayee.Name = "cmbColPayee"
            Me.cmbColPayee.Size = New System.Drawing.Size(130, 22)
            Me.cmbColPayee.TabIndex = 15
            '
            'lblColPayee
            '
            Me.lblColPayee.AutoSize = True
            Me.lblColPayee.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColPayee.Location = New System.Drawing.Point(165, 78)
            Me.lblColPayee.Name = "lblColPayee"
            Me.lblColPayee.Size = New System.Drawing.Size(97, 14)
            Me.lblColPayee.TabIndex = 14
            Me.lblColPayee.Text = "واریز کننده / ذینفع"
            '
            'cmbColDesc
            '
            Me.cmbColDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDesc.FormattingEnabled = True
            Me.cmbColDesc.Location = New System.Drawing.Point(315, 95)
            Me.cmbColDesc.Name = "cmbColDesc"
            Me.cmbColDesc.Size = New System.Drawing.Size(130, 22)
            Me.cmbColDesc.TabIndex = 13
            '
            'lblColDesc
            '
            Me.lblColDesc.AutoSize = True
            Me.lblColDesc.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDesc.Location = New System.Drawing.Point(315, 78)
            Me.lblColDesc.Name = "lblColDesc"
            Me.lblColDesc.Size = New System.Drawing.Size(31, 14)
            Me.lblColDesc.TabIndex = 12
            Me.lblColDesc.Text = "شرح"
            '
            'cmbColCredit
            '
            Me.cmbColCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColCredit.FormattingEnabled = True
            Me.cmbColCredit.Location = New System.Drawing.Point(465, 95)
            Me.cmbColCredit.Name = "cmbColCredit"
            Me.cmbColCredit.Size = New System.Drawing.Size(130, 22)
            Me.cmbColCredit.TabIndex = 11
            '
            'lblColCredit
            '
            Me.lblColCredit.AutoSize = True
            Me.lblColCredit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColCredit.Location = New System.Drawing.Point(465, 78)
            Me.lblColCredit.Name = "lblColCredit"
            Me.lblColCredit.Size = New System.Drawing.Size(93, 14)
            Me.lblColCredit.TabIndex = 10
            Me.lblColCredit.Text = "برداشت (بدهکار)"
            '
            'cmbColDebit
            '
            Me.cmbColDebit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDebit.FormattingEnabled = True
            Me.cmbColDebit.Location = New System.Drawing.Point(615, 95)
            Me.cmbColDebit.Name = "cmbColDebit"
            Me.cmbColDebit.Size = New System.Drawing.Size(130, 22)
            Me.cmbColDebit.TabIndex = 9
            '
            'lblColDebit
            '
            Me.lblColDebit.AutoSize = True
            Me.lblColDebit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDebit.Location = New System.Drawing.Point(615, 78)
            Me.lblColDebit.Name = "lblColDebit"
            Me.lblColDebit.Size = New System.Drawing.Size(82, 14)
            Me.lblColDebit.TabIndex = 8
            Me.lblColDebit.Text = "واریز (بستانکار)"
            '
            'cmbColRef
            '
            Me.cmbColRef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColRef.FormattingEnabled = True
            Me.cmbColRef.Location = New System.Drawing.Point(765, 95)
            Me.cmbColRef.Name = "cmbColRef"
            Me.cmbColRef.Size = New System.Drawing.Size(130, 22)
            Me.cmbColRef.TabIndex = 7
            '
            'lblColRef
            '
            Me.lblColRef.AutoSize = True
            Me.lblColRef.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColRef.Location = New System.Drawing.Point(765, 78)
            Me.lblColRef.Name = "lblColRef"
            Me.lblColRef.Size = New System.Drawing.Size(76, 14)
            Me.lblColRef.TabIndex = 6
            Me.lblColRef.Text = "شماره پیگیری"
            '
            'cmbColDate
            '
            Me.cmbColDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDate.FormattingEnabled = True
            Me.cmbColDate.Location = New System.Drawing.Point(915, 95)
            Me.cmbColDate.Name = "cmbColDate"
            Me.cmbColDate.Size = New System.Drawing.Size(130, 22)
            Me.cmbColDate.TabIndex = 5
            '
            'lblColDate
            '
            Me.lblColDate.AutoSize = True
            Me.lblColDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDate.Location = New System.Drawing.Point(915, 78)
            Me.lblColDate.Name = "lblColDate"
            Me.lblColDate.Size = New System.Drawing.Size(70, 14)
            Me.lblColDate.TabIndex = 4
            Me.lblColDate.Text = "تاریخ تراکنش"
            '
            'nudHeaderRow
            '
            Me.nudHeaderRow.Location = New System.Drawing.Point(155, 43)
            Me.nudHeaderRow.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudHeaderRow.Name = "nudHeaderRow"
            Me.nudHeaderRow.Size = New System.Drawing.Size(70, 22)
            Me.nudHeaderRow.TabIndex = 5
            Me.nudHeaderRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudHeaderRow.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblHeaderRow
            '
            Me.lblHeaderRow.AutoSize = True
            Me.lblHeaderRow.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblHeaderRow.Location = New System.Drawing.Point(135, 20)
            Me.lblHeaderRow.Name = "lblHeaderRow"
            Me.lblHeaderRow.Size = New System.Drawing.Size(97, 14)
            Me.lblHeaderRow.TabIndex = 4
            Me.lblHeaderRow.Text = "ردیف سرستون‌ها:"
            '
            'btnBrowseFile
            '
            Me.btnBrowseFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
            Me.btnBrowseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBrowseFile.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnBrowseFile.ForeColor = System.Drawing.Color.White
            Me.btnBrowseFile.Location = New System.Drawing.Point(245, 40)
            Me.btnBrowseFile.Name = "btnBrowseFile"
            Me.btnBrowseFile.Size = New System.Drawing.Size(120, 26)
            Me.btnBrowseFile.TabIndex = 3
            Me.btnBrowseFile.Text = "انتخاب فایل..."
            Me.btnBrowseFile.UseVisualStyleBackColor = False
            '
            'lblImportFilePath
            '
            Me.lblImportFilePath.ForeColor = System.Drawing.Color.Gray
            Me.lblImportFilePath.Location = New System.Drawing.Point(380, 43)
            Me.lblImportFilePath.Name = "lblImportFilePath"
            Me.lblImportFilePath.Size = New System.Drawing.Size(350, 20)
            Me.lblImportFilePath.TabIndex = 2
            Me.lblImportFilePath.Text = "فایلی انتخاب نشده است"
            Me.lblImportFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lblImportFilePath.RightToLeft = System.Windows.Forms.RightToLeft.No
            '
            'cmbImportBank
            '
            Me.cmbImportBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbImportBank.FormattingEnabled = True
            Me.cmbImportBank.Location = New System.Drawing.Point(745, 40)
            Me.cmbImportBank.Name = "cmbImportBank"
            Me.cmbImportBank.Size = New System.Drawing.Size(240, 22)
            Me.cmbImportBank.TabIndex = 1
            '
            'lblImportBank
            '
            Me.lblImportBank.AutoSize = True
            Me.lblImportBank.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblImportBank.Location = New System.Drawing.Point(994, 20)
            Me.lblImportBank.Name = "lblImportBank"
            Me.lblImportBank.Size = New System.Drawing.Size(77, 14)
            Me.lblImportBank.TabIndex = 0
            Me.lblImportBank.Text = "انتخاب بانک:"
            '
            'tpReconciliation
            '
            Me.tpReconciliation.Controls.Add(Me.splitRec)
            Me.tpReconciliation.Controls.Add(Me.pnlRecTop)
            Me.tpReconciliation.Location = New System.Drawing.Point(4, 23)
            Me.tpReconciliation.Name = "tpReconciliation"
            Me.tpReconciliation.Size = New System.Drawing.Size(1092, 623)
            Me.tpReconciliation.TabIndex = 2
            Me.tpReconciliation.Text = "مغایرت گیری"
            Me.tpReconciliation.UseVisualStyleBackColor = True
            '
            'splitRec
            '
            Me.splitRec.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitRec.Location = New System.Drawing.Point(0, 90)
            Me.splitRec.Name = "splitRec"
            Me.splitRec.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitRec.Panel1
            '
            Me.splitRec.Panel1.Controls.Add(Me.pnlBank)
            Me.splitRec.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'splitRec.Panel2
            '
            Me.splitRec.Panel2.Controls.Add(Me.pnlAsnad)
            Me.splitRec.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.splitRec.Size = New System.Drawing.Size(1092, 533)
            Me.splitRec.SplitterDistance = 260
            Me.splitRec.TabIndex = 1
            '
            'pnlBank
            '
            Me.pnlBank.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlBank.Controls.Add(Me.tcBank)
            Me.pnlBank.Controls.Add(Me.lblBankTitle)
            Me.pnlBank.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBank.Location = New System.Drawing.Point(0, 0)
            Me.pnlBank.Name = "pnlBank"
            Me.pnlBank.Size = New System.Drawing.Size(1092, 260)
            Me.pnlBank.TabIndex = 0
            '
            'tcBank
            '
            Me.tcBank.Controls.Add(Me.tpBank_All)
            Me.tcBank.Controls.Add(Me.tpBank_Open)
            Me.tcBank.Controls.Add(Me.tpBank_OpenDebit)
            Me.tcBank.Controls.Add(Me.tpBank_OpenCredit)
            Me.tcBank.Controls.Add(Me.tpBank_Closed)
            Me.tcBank.Controls.Add(Me.tpBank_ClosedDebit)
            Me.tcBank.Controls.Add(Me.tpBank_ClosedCredit)
            Me.tcBank.Controls.Add(Me.tpBank_Dup)
            Me.tcBank.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcBank.Location = New System.Drawing.Point(0, 20)
            Me.tcBank.Name = "tcBank"
            Me.tcBank.RightToLeftLayout = True
            Me.tcBank.SelectedIndex = 0
            Me.tcBank.Size = New System.Drawing.Size(1090, 238)
            Me.tcBank.TabIndex = 1
            '
            'tpBank_All
            '
            Me.tpBank_All.Controls.Add(Me.dgvBank_All)
            Me.tpBank_All.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_All.Name = "tpBank_All"
            Me.tpBank_All.Padding = New System.Windows.Forms.Padding(3)
            Me.tpBank_All.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_All.TabIndex = 0
            Me.tpBank_All.Text = "کل ارقام بانک"
            Me.tpBank_All.UseVisualStyleBackColor = True
            '
            'dgvBank_All
            '
            Me.dgvBank_All.AllowUserToAddRows = False
            Me.dgvBank_All.AllowUserToDeleteRows = False
            Me.dgvBank_All.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_All.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_All.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_All.Location = New System.Drawing.Point(3, 3)
            Me.dgvBank_All.Name = "dgvBank_All"
            Me.dgvBank_All.ReadOnly = True
            Me.dgvBank_All.Size = New System.Drawing.Size(1076, 205)
            Me.dgvBank_All.TabIndex = 0
            '
            'tpBank_Open
            '
            Me.tpBank_Open.Controls.Add(Me.dgvBank_Open)
            Me.tpBank_Open.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_Open.Name = "tpBank_Open"
            Me.tpBank_Open.Padding = New System.Windows.Forms.Padding(3)
            Me.tpBank_Open.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_Open.TabIndex = 1
            Me.tpBank_Open.Text = "کل ارقام باز بانک"
            Me.tpBank_Open.UseVisualStyleBackColor = True
            '
            'dgvBank_Open
            '
            Me.dgvBank_Open.AllowUserToAddRows = False
            Me.dgvBank_Open.AllowUserToDeleteRows = False
            Me.dgvBank_Open.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_Open.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_Open.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_Open.Location = New System.Drawing.Point(3, 3)
            Me.dgvBank_Open.Name = "dgvBank_Open"
            Me.dgvBank_Open.ReadOnly = True
            Me.dgvBank_Open.Size = New System.Drawing.Size(1076, 205)
            Me.dgvBank_Open.TabIndex = 0
            '
            'tpBank_OpenDebit
            '
            Me.tpBank_OpenDebit.Controls.Add(Me.dgvBank_OpenDebit)
            Me.tpBank_OpenDebit.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_OpenDebit.Name = "tpBank_OpenDebit"
            Me.tpBank_OpenDebit.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_OpenDebit.TabIndex = 2
            Me.tpBank_OpenDebit.Text = "ارقام باز بدهکار بانک"
            Me.tpBank_OpenDebit.UseVisualStyleBackColor = True
            '
            'dgvBank_OpenDebit
            '
            Me.dgvBank_OpenDebit.AllowUserToAddRows = False
            Me.dgvBank_OpenDebit.AllowUserToDeleteRows = False
            Me.dgvBank_OpenDebit.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_OpenDebit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_OpenDebit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_OpenDebit.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_OpenDebit.Name = "dgvBank_OpenDebit"
            Me.dgvBank_OpenDebit.ReadOnly = True
            Me.dgvBank_OpenDebit.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_OpenDebit.TabIndex = 0
            '
            'tpBank_OpenCredit
            '
            Me.tpBank_OpenCredit.Controls.Add(Me.dgvBank_OpenCredit)
            Me.tpBank_OpenCredit.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_OpenCredit.Name = "tpBank_OpenCredit"
            Me.tpBank_OpenCredit.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_OpenCredit.TabIndex = 3
            Me.tpBank_OpenCredit.Text = "ارقام باز بستانکار بانک"
            Me.tpBank_OpenCredit.UseVisualStyleBackColor = True
            '
            'dgvBank_OpenCredit
            '
            Me.dgvBank_OpenCredit.AllowUserToAddRows = False
            Me.dgvBank_OpenCredit.AllowUserToDeleteRows = False
            Me.dgvBank_OpenCredit.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_OpenCredit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_OpenCredit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_OpenCredit.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_OpenCredit.Name = "dgvBank_OpenCredit"
            Me.dgvBank_OpenCredit.ReadOnly = True
            Me.dgvBank_OpenCredit.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_OpenCredit.TabIndex = 0
            '
            'tpBank_Closed
            '
            Me.tpBank_Closed.Controls.Add(Me.dgvBank_Closed)
            Me.tpBank_Closed.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_Closed.Name = "tpBank_Closed"
            Me.tpBank_Closed.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_Closed.TabIndex = 4
            Me.tpBank_Closed.Text = "کل ارقام بسته بانک"
            Me.tpBank_Closed.UseVisualStyleBackColor = True
            '
            'dgvBank_Closed
            '
            Me.dgvBank_Closed.AllowUserToAddRows = False
            Me.dgvBank_Closed.AllowUserToDeleteRows = False
            Me.dgvBank_Closed.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_Closed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_Closed.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_Closed.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_Closed.Name = "dgvBank_Closed"
            Me.dgvBank_Closed.ReadOnly = True
            Me.dgvBank_Closed.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_Closed.TabIndex = 0
            '
            'tpBank_ClosedDebit
            '
            Me.tpBank_ClosedDebit.Controls.Add(Me.dgvBank_ClosedDebit)
            Me.tpBank_ClosedDebit.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_ClosedDebit.Name = "tpBank_ClosedDebit"
            Me.tpBank_ClosedDebit.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_ClosedDebit.TabIndex = 5
            Me.tpBank_ClosedDebit.Text = "ارقام بسته بدهکار بانک"
            Me.tpBank_ClosedDebit.UseVisualStyleBackColor = True
            '
            'dgvBank_ClosedDebit
            '
            Me.dgvBank_ClosedDebit.AllowUserToAddRows = False
            Me.dgvBank_ClosedDebit.AllowUserToDeleteRows = False
            Me.dgvBank_ClosedDebit.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_ClosedDebit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_ClosedDebit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_ClosedDebit.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_ClosedDebit.Name = "dgvBank_ClosedDebit"
            Me.dgvBank_ClosedDebit.ReadOnly = True
            Me.dgvBank_ClosedDebit.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_ClosedDebit.TabIndex = 0
            '
            'tpBank_ClosedCredit
            '
            Me.tpBank_ClosedCredit.Controls.Add(Me.dgvBank_ClosedCredit)
            Me.tpBank_ClosedCredit.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_ClosedCredit.Name = "tpBank_ClosedCredit"
            Me.tpBank_ClosedCredit.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_ClosedCredit.TabIndex = 6
            Me.tpBank_ClosedCredit.Text = "ارقام بسته بستانکار بانک"
            Me.tpBank_ClosedCredit.UseVisualStyleBackColor = True
            '
            'dgvBank_ClosedCredit
            '
            Me.dgvBank_ClosedCredit.AllowUserToAddRows = False
            Me.dgvBank_ClosedCredit.AllowUserToDeleteRows = False
            Me.dgvBank_ClosedCredit.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_ClosedCredit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_ClosedCredit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_ClosedCredit.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_ClosedCredit.Name = "dgvBank_ClosedCredit"
            Me.dgvBank_ClosedCredit.ReadOnly = True
            Me.dgvBank_ClosedCredit.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_ClosedCredit.TabIndex = 0
            '
            'tpBank_Dup
            '
            Me.tpBank_Dup.Controls.Add(Me.dgvBank_Dup)
            Me.tpBank_Dup.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_Dup.Name = "tpBank_Dup"
            Me.tpBank_Dup.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_Dup.TabIndex = 7
            Me.tpBank_Dup.Text = "ارقام تکراری در بانک"
            Me.tpBank_Dup.UseVisualStyleBackColor = True
            '
            'dgvBank_Dup
            '
            Me.dgvBank_Dup.AllowUserToAddRows = False
            Me.dgvBank_Dup.AllowUserToDeleteRows = False
            Me.dgvBank_Dup.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_Dup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_Dup.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_Dup.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_Dup.Name = "dgvBank_Dup"
            Me.dgvBank_Dup.ReadOnly = True
            Me.dgvBank_Dup.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_Dup.TabIndex = 0
            '
            'tpBank_Suggestions
            '
            Me.tpBank_Suggestions.Controls.Add(Me.dgvBank_Suggestions)
            Me.tpBank_Suggestions.Location = New System.Drawing.Point(4, 23)
            Me.tpBank_Suggestions.Name = "tpBank_Suggestions"
            Me.tpBank_Suggestions.Size = New System.Drawing.Size(1082, 211)
            Me.tpBank_Suggestions.TabIndex = 8
            Me.tpBank_Suggestions.Text = "پیشنهاد برای رفع مغایرت بانک"
            Me.tpBank_Suggestions.UseVisualStyleBackColor = True
            '
            'dgvBank_Suggestions
            '
            Me.dgvBank_Suggestions.AllowUserToAddRows = False
            Me.dgvBank_Suggestions.AllowUserToDeleteRows = False
            Me.dgvBank_Suggestions.BackgroundColor = System.Drawing.Color.White
            Me.dgvBank_Suggestions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBank_Suggestions.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBank_Suggestions.Location = New System.Drawing.Point(0, 0)
            Me.dgvBank_Suggestions.Name = "dgvBank_Suggestions"
            Me.dgvBank_Suggestions.ReadOnly = True
            Me.dgvBank_Suggestions.Size = New System.Drawing.Size(1082, 211)
            Me.dgvBank_Suggestions.TabIndex = 0
            '
            'lblBankTitle
            '
            Me.lblBankTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.lblBankTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblBankTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBankTitle.ForeColor = System.Drawing.Color.White
            Me.lblBankTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblBankTitle.Name = "lblBankTitle"
            Me.lblBankTitle.Size = New System.Drawing.Size(1090, 20)
            Me.lblBankTitle.TabIndex = 0
            Me.lblBankTitle.Text = "اطلاعات صورت‌حساب بانک (pnlBank)"
            Me.lblBankTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlAsnad
            '
            Me.pnlAsnad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlAsnad.Controls.Add(Me.tcAsnad)
            Me.pnlAsnad.Controls.Add(Me.lblAsnadTitle)
            Me.pnlAsnad.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlAsnad.Location = New System.Drawing.Point(0, 0)
            Me.pnlAsnad.Name = "pnlAsnad"
            Me.pnlAsnad.Size = New System.Drawing.Size(1092, 269)
            Me.pnlAsnad.TabIndex = 0
            '
            'tcAsnad
            '
            Me.tcAsnad.Controls.Add(Me.tpAsnad_All)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_Open)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_OpenDebit)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_OpenCredit)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_Closed)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_ClosedDebit)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_ClosedCredit)
            Me.tcAsnad.Controls.Add(Me.tpAsnad_Dup)
            Me.tcAsnad.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcAsnad.Location = New System.Drawing.Point(0, 20)
            Me.tcAsnad.Name = "tcAsnad"
            Me.tcAsnad.RightToLeftLayout = True
            Me.tcAsnad.SelectedIndex = 0
            Me.tcAsnad.Size = New System.Drawing.Size(1090, 247)
            Me.tcAsnad.TabIndex = 1
            '
            'tpAsnad_All
            '
            Me.tpAsnad_All.Controls.Add(Me.dgvAsnad_All)
            Me.tpAsnad_All.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_All.Name = "tpAsnad_All"
            Me.tpAsnad_All.Padding = New System.Windows.Forms.Padding(3)
            Me.tpAsnad_All.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_All.TabIndex = 0
            Me.tpAsnad_All.Text = "کل ارقام دفتر"
            Me.tpAsnad_All.UseVisualStyleBackColor = True
            '
            'dgvAsnad_All
            '
            Me.dgvAsnad_All.AllowUserToAddRows = False
            Me.dgvAsnad_All.AllowUserToDeleteRows = False
            Me.dgvAsnad_All.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_All.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_All.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_All.Location = New System.Drawing.Point(3, 3)
            Me.dgvAsnad_All.Name = "dgvAsnad_All"
            Me.dgvAsnad_All.ReadOnly = True
            Me.dgvAsnad_All.Size = New System.Drawing.Size(1076, 214)
            Me.dgvAsnad_All.TabIndex = 0
            '
            'tpAsnad_Open
            '
            Me.tpAsnad_Open.Controls.Add(Me.dgvAsnad_Open)
            Me.tpAsnad_Open.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_Open.Name = "tpAsnad_Open"
            Me.tpAsnad_Open.Padding = New System.Windows.Forms.Padding(3)
            Me.tpAsnad_Open.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_Open.TabIndex = 1
            Me.tpAsnad_Open.Text = "کل ارقام باز دفتر"
            Me.tpAsnad_Open.UseVisualStyleBackColor = True
            '
            'dgvAsnad_Open
            '
            Me.dgvAsnad_Open.AllowUserToAddRows = False
            Me.dgvAsnad_Open.AllowUserToDeleteRows = False
            Me.dgvAsnad_Open.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_Open.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_Open.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_Open.Location = New System.Drawing.Point(3, 3)
            Me.dgvAsnad_Open.Name = "dgvAsnad_Open"
            Me.dgvAsnad_Open.ReadOnly = True
            Me.dgvAsnad_Open.Size = New System.Drawing.Size(1076, 214)
            Me.dgvAsnad_Open.TabIndex = 0
            '
            'tpAsnad_OpenDebit
            '
            Me.tpAsnad_OpenDebit.Controls.Add(Me.dgvAsnad_OpenDebit)
            Me.tpAsnad_OpenDebit.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_OpenDebit.Name = "tpAsnad_OpenDebit"
            Me.tpAsnad_OpenDebit.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_OpenDebit.TabIndex = 2
            Me.tpAsnad_OpenDebit.Text = "ارقام باز بدهکار دفتر"
            Me.tpAsnad_OpenDebit.UseVisualStyleBackColor = True
            '
            'dgvAsnad_OpenDebit
            '
            Me.dgvAsnad_OpenDebit.AllowUserToAddRows = False
            Me.dgvAsnad_OpenDebit.AllowUserToDeleteRows = False
            Me.dgvAsnad_OpenDebit.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_OpenDebit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_OpenDebit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_OpenDebit.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_OpenDebit.Name = "dgvAsnad_OpenDebit"
            Me.dgvAsnad_OpenDebit.ReadOnly = True
            Me.dgvAsnad_OpenDebit.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_OpenDebit.TabIndex = 0
            '
            'tpAsnad_OpenCredit
            '
            Me.tpAsnad_OpenCredit.Controls.Add(Me.dgvAsnad_OpenCredit)
            Me.tpAsnad_OpenCredit.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_OpenCredit.Name = "tpAsnad_OpenCredit"
            Me.tpAsnad_OpenCredit.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_OpenCredit.TabIndex = 3
            Me.tpAsnad_OpenCredit.Text = "ارقام باز بستانکار دفتر"
            Me.tpAsnad_OpenCredit.UseVisualStyleBackColor = True
            '
            'dgvAsnad_OpenCredit
            '
            Me.dgvAsnad_OpenCredit.AllowUserToAddRows = False
            Me.dgvAsnad_OpenCredit.AllowUserToDeleteRows = False
            Me.dgvAsnad_OpenCredit.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_OpenCredit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_OpenCredit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_OpenCredit.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_OpenCredit.Name = "dgvAsnad_OpenCredit"
            Me.dgvAsnad_OpenCredit.ReadOnly = True
            Me.dgvAsnad_OpenCredit.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_OpenCredit.TabIndex = 0
            '
            'tpAsnad_Closed
            '
            Me.tpAsnad_Closed.Controls.Add(Me.dgvAsnad_Closed)
            Me.tpAsnad_Closed.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_Closed.Name = "tpAsnad_Closed"
            Me.tpAsnad_Closed.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_Closed.TabIndex = 4
            Me.tpAsnad_Closed.Text = "کل ارقام بسته دفتر"
            Me.tpAsnad_Closed.UseVisualStyleBackColor = True
            '
            'dgvAsnad_Closed
            '
            Me.dgvAsnad_Closed.AllowUserToAddRows = False
            Me.dgvAsnad_Closed.AllowUserToDeleteRows = False
            Me.dgvAsnad_Closed.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_Closed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_Closed.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_Closed.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_Closed.Name = "dgvAsnad_Closed"
            Me.dgvAsnad_Closed.ReadOnly = True
            Me.dgvAsnad_Closed.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_Closed.TabIndex = 0
            '
            'tpAsnad_ClosedDebit
            '
            Me.tpAsnad_ClosedDebit.Controls.Add(Me.dgvAsnad_ClosedDebit)
            Me.tpAsnad_ClosedDebit.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_ClosedDebit.Name = "tpAsnad_ClosedDebit"
            Me.tpAsnad_ClosedDebit.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_ClosedDebit.TabIndex = 5
            Me.tpAsnad_ClosedDebit.Text = "ارقام بسته بدهکار دفتر"
            Me.tpAsnad_ClosedDebit.UseVisualStyleBackColor = True
            '
            'dgvAsnad_ClosedDebit
            '
            Me.dgvAsnad_ClosedDebit.AllowUserToAddRows = False
            Me.dgvAsnad_ClosedDebit.AllowUserToDeleteRows = False
            Me.dgvAsnad_ClosedDebit.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_ClosedDebit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_ClosedDebit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_ClosedDebit.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_ClosedDebit.Name = "dgvAsnad_ClosedDebit"
            Me.dgvAsnad_ClosedDebit.ReadOnly = True
            Me.dgvAsnad_ClosedDebit.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_ClosedDebit.TabIndex = 0
            '
            'tpAsnad_ClosedCredit
            '
            Me.tpAsnad_ClosedCredit.Controls.Add(Me.dgvAsnad_ClosedCredit)
            Me.tpAsnad_ClosedCredit.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_ClosedCredit.Name = "tpAsnad_ClosedCredit"
            Me.tpAsnad_ClosedCredit.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_ClosedCredit.TabIndex = 6
            Me.tpAsnad_ClosedCredit.Text = "ارقام بسته بستانکار دفتر"
            Me.tpAsnad_ClosedCredit.UseVisualStyleBackColor = True
            '
            'dgvAsnad_ClosedCredit
            '
            Me.dgvAsnad_ClosedCredit.AllowUserToAddRows = False
            Me.dgvAsnad_ClosedCredit.AllowUserToDeleteRows = False
            Me.dgvAsnad_ClosedCredit.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_ClosedCredit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_ClosedCredit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_ClosedCredit.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_ClosedCredit.Name = "dgvAsnad_ClosedCredit"
            Me.dgvAsnad_ClosedCredit.ReadOnly = True
            Me.dgvAsnad_ClosedCredit.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_ClosedCredit.TabIndex = 0
            '
            'tpAsnad_Dup
            '
            Me.tpAsnad_Dup.Controls.Add(Me.dgvAsnad_Dup)
            Me.tpAsnad_Dup.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_Dup.Name = "tpAsnad_Dup"
            Me.tpAsnad_Dup.Size = New System.Drawing.Size(1082, 220)
            Me.tpAsnad_Dup.TabIndex = 7
            Me.tpAsnad_Dup.Text = "ارقام تکراری دفتر"
            Me.tpAsnad_Dup.UseVisualStyleBackColor = True
            '
            'dgvAsnad_Dup
            '
            Me.dgvAsnad_Dup.AllowUserToAddRows = False
            Me.dgvAsnad_Dup.AllowUserToDeleteRows = False
            Me.dgvAsnad_Dup.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_Dup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_Dup.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_Dup.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_Dup.Name = "dgvAsnad_Dup"
            Me.dgvAsnad_Dup.ReadOnly = True
            Me.dgvAsnad_Dup.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_Dup.TabIndex = 0
            '
            'tpAsnad_Suggestions
            '
            Me.tpAsnad_Suggestions.Controls.Add(Me.dgvAsnad_Suggestions)
            Me.tpAsnad_Suggestions.Location = New System.Drawing.Point(4, 23)
            Me.tpAsnad_Suggestions.Name = "tpAsnad_Suggestions"
            Me.tpAsnad_Suggestions.Size = New System.Drawing.Size(1092, 623)
            Me.tpAsnad_Suggestions.TabIndex = 3
            Me.tpAsnad_Suggestions.Text = "پیشنهاد برای رفع مغایرت"
            Me.tpAsnad_Suggestions.UseVisualStyleBackColor = True
            '
            'dgvAsnad_Suggestions
            '
            Me.dgvAsnad_Suggestions.AllowUserToAddRows = False
            Me.dgvAsnad_Suggestions.AllowUserToDeleteRows = False
            Me.dgvAsnad_Suggestions.BackgroundColor = System.Drawing.Color.White
            Me.dgvAsnad_Suggestions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAsnad_Suggestions.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAsnad_Suggestions.Location = New System.Drawing.Point(0, 0)
            Me.dgvAsnad_Suggestions.Name = "dgvAsnad_Suggestions"
            Me.dgvAsnad_Suggestions.ReadOnly = True
            Me.dgvAsnad_Suggestions.Size = New System.Drawing.Size(1082, 220)
            Me.dgvAsnad_Suggestions.TabIndex = 0
            '
            'lblAsnadTitle
            '
            Me.lblAsnadTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.lblAsnadTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblAsnadTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblAsnadTitle.ForeColor = System.Drawing.Color.White
            Me.lblAsnadTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblAsnadTitle.Name = "lblAsnadTitle"
            Me.lblAsnadTitle.Size = New System.Drawing.Size(1090, 20)
            Me.lblAsnadTitle.TabIndex = 0
            Me.lblAsnadTitle.Text = "اطلاعات دفتر کل بانک (pnlAsnad)"
            Me.lblAsnadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlRecTop
            '
            Me.pnlRecTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlRecTop.Controls.Add(Me.btnRunReconciliation)
            Me.pnlRecTop.Controls.Add(Me.grpDateOptions)
            Me.pnlRecTop.Controls.Add(Me.cmbRecBank)
            Me.pnlRecTop.Controls.Add(Me.lblRecBank)
            Me.pnlRecTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlRecTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlRecTop.Name = "pnlRecTop"
            Me.pnlRecTop.Size = New System.Drawing.Size(1092, 90)
            Me.pnlRecTop.TabIndex = 0
            '
            'btnRunReconciliation
            '
            Me.btnRunReconciliation.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
            Me.btnRunReconciliation.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnRunReconciliation.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnRunReconciliation.ForeColor = System.Drawing.Color.White
            Me.btnRunReconciliation.Location = New System.Drawing.Point(16, 20)
            Me.btnRunReconciliation.Name = "btnRunReconciliation"
            Me.btnRunReconciliation.Size = New System.Drawing.Size(134, 50)
            Me.btnRunReconciliation.TabIndex = 3
            Me.btnRunReconciliation.Text = "تهیه مغایرت بانکی"
            Me.btnRunReconciliation.UseVisualStyleBackColor = False
            '
            'grpDateOptions
            '
            Me.grpDateOptions.Controls.Add(Me.btnToDate)
            Me.grpDateOptions.Controls.Add(Me.txtToDate)
            Me.grpDateOptions.Controls.Add(Me.lblToDate)
            Me.grpDateOptions.Controls.Add(Me.btnFromDate)
            Me.grpDateOptions.Controls.Add(Me.txtFromDate)
            Me.grpDateOptions.Controls.Add(Me.lblFromDate)
            Me.grpDateOptions.Controls.Add(Me.rbCustomRange)
            Me.grpDateOptions.Controls.Add(Me.rbCurrentYear)
            Me.grpDateOptions.Controls.Add(Me.rbAllYears)
            Me.grpDateOptions.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.grpDateOptions.Location = New System.Drawing.Point(169, 6)
            Me.grpDateOptions.Name = "grpDateOptions"
            Me.grpDateOptions.Size = New System.Drawing.Size(621, 75)
            Me.grpDateOptions.TabIndex = 2
            Me.grpDateOptions.TabStop = False
            Me.grpDateOptions.Text = "بازه تاریخ مغایرت‌گیری"
            '
            'btnToDate
            '
            Me.btnToDate.Enabled = False
            Me.btnToDate.Location = New System.Drawing.Point(4, 32)
            Me.btnToDate.Name = "btnToDate"
            Me.btnToDate.Size = New System.Drawing.Size(28, 23)
            Me.btnToDate.TabIndex = 7
            Me.btnToDate.Text = "..."
            '
            'txtToDate
            '
            Me.txtToDate.Enabled = False
            Me.txtToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtToDate.Location = New System.Drawing.Point(36, 33)
            Me.txtToDate.Mask = "0000/00/00"
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(80, 21)
            Me.txtToDate.TabIndex = 6
            Me.txtToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblToDate
            '
            Me.lblToDate.AutoSize = True
            Me.lblToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblToDate.Location = New System.Drawing.Point(120, 36)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(44, 14)
            Me.lblToDate.TabIndex = 5
            Me.lblToDate.Text = "تا تاریخ:"
            '
            'btnFromDate
            '
            Me.btnFromDate.Enabled = False
            Me.btnFromDate.Location = New System.Drawing.Point(169, 32)
            Me.btnFromDate.Name = "btnFromDate"
            Me.btnFromDate.Size = New System.Drawing.Size(28, 23)
            Me.btnFromDate.TabIndex = 5
            Me.btnFromDate.Text = "..."
            '
            'txtFromDate
            '
            Me.txtFromDate.Enabled = False
            Me.txtFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtFromDate.Location = New System.Drawing.Point(201, 33)
            Me.txtFromDate.Mask = "0000/00/00"
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(80, 21)
            Me.txtFromDate.TabIndex = 4
            Me.txtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblFromDate
            '
            Me.lblFromDate.AutoSize = True
            Me.lblFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblFromDate.Location = New System.Drawing.Point(285, 36)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(44, 14)
            Me.lblFromDate.TabIndex = 3
            Me.lblFromDate.Text = "از تاریخ:"
            '
            'rbCustomRange
            '
            Me.rbCustomRange.AutoSize = True
            Me.rbCustomRange.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.rbCustomRange.Location = New System.Drawing.Point(335, 33)
            Me.rbCustomRange.Name = "rbCustomRange"
            Me.rbCustomRange.Size = New System.Drawing.Size(83, 18)
            Me.rbCustomRange.TabIndex = 2
            Me.rbCustomRange.Text = "بازه تاریخ از:"
            Me.rbCustomRange.UseVisualStyleBackColor = True
            '
            'rbCurrentYear
            '
            Me.rbCurrentYear.AutoSize = True
            Me.rbCurrentYear.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.rbCurrentYear.Location = New System.Drawing.Point(430, 33)
            Me.rbCurrentYear.Name = "rbCurrentYear"
            Me.rbCurrentYear.Size = New System.Drawing.Size(79, 18)
            Me.rbCurrentYear.TabIndex = 1
            Me.rbCurrentYear.Text = "سال جاری"
            Me.rbCurrentYear.UseVisualStyleBackColor = True
            '
            'rbAllYears
            '
            Me.rbAllYears.AutoSize = True
            Me.rbAllYears.Checked = True
            Me.rbAllYears.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.rbAllYears.Location = New System.Drawing.Point(515, 33)
            Me.rbAllYears.Name = "rbAllYears"
            Me.rbAllYears.Size = New System.Drawing.Size(86, 18)
            Me.rbAllYears.TabIndex = 0
            Me.rbAllYears.TabStop = True
            Me.rbAllYears.Text = "تمام سال‌ها"
            Me.rbAllYears.UseVisualStyleBackColor = True
            '
            'cmbRecBank
            '
            Me.cmbRecBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbRecBank.FormattingEnabled = True
            Me.cmbRecBank.Location = New System.Drawing.Point(805, 40)
            Me.cmbRecBank.Name = "cmbRecBank"
            Me.cmbRecBank.Size = New System.Drawing.Size(200, 22)
            Me.cmbRecBank.TabIndex = 1
            '
            'lblRecBank
            '
            Me.lblRecBank.AutoSize = True
            Me.lblRecBank.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblRecBank.Location = New System.Drawing.Point(1011, 43)
            Me.lblRecBank.Name = "lblRecBank"
            Me.lblRecBank.Size = New System.Drawing.Size(77, 14)
            Me.lblRecBank.TabIndex = 0
            Me.lblRecBank.Text = "انتخاب بانک:"
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(241, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.lblSummary)
            Me.pnlBottom.Controls.Add(Me.btnExport)
            Me.pnlBottom.Controls.Add(Me.btnBankStatementReport)
            Me.pnlBottom.Controls.Add(Me.btnTransferDesc)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 650)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.pnlBottom.Size = New System.Drawing.Size(1100, 50)
            Me.pnlBottom.TabIndex = 1
            '
            'lblSummary
            '
            Me.lblSummary.AutoSize = True
            Me.lblSummary.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSummary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.lblSummary.Location = New System.Drawing.Point(700, 18)
            Me.lblSummary.Name = "lblSummary"
            Me.lblSummary.Size = New System.Drawing.Size(0, 14)
            Me.lblSummary.TabIndex = 1
            '
            'btnExport
            '
            Me.btnExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(174, Byte), Integer), CType(CType(96, Byte), Integer))
            Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnExport.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnExport.ForeColor = System.Drawing.Color.White
            Me.btnExport.Location = New System.Drawing.Point(16, 10)
            Me.btnExport.Name = "btnExport"
            Me.btnExport.Size = New System.Drawing.Size(200, 30)
            Me.btnExport.TabIndex = 0
            Me.btnExport.Text = "خروجی اکسل اقلام مغایرت..."
            Me.btnExport.UseVisualStyleBackColor = False
            '
            'btnBankStatementReport
            '
            Me.btnBankStatementReport.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(219, Byte), Integer))
            Me.btnBankStatementReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBankStatementReport.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnBankStatementReport.ForeColor = System.Drawing.Color.White
            Me.btnBankStatementReport.Location = New System.Drawing.Point(226, 10)
            Me.btnBankStatementReport.Name = "btnBankStatementReport"
            Me.btnBankStatementReport.Size = New System.Drawing.Size(200, 30)
            Me.btnBankStatementReport.TabIndex = 2
            Me.btnBankStatementReport.Text = "گزارش صورتحساب بانکی"
            Me.btnBankStatementReport.UseVisualStyleBackColor = False
            '
            'btnTransferDesc
            '
            Me.btnTransferDesc.BackColor = System.Drawing.Color.FromArgb(CType(CType(142, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(173, Byte), Integer))
            Me.btnTransferDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnTransferDesc.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnTransferDesc.ForeColor = System.Drawing.Color.White
            Me.btnTransferDesc.Location = New System.Drawing.Point(436, 10)
            Me.btnTransferDesc.Name = "btnTransferDesc"
            Me.btnTransferDesc.Size = New System.Drawing.Size(250, 30)
            Me.btnTransferDesc.TabIndex = 3
            Me.btnTransferDesc.Text = "انتقال شرح صورت حساب به شرح ردیف دفتر"
            Me.btnTransferDesc.UseVisualStyleBackColor = False
            '
            'HesabdaryMogBankForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1100, 700)
            Me.Controls.Add(Me.tcMain)
            Me.Controls.Add(Me.pnlBottom)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "HesabdaryMogBankForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Text = "مغایرات بانکی"
            Me.tcMain.ResumeLayout(False)
            Me.tpIntroBanks.ResumeLayout(False)
            CType(Me.dgvBanks, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBankInput.ResumeLayout(False)
            Me.pnlBankInput.PerformLayout()
            Me.tpImportStatement.ResumeLayout(False)
            CType(Me.dgvImportPreview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlImportTop.ResumeLayout(False)
            Me.pnlImportTop.PerformLayout()
            CType(Me.nudHeaderRow, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpReconciliation.ResumeLayout(False)
            Me.splitRec.Panel1.ResumeLayout(False)
            Me.splitRec.Panel2.ResumeLayout(False)
            CType(Me.splitRec, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitRec.ResumeLayout(False)
            Me.pnlBank.ResumeLayout(False)
            Me.tcBank.ResumeLayout(False)
            Me.tpBank_All.ResumeLayout(False)
            CType(Me.dgvBank_All, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_Open.ResumeLayout(False)
            CType(Me.dgvBank_Open, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_OpenDebit.ResumeLayout(False)
            CType(Me.dgvBank_OpenDebit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_OpenCredit.ResumeLayout(False)
            CType(Me.dgvBank_OpenCredit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_Closed.ResumeLayout(False)
            CType(Me.dgvBank_Closed, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_ClosedDebit.ResumeLayout(False)
            CType(Me.dgvBank_ClosedDebit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_ClosedCredit.ResumeLayout(False)
            CType(Me.dgvBank_ClosedCredit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_Dup.ResumeLayout(False)
            CType(Me.dgvBank_Dup, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpBank_Suggestions.ResumeLayout(False)
            CType(Me.dgvBank_Suggestions, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlAsnad.ResumeLayout(False)
            Me.tcAsnad.ResumeLayout(False)
            Me.tpAsnad_All.ResumeLayout(False)
            CType(Me.dgvAsnad_All, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_Open.ResumeLayout(False)
            CType(Me.dgvAsnad_Open, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_OpenDebit.ResumeLayout(False)
            CType(Me.dgvAsnad_OpenDebit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_OpenCredit.ResumeLayout(False)
            CType(Me.dgvAsnad_OpenCredit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_Closed.ResumeLayout(False)
            CType(Me.dgvAsnad_Closed, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_ClosedDebit.ResumeLayout(False)
            CType(Me.dgvAsnad_ClosedDebit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_ClosedCredit.ResumeLayout(False)
            CType(Me.dgvAsnad_ClosedCredit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_Dup.ResumeLayout(False)
            CType(Me.dgvAsnad_Dup, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAsnad_Suggestions.ResumeLayout(False)
            CType(Me.dgvAsnad_Suggestions, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlRecTop.ResumeLayout(False)
            Me.pnlRecTop.PerformLayout()
            Me.grpDateOptions.ResumeLayout(False)
            Me.grpDateOptions.PerformLayout()
            Me.pnlBottom.ResumeLayout(False)
            Me.pnlBottom.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents tcMain As System.Windows.Forms.TabControl
        Friend WithEvents tpIntroBanks As System.Windows.Forms.TabPage
        Friend WithEvents tpImportStatement As System.Windows.Forms.TabPage
        Friend WithEvents tpReconciliation As System.Windows.Forms.TabPage
        Friend WithEvents pnlBankInput As System.Windows.Forms.Panel
        Friend WithEvents txtBankName As System.Windows.Forms.TextBox
        Friend WithEvents lblBankName As System.Windows.Forms.Label
        Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
        Friend WithEvents lblBranchName As System.Windows.Forms.Label
        Friend WithEvents txtBranchCode As System.Windows.Forms.TextBox
        Friend WithEvents lblBranchCode As System.Windows.Forms.Label
        Friend WithEvents txtBranchAddress As System.Windows.Forms.TextBox
        Friend WithEvents lblBranchAddress As System.Windows.Forms.Label
        Friend WithEvents txtAccountType As System.Windows.Forms.TextBox
        Friend WithEvents lblAccountType As System.Windows.Forms.Label
        Friend WithEvents txtAccountNumber As System.Windows.Forms.TextBox
        Friend WithEvents lblAccountNumber As System.Windows.Forms.Label
        Friend WithEvents btnSelectAccount As System.Windows.Forms.Button
        Friend WithEvents lblAccountID As System.Windows.Forms.Label
        Friend WithEvents lblAccountCodeChain As System.Windows.Forms.Label
        Friend WithEvents lblBankStatementRange As System.Windows.Forms.Label
        Friend WithEvents lblAccountCoding As System.Windows.Forms.Label
        Friend WithEvents btnSaveBank As System.Windows.Forms.Button
        Friend WithEvents btnDeleteBank As System.Windows.Forms.Button
        Friend WithEvents btnNewBank As System.Windows.Forms.Button
        Friend WithEvents dgvBanks As System.Windows.Forms.DataGridView

        Friend WithEvents pnlImportTop As System.Windows.Forms.Panel
        Friend WithEvents cmbImportBank As System.Windows.Forms.ComboBox
        Friend WithEvents lblImportBank As System.Windows.Forms.Label
        Friend WithEvents btnBrowseFile As System.Windows.Forms.Button
        Friend WithEvents lblImportFilePath As System.Windows.Forms.Label
        Friend WithEvents nudHeaderRow As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblHeaderRow As System.Windows.Forms.Label
        Friend WithEvents cmbColDate As System.Windows.Forms.ComboBox
        Friend WithEvents lblColDate As System.Windows.Forms.Label
        Friend WithEvents cmbColRef As System.Windows.Forms.ComboBox
        Friend WithEvents lblColRef As System.Windows.Forms.Label
        Friend WithEvents cmbColDebit As System.Windows.Forms.ComboBox
        Friend WithEvents lblColDebit As System.Windows.Forms.Label
        Friend WithEvents cmbColCredit As System.Windows.Forms.ComboBox
        Friend WithEvents lblColCredit As System.Windows.Forms.Label
        Friend WithEvents cmbColDesc As System.Windows.Forms.ComboBox
        Friend WithEvents lblColDesc As System.Windows.Forms.Label
        Friend WithEvents cmbColPayee As System.Windows.Forms.ComboBox
        Friend WithEvents lblColPayee As System.Windows.Forms.Label
        Friend WithEvents btnSaveImport As System.Windows.Forms.Button
        Friend WithEvents dgvImportPreview As System.Windows.Forms.DataGridView
        Friend WithEvents pnlSearchFilters As System.Windows.Forms.Panel

        Friend WithEvents splitRec As System.Windows.Forms.SplitContainer
        Friend WithEvents pnlBank As System.Windows.Forms.Panel
        Friend WithEvents lblBankTitle As System.Windows.Forms.Label
        Friend WithEvents tcBank As System.Windows.Forms.TabControl
        Friend WithEvents tpBank_All As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_All As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_Open As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_Open As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_OpenDebit As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_OpenDebit As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_OpenCredit As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_OpenCredit As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_Closed As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_Closed As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_ClosedDebit As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_ClosedDebit As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_ClosedCredit As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_ClosedCredit As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_Dup As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_Dup As System.Windows.Forms.DataGridView
        Friend WithEvents tpBank_Suggestions As System.Windows.Forms.TabPage
        Friend WithEvents dgvBank_Suggestions As System.Windows.Forms.DataGridView

        Friend WithEvents pnlAsnad As System.Windows.Forms.Panel
        Friend WithEvents lblAsnadTitle As System.Windows.Forms.Label
        Friend WithEvents tcAsnad As System.Windows.Forms.TabControl
        Friend WithEvents tpAsnad_All As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_All As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_Open As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_Open As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_OpenDebit As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_OpenDebit As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_OpenCredit As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_OpenCredit As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_Closed As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_Closed As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_ClosedDebit As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_ClosedDebit As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_ClosedCredit As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_ClosedCredit As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_Dup As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_Dup As System.Windows.Forms.DataGridView
        Friend WithEvents tpAsnad_Suggestions As System.Windows.Forms.TabPage
        Friend WithEvents dgvAsnad_Suggestions As System.Windows.Forms.DataGridView

        Friend WithEvents pnlRecTop As System.Windows.Forms.Panel
        Friend WithEvents cmbRecBank As System.Windows.Forms.ComboBox
        Friend WithEvents lblRecBank As System.Windows.Forms.Label
        Friend WithEvents grpDateOptions As System.Windows.Forms.GroupBox
        Friend WithEvents rbAllYears As System.Windows.Forms.RadioButton
        Friend WithEvents rbCurrentYear As System.Windows.Forms.RadioButton
        Friend WithEvents rbCustomRange As System.Windows.Forms.RadioButton
        Friend WithEvents txtFromDate As System.Windows.Forms.MaskedTextBox
        Friend WithEvents lblFromDate As System.Windows.Forms.Label
        Friend WithEvents txtToDate As System.Windows.Forms.MaskedTextBox
        Friend WithEvents lblToDate As System.Windows.Forms.Label
        Friend WithEvents btnFromDate As System.Windows.Forms.Button
        Friend WithEvents btnToDate As System.Windows.Forms.Button
        Friend WithEvents btnRunReconciliation As System.Windows.Forms.Button

        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents lblSummary As System.Windows.Forms.Label
        Friend WithEvents btnExport As System.Windows.Forms.Button
        Friend WithEvents btnBankStatementReport As System.Windows.Forms.Button
        Friend WithEvents btnTransferDesc As System.Windows.Forms.Button
    End Class
End Namespace
