// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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