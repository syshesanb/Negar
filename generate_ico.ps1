Add-Type -AssemblyName System.Drawing
$bmp = New-Object System.Drawing.Bitmap(64, 64)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias

$b1 = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(39, 174, 96))
$g.FillRectangle($b1, 8, 4, 48, 56)

$b2 = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
$g.FillRectangle($b2, 14, 10, 36, 10)
$g.FillRectangle($b2, 14, 24, 10, 10)
$g.FillRectangle($b2, 27, 24, 10, 10)
$g.FillRectangle($b2, 40, 24, 10, 10)
$g.FillRectangle($b2, 14, 38, 10, 10)
$g.FillRectangle($b2, 27, 38, 10, 10)
$g.FillRectangle($b2, 40, 38, 10, 10)
$g.FillRectangle($b2, 14, 50, 23, 6)

$h = $bmp.GetHicon()
$ico = [System.Drawing.Icon]::FromHandle($h)
$fs = New-Object System.IO.FileStream('C:\Sys_Hes_Anb\app.ico', [System.IO.FileMode]::Create)
$ico.Save($fs)
$fs.Close()
$bmp.Dispose()
$g.Dispose()
Write-Host "ICO_CREATED_SUCCESSFULLY"
