Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryNamKala1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnEdit As Button
        Friend WithEvents btnDelete As Button
        Friend WithEvents btnRefresh As Button

        Friend WithEvents pnlFilters As Panel
        Friend WithEvents dgvProducts As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.btnEdit = New Button()
            Me.btnDelete = New Button()
            Me.btnRefresh = New Button()
            Me.pnlFilters = New Panel()
            Me.dgvProducts = New DataGridView()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvProducts, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnEdit)
            Me.pnlTop.Controls.Add(Me.btnDelete)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(950, 50)
            Me.pnlTop.TabIndex = 0
            '
            'btnNew
            '
            Me.btnNew.BackColor = Color.FromArgb(30, 80, 160)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(12, 10)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(100, 30)
            Me.btnNew.TabIndex = 0
            Me.btnNew.Text = "جدید"
            Me.btnNew.UseVisualStyleBackColor = False
            '
            'btnEdit
            '
            Me.btnEdit.BackColor = Color.FromArgb(240, 170, 0)
            Me.btnEdit.FlatStyle = FlatStyle.Flat
            Me.btnEdit.ForeColor = Color.Black
            Me.btnEdit.Location = New Point(118, 10)
            Me.btnEdit.Name = "btnEdit"
            Me.btnEdit.Size = New Size(100, 30)
            Me.btnEdit.TabIndex = 1
            Me.btnEdit.Text = "ویرایش"
            Me.btnEdit.UseVisualStyleBackColor = False
            '
            'btnDelete
            '
            Me.btnDelete.BackColor = Color.FromArgb(220, 80, 80)
            Me.btnDelete.FlatStyle = FlatStyle.Flat
            Me.btnDelete.ForeColor = Color.White
            Me.btnDelete.Location = New Point(224, 10)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New Size(100, 30)
            Me.btnDelete.TabIndex = 2
            Me.btnDelete.Text = "حذف"
            Me.btnDelete.UseVisualStyleBackColor = False
            '
            'btnRefresh
            '
            Me.btnRefresh.Location = New Point(330, 10)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(100, 30)
            Me.btnRefresh.TabIndex = 3
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = True
            '
            'pnlFilters
            '
            Me.pnlFilters.BackColor = Color.FromArgb(245, 245, 245)
            Me.pnlFilters.Dock = DockStyle.Top
            Me.pnlFilters.Location = New Point(0, 50)
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.Size = New Size(950, 30)
            Me.pnlFilters.TabIndex = 1
            '
            'dgvProducts
            '
            Me.dgvProducts.AllowUserToAddRows = False
            Me.dgvProducts.BackgroundColor = Color.White
            Me.dgvProducts.ColumnHeadersHeight = 30
            Me.dgvProducts.Dock = DockStyle.Fill
            Me.dgvProducts.Location = New Point(0, 80)
            Me.dgvProducts.MultiSelect = False
            Me.dgvProducts.Name = "dgvProducts"
            Me.dgvProducts.ReadOnly = True
            Me.dgvProducts.RowHeadersVisible = False
            Me.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvProducts.Size = New Size(950, 520)
            Me.dgvProducts.TabIndex = 2
            '
            'AnbardaryNamKala1Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.dgvProducts)
            Me.Controls.Add(Me.pnlFilters)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryNamKala1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "مدیریت کالاها"
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvProducts, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
