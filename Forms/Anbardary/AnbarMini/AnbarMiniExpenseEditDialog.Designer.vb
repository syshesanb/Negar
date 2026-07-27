Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniExpenseEditDialog
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
            Me.lblDate = New System.Windows.Forms.Label()
            Me.txtDate = New System.Windows.Forms.TextBox()
            Me.btnPickDate = New System.Windows.Forms.Button()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.lblCategory = New System.Windows.Forms.Label()
            Me.cmbCategory = New System.Windows.Forms.ComboBox()
            Me.lblAmount = New System.Windows.Forms.Label()
            Me.txtAmount = New System.Windows.Forms.TextBox()
            Me.lblPaidTo = New System.Windows.Forms.Label()
            Me.txtPaidTo = New System.Windows.Forms.TextBox()
            Me.lblPaymentMethod = New System.Windows.Forms.Label()
            Me.cmbPaymentMethod = New System.Windows.Forms.ComboBox()
            Me.lblReferenceNo = New System.Windows.Forms.Label()
            Me.txtReferenceNo = New System.Windows.Forms.TextBox()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lblDate
            '
            Me.lblDate.AutoSize = True
            Me.lblDate.Location = New System.Drawing.Point(380, 20)
            Me.lblDate.Name = "lblDate"
            Me.lblDate.Size = New System.Drawing.Size(68, 14)
            Me.lblDate.TabIndex = 0
            Me.lblDate.Text = "تاریخ ثبت:"
            '
            'txtDate
            '
            Me.txtDate.Location = New System.Drawing.Point(260, 17)
            Me.txtDate.Name = "txtDate"
            Me.txtDate.Size = New System.Drawing.Size(110, 22)
            Me.txtDate.TabIndex = 1
            '
            'btnPickDate
            '
            Me.btnPickDate.Location = New System.Drawing.Point(230, 16)
            Me.btnPickDate.Name = "btnPickDate"
            Me.btnPickDate.Size = New System.Drawing.Size(25, 23)
            Me.btnPickDate.TabIndex = 2
            Me.btnPickDate.Text = "..."
            Me.btnPickDate.UseVisualStyleBackColor = True
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(380, 60)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(81, 14)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Text = "عنوان هزینه: *"
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(25, 57)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(345, 22)
            Me.txtTitle.TabIndex = 4
            '
            'lblCategory
            '
            Me.lblCategory.AutoSize = True
            Me.lblCategory.Location = New System.Drawing.Point(380, 100)
            Me.lblCategory.Name = "lblCategory"
            Me.lblCategory.Size = New System.Drawing.Size(60, 14)
            Me.lblCategory.TabIndex = 5
            Me.lblCategory.Text = "سرفصل:"
            '
            'cmbCategory
            '
            Me.cmbCategory.FormattingEnabled = True
            Me.cmbCategory.Location = New System.Drawing.Point(25, 97)
            Me.cmbCategory.Name = "cmbCategory"
            Me.cmbCategory.Size = New System.Drawing.Size(345, 22)
            Me.cmbCategory.TabIndex = 6
            '
            'lblAmount
            '
            Me.lblAmount.AutoSize = True
            Me.lblAmount.Location = New System.Drawing.Point(380, 140)
            Me.lblAmount.Name = "lblAmount"
            Me.lblAmount.Size = New System.Drawing.Size(85, 14)
            Me.lblAmount.TabIndex = 7
            Me.lblAmount.Text = "مبلغ (ریال): *"
            '
            'txtAmount
            '
            Me.txtAmount.Font = New System.Drawing.Font("Tahoma", 9.5!, System.Drawing.FontStyle.Bold)
            Me.txtAmount.Location = New System.Drawing.Point(25, 137)
            Me.txtAmount.Name = "txtAmount"
            Me.txtAmount.Size = New System.Drawing.Size(345, 23)
            Me.txtAmount.TabIndex = 8
            '
            'lblPaidTo
            '
            Me.lblPaidTo.AutoSize = True
            Me.lblPaidTo.Location = New System.Drawing.Point(380, 180)
            Me.lblPaidTo.Name = "lblPaidTo"
            Me.lblPaidTo.Size = New System.Drawing.Size(91, 14)
            Me.lblPaidTo.TabIndex = 9
            Me.lblPaidTo.Text = "پرداخت شده به:"
            '
            'txtPaidTo
            '
            Me.txtPaidTo.Location = New System.Drawing.Point(25, 177)
            Me.txtPaidTo.Name = "txtPaidTo"
            Me.txtPaidTo.Size = New System.Drawing.Size(345, 22)
            Me.txtPaidTo.TabIndex = 10
            '
            'lblPaymentMethod
            '
            Me.lblPaymentMethod.AutoSize = True
            Me.lblPaymentMethod.Location = New System.Drawing.Point(380, 220)
            Me.lblPaymentMethod.Name = "lblPaymentMethod"
            Me.lblPaymentMethod.Size = New System.Drawing.Size(76, 14)
            Me.lblPaymentMethod.TabIndex = 11
            Me.lblPaymentMethod.Text = "نحوه پرداخت:"
            '
            'cmbPaymentMethod
            '
            Me.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPaymentMethod.FormattingEnabled = True
            Me.cmbPaymentMethod.Location = New System.Drawing.Point(25, 217)
            Me.cmbPaymentMethod.Name = "cmbPaymentMethod"
            Me.cmbPaymentMethod.Size = New System.Drawing.Size(345, 22)
            Me.cmbPaymentMethod.TabIndex = 12
            '
            'lblReferenceNo
            '
            Me.lblReferenceNo.AutoSize = True
            Me.lblReferenceNo.Location = New System.Drawing.Point(380, 260)
            Me.lblReferenceNo.Name = "lblReferenceNo"
            Me.lblReferenceNo.Size = New System.Drawing.Size(96, 14)
            Me.lblReferenceNo.TabIndex = 13
            Me.lblReferenceNo.Text = "شماره پیگیری/فاکتور:"
            '
            'txtReferenceNo
            '
            Me.txtReferenceNo.Location = New System.Drawing.Point(25, 257)
            Me.txtReferenceNo.Name = "txtReferenceNo"
            Me.txtReferenceNo.Size = New System.Drawing.Size(345, 22)
            Me.txtReferenceNo.TabIndex = 14
            '
            'lblDescription
            '
            Me.lblDescription.AutoSize = True
            Me.lblDescription.Location = New System.Drawing.Point(380, 300)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(54, 14)
            Me.lblDescription.TabIndex = 15
            Me.lblDescription.Text = "توضیحات:"
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(25, 297)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(345, 60)
            Me.txtDescription.TabIndex = 16
            '
            'btnSave
            '
            Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSave.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSave.ForeColor = System.Drawing.Color.White
            Me.btnSave.Location = New System.Drawing.Point(195, 375)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(125, 35)
            Me.btnSave.TabIndex = 17
            Me.btnSave.Text = "💾 ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(60, 375)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(125, 35)
            Me.btnCancel.TabIndex = 18
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'AnbarMiniExpenseEditDialog
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(490, 425)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.txtReferenceNo)
            Me.Controls.Add(Me.lblReferenceNo)
            Me.Controls.Add(Me.cmbPaymentMethod)
            Me.Controls.Add(Me.lblPaymentMethod)
            Me.Controls.Add(Me.txtPaidTo)
            Me.Controls.Add(Me.lblPaidTo)
            Me.Controls.Add(Me.txtAmount)
            Me.Controls.Add(Me.lblAmount)
            Me.Controls.Add(Me.cmbCategory)
            Me.Controls.Add(Me.lblCategory)
            Me.Controls.Add(Me.txtTitle)
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.btnPickDate)
            Me.Controls.Add(Me.txtDate)
            Me.Controls.Add(Me.lblDate)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnbarMiniExpenseEditDialog"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ثبت / ویرایش سند هزینه"
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Friend WithEvents lblDate As System.Windows.Forms.Label
        Friend WithEvents txtDate As System.Windows.Forms.TextBox
        Friend WithEvents btnPickDate As System.Windows.Forms.Button
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents txtTitle As System.Windows.Forms.TextBox
        Friend WithEvents lblCategory As System.Windows.Forms.Label
        Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
        Friend WithEvents lblAmount As System.Windows.Forms.Label
        Friend WithEvents txtAmount As System.Windows.Forms.TextBox
        Friend WithEvents lblPaidTo As System.Windows.Forms.Label
        Friend WithEvents txtPaidTo As System.Windows.Forms.TextBox
        Friend WithEvents lblPaymentMethod As System.Windows.Forms.Label
        Friend WithEvents cmbPaymentMethod As System.Windows.Forms.ComboBox
        Friend WithEvents lblReferenceNo As System.Windows.Forms.Label
        Friend WithEvents txtReferenceNo As System.Windows.Forms.TextBox
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents txtDescription As System.Windows.Forms.TextBox
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace
