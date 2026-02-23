namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines filtering options for web and non-web project types.
/// </summary>
public enum WebNonWeb
{
    /// <summary>
    /// Web projects only.
    /// </summary>
    Web,
    /// <summary>
    /// Web project .csproj files only.
    /// </summary>
    WebCsproj,
    /// <summary>
    /// Non-web projects only.
    /// </summary>
    NonWeb,
    /// <summary>
    /// Non-web project .csproj files only.
    /// </summary>
    NonWebCsproj,
    /// <summary>
    /// .NET 5+ projects.
    /// </summary>
    Five,
    /// <summary>
    /// .NET 5+ project .csproj files.
    /// </summary>
    FiveCsproj,
    /// <summary>
    /// Both web and non-web projects.
    /// </summary>
    Both
}