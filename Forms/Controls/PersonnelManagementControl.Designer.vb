Namespace Negar.Forms.Controls
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PersonnelManagementControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.dgvPersonnel = New System.Windows.Forms.DataGridView()
        Me.pnlEditor = New System.Windows.Forms.Panel()
        Me.lblFullName = New System.Windows.Forms.Label()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.lblRole = New System.Windows.Forms.Label()
        Me.txtRole = New System.Windows.Forms.TextBox()
        Me.lblNationalCode = New System.Windows.Forms.Label()
        Me.txtNationalCode = New System.Windows.Forms.TextBox()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.txtPhone = New System.Windows.Forms.TextBox()
        Me.lblDepartment = New System.Windows.Forms.Label()
        Me.cmbDepartment = New System.Windows.Forms.ComboBox()
        Me.chkIsActive = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        CType(Me.dgvPersonnel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEditor.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvPersonnel
        '
        Me.dgvPersonnel.AllowUserToAddRows = False
        Me.dgvPersonnel.AllowUserToDeleteRows = False
        Me.dgvPersonnel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPersonnel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPersonnel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPersonnel.Location = New System.Drawing.Point(0, 200)
        Me.dgvPersonnel.MultiSelect = False
        Me.dgvPersonnel.Name = "dgvPersonnel"
        Me.dgvPersonnel.ReadOnly = True
        Me.dgvPersonnel.RowHeadersVisible = False
        Me.dgvPersonnel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPersonnel.Size = New System.Drawing.Size(800, 400)
        Me.dgvPersonnel.TabIndex = 1
        '
        'pnlEditor
        '
        Me.pnlEditor.Controls.Add(Me.lblFullName)
        Me.pnlEditor.Controls.Add(Me.txtFullName)
        Me.pnlEditor.Controls.Add(Me.lblRole)
        Me.pnlEditor.Controls.Add(Me.txtRole)
        Me.pnlEditor.Controls.Add(Me.lblNationalCode)
        Me.pnlEditor.Controls.Add(Me.txtNationalCode)
        Me.pnlEditor.Controls.Add(Me.lblPhone)
        Me.pnlEditor.Controls.Add(Me.txtPhone)
        Me.pnlEditor.Controls.Add(Me.lblDepartment)
        Me.pnlEditor.Controls.Add(Me.cmbDepartment)
        Me.pnlEditor.Controls.Add(Me.chkIsActive)
        Me.pnlEditor.Controls.Add(Me.btnSave)
        Me.pnlEditor.Controls.Add(Me.btnCancel)
        Me.pnlEditor.Controls.Add(Me.btnDelete)
        Me.pnlEditor.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlEditor.Location = New System.Drawing.Point(0, 0)
        Me.pnlEditor.Name = "pnlEditor"
        Me.pnlEditor.Size = New System.Drawing.Size(800, 200)
        Me.pnlEditor.TabIndex = 0
        '
        'lblFullName
        '
        Me.lblFullName.AutoSize = True
        Me.lblFullName.Location = New System.Drawing.Point(680, 25)
        Me.lblFullName.Name = "lblFullName"
        Me.lblFullName.Size = New System.Drawing.Size(100, 14)
        Me.lblFullName.TabIndex = 0
        Me.lblFullName.Text = "نام و نام خانوادگی:"
        '
        'txtFullName
        '
        Me.txtFullName.Location = New System.Drawing.Point(450, 22)
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.Size = New System.Drawing.Size(220, 22)
        Me.txtFullName.TabIndex = 1
        '
        'lblRole
        '
        Me.lblRole.AutoSize = True
        Me.lblRole.Location = New System.Drawing.Point(340, 25)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(35, 14)
        Me.lblRole.TabIndex = 2
        Me.lblRole.Text = "سمت:"
        '
        'txtRole
        '
        Me.txtRole.Location = New System.Drawing.Point(130, 22)
        Me.txtRole.Name = "txtRole"
        Me.txtRole.Size = New System.Drawing.Size(200, 22)
        Me.txtRole.TabIndex = 3
        '
        'lblNationalCode
        '
        Me.lblNationalCode.AutoSize = True
        Me.lblNationalCode.Location = New System.Drawing.Point(680, 65)
        Me.lblNationalCode.Name = "lblNationalCode"
        Me.lblNationalCode.Size = New System.Drawing.Size(50, 14)
        Me.lblNationalCode.TabIndex = 4
        Me.lblNationalCode.Text = "کد ملی:"
        '
        'txtNationalCode
        '
        Me.txtNationalCode.Location = New System.Drawing.Point(450, 62)
        Me.txtNationalCode.Name = "txtNationalCode"
        Me.txtNationalCode.Size = New System.Drawing.Size(220, 22)
        Me.txtNationalCode.TabIndex = 5
        '
        'lblPhone
        '
        Me.lblPhone.AutoSize = True
        Me.lblPhone.Location = New System.Drawing.Point(340, 65)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(70, 14)
        Me.lblPhone.TabIndex = 6
        Me.lblPhone.Text = "شماره تماس:"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(130, 62)
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(200, 22)
        Me.txtPhone.TabIndex = 7
        '
        'lblDepartment
        '
        Me.lblDepartment.AutoSize = True
        Me.lblDepartment.Location = New System.Drawing.Point(680, 105)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.Size = New System.Drawing.Size(35, 14)
        Me.lblDepartment.TabIndex = 8
        Me.lblDepartment.Text = "بخش:"
        '
        'cmbDepartment
        '
        Me.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDepartment.FormattingEnabled = True
        Me.cmbDepartment.Location = New System.Drawing.Point(450, 102)
        Me.cmbDepartment.Name = "cmbDepartment"
        Me.cmbDepartment.Size = New System.Drawing.Size(220, 22)
        Me.cmbDepartment.TabIndex = 9
        '
        'chkIsActive
        '
        Me.chkIsActive.AutoSize = True
        Me.chkIsActive.Checked = True
        Me.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIsActive.Location = New System.Drawing.Point(230, 104)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(100, 18)
        Me.chkIsActive.TabIndex = 10
        Me.chkIsActive.Text = "وضعیت فعالیت"
        Me.chkIsActive.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(30, 120, 60)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(600, 150)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "ثبت"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(490, 150)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "انصراف"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.Maroon
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(380, 150)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "حذف"
        Me.btnDelete.UseVisualStyleBackColor = False
        Me.btnDelete.Enabled = False
        '
        'PersonnelManagementControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvPersonnel)
        Me.Controls.Add(Me.pnlEditor)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Name = "PersonnelManagementControl"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Size = New System.Drawing.Size(800, 600)
        CType(Me.dgvPersonnel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEditor.ResumeLayout(False)
        Me.pnlEditor.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvPersonnel As System.Windows.Forms.DataGridView
    Friend WithEvents pnlEditor As System.Windows.Forms.Panel
    Friend WithEvents lblFullName As System.Windows.Forms.Label
    Friend WithEvents txtFullName As System.Windows.Forms.TextBox
    Friend WithEvents lblRole As System.Windows.Forms.Label
    Friend WithEvents txtRole As System.Windows.Forms.TextBox
    Friend WithEvents lblNationalCode As System.Windows.Forms.Label
    Friend WithEvents txtNationalCode As System.Windows.Forms.TextBox
    Friend WithEvents lblPhone As System.Windows.Forms.Label
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents lblDepartment As System.Windows.Forms.Label
    Friend WithEvents cmbDepartment As System.Windows.Forms.ComboBox
    Friend WithEvents chkIsActive As System.Windows.Forms.CheckBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class

End Namespace
