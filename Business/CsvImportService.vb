Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.FileIO
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class CsvImportService

        Public Shared Sub CreateTemplate(type As String, savePath As String)
            Dim content As String = ""
            Select Case type
                Case "Products"
                    content = "ProductCode,ProductName,Unit,DefaultPrice,Category"
                Case "Users"
                    content = "Username,FullName,UserType,Password"
                Case "CoA"
                    content = "AccountCode,AccountName,AccountType,ParentAccountCode,AccountNature"
                Case "SarfaslShenavar"
                    content = "AccountCode,AccountName,ParentAccountCode"
                Case "Docs"
                    content = "EntryDate,ReferenceNumber,Description,AccountCode,ShenavarCode,Debit,Credit,SharhRadif"
            End Select
            
            ' Write with UTF-8 BOM so Excel opens it correctly in Persian
            File.WriteAllText(savePath, content, New UTF8Encoding(True))
        End Sub

        Private Shared Function ReadFileToDataTable(filePath As String) As DataTable
            Dim dt As New DataTable()
            Dim ext = Path.GetExtension(filePath).ToLower()
            
            If ext = ".csv" Then
                Using parser As New TextFieldParser(filePath)
                    parser.TextFieldType = FieldType.Delimited
                    parser.SetDelimiters(",")
                    
                    If Not parser.EndOfData Then
                        Dim headers = parser.ReadFields()
                        For Each h In headers
                            dt.Columns.Add(h)
                        Next
                    End If
                    
                    While Not parser.EndOfData
                        Dim fields = parser.ReadFields()
                        If fields.Length = dt.Columns.Count Then
                            dt.Rows.Add(fields)
                        Else
                            Dim row = dt.NewRow()
                            For i = 0 To Math.Min(fields.Length, dt.Columns.Count) - 1
                                row(i) = fields(i)
                            Next
                            dt.Rows.Add(row)
                        End If
                    End While
                End Using
            ElseIf ext = ".xlsx" Or ext = ".xls" Then
                Dim connString As String
                If ext = ".xlsx" Then
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath & ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;';"
                Else
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filePath & ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';"
                End If
                
                Using conn As New OleDbConnection(connString)
                    conn.Open()
                    Dim schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                    If schemaTable.Rows.Count > 0 Then
                        Dim sheetName = schemaTable.Rows(0)("TABLE_NAME").ToString()
                        Using cmd As New OleDbCommand("SELECT * FROM [" & sheetName & "]", conn)
                            Using adapter As New OleDbDataAdapter(cmd)
                                adapter.Fill(dt)
                            End Using
                        End Using
                    End If
                End Using
            End If
            
            Return dt
        End Function

        Public Shared Function ImportData(type As String, filePath As String) As String
            Dim successCount = 0
            Dim errorCount = 0
            Dim errorLog As New StringBuilder()
            
            Dim dt As DataTable
            Try
                dt = ReadFileToDataTable(filePath)
            Catch ex As Exception
                Return "خطا در خواندن فایل. لطفاً مطمئن شوید فایل توسط برنامه دیگری باز نیست. جزئیات: " & ex.Message
            End Try
            
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return "فایل خالی است یا ساختار درستی ندارد."
            End If

            If type = "Products" Then
                For Each row As DataRow In dt.Rows
                    Try
                        Dim code = Convert.ToString(row(0)).Trim()
                        Dim name = Convert.ToString(row(1)).Trim()
                        Dim unit = If(dt.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "")
                        Dim priceStr = If(dt.Columns.Count > 3, Convert.ToString(row(3)).Trim(), "0")
                        Dim cat = If(dt.Columns.Count > 4, Convert.ToString(row(4)).Trim(), "")
                        
                        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(name) Then Continue For

                        Dim price As Decimal = 0
                        Decimal.TryParse(priceStr, price)
                        
                        Dim exists = Convert.ToInt32(Sql.ExecuteScalar("SELECT COUNT(*) FROM Products WHERE ProductCode = ?", code))
                        If exists = 0 Then
                            Sql.ExecuteNonQuery("INSERT INTO Products (ProductCode, ProductName, Unit, DefaultPrice, Category, IsActive) VALUES (?, ?, ?, ?, ?, 1)",
                                                code, name, unit, price, cat)
                            successCount += 1
                        Else
                            errorCount += 1
                            errorLog.AppendLine("کد کالا تکراری است: " & code)
                        End If
                    Catch ex As Exception
                        errorCount += 1
                        errorLog.AppendLine("خطا در پردازش سطر کالا: " & ex.Message)
                    End Try
                Next
                
            ElseIf type = "Users" Then
                For Each row As DataRow In dt.Rows
                    Try
                        Dim uname = Convert.ToString(row(0)).Trim()
                        Dim fname = Convert.ToString(row(1)).Trim()
                        Dim utype = If(dt.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "کاربر عادی")
                        Dim pass = If(dt.Columns.Count > 3, Convert.ToString(row(3)).Trim(), "123")
                        
                        If String.IsNullOrEmpty(uname) OrElse String.IsNullOrEmpty(fname) Then Continue For

                        Dim hash = PasswordHasher.Hash(pass)
                        
                        Dim exists = Convert.ToInt32(Sql.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE Username = ?", uname))
                        If exists = 0 Then
                            Sql.ExecuteNonQuery("INSERT INTO Users (Username, FullName, UserType, PasswordHash, IsActive, CreatedDate) VALUES (?, ?, ?, ?, 1, datetime('now'))",
                                                uname, fname, utype, hash)
                            successCount += 1
                        Else
                            errorCount += 1
                            errorLog.AppendLine("نام کاربری تکراری است: " & uname)
                        End If
                    Catch ex As Exception
                        errorCount += 1
                        errorLog.AppendLine("خطا در پردازش سطر کاربر: " & ex.Message)
                    End Try
                Next
                
            ElseIf type = "CoA" Then
                Dim companyId = SessionContext.CurrentCompanyID.Value
                For Each row As DataRow In dt.Rows
                    Try
                        Dim code = Convert.ToString(row(0)).Trim()
                        Dim name = Convert.ToString(row(1)).Trim()
                        Dim accType = If(dt.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "معین")
                        Dim pCode = If(dt.Columns.Count > 3, Convert.ToString(row(3)).Trim(), "")
                        Dim nature = If(dt.Columns.Count > 4, Convert.ToString(row(4)).Trim(), "بدهکار")
                        
                        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(name) Then Continue For

                        ' Get Parent ID
                        Dim parentId As Object = DBNull.Value
                        If Not String.IsNullOrEmpty(pCode) Then
                            Dim pIdObj = Sql.ExecuteScalar("SELECT AccountID FROM SarfaslHesab WHERE CompanyID = ? AND AccountCode = ?", companyId, pCode)
                            If pIdObj IsNot Nothing AndAlso pIdObj IsNot DBNull.Value Then
                                parentId = pIdObj
                            End If
                        End If
                        
                        Dim exists = Convert.ToInt32(Sql.ExecuteScalar("SELECT COUNT(*) FROM SarfaslHesab WHERE CompanyID = ? AND AccountCode = ?", companyId, code))
                        If exists = 0 Then
                            Sql.ExecuteNonQuery("INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature) VALUES (?, ?, ?, ?, ?, 1, ?)",
                                                companyId, code, name, accType, parentId, nature)
                            successCount += 1
                        Else
                            errorCount += 1
                            errorLog.AppendLine("کد سرفصل تکراری است: " & code)
                        End If
                    Catch ex As Exception
                        errorCount += 1
                        errorLog.AppendLine("خطا در پردازش سرفصل: " & ex.Message)
                    End Try
                Next
                
            ElseIf type = "SarfaslShenavar" Then
                Dim companyId = SessionContext.CurrentCompanyID.Value
                For Each row As DataRow In dt.Rows
                    Try
                        Dim code = Convert.ToString(row(0)).Trim()
                        Dim name = Convert.ToString(row(1)).Trim()
                        Dim pCode = If(dt.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "")
                        
                        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(name) Then Continue For

                        ' Get Parent ID
                        Dim parentId As Object = DBNull.Value
                        If Not String.IsNullOrEmpty(pCode) Then
                            Dim pIdObj = Sql.ExecuteScalar("SELECT ShenavarID FROM SarfaslShenavar WHERE CompanyID = ? AND AccountCode = ?", companyId, pCode)
                            If pIdObj IsNot Nothing AndAlso pIdObj IsNot DBNull.Value Then
                                parentId = pIdObj
                            End If
                        End If
                        
                        Dim exists = Convert.ToInt32(Sql.ExecuteScalar("SELECT COUNT(*) FROM SarfaslShenavar WHERE CompanyID = ? AND AccountCode = ?", companyId, code))
                        If exists = 0 Then
                            Sql.ExecuteNonQuery("INSERT INTO SarfaslShenavar (CompanyID, AccountCode, AccountName, ParentShenavarID, IsActive) VALUES (?, ?, ?, ?, 1)",
                                                companyId, code, name, parentId)
                            successCount += 1
                        Else
                            errorCount += 1
                            errorLog.AppendLine("کد شناور تکراری است: " & code)
                        End If
                    Catch ex As Exception
                        errorCount += 1
                        errorLog.AppendLine("خطا در پردازش شناور: " & ex.Message)
                    End Try
                Next
                
            ElseIf type = "Docs" Then
                Dim companyId = SessionContext.CurrentCompanyID.Value
                Dim fyId = SessionContext.CurrentFiscalYearID.Value
                Dim userId = SessionContext.CurrentUser.UserID
                
                ' Group by RefNumber and EntryDate
                Dim view As New DataView(dt)
                Dim uniqueRefs = view.ToTable(True, dt.Columns(0).ColumnName, dt.Columns(1).ColumnName) ' EntryDate, ReferenceNumber
                
                For Each refRow As DataRow In uniqueRefs.Rows
                    Dim entryDate = Convert.ToString(refRow(0)).Trim()
                    Dim refNo = Convert.ToString(refRow(1)).Trim()
                    If String.IsNullOrEmpty(entryDate) OrElse String.IsNullOrEmpty(refNo) Then Continue For
                    
                    ' Find details
                    Dim detailRows = dt.Select("[" & dt.Columns(0).ColumnName & "] = '" & entryDate.Replace("'", "''") & "' AND [" & dt.Columns(1).ColumnName & "] = '" & refNo.Replace("'", "''") & "'")
                    
                    Dim sumDebit As Decimal = 0
                    Dim sumCredit As Decimal = 0
                    
                    For Each dRow In detailRows
                        Dim d = If(dt.Columns.Count > 5, Convert.ToString(dRow(5)).Trim(), "0")
                        Dim c = If(dt.Columns.Count > 6, Convert.ToString(dRow(6)).Trim(), "0")
                        Dim decD As Decimal = 0
                        Dim decC As Decimal = 0
                        Decimal.TryParse(d, decD)
                        Decimal.TryParse(c, decC)
                        sumDebit += decD
                        sumCredit += decC
                    Next
                    
                    If sumDebit <> sumCredit Then
                        errorCount += 1
                        errorLog.AppendLine("سند عطف " & refNo & " ناتراز است (جمع بدهکار: " & sumDebit & "، جمع بستانکار: " & sumCredit & ") و ذخیره نشد.")
                        Continue For
                    End If
                    
                    Try
                        ' Create Master Entry
                        Dim entryDesc = If(dt.Columns.Count > 2, Convert.ToString(detailRows(0)(2)).Trim(), "سند انتقالی")
                        Dim entryId = Convert.ToInt32(Sql.ExecuteIdentity(
                            "INSERT INTO Sanad1 (CompanyID, FiscalYearID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, JamBedehkar, JamBestankar, TaeazSanad) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, 1)",
                            companyId, fyId, entryDate, entryDesc, refNo, userId, "سند دائم", sumDebit, sumCredit))
                        
                        Dim lineNo = 1
                        For Each dRow In detailRows
                            Dim accCode = If(dt.Columns.Count > 3, Convert.ToString(dRow(3)).Trim(), "")
                            Dim shenCode = If(dt.Columns.Count > 4, Convert.ToString(dRow(4)).Trim(), "")
                            Dim debitStr = If(dt.Columns.Count > 5, Convert.ToString(dRow(5)).Trim(), "0")
                            Dim creditStr = If(dt.Columns.Count > 6, Convert.ToString(dRow(6)).Trim(), "0")
                            Dim sharhRadif = If(dt.Columns.Count > 7, Convert.ToString(dRow(7)).Trim(), "")
                            
                            Dim decD As Decimal = 0
                            Dim decC As Decimal = 0
                            Decimal.TryParse(debitStr, decD)
                            Decimal.TryParse(creditStr, decC)
                            
                            Dim accIdObj = Sql.ExecuteScalar("SELECT AccountID FROM SarfaslHesab WHERE CompanyID = ? AND AccountCode = ?", companyId, accCode)
                            Dim accId As Object = If(accIdObj IsNot Nothing AndAlso accIdObj IsNot DBNull.Value, accIdObj, DBNull.Value)
                            
                            Dim shenId As Object = DBNull.Value
                            If Not String.IsNullOrEmpty(shenCode) Then
                                Dim sObj = Sql.ExecuteScalar("SELECT ShenavarID FROM SarfaslShenavar WHERE CompanyID = ? AND AccountCode = ?", companyId, shenCode)
                                If sObj IsNot Nothing AndAlso sObj IsNot DBNull.Value Then shenId = sObj
                            End If
                            
                            Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, AccountID, DebitAmount, CreditAmount, LineNumber, ShenavarID, SharhRadif) VALUES (?, ?, ?, ?, ?, ?, ?)",
                                                entryId, accId, decD, decC, lineNo, shenId, If(String.IsNullOrEmpty(sharhRadif), DBNull.Value, sharhRadif))
                            lineNo += 1
                        Next
                        successCount += 1
                    Catch ex As Exception
                        errorCount += 1
                        errorLog.AppendLine("خطا در ثبت سند عطف " & refNo & ": " & ex.Message)
                    End Try
                Next
            End If
            
            Dim result = "عملیات پایان یافت." & vbCrLf & "ردیف‌های موفق: " & successCount & vbCrLf & "ردیف‌های ناموفق: " & errorCount
            If errorCount > 0 Then
                result &= vbCrLf & vbCrLf & "جزئیات خطاها:" & vbCrLf & errorLog.ToString()
            End If
            Return result
        End Function
        Public Shared Sub SmartConvertCoA(sourceFile As String, destFile As String)
            Dim dt = ReadFileToDataTable(sourceFile)
            Dim sb As New StringBuilder()
            ' Standard template: AccountCode,AccountName,AccountType,ParentAccountCode,AccountNature
            sb.AppendLine("AccountCode,AccountName,AccountType,ParentAccountCode,AccountNature")
            
            For Each row As DataRow In dt.Rows
                ' Columns: 0:گروه 1:کل 2:معین 3:تفصیلی1 4:تفصیلی2 5:نام حساب
                Dim g = If(dt.Columns.Count > 0, Convert.ToString(row(0)).Trim(), "")
                Dim k = If(dt.Columns.Count > 1, Convert.ToString(row(1)).Trim(), "")
                Dim m = If(dt.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "")
                Dim t1 = If(dt.Columns.Count > 3, Convert.ToString(row(3)).Trim(), "")
                Dim t2 = If(dt.Columns.Count > 4, Convert.ToString(row(4)).Trim(), "")
                Dim name = If(dt.Columns.Count > 5, Convert.ToString(row(5)).Trim(), "")
                
                If String.IsNullOrEmpty(name) Then Continue For
                
                Dim isG = Not String.IsNullOrEmpty(g) AndAlso g <> "0" AndAlso g <> "0000"
                Dim isK = Not String.IsNullOrEmpty(k) AndAlso k <> "0" AndAlso k <> "0000"
                Dim isM = Not String.IsNullOrEmpty(m) AndAlso m <> "0" AndAlso m <> "0000"
                Dim isT1 = Not String.IsNullOrEmpty(t1) AndAlso t1 <> "0" AndAlso t1 <> "0000"
                Dim isT2 = Not String.IsNullOrEmpty(t2) AndAlso t2 <> "0" AndAlso t2 <> "0000"
                
                Dim code = ""
                Dim parent = ""
                Dim type = ""
                
                If isT2 AndAlso isT1 AndAlso isM AndAlso isK AndAlso isG Then
                    type = "تفصیلی2"
                    code = t2
                    parent = t1
                ElseIf isT1 AndAlso isM AndAlso isK AndAlso isG Then
                    type = "تفصیلی1"
                    code = t1
                    parent = m
                ElseIf isM AndAlso isK AndAlso isG Then
                    type = "معین"
                    code = m
                    parent = k
                ElseIf isK AndAlso isG Then
                    type = "کل"
                    code = k
                    parent = g
                ElseIf isG Then
                    type = "گروه"
                    code = g
                    parent = ""
                Else
                    Continue For
                End If
                
                sb.AppendLine(String.Format("{0},{1},{2},{3},بدهکار", code, name, type, parent))
            Next
            
            File.WriteAllText(destFile, sb.ToString(), New UTF8Encoding(True))
        End Sub

        Public Shared Sub SmartConvertDocs(sanad1Path As String, sanad2Path As String, destFile As String)
            Dim dt1 = ReadFileToDataTable(sanad1Path)
            Dim dt2 = ReadFileToDataTable(sanad2Path)
            
            ' Standard template: EntryDate,ReferenceNumber,Description,AccountCode,ShenavarCode,Debit,Credit,SharhRadif
            Dim sb As New StringBuilder()
            sb.AppendLine("EntryDate,ReferenceNumber,Description,AccountCode,ShenavarCode,Debit,Credit,SharhRadif")
            
            ' Dictionary of sanad1 for quick lookup: Key=DocNo, Value=(Date, Desc)
            Dim headers As New Dictionary(Of String, Tuple(Of String, String))()
            For Each r1 As DataRow In dt1.Rows
                Dim docNo = If(dt1.Columns.Count > 0, Convert.ToString(r1(0)).Trim(), "")
                Dim sDate = If(dt1.Columns.Count > 1, Convert.ToString(r1(1)).Trim(), "")
                Dim sDesc = If(dt1.Columns.Count > 2, Convert.ToString(r1(2)).Trim(), "")
                If Not String.IsNullOrEmpty(docNo) AndAlso Not headers.ContainsKey(docNo) Then
                    headers.Add(docNo, New Tuple(Of String, String)(sDate, sDesc))
                End If
            Next
            
            For Each row As DataRow In dt2.Rows
                ' sanad2 columns: 0:DocNo, 1:گروه, 2:کل, 3:معین, 4:تفصیلی1, 5:تفصیلی2, 6:بدهکار, 7:بستانکار
                Dim docNo = If(dt2.Columns.Count > 0, Convert.ToString(row(0)).Trim(), "")
                Dim g = If(dt2.Columns.Count > 1, Convert.ToString(row(1)).Trim(), "")
                Dim k = If(dt2.Columns.Count > 2, Convert.ToString(row(2)).Trim(), "")
                Dim m = If(dt2.Columns.Count > 3, Convert.ToString(row(3)).Trim(), "")
                Dim t1 = If(dt2.Columns.Count > 4, Convert.ToString(row(4)).Trim(), "")
                Dim t2 = If(dt2.Columns.Count > 5, Convert.ToString(row(5)).Trim(), "")
                Dim debit = If(dt2.Columns.Count > 6, Convert.ToString(row(6)).Trim(), "0")
                Dim credit = If(dt2.Columns.Count > 7, Convert.ToString(row(7)).Trim(), "0")
                
                If String.IsNullOrEmpty(docNo) Then Continue For
                
                Dim isG = Not String.IsNullOrEmpty(g) AndAlso g <> "0" AndAlso g <> "0000"
                Dim isK = Not String.IsNullOrEmpty(k) AndAlso k <> "0" AndAlso k <> "0000"
                Dim isM = Not String.IsNullOrEmpty(m) AndAlso m <> "0" AndAlso m <> "0000"
                Dim isT1 = Not String.IsNullOrEmpty(t1) AndAlso t1 <> "0" AndAlso t1 <> "0000"
                Dim isT2 = Not String.IsNullOrEmpty(t2) AndAlso t2 <> "0" AndAlso t2 <> "0000"
                
                Dim accCode = ""
                
                If isT2 AndAlso isT1 AndAlso isM AndAlso isK AndAlso isG Then
                    accCode = t2
                ElseIf isT1 AndAlso isM AndAlso isK AndAlso isG Then
                    accCode = t1
                ElseIf isM AndAlso isK AndAlso isG Then
                    accCode = m
                ElseIf isK AndAlso isG Then
                    accCode = k
                ElseIf isG Then
                    accCode = g
                End If
                
                Dim sDate = ""
                Dim sDesc = ""
                If headers.ContainsKey(docNo) Then
                    sDate = headers(docNo).Item1
                    sDesc = headers(docNo).Item2
                End If
                
                sb.AppendLine(String.Format("{0},{1},{2},{3},,{4},{5},", sDate, docNo, sDesc, accCode, debit, credit))
            Next
            
            File.WriteAllText(destFile, sb.ToString(), New UTF8Encoding(True))
        End Sub

    End Class
End Namespace
