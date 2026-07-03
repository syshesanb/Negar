Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Module BackupRestoreService

        ' =========================================================
        ' 1. جدول و کلاس محاسباتی CRC32 و PKZIP 2.0 ZipCrypto (Overflow-Safe)
        ' =========================================================
        Private Class Crc32Table
            Private Shared ReadOnly Table(255) As UInt32

            Shared Sub New()
                For i As Integer = 0 To 255
                    Dim entry As UInt32 = CUInt(i)
                    For j As Integer = 0 To 7
                        If (entry And 1UI) <> 0UI Then
                            entry = (entry >> 1) Xor &HEDB88320UI
                        Else
                            entry = entry >> 1
                        End If
                    Next
                    Table(i) = entry
                Next
            End Sub

            Public Shared Function ComputeByte(currentCrc As UInt32, b As Byte) As UInt32
                Dim idx As Integer = CInt((currentCrc Xor CUInt(b)) And &HFFUI)
                Return Table(idx) Xor (currentCrc >> 8)
            End Function

            Public Shared Function ComputeBuffer(buffer() As Byte, offset As Integer, length As Integer) As UInt32
                Dim crc As UInt32 = &HFFFFFFFFUI
                For i As Integer = offset To offset + length - 1
                    Dim idx As Integer = CInt((crc Xor CUInt(buffer(i))) And &HFFUI)
                    crc = Table(idx) Xor (crc >> 8)
                Next
                Return crc Xor &HFFFFFFFFUI
            End Function
        End Class

        Private Class ZipCrypto
            Private _keys(2) As UInt32

            Public Sub Init(password As String)
                _keys(0) = &H12345678UI
                _keys(1) = &H23456789UI
                _keys(2) = &H34567890UI
                Dim bytes() As Byte = Encoding.UTF8.GetBytes(password)
                For Each b As Byte In bytes
                    UpdateKeys(b)
                Next
            End Sub

            Private Sub UpdateKeys(b As Byte)
                _keys(0) = Crc32Table.ComputeByte(_keys(0), b)
                
                Dim k1 As Long = CLng(_keys(1))
                Dim k0 As Long = CLng(_keys(0) And &HFFUI)
                Dim term As Long = (k1 + k0) * 134775813L + 1L
                Dim bytes32() As Byte = BitConverter.GetBytes(term)
                _keys(1) = BitConverter.ToUInt32(bytes32, 0)

                Dim shiftByte As Byte = CByte((_keys(1) >> 24) And &HFFUI)
                _keys(2) = Crc32Table.ComputeByte(_keys(2), shiftByte)
            End Sub

            Public Function DecryptByte(cipherByte As Byte) As Byte
                Dim temp As UInt32 = (_keys(2) Or 2UI) And &HFFFFUI
                Dim prod As UInt32 = (temp * (temp Xor 1UI)) And &HFFFFUI
                Dim keyByte As Byte = CByte((prod >> 8) And &HFFUI)
                Dim plainByte As Byte = cipherByte Xor keyByte
                UpdateKeys(plainByte)
                Return plainByte
            End Function

            Public Function EncryptByte(plainByte As Byte) As Byte
                Dim temp As UInt32 = (_keys(2) Or 2UI) And &HFFFFUI
                Dim prod As UInt32 = (temp * (temp Xor 1UI)) And &HFFFFUI
                Dim keyByte As Byte = CByte((prod >> 8) And &HFFUI)
                UpdateKeys(plainByte)
                Return plainByte Xor keyByte
            End Function
        End Class

        ' =========================================================
        ' 2. توابع تولید نام فایل و کلمه عبور بر اساس تاریخ شمسی
        ' =========================================================
        Public Function GenerateBackupFileName() As String
            Dim dt As DateTime = DateTime.Now
            Dim pc As New PersianCalendar()
            Dim y As Integer = pc.GetYear(dt)
            Dim m As Integer = pc.GetMonth(dt)
            Dim d As Integer = pc.GetDayOfMonth(dt)
            Dim dateStr As String = String.Format("{0:0000}{1:00}{2:00}", y, m, d)
            Dim timeStr As String = dt.ToString("HHmmss")
            Return "SysHesAnb" & dateStr & timeStr & ".zip"
        End Function

        Public Function GeneratePasswordForDate(dt As DateTime) As String
            Dim pc As New PersianCalendar()
            Dim y As Integer = pc.GetYear(dt)
            Dim m As Integer = pc.GetMonth(dt)
            Dim d As Integer = pc.GetDayOfMonth(dt)
            Dim dateCompact As String = String.Format("{0:0000}{1:00}{2:00}", y, m, d)
            Return "hn45825hn" & dateCompact
        End Function

        Public Function DerivePasswordFromFileName(filePath As String) As String
            Dim fileName As String = Path.GetFileName(filePath)
            Dim matchNew As Match = Regex.Match(fileName, "(\d{8})")
            If matchNew.Success Then
                Return "hn45825hn" & matchNew.Groups(1).Value
            End If

            Dim matchOld As Match = Regex.Match(fileName, "(\d{4})_(\d{2})_(\d{2})")
            If matchOld.Success Then
                Dim y As String = matchOld.Groups(1).Value
                Dim m As String = matchOld.Groups(2).Value
                Dim d As String = matchOld.Groups(3).Value
                Return "hn45825hn" & y & m & d
            End If

            Dim fi As New FileInfo(filePath)
            Return GeneratePasswordForDate(fi.LastWriteTime)
        End Function

        ' =========================================================
        ' 3. عملیات پشتیبان‌گیری (Create Backup)
        ' =========================================================
        Public Sub CreateBackup(targetZipPath As String)
            Dim dataDir As String = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
            If String.IsNullOrWhiteSpace(dataDir) Then
                dataDir = Path.Combine(Application.StartupPath, "Database")
            End If
            Dim dbFile As String = Path.Combine(dataDir, "Sys_Hes_Anb.db")

            If Not File.Exists(dbFile) Then
                Throw New FileNotFoundException("فایل دیتابیس سیستم یافت نشد.", dbFile)
            End If

            ' ولش کردن تراکنش‌های SQLite
            Try
                Sql.ExecuteNonQuery("PRAGMA wal_checkpoint(FULL);")
            Catch
            End Try

            Dim tempCopyDb As String = Path.Combine(Path.GetTempPath(), "backup_temp_" & Guid.NewGuid().ToString("N") & ".db")
            File.Copy(dbFile, tempCopyDb, True)

            Try
                Dim password As String = GeneratePasswordForDate(DateTime.Now)
                WriteEncryptedZip(tempCopyDb, "Sys_Hes_Anb.db", targetZipPath, password)
            Finally
                If File.Exists(tempCopyDb) Then
                    Try
                        File.Delete(tempCopyDb)
                    Catch
                    End Try
                End If
            End Try
        End Sub

        ' =========================================================
        ' 4. عملیات بازیابی (Restore Backup)
        ' =========================================================
        Public Sub RestoreBackup(sourceZipPath As String)
            If Not File.Exists(sourceZipPath) Then
                Throw New FileNotFoundException("فایل پشتیبان انتخاب شده یافت نشد.", sourceZipPath)
            End If

            Dim password As String = DerivePasswordFromFileName(sourceZipPath)
            Dim tempExtractedDb As String = Path.Combine(Path.GetTempPath(), "restore_temp_" & Guid.NewGuid().ToString("N") & ".db")

            Try
                ReadEncryptedZip(sourceZipPath, password, tempExtractedDb)

                ' تست سلامت دیتابیس استخراج شده
                Dim connStr As String = "Data Source=" & tempExtractedDb & ";Version=3;"
                Using conn As New System.Data.SQLite.SQLiteConnection(connStr)
                    conn.Open()
                    Using cmd As New System.Data.SQLite.SQLiteCommand("PRAGMA quick_check;", conn)
                        Dim res As String = Convert.ToString(cmd.ExecuteScalar())
                        If Not String.Equals(res, "ok", StringComparison.OrdinalIgnoreCase) Then
                            Throw New InvalidOperationException("فایل پشتیبان آسیب دیده است یا کلمه عبور معتبر نمی‌باشد.")
                        End If
                    End Using
                End Using

                ' پاک‌سازی Connection Pool برای جایگزینی فایل
                System.Data.SQLite.SQLiteConnection.ClearAllPools()
                GC.Collect()
                GC.WaitForPendingFinalizers()

                Dim dataDir As String = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
                If String.IsNullOrWhiteSpace(dataDir) Then
                    dataDir = Path.Combine(Application.StartupPath, "Database")
                End If
                Dim currentDbFile As String = Path.Combine(dataDir, "Sys_Hes_Anb.db")

                ' ایجاد پشتیبان اضطراری از دیتابیس قبل از جایگزینی
                Dim emergencyBackup As String = currentDbFile & ".emg"
                If File.Exists(currentDbFile) Then
                    File.Copy(currentDbFile, emergencyBackup, True)
                End If

                Try
                    File.Copy(tempExtractedDb, currentDbFile, True)
                    If File.Exists(emergencyBackup) Then
                        File.Delete(emergencyBackup)
                    End If
                Catch ex As Exception
                    If File.Exists(emergencyBackup) Then
                        File.Copy(emergencyBackup, currentDbFile, True)
                    End If
                    Throw
                End Try
            Finally
                If File.Exists(tempExtractedDb) Then
                    Try
                        File.Delete(tempExtractedDb)
                    Catch
                    End Try
                End If
            End Try
        End Sub

        ' =========================================================
        ' 5. موتور ساخت و استخراج ZIP رمزنگاری شده (ZipCrypto Encrypted ZIP)
        ' =========================================================
        Private Sub WriteEncryptedZip(inputFile As String, entryName As String, outputZip As String, password As String)
            Dim inputBytes() As Byte = File.ReadAllBytes(inputFile)
            Dim crc As UInt32 = Crc32Table.ComputeBuffer(inputBytes, 0, inputBytes.Length)

            ' فشرده‌سازی دیتا با DeflateStream
            Dim compressedMs As New MemoryStream()
            Using ds As New DeflateStream(compressedMs, CompressionMode.Compress, True)
                ds.Write(inputBytes, 0, inputBytes.Length)
            End Using
            Dim compData() As Byte = compressedMs.ToArray()

            ' رمزنگاری دیتا با ZipCrypto
            Dim crypto As New ZipCrypto()
            crypto.Init(password)

            ' ساخت 12 بایت هدر رمزنگاری (11 بایت تصادفی + 1 بایت MSB از CRC)
            Dim header(11) As Byte
            Dim rnd As New Random()
            rnd.NextBytes(header)
            header(11) = CByte((crc >> 24) And &HFFUI)

            Dim encryptedHeader(11) As Byte
            For i As Integer = 0 To 11
                encryptedHeader(i) = crypto.EncryptByte(header(i))
            Next

            Dim encryptedCompData(compData.Length - 1) As Byte
            For i As Integer = 0 To compData.Length - 1
                encryptedCompData(i) = crypto.EncryptByte(compData(i))
            Next

            Dim encDataLen As Integer = encryptedHeader.Length + encryptedCompData.Length
            Dim entryNameBytes() As Byte = Encoding.UTF8.GetBytes(entryName)

            Using fs As New FileStream(outputZip, FileMode.Create, FileAccess.Write)
                Using bw As New BinaryWriter(fs)
                    ' Local File Header Signature = 0x04034b50
                    bw.Write(&H4034B50UI)
                    bw.Write(CUShort(20)) ' Version needed to extract
                    bw.Write(CUShort(1))  ' General purpose bit flag (Bit 0 = Encrypted)
                    bw.Write(CUShort(8))  ' Compression method (8 = Deflated)
                    bw.Write(CUShort(0))  ' File last mod time
                    bw.Write(CUShort(0))  ' File last mod date
                    bw.Write(crc)
                    bw.Write(CUInt(encDataLen))
                    bw.Write(CUInt(inputBytes.Length))
                    bw.Write(CUShort(entryNameBytes.Length))
                    bw.Write(CUShort(0))  ' Extra field length
                    bw.Write(entryNameBytes)
                    bw.Write(encryptedHeader)
                    bw.Write(encryptedCompData)

                    Dim localHeaderOffset As Integer = 0
                    Dim cdOffset As Integer = CInt(fs.Position)

                    ' Central Directory Header Signature = 0x02014b50
                    bw.Write(&H2014B50UI)
                    bw.Write(CUShort(20)) ' Version made by
                    bw.Write(CUShort(20)) ' Version needed to extract
                    bw.Write(CUShort(1))  ' General purpose bit flag
                    bw.Write(CUShort(8))  ' Compression method
                    bw.Write(CUShort(0))  ' File last mod time
                    bw.Write(CUShort(0))  ' File last mod date
                    bw.Write(crc)
                    bw.Write(CUInt(encDataLen))
                    bw.Write(CUInt(inputBytes.Length))
                    bw.Write(CUShort(entryNameBytes.Length))
                    bw.Write(CUShort(0))  ' Extra field length
                    bw.Write(CUShort(0))  ' File comment length
                    bw.Write(CUShort(0))  ' Disk number start
                    bw.Write(CUShort(0))  ' Internal file attributes
                    bw.Write(CUInt(0))    ' External file attributes
                    bw.Write(CUInt(localHeaderOffset))
                    bw.Write(entryNameBytes)

                    Dim cdSize As Integer = CInt(fs.Position) - cdOffset

                    ' End of Central Directory Record Signature = 0x06054b50
                    bw.Write(&H6054B50UI)
                    bw.Write(CUShort(0))  ' Number of this disk
                    bw.Write(CUShort(0))  ' Disk where CD starts
                    bw.Write(CUShort(1))  ' Number of CD records on this disk
                    bw.Write(CUShort(1))  ' Total number of CD records
                    bw.Write(CUInt(cdSize))
                    bw.Write(CUInt(cdOffset))
                    bw.Write(CUShort(0))  ' Comment length
                End Using
            End Using
        End Sub

        Private Sub ReadEncryptedZip(zipFile As String, password As String, outputFile As String)
            Using fs As New FileStream(zipFile, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fs)
                    Dim sig As UInt32 = br.ReadUInt32()
                    If sig <> &H4034B50UI Then
                        Throw New InvalidDataException("فایل خوانی شده یک فایل ZIP معتبر نمی‌باشد.")
                    End If

                    Dim verNeeded As UShort = br.ReadUInt16()
                    Dim bitFlag As UShort = br.ReadUInt16()
                    Dim compMethod As UShort = br.ReadUInt16()
                    Dim modTime As UShort = br.ReadUInt16()
                    Dim modDate As UShort = br.ReadUInt16()
                    Dim crc As UInt32 = br.ReadUInt32()
                    Dim compSize As UInt32 = br.ReadUInt32()
                    Dim uncompSize As UInt32 = br.ReadUInt32()
                    Dim nameLen As UShort = br.ReadUInt16()
                    Dim extraLen As UShort = br.ReadUInt16()

                    br.ReadBytes(nameLen)
                    If extraLen > 0 Then br.ReadBytes(extraLen)

                    Dim isEncrypted As Boolean = (bitFlag And 1) <> 0
                    If Not isEncrypted Then
                        Throw New InvalidDataException("فایل پشتیبان رمزنگاری شده نیست.")
                    End If

                    Dim crypto As New ZipCrypto()
                    crypto.Init(password)

                    ' رمزگشایی 12 بایت هدر
                    Dim encHeader() As Byte = br.ReadBytes(12)
                    Dim decHeader(11) As Byte
                    For i As Integer = 0 To 11
                        decHeader(i) = crypto.DecryptByte(encHeader(i))
                    Next

                    ' بررسی صحت رمز عبور با بایت ۱۱ هدر
                    If decHeader(11) <> CByte((crc >> 24) And &HFFUI) Then
                        Throw New UnauthorizedAccessException("کلمه عبور کلید پشتیبان معتبر نمی‌باشد.")
                    End If

                    Dim encDataLen As Integer = CInt(compSize) - 12
                    Dim encData() As Byte = br.ReadBytes(encDataLen)
                    Dim decCompData(encDataLen - 1) As Byte
                    For i As Integer = 0 To encDataLen - 1
                        decCompData(i) = crypto.DecryptByte(encData(i))
                    Next

                    ' فشرده‌زدایی دیتا
                    Using ms As New MemoryStream(decCompData)
                        Using ds As New DeflateStream(ms, CompressionMode.Decompress)
                            Using outFs As New FileStream(outputFile, FileMode.Create, FileAccess.Write)
                                ds.CopyTo(outFs)
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Sub
    End Module
End Namespace
