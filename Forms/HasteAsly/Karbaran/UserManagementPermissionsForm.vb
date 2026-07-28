Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Partial Class UserManagementPermissionsForm
        Inherits Form

        Private ReadOnly userService As New UserService()
        Private ReadOnly treeService As New PermissionTreeService()
        Private ReadOnly presetService As New PermissionPresetService()
        Private ReadOnly _ordinaryOnly As Boolean
        Private _selectedUserId As Integer?
        Private _isUpdatingTreeState As Boolean = False

        Public Sub New()
            Me.New(False)
        End Sub

        Public Sub New(ordinaryOnly As Boolean)
            _ordinaryOnly = ordinaryOnly
            InitializeComponent()
        End Sub

        Private Sub UserManagementPermissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            If Me.dgvUsers IsNot Nothing Then Me.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255)
            LoadUsers()
            LoadPresetsCombo()
            BuildPermissionTreeUI()
            AdjustLayoutSplitter()
        End Sub

        Private Sub UserManagementPermissionsForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown, MyBase.VisibleChanged, MyBase.Resize
            AdjustLayoutSplitter()
        End Sub

        Private Sub AdjustLayoutSplitter()
            Try
                If Me.Width > 300 Then
                    Dim targetDist As Integer = CInt(Me.Width * 0.38)
                    If targetDist < 450 Then targetDist = 480
                    If targetDist > 650 Then targetDist = 580
                    splitMain.SplitterDistance = targetDist
                Else
                    splitMain.SplitterDistance = 500
                End If
            Catch
            End Try
        End Sub

        Private Sub LoadUsers()
            If _ordinaryOnly Then
                dgvUsers.DataSource = userService.GetUsersByTypes("User")
            Else
                dgvUsers.DataSource = userService.GetUsers()
            End If

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If dgvUsers.Columns.Contains("UserID") Then
                dgvUsers.Columns("UserID").HeaderText = "کد"
                dgvUsers.Columns("UserID").Width = 50
            End If
            If dgvUsers.Columns.Contains("Username") Then
                dgvUsers.Columns("Username").HeaderText = "نام کاربری"
                dgvUsers.Columns("Username").Width = 90
            End If
            If dgvUsers.Columns.Contains("UserType") Then
                dgvUsers.Columns("UserType").HeaderText = "نوع کاربر"
                dgvUsers.Columns("UserType").Width = 85
            End If
            If dgvUsers.Columns.Contains("FullName") Then
                dgvUsers.Columns("FullName").HeaderText = "نام و نام خانوادگی"
                dgvUsers.Columns("FullName").Width = 140
            End If
            If dgvUsers.Columns.Contains("CreatedDate") Then
                dgvUsers.Columns("CreatedDate").HeaderText = "تاریخ ایجاد"
                dgvUsers.Columns("CreatedDate").Width = 95
            End If
            If dgvUsers.Columns.Contains("IsActive") Then
                dgvUsers.Columns("IsActive").HeaderText = "فعال"
                dgvUsers.Columns("IsActive").Width = 50
            End If

            dgvUsers.ClearSelection()
        End Sub

        Private Sub DgvUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUsers.CellFormatting
            If dgvUsers.Columns(e.ColumnIndex).Name = "CreatedDate" Then
                If e.Value IsNot Nothing AndAlso Not Convert.IsDBNull(e.Value) AndAlso TypeOf e.Value Is DateTime Then
                    e.Value = PersianDateHelper.ToPersian(CType(e.Value, DateTime))
                    e.FormattingApplied = True
                End If
            End If
        End Sub

        Private Sub LoadPresetsCombo()
            cmbPresets.Items.Clear()
            Dim presets = presetService.GetPresets()
            For Each p In presets
                cmbPresets.Items.Add(p.PresetName)
            Next
            If cmbPresets.Items.Count > 0 Then
                cmbPresets.SelectedIndex = 0
            End If
        End Sub

        Private Sub BuildPermissionTreeUI()
            _isUpdatingTreeState = True
            tvPermissions.Nodes.Clear()

            Dim rootNodesData = treeService.BuildDynamicTree()
            For Each rData In rootNodesData
                Dim uiNode = CreateTreeNodeRecursive(rData)
                tvPermissions.Nodes.Add(uiNode)
            Next

            tvPermissions.ExpandAll()
            _isUpdatingTreeState = False
        End Sub

        Private Function CreateTreeNodeRecursive(dataNode As PermissionTreeNode) As TreeNode
            Dim uiNode As New TreeNode(dataNode.Title)
            uiNode.Tag = dataNode
            Select Case dataNode.Level
                Case 0
                    uiNode.NodeFont = New Font("Tahoma", 9.5!, FontStyle.Bold)
                    uiNode.ForeColor = Color.FromArgb(0, 50, 120)
                Case 1
                    uiNode.NodeFont = New Font("Tahoma", 9.0!, FontStyle.Bold)
                    uiNode.ForeColor = Color.FromArgb(120, 40, 0)
                Case 2
                    uiNode.NodeFont = New Font("Tahoma", 8.5!, FontStyle.Bold)
                    uiNode.ForeColor = Color.FromArgb(0, 100, 50)
                Case 3
                    uiNode.NodeFont = New Font("Tahoma", 8.5!, FontStyle.Regular)
                    uiNode.ForeColor = Color.FromArgb(60, 60, 60)
                Case 4
                    uiNode.NodeFont = New Font("Tahoma", 8.5!, FontStyle.Regular)
                    uiNode.ForeColor = Color.FromArgb(20, 20, 20)
            End Select

            For Each childData In dataNode.Children
                uiNode.Nodes.Add(CreateTreeNodeRecursive(childData))
            Next

            Return uiNode
        End Function

        Private Sub DgvUsers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUsers.SelectionChanged
            LoadPermissionsForSelectedUser()
        End Sub

        Private Function GetSelectedUserId() As Integer?
            If dgvUsers.CurrentRow Is Nothing OrElse dgvUsers.CurrentRow.IsNewRow Then
                Return Nothing
            End If
            Dim value = dgvUsers.CurrentRow.Cells("UserID").Value
            If value Is Nothing OrElse value Is DBNull.Value Then
                Return Nothing
            End If
            Return Convert.ToInt32(value)
        End Function

        Private Sub LoadPermissionsForSelectedUser()
            Dim userId = GetSelectedUserId()
            If Not userId.HasValue Then
                _selectedUserId = Nothing
                UncheckAllTreeNodes(tvPermissions.Nodes)
                Return
            End If

            _selectedUserId = userId.Value

            ' Query granted keys for this user
            Dim grantedKeys As HashSet(Of String) = GetGrantedPermissionKeysForUser(userId.Value)

            _isUpdatingTreeState = True
            UpdateTreeCheckedStateRecursive(tvPermissions.Nodes, grantedKeys)
            _isUpdatingTreeState = False
        End Sub

        Private Function GetGrantedPermissionKeysForUser(userId As Integer) As HashSet(Of String)
            Dim setKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            Try
                Dim userTypeObj = Sql.ExecuteScalar("SELECT UserType FROM Users WHERE UserID = ?", userId)
                Dim userType = If(userTypeObj IsNot Nothing AndAlso Not Convert.IsDBNull(userTypeObj), Convert.ToString(userTypeObj), "")

                If String.Equals(userType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                    setKeys.Add("ALL")
                    Return setKeys
                End If

                Dim dt = Sql.ExecuteTable("SELECT p.PermissionKey, rp.CanView, rp.CanCreate, rp.CanEdit FROM RolePermissions rp INNER JOIN Permissions p ON rp.PermissionID = p.PermissionID WHERE rp.UserID = ?", userId)
                If dt IsNot Nothing Then
                    For Each row As DataRow In dt.Rows
                        Dim canV = If(row.IsNull("CanView"), False, Convert.ToBoolean(row("CanView")))
                        Dim canC = If(row.IsNull("CanCreate"), False, Convert.ToBoolean(row("CanCreate")))
                        Dim canE = If(row.IsNull("CanEdit"), False, Convert.ToBoolean(row("CanEdit")))
                        If canV OrElse canC OrElse canE Then
                            Dim key = Convert.ToString(row("PermissionKey"))
                            setKeys.Add(key)
                        End If
                    Next
                End If
            Catch
            End Try
            Return setKeys
        End Function

        Private Sub UpdateTreeCheckedStateRecursive(nodes As TreeNodeCollection, grantedKeys As HashSet(Of String))
            Dim isAll = grantedKeys.Contains("ALL")
            For Each node As TreeNode In nodes
                Dim data = TryCast(node.Tag, PermissionTreeNode)
                If data IsNot Nothing AndAlso Not String.IsNullOrEmpty(data.PermissionKey) Then
                    node.Checked = isAll OrElse grantedKeys.Contains(data.PermissionKey)
                Else
                    node.Checked = False
                End If

                If node.Nodes.Count > 0 Then
                    UpdateTreeCheckedStateRecursive(node.Nodes, grantedKeys)
                    ' Parent checked if any child checked
                    If Not isAll Then
                        node.Checked = HasAnyCheckedChild(node)
                    Else
                        node.Checked = True
                    End If
                End If
            Next
        End Sub

        Private Function HasAnyCheckedChild(parentNode As TreeNode) As Boolean
            For Each child As TreeNode In parentNode.Nodes
                If child.Checked OrElse HasAnyCheckedChild(child) Then Return True
            Next
            Return False
        End Function

        Private Sub UncheckAllTreeNodes(nodes As TreeNodeCollection)
            _isUpdatingTreeState = True
            For Each n As TreeNode In nodes
                n.Checked = False
                If n.Nodes.Count > 0 Then UncheckAllTreeNodes(n.Nodes)
            Next
            _isUpdatingTreeState = False
        End Sub

        Private Sub tvPermissions_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles tvPermissions.AfterCheck
            If _isUpdatingTreeState Then Return
            _isUpdatingTreeState = True

            Try
                Dim node = e.Node
                Dim isChecked = node.Checked

                ' 1. Cascade to all children
                CascadeCheckChildren(node.Nodes, isChecked)

                ' 2. Ensure parent checked if checked
                If isChecked Then
                    EnsureParentChecked(node.Parent)
                End If

                ' 3. Dependency Enforcement Engine Check
                CheckDependencyEnforcement(node)
            Finally
                _isUpdatingTreeState = False
            End Try
        End Sub

        Private Sub CascadeCheckChildren(nodes As TreeNodeCollection, isChecked As Boolean)
            For Each child As TreeNode In nodes
                child.Checked = isChecked
                If child.Nodes.Count > 0 Then CascadeCheckChildren(child.Nodes, isChecked)
            Next
        End Sub

        Private Sub EnsureParentChecked(parentNode As TreeNode)
            If parentNode IsNot Nothing Then
                parentNode.Checked = True
                If parentNode.Parent IsNot Nothing Then EnsureParentChecked(parentNode.Parent)
            End If
        End Sub

        Private Sub CheckDependencyEnforcement(targetNode As TreeNode)
            Dim data = TryCast(targetNode.Tag, PermissionTreeNode)
            If data Is Nothing Then Return

            ' Scenario A: Unchecking a node that other checked nodes depend on
            If Not targetNode.Checked AndAlso Not String.IsNullOrEmpty(data.PermissionKey) Then
                Dim affectedNodes As New List(Of TreeNode)()
                FindDependentCheckedNodesRecursive(tvPermissions.Nodes, data.PermissionKey, affectedNodes)

                If affectedNodes.Count > 0 Then
                    Dim names = String.Join(" ، ", affectedNodes.Select(Function(n) n.Text).ToArray())
                    MessageBox.Show("💡 مدیریت هوشمند وابستگی‌ها:" & Environment.NewLine & Environment.NewLine &
                                    "به دلیل لغو مجوز پایه «" & targetNode.Text & "»، مجوزهای وابسته زیر نیز خودکار غیرفعال گردیدند:" & Environment.NewLine &
                                    names, "بررسی وابستگی دسترسی‌ها", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    For Each affInNode In affectedNodes
                        affInNode.Checked = False
                    Next
                End If
            End If

            ' Scenario B: Checking a node that requires other parent dependencies
            If targetNode.Checked AndAlso data.DependsOnKeys.Count > 0 Then
                For Each reqKey In data.DependsOnKeys
                    Dim reqNode = FindTreeNodeByPermKey(tvPermissions.Nodes, reqKey)
                    If reqNode IsNot Nothing AndAlso Not reqNode.Checked Then
                        reqNode.Checked = True
                        EnsureParentChecked(reqNode.Parent)
                        MessageBox.Show("💡 مدیریت هوشمند وابستگی‌ها:" & Environment.NewLine & Environment.NewLine &
                                        "برای فعال‌سازی «" & targetNode.Text & "»، مجوز پیش‌نیاز «" & reqNode.Text & "» نیز به صورت خودکار فعال گردید.",
                                        "اعمال دسترسی پیش‌نیاز", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Next
            End If
        End Sub

        Private Sub FindDependentCheckedNodesRecursive(nodes As TreeNodeCollection, baseKey As String, result As List(Of TreeNode))
            For Each node As TreeNode In nodes
                Dim d = TryCast(node.Tag, PermissionTreeNode)
                If d IsNot Nothing AndAlso node.Checked AndAlso d.DependsOnKeys.Contains(baseKey) Then
                    result.Add(node)
                End If
                If node.Nodes.Count > 0 Then FindDependentCheckedNodesRecursive(node.Nodes, baseKey, result)
            Next
        End Sub

        Private Function FindTreeNodeByPermKey(nodes As TreeNodeCollection, permKey As String) As TreeNode
            For Each node As TreeNode In nodes
                Dim d = TryCast(node.Tag, PermissionTreeNode)
                If d IsNot Nothing AndAlso String.Equals(d.PermissionKey, permKey, StringComparison.OrdinalIgnoreCase) Then
                    Return node
                End If
                If node.Nodes.Count > 0 Then
                    Dim found = FindTreeNodeByPermKey(node.Nodes, permKey)
                    If found IsNot Nothing Then Return found
                End If
            Next
            Return Nothing
        End Function

        Private Sub btnExpandAll_Click(sender As Object, e As EventArgs) Handles btnExpandAll.Click
            tvPermissions.ExpandAll()
        End Sub

        Private Sub btnCollapseAll_Click(sender As Object, e As EventArgs) Handles btnCollapseAll.Click
            tvPermissions.CollapseAll()
        End Sub

        Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            LoadUsers()
            LoadPermissionsForSelectedUser()
        End Sub

        Private Sub btnApplyPreset_Click(sender As Object, e As EventArgs) Handles btnApplyPreset.Click
            If cmbPresets.SelectedItem Is Nothing Then
                MessageBox.Show("لطفاً ابتدا یک الگوی پیش‌فرض را انتخاب کنید.")
                Return
            End If

            Dim presetName = cmbPresets.SelectedItem.ToString()
            Dim presets = presetService.GetPresets()
            Dim matched = presets.FirstOrDefault(Function(p) p.PresetName = presetName)
            If matched Is Nothing Then Return

            _isUpdatingTreeState = True

            If matched.PermissionsData = "ALL" Then
                CheckAllTreeNodesRecursive(tvPermissions.Nodes, True)
            Else
                CheckAllTreeNodesRecursive(tvPermissions.Nodes, False)
                Dim keys = matched.PermissionsData.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)
                Dim setKeys As New HashSet(Of String)(keys, StringComparer.OrdinalIgnoreCase)

                For Each k In setKeys
                    Dim tNode = FindTreeNodeByPermKey(tvPermissions.Nodes, k)
                    If tNode IsNot Nothing Then
                        tNode.Checked = True
                        EnsureParentChecked(tNode.Parent)
                    End If
                Next
            End If

            _isUpdatingTreeState = False
            MessageBox.Show("الگوی «" & presetName & "» با موفقیت بر روی درختواره دسترسی‌ها اعمال گردید." & Environment.NewLine &
                            "برای ثبت قطعی بر روی کاربر، روی دکمه ذخیره کلیک کنید.", "اعمال الگو", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub CheckAllTreeNodesRecursive(nodes As TreeNodeCollection, isChecked As Boolean)
            For Each node As TreeNode In nodes
                node.Checked = isChecked
                If node.Nodes.Count > 0 Then CheckAllTreeNodesRecursive(node.Nodes, isChecked)
            Next
        End Sub

        Private Sub btnSavePreset_Click(sender As Object, e As EventArgs) Handles btnSavePreset.Click
            Dim presetName = Interaction.InputBox("لطفاً نام الگوی دسترسی جدید را وارد کنید:", "ایجاد الگوی پیش‌فرض دسترسی", "الگوی سفارشی جدید")
            If String.IsNullOrWhiteSpace(presetName) Then Return

            Dim checkedKeys As New List(Of String)()
            CollectCheckedPermissionKeysRecursive(tvPermissions.Nodes, checkedKeys)

            If checkedKeys.Count = 0 Then
                MessageBox.Show("هیچ دسترسی در درختواره تیک نخورده است.")
                Return
            End If

            Dim dataStr = String.Join(",", checkedKeys.ToArray())
            presetService.SavePreset(presetName.Trim(), "الگوی ایجاد شده توسط کاربر", dataStr)
            LoadPresetsCombo()
            cmbPresets.SelectedItem = presetName.Trim()
            MessageBox.Show("الگوی پیش‌فرض جدید با موفقیت ذخیره گردید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub CollectCheckedPermissionKeysRecursive(nodes As TreeNodeCollection, list As List(Of String))
            For Each node As TreeNode In nodes
                Dim d = TryCast(node.Tag, PermissionTreeNode)
                If d IsNot Nothing AndAlso node.Checked AndAlso Not String.IsNullOrEmpty(d.PermissionKey) Then
                    If Not list.Contains(d.PermissionKey) Then list.Add(d.PermissionKey)
                End If
                If node.Nodes.Count > 0 Then CollectCheckedPermissionKeysRecursive(node.Nodes, list)
            Next
        End Sub

        Private Sub btnDeletePreset_Click(sender As Object, e As EventArgs) Handles btnDeletePreset.Click
            If cmbPresets.SelectedItem Is Nothing Then Return
            Dim presetName = cmbPresets.SelectedItem.ToString()

            If MessageBox.Show("آیا از حذف الگوی «" & presetName & "» اطمینان دارید؟", "تایید حذف الگو", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim presets = presetService.GetPresets()
                Dim matched = presets.FirstOrDefault(Function(p) p.PresetName = presetName)
                If matched IsNot Nothing Then
                    presetService.DeletePreset(matched.PresetID)
                    LoadPresetsCombo()
                    MessageBox.Show("الگو با موفقیت حذف گردید.")
                End If
            End If
        End Sub

        Private Sub btnSavePermissions_Click(sender As Object, e As EventArgs) Handles btnSavePermissions.Click
            If Not _selectedUserId.HasValue Then
                MessageBox.Show("لطفاً ابتدا یک کاربر را از لیست سمت راست انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Fetch all permission IDs from DB to sync
            Dim dtAllPerms = Sql.ExecuteTable("SELECT PermissionID, PermissionKey FROM Permissions")
            If dtAllPerms Is Nothing OrElse dtAllPerms.Rows.Count = 0 Then Return

            Dim checkedKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            CollectCheckedPermissionKeysRecursive(tvPermissions.Nodes, checkedKeys.ToList())

            ' Save to RolePermissions
            For Each row As DataRow In dtAllPerms.Rows
                Dim pId = Convert.ToInt32(row("PermissionID"))
                Dim pKey = Convert.ToString(row("PermissionKey"))
                Dim isGranted = checkedKeys.Contains(pKey)

                userService.SetUserPermission(
                    _selectedUserId.Value,
                    pId,
                    isGranted, isGranted, isGranted, isGranted, isGranted, isGranted,
                    Nothing)
            Next

            MessageBox.Show("سطح دسترسی‌های درختی کاربر با موفقیت در سیستم ذخیره گردید.", "تایید ذخیره‌سازی", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
    End Class
End Namespace
