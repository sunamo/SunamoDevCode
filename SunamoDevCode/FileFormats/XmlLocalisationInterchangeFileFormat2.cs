// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode.FileFormats;

public static class XmlLocalisationInterchangeFileFormat
{
    static List<string> xlfSolutions = new List<string>();
    static Dictionary<string, string> unallowedEnds = new Dictionary<string, string>();

    public static void CopyKeysTrailedWith_()
    {
        #region copy keys trailed with _
        List<string> consts = new List<string>();
        AllLists.InitHtmlEntitiesFullNames();

        var val = AllLists.htmlEntitiesFullNames.Values.ToList();
        int i;
        for (i = 0; i < val.Count; i++)
        {
            val[i] = "_" + val[i];
        }

        var newConsts = new StringBuilder();
        var newConsts2 = new List<string>();
        //
        foreach (var item in consts)
        {
            var item3 = item;
            // replace all entity
            foreach (var item2 in val)
            {
                item3 = item3.Replace(item2, string.Empty);
            }

            if (!consts.Contains(item3) && !newConsts2.Contains(item3))
            {
                newConsts2.Add(item3);
                newConsts.AppendLine(string.Format(CSharpTemplates.constant, item3));
            }
        }

        //ClipboardHelper.SetText(newConsts.ToString());
        #endregion
    }

    static XmlLocalisationInterchangeFileFormat()
    {
        /*
SunamoAdmin
AllProjectsSearch
         */

        var slns = SHGetLines.GetLines(@"calc.sunamo.cz
ConsoleApp1
SczClientDesktop
sunamo.cz
sunamo.performance
sunamo.tasks
sunamo2
SunamoXlf
TranslateEngine");

        foreach (var item in slns)
        {
            xlfSolutions.Add(BasePathsHelper.vs + item);
        }
    }

    #region Takes XElement
    private static void TrimValueIfNot(XElement source)
    {
        if (source != null)
        {
            string sourceValue = source.Value;

            if (sourceValue.Length != 0)
            {
                if (char.IsWhiteSpace(sourceValue[sourceValue.Length - 1]) || char.IsWhiteSpace(sourceValue[0]))
                {
                    source.Value = sourceValue.Trim();
                }
            }
        }
    }
    public static char? GetLastLetter(XElement item)
    {
        string id = null;
        return GetLastLetter(item, out id);
    }

    static Tuple<string, string> GetTransUnit(XElement item)
    {
        string id = Id(item);
        XElement target = GetTarget(item);
        return new Tuple<string, string>(id, target.Value);
    }



    public static char? GetLastLetter(XElement item, out string id)
    {
        var temp = GetTransUnit(item);
        id = temp.Item1;
        if (temp.Item2.Length > 0)
        {
            return temp.Item2.Last();
        }

        return null;
    }

    public static XElement GetTarget(XElement item)
    {
        return XHelper.GetElementOfName(item, "target");
    }

    /// <summary>
    /// 0 - Source
    /// 1 - Target
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    static Tuple<XElement, XElement> SourceTarget(XElement item)
    {
        XElement source = XHelper.GetElementOfName(item, "source");
        XElement target = XHelper.GetElementOfName(item, "target");

        return new Tuple<XElement, XElement>(source, target);
    }

    /// <summary>
    /// Trim whitespaces from start/end
    /// </summary>
    /// <param name="source"></param>
    private static void TrimUnallowedChars(XElement source)
    {
        string sourceValue = source.Value;
        if (sourceValue.Length != 0)
        {
            if (char.IsWhiteSpace(sourceValue[sourceValue.Length - 1]) || char.IsWhiteSpace(sourceValue[0]))
            {
                source.Value = sourceValue.Trim();
            }
        }
    }


    #endregion





    public static IList<string> GetKeysInCsWithoutRLDataEn(ref string key, string content)
    {
        List<string> count = new List<string>();

        var occ = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);

        occ.Reverse();

        StringBuilder stringBuilder = new StringBuilder(content);

        foreach (var dx in occ)
        {
            var start = dx + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
            var end = -1;
            for (int i = start; i < content.Length; i++)
            {
                if (!char.IsLetterOrDigit(content[i]))
                {
                    end = i;
                    break;
                }
            }

            key = content.Substring(start, end - start);

            count.Add(key);
        }



        return count.Distinct().ToList();
    }



