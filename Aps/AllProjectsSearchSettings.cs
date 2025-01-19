namespace SunamoDevCode.Aps;

public static partial class AllProjectsSearchSettings
{
    static string fileNotTranslate = null;
    //
    //static List<string> dontReplaceReferencesIn = null;
    public static List<string> DontReplaceReferencesIn { get; set; } = null;
    public static
#if ASYNC
    async Task<List<string>>
#else
    List<string>
#endif
 DontReplaceReferencesInLoad(GetFileSettings getFileSettings)
    {
        if (DontReplaceReferencesIn == null)
        {
            var fn = getFileSettings("dontReplaceReferencesIn.txt");
            DontReplaceReferencesIn =
#if ASYNC
    await
#endif
 TF.ReadAllLines(fn);
            SF.RemoveComments(DontReplaceReferencesIn);
        }
        return DontReplaceReferencesIn;
    }
    public static string fileFoldersVsTemplate = "";
    public static CollectionOnDrive _foldersVsTemplate = null;
    public static async Task<CollectionOnDrive> GetFoldersVsTemplate(ILogger logger, GetFileSettings getFileSettings)
    {
        if (_foldersVsTemplate == null)
        {
            fileFoldersVsTemplate = getFileSettings("foldersVsTemplate.txt");
            _foldersVsTemplate = new CollectionOnDrive(logger);
            await _foldersVsTemplate.Load(fileFoldersVsTemplate, false);
        }
        return _foldersVsTemplate;
    }
    public static CollectionOnDrive _filesNotToTranslate = null;
    public static async Task<CollectionOnDrive> GetFilesNotToTranslate(ILogger logger, GetFileSettings getFileSettings)
    {
        if (_filesNotToTranslate == null)
        {
            fileNotTranslate = getFileSettings("notToTranslateFiles.txt");
            _filesNotToTranslate = new CollectionOnDrive(logger);
            await _filesNotToTranslate.Load(fileNotTranslate, false);
        }
        return _filesNotToTranslate;
    }
    #region webProjects
    public static string fileWebProjects = "";
    public static CollectionOnDrive _webProjects = null;
    public static async Task<CollectionOnDrive> GetWebProjects(ILogger logger, GetFileSettings getFileSettings)
    {
        if (_webProjects == null)
        {
            fileWebProjects = getFileSettings("webProjects.txt");
            _webProjects = new CollectionOnDrive(logger);
            await _webProjects.Load(fileWebProjects, false);
        }
        return _webProjects;
    }
    #endregion
    #region webProjects
    public static string fileAddWpfProjectsToSln = "";
    public static CollectionOnDrive _addWpfProjectsToSln = null;
    public static async Task<CollectionOnDrive> GetAddWpfProjectsToSln(ILogger logger, GetFileSettings getFileSettings)
    {
        if (_addWpfProjectsToSln == null)
        {
            fileAddWpfProjectsToSln = getFileSettings("AddWpfProjectsToSln.txt");
            _addWpfProjectsToSln = new CollectionOnDrive(logger);
            await _addWpfProjectsToSln.Load(fileAddWpfProjectsToSln, false);
        }
        return _addWpfProjectsToSln;
    }
    #endregion
    static Dictionary<string, Regex> _webProjectsWildCard = null;
    public static async Task<Dictionary<string, Regex>> GetWebProjectsWildCard(ILogger logger, GetFileSettings getFileSettings)
    {
        if (_webProjectsWildCard == null)
        {
            _webProjectsWildCard = new Dictionary<string, Regex>();
            foreach (var item in await GetWebProjects(logger, getFileSettings))
            {
                _webProjectsWildCard.Add(item, Wildcard.CreateInstance(item));
            }
        }
        return _webProjectsWildCard;
    }
    /// <summary>
    /// Obsahuje od nuly číslované složky ve kterých má vyhledávat, např. 1=D:\Documents\ ale to zda se má vyhledávat je v sectionSearchFoldersChecked
    /// </summary>
    const string sectionSearchFolders = "SearchFolders";
    /// <summary>
    /// Obsahuje od nuly číslované bool hodnoty, index odkazuje na složku v sectionSearchFolders a bool říká zda v této složce se má vyhledávat
    /// </summary>
    const string sectionSearchFoldersChecked = "SearchFoldersChecked";
    public static string PathAutoYes(GetFileData getFileData)
    {
        var pathToTranslate = getFileData("AutoYes.txt");
        return pathToTranslate;
    }
    public static string PathAutoNo(GetFileData getFileData)
    {
        var pathNotToTranslate = getFileData("AutoNo.txt");
        return pathNotToTranslate;
    }
    public static string PathManuallyYes(GetFileData getFileData)
    {
        var pathToTranslate = getFileData("ManuallyYes.txt");
        return pathToTranslate;
    }
    public static string PathManuallyNo(GetFileData getFileData)
    {
        var pathNotToTranslate = getFileData("ManuallyNo.txt");
        return pathNotToTranslate;
    }
    /// <summary>
    /// G zda cesta A1 je v cestách, ve kterých aplikace hledá
    /// </summary>
    /// <param name="path"></param>
    public static bool ExistsSearchFolderByPath(string path)
    {
        path = FS.WithEndSlash(path);
        for (int i = 0; i < 1000; i++)
        {
            string serieS = i.ToString();
            if (!ExistsFolderSearchBySerie(serieS))
            {
                break;
            }
            else
            {
                if (GetSearchFolderNormalized(serieS) == path)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Vrátí všechny cesty ve kterých vyhledávat.
    /// </summary>
    public static List<string> GetAllNormalizedSearchFolders()
    {
        List<string> vr = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            string serieS = i.ToString();
            var result = ExistsFolderSearchBySerie(serieS);
            if (!result)
            {
                break;
            }
            else
            {
                vr.Add(GetSearchFolderNormalized(serieS));
            }
        }
        return vr;
    }
}