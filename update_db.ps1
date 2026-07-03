$connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Sys_Hes_Anb\bin\Debug\Database\Sys_Hes_Anb.accdb;Persist Security Info=False;"
$conn = New-Object System.Data.OleDb.OleDbConnection($connStr)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "ALTER TABLE Companies ADD COLUMN AccountLevels INTEGER, Level1Length INTEGER, Level2Length INTEGER, Level3Length INTEGER, Level4Length INTEGER, Level5Length INTEGER;"
try {
    $cmd.ExecuteNonQuery()
    Write-Host "Columns added successfully."
} catch {
    Write-Host "Error adding columns: $($_.Exception.Message)"
}

$conn.Close()
