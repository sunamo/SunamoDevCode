namespace SunamoDevCode.Aps.Values;

/// <summary>
/// Implementation of move to shared folder operations
/// </summary>
public class MoveToShared : IMoveToShared
{
    /// <summary>
    /// Base folder path
    /// </summary>
    public string Folder { get; set; } = null;

    /// <summary>
    /// Sunamo folder path
    /// </summary>
    public string FolderSunamo { get; set; } = null;

    /// <summary>
    /// Sunamo web folder path
    /// </summary>
    public string FolderSunamoWeb { get; set; } = null;

    /// <summary>
    /// Postfix for folder name (e.g., web, task)
    /// </summary>
    public string Postfix { get; set; } = "web";

    /// <summary>
    /// Source solution name
    /// </summary>
    public string SlnFrom { get; set; } = "sunamo";

    /// <summary>
    /// Constructor that initializes folder paths
    /// </summary>
    /// <param name="baseFolder">Base folder path</param>
    public MoveToShared(string baseFolder)
    {
        this.Folder = baseFolder;

        // Projects\\ must be here for sunamo.cz
        FolderSunamo = baseFolder + "Projects\\" + SlnFrom + "\\";
        FolderSunamoWeb = baseFolder + "Projects\\" + "sunamo." + Postfix + "\\";
    }
}
