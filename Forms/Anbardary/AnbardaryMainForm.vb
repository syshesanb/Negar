Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class AnbardaryMainForm
        Inherits AppBaseForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardaryMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Me.WindowState = FormWindowState.Maximized
            ctrlPersonnel.Init(3)
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(10, "در حال بارگذاری ماژول انبارداری...")

                ApplySecurity()
                progress.UpdateProgress(30, "بارگذاری لیست کالاها...")

                If tabs.TabPages.Contains(tabSettings) Then HostForm(tabSettings, New AnbardarySettingsForm())
                If tabs.TabPages.Contains(tabUnits) Then HostForm(tabUnits, New AnbardaryVahedKala1Form())
                progress.UpdateProgress(25, "بارگذاری گروه های کالا...")

                If tabs.TabPages.Contains(tabProductGroups) Then HostForm(tabProductGroups, New AnbardaryGoroohKala1Form())
                progress.UpdateProgress(30, "بارگذاری لیست کالاها...")

                If tabs.TabPages.Contains(tabProducts) Then HostForm(tabProducts, New AnbardaryNamKala1Form())
                progress.UpdateProgress(50, "بارگذاری تعریف انبارها...")

                If tabs.TabPages.Contains(tabWarehouses) Then HostForm(tabWarehouses, New AnbardaryNamAnbar1Form())
                progress.UpdateProgress(70, "بارگذاری فاکتورهای خرید...")

                If tabs.TabPages.Contains(tabPurchase) Then HostForm(tabPurchase, New AnbardaryKharid1Form())
                progress.UpdateProgress(85, "بارگذاری فاکتورهای فروش...")

                If tabs.TabPages.Contains(tabSales) Then HostForm(tabSales, New AnbardaryForoosh1Form())
                progress.UpdateProgress(92, "بارگذاری انتقال کالا بین انبارها...")

                If tabs.TabPages.Contains(tabTransfer) Then HostForm(tabTransfer, New Form())
                progress.UpdateProgress(95, "بارگذاری گزارش موجودی انبار...")

                If tabs.TabPages.Contains(tabInventory) Then HostForm(tabInventory, New MojodyAnbarFormRep())
                progress.UpdateProgress(100, "اتمام بارگذاری ماژول انبارداری")
            End Using
        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim hasGlobalTrade = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageTradeWarehouse)

            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ManageProducts)) Then
                tabs.TabPages.Remove(tabUnits)
                tabs.TabPages.Remove(tabProductGroups)
                tabs.TabPages.Remove(tabProducts)
            End If
            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ManageWarehouses)) Then
                tabs.TabPages.Remove(tabWarehouses)
            End If
            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ManagePurchases)) Then
                tabs.TabPages.Remove(tabPurchase)
            End If
            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ManageSales)) Then
                tabs.TabPages.Remove(tabSales)
            End If
            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ManageSales)) Then
                tabs.TabPages.Remove(tabTransfer)
            End If
            If Not (hasGlobalTrade OrElse SessionContext.HasPermission(PermissionKeys.ViewInventory)) Then
                tabs.TabPages.Remove(tabInventory)
            End If
        End Sub

        Private Sub HostForm(targetTab As TabPage, child As Form)
            child.TopLevel = False
            child.FormBorderStyle = FormBorderStyle.None
            child.Dock = DockStyle.Fill
            child.StartPosition = FormStartPosition.Manual
            child.Visible = True
            targetTab.Controls.Add(child)
            child.Show()
            child.BringToFront()
        End Sub
    End Class
End Namespace

