Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    Public Class ProgressForm
        Inherits Form

        Private _pnlBox As Panel
        Private _lblOverallTitle As Label
        Private _pbOverall As ProgressBar
        Private _lblDetailTitle As Label
        Private _pbDetail As ProgressBar
        Private _lblStatus As Label

        Public Sub New()
            InitializeControls()
        End Sub

        Private Sub InitializeControls()
            Me.Size = New Size(540, 230)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.FromArgb(235, 235, 235)
            Me.ShowInTaskbar = False
            Me.RightToLeft = RightToLeft.Yes

            ' ─── کارت سفید مرکزی ───────────────────────────────────────────
            _pnlBox = New Panel()
            _pnlBox.BorderStyle = BorderStyle.FixedSingle
            _pnlBox.Size = New Size(500, 195)
            _pnlBox.BackColor = Color.White
            _pnlBox.Location = New Point(20, 17)

            ' ─── عنوان نوار کلی ─────────────────────────────────────────────
            _lblOverallTitle = New Label()
            _lblOverallTitle.Location = New Point(20, 15)
            _lblOverallTitle.Size = New Size(460, 20)
            _lblOverallTitle.Text = "پيشرفت کلي عمليات:"
            _lblOverallTitle.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)
            _lblOverallTitle.TextAlign = ContentAlignment.TopRight
            _lblOverallTitle.BackColor = Color.Transparent

            ' ─── نوار پیشرفت کلی (سبز استاندارد) ──────────────────────────
            _pbOverall = New ProgressBar()
            _pbOverall.Location = New Point(20, 38)
            _pbOverall.Size = New Size(460, 22)
            _pbOverall.Minimum = 0
            _pbOverall.Maximum = 100
            _pbOverall.Value = 0
            _pbOverall.Style = ProgressBarStyle.Blocks

            ' ─── عنوان نوار جزئیات ──────────────────────────────────────────
            _lblDetailTitle = New Label()
            _lblDetailTitle.Location = New Point(20, 75)
            _lblDetailTitle.Size = New Size(460, 20)
            _lblDetailTitle.Text = "جزئيات عمليات جاري:"
            _lblDetailTitle.Font = New Font("Tahoma", 8.5!, FontStyle.Bold)
            _lblDetailTitle.TextAlign = ContentAlignment.TopRight
            _lblDetailTitle.BackColor = Color.Transparent

            ' ─── نوار جزئیات (Marquee – نشان‌دهنده فعالیت جاری) ────────────
            _pbDetail = New ProgressBar()
            _pbDetail.Location = New Point(20, 98)
            _pbDetail.Size = New Size(460, 22)
            _pbDetail.Style = ProgressBarStyle.Marquee
            _pbDetail.MarqueeAnimationSpeed = 25

            ' ─── متن وضعیت (قرمز تیره) ──────────────────────────────────────
            _lblStatus = New Label()
            _lblStatus.Location = New Point(20, 132)
            _lblStatus.Size = New Size(460, 46)
            _lblStatus.Text = "در حال آماده‌سازي..."
            _lblStatus.Font = New Font("Tahoma", 8.5!)
            _lblStatus.ForeColor = Color.FromArgb(180, 0, 0)
            _lblStatus.TextAlign = ContentAlignment.TopRight
            _lblStatus.AutoEllipsis = True
            _lblStatus.BackColor = Color.Transparent

            ' ─── ترکیب کنترل‌ها ─────────────────────────────────────────────
            _pnlBox.Controls.Add(_lblOverallTitle)
            _pnlBox.Controls.Add(_pbOverall)
            _pnlBox.Controls.Add(_lblDetailTitle)
            _pnlBox.Controls.Add(_pbDetail)
            _pnlBox.Controls.Add(_lblStatus)

            Me.Controls.Add(_pnlBox)
        End Sub

        ''' <summary>نمایش فرم با مرکزیابی نسبت به پنجره والد</summary>
        Public Sub ShowAndCenter(owner As Form)
            If owner IsNot Nothing AndAlso owner.Visible Then
                Me.StartPosition = FormStartPosition.Manual
                Dim x = owner.Left + (owner.Width - Me.Width) \ 2
                Dim y = owner.Top + (owner.Height - Me.Height) \ 2
                Me.Location = New Point(Math.Max(0, x), Math.Max(0, y))
            End If
            Me.Show(owner)
        End Sub

        ''' <summary>به‌روزرسانی نوار پیشرفت و متن وضعیت</summary>
        Public Sub UpdateProgress(value As Integer, status As String)
            _pbOverall.Value = Math.Min(100, Math.Max(0, value))
            If Not String.IsNullOrEmpty(status) Then
                _lblStatus.Text = status
            End If
            Me.Update()
            Application.DoEvents()
        End Sub

        ''' <summary>به‌روزرسانی همزمان نوار پیشرفت کلی و نوار پیشرفت جزئی</summary>
        Public Sub UpdateProgress(overallValue As Integer, detailValue As Integer, status As String)
            _pbOverall.Value = Math.Min(100, Math.Max(0, overallValue))
            If _pbDetail.Style <> ProgressBarStyle.Blocks Then
                _pbDetail.Style = ProgressBarStyle.Blocks
            End If
            _pbDetail.Value = Math.Min(100, Math.Max(0, detailValue))
            If Not String.IsNullOrEmpty(status) Then
                _lblStatus.Text = status
            End If
            Me.Update()
            Application.DoEvents()
        End Sub

        ''' <summary>رسم کادر خارجی چون فرم بدون بردر است</summary>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Using pen As New Pen(Color.FromArgb(185, 185, 185), 1)
                e.Graphics.DrawRectangle(pen, 0, 0, Me.Width - 1, Me.Height - 1)
            End Using
        End Sub

    End Class
End Namespace
