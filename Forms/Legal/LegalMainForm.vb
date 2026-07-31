Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Legal
    Public Class LegalMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabCases As TabPage
        Private tabHearings As TabPage
        Private tabLawyers As TabPage
        Private tabReports As TabPage

        ' Tab Cases Controls
        Private dgvCases As DataGridView
        Private btnAddCase As Button

        ' Tab Hearings Controls
        Private dgvHearings As DataGridView

        ' Tab Lawyers Controls
        Private dgvLawyers As DataGridView

        ' Tab Reports Controls
        Private dgvReports As DataGridView

        Private _legalSvc As LegalService
        Private _currentCompanyID As Integer

        Public Sub New()
            _legalSvc = New LegalService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "⚖️ سیستم جامع مدیریت امور حقوقی، قراردادها و دعاوی (Legal & Claims Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Cases
            tabCases = New TabPage() With {.Text = "📂 شناسنامه پرونده‌های حقوقی و دعاوی قضایی"}
            InitializeCasesTab()
            tabControl.TabPages.Add(tabCases)

            ' 2. Tab Hearings
            tabHearings = New TabPage() With {.Text = "📅 تقویم جلسات دادگاه و مهلت‌های تجدیدنظرخواهی"}
            InitializeHearingsTab()
            tabControl.TabPages.Add(tabHearings)

            ' 3. Tab Lawyers
            tabLawyers = New TabPage() With {.Text = "👨‍⚖️ مدیریت وکلا، کارشناسان رسمی و حق‌الوکاله‌ها"}
            InitializeLawyersTab()
            tabControl.TabPages.Add(tabLawyers)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارش ارزیابی ریسک مالی پرونده‌ها (Financial Risk Analysis)"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf LegalMainForm_Load
        End Sub

        Private Sub LegalMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadCasesData()
            LoadHearingsData()
            LoadLawyersData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Cases Tab
        ' ----------------------------------------------------
        Private Sub InitializeCasesTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddCase = New Button() With {
                .Text = "➕ تشکیل پرونده قضایی و حقوقی جدید",
                .Size = New Size(260, 36),
                .Location = New Point(920, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddCase.Click, AddressOf BtnAddCase_Click
            pnlTop.Controls.Add(btnAddCase)

            dgvCases = New DataGridView() With {
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
            dgvCases.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCases.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabCases.Controls.Add(dgvCases)
            tabCases.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadCasesData()
            Try
                Dim dt = _legalSvc.GetCases(_currentCompanyID)
                dgvCases.DataSource = dt
                SetupGridColumns(dgvCases)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddCase_Click(sender As Object, e As EventArgs)
            Using dlg As New LegalEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadCasesData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Hearings Tab
        ' ----------------------------------------------------
        Private Sub InitializeHearingsTab()
            dgvHearings = New DataGridView() With {
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
            dgvHearings.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvHearings.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabHearings.Controls.Add(dgvHearings)
        End Sub

        Private Sub LoadHearingsData()
            Try
                Dim dt = _legalSvc.GetHearings(_currentCompanyID)
                dgvHearings.DataSource = dt
                SetupGridColumns(dgvHearings)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Lawyers Tab
        ' ----------------------------------------------------
        Private Sub InitializeLawyersTab()
            dgvLawyers = New DataGridView() With {
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
            dgvLawyers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLawyers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabLawyers.Controls.Add(dgvLawyers)
        End Sub

        Private Sub LoadLawyersData()
            Try
                Dim dt = _legalSvc.GetLawyers(_currentCompanyID)
                dgvLawyers.DataSource = dt
                SetupGridColumns(dgvLawyers)
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
                    {"CaseID", "شناسه پرونده"},
                    {"CaseCode", "کد کلاسه پرونده"},
                    {"CaseTitle", "موضوع دعوا / پرونده"},
                    {"Claimant", "خواهان (شاکی)"},
                    {"Defendant", "خوانده (متشاکی)"},
                    {"CourtBranch", "مرجع قضایی / شعبه دادگاه"},
                    {"ClaimAmount", "مبلغ خواسته (ریال)"},
                    {"Status", "وضعیت پرونده"},
                    {"CreatedAt", "تاریخ تشکیل پرونده"},
                    {"HearingID", "شناسه جلسه"},
                    {"HearingDate", "تاریخ جلسه دادگاه"},
                    {"HearingTime", "ساعت دادرسی"},
                    {"LawyerName", "وکیل پرونده"},
                    {"Subject", "موضوع دادرسی"},
                    {"LawyerID", "شناسه وکیل"},
                    {"LicenseNo", "شماره پروانه وکالت"},
                    {"FeeContractAmount", "مبلغ حق‌الوکاله (ریال)"},
                    {"PaidAmount", "مبلغ پرداختی (ریال)"},
                    {"RemainingAmount", "مانده بدهی حق‌الوکاله (ریال)"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("CaseID") Then dgv.Columns("CaseID").Visible = False
                If dgv.Columns.Contains("HearingID") Then dgv.Columns("HearingID").Visible = False
                If dgv.Columns.Contains("LawyerID") Then dgv.Columns("LawyerID").Visible = False
                If dgv.Columns.Contains("CaseTitle") Then dgv.Columns("CaseTitle").Width = 240
                If dgv.Columns.Contains("CourtBranch") Then dgv.Columns("CourtBranch").Width = 220
                If dgv.Columns.Contains("LawyerName") Then dgv.Columns("LawyerName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
