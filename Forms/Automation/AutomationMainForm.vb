Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Automation
    Public Class AutomationMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabLetters As TabPage
        Private tabReferrals As TabPage
        Private tabSecretariat As TabPage
        Private tabReports As TabPage

        ' Tab Letters Controls
        Private dgvLetters As DataGridView
        Private btnAddLetter As Button
        Private cmbFilterType As ComboBox

        ' Tab Referrals Controls
        Private dgvReferrals As DataGridView
        Private cmbLetterSelect As ComboBox
        Private cmbToPersonnel As ComboBox
        Private txtInstruction As TextBox
        Private txtDeadline As TextBox
        Private btnSubmitReferral As Button

        ' Tab Secretariat Controls
        Private lblSecStats As Label
        Private dgvSecretariat As DataGridView

        ' Tab Reports Controls
        Private dgvReport As DataGridView
        Private btnLoadReport As Button

        Private _autoSvc As AutomationService
        Private _payrollSvc As PayrollService
        Private _currentCompanyID As Integer

        Public Sub New()
            _autoSvc = New AutomationService()
            _payrollSvc = New PayrollService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📨 سیستم جامع اتوماسیون اداری و دبیرخانه هوشمند"
            Me.Size = New Size(1200, 750)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Maximized
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 246, 248)

            _currentCompanyID = SessionContext.CurrentCompanyID

            tabControl = New TabControl() With {.Dock = DockStyle.Fill}

            ' 1. Tab Letters
            tabLetters = New TabPage() With {.Text = "📨 کارتابل نامه‌ها و مکاتبات اداری"}
            InitializeLettersTab()
            tabControl.TabPages.Add(tabLetters)

            ' 2. Tab Referrals
            tabReferrals = New TabPage() With {.Text = "🔃 ارجاعات، دستورات و هامش‌نویسی"}
            InitializeReferralsTab()
            tabControl.TabPages.Add(tabReferrals)

            ' 3. Tab Secretariat
            tabSecretariat = New TabPage() With {.Text = "🏢 دبیرخانه هوشمند و اندیکاتور"}
            InitializeSecretariatTab()
            tabControl.TabPages.Add(tabSecretariat)

            ' 4. Tab Reports
            tabReports = New TabPage() With {.Text = "📊 گزارشات جامع اتوماسیون اداری"}
            InitializeReportsTab()
            tabControl.TabPages.Add(tabReports)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf AutomationMainForm_Load
        End Sub

        Private Sub AutomationMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadLettersData()
            PopulateReferralCombos()
            LoadSecretariatData()
        End Sub

        ' ----------------------------------------------------
        ' 1. Letters Tab
        ' ----------------------------------------------------
        Private Sub InitializeLettersTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            Dim lblFilter As New Label() With {.Text = "فیلتر نوع نامه:", .Location = New Point(1110, 15), .AutoSize = True}
            cmbFilterType = New ComboBox() With {.Location = New Point(930, 12), .Size = New Size(170, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbFilterType.Items.AddRange(New Object() {"همه نامه‌ها", "نامه‌های وارده 📥", "نامه‌های صادره 📤", "یادداشت‌های داخلی 📝"})
            cmbFilterType.SelectedIndex = 0
            AddHandler cmbFilterType.SelectedIndexChanged, Sub() LoadLettersData()

            btnAddLetter = New Button() With {
                .Text = "➕ ثبت نامه جدید",
                .Size = New Size(160, 36),
                .Location = New Point(750, 8),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAddLetter.Click, AddressOf BtnAddLetter_Click

            pnlTop.Controls.AddRange(New Control() {lblFilter, cmbFilterType, btnAddLetter})

            dgvLetters = New DataGridView() With {
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
            dgvLetters.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLetters.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvLetters.DataBindingComplete, Sub(s, e) SetupLettersGridColumns()
            AddHandler dgvLetters.CellContentClick, AddressOf DgvLetters_CellContentClick

            tabLetters.Controls.Add(dgvLetters)
            tabLetters.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadLettersData()
            Dim filterIdx = cmbFilterType.SelectedIndex
            Dim dt = _autoSvc.GetLetters(_currentCompanyID, filterIdx)
            dgvLetters.DataSource = dt
        End Sub

        Private Sub SetupLettersGridColumns()
            If dgvLetters.Columns.Contains("colRowIndex") Then Return

            Dim colRow As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex",
                .HeaderText = "ردیف",
                .Width = 50,
                .ReadOnly = True
            }
            dgvLetters.Columns.Insert(0, colRow)

            Dim colEdit As New DataGridViewButtonColumn() With {
                .Name = "colEdit",
                .HeaderText = "ویرایش",
                .Text = "✏️ ویرایش",
                .UseColumnTextForButtonValue = True,
                .Width = 85
            }
            dgvLetters.Columns.Insert(1, colEdit)

            Dim colDelete As New DataGridViewButtonColumn() With {
                .Name = "colDelete",
                .HeaderText = "حذف",
                .Text = "❌ حذف",
                .UseColumnTextForButtonValue = True,
                .Width = 75
            }
            dgvLetters.Columns.Insert(2, colDelete)

            For i As Integer = 0 To dgvLetters.Rows.Count - 1
                dgvLetters.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
            Next

            ApplyPersianGridHeaders(dgvLetters)
        End Sub

        Private Sub DgvLetters_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return

            Dim colName = dgvLetters.Columns(e.ColumnIndex).Name
            Dim letterID = Convert.ToInt32(dgvLetters.Rows(e.RowIndex).Cells("LetterID").Value)

            If colName = "colEdit" Then
                Using dlg As New AutomationLetterEditDialog(_currentCompanyID, letterID)
                    If dlg.ShowDialog() = DialogResult.OK Then LoadLettersData()
                End Using
            ElseIf colName = "colDelete" Then
                If MessageBox.Show("آیا از حذف این نامه اداری اطمینان دارید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    _autoSvc.DeleteLetter(letterID, _currentCompanyID)
                    LoadLettersData()
                End If
            End If
        End Sub

        Private Sub BtnAddLetter_Click(sender As Object, e As EventArgs)
            Using dlg As New AutomationLetterEditDialog(_currentCompanyID)
                If dlg.ShowDialog() = DialogResult.OK Then LoadLettersData()
            End Using
        End Sub

        ' ----------------------------------------------------
        ' 2. Referrals Tab
        ' ----------------------------------------------------
        Private Sub InitializeReferralsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 110, .BackColor = Color.FromArgb(235, 238, 242)}

            Dim lblLetter As New Label() With {.Text = "انتخاب نامه:", .Location = New Point(1110, 15), .AutoSize = True}
            cmbLetterSelect = New ComboBox() With {.Location = New Point(730, 12), .Size = New Size(370, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            AddHandler cmbLetterSelect.SelectedIndexChanged, AddressOf CmbLetterSelect_SelectedIndexChanged

            Dim lblTarget As New Label() With {.Text = "ارجاع به:", .Location = New Point(640, 15), .AutoSize = True}
            cmbToPersonnel = New ComboBox() With {.Location = New Point(380, 12), .Size = New Size(250, 26), .DropDownStyle = ComboBoxStyle.DropDownList}

            Dim lblInst As New Label() With {.Text = "دستور / هامش مدیر:", .Location = New Point(1100, 60), .AutoSize = True}
            txtInstruction = New TextBox() With {.Location = New Point(530, 57), .Size = New Size(560, 26)}

            Dim lblDead As New Label() With {.Text = "مهلت اقدام:", .Location = New Point(440, 60), .AutoSize = True}
            txtDeadline = New TextBox() With {.Location = New Point(330, 57), .Size = New Size(100, 26), .Text = PersianDateHelper.ToPersian(DateTime.Now)}

            btnSubmitReferral = New Button() With {
                .Text = "🔃 ثبت و ارسال ارجاع",
                .Size = New Size(180, 36),
                .Location = New Point(130, 52),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSubmitReferral.Click, AddressOf BtnSubmitReferral_Click

            pnlTop.Controls.AddRange(New Control() {lblLetter, cmbLetterSelect, lblTarget, cmbToPersonnel, lblInst, txtInstruction, lblDead, txtDeadline, btnSubmitReferral})

            dgvReferrals = New DataGridView() With {
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
            dgvReferrals.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvReferrals.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvReferrals.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReferrals.Controls.Add(dgvReferrals)
            tabReferrals.Controls.Add(pnlTop)
        End Sub

        Private Sub PopulateReferralCombos()
            cmbLetterSelect.Items.Clear()
            Dim dt = _autoSvc.GetLetters(_currentCompanyID)
            If dt IsNot Nothing Then
                For Each r As DataRow In dt.Rows
                    Dim lId = Convert.ToInt32(r("LetterID"))
                    Dim lNo = Convert.ToString(r("LetterNo"))
                    Dim subj = Convert.ToString(r("Subject"))
                    cmbLetterSelect.Items.Add(New KeyValuePair(Of Integer, String)(lId, lNo & " - " & subj))
                Next
            End If
            If cmbLetterSelect.Items.Count > 0 Then cmbLetterSelect.SelectedIndex = 0

            cmbToPersonnel.Items.Clear()
            Dim dtP = _payrollSvc.GetPersonnelList()
            If dtP IsNot Nothing Then
                For Each r As DataRow In dtP.Rows
                    cmbToPersonnel.Items.Add(New KeyValuePair(Of Integer, String)(Convert.ToInt32(r("PersonnelID")), Convert.ToString(r("FullName"))))
                Next
            End If
            If cmbToPersonnel.Items.Count > 0 Then cmbToPersonnel.SelectedIndex = 0
        End Sub

        Private Sub CmbLetterSelect_SelectedIndexChanged(sender As Object, e As EventArgs)
            If cmbLetterSelect.SelectedItem IsNot Nothing Then
                Dim lId = CType(cmbLetterSelect.SelectedItem, KeyValuePair(Of Integer, String)).Key
                dgvReferrals.DataSource = _autoSvc.GetReferralsForLetter(lId)
            End If
        End Sub

        Private Sub BtnSubmitReferral_Click(sender As Object, e As EventArgs)
            If cmbLetterSelect.SelectedItem Is Nothing OrElse cmbToPersonnel.SelectedItem Is Nothing Then Return

            Dim lId = CType(cmbLetterSelect.SelectedItem, KeyValuePair(Of Integer, String)).Key
            Dim toP = CType(cmbToPersonnel.SelectedItem, KeyValuePair(Of Integer, String)).Key

            _autoSvc.AddReferral(lId, _currentCompanyID, 0, toP, txtInstruction.Text, txtDeadline.Text)

            MessageBox.Show("دستور ارجاع نامه با موفقیت صادر و به کارتابل شخص ارسال شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
            dgvReferrals.DataSource = _autoSvc.GetReferralsForLetter(lId)
            LoadLettersData()
        End Sub

        ' ----------------------------------------------------
        ' 3. Secretariat Tab
        ' ----------------------------------------------------
        Private Sub InitializeSecretariatTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 65, .BackColor = Color.FromArgb(227, 242, 253)}

            lblSecStats = New Label() With {
                .Text = "📊 آمار دبیرخانه مرکزی:  تعداد کل مکاتبات ثبت‌شده: ۰  |  نامه‌های وارده: ۰  |  نامه‌های صادره: ۰  |  یادداشت داخلی: ۰",
                .Font = New Font("Tahoma", 10.5!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(13, 71, 161),
                .Location = New Point(100, 20),
                .AutoSize = True
            }
            pnlTop.Controls.Add(lblSecStats)

            dgvSecretariat = New DataGridView() With {
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
            dgvSecretariat.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvSecretariat.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler dgvSecretariat.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabSecretariat.Controls.Add(dgvSecretariat)
            tabSecretariat.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadSecretariatData()
            Dim dt = _autoSvc.GetLetters(_currentCompanyID)
            dgvSecretariat.DataSource = dt

            If dt IsNot Nothing Then
                Dim total = dt.Rows.Count
                Dim incoming = 0, outgoing = 0, internalCount = 0
                For Each r As DataRow In dt.Rows
                    Dim tStr = Convert.ToString(r("TypeTitle"))
                    If tStr.Contains("وارده") Then
                        incoming += 1
                    ElseIf tStr.Contains("صادره") Then
                        outgoing += 1
                    Else
                        internalCount += 1
                    End If
                Next
                lblSecStats.Text = "📊 آمار دبیرخانه مرکزی:  تعداد کل مکاتبات: " & total.ToString() & "  |  نامه‌های وارده: " & incoming.ToString() & "  |  نامه‌های صادره: " & outgoing.ToString() & "  |  یادداشت داخلی: " & internalCount.ToString()
            End If
        End Sub

        ' ----------------------------------------------------
        ' 4. Reports Tab
        ' ----------------------------------------------------
        Private Sub InitializeReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 238, 242)}

            btnLoadReport = New Button() With {
                .Text = "📊 دریافت گزارش جامع مکاتبات و ارجاعات",
                .Size = New Size(260, 36),
                .Location = New Point(910, 8),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadReport.Click, Sub()
                                                dgvReport.DataSource = _autoSvc.GetAutomationReports(_currentCompanyID)
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
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                {"LetterID", "شناسه"},
                {"LetterNo", "شماره اندیکاتور"},
                {"LetterDate", "تاریخ ثبت"},
                {"TypeTitle", "نوع مکاتبه"},
                {"LetterType", "نوع نامه"},
                {"Subject", "موضوع / عنوان نامه"},
                {"SenderInfo", "فرستنده"},
                {"ReceiverInfo", "گیرنده اصلی"},
                {"PriorityTitle", "اولویت"},
                {"ConfidentialityTitle", "سطح محرمانه"},
                {"Status", "وضعیت اقدام"},
                {"ContentBody", "متن/شرح نامه"},
                {"ReferralID", "شناسه ارجاع"},
                {"ReferralDate", "تاریخ ارجاع"},
                {"FromPerson", "ارسال‌کننده (دستور دهنده)"},
                {"ToPerson", "ارجاع‌شونده (اقدام‌کننده)"},
                {"InstructionText", "دستور / هامش مدیر"},
                {"DeadlineDate", "مهلت اقدام"},
                {"ReferralCount", "تعداد ارجاعات"}
            }

            For Each col As DataGridViewColumn In dgv.Columns
                If dict.ContainsKey(col.Name) Then
                    col.HeaderText = dict(col.Name)
                End If
                col.Width = 130
            Next

            If dgv.Columns.Contains("LetterID") Then dgv.Columns("LetterID").Visible = False
            If dgv.Columns.Contains("ReferralID") Then dgv.Columns("ReferralID").Visible = False
            If dgv.Columns.Contains("LetterNo") Then dgv.Columns("LetterNo").Width = 120
            If dgv.Columns.Contains("Subject") Then dgv.Columns("Subject").Width = 220
            If dgv.Columns.Contains("InstructionText") Then dgv.Columns("InstructionText").Width = 240
            If dgv.Columns.Contains("ContentBody") Then dgv.Columns("ContentBody").Width = 250
        End Sub
    End Class
End Namespace
