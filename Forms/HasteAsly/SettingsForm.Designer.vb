Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class SettingsForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents cmbTheme As ComboBox
        Friend WithEvents cmbNumberFormat As ComboBox
        Friend WithEvents txtCurrencySymbol As TextBox
        Friend WithEvents txtAboutText As TextBox
        Friend WithEvents txtContactText As TextBox
        Friend WithEvents btnSave As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.cmbTheme = New ComboBox()
            Me.cmbNumberFormat = New ComboBox()
            Me.txtCurrencySymbol = New TextBox()
            Me.txtAboutText = New TextBox()
            Me.txtContactText = New TextBox()
            Me.btnSave = New Button()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(620, 480)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "SettingsForm"
            Me.Text = "تنظیمات عمومی و اطلاع‌رسانی"

            Dim lblTheme As New Label() With {.Text = "تم ظاهری:", .AutoSize = True, .Location = New Point(470, 25)}
            Me.cmbTheme.Location = New Point(260, 22)
            Me.cmbTheme.Width = 190
            Me.cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbTheme.Items.AddRange(New Object() {"Light", "Dark", "Blue"})

            Dim lblNum As New Label() With {.Text = "فرمت اعداد:", .AutoSize = True, .Location = New Point(470, 65)}
            Me.cmbNumberFormat.Location = New Point(260, 62)
            Me.cmbNumberFormat.Width = 190
            Me.cmbNumberFormat.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cmbNumberFormat.Items.AddRange(New Object() {"N0", "N2", "C2"})

            Dim lblCurrency As New Label() With {.Text = "واحد پول:", .AutoSize = True, .Location = New Point(470, 105)}
            Me.txtCurrencySymbol.Location = New Point(260, 102)
            Me.txtCurrencySymbol.Width = 190

            ' About section
            Dim lblAbout As New Label() With {.Text = "متن «درباره...»:", .AutoSize = True, .Location = New Point(470, 150)}
            Me.txtAboutText.Location = New Point(30, 147)
            Me.txtAboutText.Size = New Size(420, 100)
            Me.txtAboutText.Multiline = True
            Me.txtAboutText.ScrollBars = ScrollBars.Vertical

            ' Contact section
            Dim lblContact As New Label() With {.Text = "متن «ارتباط با ما»:", .AutoSize = True, .Location = New Point(470, 270)}
            Me.txtContactText.Location = New Point(30, 267)
            Me.txtContactText.Size = New Size(420, 120)
            Me.txtContactText.Multiline = True
            Me.txtContactText.ScrollBars = ScrollBars.Vertical

            Me.btnSave.Text = "ثبت و ذخیره تنظیمات"
            Me.btnSave.Size = New Size(150, 34)
            Me.btnSave.Location = New Point(235, 415)

            Me.Controls.AddRange(New Control() {
                lblTheme, Me.cmbTheme,
                lblNum, Me.cmbNumberFormat,
                lblCurrency, Me.txtCurrencySymbol,
                lblAbout, Me.txtAboutText,
                lblContact, Me.txtContactText,
                Me.btnSave
            })
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
