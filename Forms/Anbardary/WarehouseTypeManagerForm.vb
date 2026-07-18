Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class WarehouseTypeManagerForm
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _editingId As Integer? = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub WarehouseTypeManagerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ConfigureGrid()
            LoadData()
        End Sub

        Private Sub ConfigureGrid()
            dgvTypes.AutoGenerateColumns = False
            dgvTypes.Columns.Clear()

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "btnEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 60

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "btnDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.Width = 60

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "TypeID"
            colId.DataPropertyName = "TypeID"
            colId.Visible = False

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "TypeName"
            colName.DataPropertyName = "TypeName"
            colName.HeaderText = "نوع انبار"
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvTypes.Columns.AddRange(New DataGridViewColumn() {colEdit, colDelete, colId, colName})
        End Sub

        Private Sub LoadData()
            Try
                dgvTypes.DataSource = _service.GetWarehouseTypes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim name = txtTypeName.Text.Trim()
            If String.IsNullOrEmpty(name) Then
                MessageBox.Show("لطفا نوع انبار را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                _service.SaveWarehouseType(_editingId, name)
                ClearForm()
                LoadData()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره اطلاعات (ممکن است نام تکراری باشد): " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
            ClearForm()
        End Sub

        Private Sub ClearForm()
            _editingId = Nothing
            txtTypeName.Text = ""
            btnSave.Text = "ثبت"
        End Sub

        Private Sub DgvTypes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTypes.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvTypes.Columns(e.ColumnIndex).Name
                If colName = "btnEdit" Then
                    _editingId = Convert.ToInt32(dgvTypes.Rows(e.RowIndex).Cells("TypeID").Value)
                    txtTypeName.Text = Convert.ToString(dgvTypes.Rows(e.RowIndex).Cells("TypeName").Value)
                    btnSave.Text = "ویرایش"
                ElseIf colName = "btnDelete" Then
                    Dim id = Convert.ToInt32(dgvTypes.Rows(e.RowIndex).Cells("TypeID").Value)
                    Dim typeName = Convert.ToString(dgvTypes.Rows(e.RowIndex).Cells("TypeName").Value)

                    If MessageBox.Show("آیا از حذف '" & typeName & "' اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                        Try
                            _service.DeleteWarehouseType(id)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
