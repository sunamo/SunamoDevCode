// variables names: ok
namespace SunamoDevCode.Aps.Algorithms;

/// <summary>
/// EN: Deletes temporary files from Visual Studio solutions
/// CZ: Maže dočasné soubory z Visual Studio solutions
/// </summary>
public class DeleteTemporaryFilesFromSolution
{
    /// <summary>
    /// EN: Deletes folder containing temporary moved files and recreates it empty
    /// CZ: Smaže složku obsahující dočasně přesunuté soubory a vytvoří ji znovu prázdnou
    /// </summary>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Path to folder with temporary moved content (without trailing backslash)</param>
    /// <returns>True if operation succeeded, false if folder cannot be removed</returns>
    public static bool DeleteFolderWithTemporaryMovedFiles(string folderWithTemporaryMovedContentWithoutBackslash)
    {
        if (FS.ExistsDirectory(folderWithTemporaryMovedContentWithoutBackslash))
        {
            try
            {
                Directory.Delete(folderWithTemporaryMovedContentWithoutBackslash, true);
            }
            catch (Exception)
            {
                ThrowEx.FolderCantBeRemoved(folderWithTemporaryMovedContentWithoutBackslash);
                return false;
            }
        }
        FS.CreateFoldersPsysicallyUnlessThere(folderWithTemporaryMovedContentWithoutBackslash);
        Directory.CreateDirectory(folderWithTemporaryMovedContentWithoutBackslash);
        return true;
    }
    /// <summary>
    /// EN: Clears solution folder from temporary files
    /// CZ: Vyčistí složku solution od dočasných souborů
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    /// <param name="solutionFolder">Path to solution folder to clear</param>
    /// <param name="delete">Whether to delete files or just list them</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder for storing temporarily moved content</param>
    /// <param name="keepPackageJson">Whether to keep package.json files (default: false)</param>
    /// <returns>List of files found during the operation</returns>
    public static List<string> ClearSolution(ILogger logger, string solutionFolder, bool delete, string folderWithTemporaryMovedContentWithoutBackslash, bool keepPackageJson = false)
    {
        // EN: Navigate up directory tree until we find the solutions folder
        // CZ: Naviguje nahoru ve stromě adresářů dokud nenajdeme složku solutions
        string folderWithProjectsFolders = FS.GetDirectoryName(solutionFolder);
        while (!SolutionsIndexerHelper.IsTheSolutionsFolder(FS.GetFileName(folderWithProjectsFolders)))
        {
            folderWithProjectsFolders = FS.GetDirectoryName(folderWithProjectsFolders);
        }
        List<string> foundFiles = new List<string>();
        foundFiles = FSGetFiles.GetFiles(logger, solutionFolder,
            true);
        foreach (var folderPattern in VisualStudioTempFse.FoldersInSolutionToDelete)
        {
            var trimmedPattern = SHTrim.Trim(folderPattern, "\\");
            DeleteFolderIn(logger, solutionFolder, trimmedPattern, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        foreach (var folderPattern in VisualStudioTempFse.FoldersInSolutionDownloaded)
        {
            var trimmedPattern = SHTrim.Trim(folderPattern, "\\");
            DeleteFolderIn(logger, solutionFolder, trimmedPattern, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
        Dictionary<string, List<string>> filesInSolutionToDelete = VisualStudioTempFse.FilesInSolutionToDelete.dictionary;
        Dictionary<string, List<string>> filesInSolutionDownloaded = VisualStudioTempFse.FilesInSolutionDownloaded.dictionary;
        Dictionary<string, List<string>> filesInSolutionReal = FS.GetDictionaryByExtension(logger, solutionFolder, "*", SearchOption.TopDirectoryOnly);
        foreach (var extensionEntry in filesInSolutionReal)
        {
            string extension = extensionEntry.Key;
            if (filesInSolutionToDelete.ContainsKey(extension))
            {
                foreach (var file in filesInSolutionReal[extension])
                {
                    if (filesInSolutionDownloaded[extension].Any(pattern => SH.MatchWildcard(file, pattern)))
                    {
                        DeleteFilesIn(solutionFolder, file + extension, folderWithProjectsFolders, foundFiles,
                            delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
            if (filesInSolutionDownloaded.ContainsKey(extension))
            {
                foreach (var file in filesInSolutionReal[extension])
                {
                    if (filesInSolutionDownloaded[extension].Any(pattern => SH.MatchWildcard(file, pattern)))
                    {
                        DeleteFilesIn(solutionFolder, file + extension, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                    }
                }
            }
        }
        if (!keepPackageJson)
        {
            foundFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, AllExtensions.JsonExtension, true).ToList());
        }
        foundFiles.LeadingRange(DictionaryHelper.GetIfExists(filesInSolutionReal, solutionFolder, "", true).ToList());
        var projects = FSGetFolders.GetFolders(solutionFolder);
        foreach (string project in projects)
        {
            ClearProject(logger, delete, folderWithProjectsFolders, foundFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        }
        // TODO: Implement using VisualStudioTempFse
        FS.DeleteFoldersWhichNotContains(solutionFolder, "bin", CA.ToListString("node_modules"));
        return foundFiles;
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
        List<string> foundFiles = FSGetFiles.GetFiles(logger, project, true);
        ClearProject(logger, delete, FS.GetDirectoryName(project), foundFiles, project, folderWithTemporaryMovedContentWithoutBackslash);
        return foundFiles;
    }
    private static void ClearProject(ILogger logger, bool delete, string folderWithProjectsFolders, List<string> foundFiles, string project, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        DeleteTemporaryFiles(logger, folderWithProjectsFolders, project, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);

        foreach (var folderInProject in VisualStudioTempFse.FoldersInProjectToDelete)
        {
            DeleteFolderIn(logger, project, folderInProject, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
        }
    }
    /// <summary>
    /// EN: Deletes temporary files from project folder
    /// CZ: Smaže dočasné soubory ze složky projektu
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="folderWithProjectsFolders">Folder containing projects (can be null for no delete)</param>
    /// <param name="project">Project path</param>
    /// <param name="foundFiles">List to track found files</param>
    /// <param name="delete">Whether to delete files</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder for temporary moved content</param>
    private static void DeleteTemporaryFiles(ILogger logger, string folderWithProjectsFolders, string project, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        Dictionary<string, List<string>> filesInProjectToDelete = VisualStudioTempFse.FilesInProjectToDelete.dictionary;
        Dictionary<string, List<string>> filesInProjectReal = FS.GetDictionaryByExtension(logger, project, "*", SearchOption.TopDirectoryOnly);

        foreach (var extensionEntry in filesInProjectReal)
        {
            string extension = extensionEntry.Key;
            if (filesInProjectToDelete.ContainsKey(extension))
            {
                foreach (var file in filesInProjectReal[extension])
                {
                    var filePatternsToDelete = filesInProjectToDelete[extension];
                    foreach (var filePattern in filePatternsToDelete)
                    {
                        if (SH.MatchWildcard(file + extension, filePattern + extension))
                        {
                            DeleteFilesIn(project, file + extension, folderWithProjectsFolders, foundFiles, delete, folderWithTemporaryMovedContentWithoutBackslash);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// EN: Deletes or tracks files within a folder
    /// CZ: Smaže nebo sleduje soubory ve složce
    /// </summary>
    /// <param name="parentPath">Parent folder path</param>
    /// <param name="fileOrFolderName">Name of file or folder to process</param>
    /// <param name="folderWithProjectsFolders">Folder containing projects (can be null for no delete)</param>
    /// <param name="foundFiles">List to track found files</param>
    /// <param name="delete">Whether to delete files</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder for temporary moved content</param>
    static void DeleteFilesIn(string parentPath, string fileOrFolderName, string folderWithProjectsFolders, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        if (folderWithProjectsFolders != null)
        {
            string filePath = Path.Combine(parentPath, fileOrFolderName);
            if (FS.ExistsFile(filePath))
            {
                if (delete)
                {
                    if (foundFiles != null)
                    {
                        foundFiles.Add(filePath);
                    }
                }
                else
                {
                    foundFiles.Remove(filePath);
                }
                if (delete)
                {
                    try
                    {
                        string destinationPath = filePath.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
                        FS.CreateUpfoldersPsysicallyUnlessThere(destinationPath);
                        File.Move(filePath, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        CL.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
    /// <summary>
    /// EN: Deletes or tracks folders within a parent folder
    /// CZ: Smaže nebo sleduje složky v nadřazené složce
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="parentPath">Parent folder path</param>
    /// <param name="folderName">Name of folder to process</param>
    /// <param name="folderWithProjectsFolders">Folder containing projects (can be null for no delete)</param>
    /// <param name="foundFiles">List to track found files</param>
    /// <param name="delete">Whether to delete files</param>
    /// <param name="folderWithTemporaryMovedContentWithoutBackslash">Folder for temporary moved content</param>
    static void DeleteFolderIn(ILogger logger, string parentPath, string folderName, string folderWithProjectsFolders, List<string> foundFiles, bool delete, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        var folders = Directory.GetDirectories(parentPath, folderName, SearchOption.TopDirectoryOnly);
        foreach (var folderPath in folders)
        {
            if (FS.ExistsDirectory(folderPath))
            {
                var filesFound = FSGetFiles.GetFiles(logger, folderPath, true);
                if (delete)
                {
                    if (foundFiles != null)
                    {
                        foundFiles.AddRange(filesFound);
                    }
                }
                else
                {
                    // EN: When delete is false, remove files from list. When delete is true, add files to history list.
                    // CZ: Když delete je false, odebrání souborů ze seznamu. Když delete je true, přidání souborů do seznamu historie.
                    foreach (var filePath in filesFound)
                    {
                        foundFiles.Remove(filePath);
                    }
                }
                if (delete)
                {
                    try
                    {
                        FS.MoveAllRecursivelyAndThenDirectory(logger, folderPath, FS.ReplaceDirectoryThrowExceptionIfFromDoesntExists(folderPath, folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash), FileMoveCollisionOptionDC.Overwrite);
                    }
                    catch (Exception ex)
                    {
                        ThisApp.Error("Can't delete folder: " + folderPath + " " + Exceptions.TextOfExceptions(ex));
                    }
                }
            }
        }
    }
}