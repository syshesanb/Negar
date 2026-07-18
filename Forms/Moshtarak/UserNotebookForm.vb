Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class UserNotebookForm
        Inherits Form

        Private ReadOnly notebookService As New UserNotebookService()
        Private _currentUserId As Integer
        Private _dtSource As DataTable

        Private dgvNotes As DataGridView
        Private btnNewNote As Button
        Private pnlFilterContainer As Panel

        Private txtFilterDate As TextBox
        Private txtFilterTime As TextBox
        Private txtFilterMain As TextBox
        Private txtFilterSub1 As TextBox
        Private txtFilterSub2 As TextBox
        Private txtFilterContent As TextBox

        Public Sub New()
            InitializeComponentCustom()
            AppIconHelper.ApplyAppIcon(Me)
        End Sub

        Private Sub InitializeComponentCustom()
            Me.Size = New Size(1150, 680)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.Text = "دفترچه یادداشت سیستم"

            _currentUserId = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

            ' Top Header Panel
            Dim pnlTop As New Panel() With {.Dock = DockStyle.Top, .Height = 50, .BackColor = Color.FromArgb(245, 247, 250)}
            btnNewNote = New Button() With {
                .Text = "+ یادداشت جدید",
                .Width = 140,
                .Height = 36,
                .Location = New Point(15, 7),
                .BackColor = Color.FromArgb(39, 174, 96),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Tahoma", 9.5!, FontStyle.Bold),
                .Cursor = Cursors.Hand
            }
            btnNewNote.FlatAppearance.BorderSize = 0
            AddHandler btnNewNote.Click, AddressOf BtnNewNote_Click
            pnlTop.Controls.Add(btnNewNote)

            Dim lblTitle As New Label() With {
                .Text = "دفترچه یادداشت کاربر (فیلتر هوشمند آنی)",
                .AutoSize = True,
                .Location = New Point(170, 14),
                .Font = New Font("Tahoma", 11.0!, FontStyle.Bold),
                .ForeColor = Color.FromArgb(44, 62, 80)
            }
            pnlTop.Controls.Add(lblTitle)

            ' Filter Header Panel directly above grid
            pnlFilterContainer = New Panel() With {.Dock = DockStyle.Top, .Height = 34, .BackColor = Color.FromArgb(236, 240, 241)}

            txtFilterDate = CreateFilterTextBox("فیلتر تاریخ...")
            txtFilterTime = CreateFilterTextBox("فیلتر ساعت...")
            txtFilterMain = CreateFilterTextBox("فیلتر موضوع اصلی...")
            txtFilterSub1 = CreateFilterTextBox("فیلتر موضوع فرعی ۱...")
            txtFilterSub2 = CreateFilterTextBox("فیلتر موضوع فرعی ۲...")
            txtFilterContent = CreateFilterTextBox("فیلتر متن یادداشت...")

            pnlFilterContainer.Controls.Add(txtFilterDate)
            pnlFilterContainer.Controls.Add(txtFilterTime)
            pnlFilterContainer.Controls.Add(txtFilterMain)
            pnlFilterContainer.Controls.Add(txtFilterSub1)
            pnlFilterContainer.Controls.Add(txtFilterSub2)
            pnlFilterContainer.Controls.Add(txtFilterContent)

            ' DataGridView
            dgvNotes = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .AllowUserToAddRows = False,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            }
            dgvNotes.RowTemplate.Height = 32

            AddHandler dgvNotes.CellContentClick, AddressOf DgvNotes_CellContentClick
            AddHandler dgvNotes.CellFormatting, AddressOf DgvNotes_CellFormatting
            AddHandler dgvNotes.ColumnWidthChanged, Sub(s, e) SyncFilterControls()
            AddHandler dgvNotes.Scroll, Sub(s, e) SyncFilterControls()
            AddHandler dgvNotes.Resize, Sub(s, e) SyncFilterControls()

            Me.Controls.Add(dgvNotes)
            Me.Controls.Add(pnlFilterContainer)
            Me.Controls.Add(pnlTop)
        End Sub

        Private Function CreateFilterTextBox(watermark As String) As TextBox
            Dim txt As New TextBox() With {
                .Font = New Font("Tahoma", 8.5!),
                .Height = 26,
                .Top = 4
            }
            AddHandler txt.TextChanged, AddressOf ApplyFilters
            Return txt
        End Function

        Private Sub UserNotebookForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)
            SetupGridColumns()
            LoadNotesData()
        End Sub

        Protected Overrides Sub OnShown(e As EventArgs)
            MyBase.OnShown(e)
            SyncFilterControls()
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            MyBase.OnLayout(e)
            SyncFilterControls()
        End Sub

        Private Sub SetupGridColumns()
            dgvNotes.Columns.Clear()

            Dim colView As New DataGridViewButtonColumn() With {
                .Name = "colBtnView",
                .HeaderText = "نمایش",
                .Text = "نمایش",
                .UseColumnTextForButtonValue = True,
                .Width = 60
            }
            dgvNotes.Columns.Add(colView)

            Dim colEdit As New DataGridViewButtonColumn() With {
                .Name = "colBtnEdit",
                .HeaderText = "ویرایش",
                .Text = "ویرایش",
                .UseColumnTextForButtonValue = True,
                .Width = 60
            }
            dgvNotes.Columns.Add(colEdit)

            Dim colDelete As New DataGridViewButtonColumn() With {
                .Name = "colBtnDelete",
                .HeaderText = "حذف",
                .Text = "حذف",
                .UseColumnTextForButtonValue = True,
                .Width = 60
            }
            dgvNotes.Columns.Add(colDelete)

            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "NoteID", .DataPropertyName = "NoteID", .Visible = False})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "UserID", .DataPropertyName = "UserID", .HeaderText = "کد کاربر", .Width = 65})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "NoteDateOnly", .DataPropertyName = "NoteDate", .HeaderText = "تاریخ", .Width = 95})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "NoteTimeOnly", .DataPropertyName = "NoteDate", .HeaderText = "ساعت", .Width = 75})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "MainSubject", .DataPropertyName = "MainSubject", .HeaderText = "موضوع اصلی", .Width = 150})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "SubSubject1", .DataPropertyName = "SubSubject1", .HeaderText = "موضوع فرعی ۱", .Width = 130})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "SubSubject2", .DataPropertyName = "SubSubject2", .HeaderText = "موضوع فرعی ۲", .Width = 130})
            dgvNotes.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "NoteContent", .DataPropertyName = "NoteContent", .HeaderText = "متن یادداشت", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        End Sub

        Public Sub LoadNotesData()
            _dtSource = notebookService.GetUserNotes(_currentUserId)
            dgvNotes.DataSource = _dtSource
            ApplyFilters(Nothing, Nothing)
            SyncFilterControls()
        End Sub

        Private Sub SyncFilterControls()
            If dgvNotes Is Nothing OrElse dgvNotes.Columns.Count = 0 OrElse pnlFilterContainer Is Nothing Then Return
            Try
                PositionFilterForColumn("NoteDateOnly", txtFilterDate)
                PositionFilterForColumn("NoteTimeOnly", txtFilterTime)
                PositionFilterForColumn("MainSubject", txtFilterMain)
                PositionFilterForColumn("SubSubject1", txtFilterSub1)
                PositionFilterForColumn("SubSubject2", txtFilterSub2)
                PositionFilterForColumn("NoteContent", txtFilterContent)
            Catch
            End Try
        End Sub

        Private Sub PositionFilterForColumn(colName As String, txt As TextBox)
            If Not dgvNotes.Columns.Contains(colName) OrElse txt Is Nothing Then Return
            Dim col = dgvNotes.Columns(colName)
            Dim rect = dgvNotes.GetColumnDisplayRectangle(col.Index, False)
            If rect.Width > 0 Then
                txt.Visible = True
                txt.Left = rect.Left + 1
                txt.Width = Math.Max(10, rect.Width - 2)
            Else
                txt.Visible = False
            End If
        End Sub

        Private Sub ApplyFilters(sender As Object, e As EventArgs)
            If _dtSource Is Nothing OrElse _dtSource.DefaultView Is Nothing Then Return
            Try
                Dim filters As New List(Of String)()
                If txtFilterMain IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(txtFilterMain.Text) Then
                    filters.Add("MainSubject LIKE '%" & txtFilterMain.Text.Trim().Replace("'", "''") & "%'")
                End If
                If txtFilterSub1 IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(txtFilterSub1.Text) Then
                    filters.Add("SubSubject1 LIKE '%" & txtFilterSub1.Text.Trim().Replace("'", "''") & "%'")
                End If
                If txtFilterSub2 IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(txtFilterSub2.Text) Then
                    filters.Add("SubSubject2 LIKE '%" & txtFilterSub2.Text.Trim().Replace("'", "''") & "%'")
                End If
                If txtFilterContent IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(txtFilterContent.Text) Then
                    filters.Add("NoteContent LIKE '%" & txtFilterContent.Text.Trim().Replace("'", "''") & "%'")
                End If

                If filters.Count > 0 Then
                    _dtSource.DefaultView.RowFilter = String.Join(" AND ", filters.ToArray())
                Else
                    _dtSource.DefaultView.RowFilter = ""
                End If
            Catch
            End Try
        End Sub

        Private Sub DgvNotes_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
            If e.RowIndex < 0 OrElse e.Value Is Nothing OrElse Convert.IsDBNull(e.Value) Then Return
            Dim colName = dgvNotes.Columns(e.ColumnIndex).Name

            If colName = "NoteDateOnly" AndAlso TypeOf e.Value Is DateTime Then
                Dim dt = CType(e.Value, DateTime)
                e.Value = PersianDateHelper.ToPersian(dt)
                e.FormattingApplied = True
            ElseIf colName = "NoteTimeOnly" AndAlso TypeOf e.Value Is DateTime Then
                Dim dt = CType(e.Value, DateTime)
                e.Value = dt.ToString("HH:mm:ss")
                e.FormattingApplied = True
            End If
        End Sub

        Private Sub DgvNotes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            If e.RowIndex < 0 Then Return
            Dim colName = dgvNotes.Columns(e.ColumnIndex).Name
            Dim noteIdObj = dgvNotes.Rows(e.RowIndex).Cells("NoteID").Value
            If noteIdObj Is Nothing OrElse Convert.IsDBNull(noteIdObj) Then Return
            Dim noteId = Convert.ToInt32(noteIdObj)

            Select Case colName
                Case "colBtnView"
                    Dim detailForm As New UserNotebookDetailForm(UserNotebookDetailForm.FormMode.View, _currentUserId, noteId)
                    detailForm.ShowDialog(Me)

                Case "colBtnEdit"
                    Dim detailForm As New UserNotebookDetailForm(UserNotebookDetailForm.FormMode.Edit, _currentUserId, noteId)
                    If detailForm.ShowDialog(Me) = DialogResult.OK Then
                        LoadNotesData()
                    End If

                Case "colBtnDelete"
                    If MessageBox.Show("آیا از حذف این یادداشت اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        notebookService.DeleteNote(noteId)
                        LoadNotesData()
                    End If
            End Select
        End Sub

        Private Sub BtnNewNote_Click(sender As Object, e As EventArgs)
            Dim detailForm As New UserNotebookDetailForm(UserNotebookDetailForm.FormMode.Create, _currentUserId)
            If detailForm.ShowDialog(Me) = DialogResult.OK Then
                LoadNotesData()
            End If
        End Sub
    End Class
End Namespace
