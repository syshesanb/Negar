Imports System
Imports System.Data.SQLite

Module Program
    Sub Main()
        Dim dbPaths = New String() {
            "C:\Sys_Hes_Anb\Database\Sys_Hes_Anb.db",
            "C:\Sys_Hes_Anb\bin\Debug\Database\Sys_Hes_Anb.db",
            "C:\Sys_Hes_Anb\bin\Debug\Sys_Hes_Anb.db"
        }
        
        For Each dbPath In dbPaths
            If System.IO.File.Exists(dbPath) Then
                Try
                    Dim connStr As String = "Data Source=" & dbPath & ";Version=3;"
                    Using conn As New SQLiteConnection(connStr)
                        conn.Open()
                        Using cmd As New SQLiteCommand("DELETE FROM AppSettings WHERE SettingKey IN ('AboutText', 'ContactText')", conn)
                            Dim r = cmd.ExecuteNonQuery()
                            Console.WriteLine("Deleted " & r & " rows in " & dbPath)
                        End Using
                    End Using
                Catch ex As Exception
                    Console.WriteLine("Error with " & dbPath & ": " & ex.Message)
                End Try
            End If
        Next
    End Sub
End Module
