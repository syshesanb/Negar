Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class BudgetingService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. BudgetItems
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS BudgetItems (" &
                    "BudgetItemID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CostCenter TEXT, " &
                    "MoeinCode TEXT, " &
                    "ItemTitle TEXT, " &
                    "AllocatedBudget REAL DEFAULT 0, " &
                    "UsedBudget REAL DEFAULT 0, " &
                    "FiscalYear TEXT, " &
                    "Status TEXT DEFAULT 'فعال', " & ' 'فعال', 'قفل شده'
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM BudgetItems"), 0))
                If count = 0 Then
                    Dim yr = SessionContext.CurrentFiscalYearName
                    If String.IsNullOrWhiteSpace(yr) Then yr = "1405"

                    Sql.ExecuteNonQuery(
                        "INSERT INTO BudgetItems (CompanyID, CostCenter, MoeinCode, ItemTitle, AllocatedBudget, UsedBudget, FiscalYear, Status, Notes) " &
                        "VALUES (1, 'دایره بازاریابی و تبلیغات', '601', 'بودجه تبلیغات و نمایشگاه‌های سالانه', 500000000, 180000000, ?, 'فعال', 'بودجه مصوب هیئت مدیره')",
                        yr
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO BudgetItems (CompanyID, CostCenter, MoeinCode, ItemTitle, AllocatedBudget, UsedBudget, FiscalYear, Status, Notes) " &
                        "VALUES (1, 'واحد فناوری اطلاعات (IT)', '602', 'بودجه توسعه نرم‌افزار و تجهیزات سرور', 350000000, 290000000, ?, 'فعال', 'پشتیبانی IT و خرید تجهیزات')",
                        yr
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO BudgetItems (CompanyID, CostCenter, MoeinCode, ItemTitle, AllocatedBudget, UsedBudget, FiscalYear, Status, Notes) " &
                        "VALUES (1, 'واحد پشتیبانی و اداری', '603', 'بودجه ملزومات مصرفی و پذیرایی اداری', 120000000, 115000000, ?, 'فعال', 'هزینه‌های جاری دفتر')",
                        yr
                    )
                End If

                ' 2. BudgetAmendments
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS BudgetAmendments (" &
                    "AmendmentID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "BudgetItemID INTEGER, " &
                    "AmendmentType TEXT DEFAULT 'افزایش بودجه', " & ' 'افزایش بودجه', 'کاهش بودجه', 'جابجایی اعتبار'
                    "Amount REAL DEFAULT 0, " &
                    "Description TEXT, " &
                    "ApprovedBy TEXT DEFAULT 'مدیر مالی', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' 3. BudgetLogs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS BudgetLogs (" &
                    "LogID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "BudgetItemID INTEGER, " &
                    "SourceModule TEXT DEFAULT 'حسابداری', " & ' 'حسابداری', 'انبار', 'حقوق', 'خزانه‌داری', 'اموال'
                    "ExpenseAmount REAL DEFAULT 0, " &
                    "Description TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetBudgetItems(companyID As Integer) As DataTable
            Dim query = "SELECT BudgetItemID, CostCenter, MoeinCode, ItemTitle, AllocatedBudget, UsedBudget, " &
                        "(AllocatedBudget - UsedBudget) AS RemainingBudget, " &
                        "ROUND((UsedBudget / NULLIF(AllocatedBudget, 0)) * 100, 1) || '%' AS UsagePercentStr, " &
                        "CASE " &
                        "  WHEN UsedBudget >= AllocatedBudget THEN '🔴 عبور از سقف بودجه' " &
                        "  WHEN UsedBudget >= (AllocatedBudget * 0.8) THEN '🟡 هشدار زرد (بالای ۸۰٪)' " &
                        "  ELSE '🟢 مطلوب (زیر ۸۰٪)' " &
                        "END AS EnforcementStatus, " &
                        "FiscalYear, Status, Notes " &
                        "FROM BudgetItems WHERE CompanyID = ? ORDER BY BudgetItemID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetBudgetItemById(id As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM BudgetItems WHERE BudgetItemID = ?", id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveBudgetItem(id As Integer, companyID As Integer, costCenter As String, moeinCode As String, title As String, allocated As Double, fiscalYear As String, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO BudgetItems (CompanyID, CostCenter, MoeinCode, ItemTitle, AllocatedBudget, UsedBudget, FiscalYear, Status, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, 0, ?, 'فعال', ?)",
                    companyID, costCenter, moeinCode, title, allocated, fiscalYear, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE BudgetItems SET CostCenter = ?, MoeinCode = ?, ItemTitle = ?, AllocatedBudget = ?, FiscalYear = ?, Notes = ? " &
                    "WHERE BudgetItemID = ? AND CompanyID = ?",
                    costCenter, moeinCode, title, allocated, fiscalYear, notes, id, companyID
                )
            End If
        End Sub

        Public Function AddAmendment(itemId As Integer, companyID As Integer, typeStr As String, amount As Double, desc As String) As Boolean
            Try
                Sql.ExecuteNonQuery(
                    "INSERT INTO BudgetAmendments (CompanyID, BudgetItemID, AmendmentType, Amount, Description, ApprovedBy) " &
                    "VALUES (?, ?, ?, ?, ?, 'مدیر مالی')",
                    companyID, itemId, typeStr, amount, desc
                )

                If typeStr.Contains("افزایش") OrElse typeStr.Contains("جابجایی") Then
                    Sql.ExecuteNonQuery("UPDATE BudgetItems SET AllocatedBudget = AllocatedBudget + ? WHERE BudgetItemID = ?", amount, itemId)
                ElseIf typeStr.Contains("کاهش") Then
                    Sql.ExecuteNonQuery("UPDATE BudgetItems SET AllocatedBudget = AllocatedBudget - ? WHERE BudgetItemID = ?", amount, itemId)
                End If

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetBudgetLogs(companyID As Integer) As DataTable
            Dim query = "SELECT l.LogID, b.CostCenter, b.ItemTitle, l.SourceModule, l.ExpenseAmount, l.Description, l.CreatedAt " &
                        "FROM BudgetLogs l " &
                        "LEFT JOIN BudgetItems b ON l.BudgetItemID = b.BudgetItemID " &
                        "WHERE l.CompanyID = ? ORDER BY l.LogID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function RecordExpenseBudget(itemId As Integer, companyID As Integer, moduleName As String, amount As Double, desc As String) As Boolean
            Try
                Sql.ExecuteNonQuery(
                    "INSERT INTO BudgetLogs (CompanyID, BudgetItemID, SourceModule, ExpenseAmount, Description) " &
                    "VALUES (?, ?, ?, ?, ?)",
                    companyID, itemId, moduleName, amount, desc
                )
                Sql.ExecuteNonQuery("UPDATE BudgetItems SET UsedBudget = UsedBudget + ? WHERE BudgetItemID = ?", amount, itemId)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetBudgetVarianceReport(companyID As Integer) As DataTable
            Dim query = "SELECT CostCenter, ItemTitle, AllocatedBudget, UsedBudget, " &
                        "(AllocatedBudget - UsedBudget) AS VarianceAmount, " &
                        "CASE WHEN UsedBudget > AllocatedBudget THEN 'نامساعد (انحراف منفی)' ELSE 'مساعد (صرفه‌جویی)' END AS VarianceType " &
                        "FROM BudgetItems WHERE CompanyID = ? ORDER BY CostCenter"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
