namespace SunamoDevCode.SunamoCSharp.Args;

/// <summary>
/// Arguments for C# code generator
/// </summary>
public class CSharpGeneratorArgs
{
    /// <summary>
    /// Whether to add quotes/hyphens around string values
    /// </summary>
    public bool AddHyphens { get; set; } = false;

    /// <summary>
    /// Whether to create an instance of the generated type
    /// </summary>
    public bool CreateInstance { get; set; } = false;

    /// <summary>
    /// Whether to also generate a field
    /// </summary>
    public bool AlsoField { get; set; } = false;

    /// <summary>
    /// Whether to add value to the generated code
    /// </summary>
    public bool AddingValue { get; set; } = false;

    /// <summary>
    /// Whether to use CA (Collection Add) helper
    /// </summary>
    public bool UseCA { get; set; } = true;

    /// <summary>
    /// Delimiter to split keys with (must be char or string)
    /// </summary>
    public string SplitKeyWith { get; set; } = null;

    /// <summary>
    /// Whether to check for null before operations
    /// </summary>
    public bool CheckForNull { get; set; } = false;
}
