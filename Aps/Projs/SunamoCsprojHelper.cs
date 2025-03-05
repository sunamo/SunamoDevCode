namespace SunamoDevCode.Aps.Projs;

public class SunamoCsprojHelper
{
    /// <summary>
    /// public should be only DetectNetVersion
    /// </summary>
    /// <param name="fileOrContent"></param>
    /// <param name="netstandard"></param>
    /// <returns></returns>
    /// <summary>
    /// public should be only DetectNetVersion
    /// mmust have postfix 2 because IsProjectCsprojSdkStyleIsCore have also same arg method
    /// </summary>
    /// <param name="fileOrContent"></param>
    /// <returns></returns>
    private static
#if ASYNC
    async Task<bool>
#else
    bool
#endif
 IsProjectCsprojSdkStyleIsCore(string fileOrContent, bool onlyContentIsPassed)
    {
        if (!onlyContentIsPassed)
        {
            if (!fileOrContent.StartsWith("<") &&
 FS.ExistsFile(fileOrContent))
            {
                fileOrContent =
#if ASYNC
    await
#endif
 TF.ReadAllText(fileOrContent);
            }
        }
        // bez ukončení závorkama protože může být i <Project Sdk="Microsoft.NET.Sdk.WindowsDesktop"> atd.
        return fileOrContent.Contains("Sdk=\"Microsoft.NET.Sdk");
    }
    public static
#if ASYNC
    async Task<IsProjectCsprojSdkStyleResult>
#else
    IsProjectCsprojSdkStyleResult
#endif
         IsProjectCsprojSdkStyleIsCore(string fileOrContent)
    {
        bool netstandard = false;
#if DEBUG
        if (fileOrContent.Contains("duom.web.csproj"))
        {
        }
#endif
        if (!fileOrContent.StartsWith("<") &&
 FS.ExistsFile(fileOrContent))
        {
            var xd =
#if ASYNC
            await
#endif
            XmlDocumentsCache.Get(fileOrContent);
            if (xd.Data == null)
            {
                return null;
            }
            fileOrContent = xd.Data.OuterXml;
        }
        if (fileOrContent.Contains("<TargetFramework>netstandard2.0</TargetFramework>"))
        {
            netstandard = true;
        }
        return new IsProjectCsprojSdkStyleResult
        {
            content = fileOrContent,
            isProjectCsprojSdkStyleIsCore =
#if ASYNC
    await
#endif
 IsProjectCsprojSdkStyleIsCore(fileOrContent, true),
            isNetstandard = netstandard
        };
    }
    /// <summary>
    /// Whether is old .net fw, version
    /// </summary>
    /// <param name="csprojContent"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static
#if ASYNC
    async Task<Tuple<bool, string>>
#else
    Tuple<bool, string>
#endif
         DetectNetVersion(string path)
    {
        var xml =
#if ASYNC
        await
#endif
        XmlDocumentsCache.Get(path);
        if (MayExcHelper.MayExc(xml.Exc))
        {
            return null;
        }
        if (xml.Data == null)
        {
            return null;
        }
        var csprojContent = xml.Data.OuterXml;
        var isNew =
#if ASYNC
    await
#endif
 IsProjectCsprojSdkStyleIsCore(csprojContent, true);
        if (isNew)
        {
            //// Probably will be first because "You need to reference the Microsoft.Build.Engine assembly"
            //Microsoft.Build.Evaluation.Project project = new Microsoft.Build.Evaluation.Project();
            ////Microsoft.CodeAnalysis.Project project = new Microsoft.CodeAnalysis.Project();
            ////Project project = new Project();
            //project.Load(fullPathName);
            //var embeddedResources =
            //    from grp in project.ItemGroups.Cast<BuildItemGroup>()
            //    from item in grp.Cast<BuildItem>()
            //    where item.Name == "EmbeddedResource"
            //    select item;
            var l = SHGetLines.GetLines(csprojContent);
            // žádný https://www.nuget.org/packages?q=Sunamo+Metaproject není, proto není ani TargetFramework a ctor s 2Mi args
            var csp = CsprojFileParser.ParseCsproj(/*l,*/ path);
            return new Tuple<bool, string>(isNew, /*csp.TargetFramework*/ null);
        }
        var v =
            FrameworkNameDetector.Detect(path);
        return new Tuple<bool, string>(isNew, VersionHelper.RemovePartsWhichIsZero(v.Version));
    }
    public static
#if ASYNC
    async Task<SupportedNetFw>
#else
    SupportedNetFw
#endif
         DetectNetVersion2(string path)
    {
        var t =
#if ASYNC
            await
#endif
            DetectNetVersion(path);
        if (t != null)
        {
            if (t.Item1)
            {
                var s = SHParts.RemoveAfterFirst(t.Item2, '-');
                s = s.Replace(".", String.Empty);
                return EnumHelper.Parse(s, SupportedNetFw.None);
            }
            else
            {
                if (t.Item2 == "4.8")
                {
                    return SupportedNetFw.net48;
                }
                return SupportedNetFw.None;
            }
        }
        return SupportedNetFw.BadXml;
    }
    public static
#if ASYNC
        async Task<List<string>>
#else
        List<string>
#endif
        BuildProjectsDependencyTreeList(string csproj, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged)
    {
        XmlDocumentsCache.cantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Clear();
        var result =
#if ASYNC
            await
#endif
            BuildProjectsDependencyTree2(csproj, dictToAvoidCollectionWasChanged);
        return result.c;
    }
    static
#if ASYNC
        async Task<CollectionWithoutDuplicatesDC<string>>
#else
        CollectionWithoutDuplicates<string>
#endif
        BuildProjectsDependencyTree2(string csproj, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged = null)
    {
        CollectionWithoutDuplicatesDC<string> p = new CollectionWithoutDuplicatesDC<string>();
#if ASYNC
        await
#endif
        BuildProjectsDependencyTree(p, csproj, dictToAvoidCollectionWasChanged);
        return p;
    }
    static
#if ASYNC
        async Task
#else
        void
#endif
        BuildProjectsDependencyTree(CollectionWithoutDuplicatesDC<string> p, string csproj, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged = null)
    {
        //E:\vs\Projects\PhotosSczClientCmd\PhotosSczClientCmd\PhotosSczClientCmd.csproj
        if (Ignored.IsIgnored(csproj))
        {
            return;
        }
        // Tohle je nějaké jeblé. V této situaci mám už jen nepoškozené projekty.
        // Třeba pro E:\vs\Projects\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths.csproj
        // mi to vyhodilo chybu ale když jsem pustil znovu jen s tímto souborem, prošlo to
        var r =
#if ASYNC
            await
#endif
            VsProjectsFileHelper.GetProjectReferences(csproj, dictToAvoidCollectionWasChanged);
        if (r.projs == null)
        {
            // teď to laď krok za krokem
            r =
#if ASYNC
                await
#endif
                VsProjectsFileHelper.GetProjectReferences(csproj, null);
            if (r.projs == null)
            {
                r =
#if ASYNC
                    await
#endif
                    VsProjectsFileHelper.GetProjectReferences(csproj, dictToAvoidCollectionWasChanged);
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                XmlDocumentsCache.cantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Add(csproj);
            }
        }
        // Nechápu jak je to možné ale pro někteér projekty na vps mi to vrací cesty jak existují na mb. proto musím kontrolovat na null
        if (r.projs != null)
        {
            p.AddRange(r.projs);
            foreach (var item in r.projs)
            {
                if (
 FS.ExistsFile(item))
                {
                    await BuildProjectsDependencyTree(p, item, dictToAvoidCollectionWasChanged);
                }
            }
        }
    }
    public static
#if ASYNC
        async Task
#else
        void
#endif
        AddMissingProjects(SolutionFolder sln, bool addAlsoDepencies = false)
    {
        var s = ApsHelper.ci.MainSln(sln);
        if (s == null)
        {
            ThisApp.Error($"Sln with name {sln.nameSolution} doesn't have main sln");
            return;
        }
#if ASYNC
        await
#endif
        AddMissingProjectsAlsoString(s, addAlsoDepencies);
    }
    public static Type type = typeof(SunamoCsprojHelper);
    /// <summary>
    /// Must be async
    /// </summary>
    /// <param name="s"></param>
    /// <param name="addAlsoDepencies"></param>
    /// <returns></returns>
    public static
#if ASYNC
        async Task<object>
#else
        object
#endif
        AddMissingProjectsAlsoString(object s, bool addAlsoDepencies = false)
    {
        throw new Exception("Používá se tu fubucsproj. přepsat do dotnet cmd");
        //        string ts2 = null;
        //        var sfs = new SolutionFolderSerialize();
        //        sfs.slnFullPath = ts2;
        //        SolutionFolder sln = null;
        //        var ts = s.GetType();
        //        if (ts == Types.tString)
        //        {
        //            ts2 = s.ToString();
        //            if (ts2.EndsWith(AllExtensions.sln))
        //            {
        //                ts2 = FS.GetDirectoryName(ts2);
        //            }
        //            sln = FoldersWithSolutions.CreateSolutionFolder(ts2, SellingUCAps.toSelling);
        //        }
        //        else if (ts == SolutionFolder.type)
        //        {
        //            sln = (SolutionFolder)s;
        //            ts2 = ApsHelper.ci.MainSln(sln);
        //        }
        //        else if (ts == SolutionFolderSerialize.type)
        //        {
        //            sln = new SolutionFolder((SolutionFolderSerialize)s);
        //            ts2 = ApsHelper.ci.MainSln(sln);
        //        }
        //        else
        //        {
        //            ThrowEx.NotImplementedCase(ts);
        //        }
        //        FubuCsprojFile.Solution solution =
        //#if ASYNC
        //    await
        //#endif
        // FubuCsprojFile.Solution.LoadFrom(s.ToString());
        //        sln.projectsInSolution = SolutionsIndexerHelper.ProjectsInSolution(true, sln.fullPathFolder);
        //        var pr = sln.projectsInSolution;
        //        CollectionWithoutDuplicates<string> projectsOnWhichDepend = new CollectionWithoutDuplicates<string>();
        //        foreach (var prr in pr)
        //        {
        //            var p = Path.Combine(sln.fullPathFolder, prr);
        //            var d2 = ApsHelper.ci.GetCsprojsOnlyTopDirectory(p);
        //            foreach (var item2 in d2)
        //            {
        //                FubuCsprojFile.CsprojFile csp = new CsprojFile(item2);
        //                if (!csp.IsValidXml)
        //                {
        //                    continue;
        //                }
        //                if (addAlsoDepencies)
        //                {
        //#if ASYNC
        //                    await
        //#endif
        //                    SunamoCsprojHelper.BuildProjectsDependencyTree(projectsOnWhichDepend, item2);
        //                }
        //#if DEBUG
        //                //if (item2.Contains("SunamoCef"))
        //                //{
        //                //}
        //#endif
        //                solution.AddProject(csp);
        //            }
        //        }
        //        var allGlobalCsprojs =
        //#if ASYNC
        //            await
        //#endif
        //            FoldersWithSolutions.AllGlobalCsprojs();
        //        allGlobalCsprojs = FoldersWithSolutions.allCsprojGlobal;
        //        foreach (var item2 in projectsOnWhichDepend.c)
        //        {
        //            string item = item2;
        //            if (!
        // FS.ExistsFile(item))
        //            {
        //                var fnwoe = Path.GetFileNameWithoutExtension(item);
        //                if (allGlobalCsprojs.ContainsKey(fnwoe))
        //                {
        //                    item = allGlobalCsprojs[fnwoe].First();
        //                }
        //            }
        //            if (FS.ExistsFile(item))
        //            {
        //                //var f = solution.Projects.FirstOrDefault(d => d.ProjectName.Contains("Scz.import"));
        //                var csp = new CsprojFile(item);
        //                if (!csp.IsValidXml)
        //                {
        //                    continue;
        //                }
        //                solution.AddProject(csp);
        //            }
        //        }
        //        solution.Save();
        //        // just for mark as finised for PB after done item (void can't be passed as finished object)
        //        return solution;
    }
}