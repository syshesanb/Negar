Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms
    Public Class SetupInstallerForm
        Private _sourceDirectory As String

        Public Sub New()
            Me.New(Path.Combine(Application.StartupPath, "app_files"))
        End Sub

        Public Sub New(sourceDir As String)
            InitializeComponent()
            _sourceDirectory = sourceDir
        End Sub

        Private Sub SetupInstallerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Negar.Business.ThemeHelper.ApplyFormTheme(Me)
            AppIconHelper.ApplyAppIcon(Me)
        End Sub

        Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
            Using fbd As New FolderBrowserDialog()
                fbd.Description = "لطفاً مسیر نصب نرم‌افزار را انتخاب کنید:"
                fbd.SelectedPath = txtInstallPath.Text
                If fbd.ShowDialog() = DialogResult.OK Then
                    txtInstallPath.Text = fbd.SelectedPath
                End If
            End Using
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub

        Private Sub BtnInstall_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
            Dim targetDir = txtInstallPath.Text.Trim()
            If String.IsNullOrWhiteSpace(targetDir) Then
                MessageBox.Show("لطفاً مسیر نصب معتبری وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not Directory.Exists(_sourceDirectory) Then
                ' اگر پوشه app_files وجود نداشت، خود پوشه فعلی را منبع فرض می‌کنیم
                If File.Exists(Path.Combine(Application.StartupPath, "Negar.exe")) Then
                    _sourceDirectory = Application.StartupPath
                Else
                    MessageBox.Show("فایلهای منبع نصب یافت نشدند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            End If

            btnInstall.Enabled = False
            btnBrowse.Enabled = False
            txtInstallPath.Enabled = False
            progressBar.Visible = True
            progressBar.Style = ProgressBarStyle.Marquee
            lblStatus.Text = "در حال کپی فایلهای سیستم و استقرار دیتابیس خام..."

            Application.DoEvents()

            Try
                If Not Directory.Exists(targetDir) Then
                    Directory.CreateDirectory(targetDir)
                End If

                CopyDirectoryRecursive(_sourceDirectory, targetDir)

                If chkCreateShortcut.Checked Then
                    CreateDesktopShortcut(Path.Combine(targetDir, "Negar.exe"), "سیستم حسابداری و انبارداری")
                End If

                lblStatus.Text = "نصب با موفقیت انجام شد."
                progressBar.Visible = False

                Dim res = MessageBox.Show("نصب نرم‌افزار در مسیر زیر با موفقیت به پایان رسید:" & Environment.NewLine & targetDir & Environment.NewLine & Environment.NewLine & "آیا می‌خواهید نرم‌افزار هم‌اکنون اجرا شود؟", "موفقیت در نصب", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If res = DialogResult.Yes Then
                    Dim exePath = Path.Combine(targetDir, "Negar.exe")
                    If File.Exists(exePath) Then
                        Process.Start(exePath)
                    End If
                End If
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در حین نصب برنامه: " & ex.Message, "خطای نصب", MessageBoxButtons.OK, MessageBoxIcon.Error)
                btnInstall.Enabled = True
                btnBrowse.Enabled = True
                txtInstallPath.Enabled = True
                progressBar.Visible = False
                lblStatus.Text = ""
            End Try
        End Sub

        Private Sub CopyDirectoryRecursive(sourceDir As String, targetDir As String)
            Dim dir As New DirectoryInfo(sourceDir)
            If Not dir.Exists Then Return

            If Not Directory.Exists(targetDir) Then
                Directory.CreateDirectory(targetDir)
            End If

            For Each fileInDir As FileInfo In dir.GetFiles()
                ' نادیده گرفتن فایلهای موقت یا دیتابیس اکسس یا خود فایل نصب کننده
                If String.Equals(fileInDir.Name, "Setup_Negar.exe", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".tmp", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".accdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".laccdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".mdb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Extension, ".ldb", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(fileInDir.Name, "bootstrap.log", StringComparison.OrdinalIgnoreCase) Then Continue For

                Dim temppath As String = Path.Combine(targetDir, fileInDir.Name)
                fileInDir.CopyTo(temppath, True)
            Next

            For Each subdir As DirectoryInfo In dir.GetDirectories()
                Dim temppath As String = Path.Combine(targetDir, subdir.Name)
                CopyDirectoryRecursive(subdir.FullName, temppath)
            Next
        End Sub

        Private Sub CreateDesktopShortcut(targetExePath As String, shortcutName As String)
            Try
                Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                ' تلاش اول: ایجاد میانبر حقیقی و استاندار ویندوز (.lnk) با WScript.Shell
                Try
                    Dim shellType As Type = Type.GetTypeFromProgID("WScript.Shell")
                    If shellType IsNot Nothing Then
                        Dim shell As Object = Activator.CreateInstance(shellType)
                        Dim lnkPath As String = Path.Combine(desktopPath, shortcutName & ".lnk")
                        Dim shortcut As Object = shellType.InvokeMember("CreateShortcut", Reflection.BindingFlags.InvokeMethod, Nothing, shell, New Object() {lnkPath})
                        
                        Dim targetDir As String = Path.GetDirectoryName(targetExePath)
                        shortcut.GetType().InvokeMember("TargetPath", Reflection.BindingFlags.SetProperty, Nothing, shortcut, New Object() {targetExePath})
                        shortcut.GetType().InvokeMember("WorkingDirectory", Reflection.BindingFlags.SetProperty, Nothing, shortcut, New Object() {targetDir})
                        shortcut.GetType().InvokeMember("IconLocation", Reflection.BindingFlags.SetProperty, Nothing, shortcut, New Object() {targetExePath & ",0"})
                        shortcut.GetType().InvokeMember("Save", Reflection.BindingFlags.InvokeMethod, Nothing, shortcut, Nothing)
                        Return
                    End If
                Catch
                End Try

                ' تلاش دوم: جایگزین رزرو با ساخت میانبر .url
                Dim shortcutPath As String = Path.Combine(desktopPath, shortcutName & ".url")
                Using writer As New StreamWriter(shortcutPath, False)
                    writer.WriteLine("[InternetShortcut]")
                    writer.WriteLine("URL=file:///" & targetExePath.Replace("\", "/"))
                    writer.WriteLine("IconIndex=0")
                    writer.WriteLine("IconFile=" & targetExePath.Replace("\", "/"))
                End Using
            Catch
            End Try
        End Sub
    End Class
End Namespace
