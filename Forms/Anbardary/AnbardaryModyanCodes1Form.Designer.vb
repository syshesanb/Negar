Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryModyanCodes1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnDownload As Button
        Friend WithEvents btnNew As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents lblRecordCount As Label
        Friend WithEvents pnlFilters As Panel
        Friend WithEvents dgvModyanCodes As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlTop = New Panel()
            Me.btnDownload = New Button()
            Me.btnNew = New Button()
            Me.btnRefresh = New Button()
            Me.lblRecordCount = New Label()
            Me.pnlFilters = New Panel()
            Me.dgvModyanCodes = New DataGridView()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvModyanCodes, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnDownload)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Controls.Add(Me.lblRecordCount)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(950, 42)
            Me.pnlTop.TabIndex = 0
            '
            'btnDownload
            '
            Me.btnDownload.BackColor = Color.FromArgb(0, 102, 204)
            Me.btnDownload.FlatStyle = FlatStyle.Flat
            Me.btnDownload.ForeColor = Color.White
            Me.btnDownload.Location = New Point(10, 8)
            Me.btnDownload.Name = "btnDownload"
            Me.btnDownload.Size = New Size(180, 26)
            Me.btnDownload.TabIndex = 0
            Me.btnDownload.Text = "دانلود کدهای سامانه مودیان"
            Me.btnDownload.UseVisualStyleBackColor = False
            '
            'btnNew
            '
            Me.btnNew.BackColor = Color.FromArgb(30, 80, 160)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(198, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(100, 26)
            Me.btnNew.TabIndex = 1
            Me.btnNew.Text = "جدید"
            Me.btnNew.UseVisualStyleBackColor = False
            '
            'btnRefresh
            '
            Me.btnRefresh.Location = New Point(306, 8)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(100, 26)
            Me.btnRefresh.TabIndex = 2
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = True
            '
            'lblRecordCount
            '
            Me.lblRecordCount.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Me.lblRecordCount.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblRecordCount.ForeColor = Color.FromArgb(0, 51, 102)
            Me.lblRecordCount.Location = New Point(420, 11)
            Me.lblRecordCount.Name = "lblRecordCount"
            Me.lblRecordCount.Size = New Size(510, 20)
            Me.lblRecordCount.TabIndex = 3
            Me.lblRecordCount.Text = "تعداد کل: ۰"
            Me.lblRecordCount.TextAlign = ContentAlignment.MiddleLeft
            '
            'pnlFilters
            '
            Me.pnlFilters.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlFilters.Dock = DockStyle.Top
            Me.pnlFilters.Location = New Point(0, 42)
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.Size = New Size(950, 30)
            Me.pnlFilters.TabIndex = 1
            '
            'dgvModyanCodes
            '
            Me.dgvModyanCodes.AllowUserToAddRows = False
            Me.dgvModyanCodes.BackgroundColor = Color.White
            Me.dgvModyanCodes.ColumnHeadersHeight = 30
            Me.dgvModyanCodes.Dock = DockStyle.Fill
            Me.dgvModyanCodes.Location = New Point(0, 72)
            Me.dgvModyanCodes.MultiSelect = False
            Me.dgvModyanCodes.Name = "dgvModyanCodes"
            Me.dgvModyanCodes.ReadOnly = True
            Me.dgvModyanCodes.RowHeadersVisible = False
            Me.dgvModyanCodes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvModyanCodes.Size = New Size(950, 528)
            Me.dgvModyanCodes.TabIndex = 2
            '
            'AnbardaryModyanCodes1Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.dgvModyanCodes)
            Me.Controls.Add(Me.pnlFilters)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryModyanCodes1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "کد کالا / مودیان"
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvModyanCodes, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
