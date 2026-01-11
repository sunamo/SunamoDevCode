// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Helper for getting folders from file system
/// </summary>
internal class FSGetFolders
{
    /// <summary>
    /// Gets all folders that contain files matching the specified pattern
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="directory">Directory to search in</param>
    /// <param name="pattern">File pattern to match</param>
    /// <param name="searchOption">Search option for directory traversal</param>
    /// <returns>List of folder paths ending with backslash</returns>
    internal static List<string> GetFoldersEveryFolderWhichContainsFiles(ILogger logger, string directory, string pattern, SearchOption searchOption)
    {
        try
        {
            var folders = Directory.GetDirectories(directory, "*", searchOption);
            var result = new List<string>();
            foreach (var folder in folders)
            {
                var files = Directory.GetFiles(folder, pattern, searchOption).ToList();
                if (files.Count != 0) result.Add(folder);
            }
            result = result.ConvertAll(folderPath => folderPath + "\\");
            return result;

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new List<string>();
        }
    }

    /// <summary>
    /// Gets all directories in the specified path
    /// </summary>
    /// <param name="path">Path to get directories from</param>
    /// <returns>Enumerable of directory paths</returns>
    internal static IEnumerable<string> GetFolders(string path)
    {
        return Directory.GetDirectories(path);
    }
}