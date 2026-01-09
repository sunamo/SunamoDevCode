$files = Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode" -Filter "*.cs" -Recurse |
    Where-Object {
        $_.Name -ne "GlobalUsings.cs" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -notmatch "\\bin\\"
    }

$filesToFix = @()

foreach ($file in $files) {
    $firstLine = Get-Content $file.FullName -First 1 -ErrorAction SilentlyContinue
    if ($firstLine -ne "// variables names: ok") {
        $filesToFix += $file.FullName
    }
}

$filesToFix | Sort-Object | ForEach-Object { Write-Host $_ }
Write-Host ""
Write-Host "Total files needing fixes: $($filesToFix.Count)"
