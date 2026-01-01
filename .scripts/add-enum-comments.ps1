# Skript pro přidání "// variables names: ok" komentáře do všech enum souborů
# Script to add "// variables names: ok" comment to all enum files

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"

# Najdi všechny .cs soubory kromě obj/bin složek
$csFiles = Get-ChildItem -Path "$projectRoot\SunamoDevCode" -Filter "*.cs" -Recurse -File |
    Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' }

$modifiedCount = 0

foreach ($file in $csFiles) {
    $content = Get-Content $file.FullName -Raw

    # Kontrola zda soubor obsahuje pouze enum (a žádné class/interface/struct)
    $hasEnum = $content -match '\benum\s+\w+'
    $hasClass = $content -match '\bclass\s+\w+'
    $hasInterface = $content -match '\binterface\s+\w+'
    $hasStruct = $content -match '\bstruct\s+\w+'

    # Pokud má enum a NEMÁ class/interface/struct A ještě NEMÁ komentář
    if ($hasEnum -and -not $hasClass -and -not $hasInterface -and -not $hasStruct -and
        $content -notmatch '// variables names: ok') {

        # Přidej komentář na začátek
        $newContent = "// variables names: ok`n" + $content
        Set-Content -Path $file.FullName -Value $newContent -NoNewline

        Write-Host "Added comment to: $($file.FullName)"
        $modifiedCount++
    }
}

Write-Host "`nTotal files modified: $modifiedCount"
