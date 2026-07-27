Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business

Namespace Negar.Forms
    Public Class AnbardaryNamKala1Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _productsTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Private Const ColBtnEdit As String = "colEdit"
        Private Const ColBtnDelete As String = "colDelete"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryNamKala1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            ' Apply Grid Styling matching HesabdarySanad1Form (Image 2)
            If Me.dgvProducts IsNot Nothing Then
                Me.dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.Single
                Me.dgvProducts.GridColor = Color.FromArgb(200, 210, 225)
                Me.dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
                Me.dgvProducts.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
                Me.dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Me.dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
                Me.dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White
                Me.dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
            CreateFilterTextBoxes()

            AddHandler dgvProducts.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvProducts.Scroll, AddressOf DgvProducts_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes

            AlignSearchBoxes()
        End Sub

        Private Sub ConfigureGrid()
            dgvProducts.AutoGenerateColumns = False
            dgvProducts.Columns.Clear()
            dgvProducts.AllowUserToResizeColumns = True

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
            colId.Name = "ProductID"
            colId.DataPropertyName = "ProductID"
            colId.Visible = False

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "ProductCode"
            colCode.DataPropertyName = "ProductCode"
            colCode.HeaderText = "کد کالا"
            colCode.Width = 90

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "ProductName"
            colName.DataPropertyName = "ProductName"
            colName.HeaderText = "نام کالا"
            colName.Width = 160

            Dim colTechName As New DataGridViewTextBoxColumn()
            colTechName.Name = "TechnicalName"
            colTechName.DataPropertyName = "TechnicalName"
            colTechName.HeaderText = "نام فنی / لاتین"
            colTechName.Width = 140

            Dim colMainGroup As New DataGridViewTextBoxColumn()
            colMainGroup.Name = "MainCategory"
            colMainGroup.DataPropertyName = "MainCategory"
            colMainGroup.HeaderText = "گروه اصلی"
            colMainGroup.Width = 120

            Dim colSubGroup As New DataGridViewTextBoxColumn()
            colSubGroup.Name = "SubCategory"
            colSubGroup.DataPropertyName = "SubCategory"
            colSubGroup.HeaderText = "گروه فرعی"
            colSubGroup.Width = 120

            Dim colBarcode As New DataGridViewTextBoxColumn()
            colBarcode.Name = "Barcode"
            colBarcode.DataPropertyName = "Barcode"
            colBarcode.HeaderText = "بارکد"
            colBarcode.Width = 110

            Dim colUnit As New DataGridViewTextBoxColumn()
            colUnit.Name = "Unit"
            colUnit.DataPropertyName = "Unit"
            colUnit.HeaderText = "واحد"
            colUnit.Width = 90

            Dim colPurchasePrice As New DataGridViewTextBoxColumn()
            colPurchasePrice.Name = "PurchasePrice"
            colPurchasePrice.DataPropertyName = "PurchasePrice"
            colPurchasePrice.HeaderText = "قیمت خرید"
            colPurchasePrice.Width = 110
            colPurchasePrice.DefaultCellStyle.Format = "N0"
            colPurchasePrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colPrice As New DataGridViewTextBoxColumn()
            colPrice.Name = "DefaultPrice"
            colPrice.DataPropertyName = "DefaultPrice"
            colPrice.HeaderText = "قیمت فروش"
            colPrice.Width = 110
            colPrice.DefaultCellStyle.Format = "N0"
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "ProductType"
            colType.DataPropertyName = "ProductType"
            colType.HeaderText = "نوع"
            colType.Width = 90

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 70
            colActive.ReadOnly = True

            dgvProducts.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colCode, colName, colTechName,
                colMainGroup, colSubGroup, colBarcode, colUnit,
                colPurchasePrice, colPrice, colType, colActive
            })
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlFilters.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvProducts.Columns
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

        Private Sub DgvProducts_Scroll(sender As Object, e As ScrollEventArgs)
            AlignSearchBoxes()
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvProducts Is Nothing OrElse dgvProducts.Columns.Count = 0 OrElse pnlFilters Is Nothing Then Return

            pnlFilters.SuspendLayout()
            For Each kvp In filterTextBoxes
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = dgvProducts.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = dgvProducts.GetColumnDisplayRectangle(col.Index, True)
                    If rect.IsEmpty OrElse rect.Width = 0 Then
                        txt.Visible = False
                    Else
                        Dim screenPt = dgvProducts.PointToScreen(New Point(rect.X, 0))
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
            If _productsTable Is Nothing Then Return

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
                _productsTable.DefaultView.RowFilter = String.Join(" AND ", filters)
            Else
                _productsTable.DefaultView.RowFilter = ""
            End If
        End Sub

        Private Sub LoadData()
            Try
                _productsTable = _service.GetProducts()
                dgvProducts.DataSource = _productsTable
                ApplyFilters()
                AlignSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست کالاها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryNamKala2Form()
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DgvProducts_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit()
            End If
        End Sub

        Private Sub DgvProducts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvProducts.Columns(e.ColumnIndex).Name
                If colName = ColBtnEdit Then
                    OpenSelectedForEdit()
                ElseIf colName = ColBtnDelete Then
                    DeleteSelected()
                End If
            End If
        End Sub

        Private Sub OpenSelectedForEdit()
            If dgvProducts.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک کالا را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells("ProductID").Value)
            Using frm As New AnbardaryNamKala2Form(productId)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DeleteSelected()
            If dgvProducts.CurrentRow Is Nothing Then
                MessageBox.Show("لطفاً یک کالا را از لیست انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells("ProductID").Value)
            Dim productName = Convert.ToString(dgvProducts.CurrentRow.Cells("ProductName").Value)

            Dim confirm = MessageBox.Show("آیا از حذف کالای «" & productName & "» اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _service.DeleteProduct(productId)
                    MessageBox.Show("کالا با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف کالا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
