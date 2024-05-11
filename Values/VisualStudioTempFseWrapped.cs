namespace SunamoDevCode;

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
        foldersInSolutionToDelete = CA.WrapWith(VisualStudioTempFse.foldersInSolutionToDelete, AllStrings.bs);
        foldersInProjectToDelete = CA.WrapWith(VisualStudioTempFse.foldersInProjectToDelete, AllStrings.bs);
        foldersAnywhereToDelete = CA.WrapWith(VisualStudioTempFse.foldersAnywhereToDelete, AllStrings.bs);
        #endregion

        #region 2) To keep
        foldersInSolutionToKeep = CA.WrapWith(VisualStudioTempFse.foldersInSolutionToKeep, AllStrings.bs);
        foldersInProjectToKeep = CA.WrapWith(VisualStudioTempFse.foldersInProjectToKeep, AllStrings.bs);
        foldersAnywhereToKeep = CA.WrapWith(VisualStudioTempFse.foldersAnywhereToKeep, AllStrings.bs);
        #endregion

        #region 3) Downloaded
        foldersInSolutionDownloaded = CA.WrapWith(VisualStudioTempFse.foldersInSolutionDownloaded, AllStrings.bs);
        foldersInProjectDownloaded = CA.WrapWith(VisualStudioTempFse.foldersInProjectDownloaded, AllStrings.bs);
        foldersAnywhereDownloaded = CA.WrapWith(VisualStudioTempFse.foldersAnywhereDownloaded, AllStrings.bs);
        #endregion

        filesWeb = CA.WrapWith(VisualStudioTempFse.filesWeb, AllStrings.bs);
        aggregate = CA.JoinIList(foldersInSolutionToDelete,
foldersInProjectToDelete,
foldersInSolutionDownloaded,
foldersAnywhereToDelete,
foldersInSolutionToKeep,
foldersInProjectToKeep,
foldersInProjectDownloaded, filesWeb);
    }
}
