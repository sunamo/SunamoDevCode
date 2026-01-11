$files = Get-ChildItem -Path "$PSScriptRoot\..\SunamoDevCode" -Recurse -Filter "*.cs" | Where-Object {
    $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\'
}

$withoutOk = @()

foreach ($file in $files) {
    $firstLine = Get-Content $file.FullName -First 1 -ErrorAction SilentlyContinue
    if ($firstLine -notmatch '^\s*// variables names: ok') {
        $withoutOk += $file.FullName
    }
}

Write-Host "Files WITHOUT comment: $($withoutOk.Count)"
Write-Host ""

foreach ($filePath in $withoutOk) {
    Write-Host $filePath
}
