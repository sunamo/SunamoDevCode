namespace SunamoDevCode;

/// <summary>
/// EN: Data for generating file mask for repository. Unsure what would be better - explicitly allowing (source files) or explicitly forbidding (e.g., images, archives). Later can add all source_file from AllExtensions here.
/// CZ: Data pro generování masky souborů pro repozitář. Nevím co by bylo lepší - zda explicitně povolovat (zdrojové soubory) nebo explicitně zakazovat (např. obrázky), archívy. Později zde můžu přidat všechny source_file z AllExtensions.
/// </summary>
public class RepoGetFileMascGeneratorData
{
    /// <summary>
    /// Whether to include C# (.cs) files.
    /// </summary>
    public bool Cs { get; set; }
    /// <summary>
    /// Whether to include CSS (.css) files.
    /// </summary>
    public bool Css { get; set; }
    /// <summary>
    /// Whether to include JavaScript (.js, .jsx) files.
    /// </summary>
    public bool JsJsx { get; set; }
    /// <summary>
    /// Whether to include JSON (.json) files.
    /// </summary>
    public bool Json { get; set; }
    /// <summary>
    /// Whether to include SCSS (.scss) files.
    /// </summary>
    public bool Scss { get; set; }
    /// <summary>
    /// Whether to include TypeScript (.ts, .tsx) files.
    /// </summary>
    public bool TsTsx { get; set; }
    /// <summary>
    /// Whether to include YAML (.yaml) files.
    /// </summary>
    public bool Yaml { get; set; }

    /// <summary>
    /// Counts how many file type flags match the specified value.
    /// </summary>
    /// <param name="value">Value to compare each flag against.</param>
    /// <returns>Number of flags matching the value.</returns>
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

    /// <summary>
    /// Creates a new instance with all source code file types enabled.
    /// </summary>
    /// <returns>A new instance with all flags set to true.</returns>
    public static RepoGetFileMascGeneratorData AllSourceCodes()
    {
        var argument = new RepoGetFileMascGeneratorData();
        argument.AllTo(true);
        return argument;
    }

    /// <summary>
    /// Sets all file type flags to the specified value.
    /// </summary>
    /// <param name="value">Value to set all flags to.</param>
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

    /// <summary>
    /// Generates a semicolon-separated file mask string based on enabled file types.
    /// </summary>
    /// <returns>File mask string (e.g., "*.ts;*.tsx;*.cs;").</returns>
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