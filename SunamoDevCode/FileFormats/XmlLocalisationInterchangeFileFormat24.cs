namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
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
    /// <param name = "path"></param>
    /// <param name = "ids"></param>
    /// <param name = "solutionsExcludeWhileWorkingOnSourceCode"></param>
    /// <param name = "addToNotToTranslateStrings"></param>
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
        var files = Directory.GetFiles(BasePathsHelper.VsProjects, "*.cs", SearchOption.AllDirectories);
        Dictionary<string, string> idTarget = new Dictionary<string, string>();
        var data = 
#if ASYNC
            await
#endif
        GetTransUnits(path);
        foreach (var item in data.TransUnits)
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
    /// <param name = "c"></param>
    /// <returns></returns>
     //public static string RemoveSessI18nIfLineContains(string count, params string[] lineCont)
    //{
    //    return RemoveSessI18nIfLineContainsWorker(count, removeSessI18nIfLineContains.ToArray());
    //}
    public static string RemoveSessI18nIfLineContains(string count)
    {
        return RemoveSessI18nIfLineContains(count, removeSessI18nIfLineContains);
    }
}