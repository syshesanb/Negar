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
            ' 1. منوی اصلی: سیستم (mSystemMgmt)
            ' =========================================================================
            Dim rSys As New PermissionTreeNode("MENU_SYS", "⚙️ سیستم", 0)

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
            ' 2. منوی اصلی: کاربران (mUserMgmt)
            ' =========================================================================
            Dim rUsers As New PermissionTreeNode("MENU_USERS", "👥 کاربران", 0)

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
            ' 3. منوی اصلی: شرکت ها و سالهای مالی (mCompanyMgmt)
            ' =========================================================================
            Dim rCompanies As New PermissionTreeNode("MENU_COMPANIES", "🏢 شرکت ها و سالهای مالی", 0)

            ' 3.1 زیر منو: شرکت ها و سالهای مالی
            Dim smCompaniesYears As New PermissionTreeNode("SM_COMP_YEARS", "📁 شرکت ها و سالهای مالی", 1)
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
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeaderNew, "🔘 ثبت جدید سرفصل", dbPermissions, new String() { PermissionKeys.AccountingHeader })
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeaderEdit, "🔘 ویرایش سرفصل", dbPermissions, new String() { PermissionKeys.AccountingHeader })
            AddActionNode(stCodingHeader, PermissionKeys.AccountingHeaderDelete, "🔘 حذف سرفصل", dbPermissions, new String() { PermissionKeys.AccountingHeader })
            tCodingHeader.Children.Add(stCodingHeader)
            smAccMain.Children.Add(tCodingHeader)

            ' تب 3: حسابهای شناور
            Dim tShenavar As New PermissionTreeNode("T_SHENAVAR", "📄 تب حساب‌های شناور", 2)
            Dim stShenavar As New PermissionTreeNode("ST_SHENAVAR", "📑 زیرتب درختواره شناور اشخاص و مراکز", 3)
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavar, "🔘 حساب‌های شناور", dbPermissions)
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavarNew, "🔘 ثبت جدید شناور", dbPermissions, new String() { PermissionKeys.AccountingShenavar })
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavarEdit, "🔘 ویرایش شناور", dbPermissions, new String() { PermissionKeys.AccountingShenavar })
            AddActionNode(stShenavar, PermissionKeys.AccountingShenavarDelete, "🔘 حذف شناور", dbPermissions, new String() { PermissionKeys.AccountingShenavar })
            tShenavar.Children.Add(stShenavar)
            smAccMain.Children.Add(tShenavar)

            ' تب 4: ثبت سند حسابداری
            Dim tSanad As New PermissionTreeNode("T_SANAD", "📄 تب ثبت و ویرایش سند حسابداری", 2)
            Dim stSanadGrid As New PermissionTreeNode("ST_SANAD_GRID", "📑 زیرتب جدول سطرها و ثبت اسناد", 3)
            AddActionNode(stSanadGrid, PermissionKeys.AccountingEntry, "🔘 ثبت سند حسابداری", dbPermissions)
            AddActionNode(stSanadGrid, PermissionKeys.AccountingEntryNew, "🔘 ثبت جدید سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadGrid, PermissionKeys.AccountingEntryEdit, "🔘 ویرایش سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadGrid, PermissionKeys.AccountingEntryDelete, "🔘 حذف سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadGrid, PermissionKeys.AccountingSanadCopy, "🔘 کپی سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadGrid, PermissionKeys.AccountingSanadMerge, "🔘 ادغام سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
            AddActionNode(stSanadGrid, PermissionKeys.AccountingSanadSplit, "🔘 تجزیه سند", dbPermissions, new String() { PermissionKeys.AccountingEntry })
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
            AddActionNode(stMogBankImport, PermissionKeys.AccountingBankRecSelectFile, "🔘 انتخاب فایل صورت حساب", dbPermissions, new String() { PermissionKeys.AccountingBankRecImport })
            AddActionNode(stMogBankImport, PermissionKeys.AccountingBankRecSaveData, "🔘 ذخیره اطلاعات صورت حساب", dbPermissions, new String() { PermissionKeys.AccountingBankRecImport })
            tMogBank.Children.Add(stMogBankImport)

            Dim stMogBankMatch As New PermissionTreeNode("ST_MOG_BANK_MATCH", "📑 زیرتب مغایرت‌گیری", 3)
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecMatch, "🔘 مغایرت‌گیری", dbPermissions)
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecTransferDesc, "🔘 انتقال شرح صورت حساب به شرح ردیف دفتر", dbPermissions, new String() { PermissionKeys.AccountingBankRecMatch })
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecStatementReport, "🔘 گزارش صورتحساب بانکی", dbPermissions, new String() { PermissionKeys.AccountingBankRecMatch })
            AddActionNode(stMogBankMatch, PermissionKeys.AccountingBankRecExportExcel, "🔘 خروجی اکسل اقلام مغایرت", dbPermissions, new String() { PermissionKeys.AccountingBankRecMatch })
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
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossSaveSettings, "🔘 ذخیره تنظیمات سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossEditSettings, "🔘 ویرایش تنظیمات سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossMapAccounts, "🔘 معرفی حساب‌ها", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossCalculate, "🔘 محاسبه و نمایش", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossPrint, "🔘 نمایش و چاپ", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            AddActionNode(stProfitLossGrid, PermissionKeys.AccountingProfitLossExport, "🔘 خروجی اکسل سود و زیان", dbPermissions, new String() { PermissionKeys.AccountingProfitLoss })
            tProfitLoss.Children.Add(stProfitLossGrid)
            smAccMain.Children.Add(tProfitLoss)

            ' تب 11: ترازنامه
            Dim tBalanceSheet As New PermissionTreeNode("T_BALANCE_SHEET", "📄 تب ترازنامه", 2)
            Dim stBalanceSheetGrid As New PermissionTreeNode("ST_BALANCE_SHEET_GRID", "📑 زیرتب ترازنامه و دارایی‌ها/بدهی‌ها", 3)
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheet, "🔘 ترازنامه مالی", dbPermissions)
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetSaveSettings, "🔘 ذخیره تنظیمات ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetEditSettings, "🔘 ویرایش تنظیمات ترازنامه", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetMapAccounts, "🔘 معرفی حساب‌ها", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetCalculate, "🔘 محاسبه و نمایش", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
            AddActionNode(stBalanceSheetGrid, PermissionKeys.AccountingBalanceSheetPrint, "🔘 نمایش و چاپ", dbPermissions, new String() { PermissionKeys.AccountingBalanceSheet })
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
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReportNew, "🔘 ایجاد گزارش جدید", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReportEdit, "🔘 ویرایش گزارش", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
            AddActionNode(stOtherCustomReports, PermissionKeys.AccountingCustomReportDelete, "🔘 حذف گزارش", dbPermissions, new String() { PermissionKeys.AccountingCustomReports })
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
            
            ' تب 1: فروش سریع (POS)
            Dim tMiniPos As New PermissionTreeNode("T_MINI_POS", "📄 تب فروش سریع (POS)", 2)
            Dim stMiniPosGrid As New PermissionTreeNode("ST_MINI_POS_GRID", "📑 زیرتب صدور فاکتور و تسویه کارتخوان/نقد", 3)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniModule, "🔘 استفاده از انبارداری مینی", dbPermissions)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniPos, "🔘 فروش سریع (POS)", dbPermissions)
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniPosNew, "🔘 فاکتور فروش جدید", dbPermissions, new String() { PermissionKeys.AnbarMiniPos })
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniPosEdit, "🔘 ویرایش فاکتور فروش", dbPermissions, new String() { PermissionKeys.AnbarMiniPos })
            AddActionNode(stMiniPosGrid, PermissionKeys.AnbarMiniPosDelete, "🔘 حذف فاکتور فروش", dbPermissions, new String() { PermissionKeys.AnbarMiniPos })
            tMiniPos.Children.Add(stMiniPosGrid)
            smTradeMini.Children.Add(tMiniPos)

            ' تب 2: خرید کالا
            Dim tMiniKharid As New PermissionTreeNode("T_MINI_KHARID", "📄 تب خرید کالا", 2)
            Dim stMiniKharidGrid As New PermissionTreeNode("ST_MINI_KHARID_GRID", "📑 زیرتب ثبت فاکتورهای خرید کالا", 3)
            AddActionNode(stMiniKharidGrid, PermissionKeys.AnbarMiniKharid, "🔘 خرید کالا", dbPermissions)
            AddActionNode(stMiniKharidGrid, PermissionKeys.AnbarMiniKharidNew, "🔘 فاکتور خرید جدید", dbPermissions, new String() { PermissionKeys.AnbarMiniKharid })
            AddActionNode(stMiniKharidGrid, PermissionKeys.AnbarMiniKharidEdit, "🔘 ویرایش فاکتور خرید", dbPermissions, new String() { PermissionKeys.AnbarMiniKharid })
            AddActionNode(stMiniKharidGrid, PermissionKeys.AnbarMiniKharidDelete, "🔘 حذف فاکتور خرید", dbPermissions, new String() { PermissionKeys.AnbarMiniKharid })
            tMiniKharid.Children.Add(stMiniKharidGrid)
            smTradeMini.Children.Add(tMiniKharid)

            ' تب 3: لیست فروشنده و خریدار
            Dim tMiniPersons As New PermissionTreeNode("T_MINI_PERSONS", "📄 تب لیست فروشنده و خریدار", 2)
            Dim stMiniPersonsGrid As New PermissionTreeNode("ST_MINI_PERSONS_GRID", "📑 زیرتب طرف حساب‌ها و اشخاص", 3)
            AddActionNode(stMiniPersonsGrid, PermissionKeys.AnbarMiniPersons, "🔘 لیست فروشنده و خریدار", dbPermissions)
            AddActionNode(stMiniPersonsGrid, PermissionKeys.AnbarMiniPersonsNew, "🔘 شخص جدید", dbPermissions, new String() { PermissionKeys.AnbarMiniPersons })
            AddActionNode(stMiniPersonsGrid, PermissionKeys.AnbarMiniPersonsEdit, "🔘 ویرایش شخص", dbPermissions, new String() { PermissionKeys.AnbarMiniPersons })
            AddActionNode(stMiniPersonsGrid, PermissionKeys.AnbarMiniPersonsDelete, "🔘 حذف شخص", dbPermissions, new String() { PermissionKeys.AnbarMiniPersons })
            tMiniPersons.Children.Add(stMiniPersonsGrid)
            smTradeMini.Children.Add(tMiniPersons)

            ' تب 4: هزینه‌ها
            Dim tMiniExpenses As New PermissionTreeNode("T_MINI_EXPENSES", "📄 تب هزینه‌ها", 2)
            Dim stMiniExpensesToolbar As New PermissionTreeNode("ST_MINI_EXPENSES_TB", "📑 زیرتب ثبت هزینه‌ها و نوار ابزار", 3)
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpenses, "🔘 هزینه‌ها", dbPermissions)
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpensesSave, "🔘 ثبت هزینه", dbPermissions, new String() { PermissionKeys.AnbarMiniExpenses })
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpensesEdit, "🔘 ویرایش هزینه", dbPermissions, new String() { PermissionKeys.AnbarMiniExpenses })
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpensesDelete, "🔘 حذف هزینه", dbPermissions, new String() { PermissionKeys.AnbarMiniExpenses })
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniExpenseLedger, "🔘 📒 دفتر هزینه", dbPermissions, new String() { PermissionKeys.AnbarMiniExpenses })
            AddActionNode(stMiniExpensesToolbar, PermissionKeys.AnbarMiniProfitLoss, "🔘 🖨️ چاپ سود و زیان", dbPermissions, new String() { PermissionKeys.AnbarMiniExpenses })
            tMiniExpenses.Children.Add(stMiniExpensesToolbar)
            smTradeMini.Children.Add(tMiniExpenses)

            ' تب 5: لیست کالاها
            Dim tMiniProducts As New PermissionTreeNode("T_MINI_PRODS", "📄 تب لیست کالاها", 2)
            Dim stMiniProdsGrid As New PermissionTreeNode("ST_MINI_PRODS_GRID", "📑 زیرتب مدیریت کالاها و قیمت‌ها", 3)
            AddActionNode(stMiniProdsGrid, PermissionKeys.AnbarMiniProducts, "🔘 لیست کالاها", dbPermissions)
            AddActionNode(stMiniProdsGrid, PermissionKeys.AnbarMiniProductsNew, "🔘 کالای جدید", dbPermissions, new String() { PermissionKeys.AnbarMiniProducts })
            AddActionNode(stMiniProdsGrid, PermissionKeys.AnbarMiniProductsEdit, "🔘 ویرایش کالا", dbPermissions, new String() { PermissionKeys.AnbarMiniProducts })
            AddActionNode(stMiniProdsGrid, PermissionKeys.AnbarMiniProductsDelete, "🔘 حذف کالا", dbPermissions, new String() { PermissionKeys.AnbarMiniProducts })
            tMiniProducts.Children.Add(stMiniProdsGrid)
            smTradeMini.Children.Add(tMiniProducts)

            ' تب 6: لیست انبارها
            Dim tMiniWarehouses As New PermissionTreeNode("T_MINI_WHS", "📄 تب لیست انبارها", 2)
            Dim stMiniWhsGrid As New PermissionTreeNode("ST_MINI_WHS_GRID", "📑 زیرتب تعریف انبارها و موجودی", 3)
            AddActionNode(stMiniWhsGrid, PermissionKeys.AnbarMiniWarehouses, "🔘 لیست انبارها", dbPermissions)
            AddActionNode(stMiniWhsGrid, PermissionKeys.AnbarMiniWarehousesNew, "🔘 انبار جدید", dbPermissions, new String() { PermissionKeys.AnbarMiniWarehouses })
            AddActionNode(stMiniWhsGrid, PermissionKeys.AnbarMiniWarehousesEdit, "🔘 ویرایش انبار", dbPermissions, new String() { PermissionKeys.AnbarMiniWarehouses })
            AddActionNode(stMiniWhsGrid, PermissionKeys.AnbarMiniWarehousesDelete, "🔘 حذف انبار", dbPermissions, new String() { PermissionKeys.AnbarMiniWarehouses })
            tMiniWarehouses.Children.Add(stMiniWhsGrid)
            smTradeMini.Children.Add(tMiniWarehouses)

            ' تب 7: دسته‌بندی کالا
            Dim tMiniGroups As New PermissionTreeNode("T_MINI_GROUPS", "📄 تب دسته‌بندی کالا", 2)
            Dim stMiniGroupsGrid As New PermissionTreeNode("ST_MINI_GROUPS_GRID", "📑 زیرتب درختواره و لیست گروه‌های کالا", 3)
            AddActionNode(stMiniGroupsGrid, PermissionKeys.AnbarMiniGroups, "🔘 دسته‌بندی کالا", dbPermissions)
            AddActionNode(stMiniGroupsGrid, PermissionKeys.AnbarMiniGroupsNewTop, "🔘 دسته‌بندی جدید (بالای فرم)", dbPermissions, new String() { PermissionKeys.AnbarMiniGroups })
            AddActionNode(stMiniGroupsGrid, PermissionKeys.AnbarMiniGroupsNew, "🔘 جدید (دیتاگرید/درختواره)", dbPermissions, new String() { PermissionKeys.AnbarMiniGroups })
            AddActionNode(stMiniGroupsGrid, PermissionKeys.AnbarMiniGroupsEdit, "🔘 ویرایش (دیتاگرید/درختواره)", dbPermissions, new String() { PermissionKeys.AnbarMiniGroups })
            AddActionNode(stMiniGroupsGrid, PermissionKeys.AnbarMiniGroupsDelete, "🔘 حذف (دیتاگرید/درختواره)", dbPermissions, new String() { PermissionKeys.AnbarMiniGroups })
            tMiniGroups.Children.Add(stMiniGroupsGrid)
            smTradeMini.Children.Add(tMiniGroups)

            ' تب 8: گزارشات
            Dim tMiniReports As New PermissionTreeNode("T_MINI_REPORTS", "📄 تب گزارشات", 2)

            Dim stMiniInvStock As New PermissionTreeNode("ST_MINI_INV_STOCK", "📑 زیرتب موجودی انبار", 3)
            AddActionNode(stMiniInvStock, PermissionKeys.AnbarMiniInvStock, "🔘 موجودی انبار", dbPermissions)
            AddActionNode(stMiniInvStock, PermissionKeys.AnbarMiniInvStockPrint, "🔘 چاپ موجودی انبار", dbPermissions, new String() { PermissionKeys.AnbarMiniInvStock })
            tMiniReports.Children.Add(stMiniInvStock)

            Dim stMiniKardex As New PermissionTreeNode("ST_MINI_KARDEX", "📑 زیرتب کاردکس کالا", 3)
            AddActionNode(stMiniKardex, PermissionKeys.AnbarMiniKardex, "🔘 کاردکس کالا", dbPermissions)
            AddActionNode(stMiniKardex, PermissionKeys.AnbarMiniKardexLoad, "🔘 نمایش کاردکس", dbPermissions, new String() { PermissionKeys.AnbarMiniKardex })
            AddActionNode(stMiniKardex, PermissionKeys.AnbarMiniKardexPrint, "🔘 چاپ کاردکس", dbPermissions, new String() { PermissionKeys.AnbarMiniKardex })
            tMiniReports.Children.Add(stMiniKardex)

            Dim stMiniProfitLossRep As New PermissionTreeNode("ST_MINI_PL_REP", "📑 زیرتب سود و زیان", 3)
            AddActionNode(stMiniProfitLossRep, PermissionKeys.AnbarMiniProfitLossRep, "🔘 سود و زیان", dbPermissions)
            AddActionNode(stMiniProfitLossRep, PermissionKeys.AnbarMiniProfitLossLoad, "🔘 نمایش گزارش", dbPermissions, new String() { PermissionKeys.AnbarMiniProfitLossRep })
            AddActionNode(stMiniProfitLossRep, PermissionKeys.AnbarMiniProfitLossPrintStatement, "🔘 چاپ عملکرد", dbPermissions, new String() { PermissionKeys.AnbarMiniProfitLossRep })
            AddActionNode(stMiniProfitLossRep, PermissionKeys.AnbarMiniProfitLossPrintRep, "🔘 چاپ سود و زیان", dbPermissions, new String() { PermissionKeys.AnbarMiniProfitLossRep })
            tMiniReports.Children.Add(stMiniProfitLossRep)

            Dim stMiniInventoryCount As New PermissionTreeNode("ST_MINI_INV_COUNT", "📑 زیرتب لیست انبارگردانی", 3)
            AddActionNode(stMiniInventoryCount, PermissionKeys.AnbarMiniInventoryCount, "🔘 لیست انبارگردانی", dbPermissions)
            AddActionNode(stMiniInventoryCount, PermissionKeys.AnbarMiniInventoryCountGenerate, "🔘 تهیه لیست انبارگردانی", dbPermissions, new String() { PermissionKeys.AnbarMiniInventoryCount })
            AddActionNode(stMiniInventoryCount, PermissionKeys.AnbarMiniInventoryCountPrint, "🔘 چاپ انبارگردانی", dbPermissions, new String() { PermissionKeys.AnbarMiniInventoryCount })
            tMiniReports.Children.Add(stMiniInventoryCount)

            ' تب 9: سامانه مودیان ساده (نوع 2)
            Dim tMiniModyan As New PermissionTreeNode("T_MINI_MODYAN", "📄 تب سامانه مودیان ساده (نوع ۲)", 2)
            Dim stMiniModyan As New PermissionTreeNode("ST_MINI_MODYAN", "📑 زیرتب ارسال فاکتور فروشگاهی نوع ۲ به مودیان", 3)
            AddActionNode(stMiniModyan, PermissionKeys.AnbarMiniModyan, "🔘 سامانه مودیان ساده", dbPermissions)
            AddActionNode(stMiniModyan, PermissionKeys.AnbarMiniModyanSend, "🔘 ارسال سریع فاکتور نوع ۲ به مودیان", dbPermissions, new String() { PermissionKeys.AnbarMiniModyan })
            AddActionNode(stMiniModyan, PermissionKeys.AnbarMiniModyanGuide, "🔘 📖 مشاهده راهنمای مودیان", dbPermissions, new String() { PermissionKeys.AnbarMiniModyan })
            tMiniModyan.Children.Add(stMiniModyan)
            smTradeMini.Children.Add(tMiniModyan)

            rTrade.Children.Add(smTradeMini)

            ' 5.2 زیر منو: استفاده از انبارداری متوسط (miTradeMedium)
            Dim smTradeMed As New PermissionTreeNode("SM_TRADE_MED", "📁 استفاده از انبارداری متوسط", 1)
            Dim tMedMain As New PermissionTreeNode("T_MED_MAIN", "📄 تب اصلی انبارداری متوسط", 2)
            Dim stMedGrid As New PermissionTreeNode("ST_MED_GRID", "📑 زیرتب عملیات متوسط انبار و کالا", 3)
            AddActionNode(stMedGrid, PermissionKeys.AnbarMediumModule, "🔘 استفاده از انبارداری متوسط", dbPermissions)
            tMedMain.Children.Add(stMedGrid)
            smTradeMed.Children.Add(tMedMain)

            Dim tMedModyan As New PermissionTreeNode("T_MED_MODYAN", "📄 تب سامانه مودیان پیشرفته (نوع ۱ و ۲)", 2)
            Dim stMedModyan As New PermissionTreeNode("ST_MED_MODYAN", "📑 زیرتب ارسال صورتحساب‌های رسمی B2B و تنظیمات مالیاتی", 3)
            AddActionNode(stMedModyan, PermissionKeys.TradeModyanMedium, "🔘 سامانه مودیان پیشرفته", dbPermissions)
            AddActionNode(stMedModyan, PermissionKeys.TradeModyanKeysSetup, "🔘 🔑 تنظیم کلیدها و حافظه مالیاتی", dbPermissions, new String() { PermissionKeys.TradeModyanMedium })
            AddActionNode(stMedModyan, PermissionKeys.TradeModyanSendInvoices, "🔘 🚀 ارسال صورتحساب‌های رسمی به کارپوشه", dbPermissions, new String() { PermissionKeys.TradeModyanMedium })
            AddActionNode(stMedModyan, PermissionKeys.TradeModyanInquiry, "🔘 🔄 استعلام وضعیت صورتحساب‌ها", dbPermissions, new String() { PermissionKeys.TradeModyanMedium })
            tMedModyan.Children.Add(stMedModyan)
            smTradeMed.Children.Add(tMedModyan)

            rTrade.Children.Add(smTradeMed)

            ' 5.3 زیر منو: استفاده از انبارداری پیشرفته (miTradeBig)
            Dim smTradeBig As New PermissionTreeNode("SM_TRADE_BIG", "📁 استفاده از انبارداری پیشرفته", 1)
            Dim tBigMain As New PermissionTreeNode("T_BIG_MAIN", "📄 تب اصلی انبارداری پیشرفته", 2)
            Dim stBigGrid As New PermissionTreeNode("ST_BIG_GRID", "📑 زیرتب کنترل پیشرفته چند انباره", 3)
            AddActionNode(stBigGrid, PermissionKeys.AnbarBigModule, "🔘 استفاده از انبارداری پیشرفته", dbPermissions)
            tBigMain.Children.Add(stBigGrid)
            smTradeBig.Children.Add(tBigMain)

            Dim tBigModyan As New PermissionTreeNode("T_BIG_MODYAN", "📄 تب سامانه مودیان پیشرفته جامع", 2)
            Dim stBigModyan As New PermissionTreeNode("ST_BIG_MODYAN", "📑 زیرتب مدیریت جامع کلیدها، شناسه کالاها و ارسال دسته‌ای", 3)
            AddActionNode(stBigModyan, PermissionKeys.TradeModyanBig, "🔘 سامانه مودیان پیشرفته جامع", dbPermissions)
            AddActionNode(stBigModyan, PermissionKeys.TradeModyanKeysSetup, "🔘 🔑 تنظیم کلیدها و حافظه مالیاتی", dbPermissions, new String() { PermissionKeys.TradeModyanBig })
            AddActionNode(stBigModyan, PermissionKeys.TradeModyanSendInvoices, "🔘 🚀 ارسال صورتحساب‌های رسمی به کارپوشه", dbPermissions, new String() { PermissionKeys.TradeModyanBig })
            AddActionNode(stBigModyan, PermissionKeys.TradeModyanInquiry, "🔘 🔄 استعلام وضعیت صورتحساب‌ها", dbPermissions, new String() { PermissionKeys.TradeModyanBig })
            tBigModyan.Children.Add(stBigModyan)
            smTradeBig.Children.Add(tBigModyan)

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

            ' =========================================================================
            ' 9. منوی اصلی: مدیریت حقوق و دستمزد (mPayroll)
            ' =========================================================================
            Dim rPayroll As New PermissionTreeNode("MENU_PAYROLL", "💳 مدیریت حقوق و دستمزد", 0)
            Dim smPayroll As New PermissionTreeNode("SM_PAYROLL", "📁 بخش جامع حقوق و دستمزد و کارکرد پرسنل", 1)
            Dim tPayroll As New PermissionTreeNode("T_PAYROLL", "📄 امکانات و ماژول‌های حقوق و دستمزد", 2)
            Dim stPayroll As New PermissionTreeNode("ST_PAYROLL", "📑 زیرتب عملیات حقوق، دیسکت‌ها و گزارشات", 3)

            AddActionNode(stPayroll, PermissionKeys.PayrollModule, "🔘 دسترسی به ماژول حقوق و دستمزد", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollPersonnel, "🔘 پرونده پرسنل و احکام حقوقی", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollAttendance, "🔘 ثبت کارکرد و حضور و غیاب", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollCalculate, "🔘 محاسبه حقوق و صدور فیش حقوقی", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollDiskettes, "🔘 تولید دیسکت‌های بیمه و مالیات", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollBankFile, "🔘 تولید فایل پرداخت گروهی بانک", dbPermissions)
            AddActionNode(stPayroll, PermissionKeys.PayrollReports, "🔘 گزارشات جامع حقوق و دستمزد", dbPermissions)

            tPayroll.Children.Add(stPayroll)
            smPayroll.Children.Add(tPayroll)
            rPayroll.Children.Add(smPayroll)
            roots.Add(rPayroll)

            ' =========================================================================
            ' 10. منوی اصلی: اموال و دارایی‌های ثابت (mAmval)
            ' =========================================================================
            Dim rAmval As New PermissionTreeNode("MENU_AMVAL", "🏛️ اموال", 0)
            Dim smAmvalMain As New PermissionTreeNode("SM_AMVAL_MAIN", "📁 سیستم جامع اموال", 1)
            Dim tAmvalMain As New PermissionTreeNode("T_AMVAL_MAIN", "📄 مدیریت شناسنامه دارایی‌ها، استهلاک و جابجایی", 2)
            Dim stAmvalMain As New PermissionTreeNode("ST_AMVAL_MAIN", "📑 عملیات پلاک‌گذاری، محاسبه استهلاک و اموال‌گردانی", 3)

            AddActionNode(stAmvalMain, PermissionKeys.AmvalModule, "🔘 دسترسی به سیستم جامع اموال", dbPermissions)
            AddActionNode(stAmvalMain, PermissionKeys.AmvalAssets, "🔘 ثبت و ویرایش شناسنامه دارایی‌ها", dbPermissions)
            AddActionNode(stAmvalMain, PermissionKeys.AmvalDepreciation, "🔘 محاسبه استهلاک دوره و صدور سند", dbPermissions)
            AddActionNode(stAmvalMain, PermissionKeys.AmvalTransfers, "🔘 ثبت جابجایی و تعمیرات اساسی", dbPermissions)
            AddActionNode(stAmvalMain, PermissionKeys.AmvalInventory, "🔘 اموال‌گردانی و خروج دارایی", dbPermissions)

            tAmvalMain.Children.Add(stAmvalMain)
            smAmvalMain.Children.Add(tAmvalMain)
            rAmval.Children.Add(smAmvalMain)

            Dim smAmvalReports As New PermissionTreeNode("SM_AMVAL_REPORTS", "📁 گزارشات جامع اموال", 1)
            Dim tAmvalReports As New PermissionTreeNode("T_AMVAL_REPORTS", "📄 گزارش‌های استهلاک، کارت اموال و اموال نزد پرسنل", 2)
            Dim stAmvalReports As New PermissionTreeNode("ST_AMVAL_REPORTS", "📑 جدول استهلاک دارایی‌ها و گزارشات مدیریتی", 3)

            AddActionNode(stAmvalReports, PermissionKeys.AmvalReports, "🔘 گزارشات جامع اموال و دارایی‌ها", dbPermissions)

            tAmvalReports.Children.Add(stAmvalReports)
            smAmvalReports.Children.Add(tAmvalReports)
            rAmval.Children.Add(smAmvalReports)
            roots.Add(rAmval)

            ' =========================================================================
            ' 11. منوی اصلی: اتوماسیون اداری (mAutomation)
            ' =========================================================================
            Dim rAuto As New PermissionTreeNode("MENU_AUTOMATION", "📨 اتوماسیون اداری", 0)
            Dim smAutoMain As New PermissionTreeNode("SM_AUTO_MAIN", "📁 سیستم جامع اتوماسیون اداری", 1)
            Dim tAutoMain As New PermissionTreeNode("T_AUTO_MAIN", "📄 مدیریت نامه‌ها، دبیرخانه، کارتابل و ارجاعات", 2)
            Dim stAutoMain As New PermissionTreeNode("ST_AUTO_MAIN", "📑 عملیات ثبت نامه، ارجاع، کارتابل و اندیکاتور", 3)

            AddActionNode(stAutoMain, PermissionKeys.AutomationModule, "🔘 دسترسی به سیستم جامع اتوماسیون اداری", dbPermissions)
            AddActionNode(stAutoMain, PermissionKeys.AutomationLetters, "🔘 ثبت و مدیریت مکاتبات و نامه‌ها", dbPermissions)
            AddActionNode(stAutoMain, PermissionKeys.AutomationInbox, "🔘 کارتابل الکترونیک و ارجاعات نامه‌ها", dbPermissions)
            AddActionNode(stAutoMain, PermissionKeys.AutomationSecretariat, "🔘 دبیرخانه، اندیکاتور و شماره‌گذاری", dbPermissions)

            tAutoMain.Children.Add(stAutoMain)
            smAutoMain.Children.Add(tAutoMain)
            rAuto.Children.Add(smAutoMain)

            Dim smAutoReports As New PermissionTreeNode("SM_AUTO_REPORTS", "📁 گزارشات جامع اتوماسیون اداری", 1)
            Dim tAutoReports As New PermissionTreeNode("T_AUTO_REPORTS", "📄 گزارش‌های آماری نامه‌ها، گردش مکاتبات و معوقات", 2)
            Dim stAutoReports As New PermissionTreeNode("ST_AUTO_REPORTS", "📑 گزارشات مدیریتی و چرخه ارجاعات اداری", 3)

            AddActionNode(stAutoReports, PermissionKeys.AutomationReports, "🔘 گزارشات جامع اتوماسیون اداری", dbPermissions)

            tAutoReports.Children.Add(stAutoReports)
            smAutoReports.Children.Add(tAutoReports)
            rAuto.Children.Add(smAutoReports)
            roots.Add(rAuto)

            ' =========================================================================
            ' 12. منوی اصلی: مدیریت ارتباط با مشتریان (mCrm)
            ' =========================================================================
            Dim rCrm As New PermissionTreeNode("MENU_CRM", "🤝 سیستم جامع باشگاه مشتریان (CRM)", 0)
            Dim smCrmMain As New PermissionTreeNode("SM_CRM_MAIN", "📁 سیستم جامع CRM و فرصت‌های فروش", 1)
            Dim tCrmMain As New PermissionTreeNode("T_CRM_MAIN", "📄 پرونده ۳۶۰ درجه مشتریان، قیف فروش و پیگیری‌ها", 2)
            Dim stCrmMain As New PermissionTreeNode("ST_CRM_MAIN", "📑 ثبت سرنخ، فرصت فروش، پیش‌فاکتور و تیکت پشتیبانی", 3)

            AddActionNode(stCrmMain, PermissionKeys.CrmModule, "🔘 دسترسی به سیستم جامع CRM", dbPermissions)
            AddActionNode(stCrmMain, PermissionKeys.CrmLeads, "🔘 مدیریت پرونده سرنخ‌ها و مشتریان", dbPermissions)
            AddActionNode(stCrmMain, PermissionKeys.CrmOpportunities, "🔘 قیف فروش و تبدیل به فاکتور فروش", dbPermissions)
            AddActionNode(stCrmMain, PermissionKeys.CrmActivities, "🔘 ثبت فعالیت‌ها و پیگیری‌ها", dbPermissions)
            AddActionNode(stCrmMain, PermissionKeys.CrmTickets, "🔘 خدمات پس از فروش و پشتیبانی", dbPermissions)

            tCrmMain.Children.Add(stCrmMain)
            smCrmMain.Children.Add(tCrmMain)
            rCrm.Children.Add(smCrmMain)

            Dim smCrmReports As New PermissionTreeNode("SM_CRM_REPORTS", "📁 گزارشات جامع CRM و تحلیل فروش", 1)
            Dim tCrmReports As New PermissionTreeNode("T_CRM_REPORTS", "📄 گزارش‌های آماری تحلیل فروش و نرخ تبدیل", 2)
            Dim stCrmReports As New PermissionTreeNode("ST_CRM_REPORTS", "📑 گزارشات مدیریتی قیف فروش و عملکرد فروشندگان", 3)

            AddActionNode(stCrmReports, PermissionKeys.CrmReports, "🔘 گزارشات جامع CRM و تحلیل فروش", dbPermissions)

            tCrmReports.Children.Add(stCrmReports)
            smCrmReports.Children.Add(tCrmReports)
            rCrm.Children.Add(smCrmReports)
            roots.Add(rCrm)

            ' =========================================================================
            ' 13. منوی اصلی: خزانه‌داری پیشرفته و جریان نقدینگی (mTreasury)
            ' =========================================================================
            Dim rTreasury As New PermissionTreeNode("MENU_TREASURY", "💰 سیستم جامع خزانه‌داری و مدیریت نقدینگی", 0)
            Dim smTreasuryMain As New PermissionTreeNode("SM_TREASURY_MAIN", "📁 سیستم جامع خزانه‌داری و اسناد تجاری", 1)
            Dim tTreasuryMain As New PermissionTreeNode("T_TREASURY_MAIN", "📄 مدیریت بانک‌ها، صندوق‌ها، چک‌ها و تسهیلات", 2)
            Dim stTreasuryMain As New PermissionTreeNode("ST_TREASURY_MAIN", "📑 عملیات دریافت و پرداخت، چرخه چک، وام و Cash Flow", 3)

            AddActionNode(stTreasuryMain, PermissionKeys.TreasuryModule, "🔘 دسترسی به سیستم جامع خزانه‌داری", dbPermissions)
            AddActionNode(stTreasuryMain, PermissionKeys.TreasuryCashBanks, "🔘 مدیریت بانک‌ها، صندوق‌ها و تنخواه‌گردان‌ها", dbPermissions)
            AddActionNode(stTreasuryMain, PermissionKeys.TreasuryChecks, "🔘 مدیریت چرخه چک‌ها و اسناد تجاری", dbPermissions)
            AddActionNode(stTreasuryMain, PermissionKeys.TreasuryLoans, "🔘 مدیریت تسهیلات و وام‌های بانکی", dbPermissions)
            AddActionNode(stTreasuryMain, PermissionKeys.TreasuryCashFlow, "🔘 پیش‌بینی جریان وجوه نقد (Cash Flow)", dbPermissions)

            tTreasuryMain.Children.Add(stTreasuryMain)
            smTreasuryMain.Children.Add(tTreasuryMain)
            rTreasury.Children.Add(smTreasuryMain)

            Dim smTreasuryReports As New PermissionTreeNode("SM_TREASURY_REPORTS", "📁 گزارشات جامع خزانه‌داری و Cash Flow", 1)
            Dim tTreasuryReports As New PermissionTreeNode("T_TREASURY_REPORTS", "📄 گزارش‌های آماری راس‌گیری، مغایرت‌گیری و تسهیلات", 2)
            Dim stTreasuryReports As New PermissionTreeNode("ST_TREASURY_REPORTS", "📑 گزارشات مدیریتی منابع و مصارف نقدینگی", 3)

            AddActionNode(stTreasuryReports, PermissionKeys.TreasuryReports, "🔘 گزارشات جامع خزانه‌داری و نقدینگی", dbPermissions)

            tTreasuryReports.Children.Add(stTreasuryReports)
            smTreasuryReports.Children.Add(tTreasuryReports)
            rTreasury.Children.Add(smTreasuryReports)
            roots.Add(rTreasury)

            ' =========================================================================
            ' 14. منوی اصلی: سیستم بودجه و کنترل هزینه‌ها (mBudgeting)
            ' =========================================================================
            Dim rBudget As New PermissionTreeNode("MENU_BUDGETING", "📊 سیستم جامع بودجه و کنترل هزینه‌ها", 0)
            Dim smBudgetMain As New PermissionTreeNode("SM_BUDGET_MAIN", "📁 سیستم جامع بودجه و کنترل هزینه‌ها", 1)
            Dim tBudgetMain As New PermissionTreeNode("T_BUDGET_MAIN", "📄 تعریف بودجه مصوب، ارزیابی انحراف زنده و متمم بودجه", 2)
            Dim stBudgetMain As New PermissionTreeNode("ST_BUDGET_MAIN", "📑 ثبت سقف اعتبار، کنترل پیشگیرانه هزینه و جابجایی اعتبار", 3)

            AddActionNode(stBudgetMain, PermissionKeys.BudgetingModule, "🔘 دسترسی به سیستم جامع بودجه و کنترل هزینه", dbPermissions)
            AddActionNode(stBudgetMain, PermissionKeys.BudgetingItems, "🔘 مدیریت ردیف‌های بودجه مصوب", dbPermissions)
            AddActionNode(stBudgetMain, PermissionKeys.BudgetingEnforcement, "🔘 پایش و کنترل زنده انحراف بودجه", dbPermissions)
            AddActionNode(stBudgetMain, PermissionKeys.BudgetingAmendments, "🔘 ثبت متمم بودجه و جابجایی اعتبار", dbPermissions)

            tBudgetMain.Children.Add(stBudgetMain)
            smBudgetMain.Children.Add(tBudgetMain)
            rBudget.Children.Add(smBudgetMain)

            Dim smBudgetReports As New PermissionTreeNode("SM_BUDGET_REPORTS", "📁 گزارشات انحراف بودجه و انضباط مالی", 1)
            Dim tBudgetReports As New PermissionTreeNode("T_BUDGET_REPORTS", "📄 گزارش‌های ماتریسی انحراف بودجه و درصد جذب", 2)
            Dim stBudgetReports As New PermissionTreeNode("ST_BUDGET_REPORTS", "📑 گزارشات مدیریتی انحرافات مساعد و نامساعد", 3)

            AddActionNode(stBudgetReports, PermissionKeys.BudgetingReports, "🔘 گزارشات جامع انحراف بودجه و انضباط مالی", dbPermissions)

            tBudgetReports.Children.Add(stBudgetReports)
            smBudgetReports.Children.Add(tBudgetReports)
            rBudget.Children.Add(smBudgetReports)

            roots.Add(rBudget)

            ' =========================================================================
            ' 15. منوی اصلی: سیستم بهای تمام‌شده و برنامه‌ریزی تولید (mProduction)
            ' =========================================================================
            Dim rProd As New PermissionTreeNode("MENU_PRODUCTION", "🏭 سیستم جامع بهای تمام‌شده و مدیریت تولید", 0)
            Dim smProdMain As New PermissionTreeNode("SM_PROD_MAIN", "📁 سیستم جامع بهای تمام‌شده و برنامه‌ریزی تولید", 1)
            Dim tProdMain As New PermissionTreeNode("T_PROD_MAIN", "📄 تعریف فرمول ساخت (BOM)، دستورات تولید و بهای تمام‌شده", 2)
            Dim stProdMain As New PermissionTreeNode("ST_PROD_MAIN", "📑 کارت تولید، ۳ عنصر اصلی بهای تمام‌شده و ارزیابی WIP", 3)

            AddActionNode(stProdMain, PermissionKeys.ProductionModule, "🔘 دسترسی به سیستم بهای تمام‌شده و تولید", dbPermissions)
            AddActionNode(stProdMain, PermissionKeys.ProductionBOM, "🔘 مدیریت فرمول ساخت و BOM کالاها", dbPermissions)
            AddActionNode(stProdMain, PermissionKeys.ProductionOrders, "🔘 صدور و مدیریت دستورات و کارت‌های تولید", dbPermissions)
            AddActionNode(stProdMain, PermissionKeys.ProductionCosting, "🔘 محاسبات بهای تمام‌شده (مواد، دستمزد، سربار)", dbPermissions)
            AddActionNode(stProdMain, PermissionKeys.ProductionWIP, "🔘 ارزیابی کالای در جریان ساخت (WIP) و ضایعات", dbPermissions)

            tProdMain.Children.Add(stProdMain)
            smProdMain.Children.Add(tProdMain)
            rProd.Children.Add(smProdMain)

            Dim smProdReports As New PermissionTreeNode("SM_PROD_REPORTS", "📁 گزارشات جامع بهای تمام‌شده و آنالیز BOM", 1)
            Dim tProdReports As New PermissionTreeNode("T_PROD_REPORTS", "📄 گزارش‌های آنالیز عناصر بهای تمام‌شده و سودآوری محصولات", 2)
            Dim stProdReports As New PermissionTreeNode("ST_PROD_REPORTS", "📑 گزارشات مدیریتی انحراف بهای تمام‌شده و راندمان تولید", 3)

            AddActionNode(stProdReports, PermissionKeys.ProductionReports, "🔘 گزارشات جامع بهای تمام‌شده و سودآوری کالاها", dbPermissions)

            tProdReports.Children.Add(stProdReports)
            smProdReports.Children.Add(tProdReports)
            rProd.Children.Add(smProdReports)

            roots.Add(rProd)

            ' =========================================================================
            ' 16. منوی اصلی: سیستم مدیریت پروژه‌ها و پیمان‌ها (mProject)
            ' =========================================================================
            Dim rProject As New PermissionTreeNode("MENU_PROJECT", "🏗️ سیستم جامع مدیریت پروژه‌ها و پیمان‌ها", 0)
            Dim smProjMain As New PermissionTreeNode("SM_PROJ_MAIN", "📁 سیستم جامع مدیریت پروژه‌ها و پیمان‌ها", 1)
            Dim tProjMain As New PermissionTreeNode("T_PROJ_MAIN", "📄 شناسنامه پیمان، ساختار شکست کار (WBS) و صورت‌وضعیت‌ها", 2)
            Dim stProjMain As New PermissionTreeNode("ST_PROJ_MAIN", "📑 ثبت کارکرد، کسورات قانونی، صورت‌وضعیت و ضمانت‌نامه‌ها", 3)

            AddActionNode(stProjMain, PermissionKeys.ProjectModule, "🔘 دسترسی به سیستم مدیریت پروژه‌ها و پیمان‌ها", dbPermissions)
            AddActionNode(stProjMain, PermissionKeys.ProjectProfile, "🔘 مدیریت شناسنامه پیمان‌ها و قراردادها", dbPermissions)
            AddActionNode(stProjMain, PermissionKeys.ProjectWBS, "🔘 مدیریت ساختار شکست کار (WBS)", dbPermissions)
            AddActionNode(stProjMain, PermissionKeys.ProjectClaims, "🔘 مدیریت صورت‌وضعیت‌ها و کسورات قانونی", dbPermissions)
            AddActionNode(stProjMain, PermissionKeys.ProjectGuarantees, "🔘 مدیریت ضمانت‌نامه‌های بانکی و سپرده‌ها", dbPermissions)

            tProjMain.Children.Add(stProjMain)
            smProjMain.Children.Add(tProjMain)
            rProject.Children.Add(smProjMain)

            Dim smProjReports As New PermissionTreeNode("SM_PROJ_REPORTS", "📁 گزارشات جامع پروژه‌ها و آنالیز سود و زیان (P&L)", 1)
            Dim tProjReports As New PermissionTreeNode("T_PROJ_REPORTS", "📄 گزارش‌های سود و زیان تفکیکی پروژه‌ها و انحراف بودجه", 2)
            Dim stProjReports As New PermissionTreeNode("ST_PROJ_REPORTS", "📑 گزارشات مدیریتی مطالبات کارفرما و سررسید ضمانت‌نامه‌ها", 3)

            AddActionNode(stProjReports, PermissionKeys.ProjectReports, "🔘 گزارشات جامع پروژه‌ها و سود و زیان پیمان‌ها", dbPermissions)

            tProjReports.Children.Add(stProjReports)
            smProjReports.Children.Add(tProjReports)
            rProject.Children.Add(smProjReports)

            roots.Add(rProject)

            ' =========================================================================
            ' 17. منوی اصلی: سیستم ارزیابی عملکرد و پاداش پرسنل (mKpi)
            ' =========================================================================
            Dim rKpi As New PermissionTreeNode("MENU_KPI", "🎯 سیستم جامع ارزیابی عملکرد و پاداش پرسنل (KPI)", 0)
            Dim smKpiMain As New PermissionTreeNode("SM_KPI_MAIN", "📁 سیستم جامع ارزیابی عملکرد و کارانه", 1)
            Dim tKpiMain As New PermissionTreeNode("T_KPI_MAIN", "📄 تعریف شاخص‌های عملکرد (KPI)، ارزیابی ۳۶۰ درجه و پاداش", 2)
            Dim stKpiMain As New PermissionTreeNode("ST_KPI_MAIN", "📑 ثبت اهداف SMART، نمره‌دهی دوره‌ای، فرمول پاداش و فیش کارانه", 3)

            AddActionNode(stKpiMain, PermissionKeys.KpiModule, "🔘 دسترسی به سیستم ارزیابی عملکرد و پاداش", dbPermissions)
            AddActionNode(stKpiMain, PermissionKeys.KpiDefinitions, "🔘 مدیریت بانک شاخص‌های KPI و هدف‌گذاری", dbPermissions)
            AddActionNode(stKpiMain, PermissionKeys.KpiEvaluations, "🔘 ارزیابی دوره‌ای و ارزیابی ۳۶۰ درجه", dbPermissions)
            AddActionNode(stKpiMain, PermissionKeys.KpiBonuses, "🔘 محاسبات پاداش، کارانه و انتقال به حقوق", dbPermissions)

            tKpiMain.Children.Add(stKpiMain)
            smKpiMain.Children.Add(tKpiMain)
            rKpi.Children.Add(smKpiMain)

            Dim smKpiReports As New PermissionTreeNode("SM_KPI_REPORTS", "📁 گزارشات جامع ارزیابی عملکرد و کارانه", 1)
            Dim tKpiReports As New PermissionTreeNode("T_KPI_REPORTS", "📄 گزارش‌های ماتریس ارزیابی (9-Box Grid) و تحقق اهداف", 2)
            Dim stKpiReports As New PermissionTreeNode("ST_KPI_REPORTS", "📑 گزارشات مدیریتی کارانه پرداختی و انحراف عملکرد", 3)

            AddActionNode(stKpiReports, PermissionKeys.KpiReports, "🔘 گزارشات جامع ارزیابی عملکرد و پاداش پرسنل", dbPermissions)

            tKpiReports.Children.Add(stKpiReports)
            smKpiReports.Children.Add(tKpiReports)
            rKpi.Children.Add(smKpiReports)

            roots.Add(rKpi)

            ' =========================================================================
            ' 18. منوی اصلی: سیستم بازرگانی خارجی و واردات/صادرات (mImportExport)
            ' =========================================================================
            Dim rImp As New PermissionTreeNode("MENU_IMPORT_EXPORT", "🚢 سیستم جامع بازرگانی خارجی و واردات/صادرات", 0)
            Dim smImpMain As New PermissionTreeNode("SM_IMP_MAIN", "📁 سیستم جامع بازرگانی خارجی و واردات", 1)
            Dim tImpMain As New PermissionTreeNode("T_IMP_MAIN", "📄 ثبت پروفرما (PI)، اعتبارات اسنادی (LC)، ترخیص و بهای تمام‌شده", 2)
            Dim stImpMain As New PermissionTreeNode("ST_IMP_MAIN", "📑 ثبت سفارش، تخصیص ارز، پروانه سبز گمرکی و Landed Costing", 3)

            AddActionNode(stImpMain, PermissionKeys.ImportExportModule, "🔘 دسترسی به سیستم بازرگانی خارجی و واردات/صادرات", dbPermissions)
            AddActionNode(stImpMain, PermissionKeys.ImportExportProforma, "🔘 مدیریت پرونده‌های خرید خارجی و پروفرما (PI)", dbPermissions)
            AddActionNode(stImpMain, PermissionKeys.ImportExportLC, "🔘 مدیریت اعتبارات اسنادی (LC) و حواله‌های ارزی", dbPermissions)
            AddActionNode(stImpMain, PermissionKeys.ImportExportCustoms, "🔘 مدیریت اسناد ترخیص و گمرک", dbPermissions)
            AddActionNode(stImpMain, PermissionKeys.ImportExportCosting, "🔘 محاسبات بهای تمام‌شده واقعی کالای وارداتی (Landed Cost)", dbPermissions)

            tImpMain.Children.Add(stImpMain)
            smImpMain.Children.Add(tImpMain)
            rImp.Children.Add(smImpMain)

            Dim smImpReports As New PermissionTreeNode("SM_IMP_REPORTS", "📁 گزارشات جامع بازرگانی خارجی و بهای تمام‌شده واردات", 1)
            Dim tImpReports As New PermissionTreeNode("T_IMP_REPORTS", "📄 گزارش‌های آنالیز بهای تمام‌شده واقعی (Landed Cost) و تسعیر ارز", 2)
            Dim stImpReports As New PermissionTreeNode("ST_IMP_REPORTS", "📑 گزارشات مدیریتی وضعیت ترخیص پرونده‌ها و اعتبارات اسنادی", 3)

            AddActionNode(stImpReports, PermissionKeys.ImportExportReports, "🔘 گزارشات جامع بازرگانی خارجی و بهای تمام‌شده واردات", dbPermissions)

            tImpReports.Children.Add(stImpReports)
            smImpReports.Children.Add(tImpReports)
            rImp.Children.Add(smImpReports)
            roots.Add(rImp)

            ' =========================================================================
            ' 19. منوی اصلی: سیستم مدیریت نت، نگهداری و تعمیرات (mPm)
            ' =========================================================================
            Dim rPm As New PermissionTreeNode("MENU_PM", "🔧 سیستم جامع مدیریت نت، نگهداری و تعمیرات (PM)", 0)
            Dim smPmMain As New PermissionTreeNode("SM_PM_MAIN", "📁 سیستم جامع مدیریت نگهداری و تعمیرات", 1)
            Dim tPmMain As New PermissionTreeNode("T_PM_MAIN", "📄 شناسنامه ماشین‌آلات، برنامه‌ریزی نت، دستورکارها و قطعات یدکی", 2)
            Dim stPmMain As New PermissionTreeNode("ST_PM_MAIN", "📑 کنترل سرویس‌های دوره‌ای، تعمیرات اضطراری (EM) و ثبت اسناد نت", 3)

            AddActionNode(stPmMain, PermissionKeys.PmModule, "🔘 دسترسی به سیستم نگهداری و تعمیرات (PM)", dbPermissions)
            AddActionNode(stPmMain, PermissionKeys.PmAssets, "🔘 مدیریت شناسنامه تجهیزات و ماشین‌آلات", dbPermissions)
            AddActionNode(stPmMain, PermissionKeys.PmSchedules, "🔘 برنامه‌ریزی نت پیشگیرانه (PM)", dbPermissions)
            AddActionNode(stPmMain, PermissionKeys.PmWorkOrders, "🔘 مدیریت دستورکارها و تعمیرات اضطراری (EM)", dbPermissions)
            AddActionNode(stPmMain, PermissionKeys.PmCosting, "🔘 محاسبات بهای تمام‌شده و ثبت سند هزینه تعمیرات", dbPermissions)

            tPmMain.Children.Add(stPmMain)
            smPmMain.Children.Add(tPmMain)
            rPm.Children.Add(smPmMain)

            Dim smPmReports As New PermissionTreeNode("SM_PM_REPORTS", "📁 گزارشات جامع نت، OEE و توقفات خط تولید", 1)
            Dim tPmReports As New PermissionTreeNode("T_PM_REPORTS", "📄 گزارش‌های شاخص‌های OEE، MTBF، MTTR و هزینه تعمیرات", 2)
            Dim stPmReports As New PermissionTreeNode("ST_PM_REPORTS", "📑 گزارشات مدیریتی کارایی تجهیزات و مصرف قطعات یدکی", 3)

            AddActionNode(stPmReports, PermissionKeys.PmReports, "🔘 گزارشات جامع مدیریت نت و شاخص‌های OEE", dbPermissions)

            tPmReports.Children.Add(stPmReports)
            smPmReports.Children.Add(tPmReports)
            rPm.Children.Add(smPmReports)
            roots.Add(rPm)

            ' =========================================================================
            ' 20. منوی اصلی: سیستم مدیریت ناوگان حمل، پخش مویرگی و لوجستیک (mLogistics)
            ' =========================================================================
            Dim rLog As New PermissionTreeNode("MENU_LOGISTICS", "🚚 سیستم جامع مدیریت ناوگان حمل، پخش مویرگی و لوجستیک", 0)
            Dim smLogMain As New PermissionTreeNode("SM_LOG_MAIN", "📁 سیستم جامع مدیریت ناوگان و پخش مویرگی", 1)
            Dim tLogMain As New PermissionTreeNode("T_LOG_MAIN", "📄 شناسنامه وسایط نقلیه، مناطق توزیع، بارنامه‌ها و پورسانت موزعان", 2)
            Dim stLogMain As New PermissionTreeNode("ST_LOG_MAIN", "📑 کنترل تورهای توزیع، تسویه‌حساب بارنامه‌ها و ثبت اسناد حمل", 3)

            AddActionNode(stLogMain, PermissionKeys.LogisticsModule, "🔘 دسترسی به سیستم لوجستیک و پخش مویرگی", dbPermissions)
            AddActionNode(stLogMain, PermissionKeys.LogisticsFleet, "🔘 مدیریت شناسنامه ناوگان و وسایط نقلیه", dbPermissions)
            AddActionNode(stLogMain, PermissionKeys.LogisticsRoutes, "🔘 مدیریت مسیرها و مناطق توزیع", dbPermissions)
            AddActionNode(stLogMain, PermissionKeys.LogisticsManifests, "🔘 مدیریت بارنامه‌ها و تورهای توزیع", dbPermissions)
            AddActionNode(stLogMain, PermissionKeys.LogisticsCosting, "🔘 محاسبات کرایه حمل، پورسانت و تسویه‌حساب بارنامه", dbPermissions)

            tLogMain.Children.Add(stLogMain)
            smLogMain.Children.Add(tLogMain)
            rLog.Children.Add(smLogMain)

            Dim smLogReports As New PermissionTreeNode("SM_LOG_REPORTS", "📁 گزارشات جامع لوجستیک، کرایه حمل و تحویل فاکتورها", 1)
            Dim tLogReports As New PermissionTreeNode("T_LOG_REPORTS", "📄 گزارش‌های انحراف کرایه حمل، تحلیل عدم تحویل و کارکرد ناوگان", 2)
            Dim stLogReports As New PermissionTreeNode("ST_LOG_REPORTS", "📑 گزارشات مدیریتی پورسانت موزعان و تسویه‌حساب بارنامه‌ها", 3)

            AddActionNode(stLogReports, PermissionKeys.LogisticsReports, "🔘 گزارشات جامع لوجستیک و تحلیل پخش مویرگی", dbPermissions)

            tLogReports.Children.Add(stLogReports)
            smLogReports.Children.Add(tLogReports)
            rLog.Children.Add(smLogReports)

            roots.Add(rLog)

            ' =========================================================================
            ' 21. منوی اصلی: سیستم ارزیابی و مدیریت ارتباط با تامین‌کنندگان (mSrm)
            ' =========================================================================
            Dim rSrm As New PermissionTreeNode("MENU_SRM", "🤝 سیستم جامع ارزیابی و مدیریت ارتباط با تامین‌کنندگان (SRM)", 0)
            Dim smSrmMain As New PermissionTreeNode("SM_SRM_MAIN", "📁 سیستم جامع مدیریت ارتباط با تامین‌کنندگان", 1)
            Dim tSrmMain As New PermissionTreeNode("T_SRM_MAIN", "📄 شناسنامه تامین‌کنندگان، استعلام قیمت (RFQ)، ارزیابی و گریدبندی", 2)
            Dim stSrmMain As New PermissionTreeNode("ST_SRM_MAIN", "📑 ثبت مناقصات، کارت امتیازی (Scorecard) و قراردادهای تامین", 3)

            AddActionNode(stSrmMain, PermissionKeys.SrmModule, "🔘 دسترسی به سیستم مدیریت تامین‌کنندگان (SRM)", dbPermissions)
            AddActionNode(stSrmMain, PermissionKeys.SrmSuppliers, "🔘 مدیریت شناسنامه و بانک اطلاعات تامین‌کنندگان", dbPermissions)
            AddActionNode(stSrmMain, PermissionKeys.SrmRfq, "🔘 مدیریت استعلام‌های قیمت خرید (RFQ)", dbPermissions)
            AddActionNode(stSrmMain, PermissionKeys.SrmEvaluations, "🔘 ارزیابی دوره‌ای و سیستم کارت امتیازی تامین‌کنندگان", dbPermissions)
            AddActionNode(stSrmMain, PermissionKeys.SrmCosting, "🔘 محاسبات انحراف قیمت خرید (PPV) و پاداش کیفی", dbPermissions)

            tSrmMain.Children.Add(stSrmMain)
            smSrmMain.Children.Add(tSrmMain)
            rSrm.Children.Add(smSrmMain)

            Dim smSrmReports As New PermissionTreeNode("SM_SRM_REPORTS", "📁 گزارشات جامع تامین‌کنندگان، OTD و گریدبندی", 1)
            Dim tSrmReports As New PermissionTreeNode("T_SRM_REPORTS", "📄 گزارش‌های ارزیابی کیفی، تحویل به موقع و صرفه‌جویی استعلام‌ها", 2)
            Dim stSrmReports As New PermissionTreeNode("ST_SRM_REPORTS", "📑 گزارشات مدیریتی کارت امتیازی و ماتریس رتبه‌بندی تامین‌کنندگان", 3)

            AddActionNode(stSrmReports, PermissionKeys.SrmReports, "🔘 گزارشات جامع مدیریت تامین‌کنندگان و شاخص‌های SRM", dbPermissions)

            tSrmReports.Children.Add(stSrmReports)
            smSrmReports.Children.Add(tSrmReports)
            rSrm.Children.Add(smSrmReports)
            roots.Add(rSrm)

            ' =========================================================================
            ' 22. منوی اصلی: سیستم کنترل کیفیت و تضمین کیفیت (mQc)
            ' =========================================================================
            Dim rQc As New PermissionTreeNode("MENU_QC", "🔬 سیستم جامع کنترل کیفیت و تضمین کیفیت (QC/QA)", 0)
            Dim smQcMain As New PermissionTreeNode("SM_QC_MAIN", "📁 سیستم جامع کنترل و تضمین کیفیت", 1)
            Dim tQcMain As New PermissionTreeNode("T_QC_MAIN", "📄 بازرسی IQC، بازرسی حین تولید IPQC، محصولات نهایی OQC و عدم انطباق", 2)
            Dim stQcMain As New PermissionTreeNode("ST_QC_MAIN", "📑 صدور آنالیز فنی (CoA)، فرم‌های NCR، اقدامات اصلاحی (CAPA) و ضایعات", 3)

            AddActionNode(stQcMain, PermissionKeys.QcModule, "🔘 دسترسی به سیستم کنترل و تضمین کیفیت (QC/QA)", dbPermissions)
            AddActionNode(stQcMain, PermissionKeys.QcIqc, "🔘 مدیریت بازرسی کیفی ورودی انبار (IQC)", dbPermissions)
            AddActionNode(stQcMain, PermissionKeys.QcIpqc, "🔘 مدیریت بازرسی حین تولید و ایستگاه‌ها (IPQC)", dbPermissions)
            AddActionNode(stQcMain, PermissionKeys.QcNcrCapa, "🔘 مدیریت عدم انطباق‌ها (NCR) و اقدامات اصلاحی (CAPA)", dbPermissions)
            AddActionNode(stQcMain, PermissionKeys.QcCosting, "🔘 محاسبات بهای ضایعات کیفی و ثبت اسناد افت ارزش", dbPermissions)

            tQcMain.Children.Add(stQcMain)
            smQcMain.Children.Add(tQcMain)
            rQc.Children.Add(smQcMain)

            Dim smQcReports As New PermissionTreeNode("SM_QC_REPORTS", "📁 گزارشات جامع کیفیت، FPY، ضایعات و COQ", 1)
            Dim tQcReports As New PermissionTreeNode("T_QC_REPORTS", "📄 گزارش‌های نرخ عبور کیفی بار اول (FPY)، هزینه عدم کیفیت و تحلیل ضایعات", 2)
            Dim stQcReports As New PermissionTreeNode("ST_QC_REPORTS", "📑 گزارشات مدیریتی برگه‌های NCR و وضعیت اقدامات اصلاحی CAPA", 3)

            AddActionNode(stQcReports, PermissionKeys.QcReports, "🔘 گزارشات جامع کنترل کیفیت و شاخص‌های FPY/COQ", dbPermissions)

            tQcReports.Children.Add(stQcReports)
            smQcReports.Children.Add(tQcReports)
            rQc.Children.Add(smQcReports)

            roots.Add(rQc)

            ' =========================================================================
            ' 23. منوی اصلی: سیستم هوش تجاری و داشبورد مدیریتی پیشرفته (mBi)
            ' =========================================================================
            Dim rBi As New PermissionTreeNode("MENU_BI", "📊 سیستم جامع هوش تجاری و داشبورد مدیریتی پیشرفته (BI)", 0)
            Dim smBiMain As New PermissionTreeNode("SM_BI_MAIN", "📁 برج کنترل مدیرعامل و داشبوردهای تخصصی", 1)
            Dim tBiMain As New PermissionTreeNode("T_BI_MAIN", "📄 برج کنترل CEO، تحلیل سود و زیان P&L، پیش‌بینی هوشمند فروش و OEE", 2)
            Dim stBiMain As New PermissionTreeNode("ST_BI_MAIN", "📑 مکعب‌های داده (OLAP)، تحلیل چندبعدی، پیش‌بینی نقدینگی و ریسک", 3)

            AddActionNode(stBiMain, PermissionKeys.BiModule, "🔘 دسترسی به سیستم هوش تجاری و داشبورد مدیریتی (BI)", dbPermissions)
            AddActionNode(stBiMain, PermissionKeys.BiExecutive, "🔘 برج کنترل مدیرعامل (CEO Control Tower)", dbPermissions)
            AddActionNode(stBiMain, PermissionKeys.BiFinancial, "🔘 هوش تجاری مالی، P&L و سودآوری به تفکیک محصولات", dbPermissions)
            AddActionNode(stBiMain, PermissionKeys.BiProduction, "🔘 هوش تجاری تولید، بهره‌وری OEE و ضایعات FPY", dbPermissions)
            AddActionNode(stBiMain, PermissionKeys.BiSales, "🔘 پیش‌بینی هوشمند فروش و پایش فروش مویرگی", dbPermissions)

            tBiMain.Children.Add(stBiMain)
            smBiMain.Children.Add(tBiMain)
            rBi.Children.Add(smBiMain)

            Dim smBiReports As New PermissionTreeNode("SM_BI_REPORTS", "📁 گزارشات تحلیلی OLAP و پیش‌بینی‌های AI", 1)
            Dim tBiReports As New PermissionTreeNode("T_BI_REPORTS", "📄 گزارش‌های چندبعدی Drill-down، تحلیل انحراف بودجه و رتبه‌بندی", 2)
            Dim stBiReports As New PermissionTreeNode("ST_BI_REPORTS", "📑 گزارشات مدیریتی ارشد و ماتریس‌های هوش تجاری", 3)

            AddActionNode(stBiReports, PermissionKeys.BiReports, "🔘 گزارشات جامع تحلیلی OLAP، پیش‌بینی هوشمند و P&L", dbPermissions)

            tBiReports.Children.Add(stBiReports)
            smBiReports.Children.Add(tBiReports)
            rBi.Children.Add(smBiReports)
            roots.Add(rBi)

            ' =========================================================================
            ' 24. منوی اصلی: سیستم مدیریت بایگانی دیجیتال و آرشیو اسناد (mDms)
            ' =========================================================================
            Dim rDms As New PermissionTreeNode("MENU_DMS", "📁 سیستم جامع مدیریت بایگانی دیجیتال و آرشیو اسناد (DMS)", 0)
            Dim smDmsMain As New PermissionTreeNode("SM_DMS_MAIN", "📁 آرشیو اسناد، پرونده‌ها و متاداده‌گذاری", 1)
            Dim tDmsMain As New PermissionTreeNode("T_DMS_MAIN", "📄 اسکن مستقیم، OCR فارسی، متاداده‌گذاری، کنترل نسخه و رمزنگاری AES-256", 2)
            Dim stDmsMain As New PermissionTreeNode("ST_DMS_MAIN", "📑 بایگانی دیجیتال، قفل‌گذاری اسناد (Check-in/Out) و پیوست به مالی/پرسنلی", 3)

            AddActionNode(stDmsMain, PermissionKeys.DmsModule, "🔘 دسترسی به سیستم مدیریت بایگانی دیجیتال (DMS)", dbPermissions)
            AddActionNode(stDmsMain, PermissionKeys.DmsDocuments, "🔘 مدیریت پرونده‌ها، اسکن مدارک و متاداده‌گذاری", dbPermissions)
            AddActionNode(stDmsMain, PermissionKeys.DmsCategories, "🔘 مدیریت درختواره زون‌های بایگانی اسناد", dbPermissions)
            AddActionNode(stDmsMain, PermissionKeys.DmsVersioning, "🔘 مدیریت تاریخچه نسخه‌های اسناد (Version Control)", dbPermissions)
            AddActionNode(stDmsMain, PermissionKeys.DmsSecurity, "🔘 سطوح محرمانگی اسناد، رمزنگاری AES و واترمارک", dbPermissions)

            tDmsMain.Children.Add(stDmsMain)
            smDmsMain.Children.Add(tDmsMain)
            rDms.Children.Add(smDmsMain)

            Dim smDmsReports As New PermissionTreeNode("SM_DMS_REPORTS", "📁 گزارشات بایگانی، سررسید اسناد و لاگ امنیتی", 1)
            Dim tDmsReports As New PermissionTreeNode("T_DMS_REPORTS", "📄 موتور جستجوی متن کامل (Full-Text)، هشدار انقضای قراردادها و ردیابی audit", 2)
            Dim stDmsReports As New PermissionTreeNode("ST_DMS_REPORTS", "📑 گزارشات مدیریتی آرشیو دیجیتال و وضعیت امنیت اسناد", 3)

            AddActionNode(stDmsReports, PermissionKeys.DmsReports, "🔘 گزارشات جامع پرونده‌های آرشیو، سررسید انقضا و لاگ امنیتی", dbPermissions)

            tDmsReports.Children.Add(stDmsReports)
            smDmsReports.Children.Add(tDmsReports)
            rDms.Children.Add(smDmsReports)

            roots.Add(rDms)

            ' =========================================================================
            ' 25. منوی اصلی: سیستم امور سهام و سهامداران (mSaham)
            ' =========================================================================
            Dim rSaham As New PermissionTreeNode("MENU_SAHAM", "🏛️ سیستم جامع امور سهام و سهامداران", 0)
            Dim smSahamMain As New PermissionTreeNode("SM_SAHAM_MAIN", "📁 دفتر سهام، نقل و انتقالات و مجمع عمومی", 1)
            Dim tSahamMain As New PermissionTreeNode("T_SAHAM_MAIN", "📄 دفتر ثبت سهامداران، نقل و انتقال سهام، سود مصوب DPS و افزایش سرمایه", 2)
            Dim stSahamMain As New PermissionTreeNode("ST_SAHAM_MAIN", "📑 صدور ورقه سهم، پرداخت سود پایا، حق تقدم و صدور خودکار اسناد حسابداری", 3)

            AddActionNode(stSahamMain, PermissionKeys.SahamModule, "🔘 دسترسی به سیستم امور سهام و سهامداران", dbPermissions)
            AddActionNode(stSahamMain, PermissionKeys.SahamRegister, "🔘 مدیریت دفتر ثبت سهامداران و صدور ورقه سهم", dbPermissions)
            AddActionNode(stSahamMain, PermissionKeys.SahamTransfers, "🔘 مدیریت نقل و انتقال سهام و معاملات حقوقی", dbPermissions)
            AddActionNode(stSahamMain, PermissionKeys.SahamDividends, "🔘 محاسبه و تقسیم سود مصوب مجمع (DPS) و فایل واریز بانکی", dbPermissions)
            AddActionNode(stSahamMain, PermissionKeys.SahamCapital, "🔘 مدیریت افزایش سرمایه و گواهی حق تقدم", dbPermissions)

            tSahamMain.Children.Add(stSahamMain)
            smSahamMain.Children.Add(tSahamMain)
            rSaham.Children.Add(smSahamMain)

            Dim smSahamReports As New PermissionTreeNode("SM_SAHAM_REPORTS", "📁 گزارشات دفتر سهام، مجمع و واریز سود", 1)
            Dim tSahamReports As New PermissionTreeNode("T_SAHAM_REPORTS", "📄 گزارش‌های ترکیب سهامداران، سود تقسیم‌شده DPS و حدنصاب مجمع", 2)
            Dim stSahamReports As New PermissionTreeNode("ST_SAHAM_REPORTS", "📑 گزارشات مدیریتی سهامداران عمده و گردش مطالبات سود", 3)

            AddActionNode(stSahamReports, PermissionKeys.SahamReports, "🔘 گزارشات جامع ترکیب سهامداران، مجمع عمومی و پرداخت سود", dbPermissions)

            tSahamReports.Children.Add(stSahamReports)
            smSahamReports.Children.Add(tSahamReports)
            rSaham.Children.Add(smSahamReports)
            roots.Add(rSaham)

            ' =========================================================================
            ' 26. منوی اصلی: سیستم وب‌سرویس و API فروشگاه اینترنتی و پوز سیار (mApi)
            ' =========================================================================
            Dim rApi As New PermissionTreeNode("MENU_API", "🌐 سیستم جامع وب‌سرویس و API فروشگاه اینترنتی و صندوق سیار", 0)
            Dim smApiMain As New PermissionTreeNode("SM_API_MAIN", "📁 کلیدهای API، سفارشات آنلاین و پوز سیار", 1)
            Dim tApiMain As New PermissionTreeNode("T_API_MAIN", "📄 همگام‌سازی زنده کالاها، صدور خودکار فاکتور فروش و کارتخوان اندرویدی", 2)
            Dim stApiMain As New PermissionTreeNode("ST_API_MAIN", "📑 کلیدهای امنیتی JWT، شبیه‌سازی تست سفارشات آنلاین و وب‌هوکس", 3)

            AddActionNode(stApiMain, PermissionKeys.ApiModule, "🔘 دسترسی به سیستم وب‌سرویس و API نگار", dbPermissions)
            AddActionNode(stApiMain, PermissionKeys.ApiProductsSync, "🔘 همگام‌سازی زنده کالاها، موجودی انبار و قیمت‌ها", dbPermissions)
            AddActionNode(stApiMain, PermissionKeys.ApiOrders, "🔘 دریافت سفارشات آنلاین و صدور خودکار فاکتور فروش", dbPermissions)
            AddActionNode(stApiMain, PermissionKeys.ApiMobilePos, "🔘 تراکنش‌های صندوق سیار و کارتخوان‌های اندرویدی (Android POS)", dbPermissions)
            AddActionNode(stApiMain, PermissionKeys.ApiWebhooks, "🔘 مدیریت رویدادهای زنده (Webhooks) و توکن‌های JWT", dbPermissions)

            tApiMain.Children.Add(stApiMain)
            smApiMain.Children.Add(tApiMain)
            rApi.Children.Add(smApiMain)

            Dim smApiReports As New PermissionTreeNode("SM_API_REPORTS", "📁 مانیتورینگ ترافیک، لاگ وب‌سرویس و تحلیل Omnichannel", 1)
            Dim tApiReports As New PermissionTreeNode("T_API_REPORTS", "📄 مانیتورینگ زنده ترافیک HTTP، زمان پاسخ‌دهی (Latency) و فروش چندهسته", 2)
            Dim stApiReports As New PermissionTreeNode("ST_API_REPORTS", "📑 گزارشات مدیریتی لاگ خطاها و تحلیلی فروش حضوری/آنلاین", 3)

            AddActionNode(stApiReports, PermissionKeys.ApiReports, "🔘 گزارشات جامع لاگ‌های API، مانیتورینگ ترافیک و فروش Omnichannel", dbPermissions)

            tApiReports.Children.Add(stApiReports)
            smApiReports.Children.Add(tApiReports)
            rApi.Children.Add(smApiReports)
            roots.Add(rApi)

            ' =========================================================================
            ' 27. منوی اصلی: سیستم مدیریت امور حقوقی، قراردادها و دعاوی (mLegal)
            ' =========================================================================
            Dim rLegal As New PermissionTreeNode("MENU_LEGAL", "⚖️ سیستم جامع مدیریت امور حقوقی، قراردادها و دعاوی", 0)
            Dim smLegalMain As New PermissionTreeNode("SM_LEGAL_MAIN", "📁 پرونده‌های قضایی، دادخواست‌ها و جلسات دادگاه", 1)
            Dim tLegalMain As New PermissionTreeNode("T_LEGAL_MAIN", "📄 پرونده‌های حقوقی/کیفری، زمان‌بندی دادگاه، لوایح و وکالت‌نامه‌ها", 2)
            Dim stLegalMain As New PermissionTreeNode("ST_LEGAL_MAIN", "📑 تشکیل پرونده، هشدار انقضای مهلت‌های قانونی، مدیریت وکلا و حق‌الوکاله", 3)

            AddActionNode(stLegalMain, PermissionKeys.LegalModule, "🔘 دسترسی به سیستم امور حقوقی و دعاوی نگار", dbPermissions)
            AddActionNode(stLegalMain, PermissionKeys.LegalCases, "🔘 مدیریت شناسنامه پرونده‌های حقوقی و دعاوی قضایی", dbPermissions)
            AddActionNode(stLegalMain, PermissionKeys.LegalHearings, "🔘 تقویم جلسات دادگاه و مهلت‌های تجدیدنظرخواهی", dbPermissions)
            AddActionNode(stLegalMain, PermissionKeys.LegalLawyers, "🔘 مدیریت وکلا، کارشناسان رسمی و حق‌الوکاله‌ها", dbPermissions)
            AddActionNode(stLegalMain, PermissionKeys.LegalDocuments, "🔘 آرشیو دادخواست‌ها، اظهارنامه‌ها و لوایح دفاعیه", dbPermissions)

            tLegalMain.Children.Add(stLegalMain)
            smLegalMain.Children.Add(tLegalMain)
            rLegal.Children.Add(smLegalMain)

            Dim smLegalReports As New PermissionTreeNode("SM_LEGAL_REPORTS", "📁 گزارشات ریسک دادرسی، پرونده‌ها و هزینه‌ها", 1)
            Dim tLegalReports As New PermissionTreeNode("T_LEGAL_REPORTS", "📄 گزارش ارزیابی ریسک مالی پرونده‌ها (Financial Risk Analysis) و بردهای حقوقی", 2)
            Dim stLegalReports As New PermissionTreeNode("ST_LEGAL_REPORTS", "📑 گزارشات مدیریتی پرونده‌های در جریان و هزینه‌های دادرسی", 3)

            AddActionNode(stLegalReports, PermissionKeys.LegalReports, "🔘 گزارشات جامع پرونده‌های حقوقی، جلسات دادگاه و ریسک دادرسی", dbPermissions)

            tLegalReports.Children.Add(stLegalReports)
            smLegalReports.Children.Add(tLegalReports)
            rLegal.Children.Add(smLegalReports)

            roots.Add(rLegal)

            ' =========================================================================
            ' 28. منوی اصلی: سیستم مدیریت تحقیق و توسعه و فرمولاسیون (mRd)
            ' =========================================================================
            Dim rRd As New PermissionTreeNode("MENU_RD", "🔬 سیستم جامع مدیریت تحقیق و توسعه و فرمولاسیون (R&D)", 0)

            Dim smRdMain As New PermissionTreeNode("SM_RD_MAIN", "📁 پروژه‌های NPD، فرمولاسیون، آزمایشگاه و پتنت", 1)
            Dim tRdMain As New PermissionTreeNode("T_RD_MAIN", "📄 پروژه‌های Stage-Gate، BOM پژوهشی، لاگ آزمایشگاه و ثبت پتنت", 2)
            Dim stRdMain As New PermissionTreeNode("ST_RD_MAIN", "📑 مدیریت کامل چرخه عمر نوآوری از ایده تا تجاری‌سازی", 3)

            AddActionNode(stRdMain, PermissionKeys.RdModule, "🔘 دسترسی به سیستم تحقیق و توسعه نگار (R&D)", dbPermissions)
            AddActionNode(stRdMain, PermissionKeys.RdProjects, "🔘 مدیریت پروژه‌های NPD با Stage-Gate و Gate Reviews", dbPermissions)
            AddActionNode(stRdMain, PermissionKeys.RdFormulations, "🔘 فرمولاسیون محصول، BOM پژوهشی و Version Control فرمول", dbPermissions)
            AddActionNode(stRdMain, PermissionKeys.RdLabTests, "🔘 لاگ آزمایشگاهی، نمونه‌های اولیه و نتایج Pilot Test", dbPermissions)
            AddActionNode(stRdMain, PermissionKeys.RdPatents, "🔘 مدیریت پتنت‌ها، اختراعات و مالکیت فکری (IPR)", dbPermissions)
            AddActionNode(stRdMain, PermissionKeys.RdBudget, "🔘 بودجه پروژه‌های R&D و محاسبه ROI تحقیقات", dbPermissions)

            tRdMain.Children.Add(stRdMain)
            smRdMain.Children.Add(tRdMain)
            rRd.Children.Add(smRdMain)

            Dim smRdReports As New PermissionTreeNode("SM_RD_REPORTS", "📁 گزارشات Innovation Funnel، ROI تحقیقات و IPR", 1)
            Dim tRdReports As New PermissionTreeNode("T_RD_REPORTS", "📄 گزارش Time-to-Market، قیف نوآوری و ارزیابی ROI", 2)
            Dim stRdReports As New PermissionTreeNode("ST_RD_REPORTS", "📑 گزارشات مدیریتی عملکرد بودجه R&D و پتنت‌های شرکت", 3)

            AddActionNode(stRdReports, PermissionKeys.RdReports, "🔘 گزارشات جامع Innovation Funnel، Time-to-Market و ROI پژوهش", dbPermissions)

            tRdReports.Children.Add(stRdReports)
            smRdReports.Children.Add(tRdReports)
            rRd.Children.Add(smRdReports)

            roots.Add(rRd)

            ' =========================================================================
            ' 29. منوی اصلی: سیستم یکپارچه‌سازی مرکز تلفن هوشمند و CRM صوتی (mVoip)
            ' =========================================================================
            Dim rVoip As New PermissionTreeNode("MENU_VOIP", "📞 سیستم یکپارچه‌سازی مرکز تلفن هوشمند و CRM صوتی (VoIP)", 0)

            Dim smVoipMain As New PermissionTreeNode("SM_VOIP_MAIN", "📁 لاگ تماس‌ها، صف ACD، ضبط مکالمه و کمپین خروجی", 1)
            Dim tVoipMain As New PermissionTreeNode("T_VOIP_MAIN", "📄 Screen Pop-Up مشتری، Sticky Agent، Click-to-Call و داشبورد Real-Time", 2)
            Dim stVoipMain As New PermissionTreeNode("ST_VOIP_MAIN", "📑 مدیریت کامل چرخه تماس از Screen Pop-Up تا آرشیو صوتی در DMS", 3)

            AddActionNode(stVoipMain, PermissionKeys.VoipModule, "🔘 دسترسی به سیستم مرکز تلفن هوشمند نگار (VoIP CRM)", dbPermissions)
            AddActionNode(stVoipMain, PermissionKeys.VoipCallLogs, "🔘 لاگ تماس‌های ورودی/خروجی، نتیجه تماس و یادداشت CRM", dbPermissions)
            AddActionNode(stVoipMain, PermissionKeys.VoipQueue, "🔘 مدیریت صف تماس (ACD)، Sticky Agent و داشبورد Real-Time اپراتورها", dbPermissions)
            AddActionNode(stVoipMain, PermissionKeys.VoipRecordings, "🔘 ضبط مکالمه، آرشیو صوتی در DMS و جستجوی متنی (STT)", dbPermissions)
            AddActionNode(stVoipMain, PermissionKeys.VoipCampaigns, "🔘 مدیریت کمپین‌های تماس خروجی، Preview Dial و Click-to-Call", dbPermissions)
            AddActionNode(stVoipMain, PermissionKeys.VoipSettings, "🔘 پیکربندی Asterisk AMI، نگاشت داخلی‌ها و قوانین مسیریابی", dbPermissions)

            tVoipMain.Children.Add(stVoipMain)
            smVoipMain.Children.Add(tVoipMain)
            rVoip.Children.Add(smVoipMain)

            Dim smVoipReports As New PermissionTreeNode("SM_VOIP_REPORTS", "📁 گزارشات KPI مرکز تماس — Answer Rate، ASA، CSAT و نرخ تبدیل", 1)
            Dim tVoipReports As New PermissionTreeNode("T_VOIP_REPORTS", "📄 گزارش عملکرد اپراتورها، تحلیل صف و نرخ تبدیل تماس به فروش", 2)
            Dim stVoipReports As New PermissionTreeNode("ST_VOIP_REPORTS", "📑 داشبورد BI تماس‌ها — هزینه هر تماس، مدت مکالمه و رضایت CSAT", 3)

            AddActionNode(stVoipReports, PermissionKeys.VoipReports, "🔘 گزارشات جامع KPI مرکز تماس، عملکرد اپراتور و نرخ تبدیل فروش", dbPermissions)

            tVoipReports.Children.Add(stVoipReports)
            smVoipReports.Children.Add(tVoipReports)
            rVoip.Children.Add(smVoipReports)

            roots.Add(rVoip)

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
