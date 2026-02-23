namespace SunamoDevCode.SunamoCSharp;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
/// <summary>
/// Provides static helper methods for C# code analysis, generation, and manipulation.
/// </summary>
public static partial class CSharpHelper
{
    /// <summary>
    /// C# using keyword prefix.
    /// </summary>
    public const string Using = "using ";
    /// <summary>
    /// JavaScript/TypeScript import keyword prefix.
    /// </summary>
    public const string Import = "import ";
    /// <summary>
    /// Removes specified using directives from the source text.
    /// </summary>
    /// <param name="text">Source code text to process.</param>
    /// <param name="removedUsings">Collection of namespace names to remove.</param>
    /// <returns>Text with the specified using directives removed.</returns>
    public static string RemoveUsing(string text, CollectionWithoutDuplicatesDC<string> removedUsings)
    {
        foreach (var usingNamespace in removedUsings.Collection)
        {
            text = SHReplace.ReplaceOnce(text, "using " + usingNamespace + ";", string.Empty);
        }

        return text;
    }

    /// <summary>
    /// Adds a static Type field (for ThrowEx) to every C# file that uses ThrowEx but lacks the type definition.
    /// </summary>
    /// <param name="BasePathsHelperVs">Base path to the Visual Studio projects folder.</param>
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
            var fileContent =
#if ASYNC
                await
#endif
            File.ReadAllTextAsync(item);
            if (fileContent.Contains("ThrowEx.") && !fileContent.Contains(mustContains))
            {
                var shared1Content = await File.ReadAllTextAsync(FS.InsertBetweenFileNameAndExtension(item, shared1));
                var shared2Content = await File.ReadAllTextAsync(FS.InsertBetweenFileNameAndExtension(item, shared2));
                if (shared1Content.Contains(mustContains) || shared2Content.Contains(mustContains))
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
                var lines = fileContent.Split(new string[] { fileContent.Contains("\r\n") ? "\r\n" : "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                bool areAllLinesCommented = true;
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (!trimmedLine.StartsWith("//"))
                    {
                        areAllLinesCommented = false;
                        break;
                    }
                }

                if (areAllLinesCommented)
                {
                    continue;
                }

                inserted = false;
                string? modifiedContent = null;
                for (i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("class "))
                    {
                        sbGeneric.Clear();
                        var line = lines[i];
                        var tokens = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        var classIndex = tokens.IndexOf("class");
                        if (tokens.Count > classIndex)
                        {
                            start = 0;
                            end = 0;
                            string className = tokens[classIndex + 1];
                            if (className.Contains("<"))
                            {
                                start += className.Count((ch) => ch == '<');
                                end += className.Count((ch) => ch == '>');
                                if (start == end)
                                {
                                    break;
                                }

                                for (int y = classIndex + 2; y < tokens.Count; y++)
                                {
                                    var token = tokens[y];
                                    start += token.Count((ch) => ch == '<');
                                    end += token.Count((ch) => ch == '>');
                                    sbGeneric.Append(token);
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

                        lines.Insert(insertTo, string.Format(template, tokens[classIndex + 1].Trim().TrimEnd('{') + sbGeneric.ToString()));
                        modifiedContent = string.Join(Environment.NewLine, lines);
                        inserted = true;
                        break;
                    }
                }

                if (inserted)
                {
                    await File.WriteAllTextAsync(item, modifiedContent);
                }
            }
        }
    }

    /// <param name="lines">Source code lines to modify in-place.</param>
    /// <param name="removed">Collection to store removed namespace names (can be null).</param>
    /// <param name="removeRegions">Whether to also remove region directives.</param>
    public static void RemoveNamespace(List<string> lines, CollectionWithoutDuplicatesDC<string>? removed, bool removeRegions = true)
    {
#region Remove namespace and #region from original
        const string startWith = "namespace ";
        List<string>? cLines = null;
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

    /// <summary>
    /// Converts static readonly string fields without values to const fields using their names as values.
    /// </summary>
    /// <param name="text">Source code lines to modify in-place.</param>
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
                    text[i] = text[i].TrimEnd(';') + " = " + SH.WrapWithQm(data.Item2!) + ";";
                }
            }
        }
    }

    /// <summary>
    /// Determines whether a line contains a string field/variable/const declaration and extracts its name.
    /// </summary>
    /// <param name="line">Source code line to analyze.</param>
    /// <returns>Tuple of (isConstField, fieldName). fieldName is null if not a const field.</returns>
    public static (bool, string?) IsFieldVariableConst(string line)
    {
        CsKeywordsList.Init();
        var text = line;
        text = text.Replace("readonly ", "");
        text = text.Replace("static ", "");
        text = text.Replace("const ", "");
        foreach (var item in CsKeywordsList.AccessModifier)
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