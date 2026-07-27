Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class PrintRangeDialog
        Inherits Form

        Private components As IContainer

        Friend WithEvents rdoByRef As RadioButton
        Friend WithEvents rdoByDate As RadioButton
        Friend WithEvents grpByRef As GroupBox
        Friend WithEvents lblFromRef As Label
        Friend WithEvents txtFromRef As TextBox
        Friend WithEvents lblToRef As Label
        Friend WithEvents txtToRef As TextBox
        Friend WithEvents grpByDate As GroupBox
        Friend WithEvents lblFromDate As Label
        Friend WithEvents txtFromDate As TextBox
        Friend WithEvents btnCalFromDate As Button
        Friend WithEvents lblToDate As Label
        Friend WithEvents txtToDate As TextBox
        Friend WithEvents btnCalToDate As Button
        Friend WithEvents btnConfirm As Button
        Friend WithEvents btnCancel As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.rdoByRef = New System.Windows.Forms.RadioButton()
            Me.rdoByDate = New System.Windows.Forms.RadioButton()
            Me.grpByRef = New System.Windows.Forms.GroupBox()
            Me.lblFromRef = New System.Windows.Forms.Label()
            Me.txtFromRef = New System.Windows.Forms.TextBox()
            Me.lblToRef = New System.Windows.Forms.Label()
            Me.txtToRef = New System.Windows.Forms.TextBox()
            Me.grpByDate = New System.Windows.Forms.GroupBox()
            Me.lblFromDate = New System.Windows.Forms.Label()
            Me.txtFromDate = New System.Windows.Forms.TextBox()
            Me.btnCalFromDate = New System.Windows.Forms.Button()
            Me.lblToDate = New System.Windows.Forms.Label()
            Me.txtToDate = New System.Windows.Forms.TextBox()
            Me.btnCalToDate = New System.Windows.Forms.Button()
            Me.btnConfirm = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.grpByRef.SuspendLayout()
            Me.grpByDate.SuspendLayout()
            Me.SuspendLayout()
            '
            'rdoByRef
            '
            Me.rdoByRef.Checked = True
            Me.rdoByRef.Location = New System.Drawing.Point(20, 15)
            Me.rdoByRef.Name = "rdoByRef"
            Me.rdoByRef.Size = New System.Drawing.Size(150, 20)
            Me.rdoByRef.TabIndex = 0
            Me.rdoByRef.TabStop = True
            Me.rdoByRef.Text = "بر اساس شماره سند"
            Me.rdoByRef.UseVisualStyleBackColor = True
            '
            'rdoByDate
            '
            Me.rdoByDate.Location = New System.Drawing.Point(200, 15)
            Me.rdoByDate.Name = "rdoByDate"
            Me.rdoByDate.Size = New System.Drawing.Size(150, 20)
            Me.rdoByDate.TabIndex = 1
            Me.rdoByDate.Text = "بر اساس تاریخ سند"
            Me.rdoByDate.UseVisualStyleBackColor = True
            '
            'grpByRef
            '
            Me.grpByRef.Controls.Add(Me.lblFromRef)
            Me.grpByRef.Controls.Add(Me.txtFromRef)
            Me.grpByRef.Controls.Add(Me.lblToRef)
            Me.grpByRef.Controls.Add(Me.txtToRef)
            Me.grpByRef.Location = New System.Drawing.Point(12, 45)
            Me.grpByRef.Name = "grpByRef"
            Me.grpByRef.Size = New System.Drawing.Size(356, 65)
            Me.grpByRef.TabIndex = 2
            Me.grpByRef.TabStop = False
            Me.grpByRef.Text = "محدوده شماره سند"
            '
            'lblFromRef
            '
            Me.lblFromRef.Location = New System.Drawing.Point(10, 28)
            Me.lblFromRef.Name = "lblFromRef"
            Me.lblFromRef.Size = New System.Drawing.Size(60, 20)
            Me.lblFromRef.TabIndex = 0
            Me.lblFromRef.Text = "از شماره:"
            Me.lblFromRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtFromRef
            '
            Me.txtFromRef.Location = New System.Drawing.Point(75, 25)
            Me.txtFromRef.Name = "txtFromRef"
            Me.txtFromRef.Size = New System.Drawing.Size(90, 22)
            Me.txtFromRef.TabIndex = 0
            Me.txtFromRef.TabStop = True
            '
            'lblToRef
            '
            Me.lblToRef.Location = New System.Drawing.Point(185, 28)
            Me.lblToRef.Name = "lblToRef"
            Me.lblToRef.Size = New System.Drawing.Size(60, 20)
            Me.lblToRef.TabIndex = 2
            Me.lblToRef.Text = "تا شماره:"
            Me.lblToRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtToRef
            '
            Me.txtToRef.Location = New System.Drawing.Point(250, 25)
            Me.txtToRef.Name = "txtToRef"
            Me.txtToRef.Size = New System.Drawing.Size(90, 22)
            Me.txtToRef.TabIndex = 1
            Me.txtToRef.TabStop = True
            '
            'grpByDate
            '
            Me.grpByDate.Controls.Add(Me.lblFromDate)
            Me.grpByDate.Controls.Add(Me.txtFromDate)
            Me.grpByDate.Controls.Add(Me.btnCalFromDate)
            Me.grpByDate.Controls.Add(Me.lblToDate)
            Me.grpByDate.Controls.Add(Me.txtToDate)
            Me.grpByDate.Controls.Add(Me.btnCalToDate)
            Me.grpByDate.Location = New System.Drawing.Point(12, 120)
            Me.grpByDate.Name = "grpByDate"
            Me.grpByDate.Size = New System.Drawing.Size(356, 65)
            Me.grpByDate.TabIndex = 3
            Me.grpByDate.TabStop = False
            Me.grpByDate.Text = "محدوده تاریخ"
            '
            'lblFromDate
            '
            Me.lblFromDate.Location = New System.Drawing.Point(10, 28)
            Me.lblFromDate.Name = "lblFromDate"
            Me.lblFromDate.Size = New System.Drawing.Size(55, 20)
            Me.lblFromDate.TabIndex = 0
            Me.lblFromDate.Text = "از تاریخ:"
            Me.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtFromDate
            '
            Me.txtFromDate.Location = New System.Drawing.Point(70, 25)
            Me.txtFromDate.Name = "txtFromDate"
            Me.txtFromDate.Size = New System.Drawing.Size(75, 22)
            Me.txtFromDate.TabIndex = 0
            Me.txtFromDate.TabStop = True
            '
            'btnCalFromDate
            '
            Me.btnCalFromDate.Location = New System.Drawing.Point(147, 24)
            Me.btnCalFromDate.Name = "btnCalFromDate"
            Me.btnCalFromDate.Size = New System.Drawing.Size(24, 24)
            Me.btnCalFromDate.TabIndex = 1
            Me.btnCalFromDate.Text = "..."
            Me.btnCalFromDate.UseVisualStyleBackColor = True
            '
            'lblToDate
            '
            Me.lblToDate.Location = New System.Drawing.Point(185, 28)
            Me.lblToDate.Name = "lblToDate"
            Me.lblToDate.Size = New System.Drawing.Size(50, 20)
            Me.lblToDate.TabIndex = 3
            Me.lblToDate.Text = "تا تاریخ:"
            Me.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtToDate
            '
            Me.txtToDate.Location = New System.Drawing.Point(240, 25)
            Me.txtToDate.Name = "txtToDate"
            Me.txtToDate.Size = New System.Drawing.Size(75, 22)
            Me.txtToDate.TabIndex = 2
            Me.txtToDate.TabStop = True
            '
            'btnCalToDate
            '
            Me.btnCalToDate.Location = New System.Drawing.Point(317, 24)
            Me.btnCalToDate.Name = "btnCalToDate"
            Me.btnCalToDate.Size = New System.Drawing.Size(24, 24)
            Me.btnCalToDate.TabIndex = 3
            Me.btnCalToDate.Text = "..."
            Me.btnCalToDate.UseVisualStyleBackColor = True
            '
            'btnConfirm
            '
            Me.btnConfirm.Location = New System.Drawing.Point(204, 195)
            Me.btnConfirm.Name = "btnConfirm"
            Me.btnConfirm.Size = New System.Drawing.Size(80, 30)
            Me.btnConfirm.TabIndex = 4
            Me.btnConfirm.Text = "تایید"
            Me.btnConfirm.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(96, 195)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(80, 30)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'PrintRangeDialog
            '
            Me.AcceptButton = Me.btnConfirm
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(380, 240)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnConfirm)
            Me.Controls.Add(Me.grpByDate)
            Me.Controls.Add(Me.grpByRef)
            Me.Controls.Add(Me.rdoByDate)
            Me.Controls.Add(Me.rdoByRef)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "PrintRangeDialog"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "چاپ گروهی اسناد حسابداری"
            Me.grpByRef.ResumeLayout(False)
            Me.grpByRef.PerformLayout()
            Me.grpByDate.ResumeLayout(False)
            Me.grpByDate.PerformLayout()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
