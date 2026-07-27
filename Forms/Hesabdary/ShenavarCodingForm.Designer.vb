Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class ShenavarCodingForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents lblCurrentLevel As Label
        Friend WithEvents btnNew As Button
        Friend WithEvents pnlSearch As Panel
        Friend WithEvents lblSearchLevel As Label
        Friend WithEvents cmbSearchLevel As ComboBox
        Friend WithEvents txtSearchCode As TextBox
        Friend WithEvents txtSearchName As TextBox

        Friend WithEvents pnlData As Panel
        Friend WithEvents lblDataCode As Label
        Friend WithEvents txtAccountCode As TextBox
        Friend WithEvents lblDataName As Label
        Friend WithEvents txtAccountName As TextBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        Friend WithEvents dgvAccounts As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.lblCurrentLevel = New System.Windows.Forms.Label()
            Me.btnNew = New System.Windows.Forms.Button()
            Me.pnlSearch = New System.Windows.Forms.Panel()
            Me.lblSearchLevel = New System.Windows.Forms.Label()
            Me.cmbSearchLevel = New System.Windows.Forms.ComboBox()
            Me.txtSearchCode = New System.Windows.Forms.TextBox()
            Me.txtSearchName = New System.Windows.Forms.TextBox()
            Me.pnlData = New System.Windows.Forms.Panel()
            Me.lblDataCode = New System.Windows.Forms.Label()
            Me.txtAccountCode = New System.Windows.Forms.TextBox()
            Me.lblDataName = New System.Windows.Forms.Label()
            Me.txtAccountName = New System.Windows.Forms.TextBox()
            Me.chkActive = New System.Windows.Forms.CheckBox()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.dgvAccounts = New System.Windows.Forms.DataGridView()
            Me.pnlTop.SuspendLayout()
            Me.pnlSearch.SuspendLayout()
            Me.pnlData.SuspendLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(235, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.lblCurrentLevel)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(1100, 38)
            Me.pnlTop.TabIndex = 0
            '
            'lblCurrentLevel
            '
            Me.lblCurrentLevel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblCurrentLevel.Location = New System.Drawing.Point(10, 10)
            Me.lblCurrentLevel.Name = "lblCurrentLevel"
            Me.lblCurrentLevel.Size = New System.Drawing.Size(560, 20)
            Me.lblCurrentLevel.TabIndex = 0
            Me.lblCurrentLevel.Text = "سطح جاری: حسابهای شناور اصلی (سطح اول)"
            '
            'btnNew
            '
            Me.btnNew.Location = New System.Drawing.Point(956, 6)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(144, 26)
            Me.btnNew.TabIndex = 1
            Me.btnNew.Text = "جدید"
            '
            'pnlSearch
            '
            Me.pnlSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(230, Byte), Integer))
            Me.pnlSearch.Controls.Add(Me.lblSearchLevel)
            Me.pnlSearch.Controls.Add(Me.cmbSearchLevel)
            Me.pnlSearch.Controls.Add(Me.txtSearchCode)
            Me.pnlSearch.Controls.Add(Me.txtSearchName)
            Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSearch.Location = New System.Drawing.Point(0, 38)
            Me.pnlSearch.Name = "pnlSearch"
            Me.pnlSearch.Size = New System.Drawing.Size(1100, 38)
            Me.pnlSearch.TabIndex = 1
            '
            'lblSearchLevel
            '
            Me.lblSearchLevel.Location = New System.Drawing.Point(958, 9)
            Me.lblSearchLevel.Name = "lblSearchLevel"
            Me.lblSearchLevel.Size = New System.Drawing.Size(130, 18)
            Me.lblSearchLevel.TabIndex = 0
            Me.lblSearchLevel.Text = "انتخاب سطح جستجو:"
            '
            'cmbSearchLevel
            '
            Me.cmbSearchLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSearchLevel.Items.AddRange(New Object() {"سطح جاری", "در تمام سطوح"})
            Me.cmbSearchLevel.Location = New System.Drawing.Point(841, 8)
            Me.cmbSearchLevel.Name = "cmbSearchLevel"
            Me.cmbSearchLevel.Size = New System.Drawing.Size(110, 22)
            Me.cmbSearchLevel.TabIndex = 1
            '
            'txtSearchCode
            '
            Me.txtSearchCode.Location = New System.Drawing.Point(720, 9)
            Me.txtSearchCode.Name = "txtSearchCode"
            Me.txtSearchCode.Size = New System.Drawing.Size(118, 22)
            Me.txtSearchCode.TabIndex = 2
            '
            'txtSearchName
            '
            Me.txtSearchName.Location = New System.Drawing.Point(430, 9)
            Me.txtSearchName.Name = "txtSearchName"
            Me.txtSearchName.Size = New System.Drawing.Size(284, 22)
            Me.txtSearchName.TabIndex = 3
            '
            'pnlData
            '
            Me.pnlData.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlData.Controls.Add(Me.lblDataCode)
            Me.pnlData.Controls.Add(Me.txtAccountCode)
            Me.pnlData.Controls.Add(Me.lblDataName)
            Me.pnlData.Controls.Add(Me.txtAccountName)
            Me.pnlData.Controls.Add(Me.chkActive)
            Me.pnlData.Controls.Add(Me.btnSave)
            Me.pnlData.Controls.Add(Me.btnCancel)
            Me.pnlData.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlData.Location = New System.Drawing.Point(730, 76)
            Me.pnlData.Name = "pnlData"
            Me.pnlData.Size = New System.Drawing.Size(370, 604)
            Me.pnlData.TabIndex = 1
            Me.pnlData.Visible = False
            '
            'lblDataCode
            '
            Me.lblDataCode.Location = New System.Drawing.Point(10, 18)
            Me.lblDataCode.Name = "lblDataCode"
            Me.lblDataCode.Size = New System.Drawing.Size(100, 20)
            Me.lblDataCode.TabIndex = 0
            Me.lblDataCode.Text = "کد حساب:"
            '
            'txtAccountCode
            '
            Me.txtAccountCode.Location = New System.Drawing.Point(120, 14)
            Me.txtAccountCode.Name = "txtAccountCode"
            Me.txtAccountCode.Size = New System.Drawing.Size(220, 22)
            Me.txtAccountCode.TabIndex = 1
            '
            'lblDataName
            '
            Me.lblDataName.Location = New System.Drawing.Point(10, 54)
            Me.lblDataName.Name = "lblDataName"
            Me.lblDataName.Size = New System.Drawing.Size(100, 20)
            Me.lblDataName.TabIndex = 2
            Me.lblDataName.Text = "نام حساب:"
            '
            'txtAccountName
            '
            Me.txtAccountName.Location = New System.Drawing.Point(120, 50)
            Me.txtAccountName.Name = "txtAccountName"
            Me.txtAccountName.Size = New System.Drawing.Size(220, 22)
            Me.txtAccountName.TabIndex = 3
            '
            'chkActive
            '
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkActive.Location = New System.Drawing.Point(120, 86)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New System.Drawing.Size(80, 24)
            Me.chkActive.TabIndex = 4
            Me.chkActive.Text = "فعال"
            '
            'btnSave
            '
            Me.btnSave.Location = New System.Drawing.Point(10, 122)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(110, 32)
            Me.btnSave.TabIndex = 5
            Me.btnSave.Text = "ذخیره"
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(130, 122)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(110, 32)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "انصراف"
            '
            'dgvAccounts
            '
            Me.dgvAccounts.AllowUserToAddRows = False
            Me.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAccounts.Location = New System.Drawing.Point(0, 76)
            Me.dgvAccounts.MultiSelect = False
            Me.dgvAccounts.Name = "dgvAccounts"
            Me.dgvAccounts.ReadOnly = True
            Me.dgvAccounts.RowHeadersVisible = False
            Me.dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvAccounts.Size = New System.Drawing.Size(730, 604)
            Me.dgvAccounts.TabIndex = 2
            '
            'ShenavarCodingForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1100, 680)
            Me.Controls.Add(Me.dgvAccounts)
            Me.Controls.Add(Me.pnlData)
            Me.Controls.Add(Me.pnlSearch)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "ShenavarCodingForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "حسابهای شناور"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlSearch.ResumeLayout(False)
            Me.pnlSearch.PerformLayout()
            Me.pnlData.ResumeLayout(False)
            Me.pnlData.PerformLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
