Imports System
Imports System.Windows.Forms

Namespace Negar.Forms
    Public Class BankTransactionEditForm
        Inherits Form

        Public Property TxDate As String
        Public Property RefNo As String
        Public Property Debit As Decimal
        Public Property Credit As Decimal
        Public Property Description As String
        Public Property Payee As String

        Public Sub New(txDateVal As String, refNoVal As String, debitVal As Decimal, creditVal As Decimal, descVal As String, payeeVal As String)
            InitializeComponent()

            ' Assign initial values
            Me.TxDate = txDateVal
            Me.RefNo = refNoVal
            Me.Debit = debitVal
            Me.Credit = creditVal
            Me.Description = descVal
            Me.Payee = payeeVal

            ' Populate textboxes
            txtTxDate.Text = Me.TxDate
            txtRefNo.Text = Me.RefNo
            txtDebit.Text = Me.Debit.ToString("0.##")
            txtCredit.Text = Me.Credit.ToString("0.##")
            txtDescription.Text = Me.Description
            txtPayee.Text = Me.Payee
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            ' Simple validation
            If String.IsNullOrWhiteSpace(txtTxDate.Text) Then
                MessageBox.Show("تاریخ تراکنش نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim debitVal As Decimal
            If Not Decimal.TryParse(txtDebit.Text.Trim(), debitVal) Then
                MessageBox.Show("مبلغ واریز نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim creditVal As Decimal
            If Not Decimal.TryParse(txtCredit.Text.Trim(), creditVal) Then
                MessageBox.Show("مبلغ برداشت نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Assign values back to properties
            Me.TxDate = txtTxDate.Text.Trim()
            Me.RefNo = txtRefNo.Text.Trim()
            Me.Debit = debitVal
            Me.Credit = creditVal
            Me.Description = txtDescription.Text.Trim()
            Me.Payee = txtPayee.Text.Trim()

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
