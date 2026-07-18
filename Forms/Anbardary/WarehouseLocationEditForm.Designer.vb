Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class WarehouseLocationEditForm
        Inherits System.Windows.Forms.Form

        Private components As System.ComponentModel.IContainer

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

        Friend WithEvents lblTitle As Label
        Friend WithEvents txtTitle As TextBox
        Friend WithEvents lblCode As Label
        Friend WithEvents txtCode As TextBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnCancel As Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblTitle = New Label()
            Me.txtTitle = New TextBox()
            Me.lblCode = New Label()
            Me.txtCode = New TextBox()
            Me.btnSave = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()
            '
            'lblTitle
            '
            Me.lblTitle.Location = New Point(320, 23)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(100, 20)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "عنوان:"
            Me.lblTitle.TextAlign = ContentAlignment.MiddleRight
            '
            'txtTitle
            '
            Me.txtTitle.Location = New Point(20, 23)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 22)
            Me.txtTitle.TabIndex = 1
            '
            'lblCode
            '
            Me.lblCode.Location = New Point(320, 63)
            Me.lblCode.Name = "lblCode"
            Me.lblCode.Size = New Size(100, 20)
            Me.lblCode.TabIndex = 2
            Me.lblCode.Text = "کد اختصاری:"
            Me.lblCode.TextAlign = ContentAlignment.MiddleRight
            '
            'txtCode
            '
            Me.txtCode.Location = New Point(20, 63)
            Me.txtCode.Name = "txtCode"
            Me.txtCode.Size = New Size(300, 22)
            Me.txtCode.TabIndex = 3
            '
            'btnSave
            '
            Me.btnSave.Location = New Point(120, 110)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(90, 30)
            Me.btnSave.TabIndex = 4
            Me.btnSave.Text = "ثبت"
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New Point(20, 110)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(90, 30)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'WarehouseLocationEditForm
            '
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(440, 160)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.txtCode)
            Me.Controls.Add(Me.lblCode)
            Me.Controls.Add(Me.txtTitle)
            Me.Controls.Add(Me.lblTitle)
            Me.Font = New Font("Tahoma", 9.0!, FontStyle.Regular, GraphicsUnit.Point)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "WarehouseLocationEditForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "ویرایش ساختار"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
    End Class
End Namespace
