Get-ChildItem -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode' -Filter '*.csproj' -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    if ($content -match '<NoWarn>(.+?)</NoWarn>') {
        Write-Host "$($_.Name): $($Matches[1])" -ForegroundColor Yellow
    } else {
        Write-Host "$($_.Name): (empty)" -ForegroundColor Green
    }
}
