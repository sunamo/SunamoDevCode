namespace SunamoDevCode;

/// <summary>
/// Holds full paths to the unindexable files configuration files, resolved from a base path.
/// </summary>
public class UnindexableFilesPaths
{
    /// <summary>
    /// Full path to the file listing unindexable file names.
    /// </summary>
    public string FileUnindexableFileNames { get; set; }
    /// <summary>
    /// Full path to the file listing exact unindexable file names.
    /// </summary>
    public string FileUnindexableFileNamesExactly { get; set; }
    /// <summary>
    /// Full path to the file listing unindexable path endings.
    /// </summary>
    public string FileUnindexablePathEnds { get; set; }
    /// <summary>
    /// Full path to the file listing unindexable path parts.
    /// </summary>
    public string FileUnindexablePathParts { get; set; }
    /// <summary>
    /// Full path to the file listing unindexable path starts.
    /// </summary>
    public string FileUnindexablePathStarts { get; set; }

    /// <summary>
    /// Initializes paths by combining the base path with unindexable file name constants.
    /// </summary>
    /// <param name="basePath">Base directory path for the configuration files.</param>
    public UnindexableFilesPaths(string basePath)
    {
        var fileNames = UnindexableFilesNames.Instance;
        FileUnindexablePathParts = basePath + fileNames.FileUnindexablePathParts;
        FileUnindexableFileNames = basePath + fileNames.FileUnindexableFileNames;
        FileUnindexableFileNamesExactly = basePath + fileNames.FileUnindexableFileNamesExactly;
        FileUnindexablePathEnds = basePath + fileNames.FileUnindexablePathEnds;
        FileUnindexablePathStarts = basePath + fileNames.FileUnindexablePathStarts;
    }
}