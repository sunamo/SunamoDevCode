namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
    /// <summary>
    /// Generates constructor body assignments in the form "this.field = field;" for each value.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="values">List of field names to assign.</param>
    /// <returns>Formatted constructor inner body string.</returns>
    public static string GetCtorInner(int tabCount, IList values)
    {
        const string assignVariable = "this.{0} = {0};";
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in values)
        {
            csg.AppendLine(tabCount, string.Format(assignVariable, item));
        }

        return csg.ToString().Trim();
    }

    /// <summary>
    /// Return also List because Array isn't use
    /// </summary>
    /// <param name = "input"></param>
    /// <param name = "arrayName"></param>
    public static string GetArray(List<string> input, string arrayName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", arrayName, input);
        return generator.ToString();
    }

    /// <summary>
    /// Generates a C# list declaration with the given name and values.
    /// </summary>
    /// <param name="input">List of string values to include.</param>
    /// <param name="listName">Variable name for the generated list.</param>
    /// <returns>Generated C# list declaration code.</returns>
    public static string GetList(List<string> input, string listName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", listName, input);
        return generator.ToString();
    }

    /// <summary>
    /// Removes all #region and #endregion directives from the lines.
    /// </summary>
    /// <param name="lines">Source code lines to process.</param>
    /// <returns>Lines with region directives removed.</returns>
    public static List<string> RemoveRegions(List<string> lines)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("#" + "region" + " ") || line.StartsWith("#endregion"))
            {
                lines.RemoveAt(i);
            }
        }

        return lines;
    }

    /// <summary>
    /// Replaces static readonly field declarations with const declarations in the given file.
    /// </summary>
    /// <param name="pathXlfKeys">Path to the file to process.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
    ReplaceForConsts(string pathXlfKeys)
    {
        var count = SHGetLines.GetLines(
#if ASYNC
            await
#endif
        File.ReadAllTextAsync(pathXlfKeys)).ToList();
        for (int i = 0; i < count.Count; i++)
        {
            var argument = count[i];
            if (argument.Contains(CSharpParser.StaticReadonlyModifier))
            {
                if (!argument.Contains("const") && !argument.Contains("class"))
                {
                    argument = SHReplace.ReplaceOnce(argument, "static ", string.Empty);
                    argument = SHReplace.ReplaceOnce(argument, "readonly ", string.Empty);
                    count[i] = SHReplace.ReplaceOnce(argument, CSharpParser.StaticReadonlyModifier, CSharpParser.PublicModifier + "const ");
                }
            }
        }

        await File.WriteAllLinesAsync(pathXlfKeys, count);
    }

    /// <summary>
    /// Generates static readonly string constants from the list with optional camel case convention.
    /// </summary>
    /// <param name="list">List of values to generate constants from.</param>
    /// <param name="toCamelConvention">Whether to apply camel case convention.</param>
    /// <returns>Generated constant declarations.</returns>
    public static string GetConsts(List<string> list, bool? toCamelConvention)
    {
        return GetConsts(null!, list, toCamelConvention);
    }

    /// <summary>
    /// Generates C# static readonly string constant declarations from the given names and values.
    /// </summary>
    /// <param name="names">Optional list of custom constant names (can be null to use values as names).</param>
    /// <param name="list">List of values for the constants.</param>
    /// <param name="toCamelConventionFirstCharLower">Whether to apply camelCase convention to constant names (null = no conversion).</param>
    /// <returns>Generated C# code with constant declarations.</returns>
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower)
    {
        return GetConsts(names, list, toCamelConventionFirstCharLower, Types.StringType);
    }

    /// <summary>
    /// A1 can be null
    ///
    /// A3 null = not use Pascal convention
    ///
    /// GenerateConstants - const without value
    /// GetConsts - static readonly with value
    /// </summary>
    /// <param name="names">Optional list of custom constant names (can be null to use values as names).</param>
    /// <param name="list">List of values for the constants.</param>
    /// <param name="toCamelConventionFirstCharLower">Whether to apply camel case convention (null to skip).</param>
    /// <param name="type">Type of the constants (e.g. string, int).</param>
    /// <returns>Generated constant declarations.</returns>
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower, Type type)
    {
        bool addHyphensToValue = true;
        if (type != Types.StringType)
        {
            addHyphensToValue = false;
        }

        if (names != null)
        {
            ThrowEx.DifferentCountInLists("names", names, "list", list);
        }

        CSharpGenerator csg = new CSharpGenerator();
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            string name = item;
            if (names != null)
            {
                name = names[i];
            }

            if (toCamelConventionFirstCharLower.HasValue)
            {
                if (toCamelConventionFirstCharLower.Value)
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
                else
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
            }

            csg.Field(0, AccessModifiers.Public, true, VariableModifiers.ReadOnly, ConvertTypeShortcutFullName.ToShortcut(type.FullName!, false), name, addHyphensToValue, item);
        }

        var result = csg.ToString();
        return result;
    }

    /// <summary>
    /// GenerateConstants - const without value
    /// GetConsts - static readonly with value
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "changeInput"></param>
    /// <param name = "input"></param>
    /// <returns></returns>
    public static string GenerateConstants(int tabCount, Func<string, string> changeInput, List<string> input)
    {
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in input)
        {
            string name = item;
            if (changeInput != null)
            {
                name = changeInput(item);
            }

            csg.Field(tabCount, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", name, true, item);
        }

        return csg.ToString();
    }

    /// <summary>
    /// Trims specified characters from the end of each string in the list.
    /// </summary>
    /// <param name="sf">List of strings to trim.</param>
    /// <param name="toTrim">Characters to trim from the end.</param>
    /// <returns>The modified list with trimmed strings.</returns>
    public static List<string> TrimEnd(List<string> sf, params char[] toTrim)
    {
        for (int i = 0; i < sf.Count; i++)
        {
            sf[i] = sf[i].TrimEnd(toTrim);
        }

        return sf;
    }

    /// <summary>
    /// value - data type
    /// key - name
    /// </summary>
    /// <param name="lines">Lines containing field declarations to parse.</param>
    /// <returns>Dictionary mapping field names (camelCase) to their data types.</returns>
    public static Dictionary<string, string> ParseFields(List<string> lines)
    {
        CA.RemoveStringsEmpty2(lines);
        CAChangeContent.ChangeContent0(null!, lines, e => SHParts.RemoveAfterFirst(e, '='));
        CA.TrimEnd(lines, ';');
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (var item in lines)
        {
            var parameter = SHSplit.SplitByWhiteSpaces(item);
            var count = parameter.Count;
            StringBuilder sb2 = new StringBuilder(parameter[count - 1]);
            sb2[0] = char.ToLower(sb2[0]);
            result.Add(sb2.ToString(), parameter[count - 2]);
        }

        return result;
    }

    /// <summary>
    /// 0 - item.Value
    /// 1 - CSharpHelperSunamo.DefaultValueForType(item.Value)
    /// 2 - item.Key
    /// 3 - SH.FirstCharUpper(item.Key)
    /// </summary>
    const string tProperty = @"{0} {2} = {1};
    public {0} {3} { get { return {2}; } set { {2} = value; OnPropertyChanged(" + "\"{3}\"); } }" + @"
";
    /// <summary>
    /// Generates C# property declarations with backing fields from the given arguments.
    /// </summary>
    /// <param name="argument">Arguments specifying input lines and generation options.</param>
    /// <returns>Generated property code.</returns>
    public static string GenerateProperties(GeneratePropertiesArgs argument)
    {
        var lines = argument.Input;
        Dictionary<string, string>? data = null;
        if (argument.AllStrings)
        {
            data = new Dictionary<string, string>(lines.Count);
            foreach (var item in lines)
            {
                var parameter = ConvertPascalConvention.ToConvention(item);
                StringBuilder sb2 = new StringBuilder(parameter);
                sb2[0] = char.ToUpper(sb2[0]);
                //p = SH.FirstCharLower(parameter);
                //DebugLogger.Instance.WriteLine(parameter);
                data.Add(sb2.ToString(), "string");
            }
        }
        else
        {
            data = ParseFields(lines);
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in data)
        {
            StringBuilder sb2 = new StringBuilder(item.Key);
            sb2[0] = char.ToUpper(sb2[0]);
            stringBuilder.AppendLine( /*SHFormat.Format3*/string.Format((string)tProperty, item.Value, CSharpHelperSunamo.DefaultValueForType(item.Value, ConvertTypeShortcutFullName.ToShortcut), item.Key, sb2.ToString()));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Finds the line containing a class declaration. If mustDerive is true, only returns classes that inherit from another.
    /// </summary>
    /// <param name="lines">Source code lines to search.</param>
    /// <param name="mustDerive">Whether the class must derive from another class.</param>
    /// <returns>The line containing the class declaration, or null if not found.</returns>
    public static string? LineWithClass(List<string> lines, bool mustDerive)
    {
        foreach (var item in lines)
        {
            if (item.Contains("class "))
            {
                if (mustDerive)
                {
                    if (item.Contains(":"))
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    static Type type = typeof(CSharpHelper);
}