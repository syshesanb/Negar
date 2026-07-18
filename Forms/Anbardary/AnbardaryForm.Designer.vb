Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryForm
        Inherits AppBaseForm

        Private components As IContainer
        Friend WithEvents tabs As TabControl
        Friend WithEvents tabProducts As TabPage
        Friend WithEvents tabWarehouses As TabPage
        Friend WithEvents tabPurchase As TabPage
        Friend WithEvents tabSales As TabPage
        Friend WithEvents tabInventory As TabPage

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabs = New TabControl()
            Me.tabProducts = New TabPage()
            Me.tabWarehouses = New TabPage()
            Me.tabPurchase = New TabPage()
            Me.tabSales = New TabPage()
            Me.tabInventory = New TabPage()
            Me.tabs.SuspendLayout()
            Me.SuspendLayout()
            '
            'AnbardaryForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1320, 760)
            Me.Name = "AnbardaryForm"
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "خرید و فروش و انبارداری"
            '
            'tabs
            '
            Me.tabs.Controls.Add(Me.tabProducts)
            Me.tabs.Controls.Add(Me.tabWarehouses)
            Me.tabs.Controls.Add(Me.tabPurchase)
            Me.tabs.Controls.Add(Me.tabSales)
            Me.tabs.Controls.Add(Me.tabInventory)
            Me.tabs.Dock = DockStyle.Fill
            Me.tabs.Location = New Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New Size(1320, 760)
            Me.tabs.TabIndex = 0
            '
            'tabProducts
            '
            Me.tabProducts.Location = New Point(4, 23)
            Me.tabProducts.Name = "tabProducts"
            Me.tabProducts.Padding = New Padding(3)
            Me.tabProducts.Size = New Size(1312, 733)
            Me.tabProducts.Text = "کالاها"
            Me.tabProducts.UseVisualStyleBackColor = True
            '
            'tabWarehouses
            '
            Me.tabWarehouses.Location = New Point(4, 23)
            Me.tabWarehouses.Name = "tabWarehouses"
            Me.tabWarehouses.Padding = New Padding(3)
            Me.tabWarehouses.Size = New Size(1312, 733)
            Me.tabWarehouses.Text = "انبارها"
            Me.tabWarehouses.UseVisualStyleBackColor = True
            '
            'tabPurchase
            '
            Me.tabPurchase.Location = New Point(4, 23)
            Me.tabPurchase.Name = "tabPurchase"
            Me.tabPurchase.Padding = New Padding(3)
            Me.tabPurchase.Size = New Size(1312, 733)
            Me.tabPurchase.Text = "خرید"
            Me.tabPurchase.UseVisualStyleBackColor = True
            '
            'tabSales
            '
            Me.tabSales.Location = New Point(4, 23)
            Me.tabSales.Name = "tabSales"
            Me.tabSales.Padding = New Padding(3)
            Me.tabSales.Size = New Size(1312, 733)
            Me.tabSales.Text = "فروش"
            Me.tabSales.UseVisualStyleBackColor = True
            '
            'tabInventory
            '
            Me.tabInventory.Location = New Point(4, 23)
            Me.tabInventory.Name = "tabInventory"
            Me.tabInventory.Padding = New Padding(3)
            Me.tabInventory.Size = New Size(1312, 733)
            Me.tabInventory.Text = "موجودی"
            Me.tabInventory.UseVisualStyleBackColor = True
            '
            'Controls
            '
            Me.Controls.Add(Me.tabs)
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
