Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardarySettingsForm
        Inherits Form

        Private ReadOnly _settingsService As New SettingsService()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AnbardarySettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadSettings()
            AddHandler cmbPurchaseMethod.SelectedIndexChanged, AddressOf CmbPurchaseMethod_SelectedIndexChanged
        End Sub

        Private Sub LoadSettings()
            Try
                ' Load product group levels
                Dim companyId = SessionContext.CurrentCompanyID
                Dim val = Sql.ExecuteScalar("SELECT ProductGroupLevels FROM Companies WHERE CompanyID = ?", companyId)
                If val IsNot Nothing AndAlso val IsNot DBNull.Value Then
                    numProductGroupLevels.Value = Convert.ToDecimal(val)
                Else
                    numProductGroupLevels.Value = 3D
                End If

                ' Load purchase pricing method
                Dim purchaseMethod = _settingsService.GetSettingValue("PurchasePricingMethod", "روش FIFO")
                Dim purchaseIdx = cmbPurchaseMethod.Items.IndexOf(purchaseMethod)
                cmbPurchaseMethod.SelectedIndex = If(purchaseIdx >= 0, purchaseIdx, 0)

                ' Load sale markup percentages (Consumer, Colleague, Wholesale)
                ' Fallback to legacy "SaleMarkupPercent" for Consumer if new key does not exist
                Dim legacyMarkup = _settingsService.GetSettingValue("SaleMarkupPercent", "0")
                txtConsumerMarkup.Text = _settingsService.GetSettingValue("SaleMarkupPercent_Consumer", legacyMarkup)
                txtColleagueMarkup.Text = _settingsService.GetSettingValue("SaleMarkupPercent_Colleague", "0")
                txtWholesaleMarkup.Text = _settingsService.GetSettingValue("SaleMarkupPercent_Wholesale", "0")

                ' Update formula labels
                UpdateSaleFormulaLabels()

            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub CmbPurchaseMethod_SelectedIndexChanged(sender As Object, e As EventArgs)
            UpdateSaleFormulaLabels()
        End Sub

        Private Sub UpdateSaleFormulaLabels()
            Dim selectedMethod As String = If(cmbPurchaseMethod.SelectedItem IsNot Nothing,
                                               cmbPurchaseMethod.SelectedItem.ToString(),
                                               "روش FIFO")
            lblConsumerFormula.Text = "قیمت مصرف‌کننده : قیمت خرید بر اساس " & selectedMethod & " + "
            lblColleagueFormula.Text = "قیمت همکار : قیمت خرید بر اساس " & selectedMethod & " + "
            lblWholesaleFormula.Text = "قیمت عمده‌فروشی : قیمت خرید بر اساس " & selectedMethod & " + "
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                ' Validate markups
                Dim consumerMarkup As Decimal
                If Not Decimal.TryParse(txtConsumerMarkup.Text.Trim(), consumerMarkup) OrElse consumerMarkup < 0 Then
                    MessageBox.Show("لطفاً یک عدد معتبر و غیر منفی برای درصد قیمت مصرف‌کننده وارد کنید.",
                                    "اعتبارسنجی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtConsumerMarkup.Focus()
                    Return
                End If

                Dim colleagueMarkup As Decimal
                If Not Decimal.TryParse(txtColleagueMarkup.Text.Trim(), colleagueMarkup) OrElse colleagueMarkup < 0 Then
                    MessageBox.Show("لطفاً یک عدد معتبر و غیر منفی برای درصد قیمت همکار وارد کنید.",
                                    "اعتبارسنجی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtColleagueMarkup.Focus()
                    Return
                End If

                Dim wholesaleMarkup As Decimal
                If Not Decimal.TryParse(txtWholesaleMarkup.Text.Trim(), wholesaleMarkup) OrElse wholesaleMarkup < 0 Then
                    MessageBox.Show("لطفاً یک عدد معتبر و غیر منفی برای درصد قیمت عمده‌فروشی وارد کنید.",
                                    "اعتبارسنجی", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtWholesaleMarkup.Focus()
                    Return
                End If

                ' Save product group levels
                Dim companyId = SessionContext.CurrentCompanyID
                Dim levelsVal = Convert.ToInt32(numProductGroupLevels.Value)

                Dim pgService As New ProductGroupService()
                Dim dtGroups = pgService.GetAll(companyId)
                For Each row As DataRow In dtGroups.Rows
                    Dim lvl = Convert.ToInt32(row("Level"))
                    If lvl >= levelsVal Then
                        MessageBox.Show(
                            "امکان کاهش تعداد سطوح به " & levelsVal & " وجود ندارد زیرا در حال حاضر گروه‌هایی در سطح " & (lvl + 1) & " تعریف شده‌اند. ابتدا زیرگروه‌های اضافی را حذف کنید.",
                            "محدودیت ساختار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                Next
                Sql.ExecuteNonQuery("UPDATE Companies SET ProductGroupLevels = ? WHERE CompanyID = ?", levelsVal, companyId)

                ' Save purchase pricing method
                Dim purchaseMethod As String = If(cmbPurchaseMethod.SelectedItem IsNot Nothing,
                                                   cmbPurchaseMethod.SelectedItem.ToString(), "روش FIFO")
                _settingsService.SaveSetting("PurchasePricingMethod", purchaseMethod, "Inventory")

                ' Save 3 sale markups
                _settingsService.SaveSetting("SaleMarkupPercent_Consumer", consumerMarkup.ToString(), "Inventory")
                _settingsService.SaveSetting("SaleMarkupPercent_Colleague", colleagueMarkup.ToString(), "Inventory")
                _settingsService.SaveSetting("SaleMarkupPercent_Wholesale", wholesaleMarkup.ToString(), "Inventory")

                MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
