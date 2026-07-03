Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms

    Public Class SanadListItem
        Public Property EntryID As Integer
        Public Property ReferenceNumber As String
        Public Property EntryDate As DateTime
        Public Property Description As String
        Public Property AdamVirayesh As Boolean
        Public Property Row As DataRow

        Public Sub New(row As DataRow)
            Me.Row = row
            Me.EntryID = Convert.ToInt32(row("EntryID"))
            Me.ReferenceNumber = Convert.ToString(row("ReferenceNumber"))
            Me.EntryDate = Convert.ToDateTime(row("EntryDate"))
            Me.Description = Convert.ToString(row("Description"))
            Me.AdamVirayesh = If(row("AdamVirayesh") Is DBNull.Value, False, Convert.ToBoolean(row("AdamVirayesh")))
        End Sub

        Public Overrides Function ToString() As String
            Return "سند شماره " & ReferenceNumber & " مورخ " & PersianDateHelper.ToPersian(EntryDate) & " (" & Description & ")"
        End Function
    End Class

    Partial Class MergeSanadsForm
        Private ReadOnly service As New AccountingService()

        Public Sub New()
            InitializeComponent()
            LoadMonths()
        End Sub

        Private Sub LoadMonths()
            cmbMonth.Items.Clear()
            For i = 1 To 12
                cmbMonth.Items.Add(i.ToString("00"))
            Next

            ' به‌طور پیش‌فرض ماه جاری شمسی را انتخاب می‌کنیم
            Try
                Dim pc As New System.Globalization.PersianCalendar()
                Dim currentMonth = pc.GetMonth(DateTime.Now)
                cmbMonth.SelectedIndex = currentMonth - 1
            Catch
                cmbMonth.SelectedIndex = 0
            End Try
        End Sub

        Private Sub CmbMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMonth.SelectedIndexChanged
            lstEntries.Items.Clear()
            If cmbMonth.SelectedIndex < 0 Then Return

            Dim selectedMonth = cmbMonth.SelectedItem.ToString()
            Try
                Dim dt = service.GetEntries()
                For Each row As DataRow In dt.Rows
                    Dim entryDate = Convert.ToDateTime(row("EntryDate"))
                    Dim persianDate = PersianDateHelper.ToPersian(entryDate)
                    Dim parts = persianDate.Split("/"c)
                    If parts.Length = 3 AndAlso parts(1) = selectedMonth Then
                        lstEntries.Items.Add(New SanadListItem(row))
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اسناد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnMerge_Click(sender As Object, e As EventArgs) Handles btnMerge.Click
            If lstEntries.SelectedItems.Count < 2 Then
                MessageBox.Show("لطفا حداقل دو سند را برای ادغام انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItems = lstEntries.SelectedItems.Cast(Of SanadListItem)().ToList()
            For Each item In selectedItems
                If item.AdamVirayesh Then
                    MessageBox.Show(
                        "سند شماره « " & item.ReferenceNumber & " » قفل شده است و امکان ادغام آن وجود ندارد. لطفا ابتدا قفل آن را باز کنید.",
                        "سند قفل شده", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            Dim ans = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این " & selectedItems.Count & " سند را در یکدیگر ادغام کنید؟ این عملیات غیرقابل بازگشت است.",
                "تایید ادغام", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ans <> DialogResult.Yes Then Return

            Try
                ' پیدا کردن بزرگترین شماره سند که دارای بزرگترین تاریخ است
                Dim targetItem As SanadListItem = Nothing
                For Each item In selectedItems
                    If targetItem Is Nothing Then
                        targetItem = item
                    Else
                        If item.EntryDate > targetItem.EntryDate Then
                            targetItem = item
                        ElseIf item.EntryDate = targetItem.EntryDate Then
                             Dim itemRef As Long = 0
                             Dim targetRefVal As Long = 0
                             Long.TryParse(item.ReferenceNumber, itemRef)
                             Long.TryParse(targetItem.ReferenceNumber, targetRefVal)
                             If itemRef > targetRefVal Then
                                 targetItem = item
                             End If
                        End If
                    End If
                Next

                ' ادغام خطوط سندها
                Dim combinedLines As New List(Of AccountingEntryLine)()
                For Each item In selectedItems
                    Dim dtDetails = service.GetEntryDetails(item.EntryID)
                    For Each row As DataRow In dtDetails.Rows
                        Dim accountId = Convert.ToInt32(row("AccountID"))
                        Dim debit = Convert.ToDecimal(If(row("DebitAmount") Is DBNull.Value, 0D, row("DebitAmount")))
                        Dim credit = Convert.ToDecimal(If(row("CreditAmount") Is DBNull.Value, 0D, row("CreditAmount")))
                        Dim shenavarId = If(row("ShenavarID") Is DBNull.Value, 0, Convert.ToInt32(row("ShenavarID")))
                        Dim sharhRadif = If(row("SharhRadif") Is DBNull.Value, "", Convert.ToString(row("SharhRadif")))
                        Dim transNum = If(row("TransactionNumber") Is DBNull.Value, "", Convert.ToString(row("TransactionNumber")))
                        Dim transDate = If(row("TransactionDate") Is DBNull.Value, "", Convert.ToString(row("TransactionDate")))

                        combinedLines.Add(New AccountingEntryLine(accountId, debit, credit, 0, shenavarId, sharhRadif, transNum, transDate))
                    Next
                Next

                ' شماره‌گذاری مجدد ردیف‌ها از ۱ تا N
                For i = 0 To combinedLines.Count - 1
                    combinedLines(i).LineNumber = i + 1
                Next

                ' محاسبه جمع‌ها و تعادل
                Dim totalBed As Decimal = 0
                Dim totalBes As Decimal = 0
                For Each line In combinedLines
                    totalBed += line.DebitAmount
                    totalBes += line.CreditAmount
                Next
                Dim taeaz = If(totalBed = totalBes, "تراز", If(totalBed > totalBes, "بدهکار", "بستانکار"))

                Dim updatedBy = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.UserID, 1)

                ' بروزرسانی سند هدف با خطوط ادغام شده
                service.UpdateEntry(targetItem.EntryID, targetItem.EntryDate, targetItem.Description, targetItem.ReferenceNumber, updatedBy, combinedLines, totalBed, totalBes, taeaz)

                ' حذف موقت بقیه اسناد (سندهای مبدا)
                For Each item In selectedItems
                    If item.EntryID <> targetItem.EntryID Then
                        service.SetEntryStatus(item.EntryID, "سند موقت - حذف موقت")
                    End If
                Next

                ' نمایش پیغام موفقیت طبق فرمت دقیق درخواستی
                Dim mergedRefs = String.Join("، ", selectedItems.Select(Function(x) x.ReferenceNumber).OrderBy(Function(n)
                                                                                                              Dim val As Long = 0
                                                                                                              Long.TryParse(n, val)
                                                                                                              Return val
                                                                                                          End Function))
                Dim targetRef = targetItem.ReferenceNumber
                Dim targetDateStr = PersianDateHelper.ToPersian(targetItem.EntryDate)

                Dim successMsg = "اسناد شماره « " & mergedRefs & " » با هم ادغام شده و در سند شماره « " & targetRef & " » به تاریخ " & targetDateStr & " قرار گرفت. " &
                                  "لازم است که مدارک فیزیکی اسناد ادغام شده را هم به سند شماره « " & targetRef & " » به تاریخ " & targetDateStr & " پیوست نمایید."

                MessageBox.Show(successMsg, "ادغام اسناد", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در فرآیند ادغام اسناد: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
