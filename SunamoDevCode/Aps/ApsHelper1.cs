namespace SunamoDevCode.Aps;

public partial class ApsHelper : ApsPluginStatic
{
    public
#if ASYNC
    async Task
#else
    void
#endif
    CheckForPushInThread(object asyncPushSolutionsObject, Func<List<string>, Task<List<List<string>>>> psInvoke, string eVs, string pathGetMessagesFromGitOutput)
    {
        AsyncPushSolutions asyncPushSolutions = (AsyncPushSolutions)asyncPushSolutionsObject;
        ThisApp.Appeal("foldersWithSolutions before " + asyncPushSolutions.foldersWithSolutions.Count);
        foreach (var sln in asyncPushSolutions.foldersWithSolutions)
        {
#if DEBUG
            //DebugLogger.Instance.WriteLine("Push: " + sln.nameSolution);
#endif
            if (
#if ASYNC
    await
#endif
            GitHelper.PushSolution(asyncPushSolutions.release, asyncPushSolutions.gitBashBuilder, asyncPushSolutions.pushArgs, asyncPushSolutions.commitMessage, sln.fullPathFolder, pushSolutionsData, asyncPushSolutions.gitStatus, psInvoke))
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

        var gitBashBuilderS = asyncPushSolutions.gitBashBuilder.ToString();
        //gitBashBuilderS = "abc";
        if (!string.IsNullOrWhiteSpace(gitBashBuilderS) || !asyncPushSolutions.release)
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

            var hasSucceeded = true;
            if (push.HasValue)
            {
                if (push.Value)
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
                else
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
            }
            else
            {
                await EnterOutputOfPowershellGit_ChangeDialogResult(hasSucceeded, null, eVs, pathGetMessagesFromGitOutput);
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
        var sections = CA.Split(lines, determining);
        List<string> result = new List<string>();
        List<List<string>> restoredSections = new List<List<string>>();
        List<List<string>> failedSections = new List<List<string>>();
        foreach (var section in sections)
        {
            for (int i = section.Count - 1; i >= 0; i--)
            {
                if (section[i].StartsWith(restored))
                {
                    restoredSections.Add(section);
                }
                else if (section[i].StartsWith(failedToRestore))
                {
                    failedSections.Add(section);
                }
                else
                {
                    ThrowEx.NotImplementedCase(section[i]);
                }

                break;
            }
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var failedSection in failedSections)
        {
            stringBuilder.AppendLine(SHJoin.JoinNL(failedSection));
        }

        ClipboardService.SetText(stringBuilder.ToString());
        return null;
    }

    /// <summary>
    /// Return empty collection if shouldProcessMessages = false
    /// </summary>
    /// <param name = "shouldProcessMessages">Whether to process and filter messages from git output</param>
    /// <param name = "pathGetMessagesFromGitOutput">Path to file containing git messages</param>
    /// <param name = "eVs">Environment variable for drive</param>

#if ASYNC
    async Task<IList<string>>
#else
    IList<string>
#endif
    GetMessagesFromGitOutput(bool? shouldProcessMessages, string pathGetMessagesFromGitOutput, string eVs)
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
        var badSolutions = GetMessagesFromGitOutput(shouldProcessMessages, ref lines, pushSolutionsData.checkForGit, eVs);
        return badSolutions;
    }

    public List<string> GetMessagesFromGitOutput(bool? shouldProcessMessages, ref List<string> lines, GitTypesOfMessages gitMessageTypes, string eVs)
    {
        List<string> badSolutions = new List<string>();
        if (BTS.GetValueOfNullable(shouldProcessMessages))
        {
            lines = CA.RemoveStringsEmptyTrimBefore(lines);
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                bool shouldAdd = false;
                if (gitMessageTypes.HasFlag(GitTypesOfMessages.fatal) && line.StartsWith("fatal: "))
                {
                    shouldAdd = true;
                }

                if (gitMessageTypes.HasFlag(GitTypesOfMessages.error) && line.StartsWith("error: "))
                {
                    shouldAdd = true;
                }

                if (shouldAdd)
                {
                    // -1 is in GetNameSolution
                    string nameSolution = GetNameSolution(lines, i, eVs);
                    string all = nameSolution + " " + line;
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
    private string GetNameSolution(List<string> lines, int lineIndex, string eVs)
    {
        lineIndex--;
        var drive = SH.FirstCharUpper(eVs);
        string promptPrefix = "PS " + drive;
        var promptPrefixLower = promptPrefix.ToLower();
        for (; lineIndex >= 0; lineIndex--)
        {
            var line = lines[lineIndex];
            // Must be lower - in powershell output is data:, here data:
            if (!line.ToLower().StartsWith(promptPrefixLower))
            {
                continue;
            }

            //PS data:\\Documents\\vs\\Code_Projects\\CodeLearnRoslyn> git
            string pathPart = line.Replace(promptPrefix, string.Empty);
            int promptEndIndex = pathPart.IndexOf('>');
            pathPart = pathPart.Substring(0, promptEndIndex);
            return pathPart;
        }

        return null;
    }

    public void EnterOutputOfPowershellGit_ChangeDialogResult(bool? shouldProcessMessages)
    {
        EnterOutputOfPowershellGit_ChangeDialogResult(shouldProcessMessages);
    }
}