Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class InfoWindowForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents lblContent As Label
        Friend WithEvents btnClose As Button
        Friend WithEvents pnlBottom As Panel

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblContent = New Label()
            Me.btnClose = New Button()
            Me.pnlBottom = New Panel()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()

            ' Form settings
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.Font = New Font("Tahoma", 9.5!)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "InfoWindowForm"
            Me.Padding = New Padding(15)

            ' pnlBottom
            Me.pnlBottom.Dock = DockStyle.Bottom
            Me.pnlBottom.Height = 45
            Me.pnlBottom.Controls.Add(Me.btnClose)

            ' btnClose
            Me.btnClose.Size = New Size(110, 32)
            Me.btnClose.Text = "بستن"
            Me.btnClose.UseVisualStyleBackColor = True

            ' lblContent
            Me.lblContent.Dock = DockStyle.Fill
            Me.lblContent.Font = New Font("Tahoma", 10.5!, FontStyle.Bold)
            Me.lblContent.ForeColor = Color.FromArgb(30, 40, 55)
            Me.lblContent.TextAlign = ContentAlignment.MiddleCenter

            ' Add controls
            Me.Controls.Add(Me.lblContent)
            Me.Controls.Add(Me.pnlBottom)

            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
