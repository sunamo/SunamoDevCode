namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines sorting modes for projects based on their names.
/// </summary>
public enum SortByProjectsNames
{
    /// <summary>
    /// Include projects with both same and other project names.
    /// </summary>
    WithSameAndOtherProjectsNames,
    /// <summary>
    /// Include only projects with the same project name.
    /// </summary>
    OnlyWithSameProjectName,
    /// <summary>
    /// Include only projects with other (different) project names.
    /// </summary>
    OnlyWithOtherProjectsNames,
    /// <summary>
    /// Include only projects with no project names.
    /// </summary>
    WithNoProjectsNames
}