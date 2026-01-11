# Comprehensive fix for all camelCase to PascalCase conversions
$rootPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

$replacements = @{
    # Static class fields
    "AllExtensions\.Sln" = "AllExtensions.SlnExtension"
    "AllExtensions\.json" = "AllExtensions.JsonExtension"
    "BasePathsHelper\.vs" = "BasePathsHelper.Vs"
    "SNumConsts\.mTwo" = "SNumConsts.MinusTwo"
    "ApsHelper\.ci" = "ApsHelper.Instance"
    "WhitespaceCharService\.whiteSpaceChars" = "WhitespaceCharService.WhiteSpaceChars"
    "Types\.tString" = "Types.StringType"

    # Unindexable properties
    "Unindexable\.unindexablePathStarts" = "Unindexable.UnindexablePathStarts"
    "Unindexable\.unindexablePathParts" = "Unindexable.UnindexablePathParts"
    "Unindexable\.unindexablePathEnds" = "Unindexable.UnindexablePathEnds"
    "Unindexable\.unindexableFileNames" = "Unindexable.UnindexableFileNames"

    # SolutionFolder properties
    "\.typeProjectFolder" = ".TypeProjectFolder"

    # IsProjectCsprojSdkStyleResult properties
    "\.isNetstandard" = ".IsNetstandard"

    # ItemGroup properties
    "\.type\b" = ".Type"

    # XmlDocumentsCache
    "XmlDocumentsCache\.cantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull" = "XmlDocumentsCache.CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull"

    # GetFilesDCArgs - already fixed by linter but catch any remaining
    "\.OnlyIn_Sunamo" = ".OnlyInSunamo"

    # ReferenceItemGroup and ProjectReferenceItemGroup
    "ReferenceItemGroup\.type" = "ReferenceItemGroup.Type"
    "ProjectReferenceItemGroup\.type" = "ProjectReferenceItemGroup.Type"

    # VpsHelperDevCode
    "VpsHelperDevCode\.listVpsNew" = "VpsHelperDevCode.ListVpsNew"
}

# Get all .cs files excluding obj and bin
$csFiles = Get-ChildItem -Path $rootPath -Filter "*.cs" -Recurse | Where-Object {
    $_.FullName -notmatch "\\obj\\" -and
    $_.FullName -notmatch "\\bin\\"
}

$filesModified = 0
$totalReplacements = 0

foreach ($file in $csFiles) {
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    if ($null -eq $content) { continue }

    $originalContent = $content
    $fileReplacements = 0

    foreach ($pattern in $replacements.Keys) {
        $replacement = $replacements[$pattern]
        $matches = ([regex]::Matches($content, $pattern)).Count
        if ($matches -gt 0) {
            $content = $content -replace $pattern, $replacement
            $fileReplacements += $matches
        }
    }

    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
        $filesModified++
        $totalReplacements += $fileReplacements
        Write-Host "Modified: $($file.Name) ($fileReplacements replacements)"
    }
}

Write-Host "`nTotal files modified: $filesModified"
Write-Host "Total replacements: $totalReplacements"
