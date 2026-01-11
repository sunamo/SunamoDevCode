// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Text file operations helper
/// </summary>
internal class TF
{
    /// <summary>
    /// Reads all lines from a file asynchronously
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <returns>List of lines from the file</returns>
    internal static async Task<List<string>?> ReadAllLines(string filePath)
    {
        return (await File.ReadAllLinesAsync(filePath)).ToList();
    }

    /// <summary>
    /// Reads all text from a file asynchronously
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <returns>File contents as string</returns>
    internal static async Task<string?> ReadAllText(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }

    /// <summary>
    /// Writes all lines to a file asynchronously
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <param name="lines">Lines to write</param>
    internal static async Task WriteAllLines(string filePath, List<string> lines)
    {
        await File.WriteAllLinesAsync(filePath, lines);
    }

    /// <summary>
    /// Writes all text to a file asynchronously
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <param name="content">Content to write</param>
    internal static async Task WriteAllText(string filePath, string content)
    {
        await File.WriteAllTextAsync(filePath, content);
    }
}