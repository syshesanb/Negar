Option Strict Off
Option Explicit On

Imports System

Namespace Sys_Hes_Anb.Business
    Public Module PermissionKeys
        Public Const ManageUsers As String = "ManageUsers"
        Public Const ManageBasicUsers As String = "ManageBasicUsers"
        Public Const ManageCompanies As String = "ManageCompanies"
        Public Const ManageFiscalYears As String = "ManageFiscalYears"
        Public Const ManageCompaniesYears As String = "ManageCompaniesYears"
        Public Const SelectCompanyFiscalYear As String = "SelectCompanyFiscalYear"
        Public Const ManageSettings As String = "ManageSettings"
        Public Const BackupData As String = "BackupData"
        Public Const RestoreData As String = "RestoreData"
        Public Const ManageBusinessShells As String = "ManageBusinessShells"
        Public Const ManageUtilities As String = "ManageUtilities"
        Public Const ViewActivityLog As String = "ViewActivityLog"
        Public Const LockSanad1 As String = "LockSanad1"
        Public Const HideSFSHInSanad As String = "HideSFSHInSanad"

        ' Accounting detailed permissions
        Public Const AccountingHeader As String = "AccountingHeader"
        Public Const AccountingShenavar As String = "AccountingShenavar"
        Public Const AccountingEntry As String = "AccountingEntry"
        Public Const AccountingBank As String = "AccountingBank"
        Public Const AccountingBalance As String = "AccountingBalance"
        Public Const AccountingLedger As String = "AccountingLedger"
        Public Const AccountingReports As String = "AccountingReports"
        Public Const ManageAccounting As String = "ManageAccounting"

        ' Trade & Warehousing detailed permissions
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

        ' CRUD suffixes
        Public Const CanCreate As String = ".CanCreate"
        Public Const CanEdit As String = ".CanEdit"
        Public Const CanDelete As String = ".CanDelete"
        Public Const CanPrint As String = ".CanPrint"
        Public Const CanExport As String = ".CanExport"
    End Module
End Namespace
