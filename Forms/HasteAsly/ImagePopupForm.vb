Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    Public Class ImagePopupForm
        Inherits Form

        Public Sub New(img As Image, themeName As String)
            InitializeComponent()
            picMain.Image = img
            Me.Text = "نمایش بزرگ - " & themeName
        End Sub

        Private Sub ImagePopupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
        End Sub
    End Class
End Namespace
