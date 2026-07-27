Imports System
Imports System.Data.SQLite

Module Program
    Sub Main()
        Try
            Dim connStr = "Data Source=C:\Negar\Database\Negar.db;Version=3;"
            Using conn As New SQLiteConnection(connStr)
                conn.Open()
                Using cmd As New SQLiteCommand("ALTER TABLE Companies ADD COLUMN Level6Length INTEGER DEFAULT 2", conn)
                    cmd.ExecuteNonQuery()
                End Using
                Console.WriteLine("Column Level6Length added successfully.")
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
