// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;
public partial class FoldersWithSolutions
{
    /// <summary>
    /// 3-10-23 bylo static ale nevím proč když solutions je instanční
    /// </summary>
    public List<SolutionFolder> solutions = null;
    /// <summary>
    /// D:\Documents
    /// </summary>
    public string documentsFolder = null;
    static Type type = typeof(FoldersWithSolutions);
    static FoldersWithSolutions _fws = null;
    public static RepositoryLocal usedRepository = RepositoryLocal.Vs17;
    protected static void IdentifyProjectType(string documentsFolder, string solutionFolder, SolutionFolder sf)
    {
        // SolutionFolderSerialize doesn't have InVsFolder or typeProjectFolder
        sf.InVsFolder = solutionFolder.Contains(SolutionsIndexerStrings.VisualStudio2017);
        if (sf.InVsFolder)
        {
            solutionFolder = SHTrim.TrimStart(solutionFolder, documentsFolder);
            var parameter = SHSplit.SplitChar(solutionFolder, '\\');
            //var dx = parameter.IndexOf(SolutionsIndexerStrings.VisualStudio2017);
            var pr = parameter[0];
            pr = pr.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
            if (projectTypes._d2.ContainsKey(pr))
            {
                sf.typeProjectFolder = projectTypes._d2[pr];
            }
            else
            {
                //ThrowEx.KeyNotFound(projectTypes._d2, "projectTypes._d2", pr);
                sf.typeProjectFolder = ProjectsTypes.Unknown;
            }
        }
    }

    /// <summary>
    /// Složka ve které se má hledat na složku Projects a složky Visual Studia
    ///
    /// přidává se mi zde když volám ctor FoldersWithSolutions
    ///
    /// pokud nemám sln, zavolat new FoldersWithSolutions(BasePathsHelper.vs, null);
    /// </summary>
    public static FoldersWithSolutionsList fwss = new FoldersWithSolutionsList();
    public static void InsertIntoFwss(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        new FoldersWithSolutions(logger, documentsFolder, toSelling, addAlsoSolutions);
    }

    /// <summary>
    /// A1 = D:\Documents
    /// This class should be instaniate only once and then call reload by needs
    /// A1 toSelling can be null
    /// </summary>
    public FoldersWithSolutions(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        // documentsFolder může být null / SE, stejně to poté doplňuji z actualPlatform
        ThrowEx.DirectoryExists(documentsFolder);
        this.documentsFolder = documentsFolder;
        if (addAlsoSolutions)
        {
            solutions = Reload(logger, documentsFolder, toSelling);
        //if (fwss.Count == 0)
        //{
        //    fwss.AddRange(new FoldersWithSolutions() );
        //}
        }

        fwss.Add(this);
    //    FoldersWithSolutions.fwss.Add(new FoldersWithSolutions());
    // Nemůžu použít, FoldersWithSolutions není odvozené od FoldersWithSolutions
    //fwss.Add(new FoldersWithSolutions(this));
    }

    public FoldersWithSolutions(FoldersWithSolutions fws2)
    {
        solutions = fws2.solutions;
        documentsFolder = fws2.documentsFolder;
        fwss = FoldersWithSolutions.fwss;
        onlyRealLoadedSolutionsFolders = FoldersWithSolutions.onlyRealLoadedSolutionsFolders;
    }

    public List<SolutionFolderWithFiles> SolutionsWithFiles()
    {
        List<SolutionFolderWithFiles> vr = new List<SolutionFolderWithFiles>();
        foreach (var item in solutions)
        {
            vr.Add(new SolutionFolderWithFiles(item));
        }

        return vr;
    }

    /// <summary>
    /// Get all projects in A1(Visual Studio Projects *) and GitHub folder and insert to global variable solutions
    /// A1 toSelling must be null
    /// </summary>
    /// <param name = "documentsFolder"></param>
    public List<SolutionFolder> Reload(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling /*, bool ignorePartAfterUnderscore = false*/)
    {
        PairProjectFolderWithEnum(logger, documentsFolder);
        // Get all projects in A1(Visual Studio Projects *) and GitHub folder
        List<string> solutionFolders = ReturnAllProjectFolders(documentsFolder /*, Path.Combine(documentsFolder, SolutionsIndexerStrings.GitHubMy)*/);
        List<string> projOnlyNames = new List<string>(solutionFolders.Count);
        var ta = solutionFolders.ToArray();
        var on = FS.OnlyNamesNoDirectEdit(ta);
        projOnlyNames.AddRange(on);
        // Initialize global variable solutions
        solutions = new List<SolutionFolder>(solutionFolders.Count);
        for (int i = 0; i < solutionFolders.Count; i++)
        {
            var solutionFolder = solutionFolders[i];
            SolutionFolder sf = CreateSolutionFolder(logger, documentsFolder, solutionFolder, toSelling, projOnlyNames[i]);
            solutions.Add(sf);
        }

        return solutions;
    }

