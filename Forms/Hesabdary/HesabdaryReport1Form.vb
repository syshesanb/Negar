Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Public Class HesabdaryReport1Form
        Inherits Form

        Public Event NewReportRequested As EventHandler
        Public Event EditReportRequested As Action(Of Integer)

        Private ReadOnly service As New AccountingService()

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
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

        Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
            RaiseEvent NewReportRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub dgvReports_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReports.CellContentClick
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
                Using printForm As New HesabdaryReportPrintForm(reportId)
                    printForm.ShowDialog(Me)
                End Using
            End If
        End Sub
    End Class
End Namespace
