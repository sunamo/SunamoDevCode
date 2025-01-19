

public partial class MoveToNet5
{
    public static MoveToNet5 ci = new MoveToNet5();

    private MoveToNet5()
    {

    }

    /// <summary>
    /// Pravděpodobně to kurví csproj
    /// A1 can be
    ///
    /// Tohle asi nemělo příliš smysl. Když jsem to změnil celé v jakémkoliv řešení z AnyCPU na x86 tak jsem měl u plno projektů modrou ikonku.
    /// Navíc s tímto nastavením když jsem spustil EveryLine a ač sln bylo nastavené AnyCPu, hned po spuštení že nemůže načíst EveryLine. Když jsem jeho csproj změnil na AnyCPU, chybu začalo hlásit zase u desktop.
    ///
    /// A1 can be x86,x64.AnyCPU
    /// </summary>
    public
#if ASYNC
    async Task
#else
    void
#endif
 ChangeConvertNonWebPlatformTargetTo(string replaceFor)
    {
        var t = WebAndNonWebProjects();
        var tt = t.Item2;

        replaceFor =
#if ASYNC
    await
#endif
 Shared.PlatformTargetTo(replaceFor, tt);
    }

    public static Type type = typeof(MoveToNet5);

    private void RemoveIncludeWhichEndOfCs()
    {
        var t = WebAndNonWebProjects();

        foreach (var item in t.Item2)
        {
            XmlAgilityDocument x = new XmlAgilityDocument();
            x.Load(item);
            var nodes = HtmlAgilityHelper.NodesWithAttrWildCard(x.hd.DocumentNode, true, "Compile", ItemGroupAttrsConsts.Include, "*.cs", true);

            foreach (var item2 in nodes)
            {
                item2.Remove();
            }

            x.Save();
        }
    }

    /// <summary>
    /// Vyčistí od dočasných souborů z NonWeb
    /// </summary>
    public void ClearUnnecessaryFromNonWeb()
    {
        Console.WriteLine("ClearUnnecessaryFromNonWeb");
        var t = WebAndNonWebSlns();

        Console.WriteLine("t.Item2.Count: " + t.Item2.Count);

        foreach (var item in t.Item2)
        {
            DeleteTemporaryFilesFromSolution.ClearSolution(item, true);
        }
    }

    /// <summary>
    /// Zakomentuje importy se kterými nemůže být převedeno na .net 5
    /// </summary>
    private
#if ASYNC
    async Task
#else
    void
#endif
 ReplaceUnneedUsings()
    {
        var linesToCommented = @"//using System.Data;";

        var toComm = SHGetLines.GetLines(linesToCommented);

        List<string> toComm1 = new List<string>(toComm.Count);
        List<string> toComm2 = new List<string>(toComm.Count);

        const string cm = CSharpConsts.lc;

        foreach (var item in toComm)
        {
            toComm1.Add(cm + item);
            toComm2.Add(cm + cm + item);
        }

        var f2 = Directory.GetFiles(@"E:\vs\Projects\", "*.cs", SearchOption.AllDirectories);
        List<string> dontReplaceUsingSystemDataIn = new List<string>() { ".web", "SunamoSqlServer", "SunamoSqlite", "SunamoCsv" };
        foreach (var item in f2)
        {
            if (!CA.ContainsAnyFromElementBool(item, dontReplaceUsingSystemDataIn))
            {
                var tf =
#if ASYNC
    await
#endif
 TF.ReadAllText(item);
                string n = tf;

                for (int i = 0; i < toComm.Count; i++)
                {
                    n = n.Replace(toComm[i], toComm1[i]);
                }

                for (int i = 0; i < toComm.Count; i++)
                {
                    n = n.Replace(toComm2[i], toComm1[i]);
                }

                if (n != tf)
                {
                    TF.WriteAllText(item, n);
                }
            }
        }
    }

    // 25-9-2022 Protože mi opět něco smazalo assembly se *.sunamo.cz csproj - .Web.Services, Data, .Web => commented, dole náhrada.
    //     const string refToRemove = @"Microsoft.CSharp
    // System.ComponentModel.Composition
    // System.Core
    // System.Data
    // System.Data.DataSetExtensions
    // System.Deployment
    // System.Design
    // System.Net.Http
    // System.Net.Http.WebRequest
    // System.Web
    // System.Web.Extensions
    // System.Web.ApplicationServices
    // System.Web.DynamicData
    // System.Web.Entity
    // System.Web.Services
    // System.Windows
    // System.Windows.Presentation
    // System.Xml
    // System.Xml.Ling
    // sunamoPortable
    // swf
    // System.Device";
    const string refToRemove = @"sunamoPortable
swf";


    string nugetPackagesCantRemove = @"System.Net.Http.Extensions
Microsoft.Threading.Tasks
Microsoft.Threading.Tasks.Extensions
Microsoft.Threading.Tasks.Extensions.Desktop
System.Globalization.Extensions
System.IO.FileSystem.Primitives
System.IO.FileSystem
System.Management.Automation
System.Net.Http.Primitives
";

    /// <summary>
    /// odstraní reference z c# které
    /// </summary>
    /// <param name="csprojPath"></param>
    public void ReplaceUnneedReferencesInCsprojs(string csprojPath)
    {
        var refe = SHGetLines.GetLines(refToRemove);
        ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, refe, csprojPath);
    }

