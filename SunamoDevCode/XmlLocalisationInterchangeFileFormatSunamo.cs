namespace SunamoDevCode;

/// <summary>
/// Provides utilities for working with XLIFF (XML Localisation Interchange File Format) files in the Sunamo ecosystem.
/// </summary>
public class XmlLocalisationInterchangeFileFormatSunamo
{
    /// <summary>
    /// C# const string declaration prefix.
    /// </summary>
    public const string Cs = "const string ";
    private const string eqBs = " = \"";

    /// <summary>
    ///     Translate.FromKey(
    /// </summary>
    public const string RLDataEn = SunamoNotTranslateAble.RLDataEn;

    /// <summary>
    /// Resource lookup data key for Czech language.
    /// </summary>
    public const string RLDataCs = SunamoNotTranslateAble.RLDataCs;
    /// <summary>
    /// Alternative resource lookup data key for English language.
    /// </summary>
    public const string RLDataEn2 = SunamoNotTranslateAble.RLDataEn2;
    /// <summary>
    /// Session i18n lookup prefix string.
    /// </summary>
    public const string SessI18n = SunamoNotTranslateAble.SessI18n;
    /// <summary>
    /// XlfKeys class name with dot separator for key lookup.
    /// </summary>
    public const string XlfKeysDot = SunamoNotTranslateAble.XlfKeysDot;
    /// <summary>
    /// Short form of session i18n lookup prefix.
    /// </summary>
    public const string SessI18nShort = SunamoNotTranslateAble.SessI18nShort;

    /// <summary>
    /// Path to the XlfKeys.cs constants file.
    /// </summary>
    public static string PathXlfKeys = BasePathsHelper.Vs + @"sunamo\sunamo\Constants\XlfKeys.cs";
    private static Type type = typeof(XmlLocalisationInterchangeFileFormatSunamo);
    /// <summary>
    /// Prefix for SunamoStrings class member access.
    /// </summary>
    public static string SunamoStringsDot = "SunamoStrings.";

    /// <summary>
    /// Removes all HTML entity references from the content of the specified XLIFF file.
    /// </summary>
    /// <param name="xlfPath">Path to the XLIFF file to process.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
        ReplaceHtmlEntitiesWithEmpty(string xlfPath)
    {

        var content =
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(xlfPath);

        #region

        var consts = new List<string>();
        AllLists.InitHtmlEntitiesFullNames();

        var values = AllLists.htmlEntitiesFullNames.Values.ToList();
        int i;
        for (i = 0; i < values.Count; i++) values[i] = "_" + values[i];


        foreach (var item in values) content = content.Replace(item, string.Empty);


#if ASYNC
        await
#endif
            File.WriteAllTextAsync(xlfPath, content);

        #endregion
    }

    /// <summary>
    /// Replaces manually entered key pairs in the XLIFF file by prepending XlfKeys prefix.
    /// </summary>
    /// <param name="xlfPath">Path to the XLIFF file to process.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
        ReplaceInXlfManuallyEnteredPairsWithPrependXlfKeys(string xlfPath)
    {
        int i;

        #region MyRegion

        string? replacePairs = null;

        #endregion

        var splitResult = SHSplit.SplitFromReplaceManyFormatList(replacePairs!);
        var to = splitResult.Item1;
        var from = splitResult.Item2;

        for (i = 0; i < from.Count; i++)
        {
            from[i] = from[i].Replace("XlfKeys.", string.Empty);
            to[i] = to[i].Replace("XlfKeys.", string.Empty);
        }

        from.Reverse();
        to.Reverse();


        var content =
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(xlfPath);

        for (i = from.Count - 1; i >= 0; i--)
            //Debug.WriteLine(i);
            content = content.Replace(from[i], to[i]);


#if ASYNC
        await
#endif
            File.WriteAllTextAsync(xlfPath, content);
    }

