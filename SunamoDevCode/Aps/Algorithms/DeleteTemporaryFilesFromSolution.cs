// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Algorithms;

public class DeleteTemporaryFilesFromSolution
{
    static Type type = typeof(DeleteTemporaryFilesFromSolution);
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
            catch (Exception ex)
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
    /// <param name="v"></param>
    /// <param name="delete"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static List<string> ClearSolution(ILogger logger, string v, bool delete, string folderWithTemporaryMovedContentWithoutBackslash, bool keepPackageJson = false)
    {
        // Never use, read doc to gitBashBuilder.Clean() to avoid my data
        //GitBashBuilder gitBashBuilder = new GitBashBuilder(new TextBuilder());
        //gitBashBuilder.Cd(v);
        //gitBashBuilder.Clean("dfx");
        //await PowershellRunner.ci.InvokeAsync(gitBashBuilder.Commands.ToArray());
        // Az narazim na slozku Projects, jeji nazev take odeberu - zustane mi jen root slozka projektu kterou nahradim
        string folderWithProjectsFolders = FS.GetDirectoryName(v);
        while (!SolutionsIndexerHelper.IsTheSolutionsFolder(FS.GetFileName(folderWithProjectsFolders)))
        {
            folderWithProjectsFolders = FS.GetDirectoryName(folderWithProjectsFolders);
        }
        List<string> foundedFiles = new List<string>();
        foundedFiles = FSGetFiles.GetFiles(logger, v,
            true);
        foreach (var item in VisualStudioTempFse.foldersInSolutionToDelete)
        {
            var item2 = SHTrim.Trim(item, "\\");
            DeleteFolderIn(logger, v, item2, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        foreach (var item in VisualStudioTempFse.foldersInSolutionDownloaded)
        {
            var item2 = SHTrim.Trim(item, "\\");
            DeleteFolderIn(logger, v, item2, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        Dictionary<string, List<string>> filesInSolutionToDelete = VisualStudioTempFse.filesInSolutionToDelete.dictionary;
        Dictionary<string, List<string>> filesInSolutionDownloaded = VisualStudioTempFse.filesInSolutionDownloaded.dictionary;
        Dictionary<string, List<string>> filesInSolutionReal = FS.GetDictionaryByExtension(logger, v, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in filesInSolutionReal)
        {
            string ext = item.Key;
            if (filesInSolutionToDelete.ContainsKey(ext))
            {
                foreach (var file in filesInSolutionReal[ext])
                {
                    if (filesInSolutionDownloaded[ext].Any(d => SH.MatchWildcard(file, d)))
                    {
                        DeleteFilesIn(v, file + ext, folderWithProjectsFolders, foundedFiles,
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
                        DeleteFilesIn(v, file + ext, folderWithProjectsFolders, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
        }
        if (!keepPackageJson)
        {
            foundedFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, v, AllExtensions.json, true).ToList());
        }
        foundedFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, v, "", true).ToList());
        var projects = FSGetFolders.GetFolders(v);
        foreach (string project in projects)
        {
            ClearProject(logger, delete, folderWithProjectsFolders, foundedFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        }
        // todo dělat přes VisualStudioTempFse
        FS.DeleteFoldersWhichNotContains(v, "bin", CA.ToListString("node_modules"));
        return foundedFiles;
    }
    public static List<string> ClearProject(ILogger logger, string project, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        List<string> foundedFiles = FSGetFiles.GetFiles(logger, project, true);
#if DEBUG
        if (project.Contains("Lyrics"))
        {
        }
#endif
        ClearProject(logger, delete, FS.GetDirectoryName(project), foundedFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        return foundedFiles;
    }
    private static void ClearProject(ILogger logger, bool delete, string folderWithProjectsFolders, List<string> foundedFiles, string project, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        DeleteTemporaryFiles(logger, folderWithProjectsFolders, project, foundedFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
#if DEBUG
        if (project.Contains("Lyrics"))
        {
        }
#endif
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
#if DEBUG
        if (project.Contains("Lyrics"))
        {
        }
#endif
        foreach (var item in filesInProjectReal)
        {
            string ext = item.Key;
            if (filesInProjectToDelete.ContainsKey(ext))
            {
                foreach (var file in filesInProjectReal[ext])
                {
                    var ls2 = filesInProjectToDelete[ext];
                    foreach (var item2 in ls2)
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
            string p = Path.Combine(fullPathToA2, nameOfFolder);
            if (FS.ExistsFile(p))
            {
                if (delete)
                {
                    if (foundedFiles != null)
                    {
                        foundedFiles.Add(p);
                    }
                }
                else
                {
                    foundedFiles.Remove(p);
                }
                if (delete)
                {
                    try
                    {
                        string renameTo = p.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
                        FS.CreateUpfoldersPsysicallyUnlessThere(renameTo);
                        File.Move(p, renameTo);
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
        foreach (var p in folders)
        {
            if (FS.ExistsDirectory(p))
            {
                var ff = FSGetFiles.GetFiles(logger, p, true);
                if (delete)
                {
                    if (foundedFiles != null)
                    {
                        foundedFiles.AddRange(ff);
                    }
                }
                else
                {
                    // Ano, je to tu pojebané. Při delete je tam přidávám bo historie. Při !delete naopak odebírám
                    foreach (var item in ff)
                    {
                        foundedFiles.Remove(item);
                    }
                }
                if (delete)
                {
                    //if (ff.Count() == 0)
                    //{
                    //    FS.TryDeleteDirectory(p);
                    //}
                    //else
                    //{
                    try
                    {
                        FS.MoveAllRecursivelyAndThenDirectory(logger, p, FS.ReplaceDirectoryThrowExceptionIfFromDoesntExists(p, folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash), FileMoveCollisionOptionDC.Overwrite);
                        //FS.DeleteAllRecursivelyAndThenDirectory(p);
                    }
                    catch (Exception ex)
                    {
                        // Throw me many errors about files which really dont exists
                        //MessageBox.Show(ex.Message);
                        ThisApp.Error("Can't delete folder: " + p + " " + Exceptions.TextOfExceptions(ex));
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