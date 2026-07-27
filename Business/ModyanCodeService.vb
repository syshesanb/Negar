Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Negar.Data

Namespace Negar.Business
    Public Class ModyanCodeService
        Public Function GetModyanCodes() As DataTable
            Return Sql.ExecuteTable("SELECT CodeID, ModyanCode, Description, CategoryName, TaxRate, IsActive FROM ModyanCodes ORDER BY CategoryName, ModyanCode")
        End Function

        Public Function SaveModyanCode(codeId As Integer?, modyanCode As String, description As String, categoryName As String, taxRate As Decimal, isActive As Boolean) As Integer
            Dim activeVal = If(isActive, 1, 0)
            If codeId.HasValue AndAlso codeId.Value > 0 Then
                Sql.ExecuteNonQuery(
                    "UPDATE ModyanCodes SET ModyanCode = ?, Description = ?, CategoryName = ?, TaxRate = ?, IsActive = ? WHERE CodeID = ?",
                    modyanCode.Trim(), description.Trim(), categoryName.Trim(), taxRate, activeVal, codeId.Value
                )
                Return codeId.Value
            Else
                Return Sql.ExecuteIdentity(
                    "INSERT INTO ModyanCodes (ModyanCode, Description, CategoryName, TaxRate, IsActive) VALUES (?, ?, ?, ?, ?)",
                    modyanCode.Trim(), description.Trim(), categoryName.Trim(), taxRate, activeVal
                )
            End If
        End Function

        Public Sub DeleteModyanCode(codeId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM ModyanCodes WHERE CodeID = ?", codeId)
        End Sub

        Public Sub DownloadModyanCodes()
            ' Clear existing first to avoid duplicate unique keys
            Sql.ExecuteNonQuery("DELETE FROM ModyanCodes")

            ' Seed only General / Category Modyan Codes (کدهای عمومی سامانه مودیان)
            Dim mockCodes As New List(Of Tuple(Of String, String, String, Decimal))()
            mockCodes.Add(Tuple.Create("29012345678901", "انواع لپ تاپ و نوت بوک", "لوازم الکترونیکی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678902", "انواع گوشی تلفن همراه", "لوازم الکترونیکی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678903", "انواع تبلت و لوازم جانبی رایانه", "لوازم الکترونیکی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678904", "شیر پاستوریزه و لبنیات مایع", "مواد غذایی", 0.00D))
            mockCodes.Add(Tuple.Create("29012345678905", "روغن های گیاهی مایع و جامد خوراکی", "مواد غذایی", 0.00D))
            mockCodes.Add(Tuple.Create("29012345678906", "انواع تایر و لاستیک خودرو سواری و باری", "لوازم یدکی خودرو", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678907", "خدمات مشاوره مالی، حسابداری و حسابرسی", "خدمات عمومی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678908", "خدمات پزشکی و درمانی عمومی و تخصصی", "خدمات درمانی", 0.00D))
            mockCodes.Add(Tuple.Create("29012345678909", "انواع مصالح ساختمانی (سیمان، آهن آلات)", "مصالح ساختمانی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678910", "کاغذ تحریر و لوازم تحریر اداری", "لوازم تحریر", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678911", "انواع لوازم خانگی برقی و غیر برقی", "لوازم خانگی", 0.10D))
            mockCodes.Add(Tuple.Create("29012345678912", "خدمات طراحی وبسایت و فناوری اطلاعات", "خدمات عمومی", 0.10D))

            For Each item In mockCodes
                Try
                    Sql.ExecuteNonQuery(
                        "INSERT OR IGNORE INTO ModyanCodes (ModyanCode, Description, CategoryName, TaxRate, IsActive) VALUES (?, ?, ?, ?, 1)",
                        item.Item1, item.Item2, item.Item3, item.Item4
                    )
                Catch
                End Try
            Next
        End Sub
    End Class
End Namespace
