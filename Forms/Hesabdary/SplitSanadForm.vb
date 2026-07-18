Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Partial Class SplitSanadForm
        Private ReadOnly service As New AccountingService()
        Private ReadOnly _sourceEntryId As Integer
        Private _loading As Boolean = True
        
        Private _sourceRefNum As String
        Private _sourceEntryDate As DateTime
        Private _sourceDescription As String
        
        Private _prevRefNum As String = "-"
        Private _prevEntryDate As DateTime? = Nothing
        Private _nextRefNum As String = "-"
        Private _nextEntryDate As DateTime? = Nothing

        ' Master lines collection
        Private _masterLines As New List(Of SplitLineItem)()

        ' Controls dictionary for new documents
        Private _txtRefs As New Dictionary(Of Integer, TextBox)()
        Private _txtDates As New Dictionary(Of Integer, TextBox)()
        Private _txtDescs As New Dictionary(Of Integer, TextBox)()
        Private _dgvs As New Dictionary(Of Integer, DataGridView)()

        Public Sub New(sourceEntryId As Integer)
            _sourceEntryId = sourceEntryId
            _loading = True
            InitializeComponent()
            LoadSourceAndAdjacentDocs()
            LoadOriginalLines()
            _loading = False
            RebuildTabs()
        End Sub

        Private Function ParseDbDate(val As Object) As DateTime?
            If val Is Nothing OrElse val Is DBNull.Value Then Return Nothing
            Try
                Dim dt = Convert.ToDateTime(val)
                If dt = DateTime.MinValue Then Return Nothing
                Return dt
            Catch
                Return Nothing
            End Try
        End Function

        Private Sub LoadSourceAndAdjacentDocs()
            Try
                Dim row = service.GetEntryById(_sourceEntryId)
                If row Is Nothing Then Throw New Exception("سند مورد نظر یافت نشد.")

                _sourceRefNum = Convert.ToString(row("ReferenceNumber"))
                Dim parsedSource = ParseDbDate(row("EntryDate"))
                If parsedSource.HasValue Then
                    _sourceEntryDate = parsedSource.Value
                Else
                    Throw New Exception("تاریخ سند مبدا نامعتبر است.")
                End If
                _sourceDescription = Convert.ToString(row("Description"))

                ' نمایش مشخصات سند اصلی
                lblSourceInfo.Text = "سند مبدا (فعلی):" & Environment.NewLine &
                                     "شماره: " & _sourceRefNum & Environment.NewLine &
                                     "تاریخ: " & PersianDateHelper.ToPersian(_sourceEntryDate) & Environment.NewLine &
                                     "شرح: " & _sourceDescription

                ' کوئری برای سند قبلی بلافصل
                Dim dtPrev = Sql.ExecuteTable(
                    "SELECT ReferenceNumber, EntryDate FROM Sanad1 " &
                    "WHERE CompanyID = ? AND FiscalYearID = ? AND CAST(ReferenceNumber AS INTEGER) < ? " &
                    "AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL) " &
                    "ORDER BY CAST(ReferenceNumber AS INTEGER) DESC LIMIT 1",
                    SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value, Convert.ToInt64(_sourceRefNum))

                If dtPrev.Rows.Count > 0 Then
                    _prevRefNum = Convert.ToString(dtPrev.Rows(0)("ReferenceNumber"))
                    Dim parsed = ParseDbDate(dtPrev.Rows(0)("EntryDate"))
                    If parsed.HasValue Then
                        _prevEntryDate = parsed.Value
                        lblPrevInfo.Text = "سند ماقبل (قبلی):" & Environment.NewLine &
                                           "شماره: " & _prevRefNum & Environment.NewLine &
                                           "تاریخ: " & PersianDateHelper.ToPersian(_prevEntryDate.Value)
                    Else
                        _prevRefNum = "-"
                        _prevEntryDate = Nothing
                        lblPrevInfo.Text = "سند ماقبل (قبلی):" & Environment.NewLine & "یافت نشد (اولین سند)"
                    End If
                Else
                    _prevRefNum = "-"
                    _prevEntryDate = Nothing
                    lblPrevInfo.Text = "سند ماقبل (قبلی):" & Environment.NewLine & "یافت نشد (اولین سند)"
                End If

                ' کوئری برای سند بعدی بلافصل
                Dim dtNext = Sql.ExecuteTable(
                    "SELECT ReferenceNumber, EntryDate FROM Sanad1 " &
                    "WHERE CompanyID = ? AND FiscalYearID = ? AND CAST(ReferenceNumber AS INTEGER) > ? " &
                    "AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL) " &
                    "ORDER BY CAST(ReferenceNumber AS INTEGER) ASC LIMIT 1",
                    SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value, Convert.ToInt64(_sourceRefNum))

                If dtNext.Rows.Count > 0 Then
                    _nextRefNum = Convert.ToString(dtNext.Rows(0)("ReferenceNumber"))
                    Dim parsed = ParseDbDate(dtNext.Rows(0)("EntryDate"))
                    If parsed.HasValue Then
                        _nextEntryDate = parsed.Value
                        lblNextInfo.Text = "سند مابعد (بعدی):" & Environment.NewLine &
                                           "شماره: " & _nextRefNum & Environment.NewLine &
                                           "تاریخ: " & PersianDateHelper.ToPersian(_nextEntryDate.Value)
                    Else
                        _nextRefNum = "-"
                        _nextEntryDate = Nothing
                        lblNextInfo.Text = "سند مابعد (بعدی):" & Environment.NewLine & "یافت نشد (آخرین سند)"
                    End If
                Else
                    _nextRefNum = "-"
                    _nextEntryDate = Nothing
                    lblNextInfo.Text = "سند مابعد (بعدی):" & Environment.NewLine & "یافت نشد (آخرین سند)"
                End If

                ' محاسبه پیشنهادات برای کاربر
                Try
                    Dim pc As New System.Globalization.PersianCalendar()
                    Dim srcPersian = PersianDateHelper.ToPersian(_sourceEntryDate)
                    Dim parts = srcPersian.Split("/"c)
                    Dim year = Convert.ToInt32(parts(0))
                    Dim month = Convert.ToInt32(parts(1))

                    Dim monthStartGeog = PersianDateHelper.ParsePersianDate(String.Format("{0:0000}/{1:00}/01", year, month)).Value
                    Dim daysInMonth = If(month <= 6, 31, If(month <= 11, 30, If(pc.IsLeapYear(year), 30, 29)))
                    Dim monthEndGeog = PersianDateHelper.ParsePersianDate(String.Format("{0:0000}/{1:00}/{2:00}", year, month, daysInMonth)).Value

                    ' دریافت لیست اسناد فعال برای محاسبه دقیق جاهای خالی و بازه زمانی مجاز آن‌ها
                    Dim activeDocs As List(Of ActiveDocInfo) = GetActiveDocsList()
                    Dim maxActiveRef As Long = If(activeDocs.Count > 0, activeDocs.Max(Function(x As ActiveDocInfo) x.RefVal), 0L)
                    Dim srcRefVal As Long = 0
                    Long.TryParse(_sourceRefNum, srcRefVal)

                    Dim limit As Long = Math.Max(maxActiveRef + 5, srcRefVal + 5)
                    Dim beforeList As New List(Of String)()
                    Dim afterList As New List(Of String)()

                    Dim activeRefHash As New HashSet(Of Long)()
                    For Each doc As ActiveDocInfo In activeDocs
                        activeRefHash.Add(doc.RefVal)
                    Next

                    For r As Long = 1 To limit
                        ' شماره سندی که خودش فعال است را رد می‌کنیم
                        If activeRefHash.Contains(r) Then Continue For

                        Dim currentRef As Long = r
                        ' پیدا کردن سند فعال قبل و بعد این شماره
                        Dim prevDoc As ActiveDocInfo = activeDocs.LastOrDefault(Function(x As ActiveDocInfo) x.RefVal < currentRef)
                        Dim nextDoc As ActiveDocInfo = activeDocs.FirstOrDefault(Function(x As ActiveDocInfo) x.RefVal > currentRef)

                        ' محاسبه محدوده تاریخ مجاز این شماره سند در داخل این ماه شمسی
                        Dim validStart = monthStartGeog
                        If prevDoc IsNot Nothing AndAlso prevDoc.DocDate > validStart Then
                            validStart = prevDoc.DocDate
                        End If

                        Dim validEnd = monthEndGeog
                        If nextDoc IsNot Nothing AndAlso nextDoc.DocDate < validEnd Then
                            validEnd = nextDoc.DocDate
                        End If

                        ' اگر بازه تاریخ مجاز برای این شماره سند در این ماه همپوشانی معتبری داشت
                        If validStart <= validEnd Then
                            If r < srcRefVal Then
                                beforeList.Add(r.ToString())
                            ElseIf r > srcRefVal Then
                                ' نمایش حداکثر ۵ شماره خالی بعد از مبداء
                                If afterList.Count < 5 Then
                                    afterList.Add(r.ToString())
                                End If
                            End If
                        End If
                    Next

                    Dim rangeStart = monthStartGeog
                    If _prevEntryDate.HasValue AndAlso _prevEntryDate.Value > rangeStart Then
                        rangeStart = _prevEntryDate.Value
                    End If

                    Dim rangeEnd = monthEndGeog
                    If _nextEntryDate.HasValue AndAlso _nextEntryDate.Value < rangeEnd Then
                        rangeEnd = _nextEntryDate.Value
                    End If

                    Dim suggestionText = "پیشنهاد تاریخ و شماره سند:" & Environment.NewLine &
                                         "بازه تاریخ مجاز: " & Environment.NewLine &
                                         PersianDateHelper.ToPersian(rangeStart) & " تا " & PersianDateHelper.ToPersian(rangeEnd) & Environment.NewLine & Environment.NewLine

                    Dim beforeRange = If(beforeList.Count > 0, String.Join("، ", beforeList), "شماره خالی یافت نشد")
                    Dim afterRange = If(afterList.Count > 0, String.Join("، ", afterList), "شماره خالی یافت نشد")

                    suggestionText &= "شماره‌های خالی قبل از مبدا: " & beforeRange & Environment.NewLine &
                                      "شماره‌های خالی بعد از مبدا: " & afterRange

                    lblSuggestions.Text = suggestionText
                Catch
                    lblSuggestions.Text = "خطا در محاسبه پیشنهادات"
                End Try

            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری مشخصات اسناد همسایه: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetActiveDocsList() As List(Of ActiveDocInfo)
            Dim activeDocs As New List(Of ActiveDocInfo)()
            Try
                Dim dt = Sql.ExecuteTable(
                    "SELECT ReferenceNumber, EntryDate FROM Sanad1 " &
                    "WHERE CompanyID = ? AND FiscalYearID = ? " &
                    "AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL)",
                    SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value)
                For Each row As DataRow In dt.Rows
                    Dim refVal As Long = 0
                    If Long.TryParse(Convert.ToString(row("ReferenceNumber")), refVal) Then
                        ' اگر هیچ ردیفی در سند اصلی مبدا باقی نمانده، آن را به عنوان فعال در نظر نمیگیریم چون قرار است حذف شود
                        Dim remainingLinesCount = _masterLines.Where(Function(x) x.TargetDocIndex = 0).Count()
                        If remainingLinesCount = 0 AndAlso refVal = Convert.ToInt64(_sourceRefNum) Then
                            Continue For
                        End If

                        Dim entryDate = Convert.ToDateTime(row("EntryDate"))
                        activeDocs.Add(New ActiveDocInfo With {
                            .RefText = Convert.ToString(row("ReferenceNumber")),
                            .RefVal = refVal,
                            .DocDate = entryDate
                        })
                    End If
                Next
            Catch
                ' Ignore
            End Try
            Return activeDocs.OrderBy(Function(x) x.RefVal).ToList()
        End Function

        Private Sub LoadOriginalLines()
            Try
                Dim dtDetails = service.GetEntryDetails(_sourceEntryId)
                _masterLines.Clear()
                For Each row As DataRow In dtDetails.Rows
                    _masterLines.Add(New SplitLineItem(row))
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری ردیف‌های سند مبدا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub NumNewDocs_ValueChanged(sender As Object, e As EventArgs) Handles numNewDocs.ValueChanged
            If _loading Then Return

            ' بازنشانی ردیف‌هایی که خارج از محدوده جدید تب‌ها قرار می‌گیرند به سند اصلی (مبداء یعنی 0)
            Dim maxAllowedIndex = Convert.ToInt32(numNewDocs.Value)
            For Each item In _masterLines
                If item.TargetDocIndex > maxAllowedIndex Then
                    item.TargetDocIndex = 0
                End If
            Next

            RebuildTabs()
        End Sub

        Private Sub RebuildTabs()
            tabDocs.TabPages.Clear()
            _txtRefs.Clear()
            _txtDates.Clear()
            _txtDescs.Clear()
            _dgvs.Clear()

            ' ۱. ایجاد تب سند مبداء (سند اصلی)
            Dim tabSource As New TabPage("سند مبدا (اصلی)")
            Dim dgvSource = CreateDgv(0)
            tabSource.Controls.Add(dgvSource)
            tabDocs.TabPages.Add(tabSource)
            _dgvs.Add(0, dgvSource)

            ' ۲. ایجاد تب‌های اسناد جدید خروجی
            Dim count = Convert.ToInt32(numNewDocs.Value)
            For i = 1 To count
                Dim tabNew As New TabPage("سند جدید " & i)
                
                ' بخش فیلدهای ورودی اطلاعات سند مقصد
                Dim pnlFields As New Panel() With {
                    .Dock = DockStyle.Top,
                    .Height = 85,
                    .BackColor = Color.FromArgb(245, 248, 253)
                }

                Dim lblRef As New Label() With { .Text = "شماره سند جدید:", .Location = New Point(10, 15), .AutoSize = True }
                Dim txtRef As New TextBox() With {
                    .Location = New Point(120, 12),
                    .Size = New Size(80, 22),
                    .Text = service.GetNextSuggestedCode(Nothing)
                }

                Dim lblDate As New Label() With { .Text = "تاریخ سند خروجی:", .Location = New Point(220, 15), .AutoSize = True }
                Dim txtDate As New TextBox() With {
                    .Location = New Point(330, 12),
                    .Size = New Size(100, 22),
                    .Text = PersianDateHelper.ToPersian(_sourceEntryDate)
                }

                Dim lblDesc As New Label() With { .Text = "شرح سند جدید:", .Location = New Point(10, 48), .AutoSize = True }
                Dim txtDesc As New TextBox() With {
                    .Location = New Point(120, 45),
                    .Size = New Size(500, 22),
                    .Text = "تجزیه شده از سند شماره " & _sourceRefNum & " - " & _sourceDescription
                }

                pnlFields.Controls.Add(lblRef)
                pnlFields.Controls.Add(txtRef)
                pnlFields.Controls.Add(lblDate)
                pnlFields.Controls.Add(txtDate)
                pnlFields.Controls.Add(lblDesc)
                pnlFields.Controls.Add(txtDesc)

                ' ذخیره ارجاع فیلدها در لغت‌نامه برای خواندن مقادیر در هنگام تایید
                _txtRefs.Add(i, txtRef)
                _txtDates.Add(i, txtDate)
                _txtDescs.Add(i, txtDesc)

                ' ساخت جدول ردیف‌های این سند
                Dim dgvNew = CreateDgv(i)
                dgvNew.Dock = DockStyle.Fill

                Dim containerPanel As New Panel() With { .Dock = DockStyle.Fill }
                containerPanel.Controls.Add(dgvNew)
                containerPanel.Controls.Add(pnlFields)

                tabNew.Controls.Add(containerPanel)
                tabDocs.TabPages.Add(tabNew)
                _dgvs.Add(i, dgvNew)
            Next

            RefreshAllGrids()
        End Sub

        Private Function CreateDgv(targetIndex As Integer) As DataGridView
            Dim dgv As New DataGridView() With {
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False,
                .ReadOnly = True,
                .Dock = DockStyle.Fill,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = True,
                .RowHeadersVisible = False,
                .AutoGenerateColumns = False
            }

            Dim colCode As New DataGridViewTextBoxColumn() With { .Name = "colCode", .HeaderText = "کد حساب", .Width = 100 }
            Dim colName As New DataGridViewTextBoxColumn() With { .Name = "colName", .HeaderText = "نام حساب", .Width = 150 }
            Dim colSharh As New DataGridViewTextBoxColumn() With { .Name = "colSharh", .HeaderText = "شرح ردیف", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
            Dim colBed As New DataGridViewTextBoxColumn() With { .Name = "colBed", .HeaderText = "بدهکار", .Width = 110 }
            Dim colBes As New DataGridViewTextBoxColumn() With { .Name = "colBes", .HeaderText = "بستانکار", .Width = 110 }

            dgv.Columns.AddRange({colCode, colName, colSharh, colBed, colBes})

            ' افزودن رویداد کلیک راست برای انتقال خطوط
            AddHandler dgv.CellMouseDown, Sub(s As Object, e As DataGridViewCellMouseEventArgs)
                If e.Button = MouseButtons.Right AndAlso e.RowIndex >= 0 Then
                    If Not dgv.Rows(e.RowIndex).Selected Then
                        dgv.ClearSelection()
                        dgv.Rows(e.RowIndex).Selected = True
                    End If
                    ShowMoveContextMenu(dgv, targetIndex, e.Location)
                End If
            End Sub

            Return dgv
        End Function

        Private Sub ShowMoveContextMenu(dgv As DataGridView, currentTab As Integer, pt As Point)
            Dim ctxMenu As New ContextMenuStrip()
            
            Dim selectedRows = dgv.SelectedRows
            If selectedRows.Count = 0 Then Return

            ' آیتم سند اصلی
            If currentTab <> 0 Then
                Dim itemSrc = ctxMenu.Items.Add("انتقال به سند اصلی (مبداء)")
                AddHandler itemSrc.Click, Sub() MoveSelectedLines(selectedRows, 0)
            End If

            ' آیتم اسناد جدید
            Dim count = Convert.ToInt32(numNewDocs.Value)
            For i = 1 To count
                If currentTab <> i Then
                    Dim target = i
                    Dim itemNew = ctxMenu.Items.Add("انتقال به سند جدید " & target)
                    AddHandler itemNew.Click, Sub() MoveSelectedLines(selectedRows, target)
                End If
            Next

            ctxMenu.Show(dgv, dgv.PointToClient(Cursor.Position))
        End Sub

        Private Sub MoveSelectedLines(selectedRows As DataGridViewSelectedRowCollection, targetIndex As Integer)
            For Each row As DataGridViewRow In selectedRows
                Dim item = TryCast(row.Tag, SplitLineItem)
                If item IsNot Nothing Then
                    item.TargetDocIndex = targetIndex
                End If
            Next

            RefreshAllGrids()
        End Sub

        Private Sub RefreshAllGrids()
            For Each pair In _dgvs
                PopulateGrid(pair.Value, pair.Key)
            Next
        End Sub

        Private Sub PopulateGrid(dgv As DataGridView, targetIndex As Integer)
            dgv.Rows.Clear()
            Dim lines = _masterLines.Where(Function(x) x.TargetDocIndex = targetIndex).ToList()
            For Each line In lines
                Dim rowIndex = dgv.Rows.Add()
                Dim row = dgv.Rows(rowIndex)
                row.Cells("colCode").Value = line.AccountCode
                row.Cells("colName").Value = line.AccountName
                row.Cells("colSharh").Value = line.SharhRadif
                row.Cells("colBed").Value = line.DebitAmount.ToString("N0")
                row.Cells("colBes").Value = line.CreditAmount.ToString("N0")
                row.Tag = line
            Next
        End Sub

        Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
            ' ۱. بررسی انتقال حداقل یک ردیف به سند جدید
            Dim movedCount = _masterLines.Where(Function(x) x.TargetDocIndex > 0).Count()
            If movedCount = 0 Then
                MessageBox.Show("لطفا حداقل یک ردیف را به اسناد مقصد جدید منتقل کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ۲. استخراج ماه تاریخ مبداء
            Dim srcDateStr = PersianDateHelper.ToPersian(_sourceEntryDate)
            Dim srcMonth = srcDateStr.Split("/"c)(1)

            Dim srcRefVal As Long = 0
            Long.TryParse(_sourceRefNum, srcRefVal)

            ' لیست نهایی اسناد مقصد جدید جهت اعتبارسنجی
            Dim destDocs As New List(Of DestDocInfo)()
            Dim count = Convert.ToInt32(numNewDocs.Value)

            For i = 1 To count
                Dim refText = _txtRefs(i).Text.Trim()
                Dim dateText = _txtDates(i).Text.Trim()
                Dim descText = _txtDescs(i).Text.Trim()

                ' بررسی وارد کردن شماره سند
                Dim refVal As Long = 0
                If Not Long.TryParse(refText, refVal) OrElse refVal <= 0 Then
                    MessageBox.Show("شماره سند وارد شده در سند جدید " & i & " نامعتبر است. شماره سند حتما باید یک عدد بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' بررسی فرمت تاریخ
                Dim parsedDate = PersianDateHelper.ParsePersianDate(dateText)
                If Not parsedDate.HasValue Then
                    MessageBox.Show("تاریخ وارد شده در سند جدید " & i & " (" & dateText & ") نامعتبر است. فرمت صحیح: yyyy/mm/dd", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' بررسی رعایت یکسانی ماه
                Dim destMonth = dateText.Split("/"c)(1)
                If destMonth <> srcMonth Then
                    MessageBox.Show("ماه تاریخ سند جدید " & i & " (" & destMonth & ") با ماه سند مبدا (" & srcMonth & ") یکسان نیست.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                destDocs.Add(New DestDocInfo With {
                    .Index = i,
                    .RefText = refText,
                    .RefVal = refVal,
                    .DocDate = parsedDate.Value,
                    .Description = descText
                })
            Next

            ' ۳. اعتبارسنجی تاریخ هر سند مقصد بر اساس موقعیت شماره سند آن در بین اسناد فعال دیتابیس
            Dim activeDocs As List(Of ActiveDocInfo) = GetActiveDocsList()

            For Each doc In destDocs
                ' پیدا کردن سند فعال ماقبل این شماره سند
                Dim prevDoc As ActiveDocInfo = activeDocs.LastOrDefault(Function(x As ActiveDocInfo) x.RefVal < doc.RefVal)
                ' پیدا کردن سند فعال مابعد این شماره سند
                Dim nextDoc As ActiveDocInfo = activeDocs.FirstOrDefault(Function(x As ActiveDocInfo) x.RefVal > doc.RefVal)

                If prevDoc IsNot Nothing AndAlso doc.DocDate < prevDoc.DocDate Then
                    MessageBox.Show("تاریخ سند جدید " & doc.Index & " (" & PersianDateHelper.ToPersian(doc.DocDate) & ") نمی‌تواند کوچکتر از تاریخ سند شماره " & prevDoc.RefText & " (" & PersianDateHelper.ToPersian(prevDoc.DocDate) & ") باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                If nextDoc IsNot Nothing AndAlso doc.DocDate > nextDoc.DocDate Then
                    MessageBox.Show("تاریخ سند جدید " & doc.Index & " (" & PersianDateHelper.ToPersian(doc.DocDate) & ") نمی‌تواند بزرگتر از تاریخ سند شماره " & nextDoc.RefText & " (" & PersianDateHelper.ToPersian(nextDoc.DocDate) & ") باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            ' ۴. اعتبارسنجی شماره اسناد مقصد نسبت به سند مبدا و تاریخ‌ها
            For Each doc In destDocs
                If doc.DocDate < _sourceEntryDate AndAlso doc.RefVal >= srcRefVal Then
                    MessageBox.Show("تاریخ سند جدید " & doc.Index & " قبل از سند مبدا است، لذا شماره آن (" & doc.RefText & ") حتما باید کوچکتر از شماره سند مبدا (" & _sourceRefNum & ") باشد تا نظم ترتیب اسناد حفظ شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                If doc.DocDate > _sourceEntryDate AndAlso doc.RefVal <= srcRefVal Then
                    MessageBox.Show("تاریخ سند جدید " & doc.Index & " بعد از سند مبدا است، لذا شماره آن (" & doc.RefText & ") حتما باید بزرگتر از شماره سند مبدا (" & _sourceRefNum & ") باشد تا نظم ترتیب اسناد حفظ شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            ' ۴. اعتبارسنجی شماره و تاریخ‌ها بین خود اسناد مقصد خروجی
            For i = 0 To destDocs.Count - 1
                For j = i + 1 To destDocs.Count - 1
                    Dim docA = destDocs(i)
                    Dim docB = destDocs(j)
                    If docA.DocDate < docB.DocDate AndAlso docA.RefVal >= docB.RefVal Then
                        MessageBox.Show("تاریخ سند جدید " & docA.Index & " قبل از سند جدید " & docB.Index & " است، لذا شماره آن (" & docA.RefText & ") باید کوچکتر از شماره سند جدید " & docB.Index & " (" & docB.RefText & ") باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    If docA.DocDate > docB.DocDate AndAlso docA.RefVal <= docB.RefVal Then
                        MessageBox.Show("تاریخ سند جدید " & docA.Index & " بعد از سند جدید " & docB.Index & " است، لذا شماره آن (" & docA.RefText & ") باید بزرگتر از شماره سند جدید " & docB.Index & " (" & docB.RefText & ") باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                Next
            Next

            ' ۵. بررسی عدم تکراری بودن شماره سندهای خروجی در پایگاه داده
            Dim remainingLinesCount = _masterLines.Where(Function(x) x.TargetDocIndex = 0).Count()
            Dim excludeId As Integer? = Nothing
            If remainingLinesCount = 0 Then
                excludeId = _sourceEntryId
            End If

            For Each doc In destDocs
                ' بررسی تکراری نبودن شماره سندهای مقصد با یکدیگر
                If destDocs.Where(Function(x) x.RefText = doc.RefText).Count() > 1 Then
                    MessageBox.Show("شماره سند جدید " & doc.Index & " (" & doc.RefText & ") در لیست اسناد مقصد تکراری است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' بررسی تکراری نبودن شماره سند مبدا در صورت باقی ماندن ردیف
                If remainingLinesCount > 0 AndAlso doc.RefText = _sourceRefNum Then
                    MessageBox.Show("از آنجا که برخی ردیف‌ها در سند اصلی باقی مانده‌اند، شماره سند جدید " & doc.Index & " نمی‌تواند برابر با شماره سند اصلی (" & _sourceRefNum & ") باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim duplicateExists = service.IsReferenceNumberDuplicate(doc.RefText, excludeId)
                If duplicateExists Then
                    MessageBox.Show("شماره سند جدید " & doc.Index & " (" & doc.RefText & ") قبلا در سیستم ثبت شده است و تکراری می‌باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            ' تایید عملیات توسط کاربر
            Dim ans = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید سند اصلی شماره " & _sourceRefNum & " را به " & (destDocs.Count + 1) & " سند مجزا تجزیه کنید؟",
                "تایید تجزیه سند", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ans <> DialogResult.Yes Then Return

            Try
                ' ۶. اجرای فرآیند تجزیه در تراکنش دیتابیس
                ' ۶-۱. بروزرسانی یا حذف موقت سند مبدا با ردیف‌های باقی‌مانده (TargetDocIndex = 0)
                Dim remainingLines = _masterLines.Where(Function(x) x.TargetDocIndex = 0).ToList()
                Dim remainingEntryLines As New List(Of AccountingEntryLine)()
                Dim remIndex = 1
                For Each line In remainingLines
                    remainingEntryLines.Add(New AccountingEntryLine(line.AccountID, line.DebitAmount, line.CreditAmount, remIndex, line.ShenavarID, line.SharhRadif, line.TransactionNumber, line.TransactionDate))
                    remIndex += 1
                Next

                Dim totalBedRem = remainingEntryLines.Sum(Function(x) x.DebitAmount)
                Dim totalBesRem = remainingEntryLines.Sum(Function(x) x.CreditAmount)
                Dim taeazRem = If(totalBedRem = totalBesRem, "تراز", If(totalBedRem > totalBesRem, "بدهکار", "بستانکار"))

                Dim updatedBy = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

                If remainingLinesCount = 0 Then
                    ' اگر هیچ ردیفی باقی نمانده، سند مبدا را حذف موقت می‌کنیم
                    service.SetEntryStatus(_sourceEntryId, "سند موقت - حذف موقت")
                Else
                    ' بروزرسانی سند اصلی
                    service.UpdateEntry(_sourceEntryId, _sourceEntryDate, _sourceDescription & " (تجزیه شده)", _sourceRefNum, updatedBy, remainingEntryLines, totalBedRem, totalBesRem, taeazRem)
                End If

                ' ۶-۲. ایجاد اسناد جدید
                For Each doc In destDocs
                    Dim targetIndex = doc.Index
                    Dim docLines = _masterLines.Where(Function(x) x.TargetDocIndex = targetIndex).ToList()
                    Dim docEntryLines As New List(Of AccountingEntryLine)()
                    Dim lineIndex = 1
                    For Each line In docLines
                        docEntryLines.Add(New AccountingEntryLine(line.AccountID, line.DebitAmount, line.CreditAmount, lineIndex, line.ShenavarID, line.SharhRadif, line.TransactionNumber, line.TransactionDate))
                        lineIndex += 1
                    Next

                    Dim totalBedDoc = docEntryLines.Sum(Function(x) x.DebitAmount)
                    Dim totalBesDoc = docEntryLines.Sum(Function(x) x.CreditAmount)
                    Dim taeazDoc = If(totalBedDoc = totalBesDoc, "تراز", If(totalBedDoc > totalBesDoc, "بدهکار", "بستانکار"))

                    ' ذخیره سند جدید خروجی
                    service.SaveEntry(doc.DocDate, doc.Description, doc.RefText, updatedBy, docEntryLines, totalBedDoc, totalBesDoc, taeazDoc)
                Next

                ' نمایش پیغام موفقیت
                Dim newDocsStr = String.Join(Environment.NewLine, destDocs.Select(Function(x) " - سند شماره " & x.RefText & " به تاریخ " & PersianDateHelper.ToPersian(x.DocDate)))
                Dim successMsg = "عملیات تجزیه سند با موفقیت انجام شد." & Environment.NewLine &
                                 "سند مبدا شماره « " & _sourceRefNum & " » به اسناد زیر تفکیک گردید:" & Environment.NewLine &
                                 newDocsStr & Environment.NewLine &
                                 "لازم است که مدارک فیزیکی ردیف‌های تفکیک شده را هم به اسناد مربوطه پیوست نمایید."

                MessageBox.Show(successMsg, "تجزیه سند موفقیت‌آمیز", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("خطا در حین فرآیند تجزیه سند در دیتابیس: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Class SplitLineItem
            Public Property AccountID As Integer
            Public Property AccountCode As String
            Public Property AccountName As String
            Public Property DebitAmount As Decimal
            Public Property CreditAmount As Decimal
            Public Property ShenavarID As Integer
            Public Property SharhRadif As String
            Public Property TransactionNumber As String
            Public Property TransactionDate As String
            Public Property TargetDocIndex As Integer

            Public Sub New(row As DataRow)
                Me.AccountID = Convert.ToInt32(row("AccountID"))
                Me.AccountCode = Convert.ToString(row("AccountCode"))
                Me.AccountName = Convert.ToString(row("AccountName"))
                Me.DebitAmount = Convert.ToDecimal(If(row("DebitAmount") Is DBNull.Value, 0D, row("DebitAmount")))
                Me.CreditAmount = Convert.ToDecimal(If(row("CreditAmount") Is DBNull.Value, 0D, row("CreditAmount")))
                Me.ShenavarID = If(row("ShenavarID") Is DBNull.Value, 0, Convert.ToInt32(row("ShenavarID")))
                Me.SharhRadif = If(row("SharhRadif") Is DBNull.Value, "", Convert.ToString(row("SharhRadif")))
                Me.TransactionNumber = If(row("TransactionNumber") Is DBNull.Value, "", Convert.ToString(row("TransactionNumber")))
                Me.TransactionDate = If(row("TransactionDate") Is DBNull.Value, "", Convert.ToString(row("TransactionDate")))
                Me.TargetDocIndex = 0
            End Sub
        End Class

        Private Class DestDocInfo
            Public Property Index As Integer
            Public Property RefText As String
            Public Property RefVal As Long
            Public Property DocDate As DateTime
            Public Property Description As String
        End Class

        Private Class ActiveDocInfo
            Public Property RefText As String
            Public Property RefVal As Long
            Public Property DocDate As DateTime
        End Class
    End Class
End Namespace
