namespace SunamoDevCode._sunamo.SunamoTextOutputGenerator;

/// <summary>
/// In Comparing
/// </summary>
internal class TextOutputGenerator //: ITextOutputGenerator
{
    private readonly static string s_znakNadpisu = "*";
    // při převádění na nugety jsem to změnil na TextBuilderDC sb = TextBuilder.Create();
    // ale asi to byla blbost, teď mám v _sunamo Create() která je ale null místo abych použil ctor
    // takže vracím nazpět.
    //internal TextBuilder sb = new TextBuilder();
    internal StringBuilder sb = new StringBuilder();
    //internal string prependEveryNoWhite
    //{
    //    get => sb.prependEveryNoWhite;
    //    set => sb.prependEveryNoWhite = value;
    //}
    #region Static texts
        #endregion
    #region Templates
        #endregion
    #region AppendLine
    internal void AppendLine(StringBuilder text)
    {
        sb.AppendLine(text.ToString());
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Append(string text)
    {
        sb.Append(text);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AppendLine(string text)
    {
        sb.AppendLine(text);
    }
    #endregion
    #region Other adding methods
    internal void Header(string v)
    {
        sb.AppendLine();
        AppendLine(v);
        sb.AppendLine();
    }
    #endregion
    public override string ToString()
    {
        var ts = sb.ToString();
        return ts;
    }
    #region List
    internal void ListSB(StringBuilder onlyStart, string v)
    {
        Header(v);
        AppendLine(onlyStart);
    }
        internal void List<Value>(IList<Value> files1, string deli = "\r\n", string whenNoEntries = "")
    {
        if (files1.Count == 0)
        {
            sb.AppendLine(whenNoEntries);
        }
        else
        {
            foreach (var item in files1)
            {
                Append(item.ToString() + deli);
            }
            //sb.AppendLine();
        }
    }
        internal void List(IList<string> files1, string header)
    {
        List<string, string>(files1, header, new TextOutputGeneratorArgs { headerWrappedEmptyLines = true, insertCount = false });
    }
    /// <summary>
    /// Use DictionaryHelper.CategoryParser
    /// </summary>
    /// <typeparam name="Header"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="files1"></param>
    /// <param name="header"></param>
    /// <param name="a"></param>
    internal void List<Header, Value>(IList<Value> files1, Header header, TextOutputGeneratorArgs a) where Header : IEnumerable<char>
    {
        if (a.insertCount)
        {
            //throw new Exception("later");
            //header = (Header)((IList<char>)CA.JoinIList<char>(header, " (" + files1.Count() + ")"));
        }
        if (a.headerWrappedEmptyLines)
        {
            sb.AppendLine();
        }
        sb.AppendLine(header + ":");
        if (a.headerWrappedEmptyLines)
        {
            sb.AppendLine();
        }
        List(files1, a.delimiter, a.whenNoEntries);
    }
    #endregion
    #region Paragraph
    internal void Paragraph(StringBuilder wrongNumberOfParts, string header)
    {
        string text = wrongNumberOfParts.ToString().Trim();
        Paragraph(text, header);
    }
    /// <summary>
    /// For ordinary text use Append*
    /// </summary>
    /// <param name="text"></param>
    /// <param name="header"></param>
    internal void Paragraph(string text, string header)
    {
        if (text != string.Empty)
        {
            sb.AppendLine(header + ":");
            sb.AppendLine(text);
            sb.AppendLine();
        }
    }
    #endregion
    #region Dictionary
    internal void Dictionary(Dictionary<string, List<string>> ls)
    {
        foreach (var item in ls)
        {
            List(item.Value, item.Key);
        }
    }
            #endregion
}
