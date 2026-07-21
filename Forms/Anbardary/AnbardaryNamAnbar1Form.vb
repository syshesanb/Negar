Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryNamAnbar1Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _warehousesTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryNamAnbar1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            If Me.dgvWarehouses IsNot Nothing Then
                Me.dgvWarehouses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
            CreateFilterTextBoxes()
        End Sub

        Private Sub ConfigureGrid()
            dgvWarehouses.AutoGenerateColumns = False
            dgvWarehouses.Columns.Clear()
            dgvWarehouses.AllowUserToResizeColumns = True

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "btnEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.FillWeight = 8
            colEdit.MinimumWidth = 60
            
            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "btnDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.FillWeight = 8
            colDelete.MinimumWidth = 60

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "WarehouseID"
            colId.DataPropertyName = "WarehouseID"
            colId.Visible = False

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "WarehouseName"
            colName.DataPropertyName = "WarehouseName"
            colName.HeaderText = "نام انبار"
            colName.FillWeight = 20

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "WarehouseType"
            colType.DataPropertyName = "WarehouseType"
            colType.HeaderText = "نوع انبار"
            colType.FillWeight = 15

            Dim colKeeper As New DataGridViewTextBoxColumn()
            colKeeper.Name = "WarehouseKeeper"
            colKeeper.DataPropertyName = "WarehouseKeeper"
            colKeeper.HeaderText = "مسئول انبار"
            colKeeper.FillWeight = 15
            
            Dim colPhone As New DataGridViewTextBoxColumn()
            colPhone.Name = "Phone"
            colPhone.DataPropertyName = "Phone"
            colPhone.HeaderText = "شماره تماس"
            colPhone.FillWeight = 15

            Dim colLocation As New DataGridViewTextBoxColumn()
            colLocation.Name = "Location"
            colLocation.DataPropertyName = "Location"
            colLocation.HeaderText = "موقعیت / آدرس"
            colLocation.FillWeight = 25

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.FillWeight = 10
            colActive.ReadOnly = True

            dgvWarehouses.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colName, colType, colKeeper, colPhone, colLocation, colActive
            })
            
            AddHandler dgvWarehouses.ColumnWidthChanged, AddressOf DgvWarehouses_LayoutChanged
            AddHandler dgvWarehouses.Scroll, AddressOf DgvWarehouses_LayoutChanged
            AddHandler dgvWarehouses.Resize, AddressOf DgvWarehouses_LayoutChanged
            AddHandler dgvWarehouses.ColumnStateChanged, AddressOf DgvWarehouses_LayoutChanged
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlFilters.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvWarehouses.Columns
                If TypeOf col Is DataGridViewButtonColumn OrElse TypeOf col Is DataGridViewCheckBoxColumn OrElse Not col.Visible Then
                    Continue For
                End If

                Dim txt As New TextBox()
                txt.Name = "txtFilter_" & col.Name
                txt.Tag = col.DataPropertyName
                txt.BorderStyle = BorderStyle.FixedSingle
                AddHandler txt.TextChanged, AddressOf FilterTextBox_TextChanged
                
                pnlFilters.Controls.Add(txt)
                filterTextBoxes.Add(col.Name, txt)
            Next
            UpdateFilterLayout()
        End Sub

        Private Sub DgvWarehouses_LayoutChanged(sender As Object, e As EventArgs)
            UpdateFilterLayout()
        End Sub

        Private Sub UpdateFilterLayout()
            If dgvWarehouses Is Nothing OrElse pnlFilters Is Nothing Then Return
            
            pnlFilters.SuspendLayout()
            For Each kvp In filterTextBoxes
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = dgvWarehouses.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = dgvWarehouses.GetColumnDisplayRectangle(col.Index, False)
                    If rect.Width > 0 Then
                        txt.Visible = True
                        txt.Location = New Point(rect.X, 4)
                        txt.Width = rect.Width - 2
                    Else
                        txt.Visible = False
                    End If
                Else
                    txt.Visible = False
                End If
            Next
            pnlFilters.ResumeLayout()
        End Sub

        Private Sub FilterTextBox_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters()
        End Sub

        Private Sub ApplyFilters()
            If _warehousesTable Is Nothing Then Return

            Dim filters As New List(Of String)()

            For Each kvp In filterTextBoxes
                Dim txt = kvp.Value
                Dim propertyName = Convert.ToString(txt.Tag)
                Dim val = txt.Text.Trim().Replace("'", "''")
                
                If Not String.IsNullOrEmpty(val) Then
                    filters.Add(String.Format("Convert({0}, 'System.String') LIKE '%{1}%'", propertyName, val))
                End If
            Next

            If filters.Count > 0 Then
                _warehousesTable.DefaultView.RowFilter = String.Join(" AND ", filters)
            Else
                _warehousesTable.DefaultView.RowFilter = ""
            End If
        End Sub

        Private Sub LoadData()
            Try
                _warehousesTable = _service.GetWarehouses()
                dgvWarehouses.DataSource = _warehousesTable
                ApplyFilters()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست انبارها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryNamAnbar2Form()
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            OpenSelectedForEdit()
        End Sub

        Private Sub DgvWarehouses_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWarehouses.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit()
            End If
        End Sub
        
        Private Sub DgvWarehouses_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWarehouses.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvWarehouses.Columns(e.ColumnIndex).Name
                If colName = "btnEdit" Then
                    OpenSelectedForEdit()
                ElseIf colName = "btnDelete" Then
                    DeleteSelected()
                End If
            End If
        End Sub

        Private Sub OpenSelectedForEdit()
            If dgvWarehouses.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک انبار را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim warehouseId = Convert.ToInt32(dgvWarehouses.CurrentRow.Cells("WarehouseID").Value)
            Using frm As New AnbardaryNamAnbar2Form(warehouseId)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            DeleteSelected()
        End Sub

        Private Sub DeleteSelected()
            If dgvWarehouses.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک انبار را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim warehouseId = Convert.ToInt32(dgvWarehouses.CurrentRow.Cells("WarehouseID").Value)
            Dim warehouseName = Convert.ToString(dgvWarehouses.CurrentRow.Cells("WarehouseName").Value)

            Dim confirm = MessageBox.Show("آیا از حذف انبار «" & warehouseName & "» اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _service.DeleteWarehouse(warehouseId)
                    MessageBox.Show("انبار با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف انبار: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            For Each txt In filterTextBoxes.Values
                txt.Clear()
            Next
            LoadData()
        End Sub
    End Class
End Namespace
