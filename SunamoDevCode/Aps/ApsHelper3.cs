namespace SunamoDevCode.Aps;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class ApsHelper : ApsPluginStatic
{
    public List<string> GetFilesForGitExcludeTemporary(ILogger logger, string folder, string typedExt)
    {
        var files = FSGetFiles.GetFiles(logger, folder, FS.MascFromExtension(typedExt), SearchOption.AllDirectories);
        SunamoDevCodeHelper.RemoveTemporaryFilesVS(files);
        //list = CA.Prepend("\\", VisualStudioTempFse.filesInSolutionToDelete);
        //CA.RemoveWhichContains(files, list, true);
        //list = CA.Prepend("\\", VisualStudioTempFse.filesInProjectToDelete);
        //CA.RemoveWhichContains(files, list, true);
        return files;
    }

    public static List<string> FullPathsFromSolutionsNames(List<KeyValuePair<string, string>> slnNames)
    {
        List<string> result = new List<string>();
        foreach (var item in slnNames)
        {
            var sln = SolutionsIndexerHelper.SolutionWithName(item.Value);
            result.Add(sln.fullPathFolder);
        }

        return result;
    }
}