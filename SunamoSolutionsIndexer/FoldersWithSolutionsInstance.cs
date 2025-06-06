//namespace SunamoDevCode.SunamoSolutionsIndexer;
//using Microsoft.Extensions.Logging;

//public class FoldersWithSolutions : IFoldersWithSolutions
//{
//    public static FoldersWithSolutions Instance = null;

//    #region data fields
//    public List<SolutionFolder> solutions = null;
//    /// <summary>
//    /// D:\Documents
//    /// </summary>
//    public string documentsFolder = null;
//    #endregion
//    Type type = typeof(FoldersWithSolutions);

//    public RepositoryLocal usedRepository = RepositoryLocal.Vs17;

//    /// <summary>
//    /// Složka ve které se má hledat na složku Projects a složky Visual Studia
//    /// </summary>
//    public FoldersWithSolutionsList fwss = new FoldersWithSolutionsList();

//    #region ctor
//    /// <summary>
//    /// A1 = D:\Documents
//    /// This class should be instaniate only once and then call reload by needs
//    /// A1 toSelling can be null
//    /// </summary>
//    public FoldersWithSolutions(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true, bool notSaveToInstance = false)
//    {
//        if (!notSaveToInstance)
//        {
//            Instance = this;
//        }

//        // documentsFolder může být null / SE, stejně to poté doplňuji z actualPlatform
//        ThrowEx.DirectoryExists(documentsFolder);

//        this.documentsFolder = documentsFolder;
//        if (addAlsoSolutions)
//        {
//            // !notSaveToInstance - useBp
//            Reload(logger, documentsFolder, toSelling, !notSaveToInstance);
//        }

//        // musím nastavit až na konci když přiřadím veškeré proměnné
//        fwss.Add(new FoldersWithSolutions(this));
//    }
//    #endregion

//    #region Returns solutions in various objects
//    public List<SolutionFolderWithFiles> SolutionsWithFiles()
//    {
//        List<SolutionFolderWithFiles> vr = new List<SolutionFolderWithFiles>();
//        foreach (var item in solutions)
//        {
//            vr.Add(new SolutionFolderWithFiles(item));
//        }
//        return vr;
//    }


//    #endregion

//    /// <summary>
//    /// Get all projects in A1(Visual Studio Projects *) and GitHub folder and insert to global variable solutions
//    /// A1 toSelling must be null
//    /// </summary>
//    /// <param name="documentsFolder"></param>
//    public List<SolutionFolder> Reload(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool useBp = true, bool ignorePartAfterUnderscore = false, bool sunamoAndSunamoWithoutDepProjectsAsFirst = true)
//    {
//        if (documentsFolder.Contains("Project"))
//        {
//            throw new Exception("Takto to zatím nefunguje (jako sln se předávají poté složky .git atd., typy projektů jsou Aps atd.) ale mohlo by");
//        }

//        FoldersWithSolutions.PairProjectFolderWithEnum(logger, documentsFolder);

//        // Get all projects in A1(Visual Studio Projects *) and GitHub folder
//        List<string> solutionFolders = ReturnAllProjectFolders(documentsFolder, useBp /*, Path.Combine(documentsFolder, SolutionsIndexerStrings.GitHubMy)*/);

//        List<string> projOnlyNames = new List<string>(solutionFolders.Count);
//        var ta = solutionFolders.ToArray();
//        var on = FS.OnlyNamesNoDirectEdit(ta);
//        projOnlyNames.AddRange(on);
//        // Initialize global variable solutions
//        solutions = new List<SolutionFolder>(solutionFolders.Count);

//        for (int i = 0; i < solutionFolders.Count; i++)
//        {
//            var solutionFolder = solutionFolders[i];

//            SolutionFolder sf = CreateSolutionFolder(documentsFolder, solutionFolder, toSelling, useBp, projOnlyNames[i]);

//            var fnwoe = Path.GetFileNameWithoutExtension(solutionFolder);

