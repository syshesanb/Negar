Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryKharid1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabPurchaseSub As TabControl
        Friend WithEvents tabPageKharid As TabPage
        Friend WithEvents tabPageResid As TabPage
        Friend WithEvents tabPageBargasht As TabPage

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
        Friend WithEvents pnlBargashtContent As Panel
        Friend WithEvents lblBargashtTitle As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabPurchaseSub = New TabControl()
            Me.tabPageKharid = New TabPage()
            Me.tabPageResid = New TabPage()
            Me.tabPageBargasht = New TabPage()

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
            Me.pnlBargashtContent = New Panel()
            Me.lblBargashtTitle = New Label()

            Me.tabPurchaseSub.SuspendLayout()
            Me.tabPageKharid.SuspendLayout()
            Me.tabPageResid.SuspendLayout()
            Me.tabPageBargasht.SuspendLayout()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvInvoices, ISupportInitialize).BeginInit()
            Me.pnlTopResid.SuspendLayout()
            CType(Me.dgvInvoicesResid, ISupportInitialize).BeginInit()
            Me.pnlBargashtContent.SuspendLayout()
            Me.SuspendLayout()

            '
            ' tabPurchaseSub
            '
            Me.tabPurchaseSub.Controls.Add(Me.tabPageKharid)
            Me.tabPurchaseSub.Controls.Add(Me.tabPageResid)
            Me.tabPurchaseSub.Controls.Add(Me.tabPageBargasht)
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
            Me.tabPageResid.Text = "رسید انبار کالا و خدمات"
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
            Me.btnNewReceiptResid.Location = New Point(148, 8)
            Me.btnNewReceiptResid.Name = "btnNewReceiptResid"
            Me.btnNewReceiptResid.Size = New Size(160, 26)
            Me.btnNewReceiptResid.TabIndex = 1
            Me.btnNewReceiptResid.Text = "ورود به انبار (رسید انبار)"
            Me.btnNewReceiptResid.UseVisualStyleBackColor = False
            Me.btnNewReceiptResid.Visible = False

            '
            ' btnRefreshResid
            '
            Me.btnRefreshResid.Location = New Point(10, 8)
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

            '
            ' tabPageBargasht
            '
            Me.tabPageBargasht.Controls.Add(Me.pnlBargashtContent)
            Me.tabPageBargasht.Location = New Point(4, 23)
            Me.tabPageBargasht.Name = "tabPageBargasht"
            Me.tabPageBargasht.Padding = New Padding(3)
            Me.tabPageBargasht.Size = New Size(942, 573)
            Me.tabPageBargasht.TabIndex = 2
            Me.tabPageBargasht.Text = "برگشت از خرید کالا و خدمات"
            Me.tabPageBargasht.UseVisualStyleBackColor = True

            '
            ' pnlBargashtContent
            '
            Me.pnlBargashtContent.Controls.Add(Me.lblBargashtTitle)
            Me.pnlBargashtContent.Dock = DockStyle.Fill
            Me.pnlBargashtContent.Location = New Point(3, 3)
            Me.pnlBargashtContent.Name = "pnlBargashtContent"
            Me.pnlBargashtContent.Size = New Size(936, 567)
            Me.pnlBargashtContent.TabIndex = 0

            '
            ' lblBargashtTitle
            '
            Me.lblBargashtTitle.Dock = DockStyle.Top
            Me.lblBargashtTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            Me.lblBargashtTitle.ForeColor = Color.FromArgb(20, 60, 120)
            Me.lblBargashtTitle.Location = New Point(0, 0)
            Me.lblBargashtTitle.Name = "lblBargashtTitle"
            Me.lblBargashtTitle.Size = New Size(936, 40)
            Me.lblBargashtTitle.Text = "عملیات برگشت از خرید کالا و خدمات"
            Me.lblBargashtTitle.TextAlign = ContentAlignment.MiddleCenter

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
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvInvoices, ISupportInitialize).EndInit()
            Me.pnlTopResid.ResumeLayout(False)
            CType(Me.dgvInvoicesResid, ISupportInitialize).EndInit()
            Me.pnlBargashtContent.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
