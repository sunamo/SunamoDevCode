// Instance variables refactored according to C# conventions
namespace SunamoDevCode;

/// <summary>
///     Cant be derived from FiltersNotTranslateAble because easy of finding instances of CsFileFilter
/// </summary>
public class CsFileFilter : ICsFileFilter
{
    private static readonly FiltersNotTranslateAble filtersNotTranslateable = FiltersNotTranslateAble.Instance;

    private static bool? _rv;
    private ContainsArgs containsArgs;

    private EndArgs endArgs;

    /// <summary>
    ///     In default is everything in false
    ///     Call some Set* method
    /// </summary>
    public CsFileFilter()
    {
    }

    private static bool? rv
    {
        get => _rv;
        set
        {
            if (value.HasValue)
                if (!value.Value)
                {
                }

            _rv = value;
        }
    }

    public List<string> GetFilesFiltered(string searchPath, string fileMask, SearchOption searchOption)
    {
        var filesList = Directory.GetFiles(searchPath, fileMask, searchOption).ToList();

        filesList.RemoveAll(AllowOnly);
        filesList.RemoveAll(AllowOnlyContains);

        return filesList;
    }

    public static bool AllowOnly(string item, EndArgs end, ContainsArgs containsArgs)
    {
        var end2 = false;
        return AllowOnly(item, end, containsArgs, ref end2, true);
    }

    /// <summary>
    ///     A2 is also for master.designer.cs and aspx.designer.cs
    ///     A2,3 can be null
    /// </summary>
    /// <param name="item"></param>
    /// <param name="designerCs"></param>
    /// <param name="xamlCs"></param>
    /// <param name="sharedCs"></param>
    public static bool AllowOnly(string item, EndArgs end, ContainsArgs containsArgs, ref bool end2, bool alsoEnds)
    {
        rv = null;

        if (alsoEnds && end != null)
        {
            end2 = true;


            if (!end.designerCs && item.EndsWith(End.designerCsPp)) rv = false;
            if (!end.xamlCs && item.EndsWith(End.xamlCsPp)) rv = false;
            if (!end.sharedCs && item.EndsWith(End.sharedCsPp)) rv = false;
            if (!end.iCs && item.EndsWith(End.iCsPp)) rv = false;
            if (!end.gICs && item.EndsWith(End.gICsPp)) rv = false;
            if (!end.gCs && item.EndsWith(End.gCsPp)) rv = false;
            if (!end.tmp && item.EndsWith(End.tmpPp)) rv = false;
            if (!end.TMP && item.EndsWith(End.TMPPp)) rv = false;
            if (!end.DesignerCs && item.EndsWith(End.DesignerCsPp)) rv = false;
            if (!end.notTranslateAble && item.EndsWith(End.NotTranslateAblePp)) rv = false;
        }


        if (rv.HasValue)
            // Always false
            return rv.Value;

        end2 = false;

        if (containsArgs != null)
        {
            if (!containsArgs.binFp && item.Contains(Contains.binFp)) rv = false;

            if (!containsArgs.objFp && item.Contains(Contains.objFp)) rv = false;

            if (!containsArgs.tildaRF && item.Contains(Contains.tildaRFFp)) rv = false;
        }


        if (rv.HasValue)
            // Always false
            return rv.Value;

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

        endArgs = new EndArgs(false, true, true, false/*, false*/, false, false, false, false);
        containsArgs = new ContainsArgs(false, false, false /*, false*/);
    }

    /// <summary>
    ///     A1 = negate
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public List<string> GetContainsByFlags(bool negate)
    {
        var containsList = new List<string>();
        if (BTS.Is(containsArgs.binFp, negate)) containsList.Add(Contains.binFp);
        if (BTS.Is(containsArgs.objFp, negate)) containsList.Add(Contains.objFp);
        if (BTS.Is(containsArgs.tildaRF, negate)) containsList.Add(Contains.tildaRFFp);


        return containsList;
    }

