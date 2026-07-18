Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryVahedKala2Form
        Inherits Form

        Private ReadOnly _service As New UnitOfMeasureService()
        Private _selectedId As Integer? = Nothing
        Private _loading As Boolean = False

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(uomId As Integer)
            InitializeComponent()
            _selectedId = uomId
        End Sub

        Private Sub AnbardaryVahedKala2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            _loading = True
            ThemeHelper.ApplyFormTheme(Me)
            LoadCategories()

            If _selectedId.HasValue Then
                LoadUoMData(_selectedId.Value)
            Else
                chkIsReferenceUoM.Checked = False
                txtNumerator.Text = "1"
                txtDenominator.Text = "1"
            End If
            _loading = False
            UpdateExplanation()
        End Sub

        Private Sub LoadCategories()
            Try
                Dim dt = _service.GetCategories()
                cmbCategory.DataSource = dt
                cmbCategory.DisplayMember = "CategoryName"
                cmbCategory.ValueMember = "CategoryID"
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری دسته‌بندی‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadUoMData(uomId As Integer)
            Try
                Dim row = _service.GetById(uomId)
                If row IsNot Nothing Then
                    cmbCategory.SelectedValue = Convert.ToInt32(row("CategoryID"))
                    txtName.Text = Convert.ToString(row("UoMName"))
                    txtAbbreviation.Text = Convert.ToString(row("Abbreviation"))
                    chkIsReferenceUoM.Checked = Convert.ToBoolean(row("IsReferenceUoM"))
                    txtNumerator.Text = Convert.ToString(row("ConversionNumerator"))
                    txtDenominator.Text = Convert.ToString(row("ConversionDenominator"))
                    chkActive.Checked = Convert.ToBoolean(row("IsActive"))
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات واحد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
            Dim name = Microsoft.VisualBasic.Interaction.InputBox("لطفاً نام دسته‌بندی جدید را وارد کنید (مثال: وزن، تعداد):", "تعریف گروه واحد جدید", "")
            If String.IsNullOrWhiteSpace(name) Then Return

            Try
                Dim newId = _service.SaveCategory(Nothing, name.Trim())
                LoadCategories()
                cmbCategory.SelectedValue = newId
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره دسته‌بندی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub chkIsReferenceUoM_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsReferenceUoM.CheckedChanged
            Dim isRef = chkIsReferenceUoM.Checked
            txtNumerator.ReadOnly = isRef
            txtDenominator.ReadOnly = isRef
            If isRef Then
                txtNumerator.Text = "1"
                txtDenominator.Text = "1"
            End If
            UpdateExplanation()
        End Sub

        Private Sub cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategory.SelectedIndexChanged
            UpdateExplanation()
        End Sub

        Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
            UpdateExplanation()
        End Sub

        Private Sub txtNumerator_TextChanged(sender As Object, e As EventArgs) Handles txtNumerator.TextChanged
            UpdateExplanation()
        End Sub

        Private Sub txtDenominator_TextChanged(sender As Object, e As EventArgs) Handles txtDenominator.TextChanged
            UpdateExplanation()
        End Sub

        Private Sub UpdateExplanation()
            If _loading Then Return
            If cmbCategory.SelectedValue Is Nothing Then
                lblExplanation.Text = ""
                Return
            End If

            Dim catId = Convert.ToInt32(cmbCategory.SelectedValue)
            Dim refRow = _service.GetReferenceUoM(catId)

            Dim refName = "واحد پایه"
            If refRow IsNot Nothing Then
                refName = Convert.ToString(refRow("UoMName"))
            End If

            Dim unitName = If(String.IsNullOrWhiteSpace(txtName.Text), "واحد جدید", txtName.Text.Trim())

            If chkIsReferenceUoM.Checked Then
                lblExplanation.Text = $"«{unitName}» به عنوان واحد مرجع گروه انتخابی تعیین می‌شود."
            Else
                Dim num As Integer = 1
                Dim den As Integer = 1
                Integer.TryParse(txtNumerator.Text, num)
                Integer.TryParse(txtDenominator.Text, den)

                If num <= 0 Then num = 1
                If den <= 0 Then den = 1

                lblExplanation.Text = $"هر ۱ {unitName} معادل با ({num} / {den}) {refName} خواهد بود."
            End If
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("نام واحد اندازه‌گیری الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If

            If cmbCategory.SelectedValue Is Nothing Then
                MessageBox.Show("انتخاب دسته‌بندی الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCategory.Focus()
                Return
            End If

            Dim catId = Convert.ToInt32(cmbCategory.SelectedValue)

            Dim num As Integer = 1
            Dim den As Integer = 1
            If Not chkIsReferenceUoM.Checked Then
                If Not Integer.TryParse(txtNumerator.Text, num) OrElse num <= 0 Then
                    MessageBox.Show("صورت کسر ضریب تبدیل باید عددی بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtNumerator.Focus()
                    Return
                End If
                If Not Integer.TryParse(txtDenominator.Text, den) OrElse den <= 0 Then
                    MessageBox.Show("مخرج کسر ضریب تبدیل باید عددی بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtDenominator.Focus()
                    Return
                End If
            End If

            Try
                _service.Save(
                    _selectedId,
                    catId,
                    txtName.Text.Trim(),
                    txtAbbreviation.Text.Trim(),
                    chkIsReferenceUoM.Checked,
                    num,
                    den,
                    chkActive.Checked)

                MessageBox.Show("واحد اندازه‌گیری با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
