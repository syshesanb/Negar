Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Production
    Public Class ProductionMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabBOM As TabPage
        Private tabOrders As TabPage
        Private tabElements As TabPage
        Private tabScrapWIP As TabPage
        Private tabReports As TabPage

        ' Tab BOM Controls
        Private dgvBOM As DataGridView

        ' Tab Orders Controls
        Private dgvOrders As DataGridView
        Private btnAddOrder As Button
        Private btnCompleteOrder As Button

        ' Tab Elements Controls
        Private dgvElements As DataGridView

        ' Tab ScrapWIP Controls
        Private dgvScrapWIP As DataGridView

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _prodSvc As ProductionService
        Private _currentCompanyID As Integer

        Public Sub New()
            _prodSvc = New ProductionService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🏭 سیستم جامع بهای تمام‌شده و برنامه‌ریزی تولید (Costing & Production Control)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab BOM
            tabBOM = New TabPage() With {.Text = "📐 فرمول ساخت و تعریف BOM کالاها"}
            InitializeBOMTab()
            tabControl.TabPages.Add(tabBOM)

            ' 2. Tab Orders
            tabOrders = New TabPage() With {.Text = "📋 کارت‌ها و دستورات تولید (Production Orders)"}
            InitializeOrdersTab()
            tabControl.TabPages.Add(tabOrders)

            ' 3. Tab Elements
            tabElements = New TabPage() With {.Text = "🧩 ۳ عنصر اصلی بهای تمام‌شده (مواد، دستمزد، سربار)"}
            InitializeElementsTab()
            tabControl.TabPages.Add(tabElements)

            ' 4. Tab ScrapWIP
            tabScrapWIP = New TabPage() With {.Text = "🔄 کالای در جریان ساخت (WIP) و ضایعات"}
            InitializeScrapWIPTab()
            tabControl.TabPages.Add(tabScrapWIP)

            ' 5. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارشات جامع بهای تمام‌شده و آنالیز BOM"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf ProductionMainForm_Load
        End Sub

        Private Sub ProductionMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadBOMData()
            LoadOrdersData()
        End Sub

        ' ----------------------------------------------------
        ' 1. BOM Tab
        ' ----------------------------------------------------
        Private Sub InitializeBOMTab()
            dgvBOM = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 50,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            dgvBOM.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvBOM.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvBOM.DataBindingComplete, Sub(s, e) SetupGridColumns(CType(s, DataGridView))

            tabBOM.Controls.Add(dgvBOM)
        End Sub

        Private Sub LoadBOMData()
            dgvBOM.DataSource = _prodSvc.GetBOMList(_currentCompanyID)
        End Sub

        ' ----------------------------------------------------
        ' 2. Orders Tab
        ' ----------------------------------------------------
        Private Sub InitializeOrdersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddOrder = New Button() With {
                .Text = "➕ صدور کارت / دستور تولید جدید",
                .Size = New Size(230, 36),
                .Location = New Point(950, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddOrder.Click, AddressOf BtnAddOrder_Click

            btnCompleteOrder = New Button() With {
                .Text = "🏆 تکمیل تولید و صدور اتوماتیک سند بهای تمام‌شده",
                .Size = New Size(340, 36),
                .Location = New Point(590, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCompleteOrder.Click, AddressOf BtnCompleteOrder_Click

            pnlTop.Controls.Add(btnAddOrder)
            pnlTop.Controls.Add(btnCompleteOrder)

            dgvOrders = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 50,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            dgvOrders.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvOrders.DataBindingComplete, Sub(s, e) SetupGridColumns(CType(s, DataGridView))

            tabOrders.Controls.Add(dgvOrders)
            tabOrders.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadOrdersData()
            Dim dt = _prodSvc.GetProductionOrders(_currentCompanyID)
            dgvOrders.DataSource = dt
            dgvElements.DataSource = dt
            dgvScrapWIP.DataSource = dt
        End Sub

        Private Sub BtnAddOrder_Click(sender As Object, e As EventArgs)
            Using dlg As New ProductionEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadOrdersData()
            End Using
        End Sub

        Private Sub BtnCompleteOrder_Click(sender As Object, e As EventArgs)
            If dgvOrders.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک دستور تولید را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells("OrderID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _prodSvc.CompleteProductionOrder(orderId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("کارت تولید با موفقیت تکمیل شد و سند حسابداری صنعتی آن به‌صورت اتوماتیک در پشت پرده صادر گردید.", "موفقیت ثبت سند", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadOrdersData()
            Else
                MessageBox.Show("خطا در تکمیل دستور تولید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 3. Elements Tab
        ' ----------------------------------------------------
        Private Sub InitializeElementsTab()
            dgvElements = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 50,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            dgvElements.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvElements.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvElements.DataBindingComplete, Sub(s, e) SetupGridColumns(CType(s, DataGridView))

            tabElements.Controls.Add(dgvElements)
        End Sub

        ' ----------------------------------------------------
        ' 4. ScrapWIP Tab
        ' ----------------------------------------------------
        Private Sub InitializeScrapWIPTab()
            dgvScrapWIP = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 50,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            dgvScrapWIP.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvScrapWIP.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvScrapWIP.DataBindingComplete, Sub(s, e) SetupGridColumns(CType(s, DataGridView))

            tabScrapWIP.Controls.Add(dgvScrapWIP)
        End Sub

        ' ----------------------------------------------------
        ' 5. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش آنالیز عناصر بهای تمام‌شده و سودآوری",
                .Size = New Size(360, 36),
                .Location = New Point(810, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub() dgvReport.DataSource = _prodSvc.GetCostBreakdownReport(_currentCompanyID)

            pnlTop.Controls.Add(btnLoadReport)

            dgvReport = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 50,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            dgvReport.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) SetupGridColumns(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Sub SetupGridColumns(dgv As DataGridView)
            If dgv Is Nothing Then Return

            If Not dgv.Columns.Contains("colRowIndex") Then
                Dim colRow As New DataGridViewTextBoxColumn() With {
                    .Name = "colRowIndex",
                    .HeaderText = "ردیف",
                    .Width = 50,
                    .ReadOnly = True
                }
                dgv.Columns.Insert(0, colRow)
            End If

            For i As Integer = 0 To dgv.Rows.Count - 1
                dgv.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgv)
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                {"BomID", "شناسه"},
                {"ProductCode", "کد محصول"},
                {"ProductName", "نام محصول"},
                {"RawMaterialName", "ماده اولیه / قطعه"},
                {"QuantityRequired", "ضریب مصرف"},
                {"WastePercent", "ضایعات (%)"},
                {"UnitName", "واحد سنجش"},
                {"OrderID", "شناسه دستور"},
                {"OrderNo", "شماره دستور تولید"},
                {"TargetQuantity", "تیراژ هدف (تعداد)"},
                {"ProducedQuantity", "تعداد تولیدشده"},
                {"DirectMaterialCost", "مواد مستقیم" & vbCrLf & "(ریال)"},
                {"DirectLaborCost", "دستمزد مستقیم" & vbCrLf & "(ریال)"},
                {"OverheadCost", "سربار تولید" & vbCrLf & "(ریال)"},
                {"TotalProductionCost", "بهای تمام‌شده کل" & vbCrLf & "(ریال)"},
                {"TotalMaterials", "کل مواد" & vbCrLf & "(ریال)"},
                {"TotalLabor", "کل دستمزد" & vbCrLf & "(ریال)"},
                {"TotalOverhead", "کل سربار" & vbCrLf & "(ریال)"},
                {"TotalCost", "بهای کل" & vbCrLf & "(ریال)"},
                {"AvgUnitCost", "میانگین واحد" & vbCrLf & "(ریال)"},
                {"UnitCost", "بهای تمام‌شده واحد" & vbCrLf & "(ریال)"},
                {"Status", "وضعیت تولید"},
                {"StartDate", "تاریخ شروع"},
                {"EndDate", "تاریخ خاتمه"},
                {"Notes", "توضیحات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 140
            Next

            If dgv.Columns.Contains("BomID") Then dgv.Columns("BomID").Visible = False
            If dgv.Columns.Contains("OrderID") Then dgv.Columns("OrderID").Visible = False
            If dgv.Columns.Contains("ProductCode") Then dgv.Columns("ProductCode").Width = 110
            If dgv.Columns.Contains("ProductName") Then dgv.Columns("ProductName").Width = 220
            If dgv.Columns.Contains("RawMaterialName") Then dgv.Columns("RawMaterialName").Width = 200
        End Sub
    End Class
End Namespace
