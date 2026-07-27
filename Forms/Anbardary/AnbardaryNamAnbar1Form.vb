Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business

Namespace Negar.Forms
    Public Class AnbardaryNamAnbar1Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _warehousesTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const ColBtnEdit As String = "colEdit"
        Private Const ColBtnDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryNamAnbar1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            ' Apply Grid Styling matching HesabdarySanad1Form (Image 2)
            If Me.dgvWarehouses IsNot Nothing Then
                Me.dgvWarehouses.CellBorderStyle = DataGridViewCellBorderStyle.Single
                Me.dgvWarehouses.GridColor = Color.FromArgb(200, 210, 225)
                Me.dgvWarehouses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
                Me.dgvWarehouses.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Me.dgvWarehouses.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.dgvWarehouses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
                Me.dgvWarehouses.DefaultCellStyle.SelectionForeColor = Color.White
                Me.dgvWarehouses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
            CreateFilterTextBoxes()

            AddHandler dgvWarehouses.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvWarehouses.Scroll, AddressOf DgvWarehouses_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes

            AlignSearchBoxes()
        End Sub

        Private Sub AnbardaryNamAnbar1Form_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
            If Me.Visible Then
                LoadData()
            End If
        End Sub

        Private Sub ConfigureGrid()
            dgvWarehouses.AutoGenerateColumns = False
            dgvWarehouses.Columns.Clear()
            dgvWarehouses.AllowUserToResizeColumns = True

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColBtnEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 60
            colEdit.FlatStyle = FlatStyle.Standard
            colEdit.ReadOnly = True

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = ColBtnDelete
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.Width = 56
            colDelete.FlatStyle = FlatStyle.Standard
            colDelete.ReadOnly = True

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "WarehouseID"
            colId.DataPropertyName = "WarehouseID"
            colId.Visible = False

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "WarehouseName"
            colName.DataPropertyName = "WarehouseName"
            colName.HeaderText = "نام انبار"
            colName.Width = 150

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "WarehouseType"
            colType.DataPropertyName = "WarehouseType"
            colType.HeaderText = "نوع انبار"
            colType.Width = 120

            Dim colKeeper As New DataGridViewTextBoxColumn()
            colKeeper.Name = "WarehouseKeeper"
            colKeeper.DataPropertyName = "WarehouseKeeper"
            colKeeper.HeaderText = "مسئول انبار"
            colKeeper.Width = 130

            Dim colPhone As New DataGridViewTextBoxColumn()
            colPhone.Name = "Phone"
            colPhone.DataPropertyName = "Phone"
            colPhone.HeaderText = "شماره تماس"
            colPhone.Width = 110

            Dim colPhone2 As New DataGridViewTextBoxColumn()
            colPhone2.Name = "Phone2"
            colPhone2.DataPropertyName = "Phone2"
            colPhone2.HeaderText = "تلفن دوم"
            colPhone2.Width = 110

            Dim colPhone3 As New DataGridViewTextBoxColumn()
            colPhone3.Name = "Phone3"
            colPhone3.DataPropertyName = "Phone3"
            colPhone3.HeaderText = "تلفن سوم"
            colPhone3.Width = 110

            Dim colPostalCode As New DataGridViewTextBoxColumn()
            colPostalCode.Name = "PostalCode"
            colPostalCode.DataPropertyName = "PostalCode"
            colPostalCode.HeaderText = "کد پستی"
            colPostalCode.Width = 100

            Dim colLocation As New DataGridViewTextBoxColumn()
            colLocation.Name = "Location"
            colLocation.DataPropertyName = "Location"
            colLocation.HeaderText = "موقعیت / آدرس"
            colLocation.Width = 180

            Dim colCapacity As New DataGridViewTextBoxColumn()
            colCapacity.Name = "Capacity"
            colCapacity.DataPropertyName = "Capacity"
            colCapacity.HeaderText = "ظرفیت"
            colCapacity.Width = 90

            Dim colCostCenter As New DataGridViewTextBoxColumn()
            colCostCenter.Name = "CostCenter"
            colCostCenter.DataPropertyName = "CostCenter"
            colCostCenter.HeaderText = "مرکز هزینه"
            colCostCenter.Width = 120

            Dim colAllowNeg As New DataGridViewCheckBoxColumn()
            colAllowNeg.Name = "AllowNegativeStock"
            colAllowNeg.DataPropertyName = "AllowNegativeStock"
            colAllowNeg.HeaderText = "موجودی منفی"
            colAllowNeg.Width = 95
            colAllowNeg.ReadOnly = True

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 70
            colActive.ReadOnly = True

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.Width = 200

            dgvWarehouses.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colName, colType, colKeeper,
                colPhone, colPhone2, colPhone3, colPostalCode, colLocation,
                colCapacity, colCostCenter, colAllowNeg, colActive, colDesc
            })
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlFilters.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvWarehouses.Columns
                Dim txt As New TextBox()
                txt.Name = "txtFilter_" & col.Name
                txt.Tag = col.DataPropertyName
                txt.BorderStyle = BorderStyle.FixedSingle

                If TypeOf col Is DataGridViewButtonColumn OrElse TypeOf col Is DataGridViewCheckBoxColumn Then
                    txt.Enabled = False
                    txt.ReadOnly = True
                Else
                    AddHandler txt.TextChanged, AddressOf FilterTextBox_TextChanged
                End If

                pnlFilters.Controls.Add(txt)
                filterTextBoxes.Add(col.Name, txt)
            Next
        End Sub

        Private Sub DgvWarehouses_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxes()
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvWarehouses Is Nothing OrElse dgvWarehouses.Columns.Count = 0 OrElse pnlFilters Is Nothing Then Return

            pnlFilters.SuspendLayout()
            For Each kvp In filterTextBoxes
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = dgvWarehouses.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = dgvWarehouses.GetColumnDisplayRectangle(col.Index, True)
                    If rect.IsEmpty OrElse rect.Width = 0 Then
                        txt.Visible = False
                    Else
                        Dim screenPt = dgvWarehouses.PointToScreen(New Point(rect.X, 0))
                        Dim panelPt = pnlFilters.PointToClient(screenPt)
                        txt.Location = New Point(panelPt.X, 4)
                        txt.Width = rect.Width
                        txt.Visible = True
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
                If String.IsNullOrEmpty(propertyName) OrElse Not txt.Enabled Then Continue For

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
                AlignSearchBoxes()
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

        Private Sub DgvWarehouses_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWarehouses.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit()
            End If
        End Sub

        Private Sub DgvWarehouses_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWarehouses.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvWarehouses.Columns(e.ColumnIndex).Name
                If colName = ColBtnEdit Then
                    OpenSelectedForEdit()
                ElseIf colName = ColBtnDelete Then
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
