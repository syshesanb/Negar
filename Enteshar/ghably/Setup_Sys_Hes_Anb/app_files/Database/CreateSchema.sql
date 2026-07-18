CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    [Password] TEXT NOT NULL,
    UserType TEXT NOT NULL,
    CreatedBy INTEGER,
    CreatedDate DATETIME,
    IsActive BOOLEAN,
    FullName TEXT,
    CreatorIP TEXT,
    MaxCompaniesAllowed INTEGER DEFAULT 0,
    MaxFiscalYearsPerCompany INTEGER DEFAULT 0
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Username ON Users (Username);

CREATE TABLE Permissions (
    PermissionID INTEGER PRIMARY KEY AUTOINCREMENT,
    PermissionName TEXT NOT NULL,
    PermissionKey TEXT NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Permissions_Key ON Permissions (PermissionKey);

CREATE TABLE RolePermissions (
    RolePermID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    PermissionID INTEGER NOT NULL,
    CanView BOOLEAN,
    CanCreate BOOLEAN,
    CanEdit BOOLEAN,
    CanDelete BOOLEAN,
    CanPrint BOOLEAN,
    CanExport BOOLEAN
);

CREATE TABLE Companies (
    CompanyID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyName TEXT,
    CompanyCode TEXT,
    BrandName TEXT,
    EconomicCode TEXT,
    FiscalYearStartDate DATETIME,
    FiscalYearEndDate DATETIME,
    PostalCode TEXT,
    RegistrationDate DATETIME,
    RegistrationNumber TEXT,
    ActivityField TEXT,
    Address TEXT,
    Phone TEXT,
    Phone2 TEXT,
    Email TEXT,
    TaxID TEXT,
    LogoImage BLOB,
    ChairmanName TEXT,
    InspectorName TEXT,
    CEOName TEXT,
    Signatory1Title TEXT,
    Signatory1Name TEXT,
    Signatory2Title TEXT,
    Signatory2Name TEXT,
    Signatory3Title TEXT,
    Signatory3Name TEXT,
    Signatory4Title TEXT,
    Signatory4Name TEXT,
    OwnerUserID INTEGER,
    AccountLevels TEXT,
    Level1Length INTEGER,
    Level2Length INTEGER,
    Level3Length INTEGER,
    Level4Length INTEGER,
    Level5Length INTEGER,
    IsActive BOOLEAN
);

CREATE TABLE FiscalYears (
    FiscalYearID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyID INTEGER NOT NULL,
    FiscalYearName TEXT NOT NULL,
    StartDate DATETIME,
    EndDate DATETIME,
    IsActive BOOLEAN
);

CREATE TABLE Products (
    ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductCode TEXT NOT NULL,
    ProductName TEXT NOT NULL,
    Unit TEXT,
    DefaultPrice DECIMAL,
    Category TEXT,
    IsActive BOOLEAN
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Products_Code ON Products (ProductCode);

CREATE TABLE Warehouses (
    WarehouseID INTEGER PRIMARY KEY AUTOINCREMENT,
    WarehouseName TEXT NOT NULL,
    Location TEXT,
    IsActive BOOLEAN
);

CREATE TABLE Inventory (
    InventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductID INTEGER NOT NULL,
    WarehouseID INTEGER NOT NULL,
    Quantity REAL,
    AverageCost DECIMAL,
    LastUpdate DATETIME
);

CREATE TABLE PurchaseInvoices (
    InvoiceID INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceNumber TEXT NOT NULL,
    InvoiceDate DATETIME,
    VendorName TEXT,
    TotalAmount DECIMAL,
    CreatedBy INTEGER,
    WarehouseID INTEGER
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_PurchaseInvoices_Number ON PurchaseInvoices (InvoiceNumber);

CREATE TABLE PurchaseInvoiceDetails (
    DetailID INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceID INTEGER NOT NULL,
    ProductID INTEGER NOT NULL,
    Quantity REAL,
    UnitPrice DECIMAL,
    TotalPrice DECIMAL
);

CREATE TABLE SalesInvoices (
    InvoiceID INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceNumber TEXT NOT NULL,
    InvoiceDate DATETIME,
    CustomerName TEXT,
    TotalAmount DECIMAL,
    CreatedBy INTEGER,
    WarehouseID INTEGER
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_SalesInvoices_Number ON SalesInvoices (InvoiceNumber);

CREATE TABLE SalesInvoiceDetails (
    DetailID INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceID INTEGER NOT NULL,
    ProductID INTEGER NOT NULL,
    Quantity REAL,
    UnitPrice DECIMAL,
    TotalPrice DECIMAL,
    CostAtSaleTime DECIMAL
);

CREATE TABLE SarfaslHesab (
    AccountID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyID INTEGER NOT NULL,
    AccountCode TEXT NOT NULL,
    AccountName TEXT NOT NULL,
    AccountType TEXT NOT NULL,
    ParentAccountID INTEGER,
    IsActive BOOLEAN,
    AccountNature TEXT
);

CREATE INDEX IF NOT EXISTS IX_SarfaslHesab_CompanyCode ON SarfaslHesab (CompanyID, AccountCode);

CREATE TABLE SarfaslShenavar (
    ShenavarID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyID INTEGER NOT NULL,
    AccountCode TEXT NOT NULL,
    AccountName TEXT NOT NULL,
    ParentShenavarID INTEGER,
    IsActive BOOLEAN
);

CREATE TABLE Sanad1 (
    EntryID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyID INTEGER NOT NULL,
    FiscalYearID INTEGER NOT NULL,
    EntryDate DATETIME,
    Description TEXT,
    ReferenceNumber TEXT,
    CreatedBy INTEGER,
    JamBedehkar DECIMAL,
    JamBestankar DECIMAL,
    TaeazSanad TEXT,
    SharhSanad TEXT,
    VazeiatSanad TEXT,
    AdamVirayesh BOOLEAN
);

CREATE TABLE Sanad2 (
    DetailID INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryID INTEGER NOT NULL,
    AccountID INTEGER NOT NULL,
    DebitAmount REAL,
    CreditAmount REAL,
    LineNumber INTEGER,
    ShenavarID INTEGER,
    SharhRadif TEXT,
    TransactionNumber TEXT,
    TransactionDate TEXT
);

CREATE TABLE SavabegEditSanad1 (
    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryID INTEGER NOT NULL,
    EditDate DATETIME NOT NULL,
    UserID INTEGER NOT NULL,
    EditDescription TEXT
);

CREATE TABLE ActivityLog (
    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    ActivityType TEXT NOT NULL,
    EntityType TEXT,
    EntityID INTEGER,
    Description TEXT,
    IPAddress TEXT,
    ActivityDate DATETIME
);

CREATE TABLE AppSettings (
    SettingID INTEGER PRIMARY KEY AUTOINCREMENT,
    SettingKey TEXT NOT NULL,
    SettingValue TEXT,
    SettingCategory TEXT
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_AppSettings_Key ON AppSettings (SettingKey);

CREATE TABLE EntryNotes (
    NoteID INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryID INTEGER NOT NULL,
    LineNumber INTEGER,
    NoteText TEXT,
    UserID INTEGER NOT NULL,
    CreatedDate DATETIME,
    ModifiedDate DATETIME
);

CREATE TABLE EntryLineAttachments (
    AttachmentID INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryID INTEGER NOT NULL,
    LineNumber INTEGER NOT NULL,
    FileName TEXT,
    ImageData BLOB,
    FileSize INTEGER,
    AddedDate DATETIME
);
