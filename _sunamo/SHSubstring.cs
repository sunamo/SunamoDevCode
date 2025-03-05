namespace SunamoDevCode._sunamo;

internal class SHSubstring
{
    public static string SubstringIfAvailableStart(string name, int v1)
    {
        if (name.Length > v1) return name.Substring(v1);
        return name;
    }
}