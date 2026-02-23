namespace SunamoDevCode._public.SunamoGitBashBuilder;

/// <summary>
/// Builder for constructing Git bash commands as text scripts.
/// </summary>
public partial class GitBashBuilder : IGitBashBuilder
{
    /// <summary>
    /// Appends a git pull command.
    /// </summary>
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }

    /// <summary>
    /// 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    /// </summary>
    /// <param name="args">Arguments for the clone command</param>
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

    /// <summary>
    /// Appends a git commit command with optional flags.
    /// </summary>
    /// <param name="addAllUntrackedFiles">Whether to add all untracked files (-a flag)</param>
    /// <param name="commitMessage">The commit message</param>
    public void Commit(bool addAllUntrackedFiles, string commitMessage)
    {
        ThrowEx.IsNullOrWhitespace("commitMessage", commitMessage);
        Git("commit ");
        if (addAllUntrackedFiles)
        {
            Append("-a");
        }

        if (!string.IsNullOrWhiteSpace(commitMessage))
        {
            Append("-m " + SH.WrapWithQm(commitMessage));
        }

        AppendLine();
    }

    /// <summary>
    /// Appends a git push command with optional force flag.
    /// </summary>
    /// <param name="force">Whether to force push</param>
    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }

        AppendLine();
    }

    /// <summary>
    /// Appends a git push command with additional arguments.
    /// </summary>
    /// <param name="arg">Additional push arguments</param>
    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Appends a git init command.
    /// </summary>
    public void Init()
    {
        Git("init");
        AppendLine();
    }

    /// <summary>
    /// Appends a git add command for the specified file.
    /// </summary>
    /// <param name="filePath">File path to add to staging</param>
    public void Add(string filePath)
    {
        Git("add");
        Append(filePath);
        AppendLine();
    }

    /// <summary>
    /// Appends a git config command.
    /// </summary>
    /// <param name="configOption">Configuration option and value</param>
    public void Config(string configOption)
    {
        Git("config");
        Append(configOption);
        AppendLine();
    }

    /// <summary>
    /// Appends a git clean command.
    /// </summary>
    /// <param name="cleanOptions">Options for the clean command</param>
    public void Clean(string cleanOptions)
    {
        Git("clean");
        Arg(cleanOptions);
        AppendLine();
    }

    /// <summary>
    /// Creates a git command string using a static StringBuilder.
    /// </summary>
    /// <param name="sb">StringBuilder to append to</param>
    /// <param name="remainCommand">Git subcommand and arguments</param>
    /// <returns>The complete git command string</returns>
    public static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }

    private void Git(string remainingCommand)
    {
        if (remainingCommand[remainingCommand.Length - 1] != ' ')
        {
            remainingCommand += " ";
        }

        StringBuilder.Append((GitForDebug ? "GitForDebug " : "git ") + remainingCommand);
    }

    private void Arg(string argument)
    {
        Append("-" + argument);
    }

    /// <summary>
    /// Appends a single-line comment to the script.
    /// </summary>
    /// <param name="commentText">Comment text</param>
    public void Comment(string commentText)
    {
        AppendLine("#" + commentText);
    }

    /// <summary>
    /// Appends a multiline comment block to the script.
    /// </summary>
    /// <param name="lines">Lines of the comment</param>
    public void MultilineComment(List<string> lines)
    {
        AppendLine("<#");
        foreach (var item in lines)
        {
            AppendLine(item);
        }

        AppendLine("#>");
    }

    /// <summary>
    /// Appends a git remote command.
    /// </summary>
    /// <param name="arg">Remote subcommand and arguments</param>
    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Appends a git status command.
    /// </summary>
    public void Status()
    {
        Git("status");
        AppendLine();
    }

    /// <summary>
    /// Appends a git fetch command.
    /// </summary>
    /// <param name="remoteName">Optional remote name to fetch from</param>
    public void Fetch(string remoteName = "")
    {
        Git("fetch " + remoteName);
        AppendLine();
    }

    /// <summary>
    /// Appends a git merge command.
    /// </summary>
    /// <param name="branchName">Branch name to merge</param>
    public void Merge(string branchName)
    {
        Git("merge " + branchName);
        AppendLine();
    }

    /// <summary>
    /// Appends a sequence of commands to add a new remote, fetch, and checkout master.
    /// </summary>
    /// <param name="remoteUrl">URL of the remote repository</param>
    public void AddNewRemote(string remoteUrl)
    {
        Remote("add origin " + remoteUrl);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }

    /// <summary>
    /// Appends a git checkout command.
    /// </summary>
    /// <param name="arg">Checkout arguments</param>
    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }

    private static Type type = typeof(GitBashBuilder);
    /// <summary>
    /// Gets or sets the text builder used for constructing the command script.
    /// </summary>
    public TextBuilderDC StringBuilder { get; set; } = null!;
    /// <summary>
    /// Initializes a new instance of GitBashBuilder with the specified text builder.
    /// </summary>
    /// <param name="stringBuilder">Text builder for constructing commands</param>
    public GitBashBuilder(TextBuilderDC stringBuilder)
    {
        this.StringBuilder = stringBuilder;
    }

    /// <summary>
    /// Gets or sets whether to use "GitForDebug" prefix instead of "git".
    /// </summary>
    public bool GitForDebug { get; set; } = false;
    /// <summary>
    /// Gets all commands as a list of lines.
    /// </summary>
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }

    /// <summary>
    /// Creates git add commands for a list of files.
    /// </summary>
    /// <param name="sb">StringBuilder to build into</param>
    /// <param name="linesFiles">List of file paths to add</param>
    /// <returns>Combined git add command string</returns>
    public static string CreateGitAddForFiles(StringBuilder sb, List<string> linesFiles)
    {
        return CreateGitCommandForFiles("add", sb, linesFiles);
    }

    /// <summary>
    /// Generates a git command for a list of files in a solution, with extension filtering.
    /// </summary>
    /// <param name="solution">Solution path</param>
    /// <param name="linesFiles">List of file paths</param>
    /// <param name="anyError">Output flag indicating if any error occurred</param>
    /// <param name="searchOnlyWithExtension">File extension filter</param>
    /// <param name="command">Git command to generate</param>
    /// <param name="basePathIfA2SolutionsWontExistsOnFilesystem">Fallback base path</param>
    /// <returns>Generated git command string</returns>
    public static string GenerateCommandForGit( /*object tlb,*/string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string command, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat( /*tlb,*/solution, linesFiles, out anyError, searchOnlyWithExtension, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null || filesToCommit.Count == 0)
        {
            return "";
        }

        string result = GitBashBuilder.CreateGitCommandForFiles(command, new StringBuilder(), filesToCommit);
        return result;
    }

    /// <summary>
    /// Creates a git checkout command with extension filtering for files in a folder.
    /// </summary>
    /// <param name="folder">Folder path</param>
    /// <param name="typedExt">File extension to filter by</param>
    /// <param name="files">List of file paths</param>
    /// <param name="basePathIfA2SolutionsWontExistsOnFilesystem">Fallback base path</param>
    /// <param name="ci">Text builder instance</param>
    /// <returns>Generated checkout command string</returns>
    public static string CheckoutWithExtension(string folder, string typedExt, List<string> files, string basePathIfA2SolutionsWontExistsOnFilesystem, TextBuilderDC ci)
    {
        ThrowEx.IsNull("typedExt", typedExt);
        GitBashBuilder bashBuilder = new GitBashBuilder(ci);
        bool anyError = false;
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat( /*null,*/folder, files, out anyError, typedExt, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null)
        {
        }

        string result = GitBashBuilder.GenerateCommandForGit( /*null,*/folder, files, out anyError, typedExt, "checkout", basePathIfA2SolutionsWontExistsOnFilesystem);
        return result;
    }
}