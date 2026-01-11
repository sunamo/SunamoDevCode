// variables names: ok
namespace SunamoDevCode.Aps.Data;

/// <summary>
/// EN: Data for asynchronous push of solutions
/// CZ: Data pro asynchronní push solutions
/// </summary>
public class AsyncPushSolutions : AsyncLoadingBaseDC<AsyncPushSolutions, IProgressBarDC>
{
    /// <summary>
    /// EN: List of solution folders to push
    /// CZ: Seznam solution složek k pushnutí
    /// </summary>
    public List<SolutionFolder> FoldersWithSolutions { get; set; } = null;

    /// <summary>
    /// EN: Whether this is a release build
    /// CZ: Zda se jedná o release build
    /// </summary>
    public bool Release { get; set; }

    /// <summary>
    /// EN: Git bash builder for git commands
    /// CZ: Git bash builder pro git příkazy
    /// </summary>
    public GitBashBuilder GitBashBuilder { get; set; }

    /// <summary>
    /// EN: Arguments for git push command
    /// CZ: Argumenty pro git push příkaz
    /// </summary>
    public string PushArgs { get; set; }

    /// <summary>
    /// EN: Commit message
    /// CZ: Commit zpráva
    /// </summary>
    public string CommitMessage { get; set; }

    /// <summary>
    /// EN: Push solutions data
    /// CZ: Data pro push solutions
    /// </summary>
    public PushSolutionsData PushSolutionsData { get; set; }

    /// <summary>
    /// EN: Git status builder
    /// CZ: Builder pro git status
    /// </summary>
    public GitBashBuilder GitStatus { get; set; }
}
