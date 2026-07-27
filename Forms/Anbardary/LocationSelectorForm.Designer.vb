Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class LocationSelectorForm
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

        Friend WithEvents lblWarehouse As Label
        Friend WithEvents cmbWarehouses As ComboBox
        Friend WithEvents tvLayout As TreeView
        Friend WithEvents btnSelect As Button
        Friend WithEvents btnCancel As Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblWarehouse = New Label()
            Me.cmbWarehouses = New ComboBox()
            Me.tvLayout = New TreeView()
            Me.btnSelect = New Button()
            Me.btnCancel = New Button()
            Me.SuspendLayout()
            '
            'lblWarehouse
            '
            Me.lblWarehouse.Location = New Point(320, 20)
            Me.lblWarehouse.Name = "lblWarehouse"
            Me.lblWarehouse.Size = New Size(100, 20)
            Me.lblWarehouse.TabIndex = 0
            Me.lblWarehouse.Text = "انتخاب انبار:"
            Me.lblWarehouse.TextAlign = ContentAlignment.MiddleRight
            '
            'cmbWarehouses
            '
            Me.cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbWarehouses.Location = New Point(20, 20)
            Me.cmbWarehouses.Name = "cmbWarehouses"
            Me.cmbWarehouses.Size = New Size(300, 22)
            Me.cmbWarehouses.TabIndex = 1
            '
            'tvLayout
            '
            Me.tvLayout.Location = New Point(20, 60)
            Me.tvLayout.Name = "tvLayout"
            Me.tvLayout.RightToLeftLayout = True
            Me.tvLayout.Size = New Size(400, 250)
            Me.tvLayout.TabIndex = 2
            '
            'btnSelect
            '
            Me.btnSelect.Location = New Point(120, 320)
            Me.btnSelect.Name = "btnSelect"
            Me.btnSelect.Size = New Size(90, 30)
            Me.btnSelect.TabIndex = 3
            Me.btnSelect.Text = "انتخاب"
            Me.btnSelect.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New Point(20, 320)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(90, 30)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'LocationSelectorForm
            '
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(440, 370)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSelect)
            Me.Controls.Add(Me.tvLayout)
            Me.Controls.Add(Me.cmbWarehouses)
            Me.Controls.Add(Me.lblWarehouse)
            Me.Font = New Font("Tahoma", 9.0!, FontStyle.Regular, GraphicsUnit.Point)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "LocationSelectorForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "انتخاب جانمایی کالا"
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