    static TwoWayDictionary<ProjectsTypes, string> projectTypes = new TwoWayDictionary<ProjectsTypes, string>();
    public static void PairProjectFolderWithEnum(ILogger logger, string documentsFolder)
    {
        if (projectTypes._d1.Count > 0)
        {
            return;
        }

        var p2 = documentsFolder;
        //var p2 = BasePathsHelper.bp;
        //if (!Directory.Exists(p2))
        //{
        //    return;
        //}
        var folders = Directory.GetDirectories(p2, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in folders)
        {
            var fn = Path.GetFileName(item);
            if (fn.EndsWith(SolutionsIndexerStrings.ProjectPostfix))
            {
                ProjectsTypes parameter = ProjectsTypes.None;
                var list = fn.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
                var l2 = list.Replace("_", string.Empty).Trim();
                switch (l2)
                {
                    case "C++":
                        parameter = ProjectsTypes.Cpp;
                        break;
                    //case "":
                    //    parameter = ProjectsTypes.Cs;
                    //    break;
                    default:
                        parameter = EnumHelper.Parse(l2, ProjectsTypes.None);
                        break;
                }

                if (parameter == ProjectsTypes.None)
                {
                    /* 
                     * Toto byl nesmysl. Když vytvořím novou složku (což vytvářím často, protože se furt něco nového učím)
                     * musím upravit i SunamoDevCode. Vytvořit nový nuget. 
                     * Pak ho updatovat do různých apps. Jen CommandsToAll* mám 3. 
                     * Zbuildit, zkopírovat, teprve pak to funguje. 
                     * 
                     * Navíc si nepamatuji že bych toto kdekoliv použil.
                     */
                    logger.LogWarning(Translate.FromKey(XlfKeys.CanTAssignToEnumTypeOfFolder) + " " + item);
                }
                else
                {
                    projectTypes.Add(parameter, list);
                }
            }
        }

        projectTypes.Add(ProjectsTypes.Cs, XlfKeys.Projects);
    }

    public static List<string> onlyRealLoadedSolutionsFolders = new List<string>();
    /// <summary>
    /// In key is fn without .csproj, in value is full path
    /// </summary>
    public static Dictionary<string, List<string>> allCsprojGlobal = new Dictionary<string, List<string>>();
#if ASYNC
    public static
#if ASYNC
    async Task<Dictionary<string, List<string>>>
#else
      Dictionary<string, List<string>>
#endif
 AllGlobalCsprojs(ILogger logger, bool listToClipboardInsteadOfThrowEx = false)
#else
    public static Dictionary<string, List<string>> AllGlobalCsprojs(bool listToClipboardInsteadOfThrowEx = false)
#endif
    {
        if (allCsprojGlobal.Count == 0)
        {
            foreach (var item in fwss)
            {
                foreach (var sln in item.Solutions(usedRepository))
                {
                    SolutionFolder.GetCsprojs(logger, sln);
                    foreach (var item2 in sln.projectsGetCsprojs)
                    {
                        ResultWithExceptionDC<XmlDocument> xml = null;
                        xml = 
#if ASYNC
                            await
#endif
                        XmlDocumentsCache.Get(item2);
                        if (MayExcHelper.MayExc(xml.Exc))
                        {
                            continue;
                        }

                        if (xml.Data != null)
                        {
                            DictionaryHelper.AddOrCreate(allCsprojGlobal, Path.GetFileNameWithoutExtension(item2), item2);
                        }
                    }
                }
            }
        }

        var builder = IsAllProjectNamesUnique(listToClipboardInsteadOfThrowEx);
        if (!builder)
        {
            return null;
        }

        return allCsprojGlobal;
    }

    public static List<string> projectsWithDuplicateName = new List<string>();
    public static bool IsAllProjectNamesUnique(bool listToClipboardInsteadOfThrowEx = false)
    {
        StringBuilder stringBuilder = null;
        if (listToClipboardInsteadOfThrowEx)
        {
            stringBuilder = new StringBuilder();
        }

        bool vr = true;
        foreach (var item in allCsprojGlobal)
        {
            if (item.Value.Count > 1)
            {
                if (listToClipboardInsteadOfThrowEx)
                {
                    foreach (var item2 in item.Value)
                    {
                        stringBuilder.AppendLine(item2);
                    }
                }
                else
                {
                    projectsWithDuplicateName.Add(Path.GetFileName(item.Key));
                    for (int i = 1; i < item.Value.Count; i++)
                    {
                        item.Value.RemoveAt(i);
                    }
                //ThrowEx.MoreThanOneElement("item.Value", item.Value.Count, "Key : "+ item.Key);
                }

                vr = false;
            }
        }

        //if (listToClipboardInsteadOfThrowEx)
        //{
        //    ClipboardHelper.SetText(stringBuilder.ToString());
        //}
        return vr;
    }

    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, SolutionFolderSerialize solutionFolder, PpkOnDriveDC toSelling, string projName = null)
    {
        return CreateSolutionFolder(logger, documentsFolder, null, solutionFolder.fullPathFolder, toSelling, projName);
    }
}