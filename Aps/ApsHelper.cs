namespace SunamoDevCode.Aps;

public class ApsHelper : ApsPluginStatic
{
    public static ApsHelper ci = new ApsHelper();
    public Type type = typeof(ApsHelper);
    /// <summary>
    /// Never create new instance, just call method
    /// </summary>
    public PushSolutionsData pushSolutionsData = new PushSolutionsData();
    string typed = null;
    GitBashBuilder gitPullVps = new GitBashBuilder(new TextBuilderDC());
    GitBashBuilder gitPushVps = new GitBashBuilder(new TextBuilderDC());
    /// <summary>
    /// Dont use XmlDocumentsCache
    /// </summary>
    /// <returns></returns>
    public static Tuple<List<string>, List<string>> WebAndNonWebProjects(ILogger logger, bool withCsprojs = true)
    {
        List<string> webProjects = new List<string>();
        List<string> notWebProjects = new List<string>();
        foreach (var item in FoldersWithSolutions.fwss)
        {
            var s = item.Solutions(RepositoryLocal.Vs17);
            foreach (var sln in s)
            {
#if DEBUG
                if (sln.fullPathFolder.Contains(@"\sunamo.web\"))
                {
                }
#endif
                SolutionFolder.GetCsprojs(logger, sln);
                foreach (var csp in sln.projectsGetCsprojs)
                {
#if DEBUG
                    if (csp.Contains("webforms"))
                    {
                    }
#endif
                    var csp2 = withCsprojs ? csp : FS.GetDirectoryName(csp);
                    if (
 IsWeb(csp))
                    {
                        webProjects.Add(csp2);
                    }
                    else
                    {
                        notWebProjects.Add(csp2);
                    }
                }
            }
        }
#if DEBUG
        var dx = webProjects.Where(d => d.Contains("desktop"));
#endif
        return new Tuple<List<string>, List<string>>(webProjects, notWebProjects);
    }
    public static
bool
 IsWeb(string csp)
    {
        //\WebApplication
        // \webelieve.cz
        return CA.ContainsAnyFromElementBool(csp,
 AllProjectsSearchSettings.DontReplaceReferencesIn);
    }
    public async Task PushSolutionsContinuouslyWindow_ChangeDialogResult(bool? b, Func<List<string>, Task<List<List<string>>>> psInvoke, string eVs, string pathGetMessagesFromGitOutput)
    {
        ThisApp.Appeal("PushSolutionsContinuouslyWindow_ChangeDialogResult");
        //string gitBashBuilderS = null;
        bool release = true;
        gitPullVps.Clear();
        GitBashBuilder gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        if (BTS.GetValueOfNullable(b))
        {
            string pushArgs = "";
            // instead of this add print errors. with force I can lose my data
            //pushArgs = " -f";
            string commitMessage = null;
            if (cmd)
            {
                commitMessage = typed;
            }
            else
            {
                MoveToApsWpf();
                //commitMessage = pushSolutionsContinuouslyUC.txt1.Text;
            }
            GitBashBuilder gitStatus = new GitBashBuilder(new TextBuilderDC());
            bool push = true;
            List<SolutionFolder> foldersWithSolutions = new List<SolutionFolder>();
            var skipTheseGit = ci.SkipTheseGit();
            if (pushSolutionsData.onlyThese != null)
            {
                ThisApp.Appeal("pushSolutionsData.onlyThese " + pushSolutionsData.onlyThese.Count);
            }
            else
            {
                ThisApp.Appeal("pushSolutionsData.onlyThese is null");
            }
            if (skipTheseGit != null)
            {
                ThisApp.Appeal("pushSolutionsData.onlyThese " + skipTheseGit.Count);
            }
            else
            {
                ThisApp.Appeal("pushSolutionsData.onlyThese is null");
            }
            bool isCs = pushSolutionsData.cs.HasValue;
            bool cs = false;
            foreach (var item in fwss)
            {
                // Its only push, no delete or change file so A2 can be true here
                var slns = item.Solutions(usedRepository, true, skipTheseGit);
                foreach (var sln in slns)
                {
#if DEBUG
                    //if (sln.nameSolution != "AllProjectsSearch.Cmd")
                    //{
                    //    continue;
                    //}
#endif
                    if (isCs)
                    {
                        #region MyRegion
                        //cs = sln.fullPathFolder.Contains(@"\Projects\");
                        //if (cs != pushSolutionsData.cs.Value)
                        //{
                        //    continue;
                        //}
                        //if (cs)
                        //{
                        //    if (sln.fullPathFolder.Contains("_"))
                        //    {
                        //        continue;
                        //    }
                        //}
                        //if (sln.nameSolution != "sunamo64")
                        //{
                        //    continue;
                        //}
                        #endregion
                    }
                    // Include also github, solutions which isnt mine return 403
                    //if (!sln.InVsFolder)
                    //{
                    push = true;
                    if (pushSolutionsData.onlyThese != null)
                    {
                        if (!pushSolutionsData.onlyThese.Contains(sln.nameSolution))
                        {
                            push = false;
                        }
                    }
                    if (push)
                    {
                        foldersWithSolutions.Add(sln);
                    }
                    else
                    {
#if DEBUG
                        //DebugLogger.Instance.WriteLine("Dont push: " + sln.nameSolution);
#endif
                    }
                }
            }
            AsyncPushSolutions al = new AsyncPushSolutions { foldersWithSolutions = foldersWithSolutions, release = release, gitBashBuilder = gitBashBuilder, pushArgs = pushArgs, commitMessage = commitMessage, pushSolutionsData = pushSolutionsData, gitStatus = gitStatus };
            if (cmd)
            {
#if ASYNC
                await
#endif
                CheckForPushInThread(al, psInvoke, eVs, pathGetMessagesFromGitOutput);
            }
            else
            {
                MoveToApsWpf();
                #region New
                //#if ASYNC
                //                await
                //#endif
                //                CheckForPushInThread(al);
                #endregion
                #region Old
                //ThisApp.Appeal( "ParameterizedThreadStart before");
                //ParameterizedThreadStart p = new ParameterizedThreadStart(CheckForPushInThread);
                //Thread t = new Thread(p) { IsBackground = true };
                //t.SetApartmentState(ApartmentState.STA);
                //t.Start(al);
                #endregion
            }
        }
        else
        {
            MoveToApsWpf();
            //windowWithUserControl.Close();
        }
        ThisApp.Appeal("PushSolutionsContinuouslyWindow_ChangeDialogResult end");
    }
    public List<string> AllProjects(ILogger logger, RepositoryLocal vs17, WebNonWeb webNonWeb, bool withCsprojs = true)
    {
        if (vs17 != RepositoryLocal.Vs17)
        {
            ThrowEx.NotImplementedCase(vs17);
        }
        var t = WebAndNonWebProjects(logger, withCsprojs);
        if (webNonWeb == WebNonWeb.NonWeb || webNonWeb == WebNonWeb.NonWebCsproj)
        {
            return t.Item2;
        }
        else if (webNonWeb == WebNonWeb.Web || webNonWeb == WebNonWeb.WebCsproj)
        {
            return t.Item1;
        }
        else if (webNonWeb == WebNonWeb.Both)
        {
            List<string> ls = new List<string>(t.Item1.Count + t.Item2.Count);
            ls.AddRange(t.Item1);
            ls.AddRange(t.Item2);
            return ls;
        }
        ThrowEx.NotImplementedCase(webNonWeb);
        return null;
    }
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
                gitPullVps.Cd(/*Path.Combine(VpsHelperDevCode.path,*/ fn);
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
            var b = true;
            if (push.HasValue)
            {
                if (push.Value)
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(b, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
                else
                {
                    await EnterOutputOfPowershellGit_ChangeDialogResult(b, gitPushVps, eVs, pathGetMessagesFromGitOutput);
                }
            }
            else
            {
                await EnterOutputOfPowershellGit_ChangeDialogResult(b, null, eVs, pathGetMessagesFromGitOutput);
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
    public IList<string> GetMessagesFromGitOutput(List<string> ls)
    {
        ls.RemoveAll(d => d.StartsWith(call));
        //var s = SHJoin.JoinNL(ls);
        var p = CA.Split(ls, determining);
        List<string> result = new List<string>();
        List<List<string>> restored2 = new List<List<string>>();
        List<List<string>> failedToRestore2 = new List<List<string>>();
        foreach (var item in p)
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
        StringBuilder sb = new StringBuilder();
        foreach (var item in failedToRestore2)
        {
            sb.AppendLine(SHJoin.JoinNL(item));
        }
        ClipboardService.SetText(sb.ToString());
        return null;
    }
    /// <summary>
    /// Return empty collection if A1 = false
    /// </summary>
    /// <param name="b"></param>
    /// <param name="git"></param>
#if ASYNC
    async Task<IList<string>>
#else
    IList<string>
#endif
GetMessagesFromGitOutput(bool? b, string pathGetMessagesFromGitOutput, string eVs)
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
        var badSolutions = GetMessagesFromGitOutput(b, ref lines, pushSolutionsData.checkForGit, eVs);
        return badSolutions;
    }
    public List<string> GetMessagesFromGitOutput(bool? b, ref List<string> lines, GitTypesOfMessages git, string eVs)
    {
        List<string> badSolutions = new List<string>();
        if (BTS.GetValueOfNullable(b))
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
    /// <param name="lines"></param>
    /// <param name="i"></param>
    private string GetNameSolution(List<string> lines, int i, string eVs)
    {
        i--;
        var drive = SH.FirstCharUpper(eVs);
        string start = "PS " + drive;
        var startToLower = start.ToLower();
        for (; i >= 0; i--)
        {
            var line = lines[i];
            // Must be lower - in powershell output is D:, here d:
            if (!line.ToLower().StartsWith(startToLower))
            {
                continue;
            }
            //PS D:\\Documents\\vs\\Code_Projects\\CodeLearnRoslyn> git
            string v = line.Replace(start, string.Empty);
            int dex = v.IndexOf('>');
            v = v.Substring(0, dex);
            return v;
        }
        return null;
    }
    public void EnterOutputOfPowershellGit_ChangeDialogResult(bool? b)
    {
        EnterOutputOfPowershellGit_ChangeDialogResult(b);
    }
    public
#if ASYNC
    async Task
#else
    void
#endif
 EnterOutputOfPowershellGit_ChangeDialogResult(bool? b, GitBashBuilder sb, string eVs, string pathGetMessagesFromGitOutput)
    {
        var messages = (
#if ASYNC
    await
#endif
 GetMessagesFromGitOutput(b, pathGetMessagesFromGitOutput, eVs)).ToList();
        CA.Prepend("#", messages);
        var mess = new StringBuilder();
        if (messages.Count() == 0)
        {
            mess.AppendLine("#no errors");
        }
        else
        {
            foreach (var item in messages)
            {
                mess.AppendLine(item);
            }
        }
        var ts = string.Empty;
        if (sb != null)
        {
            ts = sb.ToString();
        }
        if (pushSolutionsData.onlyThese != VpsHelperDevCode.listVpsNew)
        {
            string vpsPath = eVs;
            //string localPath = SH.PostfixIfNotEmpty(DefaultPaths.eVsProjects, "\\");
            var l = SHGetLines.GetLines(ts);
            CA.Trim(l);
            for (int i = 0; i < l.Count; i++)
            {
                var li = l[i];
                if (li.StartsWith("cd "))
                {
                    var p = SHSplit.SplitMore(li, vpsPath);
                    var p2 = SHSplit.SplitCharMore(p[1], '\\');
                    var slnName = p2[0].TrimEnd('"');
                    var sfo = SolutionsIndexerHelper.SolutionWithName(slnName);
                    var dn = FS.GetDirectoryName(sfo.fullPathFolder);
                    l[i] = l[i].Replace(vpsPath, dn);
                }
            }
            // Exists FS.WithEndSlash but there could be better
            //ts = ts.Replace(vpsPath, localPath);
            ts = SHJoin.JoinNL(l);
        }
        mess.AppendLine(ts);
        ClipboardService.SetText(mess.ToString());
        if (!cmd)
        {
            MoveToApsWpf();
            //WindowHelper.Close(windowWithUserControl);
        }
        else
        {
            ThisApp.Success("Result was copied to clipboard");
        }
        // In next pushSolutionsData.onlyThese dont use so set it null to not forget to it
        pushSolutionsData.onlyThese = null;
    }
    static void MoveToApsWpf()
    {
        ThrowEx.Custom("Move to Aps.wpf");
    }
    public List<string> SkipTheseGit()
    {
        var skipThese = CA.ToListString("mono-server-setup");
        skipThese.Add(".vscode");
        return skipThese;
    }
    public List<SolutionFolder> AllSolutions(RepositoryLocal vs17)
    {
        var d = new List<SolutionFolder>();
        foreach (var item in ApsMainWindow.Instance.fwss)
        {
            var sln = item.Solutions(vs17);
            d.AddRange(sln);
        }
        return d;
    }
    public RepositoryLocal repositoryUsedInApsH = RepositoryLocal.Vs17;
    /// <summary>
    ///
    /// </summary>

    private void m(string v)
    {
        if (!cmd)
        {
            MoveToApsWpf();
            //MessageBox.Show(v);
        }
    }
    public async Task<bool> IsWebProject(ILogger logger, SolutionFolder sln, GetFileSettings getFileSettings)
    {
        var d = await AllProjectsSearchSettings.GetWebProjectsWildCard(logger, getFileSettings);
        if (d.Count == 0)
        {
            ThrowEx.IsEmpty(d, "d", "For all elements will return false");
        }
        foreach (var item in d)
        {
            if (item.Value.IsMatch(sln.nameSolution))
            {
                return true;
            }
        }
        return false;
    }
    public string MainSln2(string fullPathFolder)
    {
        string r = null;
        r = Path.Combine(fullPathFolder, FS.GetFileName(fullPathFolder) + AllExtensions.sln);
        if (FS.ExistsFile(r))
        {
            return r;
        }
        else
        {
            //r = ApsHelper.ci.GetSlns(fullPathFolder).FirstOrDefault();
            return null;
        }
    }
    public string MainSln(SolutionFolder sln)
    {
        string fullPathFolder = sln.fullPathFolder;
        string r = null;
        if (sln.slnNameWoExt == null)
        {
            r = Path.Combine(fullPathFolder, FS.GetFileName(fullPathFolder) + AllExtensions.sln);
        }
        else
        {
            r = Path.Combine(fullPathFolder, sln.slnNameWoExt + AllExtensions.sln);
        }
        if (r.Contains("apps.sunamo.cz"))
        {
        }
        if (FS.ExistsFile(r))
        {
            return r;
        }
        else
        {
            //r = ApsHelper.ci.GetSlns(fullPathFolder).FirstOrDefault();
            return null;
        }
    }
    public string AbsolutePathOfProject(string project, string sln, string eVsProjects)
    {
        var path = Path.Combine(eVsProjects, sln, project, project + ".csproj");
        return path;
    }
    public string SlnFilePathFromFolder(string fullPathFolder)
    {
        var fs = FS.GetFileName(fullPathFolder);
        return Path.Combine(fullPathFolder, fs + AllExtensions.sln);
    }
    /// <summary>
    /// Save to A1 variables projectFolder and slnFullPath
    /// A2 can be null, then will be detected automatically
    /// </summary>
    /// <param name="item"></param>
    /// <param name="documentsFolder"></param>
    public void GetProjectFolderAndSlnPath(SolutionFolder item, string documentsFolder = null)
    {
        if (documentsFolder == null)
        {
            documentsFolder = ci.DetectDocumentsFolder(item);
        }
        string relativeFromProjectFolder = item.fullPathFolder.Substring(documentsFolder.Length);
        var arr = SHSplit.SplitToParts(relativeFromProjectFolder, 3, "\\");
        // 0 - VS folder
        item.projectFolder = arr[1];
        item.slnFullPath = arr[2];
    }
    /// <summary>
    /// Return without ../
    /// Get relative path to A2 without solution base folder
    ///
    /// A1 = Scripts_Project
    /// A2 = E:\vs\Projects\PlatformIndependentNuGetPackages\dll\HtmlAgilityPack.dll
    /// Result = sunamo\dll\HtmlAgilityPack.dll
    /// </summary>
    /// <param name="sln"></param>
    /// <param name="fullPathCsproj"></param>
    public string GetRelativePathFromSolution(SolutionFolder sln, string fullPathCsproj)
    {
        GetProjectFolderAndSlnPath(sln, ci.DetectDocumentsFolder(sln));
        var projectFolder = SH.WrapWith(sln.projectFolder, "\\");
        int dx = fullPathCsproj.IndexOf(projectFolder);
        string result = fullPathCsproj.Substring(dx + projectFolder.Length);
        // 25-1-20 added ..\ before
        result = "..\\" + result;
        return result;
    }
    /// <summary>
    /// A2 - must be full paths to projects
    /// </summary>
    /// <param name="addProjectReferencesWhenAlreadyIsInSln"></param>
    /// <param name="projectsToAdd"></param>
    /// <param name="slnFromWhichIsAdded"></param>
    /// <param name="projectTypes"></param>
    /// <param name="projectIds"></param>
    /// <param name="slnToWhichAdd"></param>
    public async Task AddProjectsToSln(bool addProjectReferencesWhenAlreadyIsInSln, List<string> projectsToAdd, object slnFromWhichIsAdded, Dictionary<string, Guid> projectTypes, Dictionary<string, Guid> projectIds, string slnToWhichAdd)
    {
        // with dotnet cmd
        throw new NotImplementedException();
    }
    public List<string> GetCsprojsOnlyTopDirectory(ILogger logger, string projectPath)
    {
        var d = FSGetFiles.GetFiles(logger, projectPath, "*.csproj", SearchOption.TopDirectoryOnly);
        return d;
    }
    /// <summary>
    /// Always is SearchOption.TopDirectoryOnly
    /// </summary>
    /// <param name="up"></param>
    /// <param name="getFilesArgs"></param>
    /// <returns></returns>
    public List<string> GetSlns(ILogger logger, string path, GetFilesArgsDC getFilesArgs = null)
    {
        if (getFilesArgs == null)
        {
            getFilesArgs = new GetFilesArgsDC();
        }
        var slns = FSGetFiles.GetFiles(logger, path, "*.sln", SearchOption.TopDirectoryOnly, getFilesArgs);
        return slns;
    }
    public string DetectDocumentsFolder(SolutionFolder sln)
    {
        foreach (var item in ApsMainWindow.Instance.fwss)
        {
            if (sln.fullPathFolder.Contains(item.documentsFolder))
            {
                return item.documentsFolder;
            }
        }
        return null;
    }
    #region 2
    /// <summary>
    /// A1 - file in sunamo solution structure
    /// A2 - sln folder which will obtaine new file - can be dummy. Dont pass file, will be add one more ../
    ///
    /// A1 = sunamo\dll\HtmlAgilityPack.dll
    /// A2 = sunamo\research
    /// result = ..\..\..\sunamo\dll\HtmlAgilityPack.dll - right 1. project 2. solution 3. project folder
    /// </summary>
    /// <param name="file"></param>
    /// <param name="item"></param>
    public string GetRelativePathToProject(string file, string slnFullPath, char delimiter)
    {
        int i = SH.OccurencesOfStringIn(slnFullPath, "/");
        if (i == 0)
        {
            i = SH.OccurencesOfStringIn(slnFullPath, "\\");
        }
        i += 2;
        string relativePath = FS.AddUpfoldersToRelativePath(i, file, delimiter);
        return relativePath;
    }
    public List<string> GetFilesForGitExcludeTemporary(ILogger logger, string folder, string typedExt)
    {
        var files = FSGetFiles.GetFiles(logger, folder, FS.MascFromExtension(typedExt), SearchOption.AllDirectories);
        SunamoDevCodeHelper.RemoveTemporaryFilesVS(files);
        //list = CA.Prepend("\\", VisualStudioTempFse.filesInSolutionToDelete);
        //CA.RemoveWhichContains(files, list, true);
        //list = CA.Prepend("\\", VisualStudioTempFse.filesInProjectToDelete);
        //CA.RemoveWhichContains(files, list, true);
        return files;
    }
    public static List<string> FullPathsFromSolutionsNames(List<KeyValuePair<string, string>> slnNames)
    {
        List<string> result = new List<string>();
        foreach (var item in slnNames)
        {
            var sln = SolutionsIndexerHelper.SolutionWithName(item.Value);
            result.Add(sln.fullPathFolder);
        }
        return result;
    }
    #endregion
}