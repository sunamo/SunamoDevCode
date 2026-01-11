// variables names: ok
namespace SunamoDevCode.Aps.Projs.Results;

/// <summary>
/// Result of checking if a project uses SDK-style csproj format.
/// </summary>
public class IsProjectCsprojSdkStyleResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the project is SDK-style and .NET Core.
    /// </summary>
    public bool IsProjectCsprojSdkStyleIsCore { get; set; }

    /// <summary>
    /// Gets or sets the content of the csproj file.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the project targets .NET Standard.
    /// </summary>
    public bool IsNetstandard { get; set; }
}