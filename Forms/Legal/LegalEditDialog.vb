Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Legal
    Public Class LegalEditDialog
        Inherits Form

        Private txtTitle As TextBox
        Private txtClaimant As TextBox
        Private txtDefendant As TextBox
        Private txtCourt As TextBox
        Private txtClaimAmount As TextBox
        Private cboStatus As ComboBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _legalSvc As LegalService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _legalSvc = New LegalService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "⚖️ تشکیل پرونده قضایی و حقوقی جدید"
            Me.Size = New Size(520, 350)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblTitle As New Label With {.Text = "عنوان موضوع دعوا / شکایت:", .Location = New Point(350, 25), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 22), .Size = New Size(310, 26), .Text = "دعوای مطالباتی فاکتور فروش و خسارت تاخیر"}

            Dim lblClaimant As New Label With {.Text = "خواهان (شاکی):", .Location = New Point(350, 65), .AutoSize = True}
            txtClaimant = New TextBox With {.Location = New Point(170, 62), .Size = New Size(170, 26), .Text = "شرکت نگار"}

            Dim lblDef As New Label With {.Text = "خوانده (متشاکی):", .Location = New Point(350, 105), .AutoSize = True}
            txtDefendant = New TextBox With {.Location = New Point(170, 102), .Size = New Size(170, 26), .Text = "شرکت بازرگانی پارس گستر"}

            Dim lblCourt As New Label With {.Text = "مرجع قضایی / شعبه دادگاه:", .Location = New Point(350, 145), .AutoSize = True}
            txtCourt = New TextBox With {.Location = New Point(30, 142), .Size = New Size(310, 26), .Text = "دادگاه عمومی حقوقی مجتمع شهید بهشتی شعبه ۱۰۵"}

            Dim lblAmt As New Label With {.Text = "مبلغ خواسته دعوا (ریال):", .Location = New Point(350, 185), .AutoSize = True}
            txtClaimAmount = New TextBox With {.Location = New Point(170, 182), .Size = New Size(170, 26), .Text = "2500000000"}

            Dim lblStatus As New Label With {.Text = "وضعیت پرونده:", .Location = New Point(350, 225), .AutoSize = True}
            cboStatus = New ComboBox With {.Location = New Point(170, 222), .Size = New Size(170, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboStatus.Items.AddRange(New Object() {"در حال رسیدگی", "صدور حکم بدوی به نفع", "صدور رای علیه", "مختومه"})
            cboStatus.SelectedIndex = 0

            btnSave = New Button With {
                .Text = "💾 ثبت پرونده حقوقی",
                .Size = New Size(170, 36),
                .Location = New Point(170, 265),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(60, 265),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblTitle, txtTitle, lblClaimant, txtClaimant, lblDef, txtDefendant,
                lblCourt, txtCourt, lblAmt, txtClaimAmount, lblStatus, cboStatus, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان موضوع دعوا را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim amt As Double = 0
            Double.TryParse(txtClaimAmount.Text.Replace(",", ""), amt)

            _legalSvc.SaveCase(
                _id, _companyID, txtTitle.Text, txtClaimant.Text,
                txtDefendant.Text, txtCourt.Text, amt, cboStatus.SelectedItem.ToString()
            )

            MessageBox.Show("پرونده قضایی با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
