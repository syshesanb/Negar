Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Imports Microsoft.VisualBasic
Imports Negar.Business

Namespace Negar.Forms

    Public Class CheckManagementForm
        Inherits Form

        Private _paymentSvc As New PaymentService()
        Private dgvChecks As DataGridView
        Private dgvHistory As DataGridView
        Private cmbStatus As ComboBox
        Private dtpFrom As DateTimePicker
        Private dtpTo As DateTimePicker
        Private chkFilterDate As CheckBox
        Private lblSummary As Label
        Private _selectedCheckId As Integer? = Nothing

        Public Sub New()
            Me.Text = "مدیریت چک‌های خرید"
            Me.Size = New Size(1100, 700)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Font = New Font("Tahoma", 9)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.BackColor = Color.White
            InitForm()
            LoadChecks()
        End Sub

        Private Sub InitForm()
            ' پنل فیلتر بالا
            Dim pnlFilter As New Panel() With {.Dock = DockStyle.Top, .Height = 45, .BackColor = Color.FromArgb(245, 248, 252)}
            Dim lblSt As New Label() With {.Text = "وضعیت:", .Location = New Point(900, 12), .Size = New Size(60, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbStatus = New ComboBox() With {.Location = New Point(700, 12), .Size = New Size(195, 22), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbStatus.Items.AddRange(New String() {"همه", "در جریان", "پاس شده", "برگشت خورده", "تعویض شده", "عودت داده شده"})
            cmbStatus.SelectedIndex = 0

            chkFilterDate = New CheckBox() With {.Text = "فیلتر سررسید:", .Location = New Point(560, 12), .Size = New Size(130, 22)}
            dtpFrom = New DateTimePicker() With {.Location = New Point(420, 12), .Size = New Size(130, 22), .Format = DateTimePickerFormat.Short, .Enabled = False}
            Dim lblTo As New Label() With {.Text = "تا", .Location = New Point(390, 12), .Size = New Size(25, 22), .TextAlign = ContentAlignment.MiddleCenter}
            dtpTo = New DateTimePicker() With {.Location = New Point(250, 12), .Size = New Size(130, 22), .Format = DateTimePickerFormat.Short, .Enabled = False, .Value = DateTime.Today.AddMonths(3)}
            Dim btnSearch As New Button() With {.Text = "جستجو", .Location = New Point(150, 10), .Size = New Size(90, 26), .BackColor = Color.FromArgb(0, 120, 215), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            
            AddHandler chkFilterDate.CheckedChanged, Sub(s, e)
                                                         dtpFrom.Enabled = chkFilterDate.Checked
                                                         dtpTo.Enabled = chkFilterDate.Checked
                                                     End Sub
            AddHandler btnSearch.Click, AddressOf BtnSearch_Click
            AddHandler cmbStatus.SelectedIndexChanged, AddressOf BtnSearch_Click
            pnlFilter.Controls.AddRange(New Control() {lblSt, cmbStatus, chkFilterDate, dtpFrom, lblTo, dtpTo, btnSearch})

            ' لیبل خلاصه
            lblSummary = New Label() With {.Dock = DockStyle.Top, .Height = 28, .TextAlign = ContentAlignment.MiddleRight,
                                            .Font = New Font("Tahoma", 9, FontStyle.Bold), .ForeColor = Color.DarkBlue,
                                            .BackColor = Color.FromArgb(232, 240, 255), .Padding = New Padding(0, 0, 10, 0)}

            ' جداکننده
            Dim split As New SplitContainer() With {.Dock = DockStyle.Fill, .Orientation = Orientation.Horizontal,
                                                     .SplitterDistance = 400, .Panel1MinSize = 200, .Panel2MinSize = 100}

            ' گرید چک‌ها
            dgvChecks = New DataGridView() With {.Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
                                                  .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                                  .BackgroundColor = Color.White, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                                                  .RightToLeft = RightToLeft.Yes}
            AddHandler dgvChecks.SelectionChanged, AddressOf DgvChecks_SelectionChanged
            split.Panel1.Controls.Add(dgvChecks)

            ' پنل تاریخچه
            Dim pnlHist As New Panel() With {.Dock = DockStyle.Fill}
            Dim lblHist As New Label() With {.Text = "تاریخچه وضعیت چک انتخاب شده:", .Dock = DockStyle.Top, .Height = 24,
                                              .Font = New Font("Tahoma", 9, FontStyle.Bold), .ForeColor = Color.DarkBlue,
                                              .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 5, 0)}
            dgvHistory = New DataGridView() With {.Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
                                                   .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                                   .BackgroundColor = Color.White, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                                                   .RightToLeft = RightToLeft.Yes}
            pnlHist.Controls.Add(dgvHistory)
            pnlHist.Controls.Add(lblHist)
            split.Panel2.Controls.Add(pnlHist)

            ' پنل دکمه‌های وضعیت
            Dim pnlBtns As New Panel() With {.Dock = DockStyle.Bottom, .Height = 45, .BackColor = Color.FromArgb(245, 248, 252)}
            Dim btnPassed As New Button() With {.Text = "✓ پاس شد", .Size = New Size(110, 32), .Location = New Point(960, 7), .BackColor = Color.FromArgb(39, 174, 96), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            Dim btnBounced As New Button() With {.Text = "✗ برگشت خورد", .Size = New Size(120, 32), .Location = New Point(835, 7), .BackColor = Color.FromArgb(192, 57, 43), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            Dim btnExchanged As New Button() With {.Text = "⇄ تعویض شد", .Size = New Size(110, 32), .Location = New Point(720, 7), .BackColor = Color.FromArgb(41, 128, 185), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            Dim btnReturned As New Button() With {.Text = "↩ عودت داده شد", .Size = New Size(130, 32), .Location = New Point(585, 7), .BackColor = Color.FromArgb(127, 140, 141), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            
            AddHandler btnPassed.Click, Sub(s, e)
                                           ChangeStatus("پاس شده")
                                       End Sub
            AddHandler btnBounced.Click, Sub(s, e)
                                            ChangeStatus("برگشت خورد")
                                        End Sub
            AddHandler btnExchanged.Click, Sub(s, e)
                                              ChangeStatus("تعویض شده")
                                          End Sub
            AddHandler btnReturned.Click, Sub(s, e)
                                             ChangeStatus("عودت داده شد")
                                         End Sub

            pnlBtns.Controls.AddRange(New Control() {btnPassed, btnBounced, btnExchanged, btnReturned})

            Me.Controls.Add(split)
            Me.Controls.Add(pnlBtns)
            Me.Controls.Add(lblSummary)
            Me.Controls.Add(pnlFilter)
        End Sub

        Private Sub LoadChecks()
            Dim status = If(cmbStatus.SelectedItem?.ToString() = "همه", "", cmbStatus.SelectedItem?.ToString())
            Dim fromD As Date? = If(chkFilterDate.Checked, CType(dtpFrom.Value, Date?), Nothing)
            Dim toD As Date? = If(chkFilterDate.Checked, CType(dtpTo.Value, Date?), Nothing)
            Dim dt = _paymentSvc.GetAllChecks(status, fromD, toD)
            dgvChecks.DataSource = dt

            ' برچسب‌های فارسی ستون‌ها
            Dim colNames = New Dictionary(Of String, String) From {
                {"CheckID", "شناسه"}, {"CheckNumber", "شماره چک"}, {"BankName", "بانک"},
                {"BranchName", "شعبه"}, {"Amount", "مبلغ"}, {"DueDate", "سررسید"},
                {"Status", "وضعیت"}, {"BounceFee", "جریمه برگشت"}, {"Notes", "یادداشت"},
                {"PurchaseInvoiceID", "شناسه فاکتور"}, {"InvoiceNumber", "شماره فاکتور"}, {"VendorName", "تامین‌کننده"}}
            For Each col As DataGridViewColumn In dgvChecks.Columns
                If colNames.ContainsKey(col.Name) Then col.HeaderText = colNames(col.Name)
                If col.Name = "Amount" Then col.DefaultCellStyle.Format = "N0"
                If col.Name = "BounceFee" Then col.DefaultCellStyle.Format = "N0"
            Next

            ' رنگ‌بندی بر اساس وضعیت
            For Each row As DataGridViewRow In dgvChecks.Rows
                Dim st = Convert.ToString(row.Cells("Status").Value)
                Select Case st
                    Case "پاس شده" : row.DefaultCellStyle.BackColor = Color.FromArgb(212, 245, 220)
                    Case "برگشت خورد" : row.DefaultCellStyle.BackColor = Color.FromArgb(250, 215, 215)
                    Case "تعویض شده" : row.DefaultCellStyle.BackColor = Color.FromArgb(215, 230, 250)
                    Case "عودت داده شد" : row.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230)
                End Select
            Next

            Dim total = 0D
            For Each row As DataRow In dt.Rows : total += Convert.ToDecimal(row("Amount")) : Next
            lblSummary.Text = $"  تعداد چک: {dt.Rows.Count}   |   مجموع مبلغ: {total:N0} ریال"
        End Sub

        Private Sub DgvChecks_SelectionChanged(sender As Object, e As EventArgs)
            dgvHistory.DataSource = Nothing
            If dgvChecks.SelectedRows.Count = 0 Then _selectedCheckId = Nothing : Return
            Dim checkId = Convert.ToInt32(dgvChecks.SelectedRows(0).Cells("CheckID").Value)
            _selectedCheckId = checkId
            Dim hist = _paymentSvc.GetCheckStatusHistory(checkId)
            dgvHistory.DataSource = hist
            Dim histCols = New Dictionary(Of String, String) From {
                {"HistoryID", "ردیف"}, {"ChangeDate", "تاریخ"}, {"OldStatus", "وضعیت قبلی"},
                {"NewStatus", "وضعیت جدید"}, {"BounceFee", "جریمه"}, {"Description", "توضیحات"}, {"ChangedBy", "توسط"}}
            For Each col As DataGridViewColumn In dgvHistory.Columns
                If histCols.ContainsKey(col.Name) Then col.HeaderText = histCols(col.Name)
            Next
        End Sub

        Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
            LoadChecks()
        End Sub

        Private Sub ChangeStatus(newStatus As String)
            If Not _selectedCheckId.HasValue Then
                MessageBox.Show("ابتدا یک چک انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim checkRow = _paymentSvc.GetCheckById(_selectedCheckId.Value)
            If checkRow Is Nothing Then Return
            Dim curStatus = Convert.ToString(checkRow("Status"))
            If curStatus <> "در جریان" Then
                MessageBox.Show($"این چک قبلاً با وضعیت «{curStatus}» ثبت شده است.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' دیالوگ تأیید
            Dim desc As String = ""
            Dim fee As Decimal = 0
            Dim msg = $"آیا می‌خواهید وضعیت چک {Convert.ToString(checkRow("CheckNumber"))} را به «{newStatus}» تغییر دهید؟"
            If newStatus = "برگشت خورد" Then
                msg &= Environment.NewLine & "جریمه برگشت (اختیاری، ریال):"
            End If
            If MessageBox.Show(msg, "تأیید تغییر وضعیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

            If newStatus = "برگشت خورد" Then
                Dim feeStr = Interaction.InputBox("جریمه برگشت چک (ریال) - اختیاری:", "جریمه برگشت", "0")
                Decimal.TryParse(feeStr, fee)
            End If
            Dim descInput = Interaction.InputBox("توضیحات (اختیاری):", "توضیحات", "")
            desc = If(descInput, "")

            Try
                _paymentSvc.UpdateCheckStatus(_selectedCheckId.Value, newStatus, DateTime.Today, desc, fee)
                MessageBox.Show("وضعیت چک با موفقیت تغییر یافت.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadChecks()
            Catch ex As Exception
                MessageBox.Show("خطا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class

End Namespace
