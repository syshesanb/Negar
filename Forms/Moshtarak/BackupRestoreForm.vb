Option Strict Off
Option Explicit On

Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class BackupRestoreForm
        Public Enum OperationMode
            Backup
            Restore
        End Enum

        Private _mode As OperationMode

        Public Sub New(mode As OperationMode)
            InitializeComponent()
            _mode = mode
        End Sub

        Private Sub BackupRestoreForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If _mode = OperationMode.Backup Then
                Me.Text = "پشتیبان‌گیری از اطلاعات سیستم"
                lblTitle.Text = "پشتیبان‌گیری از دیتابیس و اطلاعات"
                lblDesc.Text = "یک فایل فشرده و رمزنگاری‌شده حاوی کلیه اطلاعات سیستم در مسیر انتخابی ذخیره خواهد شد."
                btnExecute.Text = "شروع پشتیبان‌گیری"

                Dim defaultFileName As String = BackupRestoreService.GenerateBackupFileName()
                Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                txtPath.Text = Path.Combine(desktopPath, defaultFileName)
            Else
                Me.Text = "بازیابی اطلاعات سیستم"
                lblTitle.Text = "بازیابی اطلاعات از فایل پشتیبان"
                lblDesc.Text = "لطفاً فایل پشتیبان (ZIP) را انتخاب کنید. اطلاعات موجود در فایل جایگزین اطلاعات فعلی خواهد شد."
                btnExecute.Text = "شروع بازیابی"
                txtPath.Text = ""
            End If
        End Sub

        Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
            If _mode = OperationMode.Backup Then
                Using sfd As New SaveFileDialog()
                    sfd.Title = "مسیر ذخیره فایل پشتیبان"
                    sfd.Filter = "فایل فشرده پشتیبان (*.zip)|*.zip"
                    sfd.FileName = Path.GetFileName(txtPath.Text)
                    If sfd.ShowDialog() = DialogResult.OK Then
                        txtPath.Text = sfd.FileName
                    End If
                End Using
            Else
                Using ofd As New OpenFileDialog()
                    ofd.Title = "انتخاب فایل پشتیبان جهت بازیابی"
                    ofd.Filter = "فایل فشرده پشتیبان (*.zip)|*.zip"
                    If ofd.ShowDialog() = DialogResult.OK Then
                        txtPath.Text = ofd.FileName
                    End If
                End Using
            End If
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub BtnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
            Dim filePath As String = txtPath.Text.Trim()
            If String.IsNullOrWhiteSpace(filePath) Then
                MessageBox.Show("لطفاً مسیر فایل را تعیین کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _mode = OperationMode.Restore Then
                If Not File.Exists(filePath) Then
                    MessageBox.Show("فایل پشتیبان انتخاب‌شده در مسیر مشخص‌شده یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim confirm = MessageBox.Show("هشدار بسیار مهم!" & Environment.NewLine & Environment.NewLine & "با انجام بازیابی، تمام اطلاعات فعلی نرم‌افزار جایگزین اطلاعات موجود در این فایل پشتیبان خواهند شد." & Environment.NewLine & Environment.NewLine & "آیا از انجام عملیات بازیابی اطمینان کامل دارید؟", "تایید بازیابی اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                If confirm <> DialogResult.Yes Then Return
            End If

            btnExecute.Enabled = False
            btnBrowse.Enabled = False
            txtPath.Enabled = False
            progressBar.Visible = True
            progressBar.Style = ProgressBarStyle.Marquee

            Try
                If _mode = OperationMode.Backup Then
                    lblStatus.Text = "در حال فشرده‌سازی و رمزنگاری اطلاعات..."
                    Application.DoEvents()
                    BackupRestoreService.CreateBackup(filePath)
                    lblStatus.Text = "پشتیبان‌گیری با موفقیت انجام شد."
                    MessageBox.Show("پشتیبان‌گیری از اطلاعات با موفقیت انجام شد و فایل در مسیر زیر قرار گرفت:" & Environment.NewLine & Environment.NewLine & filePath, "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    lblStatus.Text = "در حال رمزگشایی و بازیابی دیتابیس..."
                    Application.DoEvents()
                    BackupRestoreService.RestoreBackup(filePath)
                    lblStatus.Text = "بازیابی با موفقیت انجام شد."
                    MessageBox.Show("اطلاعات سیستم با موفقیت از فایل پشتیبان بازیابی شد." & Environment.NewLine & Environment.NewLine & "جهت اعمال کامل تغییرات، نرم‌افزار به صورت خودکار راه‌اندازی مجدد می‌شود.", "موفقیت در بازیابی", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Application.Restart()
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در انجام عملیات: " & ex.Message, "خطای پشتیبان‌گیری/بازیابی", MessageBoxButtons.OK, MessageBoxIcon.Error)
                btnExecute.Enabled = True
                btnBrowse.Enabled = True
                txtPath.Enabled = True
                progressBar.Visible = False
                lblStatus.Text = ""
            End Try
        End Sub
    End Class
End Namespace
