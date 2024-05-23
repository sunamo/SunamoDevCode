namespace SunamoDevCode;
public class SHReplace
{
    public static string ReplaceAll(string vstup, string zaCo, params string[] co)
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
}
