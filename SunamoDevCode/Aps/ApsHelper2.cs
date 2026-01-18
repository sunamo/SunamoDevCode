namespace SunamoDevCode.Aps;

public partial class ApsHelper : ApsPluginStatic
{
    public
#if ASYNC
    async Task
#else
    void
#endif
    EnterOutputOfPowershellGit_ChangeDialogResult(bool? shouldProcessMessages, GitBashBuilder stringBuilder, string eVs, string pathGetMessagesFromGitOutput)
    {
        var messages = (
#if ASYNC
    await
#endif
        GetMessagesFromGitOutput(shouldProcessMessages, pathGetMessagesFromGitOutput, eVs)).ToList();
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

        var gitBashCommands = string.Empty;
        if (stringBuilder != null)
        {
            gitBashCommands = stringBuilder.ToString();
        }

        if (pushSolutionsData.onlyThese != VpsHelperDevCode.ListVpsNew)
        {
            string vpsPath = eVs;
            //string localPath = SH.PostfixIfNotEmpty(DefaultPaths.eVsProjects, "\\");
            var list = SHGetLines.GetLines(gitBashCommands);
            CA.Trim(list);
            for (int i = 0; i < list.Count; i++)
            {
                var line = list[i];
                if (line.StartsWith("cd "))
                {
                    var parts = SHSplit.Split(line, vpsPath);
                    var pathParts = SHSplit.SplitChar(parts[1], '\\');
                    var slnName = pathParts[0].TrimEnd('"');
                    var sfo = SolutionsIndexerHelper.SolutionWithName(slnName);
                    var dn = FS.GetDirectoryName(sfo.FullPathFolder);
                    list[i] = list[i].Replace(vpsPath, dn);
                }
            }

            // Exists FS.WithEndSlash but there could be better
            //gitBashCommands = gitBashCommands.Replace(vpsPath, localPath);
            gitBashCommands = SHJoin.JoinNL(list);
        }

        mess.AppendLine(gitBashCommands);
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
        var data = new List<SolutionFolder>();
        foreach (var item in ApsMainWindow.Instance.Fwss)
        {
            var sln = item.GetSolutions(vs17);
            data.AddRange(sln);
        }

        return data;
    }

    public RepositoryLocal repositoryUsedInApsH = RepositoryLocal.Vs17;
    /// <summary>
    ///
    /// </summary>
    public async Task<bool> IsWebProject(ILogger logger, SolutionFolder sln, GetFileSettings getFileSettings)
    {
        var webProjectsWildcard = await AllProjectsSearchSettings.GetWebProjectsWildCard(logger, getFileSettings);
        if (webProjectsWildcard.Count == 0)
        {
            ThrowEx.IsEmpty(webProjectsWildcard, "webProjectsWildcard", "For all elements will return false");
        }

        foreach (var item in webProjectsWildcard)
        {
            if (item.Value.IsMatch(sln.NameSolution))
            {
                return true;
            }
        }

        return false;
    }

    public string MainSln2(string fullPathFolder)
    {
        string slnPath = null;
        slnPath = Path.Combine(fullPathFolder, FS.GetFileName(fullPathFolder) + AllExtensions.SlnExtension);
        if (FS.ExistsFile(slnPath))
        {
            return slnPath;
        }
        else
        {
            //slnPath = ApsHelper.Instance.GetSlns(fullPathFolder).FirstOrDefault();
            return null;
        }
    }

    public string MainSln(SolutionFolder sln)
    {
        string fullPathFolder = sln.FullPathFolder;
        string slnPath = null;
        if (sln.SlnNameWithoutExtension == null)
        {
            slnPath = Path.Combine(fullPathFolder, FS.GetFileName(fullPathFolder) + AllExtensions.SlnExtension);
        }
        else
        {
            slnPath = Path.Combine(fullPathFolder, sln.SlnNameWithoutExtension + AllExtensions.SlnExtension);
        }

        if (slnPath.Contains("apps.sunamo.cz"))
        {
        }

        if (FS.ExistsFile(slnPath))
        {
            return slnPath;
        }
        else
        {
            //slnPath = ApsHelper.Instance.GetSlns(fullPathFolder).FirstOrDefault();
            return null;
        }
    }

    public string AbsolutePathOfProject(string project, string slnName, string eVsProjects)
    {
        var path = Path.Combine(eVsProjects, slnName, project, project + ".csproj");
        return path;
    }

