Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class ActivityLogForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents dgvLog As DataGridView
        Friend WithEvents btnRefresh As Button
        Friend WithEvents pnlTop As Panel
        Friend WithEvents lblTitle As Label

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.dgvLog = New DataGridView()
            Me.btnRefresh = New Button()
            Me.pnlTop = New Panel()
            Me.lblTitle = New Label()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(960, 620)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "گزارش فعالیت‌های کاربران"

            Me.lblTitle.Text = "گزارش فعالیت‌های کاربران"
            Me.lblTitle.Dock = DockStyle.Fill
            Me.lblTitle.Font = New Font("Tahoma", 11.0!, FontStyle.Bold)
            Me.lblTitle.TextAlign = ContentAlignment.MiddleCenter

            Me.btnRefresh.Text = "بروزرسانی"
            Me.btnRefresh.Width = 100
            Me.btnRefresh.Height = 30
            Me.btnRefresh.Dock = DockStyle.Right

            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Height = 40
            Me.pnlTop.Controls.Add(Me.lblTitle)
            Me.pnlTop.Controls.Add(Me.btnRefresh)

            Me.dgvLog.Dock = DockStyle.Fill
            Me.dgvLog.ReadOnly = True
            Me.dgvLog.AllowUserToAddRows = False
            Me.dgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvLog.RightToLeft = RightToLeft.Yes

            Me.Controls.Add(Me.dgvLog)
            Me.Controls.Add(Me.pnlTop)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
