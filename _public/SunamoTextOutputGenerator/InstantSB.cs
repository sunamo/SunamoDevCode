namespace SunamoDevCode._public.SunamoTextOutputGenerator;

public class InstantSB 
{
    public StringBuilder sb = new StringBuilder();
    private string _tokensDelimiter;
    public InstantSB(string znak)
    {
        _tokensDelimiter = znak;
    }
    public int Length => sb.Length;
    public override string ToString()
    {
        string vratit = sb.ToString();
        return vratit;
    }
    
    
    
    
    public void AddItem(string var)
    {
        string s = var.ToString();
        if (s != _tokensDelimiter && s != "")
        {
            sb.Append(s + _tokensDelimiter);
        }
    }
    public void AddRaw(object tab)
    {
        sb.Append(tab.ToString());
    }
    
    public void AddItems(params string[] polozky)
    {
        foreach (var var in polozky)
        {
            AddItem(var);
        }
    }
    
    
    
    
    public void EndLine(object o)
    {
        string s = o.ToString();
        if (s != _tokensDelimiter && s != "")
        {
            sb.Append(s);
        }
    }
    
    
    
    
    public void AppendLine(string p)
    {
        EndLine(p + Environment.NewLine);
    }
    public void AppendLine()
    {
        EndLine(Environment.NewLine);
    }
    public void RemoveEndDelimiter()
    {
        sb.Remove(sb.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }
    public void Clear()
    {
        sb.Clear();
    }
}