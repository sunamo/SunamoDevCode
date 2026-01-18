namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    /// <summary>
    ///
    /// </summary>
    /// <param name = "fn"></param>
    /// <param name = "list"></param>
    /// <param name = "idsEndingOn"></param>
    /// <returns></returns>
    public static 
#if ASYNC
        async Task<OutRefDC<string, List<string>>>
#else
    OutRef<string, List<string>> 
#endif
    ReturnEndingOn(string fn, List<string> list)
    {
        /*

! - always text
. - Always text
( - more often text
) - more often text
* - 50/50
, -  50/50
- Always text

Into A1 insert:
+ - all code
' - alwyas code
/ - always path
         */
        list = CAChangeContent.ChangeContent0(null, list, temp => SHParts.RemoveAfterFirst(temp, ' '));
        var idsEndingOn = new List<string>();
        Dictionary<string, StringBuilder> result = new Dictionary<string, StringBuilder>();
        TextOutputGenerator tb = new TextOutputGenerator();
        var data = 
#if ASYNC
            await
#endif
        GetTransUnits(fn);
        foreach (var item in list)
        {
            result.Add(item, new StringBuilder());
        }

        foreach (var item in data.TransUnits)
        {
            string id = null;
            var lastLetter = GetLastLetter(item, out id).ToString();
            if (list.Any(letter => letter == lastLetter))
            {
                result[lastLetter].AppendLine(GetTarget(item).Value);
                idsEndingOn.Add(id);
            }
        }

        foreach (var item in result)
        {
            tb.Paragraph(item.Value, item.Key);
        }

        return new OutRefDC<string, List<string>>(tb.StringBuilder.ToString(), idsEndingOn);
    }

    /// <summary>
    /// Before mu
    /// </summary>
    /// <param name = "path"></param>
    public static 
#if ASYNC
        async Task
#else
    void 
#endif
    ReplaceForWithoutUnderscore(ILogger logger, string folder)
    {
        Dictionary<string, string> withWithoutUnderscore = new Dictionary<string, string>();
        var files = XmlLocalisationInterchangeFileFormat.GetFilesCs(logger);
#if ASYNC
        await
#endif
        ReplaceStringKeysWithXlfKeys(files);
        string key = null;
        foreach (var item in files)
        {
            withWithoutUnderscore.Clear();
            var content = 
#if ASYNC
                await
#endif
            File.ReadAllTextAsync(item);
            var keys = GetKeysInCsWithRLDataEn(ref key, content);
            if (keys.Count > 0)
            {
                foreach (var keyWithUnderscore in keys)
                {
                    DictionaryHelper.AddOrSet(withWithoutUnderscore, keyWithUnderscore, ReplacerXlf.Instance.WithoutUnderscore(keyWithUnderscore));
                }

                foreach (var item2 in withWithoutUnderscore)
                {
                    content = content.Replace(item2.Key + '[', item2.Value + '[');
                }

                await File.WriteAllTextAsync(item, content);
            }
        }
    }

    public static List<string> GetFilesCs(ILogger logger, string path = null)
    {
        return FSGetFiles.GetFiles(logger, path, "*.cs", System.IO.SearchOption.AllDirectories, new GetFilesArgsDC() { /*excludeWithMethod = SunamoDevCodeHelper.RemoveTemporaryFilesVS*/ });
    }

    /// <summary>
    /// Is calling in XlfManager.WhichStartEndWithNonDigitNumber
    /// </summary>
    /// <param name = "pairsReplace"></param>
    public static 
#if ASYNC
        async Task
#else
    void 
#endif
    ReplaceInXlfSolutions(ILogger logger, string pairsReplace)
    {
        if (pairsReplace == string.Empty)
        {
            System.Diagnostics.Debugger.Break();
        }

        var temp = SHSplit.SplitFromReplaceManyFormatList(pairsReplace);
        var from = temp.Item1;
        var to = temp.Item2;
        foreach (var item in __xlfSolutions)
        {
            var files = GetFilesCs(logger, item);
            foreach (var item2 in files)
            {
                var content = 
#if ASYNC
                    await
#endif
                File.ReadAllTextAsync(item2);
                content = content.Replace("\"-\"+\"-\"", "\"-\"");
                for (int i = 0; i < from.Count; i++)
                {
                    content = content.Replace(from[i], to[i]);
                }

                await File.WriteAllTextAsync(item2, content);
            //break;
            }
        //break;
        }
    }

    //    public static
    //#if ASYNC
    //        async Task<XlfData>
    //#else
    //  XlfData
    //#endif
    //        GetTransUnits(LangsDC en)
    //    {
    //        return null;
    //        //        return
    //        //#if ASYNC
    //        //    await
    //        //#endif
    //        //    GetTransUnits(XlfResourcesH.PathToXlfSunamo(en));
    //    }
    /// <summary>
    /// Is used nowhere
    /// Was in MainWindow but probably was replaced with GetAllLastLetterFromEnd
    /// </summary>
    /// <param name = "fn"></param>
    /// <param name = "saveAllLastLetterToClipboard"></param>
    /// <returns></returns>
    public static 
#if ASYNC
        async Task<List<string>>
#else
    List<string> 
#endif
    GetAllLastLetterFromEnd(string fn, bool saveAllLastLetterToClipboard)
    {
        List<string> ids = new List<string>();
        List<char> allLastLetters = new List<char>();
        var data = 
#if ASYNC
            await
#endif
        GetTransUnits(fn);
        foreach (XElement item in data.TransUnits)
        {
            string id;
            var ch = GetLastLetter(item, out id);
            if (ch.HasValue)
            {
                allLastLetters.Add(ch.Value);
            }

            ids.Add(id);
        }

        allLastLetters = allLastLetters.Distinct().ToList();
        allLastLetters.Sort();

        return ids;
    }
}