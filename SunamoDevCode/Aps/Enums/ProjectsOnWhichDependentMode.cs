// variables names: ok
namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// EN: All operations defined here are performed in a single method
/// CZ: Všechny operace co jsou zde se dělají v jediné metodě
/// </summary>
public enum ProjectsOnWhichDependentMode
{
    UsedKeysInFolder,
    /// <summary>
    /// Get all files from all projects include dependant to check state of translation 
    /// </summary>
    Files,
    ReplaceStringKeysWithXlfKeys
}