Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.API
    Public Class ApiEditDialog
        Inherits Form

        Private txtClientName As TextBox
        Private cboAccessLevel As ComboBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _apiSvc As ApiService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _apiSvc = New ApiService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔑 تولید کلید دسترسی جدید (API Key & Secret)"
            Me.Size = New Size(480, 240)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblClient As New Label With {.Text = "نام کلاینت / سامانه متصل:", .Location = New Point(320, 25), .AutoSize = True}
            txtClientName = New TextBox With {.Location = New Point(30, 22), .Size = New Size(280, 26), .Text = "فروشگاه آنلاین شاپ پلاس (WooCommerce)"}

            Dim lblLvl As New Label With {.Text = "سطح دسترسی API:", .Location = New Point(320, 65), .AutoSize = True}
            cboAccessLevel = New ComboBox With {.Location = New Point(130, 62), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboAccessLevel.Items.AddRange(New Object() {"فروشگاه آنلاین", "پوز سیار", "دسترسی کامل (Full Root)"})
            cboAccessLevel.SelectedIndex = 0

            btnSave = New Button With {
                .Text = "🔑 تولید کلید و Secret",
                .Size = New Size(160, 36),
                .Location = New Point(170, 130),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(60, 130),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblClient, txtClientName, lblLvl, cboAccessLevel, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtClientName.Text) Then
                MessageBox.Show("لطفاً نام کلاینت را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _apiSvc.SaveApiKey(_id, _companyID, txtClientName.Text, cboAccessLevel.SelectedItem.ToString())

            MessageBox.Show("کلید دسترسی و API Secret با موفقیت تولید گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
