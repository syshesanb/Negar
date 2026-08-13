Option Strict Off
Option Explicit On

Imports System
Imports System.IO
Imports System.Windows.Forms

Namespace Negar.Business
    Public Class IradLogger
        Private Shared ReadOnly LogFilePath As String = "C:\Negar\irad.txt"
        Private Shared ReadOnly LockObj As New Object()

        Public Shared Sub Clear()
            Try
                SyncLock LockObj
                    File.WriteAllText(LogFilePath, $"========== IRAD LOG STARTED AT {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==========" & Environment.NewLine)
                End SyncLock
            Catch
            End Try
        End Sub

        Public Shared Sub Log(category As String, message As String)
            Try
                Dim logLine As String = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{category}] {message}"
                SyncLock LockObj
                    File.AppendAllText(LogFilePath, logLine & Environment.NewLine)
                End SyncLock
            Catch
            End Try
        End Sub
    End Class
End Namespace
