Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class UpdateInstallerForm
        Private _updateSourceDir As String

        Public Sub New()
            Me.New(Path.Combine(Application.StartupPath, "update_files"))
        End Sub

        Public Sub New(updateSourceDir As String)
            InitializeComponent()
            _updateSourceDir = updateSourceDir
        End Sub

        Private Sub UpdateInstallerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            AppIconHelper.ApplyAppIcon(Me)
            ' مسیر پیشنهاد شده: اگر برنامه نصب شده در C:\Sys_Hes_Anb وجود داشت
            If Directory.Exists("C:\Sys_Hes_Anb") Then
                txtTargetDir.Text = "C:\Sys_Hes_Anb"
            Else
                txtTargetDir.Text = Application.StartupPath
            End If
        End Sub

        Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
            Using fbd As New FolderBrowserDialog()
                fbd.Description = "لطفاً پوشه محل نصب فعلی نرم‌افزار را انتخاب کنید:"
                fbd.SelectedPath = txtTargetDir.Text
                If fbd.ShowDialog() = DialogResult.OK Then
                    txtTargetDir.Text = fbd.SelectedPath
                End If
            End Using
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub

        Private Sub BtnStartUpdate_Click(sender As Object, e As EventArgs) Handles btnStartUpdate.Click
            Dim targetDir = txtTargetDir.Text.Trim()
            If String.IsNullOrWhiteSpace(targetDir) OrElse Not Directory.Exists(targetDir) Then
                MessageBox.Show("لطفاً پوشه معتبری از محل نصب نرم‌افزار انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not Directory.Exists(_updateSourceDir) Then
                If File.Exists(Path.Combine(Application.StartupPath, "Sys_Hes_Anb.exe")) Then
                    _updateSourceDir = Application.StartupPath
                Else
                    MessageBox.Show("فایلهای منبع به‌روزرسانی یافت نشدند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            End If

            btnStartUpdate.Enabled = False
            btnBrowse.Enabled = False
            txtTargetDir.Enabled = False
            progressBar.Visible = True
            progressBar.Style = ProgressBarStyle.Marquee

            Try
                ' ۱. پشتیبان‌گیری خودکار از دیتابیس فعلی کاربر (.dat)
                lblStatus.Text = "در حال پشتیبان‌گیری خودکار از دیتابیس فعلی..."
                Application.DoEvents()

                Dim clientDbFolder = Path.Combine(targetDir, "Database")
                Dim clientDatPath = Path.Combine(clientDbFolder, "Sys_Hes_Anb.dat")
                If File.Exists(clientDatPath) Then
                    Dim backupDir = Path.Combine(clientDbFolder, "Backups")
                    If Not Directory.Exists(backupDir) Then
                        Directory.CreateDirectory(backupDir)
                    End If
                    Dim backupFileName = "Sys_Hes_Anb_backup_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".dat"
                    File.Copy(clientDatPath, Path.Combine(backupDir, backupFileName), True)
                End If

                ' خواندن تنظیمات درباره... و ارتباط با ما از دیتابیس توسعه‌دهنده (دیتابیس رمزگذاری‌شده .dat)
                Dim devAboutText As String = ""
                Dim devContactText As String = ""
                Dim devDatPath = Path.Combine(_updateSourceDir, "Database", "Sys_Hes_Anb.dat")

                If File.Exists(devDatPath) Then
                    Dim tempDevDb = Path.Combine(Path.GetTempPath(), "temp_dev_" & Guid.NewGuid().ToString() & ".db")
                    Try
                        AesDbService.DecryptFile(devDatPath, tempDevDb)
                        If File.Exists(tempDevDb) Then
                            Using conn As New System.Data.SQLite.SQLiteConnection("Data Source=" & tempDevDb & ";Version=3;")
                                conn.Open()
                                Using cmd As New System.Data.SQLite.SQLiteCommand("SELECT SettingValue FROM AppSettings WHERE SettingKey = 'AboutText'", conn)
                                    devAboutText = Convert.ToString(cmd.ExecuteScalar())
                                End Using
                                Using cmd As New System.Data.SQLite.SQLiteCommand("SELECT SettingValue FROM AppSettings WHERE SettingKey = 'ContactText'", conn)
                                    devContactText = Convert.ToString(cmd.ExecuteScalar())
                                End Using
                            End Using
                        End If
                    Catch
                        ' نادیده گرفتن خطا
                    Finally
                        If File.Exists(tempDevDb) Then
                            Try : File.Delete(tempDevDb) : Catch : End Try
                        End If
                    End Try
                End If

                ' ۲. اعمال کدهای جدید و جایگزینی فایلها
                lblStatus.Text = "در حال نوسازی فایل‌های سیستم..."
                Application.DoEvents()

                CopyUpdateFilesRecursive(_updateSourceDir, targetDir)

                ' ۳. ادغام و اعمال تنظیمات توسعه‌دهنده به دیتابیس کاربر مقصد با رعایت رمزگذاری دیتابیس
                If Not String.IsNullOrEmpty(devAboutText) OrElse Not String.IsNullOrEmpty(devContactText) Then
                    lblStatus.Text = "در حال اعمال تنظیمات عمومی جدید..."
                    Application.DoEvents()
                    MergeSettingsToClientDat(clientDatPath, devAboutText, devContactText)
                End If

                lblStatus.Text = "به‌روزرسانی با موفقیت انجام شد."
                progressBar.Visible = False

                Dim res = MessageBox.Show("نرم‌افزار با موفقیت به نسخه جدید ارتقا یافت و اطلاعات قبلی شما ۱۰۰٪ محفوظ ماند." & Environment.NewLine & Environment.NewLine & "آیا می‌خواهید نرم‌افزار هم‌اکنون اجرا شود؟", "موفقیت در به‌روزرسانی", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If res = DialogResult.Yes Then
                    Dim exePath = Path.Combine(targetDir, "Sys_Hes_Anb.exe")
                    If File.Exists(exePath) Then
                        Process.Start(exePath)
                    End If
                End If
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در حین ارتقای نرم‌افزار: " & ex.Message, "خطای به‌روزرسانی", MessageBoxButtons.OK, MessageBoxIcon.Error)
                btnStartUpdate.Enabled = True
                btnBrowse.Enabled = True
                txtTargetDir.Enabled = True
                progressBar.Visible = False
                lblStatus.Text = ""
            End Try
        End Sub

        Private Sub MergeSettingsToClientDat(clientDatPath As String, aboutText As String, contactText As String)
            If Not File.Exists(clientDatPath) Then Return
            Dim tempClientDb = Path.Combine(Path.GetTempPath(), "temp_client_" & Guid.NewGuid().ToString() & ".db")
            Try
                ' دکریپت کردن فایل dat به db موقت
                AesDbService.DecryptFile(clientDatPath, tempClientDb)

                If File.Exists(tempClientDb) Then
                    ' ویرایش تنظیمات در db موقت
                    Using conn As New System.Data.SQLite.SQLiteConnection("Data Source=" & tempClientDb & ";Version=3;")
                        conn.Open()

                        ' بروزرسانی یا درج متن درباره نرم‌افزار
                        If Not String.IsNullOrEmpty(aboutText) Then
                            Using cmd As New System.Data.SQLite.SQLiteCommand(conn)
                                cmd.CommandText = "SELECT COUNT(*) FROM AppSettings WHERE SettingKey = 'AboutText'"
                                Dim exists = Convert.ToInt32(cmd.ExecuteScalar())
                                If exists > 0 Then
                                    cmd.CommandText = "UPDATE AppSettings SET SettingValue = ? WHERE SettingKey = 'AboutText'"
                                Else
                                    cmd.CommandText = "INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES ('AboutText', ?, 'Info')"
                                End If
                                cmd.Parameters.Add(New System.Data.SQLite.SQLiteParameter(System.Data.DbType.String) With {.Value = aboutText})
                                cmd.ExecuteNonQuery()
                            End Using
                        End If

                        ' بروزرسانی یا درج متن ارتباط با ما
                        If Not String.IsNullOrEmpty(contactText) Then
                            Using cmd As New System.Data.SQLite.SQLiteCommand(conn)
                                cmd.CommandText = "SELECT COUNT(*) FROM AppSettings WHERE SettingKey = 'ContactText'"
                                Dim exists = Convert.ToInt32(cmd.ExecuteScalar())
                                If exists > 0 Then
                                    cmd.CommandText = "UPDATE AppSettings SET SettingValue = ? WHERE SettingKey = 'ContactText'"
                                Else
                                    cmd.CommandText = "INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES ('ContactText', ?, 'Info')"
                                End If
                                cmd.Parameters.Add(New System.Data.SQLite.SQLiteParameter(System.Data.DbType.String) With {.Value = contactText})
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    End Using

                    ' انکریپت کردن مجدد db موقت به فایل dat مقصد
                    AesDbService.EncryptFile(tempClientDb, clientDatPath)
                End If
            Catch ex As Exception
                ' نادیده گرفتن خطا
            Finally
                If File.Exists(tempClientDb) Then
                    Try : File.Delete(tempClientDb) : Catch : End Try
                End If
            End Try
        End Sub

        Private Sub CopyUpdateFilesRecursive(sourceDir As String, targetDir As String)
            Dim dir As New DirectoryInfo(sourceDir)
            If Not dir.Exists Then Return

            If Not Directory.Exists(targetDir) Then
                Directory.CreateDirectory(targetDir)
            End If

            For Each fileInDir As FileInfo In dir.GetFiles()
                ' **مهمترین اصل**: عدم کپی یا اوررایت کردن دیتابیس، عکسها، لاگها و فایل آپدیت‌کننده
                If String.Equals(fileInDir.Extension, ".db", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".dat", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".db-wal", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".db-shm", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".sqlite", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".sqlite3", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".accdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".laccdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".mdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".ldb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".log", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".tmp", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Name, "Update_Sys_Hes_Anb.exe", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Name, "Setup_Sys_Hes_Anb.exe", StringComparison.OrdinalIgnoreCase) Then Continue For

                Dim temppath As String = Path.Combine(targetDir, fileInDir.Name)
                fileInDir.CopyTo(temppath, True)
            Next

            For Each subdir As DirectoryInfo In dir.GetDirectories()
                ' عدم کپی پوشه‌های بک‌آپ یا خروجی انتشار
                If String.Equals(subdir.Name, "Backups", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(subdir.Name, "Enteshar", StringComparison.OrdinalIgnoreCase) Then Continue For

                Dim temppath As String = Path.Combine(targetDir, subdir.Name)
                CopyUpdateFilesRecursive(subdir.FullName, temppath)
            Next
        End Sub
    End Class
End Namespace
