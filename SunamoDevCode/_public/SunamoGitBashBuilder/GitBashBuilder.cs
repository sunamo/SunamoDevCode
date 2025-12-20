// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._public.SunamoGitBashBuilder;
public partial class GitBashBuilder : IGitBashBuilder
{
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }

    /// <summary>
    /// 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    /// 
    /// </summary>
    /// <param name = "args"></param>
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

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

    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }

        AppendLine();
    }

    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }

    public void Init()
    {
        Git("init");
        AppendLine();
    }

    public void Add(string v)
    {
        Git("add");
        Append(v);
        AppendLine();
    }

    public void Config(string v)
    {
        Git("config");
        Append(v);
        AppendLine();
    }

    public void Clean(string v)
    {
        Git("clean");
        Arg(v);
        AppendLine();
    }

    public static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }

    private void Git(string remainCommand)
    {
        if (remainCommand[remainCommand.Length - 1] != ' ')
        {
            remainCommand += " ";
        }

        sb.Append((GitForDebug ? "GitForDebug " : "git ") + remainCommand);
    }

    private void Arg(string v)
    {
        Append("-" + v);
    }

    public void Comment(string s)
    {
        AppendLine("#" + s);
    }

    public void MultilineComment(List<string> s)
    {
        AppendLine("<#");
        foreach (var item in s)
        {
            AppendLine(item);
        }

        AppendLine("#>");
    }

    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }

    public void Status()
    {
        Git("status");
        AppendLine();
    }

    public void Fetch(string s = "")
    {
        Git("fetch " + s);
        AppendLine();
    }

    public void Merge(string v)
    {
        Git("merge " + v);
        AppendLine();
    }

    public void AddNewRemote(string s)
    {
        Remote("add origin " + s);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }

    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }

    private static Type type = typeof(GitBashBuilder);
    public TextBuilderDC sb = null;
    public GitBashBuilder(TextBuilderDC sb)
    {
        this.sb = sb;
    }

    public bool GitForDebug = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }

    public static string CreateGitAddForFiles(StringBuilder sb, List<string> linesFiles)
    {
        return CreateGitCommandForFiles("add", sb, linesFiles);
    }

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