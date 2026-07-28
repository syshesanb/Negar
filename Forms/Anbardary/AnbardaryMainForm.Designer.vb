Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
    <DesignerGenerated()>
    Partial Class AnbardaryMainForm
        Inherits AppBaseForm

        Private components As IContainer
        Friend WithEvents tabs As TabControl
        Friend WithEvents tabPersonnel As TabPage
        Friend WithEvents ctrlPersonnel As Negar.Forms.Controls.PersonnelManagementControl
        Friend WithEvents tabSettings As TabPage
        Friend WithEvents tabUnits As TabPage
        Friend WithEvents tabProductGroups As TabPage
        Friend WithEvents tabProducts As TabPage
        Friend WithEvents tabWarehouses As TabPage
        Friend WithEvents tabVendorsCustomers As TabPage
        Friend WithEvents tabPurchase As TabPage
        Friend WithEvents tabSales As TabPage
        Friend WithEvents tabTransfer As TabPage
        Friend WithEvents tabInventory As TabPage

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabs = New TabControl()
            Me.tabPersonnel = New TabPage()
            Me.ctrlPersonnel = New Negar.Forms.Controls.PersonnelManagementControl()
            Me.tabSettings = New TabPage()
            Me.tabUnits = New TabPage()
            Me.tabProductGroups = New TabPage()
            Me.tabProducts = New TabPage()
            Me.tabWarehouses = New TabPage()
            Me.tabVendorsCustomers = New TabPage()
            Me.tabPurchase = New TabPage()
            Me.tabSales = New TabPage()
            Me.tabTransfer = New TabPage()
            Me.tabInventory = New TabPage()
            Me.tabModyan = New TabPage()
            Me.tabs.SuspendLayout()
            Me.tabPersonnel.SuspendLayout()
            Me.SuspendLayout()
            '
            'AnbardaryForm
            '
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1320, 760)
            Me.Name = "AnbardaryMainForm"
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "خرید و فروش و انبارداری"
            '
            'tabs
            '
            Me.tabs.Controls.Add(Me.tabSettings)
            Me.tabs.Controls.Add(Me.tabUnits)
            Me.tabs.Controls.Add(Me.tabProductGroups)
            Me.tabs.Controls.Add(Me.tabProducts)
            Me.tabs.Controls.Add(Me.tabWarehouses)
            Me.tabs.Controls.Add(Me.tabVendorsCustomers)
            Me.tabs.Controls.Add(Me.tabPurchase)
            Me.tabs.Controls.Add(Me.tabSales)
            Me.tabs.Controls.Add(Me.tabTransfer)
            Me.tabs.Controls.Add(Me.tabInventory)
            Me.tabs.Controls.Add(Me.tabModyan)
            Me.tabs.Dock = DockStyle.Fill
            Me.tabs.Location = New Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New Size(1320, 760)
            Me.tabs.TabIndex = 0
            '
            'tabPersonnel
            '
            Me.tabPersonnel.Controls.Add(Me.ctrlPersonnel)
            Me.tabPersonnel.Location = New Point(4, 25)
            Me.tabPersonnel.Name = "tabPersonnel"
            Me.tabPersonnel.Padding = New Padding(3)
            Me.tabPersonnel.Size = New Size(1312, 731)
            Me.tabPersonnel.TabIndex = 10
            Me.tabPersonnel.Text = "نیروی انسانی انبارداری و فروش"
            Me.tabPersonnel.UseVisualStyleBackColor = True
            '
            'ctrlPersonnel
            '
            Me.ctrlPersonnel.Dock = DockStyle.Fill
            Me.ctrlPersonnel.Location = New Point(3, 3)
            Me.ctrlPersonnel.Name = "ctrlPersonnel"
            Me.ctrlPersonnel.Size = New Size(1306, 725)
            Me.ctrlPersonnel.TabIndex = 0
            '
            'tabSettings
            '
            Me.tabSettings.Location = New Point(4, 23)
            Me.tabSettings.Name = "tabSettings"
            Me.tabSettings.Padding = New Padding(3)
            Me.tabSettings.Size = New Size(1312, 733)
            Me.tabSettings.Text = "تنظیمات اولیه انبارداری و فروش"
            Me.tabSettings.UseVisualStyleBackColor = True
            '
            'tabUnits
            '
            Me.tabUnits.Location = New Point(4, 23)
            Me.tabUnits.Name = "tabUnits"
            Me.tabUnits.Padding = New Padding(3)
            Me.tabUnits.Size = New Size(1312, 733)
            Me.tabUnits.Text = "واحد اندازه گیری"
            Me.tabUnits.UseVisualStyleBackColor = True
            '
            'tabProductGroups
            '
            Me.tabProductGroups.Location = New Point(4, 23)
            Me.tabProductGroups.Name = "tabProductGroups"
            Me.tabProductGroups.Padding = New Padding(3)
            Me.tabProductGroups.Size = New Size(1312, 733)
            Me.tabProductGroups.Text = "گروه بندی"
            Me.tabProductGroups.UseVisualStyleBackColor = True
            '
            'tabProducts
            '
            Me.tabProducts.Location = New Point(4, 23)
            Me.tabProducts.Name = "tabProducts"
            Me.tabProducts.Padding = New Padding(3)
            Me.tabProducts.Size = New Size(1312, 733)
            Me.tabProducts.Text = "نام کالاها و خدمات"
            Me.tabProducts.UseVisualStyleBackColor = True
            '
            'tabWarehouses
            '
            Me.tabWarehouses.Location = New Point(4, 23)
            Me.tabWarehouses.Name = "tabWarehouses"
            Me.tabWarehouses.Padding = New Padding(3)
            Me.tabWarehouses.Size = New Size(1312, 733)
            Me.tabWarehouses.Text = "تعریف انبار"
            Me.tabWarehouses.UseVisualStyleBackColor = True
            '
            'tabVendorsCustomers
            '
            Me.tabVendorsCustomers.Location = New Point(4, 23)
            Me.tabVendorsCustomers.Name = "tabVendorsCustomers"
            Me.tabVendorsCustomers.Padding = New Padding(3)
            Me.tabVendorsCustomers.Size = New Size(1312, 733)
            Me.tabVendorsCustomers.Text = "فروشندگان و خریداران"
            Me.tabVendorsCustomers.UseVisualStyleBackColor = True
            '
            'tabPurchase
            '
            Me.tabPurchase.Location = New Point(4, 23)
            Me.tabPurchase.Name = "tabPurchase"
            Me.tabPurchase.Padding = New Padding(3)
            Me.tabPurchase.Size = New Size(1312, 733)
            Me.tabPurchase.Text = "عملیات خرید"
            Me.tabPurchase.UseVisualStyleBackColor = True
            '
            'tabSales
            '
            Me.tabSales.Location = New Point(4, 23)
            Me.tabSales.Name = "tabSales"
            Me.tabSales.Padding = New Padding(3)
            Me.tabSales.Size = New Size(1312, 733)
            Me.tabSales.Text = "عملیات فروش"
            Me.tabSales.UseVisualStyleBackColor = True
            '
            'tabTransfer
            '
            Me.tabTransfer.Location = New Point(4, 23)
            Me.tabTransfer.Name = "tabTransfer"
            Me.tabTransfer.Padding = New Padding(3)
            Me.tabTransfer.Size = New Size(1312, 733)
            Me.tabTransfer.Text = "حواله بین انبارها"
            Me.tabTransfer.UseVisualStyleBackColor = True
            '
            'tabInventory
            '
            Me.tabInventory.Location = New Point(4, 23)
            Me.tabInventory.Name = "tabInventory"
            Me.tabInventory.Padding = New Padding(3)
            Me.tabInventory.Size = New Size(1312, 733)
            Me.tabInventory.Text = "موجودی انبار"
            Me.tabInventory.UseVisualStyleBackColor = True
            '
            'tabModyan
            '
            Me.tabModyan.Location = New Point(4, 23)
            Me.tabModyan.Name = "tabModyan"
            Me.tabModyan.Padding = New Padding(3)
            Me.tabModyan.Size = New Size(1312, 733)
            Me.tabModyan.Text = "🏛️ سامانه مودیان"
            Me.tabModyan.UseVisualStyleBackColor = True
            '
            'Controls
            '
            Me.Controls.Add(Me.tabs)
            Me.tabPersonnel.ResumeLayout(False)
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents tabModyan As TabPage
    End Class
End Namespace


