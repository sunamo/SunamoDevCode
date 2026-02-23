namespace SunamoDevCode;

/// <summary>
/// Contains the file names for unindexable path configuration files.
/// </summary>
public class UnindexableFilesNames
{
    /// <summary>
    /// Singleton instance providing access to unindexable file name constants.
    /// </summary>
    public static UnindexableFilesNames Instance { get; } = new();
    /// <summary>
    /// File name for the list of unindexable file names (partial match).
    /// </summary>
    public string FileUnindexableFileNames { get; } = "unindexableFileNames.txt";
    /// <summary>
    /// File name for the list of exact unindexable file names.
    /// </summary>
    public string FileUnindexableFileNamesExactly { get; } = "unindexableFileNamesExactly.txt";
    /// <summary>
    /// File name for the list of unindexable path endings.
    /// </summary>
    public string FileUnindexablePathEnds { get; } = "unindexablePathEnds.txt";

    /// <summary>
    /// File name for the list of unindexable path parts (substrings).
    /// </summary>
    public string FileUnindexablePathParts { get; } = "unindexablePathParts.txt";
    /// <summary>
    /// File name for the list of unindexable path starts (prefixes).
    /// </summary>
    public string FileUnindexablePathStarts { get; } = "unindexablePathStarts.txt";

    private UnindexableFilesNames()
    {
    }
}