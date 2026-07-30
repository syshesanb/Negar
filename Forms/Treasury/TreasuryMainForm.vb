Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Treasury
    Public Class TreasuryMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabCashBanks As TabPage
        Private tabChecks As TabPage
        Private tabLoans As TabPage
        Private tabCashFlow As TabPage
        Private tabReports As TabPage

        ' Tab CashBanks Controls
        Private dgvCashBanks As DataGridView
        Private btnAddCashBank As Button

        ' Tab Checks Controls
        Private dgvChecks As DataGridView
        Private btnUpdateCheckStatus As Button

        ' Tab Loans Controls
        Private dgvLoans As DataGridView
        Private btnPayLoan As Button

        ' Tab CashFlow Controls
        Private dgvCashFlow As DataGridView
        Private btnRefreshCashFlow As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _treasurySvc As TreasuryService
        Private _currentCompanyID As Integer

        Public Sub New()
            _treasurySvc = New TreasuryService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "💰 سیستم جامع خزانه‌داری پیشرفته و مدیریت جریان نقدینگی (Cash Flow)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab CashBanks
            tabCashBanks = New TabPage() With {.Text = "🏦 بانک‌ها، صندوق‌ها و تنخواه‌گردان‌ها"}
            InitializeCashBanksTab()
            tabControl.TabPages.Add(tabCashBanks)

            ' 2. Tab Checks
            tabChecks = New TabPage() With {.Text = "📑 چرخه حیات چک‌ها و اسناد تجاری"}
            InitializeChecksTab()
            tabControl.TabPages.Add(tabChecks)

            ' 3. Tab Loans
            tabLoans = New TabPage() With {.Text = "💳 مدیریت تسهیلات و وام‌های بانکی"}
            InitializeLoansTab()
            tabControl.TabPages.Add(tabLoans)

            ' 4. Tab CashFlow
            tabCashFlow = New TabPage() With {.Text = "📈 پیش‌بینی جریان وجوه نقد (Cash Flow Matrix)"}
            InitializeCashFlowTab()
            tabControl.TabPages.Add(tabCashFlow)

            ' 5. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارشات جامع خزانه‌داری"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf TreasuryMainForm_Load
        End Sub

        Private Sub TreasuryMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadCashBanksData()
            LoadChecksData()
            LoadLoansData()
            LoadCashFlowData()
        End Sub

        ' ----------------------------------------------------
        ' 1. CashBanks Tab
        ' ----------------------------------------------------
        Private Sub InitializeCashBanksTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddCashBank = New Button() With {
                .Text = "➕ ثبت حساب بانکی / صندوق جدید",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddCashBank.Click, AddressOf BtnAddCashBank_Click
            pnlTop.Controls.Add(btnAddCashBank)

            dgvCashBanks = New DataGridView() With {
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
            dgvCashBanks.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCashBanks.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvCashBanks.DataBindingComplete, Sub(s, e) SetupCashBanksGridColumns()

            tabCashBanks.Controls.Add(dgvCashBanks)
            tabCashBanks.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadCashBanksData()
            dgvCashBanks.DataSource = _treasurySvc.GetCashBanks(_currentCompanyID)
        End Sub

        Private Sub SetupCashBanksGridColumns()
            If dgvCashBanks.Columns.Contains("colRowIndex") Then Return

            Dim colRow As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex",
                .HeaderText = "ردیف",
                .Width = 50,
                .ReadOnly = True
            }
            dgvCashBanks.Columns.Insert(0, colRow)

            For i As Integer = 0 To dgvCashBanks.Rows.Count - 1
                dgvCashBanks.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgvCashBanks)
        End Sub

        Private Sub BtnAddCashBank_Click(sender As Object, e As EventArgs)
            Using dlg As New TreasuryEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadCashBanksData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Checks Tab
        ' ----------------------------------------------------
        Private Sub InitializeChecksTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnUpdateCheckStatus = New Button() With {
                .Text = "🔄 تغییر وضعیت چک و صدور اتوماتیک سند حسابداری",
                .Size = New Size(340, 36),
                .Location = New Point(830, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnUpdateCheckStatus.Click, AddressOf BtnUpdateCheckStatus_Click
            pnlTop.Controls.Add(btnUpdateCheckStatus)

            dgvChecks = New DataGridView() With {
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
            dgvChecks.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvChecks.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvChecks.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabChecks.Controls.Add(dgvChecks)
            tabChecks.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadChecksData()
            dgvChecks.DataSource = _treasurySvc.GetChecks(_currentCompanyID)
        End Sub

        Private Sub BtnUpdateCheckStatus_Click(sender As Object, e As EventArgs)
            If dgvChecks.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک چک را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim chkId = Convert.ToInt32(dgvChecks.CurrentRow.Cells("CheckID").Value)
            Dim curStatus = Convert.ToString(dgvChecks.CurrentRow.Cells("Status").Value)

            Dim newStatus = If(curStatus.Contains("وصول") OrElse curStatus.Contains("پاس"), "دریافت شده", "وصول شده")

            Dim res = _treasurySvc.UpdateCheckStatus(chkId, _currentCompanyID, newStatus)
            If res Then
                MessageBox.Show("تغییر وضعیت چک با موفقیت انجام گردید و سند دوبل متوازن حسابداری آن به‌صورت اتوماتیک در پشت پرده صادر گشت.", "موفقیت ثبت سند", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadChecksData()
                LoadCashBanksData()
                LoadCashFlowData()
            Else
                MessageBox.Show("خطا در تغییر وضعیت چک.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 3. Loans Tab
        ' ----------------------------------------------------
        Private Sub InitializeLoansTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnPayLoan = New Button() With {
                .Text = "💳 ثبت پرداخت قسط وام و صدور سند حسابداری",
                .Size = New Size(320, 36),
                .Location = New Point(850, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnPayLoan.Click, AddressOf BtnPayLoan_Click
            pnlTop.Controls.Add(btnPayLoan)

            dgvLoans = New DataGridView() With {
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
            dgvLoans.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLoans.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvLoans.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabLoans.Controls.Add(dgvLoans)
            tabLoans.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadLoansData()
            dgvLoans.DataSource = _treasurySvc.GetLoans(_currentCompanyID)
        End Sub

        Private Sub BtnPayLoan_Click(sender As Object, e As EventArgs)
            If dgvLoans.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک وام را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim loanId = Convert.ToInt32(dgvLoans.CurrentRow.Cells("LoanID").Value)

            Dim res = _treasurySvc.PayLoanInstallment(loanId, _currentCompanyID)
            If res Then
                MessageBox.Show("پرداخت قسط وام با موفقیت ثبت شد و سند حسابداری مربوطه در پشت پرده صادر گردید.", "موفقیت ثبت قسط", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadLoansData()
                LoadCashFlowData()
            Else
                MessageBox.Show("خطا در ثبت پرداخت قسط وام.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. CashFlow Tab
        ' ----------------------------------------------------
        Private Sub InitializeCashFlowTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnRefreshCashFlow = New Button() With {
                .Text = "📈 محاسبه و بروزرسانی ماتریس پیش‌بینی Cash Flow",
                .Size = New Size(340, 36),
                .Location = New Point(830, 8),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnRefreshCashFlow.Click, Sub() LoadCashFlowData()
            pnlTop.Controls.Add(btnRefreshCashFlow)

            dgvCashFlow = New DataGridView() With {
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
            dgvCashFlow.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCashFlow.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvCashFlow.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabCashFlow.Controls.Add(dgvCashFlow)
            tabCashFlow.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadCashFlowData()
            dgvCashFlow.DataSource = _treasurySvc.GetCashFlowMatrix(_currentCompanyID)
        End Sub

        ' ----------------------------------------------------
        ' 5. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 دریافت گزارش جامع خزانه‌داری و تحلیل سررسیدها",
                .Size = New Size(320, 36),
                .Location = New Point(850, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub() dgvReport.DataSource = _treasurySvc.GetChecks(_currentCompanyID)

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
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                {"CashBankID", "شناسه"},
                {"Code", "کد حساب"},
                {"Title", "عنوان بانک / صندوق"},
                {"Type", "نوع حساب"},
                {"AccountNumber", "شماره حساب"},
                {"Shaba", "شماره شبا (IR)"},
                {"Balance", "موجودی فعلی" & vbCrLf & "(ریال)"},
                {"CheckID", "شناسه چک"},
                {"PayerPayeeName", "نام صادرکننده / دریافت‌کننده"},
                {"CheckNo", "شماره چک"},
                {"BankName", "نام بانک"},
                {"DueDate", "تاریخ سررسید"},
                {"Amount", "مبلغ چک / وام" & vbCrLf & "(ریال)"},
                {"CheckType", "نوع چک"},
                {"Status", "وضعیت فعلی"},
                {"LoanID", "شناسه وام"},
                {"ContractNo", "شماره قرارداد"},
                {"TotalAmount", "مبلغ کل تسهیلات" & vbCrLf & "(ریال)"},
                {"InterestRate", "نرخ سود (%)"},
                {"InstallmentCount", "تعداد اقساط"},
                {"MonthlyInstallment", "مبلغ هر قسط" & vbCrLf & "(ریال)"},
                {"StartDate", "تاریخ شروع"},
                {"PaidInstallments", "اقساط پرداخت‌شده"},
                {"PeriodTitle", "دوره پیش‌بینی"},
                {"ExpectedInflow", "ورودی نقدینگی" & vbCrLf & "(ریال)"},
                {"ExpectedOutflow", "خروجی نقدینگی" & vbCrLf & "(ریال)"},
                {"NetCashFlow", "خالص جریان نقدینگی" & vbCrLf & "(ریال)"},
                {"LiquidityStatus", "وضعیت نقدینگی"},
                {"Notes", "توضیحات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 135
            Next

            If dgv.Columns.Contains("CashBankID") Then dgv.Columns("CashBankID").Visible = False
            If dgv.Columns.Contains("CheckID") Then dgv.Columns("CheckID").Visible = False
            If dgv.Columns.Contains("LoanID") Then dgv.Columns("LoanID").Visible = False
            If dgv.Columns.Contains("Title") Then dgv.Columns("Title").Width = 200
            If dgv.Columns.Contains("PayerPayeeName") Then dgv.Columns("PayerPayeeName").Width = 200
            If dgv.Columns.Contains("Shaba") Then dgv.Columns("Shaba").Width = 220
        End Sub
    End Class
End Namespace
