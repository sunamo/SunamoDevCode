// variables names: ok
namespace SunamoDevCode.Args;

/// <summary>
/// EN: Arguments for getting files in DevCode context
/// CZ: Argumenty pro získání souborů v DevCode kontextu
/// </summary>
public class GetFilesDCArgs
{
    /// <summary>
    /// EN: Whether to get files only in _sunamo folder
    /// CZ: Zda získat soubory pouze ve složce _sunamo
    /// </summary>
    public bool OnlyInSunamo { get; set; }
}
