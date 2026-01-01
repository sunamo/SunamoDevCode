namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Provides exception throwing methods for AllProjectsSearch operations
/// CZ: Poskytuje metody pro vyhazování výjimek pro operace AllProjectsSearch
/// </summary>
public class AllProjectsSearchThrow
{
    /// <summary>
    /// EN: Throws exception when project file is not valid
    /// CZ: Vyhodí výjimku když soubor projektu není platný
    /// </summary>
    /// <param name="filePath">Path to the invalid project file</param>
    public static void IsNotValidProjectFile(string filePath)
    {
        ThrowEx.Custom(filePath + " is not valid project file.");
    }
}