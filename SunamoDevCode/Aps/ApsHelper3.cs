namespace SunamoDevCode.Aps;

/// <summary>
/// EN: Helper class for APS (AllProjectsSearch) functionality - part 3
/// CZ: Pomocná třída pro APS (AllProjectsSearch) funkcionalitu - část 3
/// </summary>
public partial class ApsHelper : ApsPluginStatic
{
    /// <summary>
    /// EN: Gets files for git excluding temporary files
    /// CZ: Získá soubory pro git s vyloučením dočasných souborů
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="folder">Folder to search in</param>
    /// <param name="typedExt">File extension to search for</param>
    public List<string> GetFilesForGitExcludeTemporary(ILogger logger, string folder, string typedExt)
    {
        var files = FSGetFiles.GetFiles(logger, folder, FS.MascFromExtension(typedExt), SearchOption.AllDirectories);
        SunamoDevCodeHelper.RemoveTemporaryFilesVS(files);
        return files;
    }

    /// <summary>
    /// EN: Converts solution names to full paths
    /// CZ: Převede názvy solution na úplné cesty
    /// </summary>
    /// <param name="slnNames">List of solution names as key-value pairs</param>
    public static List<string> FullPathsFromSolutionsNames(List<KeyValuePair<string, string>> slnNames)
    {
        List<string> result = new List<string>();
        foreach (var item in slnNames)
        {
            var sln = SolutionsIndexerHelper.SolutionWithName(item.Value);
            result.Add(sln.FullPathFolder);
        }

        return result;
    }
}