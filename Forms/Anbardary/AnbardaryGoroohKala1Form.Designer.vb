Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryGoroohKala1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents lblExpand As Label
        Friend WithEvents cmbExpandToLevel As ComboBox
        Friend WithEvents pnlSath As Panel
        Friend WithEvents lblSathInfo As Label
        Friend WithEvents pnlSearch As Panel
        Friend WithEvents lblSearchPrompt As Label
        Friend WithEvents txtSearchCode As TextBox
        Friend WithEvents txtSearchName As TextBox
        Friend WithEvents dgvGroups As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.pnlTop = New Panel()
            Me.btnNew = New Button()
            Me.lblExpand = New Label()
            Me.cmbExpandToLevel = New ComboBox()
            Me.pnlSath = New Panel()
            Me.lblSathInfo = New Label()
            Me.pnlSearch = New Panel()
            Me.lblSearchPrompt = New Label()
            Me.txtSearchCode = New TextBox()
            Me.txtSearchName = New TextBox()
            Me.dgvGroups = New DataGridView()
            Me.pnlTop.SuspendLayout()
            Me.pnlSath.SuspendLayout()
            Me.pnlSearch.SuspendLayout()
            CType(Me.dgvGroups, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = Color.FromArgb(230, 238, 250)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.lblExpand)
            Me.pnlTop.Controls.Add(Me.cmbExpandToLevel)
            Me.pnlTop.Dock = DockStyle.Top
            Me.pnlTop.Location = New Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New Size(950, 45)
            Me.pnlTop.TabIndex = 0
            '
            'btnNew
            '
            Me.btnNew.BackColor = Color.FromArgb(40, 167, 69)
            Me.btnNew.FlatStyle = FlatStyle.Flat
            Me.btnNew.ForeColor = Color.White
            Me.btnNew.Location = New Point(830, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New Size(110, 28)
            Me.btnNew.Text = "جدید"
            Me.btnNew.UseVisualStyleBackColor = False
            '
            'lblExpand
            '
            Me.lblExpand.Location = New Point(590, 12)
            Me.lblExpand.Name = "lblExpand"
            Me.lblExpand.Size = New Size(170, 20)
            Me.lblExpand.Text = "نمایش گروه‌ها تا سطح:"
            Me.lblExpand.TextAlign = ContentAlignment.MiddleRight
            '
            'cmbExpandToLevel
            '
            Me.cmbExpandToLevel.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbExpandToLevel.FormattingEnabled = True
            Me.cmbExpandToLevel.Location = New Point(380, 10)
            Me.cmbExpandToLevel.Name = "cmbExpandToLevel"
            Me.cmbExpandToLevel.Size = New Size(200, 24)
            Me.cmbExpandToLevel.TabIndex = 1
            '
            'pnlSath
            '
            Me.pnlSath.BackColor = Color.FromArgb(245, 248, 253)
            Me.pnlSath.Controls.Add(Me.lblSathInfo)
            Me.pnlSath.Dock = DockStyle.Top
            Me.pnlSath.Location = New Point(0, 45)
            Me.pnlSath.Name = "pnlSath"
            Me.pnlSath.Size = New Size(950, 30)
            '
            'lblSathInfo
            '
            Me.lblSathInfo.Dock = DockStyle.Fill
            Me.lblSathInfo.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            Me.lblSathInfo.ForeColor = Color.FromArgb(0, 50, 150)
            Me.lblSathInfo.Location = New Point(0, 0)
            Me.lblSathInfo.Name = "lblSathInfo"
            Me.lblSathInfo.Size = New Size(950, 30)
            Me.lblSathInfo.Text = "سطح گروه جاری: گروه اصلی"
            Me.lblSathInfo.TextAlign = ContentAlignment.MiddleLeft
            '
            'pnlSearch
            '
            Me.pnlSearch.BackColor = Color.FromArgb(240, 244, 250)
            Me.pnlSearch.Controls.Add(Me.lblSearchPrompt)
            Me.pnlSearch.Controls.Add(Me.txtSearchCode)
            Me.pnlSearch.Controls.Add(Me.txtSearchName)
            Me.pnlSearch.Dock = DockStyle.Top
            Me.pnlSearch.Location = New Point(0, 75)
            Me.pnlSearch.Name = "pnlSearch"
            Me.pnlSearch.Size = New Size(950, 30)
            Me.pnlSearch.TabIndex = 2
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
            'txtSearchCode
            '
            Me.txtSearchCode.Location = New Point(570, 4)
            Me.txtSearchCode.Name = "txtSearchCode"
            Me.txtSearchCode.Size = New Size(140, 22)
            Me.txtSearchCode.TabIndex = 0
            Me.txtSearchCode.TextAlign = HorizontalAlignment.Center
            '
            'txtSearchName
            '
            Me.txtSearchName.Location = New Point(10, 4)
            Me.txtSearchName.Name = "txtSearchName"
            Me.txtSearchName.Size = New Size(550, 22)
            Me.txtSearchName.TabIndex = 1
            Me.txtSearchName.TextAlign = HorizontalAlignment.Center
            '
            'dgvGroups
            '
            Me.dgvGroups.AllowUserToAddRows = False
            Me.dgvGroups.AllowUserToDeleteRows = False
            Me.dgvGroups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvGroups.BackgroundColor = Color.White
            Me.dgvGroups.BorderStyle = BorderStyle.None
            Me.dgvGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvGroups.Dock = DockStyle.Fill
            Me.dgvGroups.Location = New Point(0, 105)
            Me.dgvGroups.MultiSelect = False
            Me.dgvGroups.Name = "dgvGroups"
            Me.dgvGroups.ReadOnly = True
            Me.dgvGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvGroups.Size = New Size(950, 545)
            Me.dgvGroups.TabIndex = 3
            '
            'AnbardaryGoroohKala1Form
            '
            Me.ClientSize = New Size(950, 650)
            Me.Controls.Add(Me.dgvGroups)
            Me.Controls.Add(Me.pnlSearch)
            Me.Controls.Add(Me.pnlSath)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.Name = "AnbardaryGoroohKala1Form"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Text = "مدیریت گروه‌بندی کالاها"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlSath.ResumeLayout(False)
            Me.pnlSearch.ResumeLayout(False)
            Me.pnlSearch.PerformLayout()
            CType(Me.dgvGroups, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
