Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms.Controls
    Public Class PersonnelManagementControl
        Private _service As New PersonnelService()
        Private _departmentFilter As Integer = 0
        Private _currentPersonnelId As Integer? = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub Init(departmentFilter As Integer)
            _departmentFilter = departmentFilter
            
            ' Populate ComboBox
            cmbDepartment.Items.Clear()
            cmbDepartment.Items.Add("�� �����")
            cmbDepartment.Items.Add("��� ��������")
            cmbDepartment.Items.Add("��� ��������� � ����")
            
            If _departmentFilter > 0 Then
                ' Department specific view
                cmbDepartment.Enabled = False
                cmbDepartment.SelectedIndex = _departmentFilter - 1
                lblDepartment.Visible = False
                cmbDepartment.Visible = False
            Else
                ' Global view
                cmbDepartment.Enabled = True
                cmbDepartment.SelectedIndex = 0
                lblDepartment.Visible = True
                cmbDepartment.Visible = True
            End If

            LoadData()
        End Sub

        Private Sub LoadData()
            dgvPersonnel.DataSource = _service.GetPersonnel(_departmentFilter)
            
            If dgvPersonnel.Columns.Contains("PersonnelID") Then
                dgvPersonnel.Columns("PersonnelID").Visible = False
            End If
            If dgvPersonnel.Columns.Contains("Department") Then
                dgvPersonnel.Columns("Department").Visible = False
            End If
            
            If dgvPersonnel.Columns.Contains("FullName") Then dgvPersonnel.Columns("FullName").HeaderText = "نام و نام خانوادگی"
            If dgvPersonnel.Columns.Contains("Role") Then dgvPersonnel.Columns("Role").HeaderText = "سمت"
            If dgvPersonnel.Columns.Contains("NationalCode") Then dgvPersonnel.Columns("NationalCode").HeaderText = "کد ملی"
            If dgvPersonnel.Columns.Contains("Phone") Then dgvPersonnel.Columns("Phone").HeaderText = "شماره تماس"
            If dgvPersonnel.Columns.Contains("IsActive") Then dgvPersonnel.Columns("IsActive").HeaderText = "فعال"
            
            ClearForm()
        End Sub

        Private Sub ClearForm()
            _currentPersonnelId = Nothing
            txtFullName.Clear()
            txtRole.Clear()
            txtNationalCode.Clear()
            txtPhone.Clear()
            
            If _departmentFilter > 0 Then
                cmbDepartment.SelectedIndex = _departmentFilter - 1
            Else
                cmbDepartment.SelectedIndex = 0
            End If
            
            chkIsActive.Checked = True
            btnDelete.Enabled = False
            btnSave.Text = "ثبت"
            txtFullName.Focus()
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtFullName.Text) Then
                MessageBox.Show("لطفاً نام و نام خانوادگی را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim selectedDept = If(cmbDepartment.SelectedIndex >= 0, cmbDepartment.SelectedIndex + 1, _departmentFilter)
            If selectedDept = 0 Then selectedDept = 1

            Try
                _service.SavePersonnel(_currentPersonnelId, txtFullName.Text.Trim(), txtRole.Text.Trim(), txtNationalCode.Text.Trim(), txtPhone.Text.Trim(), selectedDept, chkIsActive.Checked)
                LoadData()
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد.", "تایید", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            ClearForm()
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If _currentPersonnelId.HasValue Then
                If MessageBox.Show("آیا از حذف این مورد اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Try
                        _service.DeletePersonnel(_currentPersonnelId.Value)
                        LoadData()
                    Catch ex As Exception
                        MessageBox.Show("خطا در حذف اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub dgvPersonnel_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPersonnel.CellDoubleClick
            If e.RowIndex >= 0 Then
                Dim row = dgvPersonnel.Rows(e.RowIndex)
                _currentPersonnelId = Convert.ToInt32(row.Cells("PersonnelID").Value)
                txtFullName.Text = row.Cells("FullName").Value.ToString()
                txtRole.Text = row.Cells("Role").Value.ToString()
                txtNationalCode.Text = row.Cells("NationalCode").Value.ToString()
                txtPhone.Text = row.Cells("Phone").Value.ToString()
                
                Dim dept = Convert.ToInt32(row.Cells("Department").Value)
                If _departmentFilter = 0 Then
                    If dept >= 1 AndAlso dept <= 3 Then
                        cmbDepartment.SelectedIndex = dept - 1
                    End If
                End If
                
                chkIsActive.Checked = Convert.ToBoolean(row.Cells("IsActive").Value)
                
                btnDelete.Enabled = True
                btnSave.Text = "ویرایش"
            End If
        End Sub
    End Class
End Namespace


