Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class HesabdaryTabdilDataSarfaslForm
        Inherits Form

        Private components As IContainer

        ' ─── Import Top ──────────────────────────────────────────────────
        Friend WithEvents pnlImportTop As Panel
        Friend WithEvents lblFileStatus As Label
        Friend WithEvents btnSelectFile As Button
        Friend WithEvents lblHeaderRow As Label
        Friend WithEvents nudHeaderRow As NumericUpDown
        Friend WithEvents btnPreview As Button
        Friend WithEvents btnSmartConvert As Button

        ' ─── ردیف مپینگ ───────────────────────────────────────
        Friend WithEvents pnlMappingCodes As Panel
        Friend WithEvents lblGoruh As Label
        Friend WithEvents cmbGoruh As ComboBox
        Friend WithEvents lblKol As Label
        Friend WithEvents cmbKol As ComboBox
        Friend WithEvents lblMoein As Label
        Friend WithEvents cmbMoein As ComboBox
        Friend WithEvents lblTafsili1 As Label
        Friend WithEvents cmbTafsili1 As ComboBox
        Friend WithEvents lblTafsili2 As Label
        Friend WithEvents cmbTafsili2 As ComboBox
        Friend WithEvents lblTafsili3 As Label
        Friend WithEvents cmbTafsili3 As ComboBox
        Friend WithEvents lblAccountName As Label
        Friend WithEvents cmbAccountName As ComboBox

        ' ─── DataGridView ─────────────────────────────────────────────────
        Friend WithEvents pnlDgvSarfasl As Panel
        Friend WithEvents dgvSarfaslPreview As DataGridView



        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()


            Me.pnlImportTop = New Panel()
            Me.lblFileStatus = New Label()
            Me.btnSelectFile = New Button()
            Me.lblHeaderRow = New Label()
            Me.nudHeaderRow = New NumericUpDown()
            Me.btnPreview = New Button()
            Me.btnSmartConvert = New Button()

            Me.pnlMappingCodes = New Panel()
            Me.lblGoruh = New Label()
            Me.cmbGoruh = New ComboBox()
            Me.lblKol = New Label()
            Me.cmbKol = New ComboBox()
            Me.lblMoein = New Label()
            Me.cmbMoein = New ComboBox()
            Me.lblTafsili1 = New Label()
            Me.cmbTafsili1 = New ComboBox()
            Me.lblTafsili2 = New System.Windows.Forms.Label()
            Me.cmbTafsili2 = New System.Windows.Forms.ComboBox()
            Me.lblTafsili3 = New System.Windows.Forms.Label()
            Me.cmbTafsili3 = New System.Windows.Forms.ComboBox()
            Me.lblAccountName = New System.Windows.Forms.Label()
            Me.cmbAccountName = New ComboBox()

            Me.pnlDgvSarfasl = New Panel()
            Me.dgvSarfaslPreview = New DataGridView()

            Me.pnlImportTop.SuspendLayout()
            Me.pnlMappingCodes.SuspendLayout()
            Me.pnlDgvSarfasl.SuspendLayout()
            CType(Me.nudHeaderRow, ISupportInitialize).BeginInit()
            CType(Me.dgvSarfaslPreview, ISupportInitialize).BeginInit()
            Me.SuspendLayout()


            ' ══════════════════════════════════════════════════════════════
            ' pnlImportTop
            ' ══════════════════════════════════════════════════════════════
            Me.pnlImportTop.Dock = DockStyle.Top
            Me.pnlImportTop.Height = 85
            Me.pnlImportTop.Padding = New Padding(6)
            Me.pnlImportTop.Controls.Add(Me.btnSmartConvert)
            Me.pnlImportTop.Controls.Add(Me.btnPreview)
            Me.pnlImportTop.Controls.Add(Me.nudHeaderRow)
            Me.pnlImportTop.Controls.Add(Me.lblHeaderRow)
            Me.pnlImportTop.Controls.Add(Me.btnSelectFile)
            Me.pnlImportTop.Controls.Add(Me.lblFileStatus)
            Me.pnlImportTop.Name = "pnlImportTop"

            Me.lblFileStatus.Location = New Point(8, 8)
            Me.lblFileStatus.Name = "lblFileStatus"
            Me.lblFileStatus.Size = New Size(300, 22)
            Me.lblFileStatus.Text = "فایلی انتخاب نشده است"
            Me.lblFileStatus.TextAlign = ContentAlignment.MiddleRight

            Me.btnSelectFile.Location = New Point(320, 6)
            Me.btnSelectFile.Name = "btnSelectFile"
            Me.btnSelectFile.Size = New Size(130, 28)
            Me.btnSelectFile.Text = "انتخاب فایل سرفصل"
            Me.btnSelectFile.UseVisualStyleBackColor = True

            Me.lblHeaderRow.Location = New Point(530, 10)
            Me.lblHeaderRow.Name = "lblHeaderRow"
            Me.lblHeaderRow.AutoSize = True
            Me.lblHeaderRow.Text = "ردیف سرستون:"

            Me.nudHeaderRow.Location = New Point(460, 8)
            Me.nudHeaderRow.Name = "nudHeaderRow"
            Me.nudHeaderRow.Minimum = 1D
            Me.nudHeaderRow.Maximum = 50D
            Me.nudHeaderRow.Value = 1D
            Me.nudHeaderRow.Size = New Size(60, 22)

            Me.btnPreview.Location = New Point(320, 44)
            Me.btnPreview.Name = "btnPreview"
            Me.btnPreview.Size = New Size(160, 30)
            Me.btnPreview.Text = "پیش‌نمایش اطلاعات سرفصل"
            Me.btnPreview.UseVisualStyleBackColor = True

            Me.btnSmartConvert.Location = New Point(150, 44)
            Me.btnSmartConvert.Name = "btnSmartConvert"
            Me.btnSmartConvert.Size = New Size(150, 30)
            Me.btnSmartConvert.Text = "تبدیل هوشمند سرفصل"
            Me.btnSmartConvert.UseVisualStyleBackColor = True
            Me.btnSmartConvert.Enabled = False

            ' ══════════════════════════════════════════════════════════════
            ' pnlMappingCodes  ─ ردیف مپینگ
            ' ══════════════════════════════════════════════════════════════
            Me.pnlMappingCodes.Dock = DockStyle.Top
            Me.pnlMappingCodes.Height = 46
            Me.pnlMappingCodes.Padding = New Padding(6, 4, 6, 0)
            Me.pnlMappingCodes.Controls.Add(Me.cmbAccountName)
            Me.pnlMappingCodes.Controls.Add(Me.lblAccountName)
            Me.pnlMappingCodes.Controls.Add(Me.cmbTafsili2)
            Me.pnlMappingCodes.Controls.Add(Me.lblTafsili2)
            Me.pnlMappingCodes.Controls.Add(Me.cmbTafsili3)
            Me.pnlMappingCodes.Controls.Add(Me.lblTafsili3)
            Me.pnlMappingCodes.Controls.Add(Me.cmbTafsili1)
            Me.pnlMappingCodes.Controls.Add(Me.lblTafsili1)
            Me.pnlMappingCodes.Controls.Add(Me.cmbMoein)
            Me.pnlMappingCodes.Controls.Add(Me.lblMoein)
            Me.pnlMappingCodes.Controls.Add(Me.cmbKol)
            Me.pnlMappingCodes.Controls.Add(Me.lblKol)
            Me.pnlMappingCodes.Controls.Add(Me.cmbGoruh)
            Me.pnlMappingCodes.Controls.Add(Me.lblGoruh)
            Me.pnlMappingCodes.Name = "pnlMappingCodes"

            ' از راست به چپ: گروه، کل، معین، تفصیلی1، تفصیلی2، تفصیلی3، نام حساب
            Me.lblGoruh.Location = New Point(960, 11)
            Me.lblGoruh.AutoSize = True
            Me.lblGoruh.Text = "گروه:"
            Me.lblGoruh.Name = "lblcmbGoruh"
            Me.cmbGoruh.Location = New Point(870, 8)
            Me.cmbGoruh.Name = "cmbGoruh"
            Me.cmbGoruh.Size = New Size(85, 22)
            Me.cmbGoruh.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblKol.Location = New Point(830, 11)
            Me.lblKol.AutoSize = True
            Me.lblKol.Text = "کل:"
            Me.lblKol.Name = "lblcmbKol"
            Me.cmbKol.Location = New Point(740, 8)
            Me.cmbKol.Name = "cmbKol"
            Me.cmbKol.Size = New Size(85, 22)
            Me.cmbKol.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblMoein.Location = New Point(700, 11)
            Me.lblMoein.AutoSize = True
            Me.lblMoein.Text = "معین:"
            Me.lblMoein.Name = "lblcmbMoein"
            Me.cmbMoein.Location = New Point(610, 8)
            Me.cmbMoein.Name = "cmbMoein"
            Me.cmbMoein.Size = New Size(85, 22)
            Me.cmbMoein.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblTafsili1.Location = New Point(570, 11)
            Me.lblTafsili1.AutoSize = True
            Me.lblTafsili1.Text = "تفصیلی۱:"
            Me.lblTafsili1.Name = "lblcmbTafsili1"
            Me.cmbTafsili1.Location = New Point(480, 8)
            Me.cmbTafsili1.Name = "cmbTafsili1"
            Me.cmbTafsili1.Size = New Size(85, 22)
            Me.cmbTafsili1.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblTafsili2.Location = New Point(440, 11)
            Me.lblTafsili2.AutoSize = True
            Me.lblTafsili2.Text = "تفصیلی۲:"
            Me.lblTafsili2.Name = "lblcmbTafsili2"
            Me.cmbTafsili2.Location = New Point(350, 8)
            Me.cmbTafsili2.Name = "cmbTafsili2"
            Me.cmbTafsili2.Size = New Size(85, 22)
            Me.cmbTafsili2.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblTafsili3.Location = New Point(310, 11)
            Me.lblTafsili3.AutoSize = True
            Me.lblTafsili3.Text = "تفصیلی۳:"
            Me.lblTafsili3.Name = "lblcmbTafsili3"
            Me.cmbTafsili3.Location = New Point(220, 8)
            Me.cmbTafsili3.Name = "cmbTafsili3"
            Me.cmbTafsili3.Size = New Size(85, 22)
            Me.cmbTafsili3.DropDownStyle = ComboBoxStyle.DropDownList
            Me.lblAccountName.Location = New Point(160, 11)
            Me.lblAccountName.AutoSize = True
            Me.lblAccountName.Text = "نام حساب:"
            Me.lblAccountName.Name = "lblcmbAccountName"
            Me.cmbAccountName.Location = New Point(70, 8)
            Me.cmbAccountName.Name = "cmbAccountName"
            Me.cmbAccountName.Size = New Size(85, 22)
            Me.cmbAccountName.DropDownStyle = ComboBoxStyle.DropDownList

            ' ══════════════════════════════════════════════════════════════
            ' pnlDgvSarfasl
            ' ══════════════════════════════════════════════════════════════
            Me.pnlDgvSarfasl.Dock = DockStyle.Fill
            Me.pnlDgvSarfasl.Controls.Add(Me.dgvSarfaslPreview)
            Me.pnlDgvSarfasl.Name = "pnlDgvSarfasl"

            Me.dgvSarfaslPreview.Dock = DockStyle.Fill
            Me.dgvSarfaslPreview.Name = "dgvSarfaslPreview"
            Me.dgvSarfaslPreview.ReadOnly = True
            Me.dgvSarfaslPreview.AllowUserToAddRows = False
            Me.dgvSarfaslPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Me.dgvSarfaslPreview.RowHeadersVisible = False
            Me.dgvSarfaslPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            ' ── Form ──────────────────────────────────────────────────────
            Me.AutoScaleDimensions = New SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1060, 580)

            Me.Controls.Add(Me.pnlDgvSarfasl)
            Me.Controls.Add(Me.pnlMappingCodes)
            Me.Controls.Add(Me.pnlImportTop)
            Me.Font = New Font("Tahoma", 8.25!)
            Me.MinimumSize = New Size(1060, 520)
            Me.Name = "HesabdaryTabdilDataSarfaslForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "تبدیل هوشمند سرفصل حسابها"

            Me.pnlImportTop.ResumeLayout(False)
            Me.pnlImportTop.PerformLayout()
            Me.pnlMappingCodes.ResumeLayout(False)
            Me.pnlMappingCodes.PerformLayout()
            Me.pnlDgvSarfasl.ResumeLayout(False)
            CType(Me.nudHeaderRow, ISupportInitialize).EndInit()
            CType(Me.dgvSarfaslPreview, ISupportInitialize).EndInit()

            Me.ResumeLayout(False)
        End Sub



    End Class
End Namespace

