Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.PM
    Public Class PmEditDialog
        Inherits Form

        Private txtAssetCode As TextBox
        Private txtAssetName As TextBox
        Private cboCategory As ComboBox
        Private txtLocation As TextBox
        Private txtCostCenter As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _pmSvc As PmService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _pmSvc = New PmService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔧 ثبت شناسنامه ماشین‌آلات و تجهیزات جدید"
            Me.Size = New Size(520, 360)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد تجهیز/ماشین:", .Location = New Point(370, 25), .AutoSize = True}
            txtAssetCode = New TextBox With {.Location = New Point(170, 22), .Size = New Size(180, 26), .Text = "EQ-" & (Environment.TickCount Mod 10000).ToString()}

            Dim lblName As New Label With {.Text = "نام دستگاه/تجهیز:", .Location = New Point(370, 65), .AutoSize = True}
            txtAssetName = New TextBox With {.Location = New Point(30, 62), .Size = New Size(320, 26), .Text = "دستگاه تراش CNC - دکمه‌ای"}

            Dim lblCategory As New Label With {.Text = "دسته‌بندی تجهیز:", .Location = New Point(370, 105), .AutoSize = True}
            cboCategory = New ComboBox With {.Location = New Point(170, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCategory.Items.AddRange(New Object() {"ماشین‌آلات اصلی", "خطوط مونتاژ", "تجهیزات هیدرولیک", "تجهیزات برق و الکترونیک", "تأسیسات جانبی"})
            cboCategory.SelectedIndex = 0

            Dim lblLocation As New Label With {.Text = "موقعیت استقرار:", .Location = New Point(370, 145), .AutoSize = True}
            txtLocation = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "سالن کارگاهی شماره ۲"}

            Dim lblCostCenter As New Label With {.Text = "مرکز هزینه:", .Location = New Point(370, 185), .AutoSize = True}
            txtCostCenter = New TextBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .Text = "مرکز هزینه تراش‌کاری"}

            Dim lblNotes As New Label With {.Text = "توضیحات و مشخصات:", .Location = New Point(370, 225), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(320, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت شناسنامه تجهیز",
                .Size = New Size(150, 36),
                .Location = New Point(200, 270),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(90, 270),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtAssetCode, lblName, txtAssetName, lblCategory, cboCategory,
                lblLocation, txtLocation, lblCostCenter, txtCostCenter,
                lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtAssetName.Text) Then
                MessageBox.Show("لطفاً نام دستگاه/تجهیز را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _pmSvc.SaveAsset(
                _id, _companyID, txtAssetCode.Text, txtAssetName.Text,
                cboCategory.SelectedItem.ToString(), txtLocation.Text,
                txtCostCenter.Text, txtNotes.Text
            )

            MessageBox.Show("شناسنامه ماشین‌آلات و تجهیزات با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
