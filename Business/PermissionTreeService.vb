Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports Negar.Data

Namespace Negar.Business
    Public Class PermissionTreeNode
        Public Property Key As String
        Public Property Title As String
        Public Property Level As Integer ' 0: Root Menu, 1: SubMenu, 2: MainTab, 3: SubTab, 4: Action/Button
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

            ' Map DB PermissionKeys to PermissionIDs
            Dim dbPermissions = FetchDbPermissionsMap()

            ' Root 1: مدیریت سیستم و تنظیمات
            Dim rSys As New PermissionTreeNode("ROOT_SYS", "🏠 مدیریت سیستم و تنظیمات", 0)
            
            Dim mUsers As New PermissionTreeNode("M_USERS", "📁 مدیریت کاربران و دسترسی‌ها", 1)
            Dim tUsers As New PermissionTreeNode("T_USERS", "📄 تب مدیریت کاربران", 2)
            Dim stUsersList As New PermissionTreeNode("ST_USERS_LIST", "📑 زیرتب لیست کاربران", 3)
            AddActionNode(stUsersList, PermissionKeys.ManageUsers, "🔘 مدیریت کاربران (جامع)", dbPermissions)
            AddActionNode(stUsersList, PermissionKeys.ManageBasicUsers, "🔘 مدیریت کاربران عادی", dbPermissions)
            AddActionNode(stUsersList, PermissionKeys.SwitchUser, "🔘 ورود با کاربر دیگر", dbPermissions)
            AddActionNode(stUsersList, PermissionKeys.ChangePassword, "🔘 تغییر کلمه عبور", dbPermissions)
            tUsers.Children.Add(stUsersList)

            Dim stPerms As New PermissionTreeNode("ST_PERMS", "📑 زیرتب تنظیم دسترسی کاربران", 3)
            AddActionNode(stPerms, PermissionKeys.ViewActivityLog, "🔘 مشاهده دفتر سوابق و لاگ فعالیت‌ها", dbPermissions)
            tUsers.Children.Add(stPerms)
            mUsers.Children.Add(tUsers)

            Dim mComp As New PermissionTreeNode("M_COMP", "📁 مدیریت شرکت‌ها و سال‌های مالی", 1)
            Dim tComp As New PermissionTreeNode("T_COMP", "📄 تب شرکت‌ها و سال مالی", 2)
            Dim stComp As New PermissionTreeNode("ST_COMP", "📑 زیرتب تعریف و انتخاب شرکت", 3)
            AddActionNode(stComp, PermissionKeys.ManageCompanies, "🔘 مدیریت شرکت‌ها", dbPermissions)
            AddActionNode(stComp, PermissionKeys.ManageFiscalYears, "🔘 مدیریت سال‌های مالی", dbPermissions)
            AddActionNode(stComp, PermissionKeys.ManageCompaniesYears, "🔘 مدیریت شرکت‌ها و سال‌های مالی (جامع)", dbPermissions)
            AddActionNode(stComp, PermissionKeys.SelectCompanyFiscalYear, "🔘 انتخاب شرکت و سال مالی جاری", dbPermissions)
            tComp.Children.Add(stComp)
            mComp.Children.Add(tComp)

            Dim mSysSettings As New PermissionTreeNode("M_SYS_SET", "📁 تنظیمات و پشتیبان‌گیری", 1)
            Dim tThemes As New PermissionTreeNode("T_THEMES", "📄 تب تنظیمات و تم", 2)
            Dim stThemes As New PermissionTreeNode("ST_THEMES", "📑 زیرتب تم‌ها و پوسته", 3)
            AddActionNode(stThemes, PermissionKeys.ManageAppThemes, "🔘 مدیریت تم‌های برنامه و فرم‌ها", dbPermissions)
            AddActionNode(stThemes, PermissionKeys.ManageBusinessShells, "🔘 پوسته مشاغل", dbPermissions)
            AddActionNode(stThemes, PermissionKeys.ManageUtilities, "🔘 امکانات و ابزارها", dbPermissions)
            tThemes.Children.Add(stThemes)
            mSysSettings.Children.Add(tThemes)

            Dim tBackup As New PermissionTreeNode("T_BACKUP", "📄 تب پشتیبان‌گیری و بازیابی", 2)
            Dim stBackup As New PermissionTreeNode("ST_BACKUP", "📑 زیرتب پایگاه داده", 3)
            AddActionNode(stBackup, PermissionKeys.BackupData, "🔘 پشتیبان‌گیری اطلاعات", dbPermissions)
            AddActionNode(stBackup, PermissionKeys.RestoreData, "🔘 بازیابی اطلاعات", dbPermissions)
            tBackup.Children.Add(stBackup)
            mSysSettings.Children.Add(tBackup)

            rSys.Children.Add(mUsers)
            rSys.Children.Add(mComp)
            rSys.Children.Add(mSysSettings)
            roots.Add(rSys)

            ' Root 2: خرید و فروش و انبارداری
            Dim rTrade As New PermissionTreeNode("ROOT_TRADE", "🛒 خرید و فروش و انبارداری", 0)
            
            Dim mAnbarModules As New PermissionTreeNode("M_ANBAR_MODS", "📁 ماژول‌های انبارداری", 1)
            Dim tAnbarMini As New PermissionTreeNode("T_ANBAR_MINI", "📄 تب انبارداری مینی", 2)
            Dim stAnbarMiniPos As New PermissionTreeNode("ST_ANBAR_MINI_POS", "📑 زیرتب فروش سریع (POS) و فاکتورها", 3)
            AddActionNode(stAnbarMiniPos, PermissionKeys.AnbarMiniModule, "🔘 استفاده از انبارداری مینی", dbPermissions)
            tAnbarMini.Children.Add(stAnbarMiniPos)
            mAnbarModules.Children.Add(tAnbarMini)

            Dim tAnbarMed As New PermissionTreeNode("T_ANBAR_MED", "📄 تب انبارداری متوسط", 2)
            Dim stAnbarMed As New PermissionTreeNode("ST_ANBAR_MED", "📑 زیرتب انبار متوسط و کالاها", 3)
            AddActionNode(stAnbarMed, PermissionKeys.AnbarMediumModule, "🔘 استفاده از انبارداری متوسط", dbPermissions)
            tAnbarMed.Children.Add(stAnbarMed)
            mAnbarModules.Children.Add(tAnbarMed)

            Dim tAnbarBig As New PermissionTreeNode("T_ANBAR_BIG", "📄 تب انبارداری پیشرفته", 2)
            Dim stAnbarBig As New PermissionTreeNode("ST_ANBAR_BIG", "📑 زیرتب مدیریت جامع انبارها", 3)
            AddActionNode(stAnbarBig, PermissionKeys.AnbarBigModule, "🔘 استفاده از انبارداری پیشرفته", dbPermissions)
            tAnbarBig.Children.Add(stAnbarBig)
            mAnbarModules.Children.Add(tAnbarBig)

            Dim mProductsWh As New PermissionTreeNode("M_PROD_WH", "📁 تعریف کالاها و انبارها", 1)
            Dim tProducts As New PermissionTreeNode("T_PRODS", "📄 تب مدیریت کالاها", 2)
            Dim stProducts As New PermissionTreeNode("ST_PRODS", "📑 زیرتب تعریف کالاها و خدمات", 3)
            AddActionNode(stProducts, PermissionKeys.TradeProducts, "🔘 تعریف کالاها و خدمات", dbPermissions)
            AddActionNode(stProducts, PermissionKeys.ManageProducts, "🔘 مدیریت کالاها", dbPermissions)
            tProducts.Children.Add(stProducts)
            mProductsWh.Children.Add(tProducts)

            Dim tWarehouses As New PermissionTreeNode("T_WHS", "📄 تب مدیریت انبارها", 2)
            Dim stWarehouses As New PermissionTreeNode("ST_WHS", "📑 زیرتب تعریف انبارها و موجودی", 3)
            AddActionNode(stWarehouses, PermissionKeys.TradeWarehouses, "🔘 تعریف انبارها", dbPermissions)
            AddActionNode(stWarehouses, PermissionKeys.ManageWarehouses, "🔘 مدیریت انبارها", dbPermissions)
            AddActionNode(stWarehouses, PermissionKeys.ViewInventory, "🔘 مشاهده موجودی انبار", dbPermissions)
            tWarehouses.Children.Add(stWarehouses)
            mProductsWh.Children.Add(tWarehouses)

            Dim mInvoices As New PermissionTreeNode("M_INVOICES", "📁 فاکتورها و صدور اسناد انبار", 1)
            Dim tPurchase As New PermissionTreeNode("T_PURCHASE", "📄 تب فاکتور خرید", 2)
            Dim stPurchase As New PermissionTreeNode("ST_PURCHASE", "📑 زیرتب صدور و مدیریت خرید", 3)
            AddActionNode(stPurchase, PermissionKeys.TradePurchase, "🔘 صدور فاکتور خرید", dbPermissions)
            AddActionNode(stPurchase, PermissionKeys.ManagePurchases, "🔘 مدیریت خرید", dbPermissions)
            tPurchase.Children.Add(stPurchase)
            mInvoices.Children.Add(tPurchase)

            Dim tSales As New PermissionTreeNode("T_SALES", "📄 تب فاکتور فروش", 2)
            Dim stSales As New PermissionTreeNode("ST_SALES", "📑 زیرتب صدور و مدیریت فروش", 3)
            AddActionNode(stSales, PermissionKeys.TradeSales, "🔘 صدور فاکتور فروش", dbPermissions)
            AddActionNode(stSales, PermissionKeys.ManageSales, "🔘 مدیریت فروش", dbPermissions)
            tSales.Children.Add(stSales)
            mInvoices.Children.Add(tSales)

            Dim tRemittance As New PermissionTreeNode("T_REMITTANCE", "📄 تب حواله و رسید انبار", 2)
            Dim stRemittance As New PermissionTreeNode("ST_REMITTANCE", "📑 زیرتب رسید و انتقال کالا", 3)
            AddActionNode(stRemittance, PermissionKeys.TradeRemittance, "🔘 حواله و رسید انبار", dbPermissions)
            AddActionNode(stRemittance, PermissionKeys.ManageTradeWarehouse, "🔘 خرید و فروش و انبارداری (جامع)", dbPermissions)
            tRemittance.Children.Add(stRemittance)
            mInvoices.Children.Add(tRemittance)

            Dim mTradeReports As New PermissionTreeNode("M_TRADE_REP", "📁 گزارشات انبارداری", 1)
            Dim tTradeReports As New PermissionTreeNode("T_TRADE_REP", "📄 تب گزارشات انبار و کاردکس", 2)
            Dim stTradeReports As New PermissionTreeNode("ST_TRADE_REP", "📑 زیرتب کاردکس و گزارشات موجودی", 3)
            AddActionNode(stTradeReports, PermissionKeys.TradeReports, "🔘 گزارشات انبار و کاردکس کالا", dbPermissions)
            AddActionNode(stTradeReports, PermissionKeys.ViewReports, "🔘 مشاهده گزارش‌ها", dbPermissions)
            tTradeReports.Children.Add(stTradeReports)
            mTradeReports.Children.Add(tTradeReports)

            rTrade.Children.Add(mAnbarModules)
            rTrade.Children.Add(mProductsWh)
            rTrade.Children.Add(mInvoices)
            rTrade.Children.Add(mTradeReports)
            roots.Add(rTrade)

            ' Root 3: حسابداری مالی و دفاتر
            Dim rAcc As New PermissionTreeNode("ROOT_ACC", "📊 حسابداری مالی و دفاتر", 0)
            
            Dim mCoding As New PermissionTreeNode("M_CODING", "📁 کدینگ و تعاریف پایه حسابداری", 1)
            Dim tCodingHeader As New PermissionTreeNode("T_CODING_HEADER", "📄 تب سرفصل حساب‌ها", 2)
            Dim stCodingHeader As New PermissionTreeNode("ST_CODING_HEADER", "📑 زیرتب درختواره کدینگ سرفصل‌ها", 3)
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeader, "🔘 سرفصل حساب‌ها (کدینگ)", dbPermissions)
            AddActionNode(stCodingHeader, PermissionKeys.ManageAccounting, "🔘 حسابداری (جامع)", dbPermissions)
            tCodingHeader.Children.Add(stCodingHeader)
            mCoding.Children.Add(tCodingHeader)

            Dim tShenavar As New PermissionTreeNode("T_SHENAVAR", "📄 تب حساب‌های شناور", 2)
            Dim stShenavar As New PermissionTreeNode("ST_SHENAVAR", "📑 زیرتب درختواره شناور", 3)
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavar, "🔘 حساب‌های شناور", dbPermissions)
            tShenavar.Children.Add(stShenavar)
            mCoding.Children.Add(tShenavar)

            Dim mSanad As New PermissionTreeNode("M_SANAD", "📁 ثبت اسناد و عملیات مالی", 1)
            Dim tSanadEntry As New PermissionTreeNode("T_SANAD_ENTRY", "📄 تب اسناد حسابداری", 2)
            Dim stSanadEntry As New PermissionTreeNode("ST_SANAD_ENTRY", "📑 زیرتب صدور و قفل اسناد", 3)
            AddActionNode(stSanadEntry, PermissionKeys.AccountingEntry, "🔘 ثبت سند حسابداری", dbPermissions)
            AddActionNode(stSanadEntry, PermissionKeys.LockSanad1, "🔘 قطعی‌سازی و قفل اسناد", dbPermissions)
            AddActionNode(stSanadEntry, PermissionKeys.HideSFSHInSanad, "🔘 مخفی کردن ستون‌های SF/SH", dbPermissions)
            AddActionNode(stSanadEntry, PermissionKeys.AccountingSanad1PrintDocs, "🔘 چاپ اسناد", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadEntry, PermissionKeys.AccountingSanad1PrintJournal, "🔘 چاپ دفتر روزنامه", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadEntry, PermissionKeys.AccountingSanad2PrintVoucher, "🔘 چاپ سند (Ctrl+P)", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            tSanadEntry.Children.Add(stSanadEntry)
            mSanad.Children.Add(tSanadEntry)

            Dim tBank As New PermissionTreeNode("T_BANK", "📄 تب عملیات بانکی", 2)
            Dim stBank As New PermissionTreeNode("ST_BANK", "📑 زیرتب مغایرت‌گیری بانکی", 3)
            AddActionNode(stBank, PermissionKeys.AccountingBank, "🔘 مغایرت‌های بانکی", dbPermissions)
            AddActionNode(stBank, PermissionKeys.AccountingBankRecExportExcel, "🔘 خروجی اکسل مغایرت", dbPermissions, new String() { PermissionKeys.AccountingBank })
            tBank.Children.Add(stBank)
            mSanad.Children.Add(tBank)

            Dim mLedgerTaraz As New PermissionTreeNode("M_LEDGER_TARAZ", "📁 دفاتر و ترازهای مالی", 1)
            Dim tDaftar As New PermissionTreeNode("T_DAFTAR", "📄 تب دفاتر حساب", 2)
            Dim stDaftar As New PermissionTreeNode("ST_DAFTAR", "📑 زیرتب دفتر کل و معین", 3)
            AddActionNode(stDaftar, PermissionKeys.AccountingLedger, "🔘 دفتر حساب", dbPermissions)
            AddActionNode(stDaftar, PermissionKeys.AccountingLedgerPrint, "🔘 چاپ دفتر حساب", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            AddActionNode(stDaftar, PermissionKeys.AccountingLedgerExport, "🔘 خروجی اکسل دفتر", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            AddActionNode(stDaftar, PermissionKeys.AccountingDaftarShenavarPrint, "🔘 چاپ دفتر شناور", dbPermissions)
            AddActionNode(stDaftar, PermissionKeys.AccountingDaftarShenavarExport, "🔘 خروجی اکسل دفتر شناور", dbPermissions)
            tDaftar.Children.Add(stDaftar)
            mLedgerTaraz.Children.Add(tDaftar)

            Dim tTaraz As New PermissionTreeNode("T_TARAZ", "📄 تب ترازهای مالی", 2)
            Dim stTaraz As New PermissionTreeNode("ST_TARAZ", "📑 زیرتب تراز آزمایشی و شناور", 3)
            AddActionNode(stTaraz, PermissionKeys.AccountingBalance, "🔘 تراز آزمایشی", dbPermissions)
            AddActionNode(stTaraz, PermissionKeys.AccountingTrialPrint, "🔘 چاپ تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            AddActionNode(stTaraz, PermissionKeys.AccountingTrialExport, "🔘 خروجی اکسل تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            AddActionNode(stTaraz, PermissionKeys.AccountingTarazShenavarPrint, "🔘 چاپ تراز شناور", dbPermissions)
            AddActionNode(stTaraz, PermissionKeys.AccountingTarazShenavarExport, "🔘 خروجی اکسل تراز شناور", dbPermissions)
            tTaraz.Children.Add(stTaraz)
            mLedgerTaraz.Children.Add(tTaraz)

            Dim mAccReports As New PermissionTreeNode("M_ACC_REP", "📁 گزارشات و صورت‌های مالی", 1)
            Dim tProfitLoss As New PermissionTreeNode("T_PROFIT_LOSS", "📄 تب صورت‌های مالی", 2)
            Dim stProfitLoss As New PermissionTreeNode("ST_PROFIT_LOSS", "📑 زیرتب صورت سود و زیان و ترازنامه", 3)
            AddActionNode(stProfitLoss, PermissionKeys.AccountingProfitLoss, "🔘 صورت سود و زیان", dbPermissions)
            AddActionNode(stProfitLoss, PermissionKeys.AccountingProfitLossPrint, "🔘 چاپ سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLoss, PermissionKeys.AccountingProfitLossExport, "🔘 خروجی اکسل سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLoss, PermissionKeys.AccountingBalanceSheet, "🔘 ترازنامه مالی", dbPermissions)
            AddActionNode(stProfitLoss, PermissionKeys.AccountingBalanceSheetPrint, "🔘 چاپ ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stProfitLoss, PermissionKeys.AccountingBalanceSheetExport, "🔘 خروجی اکسل ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            tProfitLoss.Children.Add(stProfitLoss)
            mAccReports.Children.Add(tProfitLoss)

            Dim tAdvReports As New PermissionTreeNode("T_ADV_REP", "📄 تب گزارشات پیشرفته", 2)
            Dim stAdvReports As New PermissionTreeNode("ST_ADV_REP", "📑 زیرتب گزارشات دلخواه و نموداری", 3)
            AddActionNode(stAdvReports, PermissionKeys.AccountingReports, "🔘 گزارشات حسابداری", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingAdvancedReports, "🔘 گزارشات پیشرفته", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingChartReports, "🔘 گزارشات نموداری", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReports, "🔘 گزارشات دلخواه", dbPermissions)
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReportPrint, "🔘 چاپ گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            AddActionNode(stAdvReports, PermissionKeys.AccountingCustomReportExport, "🔘 خروجی اکسل گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            tAdvReports.Children.Add(stAdvReports)
            mAccReports.Children.Add(tAdvReports)

            rAcc.Children.Add(mCoding)
            rAcc.Children.Add(mSanad)
            rAcc.Children.Add(mLedgerTaraz)
            rAcc.Children.Add(mAccReports)
            roots.Add(rAcc)

            ' Dynamic Scanner: Automatically find any new/unmapped permissions in DB!
            Dim mappedKeys = GetAllMappedPermissionKeys(roots)
            Dim unmappedList As New List(Of KeyValuePair(Of String, Integer))()

            For Each kvp In dbPermissions
                If Not mappedKeys.Contains(kvp.Key) Then
                    unmappedList.Add(kvp)
                End If
            Next

            If unmappedList.Count > 0 Then
                Dim rNew As New PermissionTreeNode("ROOT_NEW", "⚡ مجوزها و امکانات جدید سیستم", 0)
                Dim mNew As New PermissionTreeNode("M_NEW", "📁 امکانات افزوده‌شده در به‌روزرسانی‌های جدید", 1)
                Dim tNew As New PermissionTreeNode("T_NEW", "📄 تب مجوزهای جدید", 2)
                Dim stNew As New PermissionTreeNode("ST_NEW", "📑 زیرتب دسترسی‌های شناسایی‌شده", 3)

                For Each kvp In unmappedList
                    AddActionNode(stNew, kvp.Key, "🔘 " & kvp.Key, dbPermissions)
                Next

                tNew.Children.Add(stNew)
                mNew.Children.Add(tNew)
                rNew.Children.Add(mNew)
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
