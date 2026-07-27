Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Windows.Forms
Imports Negar.Data

Namespace Negar.Business
    Public Class SettingsService
        Public Function GetSettings() As DataTable
            Return Sql.ExecuteTable("SELECT SettingID, SettingKey, SettingValue, SettingCategory FROM AppSettings ORDER BY SettingCategory, SettingKey")
        End Function

        Public Shared ReadOnly DefaultAboutText As String =
            "سیستم جامع حسابداری و انبارداری" & Environment.NewLine &
            "نسخه 1.0.0" & Environment.NewLine &
            "طراحی و پیاده‌سازی جهت مدیریت هوشمند مالی، حسابداری و کالاها."

        Public Shared ReadOnly DefaultContactText As String =
            "ارتباط با پشتیبانی:" & Environment.NewLine &
            "تلفن پشتیبانی: 021-12345678" & Environment.NewLine &
            "ایمیل: support@example.com" & Environment.NewLine &
            "وب‌سایت: www.example.com"

        Public Function GetSettingValue(settingKey As String, Optional defaultValue As String = "") As String
            Dim value = Sql.ExecuteScalar("SELECT SettingValue FROM AppSettings WHERE SettingKey = ? LIMIT 1", settingKey)
            If value Is Nothing OrElse Convert.IsDBNull(value) Then Return defaultValue
            Return Convert.ToString(value)
        End Function

        Public Sub SaveSetting(settingKey As String, settingValue As String, settingCategory As String)
            Dim exists = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM AppSettings WHERE SettingKey = ?", settingKey), 0))
            If exists > 0 Then
                Sql.ExecuteNonQuery("UPDATE AppSettings SET SettingValue = ?, SettingCategory = ? WHERE SettingKey = ?", settingValue, settingCategory, settingKey)
            Else
                Sql.ExecuteNonQuery("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES (?, ?, ?)", settingKey, settingValue, settingCategory)
            End If
        End Sub
    End Class

    Public Module BackgroundImageService
        Public Sub EnsureDatabaseTableAndSeed()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS BackgroundImages (" &
                    "ImageID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ImageName TEXT, " &
                    "ImageData BLOB, " &
                    "CreatedDate DATETIME);")

                Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM BackgroundImages"), 0))
                If count > 0 Then Return

                ' Seed generated background images if available on disk or generate programmatic gradients
                Dim seedPaths As String() = New String() {
                    "C:\Users\Rayanegostar\.gemini\antigravity\brain\9815d4f0-fc2b-4523-a855-9be783685916\gradient_bg_1_1782577144027.png",
                    "C:\Users\Rayanegostar\.gemini\antigravity\brain\9815d4f0-fc2b-4523-a855-9be783685916\gradient_bg_2_1782577158564.png",
                    "C:\Users\Rayanegostar\.gemini\antigravity\brain\9815d4f0-fc2b-4523-a855-9be783685916\gradient_bg_3_1782577171499.png"
                }

                Dim idx As Integer = 1
                For Each imgPath As String In seedPaths
                    If File.Exists(imgPath) Then
                        Try
                            Dim bytes As Byte() = File.ReadAllBytes(imgPath)
                            Sql.ExecuteNonQuery(
                                "INSERT INTO BackgroundImages (ImageName, ImageData, CreatedDate) VALUES (?, ?, ?)",
                                "Gradient_Theme_" & idx, bytes, DateTime.Now)
                            idx += 1
                        Catch
                        End Try
                    End If
                Next

                ' If still empty, seed programmatically generated soft gradients into database
                If Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM BackgroundImages"), 0)) = 0 Then
                    SeedProgrammaticGradients()
                End If
            Catch ex As Exception
            End Try
        End Sub

        Private Sub SeedProgrammaticGradients()
            Dim themeGradients As Tuple(Of String, Color, Color)() = New Tuple(Of String, Color, Color)() {
                Tuple.Create("Theme_Dark_Navy", Color.FromArgb(15, 23, 42), Color.FromArgb(71, 85, 105)),
                Tuple.Create("Theme_Charcoal_Silver", Color.FromArgb(24, 24, 27), Color.FromArgb(113, 113, 122)),
                Tuple.Create("Theme_Emerald_Mint", Color.FromArgb(6, 78, 59), Color.FromArgb(52, 211, 153)),
                Tuple.Create("Theme_Deep_Purple_Lavender", Color.FromArgb(46, 16, 101), Color.FromArgb(192, 132, 252))
            }

            For Each theme As Tuple(Of String, Color, Color) In themeGradients
                Using bmp As New Bitmap(1920, 1080)
                    Using g As Graphics = Graphics.FromImage(bmp)
                        g.SmoothingMode = SmoothingMode.AntiAlias
                        Using br As New LinearGradientBrush(New Rectangle(0, 0, 1920, 1080), theme.Item2, theme.Item3, LinearGradientMode.Vertical)
                            g.FillRectangle(br, 0, 0, 1920, 1080)
                        End Using
                    End Using
                    Using ms As New MemoryStream()
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                        Sql.ExecuteNonQuery(
                            "INSERT INTO BackgroundImages (ImageName, ImageData, CreatedDate) VALUES (?, ?, ?)",
                            theme.Item1, ms.ToArray(), DateTime.Now)
                    End Using
                End Using
            Next
        End Sub

        Public Function GetRandomBackgroundImage() As Image
            Try
                EnsureDatabaseTableAndSeed()
                Dim dt As DataTable = Sql.ExecuteTable("SELECT ImageData FROM BackgroundImages ORDER BY RANDOM() LIMIT 1")
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso Not dt.Rows(0).IsNull("ImageData") Then
                    Dim bytes As Byte() = CType(dt.Rows(0)("ImageData"), Byte())
                    Using ms As New MemoryStream(bytes)
                        Using tempImg = Image.FromStream(ms)
                            Return New Bitmap(tempImg)
                        End Using
                    End Using
                End If
            Catch ex As Exception
            End Try

            ' Fallback dynamic gradient
            Dim fallbackBmp As New Bitmap(1920, 1080)
            Using g As Graphics = Graphics.FromImage(fallbackBmp)
                Using br As New LinearGradientBrush(New Rectangle(0, 0, 1920, 1080), Color.FromArgb(20, 30, 48), Color.FromArgb(36, 59, 85), LinearGradientMode.Vertical)
                    g.FillRectangle(br, 0, 0, 1920, 1080)
                End Using
            End Using
            Return fallbackBmp
        End Function
    End Module

    Public Module ReleaseBuilderService
        Public Sub CreateReleasePackage(managerUserId As Integer, managerPassword As String)
            Try
                ' 1. خواندن اطلاعات کاربر میانی انتخاب شده از دیتابیس فعلی
                Dim userDt = Sql.ExecuteTable("SELECT UserID, Username, FullName, UserType, MaxCompaniesAllowed, MaxFiscalYearsPerCompany FROM Users WHERE UserID = ?", managerUserId)
                If userDt Is Nothing OrElse userDt.Rows.Count = 0 Then
                    MessageBox.Show("اطلاعات کاربر میانی انتخاب شده یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim managerRow = userDt.Rows(0)
                Dim managerUsername = Convert.ToString(managerRow("Username"))
                Dim managerFullName = Convert.ToString(managerRow("FullName"))

                ' 2. تعیین و ساخت پوشه Enteshar در کنار فایل اجرایی
                Dim projectRoot As String = Application.StartupPath
                Dim entesharDir As String = Path.Combine(projectRoot, "Enteshar")
                If Not Directory.Exists(entesharDir) Then
                    Directory.CreateDirectory(entesharDir)
                End If

                Dim packageDir As String = Path.Combine(entesharDir, "Setup_Negar")
                If Directory.Exists(packageDir) Then
                    Try
                        Directory.Delete(packageDir, True)
                    Catch
                    End Try
                End If
                Directory.CreateDirectory(packageDir)

                Dim appFilesDir As String = Path.Combine(packageDir, "app_files")
                Directory.CreateDirectory(appFilesDir)

                ' 3. کپی تمامی فایلهای خروجی سیستم (EXE, DLLs, Config, Interop) به app_files
                Dim sourceBinDir As String = Application.StartupPath
                CopyDirectoryRecursive(sourceBinDir, appFilesDir)

                ' 4. تولید دیتابیس خام و آماده به کار (Clean DB)
                Dim appFilesDbDir As String = Path.Combine(appFilesDir, "Database")
                If Not Directory.Exists(appFilesDbDir) Then
                    Directory.CreateDirectory(appFilesDbDir)
                End If
                Dim cleanDbPath As String = Path.Combine(appFilesDbDir, "Negar.db")
                CreateCleanDatabase(cleanDbPath, projectRoot, managerRow, managerPassword)

                Dim cleanDatPath As String = Path.Combine(appFilesDbDir, "Negar.dat")
                Data.AesDbService.EncryptFile(cleanDbPath, cleanDatPath)
                If File.Exists(cleanDatPath) AndAlso New FileInfo(cleanDatPath).Length > 0 Then
                    If File.Exists(cleanDbPath) Then File.Delete(cleanDbPath)
                End If

                ' 5. ایجاد فایل نصبی اصلی Setup_Negar.exe در پوشه Enteshar
                Dim setupExeSource As String = Path.Combine(sourceBinDir, "Negar.exe")
                Dim setupExeTarget As String = Path.Combine(packageDir, "Setup_Negar.exe")
                If File.Exists(setupExeSource) Then
                    File.Copy(setupExeSource, setupExeTarget, True)
                End If

                ' کپی DLLهای وابسته برای اجرای فایل نصب‌کننده در پوشه Package
                For Each dllFile As String In Directory.GetFiles(sourceBinDir, "*.dll")
                    Dim fileName As String = Path.GetFileName(dllFile)
                    File.Copy(dllFile, Path.Combine(packageDir, fileName), True)
                Next
                For Each configFile As String In Directory.GetFiles(sourceBinDir, "*.config")
                    Dim fileName As String = Path.GetFileName(configFile)
                    File.Copy(configFile, Path.Combine(packageDir, fileName), True)
                Next
                If Directory.Exists(Path.Combine(sourceBinDir, "x86")) Then
                    CopyDirectoryRecursive(Path.Combine(sourceBinDir, "x86"), Path.Combine(packageDir, "x86"))
                End If
                If Directory.Exists(Path.Combine(sourceBinDir, "x64")) Then
                    CopyDirectoryRecursive(Path.Combine(sourceBinDir, "x64"), Path.Combine(packageDir, "x64"))
                End If

                ' ایجاد پوشه پیش‌نیازها و دانلود خودکار VC++ Redistributable
                DownloadPrerequisites(packageDir)

                ' ایجاد فایل راهنمای ورود برای نصب‌کننده
                Dim readmePath As String = Path.Combine(packageDir, "راهنمای نصب و ورود.txt")
                Dim rlm As String = Char.ConvertFromUtf32(&H200F)
                Dim sbReadme As New System.Text.StringBuilder()
                sbReadme.AppendLine(rlm & "==================================================")
                sbReadme.AppendLine(rlm & "           راهنمای نصب و ورود به سیستم            ")
                sbReadme.AppendLine(rlm & "==================================================")
                sbReadme.AppendLine(rlm)
                sbReadme.AppendLine(rlm & "اطلاعات ورود به نرم‌افزار جهت استفاده نصب‌کننده:")
                sbReadme.AppendLine(rlm & "--------------------------------------------------")
                sbReadme.AppendLine(rlm & "نام کاربر: " & managerFullName)
                sbReadme.AppendLine(rlm & "نام کاربری (کاربر میانی): " & managerUsername)
                sbReadme.AppendLine(rlm & "کلمه عبور اولیه: " & managerPassword)
                sbReadme.AppendLine(rlm & "--------------------------------------------------")
                sbReadme.AppendLine(rlm)
                sbReadme.AppendLine(rlm & "دستورالعمل نصب:")
                sbReadme.AppendLine(rlm & "جهت نصب نرم‌افزار روی هر کامپیوتر دیگر، کافی است فایل Setup_Negar.exe را اجرا کنید.")
                sbReadme.AppendLine(rlm & "پس از تعیین مسیر نصب توسط نصب‌کننده، نرم‌افزار به همراه دیتابیس خام و مجوزهای تعیین‌شده مستقر خواهد شد.")
                sbReadme.AppendLine(rlm)
                sbReadme.AppendLine(rlm & "نکته بسیار مهم:")
                sbReadme.AppendLine(rlm & "در صورتی که نرم‌افزار پس از نصب در کامپیوتر مقصد اجرا نشد یا با خطای SQLite (مانند DllNotFoundException برای SQLite.Interop.dll) مواجه شدید،")
                sbReadme.AppendLine(rlm & "لطفاً فایل‌های پیش‌نیاز موجود در پوشه Prerequisites (نسخه vc_redist.x86.exe یا vc_redist.x64.exe متناسب با سیستم‌عامل مقصد) را نصب نمایید.")

                File.WriteAllText(readmePath, sbReadme.ToString(), System.Text.Encoding.UTF8)

                MessageBox.Show("نسخه قابل انتشار با موفقیت تولید شد!" & Environment.NewLine & Environment.NewLine & "مشخصات کاربر میانی (" & managerUsername & ") در دیتابیس خام قرار گرفت و فایل‌های پیش‌نیاز (Visual C++) نیز در پوشه Prerequisites دانلود گردید." & Environment.NewLine & Environment.NewLine & "مسیر نسخه خروجی:" & Environment.NewLine & packageDir, "موفقیت در ایجاد نسخه قابل انتشار", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Process.Start("explorer.exe", entesharDir)
            Catch ex As Exception
                MessageBox.Show("خطا در ایجاد نسخه قابل انتشار: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub CreateCleanDatabase(dbPath As String, projectRoot As String, managerRow As DataRow, managerPassword As String)
            Try
                If File.Exists(dbPath) Then
                    File.Delete(dbPath)
                End If

                Dim connStr As String = "Data Source=" & dbPath & ";Version=3;"
                Using conn As New System.Data.SQLite.SQLiteConnection(connStr)
                    conn.Open()

                    ' اجرا اسکریپت ساخت جداول
                    Dim schemaSqlPath As String = Path.Combine(projectRoot, "Database", "CreateSchema.sql")
                    If Not File.Exists(schemaSqlPath) Then
                        Throw New FileNotFoundException("اسکریپت ساخت جداول دیتابیس یافت نشد: " & schemaSqlPath)
                    End If
                    Dim sqlScript As String = File.ReadAllText(schemaSqlPath)
                    Using cmd As New System.Data.SQLite.SQLiteCommand(sqlScript, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' ساخت و کپی تصاویر پس‌زمینه با تم‌های ملایم به دیتابیس خام نسخه انتشار
                    Using cmd As New System.Data.SQLite.SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS BackgroundImages (ImageID INTEGER PRIMARY KEY AUTOINCREMENT, ImageName TEXT, ImageData BLOB, CreatedDate DATETIME);", conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    Dim bgDt = Sql.ExecuteTable("SELECT ImageName, ImageData FROM BackgroundImages")
                    If bgDt IsNot Nothing AndAlso bgDt.Rows.Count > 0 Then
                        For Each bgRow As DataRow In bgDt.Rows
                            Using cmd As New System.Data.SQLite.SQLiteCommand("INSERT INTO BackgroundImages (ImageName, ImageData, CreatedDate) VALUES (@name, @data, DATETIME('now'))", conn)
                                cmd.Parameters.AddWithValue("@name", bgRow("ImageName"))
                                cmd.Parameters.AddWithValue("@data", bgRow("ImageData"))
                                cmd.ExecuteNonQuery()
                            End Using
                        Next
                    End If

                    ' اطمینان از وجود ستون‌های سقف کاربر در جدول Users دیتابیس خام
                    Try
                        Using cmd As New System.Data.SQLite.SQLiteCommand("ALTER TABLE Users ADD COLUMN MaxCompaniesAllowed INTEGER DEFAULT 0;", conn)
                            cmd.ExecuteNonQuery()
                        End Using
                    Catch
                    End Try
                    Try
                        Using cmd As New System.Data.SQLite.SQLiteCommand("ALTER TABLE Users ADD COLUMN MaxFiscalYearsPerCompany INTEGER DEFAULT 0;", conn)
                            cmd.ExecuteNonQuery()
                        End Using
                    Catch
                    End Try

                    ' ۰. کپی فهرست مجوزهای سیستم به دیتابیس خام
                    Dim permsCatalog = Sql.ExecuteTable("SELECT PermissionID, PermissionName, PermissionKey FROM Permissions")
                    If permsCatalog IsNot Nothing Then
                        For Each pRow As DataRow In permsCatalog.Rows
                            Dim pId As Integer = Convert.ToInt32(pRow("PermissionID"))
                            Dim pName As String = Convert.ToString(pRow("PermissionName")).Replace("'", "''")
                            Dim pKey As String = Convert.ToString(pRow("PermissionKey")).Replace("'", "''")
                            Dim insCatSql As String = "INSERT INTO Permissions (PermissionID, PermissionName, PermissionKey) VALUES (" & pId & ", '" & pName & "', '" & pKey & "')"
                            Using cmd As New System.Data.SQLite.SQLiteCommand(insCatSql, conn)
                                cmd.ExecuteNonQuery()
                            End Using
                        Next
                    End If

                    ' ۱. ایجاد کاربر مخفی ابر مدیر
                    Dim adminPassHash As String = PasswordHasher.Hash("admin123")
                    Dim insertAdminSql As String = "INSERT INTO Users (UserID, Username, [Password], UserType, CreatedDate, IsActive, FullName) VALUES (1, 'admin', '" & adminPassHash & "', 'SuperAdmin', DATETIME('now'), 1, 'مدیر سیستم')"
                    Using cmd As New System.Data.SQLite.SQLiteCommand(insertAdminSql, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' ۲. ایجاد کاربر میانی مشخص‌شده
                    Dim managerId As Integer = Convert.ToInt32(managerRow("UserID"))
                    Dim newManagerId As Integer = If(managerId = 1, 2, managerId)
                    Dim mgrPassHash As String = PasswordHasher.Hash(managerPassword)
                    Dim mgrUsername As String = Convert.ToString(managerRow("Username")).Replace("'", "''")
                    Dim mgrFullName As String = Convert.ToString(managerRow("FullName")).Replace("'", "''")
                    Dim maxComp As Integer = If(managerRow.Table.Columns.Contains("MaxCompaniesAllowed") AndAlso Not managerRow.IsNull("MaxCompaniesAllowed"), Convert.ToInt32(managerRow("MaxCompaniesAllowed")), 0)
                    Dim maxFY As Integer = If(managerRow.Table.Columns.Contains("MaxFiscalYearsPerCompany") AndAlso Not managerRow.IsNull("MaxFiscalYearsPerCompany"), Convert.ToInt32(managerRow("MaxFiscalYearsPerCompany")), 0)

                    Dim insertMgrSql As String = "INSERT INTO Users (UserID, Username, [Password], UserType, CreatedDate, IsActive, FullName, MaxCompaniesAllowed, MaxFiscalYearsPerCompany) VALUES (" & newManagerId & ", '" & mgrUsername & "', '" & mgrPassHash & "', 'Manager', DATETIME('now'), 1, '" & mgrFullName & "', " & maxComp & ", " & maxFY & ")"
                    Using cmd As New System.Data.SQLite.SQLiteCommand(insertMgrSql, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' ۳. کپی مجوزهای کاربر میانی از دیتابیس فعلی به دیتابیس خام
                    Dim permsDt As DataTable = Sql.ExecuteTable("SELECT PermissionID, CanView, CanCreate, CanEdit, CanDelete, CanPrint, CanExport FROM RolePermissions WHERE UserID = ?", managerId)
                    If permsDt IsNot Nothing Then
                        For Each pRow As DataRow In permsDt.Rows
                            Dim pId As Integer = Convert.ToInt32(pRow("PermissionID"))
                            Dim cv As Integer = If(Not pRow.IsNull("CanView") AndAlso Convert.ToBoolean(pRow("CanView")), 1, 0)
                            Dim cc As Integer = If(Not pRow.IsNull("CanCreate") AndAlso Convert.ToBoolean(pRow("CanCreate")), 1, 0)
                            Dim ce As Integer = If(Not pRow.IsNull("CanEdit") AndAlso Convert.ToBoolean(pRow("CanEdit")), 1, 0)
                            Dim cd As Integer = If(Not pRow.IsNull("CanDelete") AndAlso Convert.ToBoolean(pRow("CanDelete")), 1, 0)
                            Dim cp As Integer = If(Not pRow.IsNull("CanPrint") AndAlso Convert.ToBoolean(pRow("CanPrint")), 1, 0)
                            Dim cx As Integer = If(Not pRow.IsNull("CanExport") AndAlso Convert.ToBoolean(pRow("CanExport")), 1, 0)

                            Dim insPermSql As String = "INSERT INTO RolePermissions (UserID, PermissionID, CanView, CanCreate, CanEdit, CanDelete, CanPrint, CanExport) VALUES (" & newManagerId & ", " & pId & ", " & cv & ", " & cc & ", " & ce & ", " & cd & ", " & cp & ", " & cx & ")"
                            Using cmd As New System.Data.SQLite.SQLiteCommand(insPermSql, conn)
                                cmd.ExecuteNonQuery()
                            End Using
                        Next
                    End If

                    ' ایجاد تنظیمات اولیه
                    Using cmd As New System.Data.SQLite.SQLiteCommand("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES ('Theme', 'Light', 'UI')", conn)
                        cmd.ExecuteNonQuery()
                    End Using
                    Using cmd As New System.Data.SQLite.SQLiteCommand("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES ('NumberFormat', 'N2', 'UI')", conn)
                        cmd.ExecuteNonQuery()
                    End Using
                    Using cmd As New System.Data.SQLite.SQLiteCommand("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES ('CurrencySymbol', 'ریال', 'UI')", conn)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New InvalidOperationException("خطا در ایجاد دیتابیس خام: " & ex.Message, ex)
            End Try
        End Sub

        Private Sub CopyDirectoryRecursive(sourceDir As String, targetDir As String)
            Dim dir As New DirectoryInfo(sourceDir)
            If Not dir.Exists Then Return

            If Not Directory.Exists(targetDir) Then
                Directory.CreateDirectory(targetDir)
            End If

            For Each fileInDir As FileInfo In dir.GetFiles()
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

                Dim temppath As String = Path.Combine(targetDir, fileInDir.Name)
                fileInDir.CopyTo(temppath, True)
            Next

            For Each subdir As DirectoryInfo In dir.GetDirectories()
                If String.Equals(subdir.Name, "Enteshar", StringComparison.OrdinalIgnoreCase) Then Continue For
                Dim temppath As String = Path.Combine(targetDir, subdir.Name)
                CopyDirectoryRecursive(subdir.FullName, temppath)
            Next
        End Sub

        Friend Sub DownloadPrerequisites(targetDir As String)
            Try
                ' فعال‌سازی پروتکل TLS 1.2 برای برقراری ارتباط امن با سرور مایکروسافت در دات‌نت‌های قدیمی
                System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType) Or System.Net.SecurityProtocolType.Tls

                Dim prereqDir As String = Path.Combine(targetDir, "Prerequisites")
                If Not Directory.Exists(prereqDir) Then
                    Directory.CreateDirectory(prereqDir)
                End If

                Using wClient As New System.Net.WebClient()
                    ' Visual C++ 2015-2022 Redistributable (x86)
                    Dim x86Path As String = Path.Combine(prereqDir, "vc_redist.x86.exe")
                    If Not File.Exists(x86Path) Then
                        wClient.DownloadFile("https://aka.ms/vs/17/release/vc_redist.x86.exe", x86Path)
                    End If

                    ' Visual C++ 2015-2022 Redistributable (x64)
                    Dim x64Path As String = Path.Combine(prereqDir, "vc_redist.x64.exe")
                    If Not File.Exists(x64Path) Then
                        wClient.DownloadFile("https://aka.ms/vs/17/release/vc_redist.x64.exe", x64Path)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("هشدار: امکان دانلود خودکار پیش‌نیازهای C++ وجود نداشت. لطفاً به اینترنت متصل شوید و مجدداً تلاش کنید." & Environment.NewLine & "در صورت عدم وجود اینترنت، می‌توانید فایل‌های vc_redist را دستی دانلود کرده و در پوشه Prerequisites قرار دهید." & Environment.NewLine & "خطا: " & ex.Message, "هشدار دانلود پیش‌نیازها", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub
    End Module

    Public Module MigrationService
        Public Sub ApplyPendingMigrations()
            Try
                Dim dataDir = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
                If String.IsNullOrWhiteSpace(dataDir) Then Return
                Dim dbFile = Path.Combine(dataDir, "Negar.db")
                If Not File.Exists(dbFile) Then Return

                Dim connStr As String = "Data Source=" & dbFile & ";Version=3;"
                Using conn As New System.Data.SQLite.SQLiteConnection(connStr)
                    conn.Open()

                    ' Check and rename old AccountingEntries table to Sanad1 if exists
                    Dim hasAccountingEntries As Boolean = False
                    Using cmdCheck1 As New System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='AccountingEntries';", conn)
                        Dim res = cmdCheck1.ExecuteScalar()
                        If res IsNot Nothing AndAlso Not Convert.IsDBNull(res) Then
                            hasAccountingEntries = True
                        End If
                    End Using
                    If hasAccountingEntries Then
                        Using cmdRename1 As New System.Data.SQLite.SQLiteCommand("ALTER TABLE AccountingEntries RENAME TO Sanad1;", conn)
                            cmdRename1.ExecuteNonQuery()
                        End Using
                    End If

                    ' Check and rename old AccountingEntryDetails table to Sanad2 if exists
                    Dim hasAccountingEntryDetails As Boolean = False
                    Using cmdCheck2 As New System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='AccountingEntryDetails';", conn)
                        Dim res = cmdCheck2.ExecuteScalar()
                        If res IsNot Nothing AndAlso Not Convert.IsDBNull(res) Then
                            hasAccountingEntryDetails = True
                        End If
                    End Using
                    If hasAccountingEntryDetails Then
                        Using cmdRename2 As New System.Data.SQLite.SQLiteCommand("ALTER TABLE AccountingEntryDetails RENAME TO Sanad2;", conn)
                            cmdRename2.ExecuteNonQuery()
                        End Using
                    End If

                                        ' Check and rename old ChartOfAccounts table to SarfaslHesab if still exists
                    Dim hasChartOfAccounts As Boolean = False
                    Using cmdCheck3 As New System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='ChartOfAccounts';", conn)
                        Dim res = cmdCheck3.ExecuteScalar()
                        If res IsNot Nothing AndAlso Not Convert.IsDBNull(res) Then
                            hasChartOfAccounts = True
                        End If
                    End Using
                    If hasChartOfAccounts Then
                        Using cmdRename3 As New System.Data.SQLite.SQLiteCommand("ALTER TABLE ChartOfAccounts RENAME TO SarfaslHesab;", conn)
                            cmdRename3.ExecuteNonQuery()
                        End Using
                    End If

                    ' Check and rename old shenavar table to SarfaslShenavar if still exists
                    Dim hasShenavar As Boolean = False
                    Using cmdCheck4 As New System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='shenavar';", conn)
                        Dim res = cmdCheck4.ExecuteScalar()
                        If res IsNot Nothing AndAlso Not Convert.IsDBNull(res) Then
                            hasShenavar = True
                        End If
                    End Using
                    If hasShenavar Then
                        Using cmdRename4 As New System.Data.SQLite.SQLiteCommand("ALTER TABLE shenavar RENAME TO SarfaslShenavar;", conn)
                            cmdRename4.ExecuteNonQuery()
                        End Using
                    End If

                    ' ۱. اطمینان از وجود جدول SchemaVersions
                    Using cmd As New System.Data.SQLite.SQLiteCommand("CREATE TABLE IF NOT EXISTS SchemaVersions (VersionID INTEGER PRIMARY KEY AUTOINCREMENT, ScriptName TEXT UNIQUE NOT NULL, AppliedDate DATETIME NOT NULL);", conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' ۲. خواندن اسکریپت‌های اعمال‌شده
                    Dim appliedScripts As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
                    Using cmd As New System.Data.SQLite.SQLiteCommand("SELECT ScriptName FROM SchemaVersions;", conn)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                appliedScripts.Add(reader.GetString(0))
                            End While
                        End Using
                    End Using

                    ' ۳. اسکن اسکریپت‌های موجود در پوشه Database\Migrations
                    Dim migrationsDir = Path.Combine(Application.StartupPath, "Database", "Migrations")
                    If Not Directory.Exists(migrationsDir) Then Return

                    Dim scriptFiles = Directory.GetFiles(migrationsDir, "*.sql")
                    Array.Sort(scriptFiles)

                    For Each scriptFile As String In scriptFiles
                        Dim scriptName = Path.GetFileName(scriptFile)
                        If Not appliedScripts.Contains(scriptName) Then
                            Dim sqlContent = File.ReadAllText(scriptFile)
                            Using trans = conn.BeginTransaction()
                                Try
                                    Dim statements = sqlContent.Split(New String() {";"}, StringSplitOptions.RemoveEmptyEntries)
                                    For Each stmt In statements
                                        Dim trimmedSql = stmt.Trim()
                                        If trimmedSql.Length > 0 Then
                                            Using cmd As New System.Data.SQLite.SQLiteCommand(trimmedSql, conn, trans)
                                                cmd.ExecuteNonQuery()
                                            End Using
                                        End If
                                    Next

                                    Using insCmd As New System.Data.SQLite.SQLiteCommand("INSERT INTO SchemaVersions (ScriptName, AppliedDate) VALUES (?, DATETIME('now'));", conn, trans)
                                        insCmd.Parameters.AddWithValue("ScriptName", scriptName)
                                        insCmd.ExecuteNonQuery()
                                    End Using

                                    trans.Commit()
                                Catch ex As Exception
                                    trans.Rollback()
                                    Throw New InvalidOperationException("خطا در اجرای اسکریپت به‌روزرسانی (" & scriptName & "): " & ex.Message, ex)
                                End Try
                            End Using
                        End If
                    Next
                End Using
            Catch ex As Exception
                Try
                    File.AppendAllText(Path.Combine(Application.StartupPath, "bootstrap.log"),
                                       DateTime.Now.ToString("yyyy-MM-dd") & " migration-error: " & ex.Message & Environment.NewLine & ex.StackTrace & Environment.NewLine)
                Catch
                End Try
            End Try
        End Sub
    End Module

    Public Module UpdateBuilderService
        Public Sub CreateUpdatePackage()
            Try
                Dim projectRoot As String = Application.StartupPath
                Dim entesharDir As String = Path.Combine(projectRoot, "Enteshar")
                If Not Directory.Exists(entesharDir) Then
                    Directory.CreateDirectory(entesharDir)
                End If

                Dim packageDir As String = Path.Combine(entesharDir, "Update_Negar")
                If Directory.Exists(packageDir) Then
                    Try
                        Directory.Delete(packageDir, True)
                    Catch
                    End Try
                End If
                Directory.CreateDirectory(packageDir)

                Dim updateFilesDir As String = Path.Combine(packageDir, "update_files")
                Directory.CreateDirectory(updateFilesDir)

                Dim sourceBinDir As String = Application.StartupPath
                CopyUpdateBinariesRecursive(sourceBinDir, updateFilesDir)

                ' رمزنگاری و خروجی گرفتن مستقیم دیتابیس فعال فعلی به پوشه آپدیت جهت ادغام تنظیمات عمومی
                Dim devDbTargetDir = Path.Combine(updateFilesDir, "Database")
                If Not Directory.Exists(devDbTargetDir) Then
                    Directory.CreateDirectory(devDbTargetDir)
                End If
                Dim runtimeDb = AesDbService.GetRuntimeDbFilePath()
                If File.Exists(runtimeDb) Then
                    AesDbService.EncryptFile(runtimeDb, Path.Combine(devDbTargetDir, "Negar.dat"))
                Else
                    Dim devDbSource = AesDbService.GetEncryptedFilePath()
                    If File.Exists(devDbSource) Then
                        File.Copy(devDbSource, Path.Combine(devDbTargetDir, "Negar.dat"), True)
                    End If
                End If

                Dim exeSource As String = Path.Combine(sourceBinDir, "Negar.exe")
                Dim updateExeTarget As String = Path.Combine(packageDir, "Update_Negar.exe")
                If File.Exists(exeSource) Then
                    File.Copy(exeSource, updateExeTarget, True)
                End If

                For Each dllFile As String In Directory.GetFiles(sourceBinDir, "*.dll")
                    File.Copy(dllFile, Path.Combine(packageDir, Path.GetFileName(dllFile)), True)
                Next
                For Each configFile As String In Directory.GetFiles(sourceBinDir, "*.config")
                    File.Copy(configFile, Path.Combine(packageDir, Path.GetFileName(configFile)), True)
                Next
                If Directory.Exists(Path.Combine(sourceBinDir, "x86")) Then
                    CopyUpdateBinariesRecursive(Path.Combine(sourceBinDir, "x86"), Path.Combine(packageDir, "x86"))
                End If
                If Directory.Exists(Path.Combine(sourceBinDir, "x64")) Then
                    CopyUpdateBinariesRecursive(Path.Combine(sourceBinDir, "x64"), Path.Combine(packageDir, "x64"))
                End If

                ' ایجاد پوشه پیش‌نیازها و دانلود خودکار VC++ Redistributable برای بسته به‌روزرسانی
                DownloadPrerequisites(packageDir)

                Dim readmePath As String = Path.Combine(packageDir, "راهنمای به‌روزرسانی.txt")
                Dim rlm As String = Char.ConvertFromUtf32(&H200F)
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine(rlm & "==================================================")
                sb.AppendLine(rlm & "         راهنمای به‌روزرسانی سیستم به نسخه جدید        ")
                sb.AppendLine(rlm & "==================================================")
                sb.AppendLine(rlm)
                sb.AppendLine(rlm & "دستورالعمل به‌روزرسانی:")
                sb.AppendLine(rlm & "جهت به‌روزرسانی سیستم مشتری، کافی است فایل Update_Negar.exe را در سیستم مقصد اجرا کنید.")
                sb.AppendLine(rlm & "۱. قبل از شروع کپی، سیستم به طور خودکار یک بک‌آپ کامل از دیتابیس فعلی مشتری تهیه خواهد کرد.")
                sb.AppendLine(rlm & "۲. فایل‌های جدید جایگزین شده و تمامی اسکریپت‌های جدید ارتقای ساختار دیتابیس اعمال می‌شوند.")
                sb.AppendLine(rlm & "۳. اطلاعات قبلی مشتری ۱۰۰٪ محفوظ می‌ماند.")
                sb.AppendLine(rlm)
                sb.AppendLine(rlm & "نکته بسیار مهم:")
                sb.AppendLine(rlm & "در صورتی که نرم‌افزار پس از به‌روزرسانی با خطای SQLite مواجه شد،")
                sb.AppendLine(rlm & "لطفاً پیش‌نیازهای موجود در پوشه Prerequisites را روی سیستم مقصد نصب نمایید.")
                File.WriteAllText(readmePath, sb.ToString(), System.Text.Encoding.UTF8)

                MessageBox.Show("بسته به‌روزرسانی (Update) با موفقیت تولید شد!" & Environment.NewLine & Environment.NewLine & "فایل‌های پیش‌نیاز (Visual C++) نیز در پوشه Prerequisites دانلود گردید." & Environment.NewLine & Environment.NewLine & "مسیر بسته خروجی:" & Environment.NewLine & packageDir, "موفقیت در ایجاد بسته به‌روزرسانی", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Process.Start("explorer.exe", entesharDir)
            Catch ex As Exception
                MessageBox.Show("خطا در ایجاد بسته به‌روزرسانی: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub CopyUpdateBinariesRecursive(sourceDir As String, targetDir As String)
            Dim dir As New DirectoryInfo(sourceDir)
            If Not dir.Exists Then Return

            If Not Directory.Exists(targetDir) Then
                Directory.CreateDirectory(targetDir)
            End If

            For Each fileInDir As FileInfo In dir.GetFiles()
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

                Dim temppath As String = Path.Combine(targetDir, fileInDir.Name)
                fileInDir.CopyTo(temppath, True)
            Next

            For Each subdir As DirectoryInfo In dir.GetDirectories()
                If String.Equals(subdir.Name, "Enteshar", StringComparison.OrdinalIgnoreCase) Then Continue For
                If String.Equals(subdir.Name, "Backups", StringComparison.OrdinalIgnoreCase) Then Continue For

                Dim temppath As String = Path.Combine(targetDir, subdir.Name)
                CopyUpdateBinariesRecursive(subdir.FullName, temppath)
            Next
        End Sub
    End Module
End Namespace
