namespace SunamoDevCode._sunamo.SunamoArgs;

/// <summary>
/// Arguments for remove starting with operation
/// </summary>
internal class RemoveStartingWithArgs
{
    /// <summary>
    /// Whether to trim strings before checking if they start with the prefix
    /// </summary>
    internal bool TrimBeforeFinding = false;

    /// <summary>
    /// Whether the comparison is case sensitive
    /// </summary>
    internal bool CaseSensitive = true;
}