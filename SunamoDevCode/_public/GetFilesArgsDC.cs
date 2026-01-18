namespace SunamoDevCode._public;

// TODO: Should this inherit from GetFoldersEveryFolderArgs? In vs2 it does
/// <summary>
/// EN: Arguments for getting files with various filtering and sorting options
/// CZ: Argumenty pro získávání souborů s různými možnostmi filtrování a řazení
/// </summary>
public class GetFilesArgsDC : GetFilesBaseArgsDC
{
    // TODO: This class has issues - clean up what should and shouldn't be here
    internal bool TrimExt { get; set; } = false;
    internal new bool TrimA1AndLeadingBs { get; set; } = false;
    internal List<string> ExcludeFromLocationsContains { get; set; } = new List<string>();
    internal bool DontIncludeNewest { get; set; } = false;
    /// <summary>
    /// EN: Method to exclude files, e.g. SunamoDevCodeHelper.RemoveTemporaryFilesVS
    /// CZ: Metoda pro vyloučení souborů, např. SunamoDevCodeHelper.RemoveTemporaryFilesVS
    /// </summary>
    internal Action<List<string>> ExcludeWithMethod { get; set; } = null;
    internal bool ByDateOfLastModifiedAsc { get; set; } = false;
    internal Func<string, DateTime?> LastModifiedFromFunc { get; set; }
    /// <summary>
    /// EN: Changed to false on 1-7-2020, still forgot to mention and method is problematic
    /// CZ: Změněno na false dne 1-7-2020, stále zapomenuto zmínit a metoda je problematická
    /// </summary>
    internal bool UseMascFromExtension { get; set; } = false;
    internal bool Wildcard { get; set; } = false;
}