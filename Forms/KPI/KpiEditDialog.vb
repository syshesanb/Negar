Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.KPI
    Public Class KpiEditDialog
        Inherits Form

        Private txtPerson As TextBox
        Private txtTitle As TextBox
        Private cboCategory As ComboBox
        Private txtTargetVal As TextBox
        Private txtActualVal As TextBox
        Private txtWeight As TextBox
        Private txtUnit As TextBox
        Private txtPeriod As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _kpiSvc As KpiService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _kpiSvc = New KpiService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🎯 تعریف شاخص عملکردی (KPI) / هدف جدید"
            Me.Size = New Size(520, 420)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblPerson As New Label With {.Text = "نام پرسنل / کارمند:", .Location = New Point(370, 25), .AutoSize = True}
            txtPerson = New TextBox With {.Location = New Point(30, 22), .Size = New Size(320, 26), .Text = "رضا محمدی"}

            Dim lblTitle As New Label With {.Text = "عنوان شاخص / هدف:", .Location = New Point(370, 65), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 62), .Size = New Size(320, 26)}

            Dim lblCategory As New Label With {.Text = "دسته شاخص:", .Location = New Point(370, 105), .AutoSize = True}
            cboCategory = New ComboBox With {.Location = New Point(170, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCategory.Items.AddRange(New Object() {"فروش", "تولید", "انضباط اداری", "پشتیبانی مشتریان", "مالی و خزانه‌داری"})
            cboCategory.SelectedIndex = 0

            Dim lblTargetVal As New Label With {.Text = "مقدار هدف (Target):", .Location = New Point(370, 145), .AutoSize = True}
            txtTargetVal = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "5000000000"}

            Dim lblActualVal As New Label With {.Text = "مقدار واقعی (Actual):", .Location = New Point(370, 185), .AutoSize = True}
            txtActualVal = New TextBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .Text = "4800000000"}

            Dim lblWeight As New Label With {.Text = "وزن شاخص (%):", .Location = New Point(370, 225), .AutoSize = True}
            txtWeight = New TextBox With {.Location = New Point(170, 222), .Size = New Size(180, 26), .Text = "25"}

            Dim lblUnit As New Label With {.Text = "واحد سنجش:", .Location = New Point(370, 265), .AutoSize = True}
            txtUnit = New TextBox With {.Location = New Point(170, 262), .Size = New Size(180, 26), .Text = "مبلغ"}

            Dim lblPeriod As New Label With {.Text = "دوره ارزیابی:", .Location = New Point(370, 305), .AutoSize = True}
            txtPeriod = New TextBox With {.Location = New Point(170, 302), .Size = New Size(180, 26), .Text = "بهار ۱۴۰۵"}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره شاخص",
                .Size = New Size(140, 36),
                .Location = New Point(210, 345),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(100, 345),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblPerson, txtPerson, lblTitle, txtTitle, lblCategory, cboCategory,
                lblTargetVal, txtTargetVal, lblActualVal, txtActualVal, lblWeight, txtWeight,
                lblUnit, txtUnit, lblPeriod, txtPeriod, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً عنوان شاخص را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim targetVal As Double = 0
            Double.TryParse(txtTargetVal.Text.Replace(",", ""), targetVal)

            Dim actualVal As Double = 0
            Double.TryParse(txtActualVal.Text.Replace(",", ""), actualVal)

            Dim weight As Double = 25
            Double.TryParse(txtWeight.Text, weight)

            _kpiSvc.SaveKpiTarget(
                _id, _companyID, txtPerson.Text, txtTitle.Text,
                cboCategory.SelectedItem.ToString(), targetVal, actualVal,
                weight, txtUnit.Text, txtPeriod.Text
            )

            MessageBox.Show("شاخص KPI جدید با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
