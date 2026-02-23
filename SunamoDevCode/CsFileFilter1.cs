namespace SunamoDevCode;

/// <summary>
///     Cant be derived from FiltersNotTranslateAble because easy of finding instances of CsFileFilter
/// </summary>
public partial class CsFileFilter : ICsFileFilter
{
    /// <summary>
    /// Arguments controlling which file endings to include or exclude during filtering.
    /// </summary>
    public class EndArgs
    {
        /// <summary>
        /// Whether to include .designer.cs files (lowercase).
        /// </summary>
        public bool designerCs;
        /// <summary>
        /// Whether to include .Designer.cs files (uppercase D).
        /// </summary>
        public bool DesignerCs;
        /// <summary>
        /// Whether to include .g.cs generated files.
        /// </summary>
        public bool gCs;
        /// <summary>
        /// Whether to include .g.i.cs generated intermediate files.
        /// </summary>
        public bool gICs;
        /// <summary>
        /// Whether to include .i.cs intermediate files.
        /// </summary>
        public bool iCs;
        /// <summary>
        /// Whether to include files marked as not translatable.
        /// </summary>
        public bool notTranslateAble;
        /// <summary>
        /// Whether to include .shared.cs files.
        /// </summary>
        public bool sharedCs;
        /// <summary>
        /// Whether to include .tmp files (lowercase).
        /// </summary>
        public bool tmp;
        /// <summary>
        /// Whether to include .TMP files (uppercase).
        /// </summary>
        public bool TMP;
        /// <summary>
        /// Whether to include .xaml.cs code-behind files.
        /// </summary>
        public bool xamlCs;
        /// <summary>
        ///     false which not to index, true which to index
        /// </summary>
        /// <param name = "designerCs"></param>
        /// <param name = "xamlCs"></param>
        /// <param name = "sharedCs"></param>
        /// <param name = "iCs"></param>
        /// <param name = "gCs"></param>
        /// <param name = "tmp"></param>
        /// <param name = "TMP"></param>
        /// <param name = "DesignerCs"></param>
        public EndArgs(bool designerCs, bool xamlCs, bool sharedCs, bool iCs /*, bool gICs*/, bool gCs, bool tmp, bool TMP, bool DesignerCs)
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

    /// <summary>
    /// Checks whether the given file path passes all filter criteria including both contains and end checks.
    /// </summary>
    /// <param name="filePath">File path to evaluate.</param>
    /// <returns>True if the file is allowed by the filter.</returns>
    public bool AllowOnly(string filePath)
    {
        return AllowOnly(filePath, true);
    }

    /// <summary>
    /// Checks whether the given file path passes filter criteria, optionally including end-of-path checks.
    /// </summary>
    /// <param name="filePath">File path to evaluate.</param>
    /// <param name="isAlsoCheckingEnds">Whether to also check file path endings.</param>
    /// <returns>True if the file is allowed by the filter.</returns>
    public bool AllowOnly(string filePath, bool isAlsoCheckingEnds)
    {
        var hasEndMatch = true;
        return !AllowOnly(filePath, endArgs!, containsArgs!, ref hasEndMatch, isAlsoCheckingEnds);
    }

    /// <summary>
    /// Checks whether the given file path passes the contains-only filter criteria.
    /// </summary>
    /// <param name="itemPath">File path to evaluate.</param>
    /// <returns>True if the file is allowed by the contains filter.</returns>
    public bool AllowOnlyContains(string itemPath)
    {
        return !AllowOnlyContains(itemPath, containsArgs!);
    }
}