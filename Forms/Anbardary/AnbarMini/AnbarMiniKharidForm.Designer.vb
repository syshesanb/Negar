Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniKharidForm
        Inherits AppBaseForm

        Private components As IContainer

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New Panel()
            Me.lblTitle = New Label()
            Me.lblInvoiceNo = New Label()
            Me.txtInvoiceNo = New TextBox()
            Me.lblInvoiceDate = New Label()
            Me.txtInvoiceDate = New TextBox()
            Me.lblVendor = New Label()
            Me.txtVendorName = New TextBox()
            Me.pnlProductAdd = New Panel()
            Me.lblSearch = New Label()
            Me.txtProductSearch = New TextBox()
            Me.lblQty = New Label()
            Me.numQuantity = New NumericUpDown()
            Me.lblBuyPrice = New Label()
            Me.txtUnitPrice = New TextBox()
            Me.btnAdd = New Button()
            Me.btnBrowseProduct = New Button()
            Me.dgvItems = New DataGridView()
            Me.colProductID = New DataGridViewTextBoxColumn()
            Me.colCode = New DataGridViewTextBoxColumn()
            Me.colName = New DataGridViewTextBoxColumn()
            Me.colQuantity = New DataGridViewTextBoxColumn()
            Me.colUnitPrice = New DataGridViewTextBoxColumn()
            Me.colTotalPrice = New DataGridViewTextBoxColumn()
            Me.pnlFooter = New Panel()
            Me.lblTotalText = New Label()
            Me.lblTotalAmount = New Label()
            Me.btnSave = New Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlProductAdd.SuspendLayout()
            CType(Me.numQuantity, ISupportInitialize).BeginInit()
            CType(Me.dgvItems, ISupportInitialize).BeginInit()
            Me.pnlFooter.SuspendLayout()
            Me.SuspendLayout()

            ' pnlHeader
            Me.pnlHeader.BackColor = Color.FromArgb(46, 204, 113)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Controls.Add(Me.lblInvoiceNo)
            Me.pnlHeader.Controls.Add(Me.txtInvoiceNo)
            Me.pnlHeader.Controls.Add(Me.lblInvoiceDate)
            Me.pnlHeader.Controls.Add(Me.txtInvoiceDate)
            Me.pnlHeader.Controls.Add(Me.btnPickDate)
            Me.pnlHeader.Controls.Add(Me.lblVendor)
            Me.pnlHeader.Controls.Add(Me.txtVendorName)
            Me.pnlHeader.Controls.Add(Me.btnPickVendor)
            Me.pnlHeader.Controls.Add(Me.lblWarehouse)
            Me.pnlHeader.Controls.Add(Me.cmbWarehouse)
            Me.pnlHeader.Dock = DockStyle.Top
            Me.pnlHeader.Location = New Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New Size(950, 60)

            ' lblTitle
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New Font("B Yekan", 12.0!, FontStyle.Bold)
            Me.lblTitle.ForeColor = Color.White
            Me.lblTitle.Location = New Point(800, 15)
            Me.lblTitle.Text = "فاکتور خرید کالا"

            ' lblInvoiceNo
            Me.lblInvoiceNo.AutoSize = True
            Me.lblInvoiceNo.ForeColor = Color.White
            Me.lblInvoiceNo.Location = New Point(745, 19)
            Me.lblInvoiceNo.Text = "شماره:"

            ' txtInvoiceNo
            Me.txtInvoiceNo.Location = New Point(665, 17)
            Me.txtInvoiceNo.Name = "txtInvoiceNo"
            Me.txtInvoiceNo.Size = New Size(75, 27)

            ' lblInvoiceDate
            Me.lblInvoiceDate.AutoSize = True
            Me.lblInvoiceDate.ForeColor = Color.White
            Me.lblInvoiceDate.Location = New Point(620, 19)
            Me.lblInvoiceDate.Text = "تاریخ:"

            ' txtInvoiceDate
            Me.txtInvoiceDate.Location = New Point(535, 17)
            Me.txtInvoiceDate.Name = "txtInvoiceDate"
            Me.txtInvoiceDate.Size = New Size(82, 27)

            ' btnPickDate
            Me.btnPickDate.BackColor = Color.White
            Me.btnPickDate.FlatStyle = FlatStyle.Flat
            Me.btnPickDate.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.btnPickDate.ForeColor = Color.Black
            Me.btnPickDate.Location = New Point(505, 17)
            Me.btnPickDate.Name = "btnPickDate"
            Me.btnPickDate.Size = New Size(28, 27)
            Me.btnPickDate.Text = "..."
            Me.btnPickDate.UseVisualStyleBackColor = False

            ' lblVendor
            Me.lblVendor.AutoSize = True
            Me.lblVendor.ForeColor = Color.White
            Me.lblVendor.Location = New Point(440, 19)
            Me.lblVendor.Text = "فروشنده:"

            ' txtVendorName
            Me.txtVendorName.Location = New Point(280, 17)
            Me.txtVendorName.Name = "txtVendorName"
            Me.txtVendorName.Size = New Size(155, 27)

            ' btnPickVendor
            Me.btnPickVendor.BackColor = Color.White
            Me.btnPickVendor.FlatStyle = FlatStyle.Flat
            Me.btnPickVendor.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.btnPickVendor.ForeColor = Color.Black
            Me.btnPickVendor.Location = New Point(250, 17)
            Me.btnPickVendor.Name = "btnPickVendor"
            Me.btnPickVendor.Size = New Size(28, 27)
            Me.btnPickVendor.Text = "..."
            Me.btnPickVendor.UseVisualStyleBackColor = False

            ' lblWarehouse
            Me.lblWarehouse.AutoSize = True
            Me.lblWarehouse.ForeColor = Color.White
            Me.lblWarehouse.Location = New Point(195, 19)
            Me.lblWarehouse.Text = "انبار:"

            ' cmbWarehouse
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouse.FormattingEnabled = True
            Me.cmbWarehouse.Location = New Point(15, 17)
            Me.cmbWarehouse.Name = "cmbWarehouse"
            Me.cmbWarehouse.Size = New Size(175, 27)

            ' pnlProductAdd
            Me.pnlProductAdd.BackColor = Color.FromArgb(245, 246, 250)
            Me.pnlProductAdd.Controls.Add(Me.lblSearch)
            Me.pnlProductAdd.Controls.Add(Me.txtProductSearch)
            Me.pnlProductAdd.Controls.Add(Me.lblQty)
            Me.pnlProductAdd.Controls.Add(Me.numQuantity)
            Me.pnlProductAdd.Controls.Add(Me.lblBuyPrice)
            Me.pnlProductAdd.Controls.Add(Me.txtUnitPrice)
            Me.pnlProductAdd.Controls.Add(Me.btnAdd)
            Me.pnlProductAdd.Controls.Add(Me.btnBrowseProduct)
            Me.pnlProductAdd.Dock = DockStyle.Top
            Me.pnlProductAdd.Location = New Point(0, 60)
            Me.pnlProductAdd.Name = "pnlProductAdd"
            Me.pnlProductAdd.Size = New Size(950, 50)

            ' lblSearch
            Me.lblSearch.AutoSize = True
            Me.lblSearch.Location = New Point(880, 15)
            Me.lblSearch.Text = "کالا:"

            ' txtProductSearch
            Me.txtProductSearch.Location = New Point(570, 12)
            Me.txtProductSearch.Name = "txtProductSearch"
            Me.txtProductSearch.Size = New Size(300, 27)

            ' lblQty
            Me.lblQty.AutoSize = True
            Me.lblQty.Location = New Point(515, 15)
            Me.lblQty.Text = "تعداد:"

            ' numQuantity
            Me.numQuantity.Location = New Point(440, 12)
            Me.numQuantity.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numQuantity.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numQuantity.Name = "numQuantity"
            Me.numQuantity.Size = New Size(70, 27)
            Me.numQuantity.Value = New Decimal(New Integer() {1, 0, 0, 0})

            ' lblBuyPrice
            Me.lblBuyPrice.AutoSize = True
            Me.lblBuyPrice.Location = New Point(360, 15)
            Me.lblBuyPrice.Text = "قیمت خرید:"

            ' txtUnitPrice
            Me.txtUnitPrice.Location = New Point(230, 12)
            Me.txtUnitPrice.Name = "txtUnitPrice"
            Me.txtUnitPrice.Size = New Size(125, 27)

            ' btnAdd
            Me.btnAdd.BackColor = Color.FromArgb(39, 174, 96)
            Me.btnAdd.FlatStyle = FlatStyle.Flat
            Me.btnAdd.ForeColor = Color.White
            Me.btnAdd.Location = New Point(115, 10)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New Size(100, 30)
            Me.btnAdd.Text = "+ افزودن"
            Me.btnAdd.UseVisualStyleBackColor = False

            ' btnBrowseProduct
            Me.btnBrowseProduct.BackColor = Color.FromArgb(41, 128, 185)
            Me.btnBrowseProduct.FlatStyle = FlatStyle.Flat
            Me.btnBrowseProduct.ForeColor = Color.White
            Me.btnBrowseProduct.Location = New Point(10, 10)
            Me.btnBrowseProduct.Name = "btnBrowseProduct"
            Me.btnBrowseProduct.Size = New Size(95, 30)
            Me.btnBrowseProduct.Text = "جستجو (F2)"
            Me.btnBrowseProduct.UseVisualStyleBackColor = False
            Me.btnAdd.UseVisualStyleBackColor = False

            ' dgvItems
            Me.dgvItems.AllowUserToAddRows = False
            Me.dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvItems.Columns.AddRange(New DataGridViewColumn() {Me.colProductID, Me.colCode, Me.colName, Me.colQuantity, Me.colUnitPrice, Me.colTotalPrice})
            Me.dgvItems.Dock = DockStyle.Fill
            Me.dgvItems.Location = New Point(0, 110)
            Me.dgvItems.Name = "dgvItems"
            Me.dgvItems.RowTemplate.Height = 28
            Me.dgvItems.Size = New Size(950, 420)

            ' colProductID
            Me.colProductID.HeaderText = "شناسه"
            Me.colProductID.Name = "colProductID"
            Me.colProductID.Visible = False

            ' colCode
            Me.colCode.HeaderText = "کد کالا"
            Me.colCode.Name = "colCode"

            ' colName
            Me.colName.HeaderText = "نام کالا"
            Me.colName.Name = "colName"

            ' colQuantity
            Me.colQuantity.HeaderText = "تعداد"
            Me.colQuantity.Name = "colQuantity"

            ' colUnitPrice
            Me.colUnitPrice.HeaderText = "قیمت خرید (ریال)"
            Me.colUnitPrice.Name = "colUnitPrice"

            ' colTotalPrice
            Me.colTotalPrice.HeaderText = "جمع کل (ریال)"
            Me.colTotalPrice.Name = "colTotalPrice"

            ' pnlFooter
            Me.pnlFooter.BackColor = Color.FromArgb(44, 62, 80)
            Me.pnlFooter.Controls.Add(Me.lblTotalText)
            Me.pnlFooter.Controls.Add(Me.lblTotalAmount)
            Me.pnlFooter.Controls.Add(Me.btnSave)
            Me.pnlFooter.Dock = DockStyle.Bottom
            Me.pnlFooter.Location = New Point(0, 530)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New Size(950, 70)

            ' lblTotalText
            Me.lblTotalText.AutoSize = True
            Me.lblTotalText.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.lblTotalText.ForeColor = Color.White
            Me.lblTotalText.Location = New Point(830, 22)
            Me.lblTotalText.Text = "جمع کل خرید:"

            ' lblTotalAmount
            Me.lblTotalAmount.AutoSize = True
            Me.lblTotalAmount.Font = New Font("B Yekan", 15.0!, FontStyle.Bold)
            Me.lblTotalAmount.ForeColor = Color.FromArgb(46, 204, 113)
            Me.lblTotalAmount.Location = New Point(620, 16)
            Me.lblTotalAmount.Text = "۰ ریال"

            ' btnSave
            Me.btnSave.BackColor = Color.FromArgb(46, 204, 113)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(30, 12)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(160, 45)
            Me.btnSave.Text = "ثبت فاکتور خرید"
            Me.btnSave.UseVisualStyleBackColor = False

            ' Form Setup
            Me.AutoScaleDimensions = New SizeF(8.0!, 19.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.dgvItems)
            Me.Controls.Add(Me.pnlFooter)
            Me.Controls.Add(Me.pnlProductAdd)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New Font("B Yekan", 9.0!)
            Me.Name = "AnbarMiniKharidForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "فاکتور خرید کالا - نسخه مینی"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlProductAdd.ResumeLayout(False)
            Me.pnlProductAdd.PerformLayout()
            CType(Me.numQuantity, ISupportInitialize).EndInit()
            CType(Me.dgvItems, ISupportInitialize).EndInit()
            Me.pnlFooter.ResumeLayout(False)
            Me.pnlFooter.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As Panel
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblInvoiceNo As Label
        Friend WithEvents txtInvoiceNo As TextBox
        Friend WithEvents lblInvoiceDate As Label
        Friend WithEvents txtInvoiceDate As TextBox
        Friend WithEvents btnPickDate As Button
        Friend WithEvents lblVendor As Label
        Friend WithEvents txtVendorName As TextBox
        Friend WithEvents btnPickVendor As Button
        Friend WithEvents lblWarehouse As Label
        Friend WithEvents cmbWarehouse As ComboBox

        Friend WithEvents pnlProductAdd As Panel
        Friend WithEvents lblSearch As Label
        Friend WithEvents txtProductSearch As TextBox
        Friend WithEvents lblQty As Label
        Friend WithEvents numQuantity As NumericUpDown
        Friend WithEvents lblBuyPrice As Label
        Friend WithEvents txtUnitPrice As TextBox
        Friend WithEvents btnAdd As Button
        Friend WithEvents btnBrowseProduct As Button

        Friend WithEvents dgvItems As DataGridView
        Friend WithEvents colProductID As DataGridViewTextBoxColumn
        Friend WithEvents colCode As DataGridViewTextBoxColumn
        Friend WithEvents colName As DataGridViewTextBoxColumn
        Friend WithEvents colQuantity As DataGridViewTextBoxColumn
        Friend WithEvents colUnitPrice As DataGridViewTextBoxColumn
        Friend WithEvents colTotalPrice As DataGridViewTextBoxColumn

        Friend WithEvents pnlFooter As Panel
        Friend WithEvents lblTotalText As Label
        Friend WithEvents lblTotalAmount As Label
        Friend WithEvents btnSave As Button
    End Class
End Namespace
