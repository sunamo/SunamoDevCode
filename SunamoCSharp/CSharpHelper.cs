namespace SunamoDevCode.SunamoCSharp;

public static class CSharpHelper
{
    public const string Using = "using ";
    public const string Import = "import ";

    #region Cs files
    public static string RemoveUsing(string text, CollectionWithoutDuplicatesDevCode<string> removed)
    {
        foreach (var r in removed.c)
        {
            text = SHReplace.ReplaceOnce(text, "using " + r + AllStrings.sc, string.Empty);
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

        files.RemoveAll(d => d.EndsWith("Shared.cs"));

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
            var c =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(item);

            if (c.Contains("ThrowEx.") && !c.Contains(mustContains))
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

                var l = c.Split(new string[] { c.Contains("\r\n") ? "\r\n" : "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                bool allCommented = true;

                foreach (var item2 in l)
                {
                    var item3 = item2.Trim();
                    if (!item.StartsWith(CSharpConsts.lc))
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

                for (i = 0; i < l.Count(); i++)
                {
                    if (l[i].Contains("class "))
                    {
                        sbGeneric.Clear();

                        var line = l[i];
                        var p = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        var cl = p.IndexOf("class");
                        if (p.Count > cl)
                        {
                            start = 0;
                            end = 0;

                            string pi = p[cl + 1];

                            if (pi.Contains("<"))
                            {
                                start += pi.Count((ch) => ch == '<'); //.CountOf(pi, '<');
                                end += pi.Count((ch) => ch == '>');


                                if (start == end)
                                {
                                    break;
                                }

                                for (int y = cl + 2; y < p.Count; y++)
                                {
                                    pi = p[y];

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

                        var lineWithClass = l[i];

                        int insertTo = i + 2;

                        if (lineWithClass.Trim().EndsWith("{"))
                        {
                            insertTo = i + 1;
                        }

                        l.Insert(insertTo, string.Format(template, p[cl + 1].Trim().TrimEnd('{') + sbGeneric.ToString()));

                        c = string.Join(Environment.NewLine, sbGeneric, l);
                        inserted = true;
                        break;
                    }
                }

                if (inserted)
                {
                    await File.WriteAllTextAsync(item, c);
                }


            }
        }
    }

    /// <param name="lines"></param>
    public static void RemoveNamespace(List<string> lines, CollectionWithoutDuplicatesDevCode<string> removed, bool removeRegions = true)
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
        int nsDx = cLines.FindIndex(s => s.Trim().StartsWith(startWith));
        if (nsDx != -1)
        {
            if (removed != null)
            {
                var line = cLines[nsDx].Trim().TrimEnd(AllChars.lcub).Substring(startWith.Length);
                removed.Add(line);
            }
            if (!cLines[nsDx].Contains(AllStrings.lcub))
            {
                cLines.RemoveAt(nsDx + 1);
            }
            cLines.RemoveAt(nsDx);

            for (int i = cLines.Count - 1; i >= 0; i--)
            {
                if (cLines[i].Contains(AllStrings.rcub))
                {
                    cLines[i] = cLines[i].Trim().TrimEnd(AllChars.rcub);
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
        var l = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(item)).ToList();
        RemoveNamespace(l, null);
        await File.WriteAllLinesAsync(item, l);
        return l;
    }
    #endregion

    public static string GetInnerContentOfCodeElementClass(List<string> l)
    {
        if (IsEmptyOrCommented(l))
        {
            return string.Empty;
        }
        //var first = l.First(d => d == "{");
        //var last = l.Last(d => d == "}");

        var dxF = l.IndexOf("{"); ;
        var dxL = l.LastIndexOf("}");

        return SHJoin.JoinNL(l.Skip(dxF + 1).Take(dxL - dxF).ToList());
    }




    public static bool IsEmptyOrCommented(List<string> l)
    {
        foreach (var item in l)
        {
            var d = item.Trim();
            if (!d.StartsWith("//") && d != string.Empty)
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

        _sunamo.SunamoCollections.CA.RemoveEmptyLinesToFirstNonEmpty(l2);
    }

    public static CollectionWithoutDuplicatesDevCode<string> Usings(List<string> lines, bool remove = false)
    {
        return Usings(lines, Using, remove);
    }

    public static CollectionWithoutDuplicatesDevCode<string> Imports(List<string> lines, bool remove = false)
    {
        return Usings(lines, Import, remove);
    }

    public static CollectionWithoutDuplicatesDevCode<string> Usings(List<string> lines, string keyword, bool remove = false)
    {
        List<int> removeLines = null;
        return Usings(lines, keyword, out removeLines, remove);
    }

    public static CollectionWithoutDuplicatesDevCode<string> Usings(List<string> lines, string keyword, out List<int> removeLines, bool remove = false)
    {
        CollectionWithoutDuplicatesDevCode<string> usings = new CollectionWithoutDuplicatesDevCode<string>();
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
                else //if (line.Contains(AllStrings.lcub))
                {
                    break;
                }
            }
        }

        if (remove)
        {
            _sunamo.SunamoCollections.CA.RemoveLines(lines, removeLines);
        }

        return usings;
    }

    public static string GetDictionaryValuesFromTwoList(List<string> names, List<string> chars)
    {
        return CSharpHelper.GetDictionaryValuesFromTwoList<string, string>(2, "a", names, chars, new CSharpGeneratorArgs { splitKeyWith = "," });
    }

    public static string GetDictionaryValuesFromDictionary(Dictionary<string, string> d)
    {
        return CSharpHelper.GetDictionaryValuesFromDictionary<string, string>(0, "name", d);
    }

    public static string GetSummaryXmlDocumentation(List<string> cs)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < cs.Count(); i++)
        {
            var line = cs[i];
            if (line.StartsWith(CodeElementsConstants.XmlDocumentationCsharp))
            {
                line = line.TrimStart('/'); //SHTrim.TrimStart(line, CodeElementsConstants.XmlDocumentationCsharp).Trim();
                sb.AppendLine(line);
                if (line.Contains("</summary>"))
                {
                    break;
                }
            }
        }
        return sb.ToString();
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
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs a = null)
    {
        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);

        genCS.DictionaryFromRandomValue<Key, Value>(0, nameDictionary, keys, randomValue, a);
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
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> d, CSharpGeneratorArgs a = null)
    {
        if (a == null)
        {
            a.addingValue = false;
        }

        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromDictionary<Key, Value>(0, nameDictionary, d, a);
        var inner = GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, d);
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
    /// a: splitKeyWith
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="tabCount"></param>
    /// <param name="nameDictionary"></param>
    /// <param name="keys"></param>
    /// <param name="values"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static string GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs a)
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromTwoList<Key, Value>(tabCount, nameDictionary, keys, values, a);
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
        const string a = @"/// <returns></returns>";
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == a)
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
        _sunamo.SunamoCollections.CA.TrimWhereIsOnlyWhitespace(list);
        var s = string.Join(Environment.NewLine, list);

        _sunamo.SunamoCollections.CA.DoubleOrMoreMultiLinesToSingle(ref s);

        return s;
    }

