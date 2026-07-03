Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class BankReconciliationForm
        Private ReadOnly recService As New BankReconciliationService()
        Private _importedTable As DataTable
        Private _recResult As ReconciliationResult
        Private _selectedFilePath As String = ""

        Private Sub BankReconciliationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadBankAccounts()
            ClearMapping()
        End Sub

        Private Sub LoadBankAccounts()
            Try
                If Not SessionContext.CurrentCompanyID.HasValue Then Return

                Dim dt = Sql.ExecuteTable(
                    "SELECT AccountID, AccountCode, AccountName FROM ChartOfAccounts " &
                    "WHERE CompanyID = ? AND IsActive = 1 ORDER BY AccountCode",
                    SessionContext.CurrentCompanyID.Value)

                Dim comboItems As New List(Of ComboItem)()
                For Each row As DataRow In dt.Rows
                    Dim accId = Convert.ToInt32(row("AccountID"))
                    Dim accCode = Convert.ToString(row("AccountCode"))
                    Dim accName = Convert.ToString(row("AccountName"))
                    comboItems.Add(New ComboItem(accId, accCode & " - " & accName))
                Next

                cmbBankAccount.DataSource = comboItems
                cmbBankAccount.DisplayMember = "Text"
                cmbBankAccount.ValueMember = "ID"
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست بانک‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub ClearMapping()
            cmbColDate.Items.Clear()
            cmbColRef.Items.Clear()
            cmbColDebit.Items.Clear()
            cmbColCredit.Items.Clear()
            cmbColDesc.Items.Clear()
            grpColumns.Enabled = False
            btnCompare.Enabled = False
        End Sub

        Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "فایل‌های پشتیبانی شده (*.csv;*.xlsx;*.xls)|*.csv;*.xlsx;*.xls"
                ofd.Title = "انتخاب صورت‌حساب بانکی"
                If ofd.ShowDialog() = DialogResult.OK Then
                    _selectedFilePath = ofd.FileName
                    lblFilePath.Text = Path.GetFileName(_selectedFilePath)
                    LoadFileHeaders(_selectedFilePath)
                End If
            End Using
        End Sub

        Private Sub LoadFileHeaders(filePath As String)
            Try
                Cursor = Cursors.WaitCursor
                _importedTable = recService.ReadBankFile(filePath)
                Cursor = Cursors.Default

                If _importedTable Is Nothing OrElse _importedTable.Columns.Count = 0 Then
                    MessageBox.Show("فایل خالی است یا ستونی یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ClearMapping()
                    Return
                End If

                ' Fill ComboBoxes
                Dim cols As New List(Of String)()
                For Each col As DataColumn In _importedTable.Columns
                    cols.Add(col.ColumnName)
                Next

                PopulateCombo(cmbColDate, cols, "تاریخ")
                PopulateCombo(cmbColRef, cols, "پیگیری", "سند", "ارجاع")
                PopulateCombo(cmbColDebit, cols, "واریز", "بستانکار", "مبلغ")
                PopulateCombo(cmbColCredit, cols, "برداشت", "بدهکار", "مبلغ")
                PopulateCombo(cmbColDesc, cols, "شرح", "بابت", "توضیحات")

                grpColumns.Enabled = True
                btnCompare.Enabled = True
            Catch ex As Exception
                Cursor = Cursors.Default
                MessageBox.Show("خطا در خواندن فایل بانکی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ClearMapping()
            End Try
        End Sub

        Private Sub PopulateCombo(combo As ComboBox, items As List(Of String), ParamArray keywords() As String)
            combo.Items.Clear()
            combo.Items.Add("-- انتخاب کنید --")
            For Each item In items
                combo.Items.Add(item)
            Next

            combo.SelectedIndex = 0

            ' Try auto-detection
            For i As Integer = 0 To items.Count - 1
                Dim item = items(i).ToLower()
                For Each kw In keywords
                    If item.Contains(kw.ToLower()) Then
                        combo.SelectedIndex = i + 1
                        Exit Sub
                    End If
                Next
            Next
        End Sub

        Private Sub BtnCompare_Click(sender As Object, e As EventArgs) Handles btnCompare.Click
            If cmbBankAccount.SelectedValue Is Nothing Then
                MessageBox.Show("لطفاً ابتدا بانک مورد نظر را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _importedTable Is Nothing OrElse _importedTable.Rows.Count = 0 Then
                MessageBox.Show("اطلاعات فایل بانکی معتبر نیست.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim mapping As New ColumnMapping()
            mapping.DateIndex = cmbColDate.SelectedIndex - 1
            mapping.RefIndex = cmbColRef.SelectedIndex - 1
            mapping.DebitIndex = cmbColDebit.SelectedIndex - 1
            mapping.CreditIndex = cmbColCredit.SelectedIndex - 1
            mapping.DescIndex = cmbColDesc.SelectedIndex - 1

            If mapping.DateIndex < 0 OrElse (mapping.DebitIndex < 0 AndAlso mapping.CreditIndex < 0) Then
                MessageBox.Show("حداقل باید ستون‌های تاریخ و یکی از ستون‌های مبلغ (واریز یا برداشت) مشخص شوند.", "خطا در تناظر", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            Dim tempDate As DateTime
            If Not String.IsNullOrWhiteSpace(txtFromDate.Text) Then
                If recService.TryParsePersianOrEnglishDate(txtFromDate.Text, tempDate) Then
                    fromDate = tempDate
                Else
                    MessageBox.Show("تاریخ شروع معتبر نیست.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            If Not String.IsNullOrWhiteSpace(txtToDate.Text) Then
                If recService.TryParsePersianOrEnglishDate(txtToDate.Text, tempDate) Then
                    toDate = tempDate
                Else
                    MessageBox.Show("تاریخ پایان معتبر نیست.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            Try
                Cursor = Cursors.WaitCursor
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim fiscalYearId = SessionContext.CurrentFiscalYearID.Value
                Dim accountId = Convert.ToInt32(cmbBankAccount.SelectedValue)

                _recResult = recService.PerformReconciliation(companyId, fiscalYearId, accountId, fromDate, toDate, _importedTable, mapping)
                
                DisplayResults(mapping)
                Cursor = Cursors.Default
            Catch ex As Exception
                Cursor = Cursors.Default
                MessageBox.Show("خطا در انجام مقایسه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DisplayResults(mapping As ColumnMapping)
            ' Display Bank Discrepancies
            Dim dtBank As New DataTable()
            dtBank.Columns.Add("ردیف در فایل", GetType(Integer))
            dtBank.Columns.Add("تاریخ", GetType(String))
            dtBank.Columns.Add("شماره پیگیری", GetType(String))
            dtBank.Columns.Add("مبلغ واریز", GetType(Decimal))
            dtBank.Columns.Add("مبلغ برداشت", GetType(Decimal))
            dtBank.Columns.Add("شرح", GetType(String))

            For i As Integer = 0 To _recResult.UnmatchedBank.Count - 1
                Dim bt = _recResult.UnmatchedBank(i)
                dtBank.Rows.Add(i + 1, bt.TxDate, bt.RefNo, bt.Debit, bt.Credit, bt.Description)
            Next
            dgvBankDiscrepancies.DataSource = dtBank
            FormatGrid(dgvBankDiscrepancies)

            ' Display Ledger Discrepancies
            Dim dtLedger As New DataTable()
            dtLedger.Columns.Add("سند", GetType(String))
            dtLedger.Columns.Add("تاریخ سند", GetType(String))
            dtLedger.Columns.Add("مبلغ بدهکار (واریز ما)", GetType(Decimal))
            dtLedger.Columns.Add("مبلغ بستانکار (برداشت ما)", GetType(Decimal))
            dtLedger.Columns.Add("شماره پیگیری دفاتر", GetType(String))
            dtLedger.Columns.Add("شرح سند", GetType(String))

            For Each lt In _recResult.UnmatchedLedger
                dtLedger.Rows.Add(lt.RefNo, lt.EntryDate.ToString("yyyy/MM/dd"), lt.Debit, lt.Credit, lt.TxNo, lt.Description)
            Next
            dgvLedgerDiscrepancies.DataSource = dtLedger
            FormatGrid(dgvLedgerDiscrepancies)

            ' Display Matched
            Dim dtMatched As New DataTable()
            dtMatched.Columns.Add("تاریخ بانک", GetType(String))
            dtMatched.Columns.Add("تاریخ دفاتر", GetType(String))
            dtMatched.Columns.Add("شماره پیگیری بانک", GetType(String))
            dtMatched.Columns.Add("شماره پیگیری دفاتر", GetType(String))
            dtMatched.Columns.Add("مبلغ بانک", GetType(Decimal))
            dtMatched.Columns.Add("مبلغ دفاتر", GetType(Decimal))

            For Each pair In _recResult.Matched
                Dim bankAmt = If(pair.BankTx.Debit > 0, pair.BankTx.Debit, pair.BankTx.Credit)
                Dim ledgerAmt = If(pair.LedgerTx.Debit > 0, pair.LedgerTx.Debit, pair.LedgerTx.Credit)
                dtMatched.Rows.Add(pair.BankTx.TxDate, pair.LedgerTx.EntryDate.ToString("yyyy/MM/dd"), pair.BankTx.RefNo, pair.LedgerTx.TxNo, bankAmt, ledgerAmt)
            Next
            dgvMatched.DataSource = dtMatched
            FormatGrid(dgvMatched)

            ' Display Summary
            Dim totalMatched = _recResult.Matched.Count
            Dim totalUnmatchedBank = _recResult.UnmatchedBank.Count
            Dim totalUnmatchedLedger = _recResult.UnmatchedLedger.Count
            lblSummary.Text = String.Format("اقلام تطبیق یافته: {0} | اقلام باز بانکی: {1} | اقلام باز دفاتر: {2}", totalMatched, totalUnmatchedBank, totalUnmatchedLedger)
        End Sub

        Private Sub FormatGrid(dgv As DataGridView)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250)
            dgv.RowHeadersVisible = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.MultiSelect = False
            
            ' Format decimal columns
            For Each col As DataGridViewColumn In dgv.Columns
                If col.ValueType Is GetType(Decimal) Then
                    col.DefaultCellStyle.Format = "N0"
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
        End Sub

        Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
            If _recResult Is Nothing Then
                MessageBox.Show("هیچ نتیجه مغایرت‌گیری برای خروجی وجود ندارد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using sfd As New SaveFileDialog()
                sfd.Filter = "فایل CSV (*.csv)|*.csv"
                sfd.Title = "ذخیره اقلام مغایرت"
                sfd.FileName = "Reconciliation_Report_" & DateTime.Now.ToString("yyyyMMdd")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        ExportToCsv(sfd.FileName)
                        MessageBox.Show("گزارش مغایرت با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ذخیره‌سازی فایل خروجی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub ExportToCsv(filePath As String)
            Dim sb As New StringBuilder()

            ' 1. Write Header Info
            sb.AppendLine("گزارش مغایرت بانکی")
            sb.AppendLine("بانک انتخاب شده," & cmbBankAccount.Text)
            sb.AppendLine("تاریخ مغایرت‌گیری," & DateTime.Now.ToString("yyyy/MM/dd HH:mm"))
            sb.AppendLine()

            ' 2. Write Bank Discrepancies
            sb.AppendLine("--- اقلام باز بانکی (غایب در دفاتر) ---")
            sb.AppendLine("ردیف,تاریخ,شماره پیگیری,مبلغ واریز,مبلغ برداشت,شرح")
            For i As Integer = 0 To _recResult.UnmatchedBank.Count - 1
                Dim bt = _recResult.UnmatchedBank(i)
                sb.AppendLine(String.Format("{0},{1},{2},{3},{4},""{5}""", i + 1, bt.TxDate, bt.RefNo, bt.Debit, bt.Credit, bt.Description.Replace("""", """""")))
            Next
            sb.AppendLine()

            ' 3. Write Ledger Discrepancies
            sb.AppendLine("--- اقلام باز دفاتر (غایب در بانک) ---")
            sb.AppendLine("سند,تاریخ سند,بدهکار (واریز),بستانکار (برداشت),شماره پیگیری دفاتر,شرح سند")
            For Each lt In _recResult.UnmatchedLedger
                sb.AppendLine(String.Format("{0},{1},{2},{3},{4},""{5}""", lt.RefNo, lt.EntryDate.ToString("yyyy/MM/dd"), lt.Debit, lt.Credit, lt.TxNo, lt.Description.Replace("""", """""")))
            Next

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8)
        End Sub

        Private Class ComboItem
            Public Property ID As Integer
            Public Property Text As String

            Public Sub New(id As Integer, text As String)
                Me.ID = id
                Me.Text = text
            End Sub
        End Class
    End Class
End Namespace
