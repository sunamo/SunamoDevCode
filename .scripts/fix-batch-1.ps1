# Fix ConstsManager.cs - remove unused Type variable and fix loop variable

$file = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ConstsManager.cs"
$content = Get-Content $file -Raw

# Remove unused 'type' variable
$content = $content -replace 'private readonly Func<string, bool> XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys;\r?\n    private static Type type = typeof\(ConstsManager\);', 'private readonly Func<string, bool> XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys;'

# Fix 'j' to 'characterIndex' in loop
$content = $content -replace 'for \(int j = 0; j < trimmedItem\.Length; j\+\+\)', 'for (int characterIndex = 0; characterIndex < trimmedItem.Length; characterIndex++)'
$content = $content -replace 'j--;', 'characterIndex--;'

# Improve comment
$content = $content -replace '// Inlined from SHTrim\.TrimLeadingNumbersAtStart - odstraňuje číslice ze začátku řetězce', '// EN: Inline from SHTrim.TrimLeadingNumbersAtStart - removes digits from start of string
                // CZ: Inline z SHTrim.TrimLeadingNumbersAtStart - odstraňuje číslice ze začátku řetězce'

# Fix AddConst method params and docs
$content = $content -replace '    /// <summary>\r?\n    ///     Add c# const code\r?\n    /// </summary>\r?\n    /// <param name="csg"></param>\r?\n    /// <param name="item"></param>\r?\n    private static void AddConst\(CSharpGenerator csg, string item, string val\)', @'
    /// <summary>
    /// EN: Add C# const code to the generator
    /// CZ: Přidá C# const kód do generátoru
    /// </summary>
    /// <param name="generator">CSharp code generator instance</param>
    /// <param name="constantName">Name of the constant to add</param>
    /// <param name="constantValue">Value of the constant</param>
    private static void AddConst(CSharpGenerator generator, string constantName, string constantValue)'@

# Fix method body
$content = $content -replace 'csg\.Field\(1, AccessModifiers\.Public, true, VariableModifiers\.Mapped, "string", item, true, val\);', 'generator.Field(1, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", constantName, true, constantValue);'

# Fix method call
$content = $content -replace 'AddConst\(csg, append \+ trimmedItem, val\);', 'AddConst(csg, append + trimmedItem, constantValue);'

# Fix variable names in AddKeysConsts
$content = $content -replace 'var val = valuesAll\[i\];', 'var constantValue = valuesAll[i];'

Set-Content -Path $file -Value $content -NoNewline
Write-Host "Fixed ConstsManager.cs"
