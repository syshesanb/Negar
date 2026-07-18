$formsDir = "c:\Sys_Hes_Anb\Forms"
$designerFiles = Get-ChildItem -Path $formsDir -Filter "*.Designer.vb" -Recurse

foreach ($designer in $designerFiles) {
    if ($designer.Name -match "HesabdaryCodingForm") { continue }
    
    $content = Get-Content $designer.FullName -Encoding UTF8
    $dgvNames = new-object System.Collections.ArrayList
    foreach ($line in $content) {
        if ($line -match "Friend WithEvents (\w+) As (System\.Windows\.Forms\.)?DataGridView\s*$") {
            $dgvNames.Add($matches[1]) > $null
        }
    }
    
    if ($dgvNames.Count -gt 0) {
        $vbFile = $designer.FullName -replace "\.Designer\.vb", ".vb"
        if (Test-Path $vbFile) {
            $vbContent = Get-Content $vbFile -Encoding UTF8
            $newVbContent = new-object System.Collections.ArrayList
            $modified = $false
            
            foreach ($line in $vbContent) {
                $newVbContent.Add($line) > $null
                
                if ($line -match "Sub .*_Load\(sender As Object, e As EventArgs\)") {
                    foreach ($dgv in $dgvNames) {
                        $injectLine = "            If Me.$dgv IsNot Nothing Then Me.$dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)"
                        $found = $false
                        foreach ($chkLine in $vbContent) {
                            if ($chkLine -match "Me\.$dgv\.AlternatingRowsDefaultCellStyle") { $found = $true; break }
                        }
                        if (-not $found) {
                            $newVbContent.Add($injectLine) > $null
                            $modified = $true
                        }
                    }
                }
            }
            
            if ($modified) {
                # Save using .NET UTF8 without BOM to prevent compiler issues
                [System.IO.File]::WriteAllLines($vbFile, $newVbContent.ToArray(), (New-Object System.Text.UTF8Encoding($False)))
                Write-Host "Updated $vbFile"
            }
        }
    }
}
Write-Host "Done"
