Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Linq
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class HesabdaryProfitLossReportControl
        Inherits UserControl

        Private pnlTop As FlowLayoutPanel
        Private dgvReport As DataGridView
        Private _settingsSvc As New SettingsService()

        ' GroupBoxes
        Private grpInventory As GroupBox
        Private grpTax As GroupBox
        Private grpActions As GroupBox

        ' Buttons
        Private btnSetup As Button
        Private btnCalculate As Button
        Private btnPrint As Button
        Private btnExcel As Button
        Private btnEdit As Button
        Private btnSave As Button

        ' Inventory Controls
        Private cmbInvMethod As ComboBox
        Private txtInvManual As TextBox

        ' Tax Controls
        Private cmbTaxMethod As ComboBox
        Private txtTaxManual As TextBox

        ' Range UI
        Private grpRange As GroupBox
        Private cmbRangeMethod As ComboBox
        Private lblRangeFrom As Label
        Private txtRangeFrom As TextBox
        Private btnCalendarFrom As Button
        Private lblRangeTo As Label
        Private txtRangeTo As TextBox
        Private btnCalendarTo As Button

        ' Actions UI
        Public Sub New()
            InitializeComponent()
            LoadSettings()
            SetEditMode(False)
            ApplySecurity()
        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            btnPrint.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossPrint) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
            btnExcel.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossExport) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
            If btnSave IsNot Nothing Then btnSave.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossSaveSettings) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
            If btnEdit IsNot Nothing Then btnEdit.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossEditSettings) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
            If btnSetup IsNot Nothing Then btnSetup.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossMapAccounts) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
            If btnCalculate IsNot Nothing Then btnCalculate.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLossCalculate) OrElse SessionContext.HasPermission(PermissionKeys.AccountingProfitLoss)
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

            ' 1. Range GroupBox
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

            lblRangeFrom = New Label()
            lblRangeFrom.Text = "از:"
            lblRangeFrom.Location = New Point(315, 58)
            lblRangeFrom.Width = 25
            lblRangeFrom.TextAlign = ContentAlignment.MiddleLeft

            txtRangeFrom = New TextBox()
            txtRangeFrom.Width = 90
            txtRangeFrom.Location = New Point(220, 55)
            AddHandler txtRangeFrom.KeyPress, AddressOf NumericOnly_KeyPress
            AddHandler txtRangeFrom.TextChanged, AddressOf txtRange_TextChanged

            btnCalendarFrom = New Button()
            btnCalendarFrom.Text = "📅"
            btnCalendarFrom.Size = New Size(26, 24)
            btnCalendarFrom.Location = New Point(190, 54)
            btnCalendarFrom.FlatStyle = FlatStyle.Flat
            btnCalendarFrom.BackColor = Color.White
            btnCalendarFrom.Visible = False
            AddHandler btnCalendarFrom.Click, AddressOf btnCalendarFrom_Click

            lblRangeTo = New Label()
            lblRangeTo.Text = "تا:"
            lblRangeTo.Location = New Point(160, 58)
            lblRangeTo.Width = 25
            lblRangeTo.TextAlign = ContentAlignment.MiddleLeft

            txtRangeTo = New TextBox()
            txtRangeTo.Width = 90
            txtRangeTo.Location = New Point(65, 55)
            AddHandler txtRangeTo.KeyPress, AddressOf NumericOnly_KeyPress
            AddHandler txtRangeTo.TextChanged, AddressOf txtRange_TextChanged

            btnCalendarTo = New Button()
            btnCalendarTo.Text = "📅"
            btnCalendarTo.Size = New Size(26, 24)
            btnCalendarTo.Location = New Point(35, 54)
            btnCalendarTo.FlatStyle = FlatStyle.Flat
            btnCalendarTo.BackColor = Color.White
            btnCalendarTo.Visible = False
            AddHandler btnCalendarTo.Click, AddressOf btnCalendarTo_Click

            grpRange.Controls.Add(cmbRangeMethod)
            grpRange.Controls.Add(lblRangeFrom)
            grpRange.Controls.Add(txtRangeFrom)
            grpRange.Controls.Add(btnCalendarFrom)
            grpRange.Controls.Add(lblRangeTo)
            grpRange.Controls.Add(txtRangeTo)
            grpRange.Controls.Add(btnCalendarTo)
            
            ' 2. Inventory GroupBox
            grpInventory = New GroupBox()
            grpInventory.Text = "تنظیمات موجودی پایان دوره"
            grpInventory.Size = New Size(250, 92)
            grpInventory.RightToLeft = RightToLeft.Yes

            cmbInvMethod = New ComboBox()
            cmbInvMethod.DropDownStyle = ComboBoxStyle.DropDownList
            cmbInvMethod.Width = 230
            cmbInvMethod.Location = New Point(10, 23)
            cmbInvMethod.Items.Add("ورود از سیستم انبارداری")
            cmbInvMethod.Items.Add("ورود بصورت دستی")
            cmbInvMethod.SelectedIndex = 0
            AddHandler cmbInvMethod.SelectedIndexChanged, AddressOf cmbInvMethod_SelectedIndexChanged

            txtInvManual = New TextBox()
            txtInvManual.Width = 230
            txtInvManual.Location = New Point(10, 55)
            txtInvManual.Enabled = False
            txtInvManual.Text = "0"
            AddHandler txtInvManual.KeyPress, AddressOf NumericOnly_KeyPress
            AddHandler txtInvManual.Enter, AddressOf txtManual_Enter
            AddHandler txtInvManual.Leave, AddressOf txtManual_Leave

            grpInventory.Controls.Add(cmbInvMethod)
            grpInventory.Controls.Add(txtInvManual)

            ' 3. Tax GroupBox
            grpTax = New GroupBox()
            grpTax.Text = "تنظیمات مالیات"
            grpTax.Size = New Size(250, 92)
            grpTax.RightToLeft = RightToLeft.Yes

            cmbTaxMethod = New ComboBox()
            cmbTaxMethod.DropDownStyle = ComboBoxStyle.DropDownList
            cmbTaxMethod.Width = 230
            cmbTaxMethod.Location = New Point(10, 23)
            cmbTaxMethod.Items.Add("ورود از سیستم حسابداری")
            cmbTaxMethod.Items.Add("ورود بصورت دستی")
            cmbTaxMethod.SelectedIndex = 0
            AddHandler cmbTaxMethod.SelectedIndexChanged, AddressOf cmbTaxMethod_SelectedIndexChanged

            txtTaxManual = New TextBox()
            txtTaxManual.Width = 230
            txtTaxManual.Location = New Point(10, 55)
            txtTaxManual.Enabled = False
            txtTaxManual.Text = "0"
            AddHandler txtTaxManual.KeyPress, AddressOf NumericOnly_KeyPress
            AddHandler txtTaxManual.Enter, AddressOf txtManual_Enter
            AddHandler txtTaxManual.Leave, AddressOf txtManual_Leave

            grpTax.Controls.Add(cmbTaxMethod)
            grpTax.Controls.Add(txtTaxManual)

            ' 4. Actions GroupBox
            grpActions = New GroupBox()
            grpActions.Text = "عملیات گزارش"
            grpActions.Size = New Size(345, 92)
            grpActions.RightToLeft = RightToLeft.Yes

            ' Row 1 Buttons
            btnCalculate = New Button()
            btnCalculate.Text = "محاسبه و نمایش"
            btnCalculate.Size = New Size(105, 28)
            btnCalculate.Location = New Point(230, 21)
            btnCalculate.BackColor = Color.FromArgb(46, 204, 113)
            btnCalculate.ForeColor = Color.White
            btnCalculate.FlatStyle = FlatStyle.Flat
            AddHandler btnCalculate.Click, AddressOf btnCalculate_Click

            btnSetup = New Button()
            btnSetup.Text = "معرفی حساب‌ها"
            btnSetup.Size = New Size(105, 28)
            btnSetup.Location = New Point(120, 21)
            btnSetup.BackColor = Color.SteelBlue
            btnSetup.ForeColor = Color.White
            btnSetup.FlatStyle = FlatStyle.Flat
            AddHandler btnSetup.Click, AddressOf btnSetup_Click

            btnEdit = New Button()
            btnEdit.Text = "ویرایش تنظیمات"
            btnEdit.Size = New Size(105, 28)
            btnEdit.Location = New Point(10, 21)
            btnEdit.BackColor = Color.FromArgb(241, 196, 15)
            btnEdit.ForeColor = Color.Black
            btnEdit.FlatStyle = FlatStyle.Flat
            AddHandler btnEdit.Click, AddressOf btnEdit_Click

            ' Row 2 Buttons
            btnPrint = New Button()
            btnPrint.Text = "نمایش و چاپ"
            btnPrint.Size = New Size(105, 28)
            btnPrint.Location = New Point(230, 55)
            btnPrint.BackColor = Color.FromArgb(52, 152, 219)
            btnPrint.ForeColor = Color.White
            btnPrint.FlatStyle = FlatStyle.Flat
            AddHandler btnPrint.Click, AddressOf btnPrint_Click

            btnExcel = New Button()
            btnExcel.Text = "خروجی اکسل"
            btnExcel.Size = New Size(105, 28)
            btnExcel.Location = New Point(120, 55)
            btnExcel.BackColor = Color.FromArgb(39, 174, 96)
            btnExcel.ForeColor = Color.White
            btnExcel.FlatStyle = FlatStyle.Flat
            AddHandler btnExcel.Click, AddressOf btnExcel_Click

            btnSave = New Button()
            btnSave.Text = "ذخیره تنظیمات"
            btnSave.Size = New Size(105, 28)
            btnSave.Location = New Point(10, 55)
            btnSave.BackColor = Color.FromArgb(142, 68, 173) ' Purple / Save style
            btnSave.ForeColor = Color.White
            btnSave.FlatStyle = FlatStyle.Flat
            AddHandler btnSave.Click, AddressOf btnSave_Click

            grpActions.Controls.Add(btnCalculate)
            grpActions.Controls.Add(btnSetup)
            grpActions.Controls.Add(btnEdit)
            grpActions.Controls.Add(btnPrint)
            grpActions.Controls.Add(btnExcel)
            grpActions.Controls.Add(btnSave)

            ' Add to Top Panel in Right-To-Left order
            ' To place grpActions on the RIGHT, we add it FIRST.
            pnlTop.Controls.Add(grpActions)
            pnlTop.Controls.Add(grpRange)
            pnlTop.Controls.Add(grpInventory)
            pnlTop.Controls.Add(grpTax)
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

            dgvReport.Columns.Add("Title", "شرح")
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
                Dim invMethodStr = _settingsSvc.GetSettingValue($"Company_{cid}_PnL_InvMethod", "0")
                Dim invManualStr = _settingsSvc.GetSettingValue($"Company_{cid}_PnL_InvManualValue", "0")
                Dim taxMethodStr = _settingsSvc.GetSettingValue($"Company_{cid}_PnL_TaxMethod", "0")
                Dim taxManualStr = _settingsSvc.GetSettingValue($"Company_{cid}_PnL_TaxManualValue", "0")

                Dim idxInv = 0
                Integer.TryParse(invMethodStr, idxInv)
                cmbInvMethod.SelectedIndex = idxInv

                Dim invVal As Decimal = 0D
                Decimal.TryParse(invManualStr, invVal)
                txtInvManual.Text = invVal.ToString("N0")

                Dim idxTax = 0
                Integer.TryParse(taxMethodStr, idxTax)
                cmbTaxMethod.SelectedIndex = idxTax

                Dim taxVal As Decimal = 0D
                Decimal.TryParse(taxManualStr, taxVal)
                txtTaxManual.Text = taxVal.ToString("N0")
            Catch
            End Try
        End Sub


        Private Sub SetEditMode(isEditMode As Boolean)
            cmbInvMethod.Enabled = isEditMode
            txtInvManual.Enabled = isEditMode AndAlso (cmbInvMethod.SelectedIndex = 1)

            cmbTaxMethod.Enabled = isEditMode
            txtTaxManual.Enabled = isEditMode AndAlso (cmbTaxMethod.SelectedIndex = 1)

            btnSetup.Enabled = isEditMode
            btnSave.Enabled = isEditMode
            btnEdit.Enabled = Not isEditMode

            btnCalculate.Enabled = True
            btnPrint.Enabled = True
            btnExcel.Enabled = True
        End Sub

        Private Sub btnEdit_Click(sender As Object, e As EventArgs)
            SetEditMode(True)
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Dim cid = SessionContext.CurrentCompanyID.Value

            Try
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_InvMethod", cmbInvMethod.SelectedIndex.ToString(), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_InvManualValue", txtInvManual.Text.Replace(",", ""), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_TaxMethod", cmbTaxMethod.SelectedIndex.ToString(), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_TaxManualValue", txtTaxManual.Text.Replace(",", ""), "PnL")

                ' Format on Save to be sure
                Dim invVal As Decimal = 0D
                Decimal.TryParse(txtInvManual.Text.Replace(",", ""), invVal)
                txtInvManual.Text = invVal.ToString("N0")

                Dim taxVal As Decimal = 0D
                Decimal.TryParse(txtTaxManual.Text.Replace(",", ""), taxVal)
                txtTaxManual.Text = taxVal.ToString("N0")

                MessageBox.Show("تنظیمات با موفقیت ذخیره شدند.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SetEditMode(False)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub cmbInvMethod_SelectedIndexChanged(sender As Object, e As EventArgs)
            txtInvManual.Enabled = btnSave.Enabled AndAlso (cmbInvMethod.SelectedIndex = 1)
            If cmbInvMethod.SelectedIndex <> 1 Then
                txtInvManual.Text = "0"
            End If
        End Sub

        Private Sub cmbTaxMethod_SelectedIndexChanged(sender As Object, e As EventArgs)
            txtTaxManual.Enabled = btnSave.Enabled AndAlso (cmbTaxMethod.SelectedIndex = 1)
            If cmbTaxMethod.SelectedIndex <> 1 Then
                txtTaxManual.Text = "0"
            End If
        End Sub

        Private Sub cmbRangeMethod_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim isDoc = (cmbRangeMethod.SelectedIndex = 0)
            Dim isDate = (cmbRangeMethod.SelectedIndex = 1)
            Dim isAll = (cmbRangeMethod.SelectedIndex = 2)

            lblRangeFrom.Visible = Not isAll
            txtRangeFrom.Visible = Not isAll
            btnCalendarFrom.Visible = isDate

            lblRangeTo.Visible = Not isAll
            txtRangeTo.Visible = Not isAll
            btnCalendarTo.Visible = isDate

            txtRangeFrom.Text = ""
            txtRangeTo.Text = ""
        End Sub

        Private Sub txtRange_TextChanged(sender As Object, e As EventArgs)
            If cmbRangeMethod.SelectedIndex = 1 Then ' Date mode
                Dim txtBox = DirectCast(sender, TextBox)
                Dim cursor = txtBox.SelectionStart
                Dim text = txtBox.Text.Replace("/", "")
                If Not String.IsNullOrEmpty(text) AndAlso text.All(Function(c) Char.IsDigit(c)) Then
                    txtBox.Text = FormatPersianDatePartial(text)
                    txtBox.SelectionStart = If(cursor > txtBox.Text.Length, txtBox.Text.Length, cursor)
                End If
            End If
        End Sub

        Private Function FormatPersianDatePartial(digits As String) As String
            If digits.Length > 8 Then digits = digits.Substring(0, 8)
            Select Case digits.Length
                Case <= 4 : Return digits
                Case <= 6 : Return digits.Substring(0, 4) & "/" & digits.Substring(4)
                Case Else : Return digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6)
            End Select
        End Function

        Private Sub btnCalendarFrom_Click(sender As Object, e As EventArgs)
            ShowCalendarForTextBox(txtRangeFrom)
        End Sub

        Private Sub btnCalendarTo_Click(sender As Object, e As EventArgs)
            ShowCalendarForTextBox(txtRangeTo)
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

        Private Sub NumericOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
            If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub txtManual_Enter(sender As Object, e As EventArgs)
            Dim txt = DirectCast(sender, TextBox)
            Dim cleanVal As Decimal = 0D
            Dim textToParse = txt.Text.Replace(",", "")
            If Decimal.TryParse(textToParse, cleanVal) Then
                txt.Text = cleanVal.ToString("F0")
            End If
            txt.SelectAll()
        End Sub

        Private Sub txtManual_Leave(sender As Object, e As EventArgs)
            Dim txt = DirectCast(sender, TextBox)
            Dim cleanVal As Decimal = 0D
            Dim textToParse = txt.Text.Replace(",", "")
            If Decimal.TryParse(textToParse, cleanVal) Then
                txt.Text = cleanVal.ToString("N0")
            End If
        End Sub

        Private Sub btnSetup_Click(sender As Object, e As EventArgs)
            Using frm As New HesabdaryProfitLossMappingForm()
                frm.ShowDialog()
            End Using
        End Sub

        Private Sub btnCalculate_Click(sender As Object, e As EventArgs)
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return

            ' Save the settings before running the report automatically
            Try
                Dim cid = SessionContext.CurrentCompanyID.Value
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_InvMethod", cmbInvMethod.SelectedIndex.ToString(), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_InvManualValue", txtInvManual.Text.Replace(",", ""), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_TaxMethod", cmbTaxMethod.SelectedIndex.ToString(), "PnL")
                _settingsSvc.SaveSetting($"Company_{cid}_PnL_TaxManualValue", txtTaxManual.Text.Replace(",", ""), "PnL")
            Catch
            End Try

            Dim companyId = SessionContext.CurrentCompanyID.Value

            ' 1. Fetch balances
            Dim sales = GetBalance("SALES", True)
            Dim salesReturns = GetBalance("SALES_RETURNS", False)
            Dim netSales = sales - salesReturns

            Dim invStart = GetBalance("INV_START", False)
            Dim purchases = GetBalance("PURCHASES", False)
            Dim purchaseReturns = GetBalance("PURCHASE_RETURNS", True)
            Dim directCosts = GetBalance("DIRECT_COSTS", False)

            ' 2. Calculate Ending Inventory
            Dim invEnd As Decimal = 0D
            If cmbInvMethod.SelectedIndex = 1 Then
                ' Manual
                Decimal.TryParse(txtInvManual.Text.Replace(",", ""), invEnd)
            Else
                ' From Inventory System
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

            Dim cogs = invStart + (purchases - purchaseReturns) + directCosts - invEnd
            Dim grossProfit = netSales - cogs

            Dim opExpenses = GetBalance("OP_EXPENSES", False)
            Dim opProfit = grossProfit - opExpenses

            Dim nonOp = GetBalance("NON_OP", True) ' Credit nature so credits are positive revenues, debits are negative expenses
            Dim ebt = opProfit + nonOp

            ' 3. Calculate Tax
            Dim tax As Decimal = 0D
            If cmbTaxMethod.SelectedIndex = 1 Then
                ' Manual
                Decimal.TryParse(txtTaxManual.Text.Replace(",", ""), tax)
            Else
                ' From Accounting System
                tax = Math.Abs(GetBalance("TAX", False))
            End If

            Dim netProfit = ebt - tax

            ' Display in Grid
            dgvReport.Rows.Clear()
            AddReportRow("فروش / درآمدها", sales, False)
            AddReportRow("کسری‌ها و برگشتی‌های فروش", salesReturns, False)
            AddReportRow("فروش خالص", netSales, True)

            AddReportRow("بهای تمام شده کالای فروش رفته", cogs, True)
            AddReportRow("   موجودی کالای اول دوره", invStart, False)
            AddReportRow("   خرید کالا", purchases, False)
            AddReportRow("   کسری‌ها و برگشتی‌های خرید", purchaseReturns, False)
            AddReportRow("   دستمزد و سربار مستقیم تولید", directCosts, False)
            AddReportRow("   (کسر می‌شود) موجودی پایان دوره", invEnd, False)

            AddReportRow("سود (زیان) ناویژه", grossProfit, True)
            AddReportRow("هزینه‌های عملیاتی / اداری و تشکیلاتی", opExpenses, False)
            AddReportRow("سود (زیان) عملیاتی", opProfit, True)
            AddReportRow("سایر درآمدها و هزینه‌های غیرعملیاتی", nonOp, False)
            AddReportRow("سود (زیان) قبل از کسر مالیات", ebt, True)
            AddReportRow("مالیات", tax, False)
            AddReportRow("سود (زیان) خالص", netProfit, True)
        End Sub

        Private Sub btnPrint_Click(sender As Object, e As EventArgs)
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("ابتدا گزارش را محاسبه و نمایش دهید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim companyName = If(SessionContext.CurrentCompanyName, "مؤسسه")
            
            Dim dateTitle = ""
            If cmbRangeMethod.SelectedIndex = 0 Then
                Dim fromStr = If(String.IsNullOrWhiteSpace(txtRangeFrom.Text), "اول", txtRangeFrom.Text)
                Dim toStr = If(String.IsNullOrWhiteSpace(txtRangeTo.Text), "آخر", txtRangeTo.Text)
                dateTitle = "از سند: " & fromStr & " تا سند: " & toStr
            Else
                Dim fromStr = If(String.IsNullOrWhiteSpace(txtRangeFrom.Text), "ابتدا", txtRangeFrom.Text)
                Dim toStr = If(String.IsNullOrWhiteSpace(txtRangeTo.Text), "انتها", txtRangeTo.Text)
                dateTitle = "از تاریخ: " & fromStr & " تا تاریخ: " & toStr
            End If

            ' Columns
            Dim printCols As New List(Of HesabdaryTarazPrintForm.PrintColumnInfo)()
            printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "AccountName", .Title = "شرح", .WidthRatio = 4.0F})
            printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "Amount", .Title = "مبلغ (ریال)", .WidthRatio = 2.0F})

            ' Rows
            Dim printRows As New List(Of HesabdaryTarazPrintForm.PrintRowInfo)()
            For Each row As DataGridViewRow In dgvReport.Rows
                Dim title = Convert.ToString(row.Cells(0).Value)
                Dim amount = Convert.ToDecimal(row.Cells(1).Value)

                Dim isTotal = row.DefaultCellStyle.Font IsNot Nothing AndAlso row.DefaultCellStyle.Font.Bold

                ' Handle indents for detail rows
                Dim level = 0
                If title.StartsWith("   ") Then
                    level = 1
                End If

                Dim rInfo As New HesabdaryTarazPrintForm.PrintRowInfo() With {
                    .AccountName = title.Trim(),
                    .IsHeader = isTotal,
                    .Level = level
                }
                rInfo.Values("Amount") = amount
                printRows.Add(rInfo)
            Next

            Dim totals As New Dictionary(Of String, Decimal)()

            Using printForm As New HesabdaryTarazPrintForm(companyName, dateTitle, printCols, printRows, totals, "گزارش عملکرد و سود و زیان")
                printForm.ShowDialog(Me)
            End Using
        End Sub

        Private Sub btnExcel_Click(sender As Object, e As EventArgs)
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("ابتدا گزارش را محاسبه و نمایش دهید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            ExportGridToExcel(dgvReport, "Profit_and_Loss_Report")
        End Sub

        Private Sub ExportGridToExcel(dgv As DataGridView, defaultFileName As String)
            Using sfd As New SaveFileDialog()
                sfd.Filter = "Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*"
                sfd.Title = "خروجی اکسل"
                sfd.FileName = defaultFileName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim sb As New System.Text.StringBuilder()

                        ' Write headers
                        Dim headers As New List(Of String)()
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                headers.Add(col.HeaderText)
                            End If
                        Next
                        sb.AppendLine(String.Join(",", headers))

                        ' Write rows
                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For

                            Dim cells As New List(Of String)()
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                    Dim val = Convert.ToString(row.Cells(col.Index).Value)
                                    ' Escape double quotes and wrap in double quotes if it contains commas or quotes
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

            ' Get mapped accounts
            Dim dtMapped As DataTable = Sql.ExecuteTable("SELECT AccountID FROM PnLAccountMappings WHERE CompanyID = ? AND CategoryKey = ?", companyId, categoryKey)
            If dtMapped.Rows.Count = 0 Then Return 0D

            Dim mappedIds As New List(Of Integer)()
            For Each row As DataRow In dtMapped.Rows
                mappedIds.Add(Convert.ToInt32(row("AccountID")))
            Next

            ' Fetch hierarchy to do recursive summing
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
                Dim fromDocNum = 0
                If Integer.TryParse(txtRangeFrom.Text, fromDocNum) Then
                    filters.Add("CAST(e.ReferenceNumber AS INTEGER) >= ?")
                    params.Add(fromDocNum)
                End If
                Dim toDocNum = 0
                If Integer.TryParse(txtRangeTo.Text, toDocNum) Then
                    filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                    params.Add(toDocNum)
                End If
            ElseIf cmbRangeMethod.SelectedIndex = 1 Then
                If Not String.IsNullOrWhiteSpace(txtRangeFrom.Text) Then
                    Try
                        Dim fromDate = PersianDateHelper.ParsePersianDate(txtRangeFrom.Text)
                        filters.Add("e.EntryDate >= ?")
                        params.Add(fromDate)
                    Catch
                    End Try
                End If
                If Not String.IsNullOrWhiteSpace(txtRangeTo.Text) Then
                    Try
                        Dim toDate = PersianDateHelper.ParsePersianDate(txtRangeTo.Text)
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
