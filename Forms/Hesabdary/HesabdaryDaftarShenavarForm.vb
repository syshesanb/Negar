Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Partial Class HesabdaryDaftarShenavarForm
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
        Private _returnTargetEntryID As Integer? = Nothing
        Private _returnTargetLineNumber As Integer? = Nothing
        Private _labelTextMain As String = ""
        Private _labelTextSub As String = ""

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryDaftarShenavarForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            Sys_Hes_Anb.Business.ThemeHelper.AppendStatusBar(Me)
            If Me.dgvLedger IsNot Nothing Then Me.dgvLedger.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            dgvLedger.RowTemplate.Height = 26
            cmbDescType.SelectedIndex = 0 ' پیش‌فرض: فقط شرح ردیف
            cmbStatus.SelectedIndex = 0 ' پیش‌فرض: موقت
            cmbSelectedAccounts.Items.Clear()
            cmbSelectedAccounts.Items.Add("چاپ تمام دفاتر")
            cmbSelectedAccounts.SelectedIndex = 0

            SetupSearchPanel()
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

        Public Sub LoadShenavar(accountId As Integer, accountCode As String, accountName As String,
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
                For Each acc In _selectedRangeAccounts
                    Dim block As New LedgerBlock() With {
                        .AccountID = acc.Item1,
                        .AccountCode = acc.Item2,
                        .AccountName = acc.Item3
                    }
                    blocks.Add(block)
                Next
            Else
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
                    progress.UpdateProgress(CInt(Math.Min(100, currentPct)), "در حال بارگذاری دفتر شناور: " & block.AccountCode & "...")

                    Dim chainStr = ""
                    Try
                        Dim chain = service.GetShenavarHierarchyChain(block.AccountID)
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

                    ' 2. Calculate prior sums
                    Dim priorDebit = 0D
                    Dim priorCredit = 0D
                    If chkFilterByDate.Checked OrElse chkFilterByDoc.Checked Then
                        Try
                            Dim beforeSums = service.GetShenavarLedgerBeforeSums(block.AccountID, fromDateStr, fromDoc, docStatus, allFiscalYears)
                            priorDebit = beforeSums.Item1
                            priorCredit = beforeSums.Item2
                        Catch
                        End Try
                    End If
                    block.PriorSums = Tuple.Create(priorDebit, priorCredit)

                    ' 3. Get ledger data
                    Try
                        Dim dt = service.GetShenavarLedgerData(block.AccountID, chkAggregate.Checked, fromDateStr, toDateStr, fromDoc, toDoc, docStatus, allFiscalYears)
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
                        MessageBox.Show("خطا در بارگذاری دفتر شناور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                Next

                If blocks.Count = 1 Then
                    _labelTextMain = "دفتر تفصیلی شناور :  " & _currentAccountCode & " — " & _currentAccountName
                    _labelTextSub = "سرفصل حساب :  " & blocks(0).HierarchyChain
                    lblAccountTitle.Text = " " ' Trigger repaint
                    _fullDataTable = blocks(0).LedgerData
                    _priorSums = blocks(0).PriorSums
                Else
                    _labelTextMain = String.Format("چاپ تمام دفاتر شناور (تعداد: {0})", blocks.Count)
                    _labelTextSub = ""
                    lblAccountTitle.Text = " " ' Trigger repaint
                    _fullDataTable = Nothing
                    _priorSums = Nothing
                End If

                FillGridWithBlocks(blocks)
            End Using
        End Sub

        Private Sub CollectAllShenavarIds(parentId As Integer, ids As List(Of Integer))
            ids.Add(parentId)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Try
                Dim dt = Sql.ExecuteTable("SELECT ShenavarID, ParentShenavarID FROM SarfaslShenavar WHERE CompanyID = ?", SessionContext.CurrentCompanyID.Value)
                Dim childMap As New Dictionary(Of Integer, List(Of Integer))()
                For Each row As DataRow In dt.Rows
                    Dim id = Convert.ToInt32(row("ShenavarID"))
                    Dim pVal = row("ParentShenavarID")
                    If pVal IsNot Nothing AndAlso Not Convert.IsDBNull(pVal) Then
                        Dim pId = Convert.ToInt32(pVal)
                        If Not childMap.ContainsKey(pId) Then
                            childMap(pId) = New List(Of Integer)()
                        End If
                        childMap(pId).Add(id)
                    End If
                Next

                Dim collect As Action(Of Integer) = Nothing
                collect = Sub(pid As Integer)
                              If childMap.ContainsKey(pid) Then
                                  For Each childId In childMap(pid)
                                      ids.Add(childId)
                                      collect(childId)
                                  Next
                              End If
                          End Sub
                collect(parentId)
            Catch
            End Try
        End Sub

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

                If showHeaderSummary Then
                    Dim firstRowIdx = dgvLedger.Rows.Add()
                    Dim firstRow = dgvLedger.Rows(firstRowIdx)
                    firstRow.Tag = "Header"
                    firstRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = ""}
                    firstRow.Cells("colRefNo").Value = ""
                    If dgvLedger.Columns("colLineNo").Visible Then
                        firstRow.Cells("colLineNo").Value = ""
                    End If
                    firstRow.Cells("colDate").Value = ""
                    firstRow.Cells("colSharh").Value = "شروع دفتر شناور: " & block.AccountCode & " — " & block.AccountName
                    firstRow.Cells("colAccountCode").Value = ""
                    firstRow.Cells("colAccountName").Value = ""
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
                        Dim stdAccIdObj = If(block.LedgerData.Columns.Contains("StandardAccountID"), row("StandardAccountID"), DBNull.Value)
                        Dim stdAccId As Integer? = If(stdAccIdObj Is DBNull.Value OrElse stdAccIdObj Is Nothing, Nothing, CType(Convert.ToInt32(stdAccIdObj), Integer?))
                        gr.Tag = New RowTagInfo(entryId, lineNumber, block.AccountID, block.AccountCode, block.AccountName, block.PriorSums, stdAccId)

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

                        gr.Cells("colAccountCode").Value = Convert.ToString(row("AccountCode"))
                        gr.Cells("colAccountName").Value = Convert.ToString(row("AccountName"))

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

                If showHeaderSummary Then
                    Dim sumRowIdx = dgvLedger.Rows.Add()
                    Dim sumRow = dgvLedger.Rows(sumRowIdx)
                    sumRow.Tag = "Summary"
                    sumRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = ""}
                    sumRow.Cells("colRefNo").Value = ""
                    If dgvLedger.Columns("colLineNo").Visible Then
                        sumRow.Cells("colLineNo").Value = ""
                    End If
                    sumRow.Cells("colDate").Value = ""
                    sumRow.Cells("colSharh").Value = "جمع دفتر شناور: " & block.AccountCode & " — " & block.AccountName
                    sumRow.Cells("colAccountCode").Value = ""
                    sumRow.Cells("colAccountName").Value = ""
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
            Using frm As New ShenavarCodingForm()
                frm.SelectMode = True
                If frm.ShowDialog(Me) = DialogResult.OK OrElse frm.SelectedShenavarID.HasValue Then
                    If frm.SelectedShenavarID.HasValue Then
                        Dim sId = frm.SelectedShenavarID.Value
                        Dim dt = Sql.ExecuteTable("SELECT AccountCode, AccountName FROM SarfaslShenavar WHERE ShenavarID = ?", sId)
                        If dt.Rows.Count > 0 Then
                            Dim sCode = Convert.ToString(dt.Rows(0)("AccountCode"))
                            Dim sName = Convert.ToString(dt.Rows(0)("AccountName"))
                            
                            Dim hasChildren = False
                            Dim dtChildren = Sql.ExecuteTable("SELECT COUNT(*) FROM SarfaslShenavar WHERE ParentShenavarID = ?", sId)
                            If dtChildren.Rows.Count > 0 AndAlso Convert.ToInt32(dtChildren.Rows(0)(0)) > 0 Then
                                hasChildren = True
                            End If
                            
                            Dim allIds As New List(Of Integer)()
                            CollectAllShenavarIds(sId, allIds)

                            LoadShenavar(sId, sCode, sName, hasChildren, allIds)
                        End If
                    End If
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

            Dim firstRowIdx = dgvLedger.Rows.Add()
            Dim firstRow = dgvLedger.Rows(firstRowIdx)
            firstRow.Tag = Nothing
            firstRow.Cells("colGoToDoc") = New DataGridViewTextBoxCell() With {.Value = ""}
            firstRow.Cells("colRefNo").Value = ""
            If dgvLedger.Columns("colLineNo").Visible Then
                firstRow.Cells("colLineNo").Value = ""
            End If
            firstRow.Cells("colDate").Value = ""
            firstRow.Cells("colSharh").Value = "گردش و مانده حساب قبلی"
            firstRow.Cells("colAccountCode").Value = ""
            firstRow.Cells("colAccountName").Value = ""
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

                Dim entryId = Convert.ToInt32(row("EntryID"))
                Dim lineNoObj = If(dt.Columns.Contains("LineNumber"), row("LineNumber"), DBNull.Value)
                Dim lineNumber As Integer? = If(lineNoObj Is DBNull.Value OrElse lineNoObj Is Nothing, Nothing, CType(Convert.ToInt32(lineNoObj), Integer?))
                Dim stdAccIdObj = If(dt.Columns.Contains("StandardAccountID"), row("StandardAccountID"), DBNull.Value)
                Dim stdAccId As Integer? = If(stdAccIdObj Is DBNull.Value OrElse stdAccIdObj Is Nothing, Nothing, CType(Convert.ToInt32(stdAccIdObj), Integer?))
                gr.Tag = New RowTagInfo(entryId, lineNumber, _currentAccountId, _currentAccountCode, _currentAccountName, _priorSums, stdAccId)

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

                gr.Cells("colAccountCode").Value = Convert.ToString(row("AccountCode"))
                gr.Cells("colAccountName").Value = Convert.ToString(row("AccountName"))

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

            Dim nameCol = dgvLedger.Columns("colSharh")
            If nameCol IsNot Nothing AndAlso nameCol.Visible Then
                Dim rect = dgvLedger.GetColumnDisplayRectangle(nameCol.Index, True)
                lblJamTitle.Left = rect.Left
                lblJamTitle.Width = rect.Width
                lblJamTitle.Visible = rect.Width > 0
                lblJamTitle.BringToFront()
            Else
                lblJamTitle.Visible = False
            End If

            AlignLabel("colDebit", lblSumDebit)
            AlignLabel("colCredit", lblSumCredit)
            AlignLabel("colTash", lblTash)
            AlignLabel("colBalance", lblSumBalance)
        End Sub

        Private Sub AlignLabel(columnName As String, label As Label)
            If label Is Nothing Then Return
            Dim col = dgvLedger.Columns(columnName)
            If col IsNot Nothing AndAlso col.Visible Then
                Dim rect = dgvLedger.GetColumnDisplayRectangle(col.Index, True)
                label.Left = rect.Left
                label.Width = rect.Width
                label.Visible = rect.Width > 0
                label.BringToFront()
            Else
                label.Visible = False
            End If
        End Sub

        Private Sub DgvLedger_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvLedger.Scroll
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub DgvLedger_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvLedger.ColumnWidthChanged
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub DgvLedger_Resize(sender As Object, e As EventArgs) Handles dgvLedger.Resize
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub HesabdaryDaftarShenavarForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            AlignJamLabels()
            AlignSearchBoxes()
        End Sub

        Private Sub HesabdaryDaftarForm_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
            If Me.Visible Then
                AlignJamLabels()
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub SetupSearchPanel()
            AddHandler txtSrcRefNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcLineNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDate.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcSharh.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcAccountCode.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcAccountName.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDebit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcCredit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcTash.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcBalance.TextChanged, AddressOf TxtSrcAny_TextChanged
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvLedger Is Nothing OrElse pnlSerch Is Nothing Then Return
            If dgvLedger.Columns.Count = 0 Then Return

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
            AlignTB(txtSrcAccountCode, "colAccountCode")
            AlignTB(txtSrcAccountName, "colAccountName")
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
                Dim codeText = txtSrcAccountCode.Text.Trim().ToLower()
                Dim nameText = txtSrcAccountName.Text.Trim().ToLower()
                Dim debitText = txtSrcDebit.Text.Trim().ToLower()
                Dim creditText = txtSrcCredit.Text.Trim().ToLower()
                Dim tashText = txtSrcTash.Text.Trim().ToLower()
                Dim balanceText = txtSrcBalance.Text.Trim().ToLower()

                Dim anyFilter = refText.Length > 0 OrElse lineText.Length > 0 OrElse
                                dateText.Length > 0 OrElse sharhText.Length > 0 OrElse
                                codeText.Length > 0 OrElse nameText.Length > 0 OrElse
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

                    Dim rowAccountCode = Convert.ToString(row("AccountCode")).ToLower()
                    Dim rowAccountName = Convert.ToString(row("AccountName")).ToLower()

                    Dim rowDebit = If(debit = 0D, "", debit.ToString("#,##0")).ToLower()
                    Dim rowCredit = If(credit = 0D, "", credit.ToString("#,##0")).ToLower()

                    Dim rowTash = ""
                    Dim rowBalanceVal = 0D
                    If chkRecalculateBalance.Checked Then
                        ' The balance will be calculated based on filtered set later, but for row-by-row match we use current running balance
                        rowBalanceVal = Math.Abs(balance)
                        If balance > 0D Then
                            rowTash = "بدهکار"
                        ElseIf balance < 0D Then
                            rowTash = "بستانکار"
                        Else
                            rowTash = "تراز"
                        End If
                    Else
                        Dim origBal = If(_fullDataTable.Columns.Contains("OriginalBalance") AndAlso Not row.IsNull("OriginalBalance"), Convert.ToDecimal(row("OriginalBalance")), balance)
                        rowBalanceVal = Math.Abs(origBal)
                        rowTash = If(_fullDataTable.Columns.Contains("OriginalTash") AndAlso Not row.IsNull("OriginalTash"), Convert.ToString(row("OriginalTash")), "")
                    End If
                    rowTash = rowTash.ToLower()
                    Dim rowBalance = rowBalanceVal.ToString("#,##0").ToLower()

                    ' Match all conditions
                    Dim matches = True
                    If refText.Length > 0 AndAlso Not rowRef.Contains(refText) Then matches = False
                    If lineText.Length > 0 AndAlso Not rowLine.Contains(lineText) Then matches = False
                    If dateText.Length > 0 AndAlso Not rowDate.Contains(dateText) Then matches = False
                    If sharhText.Length > 0 AndAlso Not rowSharh.Contains(sharhText) Then matches = False
                    If codeText.Length > 0 AndAlso Not rowAccountCode.Contains(codeText) Then matches = False
                    If nameText.Length > 0 AndAlso Not rowAccountName.Contains(nameText) Then matches = False
                    If debitText.Length > 0 AndAlso Not rowDebit.Contains(debitText) Then matches = False
                    If creditText.Length > 0 AndAlso Not rowCredit.Contains(creditText) Then matches = False
                    If tashText.Length > 0 AndAlso Not rowTash.Contains(tashText) Then matches = False
                    If balanceText.Length > 0 AndAlso Not rowBalance.Contains(balanceText) Then matches = False

                    If matches Then
                        filteredDt.ImportRow(row)
                    End If
                Next

                FillGrid(filteredDt, chkAggregate.Checked)
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub TxtSrcAny_TextChanged(sender As Object, e As EventArgs)
            ApplySearchFilter()
        End Sub

        Private Sub BtnClearSearch_Click(sender As Object, e As EventArgs) Handles btnClearSearch.Click
            RemoveHandler txtSrcRefNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcLineNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcDate.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcSharh.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcAccountCode.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcAccountName.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcDebit.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcCredit.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcTash.TextChanged, AddressOf TxtSrcAny_TextChanged
            RemoveHandler txtSrcBalance.TextChanged, AddressOf TxtSrcAny_TextChanged

            txtSrcRefNo.Clear()
            txtSrcLineNo.Clear()
            txtSrcDate.Clear()
            txtSrcSharh.Clear()
            txtSrcAccountCode.Clear()
            txtSrcAccountName.Clear()
            txtSrcDebit.Clear()
            txtSrcCredit.Clear()
            txtSrcTash.Clear()
            txtSrcBalance.Clear()

            AddHandler txtSrcRefNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcLineNo.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDate.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcSharh.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcAccountCode.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcAccountName.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDebit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcCredit.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcTash.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcBalance.TextChanged, AddressOf TxtSrcAny_TextChanged

            ApplySearchFilter()
        End Sub

        Private Sub DgvLedger_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLedger.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim colName = dgvLedger.Columns(e.ColumnIndex).Name

            If colName = "colGoToDoc" Then
                Dim tag = TryCast(dgvLedger.Rows(e.RowIndex).Tag, RowTagInfo)
                If tag IsNot Nothing AndAlso tag.EntryID > 0 Then
                    _returnTargetEntryID = tag.EntryID
                    _returnTargetLineNumber = tag.LineNumber
                    RaiseEvent EditDocumentRequested(tag.EntryID, tag.LineNumber)
                End If
            End If
        End Sub

        Private Sub BtnBackToTrial_Click(sender As Object, e As EventArgs) Handles btnBackToTrial.Click
            Dim parentPage = TryCast(Me.Parent, TabPage)
            If parentPage IsNot Nothing Then
                Dim tabCtrl = TryCast(parentPage.Parent, TabControl)
                If tabCtrl IsNot Nothing Then
                    For Each tp As TabPage In tabCtrl.TabPages
                        If tp.Text = "تراز شناور" Then
                            tabCtrl.SelectedTab = tp
                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        Private Class RowTagInfo
            Public Property EntryID As Integer
            Public Property LineNumber As Integer?
            Public Property AccountID As Integer
            Public Property AccountCode As String
            Public Property AccountName As String
            Public Property PriorSums As Tuple(Of Decimal, Decimal)
            Public Property StandardAccountID As Integer?

            Public Sub New(entryId As Integer, lineNum As Integer?, accId As Integer, accCode As String, accName As String, priorSums As Tuple(Of Decimal, Decimal), stdAccId As Integer?)
                Me.EntryID = entryId
                Me.LineNumber = lineNum
                Me.AccountID = accId
                Me.AccountCode = accCode
                Me.AccountName = accName
                Me.PriorSums = priorSums
                Me.StandardAccountID = stdAccId
            End Sub
        End Class

        Private Sub btnPrintDaftar_Click(sender As Object, e As EventArgs) Handles btnPrintDaftar.Click
            Try
                Dim printRows As New List(Of HesabdaryDaftarPrintForm.LedgerRowInfo)()

                For Each dgvRow As DataGridViewRow In dgvLedger.Rows
                    Dim tag = dgvRow.Tag
                    Dim rInfo As New HesabdaryDaftarPrintForm.LedgerRowInfo()
                    
                    rInfo.RefNo = Convert.ToString(dgvRow.Cells("colRefNo").Value)
                    rInfo.EntryDate = Convert.ToString(dgvRow.Cells("colDate").Value)
                    
                    Dim sharh = Convert.ToString(dgvRow.Cells("colSharh").Value)
                    Dim accCode = Convert.ToString(dgvRow.Cells("colAccountCode").Value)
                    Dim accName = Convert.ToString(dgvRow.Cells("colAccountName").Value)
                    If Not String.IsNullOrEmpty(accCode) Then
                        sharh = sharh & " [حسابداری متقابل: " & accCode & " - " & accName & "]"
                    End If
                    rInfo.Description = sharh

                    Dim dVal = Convert.ToString(dgvRow.Cells("colDebit").Value).Replace(",", "")
                    If Not String.IsNullOrEmpty(dVal) Then
                        Dim dDec As Decimal
                        If Decimal.TryParse(dVal, dDec) Then rInfo.DebitAmount = dDec
                    End If

                    Dim cVal = Convert.ToString(dgvRow.Cells("colCredit").Value).Replace(",", "")
                    If Not String.IsNullOrEmpty(cVal) Then
                        Dim creditValDec As Decimal
                        If Decimal.TryParse(cVal, creditValDec) Then rInfo.CreditAmount = creditValDec
                    End If

                    rInfo.Tashkhis = Convert.ToString(dgvRow.Cells("colTash").Value)

                    Dim bVal = Convert.ToString(dgvRow.Cells("colBalance").Value).Replace(",", "")
                    If Not String.IsNullOrEmpty(bVal) Then
                        Dim bDec As Decimal
                        If Decimal.TryParse(bVal, bDec) Then rInfo.BalanceAmount = bDec
                    End If

                    If TypeOf tag Is String AndAlso Convert.ToString(tag) = "Header" Then
                        rInfo.IsHeader = True
                    ElseIf TypeOf tag Is String AndAlso Convert.ToString(tag) = "Summary" Then
                        rInfo.IsSummary = True
                    End If

                    printRows.Add(rInfo)
                Next

                Dim parseAmount = Function(text As String) As Decimal
                                      If String.IsNullOrWhiteSpace(text) Then Return 0D
                                      Dim val As Decimal
                                      Decimal.TryParse(text.Replace(",", "").Trim(), val)
                                      Return val
                                  End Function

                Dim totalDebit = parseAmount(lblSumDebit.Text)
                Dim totalCredit = parseAmount(lblSumCredit.Text)
                Dim totalBalance = parseAmount(lblSumBalance.Text)
                Dim totalTash = lblTash.Text

                Using printForm As New HesabdaryDaftarPrintForm("دفتر تفصیلی شناور", _currentAccountCode & " - " & _currentAccountName, printRows, totalDebit, totalCredit, totalBalance, totalTash)
                    printForm.ShowDialog(Me)
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در فراخوانی گزارش چاپ دفتر: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
            ExportGridToExcel(dgvLedger, "Floating_Ledger")
        End Sub

        Private Sub ExportGridToExcel(dgv As DataGridView, defaultFileName As String)
            Using sfd As New SaveFileDialog()
                sfd.Filter = "Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*"
                sfd.Title = "خروجی اکسل"
                sfd.FileName = defaultFileName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim sb As New System.Text.StringBuilder()
                        
                        Dim headers As New List(Of String)()
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                headers.Add(col.HeaderText)
                            End If
                        Next
                        sb.AppendLine(String.Join(",", headers))
                        
                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For
                            
                            Dim cells As New List(Of String)()
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn Then
                                    Dim val = Convert.ToString(row.Cells(col.Index).Value)
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

        Private Sub lblAccountTitle_Paint(sender As Object, e As PaintEventArgs) Handles lblAccountTitle.Paint
            If String.IsNullOrEmpty(_labelTextMain) Then Return

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Dim fontMain = lblAccountTitle.Font
            Dim fontSub = New Font(fontMain.FontFamily, fontMain.Size - 0.5F, FontStyle.Regular)

            Dim colorMain = lblAccountTitle.ForeColor
            Dim colorSub = Color.FromArgb(120, 135, 155) ' Mild color

            Dim sizeMain = e.Graphics.MeasureString(_labelTextMain, fontMain)
            Dim sizeSub = If(String.IsNullOrEmpty(_labelTextSub), New SizeF(0, 0), e.Graphics.MeasureString(_labelTextSub, fontSub))

            Dim yMain = (lblAccountTitle.Height - sizeMain.Height) / 2
            Dim ySub = (lblAccountTitle.Height - sizeSub.Height) / 2

            ' Align Right-to-Left
            Dim currentX = lblAccountTitle.Width - 15

            ' Draw main text
            currentX -= sizeMain.Width
            Using brushMain As New SolidBrush(colorMain)
                e.Graphics.DrawString(_labelTextMain, fontMain, brushMain, currentX, yMain)
            End Using

            ' Draw sub text
            If Not String.IsNullOrEmpty(_labelTextSub) Then
                currentX -= (sizeSub.Width + 40) ' 40px spacing
                Using brushSub As New SolidBrush(colorSub)
                    e.Graphics.DrawString(_labelTextSub, fontSub, brushSub, currentX, ySub)
                End Using
            End If
        End Sub

        Private Sub dgvLedger_SelectionChanged(sender As Object, e As EventArgs) Handles dgvLedger.SelectionChanged
            UpdateHeaderTitleForSelectedRow()
        End Sub

        Private Sub UpdateHeaderTitleForSelectedRow()
            If dgvLedger.CurrentRow Is Nothing OrElse dgvLedger.CurrentRow.Index < 0 Then Return
            Dim gr = dgvLedger.CurrentRow
            Dim tag = TryCast(gr.Tag, RowTagInfo)
            
            Dim mainTitle = "دفتر تفصیلی شناور :  " & _currentAccountCode & " — " & _currentAccountName
            Dim subTitle = ""
            
            If tag IsNot Nothing AndAlso tag.StandardAccountID.HasValue AndAlso tag.StandardAccountID.Value > 0 Then
                Try
                    Dim chain = service.GetAccountHierarchyChain(tag.StandardAccountID.Value)
                    Dim parts As New List(Of String)()
                    For Each item In chain
                        parts.Add(item.Item1 & " — " & item.Item2)
                    Next
                    subTitle = "سرفصل حساب :  " & String.Join(" / ", parts.ToArray())
                Catch
                End Try
            End If
            
            If String.IsNullOrEmpty(subTitle) AndAlso _currentAccountId > 0 Then
                Try
                    Dim chain = service.GetShenavarHierarchyChain(_currentAccountId)
                    Dim parts As New List(Of String)()
                    For Each item In chain
                        parts.Add(item.Item1 & " — " & item.Item2)
                    Next
                    subTitle = "سرفصل حساب :  " & String.Join(" / ", parts.ToArray())
                Catch
                End Try
            End If
            
            _labelTextMain = mainTitle
            _labelTextSub = subTitle
            lblAccountTitle.Invalidate()
        End Sub
    End Class
End Namespace
