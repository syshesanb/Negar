Imports System
Imports System.Windows.Forms
Imports System.Drawing

Namespace Negar.Forms
    Public Class AnbardaryBargashtHistoryForm
        Inherits Form

        Private _invoiceId As Integer
        Private _invoiceService As New Negar.Business.InvoiceService()
        Private dgvBargasht As DataGridView

        Public Sub New(invoiceId As Integer)
            _invoiceId = invoiceId
            InitializeComponent()
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Size = New Size(950, 500)
            Me.Text = "تاریخچه برگشت از خریدهای فاکتور"
            Me.Font = New Font("Tahoma", 9)
            Me.BackColor = Color.White
        End Sub

        Private Sub InitializeComponent()
            Dim pnlTop As New Panel()
            pnlTop.Dock = DockStyle.Top
            pnlTop.Height = 50
            pnlTop.BackColor = Color.FromArgb(240, 240, 240)
            
            Dim lblTitle As New Label()
            lblTitle.Text = "لیست برگشت از خریدهای ثبت شده برای این فاکتور"
            lblTitle.Font = New Font("Tahoma", 10, FontStyle.Bold)
            lblTitle.AutoSize = True
            lblTitle.Location = New Point(10, 15)
            pnlTop.Controls.Add(lblTitle)
            
            Dim flpButtons As New FlowLayoutPanel()
            flpButtons.Dock = DockStyle.Top
            flpButtons.Height = 45
            flpButtons.BackColor = Color.FromArgb(245, 235, 235)
            flpButtons.Padding = New Padding(5)
            
            Dim btnNewBargasht As New Button()
            btnNewBargasht.Text = "برگشت از خرید جدید"
            btnNewBargasht.Size = New Size(140, 32)
            btnNewBargasht.BackColor = Color.FromArgb(180, 40, 40)
            btnNewBargasht.ForeColor = Color.White
            btnNewBargasht.FlatStyle = FlatStyle.Flat
            AddHandler btnNewBargasht.Click, Sub(s, e)
                                                Using frm As New AnbardaryKharid2Form(_invoiceId, "برگشت از خرید", True)
                                                    If frm.ShowDialog() = DialogResult.OK Then
                                                        LoadData()
                                                    End If
                                                End Using
                                            End Sub
            flpButtons.Controls.Add(btnNewBargasht)

            dgvBargasht = New DataGridView()
            dgvBargasht.Dock = DockStyle.Fill
            dgvBargasht.AllowUserToAddRows = False
            dgvBargasht.AllowUserToDeleteRows = False
            dgvBargasht.ReadOnly = True
            dgvBargasht.AutoGenerateColumns = False
            dgvBargasht.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvBargasht.BackgroundColor = Color.White
            dgvBargasht.RowHeadersVisible = False
            dgvBargasht.BorderStyle = BorderStyle.None
            dgvBargasht.EnableHeadersVisualStyles = False
            dgvBargasht.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 230, 230)
            dgvBargasht.ColumnHeadersHeight = 35
            dgvBargasht.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 248)

            Dim colInvoiceId As New DataGridViewTextBoxColumn()
            colInvoiceId.Name = "InvoiceID"
            colInvoiceId.DataPropertyName = "InvoiceID"
            colInvoiceId.Visible = False
            
            Dim colNumber As New DataGridViewTextBoxColumn()
            colNumber.Name = "InvoiceNumber"
            colNumber.DataPropertyName = "InvoiceNumber"
            colNumber.HeaderText = "شماره برگشت از خرید"
            colNumber.Width = 160
            
            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "InvoiceDate"
            colDate.DataPropertyName = "InvoiceDate"
            colDate.HeaderText = "تاریخ"
            colDate.Width = 110
            
            Dim colUser As New DataGridViewTextBoxColumn()
            colUser.Name = "CreatedBy"
            colUser.DataPropertyName = "CreatedBy"
            colUser.HeaderText = "کاربر ثبت کننده"
            colUser.Width = 120
            
            Dim colTotal As New DataGridViewTextBoxColumn()
            colTotal.Name = "TotalAmount"
            colTotal.DataPropertyName = "TotalAmount"
            colTotal.HeaderText = "مبلغ کل (ریال)"
            colTotal.Width = 140
            colTotal.DefaultCellStyle.Format = "N0"
            
            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.Name = "Description"
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "توضیحات"
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "colEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Text = "ویرایش"
            colEdit.UseColumnTextForButtonValue = True
            colEdit.Width = 60
            colEdit.FlatStyle = FlatStyle.Standard

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "colDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Text = "حذف"
            colDelete.UseColumnTextForButtonValue = True
            colDelete.Width = 60
            colDelete.FlatStyle = FlatStyle.Standard

            dgvBargasht.Columns.AddRange(New DataGridViewColumn() {colEdit, colDelete, colInvoiceId, colNumber, colDate, colUser, colTotal, colDesc})

            AddHandler dgvBargasht.CellContentClick, AddressOf dgvBargasht_CellContentClick
            AddHandler dgvBargasht.CellFormatting, AddressOf dgvBargasht_CellFormatting

            Dim pnlBottom As New Panel()
            pnlBottom.Dock = DockStyle.Bottom
            pnlBottom.Height = 50
            pnlBottom.BackColor = Color.FromArgb(240, 240, 240)
            
            Dim btnClose As New Button()
            btnClose.Text = "بستن"
            btnClose.Size = New Size(90, 32)
            btnClose.Location = New Point(10, 9)
            btnClose.BackColor = Color.White
            btnClose.FlatStyle = FlatStyle.Flat
            AddHandler btnClose.Click, Sub(s, e) Me.Close()
            pnlBottom.Controls.Add(btnClose)

            Me.Controls.Add(dgvBargasht)
            Me.Controls.Add(flpButtons)
            Me.Controls.Add(pnlTop)
            Me.Controls.Add(pnlBottom)
        End Sub

        Private Sub AnbardaryBargashtHistoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadData()
        End Sub

        Private Sub LoadData()
            Try
                Dim dt = _invoiceService.GetPurchaseInvoices()
                Dim dv As New System.Data.DataView(dt)
                dv.RowFilter = "InvoiceType = 'برگشت از خرید'"
                dgvBargasht.DataSource = dv
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
        
        Private Sub dgvBargasht_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
            If dgvBargasht.Columns(e.ColumnIndex).Name = "InvoiceDate" AndAlso e.Value IsNot Nothing AndAlso TypeOf e.Value Is DateTime Then
                e.Value = Negar.Business.PersianDateHelper.ToPersian(DirectCast(e.Value, DateTime))
                e.FormattingApplied = True
            End If
        End Sub
        
        Private Sub dgvBargasht_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex >= 0 Then
                Dim colName = dgvBargasht.Columns(e.ColumnIndex).Name
                Dim invoiceId = Convert.ToInt32(dgvBargasht.Rows(e.RowIndex).Cells("InvoiceID").Value)
                
                If colName = "colEdit" Then
                    Using frm As New AnbardaryKharid2Form(invoiceId, "برگشت از خرید")
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadData()
                        End If
                    End Using
                ElseIf colName = "colDelete" Then
                    If MessageBox.Show("آیا از حذف این برگشت از خرید اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            _invoiceService.DeletePurchaseInvoice(invoiceId)
                            MessageBox.Show("سند برگشت از خرید با موفقیت حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadData()
                        Catch ex As Exception
                            MessageBox.Show("خطا در حذف سند: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
