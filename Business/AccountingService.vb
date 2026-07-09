Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class AccountingService
        Private ReadOnly logService As New ActivityLogService()

        ' ========================
        ' سرفصل حسابها (مشترک در تمام سالهای مالی یک شرکت)
        ' ========================

        Public Function GetAccounts() As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Return Sql.ExecuteTable(
                "SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature " &
                "FROM ChartOfAccounts WHERE CompanyID = ? ORDER BY AccountCode",
                SessionContext.CurrentCompanyID.Value)
        End Function

        Public Function GetCompanyAccountSettings() As Tuple(Of Integer, Integer, Integer, Integer, Integer, Integer)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return Tuple.Create(4, 2, 2, 2, 2, 2)
            Dim dt = Sql.ExecuteTable(
                "SELECT AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length FROM Companies WHERE CompanyID = ?",
                SessionContext.CurrentCompanyID.Value)
            If dt.Rows.Count = 0 Then Return Tuple.Create(4, 2, 2, 2, 2, 2)
            Dim row = dt.Rows(0)
            Dim levels = If(row("AccountLevels") Is DBNull.Value, 4, Convert.ToInt32(row("AccountLevels")))
            Dim l1 = If(row("Level1Length") Is DBNull.Value, 2, Convert.ToInt32(row("Level1Length")))
            Dim l2 = If(row("Level2Length") Is DBNull.Value, 2, Convert.ToInt32(row("Level2Length")))
            Dim l3 = If(row("Level3Length") Is DBNull.Value, 2, Convert.ToInt32(row("Level3Length")))
            Dim l4 = If(row("Level4Length") Is DBNull.Value, 2, Convert.ToInt32(row("Level4Length")))
            Dim l5 = If(row("Level5Length") Is DBNull.Value, 2, Convert.ToInt32(row("Level5Length")))
            Return Tuple.Create(levels, l1, l2, l3, l4, l5)
        End Function

        Private Sub UpdateDescendantsTypeAndNature(parentId As Integer, companyId As Integer, accountType As String, accountNature As String)
            Dim dt = Sql.ExecuteTable("SELECT AccountID FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID = ?", companyId, parentId)
            For Each row As DataRow In dt.Rows
                Dim childId = Convert.ToInt32(row("AccountID"))
                Sql.ExecuteNonQuery("UPDATE ChartOfAccounts SET AccountType = ?, AccountNature = ? WHERE AccountID = ?", accountType, accountNature, childId)
                UpdateDescendantsTypeAndNature(childId, companyId, accountType, accountNature)
            Next
        End Sub

        Public Function SaveAccount(accountId As Integer?, accountCode As String, accountName As String, accountType As String, parentAccountId As Integer?, isActive As Boolean, accountNature As String) As Integer
            If Not SessionContext.CurrentCompanyID.HasValue Then
                Throw New InvalidOperationException("ابتدا باید شرکت جاری را انتخاب کنید.")
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim parentVal = If(parentAccountId.HasValue, CType(parentAccountId.Value, Object), DBNull.Value)
            Dim excludeId = If(accountId.HasValue AndAlso accountId.Value > 0, accountId.Value, 0)

            ' چک تکراری بودن کد حساب در همان حساب مادر
            Dim dupCount As Integer
            If parentAccountId.HasValue Then
                dupCount = Convert.ToInt32(If(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM ChartOfAccounts WHERE CompanyID = ? AND AccountCode = ? AND ParentAccountID = ? AND AccountID <> ?",
                    companyId, accountCode, parentAccountId.Value, excludeId), 0))
            Else
                dupCount = Convert.ToInt32(If(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM ChartOfAccounts WHERE CompanyID = ? AND AccountCode = ? AND ParentAccountID IS NULL AND AccountID <> ?",
                    companyId, accountCode, excludeId), 0))
            End If

            If dupCount > 0 Then
                Throw New InvalidOperationException(
                    "کد حساب '" & accountCode & "' در همین حساب مادر قبلاً ثبت شده است." & Environment.NewLine &
                    "لطفاً کد حساب دیگری انتخاب کنید.")
            End If

            Dim parentVal2 = If(parentAccountId.HasValue, CType(parentAccountId.Value, Object), DBNull.Value)

            If accountId.HasValue AndAlso accountId.Value > 0 Then
                Sql.ExecuteNonQuery(
                    "UPDATE ChartOfAccounts SET AccountCode = ?, AccountName = ?, AccountType = ?, ParentAccountID = ?, IsActive = ?, AccountNature = ? WHERE AccountID = ? AND CompanyID = ?",
                    accountCode, accountName, accountType, parentVal2, isActive, accountNature, accountId.Value, companyId)
                
                If Not parentAccountId.HasValue Then
                    UpdateDescendantsTypeAndNature(accountId.Value, companyId, accountType, accountNature)
                End If

                Return accountId.Value
            End If

            Return Sql.ExecuteIdentity(
                "INSERT INTO ChartOfAccounts (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature) VALUES (?, ?, ?, ?, ?, ?, ?)",
                companyId, accountCode, accountName, accountType, parentVal2, isActive, accountNature)
        End Function

        Public Sub DeleteAccount(accountId As Integer)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Sql.ExecuteNonQuery("DELETE FROM ChartOfAccounts WHERE AccountID = ? AND CompanyID = ?",
                                accountId, SessionContext.CurrentCompanyID.Value)
        End Sub

        Public Function GetAccountsByParent(parentId As Integer?) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Dim companyId = SessionContext.CurrentCompanyID.Value
            If parentId.HasValue Then
                Return Sql.ExecuteTable(
                    "SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive " &
                    "FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID = ? ORDER BY AccountCode",
                    companyId, parentId.Value)
            Else
                Return Sql.ExecuteTable(
                    "SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive " &
                    "FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID IS NULL ORDER BY AccountCode",
                    companyId)
            End If
        End Function


        Public Function AccountHasChildren(accountId As Integer) As Boolean
            If Not SessionContext.CurrentCompanyID.HasValue Then Return False
            Dim count = Convert.ToInt32(If(Sql.ExecuteScalar(
                "SELECT COUNT(*) FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID = ?",
                SessionContext.CurrentCompanyID.Value, accountId), 0))
            Return count > 0
        End Function

        Public Function GetAccountsWithChildren() As HashSet(Of Integer)
            Dim hs As New HashSet(Of Integer)()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return hs
            Dim dt = Sql.ExecuteTable(
                "SELECT DISTINCT ParentAccountID FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID IS NOT NULL",
                SessionContext.CurrentCompanyID.Value)
            For Each r As DataRow In dt.Rows
                If Not r.IsNull("ParentAccountID") Then
                    hs.Add(Convert.ToInt32(r("ParentAccountID")))
                End If
            Next
            Return hs
        End Function

        Public Function GetAccountParentId(accountId As Integer) As Integer?
            Dim result = Sql.ExecuteScalar(
                "SELECT ParentAccountID FROM ChartOfAccounts WHERE AccountID = ?", accountId)
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Return Nothing
            Return Convert.ToInt32(result)
        End Function

        Public Function GetAccountName(accountId As Integer) As String
            Dim result = Sql.ExecuteScalar(
                "SELECT AccountName FROM ChartOfAccounts WHERE AccountID = ?", accountId)
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Return String.Empty
            Return Convert.ToString(result)
        End Function

        ' کد و نام یک سرفصل حساب را با هم برمی‌گرداند
        Public Function GetAccountInfo(accountId As Integer) As Tuple(Of String, String)
            Dim dt = Sql.ExecuteTable(
                "SELECT AccountCode, AccountName FROM ChartOfAccounts WHERE AccountID = ?", accountId)
            If dt.Rows.Count = 0 Then Return Tuple.Create("", "")
            Return Tuple.Create(Convert.ToString(dt.Rows(0)("AccountCode")),
                                Convert.ToString(dt.Rows(0)("AccountName")))
        End Function

        Public Function GetAccountHierarchyChain(accountId As Integer) As List(Of Tuple(Of String, String))
            Dim chain As New List(Of Tuple(Of String, String))()
            Dim currentId As Integer? = accountId
            Dim guard = 0
            Do While currentId.HasValue AndAlso guard < 50
                guard += 1
                Dim dt = Sql.ExecuteTable(
                    "SELECT AccountCode, AccountName, ParentAccountID FROM ChartOfAccounts WHERE AccountID = ?", currentId.Value)
                If dt.Rows.Count = 0 Then Exit Do
                Dim r = dt.Rows(0)
                Dim code = Convert.ToString(r("AccountCode"))
                Dim name = Convert.ToString(r("AccountName"))
                chain.Insert(0, Tuple.Create(code, name))
                
                Dim pVal = r("ParentAccountID")
                If pVal Is Nothing OrElse Convert.IsDBNull(pVal) Then
                    currentId = Nothing
                Else
                    currentId = Convert.ToInt32(pVal)
                End If
            Loop
            Return chain
        End Function

        ' از parentId شروع می‌کند و تا اولین والد (سطح اول) بالا می‌رود و نوع حساب آن را برمی‌گرداند
        Public Function SearchAll(codeFilter As String, nameFilter As String) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim query = "SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive " &
                        "FROM ChartOfAccounts WHERE CompanyID = ?"
            Dim params As New System.Collections.Generic.List(Of Object)()
            params.Add(companyId)
            If codeFilter.Length > 0 Then
                query &= " AND AccountCode LIKE ?"
                params.Add("%" & codeFilter & "%")
            End If
            If nameFilter.Length > 0 Then
                query &= " AND AccountName LIKE ?"
                params.Add("%" & nameFilter & "%")
            End If
            query &= " ORDER BY AccountCode"
            Return Sql.ExecuteTable(query, params.ToArray())
        End Function

        Public Function GetRootAncestorAccountType(parentId As Integer) As String
            Dim currentId = parentId
            Dim guard = 0
            Do While guard < 50
                guard += 1
                Dim dt = Sql.ExecuteTable(
                    "SELECT AccountType, ParentAccountID FROM ChartOfAccounts WHERE AccountID = ?", currentId)
                If dt.Rows.Count = 0 Then Return String.Empty
                Dim r = dt.Rows(0)
                Dim pVal = r("ParentAccountID")
                If pVal Is Nothing OrElse Convert.IsDBNull(pVal) Then
                    Return Convert.ToString(r("AccountType"))
                End If
                currentId = Convert.ToInt32(pVal)
            Loop
            Return String.Empty
        End Function

        Public Function GetRootAncestorAccountNature(parentId As Integer) As String
            Dim currentId = parentId
            Dim guard = 0
            Do While guard < 50
                guard += 1
                Dim dt = Sql.ExecuteTable(
                    "SELECT AccountNature, ParentAccountID FROM ChartOfAccounts WHERE AccountID = ?", currentId)
                If dt.Rows.Count = 0 Then Return String.Empty
                Dim r = dt.Rows(0)
                Dim pVal = r("ParentAccountID")
                If pVal Is Nothing OrElse Convert.IsDBNull(pVal) Then
                    Return Convert.ToString(r("AccountNature"))
                End If
                currentId = Convert.ToInt32(pVal)
            Loop
            Return String.Empty
        End Function

        Public Function GetNextSuggestedCode(parentId As Integer?) As String
            If Not SessionContext.CurrentCompanyID.HasValue Then Return "1"
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim maxCodeObj As Object
            If parentId.HasValue Then
                maxCodeObj = Sql.ExecuteScalar(
                    "SELECT MAX(AccountCode) FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID = ?",
                    companyId, parentId.Value)
            Else
                maxCodeObj = Sql.ExecuteScalar(
                    "SELECT MAX(AccountCode) FROM ChartOfAccounts WHERE CompanyID = ? AND ParentAccountID IS NULL",
                    companyId)
            End If
            If maxCodeObj Is Nothing OrElse Convert.IsDBNull(maxCodeObj) Then Return "1"
            Dim codeStr = Convert.ToString(maxCodeObj).Trim()
            Dim numVal As Long
            If Long.TryParse(codeStr, numVal) Then Return (numVal + 1).ToString()
            Return codeStr
        End Function

        ' ========================
        ' اسناد حسابداری (مخصوص شرکت و سال مالی جاری)
        ' ========================

        ' اطمینان از وجود ستون‌های اختیاری در جدول AccountingEntryDetails
        ' در صورت عدم وجود، ستون را اضافه می‌کند (خطای تکراری بودن بی‌صدا نادیده گرفته می‌شود)
        Public Sub EnsureEntryDetailsColumns()
            For Each ddl As String In New String() {
                "ALTER TABLE AccountingEntryDetails ADD COLUMN SharhRadif MEMO",
                "ALTER TABLE AccountingEntryDetails ADD COLUMN TransactionNumber TEXT(50)",
                "ALTER TABLE AccountingEntryDetails ADD COLUMN TransactionDate TEXT(10)"}
                Try
                    Sql.ExecuteNonQuery(ddl)
                Catch
                End Try
            Next
        End Sub

        Public Function IsReferenceNumberDuplicate(referenceNumber As String, excludeEntryId As Integer?) As Boolean
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return False
            Dim excludeId = If(excludeEntryId.HasValue, excludeEntryId.Value, -1)
            Dim count = Convert.ToInt32(If(Sql.ExecuteScalar(
                "SELECT COUNT(*) FROM AccountingEntries WHERE CompanyID = ? AND FiscalYearID = ? AND ReferenceNumber = ? AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL) AND EntryID <> ?",
                SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value, referenceNumber, excludeId), 0))
            Return count > 0
        End Function

        Public Function GetNextReferenceNumber() As String
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then Return "1"
            Dim result = Sql.ExecuteScalar(
                "SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM AccountingEntries WHERE CompanyID = ? AND FiscalYearID = ?",
                SessionContext.CurrentCompanyID.Value, SessionContext.CurrentFiscalYearID.Value)
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Return "1"
            Dim doubleVal As Double
            If Double.TryParse(Convert.ToString(result).Trim(), doubleVal) Then
                Return (Convert.ToInt64(doubleVal) + 1).ToString()
            End If
            Return Convert.ToString(result)
        End Function

        Public Function GetEntries() As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim baseSelect = "SELECT EntryID, ReferenceNumber, EntryDate, Description, JamBedehkar, JamBestankar, TaeazSanad, VazeiatSanad, AdamVirayesh, CreatedBy FROM AccountingEntries "
            Dim baseWhere = "WHERE CompanyID = ? AND FiscalYearID = ? AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL) "

            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Return Sql.ExecuteTable(baseSelect & baseWhere & "ORDER BY CAST(ReferenceNumber AS INTEGER) DESC", companyId, fyId)
            End If

            Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
            Return Sql.ExecuteTable(
                baseSelect & baseWhere & "AND CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ") ORDER BY CAST(ReferenceNumber AS INTEGER) DESC",
                companyId, fyId)
        End Function

        Public Function GetEntriesForPrint(fromRef As Integer?, toRef As Integer?, fromDateStr As String, toDateStr As String) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim baseSelect = "SELECT EntryID, ReferenceNumber, EntryDate, Description FROM AccountingEntries "
            Dim baseWhere = "WHERE CompanyID = ? AND FiscalYearID = ? AND (VazeiatSanad <> 'سند موقت - حذف موقت' OR VazeiatSanad IS NULL) "
            Dim params As New List(Of Object)
            params.Add(companyId)
            params.Add(fyId)

            If fromRef.HasValue OrElse toRef.HasValue Then
                If fromRef.HasValue AndAlso toRef.HasValue Then
                    Dim rStart = fromRef.Value
                    Dim rEnd = toRef.Value
                    If rStart > rEnd Then
                        Dim temp = rStart
                        rStart = rEnd
                        rEnd = temp
                    End If
                    baseWhere &= "AND CAST(ReferenceNumber AS INTEGER) >= ? AND CAST(ReferenceNumber AS INTEGER) <= ? "
                    params.Add(rStart)
                    params.Add(rEnd)
                ElseIf fromRef.HasValue Then
                    baseWhere &= "AND CAST(ReferenceNumber AS INTEGER) >= ? "
                    params.Add(fromRef.Value)
                ElseIf toRef.HasValue Then
                    baseWhere &= "AND CAST(ReferenceNumber AS INTEGER) <= ? "
                    params.Add(toRef.Value)
                End If
            ElseIf Not String.IsNullOrEmpty(fromDateStr) OrElse Not String.IsNullOrEmpty(toDateStr) Then
                Dim fromDate = If(Not String.IsNullOrEmpty(fromDateStr), PersianDateHelper.ParsePersianDate(fromDateStr), Nothing)
                Dim toDate = If(Not String.IsNullOrEmpty(toDateStr), PersianDateHelper.ParsePersianDate(toDateStr), Nothing)

                If fromDate.HasValue AndAlso toDate.HasValue Then
                    Dim dStart = fromDate.Value
                    Dim dEnd = toDate.Value
                    If dStart > dEnd Then
                        Dim temp = dStart
                        dStart = dEnd
                        dEnd = temp
                    End If
                    baseWhere &= "AND EntryDate >= ? AND EntryDate <= ? "
                    params.Add(dStart)
                    params.Add(dEnd)
                ElseIf fromDate.HasValue Then
                    baseWhere &= "AND EntryDate >= ? "
                    params.Add(fromDate.Value)
                ElseIf toDate.HasValue Then
                    baseWhere &= "AND EntryDate <= ? "
                    params.Add(toDate.Value)
                End If
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                baseWhere &= "AND CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ") "
            End If

            Return Sql.ExecuteTable(baseSelect & baseWhere & "ORDER BY CAST(ReferenceNumber AS INTEGER) ASC", params.ToArray())
        End Function

        ' اطلاعات ناوبری: آخرین سند، سند قبلی و سند بعدی نسبت به شماره سند جاری
        ' ستون‌های DataTable برگشتی: Title, RefNumber, DateStr
        Public Function GetEntryNavigationInfo(currentRef As String) As DataTable
            Dim result As New DataTable()
            result.Columns.Add("Title", GetType(String))
            result.Columns.Add("RefNumber", GetType(String))
            result.Columns.Add("DateStr", GetType(String))

            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                result.Rows.Add("آخرین سند", "-", "-")
                result.Rows.Add("سند قبلی", "-", "-")
                result.Rows.Add("سند بعدی", "-", "-")
                Return result
            End If

            Dim cid = SessionContext.CurrentCompanyID.Value
            Dim fyid = SessionContext.CurrentFiscalYearID.Value
            Dim numRef As Long = 0
            Long.TryParse(currentRef.Trim(), numRef)

            Dim addRow = Sub(title As String, dt As DataTable)
                If dt.Rows.Count = 0 Then
                    result.Rows.Add(title, "-", "-")
                Else
                    Dim ref = Convert.ToString(dt.Rows(0)("ReferenceNumber"))
                    Dim dv = dt.Rows(0)("EntryDate")
                    Dim dateStr = If(dv Is Nothing OrElse Convert.IsDBNull(dv), "-",
                                     PersianDateHelper.ToPersian(Convert.ToDateTime(dv)))
                    result.Rows.Add(title, ref, dateStr)
                End If
            End Sub

            addRow("آخرین سند", Sql.ExecuteTable(
                "SELECT ReferenceNumber, EntryDate FROM AccountingEntries " &
                "WHERE CompanyID = ? AND FiscalYearID = ? ORDER BY CAST(ReferenceNumber AS INTEGER) DESC LIMIT 1",
                cid, fyid))

            addRow("سند قبلی", Sql.ExecuteTable(
                "SELECT ReferenceNumber, EntryDate FROM AccountingEntries " &
                "WHERE CompanyID = ? AND FiscalYearID = ? AND CAST(ReferenceNumber AS INTEGER) < ? ORDER BY CAST(ReferenceNumber AS INTEGER) DESC LIMIT 1",
                cid, fyid, numRef))

            addRow("سند بعدی", Sql.ExecuteTable(
                "SELECT ReferenceNumber, EntryDate FROM AccountingEntries " &
                "WHERE CompanyID = ? AND FiscalYearID = ? AND CAST(ReferenceNumber AS INTEGER) > ? ORDER BY CAST(ReferenceNumber AS INTEGER) ASC LIMIT 1",
                cid, fyid, numRef))

            Return result
        End Function

        Public Function GetEntryById(entryId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT EntryID, ReferenceNumber, EntryDate, Description, VazeiatSanad FROM AccountingEntries WHERE EntryID = ?",
                entryId)
            If dt.Rows.Count = 0 Then Return Nothing
            Return dt.Rows(0)
        End Function

        Public Function GetEntryDetails(entryId As Integer) As DataTable
            ' تلاش اول: با تمام ستون‌های اختیاری
            Try
                Return Sql.ExecuteTable(
                    "SELECT d.DetailID, d.AccountID, a.AccountCode, a.AccountName, d.DebitAmount, d.CreditAmount, " &
                    "d.LineNumber, d.ShenavarID, d.SharhRadif, d.TransactionNumber, d.TransactionDate " &
                    "FROM AccountingEntryDetails AS d LEFT JOIN ChartOfAccounts AS a ON d.AccountID = a.AccountID " &
                    "WHERE d.EntryID = ? ORDER BY d.LineNumber", entryId)
            Catch
            End Try

            ' تلاش دوم: بدون SharhRadif و تاریخ تراکنش (ساختار قدیمی‌تر جدول)
            Try
                Return Sql.ExecuteTable(
                    "SELECT d.DetailID, d.AccountID, a.AccountCode, a.AccountName, d.DebitAmount, d.CreditAmount, " &
                    "d.LineNumber, d.ShenavarID " &
                    "FROM AccountingEntryDetails AS d LEFT JOIN ChartOfAccounts AS a ON d.AccountID = a.AccountID " &
                    "WHERE d.EntryID = ? ORDER BY d.LineNumber", entryId)
            Catch
            End Try

            Return New DataTable()
        End Function

        Public Sub UpdateEntry(entryId As Integer, entryDate As Date, description As String, referenceNumber As String, updatedBy As Integer, lines As IEnumerable(Of AccountingEntryLine), jamBedehkar As Decimal, jamBestankar As Decimal, taeazSanad As String)
            ' اسنپشات وضعیت قبلی برای مقایسه
            Dim oldHeader = GetEntryById(entryId)
            Dim oldDesc = If(oldHeader Is Nothing, "", Convert.ToString(oldHeader("Description")))
            Dim oldRef = If(oldHeader Is Nothing, "", Convert.ToString(oldHeader("ReferenceNumber")))
            Dim oldDetails = GetEntryDetails(entryId)

            ' بروزرسانی سربرگ سند (شامل تاریخ)
            Sql.ExecuteNonQuery(
                "UPDATE AccountingEntries SET EntryDate = ?, Description = ?, ReferenceNumber = ? WHERE EntryID = ?",
                entryDate, description, referenceNumber, entryId)

            ' حذف و درج مجدد ردیف‌ها
            Sql.ExecuteNonQuery("DELETE FROM AccountingEntryDetails WHERE EntryID = ?", entryId)

            For Each line In lines
                Dim debit = Math.Truncate(line.DebitAmount)
                Dim credit = Math.Truncate(line.CreditAmount)
                Sql.ExecuteNonQuery(
                    "INSERT INTO AccountingEntryDetails (EntryID, AccountID, DebitAmount, CreditAmount, LineNumber, ShenavarID, SharhRadif, TransactionNumber, TransactionDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    entryId, line.AccountID, debit, credit, line.LineNumber,
                    If(line.ShenavarID = 0, CType(DBNull.Value, Object), CType(line.ShenavarID, Object)),
                    If(String.IsNullOrEmpty(line.SharhRadif), CType(DBNull.Value, Object), CType(line.SharhRadif, Object)),
                    If(String.IsNullOrEmpty(line.TransactionNumber), CType(DBNull.Value, Object), CType(line.TransactionNumber, Object)),
                    If(String.IsNullOrEmpty(line.TransactionDate), CType(DBNull.Value, Object), CType(line.TransactionDate, Object)))
            Next

            Sql.ExecuteNonQuery(
                "UPDATE AccountingEntries SET JamBedehkar = ?, JamBestankar = ?, TaeazSanad = ?, VazeiatSanad = 'سند موقت - ویرایش شده' WHERE EntryID = ?",
                jamBedehkar, jamBestankar, taeazSanad, entryId)

            ' دریافت ردیف‌های جدید برای مقایسه
            Dim newDetails = GetEntryDetails(entryId)

            ' ثبت تغییرات سربرگ
            If oldDesc <> description Then
                LogEditEntry(entryId, updatedBy, "شرح سند از «" & oldDesc & "» به «" & description & "» تغییر یافت")
            End If
            If oldRef <> referenceNumber Then
                LogEditEntry(entryId, updatedBy, "شماره سند از " & oldRef & " به " & referenceNumber & " تغییر یافت")
            End If

            ' مقایسه ردیف به ردیف و ثبت تغییرات
            Dim maxRows = Math.Max(oldDetails.Rows.Count, newDetails.Rows.Count)
            For i = 0 To maxRows - 1
                Dim rowNum = i + 1
                Dim prefix = "در ردیف " & rowNum & " از سند شماره " & referenceNumber & "، "
                If i >= oldDetails.Rows.Count Then
                    Dim nr = newDetails.Rows(i)
                    LogEditEntry(entryId, updatedBy, prefix & "ردیف جدید با حساب " & nr("AccountCode") & " (" & nr("AccountName") & ") اضافه شد")
                ElseIf i >= newDetails.Rows.Count Then
                    Dim oldRow = oldDetails.Rows(i)
                    LogEditEntry(entryId, updatedBy, prefix & "ردیف با حساب " & oldRow("AccountCode") & " (" & oldRow("AccountName") & ") حذف شد")
                Else
                    Dim oldRow = oldDetails.Rows(i)
                    Dim newRow = newDetails.Rows(i)
                    If Convert.ToInt32(oldRow("AccountID")) <> Convert.ToInt32(newRow("AccountID")) Then
                        LogEditEntry(entryId, updatedBy, prefix & "کد حساب از " & oldRow("AccountCode") & " به " & newRow("AccountCode") & " تغییر یافت")
                    End If
                    Dim oldD = Convert.ToDecimal(If(oldRow("DebitAmount") Is DBNull.Value, 0D, oldRow("DebitAmount")))
                    Dim newD = Convert.ToDecimal(If(newRow("DebitAmount") Is DBNull.Value, 0D, newRow("DebitAmount")))
                    If oldD <> newD Then
                        LogEditEntry(entryId, updatedBy, prefix & "مبلغ بدهکار از " & oldD.ToString("N0") & " به " & newD.ToString("N0") & " تغییر یافت")
                    End If
                    Dim oldC = Convert.ToDecimal(If(oldRow("CreditAmount") Is DBNull.Value, 0D, oldRow("CreditAmount")))
                    Dim newC = Convert.ToDecimal(If(newRow("CreditAmount") Is DBNull.Value, 0D, newRow("CreditAmount")))
                    If oldC <> newC Then
                        LogEditEntry(entryId, updatedBy, prefix & "مبلغ بستانکار از " & oldC.ToString("N0") & " به " & newC.ToString("N0") & " تغییر یافت")
                    End If
                End If
            Next

            logService.LogActivity(updatedBy, "UpdateEntry", "AccountingEntry", entryId,
                                   "ویرایش سند حسابداری: " & referenceNumber, SessionContext.CurrentIP)
        End Sub

        Private Sub LogEditEntry(entryId As Integer, userId As Integer, editDescription As String)
            Sql.ExecuteNonQuery(
                "INSERT INTO SavabegEditSanad1 (EntryID, EditDate, UserID, EditDescription) VALUES (?, ?, ?, ?)",
                entryId, DateTime.Now, userId, editDescription)
        End Sub

        Public Sub SetAdamVirayesh(entryId As Integer, value As Boolean)
            Sql.ExecuteNonQuery("UPDATE AccountingEntries SET AdamVirayesh = ? WHERE EntryID = ?", value, entryId)
        End Sub

        Public Sub SetEntryStatus(entryId As Integer, status As String)
            Sql.ExecuteNonQuery("UPDATE AccountingEntries SET VazeiatSanad = ? WHERE EntryID = ?", status, entryId)
        End Sub

        Public Sub SaveEntry(entryDate As Date, description As String, referenceNumber As String, createdBy As Integer, lines As IEnumerable(Of AccountingEntryLine), jamBedehkar As Decimal, jamBestankar As Decimal, taeazSanad As String)
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Throw New InvalidOperationException("ابتدا باید شرکت و سال مالی جاری را انتخاب کنید.")
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim entryId = Sql.ExecuteIdentity(
                "INSERT INTO AccountingEntries (CompanyID, FiscalYearID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad) VALUES (?, ?, ?, ?, ?, ?, ?)",
                companyId, fyId, entryDate, description, referenceNumber, createdBy, "سند موقت - ثبت اولیه")

            For Each line In lines
                Dim debit = Math.Truncate(line.DebitAmount)
                Dim credit = Math.Truncate(line.CreditAmount)
                Sql.ExecuteNonQuery(
                    "INSERT INTO AccountingEntryDetails (EntryID, AccountID, DebitAmount, CreditAmount, LineNumber, ShenavarID, SharhRadif, TransactionNumber, TransactionDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    entryId, line.AccountID, debit, credit, line.LineNumber,
                    If(line.ShenavarID = 0, CType(DBNull.Value, Object), CType(line.ShenavarID, Object)),
                    If(String.IsNullOrEmpty(line.SharhRadif), CType(DBNull.Value, Object), CType(line.SharhRadif, Object)),
                    If(String.IsNullOrEmpty(line.TransactionNumber), CType(DBNull.Value, Object), CType(line.TransactionNumber, Object)),
                    If(String.IsNullOrEmpty(line.TransactionDate), CType(DBNull.Value, Object), CType(line.TransactionDate, Object)))
            Next

            Sql.ExecuteNonQuery(
                "UPDATE AccountingEntries SET JamBedehkar = ?, JamBestankar = ?, TaeazSanad = ? WHERE EntryID = ?",
                jamBedehkar, jamBestankar, taeazSanad, entryId)

            logService.LogActivity(createdBy, "CreateEntry", "AccountingEntry", entryId,
                                   "ثبت سند حسابداری: " & If(String.IsNullOrWhiteSpace(referenceNumber), "-", referenceNumber),
                                   SessionContext.CurrentIP)
        End Sub

        ' ========================
        ' تراز آزمایشی (فیلتر بر اساس شرکت و سال مالی جاری)
        ' ========================

        Public Sub DebugSanad2()
            Dim outPath = "C:\myproject\Sys_Hes_Anb\debugSanad2.txt"
            Dim lines As New List(Of String)()
            Dim ts = Function() DateTime.Now.ToString("HH:mm:ss.fff")

            lines.Add("===== debugSanad2 " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " =====")
            lines.Add("")

            ' --- اطلاعات Session ---
            lines.Add("[Session]")
            Try
                lines.Add("CurrentCompanyID = " & If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value.ToString(), "NULL"))
                lines.Add("CurrentFiscalYearID = " & If(SessionContext.CurrentFiscalYearID.HasValue, SessionContext.CurrentFiscalYearID.Value.ToString(), "NULL"))
                lines.Add("CurrentUser = " & If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.Username & " / " & SessionContext.CurrentUser.UserType, "NULL"))
            Catch ex As Exception
                lines.Add("Session ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- لیست جداول ---
            lines.Add("[Tables in Database]")
            Try
                Using conn = Db.OpenConnection()
                    Dim tables = conn.GetSchema("Tables")
                    For Each row As DataRow In tables.Rows
                        Dim tType = Convert.ToString(row("TABLE_TYPE"))
                        If tType = "TABLE" Then
                            lines.Add("  " & Convert.ToString(row("TABLE_NAME")))
                        End If
                    Next
                End Using
            Catch ex As Exception
                lines.Add("Tables ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- ستون‌های AccountingEntryDetails ---
            lines.Add("[Columns: AccountingEntryDetails]")
            Try
                Using conn = Db.OpenConnection()
                    Dim cols = conn.GetSchema("Columns", New String() {Nothing, Nothing, "AccountingEntryDetails", Nothing})
                    For Each row As DataRow In cols.Rows
                        lines.Add("  " & Convert.ToString(row("COLUMN_NAME")) & " | " & Convert.ToString(row("DATA_TYPE")) & " | size=" & Convert.ToString(row("CHARACTER_MAXIMUM_LENGTH")))
                    Next
                End Using
            Catch ex As Exception
                lines.Add("Columns(AccountingEntryDetails) ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- ستون‌های AccountingEntries ---
            lines.Add("[Columns: AccountingEntries]")
            Try
                Using conn = Db.OpenConnection()
                    Dim cols = conn.GetSchema("Columns", New String() {Nothing, Nothing, "AccountingEntries", Nothing})
                    For Each row As DataRow In cols.Rows
                        lines.Add("  " & Convert.ToString(row("COLUMN_NAME")) & " | " & Convert.ToString(row("DATA_TYPE")) & " | size=" & Convert.ToString(row("CHARACTER_MAXIMUM_LENGTH")))
                    Next
                End Using
            Catch ex As Exception
                lines.Add("Columns(AccountingEntries) ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- ستون‌های ChartOfAccounts ---
            lines.Add("[Columns: ChartOfAccounts]")
            Try
                Using conn = Db.OpenConnection()
                    Dim cols = conn.GetSchema("Columns", New String() {Nothing, Nothing, "ChartOfAccounts", Nothing})
                    For Each row As DataRow In cols.Rows
                        lines.Add("  " & Convert.ToString(row("COLUMN_NAME")) & " | " & Convert.ToString(row("DATA_TYPE")))
                    Next
                End Using
            Catch ex As Exception
                lines.Add("Columns(ChartOfAccounts) ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- تعداد رکورد جداول ---
            lines.Add("[Row Counts]")
            For Each tbl In New String() {"ChartOfAccounts", "AccountingEntries", "AccountingEntryDetails"}
                Try
                    Dim cnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM " & tbl), 0))
                    lines.Add("  " & tbl & " = " & cnt & " rows")
                Catch ex As Exception
                    lines.Add("  " & tbl & " COUNT ERROR: " & ex.Message)
                End Try
            Next
            lines.Add("")

            ' --- آزمایش کوئری ۱: حسابها ---
            lines.Add("[Query Test 1: ChartOfAccounts simple]")
            Try
                If SessionContext.CurrentCompanyID.HasValue Then
                    Dim dt = Sql.ExecuteTable(
                        "SELECT AccountID, AccountCode, AccountName FROM ChartOfAccounts WHERE CompanyID = ? ORDER BY AccountCode",
                        SessionContext.CurrentCompanyID.Value)
                    lines.Add("  OK - rows=" & dt.Rows.Count)
                Else
                    lines.Add("  SKIPPED (no CompanyID)")
                End If
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- آزمایش کوئری ۲: ساده‌ترین کوئری روی AccountingEntryDetails ---
            lines.Add("[Query Test 2: AccountingEntryDetails SELECT *]")
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM AccountingEntryDetails LIMIT 1")
                lines.Add("  OK - columns=" & dt.Columns.Count)
                For Each col As DataColumn In dt.Columns
                    lines.Add("    col: " & col.ColumnName & " (" & col.DataType.Name & ")")
                Next
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- آزمایش کوئری ۳: DebitAmount / CreditAmount ---
            lines.Add("[Query Test 3: AccountingEntryDetails DebitAmount/CreditAmount]")
            Try
                Dim dt = Sql.ExecuteTable("SELECT DebitAmount, CreditAmount FROM AccountingEntryDetails LIMIT 1")
                lines.Add("  OK - DebitAmount and CreditAmount exist")
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- آزمایش کوئری ۴: INNER JOIN ساده ---
            lines.Add("[Query Test 4: INNER JOIN AccountingEntryDetails + AccountingEntries]")
            Try
                Dim dt = Sql.ExecuteTable(
                    "SELECT d.AccountID, d.DebitAmount, d.CreditAmount " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID")
                lines.Add("  OK - rows=" & dt.Rows.Count)
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- آزمایش کوئری ۵: با WHERE پارامتردار ---
            lines.Add("[Query Test 5: With ? parameters]")
            Try
                If SessionContext.CurrentCompanyID.HasValue AndAlso SessionContext.CurrentFiscalYearID.HasValue Then
                    Dim cid = SessionContext.CurrentCompanyID.Value
                    Dim fid = SessionContext.CurrentFiscalYearID.Value
                    Dim dt = Sql.ExecuteTable(
                        "SELECT d.AccountID, SUM(d.DebitAmount) AS DebitTotal, SUM(d.CreditAmount) AS CreditTotal " &
                        "FROM AccountingEntryDetails AS d " &
                        "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                        "WHERE e.CompanyID = ? AND e.FiscalYearID = ? " &
                        "GROUP BY d.AccountID",
                        cid, fid)
                    lines.Add("  OK - rows=" & dt.Rows.Count)
                Else
                    lines.Add("  SKIPPED (no session)")
                End If
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
            End Try
            lines.Add("")

            ' --- آزمایش کوئری ۶: کامل GetTrialBalance ---
            lines.Add("[Query Test 6: Full GetTrialBalance logic]")
            Try
                Dim result = GetTrialBalance()
                lines.Add("  OK - rows=" & result.Rows.Count)
            Catch ex As Exception
                lines.Add("  ERROR: " & ex.Message)
                lines.Add("  StackTrace: " & ex.StackTrace)
            End Try
            lines.Add("")

            lines.Add("===== END =====")

            Try
                File.WriteAllLines(outPath, lines.ToArray())
            Catch ex As Exception
                ' اگر نوشتن فایل هم ممکن نبود
            End Try
        End Sub

        Public Function GetAccountLedgerGrouped(accountIds As List(Of Integer)) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing OrElse accountIds.Count = 0 Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim sb As New System.Text.StringBuilder()
            For i = 0 To accountIds.Count - 1
                If i > 0 Then sb.Append(",")
                sb.Append(accountIds(i).ToString())
            Next
            Dim inClause = sb.ToString()

            Dim permFilter = ""
            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                permFilter = " AND e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")"
            End If

            Dim query =
                "SELECT e.ReferenceNumber, e.EntryDate, " &
                "IFNULL(e.Description,'') AS SharhRadif, " &
                "SUM(IFNULL(d.DebitAmount,0)) AS DebitAmount, " &
                "SUM(IFNULL(d.CreditAmount,0)) AS CreditAmount " &
                "FROM AccountingEntryDetails AS d " &
                "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                "WHERE d.AccountID IN (" & inClause & ") AND e.CompanyID = ? AND e.FiscalYearID = ?" &
                permFilter & " " &
                "GROUP BY e.ReferenceNumber, e.EntryDate, e.EntryID, e.Description " &
                "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER)"

            Return Sql.ExecuteTable(query, companyId, fyId)
        End Function

        Public Function GetAccountLedger(accountId As Integer) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim query As String
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                query =
                    "SELECT e.ReferenceNumber, d.LineNumber, e.EntryDate, " &
                    "IFNULL(d.SharhRadif,'') AS SharhRadif, " &
                    "IFNULL(d.DebitAmount,0) AS DebitAmount, " &
                    "IFNULL(d.CreditAmount,0) AS CreditAmount " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE d.AccountID = ? AND e.CompanyID = ? AND e.FiscalYearID = ? " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER), d.LineNumber"
            Else
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                Dim idClause = ActivityLogService.BuildIDInClause(visibleIds)
                query =
                    "SELECT e.ReferenceNumber, d.LineNumber, e.EntryDate, " &
                    "IFNULL(d.SharhRadif,'') AS SharhRadif, " &
                    "IFNULL(d.DebitAmount,0) AS DebitAmount, " &
                    "IFNULL(d.CreditAmount,0) AS CreditAmount " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE d.AccountID = ? AND e.CompanyID = ? AND e.FiscalYearID = ? " &
                    "AND e.CreatedBy IN (" & idClause & ") " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER), d.LineNumber"
            End If

            Return Sql.ExecuteTable(query, accountId, companyId, fyId)
        End Function

        Public Function GetLedgerData(
            accountIds As List(Of Integer),
            aggregate As Boolean,
            Optional fromDateStr As String = Nothing,
            Optional toDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional toDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing OrElse accountIds.Count = 0 Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim sb As New System.Text.StringBuilder()
            For i = 0 To accountIds.Count - 1
                If i > 0 Then sb.Append(",")
                sb.Append(accountIds(i).ToString())
            Next
            Dim inClause = sb.ToString()

            ' ساخت فیلترهای پویا
            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()

            filters.Add("d.AccountID IN (" & inClause & ")")
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)
            filters.Add("e.FiscalYearID = ?")
            params.Add(fyId)

            ' فیلتر وضعیت
            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    filters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    params.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    filters.Add("e.VazeiatSanad LIKE ?")
                    params.Add("%دائم%")
                Else
                    filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            ' فیلتر دسترسی کاربر
            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                filters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            ' فیلتر تاریخ
            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If
            If Not String.IsNullOrEmpty(toDateStr) Then
                toDate = PersianDateHelper.ParsePersianDate(toDateStr)
            End If

            ' جابجایی خودکار تاریخ‌ها در صورت لزوم
            If fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate.Value > toDate.Value Then
                Dim tempDate = fromDate
                fromDate = toDate
                toDate = tempDate
            End If

            If fromDate.HasValue Then
                filters.Add("e.EntryDate >= ?")
                params.Add(fromDate.Value)
            End If
            If toDate.HasValue Then
                filters.Add("e.EntryDate <= ?")
                params.Add(toDate.Value)
            End If

            ' فیلتر شماره سند
            If fromDoc.HasValue AndAlso toDoc.HasValue AndAlso fromDoc.Value > toDoc.Value Then
                Dim tempDoc = fromDoc
                fromDoc = toDoc
                toDoc = tempDoc
            End If

            If fromDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) >= ?")
                params.Add(fromDoc.Value)
            End If
            If toDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                params.Add(toDoc.Value)
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())

            Dim query As String
            If aggregate Then
                query =
                    "SELECT e.EntryID, e.ReferenceNumber, e.EntryDate, " &
                    "'' AS SharhRadif, " &
                    "IFNULL(e.Description,'') AS Description, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitAmount, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditAmount " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    whereClause & " " &
                    "GROUP BY e.EntryID, e.ReferenceNumber, e.EntryDate, e.Description " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER)"
            Else
                query =
                    "SELECT e.EntryID, e.ReferenceNumber, d.LineNumber, e.EntryDate, " &
                    "IFNULL(d.SharhRadif,'') AS SharhRadif, " &
                    "IFNULL(e.Description,'') AS Description, " &
                    "IFNULL(d.DebitAmount,0) AS DebitAmount, " &
                    "IFNULL(d.CreditAmount,0) AS CreditAmount " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    whereClause & " " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER), d.LineNumber"
            End If

            Return Sql.ExecuteTable(query, params.ToArray())
        End Function


        Public Function GetAllAccountsWithDirectTotals(
            Optional fromDateStr As String = Nothing,
            Optional toDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional toDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim accounts = Sql.ExecuteTable(
                "SELECT AccountID, AccountCode, AccountName, ParentAccountID, AccountNature FROM ChartOfAccounts " &
                "WHERE CompanyID = ? ORDER BY AccountCode",
                companyId)

            ' 1. Build base filters (shared by both Before and During queries)
            Dim baseFilters As New List(Of String)()
            Dim baseParams As New List(Of Object)()

            baseFilters.Add("e.CompanyID = ?")
            baseParams.Add(companyId)

            baseFilters.Add("e.FiscalYearID = ?")
            baseParams.Add(fyId)

            ' Exclude soft-deleted by default, but if user explicitly filters for a status, handle that
            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    baseFilters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    baseParams.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    baseFilters.Add("e.VazeiatSanad LIKE ?")
                    baseParams.Add("%دائم%")
                Else
                    baseFilters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                baseFilters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                baseFilters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            ' 2. Parse date/doc filter boundaries
            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If

            If Not String.IsNullOrEmpty(toDateStr) Then
                toDate = PersianDateHelper.ParsePersianDate(toDateStr)
            End If

            ' Auto-swap if From Date is greater than To Date
            If fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate.Value > toDate.Value Then
                Dim tempDate = fromDate
                fromDate = toDate
                toDate = tempDate
            End If

            ' Auto-swap if From Doc is greater than To Doc
            If fromDoc.HasValue AndAlso toDoc.HasValue AndAlso fromDoc.Value > toDoc.Value Then
                Dim tempDoc = fromDoc
                fromDoc = toDoc
                toDoc = tempDoc
            End If

            ' 3. Build Before query (prior history)
            Dim beforeFilters As New List(Of String)(baseFilters)
            Dim beforeParams As New List(Of Object)(baseParams)
            Dim hasBeforeCondition As Boolean = False

            If fromDate.HasValue AndAlso fromDoc.HasValue Then
                beforeFilters.Add("(e.EntryDate < ? OR (e.EntryDate = ? AND CAST(e.ReferenceNumber AS INTEGER) < ?))")
                beforeParams.Add(fromDate.Value)
                beforeParams.Add(fromDate.Value)
                beforeParams.Add(fromDoc.Value)
                hasBeforeCondition = True
            ElseIf fromDate.HasValue Then
                beforeFilters.Add("e.EntryDate < ?")
                beforeParams.Add(fromDate.Value)
                hasBeforeCondition = True
            ElseIf fromDoc.HasValue Then
                beforeFilters.Add("CAST(e.ReferenceNumber AS INTEGER) < ?")
                beforeParams.Add(fromDoc.Value)
                hasBeforeCondition = True
            End If

            Dim beforeSums As New Dictionary(Of Integer, Tuple(Of Decimal, Decimal))()

            If hasBeforeCondition Then
                Dim beforeFilterString = String.Join(" AND ", beforeFilters)
                Dim beforeSumsQuery =
                    "SELECT d.AccountID, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE " & beforeFilterString & " " &
                    "GROUP BY d.AccountID"
                Try
                    Dim beforeSumsTable = Sql.ExecuteTable(beforeSumsQuery, beforeParams.ToArray())
                    For Each row As DataRow In beforeSumsTable.Rows
                        beforeSums(Convert.ToInt32(row("AccountID"))) = Tuple.Create(
                            Convert.ToDecimal(row("DebitTotal")),
                            Convert.ToDecimal(row("CreditTotal"))
                        )
                    Next
                Catch ex As Exception
                    ' Log or handle query failure
                End Try
            End If

            ' 4. Build During query (period transactions)
            Dim duringFilters As New List(Of String)(baseFilters)
            Dim duringParams As New List(Of Object)(baseParams)

            If fromDate.HasValue Then
                duringFilters.Add("e.EntryDate >= ?")
                duringParams.Add(fromDate.Value)
            End If

            If toDate.HasValue Then
                duringFilters.Add("e.EntryDate <= ?")
                duringParams.Add(toDate.Value)
            End If

            If fromDoc.HasValue Then
                duringFilters.Add("CAST(e.ReferenceNumber AS INTEGER) >= ?")
                duringParams.Add(fromDoc.Value)
            End If

            If toDoc.HasValue Then
                duringFilters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                duringParams.Add(toDoc.Value)
            End If

            Dim duringFilterString = String.Join(" AND ", duringFilters)
            Dim duringSumsQuery =
                "SELECT d.AccountID, " &
                "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                "FROM AccountingEntryDetails AS d " &
                "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                "WHERE " & duringFilterString & " " &
                "GROUP BY d.AccountID"

            Dim duringSums = Sql.ExecuteTable(duringSumsQuery, duringParams.ToArray())

            Dim duringSumsLookup As New Dictionary(Of Integer, DataRow)()
            For Each row As DataRow In duringSums.Rows
                duringSumsLookup(Convert.ToInt32(row("AccountID"))) = row
            Next

            ' 5. Compile and merge results
            Dim result As New DataTable()
            result.Columns.Add("AccountID", GetType(Integer))
            result.Columns.Add("AccountCode", GetType(String))
            result.Columns.Add("AccountName", GetType(String))
            result.Columns.Add("ParentAccountID", GetType(Integer))
            result.Columns.Add("DebitBeforeDirect", GetType(Decimal))
            result.Columns.Add("CreditBeforeDirect", GetType(Decimal))
            result.Columns.Add("DebitDuringDirect", GetType(Decimal))
            result.Columns.Add("CreditDuringDirect", GetType(Decimal))
            result.Columns.Add("AccountNature", GetType(String))

            For Each acctRow As DataRow In accounts.Rows
                Dim acctId = Convert.ToInt32(acctRow("AccountID"))
                Dim dr = result.NewRow()
                dr("AccountID") = acctId
                dr("AccountCode") = acctRow("AccountCode")
                dr("AccountName") = acctRow("AccountName")
                dr("ParentAccountID") = If(acctRow.IsNull("ParentAccountID"), DBNull.Value, acctRow("ParentAccountID"))
                dr("AccountNature") = Convert.ToString(acctRow("AccountNature"))
                
                If beforeSums.ContainsKey(acctId) Then
                    dr("DebitBeforeDirect") = beforeSums(acctId).Item1
                    dr("CreditBeforeDirect") = beforeSums(acctId).Item2
                Else
                    dr("DebitBeforeDirect") = 0D
                    dr("CreditBeforeDirect") = 0D
                End If

                If duringSumsLookup.ContainsKey(acctId) Then
                    dr("DebitDuringDirect") = Convert.ToDecimal(duringSumsLookup(acctId)("DebitTotal"))
                    dr("CreditDuringDirect") = Convert.ToDecimal(duringSumsLookup(acctId)("CreditTotal"))
                Else
                    dr("DebitDuringDirect") = 0D
                    dr("CreditDuringDirect") = 0D
                End If
                result.Rows.Add(dr)
            Next

            Return result
        End Function

        Public Function GetTrialBalance() As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            ' کوئری ۱: همه حسابهای شرکت
            Dim accounts = Sql.ExecuteTable(
                "SELECT AccountID, AccountCode, AccountName FROM ChartOfAccounts " &
                "WHERE CompanyID = ? ORDER BY AccountCode",
                companyId)

            ' کوئری ۲: جمع بدهکار/بستانکار هر حساب در سال مالی جاری
            Dim sumsQuery As String
            If String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                sumsQuery =
                    "SELECT d.AccountID, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE e.CompanyID = ? AND e.FiscalYearID = ? " &
                    "GROUP BY d.AccountID"
            Else
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                Dim idClause = ActivityLogService.BuildIDInClause(visibleIds)
                sumsQuery =
                    "SELECT d.AccountID, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE e.CompanyID = ? AND e.FiscalYearID = ? AND e.CreatedBy IN (" & idClause & ") " &
                    "GROUP BY d.AccountID"
            End If
            Dim sums = Sql.ExecuteTable(sumsQuery, companyId, fyId)

            ' ساخت lookup از AccountID به مقادیر
            Dim sumsLookup As New Dictionary(Of Integer, DataRow)()
            For Each row As DataRow In sums.Rows
                sumsLookup(Convert.ToInt32(row("AccountID"))) = row
            Next

            ' ساخت جدول نتیجه و ادغام
            Dim result As New DataTable()
            result.Columns.Add("AccountCode", GetType(String))
            result.Columns.Add("AccountName", GetType(String))
            result.Columns.Add("DebitTotal", GetType(Decimal))
            result.Columns.Add("CreditTotal", GetType(Decimal))

            For Each acctRow As DataRow In accounts.Rows
                Dim acctId = Convert.ToInt32(acctRow("AccountID"))
                Dim dr = result.NewRow()
                dr("AccountCode") = acctRow("AccountCode")
                dr("AccountName") = acctRow("AccountName")
                If sumsLookup.ContainsKey(acctId) Then
                    dr("DebitTotal") = Convert.ToDecimal(sumsLookup(acctId)("DebitTotal"))
                    dr("CreditTotal") = Convert.ToDecimal(sumsLookup(acctId)("CreditTotal"))
                Else
                    dr("DebitTotal") = 0D
                    dr("CreditTotal") = 0D
                End If
                result.Rows.Add(dr)
            Next

            Return result
        End Function

        Public Function GetLedgerBeforeSums(
            accountIds As List(Of Integer),
            Optional fromDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As Tuple(Of Decimal, Decimal)
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return Tuple.Create(0D, 0D)
            End If
            If SessionContext.CurrentUser Is Nothing OrElse accountIds.Count = 0 Then Return Tuple.Create(0D, 0D)

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim sb As New System.Text.StringBuilder()
            For i = 0 To accountIds.Count - 1
                If i > 0 Then sb.Append(",")
                sb.Append(accountIds(i).ToString())
            Next
            Dim inClause = sb.ToString()

            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()

            filters.Add("d.AccountID IN (" & inClause & ")")
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)
            filters.Add("e.FiscalYearID = ?")
            params.Add(fyId)

            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    filters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    params.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    filters.Add("e.VazeiatSanad LIKE ?")
                    params.Add("%دائم%")
                Else
                    filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                filters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            Dim fromDate As DateTime? = Nothing
            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If

            Dim hasBeforeCondition = False
            If fromDate.HasValue AndAlso fromDoc.HasValue Then
                filters.Add("(e.EntryDate < ? OR (e.EntryDate = ? AND CAST(e.ReferenceNumber AS INTEGER) < ?))")
                params.Add(fromDate.Value)
                params.Add(fromDate.Value)
                params.Add(fromDoc.Value)
                hasBeforeCondition = True
            ElseIf fromDate.HasValue Then
                filters.Add("e.EntryDate < ?")
                params.Add(fromDate.Value)
                hasBeforeCondition = True
            ElseIf fromDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) < ?")
                params.Add(fromDoc.Value)
                hasBeforeCondition = True
            End If

            If Not hasBeforeCondition Then
                Return Tuple.Create(0D, 0D)
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())
            Dim query = "SELECT " &
                        "SUM(IFNULL(d.DebitAmount, 0)) AS DebitTotal, " &
                        "SUM(IFNULL(d.CreditAmount, 0)) AS CreditTotal " &
                        "FROM AccountingEntryDetails AS d " &
                        "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                        whereClause

            Try
                Dim dt = Sql.ExecuteTable(query, params.ToArray())
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    Dim deb = If(row.IsNull("DebitTotal"), 0D, Convert.ToDecimal(row("DebitTotal")))
                    Dim cred = If(row.IsNull("CreditTotal"), 0D, Convert.ToDecimal(row("CreditTotal")))
                    Return Tuple.Create(deb, cred)
                End If
            Catch
            End Try

            Return Tuple.Create(0D, 0D)
        End Function

        Public Function GetAllShenavarsWithDirectTotals(
            Optional fromDateStr As String = Nothing,
            Optional toDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional toDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim shenavars = Sql.ExecuteTable(
                "SELECT s.ShenavarID AS AccountID, s.AccountCode, s.AccountName, s.ParentShenavarID AS ParentAccountID, 'Bedehkar' AS AccountNature, " &
                "(SELECT a.AccountCode FROM AccountingEntryDetails d INNER JOIN ChartOfAccounts a ON d.AccountID = a.AccountID WHERE d.ShenavarID = s.ShenavarID LIMIT 1) AS StandardAccountCode, " &
                "(SELECT a.AccountName FROM AccountingEntryDetails d INNER JOIN ChartOfAccounts a ON d.AccountID = a.AccountID WHERE d.ShenavarID = s.ShenavarID LIMIT 1) AS StandardAccountName " &
                "FROM shenavar s " &
                "WHERE s.CompanyID = ? ORDER BY s.AccountCode",
                companyId)

            ' 1. Build base filters
            Dim baseFilters As New List(Of String)()
            Dim baseParams As New List(Of Object)()

            baseFilters.Add("e.CompanyID = ?")
            baseParams.Add(companyId)

            baseFilters.Add("e.FiscalYearID = ?")
            baseParams.Add(fyId)

            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    baseFilters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    baseParams.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    baseFilters.Add("e.VazeiatSanad LIKE ?")
                    baseParams.Add("%دائم%")
                Else
                    baseFilters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                baseFilters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                baseFilters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            ' 2. Parse date/doc filter boundaries
            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If

            If Not String.IsNullOrEmpty(toDateStr) Then
                toDate = PersianDateHelper.ParsePersianDate(toDateStr)
            End If

            If fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate.Value > toDate.Value Then
                Dim tempDate = fromDate
                fromDate = toDate
                toDate = tempDate
            End If

            If fromDoc.HasValue AndAlso toDoc.HasValue AndAlso fromDoc.Value > toDoc.Value Then
                Dim tempDoc = fromDoc
                fromDoc = toDoc
                toDoc = tempDoc
            End If

            ' 3. Build Before query
            Dim beforeFilters As New List(Of String)(baseFilters)
            Dim beforeParams As New List(Of Object)(baseParams)
            Dim hasBeforeCondition As Boolean = False

            If fromDate.HasValue AndAlso fromDoc.HasValue Then
                beforeFilters.Add("(e.EntryDate < ? OR (e.EntryDate = ? AND CAST(e.ReferenceNumber AS INTEGER) < ?))")
                beforeParams.Add(fromDate.Value)
                beforeParams.Add(fromDate.Value)
                beforeParams.Add(fromDoc.Value)
                hasBeforeCondition = True
            ElseIf fromDate.HasValue Then
                beforeFilters.Add("e.EntryDate < ?")
                beforeParams.Add(fromDate.Value)
                hasBeforeCondition = True
            ElseIf fromDoc.HasValue Then
                beforeFilters.Add("CAST(e.ReferenceNumber AS INTEGER) < ?")
                beforeParams.Add(fromDoc.Value)
                hasBeforeCondition = True
            End If

            Dim beforeSums As New Dictionary(Of Integer, Tuple(Of Decimal, Decimal))()

            If hasBeforeCondition Then
                Dim beforeFilterString = String.Join(" AND ", beforeFilters)
                Dim beforeSumsQuery =
                    "SELECT d.ShenavarID, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "WHERE " & beforeFilterString & " AND d.ShenavarID IS NOT NULL " &
                    "GROUP BY d.ShenavarID"
                Try
                    Dim beforeSumsTable = Sql.ExecuteTable(beforeSumsQuery, beforeParams.ToArray())
                    For Each row As DataRow In beforeSumsTable.Rows
                        beforeSums(Convert.ToInt32(row("ShenavarID"))) = Tuple.Create(
                            Convert.ToDecimal(row("DebitTotal")),
                            Convert.ToDecimal(row("CreditTotal"))
                        )
                    Next
                Catch ex As Exception
                End Try
            End If

            ' 4. Build During query
            Dim duringFilters As New List(Of String)(baseFilters)
            Dim duringParams As New List(Of Object)(baseParams)

            If fromDate.HasValue Then
                duringFilters.Add("e.EntryDate >= ?")
                duringParams.Add(fromDate.Value)
            End If

            If toDate.HasValue Then
                duringFilters.Add("e.EntryDate <= ?")
                duringParams.Add(toDate.Value)
            End If

            If fromDoc.HasValue Then
                duringFilters.Add("CAST(e.ReferenceNumber AS INTEGER) >= ?")
                duringParams.Add(fromDoc.Value)
            End If

            If toDoc.HasValue Then
                duringFilters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                duringParams.Add(toDoc.Value)
            End If

            Dim duringFilterString = String.Join(" AND ", duringFilters)
            Dim duringSumsQuery =
                "SELECT d.ShenavarID, " &
                "SUM(IFNULL(d.DebitAmount,0)) AS DebitTotal, " &
                "SUM(IFNULL(d.CreditAmount,0)) AS CreditTotal " &
                "FROM AccountingEntryDetails AS d " &
                "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                "WHERE " & duringFilterString & " AND d.ShenavarID IS NOT NULL " &
                "GROUP BY d.ShenavarID"

            Dim duringSums = Sql.ExecuteTable(duringSumsQuery, duringParams.ToArray())

            Dim duringSumsLookup As New Dictionary(Of Integer, DataRow)()
            For Each row As DataRow In duringSums.Rows
                duringSumsLookup(Convert.ToInt32(row("ShenavarID"))) = row
            Next

            ' 5. Compile and merge results
            Dim result As New DataTable()
            result.Columns.Add("AccountID", GetType(Integer))
            result.Columns.Add("AccountCode", GetType(String))
            result.Columns.Add("AccountName", GetType(String))
            result.Columns.Add("ParentAccountID", GetType(Integer))
            result.Columns.Add("AccountNature", GetType(String))
            result.Columns.Add("DebitBeforeDirect", GetType(Decimal))
            result.Columns.Add("CreditBeforeDirect", GetType(Decimal))
            result.Columns.Add("DebitDuringDirect", GetType(Decimal))
            result.Columns.Add("CreditDuringDirect", GetType(Decimal))
            result.Columns.Add("StandardAccountCode", GetType(String))
            result.Columns.Add("StandardAccountName", GetType(String))

            For Each sRow As DataRow In shenavars.Rows
                Dim sId = Convert.ToInt32(sRow("AccountID"))
                Dim code = Convert.ToString(sRow("AccountCode"))
                Dim name = Convert.ToString(sRow("AccountName"))
                Dim parentId = If(sRow.IsNull("ParentAccountID"), CType(Nothing, Integer?), CType(Convert.ToInt32(sRow("ParentAccountID")), Integer?))
                Dim nature = Convert.ToString(sRow("AccountNature"))
                Dim stdCode = Convert.ToString(sRow("StandardAccountCode"))
                Dim stdName = Convert.ToString(sRow("StandardAccountName"))

                Dim debBefore = 0D
                Dim credBefore = 0D
                Dim debDuring = 0D
                Dim credDuring = 0D

                If beforeSums.ContainsKey(sId) Then
                    debBefore = beforeSums(sId).Item1
                    credBefore = beforeSums(sId).Item2
                End If

                If duringSumsLookup.ContainsKey(sId) Then
                    Dim duringRow = duringSumsLookup(sId)
                    debDuring = Convert.ToDecimal(duringRow("DebitTotal"))
                    credDuring = Convert.ToDecimal(duringRow("CreditTotal"))
                End If

                result.Rows.Add(sId, code, name, parentId, nature, debBefore, credBefore, debDuring, credDuring, stdCode, stdName)
            Next

            Return result
        End Function

        Public Function GetShenavarLedgerData(
            shenavarId As Integer,
            aggregate As Boolean,
            Optional fromDateStr As String = Nothing,
            Optional toDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional toDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return New DataTable()
            End If
            If SessionContext.CurrentUser Is Nothing Then Return New DataTable()

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            ' ساخت فیلترهای پویا
            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()

            filters.Add("d.ShenavarID = ?")
            params.Add(shenavarId)
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)
            filters.Add("e.FiscalYearID = ?")
            params.Add(fyId)

            ' فیلتر وضعیت
            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    filters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    params.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    filters.Add("e.VazeiatSanad LIKE ?")
                    params.Add("%دائم%")
                Else
                    filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            ' فیلتر دسترسی کاربر
            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                filters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            ' فیلتر تاریخ
            Dim fromDate As DateTime? = Nothing
            Dim toDate As DateTime? = Nothing

            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If
            If Not String.IsNullOrEmpty(toDateStr) Then
                toDate = PersianDateHelper.ParsePersianDate(toDateStr)
            End If

            If fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate.Value > toDate.Value Then
                Dim tempDate = fromDate
                fromDate = toDate
                toDate = tempDate
            End If

            If fromDate.HasValue Then
                filters.Add("e.EntryDate >= ?")
                params.Add(fromDate.Value)
            End If
            If toDate.HasValue Then
                filters.Add("e.EntryDate <= ?")
                params.Add(toDate.Value)
            End If

            ' فیلتر شماره سند
            If fromDoc.HasValue AndAlso toDoc.HasValue AndAlso fromDoc.Value > toDoc.Value Then
                Dim tempDoc = fromDoc
                fromDoc = toDoc
                toDoc = tempDoc
            End If

            If fromDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) >= ?")
                params.Add(fromDoc.Value)
            End If
            If toDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) <= ?")
                params.Add(toDoc.Value)
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())

            Dim query As String
            If aggregate Then
                query =
                    "SELECT e.EntryID, e.ReferenceNumber, e.EntryDate, " &
                    "'' AS SharhRadif, " &
                    "IFNULL(e.Description,'') AS Description, " &
                    "SUM(IFNULL(d.DebitAmount,0)) AS DebitAmount, " &
                    "SUM(IFNULL(d.CreditAmount,0)) AS CreditAmount, " &
                    "'' AS AccountCode, '' AS AccountName, " &
                    "0 AS StandardAccountID " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    whereClause & " " &
                    "GROUP BY e.EntryID, e.ReferenceNumber, e.EntryDate, e.Description " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER)"
            Else
                query =
                    "SELECT e.EntryID, e.ReferenceNumber, d.LineNumber, e.EntryDate, " &
                    "IFNULL(d.SharhRadif,'') AS SharhRadif, " &
                    "IFNULL(e.Description,'') AS Description, " &
                    "IFNULL(d.DebitAmount,0) AS DebitAmount, " &
                    "IFNULL(d.CreditAmount,0) AS CreditAmount, " &
                    "IFNULL(a.AccountCode,'') AS AccountCode, " &
                    "IFNULL(a.AccountName,'') AS AccountName, " &
                    "IFNULL(d.AccountID, 0) AS StandardAccountID " &
                    "FROM AccountingEntryDetails AS d " &
                    "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                    "LEFT JOIN ChartOfAccounts AS a ON d.AccountID = a.AccountID " &
                    whereClause & " " &
                    "ORDER BY e.EntryDate, CAST(e.ReferenceNumber AS INTEGER), d.LineNumber"
            End If

            Return Sql.ExecuteTable(query, params.ToArray())
        End Function

        Public Function GetShenavarLedgerBeforeSums(
            shenavarId As Integer,
            Optional fromDateStr As String = Nothing,
            Optional fromDoc As Integer? = Nothing,
            Optional docStatus As String = Nothing
        ) As Tuple(Of Decimal, Decimal)
            If Not SessionContext.CurrentCompanyID.HasValue OrElse Not SessionContext.CurrentFiscalYearID.HasValue Then
                Return Tuple.Create(0D, 0D)
            End If
            If SessionContext.CurrentUser Is Nothing Then Return Tuple.Create(0D, 0D)

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim fyId = SessionContext.CurrentFiscalYearID.Value

            Dim filters As New List(Of String)()
            Dim params As New List(Of Object)()

            filters.Add("d.ShenavarID = ?")
            params.Add(shenavarId)
            filters.Add("e.CompanyID = ?")
            params.Add(companyId)
            filters.Add("e.FiscalYearID = ?")
            params.Add(fyId)

            If Not String.IsNullOrEmpty(docStatus) Then
                If docStatus = "موقت" Then
                    filters.Add("(e.VazeiatSanad LIKE ? OR e.VazeiatSanad IS NULL) AND e.VazeiatSanad <> 'سند موقت - حذف موقت'")
                    params.Add("%موقت%")
                ElseIf docStatus = "دائم" Then
                    filters.Add("e.VazeiatSanad LIKE ?")
                    params.Add("%دائم%")
                Else
                    filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
                End If
            Else
                filters.Add("(e.VazeiatSanad <> 'سند موقت - حذف موقت' OR e.VazeiatSanad IS NULL)")
            End If

            If Not String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                Dim visibleIds = ActivityLogService.GetVisibleUserIDs(SessionContext.CurrentUser.UserID, SessionContext.CurrentUser.UserType)
                filters.Add("e.CreatedBy IN (" & ActivityLogService.BuildIDInClause(visibleIds) & ")")
            End If

            Dim fromDate As DateTime? = Nothing
            If Not String.IsNullOrEmpty(fromDateStr) Then
                fromDate = PersianDateHelper.ParsePersianDate(fromDateStr)
            End If

            Dim hasBeforeCondition = False
            If fromDate.HasValue AndAlso fromDoc.HasValue Then
                filters.Add("(e.EntryDate < ? OR (e.EntryDate = ? AND CAST(e.ReferenceNumber AS INTEGER) < ?))")
                params.Add(fromDate.Value)
                params.Add(fromDate.Value)
                params.Add(fromDoc.Value)
                hasBeforeCondition = True
            ElseIf fromDate.HasValue Then
                filters.Add("e.EntryDate < ?")
                params.Add(fromDate.Value)
                hasBeforeCondition = True
            ElseIf fromDoc.HasValue Then
                filters.Add("CAST(e.ReferenceNumber AS INTEGER) < ?")
                params.Add(fromDoc.Value)
                hasBeforeCondition = True
            End If

            If Not hasBeforeCondition Then
                Return Tuple.Create(0D, 0D)
            End If

            Dim whereClause = "WHERE " & String.Join(" AND ", filters.ToArray())
            Dim query = "SELECT " &
                        "SUM(IFNULL(d.DebitAmount, 0)) AS DebitTotal, " &
                        "SUM(IFNULL(d.CreditAmount, 0)) AS CreditTotal " &
                        "FROM AccountingEntryDetails AS d " &
                        "INNER JOIN AccountingEntries AS e ON d.EntryID = e.EntryID " &
                        whereClause

            Try
                Dim dt = Sql.ExecuteTable(query, params.ToArray())
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    Dim deb = If(row.IsNull("DebitTotal"), 0D, Convert.ToDecimal(row("DebitTotal")))
                    Dim cred = If(row.IsNull("CreditTotal"), 0D, Convert.ToDecimal(row("CreditTotal")))
                    Return Tuple.Create(deb, cred)
                End If
            Catch
            End Try

            Return Tuple.Create(0D, 0D)
        End Function

        Public Function GetShenavarHierarchyChain(shenavarId As Integer) As List(Of Tuple(Of String, String))
            Dim chain As New List(Of Tuple(Of String, String))()
            Dim currentId As Integer? = shenavarId
            Dim guard = 0
            Do While currentId.HasValue AndAlso guard < 50
                guard += 1
                Dim dt = Sql.ExecuteTable(
                    "SELECT AccountCode, AccountName, ParentShenavarID FROM shenavar WHERE ShenavarID = ?", currentId.Value)
                If dt.Rows.Count = 0 Then Exit Do
                Dim r = dt.Rows(0)
                Dim code = Convert.ToString(r("AccountCode"))
                Dim name = Convert.ToString(r("AccountName"))
                chain.Insert(0, Tuple.Create(code, name))
                
                Dim pVal = r("ParentShenavarID")
                If pVal Is Nothing OrElse Convert.IsDBNull(pVal) Then
                    currentId = Nothing
                Else
                    currentId = Convert.ToInt32(pVal)
                End If
            Loop
            Return chain
        End Function
    End Class
End Namespace
