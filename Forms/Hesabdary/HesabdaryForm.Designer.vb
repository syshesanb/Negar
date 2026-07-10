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
        Friend WithEvents lblReportIntroProfitLoss As Label

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
            Me.lblReportIntroProfitLoss = New Label()
            Me.tabs.SuspendLayout()
            Me.tabBankReconciliation.SuspendLayout()
            Me.tabReports.SuspendLayout()
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
            ' lblReportIntroProfitLoss
            '
            Me.lblReportIntroProfitLoss.Dock = DockStyle.Fill
            Me.lblReportIntroProfitLoss.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblReportIntroProfitLoss.Location = New Point(3, 3)
            Me.lblReportIntroProfitLoss.Name = "lblReportIntroProfitLoss"
            Me.lblReportIntroProfitLoss.Size = New Size(1306, 716)
            Me.lblReportIntroProfitLoss.TabIndex = 0
            Me.lblReportIntroProfitLoss.Text = "معرفی حسابهای سود و زیان (در دست تهیه)"
            Me.lblReportIntroProfitLoss.TextAlign = ContentAlignment.MiddleCenter
            '
            ' tabReports
            '
            Me.tabReports.Controls.Add(Me.lblReportIntroProfitLoss)
            Me.tabReports.Location = New Point(4, 23)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.Padding = New Padding(3)
            Me.tabReports.Size = New Size(1312, 722)
            Me.tabReports.Text = "طراحی گزارشات دلخواه"
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
            Me.tabReports.ResumeLayout(False)
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
