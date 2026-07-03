Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class ReportCenterForm
        Inherits Form

        Public Sub New()
            InitializeComponent()
            ApplySecurity()
        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso
                               String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            btnActivityLog.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ViewActivityLog)
        End Sub

        Private Sub ShowSummary(title As String, table As DataTable)
            Dim summary As New Form() With {.Text = title, .Width = 1000, .Height = 650, .StartPosition = FormStartPosition.CenterParent}
            Dim grid As New DataGridView() With {.Dock = DockStyle.Fill, .DataSource = table, .ReadOnly = True, .AllowUserToAddRows = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill}
            summary.Controls.Add(grid)
            summary.ShowDialog(Me)
        End Sub

        Private Sub BtnTrial_Click(sender As Object, e As EventArgs) Handles btnTrial.Click
            ShowSummary("تراز آزمایشی", New AccountingService().GetTrialBalance())
        End Sub

        Private Sub BtnInv_Click(sender As Object, e As EventArgs) Handles btnInv.Click
            ShowSummary("موجودی انبار", New InventoryService().GetInventory())
        End Sub

        Private Sub BtnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
            ShowSummary("کاربران", New UserService().GetUsers())
        End Sub

        Private Sub BtnProducts_Click(sender As Object, e As EventArgs) Handles btnProducts.Click
            ShowSummary("کالاها", New CatalogService().GetProducts())
        End Sub

        Private Sub BtnActivityLog_Click(sender As Object, e As EventArgs) Handles btnActivityLog.Click
            Dim form As New ActivityLogForm()
            form.StartPosition = FormStartPosition.CenterParent
            form.ShowDialog(Me)
        End Sub
    End Class
End Namespace
