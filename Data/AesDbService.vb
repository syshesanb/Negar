Option Strict Off
Option Explicit On

Imports System
Imports System.Data.SQLite
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Namespace Negar.Data
    Public Module AesDbService
        Private ReadOnly AesKey As Byte() = Encoding.UTF8.GetBytes("SysHesAnb_Aes256_MasterKey_2026!") ' 32 bytes
        Private ReadOnly AesIV As Byte() = Encoding.UTF8.GetBytes("SysHesAnb_IV2026")               ' 16 bytes

        Public Function GetDataDirectory() As String
            Dim dataDir As String = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
            If String.IsNullOrWhiteSpace(dataDir) Then
                dataDir = Path.Combine(Application.StartupPath, "Database")
            End If
            If Not Directory.Exists(dataDir) Then
                Directory.CreateDirectory(dataDir)
            End If
            Return dataDir
        End Function

        Public Function GetEncryptedFilePath() As String
            Return Path.Combine(GetDataDirectory(), "Negar.dat")
        End Function

        Public Function GetRuntimeDbFilePath() As String
            Return Path.Combine(GetDataDirectory(), "Negar.db")
        End Function

        Public Sub EncryptFile(inputFile As String, outputFile As String)
            If Not File.Exists(inputFile) Then Return
            Dim bytes() As Byte = File.ReadAllBytes(inputFile)
            Using aesAlg As Aes = Aes.Create()
                aesAlg.Key = AesKey
                aesAlg.IV = AesIV
                Using encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                    Using ms As New MemoryStream()
                        Using cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
                            cs.Write(bytes, 0, bytes.Length)
                            cs.FlushFinalBlock()
                        End Using
                        File.WriteAllBytes(outputFile, ms.ToArray())
                    End Using
                End Using
            End Using
        End Sub

        Public Sub DecryptFile(inputFile As String, outputFile As String)
            If Not File.Exists(inputFile) Then Return
            Dim cipherBytes() As Byte = File.ReadAllBytes(inputFile)
            Using aesAlg As Aes = Aes.Create()
                aesAlg.Key = AesKey
                aesAlg.IV = AesIV
                Using decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                    Using ms As New MemoryStream()
                        Using cs As New CryptoStream(ms, decryptor, CryptoStreamMode.Write)
                            cs.Write(cipherBytes, 0, cipherBytes.Length)
                            cs.FlushFinalBlock()
                        End Using
                        File.WriteAllBytes(outputFile, ms.ToArray())
                    End Using
                End Using
            End Using
        End Sub

        Public Sub PrepareRuntimeDatabase()
            Dim encFile = GetEncryptedFilePath()
            Dim dbFile = GetRuntimeDbFilePath()

            If File.Exists(dbFile) AndAlso Not File.Exists(encFile) Then
                EncryptFile(dbFile, encFile)
                Try
                    File.Delete(dbFile)
                Catch
                End Try
            End If

            If File.Exists(encFile) Then
                Try
                    Dim shouldDecrypt = True
                    If File.Exists(dbFile) Then
                        Dim dbTime = File.GetLastWriteTime(dbFile)
                        Dim encTime = File.GetLastWriteTime(encFile)
                        If dbTime > encTime Then
                            shouldDecrypt = False
                        End If
                    End If

                    If shouldDecrypt Then
                        DecryptFile(encFile, dbFile)
                    End If
                Catch ex As Exception
                End Try
            End If
        End Sub

        Public Sub SyncAndLockDatabase()
            Dim dbFile = GetRuntimeDbFilePath()
            Dim encFile = GetEncryptedFilePath()

            If Not File.Exists(dbFile) Then Return

            Try
                Sql.ExecuteNonQuery("PRAGMA wal_checkpoint(FULL);")
            Catch
            End Try

            Try
                SQLiteConnection.ClearAllPools()
                GC.Collect()
                GC.WaitForPendingFinalizers()
            Catch
            End Try

            Try
                EncryptFile(dbFile, encFile)
                If File.Exists(encFile) AndAlso New FileInfo(encFile).Length > 0 Then
                    If File.Exists(dbFile) Then File.Delete(dbFile)
                    If File.Exists(dbFile & "-wal") Then File.Delete(dbFile & "-wal")
                    If File.Exists(dbFile & "-shm") Then File.Delete(dbFile & "-shm")
                End If
            Catch
            End Try
        End Sub

        Public Sub ExportDecryptedDatabase(srcFilePath As String, targetFilePath As String)
            If Not File.Exists(srcFilePath) Then
                Throw New FileNotFoundException("فایل مبدا انتخاب‌شده یافت نشد.", srcFilePath)
            End If

            Try
                DecryptFile(srcFilePath, targetFilePath)
            Catch ex As Exception
                File.Copy(srcFilePath, targetFilePath, True)
            End Try
        End Sub
    End Module
End Namespace
