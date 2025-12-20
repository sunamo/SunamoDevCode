// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps;
public partial class ApsHelper : ApsPluginStatic
{
    public 
#if ASYNC
    async Task
#else
    void 
#endif
    CheckForPushInThread(object o, Func<List<string>, Task<List<List<string>>>> psInvoke, string eVs, string pathGetMessagesFromGitOutput)
    {
        AsyncPushSolutions a = (AsyncPushSolutions)o;
        ThisApp.Appeal("foldersWithSolutions before " + a.foldersWithSolutions.Count);
        foreach (var sln in a.foldersWithSolutions)
        {
#if DEBUG
            //DebugLogger.Instance.WriteLine("Push: " + sln.nameSolution);
#endif
            if (
#if ASYNC
    await
#endif
            GitHelper.PushSolution(a.release, a.gitBashBuilder, a.pushArgs, a.commitMessage, sln.fullPathFolder, pushSolutionsData, a.gitStatus, psInvoke))
            {
                var fn = FS.GetFileName(sln.fullPathFolder);
                if (pushSolutionsData.onlyThese == VpsHelperDevCode.listVpsNew)
                {
                    if (!pushSolutionsData.onlyThese.Contains(sln.nameSolution))
                    {
                        continue;
                    }
                }

                gitPullVps.Cd( /*Path.Combine(VpsHelperDevCode.path,*/fn);
                gitPullVps.Pull();
                ThisApp.Info(fn);
            }
        }

        var gitBashBuilderS = a.gitBashBuilder.ToString();
        //gitBashBuilderS = "abc";
        if (!string.IsNullOrWhiteSpace(gitBashBuilderS) || !a.release)
        {
            //gitBashBuilderS = gitBashBuilder.ToString();
            //gitBashBuilderS = TF.ReadAllText(@"D:\Desktop\gitOutput.txt");
            ClipboardService.SetText(gitBashBuilderS);
            //LoginUc enterOutputOfPowershellGit = new LoginUc();
            pushSolutionsData.checkForGit = GitTypesOfMessages.fatal | GitTypesOfMessages.error;
            await ShowWindowForEnterOutputOfPowershell(gitBashBuilderS, true, eVs, pathGetMessagesFromGitOutput);
        //uc.Accept(gitBashBuilderS);
        }
        else
        {
            ThisApp.Appeal("gitBashBuilderS is null");
        }
    }

