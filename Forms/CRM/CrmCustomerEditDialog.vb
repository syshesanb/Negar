Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.CRM
    Public Class CrmCustomerEditDialog
        Inherits Form

        Private txtCustomerCode As TextBox
        Private txtFullName As TextBox
        Private txtPhone As TextBox
        Private txtMobile As TextBox
        Private txtEmail As TextBox
        Private cmbCategory As ComboBox
        Private cmbLeadSource As ComboBox
        Private cmbStatus As ComboBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _crmSvc As CrmService
        Private _customerID As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional customerID As Integer = 0)
            _companyID = companyID
            _customerID = customerID
            _crmSvc = New CrmService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_customerID <= 0, "🤝 ثبت پرونده جدید در CRM", "🤝 ویرایش پرونده مشتری در CRM")
            Me.Size = New Size(540, 500)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد مشتری:", .Location = New Point(380, 25), .AutoSize = True}
            txtCustomerCode = New TextBox With {.Location = New Point(180, 22), .Size = New Size(180, 26)}

            Dim lblName As New Label With {.Text = "نام و نام خانوادگی / شرکت:", .Location = New Point(380, 65), .AutoSize = True}
            txtFullName = New TextBox With {.Location = New Point(30, 62), .Size = New Size(330, 26)}

            Dim lblPhone As New Label With {.Text = "تلفن ثابت:", .Location = New Point(380, 105), .AutoSize = True}
            txtPhone = New TextBox With {.Location = New Point(180, 102), .Size = New Size(180, 26)}

            Dim lblMobile As New Label With {.Text = "تلفن همراه:", .Location = New Point(380, 145), .AutoSize = True}
            txtMobile = New TextBox With {.Location = New Point(180, 142), .Size = New Size(180, 26)}

            Dim lblEmail As New Label With {.Text = "ایمیل:", .Location = New Point(380, 185), .AutoSize = True}
            txtEmail = New TextBox With {.Location = New Point(30, 182), .Size = New Size(330, 26)}

            Dim lblCat As New Label With {.Text = "نوع مخاطب:", .Location = New Point(380, 225), .AutoSize = True}
            cmbCategory = New ComboBox With {.Location = New Point(180, 222), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbCategory.Items.AddRange(New Object() {"مشتری حقوقی", "مشتری حقیقی", "نمایندگی / عاملیت", "سازمان دولتی"})
            cmbCategory.SelectedIndex = 0

            Dim lblSource As New Label With {.Text = "منبع آشنایی:", .Location = New Point(380, 265), .AutoSize = True}
            cmbLeadSource = New ComboBox With {.Location = New Point(180, 262), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbLeadSource.Items.AddRange(New Object() {"وب‌سایت", "نمایشگاه", "تلفن / بازاریابی", "معرفی دیگران", "شبکه‌های اجتماعی"})
            cmbLeadSource.SelectedIndex = 0

            Dim lblStatus As New Label With {.Text = "وضعیت CRM:", .Location = New Point(380, 305), .AutoSize = True}
            cmbStatus = New ComboBox With {.Location = New Point(180, 302), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbStatus.Items.AddRange(New Object() {"سرنخ اولیه", "مشتری احتمالی", "مشتری قطعی", "ناموفق / انصراف"})
            cmbStatus.SelectedIndex = 1

            Dim lblNotes As New Label With {.Text = "توضیحات و سوابق:", .Location = New Point(380, 345), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 342), .Size = New Size(330, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(240, 400),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(130, 400),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtCustomerCode, lblName, txtFullName, lblPhone, txtPhone,
                lblMobile, txtMobile, lblEmail, txtEmail, lblCat, cmbCategory,
                lblSource, cmbLeadSource, lblStatus, cmbStatus, lblNotes, txtNotes,
                btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            If _customerID > 0 Then
                Dim row = _crmSvc.GetCustomerById(_customerID)
                If row IsNot Nothing Then
                    txtCustomerCode.Text = Convert.ToString(row("CustomerCode"))
                    txtFullName.Text = Convert.ToString(row("FullName"))
                    txtPhone.Text = Convert.ToString(row("Phone"))
                    txtMobile.Text = Convert.ToString(row("Mobile"))
                    txtEmail.Text = Convert.ToString(row("Email"))
                    txtNotes.Text = Convert.ToString(row("Notes"))

                    Dim catStr = Convert.ToString(row("Category"))
                    If cmbCategory.Items.Contains(catStr) Then cmbCategory.SelectedItem = catStr

                    Dim srcStr = Convert.ToString(row("LeadSource"))
                    If cmbLeadSource.Items.Contains(srcStr) Then cmbLeadSource.SelectedItem = srcStr

                    Dim stStr = Convert.ToString(row("Status"))
                    If cmbStatus.Items.Contains(stStr) Then cmbStatus.SelectedItem = stStr
                End If
            Else
                txtCustomerCode.Text = "CRM-" & (Environment.TickCount Mod 10000).ToString()
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtFullName.Text) Then
                MessageBox.Show("لطفاً نام و نام خانوادگی یا نام شرکت را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _crmSvc.SaveCustomer(
                _customerID, _companyID, txtCustomerCode.Text, txtFullName.Text,
                txtPhone.Text, txtMobile.Text, txtEmail.Text,
                cmbCategory.SelectedItem.ToString(), cmbLeadSource.SelectedItem.ToString(),
                cmbStatus.SelectedItem.ToString(), txtNotes.Text
            )

            MessageBox.Show("پرونده مشتری در سیستم CRM با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
