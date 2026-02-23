namespace SunamoDevCode;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides methods for retrieving Visual Studio solution folders.
/// </summary>
public class GetSlns
{
    /// <summary>
    /// Gets all solution folder paths, optionally filtered to C# projects only.
    /// </summary>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <param name="onlyCs">If true, returns only C# project solution folders.</param>
    /// <returns>List of solution folder paths.</returns>
    public static List<string> GetSolutions(ILogger logger, bool onlyCs = false)
    {
        var parameter = @"E:\vs\";
        FoldersWithSolutions.PairProjectFolderWithEnum(logger, parameter);
        FoldersWithSolutions d = new FoldersWithSolutions(logger, parameter, null!, false);
        d.Reload(logger, parameter, null!);

        List<SolutionFolder> solutionFolders = d.GetSolutions(SunamoDevCode.Enums.RepositoryLocal.Vs17);
        if (onlyCs)
        {
            solutionFolders = solutionFolders.Where(d => d.TypeProjectFolder == ProjectsTypes.Cs).ToList();
        }

        return solutionFolders.Select(d => d.FullPathFolder).ToList();
    }
}