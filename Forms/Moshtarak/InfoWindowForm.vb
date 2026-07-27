Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms
    Public Class InfoWindowForm
        Public Sub New(titleText As String, infoText As String, parentForm As Form)
            InitializeComponent()
            Me.Text = titleText
            Me.lblContent.Text = infoText
            If parentForm IsNot Nothing Then
                Me.Width = Math.Max(350, parentForm.Width \ 2)
                Me.Height = Math.Max(250, parentForm.Height \ 2)
            Else
                Me.Width = 600
                Me.Height = 350
            End If
        End Sub

        Private Sub InfoWindowForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            If pnlBottom IsNot Nothing AndAlso btnClose IsNot Nothing Then
                btnClose.Location = New Point((pnlBottom.Width - btnClose.Width) \ 2, (pnlBottom.Height - btnClose.Height) \ 2)
            End If
        End Sub

        Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub
    End Class
End Namespace
