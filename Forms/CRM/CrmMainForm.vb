Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.CRM
    Public Class CrmMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabCustomers As TabPage
        Private tabOpportunities As TabPage
        Private tabActivities As TabPage
        Private tabTickets As TabPage
        Private tabReports As TabPage

        ' Tab Customers Controls
        Private dgvCustomers As DataGridView
        Private btnAddCustomer As Button

        ' Tab Opportunities Controls
        Private dgvOpportunities As DataGridView
        Private btnConvertInvoice As Button

        ' Tab Activities Controls
        Private dgvActivities As DataGridView

        ' Tab Tickets Controls
        Private dgvTickets As DataGridView

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _crmSvc As CrmService
        Private _currentCompanyID As Integer

        Public Sub New()
            _crmSvc = New CrmService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🤝 سیستم جامع مدیریت ارتباط با مشتریان (CRM) و فروش"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Customers
            tabCustomers = New TabPage() With {.Text = "👤 پرونده ۳۶۰ درجه مشتریان"}
            InitializeCustomersTab()
            tabControl.TabPages.Add(tabCustomers)

            ' 2. Tab Opportunities
            tabOpportunities = New TabPage() With {.Text = "🎯 قیف فروش و فرصت‌های معامله"}
            InitializeOpportunitiesTab()
            tabControl.TabPages.Add(tabOpportunities)

            ' 3. Tab Activities
            tabActivities = New TabPage() With {.Text = "📞 ثبت فعالیت‌ها و پیگیری‌ها"}
            InitializeActivitiesTab()
            tabControl.TabPages.Add(tabActivities)

            ' 4. Tab Tickets
            tabTickets = New TabPage() With {.Text = "🎫 خدمات پس از فروش و تیکتینگ"}
            InitializeTicketsTab()
            tabControl.TabPages.Add(tabTickets)

            ' 5. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارشات جامع CRM و فروش"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf CrmMainForm_Load
        End Sub

        Private Sub CrmMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadCustomersData()
            LoadOpportunitiesData()
            LoadActivitiesData()
            LoadTicketsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Customers Tab
        ' ----------------------------------------------------
        Private Sub InitializeCustomersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddCustomer = New Button() With {
                .Text = "➕ ثبت پرونده جدید در CRM",
                .Size = New Size(210, 36),
                .Location = New Point(970, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddCustomer.Click, AddressOf BtnAddCustomer_Click
            pnlTop.Controls.Add(btnAddCustomer)

            dgvCustomers = New DataGridView() With {
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
            dgvCustomers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCustomers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvCustomers.DataBindingComplete, Sub(s, e) SetupCustomersGridColumns()
            AddHandler dgvCustomers.CellContentClick, AddressOf DgvCustomers_CellContentClick

            tabCustomers.Controls.Add(dgvCustomers)
            tabCustomers.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadCustomersData()
            Dim dt = _crmSvc.GetCustomers(_currentCompanyID)
            dgvCustomers.DataSource = dt
        End Sub

        Private Sub SetupCustomersGridColumns()
            If dgvCustomers.Columns.Contains("colRowIndex") Then Return

            Dim colRow As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex",
                .HeaderText = "ردیف",
                .Width = 50,
                .ReadOnly = True
            }
            dgvCustomers.Columns.Insert(0, colRow)

            Dim colEdit As New DataGridViewButtonColumn() With {
                .Name = "colEdit",
                .HeaderText = "ویرایش",
                .Text = "✏️ ویرایش",
                .UseColumnTextForButtonValue = True,
                .Width = 85
            }
            dgvCustomers.Columns.Insert(1, colEdit)

            Dim colDelete As New DataGridViewButtonColumn() With {
                .Name = "colDelete",
                .HeaderText = "حذف",
                .Text = "❌ حذف",
                .UseColumnTextForButtonValue = True,
                .Width = 75
            }
            dgvCustomers.Columns.Insert(2, colDelete)

            For i As Integer = 0 To dgvCustomers.Rows.Count - 1
                dgvCustomers.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgvCustomers)
        End Sub

        Private Sub DgvCustomers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return

            Dim colName = dgvCustomers.Columns(e.ColumnIndex).Name
            Dim customerID = Convert.ToInt32(dgvCustomers.Rows(e.RowIndex).Cells("CrmCustomerID").Value)

            If colName = "colEdit" Then
                Using dlg As New CrmCustomerEditDialog(_currentCompanyID, customerID)
                    If dlg.ShowDialog() = DialogResult.OK Then LoadCustomersData()
                End Using
            ElseIf colName = "colDelete" Then
                If MessageBox.Show("آیا از حذف این پرونده مشتری در CRM اطمینان دارید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    _crmSvc.DeleteCustomer(customerID, _currentCompanyID)
                    LoadCustomersData()
                End If
            End If
        End Sub

        Private Sub BtnAddCustomer_Click(sender As Object, e As EventArgs)
            Using dlg As New CrmCustomerEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadCustomersData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Opportunities Tab
        ' ----------------------------------------------------
        Private Sub InitializeOpportunitiesTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnConvertInvoice = New Button() With {
                .Text = "🏆 تبدیل به فاکتور فروش و صدور اتوماتیک سند حسابداری",
                .Size = New Size(340, 36),
                .Location = New Point(830, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnConvertInvoice.Click, AddressOf BtnConvertInvoice_Click
            pnlTop.Controls.Add(btnConvertInvoice)

            dgvOpportunities = New DataGridView() With {
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
            dgvOpportunities.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvOpportunities.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvOpportunities.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabOpportunities.Controls.Add(dgvOpportunities)
            tabOpportunities.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadOpportunitiesData()
            dgvOpportunities.DataSource = _crmSvc.GetOpportunities(_currentCompanyID)
        End Sub

        Private Sub BtnConvertInvoice_Click(sender As Object, e As EventArgs)
            If dgvOpportunities.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک فرصت فروش را از جدول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim oppId = Convert.ToInt32(dgvOpportunities.CurrentRow.Cells("OpportunityID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _crmSvc.ConvertOpportunityToInvoice(oppId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("معامله با موفقیت به «برنده شده» تغییر وضعیت داد و فاکتور فروش و سند حسابداری مربوطه در پشت پرده به‌صورت اتوماتیک صادر گردید.", "موفقیت صدور سند", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadOpportunitiesData()
                LoadCustomersData()
            Else
                MessageBox.Show("خطا در تبدیل معامله به فاکتور فروش.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 3. Activities Tab
        ' ----------------------------------------------------
        Private Sub InitializeActivitiesTab()
            dgvActivities = New DataGridView() With {
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
            dgvActivities.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvActivities.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvActivities.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabActivities.Controls.Add(dgvActivities)
        End Sub

        Private Sub LoadActivitiesData()
            dgvActivities.DataSource = _crmSvc.GetActivities(_currentCompanyID)
        End Sub

        ' ----------------------------------------------------
        ' 4. Tickets Tab
        ' ----------------------------------------------------
        Private Sub InitializeTicketsTab()
            dgvTickets = New DataGridView() With {
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
            dgvTickets.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvTickets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvTickets.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabTickets.Controls.Add(dgvTickets)
        End Sub

        Private Sub LoadTicketsData()
            dgvTickets.DataSource = _crmSvc.GetTickets(_currentCompanyID)
        End Sub

        ' ----------------------------------------------------
        ' 5. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 دریافت گزارش جامع CRM و تحلیل فروش",
                .Size = New Size(270, 36),
                .Location = New Point(900, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                                                dgvReport.DataSource = _crmSvc.GetCrmReports(_currentCompanyID)
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
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                {"CrmCustomerID", "شناسه"},
                {"CustomerCode", "کد مشتری"},
                {"FullName", "نام و نام خانوادگی / شرکت"},
                {"CustomerName", "نام مشتری"},
                {"Phone", "تلفن ثابت"},
                {"Mobile", "تلفن همراه"},
                {"Email", "پست الکترونیک"},
                {"Category", "نوع مخاطب"},
                {"LeadSource", "منبع آشنایی"},
                {"Status", "وضعیت CRM"},
                {"CustomerStatus", "وضعیت مخاطب"},
                {"OpportunityID", "شناسه معامله"},
                {"Title", "عنوان معامله / پروژه"},
                {"EstimatedValue", "ارزش تخمینی" & vbCrLf & "(ریال)"},
                {"WinProbabilityTitle", "احتمال موفقیت"},
                {"Stage", "مرحله قیف فروش"},
                {"ExpectedCloseDate", "تاریخ بستر خروجی"},
                {"ActivityID", "شناسه پیگیری"},
                {"ActivityType", "نوع پیگیری"},
                {"ActivityDate", "تاریخ پیگیری"},
                {"Subject", "موضوع"},
                {"Details", "جزییات"},
                {"TicketID", "شناسه تیکت"},
                {"TicketNo", "شماره تیکت"},
                {"Priority", "اولویت"},
                {"ContentBody", "متن درخواست"},
                {"TotalOpportunities", "تعداد معاملات"},
                {"TotalPipelineValue", "ارزش کل معاملات" & vbCrLf & "(ریال)"},
                {"Notes", "توضیحات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 130
            Next

            If dgv.Columns.Contains("CrmCustomerID") Then dgv.Columns("CrmCustomerID").Visible = False
            If dgv.Columns.Contains("OpportunityID") Then dgv.Columns("OpportunityID").Visible = False
            If dgv.Columns.Contains("ActivityID") Then dgv.Columns("ActivityID").Visible = False
            If dgv.Columns.Contains("TicketID") Then dgv.Columns("TicketID").Visible = False
            If dgv.Columns.Contains("CustomerCode") Then dgv.Columns("CustomerCode").Width = 110
            If dgv.Columns.Contains("FullName") Then dgv.Columns("FullName").Width = 200
            If dgv.Columns.Contains("Title") Then dgv.Columns("Title").Width = 220
        End Sub
    End Class
End Namespace
