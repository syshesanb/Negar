$dllPath = "c:\Sys_Hes_Anb\bin\Debug\System.Data.SQLite.dll"
$dbPath = "c:\Sys_Hes_Anb\bin\Debug\Database\Sys_Hes_Anb.dat"
if (Test-Path $dllPath) {
    [System.Reflection.Assembly]::LoadFrom($dllPath) | Out-Null
    $connString = "Data Source=$dbPath;Version=3;"
    $conn = New-Object System.Data.SQLite.SQLiteConnection($connString)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "DELETE FROM AppSettings WHERE SettingKey IN ('AboutText', 'ContactText');"
    $rows = $cmd.ExecuteNonQuery()
    $conn.Close()
    Write-Host "Deleted $rows rows from AppSettings."
} else {
    Write-Host "SQLite DLL not found at $dllPath"
}
