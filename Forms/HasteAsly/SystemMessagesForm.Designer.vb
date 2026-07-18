Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class SystemMessagesForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents txtAboutText As TextBox
        Friend WithEvents txtContactText As TextBox
        Friend WithEvents btnSave As Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.txtAboutText = New TextBox()
            Me.txtContactText = New TextBox()
            Me.btnSave = New Button()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(620, 360)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "SystemMessagesForm"
            Me.Text = "مدیریت پیام‌های سیستم"

            Dim lblAbout As New Label() With {.Text = "متن «درباره...»:", .AutoSize = True, .Location = New Point(470, 30)}
            Me.txtAboutText.Location = New Point(30, 27)
            Me.txtAboutText.Size = New Size(420, 100)
            Me.txtAboutText.Multiline = True
            Me.txtAboutText.ScrollBars = ScrollBars.Vertical

            Dim lblContact As New Label() With {.Text = "متن «ارتباط با ما»:", .AutoSize = True, .Location = New Point(470, 150)}
            Me.txtContactText.Location = New Point(30, 147)
            Me.txtContactText.Size = New Size(420, 120)
            Me.txtContactText.Multiline = True
            Me.txtContactText.ScrollBars = ScrollBars.Vertical

            Me.btnSave.Text = "ثبت پیام‌ها"
            Me.btnSave.Size = New Size(150, 34)
            Me.btnSave.Location = New Point(235, 295)

            Me.Controls.AddRange(New Control() {
                lblAbout, Me.txtAboutText,
                lblContact, Me.txtContactText,
                Me.btnSave
            })
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
