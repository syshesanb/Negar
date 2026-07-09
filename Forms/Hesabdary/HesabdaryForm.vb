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
        Private dgvReports As DataGridView
        Private btnAutoMap As Button
        Private btnSaveFormat As Button
        Private btnDeleteRow As Button
        Private btnAddToCategories As Button
        Private lblChainTitle As Label
        Private _rootNodes As New List(Of PLNode)()

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

            ' Create Top Actions Panel
            Dim pnlTopActions As New Panel()
            pnlTopActions.Dock = DockStyle.Top
            pnlTopActions.Height = 45
            pnlTopActions.BackColor = Color.FromArgb(235, 243, 255)
            pnlTopActions.Padding = New Padding(10, 8, 10, 8)
            tabReportIntroProfitLoss.Controls.Add(pnlTopActions)

            ' lblChainTitle inside pnlTopActions
            lblChainTitle = New Label()
            lblChainTitle.Dock = DockStyle.Fill
            lblChainTitle.TextAlign = ContentAlignment.MiddleRight
            lblChainTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            lblChainTitle.ForeColor = Color.FromArgb(50, 70, 100)
            lblChainTitle.Text = ""
            pnlTopActions.Controls.Add(lblChainTitle)

            ' Action buttons in top panel (docked Left)
            btnSaveFormat = New Button()
            btnSaveFormat.Text = "ذخیره فرمت گزارش"
            btnSaveFormat.Dock = DockStyle.Left
            btnSaveFormat.Width = 140
            btnSaveFormat.BackColor = Color.FromArgb(200, 240, 200)
            btnSaveFormat.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlTopActions.Controls.Add(btnSaveFormat)

            Dim pnlSpacing1 As New Panel()
            pnlSpacing1.Dock = DockStyle.Left
            pnlSpacing1.Width = 10
            pnlTopActions.Controls.Add(pnlSpacing1)

            btnDeleteRow = New Button()
            btnDeleteRow.Text = "حذف سطر"
            btnDeleteRow.Dock = DockStyle.Left
            btnDeleteRow.Width = 100
            btnDeleteRow.BackColor = Color.FromArgb(250, 210, 210)
            btnDeleteRow.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlTopActions.Controls.Add(btnDeleteRow)

            Dim pnlSpacing2 As New Panel()
            pnlSpacing2.Dock = DockStyle.Left
            pnlSpacing2.Width = 10
            pnlTopActions.Controls.Add(pnlSpacing2)

            btnAddToCategories = New Button()
            btnAddToCategories.Text = "افزودن سطر"
            btnAddToCategories.Dock = DockStyle.Left
            btnAddToCategories.Width = 100
            btnAddToCategories.BackColor = Color.FromArgb(215, 235, 255)
            btnAddToCategories.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlTopActions.Controls.Add(btnAddToCategories)

            Dim pnlSpacing3 As New Panel()
            pnlSpacing3.Dock = DockStyle.Left
            pnlSpacing3.Width = 10
            pnlTopActions.Controls.Add(pnlSpacing3)

            btnAutoMap = New Button()
            btnAutoMap.Text = "تخصیص هوشمند پیش‌فرض"
            btnAutoMap.Dock = DockStyle.Left
            btnAutoMap.Width = 180
            btnAutoMap.BackColor = Color.FromArgb(220, 230, 250)
            btnAutoMap.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlTopActions.Controls.Add(btnAutoMap)

            ' Create DataGridView for Tree representation
            dgvReports = New DataGridView()
            dgvReports.Dock = DockStyle.Fill
            dgvReports.AllowUserToAddRows = False
            dgvReports.AllowUserToDeleteRows = False
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvReports.BackgroundColor = Color.White
            dgvReports.RowHeadersVisible = False
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReports.MultiSelect = False
            dgvReports.ReadOnly = False
            dgvReports.RowTemplate.Height = 26
            tabReportIntroProfitLoss.Controls.Add(dgvReports)

            ' Add Columns
            Dim colToggle As New DataGridViewTextBoxColumn()
            colToggle.Name = "colToggle"
            colToggle.HeaderText = "+ / -"
            colToggle.Width = 45
            colToggle.ReadOnly = True
            colToggle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colToggle)

            Dim colRowNo As New DataGridViewTextBoxColumn()
            colRowNo.Name = "colRowNo"
            colRowNo.HeaderText = "ردیف"
            colRowNo.Width = 60
            colRowNo.ReadOnly = True
            colRowNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colRowNo)

            Dim colCategory As New DataGridViewTextBoxColumn()
            colCategory.Name = "colCategory"
            colCategory.HeaderText = "بخش‌های گزارش عملکرد و سود و زیان"
            colCategory.Width = 280
            colCategory.ReadOnly = False
            dgvReports.Columns.Add(colCategory)

            Dim colAdd As New DataGridViewButtonColumn()
            colAdd.Name = "colAdd"
            colAdd.HeaderText = "افزودن سرفصل"
            colAdd.Width = 110
            colAdd.ReadOnly = True
            dgvReports.Columns.Add(colAdd)

            Dim colRemove As New DataGridViewButtonColumn()
            colRemove.Name = "colRemove"
            colRemove.HeaderText = "حذف سرفصل"
            colRemove.Width = 110
            colRemove.ReadOnly = True
            dgvReports.Columns.Add(colRemove)

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colCode"
            colCode.HeaderText = "کد سرفصل"
            colCode.Width = 100
            colCode.ReadOnly = True
            colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colCode)

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colName"
            colName.HeaderText = "نام سرفصل"
            colName.Width = 250
            colName.ReadOnly = True
            dgvReports.Columns.Add(colName)

            Dim colID As New DataGridViewTextBoxColumn()
            colID.Name = "colID"
            colID.Visible = False
            colID.ReadOnly = True
            dgvReports.Columns.Add(colID)

            ' Register Event Handlers
            AddHandler btnAutoMap.Click, AddressOf BtnAutoMap_Click
            AddHandler btnAddToCategories.Click, AddressOf BtnAddCategoryRow_Click
            AddHandler btnDeleteRow.Click, AddressOf BtnDeleteCategoryRow_Click
            AddHandler btnSaveFormat.Click, AddressOf BtnSaveFormat_Click
            
            AddHandler dgvReports.CellContentClick, AddressOf DgvReports_CellContentClick
            AddHandler dgvReports.CellDoubleClick, AddressOf DgvReports_CellDoubleClick
            AddHandler dgvReports.SelectionChanged, AddressOf DgvReports_SelectionChanged
            AddHandler dgvReports.CellEndEdit, AddressOf DgvReports_CellEndEdit

            ' Load Tree Nodes
            LoadTreeData()
            BuildAndRefreshGrid()
        End Sub

        Private Sub LoadTreeData()
            Dim expansionStates As New Dictionary(Of String, Boolean)()
            For Each r In _rootNodes
                expansionStates(r.CategoryName) = r.IsExpanded
            Next
            _rootNodes.Clear()
            
            Dim dtCats = service.GetProfitLossCategories(SessionContext.CurrentCompanyID.Value)
            If dtCats.Rows.Count = 0 Then
                service.BootstrapDefaultProfitLossFormat(SessionContext.CurrentCompanyID.Value)
                dtCats = service.GetProfitLossCategories(SessionContext.CurrentCompanyID.Value)
            End If

            Dim allMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)

            For Each rowCat As DataRow In dtCats.Rows
                Dim catId = Convert.ToInt32(rowCat("CategoryID"))
                Dim catName = Convert.ToString(rowCat("CategoryName"))
                
                Dim parent As New PLNode()
                parent.CategoryID = catId
                parent.CategoryName = catName
                parent.IsCategory = True
                
                If expansionStates.ContainsKey(parent.CategoryName) Then
                    parent.IsExpanded = expansionStates(parent.CategoryName)
                Else
                    parent.IsExpanded = True
                End If

                Dim dv As New DataView(allMappings)
                dv.RowFilter = "CategoryID = " & catId
                For Each row As DataRowView In dv
                    Dim child As New PLNode()
                    child.AccountID = Convert.ToInt32(row("AccountID"))
                    child.AccountCode = Convert.ToString(row("AccountCode"))
                    child.AccountName = Convert.ToString(row("AccountName"))
                    child.IsCategory = False
                    
                    parent.Children.Add(child)
                Next
                
                _rootNodes.Add(parent)
            Next
        End Sub

        Private Sub BuildAndRefreshGrid()
            If dgvReports Is Nothing Then Return
            
            dgvReports.SuspendLayout()
            dgvReports.Rows.Clear()
            
            Dim displayList As New List(Of PLNode)()
            For Each root In _rootNodes
                displayList.Add(root)
                If root.IsExpanded Then
                    For Each child In root.Children
                        displayList.Add(child)
                    Next
                End If
            Next

            For i As Integer = 0 To displayList.Count - 1
                Dim node = displayList(i)
                Dim rowIdx = dgvReports.Rows.Add()
                Dim row = dgvReports.Rows(rowIdx)
                row.Tag = node
                
                row.Cells("colToggle").Value = If(node.IsCategory, If(node.IsExpanded, "－", "＋"), "")
                row.Cells("colRowNo").Value = i + 1
                row.Cells("colCategory").Value = If(node.IsCategory, node.CategoryName, "")
                row.Cells("colCode").Value = If(node.IsCategory, "", node.AccountCode)
                row.Cells("colName").Value = If(node.IsCategory, "", node.AccountName)
                row.Cells("colID").Value = If(node.IsCategory, 0, node.AccountID)
                
                If node.IsCategory Then
                    row.Cells("colCategory").ReadOnly = False
                    row.Cells("colAdd").Value = "افزودن سرفصل"
                    row.Cells("colRemove") = New DataGridViewTextBoxCell()
                    row.Cells("colRemove").Value = ""
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 243, 255)
                    row.DefaultCellStyle.Font = New Font(dgvReports.Font, FontStyle.Bold)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 50, 100)
                Else
                    row.Cells("colCategory").ReadOnly = True
                    row.Cells("colAdd") = New DataGridViewTextBoxCell()
                    row.Cells("colAdd").Value = ""
                    row.Cells("colRemove").Value = "حذف سرفصل"
                    row.DefaultCellStyle.BackColor = Color.White
                    row.DefaultCellStyle.Font = New Font(dgvReports.Font, FontStyle.Regular)
                    row.DefaultCellStyle.ForeColor = Color.Black
                End If
            Next
            
            dgvReports.ResumeLayout()
            UpdateReportsChainLabel()
        End Sub

        Private Sub DgvReports_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return
            
            Dim colName = dgvReports.Columns(e.ColumnIndex).Name
            
            If colName = "colToggle" AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            ElseIf colName = "colAdd" AndAlso node.IsCategory Then
                Using picker As New AccountPickerForm(SessionContext.CurrentCompanyID.Value)
                    If picker.ShowDialog(Me) = DialogResult.OK AndAlso picker.SelectedAccountID > 0 Then
                        Try
                            Dim dtAcc = Sql.ExecuteTable("SELECT AccountCode, AccountName FROM ChartOfAccounts WHERE AccountID = ?", picker.SelectedAccountID)
                            If dtAcc.Rows.Count > 0 Then
                                Dim child As New PLNode()
                                child.AccountID = picker.SelectedAccountID
                                child.AccountCode = Convert.ToString(dtAcc.Rows(0)("AccountCode"))
                                child.AccountName = Convert.ToString(dtAcc.Rows(0)("AccountName"))
                                child.IsCategory = False
                                
                                node.Children.Add(child)
                                BuildAndRefreshGrid()
                            End If
                        Catch ex As Exception
                            MessageBox.Show("خطا در افزودن حساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End Using
            ElseIf colName = "colRemove" AndAlso Not node.IsCategory Then
                If MessageBox.Show("آیا مطمئن هستید که می‌خواهید اتصال حساب '" & node.AccountName & "' را از این دسته قطع کنید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    For Each root In _rootNodes
                        If root.Children.Contains(node) Then
                            root.Children.Remove(node)
                            Exit For
                        End If
                    Next
                    BuildAndRefreshGrid()
                End If
            End If
        End Sub

        Private Sub DgvReports_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            End If
        End Sub

        Private Sub DgvReports_SelectionChanged(sender As Object, e As EventArgs)
            UpdateReportsChainLabel()
        End Sub

        Private Sub UpdateReportsChainLabel()
            If lblChainTitle Is Nothing Then Return
            If dgvReports.CurrentRow Is Nothing Then
                lblChainTitle.Text = ""
                Return
            End If
            
            Dim row = dgvReports.CurrentRow
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing OrElse node.IsCategory OrElse node.AccountID <= 0 Then
                lblChainTitle.Text = ""
                Return
            End If
            
            Try
                Dim chain = service.GetAccountHierarchyChain(node.AccountID)
                Dim parts As New List(Of String)()
                For Each item In chain
                    parts.Add(item.Item1 & " — " & item.Item2)
                Next
                lblChainTitle.Text = "زنجیره سرفصل:  " & String.Join("  /  ", parts.ToArray())
            Catch
                lblChainTitle.Text = ""
            End Try
        End Sub

        Private Sub BtnAutoMap_Click(sender As Object, e As EventArgs)
            If MessageBox.Show("آیا مایل هستید سیستم به صورت خودکار حساب‌های سود و زیانی را دسته‌بندی کند؟ (حساب‌هایی که قبلاً تخصیص داده شده‌اند تغییر نخواهند کرد)", "تخصیص هوشمند پیش‌فرض", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                service.AutoMapProfitLossAccounts(SessionContext.CurrentCompanyID.Value)
                LoadTreeData()
                BuildAndRefreshGrid()
                MessageBox.Show("تخصیص هوشمند با موفقیت انجام شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub BtnAddCategoryRow_Click(sender As Object, e As EventArgs)
            Dim newCat As New PLNode()
            newCat.CategoryID = 0
            newCat.CategoryName = "بخش جدید"
            newCat.IsCategory = True
            newCat.IsExpanded = True
            _rootNodes.Add(newCat)
            BuildAndRefreshGrid()
            
            For i As Integer = 0 To dgvReports.Rows.Count - 1
                Dim row = dgvReports.Rows(i)
                Dim node = TryCast(row.Tag, PLNode)
                If node Is newCat Then
                    dgvReports.CurrentCell = row.Cells("colCategory")
                    dgvReports.BeginEdit(True)
                    Exit For
                End If
            Next
        End Sub

        Private Sub BtnDeleteCategoryRow_Click(sender As Object, e As EventArgs)
            If dgvReports.CurrentRow Is Nothing Then Return
            Dim row = dgvReports.CurrentRow
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return
            
            If node.IsCategory Then
                If MessageBox.Show("آیا مطمئن هستید که می‌خواهید بخش '" & node.CategoryName & "' را به همراه تمام حساب‌های متصل به آن حذف کنید؟", "تایید حذف بخش", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    _rootNodes.Remove(node)
                    BuildAndRefreshGrid()
                End If
            Else
                MessageBox.Show("برای قطع اتصال حساب از دکمه 'حذف سرفصل' در همان سطر استفاده کنید.", "راهنمایی", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub BtnSaveFormat_Click(sender As Object, e As EventArgs)
            Try
                Dim dtoList As New List(Of PLNodeDto)()
                For Each root In _rootNodes
                    Dim dto As New PLNodeDto()
                    dto.CategoryName = root.CategoryName
                    For Each child In root.Children
                        dto.AccountIDs.Add(child.AccountID)
                    Next
                    dtoList.Add(dto)
                Next
                
                service.SaveProfitLossFormat(SessionContext.CurrentCompanyID.Value, dtoList)
                LoadTreeData()
                BuildAndRefreshGrid()
                
                MessageBox.Show("فرمت گزارش سود و زیان با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره فرمت گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DgvReports_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                node.CategoryName = Convert.ToString(row.Cells("colCategory").Value).Trim()
                If String.IsNullOrEmpty(node.CategoryName) Then
                    node.CategoryName = "بخش جدید"
                    row.Cells("colCategory").Value = "بخش جدید"
                End If
            End If
        End Sub

        Private Class PLNode
            Public CategoryID As Integer
            Public Key As String
            Public CategoryName As String
            Public AccountID As Integer
            Public AccountCode As String
            Public AccountName As String
            Public IsCategory As Boolean
            Public IsExpanded As Boolean = True
            Public Children As New List(Of PLNode)()
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
