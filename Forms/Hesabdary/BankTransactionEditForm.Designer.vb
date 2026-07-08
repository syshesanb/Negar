Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class BankTransactionEditForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTxDate = New System.Windows.Forms.Label()
            Me.txtTxDate = New System.Windows.Forms.TextBox()
            Me.lblRefNo = New System.Windows.Forms.Label()
            Me.txtRefNo = New System.Windows.Forms.TextBox()
            Me.lblDebit = New System.Windows.Forms.Label()
            Me.txtDebit = New System.Windows.Forms.TextBox()
            Me.lblCredit = New System.Windows.Forms.Label()
            Me.txtCredit = New System.Windows.Forms.TextBox()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.lblPayee = New System.Windows.Forms.Label()
            Me.txtPayee = New System.Windows.Forms.TextBox()
            Me.btnSave = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lblTxDate
            '
            Me.lblTxDate.AutoSize = True
            Me.lblTxDate.Location = New System.Drawing.Point(300, 20)
            Me.lblTxDate.Name = "lblTxDate"
            Me.lblTxDate.Size = New System.Drawing.Size(76, 14)
            Me.lblTxDate.TabIndex = 0
            Me.lblTxDate.Text = "تاریخ تراکنش:"
            '
            'txtTxDate
            '
            Me.txtTxDate.Location = New System.Drawing.Point(30, 17)
            Me.txtTxDate.Name = "txtTxDate"
            Me.txtTxDate.Size = New System.Drawing.Size(250, 22)
            Me.txtTxDate.TabIndex = 1
            '
            'lblRefNo
            '
            Me.lblRefNo.AutoSize = True
            Me.lblRefNo.Location = New System.Drawing.Point(300, 60)
            Me.lblRefNo.Name = "lblRefNo"
            Me.lblRefNo.Size = New System.Drawing.Size(83, 14)
            Me.lblRefNo.TabIndex = 2
            Me.lblRefNo.Text = "شماره پیگیری:"
            '
            'txtRefNo
            '
            Me.txtRefNo.Location = New System.Drawing.Point(30, 57)
            Me.txtRefNo.Name = "txtRefNo"
            Me.txtRefNo.Size = New System.Drawing.Size(250, 22)
            Me.txtRefNo.TabIndex = 3
            '
            'lblDebit
            '
            Me.lblDebit.AutoSize = True
            Me.lblDebit.Location = New System.Drawing.Point(300, 100)
            Me.lblDebit.Name = "lblDebit"
            Me.lblDebit.Size = New System.Drawing.Size(65, 14)
            Me.lblDebit.TabIndex = 4
            Me.lblDebit.Text = "مبلغ واریز:"
            '
            'txtDebit
            '
            Me.txtDebit.Location = New System.Drawing.Point(30, 97)
            Me.txtDebit.Name = "txtDebit"
            Me.txtDebit.Size = New System.Drawing.Size(250, 22)
            Me.txtDebit.TabIndex = 5
            '
            'lblCredit
            '
            Me.lblCredit.AutoSize = True
            Me.lblCredit.Location = New System.Drawing.Point(300, 140)
            Me.lblCredit.Name = "lblCredit"
            Me.lblCredit.Size = New System.Drawing.Size(76, 14)
            Me.lblCredit.TabIndex = 6
            Me.lblCredit.Text = "مبلغ برداشت:"
            '
            'txtCredit
            '
            Me.txtCredit.Location = New System.Drawing.Point(30, 137)
            Me.txtCredit.Name = "txtCredit"
            Me.txtCredit.Size = New System.Drawing.Size(250, 22)
            Me.txtCredit.TabIndex = 7
            '
            'lblDescription
            '
            Me.lblDescription.AutoSize = True
            Me.lblDescription.Location = New System.Drawing.Point(300, 180)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(60, 14)
            Me.lblDescription.TabIndex = 8
            Me.lblDescription.Text = "شرح/بابت:"
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(30, 177)
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(250, 22)
            Me.txtDescription.TabIndex = 9
            '
            'lblPayee
            '
            Me.lblPayee.AutoSize = True
            Me.lblPayee.Location = New System.Drawing.Point(300, 220)
            Me.lblPayee.Name = "lblPayee"
            Me.lblPayee.Size = New System.Drawing.Size(89, 14)
            Me.lblPayee.TabIndex = 10
            Me.lblPayee.Text = "واریزکننده/ذینفع:"
            '
            'txtPayee
            '
            Me.txtPayee.Location = New System.Drawing.Point(30, 217)
            Me.txtPayee.Name = "txtPayee"
            Me.txtPayee.Size = New System.Drawing.Size(250, 22)
            Me.txtPayee.TabIndex = 11
            '
            'btnSave
            '
            Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(174, Byte), Integer), CType(CType(96, Byte), Integer))
            Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSave.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnSave.ForeColor = System.Drawing.Color.White
            Me.btnSave.Location = New System.Drawing.Point(165, 270)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(115, 35)
            Me.btnSave.TabIndex = 12
            Me.btnSave.Text = "ذخیره"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(43, Byte), Integer))
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(30, 270)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(115, 35)
            Me.btnCancel.TabIndex = 13
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'BankTransactionEditForm
            '
            Me.AcceptButton = Me.btnSave
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(400, 330)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.txtPayee)
            Me.Controls.Add(Me.lblPayee)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.txtCredit)
            Me.Controls.Add(Me.lblCredit)
            Me.Controls.Add(Me.txtDebit)
            Me.Controls.Add(Me.lblDebit)
            Me.Controls.Add(Me.txtRefNo)
            Me.Controls.Add(Me.lblRefNo)
            Me.Controls.Add(Me.txtTxDate)
            Me.Controls.Add(Me.lblTxDate)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "BankTransactionEditForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ویرایش تراکنش صورت‌حساب"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents lblTxDate As System.Windows.Forms.Label
        Private WithEvents txtTxDate As System.Windows.Forms.TextBox
        Private WithEvents lblRefNo As System.Windows.Forms.Label
        Private WithEvents txtRefNo As System.Windows.Forms.TextBox
        Private WithEvents lblDebit As System.Windows.Forms.Label
        Private WithEvents txtDebit As System.Windows.Forms.TextBox
        Private WithEvents lblCredit As System.Windows.Forms.Label
        Private WithEvents txtCredit As System.Windows.Forms.TextBox
        Private WithEvents lblDescription As System.Windows.Forms.Label
        Private WithEvents txtDescription As System.Windows.Forms.TextBox
        Private WithEvents lblPayee As System.Windows.Forms.Label
        Private WithEvents txtPayee As System.Windows.Forms.TextBox
        Private WithEvents btnSave As System.Windows.Forms.Button
        Private WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace
