Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class ShenavarService

        Public Function GetByParent(parentId As Integer?) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Dim companyId = SessionContext.CurrentCompanyID.Value
            If parentId.HasValue Then
                Return Sql.ExecuteTable(
                    "SELECT ShenavarID AS AccountID, AccountCode, AccountName, " &
                    "ParentShenavarID AS ParentAccountID, IsActive " &
                    "FROM shenavar WHERE CompanyID = ? AND ParentShenavarID = ? ORDER BY AccountCode",
                    companyId, parentId.Value)
            Else
                Return Sql.ExecuteTable(
                    "SELECT ShenavarID AS AccountID, AccountCode, AccountName, " &
                    "ParentShenavarID AS ParentAccountID, IsActive " &
                    "FROM shenavar WHERE CompanyID = ? AND ParentShenavarID IS NULL ORDER BY AccountCode",
                    companyId)
            End If
        End Function

        Public Function Save(itemId As Integer?, accountCode As String, accountName As String,
                             parentId As Integer?, isActive As Boolean) As Integer
            If Not SessionContext.CurrentCompanyID.HasValue Then
                Throw New InvalidOperationException("ابتدا باید شرکت جاری را انتخاب کنید.")
            End If

            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim parentVal = If(parentId.HasValue, CType(parentId.Value, Object), DBNull.Value)
            Dim excludeId = If(itemId.HasValue AndAlso itemId.Value > 0, itemId.Value, 0)

            Dim dupCount As Integer
            If parentId.HasValue Then
                dupCount = Convert.ToInt32(If(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM shenavar WHERE CompanyID = ? AND AccountCode = ? AND ParentShenavarID = ? AND ShenavarID <> ?",
                    companyId, accountCode, parentId.Value, excludeId), 0))
            Else
                dupCount = Convert.ToInt32(If(Sql.ExecuteScalar(
                    "SELECT COUNT(*) FROM shenavar WHERE CompanyID = ? AND AccountCode = ? AND ParentShenavarID IS NULL AND ShenavarID <> ?",
                    companyId, accountCode, excludeId), 0))
            End If

            If dupCount > 0 Then
                Throw New InvalidOperationException(
                    "کد حساب '" & accountCode & "' در همین سطح قبلاً ثبت شده است." & Environment.NewLine &
                    "لطفاً کد دیگری انتخاب کنید.")
            End If

            If itemId.HasValue AndAlso itemId.Value > 0 Then
                Sql.ExecuteNonQuery(
                    "UPDATE shenavar SET AccountCode = ?, AccountName = ?, ParentShenavarID = ?, IsActive = ? WHERE ShenavarID = ? AND CompanyID = ?",
                    accountCode, accountName, parentVal, isActive, itemId.Value, companyId)
                Return itemId.Value
            End If

            Return Sql.ExecuteIdentity(
                "INSERT INTO shenavar (CompanyID, AccountCode, AccountName, ParentShenavarID, IsActive) VALUES (?, ?, ?, ?, ?)",
                companyId, accountCode, accountName, parentVal, isActive)
        End Function

        Public Sub Delete(itemId As Integer)
            If Not SessionContext.CurrentCompanyID.HasValue Then Return
            Sql.ExecuteNonQuery("DELETE FROM shenavar WHERE ShenavarID = ? AND CompanyID = ?",
                                itemId, SessionContext.CurrentCompanyID.Value)
        End Sub

        Public Function HasChildren(itemId As Integer) As Boolean
            If Not SessionContext.CurrentCompanyID.HasValue Then Return False
            Dim count = Convert.ToInt32(If(Sql.ExecuteScalar(
                "SELECT COUNT(*) FROM shenavar WHERE CompanyID = ? AND ParentShenavarID = ?",
                SessionContext.CurrentCompanyID.Value, itemId), 0))
            Return count > 0
        End Function

        Public Function GetParentId(itemId As Integer) As Integer?
            Dim result = Sql.ExecuteScalar(
                "SELECT ParentShenavarID FROM shenavar WHERE ShenavarID = ?", itemId)
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Return Nothing
            Return Convert.ToInt32(result)
        End Function

        Public Function GetName(itemId As Integer) As String
            Dim result = Sql.ExecuteScalar(
                "SELECT AccountName FROM shenavar WHERE ShenavarID = ?", itemId)
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Return String.Empty
            Return Convert.ToString(result)
        End Function

        ' کد و نام یک حساب شناور را با هم برمی‌گرداند
        Public Function GetItemInfo(itemId As Integer) As Tuple(Of String, String)
            Dim dt = Sql.ExecuteTable(
                "SELECT AccountCode, AccountName FROM shenavar WHERE ShenavarID = ?", itemId)
            If dt.Rows.Count = 0 Then Return Tuple.Create("", "")
            Return Tuple.Create(Convert.ToString(dt.Rows(0)("AccountCode")),
                                Convert.ToString(dt.Rows(0)("AccountName")))
        End Function

        Public Function GetNextSuggestedCode(parentId As Integer?) As String
            If Not SessionContext.CurrentCompanyID.HasValue Then Return "1"
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim maxCodeObj As Object
            If parentId.HasValue Then
                maxCodeObj = Sql.ExecuteScalar(
                    "SELECT MAX(AccountCode) FROM shenavar WHERE CompanyID = ? AND ParentShenavarID = ?",
                    companyId, parentId.Value)
            Else
                maxCodeObj = Sql.ExecuteScalar(
                    "SELECT MAX(AccountCode) FROM shenavar WHERE CompanyID = ? AND ParentShenavarID IS NULL",
                    companyId)
            End If
            If maxCodeObj Is Nothing OrElse Convert.IsDBNull(maxCodeObj) Then Return "1"
            Dim codeStr = Convert.ToString(maxCodeObj).Trim()
            Dim numVal As Long
            If Long.TryParse(codeStr, numVal) Then Return (numVal + 1).ToString()
            Return codeStr
        End Function

        Public Function SearchAll(codeFilter As String, nameFilter As String) As DataTable
            If Not SessionContext.CurrentCompanyID.HasValue Then Return New DataTable()
            Dim companyId = SessionContext.CurrentCompanyID.Value
            Dim query = "SELECT ShenavarID AS AccountID, AccountCode, AccountName, " &
                        "ParentShenavarID AS ParentAccountID, IsActive " &
                        "FROM shenavar WHERE CompanyID = ?"
            Dim params As New System.Collections.Generic.List(Of Object)()
            params.Add(companyId)
            If codeFilter.Length > 0 Then
                query &= " AND AccountCode LIKE ?"
                params.Add("%" & codeFilter & "%")
            End If
            If nameFilter.Length > 0 Then
                query &= " AND AccountName LIKE ?"
                params.Add("%" & nameFilter & "%")
            End If
            query &= " ORDER BY AccountCode"
            Return Sql.ExecuteTable(query, params.ToArray())
        End Function

        Public Function GetShenavarsWithChildren() As HashSet(Of Integer)
            Dim hs As New HashSet(Of Integer)()
            If Not SessionContext.CurrentCompanyID.HasValue Then Return hs
            Dim dt = Sql.ExecuteTable(
                "SELECT DISTINCT ParentShenavarID FROM shenavar WHERE CompanyID = ? AND ParentShenavarID IS NOT NULL",
                SessionContext.CurrentCompanyID.Value)
            For Each r As DataRow In dt.Rows
                If Not r.IsNull("ParentShenavarID") Then
                    hs.Add(Convert.ToInt32(r("ParentShenavarID")))
                End If
            Next
            Return hs
        End Function

    End Class
End Namespace
