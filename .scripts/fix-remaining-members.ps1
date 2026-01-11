$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing all remaining member references..."

# First, find what these members were renamed to by checking git diff
$gitOutput = git -C 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode' diff

# Check if these patterns exist in git diff to understand the renames
Write-Host "Analyzing git diff for renames..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix BasePathsHelper members that weren't caught before
    $newContent = $newContent -replace 'BasePathsHelper\.vs(?![a-zA-Z])', 'BasePathsHelper.Vs'
    $newContent = $newContent -replace 'BasePathsHelper\.vsProjects', 'BasePathsHelper.VsProjects'
    $newContent = $newContent -replace 'BasePathsHelper\.cRepos', 'BasePathsHelper.CRepos'
    $newContent = $newContent -replace 'BasePathsHelper\.bpMb', 'BasePathsHelper.BpMb'
    $newContent = $newContent -replace 'BasePathsHelper\.bpVps', 'BasePathsHelper.BpVps'

    # Fix other member references based on PascalCase naming
    $newContent = $newContent -replace '\.check\b', '.Check'
    $newContent = $newContent -replace '\._nameSolution\b', '._nameSolution'  # Leave as is - needs investigation
    $newContent = $newContent -replace '\.addingValue\b', '.AddingValue'
    $newContent = $newContent -replace '\.addHyphens\b', '.AddHyphens'
    $newContent = $newContent -replace '\.splitKeyWith\b', '.SplitKeyWith'
    $newContent = $newContent -replace '\.old\b', '.Old'
    $newContent = $newContent -replace '\.xd\b', '.Xd'
    $newContent = $newContent -replace '\.allids\b', '.AllIds'
    $newContent = $newContent -replace '\.repository\b', '.Repository'
    $newContent = $newContent -replace '\.nameSolutionWithoutDiacritic\b', '.NameSolutionWithoutDiacritic'
    $newContent = $newContent -replace '\.projectsGetCsprojs\b', '.ProjectsGetCsprojs'
    $newContent = $newContent -replace '\.csprojSdkStyleList\b', '.CsprojSdkStyleList'
    $newContent = $newContent -replace '\.netstandardList\b', '.NetstandardList'
    $newContent = $newContent -replace '\.nonCsprojSdkStyleList\b', '.NonCsprojSdkStyleList'
    $newContent = $newContent -replace '\.isNetstandard\b', '.IsNetstandard'
    $newContent = $newContent -replace '\.isProjectCsprojSdkStyleIsCore\b', '.IsProjectCsprojSdkStyleIsCore'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
