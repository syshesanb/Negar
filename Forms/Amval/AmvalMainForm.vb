Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Amval
    Public Class AmvalMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabAssets As TabPage
        Private tabDepreciation As TabPage
        Private tabReports As TabPage

        ' Tab Assets Controls
        Private dgvAssets As DataGridView
        Private btnAddAsset As Button

        ' Tab Depreciation Controls
        Private dgvDepreciation As DataGridView
        Private txtDepYear As TextBox
        Private cmbDepMonth As ComboBox
        Private btnCalculateDep As Button

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private txtRepYear As TextBox
        Private btnTaxReport As Button

        Private _amvalSvc As AmvalService
        Private _currentCompanyID As Integer
        Private _currentCompanyName As String

        Public Sub New()
            _amvalSvc = New AmvalService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🏛️ سیستم جامع مدیریت اموال و دارایی‌های ثابت"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID
            _currentCompanyName = SessionContext.CurrentCompanyName

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Assets
            tabAssets = New TabPage() With {.Text = "🏛️ شناسنامه و پلاک‌گذاری دارایی‌ها"}
            InitializeAssetsTab()
            tabControl.TabPages.Add(tabAssets)

            ' 2. Tab Depreciation
            tabDepreciation = New TabPage() With {.Text = "⚡ محاسبه استهلاک دوره (ماده ۱۴۹)"}
            InitializeDepreciationTab()
            tabControl.TabPages.Add(tabDepreciation)

            ' 3. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارشات جامع اموال و دارایی‌ها"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf AmvalMainForm_Load
        End Sub

        Private Sub AmvalMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadAssetsData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Assets Tab
        ' ----------------------------------------------------
        Private Sub InitializeAssetsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnAddAsset = New Button() With {
                .Text = "➕ ثبت دارایی جدید",
                .Size = New Size(160, 36),
                .Location = New Point(1020, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddAsset.Click, AddressOf BtnAddAsset_Click
            pnlTop.Controls.Add(btnAddAsset)

            dgvAssets = New DataGridView() With {
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
            dgvAssets.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvAssets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvAssets.DataBindingComplete, Sub(s, e) SetupAssetsGridColumns()
            AddHandler dgvAssets.CellContentClick, AddressOf DgvAssets_CellContentClick

            tabAssets.Controls.Add(dgvAssets)
            tabAssets.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadAssetsData()
            Dim dt = _amvalSvc.GetAssets(_currentCompanyID)
            dgvAssets.DataSource = dt
        End Sub

        Private Sub SetupAssetsGridColumns()
            If dgvAssets.Columns.Contains("colRowIndex") Then Return

            ' Add RowIndex column
            Dim colRow As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex",
                .HeaderText = "ردیف",
                .Width = 50,
                .ReadOnly = True
            }
            dgvAssets.Columns.Insert(0, colRow)

            ' Add Edit Button column
            Dim colEdit As New DataGridViewButtonColumn() With {
                .Name = "colEdit",
                .HeaderText = "ویرایش",
                .Text = "✏️ ویرایش",
                .UseColumnTextForButtonValue = True,
                .Width = 85
            }
            dgvAssets.Columns.Insert(1, colEdit)

            ' Add Delete Button column
            Dim colDelete As New DataGridViewButtonColumn() With {
                .Name = "colDelete",
                .HeaderText = "حذف",
                .Text = "❌ حذف",
                .UseColumnTextForButtonValue = True,
                .Width = 75
            }
            dgvAssets.Columns.Insert(2, colDelete)

            For i As Integer = 0 To dgvAssets.Rows.Count - 1
                dgvAssets.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgvAssets)
        End Sub

        Private Sub DgvAssets_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return

            Dim colName = dgvAssets.Columns(e.ColumnIndex).Name
            Dim assetID = Convert.ToInt32(dgvAssets.Rows(e.RowIndex).Cells("AssetID").Value)

            If colName = "colEdit" Then
                Using dlg As New AmvalAssetEditDialog(_currentCompanyID, assetID)
                    If dlg.ShowDialog() = DialogResult.OK Then LoadAssetsData()
                End Using
            ElseIf colName = "colDelete" Then
                If MessageBox.Show("آیا از حذف این دارایی ثابت اطمینان دارید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    _amvalSvc.DeleteAsset(assetID, _currentCompanyID)
                    LoadAssetsData()
                End If
            End If
        End Sub

        Private Sub BtnAddAsset_Click(sender As Object, e As EventArgs)
            Using dlg As New AmvalAssetEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadAssetsData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Depreciation Tab
        ' ----------------------------------------------------
        Private Sub InitializeDepreciationTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            Dim lblYear As New Label() With {.Text = "سال مالی:", .Location = New Point(1110, 15), .AutoSize = True}
            txtDepYear = New TextBox() With {.Text = GetCurrentFiscalYearTitle(), .Location = New Point(1030, 12), .Size = New Size(70, 26)}

            Dim lblMonth As New Label() With {.Text = "ماه:", .Location = New Point(980, 15), .AutoSize = True}
            cmbDepMonth = New ComboBox() With {.Location = New Point(890, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbDepMonth.Items.Add(i.ToString())
            Next
            cmbDepMonth.SelectedIndex = 0

            btnCalculateDep = New Button() With {
                .Text = "⚡ محاسبه و بستن استهلاک دوره",
                .Size = New Size(220, 36),
                .Location = New Point(650, 8),
                .BackColor = Color.FromArgb(230, 81, 0),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCalculateDep.Click, AddressOf BtnCalculateDep_Click

            pnlTop.Controls.AddRange(New Control() {lblYear, txtDepYear, lblMonth, cmbDepMonth, btnCalculateDep})

            dgvDepreciation = New DataGridView() With {
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
            dgvDepreciation.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvDepreciation.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvDepreciation.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabDepreciation.Controls.Add(dgvDepreciation)
            tabDepreciation.Controls.Add(pnlTop)
        End Sub

        Private Sub BtnCalculateDep_Click(sender As Object, e As EventArgs)
            Dim month = Convert.ToInt32(cmbDepMonth.SelectedItem)
            Dim year = txtDepYear.Text

            Dim res = _amvalSvc.CalculateDepreciationForPeriod(_currentCompanyID, year, month)
            If res Then
                MessageBox.Show("استهلاک دوره ماه " & month.ToString() & " سال " & year & " با موفقیت محاسبه شده و سند حسابداری آن صادر گردید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                dgvDepreciation.DataSource = _amvalSvc.GetDepreciationsForPeriod(_currentCompanyID, year, month)
            Else
                MessageBox.Show("هیچ دارایی ثابتی برای محاسبه استهلاک در این دوره یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End Sub

        ' ----------------------------------------------------
        ' 3. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            Dim lblYear As New Label() With {.Text = "سال مالی:", .Location = New Point(1110, 15), .AutoSize = True}
            txtRepYear = New TextBox() With {.Text = GetCurrentFiscalYearTitle(), .Location = New Point(1030, 12), .Size = New Size(70, 26)}

            btnTaxReport = New Button() With {
                .Text = "📊 جدول استهلاک دارایی‌ها (اداره دارایی)",
                .Size = New Size(240, 36),
                .Location = New Point(770, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnTaxReport.Click, Sub()
                                               dgvReport.DataSource = _amvalSvc.GetTaxDepreciationReport(_currentCompanyID, txtRepYear.Text)
                                           End Sub

            pnlTop.Controls.AddRange(New Control() {lblYear, txtRepYear, btnTaxReport})

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
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Function GetCurrentFiscalYearTitle() As String
            Dim title = SessionContext.CurrentFiscalYearName
            If String.IsNullOrWhiteSpace(title) Then title = "1405"
            Return title
        End Function

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                {"AssetID", "شناسه"},
                {"AssetCode", "کد دارایی"},
                {"PlakNo", "پلاک اموال" & vbCrLf & "(بارکد)"},
                {"AssetName", "نام / شرح دارایی"},
                {"CategoryName", "گروه دارایی"},
                {"PurchaseDate", "تاریخ خرید"},
                {"PurchasePrice", "بهای تمام‌شده" & vbCrLf & "(ریال)"},
                {"SalvageValue", "ارزش اسقاط" & vbCrLf & "(ریال)"},
                {"Location", "محل استقرار"},
                {"CustodianName", "امین اموال"},
                {"StatusTitle", "وضعیت"},
                {"MethodTitle", "روش استهلاک"},
                {"PeriodDepreciation", "استهلاک دوره" & vbCrLf & "(ریال)"},
                {"DepreciationAmount", "مبلغ استهلاک" & vbCrLf & "(ریال)"},
                {"AccumulatedDepreciation", "استهلاک انباشته" & vbCrLf & "(ریال)"},
                {"BookValue", "ارزش دفتری" & vbCrLf & "(ریال)"},
                {"SalMaly", "سال مالی"},
                {"MahMaly", "ماه"},
                {"SanadNo", "شماره سند"},
                {"Notes", "توضیحات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 130
            Next

            If dgv.Columns.Contains("AssetID") Then dgv.Columns("AssetID").Visible = False
            If dgv.Columns.Contains("AssetCode") Then dgv.Columns("AssetCode").Width = 90
            If dgv.Columns.Contains("PlakNo") Then dgv.Columns("PlakNo").Width = 110
            If dgv.Columns.Contains("AssetName") Then dgv.Columns("AssetName").Width = 200
            If dgv.Columns.Contains("CategoryName") Then dgv.Columns("CategoryName").Width = 140
        End Sub
    End Class
End Namespace
