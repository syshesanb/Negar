Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Forms
    Public Class ProgressForm
        Inherits Form

        Private _lblTitle As Label
        Private _lblStatus As Label
        Private _pnlProgress As Panel
        Private _progressValue As Integer = 0

        Public Sub New()
            InitializeControls()
        End Sub

        Private Sub InitializeControls()
            Me.Size = New Size(460, 130)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.FromArgb(250, 250, 250)
            Me.ShowInTaskbar = False
            Me.RightToLeft = RightToLeft.Yes

            ' Title Label
            _lblTitle = New Label()
            _lblTitle.Text = "لطفاً شکیبا باشید..."
            _lblTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            _lblTitle.ForeColor = Color.FromArgb(64, 64, 64)
            _lblTitle.Location = New Point(20, 20)
            _lblTitle.Size = New Size(420, 22)
            _lblTitle.TextAlign = ContentAlignment.TopRight

            ' Status Label
            _lblStatus = New Label()
            _lblStatus.Text = "در حال آماده‌سازی..."
            _lblStatus.Font = New Font("Tahoma", 9.0!, FontStyle.Regular)
            _lblStatus.ForeColor = Color.FromArgb(128, 128, 128)
            _lblStatus.Location = New Point(20, 48)
            _lblStatus.Size = New Size(420, 20)
            _lblStatus.TextAlign = ContentAlignment.TopRight
            _lblStatus.AutoEllipsis = True

            ' Custom Progress Panel
            _pnlProgress = New Panel()
            _pnlProgress.Location = New Point(20, 78)
            _pnlProgress.Size = New Size(420, 24)
            AddHandler _pnlProgress.Paint, AddressOf PanelProgress_Paint

            Me.Controls.Add(_lblTitle)
            Me.Controls.Add(_lblStatus)
            Me.Controls.Add(_pnlProgress)
        End Sub

        Private Sub PanelProgress_Paint(sender As Object, e As PaintEventArgs)
            Dim pnl = DirectCast(sender, Panel)
            Dim g = e.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias

            ' Clear background
            g.Clear(Color.FromArgb(235, 235, 235))

            ' Draw progress fill
            Dim fillWidth = CInt((_progressValue / 100.0) * pnl.Width)
            If fillWidth > 0 Then
                Using brush As New LinearGradientBrush(New Rectangle(0, 0, fillWidth, pnl.Height), Color.FromArgb(75, 108, 183), Color.FromArgb(242, 122, 84), LinearGradientMode.Horizontal)
                    g.FillRectangle(brush, 0, 0, fillWidth, pnl.Height)
                End Using
            End If

            ' Draw border
            Using pen As New Pen(Color.FromArgb(200, 200, 200))
                g.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1)
            End Using
        End Sub

        Public Sub ShowAndCenter(owner As Form)
            If owner IsNot Nothing AndAlso owner.Visible Then
                Me.StartPosition = FormStartPosition.Manual
                Dim x = owner.Left + (owner.Width - Me.Width) \ 2
                Dim y = owner.Top + (owner.Height - Me.Height) \ 2
                Me.Location = New Point(Math.Max(0, x), Math.Max(0, y))
            End If
            Me.Show(owner)
        End Sub

        Public Sub UpdateProgress(value As Integer, status As String)
            _progressValue = Math.Min(100, Math.Max(0, value))
            If Not String.IsNullOrEmpty(status) Then
                _lblStatus.Text = status
            End If
            _pnlProgress.Invalidate()
            _pnlProgress.Update()
            Me.Update()
            Application.DoEvents()
        End Sub

        ' Draw a border around the form since it has None border style
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Using pen As New Pen(Color.FromArgb(180, 180, 180), 2)
                e.Graphics.DrawRectangle(pen, 1, 1, Me.Width - 2, Me.Height - 2)
            End Using
        End Sub
    End Class
End Namespace
