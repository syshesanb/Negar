Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryNamKala1Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _productsTable As DataTable
        Private filterTextBoxes As New Dictionary(Of String, TextBox)()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryNamKala1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)
            If Me.dgvProducts IsNot Nothing Then
                Me.dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()
            CreateFilterTextBoxes()
        End Sub

        Private Sub ConfigureGrid()
            dgvProducts.AutoGenerateColumns = False
            dgvProducts.Columns.Clear()
            dgvProducts.AllowUserToResizeColumns = True

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
            colId.Name = "ProductID"
            colId.DataPropertyName = "ProductID"
            colId.Visible = False

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "ProductCode"
            colCode.DataPropertyName = "ProductCode"
            colCode.HeaderText = "کد کالا"
            colCode.FillWeight = 12

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "ProductName"
            colName.DataPropertyName = "ProductName"
            colName.HeaderText = "نام کالا"
            colName.FillWeight = 25

            Dim colTechName As New DataGridViewTextBoxColumn()
            colTechName.Name = "TechnicalName"
            colTechName.DataPropertyName = "TechnicalName"
            colTechName.HeaderText = "نام فنی / لاتین"
            colTechName.FillWeight = 20

            Dim colMainGroup As New DataGridViewTextBoxColumn()
            colMainGroup.Name = "MainCategory"
            colMainGroup.DataPropertyName = "MainCategory"
            colMainGroup.HeaderText = "گروه اصلی"
            colMainGroup.FillWeight = 15

            Dim colSubGroup As New DataGridViewTextBoxColumn()
            colSubGroup.Name = "SubCategory"
            colSubGroup.DataPropertyName = "SubCategory"
            colSubGroup.HeaderText = "گروه فرعی"
            colSubGroup.FillWeight = 15

            Dim colBarcode As New DataGridViewTextBoxColumn()
            colBarcode.Name = "Barcode"
            colBarcode.DataPropertyName = "Barcode"
            colBarcode.HeaderText = "بارکد"
            colBarcode.FillWeight = 12

            Dim colUnit As New DataGridViewTextBoxColumn()
            colUnit.Name = "Unit"
            colUnit.DataPropertyName = "Unit"
            colUnit.HeaderText = "واحد"
            colUnit.FillWeight = 10

            Dim colPurchasePrice As New DataGridViewTextBoxColumn()
            colPurchasePrice.Name = "PurchasePrice"
            colPurchasePrice.DataPropertyName = "PurchasePrice"
            colPurchasePrice.HeaderText = "قیمت خرید"
            colPurchasePrice.FillWeight = 12
            colPurchasePrice.DefaultCellStyle.Format = "N0"

            Dim colPrice As New DataGridViewTextBoxColumn()
            colPrice.Name = "DefaultPrice"
            colPrice.DataPropertyName = "DefaultPrice"
            colPrice.HeaderText = "قیمت فروش"
            colPrice.FillWeight = 12
            colPrice.DefaultCellStyle.Format = "N0"

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "ProductType"
            colType.DataPropertyName = "ProductType"
            colType.HeaderText = "نوع"
            colType.FillWeight = 10

            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.FillWeight = 8
            colActive.ReadOnly = True

            dgvProducts.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDelete, colId, colCode, colName, colTechName, colMainGroup, colSubGroup, 
                colBarcode, colUnit, colPurchasePrice, colPrice, colType, colActive
            })
            
            AddHandler dgvProducts.ColumnWidthChanged, AddressOf DgvProducts_LayoutChanged
            AddHandler dgvProducts.Scroll, AddressOf DgvProducts_LayoutChanged
            AddHandler dgvProducts.Resize, AddressOf DgvProducts_LayoutChanged
            AddHandler dgvProducts.ColumnStateChanged, AddressOf DgvProducts_LayoutChanged
        End Sub

        Private Sub CreateFilterTextBoxes()
            pnlFilters.Controls.Clear()
            filterTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgvProducts.Columns
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

        Private Sub DgvProducts_LayoutChanged(sender As Object, e As EventArgs)
            UpdateFilterLayout()
        End Sub

        Private Sub UpdateFilterLayout()
            If dgvProducts Is Nothing OrElse pnlFilters Is Nothing Then Return
            
            pnlFilters.SuspendLayout()
            For Each kvp In filterTextBoxes
                Dim colName = kvp.Key
                Dim txt = kvp.Value
                Dim col = dgvProducts.Columns(colName)

                If col IsNot Nothing AndAlso col.Visible Then
                    Dim rect = dgvProducts.GetColumnDisplayRectangle(col.Index, False)
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
            If _productsTable Is Nothing Then Return

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

        Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            OpenSelectedForEdit()
        End Sub

        Private Sub DgvProducts_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellDoubleClick
            If e.RowIndex >= 0 Then
                OpenSelectedForEdit()
            End If
        End Sub
        
        Private Sub DgvProducts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellContentClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvProducts.Columns(e.ColumnIndex).Name
                If colName = "btnEdit" Then
                    OpenSelectedForEdit()
                ElseIf colName = "btnDelete" Then
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

        Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            DeleteSelected()
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
