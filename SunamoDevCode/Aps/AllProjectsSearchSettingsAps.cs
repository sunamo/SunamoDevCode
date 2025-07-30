namespace SunamoDevCode.Aps;

//public static partial class AllProjectsSearchSettings
//{
//    /// <summary>
//    /// Zde jsem chtěl to taky udělat souborem, který by odkazoval na cestu se všemi ostatními soubory, ale všechno ukládám do 1. ini souboru, který je ve stejné lokaci jako binárka
//    /// </summary>
//    public static readonly string pathFileSettings = AppPaths.GetFileInStartupPath("settings.ini");
//    static IniFile ini = new IniFile(pathFileSettings);

//    #region sectionSearchFoldersChecked
//    /// <summary>
//    /// G zda cesta na sérii A1 je zaškrtnutá
//    /// </summary>
//    /// <param name="i"></param>
//    public static bool IsFolderSearchChecked(string i)
//    {
//        var val = ini.IniReadValue(sectionSearchFoldersChecked, i);

//        return val == "True";
//    }

//    /// <summary>
//    /// Nastavím cestu A1 na zaškrtnutí A2
//    /// </summary>
//    /// <param name="i"></param>
//    /// <param name="b"></param>
//    public static void SetSearchFolderChecked(string i, bool b)
//    {
//        ini.IniWriteValue(sectionSearchFoldersChecked, i, b.ToString());
//    }

//    /// <summary>
//    /// Vrátí zda existuje nějaká cesta pod sérií
//    /// </summary>
//    /// <param name="i"></param>
//    public static bool ExistsFolderSearchBySerie(string i)
//    {
//        return ini.IniReadValue(sectionSearchFoldersChecked, i) != "";
//    }

//    /// <summary>
//    /// Vrátí normalizovanou cestu pod indexem A1
//    /// </summary>
//    /// <param name="i"></param>
//    public static string GetSearchFolderNormalized(string i)
//    {
//        return FS.WithEndSlash(ini.IniReadValue(sectionSearchFolders, i.ToString()));
//    }

//    /// <summary>
//    /// Uložím normalizovanou cestu A1 na první volnou sérii a tuto sérii G
//    /// </summary>
//    /// <param name="path"></param>
//    public static int AddFolderSearch(string path)
//    {
//        path = FS.WithEndSlash(path);
//        int i = 0;
//        while (true)
//        {
//            if (GetSearchFolderNormalized(i.ToString()) == "")
//            {
//                ini.IniWriteValue(sectionSearchFolders, i.ToString(), path);
//                break;
//            }
//            i++;
//        }
//        return i;
//    }
//    #endregion
//}

static partial class AllProjectsSearchSettings
{
    public static readonly string pathFileSettings = AppPaths.GetFileInStartupPath("settings.ini");

#pragma warning disable

    #region sectionSearchFoldersChecked
    /// <summary>
    /// G zda cesta na sérii A1 je zaškrtnutá
    /// </summary>
    /// <param name="i"></param>
    public static bool IsFolderSearchChecked(string i)
    {
        return true;
    }

    /// <summary>
    /// Nastavím cestu A1 na zaškrtnutí A2
    /// </summary>
    /// <param name="i"></param>
    /// <param name="b"></param>
    public static void SetSearchFolderChecked(string i, bool b)
    {

    }

    /// <summary>
    /// Vrátí zda existuje nějaká cesta pod sérií
    /// </summary>
    /// <param name="i"></param>
    public static bool ExistsFolderSearchBySerie(string i)
    {
        return false;
    }

    /// <summary>
    /// Vrátí normalizovanou cestu pod indexem A1
    /// </summary>
    /// <param name="i"></param>
    public static string GetSearchFolderNormalized(string i)
    {
        return "";
    }

    /// <summary>
    /// Uložím normalizovanou cestu A1 na první volnou sérii a tuto sérii G
    /// </summary>
    /// <param name="path"></param>
    public static int AddFolderSearch(string path)
    {
        return 1;
    }

#pragma warning restore
    #endregion
}