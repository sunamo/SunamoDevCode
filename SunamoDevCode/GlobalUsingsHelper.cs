namespace SunamoDevCode;

/// <summary>
///     Druhá část je v ToNugets
///     Vložit sem jak toto šílenství s nugety skončí
/// </summary>
public class GlobalUsingsHelper
{
    /// <summary>
    /// Prefix for global using directives in C# source files.
    /// </summary>
    public const string globalUsing = "global using ";
    /// <summary>
    /// Prefix for global directives (used for global symbols with = assignments).
    /// </summary>
    public const string global = "global ";

    /// <summary>
    /// Parses a list of lines to extract global using directives and global symbol definitions.
    /// </summary>
    /// <param name="list">Lines to parse.</param>
    /// <returns>Parsed result containing global usings and symbols.</returns>
    public static ParseGlobalUsingsResult Parse(List<string> list)
    {
        var result = new ParseGlobalUsingsResult();
        foreach (var item in list)
            if (item.StartsWith(globalUsing))
            {
                result.GlobalUsings.Add(item.Replace(globalUsing, "").Replace(";", ""));
            }
            else if (item.StartsWith(global) && item.Contains("&"))
            {
                var kvp = ParseSymbol(item);
                result.GlobalSymbols.Add(kvp.Key, kvp.Value);
            }

        return result;
    }

    private static KeyValuePair<string, string> ParseSymbol(string item)
    {
        item = item.TrimEnd(';');
        var parameter = item.Split('=');
        return new KeyValuePair<string, string>(parameter[0].Replace(global, ""), parameter[1]);
    }
}