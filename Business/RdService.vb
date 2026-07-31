Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class RdService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. RdProjects
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS RdProjects (" &
                    "ProjectID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectCode TEXT, " &
                    "ProjectTitle TEXT, " &
                    "Category TEXT DEFAULT 'فرمول جدید', " &
                    "Stage TEXT DEFAULT 'ایده‌پردازی', " &
                    "LeadName TEXT, " &
                    "BudgetAmount REAL DEFAULT 0, " &
                    "SpentAmount REAL DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال اجرا', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
                Dim cnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM RdProjects"), 0))
                If cnt = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO RdProjects (CompanyID, ProjectCode, ProjectTitle, Category, Stage, LeadName, BudgetAmount, SpentAmount, Status) " &
                        "VALUES (1, 'NPD-1405-01', 'توسعه فرمول جدید شوینده صنعتی با پایه آنزیمی', 'فرمول جدید', 'فرمولاسیون', 'دکتر ساره محمدی', 800000000, 320000000, 'در حال اجرا')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO RdProjects (CompanyID, ProjectCode, ProjectTitle, Category, Stage, LeadName, BudgetAmount, SpentAmount, Status) " &
                        "VALUES (1, 'NPD-1405-02', 'بهبود فرمولاسیون محصول A جهت افزایش ماندگاری به ۲۴ ماه', 'بهبود محصول موجود', 'پایلوت', 'مهندس علی نوری', 350000000, 280000000, 'در حال اجرا')"
                    )
                End If

                ' 2. RdFormulations
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS RdFormulations (" &
                    "FormulationID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectCode TEXT, " &
                    "FormulationCode TEXT, " &
                    "Version TEXT DEFAULT 'v1.0', " &
                    "ComponentName TEXT, " &
                    "Percentage REAL DEFAULT 0, " &
                    "CasNumber TEXT, " &
                    "SecurityLevel TEXT DEFAULT 'محرمانه', " &
                    "Notes TEXT);"
                )
                Dim fCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM RdFormulations"), 0))
                If fCnt = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO RdFormulations (CompanyID, ProjectCode, FormulationCode, Version, ComponentName, Percentage, CasNumber, SecurityLevel) VALUES (1, 'NPD-1405-01', 'FORM-001', 'v1.0', 'آنزیم پروتئاز خالص (Grade A)', 8.5, '9014-01-1', 'محرمانه')")
                    Sql.ExecuteNonQuery("INSERT INTO RdFormulations (CompanyID, ProjectCode, FormulationCode, Version, ComponentName, Percentage, CasNumber, SecurityLevel) VALUES (1, 'NPD-1405-01', 'FORM-001', 'v1.0', 'سدیم لوریل سولفات (SLS)', 12.0, '151-21-3', 'محرمانه')")
                    Sql.ExecuteNonQuery("INSERT INTO RdFormulations (CompanyID, ProjectCode, FormulationCode, Version, ComponentName, Percentage, CasNumber, SecurityLevel) VALUES (1, 'NPD-1405-01', 'FORM-001', 'v1.0', 'کوکو بتائین (آمفوتریک)', 5.5, '61789-40-0', 'محرمانه')")
                    Sql.ExecuteNonQuery("INSERT INTO RdFormulations (CompanyID, ProjectCode, FormulationCode, Version, ComponentName, Percentage, CasNumber, SecurityLevel) VALUES (1, 'NPD-1405-01', 'FORM-001', 'v1.0', 'آب مقطر (Purified Water)', 74.0, '7732-18-5', 'عمومی')")
                End If

                ' 3. RdLabTests
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS RdLabTests (" &
                    "TestID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ProjectCode TEXT, " &
                    "TestCode TEXT, " &
                    "TestDate TEXT, " &
                    "TestType TEXT DEFAULT 'فیزیکوشیمیایی', " &
                    "Parameter TEXT, " &
                    "TargetValue TEXT, " &
                    "ActualValue TEXT, " &
                    "Result TEXT DEFAULT 'قابل قبول', " &
                    "TechnicianName TEXT);"
                )
                Dim lCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM RdLabTests"), 0))
                If lCnt = 0 Then
                    Dim td = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery("INSERT INTO RdLabTests (CompanyID, ProjectCode, TestCode, TestDate, TestType, Parameter, TargetValue, ActualValue, Result, TechnicianName) VALUES (1, 'NPD-1405-01', 'LAB-001', ?, 'فیزیکوشیمیایی', 'pH محلول ۱٪', '6.5 - 7.5', '7.1', 'قابل قبول', 'خانم دکتر رضایی')", td)
                    Sql.ExecuteNonQuery("INSERT INTO RdLabTests (CompanyID, ProjectCode, TestCode, TestDate, TestType, Parameter, TargetValue, ActualValue, Result, TechnicianName) VALUES (1, 'NPD-1405-01', 'LAB-002', ?, 'فیزیکوشیمیایی', 'ویسکوزیته (cP)', '500 - 800', '720', 'قابل قبول', 'خانم دکتر رضایی')", td)
                    Sql.ExecuteNonQuery("INSERT INTO RdLabTests (CompanyID, ProjectCode, TestCode, TestDate, TestType, Parameter, TargetValue, ActualValue, Result, TechnicianName) VALUES (1, 'NPD-1405-01', 'LAB-003', ?, 'میکروبیولوژیکی', 'شمارش کلی باکتری (CFU/g)', '< 100', '< 10', 'قابل قبول', 'آقای مهندس کریمی')", td)
                End If

                ' 4. RdPatents
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS RdPatents (" &
                    "PatentID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "PatentNo TEXT, " &
                    "Title TEXT, " &
                    "RegisterDate TEXT, " &
                    "ExpiryDate TEXT, " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "LicenseIncome REAL DEFAULT 0);"
                )
                Dim pCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM RdPatents"), 0))
                If pCnt = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO RdPatents (CompanyID, PatentNo, Title, RegisterDate, ExpiryDate, Status, LicenseIncome) VALUES (1, 'PAT-IR-140201', 'فرمول ترکیبی آنزیمی جهت شستشوی صنعتی سطوح فلزی', '1402/05/15', '1422/05/15', 'فعال', 120000000)")
                End If

            Catch ex As Exception
            End Try
        End Sub

        Public Function GetProjects(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, ProjectID, ProjectCode, ProjectTitle, Category, Stage, LeadName, BudgetAmount, SpentAmount, Status, CreatedAt FROM RdProjects WHERE CompanyID = ? ORDER BY ProjectID DESC", companyID)
        End Function

        Public Function GetFormulations(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, FormulationID, ProjectCode, FormulationCode, Version, ComponentName, Percentage, CasNumber, SecurityLevel, Notes FROM RdFormulations WHERE CompanyID = ? ORDER BY FormulationID ASC", companyID)
        End Function

        Public Function GetLabTests(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, TestID, ProjectCode, TestCode, TestDate, TestType, Parameter, TargetValue, ActualValue, Result, TechnicianName FROM RdLabTests WHERE CompanyID = ? ORDER BY TestID DESC", companyID)
        End Function

        Public Function GetPatents(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, PatentID, PatentNo, Title, RegisterDate, ExpiryDate, Status, LicenseIncome FROM RdPatents WHERE CompanyID = ? ORDER BY PatentID DESC", companyID)
        End Function

        Public Sub SaveProject(companyID As Integer, title As String, category As String, stage As String, lead As String, budget As Double)
            Dim code = "NPD-" & DateTime.Now.ToString("yyMM") & "-" & (Environment.TickCount Mod 100).ToString("D2")
            Sql.ExecuteNonQuery(
                "INSERT INTO RdProjects (CompanyID, ProjectCode, ProjectTitle, Category, Stage, LeadName, BudgetAmount, SpentAmount, Status) VALUES (?, ?, ?, ?, ?, ?, ?, 0, 'در حال اجرا')",
                companyID, code, title, category, stage, lead, budget
            )
        End Sub
    End Class
End Namespace
