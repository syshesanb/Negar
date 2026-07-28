Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Data
    Public Module DbBootstrap
        Private Sub EnsureTemFormTableAndSeed()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS TemForm (" &
                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ThemeName TEXT, " &
                    "ThemeColor TEXT, " &
                    "ThemeImage BLOB);")

                Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM TemForm"), 0))
                Sql.ExecuteNonQuery("DELETE FROM TemForm") ' Force regeneration for new graphics
                Dim sampleThemes = New Tuple(Of String, String)() {
                    Tuple.Create("تم کرم ملایم", "#FDF5E6"),
                    Tuple.Create("تم گل‌بهی ملایم", "#FFDAB9"),
                    Tuple.Create("تم نارنجی ملایم", "#FFE4B5"),
                    Tuple.Create("تم صورتی ملایم", "#FFE4E1"),
                    Tuple.Create("تم سبز ملایم", "#E0F8D8"),
                    Tuple.Create("تم آبی ملایم", "#D2E8F7"),
                    Tuple.Create("تم طوسی ویندوز (کلاسیک)", "#F0F0F0"),
                    Tuple.Create("تم ویندوز سون", "#D4E6F1")
                }
                
                For Each theme In sampleThemes
                    Dim name = theme.Item1
                    Dim colorStr = theme.Item2
                    Dim col = ColorTranslator.FromHtml(colorStr)
                    Dim imgBytes As Byte() = Nothing
                    
                    Using bmp As New Bitmap(400, 300)
                        Using g As Graphics = Graphics.FromImage(bmp)
                            ' Background
                            g.Clear(col)
                            
                            ' Title Bar
                            Dim darkCol = ControlPaint.Dark(ControlPaint.Dark(col))
                            Using titleBrush As New SolidBrush(darkCol)
                                g.FillRectangle(titleBrush, 0, 0, 400, 30)
                            End Using
                            
                            ' Window Title Text
                            Using f As New Font("Tahoma", 9, FontStyle.Bold)
                                g.DrawString("فرم نمونه", f, Brushes.White, 320, 5)
                            End Using
                            
                            ' Window Buttons (Close, Max, Min)
                            g.FillRectangle(Brushes.Red, 10, 5, 40, 20)
                            g.FillRectangle(Brushes.LightGray, 55, 5, 20, 20)
                            g.FillRectangle(Brushes.LightGray, 80, 5, 20, 20)
                            
                            ' Menu Bar
                            Using menuBrush As New SolidBrush(Color.FromArgb(200, 255, 255, 255))
                                g.FillRectangle(menuBrush, 0, 30, 400, 25)
                            End Using
                            Using f As New Font("Tahoma", 8)
                                g.DrawString("پرونده   ویرایش   امکانات   راهنما", f, Brushes.Black, 200, 35)
                            End Using
                            
                            ' Toolbar
                            Using toolBrush As New SolidBrush(Color.FromArgb(150, 255, 255, 255))
                                g.FillRectangle(toolBrush, 0, 55, 400, 35)
                            End Using
                            g.FillRectangle(Brushes.White, 360, 60, 25, 25)
                            g.FillRectangle(Brushes.White, 325, 60, 25, 25)
                            g.FillRectangle(Brushes.White, 290, 60, 25, 25)
                            
                            ' Content Area - GroupBox
                            Using f As New Font("Tahoma", 8)
                                g.DrawString("اطلاعات پایه", f, Brushes.Black, 310, 100)
                            End Using
                            g.DrawRectangle(Pens.DarkGray, 20, 110, 360, 60)
                            
                            ' TextBox and Label
                            Using f As New Font("Tahoma", 8)
                                g.DrawString("نام:", f, Brushes.Black, 340, 130)
                            End Using
                            g.FillRectangle(Brushes.White, 180, 127, 150, 20)
                            g.DrawRectangle(Pens.Gray, 180, 127, 150, 20)
                            
                            ' Calculate AltColor for grid preview
                            Dim tintR = CInt(255 - ((255 - col.R) * 0.15))
                            Dim tintG = CInt(255 - ((255 - col.G) * 0.15))
                            Dim tintB = CInt(255 - ((255 - col.B) * 0.15))
                            
                            ' DataGridView
                            g.FillRectangle(Brushes.White, 20, 190, 360, 90)
                            ' Alternating Rows Backgrounds
                            Using altBrush As New SolidBrush(Color.FromArgb(255, tintR, tintG, tintB))
                                g.FillRectangle(altBrush, 21, 211, 358, 20)
                                g.FillRectangle(altBrush, 21, 251, 358, 20)
                            End Using
                            
                            g.DrawRectangle(Pens.DarkGray, 20, 190, 360, 90)
                            ' DGV Header
                            g.FillRectangle(Brushes.LightGray, 20, 190, 360, 20)
                            ' DGV Lines
                            g.DrawLine(Pens.LightGray, 20, 210, 380, 210)
                            g.DrawLine(Pens.LightGray, 20, 230, 380, 230)
                            g.DrawLine(Pens.LightGray, 20, 250, 380, 250)
                            g.DrawLine(Pens.LightGray, 20, 270, 380, 270)
                            ' DGV Columns
                            g.DrawLine(Pens.DarkGray, 100, 190, 100, 280)
                            g.DrawLine(Pens.DarkGray, 250, 190, 250, 280)
                        End Using
                        Using ms As New IO.MemoryStream()
                            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                            imgBytes = ms.ToArray()
                        End Using
                    End Using
                    
                    Sql.ExecuteNonQuery("INSERT INTO TemForm (ThemeName, ThemeColor, ThemeImage) VALUES (?, ?, ?)", name, colorStr, imgBytes)
                Next
                Log("bootstrap:temform-seeded")
            Catch ex As Exception
                Log("EnsureTemFormTableAndSeed error: " & ex.Message)
            End Try
        End Sub
        Private ReadOnly LogPath As String = Path.Combine(Application.StartupPath, "bootstrap.log")

                Private Sub EnsurePersonnelTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Personnel (
                        PersonnelID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Role TEXT,
                        NationalCode TEXT,
                        Phone TEXT,
                        Department INTEGER NOT NULL DEFAULT 1,
                        IsActive INTEGER NOT NULL DEFAULT 1
                    );")
            Catch ex As Exception
                ' Ignore
            End Try
        End Sub

        Public Sub EnsureSeedData()
            Try
                Log("bootstrap:start")
                AesDbService.PrepareRuntimeDatabase()
                EnsureDatabaseFile()
                Log("bootstrap:db-ok")
                EnsurePermissions()
                Log("bootstrap:permissions-ok")
                EnsureDefaultAdmin()
                Log("bootstrap:admin-ok")
                EnsureDefaultSettings()
                Log("bootstrap:settings-ok")
                EnsureColumnsExist()
                Log("bootstrap:columns-ok")
                EnsureUserNotesTable()
                Log("bootstrap:usernotes-ok")
                EnsureCalendarNotesTable()
                Log("bootstrap:calendarnotes-ok")
                BackgroundImageService.EnsureDatabaseTableAndSeed()
                Log("bootstrap:background-images-ok")
                EnsureSoBankTables()
                Log("bootstrap:sobank-tables-ok")
                EnsureProfitLossMappingsTable()
                Log("bootstrap:profit-loss-mappings-ok")
                EnsureTemFormTableAndSeed()
                Log("bootstrap:temform-table-ok")
                EnsureUnitsOfMeasureTable()
                Log("bootstrap:units-of-measure-ok")
                EnsureProductGroupsTable()
                Log("bootstrap:productgroups-ok")
                EnsureTemFormTableAndSeed()
                Log("bootstrap:temform-ok")
            EnsurePersonnelTable()
            Log("bootstrap:personnel-ok")
                EnsureWarehouseTypesTable()
                Log("bootstrap:warehousetypes-ok")
                EnsurePermissionPresetsTable()
                Log("bootstrap:permissionpresets-ok")
                EnsureWarehouseLocationsTable()
                Log("bootstrap:warehouselocations-ok")
                EnsureModyanCodesTable()
                Log("bootstrap:modyancodes-ok")
                EnsureWarehouseReceiptsTable()
                Log("bootstrap:warehousereceipts-ok")
                EnsurePaymentTables()
                Log("bootstrap:payment-tables-ok")
                EnsureCodStandardTable()
                Log("bootstrap:cod-standard-ok")
                EnsurePersonsTable()
                Log("bootstrap:persons-ok")
                EnsureExpensesTable()
                Log("bootstrap:expenses-ok")
            Catch ex As Exception
                Log("bootstrap:error:" & ex.Message & Environment.NewLine & ex.StackTrace)
                Throw
            End Try
        End Sub

        Private Sub EnsureDatabaseFile()
            Dim dataDir = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
            Log("dataDir=" & dataDir)
            If String.IsNullOrWhiteSpace(dataDir) Then
                Throw New InvalidOperationException("DataDirectory is not configured.")
            End If

            Dim dbFile = Path.Combine(dataDir, "Negar.db")
            Dim accdbFile = Path.Combine(dataDir, "Negar.accdb")
            Log("dbFile=" & dbFile)

            Dim isNewDb As Boolean = Not File.Exists(dbFile)

            If isNewDb Then
                Log("Creating new SQLite database file...")
                InitializeSchema(dbFile)
                Log("database-schema-created")

                If File.Exists(accdbFile) Then
                    Log("Found existing Access database (" & accdbFile & "). Starting automatic data migration...")
                    Try
                        MigrateAccessToSQLite(accdbFile, dbFile)
                        Log("migration-completed-successfully")
                    Catch ex As Exception
                        Log("migration-failed: " & ex.Message & Environment.NewLine & ex.StackTrace)
                    End Try
                End If
            End If
        End Sub

        Private Sub InitializeSchema(dbFile As String)
            Dim schemaFile = Path.Combine(Application.StartupPath, "Database", "CreateSchema.sql")
            If Not File.Exists(schemaFile) Then
                Throw New FileNotFoundException("Schema file not found.", schemaFile)
            End If

            Dim script = File.ReadAllText(schemaFile)
            Dim statements = script.Split(New String() {";"}, StringSplitOptions.RemoveEmptyEntries)

            Using connection As New SQLiteConnection("Data Source=" & dbFile & ";Version=3;")
                connection.Open()
                Using transaction = connection.BeginTransaction()
                    For Each statement In statements
                        Dim sql = statement.Trim()
                        If sql.Length = 0 Then Continue For
                        Log("schema-sql=" & sql.Replace(Environment.NewLine, " "))
                        Using command As New SQLiteCommand(sql, connection, transaction)
                            command.ExecuteNonQuery()
                        End Using
                    Next
                    transaction.Commit()
                End Using
            End Using
        End Sub

        Private Function GetSQLiteDataType(t As Type, colName As String) As String
            If t Is GetType(Boolean) Then Return "BOOLEAN"
            If t Is GetType(Byte) OrElse t Is GetType(Int16) OrElse t Is GetType(Int32) OrElse t Is GetType(Int64) Then Return "INTEGER"
            If t Is GetType(Single) OrElse t Is GetType(Double) Then Return "REAL"
            If t Is GetType(Decimal) Then Return "DECIMAL"
            If t Is GetType(DateTime) Then Return "DATETIME"
            If t Is GetType(Byte()) Then Return "BLOB"
            Return "TEXT"
        End Function

        Private Sub MigrateAccessToSQLite(accdbFile As String, dbFile As String)
            Dim accConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & accdbFile & ";Persist Security Info=False;"
            Dim sqliteConnStr = "Data Source=" & dbFile & ";Version=3;"

            Using accConn As New OleDbConnection(accConnStr), sqliteConn As New SQLiteConnection(sqliteConnStr)
                accConn.Open()
                sqliteConn.Open()

                ' Get list of all user tables in Access database
                Dim schemaTable = accConn.GetSchema("Tables")
                Dim allAccTables As New List(Of String)()
                For Each row As DataRow In schemaTable.Rows
                    Dim tType = Convert.ToString(row("TABLE_TYPE"))
                    Dim tName = Convert.ToString(row("TABLE_NAME"))
                    If String.Equals(tType, "TABLE", StringComparison.OrdinalIgnoreCase) AndAlso Not tName.StartsWith("MSys", StringComparison.OrdinalIgnoreCase) Then
                        allAccTables.Add(tName)
                    End If
                Next

                For Each tableName In allAccTables
                    Try
                        Log("Migrating table: " & tableName)
                        Dim dt As New DataTable()
                        Using cmd As New OleDbCommand("SELECT * FROM [" & tableName & "]", accConn)
                            Using adapter As New OleDbDataAdapter(cmd)
                                adapter.Fill(dt)
                            End Using
                        End Using

                        If dt.Columns.Count = 0 Then Continue For

                        ' Get existing tables in SQLite
                        Dim sqliteTablesSchema = sqliteConn.GetSchema("Tables")
                        Dim sqliteTableExists As Boolean = False
                        For Each tRow As DataRow In sqliteTablesSchema.Rows
                            If String.Equals(Convert.ToString(tRow("TABLE_NAME")), tableName, StringComparison.OrdinalIgnoreCase) Then
                                sqliteTableExists = True
                                Exit For
                            End If
                        Next

                        If Not sqliteTableExists Then
                            ' Dynamically create table in SQLite
                            Dim colDefs As New List(Of String)()
                            For Each col As DataColumn In dt.Columns
                                Dim colType = GetSQLiteDataType(col.DataType, col.ColumnName)
                                If String.Equals(col.ColumnName, tableName & "ID", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(col.ColumnName, "ID", StringComparison.OrdinalIgnoreCase) Then
                                    colDefs.Add("[" & col.ColumnName & "] INTEGER PRIMARY KEY AUTOINCREMENT")
                                Else
                                    colDefs.Add("[" & col.ColumnName & "] " & colType)
                                End If
                            Next
                            Dim createSql = "CREATE TABLE [" & tableName & "] (" & String.Join(", ", colDefs.ToArray()) & ")"
                            Using createCmd As New SQLiteCommand(createSql, sqliteConn)
                                createCmd.ExecuteNonQuery()
                            End Using
                            Log("Created new table in SQLite: " & tableName)
                        Else
                            ' Ensure all columns from Access exist in target SQLite table
                            Dim sqliteColsTable = sqliteConn.GetSchema("Columns", New String() {Nothing, Nothing, tableName, Nothing})
                            Dim existingTargetCols As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
                            For Each cRow As DataRow In sqliteColsTable.Rows
                                existingTargetCols.Add(Convert.ToString(cRow("COLUMN_NAME")))
                            Next

                            For Each col As DataColumn In dt.Columns
                                If Not existingTargetCols.Contains(col.ColumnName) Then
                                    Dim colType = GetSQLiteDataType(col.DataType, col.ColumnName)
                                    Using alterCmd As New SQLiteCommand("ALTER TABLE [" & tableName & "] ADD COLUMN [" & col.ColumnName & "] " & colType, sqliteConn)
                                        alterCmd.ExecuteNonQuery()
                                    End Using
                                    Log("Added missing column to " & tableName & ": " & col.ColumnName)
                                End If
                            Next
                        End If

                        If dt.Rows.Count = 0 Then Continue For

                        ' Clear seeded data in SQLite before migration
                        Using delCmd As New SQLiteCommand("DELETE FROM [" & tableName & "]", sqliteConn)
                            delCmd.ExecuteNonQuery()
                        End Using

                        ' Build parameterized insert query for all columns
                        Dim colNames As New List(Of String)()
                        Dim paramNames As New List(Of String)()
                        For i As Integer = 0 To dt.Columns.Count - 1
                            colNames.Add("[" & dt.Columns(i).ColumnName & "]")
                            paramNames.Add("@p" & (i + 1))
                        Next

                        Dim insertSql = "INSERT INTO [" & tableName & "] (" & String.Join(", ", colNames.ToArray()) & ") VALUES (" & String.Join(", ", paramNames.ToArray()) & ")"

                        Using transaction = sqliteConn.BeginTransaction()
                            Using insertCmd As New SQLiteCommand(insertSql, sqliteConn, transaction)
                                For idx As Integer = 1 To dt.Columns.Count
                                    insertCmd.Parameters.Add(New SQLiteParameter("@p" & idx))
                                Next

                                For Each row As DataRow In dt.Rows
                                    For colIdx As Integer = 0 To dt.Columns.Count - 1
                                        Dim val = row(colIdx)
                                        If val Is Nothing OrElse Convert.IsDBNull(val) Then
                                            insertCmd.Parameters(colIdx).Value = DBNull.Value
                                        Else
                                            insertCmd.Parameters(colIdx).Value = val
                                        End If
                                    Next
                                    insertCmd.ExecuteNonQuery()
                                Next
                            End Using
                            transaction.Commit()
                        End Using
                        Log("Migrated " & dt.Rows.Count & " rows for table " & tableName)
                    Catch ex As Exception
                        Log("Table migration error (" & tableName & "): " & ex.Message)
                    End Try
                Next
            End Using
        End Sub

        Private Sub EnsurePermissions()
            Dim permissions = {
                Tuple.Create("مدیریت کاربران (جامع)", PermissionKeys.ManageUsers),
                Tuple.Create("مدیریت کاربران – مدیریت کاربران عادی", PermissionKeys.ManageBasicUsers),
                Tuple.Create("مدیریت شرکت‌ها", PermissionKeys.ManageCompanies),
                Tuple.Create("مدیریت سال‌های مالی", PermissionKeys.ManageFiscalYears),
                Tuple.Create("مدیریت شرکت‌ها و سال‌های مالی ( جامع )", PermissionKeys.ManageCompaniesYears),
                Tuple.Create("انتخاب شرکت و سال مالی جاری", PermissionKeys.SelectCompanyFiscalYear),
                Tuple.Create("مدیریت تمهای برنامه و فرمها", PermissionKeys.ManageAppThemes),
                Tuple.Create("مدیریت پیامهای درباره ما و ارتباط با ما", PermissionKeys.ManageAppMessages),
                Tuple.Create("تبدیل دیتا از سایر نرم افزارها", PermissionKeys.DataMigration),
                Tuple.Create("پشتیبان‌گیری اطلاعات", PermissionKeys.BackupData),
                Tuple.Create("بازیابی اطلاعات", PermissionKeys.RestoreData),
                Tuple.Create("پوسته مشاغل", PermissionKeys.ManageBusinessShells),
                Tuple.Create("امکانات", PermissionKeys.ManageUtilities),
                Tuple.Create("مشاهده دفتر سوابق و گزارش فعالیت‌ها", PermissionKeys.ViewActivityLog),
                Tuple.Create("قطعی‌سازی و قفل اسناد حسابداری", PermissionKeys.LockSanad1),
                Tuple.Create("مخفی کردن ستونهای SF و SH در فرم سند حسابداری", PermissionKeys.HideSFSHInSanad),
                Tuple.Create("مدیریت کاربران – ورود با کاربر دیگر", PermissionKeys.SwitchUser),
                Tuple.Create("مدیریت کاربران – تغییر کلمه عبور", PermissionKeys.ChangePassword),
                Tuple.Create("حسابداری – تنظیمات اولیه حسابها", PermissionKeys.AccountingSettings),
                Tuple.Create("حسابداری – سرفصل حسابها", PermissionKeys.AccountingHeader),
                Tuple.Create("حسابداری – حسابهای شناور", PermissionKeys.AccountingShenavar),
                Tuple.Create("حسابداری – ثبت سند حسابداری", PermissionKeys.AccountingEntry),
                Tuple.Create("حسابداری – مغایرات بانکی", PermissionKeys.AccountingBank),
                Tuple.Create("حسابداری – تراز آزمایشی", PermissionKeys.AccountingBalance),
                Tuple.Create("حسابداری – تراز شناور", PermissionKeys.AccountingTarazShenavar),
                Tuple.Create("حسابداری – دفتر حساب", PermissionKeys.AccountingLedger),
                Tuple.Create("حسابداری – دفتر شناور", PermissionKeys.AccountingDaftarShenavar),
                Tuple.Create("حسابداری – گزارشات حسابداری", PermissionKeys.AccountingReports),
                Tuple.Create("حسابداری – عملکرد و سود و زیان", PermissionKeys.AccountingProfitLoss),
                Tuple.Create("حسابداری – ترازنامه مالی", PermissionKeys.AccountingBalanceSheet),
                Tuple.Create("حسابداری – گزارشات پیشرفته", PermissionKeys.AccountingAdvancedReports),
                Tuple.Create("حسابداری – گزارشات نموداری", PermissionKeys.AccountingChartReports),
                Tuple.Create("حسابداری – گزارشات دلخواه", PermissionKeys.AccountingCustomReports),
                Tuple.Create("حسابداری – فرم سند 1 – چاپ اسناد", PermissionKeys.AccountingSanad1PrintDocs),
                Tuple.Create("حسابداری – فرم سند 1 – چاپ دفتر روزنامه", PermissionKeys.AccountingSanad1PrintJournal),
                Tuple.Create("حسابداری – فرم سند 2 – چاپ سند Ctrl+P", PermissionKeys.AccountingSanad2PrintVoucher),
                Tuple.Create("حسابداری – مغایرات – خروجی اکسل ", PermissionKeys.AccountingBankRecExportExcel),
                Tuple.Create("حسابداری – تراز آزمایشی – چاپ تراز", PermissionKeys.AccountingTrialPrint),
                Tuple.Create("حسابداری – تراز آزمایشی – خروجی اکسل", PermissionKeys.AccountingTrialExport),
                Tuple.Create("حسابداری – دفتر حساب – چاپ دفتر", PermissionKeys.AccountingLedgerPrint),
                Tuple.Create("حسابداری – دفتر حساب – خروجی اکسل", PermissionKeys.AccountingLedgerExport),
                Tuple.Create("حسابداری – تراز شناور – چاپ تراز", PermissionKeys.AccountingTarazShenavarPrint),
                Tuple.Create("حسابداری – تراز شناور – خروجی اکسل", PermissionKeys.AccountingTarazShenavarExport),
                Tuple.Create("حسابداری – دفتر شناور – چاپ دفتر", PermissionKeys.AccountingDaftarShenavarPrint),
                Tuple.Create("حسابداری – دفتر شناور – خروجی اکسل", PermissionKeys.AccountingDaftarShenavarExport),
                Tuple.Create("حسابداری – عملکرد و سود و زیان – چاپ", PermissionKeys.AccountingProfitLossPrint),
                Tuple.Create("حسابداری – عملکرد و سود و زیان – خروجی اکسل", PermissionKeys.AccountingProfitLossExport),
                Tuple.Create("حسابداری – ترازنامه – چاپ", PermissionKeys.AccountingBalanceSheetPrint),
                Tuple.Create("حسابداری – ترازنامه – خروجی اکسل", PermissionKeys.AccountingBalanceSheetExport),
                Tuple.Create("حسابداری – طراحی گزارش دلخواه – چاپ", PermissionKeys.AccountingCustomReportPrint),
                Tuple.Create("حسابداری – طراحی گزارش دلخواه – خروجی اکسل", PermissionKeys.AccountingCustomReportExport),
                Tuple.Create("خرید و فروش – واحدهای سنجش کالا", PermissionKeys.TradeProductUnits),
                Tuple.Create("خرید و فروش – دسته‌بندی و گروه‌های کالا", PermissionKeys.TradeProductGroups),
                Tuple.Create("خرید و فروش – تعریف کالاها و خدمات", PermissionKeys.TradeProducts),
                Tuple.Create("خرید و فروش – تعریف انبارها", PermissionKeys.TradeWarehouses),
                Tuple.Create("خرید و فروش – صدور فاکتور خرید", PermissionKeys.TradePurchase),
                Tuple.Create("خرید و فروش – صدور فاکتور فروش", PermissionKeys.TradeSales),
                Tuple.Create("خرید و فروش – حواله و رسید انبار", PermissionKeys.TradeRemittance),
                Tuple.Create("خرید و فروش – گزارشات انبار و کاردکس کالا", PermissionKeys.TradeReports),
                Tuple.Create("خرید و فروش و انبارداری ( جامع )", PermissionKeys.ManageTradeWarehouse),
                Tuple.Create("استفاده از انبارداری مینی", PermissionKeys.AnbarMiniModule),
                Tuple.Create("استفاده از انبارداری متوسط", PermissionKeys.AnbarMediumModule),
                Tuple.Create("استفاده از انبارداری پیشرفته", PermissionKeys.AnbarBigModule),
                Tuple.Create("انبار مینی – ثبت هزینه‌ها", PermissionKeys.AnbarMiniExpenses),
                Tuple.Create("انبار مینی – دفتر هزینه", PermissionKeys.AnbarMiniExpenseLedger),
                Tuple.Create("انبار مینی – چاپ سود و زیان", PermissionKeys.AnbarMiniProfitLoss)
            }

            For Each permission In permissions
                Dim exists = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Permissions WHERE PermissionKey = ?", permission.Item2), 0))
                If exists = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO Permissions (PermissionName, PermissionKey) VALUES (?, ?)", permission.Item1, permission.Item2)
                    Log("permission-added=" & permission.Item2)
                Else
                    Sql.ExecuteNonQuery("UPDATE Permissions SET PermissionName = ? WHERE PermissionKey = ?", permission.Item1, permission.Item2)
                End If
            Next
        End Sub

        Private Sub EnsureDefaultAdmin()
            Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM Users"), 0))
            Log("users-count=" & count.ToString())
            If count > 0 Then Return

            Dim passwordHash = PasswordHasher.Hash("admin123")
            Sql.ExecuteNonQuery(
                "INSERT INTO Users (Username, [Password], UserType, CreatedBy, CreatedDate, IsActive, FullName) VALUES (?, ?, ?, ?, ?, ?, ?)",
                "admin", passwordHash, "SuperAdmin", 0, DateTime.Now, True, "System Administrator")
        End Sub

        Private Sub EnsureDefaultSettings()
            Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM AppSettings"), 0))
            Log("settings-count=" & count.ToString())
            If count = 0 Then
                Sql.ExecuteNonQuery("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES (?, ?, ?)", "Theme", "Light", "UI")
                Sql.ExecuteNonQuery("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES (?, ?, ?)", "NumberFormat", "N2", "UI")
                Sql.ExecuteNonQuery("INSERT INTO AppSettings (SettingKey, SettingValue, SettingCategory) VALUES (?, ?, ?)", "CurrencySymbol", "ریال", "UI")
            End If

        End Sub

        Private Sub EnsureColumnsExist()
            ' Ensure CreatorIP in Users
            AddColumnIfMissing("Users", "CreatorIP", "TEXT")
            AddColumnIfMissing("Users", "MaxCompaniesAllowed", "INTEGER")
            AddColumnIfMissing("Users", "MaxFiscalYearsPerCompany", "INTEGER")
            ' Ensure OwnerUserID in Companies
            AddColumnIfMissing("Companies", "OwnerUserID", "INTEGER")
            AddColumnIfMissing("Companies", "AccountLevels", "TEXT")
            AddColumnIfMissing("Companies", "LogoPosition", "TEXT")
            AddColumnIfMissing("Companies", "Level1Length", "INTEGER")
            AddColumnIfMissing("Companies", "Level2Length", "INTEGER")
            AddColumnIfMissing("Companies", "Level3Length", "INTEGER")
            AddColumnIfMissing("Companies", "Level4Length", "INTEGER")
            AddColumnIfMissing("Companies", "Level5Length", "INTEGER")
            AddColumnIfMissing("Companies", "ProductGroupLevels", "INTEGER DEFAULT 3")

            ' Ensure SectionName in Permissions
            AddColumnIfMissing("Permissions", "SectionName", "TEXT")

            ' Ensure UoM & Catch Weight columns in Products
            AddColumnIfMissing("Products", "BaseUoMID", "INTEGER")
            AddColumnIfMissing("Products", "IsCatchWeight", "BOOLEAN DEFAULT 0")
            AddColumnIfMissing("Products", "SecondaryUoMID", "INTEGER")
            AddColumnIfMissing("Products", "NominalFactor", "DECIMAL")
            AddColumnIfMissing("Products", "ProductGroupID", "INTEGER")
            AddColumnIfMissing("Products", "Barcode", "TEXT")
            AddColumnIfMissing("Products", "TaxID", "TEXT")
            AddColumnIfMissing("Products", "ProductType", "TEXT DEFAULT 'کالا'")
            AddColumnIfMissing("Products", "PurchasePrice", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "MinStock", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "ReorderPoint", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "MaxStock", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "TrackingType", "TEXT DEFAULT 'عادی'")
            AddColumnIfMissing("Products", "LocationID", "INTEGER")
            AddColumnIfMissing("Products", "TechnicalName", "TEXT")
            AddColumnIfMissing("Products", "ConsumerMarkup", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "ConsumerDiscount", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "ColleagueMarkup", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "ColleagueDiscount", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "WholesaleMarkup", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "WholesaleDiscount", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "TaxPercent", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "TollPercent", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "NetWeight", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "GrossWeight", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "Length", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "Width", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "Height", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "Volume", "DECIMAL DEFAULT 0")
            AddColumnIfMissing("Products", "Color", "TEXT")
            AddColumnIfMissing("Products", "Material", "TEXT")
            AddColumnIfMissing("Products", "Size", "TEXT")
            AddColumnIfMissing("Products", "Brand", "TEXT")
            AddColumnIfMissing("Products", "CountryOfOrigin", "TEXT")
            AddColumnIfMissing("Products", "PhysicalDescription", "TEXT")
            AddColumnIfMissing("Products", "Image1", "TEXT")
            AddColumnIfMissing("Products", "Image2", "TEXT")
            AddColumnIfMissing("Products", "Image3", "TEXT")
            AddColumnIfMissing("Products", "Image4", "TEXT")
            AddColumnIfMissing("Products", "Image5", "TEXT")
            AddColumnIfMissing("Products", "Image6", "TEXT")
            
            ' Ensure extra Warehouse columns
            AddColumnIfMissing("Warehouses", "WarehouseType", "TEXT")
            AddColumnIfMissing("Warehouses", "Phone", "TEXT")
            AddColumnIfMissing("Warehouses", "Phone2", "TEXT")
            AddColumnIfMissing("Warehouses", "Phone3", "TEXT")
            AddColumnIfMissing("Warehouses", "PostalCode", "TEXT")
            AddColumnIfMissing("Warehouses", "Capacity", "REAL")
            AddColumnIfMissing("Warehouses", "WarehouseKeeper", "TEXT")
            AddColumnIfMissing("Warehouses", "CostCenter", "TEXT")
            AddColumnIfMissing("Warehouses", "AllowNegativeStock", "BOOLEAN")
            AddColumnIfMissing("Warehouses", "Description", "TEXT")

            ' Ensure extra PurchaseInvoices & Details columns
            AddColumnIfMissing("PurchaseInvoices", "InvoiceType", "TEXT")
            AddColumnIfMissing("PurchaseInvoices", "VendorInvoiceNumber", "TEXT")
            AddColumnIfMissing("PurchaseInvoices", "DiscountAmount", "DECIMAL")
            AddColumnIfMissing("PurchaseInvoices", "PaymentType", "TEXT")
            AddColumnIfMissing("PurchaseInvoices", "Description", "TEXT")
            AddColumnIfMissing("PurchaseInvoices", "TaxEntryMode", "INTEGER")
            AddColumnIfMissing("PurchaseInvoices", "TotalVat", "DECIMAL")
            AddColumnIfMissing("PurchaseInvoices", "ReceiptStatus", "TEXT")
            AddColumnIfMissing("PurchaseInvoiceDetails", "Discount", "DECIMAL")
            AddColumnIfMissing("PurchaseInvoiceDetails", "Vat", "DECIMAL")
            AddColumnIfMissing("PurchaseInvoiceDetails", "ReceivedQuantity", "DECIMAL")
            AddColumnIfMissing("SarfaslShenavar", "CreatedBy", "INTEGER")
            AddColumnIfMissing("Products", "DefaultWarehouseID", "INTEGER")

            ' Ensure CompanyID, PaymentType, Description columns for company data isolation and sales notes
            AddColumnIfMissing("Products", "CompanyID", "INTEGER")
            AddColumnIfMissing("Warehouses", "CompanyID", "INTEGER")
            AddColumnIfMissing("PurchaseInvoices", "CompanyID", "INTEGER")
            AddColumnIfMissing("SalesInvoices", "CompanyID", "INTEGER")
            AddColumnIfMissing("SalesInvoices", "PaymentType", "TEXT")
            AddColumnIfMissing("SalesInvoices", "Description", "TEXT")
            AddColumnIfMissing("WarehouseReceipts", "CompanyID", "INTEGER")

            Try
                ' Backfill CompanyID for existing legacy data if needed
                Dim defaultComp = Sql.ExecuteScalar("SELECT MIN(CompanyID) FROM Companies")
                If defaultComp IsNot Nothing AndAlso Not Convert.IsDBNull(defaultComp) Then
                    Dim cid = Convert.ToInt32(defaultComp)
                    Sql.ExecuteNonQuery("UPDATE Products SET CompanyID = ? WHERE CompanyID IS NULL", cid)
                    Sql.ExecuteNonQuery("UPDATE Warehouses SET CompanyID = ? WHERE CompanyID IS NULL", cid)
                    Sql.ExecuteNonQuery("UPDATE PurchaseInvoices SET CompanyID = ? WHERE CompanyID IS NULL", cid)
                    Sql.ExecuteNonQuery("UPDATE SalesInvoices SET CompanyID = ? WHERE CompanyID IS NULL", cid)
                    Sql.ExecuteNonQuery("UPDATE WarehouseReceipts SET CompanyID = ? WHERE CompanyID IS NULL", cid)
                End If

                ' Re-align PurchaseInvoices WarehouseID to match the invoice company's own warehouse
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoices SET WarehouseID = (" &
                                    "  SELECT MIN(w.WarehouseID) FROM Warehouses w WHERE w.CompanyID = PurchaseInvoices.CompanyID" &
                                    ") WHERE CompanyID IS NOT NULL AND WarehouseID IN (" &
                                    "  SELECT i.WarehouseID FROM PurchaseInvoices i JOIN Warehouses w ON i.WarehouseID = w.WarehouseID WHERE i.CompanyID <> w.CompanyID" &
                                    ")")

                ' Re-align SalesInvoices WarehouseID to match the invoice company's own warehouse
                Sql.ExecuteNonQuery("UPDATE SalesInvoices SET WarehouseID = (" &
                                    "  SELECT MIN(w.WarehouseID) FROM Warehouses w WHERE w.CompanyID = SalesInvoices.CompanyID" &
                                    ") WHERE CompanyID IS NOT NULL AND WarehouseID IN (" &
                                    "  SELECT i.WarehouseID FROM SalesInvoices i JOIN Warehouses w ON i.WarehouseID = w.WarehouseID WHERE i.CompanyID <> w.CompanyID" &
                                    ")")

                ' Backfill Inventory records from existing PurchaseInvoices and SalesInvoices
                Dim dtPurchaseLines = Sql.ExecuteTable(
                    "SELECT pi.CompanyID, pi.WarehouseID, pd.ProductID, SUM(pd.Quantity) AS TotalPurchased, AVG(pd.UnitPrice) AS AvgPrice " &
                    "FROM PurchaseInvoiceDetails pd JOIN PurchaseInvoices pi ON pd.InvoiceID = pi.InvoiceID " &
                    "GROUP BY pi.CompanyID, pi.WarehouseID, pd.ProductID")

                If dtPurchaseLines IsNot Nothing Then
                    For Each row As DataRow In dtPurchaseLines.Rows
                        Dim pid = Convert.ToInt32(row("ProductID"))
                        Dim wid = Convert.ToInt32(row("WarehouseID"))
                        Dim purchasedQty = Convert.ToDecimal(row("TotalPurchased"))
                        Dim avgPrice = Convert.ToDecimal(row("AvgPrice"))

                        Dim soldQtyObj = Sql.ExecuteScalar(
                            "SELECT SUM(sd.Quantity) FROM SalesInvoiceDetails sd JOIN SalesInvoices si ON sd.InvoiceID = si.InvoiceID " &
                            "WHERE sd.ProductID = ? AND si.WarehouseID = ?", pid, wid)
                        Dim soldQty As Decimal = 0D
                        If soldQtyObj IsNot Nothing AndAlso Not Convert.IsDBNull(soldQtyObj) Then
                            soldQty = Convert.ToDecimal(soldQtyObj)
                        End If

                        Dim finalStock = purchasedQty - soldQty
                        Dim invSvc As New Negar.Business.InventoryService()
                        invSvc.UpsertInventory(pid, wid, finalStock, avgPrice)
                    Next
                End If
            Catch ex As Exception
            End Try

            Try
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_Products_GroupID ON Products (ProductGroupID)")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_Products_CompanyID ON Products (CompanyID)")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_Warehouses_CompanyID ON Warehouses (CompanyID)")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_PurchaseInvoices_CompanyID ON PurchaseInvoices (CompanyID)")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_SalesInvoices_CompanyID ON SalesInvoices (CompanyID)")
            Catch ex As Exception
                ' Index might already exist
            End Try

            Try
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoiceDetails SET ReceivedQuantity = Quantity WHERE ReceivedQuantity IS NULL")
                Sql.ExecuteNonQuery("UPDATE PurchaseInvoiceDetails SET ReceivedQuantity = 0 WHERE DetailID NOT IN (SELECT PurchaseInvoiceDetailID FROM WarehouseReceiptDetails)")
            Catch ex As Exception
            End Try


            ' Populate default section names if NULL or empty
            Try
                Dim dtPerms = Sql.ExecuteTable("SELECT PermissionID, PermissionKey, SectionName FROM Permissions")
                For Each row As DataRow In dtPerms.Rows
                    If row.IsNull("SectionName") OrElse String.IsNullOrWhiteSpace(Convert.ToString(row("SectionName"))) Then
                        Dim pId = Convert.ToInt32(row("PermissionID"))
                        Dim pKey = Convert.ToString(row("PermissionKey"))
                        Sql.ExecuteNonQuery("UPDATE Permissions SET SectionName = ? WHERE PermissionID = ?", GetDefaultSectionName(pKey), pId)
                    End If
                Next
            Catch ex As Exception
                Log("EnsureColumnsExist Populate SectionName error: " & ex.Message)
            End Try

            ' Check if OwnerUserID was updated
            Dim adminIdObj = Sql.ExecuteScalar("SELECT UserID FROM Users WHERE UserType = 'SuperAdmin' ORDER BY UserID LIMIT 1")
            If adminIdObj IsNot Nothing AndAlso Not Convert.IsDBNull(adminIdObj) Then
                Sql.ExecuteNonQuery("UPDATE Companies SET OwnerUserID = ? WHERE OwnerUserID IS NULL", Convert.ToInt32(adminIdObj))
            End If
        End Sub

        Private Function GetDefaultSectionName(permissionKey As String) As String
            Select Case permissionKey
                Case PermissionKeys.AccountingHeader,
                     PermissionKeys.AccountingShenavar,
                     PermissionKeys.AccountingEntry,
                     PermissionKeys.AccountingBank,
                     PermissionKeys.AccountingBalance,
                     PermissionKeys.AccountingLedger,
                     PermissionKeys.AccountingReports,
                     PermissionKeys.ManageAccounting,
                     PermissionKeys.LockSanad1,
                     PermissionKeys.HideSFSHInSanad
                    Return "حسابداری"

                Case PermissionKeys.TradeProducts,
                     PermissionKeys.TradeWarehouses,
                     PermissionKeys.TradePurchase,
                     PermissionKeys.TradeSales,
                     PermissionKeys.TradeRemittance,
                     PermissionKeys.TradeReports,
                     PermissionKeys.ManageProducts,
                     PermissionKeys.ManageWarehouses,
                     PermissionKeys.ManagePurchases,
                     PermissionKeys.ManageSales,
                     PermissionKeys.ViewInventory,
                     PermissionKeys.ManageTradeWarehouse
                    Return "خرید و فروش"

                Case Else
                    Return "مدیریت سیستم"
            End Select
        End Function

        Private Sub EnsureUserNotesTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS UserNotes (" &
                    "NoteID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "UserID INTEGER NOT NULL, " &
                    "NoteDate DATETIME NOT NULL, " &
                    "MainSubject TEXT, " &
                    "SubSubject1 TEXT, " &
                    "SubSubject2 TEXT, " &
                    "NoteContent TEXT, " &
                    "EditHistory TEXT, " &
                    "CreatedDate DATETIME, " &
                    "UpdatedDate DATETIME);")
            Catch ex As Exception
                Log("EnsureUserNotesTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureCalendarNotesTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CalendarNotes (" &
                    "CalendarNoteID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "UserID INTEGER NOT NULL, " &
                    "PersianDate TEXT NOT NULL, " &
                    "NoteText TEXT, " &
                    "ReminderTime TEXT, " &
                    "IsReminder INTEGER DEFAULT 0, " &
                    "CreatedDate DATETIME, " &
                    "UpdatedDate DATETIME);")
            Catch ex As Exception
                Log("EnsureCalendarNotesTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub AddColumnIfMissing(tableName As String, columnName As String, columnType As String)
            Try
                Using conn = Db.OpenConnection()
                    Dim schemaTable = conn.GetSchema("Columns", New String() {Nothing, Nothing, tableName, Nothing})
                    Dim exists As Boolean = False
                    For Each row As DataRow In schemaTable.Rows
                        If String.Equals(Convert.ToString(row("COLUMN_NAME")), columnName, StringComparison.OrdinalIgnoreCase) Then
                            exists = True
                            Exit For
                        End If
                    Next

                    If Not exists Then
                        Using cmd As New SQLiteCommand("ALTER TABLE [" & tableName & "] ADD COLUMN [" & columnName & "] " & columnType, conn)
                            cmd.ExecuteNonQuery()
                        End Using
                        Log("column-added=" & tableName & "." & columnName)
                    End If
                End Using
            Catch ex As Exception
                Log("AddColumnIfMissing error (" & tableName & "." & columnName & "): " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureSoBankTables()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SoBank_1 (" &
                    "BankID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER NOT NULL, " &
                    "BankName TEXT, " &
                    "BranchName TEXT, " &
                    "BranchCode TEXT, " &
                    "BranchAddress TEXT, " &
                    "AccountType TEXT, " &
                    "AccountNumber TEXT, " &
                    "AccountID INTEGER);")

                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS SoBank_2 (" &
                    "TxID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "BankID INTEGER NOT NULL, " &
                    "TxDate TEXT NOT NULL, " &
                    "RefNo TEXT, " &
                    "Debit DECIMAL, " &
                    "Credit DECIMAL, " &
                    "Description TEXT, " &
                    "Payee TEXT, " &
                    "ImportDate DATETIME DEFAULT CURRENT_TIMESTAMP, " &
                    "FOREIGN KEY(BankID) REFERENCES SoBank_1(BankID) ON DELETE CASCADE);")

                AddColumnIfMissing("SoBank_2", "Payee", "TEXT")
                AddColumnIfMissing("SoBank_2", "MatchedDetailID", "INTEGER")
                AddColumnIfMissing("SoBank_1", "HeaderRowIndex", "INTEGER")
                AddColumnIfMissing("SoBank_1", "ColDate", "TEXT")
                AddColumnIfMissing("SoBank_1", "ColRef", "TEXT")
                AddColumnIfMissing("SoBank_1", "ColDebit", "TEXT")
                AddColumnIfMissing("SoBank_1", "ColCredit", "TEXT")
                AddColumnIfMissing("SoBank_1", "ColDesc", "TEXT")
                AddColumnIfMissing("SoBank_1", "ColPayee", "TEXT")
            Catch ex As Exception
                Log("EnsureSoBankTables error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureProfitLossMappingsTable()
            Try
                ' Drop old table if exists
                Sql.ExecuteNonQuery("DROP TABLE IF EXISTS ProfitLossCategories;")

                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Report1 (" &
                    "ReportID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ReportCode TEXT NOT NULL, " &
                    "ReportName TEXT NOT NULL, " &
                    "CompanyID INTEGER NOT NULL, " &
                    "FontHeaderName TEXT NULL, " &
                    "FontHeaderSize REAL NULL, " &
                    "FontMainRowName TEXT NULL, " &
                    "FontMainRowSize REAL NULL, " &
                    "FontDetailRowName TEXT NULL, " &
                    "FontDetailRowSize REAL NULL, " &
                    "FontFormulaName TEXT NULL, " &
                    "FontFormulaSize REAL NULL, " &
                    "RowCount INTEGER NULL, " &
                    "ColCount INTEGER NULL, " &
                    "Orientation TEXT NULL, " &
                    "MarginTop REAL NULL, " &
                    "MarginBottom REAL NULL, " &
                    "MarginLeft REAL NULL, " &
                    "MarginRight REAL NULL, " &
                    "PageBorder TEXT NULL);")

                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Report2 (" &
                    "CategoryID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ReportID INTEGER NOT NULL, " &
                    "CategoryName TEXT NOT NULL, " &
                    "SortOrder INTEGER NOT NULL, " &
                    "CompanyID INTEGER NOT NULL, " &
                    "Formula TEXT NULL, " &
                    "IsMainRow INTEGER NULL, " &
                    "RO TEXT NULL, " &
                    "SO TEXT NULL, " &
                    "RN TEXT NULL, " &
                    "SN TEXT NULL, " &
                    "UnderlineStyle TEXT NULL, " &
                    "FOREIGN KEY(ReportID) REFERENCES Report1(ReportID) ON DELETE CASCADE);")

                 ' Alter existing databases to make sure all new columns are present
                 Dim colsReport1 As String() = {
                     "FontHeaderName TEXT", "FontHeaderSize REAL", "FontMainRowName TEXT", "FontMainRowSize REAL",
                     "FontDetailRowName TEXT", "FontDetailRowSize REAL", "FontFormulaName TEXT", "FontFormulaSize REAL",
                     "RowCount INTEGER", "ColCount INTEGER", "Orientation TEXT", "MarginTop REAL", "MarginBottom REAL",
                     "MarginLeft REAL", "MarginRight REAL", "PageBorder TEXT"
                 }
                 For Each col In colsReport1
                     Try
                         Sql.ExecuteNonQuery("ALTER TABLE Report1 ADD COLUMN " & col & ";")
                     Catch
                     End Try
                 Next

                 Dim colsReport2 As String() = {
                     "Formula TEXT", "IsMainRow INTEGER", "RO TEXT", "SO TEXT", "RN TEXT", "SN TEXT", "UnderlineStyle TEXT"
                 }
                 For Each col In colsReport2
                     Try
                         Sql.ExecuteNonQuery("ALTER TABLE Report2 ADD COLUMN " & col & ";")
                     Catch
                     End Try
                 Next
            Catch ex As Exception
                Log("EnsureProfitLossMappingsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureUnitsOfMeasureTable()
            Try
                ' 1. uom_categories
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS uom_categories (" &
                    "CategoryID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CategoryName TEXT NOT NULL, " &
                    "CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP);")

                ' 2. uoms
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS uoms (" &
                    "UoMID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CategoryID INTEGER NOT NULL, " &
                    "UoMName TEXT NOT NULL, " &
                    "Abbreviation TEXT, " &
                    "IsReferenceUoM BOOLEAN DEFAULT 0, " &
                    "ConversionNumerator INTEGER DEFAULT 1, " &
                    "ConversionDenominator INTEGER DEFAULT 1, " &
                    "IsActive BOOLEAN NOT NULL DEFAULT 1, " &
                    "FOREIGN KEY(CategoryID) REFERENCES uom_categories(CategoryID));")

                ' 3. product_uom_conversions
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS product_uom_conversions (" &
                    "ConversionID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ProductID INTEGER NOT NULL, " &
                    "FromUoMID INTEGER NOT NULL, " &
                    "ToUoMID INTEGER NOT NULL, " &
                    "ConversionNumerator INTEGER NOT NULL, " &
                    "ConversionDenominator INTEGER NOT NULL, " &
                    "FOREIGN KEY(ProductID) REFERENCES Products(ProductID), " &
                    "FOREIGN KEY(FromUoMID) REFERENCES uoms(UoMID), " &
                    "FOREIGN KEY(ToUoMID) REFERENCES uoms(UoMID), " &
                    "CONSTRAINT uq_product_uom UNIQUE(ProductID, FromUoMID, ToUoMID));")

                ' Seed uom_categories if empty
                Dim catCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM uom_categories"), 0))
                If catCount = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO uom_categories (CategoryName) VALUES (?)", "تعداد (Count)")
                    Sql.ExecuteNonQuery("INSERT INTO uom_categories (CategoryName) VALUES (?)", "وزن (Weight)")
                    Sql.ExecuteNonQuery("INSERT INTO uom_categories (CategoryName) VALUES (?)", "طول (Length)")
                    Sql.ExecuteNonQuery("INSERT INTO uom_categories (CategoryName) VALUES (?)", "حجم (Volume)")
                End If

                ' Seed uoms if empty
                Dim uomCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM uoms"), 0))
                If uomCount = 0 Then
                    ' تعداد
                    Dim catCountId = Convert.ToInt32(Sql.ExecuteScalar("SELECT CategoryID FROM uom_categories WHERE CategoryName = ?", "تعداد (Count)"))
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 1, 1, 1)", catCountId, "عدد", "pcs")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 24, 1)", catCountId, "کارتن ۲۴ تایی", "box-24")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 12, 1)", catCountId, "باکس ۱۲ تایی", "box-12")

                    ' وزن
                    Dim catWeightId = Convert.ToInt32(Sql.ExecuteScalar("SELECT CategoryID FROM uom_categories WHERE CategoryName = ?", "وزن (Weight)"))
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 1, 1, 1)", catWeightId, "گرم", "g")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 1000, 1)", catWeightId, "کیلوگرم", "kg")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 1000000, 1)", catWeightId, "تن", "ton")

                    ' طول
                    Dim catLengthId = Convert.ToInt32(Sql.ExecuteScalar("SELECT CategoryID FROM uom_categories WHERE CategoryName = ?", "طول (Length)"))
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 1, 1, 1)", catLengthId, "میلی‌متر", "mm")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 10, 1)", catLengthId, "سانتی‌متر", "cm")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 1000, 1)", catLengthId, "متر", "m")

                    ' حجم
                    Dim catVolumeId = Convert.ToInt32(Sql.ExecuteScalar("SELECT CategoryID FROM uom_categories WHERE CategoryName = ?", "حجم (Volume)"))
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 1, 1, 1)", catVolumeId, "سی‌سی (میلی‌لیتر)", "cc")
                    Sql.ExecuteNonQuery("INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator) VALUES (?, ?, ?, 0, 1000, 1)", catVolumeId, "لیتر", "L")
                End If
            Catch ex As Exception
                Log("EnsureUnitsOfMeasureTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureProductGroupsTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ProductGroups (" &
                    "GroupID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER NOT NULL, " &
                    "ParentID INTEGER NULL, " &
                    "GroupCode TEXT NOT NULL, " &
                    "GroupName TEXT NOT NULL, " &
                    "Level INTEGER NOT NULL, " &
                    "IsActive INTEGER DEFAULT 1, " &
                    "FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID) ON DELETE CASCADE, " &
                    "FOREIGN KEY (ParentID) REFERENCES ProductGroups(GroupID) ON DELETE CASCADE);"
                )
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_productgroups_company ON ProductGroups(CompanyID);")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_productgroups_parent ON ProductGroups(ParentID);")
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_productgroups_code ON ProductGroups(GroupCode);")
                Log("bootstrap:productgroups-table-ok")
            Catch ex As Exception
                Log("EnsureProductGroupsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureWarehouseTypesTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseTypes (" &
                    "TypeID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "TypeName TEXT UNIQUE NOT NULL);"
                )

                Dim cnt = Convert.ToInt32(Sql.ExecuteScalar("SELECT COUNT(*) FROM WarehouseTypes"))
                If cnt = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('مواد اولیه')")
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('محصول نهایی')")
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('ضایعات')")
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('امانی')")
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('قرنطینه')")
                    Sql.ExecuteNonQuery("INSERT INTO WarehouseTypes (TypeName) VALUES ('قطعات یدکی')")
                End If
            Catch ex As Exception
                Log("EnsureWarehouseTypesTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureWarehouseLocationsTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseLocations (" &
                    "LocationID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "WarehouseID INTEGER NOT NULL, " &
                    "ParentID INTEGER, " &
                    "LocationType INTEGER NOT NULL, " &
                    "Title TEXT NOT NULL, " &
                    "Code TEXT NOT NULL, " &
                    "FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE CASCADE, " &
                    "FOREIGN KEY (ParentID) REFERENCES WarehouseLocations(LocationID) ON DELETE CASCADE);"
                )
            Catch ex As Exception
                Log("EnsureWarehouseLocationsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureModyanCodesTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS ModyanCodes (" &
                    "CodeID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ModyanCode TEXT UNIQUE NOT NULL, " &
                    "Description TEXT, " &
                    "CategoryName TEXT, " &
                    "TaxRate DECIMAL, " &
                    "IsActive BOOLEAN);"
                )
                AddColumnIfMissing("ModyanCodes", "CodeType", "TEXT")
                AddColumnIfMissing("ModyanCodes", "Brand", "TEXT")
                AddColumnIfMissing("ModyanCodes", "TechnicalSpecs", "TEXT")
                AddColumnIfMissing("ModyanCodes", "ParentID", "INTEGER")
            Catch ex As Exception
                Log("EnsureModyanCodesTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureWarehouseReceiptsTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseReceipts (" &
                    "ReceiptID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ReceiptNumber TEXT NOT NULL, " &
                    "ReceiptDate DATETIME, " &
                    "PurchaseInvoiceID INTEGER NOT NULL, " &
                    "CreatedBy INTEGER, " &
                    "WarehouseID INTEGER, " &
                    "Description TEXT);"
                )
                Sql.ExecuteNonQuery("CREATE UNIQUE INDEX IF NOT EXISTS IX_WarehouseReceipts_Number ON WarehouseReceipts (ReceiptNumber)")
                
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS WarehouseReceiptDetails (" &
                    "ReceiptDetailID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ReceiptID INTEGER NOT NULL, " &
                    "PurchaseInvoiceDetailID INTEGER NOT NULL, " &
                    "ProductID INTEGER NOT NULL, " &
                    "Quantity REAL);"
                )
            Catch ex As Exception
                Log("EnsureWarehouseReceiptsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsurePaymentTables()
            Try
                ' جدول پرداخت‌های فاکتور خرید
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PurchaseInvoicePayments (" &
                    "PaymentID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "PurchaseInvoiceID INTEGER NOT NULL, " &
                    "PaymentDate DATE NOT NULL, " &
                    "PaymentType TEXT NOT NULL, " &
                    "Amount REAL NOT NULL, " &
                    "DueDate DATE, " &
                    "Description TEXT, " &
                    "CreatedBy TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_PIPay_Invoice ON PurchaseInvoicePayments (PurchaseInvoiceID)")

                ' جدول چک‌های خرید
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PurchaseChecks (" &
                    "CheckID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "PaymentID INTEGER NOT NULL, " &
                    "CheckNumber TEXT NOT NULL, " &
                    "BankName TEXT, " &
                    "BranchName TEXT, " &
                    "AccountNumber TEXT, " &
                    "Amount REAL NOT NULL, " &
                    "DueDate DATE, " &
                    "Status TEXT NOT NULL DEFAULT 'در جریان', " &
                    "ExchangedWithCheckID INTEGER, " &
                    "BounceFee REAL, " &
                    "Notes TEXT, " &
                    "CreatedBy TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_PCh_Payment ON PurchaseChecks (PaymentID)")

                ' جدول تاریخچه وضعیت چک
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS CheckStatusHistory (" &
                    "HistoryID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CheckID INTEGER NOT NULL, " &
                    "ChangeDate DATE NOT NULL, " &
                    "OldStatus TEXT, " &
                    "NewStatus TEXT NOT NULL, " &
                    "NewCheckID INTEGER, " &
                    "BounceFee REAL, " &
                    "Description TEXT, " &
                    "ChangedBy TEXT, " &
                    "ChangedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
                Sql.ExecuteNonQuery("CREATE INDEX IF NOT EXISTS IX_CSH_Check ON CheckStatusHistory (CheckID)")
            Catch ex As Exception
                Log("EnsurePaymentTables error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsurePersonsTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Persons (" &
                    "PersonID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER NOT NULL, " &
                    "PersonType TEXT NOT NULL DEFAULT 'حقیقی', " & ' حقیقی / حقوقی
                    "RoleType TEXT NOT NULL DEFAULT 'هر دو', " & ' فروشنده / خریدار / هر دو
                    "PersonCode TEXT NOT NULL, " &
                    "FirstName TEXT, " &
                    "LastName TEXT, " &
                    "CompanyName TEXT, " &
                    "NationalCode TEXT, " & ' کدملی / شناسه ملی
                    "EconomicCode TEXT, " & ' کد اقتصادی
                    "RegistrationNumber TEXT, " & ' شماره ثبت
                    "Phone TEXT, " &
                    "Mobile TEXT, " &
                    "Address TEXT, " &
                    "PostalCode TEXT, " &
                    "ShenavarID INTEGER, " & ' لینک مستقیم به حساب شناور (SarfaslShenavar)
                    "IsActive BOOLEAN DEFAULT 1, " &
                    "CreatedBy INTEGER, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                Try
                    Sql.ExecuteNonQuery("ALTER TABLE SarfaslShenavar ADD COLUMN PersonID INTEGER;")
                Catch
                End Try
                Try
                    Sql.ExecuteNonQuery("ALTER TABLE SarfaslShenavar ADD COLUMN CreatedBy INTEGER;")
                Catch
                End Try
            Catch ex As Exception
                Log("EnsurePersonsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureCodStandardTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Cod_Standard (" &
                    "AccountID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "CompanyID INTEGER DEFAULT 0, " &
                    "AccountCode TEXT NOT NULL, " &
                    "AccountName TEXT NOT NULL, " &
                    "AccountType TEXT NOT NULL, " &
                    "ParentAccountID INTEGER, " &
                    "IsActive BOOLEAN DEFAULT 1, " &
                    "AccountNature TEXT);"
                )
                
                Try
                    Sql.ExecuteNonQuery("ALTER TABLE Companies ADD COLUMN CodingType TEXT;")
                Catch
                End Try
            Catch ex As Exception
                Log("EnsureCodStandardTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsureExpensesTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS Expenses (" &
                    "ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "ExpenseDate TEXT, " &
                    "ExpenseTitle TEXT NOT NULL, " &
                    "Category TEXT, " &
                    "Amount REAL DEFAULT 0, " &
                    "PaidTo TEXT, " &
                    "PaymentMethod TEXT, " &
                    "ReferenceNo TEXT, " &
                    "Description TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )
            Catch ex As Exception
                Log("EnsureExpensesTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub EnsurePermissionPresetsTable()
            Try
                Sql.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS PermissionPresets (" &
                    "PresetID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                    "PresetName TEXT NOT NULL UNIQUE, " &
                    "Description TEXT, " &
                    "PermissionsData TEXT, " &
                    "CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP);"
                )

                ' Seed default roles if table is empty
                Dim count = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM PermissionPresets"), 0))
                If count = 0 Then
                    Sql.ExecuteNonQuery("INSERT INTO PermissionPresets (PresetName, Description, PermissionsData) VALUES (?, ?, ?)",
                                        "📦 الگوی انباردار مینی",
                                        "الگوی دسترسی به انبارداری مینی، کالاها، انبارها و فاکتورها",
                                        "AnbarMiniModule,TradeProducts,TradeWarehouses,TradePurchase,TradeSales,TradeReports")

                    Sql.ExecuteNonQuery("INSERT INTO PermissionPresets (PresetName, Description, PermissionsData) VALUES (?, ?, ?)",
                                        "🛒 الگوی صندوق‌دار / فروشنده",
                                        "الگوی دسترسی به فاکتور فروش، کالاها و ثبت فروش سریع",
                                        "AnbarMiniModule,TradeSales,TradeProducts")

                    Sql.ExecuteNonQuery("INSERT INTO PermissionPresets (PresetName, Description, PermissionsData) VALUES (?, ?, ?)",
                                        "📊 الگوی حسابدار ارشد",
                                        "الگوی دسترسی به تمام ماژول‌های حسابداری، کدینگ، اسناد و ترازها",
                                        "ManageAccounting,AccountingHeader,AccountingShenavar,AccountingEntry,AccountingBank,AccountingBalance,AccountingLedger,AccountingReports,AccountingProfitLoss,AccountingBalanceSheet")

                    Sql.ExecuteNonQuery("INSERT INTO PermissionPresets (PresetName, Description, PermissionsData) VALUES (?, ?, ?)",
                                        "🔐 الگوی دسترسی کامل (Full Access)",
                                        "الگوی شامل تمامی دسترسی‌های سیستم",
                                        "ALL")
                End If
            Catch ex As Exception
                Log("EnsurePermissionPresetsTable error: " & ex.Message)
            End Try
        End Sub

        Private Sub Log(message As String)

            Try
                File.AppendAllText(LogPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " " & message & Environment.NewLine)
            Catch
            End Try
        End Sub
    End Module
End Namespace


