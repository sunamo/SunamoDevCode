namespace SunamoDevCode;

/// <summary>
/// Provides methods for retrieving files from solution and project directories with DevCode-specific filtering.
/// </summary>
public class FSGetFilesDC
{
    /// <summary>
    /// Gets files from project directories within a solution folder, optionally filtering to _sunamo subfolders only.
    /// </summary>
    /// <param name="slnFolder">Root solution folder to search.</param>
    /// <param name="fileMask">File mask pattern (e.g., "*.cs").</param>
    /// <param name="searchOption">Whether to search top directory only or all subdirectories.</param>
    /// <param name="arguments">Arguments controlling the search behavior.</param>
    /// <returns>List of matching file paths.</returns>
    public static List<string> GetFilesDC(string slnFolder, string fileMask, SearchOption searchOption, GetFilesDCArgs arguments)
    {
        List<string> resultFiles = new List<string>();
        var projectDirectories = Directory.GetDirectories(slnFolder);
        foreach (var projectDirectory in projectDirectories)
        {
            if (arguments.OnlyInSunamo)
            {
                var sunamoFolder = Path.Combine(projectDirectory, "_sunamo");
                if (Directory.Exists(sunamoFolder))
                {
                    resultFiles.AddRange(Directory.GetFiles(sunamoFolder, fileMask, searchOption));
                }
            }
            else
            {
                resultFiles.AddRange(Directory.GetFiles(projectDirectory, fileMask, searchOption));
            }
        }
        return resultFiles;
    }
}