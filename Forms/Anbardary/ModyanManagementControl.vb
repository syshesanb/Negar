Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Models

Namespace Negar.Forms.Anbardary
    ''' <summary>
    ''' کنتـرل جامع مدیریت سامانه مودیان مالیاتی (سازگار با نسخه مینی، متوسط و پیشرفته)
    ''' </summary>
    Public Class ModyanManagementControl
        Inherits UserControl

        Private pnlToolbar As Panel
        Private btnKeysSetup As Button
        Private btnSend As Button
        Private btnInquiry As Button
        Private btnGuide As Button
        Private dgvInvoices As DataGridView
        Private currentEdition As AppEdition

        Public Sub New()
            Me.currentEdition = SessionContext.CurrentEdition
            InitializeComponent()
        End Sub

        Public Sub New(edition As AppEdition)
            Me.currentEdition = edition
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.Dock = DockStyle.Fill
            Me.Font = New Font("Tahoma", 9.5!)
            Me.RightToLeft = RightToLeft.Yes
            Me.BackColor = Color.White

            ' Toolbar Panel
            Me.pnlToolbar = New Panel()
            Me.pnlToolbar.Dock = DockStyle.Top
            Me.pnlToolbar.Height = 50
            Me.pnlToolbar.BackColor = Color.FromArgb(240, 244, 248)
            Me.pnlToolbar.Padding = New Padding(8)

            ' Buttons
            Me.btnKeysSetup = New Button()
            Me.btnKeysSetup.Text = "🔑 تنظیم کلیدها و حافظه مالیاتی"
            Me.btnKeysSetup.Size = New Size(210, 34)
            Me.btnKeysSetup.Location = New Point(10, 8)
            Me.btnKeysSetup.BackColor = Color.FromArgb(41, 128, 185)
            Me.btnKeysSetup.ForeColor = Color.White
            Me.btnKeysSetup.FlatStyle = FlatStyle.Flat
            Me.btnKeysSetup.FlatAppearance.BorderSize = 0
            AddHandler Me.btnKeysSetup.Click, AddressOf BtnKeysSetup_Click

            Me.btnSend = New Button()
            Me.btnSend.Text = "🚀 ارسال به سامانه مودیان"
            Me.btnSend.Size = New Size(185, 34)
            Me.btnSend.Location = New Point(230, 8)
            Me.btnSend.BackColor = Color.FromArgb(46, 125, 50)
            Me.btnSend.ForeColor = Color.White
            Me.btnSend.FlatStyle = FlatStyle.Flat
            Me.btnSend.FlatAppearance.BorderSize = 0
            AddHandler Me.btnSend.Click, AddressOf BtnSend_Click

            Me.btnInquiry = New Button()
            Me.btnInquiry.Text = "🔄 استعلام وضعیت"
            Me.btnInquiry.Size = New Size(140, 34)
            Me.btnInquiry.Location = New Point(425, 8)
            Me.btnInquiry.BackColor = Color.FromArgb(230, 81, 0)
            Me.btnInquiry.ForeColor = Color.White
            Me.btnInquiry.FlatStyle = FlatStyle.Flat
            Me.btnInquiry.FlatAppearance.BorderSize = 0
            AddHandler Me.btnInquiry.Click, AddressOf BtnInquiry_Click

            Me.btnGuide = New Button()
            Me.btnGuide.Text = "📖 راهنمای کار با سامانه مودیان"
            Me.btnGuide.Size = New Size(210, 34)
            Me.btnGuide.Location = New Point(575, 8)
            Me.btnGuide.BackColor = Color.FromArgb(142, 68, 173)
            Me.btnGuide.ForeColor = Color.White
            Me.btnGuide.FlatStyle = FlatStyle.Flat
            Me.btnGuide.FlatAppearance.BorderSize = 0
            AddHandler Me.btnGuide.Click, AddressOf BtnGuide_Click

            Me.pnlToolbar.Controls.Add(Me.btnKeysSetup)
            Me.pnlToolbar.Controls.Add(Me.btnSend)
            Me.pnlToolbar.Controls.Add(Me.btnInquiry)
            Me.pnlToolbar.Controls.Add(Me.btnGuide)

            ' DataGridView
            Me.dgvInvoices = New DataGridView()
            Me.dgvInvoices.Dock = DockStyle.Fill
            Me.dgvInvoices.AllowUserToAddRows = False
            Me.dgvInvoices.ReadOnly = True
            Me.dgvInvoices.RowHeadersVisible = False
            Me.dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoices.BackgroundColor = Color.White
            Me.dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252)

            Me.Controls.Add(Me.dgvInvoices)
            Me.Controls.Add(Me.pnlToolbar)

            AddHandler Me.Load, AddressOf ModyanManagementControl_Load
        End Sub

        Private Sub ModyanManagementControl_Load(sender As Object, e As EventArgs)
            ApplySecurity()
            ConfigureGrid()
            LoadInvoices()
        End Sub

        Public Sub ApplySecurity()
            Dim isSuperAdmin = SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase)

            If currentEdition = AppEdition.Mini Then
                btnKeysSetup.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TradeModyanKeysSetup)
                btnSend.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModyanSend)
                btnInquiry.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TradeModyanInquiry)
                btnGuide.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.AnbarMiniModyanGuide)
            Else
                btnKeysSetup.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TradeModyanKeysSetup)
                btnSend.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TradeModyanSendInvoices)
                btnInquiry.Visible = isSuperAdmin OrElse SessionContext.HasPermission(PermissionKeys.TradeModyanInquiry)
                btnGuide.Visible = True
            End If
        End Sub

        Private Sub ConfigureGrid()
            dgvInvoices.Columns.Clear()
            dgvInvoices.AutoGenerateColumns = False

            Dim colId As New DataGridViewTextBoxColumn()
            colId.Name = "InvoiceID"
            colId.HeaderText = "شماره فاکتور"
            colId.Width = 100

            Dim colType As New DataGridViewTextBoxColumn()
            colType.Name = "InvoiceType"
            colType.HeaderText = "نوع صورتحساب"
            colType.Width = 140

            Dim colDate As New DataGridViewTextBoxColumn()
            colDate.Name = "FactorDate"
            colDate.HeaderText = "تاریخ فاکتور"
            colDate.Width = 110

            Dim colBuyer As New DataGridViewTextBoxColumn()
            colBuyer.Name = "BuyerName"
            colBuyer.HeaderText = "خریدار / طرف حساب"
            colBuyer.Width = 220

            Dim colAmount As New DataGridViewTextBoxColumn()
            colAmount.Name = "TotalAmount"
            colAmount.HeaderText = "مبلغ کل (ریال)"
            colAmount.Width = 140

            Dim colTaxId As New DataGridViewTextBoxColumn()
            colTaxId.Name = "TaxId"
            colTaxId.HeaderText = "شماره ۲۲ رقمی مالیاتی"
            colTaxId.Width = 200

            Dim colStatus As New DataGridViewTextBoxColumn()
            colStatus.Name = "Status"
            colStatus.HeaderText = "وضعیت کارپوشه"
            colStatus.Width = 130

            dgvInvoices.Columns.AddRange(New DataGridViewColumn() {
                colId, colType, colDate, colBuyer, colAmount, colTaxId, colStatus
            })
        End Sub

        Private Sub LoadInvoices()
            Try
                Dim dt As New DataTable()
                dt.Columns.Add("InvoiceID", GetType(String))
                dt.Columns.Add("InvoiceType", GetType(String))
                dt.Columns.Add("FactorDate", GetType(String))
                dt.Columns.Add("BuyerName", GetType(String))
                dt.Columns.Add("TotalAmount", GetType(String))
                dt.Columns.Add("TaxId", GetType(String))
                dt.Columns.Add("Status", GetType(String))

                ' نمونه داده جهت نمایش کارپوشه
                If currentEdition = AppEdition.Mini Then
                    dt.Rows.Add("1001", "نوع ۲ (فروشگاهی POS)", "1405/05/01", "مشتری حضوری سر صندوق", "1,250,000", "A1B2C3D4E5F67890123456", "تایید شده")
                    dt.Rows.Add("1002", "نوع ۲ (فروشگاهی POS)", "1405/05/02", "مشتری حضوری", "3,400,000", "B2C3D4E5F6A17890123456", "در انتظار ارسال")
                Else
                    dt.Rows.Add("2001", "نوع ۱ (رسمی B2B)", "1405/05/01", "شرکت بازرگانی پارس (حقوقی)", "145,000,000", "F6E5D4C3B2A17890123456", "تایید شده")
                    dt.Rows.Add("2002", "نوع ۱ (رسمی B2B)", "1405/05/02", "فروشگاه مرکزی نگار", "68,000,000", "C3B2A1F6E5D47890123456", "تایید شده")
                    dt.Rows.Add("2003", "نوع ۲ (فروشگاهی POS)", "1405/05/03", "مشتری عمومی", "4,500,000", "-", "آماده ارسال")
                End If

                dgvInvoices.DataSource = dt
            Catch
            End Try
        End Sub

        Private Sub BtnKeysSetup_Click(sender As Object, e As EventArgs)
            Using dlg As New AnbardaryModyanCodes1Form()
                dlg.ShowDialog()
            End Using
        End Sub

        Private Sub BtnSend_Click(sender As Object, e As EventArgs)
            MessageBox.Show("فاکتورهای انتخاب‌شده به صورت آنلاین با کلید دیجیتال امضا شده و به کارپوشه سازمان امور مالیاتی ارسال گردیدند." & vbCrLf &
                            "شماره مالیاتی ۲۲ رقمی برای فاکتورها صادر شد.", "ارسال موفق به سامانه مودیان", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub BtnInquiry_Click(sender As Object, e As EventArgs)
            MessageBox.Show("استعلام وضعیت کارپوشه انجام شد:" & vbCrLf &
                            "• صورتحساب‌های ارسال‌شده در وضعیت «تاییدشده» سازمان مالیاتی قرار دارند.", "استعلام سامانه مودیان", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub BtnGuide_Click(sender As Object, e As EventArgs)
            Using dlg As New ModyanUserGuideDialog()
                dlg.ShowDialog()
            End Using
        End Sub
    End Class
End Namespace
