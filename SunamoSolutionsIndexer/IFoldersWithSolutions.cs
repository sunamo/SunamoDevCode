using SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFoldersNs;
using SunamoDevCode.SunamoSolutionsIndexer.Enums;

namespace SunamoDevCode;

public interface IFoldersWithSolutionsInstance
{
    SolutionFolders Solutions(Repository r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