    private
#if ASYNC
    async Task
#else
    void
#endif
 ReplaceUnneedReferencesInCsprojs()
    {
        List<string> referenceToReplace = new List<string>();

        var refe = SHGetLines.GetLines(refToRemove);
        int dx = -1;

        var f = Directory.GetFiles(@"E:\vs\Projects\", "*.csproj", SearchOption.AllDirectories);
        List<string> dontReplaceReferencesIn = (
#if ASYNC
    await
#endif
 TF.ReadAllLines(@"D:\Documents\sunamo\AllProjectsSearch\Settings\dontReplaceReferencesIn.txt")).ToList();
        foreach (var item in f)
        {
            if (!CA.ContainsAnyFromElementBool(item, dontReplaceReferencesIn))
            {

#if ASYNC
                await
#endif
                ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, refe, refe2, item);
            }
        }
    }

    string refe2 = "</Reference>";



    private
#if ASYNC
    async Task
#else
    void
#endif
 ReplaceOrRemoveFile(Func<string, string> addReplace, string element, List<string> refe, string pathCsproj, string newRefe = null)
    {
        bool replace = newRefe != null;

        refe2 = "</" + element + ">";

#if DEBUG
        if (pathCsproj.EndsWith(@"sunamo.web.csproj"))
        {
            ThisApp.check = true;
        }
#endif

        var xd =
#if ASYNC
    await
#endif
 XmlDocumentsCache.Get(pathCsproj);

        if (MayExcHelper.MayExc(xd))
        {
            return;
        }

        var tf = xd.Data.OuterXml;
        string n = tf;
        int dx = -1;
        int dx2 = -1;
        bool combine = false;

        foreach (var r in refe)
        {
            combine = false;
            dx = -1;
            dx2 = -1;

            if (replace)
            {
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\" />", string.Empty, ref dx);
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\"/>", string.Empty, ref dx);
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\"></" + element + ">", string.Empty, ref dx);
            }
            else
            {
                n = n.Replace("<" + element + " Include=\"" + r + "\" />", string.Empty);
                n = n.Replace(GetReferenceShortest(element, r), string.Empty);
                n = n.Replace("<" + element + " Include=\"" + r + "\"></" + element + ">", string.Empty);
            }

            if (dx == -1)
            {
                string toFind = ReferenceLongest(element, r);
                dx = n.IndexOf(toFind);
                combine = true;
            }

            if (dx != -1 && combine)
            {
                n = n.Remove(dx, ReferenceLongest(element, r).Length);

                dx2 = n.IndexOf(refe2, dx);
                n = n.Remove(dx2, refe2.Length);
            }

            if (dx != -1)
            {
                if (replace)
                {
                    var refe3 = GetReferenceShortest(element, newRefe);
                    n = n.Insert(dx, refe3);
                }
            }
        }

        if (addReplace != null)
        {
            n = addReplace(n);
        }

        if (n != tf)
        {
#if DEBUG
            var l =
#if ASYNC
    await
#endif
 TF.ReadAllLines(pathCsproj);
            var nf = FS.InsertBetweenFileNameAndExtension(pathCsproj, "_b");

#if ASYNC
            await
#endif
            TF.WriteAllLines(nf, l);
#endif

            XmlDocumentsCache.Set(pathCsproj, n);


#if ASYNC
            await
#endif
            TF.WriteAllText(pathCsproj, n);
        }

        ThisApp.check = false;
    }

    private static string ReferenceLongest(string element, string r)
    {
        return "<" + element + " Include=\"" + r + "\">";
    }

    private static string GetReferenceShortest(string element, string r)
    {
        return "<" + element + " Include=\"" + r + "\"/>";
    }

    /// <summary>
    /// Don't run, wait whether will be really needed to run
    /// it could do more harm than good
    /// </summary>
    private
#if ASYNC
    async Task
#else
    void
#endif
 CommentAssemblyInfoCsFiles()
    {
        var d = WebAndNonWebProjects();
        var lc = CSharpConsts.lc;

        foreach (var item in d.Item2)
        {
            var d2 = FS.GetDirectoryName(item);
            var ass = FSGetFiles.GetFiles(d2, "AssemblyInfo.cs", true);
            foreach (var item2 in ass)
            {
                var l =
#if ASYNC
    await
#endif
 TF.ReadAllLines(item2);
                if (!l.All(r => r.StartsWith(lc)))
                {
                    CA.StartingWith(lc, l);
                    TF.WriteAllLines(item2, l);
                }
            }
        }
    }
}
