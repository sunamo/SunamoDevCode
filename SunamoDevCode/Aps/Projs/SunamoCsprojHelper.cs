namespace SunamoDevCode.Aps.Projs;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class SunamoCsprojHelper
{
    /// <summary>
    /// public should be only DetectNetVersion
    /// </summary>
    /// <param name = "fileOrContent"></param>
    /// <param name = "netstandard"></param>
    /// <returns></returns>
    /// <summary>
    /// public should be only DetectNetVersion
    /// mmust have postfix 2 because IsProjectCsprojSdkStyleIsCore have also same arg method
    /// </summary>
    /// <param name = "fileOrContent"></param>
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
            if (!fileOrContent.StartsWith("<") && FS.ExistsFile(fileOrContent))
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
        if (!fileOrContent.StartsWith("<") && FS.ExistsFile(fileOrContent))
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
    /// <param name = "csprojContent"></param>
    /// <param name = "path"></param>
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
            var list = SHGetLines.GetLines(csprojContent);
            // žádný https://www.nuget.org/packages?q=Sunamo+Metaproject není, proto není ani TargetFramework a ctor text 2Mi args
            var csp = CsprojFileParser.ParseCsproj( /*list,*/path);
            return new Tuple<bool, string>(isNew, /*csp.TargetFramework*/ null);
        }

        var value = FrameworkNameDetector.Detect(path);
        return new Tuple<bool, string>(isNew, VersionHelper.RemovePartsWhichIsZero(value.Version));
    }

    public static 
#if ASYNC
    async Task<SupportedNetFw>
#else
    SupportedNetFw 
#endif
    DetectNetVersion2(string path)
    {
        var temp = 
#if ASYNC
            await
#endif
        DetectNetVersion(path);
        if (temp != null)
        {
            if (temp.Item1)
            {
                var text = SHParts.RemoveAfterFirst(temp.Item2, '-');
                text = text.Replace(".", String.Empty);
                return EnumHelper.Parse(text, SupportedNetFw.None);
            }
            else
            {
                if (temp.Item2 == "4.8")
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
        CollectionWithoutDuplicatesDC<string> path = new CollectionWithoutDuplicatesDC<string>();
#if ASYNC
        await
#endif
        BuildProjectsDependencyTree(path, csproj, dictToAvoidCollectionWasChanged);
        return path;
    }

    static 
#if ASYNC
        async Task
#else
    void 
#endif
    BuildProjectsDependencyTree(CollectionWithoutDuplicatesDC<string> path, string csproj, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged = null)
    {
        //E:\vs\Projects\PhotosSczClientCmd\PhotosSczClientCmd\PhotosSczClientCmd.csproj
        if (Ignored.IsIgnored(csproj))
        {
            return;
        }

        // Tohle je nějaké jeblé. value této situaci mám už jen nepoškozené projekty.
        // Třeba pro E:\vs\Projects\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths.csproj
        // mi to vyhodilo chybu ale když jsem pustil znovu jen text tímto souborem, prošlo to
        var result = 
#if ASYNC
            await
#endif
        VsProjectsFileHelper.GetProjectReferences(csproj, dictToAvoidCollectionWasChanged);
        if (result.projs == null)
        {
            // teď to laď krok za krokem
            result = 
#if ASYNC
                await
#endif
            VsProjectsFileHelper.GetProjectReferences(csproj, null);
            if (result.projs == null)
            {
                result = 
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
        if (result.projs != null)
        {
            path.AddRange(result.projs);
            foreach (var item in result.projs)
            {
                if (FS.ExistsFile(item))
                {
                    await BuildProjectsDependencyTree(path, item, dictToAvoidCollectionWasChanged);
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
        var text = ApsHelper.ci.MainSln(sln);
        if (text == null)
        {
            ThisApp.Error($"Sln with name {sln.nameSolution} doesn't have main sln");
            return;
        }

#if ASYNC
        await
#endif
        AddMissingProjectsAlsoString(text, addAlsoDepencies);
    }
}