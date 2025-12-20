// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;
public partial class FoldersWithSolutions
{
    /// <summary>
    /// toSelling can be null
    /// </summary>
    /// <param name = "solutionFolder"></param>
    /// <param name = "toSelling"></param>
    /// <param name = "projName"></param>
    /// <returns></returns>
    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, string solutionFolder, PpkOnDriveDC toSelling, string projName = null)
    {
        return CreateSolutionFolder(logger, documentsFolder, null, solutionFolder, toSelling, projName);
    }

    /// <summary>
    /// toSelling can be null
    /// </summary>
    /// <param name = "sfs"></param>
    /// <param name = "solutionFolder"></param>
    /// <param name = "toSelling"></param>
    /// <param name = "projName"></param>
    /// <returns></returns>
    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, SolutionFolderSerialize sfs, string solutionFolder, PpkOnDriveDC toSelling, string projName = null)
    {
        if (projName == null)
        {
            projName = Path.GetFileName(solutionFolder);
        }

        SolutionFolder sf = null;
        if (sfs != null)
        {
            sf = new SolutionFolder(sfs);
        }
        else
        {
            sf = new SolutionFolder();
        }

        sf.repository = RepositoryFromFullPath(solutionFolder);
        IdentifyProjectType(documentsFolder, solutionFolder, sf);
        sf.displayedText = GetDisplayedName(solutionFolder);
        sf.fullPathFolder = solutionFolder;
        // Nevím zda je to nutné tak jsem to zakomentoval aby to bylo rychlejší
        //sf.projects = new DebugCollection<string>( SolutionsIndexerHelper.ProjectsInSolution(true, sf.fullPathFolder));
        //sf.SourceOfProjects = SourceOfProjects.ProjectsInSolution;
        sf.UpdateModules(logger, toSelling);
        sf.nameSolutionWithoutDiacritic = SH.TextWithoutDiacritic(projName);
        return sf;
    }

    private static RepositoryLocal RepositoryFromFullPath(string fullPathFolder)
    {
        if (fullPathFolder.Contains(SolutionsIndexerStrings.VisualStudio2017))
        {
            return RepositoryLocal.Vs17;
        }
        else if (fullPathFolder.Contains(SolutionsIndexerConsts.BitBucket))
        {
            return RepositoryLocal.BitBucket;
        }
        else if (fullPathFolder.Contains(BasePathsHelper.cRepos))
        {
            return RepositoryLocal.Vs17;
        }
        else if (fullPathFolder.Contains(BasePathsHelper.bpVps))
        {
            return RepositoryLocal.Vs17;
        }

        ThrowEx.NotImplementedCase(fullPathFolder);
        return RepositoryLocal.All;
    }

    /// <summary>
    /// Get name based on relative but always fully recognized project
    /// </summary>
    /// <param name = "item"></param>
    private static string GetDisplayedName(string item)
    {
        return SolutionsIndexerHelper.GetDisplayedSolutionName(item);
    }

    public List<SolutionFolder> SolutionsUap(IList<string> skipThese = null)
    {
        var slns = Solutions(RepositoryLocal.Vs17, false, skipThese);
        var uap = slns.Where(d => d.fullPathFolder.Contains(@"\_Uap\")).ToList();
        return uap;
    }

    /// <summary>
    /// A1 not have to be wildcard
    /// </summary>
    /// <param name = "vs17"></param>
    /// <param name = "toFind"></param>
    /// <returns></returns>
    public IList<SolutionFolder> SolutionsWildcard(RepositoryLocal r, string mayWildcard)
    {
        var result = Solutions(r);
        for (int i = result.Count - 1; i >= 0; i--)
        {
            var ns = result[i].nameSolution;
            if (!SH.MatchWildcard(ns, mayWildcard))
            {
                result.RemoveAt(i);
            }
        }

        return result;
    }

    /// <summary>
    /// Simple returns global variable solutions
    /// Exclude from SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode if Debugger is attached and !A2
    /// A3 - can use wildcard
    /// </summary>
    public List<SolutionFolder> Solutions(RepositoryLocal r, bool loadAll = true, IList<string> skipThese = null /*, ProjectsTypes cs = ProjectsTypes.Cs*/)
    {
        var result = new List<SolutionFolder>(solutions);
        if (r != RepositoryLocal.All)
        {
            result.RemoveAll(d => d.repository != r);
        }

        List<string> skip = null;
        if (skipThese != null)
        {
            skip = skipThese.ToList();
        }
        else
        {
            skip = new List<string>();
        }

        if (!loadAll)
        {
            if (Debugger.IsAttached)
            {
                skip.AddRange(SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode);
            }
        }

        Dictionary<string, Wildcard> dict = new Dictionary<string, Wildcard>();
        foreach (var item in skip)
        {
            dict.Add(item, new Wildcard(item));
        }

        var list = result.Count;
        for (int i = result.Count - 1; i >= 0; i--)
        {
            var it = result[i];
            foreach (var item in dict)
            {
                if (item.Value.IsMatch(it.nameSolution))
                {
                    result.RemoveAt(i);
                    break;
                }
            }
        }

        var l2 = result.Count;
        //result.RemoveAll(d => CAG.IsEqualToAnyElement(d.nameSolution, skip));
        ////////DebugLogger.Instance.WriteCount("Solutions in " + documentsFolder, solutions);
        return result;
    }

    /// <summary>
    /// Return fullpath for all folder recursively - specific and ordi
    /// </summary>
    /// <param name = "folderWithVisualStudioFolders"></param>
    /// <param name = "alsoAdd"></param>
    private List<string> ReturnAllProjectFolders(params string[] alsoAdd)
    {
        List<string> projs = new List<string>();
        var bp = BasePathsHelper.bpMb;
        //if (Directory.Exists(bp))
        //{
        //if (BasePathsHelper.bpVps == bp)
        //{
        //    AddProjectsFolder(projs, bp);
        //}
        //else
        //{
        List<string> visualStudioFolders = new List<string>([bp]); // Directory.GetDirectories(folderWithVisualStudioFolders, VpsHelperSunamo.IsQ ? "_" : SolutionsIndexerStrings.VisualStudio2017, SearchOption.TopDirectoryOnly));
        foreach (var item in alsoAdd)
        {
            AddProjectsFolder(projs, item);
        }

        foreach (var item in visualStudioFolders)
        {
            List<string> slozkySJazyky = null;
            List<string> slozkySJazykyOutsideVs17 = new List<string>();
            try
            {
                slozkySJazyky = Directory.GetDirectories(item).ToList();
            }
            catch (Exception ex)
            {
                continue;
            }

            //slozkySJazykyOutsideVs17SH.Leading(Path.Combine(folderWithVisualStudioFolders.Replace("E:\\", "D:\\"), SolutionsIndexerConsts.BitBucket));
            foreach (var item2 in slozkySJazyky)
            {
#region New
                string pfn = Path.GetFileName(item2);
                if (SolutionsIndexerHelper.IsTheSolutionsFolder(pfn))
                {
                    AddProjectsFolder(projs, item2);
                }
#endregion
            }

            foreach (var item2 in slozkySJazykyOutsideVs17)
            {
#region New
                if (Directory.Exists(item2))
                {
                    string pfn = Path.GetFileName(item2);
                    AddProjectsFolder(projs, item2);
                }
#endregion
            }
        }

        //}
        //}
        //else if (VpsHelperSunamo.IsVps)
        //{
        //    return new List<string>();
        //}
        //else
        //{
        //    throw new Exception(folderWithVisualStudioFolders + " not exists, therefore will be return none slsn");
        //}
        CAChangeContent.ChangeContent0(null, projs, SH.FirstCharUpper);
        return projs;
    }

    public static Tuple<List<string>, List<string>> ReturnNormalAndSpecialFolders(string sloz)
    {
        List<string> special = null;
        List<string> normal = null;
        ReturnNormalAndSpecialFolders(sloz, out special, out normal);
        return new Tuple<List<string>, List<string>>(normal, special);
    }

    /// <summary>
    /// Projde nerek slozky v A1 a vrati mi do A2 ty ktere zacinali na _ a do A3 zbytek.
    /// </summary>
    /// <param name = "sloz"></param>
    /// <param name = "Specialni"></param>
    /// <param name = "normal2"></param>
    public static void ReturnNormalAndSpecialFolders(string sloz, out List<string> spec, out List<string> normal)
    {
        spec = new List<string>();
        normal = new List<string>();
        try
        {
            var slo = Directory.GetDirectories(sloz);
            foreach (string var in slo)
            {
                string nazev = Path.GetFileName(var);
                if (nazev.StartsWith("_"))
                {
                    spec.Add(var);
                }
                else
                {
                    normal.Add(var);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}