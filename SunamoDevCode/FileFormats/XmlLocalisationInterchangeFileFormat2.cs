namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    private static List<string> __xlfSolutions = new();
    private static Dictionary<string, string> __unallowedEnds = new();

    public static void CopyKeysTrailedWith_()
    {
#region copy keys trailed with _
        List<string> consts = new();
        AllLists.InitHtmlEntitiesFullNames();
        var values = AllLists.htmlEntitiesFullNames.Values.ToList();
        int i;
        for (i = 0; i < values.Count; i++)
        {
            values[i] = "_" + values[i];
        }

        var newConsts = new StringBuilder();
        var newConsts2 = new List<string>();
        //
        foreach (var item in consts)
        {
            var cleanedItem = item;
            // replace all entity
            foreach (var entity in values)
            {
                cleanedItem = cleanedItem.Replace(entity, string.Empty);
            }

            if (!consts.Contains(cleanedItem) && !newConsts2.Contains(cleanedItem))
            {
                newConsts2.Add(cleanedItem);
                newConsts.AppendLine(string.Format(CSharpTemplates.constant, cleanedItem));
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
            __xlfSolutions.Add(BasePathsHelper.Vs + item);
        }
    }

    private static void TrimValueIfNot(XElement source)
    {
        if (source is not null)
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
        string? id = null;
        return GetLastLetter(item, out id);
    }

    static Tuple<string, string> GetTransUnit(XElement item)
    {
        string id = Id(item);
        XElement target = GetTarget(item);
        return new Tuple<string, string>(id, target.Value);
    }

    public static char? GetLastLetter(XElement item, out string? id)
    {
        var transUnit = GetTransUnit(item);
        id = transUnit.Item1;
        if (transUnit.Item2.Length > 0)
        {
            return transUnit.Item2.Last();
        }

        return null;
    }

    public static XElement GetTarget(XElement item) => XHelper.GetElementOfName(item, "target")!;

    static Tuple<XElement, XElement> SourceTarget(XElement item)
    {
        XElement source = XHelper.GetElementOfName(item, "source")!;
        XElement target = XHelper.GetElementOfName(item, "target")!;
        return new Tuple<XElement, XElement>(source!, target!);
    }

    // Trim whitespaces from start/end
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
        List<string> foundKeys = new();
        var occurrences = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);
        occurrences.Reverse();
        StringBuilder stringBuilder = new(content);
        foreach (var index in occurrences)
        {
            var start = index + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
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
            foundKeys.Add(key);
        }

        return foundKeys.Distinct().ToList();
    }

    public static IList<string> GetKeysInCsWithRLDataEn(ref string key, string content, string file = "")
    {
        List<string> foundKeys = new();
        var occurrences = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.RLDataEn + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);
        occurrences.Reverse();
        StringBuilder stringBuilder = new(content);
        foreach (var index in occurrences)
        {
            var start = index + XmlLocalisationInterchangeFileFormatSunamo.RLDataEn.Length + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
            var end = content.IndexOf(']', start);
            key = content.Substring(start, end - start);
            foundKeys.Add(key);
        }

        occurrences = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.SessI18n + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);

        occurrences.Reverse();
        foreach (var index in occurrences)
        {
            var start = index + XmlLocalisationInterchangeFileFormatSunamo.SessI18n.Length + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot.Length;
            var end = content.IndexOf(')', start);
            key = content.Substring(start, end - start);
            foundKeys.Add(key);
        }

        return foundKeys.Distinct().ToList();
    }
}
