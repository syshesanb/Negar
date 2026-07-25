Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryModyanCodes2Form
        Inherits Form

        Private ReadOnly _service As New ModyanCodeService()
        Private _codeId As Integer? = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(codeId As Integer)
            InitializeComponent()
            _codeId = codeId
        End Sub

        Private Sub AnbardaryModyanCodes2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            If _codeId.HasValue Then
                Me.Text = "ویرایش کد کالا و خدمات مودیان"
                LoadData(_codeId.Value)
            Else
                Me.Text = "ثبت کد جدید مودیان"
                txtModyanCode.Text = ""
                txtDescription.Text = ""
                txtCategoryName.Text = ""
                numTaxRate.Value = 0.10D
                chkActive.Checked = True
            End If
        End Sub

        Private Sub LoadData(codeId As Integer)
            Try
                Dim dt = Sql.ExecuteTable("SELECT CodeID, ModyanCode, Description, CategoryName, TaxRate, IsActive FROM ModyanCodes WHERE CodeID = ?", codeId)
                If dt.Rows.Count > 0 Then
                    Dim r = dt.Rows(0)
                    txtModyanCode.Text = Convert.ToString(r("ModyanCode"))
                    txtDescription.Text = Convert.ToString(r("Description"))
                    txtCategoryName.Text = Convert.ToString(r("CategoryName"))
                    numTaxRate.Value = Convert.ToDecimal(If(r.IsNull("TaxRate"), 0D, r("TaxRate")))
                    chkActive.Checked = Convert.ToBoolean(If(r.IsNull("IsActive"), False, r("IsActive")))
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End Try
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim codeStr = txtModyanCode.Text.Trim()
            Dim descStr = txtDescription.Text.Trim()
            Dim catStr = txtCategoryName.Text.Trim()

            If String.IsNullOrEmpty(codeStr) Then
                MessageBox.Show("لطفاً کد مودیان را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If String.IsNullOrEmpty(descStr) Then
                MessageBox.Show("لطفاً شرح کالا یا خدمت را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Try
                _service.SaveModyanCode(_codeId, codeStr, descStr, catStr, numTaxRate.Value, chkActive.Checked)
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره سازی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
