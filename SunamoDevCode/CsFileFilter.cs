// variables names: ok
namespace SunamoDevCode;

/// <summary>
/// EN: Filter for C# files with configurable filtering rules
/// CZ: Filtr pro C# soubory s konfigurovatelými pravidly filtrování
/// </summary>
/// <remarks>
/// Cannot be derived from FiltersNotTranslateAble to make finding instances of CsFileFilter easier
/// </remarks>
public partial class CsFileFilter : ICsFileFilter
{
    private static readonly FiltersNotTranslateAble filtersNotTranslateable = FiltersNotTranslateAble.Instance;
    private static bool? _returnValue;
    private ContainsArgs containsArgs;
    private EndArgs endArgs;
    /// <summary>
    ///     In default is everything in false
    ///     Call some Set* method
    /// </summary>
    public CsFileFilter()
    {
    }

    private static bool? returnValue
    {
        get => _returnValue;
        set
        {
            if (value.HasValue)
                if (!value.Value)
                {
                }

            _returnValue = value;
        }
    }

    /// <summary>
    /// Gets filtered list of C# files based on configured filtering rules
    /// </summary>
    /// <param name="path">Directory path to search</param>
    /// <param name="searchPattern">File search pattern (e.g., "*.cs")</param>
    /// <param name="searchOption">Search option (TopDirectoryOnly or AllDirectories)</param>
    /// <returns>Filtered list of file paths</returns>
    public List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption)
    {
        var files = Directory.GetFiles(path, searchPattern, searchOption).ToList();
        files.RemoveAll(AllowOnly);
        files.RemoveAll(AllowOnlyContains);
        return files;
    }

    public static bool AllowOnly(string filePath, EndArgs end, ContainsArgs containsArgs)
    {
        var hasEndMatch = false;
        return AllowOnly(filePath, end, containsArgs, ref hasEndMatch, true);
    }

    /// <summary>
    ///     A2 is also for master.designer.cs and aspx.designer.cs
    ///     A2,3 can be null
    /// </summary>
    /// <param name = "filePath">File path to check</param>
    /// <param name = "end">End arguments for filtering</param>
    /// <param name = "containsArgs">Contains arguments for filtering</param>
    /// <param name = "hasEndMatch">Output parameter indicating if end pattern matched</param>
    /// <param name = "isAlsoCheckingEnds">Whether to also check end patterns</param>
    public static bool AllowOnly(string filePath, EndArgs end, ContainsArgs containsArgs, ref bool hasEndMatch, bool isAlsoCheckingEnds)
    {
        returnValue = null;
        if (isAlsoCheckingEnds && end != null)
        {
            hasEndMatch = true;
            if (!end.designerCs && filePath.EndsWith(End.designerCsPp))
                returnValue = false;
            if (!end.xamlCs && filePath.EndsWith(End.xamlCsPp))
                returnValue = false;
            if (!end.sharedCs && filePath.EndsWith(End.sharedCsPp))
                returnValue = false;
            if (!end.iCs && filePath.EndsWith(End.iCsPp))
                returnValue = false;
            if (!end.gICs && filePath.EndsWith(End.gICsPp))
                returnValue = false;
            if (!end.gCs && filePath.EndsWith(End.gCsPp))
                returnValue = false;
            if (!end.tmp && filePath.EndsWith(End.tmpPp))
                returnValue = false;
            if (!end.TMP && filePath.EndsWith(End.TMPPp))
                returnValue = false;
            if (!end.DesignerCs && filePath.EndsWith(End.DesignerCsPp))
                returnValue = false;
            if (!end.notTranslateAble && filePath.EndsWith(End.NotTranslateAblePp))
                returnValue = false;
        }

        if (returnValue.HasValue)
            // Always false
            return returnValue.Value;
        hasEndMatch = false;
        if (containsArgs != null)
        {
            if (!containsArgs.binFp && filePath.Contains(Contains.binFp))
                returnValue = false;
            if (!containsArgs.objFp && filePath.Contains(Contains.objFp))
                returnValue = false;
            if (!containsArgs.tildaRF && filePath.Contains(Contains.tildaRFFp))
                returnValue = false;
        }

        if (returnValue.HasValue)
            // Always false
            return returnValue.Value;
        return true;
    }

    public void Set(EndArgs endArguments, ContainsArgs containsArguments)
    {
        endArgs = endArguments;
        this.containsArgs = containsArguments;
    }

    public void SetDefault()
    {
        //false which not to index, true which to index
        endArgs = new EndArgs(false, true, true, false /*, false*/, false, false, false, false);
        containsArgs = new ContainsArgs(false, false, false /*, false*/);
    }

    /// <summary>
    ///     Gets list of "contains" patterns based on flags
    /// </summary>
    /// <param name = "negate">Whether to negate the flag values</param>
    /// <returns>List of contains patterns</returns>
    public List<string> GetContainsByFlags(bool negate)
    {
        var containsList = new List<string>();
        if (BTS.Is(containsArgs.binFp, negate))
            containsList.Add(Contains.binFp);
        if (BTS.Is(containsArgs.objFp, negate))
            containsList.Add(Contains.objFp);
        if (BTS.Is(containsArgs.tildaRF, negate))
            containsList.Add(Contains.tildaRFFp);
        return containsList;
    }

    public List<string> GetEndingByFlags(bool negate)
    {
        var endingsList = new List<string>();
        if (Is(endArgs.designerCs, negate))
            endingsList.Add(End.designerCsPp);
        if (Is(endArgs.xamlCs, negate))
            endingsList.Add(End.xamlCsPp);
        if (Is(endArgs.xamlCs, negate))
            endingsList.Add(End.xamlCsPp);
        if (Is(endArgs.sharedCs, negate))
            endingsList.Add(End.sharedCsPp);
        if (Is(endArgs.iCs, negate))
            endingsList.Add(End.iCsPp);
        if (Is(endArgs.gICs, negate))
            endingsList.Add(End.gICsPp);
        if (Is(endArgs.gCs, negate))
            endingsList.Add(End.gCsPp);
        if (Is(endArgs.tmp, negate))
            endingsList.Add(End.tmpPp);
        if (Is(endArgs.TMP, negate))
            endingsList.Add(End.TMPPp);
        if (Is(endArgs.DesignerCs, negate))
            endingsList.Add(End.DesignerCsPp);
        if (Is(endArgs.notTranslateAble, negate))
            endingsList.Add(filtersNotTranslateable.NotTranslateAblePp);
        return endingsList;
    }

    private bool Is(bool flagValue, bool negate)
    {
        return BTS.Is(flagValue, negate);
    }

    public static bool AllowOnlyContains(string itemPath, ContainsArgs containsArgs)
    {
        if (!containsArgs.objFp && itemPath.Contains(@"\obj\"))
            return false;
        if (!containsArgs.binFp && itemPath.Contains(@"\bin\"))
            return false;
        if (!containsArgs.tildaRF && itemPath.Contains(@"RF~"))
            return false;
        return true;
    }

    public class Contains
    {
        public const string notTranslateAbleFp = "NotTranslateAble";
        public static string objFp = @"\obj\";
        public static string binFp = @"\bin\";
        public static string tildaRFFp = "~RF";
        private static List<string> unindexablePaths;
        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name = "unindexablePathEnds">List of unindexable path endings</param>
        /// <returns>Configured ContainsArgs instance</returns>
        public static ContainsArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            unindexablePaths = unindexablePathEnds;
            var ea = new ContainsArgs(ContainsPattern(objFp), ContainsPattern(binFp), ContainsPattern(tildaRFFp) /*, ContainsPattern(notTranslateAbleFp)*/);
            return ea;
        }

        private static bool ContainsPattern(string pattern)
        {
            return unindexablePaths.Contains(pattern);
        }
    }

    public class ContainsArgs
    {
        public bool binFp;
        public bool objFp;
        public bool tildaRF;
        /// <summary>
        ///     false which not to index, true which to index
        /// </summary>
        /// <param name = "objFp"></param>
        /// <param name = "binFp"></param>
        /// <param name = "tildaRF"></param>
        /// <param name = "notTranslateAble"></param>
        public ContainsArgs(bool objFp, bool binFp, bool tildaRF)
        {
            this.objFp = objFp;
            this.binFp = binFp;
            this.tildaRF = tildaRF;
        }
    }

    public class End
    {
        public const string NotTranslateAblePp = "NotTranslateAble";
        public const string designerCsPp = ".designer.cs";
        public const string DesignerCsPp = ".Designer.cs";
        public const string xamlCsPp = ".xaml.cs";
        public const string sharedCsPp = "Shared.cs";
        public const string iCsPp = ".i.cs";
        public const string gICsPp = ".g.i.cs";
        public const string gCsPp = ".g.cs";
        public const string tmpPp = ".tmp";
        public const string TMPPp = ".TMP";
        private static List<string> unindexablePaths;
        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name = "unindexablePathEnds">List of unindexable path endings</param>
        /// <returns>Configured EndArgs instance</returns>
        public static EndArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            unindexablePaths = unindexablePathEnds;
            var ea = new EndArgs(ContainsPattern(designerCsPp), ContainsPattern(xamlCsPp), ContainsPattern(sharedCsPp), ContainsPattern(iCsPp) /*, ContainsPattern(gICsPp)*/, ContainsPattern(gCsPp), ContainsPattern(tmpPp), ContainsPattern(TMPPp), ContainsPattern(DesignerCsPp));
            return ea;
        }

        private static bool ContainsPattern(string pattern)
        {
            if (unindexablePaths.Contains(pattern))
            {
                // Really I want to delete it
                unindexablePaths.Remove(pattern);
                return false;
            }

            return true;
        }
    }
}