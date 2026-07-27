Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class AnbardaryModyanCodes1Form
        Inherits Form

        Private ReadOnly _service As New ModyanCodeService()
        Private _codesTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const ColBtnEdit As String = "colEdit"
        Private Const ColBtnDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryModyanCodes1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            If Me.dgvModyanCodes IsNot Nothing Then
                Me.dgvModyanCodes.CellBorderStyle = DataGridViewCellBorderStyle.Single
                Me.dgvModyanCodes.GridColor = Color.FromArgb(200, 210, 225)
                Me.dgvModyanCodes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
                Me.dgvModyanCodes.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Me.dgvModyanCodes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.dgvModyanCodes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
                Me.dgvModyanCodes.DefaultCellStyle.SelectionForeColor = Color.White
                Me.dgvModyanCodes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
            CreateFilterTextBoxes()

            AddHandler dgvModyanCodes.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvModyanCodes.Scroll, AddressOf DgvModyanCodes_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes

            AlignSearchBoxes()
        End Sub

        Private Sub ConfigureGrid()
            dgvModyanCodes.AutoGenerateColumns = False
            dgvModyanCodes.Columns.Clear()
            dgvModyanCodes.AllowUserToResizeColumns = True

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
            colId.Name = "CodeID"
            colId.DataPropertyName = "CodeID"
            colId.Visible = False

            Dim colModyanCode As New DataGridViewTextBoxColumn()
            colModyanCode.Name = "ModyanCode"
            colModyanCode.DataPropertyName = "ModyanCode"
            colModyanCode.HeaderText = "کد عمومی کالا / خدمت مودیان"
            colModyanCode.Width = 180

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "شرح کالا / خدمت"
            colDesc.Width = 320

            Dim colCategory As New DataGridViewTextBoxColumn()
            colCategory.Name = "CategoryName"
            colCategory.DataPropertyName = "CategoryName"
            colCategory.HeaderText = "دسته بندی"
            colCategory.Width = 160

            Dim colTaxRate As New DataGridViewTextBoxColumn()
            colTaxRate.Name = "TaxRate"
            colTaxRate.DataPropertyName = "TaxRate"
            colTaxRate.HeaderText = "نرخ مالیات"
            colTaxRate.Width = 100
            colTaxRate.DefaultCellStyle.Format = "P0"
            colTaxRate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 70
            colActive.ReadOnly = True

            dgvModyanCodes.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colModyanCode, colDesc, colCategory, colTaxRate, colActive
            })
        End Sub

        Private Sub LoadData()
            Try
                _codesTable = _service.GetModyanCodes()
                dgvModyanCodes.DataSource = _codesTable
                UpdateRecordStats()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub UpdateRecordStats()
            If _codesTable Is Nothing OrElse lblRecordCount Is Nothing Then Return
            Dim total = _codesTable.DefaultView.Count
            lblRecordCount.Text = $"تعداد کل کدهای عمومی: {total}"
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlFilters.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvModyanCodes.Columns
                Dim txt As New TextBox()
                txt.Name = "txtFilter_" & col.Name
                txt.Tag = col.DataPropertyName
                txt.BorderStyle = BorderStyle.FixedSingle

                If TypeOf col Is DataGridViewButtonColumn Then
                    txt.Enabled = False
                    txt.ReadOnly = True
                Else
                    AddHandler txt.TextChanged, AddressOf FilterTextBox_TextChanged
                End If

                pnlFilters.Controls.Add(txt)
                filterTextBoxes.Add(col.Name, txt)
            Next
        End Sub

        Private Sub DgvModyanCodes_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxes()
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvModyanCodes Is Nothing OrElse dgvModyanCodes.Columns.Count = 0 OrElse pnlFilters Is Nothing Then Return

            pnlFilters.SuspendLayout()
            For Each kvp In filterTextBoxes
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = dgvModyanCodes.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = dgvModyanCodes.GetColumnDisplayRectangle(col.Index, True)
                    If rect.IsEmpty OrElse rect.Width = 0 Then
                        txt.Visible = False
                    Else
                        Dim screenPt = dgvModyanCodes.PointToScreen(New Point(rect.X, 0))
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
            If _codesTable Is Nothing Then Return

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
                _codesTable.DefaultView.RowFilter = String.Join(" AND ", filters)
            Else
                _codesTable.DefaultView.RowFilter = ""
            End If
            UpdateRecordStats()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadData()
        End Sub

        Private Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "اتصال به سرور سامانه مودیان...")
                System.Threading.Thread.Sleep(500)
                progress.UpdateProgress(50, "دریافت کدهای عمومی کالا و خدمات...")
                System.Threading.Thread.Sleep(500)
                progress.UpdateProgress(80, "ذخیره سازی در پایگاه داده...")
                
                _service.DownloadModyanCodes()
                
                progress.UpdateProgress(100, "کدهای عمومی سامانه مودیان با موفقیت دانلود شد.")
                System.Threading.Thread.Sleep(300)
            End Using
            LoadData()
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryModyanCodes2Form()
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DgvModyanCodes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvModyanCodes.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvModyanCodes.Columns(e.ColumnIndex).Name
                Dim row = dgvModyanCodes.Rows(e.RowIndex)
                Dim codeId = Convert.ToInt32(row.Cells("CodeID").Value)

                If colName = ColBtnEdit Then
                    Using frm As New AnbardaryModyanCodes2Form(codeId)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadData()
                        End If
                    End Using
                ElseIf colName = ColBtnDelete Then
                    If MessageBox.Show("آیا از حذف این کد مودیان مطمئن هستید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            _service.DeleteModyanCode(codeId)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف کد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
