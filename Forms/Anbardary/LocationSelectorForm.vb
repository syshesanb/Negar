Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Forms
    Public Class LocationSelectorForm
        Inherits Form

        Private ReadOnly _service As New CatalogService()
        Public Property SelectedLocationID As Integer?
        Public Property SelectedTitlePath As String
        Public Property SelectedCodePath As String

        Public Sub New()
            InitializeComponent()
            ThemeHelper.ApplyFormTheme(Me)
        End Sub

        Private Sub LocationSelectorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Dim dt = _service.GetWarehouses()
            cmbWarehouses.DisplayMember = "DisplayTitle"
            cmbWarehouses.ValueMember = "WarehouseID"
            cmbWarehouses.DataSource = dt
            cmbWarehouses.SelectedIndex = -1
        End Sub

        Private Sub cmbWarehouses_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbWarehouses.SelectedIndexChanged
            tvLayout.Nodes.Clear()
            If cmbWarehouses.SelectedValue IsNot Nothing AndAlso (TypeOf cmbWarehouses.SelectedValue Is Integer OrElse TypeOf cmbWarehouses.SelectedValue Is Long OrElse TypeOf cmbWarehouses.SelectedValue Is Short) Then
                Dim wId = Convert.ToInt32(cmbWarehouses.SelectedValue)
                LoadLayoutTree(wId)
            End If
        End Sub

        Private Sub LoadLayoutTree(warehouseId As Integer)
            Dim wName = cmbWarehouses.Text
            Dim rootNode As New TreeNode(wName)
            rootNode.Tag = -1 ' Special tag for root
            tvLayout.Nodes.Add(rootNode)

            Dim dt = _service.GetWarehouseLocations(warehouseId)
            If dt Is Nothing Then Return

            Dim nodesDict As New System.Collections.Generic.Dictionary(Of Integer, TreeNode)
            For Each row As DataRow In dt.Rows
                Dim id = Convert.ToInt32(row("LocationID"))
                Dim pId As Integer? = If(row.IsNull("ParentID"), CType(Nothing, Integer?), Convert.ToInt32(row("ParentID")))
                Dim lType = Convert.ToInt32(row("LocationType"))
                Dim title = Convert.ToString(row("Title"))
                Dim code = Convert.ToString(row("Code"))

                Dim typeName = ""
                Select Case lType
                    Case 1 : typeName = "سالن"
                    Case 2 : typeName = "بخش"
                    Case 3 : typeName = "راهرو"
                    Case 4 : typeName = "قفسه"
                    Case 5 : typeName = "ردیف"
                    Case 6 : typeName = "باکس"
                End Select

                Dim nodeText = $"{typeName}: {title} ({code})"
                Dim node As New TreeNode(nodeText)
                node.Tag = id
                nodesDict(id) = node

                If pId.HasValue AndAlso nodesDict.ContainsKey(pId.Value) Then
                    nodesDict(pId.Value).Nodes.Add(node)
                Else
                    rootNode.Nodes.Add(node)
                End If
            Next
            tvLayout.ExpandAll()
        End Sub

        Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
            If tvLayout.SelectedNode IsNot Nothing AndAlso Convert.ToInt32(tvLayout.SelectedNode.Tag) > 0 Then
                SelectedLocationID = Convert.ToInt32(tvLayout.SelectedNode.Tag)
                Dim paths = _service.GetLocationPath(SelectedLocationID.Value)
                Dim wName = cmbWarehouses.Text
                SelectedTitlePath = wName & " > " & paths.Item1
                SelectedCodePath = paths.Item2
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show("لطفا یک جانمایی را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
    End Class
End Namespace
