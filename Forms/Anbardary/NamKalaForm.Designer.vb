Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class NamKalaForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents dgv As DataGridView
        Friend WithEvents txtCode As TextBox
        Friend WithEvents txtName As TextBox
        Friend WithEvents txtUnit As TextBox
        Friend WithEvents txtPrice As TextBox
        Friend WithEvents txtCategory As TextBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnDelete As Button
        Friend WithEvents btnRefresh As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.dgv = New DataGridView()
            Me.txtCode = New TextBox()
            Me.txtName = New TextBox()
            Me.txtUnit = New TextBox()
            Me.txtPrice = New TextBox()
            Me.txtCategory = New TextBox()
            Me.chkActive = New CheckBox()
            Me.btnSave = New Button()
            Me.btnDelete = New Button()
            Me.btnRefresh = New Button()
            Me.SuspendLayout()
            '
            'NamKalaForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1100, 700)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "NamKalaForm"
            Me.Text = "مدیریت کالاها"
            '
            'dgv
            '
            Me.dgv.Dock = DockStyle.Fill
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.ReadOnly = True
            Me.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            '
            'editor labels/controls
            '
            Dim split As New SplitContainer() With {.Dock = DockStyle.Fill, .SplitterDistance = 420}
            Dim editor As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(8)}
            split.Panel1.Controls.Add(editor)
            split.Panel2.Controls.Add(Me.dgv)
            Dim lblCode As New Label() With {.Text = "کد کالا", .AutoSize = True, .Location = New Point(10, 20)}
            Me.txtCode.Location = New Point(120, 16) : Me.txtCode.Width = 220
            Dim lblName As New Label() With {.Text = "نام کالا", .AutoSize = True, .Location = New Point(10, 56)}
            Me.txtName.Location = New Point(120, 52) : Me.txtName.Width = 220
            Dim lblUnit As New Label() With {.Text = "واحد", .AutoSize = True, .Location = New Point(10, 92)}
            Me.txtUnit.Location = New Point(120, 88) : Me.txtUnit.Width = 120
            Dim lblPrice As New Label() With {.Text = "قیمت پیش‌فرض", .AutoSize = True, .Location = New Point(10, 128)}
            Me.txtPrice.Location = New Point(120, 124) : Me.txtPrice.Width = 120
            Dim lblCat As New Label() With {.Text = "دسته‌بندی", .AutoSize = True, .Location = New Point(10, 164)}
            Me.txtCategory.Location = New Point(120, 160) : Me.txtCategory.Width = 220
            Me.chkActive.Text = "فعال"
            Me.chkActive.Location = New Point(120, 196)
            Me.chkActive.Checked = True
            Me.btnSave.Text = "ثبت"
            Me.btnSave.Location = New Point(10, 240)
            Me.btnDelete.Text = "حذف"
            Me.btnDelete.Location = New Point(90, 240)
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.Location = New Point(170, 240)
            editor.Controls.AddRange(New Control() {lblCode, Me.txtCode, lblName, Me.txtName, lblUnit, Me.txtUnit, lblPrice, Me.txtPrice, lblCat, Me.txtCategory, Me.chkActive, Me.btnSave, Me.btnDelete, Me.btnRefresh})
            Me.Controls.Add(split)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
