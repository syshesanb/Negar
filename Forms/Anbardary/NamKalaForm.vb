Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class NamKalaForm
        Inherits Form

        Private ReadOnly service As New CatalogService()
        Private _selectedId As Integer?

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub NamKalaForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadData()
        End Sub

        Private Sub LoadData()
            dgv.DataSource = service.GetProducts()
            If dgv.Columns.Contains("ProductID") Then dgv.Columns("ProductID").Visible = False
        End Sub

        Private Sub Dgv_SelectionChanged(sender As Object, e As EventArgs) Handles dgv.SelectionChanged
            If dgv.CurrentRow Is Nothing Then Return
            Dim row = dgv.CurrentRow
            If row.Cells("ProductID").Value Is Nothing Then Return
            _selectedId = Convert.ToInt32(row.Cells("ProductID").Value)
            txtCode.Text = Convert.ToString(row.Cells("ProductCode").Value)
            txtName.Text = Convert.ToString(row.Cells("ProductName").Value)
            txtUnit.Text = Convert.ToString(row.Cells("Unit").Value)
            txtPrice.Text = Convert.ToString(row.Cells("DefaultPrice").Value)
            txtCategory.Text = Convert.ToString(row.Cells("Category").Value)
            chkActive.Checked = Convert.ToBoolean(row.Cells("IsActive").Value)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                Dim price As Decimal
                Decimal.TryParse(txtPrice.Text, price)
                service.SaveProduct(_selectedId, txtCode.Text.Trim(), txtName.Text.Trim(), txtUnit.Text.Trim(), price, txtCategory.Text.Trim(), chkActive.Checked)
                LoadData()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End Sub

        Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If Not _selectedId.HasValue Then Return
            service.DeleteProduct(_selectedId.Value)
            _selectedId = Nothing
            LoadData()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadData()
        End Sub
    End Class
End Namespace
