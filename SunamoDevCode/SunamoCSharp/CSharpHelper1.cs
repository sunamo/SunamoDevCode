namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
    /// <summary>
    /// Extracts the inner content of a class code element by removing the outer braces.
    /// </summary>
    /// <param name="lines">Lines of source code containing the class definition.</param>
    /// <returns>The inner content between the first opening and last closing brace, or empty string if the code is empty or commented.</returns>
    public static string GetInnerContentOfCodeElementClass(List<string> lines)
    {
        if (IsEmptyOrCommented(lines))
        {
            return string.Empty;
        }

        //var first = lines.First(data => data == "{");
        //var last = lines.Last(data => data == "}");
        var dxF = lines.IndexOf("{");
        ;
        var dxL = lines.LastIndexOf("}");
        return SHJoin.JoinNL(lines.Skip(dxF + 1).Take(dxL - dxF).ToList());
    }

    /// <summary>
    /// Vrátí true i když obsahuje kód bez středníku (např. prázdnou třídu)
    /// </summary>
    /// <param name="fnwoe">File name without extension to check against csWithSharpIf list.</param>
    /// <param name="linesOriginal">Original source code lines to analyze.</param>
    /// <param name="RemoveBetweenIfAndEndif">Optional action to remove preprocessor directive blocks. Can be null.</param>
    /// <param name="csWithSharpIf">List of filenames that contain #if directives and should not be considered empty.</param>
    /// <returns>True if the file contains only namespace declarations, usings, comments, or is empty.</returns>
    public static bool IsEmptyCommentedOrOnlyWithNamespace(string fnwoe, List<string> linesOriginal, Action<List<string>> RemoveBetweenIfAndEndif, List<string> csWithSharpIf)
    {
        var lines = linesOriginal.ToList();
        //var lines = await GetFileContentLines(value, true);
        //var fnwoe = Path.GetFileNameWithoutExtension(value);
        if (csWithSharpIf.Contains(fnwoe))
        {
            return false;
        }

        if (RemoveBetweenIfAndEndif != null)
        {
            RemoveBetweenIfAndEndif(lines);
        }

        CA.Trim(lines);
        CA.RemoveNullEmptyWs(lines);
        lines = RemoveComments(lines);
        lines = lines.Where(data => !data.StartsWith("namespace")).ToList();
        lines = lines.Where(data => !data.StartsWith("using")).ToList();
        // "," protože např. value enum souborech nemusí být žádný ;
        lines = lines.Where(data => data.EndsWith(";") || data.EndsWith(",")).ToList();
        if (!lines.Any())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether all lines are either empty or commented out.
    /// </summary>
    /// <param name="lines">Lines of source code to check.</param>
    /// <returns>True if all non-empty lines start with a comment marker.</returns>
    public static bool IsEmptyOrCommented(List<string> lines)
    {
        foreach (var item in lines)
        {
            var data = item.Trim();
            if (!data.StartsWith("//") && data != string.Empty)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Removes import/using statements and leading empty lines from the source code lines. Edits the list directly.
    /// </summary>
    /// <param name="l2">Lines of source code to process.</param>
    /// <param name="imports">If true, removes import statements; otherwise removes using statements.</param>
    public static void RemoveImportsUsings(List<string> l2, bool imports)
    {
        string k = Using;
        if (imports)
        {
            k = Import;
        }

        for (int i = 0; i < l2.Count; i++)
        {
            var line = l2[i].Trim();
            if (line.StartsWith(k) || line == string.Empty)
            {
                l2[i] = string.Empty;
            }
            else
            {
                break;
            }
        }

        CA.RemoveEmptyLinesToFirstNonEmpty(l2);
    }

    /// <summary>
    /// Extracts all using statements from source code lines, optionally removing them.
    /// </summary>
    /// <param name="lines">Lines of source code.</param>
    /// <param name="remove">If true, removes the using lines from the source.</param>
    /// <returns>Collection of unique using statements found.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, bool remove = false)
    {
        return Usings(lines, Using, remove);
    }

    /// <summary>
    /// Extracts all import statements from source code lines, optionally removing them.
    /// </summary>
    /// <param name="lines">Lines of source code.</param>
    /// <param name="remove">If true, removes the import lines from the source.</param>
    /// <returns>Collection of unique import statements found.</returns>
    public static CollectionWithoutDuplicatesDC<string> Imports(List<string> lines, bool remove = false)
    {
        return Usings(lines, Import, remove);
    }

    /// <summary>
    /// Extracts all statements matching the given keyword from source code lines, optionally removing them.
    /// </summary>
    /// <param name="lines">Lines of source code.</param>
    /// <param name="keyword">Keyword to match (e.g., "using" or "import").</param>
    /// <param name="remove">If true, removes matching lines from the source.</param>
    /// <returns>Collection of unique statements found.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, string keyword, bool remove = false)
    {
        List<int> removeLines = null!;
        return Usings(lines, keyword, out removeLines, remove);
    }

    /// <summary>
    /// Extracts all statements matching the given keyword, outputting line indices and optionally removing them.
    /// </summary>
    /// <param name="lines">Lines of source code.</param>
    /// <param name="keyword">Keyword to match (e.g., "using" or "import").</param>
    /// <param name="removeLines">Output list of line indices where matching statements were found.</param>
    /// <param name="remove">If true, removes matching lines from the source.</param>
    /// <returns>Collection of unique statements found.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, string keyword, out List<int> removeLines, bool remove = false)
    {
        CollectionWithoutDuplicatesDC<string> usings = new CollectionWithoutDuplicatesDC<string>();
        removeLines = new List<int>();
        int i = -1;
        foreach (var item in lines)
        {
            i++;
            var line = item.Trim();
            if (line != string.Empty)
            {
                if (line.StartsWith(keyword))
                {
                    removeLines.Add(i);
                    usings.Add(line);
                }
                else //if (line.Contains("{"))
                {
                    break;
                }
            }
        }

        if (remove)
        {
            CA.RemoveLines(lines, removeLines);
        }

        return usings;
    }

    /// <summary>
    /// Generates C# dictionary initialization code from two parallel lists of names and values.
    /// </summary>
    /// <param name="names">List of dictionary key names.</param>
    /// <param name="chars">List of dictionary values corresponding to each key.</param>
    /// <returns>Generated C# code string for dictionary initialization.</returns>
    public static string GetDictionaryValuesFromTwoList(List<string> names, List<string> chars)
    {
        return CSharpHelper.GetDictionaryValuesFromTwoList<string, string>(2, "a", names, chars, new CSharpGeneratorArgs { SplitKeyWith = "," });
    }

    /// <summary>
    /// Generates C# dictionary initialization code from a string-to-string dictionary.
    /// </summary>
    /// <param name="data">Source dictionary to generate code from.</param>
    /// <returns>Generated C# code string for dictionary initialization.</returns>
    public static string GetDictionaryValuesFromDictionary(Dictionary<string, string> data)
    {
        return CSharpHelper.GetDictionaryValuesFromDictionary<string, string>(0, "name", data);
    }

    /// <summary>
    /// Extracts the summary section from XML documentation comments in C# source code lines.
    /// </summary>
    /// <param name="cs">Lines of source code containing XML documentation comments.</param>
    /// <returns>The extracted summary text.</returns>
    public static string GetSummaryXmlDocumentation(List<string> cs)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < cs.Count; i++)
        {
            var line = cs[i];
            if (line.StartsWith(CodeElementsConstants.XmlDocumentationCsharp))
            {
                line = line.TrimStart('/'); //SHTrim.TrimStart(line, CodeElementsConstants.XmlDocumentationCsharp).Trim();
                stringBuilder.AppendLine(line);
                if (line.Contains("</summary>"))
                {
                    break;
                }
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Generates C# const field declarations and a list for a collection of search URIs.
    /// </summary>
    /// <param name="uris">List of URI strings to generate constants from.</param>
    /// <returns>Generated C# code with const declarations and an aggregated list.</returns>
    public static string CreateConstsForSearchUris(List<string> uris)
    {
        CSharpGenerator csg = new CSharpGenerator();
        // In key name of const, in value value
        Dictionary<string, string> dict = new Dictionary<string, string>();
        List<string> all = new List<string>();
        foreach (var item in uris)
        {
            Uri u = new Uri(item);
            string name = u.Host.ToConstantCase().ToPascalCase(); //CaseDotNet.CaseConverter.PascalCase.ConvertCase(u.Host); //ConvertPascalConvention.ToConvention(u.Host);
            dict.Add(name, item);
            all.Add(name);
        }

        CreateConsts(csg, dict);
        csg.List(2, "string", "All", all, new CSharpGeneratorArgs { AddHyphens = false });
        return csg.ToString();
    }

    /// <summary>
    /// Generates C# const field declarations from a dictionary of name-value pairs.
    /// </summary>
    /// <param name="dict">Dictionary where keys are constant names and values are constant values.</param>
    /// <returns>Generated C# code with const declarations.</returns>
    public static string CreateConsts(Dictionary<string, string> dict)
    {
        CSharpGenerator csg = new CSharpGenerator();
        return CreateConsts(csg, dict);
    }

    private static string CreateConsts(CSharpGenerator csg, Dictionary<string, string> dict)
    {
        foreach (var item in dict)
        {
            csg.Field(2, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", item.Key, true, item.Value);
        }

        return csg.ToString();
    }

    /// <summary>
    /// Generates a C# class containing a dictionary initialized with random values for each key.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys.</typeparam>
    /// <typeparam name="Value">Type of dictionary values.</typeparam>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="nameDictionary">Name for the generated class and dictionary.</param>
    /// <param name="keys">List of keys for the dictionary.</param>
    /// <param name="randomValue">Function that generates a random value for each entry.</param>
    /// <param name="argument">Optional code generation arguments.</param>
    /// <returns>Generated C# code for the class with dictionary initialization.</returns>
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs? argument = null) where Key : notnull
    {
        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromRandomValue<Key, Value>(0, nameDictionary, keys, randomValue, argument);
        var inner = GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        genCS.Ctor(1, ModifiersConstructor.Private, nameDictionary, inner);
        genCS.EndBrace(0);
        return genCS.ToString();
    }

    /// <summary>
    /// Generates a C# class containing a dictionary initialized from an existing dictionary. AddingValue defaults to false.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys.</typeparam>
    /// <typeparam name="Value">Type of dictionary values.</typeparam>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="nameDictionary">Name for the generated class and dictionary.</param>
    /// <param name="data">Source dictionary to generate code from.</param>
    /// <param name="argument">Optional code generation arguments.</param>
    /// <returns>Generated C# code for the class with dictionary initialization.</returns>
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> data, CSharpGeneratorArgs? argument = null) where Key : notnull
    {
        if (argument == null)
        {
            argument = new CSharpGeneratorArgs { AddingValue = false };
        }

        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromDictionary<Key, Value>(0, nameDictionary, data, argument);
        var inner = GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, data);
        genCS.Ctor(1, ModifiersConstructor.Private, nameDictionary, inner);
        genCS.EndBrace(0);
        return genCS.ToString();
    }

    /// <summary>
    /// Generates C# code for dictionary value assignments from a source dictionary.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys.</typeparam>
    /// <typeparam name="Value">Type of dictionary values.</typeparam>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="nameDictionary">Name of the dictionary variable in the generated code.</param>
    /// <param name="dict">Source dictionary to generate assignment code from.</param>
    /// <returns>Generated C# code for dictionary value assignments.</returns>
    public static string GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, dict);
        return csg.ToString();
    }
}