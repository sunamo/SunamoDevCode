// variables names: ok
namespace SunamoDevCode.CodeGenerator;

/// <summary>
/// Represents an item in an enumeration with metadata.
/// </summary>
public class EnumItem
{
    /// <summary>
    /// Gets or sets the hexadecimal value (without the initial 0x prefix).
    /// </summary>
    public string Hex { get; set; } = "";

    /// <summary>
    /// Gets or sets the attributes dictionary for the enum item.
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; } = null;

    /// <summary>
    /// Gets or sets the name of the enum item.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the comment for the enum item.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}