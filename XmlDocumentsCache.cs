namespace SunamoDevCode;

public class XmlDocumentsCache
{
    private const string nullable = "<Nullable>enable</Nullable>";
    private const string debugTypeNone = "<DebugType>none</DebugType>";
    public static Type type = typeof(XmlDocumentsCache);
    public static Dictionary<string, XmlDocument> cache = new();

    /// <summary>
    ///     In key is csproj path
    ///     In value is absolute path of references (recursive)
    /// </summary>
    public static Dictionary<string, List<string>> projectDeps = new();

    public static Func<string, Dictionary<string, XmlDocument>,
#if ASYNC
            Task<List<string>>
#else
List<string>
#endif
        >
        buildProjectsDependencyTreeList;

    public static int nulled;
    public static IProgressBarDC clpb = null;
    public static List<string> cantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull = new();

    /// <summary>
    ///     Nemůže se volat společně s .Result! viz. https://stackoverflow.com/a/65820543/9327173 Způsobí to deadlock! Musím to
    ///     dělat přes ThisApp.async_
    ///     Can return null during many situations
    ///     For example when ignored => must always checking for null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>

    public static
#if ASYNC
        async Task<ResultWithExceptionDC<XmlDocument>>
#else
ResultWithException<XmlDocument>
#endif
        Get(string path)
    {
#if DEBUG
        if (path.EndsWith("duom.web.csproj"))
        {
        }
#endif
        // Tady to mít je píčovina. To se nemůže nikdy s malým vyskytnout
        //path = SH.FirstCharUpper(path);
        if (cache.ContainsKey(path)) return new ResultWithExceptionDC<XmlDocument>(cache[path]);
        if (Ignored.IsIgnored(path))
        {
            cache.Add(path, null);
            nulled++;
            return new ResultWithExceptionDC<XmlDocument> { Exc = "csproj is ignored: " + path };
        }

        // Load the XML document
        var doc = new XmlDocument();
        // HACK: XmlStreamReader will fail if the file is encoded in UTF-8 but has <?xml version="1.0" encoding="utf-16"?>
        // To work around this, we load the XML content into a string and use XmlDocument.LoadXml() instead.
        // Zde bylo async. Ale na řádku s .ConfigureAwait(false) se to zaseklo. Proto je tu teď pouze ReadAllText
        //if (cache.ContainsKey(path))
        //{
        //    return cache[path];
        //}
        string xml = null;
        //if (ThisApp.async_)
        //{
        xml =
#if ASYNC
            // Tohle nechápu. FubuCsProjFile i SunamoExceptions jsou net7.0.
            // co to je za dementní chybu This call site is reachable on all platforms. 'File.ReadAllTextAsyncAsync(string)' is only supported on: 'Windows' 7.0 and later.
            await
#endif
                File.ReadAllTextAsync(path);
        //}
        //else
        //{
        //    xml = File.ReadAllTextAsync(path);
        //}
        if (xml.Contains(GitConsts.startingHead))
        {
            cache.Add(path, null);
            nulled++;
            return new ResultWithExceptionDC<XmlDocument>();
        }

        var save = false;
        if (xml.Contains(nullable))
        {
            xml = xml.Replace(nullable, string.Empty);
            save = true;
        }

        if (xml.Contains(debugTypeNone))
        {
            xml = xml.Replace(debugTypeNone, string.Empty);
            save = true;
        }

        if (save) await File.WriteAllTextAsync(path, xml);
        xml = FormatXml(xml);
        if (xml.StartsWith("Exception:")) return new ResultWithExceptionDC<XmlDocument>(xml);
        try
        {
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            // Zřejmě mi toto vyhazovalo výjimku
            //var ox = doc.OuterXml;
            //if (cache.ContainsKey(path))
            //{
            //    return cache[path];
            //}
            cache.Add(path, doc);
        }
        catch
        {
            var p = cache.Keys.ToList().IndexOf(path);
            cache.Add(path, null);
            nulled++;
            //ThrowEx.NotValidXml(path, ex);
            return new ResultWithExceptionDC<XmlDocument>();
        }

        //lock (_lock)
        //{
        // Toto bych měl dělat mimo Parallel
        if (buildProjectsDependencyTreeList != null)
        {
            var l =
#if ASYNC
                await
#endif
                    buildProjectsDependencyTreeList(path, null);
            projectDeps.Add(path, l);
        }

        //}
        return new ResultWithExceptionDC<XmlDocument>(doc);
    }

    private static string FormatXml(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
        catch (Exception)
        {
            // Handle and throw if fatal exception here; don't just ignore them
            return xml;
        }
    }

    public static Dictionary<string, XmlDocument> BuildProjectDeps()
    {
        var xd = new Dictionary<string, XmlDocument>();
        // Všechny načtené XML dokumenty do xd
        foreach (var item in cache)
            // Zde je problémů několik. xd má pouhý 1 element, ačkoliv XmlDocumentsCache.cache jich má 41
            // projectDeps má poté 41.
            if (item.Value != null)
                xd.Add(item.Key, item.Value);
        return xd;
    }

    public static List<string> BadXml()
    {
        var withNull = cache.Where(s => s.Value == null);
        var bx = new List<string>();
        foreach (var item in withNull) bx.Add(item.Key);
        return bx;
    }

    public static
#if ASYNC
        async Task
#else
void
#endif
        Set(string path, string v, bool saveToFile = false)
    {
        var xd = new XmlDocument();
        xd.PreserveWhitespace = true;
        xd.LoadXml(v);
#if ASYNC
        await
#endif
            Set(path, xd, saveToFile);
    }

    public static
#if ASYNC
        async Task
#else
void
#endif
        Set(string path, XmlDocument v, bool saveToFile = false)
    {
        if (saveToFile)
        {
            v.PreserveWhitespace = true;
#if ASYNC
            await
#endif
                File.WriteAllTextAsync(path, v.OuterXml);
        }

        DictionaryHelper.AddOrSet(cache, path, v);
    }
}