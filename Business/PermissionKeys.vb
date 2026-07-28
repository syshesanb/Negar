Option Strict Off
Option Explicit On

Imports System

Namespace Negar.Business
    Public Module PermissionKeys
        Public Const ManageUsers As String = "ManageUsers"
        Public Const ManageBasicUsers As String = "ManageBasicUsers"
        Public Const ManageCompanies As String = "ManageCompanies"
        Public Const ManageFiscalYears As String = "ManageFiscalYears"
        Public Const ManageCompaniesYears As String = "ManageCompaniesYears"
        Public Const SelectCompanyFiscalYear As String = "SelectCompanyFiscalYear"
        Public Const ManageAppThemes As String = "ManageAppThemes"
        Public Const ManageAppMessages As String = "ManageAppMessages"
        Public Const DataMigration As String = "DataMigration"
        Public Const BackupData As String = "BackupData"
        Public Const RestoreData As String = "RestoreData"
        Public Const ManageBusinessShells As String = "ManageBusinessShells"
        Public Const ManageUtilities As String = "ManageUtilities"
        Public Const ViewActivityLog As String = "ViewActivityLog"
        Public Const LockSanad1 As String = "LockSanad1"
        Public Const HideSFSHInSanad As String = "HideSFSHInSanad"
        Public Const SwitchUser As String = "SwitchUser"
        Public Const ChangePassword As String = "ChangePassword"

        ' Accounting detailed permissions
        Public Const AccountingSettings As String = "AccountingSettings"
        Public Const AccountingHeader As String = "AccountingHeader"
        Public Const AccountingShenavar As String = "AccountingShenavar"
        Public Const AccountingEntry As String = "AccountingEntry"
        Public Const AccountingBank As String = "AccountingBank"
        Public Const AccountingBalance As String = "AccountingBalance"
        Public Const AccountingTarazShenavar As String = "AccountingTarazShenavar"
        Public Const AccountingLedger As String = "AccountingLedger"
        Public Const AccountingDaftarShenavar As String = "AccountingDaftarShenavar"
        Public Const AccountingReports As String = "AccountingReports"
        Public Const AccountingProfitLoss As String = "AccountingProfitLoss"
        Public Const AccountingBalanceSheet As String = "AccountingBalanceSheet"
        Public Const AccountingAdvancedReports As String = "AccountingAdvancedReports"
        Public Const AccountingChartReports As String = "AccountingChartReports"
        Public Const AccountingCustomReports As String = "AccountingCustomReports"
        Public Const ManageAccounting As String = "ManageAccounting"

        ' Accounting detailed print/export buttons permissions
        Public Const AccountingSanad1PrintDocs As String = "AccountingSanad1PrintDocs"
        Public Const AccountingSanad1PrintJournal As String = "AccountingSanad1PrintJournal"
        Public Const AccountingSanad2PrintVoucher As String = "AccountingSanad2PrintVoucher"
        Public Const AccountingBankRecExportExcel As String = "AccountingBankRecExportExcel"
        Public Const AccountingTrialPrint As String = "AccountingTrialPrint"
        Public Const AccountingTrialExport As String = "AccountingTrialExport"
        Public Const AccountingLedgerPrint As String = "AccountingLedgerPrint"
        Public Const AccountingLedgerExport As String = "AccountingLedgerExport"
        Public Const AccountingTarazShenavarPrint As String = "AccountingTarazShenavarPrint"
        Public Const AccountingTarazShenavarExport As String = "AccountingTarazShenavarExport"
        Public Const AccountingDaftarShenavarPrint As String = "AccountingDaftarShenavarPrint"
        Public Const AccountingDaftarShenavarExport As String = "AccountingDaftarShenavarExport"
        Public Const AccountingProfitLossPrint As String = "AccountingProfitLossPrint"
        Public Const AccountingProfitLossExport As String = "AccountingProfitLossExport"
        Public Const AccountingBalanceSheetPrint As String = "AccountingBalanceSheetPrint"
        Public Const AccountingBalanceSheetExport As String = "AccountingBalanceSheetExport"
        Public Const AccountingCustomReportPrint As String = "AccountingCustomReportPrint"
        Public Const AccountingCustomReportExport As String = "AccountingCustomReportExport"

        ' Trade & Warehousing detailed permissions
        Public Const TradeProductUnits As String = "TradeProductUnits"
        Public Const TradeProductGroups As String = "TradeProductGroups"
        Public Const TradeProducts As String = "TradeProducts"
        Public Const TradeWarehouses As String = "TradeWarehouses"
        Public Const TradePurchase As String = "TradePurchase"
        Public Const TradeSales As String = "TradeSales"
        Public Const TradeRemittance As String = "TradeRemittance"
        Public Const TradeReports As String = "TradeReports"
        Public Const ManageProducts As String = "ManageProducts"
        Public Const ManageWarehouses As String = "ManageWarehouses"
        Public Const ManagePurchases As String = "ManagePurchases"
        Public Const ManageSales As String = "ManageSales"
        Public Const ViewInventory As String = "ViewInventory"
        Public Const ManageTradeWarehouse As String = "ManageTradeWarehouse"
        Public Const ViewReports As String = "ViewReports"

        ' Anbar Edition Modules Permissions
        Public Const AnbarMiniModule As String = "AnbarMiniModule"
        Public Const AnbarMediumModule As String = "AnbarMediumModule"
        Public Const AnbarBigModule As String = "AnbarBigModule"
        Public Const AnbarMiniExpenses As String = "AnbarMiniExpenses"
        Public Const AnbarMiniExpenseLedger As String = "AnbarMiniExpenseLedger"
        Public Const AnbarMiniProfitLoss As String = "AnbarMiniProfitLoss"

        ' CRUD suffixes
        Public Const CanCreate As String = ".CanCreate"
        Public Const CanEdit As String = ".CanEdit"
        Public Const CanDelete As String = ".CanDelete"
        Public Const CanPrint As String = ".CanPrint"
        Public Const CanExport As String = ".CanExport"
    End Module
End Namespace
