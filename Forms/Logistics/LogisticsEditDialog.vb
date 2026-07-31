Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Logistics
    Public Class LogisticsEditDialog
        Inherits Form

        Private txtPlateNumber As TextBox
        Private cboType As ComboBox
        Private txtDriver As TextBox
        Private txtCapacity As TextBox
        Private cboOwnership As ComboBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _logSvc As LogisticsService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _logSvc = New LogisticsService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🚚 ثبت خودرو و وسیله نقلیه جدید در ناوگان توزیع"
            Me.Size = New Size(520, 360)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblPlate As New Label With {.Text = "شماره پلاک خودرو:", .Location = New Point(370, 25), .AutoSize = True}
            txtPlateNumber = New TextBox With {.Location = New Point(170, 22), .Size = New Size(180, 26), .Text = "45-ج-812-ایران 11"}

            Dim lblType As New Label With {.Text = "نوع وسیله نقلیه:", .Location = New Point(370, 65), .AutoSize = True}
            cboType = New ComboBox With {.Location = New Point(170, 62), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboType.Items.AddRange(New Object() {"کامیونت ایسوزو", "وانت نیسان", "وانت پراید", "کامیون سنگین", "تریلی یخچال‌دار"})
            cboType.SelectedIndex = 0

            Dim lblDriver As New Label With {.Text = "نام راننده / موزع:", .Location = New Point(370, 105), .AutoSize = True}
            txtDriver = New TextBox With {.Location = New Point(30, 102), .Size = New Size(320, 26), .Text = "جناب آقای رضایی"}

            Dim lblCapacity As New Label With {.Text = "ظرفیت بارگیری (کیلوگرم):", .Location = New Point(370, 145), .AutoSize = True}
            txtCapacity = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "3500"}

            Dim lblOwnership As New Label With {.Text = "نوع مالکیت:", .Location = New Point(370, 185), .AutoSize = True}
            cboOwnership = New ComboBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboOwnership.Items.AddRange(New Object() {"شرکتی", "استیجاری", "پیمانکاری"})
            cboOwnership.SelectedIndex = 0

            Dim lblNotes As New Label With {.Text = "توضیحات و مشخصات:", .Location = New Point(370, 225), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(320, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت شناسنامه خودرو",
                .Size = New Size(160, 36),
                .Location = New Point(190, 270),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(80, 270),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblPlate, txtPlateNumber, lblType, cboType, lblDriver, txtDriver,
                lblCapacity, txtCapacity, lblOwnership, cboOwnership,
                lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtPlateNumber.Text) Then
                MessageBox.Show("لطفاً شماره پلاک خودرو را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim capKg As Double = 3500
            Double.TryParse(txtCapacity.Text.Replace(",", ""), capKg)

            _logSvc.SaveVehicle(
                _id, _companyID, txtPlateNumber.Text, cboType.SelectedItem.ToString(),
                txtDriver.Text, capKg, cboOwnership.SelectedItem.ToString(), txtNotes.Text
            )

            MessageBox.Show("شناسنامه وسیله نقلیه ناوگان توزیع با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
