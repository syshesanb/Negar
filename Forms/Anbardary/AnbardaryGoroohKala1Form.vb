Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Negar.Business

Namespace Negar.Forms
    Public Class AnbardaryGoroohKala1Form
        Inherits Form

        Private ReadOnly _service As New ProductGroupService()
        
        ' Tree state variables
        Private _rootNodes As New List(Of GroupNode)()
        Private _nodeDict As New Dictionary(Of Integer, GroupNode)()

        ' Const button column names
        Private Const ColBtnEdit As String = "colBtnEdit"
        Private Const ColBtnDelete As String = "colBtnDelete"
        Private Const ColBtnAddChild As String = "colBtnAddChild"

        ' Tree node class
        Private NotInheritable Class GroupNode
            Public GroupID As Integer
            Public ParentID As Integer?
            Public Level As Integer
            Public GroupCode As String
            Public GroupName As String
            Public IsActive As Boolean
            Public IsExpanded As Boolean
            Public ReadOnly Children As New List(Of GroupNode)()

            Public ReadOnly Property HasChildren As Boolean
                Get
                    Return Children.Count > 0
                End Get
            End Property
        End Class

        Private _isSelectMode As Boolean = False
        Public SelectedGroupID As Integer = 0
        Public SelectedGroupName As String = ""
        Public SelectedGroupCode As String = ""

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(isSelectMode As Boolean)
            InitializeComponent()
            _isSelectMode = isSelectMode
        End Sub

        Private Sub AnbardaryGoroohKala1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            If _isSelectMode Then
                Me.FormBorderStyle = FormBorderStyle.Sizable
                Me.MaximizeBox = True
                Me.MinimizeBox = False
                Me.StartPosition = FormStartPosition.CenterParent
                Me.Text = "انتخاب گروه کالا"
                Me.Size = New Size(960, 520)
            End If

            SetupGrid()
            LoadData()
            ApplySecurity()

            ' Load configured levels
            Dim companyId = SessionContext.CurrentCompanyID
            Dim levels = _service.GetMaxLevels(companyId)

            cmbExpandToLevel.Items.Clear()
            Dim levelNames As String() = {"گروه اصلی (بستن همه)", "زیرگروه سطح ۱", "زیرگروه سطح ۲", "زیرگروه سطح ۳", "زیرگروه سطح ۴"}
            For i As Integer = 0 To levels - 1
                If i < levelNames.Length Then
                    cmbExpandToLevel.Items.Add(levelNames(i))
                Else
                    cmbExpandToLevel.Items.Add($"زیرگروه سطح {i}")
                End If
            Next

            cmbExpandToLevel.SelectedIndex = 0
            ExpandTreeToLevel(0)

            ' Register event handlers for grid scroll/resize/column width change
            AddHandler dgvGroups.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvGroups.Scroll, AddressOf DgvGroups_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes

            ' Register TextChanged events for search boxes
            AddHandler txtSearchCode.TextChanged, AddressOf TxtSearch_TextChanged
            AddHandler txtSearchName.TextChanged, AddressOf TxtSearch_TextChanged

            AlignSearchBoxes()
        End Sub

        Private Sub SetupGrid()
            dgvGroups.Columns.Clear()
            dgvGroups.AutoGenerateColumns = False
            dgvGroups.RowHeadersVisible = False
            dgvGroups.AllowUserToResizeRows = False
            dgvGroups.RowTemplate.Height = 28
            dgvGroups.ColumnHeadersHeight = 32
            dgvGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

            ' Grid styling
            dgvGroups.EnableHeadersVisualStyles = False
            dgvGroups.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 238, 250)
            dgvGroups.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            dgvGroups.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            dgvGroups.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvGroups.GridColor = Color.FromArgb(224, 224, 224)
            dgvGroups.CellBorderStyle = DataGridViewCellBorderStyle.Single

            ' 1. colToggle (Toggle button + / −)
            Dim colToggle As New DataGridViewTextBoxColumn()
            colToggle.Name = "colToggle"
            colToggle.HeaderText = "+"
            colToggle.Width = 35
            colToggle.ReadOnly = True
            colToggle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colToggle.DefaultCellStyle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)

            ' 2. Edit button column
            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = ColBtnEdit
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 70
            colEdit.FlatStyle = FlatStyle.Standard

            ' 3. Delete button column
            Dim colDel As New DataGridViewButtonColumn()
            colDel.Name = ColBtnDelete
            colDel.HeaderText = "حذف"
            colDel.Text = "حذف"
            colDel.UseColumnTextForButtonValue = True
            colDel.Width = 56
            colDel.FlatStyle = FlatStyle.Standard

            ' 4. Add child button column (+)
            Dim colAddChild As New DataGridViewButtonColumn()
            colAddChild.Name = ColBtnAddChild
            colAddChild.HeaderText = "+"
            colAddChild.Text = "+"
            colAddChild.UseColumnTextForButtonValue = True
            colAddChild.Width = 40
            colAddChild.FlatStyle = FlatStyle.Standard

            ' 5. IsActive (فعال)
            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "colIsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 60
            colActive.ReadOnly = True

            ' Hidden data columns
            Dim colGroupId As New DataGridViewTextBoxColumn()
            colGroupId.Name = "colGroupID"
            colGroupId.Visible = False

            ' 6. GroupCode (کد گروه)
            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colGroupCode"
            colCode.HeaderText = "کد گروه"
            colCode.Width = 140
            colCode.ReadOnly = True
            colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 7. GroupName (نام گروه کالا)
            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colGroupName"
            colName.HeaderText = "نام گروه کالا"
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colName.ReadOnly = True

            If _isSelectMode Then
                Dim colSelect As New DataGridViewButtonColumn()
                colSelect.Name = "colBtnSelect"
                colSelect.HeaderText = "انتخاب"
                colSelect.Text = "انتخاب"
                colSelect.UseColumnTextForButtonValue = True
                colSelect.Width = 70
                colSelect.FlatStyle = FlatStyle.Standard
                
                dgvGroups.Columns.AddRange(New DataGridViewColumn() {
                    colToggle, colSelect, colEdit, colDel, colAddChild, colActive, colGroupId, colCode, colName
                })
            Else
                dgvGroups.Columns.AddRange(New DataGridViewColumn() {
                    colToggle, colEdit, colDel, colAddChild, colActive, colGroupId, colCode, colName
                })
            End If
        End Sub

        Private Sub LoadData()
            Dim dt As DataTable
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                dt = _service.GetAll(companyId)
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات گروه‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            _nodeDict.Clear()
            _rootNodes.Clear()

            For Each row As DataRow In dt.Rows
                Dim node As New GroupNode()
                node.GroupID = Convert.ToInt32(row("GroupID"))
                node.GroupCode = Convert.ToString(row("GroupCode"))
                node.GroupName = Convert.ToString(row("GroupName"))
                node.IsActive = If(row.IsNull("IsActive"), True, Convert.ToInt32(row("IsActive")) = 1)
                node.ParentID = If(row.IsNull("ParentID"), CType(Nothing, Integer?), CType(Convert.ToInt32(row("ParentID")), Integer?))
                node.IsExpanded = True
                _nodeDict(node.GroupID) = node
            Next

            For Each node In _nodeDict.Values
                If node.ParentID.HasValue AndAlso _nodeDict.ContainsKey(node.ParentID.Value) Then
                    _nodeDict(node.ParentID.Value).Children.Add(node)
                Else
                    _rootNodes.Add(node)
                End If
            Next

            SetLevels(_rootNodes, 0)
            RefreshGrid()
        End Sub

        Private Sub SetLevels(nodes As List(Of GroupNode), level As Integer)
            If level > 10 Then Return
            For Each node In nodes
                node.Level = level
                SetLevels(node.Children, level + 1)
            Next
        End Sub

        Private Sub RefreshGrid()
            Try
                If dgvGroups Is Nothing OrElse dgvGroups.Columns.Count = 0 Then Return
                dgvGroups.SuspendLayout()
                dgvGroups.Rows.Clear()

                Dim displayList As New List(Of GroupNode)()
                BuildDisplayList(_rootNodes, displayList)

                For Each node In displayList
                    Dim rowIdx = dgvGroups.Rows.Add()
                    Dim row = dgvGroups.Rows(rowIdx)
                    row.Tag = node

                    row.Cells("colToggle").Value = GetToggleText(node)
                    row.Cells("colGroupID").Value = node.GroupID
                    row.Cells(ColBtnAddChild).Value = "+"
                    row.Cells("colGroupCode").Value = node.GroupCode
                    
                    Dim indentSpaces As String = New String(Convert.ToChar(160), node.Level * 6)
                    row.Cells("colGroupName").Value = indentSpaces & node.GroupName
                    row.Cells("colIsActive").Value = node.IsActive

                    ApplyRowStyle(row, node)
                Next

                dgvGroups.ResumeLayout()
                UpdateSathPanel()
                AlignSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در به‌روزرسانی جدول: " & ex.Message, "خطای جدول", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BuildDisplayList(nodes As List(Of GroupNode), result As List(Of GroupNode))
            Dim codeF = txtSearchCode.Text.Trim()
            Dim nameF = txtSearchName.Text.Trim()
            Dim hasFilter = (codeF.Length > 0 OrElse nameF.Length > 0)

            For Each node In nodes
                Dim matchesSearch = True
                If hasFilter Then
                    If codeF.Length > 0 AndAlso Not node.GroupCode.Contains(codeF) Then matchesSearch = False
                    If nameF.Length > 0 AndAlso Not node.GroupName.Contains(nameF) Then matchesSearch = False
                End If

                Dim showNode = True
                If hasFilter Then
                    showNode = matchesSearch OrElse HasAnyMatchingChild(node, codeF, nameF)
                End If

                If showNode Then
                    result.Add(node)
                    If hasFilter Then
                        node.IsExpanded = True
                    End If

                    If node.IsExpanded AndAlso node.HasChildren Then
                        BuildDisplayList(node.Children, result)
                    End If
                End If
            Next
        End Sub

        Private Function HasAnyMatchingChild(node As GroupNode, codeF As String, nameF As String) As Boolean
            For Each child In node.Children
                Dim matches = True
                If codeF.Length > 0 AndAlso Not child.GroupCode.Contains(codeF) Then matches = False
                If nameF.Length > 0 AndAlso Not child.GroupName.Contains(nameF) Then matches = False
                If matches Then Return True
                If HasAnyMatchingChild(child, codeF, nameF) Then Return True
            Next
            Return False
        End Function

        Private Function GetToggleText(node As GroupNode) As String
            If Not node.HasChildren Then Return ""
            Return If(node.IsExpanded, "−", "+")
        End Function

        Private Sub ApplyRowStyle(row As DataGridViewRow, node As GroupNode)
            Select Case node.Level
                Case 0
                    row.DefaultCellStyle.Font = New Font(dgvGroups.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(195, 218, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(15, 30, 80)
                Case 1
                    row.DefaultCellStyle.Font = New Font(dgvGroups.Font, FontStyle.Bold)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(215, 232, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 100)
                Case 2
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 244, 255)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 60, 120)
                Case 3
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case 4
                    row.DefaultCellStyle.BackColor = Color.FromArgb(252, 254, 255)
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case Else
                    row.DefaultCellStyle.BackColor = Color.White
                    row.DefaultCellStyle.ForeColor = Color.Black
            End Select
        End Sub

        Private Sub DgvGroups_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGroups.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
            If dgvGroups.Columns(e.ColumnIndex).Name = "colToggle" Then
                Dim node = TryCast(dgvGroups.Rows(e.RowIndex).Tag, GroupNode)
                If node IsNot Nothing AndAlso node.HasChildren Then
                    node.IsExpanded = Not node.IsExpanded
                    RefreshGrid()
                End If
            End If
        End Sub

        Private Sub dgvGroups_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGroups.CellContentClick
            If e.RowIndex < 0 Then Return
            Dim row = dgvGroups.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, GroupNode)
            If node Is Nothing Then Return

            Dim colName = dgvGroups.Columns(e.ColumnIndex).Name

            Select Case colName
                Case "colBtnSelect"
                    SelectedGroupID = node.GroupID
                    SelectedGroupName = node.GroupName
                    SelectedGroupCode = node.GroupCode
                    Me.DialogResult = DialogResult.OK
                    Me.Close()

                Case ColBtnEdit
                    Using frm As New AnbardaryGoroohKala2Form(node.GroupID, Nothing)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadData()
                        End If
                    End Using

                Case ColBtnDelete
                    Dim confirm = MessageBox.Show($"آیا از حذف گروه کالا «{node.GroupName}» اطمینان دارید؟",
                                                   "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If confirm = DialogResult.Yes Then
                        Try
                            _service.Delete(node.GroupID)
                            MessageBox.Show("گروه کالا با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show(ex.Message, "خطا در حذف", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If

                Case ColBtnAddChild
                    ' Add child under this group
                    Dim companyId = SessionContext.CurrentCompanyID
                    Dim maxLevels = _service.GetMaxLevels(companyId)
                    If node.Level + 1 >= maxLevels Then
                        MessageBox.Show($"حداکثر سطوح گروه‌بندی برای این شرکت {maxLevels} سطح است. امکان تعریف زیرگروه جدید وجود ندارد.",
                                        "محدودیت سطح", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    Using frm As New AnbardaryGoroohKala2Form(Nothing, node.GroupID)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadData()
                        End If
                    End Using
            End Select
        End Sub

        Private Sub BtnSelect_Click(sender As Object, e As EventArgs)
            If dgvGroups.CurrentRow IsNot Nothing Then
                Dim node = TryCast(dgvGroups.CurrentRow.Tag, GroupNode)
                If node IsNot Nothing Then
                    SelectedGroupID = node.GroupID
                    SelectedGroupName = node.GroupName
                    SelectedGroupCode = node.GroupCode
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If
            End If
        End Sub

        Private Sub dgvGroups_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGroups.CellDoubleClick
            If e.RowIndex >= 0 Then
                Dim colName = dgvGroups.Columns(e.ColumnIndex).Name
                If colName = "colBtnEdit" OrElse colName = "colBtnDelete" OrElse colName = "colBtnAddChild" OrElse colName = "colToggle" Then
                    Return
                End If

                Dim node = TryCast(dgvGroups.Rows(e.RowIndex).Tag, GroupNode)
                If node IsNot Nothing Then
                    If _isSelectMode Then
                        SelectedGroupID = node.GroupID
                        SelectedGroupName = node.GroupName
                        SelectedGroupCode = node.GroupCode
                        Me.DialogResult = DialogResult.OK
                        Me.Close()
                    Else
                        Using frm As New AnbardaryGoroohKala2Form(node.GroupID, Nothing)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadData()
                            End If
                        End Using
                    End If
                End If
            End If
        End Sub

        Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Dim parentId As Integer? = Nothing
            If dgvGroups.CurrentRow IsNot Nothing Then
                Dim selectedNode = TryCast(dgvGroups.CurrentRow.Tag, GroupNode)
                If selectedNode IsNot Nothing Then
                    Dim companyId = SessionContext.CurrentCompanyID
                    Dim maxLevels = _service.GetMaxLevels(companyId)
                    Dim nextLvl = selectedNode.Level + 1

                    Dim res = DialogResult.No
                    If nextLvl < maxLevels Then
                        res = MessageBox.Show($"آیا مایلید زیرگروه جدیدی تحت گروه «{selectedNode.GroupName}» ایجاد کنید؟" & Environment.NewLine &
                                              "بله = ایجاد زیرگروه جدید" & Environment.NewLine &
                                              "خیر = ایجاد گروه اصلی جدید", "ایجاد گروه کالا",
                                              MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                    End If

                    If res = DialogResult.Cancel Then Return
                    If res = DialogResult.Yes Then
                        parentId = selectedNode.GroupID
                    End If
                End If
            End If

            Using frm As New AnbardaryGoroohKala2Form(Nothing, parentId)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
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

        Private Sub UpdateSathPanel()
            If dgvGroups.CurrentRow Is Nothing Then
                lblSathInfo.Text = "سطح گروه جاری: گروه اصلی"
                Return
            End If

            Dim node = TryCast(dgvGroups.CurrentRow.Tag, GroupNode)
            If node Is Nothing Then
                lblSathInfo.Text = "سطح گروه جاری: گروه اصلی"
                Return
            End If

            Dim pathList As New List(Of String)()
            Dim curr = node
            While curr IsNot Nothing
                pathList.Add(curr.GroupName)
                curr = If(curr.ParentID.HasValue AndAlso _nodeDict.ContainsKey(curr.ParentID.Value), _nodeDict(curr.ParentID.Value), Nothing)
            End While
            pathList.Reverse()

            lblSathInfo.Text = $"سطح گروه جاری: {String.Join(" / ", pathList.ToArray())} (سطح {node.Level + 1})"
        End Sub

        Private Sub DgvGroups_SelectionChanged(sender As Object, e As EventArgs) Handles dgvGroups.SelectionChanged
            UpdateSathPanel()
        End Sub

        Private Sub DgvGroups_Scroll(sender As Object, e As ScrollEventArgs)
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvGroups Is Nothing OrElse dgvGroups.Columns.Count = 0 OrElse pnlSearch Is Nothing Then Return

            Dim AlignTB As Action(Of TextBox, String) = Sub(tb As TextBox, colName As String)
                                                          Dim col = dgvGroups.Columns(colName)
                                                          If col Is Nothing OrElse Not col.Visible Then
                                                              tb.Visible = False
                                                              Return
                                                          End If
                                                          Dim r = dgvGroups.GetColumnDisplayRectangle(col.Index, True)
                                                          If r.IsEmpty OrElse r.Width = 0 Then
                                                              tb.Visible = False
                                                              Return
                                                          End If
                                                          Dim screenPt = dgvGroups.PointToScreen(New System.Drawing.Point(r.X, 0))
                                                          Dim panelPt = pnlSearch.PointToClient(screenPt)
                                                          tb.Location = New System.Drawing.Point(panelPt.X, 4)
                                                          tb.Width = r.Width
                                                          tb.Visible = True
                                                      End Sub

            AlignTB.Invoke(txtSearchCode, "colGroupCode")
            AlignTB.Invoke(txtSearchName, "colGroupName")

            ' Align lblSearchPrompt over the first 5 columns (Toggle, Edit, Delete, AddChild, Active)
            Dim colToggle = dgvGroups.Columns("colToggle")
            Dim colActive = dgvGroups.Columns("colIsActive")
            If colToggle IsNot Nothing AndAlso colActive IsNot Nothing Then
                Dim rToggle = dgvGroups.GetColumnDisplayRectangle(colToggle.Index, True)
                Dim rActive = dgvGroups.GetColumnDisplayRectangle(colActive.Index, True)
                If Not rToggle.IsEmpty AndAlso Not rActive.IsEmpty AndAlso rToggle.Width > 0 AndAlso rActive.Width > 0 Then
                    Dim screenPtToggle = dgvGroups.PointToScreen(New System.Drawing.Point(rToggle.X, 0))
                    Dim panelPtToggle = pnlSearch.PointToClient(screenPtToggle)

                    Dim screenPtActive = dgvGroups.PointToScreen(New System.Drawing.Point(rActive.X, 0))
                    Dim panelPtActive = pnlSearch.PointToClient(screenPtActive)

                    Dim rightEdge = panelPtToggle.X + rToggle.Width
                    Dim leftEdge = panelPtActive.X
                    lblSearchPrompt.Location = New System.Drawing.Point(leftEdge, 4)
                    lblSearchPrompt.Width = rightEdge - leftEdge
                    lblSearchPrompt.Visible = True
                Else
                    lblSearchPrompt.Visible = False
                End If
            End If
        End Sub

        Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
            RefreshGrid()
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim isSuperAdmin = String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            Dim canNewTop = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniGroupsNewTop) OrElse SessionContext.HasPermission(PermissionKeys.TradeProductGroups)
            Dim canNewGrid = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniGroupsNew) OrElse SessionContext.HasPermission(PermissionKeys.TradeProductGroups)
            Dim canEditGrid = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniGroupsEdit) OrElse SessionContext.HasPermission(PermissionKeys.TradeProductGroups)
            Dim canDeleteGrid = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniGroupsDelete) OrElse SessionContext.HasPermission(PermissionKeys.TradeProductGroups)

            btnNew.Visible = canNewTop
            If dgvGroups.Columns.Contains("colBtnAddChild") Then dgvGroups.Columns("colBtnAddChild").Visible = canNewGrid
            If dgvGroups.Columns.Contains("colBtnEdit") Then dgvGroups.Columns("colBtnEdit").Visible = canEditGrid
            If dgvGroups.Columns.Contains("colBtnDelete") Then dgvGroups.Columns("colBtnDelete").Visible = canDeleteGrid
        End Sub
    End Class
End Namespace
