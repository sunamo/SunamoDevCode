namespace SunamoDevCode.Aps.Projs;

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
            Content = fileOrContent,
            IsProjectCsprojSdkStyleIsCore =
#if ASYNC
    await
#endif
            IsProjectCsprojSdkStyleIsCore(fileOrContent, true),
            IsNetstandard = netstandard
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

    /// <summary>
    /// Builds a list of project dependency tree paths for the given .csproj file.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file to analyze.</param>
    /// <param name="dictToAvoidCollectionWasChanged">Dictionary to avoid collection modification during iteration.</param>
    /// <returns>List of project dependency paths.</returns>
    public static
#if ASYNC
        async Task<List<string>>
#else
    List<string>
#endif
    BuildProjectsDependencyTreeList(string csprojPath, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged)
    {
        XmlDocumentsCache.CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Clear();
        var result =
#if ASYNC
            await
#endif
        BuildProjectsDependencyTree2(csprojPath, dictToAvoidCollectionWasChanged);
        return result.Collection;
    }

    /// <summary>
    /// Builds a collection of project dependency tree paths for the given .csproj file.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file to analyze.</param>
    /// <param name="dictToAvoidCollectionWasChanged">Dictionary to avoid collection modification during iteration.</param>
    /// <returns>Collection of unique project dependency paths.</returns>
    static
#if ASYNC
        async Task<CollectionWithoutDuplicatesDC<string>>
#else
    CollectionWithoutDuplicates<string>
#endif
    BuildProjectsDependencyTree2(string csprojPath, Dictionary<string, XmlDocument>? dictToAvoidCollectionWasChanged = null)
    {
        CollectionWithoutDuplicatesDC<string> paths = new CollectionWithoutDuplicatesDC<string>();
#if ASYNC
        await
#endif
        BuildProjectsDependencyTree(paths, csprojPath, dictToAvoidCollectionWasChanged);
        return paths;
    }

    /// <summary>
    /// Recursively builds the project dependency tree.
    /// </summary>
    /// <param name="paths">Collection to store discovered project paths.</param>
    /// <param name="csprojPath">Path to the .csproj file to analyze.</param>
    /// <param name="dictToAvoidCollectionWasChanged">Dictionary to avoid collection modification during iteration.</param>
    static
#if ASYNC
        async Task
#else
    void
#endif
    BuildProjectsDependencyTree(CollectionWithoutDuplicatesDC<string> paths, string csprojPath, Dictionary<string, XmlDocument>? dictToAvoidCollectionWasChanged = null)
    {
        if (Ignored.IsIgnored(csprojPath))
        {
            return;
        }

        // This is problematic - in this situation I have only undamaged projects.
        // For example, for E:\vs\Projects\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths\AllProjectsSearch.Cmd.CsprojPaths.csproj
        // it threw an error, but when I ran it again with just this file, it passed
        var result =
#if ASYNC
            await
#endif
        VsProjectsFileHelper.GetProjectReferences(csprojPath, dictToAvoidCollectionWasChanged);
        if (result.Projects == null)
        {
            // debug step by step now
            result =
#if ASYNC
                await
#endif
            VsProjectsFileHelper.GetProjectReferences(csprojPath, null);
            if (result.Projects == null)
            {
                result =
#if ASYNC
                    await
#endif
                VsProjectsFileHelper.GetProjectReferences(csprojPath, dictToAvoidCollectionWasChanged);
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                XmlDocumentsCache.CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull.Add(csprojPath);
            }
        }

        // I don't understand how this is possible, but for some projects on VPS it returns paths as they exist on my machine, so I must check for null
        if (result.Projects != null)
        {
            paths.AddRange(result.Projects);
            foreach (var item in result.Projects)
            {
                if (FS.ExistsFile(item))
                {
                    await BuildProjectsDependencyTree(paths, item, dictToAvoidCollectionWasChanged);
                }
            }
        }
    }

    /// <summary>
    /// Adds missing projects to a solution.
    /// </summary>
    /// <param name="solutionFolder">The solution folder to add projects to.</param>
    /// <param name="isAddingDependencies">Whether to also add project dependencies.</param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
    AddMissingProjects(SolutionFolder solutionFolder, bool isAddingDependencies = false)
    {
        var text = ApsHelper.Instance.MainSln(solutionFolder);
        if (text == null)
        {
            ThisApp.Error($"Sln with name {solutionFolder.NameSolution} doesn't have main sln");
            return;
        }

#if ASYNC
        await
#endif
        AddMissingProjectsAlsoString(text, isAddingDependencies);
    }
}