Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class HesabdaryReport1Form
        Inherits Form

        Public Event NewReportRequested As EventHandler
        Public Event EditReportRequested As Action(Of Integer)

        Private dgvReports As DataGridView
        Private btnNew As Button
        Private ReadOnly service As New AccountingService()

        Public Sub New()
            InitializeControls()
        End Sub

        Private Sub InitializeControls()
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.BackColor = Color.White

            ' Top Panel
            Dim pnlTop As New Panel()
            pnlTop.Dock = DockStyle.Top
            pnlTop.Height = 45
            pnlTop.BackColor = Color.FromArgb(235, 243, 255)
            pnlTop.Padding = New Padding(10, 8, 10, 8)
            Me.Controls.Add(pnlTop)

            btnNew = New Button()
            btnNew.Text = "جدید"
            btnNew.Dock = DockStyle.Right
            btnNew.Width = 100
            btnNew.BackColor = Color.FromArgb(200, 230, 200)
            btnNew.Font = New Font("Tahoma", 9.0!, FontStyle.Bold)
            pnlTop.Controls.Add(btnNew)

            ' DataGridView
            dgvReports = New DataGridView()
            dgvReports.Dock = DockStyle.Fill
            dgvReports.AllowUserToAddRows = False
            dgvReports.AllowUserToDeleteRows = False
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvReports.BackgroundColor = Color.White
            dgvReports.RowHeadersVisible = False
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvReports.MultiSelect = False
            dgvReports.ReadOnly = True
            dgvReports.RowTemplate.Height = 26
            Me.Controls.Add(dgvReports)

            ' Add Columns
            Dim colRowNo As New DataGridViewTextBoxColumn()
            colRowNo.Name = "colRowNo"
            colRowNo.HeaderText = "ردیف"
            colRowNo.Width = 60
            colRowNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colRowNo)

            Dim colEdit As New DataGridViewButtonColumn()
            colEdit.Name = "colEdit"
            colEdit.HeaderText = "ویرایش"
            colEdit.Width = 80
            dgvReports.Columns.Add(colEdit)

            Dim colDelete As New DataGridViewButtonColumn()
            colDelete.Name = "colDelete"
            colDelete.HeaderText = "حذف"
            colDelete.Width = 80
            dgvReports.Columns.Add(colDelete)

            Dim colCode As New DataGridViewTextBoxColumn()
            colCode.Name = "colCode"
            colCode.HeaderText = "کد گزارش"
            colCode.Width = 100
            colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Columns.Add(colCode)

            Dim colName As New DataGridViewTextBoxColumn()
            colName.Name = "colName"
            colName.HeaderText = "نام گزارش"
            colName.Width = 300
            dgvReports.Columns.Add(colName)

            Dim colPrint As New DataGridViewButtonColumn()
            colPrint.Name = "colPrint"
            colPrint.HeaderText = "چاپ"
            colPrint.Width = 80
            dgvReports.Columns.Add(colPrint)

            Dim colID As New DataGridViewTextBoxColumn()
            colID.Name = "colID"
            colID.Visible = False
            dgvReports.Columns.Add(colID)

            ' Event Handlers
            AddHandler btnNew.Click, AddressOf BtnNew_Click
            AddHandler dgvReports.CellContentClick, AddressOf DgvReports_CellContentClick
        End Sub

        Public Sub RefreshData()
            Try
                dgvReports.Rows.Clear()
                Dim dt = service.GetReports(SessionContext.CurrentCompanyID.Value)
                
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim row = dt.Rows(i)
                    Dim rowIdx = dgvReports.Rows.Add()
                    Dim gridRow = dgvReports.Rows(rowIdx)
                    
                    gridRow.Cells("colRowNo").Value = i + 1
                    gridRow.Cells("colEdit").Value = "ویرایش"
                    gridRow.Cells("colDelete").Value = "حذف"
                    gridRow.Cells("colCode").Value = Convert.ToString(row("ReportCode"))
                    gridRow.Cells("colName").Value = Convert.ToString(row("ReportName"))
                    gridRow.Cells("colPrint").Value = "چاپ"
                    gridRow.Cells("colID").Value = Convert.ToInt32(row("ReportID"))
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری لیست گزارش‌ها: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnNew_Click(sender As Object, e As EventArgs)
            RaiseEvent NewReportRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub dgvReports_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim gridRow = dgvReports.Rows(e.RowIndex)
            Dim reportId = Convert.ToInt32(gridRow.Cells("colID").Value)
            Dim code = Convert.ToString(gridRow.Cells("colCode").Value)
            Dim name = Convert.ToString(gridRow.Cells("colName").Value)
            
            Dim colName = dgvReports.Columns(e.ColumnIndex).Name

            If colName = "colEdit" Then
                RaiseEvent EditReportRequested(reportId)
            ElseIf colName = "colDelete" Then
                If MessageBox.Show("آیا مطمئن هستید که می‌خواهید گزارش '" & name & "' را به همراه تمام ساختار و حساب‌های متصل به آن حذف کنید؟", "تایید حذف گزارش", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Try
                        service.DeleteReport(reportId)
                        RefreshData()
                    Catch ex As Exception
                        MessageBox.Show("خطا در حذف گزارش: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            ElseIf colName = "colPrint" Then
                MessageBox.Show("چاپ گزارش برای: " & name & " (" & code & ")" & Environment.NewLine & "این قابلیت در نسخه‌های بعدی کامل خواهد شد.", "چاپ گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub
    End Class
End Namespace
