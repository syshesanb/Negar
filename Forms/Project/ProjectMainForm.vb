Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Project
    Public Class ProjectMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabProjects As TabPage
        Private tabWBS As TabPage
        Private tabClaims As TabPage
        Private tabGuarantees As TabPage
        Private tabReports As TabPage

        ' Tab Projects Controls
        Private dgvProjects As DataGridView
        Private btnAddProject As Button

        ' Tab WBS Controls
        Private dgvWBS As DataGridView

        ' Tab Claims Controls
        Private dgvClaims As DataGridView
        Private btnAddClaim As Button
        Private btnConfirmClaim As Button

        ' Tab Guarantees Controls
        Private dgvGuarantees As DataGridView

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _projSvc As ProjectService
        Private _currentCompanyID As Integer

        Public Sub New()
            _projSvc = New ProjectService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🏗️ سیستم جامع مدیریت پروژه‌ها و پیمان‌ها (Project & Contract Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Projects
            tabProjects = New TabPage() With {.Text = "🏗️ شناسنامه و فهرست پیمان‌ها"}
            InitializeProjectsTab()
            tabControl.TabPages.Add(tabProjects)

            ' 2. Tab WBS
            tabWBS = New TabPage() With {.Text = "📊 ساختار شکست کار (WBS) و کارکرد"}
            InitializeWBSTab()
            tabControl.TabPages.Add(tabWBS)

            ' 3. Tab Claims
            tabClaims = New TabPage() With {.Text = "📑 صورت‌وضعیت‌ها و کسورات قانونی"}
            InitializeClaimsTab()
            tabControl.TabPages.Add(tabClaims)

            ' 4. Tab Guarantees
            tabGuarantees = New TabPage() With {.Text = "🏦 مدیریت ضمانت‌نامه‌های بانکی"}
            InitializeGuaranteesTab()
            tabControl.TabPages.Add(tabGuarantees)

            ' 5. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات جامع پروژه‌ها و سود و زیان (P&L)"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf ProjectMainForm_Load
        End Sub

        Private Sub ProjectMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadProjectsData()
            LoadWBSdata()
            LoadClaimsData()
            LoadGuaranteesData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Projects Tab
        ' ----------------------------------------------------
        Private Sub InitializeProjectsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddProject = New Button() With {
                .Text = "➕ ثبت شناسنامه پیمان / پروژه جدید",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddProject.Click, AddressOf BtnAddProject_Click
            pnlTop.Controls.Add(btnAddProject)

            dgvProjects = New DataGridView() With {
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
            dgvProjects.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvProjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabProjects.Controls.Add(dgvProjects)
            tabProjects.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadProjectsData()
            Try
                Dim dt = _projSvc.GetProjects(_currentCompanyID)
                dgvProjects.DataSource = dt
                SetupGridColumns(dgvProjects)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddProject_Click(sender As Object, e As EventArgs)
            Using dlg As New ProjectEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadProjectsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. WBS Tab
        ' ----------------------------------------------------
        Private Sub InitializeWBSTab()
            dgvWBS = New DataGridView() With {
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
            dgvWBS.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvWBS.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabWBS.Controls.Add(dgvWBS)
        End Sub

        Private Sub LoadWBSdata()
            Try
                Dim dt = _projSvc.GetProjectWBS(_currentCompanyID)
                dgvWBS.DataSource = dt
                SetupGridColumns(dgvWBS)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Claims Tab
        ' ----------------------------------------------------
        Private Sub InitializeClaimsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddClaim = New Button() With {
                .Text = "➕ ثبت صورت‌وضعیت جدید",
                .Size = New Size(200, 36),
                .Location = New Point(980, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddClaim.Click, AddressOf BtnAddClaim_Click

            btnConfirmClaim = New Button() With {
                .Text = "🏆 تایید قطعی صورت‌وضعیت و صدور سند پیمانکاری",
                .Size = New Size(340, 36),
                .Location = New Point(620, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnConfirmClaim.Click, AddressOf BtnConfirmClaim_Click

            pnlTop.Controls.Add(btnAddClaim)
            pnlTop.Controls.Add(btnConfirmClaim)

            dgvClaims = New DataGridView() With {
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
            dgvClaims.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvClaims.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabClaims.Controls.Add(dgvClaims)
            tabClaims.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadClaimsData()
            Try
                Dim dt = _projSvc.GetProjectClaims(_currentCompanyID)
                dgvClaims.DataSource = dt
                SetupGridColumns(dgvClaims)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddClaim_Click(sender As Object, e As EventArgs)
            Dim claimNo = "CLM-" & (Environment.TickCount Mod 100).ToString()
            _projSvc.SaveClaim(0, _currentCompanyID, 1, claimNo, 2500000000, "صورت‌وضعیت موقت کارکرد")
            MessageBox.Show("صورت‌وضعیت جدید به شماره " & claimNo & " با احتساب کسورات قانونی ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadClaimsData()
        End Sub

        Private Sub BtnConfirmClaim_Click(sender As Object, e As EventArgs)
            If dgvClaims.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک صورت‌وضعیت را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim claimId = Convert.ToInt32(dgvClaims.CurrentRow.Cells("ClaimID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _projSvc.ConfirmClaim(claimId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("صورت‌وضعیت با موفقیت تایید قطعی گردید و سند حسابداری پیمانکاری آن در پشت پرده صادر شد.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadClaimsData()
            Else
                MessageBox.Show("خطا در تایید صورت‌وضعیت.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Guarantees Tab
        ' ----------------------------------------------------
        Private Sub InitializeGuaranteesTab()
            dgvGuarantees = New DataGridView() With {
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
            dgvGuarantees.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvGuarantees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabGuarantees.Controls.Add(dgvGuarantees)
        End Sub

        Private Sub LoadGuaranteesData()
            Try
                Dim dt = _projSvc.GetProjectGuarantees(_currentCompanyID)
                dgvGuarantees.DataSource = dt
                SetupGridColumns(dgvGuarantees)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 5. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش جامع سود و زیان پیمان‌ها و مطالبات (Project P&L)",
                .Size = New Size(420, 36),
                .Location = New Point(750, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _projSvc.GetProjectPLReport(_currentCompanyID)
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
                    {"ProjectID", "شناسه"},
                    {"ProjectCode", "کد پروژه"},
                    {"ProjectTitle", "عنوان پیمان / پروژه"},
                    {"EmployerName", "نام کارفرما"},
                    {"ContractAmount", "مبلغ اولیه پیمان" & vbCrLf & "(ریال)"},
                    {"AdvancePercent", "پیش‌پرداخت (%)"},
                    {"RetentionPercent", "سپرده (%)"},
                    {"InsurancePercent", "بیمه (%)"},
                    {"WbsID", "شناسه WBS"},
                    {"TaskCode", "کد فعالیت"},
                    {"TaskName", "عنوان فعالیت"},
                    {"PlannedWeight", "وزن برنامه (%)"},
                    {"ProgressPercent", "پیشرفت واقعی (%)"},
                    {"EstimatedCost", "برآورد هزینه (ریال)"},
                    {"ClaimID", "شناسه صورت‌وضعیت"},
                    {"ClaimNo", "شماره صورت‌وضعیت"},
                    {"GrossAmount", "مبلغ ناخالص" & vbCrLf & "(ریال)"},
                    {"AdvanceDeduction", "کسر پیش‌پرداخت" & vbCrLf & "(ریال)"},
                    {"RetentionDeduction", "کسر سپرده" & vbCrLf & "(ریال)"},
                    {"InsuranceDeduction", "کسر بیمه" & vbCrLf & "(ریال)"},
                    {"TaxDeduction", "کسر مالیات" & vbCrLf & "(ریال)"},
                    {"VatAmount", "ارزش افزوده" & vbCrLf & "(ریال)"},
                    {"NetAmount", "مبلغ خالص قابل دریافت" & vbCrLf & "(ریال)"},
                    {"TotalBilled", "کل صورت‌وضعیت‌ها" & vbCrLf & "(ریال)"},
                    {"TotalNetCollected", "کل خالص وصولی" & vbCrLf & "(ریال)"},
                    {"RemainingContract", "مانده پیمان" & vbCrLf & "(ریال)"},
                    {"GuaranteeID", "شناسه ضمانت"},
                    {"GuaranteeNo", "شماره ضمانت‌نامه"},
                    {"BankName", "بانک صادرکننده"},
                    {"GuaranteeType", "نوع ضمانت‌نامه"},
                    {"Amount", "مبلغ ضمانت‌نامه" & vbCrLf & "(ریال)"},
                    {"DueDate", "تاریخ سررسید"},
                    {"Status", "وضعیت"},
                    {"StartDate", "تاریخ شروع"},
                    {"EndDate", "تاریخ پایان"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("ProjectID") Then dgv.Columns("ProjectID").Visible = False
                If dgv.Columns.Contains("WbsID") Then dgv.Columns("WbsID").Visible = False
                If dgv.Columns.Contains("ClaimID") Then dgv.Columns("ClaimID").Visible = False
                If dgv.Columns.Contains("GuaranteeID") Then dgv.Columns("GuaranteeID").Visible = False
                If dgv.Columns.Contains("ProjectCode") Then dgv.Columns("ProjectCode").Width = 110
                If dgv.Columns.Contains("ProjectTitle") Then dgv.Columns("ProjectTitle").Width = 240
                If dgv.Columns.Contains("EmployerName") Then dgv.Columns("EmployerName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
