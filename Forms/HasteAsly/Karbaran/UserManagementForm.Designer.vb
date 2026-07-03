Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class UserManagementForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents tabs As TabControl
        Friend WithEvents tabUsers As TabPage
        Friend WithEvents tabPermissions As TabPage

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabs = New System.Windows.Forms.TabControl()
            Me.tabUsers = New System.Windows.Forms.TabPage()
            Me.tabPermissions = New System.Windows.Forms.TabPage()
            Me.tabs.SuspendLayout()
            Me.SuspendLayout()
            '
            'tabs
            '
            Me.tabs.Controls.Add(Me.tabUsers)
            Me.tabs.Controls.Add(Me.tabPermissions)
            Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabs.Location = New System.Drawing.Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New System.Drawing.Size(1314, 511)
            Me.tabs.TabIndex = 0
            '
            'tabUsers
            '
            Me.tabUsers.Location = New System.Drawing.Point(4, 23)
            Me.tabUsers.Name = "tabUsers"
            Me.tabUsers.Size = New System.Drawing.Size(1306, 484)
            Me.tabUsers.TabIndex = 0
            Me.tabUsers.Text = "مدیریت کاربران"
            Me.tabUsers.UseVisualStyleBackColor = True
            '
            'tabPermissions
            '
            Me.tabPermissions.Location = New System.Drawing.Point(4, 23)
            Me.tabPermissions.Name = "tabPermissions"
            Me.tabPermissions.Size = New System.Drawing.Size(1432, 553)
            Me.tabPermissions.TabIndex = 1
            Me.tabPermissions.Text = "سطح دسترسی"
            Me.tabPermissions.UseVisualStyleBackColor = True
            '
            'UserManagementForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1314, 511)
            Me.Controls.Add(Me.tabs)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "UserManagementForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "مدیریت کاربران"
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
