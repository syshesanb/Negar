Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class UserNotebookDetailForm
        Inherits Form

        Public Enum FormMode
            View
            Create
            Edit
        End Enum

        Private _mode As FormMode
        Private _noteId As Integer?
        Private _userId As Integer
        Private ReadOnly service As New UserNotebookService()

        Private txtMainSubject As TextBox
        Private txtSubSubject1 As TextBox
        Private txtSubSubject2 As TextBox
        Private txtContent As TextBox
        Private txtHistory As TextBox
        Private btnSave As Button
        Private btnClose As Button

        Public Sub New(mode As FormMode, userId As Integer, Optional noteId As Integer? = Nothing)
            _mode = mode
            _userId = userId
            _noteId = noteId
            InitializeComponentCustom()
            AppIconHelper.ApplyAppIcon(Me)
        End Sub

        Private Sub InitializeComponentCustom()
            Me.Size = New Size(650, 580)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.0!)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.FormBorderStyle = FormBorderStyle.FixedDialog

            Select Case _mode
                Case FormMode.Create
                    Me.Text = "ایجاد یادداشت جدید"
                Case FormMode.Edit
                    Me.Text = "ویرایش یادداشت"
                Case FormMode.View
                    Me.Text = "نمایش جزئیات یادداشت"
            End Select

            Dim pnlMain As New TableLayoutPanel()
            pnlMain.Dock = DockStyle.Fill
            pnlMain.Padding = New Padding(15)
            pnlMain.ColumnCount = 2
            pnlMain.RowCount = 7
            pnlMain.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0!))
            pnlMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

            pnlMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 35.0!))
            pnlMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 35.0!))
            pnlMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 35.0!))
            pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
            pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
            pnlMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 45.0!))

            Dim lbl1 As New Label() With {.Text = "موضوع اصلی:", .Anchor = AnchorStyles.Left Or AnchorStyles.Right, .TextAlign = ContentAlignment.MiddleLeft}
            txtMainSubject = New TextBox() With {.Dock = DockStyle.Fill}
            pnlMain.Controls.Add(lbl1, 0, 0)
            pnlMain.Controls.Add(txtMainSubject, 1, 0)

            Dim lbl2 As New Label() With {.Text = "موضوع فرعی ۱:", .Anchor = AnchorStyles.Left Or AnchorStyles.Right, .TextAlign = ContentAlignment.MiddleLeft}
            txtSubSubject1 = New TextBox() With {.Dock = DockStyle.Fill}
            pnlMain.Controls.Add(lbl2, 0, 1)
            pnlMain.Controls.Add(txtSubSubject1, 1, 1)

            Dim lbl3 As New Label() With {.Text = "موضوع فرعی ۲:", .Anchor = AnchorStyles.Left Or AnchorStyles.Right, .TextAlign = ContentAlignment.MiddleLeft}
            txtSubSubject2 = New TextBox() With {.Dock = DockStyle.Fill}
            pnlMain.Controls.Add(lbl3, 0, 2)
            pnlMain.Controls.Add(txtSubSubject2, 1, 2)

            Dim lbl4 As New Label() With {.Text = "متن یادداشت:", .Anchor = AnchorStyles.Top Or AnchorStyles.Left, .TextAlign = ContentAlignment.MiddleLeft, .Margin = New Padding(0, 5, 0, 0)}
            txtContent = New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical}
            pnlMain.Controls.Add(lbl4, 0, 3)
            pnlMain.Controls.Add(txtContent, 1, 3)

            Dim lbl5 As New Label() With {.Text = "سابقه ویرایش:", .Anchor = AnchorStyles.Top Or AnchorStyles.Left, .TextAlign = ContentAlignment.MiddleLeft, .Margin = New Padding(0, 5, 0, 0)}
            txtHistory = New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical, .ReadOnly = True, .BackColor = Color.WhiteSmoke}
            pnlMain.Controls.Add(lbl5, 0, 4)
            pnlMain.Controls.Add(txtHistory, 1, 4)

            Dim pnlButtons As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft}
            btnSave = New Button() With {.Text = "ذخیره یادداشت", .Width = 120, .Height = 35, .BackColor = Color.FromArgb(46, 204, 113), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
            btnClose = New Button() With {.Text = "بستن", .Width = 90, .Height = 35, .Cursor = Cursors.Hand}

            AddHandler btnSave.Click, AddressOf BtnSave_Click
            AddHandler btnClose.Click, Sub(s, e) Me.Close()

            pnlButtons.Controls.Add(btnSave)
            pnlButtons.Controls.Add(btnClose)

            pnlMain.Controls.Add(pnlButtons, 1, 5)
            Me.Controls.Add(pnlMain)

            ConfigureMode()
        End Sub

        Private Sub ConfigureMode()
            If _mode = FormMode.View Then
                txtMainSubject.ReadOnly = True
                txtSubSubject1.ReadOnly = True
                txtSubSubject2.ReadOnly = True
                txtContent.ReadOnly = True
                btnSave.Visible = False
            End If

            If _noteId.HasValue AndAlso _noteId.Value > 0 Then
                Dim row = service.GetNoteById(_noteId.Value)
                If row IsNot Nothing Then
                    txtMainSubject.Text = If(row.IsNull("MainSubject"), "", Convert.ToString(row("MainSubject")))
                    txtSubSubject1.Text = If(row.IsNull("SubSubject1"), "", Convert.ToString(row("SubSubject1")))
                    txtSubSubject2.Text = If(row.IsNull("SubSubject2"), "", Convert.ToString(row("SubSubject2")))
                    txtContent.Text = If(row.IsNull("NoteContent"), "", Convert.ToString(row("NoteContent")))
                    txtHistory.Text = If(row.IsNull("EditHistory"), "", Convert.ToString(row("EditHistory")))
                End If
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtMainSubject.Text) AndAlso String.IsNullOrWhiteSpace(txtContent.Text) Then
                MessageBox.Show("لطفاً حداقل موضوع اصلی یا متن یادداشت را وارد کنید.", "تکمیل اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                service.SaveNote(_noteId, _userId, txtMainSubject.Text.Trim(), txtSubSubject1.Text.Trim(), txtSubSubject2.Text.Trim(), txtContent.Text)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی یادداشت: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
