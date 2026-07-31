Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.DMS
    Public Class DmsMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabArchive As TabPage
        Private tabCategories As TabPage
        Private tabExpiration As TabPage
        Private tabAuditLogs As TabPage

        ' Tab Archive Controls
        Private dgvArchive As DataGridView
        Private btnAddDocument As Button
        Private txtSearch As TextBox
        Private btnSearch As Button

        ' Tab Categories Controls
        Private dgvCategories As DataGridView

        ' Tab Expiration Controls
        Private dgvExpiration As DataGridView

        ' Tab AuditLogs Controls
        Private dgvAuditLogs As DataGridView

        Private _dmsSvc As DmsService
        Private _currentCompanyID As Integer

        Public Sub New()
            _dmsSvc = New DmsService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📁 سیستم جامع مدیریت بایگانی دیجیتال و آرشیو اسناد (Document Management System - DMS)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Archive
            tabArchive = New TabPage() With {.Text = "📂 پرونده‌ها و بایگانی دیجیتال اسناد"}
            InitializeArchiveTab()
            tabControl.TabPages.Add(tabArchive)

            ' 2. Tab Categories
            tabCategories = New TabPage() With {.Text = "🗂️ زون‌بندی و دسته‌بندی موضوعی اسناد"}
            InitializeCategoriesTab()
            tabControl.TabPages.Add(tabCategories)

            ' 3. Tab Expiration
            tabExpiration = New TabPage() With {.Text = "⏳ پایش سررسید انقضای قراردادها و تضامین"}
            InitializeExpirationTab()
            tabControl.TabPages.Add(tabExpiration)

            ' 4. Tab AuditLogs
            tabAuditLogs = New TabPage() With {.Text = "🔒 لاگ ردیابی امنیتی و دسترسی‌های فایل (Audit Trail)"}
            InitializeAuditLogsTab()
            tabControl.TabPages.Add(tabAuditLogs)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf DmsMainForm_Load
        End Sub

        Private Sub DmsMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadArchiveData()
            LoadCategoriesData()
            LoadExpirationData()
            LoadAuditLogsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Archive Tab
        ' ----------------------------------------------------
        Private Sub InitializeArchiveTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddDocument = New Button() With {
                .Text = "➕ اسکن و بایگانی جدید سند",
                .Size = New Size(210, 36),
                .Location = New Point(970, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddDocument.Click, AddressOf BtnAddDocument_Click

            txtSearch = New TextBox() With {.Location = New Point(320, 15), .Size = New Size(240, 26)}
            btnSearch = New Button() With {.Text = "🔍 جستجوی متنی (OCR / Index)", .Location = New Point(90, 12), .Size = New Size(220, 32)}
            AddHandler btnSearch.Click, Sub() LoadArchiveData(txtSearch.Text)

            pnlTop.Controls.AddRange(New Control() {btnAddDocument, txtSearch, btnSearch})

            dgvArchive = New DataGridView() With {
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
            dgvArchive.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvArchive.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabArchive.Controls.Add(dgvArchive)
            tabArchive.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadArchiveData(Optional keyword As String = "")
            Try
                Dim dt = _dmsSvc.GetDocuments(_currentCompanyID, "", keyword)
                dgvArchive.DataSource = dt
                SetupGridColumns(dgvArchive)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddDocument_Click(sender As Object, e As EventArgs)
            Using dlg As New DmsEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadArchiveData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Categories Tab
        ' ----------------------------------------------------
        Private Sub InitializeCategoriesTab()
            dgvCategories = New DataGridView() With {
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
            dgvCategories.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvCategories.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabCategories.Controls.Add(dgvCategories)
        End Sub

        Private Sub LoadCategoriesData()
            Try
                Dim dt = _dmsSvc.GetCategories()
                dgvCategories.DataSource = dt
                SetupGridColumns(dgvCategories)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Expiration Tab
        ' ----------------------------------------------------
        Private Sub InitializeExpirationTab()
            dgvExpiration = New DataGridView() With {
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
            dgvExpiration.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvExpiration.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabExpiration.Controls.Add(dgvExpiration)
        End Sub

        Private Sub LoadExpirationData()
            Try
                Dim dt = _dmsSvc.GetExpiringDocuments(_currentCompanyID)
                dgvExpiration.DataSource = dt
                SetupGridColumns(dgvExpiration)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 4. Audit Logs Tab
        ' ----------------------------------------------------
        Private Sub InitializeAuditLogsTab()
            dgvAuditLogs = New DataGridView() With {
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
            dgvAuditLogs.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvAuditLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabAuditLogs.Controls.Add(dgvAuditLogs)
        End Sub

        Private Sub LoadAuditLogsData()
            Try
                Dim dt = _dmsSvc.GetAuditLogs(_currentCompanyID)
                dgvAuditLogs.DataSource = dt
                SetupGridColumns(dgvAuditLogs)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub SetupGridColumns(dgv As DataGridView)
            Try
                If dgv Is Nothing OrElse dgv.Columns Is Nothing OrElse dgv.Columns.Count = 0 Then Return

                If dgv.Columns.Contains("colRowIndex") Then
                    For i As Integer = 0 To dgv.Rows.Count - 1
                        If i < dgv.Rows.Count Then
                            dgv.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
                        End If
                    Next
                End If

                ApplyPersianGridHeaders(dgv)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            Try
                If dgv Is Nothing OrElse dgv.Columns Is Nothing Then Return

                Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                    {"DocumentID", "شناسه سند"},
                    {"DocumentCode", "کد سند بایگانی"},
                    {"DocumentTitle", "عنوان سند / پرونده"},
                    {"CategoryName", "زون / رسته بایگانی"},
                    {"FileName", "نام فایل اسکن شده"},
                    {"FileSize", "حجم فایل"},
                    {"FileType", "نوع پسوند"},
                    {"VersionNumber", "نسخه سند"},
                    {"Keywords", "کلیدواژه‌ها (Indexing)"},
                    {"SecurityLevel", "سطح محرمانگی"},
                    {"ExpirationDate", "تاریخ انقضا"},
                    {"CreatedBy", "ثبت‌کننده سند"},
                    {"CategoryID", "شناسه زون"},
                    {"CategoryCode", "کد زون"},
                    {"CategoryTitle", "عنوان زون بایگانی"},
                    {"LogID", "شناسه لاگ"},
                    {"ActionType", "نوع اقدام امنیتی"},
                    {"UserName", "نام کاربر دسترسی‌گیرنده"},
                    {"AccessDate", "تاریخ و زمان دسترسی"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("DocumentID") Then dgv.Columns("DocumentID").Visible = False
                If dgv.Columns.Contains("CategoryID") Then dgv.Columns("CategoryID").Visible = False
                If dgv.Columns.Contains("LogID") Then dgv.Columns("LogID").Visible = False
                If dgv.Columns.Contains("DocumentTitle") Then dgv.Columns("DocumentTitle").Width = 220
                If dgv.Columns.Contains("CategoryName") Then dgv.Columns("CategoryName").Width = 180
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