    public string SlnFilePathFromFolder(string fullPathFolder)
    {
        var folderName = FS.GetFileName(fullPathFolder);
        return Path.Combine(fullPathFolder, folderName + AllExtensions.SlnExtension);
    }

    /// <summary>
    /// Save to A1 variables projectFolder and slnFullPath
    /// A2 can be null, then will be detected automatically
    /// </summary>
    /// <param name = "sln"></param>
    /// <param name = "documentsFolder"></param>
    public void GetProjectFolderAndSlnPath(SolutionFolder sln, string? documentsFolder = null)
    {
        if (documentsFolder == null)
        {
            documentsFolder = Instance.DetectDocumentsFolder(sln);
        }

        string relativeFromProjectFolder = sln.FullPathFolder.Substring(documentsFolder.Length);
        var arr = SHSplit.SplitToParts(relativeFromProjectFolder, 3, "\\");
        // 0 - VS folder
        sln.projectFolder = arr[1];
        sln.slnFullPath = arr[2];
    }

    /// <summary>
    /// Return without ../
    /// Get relative path to A2 without solution base folder
    ///
    /// A1 = Scripts_Project
    /// A2 = E:\vs\Projects\PlatformIndependentNuGetPackages\dll\HtmlAgilityPack.dll
    /// Result = sunamo\dll\HtmlAgilityPack.dll
    /// </summary>
    /// <param name = "sln"></param>
    /// <param name = "fullPathCsproj"></param>
    public string GetRelativePathFromSolution(SolutionFolder sln, string fullPathCsproj)
    {
        GetProjectFolderAndSlnPath(sln, Instance.DetectDocumentsFolder(sln));
        var projectFolder = SH.WrapWith(sln.projectFolder, "\\");
        int projectFolderIndex = fullPathCsproj.IndexOf(projectFolder);
        string result = fullPathCsproj.Substring(projectFolderIndex + projectFolder.Length);
        // 25-1-20 added ..\ before
        result = "..\\" + result;
        return result;
    }

    /// <summary>
    /// A2 - must be full paths to projects
    /// </summary>
    /// <param name = "addProjectReferencesWhenAlreadyIsInSln"></param>
    /// <param name = "projectsToAdd"></param>
    /// <param name = "slnFromWhichIsAdded"></param>
    /// <param name = "projectTypes"></param>
    /// <param name = "projectIds"></param>
    /// <param name = "slnToWhichAdd"></param>
    public async Task AddProjectsToSln(bool addProjectReferencesWhenAlreadyIsInSln, List<string> projectsToAdd, object slnFromWhichIsAdded, Dictionary<string, Guid> projectTypes, Dictionary<string, Guid> projectIds, string slnToWhichAdd)
    {
        // with dotnet cmd
        throw new NotImplementedException();
    }

    public List<string> GetCsprojsOnlyTopDirectory(ILogger logger, string projectPath)
    {
        var data = FSGetFiles.GetFiles(logger, projectPath, "*.csproj", SearchOption.TopDirectoryOnly);
        return data;
    }

    /// <summary>
    /// Always is SearchOption.TopDirectoryOnly
    /// </summary>
    /// <param name = "up"></param>
    /// <param name = "getFilesArgs"></param>
    /// <returns></returns>
    public List<string> GetSlns(ILogger logger, string path, GetFilesArgsDC? getFilesArgs = null)
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
        foreach (var item in ApsMainWindow.Instance.Fwss)
        {
            if (sln.FullPathFolder.Contains(item.DocumentsFolder))
            {
                return item.DocumentsFolder;
            }
        }

        return null;
    }

    /// <summary>
    /// A1 - file in sunamo solution structure
    /// A2 - sln folder which will obtaine new file - can be dummy. Dont pass file, will be add one more ../
    ///
    /// A1 = sunamo\dll\HtmlAgilityPack.dll
    /// A2 = sunamo\research
    /// result = ..\..\..\sunamo\dll\HtmlAgilityPack.dll - right 1. project 2. solution 3. project folder
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "item"></param>
    public string GetRelativePathToProject(string file, string slnFullPath, char delimiter)
    {
        int separatorCount = SH.OccurencesOfStringIn(slnFullPath, "/");
        if (separatorCount == 0)
        {
            separatorCount = SH.OccurencesOfStringIn(slnFullPath, "\\");
        }

        separatorCount += 2;
        string relativePath = FS.AddUpfoldersToRelativePath(separatorCount, file, delimiter);
        return relativePath;
    }
}