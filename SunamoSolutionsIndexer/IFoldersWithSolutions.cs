namespace SunamoDevCode.SunamoSolutionsIndexer;

public interface IFoldersWithSolutions
{
    SolutionFolders Solutions(RepositoryLocal r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
