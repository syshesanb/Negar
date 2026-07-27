Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    Partial Class AnbardaryTransfer1Form

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlTop = New Panel()
            Me.btnNewTransfer = New Button()
            Me.btnRefresh = New Button()
            Me.pnlFilters = New Panel()
            Me.dgvTransfers = New DataGridView()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvTransfers, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            ' pnlTop
            Me.pnlTop.Controls.Add(Me.btnNewTransfer)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Height = 42
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Padding = New Padding(5, 6, 5, 0)
            Me.pnlTop.BackColor = Color.FromArgb(235, 245, 252)

            ' btnNewTransfer
            Me.btnNewTransfer.BackColor = Color.FromArgb(0, 120, 180)
            Me.btnNewTransfer.FlatStyle = FlatStyle.Flat
            Me.btnNewTransfer.ForeColor = Color.White
            Me.btnNewTransfer.Font = New Font("Tahoma", 9.0!)
            Me.btnNewTransfer.Location = New Point(8, 8)
            Me.btnNewTransfer.Name = "btnNewTransfer"
            Me.btnNewTransfer.Size = New Size(210, 26)
            Me.btnNewTransfer.Text = "+ ثبت حواله انبار جدید"
            Me.btnNewTransfer.RightToLeft = RightToLeft.Yes
            Me.btnNewTransfer.TabIndex = 0

            ' btnRefresh
            Me.btnRefresh.BackColor = Color.FromArgb(70, 130, 80)
            Me.btnRefresh.FlatStyle = FlatStyle.Flat
            Me.btnRefresh.ForeColor = Color.White
            Me.btnRefresh.Font = New Font("Tahoma", 9.0!)
            Me.btnRefresh.Location = New Point(225, 8)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(90, 26)
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.RightToLeft = RightToLeft.Yes
            Me.btnRefresh.TabIndex = 1

            ' pnlFilters
            Me.pnlFilters.Dock = DockStyle.Top
            Me.pnlFilters.Height = 32
            Me.pnlFilters.Name = "pnlFilters"
            Me.pnlFilters.BackColor = Color.FromArgb(245, 250, 255)

            ' dgvTransfers
            Me.dgvTransfers.Dock = DockStyle.Fill
            Me.dgvTransfers.Name = "dgvTransfers"
            Me.dgvTransfers.RightToLeft = RightToLeft.Yes
            Me.dgvTransfers.ReadOnly = False
            Me.dgvTransfers.AllowUserToAddRows = False
            Me.dgvTransfers.AllowUserToDeleteRows = False

            ' AnbardaryTransfer1Form
            Me.ClientSize = New Size(1000, 600)
            Me.Controls.Add(Me.dgvTransfers)
            Me.Controls.Add(Me.pnlFilters)
            Me.Controls.Add(Me.pnlTop)
            Me.Name = "AnbardaryTransfer1Form"
            Me.Text = "حواله بین انبارها"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True

            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvTransfers, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNewTransfer As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents pnlFilters As Panel
        Friend WithEvents dgvTransfers As DataGridView

    End Class
End Namespace
