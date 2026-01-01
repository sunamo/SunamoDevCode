namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Static base class for APS plugins
/// CZ: Statická základní třída pro APS pluginy
/// </summary>
public class ApsPluginStatic
{
    /// <summary>
    /// EN: Gets whether running in command line mode
    /// CZ: Získá zda běží v režimu příkazové řádky
    /// </summary>
    public bool cmd
    {
        get
        {
            return ApsMainWindow.Instance.cmd;
        }
    }

    /// <summary>
    /// EN: Gets folders with solutions list
    /// CZ: Získá seznam složek se solutions
    /// </summary>
    public static FoldersWithSolutionsList fwss
    {
        get
        {
            return FoldersWithSolutions.fwss;
        }
    }

    /// <summary>
    /// EN: Gets or sets the used repository
    /// CZ: Získá nebo nastaví používané repository
    /// </summary>
    public static RepositoryLocal usedRepository
    {
        set => FoldersWithSolutions.usedRepository = value;
        get => FoldersWithSolutions.usedRepository;
    }
}