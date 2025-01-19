namespace SunamoDevCode.Aps.Data;

public class AsyncPushSolutions : AsyncLoadingBaseDC<AsyncPushSolutions, ProgressBar>
{
    public List<SolutionFolder> foldersWithSolutions = null;
    public bool release;
    public GitBashBuilder gitBashBuilder;
    public string pushArgs;
    public string commitMessage;
    //public string fullPathFolder;
    public PushSolutionsData pushSolutionsData;
    public GitBashBuilder gitStatus;
}