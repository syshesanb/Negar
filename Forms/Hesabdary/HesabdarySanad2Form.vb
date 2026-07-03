Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class HesabdarySanad2Form
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private ReadOnly shenavarService As New ShenavarService()
        Private _editEntryId As Integer? = Nothing
        Private _copyEntryId As Integer? = Nothing
        Private _suggestedRef As String = String.Empty
        Private _suppressDateChange As Boolean = False
        Private _suppressDgvDateChange As Boolean = False
        Private _sarfaslTargetRow As Integer = -1
        Private _shenavarTargetRow As Integer = -1
        Private _isLoading As Boolean = False
        Private _isDirty As Boolean = False
        Private _suppressCloseConfirmation As Boolean = False

        ' کَش زنجیره سرفصل و شناور — کلید: ID، مقدار: رشته زنجیره فرمت‌شده
        Private _accountChainCache As New Dictionary(Of Integer, String)()
        Private _shenavarChainCache As New Dictionary(Of Integer, String)()

        Private _targetLineNumber As Integer? = Nothing
        Public Property TargetLineNumber As Integer?
            Get
                Return _targetLineNumber
            End Get
            Set(value As Integer?)
                _targetLineNumber = value
            End Set
        End Property

        Private Const TotalPreloadedRows As Integer = 100

        ' حالت سند جدید
        Public Sub New(suggestedRef As String)
            InitializeComponent()
            _suggestedRef = suggestedRef
        End Sub

        ' حالت ویرایش
        Public Sub New(entryId As Integer)
            InitializeComponent()
            _editEntryId = entryId
        End Sub

        ' حالت کپی سند
        Public Sub New(suggestedRef As String, entryIdToCopy As Integer)
            InitializeComponent()
            _suggestedRef = suggestedRef
            _copyEntryId = entryIdToCopy
        End Sub

        Private Sub HesabdarySanad2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            _isLoading = True
            Me.KeyPreview = True
            service.EnsureEntryDetailsColumns()
            InitializeEntryGrid()
            ApplySFSHColumnsVisibilityByPermission()
            AlignSearchBoxes()
            If _editEntryId.HasValue Then
                Me.Text = "ویرایش سند حسابداری"
                LoadEntryForEdit(_editEntryId.Value)
                btnSaveAndContinue.Visible = False
            ElseIf _copyEntryId.HasValue Then
                Me.Text = "ثبت سند حسابداری جدید"
                LoadEntryForEdit(_copyEntryId.Value)
                txtEntryReference.Text = _suggestedRef
                txtDateSanad.Text = PersianDateHelper.ToPersian(DateTime.Today)
                btnSaveAndContinue.Visible = True
            Else
                Me.Text = "ثبت سند حسابداری جدید"
                txtEntryReference.Text = _suggestedRef
                txtDateSanad.Text = PersianDateHelper.ToPersian(DateTime.Today)
                btnSaveAndContinue.Visible = True
            End If
            UpdateJamLabels()

            If _targetLineNumber.HasValue Then
                Dim targetIndex = _targetLineNumber.Value - 1
                If targetIndex >= 0 AndAlso targetIndex < dgvEntryLines.Rows.Count Then
                    Me.BeginInvoke(New Action(Sub()
                                                  Try
                                                      dgvEntryLines.CurrentCell = dgvEntryLines.Rows(targetIndex).Cells("colSharhRadif")
                                                      dgvEntryLines.FirstDisplayedScrollingRowIndex = targetIndex
                                                  Catch
                                                  End Try
                                              End Sub))
                End If
            End If
            _isLoading = False
            _isDirty = False
            _suppressCloseConfirmation = False
        End Sub

        Private Sub HeaderControl_TextChanged(sender As Object, e As EventArgs) Handles txtEntryReference.TextChanged, txtEntryDescription.TextChanged
            If Not _isLoading Then _isDirty = True
        End Sub

        Private Sub InitializeEntryGrid()
            If dgvEntryLines.Columns.Count > 0 Then Return

            ' ساختار DataTable
            Dim table As New DataTable()
            table.Columns.Add("LineNumber", GetType(Integer))
            table.Columns.Add("AccountID", GetType(Integer))
            table.Columns.Add("AccountCode", GetType(String))
            table.Columns.Add("AccountName", GetType(String))
            table.Columns.Add("ShenavarID", GetType(Integer))
            table.Columns.Add("ShenavarCode", GetType(String))
            table.Columns.Add("SharhRadif", GetType(String))
            table.Columns.Add("DebitAmount", GetType(Decimal))
            table.Columns.Add("CreditAmount", GetType(Decimal))
            table.Columns.Add("TransactionNumber", GetType(String))
            table.Columns.Add("TransactionDate", GetType(String))

            ' ۱۰۰ ردیف خالی از پیش بارگذاری شده
            For i = 1 To TotalPreloadedRows
                table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, "", "")
            Next

            AddHandler table.ColumnChanged, Sub(s, ea)
                                                If Not _isLoading Then _isDirty = True
                                            End Sub
            AddHandler table.RowDeleted, Sub(s, ea)
                                             If Not _isLoading Then _isDirty = True
                                         End Sub
            AddHandler table.RowChanged, Sub(s, ea)
                                           If Not _isLoading Then _isDirty = True
                                       End Sub

            dgvEntryLines.AutoGenerateColumns = False
            dgvEntryLines.ReadOnly = False
            dgvEntryLines.DataSource = table
            dgvEntryLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' ستون ردیف — حداکثر 3 رقم (1-999)
            Dim colLineNo As New DataGridViewTextBoxColumn()
            colLineNo.Name = "colLineNo"
            colLineNo.DataPropertyName = "LineNumber"
            colLineNo.HeaderText = "ردیف"
            colLineNo.Width = 45
            colLineNo.ReadOnly = True

            ' دکمه کد سرفصل — عرض متناسب با متن دکمه
            Dim colBtnSarfasl As New DataGridViewButtonColumn()
            colBtnSarfasl.Name = "colBtnSarfasl"
            colBtnSarfasl.HeaderText = "کد سرفصل"
            colBtnSarfasl.Text = "کد سرفصل"
            colBtnSarfasl.UseColumnTextForButtonValue = True
            colBtnSarfasl.Width = 80
            colBtnSarfasl.FlatStyle = FlatStyle.Standard

            ' دکمه کد شناور — عرض متناسب با متن دکمه
            Dim colBtnShenaVar As New DataGridViewButtonColumn()
            colBtnShenaVar.Name = "colBtnShenaVar"
            colBtnShenaVar.HeaderText = "کد شناور"
            colBtnShenaVar.Text = "کد شناور"
            colBtnShenaVar.UseColumnTextForButtonValue = True
            colBtnShenaVar.Width = 75
            colBtnShenaVar.FlatStyle = FlatStyle.Standard

            ' شرح ردیف
            Dim colSharhRadif As New DataGridViewTextBoxColumn()
            colSharhRadif.Name = "colSharhRadif"
            colSharhRadif.DataPropertyName = "SharhRadif"
            colSharhRadif.HeaderText = "شرح ردیف"
            colSharhRadif.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colSharhRadif.DefaultCellStyle.WrapMode = DataGridViewTriState.True

            ' بدهکار — حداکثر 22 رقم، با جداکننده هزارگان
            Dim colDebit As New DataGridViewTextBoxColumn()
            colDebit.Name = "colDebit"
            colDebit.DataPropertyName = "DebitAmount"
            colDebit.HeaderText = "بدهکار"
            colDebit.Width = 200

            ' بستانکار — حداکثر 22 رقم، با جداکننده هزارگان
            Dim colCredit As New DataGridViewTextBoxColumn()
            colCredit.Name = "colCredit"
            colCredit.DataPropertyName = "CreditAmount"
            colCredit.HeaderText = "بستانکار"
            colCredit.Width = 200

            ' شماره تراکنش
            Dim colTransNo As New DataGridViewTextBoxColumn()
            colTransNo.Name = "colTransNo"
            colTransNo.DataPropertyName = "TransactionNumber"
            colTransNo.HeaderText = "شماره تراکنش"
            colTransNo.Width = 120

            ' دکمه TT — باز کردن تقویم برای ستون تاریخ تراکنش
            Dim colBtnTT As New DataGridViewButtonColumn()
            colBtnTT.Name = "colBtnTT"
            colBtnTT.HeaderText = "TT"
            colBtnTT.Text = "..."
            colBtnTT.UseColumnTextForButtonValue = True
            colBtnTT.Width = 38
            colBtnTT.FlatStyle = FlatStyle.Standard

            ' تاریخ تراکنش — فرمت yyyy/mm/dd شمسی
            Dim colTransDate As New DataGridViewTextBoxColumn()
            colTransDate.Name = "colTransDate"
            colTransDate.DataPropertyName = "TransactionDate"
            colTransDate.HeaderText = "تاریخ تراکنش"
            colTransDate.Width = 95

            ' ستون SF — نگه‌دارنده AccountID برای ذخیره‌سازی (فقط خواندنی، عرض = سرستون)
            Dim colSF As New DataGridViewTextBoxColumn()
            colSF.Name = "colSF"
            colSF.DataPropertyName = "AccountID"
            colSF.HeaderText = "SF"
            colSF.Width = 35
            colSF.ReadOnly = True

            ' ستون SH — نگه‌دارنده ShenavarID برای ذخیره‌سازی (فقط خواندنی، عرض = سرستون)
            Dim colSH As New DataGridViewTextBoxColumn()
            colSH.Name = "colSH"
            colSH.DataPropertyName = "ShenavarID"
            colSH.HeaderText = "SH"
            colSH.Width = 35
            colSH.ReadOnly = True

            dgvEntryLines.Columns.AddRange(New DataGridViewColumn() {
                colLineNo, colBtnSarfasl, colSF,
                colBtnShenaVar, colSH,
                colSharhRadif, colDebit, colCredit, colTransNo, colBtnTT, colTransDate})
            dgvEntryLines.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        End Sub



        ' =====================================================================
        ' ---- نمایش زنجیره سرفصل / شناور ردیف جاری ----

        ' RowEnter: لیبل‌ها را فوری خالی می‌کند و بارگذاری واقعی را دیفر می‌کند
        ' تا ناوبری بین سطرها بدون تأخیر احساس شود
        Private Sub dgvEntryLines_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.RowEnter
            If e.RowIndex < 0 Then Return
            lblSarfaslValue.Text = ""
            lblShenavarValue.Text = ""
            Dim rowIdx = e.RowIndex
            Me.BeginInvoke(New Action(Sub() UpdateRowInfoLabels(rowIdx)))
        End Sub

        Private Sub UpdateRowInfoLabels(rowIndex As Integer)
            If rowIndex < 0 OrElse rowIndex >= dgvEntryLines.Rows.Count Then Return
            Dim row = dgvEntryLines.Rows(rowIndex)

            Dim sfVal = row.Cells("colSF").Value
            Dim accountId = If(sfVal IsNot Nothing AndAlso sfVal IsNot DBNull.Value, Convert.ToInt32(sfVal), 0)
            lblSarfaslValue.Text = If(accountId > 0, BuildAccountChain(accountId), "")

            Dim shVal = row.Cells("colSH").Value
            Dim shenavarId = If(shVal IsNot Nothing AndAlso shVal IsNot DBNull.Value, Convert.ToInt32(shVal), 0)
            lblShenavarValue.Text = If(shenavarId > 0, BuildShenavarChain(shenavarId), "")
        End Sub

        ' ساخت رشته زنجیره سرفصل از ریشه تا حساب انتخابی — با کَش
        ' فرمت: کد والد : نام والد / کد فرزند اول : نام فرزند اول / ...
        Private Function BuildAccountChain(accountId As Integer) As String
            If _accountChainCache.ContainsKey(accountId) Then Return _accountChainCache(accountId)
            Dim chain As New List(Of String)()
            Dim currentId As Integer? = accountId
            Dim safety = 0
            While currentId.HasValue AndAlso safety < 20
                Dim info = service.GetAccountInfo(currentId.Value)
                chain.Add(info.Item1 & " : " & info.Item2)
                currentId = service.GetAccountParentId(currentId.Value)
                safety += 1
            End While
            chain.Reverse()
            Dim result = String.Join(" / ", chain.ToArray())
            _accountChainCache(accountId) = result
            Return result
        End Function

        ' ساخت رشته زنجیره شناور از ریشه تا حساب انتخابی — با کَش
        Private Function BuildShenavarChain(shenavarId As Integer) As String
            If _shenavarChainCache.ContainsKey(shenavarId) Then Return _shenavarChainCache(shenavarId)
            Dim chain As New List(Of String)()
            Dim currentId As Integer? = shenavarId
            Dim safety = 0
            While currentId.HasValue AndAlso safety < 20
                Dim info = shenavarService.GetItemInfo(currentId.Value)
                chain.Add(info.Item1 & " : " & info.Item2)
                currentId = shenavarService.GetParentId(currentId.Value)
                safety += 1
            End While
            chain.Reverse()
            Dim result = String.Join(" / ", chain.ToArray())
            _shenavarChainCache(shenavarId) = result
            Return result
        End Function

        ' تنظیم موقعیت و عرض TextBoxهای پنل جستجو بر اساس ستون‌های گرید
        ' هر TextBox می‌تواند روی یک یا چند ستون متوالی پوشیده شود
        Private Sub AlignSearchBoxes()
            ' تابع کمکی: TextBox tb را روی مجموع ستون‌های colNames قرار می‌دهد
            Dim AlignTB = Sub(tb As TextBox, colNames As String())
                              Dim totalW As Integer = 0
                              Dim minX As Integer = Integer.MaxValue
                              Dim anyVisible As Boolean = False
                              For Each cn In colNames
                                  Dim col = dgvEntryLines.Columns(cn)
                                  If col Is Nothing OrElse Not col.Visible Then Continue For
                                  
                                  Dim r = dgvEntryLines.GetColumnDisplayRectangle(col.Index, True)
                                  If r.IsEmpty OrElse r.Width = 0 Then Continue For
                                  
                                  totalW += r.Width
                                  If r.X < minX Then minX = r.X
                                  anyVisible = True
                              Next
                              If Not anyVisible Then
                                  tb.Visible = False
                                  Return
                              End If
                              Dim screenPt = dgvEntryLines.PointToScreen(New Point(minX, 0))
                              Dim panelPt = pnlSerch.PointToClient(screenPt)
                              tb.Location = New Point(panelPt.X, 4)
                              tb.Width = totalW
                              tb.Visible = True
                          End Sub

            AlignTB(txtSrcLineNo, New String() {"colLineNo"})
            AlignTB(txtSrcSF, New String() {"colBtnSarfasl", "colSF"})
            AlignTB(txtSrcSH, New String() {"colBtnShenaVar", "colSH"})
            AlignTB(txtSrcSharh, New String() {"colSharhRadif"})
            AlignTB(txtSrcDebit, New String() {"colDebit"})
            AlignTB(txtSrcCredit, New String() {"colCredit"})
            AlignTB(txtSrcTransNo, New String() {"colTransNo"})
            AlignTB(txtSrcTransDate, New String() {"colTransDate"})
            AlignJamBoxes()
        End Sub

        Private Sub dgvEntryLines_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvEntryLines.ColumnWidthChanged
            AlignSearchBoxes()
        End Sub

        Private Sub dgvEntryLines_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvEntryLines.Scroll
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub HesabdarySanad2Form_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
            AlignSearchBoxes()
        End Sub

        Private Sub LoadEntryForEdit(entryId As Integer)
            Dim headerRow = service.GetEntryById(entryId)
            If headerRow Is Nothing Then Return
            txtEntryDescription.Text = Convert.ToString(headerRow("Description"))
            txtEntryReference.Text = Convert.ToString(headerRow("ReferenceNumber"))
            Dim entryDateVal = headerRow("EntryDate")
            If entryDateVal IsNot Nothing AndAlso Not Convert.IsDBNull(entryDateVal) Then
                txtDateSanad.Text = PersianDateHelper.ToPersian(Convert.ToDateTime(entryDateVal))
            End If

            Dim details = service.GetEntryDetails(entryId)
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            For Each dr As DataRow In details.Rows
                Dim lineNo = Convert.ToInt32(If(dr("LineNumber") Is DBNull.Value OrElse dr("LineNumber") Is Nothing, 0, dr("LineNumber")))
                Dim rowIndex = lineNo - 1

                Dim targetRow As DataRow
                If rowIndex >= 0 AndAlso rowIndex < table.Rows.Count Then
                    targetRow = table.Rows(rowIndex)
                Else
                    ' LineNumber خارج از محدوده ۱۰۰ — ردیف جدید اضافه کن
                    targetRow = table.NewRow()
                    targetRow("LineNumber") = If(lineNo > 0, lineNo, table.Rows.Count + 1)
                    table.Rows.Add(targetRow)
                End If

                targetRow("AccountID") = Convert.ToInt32(If(dr("AccountID") Is DBNull.Value, 0, dr("AccountID")))
                targetRow("AccountCode") = Convert.ToString(If(dr("AccountCode") Is DBNull.Value, "", dr("AccountCode")))
                targetRow("AccountName") = Convert.ToString(If(dr("AccountName") Is DBNull.Value, "", dr("AccountName")))
                targetRow("ShenavarID") = Convert.ToInt32(If(dr("ShenavarID") Is DBNull.Value, 0, dr("ShenavarID")))
                targetRow("DebitAmount") = Convert.ToDecimal(If(dr("DebitAmount") Is DBNull.Value, 0D, dr("DebitAmount")))
                targetRow("CreditAmount") = Convert.ToDecimal(If(dr("CreditAmount") Is DBNull.Value, 0D, dr("CreditAmount")))
                ' ستون‌های اختیاری: فقط در صورت وجود در DataTable نتیجه بارگذاری می‌شوند
                If details.Columns.Contains("SharhRadif") Then
                    targetRow("SharhRadif") = Convert.ToString(If(dr("SharhRadif") Is DBNull.Value, "", dr("SharhRadif")))
                End If
                If details.Columns.Contains("TransactionNumber") Then
                    targetRow("TransactionNumber") = Convert.ToString(If(dr("TransactionNumber") Is DBNull.Value, "", dr("TransactionNumber")))
                End If
                If details.Columns.Contains("TransactionDate") Then
                    targetRow("TransactionDate") = Convert.ToString(If(dr("TransactionDate") Is DBNull.Value, "", dr("TransactionDate")))
                End If
            Next
            UpdateJamLabels()
        End Sub

        ' ---- تاریخ سند: ورودی شمسی با auto-format و تقویم popup ----

        Private Sub txtDateSanad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDateSanad.KeyPress
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub txtDateSanad_TextChanged(sender As Object, e As EventArgs) Handles txtDateSanad.TextChanged
            If _suppressDateChange Then Return
            Dim txt = txtDateSanad.Text
            Dim digits = New String(txt.Where(Function(c) Char.IsDigit(c)).ToArray())
            If digits.Length > 8 Then digits = digits.Substring(0, 8)
            Dim formatted = FormatPersianDigits(digits)
            If formatted = txt Then Return
            _suppressDateChange = True
            txtDateSanad.Text = formatted
            txtDateSanad.SelectionStart = formatted.Length
            _suppressDateChange = False
        End Sub

        Private Sub btnCalDate_Click(sender As Object, e As EventArgs) Handles btnCalDate.Click
            Dim anchor = EnsureOnScreen(
                txtDateSanad.PointToScreen(New Point(0, txtDateSanad.Height)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(txtDateSanad.Text)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    txtDateSanad.Text = cal.SelectedDate
                End If
            End Using
        End Sub

        ' ---- تاریخ تراکنش در دیتاگریدویو ----

        ' کلیک روی دکمه‌های داخل گرید
        Private Sub dgvEntryLines_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name

            Select Case colName
                Case "colBtnSarfasl"
                    _sarfaslTargetRow = e.RowIndex
                    ShowCodingFormForSelection()

                Case "colBtnShenaVar"
                    Dim sfVal = dgvEntryLines.Rows(e.RowIndex).Cells("colSF").Value
                    Dim sfId = If(sfVal IsNot Nothing AndAlso sfVal IsNot DBNull.Value, Convert.ToInt32(sfVal), 0)
                    If sfId <= 0 Then
                        MessageBox.Show("شما باید ابتدا کد سرفصل حساب را انتخاب کنید.",
                                        "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    _shenavarTargetRow = e.RowIndex
                    ShowShenavarFormForSelection()

                Case "colBtnTT"
                    Dim dateColIndex = dgvEntryLines.Columns("colTransDate").Index
                    Dim current = Convert.ToString(dgvEntryLines.Rows(e.RowIndex).Cells(dateColIndex).Value)
                    Dim cellRect = dgvEntryLines.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
                    Dim anchor = EnsureOnScreen(
                        dgvEntryLines.PointToScreen(New Point(cellRect.X, cellRect.Bottom)),
                        New Size(270, 228))
                    Using cal As New PersianCalendarForm(current)
                        cal.StartPosition = FormStartPosition.Manual
                        cal.Location = anchor
                        If cal.ShowDialog(Me) = DialogResult.OK Then
                            dgvEntryLines.Rows(e.RowIndex).Cells(dateColIndex).Value = cal.SelectedDate
                        End If
                    End Using
            End Select
        End Sub

        ' باز کردن فرم سرفصل در حالت انتخاب — به عنوان overlay در همان TabPage
        Private Sub ShowCodingFormForSelection()
            Using codingForm As New HesabdaryCodingForm()
                codingForm.SelectMode = True

                ' تنظیم ابعاد یکنواخت و موقعیت دیالوگ مطابق تصویر پیوست دوم
                codingForm.Size = New Size(760, 380)
                codingForm.StartPosition = FormStartPosition.CenterParent

                codingForm.ShowDialog(Me)

                If codingForm.SelectedAccountID.HasValue AndAlso _sarfaslTargetRow >= 0 Then
                    Dim targetRow = dgvEntryLines.Rows(_sarfaslTargetRow)
                    targetRow.Cells("colSF").Value = codingForm.SelectedAccountID.Value
                    
                    If targetRow.DataBoundItem IsNot Nothing Then
                        Dim drv = TryCast(targetRow.DataBoundItem, DataRowView)
                        If drv IsNot Nothing AndAlso drv.Row IsNot Nothing Then
                            Dim dr = drv.Row
                            dr("AccountID") = codingForm.SelectedAccountID.Value
                            Dim info = service.GetAccountInfo(codingForm.SelectedAccountID.Value)
                            dr("AccountCode") = info.Item1
                            dr("AccountName") = info.Item2
                        End If
                    End If
                    
                    UpdateRowInfoLabels(_sarfaslTargetRow)
                    dgvEntryLines.InvalidateRow(_sarfaslTargetRow)
                End If
                _sarfaslTargetRow = -1
            End Using
        End Sub

        ' باز کردن فرم شناور در حالت انتخاب — به عنوان overlay در همان TabPage
        Private Sub ShowShenavarFormForSelection()
            Dim shenavarForm As New ShenavarCodingForm()
            shenavarForm.SelectMode = True

            Dim parentContainer = Me.Parent   ' TabPage
            If parentContainer Is Nothing Then
                shenavarForm.ShowDialog(Me)
                If shenavarForm.SelectedShenavarID.HasValue AndAlso _shenavarTargetRow >= 0 Then
                    dgvEntryLines.Rows(_shenavarTargetRow).Cells("colSH").Value = shenavarForm.SelectedShenavarID.Value
                    UpdateRowInfoLabels(_shenavarTargetRow)
                End If
                shenavarForm.Dispose()
                _shenavarTargetRow = -1
                Return
            End If

            shenavarForm.TopLevel = False
            shenavarForm.FormBorderStyle = FormBorderStyle.None
            shenavarForm.Dock = DockStyle.Fill

            AddHandler shenavarForm.FormClosed,
                Sub(s As Object, ea As FormClosedEventArgs)
                    Dim selectedId = shenavarForm.SelectedShenavarID
                    parentContainer.Controls.Remove(shenavarForm)
                    shenavarForm.Dispose()
                    If selectedId.HasValue AndAlso _shenavarTargetRow >= 0 Then
                        dgvEntryLines.Rows(_shenavarTargetRow).Cells("colSH").Value = selectedId.Value
                        UpdateRowInfoLabels(_shenavarTargetRow)
                    End If
                    _shenavarTargetRow = -1
                End Sub

            parentContainer.Controls.Add(shenavarForm)
            shenavarForm.BringToFront()
            shenavarForm.Show()
        End Sub

        ' رنگ‌آمیزی دکمه کد شناور: خاکستری (غیرفعال) تا زمانی که سرفصل انتخاب نشده
        Private Sub dgvEntryLines_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvEntryLines.CellPainting
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvEntryLines.Columns(e.ColumnIndex).Name <> "colBtnShenaVar" Then Return

            Dim sfVal = dgvEntryLines.Rows(e.RowIndex).Cells("colSF").Value
            Dim sfId = If(sfVal IsNot Nothing AndAlso sfVal IsNot DBNull.Value, Convert.ToInt32(sfVal), 0)
            If sfId > 0 Then Return  ' فعال: رنگ پیش‌فرض

            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border)
            Dim fillRect = Rectangle.Inflate(e.CellBounds, -2, -2)
            Using brush As New SolidBrush(Color.FromArgb(210, 210, 210))
                e.Graphics.FillRectangle(brush, fillRect)
            End Using
            TextRenderer.DrawText(e.Graphics, "کد شناور", dgvEntryLines.Font, e.CellBounds,
                                  Color.FromArgb(140, 140, 140),
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End Sub

        ' دابل‌کلیک روی خود ستون تاریخ → باز کردن تقویم (روش جایگزین)
        Private Sub dgvEntryLines_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellDoubleClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvEntryLines.Columns(e.ColumnIndex).Name <> "colTransDate" Then Return
            dgvEntryLines.EndEdit()
            Dim current = Convert.ToString(dgvEntryLines.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
            Dim cellRect = dgvEntryLines.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            Dim anchor = EnsureOnScreen(
                dgvEntryLines.PointToScreen(New Point(cellRect.X, cellRect.Bottom)),
                New Size(270, 228))
            Using cal As New PersianCalendarForm(current)
                cal.StartPosition = FormStartPosition.Manual
                cal.Location = anchor
                If cal.ShowDialog(Me) = DialogResult.OK Then
                    dgvEntryLines.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = cal.SelectedDate
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, formSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - formSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - formSize.Height)))
        End Function

        Private Shared Function FormatPersianDigits(digits As String) As String
            Select Case digits.Length
                Case <= 4 : Return digits
                Case <= 6 : Return digits.Substring(0, 4) & "/" & digits.Substring(4)
                Case Else : Return digits.Substring(0, 4) & "/" & digits.Substring(4, 2) & "/" & digits.Substring(6)
            End Select
        End Function

        ' ---- اعتبارسنجی و فرمت‌بندی ستون‌های بدهکار / بستانکار ----

        Private Sub dgvEntryLines_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvEntryLines.EditingControlShowing
            Dim tb = TryCast(e.Control, TextBox)
            If tb Is Nothing Then Return

            ' حذف همه handler های سفارشی قبلی — TextBox بین سلول‌ها مشترک است
            RemoveHandler tb.KeyPress, AddressOf DebitCreditKeyPress
            RemoveHandler tb.KeyPress, AddressOf PersianDateKeyPress
            RemoveHandler tb.TextChanged, AddressOf DgvPersianDateTextChanged
            RemoveHandler tb.TextChanged, AddressOf EditingAmountChanged
            tb.MaxLength = 0

            Dim colName = If(dgvEntryLines.CurrentCell IsNot Nothing, dgvEntryLines.CurrentCell.OwningColumn.Name, Nothing)
            If colName = "colDebit" OrElse colName = "colCredit" Then
                AddHandler tb.KeyPress, AddressOf DebitCreditKeyPress
                AddHandler tb.TextChanged, AddressOf EditingAmountChanged
                tb.MaxLength = 22
                Dim raw = tb.Text.Replace(",", "").Trim()
                ' در صورت ورود به سلول، "0" پیش‌فرض را پاک کن تا کاربر مستقیم تایپ کند
                tb.Text = If(raw = "0" OrElse raw = "", "", raw)
                tb.SelectAll()
            ElseIf colName = "colTransDate" Then
                AddHandler tb.KeyPress, AddressOf PersianDateKeyPress
                AddHandler tb.TextChanged, AddressOf DgvPersianDateTextChanged
                tb.MaxLength = 10
            End If
        End Sub

        Private Sub PersianDateKeyPress(sender As Object, e As KeyPressEventArgs)
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub DgvPersianDateTextChanged(sender As Object, e As EventArgs)
            If _suppressDgvDateChange Then Return
            Dim tb = TryCast(sender, TextBox)
            If tb Is Nothing Then Return
            Dim digits = New String(tb.Text.Where(Function(c) Char.IsDigit(c)).ToArray())
            If digits.Length > 8 Then digits = digits.Substring(0, 8)
            Dim formatted = FormatPersianDigits(digits)
            If formatted = tb.Text Then Return
            _suppressDgvDateChange = True
            tb.Text = formatted
            tb.SelectionStart = formatted.Length
            _suppressDgvDateChange = False
        End Sub

        Private Sub DebitCreditKeyPress(sender As Object, e As KeyPressEventArgs)
            ' فقط ارقام و کلیدهای کنترلی (Backspace و غیره) مجاز هستند
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub dgvEntryLines_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvEntryLines.CellFormatting
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
            If colName = "colDebit" OrElse colName = "colCredit" Then
                If e.Value IsNot Nothing Then
                    Dim val As Decimal
                    If Decimal.TryParse(e.Value.ToString(), val) Then
                        e.Value = val.ToString("N0")
                        e.FormattingApplied = True
                    End If
                End If
            End If
        End Sub

        Private Sub dgvEntryLines_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles dgvEntryLines.CellParsing
            If e.ColumnIndex < 0 Then Return
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
            If colName = "colDebit" OrElse colName = "colCredit" Then
                Dim str = If(e.Value IsNot Nothing, e.Value.ToString().Replace(",", "").Trim(), "")
                Dim val As Decimal = 0D
                If Not String.IsNullOrEmpty(str) Then Decimal.TryParse(str, val)
                e.Value = val
                e.ParsingApplied = True
            End If
        End Sub

        Private Sub dgvEntryLines_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntryLines.CellEndEdit
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim colName = dgvEntryLines.Columns(e.ColumnIndex).Name
            Dim rowIdx = e.RowIndex
            If colName = "colDebit" OrElse colName = "colCredit" Then
                Me.BeginInvoke(New Action(Sub() EnforceDebitCreditExclusion(rowIdx, colName)))
            End If
        End Sub

        Private Sub EnforceDebitCreditExclusion(rowIndex As Integer, editedCol As String)
            If rowIndex < 0 OrElse rowIndex >= dgvEntryLines.Rows.Count Then Return
            Dim debitCell = dgvEntryLines.Rows(rowIndex).Cells("colDebit")
            Dim creditCell = dgvEntryLines.Rows(rowIndex).Cells("colCredit")
            Dim debit As Decimal = 0D
            Dim credit As Decimal = 0D
            Decimal.TryParse(Convert.ToString(debitCell.Value), debit)
            Decimal.TryParse(Convert.ToString(creditCell.Value), credit)
            If editedCol = "colDebit" AndAlso debit > 0 AndAlso credit > 0 Then
                creditCell.Value = 0D
            ElseIf editedCol = "colCredit" AndAlso credit > 0 AndAlso debit > 0 Then
                debitCell.Value = 0D
            End If
            UpdateJamLabels()
        End Sub

        ' ---- محاسبه و نمایش جمع بدهکار/بستانکار و کسری تا تراز ----

        Friend Sub UpdateJamLabels()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            Dim jamD As Decimal = 0D
            Dim jamC As Decimal = 0D
            If table IsNot Nothing Then
                For Each dr As DataRow In table.Rows
                    Dim d As Decimal = 0D
                    Dim c As Decimal = 0D
                    Decimal.TryParse(Convert.ToString(dr("DebitAmount")), d)
                    Decimal.TryParse(Convert.ToString(dr("CreditAmount")), c)
                    jamD += d
                    jamC += c
                Next
            End If
            ApplyJamDisplay(jamD, jamC)
        End Sub

        ' نمایش مقادیر جمع — قابل فراخوانی هم از UpdateJamLabels (کامیت شده)
        ' و هم از EditingAmountChanged (حین تایپ با مقدار موقت)
        Private Sub ApplyJamDisplay(jamD As Decimal, jamC As Decimal)
            txtJamBedehkar.Text = jamD.ToString("N0")
            txtJamBestankar.Text = jamC.ToString("N0")
            Dim diff = jamD - jamC
            If diff = 0 Then
                lblSanadStatus.Text = "تراز"
                lblSanadStatus.ForeColor = Color.DarkGreen
                txtKasriDebit.Text = "0"
                txtKasriCredit.Text = "0"
            ElseIf diff > 0 Then
                lblSanadStatus.Text = "بدهکار"
                lblSanadStatus.ForeColor = Color.DarkRed
                txtKasriDebit.Text = "0"
                txtKasriCredit.Text = diff.ToString("N0")
            Else
                lblSanadStatus.Text = "بستانکار"
                lblSanadStatus.ForeColor = Color.DarkBlue
                txtKasriDebit.Text = (-diff).ToString("N0")
                txtKasriCredit.Text = "0"
            End If
        End Sub

        ' به‌روزرسانی لحظه‌ای حین تایپ در سلول بدهکار/بستانکار
        Private Sub EditingAmountChanged(sender As Object, e As EventArgs)
            Dim tb = TryCast(sender, TextBox)
            If tb Is Nothing Then Return
            Dim cell = dgvEntryLines.CurrentCell
            If cell Is Nothing Then Return
            Dim colName = cell.OwningColumn.Name
            If colName <> "colDebit" AndAlso colName <> "colCredit" Then Return

            Dim editingVal As Decimal = 0D
            Decimal.TryParse(tb.Text.Replace(",", "").Trim(), editingVal)
            Dim rowIndex = cell.RowIndex

            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            Dim jamD As Decimal = 0D
            Dim jamC As Decimal = 0D
            If table IsNot Nothing Then
                For i As Integer = 0 To table.Rows.Count - 1
                    Dim d As Decimal = 0D
                    Dim c As Decimal = 0D
                    Decimal.TryParse(Convert.ToString(table.Rows(i)("DebitAmount")), d)
                    Decimal.TryParse(Convert.ToString(table.Rows(i)("CreditAmount")), c)
                    If i = rowIndex Then
                        If colName = "colDebit" Then d = editingVal Else c = editingVal
                    End If
                    jamD += d
                    jamC += c
                Next
            End If
            ApplyJamDisplay(jamD, jamC)
        End Sub

        ' تراز کردن TextBoxهای جمع دقیقاً زیر ستون‌های بدهکار و بستانکار گرید
        Private Sub AlignJamBoxes()
            Dim AlignPair = Sub(tbTop As TextBox, tbBot As TextBox, colName As String)
                                Dim col = dgvEntryLines.Columns(colName)
                                If col Is Nothing OrElse Not col.Visible Then
                                    tbTop.Visible = False : tbBot.Visible = False : Return
                                End If
                                Dim r = dgvEntryLines.GetColumnDisplayRectangle(col.Index, True)
                                If r.IsEmpty OrElse r.Width = 0 Then
                                    tbTop.Visible = False : tbBot.Visible = False : Return
                                End If
                                Dim screenPt = dgvEntryLines.PointToScreen(New Point(r.X, 0))
                                Dim panelPt = pnlJamSanad.PointToClient(screenPt)
                                tbTop.Location = New Point(panelPt.X, 6)
                                tbTop.Width = r.Width
                                tbTop.Visible = True
                                tbBot.Location = New Point(panelPt.X, 40)
                                tbBot.Width = r.Width
                                tbBot.Visible = True
                            End Sub

            AlignPair(txtJamBedehkar, txtKasriDebit, "colDebit")
            AlignPair(txtJamBestankar, txtKasriCredit, "colCredit")
        End Sub

        ' ---------------------------------------------------------------

        Private Sub BtnAddLine_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
            dgvEntryLines.EndEdit()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return
            Dim insertAt As Integer = If(dgvEntryLines.CurrentRow IsNot Nothing, dgvEntryLines.CurrentRow.Index + 1, table.Rows.Count)
            Dim newRow = table.NewRow()
            newRow("LineNumber") = 0
            newRow("AccountID") = 0
            newRow("AccountCode") = ""
            newRow("AccountName") = ""
            newRow("ShenavarID") = 0
            newRow("ShenavarCode") = ""
            newRow("SharhRadif") = ""
            newRow("DebitAmount") = 0D
            newRow("CreditAmount") = 0D
            newRow("TransactionNumber") = ""
            newRow("TransactionDate") = ""
            table.Rows.InsertAt(newRow, Math.Min(insertAt, table.Rows.Count))
            RenumberRows(table)
            UpdateJamLabels()
            Dim colIdx = If(dgvEntryLines.CurrentCell IsNot Nothing, dgvEntryLines.CurrentCell.ColumnIndex, 0)
            If insertAt < dgvEntryLines.Rows.Count Then
                dgvEntryLines.CurrentCell = dgvEntryLines.Rows(insertAt).Cells(colIdx)
            End If
        End Sub

        Private Sub BtnDeleteRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
            If dgvEntryLines.CurrentRow Is Nothing Then Return
            Dim rowIndex = dgvEntryLines.CurrentRow.Index
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing OrElse rowIndex < 0 OrElse rowIndex >= table.Rows.Count Then Return

            If MessageBox.Show("سطر جاری حذف شود؟", "تایید حذف",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

            dgvEntryLines.EndEdit()
            table.Rows.RemoveAt(rowIndex)
            RenumberRows(table)
            UpdateJamLabels()
        End Sub

        Private Sub BtnCopyBelow_Click(sender As Object, e As EventArgs) Handles btnCopyBelow.Click
            CopyCurrentRow(insertOffset:=1, askPosition:=False)
        End Sub

        Private Sub BtnCopyAbove_Click(sender As Object, e As EventArgs) Handles btnCopyAbove.Click
            CopyCurrentRow(insertOffset:=0, askPosition:=False)
        End Sub

        Private Sub BtnCopyToPos_Click(sender As Object, e As EventArgs) Handles btnCopyToPos.Click
            CopyCurrentRow(insertOffset:=0, askPosition:=True)
        End Sub

        Private Sub CopyCurrentRow(insertOffset As Integer, askPosition As Boolean)
            If dgvEntryLines.CurrentRow Is Nothing Then Return
            dgvEntryLines.EndEdit()
            Dim rowIndex = dgvEntryLines.CurrentRow.Index
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing OrElse rowIndex < 0 OrElse rowIndex >= table.Rows.Count Then Return

            Dim insertAt As Integer
            If askPosition Then
                Dim input = Microsoft.VisualBasic.Interaction.InputBox(
                    "شماره سطر مقصد را وارد کنید (1 تا " & table.Rows.Count & "):",
                    "کپی سطر در محل دلخواه", (rowIndex + 1).ToString())
                If String.IsNullOrWhiteSpace(input) Then Return
                Dim pos As Integer
                If Not Integer.TryParse(input.Trim(), pos) OrElse pos < 1 OrElse pos > table.Rows.Count + 1 Then
                    MessageBox.Show("شماره سطر وارد شده معتبر نیست.", "توجه",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                insertAt = pos - 1
            Else
                insertAt = rowIndex + insertOffset
            End If

            ' کپی داده‌های ردیف جاری در ردیف جدید
            Dim src = table.Rows(rowIndex)
            Dim newRow = table.NewRow()
            For Each col As DataColumn In table.Columns
                newRow(col) = src(col)
            Next
            table.Rows.InsertAt(newRow, Math.Min(insertAt, table.Rows.Count))
            RenumberRows(table)
            UpdateJamLabels()

            ' انتخاب ردیف کپی‌شده
            Dim targetRow = Math.Min(insertAt, dgvEntryLines.Rows.Count - 1)
            Dim colIdx = If(dgvEntryLines.CurrentCell IsNot Nothing, dgvEntryLines.CurrentCell.ColumnIndex, 0)
            dgvEntryLines.ClearSelection()
            dgvEntryLines.CurrentCell = dgvEntryLines.Rows(targetRow).Cells(colIdx)
        End Sub

        Private Sub RenumberRows(table As DataTable)
            For i As Integer = 0 To table.Rows.Count - 1
                table.Rows(i)("LineNumber") = i + 1
            Next
        End Sub

        Private Sub BtnClearSearch_Click(sender As Object, e As EventArgs) Handles btnClearSearch.Click
            txtSrcLineNo.Clear()
            txtSrcSF.Clear()
            txtSrcSH.Clear()
            txtSrcSharh.Clear()
            txtSrcDebit.Clear()
            txtSrcCredit.Clear()
            txtSrcTransNo.Clear()
            txtSrcTransDate.Clear()
            ' ApplyRowFilter از طریق TextChanged هر TextBox فراخوانی می‌شود
        End Sub

        Private Sub TxtSrcAny_TextChanged(sender As Object, e As EventArgs) _
            Handles txtSrcLineNo.TextChanged, txtSrcSF.TextChanged, txtSrcSH.TextChanged,
                    txtSrcSharh.TextChanged, txtSrcDebit.TextChanged, txtSrcCredit.TextChanged,
                    txtSrcTransNo.TextChanged, txtSrcTransDate.TextChanged
            ApplyRowFilter()
        End Sub

        Private Sub ApplyRowFilter()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            Dim sfText = txtSrcSF.Text.Trim().ToLower()
            Dim shText = txtSrcSH.Text.Trim().ToLower()
            Dim lineText = txtSrcLineNo.Text.Trim()
            Dim sharhText = txtSrcSharh.Text.Trim().ToLower()
            Dim debitText = txtSrcDebit.Text.Trim()
            Dim creditText = txtSrcCredit.Text.Trim()
            Dim transNoText = txtSrcTransNo.Text.Trim().ToLower()
            Dim transDateText = txtSrcTransDate.Text.Trim().ToLower()

            Dim anyFilter = sfText.Length > 0 OrElse shText.Length > 0 OrElse
                            lineText.Length > 0 OrElse sharhText.Length > 0 OrElse
                            debitText.Length > 0 OrElse creditText.Length > 0 OrElse
                            transNoText.Length > 0 OrElse transDateText.Length > 0

            dgvEntryLines.EndEdit()

            If Not anyFilter Then
                For i As Integer = 0 To dgvEntryLines.Rows.Count - 1
                    If Not dgvEntryLines.Rows(i).Visible Then dgvEntryLines.Rows(i).Visible = True
                Next
                Return
            End If

            ' محاسبه ارائه‌پذیری هر سطر بدون در نظر گرفتن سطر جاری
            Dim rowCount = dgvEntryLines.Rows.Count
            Dim visibility(rowCount - 1) As Boolean

            For i As Integer = 0 To rowCount - 1
                If i >= table.Rows.Count Then
                    visibility(i) = False
                    Continue For
                End If
                Dim dataRow = table.Rows(i)

                Dim accountId = Convert.ToInt32(If(dataRow("AccountID") Is DBNull.Value, 0, dataRow("AccountID")))
                Dim shenavarId = Convert.ToInt32(If(dataRow("ShenavarID") Is DBNull.Value, 0, dataRow("ShenavarID")))
                Dim sharhVal = Convert.ToString(If(dataRow("SharhRadif") Is DBNull.Value, "", dataRow("SharhRadif")))
                Dim debitVal = Convert.ToDecimal(If(dataRow("DebitAmount") Is DBNull.Value, 0D, dataRow("DebitAmount")))
                Dim creditVal = Convert.ToDecimal(If(dataRow("CreditAmount") Is DBNull.Value, 0D, dataRow("CreditAmount")))
                Dim transNoVal = Convert.ToString(If(dataRow("TransactionNumber") Is DBNull.Value, "", dataRow("TransactionNumber")))

                Dim isEmpty = accountId = 0 AndAlso shenavarId = 0 AndAlso
                              sharhVal.Length = 0 AndAlso debitVal = 0D AndAlso
                              creditVal = 0D AndAlso transNoVal.Length = 0

                If isEmpty Then
                    visibility(i) = False
                    Continue For
                End If

                Dim visible = True

                If lineText.Length > 0 Then
                    If Not Convert.ToString(dataRow("LineNumber")).Contains(lineText) Then visible = False
                End If

                If visible AndAlso sfText.Length > 0 Then
                    Dim chain = If(accountId > 0, BuildAccountChain(accountId).ToLower(), "")
                    If Not chain.Contains(sfText) Then visible = False
                End If

                If visible AndAlso shText.Length > 0 Then
                    Dim chain = If(shenavarId > 0, BuildShenavarChain(shenavarId).ToLower(), "")
                    If Not chain.Contains(shText) Then visible = False
                End If

                If visible AndAlso sharhText.Length > 0 Then
                    If Not sharhVal.ToLower().Contains(sharhText) Then visible = False
                End If

                If visible AndAlso debitText.Length > 0 Then
                    If debitVal = 0D OrElse Not MatchAmount(debitVal, debitText) Then visible = False
                End If

                If visible AndAlso creditText.Length > 0 Then
                    If creditVal = 0D OrElse Not MatchAmount(creditVal, creditText) Then visible = False
                End If

                If visible AndAlso transNoText.Length > 0 Then
                    If Not transNoVal.ToLower().Contains(transNoText) Then visible = False
                End If

                If visible AndAlso transDateText.Length > 0 Then
                    Dim transDateVal = Convert.ToString(If(dataRow("TransactionDate") Is DBNull.Value, "", dataRow("TransactionDate"))).ToLower()
                    If Not transDateVal.Contains(transDateText) Then visible = False
                End If

                visibility(i) = visible
            Next

            ' اگر سطر جاری پنهان می‌شود، cursor را به اولین سطر قابل نمایش منتقل کن
            Dim currentRowIdx = If(dgvEntryLines.CurrentRow IsNot Nothing, dgvEntryLines.CurrentRow.Index, -1)
            If currentRowIdx >= 0 AndAlso currentRowIdx < rowCount AndAlso Not visibility(currentRowIdx) Then
                Dim moved = False
                For i As Integer = 0 To rowCount - 1
                    If visibility(i) Then
                        ' ابتدا ردیف مقصد را visible کن تا CurrentCell روی ردیف invisible تنظیم نشود
                        If Not dgvEntryLines.Rows(i).Visible Then dgvEntryLines.Rows(i).Visible = True
                        Dim colIdx = If(dgvEntryLines.CurrentCell IsNot Nothing, dgvEntryLines.CurrentCell.ColumnIndex, 0)
                        dgvEntryLines.CurrentCell = dgvEntryLines.Rows(i).Cells(colIdx)
                        moved = True
                        Exit For
                    End If
                Next
                If Not moved Then
                    ' هیچ ردیفی با فیلتر مطابقت ندارد — انتخاب را پاک کن تا همه ردیف‌ها بتوانند پنهان شوند
                    Try
                        dgvEntryLines.CurrentCell = Nothing
                    Catch
                        visibility(currentRowIdx) = True
                    End Try
                End If
            End If

            ' اعمال ارائه‌پذیری
            For i As Integer = 0 To rowCount - 1
                If dgvEntryLines.Rows(i).Visible <> visibility(i) Then
                    dgvEntryLines.Rows(i).Visible = visibility(i)
                End If
            Next
        End Sub

        ' مقایسه مقدار عددی با متن فیلتر ستون بدهکار/بستانکار
        ' *عدد  → جستجوی رشته‌ای (Contains در رشته عددی)
        ' <عدد  → کوچکتر از عدد وارد شده
        ' >عدد  → بزرگتر از عدد وارد شده
        ' عدد   → برابر دقیق با عدد وارد شده
        Private Function MatchAmount(val As Decimal, filterText As String) As Boolean
            If filterText.Length = 0 Then Return True
            Dim ic = Globalization.CultureInfo.InvariantCulture
            Dim ch = filterText(0)
            If ch = "*"c Then
                Dim searchStr = filterText.Substring(1)
                If searchStr.Length = 0 Then Return True
                ' هر دو طرف بصورت رشته صحیح (بدون اعشار) مقایسه می‌شوند
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
                ' مقایسه دقیق عددی
                Dim target As Decimal
                If Decimal.TryParse(filterText, Globalization.NumberStyles.Integer, ic, target) Then
                    Return val = target
                End If
                Return True
            End If
        End Function

        ' محدود کردن ورودی تکست‌باکس‌های جستجوی مبلغ
        ' مجاز: ارقام 0-9، و در ابتدای متن: < > *
        Private Sub TxtSrcAmount_KeyPress(sender As Object, e As KeyPressEventArgs) _
            Handles txtSrcDebit.KeyPress, txtSrcCredit.KeyPress
            If Char.IsControl(e.KeyChar) Then Return
            If Char.IsDigit(e.KeyChar) Then Return
            If e.KeyChar = "<"c OrElse e.KeyChar = ">"c OrElse e.KeyChar = "*"c Then
                Dim tb = TryCast(sender, TextBox)
                ' فقط اگر کرسر در ابتدای متن باشد و هیچ علامت خاصی قبلاً وارد نشده باشد
                If tb IsNot Nothing AndAlso tb.SelectionStart = 0 Then
                    Dim firstIsSpecial = tb.Text.Length > 0 AndAlso
                                        (tb.Text(0) = "<"c OrElse tb.Text(0) = ">"c OrElse tb.Text(0) = "*"c)
                    If Not firstIsSpecial OrElse tb.SelectionLength > 0 Then Return
                End If
            End If
            e.Handled = True
        End Sub

        Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub HesabdarySanad2Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            If Not CheckExitConfirmation() Then
                e.Cancel = True
            End If
        End Sub

        Private Function CheckExitConfirmation() As Boolean
            If _suppressCloseConfirmation Then Return True

            dgvEntryLines.EndEdit()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)

            ' ۱. بررسی سطور نیمه‌کاره (انتخاب کد سرفصل بدون وارد کردن مبلغ بدهکار و بستانکار)
            If table IsNot Nothing Then
                For Each row As DataRow In table.Rows
                    If row.RowState = DataRowState.Deleted Then Continue For
                    Dim accountId = Convert.ToInt32(If(row("AccountID") Is DBNull.Value, 0, row("AccountID")))
                    Dim debit = Convert.ToDecimal(If(row("DebitAmount") Is DBNull.Value, 0D, row("DebitAmount")))
                    Dim credit = Convert.ToDecimal(If(row("CreditAmount") Is DBNull.Value, 0D, row("CreditAmount")))

                    If accountId > 0 AndAlso debit = 0D AndAlso credit = 0D Then
                        Dim msgIncomplete = "سند جاری نیمه کاره است و ذخیره نشده است" & Environment.NewLine & "آیا از خروج و بسته شدن سند اطمینان دارید ؟"
                        Dim res = MessageBox.Show(msgIncomplete, "تایید خروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        Return res = DialogResult.Yes
                    End If
                Next
            End If

            ' ۲. بررسی تغییرات در سند در حالت ویرایش یا تغییر در سطور سند
            If _isDirty Then
                Dim msgModified = "تغییراتی در این سند داده شده است" & Environment.NewLine & "آیا از خروج و بسته شدن سند اطمینان دارید ؟"
                Dim res = MessageBox.Show(msgModified, "تایید خروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                Return res = DialogResult.Yes
            End If

            Return True
        End Function

        Private Sub btnPrintVoucher_Click(sender As Object, e As EventArgs) Handles btnPrintVoucher.Click
            dgvEntryLines.EndEdit()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return

            ' بررسی اینکه حداقل یک سطر با آیدی حساب انتخاب شده باشد
            Dim hasData = table.AsEnumerable().Any(Function(r)
                Dim accId = 0
                If r.Table.Columns.Contains("AccountID") AndAlso Not r.IsNull("AccountID") Then
                    accId = Convert.ToInt32(r("AccountID"))
                End If
                Return accId > 0
            End Function)

            If Not hasData Then
                MessageBox.Show("سند جاری فاقد هرگونه آرتیکل برای چاپ می‌باشد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using printForm As New HesabdaryPrintForm(txtEntryReference.Text.Trim(), txtDateSanad.Text.Trim(), txtEntryDescription.Text.Trim(), table)
                printForm.ShowDialog(Me)
            End Using
        End Sub

        ' ---- F3: نمایش اطلاعات ناوبری اسناد ----

        Private Sub HesabdarySanad2Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.F3 Then
                ShowEntryNavigationInfo()
                e.Handled = True
            ElseIf e.Control AndAlso e.KeyCode = Keys.P Then
                btnPrintVoucher.PerformClick()
                e.Handled = True
            End If
        End Sub

        Private Sub ShowEntryNavigationInfo()
            Dim currentRef = txtEntryReference.Text.Trim()
            If String.IsNullOrEmpty(currentRef) Then currentRef = "0"

            Dim navData = service.GetEntryNavigationInfo(currentRef)

            ' اگر هیچ سندی در سیستم ثبت نشده باشد
            If navData.Rows.Count > 0 AndAlso Convert.ToString(navData.Rows(0)("RefNumber")) = "-" Then
                MessageBox.Show("تاکنون هیچ سندی در این سال مالی ثبت نشده است.",
                                "اطلاعات اسناد", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim dlg As New Form()
            dlg.Text = "اطلاعات اسناد"
            dlg.RightToLeft = RightToLeft.Yes
            dlg.RightToLeftLayout = True
            dlg.Width = 520
            dlg.Height = 200
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.MaximizeBox = False
            dlg.MinimizeBox = False
            dlg.KeyPreview = True

            Dim headerFont As New Font("Tahoma", 9, FontStyle.Bold)
            Dim cellFont As New Font("Tahoma", 9)

            Dim dgv As New DataGridView()
            dgv.Dock = DockStyle.Fill
            dgv.ReadOnly = True
            dgv.AllowUserToAddRows = False
            dgv.AllowUserToDeleteRows = False
            dgv.ColumnHeadersDefaultCellStyle.Font = headerFont
            dgv.DefaultCellStyle.Font = cellFont
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.RowHeadersVisible = False
            dgv.MultiSelect = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill
            dgv.ScrollBars = ScrollBars.None
            dgv.BackgroundColor = SystemColors.Window

            Dim colRowNum As New DataGridViewTextBoxColumn()
            colRowNum.HeaderText = "ردیف"
            colRowNum.Width = 48
            colRowNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.None

            Dim colTitle As New DataGridViewTextBoxColumn()
            colTitle.HeaderText = "عنوان سند"

            Dim colRef As New DataGridViewTextBoxColumn()
            colRef.HeaderText = "شماره سند"

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.HeaderText = "تاریخ سند"

            dgv.Columns.AddRange(New DataGridViewColumn() {colRowNum, colTitle, colRef, colDate})

            For i As Integer = 0 To navData.Rows.Count - 1
                Dim r = navData.Rows(i)
                dgv.Rows.Add((i + 1).ToString(),
                             Convert.ToString(r("Title")),
                             Convert.ToString(r("RefNumber")),
                             Convert.ToString(r("DateStr")))
            Next

            Dim btnClose As New Button()
            btnClose.Text = "بستن"
            btnClose.Dock = DockStyle.Bottom
            btnClose.Height = 34
            btnClose.Font = cellFont
            AddHandler btnClose.Click, Sub(s, ev) dlg.Close()
            AddHandler dlg.KeyDown, Sub(s, ev)
                                        If ev.KeyCode = Keys.Escape OrElse ev.KeyCode = Keys.F3 Then dlg.Close()
                                    End Sub

            dlg.Controls.Add(dgv)
            dlg.Controls.Add(btnClose)
            dlg.ShowDialog(Me)
            dlg.Dispose()
            headerFont.Dispose()
            cellFont.Dispose()
        End Sub

        Private Shared Function TryParsePersianDate(dateStr As String, ByRef result As Date) As Boolean
            result = Date.MinValue
            If String.IsNullOrWhiteSpace(dateStr) Then Return False
            Dim parts = dateStr.Trim().Split("/"c)
            If parts.Length <> 3 Then Return False
            If parts(0).Length <> 4 OrElse parts(1).Length <> 2 OrElse parts(2).Length <> 2 Then Return False
            Dim y, m, d As Integer
            If Not Integer.TryParse(parts(0), y) OrElse Not Integer.TryParse(parts(1), m) OrElse Not Integer.TryParse(parts(2), d) Then Return False
            Try
                Dim pc As New Globalization.PersianCalendar()
                result = pc.ToDateTime(y, m, d, 0, 0, 0, 0)
                Return True
            Catch
                Return False
            End Try
        End Function

        Private Sub BtnSaveEntry_Click(sender As Object, e As EventArgs) Handles btnSaveEntry.Click
            If TrySaveEntry() Then
                _suppressCloseConfirmation = True
                _isDirty = False
                Me.Close()
            End If
        End Sub

        Private Sub BtnSaveAndContinue_Click(sender As Object, e As EventArgs) Handles btnSaveAndContinue.Click
            If TrySaveEntry() Then
                _isDirty = False
                ResetForNewEntry()
            End If
        End Sub

        Private Sub ResetForNewEntry()
            txtEntryReference.Text = service.GetNextReferenceNumber()
            txtEntryDescription.Text = ""
            txtSrcLineNo.Clear()
            txtSrcSF.Clear()
            txtSrcSH.Clear()
            txtSrcSharh.Clear()
            txtSrcDebit.Clear()
            txtSrcCredit.Clear()
            txtSrcTransNo.Clear()
            txtSrcTransDate.Clear()
            _accountChainCache.Clear()
            _shenavarChainCache.Clear()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table IsNot Nothing Then
                table.Rows.Clear()
                For i As Integer = 1 To TotalPreloadedRows
                    table.Rows.Add(i, 0, "", "", 0, "", "", 0D, 0D, "", "")
                Next
            End If
            UpdateJamLabels()
            txtEntryReference.Focus()
            txtEntryReference.SelectAll()
        End Sub

        Private Function TrySaveEntry() As Boolean
            Dim refText = txtEntryReference.Text.Trim()

            ' ۱. شماره سند خالی نباشد
            If String.IsNullOrWhiteSpace(refText) Then
                MessageBox.Show("شماره سند نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtEntryReference.Focus()
                txtEntryReference.SelectAll()
                Return False
            End If

            ' ۲. شماره سند کوچکتر از آخرین سند نباشد (فقط در حالت ثبت سند جدید)
            If Not _editEntryId.HasValue Then
                Dim nextRefStr = service.GetNextReferenceNumber()
                Dim nextRefNum As Long = 0
                Dim enteredRefNum As Long = 0
                Long.TryParse(nextRefStr, nextRefNum)
                Long.TryParse(refText, enteredRefNum)
                Dim maxExistingRef = nextRefNum - 1
                If maxExistingRef > 0 AndAlso enteredRefNum < maxExistingRef Then
                    MessageBox.Show("شماره وارد شده برای این سند کوچکتر از شماره سند قبل می‌باشد ، لطفا شماره سند را اصلاح نمایید",
                                    "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtEntryReference.Focus()
                    txtEntryReference.SelectAll()
                    Return False
                End If
            End If

            ' ۳. تاریخ سند خالی نباشد و فرمت آن صحیح باشد
            Dim dateText = txtDateSanad.Text.Trim()
            If String.IsNullOrWhiteSpace(dateText) Then
                MessageBox.Show("تاریخ سند نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDateSanad.Focus()
                txtDateSanad.SelectAll()
                Return False
            End If

            Dim entryDate As Date = Date.MinValue
            If Not TryParsePersianDate(dateText, entryDate) Then
                MessageBox.Show("فرمت تاریخ سند صحیح نمی‌باشد." & Environment.NewLine &
                                "فرمت مورد قبول: yyyy/mm/dd", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDateSanad.Focus()
                txtDateSanad.SelectAll()
                Return False
            End If

            ' ۴. تاریخ سند کوچکتر از تاریخ آخرین سند نباشد (فقط در حالت ثبت سند جدید)
            If Not _editEntryId.HasValue Then
                Dim lastNav = service.GetEntryNavigationInfo("999999999")
                If lastNav.Rows.Count > 0 Then
                    Dim lastRefStr = Convert.ToString(lastNav.Rows(0)("RefNumber"))
                    Dim lastDateStr = Convert.ToString(lastNav.Rows(0)("DateStr"))
                    If lastRefStr <> "-" AndAlso lastDateStr.Length > 0 AndAlso lastDateStr <> "-" Then
                        If String.CompareOrdinal(dateText, lastDateStr) < 0 Then
                            MessageBox.Show("سند قبلی به شماره سند " & lastRefStr & " به تاریخ " & lastDateStr &
                                            " می‌باشد و تاریخ وارد شده برای این سند از تاریخ سند قبل کمتر می‌باشد ، لطفا تاریخ این سند را اصلاح نمایید",
                                            "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtDateSanad.Focus()
                            txtDateSanad.SelectAll()
                            Return False
                        End If
                    End If
                End If
            End If

            ' ۵. شماره سند تکراری نباشد
            If service.IsReferenceNumberDuplicate(refText, _editEntryId) Then
                MessageBox.Show("شماره سند « " & refText & " » قبلاً ثبت شده است." & Environment.NewLine &
                                "لطفاً شماره دیگری انتخاب کنید.",
                                "شماره سند تکراری", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtEntryReference.Focus()
                txtEntryReference.SelectAll()
                Return False
            End If

            ' ۶. ردیف‌هایی که کد سرفصل دارند باید مبلغ بدهکار یا بستانکار داشته باشند
            dgvEntryLines.EndEdit()
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return False



            Dim anyAccountSelected = table.Rows.Cast(Of DataRow)().Any(
                Function(r) Convert.ToInt32(If(r("AccountID") Is DBNull.Value, 0, r("AccountID"))) > 0)
            If Not anyAccountSelected Then
                MessageBox.Show("شما هیچ آرتیکلی برای این سند وارد نکرده‌اید ، برای ثبت سند حداقل باید یک آرتیکل وارد نمایید",
                                "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                If dgvEntryLines.Rows.Count > 0 Then
                    dgvEntryLines.CurrentCell = dgvEntryLines.Rows(0).Cells(dgvEntryLines.Columns("colBtnSarfasl").Index)
                End If
                Return False
            End If

            For Each row As DataRow In table.Rows
                Dim accountId = Convert.ToInt32(If(row("AccountID") Is DBNull.Value, 0, row("AccountID")))
                Dim debit = Convert.ToDecimal(If(row("DebitAmount") Is DBNull.Value, 0D, row("DebitAmount")))
                Dim credit = Convert.ToDecimal(If(row("CreditAmount") Is DBNull.Value, 0D, row("CreditAmount")))
                Dim lineNo = Convert.ToInt32(If(row("LineNumber") Is DBNull.Value, 0, row("LineNumber")))
                If accountId > 0 AndAlso debit = 0D AndAlso credit = 0D Then
                    MessageBox.Show("شما در ردیف " & lineNo & " کد سرفصل انتخاب کرده‌اید ولی هیچ مقداری برای ستون بدهکار و یا بستانکار آن ردیف وارد نکرده‌اید",
                                    "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Dim rowIdx = table.Rows.IndexOf(row)
                    If rowIdx >= 0 AndAlso rowIdx < dgvEntryLines.Rows.Count Then
                        If Not dgvEntryLines.Rows(rowIdx).Visible Then dgvEntryLines.Rows(rowIdx).Visible = True
                        dgvEntryLines.CurrentCell = dgvEntryLines.Rows(rowIdx).Cells(dgvEntryLines.Columns("colDebit").Index)
                        dgvEntryLines.BeginEdit(True)
                    End If
                    Return False
                End If
                If accountId = 0 AndAlso (debit > 0D OrElse credit > 0D) Then
                    MessageBox.Show("شما در ردیف " & lineNo & " مبلغ وارد کرده‌اید ولی کد سرفصل آن را انتخاب نکرده‌اید.",
                                    "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Dim rowIdx = table.Rows.IndexOf(row)
                    If rowIdx >= 0 AndAlso rowIdx < dgvEntryLines.Rows.Count Then
                        If Not dgvEntryLines.Rows(rowIdx).Visible Then dgvEntryLines.Rows(rowIdx).Visible = True
                        dgvEntryLines.CurrentCell = dgvEntryLines.Rows(rowIdx).Cells(dgvEntryLines.Columns("colBtnSarfasl").Index)
                    End If
                    Return False
                End If
            Next

            ' ۷. شرح سند خالی نباشد
            If String.IsNullOrWhiteSpace(txtEntryDescription.Text) Then
                MessageBox.Show("شرح سند نمی‌تواند خالی باشد.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtEntryDescription.Focus()
                Return False
            End If

            ' ۸. جمع‌های کل از کنترل‌های UI
            Dim jamD As Decimal = 0D
            Dim jamC As Decimal = 0D
            Decimal.TryParse(txtJamBedehkar.Text.Replace(",", "").Trim(), jamD)
            Decimal.TryParse(txtJamBestankar.Text.Replace(",", "").Trim(), jamC)
            Dim taeaz = lblSanadStatus.Text

            ' ۹. ذخیره‌سازی
            Try
                Dim lines As New List(Of AccountingEntryLine)()
                For Each row As DataRow In table.Rows
                    Dim debit = Convert.ToDecimal(If(row("DebitAmount") Is DBNull.Value, 0D, row("DebitAmount")))
                    Dim credit = Convert.ToDecimal(If(row("CreditAmount") Is DBNull.Value, 0D, row("CreditAmount")))
                    If debit > 0 OrElse credit > 0 Then
                        lines.Add(New AccountingEntryLine(
                            Convert.ToInt32(If(row("AccountID") Is DBNull.Value, 0, row("AccountID"))),
                            debit, credit,
                            Convert.ToInt32(If(row("LineNumber") Is DBNull.Value, 0, row("LineNumber"))),
                            Convert.ToInt32(If(row("ShenavarID") Is DBNull.Value, 0, row("ShenavarID"))),
                            Convert.ToString(If(row("SharhRadif") Is DBNull.Value, "", row("SharhRadif"))),
                            Convert.ToString(If(row("TransactionNumber") Is DBNull.Value, "", row("TransactionNumber"))),
                            Convert.ToString(If(row("TransactionDate") Is DBNull.Value, "", row("TransactionDate")))))
                    End If
                Next

                If lines.Count = 0 Then
                    MessageBox.Show("حداقل یک ردیف سند با مبلغ بدهکار یا بستانکار وارد کنید.", "توجه",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If

                Dim createdBy = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)

                If _editEntryId.HasValue Then
                    service.UpdateEntry(_editEntryId.Value, entryDate, txtEntryDescription.Text.Trim(),
                                        refText, createdBy, lines, jamD, jamC, taeaz)
                    MessageBox.Show("سند با موفقیت ویرایش شد.", "ویرایش موفق",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    service.SaveEntry(entryDate, txtEntryDescription.Text.Trim(),
                                      refText, createdBy, lines, jamD, jamC, taeaz)
                    MessageBox.Show("سند با موفقیت ثبت شد.", "ثبت موفق",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Return True
            Catch ex As Exception
                MessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        ' ──────────────────────────────────────────────────────────────────
        '  یادداشت سند
        ' ──────────────────────────────────────────────────────────────────

        Private ReadOnly noteService As New NoteService()

        Private Sub InitializeNoteGrids()
            If dgvEntryNotes.Columns.Count > 0 Then Return

            For Each dgv In New DataGridView() {dgvEntryNotes, dgvLineNotes}
                dgv.AutoGenerateColumns = False
                Dim colUser As New DataGridViewTextBoxColumn()
                colUser.Name = "colNoteUser"
                colUser.DataPropertyName = "UserDisplayName"
                colUser.HeaderText = "کاربر"
                colUser.Width = 130
                colUser.ReadOnly = True

                Dim colText As New DataGridViewTextBoxColumn()
                colText.Name = "colNoteText"
                colText.DataPropertyName = "NoteText"
                colText.HeaderText = "یادداشت"
                colText.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                colText.ReadOnly = True
                colText.DefaultCellStyle.WrapMode = DataGridViewTriState.True

                Dim colDate As New DataGridViewTextBoxColumn()
                colDate.Name = "colNoteDate"
                colDate.DataPropertyName = "ModifiedDate"
                colDate.HeaderText = "آخرین ویرایش"
                colDate.Width = 130
                colDate.ReadOnly = True

                Dim colUserId As New DataGridViewTextBoxColumn()
                colUserId.Name = "colNoteUserID"
                colUserId.DataPropertyName = "UserID"
                colUserId.Visible = False

                dgv.Columns.AddRange(New DataGridViewColumn() {colUserId, colUser, colText, colDate})
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
            Next
        End Sub

        Private Sub InitializeNoteLineSelector()
            If dgvNoteLineSelector.Columns.Count > 0 Then Return
            dgvNoteLineSelector.AutoGenerateColumns = False
            Dim col As New DataGridViewTextBoxColumn()
            col.Name = "colNoteSelectLine"
            col.DataPropertyName = "LineNumber"
            col.HeaderText = "ردیف"
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            col.ReadOnly = True
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvNoteLineSelector.Columns.Add(col)
        End Sub

        Private Sub RefreshNoteLineSelector()
            If Not _editEntryId.HasValue Then
                dgvNoteLineSelector.DataSource = Nothing
                Return
            End If
            InitializeNoteLineSelector()
            Try
                Dim dt = noteService.GetLinesWithNotes(_editEntryId.Value)
                _suppressNoteLineSelectorChange = True
                dgvNoteLineSelector.DataSource = dt
                If _zamLineNumber > 0 Then
                    For Each row As DataGridViewRow In dgvNoteLineSelector.Rows
                        Dim ln = Convert.ToInt32(If(row.Cells("colNoteSelectLine").Value Is DBNull.Value, 0, row.Cells("colNoteSelectLine").Value))
                        If ln = _zamLineNumber Then
                            dgvNoteLineSelector.CurrentCell = row.Cells(0)
                            Exit For
                        End If
                    Next
                End If
                _suppressNoteLineSelectorChange = False
            Catch ex As Exception
                _suppressNoteLineSelectorChange = False
            End Try
        End Sub

        Private Sub DgvNoteLineSelector_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNoteLineSelector.SelectionChanged
            If _suppressNoteLineSelectorChange Then Return
            If dgvNoteLineSelector.CurrentRow Is Nothing Then Return
            Dim val = dgvNoteLineSelector.CurrentRow.Cells("colNoteSelectLine").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return
            Dim lineNum = Convert.ToInt32(val)
            If lineNum = _zamLineNumber Then Return
            _zamLineNumber = lineNum
            If _editEntryId.HasValue Then _zamEntryId = _editEntryId.Value
            RefreshLineNoteSection()
        End Sub

        Private Sub RefreshLineNoteSection()
            If Not _editEntryId.HasValue Then Return
            Dim entryId = _editEntryId.Value
            Dim currentUserId = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)

            If _zamLineNumber > 0 Then
                lblLineNoteTitle.Text = "یادداشت ردیف " & _zamLineNumber.ToString()
                lblNoteHeader.Text = "یادداشت سند شماره " & txtEntryReference.Text.Trim() &
                                     "  —  ردیف انتخاب‌شده: " & _zamLineNumber.ToString()
                Try
                    Dim lineNotes = noteService.GetNotes(entryId, _zamLineNumber)
                    dgvLineNotes.DataSource = lineNotes
                    HighlightCurrentUserRow(dgvLineNotes, currentUserId)
                    txtLineNote.Text = noteService.GetCurrentUserNote(entryId, _zamLineNumber, currentUserId)
                    txtLineNote.Enabled = True
                    btnSaveLineNote.Enabled = True
                    UpdateLineNoteInfo(lineNotes, currentUserId)
                Catch ex As Exception
                    lblLineNoteInfo.Text = "خطا: " & ex.Message
                End Try
            Else
                lblLineNoteTitle.Text = "یادداشت ردیف (ردیفی انتخاب نشده)"
                lblNoteHeader.Text = "یادداشت سند شماره " & txtEntryReference.Text.Trim()
                dgvLineNotes.DataSource = Nothing
                txtLineNote.Text = ""
                txtLineNote.Enabled = False
                btnSaveLineNote.Enabled = False
                lblLineNoteInfo.Text = "ابتدا یک ردیف از لیست انتخاب کنید"
            End If
        End Sub

        Private Sub RefreshNoteTab()
            InitializeNoteGrids()
            InitializeNoteLineSelector()

            Dim currentUserId = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)
            Dim isReady = _editEntryId.HasValue

            If Not isReady Then
                lblNoteHeader.Text = "یادداشت فقط پس از ذخیره سند قابل ثبت است"
                txtEntryNote.Enabled = False
                txtLineNote.Enabled = False
                btnSaveEntryNote.Enabled = False
                btnSaveLineNote.Enabled = False
                dgvNoteLineSelector.DataSource = Nothing
                Return
            End If

            Dim entryId = _editEntryId.Value
            RefreshNoteLineSelector()

            ' یادداشت کلی سند
            lblEntryNoteTitle.Text = "یادداشت کلی سند شماره " & txtEntryReference.Text.Trim()
            Try
                Dim entryNotes = noteService.GetNotes(entryId, Nothing)
                dgvEntryNotes.DataSource = entryNotes
                HighlightCurrentUserRow(dgvEntryNotes, currentUserId)
                txtEntryNote.Text = noteService.GetCurrentUserNote(entryId, Nothing, currentUserId)
                txtEntryNote.Enabled = True
                btnSaveEntryNote.Enabled = True
                UpdateEntryNoteInfo(entryNotes, currentUserId)
            Catch ex As Exception
                lblEntryNoteInfo.Text = "خطا: " & ex.Message
            End Try

            RefreshLineNoteSection()
        End Sub

        Private Sub HighlightCurrentUserRow(dgv As DataGridView, currentUserId As Integer)
            For Each row As DataGridViewRow In dgv.Rows
                Dim uid = Convert.ToInt32(If(row.Cells("colNoteUserID").Value Is DBNull.Value, 0,
                                             row.Cells("colNoteUserID").Value))
                If uid = currentUserId Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220)
                    row.DefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)
                Else
                    row.DefaultCellStyle.BackColor = Color.WhiteSmoke
                    row.DefaultCellStyle.Font = dgv.Font
                End If
            Next
        End Sub

        Private Sub UpdateEntryNoteInfo(dt As DataTable, currentUserId As Integer)
            For Each row As DataRow In dt.Rows
                Dim uid = Convert.ToInt32(If(row("UserID") Is DBNull.Value, 0, row("UserID")))
                If uid = currentUserId Then
                    Dim modDate = If(row("ModifiedDate") Is DBNull.Value, "",
                                     Convert.ToDateTime(row("ModifiedDate")).ToString("yyyy/MM/dd HH:mm"))
                    lblEntryNoteInfo.Text = "آخرین ویرایش: " & modDate
                    Return
                End If
            Next
            lblEntryNoteInfo.Text = "یادداشتی ثبت نشده"
        End Sub

        Private Sub UpdateLineNoteInfo(dt As DataTable, currentUserId As Integer)
            For Each row As DataRow In dt.Rows
                Dim uid = Convert.ToInt32(If(row("UserID") Is DBNull.Value, 0, row("UserID")))
                If uid = currentUserId Then
                    Dim modDate = If(row("ModifiedDate") Is DBNull.Value, "",
                                     Convert.ToDateTime(row("ModifiedDate")).ToString("yyyy/MM/dd HH:mm"))
                    lblLineNoteInfo.Text = "آخرین ویرایش: " & modDate
                    Return
                End If
            Next
            lblLineNoteInfo.Text = "یادداشتی ثبت نشده"
        End Sub

        Private Sub BtnSaveEntryNote_Click(sender As Object, e As EventArgs) Handles btnSaveEntryNote.Click
            If Not _editEntryId.HasValue Then Return
            Dim userId = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)
            Try
                noteService.SaveNote(_editEntryId.Value, Nothing, userId, txtEntryNote.Text)
                Dim notes = noteService.GetNotes(_editEntryId.Value, Nothing)
                dgvEntryNotes.DataSource = notes
                HighlightCurrentUserRow(dgvEntryNotes, userId)
                UpdateEntryNoteInfo(notes, userId)
                MessageBox.Show("یادداشت سند ذخیره شد.", "ذخیره موفق",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnSaveLineNote_Click(sender As Object, e As EventArgs) Handles btnSaveLineNote.Click
            If Not _editEntryId.HasValue OrElse _zamLineNumber = 0 Then Return
            Dim userId = If(SessionContext.CurrentUser Is Nothing, 0, SessionContext.CurrentUser.UserID)
            Try
                noteService.SaveNote(_editEntryId.Value, _zamLineNumber, userId, txtLineNote.Text)
                Dim notes = noteService.GetNotes(_editEntryId.Value, _zamLineNumber)
                dgvLineNotes.DataSource = notes
                HighlightCurrentUserRow(dgvLineNotes, userId)
                UpdateLineNoteInfo(notes, userId)
                RefreshNoteLineSelector()
                MessageBox.Show("یادداشت ردیف ذخیره شد.", "ذخیره موفق",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' ──────────────────────────────────────────────────────────────────
        '  ضمائم سند
        ' ──────────────────────────────────────────────────────────────────

        Private ReadOnly attachmentService As New AttachmentService()
        Private _zamEntryId As Integer = 0
        Private _zamLineNumber As Integer = 0
        Private _currentPreviewImage As Image = Nothing
        Private _printImage As Image = Nothing
        Private _suppressZamLineSelectorChange As Boolean = False
        Private _suppressNoteLineSelectorChange As Boolean = False

        Private Sub TabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
            If tabMain.SelectedTab Is tabPageZamayem Then
                RefreshAttachmentTab()
            ElseIf tabMain.SelectedTab Is tabPageYaddasht Then
                RefreshNoteTab()
            End If
        End Sub

        Private Sub DgvEntryLines_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEntryLines.SelectionChanged
            If dgvEntryLines.CurrentRow Is Nothing Then Return
            Dim table = TryCast(dgvEntryLines.DataSource, DataTable)
            If table Is Nothing Then Return
            Dim rowIdx = dgvEntryLines.CurrentRow.Index
            If rowIdx < 0 OrElse rowIdx >= table.Rows.Count Then Return
            Dim lineNo = Convert.ToInt32(If(table.Rows(rowIdx)("LineNumber") Is DBNull.Value, 0, table.Rows(rowIdx)("LineNumber")))
            _zamLineNumber = lineNo
            If _editEntryId.HasValue Then _zamEntryId = _editEntryId.Value
        End Sub

        Private Sub InitializeAttachmentGrid()
            If dgvAttachments.Columns.Count > 0 Then Return
            dgvAttachments.AutoGenerateColumns = False

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "colAttachID"
            colId.DataPropertyName = "AttachmentID"
            colId.Visible = False

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colAttachName"
            colName.DataPropertyName = "FileName"
            colName.HeaderText = "نام فایل"
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            Dim colSize As New DataGridViewTextBoxColumn()
            colSize.Name = "colAttachSize"
            colSize.DataPropertyName = "FileSize"
            colSize.HeaderText = "حجم (بایت)"
            colSize.Width = 100

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colAttachDate"
            colDate.DataPropertyName = "AddedDate"
            colDate.HeaderText = "تاریخ افزودن"
            colDate.Width = 130

            dgvAttachments.Columns.AddRange(New DataGridViewColumn() {colId, colName, colSize, colDate})
        End Sub

        Private Sub InitializeZamLineSelector()
            If dgvZamLineSelector.Columns.Count > 0 Then Return
            dgvZamLineSelector.AutoGenerateColumns = False
            Dim col As New DataGridViewTextBoxColumn()
            col.Name = "colZamLine"
            col.DataPropertyName = "LineNumber"
            col.HeaderText = "ردیف"
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            col.ReadOnly = True
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvZamLineSelector.Columns.Add(col)
        End Sub

        Private Sub RefreshZamLineSelector()
            If Not _editEntryId.HasValue Then
                dgvZamLineSelector.DataSource = Nothing
                Return
            End If
            InitializeZamLineSelector()
            Try
                Dim dt = attachmentService.GetLinesWithAttachments(_editEntryId.Value)
                _suppressZamLineSelectorChange = True
                dgvZamLineSelector.DataSource = dt
                ' انتخاب ردیف جاری در لیست
                If _zamLineNumber > 0 Then
                    For Each row As DataGridViewRow In dgvZamLineSelector.Rows
                        Dim ln = Convert.ToInt32(If(row.Cells("colZamLine").Value Is DBNull.Value, 0, row.Cells("colZamLine").Value))
                        If ln = _zamLineNumber Then
                            dgvZamLineSelector.CurrentCell = row.Cells(0)
                            Exit For
                        End If
                    Next
                End If
                _suppressZamLineSelectorChange = False
            Catch ex As Exception
                _suppressZamLineSelectorChange = False
            End Try
        End Sub

        Private Sub DgvZamLineSelector_SelectionChanged(sender As Object, e As EventArgs) Handles dgvZamLineSelector.SelectionChanged
            If _suppressZamLineSelectorChange Then Return
            If dgvZamLineSelector.CurrentRow Is Nothing Then Return
            Dim val = dgvZamLineSelector.CurrentRow.Cells("colZamLine").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return
            Dim lineNum = Convert.ToInt32(val)
            If lineNum = _zamLineNumber Then Return
            _zamLineNumber = lineNum
            If _editEntryId.HasValue Then _zamEntryId = _editEntryId.Value
            RefreshAttachmentsForCurrentLine()
        End Sub

        Private Sub RefreshAttachmentsForCurrentLine()
            ClearPreview()
            If _zamLineNumber = 0 OrElse Not _editEntryId.HasValue Then
                lblSelectedLine.Text = "لطفاً یک ردیف از لیست انتخاب کنید"
                dgvAttachments.DataSource = Nothing
                SetAttachmentButtonsEnabled(False)
                Return
            End If
            lblSelectedLine.Text = "ضمائم ردیف سند شماره:  " & _zamLineNumber.ToString()
            Try
                dgvAttachments.DataSource = attachmentService.GetAttachments(_zamEntryId, _zamLineNumber)
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری ضمائم: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            SetAttachmentButtonsEnabled(True)
        End Sub

        Private Sub RefreshAttachmentTab()
            InitializeAttachmentGrid()
            InitializeZamLineSelector()
            ClearPreview()

            If Not _editEntryId.HasValue Then
                lblSelectedLine.Text = "ضمائم فقط پس از ذخیره سند قابل افزودن است — ابتدا سند را ثبت کنید"
                SetAttachmentButtonsEnabled(False)
                dgvZamLineSelector.DataSource = Nothing
                Return
            End If

            _zamEntryId = _editEntryId.Value
            RefreshZamLineSelector()

            If _zamLineNumber = 0 Then
                lblSelectedLine.Text = "لطفاً یک ردیف از لیست سمت راست انتخاب کنید"
                dgvAttachments.DataSource = Nothing
                SetAttachmentButtonsEnabled(False)
                Return
            End If

            RefreshAttachmentsForCurrentLine()
        End Sub

        Private Sub SetAttachmentButtonsEnabled(enabled As Boolean)
            btnZamAddFile.Enabled = enabled
            btnZamScan.Enabled = enabled
            btnZamView.Enabled = False
            btnZamPrint.Enabled = False
            btnZamDelete.Enabled = False
        End Sub

        Private Sub ClearPreview()
            If _currentPreviewImage IsNot Nothing Then
                _currentPreviewImage.Dispose()
                _currentPreviewImage = Nothing
            End If
            picAttachment.Image = Nothing
        End Sub

        Private Sub DgvAttachments_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAttachments.SelectionChanged
            If dgvAttachments.CurrentRow Is Nothing Then
                btnZamView.Enabled = False
                btnZamPrint.Enabled = False
                btnZamDelete.Enabled = False
                ClearPreview()
                Return
            End If
            LoadAttachmentPreview()
        End Sub

        Private Sub LoadAttachmentPreview()
            Dim attachId = GetSelectedAttachmentId()
            If attachId = 0 Then Return
            Try
                Dim data = attachmentService.GetImageData(attachId)
                ClearPreview()
                If data IsNot Nothing Then
                    _currentPreviewImage = attachmentService.BytesToImage(data)
                    picAttachment.Image = _currentPreviewImage
                    btnZamView.Enabled = True
                    btnZamPrint.Enabled = True
                    btnZamDelete.Enabled = True
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری تصویر: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetSelectedAttachmentId() As Integer
            If dgvAttachments.CurrentRow Is Nothing Then Return 0
            Dim val = dgvAttachments.CurrentRow.Cells("colAttachID").Value
            If val Is Nothing OrElse val Is DBNull.Value Then Return 0
            Return Convert.ToInt32(val)
        End Function

        Private Sub BtnZamAddFile_Click(sender As Object, e As EventArgs) Handles btnZamAddFile.Click
            Using dlg As New OpenFileDialog()
                dlg.Title = "انتخاب تصویر برای افزودن به ضمائم"
                dlg.Filter = "تصاویر|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff|همه فایل‌ها|*.*"
                dlg.Multiselect = True
                If dlg.ShowDialog() <> DialogResult.OK Then Return
                Try
                    For Each filePath In dlg.FileNames
                        Dim rawBytes = System.IO.File.ReadAllBytes(filePath)
                        attachmentService.SaveAttachment(_zamEntryId, _zamLineNumber,
                                                         System.IO.Path.GetFileName(filePath), rawBytes)
                    Next
                    RefreshAttachmentTab()
                Catch ex As Exception
                    MessageBox.Show("خطا در ذخیره فایل: " & ex.Message, "خطا",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Sub

        Private Sub BtnZamScan_Click(sender As Object, e As EventArgs) Handles btnZamScan.Click
            Try
                Dim wiaDialog As Object = Microsoft.VisualBasic.Interaction.CreateObject("WIA.CommonDialog")
                Dim wiaImage As Object = wiaDialog.ShowAcquireImage(
                    1,
                    1,
                    2,
                    "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}",
                    False, True, False)
                If wiaImage Is Nothing Then Return

                Dim tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                    "scan_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".bmp")
                wiaImage.SaveFile(tempFile)
                Dim rawBytes = System.IO.File.ReadAllBytes(tempFile)
                System.IO.File.Delete(tempFile)

                attachmentService.SaveAttachment(_zamEntryId, _zamLineNumber,
                    "scan_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".jpg", rawBytes)
                RefreshAttachmentTab()
            Catch comEx As System.Runtime.InteropServices.COMException
                If comEx.ErrorCode <> CInt(&H80210015) Then
                    MessageBox.Show("خطا در اتصال به اسکنر: " & comEx.Message, "خطا",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در اسکن: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnZamView_Click(sender As Object, e As EventArgs) Handles btnZamView.Click
            If _currentPreviewImage Is Nothing Then Return
            Dim viewForm As New Form()
            viewForm.Text = "مشاهده کامل ضمیمه"
            viewForm.WindowState = FormWindowState.Maximized
            viewForm.StartPosition = FormStartPosition.CenterScreen
            Dim pic As New PictureBox()
            pic.Dock = DockStyle.Fill
            pic.SizeMode = PictureBoxSizeMode.Zoom
            pic.BackColor = Color.FromArgb(30, 30, 30)
            pic.Image = _currentPreviewImage
            viewForm.Controls.Add(pic)
            viewForm.ShowDialog(Me)
            viewForm.Dispose()
        End Sub

        Private Sub BtnZamPrint_Click(sender As Object, e As EventArgs) Handles btnZamPrint.Click
            If _currentPreviewImage Is Nothing Then Return
            _printImage = _currentPreviewImage
            Using pd As New System.Drawing.Printing.PrintDocument()
                AddHandler pd.PrintPage, AddressOf AttachmentPrintPage
                Dim dlg As New PrintDialog()
                dlg.Document = pd
                If dlg.ShowDialog() = DialogResult.OK Then
                    pd.Print()
                End If
            End Using
        End Sub

        Private Sub AttachmentPrintPage(sender As Object,
                                        e As System.Drawing.Printing.PrintPageEventArgs)
            If _printImage Is Nothing Then Return
            Dim area = e.MarginBounds
            Dim ratio = Math.Min(CDbl(area.Width) / _printImage.Width,
                                 CDbl(area.Height) / _printImage.Height)
            Dim w = CInt(_printImage.Width * ratio)
            Dim h = CInt(_printImage.Height * ratio)
            Dim x = area.Left + (area.Width - w) \ 2
            Dim y = area.Top + (area.Height - h) \ 2
            e.Graphics.DrawImage(_printImage, x, y, w, h)
            e.HasMorePages = False
        End Sub

        Private Sub BtnZamDelete_Click(sender As Object, e As EventArgs) Handles btnZamDelete.Click
            Dim attachId = GetSelectedAttachmentId()
            If attachId = 0 Then Return
            Dim fileName = Convert.ToString(dgvAttachments.CurrentRow.Cells("colAttachName").Value)
            Dim ans = MessageBox.Show(
                "ضمیمه « " & fileName & " » حذف شود؟",
                "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ans <> DialogResult.Yes Then Return
            Try
                attachmentService.DeleteAttachment(attachId)
                ClearPreview()
                btnZamView.Enabled = False
                btnZamPrint.Enabled = False
                btnZamDelete.Enabled = False
                RefreshAttachmentTab()
            Catch ex As Exception
                MessageBox.Show("خطا در حذف: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub ApplySFSHColumnsVisibilityByPermission()
            Dim hide = False
            If SessionContext.CurrentPermissions IsNot Nothing AndAlso SessionContext.CurrentPermissions.Contains(PermissionKeys.HideSFSHInSanad) Then
                hide = True
            End If

            If dgvEntryLines.Columns.Contains("colSF") Then
                dgvEntryLines.Columns("colSF").Visible = Not hide
            End If
            If dgvEntryLines.Columns.Contains("colSH") Then
                dgvEntryLines.Columns("colSH").Visible = Not hide
            End If
        End Sub

        Private Sub pnlSerch_Paint(sender As Object, e As PaintEventArgs) Handles pnlSerch.Paint

        End Sub
    End Class
End Namespace
