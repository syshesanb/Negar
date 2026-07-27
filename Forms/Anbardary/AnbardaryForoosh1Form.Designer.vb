Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryForoosh1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents tabSalesSub As TabControl
        Friend WithEvents tabPageForoosh As TabPage
        Friend WithEvents tabPageHavaleh As TabPage
        Friend WithEvents tabPageBargasht As TabPage
        Friend WithEvents tabPageHavalehBargasht As TabPage

        ' Tab 1: Foroosh Controls
        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents pnlFilters As Panel
        Friend WithEvents dgvInvoices As DataGridView

        ' Tab 2: Havaleh Controls
        Friend WithEvents pnlTopHavaleh As Panel
        Friend WithEvents btnNewHavalehHavaleh As Button
        Friend WithEvents btnRefreshHavaleh As Button
        Friend WithEvents pnlFiltersHavaleh As Panel
        Friend WithEvents dgvInvoicesHavaleh As DataGridView

        ' Tab 3: Bargasht Controls
        Friend WithEvents pnlTopBargasht As Panel
        Friend WithEvents btnRefreshBargasht As Button
        Friend WithEvents pnlFiltersBargasht As Panel
        Friend WithEvents dgvInvoicesBargasht As DataGridView

        ' Tab 4: HavalehBargasht Controls
        Friend WithEvents pnlTopHavalehBargasht As Panel
        Friend WithEvents btnRefreshHavalehBargasht As Button
        Friend WithEvents pnlFiltersHavalehBargasht As Panel
        Friend WithEvents dgvInvoicesHavalehBargasht As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabSalesSub = New TabControl()
            Me.tabPageForoosh = New TabPage()
            Me.tabPageHavaleh = New TabPage()
            Me.tabPageBargasht = New TabPage()
            Me.tabPageHavalehBargasht = New TabPage()

            ' Tab 1
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.btnRefresh = New Button()
            Me.pnlFilters = New Panel()
            Me.dgvInvoices = New DataGridView()

            ' Tab 2
            Me.pnlTopHavaleh = New Panel()
            Me.btnNewHavalehHavaleh = New Button()
            Me.btnRefreshHavaleh = New Button()
            Me.pnlFiltersHavaleh = New Panel()
            Me.dgvInvoicesHavaleh = New DataGridView()

            ' Tab 3
            Me.pnlTopBargasht = New Panel()
            Me.btnRefreshBargasht = New Button()
            Me.pnlFiltersBargasht = New Panel()
            Me.dgvInvoicesBargasht = New DataGridView()

            ' Tab 4
            Me.pnlTopHavalehBargasht = New Panel()
            Me.btnRefreshHavalehBargasht = New Button()
            Me.pnlFiltersHavalehBargasht = New Panel()
            Me.dgvInvoicesHavalehBargasht = New DataGridView()

            Me.tabSalesSub.SuspendLayout()
            Me.tabPageForoosh.SuspendLayout()
            Me.tabPageHavaleh.SuspendLayout()
            Me.tabPageBargasht.SuspendLayout()
            Me.tabPageHavalehBargasht.SuspendLayout()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvInvoices, ISupportInitialize).BeginInit()
            Me.pnlTopHavaleh.SuspendLayout()
            CType(Me.dgvInvoicesHavaleh, ISupportInitialize).BeginInit()
            Me.pnlTopBargasht.SuspendLayout()
            CType(Me.dgvInvoicesBargasht, ISupportInitialize).BeginInit()
            Me.pnlTopHavalehBargasht.SuspendLayout()
            CType(Me.dgvInvoicesHavalehBargasht, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            ' tabSalesSub
            '
            Me.tabSalesSub.Controls.Add(Me.tabPageForoosh)
            Me.tabSalesSub.Controls.Add(Me.tabPageHavaleh)
            Me.tabSalesSub.Controls.Add(Me.tabPageBargasht)
            Me.tabSalesSub.Controls.Add(Me.tabPageHavalehBargasht)
            Me.tabSalesSub.Dock = DockStyle.Fill
            Me.tabSalesSub.Font = New Font("Tahoma", 9.0!)
            Me.tabSalesSub.Location = New Point(0, 0)
            Me.tabSalesSub.Name = "tabSalesSub"
            Me.tabSalesSub.RightToLeft = RightToLeft.Yes
            Me.tabSalesSub.RightToLeftLayout = True
            Me.tabSalesSub.SelectedIndex = 0
            Me.tabSalesSub.Size = New Size(950, 600)
            Me.tabSalesSub.TabIndex = 0

            '
            ' tabPageForoosh
            '
            Me.tabPageForoosh.Controls.Add(Me.dgvInvoices)
            Me.tabPageForoosh.Controls.Add(Me.pnlFilters)
            Me.tabPageForoosh.Controls.Add(Me.pnlTop)
            Me.tabPageForoosh.Location = New Point(4, 23)
            Me.tabPageForoosh.Name = "tabPageForoosh"
            Me.tabPageForoosh.Padding = New Padding(3)
            Me.tabPageForoosh.Size = New Size(942, 573)
            Me.tabPageForoosh.TabIndex = 0
            Me.tabPageForoosh.Text = "فروش کالا و خدمات"
            Me.tabPageForoosh.UseVisualStyleBackColor = True

            '
            ' pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(250, 235, 240)
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
            Me.btnNew.BackColor = Color.FromArgb(160, 30, 80)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(10, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(140, 26)
            Me.btnNew.TabIndex = 0
            Me.btnNew.Text = "+ فاکتور فروش جدید"
            Me.btnNew.UseVisualStyleBackColor = False

            '
            ' btnRefresh
            '
            Me.btnRefresh.Location = New Point(160, 8)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(90, 26)
            Me.btnRefresh.TabIndex = 1
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = True

            '
            ' pnlFilters
            '
            Me.pnlFilters.BackColor = Color.FromArgb(255, 242, 248)
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
            ' tabPageHavaleh
            '
            Me.tabPageHavaleh.Controls.Add(Me.dgvInvoicesHavaleh)
            Me.tabPageHavaleh.Controls.Add(Me.pnlFiltersHavaleh)
            Me.tabPageHavaleh.Controls.Add(Me.pnlTopHavaleh)
            Me.tabPageHavaleh.Location = New Point(4, 23)
            Me.tabPageHavaleh.Name = "tabPageHavaleh"
            Me.tabPageHavaleh.Padding = New Padding(3)
            Me.tabPageHavaleh.Size = New Size(942, 573)
            Me.tabPageHavaleh.TabIndex = 1
            Me.tabPageHavaleh.Text = "حواله خروج انبار برای فروش"
            Me.tabPageHavaleh.UseVisualStyleBackColor = True

            '
            ' pnlTopHavaleh
            '
            Me.pnlTopHavaleh.BackColor = Color.FromArgb(250, 235, 240)
            Me.pnlTopHavaleh.Controls.Add(Me.btnNewHavalehHavaleh)
            Me.pnlTopHavaleh.Controls.Add(Me.btnRefreshHavaleh)
            Me.pnlTopHavaleh.Dock = DockStyle.Top
            Me.pnlTopHavaleh.Location = New Point(3, 3)
            Me.pnlTopHavaleh.Name = "pnlTopHavaleh"
            Me.pnlTopHavaleh.Size = New Size(936, 42)
            Me.pnlTopHavaleh.TabIndex = 0

            '
            ' btnNewHavalehHavaleh
            '
            Me.btnNewHavalehHavaleh.BackColor = Color.FromArgb(160, 30, 80)
            Me.btnNewHavalehHavaleh.FlatStyle = FlatStyle.Flat
            Me.btnNewHavalehHavaleh.ForeColor = Color.White
            Me.btnNewHavalehHavaleh.Location = New Point(10, 8)
            Me.btnNewHavalehHavaleh.Name = "btnNewHavalehHavaleh"
            Me.btnNewHavalehHavaleh.Size = New Size(210, 26)
            Me.btnNewHavalehHavaleh.TabIndex = 0
            Me.btnNewHavalehHavaleh.Text = "+ ثبت حواله انبار جدید (مستقل)"
            Me.btnNewHavalehHavaleh.UseVisualStyleBackColor = False

            '
            ' btnRefreshHavaleh
            '
            Me.btnRefreshHavaleh.Location = New Point(230, 8)
            Me.btnRefreshHavaleh.Name = "btnRefreshHavaleh"
            Me.btnRefreshHavaleh.Size = New Size(90, 26)
            Me.btnRefreshHavaleh.TabIndex = 1
            Me.btnRefreshHavaleh.Text = "بازخوانی"
            Me.btnRefreshHavaleh.UseVisualStyleBackColor = True

            '
            ' pnlFiltersHavaleh
            '
            Me.pnlFiltersHavaleh.BackColor = Color.FromArgb(255, 242, 248)
            Me.pnlFiltersHavaleh.Dock = DockStyle.Top
            Me.pnlFiltersHavaleh.Location = New Point(3, 45)
            Me.pnlFiltersHavaleh.Name = "pnlFiltersHavaleh"
            Me.pnlFiltersHavaleh.Size = New Size(936, 30)
            Me.pnlFiltersHavaleh.TabIndex = 1

            '
            ' dgvInvoicesHavaleh
            '
            Me.dgvInvoicesHavaleh.AllowUserToAddRows = False
            Me.dgvInvoicesHavaleh.BackgroundColor = Color.White
            Me.dgvInvoicesHavaleh.ColumnHeadersHeight = 30
            Me.dgvInvoicesHavaleh.Dock = DockStyle.Fill
            Me.dgvInvoicesHavaleh.Location = New Point(3, 75)
            Me.dgvInvoicesHavaleh.MultiSelect = False
            Me.dgvInvoicesHavaleh.Name = "dgvInvoicesHavaleh"
            Me.dgvInvoicesHavaleh.ReadOnly = True
            Me.dgvInvoicesHavaleh.RowHeadersVisible = False
            Me.dgvInvoicesHavaleh.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoicesHavaleh.Size = New Size(936, 495)
            Me.dgvInvoicesHavaleh.TabIndex = 2

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
            Me.tabPageBargasht.Text = "برگشت از فروش کالا و خدمات"
            Me.tabPageBargasht.UseVisualStyleBackColor = True

            '
            ' pnlTopBargasht
            '
            Me.pnlTopBargasht.BackColor = Color.FromArgb(250, 235, 240)
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
            Me.pnlFiltersBargasht.BackColor = Color.FromArgb(255, 242, 248)
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
            ' tabPageHavalehBargasht
            '
            Me.tabPageHavalehBargasht.Controls.Add(Me.dgvInvoicesHavalehBargasht)
            Me.tabPageHavalehBargasht.Controls.Add(Me.pnlFiltersHavalehBargasht)
            Me.tabPageHavalehBargasht.Controls.Add(Me.pnlTopHavalehBargasht)
            Me.tabPageHavalehBargasht.Location = New Point(4, 23)
            Me.tabPageHavalehBargasht.Name = "tabPageHavalehBargasht"
            Me.tabPageHavalehBargasht.Padding = New Padding(3)
            Me.tabPageHavalehBargasht.Size = New Size(942, 573)
            Me.tabPageHavalehBargasht.TabIndex = 3
            Me.tabPageHavalehBargasht.Text = "رسید ورود انبار برای برگشت از فروش"
            Me.tabPageHavalehBargasht.UseVisualStyleBackColor = True

            '
            ' pnlTopHavalehBargasht
            '
            Me.pnlTopHavalehBargasht.BackColor = Color.FromArgb(250, 235, 240)
            Me.pnlTopHavalehBargasht.Controls.Add(Me.btnRefreshHavalehBargasht)
            Me.pnlTopHavalehBargasht.Dock = DockStyle.Top
            Me.pnlTopHavalehBargasht.Location = New Point(3, 3)
            Me.pnlTopHavalehBargasht.Name = "pnlTopHavalehBargasht"
            Me.pnlTopHavalehBargasht.Size = New Size(936, 42)
            Me.pnlTopHavalehBargasht.TabIndex = 0

            '
            ' btnRefreshHavalehBargasht
            '
            Me.btnRefreshHavalehBargasht.Location = New Point(10, 8)
            Me.btnRefreshHavalehBargasht.Name = "btnRefreshHavalehBargasht"
            Me.btnRefreshHavalehBargasht.Size = New Size(90, 26)
            Me.btnRefreshHavalehBargasht.TabIndex = 0
            Me.btnRefreshHavalehBargasht.Text = "بازخوانی"
            Me.btnRefreshHavalehBargasht.UseVisualStyleBackColor = True

            '
            ' pnlFiltersHavalehBargasht
            '
            Me.pnlFiltersHavalehBargasht.BackColor = Color.FromArgb(255, 242, 248)
            Me.pnlFiltersHavalehBargasht.Dock = DockStyle.Top
            Me.pnlFiltersHavalehBargasht.Location = New Point(3, 45)
            Me.pnlFiltersHavalehBargasht.Name = "pnlFiltersHavalehBargasht"
            Me.pnlFiltersHavalehBargasht.Size = New Size(936, 30)
            Me.pnlFiltersHavalehBargasht.TabIndex = 1

            '
            ' dgvInvoicesHavalehBargasht
            '
            Me.dgvInvoicesHavalehBargasht.AllowUserToAddRows = False
            Me.dgvInvoicesHavalehBargasht.BackgroundColor = Color.White
            Me.dgvInvoicesHavalehBargasht.ColumnHeadersHeight = 30
            Me.dgvInvoicesHavalehBargasht.Dock = DockStyle.Fill
            Me.dgvInvoicesHavalehBargasht.Location = New Point(3, 75)
            Me.dgvInvoicesHavalehBargasht.MultiSelect = False
            Me.dgvInvoicesHavalehBargasht.Name = "dgvInvoicesHavalehBargasht"
            Me.dgvInvoicesHavalehBargasht.ReadOnly = True
            Me.dgvInvoicesHavalehBargasht.RowHeadersVisible = False
            Me.dgvInvoicesHavalehBargasht.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoicesHavalehBargasht.Size = New Size(936, 495)
            Me.dgvInvoicesHavalehBargasht.TabIndex = 2

            '
            ' AnbardaryForoosh1Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.tabSalesSub)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryForoosh1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "عملیات فروش و خروج کالا"

            Me.tabSalesSub.ResumeLayout(False)
            Me.tabPageForoosh.ResumeLayout(False)
            Me.tabPageHavaleh.ResumeLayout(False)
            Me.tabPageBargasht.ResumeLayout(False)
            Me.tabPageHavalehBargasht.ResumeLayout(False)
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvInvoices, ISupportInitialize).EndInit()
            Me.pnlTopHavaleh.ResumeLayout(False)
            CType(Me.dgvInvoicesHavaleh, ISupportInitialize).EndInit()
            Me.pnlTopBargasht.ResumeLayout(False)
            CType(Me.dgvInvoicesBargasht, ISupportInitialize).EndInit()
            Me.pnlTopHavalehBargasht.ResumeLayout(False)
            CType(Me.dgvInvoicesHavalehBargasht, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
