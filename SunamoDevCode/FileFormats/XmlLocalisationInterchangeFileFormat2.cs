namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
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
    /// <param name = "item"></param>
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
    /// <param name = "source"></param>
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
    /// <param name = "key"></param>
    /// <param name = "content"></param>
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
}