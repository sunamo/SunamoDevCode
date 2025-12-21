namespace SunamoDevCode.ToNetCore.research;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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
    ChangeConvertNonWebPlatformTargetTo(ILogger logger, string replaceFor)
    {
        var temp = WebAndNonWebProjects(logger);
        var tt = temp.Item2;
        replaceFor = 
#if ASYNC
    await
#endif
        Shared.PlatformTargetTo(replaceFor, tt);
    }

    public static Type type = typeof(MoveToNet5);
    /// <summary>
    /// Vyčistí od dočasných souborů z NonWeb
    /// </summary>
    public void ClearUnnecessaryFromNonWeb(ILogger logger, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        Console.WriteLine("ClearUnnecessaryFromNonWeb");
        var temp = WebAndNonWebSlns();
        Console.WriteLine("temp.Item2.Count: " + temp.Item2.Count);
        foreach (var item in temp.Item2)
        {
            DeleteTemporaryFilesFromSolution.ClearSolution(logger, item, true, folderWithTemporaryMovedContentWithoutBackslash);
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
    ReplaceUnneedUsings(string eVsProjects)
    {
        var linesToCommented = @"//using System.Data;";
        var toComm = SHGetLines.GetLines(linesToCommented);
        List<string> toComm1 = new List<string>(toComm.Count);
        List<string> toComm2 = new List<string>(toComm.Count);
        const string cm = "//";
        foreach (var item in toComm)
        {
            toComm1.Add(cm + item);
            toComm2.Add(cm + cm + item);
        }

        var f2 = Directory.GetFiles(eVsProjects, "*.cs", SearchOption.AllDirectories);
        List<string> dontReplaceUsingSystemDataIn = new List<string>()
        {
            ".web",
            "SunamoSqlServer",
            "SunamoSqlite",
            "SunamoCsv"
        };
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
                    await TF.WriteAllText(item, n);
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
    /// <param name = "csprojPath"></param>
    public async Task ReplaceUnneedReferencesInCsprojs(string csprojPath)
    {
        var refe = SHGetLines.GetLines(refToRemove);
        await ReplaceOrRemoveFile(null, ElementsItemGroup.Reference, refe, csprojPath);
    }

    private 
#if ASYNC
    async Task
#else
    void 
#endif
    ReplaceUnneedReferencesInCsprojs(string dontReplaceReferencesInPath, string eVsProjects)
    {
        List<string> referenceToReplace = new List<string>();
        var refe = SHGetLines.GetLines(refToRemove);
        int dx = -1;
        var f = Directory.GetFiles(eVsProjects, "*.csproj", SearchOption.AllDirectories);
        List<string> dontReplaceReferencesIn = (
#if ASYNC
    await
#endif
        TF.ReadAllLines(dontReplaceReferencesInPath)).ToList();
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
}