    /// <summary>
    /// Parses constant definitions from multiline text input.
    /// </summary>
    /// <param name="input">Multiline text containing constant definitions.</param>
    public static void ConstsFromClipboard(string input)
    {
        var lines = input.Split(new[] { input.Contains("\r\n") ? "\r\n" : "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        //StringBuilder stringBuilder = new StringBuilder();
        //foreach (var item in lines)
        //{
        //    stringBuilder.AppendLine(string.Format(template, item));
        //}

        //ClipboardHelper.SetText(stringBuilder.ToString());
    }


    /// <summary>
    ///     Compare to whole line
    /// </summary>
    public static
#if ASYNC
        async Task
#else
    void
#endif
        RemoveDuplicatedXlfKeysConsts()
    {
        int i;

        var lines = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(PathXlfKeys)).ToList();

        for (i = 0; i < lines.Count; i++) lines[i] = lines[i].Trim();

        // only consts
        var consts = new List<string>();
        // all lines
        var constsAllLines = new List<string>();

        var count = "const ";


        i = 0;


        foreach (var item in lines)
        {
            i++;
            if (item.Contains(count))
            {
                // Get consts names
                var text = GetConstsFromLine(item);
                consts.Add(text);
                constsAllLines.Add(text);
            }
            else
            {
                constsAllLines.Add(string.Empty);
            }
        }

        List<string> foundedDuplicities;
        CAG.RemoveDuplicitiesList(constsAllLines, out foundedDuplicities);

        foundedDuplicities.Reverse();

        foreach (var item in foundedDuplicities)
            if (item != string.Empty)
            {
                var dx = constsAllLines.IndexOf(item);
                lines.RemoveAt(dx);
            }

        await File.WriteAllLinesAsync(PathXlfKeys, lines);
    }

    /// <summary>
    /// Removes duplicated XLF key constants from the XlfKeys source file, keeping only unique entries.
    /// </summary>
    public static
#if ASYNC
        async Task
#else
    void
#endif
        RemoveDuplicatedXlfKeysConsts2()
    {
        int y, i;
        //AllLists.InitHtmlEntitiesDict();
        var path = PathXlfKeys;
        var sourceList = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(path)).ToList();
        //var sourceList = SHGetLines.GetLines(text);
        int first;
        var consts = CSharpParser.ParseConsts(sourceList, out first);
        List<string> ls3;
        var ls2 = CAG.GetDuplicities(consts, out ls3);

        //string t = CSharpHelper.GetConsts(sourceList, false);
        //var tl = SHGetLines.GetLines(t);
        for (i = ls2.Count - 1; i >= 0; i--)
            for (y = 0; y < sourceList.Count; y++)
                if (sourceList[y].Contains(" " + ls2[i] + " "))
                {
                    ls2.RemoveAt(i);
                    sourceList.RemoveAt(y);
                    i = ls2.Count - 1;
                    break;
                }

        await File.WriteAllLinesAsync(path, sourceList);
    }

    /// <summary>
    /// Extracts the constant name from a C# const declaration line.
    /// </summary>
    /// <param name="line">Line of C# code containing a const declaration.</param>
    /// <returns>The constant name extracted from the line.</returns>
    public static string GetConstsFromLine(string line)
    {
        return SH.GetTextBetweenSimple(line, Cs, eqBs, false);
    }

#pragma warning disable
    public static LangsDC GetLangFromFilename(string text)
    {
        return LangsDC.cs;
        //return XmlLocalisationInterchangeFileFormatXlf.GetLangFromFilename(text);
    }
#pragma warning restore


    /// <summary>
    /// Returns the code snippet for getting a localized value from RLData based on the file extension.
    /// </summary>
    /// <param name="pathOrExt">File path or extension to determine the output format.</param>
    /// <param name="key2">Localization key to use in the generated code.</param>
    /// <returns>Generated code snippet for the appropriate language, or null if not implemented.</returns>
    public static string? TextFromRLData(string pathOrExt, string key2)
    {
        var ext = Path.GetExtension(pathOrExt);
        // Inlined from SH.PrefixIfNotStartedWith - přidává prefix pokud řetězec nezačíná daným prefixem
        if (!ext.StartsWith("."))
        {
            ext = "." + ext;
        }
        if (ext == AllExtensions.CsExtension)
            return SessI18n + XlfKeysDot + key2 + ")";
        if (ext == AllExtensions.TsExtension) return "su.en(\"" + key2 + "\")";
        ThrowEx.NotImplementedCase(ext);
        return null;
    }

    #region Mám už tady metodu GetKeysInCsWithRLDataEn, proto je toto zbytečné

    //public static List<string> UsedXlfKeysInCs(string count)
    //{
    //    List<string> usedKeys = new List<string>();

    //    var occ = SH.ReturnOccurencesOfString(count, SessI18n);
    //    var ending = new List<int>(occ.Count);

    //    foreach (var item in occ)
    //    {
    //        ending.Add(count.IndexOf(')', item));
    //    }

    //    var lines = SessI18n.Length;
    //    var l2 = XlfKeysDot.Length;

    //    for (int i = occ.Count - 1; i >= 0; i--)
    //    {
    //        var k = SHSubstring.Substring(count, occ[i] + lines + l2, ending[i], new SubstringArgs { returnInputIfIndexFromIsLessThanIndexTo = true } );
    //        if (k != count)
    //        {
    //            usedKeys.Add(k);
    //        }
    //    }

    //    return usedKeys;
    //}

    #endregion
}