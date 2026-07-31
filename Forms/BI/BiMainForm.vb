Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.BI
    Public Class BiMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabControlTower As TabPage
        Private tabFinancialBi As TabPage
        Private tabSalesForecast As TabPage
        Private tabOperationBi As TabPage

        ' Tab ControlTower Controls
        Private lblRevVal As Label
        Private lblProfitVal As Label
        Private lblCashVal As Label
        Private lblOeeVal As Label

        ' Tab Financial Controls
        Private dgvFinancial As DataGridView

        ' Tab Forecast Controls
        Private dgvForecast As DataGridView

        ' Tab Operation Controls
        Private dgvOperation As DataGridView

        Private _biSvc As BiService
        Private _currentCompanyID As Integer

        Public Sub New()
            _biSvc = New BiService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📊 سیستم جامع هوش تجاری و داشبورد مدیریتی پیشرفته (Business Intelligence & Executive Dashboard - BI)"
            Me.Size = New Size(1250, 780)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab ControlTower
            tabControlTower = New TabPage() With {.Text = "🏛️ برج کنترل مدیرعامل (CEO Control Tower)"}
            InitializeControlTowerTab()
            tabControl.TabPages.Add(tabControlTower)

            ' 2. Tab FinancialBi
            tabFinancialBi = New TabPage() With {.Text = "💰 هوش تجاری مالی و ساختار سودآوری (P&L BI)"}
            InitializeFinancialBiTab()
            tabControl.TabPages.Add(tabFinancialBi)

            ' 3. Tab SalesForecast
            tabSalesForecast = New TabPage() With {.Text = "📈 پیش‌بینی هوشمند فروش و نقدینگی (AI Forecasting)"}
            InitializeSalesForecastTab()
            tabControl.TabPages.Add(tabSalesForecast)

            ' 4. Tab OperationBi
            tabOperationBi = New TabPage() With {.Text = "⚙️ داشبورد پایش بهره‌وری تولید (OEE) و کیفیت (FPY)"}
            InitializeOperationBiTab()
            tabControl.TabPages.Add(tabOperationBi)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf BiMainForm_Load
        End Sub

        Private Sub BiMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadAllBiData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Control Tower Tab
        ' ----------------------------------------------------
        Private Sub InitializeControlTowerTab()
            Dim pnlContainer As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(20)}

            Dim pnlCards As New FlowLayoutPanel() With {
                .Dock = DockStyle.Top,
                .Height = 150,
                .FlowDirection = FlowDirection.RightToLeft,
                .WrapContents = False
            }

            ' Card 1: Revenue
            Dim card1 = CreateMetricCard("درآمد کل فروش (YTD)", "0 ریال", Color.FromArgb(13, 71, 161), lblRevVal)
            ' Card 2: Net Profit
            Dim card2 = CreateMetricCard("سود خالص عملیاتی", "0 ریال", Color.FromArgb(46, 125, 50), lblProfitVal)
            ' Card 3: Net Cash Flow
            Dim card3 = CreateMetricCard("جریان نقدینگی موجود", "0 ریال", Color.FromArgb(230, 81, 0), lblCashVal)
            ' Card 4: OEE Productivity
            Dim card4 = CreateMetricCard("نرخ بهره‌وری کل (OEE)", "0 %", Color.FromArgb(106, 27, 154), lblOeeVal)

            pnlCards.Controls.AddRange(New Control() {card1, card2, card3, card4})

            Dim lblTitle As New Label() With {
                .Text = "📊 سیستم هشدارهای هوشمند و پایش لحظه‌ای عملکرد کل کارخانه (Real-Time Executive Monitoring)",
                .Font = New Font("Tahoma", 11.0!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121),
                .Dock = DockStyle.Top,
                .Height = 40,
                .TextAlign = ContentAlignment.MiddleRight
            }

            pnlContainer.Controls.Add(pnlCards)
            pnlContainer.Controls.Add(lblTitle)

            tabControlTower.Controls.Add(pnlContainer)
        End Sub

        Private Function CreateMetricCard(title As String, initialVal As String, themeColor As Color, ByRef valLabel As Label) As Panel
            Dim pnl As New Panel() With {
                .Size = New Size(270, 120),
                .Margin = New Padding(10),
                .BackColor = Color.White,
                .BorderStyle = BorderStyle.FixedSingle
            }

            Dim pnlTopBar As New Panel() With {.Dock = DockStyle.Top, .Height = 6, .BackColor = themeColor}

            Dim lblT As New Label() With {
                .Text = title,
                .Font = New Font("Tahoma", 9.0!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(80, 80, 80),
                .Location = New Point(10, 20),
                .Size = New Size(250, 25),
                .TextAlign = ContentAlignment.TopRight
            }

            valLabel = New Label() With {
                .Text = initialVal,
                .Font = New Font("Tahoma", 13.0!, FontStyle.Bold),
                .ForeColor = themeColor,
                .Location = New Point(10, 55),
                .Size = New Size(250, 40),
                .TextAlign = ContentAlignment.MiddleCenter
            }

            pnl.Controls.AddRange(New Control() {pnlTopBar, lblT, valLabel})
            Return pnl
        End Function

        ' ----------------------------------------------------
        ' 2. Financial BI Tab
        ' ----------------------------------------------------
        Private Sub InitializeFinancialBiTab()
            dgvFinancial = New DataGridView() With {
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
            dgvFinancial.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFinancial.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabFinancialBi.Controls.Add(dgvFinancial)
        End Sub

        ' ----------------------------------------------------
        ' 3. Sales Forecast Tab
        ' ----------------------------------------------------
        Private Sub InitializeSalesForecastTab()
            dgvForecast = New DataGridView() With {
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
            dgvForecast.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvForecast.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabSalesForecast.Controls.Add(dgvForecast)
        End Sub

        ' ----------------------------------------------------
        ' 4. Operation BI Tab
        ' ----------------------------------------------------
        Private Sub InitializeOperationBiTab()
            dgvOperation = New DataGridView() With {
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
            dgvOperation.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvOperation.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabOperationBi.Controls.Add(dgvOperation)
        End Sub

        Private Sub LoadAllBiData()
            Try
                ' 1. Load Executive Summary
                Dim summary = _biSvc.GetExecutiveSummary(_currentCompanyID)
                Dim rev = Convert.ToDouble(summary("TotalRevenue"))
                Dim profit = Convert.ToDouble(summary("NetProfit"))
                Dim cash = Convert.ToDouble(summary("NetCashFlow"))
                Dim oee = Convert.ToDouble(summary("OeeRate"))

                lblRevVal.Text = rev.ToString("N0") & " ریال"
                lblProfitVal.Text = profit.ToString("N0") & " ریال"
                lblCashVal.Text = cash.ToString("N0") & " ریال"
                lblOeeVal.Text = oee.ToString("F1") & " %"

                ' 2. Load Financial BI
                Dim dtFin = _biSvc.GetProfitabilityByProduct(_currentCompanyID)
                dgvFinancial.DataSource = dtFin
                SetupGridColumns(dgvFinancial)

                ' 3. Load Forecast
                Dim dtFore = _biSvc.GetSalesForecast(_currentCompanyID)
                dgvForecast.DataSource = dtFore
                SetupGridColumns(dgvForecast)

                ' 4. Load Operation
                Dim dtOp = _biSvc.GetOeeBreakdown(_currentCompanyID)
                dgvOperation.DataSource = dtOp
                SetupGridColumns(dgvOperation)

            Catch ex As Exception
            End Try
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
                    {"ProductCategory", "رسته/گروه کالا"},
                    {"TotalSales", "مبلغ کل فروش (ریال)"},
                    {"CostOfGoods", "بهای تمام‌شده (ریال)"},
                    {"GrossProfit", "سود ناخالص (ریال)"},
                    {"MarginPercent", "حاشیه سود (%)"},
                    {"ProfitCategory", "رتبه‌بندی سودآوری"},
                    {"MonthName", "ماه تحلیلی"},
                    {"TargetSales", "فروش هدف بودجه (ریال)"},
                    {"ForecastSales", "پیش‌بینی فروش AI (ریال)"},
                    {"GrowthRate", "نرخ رشد پیش‌بینی (%)"},
                    {"ConfidenceScore", "ضریب اطمینان الگوریتم"},
                    {"LineName", "نام خط تولید / سالن"},
                    {"AvailabilityRate", "نرخ در دسترس بودن (%)"},
                    {"PerformanceRate", "نرخ عملکرد (%)"},
                    {"QualityRate", "نرخ کیفیت FPY (%)"},
                    {"OeePercent", "شاخص OEE نهایی (%)"},
                    {"Status", "وضعیت خط"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 150
                Next

                If dgv.Columns.Contains("ProductCategory") Then dgv.Columns("ProductCategory").Width = 220
                If dgv.Columns.Contains("LineName") Then dgv.Columns("LineName").Width = 220
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
