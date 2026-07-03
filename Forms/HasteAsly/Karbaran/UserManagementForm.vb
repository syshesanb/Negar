Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    Partial Class UserManagementForm
        Inherits Form

        Private ReadOnly _ordinaryOnly As Boolean

        Public Sub New()
            Me.New(False)
        End Sub

        Public Sub New(ordinaryOnly As Boolean)
            _ordinaryOnly = ordinaryOnly
            InitializeComponent()
        End Sub

        Private Sub UserManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            tabs.TabPages.Clear()
            tabs.TabPages.Add(tabUsers)
            tabs.TabPages.Add(tabPermissions)

            HostForm(tabUsers, New UserManagementUsersForm(_ordinaryOnly))
            HostForm(tabPermissions, New UserManagementPermissionsForm(_ordinaryOnly))

            If _ordinaryOnly Then
                Text = "مدیریت کاربران عادی"
            End If
        End Sub

        Private Sub HostForm(targetTab As TabPage, child As Form)
            child.TopLevel = False
            child.FormBorderStyle = FormBorderStyle.None
            child.Dock = DockStyle.Fill
            child.StartPosition = FormStartPosition.Manual
            child.Visible = True
            targetTab.Controls.Clear()
            targetTab.Controls.Add(child)
            child.Show()
            child.BringToFront()
        End Sub
    End Class
End Namespace
