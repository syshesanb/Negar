Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.SRM
    Public Class SrmMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabSuppliers As TabPage
        Private tabRfqs As TabPage
        Private tabEvaluations As TabPage
        Private tabReports As TabPage

        ' Tab Suppliers Controls
        Private dgvSuppliers As DataGridView
        Private btnAddSupplier As Button

        ' Tab Rfqs Controls
        Private dgvRfqs As DataGridView

        ' Tab Evaluations Controls
        Private dgvEvaluations As DataGridView
        Private btnApproveEvaluation As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _srmSvc As SrmService
        Private _currentCompanyID As Integer

        Public Sub New()
            _srmSvc = New SrmService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🤝 سیستم جامع ارزیابی و مدیریت ارتباط با تامین‌کنندگان (Supplier Relationship Management - SRM)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Suppliers
            tabSuppliers = New TabPage() With {.Text = "🏢 بانک اطلاعات و شناسنامه تامین‌کنندگان"}
            InitializeSuppliersTab()
            tabControl.TabPages.Add(tabSuppliers)

            ' 2. Tab Rfqs
            tabRfqs = New TabPage() With {.Text = "📋 استعلام‌های قیمت خرید و مناقصات (RFQ)"}
            InitializeRfqsTab()
            tabControl.TabPages.Add(tabRfqs)

            ' 3. Tab Evaluations
            tabEvaluations = New TabPage() With {.Text = "📊 ارزیابی دوره‌ای و کارت امتیازی (Scorecard)"}
            InitializeEvaluationsTab()
            tabControl.TabPages.Add(tabEvaluations)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات OTD، انحراف قیمت و تحلیل تامین"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf SrmMainForm_Load
        End Sub

        Private Sub SrmMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadSuppliersData()
            LoadRfqsData()
            LoadEvaluationsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Suppliers Tab
        ' ----------------------------------------------------
        Private Sub InitializeSuppliersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddSupplier = New Button() With {
                .Text = "➕ ثبت تامین‌کننده جدید در بانک اطلاعات",
                .Size = New Size(260, 36),
                .Location = New Point(920, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddSupplier.Click, AddressOf BtnAddSupplier_Click
            pnlTop.Controls.Add(btnAddSupplier)

            dgvSuppliers = New DataGridView() With {
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
            dgvSuppliers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabSuppliers.Controls.Add(dgvSuppliers)
            tabSuppliers.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadSuppliersData()
            Try
                Dim dt = _srmSvc.GetSuppliers(_currentCompanyID)
                dgvSuppliers.DataSource = dt
                SetupGridColumns(dgvSuppliers)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddSupplier_Click(sender As Object, e As EventArgs)
            Using dlg As New SrmEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadSuppliersData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Rfqs Tab
        ' ----------------------------------------------------
        Private Sub InitializeRfqsTab()
            dgvRfqs = New DataGridView() With {
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
            dgvRfqs.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvRfqs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabRfqs.Controls.Add(dgvRfqs)
        End Sub

        Private Sub LoadRfqsData()
            Try
                Dim dt = _srmSvc.GetRfqs(_currentCompanyID)
                dgvRfqs.DataSource = dt
                SetupGridColumns(dgvRfqs)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Evaluations Tab
        ' ----------------------------------------------------
        Private Sub InitializeEvaluationsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnApproveEvaluation = New Button() With {
                .Text = "🏆 تایید نهایی ارزیابی و ثبت پاداش کیفی تامین‌کننده",
                .Size = New Size(370, 36),
                .Location = New Point(810, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnApproveEvaluation.Click, AddressOf BtnApproveEvaluation_Click
            pnlTop.Controls.Add(btnApproveEvaluation)

            dgvEvaluations = New DataGridView() With {
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
            dgvEvaluations.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvEvaluations.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabEvaluations.Controls.Add(dgvEvaluations)
            tabEvaluations.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadEvaluationsData()
            Try
                Dim dt = _srmSvc.GetEvaluations(_currentCompanyID)
                dgvEvaluations.DataSource = dt
                SetupGridColumns(dgvEvaluations)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnApproveEvaluation_Click(sender As Object, e As EventArgs)
            If dgvEvaluations.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک ردیف ارزیابی را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim evalId = Convert.ToInt32(dgvEvaluations.CurrentRow.Cells("EvalID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _srmSvc.ApproveEvaluationAndIssueSanad(evalId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("ارزیابی با موفقیت تایید نهایی شد، گرید تامین‌کننده ارتقا یافت و سند اعتبار کیفی صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadEvaluationsData()
                LoadSuppliersData()
            Else
                MessageBox.Show("خطا در ثبت تاییدیه ارزیابی تامین‌کننده.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج کارت امتیازی و ماتریس گریدبندی تامین‌کنندگان",
                .Size = New Size(420, 36),
                .Location = New Point(750, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _srmSvc.GetSrmReport(_currentCompanyID)
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
                    {"SupplierID", "شناسه تامین‌کننده"},
                    {"SupplierCode", "کد تامین‌کننده"},
                    {"SupplierName", "نام تامین‌کننده/شرکت"},
                    {"Category", "رسته/گروه کالا"},
                    {"Grade", "گرید ارزیابی"},
                    {"EconomicCode", "کد اقتصادی"},
                    {"Phone", "شماره تلفن تماس"},
                    {"RfqID", "شناسه استعلام"},
                    {"RfqNumber", "شماره استعلام (RFQ)"},
                    {"ItemName", "شرح کالای استعلام"},
                    {"Quantity", "مقدار/تیراژ خرید"},
                    {"WinnerSupplierName", "تامین‌کننده برنده استعلام"},
                    {"WinnerPrice", "مبلغ برنده استعلام (ریال)"},
                    {"CreationDate", "تاریخ شروع استعلام"},
                    {"ClosingDate", "تاریخ پایان/اعلام برنده"},
                    {"EvalID", "شناسه ارزیابی"},
                    {"EvaluationPeriod", "دوره ارزیابی"},
                    {"QualityScore", "نمره کیفیت (از ۱۰۰)"},
                    {"DeliveryScore", "نمره تحویل به موقع (از ۱۰۰)"},
                    {"PriceScore", "نمره قیمت رقابتی (از ۱۰۰)"},
                    {"FinalScore", "امتیاز کل (Scorecard)"},
                    {"AssignedGrade", "گرید مکتسبه"},
                    {"EvaluatorName", "ارزیاب مسئول"},
                    {"TotalEvaluations", "تعداد ارزیابی‌ها"},
                    {"AvgQualityScore", "میانگین نمره کیفیت"},
                    {"AvgDeliveryScore", "میانگین تحویل به موقع"},
                    {"OverallScore", "امتیاز نهایی کارت امتیازی"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("SupplierID") Then dgv.Columns("SupplierID").Visible = False
                If dgv.Columns.Contains("RfqID") Then dgv.Columns("RfqID").Visible = False
                If dgv.Columns.Contains("EvalID") Then dgv.Columns("EvalID").Visible = False
                If dgv.Columns.Contains("SupplierCode") Then dgv.Columns("SupplierCode").Width = 120
                If dgv.Columns.Contains("SupplierName") Then dgv.Columns("SupplierName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
