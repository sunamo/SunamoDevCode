namespace SunamoDevCode.ToNetCore.research;

public partial class MoveToNet5
{
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
 PlatformTargetToWeb(ILogger logger, string replaceFor)
    {
        var t = WebAndNonWebProjects(logger);
        var tt = t.Item1;
        replaceFor =
#if ASYNC
    await
#endif
 Shared.PlatformTargetTo(replaceFor, tt);
    }
    // 25-9-2022 Protože mi opět něco smazalo assembly se *.sunamo.cz csproj - .Web.Services, Data, .Web => commented, dole náhrada.
    //     const string neededWebReferences = @"System.Web
    // System.Web.Services
    // System.Data";
    const string neededWebReferences = "";
    public
#if ASYNC
    async Task
#else
    void
#endif
 AddEssentialWebReferencesToAllWebProjects(ILogger logger)
    {
        var l = SHGetLines.GetLines(neededWebReferences);
        var t =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(logger, false);
        StringBuilder sb = new StringBuilder();
        if (t.netstandardList.Count > 0)
        {
            sb.AppendLine("Web projects which is in web standard");
            foreach (var item in t.netstandardList)
            {
                sb.AppendLine(item);
            }
        }
        var l2 = SHGetLines.GetLines(neededWebReferences);
        //CA.PostfixIfNotEnding(".dll", l2);
        foreach (var item in t.csprojSdkStyleList)
        {
            foreach (var item2 in l2)
            {
                var rig = new ReferenceItemGroup(item2, item, null);
                // Toto tu muselo být zřejmě kvůli užívání AddItemGroupNoSdkStyle. Teď mi to dělá problémy protože .dll tam nepatří
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }
        // 1 = sdk style, not netstandard2.0
        // 2 = sdk style, netstandard2.0
        // 3 = non sdk style
        foreach (var item in t.nonCsprojSdkStyleList)
        {
            foreach (var item2 in l2)
            {
#if DEBUG
                if (item.Contains("dart.sunamo.cz") && item2.Contains("System.Web"))
                {
                }
#endif
                var rig = new ReferenceItemGroup(item2, item, null);
                await VsProjectsFileHelper.AddItemGroupSdkStyle(item, ItemGroups.Reference, rig, true);
            }
        }
    }
    public
#if ASYNC
    async Task
#else
    void
#endif
 ChangeProjectsToNetStandard(ILogger logger)
    {
        var l =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(logger, false);
        await ChangeProjects.ChangeProjectsTo(ChangeProjects.netstandard20, l.csprojSdkStyleList);
    }
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 WebProjectsWhichIsNotSdkStyleFindTheirBackup(ILogger logger)
    {
        List<string> haveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackupSdkStyle = new List<string>();
        List<string> dontHaveBackup = new List<string>();
        var l =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(logger, false);
        foreach (var item in l.csprojSdkStyleList)
        {
            throw new Exception("žádné koncovky old v csproj tu nemám. tak tedy nevím co jsem tu chtěl dělat ");
            //var old = item + ".old";
            //if (FS.ExistsFile(old))
            //{
            //    if (SunamoCsprojHelper.IsProjectCsprojSdkStyleIsCore(old, false))
            //    {
            //        haveBackupSdkStyle.Add(item);
            //    }
            //    else
            //    {
            //        dontHaveBackupSdkStyle.Add(item);
            //    }
            //}
            //else
            //{
            //    dontHaveBackup.Add(item);
            //}
        }
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(haveBackupSdkStyle, nameof(haveBackupSdkStyle));
        tog.List(dontHaveBackupSdkStyle, nameof(dontHaveBackupSdkStyle));
        tog.List(dontHaveBackup, nameof(dontHaveBackup));
        return tog.ToString();
    }
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 DetectFrameworkForWebProjectsOnlySupported(ILogger logger)
    {
        TextOutputGenerator tog = new TextOutputGenerator();
        Dictionary<SupportedNetFw, StringBuilder> sb = new Dictionary<SupportedNetFw, StringBuilder>();
        var t = WebAndNonWebProjects(logger);
        foreach (var item in t.Item1)
        {
            var n =
#if ASYNC
                await
#endif
                SunamoCsprojHelper.DetectNetVersion2(item);
            // Inlined from DictionaryHelper.AppendLineOrCreate - přidává řádek do StringBuilderu nebo vytváří nový
            if (sb.ContainsKey(n))
            {
                sb[n].AppendLine(item);
            }
            else
            {
                var sb2 = new StringBuilder();
                sb2.AppendLine(item);
                sb.Add(n, sb2);
            }
        }
        foreach (var item in sb)
        {
            tog.ListSB(item.Value, item.Key.ToString());
        }
        //Output = tog.ToString();
        //OutputOpen();
        return tog.ToString();
    }
    public async Task ConvertAlLWebNetStandardProjectsToNet48(ILogger logger)
    {
        var t = WebAndNonWebProjects(logger);
        foreach (var item in t.Item1)
        {
            await ChangeProjects.ChangeProjectTo(ChangeProjects.net48, item, null, ChangeProjects.netstandard20);
        }
    }
    public string WebProjectsWhichNotEndWithDotEnd(ILogger logger)
    {
        var t = WebAndNonWebProjects(logger, true);
        StringBuilder sb = new StringBuilder();
        foreach (var item in t.Item1)
        {
            // Vše zde musí být bez koncového lomítka abych podchytil i .Tests postfix
            if (!item.EndsWith(".web.csproj") /*&& !item.Contains(".web64") && !item.Contains(".web5") && !item.Contains(@"\sunamo.cz") && !item.Contains(@"\sunamo.cz-old") && !item.Contains(@"\sunamo.cz64") && !item.Contains(@"\sunamo.web")*/)
            {
                sb.AppendLine(item);
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// 1 = sdk style, not netstandard2.0
    /// 2 = sdk style, netstandard2.0
    /// </summary>
    /// <param name="appendHeader"></param>
    /// <returns></returns>
    public
#if ASYNC
    async Task<Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>>
#else
      Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>
#endif
 DetectFrameworkForWebProjects(ILogger logger, bool appendHeader)
    {
        List<TWithStringDC<string>> l = new List<TWithStringDC<string>>();
        List<TWithStringDC<string>> l2 = new List<TWithStringDC<string>>();
        if (appendHeader)
        {
            l.Add(new TWithStringDC<string>("", "Web but in SDK style:"));
        }
        bool netstandard = false;
        var t = WebAndNonWebProjects(logger);
        Tuple<bool, string> t3 = null;
        foreach (var item2 in t.Item1)
        {
            t3 =
#if ASYNC
    await
#endif
 SunamoCsprojHelper.DetectNetVersion(item2);
            if (t3 != null)
            {
                if (t3.Item1)
                {
                    if (netstandard)
                    {
                        l2.Add(new TWithStringDC<string>(item2, t3.Item2));
                    }
                    else
                    {
                        l.Add(new TWithStringDC<string>(item2, t3.Item2));
                    }
                }
            }
        }
        return new Tuple<List<TWithStringDC<string>>, List<TWithStringDC<string>>>(l, l2);
    }
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 FindProjectsWhichIsSdkStyleList(ILogger logger, bool appendHeaderForWeb, bool web = true)
    {
        var r =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(logger, appendHeaderForWeb, web);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(r.csprojSdkStyleList, nameof(r.csprojSdkStyleList));
        tog.List(r.netstandardList, nameof(r.netstandardList));
        tog.List(r.nonCsprojSdkStyleList, nameof(r.nonCsprojSdkStyleList));
        //ProgramShared.Output = tog.ToString();
        //ProgramShared.OutputOpen();
        return tog.ToString();
    }
    /// <summary>
    /// 1 = sdk style, not netstandard2.0
    /// 2 = sdk style, netstandard2.0
    /// 3 = non sdk style
    /// </summary>
    /// <param name="appendHeaderForWeb"></param>
    /// <returns></returns>
    public
#if ASYNC
    async Task<FindProjectsWhichIsSdkStyleResult>
#else
      FindProjectsWhichIsSdkStyleResult
#endif
 FindProjectsWhichIsSdkStyle(ILogger logger, bool appendHeaderForWeb, bool web = true)
    {
        List<string> csprojSdkStyleList = new List<string>();
        List<string> netstandardList = new List<string>();
        List<string> nonCsprojSdkStyleList = new List<string>();
        if (appendHeaderForWeb)
        {
            csprojSdkStyleList.Add("Web but in SDK style:");
        }
        var t = WebAndNonWebProjects(logger);
        List<string> ls = null;
        if (web)
        {
            ls = t.Item1;
        }
        else
        {
            ls = t.Item2;
        }
        foreach (var item2 in ls)
        {
            if (Ignored.IsIgnored(item2))
            {
                continue;
            }
            var item = item2;
            var tu =
#if ASYNC
    await
#endif
 SunamoCsprojHelper.IsProjectCsprojSdkStyleIsCore(/*ref*/ item);
            if (tu.isProjectCsprojSdkStyleIsCore)
            {
                if (tu.isNetstandard)
                {
                    netstandardList.Add(item);
                }
                else
                {
                    csprojSdkStyleList.Add(item);
                }
            }
            else
            {
                nonCsprojSdkStyleList.Add(item);
            }
        }
        return new FindProjectsWhichIsSdkStyleResult
        {
            csprojSdkStyleList = csprojSdkStyleList,
            netstandardList = netstandardList,
            nonCsprojSdkStyleList = nonCsprojSdkStyleList
        };
    }
    public
#if ASYNC
    async Task<string>
#else
    void
#endif
 WebProjectsWhichIsNotSdkStyle(ILogger logger)
    {
        var u =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(logger, true);
        return SHJoin.JoinNL(u.nonCsprojSdkStyleList);
    }
    string nameProject = null;
    public async void ReplaceProjectReferenceForWeb(ILogger logger, string n, string ns)
    {
        Console.WriteLine("Solution old & new must be in same root folder");
        n = SHTrim.TrimEnd(n, ".web");
        ns = SHTrim.TrimEnd(ns, ".web");
        nameProject = n;
        string old = @"..\..\" + ns + @"\" + n + @"\" + n + ".csproj";
        string nuova = @"..\..\" + SolutionNameFor(ns) + @"\" + n + @".web\" + n + ".web.csproj";
        var t = WebAndNonWebProjects(logger);
        foreach (var item in t.Item1)
        {
            Console.WriteLine(item);
            //DebugLogger.Instance.WriteLine(item);
            //var dx =
            await ReplaceOrRemoveFile(WithWebEnd, ElementsItemGroup.ProjectReference, CAG.ToList<string>(old), item, nuova);
            //if (dx != -1)
            //{
            //    //break;
            //}
        }
    }
    string WithWebEnd(string s)
    {
        return s.Replace("<Name>" + nameProject + "</Name>", "<Name>" + nameProject + ".web</Name>");
    }
    private string SolutionNameFor(string n)
    {
        if (n == "PlatformIndependentNuGetPackages")
        {
            return "sunamo.webWithoutDep";
        }
        return n + ".web";
    }
}