//            if ((fnwoe == "sunamo" || fnwoe == "PlatformIndependentNuGetPackages") && sunamoAndSunamoWithoutDepProjectsAsFirst)
//            {
//                List<string> f = new List<string>(); //FS.FoldersWithSubfolder(solutionFolder, ".git");
//                foreach (var item in f)
//                {
//                    SolutionFolder sf2 = CreateSolutionFolder(documentsFolder, item, toSelling, useBp, projOnlyNames[i]);
//                    solutions.Add(sf2);
//                }
//            }

//            solutions.Add(sf);
//        }

//        return solutions;
//    }


//    public List<string> onlyRealLoadedSolutionsFolders = new List<string>();

//    public void AllCsprojs()
//    {

//    }

//    /// <summary>
//    /// In key is fn without .csproj, in value is full path
//    /// </summary>
//    public Dictionary<string, List<string>> allCsprojGlobal = new Dictionary<string, List<string>>();

//    public
//#if ASYNC
//    async Task<Dictionary<string, List<string>>>
//#else
//    Dictionary<string, List<string>>
//#endif
//         AllGlobalCsprojs(bool listToClipboardInsteadOfThrowEx = false)
//    {
//        if (allCsprojGlobal.Count == 0)
//        {
//            foreach (var item in fwss)
//            {
//                foreach (var sln in item.Solutions(usedRepository))
//                {
//                    SolutionFolder.GetCsprojs(sln);
//                    foreach (var item2 in sln.projectsGetCsprojs)
//                    {
//                        ResultWithExceptionDC<XmlDocument> xml = null;

//                        xml =
//#if ASYNC
//                        await
//#endif
//                        XmlDocumentsCache.Get(item2);

//                        if (MayExcHelper.MayExc(xml.exc))
//                        {
//                            continue;
//                        }

//                        if (xml.Data != null)
//                        {
//                            DictionaryHelper.AddOrCreate(allCsprojGlobal, Path.GetFileNameWithoutExtension(item2), item2);
//                        }
//                    }
//                }
//            }
//        }
//        var b = IsAllProjectNamesUnique(listToClipboardInsteadOfThrowEx);
//        if (!b)
//        {
//            return null;
//        }
//        return allCsprojGlobal;
//    }

//    public List<string> projectsWithDuplicateName = new List<string>();

//    public bool IsAllProjectNamesUnique(bool listToClipboardInsteadOfThrowEx = false)
//    {
//        StringBuilder sb = null;

//        if (listToClipboardInsteadOfThrowEx)
//        {
//            sb = new StringBuilder();
//        }

//        bool vr = true;
//        foreach (var item in allCsprojGlobal)
//        {
//            if (item.Value.Count > 1)
//            {
//                if (listToClipboardInsteadOfThrowEx)
//                {
//                    foreach (var item2 in item.Value)
//                    {
//                        sb.AppendLine(item2);
//                    }
//                }
//                else
//                {
//                    projectsWithDuplicateName.Add(Path.GetFileName(item.Key));

//                    for (int i = 1; i < item.Value.Count; i++)
//                    {
//                        item.Value.RemoveAt(i);
//                    }
//                    //ThrowEx.MoreThanOneElement("item.Value", item.Value.Count, "Key : "+ item.Key);
//                }
//                vr = false;
//            }
//        }

//        if (listToClipboardInsteadOfThrowEx)
//        {
//            //ClipboardHelper.SetText(sb.ToString());
//        }


//        return vr;
//    }

//    public SolutionFolder CreateSolutionFolder(SolutionFolderSerialize solutionFolder, PpkOnDriveDC toSelling, bool useBp, string projName = null)
//    {
//        return CreateSolutionFolder(null, solutionFolder.fullPathFolder, toSelling, useBp, projName);
//    }

//    /// <summary>
//    /// toSelling can be null
//    /// </summary>
//    /// <param name="solutionFolder"></param>
//    /// <param name="toSelling"></param>
//    /// <param name="projName"></param>
//    /// <returns></returns>
//    public SolutionFolder CreateSolutionFolder(string documentsFolder, string solutionFolder, PpkOnDriveDC toSelling, bool useBp, string projName = null)
//    {
//        return CreateSolutionFolder(documentsFolder, null, solutionFolder, toSelling, useBp, projName);
//    }

