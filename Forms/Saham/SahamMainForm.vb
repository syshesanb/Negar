Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Saham
    Public Class SahamMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabShareholders As TabPage
        Private tabTransfers As TabPage
        Private tabDividends As TabPage
        Private tabReports As TabPage

        ' Tab Shareholders Controls
        Private dgvShareholders As DataGridView
        Private btnAddShareholder As Button

        ' Tab Transfers Controls
        Private dgvTransfers As DataGridView

        ' Tab Dividends Controls
        Private dgvDividends As DataGridView
        Private btnApproveDividend As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _sahamSvc As SahamService
        Private _currentCompanyID As Integer

        Public Sub New()
            _sahamSvc = New SahamService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🏛️ سیستم جامع امور سهام و سهامداران (Shareholders & Equity Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Shareholders
            tabShareholders = New TabPage() With {.Text = "🏢 دفتر ثبت و شناسنامه سهامداران"}
            InitializeShareholdersTab()
            tabControl.TabPages.Add(tabShareholders)

            ' 2. Tab Transfers
            tabTransfers = New TabPage() With {.Text = "🔄 نقل و انتقال سهام و دفتر معاملات حقوقی"}
            InitializeTransfersTab()
            tabControl.TabPages.Add(tabTransfers)

            ' 3. Tab Dividends
            tabDividends = New TabPage() With {.Text = "💰 مصوبات مجمع، تقسیم سود (DPS) و صدور سند مالی"}
            InitializeDividendsTab()
            tabControl.TabPages.Add(tabDividends)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات ترکیب سهامداران و مطالبات سود"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf SahamMainForm_Load
        End Sub

        Private Sub SahamMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadShareholdersData()
            LoadTransfersData()
            LoadDividendsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Shareholders Tab
        ' ----------------------------------------------------
        Private Sub InitializeShareholdersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddShareholder = New Button() With {
                .Text = "➕ ثبت شناسنامه سهامدار جدید",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddShareholder.Click, AddressOf BtnAddShareholder_Click
            pnlTop.Controls.Add(btnAddShareholder)

            dgvShareholders = New DataGridView() With {
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
            dgvShareholders.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvShareholders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabShareholders.Controls.Add(dgvShareholders)
            tabShareholders.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadShareholdersData()
            Try
                Dim dt = _sahamSvc.GetShareholders(_currentCompanyID)
                dgvShareholders.DataSource = dt
                SetupGridColumns(dgvShareholders)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddShareholder_Click(sender As Object, e As EventArgs)
            Using dlg As New SahamEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadShareholdersData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Transfers Tab
        ' ----------------------------------------------------
        Private Sub InitializeTransfersTab()
            dgvTransfers = New DataGridView() With {
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
            dgvTransfers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvTransfers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabTransfers.Controls.Add(dgvTransfers)
        End Sub

        Private Sub LoadTransfersData()
            Try
                Dim dt = _sahamSvc.GetTransfers(_currentCompanyID)
                dgvTransfers.DataSource = dt
                SetupGridColumns(dgvTransfers)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Dividends Tab
        ' ----------------------------------------------------
        Private Sub InitializeDividendsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnApproveDividend = New Button() With {
                .Text = "🏆 تصویب سود مجمع (DPS) و صدور سند حسابداری",
                .Size = New Size(370, 36),
                .Location = New Point(810, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnApproveDividend.Click, AddressOf BtnApproveDividend_Click
            pnlTop.Controls.Add(btnApproveDividend)

            dgvDividends = New DataGridView() With {
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
            dgvDividends.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvDividends.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabDividends.Controls.Add(dgvDividends)
            tabDividends.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadDividendsData()
            Try
                Dim dt = _sahamSvc.GetDividends(_currentCompanyID)
                dgvDividends.DataSource = dt
                SetupGridColumns(dgvDividends)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnApproveDividend_Click(sender As Object, e As EventArgs)
            If dgvDividends.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً ابتدا یک ردیف سود مصوب مجمع را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim divId = Convert.ToInt32(dgvDividends.CurrentRow.Cells("DividendID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1404"

            Dim res = _sahamSvc.ApproveDividendAndIssueSanad(divId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("سود مصوب مجمع با موفقیت ثبت نهایی گردید و سند حسابداری بدهی سود سهامداران صادر شد.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDividendsData()
            Else
                MessageBox.Show("خطا در صدور سند تقسیم سود مجمع.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش ترکیب سهامداران عمده و بدهی سود",
                .Size = New Size(390, 36),
                .Location = New Point(780, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub() LoadShareholdersData()
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
                    {"ShareholderID", "شناسه سهامدار"},
                    {"ShareholderCode", "کد سهامدار"},
                    {"FullName", "نام و نام خانوادگی / شرکت"},
                    {"NationalID", "کد ملی / شناسه ملی"},
                    {"ShareType", "نوع سهامدار"},
                    {"ShareCount", "تعداد سهام"},
                    {"NominalValue", "ارزش اسمی سهم (ریال)"},
                    {"TotalValue", "مبلغ کل سرمایه (ریال)"},
                    {"OwnershipPercent", "درصد مالکیت (%)"},
                    {"BankAccount", "شماره شبا پایا"},
                    {"DividendID", "شناسه سود"},
                    {"FiscalYearName", "سال مالی مجمع"},
                    {"TotalNetProfit", "سود خالص قابل تقسیم (ریال)"},
                    {"DividendPerShare", "سود مصوب هر سهم DPS (ریال)"},
                    {"TotalDividends", "مجموع سود تقسیم شده (ریال)"},
                    {"ApprovedDate", "تاریخ تصویب مجمع"},
                    {"Status", "وضعیت واریز"},
                    {"TransferID", "شناسه معامله"},
                    {"SellerName", "فروشنده سهام"},
                    {"BuyerName", "خریدار سهام"},
                    {"PricePerShare", "قیمت معامله هر سهم (ریال)"},
                    {"TotalAmount", "مبلغ کل معامله (ریال)"},
                    {"TransferDate", "تاریخ نقل و انتقال"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("ShareholderID") Then dgv.Columns("ShareholderID").Visible = False
                If dgv.Columns.Contains("DividendID") Then dgv.Columns("DividendID").Visible = False
                If dgv.Columns.Contains("TransferID") Then dgv.Columns("TransferID").Visible = False
                If dgv.Columns.Contains("FullName") Then dgv.Columns("FullName").Width = 220
                If dgv.Columns.Contains("BankAccount") Then dgv.Columns("BankAccount").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
