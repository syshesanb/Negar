Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Project
    Public Class ProjectEditDialog
        Inherits Form

        Private txtCode As TextBox
        Private txtTitle As TextBox
        Private txtEmployer As TextBox
        Private txtAmount As TextBox
        Private txtAdvance As TextBox
        Private txtRetention As TextBox
        Private txtInsurance As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _projSvc As ProjectService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _projSvc = New ProjectService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_id <= 0, "🏗️ ثبت شناسنامه پیمان / پروژه جدید", "🏗️ ویرایش پیمان")
            Me.Size = New Size(540, 440)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد پروژه:", .Location = New Point(380, 25), .AutoSize = True}
            txtCode = New TextBox With {.Location = New Point(180, 22), .Size = New Size(180, 26), .Text = "PRJ-" & (Environment.TickCount Mod 1000).ToString()}

            Dim lblTitle As New Label With {.Text = "عنوان کامل پروژه:", .Location = New Point(380, 65), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 62), .Size = New Size(330, 26)}

            Dim lblEmployer As New Label With {.Text = "نام کارفرما:", .Location = New Point(380, 105), .AutoSize = True}
            txtEmployer = New TextBox With {.Location = New Point(30, 102), .Size = New Size(330, 26)}

            Dim lblAmount As New Label With {.Text = "مبلغ اولیه پیمان (ریال):", .Location = New Point(380, 145), .AutoSize = True}
            txtAmount = New TextBox With {.Location = New Point(180, 142), .Size = New Size(180, 26), .Text = "10000000000"}

            Dim lblAdvance As New Label With {.Text = "درصد پیش‌پرداخت (%):", .Location = New Point(380, 185), .AutoSize = True}
            txtAdvance = New TextBox With {.Location = New Point(180, 182), .Size = New Size(180, 26), .Text = "10"}

            Dim lblRetention As New Label With {.Text = "درصد حسن انجام کار (%):", .Location = New Point(380, 225), .AutoSize = True}
            txtRetention = New TextBox With {.Location = New Point(180, 222), .Size = New Size(180, 26), .Text = "10"}

            Dim lblInsurance As New Label With {.Text = "درصد بیمه ماده ۳۸ (%):", .Location = New Point(380, 265), .AutoSize = True}
            txtInsurance = New TextBox With {.Location = New Point(180, 262), .Size = New Size(180, 26), .Text = "5"}

            Dim lblNotes As New Label With {.Text = "توضیحات:", .Location = New Point(380, 305), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 302), .Size = New Size(330, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره پیمان",
                .Size = New Size(140, 36),
                .Location = New Point(220, 350),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(110, 350),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtCode, lblTitle, txtTitle, lblEmployer, txtEmployer,
                lblAmount, txtAmount, lblAdvance, txtAdvance, lblRetention, txtRetention,
                lblInsurance, txtInsurance, lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            If _id > 0 Then
                Dim row = _projSvc.GetProjectById(_id)
                If row IsNot Nothing Then
                    txtCode.Text = Convert.ToString(row("ProjectCode"))
                    txtTitle.Text = Convert.ToString(row("ProjectTitle"))
                    txtEmployer.Text = Convert.ToString(row("EmployerName"))
                    txtAmount.Text = Convert.ToDouble(If(IsDBNull(row("ContractAmount")), 0, row("ContractAmount"))).ToString("N0")
                    txtAdvance.Text = Convert.ToDouble(If(IsDBNull(row("AdvancePercent")), 10, row("AdvancePercent"))).ToString()
                    txtRetention.Text = Convert.ToDouble(If(IsDBNull(row("RetentionPercent")), 10, row("RetentionPercent"))).ToString()
                    txtInsurance.Text = Convert.ToDouble(If(IsDBNull(row("InsurancePercent")), 5, row("InsurancePercent"))).ToString()
                    txtNotes.Text = Convert.ToString(row("Notes"))
                End If
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان کامل پروژه را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim amount As Double = 0
            Double.TryParse(txtAmount.Text.Replace(",", ""), amount)

            Dim adv As Double = 10
            Double.TryParse(txtAdvance.Text, adv)

            Dim ret As Double = 10
            Double.TryParse(txtRetention.Text, ret)

            Dim ins As Double = 5
            Double.TryParse(txtInsurance.Text, ins)

            _projSvc.SaveProject(
                _id, _companyID, txtCode.Text, txtTitle.Text,
                txtEmployer.Text, amount, adv, ret, ins, txtNotes.Text
            )

            MessageBox.Show("شناسنامه پروژه با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