    /// <summary>
    /// To be able to found with this method must be wrapped with XlfKeys and Translate.FromKey or RLData.en
    ///
    /// A3 is here only due to breakpoint for certain files
    /// </summary>
    /// <param name="key"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static IList<string> GetKeysInCsWithRLDataEn(ref string key, string content, string file = "")
    {
        List<string> count = new List<string>();

        var occ = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.RLDataEn + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);

        occ.Reverse();

        StringBuilder stringBuilder = new StringBuilder(content);

        foreach (var dx in occ)
        {
            var start = dx + XmlLocalisationInterchangeFileFormatSunamo.RLDataEn.Length + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
            var end = content.IndexOf(']', start);

            key = content.Substring(start, end - start);

            count.Add(key);
        }

        occ = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.SessI18n + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);

        if (file.Contains("AboutApp"))
        {

        }

        occ.Reverse();

        foreach (var dx in occ)
        {
            var start = dx + XmlLocalisationInterchangeFileFormatSunamo.SessI18n.Length + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
            var end = content.IndexOf(')', start);

            key = content.Substring(start, end - start);

            count.Add(key);
        }

        return count.Distinct().ToList();
    }

    #region Manage * edit in *.xlf
    /// <summary>
    ///
    /// </summary>
    /// <param name="fn"></param>
    /// <param name="list"></param>
    /// <param name="idsEndingOn"></param>
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

        foreach (var item in data.trans_units)
        {
            string id = null;
            var lastLetter = GetLastLetter(item, out id).ToString();

            if (list.Any(data => data == lastLetter))
            {
                result[lastLetter].AppendLine(GetTarget(item).Value);
                idsEndingOn.Add(id);
            }
        }



        foreach (var item in result)
        {
            tb.Paragraph(item.Value, item.Key);
        }
        return new OutRefDC<string, List<string>>(tb.stringBuilder.ToString(), idsEndingOn);
    }



    #region Cant be in *.web - GetFilesCs
    /// <summary>
    /// Before mu
    /// </summary>
    /// <param name="path"></param>
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
                foreach (var k in keys)
                {
                    DictionaryHelper.AddOrSet(withWithoutUnderscore, k, ReplacerXlf.Instance.WithoutUnderscore(k));
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
        return FSGetFiles.GetFiles(logger, path, "*.cs", System.IO.SearchOption.AllDirectories, new GetFilesArgsDC()
        { /*excludeWithMethod = SunamoDevCodeHelper.RemoveTemporaryFilesVS*/ });
    }

    /// <summary>
    /// Is calling in XlfManager.WhichStartEndWithNonDigitNumber
    /// </summary>
    /// <param name="pairsReplace"></param>
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

        foreach (var item in xlfSolutions)
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
    #endregion



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
    /// <param name="fn"></param>
    /// <param name="saveAllLastLetterToClipboard"></param>
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
        List<XElement> tus = new List<XElement>();
        foreach (XElement item in data.trans_units)
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

        if (saveAllLastLetterToClipboard)
        {

            //ClipboardHelper.SetLines(allLastLetters.count.ConvertAll(data => data.ToString()));
        }

        return ids;
    }

    /// <summary>
    /// Into A1 insert XlfResourcesH.PathToXlfSunamo
    /// Completely IUN
    /// Remove completely whole Trans-unit
    /// </summary>
    /// <param name="fn"></param>
    public static
#if ASYNC
        async Task<string>
#else
string
#endif
        RemoveFromXlfWhichHaveEmptyTargetOrSource(string fn, XlfParts xp, RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs a = null)
    {
        if (a == null)
        {
            a = RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs.Default;
        }

        var data =
#if ASYNC
            await
#endif
                GetTransUnits(fn);
        List<XElement> tus = new List<XElement>();

        //string source =

        for (int i = data.trans_units.Count - 1; i >= 0; i--)
        {
            var item = data.trans_units[i];
            var el = SourceTarget(item);

            if (xp == XlfParts.Source)
            {
                if (el.Item1 != null)
                {
                    if (el.Item1.Value.Trim() == string.Empty)
                    {
                        if (a.removeWholeTransUnit)
                        {
                            el.Item1.Remove();
                        }
                        else
                        {
                            throw new Exception("Instead of this use <source>.*</source> in VS!");
                            el.Item1.Remove();
                        }
                    }
                }
            }
            else if (xp == XlfParts.Target)
            {
                if (el.Item2 != null)
                {
                    if (el.Item2.Value.Trim() == string.Empty)
                    {
                        if (a.removeWholeTransUnit)
                        {
                            el.Item2.Remove();
                        }
                        else
                        {
                            throw new Exception("Instead of this use <source>.*</source> in VS!");
                            el.Item2.Remove();
                        }
                    }
                }
            }
        }

        if (a.save)
        {
            data.xd.Save(fn);
        }

        return data.xd.ToString();
    }

    /// <summary>
    /// Trim whitespaces from start/end on source / target
    /// A1 is possible to obtain with XmlLocalisationInterchangeFileFormat.GetLangFromFilename
    /// </summary>
    /// <param name="enS"></param>
    public static
