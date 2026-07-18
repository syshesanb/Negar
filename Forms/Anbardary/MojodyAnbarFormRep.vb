Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class MojodyAnbarFormRep
        Inherits Form

        Private ReadOnly inventoryService As New InventoryService()
        Private ReadOnly catalogService As New CatalogService()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MojodyAnbarFormRep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            If Me.dgv IsNot Nothing Then Me.dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            cmbWarehouse.DataSource = catalogService.GetWarehouses()
            cmbWarehouse.DisplayMember = "WarehouseName"
            cmbWarehouse.ValueMember = "WarehouseID"
            LoadData()
        End Sub

        Private Sub LoadData()
            If cmbWarehouse.SelectedValue Is Nothing Then
                dgv.DataSource = inventoryService.GetInventory()
                Return
            End If
            dgv.DataSource = inventoryService.GetInventory(Convert.ToInt32(cmbWarehouse.SelectedValue))
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadData()
        End Sub
    End Class
End Namespace
