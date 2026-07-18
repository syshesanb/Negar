$formsDir = "c:\Sys_Hes_Anb\Forms"
$designerFiles = Get-ChildItem -Path $formsDir -Filter "*.Designer.vb" -Recurse

foreach ($designer in $designerFiles) {
    if ($designer.Name -match "HesabdaryCodingForm") { continue }
    
    $content = Get-Content $designer.FullName
    $dgvNames = ($content | Select-String -Pattern "Friend WithEvents (\w+) As System\.Windows\.Forms\.DataGridView$|Friend WithEvents (\w+) As DataGridView$" | ForEach-Object { 
        if ($_.Matches.Groups[1].Value) { $_.Matches.Groups[1].Value } else { $_.Matches.Groups[2].Value }
    })
    
    if ($dgvNames) {
        $vbFile = $designer.FullName -replace "\.Designer\.vb", ".vb"
        if (Test-Path $vbFile) {
            $vbContent = Get-Content $vbFile
            $newVbContent = new-object System.Collections.ArrayList
            $modified = $false
            $inLoad = $false
            
            foreach ($line in $vbContent) {
                $newVbContent.Add($line) > $null
                
                if ($line -match "Sub .*_Load\(sender As Object, e As EventArgs\) Handles MyBase\.Load") {
                    foreach ($dgv in $dgvNames) {
                        # Check if already injected
                        $injectLine = "            Me.$dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 248, 255)"
                        if ($vbContent -notcontains $injectLine) {
                            $newVbContent.Add($injectLine) > $null
                            $modified = $true
                        }
                    }
                }
            }
            
            if ($modified) {
                Set-Content -Path $vbFile -Value $newVbContent -Encoding UTF8
                Write-Host "Updated $vbFile"
            }
        }
    }
}
Write-Host "Done"
