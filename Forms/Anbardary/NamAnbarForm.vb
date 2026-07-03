Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class NamAnbarForm
        Inherits Form

        Private ReadOnly service As New CatalogService()
        Private _selectedId As Integer?

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub NamAnbarForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadData()
        End Sub

        Private Sub LoadData()
            dgv.DataSource = service.GetWarehouses()
            If dgv.Columns.Contains("WarehouseID") Then dgv.Columns("WarehouseID").Visible = False
        End Sub

        Private Sub Dgv_SelectionChanged(sender As Object, e As EventArgs) Handles dgv.SelectionChanged
            If dgv.CurrentRow Is Nothing Then Return
            Dim row = dgv.CurrentRow
            _selectedId = Convert.ToInt32(row.Cells("WarehouseID").Value)
            txtName.Text = Convert.ToString(row.Cells("WarehouseName").Value)
            txtLocation.Text = Convert.ToString(row.Cells("Location").Value)
            chkActive.Checked = Convert.ToBoolean(row.Cells("IsActive").Value)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                service.SaveWarehouse(_selectedId, txtName.Text.Trim(), txtLocation.Text.Trim(), chkActive.Checked)
                LoadData()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End Sub

        Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If Not _selectedId.HasValue Then Return
            service.DeleteWarehouse(_selectedId.Value)
            _selectedId = Nothing
            LoadData()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadData()
        End Sub
    End Class
End Namespace
