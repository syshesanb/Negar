Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Drawing
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Partial Class HesabdaryForm
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Me.WindowState = FormWindowState.Maximized
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return

            If Not SessionContext.CurrentCompanyID.HasValue Then
                MessageBox.Show(
                    "برای استفاده از ماژول حسابداری، ابتدا باید شرکت و سال مالی جاری را انتخاب کنید." & Environment.NewLine &
                    "فرم انتخاب شرکت و سال مالی جاری باز می‌شود.",
                    "شرکت انتخاب نشده", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Dim mainForm = TryCast(Me.Owner, MainForm)
                Dim cfForm As New CompanyFiscalYearForm(mainForm, openOnSelectTab:=True)
                cfForm.ShowDialog(Me)

                If Not SessionContext.CurrentCompanyID.HasValue Then
                    Me.BeginInvoke(Sub() Me.Close())
                    Return
                End If
            End If

            LoadAllTabs()
        End Sub

        Private ReadOnly service As New AccountingService()
        Private _accountsForm As HesabdaryCodingForm
        Private _shenavarForm As ShenavarCodingForm
        Private _sanad1Form As HesabdarySanad1Form
        Private _trialForm As HesabdaryTarazForm
        Private _ledgerForm As HesabdaryDaftarForm
        Private _tarazShenavarForm As HesabdaryTarazShenavarForm
        Private _daftarShenavarForm As HesabdaryDaftarShenavarForm
        
        Private _reportsTabInitialized As Boolean = False
        Private lstCategories As ListBox
        Private dgvMappedAccounts As DataGridView
        Private btnAddToCategory As Button
        Private btnRemoveFromCategory As Button
        Private btnAutoMap As Button

        Private Sub LoadAllTabs()
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "بارگذاری سرفصل حساب‌ها...")

                tabs.TabPages.Clear()
                tabs.TabPages.Add(tabAccounts)
                tabs.TabPages.Add(tabShenavar)
                tabs.TabPages.Add(tabEntry)
                tabs.TabPages.Add(tabBankReconciliation)
                tabs.TabPages.Add(tabTrial)
                tabs.TabPages.Add(tabLedger)
                tabs.TabPages.Add(tabTarazShenavar)
                tabs.TabPages.Add(tabDaftarShenavar)
                tabs.TabPages.Add(tabReports)

                ApplySecurity()

                If tabs.TabPages.Contains(tabAccounts) Then
                    _accountsForm = New HesabdaryCodingForm()
                    HostForm(tabAccounts, _accountsForm)
                End If
                progress.UpdateProgress(30, "بارگذاری کدینگ شناور...")

                If tabs.TabPages.Contains(tabShenavar) Then
                    _shenavarForm = New ShenavarCodingForm()
                    HostForm(tabShenavar, _shenavarForm)
                End If
                progress.UpdateProgress(50, "بارگذاری لیست اسناد حسابداری...")

                If tabs.TabPages.Contains(tabEntry) Then
                    _sanad1Form = New HesabdarySanad1Form()
                    HostForm(tabEntry, _sanad1Form)
                End If

                If tabs.TabPages.Contains(tabBankReconciliation) Then
                    Dim bankRecForm As New HesabdaryMogBankForm()
                    HostForm(tabBankReconciliation, bankRecForm)
                End If
                progress.UpdateProgress(70, "بارگذاری فرم تراز آزمایشی...")

                If tabs.TabPages.Contains(tabTrial) Then
                    _trialForm = New HesabdaryTarazForm()
                    HostForm(tabTrial, _trialForm)
                End If

                If tabs.TabPages.Contains(tabLedger) Then
                    _ledgerForm = New HesabdaryDaftarForm()
                    HostForm(tabLedger, _ledgerForm)
                End If
                
                If tabs.TabPages.Contains(tabTarazShenavar) Then
                    _tarazShenavarForm = New HesabdaryTarazShenavarForm()
                    HostForm(tabTarazShenavar, _tarazShenavarForm)
                End If

                If tabs.TabPages.Contains(tabDaftarShenavar) Then
                    _daftarShenavarForm = New HesabdaryDaftarShenavarForm()
                    HostForm(tabDaftarShenavar, _daftarShenavarForm)
                End If
                progress.UpdateProgress(90, "اتمام تنظیم دسترسی‌های حسابداری...")

                If _trialForm IsNot Nothing AndAlso _ledgerForm IsNot Nothing Then
                    AddHandler _trialForm.AccountSelected, AddressOf OnTrialAccountSelected
                End If
                If _ledgerForm IsNot Nothing AndAlso _sanad1Form IsNot Nothing Then
                    AddHandler _ledgerForm.EditDocumentRequested, AddressOf OnLedgerEditDocumentRequested
                End If
                If _tarazShenavarForm IsNot Nothing AndAlso _daftarShenavarForm IsNot Nothing Then
                    AddHandler _tarazShenavarForm.ShenavarSelected, AddressOf OnTarazShenavarSelected
                End If
                If _daftarShenavarForm IsNot Nothing AndAlso _sanad1Form IsNot Nothing Then
                    AddHandler _daftarShenavarForm.EditDocumentRequested, AddressOf OnDaftarShenavarEditDocumentRequested
                End If

                progress.UpdateProgress(100, "اتمام بارگذاری فرم حسابداری")
            End Using
        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim hasGlobalAccounting = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAccounting)

            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingHeader)) Then
                tabs.TabPages.Remove(tabAccounts)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar)) Then
                tabs.TabPages.Remove(tabShenavar)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntry)) Then
                tabs.TabPages.Remove(tabEntry)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingBank)) Then
                tabs.TabPages.Remove(tabBankReconciliation)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalance)) Then
                tabs.TabPages.Remove(tabTrial)
                tabs.TabPages.Remove(tabTarazShenavar)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingLedger)) Then
                tabs.TabPages.Remove(tabLedger)
                tabs.TabPages.Remove(tabDaftarShenavar)
            End If
            If Not (hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingReports)) Then
                tabs.TabPages.Remove(tabReports)
            End If
        End Sub

        Private Sub OnTrialAccountSelected(accountId As Integer, accountCode As String, accountName As String, hasChildren As Boolean, allIds As List(Of Integer))
            tabs.SelectedTab = tabLedger
            _ledgerForm.LoadAccount(accountId, accountCode, accountName, hasChildren, allIds)
        End Sub

        Private Sub OnLedgerEditDocumentRequested(entryId As Integer, lineNumber As Integer?)
            tabs.SelectedTab = tabEntry
            _sanad1Form.OpenDocumentForEdit(entryId, lineNumber, returnToLedger:=True)
        End Sub

        Public Sub SwitchToLedgerTabAndRefresh()
            tabs.SelectedTab = tabLedger
            _ledgerForm.RefreshLedger()
        End Sub

        Private Sub OnTarazShenavarSelected(shenavarId As Integer, shenavarCode As String, shenavarName As String, hasChildren As Boolean, allIds As List(Of Integer))
            tabs.SelectedTab = tabDaftarShenavar
            _daftarShenavarForm.LoadShenavar(shenavarId, shenavarCode, shenavarName, hasChildren, allIds)
        End Sub

        Private Sub OnDaftarShenavarEditDocumentRequested(entryId As Integer, lineNumber As Integer?)
            tabs.SelectedTab = tabEntry
            _sanad1Form.OpenDocumentForEdit(entryId, lineNumber, returnToLedger:=False, returnToDaftarShenavar:=True)
        End Sub

        Public Sub SwitchToDaftarShenavarTabAndRefresh()
            tabs.SelectedTab = tabDaftarShenavar
            _daftarShenavarForm.RefreshLedger()
        End Sub

        Private Sub Tabs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabs.SelectedIndexChanged
            If tabs.SelectedTab Is tabAccounts AndAlso _accountsForm IsNot Nothing Then
                _accountsForm.RefreshData()
            ElseIf tabs.SelectedTab Is tabShenavar AndAlso _shenavarForm IsNot Nothing Then
                _shenavarForm.RefreshData()
            ElseIf tabs.SelectedTab Is tabEntry AndAlso _sanad1Form IsNot Nothing Then
                _sanad1Form.RefreshData()
            ElseIf tabs.SelectedTab Is tabTrial AndAlso _trialForm IsNot Nothing Then
                _trialForm.RefreshData()
            ElseIf tabs.SelectedTab Is tabLedger AndAlso _ledgerForm IsNot Nothing Then
                _ledgerForm.RefreshLedger()
            ElseIf tabs.SelectedTab Is tabTarazShenavar AndAlso _tarazShenavarForm IsNot Nothing Then
                _tarazShenavarForm.RefreshData()
            ElseIf tabs.SelectedTab Is tabDaftarShenavar AndAlso _daftarShenavarForm IsNot Nothing Then
                _daftarShenavarForm.RefreshLedger()
            ElseIf tabs.SelectedTab Is tabReports Then
                InitializeReportsTab()
            End If
        End Sub

        Private Sub InitializeReportsTab()
            If _reportsTabInitialized Then Return
            _reportsTabInitialized = True

            ' Clear the placeholder label
            tabReportIntroProfitLoss.Controls.Clear()

            ' Create SplitContainer
            Dim sc As New SplitContainer()
            sc.Dock = DockStyle.Fill
            sc.RightToLeft = RightToLeft.Yes
            sc.SplitterDistance = 350
            tabReportIntroProfitLoss.Controls.Add(sc)

            ' Right Panel (Panel1) - Category list
            Dim pnlRight As New Panel()
            pnlRight.Dock = DockStyle.Fill
            pnlRight.Padding = New Padding(10)
            sc.Panel1.Controls.Add(pnlRight)

            Dim lblCatTitle As New Label()
            lblCatTitle.Text = "بخش‌های گزارش عملکرد و سود و زیان:"
            lblCatTitle.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            lblCatTitle.Dock = DockStyle.Top
            lblCatTitle.Height = 25
            pnlRight.Controls.Add(lblCatTitle)

            lstCategories = New ListBox()
            lstCategories.Dock = DockStyle.Fill
            lstCategories.Font = New Font("Tahoma", 9.5!)
            lstCategories.ItemHeight = 22
            pnlRight.Controls.Add(lstCategories)

            ' Populate Categories
            lstCategories.Items.Add(New CategoryItem("GrossSales", "فروش ناخالص (درآمدهای عملیاتی)"))
            lstCategories.Items.Add(New CategoryItem("SalesReturn", "برگشت از فروش و تخفیفات"))
            lstCategories.Items.Add(New CategoryItem("GrossPurchases", "خرید ناخالص"))
            lstCategories.Items.Add(New CategoryItem("PurchaseReturn", "برگشت از خرید و تخفیفات"))
            lstCategories.Items.Add(New CategoryItem("DirectPurchaseExpense", "هزینه‌های مستقیم خرید (حمل خرید)"))
            lstCategories.Items.Add(New CategoryItem("OperatingExpense", "هزینه‌های اداری، عمومی و فروش"))
            lstCategories.Items.Add(New CategoryItem("OtherOperatingRevenue", "سایر درآمدهای عملیاتی"))
            lstCategories.Items.Add(New CategoryItem("NonOperatingRevenue", "سایر درآمدهای غیرعملیاتی"))
            lstCategories.Items.Add(New CategoryItem("NonOperatingExpense", "سایر هزینه‌های غیرعملیاتی و مالی"))

            ' Left Panel (Panel2) - Mapped Accounts
            Dim pnlLeft As New Panel()
            pnlLeft.Dock = DockStyle.Fill
            pnlLeft.Padding = New Padding(10)
            sc.Panel2.Controls.Add(pnlLeft)

            ' Actions bar at top of Panel2
            Dim pnlActions As New FlowLayoutPanel()
            pnlActions.Dock = DockStyle.Top
            pnlActions.Height = 40
            pnlActions.FlowDirection = FlowDirection.RightToLeft
            pnlLeft.Controls.Add(pnlActions)

            btnAddToCategory = New Button()
            btnAddToCategory.Text = "افزودن حساب"
            btnAddToCategory.BackColor = Color.FromArgb(200, 230, 200)
            btnAddToCategory.Height = 28
            btnAddToCategory.Width = 110
            btnAddToCategory.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnAddToCategory)

            btnRemoveFromCategory = New Button()
            btnRemoveFromCategory.Text = "حذف حساب"
            btnRemoveFromCategory.BackColor = Color.FromArgb(250, 200, 200)
            btnRemoveFromCategory.Height = 28
            btnRemoveFromCategory.Width = 110
            btnRemoveFromCategory.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnRemoveFromCategory)

            btnAutoMap = New Button()
            btnAutoMap.Text = "تخصیص هوشمند پیش‌فرض"
            btnAutoMap.BackColor = Color.FromArgb(200, 220, 250)
            btnAutoMap.Height = 28
            btnAutoMap.Width = 180
            btnAutoMap.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnAutoMap)

            ' Grid for mapped accounts
            dgvMappedAccounts = New DataGridView()
            dgvMappedAccounts.Dock = DockStyle.Fill
            dgvMappedAccounts.AllowUserToAddRows = False
            dgvMappedAccounts.AllowUserToDeleteRows = False
            dgvMappedAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvMappedAccounts.BackgroundColor = Color.White
            dgvMappedAccounts.RowHeadersVisible = False
            dgvMappedAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvMappedAccounts.MultiSelect = False
            dgvMappedAccounts.ReadOnly = True
            pnlLeft.Controls.Add(dgvMappedAccounts)

            ' Add Columns to Grid
            dgvMappedAccounts.Columns.Add("colAccCode", "کد حساب")
            dgvMappedAccounts.Columns("colAccCode").DataPropertyName = "AccountCode"
            dgvMappedAccounts.Columns("colAccCode").FillWeight = 30

            dgvMappedAccounts.Columns.Add("colAccName", "نام حساب")
            dgvMappedAccounts.Columns("colAccName").DataPropertyName = "AccountName"
            dgvMappedAccounts.Columns("colAccName").FillWeight = 70

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "colAccID"
            colId.DataPropertyName = "AccountID"
            colId.Visible = False
            dgvMappedAccounts.Columns.Add(colId)

            ' Register Event Handlers
            AddHandler lstCategories.SelectedIndexChanged, AddressOf LstCategories_SelectedIndexChanged
            AddHandler btnAddToCategory.Click, AddressOf BtnAddToCategory_Click
            AddHandler btnRemoveFromCategory.Click, AddressOf BtnRemoveFromCategory_Click
            AddHandler btnAutoMap.Click, AddressOf BtnAutoMap_Click

            ' Initial Load
            If lstCategories.Items.Count > 0 Then
                lstCategories.SelectedIndex = 0
            End If
        End Sub

        Private Sub LstCategories_SelectedIndexChanged(sender As Object, e As EventArgs)
            RefreshMappedAccountsGrid()
        End Sub

        Private Sub RefreshMappedAccountsGrid()
            If lstCategories.SelectedItem Is Nothing Then Return
            Dim selectedItem = CType(lstCategories.SelectedItem, CategoryItem)
            Dim allMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)
            
            Dim dv As New DataView(allMappings)
            dv.RowFilter = "CategoryKey = '" & selectedItem.Key & "'"
            dgvMappedAccounts.DataSource = dv
        End Sub

        Private Sub BtnRemoveFromCategory_Click(sender As Object, e As EventArgs)
            If dgvMappedAccounts.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک حساب را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            
            Dim accountId = Convert.ToInt32(dgvMappedAccounts.CurrentRow.Cells("colAccID").Value)
            Dim name = Convert.ToString(dgvMappedAccounts.CurrentRow.Cells("colAccName").Value)
            
            If MessageBox.Show("آیا مطمئن هستید که می‌خواهید اتصال حساب '" & name & "' را از این دسته قطع کنید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                service.DeleteProfitLossMapping(accountId)
                RefreshMappedAccountsGrid()
            End If
        End Sub

        Private Sub BtnAutoMap_Click(sender As Object, e As EventArgs)
            If MessageBox.Show("آیا مایل هستید سیستم به صورت خودکار حساب‌های سود و زیانی را دسته‌بندی کند؟ (حساب‌هایی که قبلاً تخصیص داده شده‌اند تغییر نخواهند کرد)", "تخصیص هوشمند پیش‌فرض", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                service.AutoMapProfitLossAccounts(SessionContext.CurrentCompanyID.Value)
                RefreshMappedAccountsGrid()
                MessageBox.Show("تخصیص هوشمند با موفقیت انجام شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub BtnAddToCategory_Click(sender As Object, e As EventArgs)
            If lstCategories.SelectedItem Is Nothing Then Return
            Dim selectedItem = CType(lstCategories.SelectedItem, CategoryItem)
            
            Using picker As New AccountPickerForm(SessionContext.CurrentCompanyID.Value)
                If picker.ShowDialog(Me) = DialogResult.OK AndAlso picker.SelectedAccountID > 0 Then
                    service.SaveProfitLossMapping(picker.SelectedAccountID, selectedItem.Key, SessionContext.CurrentCompanyID.Value)
                    RefreshMappedAccountsGrid()
                End If
            End Using
        End Sub

        Private Class CategoryItem
            Public Property Key As String
            Public Property DisplayName As String
            Public Sub New(k As String, d As String)
                Key = k
                DisplayName = d
            End Sub
            Public Overrides Function ToString() As String
                Return DisplayName
            End Function
        End Class

        Private Class AccountPickerForm
            Inherits Form

            Public SelectedAccountID As Integer = 0
            Private _companyId As Integer
            Private txtSearch As TextBox
            Private lstAccounts As ListBox
            Private btnSelect As Button
            Private btnCancel As Button
            Private _allAccounts As DataTable

            Public Sub New(companyId As Integer)
                _companyId = companyId
                InitializeControls()
                LoadAccounts()
            End Sub

            Private Sub InitializeControls()
                Me.Width = 450
                Me.Height = 550
                Me.Text = "انتخاب حساب معین استاندارد"
                Me.StartPosition = FormStartPosition.CenterParent
                Me.RightToLeft = RightToLeft.Yes
                Me.RightToLeftLayout = True
                Me.Font = New Font("Tahoma", 9.0!)

                Dim pnlTop As New Panel()
                pnlTop.Dock = DockStyle.Top
                pnlTop.Height = 50
                pnlTop.Padding = New Padding(10)
                Me.Controls.Add(pnlTop)

                Dim lblSearch As New Label()
                lblSearch.Text = "جستجو:"
                lblSearch.Dock = DockStyle.Right
                lblSearch.Width = 50
                lblSearch.TextAlign = ContentAlignment.MiddleRight
                pnlTop.Controls.Add(lblSearch)

                txtSearch = New TextBox()
                txtSearch.Dock = DockStyle.Fill
                pnlTop.Controls.Add(txtSearch)

                Dim pnlButtons As New FlowLayoutPanel()
                pnlButtons.Dock = DockStyle.Bottom
                pnlButtons.Height = 45
                pnlButtons.FlowDirection = FlowDirection.LeftToRight
                pnlButtons.Padding = New Padding(10)
                Me.Controls.Add(pnlButtons)

                btnCancel = New Button()
                btnCancel.Text = "انصراف"
                btnCancel.Width = 80
                btnCancel.Height = 25
                btnCancel.DialogResult = DialogResult.Cancel
                pnlButtons.Controls.Add(btnCancel)

                btnSelect = New Button()
                btnSelect.Text = "انتخاب"
                btnSelect.Width = 80
                btnSelect.Height = 25
                pnlButtons.Controls.Add(btnSelect)

                lstAccounts = New ListBox()
                lstAccounts.Dock = DockStyle.Fill
                lstAccounts.Font = New Font("Tahoma", 9.5!)
                lstAccounts.ItemHeight = 22
                Me.Controls.Add(lstAccounts)

                ' Event handlers
                AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged
                AddHandler btnSelect.Click, AddressOf BtnSelect_Click
                AddHandler lstAccounts.DoubleClick, AddressOf LstAccounts_DoubleClick
            End Sub

            Private Sub LoadAccounts()
                Try
                    ' Fetch active standard accounts NOT mapped yet
                    _allAccounts = Sql.ExecuteTable(
                        "SELECT AccountID, AccountCode, AccountName " &
                        "FROM ChartOfAccounts " &
                        "WHERE CompanyID = ? AND IsActive = 1 " &
                        "AND AccountID NOT IN (SELECT AccountID FROM ProfitLossMappings) " &
                        "ORDER BY AccountCode", _companyId)
                    FilterList()
                Catch
                End Try
            End Sub

            Private Sub FilterList()
                If _allAccounts Is Nothing Then Return
                lstAccounts.Items.Clear()
                
                Dim filterText = txtSearch.Text.Trim()
                
                For Each row As DataRow In _allAccounts.Rows
                    Dim id = Convert.ToInt32(row("AccountID"))
                    Dim code = Convert.ToString(row("AccountCode"))
                    Dim name = Convert.ToString(row("AccountName"))
                    
                    If String.IsNullOrEmpty(filterText) OrElse code.Contains(filterText) OrElse name.Contains(filterText) Then
                        lstAccounts.Items.Add(New AccountItem(id, code, name))
                    End If
                Next
            End Sub

            Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
                FilterList()
            End Sub

            Private Sub BtnSelect_Click(sender As Object, e As EventArgs)
                SelectActiveItem()
            End Sub

            Private Sub LstAccounts_DoubleClick(sender As Object, e As EventArgs)
                SelectActiveItem()
            End Sub

            Private Sub SelectActiveItem()
                If lstAccounts.SelectedItem Is Nothing Then Return
                Dim item = CType(lstAccounts.SelectedItem, AccountItem)
                Me.SelectedAccountID = item.ID
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Sub

            Private Class AccountItem
                Public Property ID As Integer
                Public Property Code As String
                Public Property Name As String
                Public Sub New(i As Integer, c As String, n As String)
                    ID = i
                    Code = c
                    Name = n
                End Sub
                Public Overrides Function ToString() As String
                    Return Code & " — " & Name
                End Function
            End Class
        End Class

        Private Sub HostForm(targetTab As TabPage, child As Form)
            child.TopLevel = False
            child.FormBorderStyle = FormBorderStyle.None
            child.Dock = DockStyle.Fill
            child.StartPosition = FormStartPosition.Manual
            child.Visible = True
            targetTab.Controls.Clear()
            targetTab.Controls.Add(child)
            child.Show()
            child.BringToFront()
        End Sub
    End Class
End Namespace
