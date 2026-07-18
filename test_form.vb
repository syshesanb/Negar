Imports System
Imports System.Data
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Forms
Imports Sys_Hes_Anb.Business

Module TestForm
    Sub Main()
        Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            
            ' Mock session
            SessionContext.CurrentCompanyID = 1
            
            Console.WriteLine("Instantiating form...")
            Dim frm As New HesabdaryProfitLossMappingForm()
            
            ' Force form load to populate tree
            frm.Show()
            
            ' Find tvAccounts
            Dim tvAccounts As TreeView = DirectCast(frm.Controls.Find("tvAccounts", True)(0), TreeView)
            Console.WriteLine("Nodes count: " & tvAccounts.Nodes.Count)
            
            If tvAccounts.Nodes.Count > 0 Then
                Dim firstNode = tvAccounts.Nodes(0)
                Console.WriteLine("Checking node: " & firstNode.Text)
                firstNode.Checked = True
                
                ' Find btnSave
                Dim btnSave As Button = DirectCast(frm.Controls.Find("btnSave", True)(0), Button)
                Console.WriteLine("Clicking save...")
                btnSave.PerformClick()
            End If
            
            ' Check DB
            Dim dbPath As String = "c:\Sys_Hes_Anb\Database\Sys_Hes_Anb.db"
            Dim connStr = "Data Source=" & dbPath & ";Version=3;"
            Using conn As New SQLiteConnection(connStr)
                conn.Open()
                Using cmdCheck As New SQLiteCommand("SELECT COUNT(*) FROM PnLAccountMappings", conn)
                    Dim count = Convert.ToInt32(cmdCheck.ExecuteScalar())
                    Console.WriteLine("Rows in DB: " & count)
                End Using
            End Using
            
        Catch ex As Exception
            Console.WriteLine("ERROR: " & ex.Message)
        End Try
    End Sub
End Module