    public static string RemoveComments(string listOrString, bool line = true, bool block = true)
    {
        return SHJoin.JoinNL(RemoveComments(SHGetLines.GetLines(listOrString), line, block));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listOrString"></param>
    /// <param name="line"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    public static List<string> RemoveComments(List<string> listOrString, bool line = true, bool block = true)
    {
        if (line)
        {
            listOrString = RemoveLineComments(listOrString);
        }

        if (block)
        {
            listOrString = SHGetLines.GetLines(RemoveBlockComments(string.Join(Environment.NewLine, listOrString)));
        }

        //var list = CastHelper.ToListString(listOrString);
        //jak jsem mohl být takový debil a dát to tady - na co tady trimovat?
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
            list[i] = SHParts.RemoveAfterFirst(list[i], CSharpConsts.lc);
        }

        _sunamo.SunamoCollections.CA.RemoveStringsEmpty2(list);

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
        //    list[i] = SHParts.RemoveAfterFirst(list[i], CSharpConsts.lc);
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

    public static string GetDictionaryStringObject<Value>(int tabCount, List<string> keys, List<Value> values, string nameDictionary, CSharpGeneratorArgs a)
    {


        int pocetTabu = 0;
        CSharpGenerator gen = new CSharpGenerator();
        gen.DictionaryFromTwoList<string, Value>(tabCount, nameDictionary, keys, values, a);
        if (a.checkForNull)
        {
            gen.If(pocetTabu, nameDictionary + " " + "== null");
        }
        gen.GetDictionaryValuesFromDictionary<string, Value>(pocetTabu, nameDictionary, DictionaryHelper.GetDictionary<string, Value>(keys, values));
        if (a.checkForNull)
        {
            gen.EndBrace(pocetTabu);
        }

        string result = gen.ToString();
        return result;
    }


    public static string DefaultValueForTypeSqLite(string type)
    {
        if (type.Contains(AllStrings.dot))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }
        switch (type)
        {
            case "TEXT":
                return AllStrings.qm;
            case "INTEGER":
                return int.MaxValue.ToString();
            case "REAL":
                return "0.0";
            case "DATETIME":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat a proto si ušetřím v kódu práci s MSSQL
                return "DateTime.MinValue";
            case "BLOB":
                // Podporovaný typ pouze v desktopových aplikacích, kde není lsožka sbf
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
        var c = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(pathXlfKeys)).ToList();
        for (int i = 0; i < c.Count; i++)
        {
            var a = c[i];
            if (a.Contains(CSharpParser.p))
            {
                if (!a.Contains("const") && !a.Contains("class"))
                {
                    a = SHReplace.ReplaceOnce(a, "static ", string.Empty);
                    a = SHReplace.ReplaceOnce(a, "readonly ", string.Empty);
                    c[i] = SHReplace.ReplaceOnce(a, CSharpParser.p, CSharpParser.p + "const ");
                }
            }
        }

        await File.WriteAllLinesAsync(pathXlfKeys, c);
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
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower, Type t)
    {
        bool addHyphensToValue = true;
        if (t != Types.tString)
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
            csg.Field(0, AccessModifiers.Public, true, VariableModifiers.ReadOnly, ConvertTypeShortcutFullName.ToShortcut(t.FullName, false), name, addHyphensToValue, item);
        }
        var r = csg.ToString();
        return r;
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
    public static Dictionary<string, string> ParseFields(List<string> l)
    {
        _sunamo.SunamoCollections.CA.RemoveStringsEmpty2(l);
        CAChangeContent.ChangeContent0(null, l, e => SHParts.RemoveAfterFirst(e, AllChars.equals));
        _sunamo.SunamoCollections.CA.TrimEnd(l, AllChars.sc);
        Dictionary<string, string> r = new Dictionary<string, string>();
        foreach (var item in l)
        {
            var p = SHSplit.SplitByWhiteSpaces(item);
            var c = p.Count;
            StringBuilder sb2 = new StringBuilder(p[c - 1]);
            sb2[0] = char.ToLower(sb2[0]);
            r.Add(sb2.ToString(), p[c - 2]);
        }
        return r;
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

    public static string GenerateProperties(GeneratePropertiesArgs a)
    {
        var l = a.input;
        Dictionary<string, string> d = null;
        if (a.allStrings)
        {
            d = new Dictionary<string, string>(l.Count);
            foreach (var item in l)
            {
                var p = ConvertPascalConvention.ToConvention(item);

                StringBuilder sb2 = new StringBuilder(p);
                sb2[0] = char.ToUpper(sb2[0]);

                //p = SH.FirstCharLower(p);
                //DebugLogger.Instance.WriteLine(p);
                d.Add(sb2.ToString(), "string");
            }
        }
        else
        {
            d = ParseFields(l);
        }

        StringBuilder sb = new StringBuilder();
        foreach (var item in d)
        {
            StringBuilder sb2 = new StringBuilder(item.Key);
            sb2[0] = char.ToUpper(sb2[0]);
            sb.AppendLine(/*SHFormat.Format3*/ string.Format((string)tProperty, item.Value, CSharpHelperSunamo.DefaultValueForType(item.Value, ConvertTypeShortcutFullName.ToShortcut), item.Key, sb2.ToString()));
        }
        return sb.ToString();
    }

    public static string LineWithClass(List<string> l, bool mustDerive)
    {
        foreach (var item in l)
        {
            if (item.Contains("class "))
            {
                if (mustDerive)
                {
                    if (item.Contains(AllStrings.colon))
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
        if (type.Contains(AllStrings.dot))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "string":
                return AllStrings.qm + AllStrings.qm;
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
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat a proto si ušetřím v kódu práci s MSSQL
                return Consts.DateTimeMinVal;
            case "char":
                throw new Exception(type);
                return 0;
            case "byte" + "[]":
                // Podporovaný typ pouze v desktopových aplikacích, kde není lsožka sbf
                return null;
        }
        throw new Exception("Nepodporovaný typ");
        return null;
    }

    public static string WrapWithRegion(string s, string v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("#region ");
        sb.AppendLine(v);
        sb.AppendLine(s);
        sb.AppendLine("#endregion");
        return sb.ToString();
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
        StringBuilder sb = new StringBuilder();
        foreach (var item in valueS)
        {
            var v = item.ToString();
            WrapWithQuote(tValue, ref v);
            sb.Append(v + AllStrings.comma);
        }
        return sb.ToString().TrimEnd(AllChars.comma);
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
