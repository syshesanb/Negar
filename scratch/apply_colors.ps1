$files = Get-ChildItem -Path "c:\Sys_Hes_Anb\Forms" -Filter "*.Designer.vb" -Recurse
foreach ($f in $files) {
    if ($f.Name -match "HesabdaryCodingForm") { continue }
    
    $content = Get-Content $f.FullName
    $dgvNames = ($content | Select-String -Pattern "Friend WithEvents (\w+) As DataGridView" | ForEach-Object { $_.Matches.Groups[1].Value })
    
    if ($dgvNames) {
        $newContent = new-object System.Collections.ArrayList
        $modified = $false
        foreach ($line in $content) {
            $newContent.Add($line) > $null
            foreach ($dgv in $dgvNames) {
                if ($line -match "Me\.$dgv\.Name\s*=\s*`"$dgv`"") {
                    $newContent.Add("            Me.$dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)") > $null
                    $modified = $true
                }
            }
        }
        if ($modified) {
            Set-Content -Path $f.FullName -Value $newContent -Encoding UTF8
            Write-Host "Updated $($f.Name)"
        }
    }
}
Write-Host "Done"
