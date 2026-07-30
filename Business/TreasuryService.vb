Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class TreasuryService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. TreasuryCashBanks
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS TreasuryCashBanks (" &
                    "CashBankID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "Code TEXT, " &
                    "Title TEXT, " &
                    "Type TEXT DEFAULT 'بانک', " & ' 'بانک', 'صندوق', 'تنخواه', 'درگاه پرداخت'
                    "AccountNumber TEXT, " &
                    "Shaba TEXT, " &
                    "Balance REAL DEFAULT 0, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim cbCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM TreasuryCashBanks"), 0))
                If cbCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryCashBanks (CompanyID, Code, Title, Type, AccountNumber, Shaba, Balance, Notes) " &
                        "VALUES (1, 'BNK-101', 'بانک ملی - شعبه مرکزی', 'بانک', '0105555666001', 'IR120170000000105555666001', 450000000, 'حساب اصلی جاری شرکت')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryCashBanks (CompanyID, Code, Title, Type, AccountNumber, Shaba, Balance, Notes) " &
                        "VALUES (1, 'BNK-102', 'بانک ملت - شعبه میدان ونک', 'بانک', '4822990011', 'IR880120000000004822990011', 280000000, 'حساب پرداخت حقوق')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryCashBanks (CompanyID, Code, Title, Type, AccountNumber, Shaba, Balance, Notes) " &
                        "VALUES (1, 'CSH-001', 'صندوق مرکزی ریالی', 'صندوق', '-', '-', 35000000, 'صندوق نقد دفتر مرکزی')"
                    )
                End If

                ' 2. TreasuryChecks
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS TreasuryChecks (" &
                    "CheckID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PayerPayeeName TEXT, " &
                    "CheckNo TEXT, " &
                    "BankName TEXT, " &
                    "DueDate TEXT, " &
                    "Amount REAL DEFAULT 0, " &
                    "CheckType TEXT DEFAULT 'دریافتی', " & ' 'دریافتی', 'پرداختی'
                    "Status TEXT DEFAULT 'دریافت شده', " & ' 'دریافت شده', 'نزد صندوق', 'واگذاری به بانک', 'وصول شده', 'برگشت خورده', 'خرج شده', 'پاس شده', 'ابطال شده'
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim chkCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM TreasuryChecks"), 0))
                If chkCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now.AddDays(15))
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryChecks (CompanyID, PayerPayeeName, CheckNo, BankName, DueDate, Amount, CheckType, Status, Notes) " &
                        "VALUES (1, 'بازرگانی کیهان تجارت', '8849201', 'بانک تجارت', ?, 120000000, 'دریافتی', 'نزد صندوق', 'بابت بستر فاکتور فروش')",
                        dateStr
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryChecks (CompanyID, PayerPayeeName, CheckNo, BankName, DueDate, Amount, CheckType, Status, Notes) " &
                        "VALUES (1, 'شرکت صنایع فولاد', '100482', 'بانک ملی', ?, 85000000, 'پرداختی', 'صدور یافته', 'بابت پیش‌پرداخت خرید کالا')",
                        dateStr
                    )
                End If

                ' 3. TreasuryLoans
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS TreasuryLoans (" &
                    "LoanID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "BankName TEXT, " &
                    "ContractNo TEXT, " &
                    "TotalAmount REAL DEFAULT 0, " &
                    "InterestRate REAL DEFAULT 18, " &
                    "InstallmentCount INTEGER DEFAULT 12, " &
                    "MonthlyInstallment REAL DEFAULT 0, " &
                    "StartDate TEXT, " &
                    "PaidInstallments INTEGER DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال پرداخت', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim loanCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM TreasuryLoans"), 0))
                If loanCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO TreasuryLoans (CompanyID, BankName, ContractNo, TotalAmount, InterestRate, InstallmentCount, MonthlyInstallment, StartDate, PaidInstallments, Status, Notes) " &
                        "VALUES (1, 'بانک ملی', 'LN-99042', 600000000, 18, 12, 55000000, ?, 2, 'در حال پرداخت', 'تسهیلات فروش اقساطی')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetCashBanks(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM TreasuryCashBanks WHERE CompanyID = ? ORDER BY CashBankID DESC", companyID)
        End Function

        Public Function GetChecks(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM TreasuryChecks WHERE CompanyID = ? ORDER BY CheckID DESC", companyID)
        End Function

        Public Function GetLoans(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM TreasuryLoans WHERE CompanyID = ? ORDER BY LoanID DESC", companyID)
        End Function

        Public Sub SaveCashBank(id As Integer, companyID As Integer, code As String, title As String, typeStr As String, accNo As String, shaba As String, balance As Double, notes As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO TreasuryCashBanks (CompanyID, Code, Title, Type, AccountNumber, Shaba, Balance, Notes) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, code, title, typeStr, accNo, shaba, balance, notes
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE TreasuryCashBanks SET Code = ?, Title = ?, Type = ?, AccountNumber = ?, Shaba = ?, Balance = ?, Notes = ? " &
                    "WHERE CashBankID = ? AND CompanyID = ?",
                    code, title, typeStr, accNo, shaba, balance, notes, id, companyID
                )
            End If
        End Sub

        Public Function UpdateCheckStatus(checkID As Integer, companyID As Integer, newStatus As String) As Boolean
            Try
                ' Get check details
                Dim dt = Sql.ExecuteTable("SELECT * FROM TreasuryChecks WHERE CheckID = ? AND CompanyID = ?", checkID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim amount = Convert.ToDouble(If(IsDBNull(row("Amount")), 0, row("Amount")))
                Dim checkNo = Convert.ToString(row("CheckNo"))
                Dim party = Convert.ToString(row("PayerPayeeName"))
                Dim chkType = Convert.ToString(row("CheckType"))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE TreasuryChecks SET Status = ? WHERE CheckID = ?", newStatus, checkID)

                ' Issue Background Double-Entry Accounting Voucher in Sanad1 & Sanad2
                ' Even if current user has NO Accounting permission, system updates background ledgers!
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند خزانه‌داری اتوماتیک تغییر وضعیت چک " & chkType & " به شماره " & checkNo & " (" & newStatus & ") - " & party

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم خزانه‌داری', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, amount, amount
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                If newStatus = "وصول شده" OrElse newStatus = "پاس شده" Then
                    ' Bedehkar: موجودی بانک (کد کل 10)
                    Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '10', '02', ?, ?, 0)", entryID, "بانک - " & newStatus & " چک " & checkNo, amount)
                    ' Bestankar: اسناد دریافتنی/پرداختنی (کد کل 11)
                    Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '11', '01', ?, 0, ?)", entryID, "اسناد تجاری - " & party, amount)
                Else
                    ' General Status Log Voucher
                    Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '11', '01', ?, ?, 0)", entryID, "اسناد در جریان - " & party, amount)
                    Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '11', '02', ?, 0, ?)", entryID, "انتظار خزانه‌داری - " & newStatus, amount)
                End If

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function PayLoanInstallment(loanID As Integer, companyID As Integer) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM TreasuryLoans WHERE LoanID = ? AND CompanyID = ?", loanID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim installment = Convert.ToDouble(If(IsDBNull(row("MonthlyInstallment")), 0, row("MonthlyInstallment")))
                Dim bankName = Convert.ToString(row("BankName"))
                Dim contractNo = Convert.ToString(row("ContractNo"))
                Dim paid = Convert.ToInt32(If(IsDBNull(row("PaidInstallments")), 0, row("PaidInstallments"))) + 1
                Dim totalCount = Convert.ToInt32(If(IsDBNull(row("InstallmentCount")), 12, row("InstallmentCount")))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Dim stStr = If(paid >= totalCount, "تسویه شده", "در حال پرداخت")

                Sql.ExecuteNonQuery("UPDATE TreasuryLoans SET PaidInstallments = ?, Status = ? WHERE LoanID = ?", paid, stStr, loanID)

                ' Background Accounting Voucher
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "پرداخت قسط شماره " & paid & " تسهیلات بانکی " & bankName & " (قرارداد: " & contractNo & ")"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم خزانه‌داری', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, installment, installment
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: تسهیلات و وام‌های دریافتنی (کد کل 21)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '21', '01', ?, ?, 0)", entryID, "بازپرداخت تسهیلات - " & bankName, installment)

                ' Bestankar: موجودی بانک اصلی (کد کل 10)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '10', '02', ?, 0, ?)", entryID, "پرداخت از حساب بانکی", installment)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetCashFlowMatrix(companyID As Integer) As DataTable
            Dim dt As New DataTable()
            dt.Columns.Add("PeriodTitle", GetType(String))
            dt.Columns.Add("ExpectedInflow", GetType(Double))
            dt.Columns.Add("ExpectedOutflow", GetType(Double))
            dt.Columns.Add("NetCashFlow", GetType(Double))
            dt.Columns.Add("LiquidityStatus", GetType(String))

            Dim bankBalance = Convert.ToDouble(If(Sql.ExecuteScalar("SELECT COALESCE(SUM(Balance), 0) FROM TreasuryCashBanks WHERE CompanyID = ?", companyID), 0))
            Dim checkInflows = Convert.ToDouble(If(Sql.ExecuteScalar("SELECT COALESCE(SUM(Amount), 0) FROM TreasuryChecks WHERE CompanyID = ? AND CheckType = 'دریافتی' AND Status != 'وصول شده'", companyID), 0))
            Dim checkOutflows = Convert.ToDouble(If(Sql.ExecuteScalar("SELECT COALESCE(SUM(Amount), 0) FROM TreasuryChecks WHERE CompanyID = ? AND CheckType = 'پرداختی' AND Status != 'پاس شده'", companyID), 0))
            Dim loanOutflows = Convert.ToDouble(If(Sql.ExecuteScalar("SELECT COALESCE(SUM(MonthlyInstallment), 0) FROM TreasuryLoans WHERE CompanyID = ? AND Status = 'در حال پرداخت'", companyID), 0))

            ' 30 Days Forecast
            Dim net30 = (bankBalance + checkInflows) - (checkOutflows + loanOutflows)
            Dim st30 = If(net30 >= 0, "🟢 نقدینگی مطلوب (مثبت)", "🔴 هشدار کسری نقدینگی")
            dt.Rows.Add("پیش‌بینی ۳۰ روز آینده", bankBalance + checkInflows, checkOutflows + loanOutflows, net30, st30)

            ' 60 Days Forecast
            Dim net60 = net30 + (checkInflows * 0.8) - (checkOutflows * 0.9 + loanOutflows)
            Dim st60 = If(net60 >= 0, "🟢 نقدینگی مطلوب (مثبت)", "🔴 هشدار کسری نقدینگی")
            dt.Rows.Add("پیش‌بینی ۶۰ روز آینده", (bankBalance + checkInflows * 1.8), (checkOutflows * 1.9 + loanOutflows * 2), net60, st60)

            ' 90 Days Forecast
            Dim net90 = net60 + (checkInflows * 0.6) - (checkOutflows * 0.7 + loanOutflows)
            Dim st90 = If(net90 >= 0, "🟢 نقدینگی مطلوب (مثبت)", "🔴 هشدار کسری نقدینگی")
            dt.Rows.Add("پیش‌بینی ۹۰ روز آینده", (bankBalance + checkInflows * 2.4), (checkOutflows * 2.6 + loanOutflows * 3), net90, st90)

            Return dt
        End Function
    End Class
End Namespace
