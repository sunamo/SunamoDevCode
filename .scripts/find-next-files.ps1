$files = Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode" -Filter "*.cs" -Recurse -File |
    Where-Object { $_.FullName -notmatch "\\obj\\" -and $_.FullName -notmatch "\\bin\\" } |
    Where-Object {
        $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
        $content -and $content -notmatch "// variables names: ok"
    } |
    Select-Object -First 40

foreach ($file in $files) {
    Write-Host $file.FullName.Replace("E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\", "")
}

Write-Host "`nTotal files found: $($files.Count)"
