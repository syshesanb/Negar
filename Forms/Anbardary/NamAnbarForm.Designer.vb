Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class NamAnbarForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents dgv As DataGridView
        Friend WithEvents txtName As TextBox
        Friend WithEvents txtLocation As TextBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnDelete As Button
        Friend WithEvents btnRefresh As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.dgv = New DataGridView()
            Me.txtName = New TextBox()
            Me.txtLocation = New TextBox()
            Me.chkActive = New CheckBox()
            Me.btnSave = New Button()
            Me.btnDelete = New Button()
            Me.btnRefresh = New Button()
            Me.SuspendLayout()
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1000, 620)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "NamAnbarForm"
            Me.Text = "مدیریت انبارها"

            Dim split As New SplitContainer() With {.Dock = DockStyle.Fill, .SplitterDistance = 360}
            Dim editor As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(8)}
            split.Panel1.Controls.Add(editor)
            split.Panel2.Controls.Add(Me.dgv)

            Me.dgv.Dock = DockStyle.Fill
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.ReadOnly = True
            Me.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            Dim lblName As New Label() With {.Text = "نام انبار", .AutoSize = True, .Location = New Point(10, 20)}
            Me.txtName.Location = New Point(110, 16) : Me.txtName.Width = 200
            Dim lblLocation As New Label() With {.Text = "موقعیت", .AutoSize = True, .Location = New Point(10, 56)}
            Me.txtLocation.Location = New Point(110, 52) : Me.txtLocation.Width = 200
            Me.chkActive.Text = "فعال"
            Me.chkActive.Location = New Point(110, 88)
            Me.chkActive.Checked = True
            Me.btnSave.Text = "ثبت"
            Me.btnSave.Location = New Point(10, 130)
            Me.btnDelete.Text = "حذف"
            Me.btnDelete.Location = New Point(90, 130)
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.Location = New Point(170, 130)
            editor.Controls.AddRange(New Control() {lblName, Me.txtName, lblLocation, Me.txtLocation, Me.chkActive, Me.btnSave, Me.btnDelete, Me.btnRefresh})
            Me.Controls.Add(split)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
