// variables names: ok
namespace SunamoDevCode.Data;

/// <summary>
/// Singleton class that maintains collections of files that should not be indexed.
/// </summary>
public class UnindexableFiles
{
    /// <summary>
    /// Gets the singleton instance of UnindexableFiles.
    /// </summary>
    public static UnindexableFiles Instance = new UnindexableFiles();

    /// <summary>
    /// Prevents external instantiation.
    /// </summary>
    private UnindexableFiles()
    {
    }

    /// <summary>
    /// Gets or sets files that should not be indexed based on path parts.
    /// </summary>
    public CollectionWithoutDuplicatesDC<string> UnindexablePathPartsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    /// <summary>
    /// Gets or sets files that should not be indexed based on file names (partial match).
    /// </summary>
    public CollectionWithoutDuplicatesDC<string> UnindexableFileNamesFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    /// <summary>
    /// Gets or sets files that should not be indexed based on exact file names.
    /// </summary>
    public CollectionWithoutDuplicatesDC<string> UnindexableFileNamesExactlyFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    /// <summary>
    /// Gets or sets files that should not be indexed based on path endings.
    /// </summary>
    public CollectionWithoutDuplicatesDC<string> UnindexablePathEndsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();

    /// <summary>
    /// Gets or sets files that should not be indexed based on path starts.
    /// </summary>
    public CollectionWithoutDuplicatesDC<string> UnindexablePathStartsFiles { get; set; } = new CollectionWithoutDuplicatesDC<string>();
}