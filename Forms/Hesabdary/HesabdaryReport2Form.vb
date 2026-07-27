Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Partial Public Class HesabdaryReport2Form
        Inherits Form

        Public Event SaveCompleted As EventHandler
        Public Event ExitRequested As EventHandler

        Private _reportId As Integer = 0
        Private _rootNodes As New List(Of PLNode)()

        Private _cachedBalances As Dictionary(Of Integer, Tuple(Of String, Decimal)) = Nothing
        Private _cachedMappings As DataTable = Nothing
        Private _isCalculatingLive As Boolean = False

        Private Sub LoadCache()
            If SessionContext.CurrentCompanyID.HasValue AndAlso SessionContext.CurrentFiscalYearID.HasValue Then
                Try
                    _cachedBalances = service.GetAllAccountBalances(SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value)
                    _cachedMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)
                Catch
                End Try
            End If
        End Sub

        Private Sub CalculateLiveFormulas()
            If _cachedBalances Is Nothing Then Return
            If _isCalculatingLive Then Return
            _isCalculatingLive = True
            Try
                ' Pass 1: Sync grid values to nodes and calc BaseValue
                For Each row As DataGridViewRow In dgvReports.Rows
                    Dim node = TryCast(row.Tag, PLNode)
                    If node IsNot Nothing AndAlso node.IsCategory Then
                        Dim sum As Decimal = 0
                        If node.AccountID > 0 Then
                            Dim targetCode As String = ""
                            If _cachedBalances.ContainsKey(node.AccountID) Then
                                targetCode = _cachedBalances(node.AccountID).Item1
                            End If
                            If Not String.IsNullOrEmpty(targetCode) Then
                                For Each kvp In _cachedBalances.Values
                                    If kvp.Item1.StartsWith(targetCode) Then
                                        sum += kvp.Item2
                                    End If
                                Next
                            End If
                        Else
                            For Each child In node.Children
                                Dim targetCode As String = ""
                                If _cachedBalances.ContainsKey(child.AccountID) Then
                                    targetCode = _cachedBalances(child.AccountID).Item1
                                End If
                                If Not String.IsNullOrEmpty(targetCode) Then
                                    For Each kvp In _cachedBalances.Values
                                        If kvp.Item1.StartsWith(targetCode) Then
                                            sum += kvp.Item2
                                        End If
                                    Next
                                End If
                            Next
                        End If
                        node.BaseValue = sum
                    End If
                Next

                ' Pass 2: Calculate formulas
                Dim dtMath As New DataTable()
                For i As Integer = 0 To dgvReports.Rows.Count - 1
                    Dim row = dgvReports.Rows(i)
                    Dim node = TryCast(row.Tag, PLNode)
                    If node IsNot Nothing AndAlso node.IsCategory Then
                        Dim finalVal As Decimal = 0
                        If String.IsNullOrWhiteSpace(node.Formula) Then
                            finalVal = node.BaseValue
                        Else
                            Dim expr = node.Formula.Replace("=", "").Trim()
                            For j As Integer = 0 To dgvReports.Rows.Count - 1
                                Dim r2 = dgvReports.Rows(j)
                                Dim n2 = TryCast(r2.Tag, PLNode)
                                If n2 IsNot Nothing AndAlso n2.IsCategory Then
                                    Dim rowNo = Convert.ToString(r2.Cells("colRowNo").Value)
                                    If Not String.IsNullOrEmpty(rowNo) Then
                                        Dim pattern = "[" & rowNo & "]"
                                        expr = expr.Replace(pattern, n2.BaseValue.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                    End If
                                End If
                            Next
                            Try
                                Dim resultObj = dtMath.Compute(expr, "")
                                finalVal = Convert.ToDecimal(resultObj)
                            Catch ex As Exception
                                finalVal = node.BaseValue
                            End Try
                        End If
                        node.FinalValue = finalVal
                        row.Cells("colResult").Value = finalVal.ToString("N0")
                    End If
                Next
            Catch ex As Exception
            Finally
                _isCalculatingLive = False
            End Try
        End Sub

        Private ReadOnly service As New AccountingService()
        Private _sarfaslTargetRow As Integer = -1
        Private ReadOnly _columnLabels As New List(Of Label)()

        Public Property ReportID As Integer
            Get
                Return _reportId
            End Get
            Set(value As Integer)
                _reportId = value
                If _reportId > 0 Then
                    LoadReportData()
                Else
                    ResetForNewReport()
                End If
            End Set
        End Property

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)
            ApplySecurity()

            ' Register all 14 column header labels
            _columnLabels.Add(lblColA)
            _columnLabels.Add(lblColB)
            _columnLabels.Add(lblColC)
            _columnLabels.Add(lblColD)
            _columnLabels.Add(lblColE)
            _columnLabels.Add(lblColF)
            _columnLabels.Add(lblColG)
            _columnLabels.Add(lblColH)
            _columnLabels.Add(lblColI)
            _columnLabels.Add(lblColJ)
            _columnLabels.Add(lblColK)
            _columnLabels.Add(lblColL)
            _columnLabels.Add(lblColM)
            _columnLabels.Add(lblColN)
            _columnLabels.Add(lblColO)

            ' Setup colFormula alignment for LTR visual in RTL grid
            colFormula.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            colFormula.DefaultCellStyle.Font = New Font("Consolas", 10.0!, FontStyle.Bold)

            ' Auto-size columns based on content
            colCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet

            AlignColumnLetters()
        End Sub

        Private Sub ResetForNewReport()
            txtCode.Text = ""
            txtName.Text = ""
            _rootNodes.Clear()

            ' Reset UI style settings to defaults
            cmbFontHeader.Text = "Tahoma"
            numSizeHeader.Value = 12
            cmbFontMainRow.Text = "Tahoma"
            numSizeMainRow.Value = 10
            cmbFontDetailRow.Text = "Tahoma"
            numSizeDetailRow.Value = 9
            cmbFontFormula.Text = "Tahoma"
            numSizeFormula.Value = 9

            numRowCount.Value = 50
            numColCount.Value = 10
            cmbOrientation.Text = "عمودی"

            numMarginTop.Value = 10
            numMarginBottom.Value = 10
            numMarginLeft.Value = 10
            numMarginRight.Value = 10
            cmbPageBorder.Text = "بدون کادر"

            ' Setup 10 empty rows
            For i As Integer = 1 To 10
                Dim parent As New PLNode()
                parent.CategoryName = ""
                parent.Formula = ""
                parent.IsMainRow = False
                parent.RO = ""
                parent.SO = ""
                parent.RN = ""
                parent.SN = ""
                parent.UnderlineStyle = "بدون خط"
                parent.IsCategory = True
                _rootNodes.Add(parent)
            Next

            BuildAndRefreshGrid()
        End Sub

        Private Sub LoadReportData()
            Try
                LoadCache()
                Dim dtRep = Sql.ExecuteTable("SELECT ReportCode, ReportName, FontHeaderName, FontHeaderSize, FontMainRowName, FontMainRowSize, FontDetailRowName, FontDetailRowSize, FontFormulaName, FontFormulaSize, FontFormulaDetailName, FontFormulaDetailSize, RowCount, ColCount, Orientation, MarginTop, MarginBottom, MarginLeft, MarginRight, PageBorder FROM Report1 WHERE ReportID = ?", _reportId)
                If dtRep.Rows.Count > 0 Then
                    Dim r = dtRep.Rows(0)
                    txtCode.Text = Convert.ToString(r("ReportCode"))
                    txtName.Text = Convert.ToString(r("ReportName"))

                    cmbFontHeader.Text = If(Convert.IsDBNull(r("FontHeaderName")), "Tahoma", Convert.ToString(r("FontHeaderName")))
                    numSizeHeader.Value = If(Convert.IsDBNull(r("FontHeaderSize")), 12, Convert.ToDecimal(r("FontHeaderSize")))

                    cmbFontMainRow.Text = If(Convert.IsDBNull(r("FontMainRowName")), "Tahoma", Convert.ToString(r("FontMainRowName")))
                    numSizeMainRow.Value = If(Convert.IsDBNull(r("FontMainRowSize")), 10, Convert.ToDecimal(r("FontMainRowSize")))

                    cmbFontDetailRow.Text = If(Convert.IsDBNull(r("FontDetailRowName")), "Tahoma", Convert.ToString(r("FontDetailRowName")))
                    numSizeDetailRow.Value = If(Convert.IsDBNull(r("FontDetailRowSize")), 9, Convert.ToDecimal(r("FontDetailRowSize")))

                    cmbFontFormula.Text = If(Convert.IsDBNull(r("FontFormulaName")), "Tahoma", Convert.ToString(r("FontFormulaName")))
                    numSizeFormula.Value = If(Convert.IsDBNull(r("FontFormulaSize")), 9, Convert.ToDecimal(r("FontFormulaSize")))
                    cmbFontFormulaDetail.Text = If(Convert.IsDBNull(r("FontFormulaDetailName")), "Tahoma", Convert.ToString(r("FontFormulaDetailName")))
                    numSizeFormulaDetail.Value = If(Convert.IsDBNull(r("FontFormulaDetailSize")), 9, Convert.ToDecimal(r("FontFormulaDetailSize")))


                    numRowCount.Value = If(Convert.IsDBNull(r("RowCount")), 50, Convert.ToDecimal(r("RowCount")))
                    numColCount.Value = If(Convert.IsDBNull(r("ColCount")), 10, Convert.ToDecimal(r("ColCount")))

                    cmbOrientation.Text = If(Convert.IsDBNull(r("Orientation")), "عمودی", Convert.ToString(r("Orientation")))

                    numMarginTop.Value = If(Convert.IsDBNull(r("MarginTop")), 10, Convert.ToDecimal(r("MarginTop")))
                    numMarginBottom.Value = If(Convert.IsDBNull(r("MarginBottom")), 10, Convert.ToDecimal(r("MarginBottom")))
                    numMarginLeft.Value = If(Convert.IsDBNull(r("MarginLeft")), 10, Convert.ToDecimal(r("MarginLeft")))
                    numMarginRight.Value = If(Convert.IsDBNull(r("MarginRight")), 10, Convert.ToDecimal(r("MarginRight")))

                    cmbPageBorder.Text = If(Convert.IsDBNull(r("PageBorder")), "بدون کادر", Convert.ToString(r("PageBorder")))
                End If

                _rootNodes.Clear()
                Dim dtCats = service.GetProfitLossCategories(_reportId)
                Dim allMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)

                For Each rowCat As DataRow In dtCats.Rows
                    Dim catId = Convert.ToInt32(rowCat("CategoryID"))
                    Dim catName = Convert.ToString(rowCat("CategoryName"))
                    Dim formula = If(rowCat.Table.Columns.Contains("Formula"), Convert.ToString(rowCat("Formula")), "")

                    Dim isMainRow = If(rowCat.Table.Columns.Contains("IsMainRow") AndAlso Not Convert.IsDBNull(rowCat("IsMainRow")), Convert.ToInt32(rowCat("IsMainRow")) = 1, False)
                    Dim roVal = If(rowCat.Table.Columns.Contains("RO"), Convert.ToString(rowCat("RO")), "")
                    Dim soVal = If(rowCat.Table.Columns.Contains("SO"), Convert.ToString(rowCat("SO")), "")
                    Dim rnVal = If(rowCat.Table.Columns.Contains("RN"), Convert.ToString(rowCat("RN")), "")
                    Dim snVal = If(rowCat.Table.Columns.Contains("SN"), Convert.ToString(rowCat("SN")), "")
                    Dim underline = If(rowCat.Table.Columns.Contains("UnderlineStyle"), Convert.ToString(rowCat("UnderlineStyle")), "بدون خط")
                    Dim accId = If(rowCat.Table.Columns.Contains("AccountID") AndAlso Not Convert.IsDBNull(rowCat("AccountID")), Convert.ToInt32(rowCat("AccountID")), 0)
                    If String.IsNullOrEmpty(underline) Then underline = "بدون خط"

                    Dim parent As New PLNode()
                    parent.CategoryID = catId
                    parent.CategoryName = catName
                    parent.Formula = formula
                    parent.IsMainRow = isMainRow
                    parent.RO = roVal
                    parent.SO = soVal
                    parent.RN = rnVal
                    parent.SN = snVal
                    parent.UnderlineStyle = underline
                    parent.IsCategory = True
                    parent.AccountID = accId
                    If accId > 0 Then
                        Try
                            Dim info = service.GetAccountInfo(accId)
                            parent.AccountName = info.Item2
                            Dim chain = service.GetAccountHierarchyChain(accId)
                            Dim codeParts = chain.Select(Function(c) c.Item1).ToArray()
                            parent.AccountCode = String.Join(".", codeParts)
                        Catch
                        End Try
                    End If

                    Dim dv As New DataView(allMappings)
                    dv.RowFilter = "CategoryID = " & catId
                    For Each row As DataRowView In dv
                        Dim child As New PLNode()
                        child.AccountID = Convert.ToInt32(row("AccountID"))
                        child.AccountCode = Convert.ToString(row("AccountCode"))
                        child.AccountName = Convert.ToString(row("AccountName"))
                        child.IsCategory = False

                        parent.Children.Add(child)
                    Next

                    _rootNodes.Add(parent)
                Next

                BuildAndRefreshGrid()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub AlignColumnLetters()
            If dgvReports Is Nothing OrElse _columnLabels Is Nothing OrElse _columnLabels.Count = 0 Then Return

            pnlColumnLetters.SuspendLayout()

            Dim visibleLabelIndex As Integer = 0
            For colIndex As Integer = 0 To dgvReports.Columns.Count - 1
                Dim col = dgvReports.Columns(colIndex)
                If col.Visible Then
                    Dim rect = dgvReports.GetCellDisplayRectangle(colIndex, -1, True)
                    If visibleLabelIndex < _columnLabels.Count Then
                        Dim lbl = _columnLabels(visibleLabelIndex)
                        If rect.Width > 0 Then
                            lbl.Left = rect.X
                            lbl.Width = rect.Width
                            lbl.Visible = True
                        Else
                            lbl.Visible = False
                        End If
                        visibleLabelIndex += 1
                    End If
                End If
            Next

            ' Hide any remaining labels
            For i As Integer = visibleLabelIndex To _columnLabels.Count - 1
                _columnLabels(i).Visible = False
            Next

            pnlColumnLetters.ResumeLayout()
        End Sub

        Private Sub ApplyRowStyle(row As DataGridViewRow, node As PLNode)
            If node Is Nothing Then Return
            If node.IsCategory Then
                row.Cells("colCategory").ReadOnly = False
                row.Cells("colFormula").ReadOnly = False
                row.Cells("colIsMainRow").ReadOnly = False
                row.Cells("colRO").ReadOnly = False
                row.Cells("colSO").ReadOnly = False
                row.Cells("colUnderlineStyle").ReadOnly = False
                row.Cells("colRN").ReadOnly = False
                row.Cells("colSN").ReadOnly = False
                row.Cells("colAdd").Value = "کد سرفصل"
                row.Cells("colAdd").ReadOnly = False

                Dim fName = If(node.IsMainRow, cmbFontMainRow.Text, cmbFontDetailRow.Text)
                Dim fSize = CSng(If(node.IsMainRow, numSizeMainRow.Value, numSizeDetailRow.Value))
                Dim fStyle = If(node.IsMainRow, FontStyle.Bold, FontStyle.Regular)

                Try
                    row.DefaultCellStyle.Font = New Font(fName, fSize, fStyle)
                Catch
                    row.DefaultCellStyle.Font = New Font(dgvReports.Font.FontFamily, fSize, fStyle)
                End Try

                row.DefaultCellStyle.BackColor = Color.FromArgb(235, 243, 255)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 50, 100)
            Else
                row.Cells("colCategory").ReadOnly = True
                row.Cells("colFormula").ReadOnly = True
                row.Cells("colIsMainRow").ReadOnly = True
                row.Cells("colRO").ReadOnly = True
                row.Cells("colSO").ReadOnly = True
                row.Cells("colUnderlineStyle").ReadOnly = True
                row.Cells("colRN").ReadOnly = True
                row.Cells("colSN").ReadOnly = True

                row.Cells("colAdd") = New DataGridViewTextBoxCell()
                row.Cells("colAdd").Value = ""
                row.Cells("colEditFormula") = New DataGridViewTextBoxCell()
                row.Cells("colEditFormula").Value = ""

                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.Font = New Font(dgvReports.Font, FontStyle.Regular)
                row.DefaultCellStyle.ForeColor = Color.Black
            End If
        End Sub

        Private Sub BuildAndRefreshGrid()
            If dgvReports Is Nothing Then Return

            dgvReports.SuspendLayout()
            dgvReports.Rows.Clear()

            Dim displayList As New List(Of PLNode)()
            For Each root In _rootNodes
                displayList.Add(root)
                If root.IsExpanded Then
                    For Each child In root.Children
                        displayList.Add(child)
                    Next
                End If
            Next

            For i As Integer = 0 To displayList.Count - 1
                Dim node = displayList(i)
                Dim rowIdx = dgvReports.Rows.Add()
                Dim row = dgvReports.Rows(rowIdx)
                row.Tag = node

                row.Cells("colToggle").Value = If(node.IsCategory, If(node.IsExpanded, "－", "＋"), "")
                row.Cells("colRowNo").Value = i + 1
                row.Cells("colCategory").Value = If(node.IsCategory, node.CategoryName, "")
                row.Cells("colIsMainRow").Value = If(node.IsCategory, If(node.IsMainRow, "اصلی", "جزئی"), "جزئی")
                row.Cells("colRO").Value = If(node.IsCategory, node.RO, "")
                row.Cells("colSO").Value = If(node.IsCategory, node.SO, "")
                row.Cells("colResult").Value = "" ' Readonly cell
                row.Cells("colUnderlineStyle").Value = If(node.IsCategory, If(String.IsNullOrEmpty(node.UnderlineStyle), "بدون خط", node.UnderlineStyle), "بدون خط")
                row.Cells("colRN").Value = If(node.IsCategory, node.RN, "")
                row.Cells("colSN").Value = If(node.IsCategory, node.SN, "")

                row.Cells("colCode").Value = node.AccountCode
                row.Cells("colName").Value = node.AccountName
                row.Cells("colFormula").Value = node.Formula
                row.Cells("colID").Value = node.AccountID

                ApplyRowStyle(row, node)
            Next

            dgvReports.ResumeLayout()
            CalculateLiveFormulas()
            dgvReports.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            UpdateReportsChainLabel()
            AlignColumnLetters()
        End Sub

        Private Sub DgvReports_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellContentClick
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return

            Dim colName = dgvReports.Columns(e.ColumnIndex).Name

            If colName = "colToggle" AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            End If
        End Sub

        Private Sub DgvReports_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellDoubleClick
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            End If
        End Sub

        Private Sub dgvReports_SelectionChanged(sender As Object, e As EventArgs) Handles dgvReports.SelectionChanged
            UpdateReportsChainLabel()
        End Sub

        Private Sub UpdateReportsChainLabel()
            If lblAccountTitle Is Nothing Then Return
            If dgvReports.CurrentRow Is Nothing Then
                lblAccountTitle.Text = "طراحی گزارشات دلخواه"
                Return
            End If

            Dim row = dgvReports.CurrentRow
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing OrElse node.AccountID <= 0 Then
                lblAccountTitle.Text = "طراحی گزارشات دلخواه"
                Return
            End If

            Try
                Dim chain = service.GetAccountHierarchyChain(node.AccountID)
                Dim parts As New List(Of String)()
                For Each item In chain
                    parts.Add(item.Item1 & " — " & item.Item2)
                Next
                lblAccountTitle.Text = "سرفصل حساب :  " & String.Join("  /  ", parts.ToArray())
            Catch
                lblAccountTitle.Text = "طراحی گزارشات دلخواه"
            End Try
        End Sub

        Private Sub BtnAddCategoryRow_Click(sender As Object, e As EventArgs) Handles btnAddToCategories.Click
            Dim newCat As New PLNode()
            newCat.CategoryID = 0
            newCat.CategoryName = ""
            newCat.Formula = ""
            newCat.IsMainRow = False
            newCat.RO = ""
            newCat.SO = ""
            newCat.RN = ""
            newCat.SN = ""
            newCat.UnderlineStyle = "بدون خط"
            newCat.IsCategory = True
            newCat.IsExpanded = True

            Dim insertIndex As Integer = -1
            If dgvReports.CurrentRow IsNot Nothing Then
                Dim currentNode = TryCast(dgvReports.CurrentRow.Tag, PLNode)
                If currentNode IsNot Nothing Then
                    If currentNode.IsCategory Then
                        insertIndex = _rootNodes.IndexOf(currentNode)
                    Else
                        ' Find parent category
                        For i As Integer = 0 To _rootNodes.Count - 1
                            If _rootNodes(i).Children.Contains(currentNode) Then
                                insertIndex = i
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            If insertIndex >= 0 AndAlso insertIndex < _rootNodes.Count Then
                _rootNodes.Insert(insertIndex + 1, newCat)
            Else
                _rootNodes.Add(newCat)
            End If

            BuildAndRefreshGrid()

            For i As Integer = 0 To dgvReports.Rows.Count - 1
                Dim row = dgvReports.Rows(i)
                Dim node = TryCast(row.Tag, PLNode)
                If node Is newCat Then
                    dgvReports.CurrentCell = row.Cells("colCategory")
                    dgvReports.BeginEdit(True)
                    Exit For
                End If
            Next
        End Sub

        Private Sub BtnDeleteCategoryRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
            If dgvReports.CurrentRow Is Nothing Then Return
            Dim row = dgvReports.CurrentRow
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return

            If node.IsCategory Then
                If MessageBox.Show("آیا مطمئن هستید که می‌خواهید بخش '" & node.CategoryName & "' را به همراه تمام حساب‌های متصل به آن حذف کنید؟", "تایید حذف بخش", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    _rootNodes.Remove(node)
                    BuildAndRefreshGrid()
                End If
            Else
                ' Account row - delete mapping
                If MessageBox.Show("آیا مطمئن هستید که می‌خواهید اتصال حساب '" & node.AccountName & "' را از این دسته قطع کنید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    For Each root In _rootNodes
                        If root.Children.Contains(node) Then
                            root.Children.Remove(node)
                            Exit For
                        End If
                    Next
                    BuildAndRefreshGrid()
                End If
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim code = txtCode.Text.Trim()
            Dim name = txtName.Text.Trim()

            If String.IsNullOrEmpty(code) Then
                MessageBox.Show("لطفاً کد گزارش را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCode.Focus()
                Return
            End If
            If String.IsNullOrEmpty(name) Then
                MessageBox.Show("لطفاً نام گزارش را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If

            Try
                Dim dtoList As New List(Of PLNodeDto)()
                For Each root In _rootNodes
                    ' Skip completely empty rows
                    If String.IsNullOrEmpty(root.CategoryName.Trim()) AndAlso String.IsNullOrEmpty(root.Formula.Trim()) AndAlso root.Children.Count = 0 AndAlso String.IsNullOrEmpty(root.RO) AndAlso String.IsNullOrEmpty(root.SO) AndAlso String.IsNullOrEmpty(root.RN) AndAlso String.IsNullOrEmpty(root.SN) Then
                        Continue For
                    End If

                    Dim dto As New PLNodeDto()
                    dto.AccountID = root.AccountID
                    dto.CategoryName = root.CategoryName
                    dto.Formula = root.Formula
                    dto.IsMainRow = root.IsMainRow
                    dto.RO = root.RO
                    dto.SO = root.SO
                    dto.RN = root.RN
                    dto.SN = root.SN
                    dto.UnderlineStyle = root.UnderlineStyle
                    For Each child In root.Children
                        dto.AccountIDs.Add(child.AccountID)
                    Next
                    dtoList.Add(dto)
                Next

                Dim newId = service.SaveProfitLossFormat(
                    _reportId, code, name, SessionContext.CurrentCompanyID.Value, dtoList,
                    cmbFontHeader.Text, numSizeHeader.Value,
                    cmbFontMainRow.Text, numSizeMainRow.Value,
                    cmbFontDetailRow.Text, numSizeDetailRow.Value,
                    cmbFontFormula.Text, numSizeFormula.Value,
                    Convert.ToInt32(numRowCount.Value), Convert.ToInt32(numColCount.Value),
                    cmbOrientation.Text,
                    numMarginTop.Value, numMarginBottom.Value,
                    numMarginLeft.Value, numMarginRight.Value,
                    cmbPageBorder.Text
                )
                _reportId = newId

                MessageBox.Show("فرمت گزارش با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                RaiseEvent SaveCompleted(Me, EventArgs.Empty)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره فرمت گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
            RaiseEvent ExitRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub dgvReports_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellEndEdit
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                Dim colName = dgvReports.Columns(e.ColumnIndex).Name
                If colName = "colCategory" Then
                    node.CategoryName = Convert.ToString(row.Cells("colCategory").Value).Trim()
                ElseIf colName = "colFormula" Then
                    node.Formula = Convert.ToString(row.Cells("colFormula").Value).Trim()
                ElseIf colName = "colRO" Then
                    node.RO = Convert.ToString(row.Cells("colRO").Value).Trim()
                ElseIf colName = "colSO" Then
                    node.SO = Convert.ToString(row.Cells("colSO").Value).Trim()
                ElseIf colName = "colRN" Then
                    node.RN = Convert.ToString(row.Cells("colRN").Value).Trim()
                ElseIf colName = "colSN" Then
                    node.SN = Convert.ToString(row.Cells("colSN").Value).Trim()
                ElseIf colName = "colUnderlineStyle" Then
                    node.UnderlineStyle = Convert.ToString(row.Cells("colUnderlineStyle").Value).Trim()
                End If
            End If
        End Sub

        Private Sub dgvReports_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            Dim colName = dgvReports.Columns(e.ColumnIndex).Name
            If colName = "colEditFormula" Then
                Dim row = dgvReports.Rows(e.RowIndex)
                Dim node = TryCast(row.Tag, PLNode)
                If node IsNot Nothing AndAlso node.IsCategory Then
                    ShowFormulaEditor(e.RowIndex)
                End If
            ElseIf colName = "colAdd" Then
                Dim row = dgvReports.Rows(e.RowIndex)
                Dim node = TryCast(row.Tag, PLNode)
                If node IsNot Nothing Then
                    _sarfaslTargetRow = e.RowIndex
                    ShowCodingFormForSelection()
                End If
            End If
        End Sub

        Private Sub ShowCodingFormForSelection()
            Using codingForm As New HesabdaryCodingForm()
                codingForm.SelectMode = True
                codingForm.ReportSelectionMode = True
                codingForm.Size = New Size(760, 380)
                codingForm.StartPosition = FormStartPosition.CenterParent

                codingForm.ShowDialog(Me)

                If codingForm.SelectedAccountID.HasValue AndAlso _sarfaslTargetRow >= 0 Then
                    Dim targetRow = dgvReports.Rows(_sarfaslTargetRow)
                    Dim node = TryCast(targetRow.Tag, PLNode)

                    If node IsNot Nothing Then
                        node.AccountID = codingForm.SelectedAccountID.Value
                        Dim info = service.GetAccountInfo(codingForm.SelectedAccountID.Value)
                        node.AccountName = info.Item2

                        Dim chain = service.GetAccountHierarchyChain(codingForm.SelectedAccountID.Value)
                        Dim codeParts = chain.Select(Function(c) c.Item1).ToArray()
                        Dim codeStr As String = String.Join(".", codeParts)
                        node.AccountCode = codeStr

                        targetRow.Cells("colCode").Value = codeStr
                        targetRow.Cells("colName").Value = info.Item2
                        targetRow.Cells("colID").Value = node.AccountID
                    End If

                    dgvReports.InvalidateRow(_sarfaslTargetRow)
                End If
                _sarfaslTargetRow = -1
            End Using
        End Sub

        Private Sub ShowFormulaEditor(rowIndex As Integer)
            Dim row = dgvReports.Rows(rowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing OrElse Not node.IsCategory Then Return

            Dim currentFormula = Convert.ToString(row.Cells("colFormula").Value)

            ' Create Popup Form
            Dim popupForm As New Form()
            popupForm.Text = "ویرایش فرمول محاسبه"
            popupForm.Size = New Size(700, 500)
            popupForm.StartPosition = FormStartPosition.CenterParent
            popupForm.FormBorderStyle = FormBorderStyle.FixedDialog
            popupForm.MaximizeBox = False
            popupForm.MinimizeBox = False
            popupForm.RightToLeft = RightToLeft.Yes
            popupForm.RightToLeftLayout = True
            popupForm.Font = New Font("Tahoma", 9.0!)

            ' Help Button
            Dim btnHelp As New Button()
            btnHelp.Text = "راهنما"
            btnHelp.Size = New Size(120, 35)
            btnHelp.Location = New Point(30, 20)
            btnHelp.BackColor = Color.FromArgb(235, 243, 255)
            btnHelp.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            popupForm.Controls.Add(btnHelp)

            AddHandler btnHelp.Click, Sub(s, ev)
                                          ShowFormulaHelp()
                                      End Sub

            ' Label Title
            Dim lblTitle As New Label()
            lblTitle.Text = "فرمول فعلی ردیف :"
            lblTitle.Location = New Point(30, 70)
            lblTitle.Size = New Size(140, 30)
            lblTitle.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            popupForm.Controls.Add(lblTitle)

            ' Label Formula
            Dim lblFormula As New Label()
            lblFormula.Text = If(String.IsNullOrEmpty(currentFormula), "(خالی)", currentFormula)
            lblFormula.Location = New Point(170, 70)
            lblFormula.Size = New Size(480, 30)
            lblFormula.Font = New Font("Consolas", 11.0!, FontStyle.Bold)
            lblFormula.RightToLeft = RightToLeft.No
            lblFormula.TextAlign = ContentAlignment.MiddleLeft
            popupForm.Controls.Add(lblFormula)
            ' Textbox
            Dim txtFormula As New TextBox()
            txtFormula.Text = currentFormula
            txtFormula.Location = New Point(30, 110)
            txtFormula.Size = New Size(620, 240)
            txtFormula.Multiline = True
            txtFormula.ScrollBars = ScrollBars.Vertical
            txtFormula.Font = New Font("Consolas", 11.0!)
            txtFormula.RightToLeft = RightToLeft.No
            popupForm.Controls.Add(txtFormula)

            ' Buttons Panel
            Dim pnlButtons As New Panel()
            pnlButtons.Location = New Point(30, 380)
            pnlButtons.Size = New Size(620, 50)
            popupForm.Controls.Add(pnlButtons)

            ' Save Button
            Dim btnSave As New Button()
            btnSave.Text = "ذخیره"
            btnSave.DialogResult = DialogResult.OK
            btnSave.Size = New Size(110, 35)
            btnSave.Location = New Point(510, 5)
            btnSave.BackColor = Color.FromArgb(200, 240, 200)
            pnlButtons.Controls.Add(btnSave)

            ' Cancel Button
            Dim btnCancel As New Button()
            btnCancel.Text = "انصراف"
            btnCancel.DialogResult = DialogResult.Cancel
            btnCancel.Size = New Size(110, 35)
            btnCancel.Location = New Point(380, 5)
            btnCancel.BackColor = Color.FromArgb(240, 200, 200)
            pnlButtons.Controls.Add(btnCancel)

            popupForm.AcceptButton = btnSave
            popupForm.CancelButton = btnCancel

            If popupForm.ShowDialog(Me) = DialogResult.OK Then
                Dim newFormula = txtFormula.Text.Trim()
                row.Cells("colFormula").Value = newFormula
                node.Formula = newFormula
                dgvReports.NotifyCurrentCellDirty(True)
                CalculateLiveFormulas()
            End If
        End Sub

        Private Function GetFormulaHelpText() As String
            Try
                Dim path As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FormulaHelp.txt")
                If System.IO.File.Exists(path) Then
                    Return System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8)
                End If
            Catch
            End Try

            ' Default text
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine("راهنمای فرمول‌نویسی گزارشات دلخواه")
            sb.AppendLine("________________________________________")
            sb.AppendLine("۱. مقادیر پایه ردیف‌ها")
            sb.AppendLine("اگر برای یک ردیف فرمولی ننویسید، سیستم به طور خودکار جمع مانده حساب‌هایی که به آن ردیف متصل کرده‌اید (در زیرمجموعه آن قرار داده‌اید) را محاسبه کرده و به عنوان مقدار آن ردیف در نظر می‌گیرد.")
            sb.AppendLine()
            sb.AppendLine("۲. ارجاع به نتیجه سایر ردیف‌ها")
            sb.AppendLine("برای استفاده از مقدار محاسبه شده یک ردیف در فرمول ردیفی دیگر، باید شماره آن ردیف (شماره سطر) را داخل کروشه [ ] قرار دهید:")
            sb.AppendLine("•  فرمت: [شماره ردیف]")
            sb.AppendLine("•  مثال: [1] (مقدار سطر ۱)، [12] (مقدار سطر ۱۲)")
            sb.AppendLine("•  مثال در فرمول:  =[1] - [2]  (کاهش مقدار سطر ۲ از سطر ۱)")
            sb.AppendLine()
            sb.AppendLine("۳. عملگرهای ریاضی مجاز و اولویت‌ها")
            sb.AppendLine("فرمول‌ها از عملگرهای استاندارد ریاضی و پرانتز پشتیبانی می‌کنند:")
            sb.AppendLine("•  + (جمع)")
            sb.AppendLine("•  - (تفریق)")
            sb.AppendLine("•  * (ضرب)")
            sb.AppendLine("•  / (تقسیم)")
            sb.AppendLine("•  () (پرانتز برای تعیین اولویت محاسبات)")
            sb.AppendLine("________________________________________")
            sb.AppendLine("چند نمونه فرمول کاربردی:")
            sb.AppendLine("•  فرمول ۱: محاسبه سود ناویژه (فرض کنید فروش در سطر ۲ و بهای تمام شده در سطر ۳ است):  =[2] - [3]")
            sb.AppendLine("•  فرمول ۲: مجموع چند سطر:  =[1] + [4] + [7]")
            sb.AppendLine("•  فرمول ۳: محاسبه درصدی (مثلاً ۵ درصد از مبلغ سطر ۸):  =[8] * 0.05")
            sb.AppendLine("•  فرمول ۴: محاسبات ترکیبی با پرانتز:  =([5] + [6]) - [2]")
            sb.AppendLine("________________________________________")
            sb.AppendLine("نکات مهم:")
            sb.AppendLine("- فرمول می‌تواند با علامت مساوی (=) شروع شود.")
            sb.AppendLine("- در این روش، نیازی نیست کد حساب‌ها را به صورت دستی در فرمول تایپ کنید. فقط کافیست حساب(های) مورد نظر را به عنوان زیرمجموعه به یک سطر اضافه کنید و سپس در فرمول به شماره آن سطر ارجاع دهید. این کار باعث می‌شود فرمول‌ها بسیار خوانا، کوتاه و عاری از خطای تایپی باشند.")
            Return sb.ToString()
        End Function

        Private Sub SaveFormulaHelpText(text As String)
            Try
                Dim path As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FormulaHelp.txt")
                System.IO.File.WriteAllText(path, text, System.Text.Encoding.UTF8)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی متن راهنما: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub ShowFormulaHelpEditor()
            Dim path As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FormulaHelp.txt")
            Try
                If Not System.IO.File.Exists(path) Then
                    System.IO.File.WriteAllText(path, GetFormulaHelpText(), System.Text.Encoding.UTF8)
                End If

                ' Open in Microsoft Word (winword.exe)
                System.Diagnostics.Process.Start("winword.exe", """" & path & """")
            Catch ex As Exception
                ' Fallback: open with the default text editor associated in Windows (e.g. Notepad)
                Try
                    System.Diagnostics.Process.Start(path)
                Catch exInner As Exception
                    MessageBox.Show("خطا در باز کردن فایل راهنما: " & exInner.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Try
        End Sub

        Private Sub ShowFormulaHelp()
            Dim helpForm As New Form()
            helpForm.Text = "راهنمای فرمول‌نویسی گزارشات دلخواه"
            helpForm.Size = New Size(600, 520)
            helpForm.StartPosition = FormStartPosition.CenterParent
            helpForm.FormBorderStyle = FormBorderStyle.FixedDialog
            helpForm.MaximizeBox = False
            helpForm.MinimizeBox = False
            helpForm.RightToLeft = RightToLeft.Yes
            helpForm.RightToLeftLayout = True
            helpForm.Font = New Font("Tahoma", 9.0!)

            Dim txtHelp As New TextBox()
            txtHelp.Dock = DockStyle.Fill
            txtHelp.Multiline = True
            txtHelp.ReadOnly = True
            txtHelp.BackColor = Color.White
            txtHelp.Font = New Font("Tahoma", 9.5!)
            txtHelp.ScrollBars = ScrollBars.Vertical
            txtHelp.Text = GetFormulaHelpText()
            txtHelp.SelectionStart = 0
            txtHelp.SelectionLength = 0
            helpForm.Controls.Add(txtHelp)

            helpForm.ShowDialog(Me)
        End Sub

        Private Sub BtnEditHelp_Click(sender As Object, e As EventArgs) Handles btnEditHelp.Click
            ShowFormulaHelpEditor()
        End Sub

        Private Sub DgvReports_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellValueChanged
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return

            Dim colName = dgvReports.Columns(e.ColumnIndex).Name
            If colName = "colIsMainRow" Then
                Dim cellVal = Convert.ToString(row.Cells("colIsMainRow").Value)
                node.IsMainRow = (cellVal = "اصلی")
                ApplyRowStyle(row, node)
                CalculateLiveFormulas()
            ElseIf colName = "colUnderlineStyle" Then
                node.UnderlineStyle = Convert.ToString(row.Cells("colUnderlineStyle").Value)
            ElseIf colName = "colFormula" OrElse colName = "colCode" Then
                If colName = "colFormula" Then node.Formula = Convert.ToString(row.Cells("colFormula").Value)
                CalculateLiveFormulas()
            End If
        End Sub

        Private Sub DgvReports_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvReports.CurrentCellDirtyStateChanged
            If dgvReports.IsCurrentCellDirty AndAlso (TypeOf dgvReports.CurrentCell Is DataGridViewCheckBoxCell OrElse TypeOf dgvReports.CurrentCell Is DataGridViewComboBoxCell) Then
                dgvReports.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End Sub

        Private Sub FontsSettings_Changed(sender As Object, e As EventArgs) Handles cmbFontMainRow.SelectedIndexChanged, numSizeMainRow.ValueChanged, cmbFontDetailRow.SelectedIndexChanged, numSizeDetailRow.ValueChanged, cmbFontFormulaDetail.SelectedIndexChanged, numSizeFormulaDetail.ValueChanged
            If dgvReports Is Nothing Then Return
            For Each row As DataGridViewRow In dgvReports.Rows
                Dim node = TryCast(row.Tag, PLNode)
                If node IsNot Nothing AndAlso node.IsCategory Then
                    ApplyRowStyle(row, node)
                End If
            Next
        End Sub

        Private Sub BtnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
            If _reportId <= 0 Then
                MessageBox.Show("لطفاً ابتدا گزارش را ذخیره کنید تا شناسه گزارش ایجاد شود.", "پیام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using printForm As New HesabdaryReportPrintForm(_reportId)
                printForm.ShowDialog(Me)
            End Using
        End Sub

        Private Sub DgvReports_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvReports.Scroll
            AlignColumnLetters()
        End Sub

        Private Sub DgvReports_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvReports.ColumnWidthChanged
            AlignColumnLetters()
        End Sub

        Private Sub DgvReports_Resize(sender As Object, e As EventArgs) Handles dgvReports.Resize
            AlignColumnLetters()
        End Sub

        Private Sub DgvReports_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvReports.CellFormatting
            If e.ColumnIndex = colFormula.Index AndAlso e.Value IsNot Nothing Then
                Dim valStr = e.Value.ToString()
                If Not String.IsNullOrEmpty(valStr) Then
                    ' Prepend LTR Override character (U+202D) to force left-to-right rendering
                    e.Value = Convert.ToChar(&H202D) & valStr & Convert.ToChar(&H202C)
                    e.FormattingApplied = True
                End If
            End If
        End Sub

        Private Class PLNode
            Public CategoryID As Integer
            Public Key As String
            Public CategoryName As String
            Public Formula As String
            Public IsMainRow As Boolean
            Public RO As String
            Public SO As String
            Public RN As String
            Public SN As String
            Public UnderlineStyle As String
            Public AccountID As Integer
            Public AccountCode As String
            Public AccountName As String
            Public IsCategory As Boolean
            Public IsExpanded As Boolean = True
            Public Children As New List(Of PLNode)()
            Public BaseValue As Decimal = 0
            Public FinalValue As Decimal = 0
        End Class

        Private Sub UpdateCalculatedDimensions()
            If cmbPaperSize Is Nothing OrElse cmbPaperSize.SelectedIndex < 0 Then Return
            Dim sizeName = cmbPaperSize.SelectedItem.ToString()
            Dim w As Decimal = 210
            Dim h As Decimal = 297
            Select Case sizeName
                Case "A3"
                    w = 297
                    h = 420
                Case "A5"
                    w = 148
                    h = 210
                Case "Letter"
                    w = 216
                    h = 279
                Case "A4"
                    w = 210
                    h = 297
            End Select

            If cmbOrientation.SelectedItem IsNot Nothing AndAlso cmbOrientation.SelectedItem.ToString() = "افقی" Then
                Dim temp = w
                w = h
                h = temp
            End If

            Dim a As Decimal = w - (numMarginLeft.Value + numMarginRight.Value)
            Dim b As Decimal = h - (numMarginTop.Value + numMarginBottom.Value)

            lblL1.Text = "عرض متن گزارش به جز حاشیه : " & a.ToString("0.##") & " میلیمتر"
            lblL2.Text = "ارتفاع متن گزارش به جز حاشیه : " & b.ToString("0.##") & " میلیمتر"
        End Sub

        Private Sub DimensionParams_Changed(sender As Object, e As EventArgs) Handles cmbPaperSize.SelectedIndexChanged, cmbOrientation.SelectedIndexChanged, numMarginTop.ValueChanged, numMarginBottom.ValueChanged, numMarginLeft.ValueChanged, numMarginRight.ValueChanged
            UpdateCalculatedDimensions()
        End Sub

        Private Sub lblName_Click(sender As Object, e As EventArgs) Handles lblName.Click

        End Sub

        Private Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
            Dim hasGlobalAccounting = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.ManageAccounting)
            btnPrintReport.Visible = hasGlobalAccounting OrElse SessionContext.HasPermission(PermissionKeys.AccountingCustomReportPrint)
        End Sub
    End Class

    Public Class DataGridViewNumericUpDownColumn
        Inherits DataGridViewColumn

        Public Sub New()
            MyBase.New(New DataGridViewNumericUpDownCell())
            Me.DefaultCellStyle.Padding = New Padding(0, 0, 18, 0)
        End Sub

        Public Overrides Property CellTemplate As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get
            Set(value As DataGridViewCell)
                If value IsNot Nothing AndAlso Not TypeOf value Is DataGridViewNumericUpDownCell Then
                    Throw New InvalidCastException("Cell template must be a DataGridViewNumericUpDownCell.")
                End If
                MyBase.CellTemplate = value
            End Set
        End Property
    End Class

    Public Class DataGridViewNumericUpDownCell
        Inherits DataGridViewTextBoxCell

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Sub InitializeEditingControl(rowIndex As Integer, initialFormattedValue As Object, dataGridViewCellStyle As DataGridViewCellStyle)
            MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
            Dim ctl As DataGridViewNumericUpDownEditingControl = TryCast(DataGridView.EditingControl, DataGridViewNumericUpDownEditingControl)
            If ctl IsNot Nothing Then
                Dim cellVal = Me.Value
                If cellVal IsNot Nothing AndAlso Not Convert.IsDBNull(cellVal) AndAlso Not String.IsNullOrEmpty(cellVal.ToString()) Then
                    Dim val As Decimal = 0
                    If Decimal.TryParse(cellVal.ToString(), val) Then
                        ctl.Value = Math.Max(ctl.Minimum, Math.Min(ctl.Maximum, val))
                    Else
                        ctl.Value = ctl.Minimum
                    End If
                Else
                    ctl.Value = ctl.Minimum
                End If
            End If
        End Sub

        Public Overrides ReadOnly Property EditType As Type
            Get
                Return GetType(DataGridViewNumericUpDownEditingControl)
            End Get
        End Property

        Public Overrides ReadOnly Property ValueType As Type
            Get
                Return GetType(Object)
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultNewRowValue As Object
            Get
                Return ""
            End Get
        End Property

        Protected Overrides Sub Paint(graphics As Graphics, clipBounds As Rectangle, cellBounds As Rectangle, rowIndex As Integer, cellState As DataGridViewElementStates, value As Object, formattedValue As Object, errorText As String, cellStyle As DataGridViewCellStyle, advancedBorderStyle As DataGridViewAdvancedBorderStyle, paintParts As DataGridViewPaintParts)
            ' Paint the cell normally (textbox style)
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)
            
            ' Draw the up/down scroll buttons at the right side of the cell if it's not read-only
            If Not Me.ReadOnly AndAlso (paintParts And DataGridViewPaintParts.ContentForeground) <> 0 Then
                Dim buttonWidth As Integer = 16
                Dim buttonHeight As Integer = (cellBounds.Height - 2) \ 2
                If buttonHeight > 0 Then
                    Dim upButtonRect As New Rectangle(cellBounds.Right - buttonWidth - 2, cellBounds.Top + 1, buttonWidth, buttonHeight)
                    Dim downButtonRect As New Rectangle(cellBounds.Right - buttonWidth - 2, cellBounds.Top + buttonHeight + 1, buttonWidth, buttonHeight)
                    
                    ControlPaint.DrawScrollButton(graphics, upButtonRect, ScrollButton.Up, ButtonState.Normal)
                    ControlPaint.DrawScrollButton(graphics, downButtonRect, ScrollButton.Down, ButtonState.Normal)
                End If
            End If
        End Sub
    End Class

    Public Class DataGridViewNumericUpDownEditingControl
        Inherits NumericUpDown
        Implements IDataGridViewEditingControl

        Private dataGridViewControl As DataGridView
        Private _valueHasChanged As Boolean = False
        Private rowIndexNum As Integer

        Public Sub New()
            MyBase.New()
            Me.Minimum = 0
            Me.Maximum = 1000
            Me.DecimalPlaces = 0
        End Sub

        Public Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
            Get
                If Me.Value = 0 Then
                    Return ""
                End If
                Return Me.Value.ToString()
            End Get
            Set(value As Object)
                If value IsNot Nothing AndAlso Not String.IsNullOrEmpty(value.ToString()) Then
                    Dim val As Decimal = 0
                    If Decimal.TryParse(value.ToString(), val) Then
                        Me.Value = Math.Max(Me.Minimum, Math.Min(Me.Maximum, val))
                    Else
                        Me.Value = Me.Minimum
                    End If
                Else
                    Me.Value = Me.Minimum
                End If
            End Set
        End Property

        Public Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
            Return EditingControlFormattedValue
        End Function

        Public Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
            Me.Font = dataGridViewCellStyle.Font
            Me.ForeColor = dataGridViewCellStyle.ForeColor
            Me.BackColor = dataGridViewCellStyle.BackColor
        End Sub

        Public Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
            Get
                Return dataGridViewControl
            End Get
            Set(value As DataGridView)
                dataGridViewControl = value
            End Set
        End Property

        Public Property EditingControlRowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
            Get
                Return rowIndexNum
            End Get
            Set(value As Integer)
                rowIndexNum = value
            End Set
        End Property

        Public Property EditingControlValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
            Get
                Return _valueHasChanged
            End Get
            Set(value As Boolean)
                _valueHasChanged = value
            End Set
        End Property

        Public ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
            Get
                Return MyBase.Cursor
            End Get
        End Property

        Public ReadOnly Property RepositionEditingControlOnValueChange As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
            Get
                Return False
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(e As EventArgs)
            MyBase.OnValueChanged(e)
            _valueHasChanged = True
            Me.dataGridViewControl.NotifyCurrentCellDirty(True)
        End Sub

        Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
            MyBase.OnKeyPress(e)
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Public Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
            Select Case keyData And Keys.KeyCode
                Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.End, Keys.PageDown, Keys.PageUp
                    Return True
                Case Else
                    Return Not dataGridViewWantsInputKey
            End Select
        End Function

        Public Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
        End Sub
    End Class

End Namespace
