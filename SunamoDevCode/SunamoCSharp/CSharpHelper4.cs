namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
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
                return 0;
            case "byte" + "[]":
                // Podporovaný typ pouze value desktopových aplikacích, kde není lsožka sbf
                return null;
        }

        throw new Exception("Nepodporovaný typ");
        return null;
    }

    public static string WrapWithRegion(string text, string value)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#region ");
        stringBuilder.AppendLine(value);
        stringBuilder.AppendLine(text);
        stringBuilder.AppendLine("#endregion");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// call CsKeywords.Init before use
    /// </summary>
    /// <param name = "con"></param>
    /// <returns></returns>
    public static bool IsKeyword(string con)
    {
        //CsKeywords.Init();
        if (CsKeywordsList.modifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.accessModifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.statement.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.methodParameter.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList._namespace.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList._operator.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.access.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.literal.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.type.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.contextual.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.query.Contains(con))
        {
            return true;
        }

        return false;
    }

    public static string WrapWithQuoteList(Type tValue, IList valueS)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in valueS)
        {
            var value = item.ToString();
            WrapWithQuote(tValue, ref value);
            stringBuilder.Append(value + ",");
        }

        return stringBuilder.ToString().TrimEnd(',');
    }

    public static void WrapWithQuote(Type tKey, ref string keyS)
    {
        if (tKey == Types.tString)
        {
            keyS = SH.WrapWithQm(keyS);
        }
        else if (tKey == Types.tChar)
        {
            keyS = SH.WrapWith(keyS, "\'");
        }
        else
        {
        }
    }
}