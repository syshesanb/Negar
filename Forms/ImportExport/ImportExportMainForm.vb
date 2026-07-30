Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.ImportExport
    Public Class ImportExportMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabProformas As TabPage
        Private tabLCs As TabPage
        Private tabCustoms As TabPage
        Private tabReports As TabPage

        ' Tab Proformas Controls
        Private dgvProformas As DataGridView
        Private btnAddProforma As Button

        ' Tab LCs Controls
        Private dgvLCs As DataGridView

        ' Tab Customs Controls
        Private dgvCustoms As DataGridView
        Private btnConfirmLandedCost As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _impSvc As ImportExportService
        Private _currentCompanyID As Integer

        Public Sub New()
            _impSvc = New ImportExportService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🚢 سیستم جامع بازرگانی خارجی و واردات/صادرات (Commercial Import/Export Management)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Proformas
            tabProformas = New TabPage() With {.Text = "📋 پرونده‌های خرید خارجی و پروفرما (PI)"}
            InitializeProformasTab()
            tabControl.TabPages.Add(tabProformas)

            ' 2. Tab LCs
            tabLCs = New TabPage() With {.Text = "🏦 اعتبارات اسنادی (LC) و حواله‌جات ارزی"}
            InitializeLCsTab()
            tabControl.TabPages.Add(tabLCs)

            ' 3. Tab Customs
            tabCustoms = New TabPage() With {.Text = "🏛️ اظهارنامه‌های گمرکی و بهای تمام‌شده (Landed Cost)"}
            InitializeCustomsTab()
            tabControl.TabPages.Add(tabCustoms)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات جامع بهای تمام‌شده و نوسانات نرخ ارز"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf ImportExportMainForm_Load
        End Sub

        Private Sub ImportExportMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadProformasData()
            LoadLCsData()
            LoadCustomsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Proformas Tab
        ' ----------------------------------------------------
        Private Sub InitializeProformasTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddProforma = New Button() With {
                .Text = "➕ ثبت پرونده خرید خارجی / پروفرما جدید",
                .Size = New Size(260, 36),
                .Location = New Point(920, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddProforma.Click, AddressOf BtnAddProforma_Click
            pnlTop.Controls.Add(btnAddProforma)

            dgvProformas = New DataGridView() With {
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
            dgvProformas.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvProformas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabProformas.Controls.Add(dgvProformas)
            tabProformas.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadProformasData()
            Try
                Dim dt = _impSvc.GetProformas(_currentCompanyID)
                dgvProformas.DataSource = dt
                SetupGridColumns(dgvProformas)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddProforma_Click(sender As Object, e As EventArgs)
            Using dlg As New ImportExportEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadProformasData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. LCs Tab
        ' ----------------------------------------------------
        Private Sub InitializeLCsTab()
            dgvLCs = New DataGridView() With {
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
            dgvLCs.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLCs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabLCs.Controls.Add(dgvLCs)
        End Sub

        Private Sub LoadLCsData()
            Try
                Dim dt = _impSvc.GetLCs(_currentCompanyID)
                dgvLCs.DataSource = dt
                SetupGridColumns(dgvLCs)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Customs Tab
        ' ----------------------------------------------------
        Private Sub InitializeCustomsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnConfirmLandedCost = New Button() With {
                .Text = "🏆 محاسبات بهای تمام‌شده (Landed Cost) و صدور سند رسید انبار",
                .Size = New Size(420, 36),
                .Location = New Point(760, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnConfirmLandedCost.Click, AddressOf BtnConfirmLandedCost_Click
            pnlTop.Controls.Add(btnConfirmLandedCost)

            dgvCustoms = New DataGridView() With {
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
            dgvCustoms.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCustoms.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabCustoms.Controls.Add(dgvCustoms)
            tabCustoms.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadCustomsData()
            Try
                Dim dt = _impSvc.GetCustoms(_currentCompanyID)
                dgvCustoms.DataSource = dt
                SetupGridColumns(dgvCustoms)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnConfirmLandedCost_Click(sender As Object, e As EventArgs)
            If dgvCustoms.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک پرونده ترخیص را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim customsId = Convert.ToInt32(dgvCustoms.CurrentRow.Cells("CustomsID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _impSvc.CalculateLandedCostAndConfirm(customsId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("بهای تمام‌شده واقعی (Landed Cost) با تسهیم کلیه هزینه‌ها محاسبه و سند ورود به انبار در پشت پرده صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadCustomsData()
            Else
                MessageBox.Show("خطا در محاسبه بهای تمام‌شده.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش آنالیز بهای تمام‌شده واقعی خرید‌های خارجی (Landed Cost Report)",
                .Size = New Size(480, 36),
                .Location = New Point(690, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _impSvc.GetLandedCostReport(_currentCompanyID)
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
                    {"ProformaID", "شناسه پروفرما"},
                    {"PINumber", "شماره پروفرما (PI)"},
                    {"SupplierName", "تامین‌کننده خارجی"},
                    {"CurrencyCode", "کد ارز"},
                    {"CurrencyRate", "نرخ تسعیر (ریال)"},
                    {"CurrencyAmount", "مبلغ ارزی"},
                    {"IrrAmount", "مبلغ ریالی پروفرما"},
                    {"Incoterms", "اینکوترمز"},
                    {"PIDate", "تاریخ پروفرما"},
                    {"LcID", "شناسه LC"},
                    {"LcNumber", "شماره اعتبار اسنادی (LC)"},
                    {"BankName", "بانک عامل"},
                    {"AdvancePayment", "پیش‌پرداخت ارزی"},
                    {"IssueDate", "تاریخ گشایش"},
                    {"CustomsID", "شناسه گمرک"},
                    {"DeclarationNo", "شماره اظهارنامه"},
                    {"CustomsName", "گمرک ترخیص‌کننده"},
                    {"DutyAmount", "حقوق ورودی" & vbCrLf & "(ریال)"},
                    {"VatAmount", "ارزش افزوده گمرک" & vbCrLf & "(ریال)"},
                    {"ShippingCost", "هزینه حمل بین‌المللی" & vbCrLf & "(ریال)"},
                    {"ClearanceCost", "هزینه ترخیص‌کاری" & vbCrLf & "(ریال)"},
                    {"TotalExtraCosts", "مجموع هزینه‌های جانبی" & vbCrLf & "(ریال)"},
                    {"BaseCost", "ارزش پایه کالا (ریال)"},
                    {"CustomsDuty", "حقوق و عوارض (ریال)"},
                    {"FreightCost", "حمل و بیمه (ریال)"},
                    {"TotalLandedCost", "بهای تمام‌شده واقعی" & vbCrLf & "(Landed Cost)"},
                    {"ExpensePercentage", "درصد هزینه‌های جانبی (%)"},
                    {"ClearanceDate", "تاریخ ترخیص"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("ProformaID") Then dgv.Columns("ProformaID").Visible = False
                If dgv.Columns.Contains("LcID") Then dgv.Columns("LcID").Visible = False
                If dgv.Columns.Contains("CustomsID") Then dgv.Columns("CustomsID").Visible = False
                If dgv.Columns.Contains("PINumber") Then dgv.Columns("PINumber").Width = 130
                If dgv.Columns.Contains("SupplierName") Then dgv.Columns("SupplierName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
