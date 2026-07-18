Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class SettingsForm
        Inherits Form

        Private ReadOnly service As New SettingsService()
        Private _initialTheme As String

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            _initialTheme = service.GetSettingValue("Theme", "Light")
            cmbTheme.Text = _initialTheme
            cmbNumberFormat.Text = service.GetSettingValue("NumberFormat", "N2")
            txtCurrencySymbol.Text = service.GetSettingValue("CurrencySymbol", "ریال")
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            service.SaveSetting("Theme", cmbTheme.Text, "UI")
            service.SaveSetting("NumberFormat", cmbNumberFormat.Text, "UI")
            service.SaveSetting("CurrencySymbol", txtCurrencySymbol.Text, "UI")
            SessionContext.CurrentTheme = cmbTheme.Text

            MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub btnFormThemes_Click(sender As Object, e As EventArgs) Handles btnFormThemes.Click
            Using frm As New ThemeSelectionForm()
                frm.ShowDialog(Me)
            End Using
        End Sub

        Private Sub cmbTheme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTheme.SelectedIndexChanged
            ApplyPreviewTheme(cmbTheme.Text)
        End Sub

        Private Sub SettingsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
            If SessionContext.CurrentTheme = _initialTheme AndAlso cmbTheme.Text <> _initialTheme Then
                ApplyPreviewTheme(_initialTheme)
            End If
        End Sub

        Private Sub ApplyPreviewTheme(themeName As String)
            Dim mainForm = System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.OfType(Of MainForm)(Application.OpenForms))
            If mainForm IsNot Nothing Then
                If String.Equals(themeName, "Dark", StringComparison.OrdinalIgnoreCase) Then
                    mainForm.BackColor = Color.FromArgb(36, 39, 46)
                    mainForm.ForeColor = Color.WhiteSmoke
                ElseIf String.Equals(themeName, "Blue", StringComparison.OrdinalIgnoreCase) Then
                    mainForm.BackColor = Color.FromArgb(227, 238, 247)
                    mainForm.ForeColor = Color.Black
                Else
                    mainForm.BackColor = Color.WhiteSmoke
                    mainForm.ForeColor = Color.Black
                End If
            End If
        End Sub
    End Class
End Namespace
