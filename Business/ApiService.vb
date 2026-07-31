Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Negar.Data

Namespace Negar.Business
    Public Class ApiService
        Sub New()
            EnsureTables()
        End Sub

        Private Sub EnsureTables()
            Try
                ' 1. ApiKeys
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ApiKeys (" &
                    "KeyID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ClientName TEXT, " &
                    "ApiKey TEXT, " &
                    "ApiSecret TEXT, " &
                    "AccessLevel TEXT DEFAULT 'فروشگاه آنلاین', " &
                    "Status TEXT DEFAULT 'فعال', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim keyCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ApiKeys"), 0))
                If keyCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ApiKeys (CompanyID, ClientName, ApiKey, ApiSecret, AccessLevel, Status) " &
                        "VALUES (1, 'فروشگاه اینترنتی ووکامرس (WooCommerce Store)', 'ngr_live_99882103487123', 'sec_4455881122339900', 'فروشگاه آنلاین', 'فعال')"
                    )
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ApiKeys (CompanyID, ClientName, ApiKey, ApiSecret, AccessLevel, Status) " &
                        "VALUES (1, 'کارتخوان‌های اندرویدی ویزیتوران (Mobile POS)', 'ngr_mpos_77123490182344', 'sec_7711223344556677', 'پوز سیار', 'فعال')"
                    )
                End If

                ' 2. ApiLogs
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ApiLogs (" &
                    "LogID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ClientName TEXT, " &
                    "Endpoint TEXT, " &
                    "HttpMethod TEXT DEFAULT 'POST', " &
                    "StatusCode INTEGER DEFAULT 200, " &
                    "LatencyMs INTEGER DEFAULT 45, " &
                    "RequestIp TEXT DEFAULT '185.143.232.10', " &
                    "LogDate TEXT);"
                )

                Dim logCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ApiLogs"), 0))
                If logCount = 0 Then
                    Dim nowStr = PersianDateHelper.ToPersian(DateTime.Now)
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ApiLogs (CompanyID, ClientName, Endpoint, HttpMethod, StatusCode, LatencyMs, RequestIp, LogDate) " &
                        "VALUES (1, 'فروشگاه اینترنتی ووکامرس', '/api/v1/orders/sync', 'POST', 200, 38, '185.143.232.10', ?)",
                        nowStr
                    )
                End If

                ' 3. ApiOrders
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ApiOrders (" &
                    "OrderID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "ExternalOrderCode TEXT, " &
                    "CustomerName TEXT, " &
                    "TotalAmount REAL DEFAULT 1500000, " &
                    "PaymentMethod TEXT DEFAULT 'درگاه آنلاین بانکی', " &
                    "SyncStatus TEXT DEFAULT 'ثبت در انبار نگار', " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Dim orderCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ApiOrders"), 0))
                If orderCount = 0 Then
                    Sql.ExecuteNonQuery(
                        "INSERT INTO ApiOrders (CompanyID, ExternalOrderCode, CustomerName, TotalAmount, PaymentMethod, SyncStatus) " &
                        "VALUES (1, 'WEB-9901', 'مهندس کامران احمدی', 3850000, 'درگاه آنلاین بانکی', 'ثبت در انبار نگار')"
                    )
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetApiKeys(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, KeyID, ClientName, ApiKey, ApiSecret, AccessLevel, Status, CreatedAt FROM ApiKeys WHERE CompanyID = ? ORDER BY KeyID ASC", companyID)
        End Function

        Public Function GetApiLogs(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, LogID, ClientName, Endpoint, HttpMethod, StatusCode, LatencyMs, RequestIp, LogDate FROM ApiLogs WHERE CompanyID = ? ORDER BY LogID DESC", companyID)
        End Function

        Public Function GetApiOrders(companyID As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT '' AS colRowIndex, OrderID, ExternalOrderCode, CustomerName, TotalAmount, PaymentMethod, SyncStatus, CreatedAt FROM ApiOrders WHERE CompanyID = ? ORDER BY OrderID DESC", companyID)
        End Function

        Public Sub SaveApiKey(id As Integer, companyID As Integer, clientName As String, accessLvl As String)
            If id <= 0 Then
                Dim key = "ngr_live_" & Guid.NewGuid().ToString("N").Substring(0, 14)
                Dim secret = "sec_" & Guid.NewGuid().ToString("N").Substring(0, 16)
                Sql.ExecuteNonQuery(
                    "INSERT INTO ApiKeys (CompanyID, ClientName, ApiKey, ApiSecret, AccessLevel, Status) " &
                    "VALUES (?, ?, ?, ?, ?, 'فعال')",
                    companyID, clientName, key, secret, accessLvl
                )
            End If
        End Sub

        Public Function SimulateStoreOrderSync(companyID As Integer, orderCode As String, customerName As String, totalAmount As Double, payMethod As String) As Boolean
            Try
                Sql.ExecuteNonQuery(
                    "INSERT INTO ApiOrders (CompanyID, ExternalOrderCode, CustomerName, TotalAmount, PaymentMethod, SyncStatus) " &
                    "VALUES (?, ?, ?, ?, ?, 'ثبت در انبار نگار')",
                    companyID, orderCode, customerName, totalAmount, payMethod
                )

                Dim nowStr = PersianDateHelper.ToPersian(DateTime.Now)
                Sql.ExecuteNonQuery(
                    "INSERT INTO ApiLogs (CompanyID, ClientName, Endpoint, HttpMethod, StatusCode, LatencyMs, RequestIp, LogDate) " &
                    "VALUES (?, 'تست شبیه‌ساز API', '/api/v1/orders/sync', 'POST', 200, 22, '127.0.0.1', ?)",
                    companyID, nowStr
                )
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace
