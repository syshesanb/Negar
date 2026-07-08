Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Partial Public Class HesabdaryMogBankForm
        Private ReadOnly recService As New BankReconciliationService()
        Private _rawImportedTable As DataTable
        Private _recResult As ReconciliationResult
        Private _selectedFilePath As String = ""
        Private _selectedBankID As Integer = 0
        Private ReadOnly _searchTextBoxes As New List(Of TextBox)()

        Private Sub HesabdaryMogBankForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadAccountCodingCombo()
            LoadBankList()
            LoadBankCombos()
            ClearBankInputs()
            ClearMapping()
        End Sub

        Private Sub tcMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcMain.SelectedIndexChanged
            If tcMain.SelectedTab Is tpIntroBanks Then
                LoadAccountCodingCombo()
            End If
        End Sub

        ' ==========================================
        ' TAB 1: معرفی بانک‌ها (CRUD)
        ' ==========================================

        Private Sub LoadAccountCodingCombo()
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

                cmbAccountCoding.DataSource = comboItems
                cmbAccountCoding.DisplayMember = "Text"
                cmbAccountCoding.ValueMember = "ID"
                cmbAccountCoding.SelectedIndex = -1
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری سرفصل‌های حسابداری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadBankList()
            Try
                If Not SessionContext.CurrentCompanyID.HasValue Then Return
                Dim dt = Sql.ExecuteTable(
                    "SELECT b.BankID, b.BankName, b.BranchName, b.BranchCode, b.BranchAddress, b.AccountType, b.AccountNumber, b.AccountID, " &
                    "c.AccountCode || ' - ' || c.AccountName As AccountMapping " &
                    "FROM SoBank_1 b " &
                    "LEFT JOIN ChartOfAccounts c ON b.AccountID = c.AccountID " &
                    "WHERE b.CompanyID = ? ORDER BY b.BankID DESC",
                    SessionContext.CurrentCompanyID.Value)

                dgvBanks.DataSource = dt

                ' Column headers formatting
                If dgvBanks.Columns.Contains("BankID") Then dgvBanks.Columns("BankID").Visible = False
                If dgvBanks.Columns.Contains("AccountID") Then dgvBanks.Columns("AccountID").Visible = False
                If dgvBanks.Columns.Contains("BankName") Then dgvBanks.Columns("BankName").HeaderText = "نام بانک"
                If dgvBanks.Columns.Contains("BranchName") Then dgvBanks.Columns("BranchName").HeaderText = "نام شعبه"
                If dgvBanks.Columns.Contains("BranchCode") Then dgvBanks.Columns("BranchCode").HeaderText = "کد شعبه"
                If dgvBanks.Columns.Contains("BranchAddress") Then dgvBanks.Columns("BranchAddress").HeaderText = "آدرس شعبه"
                If dgvBanks.Columns.Contains("AccountType") Then dgvBanks.Columns("AccountType").HeaderText = "نوع حساب"
                If dgvBanks.Columns.Contains("AccountNumber") Then dgvBanks.Columns("AccountNumber").HeaderText = "شماره حساب"
                If dgvBanks.Columns.Contains("AccountMapping") Then dgvBanks.Columns("AccountMapping").HeaderText = "سرفصل متناظر"

                FormatGrid(dgvBanks)
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست بانک‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadBankCombos()
            Try
                If Not SessionContext.CurrentCompanyID.HasValue Then Return
                Dim dt = Sql.ExecuteTable(
                    "SELECT BankID, BankName, AccountNumber FROM SoBank_1 " &
                    "WHERE CompanyID = ? ORDER BY BankName",
                    SessionContext.CurrentCompanyID.Value)

                Dim comboItemsImport As New List(Of ComboItem)()
                Dim comboItemsRec As New List(Of ComboItem)()
                For Each row As DataRow In dt.Rows
                    Dim bankId = Convert.ToInt32(row("BankID"))
                    Dim bankName = Convert.ToString(row("BankName"))
                    Dim accNo = Convert.ToString(row("AccountNumber"))
                    Dim displayText = bankName & " - " & accNo
                    comboItemsImport.Add(New ComboItem(bankId, displayText))
                    comboItemsRec.Add(New ComboItem(bankId, displayText))
                Next

                ' Tab 2 combo
                cmbImportBank.DataSource = Nothing
                If comboItemsImport.Count > 0 Then
                    cmbImportBank.DataSource = comboItemsImport
                    cmbImportBank.DisplayMember = "Text"
                    cmbImportBank.ValueMember = "ID"
                End If

                ' Tab 3 combo
                cmbRecBank.DataSource = Nothing
                If comboItemsRec.Count > 0 Then
                    cmbRecBank.DataSource = comboItemsRec
                    cmbRecBank.DisplayMember = "Text"
                    cmbRecBank.ValueMember = "ID"
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری کامبوباکس بانک‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub cmbImportBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbImportBank.SelectedIndexChanged
            LoadImportedTransactions()
        End Sub

        Private Sub LoadImportedTransactions()
            If cmbImportBank.SelectedValue Is Nothing Then
                dgvImportPreview.DataSource = Nothing
                ClearSearchTextBoxes()
                Return
            End If

            Dim bankId As Integer
            If Not Integer.TryParse(Convert.ToString(cmbImportBank.SelectedValue), bankId) Then
                Return
            End If

            If bankId <= 0 Then
                dgvImportPreview.DataSource = Nothing
                ClearSearchTextBoxes()
                Return
            End If

            Try
                ' Load transactions from SoBank_2
                Dim dt = Sql.ExecuteTable(
                    "SELECT TxID, TxDate, RefNo, Debit, Credit, Description, Payee FROM SoBank_2 WHERE BankID = ? ORDER BY TxID DESC",
                    bankId)

                ' Bind to DataGridView
                dgvImportPreview.DataSource = dt

                ' Setup Grid headers and button columns
                SetupImportedGridView()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری صورت‌حساب‌های بانکی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub SetupImportedGridView()
            RemoveButtonColumns()

            If dgvImportPreview.Columns.Contains("TxID") Then dgvImportPreview.Columns("TxID").Visible = False

            If dgvImportPreview.Columns.Contains("TxDate") Then dgvImportPreview.Columns("TxDate").HeaderText = "تاریخ"
            If dgvImportPreview.Columns.Contains("RefNo") Then dgvImportPreview.Columns("RefNo").HeaderText = "شماره ارجاع/پیگیری"
            If dgvImportPreview.Columns.Contains("Debit") Then
                dgvImportPreview.Columns("Debit").HeaderText = "مبلغ واریز"
                dgvImportPreview.Columns("Debit").DefaultCellStyle.Format = "N0"
            End If
            If dgvImportPreview.Columns.Contains("Credit") Then
                dgvImportPreview.Columns("Credit").HeaderText = "مبلغ برداشت"
                dgvImportPreview.Columns("Credit").DefaultCellStyle.Format = "N0"
            End If
            If dgvImportPreview.Columns.Contains("Description") Then dgvImportPreview.Columns("Description").HeaderText = "شرح"
            If dgvImportPreview.Columns.Contains("Payee") Then dgvImportPreview.Columns("Payee").HeaderText = "واریزکننده/ذینفع"

            ' Add Edit button
            Dim btnEdit As New DataGridViewButtonColumn()
            btnEdit.Name = "btnEditCol"
            btnEdit.HeaderText = "ویرایش"
            btnEdit.Text = "ویرایش"
            btnEdit.UseColumnTextForButtonValue = True
            btnEdit.Width = 70
            dgvImportPreview.Columns.Add(btnEdit)

            ' Add Delete button
            Dim btnDelete As New DataGridViewButtonColumn()
            btnDelete.Name = "btnDeleteCol"
            btnDelete.HeaderText = "حذف"
            btnDelete.Text = "حذف"
            btnDelete.UseColumnTextForButtonValue = True
            btnDelete.Width = 70
            dgvImportPreview.Columns.Add(btnDelete)

            ' Set DisplayIndex explicitly to show Edit first, then Delete, then Date and other columns
            dgvImportPreview.Columns("btnEditCol").DisplayIndex = 0
            dgvImportPreview.Columns("btnDeleteCol").DisplayIndex = 1

            FormatGrid(dgvImportPreview)
            CreateSearchTextBoxes()
        End Sub

        Private Sub RemoveButtonColumns()
            If dgvImportPreview.Columns.Contains("btnEditCol") Then
                dgvImportPreview.Columns.Remove("btnEditCol")
            End If
            If dgvImportPreview.Columns.Contains("btnDeleteCol") Then
                dgvImportPreview.Columns.Remove("btnDeleteCol")
            End If
        End Sub

        Private Sub ClearSearchTextBoxes()
            For Each txt In _searchTextBoxes
                RemoveHandler txt.TextChanged, AddressOf SearchTextBox_TextChanged
                pnlSearchFilters.Controls.Remove(txt)
                txt.Dispose()
            Next
            _searchTextBoxes.Clear()
        End Sub

        Private Sub CreateSearchTextBoxes()
            ClearSearchTextBoxes()

            If dgvImportPreview.DataSource Is Nothing Then Return

            For Each col As DataGridViewColumn In dgvImportPreview.Columns
                If Not col.Visible Then Continue For

                Dim txt As New TextBox()
                txt.Name = "txtSearch_" & col.Index
                txt.Tag = col.Index
                txt.Font = New System.Drawing.Font("Tahoma", 8.25!)

                If col.DataPropertyName <> "" Then
                    txt.Text = ""
                Else
                    txt.Enabled = False
                    txt.BackColor = System.Drawing.Color.LightGray
                End If

                AddHandler txt.TextChanged, AddressOf SearchTextBox_TextChanged
                pnlSearchFilters.Controls.Add(txt)
                _searchTextBoxes.Add(txt)
            Next

            UpdateSearchTextBoxPositions()
        End Sub

        Private Sub UpdateSearchTextBoxPositions()
            If _searchTextBoxes.Count = 0 Then Return

            pnlSearchFilters.SuspendLayout()
            For Each txt In _searchTextBoxes
                Dim colIndex = Convert.ToInt32(txt.Tag)
                If colIndex >= dgvImportPreview.Columns.Count Then Continue For

                Dim col = dgvImportPreview.Columns(colIndex)
                Dim rect = dgvImportPreview.GetCellDisplayRectangle(colIndex, -1, True)

                txt.Left = rect.Left + dgvImportPreview.Left - pnlSearchFilters.Left
                txt.Width = rect.Width
                txt.Top = 2
                txt.Height = pnlSearchFilters.Height - 4
                txt.Visible = col.Visible AndAlso rect.Width > 0
            Next
            pnlSearchFilters.ResumeLayout()
        End Sub

        Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)
            ApplySearchFilters()
        End Sub

        Private Sub ApplySearchFilters()
            Dim dt = TryCast(dgvImportPreview.DataSource, DataTable)
            If dt Is Nothing Then Return

            Dim filterParts As New List(Of String)()
            For Each txt In _searchTextBoxes
                Dim colIndex = Convert.ToInt32(txt.Tag)
                If colIndex >= dgvImportPreview.Columns.Count Then Continue For

                Dim col = dgvImportPreview.Columns(colIndex)
                If col.DataPropertyName = "" Then Continue For

                If Not String.IsNullOrEmpty(txt.Text) Then
                    Dim cleanText = txt.Text.Replace("'", "''").Trim()
                    filterParts.Add(String.Format("Convert({0}, 'System.String') LIKE '%{1}%'", col.DataPropertyName, cleanText))
                End If
            Next

            Dim rowFilter = String.Join(" AND ", filterParts)
            dt.DefaultView.RowFilter = rowFilter
        End Sub

        Private Sub dgvImportPreview_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvImportPreview.Scroll
            UpdateSearchTextBoxPositions()
        End Sub

        Private Sub dgvImportPreview_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvImportPreview.ColumnWidthChanged
            UpdateSearchTextBoxPositions()
        End Sub

        Private Sub dgvImportPreview_Resize(sender As Object, e As EventArgs) Handles dgvImportPreview.Resize
            UpdateSearchTextBoxPositions()
        End Sub

        Private Sub dgvImportPreview_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvImportPreview.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim senderGrid = DirectCast(sender, DataGridView)
            Dim colName = senderGrid.Columns(e.ColumnIndex).Name

            If colName = "btnEditCol" Then
                Dim txId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("TxID").Value)
                Dim txDate = Convert.ToString(senderGrid.Rows(e.RowIndex).Cells("TxDate").Value)
                Dim refNo = Convert.ToString(senderGrid.Rows(e.RowIndex).Cells("RefNo").Value)
                Dim debit = Convert.ToDecimal(senderGrid.Rows(e.RowIndex).Cells("Debit").Value)
                Dim credit = Convert.ToDecimal(senderGrid.Rows(e.RowIndex).Cells("Credit").Value)
                Dim desc = Convert.ToString(senderGrid.Rows(e.RowIndex).Cells("Description").Value)
                Dim payee = Convert.ToString(senderGrid.Rows(e.RowIndex).Cells("Payee").Value)

                Using editForm As New BankTransactionEditForm(txDate, refNo, debit, credit, desc, payee)
                    If editForm.ShowDialog() = DialogResult.OK Then
                        Try
                            Sql.ExecuteNonQuery(
                                "UPDATE SoBank_2 SET TxDate = ?, RefNo = ?, Debit = ?, Credit = ?, Description = ?, Payee = ? WHERE TxID = ?",
                                editForm.TxDate, editForm.RefNo, editForm.Debit, editForm.Credit, editForm.Description, editForm.Payee, txId)
                            MessageBox.Show("تراکنش با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadImportedTransactions()
                        Catch ex As Exception
                            MessageBox.Show("خطا در ویرایش تراکنش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End Using

            ElseIf colName = "btnDeleteCol" Then
                Dim txId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("TxID").Value)
                Dim confirm = MessageBox.Show("آیا از حذف این تراکنش اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirm = DialogResult.Yes Then
                    Try
                        Sql.ExecuteNonQuery("DELETE FROM SoBank_2 WHERE TxID = ?", txId)
                        MessageBox.Show("تراکنش با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadImportedTransactions()
                    Catch ex As Exception
                        MessageBox.Show("خطا در حذف تراکنش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub dgvBanks_SelectionChanged(sender As Object, e As EventArgs) Handles dgvBanks.SelectionChanged
            If dgvBanks.SelectedRows.Count > 0 Then
                Dim row = dgvBanks.SelectedRows(0)
                _selectedBankID = Convert.ToInt32(row.Cells("BankID").Value)
                txtBankName.Text = Convert.ToString(row.Cells("BankName").Value)
                txtBranchName.Text = Convert.ToString(row.Cells("BranchName").Value)
                txtBranchCode.Text = Convert.ToString(row.Cells("BranchCode").Value)
                txtBranchAddress.Text = Convert.ToString(row.Cells("BranchAddress").Value)
                txtAccountType.Text = Convert.ToString(row.Cells("AccountType").Value)
                txtAccountNumber.Text = Convert.ToString(row.Cells("AccountNumber").Value)

                If Not row.Cells("AccountID").Value Is DBNull.Value Then
                    cmbAccountCoding.SelectedValue = Convert.ToInt32(row.Cells("AccountID").Value)
                Else
                    cmbAccountCoding.SelectedIndex = -1
                End If
            End If
        End Sub

        Private Sub btnSaveBank_Click(sender As Object, e As EventArgs) Handles btnSaveBank.Click
            If String.IsNullOrWhiteSpace(txtBankName.Text) OrElse String.IsNullOrWhiteSpace(txtAccountNumber.Text) Then
                MessageBox.Show("نام بانک و شماره حساب الزامی هستند.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If cmbAccountCoding.SelectedValue Is Nothing Then
                MessageBox.Show("انتخاب سرفصل حساب الزامی است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim accountId = Convert.ToInt32(cmbAccountCoding.SelectedValue)

                If _selectedBankID = 0 Then
                    ' Insert
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SoBank_1 (CompanyID, BankName, BranchName, BranchCode, BranchAddress, AccountType, AccountNumber, AccountID) " &
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                        companyId, txtBankName.Text.Trim(), txtBranchName.Text.Trim(), txtBranchCode.Text.Trim(), txtBranchAddress.Text.Trim(),
                        txtAccountType.Text.Trim(), txtAccountNumber.Text.Trim(), accountId)
                    MessageBox.Show("مشخصات بانک با موفقیت ثبت شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' Update
                    Sql.ExecuteNonQuery(
                        "UPDATE SoBank_1 SET BankName = ?, BranchName = ?, BranchCode = ?, BranchAddress = ?, AccountType = ?, AccountNumber = ?, AccountID = ? " &
                        "WHERE BankID = ?",
                        txtBankName.Text.Trim(), txtBranchName.Text.Trim(), txtBranchCode.Text.Trim(), txtBranchAddress.Text.Trim(),
                        txtAccountType.Text.Trim(), txtAccountNumber.Text.Trim(), accountId, _selectedBankID)
                    MessageBox.Show("مشخصات بانک با موفقیت ویرایش شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ClearBankInputs()
                LoadBankList()
                LoadBankCombos()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی مشخصات بانک: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnDeleteBank_Click(sender As Object, e As EventArgs) Handles btnDeleteBank.Click
            If _selectedBankID = 0 Then
                MessageBox.Show("لطفاً یک بانک را برای حذف انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim confirm = MessageBox.Show("آیا از حذف این بانک و کلیه صورت‌حساب‌های مرتبط با آن اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    Sql.ExecuteNonQuery("DELETE FROM SoBank_1 WHERE BankID = ?", _selectedBankID)
                    MessageBox.Show("بانک با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearBankInputs()
                    LoadBankList()
                    LoadBankCombos()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف بانک: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub btnNewBank_Click(sender As Object, e As EventArgs) Handles btnNewBank.Click
            ClearBankInputs()
        End Sub

        Private Sub ClearBankInputs()
            _selectedBankID = 0
            txtBankName.Clear()
            txtBranchName.Clear()
            txtBranchCode.Clear()
            txtBranchAddress.Clear()
            txtAccountType.Clear()
            txtAccountNumber.Clear()
            cmbAccountCoding.SelectedIndex = -1

            If dgvBanks.SelectedRows.Count > 0 Then
                dgvBanks.ClearSelection()
            End If
        End Sub


        ' ==========================================
        ' TAB 2: ورود صورت‌حساب بانک (Spreadsheet Import)
        ' ==========================================

        Private Sub ClearMapping()
            cmbColDate.Items.Clear()
            cmbColRef.Items.Clear()
            cmbColDebit.Items.Clear()
            cmbColCredit.Items.Clear()
            cmbColDesc.Items.Clear()
            cmbColPayee.Items.Clear()
        End Sub

        Private Sub btnBrowseFile_Click(sender As Object, e As EventArgs) Handles btnBrowseFile.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "فایل‌های پشتیبانی شده (*.csv;*.xlsx;*.xls)|*.csv;*.xlsx;*.xls"
                ofd.Title = "انتخاب صورت‌حساب بانکی"
                If ofd.ShowDialog() = DialogResult.OK Then
                    _selectedFilePath = ofd.FileName
                    lblImportFilePath.Text = Path.GetFileName(_selectedFilePath)
                    Try
                        Me.Cursor = Cursors.WaitCursor
                        RemoveButtonColumns()
                        ClearSearchTextBoxes()
                        _rawImportedTable = recService.ReadBankFileRaw(_selectedFilePath)
                        dgvImportPreview.DataSource = _rawImportedTable
                        Me.Cursor = Cursors.Default

                        ' Reset NumericUpDown to 1
                        nudHeaderRow.Value = 1
                        ' Refresh Column Mappings
                        RefreshColumnMappings()
                    Catch ex As Exception
                        Me.Cursor = Cursors.Default
                        MessageBox.Show("خطا در بارگذاری فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub nudHeaderRow_ValueChanged(sender As Object, e As EventArgs) Handles nudHeaderRow.ValueChanged
            RefreshColumnMappings()
        End Sub

        Private Sub RefreshColumnMappings()
            If _rawImportedTable Is Nothing OrElse _rawImportedTable.Rows.Count = 0 Then Return

            Dim headerRowIdx = Convert.ToInt32(nudHeaderRow.Value) - 1
            If headerRowIdx < 0 OrElse headerRowIdx >= _rawImportedTable.Rows.Count Then
                MessageBox.Show("شماره ردیف سرستون نامعتبر است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim cols As New List(Of String)()
            Dim headerRow = _rawImportedTable.Rows(headerRowIdx)
            For i As Integer = 0 To _rawImportedTable.Columns.Count - 1
                Dim colVal = Convert.ToString(headerRow(i)).Trim()
                If String.IsNullOrEmpty(colVal) Then
                    colVal = "Column" & (i + 1)
                End If
                cols.Add(colVal)
            Next

            PopulateCombo(cmbColDate, cols, "تاریخ", "date")
            PopulateCombo(cmbColRef, cols, "پیگیری", "سند", "ارجاع", "ref")
            PopulateCombo(cmbColDebit, cols, "واریز", "بستانکار", "مبلغ", "debit")
            PopulateCombo(cmbColCredit, cols, "برداشت", "بدهکار", "credit")
            PopulateCombo(cmbColDesc, cols, "شرح", "بابت", "توضیحات", "desc")
            PopulateCombo(cmbColPayee, cols, "واریز کننده", "ذینفع", "payee", "beneficiary")
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

        Private Sub btnSaveImport_Click(sender As Object, e As EventArgs) Handles btnSaveImport.Click
            If cmbImportBank.SelectedValue Is Nothing Then
                MessageBox.Show("لطفاً ابتدا بانک مورد نظر را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _rawImportedTable Is Nothing OrElse _rawImportedTable.Rows.Count = 0 Then
                MessageBox.Show("هیچ فایلی بارگذاری نشده است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim dateIdx = cmbColDate.SelectedIndex - 1
            Dim refIdx = cmbColRef.SelectedIndex - 1
            Dim debitIdx = cmbColDebit.SelectedIndex - 1
            Dim creditIdx = cmbColCredit.SelectedIndex - 1
            Dim descIdx = cmbColDesc.SelectedIndex - 1
            Dim payeeIdx = cmbColPayee.SelectedIndex - 1

            If dateIdx < 0 OrElse (debitIdx < 0 AndAlso creditIdx < 0) Then
                MessageBox.Show("حداقل باید ستون‌های تاریخ و یکی از ستون‌های مبلغ (واریز یا برداشت) متناظر شوند.", "خطا در تناظر", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim headerRowIdx = Convert.ToInt32(nudHeaderRow.Value) - 1
            If headerRowIdx < 0 OrElse headerRowIdx >= _rawImportedTable.Rows.Count Then
                MessageBox.Show("ردیف سرستون نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim bankId = Convert.ToInt32(cmbImportBank.SelectedValue)
            Dim insertedCount = 0
            Dim duplicateCount = 0

            Try
                Me.Cursor = Cursors.WaitCursor

                ' Loop through data rows (all rows after the header row)
                For rIdx As Integer = headerRowIdx + 1 To _rawImportedTable.Rows.Count - 1
                    Dim row = _rawImportedTable.Rows(rIdx)

                    ' Read values
                    Dim txDate = Convert.ToString(row(dateIdx)).Trim()
                    If String.IsNullOrWhiteSpace(txDate) Then Continue For ' Skip empty date lines

                    Dim refNo = ""
                    If refIdx >= 0 Then refNo = Convert.ToString(row(refIdx)).Trim()

                    Dim debit As Decimal = 0D
                    If debitIdx >= 0 Then
                        Dim val = Convert.ToString(row(debitIdx)).Trim()
                        Decimal.TryParse(val, debit)
                    End If

                    Dim credit As Decimal = 0D
                    If creditIdx >= 0 Then
                        Dim val = Convert.ToString(row(creditIdx)).Trim()
                        Decimal.TryParse(val, credit)
                    End If

                    Dim description = ""
                    If descIdx >= 0 Then description = Convert.ToString(row(descIdx)).Trim()

                    Dim payee = ""
                    If payeeIdx >= 0 Then payee = Convert.ToString(row(payeeIdx)).Trim()

                    ' Duplicate Check: Check if this record already exists for the same bank
                    Dim existsQuery = "SELECT COUNT(*) FROM SoBank_2 WHERE BankID = ? AND TxDate = ? AND " &
                                      "COALESCE(RefNo, '') = ? AND Debit = ? AND Credit = ? AND COALESCE(Description, '') = ? AND COALESCE(Payee, '') = ?"
                    Dim count = Convert.ToInt32(Sql.ExecuteScalar(existsQuery, bankId, txDate, If(refNo, ""), debit, credit, If(description, ""), If(payee, "")))

                    If count = 0 Then
                        ' Insert new record
                        Sql.ExecuteNonQuery(
                            "INSERT INTO SoBank_2 (BankID, TxDate, RefNo, Debit, Credit, Description, Payee) " &
                            "VALUES (?, ?, ?, ?, ?, ?, ?)",
                            bankId, txDate, refNo, debit, credit, description, payee)
                        insertedCount += 1
                    Else
                        duplicateCount += 1
                    End If
                Next

                Me.Cursor = Cursors.Default
                MessageBox.Show(String.Format("تعداد {0} تراکنش با موفقیت ذخیره شد.{1}تعداد {2} تراکنش تکراری نادیده گرفته شد.", insertedCount, Environment.NewLine, duplicateCount), "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Reset preview
                _rawImportedTable = Nothing
                _selectedFilePath = ""
                lblImportFilePath.Text = "فایلی انتخاب نشده است"
                ClearMapping()
                LoadImportedTransactions()
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                MessageBox.Show("خطا در ذخیره‌سازی اطلاعات صورت‌حساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub


        ' ==========================================
        ' TAB 3: مغایرات بانکی (Reconciliation)
        ' ==========================================

        Private Sub rbCustomRange_CheckedChanged(sender As Object, e As EventArgs) Handles rbCustomRange.CheckedChanged
            txtFromDate.Enabled = rbCustomRange.Checked
            txtToDate.Enabled = rbCustomRange.Checked
            If Not rbCustomRange.Checked Then
                txtFromDate.Clear()
                txtToDate.Clear()
            End If
        End Sub

        Private Sub btnRunReconciliation_Click(sender As Object, e As EventArgs) Handles btnRunReconciliation.Click
            If cmbRecBank.SelectedValue Is Nothing Then
                MessageBox.Show("لطفاً ابتدا بانک مورد نظر را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim bankId = Convert.ToInt32(cmbRecBank.SelectedValue)

            ' Fetch bank AccountID mapping
            Dim accRow = Sql.ExecuteTable("SELECT AccountID FROM SoBank_1 WHERE BankID = ?", bankId)
            If accRow.Rows.Count = 0 OrElse accRow.Rows(0)("AccountID") Is DBNull.Value Then
                MessageBox.Show("برای این بانک سرفصل حساب تعیین نشده است. لطفاً ابتدا در تب معرفی بانک‌ها، سرفصل آن را تعیین کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim accountId = Convert.ToInt32(accRow.Rows(0)("AccountID"))

            ' Determine date range
            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            If rbCurrentYear.Checked Then
                If Not SessionContext.CurrentFiscalYearID.HasValue Then
                    MessageBox.Show("سال مالی جاری انتخاب نشده است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim dtYear = Sql.ExecuteTable("SELECT StartDate, EndDate FROM FiscalYears WHERE FiscalYearID = ?", SessionContext.CurrentFiscalYearID.Value)
                If dtYear.Rows.Count > 0 Then
                    If Not dtYear.Rows(0).IsNull("StartDate") Then fromDate = Convert.ToDateTime(dtYear.Rows(0)("StartDate"))
                    If Not dtYear.Rows(0).IsNull("EndDate") Then toDate = Convert.ToDateTime(dtYear.Rows(0)("EndDate"))
                End If
            ElseIf rbCustomRange.Checked Then
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
            End If

            Try
                Me.Cursor = Cursors.WaitCursor
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim fiscalYearId = SessionContext.CurrentFiscalYearID.Value

                ' Perform reconciliation using DB stored statement data
                _recResult = recService.PerformDatabaseReconciliation(companyId, fiscalYearId, bankId, accountId, fromDate, toDate)

                DisplayReconciliationResults(bankId, accountId, fromDate, toDate)
                Me.Cursor = Cursors.Default
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                MessageBox.Show("خطا در انجام مغایرت‌گیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DisplayReconciliationResults(bankId As Integer, accountId As Integer, fromDate As DateTime?, toDate As DateTime?)
            If _recResult Is Nothing Then Return

            ' --- pnlBank (Top Panel) ---
            ' 1. کل ارقام صورتحساب
            Dim dtBankAll As New DataTable()
            dtBankAll.Columns.Add("ردیف", GetType(Integer))
            dtBankAll.Columns.Add("تاریخ", GetType(String))
            dtBankAll.Columns.Add("شماره پیگیری", GetType(String))
            dtBankAll.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankAll.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankAll.Columns.Add("شرح", GetType(String))
            dtBankAll.Columns.Add("واریز کننده/ذینفع", GetType(String))

            Dim bankTransactionsAll = New List(Of BankTransaction)()
            Dim dtBankDb = Sql.ExecuteTable("SELECT TxDate, RefNo, Debit, Credit, Description, Payee FROM SoBank_2 WHERE BankID = ?", bankId)
            For Each row As DataRow In dtBankDb.Rows
                Dim bt As New BankTransaction()
                bt.TxDate = Convert.ToString(row("TxDate"))
                bt.RefNo = Convert.ToString(row("RefNo"))
                bt.Debit = If(row.IsNull("Debit"), 0D, Convert.ToDecimal(row("Debit")))
                bt.Credit = If(row.IsNull("Credit"), 0D, Convert.ToDecimal(row("Credit")))
                bt.Description = Convert.ToString(row("Description"))
                bt.Payee = Convert.ToString(row("Payee"))

                Dim include = True
                If fromDate.HasValue OrElse toDate.HasValue Then
                    Dim d As DateTime
                    If recService.TryParsePersianOrEnglishDate(bt.TxDate, d) Then
                        If fromDate.HasValue AndAlso d.Date < fromDate.Value.Date Then include = False
                        If toDate.HasValue AndAlso d.Date > toDate.Value.Date Then include = False
                    End If
                End If
                If include Then bankTransactionsAll.Add(bt)
            Next

            For i As Integer = 0 To bankTransactionsAll.Count - 1
                Dim bt = bankTransactionsAll(i)
                dtBankAll.Rows.Add(i + 1, bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee)
            Next
            dgvBank_All.DataSource = dtBankAll
            FormatGrid(dgvBank_All)

            ' 2. کل ارقام باز صورتحساب
            Dim dtBankOpen As New DataTable()
            dtBankOpen.Columns.Add("ردیف", GetType(Integer))
            dtBankOpen.Columns.Add("تاریخ", GetType(String))
            dtBankOpen.Columns.Add("شماره پیگیری", GetType(String))
            dtBankOpen.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankOpen.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankOpen.Columns.Add("شرح", GetType(String))
            dtBankOpen.Columns.Add("واریز کننده/ذینفع", GetType(String))
            For i As Integer = 0 To _recResult.UnmatchedBank.Count - 1
                Dim bt = _recResult.UnmatchedBank(i)
                dtBankOpen.Rows.Add(i + 1, bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee)
            Next
            dgvBank_Open.DataSource = dtBankOpen
            FormatGrid(dgvBank_Open)

            ' 3. ارقام باز بدهکار صورتحساب (withdrawals: Credit > 0)
            Dim dtBankOpenDebit As New DataTable()
            dtBankOpenDebit.Columns.Add("ردیف", GetType(Integer))
            dtBankOpenDebit.Columns.Add("تاریخ", GetType(String))
            dtBankOpenDebit.Columns.Add("شماره پیگیری", GetType(String))
            dtBankOpenDebit.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankOpenDebit.Columns.Add("شرح", GetType(String))
            dtBankOpenDebit.Columns.Add("واریز کننده/ذینفع", GetType(String))
            Dim idxOpenDebit = 1
            For Each bt In _recResult.UnmatchedBank
                If bt.Credit > 0 Then
                    dtBankOpenDebit.Rows.Add(idxOpenDebit, bt.TxDate, bt.RefNo, bt.Credit, bt.Description, bt.Payee)
                    idxOpenDebit += 1
                End If
            Next
            dgvBank_OpenDebit.DataSource = dtBankOpenDebit
            FormatGrid(dgvBank_OpenDebit)

            ' 4. ارقام باز بستانکار صورتحساب (deposits: Debit > 0)
            Dim dtBankOpenCredit As New DataTable()
            dtBankOpenCredit.Columns.Add("ردیف", GetType(Integer))
            dtBankOpenCredit.Columns.Add("تاریخ", GetType(String))
            dtBankOpenCredit.Columns.Add("شماره پیگیری", GetType(String))
            dtBankOpenCredit.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankOpenCredit.Columns.Add("شرح", GetType(String))
            dtBankOpenCredit.Columns.Add("واریز کننده/ذینفع", GetType(String))
            Dim idxOpenCredit = 1
            For Each bt In _recResult.UnmatchedBank
                If bt.Debit > 0 Then
                    dtBankOpenCredit.Rows.Add(idxOpenCredit, bt.TxDate, bt.RefNo, bt.Debit, bt.Description, bt.Payee)
                    idxOpenCredit += 1
                End If
            Next
            dgvBank_OpenCredit.DataSource = dtBankOpenCredit
            FormatGrid(dgvBank_OpenCredit)

            ' 5. کل ارقام بسته صورتحساب
            Dim dtBankClosed As New DataTable()
            dtBankClosed.Columns.Add("ردیف", GetType(Integer))
            dtBankClosed.Columns.Add("تاریخ", GetType(String))
            dtBankClosed.Columns.Add("شماره پیگیری", GetType(String))
            dtBankClosed.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankClosed.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankClosed.Columns.Add("شرح", GetType(String))
            dtBankClosed.Columns.Add("واریز کننده/ذینفع", GetType(String))
            For i As Integer = 0 To _recResult.Matched.Count - 1
                Dim bt = _recResult.Matched(i).BankTx
                dtBankClosed.Rows.Add(i + 1, bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee)
            Next
            dgvBank_Closed.DataSource = dtBankClosed
            FormatGrid(dgvBank_Closed)

            ' 6. ارقام بسته بدهکار صورتحساب (withdrawals: Credit > 0)
            Dim dtBankClosedDebit As New DataTable()
            dtBankClosedDebit.Columns.Add("ردیف", GetType(Integer))
            dtBankClosedDebit.Columns.Add("تاریخ", GetType(String))
            dtBankClosedDebit.Columns.Add("شماره پیگیری", GetType(String))
            dtBankClosedDebit.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankClosedDebit.Columns.Add("شرح", GetType(String))
            dtBankClosedDebit.Columns.Add("واریز کننده/ذینفع", GetType(String))
            Dim idxClosedDebit = 1
            For Each pair In _recResult.Matched
                Dim bt = pair.BankTx
                If bt.Credit > 0 Then
                    dtBankClosedDebit.Rows.Add(idxClosedDebit, bt.TxDate, bt.RefNo, bt.Credit, bt.Description, bt.Payee)
                    idxClosedDebit += 1
                End If
            Next
            dgvBank_ClosedDebit.DataSource = dtBankClosedDebit
            FormatGrid(dgvBank_ClosedDebit)

            ' 7. ارقام بسته بستانکار صورتحساب (deposits: Debit > 0)
            Dim dtBankClosedCredit As New DataTable()
            dtBankClosedCredit.Columns.Add("ردیف", GetType(Integer))
            dtBankClosedCredit.Columns.Add("تاریخ", GetType(String))
            dtBankClosedCredit.Columns.Add("شماره پیگیری", GetType(String))
            dtBankClosedCredit.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankClosedCredit.Columns.Add("شرح", GetType(String))
            dtBankClosedCredit.Columns.Add("واریز کننده/ذینفع", GetType(String))
            Dim idxClosedCredit = 1
            For Each pair In _recResult.Matched
                Dim bt = pair.BankTx
                If bt.Debit > 0 Then
                    dtBankClosedCredit.Rows.Add(idxClosedCredit, bt.TxDate, bt.RefNo, bt.Debit, bt.Description, bt.Payee)
                    idxClosedCredit += 1
                End If
            Next
            dgvBank_ClosedCredit.DataSource = dtBankClosedCredit
            FormatGrid(dgvBank_ClosedCredit)

            ' 8. ارقام تکراری در صورتحساب
            Dim dtBankDup As New DataTable()
            dtBankDup.Columns.Add("ردیف", GetType(Integer))
            dtBankDup.Columns.Add("تاریخ", GetType(String))
            dtBankDup.Columns.Add("شماره پیگیری", GetType(String))
            dtBankDup.Columns.Add("برداشت (بدهکار)", GetType(Decimal))
            dtBankDup.Columns.Add("واریز (بستانکار)", GetType(Decimal))
            dtBankDup.Columns.Add("شرح", GetType(String))
            dtBankDup.Columns.Add("واریز کننده/ذینفع", GetType(String))
            Dim dupBankGroups = bankTransactionsAll.GroupBy(Function(x) New With {x.TxDate, x.RefNo, x.Debit, x.Credit, x.Description, x.Payee}).Where(Function(g) g.Count() > 1)
            Dim idxBankDup = 1
            For Each g In dupBankGroups
                For Each bt In g
                    dtBankDup.Rows.Add(idxBankDup, bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee)
                    idxBankDup += 1
                Next
            Next
            dgvBank_Dup.DataSource = dtBankDup
            FormatGrid(dgvBank_Dup)

            ' 9. پیشنهاد برای رفع مغایرت بانک
            Dim dtBankSuggestions As New DataTable()
            dtBankSuggestions.Columns.Add("TxID", GetType(Integer))
            dtBankSuggestions.Columns.Add("DetailID", GetType(Integer))
            dtBankSuggestions.Columns.Add("ردیف", GetType(Integer))
            dtBankSuggestions.Columns.Add("تاریخ بانک", GetType(String))
            dtBankSuggestions.Columns.Add("شماره پیگیری بانک", GetType(String))
            dtBankSuggestions.Columns.Add("برداشت (بدهکار) بانک", GetType(Decimal))
            dtBankSuggestions.Columns.Add("واریز (بستانکار) بانک", GetType(Decimal))
            dtBankSuggestions.Columns.Add("شرح بانک", GetType(String))
            dtBankSuggestions.Columns.Add("واریز کننده/ذینفع بانک", GetType(String))
            dtBankSuggestions.Columns.Add("سند دفتر", GetType(String))
            dtBankSuggestions.Columns.Add("تاریخ دفتر", GetType(String))
            dtBankSuggestions.Columns.Add("بدهکار دفتر", GetType(Decimal))
            dtBankSuggestions.Columns.Add("بستانکار دفتر", GetType(Decimal))
            dtBankSuggestions.Columns.Add("شماره پیگیری دفتر", GetType(String))
            dtBankSuggestions.Columns.Add("شرح ردیف دفتر", GetType(String))
            dtBankSuggestions.Columns.Add("درصد احتمال", GetType(String))

            Dim idxBankSugg = 1
            For Each sug In _recResult.Suggestions
                Dim bt = sug.BankTx
                Dim lt = sug.LedgerTx
                dtBankSuggestions.Rows.Add(
                    bt.TxID, lt.DetailID, idxBankSugg,
                    bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee,
                    lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description,
                    sug.MatchProbability.ToString("0.0") & "%"
                )
                idxBankSugg += 1
            Next

            dgvBank_Suggestions.DataSource = dtBankSuggestions
            FormatGrid(dgvBank_Suggestions)

            If dgvBank_Suggestions.Columns.Contains("TxID") Then dgvBank_Suggestions.Columns("TxID").Visible = False
            If dgvBank_Suggestions.Columns.Contains("DetailID") Then dgvBank_Suggestions.Columns("DetailID").Visible = False

            SetupSuggestionGridButtonsAndColors(dgvBank_Suggestions, True)


            ' --- pnlAsnad (Bottom Panel) ---
            ' 1. کل ارقام (Ledger)
            Dim dtAsnadAll As New DataTable()
            dtAsnadAll.Columns.Add("DetailID", GetType(Integer))
            dtAsnadAll.Columns.Add("EntryID", GetType(Integer))
            dtAsnadAll.Columns.Add("ردیف", GetType(Integer))
            dtAsnadAll.Columns.Add("سند", GetType(String))
            dtAsnadAll.Columns.Add("تاریخ", GetType(String))
            dtAsnadAll.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadAll.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadAll.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadAll.Columns.Add("شرح ردیف", GetType(String))

            Dim ledgerTable = recService.GetLedgerEntries(SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value, accountId, fromDate, toDate)
            Dim ledgerTransactionsAll As New List(Of LedgerTransaction)()
            For Each row As DataRow In ledgerTable.Rows
                Dim lt As New LedgerTransaction()
                lt.DetailID = Convert.ToInt32(row("DetailID"))
                lt.EntryID = Convert.ToInt32(row("EntryID"))
                If Not row.IsNull("EntryDate") Then lt.EntryDate = Convert.ToDateTime(row("EntryDate"))
                lt.RefNo = Convert.ToString(row("ReferenceNumber"))
                lt.Debit = If(row.IsNull("DebitAmount"), 0D, Convert.ToDecimal(row("DebitAmount")))
                lt.Credit = If(row.IsNull("CreditAmount"), 0D, Convert.ToDecimal(row("CreditAmount")))
                lt.Description = Convert.ToString(row("SharhRadif"))
                lt.TxNo = Convert.ToString(row("TransactionNumber"))
                lt.TxDate = Convert.ToString(row("TransactionDate"))
                ledgerTransactionsAll.Add(lt)
            Next

            For i As Integer = 0 To ledgerTransactionsAll.Count - 1
                Dim lt = ledgerTransactionsAll(i)
                dtAsnadAll.Rows.Add(lt.DetailID, lt.EntryID, i + 1, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description)
            Next
            dgvAsnad_All.DataSource = dtAsnadAll
            FormatGrid(dgvAsnad_All)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_All)

            ' 2. کل ارقام باز دفتر بانک
            Dim dtAsnadOpen As New DataTable()
            dtAsnadOpen.Columns.Add("DetailID", GetType(Integer))
            dtAsnadOpen.Columns.Add("EntryID", GetType(Integer))
            dtAsnadOpen.Columns.Add("ردیف", GetType(Integer))
            dtAsnadOpen.Columns.Add("سند", GetType(String))
            dtAsnadOpen.Columns.Add("تاریخ", GetType(String))
            dtAsnadOpen.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadOpen.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadOpen.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadOpen.Columns.Add("شرح ردیف", GetType(String))
            For i As Integer = 0 To _recResult.UnmatchedLedger.Count - 1
                Dim lt = _recResult.UnmatchedLedger(i)
                dtAsnadOpen.Rows.Add(lt.DetailID, lt.EntryID, i + 1, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description)
            Next
            dgvAsnad_Open.DataSource = dtAsnadOpen
            FormatGrid(dgvAsnad_Open)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_Open)

            ' 3. ارقام باز بدهکار دفتر بانک (Debit > 0)
            Dim dtAsnadOpenDebit As New DataTable()
            dtAsnadOpenDebit.Columns.Add("DetailID", GetType(Integer))
            dtAsnadOpenDebit.Columns.Add("EntryID", GetType(Integer))
            dtAsnadOpenDebit.Columns.Add("ردیف", GetType(Integer))
            dtAsnadOpenDebit.Columns.Add("سند", GetType(String))
            dtAsnadOpenDebit.Columns.Add("تاریخ", GetType(String))
            dtAsnadOpenDebit.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadOpenDebit.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadOpenDebit.Columns.Add("شرح ردیف", GetType(String))
            Dim idxAsnadOpenDebit = 1
            For Each lt In _recResult.UnmatchedLedger
                If lt.Debit > 0 Then
                    dtAsnadOpenDebit.Rows.Add(lt.DetailID, lt.EntryID, idxAsnadOpenDebit, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.TxNo, lt.Description)
                    idxAsnadOpenDebit += 1
                End If
            Next
            dgvAsnad_OpenDebit.DataSource = dtAsnadOpenDebit
            FormatGrid(dgvAsnad_OpenDebit)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_OpenDebit)

            ' 4. ارقام باز بستانکار دفتر بانک (Credit > 0)
            Dim dtAsnadOpenCredit As New DataTable()
            dtAsnadOpenCredit.Columns.Add("DetailID", GetType(Integer))
            dtAsnadOpenCredit.Columns.Add("EntryID", GetType(Integer))
            dtAsnadOpenCredit.Columns.Add("ردیف", GetType(Integer))
            dtAsnadOpenCredit.Columns.Add("سند", GetType(String))
            dtAsnadOpenCredit.Columns.Add("تاریخ", GetType(String))
            dtAsnadOpenCredit.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadOpenCredit.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadOpenCredit.Columns.Add("شرح ردیف", GetType(String))
            Dim idxAsnadOpenCredit = 1
            For Each lt In _recResult.UnmatchedLedger
                If lt.Credit > 0 Then
                    dtAsnadOpenCredit.Rows.Add(lt.DetailID, lt.EntryID, idxAsnadOpenCredit, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Credit, lt.TxNo, lt.Description)
                    idxAsnadOpenCredit += 1
                End If
            Next
            dgvAsnad_OpenCredit.DataSource = dtAsnadOpenCredit
            FormatGrid(dgvAsnad_OpenCredit)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_OpenCredit)

            ' 5. کل ارقام بسته دفتر بانک
            Dim dtAsnadClosed As New DataTable()
            dtAsnadClosed.Columns.Add("DetailID", GetType(Integer))
            dtAsnadClosed.Columns.Add("EntryID", GetType(Integer))
            dtAsnadClosed.Columns.Add("ردیف", GetType(Integer))
            dtAsnadClosed.Columns.Add("سند", GetType(String))
            dtAsnadClosed.Columns.Add("تاریخ", GetType(String))
            dtAsnadClosed.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadClosed.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadClosed.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadClosed.Columns.Add("شرح ردیف", GetType(String))
            For i As Integer = 0 To _recResult.Matched.Count - 1
                Dim lt = _recResult.Matched(i).LedgerTx
                dtAsnadClosed.Rows.Add(lt.DetailID, lt.EntryID, i + 1, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description)
            Next
            dgvAsnad_Closed.DataSource = dtAsnadClosed
            FormatGrid(dgvAsnad_Closed)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_Closed)

            ' 6. ارقام بسته بدهکار دفتر بانک (Debit > 0)
            Dim dtAsnadClosedDebit As New DataTable()
            dtAsnadClosedDebit.Columns.Add("DetailID", GetType(Integer))
            dtAsnadClosedDebit.Columns.Add("EntryID", GetType(Integer))
            dtAsnadClosedDebit.Columns.Add("ردیف", GetType(Integer))
            dtAsnadClosedDebit.Columns.Add("سند", GetType(String))
            dtAsnadClosedDebit.Columns.Add("تاریخ", GetType(String))
            dtAsnadClosedDebit.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadClosedDebit.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadClosedDebit.Columns.Add("شرح ردیف", GetType(String))
            Dim idxAsnadClosedDebit = 1
            For Each pair In _recResult.Matched
                Dim lt = pair.LedgerTx
                If lt.Debit > 0 Then
                    dtAsnadClosedDebit.Rows.Add(lt.DetailID, lt.EntryID, idxAsnadClosedDebit, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.TxNo, lt.Description)
                    idxAsnadClosedDebit += 1
                End If
            Next
            dgvAsnad_ClosedDebit.DataSource = dtAsnadClosedDebit
            FormatGrid(dgvAsnad_ClosedDebit)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_ClosedDebit)

            ' 7. ارقام بسته بستانکار دفتر بانک (Credit > 0)
            Dim dtAsnadClosedCredit As New DataTable()
            dtAsnadClosedCredit.Columns.Add("DetailID", GetType(Integer))
            dtAsnadClosedCredit.Columns.Add("EntryID", GetType(Integer))
            dtAsnadClosedCredit.Columns.Add("ردیف", GetType(Integer))
            dtAsnadClosedCredit.Columns.Add("سند", GetType(String))
            dtAsnadClosedCredit.Columns.Add("تاریخ", GetType(String))
            dtAsnadClosedCredit.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadClosedCredit.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadClosedCredit.Columns.Add("شرح ردیف", GetType(String))
            Dim idxAsnadClosedCredit = 1
            For Each pair In _recResult.Matched
                Dim lt = pair.LedgerTx
                If lt.Credit > 0 Then
                    dtAsnadClosedCredit.Rows.Add(lt.DetailID, lt.EntryID, idxAsnadClosedCredit, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Credit, lt.TxNo, lt.Description)
                    idxAsnadClosedCredit += 1
                End If
            Next
            dgvAsnad_ClosedCredit.DataSource = dtAsnadClosedCredit
            FormatGrid(dgvAsnad_ClosedCredit)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_ClosedCredit)

            ' 8. ارقام تکراری دفتر بانک
            Dim dtAsnadDup As New DataTable()
            dtAsnadDup.Columns.Add("DetailID", GetType(Integer))
            dtAsnadDup.Columns.Add("EntryID", GetType(Integer))
            dtAsnadDup.Columns.Add("ردیف", GetType(Integer))
            dtAsnadDup.Columns.Add("سند", GetType(String))
            dtAsnadDup.Columns.Add("تاریخ", GetType(String))
            dtAsnadDup.Columns.Add("بدهکار", GetType(Decimal))
            dtAsnadDup.Columns.Add("بستانکار", GetType(Decimal))
            dtAsnadDup.Columns.Add("شماره پیگیری", GetType(String))
            dtAsnadDup.Columns.Add("شرح ردیف", GetType(String))
            Dim dupLedgerGroups = ledgerTransactionsAll.GroupBy(Function(x) New With {x.EntryDate, x.RefNo, x.Debit, x.Credit, x.Description}).Where(Function(g) g.Count() > 1)
            Dim idxAsnadDup = 1
            For Each g In dupLedgerGroups
                For Each lt In g
                    dtAsnadDup.Rows.Add(lt.DetailID, lt.EntryID, idxAsnadDup, lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description)
                    idxAsnadDup += 1
                Next
            Next
            dgvAsnad_Dup.DataSource = dtAsnadDup
            FormatGrid(dgvAsnad_Dup)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_Dup)

            ' 9. پیشنهاد برای رفع مغایرت دفتر
            Dim dtAsnadSuggestions As New DataTable()
            dtAsnadSuggestions.Columns.Add("TxID", GetType(Integer))
            dtAsnadSuggestions.Columns.Add("DetailID", GetType(Integer))
            dtAsnadSuggestions.Columns.Add("EntryID", GetType(Integer))
            dtAsnadSuggestions.Columns.Add("ردیف", GetType(Integer))
            dtAsnadSuggestions.Columns.Add("سند دفتر", GetType(String))
            dtAsnadSuggestions.Columns.Add("تاریخ دفتر", GetType(String))
            dtAsnadSuggestions.Columns.Add("بدهکار دفتر", GetType(Decimal))
            dtAsnadSuggestions.Columns.Add("بستانکار دفتر", GetType(Decimal))
            dtAsnadSuggestions.Columns.Add("شماره پیگیری دفتر", GetType(String))
            dtAsnadSuggestions.Columns.Add("شرح ردیف دفتر", GetType(String))
            dtAsnadSuggestions.Columns.Add("تاریخ بانک", GetType(String))
            dtAsnadSuggestions.Columns.Add("شماره پیگیری بانک", GetType(String))
            dtAsnadSuggestions.Columns.Add("برداشت (بدهکار) بانک", GetType(Decimal))
            dtAsnadSuggestions.Columns.Add("واریز (بستانکار) بانک", GetType(Decimal))
            dtAsnadSuggestions.Columns.Add("شرح بانک", GetType(String))
            dtAsnadSuggestions.Columns.Add("واریز کننده/ذینفع بانک", GetType(String))
            dtAsnadSuggestions.Columns.Add("درصد احتمال", GetType(String))

            Dim idxAsnadSugg = 1
            For Each sug In _recResult.Suggestions
                Dim bt = sug.BankTx
                Dim lt = sug.LedgerTx
                dtAsnadSuggestions.Rows.Add(
                    bt.TxID, lt.DetailID, lt.EntryID, idxAsnadSugg,
                    lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description,
                    bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee,
                    sug.MatchProbability.ToString("0.0") & "%"
                )
                idxAsnadSugg += 1
            Next

            dgvAsnad_Suggestions.DataSource = dtAsnadSuggestions
            FormatGrid(dgvAsnad_Suggestions)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_Suggestions)

            If dgvAsnad_Suggestions.Columns.Contains("TxID") Then dgvAsnad_Suggestions.Columns("TxID").Visible = False
            If dgvAsnad_Suggestions.Columns.Contains("DetailID") Then dgvAsnad_Suggestions.Columns("DetailID").Visible = False
            If dgvAsnad_Suggestions.Columns.Contains("EntryID") Then dgvAsnad_Suggestions.Columns("EntryID").Visible = False

            SetupSuggestionGridButtonsAndColors(dgvAsnad_Suggestions, False)

            ' --- Summary Label ---
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

        Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
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

        Private Sub SetupAsnadGridButtonsAndEditing(dgv As DataGridView)
            If dgv.Columns.Contains("DetailID") Then dgv.Columns("DetailID").Visible = False
            If dgv.Columns.Contains("EntryID") Then dgv.Columns("EntryID").Visible = False

            dgv.ReadOnly = False
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect

            If dgv.Columns.Contains("btnEditSanadCol") Then
                dgv.Columns.Remove("btnEditSanadCol")
            End If

            Dim btnEditSanad As New DataGridViewButtonColumn()
            btnEditSanad.Name = "btnEditSanadCol"
            btnEditSanad.HeaderText = "سند"
            btnEditSanad.Text = "رفتن به سند"
            btnEditSanad.UseColumnTextForButtonValue = True
            btnEditSanad.Width = 100
            dgv.Columns.Add(btnEditSanad)

            btnEditSanad.DisplayIndex = 1

            For Each col As DataGridViewColumn In dgv.Columns
                If col.Name = "شماره پیگیری" OrElse col.Name = "شماره پیگیری دفتر" Then
                    col.ReadOnly = False
                Else
                    col.ReadOnly = True
                End If
            Next
        End Sub

        Private Sub dgvAsnad_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAsnad_All.CellContentClick, dgvAsnad_Open.CellContentClick, dgvAsnad_OpenDebit.CellContentClick, dgvAsnad_OpenCredit.CellContentClick, dgvAsnad_Closed.CellContentClick, dgvAsnad_ClosedDebit.CellContentClick, dgvAsnad_ClosedCredit.CellContentClick, dgvAsnad_Dup.CellContentClick, dgvAsnad_Suggestions.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim senderGrid = DirectCast(sender, DataGridView)
            Dim colName = senderGrid.Columns(e.ColumnIndex).Name

            If colName = "btnEditSanadCol" Then
                Dim entryId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("EntryID").Value)
                Dim detailId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("DetailID").Value)

                If entryId > 0 Then
                    Dim savedGrid = senderGrid
                    Dim savedCellAddress = senderGrid.CurrentCellAddress

                    Try
                        Using detailsForm As New HesabdarySanad2Form(entryId)
                            detailsForm.HighlightDetailID = detailId
                            detailsForm.ShowDialog(Me)
                        End Using

                        btnRunReconciliation.PerformClick()

                        Me.BeginInvoke(New Action(Sub()
                                                      Try
                                                          savedGrid.Focus()
                                                          If savedCellAddress.Y >= 0 AndAlso savedCellAddress.Y < savedGrid.Rows.Count AndAlso
                                                             savedCellAddress.X >= 0 AndAlso savedCellAddress.X < savedGrid.Columns.Count Then
                                                              savedGrid.CurrentCell = savedGrid.Rows(savedCellAddress.Y).Cells(savedCellAddress.X)
                                                          End If
                                                      Catch
                                                      End Try
                                                  End Sub))
                    Catch ex As Exception
                        MessageBox.Show("خطا در باز کردن سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub dgvAsnad_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAsnad_All.CellEndEdit, dgvAsnad_Open.CellEndEdit, dgvAsnad_OpenDebit.CellEndEdit, dgvAsnad_OpenCredit.CellEndEdit, dgvAsnad_Closed.CellEndEdit, dgvAsnad_ClosedDebit.CellEndEdit, dgvAsnad_ClosedCredit.CellEndEdit, dgvAsnad_Dup.CellEndEdit, dgvAsnad_Suggestions.CellEndEdit
            If e.RowIndex < 0 Then Return

            Dim senderGrid = DirectCast(sender, DataGridView)
            Dim col = senderGrid.Columns(e.ColumnIndex)

            If col.Name = "شماره پیگیری" OrElse col.Name = "شماره پیگیری دفتر" Then
                Dim newVal = Convert.ToString(senderGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                Dim detailId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("DetailID").Value)

                If detailId > 0 Then
                    Try
                        Sql.ExecuteNonQuery("UPDATE AccountingEntryDetails SET TransactionNumber = ? WHERE DetailID = ?", newVal, detailId)
                        Me.BeginInvoke(New Action(Sub()
                                                      btnRunReconciliation.PerformClick()
                                                  End Sub))
                    Catch ex As Exception
                        MessageBox.Show("خطا در ویرایش شماره پیگیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub ExportToCsv(filePath As String)
            Dim sb As New StringBuilder()

            ' 1. Write Header Info
            sb.AppendLine("گزارش مغایرت بانکی")
            sb.AppendLine("بانک انتخاب شده," & cmbRecBank.Text)
            sb.AppendLine("تاریخ مغایرت‌گیری," & DateTime.Now.ToString("yyyy/MM/dd HH:mm"))
            sb.AppendLine()

            ' 2. Write Bank Discrepancies
            sb.AppendLine("--- اقلام باز بانکی (غایب در دفاتر) ---")
            sb.AppendLine("ردیف,تاریخ,شماره پیگیری,مبلغ واریز,مبلغ برداشت,شرح,واریز کننده/ذینفع")
            For i As Integer = 0 To _recResult.UnmatchedBank.Count - 1
                Dim bt = _recResult.UnmatchedBank(i)
                sb.AppendLine(String.Format("{0},{1},{2},{3},{4},""{5}"",""{6}""", i + 1, bt.TxDate, bt.RefNo, bt.Debit, bt.Credit, bt.Description.Replace("""", """"""), bt.Payee.Replace("""", """""")))
            Next
            sb.AppendLine()

            ' 3. Write Ledger Discrepancies
            sb.AppendLine("--- اقلام باز دفاتر (غایب در بانک) ---")
            sb.AppendLine("سند,تاریخ سند,بدهکار (واریز),بستانکار (برداشت),شماره پیگیری دفاتر,شرح سند")
            For Each lt In _recResult.UnmatchedLedger
                sb.AppendLine(String.Format("{0},{1},{2},{3},{4},""{5}""", lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description.Replace("""", """""")))
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

        Private Sub txtAccountType_TextChanged(sender As Object, e As EventArgs) Handles txtAccountType.TextChanged

        End Sub

        Private Sub lblAccountType_Click(sender As Object, e As EventArgs) Handles lblAccountType.Click

        End Sub

        Private Sub txtBranchAddress_TextChanged(sender As Object, e As EventArgs) Handles txtBranchAddress.TextChanged

        End Sub

        Private Sub lblBranchAddress_Click(sender As Object, e As EventArgs) Handles lblBranchAddress.Click

        End Sub

        Private Sub txtBranchCode_TextChanged(sender As Object, e As EventArgs) Handles txtBranchCode.TextChanged

        End Sub

        Private Sub lblBranchCode_Click(sender As Object, e As EventArgs) Handles lblBranchCode.Click

        End Sub

        Private Sub lblAccountNumber_Click(sender As Object, e As EventArgs) Handles lblAccountNumber.Click

        End Sub

        Private Sub txtAccountNumber_TextChanged(sender As Object, e As EventArgs) Handles txtAccountNumber.TextChanged

        End Sub

        Private Sub lblAccountCoding_Click(sender As Object, e As EventArgs) Handles lblAccountCoding.Click

        End Sub

        Private Sub SetupSuggestionGridButtonsAndColors(dgv As DataGridView, isBankOriented As Boolean)
            If dgv.Columns.Contains("btnResolveCol") Then
                dgv.Columns.Remove("btnResolveCol")
            End If

            Dim btnResolve As New DataGridViewButtonColumn()
            btnResolve.Name = "btnResolveCol"
            btnResolve.HeaderText = "رفع مغایرت"
            btnResolve.Text = "رفع مغایرت"
            btnResolve.UseColumnTextForButtonValue = True
            btnResolve.Width = 90
            dgv.Columns.Add(btnResolve)

            dgv.Columns("btnResolveCol").DisplayIndex = dgv.Columns.Count - 1

            For Each col As DataGridViewColumn In dgv.Columns
                If Not col.Visible OrElse col.Name = "btnResolveCol" OrElse col.Name = "ردیف" Then Continue For

                If col.HeaderText.Contains("دفتر") Then
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 248, 240)
                    col.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(220, 240, 220)
                ElseIf col.HeaderText.Contains("بانک") Then
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 246, 250)
                    col.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(220, 230, 242)
                End If
            Next
            dgv.EnableHeadersVisualStyles = False
        End Sub

        Private Sub dgvSuggestions_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBank_Suggestions.CellContentClick, dgvAsnad_Suggestions.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim senderGrid = DirectCast(sender, DataGridView)
            Dim colName = senderGrid.Columns(e.ColumnIndex).Name

            If colName = "btnResolveCol" Then
                Dim txId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("TxID").Value)
                Dim detailId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("DetailID").Value)

                Dim confirm = MessageBox.Show("آیا از ثبت این مورد به عنوان مغایرت رفع‌شده و تطبیق آن‌ها اطمینان دارید؟", "تایید رفع مغایرت", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirm = DialogResult.Yes Then
                    Try
                        Sql.ExecuteNonQuery("UPDATE SoBank_2 SET MatchedDetailID = ? WHERE TxID = ?", detailId, txId)
                        MessageBox.Show("مغایرت با موفقیت رفع شد و تراکنش‌ها تطبیق یافتند.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        btnRunReconciliation.PerformClick()
                    Catch ex As Exception
                        MessageBox.Show("خطا در ثبت رفع مغایرت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub
    End Class
End Namespace
