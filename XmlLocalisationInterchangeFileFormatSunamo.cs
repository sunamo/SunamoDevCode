namespace SunamoDevCode;

public partial class XmlLocalisationInterchangeFileFormatSunamo
{
    public static
#if ASYNC
    async Task
#else
    void
#endif
 ReplaceHtmlEntitiesWithEmpty()
    {
        var path = @"D:\a\sunamo.en-US.xlf";
        var content =
#if ASYNC
    await
#endif
 File.ReadAllTextAsync(path);
        #region
        List<string> consts = new List<string>();
        AllLists.InitHtmlEntitiesFullNames();

        var val = AllLists.htmlEntitiesFullNames.Values.ToList();
        int i;
        for (i = 0; i < val.Count; i++)
        {
            val[i] = "_" + val[i];
        }


        foreach (var item in val)
        {
            content = content.Replace(item, string.Empty);
        }


#if ASYNC
        await
#endif
        File.WriteAllTextAsync(path, content);
        #endregion
    }

    public static
#if ASYNC
    async Task
#else
    void
#endif
 ReplaceInXlfManuallyEnteredPairsWithPrependXlfKeys()
    {
        int i;

        #region MyRegion
        string replacePairs = null;
        #endregion
        var p = SHSplit.SplitFromReplaceManyFormatList(replacePairs);
        var to = p.Item1;
        var from = p.Item2;

        for (i = 0; i < from.Count; i++)
        {
            from[i] = from[i].Replace("XlfKeys.", string.Empty);
            to[i] = to[i].Replace("XlfKeys.", string.Empty);
        }

        from.Reverse();
        to.Reverse();

        var path = @"D:\a\sunamo.en-US.xlf";
        var content =
#if ASYNC
    await
#endif
 File.ReadAllTextAsync(path);

        for (i = from.Count - 1; i >= 0; i--)
        {
            //Debug.WriteLine(i);
            content = content.Replace(from[i], to[i]);
        }


#if ASYNC
        await
#endif
        File.WriteAllTextAsync(path, content);
    }

    public static void ConstsFromClipboard(string input)
    {
        var l = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //StringBuilder sb = new StringBuilder();
        //foreach (var item in l)
        //{
        //    sb.AppendLine(string.Format(template, item));
        //}

        //ClipboardHelper.SetText(sb.ToString());
    }



    /// <summary>
    /// Compare to whole line
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

        var l = (
#if ASYNC
    await
#endif
 File.ReadAllLinesAsync(pathXlfKeys)).ToList();

        for (i = 0; i < l.Count(); i++)
        {
            l[i] = l[i].Trim();
        }

        // only consts
        List<string> consts = new List<string>();
        // all lines
        List<string> constsAllLines = new List<string>();

        var c = "const ";


        i = 0;



        foreach (var item in l)
        {
            i++;
            if (item.Contains(c))
            {
                // Get consts names
                var s = GetConstsFromLine(item);
                consts.Add(s);
                constsAllLines.Add(s);
            }
            else
            {
                constsAllLines.Add(string.Empty);
            }
        }

        List<string> foundedDuplicities;
        CAG.RemoveDuplicitiesList<string>(constsAllLines, out foundedDuplicities);

        foundedDuplicities.Reverse();

        foreach (var item in foundedDuplicities)
        {
            if (item != string.Empty)
            {
                var dx = constsAllLines.IndexOf(item);
                l.RemoveAt(dx);
            }
        }

        await File.WriteAllLinesAsync(pathXlfKeys, l);
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
        var path = pathXlfKeys;
        var ls = (
#if ASYNC
    await
#endif
 File.ReadAllLinesAsync(path)).ToList();
        //var ls = SHGetLines.GetLines(s);
        int first;
        var consts = CSharpParser.ParseConsts(ls, out first);
        List<string> ls3;
        var ls2 = CAG.GetDuplicities<string>(consts, out ls3);

        //string t = CSharpHelper.GetConsts(ls, false);
        //var tl = SHGetLines.GetLines(t);
        for (i = ls2.Count - 1; i >= 0; i--)
        {
            for (y = 0; y < ls.Count; y++)
            {

                if (ls[y].Contains(AllStrings.space + ls2[i] + AllStrings.space))
                {
                    ls2.RemoveAt(i);
                    ls.RemoveAt(y);
                    i = ls2.Count - 1;
                    break;
                }
            }
        }

        await File.WriteAllLinesAsync(path, ls);
    }

    #region Mám už tady metodu GetKeysInCsWithRLDataEn, proto je toto zbytečné
    //public static List<string> UsedXlfKeysInCs(string c)
    //{
    //    List<string> usedKeys = new List<string>();

    //    var occ = SH.ReturnOccurencesOfString(c, SessI18n);
    //    var ending = new List<int>(occ.Count);

    //    foreach (var item in occ)
    //    {
    //        ending.Add(c.IndexOf(AllChars.rb, item));
    //    }

    //    var l = SessI18n.Length;
    //    var l2 = XlfKeysDot.Length;

    //    for (int i = occ.Count - 1; i >= 0; i--)
    //    {
    //        var k = SHSubstring.Substring(c, occ[i] + l + l2, ending[i], new SubstringArgs { returnInputIfIndexFromIsLessThanIndexTo = true } );
    //        if (k != c)
    //        {
    //            usedKeys.Add(k);
    //        }
    //    }

    //    return usedKeys;
    //}
    #endregion
}