//    /// <summary>
//    /// toSelling can be null
//    /// </summary>
//    /// <param name="sfs"></param>
//    /// <param name="solutionFolder"></param>
//    /// <param name="toSelling"></param>
//    /// <param name="projName"></param>
//    /// <returns></returns>
//    public SolutionFolder CreateSolutionFolder(string documentsFolder, SolutionFolderSerialize sfs, string solutionFolder, PpkOnDriveDC toSelling, bool useBp, string projName = null)
//    {

//        if (projName == null)
//        {
//            projName = Path.GetFileName(solutionFolder);
//        }
//        SolutionFolder sf = null;
//        if (sfs != null)
//        {
//            sf = new SolutionFolder(sfs);
//        }
//        else
//        {
//            sf = new SolutionFolder();
//        }
//        sf.repository = RepositoryFromFullPath(solutionFolder, useBp);

//        FoldersWithSolutions.IdentifyProjectType(documentsFolder, solutionFolder, sf, useBp);

//        sf.displayedText = GetDisplayedName(solutionFolder);
//        sf.fullPathFolder = solutionFolder;

//        // Nevím zda je to nutné tak jsem to zakomentoval aby to bylo rychlejší
//        //sf.projects = new DebugCollection<string>( SolutionsIndexerHelper.ProjectsInSolution(true, sf.fullPathFolder));
//        //sf.SourceOfProjects = SourceOfProjects.ProjectsInSolution;

//        sf.UpdateModules(toSelling);
//        sf.nameSolutionWithoutDiacritic = SH.TextWithoutDiacritic(projName);
//        return sf;
//    }

//    private RepositoryLocal RepositoryFromFullPath(string fullPathFolder, bool useBp)
//    {
//        if (useBp)
//        {
//            if (fullPathFolder.Contains(SolutionsIndexerStrings.VisualStudio2017))
//            {
//                return RepositoryLocal.Vs17;
//            }
//            else if (fullPathFolder.Contains(SolutionsIndexerConsts.BitBucket))
//            {
//                return RepositoryLocal.BitBucket;
//            }
//            else if (fullPathFolder.Contains(BasePathsHelper.cRepos))
//            {
//                return RepositoryLocal.Vs17;
//            }
//            else if (fullPathFolder.Contains(BasePathsHelper.bpVps))
//            {
//                return RepositoryLocal.Vs17;
//            }
//            ThrowEx.NotImplementedCase(fullPathFolder);
//        }
//        return RepositoryLocal.All;
//    }



//    /// <summary>
//    /// Get name based on relative but always fully recognized project
//    /// </summary>
//    /// <param name="item"></param>
//    private string GetDisplayedName(string item)
//    {
//        return SolutionsIndexerHelper.GetDisplayedSolutionName(item);
//    }

//    public IList<SolutionFolder> SolutionsUap(IList<string> skipThese = null)
//    {
//        var slns = Solutions(RepositoryLocal.Vs17, false, skipThese);
//        var uap = slns.Where(d => d.fullPathFolder.Contains(@"\_Uap\")).ToList();
//        return uap;
//    }

//    /// <summary>
//    /// A1 not have to be wildcard
//    /// </summary>
//    /// <param name="vs17"></param>
//    /// <param name="toFind"></param>
//    /// <returns></returns>
//    public IList<SolutionFolder> SolutionsWildcard(RepositoryLocal r, string mayWildcard)
//    {
//        var result = Solutions(r);


//        for (int i = result.Count - 1; i >= 0; i--)
//        {
//            var ns = result[i].nameSolution;
//            if (!SH.MatchWildcard(ns, mayWildcard))
//            {
//                result.RemoveAt(i);
//            }
//        }


//        return result;
//    }

//    public static ProjectsTypes GetProjectType(SolutionFolder sf)
//    {
//        return sf.typeProjectFolder;
//    }

//    /// <summary>
//    /// Simple returns global variable solutions
//    /// Exclude from SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode if Debugger is attached and !A2
//    /// A3 - can use wildcard
//    /// </summary>
//    // , bool sunamoAndSunamoWithoutDepProjectsAsFirst = true
//    public SolutionFolders Solutions(RepositoryLocal r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None)
//    {
//        SolutionFolders result = new SolutionFolders(solutions);

