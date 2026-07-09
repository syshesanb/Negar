Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryReport2Form
        Inherits Form

        Public Event SaveCompleted As EventHandler
        Public Event ExitRequested As EventHandler

        Private _reportId As Integer = 0
        Private _rootNodes As New List(Of PLNode)()
        Private ReadOnly service As New AccountingService()

        Private txtCode As TextBox
        Private txtName As TextBox
        Private btnAutoMap As Button
        Private btnAddToCategories As Button
        Private btnDeleteRow As Button
        Private btnSave As Button
        Private btnExit As Button
        Private dgvReports As DataGridView
        Private lblChainTitle As Label

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
            InitializeControls()
        End Sub

        Private Sub InitializeControls()
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.BackColor = Color.White

            ' Master Header Panel (Top)
            Dim pnlHeader As New Panel()
            pnlHeader.Dock = DockStyle.Top
            pnlHeader.Height = 55
            pnlHeader.BackColor = Color.FromArgb(245, 248, 255)
            pnlHeader.Padding = New Padding(10)
            Me.Controls.Add(pnlHeader)

            Dim lblCode As New Label()
            lblCode.Text = "کد گزارش:"
            lblCode.Dock = DockStyle.Right
            lblCode.Width = 65
            lblCode.TextAlign = ContentAlignment.MiddleRight
            pnlHeader.Controls.Add(lblCode)

            txtCode = New TextBox()
            txtCode.Dock = DockStyle.Right
            txtCode.Width = 100
            pnlHeader.Controls.Add(txtCode)

            Dim pnlHeaderSpacer As New Panel()
            pnlHeaderSpacer.Dock = DockStyle.Right
            pnlHeaderSpacer.Width = 20
            pnlHeader.Controls.Add(pnlHeaderSpacer)

            Dim lblName As New Label()
            lblName.Text = "نام گزارش:"
            lblName.Dock = DockStyle.Right
            lblName.Width = 70
            lblName.TextAlign = ContentAlignment.MiddleRight
            pnlHeader.Controls.Add(lblName)

            txtName = New TextBox()
            txtName.Dock = DockStyle.Fill
            pnlHeader.Controls.Add(txtName)

            ' Actions Panel
            Dim pnlActions As New Panel()
            pnlActions.Dock = DockStyle.Top
            pnlActions.Height = 45
            pnlActions.BackColor = Color.FromArgb(235, 243, 255)
            pnlActions.Padding = New Padding(10, 8, 10, 8)
            Me.Controls.Add(pnlActions)

            ' lblChainTitle inside pnlActions
            lblChainTitle = New Label()
            lblChainTitle.Dock = DockStyle.Fill
            lblChainTitle.TextAlign = ContentAlignment.MiddleRight
            lblChainTitle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            lblChainTitle.ForeColor = Color.FromArgb(50, 70, 100)
            lblChainTitle.Text = ""
            pnlActions.Controls.Add(lblChainTitle)

            ' Exit button (docked Left)
            btnExit = New Button()
            btnExit.Text = "خروج"
            btnExit.Dock = DockStyle.Left
            btnExit.Width = 80
            btnExit.BackColor = Color.FromArgb(240, 240, 240)
            btnExit.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnExit)

            Dim pnlSpacing0 As New Panel()
            pnlSpacing0.Dock = DockStyle.Left
            pnlSpacing0.Width = 10
            pnlActions.Controls.Add(pnlSpacing0)

            ' Save button (docked Left)
            btnSave = New Button()
            btnSave.Text = "ذخیره"
            btnSave.Dock = DockStyle.Left
            btnSave.Width = 100
            btnSave.BackColor = Color.FromArgb(200, 240, 200)
            btnSave.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnSave)

            Dim pnlSpacing1 As New Panel()
            pnlSpacing1.Dock = DockStyle.Left
            pnlSpacing1.Width = 10
            pnlActions.Controls.Add(pnlSpacing1)

            btnDeleteRow = New Button()
            btnDeleteRow.Text = "حذف سطر"
            btnDeleteRow.Dock = DockStyle.Left
            btnDeleteRow.Width = 100
            btnDeleteRow.BackColor = Color.FromArgb(250, 210, 210)
            btnDeleteRow.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnDeleteRow)

            Dim pnlSpacing2 As New Panel()
            pnlSpacing2.Dock = DockStyle.Left
            pnlSpacing2.Width = 10
            pnlActions.Controls.Add(pnlSpacing2)

            btnAddToCategories = New Button()
            btnAddToCategories.Text = "افزودن سطر"
            btnAddToCategories.Dock = DockStyle.Left
            btnAddToCategories.Width = 100
            btnAddToCategories.BackColor = Color.FromArgb(215, 235, 255)
            btnAddToCategories.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnAddToCategories)

            Dim pnlSpacing3 As New Panel()
            pnlSpacing3.Dock = DockStyle.Left
            pnlSpacing3.Width = 10
            pnlActions.Controls.Add(pnlSpacing3)

            btnAutoMap = New Button()
            btnAutoMap.Text = "تخصیص هوشمند پیش‌فرض"
            btnAutoMap.Dock = DockStyle.Left
            btnAutoMap.Width = 180
            btnAutoMap.BackColor = Color.FromArgb(220, 230, 250)
            btnAutoMap.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlActions.Controls.Add(btnAutoMap)

            ' Create DataGridView for Tree representation
            dgvReports = New DataGridView()
            dgvReports.Dock = DockStyle.Fill
            dgvReports.AllowUserToAddRows = False
            dgvReports.AllowUserToDeleteRows = False
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvReports.BackgroundColor = Color.White
            dgvReports.RowHeadersVisible = False
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReports.MultiSelect = False
            dgvReports.ReadOnly = False
            dgvReports.RowTemplate.Height = 26
            Me.Controls.Add(dgvReports)

            ' Add Columns
            Dim colToggle As New DataGridViewTextBoxColumn()
            colToggle.Name = "colToggle"
            colToggle.HeaderText = "+ / -"
            colToggle.Width = 45
            colToggle.ReadOnly = True
            colToggle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colToggle)

            Dim colRowNo As New DataGridViewTextBoxColumn()
            colRowNo.Name = "colRowNo"
            colRowNo.HeaderText = "ردیف"
            colRowNo.Width = 60
            colRowNo.ReadOnly = True
            colRowNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colRowNo)

            Dim colCategory As New DataGridViewTextBoxColumn()
            colCategory.Name = "colCategory"
            colCategory.HeaderText = "بخش‌های گزارش عملکرد و سود و زیان"
            colCategory.Width = 280
            colCategory.ReadOnly = False
            dgvReports.Columns.Add(colCategory)

            Dim colAdd As New DataGridViewButtonColumn()
            colAdd.Name = "colAdd"
            colAdd.HeaderText = "افزودن سرفصل"
            colAdd.Width = 110
            colAdd.ReadOnly = True
            dgvReports.Columns.Add(colAdd)

            Dim colRemove As New DataGridViewButtonColumn()
            colRemove.Name = "colRemove"
            colRemove.HeaderText = "حذف سرفصل"
            colRemove.Width = 110
            colRemove.ReadOnly = True
            dgvReports.Columns.Add(colRemove)

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colCode"
            colCode.HeaderText = "کد سرفصل"
            colCode.Width = 100
            colCode.ReadOnly = True
            colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colCode)

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colName"
            colName.HeaderText = "نام سرفصل"
            colName.Width = 250
            colName.ReadOnly = True
            dgvReports.Columns.Add(colName)

            Dim colID As New DataGridViewTextBoxColumn()
            colID.Name = "colID"
            colID.Visible = False
            colID.ReadOnly = True
            dgvReports.Columns.Add(colID)

            ' Register Event Handlers
            AddHandler btnAutoMap.Click, AddressOf BtnAutoMap_Click
            AddHandler btnAddToCategories.Click, AddressOf BtnAddCategoryRow_Click
            AddHandler btnDeleteRow.Click, AddressOf BtnDeleteCategoryRow_Click
            AddHandler btnSave.Click, AddressOf BtnSave_Click
            AddHandler btnExit.Click, AddressOf BtnExit_Click
            
            AddHandler dgvReports.CellContentClick, AddressOf DgvReports_CellContentClick
            AddHandler dgvReports.CellDoubleClick, AddressOf DgvReports_CellDoubleClick
            AddHandler dgvReports.SelectionChanged, AddressOf dgvReports_SelectionChanged
            AddHandler dgvReports.CellEndEdit, AddressOf DgvReports_CellEndEdit
        End Sub

        Private Sub ResetForNewReport()
            txtCode.Text = ""
            txtName.Text = ""
            _rootNodes.Clear()
            
            ' Setup default sections
            Dim defaultSections As New List(Of String)()
            defaultSections.Add("فروش ناخالص (درآمدهای عملیاتی)")
            defaultSections.Add("برگشت از فروش و تخفیفات")
            defaultSections.Add("خرید ناخالص")
            defaultSections.Add("برگشت از خرید و تخفیفات")
            defaultSections.Add("هزینه‌های مستقیم خرید (حمل خرید)")
            defaultSections.Add("هزینه‌های اداری، عمومی و فروش")
            defaultSections.Add("سایر درآمدهای عملیاتی")
            defaultSections.Add("سایر درآمدهای غیرعملیاتی")
            defaultSections.Add("سایر هزینه‌های غیرعملیاتی و مالی")

            For Each sec In defaultSections
                Dim parent As New PLNode()
                parent.CategoryName = sec
                parent.IsCategory = True
                _rootNodes.Add(parent)
            Next
            
            BuildAndRefreshGrid()
        End Sub

        Private Sub LoadReportData()
            Try
                ' Load Report1 details
                Dim dtRep = Sql.ExecuteTable("SELECT ReportCode, ReportName FROM Report1 WHERE ReportID = ?", _reportId)
                If dtRep.Rows.Count > 0 Then
                    txtCode.Text = Convert.ToString(dtRep.Rows(0)("ReportCode"))
                    txtName.Text = Convert.ToString(dtRep.Rows(0)("ReportName"))
                End If

                _rootNodes.Clear()
                Dim dtCats = service.GetProfitLossCategories(_reportId)
                Dim allMappings = service.GetProfitLossMappings(SessionContext.CurrentCompanyID.Value)

                For Each rowCat As DataRow In dtCats.Rows
                    Dim catId = Convert.ToInt32(rowCat("CategoryID"))
                    Dim catName = Convert.ToString(rowCat("CategoryName"))
                    
                    Dim parent As New PLNode()
                    parent.CategoryID = catId
                    parent.CategoryName = catName
                    parent.IsCategory = True
                    
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
                row.Cells("colCode").Value = If(node.IsCategory, "", node.AccountCode)
                row.Cells("colName").Value = If(node.IsCategory, "", node.AccountName)
                row.Cells("colID").Value = If(node.IsCategory, 0, node.AccountID)
                
                If node.IsCategory Then
                    row.Cells("colCategory").ReadOnly = False
                    row.Cells("colAdd").Value = "افزودن سرفصل"
                    row.Cells("colRemove") = New DataGridViewTextBoxCell()
                    row.Cells("colRemove").Value = ""
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 243, 255)
                    row.DefaultCellStyle.Font = New Font(dgvReports.Font, FontStyle.Bold)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 50, 100)
                Else
                    row.Cells("colCategory").ReadOnly = True
                    row.Cells("colAdd") = New DataGridViewTextBoxCell()
                    row.Cells("colAdd").Value = ""
                    row.Cells("colRemove").Value = "حذف سرفصل"
                    row.DefaultCellStyle.BackColor = Color.White
                    row.DefaultCellStyle.Font = New Font(dgvReports.Font, FontStyle.Regular)
                    row.DefaultCellStyle.ForeColor = Color.Black
                End If
            Next
            
            dgvReports.ResumeLayout()
            UpdateReportsChainLabel()
        End Sub

        Private Sub DgvReports_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing Then Return
            
            Dim colName = dgvReports.Columns(e.ColumnIndex).Name
            
            If colName = "colToggle" AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            ElseIf colName = "colAdd" AndAlso node.IsCategory Then
                Using picker As New HesabdaryForm.AccountPickerForm(SessionContext.CurrentCompanyID.Value)
                    If picker.ShowDialog(Me) = DialogResult.OK AndAlso picker.SelectedAccountID > 0 Then
                        Try
                            Dim dtAcc = Sql.ExecuteTable("SELECT AccountCode, AccountName FROM ChartOfAccounts WHERE AccountID = ?", picker.SelectedAccountID)
                            If dtAcc.Rows.Count > 0 Then
                                Dim child As New PLNode()
                                child.AccountID = picker.SelectedAccountID
                                child.AccountCode = Convert.ToString(dtAcc.Rows(0)("AccountCode"))
                                child.AccountName = Convert.ToString(dtAcc.Rows(0)("AccountName"))
                                child.IsCategory = False
                                
                                node.Children.Add(child)
                                BuildAndRefreshGrid()
                            End If
                        Catch ex As Exception
                            MessageBox.Show("خطا در افزودن حساب: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End Using
            ElseIf colName = "colRemove" AndAlso Not node.IsCategory Then
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

        Private Sub DgvReports_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                node.IsExpanded = Not node.IsExpanded
                BuildAndRefreshGrid()
            End If
        End Sub

        Private Sub dgvReports_SelectionChanged(sender As Object, e As EventArgs)
            UpdateReportsChainLabel()
        End Sub

        Private Sub UpdateReportsChainLabel()
            If lblChainTitle Is Nothing Then Return
            If dgvReports.CurrentRow Is Nothing Then
                lblChainTitle.Text = ""
                Return
            End If
            
            Dim row = dgvReports.CurrentRow
            Dim node = TryCast(row.Tag, PLNode)
            If node Is Nothing OrElse node.IsCategory OrElse node.AccountID <= 0 Then
                lblChainTitle.Text = ""
                Return
            End If
            
            Try
                Dim chain = service.GetAccountHierarchyChain(node.AccountID)
                Dim parts As New List(Of String)()
                For Each item In chain
                    parts.Add(item.Item1 & " — " & item.Item2)
                Next
                lblChainTitle.Text = "زنجیره سرفصل:  " & String.Join("  /  ", parts.ToArray())
            Catch
                lblChainTitle.Text = ""
            End Try
        End Sub

        Private Sub BtnAutoMap_Click(sender As Object, e As EventArgs)
            If MessageBox.Show("آیا مایل هستید سیستم به صورت خودکار حساب‌های سود و زیانی را بر اساس بخش‌های فعلی دسته‌بندی کند؟ (حساب‌هایی که قبلاً تخصیص داده شده‌اند تغییر نخواهند کرد)", "تخصیص هوشمند پیش‌فرض", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    Dim accounts = Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountName FROM ChartOfAccounts WHERE CompanyID = ? AND IsActive = 1", SessionContext.CurrentCompanyID.Value)
                    For Each row As DataRow In accounts.Rows
                        Dim accountId = Convert.ToInt32(row("AccountID"))
                        Dim name = Convert.ToString(row("AccountName"))
                        
                        Dim alreadyMapped = False
                        For Each root In _rootNodes
                            For Each child In root.Children
                                If child.AccountID = accountId Then
                                    alreadyMapped = True
                                    Exit For
                                End If
                            Next
                            If alreadyMapped Then Exit For
                        Next
                        If alreadyMapped Then Continue For

                        Dim categoryNameMatch As String = Nothing
                        If name.Contains("برگشت از فروش") OrElse name.Contains("برگشت فروش") OrElse name.Contains("تخفیفات فروش") Then
                            categoryNameMatch = "برگشت از فروش و تخفیفات"
                        ElseIf name.Contains("فروش") Then
                            categoryNameMatch = "فروش ناخالص (درآمدهای عملیاتی)"
                        ElseIf name.Contains("برگشت از خرید") OrElse name.Contains("برگشت خرید") OrElse name.Contains("تخفیفات خرید") Then
                            categoryNameMatch = "برگشت از خرید و تخفیفات"
                        ElseIf name.Contains("حمل خرید") OrElse name.Contains("هزینه حمل خرید") Then
                            categoryNameMatch = "هزینه‌های مستقیم خرید (حمل خرید)"
                        ElseIf name.Contains("خرید") Then
                            categoryNameMatch = "خرید ناخالص"
                        ElseIf name.Contains("غیرعملیاتی") AndAlso (name.Contains("درآمد") OrElse name.Contains("سود")) Then
                            categoryNameMatch = "سایر درآمدهای غیرعملیاتی"
                        ElseIf name.Contains("غیرعملیاتی") AndAlso (name.Contains("هزینه") OrElse name.Contains("زیان")) Then
                            categoryNameMatch = "سایر هزینه‌های غیرعملیاتی و مالی"
                        ElseIf name.Contains("مالی") AndAlso name.Contains("هزینه") Then
                            categoryNameMatch = "سایر هزینه‌های غیرعملیاتی و مالی"
                        ElseIf name.Contains("هزینه") OrElse name.Contains("اجاره") OrElse name.Contains("حقوق") OrElse name.Contains("بیمه") OrElse name.Contains("استهلاک") Then
                            categoryNameMatch = "هزینه‌های اداری، عمومی و فروش"
                        End If

                        If categoryNameMatch IsNot Nothing Then
                            For Each root In _rootNodes
                                If String.Equals(root.CategoryName, categoryNameMatch, StringComparison.OrdinalIgnoreCase) Then
                                    Dim dtAcc = Sql.ExecuteTable("SELECT AccountCode, AccountName FROM ChartOfAccounts WHERE AccountID = ?", accountId)
                                    If dtAcc.Rows.Count > 0 Then
                                        Dim child As New PLNode()
                                        child.AccountID = accountId
                                        child.AccountCode = Convert.ToString(dtAcc.Rows(0)("AccountCode"))
                                        child.AccountName = Convert.ToString(dtAcc.Rows(0)("AccountName"))
                                        child.IsCategory = False
                                        root.Children.Add(child)
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    BuildAndRefreshGrid()
                    MessageBox.Show("تخصیص هوشمند پیش‌فرض بر روی سرفصل‌های فعلی گزارش با موفقیت انجام شد.", "پیام سیستم", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("خطا در تخصیص هوشمند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub BtnAddCategoryRow_Click(sender As Object, e As EventArgs)
            Dim newCat As New PLNode()
            newCat.CategoryID = 0
            newCat.CategoryName = "بخش جدید"
            newCat.IsCategory = True
            newCat.IsExpanded = True
            _rootNodes.Add(newCat)
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

        Private Sub BtnDeleteCategoryRow_Click(sender As Object, e As EventArgs)
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
                MessageBox.Show("برای قطع اتصال حساب از دکمه 'حذف سرفصل' در همان سطر استفاده کنید.", "راهنمایی", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
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
                    Dim dto As New PLNodeDto()
                    dto.CategoryName = root.CategoryName
                    For Each child In root.Children
                        dto.AccountIDs.Add(child.AccountID)
                    Next
                    dtoList.Add(dto)
                Next
                
                Dim newId = service.SaveProfitLossFormat(_reportId, code, name, SessionContext.CurrentCompanyID.Value, dtoList)
                _reportId = newId
                
                MessageBox.Show("فرمت گزارش سود و زیان با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                RaiseEvent SaveCompleted(Me, EventArgs.Empty)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره فرمت گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnExit_Click(sender As Object, e As EventArgs)
            RaiseEvent ExitRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub DgvReports_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim row = dgvReports.Rows(e.RowIndex)
            Dim node = TryCast(row.Tag, PLNode)
            If node IsNot Nothing AndAlso node.IsCategory Then
                node.CategoryName = Convert.ToString(row.Cells("colCategory").Value).Trim()
                If String.IsNullOrEmpty(node.CategoryName) Then
                    node.CategoryName = "بخش جدید"
                    row.Cells("colCategory").Value = "بخش جدید"
                End If
            End If
        End Sub

        Private Class PLNode
            Public CategoryID As Integer
            Public Key As String
            Public CategoryName As String
            Public AccountID As Integer
            Public AccountCode As String
            Public AccountName As String
            Public IsCategory As Boolean
            Public IsExpanded As Boolean = True
            Public Children As New List(Of PLNode)()
        End Class
    End Class
End Namespace
