Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class PermissionPreset
        Public Property PresetID As Integer
        Public Property PresetName As String
        Public Property Description As String
        Public Property PermissionsData As String
    End Class

    Public Class PermissionPresetService
        Public Function GetPresets() As List(Of PermissionPreset)
            Dim list As New List(Of PermissionPreset)()
            Try
                Dim dt = Sql.ExecuteTable("SELECT PresetID, PresetName, Description, PermissionsData FROM PermissionPresets ORDER BY PresetID ASC")
                If dt IsNot Nothing Then
                    For Each row As DataRow In dt.Rows
                        Dim p As New PermissionPreset()
                        p.PresetID = Convert.ToInt32(row("PresetID"))
                        p.PresetName = Convert.ToString(row("PresetName"))
                        p.Description = Convert.ToString(If(row.IsNull("Description"), "", row("Description")))
                        p.PermissionsData = Convert.ToString(If(row.IsNull("PermissionsData"), "", row("PermissionsData")))
                        list.Add(p)
                    Next
                End If
            Catch
            End Try
            Return list
        End Function

        Public Sub SavePreset(presetName As String, description As String, permissionsData As String)
            If String.IsNullOrWhiteSpace(presetName) Then
                Throw New InvalidOperationException("نام الگوی پیش‌فرض الزامی است.")
            End If

            Dim existingIdObj = Sql.ExecuteScalar("SELECT PresetID FROM PermissionPresets WHERE PresetName = ?", presetName.Trim())
            If existingIdObj IsNot Nothing AndAlso Not Convert.IsDBNull(existingIdObj) Then
                Dim id = Convert.ToInt32(existingIdObj)
                Sql.ExecuteNonQuery("UPDATE PermissionPresets SET Description = ?, PermissionsData = ? WHERE PresetID = ?",
                                    description, permissionsData, id)
            Else
                Sql.ExecuteNonQuery("INSERT INTO PermissionPresets (PresetName, Description, PermissionsData) VALUES (?, ?, ?)",
                                    presetName.Trim(), description, permissionsData)
            End If
        End Sub

        Public Sub DeletePreset(presetId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM PermissionPresets WHERE PresetID = ?", presetId)
        End Sub
    End Class
End Namespace
