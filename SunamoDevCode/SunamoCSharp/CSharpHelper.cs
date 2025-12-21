namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class CSharpHelper
{
    public const string Using = "using ";
    public const string Import = "import ";
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

    /// <param name = "lines"></param>
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
    /// <param name = "item"></param>
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
}