Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryVahedKala1Form
        Inherits Form

        Private ReadOnly _service As New UnitOfMeasureService()
        Private _unitsTable As DataTable

        Private _isSelectMode As Boolean = False
        Public SelectedUoMID As Integer = 0
        Public SelectedUoMName As String = ""
        Public SelectedCategoryName As String = ""

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(isSelectMode As Boolean)
            InitializeComponent()
            _isSelectMode = isSelectMode
        End Sub

        Private Sub AnbardaryVahedKala1Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            ThemeHelper.AppendStatusBar(Me)

            If _isSelectMode Then
                Me.FormBorderStyle = FormBorderStyle.Sizable
                Me.MaximizeBox = True
                Me.MinimizeBox = False
                Me.StartPosition = FormStartPosition.CenterParent
                Me.Text = "انتخاب واحد اندازه‌گیری"
                Me.Size = New Size(960, 520)
            End If

            If Me.dgvUnits IsNot Nothing Then
                Me.dgvUnits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            End If

            ConfigureGrid()
            LoadData()

            ' Register event handlers for grid scroll/resize/column width change
            AddHandler dgvUnits.ColumnWidthChanged, AddressOf AlignSearchBoxes
            AddHandler dgvUnits.Scroll, AddressOf DgvUnits_Scroll
            AddHandler Me.Resize, AddressOf AlignSearchBoxes

            ' Register TextChanged events for search boxes
            AddHandler txtSrcName.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcCategory.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcAbb.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcNumerator.TextChanged, AddressOf TxtSrcAny_TextChanged
            AddHandler txtSrcDenominator.TextChanged, AddressOf TxtSrcAny_TextChanged

            AlignSearchBoxes()
        End Sub

        Private Sub ConfigureGrid()
            dgvUnits.AutoGenerateColumns = False
            dgvUnits.Columns.Clear()
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            dgvUnits.RowHeadersVisible = False
            dgvUnits.AllowUserToResizeRows = False
            dgvUnits.RowTemplate.Height = 28
            dgvUnits.ColumnHeadersHeight = 32
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

            ' Grid styling
            dgvUnits.EnableHeadersVisualStyles = False
            dgvUnits.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 238, 250)
            dgvUnits.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            dgvUnits.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            dgvUnits.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvUnits.GridColor = Color.FromArgb(224, 224, 224)
            dgvUnits.CellBorderStyle = DataGridViewCellBorderStyle.Single ' Show vertical lines!

            ' Default Cell style (Odd rows)
            dgvUnits.DefaultCellStyle.BackColor = Color.White
            dgvUnits.DefaultCellStyle.ForeColor = Color.Black
            dgvUnits.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
            dgvUnits.DefaultCellStyle.SelectionForeColor = Color.White

            ' Alternating Cell style (Even rows)
            dgvUnits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            dgvUnits.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black
            dgvUnits.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
            dgvUnits.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White

            ' 1. Edit Button Column (دکمه ویرایش)
            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "btnGridEdit"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.HeaderText = "ویرایش"
            colEdit.Width = 70
            colEdit.FlatStyle = FlatStyle.Standard
            colEdit.ReadOnly = True

            ' 2. Delete Button Column (دکمه حذف)
            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "btnGridDelete"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.HeaderText = "حذف"
            colDelete.Width = 56
            colDelete.FlatStyle = FlatStyle.Standard
            colDelete.ReadOnly = True

            ' 3. IsActive (فعال)
            Dim colActive As New DataGridViewCheckBoxColumn()
            colActive.Name = "IsActive"
            colActive.DataPropertyName = "IsActive"
            colActive.HeaderText = "فعال"
            colActive.Width = 60
            colActive.ReadOnly = True

            ' 4. IsReferenceUoM (واحد مرجع؟)
            Dim colIsRef As New DataGridViewCheckBoxColumn()
            colIsRef.Name = "IsReferenceUoM"
            colIsRef.DataPropertyName = "IsReferenceUoM"
            colIsRef.HeaderText = "واحد مرجع؟"
            colIsRef.Width = 90
            colIsRef.ReadOnly = True

            ' 5. UoMID (Hidden)
            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "UoMID"
            colId.DataPropertyName = "UoMID"
            colId.Visible = False
            colId.ReadOnly = True

            ' 6. UoMName (نام واحد)
            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "UoMName"
            colName.DataPropertyName = "UoMName"
            colName.HeaderText = "نام واحد"
            colName.Width = 140
            colName.ReadOnly = True

            ' 7. CategoryName (دسته‌بندی واحد) - AutoSizeMode = Fill to stretch
            Dim colCategory As New DataGridViewTextBoxColumn()
            colCategory.Name = "CategoryName"
            colCategory.DataPropertyName = "CategoryName"
            colCategory.HeaderText = "دسته‌بندی واحد"
            colCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            colCategory.ReadOnly = True

            ' 8. Abbreviation (مخفف)
            Dim colAbb As New DataGridViewTextBoxColumn()
            colAbb.Name = "Abbreviation"
            colAbb.DataPropertyName = "Abbreviation"
            colAbb.HeaderText = "علامت اختصاری"
            colAbb.Width = 110
            colAbb.ReadOnly = True

            ' 9. ConversionNumerator (صورت کسر)
            Dim colNumerator As New DataGridViewTextBoxColumn()
            colNumerator.Name = "ConversionNumerator"
            colNumerator.DataPropertyName = "ConversionNumerator"
            colNumerator.HeaderText = "صورت کسر (M)"
            colNumerator.Width = 100
            colNumerator.ReadOnly = True

            ' 10. ConversionDenominator (مخرج کسر)
            Dim colDenominator As New DataGridViewTextBoxColumn()
            colDenominator.Name = "ConversionDenominator"
            colDenominator.DataPropertyName = "ConversionDenominator"
            colDenominator.HeaderText = "مخرج کسر (N)"
            colDenominator.Width = 100
            colDenominator.ReadOnly = True

            If _isSelectMode Then
                Dim colSelect As New DataGridViewButtonColumn()
                colSelect.Name = "btnGridSelect"
                colSelect.Text = "انتخاب"
                colSelect.UseColumnTextForButtonValue = True
                colSelect.HeaderText = "انتخاب"
                colSelect.Width = 70
                colSelect.FlatStyle = FlatStyle.Standard
                colSelect.ReadOnly = True

                dgvUnits.Columns.AddRange(New DataGridViewColumn() {
                    colSelect, colEdit, colDelete, colActive, colIsRef, colId, colName, colCategory, colAbb, colNumerator, colDenominator
                })
            Else
                dgvUnits.Columns.AddRange(New DataGridViewColumn() {
                    colEdit, colDelete, colActive, colIsRef, colId, colName, colCategory, colAbb, colNumerator, colDenominator
                })
            End If
        End Sub

        Private Sub LoadData()
            Try
                _unitsTable = _service.GetAll()
                dgvUnits.DataSource = _unitsTable
                AlignSearchBoxes()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست واحدها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            Using frm As New AnbardaryVahedKala2Form()
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub dgvUnits_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUnits.CellContentClick
            If e.RowIndex < 0 Then Return
            Dim colName = dgvUnits.Columns(e.ColumnIndex).Name
            Dim row = dgvUnits.Rows(e.RowIndex)

            If colName = "btnGridSelect" Then
                SelectRow(row)
            ElseIf colName = "btnGridEdit" Then
                OpenEditForm(row)
            ElseIf colName = "btnGridDelete" Then
                DeleteRow(row)
            End If
        End Sub

        Private Sub dgvUnits_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUnits.CellDoubleClick
            If e.RowIndex >= 0 Then
                If _isSelectMode Then
                    SelectRow(dgvUnits.Rows(e.RowIndex))
                Else
                    OpenEditForm(dgvUnits.Rows(e.RowIndex))
                End If
            End If
        End Sub

        Private Sub SelectRow(row As DataGridViewRow)
            If row Is Nothing Then Return
            SelectedUoMID = Convert.ToInt32(row.Cells("UoMID").Value)
            SelectedUoMName = Convert.ToString(row.Cells("UoMName").Value)
            SelectedCategoryName = Convert.ToString(row.Cells("CategoryName").Value)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub OpenEditForm(row As DataGridViewRow)
            If row Is Nothing Then Return
            Dim uomId = Convert.ToInt32(row.Cells("UoMID").Value)
            Using frm As New AnbardaryVahedKala2Form(uomId)
                If frm.ShowDialog() = DialogResult.OK Then
                    LoadData()
                End If
            End Using
        End Sub

        Private Sub DeleteRow(row As DataGridViewRow)
            If row Is Nothing Then Return
            Dim uomId = Convert.ToInt32(row.Cells("UoMID").Value)
            Dim uomName = Convert.ToString(row.Cells("UoMName").Value)

            Dim confirm = MessageBox.Show($"آیا از حذف واحد اندازه‌گیری «{uomName}» اطمینان دارید؟",
                                           "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirm = DialogResult.Yes Then
                Try
                    _service.Delete(uomId)
                    MessageBox.Show("واحد اندازه‌گیری با موفقیت حذف شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                Catch ex As Exception
                    MessageBox.Show("خطا در حذف واحد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            ClearSrcBoxes()
            LoadData()
        End Sub

        Private Sub ClearSrcBoxes()
            txtSrcName.Text = ""
            txtSrcCategory.Text = ""
            txtSrcAbb.Text = ""
            txtSrcNumerator.Text = ""
            txtSrcDenominator.Text = ""
        End Sub

        Private Sub btnManageCategories_Click(sender As Object, e As EventArgs) Handles btnManageCategories.Click
            Try
                Dim dt = _service.GetCategories()
                Dim listStr = "دسته‌بندی‌های موجود در سیستم:" & Environment.NewLine
                For Each row As DataRow In dt.Rows
                    listStr &= $"• {row("CategoryName")}" & Environment.NewLine
                Next
                listStr &= Environment.NewLine & "آیا مایلید دسته‌بندی جدیدی اضافه کنید؟"

                Dim res = MessageBox.Show(listStr, "مدیریت گروه‌های واحد اندازه‌گیری", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If res = DialogResult.Yes Then
                    Dim newCat = Microsoft.VisualBasic.Interaction.InputBox("نام دسته‌بندی جدید را وارد کنید (مثال: وزن، طول، تعداد):", "تعریف گروه واحد جدید", "")
                    If Not String.IsNullOrWhiteSpace(newCat) Then
                        _service.SaveCategory(Nothing, newCat.Trim())
                        MessageBox.Show("دسته‌بندی جدید با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadData()
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در مدیریت دسته‌بندی‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DgvUnits_Scroll(sender As Object, e As ScrollEventArgs)
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                AlignSearchBoxes()
            End If
        End Sub

        Private Sub AlignSearchBoxes()
            If dgvUnits Is Nothing OrElse dgvUnits.Columns.Count = 0 OrElse pnlSerch Is Nothing Then Return

            Dim AlignTB As Action(Of TextBox, String) = Sub(tb As TextBox, colName As String)
                                                          Dim col = dgvUnits.Columns(colName)
                                                          If col Is Nothing OrElse Not col.Visible Then
                                                              tb.Visible = False
                                                              Return
                                                          End If
                                                          Dim r = dgvUnits.GetColumnDisplayRectangle(col.Index, True)
                                                          If r.IsEmpty OrElse r.Width = 0 Then
                                                              tb.Visible = False
                                                              Return
                                                          End If
                                                          Dim screenPt = dgvUnits.PointToScreen(New System.Drawing.Point(r.X, 0))
                                                          Dim panelPt = pnlSerch.PointToClient(screenPt)
                                                          tb.Location = New System.Drawing.Point(panelPt.X, 4)
                                                          tb.Width = r.Width
                                                          tb.Visible = True
                                                      End Sub

            ' Align input textboxes
            AlignTB.Invoke(txtSrcName, "UoMName")
            AlignTB.Invoke(txtSrcCategory, "CategoryName")
            AlignTB.Invoke(txtSrcAbb, "Abbreviation")
            AlignTB.Invoke(txtSrcNumerator, "ConversionNumerator")
            AlignTB.Invoke(txtSrcDenominator, "ConversionDenominator")

            ' Align lblSearchPrompt over the first 4 columns (Edit, Delete, Active, IsReferenceUoM)
            Dim colEdit = dgvUnits.Columns("btnGridEdit")
            Dim colIsRef = dgvUnits.Columns("IsReferenceUoM")
            If colEdit IsNot Nothing AndAlso colIsRef IsNot Nothing Then
                Dim rEdit = dgvUnits.GetColumnDisplayRectangle(colEdit.Index, True)
                Dim rIsRef = dgvUnits.GetColumnDisplayRectangle(colIsRef.Index, True)
                If Not rEdit.IsEmpty AndAlso Not rIsRef.IsEmpty AndAlso rEdit.Width > 0 AndAlso rIsRef.Width > 0 Then
                    Dim screenPtEdit = dgvUnits.PointToScreen(New System.Drawing.Point(rEdit.X, 0))
                    Dim panelPtEdit = pnlSerch.PointToClient(screenPtEdit)

                    Dim screenPtIsRef = dgvUnits.PointToScreen(New System.Drawing.Point(rIsRef.X, 0))
                    Dim panelPtIsRef = pnlSerch.PointToClient(screenPtIsRef)

                    ' RTL Coordinate Math:
                    ' colEdit is at the far right of the grid (larger X).
                    ' colIsRef is to the left of colEdit (smaller X).
                    Dim rightEdge = panelPtEdit.X + rEdit.Width
                    Dim leftEdge = panelPtIsRef.X
                    lblSearchPrompt.Location = New System.Drawing.Point(leftEdge, 4)
                    lblSearchPrompt.Width = rightEdge - leftEdge
                    lblSearchPrompt.Visible = True
                Else
                    lblSearchPrompt.Visible = False
                End If
            End If
        End Sub

        Private Sub ApplySearchFilter()
            If _unitsTable Is Nothing Then Return

            Dim parts As New System.Collections.Generic.List(Of String)()

            Dim nameText = txtSrcName.Text.Trim().Replace("'", "''")
            Dim categoryText = txtSrcCategory.Text.Trim().Replace("'", "''")
            Dim abbText = txtSrcAbb.Text.Trim().Replace("'", "''")
            Dim numText = txtSrcNumerator.Text.Trim().Replace("'", "''")
            Dim denText = txtSrcDenominator.Text.Trim().Replace("'", "''")

            If nameText.Length > 0 Then
                parts.Add("UoMName LIKE '%" & nameText & "%'")
            End If
            If categoryText.Length > 0 Then
                parts.Add("CategoryName LIKE '%" & categoryText & "%'")
            End If
            If abbText.Length > 0 Then
                parts.Add("Abbreviation LIKE '%" & abbText & "%'")
            End If
            If numText.Length > 0 Then
                parts.Add("Convert(ConversionNumerator, 'System.String') LIKE '%" & numText & "%'")
            End If
            If denText.Length > 0 Then
                parts.Add("Convert(ConversionDenominator, 'System.String') LIKE '%" & denText & "%'")
            End If

            If parts.Count > 0 Then
                _unitsTable.DefaultView.RowFilter = String.Join(" AND ", parts.ToArray())
            Else
                _unitsTable.DefaultView.RowFilter = ""
            End If
        End Sub

        Private Sub TxtSrcAny_TextChanged(sender As Object, e As EventArgs)
            ApplySearchFilter()
        End Sub
    End Class
End Namespace
