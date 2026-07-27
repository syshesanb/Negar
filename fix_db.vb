Imports System
Imports System.Data.SQLite

Module FixDB
    Sub Main()
        Dim dbPath = "C:\Negar\bin\Debug\Data\Hesabdary.db"
        Dim connStr = "Data Source=" & dbPath & ";Version=3;"
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Using cmd As New SQLiteCommand("DELETE FROM FiscalYears WHERE StartDate LIKE '%/%' AND length(StartDate) = 10", conn)
                Dim rows = cmd.ExecuteNonQuery()
                Console.WriteLine("Deleted " & rows & " bad fiscal years.")
            End Using
        End Using
    End Sub
End Module
