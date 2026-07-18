Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryKharid1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnEdit As Button
        Friend WithEvents btnDelete As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents lblSearch As Label
        Friend WithEvents txtSearch As TextBox

        Friend WithEvents dgvInvoices As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.btnEdit = New Button()
            Me.btnDelete = New Button()
            Me.btnRefresh = New Button()
            Me.lblSearch = New Label()
            Me.txtSearch = New TextBox()
            Me.dgvInvoices = New DataGridView()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvInvoices, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnEdit)
            Me.pnlTop.Controls.Add(Me.btnDelete)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Controls.Add(Me.lblSearch)
            Me.pnlTop.Controls.Add(Me.txtSearch)
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
            'lblSearch
            '
            Me.lblSearch.Location = New Point(530, 15)
            Me.lblSearch.Name = "lblSearch"
            Me.lblSearch.Size = New Size(70, 20)
            Me.lblSearch.Text = "جستجو:"
            Me.lblSearch.TextAlign = ContentAlignment.MiddleRight
            '
            'txtSearch
            '
            Me.txtSearch.Location = New Point(606, 14)
            Me.txtSearch.Name = "txtSearch"
            Me.txtSearch.Size = New Size(200, 22)
            Me.txtSearch.TabIndex = 4
            '
            'dgvInvoices
            '
            Me.dgvInvoices.AllowUserToAddRows = False
            Me.dgvInvoices.BackgroundColor = Color.White
            Me.dgvInvoices.ColumnHeadersHeight = 30
            Me.dgvInvoices.Dock = DockStyle.Fill
            Me.dgvInvoices.Location = New Point(0, 50)
            Me.dgvInvoices.MultiSelect = False
            Me.dgvInvoices.Name = "dgvInvoices"
            Me.dgvInvoices.ReadOnly = True
            Me.dgvInvoices.RowHeadersVisible = False
            Me.dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoices.Size = New Size(950, 550)
            Me.dgvInvoices.TabIndex = 1
            '
            'AnbardaryKharid1Form
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(950, 600)
            Me.Controls.Add(Me.dgvInvoices)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Name = "AnbardaryKharid1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "مدیریت فاکتورهای خرید"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            CType(Me.dgvInvoices, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
