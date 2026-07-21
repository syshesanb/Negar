Imports System
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class WarehouseLocationEditForm
        Inherits Form

        Public Property NodeTitle As String
            Get
                Return txtTitle.Text.Trim()
            End Get
            Set(value As String)
                txtTitle.Text = value
            End Set
        End Property

        Public Property NodeCode As String
            Get
                Return txtCode.Text.Trim()
            End Get
            Set(value As String)
                txtCode.Text = value
            End Set
        End Property

        Public Sub New(locationTypeName As String, isEdit As Boolean, defaultTitle As String, defaultCode As String)
            InitializeComponent()
            ThemeHelper.ApplyFormTheme(Me)

            If isEdit Then
                Me.Text = "ویرایش " & locationTypeName
            Else
                Me.Text = "افزودن " & locationTypeName
            End If

            NodeTitle = defaultTitle
            NodeCode = defaultCode
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(NodeTitle) Then
                MessageBox.Show("لطفا عنوان را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If String.IsNullOrWhiteSpace(NodeCode) Then
                MessageBox.Show("لطفا کد اختصاری را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
