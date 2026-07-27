Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class HesabdaryCodingForm
        Inherits Form

        Private components As IContainer

        ' نوار ناوبری بالا
        Friend WithEvents pnlTop As Panel
        Friend WithEvents lblCurrentLevel As Label
        Friend WithEvents btnNew As Button
        Friend WithEvents lblExpand As Label
        Friend WithEvents cmbExpandToLevel As ComboBox
        Friend WithEvents pnlSath As Panel
        Friend WithEvents lblSathInfo As Label
        Friend WithEvents pnlSearch As Panel
        Friend WithEvents lblSearchLevel As Label
        Friend WithEvents cmbSearchLevel As ComboBox
        Friend WithEvents txtSearchCode As TextBox
        Friend WithEvents txtSearchName As TextBox

        ' پنل داده (ویرایش / ایجاد) - سمت راست، پیش‌فرض پنهان
        Friend WithEvents pnlData As Panel
        Friend WithEvents lblDataCode As Label
        Friend WithEvents txtAccountCode As TextBox
        Friend WithEvents lblDataName As Label
        Friend WithEvents txtAccountName As TextBox
        Friend WithEvents lblDataType As Label
        Friend WithEvents cmbAccountType As ComboBox
        Friend WithEvents lblAccountNature As Label
        Friend WithEvents cmbAccountNature As ComboBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        ' گرید سرفصل‌ها
        Friend WithEvents dgvAccounts As DataGridView

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.lblCurrentLevel = New System.Windows.Forms.Label()
            Me.btnNew = New System.Windows.Forms.Button()
            Me.lblExpand = New System.Windows.Forms.Label()
            Me.cmbExpandToLevel = New System.Windows.Forms.ComboBox()
            Me.pnlSath = New System.Windows.Forms.Panel()
            Me.lblSathInfo = New System.Windows.Forms.Label()
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
            Me.lblDataType = New System.Windows.Forms.Label()
            Me.cmbAccountType = New System.Windows.Forms.ComboBox()
            Me.lblAccountNature = New System.Windows.Forms.Label()
            Me.cmbAccountNature = New System.Windows.Forms.ComboBox()
            Me.chkActive = New System.Windows.Forms.CheckBox()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.dgvAccounts = New System.Windows.Forms.DataGridView()
            Me.pnlTop.SuspendLayout()
            Me.pnlSath.SuspendLayout()
            Me.pnlSearch.SuspendLayout()
            Me.pnlData.SuspendLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(251, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.lblCurrentLevel)
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.lblExpand)
            Me.pnlTop.Controls.Add(Me.cmbExpandToLevel)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(1100, 38)
            Me.pnlTop.TabIndex = 0
            '
            'lblCurrentLevel
            '
            Me.lblCurrentLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblCurrentLevel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblCurrentLevel.Location = New System.Drawing.Point(530, 10)
            Me.lblCurrentLevel.Name = "lblCurrentLevel"
            Me.lblCurrentLevel.Size = New System.Drawing.Size(560, 20)
            Me.lblCurrentLevel.TabIndex = 0
            Me.lblCurrentLevel.Text = "سطح جاری: حسابهای اصلی (سطح اول)"
            Me.lblCurrentLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'btnNew
            '
            Me.btnNew.Location = New System.Drawing.Point(10, 6)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(144, 26)
            Me.btnNew.TabIndex = 1
            Me.btnNew.Text = "جدید"
            '
            'lblExpand
            '
            Me.lblExpand.BackColor = System.Drawing.Color.Transparent
            Me.lblExpand.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblExpand.Location = New System.Drawing.Point(350, 10)
            Me.lblExpand.Name = "lblExpand"
            Me.lblExpand.Size = New System.Drawing.Size(159, 20)
            Me.lblExpand.TabIndex = 2
            Me.lblExpand.Text = "نمایش سرفصلها تا سطح:"
            Me.lblExpand.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cmbExpandToLevel
            '
            Me.cmbExpandToLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbExpandToLevel.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.cmbExpandToLevel.FormattingEnabled = True
            Me.cmbExpandToLevel.Items.AddRange(New Object() {"گروه (بستن همه)", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"})
            Me.cmbExpandToLevel.Location = New System.Drawing.Point(184, 7)
            Me.cmbExpandToLevel.Name = "cmbExpandToLevel"
            Me.cmbExpandToLevel.Size = New System.Drawing.Size(160, 22)
            Me.cmbExpandToLevel.TabIndex = 3
            '
            'pnlSath
            '
            Me.pnlSath.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlSath.Controls.Add(Me.lblSathInfo)
            Me.pnlSath.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSath.Location = New System.Drawing.Point(0, 38)
            Me.pnlSath.Name = "pnlSath"
            Me.pnlSath.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
            Me.pnlSath.Size = New System.Drawing.Size(1100, 35)
            Me.pnlSath.TabIndex = 3
            '
            'lblSathInfo
            '
            Me.lblSathInfo.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSathInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSathInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
            Me.lblSathInfo.Location = New System.Drawing.Point(10, 0)
            Me.lblSathInfo.Name = "lblSathInfo"
            Me.lblSathInfo.Size = New System.Drawing.Size(1080, 35)
            Me.lblSathInfo.TabIndex = 0
            Me.lblSathInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlSearch
            '
            Me.pnlSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(230, Byte), Integer))
            Me.pnlSearch.Controls.Add(Me.lblSearchLevel)
            Me.pnlSearch.Controls.Add(Me.cmbSearchLevel)
            Me.pnlSearch.Controls.Add(Me.txtSearchCode)
            Me.pnlSearch.Controls.Add(Me.txtSearchName)
            Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSearch.Location = New System.Drawing.Point(0, 73)
            Me.pnlSearch.Name = "pnlSearch"
            Me.pnlSearch.Size = New System.Drawing.Size(1100, 38)
            Me.pnlSearch.TabIndex = 1
            '
            'lblSearchLevel
            '
            Me.lblSearchLevel.Location = New System.Drawing.Point(958, 12)
            Me.lblSearchLevel.Name = "lblSearchLevel"
            Me.lblSearchLevel.Size = New System.Drawing.Size(130, 18)
            Me.lblSearchLevel.TabIndex = 0
            Me.lblSearchLevel.Text = "انتخاب سطح جستجو:"
            '
            'cmbSearchLevel
            '
            Me.cmbSearchLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSearchLevel.Items.AddRange(New Object() {"سطح جاری", "در تمام سطوح"})
            Me.cmbSearchLevel.Location = New System.Drawing.Point(842, 9)
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
            Me.pnlData.Controls.Add(Me.lblDataType)
            Me.pnlData.Controls.Add(Me.cmbAccountType)
            Me.pnlData.Controls.Add(Me.lblAccountNature)
            Me.pnlData.Controls.Add(Me.cmbAccountNature)
            Me.pnlData.Controls.Add(Me.chkActive)
            Me.pnlData.Controls.Add(Me.btnSave)
            Me.pnlData.Controls.Add(Me.btnCancel)
            Me.pnlData.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlData.Location = New System.Drawing.Point(730, 111)
            Me.pnlData.Name = "pnlData"
            Me.pnlData.Size = New System.Drawing.Size(370, 569)
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
            'lblDataType
            '
            Me.lblDataType.Location = New System.Drawing.Point(10, 90)
            Me.lblDataType.Name = "lblDataType"
            Me.lblDataType.Size = New System.Drawing.Size(100, 20)
            Me.lblDataType.TabIndex = 4
            Me.lblDataType.Text = "نوع حساب:"
            '
            'cmbAccountType
            '
            Me.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbAccountType.Location = New System.Drawing.Point(120, 86)
            Me.cmbAccountType.Name = "cmbAccountType"
            Me.cmbAccountType.Size = New System.Drawing.Size(220, 22)
            Me.cmbAccountType.TabIndex = 5
            '
            'lblAccountNature
            '
            Me.lblAccountNature.Location = New System.Drawing.Point(10, 126)
            Me.lblAccountNature.Name = "lblAccountNature"
            Me.lblAccountNature.Size = New System.Drawing.Size(110, 20)
            Me.lblAccountNature.TabIndex = 6
            Me.lblAccountNature.Text = "ماهیت مانده حساب : "
            '
            'cmbAccountNature
            '
            Me.cmbAccountNature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbAccountNature.Location = New System.Drawing.Point(120, 122)
            Me.cmbAccountNature.Name = "cmbAccountNature"
            Me.cmbAccountNature.Size = New System.Drawing.Size(220, 22)
            Me.cmbAccountNature.TabIndex = 7
            '
            'chkActive
            '
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkActive.Location = New System.Drawing.Point(120, 158)
            Me.chkActive.Name = "chkActive"
            Me.chkActive.Size = New System.Drawing.Size(80, 24)
            Me.chkActive.TabIndex = 8
            Me.chkActive.Text = "فعال"
            '
            'btnSave
            '
            Me.btnSave.Location = New System.Drawing.Point(10, 200)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(110, 32)
            Me.btnSave.TabIndex = 9
            Me.btnSave.Text = "ذخیره"
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(130, 200)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(110, 32)
            Me.btnCancel.TabIndex = 10
            Me.btnCancel.Text = "انصراف"
            '
            'dgvAccounts
            '
            Me.dgvAccounts.AllowUserToAddRows = False
            Me.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAccounts.Location = New System.Drawing.Point(0, 111)
            Me.dgvAccounts.MultiSelect = False
            Me.dgvAccounts.Name = "dgvAccounts"
            Me.dgvAccounts.ReadOnly = True
            Me.dgvAccounts.RowHeadersVisible = False
            Me.dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvAccounts.Size = New System.Drawing.Size(730, 569)
            Me.dgvAccounts.TabIndex = 2
            '
            'HesabdaryCodingForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1100, 680)
            Me.Controls.Add(Me.dgvAccounts)
            Me.Controls.Add(Me.pnlData)
            Me.Controls.Add(Me.pnlSearch)
            Me.Controls.Add(Me.pnlSath)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdaryCodingForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "سرفصل حساب‌ها"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlSath.ResumeLayout(False)
            Me.pnlSearch.ResumeLayout(False)
            Me.pnlSearch.PerformLayout()
            Me.pnlData.ResumeLayout(False)
            Me.pnlData.PerformLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
