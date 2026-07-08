Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class HesabdaryTarazForm
        Inherits Form

        Private ReadOnly service As New AccountingService()
        Private _rootNodes As New List(Of TrialNode)()
        Private _nodeDict As New Dictionary(Of Integer, TrialNode)()
        Private _showOnlyWithData As Boolean = True
        Private ReadOnly _searchTextBoxes As New Dictionary(Of String, TextBox)()

        Public Event AccountSelected(accountId As Integer, accountCode As String, accountName As String, hasChildren As Boolean, allIds As List(Of Integer))

        ' ================================================================
        ' کلاس گره درخت حساب
        ' ================================================================
        Private NotInheritable Class TrialNode
            Private _children As New List(Of TrialNode)()

            Public AccountID As Integer
            Public ParentAccountID As Integer?
            Public Level As Integer
            Public AccountCode As String
            Public AccountName As String
            Public DebitBeforeDirect As Decimal
            Public CreditBeforeDirect As Decimal
            Public DebitDuringDirect As Decimal
            Public CreditDuringDirect As Decimal
            Public DebitBeforeRollup As Decimal
            Public CreditBeforeRollup As Decimal
            Public DebitDuringRollup As Decimal
            Public CreditDuringRollup As Decimal
            Public IsExpanded As Boolean
            Public AccountNature As String

            Public ReadOnly Property Children As List(Of TrialNode)
                Get
                    Return _children
                End Get
            End Property

            Public ReadOnly Property HasChildren As Boolean
                Get
                    Return _children.Count > 0
                End Get
            End Property

            Public ReadOnly Property DebitBegin As Decimal
                Get
                    Return If(DebitBeforeRollup >= CreditBeforeRollup, DebitBeforeRollup - CreditBeforeRollup, 0D)
                End Get
            End Property

            Public ReadOnly Property CreditBegin As Decimal
                Get
                    Return If(CreditBeforeRollup > DebitBeforeRollup, CreditBeforeRollup - DebitBeforeRollup, 0D)
                End Get
            End Property

            Public ReadOnly Property DebitTotal As Decimal
                Get
                    Return DebitBegin + DebitDuringRollup
                End Get
            End Property

            Public ReadOnly Property CreditTotal As Decimal
                Get
                    Return CreditBegin + CreditDuringRollup
                End Get
            End Property

            Public ReadOnly Property DebitEnd As Decimal
                Get
                    Return If(DebitTotal >= CreditTotal, DebitTotal - CreditTotal, 0D)
                End Get
            End Property

            Public ReadOnly Property CreditEnd As Decimal
                Get
                    Return If(CreditTotal > DebitTotal, CreditTotal - DebitTotal, 0D)
                End Get
            End Property
        End Class

        ' ================================================================
        ' سازنده و بارگذاری
        ' ================================================================
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryTarazForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            dgvTrial.RowTemplate.Height = 26
            _showOnlyWithData = chkOnlyWithData.Checked
            cmbTrialType.SelectedIndex = 1 ' پیش‌فرض ۴ ستونی
            LoadData()

            Dim levelsObj = Sys_Hes_Anb.Data.Sql.ExecuteScalar("SELECT AccountLevels FROM Companies WHERE CompanyID = ?", SessionContext.CurrentCompanyID)
            Dim levels As Integer = If(levelsObj IsNot Nothing AndAlso Not Convert.IsDBNull(levelsObj), Convert.ToInt32(levelsObj), 4)
            If levels < 2 Then levels = 2
            If levels > 5 Then levels = 5

            cmbExpandToLevel.Items.Clear()
            Dim allItems As String() = {"گروه (بستن همه)", "کل", "معین", "تفضیلی ۱", "تفضیلی ۲", "تفضیلی ۳"}
            For i As Integer = 0 To levels - 1
                cmbExpandToLevel.Items.Add(allItems(i))
            Next

            SetupSearchPanel()

            cmbExpandToLevel.SelectedIndex = 0
            ExpandTreeToLevel(0)
        End Sub

        ' ================================================================
        ' بارگذاری داده‌ها و ساخت درخت
        ' ================================================================
        Private Sub LoadData()
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
                If Integer.TryParse(txtFromDoc.Text.Trim(), fDocVal) Then
                    fromDoc = fDocVal
                End If
                Dim tDocVal As Integer
                If Integer.TryParse(txtToDoc.Text.Trim(), tDocVal) Then
                    toDoc = tDocVal
                End If
            End If

            If chkFilterByStatus.Checked Then
                If cmbStatus.SelectedItem IsNot Nothing Then
                    docStatus = cmbStatus.SelectedItem.ToString()
                End If
            End If

            Dim dt As DataTable
            Using progress As New ProgressForm()
                progress.ShowAndCenter(Me)
                progress.UpdateProgress(15, "محاسبه تراز و دریافت اطلاعات حساب‌ها از دیتابیس...")

                Try
                    dt = service.GetAllAccountsWithDirectTotals(fromDateStr, toDateStr, fromDoc, toDoc, docStatus)
                Catch ex As Exception
                    MessageBox.Show("خطا در بارگذاری داده‌ها: " & ex.Message, "خطا",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                progress.UpdateProgress(50, "تحلیل و آماده‌سازی گره‌های تراز آزمایشی...")

                _nodeDict.Clear()
                _rootNodes.Clear()

                ' ساخت همه گره‌ها
                For Each row As DataRow In dt.Rows
                    Dim node As New TrialNode()
                    node.AccountID = Convert.ToInt32(row("AccountID"))
                    node.AccountCode = Convert.ToString(row("AccountCode"))
                    node.AccountName = Convert.ToString(row("AccountName"))
                    node.AccountNature = Convert.ToString(row("AccountNature"))
                    node.ParentAccountID = If(row.IsNull("ParentAccountID"),
                                              CType(Nothing, Integer?),
                                              CType(Convert.ToInt32(row("ParentAccountID")), Integer?))
                    node.DebitBeforeDirect = Convert.ToDecimal(row("DebitBeforeDirect"))
                    node.CreditBeforeDirect = Convert.ToDecimal(row("CreditBeforeDirect"))
                    node.DebitDuringDirect = Convert.ToDecimal(row("DebitDuringDirect"))
                    node.CreditDuringDirect = Convert.ToDecimal(row("CreditDuringDirect"))
                    node.DebitBeforeRollup = node.DebitBeforeDirect
                    node.CreditBeforeRollup = node.CreditBeforeDirect
                    node.DebitDuringRollup = node.DebitDuringDirect
                    node.CreditDuringRollup = node.CreditDuringDirect
                    _nodeDict(node.AccountID) = node
                Next

                progress.UpdateProgress(75, "برقراری روابط سلسله‌مراتبی حساب‌ها...")

                ' ساخت روابط والد-فرزند
                For Each node In _nodeDict.Values
                    If node.ParentAccountID.HasValue AndAlso _nodeDict.ContainsKey(node.ParentAccountID.Value) Then
                        _nodeDict(node.ParentAccountID.Value).Children.Add(node)
                    Else
                        _rootNodes.Add(node)
                    End If
                Next

                ' تنظیم سطح هر گره در درخت
                SetLevels(_rootNodes, 0)

                progress.UpdateProgress(90, "محاسبه جمع‌های تجمعی از برگ‌ها به ریشه...")

                ' محاسبه جمع تجمعی از برگ به ریشه
                For Each root In _rootNodes
                    CalculateRollup(root)
                Next

                progress.UpdateProgress(100, "تراز آزمایشی آماده شد")
            End Using
        End Sub

        Private Sub SetLevels(nodes As List(Of TrialNode), level As Integer)
            If level > 20 Then Return
            For Each node In nodes
                node.Level = level
                SetLevels(node.Children, level + 1)
            Next
        End Sub

        Private Function CalculateRollup(node As TrialNode) As Decimal()
            Dim debitBefore = node.DebitBeforeDirect
            Dim creditBefore = node.CreditBeforeDirect
            Dim debitDuring = node.DebitDuringDirect
            Dim creditDuring = node.CreditDuringDirect
            For Each child In node.Children
                Dim childTotals = CalculateRollup(child)
                debitBefore += childTotals(0)
                creditBefore += childTotals(1)
                debitDuring += childTotals(2)
                creditDuring += childTotals(3)
            Next
            node.DebitBeforeRollup = debitBefore
            node.CreditBeforeRollup = creditBefore
            node.DebitDuringRollup = debitDuring
            node.CreditDuringRollup = creditDuring
            Return New Decimal() {debitBefore, creditBefore, debitDuring, creditDuring}
        End Function

        ' ================================================================
        ' نمایش در گرید
        ' ================================================================
        Private Sub RefreshGrid()
            If dgvTrial Is Nothing OrElse dgvTrial.Columns.Count = 0 Then Return
            dgvTrial.SuspendLayout()
            dgvTrial.Rows.Clear()

            ApplyColumnVisibility()

            Dim displayList As New List(Of TrialNode)()
            BuildDisplayList(_rootNodes, displayList)

            For Each node In displayList
                Dim rowIdx = dgvTrial.Rows.Add()
                Dim row = dgvTrial.Rows(rowIdx)
                row.Tag = node

                row.Cells("colToggle").Value = GetToggleText(node)
                row.Cells("colCode").Value = node.AccountCode
                row.Cells("colName").Value = node.AccountName

                row.Cells("colDebitBefore").Value = FormatAmount(node.DebitBeforeRollup)
                row.Cells("colCreditBefore").Value = FormatAmount(node.CreditBeforeRollup)
                row.Cells("colDebitBegin").Value = FormatAmount(node.DebitBegin)
                row.Cells("colCreditBegin").Value = FormatAmount(node.CreditBegin)
                row.Cells("colDebitDuring").Value = FormatAmount(node.DebitDuringRollup)
                row.Cells("colCreditDuring").Value = FormatAmount(node.CreditDuringRollup)
                row.Cells("colDebitTotal").Value = FormatAmount(node.DebitTotal)
                row.Cells("colCreditTotal").Value = FormatAmount(node.CreditTotal)
                row.Cells("colDebitEnd").Value = FormatAmount(node.DebitEnd)
                row.Cells("colCreditEnd").Value = FormatAmount(node.CreditEnd)

                ApplyRowStyle(row, node)
            Next

            dgvTrial.ResumeLayout()

            UpdateTotals()
            AlignJamLabels()
        End Sub

        Private Sub BuildDisplayList(nodes As List(Of TrialNode), result As List(Of TrialNode))
            For Each node In nodes
                If _showOnlyWithData AndAlso
                   node.DebitBeforeRollup = 0D AndAlso
                   node.CreditBeforeRollup = 0D AndAlso
                   node.DebitDuringRollup = 0D AndAlso
                   node.CreditDuringRollup = 0D Then
                    Continue For
                End If
                
                If Not MatchesFilterOrHasDescendantMatch(node) Then
                    Continue For
                End If

                result.Add(node)
                If node.IsExpanded AndAlso node.HasChildren Then
                    BuildDisplayList(node.Children, result)
                End If
            Next
        End Sub

        Private Function GetToggleText(node As TrialNode) As String
            If Not node.HasChildren Then Return ""
            For Each child In node.Children
                If Not _showOnlyWithData OrElse
                   child.DebitBeforeRollup <> 0D OrElse
                   child.CreditBeforeRollup <> 0D OrElse
                   child.DebitDuringRollup <> 0D OrElse
                   child.CreditDuringRollup <> 0D Then
                    Return If(node.IsExpanded, "−", "+")
                End If
            Next
            Return ""
        End Function

        Private Sub ApplyRowStyle(row As DataGridViewRow, node As TrialNode)
            Select Case node.Level
                Case 0
                    row.DefaultCellStyle.Font = New Font(dgvTrial.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(210, 228, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 100)
                    row.Height = 28
                Case 1
                    row.DefaultCellStyle.Font = New Font(dgvTrial.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 243, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 60, 120)
                Case 2
                    row.DefaultCellStyle.BackColor = Color.FromArgb(246, 251, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case Else
                    row.DefaultCellStyle.BackColor = Color.White
                    row.DefaultCellStyle.ForeColor = Color.Black
            End Select
        End Sub

        Private Function FormatAmount(amount As Decimal) As String
            Return amount.ToString("#,##0")
        End Function

        ' ================================================================
        ' رویدادهای گرید
        ' ================================================================
        Private Sub DgvTrial_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTrial.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvTrial.Columns(e.ColumnIndex).Name = "colToggle" Then
                ToggleNode(e.RowIndex)
            End If
        End Sub

        Private Sub DgvTrial_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTrial.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvTrial.Columns(e.ColumnIndex).Name = "colLedger" Then
                Dim node = TryCast(dgvTrial.Rows(e.RowIndex).Tag, TrialNode)
                If node IsNot Nothing Then
                    Dim allIds As New List(Of Integer)()
                    CollectAllIds(node, allIds)
                    RaiseEvent AccountSelected(node.AccountID, node.AccountCode, node.AccountName, node.HasChildren, allIds)
                End If
            End If
        End Sub

        Private Sub CollectAllIds(node As TrialNode, ids As List(Of Integer))
            ids.Add(node.AccountID)
            For Each child In node.Children
                CollectAllIds(child, ids)
            Next
        End Sub

        Private Sub ToggleNode(rowIndex As Integer)
            Dim node = TryCast(dgvTrial.Rows(rowIndex).Tag, TrialNode)
            If node Is Nothing OrElse Not node.HasChildren Then Return
            Dim hasVisible = False
            For Each child In node.Children
                If Not _showOnlyWithData OrElse
                   child.DebitBeforeRollup <> 0D OrElse
                   child.CreditBeforeRollup <> 0D OrElse
                   child.DebitDuringRollup <> 0D OrElse
                   child.CreditDuringRollup <> 0D Then
                    hasVisible = True
                    Exit For
                End If
            Next
            If Not hasVisible Then Return
            node.IsExpanded = Not node.IsExpanded
            RefreshGrid()
        End Sub

        Private Sub DgvTrial_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvTrial.CellFormatting
            If e.RowIndex < 0 Then Return
            Dim colName = dgvTrial.Columns(e.ColumnIndex).Name

            Dim node = TryCast(dgvTrial.Rows(e.RowIndex).Tag, TrialNode)
            If node IsNot Nothing Then
                If colName = "colName" Then
                    ' تورفتگی چپ برای سطح‌بندی درختی
                    dgvTrial.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.Padding =
                        New Padding(node.Level * 20, 0, 0, 0)
                End If

                ' Highlight Nature Violations (Debit only account with Credit balance, or Credit only account with Debit balance)
                Dim isViolation = False
                If node.AccountNature = "Bedehkar" AndAlso node.CreditEnd > 0 Then
                    isViolation = True
                ElseIf node.AccountNature = "Bestankar" AndAlso node.DebitEnd > 0 Then
                    isViolation = True
                End If

                If isViolation Then
                    If colName = "colDebitEnd" OrElse colName = "colCreditEnd" OrElse colName = "colCode" OrElse colName = "colName" Then
                        e.CellStyle.BackColor = Color.FromArgb(255, 204, 204) ' Soft light red
                        e.CellStyle.ForeColor = Color.FromArgb(150, 0, 0)      ' Dark red text
                        e.CellStyle.Font = New Font(dgvTrial.Font, FontStyle.Bold)
                    End If
                End If
            End If

            If colName = "colToggle" Then
                Dim txt = Convert.ToString(e.Value)
                If txt = "+" OrElse txt = "−" Then
                    e.CellStyle.BackColor = Color.FromArgb(225, 235, 255)
                    e.CellStyle.SelectionBackColor = Color.FromArgb(100, 130, 200)
                End If
            End If
        End Sub

        Private Sub DgvTrial_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTrial.CellMouseEnter
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvTrial.Columns(e.ColumnIndex).Name = "colToggle" Then
                Dim node = TryCast(dgvTrial.Rows(e.RowIndex).Tag, TrialNode)
                If node IsNot Nothing AndAlso node.HasChildren Then
                    dgvTrial.Cursor = Cursors.Hand
                End If
            End If
        End Sub

        Private Sub DgvTrial_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTrial.CellMouseLeave
            dgvTrial.Cursor = Cursors.Default
        End Sub

        Public Sub RefreshData()
            _showOnlyWithData = chkOnlyWithData.Checked
            LoadData()
            If cmbExpandToLevel.SelectedIndex >= 0 Then
                ExpandTreeToLevel(cmbExpandToLevel.SelectedIndex)
            Else
                RefreshGrid()
            End If
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            RefreshData()
        End Sub

        Private Sub cmbExpandToLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExpandToLevel.SelectedIndexChanged
            If cmbExpandToLevel.SelectedIndex >= 0 Then
                ExpandTreeToLevel(cmbExpandToLevel.SelectedIndex)
            End If
        End Sub

        Private Sub ExpandTreeToLevel(maxLevel As Integer)
            For Each node In _nodeDict.Values
                If node.Level < maxLevel Then
                    node.IsExpanded = True
                Else
                    node.IsExpanded = False
                End If
            Next
            RefreshGrid()
        End Sub

        Private Sub ChkOnlyWithData_CheckedChanged(sender As Object, e As EventArgs) Handles chkOnlyWithData.CheckedChanged
            _showOnlyWithData = chkOnlyWithData.Checked
            RefreshGrid()
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
        End Sub

        Private Sub ChkFilterByDoc_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterByDoc.CheckedChanged
            Dim isChecked = chkFilterByDoc.Checked
            txtFromDoc.Enabled = isChecked
            txtToDoc.Enabled = isChecked
        End Sub

        Private Sub ChkFilterByStatus_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterByStatus.CheckedChanged
            cmbStatus.Enabled = chkFilterByStatus.Checked
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
                End If
            End Using
        End Sub

        Private Shared Function EnsureOnScreen(pos As Point, formSize As Size) As Point
            Dim wa = Screen.FromPoint(pos).WorkingArea
            Return New Point(
                Math.Max(wa.Left, Math.Min(pos.X, wa.Right - formSize.Width)),
                Math.Max(wa.Top, Math.Min(pos.Y, wa.Bottom - formSize.Height)))
        End Function

        Private Sub pnlFilters_Paint(sender As Object, e As PaintEventArgs) Handles pnlFilters.Paint

        End Sub

        Private Sub ApplyColumnVisibility()
            If dgvTrial Is Nothing OrElse dgvTrial.Columns.Count = 0 OrElse dgvTrial.Columns("colDebitBefore") Is Nothing Then Return
            Dim selectedType As String = Nothing
            If cmbTrialType.SelectedItem IsNot Nothing Then
                selectedType = cmbTrialType.SelectedItem.ToString()
            End If
            If String.IsNullOrEmpty(selectedType) Then selectedType = "4 ستونی"

            Dim showBefore = (selectedType = "10 ستونی")
            Dim showBegin = (selectedType = "6 ستونی" OrElse selectedType = "8 ستونی" OrElse selectedType = "10 ستونی")
            Dim showDuring = (selectedType = "4 ستونی" OrElse selectedType = "6 ستونی" OrElse selectedType = "8 ستونی" OrElse selectedType = "10 ستونی")
            Dim showTotal = (selectedType = "8 ستونی" OrElse selectedType = "10 ستونی")
            Dim showEnd = True

            dgvTrial.Columns("colDebitBefore").Visible = showBefore
            dgvTrial.Columns("colCreditBefore").Visible = showBefore
            dgvTrial.Columns("colDebitBegin").Visible = showBegin
            dgvTrial.Columns("colCreditBegin").Visible = showBegin
            dgvTrial.Columns("colDebitDuring").Visible = showDuring
            dgvTrial.Columns("colCreditDuring").Visible = showDuring
            dgvTrial.Columns("colDebitTotal").Visible = showTotal
            dgvTrial.Columns("colCreditTotal").Visible = showTotal
            dgvTrial.Columns("colDebitEnd").Visible = showEnd
            dgvTrial.Columns("colCreditEnd").Visible = showEnd

            AlignJamLabels()
            AlignSearchControls()
        End Sub

        Private Sub CmbTrialType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTrialType.SelectedIndexChanged
            ApplyColumnVisibility()
        End Sub

        Public Sub RefreshTotalsAndLayout()
            UpdateTotals()
            AlignJamLabels()
            AlignSearchControls()
        End Sub

        Private Sub UpdateTotals()
            If _rootNodes Is Nothing OrElse _rootNodes.Count = 0 Then Return

            Dim sumDebitBefore As Decimal = 0
            Dim sumCreditBefore As Decimal = 0
            Dim sumDebitBegin As Decimal = 0
            Dim sumCreditBegin As Decimal = 0
            Dim sumDebitDuring As Decimal = 0
            Dim sumCreditDuring As Decimal = 0
            Dim sumDebitTotal As Decimal = 0
            Dim sumCreditTotal As Decimal = 0
            Dim sumDebitEnd As Decimal = 0
            Dim sumCreditEnd As Decimal = 0

            For Each node In _rootNodes
                sumDebitBefore += node.DebitBeforeRollup
                sumCreditBefore += node.CreditBeforeRollup
                sumDebitBegin += node.DebitBegin
                sumCreditBegin += node.CreditBegin
                sumDebitDuring += node.DebitDuringRollup
                sumCreditDuring += node.CreditDuringRollup
                sumDebitTotal += node.DebitTotal
                sumCreditTotal += node.CreditTotal
                sumDebitEnd += node.DebitEnd
                sumCreditEnd += node.CreditEnd
            Next

            lblSumDebitBefore.Text = FormatAmount(sumDebitBefore)
            lblSumCreditBefore.Text = FormatAmount(sumCreditBefore)
            lblSumDebitBegin.Text = FormatAmount(sumDebitBegin)
            lblSumCreditBegin.Text = FormatAmount(sumCreditBegin)
            lblSumDebitDuring.Text = FormatAmount(sumDebitDuring)
            lblSumCreditDuring.Text = FormatAmount(sumCreditDuring)
            lblSumDebitTotal.Text = FormatAmount(sumDebitTotal)
            lblSumCreditTotal.Text = FormatAmount(sumCreditTotal)
            lblSumDebitEnd.Text = FormatAmount(sumDebitEnd)
            lblSumCreditEnd.Text = FormatAmount(sumCreditEnd)
        End Sub

        Private Sub AlignJamLabels()
            If dgvTrial Is Nothing OrElse dgvTrial.Columns.Count = 0 OrElse pnlJam Is Nothing Then Return

            Dim nameCol = dgvTrial.Columns("colName")
            If nameCol IsNot Nothing AndAlso nameCol.Visible Then
                Dim rect = dgvTrial.GetColumnDisplayRectangle(nameCol.Index, True)
                lblJamTitle.Left = rect.Left
                lblJamTitle.Width = rect.Width
                lblJamTitle.Visible = rect.Width > 0
                lblJamTitle.BringToFront()
            Else
                lblJamTitle.Visible = False
            End If

            AlignLabel("colDebitBefore", lblSumDebitBefore)
            AlignLabel("colCreditBefore", lblSumCreditBefore)
            AlignLabel("colDebitBegin", lblSumDebitBegin)
            AlignLabel("colCreditBegin", lblSumCreditBegin)
            AlignLabel("colDebitDuring", lblSumDebitDuring)
            AlignLabel("colCreditDuring", lblSumCreditDuring)
            AlignLabel("colDebitTotal", lblSumDebitTotal)
            AlignLabel("colCreditTotal", lblSumCreditTotal)
            AlignLabel("colDebitEnd", lblSumDebitEnd)
            AlignLabel("colCreditEnd", lblSumCreditEnd)
        End Sub

        Private Sub AlignLabel(columnName As String, label As Label)
            If label Is Nothing Then Return
            Dim col = dgvTrial.Columns(columnName)
            If col IsNot Nothing AndAlso col.Visible Then
                Dim rect = dgvTrial.GetColumnDisplayRectangle(col.Index, True)
                label.Left = rect.Left
                label.Width = rect.Width
                label.Visible = rect.Width > 0
                label.BringToFront()
            Else
                label.Visible = False
            End If
        End Sub

        Private Sub DgvTrial_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvTrial.Scroll
            AlignJamLabels()
            AlignSearchControls()
        End Sub

        Private Sub DgvTrial_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvTrial.ColumnWidthChanged
            AlignJamLabels()
            AlignSearchControls()
        End Sub

        Private Sub DgvTrial_Resize(sender As Object, e As EventArgs) Handles dgvTrial.Resize
            AlignJamLabels()
            AlignSearchControls()
        End Sub

        Private Sub HesabdaryTarazForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            RefreshTotalsAndLayout()
        End Sub

        Private Sub HesabdaryTarazForm_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
            If Me.Visible Then
                RefreshTotalsAndLayout()
            End If
        End Sub

        Private Sub PnlJam_Paint(sender As Object, e As PaintEventArgs) Handles pnlJam.Paint
            AlignJamLabels()
        End Sub

        Private Sub btnPrintTaraz_Click(sender As Object, e As EventArgs) Handles btnPrintTaraz.Click
            Try
                Dim companyName As String = "مؤسسه حسابداری"
                If SessionContext.CurrentCompanyID.HasValue Then
                    Dim dtComp = Sys_Hes_Anb.Data.Sql.ExecuteTable("SELECT CompanyName FROM Companies WHERE CompanyID = ?", SessionContext.CurrentCompanyID.Value)
                    If dtComp.Rows.Count > 0 AndAlso Not dtComp.Rows(0).IsNull("CompanyName") Then
                        companyName = dtComp.Rows(0)("CompanyName").ToString()
                    End If
                End If

                Dim dateTitle As String = "در تاریخ " & PersianDateHelper.FormatDateTime(DateTime.Now)
                If chkFilterByDate.Checked AndAlso Not String.IsNullOrWhiteSpace(txtToDate.Text) Then
                    dateTitle = "در تاریخ " & txtToDate.Text.Trim()
                End If

                ' استخراج ستون‌های فعال
                Dim printCols As New List(Of HesabdaryTarazPrintForm.PrintColumnInfo)()
                
                ' ستون کد و نام حساب
                If dgvTrial.Columns("colCode") IsNot Nothing AndAlso dgvTrial.Columns("colCode").Visible Then
                    printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "AccountCode", .Title = "کد حساب", .WidthRatio = 1.5F})
                End If
                printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = "AccountName", .Title = "نام حساب", .WidthRatio = 3.5F})

                ' نگاشت ستون‌های عددی به تیتراژ استاندارد
                Dim colMap As New Dictionary(Of String, Tuple(Of String, Single))() From {
                    {"colDebitBefore", Tuple.Create("مانده بدهکار قبل", 2.0F)},
                    {"colCreditBefore", Tuple.Create("مانده بستانکار قبل", 2.0F)},
                    {"colDebitBegin", Tuple.Create("مانده بدهکار ابتدا", 2.0F)},
                    {"colCreditBegin", Tuple.Create("مانده بستانکار ابتدا", 2.0F)},
                    {"colDebitDuring", Tuple.Create("گردش بدهکار", 2.0F)},
                    {"colCreditDuring", Tuple.Create("گردش بستانکار", 2.0F)},
                    {"colDebitTotal", Tuple.Create("جمع بدهکار", 2.0F)},
                    {"colCreditTotal", Tuple.Create("جمع بستانکار", 2.0F)},
                    {"colDebitEnd", Tuple.Create("مانده بدهکار", 2.0F)},
                    {"colCreditEnd", Tuple.Create("مانده بستانکار", 2.0F)}
                }

                For Each kvp In colMap
                    Dim col = dgvTrial.Columns(kvp.Key)
                    If col IsNot Nothing AndAlso col.Visible Then
                        printCols.Add(New HesabdaryTarazPrintForm.PrintColumnInfo() With {.Key = kvp.Key, .Title = kvp.Value.Item1, .WidthRatio = kvp.Value.Item2})
                    End If
                Next

                ' استخراج ردیف‌های داده
                Dim printRows As New List(Of HesabdaryTarazPrintForm.PrintRowInfo)()
                For Each dgvRow As DataGridViewRow In dgvTrial.Rows
                    If dgvRow.IsNewRow Then Continue For
                    Dim node = TryCast(dgvRow.Tag, TrialNode)
                    Dim rInfo As New HesabdaryTarazPrintForm.PrintRowInfo()
                    If node IsNot Nothing Then
                        rInfo.AccountCode = node.AccountCode
                        rInfo.AccountName = node.AccountName
                        rInfo.Level = node.Level
                        rInfo.IsHeader = node.HasChildren

                        rInfo.Values("colDebitBefore") = node.DebitBeforeRollup
                        rInfo.Values("colCreditBefore") = node.CreditBeforeRollup
                        rInfo.Values("colDebitBegin") = node.DebitBegin
                        rInfo.Values("colCreditBegin") = node.CreditBegin
                        rInfo.Values("colDebitDuring") = node.DebitDuringRollup
                        rInfo.Values("colCreditDuring") = node.CreditDuringRollup
                        rInfo.Values("colDebitTotal") = node.DebitTotal
                        rInfo.Values("colCreditTotal") = node.CreditTotal
                        rInfo.Values("colDebitEnd") = node.DebitEnd
                        rInfo.Values("colCreditEnd") = node.CreditEnd
                    Else
                        rInfo.AccountCode = Convert.ToString(dgvRow.Cells("colCode").Value)
                        rInfo.AccountName = Convert.ToString(dgvRow.Cells("colName").Value)
                    End If
                    printRows.Add(rInfo)
                Next

                ' استخراج جمع کل
                Dim totals As New Dictionary(Of String, Decimal)()
                Dim parseAmount = Function(lblText As String) As Decimal
                                      If String.IsNullOrWhiteSpace(lblText) Then Return 0D
                                      Dim clean = lblText.Replace(",", "").Trim()
                                      Dim val As Decimal
                                      Decimal.TryParse(clean, val)
                                      Return val
                                  End Function

                totals("colDebitBefore") = parseAmount(lblSumDebitBefore.Text)
                totals("colCreditBefore") = parseAmount(lblSumCreditBefore.Text)
                totals("colDebitBegin") = parseAmount(lblSumDebitBegin.Text)
                totals("colCreditBegin") = parseAmount(lblSumCreditBegin.Text)
                totals("colDebitDuring") = parseAmount(lblSumDebitDuring.Text)
                totals("colCreditDuring") = parseAmount(lblSumCreditDuring.Text)
                totals("colDebitTotal") = parseAmount(lblSumDebitTotal.Text)
                totals("colCreditTotal") = parseAmount(lblSumCreditTotal.Text)
                totals("colDebitEnd") = parseAmount(lblSumDebitEnd.Text)
                totals("colCreditEnd") = parseAmount(lblSumCreditEnd.Text)

                Using printForm As New HesabdaryTarazPrintForm(companyName, dateTitle, printCols, printRows, totals)
                    printForm.ShowDialog(Me)
                End Using
            Catch ex As Exception
                MessageBox.Show("خطا در فراخوانی گزارش چاپ تراز: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub SetupSearchPanel()
            ' Map designer textboxes to their corresponding column names
            _searchTextBoxes("colToggle") = txtSearchToggle
            _searchTextBoxes("colLedger") = txtSearchLedger
            _searchTextBoxes("colCode") = txtSearchCode
            _searchTextBoxes("colName") = txtSearchName
            _searchTextBoxes("colDebitBefore") = txtSearchDebitBefore
            _searchTextBoxes("colCreditBefore") = txtSearchCreditBefore
            _searchTextBoxes("colDebitBegin") = txtSearchDebitBegin
            _searchTextBoxes("colCreditBegin") = txtSearchCreditBegin
            _searchTextBoxes("colDebitDuring") = txtSearchDebitDuring
            _searchTextBoxes("colCreditDuring") = txtSearchCreditDuring
            _searchTextBoxes("colDebitTotal") = txtSearchDebitTotal
            _searchTextBoxes("colCreditTotal") = txtSearchCreditTotal
            _searchTextBoxes("colDebitEnd") = txtSearchDebitEnd
            _searchTextBoxes("colCreditEnd") = txtSearchCreditEnd

            ' Register TextChanged event handlers
            For Each colName As String In _searchTextBoxes.Keys
                If colName <> "colToggle" AndAlso colName <> "colLedger" Then
                    AddHandler _searchTextBoxes(colName).TextChanged, AddressOf SearchTextBox_TextChanged
                End If
            Next

            AlignSearchControls()
        End Sub

        Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)
            RefreshGrid()
        End Sub

        Private Sub AlignSearchControls()
            Try
                If dgvTrial Is Nothing OrElse pnlGridSearch Is Nothing Then Return
                If dgvTrial.Columns.Count = 0 Then Return

                For Each col As DataGridViewColumn In dgvTrial.Columns
                    Dim txt As TextBox = Nothing
                    If _searchTextBoxes.TryGetValue(col.Name, txt) Then
                        If col.Visible Then
                            Dim colRect = dgvTrial.GetCellDisplayRectangle(col.Index, -1, True)
                            If colRect.Width > 0 Then
                                Dim screenPt = dgvTrial.PointToScreen(New Point(colRect.Left, 0))
                                Dim clientPt = pnlGridSearch.PointToClient(screenPt)
                                txt.Left = clientPt.X
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
            Catch ex As Exception
                ' Prevent crash
            End Try
        End Sub

        Private Function MatchesFilterOrHasDescendantMatch(node As TrialNode) As Boolean
            If PassesFilter(node) Then Return True
            For Each child In node.Children
                If MatchesFilterOrHasDescendantMatch(child) Then Return True
            Next
            Return False
        End Function

        Private Function PassesFilter(node As TrialNode) As Boolean
            For Each colName As String In _searchTextBoxes.Keys
                Dim txt = _searchTextBoxes(colName)
                Dim searchVal = txt.Text.Trim()
                If String.IsNullOrEmpty(searchVal) Then Continue For

                Dim cellVal As String = ""
                Select Case colName
                    Case "colCode": cellVal = node.AccountCode
                    Case "colName": cellVal = node.AccountName
                    Case "colDebitBefore": cellVal = FormatAmount(node.DebitBeforeRollup)
                    Case "colCreditBefore": cellVal = FormatAmount(node.CreditBeforeRollup)
                    Case "colDebitBegin": cellVal = FormatAmount(node.DebitBegin)
                    Case "colCreditBegin": cellVal = FormatAmount(node.CreditBegin)
                    Case "colDebitDuring": cellVal = FormatAmount(node.DebitDuringRollup)
                    Case "colCreditDuring": cellVal = FormatAmount(node.CreditDuringRollup)
                    Case "colDebitTotal": cellVal = FormatAmount(node.DebitTotal)
                    Case "colCreditTotal": cellVal = FormatAmount(node.CreditTotal)
                    Case "colDebitEnd": cellVal = FormatAmount(node.DebitEnd)
                    Case "colCreditEnd": cellVal = FormatAmount(node.CreditEnd)
                End Select

                If Not cellVal.Contains(searchVal) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
            ExportGridToExcel(dgvTrial, "Trial_Balance")
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
                            If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn AndAlso col.Name <> "colToggle" Then
                                headers.Add(col.HeaderText)
                            End If
                        Next
                        sb.AppendLine(String.Join(",", headers))
                        
                        ' Write rows
                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For
                            
                            Dim cells As New List(Of String)()
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible AndAlso Not TypeOf col Is DataGridViewButtonColumn AndAlso col.Name <> "colToggle" Then
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