//        if (prioritize != ProjectsTypes.None)
//        {
//            var sorted = CA.SortSetFirst<SolutionFolder, SolutionFolders, ProjectsTypes>(result, GetProjectType, prioritize);
//            result = new SolutionFolders(sorted);
//        }

//        // TODO: Dočasně toto zakomentuji. Stejně tu už nemám žádné BitBuckety a mám všude Vs17, přičemž ve result jsou všechny All, takže mi to nevracelo nic
//        //if (r != Repository.All)
//        //{
//        //    result.RemoveAll(d => d.repository != r);
//        //}

//        List<string> skip = null;
//        if (skipThese != null)
//        {
//            skip = skipThese.ToList();
//        }
//        else
//        {
//            skip = new List<string>();
//        }

//        if (!loadAll)
//        {
//            if (Debugger.IsAttached)
//            {
//                skip.AddRange(SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode);
//            }
//        }

//        Dictionary<string, Wildcard> dict = new Dictionary<string, Wildcard>();
//        foreach (var item in skip)
//        {
//            dict.Add(item, new Wildcard(item));
//        }

//        var l = result.Count;
//        for (int i = result.Count - 1; i >= 0; i--)
//        {
//            var it = result[i];
//            foreach (var item in dict)
//            {
//                if (item.Value.IsMatch(it.nameSolution))
//                {
//                    result.RemoveAt(i);
//                    break;
//                }
//            }
//        }

//        var l2 = result.Count;
//        return result;
//    }

//    /// <summary>
//    /// Return fullpath for all folder recursively - specific and ordi
//    /// </summary>
//    /// <param name="folderWithVisualStudioFolders"></param>
//    /// <param name="alsoAdd"></param>
//    private List<string> ReturnAllProjectFolders(string folderWithVisualStudioFolders, bool useBp = true, params string[] alsoAdd)
//    {
//        List<string> projs = new List<string>();
//        var bp = BasePathsHelper.actualPlatform;

//        if (!useBp)
//        {
//            bp = folderWithVisualStudioFolders;
//        }

//        if (Directory.Exists(bp))
//        {
//            if (BasePathsHelper.bpVps == bp)
//            {
//                AddProjectsFolder(projs, bp);
//            }
//            else if (bp == folderWithVisualStudioFolders)
//            {
//                List<string> slozkySJazyky = null;

//                try
//                {
//                    slozkySJazyky = Directory.GetDirectories(bp).ToList();
//                }
//                catch (Exception ex)
//                {
//                    //continue;
//                }

//                foreach (var item2 in slozkySJazyky)
//                {
//                    #region New
//                    string pfn = Path.GetFileName(item2);
//                    if (SolutionsIndexerHelper.IsTheSolutionsFolder(pfn))
//                    {
//                        AddProjectsFolder(projs, item2);
//                    }
//                    #endregion
//                }
//            }
//            else
//            {
//                List<string> visualStudioFolders = new List<string>([bp]); // Directory.GetDirectories(folderWithVisualStudioFolders, VpsHelperSunamo.IsQ ? "_" : SolutionsIndexerStrings.VisualStudio2017, SearchOption.TopDirectoryOnly));
//                foreach (var item in alsoAdd)
//                {
//                    AddProjectsFolder(projs, item);
//                }
//                foreach (var item in visualStudioFolders)
//                {
//                    List<string> slozkySJazyky = null;
//                    List<string> slozkySJazykyOutsideVs17 = new List<string>();
//                    try
//                    {
//                        slozkySJazyky = Directory.GetDirectories(item).ToList();
//                    }
//                    catch (Exception ex)
//                    {
//                        continue;
//                    }

//                    slozkySJazykyOutsideVs17.Insert(0, Path.Combine(folderWithVisualStudioFolders.Replace("E:\\", "D:\\"), SolutionsIndexerConsts.BitBucket));

