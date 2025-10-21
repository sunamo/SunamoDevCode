// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode;

using System.Collections.Generic;
using System.Linq;

public class GetSlns
{
    public static List<string> GetSolutions(ILogger logger, bool onlyCs = false)
    {
        var parameter = @"E:\vs\";
        FoldersWithSolutions.PairProjectFolderWithEnum(logger, parameter);
        FoldersWithSolutions d = new FoldersWithSolutions(logger, parameter, null, false);
        d.Reload(logger, parameter, null);

        List<SolutionFolder> solutionFolders = d.Solutions(SunamoDevCode.Enums.RepositoryLocal.Vs17);
        if (onlyCs)
        {
            solutionFolders = solutionFolders.Where(d => d.typeProjectFolder == ProjectsTypes.Cs).ToList();
        }

        return solutionFolders.Select(d => d.fullPathFolder).ToList();
    }
}