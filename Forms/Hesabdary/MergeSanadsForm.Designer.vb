Option Strict Off
Option Explicit On

Namespace Sys_Hes_Anb.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class MergeSanadsForm
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
            Me.lblMonth = New System.Windows.Forms.Label()
            Me.cmbMonth = New System.Windows.Forms.ComboBox()
            Me.lblEntries = New System.Windows.Forms.Label()
            Me.lstEntries = New System.Windows.Forms.ListBox()
            Me.btnMerge = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lblMonth
            '
            Me.lblMonth.AutoSize = True
            Me.lblMonth.Location = New System.Drawing.Point(15, 15)
            Me.lblMonth.Name = "lblMonth"
            Me.lblMonth.Size = New System.Drawing.Size(225, 14)
            Me.lblMonth.TabIndex = 0
            Me.lblMonth.Text = "اسناد در چه ماهی را می‌خواهید ادغام کنید؟"
            '
            'cmbMonth
            '
            Me.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbMonth.FormattingEnabled = True
            Me.cmbMonth.Location = New System.Drawing.Point(15, 38)
            Me.cmbMonth.Name = "cmbMonth"
            Me.cmbMonth.Size = New System.Drawing.Size(400, 22)
            Me.cmbMonth.TabIndex = 1
            '
            'lblEntries
            '
            Me.lblEntries.AutoSize = True
            Me.lblEntries.Location = New System.Drawing.Point(15, 80)
            Me.lblEntries.Name = "lblEntries"
            Me.lblEntries.Size = New System.Drawing.Size(277, 14)
            Me.lblEntries.TabIndex = 2
            Me.lblEntries.Text = "انتخاب اسناد جهت ادغام (حداقل دو سند انتخاب کنید):"
            '
            'lstEntries
            '
            Me.lstEntries.FormattingEnabled = True
            Me.lstEntries.ItemHeight = 14
            Me.lstEntries.Location = New System.Drawing.Point(15, 103)
            Me.lstEntries.Name = "lstEntries"
            Me.lstEntries.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
            Me.lstEntries.Size = New System.Drawing.Size(400, 256)
            Me.lstEntries.TabIndex = 3
            '
            'btnMerge
            '
            Me.btnMerge.Location = New System.Drawing.Point(210, 390)
            Me.btnMerge.Name = "btnMerge"
            Me.btnMerge.Size = New System.Drawing.Size(100, 30)
            Me.btnMerge.TabIndex = 4
            Me.btnMerge.Text = "ادغام اسناد"
            Me.btnMerge.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(315, 390)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(100, 30)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'MergeSanadsForm
            '
            Me.AcceptButton = Me.btnMerge
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(434, 441)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnMerge)
            Me.Controls.Add(Me.lstEntries)
            Me.Controls.Add(Me.lblEntries)
            Me.Controls.Add(Me.cmbMonth)
            Me.Controls.Add(Me.lblMonth)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "MergeSanadsForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ادغام اسناد حسابداری"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents lblMonth As System.Windows.Forms.Label
        Friend WithEvents cmbMonth As System.Windows.Forms.ComboBox
        Friend WithEvents lblEntries As System.Windows.Forms.Label
        Friend WithEvents lstEntries As System.Windows.Forms.ListBox
        Friend WithEvents btnMerge As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace
