Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms

    Public Class HesabdaryTabdilDataSarfaslForm
        Inherits Form

        ' ── Fields ─────────────────────────────────────────────────────────
        ' Progress bars (Runtime only)
        Private pnlProgressOverlay As Panel
        Private pnlProgressBox As Panel
        Private lblProgressOverallTitle As Label
        Private pbProgressOverall As ProgressBar
        Private lblProgressDetailTitle As Label
        Private pbProgressDetail As ProgressBar
        Private lblProgressStatus As Label

        Private _companyId As Integer
        Private _deleteExisting As Boolean
        Private _filePath As String = String.Empty
        Private _allColumns As New List(Of String)()
        Private _fileRows As New List(Of String())()

        ' همه کامبوباکس‌ها با هم
        Private ReadOnly _allMappingCombos As ComboBox()

        ' ── Constructor ─────────────────────────────────────────────────────
        Public Sub New(companyId As Integer, deleteExisting As Boolean)
            InitializeComponent()
            _companyId = companyId
            _deleteExisting = deleteExisting
            _allMappingCombos = New ComboBox() {
                cmbGoruh, cmbKol, cmbMoein, cmbTafsili1, cmbTafsili2, cmbTafsili3, cmbAccountName
            }
        End Sub

        ' ── Load ─────────────────────────────────────────────────────────────
        Private Sub HesabdaryTabdilDataSarfaslForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            InitializeProgressOverlay()
            AddHandler nudHeaderRow.ValueChanged, AddressOf nudHeaderRow_ValueChanged
            For Each cmb In _allMappingCombos
                AddHandler cmb.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            Next
            
            Try
                Dim levelCountObj = Sql.ExecuteScalar("SELECT AccountLevels FROM Companies WHERE CompanyID = ?", _companyId)
                If levelCountObj IsNot Nothing AndAlso Not Convert.IsDBNull(levelCountObj) Then
                    Dim levelCount = Convert.ToInt32(levelCountObj)
                    If levelCount < 3 Then
                        cmbMoein.Visible = False
                        lblMoein.Visible = False
                    End If
                    If levelCount < 4 Then
                        cmbTafsili1.Visible = False
                        lblTafsili1.Visible = False
                    End If
                    If levelCount < 5 Then
                        cmbTafsili2.Visible = False
                        lblTafsili2.Visible = False
                    End If
                    If levelCount < 6 Then
                        cmbTafsili3.Visible = False
                        lblTafsili3.Visible = False
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

        ' ── انتخاب فایل ──────────────────────────────────────────────────────
        Private Sub btnSelectFile_Click(sender As Object, e As EventArgs) Handles btnSelectFile.Click
            Using ofd As New OpenFileDialog()
                ofd.Filter = "همه فایل‌های پشتیبانی شده (*.xlsx;*.xls;*.csv;*.txt)|*.xlsx;*.xls;*.csv;*.txt|اکسل (*.xlsx;*.xls)|*.xlsx;*.xls|CSV و متنی (*.csv;*.txt)|*.csv;*.txt|همه فایل‌ها (*.*)|*.*"
                ofd.Title = "انتخاب فایل سرفصل"
                If ofd.ShowDialog() <> DialogResult.OK Then Return
                _filePath = ofd.FileName
                lblFileStatus.Text = Path.GetFileName(_filePath)
                LoadFileContent()
            End Using
        End Sub

        Private Sub LoadFileContent()
            If String.IsNullOrEmpty(_filePath) Then Return
            Try
                Dim ext = Path.GetExtension(_filePath).ToLower()
                _fileRows.Clear()

                If ext = ".xlsx" OrElse ext = ".xls" Then
                    ' ── خواندن از فایل اکسل ──────────────────────────────
                    Dim rows = ReadExcelToRows(_filePath, ext)
                    If rows.Count = 0 Then
                        MessageBox.Show("فایل اکسل خالی است یا شیت‌ی یافت نشد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    _fileRows.AddRange(rows)
                Else
                    ' ── خواندن از CSV / TXT ──────────────────────────────
                    Dim lines = File.ReadAllLines(_filePath, Encoding.UTF8)
                    If lines.Length = 0 Then
                        MessageBox.Show("فایل خالی است.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    Dim sep = DetectSeparator(lines(0))
                    For Each line In lines
                        _fileRows.Add(line.Split(sep))
                    Next
                End If

                UpdateColumnMappings()

            Catch ex As Exception
                MessageBox.Show("خطا در خواندن فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' ── خواندن اکسل با OleDb ──────────────────────────────────────────
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

        Private Sub nudHeaderRow_ValueChanged(sender As Object, e As EventArgs)
            UpdateColumnMappings()
        End Sub

        ' ── پر کردن کامبوباکس‌ها ─────────────────────────────────────────
        Private Sub UpdateColumnMappings()
            If _fileRows.Count = 0 Then Return
            Dim headerRowIndex = CInt(nudHeaderRow.Value) - 1
            If headerRowIndex >= _fileRows.Count Then Return
            _allColumns = _fileRows(headerRowIndex).ToList()

            ' ذخیره انتخاب‌های فعلی
            Dim saved As New Dictionary(Of ComboBox, String)()
            For Each cmb In _allMappingCombos
                saved(cmb) = If(cmb.SelectedItem IsNot Nothing, cmb.SelectedItem.ToString(), "")
            Next

            ' پر کردن - رویدادها موقتاً غیرفعال
            For Each cmb In _allMappingCombos
                RemoveHandler cmb.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            Next

            Dim baseItems As New List(Of String)() From {"(انتخاب نشده)"}
            baseItems.AddRange(_allColumns)

            For Each cmb In _allMappingCombos
                cmb.Items.Clear()
                cmb.Items.AddRange(baseItems.ToArray())
                Dim prev = saved(cmb)
                If Not String.IsNullOrEmpty(prev) AndAlso baseItems.Contains(prev) Then
                    cmb.SelectedItem = prev
                Else
                    cmb.SelectedIndex = 0
                End If
            Next

            For Each cmb In _allMappingCombos
                AddHandler cmb.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            Next

            RefreshExclusivity()
        End Sub

        ' ── Exclusivity  ──────────────────────────
        Private _refreshing As Boolean = False

        Private Sub Combo_SelectedIndexChanged(sender As Object, e As EventArgs)
            If _refreshing Then Return
            RefreshExclusivity()
        End Sub

        Private Sub RefreshExclusivity()
            _refreshing = True
            Try
                Dim selected As New Dictionary(Of ComboBox, String)()
                For Each cmb In _allMappingCombos
                    selected(cmb) = If(cmb.SelectedItem IsNot Nothing AndAlso cmb.SelectedItem.ToString() <> "(انتخاب نشده)", cmb.SelectedItem.ToString(), "")
                Next

                Dim baseItems As New List(Of String)() From {"(انتخاب نشده)"}
                baseItems.AddRange(_allColumns)

                For Each cmb In _allMappingCombos
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
                _refreshing = False
            End Try
        End Sub

        ' ── پیش‌نمایش ──────────────────────────────────────────────────────
        Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
            If _fileRows.Count = 0 Then
                MessageBox.Show("ابتدا یک فایل انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            LoadPreview()
        End Sub

        Private Sub LoadPreview()
            dgvSarfaslPreview.Columns.Clear()
            dgvSarfaslPreview.Rows.Clear()
            If _fileRows.Count = 0 Then Return
            Dim headerRowIndex = CInt(nudHeaderRow.Value) - 1
            If headerRowIndex >= _fileRows.Count Then Return
            Dim headers = _fileRows(headerRowIndex)
            For Each col In headers
                dgvSarfaslPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = col, .ReadOnly = True})
            Next
            Dim maxRows = Math.Min(_fileRows.Count, 200)
            For i = 0 To maxRows - 1
                If i = headerRowIndex Then Continue For
                Dim row = _fileRows(i)
                Dim cellData(headers.Length - 1) As Object
                For j = 0 To headers.Length - 1
                    cellData(j) = If(j < row.Length, row(j), "")
                Next
                dgvSarfaslPreview.Rows.Add(cellData)
            Next
            btnSmartConvert.Enabled = True
        End Sub

        ' ── تبدیل هوشمند سرفصل ───────────────────────────────────────────────
                Private Sub btnSmartConvert_Click(sender As Object, e As EventArgs) Handles btnSmartConvert.Click
            InitializeProgressOverlay()
            If _fileRows.Count = 0 Then
                MessageBox.Show("ابتدا يک فايل انتخاب کنيد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If GetColIdx(cmbGoruh) < 0 AndAlso GetColIdx(cmbMoein) < 0 AndAlso GetColIdx(cmbTafsili1) < 0 AndAlso GetColIdx(cmbTafsili2) < 0 Then
                MessageBox.Show("لطفاً حداقل يک ستون کد را انتخاب کنيد.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Show Progress overlay
            pnlProgressOverlay.Size = Me.ClientSize
            pnlProgressOverlay.Location = New System.Drawing.Point(0, 0)
            pnlProgressBox.Location = New System.Drawing.Point((pnlProgressOverlay.Width - pnlProgressBox.Width) \ 2, (pnlProgressOverlay.Height - pnlProgressBox.Height) \ 2)
            pnlProgressOverlay.Visible = True
            pnlProgressOverlay.BringToFront()
            Application.DoEvents()

            Try
                pbProgressOverall.Minimum = 0
                pbProgressOverall.Maximum = 100
                pbProgressOverall.Value = 5
                lblProgressStatus.Text = "در حال آماده‌سازي..."
                Application.DoEvents()

                Dim inserted = ConvertSarfasl(_deleteExisting)

                pbProgressOverall.Value = 100
                lblProgressStatus.Text = "عمليات با موفقيت پايان يافت."
                Application.DoEvents()

                pnlProgressOverlay.Visible = False

                MessageBox.Show($"{inserted} سرفصل با موفقيت پردازش شد.", "عمليات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                pnlProgressOverlay.Visible = False
                MessageBox.Show("خطا در تبديل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function GetColIdx(cmb As ComboBox) As Integer
            If cmb.SelectedItem Is Nothing OrElse cmb.SelectedItem.ToString() = "(انتخاب نشده)" Then Return -1
            Return _allColumns.IndexOf(cmb.SelectedItem.ToString())
        End Function

        Private Function GetCell(row As String(), colIndex As Integer) As String
            If colIndex < 0 OrElse colIndex >= row.Length Then Return ""
            Return row(colIndex).Trim()
        End Function

                Private Function ConvertSarfasl(deleteExisting As Boolean) As Integer
            Dim headerRowIndex = CInt(nudHeaderRow.Value) - 1

            ' ?? حذف سرفصل‌هاي قبلي در صورت انتخاب حالت جايگزيني ?????????
            If deleteExisting Then
                Sql.ExecuteNonQuery(
                    "DELETE FROM Sanad2 WHERE EntryID IN (SELECT EntryID FROM Sanad1 WHERE CompanyID = ?)",
                    _companyId)
                Sql.ExecuteNonQuery("DELETE FROM SarfaslHesab WHERE CompanyID = ?", _companyId)
            End If

            Dim idxGoruh = GetColIdx(cmbGoruh)
            Dim idxKol = GetColIdx(cmbKol)
            Dim idxMoein = GetColIdx(cmbMoein)
            Dim idxTafs1 = GetColIdx(cmbTafsili1)
            Dim idxTafs2 = GetColIdx(cmbTafsili2)
            Dim idxTafs3 = GetColIdx(cmbTafsili3)
            Dim idxAccountName = GetColIdx(cmbAccountName)

            Dim existingDt = Sql.ExecuteTable("SELECT AccountCode, AccountID, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", _companyId)
            Dim existingMap As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            For Each row As DataRow In existingDt.Rows
                Dim code = Convert.ToString(row("AccountCode"))
                Dim pId = If(row("ParentAccountID") Is DBNull.Value, 0, Convert.ToInt32(row("ParentAccountID")))
                existingMap(pId.ToString() & "_" & code) = Convert.ToInt32(row("AccountID"))
            Next

            Dim processedCount As Integer = 0

            Dim totalSarfaslRows = _fileRows.Count
            pbProgressDetail.Minimum = 0
            pbProgressDetail.Maximum = Math.Max(1, totalSarfaslRows)
            pbProgressDetail.Value = 0
            lblProgressStatus.Text = String.Format("آماده‌سازی تبدیل سرفصل حسابداری: {0} ردیف در صفحه...", totalSarfaslRows)
            Application.DoEvents()

            For i = 0 To _fileRows.Count - 1
                If i Mod 10 = 0 Then
                    pbProgressDetail.Value = Math.Min(pbProgressDetail.Maximum, i)
                    pbProgressOverall.Value = CInt(10 + (i / Math.Max(1, totalSarfaslRows)) * 90)
                    lblProgressStatus.Text = String.Format("در حال پردازش سرفصل‌ها: ردیف {0} از {1} ‒ درج شده: {2}", i, totalSarfaslRows, processedCount)
                    Application.DoEvents()
                End If

                If i = headerRowIndex Then Continue For
                Dim row = _fileRows(i)
                If row.All(Function(c) String.IsNullOrWhiteSpace(c)) Then Continue For

                Dim goruhCode = GetCell(row, idxGoruh)
                Dim kolCode = GetCell(row, idxKol)
                Dim moeinCode = GetCell(row, idxMoein)
                Dim tafs1Code = GetCell(row, idxTafs1)
                Dim tafs2Code = GetCell(row, idxTafs2)
                Dim tafs3Code = GetCell(row, idxTafs3)
                Dim accountName = GetCell(row, idxAccountName)

                Dim deepestLevel As Integer = 0
                If Not String.IsNullOrEmpty(tafs3Code) Then
                    deepestLevel = 6
                ElseIf Not String.IsNullOrEmpty(tafs2Code) Then
                    deepestLevel = 5
                ElseIf Not String.IsNullOrEmpty(tafs1Code) Then
                    deepestLevel = 4
                ElseIf Not String.IsNullOrEmpty(moeinCode) Then
                    deepestLevel = 3
                ElseIf Not String.IsNullOrEmpty(kolCode) Then
                    deepestLevel = 2
                ElseIf Not String.IsNullOrEmpty(goruhCode) Then
                    deepestLevel = 1
                End If

                If deepestLevel = 0 Then Continue For

                If String.IsNullOrWhiteSpace(accountName) Then
                    If deepestLevel = 6 Then accountName = tafs3Code
                    If deepestLevel = 5 Then accountName = tafs2Code
                    If deepestLevel = 4 Then accountName = tafs1Code
                    If deepestLevel = 3 Then accountName = moeinCode
                    If deepestLevel = 2 Then accountName = kolCode
                    If deepestLevel = 1 Then accountName = goruhCode
                End If

                Dim currentGoruhId As Integer = 0
                If Not String.IsNullOrEmpty(goruhCode) Then
                    Dim dName = If(deepestLevel = 1, accountName, goruhCode)
                    Dim key = "0_" & goruhCode
                    If Not existingMap.ContainsKey(key) Then
                        Dim gId = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, NULL, 1)",
                            _companyId, goruhCode, dName, "گروه")
                        existingMap(key) = gId
                        currentGoruhId = gId
                        processedCount += 1
                    Else
                        currentGoruhId = existingMap(key)
                        If deepestLevel = 1 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentGoruhId)
                        End If
                    End If
                End If

                Dim currentKolId As Integer = 0
                If Not String.IsNullOrEmpty(kolCode) Then
                    Dim dName = If(deepestLevel = 2, accountName, kolCode)
                    Dim pId = currentGoruhId
                    Dim key = pId.ToString() & "_" & kolCode
                    If Not existingMap.ContainsKey(key) Then
                        Dim parentObj As Object = If(pId > 0, CObj(pId), DBNull.Value)
                        Dim kId = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                            _companyId, kolCode, dName, "کل", parentObj)
                        existingMap(key) = kId
                        currentKolId = kId
                        processedCount += 1
                    Else
                        currentKolId = existingMap(key)
                        If deepestLevel = 2 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentKolId)
                        End If
                    End If
                End If

                Dim currentMoeinId As Integer = 0
                If Not String.IsNullOrEmpty(moeinCode) Then
                    Dim dName = If(deepestLevel = 3, accountName, moeinCode)
                    Dim pId = If(currentKolId > 0, currentKolId, currentGoruhId)
                    Dim key = pId.ToString() & "_" & moeinCode
                    If Not existingMap.ContainsKey(key) Then
                        Dim parentObj As Object = If(pId > 0, CObj(pId), DBNull.Value)
                        Dim mId = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                            _companyId, moeinCode, dName, "معين", parentObj)
                        existingMap(key) = mId
                        currentMoeinId = mId
                        processedCount += 1
                    Else
                        currentMoeinId = existingMap(key)
                        If deepestLevel = 3 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentMoeinId)
                        End If
                    End If
                End If

                Dim currentTafs1Id As Integer = 0
                If Not String.IsNullOrEmpty(tafs1Code) Then
                    Dim dName = If(deepestLevel = 4, accountName, tafs1Code)
                    Dim pId = If(currentMoeinId > 0, currentMoeinId, If(currentKolId > 0, currentKolId, currentGoruhId))
                    Dim key = pId.ToString() & "_" & tafs1Code
                    If Not existingMap.ContainsKey(key) Then
                        Dim parentObj As Object = If(pId > 0, CObj(pId), DBNull.Value)
                        Dim t1Id = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                            _companyId, tafs1Code, dName, "تفصيلي", parentObj)
                        existingMap(key) = t1Id
                        currentTafs1Id = t1Id
                        processedCount += 1
                    Else
                        currentTafs1Id = existingMap(key)
                        If deepestLevel = 4 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentTafs1Id)
                        End If
                    End If
                End If

                Dim currentTafs2Id As Integer = 0
                If Not String.IsNullOrEmpty(tafs2Code) Then
                    Dim dName = If(deepestLevel = 5, accountName, tafs2Code)
                    Dim pId = If(currentTafs1Id > 0, currentTafs1Id, If(currentMoeinId > 0, currentMoeinId, If(currentKolId > 0, currentKolId, currentGoruhId)))
                    Dim key = pId.ToString() & "_" & tafs2Code
                    If Not existingMap.ContainsKey(key) Then
                        Dim parentObj As Object = If(pId > 0, CObj(pId), DBNull.Value)
                        Dim t2Id = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                            _companyId, tafs2Code, dName, "تفصيلي", parentObj)
                        existingMap(key) = t2Id
                        currentTafs2Id = t2Id
                        processedCount += 1
                    Else
                        currentTafs2Id = existingMap(key)
                        If deepestLevel = 5 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentTafs2Id)
                        End If
                    End If
                End If

                Dim currentTafs3Id As Integer = 0
                If Not String.IsNullOrEmpty(tafs3Code) Then
                    Dim dName = If(deepestLevel = 6, accountName, tafs3Code)
                    Dim pId = If(currentTafs2Id > 0, currentTafs2Id, If(currentTafs1Id > 0, currentTafs1Id, If(currentMoeinId > 0, currentMoeinId, If(currentKolId > 0, currentKolId, currentGoruhId))))
                    Dim key = pId.ToString() & "_" & tafs3Code
                    If Not existingMap.ContainsKey(key) Then
                        Dim parentObj As Object = If(pId > 0, CObj(pId), DBNull.Value)
                        Dim t3Id = Sql.ExecuteIdentity(
                            "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                            _companyId, tafs3Code, dName, "تفصيلي", parentObj)
                        existingMap(key) = t3Id
                        currentTafs3Id = t3Id
                        processedCount += 1
                    Else
                        currentTafs3Id = existingMap(key)
                        If deepestLevel = 6 Then
                            Sql.ExecuteNonQuery("UPDATE SarfaslHesab SET AccountName = ? WHERE AccountID = ?", dName, currentTafs3Id)
                        End If
                    End If
                End If
            Next

            pbProgressDetail.Value = pbProgressDetail.Maximum
            Application.DoEvents()
            Return processedCount
        End Function

    End Class
End Namespace
