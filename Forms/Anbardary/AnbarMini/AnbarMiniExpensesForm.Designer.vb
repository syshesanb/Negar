Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniExpensesForm
        Inherits System.Windows.Forms.Form

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
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.pnlFooter = New System.Windows.Forms.Panel()
            Me.lblCount = New System.Windows.Forms.Label()
            Me.lblGrandTotal = New System.Windows.Forms.Label()
            Me.dgvExpenses = New System.Windows.Forms.DataGridView()
            Me.lblSearch = New System.Windows.Forms.Label()
            Me.txtSearch = New System.Windows.Forms.TextBox()
            Me.lblCategory = New System.Windows.Forms.Label()
            Me.cmbCategory = New System.Windows.Forms.ComboBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.TextBox()
            Me.btnPickFromDate = New System.Windows.Forms.Button()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtToDate = New System.Windows.Forms.TextBox()
            Me.btnPickToDate = New System.Windows.Forms.Button()
            Me.btnFilter = New System.Windows.Forms.Button()
            Me.btnClearFilter = New System.Windows.Forms.Button()
            Me.btnAdd = New System.Windows.Forms.Button()
            Me.btnEdit = New System.Windows.Forms.Button()
            Me.btnDelete = New System.Windows.Forms.Button()
            Me.btnPrint = New System.Windows.Forms.Button()
            Me.pnlTop.SuspendLayout()
            Me.pnlFooter.SuspendLayout()
            CType(Me.dgvExpenses, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.btnPrint)
            Me.pnlTop.Controls.Add(Me.btnDelete)
            Me.pnlTop.Controls.Add(Me.btnEdit)
            Me.pnlTop.Controls.Add(Me.btnAdd)
            Me.pnlTop.Controls.Add(Me.btnClearFilter)
            Me.pnlTop.Controls.Add(Me.btnFilter)
            Me.pnlTop.Controls.Add(Me.btnPickToDate)
            Me.pnlTop.Controls.Add(Me.txtToDate)
            Me.pnlTop.Controls.Add(Me.lblToDate)
            Me.pnlTop.Controls.Add(Me.btnPickFromDate)
            Me.pnlTop.Controls.Add(Me.txtFromDate)
            Me.pnlTop.Controls.Add(Me.lblFromDate)
            Me.pnlTop.Controls.Add(Me.cmbCategory)
            Me.pnlTop.Controls.Add(Me.lblCategory)
            Me.pnlTop.Controls.Add(Me.txtSearch)
            Me.pnlTop.Controls.Add(Me.lblSearch)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(980, 50)
            Me.pnlTop.TabIndex = 0
            '
            'lblSearch
            '
            Me.lblSearch.AutoSize = True
            Me.lblSearch.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblSearch.Location = New System.Drawing.Point(920, 17)
            Me.lblSearch.Name = "lblSearch"
            Me.lblSearch.Size = New System.Drawing.Size(48, 14)
            Me.lblSearch.TabIndex = 0
            Me.lblSearch.Text = "جستجو:"
            '
            'txtSearch
            '
            Me.txtSearch.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtSearch.Location = New System.Drawing.Point(810, 14)
            Me.txtSearch.Name = "txtSearch"
            Me.txtSearch.Size = New System.Drawing.Size(105, 21)
            Me.txtSearch.TabIndex = 1
            '
            'lblCategory
            '
            Me.lblCategory.AutoSize = True
            Me.lblCategory.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblCategory.Location = New System.Drawing.Point(755, 17)
            Me.lblCategory.Name = "lblCategory"
            Me.lblCategory.Size = New System.Drawing.Size(50, 14)
            Me.lblCategory.TabIndex = 2
            Me.lblCategory.Text = "سرفصل:"
            '
            'cmbCategory
            '
            Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCategory.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbCategory.FormattingEnabled = True
            Me.cmbCategory.Location = New System.Drawing.Point(645, 14)
            Me.cmbCategory.Name = "cmbCategory"
            Me.cmbCategory.Size = New System.Drawing.Size(105, 21)
            Me.cmbCategory.TabIndex = 3
            '
            'lblFromDate
            '
            Me.lblFromDate.AutoSize = True
            Me.lblFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblFromDate.Location = New System.Drawing.Point(595, 17)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(42, 14)
            Me.lblFromDate.TabIndex = 4
            Me.lblFromDate.Text = "از تاریخ:"
            '
            'txtFromDate
            '
            Me.txtFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtFromDate.Location = New System.Drawing.Point(523, 14)
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(68, 21)
            Me.txtFromDate.TabIndex = 5
            '
            'btnPickFromDate
            '
            Me.btnPickFromDate.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.btnPickFromDate.Location = New System.Drawing.Point(500, 14)
            Me.btnPickFromDate.Name = "btnPickFromDate"
            Me.btnPickFromDate.Size = New System.Drawing.Size(21, 21)
            Me.btnPickFromDate.TabIndex = 6
            Me.btnPickFromDate.Text = "..."
            Me.btnPickFromDate.UseVisualStyleBackColor = True
            '
            'lblToDate
            '
            Me.lblToDate.AutoSize = True
            Me.lblToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblToDate.Location = New System.Drawing.Point(450, 17)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(41, 14)
            Me.lblToDate.TabIndex = 7
            Me.lblToDate.Text = "تا تاریخ:"
            '
            'txtToDate
            '
            Me.txtToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtToDate.Location = New System.Drawing.Point(378, 14)
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(68, 21)
            Me.txtToDate.TabIndex = 8
            '
            'btnPickToDate
            '
            Me.btnPickToDate.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.btnPickToDate.Location = New System.Drawing.Point(355, 14)
            Me.btnPickToDate.Name = "btnPickToDate"
            Me.btnPickToDate.Size = New System.Drawing.Size(21, 21)
            Me.btnPickToDate.TabIndex = 9
            Me.btnPickToDate.Text = "..."
            Me.btnPickToDate.UseVisualStyleBackColor = True
            '
            'btnFilter
            '
            Me.btnFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
            Me.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnFilter.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnFilter.ForeColor = System.Drawing.Color.White
            Me.btnFilter.Location = New System.Drawing.Point(285, 11)
            Me.btnFilter.Name = "btnFilter"
            Me.btnFilter.Size = New System.Drawing.Size(65, 27)
            Me.btnFilter.TabIndex = 10
            Me.btnFilter.Text = "جستجو"
            Me.btnFilter.UseVisualStyleBackColor = False
            '
            'btnClearFilter
            '
            Me.btnClearFilter.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.btnClearFilter.Location = New System.Drawing.Point(235, 11)
            Me.btnClearFilter.Name = "btnClearFilter"
            Me.btnClearFilter.Size = New System.Drawing.Size(45, 27)
            Me.btnClearFilter.TabIndex = 11
            Me.btnClearFilter.Text = "همه"
            Me.btnClearFilter.UseVisualStyleBackColor = True
            '
            'btnAdd
            '
            Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnAdd.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnAdd.ForeColor = System.Drawing.Color.White
            Me.btnAdd.Location = New System.Drawing.Point(180, 11)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(50, 27)
            Me.btnAdd.TabIndex = 12
            Me.btnAdd.Text = "+ هزینه"
            Me.btnAdd.UseVisualStyleBackColor = False
            '
            'btnEdit
            '
            Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnEdit.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnEdit.ForeColor = System.Drawing.Color.White
            Me.btnEdit.Location = New System.Drawing.Point(120, 11)
            Me.btnEdit.Name = "btnEdit"
            Me.btnEdit.Size = New System.Drawing.Size(55, 27)
            Me.btnEdit.TabIndex = 13
            Me.btnEdit.Text = "ویرایش"
            Me.btnEdit.UseVisualStyleBackColor = False
            '
            'btnDelete
            '
            Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
            Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDelete.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnDelete.ForeColor = System.Drawing.Color.White
            Me.btnDelete.Location = New System.Drawing.Point(65, 11)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(50, 27)
            Me.btnDelete.TabIndex = 14
            Me.btnDelete.Text = "حذف"
            Me.btnDelete.UseVisualStyleBackColor = False
            '
            'btnPrint
            '
            Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(145, Byte), Integer))
            Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPrint.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
            Me.btnPrint.ForeColor = System.Drawing.Color.White
            Me.btnPrint.Location = New System.Drawing.Point(10, 11)
            Me.btnPrint.Name = "btnPrint"
            Me.btnPrint.Size = New System.Drawing.Size(50, 27)
            Me.btnPrint.TabIndex = 15
            Me.btnPrint.Text = "چاپ"
            Me.btnPrint.UseVisualStyleBackColor = False
            '
            'dgvExpenses
            '
            Me.dgvExpenses.AllowUserToAddRows = False
            Me.dgvExpenses.AllowUserToDeleteRows = False
            Me.dgvExpenses.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvExpenses.Location = New System.Drawing.Point(0, 50)
            Me.dgvExpenses.MultiSelect = False
            Me.dgvExpenses.Name = "dgvExpenses"
            Me.dgvExpenses.ReadOnly = True
            Me.dgvExpenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvExpenses.Size = New System.Drawing.Size(980, 500)
            Me.dgvExpenses.TabIndex = 1
            '
            'pnlFooter
            '
            Me.pnlFooter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(235, Byte), Integer))
            Me.pnlFooter.Controls.Add(Me.lblGrandTotal)
            Me.pnlFooter.Controls.Add(Me.lblCount)
            Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlFooter.Location = New System.Drawing.Point(0, 550)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New System.Drawing.Size(980, 35)
            Me.pnlFooter.TabIndex = 2
            '
            'lblCount
            '
            Me.lblCount.AutoSize = True
            Me.lblCount.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer))
            Me.lblCount.Location = New System.Drawing.Point(15, 10)
            Me.lblCount.Name = "lblCount"
            Me.lblCount.Size = New System.Drawing.Size(120, 14)
            Me.lblCount.TabIndex = 0
            Me.lblCount.Text = "تعداد اسناد هزینه: 0"
            '
            'lblGrandTotal
            '
            Me.lblGrandTotal.AutoSize = True
            Me.lblGrandTotal.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblGrandTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.lblGrandTotal.Location = New System.Drawing.Point(600, 9)
            Me.lblGrandTotal.Name = "lblGrandTotal"
            Me.lblGrandTotal.Size = New System.Drawing.Size(220, 16)
            Me.lblGrandTotal.TabIndex = 1
            Me.lblGrandTotal.Text = "جمع کل هزینه‌ها: ۰ ریال"
            '
            'AnbarMiniExpensesForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(980, 585)
            Me.Controls.Add(Me.dgvExpenses)
            Me.Controls.Add(Me.pnlFooter)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.Name = "AnbarMiniExpensesForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "مدیریت و ثبت هزینه‌ها"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            Me.pnlFooter.ResumeLayout(False)
            Me.pnlFooter.PerformLayout()
            CType(Me.dgvExpenses, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents pnlTop As System.Windows.Forms.Panel
        Friend WithEvents lblSearch As System.Windows.Forms.Label
        Friend WithEvents txtSearch As System.Windows.Forms.TextBox
        Friend WithEvents lblCategory As System.Windows.Forms.Label
        Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
        Friend WithEvents lblFromDate As System.Windows.Forms.Label
        Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
        Friend WithEvents btnPickFromDate As System.Windows.Forms.Button
        Friend WithEvents lblToDate As System.Windows.Forms.Label
        Friend WithEvents txtToDate As System.Windows.Forms.TextBox
        Friend WithEvents btnPickToDate As System.Windows.Forms.Button
        Friend WithEvents btnFilter As System.Windows.Forms.Button
        Friend WithEvents btnClearFilter As System.Windows.Forms.Button
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnEdit As System.Windows.Forms.Button
        Friend WithEvents btnDelete As System.Windows.Forms.Button
        Friend WithEvents btnPrint As System.Windows.Forms.Button
        Friend WithEvents dgvExpenses As System.Windows.Forms.DataGridView
        Friend WithEvents pnlFooter As System.Windows.Forms.Panel
        Friend WithEvents lblCount As System.Windows.Forms.Label
        Friend WithEvents lblGrandTotal As System.Windows.Forms.Label
    End Class
End Namespace
