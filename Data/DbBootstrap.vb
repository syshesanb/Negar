Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports Sys_Hes_Anb.Business

Namespace Sys_Hes_Anb.Data
    Public Module DbBootstrap
        Private ReadOnly LogPath As String = Path.Combine(Application.StartupPath, "bootstrap.log")

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
            Catch ex As Exception
                Log("bootstrap-error: " & ex.Message & Environment.NewLine & ex.StackTrace)
                Throw
            End Try
        End Sub

        Private Sub EnsureDatabaseFile()
            Dim dataDir = Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory"))
            Log("dataDir=" & dataDir)
            If String.IsNullOrWhiteSpace(dataDir) Then
                Throw New InvalidOperationException("DataDirectory is not configured.")
            End If

            Dim dbFile = Path.Combine(dataDir, "Sys_Hes_Anb.db")
            Dim accdbFile = Path.Combine(dataDir, "Sys_Hes_Anb.accdb")
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
                Tuple.Create("مدیریت کاربران عادی", PermissionKeys.ManageBasicUsers),
                Tuple.Create("مدیریت شرکت‌ها", PermissionKeys.ManageCompanies),
                Tuple.Create("مدیریت سال‌های مالی", PermissionKeys.ManageFiscalYears),
                Tuple.Create("مدیریت شرکت‌ها و سال‌های مالی ( جامع )", PermissionKeys.ManageCompaniesYears),
                Tuple.Create("انتخاب شرکت و سال مالی جاری", PermissionKeys.SelectCompanyFiscalYear),
                Tuple.Create("تنظیمات سیستم", PermissionKeys.ManageSettings),
                Tuple.Create("پشتیبان‌گیری اطلاعات", PermissionKeys.BackupData),
                Tuple.Create("بازیابی اطلاعات", PermissionKeys.RestoreData),
                Tuple.Create("پوسته مشاغل", PermissionKeys.ManageBusinessShells),
                Tuple.Create("امکانات", PermissionKeys.ManageUtilities),
                Tuple.Create("مشاهده دفتر سوابق و گزارش فعالیت‌ها", PermissionKeys.ViewActivityLog),
                Tuple.Create("قطعی‌سازی و قفل اسناد حسابداری", PermissionKeys.LockSanad1),
                Tuple.Create("مخفی کردن ستونهای SF و SH در فرم سند حسابداری", PermissionKeys.HideSFSHInSanad),
                Tuple.Create("حسابداری – سرفصل حسابها", PermissionKeys.AccountingHeader),
                Tuple.Create("حسابداری – حسابهای شناور", PermissionKeys.AccountingShenavar),
                Tuple.Create("حسابداری – ثبت سند حسابداری", PermissionKeys.AccountingEntry),
                Tuple.Create("حسابداری – مغایرات بانکی", PermissionKeys.AccountingBank),
                Tuple.Create("حسابداری – تراز آزمایشی", PermissionKeys.AccountingBalance),
                Tuple.Create("حسابداری – دفتر حساب", PermissionKeys.AccountingLedger),
                Tuple.Create("حسابداری – گزارشات حسابداری", PermissionKeys.AccountingReports),
                Tuple.Create("خرید و فروش – تعریف کالاها و خدمات", PermissionKeys.TradeProducts),
                Tuple.Create("خرید و فروش – تعریف انبارها", PermissionKeys.TradeWarehouses),
                Tuple.Create("خرید و فروش – صدور فاکتور خرید", PermissionKeys.TradePurchase),
                Tuple.Create("خرید و فروش – صدور فاکتور فروش", PermissionKeys.TradeSales),
                Tuple.Create("خرید و فروش – حواله و رسید انبار", PermissionKeys.TradeRemittance),
                Tuple.Create("خرید و فروش – گزارشات انبار و کاردکس کالا", PermissionKeys.TradeReports),
                Tuple.Create("خرید و فروش و انبارداری ( جامع )", PermissionKeys.ManageTradeWarehouse)
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

            ' Ensure SectionName in Permissions
            AddColumnIfMissing("Permissions", "SectionName", "TEXT")

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

        Private Sub Log(message As String)
            Try
                File.AppendAllText(LogPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " " & message & Environment.NewLine)
            Catch
            End Try
        End Sub
    End Module
End Namespace
