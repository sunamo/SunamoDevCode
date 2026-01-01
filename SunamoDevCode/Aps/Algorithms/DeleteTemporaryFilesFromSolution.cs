namespace SunamoDevCode.Aps.Algorithms;

/// <summary>
/// EN: Deletes temporary files from Visual Studio solutions
/// CZ: Maže dočasné soubory z Visual Studio solutions
/// </summary>
public class DeleteTemporaryFilesFromSolution
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="folder"></param>
    public static bool DeleteFolderWithTemporaryMovedFiles(string folderWithTemporaryMovedContentWithoutBackslash)
    {
        string folder = folderWithTemporaryMovedContentWithoutBackslash;
        if (FS.ExistsDirectory(folder))
        {
            try
            {
                Directory.Delete(folder, true);
            }
            catch (Exception)
            {
                ThrowEx.FolderCantBeRemoved(folder);
                return false;
            }
        }
        FS.CreateFoldersPsysicallyUnlessThere(folder);
        Directory.CreateDirectory(folder);
        return true;
    }
    /// <summary>
    /// If !A3, wont copy .git files
    /// Return good files
    /// </summary>
    /// <param name="solutionFolder">Path to solution folder to clear</param>
    /// <param name="delete">Whether to delete files</param>
    /// <param name="clear">Clear parameter</param>
    /// <returns></returns>
    public static List<string> ClearSolution(ILogger logger, string solutionFolder, bool delete, string folderWithTemporaryMovedContentWithoutBackslash, bool keepPackageJson = false)
    {
        // Never use, read doc to gitBashBuilder.Clean() to avoid my data
        //GitBashBuilder gitBashBuilder = new GitBashBuilder(new TextBuilder());
        //gitBashBuilder.Cd(solutionFolder);
        //gitBashBuilder.Clean("dfx");
        //await PowershellRunner.ci.InvokeAsync(gitBashBuilder.Commands.ToArray());
        // Az narazim na slozku Projects, jeji nazev take odeberu - zustane mi jen root slozka projektu kterou nahradim
        string folderWithProjectsFolders = FS.GetDirectoryName(solutionFolder);
        while (!SolutionsIndexerHelper.IsTheSolutionsFolder(FS.GetFileName(folderWithProjectsFolders)))
        {
            folderWithProjectsFolders = FS.GetDirectoryName(folderWithProjectsFolders);
        }
        List<string> foundedFiles = new List<string>();
        foundedFiles = FSGetFiles.GetFiles(logger, solutionFolder,
            true);
        foreach (var item in VisualStudioTempFse.foldersInSolutionToDelete)
        {
            var item2 = SHTrim.Trim(item, "\\");
            DeleteFolderIn(logger, solutionFolder, item2, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        foreach (var item in VisualStudioTempFse.foldersInSolutionDownloaded)
        {
            var item2 = SHTrim.Trim(item, "\\");
            DeleteFolderIn(logger, solutionFolder, item2, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        Dictionary<string, List<string>> filesInSolutionToDelete = VisualStudioTempFse.filesInSolutionToDelete.dictionary;
        Dictionary<string, List<string>> filesInSolutionDownloaded = VisualStudioTempFse.filesInSolutionDownloaded.dictionary;
        Dictionary<string, List<string>> filesInSolutionReal = FS.GetDictionaryByExtension(logger, solutionFolder, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in filesInSolutionReal)
        {
            string ext = item.Key;
            if (filesInSolutionToDelete.ContainsKey(ext))
            {
                foreach (var file in filesInSolutionReal[ext])
                {
                    if (filesInSolutionDownloaded[ext].Any(d => SH.MatchWildcard(file, d)))
                    {
                        DeleteFilesIn(solutionFolder, file + ext, folderWithProjectsFolders, foundedFiles,
                            delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
            if (filesInSolutionDownloaded.ContainsKey(ext))
            {
                foreach (var file in filesInSolutionReal[ext])
                {
                    if (filesInSolutionDownloaded[ext].Any(d => SH.MatchWildcard(file, d)))
                    {
                        DeleteFilesIn(solutionFolder, file + ext, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
        }
        if (!keepPackageJson)
        {
            foundedFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, AllExtensions.json, true).ToList());
        }
        foundedFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, "", true).ToList());
        var projects = FSGetFolders.GetFolders(solutionFolder);
        foreach (string project in projects)
        {
            ClearProject(logger, delete, folderWithProjectsFolders, foundedFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        }
        // todo dělat přes VisualStudioTempFse
        FS.DeleteFoldersWhichNotContains(solutionFolder, "bin", CA.ToListString("node_modules"));
        return foundedFiles;
    }
    /// <summary>
    /// EN: Clears a project of temporary files
    /// CZ: Vyčistí projekt od dočasných souborů
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="project">Project path</param>
    /// <param name="delete">Whether to delete files</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder for temporary moved content</param>
    public static List<string> ClearProject(ILogger logger, string project, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        List<string> foundedFiles = FSGetFiles.GetFiles(logger, project, true);
        ClearProject(logger, delete, FS.GetDirectoryName(project), foundedFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        return foundedFiles;
    }
    private static void ClearProject(ILogger logger, bool delete, string folderWithProjectsFolders, List<string> foundedFiles, string project, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        DeleteTemporaryFiles(logger, folderWithProjectsFolders, project, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);

        foreach (var folderInProject in VisualStudioTempFse.foldersInProjectToDelete)
        {
            DeleteFolderIn(logger, project, folderInProject, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
    }
    /// <summary>
    /// folderWithProjectsFolders can be null => no delete
    /// </summary>
    /// <param name="folderWithProjectsFolders"></param>
    /// <param name="project"></param>
    private static void DeleteTemporaryFiles(ILogger logger, string folderWithProjectsFolders, string project, List<string> foundedFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        Dictionary<string, List<string>> filesInProjectToDelete = VisualStudioTempFse.filesInProjectToDelete.dictionary;
        Dictionary<string, List<string>> filesInProjectReal = FS.GetDictionaryByExtension(logger, project, "*", SearchOption.TopDirectoryOnly);

        foreach (var item in filesInProjectReal)
        {
            string ext = item.Key;
            if (filesInProjectToDelete.ContainsKey(ext))
            {
                foreach (var file in filesInProjectReal[ext])
                {
                    var filePatternsToDelete = filesInProjectToDelete[ext];
                    foreach (var item2 in filePatternsToDelete)
                    {
                        if (SH.MatchWildcard(file + ext, item2 + ext))
                        {
                            DeleteFilesIn(project, file + ext, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// folderWithProjectsFolders can be null => no delete
    /// </summary>
    /// <param name="fullPathToA2"></param>
    /// <param name="nameOfFolder"></param>
    /// <param name="folderWithProjectsFolders"></param>
    static void DeleteFilesIn(string fullPathToA2, string nameOfFolder, string folderWithProjectsFolders, List<string> foundedFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        if (folderWithProjectsFolders != null)
        {
            string filePath = Path.Combine(fullPathToA2, nameOfFolder);
            if (FS.ExistsFile(filePath))
            {
                if (delete)
                {
                    if (foundedFiles != null)
                    {
                        foundedFiles.Add(filePath);
                    }
                }
                else
                {
                    foundedFiles.Remove(filePath);
                }
                if (delete)
                {
                    try
                    {
                        string renameTo = filePath.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
                        FS.CreateUpfoldersPsysicallyUnlessThere(renameTo);
                        File.Move(filePath, renameTo);
                    }
                    catch (Exception ex)
                    {
                        CL.WriteLine(ex.Message);
                        // Throwing many errors but files really dont exists.
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
    /// <summary>
    /// A1 - path to A2
    /// A2 - to combine with A1
    /// A3 - full path to folder contains Project string
    /// </summary>
    /// <param name="fullPathToA2"></param>
    /// <param name="nameOfFolder"></param>
    /// <param name="folderWithProjectsFolders"></param>
    static void DeleteFolderIn(ILogger logger, string fullPathToA2, string nameOfFolder, string folderWithProjectsFolders, List<string> foundedFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        var folders = Directory.GetDirectories(fullPathToA2, nameOfFolder, SearchOption.TopDirectoryOnly);
        foreach (var folderPath in folders)
        {
            if (FS.ExistsDirectory(folderPath))
            {
                var ff = FSGetFiles.GetFiles(logger, folderPath, true);
                if (delete)
                {
                    if (foundedFiles != null)
                    {
                        foundedFiles.AddRange(ff);
                    }
                }
                else
                {
                    // EN: When delete is false, remove files from list. When delete is true, add files to history list.
                    // CZ: Když delete je false, odebrání souborů ze seznamu. Když delete je true, přidání souborů do seznamu historie.
                    foreach (var item in ff)
                    {
                        foundedFiles.Remove(item);
                    }
                }
                if (delete)
                {
                    //if (ff.Count() == 0)
                    //{
                    //    FS.TryDeleteDirectory(folderPath);
                    //}
                    //else
                    //{
                    try
                    {
                        FS.MoveAllRecursivelyAndThenDirectory(logger, folderPath, FS.ReplaceDirectoryThrowExceptionIfFromDoesntExists(folderPath, folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash), FileMoveCollisionOptionDC.Overwrite);
                        //FS.DeleteAllRecursivelyAndThenDirectory(folderPath);
                    }
                    catch (Exception ex)
                    {
                        // Throw me many errors about files which really dont exists
                        //MessageBox.Show(ex.Message);
                        ThisApp.Error("Can't delete folder: " + folderPath + " " + Exceptions.TextOfExceptions(ex));
                    }
                    //}
                }
                else
                {
                }
            }
        }
    }
}