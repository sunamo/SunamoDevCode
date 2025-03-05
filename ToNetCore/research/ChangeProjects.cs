namespace SunamoDevCode.ToNetCore.research;

public class ChangeProjects
{
    public const string start = "<TargetFramework>";
    public const string end = "</TargetFramework>";
    public const string netstandard20 = "netstandard2.0";
    public const string net48 = "net48";
    public const string net60 = "net6.0";
    /// <summary>
    /// Na mém PC bude mít vše net7.0-windows
    /// Bylo by to velmi náročné dělat průběžné změny mezi net7.0 a net7.0-windows
    /// </summary>
    public const string net70Windows = "net7.0-windows";
    public const string netstandard21 = "netstandard2.1";
    public const string net5Uap = "net5.0-windows10.0.19041.0";
    public const string netcoreapp = "netcoreapp1.1";
    public const string net472 = "net472";
    public const string net7Windows = "net7.0-windows";
    public static void Test()
    {
        var a = ChangeProjects.IsNetCore5UpMoniker(netstandard20);
        var b = ChangeProjects.IsNetCore5UpMoniker(netcoreapp);
        var c = ChangeProjects.IsNetCore5UpMoniker(net70Windows);
        var d = ChangeProjects.IsNetCore5UpMoniker(net7Windows);
    }
    public static void ChangeProjectsTo(ILogger logger, string to2, bool web)
    {
        var s = MoveToNet5.ci.WebAndNonWebProjects(logger, true);
        ChangeProjectsTo(net70Windows, s.Item2);
    }
    public static IsNetCore5UpMonikerResult IsNetCore5UpMoniker(string moniker)
    {
        if (!moniker.StartsWith("net"))
        {
            return null;
        }
        if (moniker.Length < 6)
        {
            return null;
        }
        if (BTS.IsInt(moniker[3].ToString()) && moniker[4] == '.' && BTS.IsInt(moniker[5].ToString()))
        {
            return new IsNetCore5UpMonikerResult { targetFramework = moniker.Substring(0, 6), platformTfm = SHSubstring.SubstringIfAvailableStart(moniker, 6) };
        }
        return null;
    }
    public static void ChangeProjectsTo(string to2, List<TWithStringDC<string>> l)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in l)
        {
            var content = item.t;
            var path = item.path;
            ChangeProjectTo(to2, path, parsedMonikerTo);
        }
    }
    public static void AllTargetFrameworks(bool web)
    {
    }
    /// <summary>
    /// Replace only between TargetFramework
    /// </summary>
    /// <param name="to2"></param>
    /// <param name="path"></param>
    /// <param name="dontChangeIfSourceIs"></param>
    public static
#if ASYNC
    async Task
#else
    void  
#endif
 ChangeProjectTo(string to2, string path, IsNetCore5UpMonikerResult parsedMonikerTo, string dontChangeIfSourceIs = null)
    {
#if DEBUG
        //if (path.Contains("ExCSS2.csproj"))
        //{
        //}
        //if (path == @"E:\vs\Mono_Projects\monoConsoleSqlClient\consoleSqlClient\monoConsoleSqlClient.csproj")
        //{
        //    //net7.0 
        //}
        //if (path == @"E:\vs\Projects\_ut2\AllProjectsSearch.Cmd.Tests\Runner\Runner.csproj")
        //{
        //    // net6.0-windows
        //}
        //if (path == @"E:\vs\Mono_Projects\monoConsoleSqlClient\consoleSqlClient\monoConsoleSqlClient.csproj")
        //{
        //    //net7.0
        //}
#endif
        var xd =
#if ASYNC
    await
#endif
 XmlDocumentsCache.Get(path);
        if (MayExcHelper.MayExc(xd.Exc))
        {
            return;
        }
        var content = xd.Data.OuterXml;
        var tf = SH.GetTextBetween(content, start, end, false);
        if (tf == null)
        {
            // Může se stát když to není v non sdk style
            return;
        }
        if (dontChangeIfSourceIs != null && dontChangeIfSourceIs == tf)
        {
            return;
        }
        var parsedMonikerFrom = IsNetCore5UpMoniker(tf);
        // už nechci, nestačí aby byly stejné targetFramework, musí být stejné i TFM. Vše na mém kompu bude -windows
        //if (parsedMonikerFrom?.targetFramework == parsedMonikerTo?.targetFramework)
        //{
        //    return;
        //}
        if (tf != to2)
        {
            string from = null;
            string to = null;
            from = start + tf + end;
            if (parsedMonikerFrom == null || parsedMonikerTo == null)
            {
                // není to net core, můžu to nahradit za cokoliv
                to = start + to2 + end;
            }
            else
            {
                IsNetCore5UpMonikerResult monikerTo = null;
                if (parsedMonikerFrom.platformTfm != "")
                {
                    monikerTo = new IsNetCore5UpMonikerResult()
                    {
                        targetFramework = parsedMonikerTo.targetFramework,
                        platformTfm = parsedMonikerFrom.platformTfm
                    };
                }
                else
                {
                    monikerTo = parsedMonikerTo;
                }
                to = start + monikerTo.ToString() + end;
            }
            content = content.Replace(from, to);
#if ASYNC
            await
#endif
         TF.WriteAllText(path, content);
        }
    }
    public static void ChangeProjectsTo(string to2, List<string> vs)
    {
        var parsedMonikerTo = IsNetCore5UpMoniker(to2);
        foreach (var item in vs)
        {
            ChangeProjectTo(to2, item, parsedMonikerTo);
        }
    }
}