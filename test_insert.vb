Imports System
Imports System.Data
Imports System.Data.SQLite
Imports System.Collections.Generic

Module TestInsert
    Sub Main()
        Dim dbPath As String = "c:\Sys_Hes_Anb\Database\Sys_Hes_Anb.db"
        Dim connStr = "Data Source=" & dbPath & ";Version=3;"
        
        Try
            Dim _mappings As New Dictionary(Of String, HashSet(Of Integer))()
            _mappings("SALES") = New HashSet(Of Integer) From {101, 102}
            
            Dim compId = 1
            
            Using conn As New SQLiteConnection(connStr)
                conn.Open()
                Using tr = conn.BeginTransaction()
                    Using cmdDelete As New SQLiteCommand("DELETE FROM PnLAccountMappings WHERE CompanyID = @cid", conn, tr)
                        cmdDelete.Parameters.AddWithValue("@cid", compId)
                        cmdDelete.ExecuteNonQuery()
                    End Using

                    Using cmdInsert As New SQLiteCommand("INSERT INTO PnLAccountMappings (CompanyID, CategoryKey, AccountID) VALUES (@cid, @key, @accid)", conn, tr)
                        For Each kvp In _mappings
                            For Each accId In kvp.Value
                                cmdInsert.Parameters.Clear()
                                cmdInsert.Parameters.AddWithValue("@cid", compId)
                                cmdInsert.Parameters.AddWithValue("@key", kvp.Key)
                                cmdInsert.Parameters.AddWithValue("@accid", accId)
                                cmdInsert.ExecuteNonQuery()
                            Next
                        Next
                    End Using
                    tr.Commit()
                End Using
                
                ' Verify
                Using cmdCheck As New SQLiteCommand("SELECT COUNT(*) FROM PnLAccountMappings", conn)
                    Dim count = Convert.ToInt32(cmdCheck.ExecuteScalar())
                    Console.WriteLine("Rows inserted: " & count)
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
