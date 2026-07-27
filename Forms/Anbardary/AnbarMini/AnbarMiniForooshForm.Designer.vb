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
            Me.lblCustomer = New Label()
            Me.txtCustomerName = New TextBox()
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
            Me.lblTotalPayable = New Label()
            Me.lblTotalAmountValue = New Label()
            Me.lblPaymentType = New Label()
            Me.cmbPaymentType = New ComboBox()
            Me.btnSaveAndPrint = New Button()
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
            Me.pnlHeader.Controls.Add(Me.lblCustomer)
            Me.pnlHeader.Controls.Add(Me.txtCustomerName)
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
            Me.lblInvoiceDate.Location = New Point(585, 20)
            Me.lblInvoiceDate.Text = "تاریخ:"

            ' txtInvoiceDate
            Me.txtInvoiceDate.Location = New Point(480, 17)
            Me.txtInvoiceDate.Name = "txtInvoiceDate"
            Me.txtInvoiceDate.Size = New Size(100, 27)

            ' lblCustomer
            Me.lblCustomer.AutoSize = True
            Me.lblCustomer.ForeColor = Color.White
            Me.lblCustomer.Location = New Point(400, 20)
            Me.lblCustomer.Text = "خریدار:"

            ' txtCustomerName
            Me.txtCustomerName.Location = New Point(180, 17)
            Me.txtCustomerName.Name = "txtCustomerName"
            Me.txtCustomerName.Size = New Size(215, 27)
            Me.txtCustomerName.Text = "مشتری نقدی"

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
            Me.colUnitPrice.HeaderText = "قیمت واحد (ریال)"
            Me.colUnitPrice.Name = "colUnitPrice"

            ' colTotalPrice
            Me.colTotalPrice.HeaderText = "جمع کل (ریال)"
            Me.colTotalPrice.Name = "colTotalPrice"
            Me.colTotalPrice.ReadOnly = True

            ' pnlFooter
            Me.pnlFooter.BackColor = Color.FromArgb(44, 62, 80)
            Me.pnlFooter.Controls.Add(Me.lblTotalPayable)
            Me.pnlFooter.Controls.Add(Me.lblTotalAmountValue)
            Me.pnlFooter.Controls.Add(Me.lblPaymentType)
            Me.pnlFooter.Controls.Add(Me.cmbPaymentType)
            Me.pnlFooter.Controls.Add(Me.btnSaveAndPrint)
            Me.pnlFooter.Controls.Add(Me.btnNewInvoice)
            Me.pnlFooter.Dock = DockStyle.Bottom
            Me.pnlFooter.Location = New Point(0, 530)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New Size(950, 70)

            ' lblTotalPayable
            Me.lblTotalPayable.AutoSize = True
            Me.lblTotalPayable.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.lblTotalPayable.ForeColor = Color.White
            Me.lblTotalPayable.Location = New Point(830, 22)
            Me.lblTotalPayable.Name = "lblTotalPayable"
            Me.lblTotalPayable.Size = New Size(105, 27)
            Me.lblTotalPayable.Text = "مبلغ قابل پرداخت:"

            ' lblTotalAmountValue
            Me.lblTotalAmountValue.AutoSize = True
            Me.lblTotalAmountValue.Font = New Font("B Yekan", 16.0!, FontStyle.Bold)
            Me.lblTotalAmountValue.ForeColor = Color.FromArgb(46, 204, 113)
            Me.lblTotalAmountValue.Location = New Point(600, 15)
            Me.lblTotalAmountValue.Name = "lblTotalAmountValue"
            Me.lblTotalAmountValue.Size = New Size(80, 41)
            Me.lblTotalAmountValue.Text = "۰ ریال"

            ' lblPaymentType
            Me.lblPaymentType.AutoSize = True
            Me.lblPaymentType.ForeColor = Color.White
            Me.lblPaymentType.Location = New Point(510, 24)
            Me.lblPaymentType.Text = "پرداخت:"

            ' cmbPaymentType
            Me.cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbPaymentType.FormattingEnabled = True
            Me.cmbPaymentType.Items.AddRange(New Object() {"کارتخوان (POS)", "نقدی", "چک", "نسیه"})
            Me.cmbPaymentType.Location = New Point(360, 21)
            Me.cmbPaymentType.Name = "cmbPaymentType"
            Me.cmbPaymentType.Size = New Size(140, 27)

            ' btnSaveAndPrint
            Me.btnSaveAndPrint.BackColor = Color.FromArgb(46, 204, 113)
            Me.btnSaveAndPrint.FlatAppearance.BorderSize = 0
            Me.btnSaveAndPrint.FlatStyle = FlatStyle.Flat
            Me.btnSaveAndPrint.Font = New Font("B Yekan", 11.0!, FontStyle.Bold)
            Me.btnSaveAndPrint.ForeColor = Color.White
            Me.btnSaveAndPrint.Location = New Point(160, 12)
            Me.btnSaveAndPrint.Name = "btnSaveAndPrint"
            Me.btnSaveAndPrint.Size = New Size(170, 45)
            Me.btnSaveAndPrint.Text = "ثبت و چاپ (F2)"
            Me.btnSaveAndPrint.UseVisualStyleBackColor = False

            ' btnNewInvoice
            Me.btnNewInvoice.BackColor = Color.FromArgb(149, 165, 166)
            Me.btnNewInvoice.FlatAppearance.BorderSize = 0
            Me.btnNewInvoice.FlatStyle = FlatStyle.Flat
            Me.btnNewInvoice.Font = New Font("B Yekan", 10.0!)
            Me.btnNewInvoice.ForeColor = Color.White
            Me.btnNewInvoice.Location = New Point(20, 12)
            Me.btnNewInvoice.Name = "btnNewInvoice"
            Me.btnNewInvoice.Size = New Size(130, 45)
            Me.btnNewInvoice.Text = "فاکتور جدید (F3)"
            Me.btnNewInvoice.UseVisualStyleBackColor = False

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
        Friend WithEvents lblCustomer As Label
        Friend WithEvents txtCustomerName As TextBox

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
        Friend WithEvents lblTotalPayable As Label
        Friend WithEvents lblTotalAmountValue As Label
        Friend WithEvents lblPaymentType As Label
        Friend WithEvents cmbPaymentType As ComboBox
        Friend WithEvents btnSaveAndPrint As Button
        Friend WithEvents btnNewInvoice As Button
    End Class
End Namespace
