Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports Negar.Data
Imports Negar.Models

Namespace Negar.Business
    Public Class PayrollService

        Sub New()
            EnsurePayrollTables()
        End Sub

        Public Shared Sub EnsurePayrollTables()
            Try
                ' 1. جدول احکام و اطلاعات پایه پرسنل
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PayrollPersonnel (" &
                    "PersonnelID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "FullName TEXT NOT NULL, " &
                    "NationalCode TEXT, " &
                    "InsuranceNumber TEXT, " &
                    "BankAccountNumber TEXT, " &
                    "Iban TEXT, " &
                    "ContractType TEXT, " &
                    "MaritalStatus TEXT, " &
                    "ChildCount INTEGER DEFAULT 0, " &
                    "BaseSalary DECIMAL DEFAULT 0, " &
                    "HousingAllowance DECIMAL DEFAULT 0, " &
                    "FoodAllowance DECIMAL DEFAULT 0, " &
                    "ChildAllowance DECIMAL DEFAULT 0, " &
                    "SeniorityAllowance DECIMAL DEFAULT 0, " &
                    "ManagementAllowance DECIMAL DEFAULT 0, " &
                    "IsActive BOOLEAN DEFAULT 1);"
                )

                ' 2. جدول کارکرد ماهانه پرسنل
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS MonthlyAttendance (" &
                    "AttendanceID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "PersonnelID INTEGER NOT NULL, " &
                    "SalMaly TEXT NOT NULL, " &
                    "MahMaly INTEGER NOT NULL, " &
                    "WorkDays INTEGER DEFAULT 30, " &
                    "OvertimeHours DECIMAL DEFAULT 0, " &
                    "NightShiftHours DECIMAL DEFAULT 0, " &
                    "LeaveDays DECIMAL DEFAULT 0, " &
                    "AbsenceDays DECIMAL DEFAULT 0, " &
                    "AdvancePayment DECIMAL DEFAULT 0, " &
                    "LoanDeduction DECIMAL DEFAULT 0, " &
                    "FOREIGN KEY(PersonnelID) REFERENCES PayrollPersonnel(PersonnelID));"
                )

                ' 3. جدول محاسبه حقوق ماهانه
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PayrollCalculations (" &
                    "CalculationID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "PersonnelID INTEGER NOT NULL, " &
                    "SalMaly TEXT NOT NULL, " &
                    "MahMaly INTEGER NOT NULL, " &
                    "GrossSalary DECIMAL DEFAULT 0, " &
                    "OvertimeAmount DECIMAL DEFAULT 0, " &
                    "NightShiftAmount DECIMAL DEFAULT 0, " &
                    "TotalBenefits DECIMAL DEFAULT 0, " &
                    "EmployeeInsurance DECIMAL DEFAULT 0, " &
                    "EmployerInsurance DECIMAL DEFAULT 0, " &
                    "UnemploymentInsurance DECIMAL DEFAULT 0, " &
                    "TaxAmount DECIMAL DEFAULT 0, " &
                    "TotalDeductions DECIMAL DEFAULT 0, " &
                    "NetSalary DECIMAL DEFAULT 0, " &
                    "SanadEntryID INTEGER DEFAULT 0, " &
                    "CalcDate DATETIME DEFAULT CURRENT_TIMESTAMP, " &
                    "FOREIGN KEY(PersonnelID) REFERENCES PayrollPersonnel(PersonnelID));"
                )
            Catch ex As Exception
            End Try
        End Sub

        ' ─── مدیریت احکام پرسنل ──────
        Public Function GetPersonnelList() As DataTable
            Return Sql.ExecuteTable("SELECT * FROM PayrollPersonnel ORDER BY PersonnelID DESC")
        End Function

        Public Function SavePersonnel(id As Integer?, fullName As String, nationalCode As String, insuranceNum As String, bankAcc As String, iban As String, contractType As String, marital As String, childCount As Integer, baseSal As Decimal, housing As Decimal, food As Decimal, childAllow As Decimal, seniority As Decimal, mgmtAllow As Decimal, isActive As Boolean) As Integer
            If id.HasValue AndAlso id.Value > 0 Then
                Sql.ExecuteNonQuery(
                    "UPDATE PayrollPersonnel SET FullName=?, NationalCode=?, InsuranceNumber=?, BankAccountNumber=?, Iban=?, ContractType=?, MaritalStatus=?, ChildCount=?, BaseSalary=?, HousingAllowance=?, FoodAllowance=?, ChildAllowance=?, SeniorityAllowance=?, ManagementAllowance=?, IsActive=? WHERE PersonnelID=?",
                    fullName, nationalCode, insuranceNum, bankAcc, iban, contractType, marital, childCount, baseSal, housing, food, childAllow, seniority, mgmtAllow, If(isActive, 1, 0), id.Value)
                Return id.Value
            Else
                Sql.ExecuteNonQuery(
                    "INSERT INTO PayrollPersonnel (FullName, NationalCode, InsuranceNumber, BankAccountNumber, Iban, ContractType, MaritalStatus, ChildCount, BaseSalary, HousingAllowance, FoodAllowance, ChildAllowance, SeniorityAllowance, ManagementAllowance, IsActive) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    fullName, nationalCode, insuranceNum, bankAcc, iban, contractType, marital, childCount, baseSal, housing, food, childAllow, seniority, mgmtAllow, If(isActive, 1, 0))
                Return Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))
            End If
        End Function

        Public Sub DeletePersonnel(id As Integer)
            Sql.ExecuteNonQuery("DELETE FROM PayrollPersonnel WHERE PersonnelID = ?", id)
        End Sub

        ' ─── ثبت کارکرد ماهانه ──────
        Public Function GetMonthlyAttendance(salMaly As String, mahMaly As Integer) As DataTable
            Dim query = "SELECT a.*, p.FullName, p.NationalCode FROM MonthlyAttendance a INNER JOIN PayrollPersonnel p ON a.PersonnelID = p.PersonnelID WHERE a.SalMaly = ? AND a.MahMaly = ?"
            Return Sql.ExecuteTable(query, salMaly, mahMaly)
        End Function

        Public Sub SaveMonthlyAttendance(personnelId As Integer, salMaly As String, mahMaly As Integer, workDays As Integer, overtime As Decimal, nightShift As Decimal, leaveDays As Decimal, absenceDays As Decimal, advancePay As Decimal, loanDed As Decimal)
            Dim dt = Sql.ExecuteTable("SELECT AttendanceID FROM MonthlyAttendance WHERE PersonnelID = ? AND SalMaly = ? AND MahMaly = ?", personnelId, salMaly, mahMaly)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim attId = Convert.ToInt32(dt.Rows(0)("AttendanceID"))
                Sql.ExecuteNonQuery("UPDATE MonthlyAttendance SET WorkDays=?, OvertimeHours=?, NightShiftHours=?, LeaveDays=?, AbsenceDays=?, AdvancePayment=?, LoanDeduction=? WHERE AttendanceID=?", workDays, overtime, nightShift, leaveDays, absenceDays, advancePay, loanDed, attId)
            Else
                Sql.ExecuteNonQuery("INSERT INTO MonthlyAttendance (PersonnelID, SalMaly, MahMaly, WorkDays, OvertimeHours, NightShiftHours, LeaveDays, AbsenceDays, AdvancePayment, LoanDeduction) VALUES (?,?,?,?,?,?,?,?,?,?)", personnelId, salMaly, mahMaly, workDays, overtime, nightShift, leaveDays, absenceDays, advancePay, loanDed)
            End If
        End Sub

        ' ─── محاسبه حقوق ماهانه و صدور اتوماتیک سند حسابداری ──────
        Public Function CalculatePayrollForMonth(salMaly As String, mahMaly As Integer) As DataTable
            Dim pDt = Sql.ExecuteTable("SELECT * FROM PayrollPersonnel WHERE IsActive = 1")
            If pDt Is Nothing OrElse pDt.Rows.Count = 0 Then Return New DataTable()

            Dim totalGross As Decimal = 0
            Dim totalEmpIns As Decimal = 0
            Dim totalEmployerIns As Decimal = 0
            Dim totalTax As Decimal = 0
            Dim totalNet As Decimal = 0

            For Each pRow As DataRow In pDt.Rows
                Dim pId = Convert.ToInt32(pRow("PersonnelID"))
                Dim baseSal = Convert.ToDecimal(pRow("BaseSalary"))
                Dim housing = Convert.ToDecimal(pRow("HousingAllowance"))
                Dim food = Convert.ToDecimal(pRow("FoodAllowance"))
                Dim childAllow = Convert.ToDecimal(pRow("ChildAllowance"))
                Dim seniority = Convert.ToDecimal(pRow("SeniorityAllowance"))
                Dim mgmt = Convert.ToDecimal(pRow("ManagementAllowance"))

                ' کارکرد
                Dim workDays As Integer = 30
                Dim overtimeHours As Decimal = 0
                Dim nightShiftHours As Decimal = 0
                Dim advancePay As Decimal = 0
                Dim loanDed As Decimal = 0

                Dim attDt = Sql.ExecuteTable("SELECT * FROM MonthlyAttendance WHERE PersonnelID = ? AND SalMaly = ? AND MahMaly = ?", pId, salMaly, mahMaly)
                If attDt IsNot Nothing AndAlso attDt.Rows.Count > 0 Then
                    workDays = Convert.ToInt32(attDt.Rows(0)("WorkDays"))
                    overtimeHours = Convert.ToDecimal(attDt.Rows(0)("OvertimeHours"))
                    nightShiftHours = Convert.ToDecimal(attDt.Rows(0)("NightShiftHours"))
                    advancePay = Convert.ToDecimal(attDt.Rows(0)("AdvancePayment"))
                    loanDed = Convert.ToDecimal(attDt.Rows(0)("LoanDeduction"))
                End If

                ' محاسبه کارکرد تناسبی
                Dim actualBase = (baseSal / 30.0D) * workDays
                Dim hourlyRate = (baseSal / 220.0D)
                Dim overtimeAmt = overtimeHours * hourlyRate * 1.4D
                Dim nightShiftAmt = nightShiftHours * hourlyRate * 0.35D

                Dim totalBenefits = actualBase + housing + food + childAllow + seniority + mgmt + overtimeAmt + nightShiftAmt
                Dim grossSal = totalBenefits

                ' بیمه (۷٪ کارمند، ۲۰٪ کارفرما، ۳٪ بیکاری)
                Dim empIns = grossSal * 0.07D
                Dim employerIns = grossSal * 0.20D
                Dim unempIns = grossSal * 0.03D

                ' مالیات حقوق (معافیت ۱۰ میلیون تومان ماهانه)
                Dim taxExemption As Decimal = 100000000D ' ۱۰ میلیون تومان به ریال
                Dim taxableAmt = Math.Max(0.0D, grossSal - taxExemption - empIns)
                Dim taxAmt = taxableAmt * 0.10D ' ۱۰ درصد مالیات

                Dim totalDed = empIns + taxAmt + advancePay + loanDed
                Dim netSal = grossSal - totalDed

                totalGross += grossSal
                totalEmpIns += empIns
                totalEmployerIns += (employerIns + unempIns)
                totalTax += taxAmt
                totalNet += netSal

                ' ذخیره در دیتابیس
                Sql.ExecuteNonQuery("DELETE FROM PayrollCalculations WHERE PersonnelID = ? AND SalMaly = ? AND MahMaly = ?", pId, salMaly, mahMaly)
                Sql.ExecuteNonQuery(
                    "INSERT INTO PayrollCalculations (PersonnelID, SalMaly, MahMaly, GrossSalary, OvertimeAmount, NightShiftAmount, TotalBenefits, EmployeeInsurance, EmployerInsurance, UnemploymentInsurance, TaxAmount, TotalDeductions, NetSalary) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    pId, salMaly, mahMaly, grossSal, overtimeAmt, nightShiftAmt, totalBenefits, empIns, employerIns, unempIns, taxAmt, totalDed, netSal)
            Next

            ' صدور خودکار سند حسابداری حقوق
            CreateAutoAccountingVoucherForPayroll(salMaly, mahMaly, totalGross, totalEmployerIns, totalEmpIns, totalTax, totalNet)

            Return GetMonthlyPayrollReport(salMaly, mahMaly)
        End Function

        ' ─── صدور سند خودکار حسابداری حقوق ──────
        Private Sub CreateAutoAccountingVoucherForPayroll(salMaly As String, mahMaly As Integer, totalGross As Decimal, totalEmployerIns As Decimal, totalEmpIns As Decimal, totalTax As Decimal, totalNet As Decimal)
            Try
                Dim accSvc As New AccountingService()
                Dim invSvc As New InvoiceService()
                Dim companyId = SessionContext.CurrentCompanyID
                Dim fyId = SessionContext.CurrentFiscalYearID
                Dim createdBy = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

                Dim refNum = accSvc.GetNextReferenceNumber()
                Dim desc = $"سند خودکار حقوق و دستمزد ماه {mahMaly} (سال {salMaly})"
                
                ' کدهای حساب سیستم
                Dim accExpensePayroll = invSvc.GetOrCreateSystemAccount(companyId, "501", "هزینه حقوق و مزایای پرسنل", "معین")
                Dim accExpenseIns = invSvc.GetOrCreateSystemAccount(companyId, "502", "هزینه بیمه سهم کارفرما و بیکاری", "معین")
                Dim accPayablePersonnel = invSvc.GetOrCreateSystemAccount(companyId, "201", "حساب‌های پرداختنی (پرسنل)", "معین")
                Dim accPayableInsurance = invSvc.GetOrCreateSystemAccount(companyId, "202", "سازمان تامین اجتماعی (بیمه)", "معین")
                Dim accPayableTax = invSvc.GetOrCreateSystemAccount(companyId, "203", "سازمان امور مالیاتی (مالیات حقوق)", "معین")

                Dim lines As New List(Of AccountingEntryLine)()
                Dim lineNum As Integer = 1

                ' بدهکار: هزینه حقوق
                lines.Add(New AccountingEntryLine With {
                    .LineNumber = lineNum,
                    .AccountID = accExpensePayroll,
                    .DebitAmount = totalGross,
                    .CreditAmount = 0D,
                    .SharhRadif = "هزینه حقوق و مزایای پرسنل"
                })
                lineNum += 1

                ' بدهکار: هزینه بیمه سهم کارفرما
                lines.Add(New AccountingEntryLine With {
                    .LineNumber = lineNum,
                    .AccountID = accExpenseIns,
                    .DebitAmount = totalEmployerIns,
                    .CreditAmount = 0D,
                    .SharhRadif = "هزینه بیمه سهم کارفرما ۲۳٪"
                })
                lineNum += 1

                ' بستانکار: پرسنل (خالص)
                lines.Add(New AccountingEntryLine With {
                    .LineNumber = lineNum,
                    .AccountID = accPayablePersonnel,
                    .DebitAmount = 0D,
                    .CreditAmount = totalNet,
                    .SharhRadif = "خالص حقوق قابل پرداخت پرسنل"
                })
                lineNum += 1

                ' بستانکار: تامین اجتماعی (کل بیمه ۳۰٪)
                lines.Add(New AccountingEntryLine With {
                    .LineNumber = lineNum,
                    .AccountID = accPayableInsurance,
                    .DebitAmount = 0D,
                    .CreditAmount = totalEmpIns + totalEmployerIns,
                    .SharhRadif = "حق بیمه ۳۰٪ سازمان تامین اجتماعی"
                })
                lineNum += 1

                ' بستانکار: مالیات حقوق
                lines.Add(New AccountingEntryLine With {
                    .LineNumber = lineNum,
                    .AccountID = accPayableTax,
                    .DebitAmount = 0D,
                    .CreditAmount = totalTax,
                    .SharhRadif = "مالیات تکلیفی حقوق سازمان مالیاتی"
                })

                Dim totalBedehkar = totalGross + totalEmployerIns
                Dim totalBestankar = totalNet + (totalEmpIns + totalEmployerIns) + totalTax

                accSvc.SaveEntry(DateTime.Now, desc, refNum, createdBy, lines, totalBedehkar, totalBestankar, "تراز است")
            Catch ex As Exception
            End Try
        End Sub

        ' ─── گزارشات جامع حقوق ──────
        Public Function GetMonthlyPayrollReport(salMaly As String, mahMaly As Integer) As DataTable
            Dim query = "SELECT c.*, p.FullName, p.NationalCode, p.InsuranceNumber, p.BankAccountNumber, p.Iban FROM PayrollCalculations c INNER JOIN PayrollPersonnel p ON c.PersonnelID = p.PersonnelID WHERE c.SalMaly = ? AND c.MahMaly = ? ORDER BY c.CalculationID ASC"
            Return Sql.ExecuteTable(query, salMaly, mahMaly)
        End Function

        Public Function GetEmployeeHistoricalReport(personnelId As Integer, salMaly As String) As DataTable
            Dim query = "SELECT c.*, p.FullName, p.NationalCode FROM PayrollCalculations c INNER JOIN PayrollPersonnel p ON c.PersonnelID = p.PersonnelID WHERE c.PersonnelID = ? AND c.SalMaly = ? ORDER BY c.MahMaly ASC"
            Return Sql.ExecuteTable(query, personnelId, salMaly)
        End Function

        ' ─── تولید دیسکت بیمه و مالیات و بانک ──────
        Public Function GenerateSocialSecurityDisketteText(salMaly As String, mahMaly As Integer) As String
            Dim dt = GetMonthlyPayrollReport(salMaly, mahMaly)
            Dim sb As New StringBuilder()
            sb.AppendLine("DSKKAR.DBF - SOCIAL SECURITY DISKETTE REPORT")
            sb.AppendLine($"SalMaly: {salMaly} | MahMaly: {mahMaly} | Total Employees: {dt.Rows.Count}")
            sb.AppendLine("==========================================================================")
            sb.AppendLine("Row | FullName | NationalCode | InsNumber | GrossSalary | EmpIns | EmployerIns")
            sb.AppendLine("--------------------------------------------------------------------------")

            Dim rowIdx As Integer = 1
            For Each row As DataRow In dt.Rows
                sb.AppendLine($"{rowIdx} | {row("FullName")} | {row("NationalCode")} | {row("InsuranceNumber")} | {Convert.ToDecimal(row("GrossSalary")):N0} | {Convert.ToDecimal(row("EmployeeInsurance")):N0} | {Convert.ToDecimal(row("EmployerInsurance")):N0}")
                rowIdx += 1
            Next
            Return sb.ToString()
        End Function

        Public Function GenerateTaxDisketteText(salMaly As String, mahMaly As Integer) As String
            Dim dt = GetMonthlyPayrollReport(salMaly, mahMaly)
            Dim sb As New StringBuilder()
            sb.AppendLine("SALARY TAX REPORT (ARTICLE 85 DIRECT TAX LAW)")
            sb.AppendLine($"SalMaly: {salMaly} | MahMaly: {mahMaly}")
            sb.AppendLine("==========================================================================")
            sb.AppendLine("Row | FullName | NationalCode | GrossSalary | TaxableAmount | TaxAmount")
            sb.AppendLine("--------------------------------------------------------------------------")

            Dim rowIdx As Integer = 1
            For Each row As DataRow In dt.Rows
                sb.AppendLine($"{rowIdx} | {row("FullName")} | {row("NationalCode")} | {Convert.ToDecimal(row("GrossSalary")):N0} | {Convert.ToDecimal(row("TaxAmount")):N0}")
                rowIdx += 1
            Next
            Return sb.ToString()
        End Function

        Public Function GenerateBankPaymentFileText(salMaly As String, mahMaly As Integer) As String
            Dim dt = GetMonthlyPayrollReport(salMaly, mahMaly)
            Dim sb As New StringBuilder()
            sb.AppendLine("PAYA / SATNA BATCH BANK PAYMENT FILE")
            sb.AppendLine($"SalMaly: {salMaly} | MahMaly: {mahMaly}")
            sb.AppendLine("==========================================================================")
            sb.AppendLine("Row | FullName | IBAN | NetPayableAmount (IRR)")
            sb.AppendLine("--------------------------------------------------------------------------")

            Dim rowIdx As Integer = 1
            For Each row As DataRow In dt.Rows
                sb.AppendLine($"{rowIdx} | {row("FullName")} | {row("Iban")} | {Convert.ToDecimal(row("NetSalary")):N0}")
                rowIdx += 1
            Next
            Return sb.ToString()
        End Function

    End Class
End Namespace
