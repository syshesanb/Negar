Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniMainForm
        Private khForm As AnbarMiniKharidForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbarMiniMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            khForm = New AnbarMiniKharidForm()
            HostForm(tabPOS, New AnbarMiniForooshForm())
            HostForm(tabPurchase, khForm)
            HostForm(tabParties, New AnbardaryVendorsCustomersForm())
            HostForm(tabProducts, New AnbardaryNamKala1Form())
            HostForm(tabWarehouses, New AnbardaryNamAnbar1Form())
            HostForm(tabGroups, New AnbardaryGoroohKala1Form())
            HostForm(tabInventory, New MojodyAnbarFormRep())
        End Sub

        Private Sub tabsMini_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabsMini.SelectedIndexChanged
            If tabsMini.SelectedTab Is tabPurchase AndAlso khForm IsNot Nothing Then
                khForm.LoadWarehouses()
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
