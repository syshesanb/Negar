Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms.Anbardary.AnbarMini
    Public Class AnbarMiniKharidContainerForm
        Inherits Form

        Private ReadOnly invoiceService As New InvoiceService()
        Private pnlListContainer As Panel
        Private pnlEditContainer As Panel
        Private pnlListHeader As Panel
        Private pnlListFooter As Panel
        Private lblGrandTotalText As Label
        Private lblGrandTotalValue As Label
        Private lblListTitle As Label
        Private btnNewInvoice As Button
        Private btnRefresh As Button
        Private lblSearch As Label
        Private txtSearch As TextBox
        Private lblFromDate As Label
        Private txtFromDate As TextBox
        Private btnPickFromDate As Button
        Private lblToDate As Label
        Private txtToDate As TextBox
        Private btnPickToDate As Button
        Private dgvInvoices As DataGridView
        Private dtInvoices As DataTable
        Private isFormattingDate As Boolean = False

        Private WithEvents kharidForm As AnbarMiniKharidForm

        Public Sub New()
            InitializeComponents()
        End Sub

        Private Sub InitializeComponents()
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("B Yekan", 9.0!)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "مدیریت فاکتورهای خرید"

            ' 1. Edit Container Panel
            pnlEditContainer = New Panel() With {
                .Dock = DockStyle.Fill,
                .Visible = False
            }
            Me.Controls.Add(pnlEditContainer)

            ' Instantiate Kharid Form inside Edit Container
            kharidForm = New AnbarMiniKharidForm()
            kharidForm.TopLevel = False
            kharidForm.FormBorderStyle = FormBorderStyle.None
            kharidForm.Dock = DockStyle.Fill
            kharidForm.Visible = True
            pnlEditContainer.Controls.Add(kharidForm)

            ' 2. List Container Panel
            pnlListContainer = New Panel() With {
                .Dock = DockStyle.Fill,
                .Visible = True
            }
            Me.Controls.Add(pnlListContainer)

            ' Header Panel for List
            pnlListHeader = New Panel() With {
                .Dock = DockStyle.Top,
                .Height = 55,
                .BackColor = Color.FromArgb(245, 246, 250)
            }

            lblListTitle = New Label() With {
                .Text = "📋 لیست فاکتورهای خرید کالا",
                .Font = New Font("B Yekan", 11.0!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(44, 62, 80),
                .AutoSize = True,
                .Location = New Point(1000, 16)
            }
            pnlListHeader.Controls.Add(lblListTitle)

            btnNewInvoice = New Button() With {
                .Text = "+ فاکتور خرید جدید",
                .Font = New Font("B Yekan", 10.0!, FontStyle.Bold),
                .BackColor = Color.FromArgb(39, 174, 96),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Size = New Size(150, 36),
                .Location = New Point(835, 9)
            }
            AddHandler btnNewInvoice.Click, AddressOf BtnNewInvoice_Click
            pnlListHeader.Controls.Add(btnNewInvoice)

            btnRefresh = New Button() With {
                .Text = "بازخوانی",
                .Font = New Font("Tahoma", 9.0!),
                .BackColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Size = New Size(70, 36),
                .Location = New Point(758, 9)
            }
            AddHandler btnRefresh.Click, Sub(s, e) LoadInvoicesList()
            pnlListHeader.Controls.Add(btnRefresh)

            ' Date Range Controls
            lblFromDate = New Label() With {
                .Text = "از تاریخ:",
                .AutoSize = True,
                .Location = New Point(695, 17),
                .Font = New Font("Tahoma", 9.0!)
            }
            txtFromDate = New TextBox() With {
                .Size = New Size(85, 26),
                .Location = New Point(605, 14),
                .Font = New Font("Tahoma", 9.0!)
            }
            AddHandler txtFromDate.TextChanged, AddressOf TxtDate_TextChanged
            btnPickFromDate = New Button() With {
                .Text = "...",
                .Size = New Size(26, 26),
                .Location = New Point(576, 14),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.White,
                .Font = New Font("Tahoma", 8.0!, FontStyle.Bold)
            }
            AddHandler btnPickFromDate.Click, AddressOf BtnPickFromDate_Click

            lblToDate = New Label() With {
                .Text = "تا تاریخ:",
                .AutoSize = True,
                .Location = New Point(515, 17),
                .Font = New Font("Tahoma", 9.0!)
            }
            txtToDate = New TextBox() With {
                .Size = New Size(85, 26),
                .Location = New Point(425, 14),
                .Font = New Font("Tahoma", 9.0!)
            }
            AddHandler txtToDate.TextChanged, AddressOf TxtDate_TextChanged
            btnPickToDate = New Button() With {
                .Text = "...",
                .Size = New Size(26, 26),
                .Location = New Point(396, 14),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.White,
                .Font = New Font("Tahoma", 8.0!, FontStyle.Bold)
            }
            AddHandler btnPickToDate.Click, AddressOf BtnPickToDate_Click

            pnlListHeader.Controls.AddRange(New Control() {
                lblFromDate, txtFromDate, btnPickFromDate,
                lblToDate, txtToDate, btnPickToDate
            })

            ' Search Box
            lblSearch = New Label() With {
                .Text = "جستجو:",
                .AutoSize = True,
                .Location = New Point(335, 17),
                .Font = New Font("Tahoma", 9.0!)
            }
            pnlListHeader.Controls.Add(lblSearch)

            txtSearch = New TextBox() With {
                .Size = New Size(160, 26),
                .Location = New Point(170, 14),
                .Font = New Font("Tahoma", 9.0!)
            }
            AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged
            pnlListHeader.Controls.Add(txtSearch)

            ' Footer Panel for Grand Total Purchases
            pnlListFooter = New Panel() With {
                .Dock = DockStyle.Bottom,
                .Height = 45,
                .BackColor = Color.FromArgb(44, 62, 80)
            }

            lblGrandTotalText = New Label() With {
                .Text = "جمع کل خرید:",
                .Font = New Font("Tahoma", 10.0!, FontStyle.Bold),
                .ForeColor = Color.White,
                .AutoSize = True,
                .Location = New Point(350, 12)
            }
            lblGrandTotalValue = New Label() With {
                .Text = "۰ ریال",
                .Font = New Font("Tahoma", 13.0!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(46, 204, 113),
                .AutoSize = True,
                .Location = New Point(100, 8)
            }
            pnlListFooter.Controls.Add(lblGrandTotalText)
            pnlListFooter.Controls.Add(lblGrandTotalValue)

            ' DataGridView
            dgvInvoices = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .AllowUserToAddRows = False,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .RowHeadersVisible = False,
                .AutoGenerateColumns = False,
                .EnableHeadersVisualStyles = False,
                .RowTemplate = New DataGridViewRow() With {.Height = 32}
            }

            dgvInvoices.CellBorderStyle = DataGridViewCellBorderStyle.Single
            dgvInvoices.GridColor = Color.FromArgb(200, 210, 225)
            dgvInvoices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185)
            dgvInvoices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            dgvInvoices.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            dgvInvoices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvInvoices.ColumnHeadersHeight = 36
            dgvInvoices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            dgvInvoices.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219)
            dgvInvoices.DefaultCellStyle.SelectionForeColor = Color.White
            dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)

            SetupGridColumns()
            ApplySecurity()

            ' Docking controls: Fill (dgv), Bottom (footer), Top (header)
            pnlListContainer.Controls.Add(dgvInvoices)
            pnlListContainer.Controls.Add(pnlListFooter)
            pnlListContainer.Controls.Add(pnlListHeader)

            AddHandler dgvInvoices.RowPostPaint, AddressOf DgvInvoices_RowPostPaint
            AddHandler dgvInvoices.CellContentClick, AddressOf DgvInvoices_CellContentClick
            AddHandler dgvInvoices.CellDoubleClick, AddressOf DgvInvoices_CellDoubleClick
            AddHandler Me.Load, AddressOf AnbarMiniKharidContainerForm_Load
            AddHandler Me.VisibleChanged, AddressOf AnbarMiniKharidContainerForm_VisibleChanged
        End Sub

        Private Sub SetupGridColumns()
            dgvInvoices.Columns.Clear()

            ' 1. Row Index Column
            Dim colRowIdx As New DataGridViewTextBoxColumn() With {
                .Name = "colRowIndex", .HeaderText = "ردیف",
                .Width = 55, .ReadOnly = True
            }
            colRowIdx.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 2. Edit Button Column
            Dim colEdit As New DataGridViewButtonColumn() With {
                .Name = "colEdit", .HeaderText = "ویرایش",
                .Text = "ویرایش", .UseColumnTextForButtonValue = True,
                .Width = 65, .FlatStyle = FlatStyle.Flat
            }

            ' 3. Delete Button Column
            Dim colDelete As New DataGridViewButtonColumn() With {
                .Name = "colDelete", .HeaderText = "حذف",
                .Text = "حذف", .UseColumnTextForButtonValue = True,
                .Width = 60, .FlatStyle = FlatStyle.Flat
            }

            ' Hidden ID Column
            Dim colId As New DataGridViewTextBoxColumn() With {
                .Name = "InvoiceID", .DataPropertyName = "InvoiceID", .Visible = False
            }

            ' Data Columns
            Dim colNo As New DataGridViewTextBoxColumn() With {
                .Name = "InvoiceNumber", .DataPropertyName = "InvoiceNumber",
                .HeaderText = "شماره فاکتور", .Width = 110
            }

            Dim colDate As New DataGridViewTextBoxColumn() With {
                .Name = "InvoiceDate", .DataPropertyName = "PersianDate",
                .HeaderText = "تاریخ", .Width = 130
            }
            colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim colVendor As New DataGridViewTextBoxColumn() With {
                .Name = "VendorName", .DataPropertyName = "VendorName",
                .HeaderText = "فروشنده / تامین‌کننده", .Width = 180
            }

            Dim colWh As New DataGridViewTextBoxColumn() With {
                .Name = "WarehouseName", .DataPropertyName = "WarehouseName",
                .HeaderText = "انبار مقصد", .Width = 130
            }

            Dim colTotal As New DataGridViewTextBoxColumn() With {
                .Name = "TotalAmount", .DataPropertyName = "TotalAmount",
                .HeaderText = "مبلغ کل (ریال)", .Width = 130
            }
            colTotal.DefaultCellStyle.Format = "N0"
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            Dim colPayType As New DataGridViewTextBoxColumn() With {
                .Name = "PaymentType", .DataPropertyName = "PaymentType",
                .HeaderText = "نوع پرداخت", .Width = 90
            }

            Dim colSanadRef As New DataGridViewTextBoxColumn() With {
                .Name = "SanadRef", .DataPropertyName = "SanadRef",
                .HeaderText = "عطف حسابداری (سند / سال مالی)", .Width = 175
            }
            colSanadRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            colSanadRef.DefaultCellStyle.ForeColor = Color.FromArgb(13, 71, 161)
            colSanadRef.DefaultCellStyle.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)

            Dim colDesc As New DataGridViewTextBoxColumn() With {
                .Name = "Description", .DataPropertyName = "Description",
                .HeaderText = "توضیحات", .Width = 180
            }

            dgvInvoices.Columns.AddRange(New DataGridViewColumn() {
                colRowIdx, colEdit, colDelete, colId, colNo, colDate, colVendor, colWh, colTotal, colPayType, colSanadRef, colDesc
            })
        End Sub

        Private Sub DgvInvoices_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
            dgvInvoices.Rows(e.RowIndex).Cells("colRowIndex").Value = (e.RowIndex + 1).ToString()
        End Sub

        Private Sub AnbarMiniKharidContainerForm_Load(sender As Object, e As EventArgs)
            ShowListView()
        End Sub

        Private Sub AnbarMiniKharidContainerForm_VisibleChanged(sender As Object, e As EventArgs)
            If Me.Visible AndAlso pnlListContainer.Visible Then
                LoadInvoicesList()
            End If
        End Sub

        Private Sub LoadInvoicesList()
            Try
                dtInvoices = invoiceService.GetPurchaseInvoices()
                If dtInvoices IsNot Nothing Then
                    If Not dtInvoices.Columns.Contains("PersianDate") Then
                        dtInvoices.Columns.Add("PersianDate", GetType(String))
                    End If

                    For Each row As DataRow In dtInvoices.Rows
                        If Not row.IsNull("InvoiceDate") Then
                            Try
                                Dim d = Convert.ToDateTime(row("InvoiceDate"))
                                row("PersianDate") = PersianDateHelper.ToPersian(d) & "  " & d.ToString("HH:mm")
                            Catch
                                row("PersianDate") = Convert.ToString(row("InvoiceDate"))
                            End Try
                        Else
                            row("PersianDate") = ""
                        End If
                    Next
                End If

                dgvInvoices.DataSource = dtInvoices
                ApplyFilter()
            Catch ex As Exception
                Console.WriteLine("Error loading purchase invoices list: " & ex.Message)
            End Try
        End Sub

        Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
            ApplyFilter()
        End Sub

        Private Sub TxtDate_TextChanged(sender As Object, e As EventArgs)
            Dim txt = TryCast(sender, TextBox)
            If txt IsNot Nothing Then
                FormatDateTextBox(txt)
            End If
            ApplyFilter()
        End Sub

        Private Sub FormatDateTextBox(txt As TextBox)
            If isFormattingDate Then Return
            Dim digitsOnly = System.Text.RegularExpressions.Regex.Replace(txt.Text, "[^\d]", "")
            If digitsOnly.Length = 8 Then
                isFormattingDate = True
                txt.Text = digitsOnly.Substring(0, 4) & "/" & digitsOnly.Substring(4, 2) & "/" & digitsOnly.Substring(6, 2)
                txt.SelectionStart = txt.Text.Length
                isFormattingDate = False
            End If
        End Sub

        Private Sub BtnPickFromDate_Click(sender As Object, e As EventArgs)
            Using calForm As New PersianCalendarForm()
                If calForm.ShowDialog(Me) = DialogResult.OK Then
                    txtFromDate.Text = calForm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub BtnPickToDate_Click(sender As Object, e As EventArgs)
            Using calForm As New PersianCalendarForm()
                If calForm.ShowDialog(Me) = DialogResult.OK Then
                    txtToDate.Text = calForm.SelectedDate
                End If
            End Using
        End Sub

        Private Sub ApplyFilter()
            If dtInvoices Is Nothing Then Return

            Dim filters As New System.Collections.Generic.List(Of String)()

            ' Text Search Filter
            Dim f = txtSearch.Text.Trim().Replace("'", "''")
            If Not String.IsNullOrEmpty(f) Then
                filters.Add($"(InvoiceNumber LIKE '%{f}%' OR VendorName LIKE '%{f}%' OR WarehouseName LIKE '%{f}%')")
            End If

            ' Date Range Filter (against PersianDate or InvoiceDate)
            Dim fromDate = txtFromDate.Text.Trim().Replace("'", "''")
            Dim toDate = txtToDate.Text.Trim().Replace("'", "''")

            If Not String.IsNullOrEmpty(fromDate) AndAlso fromDate.Length = 10 Then
                filters.Add($"PersianDate >= '{fromDate}'")
            End If
            If Not String.IsNullOrEmpty(toDate) AndAlso toDate.Length = 10 Then
                filters.Add($"PersianDate <= '{toDate}  99:99'")
            End If

            If filters.Count > 0 Then
                dtInvoices.DefaultView.RowFilter = String.Join(" AND ", filters.ToArray())
            Else
                dtInvoices.DefaultView.RowFilter = ""
            End If

            RecalculateTotalPurchases()
        End Sub

        Private Sub RecalculateTotalPurchases()
            If dtInvoices Is Nothing Then
                lblGrandTotalValue.Text = "۰ ریال"
                Return
            End If

            Dim dv = dtInvoices.DefaultView
            Dim totalSum As Decimal = 0D

            For Each drv As DataRowView In dv
                If Not drv.Row.IsNull("TotalAmount") Then
                    Dim val As Decimal = 0D
                    Decimal.TryParse(Convert.ToString(drv("TotalAmount")), val)
                    totalSum += val
                End If
            Next

            lblGrandTotalValue.Text = totalSum.ToString("N0") & " ریال"
        End Sub

        Private Sub BtnNewInvoice_Click(sender As Object, e As EventArgs)
            ShowEditView()
        End Sub

        Private Sub ShowListView()
            pnlEditContainer.Visible = False
            pnlListContainer.Visible = True
            pnlListContainer.BringToFront()
            LoadInvoicesList()
        End Sub

        Private Sub ShowEditView()
            kharidForm.ResetForm()
            pnlListContainer.Visible = False
            pnlEditContainer.Visible = True
            pnlEditContainer.BringToFront()
        End Sub

        Private Sub kharidForm_InvoiceSaved() Handles kharidForm.InvoiceSaved
            ShowListView()
        End Sub

        Private Sub kharidForm_InvoiceCancelled() Handles kharidForm.InvoiceCancelled
            ShowListView()
        End Sub

        Private Sub DgvInvoices_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                OpenInvoiceForView(e.RowIndex)
            End If
        End Sub

        Private Sub DgvInvoices_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                Dim colName = dgvInvoices.Columns(e.ColumnIndex).Name
                If colName = "colEdit" Then
                    OpenInvoiceForView(e.RowIndex)
                ElseIf colName = "colDelete" Then
                    Dim invId = Convert.ToInt32(dgvInvoices.Rows(e.RowIndex).Cells("InvoiceID").Value)
                    Dim invNo = Convert.ToString(dgvInvoices.Rows(e.RowIndex).Cells("InvoiceNumber").Value)

                    If MessageBox.Show($"آیا از حذف فاکتور خرید {invNo} اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            invoiceService.DeletePurchaseInvoice(invId)
                            MessageBox.Show("فاکتور خرید با موفقیت حذف گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadInvoicesList()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف فاکتور: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End Sub

        Private Sub OpenInvoiceForView(rowIndex As Integer)
            If rowIndex >= 0 AndAlso rowIndex < dgvInvoices.Rows.Count Then
                Dim invId = Convert.ToInt32(dgvInvoices.Rows(rowIndex).Cells("InvoiceID").Value)
                pnlListContainer.Visible = False
                pnlEditContainer.Visible = True
                pnlEditContainer.BringToFront()
                kharidForm.LoadInvoiceForEdit(invId)
            End If
        End Sub

        Private Sub ApplySecurity()
            If SessionContext.CurrentUser Is Nothing Then Return
            Dim isSuperAdmin = String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            Dim canCreate = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharidNew) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharid) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)
            Dim canEdit = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharidEdit) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharid) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)
            Dim canDelete = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharidDelete) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniKharid) OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModule)

            btnNewInvoice.Visible = canCreate
            If dgvInvoices.Columns.Contains("colEdit") Then dgvInvoices.Columns("colEdit").Visible = canEdit
            If dgvInvoices.Columns.Contains("colDelete") Then dgvInvoices.Columns("colDelete").Visible = canDelete
        End Sub
    End Class
End Namespace
