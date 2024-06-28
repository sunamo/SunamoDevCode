namespace SunamoDevCode;


/// <summary>
/// InstantSB(can specify own delimiter, check whether dont exists)
/// TextBuilder(implements Undo, save to Sb or List)
/// HtmlSB(Same as InstantSB, use br)
/// </summary>
internal class InstantSB //: StringWriter
{
    internal StringBuilder sb = new StringBuilder();
    private string _tokensDelimiter;
    internal InstantSB(string znak)
    {
        _tokensDelimiter = znak;
    }
    internal int Length => sb.Length;
    internal override string ToString()
    {
        string vratit = sb.ToString();
        return vratit;
    }
    /// <summary>
    /// Nep�ipisuje se k celkov�mu v�stupu ,proto vrac� sv�j valstn�.
    /// </summary>
    /// <param name="polo�ky"></param>
    internal void AddItem(string var)
    {
        string s = var.ToString();
        if (s != _tokensDelimiter && s != "")
        {
            sb.Append(s + _tokensDelimiter);
        }
    }
    internal void AddRaw(object tab)
    {
        sb.Append(tab.ToString());
    }
    /// <param name="polozky"></param>
    internal void AddItems(params string[] polozky)
    {
        foreach (var var in polozky)
        {
            AddItem(var);
        }
    }
    /// <summary>
    /// Append without token delimiter
    /// </summary>
    /// <param name="o"></param>
    internal void EndLine(object o)
    {
        string s = o.ToString();
        if (s != _tokensDelimiter && s != "")
        {
            sb.Append(s);
        }
    }
    /// <summary>
    /// Jen vol� metodu AddItem s A1 s NL
    /// </summary>
    /// <param name="p"></param>
    internal void AppendLine(string p)
    {
        EndLine(p + Environment.NewLine);
    }
    internal void AppendLine()
    {
        EndLine(Environment.NewLine);
    }
    internal void RemoveEndDelimiter()
    {
        sb.Remove(sb.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }
    internal void Clear()
    {
        sb.Clear();
    }
}