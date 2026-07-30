Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Budgeting
    Public Class BudgetEditDialog
        Inherits Form

        Private txtCostCenter As TextBox
        Private txtMoeinCode As TextBox
        Private txtTitle As TextBox
        Private txtAllocated As TextBox
        Private txtFiscalYear As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _budgetSvc As BudgetingService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _budgetSvc = New BudgetingService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_id <= 0, "📊 تعریف ردیف بودجه جدید", "📊 ویرایش ردیف بودجه مصوب")
            Me.Size = New Size(520, 380)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCenter As New Label With {.Text = "مرکز هزینه / واحد:", .Location = New Point(360, 25), .AutoSize = True}
            txtCostCenter = New TextBox With {.Location = New Point(30, 22), .Size = New Size(310, 26)}

            Dim lblCode As New Label With {.Text = "کد معین هزینه:", .Location = New Point(360, 65), .AutoSize = True}
            txtMoeinCode = New TextBox With {.Location = New Point(160, 62), .Size = New Size(180, 26)}

            Dim lblTitle As New Label With {.Text = "عنوان ردیف بودجه:", .Location = New Point(360, 105), .AutoSize = True}
            txtTitle = New TextBox With {.Location = New Point(30, 102), .Size = New Size(310, 26)}

            Dim lblAlloc As New Label With {.Text = "بودجه مصوب (ریال):", .Location = New Point(360, 145), .AutoSize = True}
            txtAllocated = New TextBox With {.Location = New Point(160, 142), .Size = New Size(180, 26), .Text = "100000000"}

            Dim lblYr As New Label With {.Text = "سال مالی:", .Location = New Point(360, 185), .AutoSize = True}
            txtFiscalYear = New TextBox With {.Location = New Point(160, 182), .Size = New Size(180, 26)}

            Dim lblNotes As New Label With {.Text = "توضیحات:", .Location = New Point(360, 225), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(310, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(230, 280),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(120, 280),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCenter, txtCostCenter, lblCode, txtMoeinCode, lblTitle, txtTitle,
                lblAlloc, txtAllocated, lblYr, txtFiscalYear, lblNotes, txtNotes,
                btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            If _id > 0 Then
                Dim row = _budgetSvc.GetBudgetItemById(_id)
                If row IsNot Nothing Then
                    txtCostCenter.Text = Convert.ToString(row("CostCenter"))
                    txtMoeinCode.Text = Convert.ToString(row("MoeinCode"))
                    txtTitle.Text = Convert.ToString(row("ItemTitle"))
                    txtAllocated.Text = Convert.ToDouble(If(IsDBNull(row("AllocatedBudget")), 0, row("AllocatedBudget"))).ToString("N0")
                    txtFiscalYear.Text = Convert.ToString(row("FiscalYear"))
                    txtNotes.Text = Convert.ToString(row("Notes"))
                End If
            Else
                Dim yr = SessionContext.CurrentFiscalYearName
                If String.IsNullOrWhiteSpace(yr) Then yr = "1405"
                txtFiscalYear.Text = yr
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtCostCenter.Text) OrElse String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("لطفاً مرکز هزینه و عنوان ردیف بودجه را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim alloc As Double = 0
            Double.TryParse(txtAllocated.Text.Replace(",", ""), alloc)

            _budgetSvc.SaveBudgetItem(
                _id, _companyID, txtCostCenter.Text, txtMoeinCode.Text,
                txtTitle.Text, alloc, txtFiscalYear.Text, txtNotes.Text
            )

            MessageBox.Show("ردیف بودجه جدید با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
