Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Linq
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class ColumnMapping
        Public Property DateIndex As Integer = -1
        Public Property RefIndex As Integer = -1
        Public Property DebitIndex As Integer = -1
        Public Property CreditIndex As Integer = -1
        Public Property DescIndex As Integer = -1
    End Class

    Public Class BankTransaction
        Public Property TxDate As String = ""
        Public Property RefNo As String = ""
        Public Property Debit As Decimal = 0D
        Public Property Credit As Decimal = 0D
        Public Property Description As String = ""
        Public Property RawRow As DataRow
    End Class

    Public Class LedgerTransaction
        Public Property DetailID As Integer
        Public Property EntryID As Integer
        Public Property EntryDate As DateTime
        Public Property RefNo As String = ""
        Public Property Debit As Decimal = 0D
        Public Property Credit As Decimal = 0D
        Public Property Description As String = ""
        Public Property TxNo As String = ""
        Public Property TxDate As String = ""
        Public Property RawRow As DataRow
    End Class

    Public Class MatchedTransactionPair
        Public Property BankTx As BankTransaction
        Public Property LedgerTx As LedgerTransaction
    End Class

    Public Class ReconciliationResult
        Public Property Matched As New List(Of MatchedTransactionPair)()
        Public Property UnmatchedBank As New List(Of BankTransaction)()
        Public Property UnmatchedLedger As New List(Of LedgerTransaction)()
    End Class

    Public Class BankReconciliationService
        Public Function ReadBankFile(filePath As String) As DataTable
            Dim ext = Path.GetExtension(filePath).ToLower()
            If ext = ".csv" Then
                Return ReadCsv(filePath)
            ElseIf ext = ".xlsx" OrElse ext = ".xls" Then
                Return ReadExcel(filePath)
            Else
                Throw New NotSupportedException("فرمت فایل انتخاب شده پشتیبانی نمی‌شود. لطفاً فایل CSV یا Excel انتخاب کنید.")
            End If
        End Function

        Private Function ReadExcel(filePath As String) As DataTable
            Dim dt As New DataTable()
            Dim connStr As String
            Dim ext = Path.GetExtension(filePath).ToLower()
            If ext = ".xlsx" Then
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath & ";Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1"""
            Else
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filePath & ";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1"""
            End If

            Using conn As New OleDbConnection(connStr)
                conn.Open()
                Dim schemaTable = conn.GetSchema("Tables")
                If schemaTable.Rows.Count > 0 Then
                    Dim sheetName = schemaTable.Rows(0)("TABLE_NAME").ToString()
                    Using cmd As New OleDbCommand("SELECT * FROM [" & sheetName & "]", conn)
                        Using adapter As New OleDbDataAdapter(cmd)
                            adapter.Fill(dt)
                        End Using
                    End Using
                End If
            End Using
            Return dt
        End Function

        Private Function ReadCsv(filePath As String) As DataTable
            Dim dt As New DataTable()
            Dim lines = File.ReadAllLines(filePath)
            If lines.Length = 0 Then Return dt

            Dim headers = SplitCsvLine(lines(0))
            For i As Integer = 0 To headers.Length - 1
                Dim colName = If(String.IsNullOrWhiteSpace(headers(i)), "Column" & (i + 1), headers(i).Trim())
                Dim name = colName
                Dim counter = 1
                While dt.Columns.Contains(name)
                    name = colName & "_" & counter
                    counter += 1
                End While
                dt.Columns.Add(name, GetType(String))
            Next

            For i As Integer = 1 To lines.Length - 1
                Dim line = lines(i)
                If String.IsNullOrWhiteSpace(line) Then Continue For
                Dim parts = SplitCsvLine(line)
                Dim row = dt.NewRow()
                For j As Integer = 0 To Math.Min(parts.Length, dt.Columns.Count) - 1
                    row(j) = parts(j)
                Next
                dt.Rows.Add(row)
            Next
            Return dt
        End Function

        Private Function SplitCsvLine(line As String) As String()
            Dim parts As New List(Of String)()
            Dim inQuotes As Boolean = False
            Dim currentToken As New System.Text.StringBuilder()
            
            Dim separator As Char = ","c
            If line.Contains(";") AndAlso Not line.Contains(",") Then
                separator = ";"c
            End If

            For i As Integer = 0 To line.Length - 1
                Dim c = line(i)
                If c = """"c Then
                    inQuotes = Not inQuotes
                ElseIf c = separator AndAlso Not inQuotes Then
                    parts.Add(currentToken.ToString())
                    currentToken.Clear()
                Else
                    currentToken.Append(c)
                End If
            Next
            parts.Add(currentToken.ToString())
            Return parts.ToArray()
        End Function

        Public Function GetLedgerEntries(companyId As Integer, fiscalYearId As Integer, accountId As Integer, fromDate As DateTime?, toDate As DateTime?) As DataTable
            Dim query = "SELECT d.DetailID, e.EntryID, e.EntryDate, e.ReferenceNumber, d.DebitAmount, d.CreditAmount, d.SharhRadif, d.TransactionNumber, d.TransactionDate " &
                        "FROM AccountingEntryDetails d " &
                        "INNER JOIN AccountingEntries e ON d.EntryID = e.EntryID " &
                        "WHERE e.CompanyID = ? AND e.FiscalYearID = ? AND d.AccountID = ?"
            
            Dim params As New List(Of Object) From {companyId, fiscalYearId, accountId}
            
            If fromDate.HasValue Then
                query &= " AND e.EntryDate >= ?"
                params.Add(fromDate.Value)
            End If
            If toDate.HasValue Then
                query &= " AND e.EntryDate <= ?"
                params.Add(toDate.Value)
            End If

            Return Sql.ExecuteTable(query, params.ToArray())
        End Function

        Public Function PerformReconciliation(companyId As Integer, fiscalYearId As Integer, accountId As Integer, fromDate As DateTime?, toDate As DateTime?, bankTable As DataTable, colMap As ColumnMapping) As ReconciliationResult
            Dim res As New ReconciliationResult()
            Dim ledgerTable = GetLedgerEntries(companyId, fiscalYearId, accountId, fromDate, toDate)

            ' 1. Map Bank statement to objects
            Dim bankTransactions As New List(Of BankTransaction)()
            For Each row As DataRow In bankTable.Rows
                Dim bt As New BankTransaction()
                bt.RawRow = row
                
                If colMap.DateIndex >= 0 AndAlso colMap.DateIndex < bankTable.Columns.Count Then
                    bt.TxDate = Convert.ToString(row(colMap.DateIndex))
                End If
                If colMap.RefIndex >= 0 AndAlso colMap.RefIndex < bankTable.Columns.Count Then
                    bt.RefNo = Convert.ToString(row(colMap.RefIndex))
                End If
                If colMap.DescIndex >= 0 AndAlso colMap.DescIndex < bankTable.Columns.Count Then
                    bt.Description = Convert.ToString(row(colMap.DescIndex))
                End If
                
                If colMap.DebitIndex >= 0 AndAlso colMap.DebitIndex < bankTable.Columns.Count Then
                    Dim val = Convert.ToString(row(colMap.DebitIndex))
                    Decimal.TryParse(val, bt.Debit)
                End If
                If colMap.CreditIndex >= 0 AndAlso colMap.CreditIndex < bankTable.Columns.Count Then
                    Dim val = Convert.ToString(row(colMap.CreditIndex))
                    Decimal.TryParse(val, bt.Credit)
                End If
                
                bankTransactions.Add(bt)
            Next

            ' 2. Map Ledger rows to objects
            Dim ledgerTransactions As New List(Of LedgerTransaction)()
            For Each row As DataRow In ledgerTable.Rows
                Dim lt As New LedgerTransaction()
                lt.RawRow = row
                lt.DetailID = Convert.ToInt32(row("DetailID"))
                lt.EntryID = Convert.ToInt32(row("EntryID"))
                If Not row.IsNull("EntryDate") Then
                    lt.EntryDate = Convert.ToDateTime(row("EntryDate"))
                End If
                lt.RefNo = Convert.ToString(row("ReferenceNumber"))
                lt.Debit = If(row.IsNull("DebitAmount"), 0D, Convert.ToDecimal(row("DebitAmount")))
                lt.Credit = If(row.IsNull("CreditAmount"), 0D, Convert.ToDecimal(row("CreditAmount")))
                lt.Description = Convert.ToString(row("SharhRadif"))
                lt.TxNo = Convert.ToString(row("TransactionNumber"))
                lt.TxDate = Convert.ToString(row("TransactionDate"))
                ledgerTransactions.Add(lt)
            Next

            Dim unmatchedLedger As New List(Of LedgerTransaction)(ledgerTransactions)

            ' 3. Run Matching logic
            For Each bt In bankTransactions
                Dim found As LedgerTransaction = Nothing
                
                ' Match 1: Exact match by Reference Number and Amount
                If Not String.IsNullOrWhiteSpace(bt.RefNo) Then
                    found = unmatchedLedger.FirstOrDefault(Function(lt) 
                        Return (String.Equals(lt.TxNo, bt.RefNo, StringComparison.OrdinalIgnoreCase) OrElse 
                                String.Equals(lt.RefNo, bt.RefNo, StringComparison.OrdinalIgnoreCase)) AndAlso
                               ((bt.Debit > 0 AndAlso lt.Debit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Credit = bt.Credit))
                    End Function)
                End If
                
                ' Match 2: Match by Date (within 3 days) and Amount
                If found Is Nothing Then
                    found = unmatchedLedger.FirstOrDefault(Function(lt)
                        Dim dateMatch = False
                        Dim btDate As DateTime
                        If TryParsePersianOrEnglishDate(bt.TxDate, btDate) Then
                            dateMatch = Math.Abs((lt.EntryDate.Date - btDate.Date).TotalDays) <= 3
                        End If
                        
                        Return dateMatch AndAlso 
                               ((bt.Debit > 0 AndAlso lt.Debit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Credit = bt.Credit))
                    End Function)
                End If
                
                ' Match 3: Match by Amount only
                If found Is Nothing Then
                    found = unmatchedLedger.FirstOrDefault(Function(lt)
                        Return (bt.Debit > 0 AndAlso lt.Debit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Credit = bt.Credit)
                    End Function)
                End If

                If found IsNot Nothing Then
                    res.Matched.Add(New MatchedTransactionPair() With {
                        .BankTx = bt,
                        .LedgerTx = found
                    })
                    unmatchedLedger.Remove(found)
                Else
                    res.UnmatchedBank.Add(bt)
                End If
            Next

            res.UnmatchedLedger = unmatchedLedger
            Return res
        End Function

        Public Function TryParsePersianOrEnglishDate(dateStr As String, ByRef result As DateTime) As Boolean
            If String.IsNullOrWhiteSpace(dateStr) Then Return False
            
            dateStr = dateStr.Trim()
            If DateTime.TryParse(dateStr, result) Then Return True
            
            Try
                Dim cleaned = dateStr.Replace("-", "/").Replace("\", "/")
                Dim parts = cleaned.Split("/"c)
                If parts.Length = 3 Then
                    Dim year = Convert.ToInt32(parts(0))
                    Dim month = Convert.ToInt32(parts(1))
                    Dim day = Convert.ToInt32(parts(2))
                    
                    If year < 100 Then
                        year += 1400
                    End If
                    
                    Dim pc As New System.Globalization.PersianCalendar()
                    result = pc.ToDateTime(year, month, day, 0, 0, 0, 0)
                    Return True
                End If
            Catch
            End Try
            
            Return False
        End Function
    End Class
End Namespace
