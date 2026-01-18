namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    private static List<string> __xlfSolutions = new List<string>();
    private static Dictionary<string, string> __unallowedEnds = new Dictionary<string, string>();

    /// <summary>
    /// Copies keys that end with underscore, replacing HTML entities.
    /// </summary>
    public static void CopyKeysTrailedWith_()
    {
#region copy keys trailed with _
        List<string> consts = new List<string>();
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

    /// <summary>
    /// Trims whitespace from the start and end of an XML element's value if present.
    /// </summary>
    /// <param name="source">XML element to trim.</param>
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

    /// <summary>
    /// Gets the last character from a trans-unit's target value.
    /// </summary>
    /// <param name="item">Trans-unit XML element.</param>
    /// <returns>Last character or null if target is empty.</returns>
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

    /// <summary>
    /// Gets the last character from a trans-unit's target value and outputs the trans-unit ID.
    /// </summary>
    /// <param name="item">Trans-unit XML element.</param>
    /// <param name="id">Output parameter for the trans-unit ID.</param>
    /// <returns>Last character or null if target is empty.</returns>
    public static char? GetLastLetter(XElement item, out string id)
    {
        var transUnit = GetTransUnit(item);
        id = transUnit.Item1;
        if (transUnit.Item2.Length > 0)
        {
            return transUnit.Item2.Last();
        }

        return null;
    }

    /// <summary>
    /// Gets the target XML element from a trans-unit.
    /// </summary>
    /// <param name="item">Trans-unit XML element.</param>
    /// <returns>Target XML element.</returns>
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

    /// <summary>
    /// Gets XLF keys from C# code without RLData.en prefix.
    /// </summary>
    /// <param name="key">Reference parameter for the current key being processed.</param>
    /// <param name="content">C# file content to search.</param>
    /// <returns>List of unique keys found in the content.</returns>
    public static IList<string> GetKeysInCsWithoutRLDataEn(ref string key, string content)
    {
        List<string> foundKeys = new List<string>();
        var occurrences = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);
        occurrences.Reverse();
        StringBuilder stringBuilder = new StringBuilder(content);
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

    /// <summary>
    /// To be able to found with this method must be wrapped with XlfKeys and Translate.FromKey or RLData.en
    ///
    /// A3 is here only due to breakpoint for certain files
    /// </summary>
    /// <param name = "key"></param>
    /// <param name = "content"></param>
    /// <returns></returns>
    /// <summary>
    /// Gets XLF keys from C# code with RLData.en prefix.
    /// To be able to be found with this method, keys must be wrapped with XlfKeys and Translate.FromKey or RLData.en.
    /// The file parameter is here only due to breakpoint for certain files.
    /// </summary>
    /// <param name="key">Reference parameter for the current key being processed.</param>
    /// <param name="content">C# file content to search.</param>
    /// <param name="file">Optional file path for debugging purposes.</param>
    /// <returns>List of unique keys found in the content.</returns>
    public static IList<string> GetKeysInCsWithRLDataEn(ref string key, string content, string file = "")
    {
        List<string> foundKeys = new List<string>();
        var occurrences = SH.ReturnOccurencesOfString(content, XmlLocalisationInterchangeFileFormatSunamo.RLDataEn + XmlLocalisationInterchangeFileFormatSunamo.XlfKeysDot);
        occurrences.Reverse();
        StringBuilder stringBuilder = new StringBuilder(content);
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