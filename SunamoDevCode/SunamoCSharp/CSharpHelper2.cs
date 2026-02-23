namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
    /// <summary>
    /// Generates C# code for populating a dictionary from two parallel lists of keys and values.
    /// </summary>
    /// <typeparam name = "Key">Type of dictionary keys.</typeparam>
    /// <typeparam name = "Value">Type of dictionary values.</typeparam>
    /// <param name = "tabCount">Number of tabs for indentation.</param>
    /// <param name = "nameDictionary">Name of the dictionary variable.</param>
    /// <param name = "keys">List of keys.</param>
    /// <param name = "values">List of values.</param>
    /// <param name = "argument">Arguments controlling code generation behavior (e.g., splitKeyWith).</param>
    /// <returns>Generated C# code string.</returns>
    public static string GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs argument) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromTwoList<Key, Value>(tabCount, nameDictionary, keys, values, argument);
        return csg.ToString();
    }

    /// <summary>
    /// Generates C# code for dictionary value assignments using random values from a factory function.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys.</typeparam>
    /// <typeparam name="Value">Type of dictionary values.</typeparam>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="nameDictionary">Variable name of the dictionary.</param>
    /// <param name="keys">List of dictionary keys.</param>
    /// <param name="randomValue">Factory function that produces random values.</param>
    /// <returns>Generated C# code string.</returns>
    public static string GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        return csg.ToString();
    }

    //public static string GetDictionary(string nameDictionary)
    //{
    //}
    /// <summary>
    /// Removes empty XML doc comment tags (like empty returns) except summary blocks.
    /// </summary>
    /// <param name="list">Source code lines to process.</param>
    /// <param name="removedAnything">Output flag indicating whether anything was removed.</param>
    /// <returns>Joined string of the modified lines.</returns>
    public static string RemoveXmlDocCommentsExceptSummary(List<string> list, ref bool removedAnything)
    {
        removedAnything = false;
        const string argument = @"/// <returns></returns>";
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == argument)
            {
                removedAnything = true;
                list.RemoveAt(i);
            }
        }

        return string.Join(Environment.NewLine, list); //string.Join(Environment.NewLine, list);
    }

    /// <summary>
    /// Removes all XML documentation comments from the source code lines.
    /// </summary>
    /// <param name="list">Source code lines to process.</param>
    /// <returns>Cleaned source code with comments removed.</returns>
    public static string RemoveXmlDocComments(List<string> list)
    {
        ThrowEx.NotImplementedMethod();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "///");
        }

        CA.TrimWhereIsOnlyWhitespace(list);
        var text = string.Join(Environment.NewLine, list);
        CA.DoubleOrMoreMultiLinesToSingle(ref text);
        return text;
    }

    /// <summary>
    /// Removes line and/or block comments from source code text.
    /// </summary>
    /// <param name="listOrString">Source code text to process.</param>
    /// <param name="line">Whether to remove single-line comments.</param>
    /// <param name="block">Whether to remove block comments.</param>
    /// <param name="keepLinesNumbers">Whether to preserve line numbers by keeping newlines.</param>
    /// <returns>Source code with comments removed.</returns>
    public static string RemoveComments(string listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return RemoveCommentsKeepLines(listOrString);
        }

        return SHJoin.JoinNL(RemoveComments(SHGetLines.GetLines(listOrString), line, block));
    }

    private static string RemoveCommentsKeepLines(string input)
    {
        string output = "";
        using (StringReader reader = new(input))
        {
            bool inSingleLineComment = false;
            bool inMultiLineComment = false;
            int currentChar;
            while ((currentChar = reader.Read()) != -1)
            {
                char current = (char)currentChar;
                char next = (char)reader.Peek();
                if (inSingleLineComment)
                {
                    if (current == '\n')
                    {
                        inSingleLineComment = false;
                        output += current;
                    }
                }
                else if (inMultiLineComment)
                {
                    if (current == '*' && next == '/')
                    {
                        inMultiLineComment = false;
                        reader.Read(); // Přečteme '/'
                    }
                    else if (current == '\n')
                    {
                        output += current; // Přidáme nový řádek do výstupu
                    }
                }
                else
                {
                    if (current == '/' && next == '/')
                    {
                        inSingleLineComment = true;
                        reader.Read(); // Přečteme '/'
                    }
                    else if (current == '/' && next == '*')
                    {
                        inMultiLineComment = true;
                        reader.Read(); // Přečteme '*'
                    }
                    else
                    {
                        output += current;
                    }
                }
            }
        }

        return output;
    }

    /// <summary>
    /// Removes C# line comments and/or block comments from the given source code lines.
    /// </summary>
    /// <param name = "listOrString">Source code lines to process.</param>
    /// <param name = "line">Whether to remove single-line comments.</param>
    /// <param name = "block">Whether to remove block comments.</param>
    /// <param name = "keepLinesNumbers">Whether to keep original line numbers (replace comments with empty lines instead of removing).</param>
    /// <returns>Processed source code lines with comments removed.</returns>
    public static List<string> RemoveComments(List<string> listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return SHGetLines.GetLines(RemoveCommentsKeepLines(SHJoin.JoinNL(listOrString)));
        }

        if (line)
        {
            listOrString = RemoveLineComments(listOrString);
        }

        if (block)
        {
            listOrString = SHGetLines.GetLines(RemoveBlockComments(string.Join(Environment.NewLine, listOrString)));
        }

        //var list = CastHelper.ToListString(listOrString);
        //jak jsem mohl být takový debil argument dát to tady - na co tady trimovat?
        //CA.Trim(listOrString);
        return listOrString;
    }

    /// <summary>
    /// Direct edit
    /// </summary>
    /// <param name = "list"></param>
    /// <returns></returns>
    public static List<string> RemoveLineComments(List<string> list)
    {
        //List<string> list = CastHelper.ToListString(listOrString);
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "//");
        }

        CA.RemoveStringsEmpty2(list);
        return list;
    }

    const string blockComments = @"/\*(.*?)\*/";
    const string lineComments = @"//(.*?)\r?\n";
    const string strings = @"""((\\[^\n]|[^""\n])*)""";
    const string verbatimStrings = @"@(""[^""]*"")+";
    /// <summary>
    /// Removes block comments (/* ... */) and line comments (//) from source code using regex.
    /// </summary>
    /// <param name="str">Source code text to process.</param>
    /// <returns>Text with block and line comments removed.</returns>
    public static string RemoveBlockComments(string str)
    {
        //var str = CastHelper.ToString(listOrString);
        //for (int i = list.Count - 1; i >= 0; i--)
        //{
        //    list[i] = SHParts.RemoveAfterFirst(list[i], CSharp""");
        //}
        str = Regex.Replace(str, blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings, me =>
        {
            if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                return me.Value.StartsWith("//") ? Environment.NewLine : "";
            // Keep the literal strings
            return me.Value;
        }, RegexOptions.Singleline);
        //CA.RemoveStringsEmpty2(list);
        return str;
    }

    /// <summary>
    /// Generates C# code for creating and populating a Dictionary with string keys and typed values.
    /// </summary>
    /// <typeparam name="Value">Type of dictionary values.</typeparam>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="keys">List of string keys.</param>
    /// <param name="values">List of values.</param>
    /// <param name="nameDictionary">Variable name for the dictionary.</param>
    /// <param name="argument">Code generation arguments.</param>
    /// <returns>Generated C# code string.</returns>
    public static string GetDictionaryStringObject<Value>(int tabCount, List<string> keys, List<Value> values, string nameDictionary, CSharpGeneratorArgs argument)
    {
        int pocetTabu = 0;
        CSharpGenerator gen = new CSharpGenerator();
        gen.DictionaryFromTwoList<string, Value>(tabCount, nameDictionary, keys, values, argument);
        if (argument.CheckForNull)
        {
            gen.If(pocetTabu, nameDictionary + " " + "== null");
        }

        gen.GetDictionaryValuesFromDictionary<string, Value>(pocetTabu, nameDictionary, DictionaryHelper.GetDictionary<string, Value>(keys, values));
        if (argument.CheckForNull)
        {
            gen.EndBrace(pocetTabu);
        }

        string result = gen.ToString();
        return result;
    }

    /// <summary>
    /// Returns the default value string for a SQLite data type.
    /// </summary>
    /// <param name="type">SQLite type name (TEXT, INTEGER, REAL, DATETIME, BLOB).</param>
    /// <returns>Default value string for the given type.</returns>
    public static string DefaultValueForTypeSqLite(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "TEXT":
                return "\"";
            case "INTEGER":
                return int.MaxValue.ToString();
            case "REAL":
                return "0.0";
            case "DATETIME":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat argument proto si ušetřím value kódu práci text MSSQL
                return "DateTime.MinValue";
            case "BLOB":
                // Podporovaný typ pouze value desktopových aplikacích, kde není lsožka sbf
                return "null";
            default:
                ThrowEx.NotImplementedCase(type);
                break;
        }

        throw new Exception("Nepodporovaný typ");
    }
}