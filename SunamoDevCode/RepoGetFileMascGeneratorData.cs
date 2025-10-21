// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

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
        var name = 0;
        if (tsTsx == b) name++;
        if (jsJsx == b) name++;
        if (scss == b) name++;
        if (json == b) name++;
        if (yaml == b) name++;
        if (css == b) name++;
        if (cs == b) name++;
        return name;
    }

    public static RepoGetFileMascGeneratorData AllSourceCodes()
    {
        var argument = new RepoGetFileMascGeneratorData();
        argument.AllTo(true);
        return argument;
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
        var stringBuilder = new StringBuilder();
        if (tsTsx) stringBuilder.Append("*.ts;*.tsx;");
        if (jsJsx) stringBuilder.Append("*.js;*.jsx;");
        if (scss) stringBuilder.Append("*.scss;");
        if (json) stringBuilder.Append("*.json;");
        if (yaml) stringBuilder.Append("*.yaml;");
        if (css) stringBuilder.Append("*.css;");
        if (cs) stringBuilder.Append("*.cs;");

        return stringBuilder.ToString();
    }
}