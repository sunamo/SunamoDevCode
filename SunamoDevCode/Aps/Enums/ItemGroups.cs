namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines types of MSBuild item groups in a project file.
/// </summary>
public enum ItemGroups
{
    /// <summary>
    /// Content item group for static files.
    /// </summary>
    Content,
    /// <summary>
    /// Compile item group for source code files.
    /// </summary>
    Compile,
    /// <summary>
    /// Reference item group for assembly references.
    /// </summary>
    Reference,
    /// <summary>
    /// PackageReference item group for NuGet packages.
    /// </summary>
    PackageReference,
    /// <summary>
    /// ProjectReference item group for project-to-project references.
    /// </summary>
    ProjectReference,
    /// <summary>
    /// No item group specified.
    /// </summary>
    None
}