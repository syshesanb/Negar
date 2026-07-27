Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    Partial Class MojodyAnbarFormRep

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabMain = New TabControl()
            Me.tabInventory = New TabPage()
            Me.tabKardex = New TabPage()
            Me.pnlInventoryTop = New Panel()
            Me.lblWarehouseLabel = New Label()
            Me.cmbWarehouse = New ComboBox()
            Me.btnRefresh = New Button()
            Me.btnPrintInventory = New Button()
            Me.lblInventoryCount = New Label()
            Me.lblSearch = New Label()
            Me.txtSearchInventory = New TextBox()
            Me.dgvInventory = New DataGridView()
            Me.pnlInventoryFooter = New Panel()
            Me.lblGrandTotalText = New Label()
            Me.lblGrandTotalValue = New Label()
            Me.pnlKardexTop = New Panel()
            Me.lblKardexProductLabel = New Label()
            Me.cmbKardexProduct = New ComboBox()
            Me.lblKardexWarehouseLabel = New Label()
            Me.cmbKardexWarehouse = New ComboBox()
            Me.lblKardexFromLabel = New Label()
            Me.txtKardexFrom = New TextBox()
            Me.lblKardexToLabel = New Label()
            Me.txtKardexTo = New TextBox()
            Me.btnKardexLoad = New Button()
            Me.btnPrintKardex = New Button()
            Me.lblKardexTitle = New Label()
            Me.lblKardexCount = New Label()
            Me.dgvKardex = New DataGridView()

            Me.tabMain.SuspendLayout()
            CType(Me.dgvInventory, ISupportInitialize).BeginInit()
            CType(Me.dgvKardex, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            ' ---------- tabMain ----------
            Me.tabMain.Dock = DockStyle.Fill
            Me.tabMain.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.RightToLeft = RightToLeft.Yes
            Me.tabMain.RightToLeftLayout = True
            Me.tabMain.TabPages.Add(Me.tabInventory)
            Me.tabMain.TabPages.Add(Me.tabKardex)

            ' ---------- tabInventory ----------
            Me.tabInventory.Text = "   موجودی انبار   "
            Me.tabInventory.Name = "tabInventory"
            Me.tabInventory.RightToLeft = RightToLeft.Yes
            Me.tabInventory.Controls.Add(Me.dgvInventory)
            Me.tabInventory.Controls.Add(Me.pnlInventoryFooter)
            Me.tabInventory.Controls.Add(Me.pnlInventoryTop)

            ' ---------- pnlInventoryTop ----------
            Me.pnlInventoryTop.Dock = DockStyle.Top
            Me.pnlInventoryTop.Height = 46
            Me.pnlInventoryTop.Name = "pnlInventoryTop"
            Me.pnlInventoryTop.BackColor = Color.FromArgb(235, 245, 252)

            Me.lblWarehouseLabel.Text = "انبار:"
            Me.lblWarehouseLabel.Location = New Point(8, 14)
            Me.lblWarehouseLabel.Size = New Size(45, 20)
            Me.lblWarehouseLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblWarehouseLabel.TextAlign = ContentAlignment.MiddleRight

            Me.cmbWarehouse.Location = New Point(55, 11)
            Me.cmbWarehouse.Size = New Size(160, 24)
            Me.cmbWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.cmbWarehouse.RightToLeft = RightToLeft.Yes
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouse.Name = "cmbWarehouse"

            Me.lblSearch.Text = "جستجو:"
            Me.lblSearch.Location = New Point(230, 14)
            Me.lblSearch.Size = New Size(50, 20)
            Me.lblSearch.Font = New Font("Tahoma", 9.0!)
            Me.lblSearch.Name = "lblSearch"

            Me.txtSearchInventory.Location = New Point(285, 10)
            Me.txtSearchInventory.Size = New Size(180, 26)
            Me.txtSearchInventory.Font = New Font("Tahoma", 9.0!)
            Me.txtSearchInventory.Name = "txtSearchInventory"

            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.Location = New Point(480, 10)
            Me.btnRefresh.Size = New Size(75, 26)
            Me.btnRefresh.Font = New Font("Tahoma", 9.0!)
            Me.btnRefresh.BackColor = Color.FromArgb(60, 130, 75)
            Me.btnRefresh.ForeColor = Color.White
            Me.btnRefresh.FlatStyle = FlatStyle.Flat
            Me.btnRefresh.Name = "btnRefresh"

            Me.btnPrintInventory.Text = "چاپ"
            Me.btnPrintInventory.Location = New Point(560, 10)
            Me.btnPrintInventory.Size = New Size(70, 26)
            Me.btnPrintInventory.Font = New Font("Tahoma", 9.0!)
            Me.btnPrintInventory.BackColor = Color.FromArgb(100, 55, 145)
            Me.btnPrintInventory.ForeColor = Color.White
            Me.btnPrintInventory.FlatStyle = FlatStyle.Flat
            Me.btnPrintInventory.Name = "btnPrintInventory"

            Me.lblInventoryCount.Text = "تعداد اقلام: 0"
            Me.lblInventoryCount.Location = New Point(640, 14)
            Me.lblInventoryCount.Size = New Size(130, 18)
            Me.lblInventoryCount.Font = New Font("Tahoma", 9.0!)
            Me.lblInventoryCount.ForeColor = Color.FromArgb(30, 80, 140)
            Me.lblInventoryCount.Name = "lblInventoryCount"

            Me.pnlInventoryTop.Controls.AddRange(New Control() {
                Me.lblWarehouseLabel, Me.cmbWarehouse, Me.lblSearch, Me.txtSearchInventory,
                Me.btnRefresh, Me.btnPrintInventory, Me.lblInventoryCount
            })

            ' ---------- pnlInventoryFooter ----------
            Me.pnlInventoryFooter.Dock = DockStyle.Bottom
            Me.pnlInventoryFooter.Height = 45
            Me.pnlInventoryFooter.Name = "pnlInventoryFooter"
            Me.pnlInventoryFooter.BackColor = Color.FromArgb(44, 62, 80)

            Me.lblGrandTotalText.Text = "جمع کل بهای تمام شده موجودی:"
            Me.lblGrandTotalText.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblGrandTotalText.ForeColor = Color.White
            Me.lblGrandTotalText.Location = New Point(350, 12)
            Me.lblGrandTotalText.AutoSize = True
            Me.lblGrandTotalText.Name = "lblGrandTotalText"

            Me.lblGrandTotalValue.Text = "۰ ریال"
            Me.lblGrandTotalValue.Font = New Font("Tahoma", 13.0!, FontStyle.Bold)
            Me.lblGrandTotalValue.ForeColor = Color.FromArgb(46, 204, 113)
            Me.lblGrandTotalValue.Location = New Point(100, 8)
            Me.lblGrandTotalValue.AutoSize = True
            Me.lblGrandTotalValue.Name = "lblGrandTotalValue"

            Me.pnlInventoryFooter.Controls.Add(Me.lblGrandTotalText)
            Me.pnlInventoryFooter.Controls.Add(Me.lblGrandTotalValue)

            ' ---------- dgvInventory ----------
            Me.dgvInventory.Dock = DockStyle.Fill
            Me.dgvInventory.Name = "dgvInventory"
            Me.dgvInventory.AllowUserToAddRows = False
            Me.dgvInventory.ReadOnly = True
            Me.dgvInventory.RightToLeft = RightToLeft.Yes

            ' ---------- tabKardex ----------
            Me.tabKardex.Text = "   کاردکس کالا   "
            Me.tabKardex.Name = "tabKardex"
            Me.tabKardex.RightToLeft = RightToLeft.Yes
            Me.tabKardex.Controls.Add(Me.dgvKardex)
            Me.tabKardex.Controls.Add(Me.lblKardexCount)
            Me.tabKardex.Controls.Add(Me.lblKardexTitle)
            Me.tabKardex.Controls.Add(Me.pnlKardexTop)

            ' ---------- pnlKardexTop ----------
            Me.pnlKardexTop.Dock = DockStyle.Top
            Me.pnlKardexTop.Height = 48
            Me.pnlKardexTop.Name = "pnlKardexTop"
            Me.pnlKardexTop.BackColor = Color.FromArgb(235, 248, 255)

            Me.lblKardexProductLabel.Text = "کالا :"
            Me.lblKardexProductLabel.Location = New Point(8, 14)
            Me.lblKardexProductLabel.Size = New Size(42, 20)
            Me.lblKardexProductLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblKardexProductLabel.TextAlign = ContentAlignment.MiddleRight

            Me.cmbKardexProduct.Location = New Point(54, 11)
            Me.cmbKardexProduct.Size = New Size(240, 24)
            Me.cmbKardexProduct.Font = New Font("Tahoma", 9.0!)
            Me.cmbKardexProduct.RightToLeft = RightToLeft.Yes
            Me.cmbKardexProduct.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbKardexProduct.Name = "cmbKardexProduct"

            Me.lblKardexWarehouseLabel.Text = "انبار :"
            Me.lblKardexWarehouseLabel.Location = New Point(302, 14)
            Me.lblKardexWarehouseLabel.Size = New Size(42, 20)
            Me.lblKardexWarehouseLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblKardexWarehouseLabel.TextAlign = ContentAlignment.MiddleRight

            Me.cmbKardexWarehouse.Location = New Point(348, 11)
            Me.cmbKardexWarehouse.Size = New Size(180, 24)
            Me.cmbKardexWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.cmbKardexWarehouse.RightToLeft = RightToLeft.Yes
            Me.cmbKardexWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbKardexWarehouse.Name = "cmbKardexWarehouse"

            Me.lblKardexFromLabel.Text = "از تاریخ:"
            Me.lblKardexFromLabel.Location = New Point(536, 14)
            Me.lblKardexFromLabel.Size = New Size(65, 20)
            Me.lblKardexFromLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblKardexFromLabel.TextAlign = ContentAlignment.MiddleRight

            Me.txtKardexFrom.Location = New Point(605, 11)
            Me.txtKardexFrom.Size = New Size(105, 24)
            Me.txtKardexFrom.Font = New Font("Tahoma", 9.0!)
            Me.txtKardexFrom.RightToLeft = RightToLeft.Yes
            Me.txtKardexFrom.Name = "txtKardexFrom"

            Me.lblKardexToLabel.Text = "تا تاریخ:"
            Me.lblKardexToLabel.Location = New Point(716, 14)
            Me.lblKardexToLabel.Size = New Size(65, 20)
            Me.lblKardexToLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblKardexToLabel.TextAlign = ContentAlignment.MiddleRight

            Me.txtKardexTo.Location = New Point(785, 11)
            Me.txtKardexTo.Size = New Size(105, 24)
            Me.txtKardexTo.Font = New Font("Tahoma", 9.0!)
            Me.txtKardexTo.RightToLeft = RightToLeft.Yes
            Me.txtKardexTo.Name = "txtKardexTo"

            Me.btnKardexLoad.Text = "نمایش کاردکس"
            Me.btnKardexLoad.Location = New Point(896, 10)
            Me.btnKardexLoad.Size = New Size(125, 26)
            Me.btnKardexLoad.Font = New Font("Tahoma", 9.0!)
            Me.btnKardexLoad.BackColor = Color.FromArgb(0, 115, 180)
            Me.btnKardexLoad.ForeColor = Color.White
            Me.btnKardexLoad.FlatStyle = FlatStyle.Flat
            Me.btnKardexLoad.Name = "btnKardexLoad"

            Me.btnPrintKardex.Text = "چاپ"
            Me.btnPrintKardex.Location = New Point(1027, 10)
            Me.btnPrintKardex.Size = New Size(88, 26)
            Me.btnPrintKardex.Font = New Font("Tahoma", 9.0!)
            Me.btnPrintKardex.BackColor = Color.FromArgb(100, 55, 145)
            Me.btnPrintKardex.ForeColor = Color.White
            Me.btnPrintKardex.FlatStyle = FlatStyle.Flat
            Me.btnPrintKardex.Name = "btnPrintKardex"

            Me.pnlKardexTop.Controls.AddRange(New Control() {
                Me.lblKardexProductLabel, Me.cmbKardexProduct,
                Me.lblKardexWarehouseLabel, Me.cmbKardexWarehouse,
                Me.lblKardexFromLabel, Me.txtKardexFrom,
                Me.lblKardexToLabel, Me.txtKardexTo,
                Me.btnKardexLoad, Me.btnPrintKardex
            })

            ' ---------- lblKardexTitle ----------
            Me.lblKardexTitle.Text = "کاردکس کالا"
            Me.lblKardexTitle.Dock = DockStyle.Top
            Me.lblKardexTitle.Height = 26
            Me.lblKardexTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblKardexTitle.ForeColor = Color.FromArgb(20, 70, 130)
            Me.lblKardexTitle.BackColor = Color.FromArgb(215, 232, 252)
            Me.lblKardexTitle.TextAlign = ContentAlignment.MiddleRight
            Me.lblKardexTitle.Padding = New Padding(10, 0, 10, 0)
            Me.lblKardexTitle.Name = "lblKardexTitle"

            ' ---------- lblKardexCount ----------
            Me.lblKardexCount.Text = ""
            Me.lblKardexCount.Dock = DockStyle.Bottom
            Me.lblKardexCount.Height = 22
            Me.lblKardexCount.Font = New Font("Tahoma", 9.0!)
            Me.lblKardexCount.ForeColor = Color.FromArgb(30, 80, 140)
            Me.lblKardexCount.BackColor = Color.FromArgb(235, 248, 255)
            Me.lblKardexCount.TextAlign = ContentAlignment.MiddleRight
            Me.lblKardexCount.Padding = New Padding(10, 0, 10, 0)
            Me.lblKardexCount.Name = "lblKardexCount"

            ' ---------- dgvKardex ----------
            Me.dgvKardex.Dock = DockStyle.Fill
            Me.dgvKardex.Name = "dgvKardex"
            Me.dgvKardex.AllowUserToAddRows = False
            Me.dgvKardex.ReadOnly = True
            Me.dgvKardex.RightToLeft = RightToLeft.Yes

            ' ---------- tabInventoryCount (انبارگردانی) ----------
            Me.tabInventoryCount = New TabPage()
            Me.pnlInventoryCountTop = New Panel()
            Me.lblInvCountWarehouseLabel = New Label()
            Me.cmbInvCountWarehouse = New ComboBox()
            Me.chkShowQty = New CheckBox()
            Me.chkShowLocation = New CheckBox()
            Me.btnGenerateInvCount = New Button()
            Me.btnPrintInvCount = New Button()
            Me.lblInvCountStatus = New Label()
            Me.dgvInvCount = New DataGridView()

            Me.tabInventoryCount.Text = "   لیست انبار گردانی   "
            Me.tabInventoryCount.Name = "tabInventoryCount"
            Me.tabInventoryCount.RightToLeft = RightToLeft.Yes
            Me.tabInventoryCount.Controls.Add(Me.dgvInvCount)
            Me.tabInventoryCount.Controls.Add(Me.lblInvCountStatus)
            Me.tabInventoryCount.Controls.Add(Me.pnlInventoryCountTop)
            Me.tabMain.TabPages.Add(Me.tabInventoryCount)

            Me.pnlInventoryCountTop.Dock = DockStyle.Top
            Me.pnlInventoryCountTop.Height = 48
            Me.pnlInventoryCountTop.Name = "pnlInventoryCountTop"
            Me.pnlInventoryCountTop.BackColor = Color.FromArgb(235, 250, 240)

            Me.lblInvCountWarehouseLabel.Text = "انبار :"
            Me.lblInvCountWarehouseLabel.Location = New Point(8, 14)
            Me.lblInvCountWarehouseLabel.Size = New Size(48, 20)
            Me.lblInvCountWarehouseLabel.Font = New Font("Tahoma", 9.0!)
            Me.lblInvCountWarehouseLabel.TextAlign = ContentAlignment.MiddleRight

            Me.cmbInvCountWarehouse.Location = New Point(60, 11)
            Me.cmbInvCountWarehouse.Size = New Size(220, 24)
            Me.cmbInvCountWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.cmbInvCountWarehouse.RightToLeft = RightToLeft.Yes
            Me.cmbInvCountWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbInvCountWarehouse.Name = "cmbInvCountWarehouse"

            Me.chkShowQty.Text = "نمایش موجودی"
            Me.chkShowQty.Location = New Point(292, 13)
            Me.chkShowQty.Size = New Size(120, 22)
            Me.chkShowQty.Font = New Font("Tahoma", 9.0!)
            Me.chkShowQty.Checked = True
            Me.chkShowQty.RightToLeft = RightToLeft.Yes
            Me.chkShowQty.Name = "chkShowQty"

            Me.chkShowLocation.Text = "نمایش محل کالا"
            Me.chkShowLocation.Location = New Point(422, 13)
            Me.chkShowLocation.Size = New Size(130, 22)
            Me.chkShowLocation.Font = New Font("Tahoma", 9.0!)
            Me.chkShowLocation.Checked = True
            Me.chkShowLocation.RightToLeft = RightToLeft.Yes
            Me.chkShowLocation.Name = "chkShowLocation"

            Me.btnGenerateInvCount.Text = "تهیه لیست"
            Me.btnGenerateInvCount.Location = New Point(562, 10)
            Me.btnGenerateInvCount.Size = New Size(105, 26)
            Me.btnGenerateInvCount.Font = New Font("Tahoma", 9.0!)
            Me.btnGenerateInvCount.BackColor = Color.FromArgb(0, 115, 60)
            Me.btnGenerateInvCount.ForeColor = Color.White
            Me.btnGenerateInvCount.FlatStyle = FlatStyle.Flat
            Me.btnGenerateInvCount.Name = "btnGenerateInvCount"

            Me.btnPrintInvCount.Text = "چاپ"
            Me.btnPrintInvCount.Location = New Point(673, 10)
            Me.btnPrintInvCount.Size = New Size(88, 26)
            Me.btnPrintInvCount.Font = New Font("Tahoma", 9.0!)
            Me.btnPrintInvCount.BackColor = Color.FromArgb(100, 55, 145)
            Me.btnPrintInvCount.ForeColor = Color.White
            Me.btnPrintInvCount.FlatStyle = FlatStyle.Flat
            Me.btnPrintInvCount.Name = "btnPrintInvCount"

            Me.lblInvCountStatus.Text = "تعداد اقلام: 0"
            Me.lblInvCountStatus.Dock = DockStyle.Bottom
            Me.lblInvCountStatus.Height = 22
            Me.lblInvCountStatus.Font = New Font("Tahoma", 9.0!)
            Me.lblInvCountStatus.ForeColor = Color.FromArgb(20, 100, 40)
            Me.lblInvCountStatus.BackColor = Color.FromArgb(235, 252, 240)
            Me.lblInvCountStatus.TextAlign = ContentAlignment.MiddleRight
            Me.lblInvCountStatus.Padding = New Padding(10, 0, 10, 0)
            Me.lblInvCountStatus.Name = "lblInvCountStatus"

            Me.pnlInventoryCountTop.Controls.AddRange(New Control() {
                Me.lblInvCountWarehouseLabel, Me.cmbInvCountWarehouse,
                Me.chkShowQty, Me.chkShowLocation,
                Me.btnGenerateInvCount, Me.btnPrintInvCount
            })

            Me.dgvInvCount.Dock = DockStyle.Fill
            Me.dgvInvCount.Name = "dgvInvCount"
            Me.dgvInvCount.AllowUserToAddRows = False
            Me.dgvInvCount.ReadOnly = True
            Me.dgvInvCount.RightToLeft = RightToLeft.Yes
            CType(Me.dgvInvCount, ISupportInitialize).BeginInit()

            ' ---------- MojodyAnbarFormRep ----------
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1200, 700)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "MojodyAnbarFormRep"
            Me.Text = "موجودی انبار، کاردکس و انبارگردانی"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Controls.Add(Me.tabMain)

            Me.tabMain.ResumeLayout(False)
            CType(Me.dgvInventory, ISupportInitialize).EndInit()
            CType(Me.dgvKardex, ISupportInitialize).EndInit()
            CType(Me.dgvInvCount, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents tabMain As TabControl
        Friend WithEvents tabInventory As TabPage
        Friend WithEvents tabKardex As TabPage
        Friend WithEvents tabInventoryCount As TabPage
        Friend WithEvents pnlInventoryTop As Panel
        Friend WithEvents lblWarehouseLabel As Label
        Friend WithEvents cmbWarehouse As ComboBox
        Friend WithEvents btnRefresh As Button
        Friend WithEvents btnPrintInventory As Button
        Friend WithEvents lblInventoryCount As Label
        Friend WithEvents lblSearch As Label
        Friend WithEvents txtSearchInventory As TextBox
        Friend WithEvents dgvInventory As DataGridView
        Friend WithEvents pnlInventoryFooter As Panel
        Friend WithEvents lblGrandTotalText As Label
        Friend WithEvents lblGrandTotalValue As Label
        Friend WithEvents pnlKardexTop As Panel
        Friend WithEvents lblKardexProductLabel As Label
        Friend WithEvents cmbKardexProduct As ComboBox
        Friend WithEvents lblKardexWarehouseLabel As Label
        Friend WithEvents cmbKardexWarehouse As ComboBox
        Friend WithEvents lblKardexFromLabel As Label
        Friend WithEvents txtKardexFrom As TextBox
        Friend WithEvents lblKardexToLabel As Label
        Friend WithEvents txtKardexTo As TextBox
        Friend WithEvents btnKardexLoad As Button
        Friend WithEvents btnPrintKardex As Button
        Friend WithEvents lblKardexTitle As Label
        Friend WithEvents lblKardexCount As Label
        Friend WithEvents dgvKardex As DataGridView
        Friend WithEvents pnlInventoryCountTop As Panel
        Friend WithEvents lblInvCountWarehouseLabel As Label
        Friend WithEvents cmbInvCountWarehouse As ComboBox
        Friend WithEvents chkShowQty As CheckBox
        Friend WithEvents chkShowLocation As CheckBox
        Friend WithEvents btnGenerateInvCount As Button
        Friend WithEvents btnPrintInvCount As Button
        Friend WithEvents lblInvCountStatus As Label
        Friend WithEvents dgvInvCount As DataGridView

    End Class
End Namespace
