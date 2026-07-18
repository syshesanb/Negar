Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class ShenavarCodingForm
        Inherits Form

        Private ReadOnly service As New ShenavarService()

        Private Declare Auto Function SendMessage Lib "user32" (hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
        Private Const EM_SETCUEBANNER As Integer = &H1501

        Private _currentParentId As Integer? = Nothing
        Private _currentParentName As String = String.Empty
        Private _currentDataTable As DataTable = Nothing
        Private _editAccountId As Integer? = Nothing
        Private _editParentId As Integer? = Nothing

        ' حالت انتخاب شناور: وقتی True کاربر باید یک حساب شناور برگ انتخاب کند
        Public Property SelectMode As Boolean = False
        Public Property SelectedShenavarID As Integer? = Nothing

        ' کَش حساب‌هایی که دارای فرزند هستند (برای رنگ‌بندی دکمه انتخاب)
        Private _accountsWithChildren As New System.Collections.Generic.HashSet(Of Integer)()

        Private Const ColBtnSelect As String = "colBtnSelect"
        Private Const ColBtnUp As String = "colBtnUp"
        Private Const ColBtnDown As String = "colBtnDown"
        Private Const ColBtnEdit As String = "colBtnEdit"
        Private Const ColBtnDelete As String = "colBtnDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ShenavarCodingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            Sys_Hes_Anb.Business.ThemeHelper.AppendStatusBar(Me)
            If Me.dgvAccounts IsNot Nothing Then Me.dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            SetupGrid()
            cmbSearchLevel.SelectedIndex = 0
            SendMessage(txtSearchCode.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی کد...")
            SendMessage(txtSearchName.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی نام...")
            LoadByParent(Nothing)
            If SelectMode Then
                dgvAccounts.Columns(ColBtnSelect).Visible = True
            End If
            ApplySecurity()
        End Sub

        Private Sub SetupGrid()
            dgvAccounts.Columns.Clear()

            Dim colSelect As New DataGridViewButtonColumn()
            colSelect.Name = ColBtnSelect
            colSelect.HeaderText = "انتخاب"
            colSelect.Text = "انتخاب"
            colSelect.UseColumnTextForButtonValue = True
            colSelect.Width = 64
            colSelect.FlatStyle = FlatStyle.Standard
            colSelect.Visible = False

            Dim colUp As New DataGridViewButtonColumn()
            colUp.Name = ColBtnUp
            colUp.HeaderText = "سطح قبل"
            colUp.Text = "سطح قبل"
            colUp.UseColumnTextForButtonValue = True
            colUp.Width = 72
            colUp.FlatStyle = FlatStyle.Standard

            Dim colDown As New DataGridViewButtonColumn()
            colDown.Name = ColBtnDown
            colDown.HeaderText = "سطح بعد"
            colDown.Text = "سطح بعد"
            colDown.UseColumnTextForButtonValue = True
            colDown.Width = 72
            colDown.FlatStyle = FlatStyle.Standard

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColBtnEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 64
            colEdit.FlatStyle = FlatStyle.Standard

            Dim colDel As New DataGridViewButtonColumn()
            colDel.Name = ColBtnDelete
            colDel.HeaderText = "حذف"
            colDel.Text = "حذف"
            colDel.UseColumnTextForButtonValue = True
            colDel.Width = 52
            colDel.FlatStyle = FlatStyle.Standard

            Dim colAccountId As New DataGridViewTextBoxColumn()
            colAccountId.Name = "colAccountID"
            colAccountId.DataPropertyName = "AccountID"
            colAccountId.Visible = False

            Dim colParentId As New DataGridViewTextBoxColumn()
            colParentId.Name = "colParentAccountID"
            colParentId.DataPropertyName = "ParentAccountID"
            colParentId.Visible = False

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colAccountCode"
            colCode.DataPropertyName = "AccountCode"
            colCode.HeaderText = "کد حساب"
            colCode.Width = 120

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colAccountName"
            colName.DataPropertyName = "AccountName"
            colName.HeaderText = "نام حساب"
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "colIsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 52
            colActive.ReadOnly = True

            dgvAccounts.Columns.AddRange(New DataGridViewColumn() {
                colSelect, colUp, colDown, colEdit, colDel,
                colAccountId, colParentId,
                colCode, colName, colActive})
        End Sub

        Public Sub RefreshData()
            LoadByParent(_currentParentId)
        End Sub

        Private Sub LoadByParent(parentId As Integer?)
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(20, "دریافت اطلاعات حساب‌های شناور...")

                _currentParentId = parentId
                _currentDataTable = service.GetByParent(parentId)

                progress.UpdateProgress(50, "تحلیل و کش درخت حساب‌های شناور...")

                ' در حالت انتخاب، کَش کن که کدام حساب‌ها فرزند دارند تا دکمه رنگ‌آمیزی شود
                _accountsWithChildren.Clear()
                If SelectMode AndAlso _currentDataTable IsNot Nothing Then
                    Dim parentsWithChildren = service.GetShenavarsWithChildren()
                    For Each dr As DataRow In _currentDataTable.Rows
                        Dim idVal = dr("AccountID")
                        If idVal IsNot Nothing AndAlso idVal IsNot DBNull.Value Then
                            Dim id = Convert.ToInt32(idVal)
                            If parentsWithChildren.Contains(id) Then _accountsWithChildren.Add(id)
                        End If
                    Next
                End If

                progress.UpdateProgress(80, "اعمال فیلترهای جستجو و نمایش...")
                ApplyFilter()
                UpdateLevelLabel()

                progress.UpdateProgress(100, "بارگذاری کامل شد")
            End Using
        End Sub

        Private Sub ApplyFilter()
            Dim codeF = txtSearchCode.Text.Trim()
            Dim nameF = txtSearchName.Text.Trim()
            Dim allLevels = (cmbSearchLevel.SelectedIndex = 1)

            If allLevels AndAlso (codeF.Length > 0 OrElse nameF.Length > 0) Then
                dgvAccounts.DataSource = service.SearchAll(codeF, nameF)
            Else
                If _currentDataTable Is Nothing Then Return
                Dim parts As New System.Collections.Generic.List(Of String)()
                If codeF.Length > 0 Then parts.Add("AccountCode LIKE '%" & codeF.Replace("'", "''") & "%'")
                If nameF.Length > 0 Then parts.Add("AccountName LIKE '%" & nameF.Replace("'", "''") & "%'")
                _currentDataTable.DefaultView.RowFilter = String.Join(" AND ", parts.ToArray())
                dgvAccounts.DataSource = _currentDataTable.DefaultView
            End If
        End Sub

        Private Sub TxtSearchCode_TextChanged(sender As Object, e As EventArgs) Handles txtSearchCode.TextChanged
            ApplyFilter()
        End Sub

        Private Sub TxtSearchName_TextChanged(sender As Object, e As EventArgs) Handles txtSearchName.TextChanged
            ApplyFilter()
        End Sub

        Private Sub CmbSearchLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearchLevel.SelectedIndexChanged
            ApplyFilter()
        End Sub

        Private Sub UpdateLevelLabel()
            If Not _currentParentId.HasValue Then
                lblCurrentLevel.Text = "سطح جاری: حسابهای شناور اصلی (سطح اول)"
            Else
                lblCurrentLevel.Text = "سطح جاری: زیرمجموعه‌های  « " & _currentParentName & " »   (برای بازگشت دکمه «سطح قبل» را کلیک کنید)"
            End If
        End Sub

        Private Sub DgvAccounts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim row = dgvAccounts.Rows(e.RowIndex)
            Dim accountIdVal = row.Cells("colAccountID").Value
            If accountIdVal Is Nothing OrElse accountIdVal Is DBNull.Value Then Return
            Dim accountId = Convert.ToInt32(accountIdVal)
            Dim accountName = Convert.ToString(row.Cells("colAccountName").Value)
            Dim colName = dgvAccounts.Columns(e.ColumnIndex).Name

            Select Case colName

                Case ColBtnUp
                    If Not _currentParentId.HasValue Then
                        MessageBox.Show("شما در بالاترین سطح این حساب شناور هستید.", "توجه",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        Dim grandParentId = service.GetParentId(_currentParentId.Value)
                        _currentParentName = If(grandParentId.HasValue, service.GetName(grandParentId.Value), String.Empty)
                        LoadByParent(grandParentId)
                        HideDataPanel()
                    End If

                Case ColBtnDown
                    If Not service.HasChildren(accountId) Then
                        Dim ans = MessageBox.Show(
                            "شما در آخرین سطح این سرفصل هستید." & Environment.NewLine &
                            "آیا می‌خواهید برای آن زیرسطح ایجاد کنید؟",
                            "توجه", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If ans = DialogResult.Yes Then
                            _currentParentName = accountName
                            LoadByParent(accountId)
                            ShowDataPanel(Nothing, accountId)
                        End If
                    Else
                        _currentParentName = accountName
                        LoadByParent(accountId)
                        HideDataPanel()
                    End If

                Case ColBtnEdit
                    Dim parentVal = row.Cells("colParentAccountID").Value
                    Dim parentId As Integer? = Nothing
                    If parentVal IsNot Nothing AndAlso parentVal IsNot DBNull.Value Then
                        parentId = Convert.ToInt32(parentVal)
                    End If
                    ShowDataPanel(accountId, parentId)

                Case ColBtnSelect
                    ' انتخاب شناور: اگر فرزند داشت → پیام + ناوبری؛ اگر برگ بود → بازگشت ShenavarID
                    If _accountsWithChildren.Contains(accountId) Then
                        MessageBox.Show("این سرفصل حساب ، دارای زیر سطح می باشد",
                                        "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _currentParentName = accountName
                        LoadByParent(accountId)
                        HideDataPanel()
                    Else
                        SelectedShenavarID = accountId
                        dgvAccounts.Columns(ColBtnSelect).Visible = False
                        Me.Close()
                    End If

                Case ColBtnDelete
                    If service.HasChildren(accountId) Then
                        MessageBox.Show("این حساب دارای زیرمجموعه است و قابل حذف نیست." & Environment.NewLine &
                                        "ابتدا زیرمجموعه‌های آن را حذف کنید.",
                                        "امکان حذف وجود ندارد", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    If MessageBox.Show("حساب «" & accountName & "» حذف شود؟",
                                       "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            service.Delete(accountId)
                            LoadByParent(_currentParentId)
                            HideDataPanel()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف: " & ex.Message, "خطا",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If

            End Select
        End Sub

        Private Sub DgvAccounts_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvAccounts.CellPainting
            If Not SelectMode Then Return
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvAccounts.Columns(e.ColumnIndex).Name <> ColBtnSelect Then Return

            Dim accountIdVal = dgvAccounts.Rows(e.RowIndex).Cells("colAccountID").Value
            If accountIdVal Is Nothing OrElse accountIdVal Is DBNull.Value Then Return
            Dim accountId = Convert.ToInt32(accountIdVal)

            Dim hasChildren = _accountsWithChildren.Contains(accountId)
            Dim btnColor = If(hasChildren, Color.Red, Color.Green)

            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border)

            Dim fillRect = Rectangle.Inflate(e.CellBounds, -2, -2)
            Using brush As New SolidBrush(btnColor)
                e.Graphics.FillRectangle(brush, fillRect)
            End Using
            Using pen As New Pen(Color.FromArgb(80, 0, 0, 0))
                e.Graphics.DrawRectangle(pen, fillRect)
            End Using

            TextRenderer.DrawText(e.Graphics, "انتخاب", dgvAccounts.Font, e.CellBounds,
                                  Color.White,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End Sub

        Private Sub ShowDataPanel(accountId As Integer?, parentId As Integer?)
            _editAccountId = accountId
            _editParentId = parentId

            If accountId.HasValue Then
                For Each row As DataGridViewRow In dgvAccounts.Rows
                    Dim rowId = row.Cells("colAccountID").Value
                    If rowId Is Nothing OrElse rowId Is DBNull.Value Then Continue For
                    If Convert.ToInt32(rowId) = accountId.Value Then
                        txtAccountCode.Text = Convert.ToString(row.Cells("colAccountCode").Value)
                        txtAccountName.Text = Convert.ToString(row.Cells("colAccountName").Value)
                        Dim activeVal = row.Cells("colIsActive").Value
                        chkActive.Checked = If(activeVal Is Nothing OrElse activeVal Is DBNull.Value,
                                               True, Convert.ToBoolean(activeVal))
                        Exit For
                    End If
                Next
            Else
                txtAccountCode.Text = service.GetNextSuggestedCode(parentId)
                txtAccountName.Clear()
                chkActive.Checked = True
            End If

            pnlData.Visible = True
            txtAccountCode.Focus()
            txtAccountCode.SelectAll()
        End Sub

        Private Sub HideDataPanel()
            pnlData.Visible = False
            _editAccountId = Nothing
            _editParentId = Nothing
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            ShowDataPanel(Nothing, _currentParentId)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtAccountCode.Text) Then
                MessageBox.Show("کد حساب نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountCode.Focus()
                Return
            End If
            If String.IsNullOrWhiteSpace(txtAccountName.Text) Then
                MessageBox.Show("نام حساب نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountName.Focus()
                Return
            End If

            Try
                service.Save(
                    _editAccountId,
                    txtAccountCode.Text.Trim(),
                    txtAccountName.Text.Trim(),
                    _editParentId,
                    chkActive.Checked)
                HideDataPanel()
                LoadByParent(_currentParentId)
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "کد حساب تکراری",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAccountCode.Focus()
                txtAccountCode.SelectAll()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            HideDataPanel()
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim userType = SessionContext.CurrentUser.UserType
            Dim isSuperAdmin = String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim hasGlobalAccounting = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAccounting)

            Dim canCreate = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanCreate)
            Dim canEdit = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanEdit)
            Dim canDelete = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanDelete)

            btnNew.Visible = canCreate
            btnSave.Visible = canCreate OrElse canEdit

            If dgvAccounts.Columns.Contains(ColBtnEdit) Then
                dgvAccounts.Columns(ColBtnEdit).Visible = canEdit
            End If
            If dgvAccounts.Columns.Contains(ColBtnDelete) Then
                dgvAccounts.Columns(ColBtnDelete).Visible = canDelete
            End If
        End Sub

    End Class
End Namespace
