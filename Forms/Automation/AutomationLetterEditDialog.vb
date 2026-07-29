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
    Public Class AutomationLetterEditDialog
        Inherits Form

        Private txtLetterNo As TextBox
        Private txtLetterDate As TextBox
        Private cmbLetterType As ComboBox
        Private txtSubject As TextBox
        Private txtSenderInfo As TextBox
        Private txtReceiverInfo As TextBox
        Private cmbPriority As ComboBox
        Private cmbConfidentiality As ComboBox
        Private txtContentBody As TextBox
        Private cmbStatus As ComboBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _autoSvc As AutomationService
        Private _letterID As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional letterID As Integer = 0)
            _companyID = companyID
            _letterID = letterID
            _autoSvc = New AutomationService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_letterID <= 0, "📨 ثبت نامه اداری جدید", "📨 ویرایش نامه اداری")
            Me.Size = New Size(580, 560)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblType As New Label With {.Text = "نوع مکاتبه:", .Location = New Point(420, 25), .AutoSize = True}
            cmbLetterType = New ComboBox With {.Location = New Point(220, 22), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbLetterType.Items.AddRange(New Object() {"نامه وارده 📥", "نامه صادره 📤", "یادداشت داخلی 📝"})
            cmbLetterType.SelectedIndex = 0

            Dim lblNo As New Label With {.Text = "شماره اندیکاتور:", .Location = New Point(420, 65), .AutoSize = True}
            txtLetterNo = New TextBox With {.Location = New Point(220, 62), .Size = New Size(180, 26)}

            Dim lblDate As New Label With {.Text = "تاریخ نامه:", .Location = New Point(420, 105), .AutoSize = True}
            txtLetterDate = New TextBox With {.Location = New Point(220, 102), .Size = New Size(180, 26), .Text = PersianDateHelper.ToPersian(DateTime.Now)}

            Dim lblSubj As New Label With {.Text = "موضوع نامه:", .Location = New Point(420, 145), .AutoSize = True}
            txtSubject = New TextBox With {.Location = New Point(30, 142), .Size = New Size(370, 26)}

            Dim lblSender As New Label With {.Text = "فرستنده:", .Location = New Point(420, 185), .AutoSize = True}
            txtSenderInfo = New TextBox With {.Location = New Point(30, 182), .Size = New Size(370, 26)}

            Dim lblRecv As New Label With {.Text = "گیرنده اصلی:", .Location = New Point(420, 225), .AutoSize = True}
            txtReceiverInfo = New TextBox With {.Location = New Point(30, 222), .Size = New Size(370, 26)}

            Dim lblPrio As New Label With {.Text = "اولویت:", .Location = New Point(420, 265), .AutoSize = True}
            cmbPriority = New ComboBox With {.Location = New Point(220, 262), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbPriority.Items.AddRange(New Object() {"عادی", "فوری ⚡", "آنی 🚨"})
            cmbPriority.SelectedIndex = 0

            Dim lblConf As New Label With {.Text = "سطح محرمانه بودن:", .Location = New Point(420, 305), .AutoSize = True}
            cmbConfidentiality = New ComboBox With {.Location = New Point(220, 302), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbConfidentiality.Items.AddRange(New Object() {"عادی", "محرمانه 🔒", "سری 🔑"})
            cmbConfidentiality.SelectedIndex = 0

            Dim lblStatus As New Label With {.Text = "وضعیت اقدام:", .Location = New Point(420, 345), .AutoSize = True}
            cmbStatus = New ComboBox With {.Location = New Point(220, 342), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbStatus.Items.AddRange(New Object() {"در دست اقدام", "ارجاع شده", "بایگانی شده", "پاسخ داده شده"})
            cmbStatus.SelectedIndex = 0

            Dim lblBody As New Label With {.Text = "متن/شرح نامه:", .Location = New Point(420, 385), .AutoSize = True}
            txtContentBody = New TextBox With {.Location = New Point(30, 382), .Size = New Size(370, 75), .Multiline = True, .ScrollBars = ScrollBars.Vertical}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(260, 470),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(150, 470),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblType, cmbLetterType, lblNo, txtLetterNo, lblDate, txtLetterDate,
                lblSubj, txtSubject, lblSender, txtSenderInfo, lblRecv, txtReceiverInfo,
                lblPrio, cmbPriority, lblConf, cmbConfidentiality, lblStatus, cmbStatus,
                lblBody, txtContentBody, btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            If _letterID > 0 Then
                Dim row = _autoSvc.GetLetterById(_letterID)
                If row IsNot Nothing Then
                    txtLetterNo.Text = Convert.ToString(row("LetterNo"))
                    txtLetterDate.Text = Convert.ToString(row("LetterDate"))
                    txtSubject.Text = Convert.ToString(row("Subject"))
                    txtSenderInfo.Text = Convert.ToString(row("SenderInfo"))
                    txtReceiverInfo.Text = Convert.ToString(row("ReceiverInfo"))
                    txtContentBody.Text = Convert.ToString(row("ContentBody"))

                    Dim lType = Convert.ToInt32(If(IsDBNull(row("LetterType")), 1, row("LetterType")))
                    If lType >= 1 AndAlso lType <= 3 Then cmbLetterType.SelectedIndex = lType - 1

                    Dim prio = Convert.ToInt32(If(IsDBNull(row("Priority")), 1, row("Priority")))
                    If prio >= 1 AndAlso prio <= 3 Then cmbPriority.SelectedIndex = prio - 1

                    Dim conf = Convert.ToInt32(If(IsDBNull(row("Confidentiality")), 1, row("Confidentiality")))
                    If conf >= 1 AndAlso conf <= 3 Then cmbConfidentiality.SelectedIndex = conf - 1

                    Dim stStr = Convert.ToString(row("Status"))
                    If cmbStatus.Items.Contains(stStr) Then cmbStatus.SelectedItem = stStr
                End If
            Else
                txtLetterNo.Text = "1405/و/" & (Environment.TickCount Mod 10000).ToString()
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtSubject.Text) Then
                MessageBox.Show("لطفاً موضوع نامه را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim lType = cmbLetterType.SelectedIndex + 1
            Dim prio = cmbPriority.SelectedIndex + 1
            Dim conf = cmbConfidentiality.SelectedIndex + 1
            Dim statusStr = cmbStatus.SelectedItem.ToString()

            _autoSvc.SaveLetter(_letterID, _companyID, txtLetterNo.Text, txtLetterDate.Text, lType, txtSubject.Text, txtSenderInfo.Text, txtReceiverInfo.Text, prio, conf, txtContentBody.Text, statusStr)

            MessageBox.Show("نامه اداری با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
