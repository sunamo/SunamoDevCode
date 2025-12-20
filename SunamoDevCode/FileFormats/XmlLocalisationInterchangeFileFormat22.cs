// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.FileFormats;
public static partial class XmlLocalisationInterchangeFileFormat
{
    /// <summary>
    /// Into A1 insert XlfResourcesH.PathToXlfSunamo
    /// Completely IUN
    /// Remove completely whole Trans-unit
    /// </summary>
    /// <param name = "fn"></param>
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
    /// <param name = "enS"></param>
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
    /// <param name = "fn"></param>
    /// <param name = "xd"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name = "toL"></param>
    /// <param name = "source"></param>
    /// <param name = "target"></param>
    /// <param name = "pascal"></param>
    /// <param name = "fn"></param>
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

        Append( /*source,*/target, pascal, data);
        data.xd.Save(fn);
        await XHelper.FormatXml(fn);
    }

    public static void Append( /*string source, */string target, string pascal, XlfData data)
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

    public static async Task RemoveFromXlfAndXlfKeys(string fn, List<string> idsEndingEnd)
    {
        await RemoveFromXlfAndXlfKeys(fn, idsEndingEnd, XlfParts.Id);
    }
}