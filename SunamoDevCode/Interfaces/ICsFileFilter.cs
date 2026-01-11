// variables names: ok
namespace SunamoDevCode.Interfaces;

/// <summary>
/// Interface for filtering C# files
/// </summary>
public interface ICsFileFilter
{
    /// <summary>
    /// Gets filtered list of files
    /// </summary>
    /// <param name="path">Directory path to search</param>
    /// <param name="searchPattern">File search pattern (e.g., "*.cs")</param>
    /// <param name="searchOption">Search option (TopDirectoryOnly or AllDirectories)</param>
    /// <returns>Filtered list of file paths</returns>
    List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption);
}
