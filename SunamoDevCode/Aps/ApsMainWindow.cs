namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Main window class for APS (AllProjectsSearch) application
/// CZ: Třída hlavního okna pro APS (AllProjectsSearch) aplikaci
/// </summary>
public class ApsMainWindow : IMainWindowCsFileFilter, IAbstractCatalog<string, string>
{
    #region IAbstractCatalog
    /// <summary>
    /// EN: Abstract catalog instance
    /// CZ: Instance abstraktního katalogu
    /// </summary>
    internal static AbstractCatalog<string, string> AbstractCatalogInstance = null;

    /// <summary>
    /// EN: Gets the abstract catalog
    /// CZ: Získá abstraktní katalog
    /// </summary>
    internal AbstractCatalog<string, string> ac => AbstractCatalogInstance;
    #endregion

    #region IFoldersWithSolutions
    /// <summary>
    /// EN: Folders with solutions instance
    /// CZ: Instance složek se solutions
    /// </summary>
    public static IFoldersWithSolutions FoldersWithSolutionsInstance = null;

    /// <summary>
    /// EN: Gets folders with solutions list
    /// CZ: Získá seznam složek se solutions
    /// </summary>
    public FoldersWithSolutionsList fwss => FoldersWithSolutions.fwss;
    #endregion

    /// <summary>
    /// EN: Singleton instance of ApsMainWindow
    /// CZ: Singleton instance ApsMainWindow
    /// </summary>
    public static ApsMainWindow Instance = new ApsMainWindow();

    /// <summary>
    /// EN: Initializes a new instance of ApsMainWindow
    /// CZ: Inicializuje novou instanci ApsMainWindow
    /// </summary>
    public ApsMainWindow()
    {
    }

    /// <summary>
    /// EN: Main window APS interface
    /// CZ: Rozhraní hlavního okna APS
    /// </summary>
    public static IMainWindowAps MainWindowAps = null;

    /// <summary>
    /// EN: Gets or sets whether running in command line mode
    /// CZ: Získá nebo nastaví zda běží v režimu příkazové řádky
    /// </summary>
    public bool cmd { get { return MainWindowAps == null ? true : MainWindowAps.cmd; } set { if (MainWindowAps != null) MainWindowAps.cmd = value; } }

    #region ICsFileFilter
    /// <summary>
    /// EN: C# file filter instance
    /// CZ: Instance filtru C# souborů
    /// </summary>
    public static CsFileFilter CsFileFilterInstance = null;

    /// <summary>
    /// EN: Gets or sets the C# file filter
    /// CZ: Získá nebo nastaví filtr C# souborů
    /// </summary>
    public CsFileFilter csFileFilter { get => CsFileFilterInstance; set => CsFileFilterInstance = value; }
    #endregion
}