//                    foreach (var item2 in slozkySJazyky)
//                    {
//                        #region New
//                        string pfn = Path.GetFileName(item2);
//                        if (SolutionsIndexerHelper.IsTheSolutionsFolder(pfn))
//                        {
//                            AddProjectsFolder(projs, item2);
//                        }
//                        #endregion
//                    }

//                    foreach (var item2 in slozkySJazykyOutsideVs17)
//                    {
//                        #region New
//                        if (Directory.Exists(item2))
//                        {
//                            string pfn = Path.GetFileName(item2);

//                            AddProjectsFolder(projs, item2);
//                        }
//                        #endregion
//                    }
//                }
//            }
//        }
//        //else if (VpsHelperSunamo.IsVps)
//        //{
//        //    return new List<string>();
//        //}
//        //else
//        //{
//        //    throw new Exception(folderWithVisualStudioFolders + " not exists, therefore will be return none slsn");
//        //}
//        CAChangeContent.ChangeContent0(null, projs, SH.FirstCharUpper);
//        return projs;
//    }

//    public Tuple<List<string>, List<string>> ReturnNormalAndSpecialFolders(string sloz)
//    {
//        List<string>? special = null;
//        List<string> normal = null;
//        ReturnNormalAndSpecialFolders(sloz, out special, out normal);
//        return new Tuple<List<string>, List<string>>(normal, special);
//    }

//    /// <summary>
//    /// Projde nerek slozky v A1 a vrati mi do A2 ty ktere zacinali na _ a do A3 zbytek.
//    /// </summary>
//    /// <param name="sloz"></param>
//    /// <param name="Specialni"></param>
//    /// <param name="normal2"></param>
//    public void ReturnNormalAndSpecialFolders(string sloz, out List<string> spec, out List<string> normal)
//    {
//        spec = new List<string>();
//        normal = new List<string>();
//        try
//        {
//            var slo = Directory.GetDirectories(sloz);
//            foreach (string var in slo)
//            {
//                string nazev = Path.GetFileName(var);
//                if (nazev.StartsWith("_"))
//                {
//                    spec.Add(var);
//                }
//                else
//                {
//                    normal.Add(var);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//        }
//    }

//    /// <summary>
//    /// Find out usuall folder and specific (which starting on _) and process then to any level
//    /// </summary>
//    /// <param name="proj"></param>
//    /// <param name="slozka"></param>
//    void AddProjectsFolder(List<string> proj, string slozka)
//    {
//        List<string> spec, norm;
//        ReturnNormalAndSpecialFolders(slozka, out spec, out norm);

//        norm = CA.EnsureBackslash(norm);
//        proj.AddRange(norm);
//        foreach (string var2 in spec)
//        {
//            AddProjectsFolder(proj, var2);
//        }
//    }

//    public List<string> FullPathFolders(RepositoryLocal usedRepository, List<string> returnOnlyThese = null, ProjectsTypes prioritize = ProjectsTypes.None)
//    {
//        Dictionary<string, SolutionFolder> sf = null;

//        return FullPathFolders(usedRepository, sf, returnOnlyThese, prioritize);
//    }

//    public List<string> FullPathFolders(RepositoryLocal usedRepository, Dictionary<string, SolutionFolder> sf, List<string> returnOnlyThese = null, ProjectsTypes prioritize = ProjectsTypes.None)
//    {
//        // TODO: dodělat to podle ProjectsTypes prioritize, ale otázka je jestli to vůbec potřebuji

//        List<string> ls = new List<string>();
//        foreach (var item in fwss)
//        {
//            var slns = item.Solutions(usedRepository);

//            foreach (var sln in slns)
//            {
//                if (returnOnlyThese != null)
//                {
//#if DEBUG
//                    if (sln.nameSolution.Contains("OnlyWeb"))
//                    {

//                    }
//#endif
//                    if (!returnOnlyThese.Contains(sln.nameSolution))
//                    {
//                        continue;
//                    }
//                }

//                if (sf != null)
//                {
//                    sf.Add(sln.fullPathFolder, sln);
//                }
//                ls.Add(sln.fullPathFolder);
//            }
//        }
//        return ls;
//    }
//}