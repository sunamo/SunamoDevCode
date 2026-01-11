// variables names: ok
namespace SunamoDevCode.Aps.Projs.Results;

/// <summary>
/// Result of finding projects that use SDK-style csproj format.
/// </summary>
public class FindProjectsWhichIsSdkStyleResult
{
    /// <summary>
    /// Gets or sets the list of SDK-style csproj projects.
    /// </summary>
    public List<string> CsprojSdkStyleList { get; set; }

    /// <summary>
    /// Gets or sets the list of .NET Standard projects.
    /// </summary>
    public List<string> NetstandardList { get; set; }

    /// <summary>
    /// Gets or sets the list of non-SDK-style csproj projects.
    /// </summary>
    public List<string> NonCsprojSdkStyleList { get; set; }
}