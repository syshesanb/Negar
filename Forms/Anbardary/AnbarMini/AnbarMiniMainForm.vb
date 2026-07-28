Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniMainForm
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ApplySecurity()

            If tabsMini.TabPages.Contains(tabPOS) Then HostForm(tabPOS, New AnbarMiniForooshContainerForm())
            If tabsMini.TabPages.Contains(tabPurchase) Then HostForm(tabPurchase, New AnbarMiniKharidContainerForm())
            If tabsMini.TabPages.Contains(tabParties) Then HostForm(tabParties, New AnbardaryVendorsCustomersForm())
            If tabsMini.TabPages.Contains(tabExpenses) Then HostForm(tabExpenses, New AnbarMiniExpensesForm())
            If tabsMini.TabPages.Contains(tabProducts) Then HostForm(tabProducts, New AnbardaryNamKala1Form())
            If tabsMini.TabPages.Contains(tabWarehouses) Then HostForm(tabWarehouses, New AnbardaryNamAnbar1Form())
            If tabsMini.TabPages.Contains(tabGroups) Then HostForm(tabGroups, New AnbardaryGoroohKala1Form())
            If tabsMini.TabPages.Contains(tabInventory) Then HostForm(tabInventory, New MojodyAnbarFormRep())
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim isSuperAdmin = String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniPos) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabPOS)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharid) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabPurchase)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniPersons) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabParties)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniExpenses) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabExpenses)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniProducts) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabProducts)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniWarehouses) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabWarehouses)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniGroups) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabGroups)
            End If
            If Not (isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniReports) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)) Then
                tabsMini.TabPages.Remove(tabInventory)
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
