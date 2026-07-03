# خلاصه پروژه Sys_Hes_Anb

## معرفی کلی

**Sys_Hes_Anb** یک نرم‌افزار دسکتاپ مدیریت کسب‌وکار است که با **VB.NET / Windows Forms** نوشته شده و از پایگاه داده **Microsoft Access (.accdb)** استفاده می‌کند.

این سیستم سه ماژول اصلی دارد:
1. **انبارداری** — خرید، فروش، موجودی کالا
2. **حسابداری** — سرفصل حساب‌ها، اسناد حسابداری، تراز آزمایشی
3. **مدیریت سیستم** — کاربران، دسترسی‌ها، شرکت‌ها، سال مالی، تنظیمات

---

## ساختار پوشه‌ها

```
Sys_Hes_Anb/
├── Program.vb                  — نقطه ورود برنامه
├── Models/                     — مدل‌های داده
├── Business/                   — منطق کسب‌وکار (Services)
├── Data/                       — لایه دسترسی به داده (Db, Sql, DbBootstrap)
├── Forms/
│   ├── HasteAsly/              — فرم اصلی، لاگین، تنظیمات، مدیریت کاربران
│   ├── Anbardary/              — فرم‌های انبارداری
│   ├── Hesabdary/              — فرم‌های حسابداری
│   └── Moshtarak/              — گزارش‌ها، شرکت/سال مالی
└── Database/
    └── CreateSchema.sql        — اسکریپت ایجاد جداول
```

---

## جریان اجرا

```
Main() → DbBootstrap.EnsureSeedData() → LoginForm → MainForm
```

- هنگام اجرا، پوشه `Database` ساخته می‌شود و داده‌های اولیه (Seed) در پایگاه داده درج می‌شوند.
- کاربر ابتدا با `LoginForm` وارد سیستم می‌شود.
- پس از احراز هویت، `MainForm` باز می‌شود و منوها بر اساس نقش و دسترسی کاربر نمایش داده می‌شوند.

---

## مدل‌های داده (Models)

| فایل | توضیح |
|---|---|
| `UserAccount.vb` | اطلاعات کاربر (شناسه، نام کاربری، نوع، وضعیت) |
| `ProductItem.vb` | کالا (کد، نام، واحد، قیمت پیش‌فرض، دسته‌بندی) |
| `WarehouseItem.vb` | انبار (نام، موقعیت) |
| `InventoryRecord.vb` | موجودی (کالا، انبار، مقدار، میانگین بهای تمام‌شده) |
| `AccountingAccount.vb` | سرفصل حساب‌ها (کد، نام، نوع، حساب والد) |
| `AccountingEntry.vb` | سند حسابداری |

---

## جداول پایگاه داده

| جدول | توضیح |
|---|---|
| `Users` | کاربران سیستم |
| `Permissions` | تعریف دسترسی‌ها |
| `RolePermissions` | دسترسی هر کاربر (View/Create/Edit/Delete/Print/Export) |
| `Companies` | شرکت‌ها |
| `FiscalYears` | سال‌های مالی هر شرکت |
| `Products` | کالاها |
| `Warehouses` | انبارها |
| `Inventory` | موجودی کالا در انبار |
| `PurchaseInvoices` / `PurchaseInvoiceDetails` | فاکتورهای خرید |
| `SalesInvoices` / `SalesInvoiceDetails` | فاکتورهای فروش |
| `ChartOfAccounts` | سرفصل حساب‌ها |
| `AccountingEntries` / `AccountingEntryDetails` | اسناد حسابداری (دوطرفه) |
| `AppSettings` | تنظیمات برنامه |

---

## سرویس‌های Business

| سرویس | وظیفه |
|---|---|
| `SecurityService` | احراز هویت کاربر |
| `UserService` | مدیریت کاربران |
| `PasswordHasher` | هش کردن رمز عبور |
| `SessionContext` | نگهداری اطلاعات جلسه جاری (کاربر، دسترسی‌ها، تم) |
| `PermissionKeys` | ثابت‌های کلید دسترسی |
| `CatalogService` | مدیریت کالاها و انبارها |
| `InventoryService` | محاسبه موجودی و میانگین بهای تمام‌شده |
| `InvoiceService` | ثبت فاکتورهای خرید و فروش |
| `AccountingService` | سرفصل حساب‌ها، اسناد، تراز آزمایشی |
| `CompanyFiscalYearService` | مدیریت شرکت‌ها و سال‌های مالی |
| `SettingsService` | خواندن/نوشتن تنظیمات برنامه |

---

## سیستم دسترسی (Permissions)

سه نوع کاربر وجود دارد:
- **SuperAdmin** — دسترسی کامل به همه بخش‌ها
- **Manager** — دسترسی به مدیریت کاربران پایه
- **سایر کاربران** — دسترسی بر اساس دسترسی‌های تعریف‌شده در جدول `RolePermissions`

کلیدهای دسترسی:

| کلید | بخش |
|---|---|
| `ManageUsers` | مدیریت کاربران |
| `ManageBasicUsers` | مدیریت کاربران پایه |
| `ManageProducts` | مدیریت کالاها |
| `ManageWarehouses` | مدیریت انبارها |
| `ManagePurchases` | خرید |
| `ManageSales` | فروش |
| `ViewInventory` | مشاهده موجودی |
| `ManageAccounting` | حسابداری |
| `ManageCompaniesYears` | شرکت‌ها و سال مالی |
| `ViewReports` | گزارش‌ها |
| `ManageSettings` | تنظیمات |

---

## تم (Theme)

سه تم قابل انتخاب است: **Default** (روشن)، **Dark** (تیره)، **Blue** (آبی).  
تم در `SessionContext.CurrentTheme` ذخیره و هنگام باز شدن `MainForm` اعمال می‌شود.

---

## فناوری‌ها

| مورد | مقدار |
|---|---|
| زبان | VB.NET |
| فریم‌ورک | .NET Framework (WinForms) |
| پایگاه داده | Microsoft Access (.accdb) |
| IDE | Visual Studio 2015 (v14) |
| نوع برنامه | دسکتاپ تک‌کاربره / شبکه محلی |
