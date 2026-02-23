namespace SunamoDevCode;

/// <summary>
/// Helper methods for common SunamoDevCode operations like file cleanup and solution copying.
/// </summary>
public class SunamoDevCodeHelper
{
    /// <summary>
    /// Attempts to delete a directory. If the first attempt fails, normalizes file attributes and retries.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="directoryPath">Path to the directory to delete.</param>
    /// <returns>True if the directory was successfully deleted or does not exist.</returns>
    public static bool TryDeleteDirectory(ILogger logger, string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) return true;

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
            // EN: It's try so don't know what this is doing here
            // CZ: Je to try takže nevím co tu dělá tohle
        }

        var files = FSGetFiles.GetFiles(logger, directoryPath, "*", SearchOption.AllDirectories);
        foreach (var filePath in files) File.SetAttributes(filePath, FileAttributes.Normal);

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
        }

        return false;
    }

    /// <summary>
    /// Copies a solution folder to a destination, excluding temporary VS files and git files, then creates an archive.
    /// </summary>
    /// <param name="slnFolder">Source solution folder path.</param>
    /// <param name="folderTo">Destination folder path.</param>
    /// <param name="archive">Action to create an archive from the copied folder.</param>
    public static void CopySolution(string slnFolder, string folderTo, Action<string> archive)
    {
        var list = Directory.GetFiles(slnFolder, "*", SearchOption.AllDirectories).ToList();
        RemoveTemporaryFilesVS(list);
        RemoveGitFiles(list);

        var sourceBasePath = Path.GetDirectoryName(slnFolder)!;
        FS.WithEndSlash(ref sourceBasePath);
        FS.WithEndSlash(ref folderTo);

        var slnFolderTo = Path.Combine(folderTo, Path.GetFileName(slnFolder));
        FS.TryDeleteDirectory(slnFolderTo);

        foreach (var item in list)
        {
            var newPath = item.Replace(sourceBasePath, folderTo);
            FS.CreateUpfoldersPsysicallyUnlessThere(newPath);
            //FS.CopyFile(item, newPath, FileMoveCollisionOptionDC.DontManipulate);
        }

        archive(slnFolderTo);
        //ThisApp.Info("Archive was created successfully, is important create archive because first open with VS because will create folders package,obj,bin");
    }

    /// <summary>
    /// Removes git-related files and downloaded/temporary folder entries from the file list.
    /// </summary>
    /// <param name="files">List of file paths to filter in-place.</param>
    /// <param name="isIncludingGitFiles">Whether to keep git files (true) or remove them (false).</param>
    /// <param name="isIncludingDownloadedFolders">Whether to keep downloaded folders (true) or remove them (false).</param>
    /// <param name="isIncludingFoldersToDelete">Whether to keep folders marked for deletion (true) or remove them (false).</param>
    public static void RemoveGitFiles(List<string> files, bool isIncludingGitFiles = true, bool isIncludingDownloadedFolders = false,
        bool isIncludingFoldersToDelete = false)
    {
        string? wrapped = null;

        if (!isIncludingGitFiles)
        {
            wrapped = SH.WrapWith(VisualStudioTempFse.GitFolderName, "\"");
            files.RemoveAll(d => d.Contains(wrapped));
        }

        if (!isIncludingDownloadedFolders)
            foreach (var item in VisualStudioTempFse.FoldersInSolutionDownloaded)
            {
                wrapped = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wrapped));
            }

        if (!isIncludingFoldersToDelete)
            foreach (var item in VisualStudioTempFse.FoldersInSolutionToDelete)
            {
                wrapped = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wrapped));
            }
    }

    /// <summary>
    /// Removes Visual Studio temporary files (bin, obj, packages, etc.) from the file list.
    /// </summary>
    /// <param name="files">List of file paths to filter in-place.</param>
    public static void RemoveTemporaryFilesVS(List<string> files)
    {
        var list = VisualStudioTempFseWrapped.FoldersInSolutionToDelete;

        // todo list je zde List<string>, chce jen string, později to analyzovat 

        //As foldersInProjectToDelete dont have contains WildCard, set false
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersInProjectToDelete;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersAnywhereToDelete;
        CA.RemoveWhichContainsList(files, list, false);

        list = VisualStudioTempFseWrapped.FoldersInSolutionDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersInProjectDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.FoldersAnywhereDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
    }

    private static bool IsNameOfHtmlAttrValue(string between)
    {
        return AllHtmlAttrsValues.list.Contains(between.Trim());
    }

    private static bool IsNameOfHtmlAttr(string between)
    {
        return AllHtmlAttrs.list!.Contains(between.Trim());
    }

    /// <summary>
    /// Determines whether the given text is a known HTML tag name, optionally with a numeric suffix.
    /// </summary>
    /// <param name="between">Text to check against known HTML tags.</param>
    /// <param name="add">Initial value indicating whether to add (overwritten internally).</param>
    /// <returns>True if the text matches an HTML tag name.</returns>
    public static bool IsNameOfHtmlTag(string between, bool add)
    {
        string? element = null;
        var startWithTag = CA.StartWith(AllHtmlTags.list!, between, out element);
        startWithTag = element;
        if (startWithTag != null)
        {
            if (startWithTag == between)
            {
                add = true;
            }
            else
            {
                var remain = between.Substring(startWithTag.Length);
                add = int.TryParse(remain, out var _);
            }
        }

        return add;
    }

    /// <summary>
    ///     A1 normal, not lower
    /// </summary>
    /// <param name="between"></param>
    public static bool IsNameOfControl(string between)
    {
        var add = false;
        add = IsNameOfHtmlTag(between, add);
        if (!add) add = IsNameOfHtmlAttr(between);

        if (!add) add = IsNameOfHtmlAttrValue(between);

        if (!add)
        {
            var firstInt = -1;
            var i = 0;
            foreach (var item in between)
            {
                if (char.IsLower(item))
                {
                    if (firstInt != -1)
                    {
                        add = false;
                        break;
                    }
                }
                else if (char.IsNumber(item))
                {
                    if (firstInt == -1) firstInt = i;
                }
                else
                {
                    add = false;
                    break;
                }

                i++;
            }

            var prefix = between;
            if (firstInt != -1) prefix = between.Substring(0, firstInt);

            add = SystemWindowsControls.IsShortcutOfControl(prefix);
        }

        return add;
    }
}