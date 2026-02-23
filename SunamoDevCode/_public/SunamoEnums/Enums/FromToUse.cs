namespace SunamoDevCode._public.SunamoEnums.Enums;

/// <summary>
/// Defines the format used for date/time representation in FromTo operations.
/// </summary>
public enum FromToUseDC
{
    /// <summary>
    /// Standard DateTime format.
    /// </summary>
    DateTime,
    /// <summary>
    /// Unix timestamp format.
    /// </summary>
    Unix,
    /// <summary>
    /// Unix timestamp format containing only time portion.
    /// </summary>
    UnixJustTime,
    /// <summary>
    /// No specific format.
    /// </summary>
    None
}