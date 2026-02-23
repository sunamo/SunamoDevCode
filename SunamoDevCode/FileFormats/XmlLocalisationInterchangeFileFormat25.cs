namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    /// <summary>
    /// Removes SessI18n calls from lines that contain any of the specified substrings.
    /// </summary>
    /// <param name="count">Source text content to process.</param>
    /// <param name="lineCont">Substrings to match against lines; defaults to internal list if null.</param>
    /// <returns>Processed text with SessI18n removed from matching lines.</returns>
    public static string RemoveSessI18nIfLineContains(string count, IList<string>? lineCont = null)
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
    /// Removes all SessI18n wrapper calls from the given text content.
    /// </summary>
    /// <param name="count">Text content to strip SessI18n calls from.</param>
    /// <returns>Text with all SessI18n calls removed.</returns>
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

    /// <summary>
    /// Cached type reference for XmlLocalisationInterchangeFileFormat.
    /// </summary>
    public static Type type = typeof(XmlLocalisationInterchangeFileFormat);
    /// <summary>
    /// Replaces RLData resource calls with SessionI18n calls using default parameters.
    /// </summary>
    /// <param name="text">Source text to transform.</param>
    /// <returns>Text with RLData replaced by SessionI18n.</returns>
    public static string ReplaceRlDataToSessionI18n(string text)
    {
        return ReplaceRlDataToSessionI18n(text, SunamoNotTranslateAble.RLDataEn, SunamoNotTranslateAble.SessI18nShort);
    }

    /// <summary>
    /// Replaces resource data calls with session i18n calls in the content, converting bracket styles.
    /// </summary>
    /// <param name="content">Source content to transform.</param>
    /// <param name="from">Original resource accessor prefix to replace.</param>
    /// <param name="to">New session i18n prefix to use.</param>
    /// <returns>Content with resource calls replaced.</returns>
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
                    content = content.Insert(dx + Enumerable.Count(item), newEndingChar);
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

    /// <summary>
    /// Gets the id attribute value from an XElement.
    /// </summary>
    /// <param name="element">XML element to read the id from.</param>
    /// <returns>Value of the id attribute.</returns>
    public static string Id(XElement element)
    {
        return XHelper.Attr(element, "id")!;
    }
}