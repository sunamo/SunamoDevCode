namespace SunamoDevCode;
internal class SHReplace
{
    internal static string ReplaceAll(string vstup, string zaCo, params string[] co)
    {
        foreach (var item in co)
        {
            if (string.IsNullOrEmpty(item))
            {
                return vstup;
            }
        }

        foreach (var item in co)
        {
            vstup = vstup.Replace(item, zaCo);
        }
        return vstup;
    }

    internal static string ReplaceOnce(string a, string v, string empty)
    {
        return a.Replace(v, empty);
    }
}
