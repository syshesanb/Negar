Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class HesabdaryTabdilDataSanadForm
        Inherits Form

        Private components As IContainer

        ' â”€â”€ Sanad2 Controls â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Friend WithEvents grpSanad2 As GroupBox

        Friend WithEvents pnlSanad2Top As Panel
        Friend WithEvents lblSanad2File As Label
        Friend WithEvents btnSanad2SelectFile As Button
        Friend WithEvents lblSanad2HeaderRow As Label
        Friend WithEvents nudSanad2HeaderRow As NumericUpDown
        Friend WithEvents btnSanad2Preview As Button
        Friend WithEvents lblSanad2RecordCount As Label

        Friend WithEvents pnlSanad2Mapping As Panel
        Friend WithEvents lblSanad2ShomareSanad As Label
        Friend WithEvents cmbSanad2ShomareSanad As ComboBox
        Friend WithEvents lblSanad2Goruh As Label
        Friend WithEvents cmbSanad2Goruh As ComboBox
        Friend WithEvents lblSanad2Kol As Label
        Friend WithEvents cmbSanad2Kol As ComboBox
        Friend WithEvents lblSanad2Moein As Label
        Friend WithEvents cmbSanad2Moein As ComboBox
        Friend WithEvents lblSanad2Tafsili1 As Label
        Friend WithEvents cmbSanad2Tafsili1 As ComboBox
        Friend WithEvents lblSanad2Tafsili2 As Label
        Friend WithEvents cmbSanad2Tafsili2 As ComboBox
        Friend WithEvents lblSanad2Tafsili3 As Label
        Friend WithEvents cmbSanad2Tafsili3 As ComboBox
        Friend WithEvents lblSanad2Bedehkar As Label
        Friend WithEvents cmbSanad2Bedehkar As ComboBox
        Friend WithEvents lblSanad2Bestankar As Label
        Friend WithEvents cmbSanad2Bestankar As ComboBox
        Friend WithEvents lblSanad2TxNum As Label
        Friend WithEvents cmbSanad2TxNum As ComboBox
        Friend WithEvents lblSanad2TxDate As Label
        Friend WithEvents cmbSanad2TxDate As ComboBox
        Friend WithEvents lblSanad2SharhRadif As Label
        Friend WithEvents cmbSanad2SharhRadif As ComboBox
        Friend WithEvents lblSanad2TarikSanad As Label
        Friend WithEvents cmbSanad2TarikSanad As ComboBox


        Friend WithEvents pnlDgvSanad2 As Panel
        Friend WithEvents dgvSanad2Preview As DataGridView

        ' â”€â”€ Sanad1 Controls â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Friend WithEvents grpSanad1 As GroupBox

        Friend WithEvents pnlSanad1Top As Panel
        Friend WithEvents lblSanad1File As Label
        Friend WithEvents btnSanad1SelectFile As Button
        Friend WithEvents lblSanad1HeaderRow As Label
        Friend WithEvents nudSanad1HeaderRow As NumericUpDown
        Friend WithEvents btnSanad1Preview As Button
        Friend WithEvents lblSanad1RecordCount As Label

        Friend WithEvents pnlSanad1Mapping As Panel
        Friend WithEvents lblSanad1ShomareSanad As Label
        Friend WithEvents cmbSanad1ShomareSanad As ComboBox
        Friend WithEvents lblSanad1TarikSanad As Label
        Friend WithEvents cmbSanad1TarikSanad As ComboBox
        Friend WithEvents lblSanad1Sharh As Label
        Friend WithEvents cmbSanad1Sharh As ComboBox

        Friend WithEvents pnlDgvSanad1 As Panel
        Friend WithEvents dgvSanad1Preview As DataGridView

        ' â”€â”€ Splitter â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Friend WithEvents splitterMain As Splitter

        ' â”€â”€ Bottom Panel & Combined Convert Button â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Friend WithEvents btnConvertBoth As Button



        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.grpSanad2 = New System.Windows.Forms.GroupBox()
            Me.pnlDgvSanad2 = New System.Windows.Forms.Panel()
            Me.dgvSanad2Preview = New System.Windows.Forms.DataGridView()
            Me.pnlSanad2Mapping = New System.Windows.Forms.Panel()
            Me.cmbSanad2TxDate = New System.Windows.Forms.ComboBox()
            Me.lblSanad2TxDate = New System.Windows.Forms.Label()
            Me.cmbSanad2TxNum = New System.Windows.Forms.ComboBox()
            Me.lblSanad2TxNum = New System.Windows.Forms.Label()
            Me.cmbSanad2Bestankar = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Bestankar = New System.Windows.Forms.Label()
            Me.cmbSanad2Bedehkar = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Bedehkar = New System.Windows.Forms.Label()
            Me.cmbSanad2SharhRadif = New System.Windows.Forms.ComboBox()
            Me.lblSanad2SharhRadif = New System.Windows.Forms.Label()
            Me.cmbSanad2TarikSanad = New System.Windows.Forms.ComboBox()
            Me.lblSanad2TarikSanad = New System.Windows.Forms.Label()
            Me.cmbSanad2Tafsili3 = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Tafsili3 = New System.Windows.Forms.Label()
            Me.cmbSanad2Tafsili2 = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Tafsili2 = New System.Windows.Forms.Label()
            Me.cmbSanad2Tafsili1 = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Tafsili1 = New System.Windows.Forms.Label()
            Me.cmbSanad2Moein = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Moein = New System.Windows.Forms.Label()
            Me.cmbSanad2Kol = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Kol = New System.Windows.Forms.Label()
            Me.cmbSanad2Goruh = New System.Windows.Forms.ComboBox()
            Me.lblSanad2Goruh = New System.Windows.Forms.Label()
            Me.cmbSanad2ShomareSanad = New System.Windows.Forms.ComboBox()
            Me.lblSanad2ShomareSanad = New System.Windows.Forms.Label()
            Me.pnlSanad2Top = New System.Windows.Forms.Panel()
            Me.btnConvertBoth = New System.Windows.Forms.Button()
            Me.lblSanad2RecordCount = New System.Windows.Forms.Label()
            Me.btnSanad2Preview = New System.Windows.Forms.Button()
            Me.nudSanad2HeaderRow = New System.Windows.Forms.NumericUpDown()
            Me.lblSanad2HeaderRow = New System.Windows.Forms.Label()
            Me.btnSanad2SelectFile = New System.Windows.Forms.Button()
            Me.lblSanad2File = New System.Windows.Forms.Label()
            Me.grpSanad1 = New System.Windows.Forms.GroupBox()
            Me.pnlDgvSanad1 = New System.Windows.Forms.Panel()
            Me.dgvSanad1Preview = New System.Windows.Forms.DataGridView()
            Me.pnlSanad1Mapping = New System.Windows.Forms.Panel()
            Me.cmbSanad1Sharh = New System.Windows.Forms.ComboBox()
            Me.lblSanad1Sharh = New System.Windows.Forms.Label()
            Me.cmbSanad1TarikSanad = New System.Windows.Forms.ComboBox()
            Me.lblSanad1TarikSanad = New System.Windows.Forms.Label()
            Me.cmbSanad1ShomareSanad = New System.Windows.Forms.ComboBox()
            Me.lblSanad1ShomareSanad = New System.Windows.Forms.Label()
            Me.pnlSanad1Top = New System.Windows.Forms.Panel()
            Me.lblSanad1RecordCount = New System.Windows.Forms.Label()
            Me.btnSanad1Preview = New System.Windows.Forms.Button()
            Me.nudSanad1HeaderRow = New System.Windows.Forms.NumericUpDown()
            Me.lblSanad1HeaderRow = New System.Windows.Forms.Label()
            Me.btnSanad1SelectFile = New System.Windows.Forms.Button()
            Me.lblSanad1File = New System.Windows.Forms.Label()
            Me.splitterMain = New System.Windows.Forms.Splitter()

            Me.grpSanad2.SuspendLayout()
            Me.pnlDgvSanad2.SuspendLayout()
            CType(Me.dgvSanad2Preview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSanad2Mapping.SuspendLayout()
            Me.pnlSanad2Top.SuspendLayout()
            CType(Me.nudSanad2HeaderRow, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpSanad1.SuspendLayout()
            Me.pnlDgvSanad1.SuspendLayout()
            CType(Me.dgvSanad1Preview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSanad1Mapping.SuspendLayout()
            Me.pnlSanad1Top.SuspendLayout()
            CType(Me.nudSanad1HeaderRow, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            'grpSanad2
            '
            Me.grpSanad2.Controls.Add(Me.pnlDgvSanad2)
            Me.grpSanad2.Controls.Add(Me.pnlSanad2Mapping)
            Me.grpSanad2.Controls.Add(Me.pnlSanad2Top)
            Me.grpSanad2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpSanad2.Location = New System.Drawing.Point(0, 335)
            Me.grpSanad2.Name = "grpSanad2"
            Me.grpSanad2.Padding = New System.Windows.Forms.Padding(4)
            Me.grpSanad2.Size = New System.Drawing.Size(1180, 365)
            Me.grpSanad2.TabIndex = 0
            Me.grpSanad2.TabStop = False
            Me.grpSanad2.Text = "تبديل اطلاعات سند 2 (رديف‌هاي سند)"
            '
            'pnlDgvSanad2
            '
            Me.pnlDgvSanad2.Controls.Add(Me.dgvSanad2Preview)
            Me.pnlDgvSanad2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlDgvSanad2.Location = New System.Drawing.Point(4, 208)
            Me.pnlDgvSanad2.Name = "pnlDgvSanad2"
            Me.pnlDgvSanad2.Size = New System.Drawing.Size(1172, 153)
            Me.pnlDgvSanad2.TabIndex = 0
            '
            'dgvSanad2Preview
            '
            Me.dgvSanad2Preview.AllowUserToAddRows = False
            Me.dgvSanad2Preview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.dgvSanad2Preview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSanad2Preview.Location = New System.Drawing.Point(0, 0)
            Me.dgvSanad2Preview.Name = "dgvSanad2Preview"
            Me.dgvSanad2Preview.ReadOnly = True
            Me.dgvSanad2Preview.RowHeadersVisible = False
            Me.dgvSanad2Preview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSanad2Preview.Size = New System.Drawing.Size(1172, 153)
            Me.dgvSanad2Preview.TabIndex = 0
            '
            'pnlSanad2Mapping
            '
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2TxDate)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2TxDate)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2TxNum)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2TxNum)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Bestankar)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Bestankar)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Bedehkar)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Bedehkar)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2SharhRadif)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2SharhRadif)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2TarikSanad)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2TarikSanad)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Tafsili3)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Tafsili3)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Tafsili2)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Tafsili2)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Tafsili1)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Tafsili1)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Moein)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Moein)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Kol)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Kol)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2Goruh)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2Goruh)
            Me.pnlSanad2Mapping.Controls.Add(Me.cmbSanad2ShomareSanad)
            Me.pnlSanad2Mapping.Controls.Add(Me.lblSanad2ShomareSanad)
            Me.pnlSanad2Mapping.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSanad2Mapping.Location = New System.Drawing.Point(4, 108)
            Me.pnlSanad2Mapping.Name = "pnlSanad2Mapping"
            Me.pnlSanad2Mapping.Padding = New System.Windows.Forms.Padding(5, 4, 5, 4)
            Me.pnlSanad2Mapping.Size = New System.Drawing.Size(1172, 100)
            Me.pnlSanad2Mapping.TabIndex = 1

            Me.cmbSanad2TxDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2TxDate.Location = New System.Drawing.Point(150, 42)
            Me.cmbSanad2TxDate.Name = "cmbSanad2TxDate"
            Me.cmbSanad2TxDate.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2TxDate.TabIndex = 0
            '
            'lblSanad2TxDate
            '
            Me.lblSanad2TxDate.AutoSize = True
            Me.lblSanad2TxDate.Location = New System.Drawing.Point(235, 45)
            Me.lblSanad2TxDate.Name = "lblSanad2TxDate"
            Me.lblSanad2TxDate.Size = New System.Drawing.Size(69, 13)
            Me.lblSanad2TxDate.TabIndex = 1
            Me.lblSanad2TxDate.Text = "تاريخ تراکنش:"
            '
            'cmbSanad2TxNum
            '
            Me.cmbSanad2TxNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2TxNum.Location = New System.Drawing.Point(330, 42)
            Me.cmbSanad2TxNum.Name = "cmbSanad2TxNum"
            Me.cmbSanad2TxNum.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2TxNum.TabIndex = 2
            '
            'lblSanad2TxNum
            '
            Me.lblSanad2TxNum.AutoSize = True
            Me.lblSanad2TxNum.Location = New System.Drawing.Point(415, 45)
            Me.lblSanad2TxNum.Name = "lblSanad2TxNum"
            Me.lblSanad2TxNum.Size = New System.Drawing.Size(78, 13)
            Me.lblSanad2TxNum.TabIndex = 3
            Me.lblSanad2TxNum.Text = "شماره تراکنش:"
            '
            'cmbSanad2Bestankar
            '
            Me.cmbSanad2Bestankar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Bestankar.Location = New System.Drawing.Point(510, 42)
            Me.cmbSanad2Bestankar.Name = "cmbSanad2Bestankar"
            Me.cmbSanad2Bestankar.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Bestankar.TabIndex = 4
            '
            'lblSanad2Bestankar
            '
            Me.lblSanad2Bestankar.AutoSize = True
            Me.lblSanad2Bestankar.Location = New System.Drawing.Point(595, 45)
            Me.lblSanad2Bestankar.Name = "lblSanad2Bestankar"
            Me.lblSanad2Bestankar.Size = New System.Drawing.Size(49, 13)
            Me.lblSanad2Bestankar.TabIndex = 5
            Me.lblSanad2Bestankar.Text = "بستانکار:"
            '
            'cmbSanad2Bedehkar
            '
            Me.cmbSanad2Bedehkar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Bedehkar.Location = New System.Drawing.Point(670, 42)
            Me.cmbSanad2Bedehkar.Name = "cmbSanad2Bedehkar"
            Me.cmbSanad2Bedehkar.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Bedehkar.TabIndex = 6
            '
            'lblSanad2Bedehkar
            '
            Me.lblSanad2Bedehkar.AutoSize = True
            Me.lblSanad2Bedehkar.Location = New System.Drawing.Point(755, 45)
            Me.lblSanad2Bedehkar.Name = "lblSanad2Bedehkar"
            Me.lblSanad2Bedehkar.Size = New System.Drawing.Size(41, 13)
            Me.lblSanad2Bedehkar.TabIndex = 7
            Me.lblSanad2Bedehkar.Text = "بدهکار:"
            '
            'cmbSanad2SharhRadif
            '
            Me.cmbSanad2SharhRadif.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2SharhRadif.Location = New System.Drawing.Point(820, 42)
            Me.cmbSanad2SharhRadif.Name = "cmbSanad2SharhRadif"
            Me.cmbSanad2SharhRadif.Size = New System.Drawing.Size(90, 21)
            Me.cmbSanad2SharhRadif.TabIndex = 80
            '
            'lblSanad2SharhRadif
            '
            Me.lblSanad2SharhRadif.AutoSize = True
            Me.lblSanad2SharhRadif.Location = New System.Drawing.Point(915, 45)
            Me.lblSanad2SharhRadif.Name = "lblSanad2SharhRadif"
            Me.lblSanad2SharhRadif.Size = New System.Drawing.Size(58, 13)
            Me.lblSanad2SharhRadif.TabIndex = 81
            Me.lblSanad2SharhRadif.Text = "شرح رديف:"
            '
            'cmbSanad2TarikSanad
            '
            Me.cmbSanad2TarikSanad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2TarikSanad.Location = New System.Drawing.Point(980, 42)
            Me.cmbSanad2TarikSanad.Name = "cmbSanad2TarikSanad"
            Me.cmbSanad2TarikSanad.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2TarikSanad.TabIndex = 82
            '
            'lblSanad2TarikSanad
            '
            Me.lblSanad2TarikSanad.AutoSize = True
            Me.lblSanad2TarikSanad.Location = New System.Drawing.Point(1065, 45)
            Me.lblSanad2TarikSanad.Name = "lblSanad2TarikSanad"
            Me.lblSanad2TarikSanad.Size = New System.Drawing.Size(70, 13)
            Me.lblSanad2TarikSanad.TabIndex = 83
            Me.lblSanad2TarikSanad.Text = "تاريخ سند:"

            'cmbSanad2Tafsili3
            '
            Me.cmbSanad2Tafsili3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Tafsili3.Location = New System.Drawing.Point(30, 5)
            Me.cmbSanad2Tafsili3.Name = "cmbSanad2Tafsili3"
            Me.cmbSanad2Tafsili3.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Tafsili3.TabIndex = 8
            '
            'lblSanad2Tafsili3
            '
            Me.lblSanad2Tafsili3.AutoSize = True
            Me.lblSanad2Tafsili3.Location = New System.Drawing.Point(115, 8)
            Me.lblSanad2Tafsili3.Name = "lblSanad2Tafsili3"
            Me.lblSanad2Tafsili3.Size = New System.Drawing.Size(57, 13)
            Me.lblSanad2Tafsili3.TabIndex = 9
            Me.lblSanad2Tafsili3.Text = "تفصیلی 3:"
            '
            'cmbSanad2Tafsili2
            '
            Me.cmbSanad2Tafsili2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Tafsili2.Location = New System.Drawing.Point(200, 5)
            Me.cmbSanad2Tafsili2.Name = "cmbSanad2Tafsili2"
            Me.cmbSanad2Tafsili2.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Tafsili2.TabIndex = 10
            '
            'lblSanad2Tafsili2
            '
            Me.lblSanad2Tafsili2.AutoSize = True
            Me.lblSanad2Tafsili2.Location = New System.Drawing.Point(285, 8)
            Me.lblSanad2Tafsili2.Name = "lblSanad2Tafsili2"
            Me.lblSanad2Tafsili2.Size = New System.Drawing.Size(57, 13)
            Me.lblSanad2Tafsili2.TabIndex = 11
            Me.lblSanad2Tafsili2.Text = "تفصیلی 2:"
            '
            'cmbSanad2Tafsili1
            '
            Me.cmbSanad2Tafsili1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Tafsili1.Location = New System.Drawing.Point(360, 5)
            Me.cmbSanad2Tafsili1.Name = "cmbSanad2Tafsili1"
            Me.cmbSanad2Tafsili1.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Tafsili1.TabIndex = 12
            '
            'lblSanad2Tafsili1
            '
            Me.lblSanad2Tafsili1.AutoSize = True
            Me.lblSanad2Tafsili1.Location = New System.Drawing.Point(445, 8)
            Me.lblSanad2Tafsili1.Name = "lblSanad2Tafsili1"
            Me.lblSanad2Tafsili1.Size = New System.Drawing.Size(57, 13)
            Me.lblSanad2Tafsili1.TabIndex = 13
            Me.lblSanad2Tafsili1.Text = "تفصیلی 1:"
            '
            'cmbSanad2Moein
            '
            Me.cmbSanad2Moein.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Moein.Location = New System.Drawing.Point(520, 5)
            Me.cmbSanad2Moein.Name = "cmbSanad2Moein"
            Me.cmbSanad2Moein.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Moein.TabIndex = 14
            '
            'lblSanad2Moein
            '
            Me.lblSanad2Moein.AutoSize = True
            Me.lblSanad2Moein.Location = New System.Drawing.Point(605, 8)
            Me.lblSanad2Moein.Name = "lblSanad2Moein"
            Me.lblSanad2Moein.Size = New System.Drawing.Size(35, 13)
            Me.lblSanad2Moein.TabIndex = 15
            Me.lblSanad2Moein.Text = "معين:"
            '
            'cmbSanad2Kol
            '
            Me.cmbSanad2Kol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Kol.Location = New System.Drawing.Point(680, 5)
            Me.cmbSanad2Kol.Name = "cmbSanad2Kol"
            Me.cmbSanad2Kol.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Kol.TabIndex = 16
            '
            'lblSanad2Kol
            '
            Me.lblSanad2Kol.AutoSize = True
            Me.lblSanad2Kol.Location = New System.Drawing.Point(765, 8)
            Me.lblSanad2Kol.Name = "lblSanad2Kol"
            Me.lblSanad2Kol.Size = New System.Drawing.Size(24, 13)
            Me.lblSanad2Kol.TabIndex = 17
            Me.lblSanad2Kol.Text = "کل:"
            '
            'cmbSanad2Goruh
            '
            Me.cmbSanad2Goruh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2Goruh.Location = New System.Drawing.Point(840, 5)
            Me.cmbSanad2Goruh.Name = "cmbSanad2Goruh"
            Me.cmbSanad2Goruh.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2Goruh.TabIndex = 18
            '
            'lblSanad2Goruh
            '
            Me.lblSanad2Goruh.AutoSize = True
            Me.lblSanad2Goruh.Location = New System.Drawing.Point(925, 8)
            Me.lblSanad2Goruh.Name = "lblSanad2Goruh"
            Me.lblSanad2Goruh.Size = New System.Drawing.Size(31, 13)
            Me.lblSanad2Goruh.TabIndex = 19
            Me.lblSanad2Goruh.Text = "گروه:"
            '
            'cmbSanad2ShomareSanad
            '
            Me.cmbSanad2ShomareSanad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad2ShomareSanad.Location = New System.Drawing.Point(1000, 5)
            Me.cmbSanad2ShomareSanad.Name = "cmbSanad2ShomareSanad"
            Me.cmbSanad2ShomareSanad.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad2ShomareSanad.TabIndex = 20
            '
            'lblSanad2ShomareSanad
            '
            Me.lblSanad2ShomareSanad.AutoSize = True
            Me.lblSanad2ShomareSanad.Location = New System.Drawing.Point(1085, 8)
            Me.lblSanad2ShomareSanad.Name = "lblSanad2ShomareSanad"
            Me.lblSanad2ShomareSanad.Size = New System.Drawing.Size(65, 13)
            Me.lblSanad2ShomareSanad.TabIndex = 21
            Me.lblSanad2ShomareSanad.Text = "شماره سند:"
            '
            'pnlSanad2Top
            '
            Me.pnlSanad2Top.Controls.Add(Me.btnConvertBoth)
            Me.pnlSanad2Top.Controls.Add(Me.lblSanad2RecordCount)
            Me.pnlSanad2Top.Controls.Add(Me.btnSanad2Preview)
            Me.pnlSanad2Top.Controls.Add(Me.nudSanad2HeaderRow)
            Me.pnlSanad2Top.Controls.Add(Me.lblSanad2HeaderRow)
            Me.pnlSanad2Top.Controls.Add(Me.btnSanad2SelectFile)
            Me.pnlSanad2Top.Controls.Add(Me.lblSanad2File)
            Me.pnlSanad2Top.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSanad2Top.Location = New System.Drawing.Point(4, 18)
            Me.pnlSanad2Top.Name = "pnlSanad2Top"
            Me.pnlSanad2Top.Padding = New System.Windows.Forms.Padding(5)
            Me.pnlSanad2Top.Size = New System.Drawing.Size(1172, 90)
            Me.pnlSanad2Top.TabIndex = 2
            '
            'btnConvertBoth
            '
            Me.btnConvertBoth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnConvertBoth.Enabled = False
            Me.btnConvertBoth.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnConvertBoth.Location = New System.Drawing.Point(850, 25)
            Me.btnConvertBoth.Name = "btnConvertBoth"
            Me.btnConvertBoth.Size = New System.Drawing.Size(260, 40)
            Me.btnConvertBoth.TabIndex = 0
            Me.btnConvertBoth.Text = "تبدیل هوشمند سند 1 و سند 2"
            '
            'lblSanad2RecordCount
            '
            Me.lblSanad2RecordCount.Location = New System.Drawing.Point(440, 50)
            Me.lblSanad2RecordCount.Name = "lblSanad2RecordCount"
            Me.lblSanad2RecordCount.Size = New System.Drawing.Size(300, 20)
            Me.lblSanad2RecordCount.TabIndex = 0
            Me.lblSanad2RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 2: 0"
            Me.lblSanad2RecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnSanad2Preview
            '
            Me.btnSanad2Preview.Location = New System.Drawing.Point(290, 44)
            Me.btnSanad2Preview.Name = "btnSanad2Preview"
            Me.btnSanad2Preview.Size = New System.Drawing.Size(140, 30)
            Me.btnSanad2Preview.TabIndex = 1
            Me.btnSanad2Preview.Text = "پيش‌نمايش سند 2"
            Me.btnSanad2Preview.UseVisualStyleBackColor = True
            '
            'nudSanad2HeaderRow
            '
            Me.nudSanad2HeaderRow.Location = New System.Drawing.Point(430, 8)
            Me.nudSanad2HeaderRow.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.nudSanad2HeaderRow.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudSanad2HeaderRow.Name = "nudSanad2HeaderRow"
            Me.nudSanad2HeaderRow.Size = New System.Drawing.Size(70, 21)
            Me.nudSanad2HeaderRow.TabIndex = 2
            Me.nudSanad2HeaderRow.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblSanad2HeaderRow
            '
            Me.lblSanad2HeaderRow.AutoSize = True
            Me.lblSanad2HeaderRow.Location = New System.Drawing.Point(510, 10)
            Me.lblSanad2HeaderRow.Name = "lblSanad2HeaderRow"
            Me.lblSanad2HeaderRow.Size = New System.Drawing.Size(79, 13)
            Me.lblSanad2HeaderRow.TabIndex = 3
            Me.lblSanad2HeaderRow.Text = "رديف سرستون:"
            '
            'btnSanad2SelectFile
            '
            Me.btnSanad2SelectFile.Location = New System.Drawing.Point(290, 6)
            Me.btnSanad2SelectFile.Name = "btnSanad2SelectFile"
            Me.btnSanad2SelectFile.Size = New System.Drawing.Size(120, 28)
            Me.btnSanad2SelectFile.TabIndex = 4
            Me.btnSanad2SelectFile.Text = "انتخاب فايل سند 2"
            Me.btnSanad2SelectFile.UseVisualStyleBackColor = True
            '
            'lblSanad2File
            '
            Me.lblSanad2File.Location = New System.Drawing.Point(8, 8)
            Me.lblSanad2File.Name = "lblSanad2File"
            Me.lblSanad2File.Size = New System.Drawing.Size(270, 22)
            Me.lblSanad2File.TabIndex = 5
            Me.lblSanad2File.Text = "فايلي انتخاب نشده است"
            Me.lblSanad2File.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'grpSanad1
            '
            Me.grpSanad1.Controls.Add(Me.pnlDgvSanad1)
            Me.grpSanad1.Controls.Add(Me.pnlSanad1Mapping)
            Me.grpSanad1.Controls.Add(Me.pnlSanad1Top)
            Me.grpSanad1.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpSanad1.Location = New System.Drawing.Point(0, 0)
            Me.grpSanad1.Name = "grpSanad1"
            Me.grpSanad1.Padding = New System.Windows.Forms.Padding(4)
            Me.grpSanad1.Size = New System.Drawing.Size(1180, 330)
            Me.grpSanad1.TabIndex = 2
            Me.grpSanad1.TabStop = False
            Me.grpSanad1.Text = "تبديل اطلاعات سند 1 (سرسند)"
            '
            'pnlDgvSanad1
            '
            Me.pnlDgvSanad1.Controls.Add(Me.dgvSanad1Preview)
            Me.pnlDgvSanad1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlDgvSanad1.Location = New System.Drawing.Point(4, 163)
            Me.pnlDgvSanad1.Name = "pnlDgvSanad1"
            Me.pnlDgvSanad1.Size = New System.Drawing.Size(1172, 163)
            Me.pnlDgvSanad1.TabIndex = 0
            '
            'dgvSanad1Preview
            '
            Me.dgvSanad1Preview.AllowUserToAddRows = False
            Me.dgvSanad1Preview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.dgvSanad1Preview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSanad1Preview.Location = New System.Drawing.Point(0, 0)
            Me.dgvSanad1Preview.Name = "dgvSanad1Preview"
            Me.dgvSanad1Preview.ReadOnly = True
            Me.dgvSanad1Preview.RowHeadersVisible = False
            Me.dgvSanad1Preview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSanad1Preview.Size = New System.Drawing.Size(1172, 163)
            Me.dgvSanad1Preview.TabIndex = 0
            '
            'pnlSanad1Mapping
            '
            Me.pnlSanad1Mapping.Controls.Add(Me.cmbSanad1Sharh)
            Me.pnlSanad1Mapping.Controls.Add(Me.lblSanad1Sharh)
            Me.pnlSanad1Mapping.Controls.Add(Me.cmbSanad1TarikSanad)
            Me.pnlSanad1Mapping.Controls.Add(Me.lblSanad1TarikSanad)
            Me.pnlSanad1Mapping.Controls.Add(Me.cmbSanad1ShomareSanad)
            Me.pnlSanad1Mapping.Controls.Add(Me.lblSanad1ShomareSanad)
            Me.pnlSanad1Mapping.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSanad1Mapping.Location = New System.Drawing.Point(4, 108)
            Me.pnlSanad1Mapping.Name = "pnlSanad1Mapping"
            Me.pnlSanad1Mapping.Padding = New System.Windows.Forms.Padding(5, 4, 5, 4)
            Me.pnlSanad1Mapping.Size = New System.Drawing.Size(1172, 55)
            Me.pnlSanad1Mapping.TabIndex = 1
            '
            'cmbSanad1Sharh
            '
            Me.cmbSanad1Sharh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad1Sharh.Location = New System.Drawing.Point(450, 8)
            Me.cmbSanad1Sharh.Name = "cmbSanad1Sharh"
            Me.cmbSanad1Sharh.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad1Sharh.TabIndex = 0
            '
            'lblSanad1Sharh
            '
            Me.lblSanad1Sharh.AutoSize = True
            Me.lblSanad1Sharh.Location = New System.Drawing.Point(535, 11)
            Me.lblSanad1Sharh.Name = "lblSanad1Sharh"
            Me.lblSanad1Sharh.Size = New System.Drawing.Size(81, 13)
            Me.lblSanad1Sharh.TabIndex = 1
            Me.lblSanad1Sharh.Text = "شرح کلي سند:"
            '
            'cmbSanad1TarikSanad
            '
            Me.cmbSanad1TarikSanad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad1TarikSanad.Location = New System.Drawing.Point(700, 8)
            Me.cmbSanad1TarikSanad.Name = "cmbSanad1TarikSanad"
            Me.cmbSanad1TarikSanad.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad1TarikSanad.TabIndex = 2
            '
            'lblSanad1TarikSanad
            '
            Me.lblSanad1TarikSanad.AutoSize = True
            Me.lblSanad1TarikSanad.Location = New System.Drawing.Point(785, 11)
            Me.lblSanad1TarikSanad.Name = "lblSanad1TarikSanad"
            Me.lblSanad1TarikSanad.Size = New System.Drawing.Size(56, 13)
            Me.lblSanad1TarikSanad.TabIndex = 3
            Me.lblSanad1TarikSanad.Text = "تاريخ سند:"
            '
            'cmbSanad1ShomareSanad
            '
            Me.cmbSanad1ShomareSanad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSanad1ShomareSanad.Location = New System.Drawing.Point(950, 8)
            Me.cmbSanad1ShomareSanad.Name = "cmbSanad1ShomareSanad"
            Me.cmbSanad1ShomareSanad.Size = New System.Drawing.Size(80, 21)
            Me.cmbSanad1ShomareSanad.TabIndex = 4
            '
            'lblSanad1ShomareSanad
            '
            Me.lblSanad1ShomareSanad.AutoSize = True
            Me.lblSanad1ShomareSanad.Location = New System.Drawing.Point(1035, 11)
            Me.lblSanad1ShomareSanad.Name = "lblSanad1ShomareSanad"
            Me.lblSanad1ShomareSanad.Size = New System.Drawing.Size(65, 13)
            Me.lblSanad1ShomareSanad.TabIndex = 5
            Me.lblSanad1ShomareSanad.Text = "شماره سند:"
            '
            'pnlSanad1Top
            '
            Me.pnlSanad1Top.Controls.Add(Me.lblSanad1RecordCount)
            Me.pnlSanad1Top.Controls.Add(Me.btnSanad1Preview)
            Me.pnlSanad1Top.Controls.Add(Me.nudSanad1HeaderRow)
            Me.pnlSanad1Top.Controls.Add(Me.lblSanad1HeaderRow)
            Me.pnlSanad1Top.Controls.Add(Me.btnSanad1SelectFile)
            Me.pnlSanad1Top.Controls.Add(Me.lblSanad1File)
            Me.pnlSanad1Top.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSanad1Top.Location = New System.Drawing.Point(4, 18)
            Me.pnlSanad1Top.Name = "pnlSanad1Top"
            Me.pnlSanad1Top.Padding = New System.Windows.Forms.Padding(5)
            Me.pnlSanad1Top.Size = New System.Drawing.Size(1172, 90)
            Me.pnlSanad1Top.TabIndex = 2
            '
            'lblSanad1RecordCount
            '
            Me.lblSanad1RecordCount.Location = New System.Drawing.Point(440, 50)
            Me.lblSanad1RecordCount.Name = "lblSanad1RecordCount"
            Me.lblSanad1RecordCount.Size = New System.Drawing.Size(300, 20)
            Me.lblSanad1RecordCount.TabIndex = 0
            Me.lblSanad1RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 1: 0"
            Me.lblSanad1RecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnSanad1Preview
            '
            Me.btnSanad1Preview.Location = New System.Drawing.Point(290, 44)
            Me.btnSanad1Preview.Name = "btnSanad1Preview"
            Me.btnSanad1Preview.Size = New System.Drawing.Size(140, 30)
            Me.btnSanad1Preview.TabIndex = 1
            Me.btnSanad1Preview.Text = "پيش‌نمايش سند 1"
            Me.btnSanad1Preview.UseVisualStyleBackColor = True
            '
            'nudSanad1HeaderRow
            '
            Me.nudSanad1HeaderRow.Location = New System.Drawing.Point(430, 8)
            Me.nudSanad1HeaderRow.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.nudSanad1HeaderRow.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudSanad1HeaderRow.Name = "nudSanad1HeaderRow"
            Me.nudSanad1HeaderRow.Size = New System.Drawing.Size(70, 21)
            Me.nudSanad1HeaderRow.TabIndex = 2
            Me.nudSanad1HeaderRow.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblSanad1HeaderRow
            '
            Me.lblSanad1HeaderRow.AutoSize = True
            Me.lblSanad1HeaderRow.Location = New System.Drawing.Point(510, 10)
            Me.lblSanad1HeaderRow.Name = "lblSanad1HeaderRow"
            Me.lblSanad1HeaderRow.Size = New System.Drawing.Size(79, 13)
            Me.lblSanad1HeaderRow.TabIndex = 3
            Me.lblSanad1HeaderRow.Text = "رديف سرستون:"
            '
            'btnSanad1SelectFile
            '
            Me.btnSanad1SelectFile.Location = New System.Drawing.Point(290, 6)
            Me.btnSanad1SelectFile.Name = "btnSanad1SelectFile"
            Me.btnSanad1SelectFile.Size = New System.Drawing.Size(120, 28)
            Me.btnSanad1SelectFile.TabIndex = 4
            Me.btnSanad1SelectFile.Text = "انتخاب فايل سند 1"
            Me.btnSanad1SelectFile.UseVisualStyleBackColor = True
            '
            'lblSanad1File
            '
            Me.lblSanad1File.Location = New System.Drawing.Point(8, 8)
            Me.lblSanad1File.Name = "lblSanad1File"
            Me.lblSanad1File.Size = New System.Drawing.Size(270, 22)
            Me.lblSanad1File.TabIndex = 5
            Me.lblSanad1File.Text = "فايلي انتخاب نشده است"
            Me.lblSanad1File.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'splitterMain
            '
            Me.splitterMain.Dock = System.Windows.Forms.DockStyle.Top
            Me.splitterMain.Location = New System.Drawing.Point(0, 330)
            Me.splitterMain.Name = "splitterMain"
            Me.splitterMain.Size = New System.Drawing.Size(1180, 5)
            Me.splitterMain.TabIndex = 1
            Me.splitterMain.TabStop = False
            '
            'HesabdaryTabdilDataSanadForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1180, 700)
            Me.Controls.Add(Me.grpSanad2)
            Me.Controls.Add(Me.splitterMain)
            Me.Controls.Add(Me.grpSanad1)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.MinimumSize = New System.Drawing.Size(1180, 600)
            Me.Name = "HesabdaryTabdilDataSanadForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "تبديل هوشمند اسناد حسابداري"
            Me.grpSanad2.ResumeLayout(False)
            Me.pnlDgvSanad2.ResumeLayout(False)
            CType(Me.dgvSanad2Preview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSanad2Mapping.ResumeLayout(False)
            Me.pnlSanad2Mapping.PerformLayout()
            Me.pnlSanad2Top.ResumeLayout(False)
            Me.pnlSanad2Top.PerformLayout()
            CType(Me.nudSanad2HeaderRow, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpSanad1.ResumeLayout(False)
            Me.pnlDgvSanad1.ResumeLayout(False)
            CType(Me.dgvSanad1Preview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSanad1Mapping.ResumeLayout(False)
            Me.pnlSanad1Mapping.PerformLayout()
            Me.pnlSanad1Top.ResumeLayout(False)
            Me.pnlSanad1Top.PerformLayout()
            CType(Me.nudSanad1HeaderRow, System.ComponentModel.ISupportInitialize).EndInit()

            Me.ResumeLayout(False)

        End Sub

        ' Helper: ظ‚ط±ط§ط± ط¯ط§ط¯ظ† ظ„غŒط¨ظ„ + ع©ط§ظ…ط¨ظˆط¨ط§ع©ط³ ط¯ط± ظ…ظˆظ‚ط¹غŒطھ ظ…ط´ط®طµ


    End Class
End Namespace

