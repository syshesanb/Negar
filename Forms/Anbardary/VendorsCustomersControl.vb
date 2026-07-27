Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Controls
    Public Class VendorsCustomersControl
        Inherits UserControl

        Private ReadOnly service As New PersonService()

        ' کنترل‌های پانل بالای جستجو و فیلتر
        Private pnlFilter As Panel
        Private lblFilterRole As Label
        Private cmbFilterRole As ComboBox
        Private lblFilterType As Label
        Private cmbFilterType As ComboBox
        Private txtSearch As TextBox
        Private btnNew As Button

        ' گرید نمایش اشخاص
        Private dgvPersons As DataGridView

        ' پانل ثبت و ویرایش مشخصات (پایین یا سمت چپ/راست)
        Private pnlEdit As Panel
        Private rdbHaghighi As RadioButton
        Private rdbHoghooghi As RadioButton
        Private cmbRoleType As ComboBox
        Private txtPersonCode As TextBox
        Private txtFirstName As TextBox
        Private txtLastName As TextBox
        Private txtCompanyName As TextBox
        Private txtNationalCode As Label ' Dynamic Label for NationalCode / NationalID
        Private txtNationalCodeBox As TextBox
        Private lblEconomicCode As Label
        Private txtEconomicCode As TextBox
        Private lblRegistrationNumber As Label
        Private txtRegistrationNumber As TextBox
        Private txtPhone As TextBox
        Private txtMobile As TextBox
        Private txtPostalCode As TextBox
        Private txtAddress As TextBox
        Private chkActive As CheckBox
        Private btnSave As Button
        Private btnCancel As Button

        Private lblFirstName As Label
        Private lblLastName As Label
        Private lblCompanyName As Label

        Private _editPersonId As Integer? = Nothing

        Public Sub New()
            InitializeControl()
        End Sub

        Private Sub InitializeControl()
            Me.RightToLeft = RightToLeft.Yes
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Dock = DockStyle.Fill

            ' --- 1. Filter Panel (Top) ---
            pnlFilter = New Panel() With {.Dock = DockStyle.Top, .Height = 45, .BackColor = Color.FromArgb(235, 243, 255)}

            lblFilterRole = New Label() With {.Text = "نقش:", .AutoSize = True, .Location = New Point(1120, 14)}
            cmbFilterRole = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(1020, 11), .Width = 95}
            cmbFilterRole.Items.AddRange(New Object() {"همه", "فروشنده", "خریدار", "هر دو"})
            cmbFilterRole.SelectedIndex = 0

            lblFilterType = New Label() With {.Text = "نوع شخص:", .AutoSize = True, .Location = New Point(940, 14)}
            cmbFilterType = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(840, 11), .Width = 95}
            cmbFilterType.Items.AddRange(New Object() {"همه", "حقیقی", "حقوقی"})
            cmbFilterType.SelectedIndex = 0

            txtSearch = New TextBox() With {.Location = New Point(320, 11), .Width = 480}
            ' Hint text banner
            AddHandler txtSearch.TextChanged, AddressOf Filter_Changed
            AddHandler cmbFilterRole.SelectedIndexChanged, AddressOf Filter_Changed
            AddHandler cmbFilterType.SelectedIndexChanged, AddressOf Filter_Changed

            btnNew = New Button() With {.Text = "+ شخص جدید", .Location = New Point(15, 8), .Width = 110, .Height = 28, .BackColor = Color.FromArgb(40, 130, 220), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            AddHandler btnNew.Click, AddressOf BtnNew_Click

            pnlFilter.Controls.AddRange(New Control() {lblFilterRole, cmbFilterRole, lblFilterType, cmbFilterType, txtSearch, btnNew})

            ' --- 2. Edit Panel (Bottom) ---
            pnlEdit = New Panel() With {.Dock = DockStyle.Bottom, .Height = 240, .BackColor = Color.FromArgb(248, 251, 255), .Visible = False}
            pnlEdit.BorderStyle = BorderStyle.FixedSingle

            BuildEditPanelControls()

            ' --- 3. DataGridView (Center) ---
            dgvPersons = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AutoGenerateColumns = False,
                .RowHeadersVisible = False
            }

            SetupGridColumns()
            AddHandler dgvPersons.CellContentClick, AddressOf DgvPersons_CellContentClick
            AddHandler dgvPersons.RowPrePaint, AddressOf DgvPersons_RowPrePaint

            Me.Controls.Add(dgvPersons)
            Me.Controls.Add(pnlEdit)
            Me.Controls.Add(pnlFilter)

            AddHandler Me.Load, AddressOf Control_Load
        End Sub

        Private Sub Control_Load(sender As Object, e As EventArgs)
            RefreshGrid()
        End Sub

        Private Sub SetupGridColumns()
            dgvPersons.Columns.Clear()

            Dim colEdit As New DataGridViewButtonColumn() With {.Name = "colEdit", .HeaderText = "ویرایش", .Text = "ویرایش", .UseColumnTextForButtonValue = True, .Width = 60}
            Dim colDel As New DataGridViewButtonColumn() With {.Name = "colDel", .HeaderText = "حذف", .Text = "حذف", .UseColumnTextForButtonValue = True, .Width = 50}
            Dim colDaftar As New DataGridViewButtonColumn() With {.Name = "colDaftar", .HeaderText = "دفتر حساب", .Text = "دفتر حساب", .UseColumnTextForButtonValue = True, .Width = 85}

            Dim colCode As New DataGridViewTextBoxColumn() With {.Name = "colPersonCode", .DataPropertyName = "PersonCode", .HeaderText = "کد شخص", .Width = 90}
            Dim colName As New DataGridViewTextBoxColumn() With {.Name = "colDisplayName", .DataPropertyName = "DisplayName", .HeaderText = "نام شخص / شرکت", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill}
            Dim colType As New DataGridViewTextBoxColumn() With {.Name = "colPersonType", .DataPropertyName = "PersonType", .HeaderText = "نوع", .Width = 70}
            Dim colRole As New DataGridViewTextBoxColumn() With {.Name = "colRoleType", .DataPropertyName = "RoleType", .HeaderText = "نقش", .Width = 80}
            Dim colNationalCode As New DataGridViewTextBoxColumn() With {.Name = "colNationalCode", .DataPropertyName = "NationalCode", .HeaderText = "کد/شناسه ملی", .Width = 110}
            Dim colMobile As New DataGridViewTextBoxColumn() With {.Name = "colMobile", .DataPropertyName = "Mobile", .HeaderText = "همراه", .Width = 100}
            Dim colPhone As New DataGridViewTextBoxColumn() With {.Name = "colPhone", .DataPropertyName = "Phone", .HeaderText = "تلفن", .Width = 100}
            Dim colActive As New DataGridViewCheckBoxColumn() With {.Name = "colIsActive", .DataPropertyName = "IsActive", .HeaderText = "فعال", .Width = 50}

            Dim colID As New DataGridViewTextBoxColumn() With {.Name = "colPersonID", .DataPropertyName = "PersonID", .Visible = False}
            Dim colShenavarID As New DataGridViewTextBoxColumn() With {.Name = "colShenavarID", .DataPropertyName = "ShenavarID", .Visible = False}

            dgvPersons.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDel, colDaftar, colCode, colName, colType, colRole, colNationalCode, colMobile, colPhone, colActive, colID, colShenavarID
            })
        End Sub

        Private Sub BuildEditPanelControls()
            ' Row 1: Radio buttons (حقیقی / حقوقی) & Role Combo & Person Code
            Dim lblType As New Label() With {.Text = "نوع شخص:", .Location = New Point(1020, 15), .AutoSize = True}
            rdbHaghighi = New RadioButton() With {.Text = "حقیقی (فرد)", .Checked = True, .Location = New Point(930, 13), .AutoSize = True}
            rdbHoghooghi = New RadioButton() With {.Text = "حقوقی (شرکت/موسسه)", .Location = New Point(790, 13), .AutoSize = True}

            AddHandler rdbHaghighi.CheckedChanged, AddressOf PersonType_Changed
            AddHandler rdbHoghooghi.CheckedChanged, AddressOf PersonType_Changed

            Dim lblRole As New Label() With {.Text = "نقش:", .Location = New Point(720, 15), .AutoSize = True}
            cmbRoleType = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(610, 12), .Width = 100}
            cmbRoleType.Items.AddRange(New Object() {"فروشنده", "خریدار", "هر دو"})
            cmbRoleType.SelectedIndex = 2

            Dim lblCode As New Label() With {.Text = "کد شخص:", .Location = New Point(530, 15), .AutoSize = True}
            txtPersonCode = New TextBox() With {.Location = New Point(440, 12), .Width = 85}

            chkActive = New CheckBox() With {.Text = "فعال", .Checked = True, .Location = New Point(360, 14), .AutoSize = True}

            ' Row 2: Name fields
            lblFirstName = New Label() With {.Text = "نام:", .Location = New Point(1020, 50), .AutoSize = True}
            txtFirstName = New TextBox() With {.Location = New Point(870, 47), .Width = 140}

            lblLastName = New Label() With {.Text = "نام خانوادگی:", .Location = New Point(775, 50), .AutoSize = True}
            txtLastName = New TextBox() With {.Location = New Point(610, 47), .Width = 160}

            lblCompanyName = New Label() With {.Text = "نام شرکت/موسسه:", .Location = New Point(1000, 50), .AutoSize = True, .Visible = False}
            txtCompanyName = New TextBox() With {.Location = New Point(610, 47), .Width = 380, .Visible = False}

            txtNationalCode = New Label() With {.Text = "کد ملی:", .Location = New Point(520, 50), .AutoSize = True}
            txtNationalCodeBox = New TextBox() With {.Location = New Point(380, 47), .Width = 130}

            lblEconomicCode = New Label() With {.Text = "کد اقتصادی:", .Location = New Point(290, 50), .AutoSize = True}
            txtEconomicCode = New TextBox() With {.Location = New Point(150, 47), .Width = 130}

            lblRegistrationNumber = New Label() With {.Text = "شماره ثبت:", .Location = New Point(290, 15), .AutoSize = True, .Visible = False}
            txtRegistrationNumber = New TextBox() With {.Location = New Point(150, 12), .Width = 130, .Visible = False}

            ' Row 3: Contacts
            Dim lblMobile As New Label() With {.Text = "تلفن همراه:", .Location = New Point(1020, 85), .AutoSize = True}
            txtMobile = New TextBox() With {.Location = New Point(870, 82), .Width = 140}

            Dim lblPhone As New Label() With {.Text = "تلفن ثابت:", .Location = New Point(775, 85), .AutoSize = True}
            txtPhone = New TextBox() With {.Location = New Point(610, 82), .Width = 160}

            Dim lblPostal As New Label() With {.Text = "کد پستی:", .Location = New Point(520, 85), .AutoSize = True}
            txtPostalCode = New TextBox() With {.Location = New Point(380, 82), .Width = 130}

            ' Row 4: Address
            Dim lblAddr As New Label() With {.Text = "آدرس کامل:", .Location = New Point(1020, 120), .AutoSize = True}
            txtAddress = New TextBox() With {.Location = New Point(150, 117), .Width = 860, .Height = 45, .Multiline = True}

            ' Buttons (Save / Cancel)
            btnSave = New Button() With {.Text = "ذخیره مشخصات", .Location = New Point(150, 180), .Width = 130, .Height = 35, .BackColor = Color.FromArgb(40, 160, 80), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            btnCancel = New Button() With {.Text = "انصراف", .Location = New Point(15, 180), .Width = 100, .Height = 35, .BackColor = Color.Gray, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

            AddHandler btnSave.Click, AddressOf BtnSave_Click
            AddHandler btnCancel.Click, AddressOf BtnCancel_Click

            pnlEdit.Controls.AddRange(New Control() {
                lblType, rdbHaghighi, rdbHoghooghi, lblRole, cmbRoleType, lblCode, txtPersonCode, chkActive,
                lblFirstName, txtFirstName, lblLastName, txtLastName, lblCompanyName, txtCompanyName,
                txtNationalCode, txtNationalCodeBox, lblEconomicCode, txtEconomicCode, lblRegistrationNumber, txtRegistrationNumber,
                lblMobile, txtMobile, lblPhone, txtPhone, lblPostal, txtPostalCode,
                lblAddr, txtAddress, btnSave, btnCancel
            })
        End Sub

        Private Sub PersonType_Changed(sender As Object, e As EventArgs)
            If rdbHoghooghi.Checked Then
                lblFirstName.Visible = False
                txtFirstName.Visible = False
                lblLastName.Visible = False
                txtLastName.Visible = False

                lblCompanyName.Visible = True
                txtCompanyName.Visible = True

                txtNationalCode.Text = "شناسه ملی:"
                lblRegistrationNumber.Visible = True
                txtRegistrationNumber.Visible = True
            Else
                lblFirstName.Visible = True
                txtFirstName.Visible = True
                lblLastName.Visible = True
                txtLastName.Visible = True

                lblCompanyName.Visible = False
                txtCompanyName.Visible = False

                txtNationalCode.Text = "کد ملی:"
                lblRegistrationNumber.Visible = False
                txtRegistrationNumber.Visible = False
            End If
        End Sub

        Public Sub RefreshGrid()
            Try
                Dim dt = service.GetAll(cmbFilterRole.Text, cmbFilterType.Text, txtSearch.Text)
                dgvPersons.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("خطا در دريافت داده‌های اشخاص: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub Filter_Changed(sender As Object, e As EventArgs)
            RefreshGrid()
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs)
            ShowEditPanel(Nothing)
        End Sub

        Private Sub ShowEditPanel(personId As Integer?)
            _editPersonId = personId

            If personId.HasValue Then
                ' ویرایش
                For Each row As DataGridViewRow In dgvPersons.Rows
                    Dim pVal = row.Cells("colPersonID").Value
                    If pVal IsNot Nothing AndAlso Convert.ToInt32(pVal) = personId.Value Then
                        Dim dt = DirectCast(dgvPersons.DataSource, DataTable)
                        Dim drs = dt.Select("PersonID = " & personId.Value)
                        If drs.Length > 0 Then
                            Dim dr = drs(0)
                            Dim pType = Convert.ToString(dr("PersonType"))
                            If pType = "حقوقی" Then rdbHoghooghi.Checked = True Else rdbHaghighi.Checked = True

                            cmbRoleType.Text = Convert.ToString(dr("RoleType"))
                            txtPersonCode.Text = Convert.ToString(dr("PersonCode"))
                            txtFirstName.Text = Convert.ToString(dr("FirstName"))
                            txtLastName.Text = Convert.ToString(dr("LastName"))
                            txtCompanyName.Text = Convert.ToString(dr("CompanyName"))
                            txtNationalCodeBox.Text = Convert.ToString(dr("NationalCode"))
                            txtEconomicCode.Text = Convert.ToString(dr("EconomicCode"))
                            txtRegistrationNumber.Text = Convert.ToString(dr("RegistrationNumber"))
                            txtPhone.Text = Convert.ToString(dr("Phone"))
                            txtMobile.Text = Convert.ToString(dr("Mobile"))
                            txtAddress.Text = Convert.ToString(dr("Address"))
                            txtPostalCode.Text = Convert.ToString(dr("PostalCode"))
                            chkActive.Checked = If(dr.IsNull("IsActive"), True, Convert.ToBoolean(dr("IsActive")))
                        End If
                        Exit For
                    End If
                Next
            Else
                ' ثبت جدید
                _editPersonId = Nothing
                rdbHaghighi.Checked = True
                cmbRoleType.SelectedIndex = 2
                txtPersonCode.Text = service.GetNextCode()
                txtFirstName.Clear()
                txtLastName.Clear()
                txtCompanyName.Clear()
                txtNationalCodeBox.Clear()
                txtEconomicCode.Clear()
                txtRegistrationNumber.Clear()
                txtPhone.Clear()
                txtMobile.Clear()
                txtAddress.Clear()
                txtPostalCode.Clear()
                chkActive.Checked = True
            End If

            pnlEdit.Visible = True
            If rdbHoghooghi.Checked Then txtCompanyName.Focus() Else txtFirstName.Focus()
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            Dim personType = If(rdbHoghooghi.Checked, "حقوقی", "حقیقی")
            If personType = "حقیقی" AndAlso String.IsNullOrWhiteSpace(txtLastName.Text) Then
                MessageBox.Show("ورود نام خانوادگی برای افراد حقیقی الزامی است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtLastName.Focus()
                Return
            ElseIf personType = "حقوقی" AndAlso String.IsNullOrWhiteSpace(txtCompanyName.Text) Then
                MessageBox.Show("ورود نام شرکت/موسسه برای افراد حقوقی الزامی است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCompanyName.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(txtPersonCode.Text) Then
                MessageBox.Show("کد شخص نمی‌تواند خالی باشد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPersonCode.Focus()
                Return
            End If

            Try
                service.Save(
                    _editPersonId,
                    personType,
                    cmbRoleType.Text,
                    txtPersonCode.Text,
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtCompanyName.Text,
                    txtNationalCodeBox.Text,
                    txtEconomicCode.Text,
                    txtRegistrationNumber.Text,
                    txtPhone.Text,
                    txtMobile.Text,
                    txtAddress.Text,
                    txtPostalCode.Text,
                    chkActive.Checked)

                pnlEdit.Visible = False
                RefreshGrid()
                MessageBox.Show("مشخصات شخص با موفقیت ثبت/ویرایش گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره مشخصات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs)
            pnlEdit.Visible = False
        End Sub

        Private Sub DgvPersons_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return

            Dim colName = dgvPersons.Columns(e.ColumnIndex).Name
            Dim row = dgvPersons.Rows(e.RowIndex)
            Dim personId = Convert.ToInt32(row.Cells("colPersonID").Value)
            Dim shenavarIdVal = row.Cells("colShenavarID").Value
            Dim shenavarId As Integer? = If(shenavarIdVal IsNot Nothing AndAlso shenavarIdVal IsNot DBNull.Value, Convert.ToInt32(shenavarIdVal), CType(Nothing, Integer?))

            Select Case colName
                Case "colEdit"
                    ShowEditPanel(personId)

                Case "colDel"
                    Dim nameStr = Convert.ToString(row.Cells("colDisplayName").Value)
                    If MessageBox.Show("آیا از حذف شخص «" & nameStr & "» اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            service.Delete(personId)
                            RefreshGrid()
                            pnlEdit.Visible = False
                        Catch ex As Exception
                            MessageBox.Show(ex.Message, "عدم امکان حذف", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If

                Case "colDaftar"
                    If Not shenavarId.HasValue Then
                        MessageBox.Show("برای این شخص حساب شناور تعریف نشده است.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    ' باز کردن دفتر حساب شناور برای این شخص
                    Try
                        Dim codeStr = Convert.ToString(row.Cells("colPersonCode").Value)
                        Dim nameStr = Convert.ToString(row.Cells("colDisplayName").Value)
                        Using dlg As New HesabdaryDaftarShenavarForm(shenavarId.Value, codeStr, nameStr)
                            dlg.ShowDialog()
                        End Using
                    Catch ex As Exception
                        MessageBox.Show("خطا در نمایش دفتر حساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
            End Select
        End Sub

        Private Sub DgvPersons_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs)
            If e.RowIndex < 0 OrElse e.RowIndex >= dgvPersons.Rows.Count Then Return
            Dim row = dgvPersons.Rows(e.RowIndex)
            If (e.RowIndex Mod 2) = 0 Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(235, 244, 255)
            Else
                row.DefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255)
            End If
            row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 100)
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
            row.DefaultCellStyle.SelectionForeColor = Color.White
        End Sub

    End Class
End Namespace