#if ASYNC
        async Task
#else
void
#endif
        TrimStringResources(string fn)
    {
        var data =
#if ASYNC
            await
#endif
                GetTransUnits(fn);
        List<XElement> tus = new List<XElement>();
        foreach (XElement item in data.trans_units)
        {
            XElement source = null;
            XElement target = null;

            var temp = SourceTarget(item);
            source = temp.Item1;
            target = temp.Item2;

            var id = Id(item);



            TrimValueIfNot(source);
            TrimValueIfNot(target);
        }

        data.xd.Save(fn);
    }



    /// <summary>
    /// A1 is possible to obtain with XlfResourcesH.PathToXlfSunamo
    /// </summary>
    /// <param name="fn"></param>
    /// <param name="xd"></param>
    public static
#if ASYNC
        async Task<XlfData>
#else
  XlfData
#endif
        GetTransUnits(string fn)
    {
        LangsDC toL = XmlLocalisationInterchangeFileFormatSunamo.GetLangFromFilename(fn);

        string enS =
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(fn);
        XlfData data = new XlfData();

        data.path = fn;

        XmlNamespacesHolder h = new XmlNamespacesHolder();
        h.ParseAndRemoveNamespacesXmlDocument(enS);

        data.xd =
#if ASYNC
            await
#endif
                XHelper.CreateXDocument(fn);

        XHelper.AddXmlNamespaces(h.nsmgr);

        XElement xliff = XHelper.GetElementOfName(data.xd, "xliff");
        var allElements = XHelper.GetElementsOfNameWithAttrContains(xliff, "file", "target-language", toL.ToString());
        var resources = allElements.Where(d2 => XHelper.Attr(d2, "original").Contains("/" + "RESOURCES" + "/"));
        XElement file = resources.First();
        XElement body = XHelper.GetElementOfName(file, "body");
        data.group = XHelper.GetElementOfName(body, "group");
        data.trans_units = XHelper.GetElementsOfName(data.group, TransUnit.tTransUnit);

        return data;
    }
    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="toL"></param>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="pascal"></param>
    /// <param name="fn"></param>
    public static
#if ASYNC
        async Task
#else
void
#endif
        Append(string source, string target, string pascal, string fn)
    {
        var data =
#if ASYNC
            await
#endif
                GetTransUnits(fn);

        var exists = XHelper.GetElementOfNameWithAttr(data.group, TransUnit.tTransUnit, "id", pascal);

        if (exists != null)
        {
            return;
        }

        Append(/*source,*/ target, pascal, data);
        data.xd.Save(fn);

        await XHelper.FormatXml(fn);
    }

    public static void Append(/*string source, */string target, string pascal, XlfData data)
    {
        TransUnit tu = new TransUnit();
        tu.id = pascal;
        // Directly set to null due to not inserting into .xlf
        tu.source = null;
        //tu.translate = true;
        // Inlined from SHTrim.TrimStartAndEnd - ořezává znaky ze začátku a konce podle podmínky
        var trimmedTarget = target;
        // Ořez ze začátku
        for (int i = 0; i < trimmedTarget.Length; i++)
        {
            if (!char.IsLetterOrDigit(trimmedTarget[i]))
            {
                trimmedTarget = trimmedTarget.Substring(1);
                i--;
            }
            else
            {
                break;
            }
        }
        // Ořez z konce
        for (int i = trimmedTarget.Length - 1; i >= 0; i--)
        {
            if (!char.IsLetterOrDigit(trimmedTarget[i]))
            {
                trimmedTarget = trimmedTarget.Remove(trimmedTarget.Length - 1, 1);
            }
            else
            {
                break;
            }
        }
        tu.target = trimmedTarget;

        var xml = tu.ToString();
        XElement xe = XElement.Parse(xml);
        xe = XHelper.MakeAllElementsWithDefaultNs(xe);

        data.group.Add(xe);
    }

    #region Cooperating XlfKeys and *.xlf
    public static async Task RemoveFromXlfAndXlfKeys(string fn, List<string> idsEndingEnd)
    {
        await RemoveFromXlfAndXlfKeys(fn, idsEndingEnd, XlfParts.Id);
    }

    /// <summary>
    /// AndXlfKeys
    /// </summary>
    /// <param name="fn"></param>
    /// <param name="p"></param>
    /// <param name="saveToClipboard"></param>
    public static
