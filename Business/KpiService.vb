Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class KpiService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. KpiTargets
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS KpiTargets (" &
                    "TargetID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PersonnelName TEXT, " &
                    "TargetTitle TEXT, " &
                    "Category TEXT DEFAULT 'فروش', " &
                    "TargetValue REAL DEFAULT 0, " &
                    "ActualValue REAL DEFAULT 0, " &
                    "Weight REAL DEFAULT 1, " &
                    "Unit TEXT DEFAULT 'درصد', " &
                    "PeriodName TEXT DEFAULT 'سه ماهه اول', " &
                    "Status TEXT DEFAULT 'در حال پایش', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim targetCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM KpiTargets"), 0))
                If targetCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO KpiTargets (CompanyID, PersonnelName, TargetTitle, Category, TargetValue, ActualValue, Weight, Unit, PeriodName, Status) " &
                        "VALUES (1, 'رضا محمدی', 'تحقق ۵ میلیارد ریال فروش ماهانه', 'فروش', 5000000000, 4800000000, 30, 'مبلغ', 'بهار ۱۴۰۵', 'در حال پایش')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO KpiTargets (CompanyID, PersonnelName, TargetTitle, Category, TargetValue, ActualValue, Weight, Unit, PeriodName, Status) " &
                        "VALUES (1, 'مریم احمدی', 'کاهش ضایعات تولید به زیر ۲ درصد', 'تولید', 2.0, 1.5, 25, 'درصد', 'بهار ۱۴۰۵', 'در حال پایش')"
                    )
                End If

                ' 2. KpiEvaluations
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS KpiEvaluations (" &
                    "EvalID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PersonnelName TEXT, " &
                    "EvalDate TEXT, " &
                    "SelfScore REAL DEFAULT 0, " &
                    "ManagerScore REAL DEFAULT 0, " &
                    "FinalScore REAL DEFAULT 0, " &
                    "PerformanceGrade TEXT DEFAULT 'عالی (A)', " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim evalCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM KpiEvaluations"), 0))
                If evalCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO KpiEvaluations (CompanyID, PersonnelName, EvalDate, SelfScore, ManagerScore, FinalScore, PerformanceGrade, Notes) " &
                        "VALUES (1, 'رضا محمدی', ?, 90, 95, 92.5, 'عالی (A)', 'عملکرد بسیار خوب در تحقق فروش بهار')",
                        dateStr
                    )
                End If

                ' 3. KpiBonuses
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS KpiBonuses (" &
                    "BonusID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PersonnelName TEXT, " &
                    "PeriodName TEXT DEFAULT 'بهار ۱۴۰۵', " &
                    "BaseAmount REAL DEFAULT 0, " &
                    "PerformanceFactor REAL DEFAULT 1.0, " &
                    "CalculatedBonus REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'محاسبه‌شده', " &
                    "BonusDate TEXT, " &
                    "Notes TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim bonusCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM KpiBonuses"), 0))
                If bonusCount = 0 Then
                    Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO KpiBonuses (CompanyID, PersonnelName, PeriodName, BaseAmount, PerformanceFactor, CalculatedBonus, Status, BonusDate, Notes) " &
                        "VALUES (1, 'رضا محمدی', 'بهار ۱۴۰۵', 50000000, 1.2, 60000000, 'محاسبه‌شده', ?, 'پاداش تحقق اهداف فروش')",
                        dateStr
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetKpiTargets(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, TargetID, PersonnelName, TargetTitle, Category, TargetValue, ActualValue, Weight, Unit, PeriodName, Status FROM KpiTargets WHERE CompanyID = ? ORDER BY TargetID DESC", companyID)
        End Function

        Public Function GetKpiEvaluations(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, EvalID, PersonnelName, EvalDate, SelfScore, ManagerScore, FinalScore, PerformanceGrade, Notes FROM KpiEvaluations WHERE CompanyID = ? ORDER BY EvalID DESC", companyID)
        End Function

        Public Function GetKpiBonuses(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, BonusID, PersonnelName, PeriodName, BaseAmount, PerformanceFactor, CalculatedBonus, Status, BonusDate, Notes FROM KpiBonuses WHERE CompanyID = ? ORDER BY BonusID DESC", companyID)
        End Function

        Public Sub SaveKpiTarget(id As Integer, companyID As Integer, person As String, title As String, category As String, targetVal As Double, actualVal As Double, weight As Double, unit As String, period As String)
            If id <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO KpiTargets (CompanyID, PersonnelName, TargetTitle, Category, TargetValue, ActualValue, Weight, Unit, PeriodName, Status) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, 'در حال پایش')",
                    companyID, person, title, category, targetVal, actualVal, weight, unit, period
                )
            End If
        End Sub

        Public Sub SaveKpiEvaluation(companyID As Integer, person As String, selfScore As Double, mgrScore As Double, notes As String)
            Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)
            Dim finalScore = (selfScore + mgrScore) / 2.0
            Dim grade = "متوسط (C)"
            If finalScore >= 90 Then
                grade = "عالی (A)"
            ElseIf finalScore >= 75 Then
                grade = "خوب (B)"
            ElseIf finalScore < 60 Then
                grade = "ضعیف (D)"
            End If

            Sql.ExecuteNonQuery(
                "INSERT INTO KpiEvaluations (CompanyID, PersonnelName, EvalDate, SelfScore, ManagerScore, FinalScore, PerformanceGrade, Notes) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                companyID, person, dateStr, selfScore, mgrScore, finalScore, grade, notes
            )
        End Sub

        Public Function ConfirmAndTransferBonus(bonusID As Integer, companyID As Integer, salMaly As String) As Boolean
            Try
                Dim dt = Sql.ExecuteTable("SELECT * FROM KpiBonuses WHERE BonusID = ? AND CompanyID = ?", bonusID, companyID)
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

                Dim row = dt.Rows(0)
                Dim person = Convert.ToString(row("PersonnelName"))
                Dim bonusAmt = Convert.ToDouble(If(IsDBNull(row("CalculatedBonus")), 0, row("CalculatedBonus")))
                Dim period = Convert.ToString(row("PeriodName"))
                Dim dateStr = PersianDateHelper.ToPersian(DateTime.Now)

                Sql.ExecuteNonQuery("UPDATE KpiBonuses SET Status = 'تایید نهایی و انتقال به حقوق' WHERE BonusID = ?", bonusID)

                ' Issue Background Accounting Voucher for Performance Bonus in Sanad1 & Sanad2
                Dim nextRef = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT MAX(CAST(ReferenceNumber AS INTEGER)) FROM Sanad1 WHERE CompanyID = ?", companyID), 0)) + 1
                Dim desc = "سند حسابداری ذخیره پاداش عملکرد و کارانه " & person & " دوره " & period & " (مبلغ: " & bonusAmt.ToString("N0") & " ریال)"

                Sql.ExecuteNonQuery(
                    "INSERT INTO Sanad1 (CompanyID, EntryDate, Description, ReferenceNumber, CreatedBy, VazeiatSanad, AdamVirayesh, JamBedehkar, JamBestankar, TaeazSanad) " &
                    "VALUES (?, ?, ?, ?, 'سیستم ارزیابی عملکرد', 'سند موقت', 1, ?, ?, 1)",
                    companyID, dateStr, desc, nextRef, bonusAmt, bonusAmt
                )

                Dim entryID = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))

                ' Bedehkar: هزینه پاداش و کارانه پرسنل (کد کل 51)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '51', '05', ?, ?, 0)", entryID, "هزینه پاداش عملکرد پرسنل - " & person, bonusAmt)

                ' Bestankar: جاری پرسنل / پاداش پرداختنی (کد کل 21)
                Sql.ExecuteNonQuery("INSERT INTO Sanad2 (EntryID, KolCode, MoeinCode, Description, Bedehkar, Bestankar) VALUES (?, '21', '03', ?, 0, ?)", entryID, "پاداش و کارانه پرداختنی پرسنل - " & person, bonusAmt)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetKpiPerformanceReport(companyID As Integer) As DataTable
            Dim query = "SELECT '' AS colRowIndex, PersonnelName, COUNT(TargetID) AS TotalTargets, " &
                        "AVG(ROUND((ActualValue / CASE WHEN TargetValue = 0 THEN 1 ELSE TargetValue END) * 100, 1)) AS AvgAchievementRate, " &
                        "MAX(Status) AS OverallStatus " &
                        "FROM KpiTargets WHERE CompanyID = ? GROUP BY PersonnelName"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
