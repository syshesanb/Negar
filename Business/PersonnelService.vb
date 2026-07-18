Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class PersonnelService
        ' departmentFilter: 0 = All, 1 = System, 2 = Accounting, 3 = Warehousing
        Public Function GetPersonnel(Optional departmentFilter As Integer = 0) As DataTable
            Dim query As String = "SELECT PersonnelID, FullName, Role, NationalCode, Phone, Department, IsActive FROM Personnel"
            If departmentFilter > 0 Then
                query &= " WHERE Department = " & departmentFilter
            End If
            query &= " ORDER BY FullName"
            Return Sql.ExecuteTable(query)
        End Function

        Public Function SavePersonnel(id As Integer?, fullName As String, role As String, nationalCode As String, phone As String, department As Integer, isActive As Boolean) As Integer
            Dim activeVal = If(isActive, 1, 0)
            
            If id.HasValue AndAlso id.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Personnel SET FullName = ?, Role = ?, NationalCode = ?, Phone = ?, Department = ?, IsActive = ? WHERE PersonnelID = ?",
                                    fullName, role, nationalCode, phone, department, activeVal, id.Value)
                Return id.Value
            Else
                Return Sql.ExecuteIdentity("INSERT INTO Personnel (FullName, Role, NationalCode, Phone, Department, IsActive) VALUES (?, ?, ?, ?, ?, ?)",
                                           fullName, role, nationalCode, phone, department, activeVal)
            End If
        End Function

        Public Sub DeletePersonnel(id As Integer)
            Sql.ExecuteNonQuery("DELETE FROM Personnel WHERE PersonnelID = ?", id)
        End Sub
    End Class
End Namespace