#if ASYNC
        async Task<List<string>>
#else
List<string>
#endif
        FromXlfWithDiacritic(string fn, XlfParts p, bool saveToClipboard = false)
    {
        // Dont use, its also non czech with diacritic hats tuồng (hats bôi)

        var data =
#if ASYNC
            await
#endif
                GetTransUnits(fn);

        List<string> r = new List<string>();

        if (p == XlfParts.Id)
        {
            foreach (var item in data.trans_units)
            {
                string idTransUnit = null;
                GetLastLetter(item, out idTransUnit);



                if (SH.ContainsDiacritic(idTransUnit))
                {
                    r.Add(idTransUnit);
                    // dont remove, just save ID, coz many strings have diac and is not czech hats tuồng (hats bôi)
                    //item.Remove();
                    //; break;

                }
            }

        }
        else if (p == XlfParts.Target)
        {

            foreach (var item in data.trans_units)
            {
                var target = GetTarget(item).Value;
                string idTransUnit = null;
                GetLastLetter(item, out idTransUnit);

                if (SH.ContainsDiacritic(target))
                {

                    r.Add(idTransUnit);
                    // dont remove, just save ID, coz many strings have diac and is not czech hats tuồng (hats bôi)
                    //item.Remove();

                }
            }

        }

        //if (saveToClipboard)
        //{
        //    ClipboardHelper.SetLines(r);
        //}

        return r;
    }

    public static
#if ASYNC
        async Task
#else
void
#endif
        RemoveFromXlfAndXlfKeys(string fn, List<string> idsEndingEnd, XlfParts p)
    {
        var data =
#if ASYNC
            await
#endif
                GetTransUnits(fn);

        bool removed = false;

        if (p == XlfParts.Id)
        {
            for (int i = idsEndingEnd.Count - 1; i >= 0; i--)
            {
                foreach (var item in data.trans_units)
                {
                    string idTransUnit = null;
                    GetLastLetter(item, out idTransUnit);

                    var id = idsEndingEnd[i];

                    if (id == idTransUnit)
                    {
                        item.Remove();
                        break;
                    }
                }
            }
        }
        else if (p == XlfParts.Target)
        {
            for (int i = idsEndingEnd.Count - 1; i >= 0; i--)
            {
                removed = false;

                foreach (var item in data.trans_units)
                {
                    var target = HtmlAssistant.HtmlDecode(GetTarget(item).Value);
                    var id = idsEndingEnd[i];

                    if (id == target)
                    {
                        try
                        {
                            item.Remove();
                            removed = true;
                        }
                        catch (Exception ex)
                        {
                            ThrowEx.ExcAsArg(ex, "Element can't be removed");
                            // have no parent
                        }

                        break;
                    }
                }

                if (!removed)
                {
#if DEBUG
                    //DebugLogger.Instance.WriteLine(idsEndingEnd[i]);
#endif
                }
            }
        }

        await CSharpParser.RemoveConsts(XmlLocalisationInterchangeFileFormatSunamo.pathXlfKeys, idsEndingEnd);

        data.xd.Save(fn);
    }

    public static
#if ASYNC
        async Task
