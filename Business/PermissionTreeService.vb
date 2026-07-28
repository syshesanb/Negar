Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports Negar.Data

Namespace Negar.Business
    ''' <summary>
    ''' Represents a Node in the 5-Level Permission Tree Hierarchy:
    ''' Level 0: Top-Level Main Menu (منوی اصلی سیستم)
    ''' Level 1: Direct SubMenu (زیر منوهای فرعی سیستم)
    ''' Level 2: Main Form Tabs (تب‌های اصلی در فرم‌ها)
    ''' Level 3: Sub-Tabs & Sub-Forms (زیر تب‌ها و بخش‌های داخلی)
    ''' Level 4: Action Buttons & Controls (دکمه‌ها و سایر کنترل‌ها)
    ''' </summary>
    Public Class PermissionTreeNode
        Public Property Key As String
        Public Property Title As String
        Public Property Level As Integer
        Public Property PermissionID As Integer?
        Public Property PermissionKey As String
        Public Property Children As New List(Of PermissionTreeNode)()
        Public Property DependsOnKeys As New List(Of String)()

        Public Sub New(key As String, title As String, level As Integer, Optional permKey As String = Nothing, Optional dependsOn As String() = Nothing)
            Me.Key = key
            Me.Title = title
            Me.Level = level
            Me.PermissionKey = permKey
            If dependsOn IsNot Nothing Then
                Me.DependsOnKeys.AddRange(dependsOn)
            End If
        End Sub
    End Class

    Public Class PermissionTreeService
        Public Function BuildDynamicTree() As List(Of PermissionTreeNode)
            Dim roots As New List(Of PermissionTreeNode)()
            Dim dbPermissions = FetchDbPermissionsMap()

            ' =========================================================================
            ' 1. منوی اصلی: خریدو فروش و انبارداری (mTradeWarehouse)
            ' =========================================================================
            Dim rTrade As New PermissionTreeNode("MENU_TRADE", "🛒 منوی اصلی: خریدو فروش و انبارداری", 0)

            ' 1.1 زیر منو: استفاده از انبارداری مینی (miTradeMini)
            Dim smTradeMini As New PermissionTreeNode("SM_TRADE_MINI", "📁 زیرمنو: استفاده از انبارداری مینی", 1)
            
            Dim tMiniPos As New PermissionTreeNode("T_MINI_POS", "📄 تب فروش سریع (POS)", 2)
            Dim stMiniPosGrid As New PermissionTreeNode("ST_MINI_POS_GRID", "📑 زیرتب صدور فاکتور و تسویه کارتخوان/نقد", 3)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniModule, "🔘 استفاده از انبارداری مینی", dbPermissions)
            AddActionNode(stMiniPosGrid, PermissionKeys.TradeSales, "🔘 صدور و ورود اطلاعات فاکتور فروش", dbPermissions)
            tMiniPos.Children.Add(stMiniPosGrid)
            smTradeMini.Children.Add(tMiniPos)

            Dim tMiniKharid As New PermissionTreeNode("T_MINI_KHARID", "📄 تب خرید کالا", 2)
            Dim stMiniKharidGrid As New PermissionTreeNode("ST_MINI_KHARID_GRID", "📑 زیرتب ثبت فاکتورهای خرید کالا", 3)
            AddActionNode(stMiniKharidGrid, PermissionKeys.TradePurchase, "🔘 صدور فاکتور خرید", dbPermissions)
            tMiniKharid.Children.Add(stMiniKharidGrid)
            smTradeMini.Children.Add(tMiniKharid)

            Dim tMiniPersons As New PermissionTreeNode("T_MINI_PERSONS", "📄 تب لیست فروشنده و خریدار", 2)
            Dim stMiniPersonsGrid As New PermissionTreeNode("ST_MINI_PERSONS_GRID", "📑 زیرتب طرف حساب‌ها و اشخاص", 3)
            AddActionNode(stMiniPersonsGrid, PermissionKeys.TradeProducts, "🔘 ثبت و ویرایش طرف حساب‌ها", dbPermissions)
            tMiniPersons.Children.Add(stMiniPersonsGrid)
            smTradeMini.Children.Add(tMiniPersons)

            Dim tMiniExpenses As New PermissionTreeNode("T_MINI_EXPENSES", "📄 تب هزینه‌ها", 2)
            Dim stMiniExpensesToolbar As New PermissionTreeNode("ST_MINI_EXPENSES_TB", "📑 زیرتب ثبت هزینه‌ها و نوار ابزار", 3)
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.TradeReports, "🔘 ثبت و ویرایش اسناد هزینه", dbPermissions)
            tMiniExpenses.Children.Add(stMiniExpensesToolbar)

            Dim stMiniExpenseLedger As New PermissionTreeNode("ST_MINI_EXPENSE_LEDGER", "📑 دیالوگ دفتر هزینه (سرفصل/عنوان)", 3)
            AddActionNode(stMiniExpenseLedger, PermissionKeys.ViewReports, "🔘 📒 تهیه و چاپ دفتر هزینه", dbPermissions)
            tMiniExpenses.Children.Add(stMiniExpenseLedger)

            Dim stMiniProfitLoss As New PermissionTreeNode("ST_MINI_PROFIT_LOSS", "📑 پیش‌نمایش چاپی عملکرد و سود و زیان", 3)
            AddActionNode(stMiniProfitLoss, PermissionKeys.AccountingProfitLoss, "🔘 🖨️ چاپ عملکرد و سود و زیان", dbPermissions)
            tMiniExpenses.Children.Add(stMiniProfitLoss)
            smTradeMini.Children.Add(tMiniExpenses)

            Dim tMiniProducts As New PermissionTreeNode("T_MINI_PRODS", "📄 تب لیست کالاها", 2)
            Dim stMiniProdsGrid As New PermissionTreeNode("ST_MINI_PRODS_GRID", "📑 زیرتب مدیریت کالاها و قیمت‌ها", 3)
            AddActionNode(stMiniProdsGrid, PermissionKeys.ManageProducts, "🔘 مدیریت کالاها و خدمات", dbPermissions)
            tMiniProducts.Children.Add(stMiniProdsGrid)
            smTradeMini.Children.Add(tMiniProducts)

            Dim tMiniWarehouses As New PermissionTreeNode("T_MINI_WHS", "📄 تب لیست انبارها", 2)
            Dim stMiniWhsGrid As New PermissionTreeNode("ST_MINI_WHS_GRID", "📑 زیرتب تعریف انبارها و موجودی", 3)
            AddActionNode(stMiniWhsGrid, PermissionKeys.ManageWarehouses, "🔘 مدیریت انبارها", dbPermissions)
            AddActionNode(stMiniWhsGrid, PermissionKeys.ViewInventory, "🔘 مشاهده موجودی انبارها", dbPermissions)
            tMiniWarehouses.Children.Add(stMiniWhsGrid)
            smTradeMini.Children.Add(tMiniWarehouses)

            rTrade.Children.Add(smTradeMini)

            ' 1.2 زیر منو: استفاده از انبارداری متوسط (miTradeMedium)
            Dim smTradeMed As New PermissionTreeNode("SM_TRADE_MED", "📁 زیرمنو: استفاده از انبارداری متوسط", 1)
            Dim tMedMain As New PermissionTreeNode("T_MED_MAIN", "📄 تب اصلی انبارداری متوسط", 2)
            Dim stMedGrid As New PermissionTreeNode("ST_MED_GRID", "📑 زیرتب عملیات متوسط انبار و کالا", 3)
            AddActionNode(stMedGrid, PermissionKeys.AnbarMediumModule, "🔘 استفاده از انبارداری متوسط", dbPermissions)
            tMedMain.Children.Add(stMedGrid)
            smTradeMed.Children.Add(tMedMain)
            rTrade.Children.Add(smTradeMed)

            ' 1.3 زیر منو: استفاده از انبارداری پیشرفته (miTradeBig)
            Dim smTradeBig As New PermissionTreeNode("SM_TRADE_BIG", "📁 زیرمنو: استفاده از انبارداری پیشرفته", 1)
            Dim tBigMain As New PermissionTreeNode("T_BIG_MAIN", "📄 تب اصلی انبارداری پیشرفته", 2)
            Dim stBigGrid As New PermissionTreeNode("ST_BIG_GRID", "📑 زیرتب کنترل پیشرفته چند انباره", 3)
            AddActionNode(stBigGrid, PermissionKeys.AnbarBigModule, "🔘 استفاده از انبارداری پیشرفته", dbPermissions)
            tBigMain.Children.Add(stBigGrid)
            smTradeBig.Children.Add(tBigMain)
            rTrade.Children.Add(smTradeBig)

            ' 1.4 زیر منو: فاکتورها، انبار و مدیریت کالاها (جامع) (miTradeWarehouseMain)
            Dim smTradeComprehensive As New PermissionTreeNode("SM_TRADE_COMP", "📁 زیرمنو: فاکتورها، انبار و مدیریت کالاها (جامع)", 1)
            
            Dim tCompProds As New PermissionTreeNode("T_COMP_PRODS", "📄 تب تعریف کالاها و خدمات", 2)
            Dim stCompProds As New PermissionTreeNode("ST_COMP_PRODS", "📑 زیرتب لیست کالاها و واحدهای سنجش", 3)
            AddActionNode(stCompProds, PermissionKeys.TradeProducts, "🔘 تعریف کالاها و خدمات", dbPermissions)
            tCompProds.Children.Add(stCompProds)
            smTradeComprehensive.Children.Add(tCompProds)

            Dim tCompWhs As New PermissionTreeNode("T_COMP_WHS", "📄 تب تعریف انبارها", 2)
            Dim stCompWhs As New PermissionTreeNode("ST_COMP_WHS", "📑 زیرتب تعریف انبارها و جانمایی", 3)
            AddActionNode(stCompWhs, PermissionKeys.TradeWarehouses, "🔘 تعریف انبارها", dbPermissions)
            tCompWhs.Children.Add(stCompWhs)
            smTradeComprehensive.Children.Add(tCompWhs)

            Dim tCompPurch As New PermissionTreeNode("T_COMP_PURCH", "📄 تب صدور فاکتور خرید", 2)
            Dim stCompPurch As New PermissionTreeNode("ST_COMP_PURCH", "📑 زیرتب فاکتورهای خرید", 3)
            AddActionNode(stCompPurch, PermissionKeys.ManagePurchases, "🔘 مدیریت خرید و رسید انبار", dbPermissions)
            tCompPurch.Children.Add(stCompPurch)
            smTradeComprehensive.Children.Add(tCompPurch)

            Dim tCompSales As New PermissionTreeNode("T_COMP_SALES", "📄 تب صدور فاکتور فروش", 2)
            Dim stCompSales As New PermissionTreeNode("ST_COMP_SALES", "📑 زیرتب فاکتورهای فروش", 3)
            AddActionNode(stCompSales, PermissionKeys.ManageSales, "🔘 مدیریت فروش و حواله خروجی", dbPermissions)
            tCompSales.Children.Add(stCompSales)
            smTradeComprehensive.Children.Add(tCompSales)

            Dim tCompRemit As New PermissionTreeNode("T_COMP_REMIT", "📄 تب حواله و انتقال کالا بین انبارها", 2)
            Dim stCompRemit As New PermissionTreeNode("ST_COMP_REMIT", "📑 زیرتب جابه‌جایی بین انبارها", 3)
            AddActionNode(stCompRemit, PermissionKeys.TradeRemittance, "🔘 حواله و رسید انبار", dbPermissions)
            AddActionNode(stCompRemit, PermissionKeys.ManageTradeWarehouse, "🔘 خرید و فروش و انبارداری (جامع)", dbPermissions)
            tCompRemit.Children.Add(stCompRemit)
            smTradeComprehensive.Children.Add(tCompRemit)

            rTrade.Children.Add(smTradeComprehensive)

            ' 1.5 زیر منو: گزارشات فاکتورها و موجودی انبار (miReportsTrade)
            Dim smTradeReports As New PermissionTreeNode("SM_TRADE_REPORTS", "📁 زیرمنو: گزارشات فاکتورها و موجودی انبار", 1)
            Dim tTradeRepMain As New PermissionTreeNode("T_TRADE_REP_MAIN", "📄 تب گزارشات موجودی و کاردکس", 2)
            Dim stTradeRepGrid As New PermissionTreeNode("ST_TRADE_REP_GRID", "📑 زیرتب کاردکس کالا و مرور فاکتورها", 3)
            AddActionNode(stTradeRepGrid, PermissionKeys.TradeReports, "🔘 گزارشات انبار و کاردکس کالا", dbPermissions)
            tTradeRepMain.Children.Add(stTradeRepGrid)
            smTradeReports.Children.Add(tTradeRepMain)
            rTrade.Children.Add(smTradeReports)

            roots.Add(rTrade)

            ' =========================================================================
            ' 2. منوی اصلی: حسابداری (mAccounting)
            ' =========================================================================
            Dim rAccounting As New PermissionTreeNode("MENU_ACCOUNTING", "📊 منوی اصلی: حسابداری", 0)

            ' 2.1 زیر منو: کدینگ، ثبت اسناد و دفاتر (miAccountingMain)
            Dim smAccMain As New PermissionTreeNode("SM_ACC_MAIN", "📁 زیرمنو: کدینگ، ثبت اسناد و دفاتر", 1)

            Dim tCodingHeader As New PermissionTreeNode("T_CODING_HEADER", "📄 تب سرفصل حساب‌ها (کدینگ)", 2)
            Dim stCodingHeader As New PermissionTreeNode("ST_CODING_HEADER", "📑 زیرتب درختواره حساب‌های کل و معین", 3)
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeader, "🔘 سرفصل حساب‌ها (کدینگ)", dbPermissions)
            AddActionNode(stCodingHeader, PermissionKeys.ManageAccounting, "🔘 حسابداری (جامع)", dbPermissions)
            tCodingHeader.Children.Add(stCodingHeader)
            smAccMain.Children.Add(tCodingHeader)

            Dim tShenavar As New PermissionTreeNode("T_SHENAVAR", "📄 تب حساب‌های شناور", 2)
            Dim stShenavar As New PermissionTreeNode("ST_SHENAVAR", "📑 زیرتب درختواره شناور اشخاص و مراکز", 3)
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavar, "🔘 حساب‌های شناور", dbPermissions)
            tShenavar.Children.Add(stShenavar)
            smAccMain.Children.Add(tShenavar)

            Dim tSanad As New PermissionTreeNode("T_SANAD", "📄 تب ثبت و ویرایش سند حسابداری", 2)
            Dim stSanadGrid As New PermissionTreeNode("ST_SANAD_GRID", "📑 زیرتب جدول سطرها و ثبت اسناد", 3)
            AddActionNode(stSanadGrid, PermissionKeys.AccountingEntry, "🔘 ثبت سند حسابداری", dbPermissions)
            AddActionNode(stSanadGrid, PermissionKeys.LockSanad1, "🔘 قطعی‌سازی و قفل اسناد", dbPermissions)
            AddActionNode(stSanadGrid, PermissionKeys.HideSFSHInSanad, "🔘 مخفی کردن ستون‌های SF/SH", dbPermissions)
            tSanad.Children.Add(stSanadGrid)

            Dim stSanadPrint As New PermissionTreeNode("ST_SANAD_PRINT", "📑 زیرتب چاپ اسناد و دفاتر روزنامه", 3)
            AddActionNode(stSanadPrint, PermissionKeys.AccountingSanad1PrintDocs, "🔘 چاپ اسناد", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadPrint, PermissionKeys.AccountingSanad1PrintJournal, "🔘 چاپ دفتر روزنامه", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadPrint, PermissionKeys.AccountingSanad2PrintVoucher, "🔘 چاپ سند (Ctrl+P)", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            tSanad.Children.Add(stSanadPrint)
            smAccMain.Children.Add(tSanad)

            Dim tMogBank As New PermissionTreeNode("T_MOG_BANK", "📄 تب مغایرت‌گیری بانکی", 2)
            Dim stMogBank As New PermissionTreeNode("ST_MOG_BANK", "📑 زیرتب تطبیق صورت‌حساب بانک", 3)
            AddActionNode(stMogBank, PermissionKeys.AccountingBank, "🔘 مغایرت‌های بانکی", dbPermissions)
            AddActionNode(stMogBank, PermissionKeys.AccountingBankRecExportExcel, "🔘 خروجی اکسل مغایرت", dbPermissions, new String() { PermissionKeys.AccountingBank })
            tMogBank.Children.Add(stMogBank)
            smAccMain.Children.Add(tMogBank)

            Dim tDaftar As New PermissionTreeNode("T_DAFTAR", "📄 تب دفاتر حساب (روزنامه/کل/معین)", 2)
            Dim stDaftarGrid As New PermissionTreeNode("ST_DAFTAR_GRID", "📑 زیرتب مرور دفاتر و گردش حساب‌ها", 3)
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedger, "🔘 دفتر حساب", dbPermissions)
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedgerPrint, "🔘 چاپ دفتر حساب", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedgerExport, "🔘 خروجی اکسل دفتر", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingDaftarShenavarPrint, "🔘 چاپ دفتر شناور", dbPermissions)
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingDaftarShenavarExport, "🔘 خروجی اکسل دفتر شناور", dbPermissions)
            tDaftar.Children.Add(stDaftarGrid)
            smAccMain.Children.Add(tDaftar)

            rAccounting.Children.Add(smAccMain)

            ' 2.2 زیر منو: گزارشات و ترازهای حسابداری (miReportsAccounting)
            Dim smAccReports As New PermissionTreeNode("SM_ACC_REPORTS", "📁 زیرمنو: گزارشات و ترازهای حسابداری", 1)

            Dim tTaraz As New PermissionTreeNode("T_TARAZ", "📄 تب ترازهای مالی (۲، ۴ و ۸ ستونی)", 2)
            Dim stTarazGrid As New PermissionTreeNode("ST_TARAZ_GRID", "📑 زیرتب محاسبه تراز آزمایشی و شناور", 3)
            AddActionNode(stTarazGrid, PermissionKeys.AccountingBalance, "🔘 تراز آزمایشی", dbPermissions)
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTrialPrint, "🔘 چاپ تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTrialExport, "🔘 خروجی اکسل تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTarazShenavarPrint, "🔘 چاپ تراز شناور", dbPermissions)
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTarazShenavarExport, "🔘 خروجی اکسل تراز شناور", dbPermissions)
            tTaraz.Children.Add(stTarazGrid)
            smAccReports.Children.Add(tTaraz)

            Dim tProfitLossSheet As New PermissionTreeNode("T_PROFIT_LOSS_SHEET", "📄 تب صورت‌های مالی (سود و زیان / ترازنامه)", 2)
            Dim stProfitLossSheet As New PermissionTreeNode("ST_PROFIT_LOSS_SHEET", "📑 زیرتب گزارش عملکرد، سود/زیان و ترازنامه", 3)
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingProfitLoss, "🔘 صورت سود و زیان", dbPermissions)
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingProfitLossPrint, "🔘 چاپ سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingProfitLossExport, "🔘 خروجی اکسل سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingBalanceSheet, "🔘 ترازنامه مالی", dbPermissions)
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingBalanceSheetPrint, "🔘 چاپ ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stProfitLossSheet, PermissionKeys.AccountingBalanceSheetExport, "🔘 خروجی اکسل ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            tProfitLossSheet.Children.Add(stProfitLossSheet)
            smAccReports.Children.Add(tProfitLossSheet)

            Dim tAdvReports As New PermissionTreeNode("T_ADV_REPORTS", "📄 تب گزارشات پیشرفته و نموداری", 2)
            Dim stAdvReports As New PermissionTreeNode("ST_ADV_REPORTS", "📑 زیرتب طراحی گزارشات دلخواه و گرافیکی", 3)
            AddActionNode(stAdvReports, PermissionKeys.AccountingReports, "🔘 گزارشات حسابداری", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingAdvancedReports, "🔘 گزارشات پیشرفته", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingChartReports, "🔘 گزارشات نموداری", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReports, "🔘 گزارشات دلخواه", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReportPrint, "🔘 چاپ گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReportExport, "🔘 خروجی اکسل گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            tAdvReports.Children.Add(stAdvReports)
            smAccReports.Children.Add(tAdvReports)

            rAccounting.Children.Add(smAccReports)
            roots.Add(rAccounting)

            ' =========================================================================
            ' 3. منوی اصلی: کاربران (mUserMgmt)
            ' =========================================================================
            Dim rUsers As New PermissionTreeNode("MENU_USERS", "👥 منوی اصلی: کاربران", 0)

            Dim smUsersComprehensive As New PermissionTreeNode("SM_USERS_COMP", "📁 زیرمنو: مدیریت کاربران (جامع)", 1)
            Dim tUsersList As New PermissionTreeNode("T_USERS_LIST", "📄 تب کاربران سیستم", 2)
            Dim stUsersList As New PermissionTreeNode("ST_USERS_LIST", "📑 زیرتب لیست و ویرایش کاربران", 3)
            AddActionNode(stUsersList, PermissionKeys.ManageUsers, "🔘 مدیریت کاربران (جامع)", dbPermissions)
            tUsersList.Children.Add(stUsersList)
            smUsersComprehensive.Children.Add(tUsersList)

            Dim tUsersPerms As New PermissionTreeNode("T_USERS_PERMS", "📄 تب سطح دسترسی‌ها", 2)
            Dim stUsersPerms As New PermissionTreeNode("ST_USERS_PERMS", "📑 زیرتب تنظیم درختی دسترسی‌ها و الگوها", 3)
            AddActionNode(stUsersPerms, PermissionKeys.ViewActivityLog, "🔘 مشاهده دفتر سوابق و لاگ فعالیت‌ها", dbPermissions)
            tUsersPerms.Children.Add(stUsersPerms)
            smUsersComprehensive.Children.Add(tUsersPerms)
            rUsers.Children.Add(smUsersComprehensive)

            Dim smUsersBasic As New PermissionTreeNode("SM_USERS_BASIC", "📁 زیرمنو: مدیریت کاربران – مدیریت کاربران عادی", 1)
            Dim tBasicUsers As New PermissionTreeNode("T_BASIC_USERS", "📄 تب کاربران عادی", 2)
            Dim stBasicUsers As New PermissionTreeNode("ST_BASIC_USERS", "📑 زیرتب تعاریف اپراتورها", 3)
            AddActionNode(stBasicUsers, PermissionKeys.ManageBasicUsers, "🔘 مدیریت کاربران عادی", dbPermissions)
            tBasicUsers.Children.Add(stBasicUsers)
            smUsersBasic.Children.Add(tBasicUsers)
            rUsers.Children.Add(smUsersBasic)

            Dim smChangeProfile As New PermissionTreeNode("SM_CHANGE_PROFILE", "📁 زیرمنو: تغییر کلمه عبور", 1)
            Dim tChangePass As New PermissionTreeNode("T_CHANGE_PASS", "📄 تب تغییر پروفایل و رمزیابی", 2)
            Dim stChangePass As New PermissionTreeNode("ST_CHANGE_PASS", "📑 زیرتب تغییر رمز عبور کاربر", 3)
            AddActionNode(stChangePass, PermissionKeys.ChangePassword, "🔘 تغییر کلمه عبور", dbPermissions)
            tChangePass.Children.Add(stChangePass)
            smChangeProfile.Children.Add(tChangePass)
            rUsers.Children.Add(smChangeProfile)

            Dim smSwitchUser As New PermissionTreeNode("SM_SWITCH_USER", "📁 زیرمنو: ورود با کاربر دیگر", 1)
            Dim tSwitchUser As New PermissionTreeNode("T_SWITCH_USER", "📄 تب تعویض کاربر لاگین", 2)
            Dim stSwitchUser As New PermissionTreeNode("ST_SWITCH_USER", "📑 زیرتب انتخاب کاربر جدید", 3)
            AddActionNode(stSwitchUser, PermissionKeys.SwitchUser, "🔘 ورود با کاربر دیگر", dbPermissions)
            tSwitchUser.Children.Add(stSwitchUser)
            smSwitchUser.Children.Add(tSwitchUser)
            rUsers.Children.Add(smSwitchUser)

            roots.Add(rUsers)

            ' =========================================================================
            ' 4. منوی اصلی: شرکت‌ها و سال‌های مالی (mCompanyMgmt)
            ' =========================================================================
            Dim rCompanies As New PermissionTreeNode("MENU_COMPANIES", "🏢 منوی اصلی: شرکت‌ها و سال‌های مالی", 0)

            Dim smCompaniesYears As New PermissionTreeNode("SM_COMP_YEARS", "📁 زیرمنو: مدیریت شرکت‌ها و سال‌های مالی", 1)
            Dim tCompaniesList As New PermissionTreeNode("T_COMP_LIST", "📄 تب تعریف شرکت‌ها", 2)
            Dim stCompaniesGrid As New PermissionTreeNode("ST_COMP_GRID", "📑 زیرتب لیست شرکت‌ها و کدهای اقتصادی", 3)
            AddActionNode(stCompaniesGrid, PermissionKeys.ManageCompanies, "🔘 مدیریت شرکت‌ها", dbPermissions)
            AddActionNode(stCompaniesGrid, PermissionKeys.ManageCompaniesYears, "🔘 مدیریت شرکت‌ها و سال‌های مالی (جامع)", dbPermissions)
            tCompaniesList.Children.Add(stCompaniesGrid)
            smCompaniesYears.Children.Add(tCompaniesList)

            Dim tFiscalYearsList As New PermissionTreeNode("T_FY_LIST", "📄 تب سال‌های مالی", 2)
            Dim stFiscalYearsGrid As New PermissionTreeNode("ST_FY_GRID", "📑 زیرتب تعریف دوره‌های مالی", 3)
            AddActionNode(stFiscalYearsGrid, PermissionKeys.ManageFiscalYears, "🔘 مدیریت سال‌های مالی", dbPermissions)
            tFiscalYearsList.Children.Add(stFiscalYearsGrid)
            smCompaniesYears.Children.Add(tFiscalYearsList)

            Dim tSelectActiveCompany As New PermissionTreeNode("T_SELECT_ACTIVE", "📄 تب انتخاب شرکت و سال مالی فعال", 2)
            Dim stSelectActive As New PermissionTreeNode("ST_SELECT_ACTIVE", "📑 زیرتب فعال‌سازی محیط کاری", 3)
            AddActionNode(stSelectActive, PermissionKeys.SelectCompanyFiscalYear, "🔘 انتخاب شرکت و سال مالی جاری", dbPermissions)
            tSelectActiveCompany.Children.Add(stSelectActive)
            smCompaniesYears.Children.Add(tSelectActiveCompany)

            rCompanies.Children.Add(smCompaniesYears)
            roots.Add(rCompanies)

            ' =========================================================================
            ' 5. منوی اصلی: پوسته مشاغل (mBusinessShells)
            ' =========================================================================
            Dim rBusinessShells As New PermissionTreeNode("MENU_SHELLS", "🏢 منوی اصلی: پوسته مشاغل", 0)
            Dim smShells As New PermissionTreeNode("SM_SHELLS", "📁 زیرمنو: انتخاب و پیکربندی پوسته مشاغل", 1)
            Dim tShells As New PermissionTreeNode("T_SHELLS", "📄 تب پوسته‌ها و چیدمان اصناف", 2)
            Dim stShells As New PermissionTreeNode("ST_SHELLS", "📑 زیرتب پوسته‌های عمومی، فروشگاهی و خدماتی", 3)
            AddActionNode(stShells, PermissionKeys.ManageBusinessShells, "🔘 پوسته مشاغل", dbPermissions)
            tShells.Children.Add(stShells)
            smShells.Children.Add(tShells)
            rBusinessShells.Children.Add(smShells)
            roots.Add(rBusinessShells)

            ' =========================================================================
            ' 6. منوی اصلی: امکانات و ابزارها (mUtilities & mSystemMgmt)
            ' =========================================================================
            Dim rSysMgmt As New PermissionTreeNode("MENU_SYS_MGMT", "🛠️ منوی اصلی: سیستم، امکانات و پشتیبان‌گیری", 0)

            Dim smThemes As New PermissionTreeNode("SM_THEMES", "📁 زیرمنو: مدیریت تمهای برنامه و فرمها", 1)
            Dim tThemes As New PermissionTreeNode("T_THEMES_M", "📄 تب ظاهر و تم‌ها", 2)
            Dim stThemes As New PermissionTreeNode("ST_THEMES_M", "📑 زیرتب تغییر رنگ و آیکون‌ها", 3)
            AddActionNode(stThemes, PermissionKeys.ManageAppThemes, "🔘 مدیریت تم‌های برنامه و فرم‌ها", dbPermissions)
            tThemes.Children.Add(stThemes)
            smThemes.Children.Add(tThemes)
            rSysMgmt.Children.Add(smThemes)

            Dim smBackup As New PermissionTreeNode("SM_BACKUP", "📁 زیرمنو: پشتیبان‌گیری و بازیابی اطلاعات", 1)
            Dim tBackup As New PermissionTreeNode("T_BACKUP_M", "📄 تب پایگاه داده", 2)
            Dim stBackup As New PermissionTreeNode("ST_BACKUP_M", "📑 زیرتب تهیه و بازگردانی نسخه پشتیبان", 3)
            AddActionNode(stBackup, PermissionKeys.BackupData, "🔘 پشتیبان‌گیری اطلاعات", dbPermissions)
            AddActionNode(stBackup, PermissionKeys.RestoreData, "🔘 بازیابی اطلاعات", dbPermissions)
            tBackup.Children.Add(stBackup)
            smBackup.Children.Add(tBackup)
            rSysMgmt.Children.Add(smBackup)

            Dim smUtils As New PermissionTreeNode("SM_UTILS", "📁 زیرمنو: ماشین حساب، یادداشت و تقویم", 1)
            Dim tUtils As New PermissionTreeNode("T_UTILS_M", "📄 تب ابزارهای جانبی", 2)
            Dim stUtils As New PermissionTreeNode("ST_UTILS_M", "📑 زیرتب یادداشت‌ها، تقویم و مناسبت‌ها", 3)
            AddActionNode(stUtils, PermissionKeys.ManageUtilities, "🔘 امکانات", dbPermissions)
            tUtils.Children.Add(stUtils)
            smUtils.Children.Add(tUtils)
            rSysMgmt.Children.Add(smUtils)

            roots.Add(rSysMgmt)

            ' Dynamic Scanner: Automatically discover any unmapped permissions in DB!
            Dim mappedKeys = GetAllMappedPermissionKeys(roots)
            Dim unmappedList As New List(Of KeyValuePair(Of String, Integer))()

            For Each kvp In dbPermissions
                If Not mappedKeys.Contains(kvp.Key) Then
                    unmappedList.Add(kvp)
                End If
            Next

            If unmappedList.Count > 0 Then
                Dim rNew As New PermissionTreeNode("MENU_NEW", "⚡ منوی اصلی: مجوزها و امکانات جدید سیستم", 0)
                Dim smNew As New PermissionTreeNode("SM_NEW", "📁 زیرمنو: امکانات جدید افزوده شده در به روزرسانی‌ها", 1)
                Dim tNew As New PermissionTreeNode("T_NEW", "📄 تب مجوزهای جدید", 2)
                Dim stNew As New PermissionTreeNode("ST_NEW", "📑 زیرتب دسترسی‌های شناسایی‌شده", 3)

                For Each kvp In unmappedList
                    AddActionNode(stNew, kvp.Key, "🔘 " & kvp.Key, dbPermissions)
                Next

                tNew.Children.Add(stNew)
                smNew.Children.Add(tNew)
                rNew.Children.Add(smNew)
                roots.Add(rNew)
            End If

            Return roots
        End Function

        Private Sub AddActionNode(parent As PermissionTreeNode, permKey As String, title As String, dbMap As Dictionary(Of String, Integer), Optional dependsOn As String() = Nothing)
            Dim node As New PermissionTreeNode(permKey, title, 4, permKey, dependsOn)
            If dbMap.ContainsKey(permKey) Then
                node.PermissionID = dbMap(permKey)
            End If
            parent.Children.Add(node)
        End Sub

        Private Function FetchDbPermissionsMap() As Dictionary(Of String, Integer)
            Dim map As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            Try
                Dim dt = Sql.ExecuteTable("SELECT PermissionID, PermissionKey FROM Permissions")
                If dt IsNot Nothing Then
                    For Each row As DataRow In dt.Rows
                        Dim id = Convert.ToInt32(row("PermissionID"))
                        Dim key = Convert.ToString(row("PermissionKey"))
                        If Not map.ContainsKey(key) Then
                            map.Add(key, id)
                        End If
                    Next
                End If
            Catch
            End Try
            Return map
        End Function

        Private Function GetAllMappedPermissionKeys(roots As List(Of PermissionTreeNode)) As HashSet(Of String)
            Dim setKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each r In roots
                CollectKeysRecursive(r, setKeys)
            Next
            Return setKeys
        End Function

        Private Sub CollectKeysRecursive(node As PermissionTreeNode, setKeys As HashSet(Of String))
            If Not String.IsNullOrEmpty(node.PermissionKey) Then
                setKeys.Add(node.PermissionKey)
            End If
            For Each child In node.Children
                CollectKeysRecursive(child, setKeys)
            Next
        End Sub
    End Class
End Namespace
