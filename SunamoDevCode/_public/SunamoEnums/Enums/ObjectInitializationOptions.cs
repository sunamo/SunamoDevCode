namespace SunamoDevCode._public.SunamoEnums.Enums;

/// <summary>
/// Defines options for how objects are initialized in generated code.
/// </summary>
public enum ObjectInitializationOptions
{
    /// <summary>
    /// Initialize properties using hyphen-separated values.
    /// </summary>
    Hyphens,
    /// <summary>
    /// Keep the original initialization format.
    /// </summary>
    Original,
    /// <summary>
    /// Initialize using new keyword with property assignment.
    /// </summary>
    NewAssign
}