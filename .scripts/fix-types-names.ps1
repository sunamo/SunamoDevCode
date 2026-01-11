$files = @(
    'SunamoCSharp\CSharpHelper3.cs',
    'SunamoCSharp\CSharpHelper4.cs',
    'Aps\Projs\SunamoCsprojHelper1.cs',
    'CSharpGenerator3.cs'
)

$basePath = "$PSScriptRoot\..\SunamoDevCode"

foreach ($file in $files) {
    $fullPath = Join-Path $basePath $file
    Write-Host "Processing: $fullPath"

    $content = Get-Content $fullPath -Raw

    # Replace all old type names with new PascalCase names
    $content = $content -replace '\btString\b', 'StringType'
    $content = $content -replace '\btChar\b', 'CharType'
    $content = $content -replace '\btInt\b', 'IntType'
    $content = $content -replace '\btDouble\b', 'DoubleType'
    $content = $content -replace '\btFloat\b', 'FloatType'
    $content = $content -replace '\btBool\b', 'BoolType'
    $content = $content -replace '\btObject\b', 'ObjectType'
    $content = $content -replace '\btDateTime\b', 'DateTimeType'
    $content = $content -replace '\btLong\b', 'LongType'
    $content = $content -replace '\btShort\b', 'ShortType'
    $content = $content -replace '\btDecimal\b', 'DecimalType'
    $content = $content -replace '\btByte\b', 'ByteType'
    $content = $content -replace '\btSbyte\b', 'SbyteType'
    $content = $content -replace '\btUshort\b', 'UshortType'
    $content = $content -replace '\btUint\b', 'UintType'
    $content = $content -replace '\btUlong\b', 'UlongType'
    $content = $content -replace '\btBinary\b', 'BinaryType'
    $content = $content -replace '\btStringBuilder\b', 'StringBuilderType'
    $content = $content -replace '\btIEnumerable\b', 'IEnumerableType'
    $content = $content -replace '\btGuid\b', 'GuidType'
    $content = $content -replace 'Types\.list\b', 'Types.ListType'
    $content = $content -replace 'Types\.allBasicTypes\b', 'Types.AllBasicTypes'

    Set-Content -Path $fullPath -Value $content -NoNewline
    Write-Host "Fixed: $file"
}

Write-Host "All files fixed!"
