# Add XML documentation to enum files that are missing it

Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode" -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' } |
    ForEach-Object {
        $content = Get-Content $_.FullName -Raw

        # Check if it's an enum file and missing documentation
        if ($content -match 'public\s+enum\s+(\w+)' -and $content -notmatch '/// <summary>') {
            $enumName = $Matches[1]

            # Add documentation before the enum declaration
            $content = $content -replace "(public\s+enum\s+$enumName)", @"
/// <summary>
/// Enum $enumName.
/// </summary>
`$1
"@

            # Add documentation for enum members without it
            $content = $content -replace '(\r?\n\s+)([A-Z]\w+)(\s*[,=])', @"
`$1/// <summary>
`$1/// $enumName.`$2 value.
`$1/// </summary>
`$1`$2`$3
"@

            Set-Content $_.FullName $content -NoNewline
            Write-Host "Added XML docs to: $($_.FullName)"
        }
    }

Write-Host "Done adding XML documentation to enums."
