// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Helper class for application paths
/// </summary>
internal class AppPaths
{
    /// <summary>
    /// Gets the startup path of the current process
    /// </summary>
    /// <returns>Directory path where the application started</returns>
    internal static string GetStartupPath()
    {
        return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }

    /// <summary>
    /// Gets the full path to a file in the startup directory
    /// </summary>
    /// <param name="fileName">File name to combine with startup path</param>
    /// <returns>Full path to the file</returns>
    internal static string GetFileInStartupPath(string fileName)
    {
        return Path.Combine(GetStartupPath(), fileName);
    }
}