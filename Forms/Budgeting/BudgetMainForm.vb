Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Budgeting
    Public Class BudgetMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabBudgetItems As TabPage
        Private tabEnforcement As TabPage
        Private tabAmendments As TabPage
        Private tabLogs As TabPage
        Private tabReports As TabPage

        ' Tab BudgetItems Controls
        Private dgvBudgetItems As DataGridView
        Private btnAddBudgetItem As Button

        ' Tab Enforcement Controls
        Private dgvEnforcement As DataGridView
        Private btnRefreshEnforcement As Button

        ' Tab Amendments Controls
        Private btnAddAmendment As Button

        ' Tab Logs Controls
        Private dgvLogs As DataGridView

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _budgetSvc As BudgetingService
        Private _currentCompanyID As Integer

        Public Sub New()
            _budgetSvc = New BudgetingService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📊 سیستم جامع بودجه و کنترل هزینه‌ها (Budgeting & Cost Control)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab BudgetItems
            tabBudgetItems = New TabPage() With {.Text = "🎯 ردیف‌های بودجه مصوب سالانه"}
            InitializeBudgetItemsTab()
            tabControl.TabPages.Add(tabBudgetItems)

            ' 2. Tab Enforcement
            tabEnforcement = New TabPage() With {.Text = "⚠️ پایش و کنترل زنده انحرافات (Enforcement)"}
            InitializeEnforcementTab()
            tabControl.TabPages.Add(tabEnforcement)

            ' 3. Tab Amendments
            tabAmendments = New TabPage() With {.Text = "🔄 متمم بودجه و جابجایی اعتبار (Virement)"}
            InitializeAmendmentsTab()
            tabControl.TabPages.Add(tabAmendments)

            ' 4. Tab Logs
            tabLogs = New TabPage() With {.Text = "📋 سوابق مصرف بودجه در ماژول‌ها"}
            InitializeLogsTab()
            tabControl.TabPages.Add(tabLogs)

            ' 5. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات انحراف بودجه و انضباط مالی"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf BudgetMainForm_Load
        End Sub

        Private Sub BudgetMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadBudgetItemsData()
            LoadLogsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. BudgetItems Tab
        ' ----------------------------------------------------
        Private Sub InitializeBudgetItemsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddBudgetItem = New Button() With {
                .Text = "➕ ثبت ردیف بودجه مصوب جدید",
                .Size = New Size(230, 36),
                .Location = New Point(950, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddBudgetItem.Click, AddressOf BtnAddBudgetItem_Click
            pnlTop.Controls.Add(btnAddBudgetItem)

            dgvBudgetItems = New DataGridView() With {
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
            dgvBudgetItems.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvBudgetItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvBudgetItems.DataBindingComplete, Sub(s, e) SetupBudgetItemsGridColumns()

            tabBudgetItems.Controls.Add(dgvBudgetItems)
            tabBudgetItems.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadBudgetItemsData()
            Dim dt = _budgetSvc.GetBudgetItems(_currentCompanyID)
            dgvBudgetItems.DataSource = dt
            dgvEnforcement.DataSource = dt
        End Sub

        Private Sub SetupBudgetItemsGridColumns()
            If dgvBudgetItems.Columns.Contains("colRowIndex") Then Return

            Dim colRow As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex",
                .HeaderText = "ردیف",
                .Width = 50,
                .ReadOnly = True
            }
            dgvBudgetItems.Columns.Insert(0, colRow)

            For i As Integer = 0 To dgvBudgetItems.Rows.Count - 1
                dgvBudgetItems.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgvBudgetItems)
        End Sub

        Private Sub BtnAddBudgetItem_Click(sender As Object, e As EventArgs)
            Using dlg As New BudgetEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadBudgetItemsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Enforcement Tab
        ' ----------------------------------------------------
        Private Sub InitializeEnforcementTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnRefreshEnforcement = New Button() With {
                .Text = "🔄 پایش زنده وضعیت بودجه مراکزهزینه",
                .Size = New Size(280, 36),
                .Location = New Point(890, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnRefreshEnforcement.Click, Sub() LoadBudgetItemsData()
            pnlTop.Controls.Add(btnRefreshEnforcement)

            dgvEnforcement = New DataGridView() With {
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
            dgvEnforcement.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvEnforcement.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvEnforcement.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabEnforcement.Controls.Add(dgvEnforcement)
            tabEnforcement.Controls.Add(pnlTop)
        End Sub

        ' ----------------------------------------------------
        ' 3. Amendments Tab
        ' ----------------------------------------------------
        Private Sub InitializeAmendmentsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddAmendment = New Button() With {
                .Text = "➕ ثبت متمم بودجه / جابجایی اعتبار",
                .Size = New Size(260, 36),
                .Location = New Point(910, 8),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddAmendment.Click, AddressOf BtnAddAmendment_Click
            pnlTop.Controls.Add(btnAddAmendment)

            Dim lblInfo As New Label With {
                .Text = "در این قسمت می‌توانید جابجایی اعتبار بین ردیف‌های بودجه (Virement) یا افزایش و متمم بودجه سالانه را ثبت کنید.",
                .Location = New Point(20, 15),
                .AutoSize = True,
                .Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            }
            pnlTop.Controls.Add(lblInfo)

            tabAmendments.Controls.Add(pnlTop)
        End Sub

        Private Sub BtnAddAmendment_Click(sender As Object, e As EventArgs)
            If dgvBudgetItems.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک ردیف بودجه را از تب اول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim itemId = Convert.ToInt32(dgvBudgetItems.CurrentRow.Cells("BudgetItemID").Value)
            Dim res = _budgetSvc.AddAmendment(itemId, _currentCompanyID, "افزایش بودجه (متمم)", 50000000, "اصلاح و متمم بودجه بر اساس مصوبه هیئت مدیره")
            If res Then
                MessageBox.Show("متمم بودجه با موفقیت اعمال گردید و اعتبار ردیف بودجه بروزرسانی شد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBudgetItemsData()
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Logs Tab
        ' ----------------------------------------------------
        Private Sub InitializeLogsTab()
            dgvLogs = New DataGridView() With {
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
            dgvLogs.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvLogs.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabLogs.Controls.Add(dgvLogs)
        End Sub

        Private Sub LoadLogsData()
            dgvLogs.DataSource = _budgetSvc.GetBudgetLogs(_currentCompanyID)
        End Sub

        ' ----------------------------------------------------
        ' 5. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📈 استخراج گزارش ماتریسی انحراف بودجه (Variance)",
                .Size = New Size(320, 36),
                .Location = New Point(850, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub() dgvReport.DataSource = _budgetSvc.GetBudgetVarianceReport(_currentCompanyID)

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
                {"BudgetItemID", "شناسه"},
                {"CostCenter", "مرکز هزینه / واحد"},
                {"MoeinCode", "کد معین"},
                {"ItemTitle", "عنوان ردیف بودجه"},
                {"AllocatedBudget", "بودجه مصوب سالانه" & vbCrLf & "(ریال)"},
                {"UsedBudget", "عملکرد واقعی (مصرف)" & vbCrLf & "(ریال)"},
                {"RemainingBudget", "مانده اعتبار مجاز" & vbCrLf & "(ریال)"},
                {"UsagePercentStr", "درصد جذب (%)"},
                {"EnforcementStatus", "وضعیت پایش زنده"},
                {"FiscalYear", "سال مالی"},
                {"Status", "وضعیت"},
                {"LogID", "شناسه سابقه"},
                {"SourceModule", "ماژول ثبت‌کننده"},
                {"ExpenseAmount", "مبلغ هزینه" & vbCrLf & "(ریال)"},
                {"Description", "شرح عملیات"},
                {"CreatedAt", "تاریخ ثبت"},
                {"VarianceAmount", "مبلغ انحراف بودجه" & vbCrLf & "(ریال)"},
                {"VarianceType", "نوع انحراف (مساعد/نامساعد)"},
                {"Notes", "توضیحات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 140
            Next

            If dgv.Columns.Contains("BudgetItemID") Then dgv.Columns("BudgetItemID").Visible = False
            If dgv.Columns.Contains("LogID") Then dgv.Columns("LogID").Visible = False
            If dgv.Columns.Contains("CostCenter") Then dgv.Columns("CostCenter").Width = 180
            If dgv.Columns.Contains("ItemTitle") Then dgv.Columns("ItemTitle").Width = 240
            If dgv.Columns.Contains("EnforcementStatus") Then dgv.Columns("EnforcementStatus").Width = 180
        End Sub
    End Class
End Namespace
