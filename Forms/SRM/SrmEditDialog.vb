Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.SRM
    Public Class SrmEditDialog
        Inherits Form

        Private txtSupplierCode As TextBox
        Private txtSupplierName As TextBox
        Private cboCategory As ComboBox
        Private cboGrade As ComboBox
        Private txtEconCode As TextBox
        Private txtPhone As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _srmSvc As SrmService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _srmSvc = New SrmService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🤝 ثبت تامین‌کننده جدید در سیستم مدیریت ارتباط با تامین‌کنندگان (SRM)"
            Me.Size = New Size(520, 360)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد تامین‌کننده:", .Location = New Point(370, 25), .AutoSize = True}
            txtSupplierCode = New TextBox With {.Location = New Point(170, 22), .Size = New Size(180, 26), .Text = "SUP-" & (Environment.TickCount Mod 10000).ToString()}

            Dim lblName As New Label With {.Text = "نام تامین‌کننده/شرکت:", .Location = New Point(370, 65), .AutoSize = True}
            txtSupplierName = New TextBox With {.Location = New Point(30, 62), .Size = New Size(320, 26), .Text = "شرکت پتروشیمی شازند - اراک"}

            Dim lblCategory As New Label With {.Text = "رسته/گروه کالا:", .Location = New Point(370, 105), .AutoSize = True}
            cboCategory = New ComboBox With {.Location = New Point(170, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCategory.Items.AddRange(New Object() {"مواد اولیه پلیمری", "مواد اولیه فلزی", "قطعات یدکی و ماشین‌آلات", "ملزومات بسته‌بندی", "خدمات و پیمانکاری"})
            cboCategory.SelectedIndex = 0

            Dim lblGrade As New Label With {.Text = "گرید ارزیابی اولیه:", .Location = New Point(370, 145), .AutoSize = True}
            cboGrade = New ComboBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboGrade.Items.AddRange(New Object() {"گرید A", "گرید B", "گرید C", "گرید D (غیرمجاز)"})
            cboGrade.SelectedIndex = 0

            Dim lblEconCode As New Label With {.Text = "کد اقتصادی/شناسه ملی:", .Location = New Point(370, 185), .AutoSize = True}
            txtEconCode = New TextBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .Text = "10100412890"}

            Dim lblPhone As New Label With {.Text = "تلفن تماس:", .Location = New Point(370, 225), .AutoSize = True}
            txtPhone = New TextBox With {.Location = New Point(170, 222), .Size = New Size(180, 26), .Text = "021-88776655"}

            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(130, 26), .Visible = False}

            btnSave = New Button With {
                .Text = "💾 ثبت اطلاعات تامین‌کننده",
                .Size = New Size(180, 36),
                .Location = New Point(170, 270),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(60, 270),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtSupplierCode, lblName, txtSupplierName, lblCategory, cboCategory,
                lblGrade, cboGrade, lblEconCode, txtEconCode, lblPhone, txtPhone,
                btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtSupplierName.Text) Then
                MessageBox.Show("لطفاً نام تامین‌کننده را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _srmSvc.SaveSupplier(
                _id, _companyID, txtSupplierCode.Text, txtSupplierName.Text,
                cboCategory.SelectedItem.ToString(), cboGrade.SelectedItem.ToString(),
                txtEconCode.Text, txtPhone.Text, "تامین‌کننده تایید شده"
            )

            MessageBox.Show("اطلاعات تامین‌کننده با موفقیت در سیستم SRM ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
