Option Strict Off
Option Explicit On

Imports System
Imports System.Security.Cryptography
Imports System.Text

Namespace Negar.Business
    Public Module PasswordHasher
        Public Function Hash(input As String) As String
            Using sha = SHA256.Create()
                Dim bytes = Encoding.UTF8.GetBytes(If(input, String.Empty))
                Dim digest = sha.ComputeHash(bytes)
                Dim builder As New StringBuilder()
                For Each value In digest
                    builder.Append(value.ToString("x2"))
                Next
                Return builder.ToString()
            End Using
        End Function
    End Module
End Namespace
