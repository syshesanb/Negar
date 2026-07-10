Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class HesabdaryReport1Form
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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
            Me.dgvReports = New System.Windows.Forms.DataGridView()
            Me.btnNew = New System.Windows.Forms.Button()
            Dim pnlTop As New System.Windows.Forms.Panel()
            Dim colRowNo As New System.Windows.Forms.DataGridViewTextBoxColumn()
            Dim colEdit As New System.Windows.Forms.DataGridViewButtonColumn()
            Dim colDelete As New System.Windows.Forms.DataGridViewButtonColumn()
            Dim colCode As New System.Windows.Forms.DataGridViewTextBoxColumn()
            Dim colName As New System.Windows.Forms.DataGridViewTextBoxColumn()
            Dim colPrint As New System.Windows.Forms.DataGridViewButtonColumn()
            Dim colID As New System.Windows.Forms.DataGridViewTextBoxColumn()
            Dim headerCellStyle As New System.Windows.Forms.DataGridViewCellStyle()
            pnlTop.SuspendLayout()
            CType(Me.dgvReports, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
            pnlTop.Controls.Add(Me.btnNew)
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            pnlTop.Location = New System.Drawing.Point(0, 0)
            pnlTop.Name = "pnlTop"
            pnlTop.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            pnlTop.Size = New System.Drawing.Size(1200, 45)
            pnlTop.TabIndex = 0
            '
            'btnNew
            '
            Me.btnNew.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.btnNew.Dock = System.Windows.Forms.DockStyle.Right
            Me.btnNew.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnNew.Location = New System.Drawing.Point(1090, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(100, 29)
            Me.btnNew.TabIndex = 0
            Me.btnNew.Text = "جدید"
            Me.btnNew.UseVisualStyleBackColor = False
            '
            'dgvReports
            '
            Me.dgvReports.AllowUserToAddRows = False
            Me.dgvReports.AllowUserToDeleteRows = False
            Me.dgvReports.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvReports.BackgroundColor = System.Drawing.Color.White
            Me.dgvReports.RowHeadersVisible = False
            Me.dgvReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvReports.MultiSelect = False
            Me.dgvReports.ReadOnly = True
            Me.dgvReports.RowTemplate.Height = 26
            Me.dgvReports.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvReports.Location = New System.Drawing.Point(0, 45)
            Me.dgvReports.Name = "dgvReports"
            Me.dgvReports.Size = New System.Drawing.Size(1200, 600)
            Me.dgvReports.TabIndex = 1
            Me.dgvReports.EnableHeadersVisualStyles = False
            Me.dgvReports.ColumnHeadersVisible = True
            Me.dgvReports.ColumnHeadersHeight = 30
            Me.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            
            headerCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            headerCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(160, Byte), Integer))
            headerCellStyle.Font = New System.Drawing.Font("Tahoma", 9.0!)
            headerCellStyle.ForeColor = System.Drawing.Color.White
            headerCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight
            headerCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            headerCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True
            Me.dgvReports.ColumnHeadersDefaultCellStyle = headerCellStyle
            
            Me.dgvReports.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colRowNo, colEdit, colDelete, colCode, colName, colPrint, colID})
            '
            'colRowNo
            '
            colRowNo.Name = "colRowNo"
            colRowNo.HeaderText = "ردیف"
            colRowNo.Width = 60
            colRowNo.ReadOnly = True
            colRowNo.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            '
            'colEdit
            '
            colEdit.Name = "colEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Width = 80
            colEdit.ReadOnly = True
            colEdit.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            '
            'colDelete
            '
            colDelete.Name = "colDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Width = 80
            colDelete.ReadOnly = True
            colDelete.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            '
            'colCode
            '
            colCode.Name = "colCode"
            colCode.HeaderText = "کد گزارش"
            colCode.Width = 100
            colCode.ReadOnly = True
            colCode.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            '
            'colName
            '
            colName.Name = "colName"
            colName.HeaderText = "نام گزارش"
            colName.Width = 300
            colName.ReadOnly = True
            '
            'colPrint
            '
            colPrint.Name = "colPrint"
            colPrint.HeaderText = "چاپ"
            colPrint.Width = 80
            colPrint.ReadOnly = True
            colPrint.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            '
            'colID
            '
            colID.Name = "colID"
            colID.Visible = False
            colID.ReadOnly = True
            '
            'HesabdaryReport1Form
            '
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1200, 645)
            Me.Controls.Add(Me.dgvReports)
            Me.Controls.Add(pnlTop)
            Me.Name = "HesabdaryReport1Form"
            
            pnlTop.ResumeLayout(False)
            CType(Me.dgvReports, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents dgvReports As System.Windows.Forms.DataGridView
        Friend WithEvents btnNew As System.Windows.Forms.Button
    End Class
End Namespace
