namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    /// <summary>
    /// Gets trans-unit IDs or targets from XLF file that contain diacritics.
    /// </summary>
    /// <param name="fn">Path to the XLF file.</param>
    /// <param name="p">Which part of the trans-unit to check (Id or Target).</param>
    /// <param name="saveToClipboard">Reserved for clipboard functionality.</param>
    /// <returns>List of trans-unit IDs that contain diacritics.</returns>
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
            foreach (var item in data.TransUnits)
            {
                string? idTransUnit = null;
                GetLastLetter(item, out idTransUnit);
                if (SH.ContainsDiacritic(idTransUnit!))
                {
                    r.Add(idTransUnit!);
                // dont remove, just save ID, coz many strings have diac and is not czech hats tuồng (hats bôi)
                //item.Remove();
                //; break;
                }
            }
        }
        else if (p == XlfParts.Target)
        {
            foreach (var item in data.TransUnits)
            {
                var target = GetTarget(item).Value;
                string? idTransUnit = null;
                GetLastLetter(item, out idTransUnit);
                if (SH.ContainsDiacritic(target))
                {
                    r.Add(idTransUnit!);
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

    /// <summary>
    /// Removes specified trans-units from the XLF file and corresponding constants from XlfKeys.
    /// </summary>
    /// <param name="fn">Path to the XLF file.</param>
    /// <param name="idsEndingEnd">List of IDs or target values to remove.</param>
    /// <param name="p">Which part to match against (Id or Target).</param>
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
                foreach (var item in data.TransUnits)
                {
                    string? idTransUnit = null;
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
                foreach (var item in data.TransUnits)
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

        await CSharpParser.RemoveConsts(XmlLocalisationInterchangeFileFormatSunamo.PathXlfKeys, idsEndingEnd);
        data.XmlDocument.Save(fn);
    }

    /// <summary>
    /// Removes duplicate trans-units from an XLF file, keeping only the first occurrence of each ID.
    /// </summary>
    /// <param name="xlfPath">Path to the XLF file to deduplicate.</param>
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
        //data.XmlDocument.XPathSelectElement("/xliff/file[original=@'WPF.TESTS/RESOURCES/EN-US.RESX']");
        //List<string> duplicated;
        //CAG.RemoveDuplicitiesList(ids, out duplicated);
        //var b2 = data.XmlDocument.Descendants().Count();
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
        //var b3 = data.XmlDocument.Descendants().Count();
        //data.XmlDocument.Save(xlfPath);
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
            xlfData.TransUnits.First(data => XHelper.Attr(data, "id") == item).Remove();
        }

        var outer = xlfData.XmlDocument.ToString();
        xlfData.XmlDocument.Save(xlfPath);
    }

    /// <summary>
    /// Gets all trans-unit IDs from an XLF file along with the parsed XLF data.
    /// </summary>
    /// <param name="xlfPath">Path to the XLF file.</param>
    /// <returns>OutRef containing list of IDs and the parsed XlfData.</returns>
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
        return new OutRefDC<List<string>, XlfData>(xlfData.AllIds, xlfData);
    }

    /// <summary>
    /// Replaces string keys with XLF keys in all C# files under the given path.
    /// </summary>
    /// <param name="path">Root directory to scan for C# files.</param>
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
}