Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class F1KharidForoshFormBase
        Inherits Form

        Private components As IContainer
        Friend WithEvents txtInvoiceNumber As TextBox
        Friend WithEvents dtpInvoiceDate As DateTimePicker
        Friend WithEvents txtPartyName As TextBox
        Friend WithEvents cmbWarehouse As ComboBox
        Friend WithEvents cmbProduct As ComboBox
        Friend WithEvents txtQuantity As TextBox
        Friend WithEvents txtUnitPrice As TextBox
        Friend WithEvents dgvLines As DataGridView
        Friend WithEvents lblTotal As Label
        Friend WithEvents btnAddLine As Button
        Friend WithEvents btnSave As Button
        Friend WithEvents btnRemoveLine As Button
        Friend WithEvents lblInvoiceNumber As Label
        Friend WithEvents lblInvoiceDate As Label
        Friend WithEvents lblInvoiceDatePersian As Label
        Friend WithEvents lblParty As Label
        Friend WithEvents lblWarehouse As Label
        Friend WithEvents lblProduct As Label
        Friend WithEvents lblQuantity As Label
        Friend WithEvents lblUnitPrice As Label
        Friend WithEvents topPanel As Panel
        Friend WithEvents centerPanel As Panel
        Friend WithEvents bottomPanel As Panel

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
            Me.lblTotal = New Label()
            Me.topPanel.SuspendLayout()
            Me.centerPanel.SuspendLayout()
            Me.bottomPanel.SuspendLayout()
            CType(Me.dgvLines, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'F1KharidForoshFormBase
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1120, 720)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "F1KharidForoshFormBase"
            Me.Text = "فاکتور"
            '
            'topPanel
            '
            Me.topPanel.Dock = DockStyle.Top
            Me.topPanel.Height = 120
            Me.topPanel.Padding = New Padding(8)
            '
            'centerPanel
            '
            Me.centerPanel.Dock = DockStyle.Fill
            Me.centerPanel.Padding = New Padding(8)
            '
            'bottomPanel
            '
            Me.bottomPanel.Dock = DockStyle.Bottom
            Me.bottomPanel.Height = 60
            Me.bottomPanel.Padding = New Padding(8)
            '
            'lblInvoiceNumber
            '
            Me.lblInvoiceNumber.AutoSize = True
            Me.lblInvoiceNumber.Location = New Point(20, 18)
            Me.lblInvoiceNumber.Text = "شماره فاکتور"
            '
            'txtInvoiceNumber
            '
            Me.txtInvoiceNumber.Location = New Point(110, 15)
            Me.txtInvoiceNumber.Width = 170
            '
            'lblInvoiceDate
            '
            Me.lblInvoiceDate.AutoSize = True
            Me.lblInvoiceDate.Location = New Point(300, 18)
            Me.lblInvoiceDate.Text = "تاریخ"
            '
            'dtpInvoiceDate
            '
            Me.dtpInvoiceDate.CustomFormat = " "
            Me.dtpInvoiceDate.Format = DateTimePickerFormat.Custom
            Me.dtpInvoiceDate.Location = New Point(350, 15)
            Me.dtpInvoiceDate.Width = 26
            '
            'lblInvoiceDatePersian
            '
            Me.lblInvoiceDatePersian.AutoSize = False
            Me.lblInvoiceDatePersian.Location = New Point(380, 18)
            Me.lblInvoiceDatePersian.Width = 150
            Me.lblInvoiceDatePersian.Font = New Font("Courier New", 9.0!)
            Me.lblInvoiceDatePersian.ForeColor = Color.DarkBlue
            '
            'lblParty
            '
            Me.lblParty.AutoSize = True
            Me.lblParty.Location = New Point(520, 18)
            Me.lblParty.Text = "طرف"
            '
            'txtPartyName
            '
            Me.txtPartyName.Location = New Point(610, 15)
            Me.txtPartyName.Width = 200
            '
            'lblWarehouse
            '
            Me.lblWarehouse.AutoSize = True
            Me.lblWarehouse.Location = New Point(20, 55)
            Me.lblWarehouse.Text = "انبار"
            '
            'cmbWarehouse
            '
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouse.Location = New Point(110, 52)
            Me.cmbWarehouse.Width = 200
            '
            'lblProduct
            '
            Me.lblProduct.AutoSize = True
            Me.lblProduct.Location = New Point(330, 55)
            Me.lblProduct.Text = "کالا"
            '
            'cmbProduct
            '
            Me.cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbProduct.Location = New Point(390, 52)
            Me.cmbProduct.Width = 220
            '
            'lblQuantity
            '
            Me.lblQuantity.AutoSize = True
            Me.lblQuantity.Location = New Point(630, 55)
            Me.lblQuantity.Text = "مقدار"
            '
            'txtQuantity
            '
            Me.txtQuantity.Location = New Point(680, 52)
            Me.txtQuantity.Width = 90
            '
            'lblUnitPrice
            '
            Me.lblUnitPrice.AutoSize = True
            Me.lblUnitPrice.Location = New Point(790, 55)
            Me.lblUnitPrice.Text = "فی"
            '
            'txtUnitPrice
            '
            Me.txtUnitPrice.Location = New Point(825, 52)
            Me.txtUnitPrice.Width = 100
            '
            'btnAddLine
            '
            Me.btnAddLine.Location = New Point(950, 50)
            Me.btnAddLine.Text = "افزودن ردیف"
            Me.btnAddLine.Width = 120
            '
            'dgvLines
            '
            Me.dgvLines.AllowUserToAddRows = False
            Me.dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvLines.Dock = DockStyle.Fill
            Me.dgvLines.ReadOnly = True
            Me.dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            '
            'btnRemoveLine
            '
            Me.btnRemoveLine.Location = New Point(10, 12)
            Me.btnRemoveLine.Width = 110
            Me.btnRemoveLine.Text = "حذف ردیف"
            '
            'btnSave
            '
            Me.btnSave.Location = New Point(140, 12)
            Me.btnSave.Width = 120
            Me.btnSave.Text = "ثبت فاکتور"
            '
            'lblTotal
            '
            Me.lblTotal.AutoSize = False
            Me.lblTotal.Dock = DockStyle.Right
            Me.lblTotal.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblTotal.TextAlign = ContentAlignment.MiddleRight
            Me.lblTotal.Width = 320
            Me.lblTotal.Text = "جمع کل: 0"
            '
            'Controls
            '
            Me.topPanel.Controls.AddRange(New Control() {Me.lblInvoiceNumber, Me.txtInvoiceNumber, Me.lblInvoiceDate, Me.dtpInvoiceDate, Me.lblInvoiceDatePersian, Me.lblParty, Me.txtPartyName, Me.lblWarehouse, Me.cmbWarehouse, Me.lblProduct, Me.cmbProduct, Me.lblQuantity, Me.txtQuantity, Me.lblUnitPrice, Me.txtUnitPrice, Me.btnAddLine})
            Me.centerPanel.Controls.Add(Me.dgvLines)
            Me.bottomPanel.Controls.AddRange(New Control() {Me.btnRemoveLine, Me.btnSave, Me.lblTotal})
            Me.Controls.Add(Me.centerPanel)
            Me.Controls.Add(Me.bottomPanel)
            Me.Controls.Add(Me.topPanel)
            Me.topPanel.ResumeLayout(False)
            Me.topPanel.PerformLayout()
            Me.centerPanel.ResumeLayout(False)
            Me.bottomPanel.ResumeLayout(False)
            CType(Me.dgvLines, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
