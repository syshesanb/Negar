Imports System
Imports System.Windows.Forms

Namespace Negar.Forms
    Public Class PersonnelManagementForm
        Inherits Form

        Private ctrlPersonnel As Controls.PersonnelManagementControl

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.ctrlPersonnel = New Controls.PersonnelManagementControl()
            Me.SuspendLayout()
            '
            'ctrlPersonnel
            '
            Me.ctrlPersonnel.Dock = DockStyle.Fill
            Me.ctrlPersonnel.Location = New System.Drawing.Point(0, 0)
            Me.ctrlPersonnel.Name = "ctrlPersonnel"
            Me.ctrlPersonnel.Size = New System.Drawing.Size(800, 600)
            Me.ctrlPersonnel.TabIndex = 0
            '
            'PersonnelManagementForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(800, 600)
            Me.Controls.Add(Me.ctrlPersonnel)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "PersonnelManagementForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "مدیریت جامع نیروی انسانی"
            Me.ResumeLayout(False)
        End Sub

        Private Sub PersonnelManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ctrlPersonnel.Init(0) ' 0 means ALL
        End Sub
    End Class
End Namespace


