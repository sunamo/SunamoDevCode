namespace SunamoDevCode.Aps;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class ApsHelper : ApsPluginStatic
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
            var text = item.Solutions(RepositoryLocal.Vs17);
            foreach (var sln in text)
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
                    if (IsWeb(csp))
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
        var dx = webProjects.Where(data => data.Contains("desktop"));
#endif
        return new Tuple<List<string>, List<string>>(webProjects, notWebProjects);
    }

    public static bool IsWeb(string csp)
    {
        //\WebApplication
        // \webelieve.cz
        return CA.ContainsAnyFromElementBool(csp, AllProjectsSearchSettings.DontReplaceReferencesIn);
    }

    public async Task PushSolutionsContinuouslyWindow_ChangeDialogResult(bool? builder, Func<List<string>, Task<List<List<string>>>> psInvoke, string eVs, string pathGetMessagesFromGitOutput)
    {
        ThisApp.Appeal("PushSolutionsContinuouslyWindow_ChangeDialogResult");
        //string gitBashBuilderS = null;
        bool release = true;
        gitPullVps.Clear();
        GitBashBuilder gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        if (BTS.GetValueOfNullable(builder))
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

            AsyncPushSolutions al = new AsyncPushSolutions
            {
                foldersWithSolutions = foldersWithSolutions,
                release = release,
                gitBashBuilder = gitBashBuilder,
                pushArgs = pushArgs,
                commitMessage = commitMessage,
                pushSolutionsData = pushSolutionsData,
                gitStatus = gitStatus
            };
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
            //ParameterizedThreadStart parameter = new ParameterizedThreadStart(CheckForPushInThread);
            //Thread temp = new Thread(parameter) { IsBackground = true };
            //temp.SetApartmentState(ApartmentState.STA);
            //temp.Start(al);
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

        var temp = WebAndNonWebProjects(logger, withCsprojs);
        if (webNonWeb == WebNonWeb.NonWeb || webNonWeb == WebNonWeb.NonWebCsproj)
        {
            return temp.Item2;
        }
        else if (webNonWeb == WebNonWeb.Web || webNonWeb == WebNonWeb.WebCsproj)
        {
            return temp.Item1;
        }
        else if (webNonWeb == WebNonWeb.Both)
        {
            List<string> lines = new List<string>(temp.Item1.Count + temp.Item2.Count);
            lines.AddRange(temp.Item1);
            lines.AddRange(temp.Item2);
            return lines;
        }

        ThrowEx.NotImplementedCase(webNonWeb);
        return null;
    }
}