Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryVahedKala1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents btnManageCategories As Button
        Friend WithEvents pnlSerch As Panel
        Friend WithEvents lblSearchPrompt As Label
        Friend WithEvents txtSrcName As TextBox
        Friend WithEvents txtSrcCategory As TextBox
        Friend WithEvents txtSrcAbb As TextBox
        Friend WithEvents txtSrcNumerator As TextBox
        Friend WithEvents txtSrcDenominator As TextBox
        Friend WithEvents dgvUnits As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.btnRefresh = New Button()
            Me.btnManageCategories = New Button()
            Me.pnlSerch = New Panel()
            Me.lblSearchPrompt = New Label()
            Me.txtSrcName = New TextBox()
            Me.txtSrcCategory = New TextBox()
            Me.txtSrcAbb = New TextBox()
            Me.txtSrcNumerator = New TextBox()
            Me.txtSrcDenominator = New TextBox()
            Me.dgvUnits = New DataGridView()
            Me.pnlTop.SuspendLayout()
            Me.pnlSerch.SuspendLayout()
            CType(Me.dgvUnits, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnRefresh)
            Me.pnlTop.Controls.Add(Me.btnManageCategories)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(950, 50)
            Me.pnlTop.TabIndex = 0
            '
            'btnNew
            '
            Me.btnNew.BackColor = Color.FromArgb(40, 167, 69)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(850, 10)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(90, 30)
            Me.btnNew.Text = "واحد جدید"
            Me.btnNew.UseVisualStyleBackColor = False
            '
            'btnRefresh
            '
            Me.btnRefresh.BackColor = Color.FromArgb(108, 117, 125)
            Me.btnRefresh.FlatStyle = FlatStyle.Flat
            Me.btnRefresh.ForeColor = Color.White
            Me.btnRefresh.Location = New Point(755, 10)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New Size(90, 30)
            Me.btnRefresh.Text = "بازخوانی"
            Me.btnRefresh.UseVisualStyleBackColor = False
            '
            'btnManageCategories
            '
            Me.btnManageCategories.BackColor = Color.FromArgb(23, 162, 184)
            Me.btnManageCategories.FlatStyle = FlatStyle.Flat
            Me.btnManageCategories.ForeColor = Color.White
            Me.btnManageCategories.Location = New Point(605, 10)
            Me.btnManageCategories.Name = "btnManageCategories"
            Me.btnManageCategories.Size = New Size(140, 30)
            Me.btnManageCategories.Text = "مدیریت دسته‌بندی‌ها"
            Me.btnManageCategories.UseVisualStyleBackColor = False
            '
            'pnlSerch
            '
            Me.pnlSerch.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlSerch.Controls.Add(Me.lblSearchPrompt)
            Me.pnlSerch.Controls.Add(Me.txtSrcName)
            Me.pnlSerch.Controls.Add(Me.txtSrcCategory)
            Me.pnlSerch.Controls.Add(Me.txtSrcAbb)
            Me.pnlSerch.Controls.Add(Me.txtSrcNumerator)
            Me.pnlSerch.Controls.Add(Me.txtSrcDenominator)
            Me.pnlSerch.Dock = DockStyle.Top
            Me.pnlSerch.Location = New Point(0, 50)
            Me.pnlSerch.Name = "pnlSerch"
            Me.pnlSerch.Size = New Size(950, 30)
            Me.pnlSerch.TabIndex = 1
            '
            'lblSearchPrompt
            '
            Me.lblSearchPrompt.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblSearchPrompt.ForeColor = Color.FromArgb(0, 50, 120)
            Me.lblSearchPrompt.Location = New Point(720, 4)
            Me.lblSearchPrompt.Name = "lblSearchPrompt"
            Me.lblSearchPrompt.Size = New Size(220, 22)
            Me.lblSearchPrompt.Text = "جستجو :"
            Me.lblSearchPrompt.TextAlign = ContentAlignment.MiddleCenter
            '
            'txtSrcName
            '
            Me.txtSrcName.Location = New Point(570, 4)
            Me.txtSrcName.Name = "txtSrcName"
            Me.txtSrcName.Size = New Size(140, 22)
            Me.txtSrcName.TabIndex = 3
            Me.txtSrcName.TextAlign = HorizontalAlignment.Center
            '
            'txtSrcCategory
            '
            Me.txtSrcCategory.Location = New Point(420, 4)
            Me.txtSrcCategory.Name = "txtSrcCategory"
            Me.txtSrcCategory.Size = New Size(140, 22)
            Me.txtSrcCategory.TabIndex = 4
            Me.txtSrcCategory.TextAlign = HorizontalAlignment.Center
            '
            'txtSrcAbb
            '
            Me.txtSrcAbb.Location = New Point(300, 4)
            Me.txtSrcAbb.Name = "txtSrcAbb"
            Me.txtSrcAbb.Size = New Size(110, 22)
            Me.txtSrcAbb.TabIndex = 5
            Me.txtSrcAbb.TextAlign = HorizontalAlignment.Center
            '
            'txtSrcNumerator
            '
            Me.txtSrcNumerator.Location = New Point(90, 4)
            Me.txtSrcNumerator.Name = "txtSrcNumerator"
            Me.txtSrcNumerator.Size = New Size(100, 22)
            Me.txtSrcNumerator.TabIndex = 7
            Me.txtSrcNumerator.TextAlign = HorizontalAlignment.Center
            '
            'txtSrcDenominator
            '
            Me.txtSrcDenominator.Location = New Point(10, 4)
            Me.txtSrcDenominator.Name = "txtSrcDenominator"
            Me.txtSrcDenominator.Size = New Size(100, 22)
            Me.txtSrcDenominator.TabIndex = 8
            Me.txtSrcDenominator.TextAlign = HorizontalAlignment.Center
            '
            'dgvUnits
            '
            Me.dgvUnits.AllowUserToAddRows = False
            Me.dgvUnits.AllowUserToDeleteRows = False
            Me.dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvUnits.BackgroundColor = Color.White
            Me.dgvUnits.BorderStyle = BorderStyle.None
            Me.dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvUnits.Dock = DockStyle.Fill
            Me.dgvUnits.Location = New Point(0, 80)
            Me.dgvUnits.MultiSelect = False
            Me.dgvUnits.Name = "dgvUnits"
            Me.dgvUnits.ReadOnly = True
            Me.dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvUnits.Size = New Size(950, 570)
            Me.dgvUnits.TabIndex = 2
            '
            'AnbardaryVahedKala1Form
            '
            Me.ClientSize = New Size(950, 650)
            Me.Controls.Add(Me.dgvUnits)
            Me.Controls.Add(Me.pnlSerch)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.Name = "AnbardaryVahedKala1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Text = "واحدهای اندازه‌گیری کالا"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlSerch.ResumeLayout(False)
            Me.pnlSerch.PerformLayout()
            CType(Me.dgvUnits, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
