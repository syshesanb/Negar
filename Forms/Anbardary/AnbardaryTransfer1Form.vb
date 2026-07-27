Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Business.PersianDateHelper
Imports Negar.Data

Namespace Negar.Forms
    Public Class AnbardaryTransfer1Form
        Inherits Form

        Private _transferTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const ColNameEdit As String = "colEdit"
        Private Const ColNameDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryTransfer1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            If Me.dgvTransfers IsNot Nothing Then ApplyGridStyling(Me.dgvTransfers)

            ConfigureGrid(dgvTransfers)
            LoadData()

            CreateFilterTextBoxes(dgvTransfers, pnlFilters, filterTextBoxes, AddressOf FilterTextBox_TextChanged)
            AddHandler dgvTransfers.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvTransfers.Scroll, AddressOf DgvTransfers_Scroll
            AddHandler Me.Resize, AddressOf AlignAllSearchBoxes
            AlignAllSearchBoxes()
        End Sub

        Private Sub ApplyGridStyling(grid As DataGridView)
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single
            grid.GridColor = Color.FromArgb(200, 220, 230)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 252)
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 80, 130)
            grid.EnableHeadersVisualStyles = False
            grid.RowHeadersVisible = False
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255)
            grid.DefaultCellStyle.Font = New Font("Tahoma", 9.0!)
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        End Sub

        Private Sub ConfigureGrid(grid As DataGridView)
            grid.AutoGenerateColumns = False
            grid.Columns.Clear()
            grid.AllowUserToResizeColumns = True

            Dim colBtnEdit As New DataGridViewButtonColumn()
            colBtnEdit.Name = ColNameEdit
            colBtnEdit.HeaderText = ""
            colBtnEdit.Text = "ویرایش"
            colBtnEdit.UseColumnTextForButtonValue = True
            colBtnEdit.Width = 65
            colBtnEdit.FlatStyle = FlatStyle.Standard

            Dim colBtnDelete As New DataGridViewButtonColumn()
            colBtnDelete.Name = ColNameDelete
            colBtnDelete.HeaderText = ""
            colBtnDelete.Text = "حذف"
            colBtnDelete.UseColumnTextForButtonValue = True
            colBtnDelete.Width = 55
            colBtnDelete.FlatStyle = FlatStyle.Standard

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "colId"
            colId.DataPropertyName = "TransferID"
            colId.HeaderText = "شناسه"
            colId.Width = 55
            colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colId.ReadOnly = True

            Dim colNum As New DataGridViewTextBoxColumn()
            colNum.Name = "colNum"
            colNum.DataPropertyName = "TransferNumber"
            colNum.HeaderText = "شماره حواله"
            colNum.Width = 100
            colNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colNum.ReadOnly = True

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colDate"
            colDate.DataPropertyName = "PersianDate"
            colDate.HeaderText = "تاریخ حواله"
            colDate.Width = 100
            colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colDate.ReadOnly = True

            Dim colFrom As New DataGridViewTextBoxColumn()
            colFrom.Name = "colFrom"
            colFrom.DataPropertyName = "FromWarehouseName"
            colFrom.HeaderText = "انبار مبدا"
            colFrom.Width = 140
            colFrom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            colFrom.ReadOnly = True

            Dim colTo As New DataGridViewTextBoxColumn()
            colTo.Name = "colTo"
            colTo.DataPropertyName = "ToWarehouseName"
            colTo.HeaderText = "انبار مقصد"
            colTo.Width = 140
            colTo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            colTo.ReadOnly = True

            Dim colStatus As New DataGridViewTextBoxColumn()
            colStatus.Name = "colStatus"
            colStatus.DataPropertyName = "Status"
            colStatus.HeaderText = "وضعیت"
            colStatus.Width = 100
            colStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colStatus.ReadOnly = True

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            colDesc.ReadOnly = True

            grid.Columns.AddRange(New DataGridViewColumn() {
                colBtnEdit, colBtnDelete, colId, colNum, colDate,
                colFrom, colTo, colStatus, colDesc
            })

            grid.RightToLeft = RightToLeft.Yes
        End Sub

        Private Sub LoadData()
            Try
                _transferTable = GetTransfers()

                If _transferTable IsNot Nothing Then
                    If Not _transferTable.Columns.Contains("PersianDate") Then
                        _transferTable.Columns.Add("PersianDate", GetType(String))
                    End If
                    If Not _transferTable.Columns.Contains("Description") Then
                        _transferTable.Columns.Add("Description", GetType(String))
                    End If
                    If Not _transferTable.Columns.Contains("Status") Then
                        _transferTable.Columns.Add("Status", GetType(String))
                    End If

                    For Each row As DataRow In _transferTable.Rows
                        If Not row.IsNull("TransferDate") Then
                            row("PersianDate") = ToPersian(Convert.ToDateTime(row("TransferDate")))
                        End If
                        If row.IsNull("Status") OrElse String.IsNullOrEmpty(Convert.ToString(row("Status"))) Then
                            row("Status") = "ثبت شده"
                        End If
                    Next
                End If

                dgvTransfers.DataSource = _transferTable

                ApplyFilters(dgvTransfers, filterTextBoxes)
                AlignAllSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری حوالههای بین انبارها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetTransfers() As DataTable
            Try
                Return Sql.ExecuteTable(
                    "SELECT t.TransferID, t.TransferNumber, t.TransferDate, " &
                    "COALESCE(wf.WarehouseName, '---') AS FromWarehouseName, " &
                    "COALESCE(wt.WarehouseName, '---') AS ToWarehouseName, " &
                    "COALESCE(t.Status, 'ثبت شده') AS Status, " &
                    "COALESCE(t.Description, '') AS Description " &
                    "FROM WarehouseTransfers t " &
                    "LEFT JOIN Warehouses wf ON wf.WarehouseID = t.FromWarehouseID " &
                    "LEFT JOIN Warehouses wt ON wt.WarehouseID = t.ToWarehouseID " &
                    "ORDER BY t.TransferID DESC")
            Catch
                ' اگر جدول وجود نداشت، جدول خالی برگردان
                Dim dt As New DataTable()
                dt.Columns.Add("TransferID", GetType(Integer))
                dt.Columns.Add("TransferNumber", GetType(String))
                dt.Columns.Add("TransferDate", GetType(DateTime))
                dt.Columns.Add("FromWarehouseName", GetType(String))
                dt.Columns.Add("ToWarehouseName", GetType(String))
                dt.Columns.Add("Status", GetType(String))
                dt.Columns.Add("Description", GetType(String))
                Return dt
            End Try
        End Function

        Private Sub DgvTransfers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTransfers.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvTransfers.Columns(e.ColumnIndex).Name
                Dim row = dgvTransfers.Rows(e.RowIndex)
                If row.IsNewRow OrElse row.DataBoundItem Is Nothing Then Return

                Dim drv = TryCast(row.DataBoundItem, DataRowView)
                If drv Is Nothing Then Return
                Dim transferId As Integer = Convert.ToInt32(drv("TransferID"))

                If colName = ColNameEdit Then
                    Dim frm As New AnbardaryTransfer2Form(transferId)
                    frm.ShowDialog(Me)
                    LoadData()
                ElseIf colName = ColNameDelete Then
                    If MessageBox.Show("آیا از حذف این حواله انبار مطمئنید؟" & Environment.NewLine & "این عمل موجودی انبارها را به حالت قبل برمیگرداند.",
                                       "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                        Try
                            Sql.ExecuteNonQuery("DELETE FROM WarehouseTransferDetails WHERE TransferID = ?", transferId)
                            Sql.ExecuteNonQuery("DELETE FROM WarehouseTransfers WHERE TransferID = ?", transferId)
                            MessageBox.Show("حواله با موفقیت حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف حواله: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End Sub

        Private Sub DgvTransfers_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTransfers.CellDoubleClick
            If e.RowIndex >= 0 Then
                Dim row = dgvTransfers.Rows(e.RowIndex)
                If row.IsNewRow OrElse row.DataBoundItem Is Nothing Then Return
                Dim drv = TryCast(row.DataBoundItem, DataRowView)
                If drv Is Nothing Then Return
                Dim transferId As Integer = Convert.ToInt32(drv("TransferID"))
                Dim frm As New AnbardaryTransfer2Form(transferId)
                frm.ShowDialog(Me)
                LoadData()
            End If
        End Sub

        Private Sub BtnNewTransfer_Click(sender As Object, e As EventArgs) Handles btnNewTransfer.Click
            Dim frm As New AnbardaryTransfer2Form()
            frm.ShowDialog(Me)
            LoadData()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadData()
        End Sub

        ' ===== Filter & Align Helpers =====

        Private Sub CreateFilterTextBoxes(grid As DataGridView, panel As Panel, dict As Dictionary(Of String, TextBox), handler As EventHandler)
            panel.Controls.Clear()
            dict.Clear()
            For i = 0 To grid.Columns.Count - 1
                Dim col = grid.Columns(i)
                If TypeOf col Is DataGridViewButtonColumn OrElse Not col.Visible Then Continue For
                Dim tb As New TextBox()
                tb.Tag = col.Name
                tb.Font = New Font("Tahoma", 8.5!)
                tb.RightToLeft = RightToLeft.Yes
                AddHandler tb.TextChanged, handler
                panel.Controls.Add(tb)
                dict(col.Name) = tb
            Next
            AlignSearchBoxes(Nothing, Nothing)
        End Sub

        Private Sub AlignSearchBoxes(sender As Object, e As EventArgs)
            AlignBoxes(dgvTransfers, pnlFilters, filterTextBoxes)
        End Sub

        Private Sub AlignAllSearchBoxes(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
            AlignBoxes(dgvTransfers, pnlFilters, filterTextBoxes)
        End Sub

        Private Sub AlignBoxes(grid As DataGridView, panel As Panel, dict As Dictionary(Of String, TextBox))
            Dim headerHeight = grid.ColumnHeadersHeight
            For Each kvp In dict
                Dim col = grid.Columns(kvp.Key)
                If col Is Nothing OrElse Not col.Visible Then
                    kvp.Value.Visible = False
                    Continue For
                End If
                Dim rect = grid.GetColumnDisplayRectangle(col.Index, True)
                kvp.Value.Left = grid.Left + rect.Left
                kvp.Value.Top = grid.Top - 28
                kvp.Value.Width = rect.Width
                kvp.Value.Height = 22
                kvp.Value.Visible = True
            Next
        End Sub

        Private Sub ApplyFilters(grid As DataGridView, dict As Dictionary(Of String, TextBox))
            Dim dv = TryCast(grid.DataSource, DataView)
            If dv Is Nothing Then
                dv = New DataView(_transferTable)
                grid.DataSource = dv
            End If
            Dim filters As New List(Of String)()
            For Each kvp In dict
                If String.IsNullOrWhiteSpace(kvp.Value.Text) Then Continue For
                Dim col = grid.Columns(kvp.Key)
                If col IsNot Nothing Then
                    Dim prop = col.DataPropertyName
                    Dim val = kvp.Value.Text.Replace("'", "''")
                    filters.Add(String.Format("CONVERT([{0}], System.String) LIKE '%{1}%'", prop, val))
                End If
            Next
            dv.RowFilter = String.Join(" AND ", filters)
        End Sub

        Private Sub FilterTextBox_TextChanged(sender As Object, e As EventArgs)
            ApplyFilters(dgvTransfers, filterTextBoxes)
        End Sub

        Private Sub DgvTransfers_Scroll(sender As Object, e As ScrollEventArgs)
            AlignBoxes(dgvTransfers, pnlFilters, filterTextBoxes)
        End Sub

    End Class
End Namespace
