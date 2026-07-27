Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    Partial Class AnbardaryTransfer2Form

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New Panel()
            Me.pnlHeaderLeft = New Panel()
            Me.lblTransferNumber = New Label()
            Me.txtTransferNumber = New TextBox()
            Me.lblTransferDate = New Label()
            Me.txtTransferDate = New TextBox()
            Me.pnlHeaderRight = New Panel()
            Me.lblFromWarehouse = New Label()
            Me.cmbFromWarehouse = New ComboBox()
            Me.lblToWarehouse = New Label()
            Me.cmbToWarehouse = New ComboBox()
            Me.lblDescription = New Label()
            Me.txtDescription = New TextBox()
            Me.pnlGridToolbar = New Panel()
            Me.btnAddRow = New Button()
            Me.btnDeleteRow = New Button()
            Me.dgvEntryLines = New DataGridView()
            Me.pnlBottom = New Panel()
            Me.btnSave = New Button()
            Me.btnSaveExit = New Button()
            Me.btnExit = New Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlHeaderLeft.SuspendLayout()
            Me.pnlHeaderRight.SuspendLayout()
            Me.pnlGridToolbar.SuspendLayout()
            CType(Me.dgvEntryLines, ISupportInitialize).BeginInit()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()

            ' lblTransferNumber
            Me.lblTransferNumber.Text = "شماره حواله:"
            Me.lblTransferNumber.Location = New Point(10, 10)
            Me.lblTransferNumber.Size = New Size(85, 20)
            Me.lblTransferNumber.Font = New Font("Tahoma", 9.0!)
            Me.lblTransferNumber.TextAlign = ContentAlignment.MiddleRight

            ' txtTransferNumber
            Me.txtTransferNumber.Location = New Point(100, 8)
            Me.txtTransferNumber.Size = New Size(130, 24)
            Me.txtTransferNumber.Font = New Font("Tahoma", 9.0!)
            Me.txtTransferNumber.RightToLeft = RightToLeft.Yes
            Me.txtTransferNumber.Name = "txtTransferNumber"

            ' lblTransferDate
            Me.lblTransferDate.Text = "تاریخ حواله:"
            Me.lblTransferDate.Location = New Point(10, 42)
            Me.lblTransferDate.Size = New Size(85, 20)
            Me.lblTransferDate.Font = New Font("Tahoma", 9.0!)
            Me.lblTransferDate.TextAlign = ContentAlignment.MiddleRight

            ' txtTransferDate
            Me.txtTransferDate.Location = New Point(100, 40)
            Me.txtTransferDate.Size = New Size(130, 24)
            Me.txtTransferDate.Font = New Font("Tahoma", 9.0!)
            Me.txtTransferDate.RightToLeft = RightToLeft.Yes
            Me.txtTransferDate.Name = "txtTransferDate"

            ' pnlHeaderLeft
            Me.pnlHeaderLeft.Controls.Add(Me.lblTransferNumber)
            Me.pnlHeaderLeft.Controls.Add(Me.txtTransferNumber)
            Me.pnlHeaderLeft.Controls.Add(Me.lblTransferDate)
            Me.pnlHeaderLeft.Controls.Add(Me.txtTransferDate)
            Me.pnlHeaderLeft.Location = New Point(0, 0)
            Me.pnlHeaderLeft.Size = New Size(280, 80)
            Me.pnlHeaderLeft.Name = "pnlHeaderLeft"
            Me.pnlHeaderLeft.BorderStyle = BorderStyle.None

            ' lblFromWarehouse
            Me.lblFromWarehouse.Text = "انبار مبدا :"
            Me.lblFromWarehouse.Location = New Point(10, 10)
            Me.lblFromWarehouse.Size = New Size(80, 20)
            Me.lblFromWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.lblFromWarehouse.TextAlign = ContentAlignment.MiddleRight

            ' cmbFromWarehouse
            Me.cmbFromWarehouse.Location = New Point(95, 8)
            Me.cmbFromWarehouse.Size = New Size(200, 24)
            Me.cmbFromWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.cmbFromWarehouse.RightToLeft = RightToLeft.Yes
            Me.cmbFromWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbFromWarehouse.Name = "cmbFromWarehouse"

            ' lblToWarehouse
            Me.lblToWarehouse.Text = "انبار مقصد :"
            Me.lblToWarehouse.Location = New Point(10, 42)
            Me.lblToWarehouse.Size = New Size(80, 20)
            Me.lblToWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.lblToWarehouse.TextAlign = ContentAlignment.MiddleRight

            ' cmbToWarehouse
            Me.cmbToWarehouse.Location = New Point(95, 40)
            Me.cmbToWarehouse.Size = New Size(200, 24)
            Me.cmbToWarehouse.Font = New Font("Tahoma", 9.0!)
            Me.cmbToWarehouse.RightToLeft = RightToLeft.Yes
            Me.cmbToWarehouse.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbToWarehouse.Name = "cmbToWarehouse"

            ' pnlHeaderRight
            Me.pnlHeaderRight.Controls.Add(Me.lblFromWarehouse)
            Me.pnlHeaderRight.Controls.Add(Me.cmbFromWarehouse)
            Me.pnlHeaderRight.Controls.Add(Me.lblToWarehouse)
            Me.pnlHeaderRight.Controls.Add(Me.cmbToWarehouse)
            Me.pnlHeaderRight.Location = New Point(290, 0)
            Me.pnlHeaderRight.Size = New Size(340, 80)
            Me.pnlHeaderRight.Name = "pnlHeaderRight"

            ' lblDescription
            Me.lblDescription.Text = "توضیحات:"
            Me.lblDescription.Location = New Point(640, 10)
            Me.lblDescription.Size = New Size(70, 20)
            Me.lblDescription.Font = New Font("Tahoma", 9.0!)
            Me.lblDescription.TextAlign = ContentAlignment.MiddleRight

            ' txtDescription
            Me.txtDescription.Location = New Point(715, 8)
            Me.txtDescription.Size = New Size(200, 70)
            Me.txtDescription.Font = New Font("Tahoma", 9.0!)
            Me.txtDescription.RightToLeft = RightToLeft.Yes
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"

            ' pnlHeader
            Me.pnlHeader.Controls.Add(Me.pnlHeaderLeft)
            Me.pnlHeader.Controls.Add(Me.pnlHeaderRight)
            Me.pnlHeader.Controls.Add(Me.lblDescription)
            Me.pnlHeader.Controls.Add(Me.txtDescription)
            Me.pnlHeader.Dock = DockStyle.Top
            Me.pnlHeader.Height = 90
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.BackColor = Color.FromArgb(240, 248, 255)
            Me.pnlHeader.Padding = New Padding(8)

            ' btnAddRow
            Me.btnAddRow.Text = "+ افزودن ردیف"
            Me.btnAddRow.Location = New Point(8, 6)
            Me.btnAddRow.Size = New Size(120, 26)
            Me.btnAddRow.Font = New Font("Tahoma", 9.0!)
            Me.btnAddRow.BackColor = Color.FromArgb(0, 120, 180)
            Me.btnAddRow.ForeColor = Color.White
            Me.btnAddRow.FlatStyle = FlatStyle.Flat
            Me.btnAddRow.Name = "btnAddRow"

            ' btnDeleteRow
            Me.btnDeleteRow.Text = "حذف ردیف"
            Me.btnDeleteRow.Location = New Point(135, 6)
            Me.btnDeleteRow.Size = New Size(100, 26)
            Me.btnDeleteRow.Font = New Font("Tahoma", 9.0!)
            Me.btnDeleteRow.BackColor = Color.FromArgb(180, 60, 60)
            Me.btnDeleteRow.ForeColor = Color.White
            Me.btnDeleteRow.FlatStyle = FlatStyle.Flat
            Me.btnDeleteRow.Name = "btnDeleteRow"

            ' pnlGridToolbar
            Me.pnlGridToolbar.Controls.Add(Me.btnAddRow)
            Me.pnlGridToolbar.Controls.Add(Me.btnDeleteRow)
            Me.pnlGridToolbar.Dock = DockStyle.Top
            Me.pnlGridToolbar.Height = 38
            Me.pnlGridToolbar.Name = "pnlGridToolbar"
            Me.pnlGridToolbar.BackColor = Color.FromArgb(235, 245, 255)

            ' dgvEntryLines
            Me.dgvEntryLines.Dock = DockStyle.Fill
            Me.dgvEntryLines.Name = "dgvEntryLines"
            Me.dgvEntryLines.AllowUserToAddRows = False
            Me.dgvEntryLines.AllowUserToDeleteRows = False
            Me.dgvEntryLines.RowHeadersVisible = False
            Me.dgvEntryLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvEntryLines.RightToLeft = RightToLeft.Yes

            ' btnSave
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.Location = New Point(8, 8)
            Me.btnSave.Size = New Size(90, 30)
            Me.btnSave.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.btnSave.BackColor = Color.FromArgb(0, 140, 70)
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.Name = "btnSave"

            ' btnSaveExit
            Me.btnSaveExit.Text = "ثبت و خروج"
            Me.btnSaveExit.Location = New Point(105, 8)
            Me.btnSaveExit.Size = New Size(110, 30)
            Me.btnSaveExit.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            Me.btnSaveExit.BackColor = Color.FromArgb(0, 100, 180)
            Me.btnSaveExit.ForeColor = Color.White
            Me.btnSaveExit.FlatStyle = FlatStyle.Flat
            Me.btnSaveExit.Name = "btnSaveExit"

            ' btnExit
            Me.btnExit.Text = "خروج"
            Me.btnExit.Location = New Point(222, 8)
            Me.btnExit.Size = New Size(80, 30)
            Me.btnExit.Font = New Font("Tahoma", 9.5!)
            Me.btnExit.BackColor = Color.FromArgb(100, 100, 100)
            Me.btnExit.ForeColor = Color.White
            Me.btnExit.FlatStyle = FlatStyle.Flat
            Me.btnExit.Name = "btnExit"

            ' pnlBottom
            Me.pnlBottom.Controls.Add(Me.btnSave)
            Me.pnlBottom.Controls.Add(Me.btnSaveExit)
            Me.pnlBottom.Controls.Add(Me.btnExit)
            Me.pnlBottom.Dock = DockStyle.Bottom
            Me.pnlBottom.Height = 48
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.BackColor = Color.FromArgb(240, 240, 245)

            ' AnbardaryTransfer2Form
            Me.ClientSize = New Size(960, 600)
            Me.Controls.Add(Me.dgvEntryLines)
            Me.Controls.Add(Me.pnlGridToolbar)
            Me.Controls.Add(Me.pnlHeader)
            Me.Controls.Add(Me.pnlBottom)
            Me.Name = "AnbardaryTransfer2Form"
            Me.Text = "حواله بین انبارها"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent

            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeaderLeft.ResumeLayout(False)
            Me.pnlHeaderRight.ResumeLayout(False)
            Me.pnlGridToolbar.ResumeLayout(False)
            CType(Me.dgvEntryLines, ISupportInitialize).EndInit()
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents pnlHeader As Panel
        Friend WithEvents pnlHeaderLeft As Panel
        Friend WithEvents pnlHeaderRight As Panel
        Friend WithEvents lblTransferNumber As Label
        Friend WithEvents txtTransferNumber As TextBox
        Friend WithEvents lblTransferDate As Label
        Friend WithEvents txtTransferDate As TextBox
        Friend WithEvents lblFromWarehouse As Label
        Friend WithEvents cmbFromWarehouse As ComboBox
        Friend WithEvents lblToWarehouse As Label
        Friend WithEvents cmbToWarehouse As ComboBox
        Friend WithEvents lblDescription As Label
        Friend WithEvents txtDescription As TextBox
        Friend WithEvents pnlGridToolbar As Panel
        Friend WithEvents btnAddRow As Button
        Friend WithEvents btnDeleteRow As Button
        Friend WithEvents dgvEntryLines As DataGridView
        Friend WithEvents pnlBottom As Panel
        Friend WithEvents btnSave As Button
        Friend WithEvents btnSaveExit As Button
        Friend WithEvents btnExit As Button

    End Class
End Namespace
