Imports System
Imports System.Data
Imports System.Data.SQLite
Imports System.Collections.Generic

Module TestSQLiteParams
    Sub Main()
        Dim dbPath As String = "c:\Negar\bin\Debug\Database.sqlite"
        If Not System.IO.File.Exists(dbPath) Then
            dbPath = "c:\Negar\Hesabdary.db"
        End If

        Dim connStr = "Data Source=" & dbPath & ";Version=3;"
        Try
            Using conn As New SQLiteConnection(connStr)
                conn.Open()
                ' Just test the exact command structure
                Using cmdInsert As New SQLiteCommand("SELECT @cid AS cid, @key AS key, @accid AS accid", conn)
                    cmdInsert.Parameters.Add("@cid", DbType.Int32)
                    cmdInsert.Parameters.Add("@key", DbType.String)
                    cmdInsert.Parameters.Add("@accid", DbType.Int32)

                    cmdInsert.Parameters("@cid").Value = 1
                    cmdInsert.Parameters("@key").Value = "TEST_KEY"
                    cmdInsert.Parameters("@accid").Value = 999

                    Using reader = cmdInsert.ExecuteReader()
                        If reader.Read() Then
                            Console.WriteLine("Read: " & reader("cid").ToString() & ", " & reader("key").ToString() & ", " & reader("accid").ToString())
                        Else
                            Console.WriteLine("No read")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
