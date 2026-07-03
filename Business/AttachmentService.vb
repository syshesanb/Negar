Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business

    Public Class AttachmentService

        Private Const JpegQuality As Long = 82

        Public Function GetLinesWithAttachments(entryId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT DISTINCT LineNumber FROM EntryLineAttachments " &
                "WHERE EntryID = ? ORDER BY LineNumber",
                entryId)
        End Function

        Public Function GetAttachments(entryId As Integer, lineNumber As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT AttachmentID, FileName, FileSize, AddedDate FROM EntryLineAttachments " &
                "WHERE EntryID = ? AND LineNumber = ? ORDER BY AddedDate",
                entryId, lineNumber)
        End Function

        Public Sub SaveAttachment(entryId As Integer, lineNumber As Integer,
                                  fileName As String, rawBytes As Byte())
            Dim compressed = CompressToJpeg(rawBytes, JpegQuality)
            Sql.ExecuteNonQuery(
                "INSERT INTO EntryLineAttachments (EntryID, LineNumber, FileName, ImageData, FileSize, AddedDate) " &
                "VALUES (?, ?, ?, ?, ?, ?)",
                entryId, lineNumber,
                Path.GetFileNameWithoutExtension(fileName) & ".jpg",
                compressed, compressed.Length, Date.Now)
        End Sub

        Public Sub DeleteAttachment(attachmentId As Integer)
            Sql.ExecuteNonQuery(
                "DELETE FROM EntryLineAttachments WHERE AttachmentID = ?", attachmentId)
        End Sub

        Public Function GetImageData(attachmentId As Integer) As Byte()
            Dim dt = Sql.ExecuteTable(
                "SELECT ImageData FROM EntryLineAttachments WHERE AttachmentID = ?", attachmentId)
            If dt.Rows.Count = 0 Then Return Nothing
            Dim val = dt.Rows(0)("ImageData")
            If val Is Nothing OrElse val Is DBNull.Value Then Return Nothing
            Return CType(val, Byte())
        End Function

        Public Function BytesToImage(data As Byte()) As Image
            If data Is Nothing OrElse data.Length = 0 Then Return Nothing
            Try
                Using ms As New MemoryStream(data)
                    Return Image.FromStream(ms)
                End Using
            Catch
                Return Nothing
            End Try
        End Function

        Private Function CompressToJpeg(imageData As Byte(), quality As Long) As Byte()
            Try
                Using ms As New MemoryStream(imageData)
                    Using img As Image = Image.FromStream(ms)
                        Dim codec = GetJpegCodec()
                        If codec Is Nothing Then Return imageData
                        Dim encoderParams As New EncoderParameters(1)
                        encoderParams.Param(0) = New EncoderParameter(Encoder.Quality, quality)
                        Using outMs As New MemoryStream()
                            img.Save(outMs, codec, encoderParams)
                            Return outMs.ToArray()
                        End Using
                    End Using
                End Using
            Catch
                Return imageData
            End Try
        End Function

        Private Shared Function GetJpegCodec() As ImageCodecInfo
            For Each codec In ImageCodecInfo.GetImageEncoders()
                If codec.MimeType = "image/jpeg" Then Return codec
            Next
            Return Nothing
        End Function

    End Class

End Namespace
