Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Production
    Public Class ProductionEditDialog
        Inherits Form

        Private txtOrderNo As TextBox
        Private txtProdCode As TextBox
        Private txtProdName As TextBox
        Private txtTargetQty As TextBox
        Private txtMatCost As TextBox
        Private txtLaborCost As TextBox
        Private txtOverheadCost As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _prodSvc As ProductionService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _prodSvc = New ProductionService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_id <= 0, "🏭 ثبت کارت / دستور تولید جدید", "🏭 ویرایش کارت تولید")
            Me.Size = New Size(540, 440)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblOrderNo As New Label With {.Text = "شماره دستور تولید:", .Location = New Point(380, 25), .AutoSize = True}
            txtOrderNo = New TextBox With {.Location = New Point(180, 22), .Size = New Size(180, 26), .Text = "ORD-" & (Environment.TickCount Mod 10000).ToString()}

            Dim lblCode As New Label With {.Text = "کد محصول:", .Location = New Point(380, 65), .AutoSize = True}
            txtProdCode = New TextBox With {.Location = New Point(180, 62), .Size = New Size(180, 26), .Text = "PRD-501"}

            Dim lblName As New Label With {.Text = "نام محصول:", .Location = New Point(380, 105), .AutoSize = True}
            txtProdName = New TextBox With {.Location = New Point(30, 102), .Size = New Size(330, 26)}

            Dim lblTargetQty As New Label With {.Text = "تیراژ تولید (تعداد):", .Location = New Point(380, 145), .AutoSize = True}
            txtTargetQty = New TextBox With {.Location = New Point(180, 142), .Size = New Size(180, 26), .Text = "10"}

            Dim lblMatCost As New Label With {.Text = "هزینه مواد مستقیم (ریال):", .Location = New Point(380, 185), .AutoSize = True}
            txtMatCost = New TextBox With {.Location = New Point(180, 182), .Size = New Size(180, 26), .Text = "150000000"}

            Dim lblLaborCost As New Label With {.Text = "هزینه دستمزد مستقیم (ریال):", .Location = New Point(380, 225), .AutoSize = True}
            txtLaborCost = New TextBox With {.Location = New Point(180, 222), .Size = New Size(180, 26), .Text = "30000000"}

            Dim lblOverhead As New Label With {.Text = "هزینه سربار تولید (ریال):", .Location = New Point(380, 265), .AutoSize = True}
            txtOverheadCost = New TextBox With {.Location = New Point(180, 262), .Size = New Size(180, 26), .Text = "20000000"}

            Dim lblNotes As New Label With {.Text = "توضیحات:", .Location = New Point(380, 305), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 302), .Size = New Size(330, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(240, 350),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(130, 350),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblOrderNo, txtOrderNo, lblCode, txtProdCode, lblName, txtProdName,
                lblTargetQty, txtTargetQty, lblMatCost, txtMatCost, lblLaborCost, txtLaborCost,
                lblOverhead, txtOverheadCost, lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            If _id > 0 Then
                Dim row = _prodSvc.GetProductionOrderById(_id)
                If row IsNot Nothing Then
                    txtOrderNo.Text = Convert.ToString(row("OrderNo"))
                    txtProdCode.Text = Convert.ToString(row("ProductCode"))
                    txtProdName.Text = Convert.ToString(row("ProductName"))
                    txtTargetQty.Text = Convert.ToDouble(If(IsDBNull(row("TargetQuantity")), 1, row("TargetQuantity"))).ToString()
                    txtMatCost.Text = Convert.ToDouble(If(IsDBNull(row("DirectMaterialCost")), 0, row("DirectMaterialCost"))).ToString("N0")
                    txtLaborCost.Text = Convert.ToDouble(If(IsDBNull(row("DirectLaborCost")), 0, row("DirectLaborCost"))).ToString("N0")
                    txtOverheadCost.Text = Convert.ToDouble(If(IsDBNull(row("OverheadCost")), 0, row("OverheadCost"))).ToString("N0")
                    txtNotes.Text = Convert.ToString(row("Notes"))
                End If
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtProdName.Text) Then
                MessageBox.Show("لطفاً نام محصول را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim targetQty As Double = 1
            Double.TryParse(txtTargetQty.Text, targetQty)

            Dim matCost As Double = 0
            Double.TryParse(txtMatCost.Text.Replace(",", ""), matCost)

            Dim laborCost As Double = 0
            Double.TryParse(txtLaborCost.Text.Replace(",", ""), laborCost)

            Dim overheadCost As Double = 0
            Double.TryParse(txtOverheadCost.Text.Replace(",", ""), overheadCost)

            _prodSvc.SaveProductionOrder(
                _id, _companyID, txtOrderNo.Text, txtProdCode.Text,
                txtProdName.Text, targetQty, matCost, laborCost, overheadCost, txtNotes.Text
            )

            MessageBox.Show("کارت دستور تولید با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
