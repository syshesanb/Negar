Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniForooshForm
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
            Me.btnPickDate = New Button()
            Me.lblCustomer = New Label()
            Me.txtCustomerName = New TextBox()
            Me.btnPickCustomer = New Button()
            Me.lblWarehouse = New Label()
            Me.cmbWarehouse = New ComboBox()

            Me.pnlScan = New Panel()
            Me.lblScan = New Label()
            Me.txtBarcodeScan = New TextBox()
            Me.btnAddProduct = New Button()
            Me.btnBrowseProduct = New Button()

            Me.dgvCart = New DataGridView()
            Me.colProductID = New DataGridViewTextBoxColumn()
            Me.colCode = New DataGridViewTextBoxColumn()
            Me.colName = New DataGridViewTextBoxColumn()
            Me.colUnit = New DataGridViewTextBoxColumn()
            Me.colQuantity = New DataGridViewTextBoxColumn()
            Me.colUnitPrice = New DataGridViewTextBoxColumn()
            Me.colTotalPrice = New DataGridViewTextBoxColumn()

            Me.pnlFooter = New Panel()
            Me.lblDescription = New Label()
            Me.txtDescription = New TextBox()
            Me.lblTotalPayable = New Label()
            Me.lblTotalAmountValue = New Label()
            Me.lblPaymentType = New Label()
            Me.cmbPaymentType = New ComboBox()
            Me.btnSaveAndPrint = New Button()
            Me.btnCancel = New Button()
            Me.btnNewInvoice = New Button()

            Me.pnlHeader.SuspendLayout()
            Me.pnlScan.SuspendLayout()
            CType(Me.dgvCart, ISupportInitialize).BeginInit()
            Me.pnlFooter.SuspendLayout()
            Me.SuspendLayout()

            ' pnlHeader
            Me.pnlHeader.BackColor = Color.FromArgb(41, 128, 185)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Controls.Add(Me.lblInvoiceNo)
            Me.pnlHeader.Controls.Add(Me.txtInvoiceNo)
            Me.pnlHeader.Controls.Add(Me.lblInvoiceDate)
            Me.pnlHeader.Controls.Add(Me.txtInvoiceDate)
            Me.pnlHeader.Controls.Add(Me.btnPickDate)
            Me.pnlHeader.Controls.Add(Me.lblCustomer)
            Me.pnlHeader.Controls.Add(Me.txtCustomerName)
            Me.pnlHeader.Controls.Add(Me.btnPickCustomer)
            Me.pnlHeader.Controls.Add(Me.lblWarehouse)
            Me.pnlHeader.Controls.Add(Me.cmbWarehouse)
            Me.pnlHeader.Dock = DockStyle.Top
            Me.pnlHeader.Location = New Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New Size(950, 60)

            ' lblTitle
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New Font("B Yekan", 12.0!, FontStyle.Bold, GraphicsUnit.Point, CByte(178))
            Me.lblTitle.ForeColor = Color.White
            Me.lblTitle.Location = New Point(800, 15)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(135, 29)
            Me.lblTitle.Text = "فروش سریع (POS)"

            ' lblInvoiceNo
            Me.lblInvoiceNo.AutoSize = True
            Me.lblInvoiceNo.ForeColor = Color.White
            Me.lblInvoiceNo.Location = New Point(730, 20)
            Me.lblInvoiceNo.Text = "شماره:"

            ' txtInvoiceNo
            Me.txtInvoiceNo.Location = New Point(640, 17)
            Me.txtInvoiceNo.Name = "txtInvoiceNo"
            Me.txtInvoiceNo.ReadOnly = True
            Me.txtInvoiceNo.Size = New Size(85, 27)

            ' lblInvoiceDate
            Me.lblInvoiceDate.AutoSize = True
            Me.lblInvoiceDate.ForeColor = Color.White
            Me.lblInvoiceDate.Location = New Point(595, 20)
            Me.lblInvoiceDate.Text = "تاریخ:"

            ' txtInvoiceDate
            Me.txtInvoiceDate.Location = New Point(505, 17)
            Me.txtInvoiceDate.Name = "txtInvoiceDate"
            Me.txtInvoiceDate.Size = New Size(85, 27)

            ' btnPickDate
            Me.btnPickDate.BackColor = Color.White
            Me.btnPickDate.FlatStyle = FlatStyle.Flat
            Me.btnPickDate.Font = New Font("Tahoma", 8.0!, FontStyle.Bold)
            Me.btnPickDate.ForeColor = Color.Black
            Me.btnPickDate.Location = New Point(470, 17)
            Me.btnPickDate.Name = "btnPickDate"
            Me.btnPickDate.Size = New Size(30, 27)
            Me.btnPickDate.Text = "..."
            Me.btnPickDate.UseVisualStyleBackColor = False

            ' lblCustomer
            Me.lblCustomer.AutoSize = True
            Me.lblCustomer.ForeColor = Color.White
            Me.lblCustomer.Location = New Point(415, 20)
            Me.lblCustomer.Text = "خریدار:"

            ' txtCustomerName
            Me.txtCustomerName.Location = New Point(265, 17)
            Me.txtCustomerName.Name = "txtCustomerName"
            Me.txtCustomerName.Size = New Size(145, 27)
            Me.txtCustomerName.Text = "مشتری نقدی"

            ' btnPickCustomer
            Me.btnPickCustomer.BackColor = Color.White
            Me.btnPickCustomer.FlatStyle = FlatStyle.Flat
            Me.btnPickCustomer.Font = New Font("Tahoma", 8.0!, FontStyle.Bold)
            Me.btnPickCustomer.ForeColor = Color.Black
            Me.btnPickCustomer.Location = New Point(230, 17)
            Me.btnPickCustomer.Name = "btnPickCustomer"
            Me.btnPickCustomer.Size = New Size(30, 27)
            Me.btnPickCustomer.Text = "..."
            Me.btnPickCustomer.UseVisualStyleBackColor = False

            ' lblWarehouse
            Me.lblWarehouse.AutoSize = True
            Me.lblWarehouse.ForeColor = Color.White
            Me.lblWarehouse.Location = New Point(180, 20)
            Me.lblWarehouse.Text = "انبار:"

            ' cmbWarehouse
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouse.FormattingEnabled = True
            Me.cmbWarehouse.Location = New Point(10, 17)
            Me.cmbWarehouse.Name = "cmbWarehouse"
            Me.cmbWarehouse.Size = New Size(165, 27)

            ' pnlScan
            Me.pnlScan.BackColor = Color.FromArgb(236, 240, 241)
            Me.pnlScan.Controls.Add(Me.lblScan)
            Me.pnlScan.Controls.Add(Me.txtBarcodeScan)
            Me.pnlScan.Controls.Add(Me.btnAddProduct)
            Me.pnlScan.Controls.Add(Me.btnBrowseProduct)
            Me.pnlScan.Dock = DockStyle.Top
            Me.pnlScan.Location = New Point(0, 60)
            Me.pnlScan.Name = "pnlScan"
            Me.pnlScan.Size = New Size(950, 50)

            ' lblScan
            Me.lblScan.AutoSize = True
            Me.lblScan.Font = New Font("B Yekan", 10.0!, FontStyle.Bold)
            Me.lblScan.Location = New Point(790, 13)
            Me.lblScan.Name = "lblScan"
            Me.lblScan.Size = New Size(145, 25)
            Me.lblScan.Text = "اسکن بارکد / کد کالا:"

            ' txtBarcodeScan
            Me.txtBarcodeScan.Font = New Font("B Yekan", 11.0!)
            Me.txtBarcodeScan.Location = New Point(230, 9)
            Me.txtBarcodeScan.Name = "txtBarcodeScan"
            Me.txtBarcodeScan.Size = New Size(550, 32)

            ' btnAddProduct
            Me.btnAddProduct.BackColor = Color.FromArgb(52, 152, 219)
            Me.btnAddProduct.FlatStyle = FlatStyle.Flat
            Me.btnAddProduct.ForeColor = Color.White
            Me.btnAddProduct.Location = New Point(115, 9)
            Me.btnAddProduct.Name = "btnAddProduct"
            Me.btnAddProduct.Size = New Size(105, 32)
            Me.btnAddProduct.Text = "افزودن (Enter)"
            Me.btnAddProduct.UseVisualStyleBackColor = False

            ' btnBrowseProduct
            Me.btnBrowseProduct.BackColor = Color.FromArgb(39, 174, 96)
            Me.btnBrowseProduct.FlatStyle = FlatStyle.Flat
            Me.btnBrowseProduct.ForeColor = Color.White
            Me.btnBrowseProduct.Location = New Point(10, 9)
            Me.btnBrowseProduct.Name = "btnBrowseProduct"
            Me.btnBrowseProduct.Size = New Size(100, 32)
            Me.btnBrowseProduct.Text = "جستجو (F2)"
            Me.btnBrowseProduct.UseVisualStyleBackColor = False

            ' dgvCart
            Me.dgvCart.AllowUserToAddRows = False
            Me.dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvCart.Columns.AddRange(New DataGridViewColumn() {Me.colProductID, Me.colCode, Me.colName, Me.colUnit, Me.colQuantity, Me.colUnitPrice, Me.colTotalPrice})
            Me.dgvCart.Dock = DockStyle.Fill
            Me.dgvCart.Location = New Point(0, 110)
            Me.dgvCart.Name = "dgvCart"
            Me.dgvCart.RowTemplate.Height = 30
            Me.dgvCart.Size = New Size(950, 420)

            ' colProductID
            Me.colProductID.HeaderText = "شناسه"
            Me.colProductID.Name = "colProductID"
            Me.colProductID.Visible = False

            ' colCode
            Me.colCode.HeaderText = "کد کالا"
            Me.colCode.Name = "colCode"
            Me.colCode.ReadOnly = True

            ' colName
            Me.colName.HeaderText = "نام کالا"
            Me.colName.Name = "colName"
            Me.colName.ReadOnly = True

            ' colUnit
            Me.colUnit.HeaderText = "واحد"
            Me.colUnit.Name = "colUnit"
            Me.colUnit.ReadOnly = True

            ' colQuantity
            Me.colQuantity.HeaderText = "تعداد"
            Me.colQuantity.Name = "colQuantity"

            ' colUnitPrice
            Me.colUnitPrice.HeaderText = "قیمت فروش واحد (ریال)"
            Me.colUnitPrice.Name = "colUnitPrice"

            ' colTotalPrice
            Me.colTotalPrice.HeaderText = "جمع کل (ریال)"
            Me.colTotalPrice.Name = "colTotalPrice"
            Me.colTotalPrice.ReadOnly = True

            ' pnlFooter
            Me.pnlFooter.BackColor = Color.FromArgb(44, 62, 80)
            Me.pnlFooter.Controls.Add(Me.lblDescription)
            Me.pnlFooter.Controls.Add(Me.txtDescription)
            Me.pnlFooter.Controls.Add(Me.lblTotalPayable)
            Me.pnlFooter.Controls.Add(Me.lblTotalAmountValue)
            Me.pnlFooter.Controls.Add(Me.lblPaymentType)
            Me.pnlFooter.Controls.Add(Me.cmbPaymentType)
            Me.pnlFooter.Controls.Add(Me.btnSaveAndPrint)
            Me.pnlFooter.Controls.Add(Me.btnCancel)
            Me.pnlFooter.Controls.Add(Me.btnNewInvoice)
            Me.pnlFooter.Dock = DockStyle.Bottom
            Me.pnlFooter.Location = New Point(0, 530)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New Size(950, 70)

            ' lblDescription
            Me.lblDescription.AutoSize = True
            Me.lblDescription.Font = New Font("B Yekan", 10.0!, FontStyle.Bold)
            Me.lblDescription.ForeColor = Color.White
            Me.lblDescription.Location = New Point(870, 22)
            Me.lblDescription.Text = "توضیحات:"

            ' txtDescription
            Me.txtDescription.Font = New Font("B Yekan", 9.0!)
            Me.txtDescription.Location = New Point(690, 10)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.ScrollBars = ScrollBars.Vertical
            Me.txtDescription.Size = New Size(175, 52)

            ' lblTotalPayable
            Me.lblTotalPayable.AutoSize = True
            Me.lblTotalPayable.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.lblTotalPayable.ForeColor = Color.White
            Me.lblTotalPayable.Location = New Point(570, 22)
            Me.lblTotalPayable.Text = "مبلغ قابل پرداخت:"

            ' lblTotalAmountValue
            Me.lblTotalAmountValue.AutoSize = True
            Me.lblTotalAmountValue.Font = New Font("B Yekan", 15.0!, FontStyle.Bold)
            Me.lblTotalAmountValue.ForeColor = Color.FromArgb(46, 204, 113)
            Me.lblTotalAmountValue.Location = New Point(430, 16)
            Me.lblTotalAmountValue.Text = "۰ ریال"

            ' lblPaymentType
            Me.lblPaymentType.AutoSize = True
            Me.lblPaymentType.ForeColor = Color.White
            Me.lblPaymentType.Location = New Point(365, 22)
            Me.lblPaymentType.Text = "پرداخت:"

            ' cmbPaymentType
            Me.cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbPaymentType.FormattingEnabled = True
            Me.cmbPaymentType.Items.AddRange(New Object() {"کارتخوان (POS)", "نقدی", "چک", "نسیه"})
            Me.cmbPaymentType.Location = New Point(255, 20)
            Me.cmbPaymentType.Name = "cmbPaymentType"
            Me.cmbPaymentType.Size = New Size(105, 27)

            ' btnSaveAndPrint
            Me.btnSaveAndPrint.BackColor = Color.FromArgb(46, 204, 113)
            Me.btnSaveAndPrint.FlatAppearance.BorderSize = 0
            Me.btnSaveAndPrint.FlatStyle = FlatStyle.Flat
            Me.btnSaveAndPrint.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.btnSaveAndPrint.ForeColor = Color.White
            Me.btnSaveAndPrint.Location = New Point(125, 12)
            Me.btnSaveAndPrint.Name = "btnSaveAndPrint"
            Me.btnSaveAndPrint.Size = New Size(120, 45)
            Me.btnSaveAndPrint.Text = "ثبت و چاپ (F2)"
            Me.btnSaveAndPrint.UseVisualStyleBackColor = False

            ' btnCancel
            Me.btnCancel.BackColor = Color.FromArgb(108, 122, 137)
            Me.btnCancel.FlatAppearance.BorderSize = 0
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.Font = New Font("B Yekan", 10.0!, FontStyle.Bold)
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(10, 12)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(110, 45)
            Me.btnCancel.Text = "بازگشت به لیست"
            Me.btnCancel.UseVisualStyleBackColor = False

            ' btnNewInvoice
            Me.btnNewInvoice.BackColor = Color.FromArgb(149, 165, 166)
            Me.btnNewInvoice.Visible = False

            ' Form Setup
            Me.AutoScaleDimensions = New SizeF(8.0!, 19.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.dgvCart)
            Me.Controls.Add(Me.pnlFooter)
            Me.Controls.Add(Me.pnlScan)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New Font("B Yekan", 9.0!)
            Me.KeyPreview = True
            Me.Name = "AnbarMiniForooshForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "فروش سریع (POS) - نسخه فروشگاهی مینی"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlScan.ResumeLayout(False)
            Me.pnlScan.PerformLayout()
            CType(Me.dgvCart, ISupportInitialize).EndInit()
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
        Friend WithEvents lblCustomer As Label
        Friend WithEvents txtCustomerName As TextBox
        Friend WithEvents btnPickCustomer As Button
        Friend WithEvents lblWarehouse As Label
        Friend WithEvents cmbWarehouse As ComboBox

        Friend WithEvents pnlScan As Panel
        Friend WithEvents lblScan As Label
        Friend WithEvents txtBarcodeScan As TextBox
        Friend WithEvents btnAddProduct As Button
        Friend WithEvents btnBrowseProduct As Button

        Friend WithEvents dgvCart As DataGridView
        Friend WithEvents colProductID As DataGridViewTextBoxColumn
        Friend WithEvents colCode As DataGridViewTextBoxColumn
        Friend WithEvents colName As DataGridViewTextBoxColumn
        Friend WithEvents colUnit As DataGridViewTextBoxColumn
        Friend WithEvents colQuantity As DataGridViewTextBoxColumn
        Friend WithEvents colUnitPrice As DataGridViewTextBoxColumn
        Friend WithEvents colTotalPrice As DataGridViewTextBoxColumn

        Friend WithEvents pnlFooter As Panel
        Friend WithEvents lblDescription As Label
        Friend WithEvents txtDescription As TextBox
        Friend WithEvents lblTotalPayable As Label
        Friend WithEvents lblTotalAmountValue As Label
        Friend WithEvents lblPaymentType As Label
        Friend WithEvents cmbPaymentType As ComboBox
        Friend WithEvents btnSaveAndPrint As Button
        Friend WithEvents btnCancel As Button
        Friend WithEvents btnNewInvoice As Button
    End Class
End Namespace
