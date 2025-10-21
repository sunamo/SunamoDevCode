// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Values;

/// <summary>
/// Only folders here - wrapped by bs
/// </summary>
public class VisualStudioTempFseWrapped
{
    #region 1) To delete
    public static List<string> foldersInSolutionToDelete = null;
    public static List<string> foldersInProjectToDelete = null;
    public static List<string> foldersAnywhereToDelete = null;
    #endregion

    #region 2) To keep
    public static List<string> foldersInSolutionToKeep = null;
    public static List<string> foldersInProjectToKeep = null;
    public static List<string> foldersAnywhereToKeep = null;
    #endregion

    #region 3) Downloaded
    public static List<string> foldersInSolutionDownloaded = null;
    public static List<string> foldersInProjectDownloaded = null;
    public static List<string> foldersAnywhereDownloaded = null;
    #endregion

    public static List<string> filesWeb = null;
    public static List<string> aggregate = null;

    public static bool IsToIndexed(string p)
    {
        if (SH.ContainsAtLeastOne(p, aggregate))
        {
            return false;
        }
        return true;
    }

    static VisualStudioTempFseWrapped()
    {
        #region 1) To delete
        foldersInSolutionToDelete = CA.WrapWith(VisualStudioTempFse.foldersInSolutionToDelete, "\"");
        foldersInProjectToDelete = CA.WrapWith(VisualStudioTempFse.foldersInProjectToDelete, "\"");
        foldersAnywhereToDelete = CA.WrapWith(VisualStudioTempFse.foldersAnywhereToDelete, "\"");
        #endregion

        #region 2) To keep
        foldersInSolutionToKeep = CA.WrapWith(VisualStudioTempFse.foldersInSolutionToKeep, "\"");
        foldersInProjectToKeep = CA.WrapWith(VisualStudioTempFse.foldersInProjectToKeep, "\"");
        foldersAnywhereToKeep = CA.WrapWith(VisualStudioTempFse.foldersAnywhereToKeep, "\"");
        #endregion

        #region 3) Downloaded
        foldersInSolutionDownloaded = CA.WrapWith(VisualStudioTempFse.foldersInSolutionDownloaded, "\"");
        foldersInProjectDownloaded = CA.WrapWith(VisualStudioTempFse.foldersInProjectDownloaded, "\"");
        foldersAnywhereDownloaded = CA.WrapWith(VisualStudioTempFse.foldersAnywhereDownloaded, "\"");
        #endregion

        filesWeb = CA.WrapWith(VisualStudioTempFse.filesWeb, "\"");
        aggregate = CA.JoinIList(foldersInSolutionToDelete,
foldersInProjectToDelete,
foldersInSolutionDownloaded,
foldersAnywhereToDelete,
foldersInSolutionToKeep,
foldersInProjectToKeep,
foldersInProjectDownloaded, filesWeb);
    }
}
