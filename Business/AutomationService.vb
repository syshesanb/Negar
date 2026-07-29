Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class AutomationService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. Office Letters
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS OfficeLetters (" &
                    "LetterID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "LetterNo TEXT, " &
                    "LetterDate TEXT, " &
                    "LetterType INTEGER DEFAULT 1, " & ' 1=Incoming/وارده, 2=Outgoing/صادره, 3=Internal/داخلی
                    "Subject TEXT, " &
                    "SenderInfo TEXT, " &
                    "ReceiverInfo TEXT, " &
                    "Priority INTEGER DEFAULT 1, " & ' 1=Normal/عادی, 2=Urgent/فوری, 3=Immediate/آنی
                    "Confidentiality INTEGER DEFAULT 1, " & ' 1=Normal/عادی, 2=Confidential/محرمانه, 3=Secret/سری
                    "ContentBody TEXT, " &
                    "Status TEXT DEFAULT 'در دست اقدام', " &
                    "PersonnelID INTEGER DEFAULT 0, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' Seed sample operational letters if table is empty
                Dim letterCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM OfficeLetters"), 0))
                If letterCount = 0 Then
                    Dim todayStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO OfficeLetters (CompanyID, LetterNo, LetterDate, LetterType, Subject, SenderInfo, ReceiverInfo, Priority, Confidentiality, ContentBody, Status) " &
                        "VALUES (1, '1405/و/1001', ?, 1, 'درخواست استعلام قیمت تجهیزات اداری', 'شرکت همکاران آریا', 'مدیریت تدارکات', 1, 1, 'با سلام، احتراماً خواهشمند است استعلام قیمت تجهیزات اداری فوق را ارسال فرمایید.', 'در دست اقدام')",
                        todayStr
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO OfficeLetters (CompanyID, LetterNo, LetterDate, LetterType, Subject, SenderInfo, ReceiverInfo, Priority, Confidentiality, ContentBody, Status) " &
                        "VALUES (1, '1405/ص/2005', ?, 2, 'ارسال صورتحساب‌های فصل بهار', 'مدیریت مالی', 'سازمان امور مالیاتی', 2, 2, 'با سلام، به پیوست کلیه صورتحساب‌های الکترونیکی مربوط به دوره بهار ۱۴۰۵ ایفاد می‌گردد.', 'ارسال شده')",
                        todayStr
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO OfficeLetters (CompanyID, LetterNo, LetterDate, LetterType, Subject, SenderInfo, ReceiverInfo, Priority, Confidentiality, ContentBody, Status) " &
                        "VALUES (1, '1405/د/3012', ?, 3, 'دستورالعمل حضور و غیاب و مرخصی‌ها', 'مدیریت منابع انسانی', 'کلیه واحدهای سازمانی', 1, 1, 'بدین‌وسیله به اطلاع کلیه همکاران می‌رساند ساعت کاری واحد اداری از تاریخ جاری ثبت می‌شود.', 'خاتمه یافته')",
                        todayStr
                    )
                End If

                ' 2. Office Letter Referrals
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS OfficeLetterReferrals (" &
                    "ReferralID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "LetterID INTEGER, " &
                    "CompanyID INTEGER, " &
                    "FromPersonnelID INTEGER DEFAULT 0, " &
                    "ToPersonnelID INTEGER DEFAULT 0, " &
                    "ReferralDate TEXT, " &
                    "InstructionText TEXT, " &
                    "DeadlineDate TEXT, " &
                    "Status TEXT DEFAULT 'جدید', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetLetters(companyID As Integer, Optional letterTypeFilter As Integer = 0) As DataTable
            Dim query As String = "SELECT l.LetterID, l.LetterNo, l.LetterDate, " &
                        "CASE l.LetterType WHEN 1 THEN 'وارده 📥' WHEN 2 THEN 'صادره 📤' ELSE 'داخلی 📝' END AS TypeTitle, " &
                        "l.Subject, l.SenderInfo, l.ReceiverInfo, " &
                        "CASE l.Priority WHEN 1 THEN 'عادی' WHEN 2 THEN 'فوری ⚡' ELSE 'آنی 🚨' END AS PriorityTitle, " &
                        "CASE l.Confidentiality WHEN 1 THEN 'عادی' WHEN 2 THEN 'محرمانه 🔒' ELSE 'سری 🔑' END AS ConfidentialityTitle, " &
                        "l.Status, l.ContentBody " &
                        "FROM OfficeLetters l WHERE l.CompanyID = ? "

            If letterTypeFilter > 0 Then
                query &= "AND l.LetterType = " & letterTypeFilter.ToString() & " "
            End If

            query &= "ORDER BY l.LetterID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function

        Public Function GetLetterById(letterID As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM OfficeLetters WHERE LetterID = ?", letterID)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub SaveLetter(letterID As Integer, companyID As Integer, letterNo As String, letterDate As String, letterType As Integer, subject As String, senderInfo As String, receiverInfo As String, priority As Integer, confidentiality As Integer, contentBody As String, status As String)
            If letterID <= 0 Then
                Sql.ExecuteNonQuery(
                    "INSERT INTO OfficeLetters (CompanyID, LetterNo, LetterDate, LetterType, Subject, SenderInfo, ReceiverInfo, Priority, Confidentiality, ContentBody, Status) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    companyID, letterNo, letterDate, letterType, subject, senderInfo, receiverInfo, priority, confidentiality, contentBody, status
                )
            Else
                Sql.ExecuteNonQuery(
                    "UPDATE OfficeLetters SET LetterNo = ?, LetterDate = ?, LetterType = ?, Subject = ?, SenderInfo = ?, ReceiverInfo = ?, Priority = ?, Confidentiality = ?, ContentBody = ?, Status = ? " &
                    "WHERE LetterID = ? AND CompanyID = ?",
                    letterNo, letterDate, letterType, subject, senderInfo, receiverInfo, priority, confidentiality, contentBody, status, letterID, companyID
                )
            End If
        End Sub

        Public Sub DeleteLetter(letterID As Integer, companyID As Integer)
            Sql.ExecuteNonQuery("DELETE FROM OfficeLetters WHERE LetterID = ? AND CompanyID = ?", letterID, companyID)
            Sql.ExecuteNonQuery("DELETE FROM OfficeLetterReferrals WHERE LetterID = ? AND CompanyID = ?", letterID, companyID)
        End Sub

        Public Sub AddReferral(letterID As Integer, companyID As Integer, fromPersonId As Integer, toPersonId As Integer, instructionText As String, deadlineDate As String)
            Dim todayStr = PersianDateHelper.ToPersian(DateTime.Now)
            Sql.ExecuteNonQuery(
                "INSERT INTO OfficeLetterReferrals (LetterID, CompanyID, FromPersonnelID, ToPersonnelID, ReferralDate, InstructionText, DeadlineDate, Status) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, 'جدید')",
                letterID, companyID, fromPersonId, toPersonId, todayStr, instructionText, deadlineDate
            )
            Sql.ExecuteNonQuery("UPDATE OfficeLetters SET Status = 'ارجاع شده' WHERE LetterID = ?", letterID)
        End Sub

        Public Function GetReferralsForLetter(letterID As Integer) As DataTable
            Dim query = "SELECT r.ReferralID, r.ReferralDate, COALESCE(p1.FullName, 'دبیرخانه') AS FromPerson, " &
                        "COALESCE(p2.FullName, 'مسئول مربوطه') AS ToPerson, r.InstructionText, r.DeadlineDate, r.Status " &
                        "FROM OfficeLetterReferrals r " &
                        "LEFT JOIN PayrollPersonnel p1 ON r.FromPersonnelID = p1.PersonnelID " &
                        "LEFT JOIN PayrollPersonnel p2 ON r.ToPersonnelID = p2.PersonnelID " &
                        "WHERE r.LetterID = ? ORDER BY r.ReferralID DESC"
            Return Sql.ExecuteTable(query, letterID)
        End Function

        Public Function GetAutomationReports(companyID As Integer) As DataTable
            Dim query = "SELECT l.LetterNo, l.LetterDate, l.Subject, " &
                        "CASE l.LetterType WHEN 1 THEN 'وارده' WHEN 2 THEN 'صادره' ELSE 'داخلی' END AS LetterType, " &
                        "l.SenderInfo, l.ReceiverInfo, l.Status, " &
                        "(SELECT COUNT(*) FROM OfficeLetterReferrals r WHERE r.LetterID = l.LetterID) AS ReferralCount " &
                        "FROM OfficeLetters l WHERE l.CompanyID = ? ORDER BY l.LetterID DESC"
            Return Sql.ExecuteTable(query, companyID)
        End Function
    End Class
End Namespace
