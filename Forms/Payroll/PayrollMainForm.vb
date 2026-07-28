Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Business

Namespace Negar.Forms.Payroll
    Public Class PayrollMainForm
        Inherits AppBaseForm

        Private _payrollSvc As New PayrollService()
        Private tabControl As TabControl

        ' Tab Pages
        Private tabPersonnel As TabPage
        Private tabAttendance As TabPage
        Private tabCalculate As TabPage
        Private tabDiskettes As TabPage
        Private tabBankFile As TabPage
        Private tabReports As TabPage

        ' Personnel Controls
        Private dgvPersonnel As DataGridView
        Private btnAddPersonnel As Button
        Private btnEditPersonnel As Button
        Private btnDeletePersonnel As Button

        ' Attendance Controls
        Private dgvAttendance As DataGridView
        Private cmbAttMonth As ComboBox
        Private txtAttYear As TextBox
        Private btnSaveAttendance As Button

        ' Calculate Controls
        Private dgvCalculate As DataGridView
        Private cmbCalcMonth As ComboBox
        Private txtCalcYear As TextBox
        Private btnRunCalc As Button

        ' Diskettes Controls
        Private rtbDiskette As RichTextBox
        Private cmbDisksMonth As ComboBox
        Private txtDisksYear As TextBox
        Private btnGenInsuranceDisk As Button
        Private btnGenTaxDisk As Button

        ' Bank File Controls
        Private rtbBank As RichTextBox
        Private cmbBankMonth As ComboBox
        Private txtBankYear As TextBox
        Private btnGenBankFile As Button

        ' Reports Controls
        Private dgvReport As DataGridView
        Private cmbRepMonth As ComboBox
        Private txtRepYear As TextBox
        Private btnLoadMonthlyRep As Button
        Private cmbPersonnelRep As ComboBox
        Private btnLoadPersonRep As Button

        Public Sub New()
            InitializeComponentCustom()
        End Sub

        Private Sub InitializeComponentCustom()
            Me.Text = "سیستم جامع حقوق و دستمزد و کارکرد پرسنل نگار"
            Me.Size = New Size(1100, 700)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.FromArgb(245, 247, 250)

            ' Main TabControl
            tabControl = New TabControl() With {
                .Dock = DockStyle.Fill,
                .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                .Padding = New Point(12, 8)
            }

            ' Create Tabs
            tabPersonnel = New TabPage("👥 پرونده پرسنل و احکام حقوقی")
            tabAttendance = New TabPage("🕒 کارکرد ماهانه و حضور و غیاب")
            tabCalculate = New TabPage("🧮 محاسبه حقوق و صدور فیش")
            tabDiskettes = New TabPage("🏛️ دیسکت‌های بیمه و مالیات")
            tabBankFile = New TabPage("🏦 فایل پرداخت گروهی بانک")
            tabReports = New TabPage("📊 گزارشات جامع حقوق و دستمزد")

            tabControl.TabPages.AddRange(New TabPage() {
                tabPersonnel, tabAttendance, tabCalculate, tabDiskettes, tabBankFile, tabReports
            })

            SetupPersonnelTab()
            SetupAttendanceTab()
            SetupCalculateTab()
            SetupDiskettesTab()
            SetupBankFileTab()
            SetupReportsTab()

            Me.Controls.Add(tabControl)

            AddHandler Me.Load, AddressOf PayrollMainForm_Load
        End Sub

        Private Sub PayrollMainForm_Load(sender As Object, e As EventArgs)
            LoadPersonnelData()
        End Sub

        ' ─── 1. TAB PERSONNEL ──────
        Private Sub SetupPersonnelTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 50, .BackColor = Color.FromArgb(235, 240, 245)}
            
            btnAddPersonnel = New Button() With {
                .Text = "➕ ثبت پرسنل جدید",
                .Size = New Size(150, 36),
                .Location = New Point(920, 7),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            }
            AddHandler btnAddPersonnel.Click, AddressOf BtnAddPersonnel_Click

            btnDeletePersonnel = New Button() With {
                .Text = "🗑️ حذف پرسنل",
                .Size = New Size(120, 36),
                .Location = New Point(790, 7),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            }
            AddHandler btnDeletePersonnel.Click, AddressOf BtnDeletePersonnel_Click

            pnlTop.Controls.AddRange(New Control() {btnAddPersonnel, btnDeletePersonnel})

            dgvPersonnel = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            AddHandler dgvPersonnel.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabPersonnel.Controls.Add(dgvPersonnel)
            tabPersonnel.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadPersonnelData()
            dgvPersonnel.DataSource = _payrollSvc.GetPersonnelList()
            FillPersonnelCombo()
        End Sub

        Private Sub BtnAddPersonnel_Click(sender As Object, e As EventArgs)
            Dim name = InputBox("نام و نام خانوادگی پرسنل را وارد کنید:", "ثبت پرسنل جدید")
            If String.IsNullOrWhiteSpace(name) Then Return
            Dim nCode = InputBox("شماره ملی پرسنل:", "اطلاعات پرسنل")
            Dim insNum = InputBox("شماره بیمه تامین اجتماعی:", "اطلاعات بیمه")
            Dim baseSalStr = InputBox("مبلغ حقوق پایه (ریال):", "حقوق پایه", "100000000")
            Dim baseSal As Decimal = 100000000
            Decimal.TryParse(baseSalStr, baseSal)

            _payrollSvc.SavePersonnel(Nothing, name, nCode, insNum, "6037991234567890", "IR120170000000001234567890", "قراردادی", "متاهل", 1, baseSal, 9000000, 14000000, 5000000, 2000000, 0, True)
            LoadPersonnelData()
            MessageBox.Show("اطلاعات پرسنل جدید با موفقیت ذخیره شد.", "تایید", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub BtnDeletePersonnel_Click(sender As Object, e As EventArgs)
            If dgvPersonnel.CurrentRow IsNot Nothing Then
                Dim id = Convert.ToInt32(dgvPersonnel.CurrentRow.Cells("PersonnelID").Value)
                If MessageBox.Show("آیا از حذف این پرسنل اطمینان دارید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                    _payrollSvc.DeletePersonnel(id)
                    LoadPersonnelData()
                End If
            End If
        End Sub

        ' ─── 2. TAB ATTENDANCE ──────
        Private Sub SetupAttendanceTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 240, 245)}
            
            Dim lblYear As New Label() With {.Text = "سال مالی:", .Location = New Point(1000, 16), .AutoSize = True}
            txtAttYear = New TextBox() With {.Text = "1405", .Location = New Point(920, 12), .Size = New Size(70, 26)}
            
            Dim lblMonth As New Label() With {.Text = "ماه:", .Location = New Point(875, 16), .AutoSize = True}
            cmbAttMonth = New ComboBox() With {.Location = New Point(790, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbAttMonth.Items.Add(i.ToString())
            Next
            cmbAttMonth.SelectedIndex = 0

            btnSaveAttendance = New Button() With {
                .Text = "💾 ذخیره کارکرد ماهانه",
                .Size = New Size(160, 34),
                .Location = New Point(600, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSaveAttendance.Click, AddressOf BtnSaveAttendance_Click

            pnlTop.Controls.AddRange(New Control() {lblYear, txtAttYear, lblMonth, cmbAttMonth, btnSaveAttendance})

            dgvAttendance = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            AddHandler dgvAttendance.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabAttendance.Controls.Add(dgvAttendance)
            tabAttendance.Controls.Add(pnlTop)
        End Sub

        Private Sub BtnSaveAttendance_Click(sender As Object, e As EventArgs)
            Dim sal = txtAttYear.Text.Trim()
            Dim mah = Convert.ToInt32(cmbAttMonth.SelectedItem)
            Dim pList = _payrollSvc.GetPersonnelList()
            If pList IsNot Nothing Then
                For Each r As DataRow In pList.Rows
                    Dim pId = Convert.ToInt32(r("PersonnelID"))
                    _payrollSvc.SaveMonthlyAttendance(pId, sal, mah, 30, 10, 0, 0, 0, 0, 0)
                Next
            End If
            dgvAttendance.DataSource = _payrollSvc.GetMonthlyAttendance(sal, mah)
            MessageBox.Show($"کارکرد ماهانه برای ماه {mah} با موفقیت ثبت شد.", "تایید", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ' ─── 3. TAB CALCULATE ──────
        Private Sub SetupCalculateTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 240, 245)}
            
            Dim lblYear As New Label() With {.Text = "سال مالی:", .Location = New Point(1000, 16), .AutoSize = True}
            txtCalcYear = New TextBox() With {.Text = "1405", .Location = New Point(920, 12), .Size = New Size(70, 26)}
            
            Dim lblMonth As New Label() With {.Text = "ماه:", .Location = New Point(875, 16), .AutoSize = True}
            cmbCalcMonth = New ComboBox() With {.Location = New Point(790, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbCalcMonth.Items.Add(i.ToString())
            Next
            cmbCalcMonth.SelectedIndex = 0

            btnRunCalc = New Button() With {
                .Text = "⚡ محاسبه حقوق و صدور سند اتوماتیک",
                .Size = New Size(240, 34),
                .Location = New Point(520, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnRunCalc.Click, AddressOf BtnRunCalc_Click

            pnlTop.Controls.AddRange(New Control() {lblYear, txtCalcYear, lblMonth, cmbCalcMonth, btnRunCalc})

            dgvCalculate = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            AddHandler dgvCalculate.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabCalculate.Controls.Add(dgvCalculate)
            tabCalculate.Controls.Add(pnlTop)
        End Sub

        Private Sub BtnRunCalc_Click(sender As Object, e As EventArgs)
            Dim sal = txtCalcYear.Text.Trim()
            Dim mah = Convert.ToInt32(cmbCalcMonth.SelectedItem)
            Dim dt = _payrollSvc.CalculatePayrollForMonth(sal, mah)
            dgvCalculate.DataSource = dt
            MessageBox.Show($"محاسبه حقوق و صدور سند حسابداری اتوماتیک برای ماه {mah} با موفقیت انجام گردید.", "محاسبه موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ' ─── 4. TAB DISKETTES ──────
        Private Sub SetupDiskettesTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 240, 245)}
            
            txtDisksYear = New TextBox() With {.Text = "1405", .Location = New Point(920, 12), .Size = New Size(70, 26)}
            cmbDisksMonth = New ComboBox() With {.Location = New Point(790, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbDisksMonth.Items.Add(i.ToString())
            Next
            cmbDisksMonth.SelectedIndex = 0

            btnGenInsuranceDisk = New Button() With {
                .Text = "🏛️ تولید دیسکت بیمه (DBF)",
                .Size = New Size(180, 34),
                .Location = New Point(590, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnGenInsuranceDisk.Click, Sub()
                                                      rtbDiskette.Text = _payrollSvc.GenerateSocialSecurityDisketteText(txtDisksYear.Text, Convert.ToInt32(cmbDisksMonth.SelectedItem))
                                                  End Sub

            btnGenTaxDisk = New Button() With {
                .Text = "📑 تولید دیسکت مالیات حقوق",
                .Size = New Size(180, 34),
                .Location = New Point(395, 10),
                .BackColor = Color.FromArgb(216, 67, 21),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnGenTaxDisk.Click, Sub()
                                                rtbDiskette.Text = _payrollSvc.GenerateTaxDisketteText(txtDisksYear.Text, Convert.ToInt32(cmbDisksMonth.SelectedItem))
                                            End Sub

            pnlTop.Controls.AddRange(New Control() {txtDisksYear, cmbDisksMonth, btnGenInsuranceDisk, btnGenTaxDisk})

            rtbDiskette = New RichTextBox() With {
                .Dock = DockStyle.Fill,
                .Font = New Font("Consolas", 10.0!),
                .BackColor = Color.FromArgb(30, 30, 30),
                .ForeColor = Color.FromArgb(76, 175, 80)
            }

            tabDiskettes.Controls.Add(rtbDiskette)
            tabDiskettes.Controls.Add(pnlTop)
        End Sub

        ' ─── 5. TAB BANK FILE ──────
        Private Sub SetupBankFileTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 240, 245)}
            
            txtBankYear = New TextBox() With {.Text = "1405", .Location = New Point(920, 12), .Size = New Size(70, 26)}
            cmbBankMonth = New ComboBox() With {.Location = New Point(790, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbBankMonth.Items.Add(i.ToString())
            Next
            cmbBankMonth.SelectedIndex = 0

            btnGenBankFile = New Button() With {
                .Text = "🏦 ساخت فایل پایا / ساتنا بانک",
                .Size = New Size(220, 34),
                .Location = New Point(550, 10),
                .BackColor = Color.FromArgb(0, 121, 107),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnGenBankFile.Click, Sub()
                                                 rtbBank.Text = _payrollSvc.GenerateBankPaymentFileText(txtBankYear.Text, Convert.ToInt32(cmbBankMonth.SelectedItem))
                                             End Sub

            pnlTop.Controls.AddRange(New Control() {txtBankYear, cmbBankMonth, btnGenBankFile})

            rtbBank = New RichTextBox() With {
                .Dock = DockStyle.Fill,
                .Font = New Font("Consolas", 10.0!),
                .BackColor = Color.FromArgb(30, 30, 30),
                .ForeColor = Color.FromArgb(33, 150, 243)
            }

            tabBankFile.Controls.Add(rtbBank)
            tabBankFile.Controls.Add(pnlTop)
        End Sub

        ' ─── 6. TAB REPORTS ──────
        Private Sub SetupReportsTab()
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(235, 240, 245)}
            
            txtRepYear = New TextBox() With {.Text = "1405", .Location = New Point(920, 12), .Size = New Size(70, 26)}
            cmbRepMonth = New ComboBox() With {.Location = New Point(790, 12), .Size = New Size(80, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            For i As Integer = 1 To 12
                cmbRepMonth.Items.Add(i.ToString())
            Next
            cmbRepMonth.SelectedIndex = 0

            btnLoadMonthlyRep = New Button() With {
                .Text = "📊 گزارش ماهانه حقوق",
                .Size = New Size(160, 34),
                .Location = New Point(615, 10),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadMonthlyRep.Click, Sub()
                                                    dgvReport.DataSource = _payrollSvc.GetMonthlyPayrollReport(txtRepYear.Text, Convert.ToInt32(cmbRepMonth.SelectedItem))
                                                End Sub

            cmbPersonnelRep = New ComboBox() With {.Location = New Point(380, 12), .Size = New Size(220, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            btnLoadPersonRep = New Button() With {
                .Text = "👤 سوابق فردی پرسنل",
                .Size = New Size(160, 34),
                .Location = New Point(205, 10),
                .BackColor = Color.FromArgb(13, 71, 161),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnLoadPersonRep.Click, AddressOf BtnLoadPersonRep_Click

            pnlTop.Controls.AddRange(New Control() {txtRepYear, cmbRepMonth, btnLoadMonthlyRep, cmbPersonnelRep, btnLoadPersonRep})

            dgvReport = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AllowUserToAddRows = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .RowHeadersVisible = False,
                .BackgroundColor = Color.White
            }
            AddHandler dgvReport.DataBindingComplete, Sub(s, e) ApplyPersianGridHeaders(CType(s, DataGridView))

            tabReports.Controls.Add(dgvReport)
            tabReports.Controls.Add(pnlTop)
        End Sub

        Private Sub FillPersonnelCombo()
            cmbPersonnelRep.Items.Clear()
            Dim dt = _payrollSvc.GetPersonnelList()
            If dt IsNot Nothing Then
                For Each r As DataRow In dt.Rows
                    cmbPersonnelRep.Items.Add(New KeyValuePair(Of Integer, String)(Convert.ToInt32(r("PersonnelID")), Convert.ToString(r("FullName"))))
                Next
            End If
            If cmbPersonnelRep.Items.Count > 0 Then cmbPersonnelRep.SelectedIndex = 0
        End Sub

        Private Sub BtnLoadPersonRep_Click(sender As Object, e As EventArgs)
            If cmbPersonnelRep.SelectedItem IsNot Nothing Then
                Dim kvp = CType(cmbPersonnelRep.SelectedItem, KeyValuePair(Of Integer, String))
                dgvReport.DataSource = _payrollSvc.GetEmployeeHistoricalReport(kvp.Key, txtRepYear.Text)
            End If
        End Sub

        Private Sub ApplyPersianGridHeaders(dgv As DataGridView)
            If dgv Is Nothing OrElse dgv.Columns Is Nothing Then Return
            
            For Each col As DataGridViewColumn In dgv.Columns
                Select Case col.Name
                    Case "PersonnelID": col.HeaderText = "کد پرسنلی" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "FullName": col.HeaderText = "نام و نام خانوادگی"
                    Case "NationalCode": col.HeaderText = "کد ملی" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "InsuranceNumber": col.HeaderText = "شماره بیمه" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "BankAccountNumber": col.HeaderText = "شماره حساب"
                    Case "Iban": col.HeaderText = "شماره شبا"
                    Case "ContractType": col.HeaderText = "نوع قرارداد" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "MaritalStatus": col.HeaderText = "وضعیت تأهل" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "ChildCount": col.HeaderText = "تعداد فرزند" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "BaseSalary": col.HeaderText = "حقوق پایه (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "HousingAllowance": col.HeaderText = "حق مسکن (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "FoodAllowance": col.HeaderText = "بن کارگری (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "ChildAllowance": col.HeaderText = "حق اولاد (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "SeniorityAllowance": col.HeaderText = "پایه سنوات (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "ManagementAllowance": col.HeaderText = "فوق‌العاده مدیریت (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "IsActive": col.HeaderText = "وضعیت فعال" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "WorkDays": col.HeaderText = "روزهای کارکرد" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "OvertimeHours": col.HeaderText = "ساعات اضافه کاری" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "NightShiftHours": col.HeaderText = "ساعات شب‌کاری" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "LeaveDays": col.HeaderText = "روزهای مرخصی" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "AbsenceDays": col.HeaderText = "روزهای غیبت" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "AdvancePayment": col.HeaderText = "مساعده دریافتی (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "LoanDeduction": col.HeaderText = "قسط وام (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "SalMaly": col.HeaderText = "سال مالی" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "MahMaly": col.HeaderText = "ماه مالی" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "GrossSalary": col.HeaderText = "ناخالص حقوق (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "OvertimeAmount": col.HeaderText = "مبلغ اضافه کاری (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "NightShiftAmount": col.HeaderText = "مبلغ شب‌کاری (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "TotalBenefits": col.HeaderText = "مجموع مزایا (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "EmployeeInsurance": col.HeaderText = "بیمه سهم کارمند ۷٪ (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "EmployerInsurance": col.HeaderText = "بیمه سهم کارفرما ۲۰٪ (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "UnemploymentInsurance": col.HeaderText = "بیمه بیکاری ۳٪ (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "TaxAmount": col.HeaderText = "مالیات حقوق (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "TotalDeductions": col.HeaderText = "جمع کسورات (ریال)" : col.DefaultCellStyle.Format = "N0"
                    Case "NetSalary": col.HeaderText = "خالص قابل پرداخت (ریال)" : col.DefaultCellStyle.Format = "N0" : col.DefaultCellStyle.ForeColor = Color.FromArgb(13, 71, 161) : col.DefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                    Case "CalcDate": col.HeaderText = "تاریخ محاسبه" : col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Case "AttendanceID", "CalculationID", "SanadEntryID": col.Visible = False
                End Select
            Next
        End Sub

    End Class
End Namespace
