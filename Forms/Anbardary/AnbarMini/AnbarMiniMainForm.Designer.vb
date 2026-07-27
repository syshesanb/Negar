Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

Namespace Negar.Forms.Anbardary.AnbarMini
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class AnbarMiniMainForm
        Inherits AppBaseForm

        Private components As IContainer

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.tabsMini = New TabControl()
            Me.tabPOS = New TabPage()
            Me.tabPurchase = New TabPage()
            Me.tabParties = New TabPage()
            Me.tabExpenses = New TabPage()
            Me.tabProducts = New TabPage()
            Me.tabWarehouses = New TabPage()
            Me.tabGroups = New TabPage()
            Me.tabInventory = New TabPage()
            Me.tabsMini.SuspendLayout()
            Me.SuspendLayout()

            ' tabsMini
            Me.tabsMini.Controls.Add(Me.tabPOS)
            Me.tabsMini.Controls.Add(Me.tabPurchase)
            Me.tabsMini.Controls.Add(Me.tabParties)
            Me.tabsMini.Controls.Add(Me.tabExpenses)
            Me.tabsMini.Controls.Add(Me.tabProducts)
            Me.tabsMini.Controls.Add(Me.tabWarehouses)
            Me.tabsMini.Controls.Add(Me.tabGroups)
            Me.tabsMini.Controls.Add(Me.tabInventory)
            Me.tabsMini.Dock = DockStyle.Fill
            Me.tabsMini.Font = New Font("B Yekan", 10.0!)
            Me.tabsMini.ItemSize = New Size(130, 35)
            Me.tabsMini.Location = New Point(0, 0)
            Me.tabsMini.Name = "tabsMini"
            Me.tabsMini.SelectedIndex = 0
            Me.tabsMini.Size = New Size(1000, 650)
            Me.tabsMini.SizeMode = TabSizeMode.Fixed

            ' tabPOS
            Me.tabPOS.Location = New Point(4, 39)
            Me.tabPOS.Name = "tabPOS"
            Me.tabPOS.Padding = New Padding(3)
            Me.tabPOS.Size = New Size(992, 607)
            Me.tabPOS.Text = "🛒 فروش سریع (POS)"

            ' tabPurchase
            Me.tabPurchase.Location = New Point(4, 39)
            Me.tabPurchase.Name = "tabPurchase"
            Me.tabPurchase.Size = New Size(992, 607)
            Me.tabPurchase.Text = "📦 خرید کالا"

            ' tabParties
            Me.tabParties.Location = New Point(4, 39)
            Me.tabParties.Name = "tabParties"
            Me.tabParties.Size = New Size(992, 607)
            Me.tabParties.Text = "👥 لیست فروشنده و خریدار"

            ' tabExpenses
            Me.tabExpenses.Location = New Point(4, 39)
            Me.tabExpenses.Name = "tabExpenses"
            Me.tabExpenses.Size = New Size(992, 607)
            Me.tabExpenses.Text = "💸 هزینه‌ها"

            ' tabProducts
            Me.tabProducts.Location = New Point(4, 39)
            Me.tabProducts.Name = "tabProducts"
            Me.tabProducts.Size = New Size(992, 607)
            Me.tabProducts.Text = "🏷️ لیست کالاها"

            ' tabWarehouses
            Me.tabWarehouses.Location = New Point(4, 39)
            Me.tabWarehouses.Name = "tabWarehouses"
            Me.tabWarehouses.Size = New Size(992, 607)
            Me.tabWarehouses.Text = "🏢 لیست انبارها"

            ' tabGroups
            Me.tabGroups.Location = New Point(4, 39)
            Me.tabGroups.Name = "tabGroups"
            Me.tabGroups.Size = New Size(992, 607)
            Me.tabGroups.Text = "📁 دسته‌بندی کالا"

            ' tabInventory
            Me.tabInventory.Location = New Point(4, 39)
            Me.tabInventory.Name = "tabInventory"
            Me.tabInventory.Size = New Size(992, 607)
            Me.tabInventory.Text = "📊 گزارش"

            ' Form Setup
            Me.AutoScaleDimensions = New SizeF(8.0!, 19.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1000, 650)
            Me.Controls.Add(Me.tabsMini)
            Me.Font = New Font("B Yekan", 9.0!)
            Me.Name = "AnbarMiniMainForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "مدیریت فروشگاه و انبار - نسخه مینی (AnbarMini)"
            Me.WindowState = FormWindowState.Maximized
            Me.tabsMini.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents tabsMini As TabControl
        Friend WithEvents tabPOS As TabPage
        Friend WithEvents tabPurchase As TabPage
        Friend WithEvents tabParties As TabPage
        Friend WithEvents tabExpenses As TabPage
        Friend WithEvents tabProducts As TabPage
        Friend WithEvents tabWarehouses As TabPage
        Friend WithEvents tabGroups As TabPage
        Friend WithEvents tabInventory As TabPage
    End Class
End Namespace
