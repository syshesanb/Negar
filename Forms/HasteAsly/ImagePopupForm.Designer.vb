Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ImagePopupForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents picMain As PictureBox

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.picMain = New PictureBox()
            CType(Me.picMain, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            
            Me.picMain.Dock = DockStyle.Fill
            Me.picMain.Location = New Point(0, 0)
            Me.picMain.Name = "picMain"
            Me.picMain.Size = New Size(800, 600)
            Me.picMain.SizeMode = PictureBoxSizeMode.Zoom
            Me.picMain.TabIndex = 0
            Me.picMain.TabStop = False
            
            Me.AutoScaleDimensions = New SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(800, 600)
            Me.Controls.Add(Me.picMain)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ImagePopupForm"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "نمایش بزرگ تصویر"
            
            CType(Me.picMain, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
