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
        Private lblListTitle As Label
        Private btnNewInvoice As Button
        Private btnRefresh As Button
        Private lblSearch As Label
        Private txtSearch As TextBox
        Private dgvInvoices As DataGridView
        Private dtInvoices As DataTable

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
                .Location = New Point(720, 15)
            }
            pnlListHeader.Controls.Add(lblListTitle)

            btnNewInvoice = New Button() With {
                .Text = "+ فاکتور خرید جدید",
                .Font = New Font("B Yekan", 10.0!, FontStyle.Bold),
                .BackColor = Color.FromArgb(39, 174, 96),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Size = New Size(160, 36),
                .Location = New Point(530, 9)
            }
            AddHandler btnNewInvoice.Click, AddressOf BtnNewInvoice_Click
            pnlListHeader.Controls.Add(btnNewInvoice)

            btnRefresh = New Button() With {
                .Text = "بازخوانی",
                .Font = New Font("Tahoma", 9.0!),
                .BackColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Size = New Size(85, 36),
                .Location = New Point(435, 9)
            }
            AddHandler btnRefresh.Click, Sub(s, e) LoadInvoicesList()
            pnlListHeader.Controls.Add(btnRefresh)

            lblSearch = New Label() With {
                .Text = "جستجو:",
                .AutoSize = True, .Location = New Point(365, 17)
            }
            pnlListHeader.Controls.Add(lblSearch)

            txtSearch = New TextBox() With {
                .Size = New Size(240, 27),
                .Location = New Point(115, 14)
            }
            AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged
            pnlListHeader.Controls.Add(txtSearch)

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

            ' Docking controls correctly: DataGridView Fill, Header Panel Top
            pnlListContainer.Controls.Add(dgvInvoices)
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
                .Name = "InvoiceDate", .DataPropertyName = "InvoiceDate",
                .HeaderText = "تاریخ", .Width = 100
            }

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
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim colPayType As New DataGridViewTextBoxColumn() With {
                .Name = "PaymentType", .DataPropertyName = "PaymentType",
                .HeaderText = "نوع پرداخت", .Width = 90
            }

            Dim colDesc As New DataGridViewTextBoxColumn() With {
                .Name = "Description", .DataPropertyName = "Description",
                .HeaderText = "توضیحات", .Width = 180
            }

            dgvInvoices.Columns.AddRange(New DataGridViewColumn() {
                colRowIdx, colEdit, colDelete, colId, colNo, colDate, colVendor, colWh, colTotal, colPayType, colDesc
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
                dgvInvoices.DataSource = dtInvoices
                ApplyFilter()
            Catch ex As Exception
                Console.WriteLine("Error loading purchase invoices list: " & ex.Message)
            End Try
        End Sub

        Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
            ApplyFilter()
        End Sub

        Private Sub ApplyFilter()
            If dtInvoices Is Nothing Then Return
            Dim f = txtSearch.Text.Trim().Replace("'", "''")
            If String.IsNullOrEmpty(f) Then
                dtInvoices.DefaultView.RowFilter = ""
            Else
                dtInvoices.DefaultView.RowFilter = $"InvoiceNumber LIKE '%{f}%' OR VendorName LIKE '%{f}%' OR WarehouseName LIKE '%{f}%'"
            End If
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
            ShowEditView()
        End Sub
    End Class
End Namespace