    public async Task ShowWindowForEnterOutputOfPowershell(string gitBashBuilder, bool? push, string eVs, string pathGetMessagesFromGitOutput)
    {
        if (cmd)
        {
            typed = SNumConsts.mTwo;
            while (typed == SNumConsts.mTwo)
            {
                typed = CL.UserMustTypeMultiLine("Output of powershell. Enter -2 for copy again push commands", SNumConsts.mTwo);
                if (typed == SNumConsts.mTwo)
                {
                    ClipboardService.SetText(gitBashBuilder);
                    break;
                }
            }

            var builder = true;
            if (push.HasValue)
            {
                if (push.Value)
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(builder, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
                else
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(builder, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
            }
            else
            {
                await EnterOutputOfPowershellGit_ChangeDialogResult(builder, null, eVs, pathGetMessagesFromGitOutput);
            }
        }
        else
        {
            MoveToApsWpf();
        //enterOneValueUC = new EnterOneValueUC("output of powershell");
        //enterOneValueUC.txtEnteredText.AcceptsReturn = true;
        //enterOneValueUC.txtEnteredText.Text = string.Empty;
        //enterOneValueUC.txtEnteredText.TextWrapping = TextWrapping.Wrap;
        //enterOneValueUC.txtEnteredText.Height = 400;
        //enterOneValueUC.txtEnteredText.Width = 400;
        //SuMenuItem miCopyToClipboard = new SuMenuItem();
        //miCopyToClipboard.Header = "Copy git commands to clipboard";
        //miCopyToClipboard.Click += MiCopyToClipboard_Click;
        //miCopyToClipboard.Tag = gitBashBuilder;
        //enterOneValueUC.suSuMenuItems.Add(miCopyToClipboard);
        //enterOutputOfPowershellGitWindow = new WindowWithUserControl(enterOneValueUC, ResizeMode.CanResize, false, "enterOutputOfPowershellGit");
        //enterOneValueUC.ChangeDialogResult += EnterOutputOfPowershellGit_ChangeDialogResult;
        //enterOutputOfPowershellGitWindow.ShowDialog();
        }
    }

    const string determining = "  Determining projects to restore...";
    const string call = "Call: ";
    const string restored = "log  : Restored";
    const string failedToRestore = "log  : Failed to restore";
    public IList<string> GetMessagesFromGitOutput(List<string> lines)
    {
        lines.RemoveAll(data => data.StartsWith(call));
        //var text = SHJoin.JoinNL(lines);
        var parameter = CA.Split(lines, determining);
        List<string> result = new List<string>();
        List<List<string>> restored2 = new List<List<string>>();
        List<List<string>> failedToRestore2 = new List<List<string>>();
        foreach (var item in parameter)
        {
            for (int i = item.Count - 1; i >= 0; i--)
            {
                if (item[i].StartsWith(restored))
                {
                    restored2.Add(item);
                }
                else if (item[i].StartsWith(failedToRestore))
                {
                    failedToRestore2.Add(item);
                }
                else
                {
                    ThrowEx.NotImplementedCase(item[i]);
                }

                break;
            }
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in failedToRestore2)
        {
            stringBuilder.AppendLine(SHJoin.JoinNL(item));
        }

        ClipboardService.SetText(stringBuilder.ToString());
        return null;
    }

    /// <summary>
    /// Return empty collection if A1 = false
    /// </summary>
    /// <param name = "b"></param>
    /// <param name = "git"></param>
    
#if ASYNC
    async Task<IList<string>>
#else
    IList<string> 
#endif
    GetMessagesFromGitOutput(bool? builder, string pathGetMessagesFromGitOutput, string eVs)
    {
        if (cmd)
        {
            //#if !DEBUG
            if (typed == null)
            {
                typed = CL.UserMustTypeMultiLine("git output. Enter -1 for loading default from file");
            }

            //#endif
            if (string.IsNullOrEmpty(typed))
            {
                //var pathGetMessagesFromGitOutput = @"D:\_Test\AllProjectsSearch\AllProjectsSearch\SearchInSolutionsUC\GetMessagesFromGitOutput.txt";
                FS.CreateUpfoldersPsysicallyUnlessThere(pathGetMessagesFromGitOutput);
                typed = 
#if ASYNC
    await
#endif
                TF.ReadAllText(pathGetMessagesFromGitOutput);
            }
        }
        else
        {
            MoveToApsWpf();
        //typed = enterOutputOfPowershellGit.txtEnteredText.Text;
        }

        var lines = SHGetLines.GetLines(typed);
        //const string storingIndexDone = "remote: Storing index... done";
        //const string m2m = "master -> master";
        //for (int i = lines.Count - 1; i >= 0; i--)
        //{
        //    if (lines[i].Contains(m2m))
        //    {
        //        //lines.Insert(i, "fatal: femme fatale");
        //        break;
        //    }
        //}
        var badSolutions = GetMessagesFromGitOutput(builder, ref lines, pushSolutionsData.checkForGit, eVs);
        return badSolutions;
    }

    public List<string> GetMessagesFromGitOutput(bool? builder, ref List<string> lines, GitTypesOfMessages git, string eVs)
    {
        List<string> badSolutions = new List<string>();
        if (BTS.GetValueOfNullable(builder))
        {
            lines = CA.RemoveStringsEmptyTrimBefore(lines);
            for (int i = 0; i < lines.Count; i++)
            {
                string item = lines[i];
                bool add = false;
                if (git.HasFlag(GitTypesOfMessages.fatal) && item.StartsWith("fatal: "))
                {
                    add = true;
                }

                if (git.HasFlag(GitTypesOfMessages.error) && item.StartsWith("error: "))
                {
                    add = true;
                }

                if (add)
                {
                    // -1 is in GetNameSolution
                    string nameSolution = GetNameSolution(lines, i, eVs);
                    string all = nameSolution + " " + item;
                    badSolutions.Add(all);
                }
            }

            ThisApp.Success("Solutions with selected types messages were copied to clipboard.");
        }

        badSolutions = badSolutions.Distinct().ToList();
        return badSolutions;
    }

    /// <summary>
    /// Will decrement A2
    /// Return relative paths like Projects\_Uap\CreateW10AppGraphics
    /// </summary>
    /// <param name = "lines"></param>
    /// <param name = "i"></param>
    private string GetNameSolution(List<string> lines, int i, string eVs)
    {
        i--;
        var drive = SH.FirstCharUpper(eVs);
        string start = "PS " + drive;
        var startToLower = start.ToLower();
        for (; i >= 0; i--)
        {
            var line = lines[i];
            // Must be lower - in powershell output is data:, here data:
            if (!line.ToLower().StartsWith(startToLower))
            {
                continue;
            }

            //PS data:\\Documents\\vs\\Code_Projects\\CodeLearnRoslyn> git
            string v = line.Replace(start, string.Empty);
            int dex = v.IndexOf('>');
            v = v.Substring(0, dex);
            return v;
        }

        return null;
    }

    public void EnterOutputOfPowershellGit_ChangeDialogResult(bool? builder)
    {
        EnterOutputOfPowershellGit_ChangeDialogResult(builder);
    }
}