Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class UserManagementPermissionsForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents splitMain As SplitContainer
        Friend WithEvents dgvUsers As DataGridView
        Friend WithEvents dgvPermissions As DataGridView
        Friend WithEvents btnSavePermissions As Button
        Friend WithEvents btnRefresh As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.splitMain = New SplitContainer()
            Me.dgvUsers = New DataGridView()
            Me.dgvPermissions = New DataGridView()
            Me.btnSavePermissions = New Button()
            Me.btnRefresh = New Button()
            CType(Me.splitMain, ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            CType(Me.dgvUsers, ISupportInitialize).BeginInit()
            CType(Me.dgvPermissions, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1440, 580)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Name = "UserManagementPermissionsForm"
            Me.Text = "سطح دسترسی کاربران"

            Me.splitMain.Dock = DockStyle.Fill
            Me.splitMain.SplitterDistance = 670
            Me.splitMain.Panel1.Controls.Add(Me.dgvUsers)
            Me.splitMain.Panel1.Controls.Add(Me.btnRefresh)
            Me.splitMain.Panel2.Controls.Add(Me.dgvPermissions)
            Me.splitMain.Panel2.Controls.Add(Me.btnSavePermissions)

            Me.dgvUsers.Dock = DockStyle.Fill
            Me.dgvUsers.AllowUserToAddRows = False
            Me.dgvUsers.ReadOnly = True
            Me.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            Me.dgvPermissions.Dock = DockStyle.Fill
            Me.dgvPermissions.AllowUserToAddRows = False
            Me.dgvPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            Me.btnRefresh.Dock = DockStyle.Top
            Me.btnRefresh.Height = 36
            Me.btnRefresh.Text = "بازخوانی"

            Me.btnSavePermissions.Dock = DockStyle.Bottom
            Me.btnSavePermissions.Height = 36
            Me.btnSavePermissions.Text = "ذخیره سطح دسترسی"

            Me.Controls.Add(Me.splitMain)

            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            CType(Me.splitMain, ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            CType(Me.dgvUsers, ISupportInitialize).EndInit()
            CType(Me.dgvPermissions, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
