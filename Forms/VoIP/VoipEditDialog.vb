Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.VoIP
    Public Class VoipEditDialog
        Inherits Form

        Private txtCallerNo As TextBox
        Private txtCustomer As TextBox
        Private txtOperator As TextBox
        Private cboDirection As ComboBox
        Private txtDuration As TextBox
        Private cboOutcome As ComboBox
        Private txtNote As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _voipSvc As VoipService
        Private _companyID As Integer

        Public Sub New(companyID As Integer)
            _companyID = companyID
            _voipSvc = New VoipService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📞 ثبت دستی لاگ تماس جدید در CRM"
            Me.Size = New Size(540, 400)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 252)

            Dim y = 22
            Dim gap = 44

            Dim lblCaller As New Label With {.Text = "شماره تماس‌گیرنده:", .Location = New Point(360, y), .AutoSize = True}
            txtCallerNo = New TextBox With {.Location = New Point(140, y - 2), .Size = New Size(210, 26), .Text = "09123456789"}
            y += gap

            Dim lblCust As New Label With {.Text = "نام مشتری / شرکت:", .Location = New Point(360, y), .AutoSize = True}
            txtCustomer = New TextBox With {.Location = New Point(140, y - 2), .Size = New Size(210, 26), .Text = ""}
            y += gap

            Dim lblOper As New Label With {.Text = "نام اپراتور:", .Location = New Point(360, y), .AutoSize = True}
            txtOperator = New TextBox With {.Location = New Point(140, y - 2), .Size = New Size(210, 26), .Text = ""}
            y += gap

            Dim lblDir As New Label With {.Text = "جهت تماس:", .Location = New Point(360, y), .AutoSize = True}
            cboDirection = New ComboBox With {.Location = New Point(250, y - 2), .Size = New Size(100, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboDirection.Items.AddRange(New Object() {"ورودی", "خروجی"})
            cboDirection.SelectedIndex = 0
            y += gap

            Dim lblDur As New Label With {.Text = "مدت مکالمه (ثانیه):", .Location = New Point(360, y), .AutoSize = True}
            txtDuration = New TextBox With {.Location = New Point(250, y - 2), .Size = New Size(100, 26), .Text = "0"}
            y += gap

            Dim lblOut As New Label With {.Text = "نتیجه تماس:", .Location = New Point(360, y), .AutoSize = True}
            cboOutcome = New ComboBox With {.Location = New Point(140, y - 2), .Size = New Size(210, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboOutcome.Items.AddRange(New Object() {"فروش انجام شد", "قرار ملاقات گذاشته شد", "پیگیری شکایت", "بی‌نتیجه", "بی‌پاسخ (صف رها شد)", "پیام گرفته شد"})
            cboOutcome.SelectedIndex = 0
            y += gap

            Dim lblNote As New Label With {.Text = "یادداشت تماس:", .Location = New Point(360, y), .AutoSize = True}
            txtNote = New TextBox With {.Location = New Point(40, y - 2), .Size = New Size(310, 52), .Multiline = True, .ScrollBars = ScrollBars.Vertical, .Text = ""}
            y += 62

            btnSave = New Button With {
                .Text = "💾 ثبت لاگ تماس",
                .Size = New Size(180, 36), .Location = New Point(180, y),
                .BackColor = Color.FromArgb(13, 71, 161), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(110, 36), .Location = New Point(40, y),
                .BackColor = Color.FromArgb(183, 28, 28), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCaller, txtCallerNo, lblCust, txtCustomer, lblOper, txtOperator,
                lblDir, cboDirection, lblDur, txtDuration, lblOut, cboOutcome,
                lblNote, txtNote, btnSave, btnCancel
            })
            Me.ClientSize = New Size(540, y + 55)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtCallerNo.Text) Then
                MessageBox.Show("لطفاً شماره تماس را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim dur As Integer = 0
            Integer.TryParse(txtDuration.Text, dur)
            _voipSvc.LogCall(_companyID, txtCallerNo.Text, txtCustomer.Text, txtOperator.Text,
                             cboDirection.SelectedItem.ToString(), dur,
                             cboOutcome.SelectedItem.ToString(), txtNote.Text)
            MessageBox.Show("لاگ تماس با موفقیت در CRM ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
