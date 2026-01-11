// variables names: ok
namespace SunamoDevCode.Data;

/// <summary>
/// Result of parsing global usings from a C# file.
/// </summary>
public class ParseGlobalUsingsResult
{
    /// <summary>
    /// Gets or sets the list of global using statements.
    /// </summary>
    public List<string> GlobalUsings { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the dictionary of global symbols (e.g., #define symbols).
    /// </summary>
    public Dictionary<string, string> GlobalSymbols { get; set; } = new();
}