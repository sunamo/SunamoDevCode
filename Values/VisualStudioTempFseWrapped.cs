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
        foldersInSolutionToDelete = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInSolutionToDelete, AllStrings.bs);
        foldersInProjectToDelete = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInProjectToDelete, AllStrings.bs);
        foldersAnywhereToDelete = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersAnywhereToDelete, AllStrings.bs);
        #endregion

        #region 2) To keep
        foldersInSolutionToKeep = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInSolutionToKeep, AllStrings.bs);
        foldersInProjectToKeep = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInProjectToKeep, AllStrings.bs);
        foldersAnywhereToKeep = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersAnywhereToKeep, AllStrings.bs);
        #endregion

        #region 3) Downloaded
        foldersInSolutionDownloaded = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInSolutionDownloaded, AllStrings.bs);
        foldersInProjectDownloaded = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersInProjectDownloaded, AllStrings.bs);
        foldersAnywhereDownloaded = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.foldersAnywhereDownloaded, AllStrings.bs);
        #endregion

        filesWeb = _sunamo.SunamoCollections.CA.WrapWith(VisualStudioTempFse.filesWeb, AllStrings.bs);
        aggregate = _sunamo.SunamoCollections.CA.JoinIList(foldersInSolutionToDelete,
foldersInProjectToDelete,
foldersInSolutionDownloaded,
foldersAnywhereToDelete,
foldersInSolutionToKeep,
foldersInProjectToKeep,
foldersInProjectDownloaded, filesWeb);
    }
}
