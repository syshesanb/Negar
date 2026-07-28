Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniExpenseLedgerDialog
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
            Me.grpLevel = New System.Windows.Forms.GroupBox()
            Me.rbCategoryLevel = New System.Windows.Forms.RadioButton()
            Me.rbTitleLevel = New System.Windows.Forms.RadioButton()
            Me.lblSelectCategory = New System.Windows.Forms.Label()
            Me.cmbCategory = New System.Windows.Forms.ComboBox()
            Me.lblSelectTitle = New System.Windows.Forms.Label()
            Me.cmbTitle = New System.Windows.Forms.ComboBox()
            Me.grpDate = New System.Windows.Forms.GroupBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.TextBox()
            Me.btnPickFromDate = New System.Windows.Forms.Button()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtToDate = New System.Windows.Forms.TextBox()
            Me.btnPickToDate = New System.Windows.Forms.Button()
            Me.btnPreviewPrint = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.grpLevel.SuspendLayout()
            Me.grpDate.SuspendLayout()
            Me.SuspendLayout()
            '
            'grpLevel
            '
            Me.grpLevel.Controls.Add(Me.cmbTitle)
            Me.grpLevel.Controls.Add(Me.lblSelectTitle)
            Me.grpLevel.Controls.Add(Me.cmbCategory)
            Me.grpLevel.Controls.Add(Me.lblSelectCategory)
            Me.grpLevel.Controls.Add(Me.rbTitleLevel)
            Me.grpLevel.Controls.Add(Me.rbCategoryLevel)
            Me.grpLevel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpLevel.Location = New System.Drawing.Point(15, 15)
            Me.grpLevel.Name = "grpLevel"
            Me.grpLevel.Size = New System.Drawing.Size(460, 195)
            Me.grpLevel.TabIndex = 0
            Me.grpLevel.TabStop = False
            Me.grpLevel.Text = "تعیین سطح گزارش و دفتر هزینه"
            '
            'rbCategoryLevel
            '
            Me.rbCategoryLevel.AutoSize = True
            Me.rbCategoryLevel.Checked = True
            Me.rbCategoryLevel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.rbCategoryLevel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(160, Byte), Integer))
            Me.rbCategoryLevel.Location = New System.Drawing.Point(210, 30)
            Me.rbCategoryLevel.Name = "rbCategoryLevel"
            Me.rbCategoryLevel.Size = New System.Drawing.Size(235, 18)
            Me.rbCategoryLevel.TabIndex = 0
            Me.rbCategoryLevel.TabStop = True
            Me.rbCategoryLevel.Text = "۱. دفتر در سطح سرفصل (دسته‌بندی)"
            Me.rbCategoryLevel.UseVisualStyleBackColor = True
            '
            'lblSelectCategory
            '
            Me.lblSelectCategory.AutoSize = True
            Me.lblSelectCategory.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblSelectCategory.Location = New System.Drawing.Point(340, 60)
            Me.lblSelectCategory.Name = "lblSelectCategory"
            Me.lblSelectCategory.Size = New System.Drawing.Size(104, 14)
            Me.lblSelectCategory.TabIndex = 1
            Me.lblSelectCategory.Text = "انتخاب سرفصل:"
            '
            'cmbCategory
            '
            Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCategory.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbCategory.FormattingEnabled = True
            Me.cmbCategory.Location = New System.Drawing.Point(25, 57)
            Me.cmbCategory.Name = "cmbCategory"
            Me.cmbCategory.Size = New System.Drawing.Size(310, 21)
            Me.cmbCategory.TabIndex = 2
            '
            'rbTitleLevel
            '
            Me.rbTitleLevel.AutoSize = True
            Me.rbTitleLevel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.rbTitleLevel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(160, Byte), Integer))
            Me.rbTitleLevel.Location = New System.Drawing.Point(238, 110)
            Me.rbTitleLevel.Name = "rbTitleLevel"
            Me.rbTitleLevel.Size = New System.Drawing.Size(207, 18)
            Me.rbTitleLevel.TabIndex = 3
            Me.rbTitleLevel.Text = "۲. دفتر در سطح عنوان / شرح هزینه"
            Me.rbTitleLevel.UseVisualStyleBackColor = True
            '
            'lblSelectTitle
            '
            Me.lblSelectTitle.AutoSize = True
            Me.lblSelectTitle.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblSelectTitle.Location = New System.Drawing.Point(340, 140)
            Me.lblSelectTitle.Name = "lblSelectTitle"
            Me.lblSelectTitle.Size = New System.Drawing.Size(107, 14)
            Me.lblSelectTitle.TabIndex = 4
            Me.lblSelectTitle.Text = "انتخاب عنوان هزینه:"
            '
            'cmbTitle
            '
            Me.cmbTitle.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.cmbTitle.FormattingEnabled = True
            Me.cmbTitle.Location = New System.Drawing.Point(25, 137)
            Me.cmbTitle.Name = "cmbTitle"
            Me.cmbTitle.Size = New System.Drawing.Size(310, 21)
            Me.cmbTitle.TabIndex = 5
            '
            'grpDate
            '
            Me.grpDate.Controls.Add(Me.btnPickToDate)
            Me.grpDate.Controls.Add(Me.txtToDate)
            Me.grpDate.Controls.Add(Me.lblToDate)
            Me.grpDate.Controls.Add(Me.btnPickFromDate)
            Me.grpDate.Controls.Add(Me.txtFromDate)
            Me.grpDate.Controls.Add(Me.lblFromDate)
            Me.grpDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpDate.Location = New System.Drawing.Point(15, 220)
            Me.grpDate.Name = "grpDate"
            Me.grpDate.Size = New System.Drawing.Size(460, 75)
            Me.grpDate.TabIndex = 1
            Me.grpDate.TabStop = False
            Me.grpDate.Text = "بازه زمانی دفتر"
            '
            'lblFromDate
            '
            Me.lblFromDate.AutoSize = True
            Me.lblFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblFromDate.Location = New System.Drawing.Point(395, 32)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(52, 14)
            Me.lblFromDate.TabIndex = 0
            Me.lblFromDate.Text = "از تاریخ:"
            '
            'txtFromDate
            '
            Me.txtFromDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtFromDate.Location = New System.Drawing.Point(295, 29)
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(95, 21)
            Me.txtFromDate.TabIndex = 1
            '
            'btnPickFromDate
            '
            Me.btnPickFromDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnPickFromDate.Location = New System.Drawing.Point(265, 28)
            Me.btnPickFromDate.Name = "btnPickFromDate"
            Me.btnPickFromDate.Size = New System.Drawing.Size(28, 23)
            Me.btnPickFromDate.TabIndex = 2
            Me.btnPickFromDate.Text = "..."
            Me.btnPickFromDate.UseVisualStyleBackColor = True
            '
            'lblToDate
            '
            Me.lblToDate.AutoSize = True
            Me.lblToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.lblToDate.Location = New System.Drawing.Point(165, 32)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(51, 14)
            Me.lblToDate.TabIndex = 3
            Me.lblToDate.Text = "تا تاریخ:"
            '
            'txtToDate
            '
            Me.txtToDate.Font = New System.Drawing.Font("Tahoma", 8.5!)
            Me.txtToDate.Location = New System.Drawing.Point(65, 29)
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(95, 21)
            Me.txtToDate.TabIndex = 4
            '
            'btnPickToDate
            '
            Me.btnPickToDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnPickToDate.Location = New System.Drawing.Point(35, 28)
            Me.btnPickToDate.Name = "btnPickToDate"
            Me.btnPickToDate.Size = New System.Drawing.Size(28, 23)
            Me.btnPickToDate.TabIndex = 5
            Me.btnPickToDate.Text = "..."
            Me.btnPickToDate.UseVisualStyleBackColor = True
            '
            'btnPreviewPrint
            '
            Me.btnPreviewPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(120, Byte), Integer))
            Me.btnPreviewPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPreviewPrint.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnPreviewPrint.ForeColor = System.Drawing.Color.White
            Me.btnPreviewPrint.Location = New System.Drawing.Point(220, 310)
            Me.btnPreviewPrint.Name = "btnPreviewPrint"
            Me.btnPreviewPrint.Size = New System.Drawing.Size(200, 38)
            Me.btnPreviewPrint.TabIndex = 2
            Me.btnPreviewPrint.Text = "🖨️ تهیه و چاپ دفتر هزینه"
            Me.btnPreviewPrint.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(70, 310)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(130, 38)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'AnbarMiniExpenseLedgerDialog
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(490, 365)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnPreviewPrint)
            Me.Controls.Add(Me.grpDate)
            Me.Controls.Add(Me.grpLevel)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbarMiniExpenseLedgerDialog"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "تهیه دفتر هزینه (سرفصل / عنوان)"
            Me.grpLevel.ResumeLayout(False)
            Me.grpLevel.PerformLayout()
            Me.grpDate.ResumeLayout(False)
            Me.grpDate.PerformLayout()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents grpLevel As System.Windows.Forms.GroupBox
        Friend WithEvents rbCategoryLevel As System.Windows.Forms.RadioButton
        Friend WithEvents lblSelectCategory As System.Windows.Forms.Label
        Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
        Friend WithEvents rbTitleLevel As System.Windows.Forms.RadioButton
        Friend WithEvents lblSelectTitle As System.Windows.Forms.Label
        Friend WithEvents cmbTitle As System.Windows.Forms.ComboBox
        Friend WithEvents grpDate As System.Windows.Forms.GroupBox
        Friend WithEvents lblFromDate As System.Windows.Forms.Label
        Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
        Friend WithEvents btnPickFromDate As System.Windows.Forms.Button
        Friend WithEvents lblToDate As System.Windows.Forms.Label
        Friend WithEvents txtToDate As System.Windows.Forms.TextBox
        Friend WithEvents btnPickToDate As System.Windows.Forms.Button
        Friend WithEvents btnPreviewPrint As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace
