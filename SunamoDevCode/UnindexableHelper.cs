// variables names: ok
namespace SunamoDevCode;

/// <summary>
/// Helper class for managing Unindexable paths and files configuration.
/// </summary>
public class UnindexableHelper
{
    /// <summary>
    /// Gets or sets the Unindexable configuration.
    /// </summary>
    public static Unindexable Unindexable;

    /// <summary>
    /// Gets the Unindexable path parts configuration.
    /// </summary>
    public static PpkOnDriveDC UnindexablePathParts => Unindexable.UnindexablePathParts;

    /// <summary>
    /// Gets the Unindexable file names configuration.
    /// </summary>
    public static PpkOnDriveDC UnindexableFileNames => Unindexable.UnindexableFileNames;

    /// <summary>
    /// Gets the Unindexable path ends configuration.
    /// </summary>
    public static PpkOnDriveDC UnindexablePathEnds => Unindexable.UnindexablePathEnds;

    /// <summary>
    /// Gets the Unindexable path starts configuration.
    /// </summary>
    public static PpkOnDriveDC UnindexablePathStarts => Unindexable.UnindexablePathStarts;

    /// <summary>
    ///     Into A1 insert SearchCodeElementsUC .ufp
    /// </summary>
    /// <param name="filePaths">Unindexable file paths configuration</param>
    public static void Load(UnindexableFilesPaths filePaths)
    {
        Unindexable = new Unindexable();

        //ClipboardService.SetText(filePaths.fileUnindexablePathParts);
        //PD.ShowMb(filePaths.fileUnindexablePathParts);

        Unindexable.UnindexablePathParts = new PpkOnDriveDC(filePaths.FileUnindexablePathParts);

        Unindexable.UnindexableFileNames = new PpkOnDriveDC(filePaths.FileUnindexableFileNames);
        Unindexable.UnindexableFileNamesExactly = new PpkOnDriveDC(filePaths.FileUnindexableFileNamesExactly);
        Unindexable.UnindexablePathEnds = new PpkOnDriveDC(filePaths.FileUnindexablePathEnds);
        Unindexable.UnindexablePathStarts = new PpkOnDriveDC(filePaths.FileUnindexablePathStarts);
    }

    /// <summary>
    /// Determines whether the specified path should be indexed based on folder rules.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the folder should be indexed, false otherwise.</returns>
    public static bool IsToIndexedFolder(string path)
    {
        if (UnindexablePathStarts != null && UnindexablePathParts != null)
        {
            if (UnindexablePathParts.TrueForAll(part => !path.Contains(part)))
                if (UnindexablePathStarts.TrueForAll(start => !path.StartsWith(start)))
                    return true;
        }
        else
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified file should be indexed based on path and file name rules.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <param name="fileName">The file name to check.</param>
    /// <param name="sci_IsIndexed">Optional additional indexing check function.</param>
    /// <returns>True if the file should be indexed, false otherwise.</returns>
    public static bool IsToIndexed(string path, string fileName, Func<string, bool> sci_IsIndexed)
    {
        if (UnindexablePathEnds != null && UnindexableFileNames != null)
        {
            //Checking for sth for which is checking in SourceCodeIndexerRoslyn.ProcessFile
            if (UnindexablePathEnds.TrueForAll(ending => !path.EndsWith(ending)))
                if (UnindexableFileNames.TrueForAll(name => !fileName.Contains(name)))
                {
                    if (sci_IsIndexed == null)
                        return true;
                    return sci_IsIndexed(path);
                }
        }
        else
        {
            return true;
        }

        return false;
    }
}