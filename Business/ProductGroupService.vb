Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class ProductGroupService

        ''' <summary>
        ''' Gets all product groups for a given company.
        ''' </summary>
        Public Function GetAll(companyId As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT * FROM ProductGroups WHERE CompanyID = ? ORDER BY GroupCode ASC", companyId)
        End Function

        ''' <summary>
        ''' Gets the maximum depth levels configured for the company (defaults to 3 if not found).
        ''' </summary>
        Public Function GetMaxLevels(companyId As Integer) As Integer
            Dim val = Sql.ExecuteScalar("SELECT ProductGroupLevels FROM Companies WHERE CompanyID = ?", companyId)
            If val Is Nothing OrElse val Is DBNull.Value Then
                Return 3
            End If
            Dim lvls = Convert.ToInt32(val)
            If lvls < 2 Then Return 2
            If lvls > 5 Then Return 5
            Return lvls
        End Function

        ''' <summary>
        ''' Gets a single product group by ID.
        ''' </summary>
        Public Function GetById(groupId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM ProductGroups WHERE GroupID = ?", groupId)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets groups under a specific parent ID. If parentId is null, returns root groups.
        ''' </summary>
        Public Function GetByParent(companyId As Integer, parentId As Integer?) As DataTable
            If parentId.HasValue Then
                Return Sql.ExecuteTable("SELECT * FROM ProductGroups WHERE CompanyID = ? AND ParentID = ? ORDER BY GroupCode ASC", companyId, parentId.Value)
            Else
                Return Sql.ExecuteTable("SELECT * FROM ProductGroups WHERE CompanyID = ? AND ParentID IS NULL ORDER BY GroupCode ASC", companyId)
            End If
        End Function

        ''' <summary>
        ''' Saves or updates a product group.
        ''' </summary>
        Public Function Save(groupId As Integer?, companyId As Integer, parentId As Integer?, groupCode As String, groupName As String, level As Integer, isActive As Boolean) As Integer
            ' Validate Level constraint
            Dim maxLevels = GetMaxLevels(companyId)
            If level < 0 OrElse level >= maxLevels Then
                Throw New InvalidOperationException($"خطا: حداکثر تعداد سطوح گروه‌بندی برای این شرکت {maxLevels} سطح تنظیم شده است. امکان ثبت در سطح {level + 1} وجود ندارد.")
            End If

            ' Check if code is already used by another sibling
            Dim checkSql As String
            Dim existsObj As Object
            If groupId.HasValue AndAlso groupId.Value > 0 Then
                If parentId.HasValue Then
                    checkSql = "SELECT GroupID FROM ProductGroups WHERE CompanyID = ? AND ParentID = ? AND GroupCode = ? AND GroupID <> ?"
                    existsObj = Sql.ExecuteScalar(checkSql, companyId, parentId.Value, groupCode, groupId.Value)
                Else
                    checkSql = "SELECT GroupID FROM ProductGroups WHERE CompanyID = ? AND ParentID IS NULL AND GroupCode = ? AND GroupID <> ?"
                    existsObj = Sql.ExecuteScalar(checkSql, companyId, groupCode, groupId.Value)
                End If
            Else
                If parentId.HasValue Then
                    checkSql = "SELECT GroupID FROM ProductGroups WHERE CompanyID = ? AND ParentID = ? AND GroupCode = ?"
                    existsObj = Sql.ExecuteScalar(checkSql, companyId, parentId.Value, groupCode)
                Else
                    checkSql = "SELECT GroupID FROM ProductGroups WHERE CompanyID = ? AND ParentID IS NULL AND GroupCode = ?"
                    existsObj = Sql.ExecuteScalar(checkSql, companyId, groupCode)
                End If
            End If

            If existsObj IsNot Nothing AndAlso Not Convert.IsDBNull(existsObj) Then
                Throw New InvalidOperationException($"خطا: کد گروه «{groupCode}» در این سطح تکراری است.")
            End If

            ' Insert or Update
            Dim activeVal = If(isActive, 1, 0)
            If groupId.HasValue AndAlso groupId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE ProductGroups SET ParentID = ?, GroupCode = ?, GroupName = ?, Level = ?, IsActive = ? WHERE GroupID = ?",
                                    If(parentId.HasValue, CObj(parentId.Value), DBNull.Value), groupCode, groupName, level, activeVal, groupId.Value)
                Return groupId.Value
            Else
                Dim newId = Sql.ExecuteIdentity("INSERT INTO ProductGroups (CompanyID, ParentID, GroupCode, GroupName, Level, IsActive) VALUES (?, ?, ?, ?, ?, ?)",
                                                companyId, If(parentId.HasValue, CObj(parentId.Value), DBNull.Value), groupCode, groupName, level, activeVal)
                Return newId
            End If
        End Function

        ''' <summary>
        ''' Deletes a product group. Throws if group has children or if products are linked to it.
        ''' </summary>
        Public Sub Delete(groupId As Integer)
            ' Check if group has children
            Dim childCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM ProductGroups WHERE ParentID = ?", groupId), 0))
            If childCount > 0 Then
                Throw New InvalidOperationException("امکان حذف این گروه وجود ندارد زیرا دارای زیرگروه می‌باشد.")
            End If

            Try
                Dim compId = SessionContext.CurrentCompanyID
                If compId.HasValue Then
                    Sql.ExecuteNonQuery("DELETE FROM ProductGroups WHERE GroupID = ? AND (CompanyID = ? OR CompanyID IS NULL)", groupId, compId.Value)
                Else
                    Sql.ExecuteNonQuery("DELETE FROM ProductGroups WHERE GroupID = ?", groupId)
                End If
            Catch ex As Exception
                Throw New InvalidOperationException("خطا در حذف گروه کالا: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Computes the next available sequential code for a parent.
        ''' For level 0: "01", "02", ...
        ''' For sub-levels: parentCode + "01", parentCode + "02", ...
        ''' </summary>
        Public Function GetNextAvailableCode(companyId As Integer, parentId As Integer?) As String
            Dim parentCode = ""
            Dim siblings As DataTable

            If parentId.HasValue Then
                Dim parentRow = GetById(parentId.Value)
                If parentRow IsNot Nothing Then
                    parentCode = Convert.ToString(parentRow("GroupCode"))
                End If
                siblings = Sql.ExecuteTable("SELECT GroupCode FROM ProductGroups WHERE CompanyID = ? AND ParentID = ? ORDER BY GroupCode DESC", companyId, parentId.Value)
            Else
                siblings = Sql.ExecuteTable("SELECT GroupCode FROM ProductGroups WHERE CompanyID = ? AND ParentID IS NULL ORDER BY GroupCode DESC", companyId)
            End If

            Dim lastCode = ""
            If siblings.Rows.Count > 0 Then
                lastCode = Convert.ToString(siblings.Rows(0)("GroupCode"))
            End If

            Dim nextSeq = 1
            If Not String.IsNullOrEmpty(lastCode) Then
                ' Get the last 2 digits of the code
                If lastCode.Length >= 2 Then
                    Dim lastTwo = lastCode.Substring(lastCode.Length - 2)
                    Dim seq As Integer
                    If Integer.TryParse(lastTwo, seq) Then
                        nextSeq = seq + 1
                    End If
                End If
            End If

            Return parentCode & nextSeq.ToString("D2")
        End Function
    End Class
End Namespace
