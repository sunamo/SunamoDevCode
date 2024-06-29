namespace SunamoDevCode;


public class PushSolutionsData
{
    internal bool mergeAndFetch = false;
    internal bool addGitignore = false;
    internal List<string> onlyThese = null;
    internal bool? cs = null;
    /// <summary>
    /// Když nemám očíslované, počítá od 0. tedy warning = 0, error = 1, fatal = 2, ve VS debuggeru při error | fatal vidím 3 
    /// </summary>
    internal GitTypesOfMessages checkForGit = GitTypesOfMessages.error | GitTypesOfMessages.fatal;
    internal void Set(bool mergeAndFetch, bool addGitignore = false)
    {
        this.mergeAndFetch = mergeAndFetch;
        this.addGitignore = addGitignore;
    }
}