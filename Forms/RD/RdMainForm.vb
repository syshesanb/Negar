Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.RD
    Public Class RdMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabProjects As TabPage
        Private tabFormulations As TabPage
        Private tabLabTests As TabPage
        Private tabPatents As TabPage
        Private tabReports As TabPage

        Private dgvProjects As DataGridView
        Private dgvFormulations As DataGridView
        Private dgvLabTests As DataGridView
        Private dgvPatents As DataGridView
        Private dgvReports As DataGridView

        Private _rdSvc As RdService
        Private _companyID As Integer

        Public Sub New()
            _rdSvc = New RdService()
            _companyID = SessionContext.CurrentCompanyID
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔬 سیستم جامع مدیریت تحقیق و توسعه و فرمولاسیون (R&D & Innovation Management)"
            Me.WindowState = FormWindowState.Maximized
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(244, 246, 250)

            tabControl = New TabControl() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.5!, FontStyle.Bold)}

            ' Tab 1 — Projects
            tabProjects = New TabPage("🔬 پروژه‌های NPD — Stage-Gate و Gate Reviews")
            InitProjectsTab()
            tabControl.TabPages.Add(tabProjects)

            ' Tab 2 — Formulations
            tabFormulations = New TabPage("🧪 فرمولاسیون محصول، BOM پژوهشی و Version Control")
            InitFormulationsTab()
            tabControl.TabPages.Add(tabFormulations)

            ' Tab 3 — Lab Tests
            tabLabTests = New TabPage("🧫 لاگ آزمایشگاهی، Pilot Test و نتایج کنترل کیفی")
            InitLabTestsTab()
            tabControl.TabPages.Add(tabLabTests)

            ' Tab 4 — Patents
            tabPatents = New TabPage("🏛️ مدیریت پتنت‌ها، اختراعات و مالکیت فکری (IPR)")
            InitPatentsTab()
            tabControl.TabPages.Add(tabPatents)

            ' Tab 5 — Reports
            tabReports = New TabPage("📊 گزارشات Innovation Funnel، ROI تحقیقات و IPR")
            InitReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf OnLoad
        End Sub

        Private Sub OnLoad(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadAll()
        End Sub

        Private Sub LoadAll()
            Try : dgvProjects.DataSource = _rdSvc.GetProjects(_companyID) : ApplyHeaders(dgvProjects) : Catch : End Try
            Try : dgvFormulations.DataSource = _rdSvc.GetFormulations(_companyID) : ApplyHeaders(dgvFormulations) : Catch : End Try
            Try : dgvLabTests.DataSource = _rdSvc.GetLabTests(_companyID) : ApplyHeaders(dgvLabTests) : Catch : End Try
            Try : dgvPatents.DataSource = _rdSvc.GetPatents(_companyID) : ApplyHeaders(dgvPatents) : Catch : End Try
            Try : dgvReports.DataSource = _rdSvc.GetProjects(_companyID) : ApplyHeaders(dgvReports) : Catch : End Try
        End Sub

        ' ─── Tab Initializers ───────────────────────────────────────────────

        Private Sub InitProjectsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(232, 238, 248)}

            Dim btnAdd As New Button With {
                .Text = "➕ تعریف پروژه NPD جدید",
                .Size = New Size(230, 36), .Location = New Point(900, 10),
                .BackColor = Color.FromArgb(13, 71, 161), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAdd.Click, Sub(s, e)
                Using dlg As New RdEditDialog(_companyID)
                    If dlg.ShowDialog() = DialogResult.OK Then
                        Try : dgvProjects.DataSource = _rdSvc.GetProjects(_companyID) : ApplyHeaders(dgvProjects) : Catch : End Try
                    End If
                End Using
            End Sub

            Dim lblInfo As New Label With {
                .Text = "💡 هر پروژه از ایده‌پردازی تا تجاری‌سازی در فازهای Stage-Gate مدیریت می‌شود",
                .Location = New Point(200, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(50, 90, 160), .Font = New Font("Tahoma", 9!)
            }

            pnl.Controls.AddRange(New Control() {btnAdd, lblInfo})
            dgvProjects = CreateGrid()
            tabProjects.Controls.Add(dgvProjects)
            tabProjects.Controls.Add(pnl)
        End Sub

        Private Sub InitFormulationsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(232, 248, 232)}
            Dim lblInfo As New Label With {
                .Text = "🔒 فرمول‌های سطح 'محرمانه' فقط برای کاربران مجاز قابل مشاهده است | Version Control کامل تاریخچه تغییرات فرمول",
                .Location = New Point(50, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(27, 94, 32), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvFormulations = CreateGrid()
            tabFormulations.Controls.Add(dgvFormulations)
            tabFormulations.Controls.Add(pnl)
        End Sub

        Private Sub InitLabTestsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(252, 248, 230)}
            Dim lblInfo As New Label With {
                .Text = "🧫 لاگ آزمایشگاهی دیجیتال — ثبت نتایج آزمون‌های فیزیکوشیمیایی، میکروبیولوژی و کاربردی | مقایسه با Target Specs",
                .Location = New Point(50, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(130, 80, 20), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvLabTests = CreateGrid()
            tabLabTests.Controls.Add(dgvLabTests)
            tabLabTests.Controls.Add(pnl)
        End Sub

        Private Sub InitPatentsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(240, 230, 252)}
            Dim lblInfo As New Label With {
                .Text = "🏛️ آرشیو پتنت‌ها و حق اختراع — هشدار تجدید پتنت ۱۲ ماه قبل از انقضا | پیگیری درآمد لایسنس فناوری",
                .Location = New Point(50, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(80, 20, 130), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvPatents = CreateGrid()
            tabPatents.Controls.Add(dgvPatents)
            tabPatents.Controls.Add(pnl)
        End Sub

        Private Sub InitReportsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(230, 240, 255)}
            Dim lblInfo As New Label With {
                .Text = "📊 Innovation Funnel — نرخ تبدیل ایده به محصول | Time-to-Market | ROI پروژه‌های R&D | عملکرد بودجه پژوهشی",
                .Location = New Point(50, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(13, 50, 120), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvReports = CreateGrid()
            tabReports.Controls.Add(dgvReports)
            tabReports.Controls.Add(pnl)
        End Sub

        ' ─── Helpers ────────────────────────────────────────────────────────

        Private Function CreateGrid() As DataGridView
            Return New DataGridView With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 48,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White,
                .RowTemplate = New DataGridViewRow() With {.Height = 30}
            }
        End Function

        Private Sub ApplyHeaders(dgv As DataGridView)
            Try
                If dgv Is Nothing OrElse dgv.Columns.Count = 0 Then Return
                Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                    {"colRowIndex", "#"}, {"ProjectID", "شناسه"}, {"ProjectCode", "کد پروژه"},
                    {"ProjectTitle", "عنوان پروژه NPD"}, {"Category", "دسته‌بندی"}, {"Stage", "مرحله Stage-Gate"},
                    {"LeadName", "سرپرست فنی (R&D Lead)"}, {"BudgetAmount", "بودجه (ریال)"}, {"SpentAmount", "هزینه‌شده (ریال)"},
                    {"Status", "وضعیت"}, {"CreatedAt", "تاریخ ایجاد"},
                    {"FormulationID", "شناسه"}, {"FormulationCode", "کد فرمول"}, {"Version", "نسخه"},
                    {"ComponentName", "نام ماده اولیه / جزء"}, {"Percentage", "درصد وزنی/حجمی"},
                    {"CasNumber", "شماره CAS"}, {"SecurityLevel", "سطح محرمانگی"}, {"Notes", "توضیحات"},
                    {"TestID", "شناسه"}, {"TestCode", "کد آزمایش"}, {"TestDate", "تاریخ آزمون"},
                    {"TestType", "نوع آزمون"}, {"Parameter", "پارامتر اندازه‌گیری"},
                    {"TargetValue", "مقدار هدف (Spec)"}, {"ActualValue", "مقدار واقعی"}, {"Result", "نتیجه"},
                    {"TechnicianName", "کارشناس آزمایشگاه"},
                    {"PatentID", "شناسه"}, {"PatentNo", "شماره ثبت پتنت"}, {"Title", "عنوان اختراع"},
                    {"RegisterDate", "تاریخ ثبت"}, {"ExpiryDate", "تاریخ انقضا"},
                    {"LicenseIncome", "درآمد لایسنس (ریال)"}
                }
                Dim hideIds = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {"ProjectID", "FormulationID", "TestID", "PatentID"}
                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then col.HeaderText = dict(col.Name)
                    If hideIds.Contains(col.Name) Then col.Visible = False Else col.Width = 145
                Next
                If dgv.Columns.Contains("ProjectTitle") Then dgv.Columns("ProjectTitle").Width = 280
                If dgv.Columns.Contains("ComponentName") Then dgv.Columns("ComponentName").Width = 220
                If dgv.Columns.Contains("Title") Then dgv.Columns("Title").Width = 260
                If dgv.Columns.Contains("colRowIndex") Then
                    dgv.Columns("colRowIndex").Width = 40
                    For i = 0 To dgv.Rows.Count - 1
                        If i < dgv.Rows.Count Then dgv.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
                    Next
                End If
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
