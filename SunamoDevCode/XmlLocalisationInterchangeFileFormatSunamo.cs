// variables names: ok
namespace SunamoDevCode;

public class XmlLocalisationInterchangeFileFormatSunamo
{
    public const string Cs = "const string ";
    private const string eqBs = " = \"";

    /// <summary>
    ///     Translate.FromKey(
    /// </summary>
    public const string RLDataEn = SunamoNotTranslateAble.RLDataEn;

    public const string RLDataCs = SunamoNotTranslateAble.RLDataCs;
    public const string RLDataEn2 = SunamoNotTranslateAble.RLDataEn2;
    public const string SessI18n = SunamoNotTranslateAble.SessI18n;
    public const string XlfKeysDot = SunamoNotTranslateAble.XlfKeysDot;
    public const string SessI18nShort = SunamoNotTranslateAble.SessI18nShort;

    public static string PathXlfKeys = BasePathsHelper.vs + @"sunamo\sunamo\Constants\XlfKeys.cs";
    private static Type type = typeof(XmlLocalisationInterchangeFileFormatSunamo);
    public static string SunamoStringsDot = "SunamoStrings.";

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

        string replacePairs = null;

        #endregion

        var splitResult = SHSplit.SplitFromReplaceManyFormatList(replacePairs);
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
    ///     return code for getting from RLData.en
    /// </summary>
    /// <param name="key2"></param>
    public static string TextFromRLData(string pathOrExt, string key2)
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