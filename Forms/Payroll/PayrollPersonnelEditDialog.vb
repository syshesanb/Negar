Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Payroll
    Public Class PayrollPersonnelEditDialog
        Inherits Form

        Private _personnelId As Integer?
        Private _payrollSvc As New PayrollService()

        ' Form Controls
        Private txtFullName As TextBox
        Private txtNationalCode As TextBox
        Private txtInsuranceNumber As TextBox
        Private txtBankAccountNumber As TextBox
        Private txtIban As TextBox
        Private cmbContractType As ComboBox
        Private cmbMaritalStatus As ComboBox
        Private numChildCount As NumericUpDown
        Private txtBaseSalary As TextBox
        Private txtHousingAllowance As TextBox
        Private txtFoodAllowance As TextBox
        Private txtChildAllowance As TextBox
        Private txtSeniorityAllowance As TextBox
        Private txtManagementAllowance As TextBox
        Private chkIsActive As CheckBox

        Private btnSave As Button
        Private btnCancel As Button

        Public Sub New(Optional personnelId As Integer? = Nothing)
            _personnelId = personnelId
            InitializeComponentCustom()
            If _personnelId.HasValue AndAlso _personnelId.Value > 0 Then
                LoadPersonnelData(_personnelId.Value)
            End If
        End Sub

        Private Sub InitializeComponentCustom()
            Me.Text = If(_personnelId.HasValue, "ویرایش اطلاعات پرسنل و احکام حقوقی", "ثبت پرسنل جدید و احکام حقوقی")
            Me.Size = New Size(720, 560)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.BackColor = Color.FromArgb(245, 247, 250)

            ' GroupBox 1: اطلاعات فردی و پرسنلی
            Dim gbPersonal As New GroupBox() With {
                .Text = "👤 اطلاعات فردی و استخدامی پرسنل",
                .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(13, 71, 161),
                .Location = New Point(15, 12),
                .Size = New Size(670, 210)
            }

            ' FullName
            gbPersonal.Controls.Add(New Label() With {.Text = "نام و نام خانوادگی:", .Location = New Point(540, 30), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtFullName = New TextBox() With {.Location = New Point(340, 27), .Size = New Size(190, 26), .Font = New Font("Tahoma", 9.0!)}
            gbPersonal.Controls.Add(txtFullName)

            ' NationalCode
            gbPersonal.Controls.Add(New Label() With {.Text = "کد ملی:", .Location = New Point(250, 30), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtNationalCode = New TextBox() With {.Location = New Point(50, 27), .Size = New Size(190, 26), .Font = New Font("Tahoma", 9.0!)}
            gbPersonal.Controls.Add(txtNationalCode)

            ' InsuranceNumber
            gbPersonal.Controls.Add(New Label() With {.Text = "شماره بیمه:", .Location = New Point(540, 75), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtInsuranceNumber = New TextBox() With {.Location = New Point(340, 72), .Size = New Size(190, 26), .Font = New Font("Tahoma", 9.0!)}
            gbPersonal.Controls.Add(txtInsuranceNumber)

            ' BankAccountNumber
            gbPersonal.Controls.Add(New Label() With {.Text = "شماره حساب:", .Location = New Point(250, 75), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtBankAccountNumber = New TextBox() With {.Location = New Point(50, 72), .Size = New Size(190, 26), .Font = New Font("Tahoma", 9.0!)}
            gbPersonal.Controls.Add(txtBankAccountNumber)

            ' Iban
            gbPersonal.Controls.Add(New Label() With {.Text = "شماره شبا (IR):", .Location = New Point(540, 120), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtIban = New TextBox() With {.Location = New Point(50, 117), .Size = New Size(480, 26), .Font = New Font("Tahoma", 9.0!), .RightToLeft = RightToLeft.No}
            gbPersonal.Controls.Add(txtIban)

            ' ContractType
            gbPersonal.Controls.Add(New Label() With {.Text = "نوع قرارداد:", .Location = New Point(540, 165), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            cmbContractType = New ComboBox() With {.Location = New Point(340, 162), .Size = New Size(190, 26), .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Tahoma", 9.0!)}
            cmbContractType.Items.AddRange(New Object() {"قراردادی", "رسمی", "پیمانی", "ساعتی", "پاره‌وقت"})
            cmbContractType.SelectedIndex = 0
            gbPersonal.Controls.Add(cmbContractType)

            ' MaritalStatus & ChildCount
            gbPersonal.Controls.Add(New Label() With {.Text = "تأهل / فرزندان:", .Location = New Point(250, 165), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            cmbMaritalStatus = New ComboBox() With {.Location = New Point(145, 162), .Size = New Size(95, 26), .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Tahoma", 9.0!)}
            cmbMaritalStatus.Items.AddRange(New Object() {"متاهل", "مجرد"})
            cmbMaritalStatus.SelectedIndex = 0
            gbPersonal.Controls.Add(cmbMaritalStatus)

            numChildCount = New NumericUpDown() With {.Location = New Point(50, 162), .Size = New Size(85, 26), .Minimum = 0, .Maximum = 10, .Font = New Font("Tahoma", 9.0!)}
            gbPersonal.Controls.Add(numChildCount)

            Me.Controls.Add(gbPersonal)

            ' GroupBox 2: احکام و اقلام حقوق ماهانه
            Dim gbSalary As New GroupBox() With {
                .Text = "💰 احکام حقوقی و مزایای ماهانه (ریال)",
                .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(46, 125, 50),
                .Location = New Point(15, 230),
                .Size = New Size(670, 220)
            }

            ' BaseSalary
            gbSalary.Controls.Add(New Label() With {.Text = "حقوق پایه:", .Location = New Point(540, 30), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtBaseSalary = New TextBox() With {.Location = New Point(340, 27), .Size = New Size(190, 26), .Text = "100,000,000", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtBaseSalary)

            ' HousingAllowance
            gbSalary.Controls.Add(New Label() With {.Text = "حق مسکن:", .Location = New Point(250, 30), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtHousingAllowance = New TextBox() With {.Location = New Point(50, 27), .Size = New Size(190, 26), .Text = "9,000,000", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtHousingAllowance)

            ' FoodAllowance
            gbSalary.Controls.Add(New Label() With {.Text = "بن کارگری:", .Location = New Point(540, 75), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtFoodAllowance = New TextBox() With {.Location = New Point(340, 72), .Size = New Size(190, 26), .Text = "14,000,000", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtFoodAllowance)

            ' ChildAllowance
            gbSalary.Controls.Add(New Label() With {.Text = "حق اولاد:", .Location = New Point(250, 75), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtChildAllowance = New TextBox() With {.Location = New Point(50, 72), .Size = New Size(190, 26), .Text = "7,000,000", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtChildAllowance)

            ' SeniorityAllowance
            gbSalary.Controls.Add(New Label() With {.Text = "پایه سنوات:", .Location = New Point(540, 120), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtSeniorityAllowance = New TextBox() With {.Location = New Point(340, 117), .Size = New Size(190, 26), .Text = "2,000,000", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtSeniorityAllowance)

            ' ManagementAllowance
            gbSalary.Controls.Add(New Label() With {.Text = "فوق‌العاده مدیریت:", .Location = New Point(250, 120), .AutoSize = True, .Font = New Font("Tahoma", 9.0!)})
            txtManagementAllowance = New TextBox() With {.Location = New Point(50, 117), .Size = New Size(190, 26), .Text = "0", .Font = New Font("Tahoma", 9.0!)}
            gbSalary.Controls.Add(txtManagementAllowance)

            ' IsActive
            chkIsActive = New CheckBox() With {.Text = "وضعیت پرسنل فعال می‌باشد", .Location = New Point(340, 165), .AutoSize = True, .Checked = True, .Font = New Font("Tahoma", 9.0!, FontStyle.Bold)}
            gbSalary.Controls.Add(chkIsActive)

            Me.Controls.Add(gbSalary)

            ' Bottom Panel for Action Buttons
            btnSave = New Button() With {
                .Text = "💾 ثبت و ذخیره اطلاعات",
                .Size = New Size(160, 38),
                .Location = New Point(180, 465),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                .Cursor = Cursors.Hand
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button() With {
                .Text = "انصراف",
                .Size = New Size(100, 38),
                .Location = New Point(65, 465),
                .BackColor = Color.FromArgb(120, 130, 140),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9.5!),
                .Cursor = Cursors.Hand
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.Add(btnSave)
            Me.Controls.Add(btnCancel)
        End Sub

        Private Sub LoadPersonnelData(id As Integer)
            Dim dt = Sql.ExecuteTable("SELECT * FROM PayrollPersonnel WHERE PersonnelID = ?", id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txtFullName.Text = Convert.ToString(row("FullName"))
                txtNationalCode.Text = Convert.ToString(row("NationalCode"))
                txtInsuranceNumber.Text = Convert.ToString(row("InsuranceNumber"))
                txtBankAccountNumber.Text = Convert.ToString(row("BankAccountNumber"))
                txtIban.Text = Convert.ToString(row("Iban"))
                cmbContractType.SelectedItem = Convert.ToString(row("ContractType"))
                cmbMaritalStatus.SelectedItem = Convert.ToString(row("MaritalStatus"))
                numChildCount.Value = Convert.ToDecimal(row("ChildCount"))
                txtBaseSalary.Text = Convert.ToDecimal(row("BaseSalary")).ToString("N0")
                txtHousingAllowance.Text = Convert.ToDecimal(row("HousingAllowance")).ToString("N0")
                txtFoodAllowance.Text = Convert.ToDecimal(row("FoodAllowance")).ToString("N0")
                txtChildAllowance.Text = Convert.ToDecimal(row("ChildAllowance")).ToString("N0")
                txtSeniorityAllowance.Text = Convert.ToDecimal(row("SeniorityAllowance")).ToString("N0")
                txtManagementAllowance.Text = Convert.ToDecimal(row("ManagementAllowance")).ToString("N0")
                chkIsActive.Checked = Convert.ToBoolean(row("IsActive"))
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtFullName.Text) Then
                MessageBox.Show("لطفاً نام و نام خانوادگی پرسنل را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim baseSal = ParseDecimal(txtBaseSalary.Text)
            Dim housing = ParseDecimal(txtHousingAllowance.Text)
            Dim food = ParseDecimal(txtFoodAllowance.Text)
            Dim childAllow = ParseDecimal(txtChildAllowance.Text)
            Dim seniority = ParseDecimal(txtSeniorityAllowance.Text)
            Dim mgmt = ParseDecimal(txtManagementAllowance.Text)

            _payrollSvc.SavePersonnel(_personnelId, txtFullName.Text.Trim(), txtNationalCode.Text.Trim(), txtInsuranceNumber.Text.Trim(), txtBankAccountNumber.Text.Trim(), txtIban.Text.Trim(), cmbContractType.SelectedItem.ToString(), cmbMaritalStatus.SelectedItem.ToString(), Convert.ToInt32(numChildCount.Value), baseSal, housing, food, childAllow, seniority, mgmt, chkIsActive.Checked)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Function ParseDecimal(val As String) As Decimal
            If String.IsNullOrWhiteSpace(val) Then Return 0D
            Dim clean = val.Replace(",", "").Replace(" ", "")
            Dim res As Decimal = 0D
            Decimal.TryParse(clean, res)
            Return res
        End Function

    End Class
End Namespace
