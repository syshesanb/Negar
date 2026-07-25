Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms

    Public Class UserPickerForm
        Inherits Form

        Private _pnlTop As Panel
        Private _lblTitle As Label
        Private _lblSearch As Label
        Private _txtSearch As TextBox
        Private _dgvUsers As DataGridView
        Private _pnlBottom As Panel
        Private _btnSelect As Button
        Private _btnCancel As Button

        Private _userService As New UserService()
        Private _dtUsers As DataTable

        Public Property SelectedUserId As Integer? = Nothing
        Public Property SelectedUsername As String = String.Empty
        Public Property SelectedUserFullName As String = String.Empty

        Public Sub New()
            BuildUI()
        End Sub

        Private Sub UserPickerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadUsers()
        End Sub

        Private Sub BuildUI()
            Me.Text = "انتخاب انباردار (کاربران عادی)"
            Me.Size = New Size(520, 420)
            Me.MinimumSize = New Size(450, 350)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.KeyPreview = True

            ' Top Panel
            _pnlTop = New Panel()
            _pnlTop.Dock = DockStyle.Top
            _pnlTop.Height = 65
            _pnlTop.Padding = New Padding(10)

            _lblTitle = New Label()
            _lblTitle.Text = "لیست کاربران عادی ایجاد شده توسط شما:"
            _lblTitle.AutoSize = True
            _lblTitle.Location = New Point(10, 8)
            _lblTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)

            _lblSearch = New Label()
            _lblSearch.Text = "جستجو:"
            _lblSearch.AutoSize = True
            _lblSearch.Location = New Point(440, 35)

            _txtSearch = New TextBox()
            _txtSearch.Location = New Point(10, 32)
            _txtSearch.Size = New Size(420, 22)
            AddHandler _txtSearch.TextChanged, AddressOf TxtSearch_TextChanged

            _pnlTop.Controls.Add(_lblTitle)
            _pnlTop.Controls.Add(_lblSearch)
            _pnlTop.Controls.Add(_txtSearch)

            ' DataGridView
            _dgvUsers = New DataGridView()
            _dgvUsers.Dock = DockStyle.Fill
            _dgvUsers.AutoGenerateColumns = False
            _dgvUsers.AllowUserToAddRows = False
            _dgvUsers.AllowUserToDeleteRows = False
            _dgvUsers.ReadOnly = True
            _dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            _dgvUsers.MultiSelect = False
            _dgvUsers.RowHeadersVisible = False
            _dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "UserID"
            colId.DataPropertyName = "UserID"
            colId.Visible = False

            Dim colUsername As New DataGridViewTextBoxColumn()
            colUsername.Name = "Username"
            colUsername.DataPropertyName = "Username"
            colUsername.HeaderText = "نام کاربری"
            colUsername.Width = 180

            Dim colFullName As New DataGridViewTextBoxColumn()
            colFullName.Name = "FullName"
            colFullName.DataPropertyName = "FullName"
            colFullName.HeaderText = "نام و نام خانوادگی"
            colFullName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            _dgvUsers.Columns.Add(colId)
            _dgvUsers.Columns.Add(colUsername)
            _dgvUsers.Columns.Add(colFullName)

            AddHandler _dgvUsers.CellDoubleClick, AddressOf DgvUsers_CellDoubleClick
            AddHandler _dgvUsers.KeyDown, AddressOf DgvUsers_KeyDown

            ' Bottom Panel
            _pnlBottom = New Panel()
            _pnlBottom.Dock = DockStyle.Bottom
            _pnlBottom.Height = 45

            _btnSelect = New Button()
            _btnSelect.Text = "انتخاب"
            _btnSelect.Size = New Size(90, 30)
            _btnSelect.Location = New Point(105, 7)
            _btnSelect.BackColor = Color.FromArgb(40, 167, 69)
            _btnSelect.ForeColor = Color.White
            _btnSelect.FlatStyle = FlatStyle.Flat
            AddHandler _btnSelect.Click, AddressOf BtnSelect_Click

            _btnCancel = New Button()
            _btnCancel.Text = "انصراف"
            _btnCancel.Size = New Size(90, 30)
            _btnCancel.Location = New Point(10, 7)
            _btnCancel.BackColor = Color.FromArgb(108, 117, 125)
            _btnCancel.ForeColor = Color.White
            _btnCancel.FlatStyle = FlatStyle.Flat
            _btnCancel.DialogResult = DialogResult.Cancel
            AddHandler _btnCancel.Click, Sub() Me.DialogResult = DialogResult.Cancel

            _pnlBottom.Controls.Add(_btnSelect)
            _pnlBottom.Controls.Add(_btnCancel)

            Me.Controls.Add(_dgvUsers)
            Me.Controls.Add(_pnlTop)
            Me.Controls.Add(_pnlBottom)

            Me.AcceptButton = _btnSelect
            Me.CancelButton = _btnCancel
        End Sub

        Private Sub LoadUsers()
            Try
                _dtUsers = _userService.GetUsersByTypes("User")
                _dgvUsers.DataSource = _dtUsers
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست کاربران: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
            If _dtUsers Is Nothing Then Return
            Dim filterText = _txtSearch.Text.Trim().Replace("'", "''")
            If String.IsNullOrEmpty(filterText) Then
                _dtUsers.DefaultView.RowFilter = String.Empty
            Else
                _dtUsers.DefaultView.RowFilter = String.Format("Username LIKE '%{0}%' OR FullName LIKE '%{0}%'", filterText)
            End If
        End Sub

        Private Sub SelectCurrentRow()
            If _dgvUsers.CurrentRow IsNot Nothing AndAlso Not _dgvUsers.CurrentRow.IsNewRow Then
                Dim row = CType(_dgvUsers.CurrentRow.DataBoundItem, DataRowView).Row
                SelectedUserId = Convert.ToInt32(row("UserID"))
                SelectedUsername = Convert.ToString(row("Username"))
                Dim fn = Convert.ToString(row("FullName"))
                SelectedUserFullName = If(String.IsNullOrWhiteSpace(fn), SelectedUsername, fn)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End Sub

        Private Sub BtnSelect_Click(sender As Object, e As EventArgs)
            SelectCurrentRow()
        End Sub

        Private Sub DgvUsers_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                SelectCurrentRow()
            End If
        End Sub

        Private Sub DgvUsers_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                e.Handled = True
                SelectCurrentRow()
            End If
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            If keyData = Keys.Escape Then
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
                Return True
            End If
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function
    End Class

End Namespace
