Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Partial Class ShenavarCodingForm
        Inherits Form

        Public Class ShenavarNode
            Public AccountID As Integer
            Public AccountCode As String
            Public AccountName As String
            Public IsActive As Boolean
            Public ParentAccountID As Integer?
            Public Level As Integer = 0
            Public IsExpanded As Boolean = True
            Public Children As New List(Of ShenavarNode)()

            Public ReadOnly Property HasChildren As Boolean
                Get
                    Return Children.Count > 0
                End Get
            End Property
        End Class

        Private ReadOnly service As New ShenavarService()

        Private Declare Auto Function SendMessage Lib "user32" (hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
        Private Const EM_SETCUEBANNER As Integer = &H1501

        Private _nodeDict As New Dictionary(Of Integer, ShenavarNode)()
        Private _rootNodes As New List(Of ShenavarNode)()

        Private _currentParentId As Integer? = Nothing
        Private _currentParentName As String = String.Empty
        Private _editAccountId As Integer? = Nothing
        Private _editParentId As Integer? = Nothing

        ' حالت انتخاب شناور: وقتی True کاربر باید یک حساب شناور برگ انتخاب کند
        Public Property SelectMode As Boolean = False
        Public Property SelectedShenavarID As Integer? = Nothing

        ' کَش حساب‌هایی که دارای فرزند هستند
        Private _accountsWithChildren As New HashSet(Of Integer)()

        Private Const ColBtnSelect As String = "colBtnSelect"
        Private Const ColBtnEdit As String = "colBtnEdit"
        Private Const ColBtnDelete As String = "colBtnDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ShenavarCodingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            If Me.dgvAccounts IsNot Nothing Then Me.dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            SetupGrid()
            cmbSearchLevel.SelectedIndex = 0
            SendMessage(txtSearchCode.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی کد...")
            SendMessage(txtSearchName.Handle, EM_SETCUEBANNER, New IntPtr(1), "جستجوی نام...")
            LoadTreeData()
            If SelectMode Then
                dgvAccounts.Columns(ColBtnSelect).Visible = True
            End If
            ApplySecurity()
        End Sub

        Private Sub SetupGrid()
            dgvAccounts.Columns.Clear()

            ' Toggle button (+ / -)
            Dim colToggle As New DataGridViewTextBoxColumn()
            colToggle.Name = "colToggle"
            colToggle.HeaderText = ""
            colToggle.Width = 35
            colToggle.ReadOnly = True
            colToggle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colSelect As New DataGridViewButtonColumn()
            colSelect.Name = ColBtnSelect
            colSelect.HeaderText = "انتخاب"
            colSelect.Text = "انتخاب"
            colSelect.UseColumnTextForButtonValue = True
            colSelect.Width = 64
            colSelect.FlatStyle = FlatStyle.Standard
            colSelect.Visible = False

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
                colToggle, colSelect, colEdit, colDel,
                colAccountId, colParentId,
                colCode, colName, colActive})
        End Sub

        Public Sub RefreshData()
            LoadTreeData()
        End Sub

        Private Sub LoadTreeData()
            Dim dt As DataTable
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(20, "دریافت اطلاعات حساب‌های شناور از پایگاه داده...")

                Try
                    dt = service.GetAllAccounts()
                Catch ex As Exception
                    MessageBox.Show("خطا در بارگذاری داده‌ها: " & ex.Message, "خطا",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                progress.UpdateProgress(60, "تحلیل ساختار درختی حساب‌های شناور...")

                _nodeDict.Clear()
                _rootNodes.Clear()
                _accountsWithChildren.Clear()

                For Each row As DataRow In dt.Rows
                    Dim node As New ShenavarNode()
                    node.AccountID = Convert.ToInt32(row("AccountID"))
                    node.AccountCode = Convert.ToString(row("AccountCode"))
                    node.AccountName = Convert.ToString(row("AccountName"))
                    node.IsActive = If(row.IsNull("IsActive"), True, Convert.ToBoolean(row("IsActive")))
                    node.ParentAccountID = If(row.IsNull("ParentAccountID"),
                                              CType(Nothing, Integer?),
                                              CType(Convert.ToInt32(row("ParentAccountID")), Integer?))
                    node.IsExpanded = True
                    _nodeDict(node.AccountID) = node
                Next

                For Each node In _nodeDict.Values
                    If node.ParentAccountID.HasValue AndAlso _nodeDict.ContainsKey(node.ParentAccountID.Value) Then
                        _nodeDict(node.ParentAccountID.Value).Children.Add(node)
                        _accountsWithChildren.Add(node.ParentAccountID.Value)
                    Else
                        _rootNodes.Add(node)
                    End If
                Next

                SetLevels(_rootNodes, 0)
                progress.UpdateProgress(100, "بارگذاری درختی کامل شد")
            End Using

            RefreshGrid()
        End Sub

        Private Sub SetLevels(nodes As List(Of ShenavarNode), level As Integer)
            If level > 20 Then Return
            For Each node In nodes
                node.Level = level
                SetLevels(node.Children, level + 1)
            Next
        End Sub

        Private Sub RefreshGrid()
            Try
                If dgvAccounts Is Nothing OrElse dgvAccounts.Columns.Count = 0 Then Return
                dgvAccounts.SuspendLayout()
                dgvAccounts.Rows.Clear()

                Dim displayList As New List(Of ShenavarNode)()
                BuildDisplayList(_rootNodes, displayList)

                For Each node In displayList
                    Dim rowIdx = dgvAccounts.Rows.Add()
                    Dim row = dgvAccounts.Rows(rowIdx)
                    row.Tag = node

                    row.Cells("colToggle").Value = GetToggleText(node)
                    row.Cells("colAccountID").Value = node.AccountID
                    row.Cells("colParentAccountID").Value = node.ParentAccountID
                    row.Cells("colAccountCode").Value = node.AccountCode

                    Dim indentSpaces As String = New String(Convert.ToChar(160), node.Level * 6)
                    row.Cells("colAccountName").Value = indentSpaces & node.AccountName
                    row.Cells("colIsActive").Value = node.IsActive

                    ApplyRowStyle(row, node)
                Next

                dgvAccounts.ResumeLayout()
                UpdateLevelLabel()
            Catch ex As Exception
                MessageBox.Show("خطا در به‌روزرسانی جدول: " & ex.Message, "خطای جدول", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BuildDisplayList(nodes As List(Of ShenavarNode), result As List(Of ShenavarNode))
            Dim codeF = txtSearchCode.Text.Trim()
            Dim nameF = txtSearchName.Text.Trim()
            Dim hasFilter = (codeF.Length > 0 OrElse nameF.Length > 0)

            For Each node In nodes
                Dim matchesSearch = True
                If hasFilter Then
                    If codeF.Length > 0 AndAlso Not node.AccountCode.Contains(codeF) Then matchesSearch = False
                    If nameF.Length > 0 AndAlso Not node.AccountName.Contains(nameF) Then matchesSearch = False
                End If

                Dim showNode = True
                If hasFilter Then
                    showNode = matchesSearch OrElse HasAnyMatchingChild(node, codeF, nameF)
                End If

                If showNode Then
                    result.Add(node)
                    If hasFilter Then
                        node.IsExpanded = True
                    End If

                    If node.IsExpanded AndAlso node.HasChildren Then
                        BuildDisplayList(node.Children, result)
                    End If
                End If
            Next
        End Sub

        Private Function HasAnyMatchingChild(node As ShenavarNode, codeF As String, nameF As String) As Boolean
            For Each child In node.Children
                Dim matches = True
                If codeF.Length > 0 AndAlso Not child.AccountCode.Contains(codeF) Then matches = False
                If nameF.Length > 0 AndAlso Not child.AccountName.Contains(nameF) Then matches = False
                If matches Then Return True
                If HasAnyMatchingChild(child, codeF, nameF) Then Return True
            Next
            Return False
        End Function

        Private Function GetToggleText(node As ShenavarNode) As String
            If Not node.HasChildren Then Return ""
            Return If(node.IsExpanded, "−", "+")
        End Function

        Private Sub ApplyRowStyle(row As DataGridViewRow, node As ShenavarNode)
            Select Case node.Level
                Case 0
                    row.DefaultCellStyle.Font = New Font(dgvAccounts.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(195, 218, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(15, 30, 80)
                Case 1
                    row.DefaultCellStyle.Font = New Font(dgvAccounts.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(215, 232, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 100)
                Case 2
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 244, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 60, 120)
                Case Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
            End Select
        End Sub

        Private Sub TxtSearchCode_TextChanged(sender As Object, e As EventArgs) Handles txtSearchCode.TextChanged
            RefreshGrid()
        End Sub

        Private Sub TxtSearchName_TextChanged(sender As Object, e As EventArgs) Handles txtSearchName.TextChanged
            RefreshGrid()
        End Sub

        Private Sub CmbSearchLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearchLevel.SelectedIndexChanged
            RefreshGrid()
        End Sub

        Private Sub UpdateLevelLabel()
            lblCurrentLevel.Text = "نمایش درختی و ساختار سلسله‌مراتبی حساب‌های شناور"
        End Sub

        Private Sub DgvAccounts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvAccounts.Columns(e.ColumnIndex).Name = "colToggle" Then
                ToggleNode(e.RowIndex)
            End If
        End Sub

        Private Sub ToggleNode(rowIndex As Integer)
            Dim node = TryCast(dgvAccounts.Rows(rowIndex).Tag, ShenavarNode)
            If node Is Nothing OrElse Not node.HasChildren Then Return
            node.IsExpanded = Not node.IsExpanded
            RefreshGrid()
        End Sub

        Private Sub DgvAccounts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim row = dgvAccounts.Rows(e.RowIndex)
            Dim accountIdVal = row.Cells("colAccountID").Value
            If accountIdVal Is Nothing OrElse accountIdVal Is DBNull.Value Then Return
            Dim accountId = Convert.ToInt32(accountIdVal)
            Dim accountName = Convert.ToString(row.Cells("colAccountName").Value).Trim()
            Dim colName = dgvAccounts.Columns(e.ColumnIndex).Name

            Select Case colName
                Case ColBtnEdit
                    Dim parentVal = row.Cells("colParentAccountID").Value
                    Dim parentId As Integer? = Nothing
                    If parentVal IsNot Nothing AndAlso parentVal IsNot DBNull.Value Then
                        parentId = Convert.ToInt32(parentVal)
                    End If
                    ShowDataPanel(accountId, parentId)

                Case ColBtnSelect
                    Dim selectedNode = TryCast(row.Tag, ShenavarNode)
                    If selectedNode IsNot Nothing AndAlso selectedNode.HasChildren Then
                        MessageBox.Show("این حساب شناور دارای زیر سطح می‌باشد.",
                                        "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        SelectedShenavarID = accountId
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
                            LoadTreeData()
                            HideDataPanel()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف: " & ex.Message, "خطا",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
            End Select
        End Sub

        Private Sub dgvAccounts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAccounts.CellFormatting
            If e.RowIndex < 0 Then Return
            Dim colName = dgvAccounts.Columns(e.ColumnIndex).Name

            If colName = "colAccountName" Then
                Dim node = TryCast(dgvAccounts.Rows(e.RowIndex).Tag, ShenavarNode)
                If node IsNot Nothing Then
                    dgvAccounts.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.Padding =
                        New Padding(0, 0, node.Level * 20, 0)
                End If
            ElseIf colName = "colToggle" Then
                Dim txt = Convert.ToString(e.Value)
                If txt = "+" OrElse txt = "−" Then
                    e.CellStyle.BackColor = Color.FromArgb(225, 235, 255)
                    e.CellStyle.SelectionBackColor = Color.FromArgb(100, 130, 200)
                End If
            End If
        End Sub

        Private Sub dgvAccounts_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellMouseEnter
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvAccounts.Columns(e.ColumnIndex).Name = "colToggle" Then
                Dim node = TryCast(dgvAccounts.Rows(e.RowIndex).Tag, ShenavarNode)
                If node IsNot Nothing AndAlso node.HasChildren Then
                    dgvAccounts.Cursor = Cursors.Hand
                End If
            End If
        End Sub

        Private Sub dgvAccounts_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAccounts.CellMouseLeave
            dgvAccounts.Cursor = Cursors.Default
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
                        txtAccountName.Text = Convert.ToString(row.Cells("colAccountName").Value).Trim()
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
                LoadTreeData()
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

            Dim canCreate = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavarNew) OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanCreate)
            Dim canEdit = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavarEdit) OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanEdit)
            Dim canDelete = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavarDelete) OrElse SessionContext.HasPermission(PermissionKeys.AccountingShenavar & PermissionKeys.CanDelete)

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
