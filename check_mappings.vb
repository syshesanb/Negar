Imports System.Data.SQLite
Imports System.IO

Module CheckMappings
    Sub Main()
        Try
            Dim dbPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database.sqlite") ' Let's find the correct path
            If Not File.Exists(dbPath) Then
                dbPath = "c:\Sys_Hes_Anb\bin\Debug\Database.sqlite"
            End If
            If Not File.Exists(dbPath) Then
                dbPath = "c:\Sys_Hes_Anb\Hesabdary.db"
            End If
            
            Dim connStr = "Data Source=" & dbPath & ";Version=3;"
            Using conn As New SQLiteConnection(connStr)
                conn.Open()
                Using cmd As New SQLiteCommand("SELECT COUNT(*) FROM BalanceSheetAccountMappings", conn)
                    Dim bsCount = Convert.ToInt32(cmd.ExecuteScalar())
                    Console.WriteLine("BS Mappings Count: " & bsCount)
                End Using
                Using cmd As New SQLiteCommand("SELECT COUNT(*) FROM PnLAccountMappings", conn)
                    Dim pnlCount = Convert.ToInt32(cmd.ExecuteScalar())
                    Console.WriteLine("PnL Mappings Count: " & pnlCount)
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
