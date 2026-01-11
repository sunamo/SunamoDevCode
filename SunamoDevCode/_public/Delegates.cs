// variables names: ok
namespace SunamoDevCode._public;

/// <summary>
/// EN: Delegate for retrieving file settings path
/// CZ: Delegát pro získání cesty k souboru nastavení
/// </summary>
/// <param name="filePath">File path to get settings for</param>
/// <returns>Settings file path</returns>
public delegate string GetFileSettings(string filePath);

/// <summary>
/// EN: Delegate for retrieving file data path
/// CZ: Delegát pro získání cesty k datovému souboru
/// </summary>
/// <param name="filePath">File path to get data for</param>
/// <returns>Data file path</returns>
public delegate string GetFileData(string filePath);