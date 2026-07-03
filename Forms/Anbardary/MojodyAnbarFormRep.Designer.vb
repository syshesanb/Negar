Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class MojodyAnbarFormRep
        Inherits Form

        Private components As IContainer
        Friend WithEvents dgv As DataGridView
        Friend WithEvents cmbWarehouse As ComboBox
        Friend WithEvents btnRefresh As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.dgv = New DataGridView()
            Me.cmbWarehouse = New ComboBox()
            Me.btnRefresh = New Button()
            Me.SuspendLayout()
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1150, 700)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "MojodyAnbarFormRep"
            Me.Text = "موجودی انبار"

            Dim top As New Panel() With {.Dock = DockStyle.Top, .Height = 55, .Padding = New Padding(8)}
            Dim lblWarehouse As New Label() With {.Text = "انبار", .AutoSize = True, .Location = New Point(10, 18)}
            Me.cmbWarehouse.Location = New Point(60, 14)
            Me.cmbWarehouse.Width = 220
            Me.cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.Location = New Point(300, 12)
            top.Controls.AddRange(New Control() {lblWarehouse, Me.cmbWarehouse, Me.btnRefresh})
            Me.dgv.Dock = DockStyle.Fill
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.ReadOnly = True
            Me.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.Controls.Add(Me.dgv)
            Me.Controls.Add(top)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
