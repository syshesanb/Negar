Option Strict Off
Option Explicit On
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Imports Negar.Business
Imports Negar.Business.PersianDateHelper

Namespace Negar.Forms

    Public Class PaymentEntryForm
        Inherits Form

        Private _invoiceId As Integer
        Private _invoiceTotal As Decimal
        Private _totalAlreadyAllocated As Decimal
        Private _editPaymentId As Integer? = Nothing
        Private _oldPaymentAmount As Decimal = 0D
        Private _paymentSvc As New PaymentService()

        ' Controls
        Private cmbPayType As ComboBox
        Private txtAmount As TextBox
        Private txtPayDate As TextBox
        Private btnCalPayDate As Button
        Private txtDueDate As TextBox
        Private btnCalDueDate As Button
        Private lblDueDate As Label
        Private txtDescription As TextBox

        ' GroupBox panels
        ' 1. Cash GroupBox
        Private grpCash As GroupBox
        Private txtCashReceiptNo As TextBox
        Private txtCashierName As TextBox
        Private txtReceiverName As TextBox
        Private txtCashNotes As TextBox

        ' 2. Bank GroupBox
        Private grpBank As GroupBox
        Private txtBankReceiptNo As TextBox
        Private txtBankName As TextBox
        Private txtAccountCardNo As TextBox
        Private txtBankNotes As TextBox

        ' 3. Check GroupBox
        Private grpCheck As GroupBox
        Private txtCheckNumber As TextBox
        Private txtCheckBankName As TextBox
        Private txtCheckBranchName As TextBox
        Private txtCheckAccountNumber As TextBox
        Private txtCheckDueDate As TextBox
        Private btnCalCheckDueDate As Button
        Private txtCheckNotes As TextBox

        ' Buttons
        Private btnOK As Button
        Private btnCancel As Button

        Public Sub New(invoiceId As Integer, invoiceTotal As Decimal, totalAlreadyAllocated As Decimal, Optional editPaymentId As Integer? = Nothing)
            _invoiceId = invoiceId
            _invoiceTotal = invoiceTotal
            _totalAlreadyAllocated = totalAlreadyAllocated
            _editPaymentId = editPaymentId
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Font = New Font("Tahoma", 9)
            Me.BackColor = Color.White
            InitForm()

            If _editPaymentId.HasValue Then
                Me.Text = "ویرایش پرداخت"
                LoadPaymentForEdit(_editPaymentId.Value)
            End If
        End Sub

        Private Sub InitForm()
            Me.Text = "افزودن پرداخت"
            Me.Size = New Size(500, 580)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False

            Dim y As Integer = 20

            ' نوع پرداخت (۱. چک ، ۲. واریز به بانک ، ۳. نقد ، ۴. بدهی — پیش‌فرض: چک)
            Dim lblType As New Label() With {.Text = "نوع پرداخت:", .Location = New Point(320, y), .Size = New Size(140, 22), .TextAlign = ContentAlignment.MiddleLeft}
            cmbPayType = New ComboBox() With {.Location = New Point(30, y), .Size = New Size(280, 22), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbPayType.Items.AddRange(New String() {"چک", "واریز به بانک", "نقد", "بدهی"})
            cmbPayType.SelectedIndex = 0
            AddHandler cmbPayType.SelectedIndexChanged, AddressOf CmbPayType_Changed
            y += 35

            ' مبلغ
            Dim lblAmt As New Label() With {.Text = "مبلغ (ریال):", .Location = New Point(320, y), .Size = New Size(140, 22), .TextAlign = ContentAlignment.MiddleLeft}
            txtAmount = New TextBox() With {.Location = New Point(30, y), .Size = New Size(280, 22)}
            Dim remaining = _invoiceTotal - _totalAlreadyAllocated
            If remaining > 0 Then txtAmount.Text = remaining.ToString("N0")
            AddHandler txtAmount.TextChanged, AddressOf TxtAmount_TextChanged
            y += 35

            ' تاریخ پرداخت همراه با دکمه تقویم
            Dim lblPD As New Label() With {.Text = "تاریخ پرداخت:", .Location = New Point(320, y), .Size = New Size(140, 22), .TextAlign = ContentAlignment.MiddleLeft}
            txtPayDate = New TextBox() With {.Location = New Point(65, y), .Size = New Size(245, 22), .Text = ToPersian(DateTime.Today)}
            btnCalPayDate = New Button() With {.Text = "...", .Location = New Point(30, y), .Size = New Size(30, 23), .FlatStyle = FlatStyle.System}
            AddHandler btnCalPayDate.Click, Sub(s, e) ShowCalendarForTextBox(txtPayDate)
            y += 35

            ' سررسید بدهی همراه با دکمه تقویم
            lblDueDate = New Label() With {.Text = "تاریخ سررسید:", .Location = New Point(320, y), .Size = New Size(140, 22), .TextAlign = ContentAlignment.MiddleLeft}
            txtDueDate = New TextBox() With {.Location = New Point(65, y), .Size = New Size(245, 22), .Text = ToPersian(DateTime.Today.AddMonths(1))}
            btnCalDueDate = New Button() With {.Text = "...", .Location = New Point(30, y), .Size = New Size(30, 23), .FlatStyle = FlatStyle.System}
            AddHandler btnCalDueDate.Click, Sub(s, e) ShowCalendarForTextBox(txtDueDate)
            y += 35

            ' توضیحات
            Dim lblDesc As New Label() With {.Text = "توضیحات:", .Location = New Point(320, y), .Size = New Size(140, 22), .TextAlign = ContentAlignment.MiddleLeft}
            txtDescription = New TextBox() With {.Location = New Point(30, y), .Size = New Size(280, 22)}
            y += 40

            ' ==================== 1. GroupBox مشخصات وجه نقد پرداختی ====================
            grpCash = New GroupBox() With {
                .Text = "مشخصات وجه نقد پرداختی",
                .Location = New Point(20, y),
                .Size = New Size(450, 180),
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Tahoma", 9, FontStyle.Bold)
            }
            Dim cY As Integer = 25
            txtCashReceiptNo = AddGroupRow(grpCash, "شماره رسید صندوق:", cY) : cY += 30
            Dim currentUsername = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.FullName, "کاربر جاری")
            txtCashierName = AddGroupRow(grpCash, "نام صندوقدار(کاربر جاری):", cY, currentUsername) : cY += 30
            txtReceiverName = AddGroupRow(grpCash, "نام دریافت کننده وجه:", cY) : cY += 30
            txtCashNotes = AddGroupRow(grpCash, "یادداشت:", cY)

            ' ==================== 2. GroupBox مشخصات وجه پرداختی از طریق بانک ====================
            grpBank = New GroupBox() With {
                .Text = "مشخصات وجه پرداختی از طریق بانک",
                .Location = New Point(20, y),
                .Size = New Size(450, 180),
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Tahoma", 9, FontStyle.Bold)
            }
            Dim bY As Integer = 25
            txtBankReceiptNo = AddGroupRow(grpBank, "شماره فیش / حواله:", bY) : bY += 30
            txtBankName = AddGroupRow(grpBank, "نام بانک:", bY) : bY += 30
            txtAccountCardNo = AddGroupRow(grpBank, "شماره حساب / شماره کارت:", bY) : bY += 30
            txtBankNotes = AddGroupRow(grpBank, "یادداشت:", bY)

            ' ==================== 3. GroupBox مشخصات چک ====================
            grpCheck = New GroupBox() With {
                .Text = "مشخصات چک",
                .Location = New Point(20, y),
                .Size = New Size(450, 240),
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Tahoma", 9, FontStyle.Bold)
            }
            Dim chkY As Integer = 25
            txtCheckNumber = AddGroupRow(grpCheck, "شماره چک:", chkY) : chkY += 30
            txtCheckBankName = AddGroupRow(grpCheck, "نام بانک:", chkY) : chkY += 30
            txtCheckBranchName = AddGroupRow(grpCheck, "نام شعبه:", chkY) : chkY += 30
            txtCheckAccountNumber = AddGroupRow(grpCheck, "شماره حساب:", chkY) : chkY += 30

            ' سطر تاریخ سررسید چک همراه با دکمه تقویم
            Dim lblCheckDue As New Label() With {
                .Text = "تاریخ سررسید چک:",
                .Location = New Point(230, chkY),
                .Size = New Size(205, 22),
                .TextAlign = ContentAlignment.MiddleLeft,
                .Font = New Font("Tahoma", 8.5!, FontStyle.Regular),
                .ForeColor = Color.Black
            }
            txtCheckDueDate = New TextBox() With {
                .Location = New Point(50, chkY),
                .Size = New Size(175, 22),
                .Text = ToPersian(DateTime.Today.AddMonths(1)),
                .Font = New Font("Tahoma", 9, FontStyle.Regular)
            }
            btnCalCheckDueDate = New Button() With {
                .Text = "...",
                .Location = New Point(15, chkY),
                .Size = New Size(30, 23),
                .FlatStyle = FlatStyle.System
            }
            AddHandler btnCalCheckDueDate.Click, Sub(s, e) ShowCalendarForTextBox(txtCheckDueDate)
            grpCheck.Controls.Add(lblCheckDue)
            grpCheck.Controls.Add(txtCheckDueDate)
            grpCheck.Controls.Add(btnCalCheckDueDate)
            chkY += 30

            txtCheckNotes = AddGroupRow(grpCheck, "یادداشت:", chkY)

            ' دکمه‌ها
            btnOK = New Button() With {
                .Text = "تأیید و ذخیره",
                .Size = New Size(110, 32),
                .Location = New Point(360, 480),
                .BackColor = Color.FromArgb(0, 120, 215),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9, FontStyle.Bold)
            }
            btnCancel = New Button() With {
                .Text = "انصراف",
                .Size = New Size(90, 32),
                .Location = New Point(260, 480),
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnOK.Click, AddressOf BtnOK_Click
            AddHandler btnCancel.Click, Sub(s, e) Me.DialogResult = DialogResult.Cancel

            Me.Controls.AddRange(New Control() {
                lblType, cmbPayType, lblAmt, txtAmount,
                lblPD, txtPayDate, btnCalPayDate,
                lblDueDate, txtDueDate, btnCalDueDate,
                lblDesc, txtDescription,
                grpCash, grpBank, grpCheck, btnOK, btnCancel
            })

            CmbPayType_Changed(Nothing, Nothing)
        End Sub

        Private Sub LoadPaymentForEdit(paymentId As Integer)
            Dim payRow = _paymentSvc.GetPaymentById(paymentId)
            If payRow Is Nothing Then Return

            Dim pType = Convert.ToString(payRow("PaymentType"))
            _oldPaymentAmount = Convert.ToDecimal(payRow("Amount"))
            txtAmount.Text = _oldPaymentAmount.ToString("N0")

            If cmbPayType.Items.Contains(pType) Then
                cmbPayType.SelectedItem = pType
            End If

            If Not payRow.IsNull("PaymentDate") Then
                Dim pDate = Convert.ToDateTime(payRow("PaymentDate"))
                txtPayDate.Text = ToPersian(pDate)
            End If

            If Not payRow.IsNull("DueDate") Then
                Dim dDate = Convert.ToDateTime(payRow("DueDate"))
                txtDueDate.Text = ToPersian(dDate)
            End If

            txtDescription.Text = Convert.ToString(If(payRow.IsNull("Description"), "", payRow("Description")))

            ' بارگذاری مشخصات چک در صورت وجود
            If pType = "چک" Then
                Dim checksDt = _paymentSvc.GetChecksForPayment(paymentId)
                If checksDt IsNot Nothing AndAlso checksDt.Rows.Count > 0 Then
                    Dim cRow = checksDt.Rows(0)
                    txtCheckNumber.Text = Convert.ToString(If(cRow.IsNull("CheckNumber"), "", cRow("CheckNumber")))
                    txtCheckBankName.Text = Convert.ToString(If(cRow.IsNull("BankName"), "", cRow("BankName")))
                    txtCheckBranchName.Text = Convert.ToString(If(cRow.IsNull("BranchName"), "", cRow("BranchName")))
                    txtCheckAccountNumber.Text = Convert.ToString(If(cRow.IsNull("AccountNumber"), "", cRow("AccountNumber")))
                    If Not cRow.IsNull("DueDate") Then
                        txtCheckDueDate.Text = ToPersian(Convert.ToDateTime(cRow("DueDate")))
                    End If
                    txtCheckNotes.Text = Convert.ToString(If(cRow.IsNull("Notes"), "", cRow("Notes")))
                End If
            End If
        End Sub

        Private Function AddGroupRow(parent As GroupBox, labelText As String, y As Integer, Optional defaultValue As String = "") As TextBox
            Dim lbl As New Label() With {
                .Text = labelText,
                .Location = New Point(230, y),
                .Size = New Size(205, 22),
                .TextAlign = ContentAlignment.MiddleLeft,
                .Font = New Font("Tahoma", 8.5!, FontStyle.Regular),
                .ForeColor = Color.Black
            }
            Dim txt As New TextBox() With {
                .Location = New Point(15, y),
                .Size = New Size(210, 22),
                .Text = defaultValue,
                .Font = New Font("Tahoma", 9, FontStyle.Regular)
            }
            parent.Controls.Add(lbl)
            parent.Controls.Add(txt)
            Return txt
        End Function

        Private Sub ShowCalendarForTextBox(targetTxt As TextBox)
            Dim anchor = targetTxt.PointToScreen(New Point(0, targetTxt.Height))
            Using cal As New PersianCalendarForm(targetTxt.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    targetTxt.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub TxtAmount_TextChanged(sender As Object, e As EventArgs)
            RemoveHandler txtAmount.TextChanged, AddressOf TxtAmount_TextChanged
            Try
                Dim raw = txtAmount.Text.Replace(",", "").Trim()
                Dim val As Decimal
                If Decimal.TryParse(raw, val) AndAlso val > 0 Then
                    txtAmount.Text = val.ToString("N0")
                    txtAmount.SelectionStart = txtAmount.Text.Length
                End If
            Catch
            End Try
            AddHandler txtAmount.TextChanged, AddressOf TxtAmount_TextChanged
        End Sub

        Private Sub CmbPayType_Changed(sender As Object, e As EventArgs)
            Dim selectedType = cmbPayType.SelectedItem?.ToString()
            Dim isCash = (selectedType = "نقد")
            Dim isBank = (selectedType = "واریز به بانک")
            Dim isCheck = (selectedType = "چک")
            Dim isDebt = (selectedType = "بدهی")

            grpCash.Visible = isCash
            grpBank.Visible = isBank
            grpCheck.Visible = isCheck

            lblDueDate.Visible = isDebt
            txtDueDate.Visible = isDebt
            btnCalDueDate.Visible = isDebt

            Dim btnY As Integer = 0
            If isCash OrElse isBank Then
                grpCash.Location = New Point(20, 195)
                grpBank.Location = New Point(20, 195)
                btnY = 390
                Me.Height = 470
            ElseIf isCheck Then
                grpCheck.Location = New Point(20, 195)
                btnY = 450
                Me.Height = 530
            Else
                btnY = 220
                Me.Height = 300
            End If

            If btnOK IsNot Nothing Then btnOK.Location = New Point(360, btnY)
            If btnCancel IsNot Nothing Then btnCancel.Location = New Point(260, btnY)
        End Sub

        Private Sub BtnOK_Click(sender As Object, e As EventArgs)
            Dim payType = cmbPayType.SelectedItem?.ToString()
            Dim amount As Decimal
            If Not Decimal.TryParse(txtAmount.Text.Replace(",", ""), amount) OrElse amount <= 0 Then
                MessageBox.Show("لطفاً مبلغ معتبر وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAmount.Focus()
                Return
            End If

            ' محاسبه مجموع با احتساب این رکورد و مقایسه با مبلغ فاکتور
            Dim currentTotalAllocated = _totalAlreadyAllocated - _oldPaymentAmount
            Dim newTotalAllocated = currentTotalAllocated + amount

            If newTotalAllocated > _invoiceTotal Then
                Dim totalA = newTotalAllocated.ToString("N0")
                Dim invoiceB = _invoiceTotal.ToString("N0")
                Dim excessC = (newTotalAllocated - _invoiceTotal).ToString("N0")

                Dim errorMsg = $"با احتساب مبلغ وارد شده در این رسید ، مجموع مبالغ وارد شده در رسیدهای تسویه {totalA} ریال می شود که از مبلغ فاکتور که {invoiceB} ریال می باشد ، مبلغ {excessC} ریال بیشتر می شود."
                MessageBox.Show(errorMsg, "خطای سقف مبلغ تسویه", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtAmount.Focus()
                txtAmount.SelectAll()
                Return
            End If

            Try
                Dim payDate As Date = ParsePersianDate(txtPayDate.Text.Trim())
                Dim dueDate As Date? = Nothing
                If payType = "بدهی" Then
                    dueDate = ParsePersianDate(txtDueDate.Text.Trim())
                End If

                Dim desc = txtDescription.Text.Trim()

                ' ساخت توضیحات کامل بر اساس GroupBox انتخاب شده
                If payType = "نقد" Then
                    Dim parts As New List(Of String)
                    If Not String.IsNullOrWhiteSpace(txtCashReceiptNo.Text) Then parts.Add("رسید صندوق: " & txtCashReceiptNo.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtCashierName.Text) Then parts.Add("صندوقدار: " & txtCashierName.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtReceiverName.Text) Then parts.Add("دریافت‌کننده: " & txtReceiverName.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtCashNotes.Text) Then parts.Add("یادداشت: " & txtCashNotes.Text.Trim())
                    If parts.Count > 0 Then
                        desc = String.Join(" | ", parts) & If(String.IsNullOrEmpty(desc), "", " - " & desc)
                    End If
                ElseIf payType = "واریز به بانک" Then
                    Dim parts As New List(Of String)
                    If Not String.IsNullOrWhiteSpace(txtBankReceiptNo.Text) Then parts.Add("فیش/حواله: " & txtBankReceiptNo.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtBankName.Text) Then parts.Add("بانک: " & txtBankName.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtAccountCardNo.Text) Then parts.Add("حساب/کارت: " & txtAccountCardNo.Text.Trim())
                    If Not String.IsNullOrWhiteSpace(txtBankNotes.Text) Then parts.Add("یادداشت: " & txtBankNotes.Text.Trim())
                    If parts.Count > 0 Then
                        desc = String.Join(" | ", parts) & If(String.IsNullOrEmpty(desc), "", " - " & desc)
                    End If
                End If

                If _editPaymentId.HasValue Then
                    _paymentSvc.UpdatePayment(_editPaymentId.Value, payType, amount, payDate, dueDate, desc)
                Else
                    Dim payId = _paymentSvc.AddPayment(_invoiceId, payType, amount, payDate, dueDate, desc)
                    If payType = "چک" Then
                        If String.IsNullOrWhiteSpace(txtCheckNumber.Text) Then
                            MessageBox.Show("شماره چک الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                        Dim chkDue As Date = ParsePersianDate(txtCheckDueDate.Text.Trim())
                        _paymentSvc.AddCheck(payId, txtCheckNumber.Text.Trim(),
                                             txtCheckBankName.Text.Trim(), txtCheckBranchName.Text.Trim(),
                                             txtCheckAccountNumber.Text.Trim(), amount,
                                             chkDue, txtCheckNotes.Text.Trim())
                    End If
                End If

                Me.DialogResult = DialogResult.OK
            Catch ex As Exception
                MessageBox.Show("خطا در ثبت پرداخت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class

End Namespace
