Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Moshtarak
    Public Class ShenavarTreePickerForm
        Inherits Form

        Private _pnlTop As Panel
        Private _lblTitle As Label
        Private _lblChain As Label
        Private _pnlSearch As Panel
        Private _txtSearchCode As TextBox
        Private _txtSearchName As TextBox
        Private _dgvAccounts As DataGridView
        Private _pnlBottom As Panel
        Private _btnCancel As Button

        Private _currentParentId As Integer? = Nothing
        Private _accountsWithChildren As New HashSet(Of Integer)()

        Public Property SelectedShenavarID As Integer? = Nothing
        Public Property SelectedAccountCode As String = String.Empty
        Public Property SelectedAccountName As String = String.Empty

        Public Sub New()
            InitializeForm()
        End Sub

        Private Sub InitializeForm()
            Me.Text = "انتخاب حساب شناور (فروشنده / تامین‌کننده)"
            Me.Size = New Size(800, 500)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)

            _pnlTop = New Panel()
            _pnlTop.Dock = DockStyle.Top
            _pnlTop.Height = 70
            _pnlTop.BackColor = Color.FromArgb(235, 243, 255)

            _lblTitle = New Label()
            _lblTitle.Text = "نمایش درختی و ساختار سلسله‌مراتبی حساب‌های شناور"
            _lblTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            _lblTitle.ForeColor = Color.FromArgb(20, 60, 120)
            _lblTitle.Dock = DockStyle.Top
            _lblTitle.Height = 30
            _lblTitle.TextAlign = ContentAlignment.MiddleCenter
            _pnlTop.Controls.Add(_lblTitle)

            _lblChain = New Label()
            _lblChain.Text = "سطح جاری: ریشه اصلی"
            _lblChain.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            _lblChain.ForeColor = Color.FromArgb(0, 102, 204)
            _lblChain.Dock = DockStyle.Bottom
            _lblChain.Height = 35
            _lblChain.TextAlign = ContentAlignment.MiddleCenter
            _pnlTop.Controls.Add(_lblChain)

            _pnlSearch = New Panel()
            _pnlSearch.Dock = DockStyle.Top
            _pnlSearch.Height = 35
            _pnlSearch.BackColor = Color.FromArgb(245, 248, 252)

            _txtSearchCode = New TextBox()
            _txtSearchCode.Size = New Size(140, 22)
            _txtSearchCode.Location = New Point(520, 6)
            AddHandler _txtSearchCode.TextChanged, AddressOf Search_TextChanged
            _pnlSearch.Controls.Add(_txtSearchCode)

            Dim lblCode As New Label()
            lblCode.Text = "جستجوی کد:"
            lblCode.Location = New Point(665, 8)
            lblCode.AutoSize = True
            _pnlSearch.Controls.Add(lblCode)

            _txtSearchName = New TextBox()
            _txtSearchName.Size = New Size(220, 22)
            _txtSearchName.Location = New Point(180, 6)
            AddHandler _txtSearchName.TextChanged, AddressOf Search_TextChanged
            _pnlSearch.Controls.Add(_txtSearchName)

            Dim lblName As New Label()
            lblName.Text = "جستجوی نام:"
            lblName.Location = New Point(405, 8)
            lblName.AutoSize = True
            _pnlSearch.Controls.Add(lblName)

            _dgvAccounts = New DataGridView()
            _dgvAccounts.Dock = DockStyle.Fill
            _dgvAccounts.AllowUserToAddRows = False
            _dgvAccounts.AllowUserToDeleteRows = False
            _dgvAccounts.ReadOnly = True
            _dgvAccounts.RowHeadersVisible = False
            _dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            _dgvAccounts.MultiSelect = False
            _dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            AddHandler _dgvAccounts.CellContentClick, AddressOf DgvAccounts_CellContentClick

            _pnlBottom = New Panel()
            _pnlBottom.Dock = DockStyle.Bottom
            _pnlBottom.Height = 40
            _pnlBottom.BackColor = Color.FromArgb(235, 240, 250)

            _btnCancel = New Button()
            _btnCancel.Text = "انصراف"
            _btnCancel.Size = New Size(100, 28)
            _btnCancel.Location = New Point(12, 6)
            AddHandler _btnCancel.Click, Sub() Me.Close()
            _pnlBottom.Controls.Add(_btnCancel)

            Me.Controls.Add(_dgvAccounts)
            Me.Controls.Add(_pnlSearch)
            Me.Controls.Add(_pnlTop)
            Me.Controls.Add(_pnlBottom)
        End Sub

        Private Sub ShenavarTreePickerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            SetupGrid()
            LoadLevel(Nothing)
        End Sub

        Private Sub SetupGrid()
            _dgvAccounts.Columns.Clear()
            _dgvAccounts.AutoGenerateColumns = False

            Dim colSelect As New DataGridViewButtonColumn()
            colSelect.Name = "colSelect"
            colSelect.HeaderText = "انتخاب"
            colSelect.Text = "انتخاب"
            colSelect.UseColumnTextForButtonValue = True
            colSelect.Width = 80
            colSelect.FlatStyle = FlatStyle.Standard

            Dim colChildren As New DataGridViewButtonColumn()
            colChildren.Name = "colChildren"
            colChildren.HeaderText = "زیرمجموعه"
            colChildren.Text = "..."
            colChildren.UseColumnTextForButtonValue = True
            colChildren.Width = 80
            colChildren.FlatStyle = FlatStyle.Standard

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "AccountCode"
            colCode.DataPropertyName = "AccountCode"
            colCode.HeaderText = "کد حساب"
            colCode.Width = 140

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "AccountName"
            colName.DataPropertyName = "AccountName"
            colName.HeaderText = "نام حساب"
            colName.Width = 320

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 60

            _dgvAccounts.Columns.AddRange(New DataGridViewColumn() {colSelect, colChildren, colCode, colName, colActive})
        End Sub

        Private Function GetVisibleUserIDsClause() As String
            If SessionContext.CurrentUser Is Nothing Then Return "0"
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return ""
            End If
            Dim ids = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
            Return ActivityLogService.BuildIDInClause(ids)
        End Function

        Private Sub LoadLevel(parentId As Integer?)
            _currentParentId = parentId
            Dim companyId = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, 0)
            Dim userClause = GetVisibleUserIDsClause()

            Dim query = "SELECT ShenavarID AS AccountID, AccountCode, AccountName, ParentShenavarID, IsActive " &
                        "FROM SarfaslShenavar WHERE CompanyID = " & companyId & " "

            If Not String.IsNullOrEmpty(userClause) Then
                query &= "AND (CreatedBy IS NULL OR CreatedBy IN (" & userClause & ")) "
            End If

            If parentId.HasValue Then
                query &= "AND ParentShenavarID = " & parentId.Value & " "
            Else
                query &= "AND ParentShenavarID IS NULL "
            End If

            query &= "ORDER BY AccountCode"

            Dim dt = Sql.ExecuteTable(query)
            _dgvAccounts.DataSource = dt

            ' به‌روزرسانی زنجیره
            If parentId.HasValue Then
                Dim parentName = Sql.ExecuteScalar("SELECT AccountName FROM SarfaslShenavar WHERE ShenavarID = ?", parentId.Value)
                _lblChain.Text = "سطح جاری: " & Convert.ToString(parentName)
            Else
                _lblChain.Text = "سطح جاری: ریشه اصلی"
            End If
        End Sub

        Private Sub Search_TextChanged(sender As Object, e As EventArgs)
            Dim codeVal = _txtSearchCode.Text.Trim().Replace("'", "''")
            Dim nameVal = _txtSearchName.Text.Trim().Replace("'", "''")

            If String.IsNullOrEmpty(codeVal) AndAlso String.IsNullOrEmpty(nameVal) Then
                LoadLevel(_currentParentId)
                Return
            End If

            Dim companyId = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, 0)
            Dim userClause = GetVisibleUserIDsClause()

            Dim query = "SELECT ShenavarID AS AccountID, AccountCode, AccountName, ParentShenavarID, IsActive " &
                        "FROM SarfaslShenavar WHERE CompanyID = " & companyId & " "

            If Not String.IsNullOrEmpty(userClause) Then
                query &= "AND (CreatedBy IS NULL OR CreatedBy IN (" & userClause & ")) "
            End If

            If Not String.IsNullOrEmpty(codeVal) Then
                query &= "AND AccountCode LIKE '%" & codeVal & "%' "
            End If
            If Not String.IsNullOrEmpty(nameVal) Then
                query &= "AND AccountName LIKE '%" & nameVal & "%' "
            End If

            query &= "ORDER BY AccountCode"
            _dgvAccounts.DataSource = Sql.ExecuteTable(query)
        End Sub

        Private Sub DgvAccounts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                Dim colName = _dgvAccounts.Columns(e.ColumnIndex).Name
                Dim row = _dgvAccounts.Rows(e.RowIndex)
                Dim accountId = Convert.ToInt32(row.Cells("colSelect").OwningColumn.DataGridView.Rows(e.RowIndex).Cells(2).OwningRow.Cells("AccountCode").Tag)
                
                ' Get values from DataSource row
                Dim drv = DirectCast(row.DataBoundItem, DataRowView)
                Dim sId = Convert.ToInt32(drv("AccountID"))
                Dim code = Convert.ToString(drv("AccountCode"))
                Dim name = Convert.ToString(drv("AccountName"))

                If colName = "colSelect" Then
                    SelectedShenavarID = sId
                    SelectedAccountCode = code
                    SelectedAccountName = name
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                ElseIf colName = "colChildren" Then
                    LoadLevel(sId)
                End If
            End If
        End Sub
    End Class
End Namespace
