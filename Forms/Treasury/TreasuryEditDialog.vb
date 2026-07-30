Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Treasury
    Public Class TreasuryEditDialog
        Inherits Form

        Private txtCode As TextBox
        Private txtTitle As TextBox
        Private cmbType As ComboBox
        Private txtAccNo As TextBox
        Private txtShaba As TextBox
        Private txtBalance As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _treasurySvc As TreasuryService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _treasurySvc = New TreasuryService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "💰 ثبت حساب بانکی / صندوق / تنخواه جدید"
            Me.Size = New Size(520, 420)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد حساب:", .Location = New Point(380, 25), .AutoSize = True}
            txtCode = New TextBox With {.Location = New Point(180, 22), .Size = New Size(180, 26), .Text = "BNK-" & (Environment.TickCount Mod 1000).ToString()}

            Dim lblTitle As New Label With {.Text = "عنوان بانک / صندوق:", .Location = New Point(380, 65), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 62), .Size = New Size(330, 26)}

            Dim lblType As New Label With {.Text = "نوع حساب:", .Location = New Point(380, 105), .AutoSize = True}
            cmbType = New ComboBox With {.Location = New Point(180, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbType.Items.AddRange(New Object() {"بانک", "صندوق", "تنخواه", "درگاه پرداخت POS/IPG"})
            cmbType.SelectedIndex = 0

            Dim lblAccNo As New Label With {.Text = "شماره حساب:", .Location = New Point(380, 145), .AutoSize = True}
            txtAccNo = New TextBox With {.Location = New Point(180, 142), .Size = New Size(180, 26)}

            Dim lblShaba As New Label With {.Text = "شماره شبا (IR):", .Location = New Point(380, 185), .AutoSize = True}
            txtShaba = New TextBox With {.Location = New Point(30, 182), .Size = New Size(330, 26)}

            Dim lblBal As New Label With {.Text = "موجودی اولیه (ریال):", .Location = New Point(380, 225), .AutoSize = True}
            txtBalance = New TextBox With {.Location = New Point(180, 222), .Size = New Size(180, 26), .Text = "0"}

            Dim lblNotes As New Label With {.Text = "توضیحات:", .Location = New Point(380, 265), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 262), .Size = New Size(330, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(230, 320),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(120, 320),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtCode, lblTitle, txtTitle, lblType, cmbType,
                lblAccNo, txtAccNo, lblShaba, txtShaba, lblBal, txtBalance,
                lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان بانک یا صندوق را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim bal As Double = 0
            Double.TryParse(txtBalance.Text, bal)

            _treasurySvc.SaveCashBank(
                _id, _companyID, txtCode.Text, txtTitle.Text,
                cmbType.SelectedItem.ToString(), txtAccNo.Text, txtShaba.Text, bal, txtNotes.Text
            )

            MessageBox.Show("حساب جدید خزانه‌داری با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
