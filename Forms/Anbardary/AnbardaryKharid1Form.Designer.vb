Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryKharid1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabPurchaseSub As TabControl
        Friend WithEvents tabPageKharid As TabPage
        Friend WithEvents tabPageResid As TabPage
        Friend WithEvents tabPageBargasht As TabPage
        Friend WithEvents tabPageResidBargasht As TabPage

        ' Tab 1: Kharid Controls
        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnNewReceipt As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents pnlFilters As Panel
        Friend WithEvents dgvInvoices As DataGridView

        ' Tab 2: Resid Controls
        Friend WithEvents pnlTopResid As Panel
        Friend WithEvents btnNewResid As Button
        Friend WithEvents btnNewReceiptResid As Button
        Friend WithEvents btnRefreshResid As Button
        Friend WithEvents pnlFiltersResid As Panel
        Friend WithEvents dgvInvoicesResid As DataGridView

        ' Tab 3: Bargasht Controls
        Friend WithEvents pnlTopBargasht As Panel
        Friend WithEvents btnRefreshBargasht As Button
        Friend WithEvents pnlFiltersBargasht As Panel
        Friend WithEvents dgvInvoicesBargasht As DataGridView

        ' Tab 4: ResidBargasht Controls
        Friend WithEvents pnlTopResidBargasht As Panel
        Friend WithEvents btnRefreshResidBargasht As Button
        Friend WithEvents pnlFiltersResidBargasht As Panel
        Friend WithEvents dgvInvoicesResidBargasht As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabPurchaseSub = New TabControl()
            Me.tabPageKharid = New TabPage()
            Me.tabPageResid = New TabPage()
            Me.tabPageBargasht = New TabPage()
            Me.tabPageResidBargasht = New TabPage()

            ' Tab 1
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.btnNewReceipt = New Button()
            Me.btnRefresh = New Button()
            Me.pnlFilters = New Panel()
            Me.dgvInvoices = New DataGridView()

            ' Tab 2
            Me.pnlTopResid = New Panel()
            Me.btnNewResid = New Button()
            Me.btnNewReceiptResid = New Button()
            Me.btnRefreshResid = New Button()
            Me.pnlFiltersResid = New Panel()
            Me.dgvInvoicesResid = New DataGridView()

            ' Tab 3
            Me.pnlTopBargasht = New Panel()
            Me.btnRefreshBargasht = New Button()
            Me.pnlFiltersBargasht = New Panel()
            Me.dgvInvoicesBargasht = New DataGridView()

            ' Tab 4
            Me.pnlTopResidBargasht = New Panel()
            Me.btnRefreshResidBargasht = New Button()
            Me.pnlFiltersResidBargasht = New Panel()
            Me.dgvInvoicesResidBargasht = New DataGridView()

            Me.tabPurchaseSub.SuspendLayout()
            Me.tabPageKharid.SuspendLayout()
            Me.tabPageResid.SuspendLayout()
            Me.tabPageBargasht.SuspendLayout()
            Me.tabPageResidBargasht.SuspendLayout()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvInvoices, ISupportInitialize).BeginInit()
            Me.pnlTopResid.SuspendLayout()
            CType(Me.dgvInvoicesResid, ISupportInitialize).BeginInit()
            Me.pnlTopBargasht.SuspendLayout()
            CType(Me.dgvInvoicesBargasht, ISupportInitialize).BeginInit()
            Me.pnlTopResidBargasht.SuspendLayout()
            CType(Me.dgvInvoicesResidBargasht, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            ' tabPurchaseSub
            '
            Me.tabPurchaseSub.Controls.Add(Me.tabPageKharid)
            Me.tabPurchaseSub.Controls.Add(Me.tabPageResid)
            Me.tabPurchaseSub.Controls.Add(Me.tabPageBargasht)
            Me.tabPurchaseSub.Controls.Add(Me.tabPageResidBargasht)
            Me.tabPurchaseSub.Dock = DockStyle.Fill
            Me.tabPurchaseSub.Font = New Font("Tahoma", 9.0!)
            Me.tabPurchaseSub.Location = New Point(0, 0)
            Me.tabPurchaseSub.Name = "tabPurchaseSub"
            Me.tabPurchaseSub.RightToLeft = RightToLeft.Yes
            Me.tabPurchaseSub.RightToLeftLayout = True
            Me.tabPurchaseSub.SelectedIndex = 0
            Me.tabPurchaseSub.Size = New Size(950, 600)
            Me.tabPurchaseSub.TabIndex = 0

            '
            ' tabPageKharid
            '
            Me.tabPageKharid.Controls.Add(Me.dgvInvoices)
            Me.tabPageKharid.Controls.Add(Me.pnlFilters)
            Me.tabPageKharid.Controls.Add(Me.pnlTop)
            Me.tabPageKharid.Location = New Point(4, 23)
            Me.tabPageKharid.Name = "tabPageKharid"
            Me.tabPageKharid.Padding = New Padding(3)
            Me.tabPageKharid.Size = New Size(942, 573)
            Me.tabPageKharid.TabIndex = 0
            Me.tabPageKharid.Text = "خرید کالا و خدمات"
            Me.tabPageKharid.UseVisualStyleBackColor = True

            '
            ' pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(3, 3)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(936, 42)
            Me.pnlTop.TabIndex = 0

            '
            ' btnNew
            '
            Me.btnNew.BackColor = Color.FromArgb(30, 80, 160)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(10, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(130, 26)
            Me.btnNew.TabIndex = 0
            Me.btnNew.Text = "جدید (فاکتور خرید)"
            Me.btnNew.UseVisualStyleBackColor = False

            '
            ' btnNewReceipt
            '
            Me.btnNewReceipt.BackColor = Color.FromArgb(40, 127, 186)
            Me.btnNewReceipt.FlatStyle = FlatStyle.Flat
            Me.btnNewReceipt.ForeColor = Color.White
            Me.btnNewReceipt.Location = New Point(148, 8)
            Me.btnNewReceipt.Name = "btnNewReceipt"
            Me.btnNewReceipt.Size = New Size(160, 26)
            Me.btnNewReceipt.TabIndex = 1
            Me.btnNewReceipt.Text = "ورود به انبار (رسید انبار)"
            Me.btnNewReceipt.UseVisualStyleBackColor = False

            '
            ' btnRefresh
            '
            Me.btnRefresh.Location = New Point(148, 8)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(90, 26)
            Me.btnRefresh.TabIndex = 1
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = True

            '
            ' pnlFilters
            '
            Me.pnlFilters.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlFilters.Dock = DockStyle.Top
            Me.pnlFilters.Location = New Point(3, 45)
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.Size = New Size(936, 30)
            Me.pnlFilters.TabIndex = 1

            '
            ' dgvInvoices
            '
            Me.dgvInvoices.AllowUserToAddRows = False
            Me.dgvInvoices.BackgroundColor = Color.White
            Me.dgvInvoices.ColumnHeadersHeight = 30
            Me.dgvInvoices.Dock = DockStyle.Fill
            Me.dgvInvoices.Location = New Point(3, 75)
            Me.dgvInvoices.MultiSelect = False
            Me.dgvInvoices.Name = "dgvInvoices"
            Me.dgvInvoices.ReadOnly = True
            Me.dgvInvoices.RowHeadersVisible = False
            Me.dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoices.Size = New Size(936, 495)
            Me.dgvInvoices.TabIndex = 2

            '
            ' tabPageResid
            '
            Me.tabPageResid.Controls.Add(Me.dgvInvoicesResid)
            Me.tabPageResid.Controls.Add(Me.pnlFiltersResid)
            Me.tabPageResid.Controls.Add(Me.pnlTopResid)
            Me.tabPageResid.Location = New Point(4, 23)
            Me.tabPageResid.Name = "tabPageResid"
            Me.tabPageResid.Padding = New Padding(3)
            Me.tabPageResid.Size = New Size(942, 573)
            Me.tabPageResid.TabIndex = 1
            Me.tabPageResid.Text = "رسید ورود انبار برای خرید"
            Me.tabPageResid.UseVisualStyleBackColor = True

            '
            ' pnlTopResid
            '
            Me.pnlTopResid.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTopResid.Controls.Add(Me.btnNewResid)
            Me.pnlTopResid.Controls.Add(Me.btnNewReceiptResid)
            Me.pnlTopResid.Controls.Add(Me.btnRefreshResid)
            Me.pnlTopResid.Dock = DockStyle.Top
            Me.pnlTopResid.Location = New Point(3, 3)
            Me.pnlTopResid.Name = "pnlTopResid"
            Me.pnlTopResid.Size = New Size(936, 42)
            Me.pnlTopResid.TabIndex = 0

            '
            ' btnNewResid
            '
            Me.btnNewResid.BackColor = Color.FromArgb(30, 80, 160)
            Me.btnNewResid.FlatStyle = FlatStyle.Flat
            Me.btnNewResid.ForeColor = Color.White
            Me.btnNewResid.Location = New Point(10, 8)
            Me.btnNewResid.Name = "btnNewResid"
            Me.btnNewResid.Size = New Size(130, 26)
            Me.btnNewResid.TabIndex = 0
            Me.btnNewResid.Text = "جدید (فاکتور خرید)"
            Me.btnNewResid.UseVisualStyleBackColor = False
            Me.btnNewResid.Visible = False

            '
            ' btnNewReceiptResid
            '
            Me.btnNewReceiptResid.BackColor = Color.FromArgb(40, 127, 186)
            Me.btnNewReceiptResid.FlatStyle = FlatStyle.Flat
            Me.btnNewReceiptResid.ForeColor = Color.White
            Me.btnNewReceiptResid.Location = New Point(10, 8)
            Me.btnNewReceiptResid.Name = "btnNewReceiptResid"
            Me.btnNewReceiptResid.Size = New Size(210, 26)
            Me.btnNewReceiptResid.TabIndex = 1
            Me.btnNewReceiptResid.Text = "+ ثبت رسید انبار جدید (مستقل)"
            Me.btnNewReceiptResid.UseVisualStyleBackColor = False
            Me.btnNewReceiptResid.Visible = True

            '
            ' btnRefreshResid
            '
            Me.btnRefreshResid.Location = New Point(230, 8)
            Me.btnRefreshResid.Name = "btnRefreshResid"
            Me.btnRefreshResid.Size = New Size(90, 26)
            Me.btnRefreshResid.TabIndex = 2
            Me.btnRefreshResid.Text = "بازخوانی"
            Me.btnRefreshResid.UseVisualStyleBackColor = True

            '
            ' pnlFiltersResid
            '
            Me.pnlFiltersResid.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlFiltersResid.Dock = DockStyle.Top
            Me.pnlFiltersResid.Location = New Point(3, 45)
            Me.pnlFiltersResid.Name = "pnlFiltersResid"
            Me.pnlFiltersResid.Size = New Size(936, 30)
            Me.pnlFiltersResid.TabIndex = 1

            '
            ' dgvInvoicesResid
            '
            Me.dgvInvoicesResid.AllowUserToAddRows = False
            Me.dgvInvoicesResid.BackgroundColor = Color.White
            Me.dgvInvoicesResid.ColumnHeadersHeight = 30
            Me.dgvInvoicesResid.Dock = DockStyle.Fill
            Me.dgvInvoicesResid.Location = New Point(3, 75)
            Me.dgvInvoicesResid.MultiSelect = False
            Me.dgvInvoicesResid.Name = "dgvInvoicesResid"
            Me.dgvInvoicesResid.ReadOnly = True
            Me.dgvInvoicesResid.RowHeadersVisible = False
            Me.dgvInvoicesResid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoicesResid.Size = New Size(936, 495)
            Me.dgvInvoicesResid.TabIndex = 2

            ' Tab 3
            Me.pnlTopBargasht = New Panel()
            Me.btnRefreshBargasht = New Button()
            Me.pnlFiltersBargasht = New Panel()
            Me.dgvInvoicesBargasht = New DataGridView()

            '
            ' tabPageBargasht
            '
            Me.tabPageBargasht.Controls.Add(Me.dgvInvoicesBargasht)
            Me.tabPageBargasht.Controls.Add(Me.pnlFiltersBargasht)
            Me.tabPageBargasht.Controls.Add(Me.pnlTopBargasht)
            Me.tabPageBargasht.Location = New Point(4, 23)
            Me.tabPageBargasht.Name = "tabPageBargasht"
            Me.tabPageBargasht.Padding = New Padding(3)
            Me.tabPageBargasht.Size = New Size(942, 573)
            Me.tabPageBargasht.TabIndex = 2
            Me.tabPageBargasht.Text = "برگشت از خرید کالا و خدمات"
            Me.tabPageBargasht.UseVisualStyleBackColor = True

            '
            ' pnlTopBargasht
            '
            Me.pnlTopBargasht.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlTopBargasht.Controls.Add(Me.btnRefreshBargasht)
            Me.pnlTopBargasht.Dock = DockStyle.Top
            Me.pnlTopBargasht.Location = New Point(3, 3)
            Me.pnlTopBargasht.Name = "pnlTopBargasht"
            Me.pnlTopBargasht.Size = New Size(936, 42)
            Me.pnlTopBargasht.TabIndex = 0

            '
            ' btnRefreshBargasht
            '
            Me.btnRefreshBargasht.Location = New Point(10, 8)
            Me.btnRefreshBargasht.Name = "btnRefreshBargasht"
            Me.btnRefreshBargasht.Size = New Size(90, 26)
            Me.btnRefreshBargasht.TabIndex = 0
            Me.btnRefreshBargasht.Text = "بازخوانی"
            Me.btnRefreshBargasht.UseVisualStyleBackColor = True

            '
            ' pnlFiltersBargasht
            '
            Me.pnlFiltersBargasht.BackColor = Color.FromArgb(250, 240, 240)
            Me.pnlFiltersBargasht.Dock = DockStyle.Top
            Me.pnlFiltersBargasht.Location = New Point(3, 45)
            Me.pnlFiltersBargasht.Name = "pnlFiltersBargasht"
            Me.pnlFiltersBargasht.Size = New Size(936, 30)
            Me.pnlFiltersBargasht.TabIndex = 1

            '
            ' dgvInvoicesBargasht
            '
            Me.dgvInvoicesBargasht.AllowUserToAddRows = False
            Me.dgvInvoicesBargasht.BackgroundColor = Color.White
            Me.dgvInvoicesBargasht.ColumnHeadersHeight = 30
            Me.dgvInvoicesBargasht.Dock = DockStyle.Fill
            Me.dgvInvoicesBargasht.Location = New Point(3, 75)
            Me.dgvInvoicesBargasht.MultiSelect = False
            Me.dgvInvoicesBargasht.Name = "dgvInvoicesBargasht"
            Me.dgvInvoicesBargasht.ReadOnly = True
            Me.dgvInvoicesBargasht.RowHeadersVisible = False
            Me.dgvInvoicesBargasht.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoicesBargasht.Size = New Size(936, 495)
            Me.dgvInvoicesBargasht.TabIndex = 2

            '
            ' tabPageResidBargasht
            '
            Me.tabPageResidBargasht.Controls.Add(Me.dgvInvoicesResidBargasht)
            Me.tabPageResidBargasht.Controls.Add(Me.pnlFiltersResidBargasht)
            Me.tabPageResidBargasht.Controls.Add(Me.pnlTopResidBargasht)
            Me.tabPageResidBargasht.Location = New Point(4, 23)
            Me.tabPageResidBargasht.Name = "tabPageResidBargasht"
            Me.tabPageResidBargasht.Padding = New Padding(3)
            Me.tabPageResidBargasht.Size = New Size(942, 573)
            Me.tabPageResidBargasht.TabIndex = 3
            Me.tabPageResidBargasht.Text = "رسید خروج انبار برای برگشت از خرید"
            Me.tabPageResidBargasht.UseVisualStyleBackColor = True

            '
            ' pnlTopResidBargasht
            '
            Me.pnlTopResidBargasht.BackColor = Color.FromArgb(245, 235, 240)
            Me.pnlTopResidBargasht.Controls.Add(Me.btnRefreshResidBargasht)
            Me.pnlTopResidBargasht.Dock = DockStyle.Top
            Me.pnlTopResidBargasht.Location = New Point(3, 3)
            Me.pnlTopResidBargasht.Name = "pnlTopResidBargasht"
            Me.pnlTopResidBargasht.Size = New Size(936, 42)
            Me.pnlTopResidBargasht.TabIndex = 0

            '
            ' btnRefreshResidBargasht
            '
            Me.btnRefreshResidBargasht.Location = New Point(10, 8)
            Me.btnRefreshResidBargasht.Name = "btnRefreshResidBargasht"
            Me.btnRefreshResidBargasht.Size = New Size(90, 26)
            Me.btnRefreshResidBargasht.TabIndex = 0
            Me.btnRefreshResidBargasht.Text = "بازخوانی"
            Me.btnRefreshResidBargasht.UseVisualStyleBackColor = True

            '
            ' pnlFiltersResidBargasht
            '
            Me.pnlFiltersResidBargasht.BackColor = Color.FromArgb(250, 242, 245)
            Me.pnlFiltersResidBargasht.Dock = DockStyle.Top
            Me.pnlFiltersResidBargasht.Location = New Point(3, 45)
            Me.pnlFiltersResidBargasht.Name = "pnlFiltersResidBargasht"
            Me.pnlFiltersResidBargasht.Size = New Size(936, 30)
            Me.pnlFiltersResidBargasht.TabIndex = 1

            '
            ' dgvInvoicesResidBargasht
            '
            Me.dgvInvoicesResidBargasht.AllowUserToAddRows = False
            Me.dgvInvoicesResidBargasht.BackgroundColor = Color.White
            Me.dgvInvoicesResidBargasht.ColumnHeadersHeight = 30
            Me.dgvInvoicesResidBargasht.Dock = DockStyle.Fill
            Me.dgvInvoicesResidBargasht.Location = New Point(3, 75)
            Me.dgvInvoicesResidBargasht.MultiSelect = False
            Me.dgvInvoicesResidBargasht.Name = "dgvInvoicesResidBargasht"
            Me.dgvInvoicesResidBargasht.ReadOnly = True
            Me.dgvInvoicesResidBargasht.RowHeadersVisible = False
            Me.dgvInvoicesResidBargasht.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoicesResidBargasht.Size = New Size(936, 495)
            Me.dgvInvoicesResidBargasht.TabIndex = 2

            '
            ' AnbardaryKharid1Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.tabPurchaseSub)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryKharid1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "عملیات خرید و ورود کالا"

            Me.tabPurchaseSub.ResumeLayout(False)
            Me.tabPageKharid.ResumeLayout(False)
            Me.tabPageResid.ResumeLayout(False)
            Me.tabPageBargasht.ResumeLayout(False)
            Me.tabPageResidBargasht.ResumeLayout(False)
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvInvoices, ISupportInitialize).EndInit()
            Me.pnlTopResid.ResumeLayout(False)
            CType(Me.dgvInvoicesResid, ISupportInitialize).EndInit()
            Me.pnlTopBargasht.ResumeLayout(False)
            CType(Me.dgvInvoicesBargasht, ISupportInitialize).EndInit()
            Me.pnlTopResidBargasht.ResumeLayout(False)
            CType(Me.dgvInvoicesResidBargasht, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
