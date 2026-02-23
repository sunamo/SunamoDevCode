namespace SunamoDevCode.Enums;

/// <summary>
/// Defines what type of project file is expected in a search or operation.
/// </summary>
public enum WhatIsExcepted
{
    /// <summary>
    /// Expects a solution (.sln) file.
    /// </summary>
    Sln,
    /// <summary>
    /// Expects a C# project (.csproj) file.
    /// </summary>
    Csproj,
    /// <summary>
    /// Expects both solution and project files.
    /// </summary>
    Both
}