Option Strict Off
Option Explicit On

Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Partial Class ActivityLogForm
        Inherits Form

        Private ReadOnly service As New ActivityLogService()

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ActivityLogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvLog IsNot Nothing Then Me.dgvLog.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)
            LoadLog()
        End Sub

        Private Sub LoadLog()
            dgvLog.DataSource = service.GetActivityLog()
            ApplyColumnHeaders()
        End Sub

        Private Sub ApplyColumnHeaders()
            If dgvLog.Columns.Count = 0 Then Return
            If dgvLog.Columns.Contains("LogID") Then dgvLog.Columns("LogID").Visible = False
            If dgvLog.Columns.Contains("EntityID") Then dgvLog.Columns("EntityID").Visible = False
            If dgvLog.Columns.Contains("Username") Then
                dgvLog.Columns("Username").HeaderText = "نام کاربری"
                dgvLog.Columns("Username").Width = 100
            End If
            If dgvLog.Columns.Contains("FullName") Then
                dgvLog.Columns("FullName").HeaderText = "نام کامل"
                dgvLog.Columns("FullName").Width = 130
            End If
            If dgvLog.Columns.Contains("ActivityType") Then
                dgvLog.Columns("ActivityType").HeaderText = "نوع فعالیت"
                dgvLog.Columns("ActivityType").Width = 120
            End If
            If dgvLog.Columns.Contains("EntityType") Then
                dgvLog.Columns("EntityType").HeaderText = "موجودیت"
                dgvLog.Columns("EntityType").Width = 110
            End If
            If dgvLog.Columns.Contains("Description") Then
                dgvLog.Columns("Description").HeaderText = "شرح فعالیت"
                dgvLog.Columns("Description").FillWeight = 200
            End If
            If dgvLog.Columns.Contains("IPAddress") Then
                dgvLog.Columns("IPAddress").HeaderText = "آدرس IP"
                dgvLog.Columns("IPAddress").Width = 110
            End If
            If dgvLog.Columns.Contains("ActivityDate") Then
                dgvLog.Columns("ActivityDate").HeaderText = "تاریخ و ساعت"
                dgvLog.Columns("ActivityDate").Width = 140
            End If
        End Sub

        Private Sub DgvLog_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvLog.CellFormatting
            PersianDateHelper.ApplyToGrid(sender, e)
        End Sub

        Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadLog()
        End Sub
    End Class
End Namespace
