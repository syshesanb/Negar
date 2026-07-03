Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class BankReconciliationForm
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
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.btnCompare = New System.Windows.Forms.Button()
            Me.grpColumns = New System.Windows.Forms.GroupBox()
            Me.lblColDesc = New System.Windows.Forms.Label()
            Me.cmbColDesc = New System.Windows.Forms.ComboBox()
            Me.lblColCredit = New System.Windows.Forms.Label()
            Me.cmbColCredit = New System.Windows.Forms.ComboBox()
            Me.lblColDebit = New System.Windows.Forms.Label()
            Me.cmbColDebit = New System.Windows.Forms.ComboBox()
            Me.lblColRef = New System.Windows.Forms.Label()
            Me.cmbColRef = New System.Windows.Forms.ComboBox()
            Me.lblColDate = New System.Windows.Forms.Label()
            Me.cmbColDate = New System.Windows.Forms.ComboBox()
            Me.txtToDate = New System.Windows.Forms.TextBox()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.TextBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.btnBrowse = New System.Windows.Forms.Button()
            Me.lblFilePath = New System.Windows.Forms.Label()
            Me.cmbBankAccount = New System.Windows.Forms.ComboBox()
            Me.lblBankAccount = New System.Windows.Forms.Label()
            Me.tabs = New System.Windows.Forms.TabControl()
            Me.tpBankDiscrepancies = New System.Windows.Forms.TabPage()
            Me.dgvBankDiscrepancies = New System.Windows.Forms.DataGridView()
            Me.tpLedgerDiscrepancies = New System.Windows.Forms.TabPage()
            Me.dgvLedgerDiscrepancies = New System.Windows.Forms.DataGridView()
            Me.tpMatched = New System.Windows.Forms.TabPage()
            Me.dgvMatched = New System.Windows.Forms.DataGridView()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.lblSummary = New System.Windows.Forms.Label()
            Me.btnExport = New System.Windows.Forms.Button()
            Me.pnlTop.SuspendLayout()
            Me.grpColumns.SuspendLayout()
            Me.tabs.SuspendLayout()
            Me.tpBankDiscrepancies.SuspendLayout()
            CType(Me.dgvBankDiscrepancies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpLedgerDiscrepancies.SuspendLayout()
            CType(Me.dgvLedgerDiscrepancies, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpMatched.SuspendLayout()
            CType(Me.dgvMatched, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.btnCompare)
            Me.pnlTop.Controls.Add(Me.grpColumns)
            Me.pnlTop.Controls.Add(Me.txtToDate)
            Me.pnlTop.Controls.Add(Me.lblToDate)
            Me.pnlTop.Controls.Add(Me.txtFromDate)
            Me.pnlTop.Controls.Add(Me.lblFromDate)
            Me.pnlTop.Controls.Add(Me.btnBrowse)
            Me.pnlTop.Controls.Add(Me.lblFilePath)
            Me.pnlTop.Controls.Add(Me.cmbBankAccount)
            Me.pnlTop.Controls.Add(Me.lblBankAccount)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.pnlTop.Size = New System.Drawing.Size(1100, 185)
            Me.pnlTop.TabIndex = 0
            '
            'btnCompare
            '
            Me.btnCompare.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
            Me.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCompare.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnCompare.ForeColor = System.Drawing.Color.White
            Me.btnCompare.Location = New System.Drawing.Point(16, 126)
            Me.btnCompare.Name = "btnCompare"
            Me.btnCompare.Size = New System.Drawing.Size(158, 45)
            Me.btnCompare.TabIndex = 9
            Me.btnCompare.Text = "شروع مغایرت‌گیری"
            Me.btnCompare.UseVisualStyleBackColor = False
            '
            'grpColumns
            '
            Me.grpColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpColumns.Controls.Add(Me.lblColDesc)
            Me.grpColumns.Controls.Add(Me.cmbColDesc)
            Me.grpColumns.Controls.Add(Me.lblColCredit)
            Me.grpColumns.Controls.Add(Me.cmbColCredit)
            Me.grpColumns.Controls.Add(Me.lblColDebit)
            Me.grpColumns.Controls.Add(Me.cmbColDebit)
            Me.grpColumns.Controls.Add(Me.lblColRef)
            Me.grpColumns.Controls.Add(Me.cmbColRef)
            Me.grpColumns.Controls.Add(Me.lblColDate)
            Me.grpColumns.Controls.Add(Me.cmbColDate)
            Me.grpColumns.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpColumns.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.grpColumns.Location = New System.Drawing.Point(193, 85)
            Me.grpColumns.Name = "grpColumns"
            Me.grpColumns.Size = New System.Drawing.Size(895, 90)
            Me.grpColumns.TabIndex = 8
            Me.grpColumns.TabStop = False
            Me.grpColumns.Text = "تناظر ستون‌های فایل بانکی"
            '
            'lblColDesc
            '
            Me.lblColDesc.AutoSize = True
            Me.lblColDesc.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDesc.Location = New System.Drawing.Point(125, 27)
            Me.lblColDesc.Name = "lblColDesc"
            Me.lblColDesc.Size = New System.Drawing.Size(32, 14)
            Me.lblColDesc.TabIndex = 9
            Me.lblColDesc.Text = "شرح"
            '
            'cmbColDesc
            '
            Me.cmbColDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDesc.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbColDesc.FormattingEnabled = True
            Me.cmbColDesc.Location = New System.Drawing.Point(16, 47)
            Me.cmbColDesc.Name = "cmbColDesc"
            Me.cmbColDesc.Size = New System.Drawing.Size(140, 22)
            Me.cmbColDesc.TabIndex = 8
            '
            'lblColCredit
            '
            Me.lblColCredit.AutoSize = True
            Me.lblColCredit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColCredit.Location = New System.Drawing.Point(260, 27)
            Me.lblColCredit.Name = "lblColCredit"
            Me.lblColCredit.Size = New System.Drawing.Size(86, 14)
            Me.lblColCredit.TabIndex = 7
            Me.lblColCredit.Text = "برداشت (بدهکار)"
            '
            'cmbColCredit
            '
            Me.cmbColCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColCredit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbColCredit.FormattingEnabled = True
            Me.cmbColCredit.Location = New System.Drawing.Point(200, 47)
            Me.cmbColCredit.Name = "cmbColCredit"
            Me.cmbColCredit.Size = New System.Drawing.Size(145, 22)
            Me.cmbColCredit.TabIndex = 6
            '
            'lblColDebit
            '
            Me.lblColDebit.AutoSize = True
            Me.lblColDebit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDebit.Location = New System.Drawing.Point(440, 27)
            Me.lblColDebit.Name = "lblColDebit"
            Me.lblColDebit.Size = New System.Drawing.Size(80, 14)
            Me.lblColDebit.TabIndex = 5
            Me.lblColDebit.Text = "واریز (بستانکار)"
            '
            'cmbColDebit
            '
            Me.cmbColDebit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDebit.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbColDebit.FormattingEnabled = True
            Me.cmbColDebit.Location = New System.Drawing.Point(380, 47)
            Me.cmbColDebit.Name = "cmbColDebit"
            Me.cmbColDebit.Size = New System.Drawing.Size(140, 22)
            Me.cmbColDebit.TabIndex = 4
            '
            'lblColRef
            '
            Me.lblColRef.AutoSize = True
            Me.lblColRef.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColRef.Location = New System.Drawing.Point(595, 27)
            Me.lblColRef.Name = "lblColRef"
            Me.lblColRef.Size = New System.Drawing.Size(73, 14)
            Me.lblColRef.TabIndex = 3
            Me.lblColRef.Text = "شماره پیگیری"
            '
            'cmbColRef
            '
            Me.cmbColRef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColRef.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbColRef.FormattingEnabled = True
            Me.cmbColRef.Location = New System.Drawing.Point(555, 47)
            Me.cmbColRef.Name = "cmbColRef"
            Me.cmbColRef.Size = New System.Drawing.Size(120, 22)
            Me.cmbColRef.TabIndex = 2
            '
            'lblColDate
            '
            Me.lblColDate.AutoSize = True
            Me.lblColDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblColDate.Location = New System.Drawing.Point(795, 27)
            Me.lblColDate.Name = "lblColDate"
            Me.lblColDate.Size = New System.Drawing.Size(76, 14)
            Me.lblColDate.TabIndex = 1
            Me.lblColDate.Text = "تاریخ تراکنش"
            '
            'cmbColDate
            '
            Me.cmbColDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbColDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbColDate.FormattingEnabled = True
            Me.cmbColDate.Location = New System.Drawing.Point(710, 47)
            Me.cmbColDate.Name = "cmbColDate"
            Me.cmbColDate.Size = New System.Drawing.Size(160, 22)
            Me.cmbColDate.TabIndex = 0
            '
            'txtToDate
            '
            Me.txtToDate.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtToDate.Location = New System.Drawing.Point(16, 52)
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(100, 22)
            Me.txtToDate.TabIndex = 7
            Me.txtToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblToDate
            '
            Me.lblToDate.AutoSize = True
            Me.lblToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblToDate.Location = New System.Drawing.Point(120, 56)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(40, 14)
            Me.lblToDate.TabIndex = 6
            Me.lblToDate.Text = "تا تاریخ"
            '
            'txtFromDate
            '
            Me.txtFromDate.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.txtFromDate.Location = New System.Drawing.Point(180, 52)
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(100, 22)
            Me.txtFromDate.TabIndex = 5
            Me.txtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblFromDate
            '
            Me.lblFromDate.AutoSize = True
            Me.lblFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblFromDate.Location = New System.Drawing.Point(285, 56)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(40, 14)
            Me.lblFromDate.TabIndex = 4
            Me.lblFromDate.Text = "از تاریخ"
            '
            'btnBrowse
            '
            Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
            Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBrowse.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnBrowse.ForeColor = System.Drawing.Color.White
            Me.btnBrowse.Location = New System.Drawing.Point(345, 48)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(110, 30)
            Me.btnBrowse.TabIndex = 3
            Me.btnBrowse.Text = "انتخاب فایل..."
            Me.btnBrowse.UseVisualStyleBackColor = False
            '
            'lblFilePath
            '
            Me.lblFilePath.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblFilePath.ForeColor = System.Drawing.Color.Gray
            Me.lblFilePath.Location = New System.Drawing.Point(461, 55)
            Me.lblFilePath.Name = "lblFilePath"
            Me.lblFilePath.Size = New System.Drawing.Size(262, 23)
            Me.lblFilePath.TabIndex = 2
            Me.lblFilePath.Text = "فایلی انتخاب نشده است"
            '
            'cmbBankAccount
            '
            Me.cmbBankAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmbBankAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbBankAccount.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.cmbBankAccount.FormattingEnabled = True
            Me.cmbBankAccount.Location = New System.Drawing.Point(740, 52)
            Me.cmbBankAccount.Name = "cmbBankAccount"
            Me.cmbBankAccount.Size = New System.Drawing.Size(348, 22)
            Me.cmbBankAccount.TabIndex = 1
            '
            'lblBankAccount
            '
            Me.lblBankAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblBankAccount.AutoSize = True
            Me.lblBankAccount.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblBankAccount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.lblBankAccount.Location = New System.Drawing.Point(1005, 30)
            Me.lblBankAccount.Name = "lblBankAccount"
            Me.lblBankAccount.Size = New System.Drawing.Size(83, 14)
            Me.lblBankAccount.TabIndex = 0
            Me.lblBankAccount.Text = "انتخاب بانک:"
            '
            'tabs
            '
            Me.tabs.Controls.Add(Me.tpBankDiscrepancies)
            Me.tabs.Controls.Add(Me.tpLedgerDiscrepancies)
            Me.tabs.Controls.Add(Me.tpMatched)
            Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabs.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.tabs.Location = New System.Drawing.Point(0, 185)
            Me.tabs.Name = "tabs"
            Me.tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.tabs.RightToLeftLayout = True
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New System.Drawing.Size(1100, 465)
            Me.tabs.TabIndex = 1
            '
            'tpBankDiscrepancies
            '
            Me.tpBankDiscrepancies.Controls.Add(Me.dgvBankDiscrepancies)
            Me.tpBankDiscrepancies.Location = New System.Drawing.Point(4, 23)
            Me.tpBankDiscrepancies.Name = "tpBankDiscrepancies"
            Me.tpBankDiscrepancies.Padding = New System.Windows.Forms.Padding(3)
            Me.tpBankDiscrepancies.Size = New System.Drawing.Size(1092, 438)
            Me.tpBankDiscrepancies.TabIndex = 0
            Me.tpBankDiscrepancies.Text = "اقلام باز بانکی (غایب در دفاتر)"
            Me.tpBankDiscrepancies.UseVisualStyleBackColor = True
            '
            'dgvBankDiscrepancies
            '
            Me.dgvBankDiscrepancies.AllowUserToAddRows = False
            Me.dgvBankDiscrepancies.AllowUserToDeleteRows = False
            Me.dgvBankDiscrepancies.BackgroundColor = System.Drawing.Color.White
            Me.dgvBankDiscrepancies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvBankDiscrepancies.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBankDiscrepancies.Location = New System.Drawing.Point(3, 3)
            Me.dgvBankDiscrepancies.Name = "dgvBankDiscrepancies"
            Me.dgvBankDiscrepancies.ReadOnly = True
            Me.dgvBankDiscrepancies.Size = New System.Drawing.Size(1086, 432)
            Me.dgvBankDiscrepancies.TabIndex = 0
            '
            'tpLedgerDiscrepancies
            '
            Me.tpLedgerDiscrepancies.Controls.Add(Me.dgvLedgerDiscrepancies)
            Me.tpLedgerDiscrepancies.Location = New System.Drawing.Point(4, 23)
            Me.tpLedgerDiscrepancies.Name = "tpLedgerDiscrepancies"
            Me.tpLedgerDiscrepancies.Padding = New System.Windows.Forms.Padding(3)
            Me.tpLedgerDiscrepancies.Size = New System.Drawing.Size(1092, 438)
            Me.tpLedgerDiscrepancies.TabIndex = 1
            Me.tpLedgerDiscrepancies.Text = "اقلام باز دفاتر (غایب در بانک)"
            Me.tpLedgerDiscrepancies.UseVisualStyleBackColor = True
            '
            'dgvLedgerDiscrepancies
            '
            Me.dgvLedgerDiscrepancies.AllowUserToAddRows = False
            Me.dgvLedgerDiscrepancies.AllowUserToDeleteRows = False
            Me.dgvLedgerDiscrepancies.BackgroundColor = System.Drawing.Color.White
            Me.dgvLedgerDiscrepancies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvLedgerDiscrepancies.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvLedgerDiscrepancies.Location = New System.Drawing.Point(3, 3)
            Me.dgvLedgerDiscrepancies.Name = "dgvLedgerDiscrepancies"
            Me.dgvLedgerDiscrepancies.ReadOnly = True
            Me.dgvLedgerDiscrepancies.Size = New System.Drawing.Size(1086, 432)
            Me.dgvLedgerDiscrepancies.TabIndex = 0
            '
            'tpMatched
            '
            Me.tpMatched.Controls.Add(Me.dgvMatched)
            Me.tpMatched.Location = New System.Drawing.Point(4, 23)
            Me.tpMatched.Name = "tpMatched"
            Me.tpMatched.Size = New System.Drawing.Size(1092, 438)
            Me.tpMatched.TabIndex = 2
            Me.tpMatched.Text = "اقلام تطبیق‌یافته"
            Me.tpMatched.UseVisualStyleBackColor = True
            '
            'dgvMatched
            '
            Me.dgvMatched.AllowUserToAddRows = False
            Me.dgvMatched.AllowUserToDeleteRows = False
            Me.dgvMatched.BackgroundColor = System.Drawing.Color.White
            Me.dgvMatched.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvMatched.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvMatched.Location = New System.Drawing.Point(0, 0)
            Me.dgvMatched.Name = "dgvMatched"
            Me.dgvMatched.ReadOnly = True
            Me.dgvMatched.Size = New System.Drawing.Size(1092, 438)
            Me.dgvMatched.TabIndex = 0
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(241, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.lblSummary)
            Me.pnlBottom.Controls.Add(Me.btnExport)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 650)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.pnlBottom.Size = New System.Drawing.Size(1100, 50)
            Me.pnlBottom.TabIndex = 2
            '
            'lblSummary
            '
            Me.lblSummary.AutoSize = True
            Me.lblSummary.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSummary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
            Me.lblSummary.Location = New System.Drawing.Point(234, 18)
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
            'BankReconciliationForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1100, 700)
            Me.Controls.Add(Me.tabs)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.pnlTop)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "BankReconciliationForm"
            Me.Text = "مغایرات بانکی"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            Me.grpColumns.ResumeLayout(False)
            Me.grpColumns.PerformLayout()
            Me.tabs.ResumeLayout(False)
            Me.tpBankDiscrepancies.ResumeLayout(False)
            CType(Me.dgvBankDiscrepancies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpLedgerDiscrepancies.ResumeLayout(False)
            CType(Me.dgvLedgerDiscrepancies, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpMatched.ResumeLayout(False)
            CType(Me.dgvMatched, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBottom.ResumeLayout(False)
            Me.pnlBottom.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlTop As System.Windows.Forms.Panel
        Friend WithEvents cmbBankAccount As System.Windows.Forms.ComboBox
        Friend WithEvents lblBankAccount As System.Windows.Forms.Label
        Friend WithEvents btnBrowse As System.Windows.Forms.Button
        Friend WithEvents lblFilePath As System.Windows.Forms.Label
        Friend WithEvents txtToDate As System.Windows.Forms.TextBox
        Friend WithEvents lblToDate As System.Windows.Forms.Label
        Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
        Friend WithEvents lblFromDate As System.Windows.Forms.Label
        Friend WithEvents grpColumns As System.Windows.Forms.GroupBox
        Friend WithEvents lblColDesc As System.Windows.Forms.Label
        Friend WithEvents cmbColDesc As System.Windows.Forms.ComboBox
        Friend WithEvents lblColCredit As System.Windows.Forms.Label
        Friend WithEvents cmbColCredit As System.Windows.Forms.ComboBox
        Friend WithEvents lblColDebit As System.Windows.Forms.Label
        Friend WithEvents cmbColDebit As System.Windows.Forms.ComboBox
        Friend WithEvents lblColRef As System.Windows.Forms.Label
        Friend WithEvents cmbColRef As System.Windows.Forms.ComboBox
        Friend WithEvents lblColDate As System.Windows.Forms.Label
        Friend WithEvents cmbColDate As System.Windows.Forms.ComboBox
        Friend WithEvents btnCompare As System.Windows.Forms.Button
        Friend WithEvents tabs As System.Windows.Forms.TabControl
        Friend WithEvents tpBankDiscrepancies As System.Windows.Forms.TabPage
        Friend WithEvents dgvBankDiscrepancies As System.Windows.Forms.DataGridView
        Friend WithEvents tpLedgerDiscrepancies As System.Windows.Forms.TabPage
        Friend WithEvents dgvLedgerDiscrepancies As System.Windows.Forms.DataGridView
        Friend WithEvents tpMatched As System.Windows.Forms.TabPage
        Friend WithEvents dgvMatched As System.Windows.Forms.DataGridView
        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents lblSummary As System.Windows.Forms.Label
        Friend WithEvents btnExport As System.Windows.Forms.Button
    End Class
End Namespace
