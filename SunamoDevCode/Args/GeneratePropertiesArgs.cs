// variables names: ok
namespace SunamoDevCode.Args;

/// <summary>
/// EN: Arguments for generating properties
/// CZ: Argumenty pro generování properties
/// </summary>
public class GeneratePropertiesArgs
{
    /// <summary>
    /// EN: Input list of strings
    /// CZ: Vstupní seznam stringů
    /// </summary>
    public List<string> Input { get; set; } = null!;

    /// <summary>
    /// EN: Whether all items should be strings
    /// CZ: Zda všechny položky mají být stringy
    /// </summary>
    public bool AllStrings { get; set; } = false;
}