#else
void
#endif
        RemoveDuplicatesInXlfFile(string xlfPath)
    {
        // There is no way to delete node in xlf file with XlfDocument.
        // XlfDocument is using XDocument but its private
        /*
         1) Use xliffParser in sunamo.notmine
         2) Load in my own XmlDocument and remove throught XPath
         */

        /*
        I HAVE IT IN XDOCUMENT, I WILL USE THEREFORE METHODS OF LINQ
        METHOD REMOVE() IS THERE ISNT FOR FUN!!
         */
        if (false)
        {
            //XlfData data;
            //var ids = GetIds(xlfPath, out data);

            //data.xd.XPathSelectElement("/xliff/file[original=@'WPF.TESTS/RESOURCES/EN-US.RESX']");

            //List<string> duplicated;

            //CAG.RemoveDuplicitiesList(ids, out duplicated);

            //var b2 = data.xd.Descendants().Count();


            //foreach (var item in duplicated)
            //{
            //    var elements = data.group.Elements().ToList();
            //    for (int i = 0; i < elements.Count(); i++)
            //    {
            //        var id = XHelper.Attr(elements[i], "id");
            //        if (id == item)
            //        {
            //            elements.Remove(elements[i]);
            //            break;
            //        }
            //    }
            //}

            //var b3 = data.xd.Descendants().Count();

            //data.xd.Save(xlfPath);
        }


        var allIds =
#if ASYNC
            await
#endif
                GetIds(xlfPath);
        XlfData xlfData = allIds.Item2;

        List<string> duplicated;
        CAG.RemoveDuplicitiesList<string>(allIds.Item1, out duplicated);

        foreach (var item in duplicated)
        {
            xlfData.trans_units.First(data => XHelper.Attr(data, "id") == item).Remove();
        }

        var outer = xlfData.xd.ToString();
        xlfData.xd.Save(xlfPath);
    }


    /// <summary>
    /// Into A1 pass XlfResourcesH.PathToXlfSunamo
    /// </summary>
    /// <param name="xlfPath"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static
#if ASYNC
        async Task<OutRefDC<List<string>, XlfData>>
#else
OutRef<List<string>, XlfData>
#endif
        GetIds(string xlfPath)
    {

        var xlfData =
#if ASYNC
            await
#endif
                XmlLocalisationInterchangeFileFormat.GetTransUnits(xlfPath);
        xlfData.FillIds();

        return new OutRefDC<List<string>, XlfData>(xlfData.allids, xlfData);
    }



    public static
#if ASYNC
        async Task
#else
void
#endif
        ReplaceStringKeysWithXlfKeys(string path)
    {


        List<string> files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories).ToList();
        await ReplaceStringKeysWithXlfKeys(files);
    }

    public static
#if ASYNC
        async Task
#else
void
#endif
        ReplaceStringKeysWithXlfKeys(List<string> files)
    {
        string key = null;

        foreach (var item in files)
        {
            var content =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(item);
            var content2 = ReplaceStringKeysWithXlfKeysWorker(ref key, content);
            if (content != content2)
            {
                await File.WriteAllTextAsync(item, content2);
            }
        }
    }



    public static string ReplaceStringKeysWithXlfKeysWorker(ref string key, string content)
    {

        var occ = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.SessI18n + "\"");

        occ.Reverse();

        StringBuilder stringBuilder = new StringBuilder(content);

        foreach (var dx in occ)
        {
            var start = dx + 1 + XmlLocalisationInterchangeFileFormatSunamo.SessI18n.Length;
            var end = content.IndexOf('"', start);

            key = content.Substring(start, end - start);

            stringBuilder.Remove(start - 1, end - start + 2);
            stringBuilder.Insert(start - 1, XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot + key);
        }

        return stringBuilder.ToString();
    }



    public static List<string> GetSunamoStrings()
    {
        var list = sunamoStrings.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = SHReplace.ReplaceOnce(list[i], SunamoNotTranslateAble.SessI18n + SunamoNotTranslateAble.XlfKeysDot, string.Empty).TrimEnd(')');
        }
        return list;
    }

    public static string ReplaceSunamoStringsWithSessI18n(string count)
    {
        var from = GetSunamoStrings();
        CA.Prepend("SunamoStrings.", from);
        var to = sunamoStrings;

        for (int i = 0; i < from.Count; i++)
        {
            count = count.Replace(from[i], to[i]);
        }
        return count;
    }



    /// <summary>
    /// ReplaceXlfKeysForString - Convert from XlfKeys to ""
    /// Cooperating with NotToTranslateStrings
    /// </summary>
    /// <param name="path"></param>
    /// <param name="ids"></param>
    /// <param name="solutionsExcludeWhileWorkingOnSourceCode"></param>
    /// <param name="addToNotToTranslateStrings"></param>
    public static
#if ASYNC
        async Task<OutRefDC<object, List<string>>>
