Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms

    ''' <summary>
    ''' پنجره سریع انتخاب سال مالی — با Alt+S از هر جای برنامه باز می‌شود.
    ''' </summary>
    Public Class FiscalYearSelectorForm
        Inherits Form

        ' ─── Controls ────────────────────────────────────────────────────────
        Private _lstYears As ListBox
        Private _btnSelect As Button
        Private _btnCancel As Button
        Private _lblTitle As Label
        Private _pnlTop As Panel
        Private _pnlBottom As Panel

        ' ─── State ───────────────────────────────────────────────────────────
        ''' <summary>شناسه سال مالی انتخاب‌شده پس از بستن پنجره</summary>
        Public Property SelectedFiscalYearID As Integer? = Nothing
        ''' <summary>نام سال مالی انتخاب‌شده</summary>
        Public Property SelectedFiscalYearName As String = String.Empty

        ' ─── Constructor ─────────────────────────────────────────────────────
        Public Sub New()
            BuildUI()
            LoadFiscalYears()
        End Sub

        ' ─── Build UI (runtime — no Designer) ────────────────────────────────
        Private Sub BuildUI()
            Me.Text = "انتخاب سریع سال مالی"
            Me.Size = New Size(340, 380)
            Me.MinimumSize = New Size(300, 320)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.FromArgb(245, 247, 250)
            Me.ShowInTaskbar = False
            Me.RightToLeft = RightToLeft.Yes
            Me.TopMost = True
            Me.KeyPreview = True

            ' ── Top panel with title ─────────────────────────────────────────
            _pnlTop = New Panel()
            _pnlTop.Dock = DockStyle.Top
            _pnlTop.Height = 48
            _pnlTop.BackColor = Color.FromArgb(41, 98, 180)

            _lblTitle = New Label()
            _lblTitle.Text = "  🗓  انتخاب سال مالی  ( Alt + S )"
            _lblTitle.Font = New Font("Tahoma", 10.0!, FontStyle.Bold)
            _lblTitle.ForeColor = Color.White
            _lblTitle.Dock = DockStyle.Fill
            _lblTitle.TextAlign = ContentAlignment.MiddleRight
            _lblTitle.Padding = New Padding(0, 0, 12, 0)
            _pnlTop.Controls.Add(_lblTitle)

            ' ── ListBox ─────────────────────────────────────────────────────
            _lstYears = New ListBox()
            _lstYears.Dock = DockStyle.Fill
            _lstYears.Font = New Font("Tahoma", 11.5!, FontStyle.Bold)
            _lstYears.ItemHeight = 32
            _lstYears.BorderStyle = BorderStyle.None
            _lstYears.BackColor = Color.White
            _lstYears.ForeColor = Color.FromArgb(40, 60, 100)
            _lstYears.SelectionMode = SelectionMode.One
            AddHandler _lstYears.DoubleClick, AddressOf OnSelectClicked
            AddHandler _lstYears.KeyDown, AddressOf OnListKeyDown

            ' ── Bottom panel with buttons ─────────────────────────────────────
            _pnlBottom = New Panel()
            _pnlBottom.Dock = DockStyle.Bottom
            _pnlBottom.Height = 54
            _pnlBottom.BackColor = Color.FromArgb(230, 235, 245)
            _pnlBottom.Padding = New Padding(10, 8, 10, 8)

            _btnSelect = New Button()
            _btnSelect.Text = "✔  انتخاب"
            _btnSelect.Font = New Font("Tahoma", 9.5!, FontStyle.Bold)
            _btnSelect.Size = New Size(130, 36)
            _btnSelect.Location = New Point(_pnlBottom.Width - 150, 8)
            _btnSelect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            _btnSelect.BackColor = Color.FromArgb(41, 98, 180)
            _btnSelect.ForeColor = Color.White
            _btnSelect.FlatStyle = FlatStyle.Flat
            _btnSelect.FlatAppearance.BorderSize = 0
            _btnSelect.Cursor = Cursors.Hand
            AddHandler _btnSelect.Click, AddressOf OnSelectClicked

            _btnCancel = New Button()
            _btnCancel.Text = "انصراف"
            _btnCancel.Font = New Font("Tahoma", 9.0!)
            _btnCancel.Size = New Size(90, 36)
            _btnCancel.Location = New Point(10, 8)
            _btnCancel.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            _btnCancel.BackColor = Color.FromArgb(180, 60, 60)
            _btnCancel.ForeColor = Color.White
            _btnCancel.FlatStyle = FlatStyle.Flat
            _btnCancel.FlatAppearance.BorderSize = 0
            _btnCancel.Cursor = Cursors.Hand
            AddHandler _btnCancel.Click, AddressOf OnCancelClicked

            _pnlBottom.Controls.Add(_btnSelect)
            _pnlBottom.Controls.Add(_btnCancel)

            ' ── Assemble ─────────────────────────────────────────────────────
            Me.Controls.Add(_lstYears)
            Me.Controls.Add(_pnlTop)
            Me.Controls.Add(_pnlBottom)

            AddHandler Me.KeyDown, AddressOf OnFormKeyDown
            AddHandler Me.Paint, AddressOf OnFormPaint
        End Sub

        ' ─── Load fiscal years from DB ────────────────────────────────────────
        Private Sub LoadFiscalYears()
            If Not SessionContext.CurrentCompanyID.HasValue Then
                _lstYears.Items.Add("ابتدا یک شرکت انتخاب کنید")
                _btnSelect.Enabled = False
                Return
            End If

            Try
                Dim dt = Sql.ExecuteTable(
                    "SELECT FiscalYearID, FiscalYearName FROM FiscalYears " &
                    "WHERE CompanyID = ? ORDER BY StartDate DESC",
                    SessionContext.CurrentCompanyID.Value)

                For Each row As DataRow In dt.Rows
                    Dim fyId = Convert.ToInt32(row("FiscalYearID"))
                    Dim fyName = Convert.ToString(row("FiscalYearName"))
                    _lstYears.Items.Add(New FiscalYearItem(fyId, fyName))
                Next

                ' Pre-select current year
                If SessionContext.CurrentFiscalYearID.HasValue Then
                    For i = 0 To _lstYears.Items.Count - 1
                        Dim item = TryCast(_lstYears.Items(i), FiscalYearItem)
                        If item IsNot Nothing AndAlso item.FiscalYearID = SessionContext.CurrentFiscalYearID.Value Then
                            _lstYears.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

                If _lstYears.Items.Count = 0 Then
                    _lstYears.Items.Add("هیچ سال مالی‌ای تعریف نشده")
                    _btnSelect.Enabled = False
                End If

            Catch ex As Exception
                _lstYears.Items.Add("خطا در بارگذاری سال‌های مالی")
                _btnSelect.Enabled = False
            End Try
        End Sub

        ' ─── Event Handlers ───────────────────────────────────────────────────
        Private Sub OnSelectClicked(sender As Object, e As EventArgs)
            If _lstYears.SelectedItem Is Nothing Then
                MessageBox.Show("لطفاً یک سال مالی انتخاب کنید.", "توجه",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Dim item = TryCast(_lstYears.SelectedItem, FiscalYearItem)
            If item Is Nothing Then Return

            SelectedFiscalYearID = item.FiscalYearID
            SelectedFiscalYearName = item.FiscalYearName
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub OnCancelClicked(sender As Object, e As EventArgs)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub OnListKeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Return Then
                OnSelectClicked(sender, EventArgs.Empty)
                e.Handled = True
            ElseIf e.KeyCode = Keys.Escape Then
                OnCancelClicked(sender, EventArgs.Empty)
                e.Handled = True
            End If
        End Sub

        Private Sub OnFormKeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                OnCancelClicked(sender, EventArgs.Empty)
                e.Handled = True
            End If
        End Sub

        ' Draw subtle border around borderless form
        Private Sub OnFormPaint(sender As Object, e As PaintEventArgs)
            Using pen As New Pen(Color.FromArgb(41, 98, 180), 2)
                e.Graphics.DrawRectangle(pen, 1, 1, Me.Width - 2, Me.Height - 2)
            End Using
        End Sub

        ' ─── Helper: Center relative to owner or screen ───────────────────────
        Public Sub ShowCentered(owner As Form)
            If owner IsNot Nothing AndAlso owner.Visible Then
                Me.StartPosition = FormStartPosition.Manual
                Dim x = owner.Left + (owner.Width - Me.Width) \ 2
                Dim y = owner.Top + (owner.Height - Me.Height) \ 2
                Me.Location = New Point(Math.Max(0, x), Math.Max(0, y))
            End If
        End Sub

        ' ─── Inner class: ListBox item ────────────────────────────────────────
        Private Class FiscalYearItem
            Public ReadOnly Property FiscalYearID As Integer
            Public ReadOnly Property FiscalYearName As String

            Public Sub New(id As Integer, name As String)
                FiscalYearID = id
                FiscalYearName = name
            End Sub

            Public Overrides Function ToString() As String
                Return "  " & FiscalYearName & "  "
            End Function
        End Class

    End Class

End Namespace
