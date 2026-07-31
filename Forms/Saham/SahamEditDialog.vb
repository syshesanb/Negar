Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Saham
    Public Class SahamEditDialog
        Inherits Form

        Private txtFullName As TextBox
        Private txtNationalID As TextBox
        Private cboType As ComboBox
        Private txtShareCount As TextBox
        Private txtBankAccount As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _sahamSvc As SahamService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _sahamSvc = New SahamService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🏛️ ثبت و تشکیل شناسنامه سهامدار جدید"
            Me.Size = New Size(520, 360)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblName As New Label With {.Text = "نام و نام خانوادگی / نام شرکت:", .Location = New Point(360, 25), .AutoSize = True}
            txtFullName = New TextBox With {.Location = New Point(30, 22), .Size = New Size(320, 26), .Text = "مهندس محمدحسین رضایی"}

            Dim lblNat As New Label With {.Text = "کد ملی / شناسه ملی:", .Location = New Point(360, 65), .AutoSize = True}
            txtNationalID = New TextBox With {.Location = New Point(170, 62), .Size = New Size(180, 26), .Text = "0058493021"}

            Dim lblType As New Label With {.Text = "نوع سهامدار:", .Location = New Point(360, 105), .AutoSize = True}
            cboType = New ComboBox With {.Location = New Point(170, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboType.Items.AddRange(New Object() {"حقیقی", "حقوقی"})
            cboType.SelectedIndex = 0

            Dim lblCount As New Label With {.Text = "تعداد سهام اولیه:", .Location = New Point(360, 145), .AutoSize = True}
            txtShareCount = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "50000"}

            Dim lblBank As New Label With {.Text = "شماره شبا (واریز سود):", .Location = New Point(360, 185), .AutoSize = True}
            txtBankAccount = New TextBox With {.Location = New Point(30, 182), .Size = New Size(320, 26), .Text = "IR880170000000987654321012"}

            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(320, 26), .Visible = False}

            btnSave = New Button With {
                .Text = "💾 ثبت شناسنامه سهامدار",
                .Size = New Size(170, 36),
                .Location = New Point(180, 260),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(70, 260),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblName, txtFullName, lblNat, txtNationalID, lblType, cboType,
                lblCount, txtShareCount, lblBank, txtBankAccount, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtFullName.Text) Then
                MessageBox.Show("لطفاً نام سهامدار را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim count As Double = 10000
            Double.TryParse(txtShareCount.Text.Replace(",", ""), count)

            _sahamSvc.SaveShareholder(
                _id, _companyID, txtFullName.Text, txtNationalID.Text,
                cboType.SelectedItem.ToString(), count, txtBankAccount.Text, "ثبت سهامدار جدید"
            )

            MessageBox.Show("شناسنامه سهامدار با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
