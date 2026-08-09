Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Partial Class HesabdaryDaftarForm
        Inherits Form

        Public Event EditDocumentRequested(entryId As Integer, lineNumber As Integer?)

        Private ReadOnly service As New AccountingService()

        Private _currentAccountId As Integer
        Private _currentAccountCode As String
        Private _currentAccountName As String
        Private _currentHasChildren As Boolean
        Private _currentAllIds As List(Of Integer)
        Private _fullDataTable As DataTable
        Private _priorSums As Tuple(Of Decimal, Decimal)
        Private _selectedRangeAccounts As New List(Of Tuple(Of Integer, String, String))()
        Private _selectedFromChain As String = String.Empty
        Private _selectedToChain As String = String.Empty
        Private _returnTargetEntryID As Integer? = Nothing
        Private _returnTargetLineNumber As Integer? = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryDaftarForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            Negar.Business.ThemeHelper.AppendStatusBar(Me)
            If Me.dgvLedger IsNot Nothing Then Me.dgvLedger.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            dgvLedger.RowTemplate.Height = 26
            cmbDescType.SelectedIndex = 0 ' پیش‌فرض: فقط شرح ردیف
            cmbStatus.SelectedIndex = 0 ' پیش‌فرض: موقت
            cmbSelectedAccounts.Items.Clear()
            cmbSelectedAccounts.Items.Add("چاپ تمام دفاتر")
            cmbSelectedAccounts.SelectedIndex = 0

            SetupRangeFilterUI()
        End Sub

        Private Sub SetupRangeFilterUI()
            ' ۱. مخفی کردن چک‌باکس‌های فیلتر تاریخ و سند
            chkFilterByDate.Visible = False
            chkFilterByDoc.Visible = False

            ' ۲. ایجاد و طراحی عنوان
            Dim lblRangeMethodTitle As New Label()
            lblRangeMethodTitle.Text = "مبنای گزارش:"
            lblRangeMethodTitle.Location = New Point(970, 12)
            lblRangeMethodTitle.Size = New Size(90, 18)
            lblRangeMethodTitle.TextAlign = ContentAlignment.MiddleLeft
            lblRangeMethodTitle.Font = New Font("Tahoma", 9.0!)

            ' ۳. ایجاد کامبوباکس انتخاب مبنا
            Dim cmbRangeMethod As New ComboBox()
            cmbRangeMethod.Name = "cmbRangeMethod"
            cmbRangeMethod.DropDownStyle = ComboBoxStyle.DropDownList
            cmbRangeMethod.Location = New Point(710, 9)
            cmbRangeMethod.Size = New Size(250, 22)
            cmbRangeMethod.DropDownWidth = 270
            cmbRangeMethod.Font = New Font("Tahoma", 9.0!)
            cmbRangeMethod.Items.Add("بر اساس شماره سند در سال جاری")
            cmbRangeMethod.Items.Add("بر اساس تاریخ")
            cmbRangeMethod.Items.Add("بر اساس تمام اسناد در تمام سالهای مالی")

            pnlFilters.Controls.Add(lblRangeMethodTitle)
            pnlFilters.Controls.Add(cmbRangeMethod)

            ' ۴. افزودن رویداد تغییر مبنا
            AddHandler cmbRangeMethod.SelectedIndexChanged,
                Sub(s, ea)
                    Dim idx = cmbRangeMethod.SelectedIndex
                    Dim isDocMode = (idx = 0)
                    Dim isDateMode = (idx = 1)
                    Dim isAllMode = (idx = 2)

                    ' فعال‌سازی فیلترها و مقادیر درونی
                    chkFilterByDoc.Checked = isDocMode
                    chkFilterByDate.Checked = isDateMode

                    ' فعال/غیرفعال کردن کنترل‌های مربوطه برای این‌که اعتبارسنجی‌ها به درستی عمل کنند
                    txtFromDoc.Enabled = isDocMode
                    txtToDoc.Enabled = isDocMode
                    txtFromDate.Enabled = isDateMode
                    btnFromDate.Enabled = isDateMode
                    txtToDate.Enabled = isDateMode
                    btnToDate.Enabled = isDateMode

                    ' نمایش/عدم نمایش فیلدهای سند
                    lblFromDoc.Visible = isDocMode
                    txtFromDoc.Visible = isDocMode
                    lblToDoc.Visible = isDocMode
                    txtToDoc.Visible = isDocMode

                    ' نمایش/عدم نمایش فیلدهای تاریخ
                    lblFromDate.Visible = isDateMode
                    txtFromDate.Visible = isDateMode
                    btnFromDate.Visible = isDateMode
                    lblToDate.Visible = isDateMode
                    txtToDate.Visible = isDateMode
                    btnToDate.Visible = isDateMode

                    ' مقداردهی پیش‌فرض تاریخ‌ها در صورت لزوم
                    If isDateMode Then
                        If String.IsNullOrWhiteSpace(txtFromDate.Text.Replace("/", "").Replace(" ", "")) Then
                            txtFromDate.Text = PersianDateHelper.ToPersian(DateTime.Today)
                        End If
                        If String.IsNullOrWhiteSpace(txtToDate.Text.Replace("/", "").Replace(" ", "")) Then
                            txtToDate.Text = PersianDateHelper.ToPersian(DateTime.Today)
                        End If
                    End If

                    ' جابجایی کنترل‌ها برای تراز بودن در پنل
                    If isDateMode Then
                        lblFromDate.Location = New Point(660, 12)
                        txtFromDate.Location = New Point(550, 9)
                        btnFromDate.Location = New Point(518, 9)
                        lblToDate.Location = New Point(470, 12)
                        txtToDate.Location = New Point(360, 9)
                        btnToDate.Location = New Point(328, 9)
                    ElseIf isDocMode Then
                        lblFromDoc.Location = New Point(660, 12)
                        txtFromDoc.Location = New Point(550, 9)
                        lblToDoc.Location = New Point(470, 12)
                        txtToDoc.Location = New Point(360, 9)
                    End If
                End Sub

            ' ۵. مقدار پیش‌فرض: بر اساس تاریخ (۱)
            cmbRangeMethod.SelectedIndex = 1
        End Sub

        Public Sub LoadAccount(accountId As Integer, accountCode As String, accountName As String,
                               hasChildren As Boolean, allIds As List(Of Integer))
            _selectedRangeAccounts.Clear()
            RemoveHandler cmbSelectedAccounts.SelectedIndexChanged, AddressOf CmbSelectedAccounts_SelectedIndexChanged
            cmbSelectedAccounts.Items.Clear()
            cmbSelectedAccounts.Items.Add("چاپ تمام دفاتر")
            cmbSelectedAccounts.SelectedIndex = 0
            AddHandler cmbSelectedAccounts.SelectedIndexChanged, AddressOf CmbSelectedAccounts_SelectedIndexChanged

            _currentAccountId = accountId
            _currentAccountCode = accountCode
            _currentAccountName = accountName
            _currentHasChildren = hasChildren
            _currentAllIds = allIds

            RemoveHandler chkAggregate.CheckedChanged, AddressOf ChkAggregate_CheckedChanged
            chkAggregate.Checked = hasChildren
            AddHandler chkAggregate.CheckedChanged, AddressOf ChkAggregate_CheckedChanged

            RefreshLedger()
        End Sub

        Public Sub RefreshLedger()
            If _currentAccountId = 0 AndAlso _selectedRangeAccounts.Count = 0 Then Return

            Dim blocks As New List(Of LedgerBlock)()

            If _selectedRangeAccounts.Count > 0 AndAlso cmbSelectedAccounts.SelectedIndex = 0 Then
                ' Mode B: Load all accounts in the range
                For Each acc In _selectedRangeAccounts
                    Dim block As New LedgerBlock() With {
                        .AccountID = acc.Item1,
                        .AccountCode = acc.Item2,
                        .AccountName = acc.Item3
                    }
                    blocks.Add(block)
                Next
            Else
                ' Mode A: Load a single account
                Dim activeId = _currentAccountId
                Dim activeCode = _currentAccountCode
                Dim activeName = _currentAccountName
                
                If _selectedRangeAccounts.Count > 0 AndAlso cmbSelectedAccounts.SelectedIndex > 0 Then
                    Dim item = DirectCast(cmbSelectedAccounts.SelectedItem, SelectedComboItem)
                    activeId = item.ID
                    activeCode = item.Code
                    activeName = item.Name
                End If

                If activeId > 0 Then
                    Dim block As New LedgerBlock() With {
                        .AccountID = activeId,
                        .AccountCode = activeCode,
                        .AccountName = activeName
                    }
                    blocks.Add(block)
                End If
            End If

            If blocks.Count = 0 Then Return

            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                
                Dim stepPct = 100 / blocks.Count
                Dim currentPct = 0.0

                For Each block In blocks
                    currentPct += stepPct
                    progress.UpdateProgress(CInt(Math.Min(100, currentPct)), "در حال بارگذاری دفتر سرفصل: " & block.AccountCode & "...")

                    ' 1. Calculate chain
                    Dim chainStr = ""
                    Try
                        Dim chain = service.GetAccountHierarchyChain(block.AccountID)
                        Dim parts As New List(Of String)()
                        For Each item In chain
                            parts.Add(item.Item1 & " — " & item.Item2)
                        Next
                        chainStr = String.Join(" / ", parts.ToArray())
                    Catch
                        chainStr = block.AccountCode & " — " & block.AccountName
                    End Try
                    block.HierarchyChain = chainStr

                    ' Get filters
                    Dim fromDateStr As String = Nothing
                    Dim toDateStr As String = Nothing
                    Dim fromDoc As Integer? = Nothing
                    Dim toDoc As Integer? = Nothing
                    Dim docStatus As String = Nothing

                    If chkFilterByDate.Checked Then
                        fromDateStr = txtFromDate.Text
                        toDateStr = txtToDate.Text
                    End If

                    If chkFilterByDoc.Checked Then
                        Dim fDocVal As Integer
                        If Integer.TryParse(txtFromDoc.Text.Trim(), fDocVal) Then fromDoc = fDocVal
                        Dim tDocVal As Integer
                        If Integer.TryParse(txtToDoc.Text.Trim(), tDocVal) Then toDoc = tDocVal
                    End If

                    If chkFilterByStatus.Checked Then
                        If cmbStatus.SelectedItem IsNot Nothing Then docStatus = cmbStatus.SelectedItem.ToString()
                    End If

                    Dim cmbRangeMethod As ComboBox = Nothing
                    For Each ctrl As Control In pnlFilters.Controls
                        If ctrl.Name = "cmbRangeMethod" AndAlso TypeOf ctrl Is ComboBox Then
                            cmbRangeMethod = CType(ctrl, ComboBox)
                            Exit For
                        End If
                    Next
                    Dim allFiscalYears As Boolean = (cmbRangeMethod IsNot Nothing AndAlso cmbRangeMethod.SelectedIndex = 2)

                    Dim blockAllIds = GetAccountAndDescendantIds(block.AccountID)

                    ' 2. Calculate prior sums
                    Dim priorDebit = 0D
                    Dim priorCredit = 0D
                    If chkFilterByDate.Checked OrElse chkFilterByDoc.Checked Then
                        Try
                            Dim beforeSums = service.GetLedgerBeforeSums(blockAllIds, fromDateStr, fromDoc, docStatus, allFiscalYears)
                            priorDebit = beforeSums.Item1
                            priorCredit = beforeSums.Item2
                        Catch
                        End Try
                    End If
                    block.PriorSums = Tuple.Create(priorDebit, priorCredit)

                    ' 3. Get ledger data
                    Try
                        Dim dt = service.GetLedgerData(blockAllIds, chkAggregate.Checked, fromDateStr, toDateStr, fromDoc, toDoc, docStatus, allFiscalYears)
                        If dt IsNot Nothing Then
                            If Not dt.Columns.Contains("OriginalBalance") Then
                                dt.Columns.Add("OriginalBalance", GetType(Decimal))
                            End If
                            If Not dt.Columns.Contains("OriginalTash") Then
                                dt.Columns.Add("OriginalTash", GetType(String))
                            End If

                            Dim runningBalance = priorDebit - priorCredit
                            For Each row As DataRow In dt.Rows
                                Dim debit = Convert.ToDecimal(row("DebitAmount"))
                                Dim credit = Convert.ToDecimal(row("CreditAmount"))
                                runningBalance += debit - credit
                                row("OriginalBalance") = runningBalance
                                If runningBalance > 0D Then
                                    row("OriginalTash") = "بدهکار"
                                ElseIf runningBalance < 0D Then
                                    row("OriginalTash") = "بستانکار"
                                Else
                                    row("OriginalTash") = "تراز"
                                End If
                            Next
                        End If
                        block.LedgerData = dt
                    Catch ex As Exception
                        MessageBox.Show("خطا در بارگذاری دفتر حساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                Next

                If blocks.Count = 1 Then
                    lblAccountTitle.Text = "دفتر حساب :  " & blocks(0).HierarchyChain
                    _fullDataTable = blocks(0).LedgerData
                    _priorSums = blocks(0).PriorSums
                Else
                    If _selectedRangeAccounts.Count > 0 AndAlso cmbSelectedAccounts.SelectedIndex = 0 Then
                        lblAccountTitle.Text = "دفتر حساب :  از کد: " & _selectedFromChain & Environment.NewLine & "تا کد: " & _selectedToChain
                    Else
                        lblAccountTitle.Text = String.Format("چاپ تمام دفاتر (تعداد: {0} دفتر از سطح {1})", blocks.Count, cmbSelectedAccounts.Text)
                    End If
                    _fullDataTable = Nothing
                    _priorSums = Nothing
                End If

                FillGridWithBlocks(blocks)
            End Using
        End Sub

        Private Function GetAccountAndDescendantIds(accountId As Integer) As List(Of Integer)
            Dim allIds As New List(Of Integer)()
            allIds.Add(accountId)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return allIds

            Try
                Dim dt = Sql.ExecuteTable("SELECT AccountID, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ? AND IsActive = 1", SessionContext.CurrentCompanyID.Value)
                Dim childMap As New Dictionary(Of Integer, List(Of Integer))()
                For Each row As DataRow In dt.Rows
                    Dim id = Convert.ToInt32(row("AccountID"))
                    Dim pVal = row("ParentAccountID")
                    If pVal IsNot Nothing AndAlso Not Convert.IsDBNull(pVal) Then
                        Dim parentId = Convert.ToInt32(pVal)
                        If Not childMap.ContainsKey(parentId) Then
                            childMap(parentId) = New List(Of Integer)()
                        End If
                        childMap(parentId).Add(id)
                    End If
                Next

                Dim collect As Action(Of Integer) = Nothing
                collect = Sub(pid As Integer)
                              If childMap.ContainsKey(pid) Then
                                  For Each childId In childMap(pid)
                                      allIds.Add(childId)
                                      collect(childId)
                                  Next
                              End If
                          End Sub
                collect(accountId)
            Catch
            End Try

            Return allIds
        End Function

        Private Sub FillGridWithBlocks(blocks As List(Of LedgerBlock))
            dgvLedger.SuspendLayout()
            dgvLedger.Rows.Clear()

            Dim showHeaderSummary = (_selectedRangeAccounts.Count = 0)

            Dim headerText As String = "شرح ردیف"
            Select Case cmbDescType.SelectedIndex
                Case 1
                    headerText = "شرح سند"
                Case 2
                    headerText = "شرح ردیف / شرح سند"
                Case Else
                    headerText = "شرح ردیف"
            End Select
            dgvLedger.Columns("colSharh").HeaderText = headerText
            dgvLedger.Columns("colLineNo").Visible = Not chkAggregate.Checked

            Dim overallDebit = 0D
            Dim overallCredit = 0D

            For Each block In blocks
                Dim priorDebit = block.PriorSums.Item1
                Dim priorCredit = block.PriorSums.Item2
                Dim priorBalance = priorDebit - priorCredit
                Dim priorTash = "تراز"

                If priorBalance > 0D Then
                    priorTash = "بدهکار"
                ElseIf priorBalance < 0D Then
                    priorTash = "بستانکار"
                Else
                    priorTash = "تراز"
                End If

                ' ۱. اضافه کردن سطر شروع دفتر حساب
                If showHeaderSummary Then
                    Dim firstRowIdx = dgvLedger.Rows.Add()
                    Dim firstRow = dgvLedger.Rows(firstRowIdx)
                    firstRow.Tag = "Header"
                    firstRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = "—"}
                    firstRow.Cells("colRefNo").Value = "—"
                    If dgvLedger.Columns("colLineNo").Visible Then
                        firstRow.Cells("colLineNo").Value = "—"
                    End If
                    firstRow.Cells("colDate").Value = "—"
                    firstRow.Cells("colSharh").Value = "شروع دفتر حساب: " & block.AccountCode & " — " & block.AccountName
                    firstRow.Cells("colDebit").Value = If(priorDebit = 0D, "0", priorDebit.ToString("#,##0"))
                    firstRow.Cells("colCredit").Value = If(priorCredit = 0D, "0", priorCredit.ToString("#,##0"))
                    firstRow.Cells("colBalance").Value = If(priorBalance = 0D, "0", Math.Abs(priorBalance).ToString("#,##0"))
                    firstRow.Cells("colTash").Value = priorTash
                    
                    firstRow.DefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255)
                    firstRow.DefaultCellStyle.ForeColor = Color.FromArgb(20, 60, 120)
                    firstRow.DefaultCellStyle.Font = New Font(dgvLedger.Font, FontStyle.Bold)
                End If

                Dim balance = priorBalance
                Dim totalDebit = priorDebit
                Dim totalCredit = priorCredit
                Dim useOriginalBalance As Boolean = Not chkRecalculateBalance.Checked

                If block.LedgerData IsNot Nothing Then
                    For Each row As DataRow In block.LedgerData.Rows
                        Dim debit = Convert.ToDecimal(row("DebitAmount"))
                        Dim credit = Convert.ToDecimal(row("CreditAmount"))
                        balance += debit - credit
                        totalDebit += debit
                        totalCredit += credit

                        Dim rowIdx = dgvLedger.Rows.Add()
                        Dim gr = dgvLedger.Rows(rowIdx)

                        Dim entryId = Convert.ToInt32(row("EntryID"))
                        Dim lineNoObj = If(block.LedgerData.Columns.Contains("LineNumber"), row("LineNumber"), DBNull.Value)
                        Dim lineNumber As Integer? = If(lineNoObj Is DBNull.Value OrElse lineNoObj Is Nothing, Nothing, CType(Convert.ToInt32(lineNoObj), Integer?))
                        gr.Tag = New RowTagInfo(entryId, lineNumber, block.AccountID, block.AccountCode, block.AccountName, block.PriorSums)

                        Dim bCell As New DataGridViewButtonCell()
                        bCell.Value = "رفتن به سند"
                        gr.Cells("colGoToDoc") = bCell

                        gr.Cells("colRefNo").Value = Convert.ToString(row("ReferenceNumber"))

                        If dgvLedger.Columns("colLineNo").Visible Then
                            gr.Cells("colLineNo").Value = Convert.ToString(row("LineNumber"))
                        End If

                        Dim dateStr = ""
                        If Not row.IsNull("EntryDate") Then
                            Try
                                dateStr = PersianDateHelper.ToPersian(Convert.ToDateTime(row("EntryDate")))
                            Catch
                                dateStr = Convert.ToString(row("EntryDate"))
                            End Try
                        End If
                        gr.Cells("colDate").Value = dateStr

                        Dim sharhRadifVal = Convert.ToString(row("SharhRadif"))
                        Dim descriptionVal = Convert.ToString(row("Description"))
                        Dim cellValue As String = ""
                        Select Case cmbDescType.SelectedIndex
                            Case 1
                                cellValue = descriptionVal
                            Case 2
                                cellValue = "شرح ردیف : " & sharhRadifVal & " / شرح سند : " & descriptionVal
                            Case Else
                                cellValue = sharhRadifVal
                        End Select
                        gr.Cells("colSharh").Value = cellValue

                        gr.Cells("colDebit").Value = If(debit = 0D, "", debit.ToString("#,##0"))
                        gr.Cells("colCredit").Value = If(credit = 0D, "", credit.ToString("#,##0"))

                        Dim displayBalance As Decimal
                        Dim displayTash As String = ""

                        If useOriginalBalance Then
                            Dim origBal = If(block.LedgerData.Columns.Contains("OriginalBalance") AndAlso Not row.IsNull("OriginalBalance"), Convert.ToDecimal(row("OriginalBalance")), balance)
                            displayBalance = Math.Abs(origBal)
                            displayTash = If(block.LedgerData.Columns.Contains("OriginalTash") AndAlso Not row.IsNull("OriginalTash"), Convert.ToString(row("OriginalTash")), "")
                            If String.IsNullOrEmpty(displayTash) Then
                                If origBal > 0D Then
                                    displayTash = "بدهکار"
                                ElseIf origBal < 0D Then
                                    displayTash = "بستانکار"
                                Else
                                    displayTash = "تراز"
                                End If
                            End If
                        Else
                            displayBalance = Math.Abs(balance)
                            If balance > 0D Then
                                displayTash = "بدهکار"
                            ElseIf balance < 0D Then
                                displayTash = "بستانکار"
                            Else
                                displayTash = "تراز"
                            End If
                        End If

                        gr.Cells("colBalance").Value = displayBalance.ToString("#,##0")
                        gr.Cells("colTash").Value = displayTash

                        If displayTash = "بدهکار" Then
                            gr.Cells("colTash").Style.ForeColor = Color.DarkRed
                        ElseIf displayTash = "بستانکار" Then
                            gr.Cells("colTash").Style.ForeColor = Color.DarkBlue
                        ElseIf displayTash = "تراز" Then
                            gr.Cells("colTash").Style.ForeColor = Color.DarkGreen
                        End If
                    Next
                End If

                ' ۲. اضافه کردن سطر جمع دفتر حساب
                If showHeaderSummary Then
                    Dim sumRowIdx = dgvLedger.Rows.Add()
                    Dim sumRow = dgvLedger.Rows(sumRowIdx)
                    sumRow.Tag = "Summary"
                    sumRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = "—"}
                    sumRow.Cells("colRefNo").Value = "—"
                    If dgvLedger.Columns("colLineNo").Visible Then
                        sumRow.Cells("colLineNo").Value = "—"
                    End If
                    sumRow.Cells("colDate").Value = "—"
                    sumRow.Cells("colSharh").Value = "جمع دفتر حساب: " & block.AccountCode & " — " & block.AccountName
                    sumRow.Cells("colDebit").Value = If(totalDebit = 0D, "0", totalDebit.ToString("#,##0"))
                    sumRow.Cells("colCredit").Value = If(totalCredit = 0D, "0", totalCredit.ToString("#,##0"))
                    sumRow.Cells("colBalance").Value = If(balance = 0D, "0", Math.Abs(balance).ToString("#,##0"))
                    
                    Dim sumTash = "تراز"
                    If balance > 0D Then
                        sumTash = "بدهکار"
                    ElseIf balance < 0D Then
                        sumTash = "بستانکار"
                    End If
                    sumRow.Cells("colTash").Value = sumTash
                    
                    sumRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200)
                    sumRow.DefaultCellStyle.ForeColor = Color.Black
                    sumRow.DefaultCellStyle.Font = New Font(dgvLedger.Font, FontStyle.Bold)
                End If

                overallDebit += totalDebit
                overallCredit += totalCredit
            Next

            lblSumDebit.Text = If(overallDebit = 0D, "0", overallDebit.ToString("#,##0"))
            lblSumCredit.Text = If(overallCredit = 0D, "0", overallCredit.ToString("#,##0"))

            Dim finalDiff = overallDebit - overallCredit
            lblSumBalance.Text = Math.Abs(finalDiff).ToString("#,##0")
            If finalDiff > 0D Then
                lblTash.Text = "بدهکار"
                lblTash.ForeColor = Color.DarkRed
            ElseIf finalDiff < 0D Then
                lblTash.Text = "بستانکار"
                lblTash.ForeColor = Color.DarkBlue
            Else
                lblTash.Text = "تراز"
                lblTash.ForeColor = Color.DarkGreen
            End If

            dgvLedger.ResumeLayout()
            AlignJamLabels()

            If _returnTargetEntryID.HasValue Then
                Dim targetEntryId = _returnTargetEntryID.Value
                Dim targetLineNo = _returnTargetLineNumber
                Dim matchedRowIndex As Integer = -1

                For rowIndex As Integer = 0 To dgvLedger.Rows.Count - 1
                    Dim gr = dgvLedger.Rows(rowIndex)
                    Dim tag = TryCast(gr.Tag, RowTagInfo)
                    If tag IsNot Nothing Then
                        If tag.EntryID = targetEntryId Then
                            matchedRowIndex = rowIndex
                            If targetLineNo.HasValue AndAlso tag.LineNumber.HasValue AndAlso tag.LineNumber.Value = targetLineNo.Value Then
                                ' Exact match found
                                Exit For
                            End If
                        End If
                    End If
                Next

                If matchedRowIndex >= 0 Then
                    Try
                        dgvLedger.CurrentCell = dgvLedger.Rows(matchedRowIndex).Cells("colGoToDoc")
                        dgvLedger.FirstDisplayedScrollingRowIndex = Math.Max(0, matchedRowIndex - 2)
                        dgvLedger.Focus()
                    Catch
                    End Try
                End If

                _returnTargetEntryID = Nothing
                _returnTargetLineNumber = Nothing
            End If
        End Sub

        Private Sub BtnSelectAccountsPopup_Click(sender As Object, e As EventArgs) Handles btnSelectAccountsPopup.Click
            Using frm As New SelectAccountsRangeForm()
                If frm.ShowDialog(Me) = DialogResult.OK Then
                    _selectedRangeAccounts.Clear()
                    _selectedRangeAccounts.AddRange(frm.SelectedAccounts)
                    _selectedFromChain = frm.SelectedFromChain
                    _selectedToChain = frm.SelectedToChain

                    RemoveHandler cmbSelectedAccounts.SelectedIndexChanged, AddressOf CmbSelectedAccounts_SelectedIndexChanged
                    cmbSelectedAccounts.Items.Clear()
                    cmbSelectedAccounts.Items.Add("چاپ تمام دفاتر")
                    For Each acc In _selectedRangeAccounts
                        cmbSelectedAccounts.Items.Add(New SelectedComboItem(acc.Item1, acc.Item2, acc.Item3))
                    Next
                    cmbSelectedAccounts.SelectedIndex = 0
                    AddHandler cmbSelectedAccounts.SelectedIndexChanged, AddressOf CmbSelectedAccounts_SelectedIndexChanged

                    RefreshLedger()
                End If
            End Using
        End Sub

        Private Sub CmbSelectedAccounts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSelectedAccounts.SelectedIndexChanged
            RefreshLedger()
        End Sub

        Private Class LedgerBlock
            Public Property AccountID As Integer
            Public Property AccountCode As String
            Public Property AccountName As String
            Public Property HierarchyChain As String
            Public Property PriorSums As Tuple(Of Decimal, Decimal)
            Public Property LedgerData As DataTable
        End Class

        Private Class SelectedComboItem
            Public Property ID As Integer
            Public Property Code As String
            Public Property Name As String

            Public Sub New(id As Integer, code As String, name As String)
                Me.ID = id
                Me.Code = code
                Me.Name = name
            End Sub

            Public Overrides Function ToString() As String
                Return Me.Code & " - " & Me.Name
            End Function
        End Class

        Private Sub ChkAggregate_CheckedChanged(sender As Object, e As EventArgs) Handles chkAggregate.CheckedChanged
            RefreshLedger()
        End Sub

        Private Sub ChkFilterByDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterByDate.CheckedChanged
            Dim isChecked = chkFilterByDate.Checked
            txtFromDate.Enabled = isChecked
            btnFromDate.Enabled = isChecked
            txtToDate.Enabled = isChecked
            btnToDate.Enabled = isChecked

            If isChecked Then
                If String.IsNullOrWhiteSpace(txtFromDate.Text.Replace("/", "").Replace(" ", "")) Then
                    txtFromDate.Text = PersianDateHelper.ToPersian(DateTime.Today)
                End If
                If String.IsNullOrWhiteSpace(txtToDate.Text.Replace("/", "").Replace(" ", "")) Then
                    txtToDate.Text = PersianDateHelper.ToPersian(DateTime.Today)
                End If
            End If
            RefreshLedger()
        End Sub

        Private Sub ChkFilterByDoc_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterByDoc.CheckedChanged
            Dim isChecked = chkFilterByDoc.Checked
            txtFromDoc.Enabled = isChecked
            txtToDoc.Enabled = isChecked
            RefreshLedger()
        End Sub

        Private Sub ChkFilterByStatus_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterByStatus.CheckedChanged
            cmbStatus.Enabled = chkFilterByStatus.Checked
            RefreshLedger()
        End Sub

        Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged
            If chkFilterByStatus.Checked Then
                RefreshLedger()
            End If
        End Sub

        Private Sub CmbDescType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDescType.SelectedIndexChanged
            RefreshLedger()
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            RefreshLedger()
        End Sub

        Private Sub BtnFromDate_Click(sender As Object, e As EventArgs) Handles btnFromDate.Click
            Dim anchor = EnsureOnScreen(
                txtFromDate.PointToScreen(New Point(0, txtFromDate.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtFromDate.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtFromDate.Text = cal.SelectedDate
                    RefreshLedger()
                End If
            End Using
        End Sub

        Private Sub BtnToDate_Click(sender As Object, e As EventArgs) Handles btnToDate.Click
            Dim anchor = EnsureOnScreen(
                txtToDate.PointToScreen(New Point(0, txtToDate.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtToDate.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtToDate.Text = cal.SelectedDate
                    RefreshLedger()
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, formSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - formSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - formSize.Height)))
        End Function

        Private Sub FillGrid(dt As DataTable, isGrouped As Boolean)
            dgvLedger.SuspendLayout()
            dgvLedger.Rows.Clear()

            ' تنظیم هدر ستون شرح بر اساس نوع نمایش
            Dim headerText As String = "شرح ردیف"
            Select Case cmbDescType.SelectedIndex
                Case 1
                    headerText = "شرح سند"
                Case 2
                    headerText = "شرح ردیف / شرح سند"
                Case Else
                    headerText = "شرح ردیف"
            End Select
            dgvLedger.Columns("colSharh").HeaderText = headerText
            dgvLedger.Columns("colLineNo").Visible = Not isGrouped

            Dim priorDebit = 0D
            Dim priorCredit = 0D
            Dim priorBalance = 0D
            Dim priorTash = "تراز"

            If _priorSums IsNot Nothing Then
                priorDebit = _priorSums.Item1
                priorCredit = _priorSums.Item2
                priorBalance = priorDebit - priorCredit
                If priorBalance > 0D Then
                    priorTash = "بدهکار"
                ElseIf priorBalance < 0D Then
                    priorTash = "بستانکار"
                Else
                    priorTash = "تراز"
                End If
            End If

            ' همیشه سطر اول را برای مقادیر قبلی اضافه کن
            Dim firstRowIdx = dgvLedger.Rows.Add()
            Dim firstRow = dgvLedger.Rows(firstRowIdx)
            firstRow.Tag = Nothing
            firstRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = "—"}
            firstRow.Cells("colRefNo").Value = "—"
            If dgvLedger.Columns("colLineNo").Visible Then
                firstRow.Cells("colLineNo").Value = "—"
            End If
            firstRow.Cells("colDate").Value = "—"
            firstRow.Cells("colSharh").Value = "گردش و مانده حساب قبلی"
            firstRow.Cells("colDebit").Value = If(priorDebit = 0D, "0", priorDebit.ToString("#,##0"))
            firstRow.Cells("colCredit").Value = If(priorCredit = 0D, "0", priorCredit.ToString("#,##0"))
            firstRow.Cells("colBalance").Value = If(priorBalance = 0D, "0", Math.Abs(priorBalance).ToString("#,##0"))
            firstRow.Cells("colTash").Value = priorTash
            If priorTash = "بدهکار" Then
                firstRow.Cells("colTash").Style.ForeColor = Color.DarkRed
            ElseIf priorTash = "بستانکار" Then
                firstRow.Cells("colTash").Style.ForeColor = Color.DarkBlue
            ElseIf priorTash = "تراز" Then
                firstRow.Cells("colTash").Style.ForeColor = Color.DarkGreen
            End If

            Dim balance = priorBalance
            Dim totalDebit = priorDebit
            Dim totalCredit = priorCredit
            Dim useOriginalBalance As Boolean = Not chkRecalculateBalance.Checked

            For Each row As DataRow In dt.Rows
                Dim debit = Convert.ToDecimal(row("DebitAmount"))
                Dim credit = Convert.ToDecimal(row("CreditAmount"))
                balance += debit - credit
                totalDebit += debit
                totalCredit += credit

                Dim rowIdx = dgvLedger.Rows.Add()
                Dim gr = dgvLedger.Rows(rowIdx)

                ' ذخیره شناسه سند و خط در تگ ردیف برای ناوبری
                Dim entryId = Convert.ToInt32(row("EntryID"))
                Dim lineNoObj = If(dt.Columns.Contains("LineNumber"), row("LineNumber"), DBNull.Value)
                Dim lineNumber As Integer? = If(lineNoObj Is DBNull.Value OrElse lineNoObj Is Nothing, Nothing, CType(Convert.ToInt32(lineNoObj), Integer?))
                gr.Tag = Tuple.Create(entryId, lineNumber)

                ' دکمه رفتن به سند همیشه نمایش داده می‌شود
                Dim bCell As New DataGridViewButtonCell()
                bCell.Value = "رفتن به سند"
                gr.Cells("colGoToDoc") = bCell

                gr.Cells("colRefNo").Value = Convert.ToString(row("ReferenceNumber"))

                If Not isGrouped Then
                    gr.Cells("colLineNo").Value = Convert.ToString(row("LineNumber"))
                End If

                Dim dateStr = ""
                If Not row.IsNull("EntryDate") Then
                    Try
                        dateStr = PersianDateHelper.ToPersian(Convert.ToDateTime(row("EntryDate")))
                    Catch
                        dateStr = Convert.ToString(row("EntryDate"))
                    End Try
                End If
                gr.Cells("colDate").Value = dateStr

                ' قالب‌بندی ستون شرح به صورت پویا بر اساس نوع شرح دفتر انتخابی
                Dim sharhRadifVal = Convert.ToString(row("SharhRadif"))
                Dim descriptionVal = Convert.ToString(row("Description"))
                Dim cellValue As String = ""
                Select Case cmbDescType.SelectedIndex
                    Case 1
                        cellValue = descriptionVal
                    Case 2
                        cellValue = "شرح ردیف : " & sharhRadifVal & " / شرح سند : " & descriptionVal
                    Case Else
                        cellValue = sharhRadifVal
                End Select
                gr.Cells("colSharh").Value = cellValue

                gr.Cells("colDebit").Value = If(debit = 0D, "", debit.ToString("#,##0"))
                gr.Cells("colCredit").Value = If(credit = 0D, "", credit.ToString("#,##0"))

                Dim displayBalance As Decimal
                Dim displayTash As String = ""

                If useOriginalBalance Then
                    Dim origBal = If(dt.Columns.Contains("OriginalBalance") AndAlso Not row.IsNull("OriginalBalance"), Convert.ToDecimal(row("OriginalBalance")), balance)
                    displayBalance = Math.Abs(origBal)
                    displayTash = If(dt.Columns.Contains("OriginalTash") AndAlso Not row.IsNull("OriginalTash"), Convert.ToString(row("OriginalTash")), "")
                    If String.IsNullOrEmpty(displayTash) Then
                        If origBal > 0D Then
                            displayTash = "بدهکار"
                        ElseIf origBal < 0D Then
                            displayTash = "بستانکار"
                        Else
                            displayTash = "تراز"
                        End If
                    End If
                Else
                    displayBalance = Math.Abs(balance)
                    If balance > 0D Then
                        displayTash = "بدهکار"
                    ElseIf balance < 0D Then
                        displayTash = "بستانکار"
                    Else
                        displayTash = "تراز"
                    End If
                End If

                gr.Cells("colBalance").Value = displayBalance.ToString("#,##0")
                gr.Cells("colTash").Value = displayTash

                If displayTash = "بدهکار" Then
                    gr.Cells("colTash").Style.ForeColor = Color.DarkRed
                ElseIf displayTash = "بستانکار" Then
                    gr.Cells("colTash").Style.ForeColor = Color.DarkBlue
                ElseIf displayTash = "تراز" Then
                    gr.Cells("colTash").Style.ForeColor = Color.DarkGreen
                End If
            Next

            ' تنظیم مقادیر جمع کل در پنل پایین
            lblSumDebit.Text = If(totalDebit = 0D, "0", totalDebit.ToString("#,##0"))
            lblSumCredit.Text = If(totalCredit = 0D, "0", totalCredit.ToString("#,##0"))

            Dim finalDiff = totalDebit - totalCredit
            lblSumBalance.Text = Math.Abs(finalDiff).ToString("#,##0")
            If finalDiff > 0D Then
                lblTash.Text = "بدهکار"
                lblTash.ForeColor = Color.DarkRed
            ElseIf finalDiff < 0D Then
                lblTash.Text = "بستانکار"
                lblTash.ForeColor = Color.DarkBlue
            Else
                lblTash.Text = "تراز"
                lblTash.ForeColor = Color.DarkGreen
            End If

            dgvLedger.ResumeLayout()
            AlignJamLabels()
        End Sub

        Private Sub AlignJamLabels()
            If dgvLedger Is Nothing OrElse dgvLedger.Columns.Count = 0 OrElse pnlJamDaftar Is Nothing Then Return

            Dim sharhCol = dgvLedger.Columns("colSharh")
            If sharhCol IsNot Nothing AndAlso sharhCol.Visible Then
                Dim rect = dgvLedger.GetColumnDisplayRectangle(sharhCol.Index, True)
                lblJamTitle.Left = rect.Left
                lblJamTitle.Width = rect.Width
                lblJamTitle.Visible = rect.Width > 0
            Else
                lblJamTitle.Visible = False
            End If

            AlignLabel("colDebit", lblSumDebit)
            AlignLabel("colCredit", lblSumCredit)
            AlignLabel("colTash", lblTash)
            AlignLabel("colBalance", lblSumBalance)
        End Sub

        Private Sub AlignLabel(columnName As String, label As Label)
            Dim col = dgvLedger.Columns(columnName)
            If col IsNot Nothing AndAlso col.Visible Then
                Dim rect = dgvLedger.GetColumnDisplayRectangle(col.Index, True)
                label.Left = rect.Left
                label.Width = rect.Width
                label.Visible = rect.Width > 0
            Else
                label.Visible = False
            End If
        End Sub

        Private Sub DgvLedger_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvLedger.ColumnWidthChanged
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub DgvLedger_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvLedger.Scroll
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub HesabdaryDaftarForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub DgvLedger_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLedger.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim col = dgvLedger.Columns(e.ColumnIndex)
            If col.Name = "colGoToDoc" Then
                OpenVoucherFromRow(e.RowIndex)
            End If
        End Sub

        Private Sub DgvLedger_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLedger.CellDoubleClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            OpenVoucherFromRow(e.RowIndex)
        End Sub

        Private Sub OpenVoucherFromRow(rowIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= dgvLedger.Rows.Count Then Return
            Dim tagVal = dgvLedger.Rows(rowIndex).Tag
            If tagVal IsNot Nothing AndAlso (tagVal.ToString() = "Header" OrElse tagVal.ToString() = "Summary") Then
                Return
            End If

            If chkAggregate.Checked Then
                MessageBox.Show("لطفاً تیک تجمیع سطرهای هم سطح با کد یکسان در یک سند را بردارید و مجدداً روی این ردیف کلیک کنید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim tag = TryCast(dgvLedger.Rows(rowIndex).Tag, RowTagInfo)
            If tag IsNot Nothing Then
                _returnTargetEntryID = tag.EntryID
                _returnTargetLineNumber = tag.LineNumber
                RaiseEvent EditDocumentRequested(tag.EntryID, tag.LineNumber)
            Else
                Dim tupleTag = TryCast(dgvLedger.Rows(rowIndex).Tag, Tuple(Of Integer, Integer?))
                If tupleTag IsNot Nothing Then
                    _returnTargetEntryID = tupleTag.Item1
                    _returnTargetLineNumber = tupleTag.Item2
                    RaiseEvent EditDocumentRequested(tupleTag.Item1, tupleTag.Item2)
                End If
            End If
        End Sub

        Private Sub BtnBackToTrial_Click(sender As Object, e As EventArgs) Handles btnBackToTrial.Click
            Dim parentPage = TryCast(Me.Parent, TabPage)
            If parentPage IsNot Nothing Then
                Dim tabCtrl = TryCast(parentPage.Parent, TabControl)
                If tabCtrl IsNot Nothing Then
                    For Each tp As TabPage In tabCtrl.TabPages
                        If tp.Text = "تراز آزمایشی" Then
                            tabCtrl.SelectedTab = tp
                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvLedger Is Nothing OrElse dgvLedger.Columns.Count = 0 OrElse pnlSerch Is Nothing Then Return

            Dim AlignTB = Sub(tb As TextBox, colName As String)
                              Dim col = dgvLedger.Columns(colName)
                              If col Is Nothing OrElse Not col.Visible Then
                                  tb.Visible = False
                                  Return
                              End If
                              Dim r = dgvLedger.GetColumnDisplayRectangle(col.Index, True)
                              If r.IsEmpty OrElse r.Width = 0 Then
                                  tb.Visible = False
                                  Return
                              End If
                              Dim screenPt = dgvLedger.PointToScreen(New Point(r.X, 0))
                              Dim panelPt = pnlSerch.PointToClient(screenPt)
                              tb.Location = New Point(panelPt.X, 4)
                              tb.Width = r.Width
                              tb.Visible = True
                          End Sub

            AlignTB(txtSrcRefNo, "colRefNo")
            AlignTB(txtSrcLineNo, "colLineNo")
            AlignTB(txtSrcDate, "colDate")
            AlignTB(txtSrcSharh, "colSharh")
            AlignTB(txtSrcDebit, "colDebit")
            AlignTB(txtSrcCredit, "colCredit")
            AlignTB(txtSrcTash, "colTash")
            AlignTB(txtSrcBalance, "colBalance")
        End Sub

        Private Sub ApplySearchFilter()
            If _fullDataTable IsNot Nothing Then
                Dim refText = txtSrcRefNo.Text.Trim().ToLower()
                Dim lineText = txtSrcLineNo.Text.Trim().ToLower()
                Dim dateText = txtSrcDate.Text.Trim().ToLower()
                Dim sharhText = txtSrcSharh.Text.Trim().ToLower()
                Dim debitText = txtSrcDebit.Text.Trim().ToLower()
                Dim creditText = txtSrcCredit.Text.Trim().ToLower()
                Dim tashText = txtSrcTash.Text.Trim().ToLower()
                Dim balanceText = txtSrcBalance.Text.Trim().ToLower()

                Dim anyFilter = refText.Length > 0 OrElse lineText.Length > 0 OrElse
                                dateText.Length > 0 OrElse sharhText.Length > 0 OrElse
                                debitText.Length > 0 OrElse creditText.Length > 0 OrElse
                                tashText.Length > 0 OrElse balanceText.Length > 0

                If Not anyFilter Then
                    FillGrid(_fullDataTable, chkAggregate.Checked)
                    AlignSearchBoxes()
                    Return
                End If

                Dim priorBalance = 0D
                If _priorSums IsNot Nothing Then
                    priorBalance = _priorSums.Item1 - _priorSums.Item2
                End If

                Dim filteredDt = _fullDataTable.Clone()
                Dim balance = priorBalance

                For Each row As DataRow In _fullDataTable.Rows
                    Dim debit = Convert.ToDecimal(row("DebitAmount"))
                    Dim credit = Convert.ToDecimal(row("CreditAmount"))
                    balance += debit - credit

                    Dim rowRef = Convert.ToString(row("ReferenceNumber")).ToLower()

                    Dim rowLine = ""
                    If _fullDataTable.Columns.Contains("LineNumber") AndAlso Not row.IsNull("LineNumber") Then
                        rowLine = Convert.ToString(row("LineNumber"))
                    End If
                    rowLine = rowLine.ToLower()

                    Dim rowDate = ""
                    If Not row.IsNull("EntryDate") Then
                        Try
                            rowDate = PersianDateHelper.ToPersian(Convert.ToDateTime(row("EntryDate"))).ToLower()
                        Catch
                            rowDate = Convert.ToString(row("EntryDate")).ToLower()
                        End Try
                    End If

                    Dim sharhRadifVal = Convert.ToString(row("SharhRadif"))
                    Dim descriptionVal = Convert.ToString(row("Description"))
                    Dim rowSharh = ""
                    Select Case cmbDescType.SelectedIndex
                        Case 1
                            rowSharh = descriptionVal
                        Case 2
                            rowSharh = "شرح ردیف : " & sharhRadifVal & " / شرح سند : " & descriptionVal
                        Case Else
                            rowSharh = sharhRadifVal
                    End Select
                    rowSharh = rowSharh.ToLower()

                    Dim rowDebit = If(debit = 0D, "", debit.ToString("#,##0")).ToLower()
                    Dim rowCredit = If(credit = 0D, "", credit.ToString("#,##0")).ToLower()

                    Dim displayBalanceVal As Decimal
                    Dim rowTash = ""

                    If Not chkRecalculateBalance.Checked Then
                        displayBalanceVal = Math.Abs(Convert.ToDecimal(row("OriginalBalance")))
                        rowTash = Convert.ToString(row("OriginalTash")).ToLower()
                    Else
                        displayBalanceVal = Math.Abs(balance)
                        If balance > 0D Then
                            rowTash = "بدهکار"
                        ElseIf balance < 0D Then
                            rowTash = "بستانکار"
                        Else
                            rowTash = "تراز"
                        End If
                        rowTash = rowTash.ToLower()
                    End If

                    Dim rowBalance = displayBalanceVal.ToString("#,##0").ToLower()

                    Dim match = True
                    If refText.Length > 0 AndAlso Not rowRef.Contains(refText) Then match = False
                    If lineText.Length > 0 AndAlso Not rowLine.Contains(lineText) Then match = False
                    If dateText.Length > 0 AndAlso Not rowDate.Contains(dateText) Then match = False
                    If sharhText.Length > 0 AndAlso Not rowSharh.Contains(sharhText) Then match = False
                    If debitText.Length > 0 AndAlso (debit = 0D OrElse Not MatchAmount(debit, debitText)) Then match = False
                    If creditText.Length > 0 AndAlso (credit = 0D OrElse Not MatchAmount(credit, creditText)) Then match = False
                    If tashText.Length > 0 AndAlso Not rowTash.Contains(tashText) Then match = False
                    If balanceText.Length > 0 AndAlso Not MatchAmount(displayBalanceVal, balanceText) Then match = False

                    If match Then
                        filteredDt.ImportRow(row)
                    End If
                Next

                FillGrid(filteredDt, chkAggregate.Checked)
                AlignSearchBoxes()
            Else
                ' Range selection mode or multi-ledger view: Filter GridView rows directly
                Dim refText = txtSrcRefNo.Text.Trim().ToLower()
                Dim lineText = txtSrcLineNo.Text.Trim().ToLower()
                Dim dateText = txtSrcDate.Text.Trim().ToLower()
                Dim sharhText = txtSrcSharh.Text.Trim().ToLower()
                Dim debitText = txtSrcDebit.Text.Trim().ToLower()
                Dim creditText = txtSrcCredit.Text.Trim().ToLower()
                Dim tashText = txtSrcTash.Text.Trim().ToLower()
                Dim balanceText = txtSrcBalance.Text.Trim().ToLower()

                dgvLedger.SuspendLayout()
                Dim prevCurrentCell = dgvLedger.CurrentCell
                dgvLedger.CurrentCell = Nothing

                Dim overallDebit = 0D
                Dim overallCredit = 0D

                For Each row As DataGridViewRow In dgvLedger.Rows
                    If row.IsNewRow Then Continue For
                    Dim tagStr = Convert.ToString(row.Tag)
                    If tagStr = "Header" OrElse tagStr = "Summary" Then Continue For

                    Dim tagInfo = TryCast(row.Tag, RowTagInfo)
                    If tagInfo Is Nothing Then Continue For

                    Dim rowRef = Convert.ToString(row.Cells("colRefNo").Value).ToLower()
                    Dim rowLine = Convert.ToString(row.Cells("colLineNo").Value).ToLower()
                    Dim rowDate = Convert.ToString(row.Cells("colDate").Value).ToLower()
                    Dim rowSharh = Convert.ToString(row.Cells("colSharh").Value).ToLower()
                    Dim rowDebitStr = Convert.ToString(row.Cells("colDebit").Value).Replace(",", "").Trim()
                    Dim rowCreditStr = Convert.ToString(row.Cells("colCredit").Value).Replace(",", "").Trim()
                    Dim rowTash = Convert.ToString(row.Cells("colTash").Value).ToLower()
                    Dim rowBalanceStr = Convert.ToString(row.Cells("colBalance").Value).Replace(",", "").Trim()

                    Dim debit As Decimal = 0D
                    Decimal.TryParse(rowDebitStr, debit)

                    Dim credit As Decimal = 0D
                    Decimal.TryParse(rowCreditStr, credit)

                    Dim balVal As Decimal = 0D
                    Decimal.TryParse(rowBalanceStr, balVal)

                    Dim match = True
                    If refText.Length > 0 AndAlso Not rowRef.Contains(refText) Then match = False
                    If lineText.Length > 0 AndAlso Not rowLine.Contains(lineText) Then match = False
                    If dateText.Length > 0 AndAlso Not rowDate.Contains(dateText) Then match = False
                    If sharhText.Length > 0 AndAlso Not rowSharh.Contains(sharhText) Then match = False
                    If debitText.Length > 0 AndAlso (debit = 0D OrElse Not MatchAmount(debit, debitText)) Then match = False
                    If creditText.Length > 0 AndAlso (credit = 0D OrElse Not MatchAmount(credit, creditText)) Then match = False
                    If tashText.Length > 0 AndAlso Not rowTash.Contains(tashText) Then match = False
                    If balanceText.Length > 0 AndAlso Not MatchAmount(balVal, balanceText) Then match = False

                    row.Visible = match

                    If match Then
                        overallDebit += debit
                        overallCredit += credit
                    End If
                Next

                Try
                    If prevCurrentCell IsNot Nothing AndAlso prevCurrentCell.OwningRow.Visible Then
                        dgvLedger.CurrentCell = prevCurrentCell
                    End If
                Catch
                End Try

                lblSumDebit.Text = If(overallDebit = 0D, "0", overallDebit.ToString("#,##0"))
                lblSumCredit.Text = If(overallCredit = 0D, "0", overallCredit.ToString("#,##0"))

                Dim finalDiff = overallDebit - overallCredit
                lblSumBalance.Text = Math.Abs(finalDiff).ToString("#,##0")
                If finalDiff > 0D Then
                    lblTash.Text = "بدهکار"
                    lblTash.ForeColor = Color.DarkRed
                ElseIf finalDiff < 0D Then
                    lblTash.Text = "بستانکار"
                    lblTash.ForeColor = Color.DarkBlue
                Else
                    lblTash.Text = "تراز"
                    lblTash.ForeColor = Color.DarkGreen
                End If

                dgvLedger.ResumeLayout()
                AlignJamLabels()
            End If
        End Sub

        Private Sub TxtSrcAny_TextChanged(sender As Object, e As EventArgs) _
            Handles txtSrcRefNo.TextChanged, txtSrcLineNo.TextChanged, txtSrcDate.TextChanged,
                    txtSrcSharh.TextChanged, txtSrcDebit.TextChanged, txtSrcCredit.TextChanged,
                    txtSrcTash.TextChanged, txtSrcBalance.TextChanged
            ApplySearchFilter()
        End Sub

        Private Sub BtnClearSearch_Click(sender As Object, e As EventArgs) Handles btnClearSearch.Click
            RemoveHandler txtSrcRefNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcLineNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcDate.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcSharh.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcDebit.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcCredit.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcTash.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcBalance.TextChanged, AddressOf TxtSrcAny_TextChanged

            txtSrcRefNo.Clear()
            txtSrcLineNo.Clear()
            txtSrcDate.Clear()
            txtSrcSharh.Clear()
            txtSrcDebit.Clear()
            txtSrcCredit.Clear()
            txtSrcTash.Clear()
            txtSrcBalance.Clear()

            AddHandler txtSrcRefNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcLineNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDate.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcSharh.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDebit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcCredit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcTash.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcBalance.TextChanged, AddressOf TxtSrcAny_TextChanged

            ApplySearchFilter()
        End Sub

        Private Function MatchAmount(val As Decimal, filterText As String) As Boolean
            If filterText.Length = 0 Then Return True
            Dim ic = Globalization.CultureInfo.InvariantCulture
            Dim ch = filterText(0)
            If ch = "*"c Then
                Dim searchStr = filterText.Substring(1)
                If searchStr.Length = 0 Then Return True
                Dim valStr = Math.Truncate(val).ToString(ic)
                Return valStr.Contains(searchStr)
            ElseIf ch = "<"c Then
                Dim numStr = filterText.Substring(1).Trim()
                Dim threshold As Decimal
                If numStr.Length = 0 OrElse Not Decimal.TryParse(numStr, Globalization.NumberStyles.Integer, ic, threshold) Then Return True
                Return val < threshold
            ElseIf ch = ">"c Then
                Dim numStr = filterText.Substring(1).Trim()
                Dim threshold As Decimal
                If numStr.Length = 0 OrElse Not Decimal.TryParse(numStr, Globalization.NumberStyles.Integer, ic, threshold) Then Return True
                Return val > threshold
            Else
                Dim target As Decimal
                If Decimal.TryParse(filterText, Globalization.NumberStyles.Integer, ic, target) Then
                    Return val = target
                End If
                Return True
            End If
        End Function

        Private Sub TxtSrcAmount_KeyPress(sender As Object, e As KeyPressEventArgs) _
            Handles txtSrcDebit.KeyPress, txtSrcCredit.KeyPress, txtSrcBalance.KeyPress
            If Char.IsControl(e.KeyChar) Then Return
            If Char.IsDigit(e.KeyChar) Then Return
            If e.KeyChar = "<"c OrElse e.KeyChar = ">"c OrElse e.KeyChar = "*"c Then
                Dim tb = TryCast(sender, TextBox)
                If tb IsNot Nothing AndAlso tb.SelectionStart = 0 Then
                    Dim firstIsSpecial = tb.Text.Length > 0 AndAlso
                                        (tb.Text(0) = "<"c OrElse tb.Text(0) = ">"c OrElse tb.Text(0) = "*"c)
                    If Not firstIsSpecial OrElse tb.SelectionLength > 0 Then Return
                End If
            End If
            e.Handled = True
        End Sub

        Private Sub ChkRecalculateBalance_CheckedChanged(sender As Object, e As EventArgs) Handles chkRecalculateBalance.CheckedChanged
            ApplySearchFilter()
        End Sub

        Private Sub btnPrintDaftar_Click(sender As Object, e As EventArgs) Handles btnPrintDaftar.Click
            Try
                If dgvLedger Is Nothing OrElse dgvLedger.Rows.Count = 0 Then
                    MessageBox.Show("هیچ حسابی برای چاپ انتخاب نشده است.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim blocks As New List(Of LedgerPrintBlock)()

                Dim parseDec = Function(valObj As Object) As Decimal?
                                   If valObj Is Nothing OrElse valObj Is DBNull.Value Then Return Nothing
                                   Dim str = valObj.ToString().Replace(",", "").Trim()
                                   Dim decVal As Decimal
                                   If Decimal.TryParse(str, decVal) Then Return decVal
                                   Return Nothing
                               End Function

                If _selectedRangeAccounts.Count > 0 Then
                    Dim currentBlock As LedgerPrintBlock = Nothing
                    Dim currentBlockId As Integer = 0

                    For Each row As DataGridViewRow In dgvLedger.Rows
                        If row.IsNewRow Then Continue For
                        Dim tagInfo = TryCast(row.Tag, RowTagInfo)
                        If tagInfo Is Nothing Then Continue For

                        If currentBlock Is Nothing OrElse tagInfo.AccountID <> currentBlockId Then
                            currentBlockId = tagInfo.AccountID
                            
                            Dim blockChain As List(Of Tuple(Of String, String)) = Nothing
                            Dim blockLedgerTitle = "دفتر حساب"
                            Try
                                Dim chain = service.GetAccountHierarchyChain(tagInfo.AccountID)
                                blockChain = chain
                                If chain IsNot Nothing Then
                                    Select Case chain.Count
                                        Case 1: blockLedgerTitle = "دفتر گروه"
                                        Case 2: blockLedgerTitle = "دفتر کل"
                                        Case 3: blockLedgerTitle = "دفتر معین"
                                        Case Else: blockLedgerTitle = "دفتر تفصیلی"
                                    End Select
                                End If
                            Catch
                            End Try

                            currentBlock = New LedgerPrintBlock() With {
                                .LedgerTitle = blockLedgerTitle,
                                .AccountNameTitle = tagInfo.AccountCode & " — " & tagInfo.AccountName,
                                .AccountHierarchyChain = blockChain,
                                .TotalDebit = tagInfo.PriorSums.Item1,
                                .TotalCredit = tagInfo.PriorSums.Item2
                            }
                            blocks.Add(currentBlock)
                        End If

                        Dim rInfo As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                        rInfo.RefNo = Convert.ToString(row.Cells("colRefNo").Value)
                        rInfo.EntryDate = Convert.ToString(row.Cells("colDate").Value)
                        rInfo.Description = Convert.ToString(row.Cells("colSharh").Value)
                        rInfo.Tashkhis = Convert.ToString(row.Cells("colTash").Value)
                        rInfo.DebitAmount = parseDec(row.Cells("colDebit").Value)
                        rInfo.CreditAmount = parseDec(row.Cells("colCredit").Value)
                        rInfo.BalanceAmount = parseDec(row.Cells("colBalance").Value)

                        currentBlock.Rows.Add(rInfo)

                        If rInfo.DebitAmount.HasValue Then currentBlock.TotalDebit += rInfo.DebitAmount.Value
                        If rInfo.CreditAmount.HasValue Then currentBlock.TotalCredit += rInfo.CreditAmount.Value
                    Next

                    For Each b In blocks
                        Dim diff = b.TotalDebit - b.TotalCredit
                        b.TotalBalance = Math.Abs(diff)
                        If diff > 0D Then
                            b.TotalTashkhis = "بدهکار"
                        ElseIf diff < 0D Then
                            b.TotalTashkhis = "بستانکار"
                        Else
                            b.TotalTashkhis = "تراز"
                        End If
                    Next
                Else
                    Dim blockChain As List(Of Tuple(Of String, String)) = Nothing
                    Dim blockLedgerTitle = "دفتر حساب"
                    Try
                        Dim chain = service.GetAccountHierarchyChain(_currentAccountId)
                        blockChain = chain
                        If chain IsNot Nothing Then
                            Select Case chain.Count
                                Case 1: blockLedgerTitle = "دفتر گروه"
                                Case 2: blockLedgerTitle = "دفتر کل"
                                Case 3: blockLedgerTitle = "دفتر معین"
                                Case Else: blockLedgerTitle = "دفتر تفصیلی"
                            End Select
                        End If
                    Catch
                    End Try

                    Dim block As New LedgerPrintBlock() With {
                        .LedgerTitle = blockLedgerTitle,
                        .AccountNameTitle = _currentAccountCode & " — " & _currentAccountName,
                        .AccountHierarchyChain = blockChain
                    }

                    For Each row As DataGridViewRow In dgvLedger.Rows
                        If row.IsNewRow Then Continue For
                        Dim tagStr = Convert.ToString(row.Tag)
                        If tagStr = "Header" OrElse tagStr = "Summary" Then Continue For

                        Dim rInfo As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                        rInfo.RefNo = Convert.ToString(row.Cells("colRefNo").Value)
                        rInfo.EntryDate = Convert.ToString(row.Cells("colDate").Value)
                        rInfo.Description = Convert.ToString(row.Cells("colSharh").Value)
                        rInfo.Tashkhis = Convert.ToString(row.Cells("colTash").Value)
                        rInfo.DebitAmount = parseDec(row.Cells("colDebit").Value)
                        rInfo.CreditAmount = parseDec(row.Cells("colCredit").Value)
                        rInfo.BalanceAmount = parseDec(row.Cells("colBalance").Value)

                        block.Rows.Add(rInfo)
                    Next

                    Dim parseTotal = Function(lblText As String) As Decimal
                                         If String.IsNullOrWhiteSpace(lblText) Then Return 0D
                                         Dim clean = lblText.Replace(",", "").Trim()
                                         Dim val As Decimal
                                         Decimal.TryParse(clean, val)
                                         Return val
                                     End Function

                    block.TotalDebit = parseTotal(lblSumDebit.Text)
                    block.TotalCredit = parseTotal(lblSumCredit.Text)
                    block.TotalBalance = parseTotal(lblSumBalance.Text)
                    block.TotalTashkhis = If(lblTash IsNot Nothing, lblTash.Text.Trim(), "")

                    blocks.Add(block)
                End If

                If blocks.Count = 0 Then
                    MessageBox.Show("هیچ داده‌ای برای چاپ وجود ندارد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Using printForm As New HesabdaryDaftarPrintForm(blocks)
                    printForm.ShowDialog(Me)
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در فراخوانی گزارش چاپ دفتر: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Class RowTagInfo
            Public Property EntryID As Integer
            Public Property LineNumber As Integer?
            Public Property AccountID As Integer
            Public Property AccountCode As String
            Public Property AccountName As String
            Public Property PriorSums As Tuple(Of Decimal, Decimal)

            Public Sub New(entryId As Integer, lineNum As Integer?, accId As Integer, accCode As String, accName As String, priorSums As Tuple(Of Decimal, Decimal))
                Me.EntryID = entryId
                Me.LineNumber = lineNum
                Me.AccountID = accId
                Me.AccountCode = accCode
                Me.AccountName = accName
                Me.PriorSums = priorSums
            End Sub
        End Class

        Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
            ExportGridToExcel(dgvLedger, "Ledger_Book")
        End Sub

        Private Sub ExportGridToExcel(dgv As DataGridView, defaultFileName As String)
            Using sfd As New SaveFileDialog()
                sfd.Filter = "Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*"
                sfd.Title = "خروجی اکسل"
                sfd.FileName = defaultFileName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim sb As New System.Text.StringBuilder()
                        
                        ' Write headers
                        Dim headers As New List(Of String)()
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                headers.Add(col.HeaderText)
                            End If
                        Next
                        sb.AppendLine(String.Join(",", headers))
                        
                        ' Write rows
                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For
                            
                            Dim cells As New List(Of String)()
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                    Dim val = Convert.ToString(row.Cells(col.Index).Value)
                                    ' Escape double quotes and wrap in double quotes if it contains commas or quotes
                                    If val.Contains(",") OrElse val.Contains("""") OrElse val.Contains(Microsoft.VisualBasic.ControlChars.CrLf) OrElse val.Contains(Microsoft.VisualBasic.ControlChars.Lf) Then
                                        val = """" & val.Replace("""", """""") & """"
                                    End If
                                    cells.Add(val)
                                End If
                            Next
                            sb.AppendLine(String.Join(",", cells))
                        Next
                        
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8)
                        MessageBox.Show("خروجی اکسل با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ذخیره فایل خروجی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub
    End Class
End Namespace
