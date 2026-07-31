Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.RD
    Public Class RdEditDialog
        Inherits Form

        Private txtTitle As TextBox
        Private cboCategory As ComboBox
        Private cboStage As ComboBox
        Private txtLead As TextBox
        Private txtBudget As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _rdSvc As RdService
        Private _companyID As Integer

        Public Sub New(companyID As Integer)
            _companyID = companyID
            _rdSvc = New RdService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔬 تعریف پروژه تحقیق و توسعه (NPD) جدید"
            Me.Size = New Size(520, 340)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 252)

            Dim y = 22
            Dim gap = 44

            Dim lblTitle As New Label With {.Text = "عنوان پروژه / محصول در حال توسعه:", .Location = New Point(340, y), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(20, y - 2), .Size = New Size(312, 26), .Text = "توسعه فرمول جدید محصول شوینده آنزیمی"}
            y += gap

            Dim lblCat As New Label With {.Text = "دسته‌بندی پروژه:", .Location = New Point(340, y), .AutoSize = True}
            cboCategory = New ComboBox With {.Location = New Point(160, y - 2), .Size = New Size(172, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCategory.Items.AddRange(New Object() {"فرمول جدید", "بهبود محصول موجود", "ارزیابی ماده اولیه", "تحقیق بنیادی"})
            cboCategory.SelectedIndex = 0
            y += gap

            Dim lblStage As New Label With {.Text = "مرحله فعلی (Stage-Gate):", .Location = New Point(340, y), .AutoSize = True}
            cboStage = New ComboBox With {.Location = New Point(160, y - 2), .Size = New Size(172, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboStage.Items.AddRange(New Object() {"ایده‌پردازی", "تحقیق اولیه", "فرمولاسیون", "پایلوت", "آزمون بازار", "تجاری‌سازی"})
            cboStage.SelectedIndex = 0
            y += gap

            Dim lblLead As New Label With {.Text = "R&D Lead (سرپرست فنی):", .Location = New Point(340, y), .AutoSize = True}
            txtLead = New TextBox With {.Location = New Point(160, y - 2), .Size = New Size(172, 26), .Text = "دکتر ساره محمدی"}
            y += gap

            Dim lblBudget As New Label With {.Text = "بودجه پروژه (ریال):", .Location = New Point(340, y), .AutoSize = True}
            txtBudget = New TextBox With {.Location = New Point(160, y - 2), .Size = New Size(172, 26), .Text = "500000000"}
            y += gap + 6

            btnSave = New Button With {
                .Text = "💾 ثبت پروژه R&D",
                .Size = New Size(170, 36),
                .Location = New Point(160, y),
                .BackColor = Color.FromArgb(27, 94, 32),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(110, 36),
                .Location = New Point(40, y),
                .BackColor = Color.FromArgb(183, 28, 28),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblTitle, txtTitle, lblCat, cboCategory, lblStage, cboStage,
                lblLead, txtLead, lblBudget, txtBudget, btnSave, btnCancel
            })
            Me.ClientSize = New Size(520, y + 50)
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان پروژه را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim budget As Double = 0
            Double.TryParse(txtBudget.Text.Replace(",", ""), budget)
            _rdSvc.SaveProject(_companyID, txtTitle.Text, cboCategory.SelectedItem.ToString(), cboStage.SelectedItem.ToString(), txtLead.Text, budget)
            MessageBox.Show("پروژه R&D با موفقیت ثبت گردید.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
