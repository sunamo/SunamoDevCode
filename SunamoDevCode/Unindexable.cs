namespace SunamoDevCode;

/// <summary>
/// Holds collections of unindexable path patterns loaded from configuration files.
/// </summary>
public class Unindexable
{
    /// <summary>
    /// Collection of file name patterns that should not be indexed (partial match).
    /// </summary>
    public PpkOnDriveDC UnindexableFileNames { get; set; } = null!;
    /// <summary>
    /// Collection of exact file names that should not be indexed.
    /// </summary>
    public PpkOnDriveDC UnindexableFileNamesExactly { get; set; } = null!;
    /// <summary>
    /// Collection of path ending patterns that should not be indexed.
    /// </summary>
    public PpkOnDriveDC UnindexablePathEnds { get; set; } = null!;
    /// <summary>
    /// Collection of path substring patterns that should not be indexed.
    /// </summary>
    public PpkOnDriveDC UnindexablePathParts { get; set; } = null!;
    /// <summary>
    /// Collection of path prefix patterns that should not be indexed.
    /// </summary>
    public PpkOnDriveDC UnindexablePathStarts { get; set; } = null!;
}