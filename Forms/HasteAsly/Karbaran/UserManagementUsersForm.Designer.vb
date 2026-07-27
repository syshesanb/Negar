Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class UserManagementUsersForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents splitMain As SplitContainer
        Friend WithEvents splitLeft As SplitContainer
        Friend WithEvents dgvUsers As DataGridView
        Friend WithEvents txtUsername As TextBox
        Friend WithEvents txtPassword As TextBox
        Friend WithEvents txtFullName As TextBox
        Friend WithEvents cmbUserType As ComboBox
        Friend WithEvents chkActive As CheckBox
        Friend WithEvents btnNewUser As Button
        Friend WithEvents btnSaveUser As Button
        Friend WithEvents btnDeleteUser As Button
        Friend WithEvents btnRefresh As Button
        Friend WithEvents numMaxCompanies As NumericUpDown
        Friend WithEvents numMaxFiscalYears As NumericUpDown
        Private lblUsername As Label
        Private lblPassword As Label
        Private lblFullName As Label
        Private lblUserType As Label
        Private lblMaxCompanies As Label
        Private lblMaxFiscalYears As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.splitMain = New SplitContainer()
            Me.splitLeft = New SplitContainer()
            Me.lblUsername = New Label()
            Me.txtUsername = New TextBox()
            Me.lblPassword = New Label()
            Me.txtPassword = New TextBox()
            Me.lblFullName = New Label()
            Me.txtFullName = New TextBox()
            Me.lblUserType = New Label()
            Me.cmbUserType = New ComboBox()
            Me.lblMaxCompanies = New Label()
            Me.numMaxCompanies = New NumericUpDown()
            Me.lblMaxFiscalYears = New Label()
            Me.numMaxFiscalYears = New NumericUpDown()
            Me.chkActive = New CheckBox()
            Me.btnNewUser = New Button()
            Me.btnSaveUser = New Button()
            Me.btnDeleteUser = New Button()
            Me.btnRefresh = New Button()
            Me.dgvUsers = New DataGridView()
            CType(Me.splitMain, ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            CType(Me.splitLeft, ISupportInitialize).BeginInit()
            Me.splitLeft.Panel1.SuspendLayout()
            Me.splitLeft.Panel2.SuspendLayout()
            Me.splitLeft.SuspendLayout()
            CType(Me.numMaxCompanies, ISupportInitialize).BeginInit()
            CType(Me.numMaxFiscalYears, ISupportInitialize).BeginInit()
            CType(Me.dgvUsers, ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1284, 550)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Name = "UserManagementUsersForm"
            Me.Text = "مدیریت کاربران"

            Me.splitMain.Dock = DockStyle.Fill
            Me.splitMain.SplitterDistance = 564
            Me.splitMain.Panel1.Controls.Add(Me.splitLeft)
            Me.splitMain.Panel2.Controls.Add(Me.dgvUsers)

            Me.splitLeft.Dock = DockStyle.Fill
            Me.splitLeft.Orientation = Orientation.Horizontal
            Me.splitLeft.SplitterDistance = 180
            Me.splitLeft.Panel1.Controls.Add(Me.lblUsername)
            Me.splitLeft.Panel1.Controls.Add(Me.txtUsername)
            Me.splitLeft.Panel1.Controls.Add(Me.lblPassword)
            Me.splitLeft.Panel1.Controls.Add(Me.txtPassword)
            Me.splitLeft.Panel1.Controls.Add(Me.lblFullName)
            Me.splitLeft.Panel1.Controls.Add(Me.txtFullName)
            Me.splitLeft.Panel1.Controls.Add(Me.lblUserType)
            Me.splitLeft.Panel1.Controls.Add(Me.cmbUserType)
            Me.splitLeft.Panel1.Controls.Add(Me.lblMaxCompanies)
            Me.splitLeft.Panel1.Controls.Add(Me.numMaxCompanies)
            Me.splitLeft.Panel1.Controls.Add(Me.lblMaxFiscalYears)
            Me.splitLeft.Panel1.Controls.Add(Me.numMaxFiscalYears)
            Me.splitLeft.Panel1.Controls.Add(Me.chkActive)
            Me.splitLeft.Panel1.Controls.Add(Me.btnNewUser)
            Me.splitLeft.Panel1.Controls.Add(Me.btnSaveUser)
            Me.splitLeft.Panel1.Controls.Add(Me.btnDeleteUser)
            Me.splitLeft.Panel1.Controls.Add(Me.btnRefresh)
            Me.splitLeft.Panel2.Controls.Add(Me.dgvUsers)

            Me.lblUsername.AutoSize = True
            Me.lblUsername.Location = New Point(497, 21)
            Me.lblUsername.Text = "نام کاربری"
            Me.txtUsername.Location = New Point(300, 18) : Me.txtUsername.Size = New Size(180, 22)
            Me.lblPassword.AutoSize = True
            Me.lblPassword.Location = New Point(497, 57)
            Me.lblPassword.Text = "رمز عبور"
            Me.txtPassword.Location = New Point(300, 54) : Me.txtPassword.Size = New Size(180, 22) : Me.txtPassword.UseSystemPasswordChar = True
            Me.lblFullName.AutoSize = True
            Me.lblFullName.Location = New Point(211, 21)
            Me.lblFullName.Text = "نام کامل"
            Me.txtFullName.Location = New Point(18, 18) : Me.txtFullName.Size = New Size(180, 22)
            Me.lblUserType.AutoSize = True
            Me.lblUserType.Location = New Point(213, 57)
            Me.lblUserType.Text = "نوع کاربر"
            Me.cmbUserType.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbUserType.Location = New Point(18, 54) : Me.cmbUserType.Size = New Size(180, 22)

            Me.lblMaxCompanies.AutoSize = True
            Me.lblMaxCompanies.Location = New Point(485, 93)
            Me.lblMaxCompanies.Text = "سقف شرکت (0 نامحدود)"
            Me.numMaxCompanies.Location = New Point(300, 90) : Me.numMaxCompanies.Size = New Size(180, 22) : Me.numMaxCompanies.Maximum = 1000 : Me.numMaxCompanies.Minimum = 0

            Me.lblMaxFiscalYears.AutoSize = True
            Me.lblMaxFiscalYears.Location = New Point(203, 93)
            Me.lblMaxFiscalYears.Text = "سقف سال مالی (0 نامحدود)"
            Me.numMaxFiscalYears.Location = New Point(18, 90) : Me.numMaxFiscalYears.Size = New Size(180, 22) : Me.numMaxFiscalYears.Maximum = 1000 : Me.numMaxFiscalYears.Minimum = 0

            Me.chkActive.AutoSize = True
            Me.chkActive.Checked = True
            Me.chkActive.CheckState = CheckState.Checked
            Me.chkActive.Location = New Point(456, 134)
            Me.chkActive.Text = "فعال"
            Me.btnNewUser.Location = New Point(18, 134) : Me.btnNewUser.Size = New Size(75, 26) : Me.btnNewUser.Text = "جدید"
            Me.btnSaveUser.Location = New Point(102, 134) : Me.btnSaveUser.Size = New Size(75, 26) : Me.btnSaveUser.Text = "ذخیره"
            Me.btnDeleteUser.Location = New Point(186, 134) : Me.btnDeleteUser.Size = New Size(75, 26) : Me.btnDeleteUser.Text = "حذف"
            Me.btnRefresh.Location = New Point(270, 134) : Me.btnRefresh.Size = New Size(90, 26) : Me.btnRefresh.Text = "بازخوانی"

            Me.dgvUsers.Dock = DockStyle.Fill
            Me.dgvUsers.AllowUserToAddRows = False
            Me.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvUsers.ReadOnly = True
            Me.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            Me.Controls.Add(Me.splitMain)

            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            CType(Me.splitMain, ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            Me.splitLeft.Panel1.ResumeLayout(False)
            Me.splitLeft.Panel1.PerformLayout()
            Me.splitLeft.Panel2.ResumeLayout(False)
            CType(Me.splitLeft, ISupportInitialize).EndInit()
            Me.splitLeft.ResumeLayout(False)
            CType(Me.numMaxCompanies, ISupportInitialize).EndInit()
            CType(Me.numMaxFiscalYears, ISupportInitialize).EndInit()
            CType(Me.dgvUsers, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
