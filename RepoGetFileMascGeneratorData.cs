namespace SunamoDevCode;

/// <summary>
///     Nevím co by bylo lepší
///     Zda explicitně povolovat (zdrojové soubory) nebo explicitně zakazovat (např. obrázky), archívy
///     později zde můžu přidat všechny source_file z AllExtensions
/// </summary>
public class RepoGetFileMascGeneratorData
{
    public bool cs;
    public bool css;
    public bool jsJsx;
    public bool json;
    public bool scss;
    public bool tsTsx;
    public bool yaml;

    public int NumbersOf(bool b)
    {
        var n = 0;
        if (tsTsx == b) n++;
        if (jsJsx == b) n++;
        if (scss == b) n++;
        if (json == b) n++;
        if (yaml == b) n++;
        if (css == b) n++;
        if (cs == b) n++;
        return n;
    }

    public static RepoGetFileMascGeneratorData AllSourceCodes()
    {
        var a = new RepoGetFileMascGeneratorData();
        a.AllTo(true);
        return a;
    }

    public void AllTo(bool v)
    {
        // do it with reflection
        tsTsx = v;
        jsJsx = v;
        scss = v;
        json = v;
        yaml = v;
        css = v;
        cs = v;
    }

    public string Generate()
    {
        var sb = new StringBuilder();
        if (tsTsx) sb.Append("*.ts;*.tsx;");
        if (jsJsx) sb.Append("*.js;*.jsx;");
        if (scss) sb.Append("*.scss;");
        if (json) sb.Append("*.json;");
        if (yaml) sb.Append("*.yaml;");
        if (css) sb.Append("*.css;");
        if (cs) sb.Append("*.cs;");

        return sb.ToString();
    }
}