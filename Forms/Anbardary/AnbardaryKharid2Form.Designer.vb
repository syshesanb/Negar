Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryKharid2Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents topPanel As Panel
        Friend WithEvents centerPanel As Panel
        Friend WithEvents bottomPanel As Panel

        Friend WithEvents lblInvoiceNumber As Label
        Friend WithEvents txtInvoiceNumber As TextBox
        Friend WithEvents lblInvoiceDate As Label
        Friend WithEvents dtpInvoiceDate As DateTimePicker
        Friend WithEvents lblInvoiceDatePersian As Label
        Friend WithEvents lblParty As Label
        Friend WithEvents txtPartyName As TextBox
        Friend WithEvents lblWarehouse As Label
        Friend WithEvents cmbWarehouse As ComboBox

        Friend WithEvents grpAddProduct As GroupBox
        Friend WithEvents lblProduct As Label
        Friend WithEvents cmbProduct As ComboBox
        Friend WithEvents lblQuantity As Label
        Friend WithEvents txtQuantity As TextBox
        Friend WithEvents lblUnitPrice As Label
        Friend WithEvents txtUnitPrice As TextBox
        Friend WithEvents btnAddLine As Button

        Friend WithEvents dgvLines As DataGridView
        Friend WithEvents btnRemoveLine As Button
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button
        Friend WithEvents lblTotal As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.topPanel = New Panel()
            Me.centerPanel = New Panel()
            Me.bottomPanel = New Panel()
            Me.lblInvoiceNumber = New Label()
            Me.txtInvoiceNumber = New TextBox()
            Me.lblInvoiceDate = New Label()
            Me.dtpInvoiceDate = New DateTimePicker()
            Me.lblInvoiceDatePersian = New Label()
            Me.lblParty = New Label()
            Me.txtPartyName = New TextBox()
            Me.lblWarehouse = New Label()
            Me.cmbWarehouse = New ComboBox()
            Me.grpAddProduct = New GroupBox()
            Me.lblProduct = New Label()
            Me.cmbProduct = New ComboBox()
            Me.lblQuantity = New Label()
            Me.txtQuantity = New TextBox()
            Me.lblUnitPrice = New Label()
            Me.txtUnitPrice = New TextBox()
            Me.btnAddLine = New Button()
            Me.dgvLines = New DataGridView()
            Me.btnRemoveLine = New Button()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.lblTotal = New Label()
            Me.topPanel.SuspendLayout()
            Me.centerPanel.SuspendLayout()
            Me.bottomPanel.SuspendLayout()
            Me.grpAddProduct.SuspendLayout()
            CType(Me.dgvLines, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'topPanel
            '
            Me.topPanel.BackColor = Color.FromArgb(235, 240, 250)
            Me.topPanel.Controls.Add(Me.grpAddProduct)
            Me.topPanel.Controls.Add(Me.lblInvoiceNumber)
            Me.topPanel.Controls.Add(Me.txtInvoiceNumber)
            Me.topPanel.Controls.Add(Me.lblInvoiceDate)
            Me.topPanel.Controls.Add(Me.dtpInvoiceDate)
            Me.topPanel.Controls.Add(Me.lblInvoiceDatePersian)
            Me.topPanel.Controls.Add(Me.lblParty)
            Me.topPanel.Controls.Add(Me.txtPartyName)
            Me.topPanel.Controls.Add(Me.lblWarehouse)
            Me.topPanel.Controls.Add(Me.cmbWarehouse)
            Me.topPanel.Dock = DockStyle.Top
            Me.topPanel.Location = New Point(0, 0)
            Me.topPanel.Name = "topPanel"
            Me.topPanel.Size = New Size(950, 180)
            Me.topPanel.TabIndex = 0
            '
            'lblInvoiceNumber
            '
            Me.lblInvoiceNumber.Location = New Point(12, 18)
            Me.lblInvoiceNumber.Name = "lblInvoiceNumber"
            Me.lblInvoiceNumber.Size = New Size(90, 20)
            Me.lblInvoiceNumber.Text = "شماره فاکتور:"
            Me.lblInvoiceNumber.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtInvoiceNumber
            '
            Me.txtInvoiceNumber.Location = New Point(108, 15)
            Me.txtInvoiceNumber.Name = "txtInvoiceNumber"
            Me.txtInvoiceNumber.Size = New Size(140, 22)
            Me.txtInvoiceNumber.TabIndex = 1
            '
            'lblInvoiceDate
            '
            Me.lblInvoiceDate.Location = New Point(265, 18)
            Me.lblInvoiceDate.Name = "lblInvoiceDate"
            Me.lblInvoiceDate.Size = New Size(50, 20)
            Me.lblInvoiceDate.Text = "تاریخ:"
            Me.lblInvoiceDate.TextAlign = ContentAlignment.MiddleLeft
            '
            'dtpInvoiceDate
            '
            Me.dtpInvoiceDate.CustomFormat = " "
            Me.dtpInvoiceDate.Format = DateTimePickerFormat.Custom
            Me.dtpInvoiceDate.Location = New Point(321, 15)
            Me.dtpInvoiceDate.Name = "dtpInvoiceDate"
            Me.dtpInvoiceDate.Size = New Size(26, 22)
            Me.dtpInvoiceDate.TabIndex = 2
            '
            'lblInvoiceDatePersian
            '
            Me.lblInvoiceDatePersian.BorderStyle = BorderStyle.Fixed3D
            Me.lblInvoiceDatePersian.Location = New Point(353, 15)
            Me.lblInvoiceDatePersian.Name = "lblInvoiceDatePersian"
            Me.lblInvoiceDatePersian.Size = New Size(120, 22)
            Me.lblInvoiceDatePersian.TextAlign = ContentAlignment.MiddleCenter
            Me.lblInvoiceDatePersian.Font = New Font("Courier New", 9.0!, FontStyle.Bold)
            '
            'lblParty
            '
            Me.lblParty.Location = New Point(490, 18)
            Me.lblParty.Name = "lblParty"
            Me.lblParty.Size = New Size(80, 20)
            Me.lblParty.Text = "فروشنده:"
            Me.lblParty.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtPartyName
            '
            Me.txtPartyName.Location = New Point(576, 15)
            Me.txtPartyName.Name = "txtPartyName"
            Me.txtPartyName.Size = New Size(160, 22)
            Me.txtPartyName.TabIndex = 3
            '
            'lblWarehouse
            '
            Me.lblWarehouse.Location = New Point(752, 18)
            Me.lblWarehouse.Name = "lblWarehouse"
            Me.lblWarehouse.Size = New Size(40, 20)
            Me.lblWarehouse.Text = "انبار:"
            Me.lblWarehouse.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbWarehouse
            '
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouse.Location = New Point(798, 15)
            Me.cmbWarehouse.Name = "cmbWarehouse"
            Me.cmbWarehouse.Size = New Size(140, 22)
            Me.cmbWarehouse.TabIndex = 4
            '
            'grpAddProduct
            '
            Me.grpAddProduct.Controls.Add(Me.lblProduct)
            Me.grpAddProduct.Controls.Add(Me.cmbProduct)
            Me.grpAddProduct.Controls.Add(Me.lblQuantity)
            Me.grpAddProduct.Controls.Add(Me.txtQuantity)
            Me.grpAddProduct.Controls.Add(Me.lblUnitPrice)
            Me.grpAddProduct.Controls.Add(Me.txtUnitPrice)
            Me.grpAddProduct.Controls.Add(Me.btnAddLine)
            Me.grpAddProduct.Location = New Point(12, 53)
            Me.grpAddProduct.Name = "grpAddProduct"
            Me.grpAddProduct.Size = New Size(926, 110)
            Me.grpAddProduct.TabIndex = 5
            Me.grpAddProduct.TabStop = False
            Me.grpAddProduct.Text = "افزودن کالا به فاکتور"
            '
            'lblProduct
            '
            Me.lblProduct.Location = New Point(15, 45)
            Me.lblProduct.Name = "lblProduct"
            Me.lblProduct.Size = New Size(60, 20)
            Me.lblProduct.Text = "کالا:"
            Me.lblProduct.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbProduct
            '
            Me.cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbProduct.Location = New Point(81, 42)
            Me.cmbProduct.Name = "cmbProduct"
            Me.cmbProduct.Size = New Size(280, 22)
            Me.cmbProduct.TabIndex = 0
            '
            'lblQuantity
            '
            Me.lblQuantity.Location = New Point(380, 45)
            Me.lblQuantity.Name = "lblQuantity"
            Me.lblQuantity.Size = New Size(50, 20)
            Me.lblQuantity.Text = "مقدار:"
            Me.lblQuantity.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtQuantity
            '
            Me.txtQuantity.Location = New Point(436, 42)
            Me.txtQuantity.Name = "txtQuantity"
            Me.txtQuantity.Size = New Size(100, 22)
            Me.txtQuantity.TabIndex = 1
            '
            'lblUnitPrice
            '
            Me.lblUnitPrice.Location = New Point(555, 45)
            Me.lblUnitPrice.Name = "lblUnitPrice"
            Me.lblUnitPrice.Size = New Size(40, 20)
            Me.lblUnitPrice.Text = "فی:"
            Me.lblUnitPrice.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtUnitPrice
            '
            Me.txtUnitPrice.Location = New Point(601, 42)
            Me.txtUnitPrice.Name = "txtUnitPrice"
            Me.txtUnitPrice.Size = New Size(140, 22)
            Me.txtUnitPrice.TabIndex = 2
            '
            'btnAddLine
            '
            Me.btnAddLine.Location = New Point(765, 38)
            Me.btnAddLine.Name = "btnAddLine"
            Me.btnAddLine.Size = New Size(140, 30)
            Me.btnAddLine.TabIndex = 3
            Me.btnAddLine.Text = "افزودن به لیست"
            Me.btnAddLine.UseVisualStyleBackColor = True
            '
            'centerPanel
            '
            Me.centerPanel.Controls.Add(Me.dgvLines)
            Me.centerPanel.Dock = DockStyle.Fill
            Me.centerPanel.Location = New Point(0, 180)
            Me.centerPanel.Name = "centerPanel"
            Me.centerPanel.Padding = New Padding(12)
            Me.centerPanel.Size = New Size(950, 390)
            Me.centerPanel.TabIndex = 1
            '
            'dgvLines
            '
            Me.dgvLines.AllowUserToAddRows = False
            Me.dgvLines.BackgroundColor = Color.White
            Me.dgvLines.ColumnHeadersHeight = 30
            Me.dgvLines.Dock = DockStyle.Fill
            Me.dgvLines.Location = New Point(12, 12)
            Me.dgvLines.MultiSelect = False
            Me.dgvLines.Name = "dgvLines"
            Me.dgvLines.ReadOnly = True
            Me.dgvLines.RowHeadersVisible = False
            Me.dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvLines.Size = New Size(926, 366)
            Me.dgvLines.TabIndex = 0
            '
            'bottomPanel
            '
            Me.bottomPanel.BackColor = Color.FromArgb(235, 240, 250)
            Me.bottomPanel.Controls.Add(Me.btnRemoveLine)
            Me.bottomPanel.Controls.Add(Me.btnSave)
            Me.bottomPanel.Controls.Add(Me.btnCancel)
            Me.bottomPanel.Controls.Add(Me.lblTotal)
            Me.bottomPanel.Dock = DockStyle.Bottom
            Me.bottomPanel.Location = New Point(0, 570)
            Me.bottomPanel.Name = "bottomPanel"
            Me.bottomPanel.Size = New Size(950, 60)
            Me.bottomPanel.TabIndex = 2
            '
            'btnRemoveLine
            '
            Me.btnRemoveLine.BackColor = Color.FromArgb(220, 80, 80)
            Me.btnRemoveLine.FlatStyle = FlatStyle.Flat
            Me.btnRemoveLine.ForeColor = Color.White
            Me.btnRemoveLine.Location = New Point(12, 15)
            Me.btnRemoveLine.Name = "btnRemoveLine"
            Me.btnRemoveLine.Size = New Size(110, 30)
            Me.btnRemoveLine.TabIndex = 0
            Me.btnRemoveLine.Text = "حذف ردیف"
            Me.btnRemoveLine.UseVisualStyleBackColor = False
            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(30, 120, 60)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(135, 15)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(120, 30)
            Me.btnSave.TabIndex = 1
            Me.btnSave.Text = "ثبت فاکتور"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = Color.FromArgb(120, 120, 120)
            Me.btnCancel.FlatStyle = FlatStyle.Flat
            Me.btnCancel.ForeColor = Color.White
            Me.btnCancel.Location = New Point(265, 15)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(100, 30)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'lblTotal
            '
            Me.lblTotal.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblTotal.ForeColor = Color.FromArgb(30, 80, 160)
            Me.lblTotal.Location = New Point(500, 15)
            Me.lblTotal.Name = "lblTotal"
            Me.lblTotal.Size = New Size(438, 30)
            Me.lblTotal.Text = "جمع کل: 0"
            Me.lblTotal.TextAlign = ContentAlignment.MiddleRight
            '
            'AnbardaryKharid2Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 630)
            Me.Controls.Add(Me.centerPanel)
            Me.Controls.Add(Me.topPanel)
            Me.Controls.Add(Me.bottomPanel)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryKharid2Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش فاکتور خرید"
            Me.topPanel.ResumeLayout(False)
            Me.topPanel.PerformLayout()
            Me.centerPanel.ResumeLayout(False)
            Me.bottomPanel.ResumeLayout(False)
            Me.grpAddProduct.ResumeLayout(False)
            Me.grpAddProduct.PerformLayout()
            CType(Me.dgvLines, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
