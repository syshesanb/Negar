Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniExpenseEditDialog
        Inherits Form

        Public Property SavedExpenseID As Integer? = Nothing
        Private ReadOnly _editId As Integer?

        Public Sub New(Optional editId As Integer? = Nothing)
            InitializeComponent()
            _editId = editId
        End Sub

        Private Sub AnbarMiniExpenseEditDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)

            cmbCategory.Items.Clear()
            cmbCategory.Items.AddRange(New Object() {
                "هزینه‌های جاری",
                "هزینه‌های اداری و عمومی",
                "هزینه اجاره",
                "حقوق و دستمزد",
                "هزینه حمل و نقل",
                "پذیرایی و ملزومات",
                "استهلاک و تعمیرات",
                "سایر هزینه‌ها"
            })
            cmbCategory.SelectedIndex = 0

            cmbPaymentMethod.Items.Clear()
            cmbPaymentMethod.Items.AddRange(New Object() {
                "کارت‌خوان / بانک",
                "نقد",
                "چک",
                "حواله / پایا"
            })
            cmbPaymentMethod.SelectedIndex = 0

            If _editId.HasValue Then
                Me.Text = "ویرایش سند هزینه"
                LoadExpenseData(_editId.Value)
            Else
                Me.Text = "ثبت هزینه جدید"
                txtDate.Text = PersianDateHelper.ToPersian(DateTime.Now)
            End If
        End Sub

        Private Sub LoadExpenseData(id As Integer)
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM Expenses WHERE ExpenseID = ?", id)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    txtDate.Text = Convert.ToString(row("ExpenseDate"))
                    txtTitle.Text = Convert.ToString(row("ExpenseTitle"))
                    
                    Dim cat = Convert.ToString(row("Category"))
                    If cmbCategory.Items.Contains(cat) Then
                        cmbCategory.SelectedItem = cat
                    Else
                        cmbCategory.Text = cat
                    End If

                    Dim amt = Convert.ToDecimal(row("Amount"))
                    txtAmount.Text = amt.ToString("N0")

                    txtPaidTo.Text = Convert.ToString(row("PaidTo"))
                    
                    Dim pm = Convert.ToString(row("PaymentMethod"))
                    If cmbPaymentMethod.Items.Contains(pm) Then
                        cmbPaymentMethod.SelectedItem = pm
                    End If

                    txtReferenceNo.Text = Convert.ToString(row("ReferenceNo"))
                    txtDescription.Text = Convert.ToString(row("Description"))
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات هزینه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private isFormattingAmount As Boolean = False
        Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged
            If isFormattingAmount Then Return
            Dim digitsOnly = System.Text.RegularExpressions.Regex.Replace(txtAmount.Text, "[^\d]", "")
            If String.IsNullOrEmpty(digitsOnly) Then
                txtAmount.Text = ""
                Return
            End If

            Dim val As Decimal
            If Decimal.TryParse(digitsOnly, val) Then
                isFormattingAmount = True
                txtAmount.Text = val.ToString("N0")
                txtAmount.SelectionStart = txtAmount.Text.Length
                isFormattingAmount = False
            End If
        End Sub

        Private Sub btnPickDate_Click(sender As Object, e As EventArgs) Handles btnPickDate.Click
            Using cal As New PersianCalendarForm()
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim title = txtTitle.Text.Trim()
            If String.IsNullOrEmpty(title) Then
                MessageBox.Show("لطفاً عنوان هزینه را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtTitle.Focus()
                Return
            End If

            Dim rawAmount = System.Text.RegularExpressions.Regex.Replace(txtAmount.Text, "[^\d]", "")
            Dim amt As Decimal = 0D
            Decimal.TryParse(rawAmount, amt)
            If amt <= 0 Then
                MessageBox.Show("لطفاً مبلغ معتبر برای هزینه وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAmount.Focus()
                Return
            End If

            Dim pDate = txtDate.Text.Trim()
            If String.IsNullOrEmpty(pDate) Then pDate = PersianDateHelper.ToPersian(DateTime.Now)

            Dim category = cmbCategory.Text.Trim()
            Dim paidTo = txtPaidTo.Text.Trim()
            Dim paymentMethod = cmbPaymentMethod.Text.Trim()
            Dim refNo = txtReferenceNo.Text.Trim()
            Dim desc = txtDescription.Text.Trim()

            Try
                If _editId.HasValue Then
                    Sql.ExecuteNonQuery(
                        "UPDATE Expenses SET ExpenseDate = ?, ExpenseTitle = ?, Category = ?, Amount = ?, PaidTo = ?, PaymentMethod = ?, ReferenceNo = ?, Description = ? WHERE ExpenseID = ?",
                        pDate, title, category, amt, paidTo, paymentMethod, refNo, desc, _editId.Value
                    )
                    SavedExpenseID = _editId.Value
                Else
                    Sql.ExecuteNonQuery(
                        "INSERT INTO Expenses (ExpenseDate, ExpenseTitle, Category, Amount, PaidTo, PaymentMethod, ReferenceNo, Description) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                        pDate, title, category, amt, paidTo, paymentMethod, refNo, desc
                    )
                    SavedExpenseID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))
                End If

                ' صدور/بروزرسانی خودکار سند حسابداری هزینه
                If SavedExpenseID.HasValue Then
                    InvoiceService.CreateOrUpdateAutoVoucherForExpense(SavedExpenseID.Value, pDate, title, category, amt, paidTo, paymentMethod, desc)
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی هزینه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
