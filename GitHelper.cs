namespace SunamoDevCode;

/// <summary>
///     Must be in Win because use powershell
///     In shared cannot because win derife from shared.
///     If I have abstract layer for shared, then yes
/// </summary>
public class GitHelper
{
    //https://5vzabodsgurc2qeufb56xlzu7hggrf2qyy3fatubdadxb5oto53q@radekjancik.visualstudio.com/AllProjectsSearch.Cmd.Parallel/_git/AllProjectsSearch.Cmd.Parallel

    /// <summary>
    ///     https://radekjancik.visualstudio.com/_git/AllProjectsSearch
    /// </summary>
    private const string b1 =
        "https://5vzabodsgurc2qeufb56xlzu7hggrf2qyy3fatubdadxb5oto53q@radekjancik.visualstudio.com/_git/";

    /// <summary>
    ///     https://radekjancik@dev.azure.com/radekjancik/CodeProjects_Bobril/_git/CodeProjects_Bobril
    /// </summary>
    private const string b2_s =
        "https://5vzabodsgurc2qeufb56xlzu7hggrf2qyy3fatubdadxb5oto53q@radekjancik.visualstudio.com/";

    private const string b2_e = "/_git/";

    /// <summary>
    ///     https://radekjancik.visualstudio.com/AllProjectsSearch.ToNet5/_git/AllProjectsSearch.ToNet5
    /// </summary>
    private const string b3_s = "https://radekjancik.visualstudio.com/";

    /// <summary>
    ///     https://github.com/sunamo/sunamo.git
    /// </summary>
    private const string b4 = "https://github.com/sunamo/";

    /// <summary>
    ///     https://dev.azure.com/radekjancik/_git/sunamo.webWithoutDep
    /// </summary>
    private const string b5 = "https://dev.azure.com/radekjancik/_git/";

    // https://bitbucket.org/sunamo/1gp-gopay-master
    private const string b6 = @"https://bitbucket.org/sunamo/";

    public static string PowershellForPull(List<string> folders)
    {
        var gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        foreach (var item in folders)
        {
            gitBashBuilder.Cd(item);
            gitBashBuilder.Pull();
        }

        var pullAllResult = gitBashBuilder.ToString();
        return pullAllResult;
    }

    public static
#if ASYNC
        async Task<bool>
#else
    bool
#endif
        PushSolution(bool release, GitBashBuilder gitBashBuilder, string pushArgs, string commitMessage,
            string fullPathFolder, PushSolutionsData pushSolutionsData, GitBashBuilder gitStatus,
            Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        // 1. better solution is commented only getting files
        var countFiles = 0;
        if (release) countFiles = Directory.GetFiles(fullPathFolder, "*.*", SearchOption.AllDirectories).Length;

        if (fullPathFolder.Contains("SunamoCzAdmin"))
        {
        }

        if (countFiles > 0)
        {
            gitStatus.Clear();
            gitStatus.Cd(fullPathFolder);
            gitStatus.Status();

            var result = new List<List<string>>(new List<List<string>>([new List<string>(), new List<string>()]));
            // 2. or powershell
            if (release)
                result =
#if ASYNC
                    await
#endif
                        psInvoke(gitStatus.Commands);

            var statusOutput = result[1];
            // If solution has changes
            var hasChanges = statusOutput.Where(d => d.Contains("nothing to commit")).Count() == 0;
            if (!hasChanges)
                foreach (var lineStatus in statusOutput)
                {
                    var statusLine = lineStatus.Trim();
                    if (statusOutput.Contains("modified:"))
                        if (statusOutput.Contains(".gitignore"))
                        {
                            hasChanges = true;
                            break;
                        }
                }

            if (!hasChanges)
                foreach (var lineStatus in statusOutput)
                {
                    //
                    var statusLine = lineStatus.Trim();
                    if (statusOutput.Contains("but the upstream is gone"))
                    {
                        hasChanges = true;
                        break;
                    }
                }

            // or/and is a git repository
            var isGitRepository =
                statusOutput.Where(d => d.Contains("not a git repository")).Count() ==
                0; // CA.ReturnWhichContains(, ).Count == 0;
            if (hasChanges && isGitRepository)
            {
                gitBashBuilder.Cd(fullPathFolder);

                if (pushSolutionsData.mergeAndFetch) gitBashBuilder.Fetch();

                gitBashBuilder.Add("*");

                gitBashBuilder.Commit(false, commitMessage);

                if (pushSolutionsData.mergeAndFetch) gitBashBuilder.Merge("--allow-unrelated-histories");

                if (pushSolutionsData.addGitignore) gitBashBuilder.Add(".gitignore");

                gitBashBuilder.Push(pushArgs);

                gitBashBuilder.AppendLine();

                // Dont run, better is paste into powershell due to checking errors
                //var git = gitBashBuilder.Commands;
                //PowershellRunner.ci.Invoke(git);

                return true;
            }
        }

        return false;
    }

    public static string NameOfRepoFromOriginUri(string s)
    {
        s = HttpUtility.UrlDecode(s);
        if (s.StartsWith(b1))
        {
            s = s.Replace(b1, string.Empty);
        }
        else if (s.StartsWith(b2_s))
        {
            s = SH.GetTextBetweenSimple(s, b2_s, b2_e);
        }
        else if (s.StartsWith(b3_s))
        {
            s = SH.GetTextBetweenSimple(s, b3_s, b2_e);
        }
        else if (s.StartsWith(b4))
        {
            s = s.Replace(b4, string.Empty);
            s = SHTrim.TrimEnd(s, ".git");
        }
        else if (s.StartsWith(b5))
        {
            s = s.Replace(b5, string.Empty);
        }
        else if (s.StartsWith(b6))
        {
            s = s.Replace(b6, string.Empty);
        }

        if (s.Contains("/")) throw new Exception(s + " - name of repo contains still /");

        return s;
    }
}