Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.API
    Public Class ApiMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabKeys As TabPage
        Private tabOrders As TabPage
        Private tabLogs As TabPage
        Private tabReports As TabPage

        ' Tab Keys Controls
        Private dgvKeys As DataGridView
        Private btnAddKey As Button

        ' Tab Orders Controls
        Private dgvOrders As DataGridView
        Private btnSimulateOrder As Button

        ' Tab Logs Controls
        Private dgvLogs As DataGridView

        ' Tab Reports Controls
        Private dgvReports As DataGridView

        Private _apiSvc As ApiService
        Private _currentCompanyID As Integer

        Public Sub New()
            _apiSvc = New ApiService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🌐 سیستم جامع وب‌سرویس و API فروشگاه اینترنتی و صندوق سیار (E-Commerce & Mobile POS API)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Keys
            tabKeys = New TabPage() With {.Text = "🔑 کلیدهای دسترسی API و احراز هویت JWT"}
            InitializeKeysTab()
            tabControl.TabPages.Add(tabKeys)

            ' 2. Tab Orders
            tabOrders = New TabPage() With {.Text = "🛒 سفارشات همگام‌شده از سایت و پوز سیار"}
            InitializeOrdersTab()
            tabControl.TabPages.Add(tabOrders)

            ' 3. Tab Logs
            tabLogs = New TabPage() With {.Text = "📊 مانیتورینگ زنده ترافیک و لاگ وب‌سرویس (API Traffic Logs)"}
            InitializeLogsTab()
            tabControl.TabPages.Add(tabLogs)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارش تحلیلی کانال‌های فروش Omnichannel"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf ApiMainForm_Load
        End Sub

        Private Sub ApiMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadKeysData()
            LoadOrdersData()
            LoadLogsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Keys Tab
        ' ----------------------------------------------------
        Private Sub InitializeKeysTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddKey = New Button() With {
                .Text = "➕ تولید کلید جدید API Key",
                .Size = New Size(210, 36),
                .Location = New Point(970, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddKey.Click, AddressOf BtnAddKey_Click
            pnlTop.Controls.Add(btnAddKey)

            dgvKeys = New DataGridView() With {
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
            dgvKeys.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvKeys.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabKeys.Controls.Add(dgvKeys)
            tabKeys.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadKeysData()
            Try
                Dim dt = _apiSvc.GetApiKeys(_currentCompanyID)
                dgvKeys.DataSource = dt
                SetupGridColumns(dgvKeys)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddKey_Click(sender As Object, e As EventArgs)
            Using dlg As New ApiEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadKeysData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Orders Tab
        ' ----------------------------------------------------
        Private Sub InitializeOrdersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnSimulateOrder = New Button() With {
                .Text = "🔄 شبیه‌سازی تست دریافت سفارش آنلاین (WooCommerce/POS)",
                .Size = New Size(420, 36),
                .Location = New Point(760, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSimulateOrder.Click, AddressOf BtnSimulateOrder_Click
            pnlTop.Controls.Add(btnSimulateOrder)

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

            tabOrders.Controls.Add(dgvOrders)
            tabOrders.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadOrdersData()
            Try
                Dim dt = _apiSvc.GetApiOrders(_currentCompanyID)
                dgvOrders.DataSource = dt
                SetupGridColumns(dgvOrders)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnSimulateOrder_Click(sender As Object, e As EventArgs)
            Dim orderCode = "WEB-" & (Environment.TickCount Mod 10000).ToString()
            Dim res = _apiSvc.SimulateStoreOrderSync(_currentCompanyID, orderCode, "مهندس حمید کلانتری (خرید وب‌سایت)", 2450000, "درگاه پرداخت آنلاین بانکی")
            If res Then
                MessageBox.Show("سفارش آنلاین با موفقیت via API دریافت گردید، پرونده مشتری ایجاد شد و کالا در انبار نگار رزرو گردید.", "موفقیت API", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadOrdersData()
                LoadLogsData()
            End If
        End Sub

        ' ----------------------------------------------------
        ' 3. Logs Tab
        ' ----------------------------------------------------
        Private Sub InitializeLogsTab()
            dgvLogs = New DataGridView() With {
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
            dgvLogs.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabLogs.Controls.Add(dgvLogs)
        End Sub

        Private Sub LoadLogsData()
            Try
                Dim dt = _apiSvc.GetApiLogs(_currentCompanyID)
                dgvLogs.DataSource = dt
                SetupGridColumns(dgvLogs)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            dgvReports = New DataGridView() With {
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
            dgvReports.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvReports.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabReports.Controls.Add(dgvReports)
        End Sub

        Private Sub SetupGridColumns(dgv As DataGridView)
            Try
                If dgv Is Nothing OrElse dgv.Columns Is Nothing OrElse dgv.Columns.Count = 0 Then Return

                If dgv.Columns.Contains("colRowIndex") Then
                    For i As Integer = 0 To dgv.Rows.Count - 1
                        If i < dgv.Rows.Count Then
                            dgv.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
                        End If
                    Next
                End If

                ApplyPersianGridHeaders(dgv)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            Try
                If dgv Is Nothing OrElse dgv.Columns Is Nothing Then Return

                Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                    {"KeyID", "شناسه کلید"},
                    {"ClientName", "نام کلاینت / سامانه متصل"},
                    {"ApiKey", "کلید API Key"},
                    {"ApiSecret", "کلید سری API Secret"},
                    {"AccessLevel", "سطح دسترسی API"},
                    {"Status", "وضعیت فعال‌سازی"},
                    {"CreatedAt", "تاریخ ایجاد"},
                    {"LogID", "شناسه لاگ"},
                    {"Endpoint", "آدرس اندپوینت (URI)"},
                    {"HttpMethod", "متد HTTP"},
                    {"StatusCode", "کد وضعیت HTTP"},
                    {"LatencyMs", "زمان پاسخ‌دهی (ms)"},
                    {"RequestIp", "IP درخواست‌کننده"},
                    {"LogDate", "تاریخ و زمان درخواست"},
                    {"OrderID", "شناسه سفارش"},
                    {"ExternalOrderCode", "کد سفارش در سایت"},
                    {"CustomerName", "نام خریدار آنلاین"},
                    {"TotalAmount", "مبلغ کل فاکتور (ریال)"},
                    {"PaymentMethod", "روش پرداخت"},
                    {"SyncStatus", "وضعیت همگام‌سازی در نگار"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("KeyID") Then dgv.Columns("KeyID").Visible = False
                If dgv.Columns.Contains("LogID") Then dgv.Columns("LogID").Visible = False
                If dgv.Columns.Contains("OrderID") Then dgv.Columns("OrderID").Visible = False
                If dgv.Columns.Contains("ApiKey") Then dgv.Columns("ApiKey").Width = 190
                If dgv.Columns.Contains("ApiSecret") Then dgv.Columns("ApiSecret").Width = 190
                If dgv.Columns.Contains("ClientName") Then dgv.Columns("ClientName").Width = 220
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
