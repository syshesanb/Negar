Option Strict Off
Option Explicit On

Imports System
Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Data
    Public Module DbEncryptionService
        Private Const MasterPassword As String = "SysHesAnb_Secure_Db_Key_2026_@Sec!"

        Public Function GetMasterPassword() As String
            Return MasterPassword
        End Function

        Public Function GetEncryptedConnectionString(dbFilePath As String) As String
            Return "Data Source=" & dbFilePath & ";Version=3;Password=" & MasterPassword & ";"
        End Function

        ' اطمینان از اینکه دیتابیس فعلی با رمز عبور سرور قفل شده است
        Public Sub EnsureDatabaseEncrypted(dbFilePath As String)
            If Not File.Exists(dbFilePath) Then Return

            Try
                ' امتحان ورود با رمز عبور
                Dim connStr As String = GetEncryptedConnectionString(dbFilePath)
                Using conn As New SQLiteConnection(connStr)
                    conn.Open()
                    ' دیتابیس قبلاً رمزنگاری شده است
                End Using
            Catch ex As Exception
                ' اگر با رمز باز نشد، یعنی بدون رمز بوده؛ بنابراین رمز روی آن ست می‌شود
                Try
                    Dim unencConnStr As String = "Data Source=" & dbFilePath & ";Version=3;"
                    Using conn As New SQLiteConnection(unencConnStr)
                        conn.Open()
                        conn.SetPassword(MasterPassword)
                    End Using
                Catch
                End Try
            End Try
        End Sub

        ' خروجی دیتابیس بدون رمز برای بازرسی برنامه‌نویس و ابر مدیر
        Public Sub ExportDecryptedCopy(targetFilePath As String)
            Dim dataDir As String = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
            If String.IsNullOrWhiteSpace(dataDir) Then
                dataDir = Path.Combine(Application.StartupPath, "Database")
            End If
            Dim currentDbFile As String = Path.Combine(dataDir, "Sys_Hes_Anb.db")

            If Not File.Exists(currentDbFile) Then
                Throw New FileNotFoundException("فایل دیتابیس سیستم یافت نشد.", currentDbFile)
            End If

            Try
                Sql.ExecuteNonQuery("PRAGMA wal_checkpoint(FULL);")
            Catch
            End Try

            ' کپی دیتابیس جاری به مسیر خروجی
            File.Copy(currentDbFile, targetFilePath, True)

            ' برداشتن کلمه عبور روی فایل خروجی تا کاملاً بدون رمز شود
            Try
                Dim connStr As String = GetEncryptedConnectionString(targetFilePath)
                Using conn As New SQLiteConnection(connStr)
                    conn.Open()
                    conn.ChangePassword("")
                End Using
            Catch ex As Exception
                Throw New InvalidOperationException("خطا در جداسازی رمز عبور فایل خروجی: " & ex.Message, ex)
            End Try
        End Sub
    End Module
End Namespace
