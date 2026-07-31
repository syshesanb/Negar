Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.PM
    Public Class PmMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabAssets As TabPage
        Private tabSchedules As TabPage
        Private tabWorkOrders As TabPage
        Private tabReports As TabPage

        ' Tab Assets Controls
        Private dgvAssets As DataGridView
        Private btnAddAsset As Button

        ' Tab Schedules Controls
        Private dgvSchedules As DataGridView

        ' Tab WorkOrders Controls
        Private dgvWorkOrders As DataGridView
        Private btnCompleteWorkOrder As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _pmSvc As PmService
        Private _currentCompanyID As Integer

        Public Sub New()
            _pmSvc = New PmService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔧 سیستم جامع مدیریت نت، نگهداری و تعمیرات (Preventive Maintenance Management - PM)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Assets
            tabAssets = New TabPage() With {.Text = "🏭 شناسنامه تجهیزات و ماشین‌آلات"}
            InitializeAssetsTab()
            tabControl.TabPages.Add(tabAssets)

            ' 2. Tab Schedules
            tabSchedules = New TabPage() With {.Text = "📅 برنامه‌ریزی نت پیشگیرانه (PM)"}
            InitializeSchedulesTab()
            tabControl.TabPages.Add(tabSchedules)

            ' 3. Tab WorkOrders
            tabWorkOrders = New TabPage() With {.Text = "🛠️ دستورکارها و تعمیرات اضطراری (EM)"}
            InitializeWorkOrdersTab()
            tabControl.TabPages.Add(tabWorkOrders)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات OEE، MTBF و بهای تمام‌شده نت"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf PmMainForm_Load
        End Sub

        Private Sub PmMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadAssetsData()
            LoadSchedulesData()
            LoadWorkOrdersData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Assets Tab
        ' ----------------------------------------------------
        Private Sub InitializeAssetsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddAsset = New Button() With {
                .Text = "➕ ثبت شناسنامه ماشین‌آلات و تجهیز جدید",
                .Size = New Size(260, 36),
                .Location = New Point(920, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddAsset.Click, AddressOf BtnAddAsset_Click
            pnlTop.Controls.Add(btnAddAsset)

            dgvAssets = New DataGridView() With {
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
            dgvAssets.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvAssets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabAssets.Controls.Add(dgvAssets)
            tabAssets.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadAssetsData()
            Try
                Dim dt = _pmSvc.GetAssets(_currentCompanyID)
                dgvAssets.DataSource = dt
                SetupGridColumns(dgvAssets)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddAsset_Click(sender As Object, e As EventArgs)
            Using dlg As New PmEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadAssetsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Schedules Tab
        ' ----------------------------------------------------
        Private Sub InitializeSchedulesTab()
            dgvSchedules = New DataGridView() With {
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
            dgvSchedules.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvSchedules.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabSchedules.Controls.Add(dgvSchedules)
        End Sub

        Private Sub LoadSchedulesData()
            Try
                Dim dt = _pmSvc.GetSchedules(_currentCompanyID)
                dgvSchedules.DataSource = dt
                SetupGridColumns(dgvSchedules)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. WorkOrders Tab
        ' ----------------------------------------------------
        Private Sub InitializeWorkOrdersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnCompleteWorkOrder = New Button() With {
                .Text = "🏆 ثبت اتمام تعمیر و صدور سند حسابداری نت",
                .Size = New Size(340, 36),
                .Location = New Point(840, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCompleteWorkOrder.Click, AddressOf BtnCompleteWorkOrder_Click
            pnlTop.Controls.Add(btnCompleteWorkOrder)

            dgvWorkOrders = New DataGridView() With {
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
            dgvWorkOrders.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvWorkOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabWorkOrders.Controls.Add(dgvWorkOrders)
            tabWorkOrders.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadWorkOrdersData()
            Try
                Dim dt = _pmSvc.GetWorkOrders(_currentCompanyID)
                dgvWorkOrders.DataSource = dt
                SetupGridColumns(dgvWorkOrders)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnCompleteWorkOrder_Click(sender As Object, e As EventArgs)
            If dgvWorkOrders.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک دستورکار را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim woId = Convert.ToInt32(dgvWorkOrders.CurrentRow.Cells("WorkOrderID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _pmSvc.CompleteWorkOrderAndIssueSanad(woId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("دستورکار با موفقیت ثبت قطعی شد و سند حسابداری هزینه تعمیرات در پشت پرده صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadWorkOrdersData()
            Else
                MessageBox.Show("خطا در ثبت سند نت.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش جامع OEE و کارایی ماشین‌آلات",
                .Size = New Size(380, 36),
                .Location = New Point(790, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _pmSvc.GetOeeReport(_currentCompanyID)
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
                    {"AssetID", "شناسه تجهیز"},
                    {"AssetCode", "کد تجهیز"},
                    {"AssetName", "نام دستگاه/تجهیز"},
                    {"Category", "دسته‌بندی تجهیز"},
                    {"LocationName", "موقعیت استقرار"},
                    {"CostCenter", "مرکز هزینه"},
                    {"ScheduleID", "شناسه برنامه"},
                    {"TaskTitle", "عنوان سرویس/دستورکار"},
                    {"IntervalType", "نوع دوره"},
                    {"IntervalValue", "مقدار دوره"},
                    {"LastDoneDate", "تاریخ آخرین سرویس"},
                    {"NextDueDate", "تاریخ سررسید بعدی"},
                    {"WorkOrderID", "شناسه دستورکار"},
                    {"OrderType", "نوع دستورکار"},
                    {"Title", "عنوان تعمیرات"},
                    {"TechnicianName", "تکنسین مسئول"},
                    {"DowntimeHours", "ساعات توقف خط (Downtime)"},
                    {"PartsCost", "هزینه قطعات یدکی (ریال)"},
                    {"LaborCost", "هزینه دستمزد (ریال)"},
                    {"TotalCost", "مجموع هزینه نت (ریال)"},
                    {"StartDate", "تاریخ شروع"},
                    {"CompletionDate", "تاریخ اتمام"},
                    {"TotalDowntimeHours", "مجموع ساعات توقف خط"},
                    {"TotalMaintenanceOrders", "تعداد کل دستورکارها"},
                    {"TotalMaintenanceCost", "مجموع هزینه‌های نت (ریال)"},
                    {"OeePercentage", "شاخص اثربخشی کلی تجهیزات (%)"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("AssetID") Then dgv.Columns("AssetID").Visible = False
                If dgv.Columns.Contains("ScheduleID") Then dgv.Columns("ScheduleID").Visible = False
                If dgv.Columns.Contains("WorkOrderID") Then dgv.Columns("WorkOrderID").Visible = False
                If dgv.Columns.Contains("AssetCode") Then dgv.Columns("AssetCode").Width = 110
                If dgv.Columns.Contains("AssetName") Then dgv.Columns("AssetName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
