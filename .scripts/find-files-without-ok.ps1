$allFiles = Get-ChildItem -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode' -Recurse -Filter '*.cs' | Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' }
$filesWithOk = @()
$allFiles | ForEach-Object {
    $content = Get-Content $_.FullName -TotalCount 1 -ErrorAction SilentlyContinue
    if ($content -match '^// variables names: ok') {
        $filesWithOk += $_.FullName
    }
}
$filesWithoutOk = $allFiles | Where-Object { $filesWithOk -notcontains $_.FullName }
Write-Host "Total files: $($allFiles.Count)"
Write-Host "Files WITH comment: $($filesWithOk.Count)"
Write-Host "Files WITHOUT comment: $($filesWithoutOk.Count)"
$filesWithoutOk.FullName | Out-File -FilePath 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\.scripts\files-without-ok.txt' -Encoding UTF8
Write-Host "List saved to .scripts\files-without-ok.txt"
