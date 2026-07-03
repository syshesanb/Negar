Option Strict Off
Option Explicit On

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SQLite

Namespace Sys_Hes_Anb.Data
    Public Module Db
        Public ReadOnly Property ConnectionString As String
            Get
                Dim settings = ConfigurationManager.ConnectionStrings("SysHesAnbDb")
                If settings Is Nothing OrElse String.IsNullOrWhiteSpace(settings.ConnectionString) Then
                    Throw New InvalidOperationException("Connection string 'SysHesAnbDb' was not found.")
                End If

                Return settings.ConnectionString
            End Get
        End Property

        Public Function OpenConnection() As SQLiteConnection
            Dim connection As New SQLiteConnection(ConnectionString)
            connection.Open()
            Return connection
        End Function
    End Module
End Namespace
