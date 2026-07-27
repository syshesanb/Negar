Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Negar.Forms
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
        Friend WithEvents tabProfitLoss As TabPage
        Friend WithEvents tabBalanceSheet As TabPage
        Friend WithEvents tabAdvancedReports As TabPage
        Friend WithEvents tabChartReports As TabPage
        Friend WithEvents tabReports As TabPage
        Friend WithEvents lblBankReconciliation As Label
        Friend WithEvents lblReportIntroProfitLoss As Label
        Friend WithEvents lblProfitLossIntro As Label
        Friend WithEvents lblBalanceSheetIntro As Label
        Friend WithEvents lblAdvancedReportsIntro As Label
        Friend WithEvents lblChartReportsIntro As Label

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
            Me.tabProfitLoss = New TabPage()
            Me.tabBalanceSheet = New TabPage()
            Me.tabAdvancedReports = New TabPage()
            Me.tabChartReports = New TabPage()
            Me.tabReports = New TabPage()
            Me.lblReportIntroProfitLoss = New Label()
            Me.lblProfitLossIntro = New Label()
            Me.lblBalanceSheetIntro = New Label()
            Me.lblAdvancedReportsIntro = New Label()
            Me.lblChartReportsIntro = New Label()
            Me.tabs.SuspendLayout()
            Me.tabBankReconciliation.SuspendLayout()
            Me.tabReports.SuspendLayout()
            Me.tabProfitLoss.SuspendLayout()
            Me.tabBalanceSheet.SuspendLayout()
            Me.tabAdvancedReports.SuspendLayout()
            Me.tabChartReports.SuspendLayout()
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
            ' tabProfitLoss
            '
            Me.tabProfitLoss.Controls.Add(Me.lblProfitLossIntro)
            Me.tabProfitLoss.Location = New Point(4, 23)
            Me.tabProfitLoss.Name = "tabProfitLoss"
            Me.tabProfitLoss.Padding = New Padding(3)
            Me.tabProfitLoss.Size = New Size(1312, 722)
            Me.tabProfitLoss.Text = "عملکرد و سود و زیان"
            Me.tabProfitLoss.UseVisualStyleBackColor = True
            '
            ' tabBalanceSheet
            '
            Me.tabBalanceSheet.Controls.Add(Me.lblBalanceSheetIntro)
            Me.tabBalanceSheet.Location = New Point(4, 23)
            Me.tabBalanceSheet.Name = "tabBalanceSheet"
            Me.tabBalanceSheet.Padding = New Padding(3)
            Me.tabBalanceSheet.Size = New Size(1312, 722)
            Me.tabBalanceSheet.Text = "ترازنامه"
            Me.tabBalanceSheet.UseVisualStyleBackColor = True
            '
            ' tabAdvancedReports
            '
            Me.tabAdvancedReports.Controls.Add(Me.lblAdvancedReportsIntro)
            Me.tabAdvancedReports.Location = New Point(4, 23)
            Me.tabAdvancedReports.Name = "tabAdvancedReports"
            Me.tabAdvancedReports.Padding = New Padding(3)
            Me.tabAdvancedReports.Size = New Size(1312, 722)
            Me.tabAdvancedReports.Text = "گزارشات پیشرفته"
            Me.tabAdvancedReports.UseVisualStyleBackColor = True
            '
            ' tabChartReports
            '
            Me.tabChartReports.Controls.Add(Me.lblChartReportsIntro)
            Me.tabChartReports.Location = New Point(4, 23)
            Me.tabChartReports.Name = "tabChartReports"
            Me.tabChartReports.Padding = New Padding(3)
            Me.tabChartReports.Size = New Size(1312, 722)
            Me.tabChartReports.Text = "گزارشات نموداری"
            Me.tabChartReports.UseVisualStyleBackColor = True

            '
            ' lblProfitLossIntro
            '
            Me.lblProfitLossIntro.Dock = DockStyle.Fill
            Me.lblProfitLossIntro.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblProfitLossIntro.Location = New Point(3, 3)
            Me.lblProfitLossIntro.Name = "lblProfitLossIntro"
            Me.lblProfitLossIntro.Size = New Size(1306, 716)
            Me.lblProfitLossIntro.TabIndex = 0
            Me.lblProfitLossIntro.Text = "این گزارش در دست تکمیل می باشد"
            Me.lblProfitLossIntro.TextAlign = ContentAlignment.MiddleCenter
            '
            ' lblBalanceSheetIntro
            '
            Me.lblBalanceSheetIntro.Dock = DockStyle.Fill
            Me.lblBalanceSheetIntro.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblBalanceSheetIntro.Location = New Point(3, 3)
            Me.lblBalanceSheetIntro.Name = "lblBalanceSheetIntro"
            Me.lblBalanceSheetIntro.Size = New Size(1306, 716)
            Me.lblBalanceSheetIntro.TabIndex = 0
            Me.lblBalanceSheetIntro.Text = "این گزارش در دست تکمیل می باشد"
            Me.lblBalanceSheetIntro.TextAlign = ContentAlignment.MiddleCenter
            '
            ' lblAdvancedReportsIntro
            '
            Me.lblAdvancedReportsIntro.Dock = DockStyle.Fill
            Me.lblAdvancedReportsIntro.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblAdvancedReportsIntro.Location = New Point(3, 3)
            Me.lblAdvancedReportsIntro.Name = "lblAdvancedReportsIntro"
            Me.lblAdvancedReportsIntro.Size = New Size(1306, 716)
            Me.lblAdvancedReportsIntro.TabIndex = 0
            Me.lblAdvancedReportsIntro.Text = "این گزارش در دست تکمیل می باشد"
            Me.lblAdvancedReportsIntro.TextAlign = ContentAlignment.MiddleCenter
            '
            ' lblChartReportsIntro
            '
            Me.lblChartReportsIntro.Dock = DockStyle.Fill
            Me.lblChartReportsIntro.Font = New Font("Tahoma", 14.0!, FontStyle.Bold)
            Me.lblChartReportsIntro.Location = New Point(3, 3)
            Me.lblChartReportsIntro.Name = "lblChartReportsIntro"
            Me.lblChartReportsIntro.Size = New Size(1306, 716)
            Me.lblChartReportsIntro.TabIndex = 0
            Me.lblChartReportsIntro.Text = "این گزارش در دست تکمیل می باشد"
            Me.lblChartReportsIntro.TextAlign = ContentAlignment.MiddleCenter
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
            Me.tabs.Controls.Add(Me.tabProfitLoss)
            Me.tabs.Controls.Add(Me.tabBalanceSheet)
            Me.tabs.Controls.Add(Me.tabAdvancedReports)
            Me.tabs.Controls.Add(Me.tabChartReports)
            Me.tabs.Controls.Add(Me.tabReports)
 
            Me.Controls.Add(Me.tabs)
            Me.tabBankReconciliation.ResumeLayout(False)
            Me.tabReports.ResumeLayout(False)
            Me.tabProfitLoss.ResumeLayout(False)
            Me.tabBalanceSheet.ResumeLayout(False)
            Me.tabAdvancedReports.ResumeLayout(False)
            Me.tabChartReports.ResumeLayout(False)
            Me.tabs.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
