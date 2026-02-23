namespace SunamoDevCode.MsBuild.Values;

/// <summary>
/// Constants for MSBuild project item types used in csproj files.
/// </summary>
public class VsProjectItemTypes
{
    /// <summary>
    /// Content item type - files included as content in the output.
    /// </summary>
    public const string Content = "Content";
    /// <summary>
    /// Compile item type - source code files to compile.
    /// </summary>
    public const string Compile = "Compile";
    /// <summary>
    /// Reference item type - assembly references.
    /// </summary>
    public const string Reference = "Reference";
    /// <summary>
    /// PackageReference item type - NuGet package references.
    /// </summary>
    public const string PackageReference = "PackageReference";
    /// <summary>
    /// ProjectReference item type - project-to-project references.
    /// </summary>
    public const string ProjectReference = "ProjectReference";
    /// <summary>
    /// None item type - files included in the project but not compiled or copied.
    /// </summary>
    public const string None = "None";
}