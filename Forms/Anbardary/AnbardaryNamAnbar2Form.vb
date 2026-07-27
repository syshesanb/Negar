Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.VisualBasic
Imports Negar.Business

Namespace Negar.Forms
    Public Class AnbardaryNamAnbar2Form
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Private _selectedId As Integer? = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(warehouseId As Integer)
            InitializeComponent()
            _selectedId = warehouseId
        End Sub

        Private Sub AnbardaryNamAnbar2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadWarehouseTypes()
            If _selectedId.HasValue Then
                LoadWarehouseData(_selectedId.Value)
            End If
            LoadLayoutTree()
        End Sub

        Private Sub LoadWarehouseTypes()
            Try
                cmbType.Items.Clear()
                cmbType.Items.Add("--- انتخاب نشده ---")
                
                Dim dt = _service.GetWarehouseTypes()
                For Each row As DataRow In dt.Rows
                    cmbType.Items.Add(Convert.ToString(row("TypeName")))
                Next
                cmbType.SelectedIndex = 0
            Catch ex As Exception
                ' Ignore errors during load
            End Try
        End Sub

        Private Sub BtnManageTypes_Click(sender As Object, e As EventArgs) Handles btnManageTypes.Click
            Using frm As New WarehouseTypeManagerForm()
                frm.ShowDialog()
                Dim currentType = cmbType.Text
                LoadWarehouseTypes()
                If cmbType.Items.Contains(currentType) Then
                    cmbType.Text = currentType
                End If
            End Using
        End Sub

        Private Sub LoadWarehouseData(warehouseId As Integer)
            Try
                Dim row = _service.GetWarehouseById(warehouseId)
                If row IsNot Nothing Then
                    txtName.Text = Convert.ToString(row("WarehouseName"))
                    cmbType.Text = Convert.ToString(row("WarehouseType"))
                    txtKeeper.Text = Convert.ToString(row("WarehouseKeeper"))
                    chkActive.Checked = If(row.IsNull("IsActive"), True, Convert.ToBoolean(row("IsActive")))
                    chkAllowNegative.Checked = If(row.IsNull("AllowNegativeStock"), False, Convert.ToBoolean(row("AllowNegativeStock")))
                    
                    txtLocation.Text = Convert.ToString(row("Location"))
                    txtPhone.Text = Convert.ToString(row("Phone"))
                    txtPhone2.Text = Convert.ToString(row("Phone2"))
                    txtPhone3.Text = Convert.ToString(row("Phone3"))
                    txtPostalCode.Text = Convert.ToString(row("PostalCode"))
                    
                    numCapacity.Value = If(row.IsNull("Capacity"), 0D, Convert.ToDecimal(row("Capacity")))
                    txtCostCenter.Text = Convert.ToString(row("CostCenter"))
                    txtDescription.Text = Convert.ToString(row("Description"))
                End If
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری اطلاعات انبار: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("نام انبار الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If

            Try
                _service.SaveWarehouse(
                    _selectedId,
                    txtName.Text.Trim(),
                    txtLocation.Text.Trim(),
                    chkActive.Checked,
                    cmbType.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtPhone2.Text.Trim(),
                    txtPhone3.Text.Trim(),
                    txtPostalCode.Text.Trim(),
                    Convert.ToDouble(numCapacity.Value),
                    txtKeeper.Text.Trim(),
                    txtCostCenter.Text.Trim(),
                    chkAllowNegative.Checked,
                    txtDescription.Text.Trim()
                )

                MessageBox.Show("اطلاعات انبار با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره اطلاعات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub LoadLayoutTree()
            tvLayout.Nodes.Clear()
            If Not _selectedId.HasValue Then
                tvLayout.Enabled = False
                tvLayout.Nodes.Add("برای افزودن جانمایی ابتدا انبار را ذخیره کنید")
                Return
            End If

            tvLayout.Enabled = True
            Dim dt = _service.GetWarehouseLocations(_selectedId.Value)
            If dt Is Nothing Then Return

            ' Build tree
            Dim nodesDict As New Dictionary(Of Integer, TreeNode)
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("LocationID"))
                Dim pId As Integer? = If(row.IsNull("ParentID"), CType(Nothing, Integer?), Convert.ToInt32(row("ParentID")))
                Dim lType = Convert.ToInt32(row("LocationType"))
                Dim title = Convert.ToString(row("Title"))
                Dim code = Convert.ToString(row("Code"))

                Dim nodeText = $"{GetLocationTypeName(lType)}: {title} ({code})"
                Dim node As New TreeNode(nodeText)
                node.Tag = id
                nodesDict(id) = node

                If pId.HasValue AndAlso nodesDict.ContainsKey(pId.Value) Then
                    nodesDict(pId.Value).Nodes.Add(node)
                Else
                    tvLayout.Nodes.Add(node)
                End If
            Next
            tvLayout.ExpandAll()
        End Sub

        Private Function GetLocationTypeName(typeId As Integer) As String
            Select Case typeId
                Case 1 : Return "سالن"
                Case 2 : Return "بخش"
                Case 3 : Return "راهرو"
                Case 4 : Return "قفسه"
                Case 5 : Return "ردیف"
                Case 6 : Return "باکس"
                Case Else : Return ""
            End Select
        End Function

        Private Sub ctxLayout_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ctxLayout.Opening
            ctxLayout.Items.Clear()
            If Not _selectedId.HasValue Then
                e.Cancel = True
                Return
            End If

            Dim selNode = tvLayout.SelectedNode
            
            Dim addRootItem = ctxLayout.Items.Add("افزودن سالن جدید", Nothing, AddressOf AddLocation_Click)
            addRootItem.Tag = 1 ' LocationType = 1 (Salon)

            If selNode IsNot Nothing Then
                Dim locId = Convert.ToInt32(selNode.Tag)
                Dim dt = _service.GetWarehouseLocations(_selectedId.Value)
                Dim row = dt.Select($"LocationID = {locId}").FirstOrDefault()
                
                If row IsNot Nothing Then
                    Dim curType = Convert.ToInt32(row("LocationType"))
                    If curType < 6 Then
                        Dim nextType = curType + 1
                        Dim addChildItem = ctxLayout.Items.Add($"افزودن {GetLocationTypeName(nextType)} به {row("Title")}", Nothing, AddressOf AddChildLocation_Click)
                        addChildItem.Tag = nextType
                    End If

                    ctxLayout.Items.Add("-")
                    ctxLayout.Items.Add("ویرایش", Nothing, AddressOf EditLocation_Click)
                    ctxLayout.Items.Add("حذف", Nothing, AddressOf DeleteLocation_Click)
                End If
            End If
        End Sub

        Private Sub AddLocation_Click(sender As Object, e As EventArgs)
            Dim item = CType(sender, ToolStripItem)
            Dim locType = Convert.ToInt32(item.Tag)
            ShowLocationEditDialog(Nothing, locType, Nothing)
        End Sub

        Private Sub AddChildLocation_Click(sender As Object, e As EventArgs)
            Dim item = CType(sender, ToolStripItem)
            Dim locType = Convert.ToInt32(item.Tag)
            Dim parentId = Convert.ToInt32(tvLayout.SelectedNode.Tag)
            ShowLocationEditDialog(parentId, locType, Nothing)
        End Sub

        Private Sub EditLocation_Click(sender As Object, e As EventArgs)
            Dim locId = Convert.ToInt32(tvLayout.SelectedNode.Tag)
            Dim dt = _service.GetWarehouseLocations(_selectedId.Value)
            Dim row = dt.Select($"LocationID = {locId}").FirstOrDefault()
            If row IsNot Nothing Then
                ShowLocationEditDialog(If(row.IsNull("ParentID"), CType(Nothing, Integer?), Convert.ToInt32(row("ParentID"))), Convert.ToInt32(row("LocationType")), locId)
            End If
        End Sub

        Private Sub ShowLocationEditDialog(parentId As Integer?, locType As Integer, editLocId As Integer?)
            Dim defaultCode As String = ""
            Dim defaultTitle As String = ""

            If editLocId.HasValue Then
                Dim dt = _service.GetWarehouseLocations(_selectedId.Value)
                Dim row = dt.Select($"LocationID = {editLocId.Value}").FirstOrDefault()
                If row IsNot Nothing Then
                    defaultTitle = Convert.ToString(row("Title"))
                    defaultCode = Convert.ToString(row("Code"))
                End If
            Else
                defaultCode = _service.GenerateNextLocationCode(_selectedId.Value, locType)
                defaultTitle = GetLocationTypeName(locType) & " " & defaultCode.Split("-"c).LastOrDefault()
            End If

            Using frm As New WarehouseLocationEditForm(GetLocationTypeName(locType), editLocId.HasValue, defaultTitle, defaultCode)
                If frm.ShowDialog() = DialogResult.OK Then
                    _service.SaveWarehouseLocation(editLocId, _selectedId.Value, parentId, locType, frm.NodeTitle, frm.NodeCode)
                    LoadLayoutTree()
                End If
            End Using
        End Sub

        Private Sub DeleteLocation_Click(sender As Object, e As EventArgs)
            If MessageBox.Show("آیا از حذف این مورد و تمامی زیرمجموعه‌های آن اطمینان دارید؟", "تایید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Dim locId = Convert.ToInt32(tvLayout.SelectedNode.Tag)
                _service.DeleteWarehouseLocation(locId)
                LoadLayoutTree()
            End If
        End Sub

        Private Sub tvLayout_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvLayout.NodeMouseClick
            If e.Button = MouseButtons.Right Then
                tvLayout.SelectedNode = e.Node
            End If
        End Sub
    
        Private Sub BtnSelectKeeper_Click(sender As Object, e As EventArgs) Handles btnSelectKeeper.Click
            Using frm As New UserPickerForm()
                If frm.ShowDialog() = DialogResult.OK Then
                    txtKeeper.Text = frm.SelectedUserFullName
                End If
            End Using
        End Sub

End Class
End Namespace
