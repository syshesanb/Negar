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
    PermissionKey TEXT NOT NULL,
    SectionName TEXT
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
    ProductGroupLevels INTEGER DEFAULT 3,
    LogoPosition TEXT,
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
    IsActive BOOLEAN,
    BaseUoMID INTEGER,
    IsCatchWeight BOOLEAN DEFAULT 0,
    SecondaryUoMID INTEGER,
    NominalFactor DECIMAL,
    ProductGroupID INTEGER,
    Barcode TEXT,
    TaxID TEXT,
    ProductType TEXT DEFAULT 'کالا',
    PurchasePrice DECIMAL DEFAULT 0,
    MinStock DECIMAL DEFAULT 0,
    ReorderPoint DECIMAL DEFAULT 0,
    MaxStock DECIMAL DEFAULT 0,
    TrackingType TEXT DEFAULT 'عادی',
    LocationID INTEGER,
    TechnicalName TEXT,
    ConsumerMarkup DECIMAL DEFAULT 0,
    ConsumerDiscount DECIMAL DEFAULT 0,
    ColleagueMarkup DECIMAL DEFAULT 0,
    ColleagueDiscount DECIMAL DEFAULT 0,
    WholesaleMarkup DECIMAL DEFAULT 0,
    WholesaleDiscount DECIMAL DEFAULT 0,
    TaxPercent DECIMAL DEFAULT 0,
    TollPercent DECIMAL DEFAULT 0,
    NetWeight DECIMAL DEFAULT 0,
    GrossWeight DECIMAL DEFAULT 0,
    Length DECIMAL DEFAULT 0,
    Width DECIMAL DEFAULT 0,
    Height DECIMAL DEFAULT 0,
    Volume DECIMAL DEFAULT 0,
    Color TEXT,
    Material TEXT,
    Size TEXT,
    Brand TEXT,
    CountryOfOrigin TEXT,
    PhysicalDescription TEXT,
    Image1 TEXT,
    Image2 TEXT,
    Image3 TEXT,
    Image4 TEXT,
    Image5 TEXT,
    Image6 TEXT
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Products_Code ON Products (ProductCode);
CREATE INDEX IF NOT EXISTS IX_Products_GroupID ON Products (ProductGroupID);

CREATE TABLE WarehouseTypes (
    TypeID INTEGER PRIMARY KEY AUTOINCREMENT,
    TypeName TEXT UNIQUE NOT NULL
);

CREATE TABLE Warehouses (
    WarehouseID INTEGER PRIMARY KEY AUTOINCREMENT,
    WarehouseName TEXT NOT NULL,
    Location TEXT,
    IsActive INTEGER DEFAULT 1,
    WarehouseType TEXT,
    Phone TEXT,
    Phone2 TEXT,
    Phone3 TEXT,
    PostalCode TEXT,
    Capacity REAL,
    WarehouseKeeper TEXT,
    CostCenter TEXT,
    AllowNegativeStock BOOLEAN,
    Description TEXT
);

CREATE TABLE WarehouseLocations (
    LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
    WarehouseID INTEGER NOT NULL,
    ParentID INTEGER,
    LocationType INTEGER NOT NULL, -- 1:Salon, 2:Section, 3:Aisle, 4:Shelf, 5:Row, 6:Box
    Title TEXT NOT NULL,
    Code TEXT NOT NULL,
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE CASCADE,
    FOREIGN KEY (ParentID) REFERENCES WarehouseLocations(LocationID) ON DELETE CASCADE
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

CREATE TABLE uom_categories (
    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE uoms (
    UoMID INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryID INTEGER NOT NULL,
    UoMName TEXT NOT NULL,
    Abbreviation TEXT,
    IsReferenceUoM BOOLEAN DEFAULT 0,
    ConversionNumerator INTEGER DEFAULT 1,
    ConversionDenominator INTEGER DEFAULT 1,
    IsActive BOOLEAN NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoryID) REFERENCES uom_categories(CategoryID)
);

CREATE TABLE product_uom_conversions (
    ConversionID INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductID INTEGER NOT NULL,
    FromUoMID INTEGER NOT NULL,
    ToUoMID INTEGER NOT NULL,
    ConversionNumerator INTEGER NOT NULL,
    ConversionDenominator INTEGER NOT NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (FromUoMID) REFERENCES uoms(UoMID),
    FOREIGN KEY (ToUoMID) REFERENCES uoms(UoMID),
    CONSTRAINT uq_product_uom UNIQUE(ProductID, FromUoMID, ToUoMID)
);

CREATE TABLE ProductGroups (
    GroupID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyID INTEGER NOT NULL,
    ParentID INTEGER NULL,
    GroupCode TEXT NOT NULL,
    GroupName TEXT NOT NULL,
    Level INTEGER NOT NULL,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID) ON DELETE CASCADE,
    FOREIGN KEY (ParentID) REFERENCES ProductGroups(GroupID) ON DELETE CASCADE
);

CREATE INDEX idx_productgroups_company ON ProductGroups(CompanyID);
CREATE INDEX idx_productgroups_parent ON ProductGroups(ParentID);
CREATE INDEX idx_productgroups_code ON ProductGroups(GroupCode);

CREATE TABLE IF NOT EXISTS Personnel (
    PersonnelID INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    Role TEXT,
    NationalCode TEXT,
    Phone TEXT,
    Department INTEGER DEFAULT 1, -- 1: All, 2: Accounting, 3: Warehousing
    IsActive INTEGER DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
