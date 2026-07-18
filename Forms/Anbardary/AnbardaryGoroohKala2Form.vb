Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class AnbardaryGoroohKala2Form
        Inherits Form

        Private ReadOnly _service As New ProductGroupService()
        Private _groupId As Integer?
        Private _parentId As Integer?
        Private _level As Integer = 0

        Public Sub New(groupId As Integer?, parentId As Integer?)
            InitializeComponent()
            _groupId = groupId
            _parentId = parentId
        End Sub

        Private Sub AnbardaryGoroohKala2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            Dim companyId = SessionContext.CurrentCompanyID

            If _groupId.HasValue AndAlso _groupId.Value > 0 Then
                ' Editing mode
                Me.Text = "ویرایش گروه کالا"
                Dim row = _service.GetById(_groupId.Value)
                If row IsNot Nothing Then
                    txtGroupName.Text = Convert.ToString(row("GroupName"))
                    txtGroupCode.Text = Convert.ToString(row("GroupCode"))
                    chkActive.Checked = (Convert.ToInt32(row("IsActive")) = 1)
                    _level = Convert.ToInt32(row("Level"))
                    
                    Dim pVal = row("ParentID")
                    If pVal IsNot Nothing AndAlso pVal IsNot DBNull.Value Then
                        _parentId = Convert.ToInt32(pVal)
                        Dim parentRow = _service.GetById(_parentId.Value)
                        If parentRow IsNot Nothing Then
                            txtParent.Text = $"{parentRow("GroupCode")} - {parentRow("GroupName")}"
                        End If
                    Else
                        txtParent.Text = "گروه اصلی"
                    End If
                End If
            Else
                ' Creating mode
                Me.Text = "تعریف گروه کالا جدید"
                chkActive.Checked = True

                If _parentId.HasValue AndAlso _parentId.Value > 0 Then
                    Dim parentRow = _service.GetById(_parentId.Value)
                    If parentRow IsNot Nothing Then
                        txtParent.Text = $"{parentRow("GroupCode")} - {parentRow("GroupName")}"
                        _level = Convert.ToInt32(parentRow("Level")) + 1
                    End If
                Else
                    txtParent.Text = "گروه اصلی"
                    _level = 0
                End If

                Try
                    txtGroupCode.Text = _service.GetNextAvailableCode(companyId, _parentId)
                Catch ex As Exception
                    txtGroupCode.Text = ""
                End Try
            End If
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim groupName = txtGroupName.Text.Trim()
            If String.IsNullOrEmpty(groupName) Then
                MessageBox.Show("لطفاً نام گروه کالا را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtGroupName.Focus()
                Return
            End If

            Dim companyId = SessionContext.CurrentCompanyID
            Dim groupCode = txtGroupCode.Text.Trim()

            Try
                _service.Save(_groupId, companyId, _parentId, groupCode, groupName, _level, chkActive.Checked)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی گروه کالا: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
