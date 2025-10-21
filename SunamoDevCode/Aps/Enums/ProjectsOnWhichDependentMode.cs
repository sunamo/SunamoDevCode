// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Všechny operace co jsou zde se dělají v jediné metodě
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