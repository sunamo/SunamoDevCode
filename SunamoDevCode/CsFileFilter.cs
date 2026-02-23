namespace SunamoDevCode;

/// <summary>
/// EN: Filter for C# files with configurable filtering rules
/// CZ: Filtr pro C# soubory s konfigurovatelными pravidly filtrování
/// </summary>
/// <remarks>
/// Cannot be derived from FiltersNotTranslateAble to make finding instances of CsFileFilter easier
/// </remarks>
public partial class CsFileFilter : ICsFileFilter
{
    private static readonly FiltersNotTranslateAble filtersNotTranslateable = FiltersNotTranslateAble.Instance;
    private static bool? _returnValue;
    private ContainsArgs? containsArgs;
    private EndArgs? endArgs;
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
    /// Gets filtered list of C# files based on configured filtering rules.
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

    /// <summary>
    /// Checks if a file path is allowed based on end and contains filtering rules.
    /// </summary>
    /// <param name="filePath">File path to check</param>
    /// <param name="end">End arguments for filtering</param>
    /// <param name="containsArgs">Contains arguments for filtering</param>
    /// <returns>True if the file is allowed, false if it should be filtered out</returns>
    public static bool AllowOnly(string filePath, EndArgs end, ContainsArgs containsArgs)
    {
        var hasEndMatch = false;
        return AllowOnly(filePath, end, containsArgs, ref hasEndMatch, true);
    }

    /// <summary>
    ///     A2 is also for master.designer.cs and aspx.designer.cs
    ///     A2,3 can be null
    /// </summary>
    /// <param name="filePath">File path to check</param>
    /// <param name="end">End arguments for filtering</param>
    /// <param name="containsArgs">Contains arguments for filtering</param>
    /// <param name="hasEndMatch">Output parameter indicating if end pattern matched</param>
    /// <param name="isAlsoCheckingEnds">Whether to also check end patterns</param>
    /// <returns>True if the file is allowed, false if it should be filtered out</returns>
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

    /// <summary>
    /// Sets the end and contains arguments for filtering.
    /// </summary>
    /// <param name="endArguments">End arguments for filtering</param>
    /// <param name="containsArguments">Contains arguments for filtering</param>
    public void Set(EndArgs endArguments, ContainsArgs containsArguments)
    {
        endArgs = endArguments;
        this.containsArgs = containsArguments;
    }

    /// <summary>
    /// Sets the default filtering configuration with standard exclusion patterns.
    /// </summary>
    public void SetDefault()
    {
        //false which not to index, true which to index
        endArgs = new EndArgs(false, true, true, false /*, false*/, false, false, false, false);
        containsArgs = new ContainsArgs(false, false, false /*, false*/);
    }

    /// <summary>
    ///     Gets list of "contains" patterns based on flags
    /// </summary>
    /// <param name="negate">Whether to negate the flag values</param>
    /// <returns>List of contains patterns</returns>
    public List<string> GetContainsByFlags(bool negate)
    {
        var containsList = new List<string>();
        if (BTS.Is(containsArgs!.binFp, negate))
            containsList.Add(Contains.binFp);
        if (BTS.Is(containsArgs.objFp, negate))
            containsList.Add(Contains.objFp);
        if (BTS.Is(containsArgs.tildaRF, negate))
            containsList.Add(Contains.tildaRFFp);
        return containsList;
    }

    /// <summary>
    /// Gets list of file ending patterns based on configured flags.
    /// </summary>
    /// <param name="negate">Whether to negate the flag values</param>
    /// <returns>List of file ending patterns</returns>
    public List<string> GetEndingByFlags(bool negate)
    {
        var endingsList = new List<string>();
        if (Is(endArgs!.designerCs, negate))
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

    /// <summary>
    /// Checks if a file path is allowed based on contains filtering rules only.
    /// </summary>
    /// <param name="itemPath">File path to check</param>
    /// <param name="containsArgs">Contains arguments for filtering</param>
    /// <returns>True if the file is allowed, false if it should be filtered out</returns>
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

    /// <summary>
    /// Constants and helper methods for path-contains based filtering patterns.
    /// </summary>
    public class Contains
    {
        /// <summary>
        /// Pattern for files containing "NotTranslateAble" in the path.
        /// </summary>
        public const string notTranslateAbleFp = "NotTranslateAble";
        /// <summary>
        /// Pattern for files in the obj directory.
        /// </summary>
        public static string objFp = @"\obj\";
        /// <summary>
        /// Pattern for files in the bin directory.
        /// </summary>
        public static string binFp = @"\bin\";
        /// <summary>
        /// Pattern for files with tilda RF marker.
        /// </summary>
        public static string tildaRFFp = "~RF";
        private static List<string> unindexablePaths = null!;
        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name="unindexablePathEnds">List of unindexable path endings</param>
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

    /// <summary>
    /// Arguments controlling which path-contains patterns to include or exclude during filtering.
    /// </summary>
    public class ContainsArgs
    {
        /// <summary>
        /// Whether to include files from bin directories.
        /// </summary>
        public bool binFp;
        /// <summary>
        /// Whether to include files from obj directories.
        /// </summary>
        public bool objFp;
        /// <summary>
        /// Whether to include files with tilda RF marker.
        /// </summary>
        public bool tildaRF;
        /// <summary>
        ///     false which not to index, true which to index
        /// </summary>
        /// <param name="objFp">Whether to include obj directory files</param>
        /// <param name="binFp">Whether to include bin directory files</param>
        /// <param name="tildaRF">Whether to include tilda RF files</param>
        public ContainsArgs(bool objFp, bool binFp, bool tildaRF)
        {
            this.objFp = objFp;
            this.binFp = binFp;
            this.tildaRF = tildaRF;
        }
    }

    /// <summary>
    /// Constants and helper methods for file-ending based filtering patterns.
    /// </summary>
    public class End
    {
        /// <summary>
        /// Pattern for NotTranslateAble files.
        /// </summary>
        public const string NotTranslateAblePp = "NotTranslateAble";
        /// <summary>
        /// Pattern for .designer.cs files (lowercase).
        /// </summary>
        public const string designerCsPp = ".designer.cs";
        /// <summary>
        /// Pattern for .Designer.cs files (PascalCase).
        /// </summary>
        public const string DesignerCsPp = ".Designer.cs";
        /// <summary>
        /// Pattern for .xaml.cs code-behind files.
        /// </summary>
        public const string xamlCsPp = ".xaml.cs";
        /// <summary>
        /// Pattern for Shared.cs files.
        /// </summary>
        public const string sharedCsPp = "Shared.cs";
        /// <summary>
        /// Pattern for .i.cs intermediate files.
        /// </summary>
        public const string iCsPp = ".i.cs";
        /// <summary>
        /// Pattern for .g.i.cs generated intermediate files.
        /// </summary>
        public const string gICsPp = ".g.i.cs";
        /// <summary>
        /// Pattern for .g.cs generated files.
        /// </summary>
        public const string gCsPp = ".g.cs";
        /// <summary>
        /// Pattern for .tmp temporary files (lowercase).
        /// </summary>
        public const string tmpPp = ".tmp";
        /// <summary>
        /// Pattern for .TMP temporary files (uppercase).
        /// </summary>
        public const string TMPPp = ".TMP";
        private static List<string> unindexablePaths = null!;
        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name="unindexablePathEnds">List of unindexable path endings</param>
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