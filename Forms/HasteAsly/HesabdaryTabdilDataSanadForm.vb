Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms

    Public Class HesabdaryTabdilDataSanadForm
        Inherits Form

        ' â”€â”€ Fields â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Private _companyId As Integer
        Private _deleteExisting As Boolean
        Private _hasDeleted As Boolean = False

        ' Sanad2 state
        Private _sanad2FilePath As String = String.Empty
        Private _sanad2Columns As New List(Of String)()
        Private _sanad2Rows As New List(Of String())()

        ' Sanad1 state
        Private _sanad1FilePath As String = String.Empty
        Private _sanad1Columns As New List(Of String)()
        Private _sanad1Rows As New List(Of String())()

        ' Progress bars (Runtime only)
        Private pnlProgressOverlay As Panel
        Private pnlProgressBox As Panel
        Private lblProgressOverallTitle As Label
        Private pbProgressOverall As ProgressBar
        Private lblProgressDetailTitle As Label
        Private pbProgressDetail As ProgressBar
        Private lblProgressStatus As Label

        ' Mapping combos
        Private ReadOnly _sanad2MappingCombos As ComboBox()
        Private ReadOnly _sanad1MappingCombos As ComboBox()

        ' â”€â”€ Constructor â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Public Sub New(companyId As Integer, deleteExisting As Boolean)
            InitializeComponent()
            _companyId = companyId
            _deleteExisting = deleteExisting
            _sanad2MappingCombos = New ComboBox() {
                cmbSanad2ShomareSanad, cmbSanad2TarikSanad, cmbSanad2Goruh, cmbSanad2Kol, cmbSanad2Moein,
                cmbSanad2Tafsili1, cmbSanad2Tafsili2, cmbSanad2Tafsili3, cmbSanad2Bedehkar, cmbSanad2Bestankar,
                cmbSanad2TxNum, cmbSanad2TxDate, cmbSanad2SharhRadif
            }
            _sanad1MappingCombos = New ComboBox() {
                cmbSanad1ShomareSanad, cmbSanad1TarikSanad, cmbSanad1Sharh
            }
        End Sub

        ' â”€â”€ Load â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Private Sub HesabdaryTabdilDataSanadForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            InitializeProgressOverlay()

            AddHandler nudSanad2HeaderRow.ValueChanged, AddressOf nudSanad2HeaderRow_ValueChanged
            AddHandler nudSanad1HeaderRow.ValueChanged, AddressOf nudSanad1HeaderRow_ValueChanged

            For Each cmb In _sanad2MappingCombos
                AddHandler cmb.SelectedIndexChanged, AddressOf Sanad2MappingCombo_Changed
            Next
            For Each cmb In _sanad1MappingCombos
                AddHandler cmb.SelectedIndexChanged, AddressOf Sanad1MappingCombo_Changed
            Next

            Try
                Dim levelCountObj = Sql.ExecuteScalar("SELECT AccountLevels FROM Companies WHERE CompanyID = ?", _companyId)
                If levelCountObj IsNot Nothing AndAlso Not Convert.IsDBNull(levelCountObj) Then
                    Dim levelCount = Convert.ToInt32(levelCountObj)
                    If levelCount < 3 Then
                        cmbSanad2Moein.Visible = False
                        lblSanad2Moein.Visible = False
                    End If
                    If levelCount < 4 Then
                        cmbSanad2Tafsili1.Visible = False
                        lblSanad2Tafsili1.Visible = False
                    End If
                    If levelCount < 5 Then
                        cmbSanad2Tafsili2.Visible = False
                        lblSanad2Tafsili2.Visible = False
                    End If
                    If levelCount < 6 Then
                        cmbSanad2Tafsili3.Visible = False
                        lblSanad2Tafsili3.Visible = False
                    End If
                End If
            Catch
            End Try
        End Sub

        Private Sub InitializeProgressOverlay()
            If pnlProgressOverlay IsNot Nothing Then Return

            pnlProgressOverlay = New Panel()
            pnlProgressBox = New Panel()
            lblProgressOverallTitle = New Label()
            pbProgressOverall = New ProgressBar()
            lblProgressDetailTitle = New Label()
            pbProgressDetail = New ProgressBar()
            lblProgressStatus = New Label()

            ' pnlProgressOverlay
            pnlProgressOverlay.Dock = DockStyle.None
            pnlProgressOverlay.Location = New System.Drawing.Point(0, 0)
            pnlProgressOverlay.Size = Me.ClientSize
            pnlProgressOverlay.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            pnlProgressOverlay.BackColor = System.Drawing.Color.FromArgb(240, 240, 240)
            pnlProgressOverlay.Visible = False

            ' pnlProgressBox
            pnlProgressBox.BorderStyle = BorderStyle.FixedSingle
            pnlProgressBox.Size = New System.Drawing.Size(500, 190)
            pnlProgressBox.BackColor = System.Drawing.Color.White
            pnlProgressBox.Location = New System.Drawing.Point((pnlProgressOverlay.Width - 500) \ 2, (pnlProgressOverlay.Height - 190) \ 2)
            pnlProgressBox.Anchor = AnchorStyles.None

            ' lblProgressOverallTitle
            lblProgressOverallTitle.Location = New System.Drawing.Point(20, 15)
            lblProgressOverallTitle.Size = New System.Drawing.Size(460, 20)
            lblProgressOverallTitle.Text = "پيشرفت کلي عمليات:"
            lblProgressOverallTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)

            ' pbProgressOverall
            pbProgressOverall.Location = New System.Drawing.Point(20, 38)
            pbProgressOverall.Size = New System.Drawing.Size(460, 22)

            ' lblProgressDetailTitle
            lblProgressDetailTitle.Location = New System.Drawing.Point(20, 75)
            lblProgressDetailTitle.Size = New System.Drawing.Size(460, 20)
            lblProgressDetailTitle.Text = "جزئيات عمليات جاري:"
            lblProgressDetailTitle.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)

            ' pbProgressDetail
            pbProgressDetail.Location = New System.Drawing.Point(20, 98)
            pbProgressDetail.Size = New System.Drawing.Size(460, 22)

            ' lblProgressStatus
            lblProgressStatus.Location = New System.Drawing.Point(20, 130)
            lblProgressStatus.Size = New System.Drawing.Size(460, 45)
            lblProgressStatus.Text = "در حال شروع..."
            lblProgressStatus.Font = New System.Drawing.Font("Tahoma", 8.5!)
            lblProgressStatus.ForeColor = System.Drawing.Color.FromArgb(180, 0, 0)
            lblProgressStatus.TextAlign = System.Drawing.ContentAlignment.TopRight

            ' Assemble
            pnlProgressBox.Controls.Add(lblProgressOverallTitle)
            pnlProgressBox.Controls.Add(pbProgressOverall)
            pnlProgressBox.Controls.Add(lblProgressDetailTitle)
            pnlProgressBox.Controls.Add(pbProgressDetail)
            pnlProgressBox.Controls.Add(lblProgressStatus)

            pnlProgressOverlay.Controls.Add(pnlProgressBox)
            Me.Controls.Add(pnlProgressOverlay)
        End Sub

        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
        ' SANAD 2
        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ

        Private Sub btnSanad2SelectFile_Click(sender As Object, e As EventArgs) Handles btnSanad2SelectFile.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "همه فایل‌های پشتیبانی شده (*.xlsx;*.xls;*.csv;*.txt)|*.xlsx;*.xls;*.csv;*.txt|اکسل (*.xlsx;*.xls)|*.xlsx;*.xls|CSV و متنی (*.csv;*.txt)|*.csv;*.txt|همه فایل‌ها (*.*)|*.*"
                ofd.Title = "انتخاب فایل سند 2"
                If ofd.ShowDialog() <> DialogResult.OK Then Return
                _sanad2FilePath = ofd.FileName
                lblSanad2File.Text = Path.GetFileName(_sanad2FilePath)
                LoadSanad2File()
            End Using
        End Sub

        Private Sub LoadSanad2File()
            If String.IsNullOrEmpty(_sanad2FilePath) Then Return
            Try
                Dim ext = Path.GetExtension(_sanad2FilePath).ToLower()
                _sanad2Rows.Clear()
                If ext = ".xlsx" OrElse ext = ".xls" Then
                    Dim rows = ReadExcelToRows(_sanad2FilePath, ext)
                    If rows.Count = 0 Then
                        MessageBox.Show("فایل اکسل خالی است یا شیت‌ی یافت نشد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    _sanad2Rows.AddRange(rows)
                Else
                    Dim lines = File.ReadAllLines(_sanad2FilePath, Encoding.UTF8)
                    Dim sep = DetectSeparator(lines(0))
                    For Each line In lines
                        _sanad2Rows.Add(line.Split(sep))
                    Next
                End If
                UpdateSanad2ColumnMappings()
                lblSanad2RecordCount.Text = "تعداد رکوردهای فایل اکسل سند 2: 0"
                dgvSanad2Preview.DataSource = Nothing
                CheckAndEnableConvertButton()
            Catch ex As Exception
                MessageBox.Show("خطا در خواندن فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub nudSanad2HeaderRow_ValueChanged(sender As Object, e As EventArgs)
            UpdateSanad2ColumnMappings()
            lblSanad2RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 2: 0"
            dgvSanad2Preview.DataSource = Nothing
            CheckAndEnableConvertButton()
        End Sub

        Private Sub UpdateSanad2ColumnMappings()
            lblSanad2RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 2: 0"
            dgvSanad2Preview.DataSource = Nothing
            CheckAndEnableConvertButton()
            If _sanad2Rows.Count = 0 Then Return
            Dim hIdx = CInt(nudSanad2HeaderRow.Value) - 1
            If hIdx >= _sanad2Rows.Count Then Return
            _sanad2Columns = _sanad2Rows(hIdx).ToList()
            FillMappingCombos(_sanad2MappingCombos, _sanad2Columns)
        End Sub

        Private _s2Refreshing As Boolean = False
        Private Sub Sanad2MappingCombo_Changed(sender As Object, e As EventArgs)
            If _s2Refreshing Then Return
            RefreshExclusivity(_sanad2MappingCombos, _sanad2Columns, _s2Refreshing)
        End Sub

        Private Sub btnSanad2Preview_Click(sender As Object, e As EventArgs) Handles btnSanad2Preview.Click
            If _sanad2Rows.Count = 0 Then
                MessageBox.Show("ابتدا فایل سند 2 را انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim headerIdx = CInt(nudSanad2HeaderRow.Value) - 1
            LoadPreview(dgvSanad2Preview, _sanad2Rows, headerIdx)

            Dim realCount As Integer = 0
            For i = 0 To _sanad2Rows.Count - 1
                If i = headerIdx Then Continue For
                Dim row = _sanad2Rows(i)
                If Not row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then
                    realCount += 1
                End If
            Next
            lblSanad2RecordCount.Text = "تعداد رکوردهای فایل اکسل سند 2: " & realCount.ToString()

            CheckAndEnableConvertButton()
        End Sub



        Private Function ConvertSanad2(maxDate As DateTime) As Integer
            If _deleteExisting AndAlso Not _hasDeleted Then
                Sql.ExecuteNonQuery("DELETE FROM Sanad2 WHERE EntryID IN (SELECT EntryID FROM Sanad1 WHERE CompanyID = ?)", _companyId)
                Sql.ExecuteNonQuery("DELETE FROM Sanad1 WHERE CompanyID = ?", _companyId)
                _hasDeleted = True
            End If

            Dim hIdx = CInt(nudSanad2HeaderRow.Value) - 1

            Dim idxShSanad = GetColIdx(cmbSanad2ShomareSanad, _sanad2Columns)
            Dim idxGoruh = GetColIdx(cmbSanad2Goruh, _sanad2Columns)
            Dim idxKol = GetColIdx(cmbSanad2Kol, _sanad2Columns)
            Dim idxMoein = GetColIdx(cmbSanad2Moein, _sanad2Columns)
            Dim idxTafs1 = GetColIdx(cmbSanad2Tafsili1, _sanad2Columns)
            Dim idxTafs2 = GetColIdx(cmbSanad2Tafsili2, _sanad2Columns)
            Dim idxTafs3 = GetColIdx(cmbSanad2Tafsili3, _sanad2Columns)
            Dim idxBedeh = GetColIdx(cmbSanad2Bedehkar, _sanad2Columns)
            Dim idxBestan = GetColIdx(cmbSanad2Bestankar, _sanad2Columns)
            Dim idxTxNum = GetColIdx(cmbSanad2TxNum, _sanad2Columns)
            Dim idxTxDate = GetColIdx(cmbSanad2TxDate, _sanad2Columns)
            Dim idxTarikSanad = GetColIdx(cmbSanad2TarikSanad, _sanad2Columns)
            Dim idxSharhRadif = GetColIdx(cmbSanad2SharhRadif, _sanad2Columns)

            ' ─── پیش‌پردازش: بررسی تکراری بودن شماره سندها در فایل اکسل سند2 ───
            Dim _s2PreDocNums As New List(Of String)()
            For _pi = 0 To _sanad2Rows.Count - 1
                If _pi = hIdx Then Continue For
                Dim _prow = _sanad2Rows(_pi)
                If _prow.All(Function(c) String.IsNullOrWhiteSpace(c)) Then Continue For
                Dim _psn = GetCell(_prow, idxShSanad)
                If Not String.IsNullOrEmpty(_psn) Then _s2PreDocNums.Add(_psn.Trim())
            Next
            Dim hasDuplicateDocNums As Boolean = _s2PreDocNums.Count <> _s2PreDocNums.Distinct(StringComparer.OrdinalIgnoreCase).Count()

            ' مرحله اول: ساخت جدول جستجوی شماره سند → سال مالی از تیبل سند1 (که قبلاً درج شده)
            Dim sanad1FyLookup As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            If Not hasDuplicateDocNums Then
                Try
                    Dim _s1Dt = Sql.ExecuteTable("SELECT ReferenceNumber, FiscalYearID FROM Sanad1 WHERE CompanyID = ?", _companyId)
                    For Each _s1r As DataRow In _s1Dt.Rows
                        Dim _rn = Convert.ToString(_s1r("ReferenceNumber")).Trim()
                        Dim _fyIdVal = Convert.ToInt32(_s1r("FiscalYearID"))
                        If Not String.IsNullOrEmpty(_rn) AndAlso Not sanad1FyLookup.ContainsKey(_rn) Then
                            sanad1FyLookup(_rn) = _fyIdVal
                        End If
                    Next
                Catch
                End Try
            End If

            Dim sn2EntryMap As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)

            Dim existingDt = Sql.ExecuteTable("SELECT AccountCode, AccountID, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", _companyId)
            Dim codeToIds As New Dictionary(Of String, List(Of Integer))(StringComparer.OrdinalIgnoreCase)
            Dim idToParent As New Dictionary(Of Integer, Integer)()

            For Each row As DataRow In existingDt.Rows
                Dim code = NormalizeCode(Convert.ToString(row("AccountCode")))
                Dim aId = Convert.ToInt32(row("AccountID"))
                Dim pId = If(row("ParentAccountID") Is DBNull.Value, 0, Convert.ToInt32(row("ParentAccountID")))

                If Not codeToIds.ContainsKey(code) Then codeToIds(code) = New List(Of Integer)()
                codeToIds(code).Add(aId)
                idToParent(aId) = pId
            Next

            Dim inserted As Integer = 0
            Dim lineNum As Integer = 0

            Dim s2TotalRows = _sanad2Rows.Count
            pbProgressDetail.Minimum = 0
            pbProgressDetail.Maximum = Math.Max(1, s2TotalRows)
            pbProgressDetail.Value = 0
            lblProgressStatus.Text = String.Format("در حال بررسی تکراری بودن شماره سندها... ({0} ردیف)", s2TotalRows)
            Application.DoEvents()

            Dim lastYear As String = "-"
            Dim lastMonthName As String = "-"

            For i = 0 To _sanad2Rows.Count - 1
                If i = hIdx Then Continue For
                Dim row = _sanad2Rows(i)
                If row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then Continue For

                If i Mod 10 = 0 Then
                    pbProgressDetail.Value = Math.Min(pbProgressDetail.Maximum, i)
                    lblProgressStatus.Text = String.Format("در حال تبدیل آرتیکل‌ها (سند 2): سال {0} ‒ ردیف {1} از {2} ‒ درج شده: {3}", lastYear, i, s2TotalRows, inserted)
                    Application.DoEvents()
                End If

                Dim shomareSanad = GetCell(row, idxShSanad)
                Dim goruhCode = GetCell(row, idxGoruh)
                Dim kolCode = GetCell(row, idxKol)
                Dim moeinCode = GetCell(row, idxMoein)
                Dim tafs1Code = GetCell(row, idxTafs1)
                Dim tafs2Code = GetCell(row, idxTafs2)
                Dim tafs3Code = GetCell(row, idxTafs3)
                Dim bedehStr = GetCell(row, idxBedeh)
                Dim bestanStr = GetCell(row, idxBestan)
                Dim txNum = GetCell(row, idxTxNum)
                Dim txDate = GetCell(row, idxTxDate)
                Dim sharhRadif = If(idxSharhRadif >= 0, GetCell(row, idxSharhRadif), "")

                Dim providedCodes As New List(Of String)()
                If Not String.IsNullOrEmpty(tafs3Code) Then providedCodes.Add(NormalizeCode(tafs3Code))
                If Not String.IsNullOrEmpty(tafs2Code) Then providedCodes.Add(NormalizeCode(tafs2Code))
                If Not String.IsNullOrEmpty(tafs1Code) Then providedCodes.Add(NormalizeCode(tafs1Code))
                If Not String.IsNullOrEmpty(moeinCode) Then providedCodes.Add(NormalizeCode(moeinCode))
                If Not String.IsNullOrEmpty(kolCode) Then providedCodes.Add(NormalizeCode(kolCode))
                If Not String.IsNullOrEmpty(goruhCode) Then providedCodes.Add(NormalizeCode(goruhCode))

                Dim accountId As Integer = 0
                If providedCodes.Count > 0 Then
                    Dim deepestCode = providedCodes(0)
                    If codeToIds.ContainsKey(deepestCode) Then
                        Dim candidates = codeToIds(deepestCode)
                        If candidates.Count = 1 Then
                            accountId = candidates(0)
                        Else
                            For Each cand In candidates
                                Dim currId = cand
                                Dim isMatch = True
                                For p = 1 To providedCodes.Count - 1
                                    Dim parentCodeExpected = providedCodes(p)
                                    Dim actualParentId = If(idToParent.ContainsKey(currId) AndAlso idToParent(currId) > 0, idToParent(currId), 0)
                                    If actualParentId = 0 Then
                                        isMatch = False
                                        Exit For
                                    End If
                                    If Not codeToIds.ContainsKey(parentCodeExpected) OrElse Not codeToIds(parentCodeExpected).Contains(actualParentId) Then
                                        isMatch = False
                                        Exit For
                                    End If
                                    currId = actualParentId
                                Next
                                If isMatch Then
                                    accountId = cand
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If

                If accountId = 0 Then Continue For

                Dim rowFyId As Object = DBNull.Value
                Dim resolvedYear As Integer = 0

                ' ── مرحله اول: شماره سند در سند2 تکراری نیست → سال مالی را از تیبل سند1 بخوان ──
                If Not hasDuplicateDocNums AndAlso Not String.IsNullOrEmpty(shomareSanad) Then
                    If sanad1FyLookup.ContainsKey(shomareSanad) Then
                        rowFyId = sanad1FyLookup(shomareSanad)
                    End If
                End If

                ' ── مرحله دوم: استفاده از ستون «تاریخ سند» (نه تاریخ تراکنش) در فایل اکسل سند2 ──
                If rowFyId Is DBNull.Value Then
                    Dim tarikSanadCell = If(idxTarikSanad >= 0, GetCell(row, idxTarikSanad), "")
                    If Not String.IsNullOrEmpty(tarikSanadCell) Then
                        ' سعی در پارس تاریخ شمسی
                        Dim parsedDt2 = PersianDateHelper.ParsePersianDate(tarikSanadCell)
                        If parsedDt2.HasValue Then
                            Dim pc2 As New System.Globalization.PersianCalendar()
                            resolvedYear = pc2.GetYear(parsedDt2.Value)
                        Else
                            ' سعی در پارس تاریخ میلادی
                            Dim dt2 As DateTime
                            If DateTime.TryParse(tarikSanadCell, dt2) Then
                                Dim pc2 As New System.Globalization.PersianCalendar()
                                resolvedYear = pc2.GetYear(dt2)
                            End If
                        End If
                        ' استخراج مستقیم سال ۴ رقمی از رشته تاریخ (مثل ۱۳۹۵/۰۱/۰۱)
                        If resolvedYear = 0 Then
                            Dim parts2 = tarikSanadCell.Replace("-"c, "/"c).Split("/"c)
                            For Each part2 In parts2
                                Dim y2 As Integer
                                If part2.Trim().Length = 4 AndAlso Integer.TryParse(part2.Trim(), y2) AndAlso y2 >= 1300 AndAlso y2 <= 1500 Then
                                    resolvedYear = y2
                                    Exit For
                                End If
                            Next
                        End If
                        ' اگر هنوز یافت نشد، ۴ رقم اول رشته را بررسی کن
                        If resolvedYear = 0 AndAlso tarikSanadCell.Length >= 4 Then
                            Dim first4 As Integer
                            If Integer.TryParse(tarikSanadCell.Substring(0, 4), first4) AndAlso first4 >= 1300 AndAlso first4 <= 1500 Then
                                resolvedYear = first4
                            End If
                        End If
                    End If
                End If

                ' ── مرحله سوم: اسکن هوشمند تک‌تک سلول‌های ردیف برای یافتن سال شمسی ──
                If rowFyId Is DBNull.Value AndAlso resolvedYear = 0 Then
                    For colIdx = 0 To row.Length - 1
                        Dim val = GetCell(row, colIdx)
                        If Not String.IsNullOrEmpty(val) Then
                            Dim y As Integer
                            ' عدد دقیقاً ۴ رقمی که با ۱۳ یا ۱۴ شروع می‌شود (مثل ۱۳۹۵)
                            If val.Length = 4 AndAlso Integer.TryParse(val, y) AndAlso y >= 1300 AndAlso y <= 1500 Then
                                resolvedYear = y
                                Exit For
                            End If
                            ' عدد ۸ رقمی شمسی (مثل ۱۳۹۵۰۱۰۱)
                            If val.Length = 8 AndAlso Integer.TryParse(val.Substring(0, 4), y) AndAlso y >= 1300 AndAlso y <= 1500 Then
                                resolvedYear = y
                                Exit For
                            End If
                            ' فرمت تاریخ شمسی با جداکننده / یا -
                            If val.Contains("/") OrElse val.Contains("-") Then
                                Dim parts = val.Replace("-"c, "/"c).Split("/"c)
                                If parts.Length >= 1 AndAlso parts(0).Trim().Length = 4 AndAlso
                                   Integer.TryParse(parts(0).Trim(), y) AndAlso y >= 1300 AndAlso y <= 1500 Then
                                    resolvedYear = y
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                ' یافتن یا ایجاد سال مالی در دیتابیس بر اساس resolvedYear
                If rowFyId Is DBNull.Value AndAlso resolvedYear > 0 Then
                    Dim existingFy = Sql.ExecuteScalar("SELECT FiscalYearID FROM FiscalYears WHERE CompanyID = ? AND FiscalYearName = ?", _companyId, resolvedYear.ToString())
                    If existingFy IsNot Nothing AndAlso Not Convert.IsDBNull(existingFy) Then
                        rowFyId = Convert.ToInt32(existingFy)
                    Else
                        ' ایجاد سال مالی جدید به صورت خودکار
                        Dim pc As New System.Globalization.PersianCalendar()
                        Try
                            Dim startDay As Integer = 1
                            Dim startMonth As Integer = 1
                            Dim endMonth As Integer = 12
                            Dim endDay As Integer = If(pc.IsLeapYear(resolvedYear), 30, 29)

                            Dim startDateObj = pc.ToDateTime(resolvedYear, startMonth, startDay, 0, 0, 0, 0)
                            Dim endDateObj = pc.ToDateTime(resolvedYear, endMonth, endDay, 0, 0, 0, 0)

                            rowFyId = Sql.ExecuteIdentity(
                                "INSERT INTO FiscalYears (CompanyID, FiscalYearName, StartDate, EndDate, IsActive) VALUES (?, ?, ?, ?, 1)",
                                _companyId, resolvedYear.ToString(), startDateObj, endDateObj)
                        Catch
                        End Try
                    End If
                End If

                ' Fallback: سال مالی بر اساس maxDate (آخرین تاریخ سند1)
                If rowFyId Is DBNull.Value Then
                    Dim pc As New System.Globalization.PersianCalendar()
                    Try
                        Dim year = pc.GetYear(maxDate)
                        Dim existingFy = Sql.ExecuteScalar("SELECT FiscalYearID FROM FiscalYears WHERE CompanyID = ? AND FiscalYearName = ?", _companyId, year.ToString())
                        If existingFy IsNot Nothing AndAlso Not Convert.IsDBNull(existingFy) Then
                            rowFyId = Convert.ToInt32(existingFy)
                        End If
                    Catch
                    End Try
                End If

                ' Fallback نهایی: آخرین سال مالی موجود در دیتابیس
                If rowFyId Is DBNull.Value Then
                    Dim defFy = Sql.ExecuteScalar("SELECT FiscalYearID FROM FiscalYears WHERE CompanyID = ? ORDER BY StartDate DESC LIMIT 1", _companyId)
                    If defFy IsNot Nothing AndAlso Not Convert.IsDBNull(defFy) Then
                        rowFyId = Convert.ToInt32(defFy)
                    End If
                End If

                ' Update progress display based on resolvedYear/maxDate
                Try
                    Dim pc As New System.Globalization.PersianCalendar()
                    Dim showYear = If(resolvedYear > 0, resolvedYear, pc.GetYear(maxDate))
                    lastYear = showYear.ToString()
                    lastMonthName = "تفکیکی"
                Catch
                End Try

                Dim entryId As Integer = 0
                Dim mapKey = rowFyId.ToString() & "_" & shomareSanad
                If Not String.IsNullOrEmpty(shomareSanad) Then
                    If sn2EntryMap.ContainsKey(mapKey) Then
                        entryId = sn2EntryMap(mapKey)
                    Else
                        Dim eid = Sql.ExecuteScalar(
                            "SELECT EntryID FROM Sanad1 WHERE CompanyID = ? AND ReferenceNumber = ? AND FiscalYearID = ? ORDER BY EntryID LIMIT 1",
                            _companyId, shomareSanad, rowFyId)
                        If eid IsNot Nothing AndAlso Not Convert.IsDBNull(eid) Then
                            entryId = Convert.ToInt32(eid)
                            sn2EntryMap(mapKey) = entryId
                        End If
                    End If
                End If

                If entryId = 0 AndAlso Not String.IsNullOrEmpty(shomareSanad) Then
                    Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)
                    Dim dummyDesc = "اين سند ، در سند1 معادل نداشت و بلا تکليف بود"

                    entryId = Sql.ExecuteIdentity(
                        "INSERT INTO Sanad1 (CompanyID, FiscalYearID, EntryDate, Description, ReferenceNumber, CreatedBy, JamBedehkar, JamBestankar, TaeazSanad, SharhSanad, VazeiatSanad, AdamVirayesh) " &
                        "VALUES (?, ?, ?, ?, ?, ?, 0, 0, 'ناتراز', ?, ?, 0)",
                        _companyId, rowFyId, maxDate, dummyDesc, shomareSanad, currentUserId, dummyDesc, "پيش‌نويس")

                    sn2EntryMap(mapKey) = entryId
                End If

                Dim bedeh As Decimal = 0
                Dim bestan As Decimal = 0
                Decimal.TryParse(bedehStr.Replace(",", "").Replace("،", ""), bedeh)
                Decimal.TryParse(bestanStr.Replace(",", "").Replace("،", ""), bestan)

                lineNum += 1
                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad2 (EntryID, AccountID, DebitAmount, CreditAmount, LineNumber, ShenavarID, SharhRadif, TransactionNumber, TransactionDate) " &
                    "VALUES (?, ?, ?, ?, ?, NULL, ?, ?, ?)",
                    entryId, accountId, bedeh, bestan, lineNum, sharhRadif, txNum, txDate)

                inserted += 1
            Next

            pbProgressDetail.Value = pbProgressDetail.Maximum
            Application.DoEvents()
            Return inserted
        End Function

        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
        ' SANAD 1
        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ

        Private Sub btnSanad1SelectFile_Click(sender As Object, e As EventArgs) Handles btnSanad1SelectFile.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "همه فایل‌های پشتیبانی شده (*.xlsx;*.xls;*.csv;*.txt)|*.xlsx;*.xls;*.csv;*.txt|اکسل (*.xlsx;*.xls)|*.xlsx;*.xls|CSV و متنی (*.csv;*.txt)|*.csv;*.txt|همه فایل‌ها (*.*)|*.*"
                ofd.Title = "انتخاب فایل سند 1"
                If ofd.ShowDialog() <> DialogResult.OK Then Return
                _sanad1FilePath = ofd.FileName
                lblSanad1File.Text = Path.GetFileName(_sanad1FilePath)
                LoadSanad1File()
            End Using
        End Sub

        Private Sub LoadSanad1File()
            If String.IsNullOrEmpty(_sanad1FilePath) Then Return
            Try
                Dim ext = Path.GetExtension(_sanad1FilePath).ToLower()
                _sanad1Rows.Clear()
                If ext = ".xlsx" OrElse ext = ".xls" Then
                    Dim rows = ReadExcelToRows(_sanad1FilePath, ext)
                    If rows.Count = 0 Then
                        MessageBox.Show("فایل اکسل خالی است یا شیت‌ی یافت نشد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    _sanad1Rows.AddRange(rows)
                Else
                    Dim lines = File.ReadAllLines(_sanad1FilePath, Encoding.UTF8)
                    Dim sep = DetectSeparator(lines(0))
                    For Each line In lines
                        _sanad1Rows.Add(line.Split(sep))
                    Next
                End If
                UpdateSanad1ColumnMappings()
                lblSanad1RecordCount.Text = "تعداد رکوردهای فایل اکسل سند 1: 0"
                dgvSanad1Preview.DataSource = Nothing
                CheckAndEnableConvertButton()
            Catch ex As Exception
                MessageBox.Show("خطا در خواندن فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub nudSanad1HeaderRow_ValueChanged(sender As Object, e As EventArgs)
            UpdateSanad1ColumnMappings()
            lblSanad1RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 1: 0"
            dgvSanad1Preview.DataSource = Nothing
            CheckAndEnableConvertButton()
        End Sub

        Private Sub UpdateSanad1ColumnMappings()
            lblSanad1RecordCount.Text = "تعداد رکوردهاي فايل اکسل سند 1: 0"
            dgvSanad1Preview.DataSource = Nothing
            CheckAndEnableConvertButton()
            If _sanad1Rows.Count = 0 Then Return
            Dim hIdx = CInt(nudSanad1HeaderRow.Value) - 1
            If hIdx >= _sanad1Rows.Count Then Return
            _sanad1Columns = _sanad1Rows(hIdx).ToList()
            FillMappingCombos(_sanad1MappingCombos, _sanad1Columns)
        End Sub

        Private _s1Refreshing As Boolean = False
        Private Sub Sanad1MappingCombo_Changed(sender As Object, e As EventArgs)
            If _s1Refreshing Then Return
            RefreshExclusivity(_sanad1MappingCombos, _sanad1Columns, _s1Refreshing)
        End Sub

        Private Sub btnSanad1Preview_Click(sender As Object, e As EventArgs) Handles btnSanad1Preview.Click
            If _sanad1Rows.Count = 0 Then
                MessageBox.Show("ابتدا فایل سند 1 را انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim headerIdx = CInt(nudSanad1HeaderRow.Value) - 1
            LoadPreview(dgvSanad1Preview, _sanad1Rows, headerIdx)

            Dim realCount As Integer = 0
            For i = 0 To _sanad1Rows.Count - 1
                If i = headerIdx Then Continue For
                Dim row = _sanad1Rows(i)
                If Not row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then
                    realCount += 1
                End If
            Next
            lblSanad1RecordCount.Text = "تعداد رکوردهای فایل اکسل سند 1: " & realCount.ToString()

            CheckAndEnableConvertButton()
        End Sub



        Private Function ConvertSanad1() As Integer
            If _deleteExisting AndAlso Not _hasDeleted Then
                Sql.ExecuteNonQuery("DELETE FROM Sanad2 WHERE EntryID = 0")
                Sql.ExecuteNonQuery("DELETE FROM Sanad2 WHERE EntryID IN (SELECT EntryID FROM Sanad1 WHERE CompanyID = ?)", _companyId)
                Sql.ExecuteNonQuery("DELETE FROM Sanad1 WHERE CompanyID = ?", _companyId)
                _hasDeleted = True
            End If

            Dim hIdx = CInt(nudSanad1HeaderRow.Value) - 1
            Dim idxShSanad = GetColIdx(cmbSanad1ShomareSanad, _sanad1Columns)
            Dim idxTarikh = GetColIdx(cmbSanad1TarikSanad, _sanad1Columns)
            Dim idxSharh = GetColIdx(cmbSanad1Sharh, _sanad1Columns)

            Dim inserted As Integer = 0
            Dim currentUserId As Integer = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 0)

            Dim s1TotalRows = _sanad1Rows.Count
            pbProgressDetail.Minimum = 0
            pbProgressDetail.Maximum = Math.Max(1, s1TotalRows)
            pbProgressDetail.Value = 0
            lblProgressStatus.Text = String.Format("آماده‌سازی تبدیل سرسندها (سند 1): {0} ردیف در صفحه...", s1TotalRows)
            Application.DoEvents()

            Dim lastYear As String = "-"
            Dim lastMonthName As String = "-"

            For i = 0 To _sanad1Rows.Count - 1
                If i = hIdx Then Continue For
                Dim row = _sanad1Rows(i)
                If row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then Continue For

                Dim shomareSanad = GetCell(row, idxShSanad)
                Dim tarikSanad = GetCell(row, idxTarikh)
                Dim sharhSanad = GetCell(row, idxSharh)

                If String.IsNullOrEmpty(shomareSanad) Then Continue For

                Dim entryDate As Object = DBNull.Value
                If Not String.IsNullOrEmpty(tarikSanad) Then
                    Dim dtParsed = PersianDateHelper.ParsePersianDate(tarikSanad)
                    If dtParsed.HasValue Then
                        entryDate = dtParsed.Value
                    Else
                        Dim dt2 As DateTime
                        If DateTime.TryParse(tarikSanad, dt2) Then entryDate = dt2
                    End If
                End If

                Dim fyId As Object = DBNull.Value
                If entryDate IsNot DBNull.Value Then
                    Dim dt As DateTime = Convert.ToDateTime(entryDate)
                    Dim pc As New System.Globalization.PersianCalendar()
                    Try
                        Dim year = pc.GetYear(dt)
                        Dim month = pc.GetMonth(dt)
                        lastYear = year.ToString()
                        lastMonthName = GetPersianMonthName(month)
                        pbProgressDetail.Value = Math.Min(12, Math.Max(1, month))

                        Dim existingFy = Sql.ExecuteScalar("SELECT FiscalYearID FROM FiscalYears WHERE CompanyID = ? AND FiscalYearName = ?", _companyId, year.ToString())
                        If existingFy IsNot Nothing AndAlso Not Convert.IsDBNull(existingFy) Then
                            fyId = Convert.ToInt32(existingFy)
                        Else
                            Dim startDay As Integer = 1
                            Dim startMonth As Integer = 1
                            Dim endMonth As Integer = 12
                            Dim endDay As Integer = If(pc.IsLeapYear(year), 30, 29)

                            Dim startDateObj = pc.ToDateTime(year, startMonth, startDay, 0, 0, 0, 0)
                            Dim endDateObj = pc.ToDateTime(year, endMonth, endDay, 0, 0, 0, 0)

                            fyId = Sql.ExecuteIdentity(
                                "INSERT INTO FiscalYears (CompanyID, FiscalYearName, StartDate, EndDate, IsActive) VALUES (?, ?, ?, ?, 1)",
                                _companyId, year.ToString(), startDateObj, endDateObj)
                        End If
                    Catch ex As Exception
                    End Try
                End If

                If i Mod 10 = 0 Then
                    pbProgressDetail.Value = Math.Min(pbProgressDetail.Maximum, i)
                    lblProgressStatus.Text = String.Format("در حال تبدیل سرسندها (سند 1): {0} {1} ‒ ردیف {2} از {3} ‒ درج شده: {4}", lastMonthName, lastYear, i, s1TotalRows, inserted)
                    Application.DoEvents()
                End If

                If fyId Is DBNull.Value Then
                    Dim defFy = Sql.ExecuteScalar("SELECT FiscalYearID FROM FiscalYears WHERE CompanyID = ? ORDER BY StartDate DESC LIMIT 1", _companyId)
                    If defFy IsNot Nothing AndAlso Not Convert.IsDBNull(defFy) Then
                        fyId = Convert.ToInt32(defFy)
                    End If
                End If

                Dim desc = If(String.IsNullOrEmpty(sharhSanad), "تبديل اطلاعات سند 1", sharhSanad)

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, FiscalYearID, EntryDate, Description, ReferenceNumber, CreatedBy, JamBedehkar, JamBestankar, TaeazSanad, SharhSanad, VazeiatSanad, AdamVirayesh) " &
                    "VALUES (?, ?, ?, ?, ?, ?, 0, 0, 'ناتراز', ?, 'پيش‌نويس', 0)",
                    _companyId, fyId, If(entryDate Is DBNull.Value, DateTime.Today, entryDate), desc, shomareSanad, currentUserId, desc)

                inserted += 1
            Next

            pbProgressDetail.Value = pbProgressDetail.Maximum
            Application.DoEvents()
            Return inserted
        End Function

        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
        ' SHARED HELPERS
        ' â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ

        ' â”€â”€ ط®ظˆط§ظ†ط¯ظ† ظپط§غŒظ„ ط§ع©ط³ظ„ ط¨ط§ OleDb ظˆ طھط¨ط¯غŒظ„ ط¨ظ‡ List(Of String()) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        Private Function ReadExcelToRows(filePath As String, ext As String) As List(Of String())
            Dim result As New List(Of String())()
            Dim tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() & ext)
            Try
                File.Copy(filePath, tempPath, True)
            Catch ex As Exception
                tempPath = filePath
            End Try

            Dim connString As String
            If ext = ".xlsx" Then
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & tempPath & ";Extended Properties='Excel 12.0 Xml;HDR=NO;IMEX=1;';"
            Else
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & tempPath & ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1;';"
            End If

            Try
                Using conn As New OleDbConnection(connString)
                    conn.Open()
                    Dim schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                    If schema Is Nothing OrElse schema.Rows.Count = 0 Then Return result
                    Dim sheetName = schema.Rows(0)("TABLE_NAME").ToString()
                    Using cmd As New OleDbCommand("SELECT * FROM [" & sheetName & "]", conn)
                        Using reader = cmd.ExecuteReader()
                            Dim colCount = reader.FieldCount
                            Do While reader.Read()
                                Dim rowData(colCount - 1) As String
                                For i = 0 To colCount - 1
                                    rowData(i) = If(reader.IsDBNull(i), "", Convert.ToString(reader(i)).Trim())
                                Next
                                result.Add(rowData)
                            Loop
                        End Using
                    End Using
                End Using
            Finally
                If tempPath <> filePath AndAlso File.Exists(tempPath) Then
                    Try
                        File.Delete(tempPath)
                    Catch
                    End Try
                End If
            End Try
            Return result
        End Function

        Private Function DetectSeparator(line As String) As Char
            Dim tabCount = line.Count(Function(c) c = ControlChars.Tab)
            Dim commaCount = line.Count(Function(c) c = ","c)
            Dim semiCount = line.Count(Function(c) c = ";"c)
            If tabCount >= commaCount AndAlso tabCount >= semiCount Then Return ControlChars.Tab
            If semiCount > commaCount Then Return ";"c
            Return ","c
        End Function

        Private Sub FillMappingCombos(combos As ComboBox(), columns As List(Of String))
            For Each cmb In combos
                Dim prev = If(cmb.SelectedItem IsNot Nothing, cmb.SelectedItem.ToString(), "")
                cmb.Items.Clear()
                cmb.Items.Add("(انتخاب نشده)")
                For Each col In columns
                    cmb.Items.Add(col)
                Next
                If Not String.IsNullOrEmpty(prev) AndAlso cmb.Items.Contains(prev) Then
                    cmb.SelectedItem = prev
                Else
                    cmb.SelectedIndex = 0
                End If
            Next
        End Sub

        Private Sub RefreshExclusivity(combos As ComboBox(), columns As List(Of String), ByRef flag As Boolean)
            flag = True
            Try
                Dim selected As New Dictionary(Of ComboBox, String)()
                For Each cmb In combos
                    selected(cmb) = If(cmb.SelectedItem IsNot Nothing AndAlso cmb.SelectedItem.ToString() <> "(انتخاب نشده)", cmb.SelectedItem.ToString(), "")
                Next

                Dim baseItems As New List(Of String)() From {"(انتخاب نشده)"}
                baseItems.AddRange(columns)

                For Each cmb In combos
                    Dim cur = selected(cmb)
                    Dim usedByOthers = (From kvp In selected Where kvp.Key IsNot cmb AndAlso kvp.Value <> "" Select kvp.Value).ToList()
                    Dim avail = baseItems.Where(Function(x) x = "(انتخاب نشده)" OrElse x = cur OrElse Not usedByOthers.Contains(x)).ToList()
                    cmb.Items.Clear()
                    cmb.Items.AddRange(avail.ToArray())
                    If Not String.IsNullOrEmpty(cur) AndAlso avail.Contains(cur) Then
                        cmb.SelectedItem = cur
                    Else
                        cmb.SelectedIndex = 0
                    End If
                Next
            Finally
                flag = False
            End Try
        End Sub

        Private Function GetColIdx(cmb As ComboBox, columns As List(Of String)) As Integer
            If cmb.SelectedItem Is Nothing OrElse cmb.SelectedItem.ToString() = "(انتخاب نشده)" Then Return -1
            Return columns.IndexOf(cmb.SelectedItem.ToString())
        End Function

        Private Function GetCell(row As String(), idx As Integer) As String
            If idx < 0 OrElse idx >= row.Length Then Return ""
            Return row(idx).Trim()
        End Function

        Private Function NormalizeCode(code As String) As String
            If String.IsNullOrEmpty(code) Then Return ""
            Dim trimmed = code.Trim()
            Dim startIdx = 0
            While startIdx < trimmed.Length AndAlso trimmed(startIdx) = "0"c
                startIdx += 1
            End While
            If startIdx = trimmed.Length Then Return "0"
            Return trimmed.Substring(startIdx)
        End Function

        Private Sub LoadPreview(dgv As DataGridView, rows As List(Of String()), headerRowIndex As Integer)
            dgv.Columns.Clear()
            dgv.Rows.Clear()

            If rows.Count = 0 OrElse headerRowIndex >= rows.Count Then Return

            Dim headers = rows(headerRowIndex)
            For Each col In headers
                dgv.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = col, .ReadOnly = True})
            Next

            Dim maxRows = Math.Min(rows.Count, 200)
            For i = 0 To maxRows - 1
                If i = headerRowIndex Then Continue For
                Dim row = rows(i)
                Dim cellData(headers.Length - 1) As Object
                For j = 0 To headers.Length - 1
                    cellData(j) = If(j < row.Length, row(j), "")
                Next
                dgv.Rows.Add(cellData)
            Next
        End Sub

        Private Sub CheckAndEnableConvertButton()
            btnConvertBoth.Enabled = (_sanad1Rows.Count > 0 AndAlso _sanad2Rows.Count > 0 AndAlso dgvSanad1Preview.Rows.Count > 0 AndAlso dgvSanad2Preview.Rows.Count > 0)
        End Sub

        Private Sub btnConvertBoth_Click(sender As Object, e As EventArgs) Handles btnConvertBoth.Click
            InitializeProgressOverlay()
            If _sanad1Rows.Count = 0 OrElse _sanad2Rows.Count = 0 Then
                MessageBox.Show("ابتدا فايل‌هاي سند 1 و سند 2 را انتخاب کنيد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If GetColIdx(cmbSanad1ShomareSanad, _sanad1Columns) < 0 Then
                MessageBox.Show("لطفاً ستون «شماره سند» را در سند 1 انتخاب کنيد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If GetColIdx(cmbSanad2ShomareSanad, _sanad2Columns) < 0 AndAlso
               GetColIdx(cmbSanad2Goruh, _sanad2Columns) < 0 AndAlso
               GetColIdx(cmbSanad2Moein, _sanad2Columns) < 0 Then
                MessageBox.Show("لطفاً حداقل «شماره سند» يا يکي از ستون‌هاي حساب را در سند 2 انتخاب کنيد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim ans = MessageBox.Show("آيا از تبديل همزمان اسناد 1 و 2 اطمينان داريد؟", "تأييد", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ans <> DialogResult.Yes Then Return

            ' Show Progress overlay
            pnlProgressOverlay.Size = Me.ClientSize
            pnlProgressOverlay.Location = New System.Drawing.Point(0, 0)
            pnlProgressBox.Location = New System.Drawing.Point((pnlProgressOverlay.Width - pnlProgressBox.Width) \ 2, (pnlProgressOverlay.Height - pnlProgressBox.Height) \ 2)
            pnlProgressOverlay.Visible = True
            pnlProgressOverlay.BringToFront()
            Application.DoEvents()

            Try
                _hasDeleted = False

                pbProgressOverall.Minimum = 0
                pbProgressOverall.Maximum = 100
                pbProgressOverall.Value = 10
                lblProgressStatus.Text = "در حال تحليل و آماده‌سازي داده‌ها..."
                Application.DoEvents()

                Dim maxDate As DateTime = DateTime.MinValue
                Dim hIdx1 = CInt(nudSanad1HeaderRow.Value) - 1
                Dim idxTarikh1 = GetColIdx(cmbSanad1TarikSanad, _sanad1Columns)

                For i = 0 To _sanad1Rows.Count - 1
                    If i = hIdx1 Then Continue For
                    Dim row = _sanad1Rows(i)
                    If row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then Continue For

                    Dim tarikSanad = GetCell(row, idxTarikh1)
                    If Not String.IsNullOrEmpty(tarikSanad) Then
                        Dim dtParsed = PersianDateHelper.ParsePersianDate(tarikSanad)
                        Dim dtVal As DateTime? = Nothing
                        If dtParsed.HasValue Then
                            dtVal = dtParsed.Value
                        Else
                            Dim dt2 As DateTime
                            If DateTime.TryParse(tarikSanad, dt2) Then dtVal = dt2
                        End If

                        If dtVal.HasValue AndAlso dtVal.Value > maxDate Then
                            maxDate = dtVal.Value
                        End If
                    End If
                Next

                If maxDate = DateTime.MinValue Then
                    maxDate = DateTime.Today
                End If

                pbProgressOverall.Value = 20
                Application.DoEvents()

                Dim s1Inserted = ConvertSanad1()

                pbProgressOverall.Value = 50
                Application.DoEvents()

                Dim s2Inserted = ConvertSanad2(maxDate)

                pbProgressOverall.Value = 80
                lblProgressStatus.Text = "در حال ايجاد آرتيکل‌هاي فرضي براي سندهاي بدون جزئيات..."
                pbProgressDetail.Minimum = 0
                pbProgressDetail.Maximum = 100
                pbProgressDetail.Value = 0
                Application.DoEvents()

                Dim dummyGroupId As Integer = 0
                Dim dummyKolId As Integer = 0

                Dim gId = Sql.ExecuteScalar("SELECT AccountID FROM SarfaslHesab WHERE CompanyID = ? AND AccountType = 'گروه' AND AccountName = 'حساب انتظامي اسناد بلا تکليف'", _companyId)
                If gId IsNot Nothing AndAlso Not Convert.IsDBNull(gId) Then
                    dummyGroupId = Convert.ToInt32(gId)
                Else
                    Dim groupLenObj = Sql.ExecuteScalar("SELECT Level1Length FROM Companies WHERE CompanyID = ?", _companyId)
                    Dim groupLen As Integer = 1
                    If groupLenObj IsNot Nothing AndAlso Not Convert.IsDBNull(groupLenObj) Then
                        groupLen = Convert.ToInt32(groupLenObj)
                    End If
                    Dim maxGroupObj = Sql.ExecuteScalar("SELECT MAX(CAST(AccountCode AS INTEGER)) FROM SarfaslHesab WHERE CompanyID = ? AND AccountType = 'گروه'", _companyId)
                    Dim nextGroupInt As Integer = 1
                    If maxGroupObj IsNot Nothing AndAlso Not Convert.IsDBNull(maxGroupObj) Then
                        nextGroupInt = Convert.ToInt32(maxGroupObj) + 1
                    End If
                    Dim nextGroupCode = nextGroupInt.ToString().PadLeft(groupLen, "0"c)

                    dummyGroupId = Sql.ExecuteIdentity(
                        "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, 'گروه', NULL, 1)",
                        _companyId, nextGroupCode, "حساب انتظامي اسناد بلا تکليف")
                End If

                Dim kId = Sql.ExecuteScalar("SELECT AccountID FROM SarfaslHesab WHERE CompanyID = ? AND AccountType = 'کل' AND AccountName = 'اسناد موجود در سند1 و فاقد آرتيل در سند2' AND ParentAccountID = ?", _companyId, dummyGroupId)
                If kId IsNot Nothing AndAlso Not Convert.IsDBNull(kId) Then
                    dummyKolId = Convert.ToInt32(kId)
                Else
                    Dim kolLenObj = Sql.ExecuteScalar("SELECT Level2Length FROM Companies WHERE CompanyID = ?", _companyId)
                    Dim kolLen As Integer = 2
                    If kolLenObj IsNot Nothing AndAlso Not Convert.IsDBNull(kolLenObj) Then
                        kolLen = Convert.ToInt32(kolLenObj)
                    End If
                    Dim nextKolCode = "1".PadLeft(kolLen, "0"c)

                    dummyKolId = Sql.ExecuteIdentity(
                        "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, 'کل', ?, 1)",
                        _companyId, nextKolCode, "اسناد موجود در سند1 و فاقد آرتيل در سند2", dummyGroupId)
                End If

                Dim emptySanads = Sql.ExecuteTable(
                    "SELECT EntryID FROM Sanad1 WHERE CompanyID = ? AND EntryID NOT IN (SELECT DISTINCT EntryID FROM Sanad2)",
                    _companyId)

                Dim dummyRowsInserted As Integer = 0
                pbProgressDetail.Minimum = 0
                pbProgressDetail.Maximum = Math.Max(1, emptySanads.Rows.Count)
                pbProgressDetail.Value = 0
                For Each r As DataRow In emptySanads.Rows
                    If dummyRowsInserted Mod 50 = 0 Then
                        pbProgressDetail.Value = dummyRowsInserted
                        lblProgressStatus.Text = String.Format("در حال ايجاد آرتيکل‌هاي فرضي... {0} از {1}", dummyRowsInserted, emptySanads.Rows.Count)
                        Application.DoEvents()
                    End If

                    Dim eid = Convert.ToInt32(r("EntryID"))

                    Sql.ExecuteNonQuery(
                        "INSERT INTO Sanad2 (EntryID, AccountID, DebitAmount, CreditAmount, LineNumber, ShenavarID, SharhRadif, TransactionNumber, TransactionDate) " &
                        "VALUES (?, ?, 1, 0, 1, NULL, 'اين آرتيل فرضي ايجاد شده است', '', '')",
                        eid, dummyKolId)
                    dummyRowsInserted += 1
                Next

                pbProgressOverall.Value = 90
                lblProgressStatus.Text = "در حال موازنه و محاسبه تراز اسناد..."
                pbProgressDetail.Minimum = 0
                pbProgressDetail.Maximum = 100
                pbProgressDetail.Value = 0
                Application.DoEvents()

                Sql.ExecuteNonQuery(
                    "UPDATE Sanad1 SET " &
                    "JamBedehkar = (SELECT COALESCE(SUM(DebitAmount), 0) FROM Sanad2 WHERE Sanad2.EntryID = Sanad1.EntryID), " &
                    "JamBestankar = (SELECT COALESCE(SUM(CreditAmount), 0) FROM Sanad2 WHERE Sanad2.EntryID = Sanad1.EntryID) " &
                    "WHERE CompanyID = ?", _companyId)

                Sql.ExecuteNonQuery(
                    "UPDATE Sanad1 SET " &
                    "TaeazSanad = CASE " &
                    "  WHEN JamBedehkar = JamBestankar THEN 'تراز' " &
                    "  WHEN JamBedehkar > JamBestankar THEN 'بدهکار' " &
                    "  ELSE 'بستانکار' " &
                    "END " &
                    "WHERE CompanyID = ?", _companyId)

                pbProgressOverall.Value = 100
                lblProgressStatus.Text = "عمليات با موفقيت پايان يافت."
                Application.DoEvents()

                pnlProgressOverlay.Visible = False

                Dim msg = String.Format("تبديل با موفقيت انجام شد.{0}تعداد سرسند درج شده (سند 1): {1}{0}تعداد رديف درج شده (سند 2): {2}{0}رديف‌هاي فرضي ايجاد شده براي سندهاي بدون آرتيکل: {3}", Environment.NewLine, s1Inserted, s2Inserted, dummyRowsInserted)
                MessageBox.Show(msg, "عمليات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                pnlProgressOverlay.Visible = False
                MessageBox.Show("خطا در فرآيند تبديل همزمان: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetPersianMonthName(month As Integer) As String
            Select Case month
                Case 1 : Return "فروردين"
                Case 2 : Return "ارديبهشت"
                Case 3 : Return "خرداد"
                Case 4 : Return "تير"
                Case 5 : Return "مرداد"
                Case 6 : Return "شهريور"
                Case 7 : Return "مهر"
                Case 8 : Return "آبان"
                Case 9 : Return "آذر"
                Case 10 : Return "دي"
                Case 11 : Return "بهمن"
                Case 12 : Return "اسفند"
                Case Else : Return ""
            End Select
        End Function

    End Class
End Namespace