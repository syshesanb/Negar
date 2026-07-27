Imports System
Imports System.Data.SQLite

Module Program
    Sub Main()
        Dim dbPaths = New String() {
            "C:\Negar\Database\Negar.db",
            "C:\Negar\bin\Debug\Database\Negar.db",
            "C:\Negar\bin\Debug\Negar.db"
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
