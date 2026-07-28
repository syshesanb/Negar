Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports Negar.Data

Namespace Negar.Business
    ''' <summary>
    ''' Represents a Node in the 5-Level Permission Tree Hierarchy:
    ''' Level 0: Main Menu (منوی اصلی سیستم)
    ''' Level 1: SubMenu (زیر منوهای مستقیم سیستم)
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
            ' 1. منوی اصلی: مدیریت سیستم (mSystemMgmt)
            ' =========================================================================
            Dim rSys As New PermissionTreeNode("MENU_SYS", "⚙️ مدیریت سیستم", 0)

            ' 1.1 زیر منو: مدیریت پیامهای : درباره... و ارتباط با ما
            Dim smSysMessages As New PermissionTreeNode("SM_SYS_MSG", "📁 مدیریت پیامهای : درباره... و ارتباط با ما", 1)
            Dim tSysMessages As New PermissionTreeNode("T_SYS_MSG", "📄 فرم ویرایش پیام‌ها و ارتباط با ما", 2)
            Dim stSysMessages As New PermissionTreeNode("ST_SYS_MSG", "📑 تنظیم متون و راه‌های ارتباطی", 3)
            AddActionNode(stSysMessages, PermissionKeys.ManageAppMessages, "🔘 مدیریت پیام‌های سیستم", dbPermissions)
            tSysMessages.Children.Add(stSysMessages)
            smSysMessages.Children.Add(tSysMessages)
            rSys.Children.Add(smSysMessages)

            ' 1.2 زیر منو: مدیریت تمهای برنامه و فرمها
            Dim smSysThemes As New PermissionTreeNode("SM_SYS_THEMES", "📁 مدیریت تمهای برنامه و فرمها", 1)
            Dim tSysThemes As New PermissionTreeNode("T_SYS_THEMES", "📄 فرم انتخاب رنگ و پوسته visual", 2)
            Dim stSysThemes As New PermissionTreeNode("ST_SYS_THEMES", "📑 پالت رنگی و تم‌های تیره/روشن", 3)
            AddActionNode(stSysThemes, PermissionKeys.ManageAppThemes, "🔘 تغییر و اعمال تم‌های برنامه", dbPermissions)
            tSysThemes.Children.Add(stSysThemes)
            smSysThemes.Children.Add(tSysThemes)
            rSys.Children.Add(smSysThemes)

            ' 1.3 زیر منو: پشتیبان‌گیری اطلاعات
            Dim smBackup As New PermissionTreeNode("SM_BACKUP", "📁 پشتیبان‌گیری اطلاعات", 1)
            Dim tBackup As New PermissionTreeNode("T_BACKUP", "📄 فرم تهیه نسخه پشتیبان دیتابیس", 2)
            Dim stBackup As New PermissionTreeNode("ST_BACKUP", "📑 مسیر ذخیره‌سازی و فشردگی فایل", 3)
            AddActionNode(stBackup, PermissionKeys.BackupData, "🔘 پشتیبان‌گیری اطلاعات", dbPermissions)
            tBackup.Children.Add(stBackup)
            smBackup.Children.Add(tBackup)
            rSys.Children.Add(smBackup)

            ' 1.4 زیر منو: بازیابی اطلاعات
            Dim smRestore As New PermissionTreeNode("SM_RESTORE", "📁 بازیابی اطلاعات", 1)
            Dim tRestore As New PermissionTreeNode("T_RESTORE", "📄 فرم بازگردانی فایل پشتیبان دیتابیس", 2)
            Dim stRestore As New PermissionTreeNode("ST_RESTORE", "📑 تایید بازنویسی داده‌ها", 3)
            AddActionNode(stRestore, PermissionKeys.RestoreData, "🔘 بازیابی اطلاعات", dbPermissions)
            tRestore.Children.Add(stRestore)
            smRestore.Children.Add(tRestore)
            rSys.Children.Add(smRestore)

            ' 1.5 زیر منو: تبدیل دیتا از سایر نرم افزارها
            Dim smDataMigration As New PermissionTreeNode("SM_DATA_MIGRATION", "📁 تبدیل دیتا از سایر نرم افزارها", 1)
            Dim tDataMigration As New PermissionTreeNode("T_DATA_MIGRATION", "📄 فرم انتقال و ایمپورت داده‌ها", 2)
            Dim stDataMigration As New PermissionTreeNode("ST_DATA_MIGRATION", "📑 نگاشت جدول سرفصل‌ها و اسناد", 3)
            AddActionNode(stDataMigration, PermissionKeys.DataMigration, "🔘 انتقال و تبدیل اطلاعات دیتابیس", dbPermissions)
            tDataMigration.Children.Add(stDataMigration)
            smDataMigration.Children.Add(tDataMigration)
            rSys.Children.Add(smDataMigration)

            roots.Add(rSys)

            ' =========================================================================
            ' 2. منوی اصلی: مدیریت کاربران (mUserMgmt)
            ' =========================================================================
            Dim rUsers As New PermissionTreeNode("MENU_USERS", "👥 مدیریت کاربران", 0)

            ' 2.1 زیر منو: مدیریت کاربران (جامع)
            Dim smUsersComprehensive As New PermissionTreeNode("SM_USERS_COMP", "📁 مدیریت کاربران (جامع)", 1)
            Dim tUsersList As New PermissionTreeNode("T_USERS_LIST", "📄 تب مدیریت کاربران سیستم", 2)
            Dim stUsersList As New PermissionTreeNode("ST_USERS_LIST", "📑 زیرتب ایجاد، ویرایش و سقف شرکت‌ها", 3)
            AddActionNode(stUsersList, PermissionKeys.ManageUsers, "🔘 مدیریت کاربران (جامع)", dbPermissions)
            tUsersList.Children.Add(stUsersList)
            smUsersComprehensive.Children.Add(tUsersList)

            Dim tUsersPerms As New PermissionTreeNode("T_USERS_PERMS", "📄 تب سطح دسترسی‌ها و الگوها", 2)
            Dim stUsersPerms As New PermissionTreeNode("ST_USERS_PERMS", "📑 زیرتب تنظیم درختی دسترسی‌ها و نقش‌ها", 3)
            AddActionNode(stUsersPerms, PermissionKeys.ViewActivityLog, "🔘 مشاهده دفتر سوابق و لاگ فعالیت‌ها", dbPermissions)
            tUsersPerms.Children.Add(stUsersPerms)
            smUsersComprehensive.Children.Add(tUsersPerms)
            rUsers.Children.Add(smUsersComprehensive)

            ' 2.2 زیر منو: مدیریت کاربران – مدیریت کاربران عادی
            Dim smUsersBasic As New PermissionTreeNode("SM_USERS_BASIC", "📁 مدیریت کاربران – مدیریت کاربران عادی", 1)
            Dim tBasicUsers As New PermissionTreeNode("T_BASIC_USERS", "📄 تب کاربران عادی و اپراتورها", 2)
            Dim stBasicUsers As New PermissionTreeNode("ST_BASIC_USERS", "📑 زیرتب لیست کاربران عادی زیرمجموعه", 3)
            AddActionNode(stBasicUsers, PermissionKeys.ManageBasicUsers, "🔘 مدیریت کاربران عادی", dbPermissions)
            tBasicUsers.Children.Add(stBasicUsers)
            smUsersBasic.Children.Add(tBasicUsers)
            rUsers.Children.Add(smUsersBasic)

            ' 2.3 زیر منو: تغییر نام کاربری و رمز عبور
            Dim smChangeProfile As New PermissionTreeNode("SM_CHANGE_PROFILE", "📁 تغییر نام کاربری و رمز عبور", 1)
            Dim tChangePass As New PermissionTreeNode("T_CHANGE_PASS", "📄 فرم پروفایل کاربری", 2)
            Dim stChangePass As New PermissionTreeNode("ST_CHANGE_PASS", "📑 تغییر کلمه عبور کاربر لاگین‌شده", 3)
            AddActionNode(stChangePass, PermissionKeys.ChangePassword, "🔘 تغییر کلمه عبور", dbPermissions)
            tChangePass.Children.Add(stChangePass)
            smChangeProfile.Children.Add(tChangePass)
            rUsers.Children.Add(smChangeProfile)

            ' 2.4 زیر منو: ورود با نام کاربری دیگر
            Dim smSwitchUser As New PermissionTreeNode("SM_SWITCH_USER", "📁 ورود با نام کاربری دیگر", 1)
            Dim tSwitchUser As New PermissionTreeNode("T_SWITCH_USER", "📄 دیالوگ تعویض سریع کاربر", 2)
            Dim stSwitchUser As New PermissionTreeNode("ST_SWITCH_USER", "📑 احراز هویت مجدد بدون خروج", 3)
            AddActionNode(stSwitchUser, PermissionKeys.SwitchUser, "🔘 ورود با کاربر دیگر", dbPermissions)
            tSwitchUser.Children.Add(stSwitchUser)
            smSwitchUser.Children.Add(tSwitchUser)
            rUsers.Children.Add(smSwitchUser)

            roots.Add(rUsers)

            ' =========================================================================
            ' 3. منوی اصلی: مدیریت شرکت و سال مالی (mCompanyMgmt)
            ' =========================================================================
            Dim rCompanies As New PermissionTreeNode("MENU_COMPANIES", "🏢 مدیریت شرکت و سال مالی", 0)

            ' 3.1 زیر منو: مدیریت شرکت‌ها و سال‌های مالی
            Dim smCompaniesYears As New PermissionTreeNode("SM_COMP_YEARS", "📁 مدیریت شرکت‌ها و سال‌های مالی", 1)
            Dim tCompaniesList As New PermissionTreeNode("T_COMP_LIST", "📄 تب مدیریت شرکت‌ها", 2)
            Dim stCompaniesGrid As New PermissionTreeNode("ST_COMP_GRID", "📑 زیرتب لیست شرکت‌ها، کد اقتصادی و لوگو", 3)
            AddActionNode(stCompaniesGrid, PermissionKeys.ManageCompanies, "🔘 مدیریت شرکت‌ها", dbPermissions)
            tCompaniesList.Children.Add(stCompaniesGrid)
            smCompaniesYears.Children.Add(tCompaniesList)

            Dim tFiscalYearsList As New PermissionTreeNode("T_FY_LIST", "📄 تب سال‌های مالی", 2)
            Dim stFiscalYearsGrid As New PermissionTreeNode("ST_FY_GRID", "📑 زیرتب تعریف و بستن دوره‌های مالی", 3)
            AddActionNode(stFiscalYearsGrid, PermissionKeys.ManageFiscalYears, "🔘 مدیریت سال‌های مالی", dbPermissions)
            tFiscalYearsList.Children.Add(stFiscalYearsGrid)
            smCompaniesYears.Children.Add(tFiscalYearsList)

            Dim tSelectActiveCompany As New PermissionTreeNode("T_SELECT_ACTIVE", "📄 تب انتخاب شرکت و سال مالی فعال", 2)
            Dim stSelectActive As New PermissionTreeNode("ST_SELECT_ACTIVE", "📑 زیرتب فعال‌سازی محیط کاری شرکت", 3)
            AddActionNode(stSelectActive, PermissionKeys.SelectCompanyFiscalYear, "🔘 انتخاب شرکت و سال مالی جاری", dbPermissions)
            tSelectActiveCompany.Children.Add(stSelectActive)
            smCompaniesYears.Children.Add(tSelectActiveCompany)

            rCompanies.Children.Add(smCompaniesYears)
            roots.Add(rCompanies)

            ' =========================================================================
            ' 4. منوی اصلی: حسابداری (mAccounting)
            ' =========================================================================
            Dim rAccounting As New PermissionTreeNode("MENU_ACCOUNTING", "📊 حسابداری", 0)

            ' 4.1 زیر منو: کدینگ، ثبت اسناد و دفاتر حسابداری (miAccountingMain)
            Dim smAccMain As New PermissionTreeNode("SM_ACC_MAIN", "📁 کدینگ، ثبت اسناد و دفاتر حسابداری", 1)

            ' تب 1: تنظیمات اولیه حسابها
            Dim tAccSettings As New PermissionTreeNode("T_ACC_SETTINGS", "📄 تب تنظیمات اولیه حسابها", 2)
            Dim stAccSettings As New PermissionTreeNode("ST_ACC_SETTINGS", "📑 زیرتب مشخصات مالیاتی و تنظیمات طول کدینگ", 3)
            AddActionNode(stAccSettings, PermissionKeys.AccountingSettings, "🔘 تنظیمات اولیه حساب‌ها", dbPermissions)
            tAccSettings.Children.Add(stAccSettings)
            smAccMain.Children.Add(tAccSettings)

            ' تب 2: سرفصل حسابها
            Dim tCodingHeader As New PermissionTreeNode("T_CODING_HEADER", "📄 تب سرفصل حساب‌ها (کدینگ)", 2)
            Dim stCodingHeader As New PermissionTreeNode("ST_CODING_HEADER", "📑 زیرتب درختواره کدینگ گروه، کل و معین", 3)
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeader, "🔘 سرفصل حساب‌ها (کدینگ)", dbPermissions)
            tCodingHeader.Children.Add(stCodingHeader)
            smAccMain.Children.Add(tCodingHeader)

            ' تب 3: حسابهای شناور
            Dim tShenavar As New PermissionTreeNode("T_SHENAVAR", "📄 تب حساب‌های شناور", 2)
            Dim stShenavar As New PermissionTreeNode("ST_SHENAVAR", "📑 زیرتب درختواره شناور اشخاص و مراکز", 3)
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavar, "🔘 حساب‌های شناور", dbPermissions)
            tShenavar.Children.Add(stShenavar)
            smAccMain.Children.Add(tShenavar)

            ' تب 4: ثبت سند حسابداری
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

            ' تب 5: مغایرت بانکی
            Dim tMogBank As New PermissionTreeNode("T_MOG_BANK", "📄 تب مغایرت‌گیری بانکی", 2)
            
            Dim stMogBankIntro As New PermissionTreeNode("ST_MOG_BANK_INTRO", "📑 زیرتب معرفی بانک‌ها", 3)
            AddActionNode(stMogBankIntro, PermissionKeys.AccountingBankRecIntro, "🔘 معرفی بانک‌ها", dbPermissions)
            tMogBank.Children.Add(stMogBankIntro)

            Dim stMogBankImport As New PermissionTreeNode("ST_MOG_BANK_IMPORT", "📑 زیرتب ورود صورت حساب بانک", 3)
            AddActionNode(stMogBankImport, PermissionKeys.AccountingBankRecImport, "🔘 ورود صورت حساب بانک", dbPermissions)
            tMogBank.Children.Add(stMogBankImport)

            Dim stMogBankMatch As New PermissionTreeNode("ST_MOG_BANK_MATCH", "📑 زیرتب مغایرت‌گیری", 3)
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecMatch, "🔘 مغایرت‌گیری", dbPermissions)
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecExportExcel, "🔘 خروجی اکسل مغایرت", dbPermissions, new String() { PermissionKeys.AccountingBankRecMatch })
            tMogBank.Children.Add(stMogBankMatch)

            Dim stMogBankSugg As New PermissionTreeNode("ST_MOG_BANK_SUGG", "📑 زیرتب پیشنهاد برای رفع مغایرت", 3)
            AddActionNode(stMogBankSugg, PermissionKeys.AccountingBankRecSuggestions, "🔘 پیشنهاد برای رفع مغایرت", dbPermissions)
            tMogBank.Children.Add(stMogBankSugg)

            smAccMain.Children.Add(tMogBank)

            ' تب 6: تراز آزمایشی
            Dim tTaraz As New PermissionTreeNode("T_TARAZ", "📄 تب تراز آزمایشی", 2)
            Dim stTarazGrid As New PermissionTreeNode("ST_TARAZ_GRID", "📑 زیرتب محاسبه تراز آزمایشی ۲، ۴ و ۸ ستونی", 3)
            AddActionNode(stTarazGrid, PermissionKeys.AccountingBalance, "🔘 تراز آزمایشی", dbPermissions)
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTrialPrint, "🔘 چاپ تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            AddActionNode(stTarazGrid, PermissionKeys.AccountingTrialExport, "🔘 خروجی اکسل تراز آزمایشی", dbPermissions, new String() { PermissionKeys.AccountingBalance })
            tTaraz.Children.Add(stTarazGrid)
            smAccMain.Children.Add(tTaraz)

            ' تب 7: دفتر حساب
            Dim tDaftar As New PermissionTreeNode("T_DAFTAR", "📄 تب دفتر حساب (روزنامه/کل/معین)", 2)
            Dim stDaftarGrid As New PermissionTreeNode("ST_DAFTAR_GRID", "📑 زیرتب مرور دفاتر و گردش حساب‌ها", 3)
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedger, "🔘 دفتر حساب", dbPermissions)
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedgerPrint, "🔘 چاپ دفتر حساب", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            AddActionNode(stDaftarGrid, PermissionKeys.AccountingLedgerExport, "🔘 خروجی اکسل دفتر", dbPermissions, new String() { PermissionKeys.AccountingLedger })
            tDaftar.Children.Add(stDaftarGrid)
            smAccMain.Children.Add(tDaftar)

            ' تب 8: تراز شناور
            Dim tTarazShenavar As New PermissionTreeNode("T_TARAZ_SHENAVAR", "📄 تب تراز شناور", 2)
            Dim stTarazShenavarGrid As New PermissionTreeNode("ST_TARAZ_SHENAVAR_GRID", "📑 زیرتب تراز آزمایشی حساب‌های شناور", 3)
            AddActionNode(stTarazShenavarGrid, PermissionKeys.AccountingTarazShenavar, "🔘 تراز شناور", dbPermissions)
            AddActionNode(stTarazShenavarGrid, PermissionKeys.AccountingTarazShenavarPrint, "🔘 چاپ تراز شناور", dbPermissions, new String() { PermissionKeys.AccountingTarazShenavar })
            AddActionNode(stTarazShenavarGrid, PermissionKeys.AccountingTarazShenavarExport, "🔘 خروجی اکسل تراز شناور", dbPermissions, new String() { PermissionKeys.AccountingTarazShenavar })
            tTarazShenavar.Children.Add(stTarazShenavarGrid)
            smAccMain.Children.Add(tTarazShenavar)

            ' تب 9: دفتر شناور
            Dim tDaftarShenavar As New PermissionTreeNode("T_DAFTAR_SHENAVAR", "📄 تب دفتر شناور", 2)
            Dim stDaftarShenavarGrid As New PermissionTreeNode("ST_DAFTAR_SHENAVAR_GRID", "📑 زیرتب دفتر و گردش حساب‌های شناور", 3)
            AddActionNode(stDaftarShenavarGrid, PermissionKeys.AccountingDaftarShenavar, "🔘 دفتر شناور", dbPermissions)
            AddActionNode(stDaftarShenavarGrid, PermissionKeys.AccountingDaftarShenavarPrint, "🔘 چاپ دفتر شناور", dbPermissions, new String() { PermissionKeys.AccountingDaftarShenavar })
            AddActionNode(stDaftarShenavarGrid, PermissionKeys.AccountingDaftarShenavarExport, "🔘 خروجی اکسل دفتر شناور", dbPermissions, new String() { PermissionKeys.AccountingDaftarShenavar })
            tDaftarShenavar.Children.Add(stDaftarShenavarGrid)
            smAccMain.Children.Add(tDaftarShenavar)

            ' تب 10: عملکرد و سود و زیان
            Dim tProfitLoss As New PermissionTreeNode("T_PROFIT_LOSS", "📄 تب عملکرد و سود و زیان", 2)
            Dim stProfitLossGrid As New PermissionTreeNode("ST_PROFIT_LOSS_GRID", "📑 زیرتب صورت حساب سود و زیان و عملکرد", 3)
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLoss, "🔘 صورت سود و زیان", dbPermissions)
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossPrint, "🔘 چاپ سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossExport, "🔘 خروجی اکسل سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            tProfitLoss.Children.Add(stProfitLossGrid)
            smAccMain.Children.Add(tProfitLoss)

            ' تب 11: ترازنامه
            Dim tBalanceSheet As New PermissionTreeNode("T_BALANCE_SHEET", "📄 تب ترازنامه", 2)
            Dim stBalanceSheetGrid As New PermissionTreeNode("ST_BALANCE_SHEET_GRID", "📑 زیرتب ترازنامه و دارایی‌ها/بدهی‌ها", 3)
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheet, "🔘 ترازنامه مالی", dbPermissions)
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetPrint, "🔘 چاپ ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetExport, "🔘 خروجی اکسل ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            tBalanceSheet.Children.Add(stBalanceSheetGrid)
            smAccMain.Children.Add(tBalanceSheet)

            ' تب 12: سایر گزارشات
            Dim tOtherReports As New PermissionTreeNode("T_OTHER_REPORTS", "📄 تب سایر گزارشات", 2)

            Dim stOtherAdvReports As New PermissionTreeNode("ST_OTHER_ADV_REPORTS", "📑 زیرتب گزارشات پیشرفته", 3)
            AddActionNode(stOtherAdvReports, PermissionKeys.AccountingAdvancedReports, "🔘 گزارشات پیشرفته", dbPermissions)
            tOtherReports.Children.Add(stOtherAdvReports)

            Dim stOtherChartReports As New PermissionTreeNode("ST_OTHER_CHART_REPORTS", "📑 زیرتب گزارشات نموداری", 3)
            AddActionNode(stOtherChartReports, PermissionKeys.AccountingChartReports, "🔘 گزارشات نموداری", dbPermissions)
            tOtherReports.Children.Add(stOtherChartReports)

            Dim stOtherCustomReports As New PermissionTreeNode("ST_OTHER_CUSTOM_REPORTS", "📑 زیرتب طراحی گزارشات دلخواه", 3)
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReports, "🔘 طراحی گزارشات دلخواه", dbPermissions)
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReportPrint, "🔘 چاپ گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReportExport, "🔘 خروجی اکسل گزارش دلخواه", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            tOtherReports.Children.Add(stOtherCustomReports)

            smAccMain.Children.Add(tOtherReports)

            rAccounting.Children.Add(smAccMain)

            ' 4.2 زیر منو: گزارشات و ترازهای حسابداری (miReportsAccounting)
            Dim smAccReports As New PermissionTreeNode("SM_ACC_REPORTS", "📁 گزارشات و ترازهای حسابداری", 1)

            Dim tAccRepMain As New PermissionTreeNode("T_ACC_REP_MAIN", "📄 تب گزارشات و ترازهای حسابداری", 2)
            Dim stAccRepMain As New PermissionTreeNode("ST_ACC_REP_MAIN", "📑 زیرتب گزارشات حسابداری", 3)
            AddActionNode(stAccRepMain, PermissionKeys.AccountingReportsMenu, "🔘 گزارشات و ترازهای حسابداری (منو)", dbPermissions)
            tAccRepMain.Children.Add(stAccRepMain)
            smAccReports.Children.Add(tAccRepMain)

            rAccounting.Children.Add(smAccReports)
            roots.Add(rAccounting)

            ' =========================================================================
            ' 5. منوی اصلی: خریدو فروش و انبارداری (mTradeWarehouse)
            ' =========================================================================
            Dim rTrade As New PermissionTreeNode("MENU_TRADE", "🛒 خریدو فروش و انبارداری", 0)

            ' 5.1 زیر منو: استفاده از انبارداری مینی (miTradeMini)
            Dim smTradeMini As New PermissionTreeNode("SM_TRADE_MINI", "📁 استفاده از انبارداری مینی", 1)
            
            Dim tMiniPos As New PermissionTreeNode("T_MINI_POS", "📄 تب فروش سریع (POS)", 2)
            Dim stMiniPosGrid As New PermissionTreeNode("ST_MINI_POS_GRID", "📑 زیرتب صدور فاکتور و تسویه کارتخوان/نقد", 3)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniModule, "🔘 استفاده از انبارداری مینی", dbPermissions)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniPos, "🔘 صدور و ورود اطلاعات فاکتور فروش مینی", dbPermissions)
            tMiniPos.Children.Add(stMiniPosGrid)
            smTradeMini.Children.Add(tMiniPos)

            Dim tMiniKharid As New PermissionTreeNode("T_MINI_KHARID", "📄 تب خرید کالا", 2)
            Dim stMiniKharidGrid As New PermissionTreeNode("ST_MINI_KHARID_GRID", "📑 زیرتب ثبت فاکتورهای خرید کالا", 3)
            AddActionNode(stMiniKharidGrid, PermissionKeys.AnbarMiniKharid, "🔘 صدور فاکتور خرید مینی", dbPermissions)
            tMiniKharid.Children.Add(stMiniKharidGrid)
            smTradeMini.Children.Add(tMiniKharid)

            Dim tMiniPersons As New PermissionTreeNode("T_MINI_PERSONS", "📄 تب لیست فروشنده و خریدار", 2)
            Dim stMiniPersonsGrid As New PermissionTreeNode("ST_MINI_PERSONS_GRID", "📑 زیرتب طرف حساب‌ها و اشخاص", 3)
            AddActionNode(stMiniPersonsGrid, PermissionKeys.AnbarMiniPersons, "🔘 ثبت و ویرایش طرف حساب‌های مینی", dbPermissions)
            tMiniPersons.Children.Add(stMiniPersonsGrid)
            smTradeMini.Children.Add(tMiniPersons)

            Dim tMiniExpenses As New PermissionTreeNode("T_MINI_EXPENSES", "📄 تب هزینه‌ها", 2)
            Dim stMiniExpensesToolbar As New PermissionTreeNode("ST_MINI_EXPENSES_TB", "📑 زیرتب ثبت هزینه‌ها و نوار ابزار", 3)
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpenses, "🔘 ثبت و ویرایش اسناد هزینه", dbPermissions)
            tMiniExpenses.Children.Add(stMiniExpensesToolbar)

            Dim stMiniExpenseLedger As New PermissionTreeNode("ST_MINI_EXPENSE_LEDGER", "📑 دیالوگ دفتر هزینه (سرفصل/عنوان)", 3)
            AddActionNode(stMiniExpenseLedger, PermissionKeys.AnbarMiniExpenseLedger, "🔘 📒 تهیه و چاپ دفتر هزینه", dbPermissions)
            tMiniExpenses.Children.Add(stMiniExpenseLedger)

            Dim stMiniProfitLoss As New PermissionTreeNode("ST_MINI_PROFIT_LOSS", "📑 پیش‌نمایش چاپی عملکرد و سود و زیان", 3)
            AddActionNode(stMiniProfitLoss, PermissionKeys.AnbarMiniProfitLoss, "🔘 🖨️ چاپ عملکرد و سود و زیان", dbPermissions)
            tMiniExpenses.Children.Add(stMiniProfitLoss)
            smTradeMini.Children.Add(tMiniExpenses)

            Dim tMiniProducts As New PermissionTreeNode("T_MINI_PRODS", "📄 تب لیست کالاها", 2)
            Dim stMiniProdsGrid As New PermissionTreeNode("ST_MINI_PRODS_GRID", "📑 زیرتب مدیریت کالاها و قیمت‌ها", 3)
            AddActionNode(stMiniProdsGrid, PermissionKeys.AnbarMiniProducts, "🔘 مدیریت کالاها و خدمات مینی", dbPermissions)
            tMiniProducts.Children.Add(stMiniProdsGrid)
            smTradeMini.Children.Add(tMiniProducts)

            Dim tMiniWarehouses As New PermissionTreeNode("T_MINI_WHS", "📄 تب لیست انبارها", 2)
            Dim stMiniWhsGrid As New PermissionTreeNode("ST_MINI_WHS_GRID", "📑 زیرتب تعریف انبارها و موجودی", 3)
            AddActionNode(stMiniWhsGrid, PermissionKeys.AnbarMiniWarehouses, "🔘 مدیریت انبارها و موجودی مینی", dbPermissions)
            tMiniWarehouses.Children.Add(stMiniWhsGrid)
            smTradeMini.Children.Add(tMiniWarehouses)

            rTrade.Children.Add(smTradeMini)

            ' 5.2 زیر منو: استفاده از انبارداری متوسط (miTradeMedium)
            Dim smTradeMed As New PermissionTreeNode("SM_TRADE_MED", "📁 استفاده از انبارداری متوسط", 1)
            Dim tMedMain As New PermissionTreeNode("T_MED_MAIN", "📄 تب اصلی انبارداری متوسط", 2)
            Dim stMedGrid As New PermissionTreeNode("ST_MED_GRID", "📑 زیرتب عملیات متوسط انبار و کالا", 3)
            AddActionNode(stMedGrid, PermissionKeys.AnbarMediumModule, "🔘 استفاده از انبارداری متوسط", dbPermissions)
            tMedMain.Children.Add(stMedGrid)
            smTradeMed.Children.Add(tMedMain)
            rTrade.Children.Add(smTradeMed)

            ' 5.3 زیر منو: استفاده از انبارداری پیشرفته (miTradeBig)
            Dim smTradeBig As New PermissionTreeNode("SM_TRADE_BIG", "📁 استفاده از انبارداری پیشرفته", 1)
            Dim tBigMain As New PermissionTreeNode("T_BIG_MAIN", "📄 تب اصلی انبارداری پیشرفته", 2)
            Dim stBigGrid As New PermissionTreeNode("ST_BIG_GRID", "📑 زیرتب کنترل پیشرفته چند انباره", 3)
            AddActionNode(stBigGrid, PermissionKeys.AnbarBigModule, "🔘 استفاده از انبارداری پیشرفته", dbPermissions)
            tBigMain.Children.Add(stBigGrid)
            smTradeBig.Children.Add(tBigMain)
            rTrade.Children.Add(smTradeBig)

            ' 5.4 زیر منو: فاکتورها، انبار و مدیریت کالاها (جامع) (miTradeWarehouseMain)
            Dim smTradeComprehensive As New PermissionTreeNode("SM_TRADE_COMP", "📁 فاکتورها، انبار و مدیریت کالاها (جامع)", 1)

            Dim tCompUnits As New PermissionTreeNode("T_COMP_UNITS", "📄 تب واحدهای سنجش کالا", 2)
            Dim stCompUnits As New PermissionTreeNode("ST_COMP_UNITS", "📑 زیرتب تعاریف واحدهای شمارش و سنجش", 3)
            AddActionNode(stCompUnits, PermissionKeys.TradeProductUnits, "🔘 واحدهای سنجش کالا", dbPermissions)
            tCompUnits.Children.Add(stCompUnits)
            smTradeComprehensive.Children.Add(tCompUnits)

            Dim tCompGroups As New PermissionTreeNode("T_COMP_GROUPS", "📄 تب دسته‌بندی و گروه‌های کالا", 2)
            Dim stCompGroups As New PermissionTreeNode("ST_COMP_GROUPS", "📑 زیرتب درختواره گروه کالا و خدمات", 3)
            AddActionNode(stCompGroups, PermissionKeys.TradeProductGroups, "🔘 دسته‌بندی و گروه‌های کالا", dbPermissions)
            tCompGroups.Children.Add(stCompGroups)
            smTradeComprehensive.Children.Add(tCompGroups)
            
            Dim tCompProds As New PermissionTreeNode("T_COMP_PRODS", "📄 تب تعریف کالاها و خدمات", 2)
            Dim stCompProds As New PermissionTreeNode("ST_COMP_PRODS", "📑 زیرتب لیست کالاها و مشخصات", 3)
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

            Dim tCompRemit As New PermissionTreeNode("T_COMP_REMIT", "📄 تب حواله و رسید انبار", 2)
            Dim stCompRemit As New PermissionTreeNode("ST_COMP_REMIT", "📑 زیرتب رسید و انتقال کالا", 3)
            AddActionNode(stCompRemit, PermissionKeys.TradeRemittance, "🔘 حواله و رسید انبار", dbPermissions)
            AddActionNode(stCompRemit, PermissionKeys.ManageTradeWarehouse, "🔘 خرید و فروش و انبارداری (جامع)", dbPermissions)
            tCompRemit.Children.Add(stCompRemit)
            smTradeComprehensive.Children.Add(tCompRemit)

            rTrade.Children.Add(smTradeComprehensive)

            ' 5.5 زیر منو: گزارشات فاکتورها و موجودی انبار (miReportsTrade)
            Dim smTradeReports As New PermissionTreeNode("SM_TRADE_REPORTS", "📁 گزارشات فاکتورها و موجودی انبار", 1)
            Dim tTradeRepMain As New PermissionTreeNode("T_TRADE_REP_MAIN", "📄 تب گزارشات موجودی و کاردکس", 2)
            Dim stTradeRepGrid As New PermissionTreeNode("ST_TRADE_REP_GRID", "📑 زیرتب کاردکس کالا و مرور فاکتورها", 3)
            AddActionNode(stTradeRepGrid, PermissionKeys.TradeReports, "🔘 گزارشات انبار و کاردکس کالا", dbPermissions)
            tTradeRepMain.Children.Add(stTradeRepGrid)
            smTradeReports.Children.Add(tTradeRepMain)
            rTrade.Children.Add(smTradeReports)

            roots.Add(rTrade)

            ' =========================================================================
            ' 6. منوی اصلی: پوسته مشاغل (mBusinessShells)
            ' =========================================================================
            Dim rBusinessShells As New PermissionTreeNode("MENU_SHELLS", "💼 پوسته مشاغل", 0)
            Dim smShells As New PermissionTreeNode("SM_SHELLS", "📁 انتخاب و پیکربندی پوسته مشاغل", 1)
            Dim tShells As New PermissionTreeNode("T_SHELLS", "📄 تب پوسته‌ها و چیدمان اصناف", 2)
            Dim stShells As New PermissionTreeNode("ST_SHELLS", "📑 زیرتب پوسته‌های عمومی، فروشگاهی و خدماتی", 3)
            AddActionNode(stShells, PermissionKeys.ManageBusinessShells, "🔘 پوسته مشاغل", dbPermissions)
            tShells.Children.Add(stShells)
            smShells.Children.Add(tShells)
            rBusinessShells.Children.Add(smShells)
            roots.Add(rBusinessShells)

            ' =========================================================================
            ' 7. منوی اصلی: امکانات (mUtilities)
            ' =========================================================================
            Dim rUtils As New PermissionTreeNode("MENU_UTILS", "🛠️ امکانات", 0)
            Dim smUtils As New PermissionTreeNode("SM_UTILS", "📁 ماشین حساب، یادداشت و تقویم", 1)
            Dim tUtils As New PermissionTreeNode("T_UTILS", "📄 تب ابزارهای جانبی و کاربردی", 2)
            Dim stUtils As New PermissionTreeNode("ST_UTILS", "📑 زیرتب یادداشت‌ها، تقویم و مناسبت‌ها", 3)
            AddActionNode(stUtils, PermissionKeys.ManageUtilities, "🔘 امکانات", dbPermissions)
            tUtils.Children.Add(stUtils)
            smUtils.Children.Add(tUtils)
            rUtils.Children.Add(smUtils)
            roots.Add(rUtils)

            ' Dynamic Scanner: Automatically discover any unmapped permissions in DB!
            Dim mappedKeys = GetAllMappedPermissionKeys(roots)
            Dim unmappedList As New List(Of KeyValuePair(Of String, Integer))()

            For Each kvp In dbPermissions
                If Not mappedKeys.Contains(kvp.Key) Then
                    unmappedList.Add(kvp)
                End If
            Next

            If unmappedList.Count > 0 Then
                Dim rNew As New PermissionTreeNode("MENU_NEW", "⚡ مجوزها و امکانات جدید سیستم", 0)
                Dim smNew As New PermissionTreeNode("SM_NEW", "📁 امکانات جدید افزوده شده در به روزرسانی‌ها", 1)
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
