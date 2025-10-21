// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode._public.SunamoTextOutputGenerator;

public class InstantSB 
{
    public StringBuilder stringBuilder = new StringBuilder();
    private string _tokensDelimiter;
    public InstantSB(string znak)
    {
        _tokensDelimiter = znak;
    }
    public int Length => stringBuilder.Length;
    public override string ToString()
    {
        string vratit = stringBuilder.ToString();
        return vratit;
    }




    public void AddItem(string var)
    {
        string text = var.ToString();
        if (text != _tokensDelimiter && text != "")
        {
            stringBuilder.Append(text + _tokensDelimiter);
        }
    }
    public void AddRaw(object tab)
    {
        stringBuilder.Append(tab.ToString());
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
        string text = o.ToString();
        if (text != _tokensDelimiter && text != "")
        {
            stringBuilder.Append(text);
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
        stringBuilder.Remove(stringBuilder.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }
    public void Clear()
    {
        stringBuilder.Clear();
    }
}