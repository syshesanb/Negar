Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Partial Class UserManagementPermissionsForm
        Inherits Form

        Private ReadOnly service As New UserService()
        Private ReadOnly _ordinaryOnly As Boolean
        Private _selectedUserId As Integer?

        Public Sub New()
            Me.New(False)
        End Sub

        Public Sub New(ordinaryOnly As Boolean)
            _ordinaryOnly = ordinaryOnly
            InitializeComponent()
        End Sub

        Private Sub UserManagementPermissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvUsers IsNot Nothing Then Me.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvPermissions IsNot Nothing Then Me.dgvPermissions.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            LoadUsers()
            LoadPermissionsForSelection()
            AdjustLayoutSplitter()
        End Sub

        Private Sub UserManagementPermissionsForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown, MyBase.VisibleChanged, MyBase.Resize
            AdjustLayoutSplitter()
        End Sub

        Private Sub AdjustLayoutSplitter()
            Try
                If Me.Width > 300 Then
                    Dim targetDist As Integer = CInt(Me.Width * 0.44)
                    If targetDist < 550 Then targetDist = 620
                    If targetDist > 700 Then targetDist = 650
                    splitMain.SplitterDistance = targetDist
                Else
                    splitMain.SplitterDistance = 630
                End If
            Catch
            End Try
        End Sub

        Private Sub LoadUsers()
            If _ordinaryOnly Then
                dgvUsers.DataSource = service.GetUsersByTypes("User")
            Else
                dgvUsers.DataSource = service.GetUsers()
            End If

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If dgvUsers.Columns.Contains("UserID") Then
                dgvUsers.Columns("UserID").HeaderText = "کد"
                dgvUsers.Columns("UserID").Width = 50
            End If
            If dgvUsers.Columns.Contains("Username") Then
                dgvUsers.Columns("Username").HeaderText = "نام کاربری"
                dgvUsers.Columns("Username").Width = 90
            End If
            If dgvUsers.Columns.Contains("UserType") Then
                dgvUsers.Columns("UserType").HeaderText = "نوع کاربر"
                dgvUsers.Columns("UserType").Width = 85
            End If
            If dgvUsers.Columns.Contains("FullName") Then
                dgvUsers.Columns("FullName").HeaderText = "نام و نام خانوادگی"
                dgvUsers.Columns("FullName").Width = 140
            End If
            If dgvUsers.Columns.Contains("CreatedDate") Then
                dgvUsers.Columns("CreatedDate").HeaderText = "تاریخ ایجاد"
                dgvUsers.Columns("CreatedDate").Width = 95
            End If
            If dgvUsers.Columns.Contains("IsActive") Then
                dgvUsers.Columns("IsActive").HeaderText = "فعال"
                dgvUsers.Columns("IsActive").Width = 50
            End If
            If dgvUsers.Columns.Contains("MaxCompaniesAllowed") Then
                dgvUsers.Columns("MaxCompaniesAllowed").HeaderText = "سقف شرکت"
                dgvUsers.Columns("MaxCompaniesAllowed").Width = 80
            End If
            If dgvUsers.Columns.Contains("MaxFiscalYearsPerCompany") Then
                dgvUsers.Columns("MaxFiscalYearsPerCompany").HeaderText = "سقف سال مالی"
                dgvUsers.Columns("MaxFiscalYearsPerCompany").Width = 85
            End If

            dgvUsers.ClearSelection()
        End Sub

        Private Sub DgvUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUsers.CellFormatting
            If dgvUsers.Columns(e.ColumnIndex).Name = "CreatedDate" Then
                If e.Value IsNot Nothing AndAlso Not Convert.IsDBNull(e.Value) AndAlso TypeOf e.Value Is DateTime Then
                    e.Value = PersianDateHelper.ToPersian(CType(e.Value, DateTime))
                    e.FormattingApplied = True
                End If
            End If
        End Sub

        Private Function GetAllowedPermissionKeys() As List(Of String)
            If SessionContext.CurrentUser Is Nothing Then
                Return New List(Of String)()
            End If

            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Nothing
            End If

            Return New List(Of String)(SessionContext.CurrentPermissions)
        End Function

        Private Function GetSelectedUserId() As Integer?
            If dgvUsers.CurrentRow Is Nothing OrElse dgvUsers.CurrentRow.IsNewRow Then
                Return Nothing
            End If

            Dim value = dgvUsers.CurrentRow.Cells("UserID").Value
            If value Is Nothing OrElse value Is DBNull.Value Then
                Return Nothing
            End If

            Return Convert.ToInt32(value)
        End Function

        Private Sub LoadPermissionsForSelection()
            Dim userId = GetSelectedUserId()
            If userId.HasValue Then
                _selectedUserId = userId.Value
                LoadPermissions(userId.Value)
            Else
                dgvPermissions.DataSource = Nothing
            End If
        End Sub

        Private Sub LoadPermissions(userId As Integer)
            dgvPermissions.DataSource = service.GetPermissionMatrix(userId, GetAllowedPermissionKeys())
            ApplyPermissionsGridText()
        End Sub

        Private Sub ApplyPermissionsGridText()
            dgvPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If dgvPermissions.Columns.Contains("PermissionID") Then dgvPermissions.Columns("PermissionID").Visible = False
            If dgvPermissions.Columns.Contains("PermissionKey") Then dgvPermissions.Columns("PermissionKey").Visible = False

            If dgvPermissions.Columns.Contains("SectionName") Then
                dgvPermissions.Columns("SectionName").HeaderText = "بخش"
                dgvPermissions.Columns("SectionName").Width = 110
                dgvPermissions.Columns("SectionName").ReadOnly = False
                dgvPermissions.Columns("SectionName").DisplayIndex = 0
            End If

            If dgvPermissions.Columns.Contains("PermissionName") Then
                dgvPermissions.Columns("PermissionName").HeaderText = "مجوز"
                dgvPermissions.Columns("PermissionName").Width = 280
                dgvPermissions.Columns("PermissionName").DisplayIndex = 1
                For Each gridRow As DataGridViewRow In dgvPermissions.Rows
                    If Not gridRow.IsNewRow Then
                        Dim permissionKey = Convert.ToString(gridRow.Cells("PermissionKey").Value)
                        Dim fallbackName = Convert.ToString(gridRow.Cells("PermissionName").Value)
                        gridRow.Cells("PermissionName").Value = TranslatePermissionName(permissionKey, fallbackName)
                    End If
                Next
            End If

            Dim checkCols = {"CanView", "CanCreate", "CanEdit", "CanDelete", "CanPrint", "CanExport"}
            Dim titles = {"مشاهده", "ایجاد", "ویرایش", "حذف", "چاپ", "خروجی"}

            For i As Integer = 0 To checkCols.Length - 1
                Dim colName = checkCols(i)
                If dgvPermissions.Columns.Contains(colName) Then
                    dgvPermissions.Columns(colName).HeaderText = titles(i)
                    dgvPermissions.Columns(colName).Width = 55
                    dgvPermissions.Columns(colName).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    dgvPermissions.Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
            Next

            DisableInactiveCells()
        End Sub

        Private Function TranslatePermissionName(permissionKey As String, fallbackName As String) As String
            Select Case permissionKey
                Case PermissionKeys.ManageBasicUsers
                    Return "مدیریت کاربران عادی"
                Case PermissionKeys.ManageUsers
                    Return "مدیریت کاربران (جامع)"
                Case PermissionKeys.ManageCompanies
                    Return "مدیریت شرکت‌ها"
                Case PermissionKeys.ManageFiscalYears
                    Return "مدیریت سال‌های مالی"
                Case PermissionKeys.ManageCompaniesYears
                    Return "مدیریت شرکت‌ها و سال‌های مالی ( جامع )"
                Case PermissionKeys.SelectCompanyFiscalYear
                    Return "انتخاب شرکت و سال مالی جاری"
                Case PermissionKeys.ManageAppThemes
                    Return "مدیریت تمهای برنامه و فرمها"
                Case PermissionKeys.BackupData
                    Return "پشتیبان‌گیری اطلاعات"
                Case PermissionKeys.RestoreData
                    Return "بازیابی اطلاعات"
                Case PermissionKeys.ManageBusinessShells
                    Return "پوسته مشاغل"
                Case PermissionKeys.ManageUtilities
                    Return "امکانات"
                Case PermissionKeys.ViewActivityLog
                    Return "مشاهده دفتر سوابق و گزارش فعالیت‌ها"
                Case PermissionKeys.LockSanad1
                    Return "قطعی‌سازی و قفل اسناد حسابداری"
                Case PermissionKeys.HideSFSHInSanad
                    Return "مخفی کردن ستونهای SF و SH در فرم سند حسابداری"

                ' Accounting
                Case PermissionKeys.AccountingHeader
                    Return "حسابداری – سرفصل حسابها"
                Case PermissionKeys.AccountingShenavar
                    Return "حسابداری – حسابهای شناور"
                Case PermissionKeys.AccountingEntry
                    Return "حسابداری – ثبت سند حسابداری"
                Case PermissionKeys.AccountingBank
                    Return "حسابداری – مغایرات بانکی"
                Case PermissionKeys.AccountingBalance
                    Return "حسابداری – تراز آزمایشی"
                Case PermissionKeys.AccountingLedger
                    Return "حسابداری – دفتر حساب"
                Case PermissionKeys.AccountingReports
                    Return "حسابداری – گزارشات حسابداری"
                Case PermissionKeys.ManageAccounting
                    Return "حسابداری (جامع)"

                ' Trade & Warehousing
                Case PermissionKeys.TradeProducts
                    Return "خرید و فروش – تعریف کالاها و خدمات"
                Case PermissionKeys.TradeWarehouses
                    Return "خرید و فروش – تعریف انبارها"
                Case PermissionKeys.TradePurchase
                    Return "خرید و فروش – صدور فاکتور خرید"
                Case PermissionKeys.TradeSales
                    Return "خرید و فروش – صدور فاکتور فروش"
                Case PermissionKeys.TradeRemittance
                    Return "خرید و فروش – حواله و رسید انبار"
                Case PermissionKeys.TradeReports
                    Return "خرید و فروش – گزارشات انبار و کاردکس کالا"
                Case PermissionKeys.ManageProducts
                    Return "مدیریت کالاها"
                Case PermissionKeys.ManageWarehouses
                    Return "مدیریت انبارها"
                Case PermissionKeys.ManagePurchases
                    Return "مدیریت خرید"
                Case PermissionKeys.ManageSales
                    Return "مدیریت فروش"
                Case PermissionKeys.ViewInventory
                    Return "مشاهده موجودی انبار"
                Case PermissionKeys.ManageTradeWarehouse
                    Return "خرید و فروش و انبارداری ( جامع )"
                Case PermissionKeys.ViewReports
                    Return "مشاهده گزارش‌ها"
                Case Else
                    Return fallbackName
            End Select
        End Function

        Private Sub DgvUsers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUsers.SelectionChanged
            LoadPermissionsForSelection()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadUsers()
            LoadPermissionsForSelection()
        End Sub

        Private Sub BtnSavePermissions_Click(sender As Object, e As EventArgs) Handles btnSavePermissions.Click
            If Not _selectedUserId.HasValue Then
                MessageBox.Show("ابتدا یک کاربر را انتخاب کنید.")
                Return
            End If

            Dim table = TryCast(dgvPermissions.DataSource, DataTable)
            If table Is Nothing Then
                MessageBox.Show("مجوزی برای ذخیره وجود ندارد.")
                Return
            End If

            Dim allowedKeys = GetAllowedPermissionKeys()

            For Each row As DataRow In table.Rows
                Dim permissionKey = Convert.ToString(row("PermissionKey"))

                ' Save custom section name to database
                Dim sectionName = Convert.ToString(row("SectionName"))
                service.UpdatePermissionSection(Convert.ToInt32(row("PermissionID")), sectionName)

                If allowedKeys IsNot Nothing AndAlso Not service.CanAssignPermission(permissionKey, allowedKeys) Then
                    Continue For
                End If

                service.SetUserPermission(
                    _selectedUserId.Value,
                    Convert.ToInt32(row("PermissionID")),
                    If(row.IsNull("CanView"), False, Convert.ToBoolean(row("CanView"))),
                    If(row.IsNull("CanCreate"), False, Convert.ToBoolean(row("CanCreate"))),
                    If(row.IsNull("CanEdit"), False, Convert.ToBoolean(row("CanEdit"))),
                    If(row.IsNull("CanDelete"), False, Convert.ToBoolean(row("CanDelete"))),
                    If(row.IsNull("CanPrint"), False, Convert.ToBoolean(row("CanPrint"))),
                    If(row.IsNull("CanExport"), False, Convert.ToBoolean(row("CanExport"))),
                    allowedKeys)
            Next

            MessageBox.Show("سطح دسترسی‌ها ذخیره شد.")
        End Sub

        Private Function IsColumnActiveForPermission(permissionKey As String, columnName As String) As Boolean
            If String.Equals(columnName, "CanView", StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If

            If String.Equals(permissionKey, PermissionKeys.ManageCompanies, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.ManageFiscalYears, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingHeader, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingShenavar, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingEntry, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingBank, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingBalance, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingLedger, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.AccountingReports, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(permissionKey, PermissionKeys.LockSanad1, StringComparison.OrdinalIgnoreCase) Then
                Dim activeCols = {"CanCreate", "CanEdit", "CanDelete"}
                Return Array.IndexOf(activeCols, columnName) >= 0
            End If

            Return False
        End Function

        Private Function IsCompPermissionChecked(compKey As String) As Boolean
            For Each row As DataGridViewRow In dgvPermissions.Rows
                If row.IsNewRow Then Continue For
                Dim key = Convert.ToString(row.Cells("PermissionKey").Value)
                If String.Equals(key, compKey, StringComparison.OrdinalIgnoreCase) Then
                    Return Convert.ToBoolean(row.Cells("CanView").Value)
                End If
            Next
            Return False
        End Function

        Private Sub DisableInactiveCells()
            If dgvPermissions.Rows.Count = 0 Then Return
            Dim isCompChecked = IsCompPermissionChecked(PermissionKeys.ManageCompaniesYears)
            Dim isAccChecked = IsCompPermissionChecked(PermissionKeys.ManageAccounting)

            Dim companyTargetKeys = {PermissionKeys.ManageCompanies, PermissionKeys.ManageFiscalYears, PermissionKeys.SelectCompanyFiscalYear}
            Dim accTargetKeys = {
                PermissionKeys.AccountingHeader,
                PermissionKeys.AccountingShenavar,
                PermissionKeys.AccountingEntry,
                PermissionKeys.AccountingBank,
                PermissionKeys.AccountingBalance,
                PermissionKeys.AccountingLedger,
                PermissionKeys.AccountingReports,
                PermissionKeys.LockSanad1
            }

            Dim checkCols = {"CanView", "CanCreate", "CanEdit", "CanDelete", "CanPrint", "CanExport"}
            
            For Each gridRow As DataGridViewRow In dgvPermissions.Rows
                If gridRow.IsNewRow Then Continue For
                Dim permissionKey = Convert.ToString(gridRow.Cells("PermissionKey").Value)
                For Each colName In checkCols
                    If dgvPermissions.Columns.Contains(colName) Then
                        Dim cell = gridRow.Cells(colName)
                        Dim isActive = IsColumnActiveForPermission(permissionKey, colName)
                        
                        If isActive Then
                            If isCompChecked AndAlso Array.IndexOf(companyTargetKeys, permissionKey) >= 0 Then
                                isActive = False
                            ElseIf isAccChecked AndAlso Array.IndexOf(accTargetKeys, permissionKey) >= 0 Then
                                isActive = False
                            End If
                        End If

                        If Not isActive Then
                            cell.Value = False
                            cell.ReadOnly = True
                        End If
                    End If
                Next
            Next
        End Sub

        Private Sub UpdateGranularPermissionsState(compKey As String, targetKeys As String())
            Dim isCompChecked = IsCompPermissionChecked(compKey)
            Dim checkCols = {"CanView", "CanCreate", "CanEdit", "CanDelete", "CanPrint", "CanExport"}

            For Each row As DataGridViewRow In dgvPermissions.Rows
                If row.IsNewRow Then Continue For
                Dim permissionKey = Convert.ToString(row.Cells("PermissionKey").Value)
                If Array.IndexOf(targetKeys, permissionKey) >= 0 Then
                    For Each colName In checkCols
                        If dgvPermissions.Columns.Contains(colName) Then
                            Dim cell = row.Cells(colName)
                            If isCompChecked Then
                                cell.Value = False
                                cell.ReadOnly = True
                            Else
                                Dim isActive = IsColumnActiveForPermission(permissionKey, colName)
                                cell.ReadOnly = Not isActive
                            End If
                        End If
                    Next
                End If
            Next
            dgvPermissions.Invalidate()
        End Sub

        Private Sub DgvPermissions_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvPermissions.CurrentCellDirtyStateChanged
            If TypeOf dgvPermissions.CurrentCell Is DataGridViewCheckBoxCell Then
                dgvPermissions.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End Sub

        Private Sub DgvPermissions_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPermissions.CellValueChanged
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                Dim colName = dgvPermissions.Columns(e.ColumnIndex).Name
                If String.Equals(colName, "CanView", StringComparison.OrdinalIgnoreCase) Then
                    Dim gridRow = dgvPermissions.Rows(e.RowIndex)
                    Dim permissionKey = Convert.ToString(gridRow.Cells("PermissionKey").Value)
                    
                    If String.Equals(permissionKey, PermissionKeys.ManageCompaniesYears, StringComparison.OrdinalIgnoreCase) Then
                        Dim companyTargetKeys = {PermissionKeys.ManageCompanies, PermissionKeys.ManageFiscalYears, PermissionKeys.SelectCompanyFiscalYear}
                        UpdateGranularPermissionsState(PermissionKeys.ManageCompaniesYears, companyTargetKeys)
                    ElseIf String.Equals(permissionKey, PermissionKeys.ManageAccounting, StringComparison.OrdinalIgnoreCase) Then
                        Dim accTargetKeys = {
                            PermissionKeys.AccountingHeader,
                            PermissionKeys.AccountingShenavar,
                            PermissionKeys.AccountingEntry,
                            PermissionKeys.AccountingBank,
                            PermissionKeys.AccountingBalance,
                            PermissionKeys.AccountingLedger,
                            PermissionKeys.AccountingReports,
                            PermissionKeys.LockSanad1
                        }
                        UpdateGranularPermissionsState(PermissionKeys.ManageAccounting, accTargetKeys)
                    End If
                End If
            End If
        End Sub

        Private Sub DgvPermissions_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvPermissions.CellPainting
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                Dim colName = dgvPermissions.Columns(e.ColumnIndex).Name
                Dim checkCols = {"CanView", "CanCreate", "CanEdit", "CanDelete", "CanPrint", "CanExport"}
                If Array.IndexOf(checkCols, colName) >= 0 Then
                    Dim gridRow = dgvPermissions.Rows(e.RowIndex)
                    Dim permissionKey = Convert.ToString(gridRow.Cells("PermissionKey").Value)
                    
                    Dim isActive = IsColumnActiveForPermission(permissionKey, colName)
                    If isActive Then
                        Dim companyTargetKeys = {PermissionKeys.ManageCompanies, PermissionKeys.ManageFiscalYears, PermissionKeys.SelectCompanyFiscalYear}
                        Dim accTargetKeys = {
                            PermissionKeys.AccountingHeader,
                            PermissionKeys.AccountingShenavar,
                            PermissionKeys.AccountingEntry,
                            PermissionKeys.AccountingBank,
                            PermissionKeys.AccountingBalance,
                            PermissionKeys.AccountingLedger,
                            PermissionKeys.AccountingReports,
                            PermissionKeys.LockSanad1
                        }
                        If Array.IndexOf(companyTargetKeys, permissionKey) >= 0 AndAlso IsCompPermissionChecked(PermissionKeys.ManageCompaniesYears) Then
                            isActive = False
                        ElseIf Array.IndexOf(accTargetKeys, permissionKey) >= 0 AndAlso IsCompPermissionChecked(PermissionKeys.ManageAccounting) Then
                            isActive = False
                        End If
                    End If

                    If Not isActive Then
                        e.PaintBackground(e.CellBounds, True)
                        Using brush As New System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(240, 240, 240))
                            e.Graphics.FillRectangle(brush, e.CellBounds)
                        End Using
                        Using pen As New System.Drawing.Pen(dgvPermissions.GridColor)
                            e.Graphics.DrawRectangle(pen, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1)
                        End Using
                        e.Handled = True
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
