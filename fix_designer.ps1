$filePath = "c:\Sys_Hes_Anb\Forms\Moshtarak\CompanyFiscalYearForm.Designer.vb"
$lines = [System.IO.File]::ReadAllLines($filePath)

$newContent = @"
            Me.tabs.Controls.Add(Me.tabFiscalYears)
            Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabs.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.tabs.Location = New System.Drawing.Point(0, 0)
            Me.tabs.Name = "tabs"
            Me.tabs.SelectedIndex = 0
            Me.tabs.Size = New System.Drawing.Size(900, 650)
            Me.tabs.TabIndex = 0
            '
            'tabSelectActive
            '
            Me.tabSelectActive.Controls.Add(Me.selectSplit)
            Me.tabSelectActive.Controls.Add(Me.pnlSelectBottom)
            Me.tabSelectActive.Location = New System.Drawing.Point(4, 23)
            Me.tabSelectActive.Name = "tabSelectActive"
            Me.tabSelectActive.Size = New System.Drawing.Size(892, 623)
            Me.tabSelectActive.TabIndex = 0
            Me.tabSelectActive.Text = "انتخاب شرکت و سال مالی جاری"
            '
            'selectSplit
            '
            Me.selectSplit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.selectSplit.Location = New System.Drawing.Point(0, 0)
            Me.selectSplit.Name = "selectSplit"
            Me.selectSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'selectSplit.Panel1
            '
            Me.selectSplit.Panel1.Controls.Add(Me.dgvSelectCompanies)
            Me.selectSplit.Panel1.Controls.Add(Me.pnlSelectCompaniesHeader)
            Me.selectSplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            '
            'selectSplit.Panel2
            '
            Me.selectSplit.Panel2.Controls.Add(Me.dgvSelectFiscalYears)
            Me.selectSplit.Panel2.Controls.Add(Me.pnlSelectFiscalYearsHeader)
            Me.selectSplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.selectSplit.Size = New System.Drawing.Size(892, 567)
            Me.selectSplit.SplitterDistance = 300
            Me.selectSplit.TabIndex = 0
            '
            'dgvSelectCompanies
            '
            Me.dgvSelectCompanies.AllowUserToAddRows = False
            Me.dgvSelectCompanies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvSelectCompanies.BackgroundColor = System.Drawing.Color.White
            Me.dgvSelectCompanies.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSelectCompanies.Location = New System.Drawing.Point(0, 30)
            Me.dgvSelectCompanies.MultiSelect = False
            Me.dgvSelectCompanies.Name = "dgvSelectCompanies"
            Me.dgvSelectCompanies.ReadOnly = True
            Me.dgvSelectCompanies.RowHeadersVisible = False
            Me.dgvSelectCompanies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSelectCompanies.Size = New System.Drawing.Size(892, 270)
            Me.dgvSelectCompanies.TabIndex = 0
            '
            'pnlSelectCompaniesHeader
            '
            Me.pnlSelectCompaniesHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlSelectCompaniesHeader.Controls.Add(Me.lblSelectCompaniesTitle)
            Me.pnlSelectCompaniesHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSelectCompaniesHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlSelectCompaniesHeader.Name = "pnlSelectCompaniesHeader"
            Me.pnlSelectCompaniesHeader.Size = New System.Drawing.Size(892, 30)
            Me.pnlSelectCompaniesHeader.TabIndex = 1
            '
            'lblSelectCompaniesTitle
            '
            Me.lblSelectCompaniesTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSelectCompaniesTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSelectCompaniesTitle.ForeColor = System.Drawing.Color.DarkBlue
            Me.lblSelectCompaniesTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblSelectCompaniesTitle.Name = "lblSelectCompaniesTitle"
            Me.lblSelectCompaniesTitle.Size = New System.Drawing.Size(892, 30)
            Me.lblSelectCompaniesTitle.TabIndex = 0
            Me.lblSelectCompaniesTitle.Text = "  لیست شرکتها  (یک شرکت را انتخاب کنید)"
            Me.lblSelectCompaniesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'dgvSelectFiscalYears
            '
            Me.dgvSelectFiscalYears.AllowUserToAddRows = False
            Me.dgvSelectFiscalYears.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
"@

$insertIndex = -1
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match "Me.dgvSelectFiscalYears.BackgroundColor = System.Drawing.Color.White") {
        $insertIndex = $i
        break
    }
}

if ($insertIndex -ne -1) {
    $newLines = @()
    $newLines += $lines[0..($insertIndex-1)]
    $newLines += $newContent -split "`r`n|`n"
    $newLines += $lines[$insertIndex..($lines.Count-1)]
    [System.IO.File]::WriteAllLines($filePath, $newLines, [System.Text.Encoding]::UTF8)
    Write-Host "Injected successfully at line index $insertIndex"
} else {
    Write-Host "Target line not found"
}
