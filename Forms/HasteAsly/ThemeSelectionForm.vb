Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class ThemeSelectionForm
        Inherits Form

        Private _service As New SettingsService()

        Private Sub ThemeSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            LoadThemes()
        End Sub

        Private Sub LoadThemes()
            dgvThemes.Rows.Clear()
            Try
                Dim dt = Sql.ExecuteTable("SELECT ID, ThemeName, ThemeColor, ThemeImage FROM TemForm")
                Dim activeColor = _service.GetSettingValue("AdvancedFormThemeColor", "")
                
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim row = dt.Rows(i)
                    Dim colorHex = Convert.ToString(row("ThemeColor"))
                    Dim isSelected = (colorHex = activeColor)
                    
                    Dim img As Image = Nothing
                    If Not row.IsNull("ThemeImage") Then
                        Dim bytes = DirectCast(row("ThemeImage"), Byte())
                        Using ms As New MemoryStream(bytes)
                            img = Image.FromStream(ms)
                        End Using
                    End If
                    
                    dgvThemes.Rows.Add(i + 1, isSelected, row("ThemeName"), Nothing, img, colorHex)
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری تم‌ها: " & ex.Message)
            End Try
        End Sub

        Private Sub dgvThemes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvThemes.CellContentClick
            If e.RowIndex >= 0 Then
                If e.ColumnIndex = colSelect.Index Then
                    ' Mimic RadioButton behavior
                    For i As Integer = 0 To dgvThemes.Rows.Count - 1
                        If i <> e.RowIndex Then
                            dgvThemes.Rows(i).Cells(colSelect.Index).Value = False
                        End If
                    Next
                    dgvThemes.Rows(e.RowIndex).Cells(colSelect.Index).Value = True
                    dgvThemes.EndEdit()
                ElseIf e.ColumnIndex = colShowImage.Index Then
                    ' Show image popup
                    Dim img = TryCast(dgvThemes.Rows(e.RowIndex).Cells(colPreview.Index).Value, Image)
                    Dim themeName = Convert.ToString(dgvThemes.Rows(e.RowIndex).Cells(colName.Index).Value)
                    If img IsNot Nothing Then
                        Using frm As New ImagePopupForm(img, themeName)
                            frm.ShowDialog(Me)
                        End Using
                    End If
                End If
            End If
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim selectedColorHex As String = ""
            For i As Integer = 0 To dgvThemes.Rows.Count - 1
                Dim isSelected As Boolean = Convert.ToBoolean(dgvThemes.Rows(i).Cells(colSelect.Index).Value)
                If isSelected Then
                    selectedColorHex = Convert.ToString(dgvThemes.Rows(i).Cells(colColorHex.Index).Value)
                    Exit For
                End If
            Next
            
            If Not String.IsNullOrEmpty(selectedColorHex) Then
                _service.SaveSetting("AdvancedFormThemeColor", selectedColorHex, "UI")
                SessionContext.CurrentFormThemeColorHex = selectedColorHex
                MessageBox.Show("تم فرم‌ها با موفقیت ذخیره شد. برای اعمال کامل روی سایر فرم‌ها، آن‌ها را ببندید و مجدداً باز کنید.", "ثبت موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show("لطفاً یک تم را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End Sub
    End Class
End Namespace
