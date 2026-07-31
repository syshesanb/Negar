Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.QC
    Public Class QcMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabInspections As TabPage
        Private tabNcrCapa As TabPage
        Private tabScrap As TabPage
        Private tabReports As TabPage

        ' Tab Inspections Controls
        Private dgvInspections As DataGridView
        Private btnAddInspection As Button

        ' Tab NcrCapa Controls
        Private dgvNcrCapa As DataGridView

        ' Tab Scrap Controls
        Private dgvScrap As DataGridView
        Private btnApproveScrap As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _qcSvc As QcService
        Private _currentCompanyID As Integer

        Public Sub New()
            _qcSvc = New QcService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔬 سیستم جامع کنترل کیفیت و تضمین کیفیت (Quality Control & Assurance Management - QC/QA)"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Inspections
            tabInspections = New TabPage() With {.Text = "🔍 بازرسی‌های کیفی ورودی و حین تولید (IQC / IPQC)"}
            InitializeInspectionsTab()
            tabControl.TabPages.Add(tabInspections)

            ' 2. Tab NcrCapa
            tabNcrCapa = New TabPage() With {.Text = "⚠️ برگه‌های عدم انطباق و اقدامات اصلاحی (NCR / CAPA)"}
            InitializeNcrCapaTab()
            tabControl.TabPages.Add(tabNcrCapa)

            ' 3. Tab Scrap
            tabScrap = New TabPage() With {.Text = "♻️ ثبت ضایعات کیفی و صدور اسناد مالی (COQ)"}
            InitializeScrapTab()
            tabControl.TabPages.Add(tabScrap)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📈 گزارشات FPY، هزینه‌های عدم کیفیت و پارتو ضایعات"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf QcMainForm_Load
        End Sub

        Private Sub QcMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadInspectionsData()
            LoadNcrCapaData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Inspections Tab
        ' ----------------------------------------------------
        Private Sub InitializeInspectionsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddInspection = New Button() With {
                .Text = "➕ ثبت جدید برگه بازرسی کیفی (QC)",
                .Size = New Size(240, 36),
                .Location = New Point(940, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddInspection.Click, AddressOf BtnAddInspection_Click
            pnlTop.Controls.Add(btnAddInspection)

            dgvInspections = New DataGridView() With {
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
            dgvInspections.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvInspections.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabInspections.Controls.Add(dgvInspections)
            tabInspections.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadInspectionsData()
            Try
                Dim dt = _qcSvc.GetInspections(_currentCompanyID)
                dgvInspections.DataSource = dt
                SetupGridColumns(dgvInspections)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub BtnAddInspection_Click(sender As Object, e As EventArgs)
            Using dlg As New QcEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadInspectionsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. NcrCapa Tab
        ' ----------------------------------------------------
        Private Sub InitializeNcrCapaTab()
            dgvNcrCapa = New DataGridView() With {
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
            dgvNcrCapa.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvNcrCapa.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabNcrCapa.Controls.Add(dgvNcrCapa)
        End Sub

        Private Sub LoadNcrCapaData()
            Try
                Dim dt = _qcSvc.GetNcrCapas(_currentCompanyID)
                dgvNcrCapa.DataSource = dt
                SetupGridColumns(dgvNcrCapa)
            Catch ex As Exception
            End Try
        End Sub

        ' ----------------------------------------------------
        ' 3. Scrap Tab
        ' ----------------------------------------------------
        Private Sub InitializeScrapTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnApproveScrap = New Button() With {
                .Text = "🏆 ثبت قطعی ضایعات و صدور سند حسابداری کیفیت",
                .Size = New Size(370, 36),
                .Location = New Point(810, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnApproveScrap.Click, AddressOf BtnApproveScrap_Click
            pnlTop.Controls.Add(btnApproveScrap)

            dgvScrap = New DataGridView() With {
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
            dgvScrap.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvScrap.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            tabScrap.Controls.Add(dgvScrap)
            tabScrap.Controls.Add(pnlTop)
        End Sub

        Private Sub BtnApproveScrap_Click(sender As Object, e As EventArgs)
            If dgvInspections.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً ابتدا یک برگه بازرسی کیفی را از تب اول انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim inspId = Convert.ToInt32(dgvInspections.CurrentRow.Cells("InspectionID").Value)
            Dim salMaly = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(salMaly) Then salMaly = "1405"

            Dim res = _qcSvc.ApproveInspectionAndIssueSanad(inspId, _currentCompanyID, salMaly)
            If res Then
                MessageBox.Show("ضایعات کیفی با موفقیت تایید شد و سند حسابداری مربوطه در پشت پرده صادر گردید.", "موفقیت ثبت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadInspectionsData()
            Else
                MessageBox.Show("خطا در ثبت سند ضایعات کیفی.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 استخراج گزارش نرخ عبور کیفی بار اول (FPY)",
                .Size = New Size(390, 36),
                .Location = New Point(780, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                Try
                    Dim dt = _qcSvc.GetQcReport(_currentCompanyID)
                    dgvReport.DataSource = dt
                    SetupGridColumns(dgvReport)
                Catch ex As Exception
                End Try
            End Sub

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

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
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
                    {"InspectionID", "شناسه بازرسی"},
                    {"InspectionType", "نوع بازرسی کیفی"},
                    {"BatchNumber", "شماره محموله/بچ"},
                    {"ItemName", "نام کالا / محصول"},
                    {"SampleQuantity", "تعداد نمونه آزمایشی"},
                    {"PassedQuantity", "تعداد سالم (Pass)"},
                    {"RejectedQuantity", "تعداد ضایعات (Reject)"},
                    {"InspectorName", "بازرس مسئول QC"},
                    {"Result", "نتیجه بازرسی"},
                    {"NcrID", "شناسه NCR"},
                    {"NcrNumber", "شماره عدم انطباق (NCR)"},
                    {"IssueTitle", "عنوان عدم انطباق کیفی"},
                    {"Department", "واحد/ایستگاه مربوطه"},
                    {"RootCause", "تحلیل علت ریشه‌ای"},
                    {"CorrectiveAction", "اقدام اصلاحی (CAPA)"},
                    {"IssueDate", "تاریخ صدور NCR"},
                    {"ClosureDate", "تاریخ خاتمه/بستن"},
                    {"TotalInspections", "تعداد کل بازرسی‌ها"},
                    {"TotalSampleQty", "مجموع قطعات نمونه"},
                    {"TotalPassedQty", "مجموع قطعات سالم"},
                    {"TotalRejectedQty", "مجموع قطعات ضایعاتی"},
                    {"FpyPercentage", "نرخ عبور کیفی بار اول FPY (%)"},
                    {"Status", "وضعیت"},
                    {"Notes", "توضیحات"}
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then
                        col.HeaderText = dict(col.Name)
                    End If
                    col.Width = 140
                Next

                If dgv.Columns.Contains("InspectionID") Then dgv.Columns("InspectionID").Visible = False
                If dgv.Columns.Contains("NcrID") Then dgv.Columns("NcrID").Visible = False
                If dgv.Columns.Contains("BatchNumber") Then dgv.Columns("BatchNumber").Width = 120
                If dgv.Columns.Contains("ItemName") Then dgv.Columns("ItemName").Width = 200
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
