Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class VoipService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. VoipCallLogs — تاریخچه تماس‌ها
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS VoipCallLogs (" &
                    "CallID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CallDate TEXT, " &
                    "CallTime TEXT, " &
                    "Direction TEXT DEFAULT 'ورودی', " &
                    "CallerNumber TEXT, " &
                    "CustomerName TEXT, " &
                    "OperatorName TEXT, " &
                    "Duration INTEGER DEFAULT 0, " &
                    "Outcome TEXT DEFAULT 'بی‌پاسخ', " &
                    "Note TEXT, " &
                    "CsatScore INTEGER DEFAULT 0);"
                )
                Dim cnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM VoipCallLogs"), 0))
                If cnt = 0 Then
                    Dim td = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery("INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note, CsatScore) VALUES (1, ?, '09:15', 'ورودی', '09123456789', 'شرکت نیکان تجارت', 'آقای رضایی', 412, 'سفارش ثبت شد', 'مشتری خواستار تخفیف ویژه بود — توافق ۵٪ انجام شد.', 5)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note, CsatScore) VALUES (1, ?, '10:42', 'ورودی', '02144332211', 'آقای محمد کریمی', 'خانم احمدی', 185, 'پیگیری شکایت', 'مشتری از تأخیر در تحویل ناراضی بود. تیکت پشتیبانی ثبت گردید.', 3)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note, CsatScore) VALUES (1, ?, '11:30', 'خروجی', '09351234567', 'خانم سارا موسوی', 'آقای رضایی', 278, 'قرار ملاقات گذاشته شد', 'ارائه کاتالوگ محصولات جدید و هماهنگی جلسه برای هفته آینده.', 5)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note, CsatScore) VALUES (1, ?, '13:05', 'ورودی', '09219876543', 'شرکت پارسیان صنعت', 'خانم احمدی', 0, 'بی‌پاسخ (صف رها شد)', '', 0)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note, CsatScore) VALUES (1, ?, '14:22', 'ورودی', '02188776655', 'آقای علی نظری', 'آقای رضایی', 542, 'فروش انجام شد', 'خرید ۱۵۰ عدد محصول A با ۷٪ تخفیف — فاکتور صادر شد.', 5)", td)
                End If

                ' 2. VoipQueue — وضعیت Real-Time صف
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS VoipQueue (" &
                    "QueueID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "OperatorName TEXT, " &
                    "Extension TEXT, " &
                    "Status TEXT DEFAULT 'آزاد', " &
                    "TotalCallsToday INTEGER DEFAULT 0, " &
                    "AvgDuration INTEGER DEFAULT 0, " &
                    "ConversionRate REAL DEFAULT 0);"
                )
                Dim qCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM VoipQueue"), 0))
                If qCnt = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO VoipQueue (CompanyID, OperatorName, Extension, Status, TotalCallsToday, AvgDuration, ConversionRate) VALUES (1, 'آقای رضایی (فروش ارشد)', '101', 'مشغول', 18, 380, 38.5)")
                    Sql.ExecuteNonQuery("INSERT INTO VoipQueue (CompanyID, OperatorName, Extension, Status, TotalCallsToday, AvgDuration, ConversionRate) VALUES (1, 'خانم احمدی (پشتیبانی)', '102', 'آزاد', 12, 220, 15.0)")
                    Sql.ExecuteNonQuery("INSERT INTO VoipQueue (CompanyID, OperatorName, Extension, Status, TotalCallsToday, AvgDuration, ConversionRate) VALUES (1, 'آقای کریمی (فروش)', '103', 'استراحت', 9, 310, 22.2)")
                End If

                ' 3. VoipRecordings — آرشیو صوتی
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS VoipRecordings (" &
                    "RecordID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CallDate TEXT, " &
                    "CustomerName TEXT, " &
                    "OperatorName TEXT, " &
                    "Duration INTEGER DEFAULT 0, " &
                    "FileName TEXT, " &
                    "FileSize TEXT, " &
                    "Transcribed INTEGER DEFAULT 0);"
                )
                Dim rCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM VoipRecordings"), 0))
                If rCnt = 0 Then
                    Dim td = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery("INSERT INTO VoipRecordings (CompanyID, CallDate, CustomerName, OperatorName, Duration, FileName, FileSize, Transcribed) VALUES (1, ?, 'شرکت نیکان تجارت', 'آقای رضایی', 412, 'REC-20260731-090101.mp3', '3.8 MB', 1)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipRecordings (CompanyID, CallDate, CustomerName, OperatorName, Duration, FileName, FileSize, Transcribed) VALUES (1, ?, 'آقای علی نظری', 'آقای رضایی', 542, 'REC-20260731-142201.mp3', '5.1 MB', 0)", td)
                    Sql.ExecuteNonQuery("INSERT INTO VoipRecordings (CompanyID, CallDate, CustomerName, OperatorName, Duration, FileName, FileSize, Transcribed) VALUES (1, ?, 'خانم سارا موسوی', 'آقای رضایی', 278, 'REC-20260731-113001.mp3', '2.6 MB', 0)", td)
                End If

                ' 4. VoipCampaigns — کمپین‌های خروجی
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS VoipCampaigns (" &
                    "CampaignID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "CampaignName TEXT, " &
                    "StartDate TEXT, " &
                    "EndDate TEXT, " &
                    "TotalContacts INTEGER DEFAULT 0, " &
                    "Contacted INTEGER DEFAULT 0, " &
                    "Converted INTEGER DEFAULT 0, " &
                    "Status TEXT DEFAULT 'در حال اجرا');"
                )
                Dim cCnt = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM VoipCampaigns"), 0))
                If cCnt = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO VoipCampaigns (CompanyID, CampaignName, StartDate, EndDate, TotalContacts, Contacted, Converted, Status) VALUES (1, 'کمپین معرفی محصولات جدید مرداد ۱۴۰۵', '1405/05/01', '1405/05/31', 250, 178, 42, 'در حال اجرا')")
                    Sql.ExecuteNonQuery("INSERT INTO VoipCampaigns (CompanyID, CampaignName, StartDate, EndDate, TotalContacts, Contacted, Converted, Status) VALUES (1, 'پیگیری مشتریان راکد بیش از ۶ ماه', '1405/04/15', '1405/04/30', 120, 120, 19, 'پایان‌یافته')")
                End If

            Catch ex As Exception
            End Try
        End Sub

        Public Function GetCallLogs(companyID As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT '' AS colRowIndex, CallID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, " &
                "Duration, Outcome, Note, CsatScore FROM VoipCallLogs WHERE CompanyID = ? ORDER BY CallID DESC", companyID)
        End Function

        Public Function GetQueue(companyID As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT '' AS colRowIndex, QueueID, OperatorName, Extension, Status, TotalCallsToday, AvgDuration, ConversionRate " &
                "FROM VoipQueue WHERE CompanyID = ? ORDER BY TotalCallsToday DESC", companyID)
        End Function

        Public Function GetRecordings(companyID As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT '' AS colRowIndex, RecordID, CallDate, CustomerName, OperatorName, Duration, FileName, FileSize, Transcribed " &
                "FROM VoipRecordings WHERE CompanyID = ? ORDER BY RecordID DESC", companyID)
        End Function

        Public Function GetCampaigns(companyID As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT '' AS colRowIndex, CampaignID, CampaignName, StartDate, EndDate, TotalContacts, Contacted, Converted, Status " &
                "FROM VoipCampaigns WHERE CompanyID = ? ORDER BY CampaignID DESC", companyID)
        End Function

        Public Sub LogCall(companyID As Integer, callerNo As String, customerName As String, operatorName As String,
                           direction As String, durationSec As Integer, outcome As String, note As String)
            Dim td = PersianDateHelper.ToPersian(DateTime.Now)
            Dim tt = DateTime.Now.ToString("HH:mm")
            Sql.ExecuteNonQuery(
                "INSERT INTO VoipCallLogs (CompanyID, CallDate, CallTime, Direction, CallerNumber, CustomerName, OperatorName, Duration, Outcome, Note) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                companyID, td, tt, direction, callerNo, customerName, operatorName, durationSec, outcome, note)
        End Sub
    End Class
End Namespace
