# Fix all broken references from incomplete batch replacements

$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

# Fix BasePathsHelper references
Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix BasePathsHelper properties
    $newContent = $newContent -replace 'BasePathsHelper\.vs([^A-Z])', 'BasePathsHelper.Vs$1'
    $newContent = $newContent -replace 'BasePathsHelper\.vsProjects', 'BasePathsHelper.VsProjects'
    $newContent = $newContent -replace 'BasePathsHelper\.cRepos', 'BasePathsHelper.CRepos'
    $newContent = $newContent -replace 'BasePathsHelper\.actualPlatform', 'BasePathsHelper.ActualPlatform'
    $newContent = $newContent -replace 'BasePathsHelper\.bpMb', 'BasePathsHelper.BpMb'
    $newContent = $newContent -replace 'BasePathsHelper\.bpQ', 'BasePathsHelper.BpQ'
    $newContent = $newContent -replace 'BasePathsHelper\.bpVps', 'BasePathsHelper.BpVps'
    $newContent = $newContent -replace 'BasePathsHelper\.bpBb', 'BasePathsHelper.BpBb'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "BasePathsHelper references fixed."
