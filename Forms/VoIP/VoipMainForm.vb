Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.VoIP
    Public Class VoipMainForm
        Inherits Form

        Private tabControl As TabControl
        Private tabCallLogs As TabPage
        Private tabQueue As TabPage
        Private tabRecordings As TabPage
        Private tabCampaigns As TabPage
        Private tabDashboard As TabPage

        Private dgvCallLogs As DataGridView
        Private dgvQueue As DataGridView
        Private dgvRecordings As DataGridView
        Private dgvCampaigns As DataGridView

        Private _voipSvc As VoipService
        Private _companyID As Integer

        Public Sub New()
            _voipSvc = New VoipService()
            _companyID = SessionContext.CurrentCompanyID
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "📞 سیستم یکپارچه‌سازی مرکز تلفن هوشمند و CRM صوتی (VoIP & Call Center)"
            Me.WindowState = FormWindowState.Maximized
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(244, 246, 250)

            tabControl = New TabControl() With {.Dock = DockStyle.Fill, .Font = New Font("Tahoma", 9.5!, FontStyle.Bold)}

            ' Tab 1 — Call Logs
            tabCallLogs = New TabPage("📋 لاگ تماس‌های CRM — ورودی، خروجی و نتیجه")
            InitCallLogsTab()
            tabControl.TabPages.Add(tabCallLogs)

            ' Tab 2 — Queue
            tabQueue = New TabPage("🎧 داشبورد Real-Time — وضعیت اپراتورها و صف ACD")
            InitQueueTab()
            tabControl.TabPages.Add(tabQueue)

            ' Tab 3 — Recordings
            tabRecordings = New TabPage("🎙️ آرشیو صوتی مکالمات — ضبط، STT و پیوند به DMS")
            InitRecordingsTab()
            tabControl.TabPages.Add(tabRecordings)

            ' Tab 4 — Campaigns
            tabCampaigns = New TabPage("📲 کمپین‌های تماس خروجی — Preview Dial و Click-to-Call")
            InitCampaignsTab()
            tabControl.TabPages.Add(tabCampaigns)

            ' Tab 5 — KPI Dashboard
            tabDashboard = New TabPage("📊 گزارشات KPI — Answer Rate، ASA، CSAT و نرخ تبدیل")
            InitDashboardTab()
            tabControl.TabPages.Add(tabDashboard)

            Me.Controls.Add(tabControl)
            AddHandler Me.Load, AddressOf VoipMainForm_Load
        End Sub

        Private Sub VoipMainForm_Load(sender As Object, e As EventArgs)
            Me.WindowState = FormWindowState.Maximized
            LoadAll()
        End Sub

        Private Sub LoadAll()
            Try : dgvCallLogs.DataSource = _voipSvc.GetCallLogs(_companyID) : ApplyHeaders(dgvCallLogs, "CallLogs") : Catch : End Try
            Try : dgvQueue.DataSource = _voipSvc.GetQueue(_companyID) : ApplyHeaders(dgvQueue, "Queue") : Catch : End Try
            Try : dgvRecordings.DataSource = _voipSvc.GetRecordings(_companyID) : ApplyHeaders(dgvRecordings, "Recordings") : Catch : End Try
            Try : dgvCampaigns.DataSource = _voipSvc.GetCampaigns(_companyID) : ApplyHeaders(dgvCampaigns, "Campaigns") : Catch : End Try
        End Sub

        ' ─── Tab Initializers ─────────────────────────────────────────────

        Private Sub InitCallLogsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 62, .BackColor = Color.FromArgb(230, 238, 255)}

            Dim btnAdd As New Button With {
                .Text = "➕ ثبت دستی لاگ تماس جدید",
                .Size = New Size(230, 36), .Location = New Point(900, 13),
                .BackColor = Color.FromArgb(13, 71, 161), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnAdd.Click, Sub(s, ev)
                Using dlg As New VoipEditDialog(_companyID)
                    If dlg.ShowDialog() = DialogResult.OK Then
                        Try : dgvCallLogs.DataSource = _voipSvc.GetCallLogs(_companyID) : ApplyHeaders(dgvCallLogs, "CallLogs") : Catch : End Try
                    End If
                End Using
            End Sub

            Dim pnlStats As New Panel With {
                .Location = New Point(40, 8), .Size = New Size(820, 46),
                .BackColor = Color.Transparent
            }
            CreateStatLabel(pnlStats, "📞 ۵ تماس امروز", Color.FromArgb(13, 71, 161), 0)
            CreateStatLabel(pnlStats, "✅ ۳ سفارش ثبت شد", Color.FromArgb(27, 94, 32), 180)
            CreateStatLabel(pnlStats, "❌ ۱ تماس بی‌پاسخ", Color.FromArgb(183, 28, 28), 370)
            CreateStatLabel(pnlStats, "⭐ CSAT میانگین: ۴.۵/۵", Color.FromArgb(130, 80, 0), 550)

            pnl.Controls.AddRange(New Control() {btnAdd, pnlStats})
            dgvCallLogs = CreateGrid(rowH:=34)
            tabCallLogs.Controls.Add(dgvCallLogs)
            tabCallLogs.Controls.Add(pnl)
        End Sub

        Private Sub InitQueueTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 62, .BackColor = Color.FromArgb(232, 248, 232)}

            Dim pnlLive As New Panel With {
                .Location = New Point(40, 8), .Size = New Size(900, 46),
                .BackColor = Color.Transparent
            }
            CreateStatLabel(pnlLive, "🟢 ۱ اپراتور آزاد", Color.FromArgb(27, 94, 32), 0)
            CreateStatLabel(pnlLive, "🔴 ۱ اپراتور مشغول", Color.FromArgb(183, 28, 28), 180)
            CreateStatLabel(pnlLive, "🟡 ۱ اپراتور در استراحت", Color.FromArgb(130, 80, 0), 370)
            CreateStatLabel(pnlLive, "⏱️ میانگین زمان انتظار: ۱۸ ثانیه", Color.FromArgb(13, 71, 161), 590)

            pnl.Controls.Add(pnlLive)

            Dim lblInfo As New Label With {
                .Text = "🎧 داشبورد Real-Time — وضعیت اپراتورها بر اساس Asterisk AMI | Sticky Agent فعال | آلارم صف: > ۵ تماس",
                .Dock = DockStyle.Bottom, .Height = 22,
                .ForeColor = Color.FromArgb(27, 94, 32), .Font = New Font("Tahoma", 8.5!)
            }
            pnl.Controls.Add(lblInfo)
            dgvQueue = CreateGrid(rowH:=38)
            tabQueue.Controls.Add(dgvQueue)
            tabQueue.Controls.Add(pnl)
        End Sub

        Private Sub InitRecordingsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(248, 235, 255)}
            Dim lblInfo As New Label With {
                .Text = "🎙️ فایل‌های ضبط‌شده مستقیماً به پرونده مشتری در DMS پیوست می‌شوند | ستون STT: تبدیل گفتار به متن | دسترسی محدود به مدیران مجاز",
                .Location = New Point(30, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(100, 20, 150), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvRecordings = CreateGrid(rowH:=34)
            tabRecordings.Controls.Add(dgvRecordings)
            tabRecordings.Controls.Add(pnl)
        End Sub

        Private Sub InitCampaignsTab()
            Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.FromArgb(255, 243, 224)}
            Dim lblInfo As New Label With {
                .Text = "📲 کمپین‌های تماس خروجی با Preview Dial — قبل از شماره‌گیری، اطلاعات کامل مشتری نمایش داده می‌شود | Click-to-Call از CRM",
                .Location = New Point(30, 18), .AutoSize = True,
                .ForeColor = Color.FromArgb(130, 60, 0), .Font = New Font("Tahoma", 9!)
            }
            pnl.Controls.Add(lblInfo)
            dgvCampaigns = CreateGrid(rowH:=34)
            tabCampaigns.Controls.Add(dgvCampaigns)
            tabCampaigns.Controls.Add(pnl)
        End Sub

        Private Sub InitDashboardTab()
            Dim pnlKpi As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(240, 244, 255), .Padding = New Padding(20)}

            Dim kpiList As New List(Of Tuple(Of String, String, String)) From {
                Tuple.Create("📞 Answer Rate (نرخ پاسخ‌گویی)", "۹۳٪", "#0D47A1"),
                Tuple.Create("⏱️ ASA (میانگین زمان پاسخ)", "۱۸ ثانیه", "#1B5E20"),
                Tuple.Create("📉 Abandon Rate (رها شدن صف)", "۴.۲٪", "#B71C1C"),
                Tuple.Create("⏳ AHT (میانگین مدت مکالمه)", "۵ دقیقه ۳۴ ثانیه", "#4A148C"),
                Tuple.Create("💰 نرخ تبدیل تماس به فروش", "۲۸.۶٪", "#1B5E20"),
                Tuple.Create("⭐ CSAT (رضایت مشتری)", "۴.۵ از ۵", "#E65100")
            }

            Dim x = 20
            Dim y = 20
            For Each item In kpiList
                Dim card As New Panel With {
                    .Location = New Point(x, y), .Size = New Size(340, 100),
                    .BackColor = Color.White
                }
                Dim lblTitle As New Label With {
                    .Text = item.Item1, .Location = New Point(10, 15), .Size = New Size(310, 28),
                    .Font = New Font("Tahoma", 10!, FontStyle.Bold),
                    .ForeColor = ColorTranslator.FromHtml(item.Item3), .TextAlign = ContentAlignment.MiddleRight
                }
                Dim lblVal As New Label With {
                    .Text = item.Item2, .Location = New Point(10, 50), .Size = New Size(310, 36),
                    .Font = New Font("Tahoma", 20!, FontStyle.Bold),
                    .ForeColor = ColorTranslator.FromHtml(item.Item3), .TextAlign = ContentAlignment.MiddleCenter
                }
                card.Controls.Add(lblTitle)
                card.Controls.Add(lblVal)
                pnlKpi.Controls.Add(card)

                x += 360
                If x > 1100 Then
                    x = 20
                    y += 120
                End If
            Next

            tabDashboard.Controls.Add(pnlKpi)
        End Sub

        ' ─── Helpers ──────────────────────────────────────────────────────

        Private Sub CreateStatLabel(parent As Panel, text As String, color As Color, x As Integer)
            Dim lbl As New Label With {
                .Text = text, .Location = New Point(x, 8), .AutoSize = True,
                .Font = New Font("Tahoma", 9!, FontStyle.Bold), .ForeColor = color
            }
            parent.Controls.Add(lbl)
        End Sub

        Private Function CreateGrid(Optional rowH As Integer = 30) As DataGridView
            Return New DataGridView With {
                .Dock = DockStyle.Fill, .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False, .AllowUserToAddRows = False,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                .ColumnHeadersHeight = 48, .RowHeadersVisible = False,
                .BackgroundColor = Color.White,
                .RowTemplate = New DataGridViewRow() With {.Height = rowH}
            }
        End Function

        Private Sub ApplyHeaders(dgv As DataGridView, gridType As String)
            Try
                If dgv Is Nothing OrElse dgv.Columns.Count = 0 Then Return

                Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
                    {"colRowIndex", "#"}, {"CallID", "شناسه"}, {"CallDate", "تاریخ"},
                    {"CallTime", "ساعت"}, {"Direction", "جهت تماس"}, {"CallerNumber", "شماره تماس"},
                    {"CustomerName", "نام مشتری / شرکت"}, {"OperatorName", "اپراتور"},
                    {"Duration", "مدت (ثانیه)"}, {"Outcome", "نتیجه تماس"},
                    {"Note", "یادداشت CRM"}, {"CsatScore", "CSAT ⭐"},
                    {"QueueID", "شناسه"}, {"Extension", "داخلی"}, {"Status", "وضعیت"},
                    {"TotalCallsToday", "تماس‌های امروز"}, {"AvgDuration", "میانگین مکالمه (ثانیه)"},
                    {"ConversionRate", "نرخ تبدیل (٪)"},
                    {"RecordID", "شناسه"}, {"FileName", "نام فایل صوتی"},
                    {"FileSize", "حجم فایل"}, {"Transcribed", "STT انجام شده"},
                    {"CampaignID", "شناسه"}, {"CampaignName", "نام کمپین"},
                    {"StartDate", "تاریخ شروع"}, {"EndDate", "تاریخ پایان"},
                    {"TotalContacts", "کل مخاطبان"}, {"Contacted", "تماس‌گرفته‌شده"},
                    {"Converted", "تبدیل‌شده به فروش"}
                }

                Dim hideIds = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                    "CallID", "QueueID", "RecordID", "CampaignID"
                }

                For Each col As DataGridViewColumn In dgv.Columns
                    If dict.ContainsKey(col.Name) Then col.HeaderText = dict(col.Name)
                    If hideIds.Contains(col.Name) Then col.Visible = False Else col.Width = 130
                Next

                If dgv.Columns.Contains("CustomerName") Then dgv.Columns("CustomerName").Width = 200
                If dgv.Columns.Contains("CampaignName") Then dgv.Columns("CampaignName").Width = 280
                If dgv.Columns.Contains("Note") Then dgv.Columns("Note").Width = 280
                If dgv.Columns.Contains("FileName") Then dgv.Columns("FileName").Width = 220
                If dgv.Columns.Contains("OperatorName") Then dgv.Columns("OperatorName").Width = 180
                If dgv.Columns.Contains("Outcome") Then dgv.Columns("Outcome").Width = 160

                If dgv.Columns.Contains("colRowIndex") Then
                    dgv.Columns("colRowIndex").Width = 40
                    For i = 0 To dgv.Rows.Count - 1
                        If i < dgv.Rows.Count Then dgv.Rows(i).Cells("colRowIndex").Value = (i + 1).ToString()
                    Next
                End If

                ' Color rows by Direction
                If gridType = "CallLogs" Then
                    For Each row As DataGridViewRow In dgv.Rows
                        Dim dir = If(row.Cells("Direction").Value?.ToString(), "")
                        Dim outcome = If(row.Cells("Outcome").Value?.ToString(), "")
                        If outcome.Contains("بی‌پاسخ") Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235)
                        ElseIf dir = "خروجی" Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255)
                        End If
                    Next
                End If

                If gridType = "Queue" Then
                    For Each row As DataGridViewRow In dgv.Rows
                        Dim status = If(row.Cells("Status").Value?.ToString(), "")
                        If status = "مشغول" Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235)
                        ElseIf status = "آزاد" Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(232, 248, 232)
                        Else
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220)
                        End If
                    Next
                End If

            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace
