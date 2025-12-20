// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;
public partial class FoldersWithSolutions
{
    /// <summary>
    /// Find out usuall folder and specific (which starting on _) and process then to any level
    /// </summary>
    /// <param name = "proj"></param>
    /// <param name = "slozka"></param>
    void AddProjectsFolder(List<string> proj, string slozka)
    {
        List<string> spec, norm;
        ReturnNormalAndSpecialFolders(slozka, out spec, out norm);
        norm = CA.EnsureBackslash(norm);
        proj.AddRange(norm);
        foreach (string var2 in spec)
        {
            AddProjectsFolder(proj, var2);
        }
    }

    public static List<string> FullPathFolders(RepositoryLocal usedRepository, List<string> returnOnlyThese = null)
    {
        Dictionary<string, SolutionFolder> sf = null;
        return FullPathFolders(usedRepository, sf, returnOnlyThese);
    }

    public static List<string> FullPathFolders(RepositoryLocal usedRepository, Dictionary<string, SolutionFolder> sf, List<string> returnOnlyThese = null)
    {
        List<string> lines = new List<string>();
        foreach (var item in fwss)
        {
            var slns = item.Solutions(usedRepository);
            foreach (var sln in slns)
            {
                if (returnOnlyThese != null)
                {
#if DEBUG
                    if (sln.nameSolution.Contains("OnlyWeb"))
                    {
                    }
#endif
                    if (!returnOnlyThese.Contains(sln.nameSolution))
                    {
                        continue;
                    }
                }

                if (sf != null)
                {
                    sf.Add(sln.fullPathFolder, sln);
                }

                lines.Add(sln.fullPathFolder);
            }
        }

        return lines;
    }
}