// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode.SunamoCSharp;

public static class CSharpHelper
{
    public const string Using = "using ";
    public const string Import = "import ";

    #region Cs files
    public static string RemoveUsing(string text, CollectionWithoutDuplicatesDC<string> removed)
    {
        foreach (var result in removed.c)
        {
            text = SHReplace.ReplaceOnce(text, "using " + result + ";", string.Empty);
        }
        return text;
    }

    public static
#if ASYNC
        async Task
#else
    void
#endif
        AddTypeToEveryFile(string BasePathsHelperVs)
    {
        int i = 0;
        var path = BasePathsHelperVs;

        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories).ToList();

        files.RemoveAll(data => data.EndsWith("Shared.cs"));

        const string template = "static Type type = typeof({0});";

        StringBuilder sbGeneric = new StringBuilder();
        int start = 0;
        int end = 0;
        bool inserted = false;

        const string shared1 = "Shared";
        const string shared2 = "SharedShared";

        const string mustContains = "Type type = typeof(";

        foreach (var item in files)
        {
            var count =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(item);

            if (count.Contains("ThrowEx.") && !count.Contains(mustContains))
            {
                var c1 = await File.ReadAllTextAsync(FS.InsertBetweenFileNameAndExtension(item, shared1));
                var c2 = await File.ReadAllTextAsync(FS.InsertBetweenFileNameAndExtension(item, shared2));

                if (c1.Contains(mustContains) || c2.Contains(mustContains))
                {
                    continue;
                }

                //                if (
                //#if ASYNC
                //    await
                //#endif
                // .ContainsInShared(item, mustContains, shared1))
                //                {
                //                    continue;
                //                }

                //#if ASYNC
                //                await
                //#endif
                //             .ContainsInShared(item, mustContains, shared2))
                //                {
                //                    continue;
                //                }

                var lines = count.Split(new string[] { count.Contains("\r\n") ? "\r\n" : "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                bool allCommented = true;

                foreach (var item2 in lines)
                {
                    var item3 = item2.Trim();
                    if (!item.StartsWith("//"))
                    {
                        allCommented = false;
                        break;
                    }
                }

                if (allCommented)
                {
                    continue;
                }

                inserted = false;

                for (i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("class "))
                    {
                        sbGeneric.Clear();

                        var line = lines[i];
                        var parameter = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        var cl = parameter.IndexOf("class");
                        if (parameter.Count > cl)
                        {
                            start = 0;
                            end = 0;

                            string pi = parameter[cl + 1];

                            if (pi.Contains("<"))
                            {
                                start += pi.Count((ch) => ch == '<'); //.CountOf(pi, '<');
                                end += pi.Count((ch) => ch == '>');


                                if (start == end)
                                {
                                    break;
                                }

                                for (int y = cl + 2; y < parameter.Count; y++)
                                {
                                    pi = parameter[y];

                                    start += pi.Count((ch) => ch == '<');
                                    end += pi.Count((ch) => ch == '>');

                                    sbGeneric.Append(pi);

                                    if (start == end)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        var lineWithClass = lines[i];

                        int insertTo = i + 2;

                        if (lineWithClass.Trim().EndsWith("{"))
                        {
                            insertTo = i + 1;
                        }

                        lines.Insert(insertTo, string.Format(template, parameter[cl + 1].Trim().TrimEnd('{') + sbGeneric.ToString()));

                        count = string.Join(Environment.NewLine, sbGeneric, lines);
                        inserted = true;
                        break;
                    }
                }

                if (inserted)
                {
                    await File.WriteAllTextAsync(item, count);
                }


            }
        }
    }

    /// <param name="lines"></param>
    public static void RemoveNamespace(List<string> lines, CollectionWithoutDuplicatesDC<string> removed, bool removeRegions = true)
    {


        #region Remove namespace and #region from original
        const string startWith = "namespace ";

        List<string> cLines = null;
        if (removeRegions)
        {
            cLines = CSharpHelper.RemoveRegions(lines);
        }
        else
        {
            cLines = lines;
        }
        int nsDx = cLines.FindIndex(text => text.Trim().StartsWith(startWith));
        if (nsDx != -1)
        {
            if (removed != null)
            {
                var line = cLines[nsDx].Trim().TrimEnd('{').Substring(startWith.Length);
                removed.Add(line);
            }
            if (!cLines[nsDx].Contains("{"))
            {
                cLines.RemoveAt(nsDx + 1);
            }
            cLines.RemoveAt(nsDx);

            for (int i = cLines.Count - 1; i >= 0; i--)
            {
                if (cLines[i].Contains("}"))
                {
                    cLines[i] = cLines[i].Trim().TrimEnd('}');
                    // break = remove only first
                    break;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Return written content to drive
    /// </summary>
    /// <param name="item"></param>
    public static
#if ASYNC
        async Task<List<string>>
#else
    List<string>
#endif
        RemoveNamespace(string item)
    {
        var lines = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(item)).ToList();
        RemoveNamespace(lines, null);
        await File.WriteAllLinesAsync(item, lines);
        return lines;
    }
    #endregion

    public static void SetValuesAsNamesToConsts(List<string> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            var line = text[i];
            var data = IsFieldVariableConst(line);
            if (data.Item1)
            {
                if (line.EndsWith(";") && !line.Contains("="))
                {
                    text[i] = text[i].Replace("readonly ", "");
                    text[i] = text[i].Replace("static ", "const ");
                    text[i] = text[i].TrimEnd(';') + " = " + SH.WrapWithQm(data.Item2) + ";";
                }
            }
        }
    }

    public static (bool, string) IsFieldVariableConst(string line)
    {
        CsKeywordsList.Init();

        var text = line;
        text = text.Replace("readonly ", "");
        text = text.Replace("static ", "");
        text = text.Replace("const ", "");

        foreach (var item in CsKeywordsList.accessModifier)
        {
            text = text.Replace(item + " ", "");
        }

        var parameter = SHSplit.Split(text, " ");
        if (parameter.Count == 2 && parameter[0] == "string")
        {
            return (true, parameter[1].Trim().TrimEnd(';'));
        }
        return (false, null);
    }

    public static string GetInnerContentOfCodeElementClass(List<string> lines)
    {
        if (IsEmptyOrCommented(lines))
        {
            return string.Empty;
        }
        //var first = lines.First(data => data == "{");
        //var last = lines.Last(data => data == "}");

        var dxF = lines.IndexOf("{"); ;
        var dxL = lines.LastIndexOf("}");

        return SHJoin.JoinNL(lines.Skip(dxF + 1).Take(dxL - dxF).ToList());
    }


    /// <summary>
    /// Vrátí true i když obsahuje kód bez středníku (např. prázdnou třídu)
    /// </summary>
    /// <param name="fnwoe"></param>
    /// <param name="linesOriginal"></param>
    /// <param name="RemoveBetweenIfAndEndif">Can be null</param>
    /// <param name="csWithSharpIf">Can't be null</param>
    /// <returns></returns>
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
    /// Direct edit
    /// </summary>
    /// <param name="l2"></param>
    /// <param name="v"></param>
    /// <exception cref="NotImplementedException"></exception>
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

    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, bool remove = false)
    {
        return Usings(lines, Using, remove);
    }

    public static CollectionWithoutDuplicatesDC<string> Imports(List<string> lines, bool remove = false)
    {
        return Usings(lines, Import, remove);
    }

    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, string keyword, bool remove = false)
    {
        List<int> removeLines = null;
        return Usings(lines, keyword, out removeLines, remove);
    }

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

    public static string GetDictionaryValuesFromTwoList(List<string> names, List<string> chars)
    {
        return CSharpHelper.GetDictionaryValuesFromTwoList<string, string>(2, "a", names, chars, new CSharpGeneratorArgs { splitKeyWith = "," });
    }

    public static string GetDictionaryValuesFromDictionary(Dictionary<string, string> data)
    {
        return CSharpHelper.GetDictionaryValuesFromDictionary<string, string>(0, "name", data);
    }

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
    public static string CreateConstsForSearchUris(List<string> uris)
    {
        CSharpGenerator csg = new CSharpGenerator();

        // In key name of const, in value value
        Dictionary<string, string> dict = new Dictionary<string, string>();
        List<string> all = new List<string>();

        foreach (var item in uris)
        {
            Uri u = new Uri(item);
            string name = u.Host.ToConstantCase().ToPascalCase();  //CaseDotNet.CaseConverter.PascalCase.ConvertCase(u.Host); //ConvertPascalConvention.ToConvention(u.Host);
            dict.Add(name, item);
            all.Add(name);
        }

        CreateConsts(csg, dict);

        csg.List(2, "string", "All", all, new CSharpGeneratorArgs { addHyphens = false });

        return csg.ToString();
    }

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

    #region DictionaryWithClass
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs argument = null)
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
    /// addingValue = 0
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="tabCount"></param>
    /// <param name="nameDictionary"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> data, CSharpGeneratorArgs argument = null)
    {
        if (argument == null)
        {
            argument.addingValue = false;
        }

        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromDictionary<Key, Value>(0, nameDictionary, data, argument);
        var inner = GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, data);
        genCS.Ctor(1, ModifiersConstructor.Private, nameDictionary, inner);
        genCS.EndBrace(0);
        return genCS.ToString();
    }
    #endregion
    #region GetDictionaryValues
    public static string GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict)
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, dict);
        return csg.ToString();
    }
    /// <summary>
    /// argument: splitKeyWith
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="tabCount"></param>
    /// <param name="nameDictionary"></param>
    /// <param name="keys"></param>
    /// <param name="values"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static string GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs argument)
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromTwoList<Key, Value>(tabCount, nameDictionary, keys, values, argument);
        return csg.ToString();
    }
    public static string GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue)
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        return csg.ToString();
    }
    #endregion

    //public static string GetDictionary(string nameDictionary)
    //{
    //}
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
    /// 
    /// </summary>
    /// <param name="listOrString"></param>
    /// <param name="line"></param>
    /// <param name="block"></param>
    /// <returns></returns>
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
    /// <param name="list"></param>
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



    public static string RemoveBlockComments(string str)
    {
        //var str = CastHelper.ToString(listOrString);

        //for (int i = list.Count - 1; i >= 0; i--)
        //{
        //    list[i] = SHParts.RemoveAfterFirst(list[i], CSharp""");
        //}

        str = Regex.Replace(str,
            blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
            me =>
            {
                if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                    return me.Value.StartsWith("//") ? Environment.NewLine : "";
                // Keep the literal strings
                return me.Value;
            },
            RegexOptions.Singleline);

        //CA.RemoveStringsEmpty2(list);

        return str;
    }

    public static string GetDictionaryStringObject<Value>(int tabCount, List<string> keys, List<Value> values, string nameDictionary, CSharpGeneratorArgs argument)
    {


        int pocetTabu = 0;
        CSharpGenerator gen = new CSharpGenerator();
        gen.DictionaryFromTwoList<string, Value>(tabCount, nameDictionary, keys, values, argument);
        if (argument.checkForNull)
        {
            gen.If(pocetTabu, nameDictionary + " " + "== null");
        }
        gen.GetDictionaryValuesFromDictionary<string, Value>(pocetTabu, nameDictionary, DictionaryHelper.GetDictionary<string, Value>(keys, values));
        if (argument.checkForNull)
        {
            gen.EndBrace(pocetTabu);
        }

        string result = gen.ToString();
        return result;
    }


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
        return null;
    }

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
    public static string GetList(List<string> input, string listName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", listName, input);
        return generator.ToString();
    }
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
            if (argument.Contains(CSharpParser.parameter))
            {
                if (!argument.Contains("const") && !argument.Contains("class"))
                {
                    argument = SHReplace.ReplaceOnce(argument, "static ", string.Empty);
                    argument = SHReplace.ReplaceOnce(argument, "readonly ", string.Empty);
                    count[i] = SHReplace.ReplaceOnce(argument, CSharpParser.parameter, CSharpParser.p + "const ");
                }
            }
        }

        await File.WriteAllLinesAsync(pathXlfKeys, count);
    }

    public static string GetConsts(List<string> list, bool? toCamelConvention)
    {
        return GetConsts(null, list, toCamelConvention);
    }

    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower)
    {
        return GetConsts(names, list, toCamelConventionFirstCharLower, Types.tString);
    }

    /// <summary>
    /// A1 can be null
    ///
    /// A3 null = not use Pascal convention
    ///
    /// GenerateConstants - const without value
    /// GetConsts - static readonly with value
    /// </summary>
    /// <param name="list"></param>
    /// <param name="toCamelConventionFirstCharLower"></param>
    /// <returns></returns>
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower, Type type)
    {
        bool addHyphensToValue = true;
        if (type != Types.tString)
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
            csg.Field(0, AccessModifiers.Public, true, VariableModifiers.ReadOnly, ConvertTypeShortcutFullName.ToShortcut(type.FullName, false), name, addHyphensToValue, item);
        }
        var result = csg.ToString();
        return result;
    }

    /// <summary>
    /// GenerateConstants - const without value
    /// GetConsts - static readonly with value
    /// </summary>
    /// <param name="tabCount"></param>
    /// <param name="changeInput"></param>
    /// <param name="input"></param>
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

            csg.Field(tabCount, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", name, true, item
            );
        }
        return csg.ToString();
    }

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
    /// <param name="l"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ParseFields(List<string> lines)
    {
        CA.RemoveStringsEmpty2(lines);
        CAChangeContent.ChangeContent0(null, lines, e => SHParts.RemoveAfterFirst(e, '='));
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

    public static string GenerateProperties(GeneratePropertiesArgs argument)
    {
        var lines = argument.input;
        Dictionary<string, string> data = null;
        if (argument.allStrings)
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
            stringBuilder.AppendLine(/*SHFormat.Format3*/ string.Format((string)tProperty, item.Value, CSharpHelperSunamo.DefaultValueForType(item.Value, ConvertTypeShortcutFullName.ToShortcut), item.Key, sb2.ToString()));
        }
        return stringBuilder.ToString();
    }

    public static string LineWithClass(List<string> lines, bool mustDerive)
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
    /// <param name="con"></param>
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