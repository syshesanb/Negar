Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class ReportCenterForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents btnTrial As Button
        Friend WithEvents btnInv As Button
        Friend WithEvents btnUsers As Button
        Friend WithEvents btnProducts As Button
        Friend WithEvents btnActivityLog As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.btnTrial = New Button()
            Me.btnInv = New Button()
            Me.btnUsers = New Button()
            Me.btnProducts = New Button()
            Me.btnActivityLog = New Button()
            Me.SuspendLayout()
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(640, 420)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "مرکز گزارشات"

            Dim layout As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .Padding = New Padding(20)}
            Me.btnTrial.Text = "تراز آزمایشی" : Me.btnTrial.Width = 160 : Me.btnTrial.Height = 45
            Me.btnInv.Text = "موجودی انبار" : Me.btnInv.Width = 160 : Me.btnInv.Height = 45
            Me.btnUsers.Text = "کاربران" : Me.btnUsers.Width = 160 : Me.btnUsers.Height = 45
            Me.btnProducts.Text = "کالاها" : Me.btnProducts.Width = 160 : Me.btnProducts.Height = 45
            Me.btnActivityLog.Text = "لاگ فعالیت‌ها" : Me.btnActivityLog.Width = 160 : Me.btnActivityLog.Height = 45
            layout.Controls.AddRange(New Control() {Me.btnTrial, Me.btnInv, Me.btnUsers, Me.btnProducts, Me.btnActivityLog})
            Me.Controls.Add(layout)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
