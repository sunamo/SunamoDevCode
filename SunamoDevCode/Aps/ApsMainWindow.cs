// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps;

public class ApsMainWindow : IMainWindowCsFileFilter, /*IFoldersWithSolutions,*/ IAbstractCatalog<string, string>
{
    #region IAbstractCatalog iac
    internal static AbstractCatalog<string, string> iac = null;
    internal AbstractCatalog<string, string> ac => iac;
    #endregion
    #region IFoldersWithSolutions fws
    public static IFoldersWithSolutions fws = null;
    public FoldersWithSolutionsList fwss => FoldersWithSolutions.fwss;
    #endregion
    public static ApsMainWindow Instance = new ApsMainWindow();
    public ApsMainWindow()
    {
    }
    public static IMainWindowAps mwAps = null;
    public bool cmd { get { return mwAps == null ? true : mwAps.cmd; } set { if (mwAps != null) mwAps.cmd = value; } }
    #region ICsFileFilter iCsFileFilter
    public static CsFileFilter iCsFileFilter = null;
    public CsFileFilter csFileFilter { get => iCsFileFilter; set => iCsFileFilter = value; }
    #endregion
}