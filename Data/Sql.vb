Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SQLite

Namespace Negar.Data
    Public Module Sql
        Public Function ExecuteTable(sql As String, ParamArray values() As Object) As DataTable
            Dim table As New DataTable()

            Using connection = Db.OpenConnection()
                Using command As New SQLiteCommand(sql, connection)
                    AddParameters(command, values)
                    Using adapter As New SQLiteDataAdapter(command)
                        adapter.Fill(table)
                    End Using
                End Using
            End Using

            Return table
        End Function

        Public Function ExecuteScalar(sql As String, ParamArray values() As Object) As Object
            Using connection = Db.OpenConnection()
                Using command As New SQLiteCommand(sql, connection)
                    AddParameters(command, values)
                    Return command.ExecuteScalar()
                End Using
            End Using
        End Function

        Public Function ExecuteNonQuery(sql As String, ParamArray values() As Object) As Integer
            Using connection = Db.OpenConnection()
                Using command As New SQLiteCommand(sql, connection)
                    AddParameters(command, values)
                    Return command.ExecuteNonQuery()
                End Using
            End Using
        End Function

        Public Function ExecuteIdentity(sql As String, ParamArray values() As Object) As Integer
            Using connection = Db.OpenConnection()
                Using command As New SQLiteCommand(sql, connection)
                    AddParameters(command, values)
                    command.ExecuteNonQuery()
                    command.CommandText = "SELECT last_insert_rowid()"
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        End Function

        Private Sub AddParameters(command As SQLiteCommand, values() As Object)
            If values Is Nothing Then Return
            For i As Integer = 0 To values.Length - 1
                Dim value = values(i)
                Dim parameter As SQLiteParameter = command.CreateParameter()
                parameter.ParameterName = "@p" & (i + 1)

                If value Is Nothing OrElse Convert.IsDBNull(value) Then
                    parameter.Value = DBNull.Value
                ElseIf TypeOf value Is String Then
                    parameter.DbType = DbType.String
                    parameter.Value = value
                ElseIf TypeOf value Is Boolean Then
                    parameter.DbType = DbType.Boolean
                    parameter.Value = value
                ElseIf TypeOf value Is DateTime Then
                    parameter.DbType = DbType.DateTime
                    parameter.Value = value
                ElseIf TypeOf value Is Integer OrElse TypeOf value Is Short Then
                    parameter.DbType = DbType.Int32
                    parameter.Value = Convert.ToInt32(value)
                ElseIf TypeOf value Is Long Then
                    parameter.DbType = DbType.Int64
                    parameter.Value = value
                ElseIf TypeOf value Is Decimal Then
                    parameter.DbType = DbType.Decimal
                    parameter.Value = value
                ElseIf TypeOf value Is Double OrElse TypeOf value Is Single Then
                    parameter.DbType = DbType.Double
                    parameter.Value = value
                ElseIf TypeOf value Is Byte() Then
                    parameter.DbType = DbType.Binary
                    parameter.Value = value
                Else
                    parameter.Value = value
                End If

                command.Parameters.Add(parameter)
            Next
        End Sub
    End Module
End Namespace
