namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Settings for AllProjectsSearch with APS-specific functionality
/// CZ: Nastavení pro AllProjectsSearch s APS-specifickou funkcionalitou
/// </summary>
internal static partial class AllProjectsSearchSettings
{
    /// <summary>
    /// EN: Path to settings file
    /// CZ: Cesta k souboru nastavení
    /// </summary>
    public static readonly string PathFileSettings = AppPaths.GetFileInStartupPath("settings.ini");

    #region sectionSearchFoldersChecked

    /// <summary>
    /// EN: Returns whether path at series index is checked
    /// CZ: Vrací zda je cesta na indexu série zaškrtnutá
    /// </summary>
    /// <param name="seriesIndex">Series index to check</param>
    public static bool IsFolderSearchChecked(string seriesIndex)
    {
        return true;
    }

    /// <summary>
    /// EN: Sets path at series index to checked state
    /// CZ: Nastaví cestu na indexu série na zaškrtnutý stav
    /// </summary>
    /// <param name="seriesIndex">Series index to set</param>
    /// <param name="isChecked">Whether the folder should be checked</param>
    public static void SetSearchFolderChecked(string seriesIndex, bool isChecked)
    {
    }

    /// <summary>
    /// EN: Returns whether any path exists under the series
    /// CZ: Vrací zda existuje nějaká cesta pod sérií
    /// </summary>
    /// <param name="seriesIndex">Series index to check</param>
    public static bool ExistsFolderSearchBySerie(string seriesIndex)
    {
        return false;
    }

    /// <summary>
    /// EN: Returns normalized path under index
    /// CZ: Vrací normalizovanou cestu pod indexem
    /// </summary>
    /// <param name="seriesIndex">Series index to get path for</param>
    public static string GetSearchFolderNormalized(string seriesIndex)
    {
        return string.Empty;
    }

    /// <summary>
    /// EN: Saves normalized path to first free series and returns that series index
    /// CZ: Uloží normalizovanou cestu na první volnou sérii a vrátí index této série
    /// </summary>
    /// <param name="folderPath">Folder path to add</param>
    public static int AddFolderSearch(string folderPath)
    {
        return 1;
    }

    #endregion
}