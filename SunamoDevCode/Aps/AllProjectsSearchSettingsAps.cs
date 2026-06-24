namespace SunamoDevCode.Aps;

internal static partial class AllProjectsSearchSettings
{
    public static readonly string PathFileSettings = AppPaths.GetFileInStartupPath("settings.ini");

    #region sectionSearchFoldersChecked

    public static bool IsFolderSearchChecked(string seriesIndex)
    {
        return true;
    }

    public static void SetSearchFolderChecked(string seriesIndex, bool isChecked)
    {
    }

    public static bool ExistsFolderSearchBySerie(string seriesIndex)
    {
        return false;
    }

    public static string GetSearchFolderNormalized(string seriesIndex)
    {
        return string.Empty;
    }

    public static int AddFolderSearch(string folderPath)
    {
        return 1;
    }

    #endregion
}
