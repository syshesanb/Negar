Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardarySettingsForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents grpProductGroupSettings As GroupBox
        Friend WithEvents lblProductGroupLevels As Label
        Friend WithEvents numProductGroupLevels As NumericUpDown
        Friend WithEvents btnSave As Button

        Friend WithEvents grpPurchasePricing As GroupBox
        Friend WithEvents lblPurchaseMethod As Label
        Friend WithEvents cmbPurchaseMethod As ComboBox

        Friend WithEvents grpSalePricing As GroupBox
        Friend WithEvents lblConsumerFormula As Label
        Friend WithEvents txtConsumerMarkup As TextBox
        Friend WithEvents lblConsumerPercent As Label

        Friend WithEvents lblColleagueFormula As Label
        Friend WithEvents txtColleagueMarkup As TextBox
        Friend WithEvents lblColleaguePercent As Label

        Friend WithEvents lblWholesaleFormula As Label
        Friend WithEvents txtWholesaleMarkup As TextBox
        Friend WithEvents lblWholesalePercent As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.grpProductGroupSettings = New GroupBox()
            Me.lblProductGroupLevels = New Label()
            Me.numProductGroupLevels = New NumericUpDown()
            Me.btnSave = New Button()
            Me.grpPurchasePricing = New GroupBox()
            Me.lblPurchaseMethod = New Label()
            Me.cmbPurchaseMethod = New ComboBox()
            Me.grpSalePricing = New GroupBox()
            Me.lblConsumerFormula = New Label()
            Me.txtConsumerMarkup = New TextBox()
            Me.lblConsumerPercent = New Label()
            Me.lblColleagueFormula = New Label()
            Me.txtColleagueMarkup = New TextBox()
            Me.lblColleaguePercent = New Label()
            Me.lblWholesaleFormula = New Label()
            Me.txtWholesaleMarkup = New TextBox()
            Me.lblWholesalePercent = New Label()
            CType(Me.numProductGroupLevels, ISupportInitialize).BeginInit()
            Me.grpProductGroupSettings.SuspendLayout()
            Me.grpPurchasePricing.SuspendLayout()
            Me.grpSalePricing.SuspendLayout()
            Me.SuspendLayout()

            '
            'grpProductGroupSettings
            '
            Me.grpProductGroupSettings.Controls.Add(Me.lblProductGroupLevels)
            Me.grpProductGroupSettings.Controls.Add(Me.numProductGroupLevels)
            Me.grpProductGroupSettings.Controls.Add(Me.btnSave)
            Me.grpProductGroupSettings.Location = New Point(15, 15)
            Me.grpProductGroupSettings.Name = "grpProductGroupSettings"
            Me.grpProductGroupSettings.Size = New Size(740, 75)
            Me.grpProductGroupSettings.TabIndex = 0
            Me.grpProductGroupSettings.TabStop = False
            Me.grpProductGroupSettings.Text = "تنظیمات گروه بندی کالاها"
            '
            'lblProductGroupLevels
            '
            Me.lblProductGroupLevels.Location = New Point(490, 32)
            Me.lblProductGroupLevels.Name = "lblProductGroupLevels"
            Me.lblProductGroupLevels.Size = New Size(235, 20)
            Me.lblProductGroupLevels.TabIndex = 0
            Me.lblProductGroupLevels.Text = "تعداد سطوح گروه بندی کالا:"
            Me.lblProductGroupLevels.TextAlign = ContentAlignment.MiddleLeft
            '
            'numProductGroupLevels
            '
            Me.numProductGroupLevels.Location = New Point(380, 30)
            Me.numProductGroupLevels.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numProductGroupLevels.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.numProductGroupLevels.Value = New Decimal(New Integer() {3, 0, 0, 0})
            Me.numProductGroupLevels.Name = "numProductGroupLevels"
            Me.numProductGroupLevels.Size = New Size(80, 22)
            Me.numProductGroupLevels.TabIndex = 1
            Me.numProductGroupLevels.TextAlign = HorizontalAlignment.Center
            '
            'btnSave
            '
            Me.btnSave.BackColor = Color.FromArgb(40, 167, 69)
            Me.btnSave.FlatStyle = FlatStyle.Flat
            Me.btnSave.ForeColor = Color.White
            Me.btnSave.Location = New Point(15, 27)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(130, 30)
            Me.btnSave.TabIndex = 2
            Me.btnSave.Text = "ذخیره تنظیمات"
            Me.btnSave.UseVisualStyleBackColor = False

            '
            'grpPurchasePricing
            '
            Me.grpPurchasePricing.Controls.Add(Me.lblPurchaseMethod)
            Me.grpPurchasePricing.Controls.Add(Me.cmbPurchaseMethod)
            Me.grpPurchasePricing.Location = New Point(15, 105)
            Me.grpPurchasePricing.Name = "grpPurchasePricing"
            Me.grpPurchasePricing.Size = New Size(740, 75)
            Me.grpPurchasePricing.TabIndex = 1
            Me.grpPurchasePricing.TabStop = False
            Me.grpPurchasePricing.Text = "تنظیمات روش قیمت گذاری خرید"
            '
            'lblPurchaseMethod
            '
            Me.lblPurchaseMethod.Location = New Point(490, 32)
            Me.lblPurchaseMethod.Name = "lblPurchaseMethod"
            Me.lblPurchaseMethod.Size = New Size(235, 20)
            Me.lblPurchaseMethod.TabIndex = 0
            Me.lblPurchaseMethod.Text = "انتخاب روش قیمت گذاری خرید :"
            Me.lblPurchaseMethod.TextAlign = ContentAlignment.MiddleLeft
            '
            'cmbPurchaseMethod
            '
            Me.cmbPurchaseMethod.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbPurchaseMethod.Items.AddRange(New Object() {
                "روش FIFO",
                "روش LIFO",
                "روش میانگین ساده",
                "روش میانگین موزون",
                "روش میانگین متحرک"
            })
            Me.cmbPurchaseMethod.Location = New Point(200, 29)
            Me.cmbPurchaseMethod.Name = "cmbPurchaseMethod"
            Me.cmbPurchaseMethod.Size = New Size(270, 22)
            Me.cmbPurchaseMethod.TabIndex = 1

            '
            'grpSalePricing
            '
            Me.grpSalePricing.Controls.Add(Me.lblConsumerFormula)
            Me.grpSalePricing.Controls.Add(Me.txtConsumerMarkup)
            Me.grpSalePricing.Controls.Add(Me.lblConsumerPercent)
            Me.grpSalePricing.Controls.Add(Me.lblColleagueFormula)
            Me.grpSalePricing.Controls.Add(Me.txtColleagueMarkup)
            Me.grpSalePricing.Controls.Add(Me.lblColleaguePercent)
            Me.grpSalePricing.Controls.Add(Me.lblWholesaleFormula)
            Me.grpSalePricing.Controls.Add(Me.txtWholesaleMarkup)
            Me.grpSalePricing.Controls.Add(Me.lblWholesalePercent)
            Me.grpSalePricing.Location = New Point(15, 195)
            Me.grpSalePricing.Name = "grpSalePricing"
            Me.grpSalePricing.Size = New Size(740, 155)
            Me.grpSalePricing.TabIndex = 2
            Me.grpSalePricing.TabStop = False
            Me.grpSalePricing.Text = "تنظیمات روش قیمت گذاری فروش"
            '
            'lblConsumerFormula
            '
            Me.lblConsumerFormula.Location = New Point(370, 28)
            Me.lblConsumerFormula.Name = "lblConsumerFormula"
            Me.lblConsumerFormula.Size = New Size(355, 20)
            Me.lblConsumerFormula.TabIndex = 0
            Me.lblConsumerFormula.Text = "قیمت مصرف‌کننده : قیمت خرید بر اساس روش انتخابی + "
            Me.lblConsumerFormula.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtConsumerMarkup
            '
            Me.txtConsumerMarkup.Location = New Point(265, 25)
            Me.txtConsumerMarkup.Name = "txtConsumerMarkup"
            Me.txtConsumerMarkup.Size = New Size(95, 22)
            Me.txtConsumerMarkup.TabIndex = 1
            Me.txtConsumerMarkup.TextAlign = HorizontalAlignment.Center
            '
            'lblConsumerPercent
            '
            Me.lblConsumerPercent.Location = New Point(100, 28)
            Me.lblConsumerPercent.Name = "lblConsumerPercent"
            Me.lblConsumerPercent.Size = New Size(160, 20)
            Me.lblConsumerPercent.TabIndex = 2
            Me.lblConsumerPercent.Text = "درصد قیمت خرید"
            Me.lblConsumerPercent.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblColleagueFormula
            '
            Me.lblColleagueFormula.Location = New Point(370, 68)
            Me.lblColleagueFormula.Name = "lblColleagueFormula"
            Me.lblColleagueFormula.Size = New Size(355, 20)
            Me.lblColleagueFormula.TabIndex = 3
            Me.lblColleagueFormula.Text = "قیمت همکار : قیمت خرید بر اساس روش انتخابی + "
            Me.lblColleagueFormula.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtColleagueMarkup
            '
            Me.txtColleagueMarkup.Location = New Point(265, 65)
            Me.txtColleagueMarkup.Name = "txtColleagueMarkup"
            Me.txtColleagueMarkup.Size = New Size(95, 22)
            Me.txtColleagueMarkup.TabIndex = 4
            Me.txtColleagueMarkup.TextAlign = HorizontalAlignment.Center
            '
            'lblColleaguePercent
            '
            Me.lblColleaguePercent.Location = New Point(100, 68)
            Me.lblColleaguePercent.Name = "lblColleaguePercent"
            Me.lblColleaguePercent.Size = New Size(160, 20)
            Me.lblColleaguePercent.TabIndex = 5
            Me.lblColleaguePercent.Text = "درصد قیمت خرید"
            Me.lblColleaguePercent.TextAlign = ContentAlignment.MiddleLeft
            '
            'lblWholesaleFormula
            '
            Me.lblWholesaleFormula.Location = New Point(370, 108)
            Me.lblWholesaleFormula.Name = "lblWholesaleFormula"
            Me.lblWholesaleFormula.Size = New Size(355, 20)
            Me.lblWholesaleFormula.TabIndex = 6
            Me.lblWholesaleFormula.Text = "قیمت عمده‌فروشی : قیمت خرید بر اساس روش انتخابی + "
            Me.lblWholesaleFormula.TextAlign = ContentAlignment.MiddleLeft
            '
            'txtWholesaleMarkup
            '
            Me.txtWholesaleMarkup.Location = New Point(265, 105)
            Me.txtWholesaleMarkup.Name = "txtWholesaleMarkup"
            Me.txtWholesaleMarkup.Size = New Size(95, 22)
            Me.txtWholesaleMarkup.TabIndex = 7
            Me.txtWholesaleMarkup.TextAlign = HorizontalAlignment.Center
            '
            'lblWholesalePercent
            '
            Me.lblWholesalePercent.Location = New Point(100, 108)
            Me.lblWholesalePercent.Name = "lblWholesalePercent"
            Me.lblWholesalePercent.Size = New Size(160, 20)
            Me.lblWholesalePercent.TabIndex = 8
            Me.lblWholesalePercent.Text = "درصد قیمت خرید"
            Me.lblWholesalePercent.TextAlign = ContentAlignment.MiddleLeft

            '
            'AnbardarySettingsForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1300, 700)
            Me.Controls.Add(Me.grpProductGroupSettings)
            Me.Controls.Add(Me.grpPurchasePricing)
            Me.Controls.Add(Me.grpSalePricing)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.Name = "AnbardarySettingsForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Text = "تنظیمات اولیه انبارداری و فروش"
            CType(Me.numProductGroupLevels, ISupportInitialize).EndInit()
            Me.grpProductGroupSettings.ResumeLayout(False)
            Me.grpPurchasePricing.ResumeLayout(False)
            Me.grpSalePricing.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
