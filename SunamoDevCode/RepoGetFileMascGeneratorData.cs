// variables names: ok
namespace SunamoDevCode;

/// <summary>
/// EN: Data for generating file mask for repository. Unsure what would be better - explicitly allowing (source files) or explicitly forbidding (e.g., images, archives). Later can add all source_file from AllExtensions here.
/// CZ: Data pro generování masky souborů pro repozitář. Nevím co by bylo lepší - zda explicitně povolovat (zdrojové soubory) nebo explicitně zakazovat (např. obrázky), archívy. Později zde můžu přidat všechny source_file z AllExtensions.
/// </summary>
public class RepoGetFileMascGeneratorData
{
    public bool Cs { get; set; }
    public bool Css { get; set; }
    public bool JsJsx { get; set; }
    public bool Json { get; set; }
    public bool Scss { get; set; }
    public bool TsTsx { get; set; }
    public bool Yaml { get; set; }

    public int NumbersOf(bool value)
    {
        var count = 0;
        if (TsTsx == value) count++;
        if (JsJsx == value) count++;
        if (Scss == value) count++;
        if (Json == value) count++;
        if (Yaml == value) count++;
        if (Css == value) count++;
        if (Cs == value) count++;
        return count;
    }

    public static RepoGetFileMascGeneratorData AllSourceCodes()
    {
        var argument = new RepoGetFileMascGeneratorData();
        argument.AllTo(true);
        return argument;
    }

    public void AllTo(bool value)
    {
        // do it with reflection
        TsTsx = value;
        JsJsx = value;
        Scss = value;
        Json = value;
        Yaml = value;
        Css = value;
        Cs = value;
    }

    public string Generate()
    {
        var stringBuilder = new StringBuilder();
        if (TsTsx) stringBuilder.Append("*.ts;*.tsx;");
        if (JsJsx) stringBuilder.Append("*.js;*.jsx;");
        if (Scss) stringBuilder.Append("*.scss;");
        if (Json) stringBuilder.Append("*.json;");
        if (Yaml) stringBuilder.Append("*.yaml;");
        if (Css) stringBuilder.Append("*.css;");
        if (Cs) stringBuilder.Append("*.cs;");

        return stringBuilder.ToString();
    }
}