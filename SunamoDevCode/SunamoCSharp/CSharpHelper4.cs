namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
    /// <summary>
    /// Returns the default value for a C# type as an object.
    /// </summary>
    /// <param name="type">C# type name or full name to get the default for.</param>
    /// <returns>Default value for the given type.</returns>
    public static object DefaultValueForTypeObject(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "string":
                return "\"" + "\"";
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
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat argument proto si ušetřím value kódu práci text MSSQL
                return "new(1900, 1, 1)";
            case "char":
                throw new Exception(type);
            case "byte" + "[]":
                // Podporovaný typ pouze value desktopových aplikacích, kde není lsožka sbf
                return null!;
        }

        throw new Exception("Nepodporovaný typ");
    }

    /// <summary>
    /// Wraps the given text in a C# region directive.
    /// </summary>
    /// <param name="text">Text content to wrap.</param>
    /// <param name="regionName">Name for the region.</param>
    /// <returns>Text wrapped in region and endregion directives.</returns>
    public static string WrapWithRegion(string text, string regionName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#region ");
        stringBuilder.AppendLine(regionName);
        stringBuilder.AppendLine(text);
        stringBuilder.AppendLine("#endregion");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Determines whether the given string is a C# keyword. Call CsKeywords.Init before use.
    /// </summary>
    /// <param name="con">String to check against keyword lists.</param>
    /// <returns>True if the string is a C# keyword.</returns>
    public static bool IsKeyword(string con)
    {
        //CsKeywords.Init();
        if (CsKeywordsList.Modifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.AccessModifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Statement.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.MethodParameter.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Namespace.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Operator.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Access.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Literal.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Type.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Contextual.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Query.Contains(con))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Wraps each item in the list with appropriate quotes based on the value type and joins them with commas.
    /// </summary>
    /// <param name="tValue">Type of values to determine quoting style.</param>
    /// <param name="valueS">List of values to wrap and join.</param>
    /// <returns>Comma-separated string of quoted values.</returns>
    public static string WrapWithQuoteList(Type tValue, IList valueS)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in valueS)
        {
            string value = item!.ToString()!;
            WrapWithQuote(tValue, ref value);
            stringBuilder.Append(value + ",");
        }

        return stringBuilder.ToString().TrimEnd(',');
    }

    /// <summary>
    /// Wraps the string value with quotes appropriate for the given type (double quotes for string, single for char).
    /// </summary>
    /// <param name="tKey">Type to determine the quote character.</param>
    /// <param name="keyS">String value to wrap with quotes, modified in place.</param>
    public static void WrapWithQuote(Type tKey, ref string keyS)
    {
        if (tKey == Types.StringType)
        {
            keyS = SH.WrapWithQm(keyS);
        }
        else if (tKey == Types.CharType)
        {
            keyS = SH.WrapWith(keyS, "\'");
        }
        else
        {
        }
    }
}