$formsDir = "c:\Sys_Hes_Anb\Forms"
$vbFiles = Get-ChildItem -Path $formsDir -Filter "*.vb" -Recurse | Where-Object { $_.Name -notmatch "\.Designer\.vb" }

foreach ($vb in $vbFiles) {
    $content = Get-Content $vb.FullName -Encoding UTF8
    $newContent = new-object System.Collections.ArrayList
    $modified = $false
    
    foreach ($line in $content) {
        $newContent.Add($line) > $null
        
        # Match any Form Load method
        if ($line -match "Sub .*_Load\(sender As Object, e As EventArgs\)") {
            $injectLine = "            Sys_Hes_Anb.Business.ThemeHelper.ApplyFormTheme(Me)"
            $found = $false
            
            # Check if already injected
            foreach ($chkLine in $content) {
                if ($chkLine -match "ThemeHelper\.ApplyFormTheme\(Me\)") { $found = $true; break }
            }
            
            if (-not $found) {
                $newContent.Add($injectLine) > $null
                $modified = $true
            }
        }
    }
    
    if ($modified) {
        # Save using .NET UTF8 without BOM to prevent compiler issues
        [System.IO.File]::WriteAllLines($vb.FullName, $newContent.ToArray(), (New-Object System.Text.UTF8Encoding($False)))
        Write-Host "Updated $($vb.FullName)"
    }
}
Write-Host "Done"
