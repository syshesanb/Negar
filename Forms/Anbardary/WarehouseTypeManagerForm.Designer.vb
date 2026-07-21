Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class WarehouseTypeManagerForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents txtTypeName As TextBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnClear As Button
        Friend WithEvents lblTitle As Label
        Friend WithEvents dgvTypes As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlTop = New Panel()
            Me.lblTitle = New Label()
            Me.txtTypeName = New TextBox()
            Me.btnSave = New Button()
            Me.btnClear = New Button()
            Me.dgvTypes = New DataGridView()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvTypes, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.Controls.Add(Me.btnClear)
            Me.pnlTop.Controls.Add(Me.btnSave)
            Me.pnlTop.Controls.Add(Me.txtTypeName)
            Me.pnlTop.Controls.Add(Me.lblTitle)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(400, 50)
            Me.pnlTop.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(320, 15)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(65, 14)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "نوع انبار:"
            '
            'txtTypeName
            '
            Me.txtTypeName.Location = New Point(120, 12)
            Me.txtTypeName.Name = "txtTypeName"
            Me.txtTypeName.Size = New Size(190, 22)
            Me.txtTypeName.TabIndex = 1
            '
            'btnSave
            '
            Me.btnSave.Location = New Point(60, 10)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(50, 25)
            Me.btnSave.TabIndex = 2
            Me.btnSave.Text = "ثبت"
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'btnClear
            '
            Me.btnClear.Location = New Point(10, 10)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New Size(45, 25)
            Me.btnClear.TabIndex = 3
            Me.btnClear.Text = "لغو"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'dgvTypes
            '
            Me.dgvTypes.AllowUserToAddRows = False
            Me.dgvTypes.AllowUserToDeleteRows = False
            Me.dgvTypes.BackgroundColor = Color.White
            Me.dgvTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvTypes.Dock = DockStyle.Fill
            Me.dgvTypes.Location = New Point(0, 50)
            Me.dgvTypes.MultiSelect = False
            Me.dgvTypes.Name = "dgvTypes"
            Me.dgvTypes.ReadOnly = True
            Me.dgvTypes.RowHeadersVisible = False
            Me.dgvTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvTypes.Size = New Size(400, 300)
            Me.dgvTypes.TabIndex = 1
            '
            'WarehouseTypeManagerForm
            '
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(400, 350)
            Me.Controls.Add(Me.dgvTypes)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "WarehouseTypeManagerForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "مدیریت انواع انبار"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            CType(Me.dgvTypes, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
