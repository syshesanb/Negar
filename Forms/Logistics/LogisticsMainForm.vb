Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Logistics
    Public Class LogisticsMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabFleet As TabPage
        Private tabRoutes As TabPage
        Private tabManifests As TabPage
        Private tabReports As TabPage

        ' Tab Fleet Controls
        Private dgvFleet As DataGridView
        Private btnAddVehicle As Button

        ' Tab Routes Controls
        Private dgvRoutes As DataGridView

        ' Tab Manifests Controls
        Private dgvManifests As DataGridView
        Private btnSettleManifest As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _logSvc As LogisticsService
        Private _currentCompanyID As Integer

        Public Sub New()
            _logSvc = New LogisticsService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🚚 سیستم جامع مدیریت ناوگان حمل، پخش مویرگی و لوجستیک (Logistics & Fleet Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Fleet
            tabFleet = New TabPage() With {.Text = "🚛 شناسنامه ناوگان و وسایط نقلیه"}
            InitializeFleetTab()
            tabControl.TabPages.Add(tabFleet)

            ' 2. Tab Routes
            tabRoutes = New TabPage() With {.Text = "🗺️ مسیرها و مناطق توزیع شهری/استانی"}
            InitializeRoutesTab()
            tabControl.TabPages.Add(tabRoutes)

            ' 3. Tab Manifests
            tabManifests = New TabPage() With {.Text = "📋 بارنامه‌ها و تورهای توزیع (Manifests)"}
            InitializeManifestsTab()
            tabControl.TabPages.Add(tabManifests)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات کرایه حمل، پورسانت و تحلیل پخش"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf LogisticsMainForm_Load
        End Sub

        Private Sub LogisticsMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadFleetData()
            LoadRoutesData()
            LoadManifestsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Fleet Tab
        ' ----------------------------------------------------
        Private Sub InitializeFleetTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddVehicle = New Button() With {
                .Text = "➕ ثبت خودرو و وسیله نقلیه جدید",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddVehicle.Click, AddressOf BtnAddVehicle_Click
            pnlTop.Controls.Add(btnAddVehicle)

            dgvFleet = New DataGridView() With {
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
            dgvFleet.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFleet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabFleet.Controls.Add(dgvFleet)
            tabFleet.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadFleetData()
            Try
                Dim dt = _logSvc.GetFleet(_currentCompanyID)
                dgvFleet.DataSource = dt
                SetupGridColumns(dgvFleet)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddVehicle_Click(sender As Object, e As EventArgs)
            Using dlg As New LogisticsEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadFleetData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Routes Tab
        ' ----------------------------------------------------
        Private Sub InitializeRoutesTab()
            dgvRoutes = New DataGridView() With {
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
            dgvRoutes.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvRoutes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabRoutes.Controls.Add(dgvRoutes)
        End Sub

        Private Sub LoadRoutesData()
            Try
                Dim dt = _logSvc.GetRoutes(_currentCompanyID)
                dgvRoutes.DataSource = dt
                SetupGridColumns(dgvRoutes)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Manifests Tab
        ' ----------------------------------------------------
        Private Sub InitializeManifestsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnSettleManifest = New Button() With {
                .Text = "🏆 تسویه‌حساب بارنامه و صدور سند حسابداری حمل",
                .Size = New Size(360, 36),
                .Location = New Point(820, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSettleManifest.Click, AddressOf BtnSettleManifest_Click
            pnlTop.Controls.Add(btnSettleManifest)

            dgvManifests = New DataGridView() With {
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
            dgvManifests.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvManifests.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabManifests.Controls.Add(dgvManifests)
            tabManifests.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadManifestsData()
            Try
                Dim dt = _logSvc.GetManifests(_currentCompanyID)
                dgvManifests.DataSource = dt
                SetupGridColumns(dgvManifests)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnSettleManifest_Click(sender As Object, e As EventArgs)
            If dgvManifests.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک بارنامه را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim manId = Convert.ToInt32(dgvManifests.CurrentRow.Cells("ManifestID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _logSvc.SettleManifestAndIssueSanad(manId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("بارنامه توزیع با موفقیت تسویه شد و سند حسابداری هزینه حمل در پشت پرده صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadManifestsData()
            Else
                MessageBox.Show("خطا در ثبت تسویه‌حساب بارنامه.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش عملکرد ناوگان و پورسانت موزعان",
                .Size = New Size(400, 36),
                .Location = New Point(770, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _logSvc.GetLogisticsReport(_currentCompanyID)
                    dgvReport.DataSource = dt
                    SetupGridColumns(dgvReport)
                Catch ex As Exception
                End Try
            End Sub

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

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
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
                    {"VehicleID", "شناسه خودرو"},
                    {"PlateNumber", "شماره پلاک خودرو"},
                    {"VehicleType", "نوع وسیله نقلیه"},
                    {"DriverName", "نام راننده / موزع"},
                    {"CapacityKg", "ظرفیت (کیلوگرم)"},
                    {"Ownership", "مالکیت"},
                    {"RouteID", "شناسه مسیر"},
                    {"RouteCode", "کد مسیر"},
                    {"RouteName", "نام مسیر توزیع"},
                    {"CityZone", "منطقه توزیع"},
                    {"EstimatedHours", "زمان تخمینی (ساعت)"},
                    {"ManifestID", "شناسه بارنامه"},
                    {"ManifestNumber", "شماره بارنامه توزیع"},
                    {"InvoiceCount", "تعداد فاکتورها"},
                    {"TotalWeightKg", "وزن کل بار (کیلوگرم)"},
                    {"FreightCost", "کرایه حمل (ریال)"},
                    {"DistributorCommission", "پورسانت موزع (ریال)"},
                    {"DispatchDate", "تاریخ خروج تور"},
                    {"SettlementDate", "تاریخ تسویه‌حساب"},
                    {"TotalManifests", "تعداد کل بارنامه‌ها"},
                    {"TotalDeliveredInvoices", "مجموع فاکتورهای تحویلی"},
                    {"TotalTonnageKg", "مجموع وزن حمل شده (Kg)"},
                    {"TotalFreightCost", "مجموع کرایه حمل (ریال)"},
                    {"TotalCommission", "مجموع پورسانت توزیع (ریال)"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("VehicleID") Then dgv.Columns("VehicleID").Visible = False
                If dgv.Columns.Contains("RouteID") Then dgv.Columns("RouteID").Visible = False
                If dgv.Columns.Contains("ManifestID") Then dgv.Columns("ManifestID").Visible = False
                If dgv.Columns.Contains("PlateNumber") Then dgv.Columns("PlateNumber").Width = 140
                If dgv.Columns.Contains("DriverName") Then dgv.Columns("DriverName").Width = 180
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
