# Script to rename enum parameters to match their type names in camelCase

$enums = @{
    'AccessModifiers' = 'accessModifiers'
    'RepositoryLocal' = 'repositoryLocal'
    'TypesTs' = 'typesTs'
    'VsProjectTemplateParameters' = 'vsProjectTemplateParameters'
    'WhatIsExcepted' = 'whatIsExcepted'
    'XlfParts' = 'xlfParts'
    'ApsPlugins' = 'apsPlugins'
    'ClassCodeElements' = 'classCodeElements'
    'AspxToEnFiles' = 'aspxToEnFiles'
    'ProjectsOnWhichDependentMode' = 'projectsOnWhichDependentMode'
    'WebNonWeb' = 'webNonWeb'
    'SortByProjectsNames' = 'sortByProjectsNames'
    'SavedStrings' = 'savedStrings'
    'TsToEnFiles' = 'tsToEnFiles'
    'RepairWrongCharsInSourceCodeMode' = 'repairWrongCharsInSourceCodeMode'
    'ItemGroups' = 'itemGroups'
    'SolutionNameMustContains' = 'solutionNameMustContains'
    'CsToEnFiles' = 'csToEnFiles'
    'KeysInConfiguration' = 'keysInConfiguration'
    'Configuration' = 'configuration'
    'SupportedNetFw' = 'supportedNetFw'
    'ProjectsTypes' = 'projectsTypes'
    'SourceOfProjects' = 'sourceOfProjects'
    'TypeOfExtensionDC' = 'typeOfExtensionDC'
    'VariableModifiers' = 'variableModifiers'
    'ObjectInitializationOptions' = 'objectInitializationOptions'
    'GitTypesOfMessages' = 'gitTypesOfMessages'
    'ModifiersConstructor' = 'modifiersConstructor'
    'FromToUseDC' = 'fromToUseDC'
    'LangsDC' = 'langsDC'
}

# Get all C# files except in obj, bin, etc.
$files = Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse | Where-Object {
    $_.FullName -notmatch '\\obj\\' -and
    $_.FullName -notmatch '\\bin\\' -and
    $_.FullName -notmatch 'GlobalUsings\.g\.cs' -and
    $_.FullName -notmatch 'AssemblyInfo\.cs' -and
    $_.FullName -notmatch 'AssemblyAttributes\.cs'
}

$changesReport = @()

foreach ($enumType in $enums.Keys) {
    $expectedName = $enums[$enumType]

    Write-Host "Processing enum: $enumType (expected parameter name: $expectedName)" -ForegroundColor Cyan

    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw -Encoding UTF8
        $originalContent = $content

        # Pattern to find enum parameters with wrong names
        # Match: EnumType paramName where paramName is NOT the expected camelCase name
        # This pattern handles method parameters, lambda parameters, foreach variables
        $pattern = "\b$enumType\s+(?!$expectedName\b)(\w+)\b"

        $matches = [regex]::Matches($content, $pattern)

        if ($matches.Count -gt 0) {
            Write-Host "  Found $($matches.Count) potential issues in: $($file.FullName)" -ForegroundColor Yellow

            foreach ($match in $matches) {
                $wrongName = $match.Groups[1].Value
                $context = $content.Substring([Math]::Max(0, $match.Index - 50), [Math]::Min(100, $content.Length - [Math]::Max(0, $match.Index - 50)))

                Write-Host "    Wrong name: '$wrongName' (should be '$expectedName')" -ForegroundColor Red
                Write-Host "    Context: ...$(($context -replace '\r?\n', ' '))..." -ForegroundColor Gray

                $changesReport += [PSCustomObject]@{
                    File = $file.FullName
                    EnumType = $enumType
                    WrongName = $wrongName
                    CorrectName = $expectedName
                }
            }
        }
    }
}

# Display summary
Write-Host "`nSummary of changes needed:" -ForegroundColor Green
$changesReport | Format-Table -AutoSize

Write-Host "`nTotal files with issues: $(($changesReport | Select-Object -Unique File).Count)" -ForegroundColor Magenta
Write-Host "Total parameters to rename: $($changesReport.Count)" -ForegroundColor Magenta

# Ask for confirmation
$confirm = Read-Host "`nDo you want to proceed with renaming? (yes/no)"

if ($confirm -eq 'yes') {
    Write-Host "`nProceeding with renaming..." -ForegroundColor Green

    # Group changes by file
    $changesByFile = $changesReport | Group-Object -Property File

    foreach ($fileGroup in $changesByFile) {
        $file = $fileGroup.Name
        $content = Get-Content $file -Raw -Encoding UTF8

        # Sort changes by WrongName length (longest first) to avoid partial replacements
        $sortedChanges = $fileGroup.Group | Sort-Object { $_.WrongName.Length } -Descending

        foreach ($change in $sortedChanges) {
            $enumType = $change.EnumType
            $wrongName = $change.WrongName
            $correctName = $change.CorrectName

            # Replace enum parameter declarations
            # Pattern: EnumType wrongName (with word boundaries)
            $pattern = "\b$enumType\s+$wrongName\b"
            $replacement = "$enumType $correctName"
            $content = $content -replace $pattern, $replacement

            # Also replace usages of the wrong name within the same scope
            # This is tricky and might need manual review
            # For now, we'll just replace the parameter declarations
        }

        Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline
        Write-Host "  Updated: $file" -ForegroundColor Green
    }

    Write-Host "`nDone! Please review the changes and update usages of renamed parameters manually." -ForegroundColor Yellow
} else {
    Write-Host "`nCancelled. No changes made." -ForegroundColor Red
}
