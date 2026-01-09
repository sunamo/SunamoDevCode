$rootPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

Write-Host "Fixing usedRepository -> UsedRepository..." -ForegroundColor Cyan
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.usedRepository\b'
}
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.usedRepository\b', '.UsedRepository'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "  Fixed in: $($file.Name)" -ForegroundColor Green
    }
}

Write-Host "Fixing FromToDC.Empty instance access..." -ForegroundColor Cyan
$file = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\_public\SunamoData\Data\FromToDC.cs"
$content = Get-Content $file -Raw
# The issue is that Empty is both a static field and being set as instance. Need a private instance field.
$oldCode = @"
    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToDC(bool empty)
    {
        this.Empty = empty;
    }
"@

$newCode = @"
    private bool _empty;

    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToDC(bool empty)
    {
        this._empty = empty;
    }
"@

$newContent = $content -replace [regex]::Escape($oldCode), $newCode
if ($content -ne $newContent) {
    Set-Content $file -Value $newContent -NoNewline
    Write-Host "  Fixed FromToDC.Empty issue" -ForegroundColor Green
}

Write-Host "Done!" -ForegroundColor Yellow
