Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business

    Public Class PaymentService

        ' پرداخت‌های فاکتور

        Public Function AddPayment(invoiceId As Integer, paymentType As String,
                                   amount As Decimal, paymentDate As Date,
                                   dueDate As Date?,
                                   description As String) As Integer
            Dim user = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.Username, "سیستم")
            Dim dueDateStr As Object = If(dueDate.HasValue, CObj(dueDate.Value.ToString("yyyy-MM-dd")), DBNull.Value)
            Sql.ExecuteNonQuery(
                "INSERT INTO PurchaseInvoicePayments " &
                "(PurchaseInvoiceID, PaymentDate, PaymentType, Amount, DueDate, Description, CreatedBy) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?)",
                invoiceId, paymentDate.ToString("yyyy-MM-dd"),
                paymentType, amount, dueDateStr, description, user)
            Return Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))
        End Function

        Public Function GetPaymentById(paymentId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM PurchaseInvoicePayments WHERE PaymentID = ?", paymentId)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub UpdatePayment(paymentId As Integer, paymentType As String,
                                  amount As Decimal, paymentDate As Date,
                                  dueDate As Date?, description As String)
            Dim user = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.Username, "سیستم")
            Dim dueDateStr As Object = If(dueDate.HasValue, CObj(dueDate.Value.ToString("yyyy-MM-dd")), DBNull.Value)
            Sql.ExecuteNonQuery(
                "UPDATE PurchaseInvoicePayments SET PaymentDate = ?, PaymentType = ?, Amount = ?, DueDate = ?, Description = ?, CreatedBy = ? " &
                "WHERE PaymentID = ?",
                paymentDate.ToString("yyyy-MM-dd"), paymentType, amount, dueDateStr, description, user, paymentId)
        End Sub

        Public Sub DeletePayment(paymentId As Integer)
            Dim checks = GetChecksForPayment(paymentId)
            For Each row As DataRow In checks.Rows
                Dim cid = Convert.ToInt32(row("CheckID"))
                Sql.ExecuteNonQuery("DELETE FROM CheckStatusHistory WHERE CheckID = ?", cid)
            Next
            Sql.ExecuteNonQuery("DELETE FROM PurchaseChecks WHERE PaymentID = ?", paymentId)
            Sql.ExecuteNonQuery("DELETE FROM PurchaseInvoicePayments WHERE PaymentID = ?", paymentId)
        End Sub

        Public Function GetPaymentsForInvoice(invoiceId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT p.PaymentID, p.PaymentDate, p.PaymentType, p.Amount, p.DueDate, p.Description, " &
                "p.CreatedBy, p.CreatedAt, " &
                "c.CheckID, c.CheckNumber, c.BankName, c.Status AS CheckStatus " &
                "FROM PurchaseInvoicePayments p " &
                "LEFT JOIN PurchaseChecks c ON c.PaymentID = p.PaymentID " &
                "WHERE p.PurchaseInvoiceID = ? " &
                "ORDER BY p.PaymentDate, p.PaymentID", invoiceId)
        End Function

        Public Function GetTotalPaid(invoiceId As Integer) As Decimal
            Dim val = Sql.ExecuteScalar(
                "SELECT COALESCE(SUM(Amount),0) FROM PurchaseInvoicePayments " &
                "WHERE PurchaseInvoiceID = ?", invoiceId)
            Return Convert.ToDecimal(If(val, 0D))
        End Function

        Public Structure SettlementStatusInfo
            Public StatusText As String
            Public TextColor As System.Drawing.Color
            Public BackColor As System.Drawing.Color
        End Structure

        Public Function GetSettlementStatus(invoiceId As Integer, totalAmount As Decimal) As SettlementStatusInfo
            Dim info As New SettlementStatusInfo()
            If invoiceId <= 0 OrElse totalAmount <= 0 Then
                info.StatusText = "تسویه نشده"
                info.TextColor = System.Drawing.Color.Red
                info.BackColor = System.Drawing.Color.FromArgb(255, 235, 235)
                Return info
            End If

            Dim totalPayments = GetTotalPaid(invoiceId)
            If totalPayments <= 0 Then
                info.StatusText = "تسویه نشده"
                info.TextColor = System.Drawing.Color.Red
                info.BackColor = System.Drawing.Color.FromArgb(255, 235, 235)
            ElseIf totalPayments < totalAmount Then
                info.StatusText = "تسویه ناقص"
                info.TextColor = System.Drawing.Color.DarkOrange
                info.BackColor = System.Drawing.Color.FromArgb(255, 250, 220)
            Else
                info.StatusText = "تسویه کامل"
                info.TextColor = System.Drawing.Color.DarkGreen
                info.BackColor = System.Drawing.Color.FromArgb(235, 255, 235)
            End If
            Return info
        End Function

        Public Function GetTotalDebt(invoiceId As Integer) As Decimal
            Dim val = Sql.ExecuteScalar(
                "SELECT COALESCE(SUM(Amount),0) FROM PurchaseInvoicePayments " &
                "WHERE PurchaseInvoiceID = ? AND PaymentType = 'بدهی'", invoiceId)
            Return Convert.ToDecimal(If(val, 0D))
        End Function

        Public Sub EnsureAutoDebtIfNeeded(invoiceId As Integer, invoiceTotal As Decimal)
            Dim cnt = Convert.ToInt32(
                Sql.ExecuteScalar("SELECT COUNT(*) FROM PurchaseInvoicePayments WHERE PurchaseInvoiceID = ?", invoiceId))
            If cnt = 0 AndAlso invoiceTotal > 0 Then
                Dim due As Date = DateTime.Today.AddMonths(1)
                AddPayment(invoiceId, "بدهی", invoiceTotal, DateTime.Today, due, "بدهی خودکار - سررسید یک ماهه")
            End If
        End Sub

        ' مدیریت چک‌ها

        Public Function AddCheck(paymentId As Integer, checkNumber As String,
                                 bankName As String, branchName As String,
                                 accountNumber As String, amount As Decimal,
                                 dueDate As Date, notes As String) As Integer
            Dim user = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.Username, "سیستم")
            Sql.ExecuteNonQuery(
                "INSERT INTO PurchaseChecks " &
                "(PaymentID, CheckNumber, BankName, BranchName, AccountNumber, Amount, DueDate, Status, Notes, CreatedBy) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, 'در جریان', ?, ?)",
                paymentId, checkNumber, bankName, branchName, accountNumber,
                amount, dueDate.ToString("yyyy-MM-dd"), notes, user)
            Return Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))
        End Function

        Public Function GetChecksForPayment(paymentId As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM PurchaseChecks WHERE PaymentID = ?", paymentId)
        End Function

        Public Function GetCheckById(checkId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM PurchaseChecks WHERE CheckID = ?", checkId)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub UpdateCheckStatus(checkId As Integer, newStatus As String,
                                     changeDate As Date, description As String,
                                     Optional bounceFee As Decimal = 0,
                                     Optional newCheckId As Integer? = Nothing)
            Dim user = If(SessionContext.CurrentUser IsNot Nothing, SessionContext.CurrentUser.Username, "سیستم")
            Dim oldRow = GetCheckById(checkId)
            Dim oldStatus = If(oldRow IsNot Nothing, Convert.ToString(oldRow("Status")), "")
            Sql.ExecuteNonQuery(
                "INSERT INTO CheckStatusHistory (CheckID, ChangeDate, OldStatus, NewStatus, NewCheckID, BounceFee, Description, ChangedBy) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                checkId, changeDate.ToString("yyyy-MM-dd"), oldStatus, newStatus,
                If(newCheckId.HasValue, CObj(newCheckId.Value), DBNull.Value),
                If(bounceFee > 0, CObj(bounceFee), DBNull.Value),
                description, user)
            Sql.ExecuteNonQuery(
                "UPDATE PurchaseChecks SET Status = ?, BounceFee = ?, ExchangedWithCheckID = ? WHERE CheckID = ?",
                newStatus,
                If(bounceFee > 0, CObj(bounceFee), DBNull.Value),
                If(newCheckId.HasValue, CObj(newCheckId.Value), DBNull.Value),
                checkId)
        End Sub

        Public Function GetCheckStatusHistory(checkId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT * FROM CheckStatusHistory WHERE CheckID = ? ORDER BY ChangedAt", checkId)
        End Function

        ' گزارش‌ها

        Public Function GetAllChecks(Optional statusFilter As String = "",
                                     Optional fromDate As Date? = Nothing,
                                     Optional toDate As Date? = Nothing) As DataTable
            Dim qry = "SELECT c.CheckID, c.CheckNumber, c.BankName, c.BranchName, " &
                      "c.Amount, c.DueDate, c.Status, c.BounceFee, c.Notes, " &
                      "p.PurchaseInvoiceID, " &
                      "i.VendorInvoiceNumber AS InvoiceNumber, " &
                      "v.Name AS VendorName " &
                      "FROM PurchaseChecks c " &
                      "JOIN PurchaseInvoicePayments p ON c.PaymentID = p.PaymentID " &
                      "LEFT JOIN PurchaseInvoices i ON p.PurchaseInvoiceID = i.InvoiceID " &
                      "LEFT JOIN Vendors v ON i.VendorID = v.VendorID " &
                      "WHERE 1=1"
            Dim args As New List(Of Object)
            If Not String.IsNullOrEmpty(statusFilter) Then
                qry &= " AND c.Status = ?"
                args.Add(statusFilter)
            End If
            If fromDate.HasValue Then
                qry &= " AND c.DueDate >= ?"
                args.Add(fromDate.Value.ToString("yyyy-MM-dd"))
            End If
            If toDate.HasValue Then
                qry &= " AND c.DueDate <= ?"
                args.Add(toDate.Value.ToString("yyyy-MM-dd"))
            End If
            qry &= " ORDER BY c.DueDate"
            Return Sql.ExecuteTable(qry, args.ToArray())
        End Function

        Public Function GetUpcomingChecks(daysAhead As Integer) As DataTable
            Dim today = DateTime.Today.ToString("yyyy-MM-dd")
            Dim future = DateTime.Today.AddDays(daysAhead).ToString("yyyy-MM-dd")
            Return Sql.ExecuteTable(
                "SELECT c.CheckID, c.CheckNumber, c.BankName, c.Amount, c.DueDate, " &
                "i.VendorInvoiceNumber AS InvoiceNumber, v.Name AS VendorName " &
                "FROM PurchaseChecks c " &
                "JOIN PurchaseInvoicePayments p ON c.PaymentID = p.PaymentID " &
                "LEFT JOIN PurchaseInvoices i ON p.PurchaseInvoiceID = i.InvoiceID " &
                "LEFT JOIN Vendors v ON i.VendorID = v.VendorID " &
                "WHERE c.Status = 'در جریان' AND c.DueDate BETWEEN ? AND ? " &
                "ORDER BY c.DueDate",
                today, future)
        End Function

        Public Function GetInvoiceSettlementReport(Optional vendorId As Integer? = Nothing) As DataTable
            Dim qry = "SELECT i.InvoiceID, i.VendorInvoiceNumber AS InvoiceNumber, " &
                      "i.InvoiceDate, i.GrandTotal AS InvoiceTotal, " &
                      "v.Name AS VendorName, " &
                      "COALESCE(SUM(CASE WHEN p.PaymentType <> 'بدهی' THEN p.Amount ELSE 0 END),0) AS TotalPaid, " &
                      "COALESCE(SUM(CASE WHEN p.PaymentType = 'بدهی' THEN p.Amount ELSE 0 END),0) AS TotalDebt, " &
                      "CASE WHEN COUNT(p.PaymentID)=0 THEN 'تسویه نشده' " &
                      "WHEN COALESCE(SUM(CASE WHEN p.PaymentType='بدهی' THEN p.Amount ELSE 0 END),0)=0 THEN 'کاملاً تسویه شده' " &
                      "ELSE 'تسویه جزئی' END AS SettlementStatus " &
                      "FROM PurchaseInvoices i " &
                      "LEFT JOIN Vendors v ON i.VendorID = v.VendorID " &
                      "LEFT JOIN PurchaseInvoicePayments p ON i.InvoiceID = p.PurchaseInvoiceID " &
                      "WHERE 1=1"
            Dim args As New List(Of Object)
            If vendorId.HasValue Then
                qry &= " AND i.VendorID = ?"
                args.Add(vendorId.Value)
            End If
            qry &= " GROUP BY i.InvoiceID ORDER BY i.InvoiceDate DESC"
            Return Sql.ExecuteTable(qry, args.ToArray())
        End Function

        Public Function GetVendorBalanceReport() As DataTable
            Return Sql.ExecuteTable(
                "SELECT v.VendorID, v.Name AS VendorName, v.Phone, " &
                "COUNT(DISTINCT i.InvoiceID) AS InvoiceCount, " &
                "COALESCE(SUM(i.GrandTotal),0) AS TotalInvoiced, " &
                "COALESCE(SUM(CASE WHEN p.PaymentType <> 'بدهی' THEN p.Amount ELSE 0 END),0) AS TotalPaid, " &
                "COALESCE(SUM(i.GrandTotal),0) - COALESCE(SUM(CASE WHEN p.PaymentType <> 'بدهی' THEN p.Amount ELSE 0 END),0) AS Balance " &
                "FROM Vendors v " &
                "LEFT JOIN PurchaseInvoices i ON i.VendorID = v.VendorID " &
                "LEFT JOIN PurchaseInvoicePayments p ON i.InvoiceID = p.PurchaseInvoiceID " &
                "GROUP BY v.VendorID ORDER BY Balance DESC")
        End Function

    End Class

End Namespace
