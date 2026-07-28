Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class UserManagementPermissionsForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents splitMain As SplitContainer
        Friend WithEvents dgvUsers As DataGridView
        Friend WithEvents pnlPresets As Panel
        Friend WithEvents lblPresets As Label
        Friend WithEvents cmbPresets As ComboBox
        Friend WithEvents btnApplyPreset As Button
        Friend WithEvents btnSavePreset As Button
        Friend WithEvents btnDeletePreset As Button
        Friend WithEvents pnlTreeToolbar As Panel
        Friend WithEvents btnExpandAll As Button
        Friend WithEvents btnCollapseAll As Button
        Friend WithEvents tvPermissions As TreeView
        Friend WithEvents btnSavePermissions As Button
        Friend WithEvents btnRefresh As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.splitMain = New SplitContainer()
            Me.dgvUsers = New DataGridView()
            Me.pnlPresets = New Panel()
            Me.lblPresets = New Label()
            Me.cmbPresets = New ComboBox()
            Me.btnApplyPreset = New Button()
            Me.btnSavePreset = New Button()
            Me.btnDeletePreset = New Button()
            Me.pnlTreeToolbar = New Panel()
            Me.btnExpandAll = New Button()
            Me.btnCollapseAll = New Button()
            Me.tvPermissions = New TreeView()
            Me.btnSavePermissions = New Button()
            Me.btnRefresh = New Button()

            CType(Me.splitMain, ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            CType(Me.dgvUsers, ISupportInitialize).BeginInit()
            Me.pnlPresets.SuspendLayout()
            Me.pnlTreeToolbar.SuspendLayout()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1440, 680)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Name = "UserManagementPermissionsForm"
            Me.Text = "مدیریت درختی سطح دسترسی‌ها و الگوها"

            Me.splitMain.Dock = DockStyle.Fill
            Me.splitMain.SplitterDistance = 550

            ' Panel 1 (Users list)
            Me.splitMain.Panel1.Controls.Add(Me.dgvUsers)
            Me.splitMain.Panel1.Controls.Add(Me.btnRefresh)

            Me.dgvUsers.Dock = DockStyle.Fill
            Me.dgvUsers.AllowUserToAddRows = False
            Me.dgvUsers.ReadOnly = True
            Me.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            Me.btnRefresh.Dock = DockStyle.Top
            Me.btnRefresh.Height = 36
            Me.btnRefresh.Text = "🔄 بازخوانی کاربران"
            Me.btnRefresh.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)

            ' Panel 2 (Presets bar + TreeView + Save button)
            Me.splitMain.Panel2.Controls.Add(Me.tvPermissions)
            Me.splitMain.Panel2.Controls.Add(Me.pnlTreeToolbar)
            Me.splitMain.Panel2.Controls.Add(Me.pnlPresets)
            Me.splitMain.Panel2.Controls.Add(Me.btnSavePermissions)

            ' Presets Panel
            Me.pnlPresets.Dock = DockStyle.Top
            Me.pnlPresets.Height = 45
            Me.pnlPresets.BackColor = Color.FromArgb(240, 245, 250)
            Me.pnlPresets.Padding = New Padding(5)
            Me.pnlPresets.RightToLeft = RightToLeft.Yes

            Me.lblPresets.Text = "الگوی پیش‌فرض (Role):"
            Me.lblPresets.Location = New Point(730, 12)
            Me.lblPresets.AutoSize = True
            Me.lblPresets.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)

            Me.cmbPresets.Location = New Point(490, 9)
            Me.cmbPresets.Size = New Size(235, 23)
            Me.cmbPresets.DropDownStyle = ComboBoxStyle.DropDownList

            Me.btnApplyPreset.Text = "⚡ اعمال الگو"
            Me.btnApplyPreset.Location = New Point(385, 8)
            Me.btnApplyPreset.Size = New Size(95, 26)
            Me.btnApplyPreset.BackColor = Color.FromArgb(0, 120, 180)
            Me.btnApplyPreset.ForeColor = Color.White
            Me.btnApplyPreset.FlatStyle = FlatStyle.Flat

            Me.btnSavePreset.Text = "💾 ذخیره به عنوان الگوی جدید"
            Me.btnSavePreset.Location = New Point(185, 8)
            Me.btnSavePreset.Size = New Size(190, 26)
            Me.btnSavePreset.BackColor = Color.FromArgb(40, 150, 90)
            Me.btnSavePreset.ForeColor = Color.White
            Me.btnSavePreset.FlatStyle = FlatStyle.Flat

            Me.btnDeletePreset.Text = "🗑️ حذف الگو"
            Me.btnDeletePreset.Location = New Point(80, 8)
            Me.btnDeletePreset.Size = New Size(95, 26)
            Me.btnDeletePreset.BackColor = Color.FromArgb(180, 50, 50)
            Me.btnDeletePreset.ForeColor = Color.White
            Me.btnDeletePreset.FlatStyle = FlatStyle.Flat

            Me.pnlPresets.Controls.Add(Me.lblPresets)
            Me.pnlPresets.Controls.Add(Me.cmbPresets)
            Me.pnlPresets.Controls.Add(Me.btnApplyPreset)
            Me.pnlPresets.Controls.Add(Me.btnSavePreset)
            Me.pnlPresets.Controls.Add(Me.btnDeletePreset)

            ' Tree Toolbar (Expand/Collapse)
            Me.pnlTreeToolbar.Dock = DockStyle.Top
            Me.pnlTreeToolbar.Height = 32
            Me.pnlTreeToolbar.BackColor = Color.FromArgb(248, 250, 252)
            Me.pnlTreeToolbar.RightToLeft = RightToLeft.Yes

            Me.btnExpandAll.Text = "➕ باز کردن کامل درختواره"
            Me.btnExpandAll.Location = New Point(680, 4)
            Me.btnExpandAll.Size = New Size(160, 24)

            Me.btnCollapseAll.Text = "➖ بستن همه شاخه‌ها"
            Me.btnCollapseAll.Location = New Point(515, 4)
            Me.btnCollapseAll.Size = New Size(155, 24)

            Me.pnlTreeToolbar.Controls.Add(Me.btnExpandAll)
            Me.pnlTreeToolbar.Controls.Add(Me.btnCollapseAll)

            ' TreeView
            Me.tvPermissions.Dock = DockStyle.Fill
            Me.tvPermissions.CheckBoxes = True
            Me.tvPermissions.RightToLeft = RightToLeft.Yes
            Me.tvPermissions.RightToLeftLayout = True
            Me.tvPermissions.Font = New Font("Tahoma", 9.0!, FontStyle.Regular)

            ' Save Button
            Me.btnSavePermissions.Dock = DockStyle.Bottom
            Me.btnSavePermissions.Height = 42
            Me.btnSavePermissions.Text = "💾 ذخیره نهایی سطح دسترسی کاربر"
            Me.btnSavePermissions.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.btnSavePermissions.BackColor = Color.FromArgb(0, 128, 128)
            Me.btnSavePermissions.ForeColor = Color.White
            Me.btnSavePermissions.FlatStyle = FlatStyle.Flat

            Me.Controls.Add(Me.splitMain)

            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            CType(Me.splitMain, ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            CType(Me.dgvUsers, ISupportInitialize).EndInit()
            Me.pnlPresets.ResumeLayout(False)
            Me.pnlPresets.PerformLayout()
            Me.pnlTreeToolbar.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
