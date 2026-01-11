$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing all remaining member references..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix SolutionFolderSerialize members
    $newContent = $newContent -replace '\.displayedText\b', '.DisplayedText'
    $newContent = $newContent -replace '\._displayedText\b', '._DisplayedText'
    $newContent = $newContent -replace '\._fullPathFolder\b', '._FullPathFolder'
    $newContent = $newContent -replace '\._nameSolution\b', '._NameSolution'
    $newContent = $newContent -replace 'SolutionFolderSerialize\.type\b', 'SolutionFolderSerialize.Type'

    # Fix XlfData members
    $newContent = $newContent -replace '(?<=XlfData[^.]*\.)path\b', 'Path'
    $newContent = $newContent -replace '(?<=XlfData[^.]*\.)group\b', 'Group'
    $newContent = $newContent -replace '(?<=XlfData[^.]*\.)xd\b', 'XmlDocument'
    $newContent = $newContent -replace '(?<=XlfData[^.]*\.)trans_units\b', 'TransUnits'
    $newContent = $newContent -replace '(?<=XlfData[^.]*\.)allids\b', 'AllIds'

    # Fix specific patterns that regex above might miss
    $newContent = $newContent -replace '\.xd\.', '.XmlDocument.'
    $newContent = $newContent -replace '\.allids\b', '.AllIds'
    $newContent = $newContent -replace '\.trans_units\b', '.TransUnits'

    # Fix more member references
    $newContent = $newContent -replace '\.repository\b', '.Repository'
    $newContent = $newContent -replace '\.nameSolutionWithoutDiacritic\b', '.NameSolutionWithoutDiacritic'
    $newContent = $newContent -replace '\.projectsGetCsprojs\b', '.ProjectsGetCsprojs'
    $newContent = $newContent -replace '\.splitKeyWith\b', '.SplitKeyWith'
    $newContent = $newContent -replace '\.addingValue\b', '.AddingValue'
    $newContent = $newContent -replace '\.addHyphens\b', '.AddHyphens'
    $newContent = $newContent -replace '\.old\b', '.Old'

    # Fix list members
    $newContent = $newContent -replace '\.csprojSdkStyleList\b', '.CsprojSdkStyleList'
    $newContent = $newContent -replace '\.netstandardList\b', '.NetstandardList'
    $newContent = $newContent -replace '\.nonCsprojSdkStyleList\b', '.NonCsprojSdkStyleList'
    $newContent = $newContent -replace '\.isNetstandard\b', '.IsNetstandard'
    $newContent = $newContent -replace '\.isProjectCsprojSdkStyleIsCore\b', '.IsProjectCsprojSdkStyleIsCore'

    # Fix BasePathsHelper - more thorough
    $newContent = $newContent -replace 'BasePathsHelper\.vs\s', 'BasePathsHelper.Vs '
    $newContent = $newContent -replace 'BasePathsHelper\.vs\)', 'BasePathsHelper.Vs)'
    $newContent = $newContent -replace 'BasePathsHelper\.vs;', 'BasePathsHelper.Vs;'
    $newContent = $newContent -replace 'BasePathsHelper\.vs,', 'BasePathsHelper.Vs,'
    $newContent = $newContent -replace 'BasePathsHelper\.vs\+', 'BasePathsHelper.Vs+'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
