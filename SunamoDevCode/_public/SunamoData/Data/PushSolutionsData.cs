namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
/// Configuration data for push solutions operations.
/// </summary>
public class PushSolutionsData
{
    /// <summary>
    /// Whether to perform merge and fetch before push.
    /// </summary>
    public bool mergeAndFetch = false;
    /// <summary>
    /// Whether to add .gitignore file.
    /// </summary>
    public bool addGitignore = false;
    /// <summary>
    /// If set, only push these specific solutions.
    /// </summary>
    public List<string>? onlyThese = null;
    /// <summary>
    /// Whether to process C# projects (null for default behavior).
    /// </summary>
    public bool? cs = null;

    /// <summary>
    /// Types of git messages to check for in output.
    /// </summary>
    public GitTypesOfMessages checkForGit = GitTypesOfMessages.error | GitTypesOfMessages.fatal;
    /// <summary>
    /// Sets the merge and fetch and gitignore options.
    /// </summary>
    /// <param name="mergeAndFetch">Whether to merge and fetch.</param>
    /// <param name="addGitignore">Whether to add .gitignore.</param>
    public void Set(bool mergeAndFetch, bool addGitignore = false)
    {
        this.mergeAndFetch = mergeAndFetch;
        this.addGitignore = addGitignore;
    }
}