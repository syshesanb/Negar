Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class HesabdaryForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents tabs As TabControl
        Friend WithEvents tabAccounts As TabPage
        Friend WithEvents tabShenavar As TabPage
        Friend WithEvents tabEntry As TabPage
        Friend WithEvents tabBankReconciliation As TabPage
        Friend WithEvents tabTrial As TabPage
        Friend WithEvents tabLedger As TabPage
        Friend WithEvents tabTarazShenavar As TabPage
        Friend WithEvents tabDaftarShenavar As TabPage
        Friend WithEvents tabReports As TabPage
        Friend WithEvents lblBankReconciliation As Label
        Friend WithEvents tabsReports As TabControl
        Friend WithEvents tabReportIntroProfitLoss As TabPage
        Friend WithEvents tabReportPerformance As TabPage
        Friend WithEvents tabReportProfitLoss As TabPage
        Friend WithEvents tabReportPerformanceProfitLoss As TabPage
        Friend WithEvents tabReportBalanceSheet As TabPage
        Friend WithEvents lblReportIntroProfitLoss As Label
        Friend WithEvents lblReportPerformance As Label
        Friend WithEvents lblReportProfitLoss As Label
        Friend WithEvents lblReportPerformanceProfitLoss As Label
        Friend WithEvents lblReportBalanceSheet As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.tabs = New TabControl()
            Me.tabAccounts = New TabPage()
            Me.tabShenavar = New TabPage()
            Me.tabEntry = New TabPage()
            Me.tabBankReconciliation = New TabPage()
            Me.lblBankReconciliation = New Label()
            Me.tabTrial = New TabPage()
            Me.tabLedger = New TabPage()
            Me.tabTarazShenavar = New TabPage()
            Me.tabDaftarShenavar = New TabPage()
            Me.tabReports = New TabPage()
            Me.tabsReports = New TabControl()
            Me.tabReportIntroProfitLoss = New TabPage()
            Me.tabReportPerformance = New TabPage()
            Me.tabReportProfitLoss = New TabPage()
            Me.tabReportPerformanceProfitLoss = New TabPage()
            Me.tabReportBalanceSheet = New TabPage()
            Me.lblReportIntroProfitLoss = New Label()
            Me.lblReportPerformance = New Label()
            Me.lblReportProfitLoss = New Label()
            Me.lblReportPerformanceProfitLoss = New Label()
            Me.lblReportBalanceSheet = New Label()
            Me.tabs.SuspendLayout()
            Me.tabBankReconciliation.SuspendLayout()
            Me.tabReports.SuspendLayout()
            Me.tabsReports.SuspendLayout()
            Me.tabReportIntroProfitLoss.SuspendLayout()
            Me.tabReportPerformance.SuspendLayout()
            Me.tabReportProfitLoss.SuspendLayout()
            Me.tabReportPerformanceProfitLoss.SuspendLayout()
            Me.tabReportBalanceSheet.SuspendLayout()
            Me.SuspendLayout()

            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(1320, 749)
            Me.Font = New Font("Tahoma", 9.0!)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Name = "HesabdaryForm"
            Me.Text = "حسابداری"

            Me.tabs.Dock = DockStyle.Fill
            Me.tabs.Location = New Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New Size(1320, 749)
            Me.tabs.TabIndex = 0

            Me.tabAccounts.Location = New Point(4, 23)
            Me.tabAccounts.Name = "tabAccounts"
            Me.tabAccounts.Padding = New Padding(3)
            Me.tabAccounts.Size = New Size(1312, 722)
            Me.tabAccounts.Text = "سرفصل حساب‌ها"
            Me.tabAccounts.UseVisualStyleBackColor = True

            Me.tabShenavar.Location = New Point(4, 23)
            Me.tabShenavar.Name = "tabShenavar"
            Me.tabShenavar.Padding = New Padding(3)
            Me.tabShenavar.Size = New Size(1312, 722)
            Me.tabShenavar.Text = "حسابهای شناور"
            Me.tabShenavar.UseVisualStyleBackColor = True

            Me.tabEntry.Location = New Point(4, 23)
            Me.tabEntry.Name = "tabEntry"
            Me.tabEntry.Padding = New Padding(3)
            Me.tabEntry.Size = New Size(1312, 722)
            Me.tabEntry.Text = "ثبت سند حسابداری"
            Me.tabEntry.UseVisualStyleBackColor = True

            '
            ' lblBankReconciliation
            '
            Me.lblBankReconciliation.Dock = DockStyle.Fill
            Me.lblBankReconciliation.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblBankReconciliation.Location = New Point(3, 3)
            Me.lblBankReconciliation.Name = "lblBankReconciliation"
            Me.lblBankReconciliation.Size = New Size(1306, 716)
            Me.lblBankReconciliation.TabIndex = 0
            Me.lblBankReconciliation.Text = "این قسمت برنامه در دست تهیه و تکمیل می باشد"
            Me.lblBankReconciliation.TextAlign = ContentAlignment.MiddleCenter

            '
            ' tabBankReconciliation
            '
            Me.tabBankReconciliation.Controls.Add(Me.lblBankReconciliation)
            Me.tabBankReconciliation.Location = New Point(4, 23)
            Me.tabBankReconciliation.Name = "tabBankReconciliation"
            Me.tabBankReconciliation.Padding = New Padding(3)
            Me.tabBankReconciliation.Size = New Size(1312, 722)
            Me.tabBankReconciliation.Text = "مغایرات بانکی"
            Me.tabBankReconciliation.UseVisualStyleBackColor = True

            Me.tabTrial.Location = New Point(4, 23)
            Me.tabTrial.Name = "tabTrial"
            Me.tabTrial.Padding = New Padding(3)
            Me.tabTrial.Size = New Size(1312, 722)
            Me.tabTrial.Text = "تراز آزمایشی"
            Me.tabTrial.UseVisualStyleBackColor = True

            Me.tabLedger.Location = New Point(4, 23)
            Me.tabLedger.Name = "tabLedger"
            Me.tabLedger.Padding = New Padding(3)
            Me.tabLedger.Size = New Size(1312, 722)
            Me.tabLedger.Text = "دفتر حساب"
            Me.tabLedger.UseVisualStyleBackColor = True
            '
            ' tabTarazShenavar
            '
            Me.tabTarazShenavar.Location = New Point(4, 23)
            Me.tabTarazShenavar.Name = "tabTarazShenavar"
            Me.tabTarazShenavar.Padding = New Padding(3)
            Me.tabTarazShenavar.Size = New Size(1312, 722)
            Me.tabTarazShenavar.Text = "تراز شناور"
            Me.tabTarazShenavar.UseVisualStyleBackColor = True
            '
            ' tabDaftarShenavar
            '
            Me.tabDaftarShenavar.Location = New Point(4, 23)
            Me.tabDaftarShenavar.Name = "tabDaftarShenavar"
            Me.tabDaftarShenavar.Padding = New Padding(3)
            Me.tabDaftarShenavar.Size = New Size(1312, 722)
            Me.tabDaftarShenavar.Text = "دفتر شناور"
            Me.tabDaftarShenavar.UseVisualStyleBackColor = True

            '
            ' tabsReports
            '
            Me.tabsReports.Controls.Add(Me.tabReportIntroProfitLoss)
            Me.tabsReports.Controls.Add(Me.tabReportPerformance)
            Me.tabsReports.Controls.Add(Me.tabReportProfitLoss)
            Me.tabsReports.Controls.Add(Me.tabReportPerformanceProfitLoss)
            Me.tabsReports.Controls.Add(Me.tabReportBalanceSheet)
            Me.tabsReports.Dock = DockStyle.Fill
            Me.tabsReports.Location = New Point(3, 3)
            Me.tabsReports.Name = "tabsReports"
            Me.tabsReports.SelectedIndex = 0
            Me.tabsReports.Size = New Size(1306, 716)
            Me.tabsReports.TabIndex = 0
            '
            ' tabReportIntroProfitLoss
            '
            Me.tabReportIntroProfitLoss.Controls.Add(Me.lblReportIntroProfitLoss)
            Me.tabReportIntroProfitLoss.Location = New Point(4, 23)
            Me.tabReportIntroProfitLoss.Name = "tabReportIntroProfitLoss"
            Me.tabReportIntroProfitLoss.Padding = New Padding(3)
            Me.tabReportIntroProfitLoss.Size = New Size(1298, 689)
            Me.tabReportIntroProfitLoss.Text = "معرفی حسابهای سود و زیان"
            Me.tabReportIntroProfitLoss.UseVisualStyleBackColor = True
            '
            ' lblReportIntroProfitLoss
            '
            Me.lblReportIntroProfitLoss.Dock = DockStyle.Fill
            Me.lblReportIntroProfitLoss.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportIntroProfitLoss.Location = New Point(3, 3)
            Me.lblReportIntroProfitLoss.Name = "lblReportIntroProfitLoss"
            Me.lblReportIntroProfitLoss.Size = New Size(1292, 683)
            Me.lblReportIntroProfitLoss.TabIndex = 0
            Me.lblReportIntroProfitLoss.Text = "معرفی حسابهای سود و زیان (در دست تهیه)"
            Me.lblReportIntroProfitLoss.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReportPerformance
            '
            Me.tabReportPerformance.Controls.Add(Me.lblReportPerformance)
            Me.tabReportPerformance.Location = New Point(4, 23)
            Me.tabReportPerformance.Name = "tabReportPerformance"
            Me.tabReportPerformance.Padding = New Padding(3)
            Me.tabReportPerformance.Size = New Size(1298, 689)
            Me.tabReportPerformance.Text = "گزارش عملکرد"
            Me.tabReportPerformance.UseVisualStyleBackColor = True
            '
            ' lblReportPerformance
            '
            Me.lblReportPerformance.Dock = DockStyle.Fill
            Me.lblReportPerformance.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportPerformance.Location = New Point(3, 3)
            Me.lblReportPerformance.Name = "lblReportPerformance"
            Me.lblReportPerformance.Size = New Size(1292, 683)
            Me.lblReportPerformance.TabIndex = 0
            Me.lblReportPerformance.Text = "گزارش عملکرد (در دست تهیه)"
            Me.lblReportPerformance.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReportProfitLoss
            '
            Me.tabReportProfitLoss.Controls.Add(Me.lblReportProfitLoss)
            Me.tabReportProfitLoss.Location = New Point(4, 23)
            Me.tabReportProfitLoss.Name = "tabReportProfitLoss"
            Me.tabReportProfitLoss.Padding = New Padding(3)
            Me.tabReportProfitLoss.Size = New Size(1298, 689)
            Me.tabReportProfitLoss.Text = "گزارش سود و زیان"
            Me.tabReportProfitLoss.UseVisualStyleBackColor = True
            '
            ' lblReportProfitLoss
            '
            Me.lblReportProfitLoss.Dock = DockStyle.Fill
            Me.lblReportProfitLoss.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportProfitLoss.Location = New Point(3, 3)
            Me.lblReportProfitLoss.Name = "lblReportProfitLoss"
            Me.lblReportProfitLoss.Size = New Size(1292, 683)
            Me.lblReportProfitLoss.TabIndex = 0
            Me.lblReportProfitLoss.Text = "گزارش سود و زیان (در دست تهیه)"
            Me.lblReportProfitLoss.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReportPerformanceProfitLoss
            '
            Me.tabReportPerformanceProfitLoss.Controls.Add(Me.lblReportPerformanceProfitLoss)
            Me.tabReportPerformanceProfitLoss.Location = New Point(4, 23)
            Me.tabReportPerformanceProfitLoss.Name = "tabReportPerformanceProfitLoss"
            Me.tabReportPerformanceProfitLoss.Padding = New Padding(3)
            Me.tabReportPerformanceProfitLoss.Size = New Size(1298, 689)
            Me.tabReportPerformanceProfitLoss.Text = "گزارش عملکرد و سود و زیان"
            Me.tabReportPerformanceProfitLoss.UseVisualStyleBackColor = True
            '
            ' lblReportPerformanceProfitLoss
            '
            Me.lblReportPerformanceProfitLoss.Dock = DockStyle.Fill
            Me.lblReportPerformanceProfitLoss.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportPerformanceProfitLoss.Location = New Point(3, 3)
            Me.lblReportPerformanceProfitLoss.Name = "lblReportPerformanceProfitLoss"
            Me.lblReportPerformanceProfitLoss.Size = New Size(1292, 683)
            Me.lblReportPerformanceProfitLoss.TabIndex = 0
            Me.lblReportPerformanceProfitLoss.Text = "گزارش عملکرد و سود و زیان (در دست تهیه)"
            Me.lblReportPerformanceProfitLoss.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReportBalanceSheet
            '
            Me.tabReportBalanceSheet.Controls.Add(Me.lblReportBalanceSheet)
            Me.tabReportBalanceSheet.Location = New Point(4, 23)
            Me.tabReportBalanceSheet.Name = "tabReportBalanceSheet"
            Me.tabReportBalanceSheet.Padding = New Padding(3)
            Me.tabReportBalanceSheet.Size = New Size(1298, 689)
            Me.tabReportBalanceSheet.Text = "ترازنامه"
            Me.tabReportBalanceSheet.UseVisualStyleBackColor = True
            '
            ' lblReportBalanceSheet
            '
            Me.lblReportBalanceSheet.Dock = DockStyle.Fill
            Me.lblReportBalanceSheet.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportBalanceSheet.Location = New Point(3, 3)
            Me.lblReportBalanceSheet.Name = "lblReportBalanceSheet"
            Me.lblReportBalanceSheet.Size = New Size(1292, 683)
            Me.lblReportBalanceSheet.TabIndex = 0
            Me.lblReportBalanceSheet.Text = "ترازنامه (در دست تهیه)"
            Me.lblReportBalanceSheet.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReports
            '
            Me.tabReports.Controls.Add(Me.tabsReports)
            Me.tabReports.Location = New Point(4, 23)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.Padding = New Padding(3)
            Me.tabReports.Size = New Size(1312, 722)
            Me.tabReports.Text = "گزارشات حسابداری"
            Me.tabReports.UseVisualStyleBackColor = True
 
            Me.tabs.Controls.Add(Me.tabAccounts)
            Me.tabs.Controls.Add(Me.tabShenavar)
            Me.tabs.Controls.Add(Me.tabEntry)
            Me.tabs.Controls.Add(Me.tabBankReconciliation)
            Me.tabs.Controls.Add(Me.tabTrial)
            Me.tabs.Controls.Add(Me.tabLedger)
            Me.tabs.Controls.Add(Me.tabTarazShenavar)
            Me.tabs.Controls.Add(Me.tabDaftarShenavar)
            Me.tabs.Controls.Add(Me.tabReports)
 
            Me.Controls.Add(Me.tabs)
            Me.tabBankReconciliation.ResumeLayout(False)
            Me.tabReportIntroProfitLoss.ResumeLayout(False)
            Me.tabReportPerformance.ResumeLayout(False)
            Me.tabReportProfitLoss.ResumeLayout(False)
            Me.tabReportPerformanceProfitLoss.ResumeLayout(False)
            Me.tabReportBalanceSheet.ResumeLayout(False)
            Me.tabsReports.ResumeLayout(False)
            Me.tabReports.ResumeLayout(False)
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
