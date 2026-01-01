namespace SunamoDevCode.Aps.Data;

/// <summary>
/// EN: Solution folder with detailed file count information
/// CZ: Solution složka s detailními informacemi o počtu souborů
/// </summary>
public class SolutionFolderDetailedInfo : SolutionFolder
{
    // TODO: whole was moved to Aps.Statistic. use interface and to initialize delegate

    /// <summary>
    /// EN: Overall count of files in the solution
    /// CZ: Celkový počet souborů v solution
    /// </summary>
    public int OverallFiles = 0;

    /// <summary>
    /// EN: Initializes a new instance from a SolutionFolder
    /// CZ: Inicializuje novou instanci ze SolutionFolder
    /// </summary>
    /// <param name="sf">Source solution folder</param>
    public SolutionFolderDetailedInfo(SolutionFolder sf)
    {
        countOfImages = sf.countOfImages;
        displayedText = sf.displayedText;
        fullPathFolder = sf.fullPathFolder;
        nameSolutionWithoutDiacritic = sf.nameSolutionWithoutDiacritic;

        displayedText += " (" + OverallFiles.ToString() + ")";
    }
}

/// <summary>
/// EN: Comparer for sorting solution folders by source file count (descending)
/// CZ: Comparátor pro řazení solution složek podle počtu zdrojových souborů (sestupně)
/// </summary>
internal class SolutionFolderDetailedInfoComparerBySourceFiles : IComparer<SolutionFolderDetailedInfo>
{
    /// <summary>
    /// EN: Compares two solution folders by overall file count in descending order
    /// CZ: Porovná dvě solution složky podle celkového počtu souborů sestupně
    /// </summary>
    public int Compare(SolutionFolderDetailedInfo? x, SolutionFolderDetailedInfo? y)
    {
        if (x == null || y == null) return 0;
        return x.OverallFiles.CompareTo(y.OverallFiles) * -1;
    }
}