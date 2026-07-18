Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryInitialSettingsForm
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryInitialSettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadSettings()
        End Sub

        Private Sub LoadSettings()
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                Dim dt As DataTable = Sql.ExecuteTable("SELECT EconomicCode, TaxID, AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length, Level6Length FROM Companies WHERE CompanyID = ?", companyId)
                
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    txtEconomicCode.Text = Convert.ToString(row("EconomicCode"))
                    txtTaxId.Text = Convert.ToString(row("TaxID"))
                    numAccountLevels.Value = If(Convert.IsDBNull(row("AccountLevels")), 4D, Convert.ToDecimal(row("AccountLevels")))
                    numLevel1Length.Value = If(Convert.IsDBNull(row("Level1Length")), 2D, Convert.ToDecimal(row("Level1Length")))
                    numLevel2Length.Value = If(Convert.IsDBNull(row("Level2Length")), 2D, Convert.ToDecimal(row("Level2Length")))
                    numLevel3Length.Value = If(Convert.IsDBNull(row("Level3Length")), 2D, Convert.ToDecimal(row("Level3Length")))
                    numLevel4Length.Value = If(Convert.IsDBNull(row("Level4Length")), 2D, Convert.ToDecimal(row("Level4Length")))
                    numLevel5Length.Value = If(Convert.IsDBNull(row("Level5Length")), 2D, Convert.ToDecimal(row("Level5Length")))
                    numLevel6Length.Value = If(Convert.IsDBNull(row("Level6Length")), 2D, Convert.ToDecimal(row("Level6Length")))
                End If
                UpdateLevelsControlsState()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub numAccountLevels_ValueChanged(sender As Object, e As EventArgs) Handles numAccountLevels.ValueChanged
            UpdateLevelsControlsState()
        End Sub

        Private Sub UpdateLevelsControlsState()
            Dim lvls = CInt(numAccountLevels.Value)

            Dim UpdateControl = Sub(num As NumericUpDown, enabled As Boolean)
                                    If enabled Then
                                        num.Enabled = True
                                        num.Minimum = 2
                                        If num.Value = 0 Then num.Value = 2
                                    Else
                                        num.Minimum = 0
                                        num.Value = 0
                                        num.Enabled = False
                                    End If
                                End Sub

            UpdateControl(numLevel3Length, lvls >= 3)
            UpdateControl(numLevel4Length, lvls >= 4)
            UpdateControl(numLevel5Length, lvls >= 5)
            UpdateControl(numLevel6Length, lvls >= 6)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                
                Sql.ExecuteNonQuery("UPDATE Companies SET EconomicCode = ?, TaxID = ?, AccountLevels = ?, Level1Length = ?, Level2Length = ?, Level3Length = ?, Level4Length = ?, Level5Length = ?, Level6Length = ? WHERE CompanyID = ?", 
                                    txtEconomicCode.Text.Trim(), 
                                    txtTaxId.Text.Trim(), 
                                    Convert.ToInt32(numAccountLevels.Value), 
                                    Convert.ToInt32(numLevel1Length.Value), 
                                    Convert.ToInt32(numLevel2Length.Value), 
                                    Convert.ToInt32(numLevel3Length.Value), 
                                    Convert.ToInt32(numLevel4Length.Value), 
                                    Convert.ToInt32(numLevel5Length.Value), 
                                    Convert.ToInt32(numLevel6Length.Value), 
                                    companyId)
                                    
                MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
