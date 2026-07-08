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
        Public Property TxID As Integer = 0
        Public Property TxDate As String = ""
        Public Property RefNo As String = ""
        Public Property Debit As Decimal = 0D
        Public Property Credit As Decimal = 0D
        Public Property Description As String = ""
        Public Property Payee As String = ""
        Public Property MatchedDetailID As Integer? = Nothing
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

    Public Class SuggestedMatchedTransactionPair
        Public Property BankTx As BankTransaction
        Public Property LedgerTx As LedgerTransaction
        Public Property MatchProbability As Double
        Public Property MatchReason As String = ""
    End Class

    Public Class ReconciliationResult
        Public Property Matched As New List(Of MatchedTransactionPair)()
        Public Property UnmatchedBank As New List(Of BankTransaction)()
        Public Property UnmatchedLedger As New List(Of LedgerTransaction)()
        Public Property Suggestions As New List(Of SuggestedMatchedTransactionPair)()
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

        Public Function ReadBankFileRaw(filePath As String) As DataTable
            Dim ext = Path.GetExtension(filePath).ToLower()
            If ext = ".csv" Then
                Return ReadCsvRaw(filePath)
            ElseIf ext = ".xlsx" OrElse ext = ".xls" Then
                Return ReadExcelRaw(filePath)
            Else
                Throw New NotSupportedException("فرمت فایل انتخاب شده پشتیبانی نمی‌شود. لطفاً فایل CSV یا Excel انتخاب کنید.")
            End If
        End Function

        Private Function ReadExcelRaw(filePath As String) As DataTable
            Dim dt As New DataTable()
            Dim connStr As String
            Dim ext = Path.GetExtension(filePath).ToLower()
            If ext = ".xlsx" Then
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath & ";Extended Properties=""Excel 12.0 Xml;HDR=NO;IMEX=1"""
            Else
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filePath & ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1"""
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

        Private Function ReadCsvRaw(filePath As String) As DataTable
            Dim dt As New DataTable()
            Dim lines = File.ReadAllLines(filePath)
            If lines.Length = 0 Then Return dt

            ' Create generic columns
            Dim firstLineParts = SplitCsvLine(lines(0))
            For i As Integer = 0 To firstLineParts.Length - 1
                dt.Columns.Add("Column" & (i + 1), GetType(String))
            Next

            For i As Integer = 0 To lines.Length - 1
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

        Public Function PerformDatabaseReconciliation(companyId As Integer, fiscalYearId As Integer, bankId As Integer, accountId As Integer, fromDate As DateTime?, toDate As DateTime?) As ReconciliationResult
            Dim res As New ReconciliationResult()
            Dim ledgerTable = GetLedgerEntries(companyId, fiscalYearId, accountId, fromDate, toDate)

            ' 1. Load Bank statement from SoBank_2
            Dim bankTransactions As New List(Of BankTransaction)()
            Dim dtBank = Sql.ExecuteTable("SELECT TxID, TxDate, RefNo, Debit, Credit, Description, Payee, MatchedDetailID FROM SoBank_2 WHERE BankID = ?", bankId)
            For Each row As DataRow In dtBank.Rows
                Dim bt As New BankTransaction()
                bt.TxID = Convert.ToInt32(row("TxID"))
                bt.TxDate = Convert.ToString(row("TxDate"))
                bt.RefNo = Convert.ToString(row("RefNo"))
                bt.Debit = If(row.IsNull("Debit"), 0D, Convert.ToDecimal(row("Debit")))
                bt.Credit = If(row.IsNull("Credit"), 0D, Convert.ToDecimal(row("Credit")))
                bt.Description = Convert.ToString(row("Description"))
                bt.Payee = Convert.ToString(row("Payee"))
                If Not row.IsNull("MatchedDetailID") Then
                    bt.MatchedDetailID = Convert.ToInt32(row("MatchedDetailID"))
                End If
                bt.RawRow = row

                Dim includeRow = True
                If fromDate.HasValue OrElse toDate.HasValue Then
                    Dim txDateVal As DateTime
                    If TryParsePersianOrEnglishDate(bt.TxDate, txDateVal) Then
                        If fromDate.HasValue AndAlso txDateVal.Date < fromDate.Value.Date Then
                            includeRow = False
                        End If
                        If toDate.HasValue AndAlso txDateVal.Date > toDate.Value.Date Then
                            includeRow = False
                        End If
                    End If
                End If

                If includeRow Then
                    bankTransactions.Add(bt)
                End If
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
            Dim unmatchedBank As New List(Of BankTransaction)()

            ' 2.5 Separate already matched entries based on database MatchedDetailID
            For Each bt In bankTransactions
                If bt.MatchedDetailID.HasValue AndAlso bt.MatchedDetailID.Value > 0 Then
                    Dim found = unmatchedLedger.FirstOrDefault(Function(lt) lt.DetailID = bt.MatchedDetailID.Value)
                    If found IsNot Nothing Then
                        res.Matched.Add(New MatchedTransactionPair() With {
                            .BankTx = bt,
                            .LedgerTx = found
                        })
                        unmatchedLedger.Remove(found)
                        Continue For
                    End If
                End If
                unmatchedBank.Add(bt)
            Next

            ' 3. Priority 1 Matching: Exact match by Reference Number and Amount
            Dim remainingBank As New List(Of BankTransaction)()
            For Each bt In unmatchedBank
                Dim found As LedgerTransaction = Nothing
                If Not String.IsNullOrWhiteSpace(bt.RefNo) Then
                    found = unmatchedLedger.FirstOrDefault(Function(lt) 
                        Return (String.Equals(lt.TxNo, bt.RefNo, StringComparison.OrdinalIgnoreCase) OrElse 
                                String.Equals(lt.RefNo, bt.RefNo, StringComparison.OrdinalIgnoreCase)) AndAlso
                               ((bt.Debit > 0 AndAlso lt.Debit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Credit = bt.Credit))
                    End Function)
                End If

                If found IsNot Nothing Then
                    res.Matched.Add(New MatchedTransactionPair() With {
                        .BankTx = bt,
                        .LedgerTx = found
                    })
                    unmatchedLedger.Remove(found)
                Else
                    remainingBank.Add(bt)
                End If
            Next

            ' 4. Suggestion Algorithm: find the closest matches based on RefNo, Amount, Date, and Description
            Dim tempUnmatchedLedger As New List(Of LedgerTransaction)(unmatchedLedger)
            For Each bt In remainingBank
                Dim bestLedger As LedgerTransaction = Nothing
                Dim maxScore As Double = 0.0
                
                Dim btDate As DateTime
                Dim hasBtDate = TryParsePersianOrEnglishDate(bt.TxDate, btDate)

                ' Bank transaction direction and amount values
                Dim btIsDebit = bt.Debit > 0
                Dim btAmt = If(btIsDebit, bt.Debit, bt.Credit)

                For Each lt In tempUnmatchedLedger
                    Dim score As Double = 0.0

                    ' Ledger transaction direction and amount values
                    ' Note: in normal matching, bank debit matches ledger credit, and bank credit matches ledger debit
                    Dim correctDirection = (bt.Debit > 0 AndAlso lt.Credit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Debit = bt.Credit)
                    Dim swappedDirection = (bt.Debit > 0 AndAlso lt.Debit = bt.Debit) OrElse (bt.Credit > 0 AndAlso lt.Credit = bt.Credit)
                    
                    ' Reference numbers comparison
                    Dim refMatchesExactly = False
                    If Not String.IsNullOrWhiteSpace(bt.RefNo) Then
                        refMatchesExactly = String.Equals(lt.TxNo, bt.RefNo, StringComparison.OrdinalIgnoreCase) OrElse 
                                            String.Equals(lt.RefNo, bt.RefNo, StringComparison.OrdinalIgnoreCase)
                    End If

                    Dim refIsClose = False
                    If Not refMatchesExactly AndAlso Not String.IsNullOrWhiteSpace(bt.RefNo) Then
                        refIsClose = AreReferenceNumbersVeryClose(bt.RefNo, lt.TxNo) OrElse 
                                     AreReferenceNumbersVeryClose(bt.RefNo, lt.RefNo)
                    End If

                    ' Amounts comparison
                    Dim ltAmtCorrectDir = If(btIsDebit, lt.Credit, lt.Debit)
                    Dim ltAmtSwappedDir = If(btIsDebit, lt.Debit, lt.Credit)

                    Dim amtMatchesExactly = False
                    Dim amtIsClose = False

                    If correctDirection Then
                        amtMatchesExactly = (ltAmtCorrectDir = btAmt)
                        If Not amtMatchesExactly Then
                            amtIsClose = AreAmountsVeryClose(btAmt, ltAmtCorrectDir)
                        End If
                    ElseIf swappedDirection Then
                        amtMatchesExactly = (ltAmtSwappedDir = btAmt)
                        If Not amtMatchesExactly Then
                            amtIsClose = AreAmountsVeryClose(btAmt, ltAmtSwappedDir)
                        End If
                    End If

                    ' Now evaluate scores based on scenarios
                    If refMatchesExactly Then
                        If correctDirection AndAlso amtMatchesExactly Then
                            score = 100.0
                        ElseIf swappedDirection AndAlso amtMatchesExactly Then
                            ' Scenario 1.2: Ref matches, amount matches, but direction is swapped
                            score = 95.0
                        ElseIf correctDirection AndAlso amtIsClose Then
                            ' Scenario 1.3: Ref matches, direction correct, amount has typo
                            score = 90.0
                        ElseIf swappedDirection AndAlso amtIsClose Then
                            ' Scenario 1.4: Ref matches, direction swapped, amount has typo
                            score = 85.0
                        Else
                            ' Scenario 1.5: Ref matches, but amount is completely different
                            score = 75.0
                        End If
                    ElseIf refIsClose Then
                        If correctDirection AndAlso amtMatchesExactly Then
                            ' Scenario 2.1: Ref is close, direction correct, amount matches
                            score = 92.0
                        ElseIf swappedDirection AndAlso amtMatchesExactly Then
                            ' Scenario 2.2: Ref is close, direction swapped, amount matches
                            score = 85.0
                        ElseIf correctDirection AndAlso amtIsClose Then
                            ' Scenario 2.3: Ref is close, direction correct, amount has typo
                            score = 70.0
                        Else
                            score = 50.0
                        End If
                    Else
                        ' No RefNo match/proximity
                        Dim daysDiff = If(hasBtDate, Math.Abs((lt.EntryDate.Date - btDate.Date).TotalDays), 999.0)
                        
                        If correctDirection AndAlso amtMatchesExactly Then
                            ' Scenario 3.1: Same amount, correct direction (standard suggestion)
                            score = 60.0
                            If daysDiff <= 30 Then
                                score += 30.0 * (1.0 - (daysDiff / 30.0))
                            End If
                            Dim textOverlap = GetTextSimilarity(bt.Description & " " & bt.Payee, lt.Description)
                            score += 10.0 * textOverlap
                        ElseIf swappedDirection AndAlso amtMatchesExactly Then
                            ' Scenario 3.2: Same amount, swapped direction
                            score = 50.0
                            If daysDiff <= 30 Then
                                score += 20.0 * (1.0 - (daysDiff / 30.0))
                            End If
                            Dim textOverlap = GetTextSimilarity(bt.Description & " " & bt.Payee, lt.Description)
                            score += 10.0 * textOverlap
                        ElseIf correctDirection AndAlso amtIsClose AndAlso daysDiff <= 10 Then
                            ' Scenario 3.3: Amount typo, correct direction, within 10 days
                            score = 40.0
                            score += 20.0 * (1.0 - (daysDiff / 10.0))
                            Dim textOverlap = GetTextSimilarity(bt.Description & " " & bt.Payee, lt.Description)
                            score += 10.0 * textOverlap
                        End If
                    End If

                    If score > maxScore Then
                        maxScore = score
                        bestLedger = lt
                    End If
                Next

                ' If we find a match with probability above 40%, we suggest it
                If bestLedger IsNot Nothing AndAlso maxScore >= 40.0 Then
                    res.Suggestions.Add(New SuggestedMatchedTransactionPair() With {
                        .BankTx = bt,
                        .LedgerTx = bestLedger,
                        .MatchProbability = Math.Round(maxScore, 1)
                    })
                    tempUnmatchedLedger.Remove(bestLedger)
                End If

                ' Add to unmatched bank list for separate display
                res.UnmatchedBank.Add(bt)
            Next

            res.UnmatchedLedger = unmatchedLedger
            Return res
        End Function

        Private Function AreReferenceNumbersVeryClose(ref1 As String, ref2 As String) As Boolean
            If String.IsNullOrWhiteSpace(ref1) OrElse String.IsNullOrWhiteSpace(ref2) Then Return False
            ref1 = ref1.Trim()
            ref2 = ref2.Trim()
            
            If ref1 = ref2 Then Return True
            
            If Math.Abs(ref1.Length - ref2.Length) > 1 Then Return False
            
            If GetLevenshteinDistance(ref1, ref2) <= 1 Then Return True
            
            If ref1.Length = ref2.Length Then
                Dim diffIndices As New List(Of Integer)()
                For i As Integer = 0 To ref1.Length - 1
                    If ref1(i) <> ref2(i) Then
                        diffIndices.Add(i)
                    End If
                Next
                If diffIndices.Count = 2 AndAlso diffIndices(1) - diffIndices(0) = 1 Then
                    If ref1(diffIndices(0)) = ref2(diffIndices(1)) AndAlso ref1(diffIndices(1)) = ref2(diffIndices(0)) Then
                        Return True
                    End If
                End If
            End If
            
            Return False
        End Function

        Private Function AreAmountsVeryClose(amt1 As Decimal, amt2 As Decimal) As Boolean
            Dim s1 = amt1.ToString("F0")
            Dim s2 = amt2.ToString("F0")
            Return AreReferenceNumbersVeryClose(s1, s2)
        End Function

        Private Function GetLevenshteinDistance(s As String, t As String) As Integer
            Dim n As Integer = s.Length
            Dim m As Integer = t.Length
            If n = 0 Then Return m
            If m = 0 Then Return n

            Dim d(n, m) As Integer

            For i As Integer = 0 To n
                d(i, 0) = i
            Next
            For j As Integer = 0 To m
                d(0, j) = j
            Next

            For i As Integer = 1 To n
                For j As Integer = 1 To m
                    Dim cost As Integer = If(t(j - 1) = s(i - 1), 0, 1)
                    d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + cost)
                Next
            Next
            Return d(n, m)
        End Function

        Private Function GetTextSimilarity(text1 As String, text2 As String) As Double
            If String.IsNullOrWhiteSpace(text1) OrElse String.IsNullOrWhiteSpace(text2) Then Return 0.0
            
            Dim words1 = text1.ToLower().Split(New Char() {" "c, ","c, ";"c, "-"c}, StringSplitOptions.RemoveEmptyEntries)
            Dim words2 = text2.ToLower().Split(New Char() {" "c, ","c, ";"c, "-"c}, StringSplitOptions.RemoveEmptyEntries)
            
            Dim set1 = words1.Where(Function(w) w.Length >= 3).Distinct().ToList()
            Dim set2 = words2.Where(Function(w) w.Length >= 3).Distinct().ToList()
            
            If set1.Count = 0 OrElse set2.Count = 0 Then Return 0.0
            
            Dim common = set1.Intersect(set2).Count()
            Dim total = set1.Union(set2).Count()
            
            Return Convert.ToDouble(common) / Convert.ToDouble(total)
        End Function
    End Class
End Namespace
