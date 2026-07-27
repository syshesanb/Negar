Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Partial Public Class HesabdaryMogBankForm
        Private ReadOnly recService As New BankReconciliationService()
        Private ReadOnly service As New AccountingService()
        Private _rawImportedTable As DataTable
        Private _recResult As ReconciliationResult
        Private _selectedFilePath As String = ""
        Private _selectedBankID As Integer = 0
        Private _selectedMappingAccountID As Integer? = Nothing
        Private ReadOnly _searchTextBoxes As New List(Of TextBox)()

        Private _savedHeaderRow As Integer = 1
        Private _savedColDate As String = ""
        Private _savedColRef As String = ""
        Private _savedColDebit As String = ""
        Private _savedColCredit As String = ""
        Private _savedColDesc As String = ""
        Private _savedColPayee As String = ""

        Private lblBankRowCount As Label
        Private lblAsnadRowCount As Label

        Private Sub InitRowCountLabels()
            lblBankRowCount = New Label()
            lblBankRowCount.AutoSize = True
            lblBankRowCount.BackColor = Color.Transparent
            lblBankRowCount.ForeColor = Color.White
            lblBankRowCount.Font = New Font("Tahoma", 8.5!, FontStyle.Regular)
            lblBankRowCount.Location = New Point(10, 2)
            lblBankTitle.Controls.Add(lblBankRowCount)

            lblAsnadRowCount = New Label()
            lblAsnadRowCount.AutoSize = True
            lblAsnadRowCount.BackColor = Color.Transparent
            lblAsnadRowCount.ForeColor = Color.White
            lblAsnadRowCount.Font = New Font("Tahoma", 8.5!, FontStyle.Regular)
            lblAsnadRowCount.Location = New Point(10, 2)
            lblAsnadTitle.Controls.Add(lblAsnadRowCount)
        End Sub

        Private Sub UpdateBankRowCount()
            If lblBankRowCount Is Nothing Then Return
            Dim count As Integer = 0

            If tcBank IsNot Nothing AndAlso tcBank.SelectedTab IsNot Nothing Then
                For Each ctrl As Control In tcBank.SelectedTab.Controls
                    If TypeOf ctrl Is DataGridView Then
                        Dim dgv = DirectCast(ctrl, DataGridView)
                        count = dgv.Rows.Count
                        If dgv.AllowUserToAddRows Then count -= 1
                        Exit For
                    End If
                Next
            End If

            lblBankRowCount.Text = "تعداد رکورد در این تب : " & count
        End Sub

        Private Sub UpdateAsnadRowCount()
            If lblAsnadRowCount Is Nothing Then Return
            Dim count As Integer = 0

            If tcAsnad IsNot Nothing AndAlso tcAsnad.SelectedTab IsNot Nothing Then
                For Each ctrl As Control In tcAsnad.SelectedTab.Controls
                    If TypeOf ctrl Is DataGridView Then
                        Dim dgv = DirectCast(ctrl, DataGridView)
                        count = dgv.Rows.Count
                        If dgv.AllowUserToAddRows Then count -= 1
                        Exit For
                    End If
                Next
            End If

            lblAsnadRowCount.Text = "تعداد رکورد در این تب : " & count
        End Sub

        Private Sub TcBank_SelectedIndexChanged(sender As Object, e As EventArgs)
            UpdateBankRowCount()
        End Sub

        Private Sub TcAsnad_SelectedIndexChanged(sender As Object, e As EventArgs)
            UpdateAsnadRowCount()
        End Sub

        Private Sub HesabdaryMogBankForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            Negar.Business.ThemeHelper.AppendStatusBar(Me)
            If Me.dgvBanks IsNot Nothing Then Me.dgvBanks.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvImportPreview IsNot Nothing Then Me.dgvImportPreview.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_All IsNot Nothing Then Me.dgvBank_All.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_Open IsNot Nothing Then Me.dgvBank_Open.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_OpenDebit IsNot Nothing Then Me.dgvBank_OpenDebit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_OpenCredit IsNot Nothing Then Me.dgvBank_OpenCredit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_Closed IsNot Nothing Then Me.dgvBank_Closed.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_ClosedDebit IsNot Nothing Then Me.dgvBank_ClosedDebit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_ClosedCredit IsNot Nothing Then Me.dgvBank_ClosedCredit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_Dup IsNot Nothing Then Me.dgvBank_Dup.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvBank_Suggestions IsNot Nothing Then Me.dgvBank_Suggestions.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_All IsNot Nothing Then Me.dgvAsnad_All.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_Open IsNot Nothing Then Me.dgvAsnad_Open.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_OpenDebit IsNot Nothing Then Me.dgvAsnad_OpenDebit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_OpenCredit IsNot Nothing Then Me.dgvAsnad_OpenCredit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_Closed IsNot Nothing Then Me.dgvAsnad_Closed.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_ClosedDebit IsNot Nothing Then Me.dgvAsnad_ClosedDebit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_ClosedCredit IsNot Nothing Then Me.dgvAsnad_ClosedCredit.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_Dup IsNot Nothing Then Me.dgvAsnad_Dup.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            If Me.dgvAsnad_Suggestions IsNot Nothing Then Me.dgvAsnad_Suggestions.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            LoadBankList()
            LoadBankCombos()
            ClearBankInputs()
            ClearMapping()

            InitRowCountLabels()
            AddHandler tcBank.SelectedIndexChanged, AddressOf TcBank_SelectedIndexChanged
            AddHandler tcAsnad.SelectedIndexChanged, AddressOf TcAsnad_SelectedIndexChanged
        End Sub

        Private Sub tcMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcMain.SelectedIndexChanged
            If tcMain.SelectedTab Is tpIntroBanks Then
                ' No need to load coding combo since we use account dialog selection
            End If
        End Sub

        ' ==========================================
        ' TAB 1: معرفی بانک‌ها (CRUD)
        ' ==========================================

        Private Sub LoadBankList()
            Try
                If Not SessionContext.CurrentCompanyID.HasValue Then Return
                Dim dt = Sql.ExecuteTable(
                    "SELECT b.BankID, b.BankName, b.BranchName, b.BranchCode, b.BranchAddress, b.AccountType, b.AccountNumber, b.AccountID, " &
                    "c.AccountCode || ' - ' || c.AccountName As AccountMapping " &
                    "FROM SoBank_1 b " &
                    "LEFT JOIN SarfaslHesab c ON b.AccountID = c.AccountID " &
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

            If cmbImportBank.SelectedValue IsNot Nothing AndAlso Not Convert.IsDBNull(cmbImportBank.SelectedValue) Then
                Dim bId As Integer
                If Integer.TryParse(Convert.ToString(cmbImportBank.SelectedValue), bId) Then
                    Try
                        Dim dtMap = Sql.ExecuteTable("SELECT HeaderRowIndex, ColDate, ColRef, ColDebit, ColCredit, ColDesc, ColPayee FROM SoBank_1 WHERE BankID = ?", bId)
                        If dtMap.Rows.Count > 0 Then
                            Dim r = dtMap.Rows(0)
                            _savedHeaderRow = If(r("HeaderRowIndex") Is DBNull.Value, 1, Convert.ToInt32(r("HeaderRowIndex")))
                            _savedColDate = If(r("ColDate") Is DBNull.Value, "", Convert.ToString(r("ColDate")))
                            _savedColRef = If(r("ColRef") Is DBNull.Value, "", Convert.ToString(r("ColRef")))
                            _savedColDebit = If(r("ColDebit") Is DBNull.Value, "", Convert.ToString(r("ColDebit")))
                            _savedColCredit = If(r("ColCredit") Is DBNull.Value, "", Convert.ToString(r("ColCredit")))
                            _savedColDesc = If(r("ColDesc") Is DBNull.Value, "", Convert.ToString(r("ColDesc")))
                            _savedColPayee = If(r("ColPayee") Is DBNull.Value, "", Convert.ToString(r("ColPayee")))

                            ' Apply if file is already loaded
                            If _rawImportedTable IsNot Nothing AndAlso _rawImportedTable.Rows.Count > 0 Then
                                nudHeaderRow.Value = _savedHeaderRow
                                RefreshColumnMappings()
                            End If
                        Else
                            _savedHeaderRow = 1
                            _savedColDate = ""
                            _savedColRef = ""
                            _savedColDebit = ""
                            _savedColCredit = ""
                            _savedColDesc = ""
                            _savedColPayee = ""
                        End If
                    Catch ex As Exception
                    End Try
                End If
            End If
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
                ' Load transactions from SoBank_2 in chronological order to calculate running balance
                Dim dt = Sql.ExecuteTable(
                    "SELECT TxID, TxDate, RefNo, Debit, Credit, Description, Payee FROM SoBank_2 WHERE BankID = ? ORDER BY TxDate ASC, TxID ASC",
                    bankId)

                dt.Columns.Add("Balance", GetType(Decimal))
                Dim runningBal As Decimal = 0D
                For Each row As DataRow In dt.Rows
                    Dim debit = If(row("Debit") Is DBNull.Value, 0D, Convert.ToDecimal(row("Debit")))
                    Dim credit = If(row("Credit") Is DBNull.Value, 0D, Convert.ToDecimal(row("Credit")))
                    runningBal += (debit - credit)
                    row("Balance") = runningBal
                Next

                ' Sort descending for display (newest first)
                Dim dv = dt.DefaultView
                dv.Sort = "TxDate DESC, TxID DESC"

                ' Bind to DataGridView
                dgvImportPreview.DataSource = dv

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
            If dgvImportPreview.Columns.Contains("Balance") Then
                dgvImportPreview.Columns("Balance").HeaderText = "مانده"
                dgvImportPreview.Columns("Balance").DefaultCellStyle.Format = "N0"
                dgvImportPreview.Columns("Balance").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            If dgvImportPreview.Columns.Contains("Description") Then dgvImportPreview.Columns("Description").HeaderText = "شرح"
            If dgvImportPreview.Columns.Contains("Payee") Then dgvImportPreview.Columns("Payee").HeaderText = "واریزکننده/ذینفع"

            ' Add Edit button
            Dim btnEdit As New DataGridViewButtonColumn()
            btnEdit.Name = "btnEditCol"
            btnEdit.HeaderText = "ویرایش"
            btnEdit.Text = "ویرایش"
            btnEdit.UseColumnTextForButtonValue = True
            btnEdit.Width = 50
            dgvImportPreview.Columns.Add(btnEdit)

            ' Add Delete button
            Dim btnDelete As New DataGridViewButtonColumn()
            btnDelete.Name = "btnDeleteCol"
            btnDelete.HeaderText = "حذف"
            btnDelete.Text = "حذف"
            btnDelete.UseColumnTextForButtonValue = True
            btnDelete.Width = 50
            dgvImportPreview.Columns.Add(btnDelete)

            ' Set DisplayIndex explicitly to show Edit first, then Delete, then Date and other columns in order
            If dgvImportPreview.Columns.Contains("btnEditCol") Then dgvImportPreview.Columns("btnEditCol").DisplayIndex = 0
            If dgvImportPreview.Columns.Contains("btnDeleteCol") Then dgvImportPreview.Columns("btnDeleteCol").DisplayIndex = 1
            If dgvImportPreview.Columns.Contains("TxDate") Then dgvImportPreview.Columns("TxDate").DisplayIndex = 2
            If dgvImportPreview.Columns.Contains("RefNo") Then dgvImportPreview.Columns("RefNo").DisplayIndex = 3
            If dgvImportPreview.Columns.Contains("Debit") Then dgvImportPreview.Columns("Debit").DisplayIndex = 4
            If dgvImportPreview.Columns.Contains("Credit") Then dgvImportPreview.Columns("Credit").DisplayIndex = 5
            If dgvImportPreview.Columns.Contains("Balance") Then dgvImportPreview.Columns("Balance").DisplayIndex = 6
            If dgvImportPreview.Columns.Contains("Description") Then dgvImportPreview.Columns("Description").DisplayIndex = 7
            If dgvImportPreview.Columns.Contains("Payee") Then dgvImportPreview.Columns("Payee").DisplayIndex = 8

            FormatGrid(dgvImportPreview)

            ' Override AutoSize to make columns fixed width and Payee fill
            dgvImportPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If dgvImportPreview.Columns.Contains("btnEditCol") Then
                dgvImportPreview.Columns("btnEditCol").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("btnEditCol").Width = 50
            End If
            If dgvImportPreview.Columns.Contains("btnDeleteCol") Then
                dgvImportPreview.Columns("btnDeleteCol").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("btnDeleteCol").Width = 50
            End If
            If dgvImportPreview.Columns.Contains("TxDate") Then
                dgvImportPreview.Columns("TxDate").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("TxDate").Width = 80
            End If
            If dgvImportPreview.Columns.Contains("RefNo") Then
                dgvImportPreview.Columns("RefNo").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("RefNo").Width = 120
            End If
            If dgvImportPreview.Columns.Contains("Debit") Then
                dgvImportPreview.Columns("Debit").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("Debit").Width = 100
            End If
            If dgvImportPreview.Columns.Contains("Credit") Then
                dgvImportPreview.Columns("Credit").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("Credit").Width = 100
            End If
            If dgvImportPreview.Columns.Contains("Balance") Then
                dgvImportPreview.Columns("Balance").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvImportPreview.Columns("Balance").Width = 110
            End If
            If dgvImportPreview.Columns.Contains("Payee") Then
                dgvImportPreview.Columns("Payee").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
            If dgvImportPreview.Columns.Contains("Description") Then
                dgvImportPreview.Columns("Description").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
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
                    Dim accId = Convert.ToInt32(row.Cells("AccountID").Value)
                    _selectedMappingAccountID = accId
                    lblAccountID.Text = accId.ToString()
                    Dim chain = service.GetAccountHierarchyChain(accId)
                    Dim codes As New List(Of String)()
                    For Each item In chain
                        codes.Add(item.Item1)
                    Next
                    lblAccountCodeChain.Text = "کد سرفصل: " & String.Join("/", codes)
                Else
                    _selectedMappingAccountID = Nothing
                    lblAccountID.Text = ""
                    lblAccountCodeChain.Text = ""
                End If

                ' استخراج و نمایش بازه زمانی صورت حساب بانکی وارد شده از دیتابیس
                Dim minDate As String = ""
                Dim maxDate As String = ""
                Try
                    Dim dtDates = Sql.ExecuteTable("SELECT MIN(TxDate), MAX(TxDate) FROM SoBank_2 WHERE BankID = ?", _selectedBankID)
                    If dtDates.Rows.Count > 0 Then
                        Dim minVal = dtDates.Rows(0)(0)
                        Dim maxVal = dtDates.Rows(0)(1)
                        If minVal IsNot Nothing AndAlso Not Convert.IsDBNull(minVal) Then minDate = Convert.ToString(minVal)
                        If maxVal IsNot Nothing AndAlso Not Convert.IsDBNull(maxVal) Then maxDate = Convert.ToString(maxVal)
                    End If
                Catch ex As Exception
                End Try

                If Not String.IsNullOrEmpty(minDate) AndAlso Not String.IsNullOrEmpty(maxDate) Then
                    lblBankStatementRange.Text = "بازه تاریخی صورت حساب وارد شده: از تاریخ: " & minDate & " تا تاریخ: " & maxDate
                Else
                    lblBankStatementRange.Text = "بازه تاریخی صورت حساب وارد شده: فاقد صورت حساب وارد شده"
                End If
            End If
        End Sub

        Private Sub btnSaveBank_Click(sender As Object, e As EventArgs) Handles btnSaveBank.Click
            If String.IsNullOrWhiteSpace(txtBankName.Text) OrElse String.IsNullOrWhiteSpace(txtAccountNumber.Text) Then
                MessageBox.Show("نام بانک و شماره حساب الزامی هستند.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not _selectedMappingAccountID.HasValue Then
                MessageBox.Show("انتخاب سرفصل حساب الزامی است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim accountId = _selectedMappingAccountID.Value

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
            _selectedMappingAccountID = Nothing
            lblAccountID.Text = ""
            lblAccountCodeChain.Text = ""
            If lblBankStatementRange IsNot Nothing Then
                lblBankStatementRange.Text = "بازه تاریخی صورت حساب وارد شده: فاقد صورت حساب وارد شده"
            End If

            If dgvBanks.SelectedRows.Count > 0 Then
                dgvBanks.ClearSelection()
            End If
        End Sub

        Private Sub btnSelectAccount_Click(sender As Object, e As EventArgs) Handles btnSelectAccount.Click
            Using codingForm As New HesabdaryCodingForm()
                codingForm.SelectMode = True
                codingForm.Size = New Size(760, 380)
                codingForm.StartPosition = FormStartPosition.CenterParent
                codingForm.ShowDialog(Me)
                If codingForm.SelectedAccountID.HasValue Then
                    Dim accId = codingForm.SelectedAccountID.Value
                    _selectedMappingAccountID = accId
                    lblAccountID.Text = accId.ToString()

                    Dim chain = service.GetAccountHierarchyChain(accId)
                    Dim codes As New List(Of String)()
                    For Each item In chain
                        codes.Add(item.Item1)
                    Next
                    lblAccountCodeChain.Text = "کد سرفصل: " & String.Join("/", codes)
                End If
            End Using
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
                ofd.Filter = "کل فایل‌های پشتیبانی شده (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|فایل‌های اکسل (*.xlsx;*.xls)|*.xlsx;*.xls|فایل‌های متنی (*.csv)|*.csv|همه فایل‌ها (*.*)|*.*"
                ofd.Title = "انتخاب صورت‌حساب بانکی"
                If ofd.ShowDialog() = DialogResult.OK Then
                    _selectedFilePath = ofd.FileName
                    lblImportFilePath.Text = _selectedFilePath
                    Try
                        Me.Cursor = Cursors.WaitCursor
                        RemoveButtonColumns()
                        ClearSearchTextBoxes()
                        _rawImportedTable = recService.ReadBankFileRaw(_selectedFilePath)
                        dgvImportPreview.DataSource = _rawImportedTable
                        Me.Cursor = Cursors.Default

                        ' Apply saved NumericUpDown header row index
                        nudHeaderRow.Value = If(_savedHeaderRow > 0, _savedHeaderRow, 1)
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

            PopulateCombo(cmbColDate, cols, _savedColDate, "تاریخ", "date")
            PopulateCombo(cmbColRef, cols, _savedColRef, "پیگیری", "سند", "ارجاع", "ref")
            PopulateCombo(cmbColDebit, cols, _savedColDebit, "واریز", "بستانکار", "مبلغ", "debit")
            PopulateCombo(cmbColCredit, cols, _savedColCredit, "برداشت", "بدهکار", "credit")
            PopulateCombo(cmbColDesc, cols, _savedColDesc, "شرح", "بابت", "توضیحات", "desc")
            PopulateCombo(cmbColPayee, cols, _savedColPayee, "واریز کننده", "ذینفع", "payee", "beneficiary")
        End Sub

        Private Sub PopulateCombo(combo As ComboBox, items As List(Of String), savedColumn As String, ParamArray keywords() As String)
            combo.Items.Clear()
            combo.Items.Add("-- انتخاب کنید --")
            For Each item In items
                combo.Items.Add(item)
            Next

            ' 1. If we have a saved column mapping name and it exists in items, select it!
            If Not String.IsNullOrEmpty(savedColumn) AndAlso items.Contains(savedColumn) Then
                combo.SelectedItem = savedColumn
                Exit Sub
            End If

            ' 2. Fallback to auto-detection
            combo.SelectedIndex = 0
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

            ' Save mapping configuration to SoBank_1
            Dim colDateText = If(cmbColDate.SelectedIndex > 0, cmbColDate.SelectedItem.ToString(), "")
            Dim colRefText = If(cmbColRef.SelectedIndex > 0, cmbColRef.SelectedItem.ToString(), "")
            Dim colDebitText = If(cmbColDebit.SelectedIndex > 0, cmbColDebit.SelectedItem.ToString(), "")
            Dim colCreditText = If(cmbColCredit.SelectedIndex > 0, cmbColCredit.SelectedItem.ToString(), "")
            Dim colDescText = If(cmbColDesc.SelectedIndex > 0, cmbColDesc.SelectedItem.ToString(), "")
            Dim colPayeeText = If(cmbColPayee.SelectedIndex > 0, cmbColPayee.SelectedItem.ToString(), "")

            Try
                Sql.ExecuteNonQuery(
                    "UPDATE SoBank_1 SET HeaderRowIndex = ?, ColDate = ?, ColRef = ?, ColDebit = ?, ColCredit = ?, ColDesc = ?, ColPayee = ? " &
                    "WHERE BankID = ?",
                    Convert.ToInt32(nudHeaderRow.Value), colDateText, colRefText, colDebitText, colCreditText, colDescText, colPayeeText, bankId)

                _savedHeaderRow = Convert.ToInt32(nudHeaderRow.Value)
                _savedColDate = colDateText
                _savedColRef = colRefText
                _savedColDebit = colDebitText
                _savedColCredit = colCreditText
                _savedColDesc = colDescText
                _savedColPayee = colPayeeText
            Catch ex As Exception
            End Try

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
            btnFromDate.Enabled = rbCustomRange.Checked
            btnToDate.Enabled = rbCustomRange.Checked
            If Not rbCustomRange.Checked Then
                txtFromDate.Clear()
                txtToDate.Clear()
            End If
        End Sub

        Private Sub btnFromDate_Click(sender As Object, e As EventArgs) Handles btnFromDate.Click
            Dim anchor = EnsureOnScreen(
                txtFromDate.PointToScreen(New Point(0, txtFromDate.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtFromDate.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtFromDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Sub btnToDate_Click(sender As Object, e As EventArgs) Handles btnToDate.Click
            Dim anchor = EnsureOnScreen(
                txtToDate.PointToScreen(New Point(0, txtToDate.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtToDate.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtToDate.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, formSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - formSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - formSize.Height)))
        End Function

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

                Dim targetFiscalYearId As Integer? = Nothing
                If rbCurrentYear.Checked Then
                    targetFiscalYearId = SessionContext.CurrentFiscalYearID.Value
                End If

                Using progress As New ProgressForm()
                    progress.ShowAndCenter(Me)

                    ' Perform reconciliation using DB stored statement data
                    _recResult = recService.PerformDatabaseReconciliation(companyId, targetFiscalYearId, bankId, accountId, fromDate, toDate,
                        Sub(overall, detail, msg)
                            progress.UpdateProgress(overall, detail, msg)
                        End Sub)

                    progress.UpdateProgress(100, 100, "در حال رسم جداول و نمایش نتایج...")
                    DisplayReconciliationResults(bankId, accountId, targetFiscalYearId, fromDate, toDate)
                End Using
                Me.Cursor = Cursors.Default
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                MessageBox.Show("خطا در انجام مغایرت‌گیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DisplayReconciliationResults(bankId As Integer, accountId As Integer, targetFiscalYearId As Integer?, fromDate As DateTime?, toDate As DateTime?)
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

            ' 9. پیشنهاد برای رفع مغایرت بانک (غیرفعال شده به دلیل حذف تب مربوطه)
            ' Dim dtBankSuggestions As New DataTable()
            ' dtBankSuggestions.Columns.Add("TxID", GetType(Integer))
            ' dtBankSuggestions.Columns.Add("DetailID", GetType(Integer))
            ' dtBankSuggestions.Columns.Add("ردیف", GetType(Integer))
            ' dtBankSuggestions.Columns.Add("تاریخ بانک", GetType(String))
            ' dtBankSuggestions.Columns.Add("شماره پیگیری بانک", GetType(String))
            ' dtBankSuggestions.Columns.Add("برداشت (بدهکار) بانک", GetType(Decimal))
            ' dtBankSuggestions.Columns.Add("واریز (بستانکار) بانک", GetType(Decimal))
            ' dtBankSuggestions.Columns.Add("شرح بانک", GetType(String))
            ' dtBankSuggestions.Columns.Add("واریز کننده/ذینفع بانک", GetType(String))
            ' dtBankSuggestions.Columns.Add("سند دفتر", GetType(String))
            ' dtBankSuggestions.Columns.Add("تاریخ دفتر", GetType(String))
            ' dtBankSuggestions.Columns.Add("بدهکار دفتر", GetType(Decimal))
            ' dtBankSuggestions.Columns.Add("بستانکار دفتر", GetType(Decimal))
            ' dtBankSuggestions.Columns.Add("شماره پیگیری دفتر", GetType(String))
            ' dtBankSuggestions.Columns.Add("شرح ردیف دفتر", GetType(String))
            ' dtBankSuggestions.Columns.Add("درصد احتمال", GetType(String))

            ' Dim idxBankSugg = 1
            ' For Each sug In _recResult.Suggestions
            '     Dim bt = sug.BankTx
            '     Dim lt = sug.LedgerTx
            '     dtBankSuggestions.Rows.Add(
            '         bt.TxID, lt.DetailID, idxBankSugg,
            '         bt.TxDate, bt.RefNo, bt.Credit, bt.Debit, bt.Description, bt.Payee,
            '         lt.RefNo, Business.PersianDateHelper.ToPersian(lt.EntryDate), lt.Debit, lt.Credit, lt.TxNo, lt.Description,
            '         sug.MatchProbability.ToString("0.0") & "%"
            '     )
            '     idxBankSugg += 1
            ' Next

            ' dgvBank_Suggestions.DataSource = dtBankSuggestions
            ' FormatGrid(dgvBank_Suggestions)

            ' If dgvBank_Suggestions.Columns.Contains("TxID") Then dgvBank_Suggestions.Columns("TxID").Visible = False
            ' If dgvBank_Suggestions.Columns.Contains("DetailID") Then dgvBank_Suggestions.Columns("DetailID").Visible = False

            ' SetupSuggestionGridButtonsAndColors(dgvBank_Suggestions, True)


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

            Dim ledgerTable = recService.GetLedgerEntries(SessionContext.CurrentCompanyID.Value, targetFiscalYearId, accountId, fromDate, toDate)
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

            ' Save sorting state and vertical scroll position of suggestions grid
            Dim savedSortColName As String = ""
            Dim savedSortOrder As SortOrder = SortOrder.None
            Dim savedFirstRowIndex As Integer = -1

            If dgvAsnad_Suggestions.SortedColumn IsNot Nothing Then
                savedSortColName = dgvAsnad_Suggestions.SortedColumn.Name
                savedSortOrder = dgvAsnad_Suggestions.SortOrder
            End If

            Try
                If dgvAsnad_Suggestions.FirstDisplayedScrollingRowIndex >= 0 Then
                    savedFirstRowIndex = dgvAsnad_Suggestions.FirstDisplayedScrollingRowIndex
                End If
            Catch
            End Try

            dgvAsnad_Suggestions.DataSource = dtAsnadSuggestions
            FormatGrid(dgvAsnad_Suggestions)
            SetupAsnadGridButtonsAndEditing(dgvAsnad_Suggestions)

            If dgvAsnad_Suggestions.Columns.Contains("TxID") Then dgvAsnad_Suggestions.Columns("TxID").Visible = False
            If dgvAsnad_Suggestions.Columns.Contains("DetailID") Then dgvAsnad_Suggestions.Columns("DetailID").Visible = False
            If dgvAsnad_Suggestions.Columns.Contains("EntryID") Then dgvAsnad_Suggestions.Columns("EntryID").Visible = False

            SetupSuggestionGridButtonsAndColors(dgvAsnad_Suggestions, False)

            ' Restore sorting state of suggestions grid
            If Not String.IsNullOrEmpty(savedSortColName) AndAlso savedSortOrder <> SortOrder.None Then
                If dgvAsnad_Suggestions.Columns.Contains(savedSortColName) Then
                    Dim sortCol = dgvAsnad_Suggestions.Columns(savedSortColName)
                    Dim listSortDir = If(savedSortOrder = SortOrder.Descending, System.ComponentModel.ListSortDirection.Descending, System.ComponentModel.ListSortDirection.Ascending)
                    dgvAsnad_Suggestions.Sort(sortCol, listSortDir)
                End If
            End If

            ' Restore vertical scroll position of suggestions grid
            If savedFirstRowIndex >= 0 AndAlso savedFirstRowIndex < dgvAsnad_Suggestions.Rows.Count Then
                Try
                    dgvAsnad_Suggestions.FirstDisplayedScrollingRowIndex = savedFirstRowIndex
                Catch
                End Try
            End If

            ' --- Summary Label ---
            Dim totalMatched = _recResult.Matched.Count
            Dim totalUnmatchedBank = _recResult.UnmatchedBank.Count
            Dim totalUnmatchedLedger = _recResult.UnmatchedLedger.Count
            lblSummary.Text = String.Format("اقلام تطبیق یافته: {0} | اقلام باز بانکی: {1} | اقلام باز دفاتر: {2}", totalMatched, totalUnmatchedBank, totalUnmatchedLedger)
            UpdateBankRowCount()
            UpdateAsnadRowCount()

            Try
                Dim actualLogPath As String = System.IO.Path.Combine(Application.StartupPath, "actual_columns.txt")
                Using sw As New System.IO.StreamWriter(actualLogPath, False)
                    sw.WriteLine("=== dgvAsnad_Open Columns ===")
                    For Each col As DataGridViewColumn In dgvAsnad_Open.Columns
                        sw.WriteLine(String.Format("Name={0}, Header={1}, Index={2}, DisplayIndex={3}, Visible={4}", col.Name, col.HeaderText, col.Index, col.DisplayIndex, col.Visible))
                    Next
                End Using
            Catch
            End Try
        End Sub

        Private Sub FormatGrid(dgv As DataGridView)
            If dgv.BindingContext Is Nothing Then
                dgv.BindingContext = New BindingContext()
            End If
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250)
            dgv.RowHeadersVisible = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.MultiSelect = False
            dgv.AllowUserToResizeColumns = True

            ' Format decimal columns, align headers to center, and apply custom styles
            For Each col As DataGridViewColumn In dgv.Columns
                ' Center-align all headers
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                If col.ValueType Is GetType(Decimal) Then
                    col.DefaultCellStyle.Format = "N0"
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If

                Dim txt = col.HeaderText
                ' Soft colors: Green for Incoming (Bank's Credit / Ledger's Debit) and Pink for Outgoing (Bank's Debit / Ledger's Credit)
                ' Left-align values in Debit/Credit / Withdrawal/Deposit columns as requested
                If txt = "واریز (بستانکار)" OrElse txt = "واریز (بستانکار) بانک" OrElse txt = "بدهکار" OrElse txt = "بدهکار دفتر" Then
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 247, 235)
                    col.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(215, 240, 215)
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                ElseIf txt = "برداشت (بدهکار)" OrElse txt = "برداشت (بدهکار) بانک" OrElse txt = "بستانکار" OrElse txt = "بستانکار دفتر" Then
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 242, 242)
                    col.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(255, 225, 225)
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End If
            Next
            dgv.EnableHeadersVisualStyles = False

            ' Configure explicit widths for standard columns to align top/bottom grids perfectly

            ' 1. Prefix columns alignment
            If dgv.Columns.Contains("سند") Then
                dgv.Columns("سند").Width = 60
                dgv.Columns("سند").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If

            If dgv.Columns.Contains("ردیف") Then
                ' In Bank we make it 210px (compensates for lack of 60px "سند" + 100px "btnEditSanadCol"). In Ledger we make it 50px.
                dgv.Columns("ردیف").Width = If(dgv.Columns.Contains("سند") OrElse dgv.Columns.Contains("سند دفتر"), 50, 210)
                dgv.Columns("ردیف").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If

            If dgv.Columns.Contains("تاریخ") Then
                dgv.Columns("تاریخ").Width = 80
                dgv.Columns("تاریخ").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If

            If dgv.Columns.Contains("شماره پیگیری") Then
                ' Set both to 100px so they align perfectly
                dgv.Columns("شماره پیگیری").Width = 100
                dgv.Columns("شماره پیگیری").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If

            ' 2. Currency columns alignment (Withdrawal Bank aligns with Debit Ledger, Deposit Bank aligns with Credit Ledger)
            If dgv.Columns.Contains("برداشت (بدهکار)") Then
                dgv.Columns("برداشت (بدهکار)").Width = 120
            End If

            If dgv.Columns.Contains("واریز (بستانکار)") Then
                dgv.Columns("واریز (بستانکار)").Width = 120
            End If

            If dgv.Columns.Contains("بدهکار") Then
                dgv.Columns("بدهکار").Width = 120
            End If

            If dgv.Columns.Contains("بستانکار") Then
                dgv.Columns("بستانکار").Width = 120
            End If

            ' 3. Text columns (Description and Payee)
            If dgv.Columns.Contains("شرح") Then
                dgv.Columns("شرح").Width = 250
            End If

            If dgv.Columns.Contains("واریز کننده/ذینفع") Then
                dgv.Columns("واریز کننده/ذینفع").Width = 200
            End If

            If dgv.Columns.Contains("شرح ردیف") Then
                dgv.Columns("شرح ردیف").Width = 250
            End If

            ' Suggestion and detail specific columns
            If dgv.Columns.Contains("تاریخ بانک") Then dgv.Columns("تاریخ بانک").Width = 80
            If dgv.Columns.Contains("شماره پیگیری بانک") Then dgv.Columns("شماره پیگیری بانک").Width = 100
            If dgv.Columns.Contains("برداشت (بدهکار) بانک") Then dgv.Columns("برداشت (بدهکار) بانک").Width = 120
            If dgv.Columns.Contains("واریز (بستانکار) بانک") Then dgv.Columns("واریز (بستانکار) بانک").Width = 120
            If dgv.Columns.Contains("شرح بانک") Then dgv.Columns("شرح بانک").Width = 200
            If dgv.Columns.Contains("واریز کننده/ذینفع بانک") Then dgv.Columns("واریز کننده/ذینفع بانک").Width = 150

            If dgv.Columns.Contains("سند دفتر") Then dgv.Columns("سند دفتر").Width = 60
            If dgv.Columns.Contains("تاریخ دفتر") Then dgv.Columns("تاریخ دفتر").Width = 80
            If dgv.Columns.Contains("بدهکار دفتر") Then dgv.Columns("بدهکار دفتر").Width = 120
            If dgv.Columns.Contains("بستانکار دفتر") Then dgv.Columns("بستانکار دفتر").Width = 120
            If dgv.Columns.Contains("شماره پیگیری دفتر") Then dgv.Columns("شماره پیگیری دفتر").Width = 100
            If dgv.Columns.Contains("شرح ردیف دفتر") Then dgv.Columns("شرح ردیف دفتر").Width = 200
            If dgv.Columns.Contains("درصد احتمال") Then dgv.Columns("درصد احتمال").Width = 70

            ' 4. Apply column ordering
            ApplyColumnOrdering(dgv)

            ' 5. Setup search textboxes
            SetupSearchForGrid(dgv)
        End Sub

        Private Sub ApplyColumnOrdering(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim orderList As List(Of String) = Nothing

            ' 1. Determine the appropriate order list
            If dgv.Columns.Contains("سند دفتر") Then
                orderList = New List(Of String)({
                    "ردیف",
                    "btnResolveCol",
                    "btnEditSanadCol",
                    "سند دفتر",
                    "درصد احتمال",
                    "تاریخ دفتر",
                    "تاریخ بانک",
                    "شماره پیگیری دفتر",
                    "شماره پیگیری بانک",
                    "بدهکار دفتر",
                    "واریز (بستانکار) بانک",
                    "بستانکار دفتر",
                    "برداشت (بدهکار) بانک",
                    "واریز کننده/ذینفع بانک",
                    "شرح ردیف دفتر",
                    "شرح بانک"
                })
            ElseIf dgv.Columns.Contains("سند") Then
                orderList = New List(Of String)({
                    "ردیف", "سند", "btnEditSanadCol", "تاریخ", "شماره پیگیری", "بدهکار", "بستانکار", "شرح ردیف"
                })
            ElseIf dgv.Columns.Contains("ردیف") AndAlso Not dgv.Columns.Contains("سند") Then
                orderList = New List(Of String)({
                    "ردیف", "تاریخ", "شماره پیگیری", "برداشت (بدهکار)", "واریز (بستانکار)", "شرح", "واریز کننده/ذینفع"
                })
            End If

            If orderList Is Nothing Then Return

            Try
                ' Set display index sequentially in desired order.
                ' By assigning them directly, WinForms automatically moves other columns
                ' out of the way. We do it in ascending order, which is the most stable method.
                Dim currentIdx As Integer = 0
                For Each colName In orderList
                    If dgv.Columns.Contains(colName) Then
                        dgv.Columns(colName).DisplayIndex = currentIdx
                        currentIdx += 1
                    End If
                Next
            Catch ex As Exception
                ' Silent fallback
            End Try
        End Sub

        Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
            If _recResult Is Nothing Then
                MessageBox.Show("هیچ نتیجه مغایرت‌گیری برای خروجی وجود ندارد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using sfd As New SaveFileDialog()
                sfd.Filter = "فایل اکسل (*.xls)|*.xls|فایل CSV (*.csv)|*.csv"
                sfd.Title = "ذخیره اقلام مغایرت"
                sfd.FileName = "Reconciliation_Report_" & DateTime.Now.ToString("yyyyMMdd")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim ext = Path.GetExtension(sfd.FileName).ToLower()
                        If ext = ".csv" Then
                            ExportToCsv(sfd.FileName)
                        Else
                            ExportToExcelFile(sfd.FileName)
                        End If
                        MessageBox.Show("گزارش مغایرت با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ذخیره‌سازی فایل خروجی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub btnTransferDesc_Click(sender As Object, e As EventArgs) Handles btnTransferDesc.Click
            If _recResult Is Nothing OrElse _recResult.Matched Is Nothing OrElse _recResult.Matched.Count = 0 Then
                MessageBox.Show("هیچ نتیجه مغایرت‌گیری فعال یا اقلام تطبیق یافته‌ای یافت نشد. ابتدا دکمه تهیه مغایرت بانکی را فشار دهید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim confirmResult = MessageBox.Show(
                "آیا از انتقال شرح صورت‌حساب‌های بانکی به شرح آرتیکل‌های متناظر دفاتر که فاقد شرح هستند اطمینان دارید؟" & Environment.NewLine &
                "این عملیات روی تمامی اسناد و در تمامی سال‌های مالی اعمال خواهد شد.",
                "تایید عملیات",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign
            )

            If confirmResult <> DialogResult.Yes Then Return

            Dim count = 0
            Try
                ' 1. Process dynamically matched pairs in memory from current reconciliation
                For Each pair In _recResult.Matched
                    Dim bt = pair.BankTx
                    Dim lt = pair.LedgerTx

                    ' Query the database to get the current value of SharhRadif directly to be safe
                    Dim dbVal = Sql.ExecuteScalar("SELECT SharhRadif FROM Sanad2 WHERE DetailID = ?", lt.DetailID)
                    Dim currentSharh = If(dbVal Is DBNull.Value, "", Convert.ToString(dbVal))

                    If String.IsNullOrWhiteSpace(currentSharh) Then
                        Dim desc = If(bt.Description, "")
                        Dim payee = If(bt.Payee, "").Trim()
                        Dim fullDesc = desc & " / واریز کننده یا ذینفع  : " & payee

                        Sql.ExecuteNonQuery("UPDATE Sanad2 SET SharhRadif = ? WHERE DetailID = ?", fullDesc, lt.DetailID)
                        count += 1
                    End If
                Next

                ' 2. Process database manually matched pairs as well
                Dim dt = Sql.ExecuteTable(
                    "SELECT b.Description, b.Payee, d.DetailID, d.SharhRadif " &
                    "FROM SoBank_2 b " &
                    "INNER JOIN Sanad2 d ON b.MatchedDetailID = d.DetailID"
                )
                For Each row As DataRow In dt.Rows
                    Dim currentSharh = If(row.IsNull("SharhRadif"), "", Convert.ToString(row("SharhRadif")))
                    If String.IsNullOrWhiteSpace(currentSharh) Then
                        Dim desc = Convert.ToString(row("Description"))
                        Dim payee = If(row.IsNull("Payee"), "", Convert.ToString(row("Payee"))).Trim()
                        Dim fullDesc = desc & " / واریز کننده یا ذینفع  : " & payee
                        Dim detailId = Convert.ToInt32(row("DetailID"))

                        Sql.ExecuteNonQuery("UPDATE Sanad2 SET SharhRadif = ? WHERE DetailID = ?", fullDesc, detailId)
                        count += 1
                    End If
                Next

                If count > 0 Then
                    MessageBox.Show(
                        String.Format("انتقال شرح با موفقیت انجام شد. تعداد {0} آرتیکل سند به‌روزرسانی گردید.", count),
                        "موفقیت",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    )
                    ' Refresh the reconciliation grids
                    btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)
                Else
                    MessageBox.Show("هیچ آرتیکل سند بسته‌ای با شرح ردیف خالی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در انتقال شرح صورتحساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
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
            btnEditSanad.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns.Add(btnEditSanad)


            For Each col As DataGridViewColumn In dgv.Columns
                If col.Name = "شماره پیگیری" OrElse col.Name = "شماره پیگیری دفتر" Then
                    col.ReadOnly = False
                Else
                    col.ReadOnly = True
                End If
            Next
            ApplyColumnOrdering(dgv)
            SetupSearchForGrid(dgv)
        End Sub

        Private Sub dgvAsnad_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAsnad_All.CellContentClick, dgvAsnad_Open.CellContentClick, dgvAsnad_OpenDebit.CellContentClick, dgvAsnad_OpenCredit.CellContentClick, dgvAsnad_Closed.CellContentClick, dgvAsnad_ClosedDebit.CellContentClick, dgvAsnad_ClosedCredit.CellContentClick, dgvAsnad_Dup.CellContentClick
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
                            detailsForm.WindowState = FormWindowState.Maximized
                            detailsForm.ShowDialog(Me)
                        End Using

                        btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)

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
                        Sql.ExecuteNonQuery("UPDATE Sanad2 SET TransactionNumber = ? WHERE DetailID = ?", newVal, detailId)
                        Me.BeginInvoke(New Action(Sub()
                                                      btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)
                                                  End Sub))
                    Catch ex As Exception
                        MessageBox.Show("خطا در ویرایش شماره پیگیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub dgvAsnad_Suggestions_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvAsnad_Suggestions.KeyDown
            Dim dgv = DirectCast(sender, DataGridView)
            If dgv.CurrentCell Is Nothing Then Return

            Dim colName = dgv.Columns(dgv.CurrentCell.ColumnIndex).Name

            ' Ctrl + C: Copy cell value to Clipboard
            If e.Control AndAlso e.KeyCode = Keys.C Then
                If dgv.CurrentCell.Value IsNot Nothing Then
                    Try
                        Clipboard.SetText(dgv.CurrentCell.Value.ToString())
                        e.Handled = True
                    Catch ex As Exception
                    End Try
                End If
            End If

            ' Ctrl + V: Paste Clipboard text to Cell
            If e.Control AndAlso e.KeyCode = Keys.V Then
                If colName = "شماره پیگیری دفتر" Then
                    Try
                        Dim clipText = Clipboard.GetText().Trim()
                        Dim detailId = Convert.ToInt32(dgv.Rows(dgv.CurrentCell.RowIndex).Cells("DetailID").Value)
                        If detailId > 0 Then
                            ' Update cell value visually
                            dgv.CurrentCell.Value = clipText
                            ' Update database immediately
                            Sql.ExecuteNonQuery("UPDATE Sanad2 SET TransactionNumber = ? WHERE DetailID = ?", clipText, detailId)
                            ' Refresh reconciliation
                            btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)
                            e.Handled = True
                        End If
                    Catch ex As Exception
                        MessageBox.Show("خطا در چسباندن شماره پیگیری: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        Private Function EscapeXml(val As String) As String
            If String.IsNullOrEmpty(val) Then Return ""
            Return val.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("""", "&quot;").Replace("'", "&apos;")
        End Function

        Private Sub ExportToExcelFile(filePath As String)
            Dim sb As New StringBuilder()

            sb.AppendLine("<?xml version=""1.0""?>")
            sb.AppendLine("<?mso-application progid=""Excel.Sheet""?>")
            sb.AppendLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""")
            sb.AppendLine(" xmlns:o=""urn:schemas-microsoft-com:office:office""")
            sb.AppendLine(" xmlns:x=""urn:schemas-microsoft-com:office:excel""")
            sb.AppendLine(" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""")
            sb.AppendLine(" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            sb.AppendLine(" <DocumentProperties xmlns=""urn:schemas-microsoft-com:office:office"">")
            sb.AppendLine("  <Author>System</Author>")
            sb.AppendLine("  <Created>" & DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") & "</Created>")
            sb.AppendLine(" </DocumentProperties>")

            sb.AppendLine(" <Styles>")
            sb.AppendLine("  <Style ss:ID=""Default"" ss:Name=""Normal"">")
            sb.AppendLine("   <Alignment ss:Vertical=""Bottom""/>")
            sb.AppendLine("   <Font ss:FontName=""Tahoma"" x:CharSet=""178"" ss:Size=""9""/>")
            sb.AppendLine("  </Style>")

            ' Header Style
            sb.AppendLine("  <Style ss:ID=""sHeader"">")
            sb.AppendLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>")
            sb.AppendLine("   <Font ss:FontName=""Tahoma"" x:CharSet=""178"" ss:Size=""10"" ss:Bold=""1""/>")
            sb.AppendLine("   <Interior ss:Color=""#ECF0F1"" ss:Pattern=""Solid""/>")
            sb.AppendLine("   <Borders>")
            sb.AppendLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#BDC3C7""/>")
            sb.AppendLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#BDC3C7""/>")
            sb.AppendLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#BDC3C7""/>")
            sb.AppendLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#BDC3C7""/>")
            sb.AppendLine("   </Borders>")
            sb.AppendLine("  </Style>")

            ' Title Style
            sb.AppendLine("  <Style ss:ID=""sTitle"">")
            sb.AppendLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>")
            sb.AppendLine("   <Font ss:FontName=""Tahoma"" x:CharSet=""178"" ss:Size=""12"" ss:Bold=""1"" ss:Color=""#2C3E50""/>")
            sb.AppendLine("  </Style>")

            ' Data Style
            sb.AppendLine("  <Style ss:ID=""sData"">")
            sb.AppendLine("   <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""/>")
            sb.AppendLine("   <Borders>")
            sb.AppendLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("   </Borders>")
            sb.AppendLine("  </Style>")

            ' Text / Code Style (keeps leading zeros)
            sb.AppendLine("  <Style ss:ID=""sText"">")
            sb.AppendLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>")
            sb.AppendLine("   <NumberFormat ss:Format=""@""/>")
            sb.AppendLine("   <Borders>")
            sb.AppendLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("   </Borders>")
            sb.AppendLine("  </Style>")

            ' Numeric Style
            sb.AppendLine("  <Style ss:ID=""sNum"">")
            sb.AppendLine("   <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""/>")
            sb.AppendLine("   <NumberFormat ss:Format=""#,##0""/>")
            sb.AppendLine("   <Borders>")
            sb.AppendLine("    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#E5E7E9""/>")
            sb.AppendLine("   </Borders>")
            sb.AppendLine("  </Style>")
            sb.AppendLine(" </Styles>")

            ' ==========================================
            ' Sheet 1: اقلام باز بانک - غایب در دفاتر
            ' ==========================================
            sb.AppendLine(" <Worksheet ss:Name=""اقلام باز بانک - غایب در دفاتر"">")
            sb.AppendLine("  <Table>")
            ' Title row
            sb.AppendLine("   <Row ss:Height=""24"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""6"" ss:StyleID=""sTitle""><Data ss:Type=""String"">گزارش مغایرت بانکی - اقلام باز بانک (غایب در دفاتر)</Data></Cell>")
            sb.AppendLine("   </Row>")
            ' Info rows
            sb.AppendLine("   <Row ss:Height=""18"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""6""><Data ss:Type=""String"">بانک انتخاب شده: " & EscapeXml(cmbRecBank.Text) & "</Data></Cell>")
            sb.AppendLine("   </Row>")
            sb.AppendLine("   <Row ss:Height=""18"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""6""><Data ss:Type=""String"">تاریخ مغایرت‌گیری: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm") & "</Data></Cell>")
            sb.AppendLine("   </Row>")
            sb.AppendLine("   <Row ss:Height=""10""><Cell ss:MergeAcross=""6""/></Row>") ' Empty row

            ' Header row
            sb.AppendLine("   <Row ss:Height=""22"">")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">ردیف</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">تاریخ</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">شماره پیگیری</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">مبلغ واریز</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">مبلغ برداشت</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">شرح</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">واریز کننده/ذینفع</Data></Cell>")
            sb.AppendLine("   </Row>")

            For i As Integer = 0 To _recResult.UnmatchedBank.Count - 1
                Dim bt = _recResult.UnmatchedBank(i)
                sb.AppendLine("   <Row ss:Height=""20"">")
                sb.AppendLine("    <Cell ss:StyleID=""sData""><Data ss:Type=""Number"">" & (i + 1) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sText""><Data ss:Type=""String"">" & EscapeXml(bt.TxDate) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sText""><Data ss:Type=""String"">" & EscapeXml(bt.RefNo) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sNum""><Data ss:Type=""Number"">" & bt.Debit & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sNum""><Data ss:Type=""Number"">" & bt.Credit & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sData""><Data ss:Type=""String"">" & EscapeXml(bt.Description) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sData""><Data ss:Type=""String"">" & EscapeXml(bt.Payee) & "</Data></Cell>")
                sb.AppendLine("   </Row>")
            Next
            sb.AppendLine("  </Table>")
            sb.AppendLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            sb.AppendLine("   <DisplayRightToLeft/>")
            sb.AppendLine("  </WorksheetOptions>")
            sb.AppendLine(" </Worksheet>")

            ' ==========================================
            ' Sheet 2: اقلام باز دفاتر- غایب در بانک
            ' ==========================================
            sb.AppendLine(" <Worksheet ss:Name=""اقلام باز دفاتر- غایب در بانک"">")
            sb.AppendLine("  <Table>")
            ' Title row
            sb.AppendLine("   <Row ss:Height=""24"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""5"" ss:StyleID=""sTitle""><Data ss:Type=""String"">گزارش مغایرت بانکی - اقلام باز دفاتر (غایب در بانک)</Data></Cell>")
            sb.AppendLine("   </Row>")
            ' Info rows
            sb.AppendLine("   <Row ss:Height=""18"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""5""><Data ss:Type=""String"">بانک انتخاب شده: " & EscapeXml(cmbRecBank.Text) & "</Data></Cell>")
            sb.AppendLine("   </Row>")
            sb.AppendLine("   <Row ss:Height=""18"">")
            sb.AppendLine("    <Cell ss:MergeAcross=""5""><Data ss:Type=""String"">تاریخ مغایرت‌گیری: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm") & "</Data></Cell>")
            sb.AppendLine("   </Row>")
            sb.AppendLine("   <Row ss:Height=""10""><Cell ss:MergeAcross=""5""/></Row>") ' Empty row

            ' Header row
            sb.AppendLine("   <Row ss:Height=""22"">")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">سند</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">تاریخ سند</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">بدهکار (واریز)</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">بستانکار (برداشت)</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">شماره پیگیری دفاتر</Data></Cell>")
            sb.AppendLine("    <Cell ss:StyleID=""sHeader""><Data ss:Type=""String"">شرح سند</Data></Cell>")
            sb.AppendLine("   </Row>")

            For Each lt In _recResult.UnmatchedLedger
                sb.AppendLine("   <Row ss:Height=""20"">")
                sb.AppendLine("    <Cell ss:StyleID=""sText""><Data ss:Type=""String"">" & EscapeXml(lt.RefNo) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sText""><Data ss:Type=""String"">" & EscapeXml(Business.PersianDateHelper.ToPersian(lt.EntryDate)) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sNum""><Data ss:Type=""Number"">" & lt.Debit & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sNum""><Data ss:Type=""Number"">" & lt.Credit & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sText""><Data ss:Type=""String"">" & EscapeXml(lt.TxNo) & "</Data></Cell>")
                sb.AppendLine("    <Cell ss:StyleID=""sData""><Data ss:Type=""String"">" & EscapeXml(lt.Description) & "</Data></Cell>")
                sb.AppendLine("   </Row>")
            Next
            sb.AppendLine("  </Table>")
            sb.AppendLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            sb.AppendLine("   <DisplayRightToLeft/>")
            sb.AppendLine("  </WorksheetOptions>")
            sb.AppendLine(" </Worksheet>")

            sb.AppendLine("</Workbook>")

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
            ApplyColumnOrdering(dgv)
            SetupSearchForGrid(dgv)
        End Sub

        Private Sub dgvSuggestions_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAsnad_Suggestions.CellContentClick
            If e.RowIndex < 0 Then Return

            Dim senderGrid = DirectCast(sender, DataGridView)
            Dim colName = senderGrid.Columns(e.ColumnIndex).Name

            If colName = "btnEditSanadCol" Then
                ' --- Go to Sanad ---
                Dim entryId As Integer = 0
                Dim detailId As Integer = 0
                Try
                    entryId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("EntryID").Value)
                    detailId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("DetailID").Value)
                Catch
                    Return
                End Try

                If entryId > 0 Then
                    Try
                        Using detailsForm As New HesabdarySanad2Form(entryId)
                            detailsForm.HighlightDetailID = detailId
                            detailsForm.WindowState = FormWindowState.Maximized
                            detailsForm.ShowDialog(Me)
                        End Using
                        btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)
                    Catch ex As Exception
                        MessageBox.Show("خطا در باز کردن سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If

            ElseIf colName = "btnResolveCol" Then
                ' --- Resolve Discrepancy ---
                Dim txId As Integer = 0
                Dim detailId As Integer = 0
                Try
                    txId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("TxID").Value)
                    detailId = Convert.ToInt32(senderGrid.Rows(e.RowIndex).Cells("DetailID").Value)
                Catch
                    Return
                End Try

                Dim confirm = MessageBox.Show("آیا از ثبت این مورد به عنوان مغایرت رفع‌شده و تطبیق آن‌ها اطمینان دارید؟", "تایید رفع مغایرت", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirm = DialogResult.Yes Then
                    Try
                        Dim bankRefNoObj = Sql.ExecuteScalar("SELECT RefNo FROM SoBank_2 WHERE TxID = ?", txId)
                        Dim bankRefNo As String = If(bankRefNoObj IsNot Nothing AndAlso Not Convert.IsDBNull(bankRefNoObj), Convert.ToString(bankRefNoObj), "")
                        Sql.ExecuteNonQuery("UPDATE SoBank_2 SET MatchedDetailID = ? WHERE TxID = ?", detailId, txId)
                        Sql.ExecuteNonQuery("UPDATE Sanad2 SET TransactionNumber = ? WHERE DetailID = ?", bankRefNo, detailId)
                        MessageBox.Show("مغایرت با موفقیت رفع شد و تراکنش‌ها تطبیق یافتند.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        btnRunReconciliation_Click(btnRunReconciliation, EventArgs.Empty)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ثبت رفع مغایرت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

        Private Sub tcTabControls_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcAsnad.SelectedIndexChanged, tcBank.SelectedIndexChanged, tcMain.SelectedIndexChanged
            Dim tc = DirectCast(sender, TabControl)
            If tc.SelectedTab IsNot Nothing Then
                ' Align grids directly inside this tab
                For Each ctrl As Control In tc.SelectedTab.Controls
                    If TypeOf ctrl Is DataGridView Then
                        Dim dgv = DirectCast(ctrl, DataGridView)
                        ApplyColumnOrdering(dgv)
                        AlignSearchControlsForGrid(dgv)
                    End If
                Next
            End If

            ' If the main tab changed, also align the currently selected bank and ledger grids
            If sender Is tcMain Then
                UpdateBankRowCount()
                UpdateAsnadRowCount()
                ' Align bank grid in currently selected bank tab
                If tcBank IsNot Nothing AndAlso tcBank.SelectedTab IsNot Nothing Then
                    For Each ctrl As Control In tcBank.SelectedTab.Controls
                        If TypeOf ctrl Is DataGridView Then
                            AlignSearchControlsForGrid(DirectCast(ctrl, DataGridView))
                        End If
                    Next
                End If
                ' Align ledger grid in currently selected asnad tab
                If tcAsnad IsNot Nothing AndAlso tcAsnad.SelectedTab IsNot Nothing Then
                    For Each ctrl As Control In tcAsnad.SelectedTab.Controls
                        If TypeOf ctrl Is DataGridView Then
                            AlignSearchControlsForGrid(DirectCast(ctrl, DataGridView))
                        End If
                    Next
                End If
            End If
        End Sub

        Private Sub HesabdaryMogBankForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            If tcBank.SelectedTab IsNot Nothing Then
                For Each ctrl As Control In tcBank.SelectedTab.Controls
                    If TypeOf ctrl Is DataGridView Then
                        AlignSearchControlsForGrid(DirectCast(ctrl, DataGridView))
                    End If
                Next
            End If
            If tcAsnad.SelectedTab IsNot Nothing Then
                For Each ctrl As Control In tcAsnad.SelectedTab.Controls
                    If TypeOf ctrl Is DataGridView Then
                        AlignSearchControlsForGrid(DirectCast(ctrl, DataGridView))
                    End If
                Next
            End If
        End Sub

        Private Sub btnBankStatementReport_Click(sender As Object, e As EventArgs) Handles btnBankStatementReport.Click
            Dim activeBankID As Integer = 0
            If tcMain.SelectedTab Is tpIntroBanks Then
                activeBankID = _selectedBankID
            ElseIf tcMain.SelectedTab Is tpImportStatement Then
                If cmbImportBank.SelectedValue IsNot Nothing AndAlso Not Convert.IsDBNull(cmbImportBank.SelectedValue) Then
                    activeBankID = Convert.ToInt32(cmbImportBank.SelectedValue)
                End If
            ElseIf tcMain.SelectedTab Is tpReconciliation Then
                If cmbRecBank.SelectedValue IsNot Nothing AndAlso Not Convert.IsDBNull(cmbRecBank.SelectedValue) Then
                    activeBankID = Convert.ToInt32(cmbRecBank.SelectedValue)
                End If
            End If

            If activeBankID = 0 Then
                MessageBox.Show("لطفاً ابتدا یک بانک را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim fromDateStr As String = ""
            Dim toDateStr As String = ""

            Using dlg As New BankStatementReportRangeForm()
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    fromDateStr = dlg.FromDate
                    toDateStr = dlg.ToDate
                Else
                    Return
                End If
            End Using

            ' If both dates are empty, fetch the min/max dates from database across all years
            If String.IsNullOrEmpty(fromDateStr) AndAlso String.IsNullOrEmpty(toDateStr) Then
                Try
                    Dim dtDates = Sql.ExecuteTable("SELECT MIN(TxDate), MAX(TxDate) FROM SoBank_2 WHERE BankID = ?", activeBankID)
                    If dtDates.Rows.Count > 0 Then
                        Dim minVal = dtDates.Rows(0)(0)
                        Dim maxVal = dtDates.Rows(0)(1)
                        If minVal IsNot Nothing AndAlso Not Convert.IsDBNull(minVal) Then fromDateStr = Convert.ToString(minVal)
                        If maxVal IsNot Nothing AndAlso Not Convert.IsDBNull(maxVal) Then toDateStr = Convert.ToString(maxVal)
                    End If
                Catch ex As Exception
                End Try
            End If

            If String.IsNullOrEmpty(fromDateStr) OrElse String.IsNullOrEmpty(toDateStr) Then
                MessageBox.Show("صورت حسابی برای این بانک یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Try
                ' 1. Calculate prior balance (ledger logic)
                Dim priorDebit As Decimal = 0D
                Dim priorCredit As Decimal = 0D
                Dim priorObj = Sql.ExecuteTable(
                    "SELECT SUM(Debit), SUM(Credit) FROM SoBank_2 WHERE BankID = ? AND TxDate < ?",
                    activeBankID, fromDateStr)
                If priorObj.Rows.Count > 0 Then
                    priorDebit = If(priorObj.Rows(0)(0) Is DBNull.Value, 0D, Convert.ToDecimal(priorObj.Rows(0)(0)))
                    priorCredit = If(priorObj.Rows(0)(1) Is DBNull.Value, 0D, Convert.ToDecimal(priorObj.Rows(0)(1)))
                End If

                Dim beginningBalance = priorDebit - priorCredit

                ' 2. Retrieve transactions within date range
                Dim dt = Sql.ExecuteTable(
                    "SELECT TxDate, RefNo, Debit, Credit, Description, Payee FROM SoBank_2 " &
                    "WHERE BankID = ? AND TxDate >= ? AND TxDate <= ? " &
                    "ORDER BY TxDate, TxID", activeBankID, fromDateStr, toDateStr)

                Dim printRows As New List(Of HesabdaryDaftarPrintForm.LedgerRowInfo)()

                ' Beginning Balance Row
                Dim startRow As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                startRow.EntryDate = fromDateStr
                startRow.Description = "منقول از قبل"
                startRow.DebitAmount = If(beginningBalance > 0, beginningBalance, CType(Nothing, Decimal?))
                startRow.CreditAmount = If(beginningBalance < 0, Math.Abs(beginningBalance), CType(Nothing, Decimal?))
                startRow.BalanceAmount = Math.Abs(beginningBalance)
                startRow.Tashkhis = If(beginningBalance > 0, "بد", If(beginningBalance < 0, "بس", "تراز"))
                startRow.IsHeader = True
                printRows.Add(startRow)

                Dim runningBalance = beginningBalance
                Dim totalDebit = 0D
                Dim totalCredit = 0D

                For Each row As DataRow In dt.Rows
                    Dim txDate = Convert.ToString(row("TxDate"))
                    Dim refNo = Convert.ToString(row("RefNo"))
                    Dim debit = If(row("Debit") Is DBNull.Value, 0D, Convert.ToDecimal(row("Debit")))
                    Dim credit = If(row("Credit") Is DBNull.Value, 0D, Convert.ToDecimal(row("Credit")))
                    Dim desc = Convert.ToString(row("Description"))
                    Dim payee = Convert.ToString(row("Payee"))

                    Dim fullDesc = desc
                    If Not String.IsNullOrEmpty(payee) Then
                        fullDesc &= " - " & payee
                    End If

                    runningBalance += (debit - credit)
                    totalDebit += debit
                    totalCredit += credit

                    Dim rInfo As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                    rInfo.EntryDate = txDate
                    rInfo.RefNo = refNo
                    rInfo.Description = fullDesc
                    rInfo.DebitAmount = If(debit > 0, debit, CType(Nothing, Decimal?))
                    rInfo.CreditAmount = If(credit > 0, credit, CType(Nothing, Decimal?))
                    rInfo.BalanceAmount = Math.Abs(runningBalance)
                    rInfo.Tashkhis = If(runningBalance > 0, "بد", If(runningBalance < 0, "بس", "تراز"))
                    printRows.Add(rInfo)
                Next

                ' Summary Row
                Dim totalRow As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                totalRow.Description = "جمع کل"
                totalRow.DebitAmount = totalDebit
                totalRow.CreditAmount = totalCredit
                totalRow.BalanceAmount = Math.Abs(runningBalance)
                totalRow.Tashkhis = If(runningBalance > 0, "بد", If(runningBalance < 0, "بس", "تراز"))
                totalRow.IsSummary = True
                printRows.Add(totalRow)

                ' Get bank description for sheet title
                Dim bankInfo = ""
                Dim dtBank = Sql.ExecuteTable("SELECT BankName, AccountNumber FROM SoBank_1 WHERE BankID = ?", activeBankID)
                If dtBank.Rows.Count > 0 Then
                    bankInfo = Convert.ToString(dtBank.Rows(0)("BankName")) & " - " & Convert.ToString(dtBank.Rows(0)("AccountNumber"))
                End If

                Using printForm As New HesabdaryDaftarPrintForm(
                    "گزارش صورت حساب بانکی",
                    bankInfo,
                    printRows,
                    totalDebit,
                    totalCredit,
                    Math.Abs(runningBalance),
                    If(runningBalance > 0, "بدهکار", If(runningBalance < 0, "بستانکار", "تراز")))
                    printForm.ShowDialog(Me)
                End Using

            Catch ex As Exception
                MessageBox.Show("خطا در ایجاد گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Class BankStatementReportRangeForm
            Inherits Form

            Public Property FromDate As String = ""
            Public Property ToDate As String = ""

            Private txtFromDate As TextBox
            Private txtToDate As TextBox
            Private btnCalFrom As Button
            Private btnCalTo As Button
            Private btnOK As Button
            Private btnCancel As Button

            Public Sub New()
                Me.Text = "بازه گزارش صورتحساب بانکی"
                Me.Font = New Font("Tahoma", 9.0!)
                Me.Size = New Size(320, 170)
                Me.FormBorderStyle = FormBorderStyle.FixedDialog
                Me.MaximizeBox = False
                Me.MinimizeBox = False
                Me.StartPosition = FormStartPosition.CenterParent
                Me.RightToLeft = RightToLeft.Yes
                Me.RightToLeftLayout = True

                Dim lblFrom As New Label()
                lblFrom.Text = "از تاریخ:"
                lblFrom.Location = New Point(20, 20)
                lblFrom.Size = New Size(70, 20)

                txtFromDate = New TextBox()
                txtFromDate.Location = New Point(100, 17)
                txtFromDate.Size = New Size(120, 22)
                txtFromDate.RightToLeft = RightToLeft.No

                btnCalFrom = New Button()
                btnCalFrom.Text = "📅"
                btnCalFrom.Location = New Point(225, 16)
                btnCalFrom.Size = New Size(30, 24)
                btnCalFrom.FlatStyle = FlatStyle.Flat
                btnCalFrom.BackColor = Color.White
                AddHandler btnCalFrom.Click, AddressOf BtnCalFrom_Click

                Dim lblTo As New Label()
                lblTo.Text = "تا تاریخ:"
                lblTo.Location = New Point(20, 50)
                lblTo.Size = New Size(70, 20)

                txtToDate = New TextBox()
                txtToDate.Location = New Point(100, 47)
                txtToDate.Size = New Size(120, 22)
                txtToDate.RightToLeft = RightToLeft.No

                btnCalTo = New Button()
                btnCalTo.Text = "📅"
                btnCalTo.Location = New Point(225, 46)
                btnCalTo.Size = New Size(30, 24)
                btnCalTo.FlatStyle = FlatStyle.Flat
                btnCalTo.BackColor = Color.White
                AddHandler btnCalTo.Click, AddressOf BtnCalTo_Click

                btnOK = New Button()
                btnOK.Text = "تأیید"
                btnOK.Location = New Point(60, 95)
                btnOK.Size = New Size(80, 28)
                btnOK.DialogResult = DialogResult.OK
                AddHandler btnOK.Click, AddressOf BtnOK_Click

                btnCancel = New Button()
                btnCancel.Text = "انصراف"
                btnCancel.Location = New Point(160, 95)
                btnCancel.Size = New Size(80, 28)
                btnCancel.DialogResult = DialogResult.Cancel

                Me.Controls.Add(lblFrom)
                Me.Controls.Add(txtFromDate)
                Me.Controls.Add(btnCalFrom)
                Me.Controls.Add(lblTo)
                Me.Controls.Add(txtToDate)
                Me.Controls.Add(btnCalTo)
                Me.Controls.Add(btnOK)
                Me.Controls.Add(btnCancel)

                Me.AcceptButton = btnOK
                Me.CancelButton = btnCancel
            End Sub

            Private Sub BtnCalFrom_Click(sender As Object, e As EventArgs)
                Using cal As New PersianCalendarForm(txtFromDate.Text)
                    cal.StartPosition = FormStartPosition.CenterParent
                    If cal.ShowDialog(Me) = DialogResult.OK Then
                        txtFromDate.Text = cal.SelectedDate
                    End If
                End Using
            End Sub

            Private Sub BtnCalTo_Click(sender As Object, e As EventArgs)
                Using cal As New PersianCalendarForm(txtToDate.Text)
                    cal.StartPosition = FormStartPosition.CenterParent
                    If cal.ShowDialog(Me) = DialogResult.OK Then
                        txtToDate.Text = cal.SelectedDate
                    End If
                End Using
            End Sub

            Private Sub BtnOK_Click(sender As Object, e As EventArgs)
                Me.FromDate = txtFromDate.Text.Trim()
                Me.ToDate = txtToDate.Text.Trim()
                Me.Close()
            End Sub
        End Class

        Private Class GridSearchInfo
            Public SearchPanel As Panel
            Public SearchTextBoxes As New Dictionary(Of String, TextBox)()
        End Class

        Private Sub SetupSearchForGrid(dgv As DataGridView)
            If dgv Is Nothing Then Return

            Dim info = TryCast(dgv.Tag, GridSearchInfo)
            If info Is Nothing Then
                info = New GridSearchInfo()
                dgv.Tag = info
            End If

            If info.SearchPanel Is Nothing Then
                info.SearchPanel = New Panel()
                info.SearchPanel.Height = 22
                info.SearchPanel.Dock = DockStyle.Top
                info.SearchPanel.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
                info.SearchPanel.RightToLeft = RightToLeft.Yes

                Dim parent = dgv.Parent
                If parent IsNot Nothing Then
                    parent.Controls.Add(info.SearchPanel)
                    info.SearchPanel.SendToBack()
                    dgv.BringToFront()
                End If

                AddHandler dgv.Scroll, Sub(s, ev) AlignSearchControlsForGrid(dgv)
                AddHandler dgv.ColumnWidthChanged, Sub(s, ev) AlignSearchControlsForGrid(dgv)
                AddHandler dgv.Resize, Sub(s, ev) AlignSearchControlsForGrid(dgv)
            End If

            info.SearchPanel.Controls.Clear()
            info.SearchTextBoxes.Clear()

            For Each col As DataGridViewColumn In dgv.Columns
                If col.Name = "DetailID" OrElse col.Name = "EntryID" OrElse col.Name = "TxID" OrElse col.Name = "btnEditSanadCol" OrElse col.Name = "btnResolveCol" Then
                    Continue For
                End If

                Dim txt As New TextBox()
                txt.Font = New System.Drawing.Font("Tahoma", 8.5!)
                txt.Height = 20
                txt.TextAlign = HorizontalAlignment.Center
                txt.BorderStyle = BorderStyle.FixedSingle

                AddHandler txt.TextChanged, Sub(s, ev) ApplyGridFilter(dgv)

                info.SearchTextBoxes(col.Name) = txt
                info.SearchPanel.Controls.Add(txt)
            Next

            AlignSearchControlsForGrid(dgv)
        End Sub

        Private Sub AlignSearchControlsForGrid(dgv As DataGridView)
            Dim info = TryCast(dgv.Tag, GridSearchInfo)
            If info Is Nothing OrElse info.SearchPanel Is Nothing Then Return

            Try
                For Each col As DataGridViewColumn In dgv.Columns
                    Dim txt As TextBox = Nothing
                    If info.SearchTextBoxes.TryGetValue(col.Name, txt) Then
                        If col.Visible Then
                            Dim colRect = dgv.GetCellDisplayRectangle(col.Index, -1, True)
                            If colRect.Width > 0 Then
                                txt.Left = colRect.Left
                                txt.Width = colRect.Width
                                txt.Visible = True
                            Else
                                txt.Visible = False
                            End If
                        Else
                            txt.Visible = False
                        End If
                    End If
                Next
            Catch
            End Try
        End Sub

        Private Sub ApplyGridFilter(dgv As DataGridView)
            Dim info = TryCast(dgv.Tag, GridSearchInfo)
            If info Is Nothing Then Return

            ' Support both DataTable and BindingSource DataSource
            Dim dt As DataTable = Nothing
            If TypeOf dgv.DataSource Is DataTable Then
                dt = DirectCast(dgv.DataSource, DataTable)
            ElseIf TypeOf dgv.DataSource Is BindingSource Then
                Dim bs = DirectCast(dgv.DataSource, BindingSource)
                If TypeOf bs.DataSource Is DataTable Then
                    dt = DirectCast(bs.DataSource, DataTable)
                End If
            End If
            If dt Is Nothing Then Return

            Dim filters As New List(Of String)()
            For Each kvp In info.SearchTextBoxes
                Dim colName = kvp.Key
                Dim txtVal = kvp.Value.Text.Trim().Replace("'", "''")
                If Not String.IsNullOrEmpty(txtVal) Then
                    If dt.Columns.Contains(colName) Then
                        Dim dtCol = dt.Columns(colName)
                        ' Escape column name for RowFilter - bracket it
                        Dim safeName = "[" & colName & "]"
                        If dtCol.DataType Is GetType(Decimal) OrElse dtCol.DataType Is GetType(Integer) OrElse
                           dtCol.DataType Is GetType(Double) OrElse dtCol.DataType Is GetType(Long) Then
                            ' For numeric columns: try Convert to string; if column name has
                            ' parentheses that may confuse the RowFilter expression engine,
                            ' we rename internally by using a computed column approach.
                            ' Simpler: just convert value to text and do substring match.
                            filters.Add("Convert(" & safeName & ", 'System.String') LIKE '%" & txtVal & "%'")
                        Else
                            filters.Add(safeName & " LIKE '%" & txtVal & "%'")
                        End If
                    End If
                End If
            Next

            ' Apply filters one at a time to avoid a combined expression failure
            Dim appliedFilter As String = ""
            Try
                If filters.Count > 0 Then
                    ' Try all at once first
                    Dim combined = String.Join(" AND ", filters)
                    Try
                        dt.DefaultView.RowFilter = combined
                        appliedFilter = combined
                    Catch
                        ' If combined fails, try each filter individually and combine what works
                        Dim workingFilters As New List(Of String)()
                        For Each f In filters
                            Try
                                dt.DefaultView.RowFilter = f
                                workingFilters.Add(f)
                            Catch
                            End Try
                        Next
                        If workingFilters.Count > 0 Then
                            appliedFilter = String.Join(" AND ", workingFilters)
                            dt.DefaultView.RowFilter = appliedFilter
                        Else
                            dt.DefaultView.RowFilter = ""
                        End If
                    End Try
                Else
                    dt.DefaultView.RowFilter = ""
                End If

                UpdateBankRowCount()
                UpdateAsnadRowCount()
            Catch ex As Exception
                Try
                    dt.DefaultView.RowFilter = ""
                Catch
                End Try
            End Try
        End Sub
    End Class
End Namespace
