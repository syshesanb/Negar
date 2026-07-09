Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

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

        Private _accountsForm As HesabdaryCodingForm
        Private _shenavarForm As ShenavarCodingForm
        Private _sanad1Form As HesabdarySanad1Form
        Private _trialForm As HesabdaryTarazForm
        Private _ledgerForm As HesabdaryDaftarForm
        Private _tarazShenavarForm As HesabdaryTarazShenavarForm
        Private _daftarShenavarForm As HesabdaryDaftarShenavarForm

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
            End If
        End Sub

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
