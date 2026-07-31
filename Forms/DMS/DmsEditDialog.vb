Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.DMS
    Public Class DmsEditDialog
        Inherits Form

        Private txtTitle As TextBox
        Private cboCategory As ComboBox
        Private txtFileName As TextBox
        Private btnBrowse As Button
        Private txtKeywords As TextBox
        Private cboSecurity As ComboBox
        Private txtExpDate As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _dmsSvc As DmsService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _dmsSvc = New DmsService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📁 ثبت و اسکن جدید برگه در بایگانی دیجیتال (DMS)"
            Me.Size = New Size(540, 380)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblTitle As New Label With {.Text = "عنوان سند / پرونده:", .Location = New Point(380, 25), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 22), .Size = New Size(340, 26), .Text = "ضمانت‌نامه بانکی حسن انجام کار"}

            Dim lblCat As New Label With {.Text = "زون / رسته بایگانی:", .Location = New Point(380, 65), .AutoSize = True}
            cboCategory = New ComboBox With {.Location = New Point(180, 62), .Size = New Size(190, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCategory.Items.AddRange(New Object() {"زون مالی و اسناد حسابداری", "زون قراردادها و تضامین", "زون پرسنلی و منابع انسانی", "زون نقشه و مدارک فنی"})
            cboCategory.SelectedIndex = 1

            Dim lblFile As New Label With {.Text = "فایل اسکن شده (مدرک):", .Location = New Point(380, 105), .AutoSize = True}
            txtFileName = New TextBox With {.Location = New Point(130, 102), .Size = New Size(240, 26), .Text = "Bank_Guarantee_1405.pdf", .ReadOnly = True}
            btnBrowse = New Button With {.Text = "انتخاب فایل...", .Location = New Point(30, 100), .Size = New Size(90, 30)}
            AddHandler btnBrowse.Click, AddressOf BtnBrowse_Click

            Dim lblKey As New Label With {.Text = "کلیدواژه‌ها (Indexing):", .Location = New Point(380, 145), .AutoSize = True}
            txtKeywords = New TextBox With {.Location = New Point(30, 142), .Size = New Size(340, 26), .Text = "ضمانت‌نامه، تضامین، بانک تجارت، فاکتور"}

            Dim lblSec As New Label With {.Text = "سطح محرمانگی:", .Location = New Point(380, 185), .AutoSize = True}
            cboSecurity = New ComboBox With {.Location = New Point(180, 182), .Size = New Size(190, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboSecurity.Items.AddRange(New Object() {"عادی", "محرمانه", "خیلی محرمانه / سری"})
            cboSecurity.SelectedIndex = 1

            Dim lblExp As New Label With {.Text = "تاریخ انقضا (در صورت وجود):", .Location = New Point(370, 225), .AutoSize = True}
            txtExpDate = New TextBox With {.Location = New Point(180, 222), .Size = New Size(190, 26), .Text = PersianDateHelper.ToPersian(DateTime.Now.AddDays(365))}

            btnSave = New Button With {
                .Text = "💾 ذخیره و بایگانی سند",
                .Size = New Size(170, 36),
                .Location = New Point(180, 280),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(70, 280),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblTitle, txtTitle, lblCat, cboCategory, lblFile, txtFileName, btnBrowse,
                lblKey, txtKeywords, lblSec, cboSecurity, lblExp, txtExpDate, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnBrowse_Click(sender As Object, e As EventArgs)
            Using ofd As New OpenFileDialog()
                ofd.Filter = "Document Files|*.pdf;*.jpg;*.png;*.docx|All Files|*.*"
                If ofd.ShowDialog() = DialogResult.OK Then
                    txtFileName.Text = System.IO.Path.GetFileName(ofd.FileName)
                End If
            End Using
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان سند را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _dmsSvc.SaveDocument(
                _id, _companyID, txtTitle.Text, cboCategory.SelectedItem.ToString(),
                txtFileName.Text, txtKeywords.Text, cboSecurity.SelectedItem.ToString(),
                txtExpDate.Text, "مدیر بایگانی"
            )

            MessageBox.Show("سند با موفقیت در بایگانی دیجیتال ثبت و رمزنگاری گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
