namespace SunamoDevCode.Args;

/// <summary>
/// EN: Arguments for removing empty target or source entries from XLF files
/// CZ: Argumenty pro odstranění prázdných target nebo source záznamů z XLF souborů
/// </summary>
public class RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs
{
    /// <summary>
    /// EN: Default instance with default settings
    /// CZ: Výchozí instance s výchozím nastavením
    /// </summary>
    public static RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs Default { get; set; } = new RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs();

    /// <summary>
    /// EN: Whether to remove the entire trans-unit element when target or source is empty
    /// CZ: Zda odstranit celý trans-unit element když je target nebo source prázdný
    /// </summary>
    public bool RemoveWholeTransUnit { get; set; } = true;

    /// <summary>
    /// EN: Whether to save the file after modifications
    /// CZ: Zda uložit soubor po úpravách
    /// </summary>
    public bool Save { get; set; } = true;
}