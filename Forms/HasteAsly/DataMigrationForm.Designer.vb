Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Namespace Sys_Hes_Anb.Forms
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class DataMigrationForm
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlUserSelect As Panel
        Friend WithEvents lblTargetUser As Label
        Friend WithEvents cmbTargetUser As ComboBox
        Friend WithEvents lblTargetCompany As Label
        Friend WithEvents cmbTargetCompany As ComboBox

        Friend WithEvents tabControl As TabControl

        Friend WithEvents tabCoA As TabPage
        Friend WithEvents btnDownloadCoATemplate As Button
        Friend WithEvents btnImportCoA As Button
        Friend WithEvents btnHelpCoA As Button
        Friend WithEvents btnSmartConvertCoA As Button
        Friend WithEvents lblCoA As Label

        Friend WithEvents tabShenavar As TabPage
        Friend WithEvents btnDownloadShenavarTemplate As Button
        Friend WithEvents btnImportShenavar As Button
        Friend WithEvents btnHelpShenavar As Button
        Friend WithEvents lblShenavar As Label

        Friend WithEvents tabDocs As TabPage
        Friend WithEvents btnDownloadDocsTemplate As Button
        Friend WithEvents btnImportDocs As Button
        Friend WithEvents btnHelpDocs As Button
        Friend WithEvents btnSmartConvertDocs As Button
        Friend WithEvents lblDocs As Label

        Friend WithEvents tabProducts As TabPage
        Friend WithEvents btnDownloadProductTemplate As Button
        Friend WithEvents btnImportProducts As Button
        Friend WithEvents btnHelpProducts As Button
        Friend WithEvents lblInfo1 As Label

        Friend WithEvents tabUsers As TabPage
        Friend WithEvents btnDownloadUserTemplate As Button
        Friend WithEvents btnImportUsers As Button
        Friend WithEvents btnHelpUsers As Button
        Friend WithEvents lblInfo2 As Label

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()

            Me.pnlUserSelect = New Panel()
            Me.lblTargetUser = New Label()
            Me.cmbTargetUser = New ComboBox()
            Me.lblTargetCompany = New Label()
            Me.cmbTargetCompany = New ComboBox()

            Me.tabControl = New TabControl()

            Me.tabCoA = New TabPage()
            Me.btnDownloadCoATemplate = New Button()
            Me.btnImportCoA = New Button()
            Me.btnHelpCoA = New Button()
            Me.btnSmartConvertCoA = New Button()
            Me.lblCoA = New Label()

            Me.tabShenavar = New TabPage()
            Me.btnDownloadShenavarTemplate = New Button()
            Me.btnImportShenavar = New Button()
            Me.btnHelpShenavar = New Button()
            Me.lblShenavar = New Label()

            Me.tabDocs = New TabPage()
            Me.btnDownloadDocsTemplate = New Button()
            Me.btnImportDocs = New Button()
            Me.btnHelpDocs = New Button()
            Me.btnSmartConvertDocs = New Button()
            Me.lblDocs = New Label()

            Me.tabProducts = New TabPage()
            Me.btnDownloadProductTemplate = New Button()
            Me.btnImportProducts = New Button()
            Me.btnHelpProducts = New Button()
            Me.lblInfo1 = New Label()

            Me.tabUsers = New TabPage()
            Me.btnDownloadUserTemplate = New Button()
            Me.btnImportUsers = New Button()
            Me.btnHelpUsers = New Button()
            Me.lblInfo2 = New Label()

            Me.pnlUserSelect.SuspendLayout()
            Me.tabControl.SuspendLayout()
            Me.tabCoA.SuspendLayout()
            Me.tabShenavar.SuspendLayout()
            Me.tabDocs.SuspendLayout()
            Me.tabProducts.SuspendLayout()
            Me.tabUsers.SuspendLayout()
            Me.SuspendLayout()

            ' pnlUserSelect
            Me.pnlUserSelect.Dock = DockStyle.Top
            Me.pnlUserSelect.Height = 50
            Me.pnlUserSelect.Padding = New Padding(8, 6, 8, 6)
            Me.pnlUserSelect.BackColor = System.Drawing.Color.FromArgb(240, 240, 240)
            Me.pnlUserSelect.Controls.Add(Me.cmbTargetCompany)
            Me.pnlUserSelect.Controls.Add(Me.lblTargetCompany)
            Me.pnlUserSelect.Controls.Add(Me.cmbTargetUser)
            Me.pnlUserSelect.Controls.Add(Me.lblTargetUser)
            Me.pnlUserSelect.Name = "pnlUserSelect"

            Me.lblTargetUser.AutoSize = True
            Me.lblTargetUser.Location = New Point(490, 15)
            Me.lblTargetUser.Name = "lblTargetUser"
            Me.lblTargetUser.Text = "انتخاب کاربری که تبدیل برای او انجام می‌شود:"

            Me.cmbTargetUser.Location = New Point(210, 11)
            Me.cmbTargetUser.Name = "cmbTargetUser"
            Me.cmbTargetUser.Size = New Size(270, 22)
            Me.cmbTargetUser.DropDownStyle = ComboBoxStyle.DropDownList

            Me.lblTargetCompany.AutoSize = True
            Me.lblTargetCompany.Location = New Point(490, 35)
            Me.lblTargetCompany.Name = "lblTargetCompany"
            Me.lblTargetCompany.Text = "انتخاب شرکت:"
            Me.lblTargetCompany.Visible = False

            Me.cmbTargetCompany.Location = New Point(210, 31)
            Me.cmbTargetCompany.Name = "cmbTargetCompany"
            Me.cmbTargetCompany.Size = New Size(270, 22)
            Me.cmbTargetCompany.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbTargetCompany.Visible = False

            ' tabControl
            Me.tabControl.Controls.Add(Me.tabCoA)
            Me.tabControl.Controls.Add(Me.tabShenavar)
            Me.tabControl.Controls.Add(Me.tabDocs)
            Me.tabControl.Controls.Add(Me.tabProducts)
            Me.tabControl.Controls.Add(Me.tabUsers)
            Me.tabControl.Dock = DockStyle.Fill
            Me.tabControl.Name = "tabControl"
            Me.tabControl.RightToLeftLayout = True
            Me.tabControl.SelectedIndex = 0
            Me.tabControl.TabIndex = 0

            Dim infoText As String = "مرحله ۱: فایل نمونه را دانلود کنید." & vbCrLf & "مرحله ۲: اطلاعات خود را در فایل جایگذاری کنید." & vbCrLf & "مرحله ۳: فایل تکمیل شده را برای درج در سیستم انتخاب کنید."

            ' tabCoA
            Me.tabCoA.Controls.Add(Me.lblCoA)
            Me.tabCoA.Controls.Add(Me.btnDownloadCoATemplate)
            Me.tabCoA.Controls.Add(Me.btnImportCoA)
            Me.tabCoA.Controls.Add(Me.btnHelpCoA)
            Me.tabCoA.Controls.Add(Me.btnSmartConvertCoA)
            Me.tabCoA.Name = "tabCoA"
            Me.tabCoA.Text = "انتقال سرفصل حسابها"
            Me.tabCoA.UseVisualStyleBackColor = True

            Me.lblCoA.Location = New Point(20, 20)
            Me.lblCoA.Name = "lblCoA"
            Me.lblCoA.Size = New Size(700, 60)
            Me.lblCoA.Text = infoText

            Me.btnDownloadCoATemplate.Location = New Point(420, 100)
            Me.btnDownloadCoATemplate.Name = "btnDownloadCoATemplate"
            Me.btnDownloadCoATemplate.Size = New Size(220, 40)
            Me.btnDownloadCoATemplate.Text = "دانلود فایل نمونه سرفصل‌ها"
            Me.btnDownloadCoATemplate.UseVisualStyleBackColor = True

            Me.btnImportCoA.Location = New Point(170, 100)
            Me.btnImportCoA.Name = "btnImportCoA"
            Me.btnImportCoA.Size = New Size(220, 40)
            Me.btnImportCoA.Text = "انتخاب فایل و انتقال سرفصل‌ها"
            Me.btnImportCoA.UseVisualStyleBackColor = True

            Me.btnHelpCoA.Location = New Point(170, 160)
            Me.btnHelpCoA.Name = "btnHelpCoA"
            Me.btnHelpCoA.Size = New Size(470, 40)
            Me.btnHelpCoA.Text = "راهنمای تکمیل ستون‌های فایل نمونه سرفصل‌ها"
            Me.btnHelpCoA.UseVisualStyleBackColor = True

            Me.btnSmartConvertCoA.Location = New Point(170, 220)
            Me.btnSmartConvertCoA.Name = "btnSmartConvertCoA"
            Me.btnSmartConvertCoA.Size = New Size(470, 40)
            Me.btnSmartConvertCoA.Text = "تبدیل هوشمند سرفصل حسابها به فایل نمونه"
            Me.btnSmartConvertCoA.UseVisualStyleBackColor = True

            ' tabShenavar
            Me.tabShenavar.Controls.Add(Me.lblShenavar)
            Me.tabShenavar.Controls.Add(Me.btnDownloadShenavarTemplate)
            Me.tabShenavar.Controls.Add(Me.btnImportShenavar)
            Me.tabShenavar.Controls.Add(Me.btnHelpShenavar)
            Me.tabShenavar.Name = "tabShenavar"
            Me.tabShenavar.Text = "انتقال حسابهای شناور"
            Me.tabShenavar.UseVisualStyleBackColor = True

            Me.lblShenavar.Location = New Point(20, 20)
            Me.lblShenavar.Name = "lblShenavar"
            Me.lblShenavar.Size = New Size(700, 60)
            Me.lblShenavar.Text = infoText

            Me.btnDownloadShenavarTemplate.Location = New Point(420, 100)
            Me.btnDownloadShenavarTemplate.Name = "btnDownloadShenavarTemplate"
            Me.btnDownloadShenavarTemplate.Size = New Size(220, 40)
            Me.btnDownloadShenavarTemplate.Text = "دانلود فایل نمونه شناورها"
            Me.btnDownloadShenavarTemplate.UseVisualStyleBackColor = True

            Me.btnImportShenavar.Location = New Point(170, 100)
            Me.btnImportShenavar.Name = "btnImportShenavar"
            Me.btnImportShenavar.Size = New Size(220, 40)
            Me.btnImportShenavar.Text = "انتخاب فایل و انتقال شناورها"
            Me.btnImportShenavar.UseVisualStyleBackColor = True

            Me.btnHelpShenavar.Location = New Point(170, 160)
            Me.btnHelpShenavar.Name = "btnHelpShenavar"
            Me.btnHelpShenavar.Size = New Size(470, 40)
            Me.btnHelpShenavar.Text = "راهنمای تکمیل ستون‌های فایل نمونه شناورها"
            Me.btnHelpShenavar.UseVisualStyleBackColor = True

            ' tabDocs
            Me.tabDocs.Controls.Add(Me.lblDocs)
            Me.tabDocs.Controls.Add(Me.btnDownloadDocsTemplate)
            Me.tabDocs.Controls.Add(Me.btnImportDocs)
            Me.tabDocs.Controls.Add(Me.btnHelpDocs)
            Me.tabDocs.Controls.Add(Me.btnSmartConvertDocs)
            Me.tabDocs.Name = "tabDocs"
            Me.tabDocs.Text = "انتقال اسناد حسابداری"
            Me.tabDocs.UseVisualStyleBackColor = True

            Me.lblDocs.Location = New Point(20, 20)
            Me.lblDocs.Name = "lblDocs"
            Me.lblDocs.Size = New Size(700, 60)
            Me.lblDocs.Text = infoText & vbCrLf & "نکته: در صورت ناتراز بودن مبلغ اسناد، عملیات انتقال برای آن سند متوقف خواهد شد."

            Me.btnDownloadDocsTemplate.Location = New Point(420, 100)
            Me.btnDownloadDocsTemplate.Name = "btnDownloadDocsTemplate"
            Me.btnDownloadDocsTemplate.Size = New Size(220, 40)
            Me.btnDownloadDocsTemplate.Text = "دانلود فایل نمونه اسناد"
            Me.btnDownloadDocsTemplate.UseVisualStyleBackColor = True

            Me.btnImportDocs.Location = New Point(170, 100)
            Me.btnImportDocs.Name = "btnImportDocs"
            Me.btnImportDocs.Size = New Size(220, 40)
            Me.btnImportDocs.Text = "انتخاب فایل و انتقال اسناد"
            Me.btnImportDocs.UseVisualStyleBackColor = True

            Me.btnHelpDocs.Location = New Point(170, 160)
            Me.btnHelpDocs.Name = "btnHelpDocs"
            Me.btnHelpDocs.Size = New Size(470, 40)
            Me.btnHelpDocs.Text = "راهنمای تکمیل ستون‌های فایل نمونه اسناد"
            Me.btnHelpDocs.UseVisualStyleBackColor = True

            Me.btnSmartConvertDocs.Location = New Point(170, 220)
            Me.btnSmartConvertDocs.Name = "btnSmartConvertDocs"
            Me.btnSmartConvertDocs.Size = New Size(470, 40)
            Me.btnSmartConvertDocs.Text = "تبدیل هوشمند اسناد حسابداری به فایل نمونه"
            Me.btnSmartConvertDocs.UseVisualStyleBackColor = True

            ' tabProducts
            Me.tabProducts.Controls.Add(Me.lblInfo1)
            Me.tabProducts.Controls.Add(Me.btnDownloadProductTemplate)
            Me.tabProducts.Controls.Add(Me.btnImportProducts)
            Me.tabProducts.Controls.Add(Me.btnHelpProducts)
            Me.tabProducts.Name = "tabProducts"
            Me.tabProducts.Text = "انتقال کالاها"
            Me.tabProducts.UseVisualStyleBackColor = True

            Me.lblInfo1.Location = New Point(20, 20)
            Me.lblInfo1.Name = "lblInfo1"
            Me.lblInfo1.Size = New Size(700, 60)
            Me.lblInfo1.Text = infoText

            Me.btnDownloadProductTemplate.Location = New Point(420, 100)
            Me.btnDownloadProductTemplate.Name = "btnDownloadProductTemplate"
            Me.btnDownloadProductTemplate.Size = New Size(220, 40)
            Me.btnDownloadProductTemplate.Text = "دانلود فایل نمونه کالاها"
            Me.btnDownloadProductTemplate.UseVisualStyleBackColor = True

            Me.btnImportProducts.Location = New Point(170, 100)
            Me.btnImportProducts.Name = "btnImportProducts"
            Me.btnImportProducts.Size = New Size(220, 40)
            Me.btnImportProducts.Text = "انتخاب فایل و انتقال کالاها"
            Me.btnImportProducts.UseVisualStyleBackColor = True

            Me.btnHelpProducts.Location = New Point(170, 160)
            Me.btnHelpProducts.Name = "btnHelpProducts"
            Me.btnHelpProducts.Size = New Size(470, 40)
            Me.btnHelpProducts.Text = "راهنمای تکمیل ستون‌های فایل نمونه کالاها"
            Me.btnHelpProducts.UseVisualStyleBackColor = True

            ' tabUsers
            Me.tabUsers.Controls.Add(Me.lblInfo2)
            Me.tabUsers.Controls.Add(Me.btnDownloadUserTemplate)
            Me.tabUsers.Controls.Add(Me.btnImportUsers)
            Me.tabUsers.Controls.Add(Me.btnHelpUsers)
            Me.tabUsers.Name = "tabUsers"
            Me.tabUsers.Text = "انتقال اشخاص/کاربران"
            Me.tabUsers.UseVisualStyleBackColor = True

            Me.lblInfo2.Location = New Point(20, 20)
            Me.lblInfo2.Name = "lblInfo2"
            Me.lblInfo2.Size = New Size(700, 60)
            Me.lblInfo2.Text = infoText

            Me.btnDownloadUserTemplate.Location = New Point(420, 100)
            Me.btnDownloadUserTemplate.Name = "btnDownloadUserTemplate"
            Me.btnDownloadUserTemplate.Size = New Size(220, 40)
            Me.btnDownloadUserTemplate.Text = "دانلود فایل نمونه اشخاص"
            Me.btnDownloadUserTemplate.UseVisualStyleBackColor = True

            Me.btnImportUsers.Location = New Point(170, 100)
            Me.btnImportUsers.Name = "btnImportUsers"
            Me.btnImportUsers.Size = New Size(220, 40)
            Me.btnImportUsers.Text = "انتخاب فایل و انتقال اشخاص"
            Me.btnImportUsers.UseVisualStyleBackColor = True

            Me.btnHelpUsers.Location = New Point(170, 160)
            Me.btnHelpUsers.Name = "btnHelpUsers"
            Me.btnHelpUsers.Size = New Size(470, 40)
            Me.btnHelpUsers.Text = "راهنمای تکمیل ستون‌های فایل نمونه اشخاص"
            Me.btnHelpUsers.UseVisualStyleBackColor = True

            ' Form
            Me.AutoScaleDimensions = New SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(760, 500)
            Me.Controls.Add(Me.tabControl)
            Me.Controls.Add(Me.pnlUserSelect)
            Me.Font = New Font("Tahoma", 8.25!)
            Me.FormBorderStyle = FormBorderStyle.Sizable
            Me.MinimumSize = New Size(760, 500)
            Me.Name = "DataMigrationForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "تبدیل دیتا از سایر نرم افزارها"

            Me.pnlUserSelect.ResumeLayout(False)
            Me.pnlUserSelect.PerformLayout()
            Me.tabControl.ResumeLayout(False)
            Me.tabCoA.ResumeLayout(False)
            Me.tabShenavar.ResumeLayout(False)
            Me.tabDocs.ResumeLayout(False)
            Me.tabProducts.ResumeLayout(False)
            Me.tabUsers.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
