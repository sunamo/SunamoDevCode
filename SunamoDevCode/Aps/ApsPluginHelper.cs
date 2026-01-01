namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Helper class for APS plugins
/// CZ: Pomocná třída pro APS pluginy
/// </summary>
public partial class ApsPluginHelper
{
    /// <summary>
    /// EN: Singleton instance of ApsPluginHelper
    /// CZ: Singleton instance ApsPluginHelper
    /// </summary>
    public static ApsPluginHelper Instance = new ApsPluginHelper();

    /// <summary>
    /// EN: Private constructor for singleton pattern
    /// CZ: Soukromý konstruktor pro singleton pattern
    /// </summary>
    private ApsPluginHelper()
    {
    }
}