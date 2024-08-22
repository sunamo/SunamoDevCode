namespace SunamoDevCode.SunamoSolutionsIndexer;

public interface IFoldersWithSolutionsInstance
{
    SolutionFolders Solutions(RepositoryLocal r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
