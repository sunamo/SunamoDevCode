// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Helper class for default path checks
/// </summary>
internal class DefaultPaths
{
    /// <summary>
    /// Checks if the given path should be ignored based on the base path
    /// </summary>
    /// <param name="path">Path to check</param>
    /// <param name="bpBb">Base path to check against (e.g., BitBucket directory)</param>
    /// <returns>True if path starts with the base path and should be ignored</returns>
    internal static bool IsIgnored(string path, string bpBb)
    {
        if (path.StartsWith(bpBb)) return true;
        return false;
    }
}