Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Partial Class HesabdarySanad1Form
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private _entriesTable As DataTable
 
        Private Const ColBtnEdit As String = "colBtnEdit"
        Private Const ColBtnDelete As String = "colBtnDelete"
        Private Const ColLock As String = "colAdamVirayesh"

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdarySanad1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            Negar.Business.ThemeHelper.AppendStatusBar(Me)
            If Me.dgvEntries IsNot Nothing Then Me.dgvEntries.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            Me.KeyPreview = True
            SetupGrid()
            InitializeSearchPanel()
            LoadEntries()
            ApplySecurity()
        End Sub

        Public Sub RefreshData()
            LoadEntries()
        End Sub

        Private Sub SetupGrid()
            dgvEntries.AutoGenerateColumns = False
            dgvEntries.Columns.Clear()

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColBtnEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 70
            colEdit.FlatStyle = FlatStyle.Standard
            colEdit.ReadOnly = True

            Dim colDel As New DataGridViewButtonColumn()
            colDel.Name = ColBtnDelete
            colDel.HeaderText = "حذف"
            colDel.Text = "حذف"
            colDel.UseColumnTextForButtonValue = True
            colDel.Width = 56
            colDel.FlatStyle = FlatStyle.Standard
            colDel.ReadOnly = True

            Dim colLockChk As New DataGridViewCheckBoxColumn()
            colLockChk.Name = ColLock
            colLockChk.DataPropertyName = "AdamVirayesh"
            colLockChk.HeaderText = "عدم ویرایش"
            colLockChk.Width = 80
            colLockChk.ReadOnly = False
            colLockChk.Visible = SessionContext.HasPermission("LockSanad1")

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "colEntryID"
            colId.DataPropertyName = "EntryID"
            colId.Visible = False
            colId.ReadOnly = True

            Dim colRef As New DataGridViewTextBoxColumn()
            colRef.Name = "colRef"
            colRef.DataPropertyName = "ReferenceNumber"
            colRef.HeaderText = "شماره سند"
            colRef.Width = 90
            colRef.ReadOnly = True

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "colDate"
            colDate.DataPropertyName = "EntryDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110
            colDate.ReadOnly = True

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "colDesc"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "شرح سند"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colDesc.ReadOnly = True
            colDesc.DefaultCellStyle.WrapMode = DataGridViewTriState.True

            Dim colBed As New DataGridViewTextBoxColumn()
            colBed.Name = "colBed"
            colBed.DataPropertyName = "JamBedehkar"
            colBed.HeaderText = "جمع بدهکار"
            colBed.Width = 130
            colBed.ReadOnly = True
            colBed.DefaultCellStyle.Format = "N0"
            colBed.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colBed.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colBes As New DataGridViewTextBoxColumn()
            colBes.Name = "colBes"
            colBes.DataPropertyName = "JamBestankar"
            colBes.HeaderText = "جمع بستانکار"
            colBes.Width = 130
            colBes.ReadOnly = True
            colBes.DefaultCellStyle.Format = "N0"
            colBes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            colBes.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colTaeaz As New DataGridViewTextBoxColumn()
            colTaeaz.Name = "colTaeaz"
            colTaeaz.DataPropertyName = "TaeazSanad"
            colTaeaz.HeaderText = "تعادل سند"
            colTaeaz.Width = 90
            colTaeaz.ReadOnly = True

            Dim colVazeiat As New DataGridViewTextBoxColumn()
            colVazeiat.Name = "colVazeiat"
            colVazeiat.DataPropertyName = "VazeiatSanad"
            colVazeiat.HeaderText = "وضعیت سند"
            colVazeiat.Width = 175
            colVazeiat.ReadOnly = True

            dgvEntries.Columns.AddRange(New DataGridViewColumn() {
                colEdit, colDel, colLockChk,
                colId, colRef, colDate, colDesc,
                colBed, colBes, colTaeaz, colVazeiat})
            dgvEntries.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        End Sub

        Private Sub LoadEntries()
            _entriesTable = service.GetEntries()
            dgvEntries.DataSource = _entriesTable
            ApplySearchFilter()
            AlignSearchBoxes()
        End Sub

        Private Function IsRowLocked(row As DataGridViewRow) As Boolean
            Dim lockVal = row.Cells(ColLock).Value
            Return lockVal IsNot Nothing AndAlso lockVal IsNot DBNull.Value AndAlso Convert.ToBoolean(lockVal)
        End Function

        Private Function GetEntryId(row As DataGridViewRow) As Integer?
            Dim idVal = row.Cells("colEntryID").Value
            If idVal Is Nothing OrElse idVal Is DBNull.Value Then Return Nothing
            Return Convert.ToInt32(idVal)
        End Function

        Private Sub DgvEntries_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntries.CellContentClick
            If e.RowIndex < 0 Then Return
            Dim colName = dgvEntries.Columns(e.ColumnIndex).Name

            ' کلیک روی چک‌باکس — commit کن تا CellValueChanged بزنه
            If colName = ColLock Then
                dgvEntries.CommitEdit(DataGridViewDataErrorContexts.Commit)
                Return
            End If

            Dim row = dgvEntries.Rows(e.RowIndex)
            Dim entryId = GetEntryId(row)
            If Not entryId.HasValue Then Return
            Dim refNum = Convert.ToString(row.Cells("colRef").Value)

            Select Case colName
                Case ColBtnEdit
                    If IsRowLocked(row) Then
                        MessageBox.Show(
                            "کاربر گرامی شما اجاره ویرایش این سند را ندارید ، لطفا برای ویرایش سند به مدیر ارشد خود مراجعه کنید",
                            "دسترسی محدود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    ShowDetailsFormInTab(New HesabdarySanad2Form(entryId.Value))

                Case ColBtnDelete
                    Dim ans = MessageBox.Show(
                        "سند شماره « " & refNum & " » حذف شود؟" & Environment.NewLine &
                        "سند به صورت موقت حذف می‌شود و قابل بازیابی است.",
                        "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If ans = DialogResult.Yes Then
                        Try
                            service.SetEntryStatus(entryId.Value, "سند موقت - حذف موقت")
                            LoadEntries()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف: " & ex.Message, "خطا",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
            End Select
        End Sub

        Private Sub DgvEntries_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEntries.CellValueChanged
            If e.RowIndex < 0 Then Return
            If dgvEntries.Columns(e.ColumnIndex).Name <> ColLock Then Return

            Dim row = dgvEntries.Rows(e.RowIndex)
            Dim entryId = GetEntryId(row)
            If Not entryId.HasValue Then Return

            Dim lockVal = row.Cells(ColLock).Value
            Dim newVal = If(lockVal IsNot Nothing AndAlso lockVal IsNot DBNull.Value, Convert.ToBoolean(lockVal), False)

            Try
                service.SetAdamVirayesh(entryId.Value, newVal)
            Catch ex As Exception
                MessageBox.Show("خطا در به‌روزرسانی وضعیت قفل: " & ex.Message, "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Dim suggestedRef = service.GetNextReferenceNumber()
            ShowDetailsFormInTab(New HesabdarySanad2Form(suggestedRef))
        End Sub

        Private Sub BtnCopySanad_Click(sender As Object, e As EventArgs) Handles btnCopySanad.Click
            If dgvEntries.CurrentRow Is Nothing Then
                MessageBox.Show("لطفا ابتدا یک سند را از لیست انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim entryId = GetEntryId(dgvEntries.CurrentRow)
            If Not entryId.HasValue Then
                MessageBox.Show("سند انتخاب شده نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim suggestedRef = service.GetNextReferenceNumber()
            ShowDetailsFormInTab(New HesabdarySanad2Form(suggestedRef, entryId.Value))
        End Sub

        Private Sub BtnMerge_Click(sender As Object, e As EventArgs) Handles btnMerge.Click
            Using dlg As New MergeSanadsForm()
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    LoadEntries()
                End If
            End Using
        End Sub

        Private Sub BtnSplit_Click(sender As Object, e As EventArgs) Handles btnSplit.Click
            If dgvEntries.CurrentRow Is Nothing Then
                MessageBox.Show("لطفا ابتدا یک سند را از لیست انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim entryId = GetEntryId(dgvEntries.CurrentRow)
            If Not entryId.HasValue Then
                MessageBox.Show("سند انتخاب شده نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim row = dgvEntries.CurrentRow
            If IsRowLocked(row) Then
                MessageBox.Show(
                    "کاربر گرامی، این سند قفل شده است و امکان تجزیه آن وجود ندارد.",
                    "سند قفل شده", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using dlg As New SplitSanadForm(entryId.Value)
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    LoadEntries()
                End If
            End Using
        End Sub

        Private Sub HesabdarySanad1Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.Control AndAlso e.KeyCode = Keys.P Then
                e.SuppressKeyPress = True
                btnPrintDocs.PerformClick()
            End If
        End Sub

        Private Sub BtnPrintDocs_Click(sender As Object, e As EventArgs) Handles btnPrintDocs.Click
            Using dlg As New PrintRangeDialog()
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    Dim printForm As HesabdaryPrintForm
                    If dlg.PrintByRef Then
                        printForm = New HesabdaryPrintForm(dlg.FromRef, dlg.ToRef, String.Empty, String.Empty)
                    Else
                        printForm = New HesabdaryPrintForm(Nothing, Nothing, dlg.FromDate, dlg.ToDate)
                    End If
                    printForm.ShowDialog(Me)
                End If
            End Using
        End Sub

        Private Sub BtnPrintJournal_Click(sender As Object, e As EventArgs) Handles btnPrintJournal.Click
            Using dlg As New PrintRangeDialog()
                If dlg.ShowDialog(Me) = DialogResult.OK Then
                    Dim journalForm As HesabdaryRooznamePrintForm
                    If dlg.PrintByRef Then
                        journalForm = New HesabdaryRooznamePrintForm(dlg.FromRef, dlg.ToRef, String.Empty, String.Empty)
                    Else
                        journalForm = New HesabdaryRooznamePrintForm(Nothing, Nothing, dlg.FromDate, dlg.ToDate)
                    End If
                    journalForm.ShowDialog(Me)
                End If
            End Using
        End Sub

        ' نمایش فرم جزئیات سند درون همان تب — روی فرم لیست را می‌پوشاند
        ' وقتی فرم جزئیات بسته شد خودش از تب حذف می‌شود و فرم لیست مجدداً ظاهر می‌شود
        Private Sub ShowDetailsFormInTab(detailsForm As HesabdarySanad2Form)
            Dim parentContainer = Me.Parent   ' TabPage
            If parentContainer Is Nothing Then
                ' پشتیبان: اگر به هر دلیل parent یافت نشد، dialog معمولی باز کن
                detailsForm.ShowDialog(Me)
                LoadEntries()
                Return
            End If

            detailsForm.TopLevel = False
            detailsForm.FormBorderStyle = FormBorderStyle.None
            detailsForm.Dock = DockStyle.Fill

            AddHandler detailsForm.FormClosed,
                Sub(s As Object, ea As FormClosedEventArgs)
                    parentContainer.Controls.Remove(detailsForm)
                    detailsForm.Dispose()
                    LoadEntries()
                End Sub

            parentContainer.Controls.Add(detailsForm)
            detailsForm.BringToFront()
            detailsForm.Show()
        End Sub

        Public Sub OpenDocumentForEdit(entryId As Integer, targetLineNumber As Integer?, returnToLedger As Boolean, Optional returnToDaftarShenavar As Boolean = False)
            Dim parentContainer = Me.Parent   ' TabPage
            If parentContainer IsNot Nothing Then
                ' بستن هرگونه فرم جزئیات باز قبلی در تب جاری
                For Each ctrl As Control In parentContainer.Controls
                    If TypeOf ctrl Is HesabdarySanad2Form Then
                        Dim existing = DirectCast(ctrl, HesabdarySanad2Form)
                        existing.Close()
                    End If
                Next
            End If

            Dim detailsForm As New HesabdarySanad2Form(entryId)
            If targetLineNumber.HasValue Then
                detailsForm.TargetLineNumber = targetLineNumber
            End If

            If parentContainer Is Nothing Then
                detailsForm.ShowDialog(Me)
                LoadEntries()
                Return
            End If

            detailsForm.TopLevel = False
            detailsForm.FormBorderStyle = FormBorderStyle.None
            detailsForm.Dock = DockStyle.Fill

            AddHandler detailsForm.FormClosed,
                Sub(s As Object, ea As FormClosedEventArgs)
                    parentContainer.Controls.Remove(detailsForm)
                    detailsForm.Dispose()
                    LoadEntries()

                    If returnToLedger Then
                        Dim parentForm = TryCast(Application.OpenForms("HesabdaryMainForm"), HesabdaryMainForm)
                        If parentForm IsNot Nothing Then
                            parentForm.SwitchToLedgerTabAndRefresh()
                        End If
                    ElseIf returnToDaftarShenavar Then
                        Dim parentForm = TryCast(Application.OpenForms("HesabdaryMainForm"), HesabdaryMainForm)
                        If parentForm IsNot Nothing Then
                            parentForm.SwitchToDaftarShenavarTabAndRefresh()
                        End If
                    End If
                End Sub

            parentContainer.Controls.Add(detailsForm)
            detailsForm.BringToFront()
            detailsForm.Show()
        End Sub

        Private Sub DgvEntries_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvEntries.CellFormatting
            PersianDateHelper.ApplyToGrid(sender, e)
        End Sub

        Private Sub InitializeSearchPanel()
            Dim allTBs = {txtSrcEdit, txtSrcDel, txtSrcLock, txtSrcRef, txtSrcDate, txtSrcDesc, txtSrcBed, txtSrcBes, txtSrcTaeaz, txtSrcVazeiat}
            For Each tb In allTBs
                tb.Height = 22
                tb.Font = New System.Drawing.Font("Tahoma", 8.25!)
                If Not tb.ReadOnly Then
                    AddHandler tb.TextChanged, AddressOf TxtSrcAny_TextChanged
                End If
            Next

            pnlSerch.SendToBack()
            pnlTop.SendToBack()

            ' Register event handlers for grid scroll/resize/column width change
            AddHandler dgvEntries.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvEntries.Scroll, AddressOf DgvEntries_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes
            
            AlignSearchBoxes()
        End Sub

        Private Sub DgvEntries_Scroll(sender As Object, e As ScrollEventArgs)
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvEntries Is Nothing OrElse dgvEntries.Columns.Count = 0 OrElse pnlSerch Is Nothing Then Return

            Dim AlignTB As Action(Of TextBox, String) = Sub(tb As TextBox, colName As String)
                              Dim col = dgvEntries.Columns(colName)
                              If col Is Nothing OrElse Not col.Visible Then
                                  tb.Visible = False
                                  Return
                              End If
                              Dim r = dgvEntries.GetColumnDisplayRectangle(col.Index, True)
                              If r.IsEmpty OrElse r.Width = 0 Then
                                  tb.Visible = False
                                  Return
                              End If
                              Dim screenPt = dgvEntries.PointToScreen(New System.Drawing.Point(r.X, 0))
                              Dim panelPt = pnlSerch.PointToClient(screenPt)
                              tb.Location = New System.Drawing.Point(panelPt.X, 4)
                              tb.Width = r.Width
                              tb.Visible = True
                          End Sub

            AlignTB.Invoke(txtSrcEdit, ColBtnEdit)
            AlignTB.Invoke(txtSrcDel, ColBtnDelete)
            AlignTB.Invoke(txtSrcLock, ColLock)
            AlignTB.Invoke(txtSrcRef, "colRef")
            AlignTB.Invoke(txtSrcDate, "colDate")
            AlignTB.Invoke(txtSrcDesc, "colDesc")
            AlignTB.Invoke(txtSrcBed, "colBed")
            AlignTB.Invoke(txtSrcBes, "colBes")
            AlignTB.Invoke(txtSrcTaeaz, "colTaeaz")
            AlignTB.Invoke(txtSrcVazeiat, "colVazeiat")
        End Sub

        Private Sub ApplySearchFilter()
            If _entriesTable Is Nothing Then Return

            Dim parts As New System.Collections.Generic.List(Of String)()

            Dim refText = If(txtSrcRef IsNot Nothing, txtSrcRef.Text.Trim().Replace("'", "''"), "")
            Dim dateText = If(txtSrcDate IsNot Nothing, txtSrcDate.Text.Trim().Replace("'", "''"), "")
            Dim descText = If(txtSrcDesc IsNot Nothing, txtSrcDesc.Text.Trim().Replace("'", "''"), "")
            Dim bedText = If(txtSrcBed IsNot Nothing, txtSrcBed.Text.Trim().Replace("'", "''"), "")
            Dim besText = If(txtSrcBes IsNot Nothing, txtSrcBes.Text.Trim().Replace("'", "''"), "")
            Dim taeazText = If(txtSrcTaeaz IsNot Nothing, txtSrcTaeaz.Text.Trim().Replace("'", "''"), "")
            Dim vazeiatText = If(txtSrcVazeiat IsNot Nothing, txtSrcVazeiat.Text.Trim().Replace("'", "''"), "")

            If refText.Length > 0 Then
                parts.Add("Convert(ReferenceNumber, 'System.String') LIKE '%" & refText & "%'")
            End If
            If dateText.Length > 0 Then
                parts.Add("EntryDate LIKE '%" & dateText & "%'")
            End If
            If descText.Length > 0 Then
                parts.Add("Description LIKE '%" & descText & "%'")
            End If
            If bedText.Length > 0 Then
                parts.Add("Convert(JamBedehkar, 'System.String') LIKE '%" & bedText & "%'")
            End If
            If besText.Length > 0 Then
                parts.Add("Convert(JamBestankar, 'System.String') LIKE '%" & besText & "%'")
            End If
            If taeazText.Length > 0 Then
                parts.Add("TaeazSanad LIKE '%" & taeazText & "%'")
            End If
            If vazeiatText.Length > 0 Then
                parts.Add("VazeiatSanad LIKE '%" & vazeiatText & "%'")
            End If

            If parts.Count > 0 Then
                _entriesTable.DefaultView.RowFilter = String.Join(" AND ", parts.ToArray())
            Else
                _entriesTable.DefaultView.RowFilter = ""
            End If
        End Sub

        Private Sub TxtSrcAny_TextChanged(sender As Object, e As EventArgs)
            ApplySearchFilter()
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim userType = SessionContext.CurrentUser.UserType
            Dim isSuperAdmin = String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            Dim canCreate = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntryNew) OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntry & PermissionKeys.CanCreate)
            Dim canEdit = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntryEdit) OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntry & PermissionKeys.CanEdit)
            Dim canDelete = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntryDelete) OrElse SessionContext.HasPermission(PermissionKeys.AccountingEntry & PermissionKeys.CanDelete)
            Dim canCopy = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingSanadCopy) OrElse canCreate
            Dim canMerge = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingSanadMerge) OrElse canEdit
            Dim canSplit = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingSanadSplit) OrElse canEdit
            Dim canPrintDocs = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingSanad1PrintDocs)
            Dim canPrintJournal = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AccountingSanad1PrintJournal)

            btnNew.Visible = canCreate
            btnCopySanad.Visible = canCopy
            btnMerge.Visible = canMerge
            btnSplit.Visible = canSplit
            btnPrintDocs.Visible = canPrintDocs
            btnPrintJournal.Visible = canPrintJournal

            If dgvEntries.Columns.Contains(ColBtnEdit) Then
                dgvEntries.Columns(ColBtnEdit).Visible = canEdit
            End If
            If dgvEntries.Columns.Contains(ColBtnDelete) Then
                dgvEntries.Columns(ColBtnDelete).Visible = canDelete
            End If
        End Sub

    End Class
End Namespace
