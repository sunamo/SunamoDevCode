namespace SunamoDevCode;

using Microsoft.Extensions.Logging;

public class GetCsprojs
{

    /// <summary>
    /// Vrátí plné cesty k složce řešení
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="__NotCore_Projects"></param>
    /// <param name="__NotCoreWeb_Projects"></param>
    /// <param name="__OnlyWindowsCore_Projects"></param>
    /// <returns></returns>
    public static List<string> GetCsprojsAll(ILogger logger, bool __NotCore_Projects = false, bool __NotCoreWeb_Projects = false, bool __OnlyWindowsCore_Projects = false)
    {
        var slns = GetSlns.GetSolutions(logger);
        //FoldersWithSolutions.fwss.Where(d => d.)

        if (!__NotCore_Projects)
        {
            slns = slns.Where(d => !d.Contains("__NotCore_Projects")).ToList();
        }
        if (!__NotCoreWeb_Projects)
        {
            slns = slns.Where(d => !d.Contains("__NotCoreWeb_Projects")).ToList();
        }
        if (!__OnlyWindowsCore_Projects)
        {
            slns = slns.Where(d => !d.Contains("__OnlyWindowsCore_Projects")).ToList();
        }

        List<string> result = [];
        foreach (var item in slns)
        {
            result.AddRange(GetCsprojInSolution(logger, item).Item2);
        }

        return result;
    }

    public static List<string> GetFoldersWithAtLeastOneCsprojInSolution(ILogger logger, string slnFolder)
    {
        var f = FSGetFolders.GetFoldersEveryFolderWhichContainsFiles(logger, slnFolder, "*.csproj", SearchOption.TopDirectoryOnly);
        for (int i = f.Count - 1; i >= 0; i--)
        {
            var csprojs = FSGetFiles.GetFilesEveryFolder(logger, f[i], "*.csproj", SearchOption.TopDirectoryOnly).ToList();

            if (csprojs.Count == 0)
            {
                Error("No csproj");
                f.RemoveAt(i);
            }
            else if (csprojs.Count > 1)
            {
                Error("More than one csproj");
                f.RemoveAt(i);
            }


        }

        return f;
    }

    /// <summary>
    /// 1 - csprojFolderPaths
    /// 2 - csprojPaths
    /// </summary>
    /// <param name="slnFolder"></param>
    /// <returns></returns>
    public static (List<string>, List<string>) GetCsprojInSolution(ILogger logger, string slnFolder)
    {
        List<string> csprojPaths = [];

        var csprojFolderPaths = FSGetFolders.GetFoldersEveryFolderWhichContainsFiles(logger, slnFolder, "*.csproj", SearchOption.TopDirectoryOnly);
        for (int i = csprojFolderPaths.Count - 1; i >= 0; i--)
        {
            var fi = csprojFolderPaths[i];
            var csprojs = FSGetFiles.GetFilesEveryFolder(logger, fi, "*.csproj", SearchOption.TopDirectoryOnly).ToList();

            if (csprojs.Count == 0)
            {
                Error("No csproj in " + fi);
                csprojFolderPaths.RemoveAt(i);
            }
            else if (csprojs.Count > 1)
            {
                foreach (var item in csprojs.Where(d => d.Contains(" - Backup")))
                {
                    File.Delete(item);
                }
                csprojs = csprojs.Where(d => !d.Contains(" - Backup")).ToList();
                if (csprojs.Count > 1)
                {
                    Error("More than one csproj in " + fi);
                    csprojFolderPaths.RemoveAt(i);
                }
            }
            else
            {
                csprojPaths.Add(csprojs[0]);
            }
            //E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoCl\SunamoCl.csproj
        }

        csprojPaths.Reverse();

        return (csprojFolderPaths, csprojPaths);
    }

    static void Error(string s)
    {
        throw new Exception(s);
    }















    public static CsprojsInSolution GetCsprojInSwdNotmineAndSunamo(ILogger logger)
    {
        var PlatformIndependentNuGetPackages = GetCsprojInSolutionClass(logger, @"E:\vs\Projects\PlatformIndependentNuGetPackages\");
        return PlatformIndependentNuGetPackages;

        //var sunamoNotmine = GetCsprojInSolutionClass(@"E:\vs\Projects\sunamo.notmine\");
        ////var sunamo = GetCsprojInSolutionClass(@"E:\vs\Projects\PlatformIndependentNuGetPackages\");

        //return PlatformIndependentNuGetPackages.Intersect(sunamoNotmine);
    }

    public static CsprojsInSolution GetCsprojInSolutionClass(ILogger logger, string slnFolder, bool automaticallyAddToUniqueCsprojs = true)
    {
        var (csprojNames, csprojPaths) = GetCsprojInSolution(logger
            , slnFolder);
        var result = new CsprojsInSolution { CsprojFolderPaths = csprojNames, CsprojPaths = csprojPaths };
        if (automaticallyAddToUniqueCsprojs)
        {
            UniqueCsprojs.AddFromSlnFolder(result);
        }

        return result;
    }














    public static List<string> GetCsprojsInSolution(ILogger logger, string slnFolder)
    {
        var f = GetFoldersWithAtLeastOneCsprojInSolution(logger, slnFolder);
        List<string> result = new List<string>(f.Count);
        foreach (var item in f)
        {
            result.Add(FSGetFiles.GetFilesEveryFolder(logger, item, "*.csproj", SearchOption.TopDirectoryOnly)[0]);
        }
        return result;
    }



    //private static void Error(string v)
    //{
    //    throw new Exception(v);
    //}

    public static Dictionary<string, string> GetCsprojsAllDict(ILogger logger)
    {
        Dictionary<string, string> d = [];

        var csprojs = GetCsprojsAll(logger);
        foreach (var item in csprojs)
        {
            var fn = Path.GetFileName(item);

            if (fn == "Runner.csproj")
            {
                continue;
            }

            d.Add(fn, item);
        }

        return d;
    }

}