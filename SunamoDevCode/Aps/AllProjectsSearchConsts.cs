namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Constants for AllProjectsSearch functionality
/// CZ: Konstanty pro funkcionalitu AllProjectsSearch
/// </summary>
public partial class AllProjectsSearchConsts
{
    /// <summary>
    /// EN: Filename for latest clear temp files operation
    /// CZ: Název souboru pro poslední operaci čištění dočasných souborů
    /// </summary>
    public const string FnLatestClearTempFiles = "LatestClearTempFiles";

    /// <summary>
    /// EN: List of folders to ignore during search
    /// CZ: Seznam složek které se mají ignorovat při vyhledávání
    /// </summary>
    private List<string> foldersIgnore = new List<string> { "Addins", "Settings", "Templates", "Visualizers",
    "ArchitectureExplorer", "Backup Files",  "Code Snippets", "StartPages", "Assemblies", "Blend" };

    /// <summary>
    /// EN: File extensions for code elements
    /// CZ: Přípony souborů pro kódové elementy
    /// </summary>
    public static List<string> ExtCodeElements { get; set; } = new List<string> { ".cs", ".ts", ".dart" };

    /// <summary>
    /// EN: Additional file extensions
    /// CZ: Další přípony souborů
    /// </summary>
    public static List<string> ExtFiles2 { get; set; } = new List<string> { ".ashx", ".aspx", ".xaml", ".cshtml" };

    /// <summary>
    /// EN: Number for APS plugins
    /// CZ: Číslo pro APS pluginy
    /// </summary>
    public const byte ForApsPlugins = 9;
}