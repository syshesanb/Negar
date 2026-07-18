$formsDir = "c:\Sys_Hes_Anb\Forms"
$designerFiles = Get-ChildItem -Path $formsDir -Filter "*.Designer.vb" -Recurse

foreach ($designer in $designerFiles) {
    if ($designer.Name -match "HesabdaryCodingForm") { continue }
    
    $content = [System.IO.File]::ReadAllText($designer.FullName, [System.Text.Encoding]::UTF8)
    $matches = [regex]::Matches($content, '(?m)Friend WithEvents (\w+) As (System\.Windows\.Forms\.)?DataGridView$')
    
    $dgvNames = @()
    foreach ($m in $matches) {
        $dgvNames += $m.Groups[1].Value
    }
    
    if ($dgvNames.Count -gt 0) {
        $vbFile = $designer.FullName -replace "\.Designer\.vb", ".vb"
        if (Test-Path $vbFile) {
            $vbContent = [System.IO.File]::ReadAllLines($vbFile, [System.Text.Encoding]::UTF8)
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
                [System.IO.File]::WriteAllLines($vbFile, $newVbContent.ToArray(), [System.Text.Encoding]::UTF8)
                Write-Host "Updated $vbFile"
            }
        }
    }
}
Write-Host "Done"
