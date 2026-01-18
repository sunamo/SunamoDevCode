namespace SunamoDevCode.SunamoCSharp.Helpers;

/// <summary>
/// Helper class for C# code processing and generation
/// </summary>
public class CSharpHelperSunamo
{
    /// <summary>
    /// Type information for runtime type checking
    /// </summary>
    internal static Type Type = typeof(CSharpHelperSunamo);

    /// <summary>
    /// Checks if all csproj and sln files are in correct hierarchy
    /// </summary>
    /// <param name="path">Root path to search</param>
    /// <param name="textOutputGenerator">Text output generator for results</param>
    /// <returns>String containing report of incorrectly placed files</returns>
    public static string IsAllCsprojAndSlnRightInHiearchy(string path, TextOutputGeneratorDC textOutputGenerator)
    {
        var csprojFiles = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).ToList();
        var slnFiles = Directory.GetFiles(path, "*.sln", SearchOption.AllDirectories).ToList();

        List<string> incorrectSlnFolders = new List<string>();

        for (int i = slnFiles.Count - 1; i >= 0; i--)
        {
            if (!IsOnlyInSpecialOrProjectFolders(slnFiles[i]))
            {
                incorrectSlnFolders.Add(slnFiles[i]);
                slnFiles.RemoveAt(i);
            }
        }

        for (int i = 0; i < slnFiles.Count; i++)
        {
            slnFiles[i] = Path.GetDirectoryName(slnFiles[i]);
        }

        for (int i = csprojFiles.Count - 1; i >= 0; i--)
        {
            var csprojFolder = Path.GetDirectoryName(csprojFiles[i]);
            var slnFolder = Path.GetDirectoryName(csprojFolder);

            if (slnFiles.Contains(slnFolder))
            {
                csprojFiles.RemoveAt(i);
            }
        }

        textOutputGenerator.List(incorrectSlnFolders, "Sln in wrong folder:");
        textOutputGenerator.List(csprojFiles, "Csproj in wrong folder:");

        return textOutputGenerator.ToString();
    }

    /// <summary>
    /// Checks if file is only in special folders (starting with _) or Projects folders
    /// </summary>
    /// <param name="filePath">Path to file to check</param>
    /// <returns>True if file is in valid folder structure</returns>
    private static bool IsOnlyInSpecialOrProjectFolders(string filePath)
    {
        filePath = Path.GetDirectoryName(filePath);

        while (true)
        {
            filePath = Path.GetDirectoryName(filePath);

            if (filePath.Length < 4)
            {
                return false;
            }

            var fileName = Path.GetFileName(filePath);

            if (fileName.EndsWith("Projects"))
            {
                return true;
            }

            if (!fileName.StartsWith("_"))
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Detects string literal ranges in text by finding quote pairs
    /// </summary>
    /// <param name="text">Text to analyze</param>
    /// <returns>List of from-to ranges for string literals</returns>
    public static FromToList DetectFromToString(string text)
    {
        List<int> quoteIndices = null;// SH.ReturnOccurencesOfString(text, "\"");
        for (int i = quoteIndices.Count - 1; i >= 0; i--)
        {
            if (text[quoteIndices[i] - 1] == '\\')
            {
                quoteIndices.RemoveAt(i);
            }
        }

        ThrowEx.HasOddNumberOfElements("quoteIndices", quoteIndices);

        var fromToList = new FromToList();
        for (int i = 0; i < quoteIndices.Count; i++)
        {
            fromToList.Ranges.Add(new FromToDC(quoteIndices[i], quoteIndices[++i]));
        }
        return fromToList;
    }

    /// <summary>
    /// Indents lines to match the indentation of the previous line
    /// </summary>
    /// <param name="lines">Lines to process</param>
    public static void IndentAsPreviousLine(List<string> lines)
    {
        string previousIndent = string.Empty;
        string line = null;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < lines.Count; i++)
        {
            line = lines[i];
            if (line.Length > 0)
            {
                if (!char.IsWhiteSpace(line[0]))
                {
                    lines[i] = previousIndent + lines[i];
                }
                else
                {
                    StringBuilder whitespaceBuilder = new StringBuilder();
                    foreach (var item in line)
                    {
                        if (char.IsWhiteSpace(item))
                        {
                            whitespaceBuilder.Append(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                    previousIndent = stringBuilder.ToString();
                }
            }
        }
    }

    /// <summary>
    /// Checks if type name follows interface naming convention (starts with 'I' followed by uppercase letter)
    /// </summary>
    /// <param name="text">Type name to check</param>
    /// <returns>True if name follows interface convention</returns>
    public static bool IsInterface(string text)
    {
        if (text[0] == 'I')
        {
            if (char.IsUpper(text[1]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes "(null)" text from string
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <returns>Text with "(null)" removed and trimmed</returns>
    public static string ReplaceNulled(string text)
    {
        return text.Replace("(null)", string.Empty).Trim();
    }

    /// <summary>
    /// Creates shortcut from control name by extracting uppercase letters
    /// </summary>
    /// <param name="text">Control name</param>
    /// <returns>Shortcut created from uppercase letters</returns>
    public static string ShortcutForControl(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in text)
        {
            if (char.IsUpper(item))
            {
                stringBuilder.Append(item.ToString().ToLower());
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Gets default value for type T. Not compatible with default operator - must cast manually
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values
    /// </summary>
    /// <typeparam name="T">Type to get default value for</typeparam>
    /// <param name="value">Example value to get type information from</param>
    /// <param name="convertTypeShortcutFullNameToShortcut">Function to convert full type name to shortcut</param>
    /// <returns>Default value for the type</returns>
    public static object DefaultValueForTypeT<T>(T value, Func<string, string> convertTypeShortcutFullNameToShortcut)
    {
        var typeName = value.GetType().FullName;
        if (typeName.Contains("."))
        {
            typeName = convertTypeShortcutFullNameToShortcut(typeName);
        }
        switch (typeName)
        {
            case "string":
                return string.Empty;
            case "bool":
                return false;
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return -1;
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return 0;
            case "DateTime":
                return "new(1900, 1, 1)";
            case "byte[]":
                return null;
            case "Guid":
                return Guid.Empty;
            case "char":
                throw new Exception("Unsupported type");
        }
        throw new Exception("Unsupported type");
    }

    /// <summary>
    /// Gets default value for type as string representation
    /// </summary>
    /// <param name="typeName">Type name</param>
    /// <param name="convertTypeShortcutFullNameToShortcut">Function to convert full type name to shortcut</param>
    /// <returns>String representation of default value</returns>
    public static string DefaultValueForType(string typeName, Func<string, string> convertTypeShortcutFullNameToShortcut)
    {
        if (typeName.Contains("."))
        {
            typeName = convertTypeShortcutFullNameToShortcut(typeName);
        }
        switch (typeName)
        {
            case "string":
                return "\"" + "\"";
            case "bool":
                return "false";
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return "-1";
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return "0";
            case "DateTime":
                return "SqlServerHelper.DateTimeMinVal";
            case "byte[]":
                return "null";
            case "Guid":
                return "Guid.Empty";
            case "char":
                throw new Exception("Unsupported type");
            default:
                // For types like Dictionary<int,int>
                return "new " + typeName + "()";
        }
    }

    /// <summary>
    /// Replaces 'readonly' and 'static readonly' keywords with 'const' keyword
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <returns>Text with readonly keywords replaced by const</returns>
    public static string ReplaceReadonlyToConst(string text)
    {
        text = text.Replace("static readonly", "const");
        text = text.Replace("readonly", "const");
        return text;
    }
}