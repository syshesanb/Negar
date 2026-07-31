Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class LegalService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. LegalCases
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LegalCases (" &
                    "CaseID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CaseCode TEXT, " &
                    "CaseTitle TEXT, " &
                    "Claimant TEXT, " &
                    "Defendant TEXT, " &
                    "CourtBranch TEXT DEFAULT 'دادگاه عمومی حقوقی شعبه ۱۰۵', " &
                    "ClaimAmount REAL DEFAULT 150000000, " &
                    "Status TEXT DEFAULT 'در حال رسیدگی', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim caseCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LegalCases"), 0))
                If caseCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LegalCases (CompanyID, CaseCode, CaseTitle, Claimant, Defendant, CourtBranch, ClaimAmount, Status) " &
                        "VALUES (1, 'LEG-1405-01', 'دعوای مطالبه وجه فاکتور تجاری و خسارت تاخیر تادیه', 'شرکت نگار', 'شرکت بازرگانی پارس گستر', 'دادگاه عمومی حقوقی مجتمع شهید بهشتی', 4500000000, 'در حال رسیدگی')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LegalCases (CompanyID, CaseCode, CaseTitle, Claimant, Defendant, CourtBranch, ClaimAmount, Status) " &
                        "VALUES (1, 'LEG-1405-02', 'واخواست و مطالبه چک برگشتی تضمین حسن انجام کار', 'شرکت نگار', 'پیمانکاری سپهر صنعت', 'شورای حل اختلاف منطقه ۲', 1200000000, 'صدور حکم بدوی به نفع')"
                    )
                End If

                ' 2. LegalHearings
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LegalHearings (" &
                    "HearingID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CaseCode TEXT, " &
                    "HearingDate TEXT, " &
                    "HearingTime TEXT DEFAULT '10:30', " &
                    "LawyerName TEXT DEFAULT 'دکتر علیرضا رستمی', " &
                    "Subject TEXT DEFAULT 'جلسه دادرسی و استماع گواهان', " &
                    "Notes TEXT);"
                )

                Dim hearingCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LegalHearings"), 0))
                If hearingCount = 0 Then
                    Dim hearingDate = PersianDateHelper.ToPersian(DateTime.Now.AddDays(14))
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LegalHearings (CompanyID, CaseCode, HearingDate, HearingTime, LawyerName, Subject, Notes) " &
                        "VALUES (1, 'LEG-1405-01', ?, '11:00', 'دکتر علیرضا رستمی (وکیل پایه یک)', 'جلسه اصلی دادرسی و ارزیابی لایحه دفاعیه', 'ارائه اصل فاکتورها و تضامین جهت ارائه به قاضی شعبه')",
                        hearingDate
                    )
                End If

                ' 3. LegalLawyers
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS LegalLawyers (" &
                    "LawyerID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "LawyerName TEXT, " &
                    "LicenseNo TEXT, " &
                    "FeeContractAmount REAL DEFAULT 50000000, " &
                    "PaidAmount REAL DEFAULT 20000000, " &
                    "RemainingAmount REAL DEFAULT 30000000);"
                )

                Dim lawyerCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM LegalLawyers"), 0))
                If lawyerCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO LegalLawyers (CompanyID, LawyerName, LicenseNo, FeeContractAmount, PaidAmount, RemainingAmount) " &
                        "VALUES (1, 'دکتر علیرضا رستمی (وکیل پایه یک دادگستری)', '1402-99881', 180000000, 90000000, 90000000)"
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetCases(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, CaseID, CaseCode, CaseTitle, Claimant, Defendant, CourtBranch, ClaimAmount, Status, CreatedAt FROM LegalCases WHERE CompanyID = ? ORDER BY CaseID DESC", companyID)
        End Function

        Public Function GetHearings(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, HearingID, CaseCode, HearingDate, HearingTime, LawyerName, Subject, Notes FROM LegalHearings WHERE CompanyID = ? ORDER BY HearingID DESC", companyID)
        End Function

        Public Function GetLawyers(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, LawyerID, LawyerName, LicenseNo, FeeContractAmount, PaidAmount, RemainingAmount FROM LegalLawyers WHERE CompanyID = ? ORDER BY LawyerID ASC", companyID)
        End Function

        Public Sub SaveCase(id As Integer, companyID As Integer, title As String, claimant As String, defendant As String, court As String, claimAmt As Double, status As String)
            If id <= 0 Then
                Dim code = "LEG-" & (Environment.TickCount Mod 10000).ToString()
                Sql.ExecuteNonQuery(
                    "INSERT INTO LegalCases (CompanyID, CaseCode, CaseTitle, Claimant, Defendant, CourtBranch, ClaimAmount, Status) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, code, title, claimant, defendant, court, claimAmt, status
                )
            End If
        End Sub
    End Class
End Namespace
