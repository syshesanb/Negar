Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class SettingsForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents cmbTheme As ComboBox
        Friend WithEvents cmbNumberFormat As ComboBox
        Friend WithEvents txtCurrencySymbol As TextBox
        Friend WithEvents btnSave As Button
        Friend WithEvents btnFormThemes As Button

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.cmbTheme = New ComboBox()
            Me.cmbNumberFormat = New ComboBox()
            Me.txtCurrencySymbol = New TextBox()
            Me.btnSave = New Button()
            Me.btnFormThemes = New Button()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(620, 240)
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

            Me.btnFormThemes.Text = "تم ظاهری فرمها"
            Me.btnFormThemes.Size = New Size(130, 30)
            Me.btnFormThemes.Location = New Point(30, 18)

            Me.btnSave.Text = "ثبت و ذخیره تنظیمات"
            Me.btnSave.Size = New Size(150, 34)
            Me.btnSave.Location = New Point(235, 170)

            Me.Controls.AddRange(New Control() {
                lblTheme, Me.cmbTheme,
                lblNum, Me.cmbNumberFormat,
                lblCurrency, Me.txtCurrencySymbol,
                Me.btnSave, Me.btnFormThemes
            })
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
