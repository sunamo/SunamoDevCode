namespace SunamoDevCode.Aps.Projs.Enums;

/// <summary>
/// Defines supported .NET framework versions for project detection.
/// </summary>
public enum SupportedNetFw
{
    /// <summary>
    /// No framework detected.
    /// </summary>
    None,
    /// <summary>
    /// .NET Framework 4.8.
    /// </summary>
    net48,
    /// <summary>
    /// .NET 6.0.
    /// </summary>
    net60,
    /// <summary>
    /// .NET Standard 2.0.
    /// </summary>
    netstandard20,
    /// <summary>
    /// Invalid or malformed XML in project file.
    /// </summary>
    BadXml
}