#else
OutRef<object, CollectionWithoutDuplicates<string>>
#endif
        ReplaceXlfKeysForString(string path, List<string> ids, List<string> solutionsExcludeWhileWorkingOnSourceCode)
    {
        var addToNotToTranslateStrings = new List<string>();
        solutionsExcludeWhileWorkingOnSourceCode.Add("AllProjectsSearchTestFiles");

        CA.WrapWith(solutionsExcludeWhileWorkingOnSourceCode, @"\");

        Dictionary<string, string> filesWithXlf = new Dictionary<string, string>();

        var files = Directory.GetFiles(BasePathsHelper.vsProjects, "*.cs", SearchOption.AllDirectories);

        Dictionary<string, string> idTarget = new Dictionary<string, string>();

        var data =
#if ASYNC
            await
#endif
                GetTransUnits(path);

        foreach (var item in data.trans_units)
        {
            var temp = GetTransUnit(item);
            if (ids.Contains(temp.Item1))
            {
                idTarget.Add(temp.Item1, temp.Item2);
            }
        }

        foreach (var item in files)
        {
            bool continue2 = false;

            foreach (var item2 in solutionsExcludeWhileWorkingOnSourceCode)
            {
                if (item.Contains(item2))
                {
                    continue2 = true;
                    break;
                }
            }

            if (continue2)
            {
                continue;
            }

            var content =
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(item);
            if (content.Contains(XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot))
            {
                filesWithXlf.Add(item, content);
            }
        }

        List<string> replacedKeys = new List<string>();

        foreach (var kv in filesWithXlf)
        {
            var content = kv.Value;
            StringBuilder stringBuilder = new StringBuilder(content);

            replacedKeys.Clear();

            foreach (var item in ids)
            {
                var item2 = XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot + item + "]";
                var toReplace = XmlLocalisationInterchangeFileFormatSunamo.RLDataEn + item2;

                var toString = stringBuilder.ToString();
                var points = SH.ReturnOccurencesOfString(toString, toReplace);
                var points2 = SH.ReturnOccurencesOfString(toString, item2);

                if (points2.Count > points.Count)
                {

                }

                if (points.Count > 0)
                {
                    replacedKeys.Add(item);
                    addToNotToTranslateStrings.Add(idTarget[item]);
                }

                for (int i = points.Count - 1; i >= 0; i--)
                {
                    var dx = points[i];

                    var dxNextChar = dx + toReplace.Length;

                    stringBuilder.Remove(dx, toReplace.Length);
                    stringBuilder.Insert(dx, SH.WrapWithQm(idTarget[item]));
                }
            }

            replacedKeys = replacedKeys.Distinct().ToList();
            if (replacedKeys.Count > 0)
            {

                await File.WriteAllTextAsync(kv.Key, stringBuilder.ToString());
            }
        }
        return new OutRefDC<object, List<string>>(null, addToNotToTranslateStrings.Distinct().ToList());
        // Nepřidávat znovu pokud již končí na postfix
    }


    #endregion

    public static bool IsToBeInXlfKeys(string key)
    {
        var b1 = !SystemWindowsControls.StartingWithShortcutOfControl(key);
        var b2 = !key.StartsWith("Resources\\");
        var b3 = !CA.HasPostfix(key, ".PlaceholderText", ".Content");
        var b4 = !key.Contains(".");
        var b5 = !key.Contains("\"");
        return b1 && b2 && b3 && b4 && b5;
    }

    /// <summary>
    /// was collection with previously existed properties in SunamoStrings class like Translate.FromKey(XlfKeys.EditUserAccount)
    /// </summary>
    static readonly List<string> sunamoStrings = SHGetLines.GetLines(@"");


    /// <summary>
    /// XmlLocalisationInterchangeFileFormatSunamo.removeSessI18nIfLineContains
    /// </summary>
    public static List<string> removeSessI18nIfLineContains = new List<string>(["MSStoredProceduresI"]);

    /// <summary>
    /// Before is possible use ReplaceRlDataToSessionI18n
    /// Was earlier in sunamo, now in SunamoDevCode
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    //public static string RemoveSessI18nIfLineContains(string count, params string[] lineCont)
    //{
    //    return RemoveSessI18nIfLineContainsWorker(count, removeSessI18nIfLineContains.ToArray());
    //}

    public static string RemoveSessI18nIfLineContains(string count)
    {
        return RemoveSessI18nIfLineContains(count, removeSessI18nIfLineContains);
    }

    /// <summary>
    /// Was earlier in sunamo, now in SunamoDevCode
    /// </summary>
    /// <param name="c"></param>
    /// <param name="lineCont"></param>
    /// <returns></returns>
    public static string RemoveSessI18nIfLineContains(string count, IList<string> lineCont = null)
    {
        if (lineCont == null || lineCont.Count == 0)
        {
            lineCont = removeSessI18nIfLineContains;
        }

        count = XmlLocalisationInterchangeFileFormat.ReplaceRlDataToSessionI18n(count);

        var list = SHGetLines.GetLines(count);
        bool cont = false;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var line = list[i];
            cont = false;
            foreach (var item in lineCont)
            {
                if (line.Contains(item))
                {
                    cont = true;
                    break;
                }
            }

            if (cont)
            {
                list[i] = RemoveAllSessI18n(list[i]);
            }
        }

        return string.Join(Environment.NewLine, list);
    }

    /// <summary>
    /// Before is possible use ReplaceRlDataToSessionI18n
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string RemoveAllSessI18n(string count)
    {
        var stringBuilder = new StringBuilder(count);

        var sessI18n = XmlLocalisationInterchangeFileFormatSunamo.SessI18nShort;

        var occ = SH.ReturnOccurencesOfString(count, sessI18n);
        var ending = new List<int>(occ.Count);

        foreach (var item in occ)
        {
            ending.Add(count.IndexOf(')', item));
        }

        var list = sessI18n.Length;

        for (int i = occ.Count - 1; i >= 0; i--)
        {
            stringBuilder = stringBuilder.Remove(ending[i], 1);
            stringBuilder = stringBuilder.Remove(occ[i], list);
        }

        var result = stringBuilder.ToString();
        return result;
    }
    public static Type type = typeof(XmlLocalisationInterchangeFileFormat);

    public static string ReplaceRlDataToSessionI18n(string text)
    {
        return ReplaceRlDataToSessionI18n(text, SunamoNotTranslateAble.RLDataEn, SunamoNotTranslateAble.SessI18nShort);
    }

    public static string ReplaceRlDataToSessionI18n(string content, string from, string to)
    {
        var RLDataEn = SunamoNotTranslateAble.RLDataEn;
        var SessI18n = SunamoNotTranslateAble.SessI18nShort;
        var RLDataCs = SunamoNotTranslateAble.RLDataCs;

        char endingChar = ']';
        string newEndingChar = ")";
        if (from == SessI18n)
        {
            endingChar = ')';
            newEndingChar = "]";
        }
        else if (from == RLDataCs || from == RLDataEn)
        {
            // keep as is
        }
        else
        {
            ThrowEx.NotImplementedCase(from);
        }

        string SunamoStringsDot = XmlLocalisationInterchangeFileFormatSunamo.SunamoStringsDot;

        int dx = -1;

        foreach (var item in sunamoStrings)
        {
            dx = content.IndexOf((string)item);
            if (dx != -1)
            {
                var line = SH.GetLineFromCharIndex(content, SHGetLines.GetLines(content), dx);
                if (line.Contains(SunamoStringsDot))
                {
                    content = content.Insert(dx + Enumerable.Count(item)
                        , newEndingChar);
                    content = content.Remove(dx, SunamoStringsDot.Length);
                    content = content.Insert(dx, to + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);
                }
            }
        }

        var list = from.Length;

        content = content.Replace(XmlLocalisationInterchangeFileFormatSunamo.RLDataEn2, from);

        var occ = SH.ReturnOccurencesOfString(content, from);
        List<int> ending = new List<int>();
        foreach (var item in occ)
        {
            var io = content.IndexOf(endingChar, item);
            ending.Add(io);
        }

        StringBuilder stringBuilder = new StringBuilder(content);

        occ.Reverse();
        ending.Reverse();

        for (int i = 0; i < occ.Count; i++)
        {
            stringBuilder.Remove(occ[i], list);
            stringBuilder.Insert(occ[i], to);

            var ending2 = ending[i];
            stringBuilder.Remove(ending2, 1);
            stringBuilder.Insert(ending2, newEndingChar);
        }

        var count = stringBuilder.ToString();
        //TF.SaveFile(count, )
        return count;
    }
    public static string Id(XElement item)
    {
        return XHelper.Attr(item, "id");
    }
}