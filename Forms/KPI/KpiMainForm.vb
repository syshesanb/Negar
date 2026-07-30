Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.KPI
    Public Class KpiMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabTargets As TabPage
        Private tabAppraisals As TabPage
        Private tabBonuses As TabPage
        Private tabReports As TabPage

        ' Tab Targets Controls
        Private dgvTargets As DataGridView
        Private btnAddTarget As Button

        ' Tab Appraisals Controls
        Private dgvAppraisals As DataGridView
        Private btnAddAppraisal As Button

        ' Tab Bonuses Controls
        Private dgvBonuses As DataGridView
        Private btnConfirmBonus As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _kpiSvc As KpiService
        Private _currentCompanyID As Integer

        Public Sub New()
            _kpiSvc = New KpiService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🎯 سیستم جامع ارزیابی عملکرد و پاداش پرسنل (KPI & Performance Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Targets
            tabTargets = New TabPage() With {.Text = "🎯 بانک شاخص‌های KPI و هدف‌گذاری"}
            InitializeTargetsTab()
            tabControl.TabPages.Add(tabTargets)

            ' 2. Tab Appraisals
            tabAppraisals = New TabPage() With {.Text = "📊 ارزیابی دوره‌ای و ارزیابی ۳۶۰ درجه"}
            InitializeAppraisalsTab()
            tabControl.TabPages.Add(tabAppraisals)

            ' 3. Tab Bonuses
            tabBonuses = New TabPage() With {.Text = "💰 محاسبه پاداش و کارانه پرسنل"}
            InitializeBonusesTab()
            tabControl.TabPages.Add(tabBonuses)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات جامع عملکرد و ماتریس 9-Box"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf KpiMainForm_Load
        End Sub

        Private Sub KpiMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadTargetsData()
            LoadAppraisalsData()
            LoadBonusesData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Targets Tab
        ' ----------------------------------------------------
        Private Sub InitializeTargetsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddTarget = New Button() With {
                .Text = "➕ تعریف شاخص جدید (KPI Target)",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddTarget.Click, AddressOf BtnAddTarget_Click
            pnlTop.Controls.Add(btnAddTarget)

            dgvTargets = New DataGridView() With {
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
            dgvTargets.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvTargets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabTargets.Controls.Add(dgvTargets)
            tabTargets.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadTargetsData()
            Try
                Dim dt = _kpiSvc.GetKpiTargets(_currentCompanyID)
                dgvTargets.DataSource = dt
                SetupGridColumns(dgvTargets)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddTarget_Click(sender As Object, e As EventArgs)
            Using dlg As New KpiEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadTargetsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Appraisals Tab
        ' ----------------------------------------------------
        Private Sub InitializeAppraisalsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddAppraisal = New Button() With {
                .Text = "➕ ثبت ارزیابی ۳۶۰ درجه جدید",
                .Size = New Size(220, 36),
                .Location = New Point(960, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddAppraisal.Click, AddressOf BtnAddAppraisal_Click
            pnlTop.Controls.Add(btnAddAppraisal)

            dgvAppraisals = New DataGridView() With {
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
            dgvAppraisals.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvAppraisals.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabAppraisals.Controls.Add(dgvAppraisals)
            tabAppraisals.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadAppraisalsData()
            Try
                Dim dt = _kpiSvc.GetKpiEvaluations(_currentCompanyID)
                dgvAppraisals.DataSource = dt
                SetupGridColumns(dgvAppraisals)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddAppraisal_Click(sender As Object, e As EventArgs)
            _kpiSvc.SaveKpiEvaluation(_currentCompanyID, "مریم احمدی", 85, 90, "ارزیابی فصل بهار")
            MessageBox.Show("ارزیابی ۳۶۰ درجه جدید با نمره نهایی ۸۷.۵ (درجه B) ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadAppraisalsData()
        End Sub

        ' ----------------------------------------------------
        ' 3. Bonuses Tab
        ' ----------------------------------------------------
        Private Sub InitializeBonusesTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnConfirmBonus = New Button() With {
                .Text = "🏆 تایید نهایی پاداش و صدور سند و فیش حقوقی",
                .Size = New Size(340, 36),
                .Location = New Point(840, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnConfirmBonus.Click, AddressOf BtnConfirmBonus_Click
            pnlTop.Controls.Add(btnConfirmBonus)

            dgvBonuses = New DataGridView() With {
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
            dgvBonuses.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvBonuses.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabBonuses.Controls.Add(dgvBonuses)
            tabBonuses.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadBonusesData()
            Try
                Dim dt = _kpiSvc.GetKpiBonuses(_currentCompanyID)
                dgvBonuses.DataSource = dt
                SetupGridColumns(dgvBonuses)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnConfirmBonus_Click(sender As Object, e As EventArgs)
            If dgvBonuses.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک ردیف پاداش را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim bonusId = Convert.ToInt32(dgvBonuses.CurrentRow.Cells("BonusID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _kpiSvc.ConfirmAndTransferBonus(bonusId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("پاداش با موفقیت تایید نهایی شد و سند ذخیره پاداش پرسنل در پشت پرده صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBonusesData()
            Else
                MessageBox.Show("خطا در تایید پاداش.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش تحلیلی تحقق اهداف و رتبه‌بندی پرسنل",
                .Size = New Size(420, 36),
                .Location = New Point(750, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _kpiSvc.GetKpiPerformanceReport(_currentCompanyID)
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
                    {"TargetID", "شناسه شاخص"},
                    {"PersonnelName", "نام و نام خانوادگی پرسنل"},
                    {"TargetTitle", "عنوان شاخص / هدف"},
                    {"Category", "دسته شاخص"},
                    {"TargetValue", "مقدار هدف (Target)"},
                    {"ActualValue", "مقدار واقعی (Actual)"},
                    {"Weight", "وزن شاخص (%)"},
                    {"Unit", "واحد سنجش"},
                    {"PeriodName", "دوره ارزیابی"},
                    {"EvalID", "شناسه ارزیابی"},
                    {"EvalDate", "تاریخ ارزیابی"},
                    {"SelfScore", "امتیاز خودارزیابی"},
                    {"ManagerScore", "امتیاز مدیر"},
                    {"FinalScore", "امتیاز نهایی (۳۶۰)"},
                    {"PerformanceGrade", "رتبه عملکردی"},
                    {"BonusID", "شناسه پاداش"},
                    {"BaseAmount", "مبلغ پایه پاداش (ریال)"},
                    {"PerformanceFactor", "ضریب عملکرد"},
                    {"CalculatedBonus", "پاداش محاسبه‌شده (ریال)"},
                    {"TotalTargets", "تعداد کل شاخص‌ها"},
                    {"AvgAchievementRate", "میانگین تحقق اهداف (%)"},
                    {"OverallStatus", "وضعیت کلی"},
                    {"BonusDate", "تاریخ ثبت"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("TargetID") Then dgv.Columns("TargetID").Visible = False
                If dgv.Columns.Contains("EvalID") Then dgv.Columns("EvalID").Visible = False
                If dgv.Columns.Contains("BonusID") Then dgv.Columns("BonusID").Visible = False
                If dgv.Columns.Contains("PersonnelName") Then dgv.Columns("PersonnelName").Width = 180
                If dgv.Columns.Contains("TargetTitle") Then dgv.Columns("TargetTitle").Width = 240
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