    public List<string> GetEndingByFlags(bool negate)
    {
        var endingsList = new List<string>();
        if (Is(endArgs.designerCs, negate)) endingsList.Add(End.designerCsPp);
        if (Is(endArgs.xamlCs, negate)) endingsList.Add(End.xamlCsPp);
        if (Is(endArgs.xamlCs, negate)) endingsList.Add(End.xamlCsPp);
        if (Is(endArgs.sharedCs, negate)) endingsList.Add(End.sharedCsPp);
        if (Is(endArgs.iCs, negate)) endingsList.Add(End.iCsPp);
        if (Is(endArgs.gICs, negate)) endingsList.Add(End.gICsPp);
        if (Is(endArgs.gCs, negate)) endingsList.Add(End.gCsPp);
        if (Is(endArgs.tmp, negate)) endingsList.Add(End.tmpPp);
        if (Is(endArgs.TMP, negate)) endingsList.Add(End.TMPPp);
        if (Is(endArgs.DesignerCs, negate)) endingsList.Add(End.DesignerCsPp);
        if (Is(endArgs.notTranslateAble, negate)) endingsList.Add(filtersNotTranslateable.NotTranslateAblePp);


        return endingsList;
    }

    private bool Is(bool temporaryFlag, bool negate)
    {
        return BTS.Is(temporaryFlag, negate);
    }

    #region Take by method

    public static bool AllowOnlyContains(string itemPath, ContainsArgs containsArgs)
    {
        if (!containsArgs.objFp && itemPath.Contains(@"\obj\")) return false;
        if (!containsArgs.binFp && itemPath.Contains(@"\bin\")) return false;
        if (!c.tildaRF && i.Contains(@"RF~")) return false;

        return true;
    }

    #endregion

    public class Contains
    {
        public const string notTranslateAbleFp = "NotTranslateAble";
        public static string objFp = @"\obj\";
        public static string binFp = @"\bin\";
        public static string tildaRFFp = "~RF";

        public static List<string> u;

        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name="unindexablePathEnds"></param>
        /// <returns></returns>
        public static ContainsArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            u = unindexablePathEnds;
            var ea = new ContainsArgs(c(objFp), c(binFp), c(tildaRFFp) /*, c(notTranslateAbleFp)*/);
            return ea;
        }

        private static bool c(string k)
        {
            return u.Contains(k);
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
        /// <param name="objFp"></param>
        /// <param name="binFp"></param>
        /// <param name="tildaRF"></param>
        /// <param name="notTranslateAble"></param>
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

        public static List<string> u;

        /// <summary>
        ///     Into A1 is inserting copy to leave only unindexed
        /// </summary>
        /// <param name="unindexablePathEnds"></param>
        /// <returns></returns>
        public static EndArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            u = unindexablePathEnds;
            var x = c(xamlCsPp);

            var ea = new EndArgs(c(designerCsPp), x, c(sharedCsPp), c(iCsPp)/*, c(gICsPp)*/, c(gCsPp), c(tmpPp), c(TMPPp),
                c(DesignerCsPp));
            return ea;
        }

        private static bool c(string k)
        {
            if (u.Contains(k))
            {
                // Really I want to delete it
                u.Remove(k);
                return false;
            }

            return true;
        }
    }

    public class EndArgs
    {
        public bool designerCs;
        public bool DesignerCs;
        public bool gCs;
        public bool gICs;
        public bool iCs;
        public bool notTranslateAble;
        public bool sharedCs;
        public bool tmp;
        public bool TMP;
        public bool xamlCs;

        /// <summary>
        ///     false which not to index, true which to index
        /// </summary>
        /// <param name="designerCs"></param>
        /// <param name="xamlCs"></param>
        /// <param name="sharedCs"></param>
        /// <param name="iCs"></param>
        /// <param name="gICs"></param>
        /// <param name="gCs"></param>
        /// <param name="tmp"></param>
        /// <param name="TMP"></param>
        /// <param name="DesignerCs"></param>
        public EndArgs(bool designerCs, bool xamlCs, bool sharedCs, bool iCs/*, bool gICs*/, bool gCs, bool tmp, bool TMP,
            bool DesignerCs)
        {
            this.designerCs = designerCs;
            this.xamlCs = xamlCs;
            this.sharedCs = sharedCs;
            this.iCs = iCs;
            this.gCs = gCs;
            this.tmp = tmp;
            this.TMP = TMP;
            this.DesignerCs = DesignerCs;
        }
    }


    #region Take by class variables

    public bool AllowOnly(string item)
    {
        return AllowOnly(item, true);
    }

    public bool AllowOnly(string item, bool alsoEnds)
    {
        var end2 = true;
        return !AllowOnly(item, e, c, ref end2, alsoEnds);
    }

    public bool AllowOnlyContains(string i)
    {
        return !AllowOnlyContains(i, c);
    }

    #endregion
}