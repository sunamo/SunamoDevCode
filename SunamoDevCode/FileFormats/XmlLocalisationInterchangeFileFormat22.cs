namespace SunamoDevCode.FileFormats;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public static partial class XmlLocalisationInterchangeFileFormat
{
    /// <summary>
    /// Into A1 insert XlfResourcesH.PathToXlfSunamo
    /// Completely IUN
    /// Remove completely whole Trans-unit
    /// </summary>
    /// <param name="fn">Path to the XLF file to process.</param>
    /// <param name="xp">Which XLF part (Source or Target) to check for empty values.</param>
    /// <param name="a">Options controlling removal behavior (defaults to Default if null).</param>
    /// <returns>String representation of the modified XML document.</returns>
    public static
#if ASYNC
        async Task<string>
#else
    string
#endif
    RemoveFromXlfWhichHaveEmptyTargetOrSource(string fn, XlfParts xp, RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs? a = null)
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
        for (int i = data.TransUnits.Count - 1; i >= 0; i--)
        {
            var item = data.TransUnits[i];
            var el = SourceTarget(item);
            if (xp == XlfParts.Source)
            {
                if (el.Item1 != null)
                {
                    if (el.Item1.Value.Trim() == string.Empty)
                    {
                        if (a.RemoveWholeTransUnit)
                        {
                            el.Item1.Remove();
                        }
                        else
                        {
                            throw new Exception("Instead of this use <source>.*</source> in VS!");
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
                        if (a.RemoveWholeTransUnit)
                        {
                            el.Item2.Remove();
                        }
                        else
                        {
                            throw new Exception("Instead of this use <source>.*</source> in VS!");
                        }
                    }
                }
            }
        }

        if (a.Save)
        {
            data.XmlDocument.Save(fn);
        }

        return data.XmlDocument.ToString();
    }

    /// <summary>
    /// Trim whitespaces from start/end on source / target
    /// A1 is possible to obtain with XmlLocalisationInterchangeFileFormat.GetLangFromFilename
    /// </summary>
    /// <param name="fn">Path to the XLF file to trim.</param>
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
        foreach (XElement item in data.TransUnits)
        {
            XElement? source = null;
            XElement? target = null;
            var temp = SourceTarget(item);
            source = temp.Item1;
            target = temp.Item2;
            var id = Id(item);
            TrimValueIfNot(source);
            TrimValueIfNot(target);
        }

        data.XmlDocument.Save(fn);
    }

    /// <summary>
    /// A1 is possible to obtain with XlfResourcesH.PathToXlfSunamo
    /// </summary>
    /// <param name="fn">Path to the XLF file to parse.</param>
    /// <returns>XlfData containing the parsed trans-units and XML document.</returns>
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
        data.Path = fn;
        XmlNamespacesHolder h = new XmlNamespacesHolder();
        h.ParseAndRemoveNamespacesXmlDocument(enS);
        data.XmlDocument = 
#if ASYNC
            await
#endif
        XHelper.CreateXDocument(fn);
        XHelper.AddXmlNamespaces(h.NamespaceManager);
        XElement xliff = XHelper.GetElementOfName(data.XmlDocument, "xliff")!;
        var allElements = XHelper.GetElementsOfNameWithAttrContains(xliff!, "file", "target-language", toL.ToString());
        var resources = allElements.Where(d2 => XHelper.Attr(d2, "original")!.Contains("/" + "RESOURCES" + "/"));
        XElement file = resources.First();
        XElement body = XHelper.GetElementOfName(file, "body")!;
        data.Group = XHelper.GetElementOfName(body!, "group")!;
        data.TransUnits = XHelper.GetElementsOfName(data.Group!, TransUnit.TransUnitTagName);
        return data;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source">Source text for the translation unit.</param>
    /// <param name="target">Target text for the translation unit.</param>
    /// <param name="pascal">Pascal case identifier used as the trans-unit ID.</param>
    /// <param name="fn">Path to the XLF file to append to.</param>
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
        var exists = XHelper.GetElementOfNameWithAttr(data.Group, TransUnit.TransUnitTagName, "id", pascal);
        if (exists != null)
        {
            return;
        }

        Append( /*source,*/target, pascal, data);
        data.XmlDocument.Save(fn);
        await XHelper.FormatXml(fn);
    }

    /// <summary>
    /// Appends a new trans-unit element with the specified target text and ID to the XLF data group.
    /// </summary>
    /// <param name="target">Target text for the translation unit.</param>
    /// <param name="pascal">Pascal case identifier used as the trans-unit ID.</param>
    /// <param name="data">XLF data containing the group to append to.</param>
    public static void Append( /*string source, */string target, string pascal, XlfData data)
    {
        TransUnit tu = new TransUnit();
        tu.Id = pascal;
        // Directly set to null due to not inserting into .xlf
        tu.Source = null!;
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

        tu.Target = trimmedTarget;
        var xml = tu.ToString()!;
        XElement xe = XElement.Parse(xml!);
        xe = XHelper.MakeAllElementsWithDefaultNs(xe);
        data.Group.Add(xe);
    }

    /// <summary>
    /// Removes trans-units from both XLF file and XLF keys by matching IDs.
    /// </summary>
    /// <param name="fn">Path to the XLF file.</param>
    /// <param name="idsEndingEnd">List of ID endings to match for removal.</param>
    public static async Task RemoveFromXlfAndXlfKeys(string fn, List<string> idsEndingEnd)
    {
        await RemoveFromXlfAndXlfKeys(fn, idsEndingEnd, XlfParts.Id);
    }
}