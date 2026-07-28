Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class HesabdaryBalanceSheetReportControl
        Inherits UserControl

        Private pnlTop As FlowLayoutPanel
        Private dgvReport As DataGridView
        Private _settingsSvc As New SettingsService()
        Private _isUpdatingChecks As Boolean = False
        Private _suppressDateChange As Boolean = False

        ' GroupBoxes
        Private grpLevels As GroupBox
        Private grpRange As GroupBox
        Private grpActions As GroupBox

        ' Level CheckBoxes
        Private chkGroup As CheckBox
        Private chkKol As CheckBox
        Private chkMoein As CheckBox
        Private chkTaf1 As CheckBox
        Private chkTaf2 As CheckBox

        ' Range Controls
        Private cmbRangeMethod As ComboBox
        Private lblRangeValue As Label
        Private txtRangeValue As TextBox
        Private btnCalendar As Button

        ' Buttons
        Private btnSetup As Button
        Private btnCalculate As Button
        Private btnPrint As Button
        Private btnExcel As Button
        Private btnEdit As Button
        Private btnSave As Button

        Public Sub New()
            InitializeComponent()
            LoadSettings()
            SetEditMode(False)
            ApplySecurity()
        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            btnPrint.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetPrint) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
            btnExcel.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetExport) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
            If btnSave IsNot Nothing Then btnSave.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetSaveSettings) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
            If btnEdit IsNot Nothing Then btnEdit.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetEditSettings) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
            If btnSetup IsNot Nothing Then btnSetup.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetMapAccounts) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
            If btnCalculate IsNot Nothing Then btnCalculate.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheetCalculate) OrElse SessionContext.HasPermission(PermissionKeys.AccountingBalanceSheet)
        End Sub

        Private Sub InitializeComponent()
            Me.Dock = DockStyle.Fill
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes

            ' Top panel (FlowLayoutPanel for responsive wrapping of GroupBoxes)
            pnlTop = New FlowLayoutPanel()
            pnlTop.Dock = DockStyle.Top
            pnlTop.Height = 115
            pnlTop.BackColor = Color.FromArgb(240, 240, 240)
            pnlTop.FlowDirection = FlowDirection.RightToLeft
            pnlTop.Padding = New Padding(5, 5, 5, 5)

            ' 1. Levels GroupBox
            grpLevels = New GroupBox()
            grpLevels.Text = "انتخاب سطح ترازنامه"
            grpLevels.Size = New Size(345, 92)
            grpLevels.RightToLeft = RightToLeft.Yes

            chkGroup = New CheckBox() With {.Text = "گروه", .Location = New Point(280, 23), .Width = 55}
            chkKol = New CheckBox() With {.Text = "کل", .Location = New Point(230, 23), .Width = 45}
            chkMoein = New CheckBox() With {.Text = "معین", .Location = New Point(165, 23), .Width = 60}
            chkTaf1 = New CheckBox() With {.Text = "تفصیلی ۱", .Location = New Point(80, 23), .Width = 80}
            chkTaf2 = New CheckBox() With {.Text = "تفصیلی ۲", .Location = New Point(80, 55), .Width = 80}

            AddHandler chkGroup.CheckedChanged, AddressOf chkLevel_CheckedChanged
            AddHandler chkKol.CheckedChanged, AddressOf chkLevel_CheckedChanged
            AddHandler chkMoein.CheckedChanged, AddressOf chkLevel_CheckedChanged
            AddHandler chkTaf1.CheckedChanged, AddressOf chkLevel_CheckedChanged
            AddHandler chkTaf2.CheckedChanged, AddressOf chkLevel_CheckedChanged

            grpLevels.Controls.Add(chkGroup)
            grpLevels.Controls.Add(chkKol)
            grpLevels.Controls.Add(chkMoein)
            grpLevels.Controls.Add(chkTaf1)
            grpLevels.Controls.Add(chkTaf2)

            ' 2. Range GroupBox
            grpRange = New GroupBox()
            grpRange.Text = "تنظیم بازه گزارش"
            grpRange.Size = New Size(345, 92)
            grpRange.RightToLeft = RightToLeft.Yes

            cmbRangeMethod = New ComboBox()
            cmbRangeMethod.DropDownStyle = ComboBoxStyle.DropDownList
            cmbRangeMethod.Width = 325
            cmbRangeMethod.Location = New Point(10, 23)
            cmbRangeMethod.Items.Add("بر اساس شماره سند در سال جاری")
            cmbRangeMethod.Items.Add("بر اساس تاریخ")
            cmbRangeMethod.Items.Add("بر اساس تمام اسناد در تمام سالهای مالی")
            cmbRangeMethod.SelectedIndex = 1 ' Default to date mode
            AddHandler cmbRangeMethod.SelectedIndexChanged, AddressOf cmbRangeMethod_SelectedIndexChanged

            lblRangeValue = New Label()
            lblRangeValue.Text = "تا سند شماره:"
            lblRangeValue.Location = New Point(245, 58)
            lblRangeValue.Width = 90
            lblRangeValue.TextAlign = ContentAlignment.MiddleLeft

            txtRangeValue = New TextBox()
            txtRangeValue.Width = 220
            txtRangeValue.Location = New Point(10, 55)
            AddHandler txtRangeValue.KeyPress, AddressOf NumericOnly_KeyPress
            AddHandler txtRangeValue.TextChanged, AddressOf txtRangeValue_TextChanged

            btnCalendar = New Button()
            btnCalendar.Text = "📅"
            btnCalendar.Size = New Size(30, 24)
            btnCalendar.Location = New Point(10, 54)
            btnCalendar.FlatStyle = FlatStyle.Flat
            btnCalendar.BackColor = Color.White
            btnCalendar.Visible = False
            AddHandler btnCalendar.Click, AddressOf btnCalendar_Click

            grpRange.Controls.Add(cmbRangeMethod)
            grpRange.Controls.Add(lblRangeValue)
            grpRange.Controls.Add(txtRangeValue)
            grpRange.Controls.Add(btnCalendar)

            ' 3. Actions GroupBox
            grpActions = New GroupBox()
            grpActions.Text = "عملیات گزارش"
            grpActions.Size = New Size(490, 92)
            grpActions.RightToLeft = RightToLeft.Yes

            ' Row 1 Buttons
            btnCalculate = New Button()
            btnCalculate.Text = "محاسبه و نمایش"
            btnCalculate.Size = New Size(150, 28)
            btnCalculate.Location = New Point(330, 21)
            btnCalculate.BackColor = Color.FromArgb(46, 204, 113)
            btnCalculate.ForeColor = Color.White
            btnCalculate.FlatStyle = FlatStyle.Flat
            AddHandler btnCalculate.Click, AddressOf btnCalculate_Click

            btnSetup = New Button()
            btnSetup.Text = "معرفی حساب‌ها"
            btnSetup.Size = New Size(150, 28)
            btnSetup.Location = New Point(170, 21)
            btnSetup.BackColor = Color.SteelBlue
            btnSetup.ForeColor = Color.White
            btnSetup.FlatStyle = FlatStyle.Flat
            AddHandler btnSetup.Click, AddressOf btnSetup_Click

            btnEdit = New Button()
            btnEdit.Text = "ویرایش تنظیمات"
            btnEdit.Size = New Size(150, 28)
            btnEdit.Location = New Point(10, 21)
            btnEdit.BackColor = Color.FromArgb(241, 196, 15)
            btnEdit.ForeColor = Color.Black
            btnEdit.FlatStyle = FlatStyle.Flat
            AddHandler btnEdit.Click, AddressOf btnEdit_Click

            ' Row 2 Buttons
            btnPrint = New Button()
            btnPrint.Text = "نمایش و چاپ"
            btnPrint.Size = New Size(150, 28)
            btnPrint.Location = New Point(330, 55)
            btnPrint.BackColor = Color.FromArgb(52, 152, 219)
            btnPrint.ForeColor = Color.White
            btnPrint.FlatStyle = FlatStyle.Flat
            AddHandler btnPrint.Click, AddressOf btnPrint_Click

            btnExcel = New Button()
            btnExcel.Text = "خروجی اکسل"
            btnExcel.Size = New Size(150, 28)
            btnExcel.Location = New Point(170, 55)
            btnExcel.BackColor = Color.FromArgb(39, 174, 96)
            btnExcel.ForeColor = Color.White
            btnExcel.FlatStyle = FlatStyle.Flat
            AddHandler btnExcel.Click, AddressOf btnExcel_Click

            btnSave = New Button()
            btnSave.Text = "ذخیره تنظیمات"
            btnSave.Size = New Size(150, 28)
            btnSave.Location = New Point(10, 55)
            btnSave.BackColor = Color.FromArgb(142, 68, 173)
            btnSave.ForeColor = Color.White
            btnSave.FlatStyle = FlatStyle.Flat
            AddHandler btnSave.Click, AddressOf btnSave_Click

            grpActions.Controls.Add(btnCalculate)
            grpActions.Controls.Add(btnSetup)
            grpActions.Controls.Add(btnEdit)
            grpActions.Controls.Add(btnPrint)
            grpActions.Controls.Add(btnExcel)
            grpActions.Controls.Add(btnSave)

            ' Add to Top Panel
            pnlTop.Controls.Add(grpLevels)
            pnlTop.Controls.Add(grpRange)
            pnlTop.Controls.Add(grpActions)

            ' Grid View
            dgvReport = New DataGridView()
            dgvReport.Dock = DockStyle.Fill
            dgvReport.AllowUserToAddRows = False
            dgvReport.AllowUserToDeleteRows = False
            dgvReport.ReadOnly = True
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvReport.BackgroundColor = Color.White
            dgvReport.BorderStyle = BorderStyle.None
            dgvReport.RowTemplate.Height = 35
            
            dgvReport.Columns.Add("Title", "شرح ترازنامه")
            dgvReport.Columns.Add("Amount", "مبلغ (ریال)")
            dgvReport.Columns(1).DefaultCellStyle.Format = "N0"
            dgvReport.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Me.Controls.Add(dgvReport)
            Me.Controls.Add(pnlTop)
        End Sub

        Private Sub LoadSettings()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Dim cid = SessionContext.CurrentCompanyID.Value

            Try
                Dim rangeMethodStr = _settingsSvc.GetSettingValue($"Company_{cid}_BS_RangeMethod", "0")
                Dim rangeValueStr = _settingsSvc.GetSettingValue($"Company_{cid}_BS_RangeValue", "")
                
                Dim gStr = _settingsSvc.GetSettingValue($"Company_{cid}_BS_LGroup", "False")
                Dim kStr = _settingsSvc.GetSettingValue($"Company_{cid}_BS_LKol", "False")
                Dim mStr = _settingsSvc.GetSettingValue($"Company_{cid}_BS_LMoein", "False")
                Dim t1Str = _settingsSvc.GetSettingValue($"Company_{cid}_BS_LTaf1", "False")
                Dim t2Str = _settingsSvc.GetSettingValue($"Company_{cid}_BS_LTaf2", "False")

                Dim idxRange = 0
                Integer.TryParse(rangeMethodStr, idxRange)
                cmbRangeMethod.SelectedIndex = idxRange
                txtRangeValue.Text = rangeValueStr

                _isUpdatingChecks = True
                chkGroup.Checked = Convert.ToBoolean(gStr)
                chkKol.Checked = Convert.ToBoolean(kStr)
                chkMoein.Checked = Convert.ToBoolean(mStr)
                chkTaf1.Checked = Convert.ToBoolean(t1Str)
                chkTaf2.Checked = Convert.ToBoolean(t2Str)
                _isUpdatingChecks = False

                UpdateLevelCheckboxes()
            Catch
            End Try
        End Sub



        Private Sub SetEditMode(isEditMode As Boolean)
            cmbRangeMethod.Enabled = True
            txtRangeValue.Enabled = True

            btnSetup.Enabled = isEditMode
            btnSave.Enabled = isEditMode
            btnEdit.Enabled = Not isEditMode
            
            btnCalculate.Enabled = True
            btnPrint.Enabled = True
            btnExcel.Enabled = True

            UpdateLevelCheckboxes()
        End Sub

        Private Sub btnEdit_Click(sender As Object, e As EventArgs)
            SetEditMode(True)
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Dim cid = SessionContext.CurrentCompanyID.Value

            Try
                _settingsSvc.SaveSetting($"Company_{cid}_BS_RangeMethod", cmbRangeMethod.SelectedIndex.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_RangeValue", txtRangeValue.Text, "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LGroup", chkGroup.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LKol", chkKol.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LMoein", chkMoein.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LTaf1", chkTaf1.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LTaf2", chkTaf2.Checked.ToString(), "BS")

                MessageBox.Show("تنظیمات ترازنامه با موفقیت ذخیره شدند.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SetEditMode(False)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub cmbRangeMethod_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim isDoc = (cmbRangeMethod.SelectedIndex = 0)
            Dim isDate = (cmbRangeMethod.SelectedIndex = 1)
            Dim isAll = (cmbRangeMethod.SelectedIndex = 2)

            lblRangeValue.Visible = Not isAll
            txtRangeValue.Visible = Not isAll
            btnCalendar.Visible = isDate

            If isDoc Then
                lblRangeValue.Text = "تا سند شماره:"
                txtRangeValue.Location = New Point(10, 55)
                txtRangeValue.Width = 220
                txtRangeValue.Text = ""
            ElseIf isDate Then
                lblRangeValue.Text = "تا تاریخ:"
                txtRangeValue.Location = New Point(45, 55)
                txtRangeValue.Width = 185
                If String.IsNullOrWhiteSpace(txtRangeValue.Text) OrElse Not txtRangeValue.Text.Contains("/") Then
                    txtRangeValue.Text = PersianDateHelper.ToPersian(DateTime.Today)
                End If
            ElseIf isAll Then
                txtRangeValue.Text = ""
            End If
        End Sub

        Private Sub txtRangeValue_TextChanged(sender As Object, e As EventArgs)
            If cmbRangeMethod.SelectedIndex = 1 Then
                FormatDateTextBox(txtRangeValue)
            End If
        End Sub

        Private Sub FormatDateTextBox(txtBox As TextBox)
            If _suppressDateChange Then Return
            Dim txt = txtBox.Text
            
            Dim sb As New System.Text.StringBuilder()
            For Each c As Char In txt
                If Char.IsDigit(c) Then sb.Append(c)
            Next
            Dim digits = sb.ToString()
            
            If digits.Length > 8 Then digits = digits.Substring(0, 8)
            Dim formatted = FormatPersianDigits(digits)
            If formatted = txt Then Return
            _suppressDateChange = True
            txtBox.Text = formatted
            txtBox.SelectionStart = formatted.Length
            _suppressDateChange = False
        End Sub

        Private Shared Function FormatPersianDigits(digits As String) As String
            Select Case digits.Length
                Case <= 4 : Return digits
                Case <= 6 : Return digits.Substring(0, 4) & "/" & digits.Substring(4)
                Case Else : Return digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6)
            End Select
        End Function

        Private Sub btnCalendar_Click(sender As Object, e As EventArgs)
            ShowCalendarForTextBox(txtRangeValue)
        End Sub

        Private Sub ShowCalendarForTextBox(txtBox As TextBox)
            Dim anchor = EnsureOnScreen(
                txtBox.PointToScreen(New Point(0, txtBox.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtBox.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtBox.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, waSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - waSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - waSize.Height)))
        End Function

        Private Sub chkLevel_CheckedChanged(sender As Object, e As EventArgs)
            If _isUpdatingChecks Then Return
            _isUpdatingChecks = True
            UpdateLevelCheckboxes()
            _isUpdatingChecks = False
        End Sub

        Private Sub UpdateLevelCheckboxes()
            Dim chkLevels As CheckBox() = {chkGroup, chkKol, chkMoein, chkTaf1, chkTaf2}
            Dim checkedIndices As New List(Of Integer)()
            For i As Integer = 0 To 4
                If chkLevels(i).Checked Then
                    checkedIndices.Add(i)
                End If
            Next


            If checkedIndices.Count = 0 Then
                ' Nothing checked: all are checkable
                For i As Integer = 0 To 4
                    chkLevels(i).Enabled = True
                Next
            Else
                Dim minChecked = checkedIndices(0)
                Dim maxChecked = checkedIndices(checkedIndices.Count - 1)

                For i As Integer = 0 To 4
                    If chkLevels(i).Checked Then
                        chkLevels(i).Enabled = True
                    Else
                        ' Enabled if immediately above or below the checked range
                        chkLevels(i).Enabled = (i = minChecked - 1) OrElse (i = maxChecked + 1)
                    End If
                Next
            End If
        End Sub

        Private Sub NumericOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
            If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub btnSetup_Click(sender As Object, e As EventArgs)
            Using frm As New HesabdaryBalanceSheetMappingForm()
                frm.ShowDialog()
            End Using
        End Sub

        ' Data structure for dynamic detailed rows
        Private Class BSAccountNode
            Public AccountID As Integer
            Public AccountCode As String
            Public AccountName As String
            Public ParentID As Integer
            Public Level As Integer
            Public Balance As Decimal
        End Class

        Private Sub btnCalculate_Click(sender As Object, e As EventArgs)
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return

            ' Save the settings before running the report automatically
            Try
                Dim cid = SessionContext.CurrentCompanyID.Value
                _settingsSvc.SaveSetting($"Company_{cid}_BS_RangeMethod", cmbRangeMethod.SelectedIndex.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_RangeValue", txtRangeValue.Text, "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LGroup", chkGroup.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LKol", chkKol.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LMoein", chkMoein.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LTaf1", chkTaf1.Checked.ToString(), "BS")
                _settingsSvc.SaveSetting($"Company_{cid}_BS_LTaf2", chkTaf2.Checked.ToString(), "BS")
            Catch
            End Try
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fiscalYearId = SessionContext.CurrentFiscalYearID.Value

            ' 1. Calculate ending inventory and dynamic net profit
            Dim endingInv = GetEndingInventoryValue()
            Dim currentProfit = GetNetProfit()

            ' 2. Determine which levels are active
            Dim showGroup = chkGroup.Checked
            Dim showKol = chkKol.Checked
            Dim showMoein = chkMoein.Checked
            Dim showTaf1 = chkTaf1.Checked
            Dim showTaf2 = chkTaf2.Checked
            Dim hasLevels = showGroup OrElse showKol OrElse showMoein OrElse showTaf1 OrElse showTaf2

            ' 3. Calculate all account balances
            Dim allAccounts = LoadAllAccountBalances(companyId, fiscalYearId)

            ' 4. Sum assets and liabilities/equity
            Dim totalCurrAssets = SumCategory(allAccounts, "CURR_ASSETS", False) + endingInv
            Dim totalNonCurrAssets = SumCategory(allAccounts, "NON_CURR_ASSETS", False)
            Dim totalAssets = totalCurrAssets + totalNonCurrAssets

            Dim totalCurrLiabilities = SumCategory(allAccounts, "CURR_LIABILITIES", True)
            Dim totalNonCurrLiabilities = SumCategory(allAccounts, "NON_CURR_LIABILITIES", True)
            Dim totalLiabilities = totalCurrLiabilities + totalNonCurrLiabilities

            Dim capital = SumCategory(allAccounts, "EQUITY_CAPITAL", True)
            Dim reserves = SumCategory(allAccounts, "EQUITY_RESERVES", True)
            Dim totalEquity = capital + reserves + currentProfit

            Dim totalLiabilitiesAndEquity = totalLiabilities + totalEquity
            Dim diff = totalAssets - totalLiabilitiesAndEquity

            ' Display in Grid
            dgvReport.Rows.Clear()

            ' --- ASSETS SECTION ---
            AddReportRow("۱. دارایی‌ها", 0, True)
            
            ' Detailed Current Assets if levels selected
            If hasLevels Then
                AddDetailedAccounts(allAccounts, "CURR_ASSETS", False)
            Else
                AddReportRow("   دارایی‌های جاری", totalCurrAssets - endingInv, False)
            End If
            AddReportRow("   موجودی کالای پایان دوره", endingInv, False)
            AddReportRow("جمع دارایی‌های جاری", totalCurrAssets, True)

            If hasLevels Then
                AddDetailedAccounts(allAccounts, "NON_CURR_ASSETS", False)
            Else
                AddReportRow("   دارایی‌های غیرجاری (ثابت)", totalNonCurrAssets, False)
            End If
            AddReportRow("جمع کل دارایی‌ها", totalAssets, True)

            ' Separator
            AddReportRow("----------------------------------------------------------------", 0, False)

            ' --- LIABILITIES & EQUITY SECTION ---
            AddReportRow("۲. بدهی‌ها و حقوق صاحبان سهام", 0, True)
            
            If hasLevels Then
                AddDetailedAccounts(allAccounts, "CURR_LIABILITIES", True)
            Else
                AddReportRow("   بدهی‌های جاری", totalCurrLiabilities, False)
            End If
            
            If hasLevels Then
                AddDetailedAccounts(allAccounts, "NON_CURR_LIABILITIES", True)
            Else
                AddReportRow("   بدهی‌های غیرجاری", totalNonCurrLiabilities, False)
            End If
            AddReportRow("جمع کل بدهی‌ها", totalLiabilities, True)

            If hasLevels Then
                AddDetailedAccounts(allAccounts, "EQUITY_CAPITAL", True)
                AddDetailedAccounts(allAccounts, "EQUITY_RESERVES", True)
            Else
                AddReportRow("   سرمایه ثبت شده", capital, False)
                AddReportRow("   اندوخته‌ها و سود/زیان انباشته", reserves, False)
            End If
            AddReportRow("   سود (زیان) جاری دوره مالی", currentProfit, False)
            AddReportRow("جمع حقوق صاحبان سهام", totalEquity, True)

            AddReportRow("جمع کل بدهی‌ها و حقوق صاحبان سهام", totalLiabilitiesAndEquity, True)

            ' Separator
            AddReportRow("----------------------------------------------------------------", 0, False)

            ' --- BALANCE VALIDATION ROW ---
            Dim validationTitle = "وضعیت تراز (اختلاف دارایی با بدهی و سرمایه)"
            Dim validationRowIdx = dgvReport.Rows.Add(validationTitle, diff)
            Dim valRow = dgvReport.Rows(validationRowIdx)
            valRow.DefaultCellStyle.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            If diff = 0D Then
                valRow.DefaultCellStyle.BackColor = Color.FromArgb(200, 247, 197) ' Light green
                valRow.Cells(1).Value = "تراز (0)"
            Else
                valRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210) ' Light red
            End If
        End Sub

        ' Load all accounts and compute their hierarchical level and balances under filters
        Private Function LoadAllAccountBalances(companyId As Integer, fiscalYearId As Integer) As List(Of BSAccountNode)
            Dim nodesList As New List(Of BSAccountNode)()
            
            Try
                ' 1. Fetch SarfaslHesab
                Dim dtAccs = Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountName, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", companyId)
                Dim parentMap As New Dictionary(Of Integer, Integer)()
                Dim allNodes As New Dictionary(Of Integer, BSAccountNode)()

                For Each row As DataRow In dtAccs.Rows
                    Dim id = Convert.ToInt32(row("AccountID"))
                    Dim parentId = If(Convert.IsDBNull(row("ParentAccountID")), 0, Convert.ToInt32(row("ParentAccountID")))
                    parentMap(id) = parentId

                    Dim node As New BSAccountNode() With {
                        .AccountID = id,
                        .AccountCode = Convert.ToString(row("AccountCode")),
                        .AccountName = Convert.ToString(row("AccountName")),
                        .ParentID = parentId,
                        .Balance = 0D
                    }
                    allNodes(id) = node
                Next

                ' Compute level recursively
                For Each kvp In allNodes
                    Dim lvl = 1
                    Dim curr = kvp.Value.ParentID
                    Dim guard = 0
                    Do While curr > 0 AndAlso parentMap.ContainsKey(curr) AndAlso guard < 50
                        guard += 1
                        curr = parentMap(curr)
                        lvl += 1
                    Loop
                    kvp.Value.Level = lvl
                Next

                ' Build parent-child mappings
                Dim parentToChildren As New Dictionary(Of Integer, List(Of Integer))()
                For Each kvp In allNodes
                    Dim pid = kvp.Value.ParentID
                    If pid > 0 Then
                        If Not parentToChildren.ContainsKey(pid) Then parentToChildren(pid) = New List(Of Integer)()
                        parentToChildren(pid).Add(kvp.Value.AccountID)
                    End If
                Next

                ' Helper to retrieve all descendant IDs recursively
                Dim getDescendants As Func(Of Integer, List(Of Integer)) = Nothing
                getDescendants = Function(parentId As Integer)
                                     Dim list As New List(Of Integer)()
                                     list.Add(parentId)
                                     If parentToChildren.ContainsKey(parentId) Then
                                         For Each childId In parentToChildren(parentId)
                                             list.AddRange(getDescendants(childId))
                                         Next
                                     End If
                                     Return list
                                 End Function

                ' 2. Fetch raw account postings under active range filters
                Dim filters As New List(Of String)()
                Dim params As New List(Of Object)()
                filters.Add("e.CompanyID = ?")
                params.Add(companyId)

                Dim useFiscalYearFilter As Boolean = (cmbRangeMethod.SelectedIndex = 0)
                If useFiscalYearFilter Then
                    filters.Add("e.FiscalYearID = ?")
                    params.Add(fiscalYearId)
                End If

                If cmbRangeMethod.SelectedIndex = 0 Then
                    Dim docNum = 0
                    If Integer.TryParse(txtRangeValue.Text, docNum) Then
                        filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                        params.Add(docNum)
                    End If
                ElseIf cmbRangeMethod.SelectedIndex = 1 Then
                    If Not String.IsNullOrWhiteSpace(txtRangeValue.Text) Then
                        Try
                            Dim toDate = PersianDateHelper.ParsePersianDate(txtRangeValue.Text)
                            filters.Add("e.EntryDate <= ?")
                            params.Add(toDate)
                        Catch
                        End Try
                    End If
                End If

                Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())
                Dim query = "SELECT d.AccountID, SUM(IFNULL(d.DebitAmount, 0) - IFNULL(d.CreditAmount, 0)) AS NetDebit " &
                            "FROM Sanad2 d " &
                            "INNER JOIN Sanad1 e ON d.EntryID = e.EntryID " &
                            whereClause & " GROUP BY d.AccountID"

                Dim dtRaw = Sql.ExecuteTable(query, params.ToArray())
                Dim rawBalances As New Dictionary(Of Integer, Decimal)()
                For Each row As DataRow In dtRaw.Rows
                    rawBalances(Convert.ToInt32(row("AccountID"))) = Convert.ToDecimal(row("NetDebit"))
                Next

                ' Compute full recursive balance for each node
                For Each kvp In allNodes
                    Dim decs = getDescendants(kvp.Value.AccountID)
                    Dim sumVal = 0D
                    For Each dId In decs
                        If rawBalances.ContainsKey(dId) Then sumVal += rawBalances(dId)
                    Next
                    kvp.Value.Balance = sumVal
                    nodesList.Add(kvp.Value)
                Next

            Catch
            End Try

            Return nodesList
        End Function

        ' Check if account belongs to a mapped BS Category (checks itself or any parent)
        Private Function IsAccountInCategory(node As BSAccountNode, categoryKey As String, allNodes As List(Of BSAccountNode)) As Boolean
            If Not SessionContext.CurrentCompanyID.HasValue Then Return False
            Dim companyId = SessionContext.CurrentCompanyID.Value

            ' Cache mapped account IDs
            Dim dtMapped = Sql.ExecuteTable("SELECT AccountID FROM BalanceSheetAccountMappings WHERE CompanyID = ? AND CategoryKey = ?", companyId, categoryKey)
            Dim mappedSet As New HashSet(Of Integer)()
            For Each row As DataRow In dtMapped.Rows
                mappedSet.Add(Convert.ToInt32(row("AccountID")))
            Next

            If mappedSet.Count = 0 Then Return False

            ' Build parent lookup
            Dim parentMap As New Dictionary(Of Integer, Integer)()
            For Each n In allNodes
                parentMap(n.AccountID) = n.ParentID
            Next

            Dim curr = node.AccountID
            Dim guard = 0
            Do While curr > 0 AndAlso guard < 100
                guard += 1
                If mappedSet.Contains(curr) Then Return True
                If parentMap.ContainsKey(curr) Then
                    curr = parentMap(curr)
                Else
                    curr = 0
                End If
            Loop
            Return False
        End Function

        ' Sum total balance for a category
        Private Function SumCategory(allAccounts As List(Of BSAccountNode), categoryKey As String, forceCredit As Boolean) As Decimal
            If Not SessionContext.CurrentCompanyID.HasValue Then Return 0D
            Dim companyId = SessionContext.CurrentCompanyID.Value

            ' Mapped accounts
            Dim dtMapped = Sql.ExecuteTable("SELECT AccountID FROM BalanceSheetAccountMappings WHERE CompanyID = ? AND CategoryKey = ?", companyId, categoryKey)
            Dim mappedIds As New List(Of Integer)()
            For Each row As DataRow In dtMapped.Rows
                mappedIds.Add(Convert.ToInt32(row("AccountID")))
            Next

            If mappedIds.Count = 0 Then Return 0D

            Dim sumDebit = 0D
            Dim mappedSet As New HashSet(Of Integer)(mappedIds)

            ' We only sum the root-mapped accounts (since their balances already include descendants recursively)
            For Each n In allAccounts
                If mappedSet.Contains(n.AccountID) Then
                    sumDebit += n.Balance
                End If
            Next

            Return If(forceCredit, -sumDebit, sumDebit)
        End Function

        ' Add detailed rows to grid based on Checked Level filters
        Private Sub AddDetailedAccounts(allAccounts As List(Of BSAccountNode), categoryKey As String, forceCredit As Boolean)
            Dim activeLevels As New List(Of Integer)()
            If chkGroup.Checked Then activeLevels.Add(1)
            If chkKol.Checked Then activeLevels.Add(2)
            If chkMoein.Checked Then activeLevels.Add(3)
            If chkTaf1.Checked Then activeLevels.Add(4)
            If chkTaf2.Checked Then activeLevels.Add(5)

            ' Filter accounts in this category
            Dim categoryNodes As New List(Of BSAccountNode)()
            For Each n In allAccounts
                If IsAccountInCategory(n, categoryKey, allAccounts) Then
                    categoryNodes.Add(n)
                End If
            Next

            ' Sort by account code
            categoryNodes.Sort(Function(x, y) x.AccountCode.CompareTo(y.AccountCode))

            ' Add matching level rows to Grid
            For Each n In categoryNodes
                If activeLevels.Contains(n.Level) Then
                    Dim balanceVal = If(forceCredit, -n.Balance, n.Balance)
                    ' Indent spaces based on level
                    Dim spaces = New String(" "c, (n.Level - 1) * 3)
                    Dim title = spaces & n.AccountCode & " - " & n.AccountName
                    
                    Dim isBold = (n.Level = 1)
                    AddReportRow(title, balanceVal, isBold)
                End If
            Next
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs)
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("ابتدا ترازنامه را محاسبه و نمایش دهید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim companyName = If(SessionContext.CurrentCompanyName, "مؤسسه")
            Dim dateTitle = "گزارش ترازنامه مالی"

            Dim printCols As New List(Of HesabdaryTarazPrintForm.PrintColumnInfo)()
            printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "AccountName", .Title = "شرح ترازنامه", .WidthRatio = 4.0F})
            printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "Amount", .Title = "مبلغ (ریال)", .WidthRatio = 2.0F})

            Dim printRows As New List(Of HesabdaryTarazPrintForm.PrintRowInfo)()
            For Each row As DataGridViewRow In dgvReport.Rows
                Dim title = Convert.ToString(row.Cells(0).Value)
                Dim amountObj = row.Cells(1).Value
                
                Dim isTotal = row.DefaultCellStyle.Font IsNot Nothing AndAlso row.DefaultCellStyle.Font.Bold
                
                ' Calculate level from leading spaces
                Dim level = 0
                Dim leadingSpaces = title.Length - title.TrimStart(" "c).Length
                level = leadingSpaces \ 3

                Dim rInfo As New HesabdaryTarazPrintForm.PrintRowInfo() With {
                    .AccountName = title.Trim(),
                    .IsHeader = isTotal,
                    .Level = level
                }
                
                If TypeOf amountObj Is Decimal Then
                    rInfo.Values("Amount") = Convert.ToDecimal(amountObj)
                Else
                    rInfo.Values("Amount") = 0D
                End If

                printRows.Add(rInfo)
            Next

            Dim totals As New Dictionary(Of String, Decimal)()

            Using printForm As New HesabdaryTarazPrintForm(companyName, dateTitle, printCols, printRows, totals, "گزارش ترازنامه مالی")
                printForm.ShowDialog(Me)
            End Using
        End Sub

        Private Sub btnExcel_Click(sender As Object, e As EventArgs)
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("ابتدا ترازنامه را محاسبه و نمایش دهید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            ExportGridToExcel(dgvReport, "Balance_Sheet_Report")
        End Sub

        Private Sub ExportGridToExcel(dgv As DataGridView, defaultFileName As String)
            Using sfd As New SaveFileDialog()
                sfd.Filter = "Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*"
                sfd.Title = "خروجی اکسل"
                sfd.FileName = defaultFileName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim sb As New System.Text.StringBuilder()
                        
                        Dim headers As New List(Of String)()
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                headers.Add(col.HeaderText)
                            End If
                        Next
                        sb.AppendLine(String.Join(",", headers))
                        
                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For
                            
                            Dim cells As New List(Of String)()
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                    Dim val = Convert.ToString(row.Cells(col.Index).Value)
                                    If val.Contains(",") OrElse val.Contains("""") OrElse val.Contains(Microsoft.VisualBasic.ControlChars.CrLf) OrElse val.Contains(Microsoft.VisualBasic.ControlChars.Lf) Then
                                        val = """" & val.Replace("""", """""") & """"
                                    End If
                                    cells.Add(val)
                                End If
                            Next
                            sb.AppendLine(String.Join(",", cells))
                        Next
                        
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8)
                        MessageBox.Show("خروجی اکسل با موفقیت ذخیره شد.", "عملیات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ذخیره فایل اکسل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Function GetBalance(categoryKey As String, forceCreditNature As Boolean) As Decimal
            Dim bal = GetCategoryBalance(categoryKey)
            If forceCreditNature Then
                Return -bal
            Else
                Return bal
            End If
        End Function

        Private Function GetCategoryBalance(categoryKey As String) As Decimal
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return 0D
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fiscalYearId = SessionContext.CurrentFiscalYearID.Value

            ' Get mapped accounts from BalanceSheetAccountMappings
            Dim dtMapped As DataTable = Sql.ExecuteTable("SELECT AccountID FROM BalanceSheetAccountMappings WHERE CompanyID = ? AND CategoryKey = ?", companyId, categoryKey)
            If dtMapped.Rows.Count = 0 Then Return 0D

            Dim mappedIds As New List(Of Integer)()
            For Each row As DataRow In dtMapped.Rows
                mappedIds.Add(Convert.ToInt32(row("AccountID")))
            Next

            ' Fetch hierarchy
            Dim dtAll As DataTable = Sql.ExecuteTable("SELECT AccountID, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", companyId)
            
            Dim parentToChildren As New Dictionary(Of Integer, List(Of Integer))()
            For Each row As DataRow In dtAll.Rows
                Dim accId = Convert.ToInt32(row("AccountID"))
                Dim parentId = If(Convert.IsDBNull(row("ParentAccountID")), 0, Convert.ToInt32(row("ParentAccountID")))
                If parentId > 0 Then
                    If Not parentToChildren.ContainsKey(parentId) Then
                        parentToChildren(parentId) = New List(Of Integer)()
                    End If
                    parentToChildren(parentId).Add(accId)
                End If
            Next

            Dim allIds As New HashSet(Of Integer)()
            Dim collectChildren As Action(Of Integer) = Nothing
            collectChildren = Sub(parentId As Integer)
                                  allIds.Add(parentId)
                                  If parentToChildren.ContainsKey(parentId) Then
                                      For Each childId In parentToChildren(parentId)
                                          collectChildren(childId)
                                      Next
                                  End If
                              End Sub

            For Each id In mappedIds
                collectChildren(id)
            Next

            If allIds.Count = 0 Then Return 0D

            ' Build dynamic query filters
            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)

            Dim useFiscalYearFilter As Boolean = (cmbRangeMethod.SelectedIndex = 0)
            If useFiscalYearFilter Then
                filters.Add("e.FiscalYearID = ?")
                params.Add(fiscalYearId)
            End If

            filters.Add("d.AccountID IN (" & String.Join(",", allIds) & ")")

            If cmbRangeMethod.SelectedIndex = 0 Then
                Dim docNum = 0
                If Integer.TryParse(txtRangeValue.Text, docNum) Then
                    filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                    params.Add(docNum)
                End If
            ElseIf cmbRangeMethod.SelectedIndex = 1 Then
                If Not String.IsNullOrWhiteSpace(txtRangeValue.Text) Then
                    Try
                        Dim toDate = PersianDateHelper.ParsePersianDate(txtRangeValue.Text)
                        filters.Add("e.EntryDate <= ?")
                        params.Add(toDate)
                    Catch
                    End Try
                End If
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())
            Dim query = "SELECT SUM(IFNULL(d.DebitAmount, 0) - IFNULL(d.CreditAmount, 0)) " &
                        "FROM Sanad2 d " &
                        "INNER JOIN Sanad1 e ON d.EntryID = e.EntryID " &
                        whereClause
            
            Dim balanceObj = Sql.ExecuteScalar(query, params.ToArray())
            If balanceObj Is Nothing OrElse Convert.IsDBNull(balanceObj) Then
                Return 0D
            Else
                Return Convert.ToDecimal(balanceObj)
            End If
        End Function

        Private Function GetEndingInventoryValue() As Decimal
            If Not SessionContext.CurrentCompanyID.HasValue Then Return 0D
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim settingsSvc As New SettingsService()

            Dim invMethodStr = settingsSvc.GetSettingValue($"Company_{companyId}_PnL_InvMethod", "0")
            Dim invManualStr = settingsSvc.GetSettingValue($"Company_{companyId}_PnL_InvManualValue", "0")

            Dim invEnd As Decimal = 0D
            If invMethodStr = "1" Then
                Decimal.TryParse(invManualStr, invEnd)
            Else
                Try
                    Dim query = "SELECT SUM(i.Quantity * i.AverageCost) " &
                                "FROM Inventory i " &
                                "INNER JOIN Products p ON i.ProductID = p.ProductID " &
                                "WHERE p.CompanyID = ?"
                    Dim obj = Sql.ExecuteScalar(query, companyId)
                    If obj IsNot Nothing AndAlso Not Convert.IsDBNull(obj) Then
                        invEnd = Convert.ToDecimal(obj)
                    End If
                Catch
                    invEnd = 0D
                End Try
            End If
            Return invEnd
        End Function

        Private Function GetPnLBalance(categoryKey As String, forceCreditNature As Boolean) As Decimal
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return 0D
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fiscalYearId = SessionContext.CurrentFiscalYearID.Value

            ' Get mapped accounts from PnLAccountMappings
            Dim dtMapped As DataTable = Sql.ExecuteTable("SELECT AccountID FROM PnLAccountMappings WHERE CompanyID = ? AND CategoryKey = ?", companyId, categoryKey)
            If dtMapped.Rows.Count = 0 Then Return 0D

            Dim mappedIds As New List(Of Integer)()
            For Each row As DataRow In dtMapped.Rows
                mappedIds.Add(Convert.ToInt32(row("AccountID")))
            Next

            ' Fetch hierarchy
            Dim dtAll As DataTable = Sql.ExecuteTable("SELECT AccountID, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", companyId)
            
            Dim parentToChildren As New Dictionary(Of Integer, List(Of Integer))()
            For Each row As DataRow In dtAll.Rows
                Dim accId = Convert.ToInt32(row("AccountID"))
                Dim parentId = If(Convert.IsDBNull(row("ParentAccountID")), 0, Convert.ToInt32(row("ParentAccountID")))
                If parentId > 0 Then
                    If Not parentToChildren.ContainsKey(parentId) Then
                        parentToChildren(parentId) = New List(Of Integer)()
                    End If
                    parentToChildren(parentId).Add(accId)
                End If
            Next

            Dim allIds As New HashSet(Of Integer)()
            Dim collectChildren As Action(Of Integer) = Nothing
            collectChildren = Sub(parentId As Integer)
                                  allIds.Add(parentId)
                                  If parentToChildren.ContainsKey(parentId) Then
                                      For Each childId In parentToChildren(parentId)
                                          collectChildren(childId)
                                      Next
                                  End If
                              End Sub

            For Each id In mappedIds
                collectChildren(id)
            Next

            If allIds.Count = 0 Then Return 0D

            ' Build PnL Dynamic Query Filters based on the active range in BS!
            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)

            Dim useFiscalYearFilter As Boolean = (cmbRangeMethod.SelectedIndex = 0)
            If useFiscalYearFilter Then
                filters.Add("e.FiscalYearID = ?")
                params.Add(fiscalYearId)
            End If

            filters.Add("d.AccountID IN (" & String.Join(",", allIds) & ")")

            If cmbRangeMethod.SelectedIndex = 0 Then
                Dim docNum = 0
                If Integer.TryParse(txtRangeValue.Text, docNum) Then
                    filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                    params.Add(docNum)
                End If
            ElseIf cmbRangeMethod.SelectedIndex = 1 Then
                If Not String.IsNullOrWhiteSpace(txtRangeValue.Text) Then
                    Try
                        Dim toDate = PersianDateHelper.ParsePersianDate(txtRangeValue.Text)
                        filters.Add("e.EntryDate <= ?")
                        params.Add(toDate)
                    Catch
                    End Try
                End If
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())
            Dim query = "SELECT SUM(IFNULL(d.DebitAmount, 0) - IFNULL(d.CreditAmount, 0)) " &
                        "FROM Sanad2 d " &
                        "INNER JOIN Sanad1 e ON d.EntryID = e.EntryID " &
                        whereClause
            
            Dim balanceObj = Sql.ExecuteScalar(query, params.ToArray())
            Dim bal = 0D
            If balanceObj IsNot Nothing AndAlso Not Convert.IsDBNull(balanceObj) Then
                bal = Convert.ToDecimal(balanceObj)
            End If

            If forceCreditNature Then
                Return -bal
            Else
                Return bal
            End If
        End Function

        Private Function GetNetProfit() As Decimal
            If Not SessionContext.CurrentCompanyID.HasValue Then Return 0D
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim settingsSvc As New SettingsService()

            Dim sales = GetPnLBalance("SALES", True)
            Dim salesReturns = GetPnLBalance("SALES_RETURNS", False)
            Dim netSales = sales - salesReturns

            Dim invStart = GetPnLBalance("INV_START", False)
            Dim purchases = GetPnLBalance("PURCHASES", False)
            Dim purchaseReturns = GetPnLBalance("PURCHASE_RETURNS", True)
            Dim directCosts = GetPnLBalance("DIRECT_COSTS", False)

            Dim invEnd = GetEndingInventoryValue()
            Dim cogs = invStart + (purchases - purchaseReturns) + directCosts - invEnd
            Dim grossProfit = netSales - cogs

            Dim opExpenses = GetPnLBalance("OP_EXPENSES", False)
            Dim opProfit = grossProfit - opExpenses

            Dim nonOp = GetPnLBalance("NON_OP", True)
            Dim ebt = opProfit + nonOp

            Dim taxMethodStr = settingsSvc.GetSettingValue($"Company_{companyId}_PnL_TaxMethod", "0")
            Dim taxManualStr = settingsSvc.GetSettingValue($"Company_{companyId}_PnL_TaxManualValue", "0")

            Dim tax As Decimal = 0D
            If taxMethodStr = "1" Then
                Decimal.TryParse(taxManualStr, tax)
            Else
                tax = Math.Abs(GetPnLBalance("TAX", False))
            End If

            Return ebt - tax
        End Function

        Private Sub AddReportRow(title As String, amount As Decimal, isTotal As Boolean)
            Dim idx = dgvReport.Rows.Add(title, amount)
            Dim row = dgvReport.Rows(idx)
            If isTotal Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255)
                row.DefaultCellStyle.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            End If
        End Sub

    End Class
End Namespace
