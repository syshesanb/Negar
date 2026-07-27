Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class SystemMessagesForm
        Inherits Form

        Private ReadOnly service As New SettingsService()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub SystemMessagesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            txtAboutText.Text = service.GetSettingValue("AboutText", "")
            txtContactText.Text = service.GetSettingValue("ContactText", "")
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            service.SaveSetting("AboutText", txtAboutText.Text, "General")
            service.SaveSetting("ContactText", txtContactText.Text, "General")

            MessageBox.Show("پیام‌ها با موفقیت ذخیره شدند.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
