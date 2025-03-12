namespace SunamoDevCode;

using System.Collections.Generic;
using System.Linq;

public class GetSlns
{
    public static List<string> GetSolutions(ILogger logger, bool onlyCs = false)
    {
        var p = @"E:\vs\";
        FoldersWithSolutions.PairProjectFolderWithEnum(logger, p);
        FoldersWithSolutions d = new FoldersWithSolutions(logger, p, null, false);
        d.Reload(logger, p, null, false);

        List<SolutionFolder> solutionFolders = d.Solutions(SunamoDevCode.Enums.RepositoryLocal.Vs17);
        if (onlyCs)
        {
            solutionFolders = solutionFolders.Where(d => d.typeProjectFolder == ProjectsTypes.Cs).ToList();
        }

        return solutionFolders.Select(d => d.fullPathFolder).ToList();
    }
}