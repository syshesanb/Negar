Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class SettingsForm
        Inherits Form

        Private ReadOnly service As New SettingsService()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            cmbTheme.Text = service.GetSettingValue("Theme", "Light")
            cmbNumberFormat.Text = service.GetSettingValue("NumberFormat", "N2")
            txtCurrencySymbol.Text = service.GetSettingValue("CurrencySymbol", "ریال")

            txtAboutText.Text = service.GetSettingValue("AboutText", SettingsService.DefaultAboutText)
            txtContactText.Text = service.GetSettingValue("ContactText", SettingsService.DefaultContactText)

            ' فقط ابر مدیر امکان تغییر متن‌های اطلاع‌رسانی را دارد
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            txtAboutText.ReadOnly = Not isSuperAdmin
            txtContactText.ReadOnly = Not isSuperAdmin
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            service.SaveSetting("Theme", cmbTheme.Text, "UI")
            service.SaveSetting("NumberFormat", cmbNumberFormat.Text, "UI")
            service.SaveSetting("CurrencySymbol", txtCurrencySymbol.Text, "UI")
            SessionContext.CurrentTheme = cmbTheme.Text

            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            If isSuperAdmin Then
                service.SaveSetting("AboutText", txtAboutText.Text, "Info")
                service.SaveSetting("ContactText", txtContactText.Text, "Info")
            End If

            MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
    End Class
End Namespace
