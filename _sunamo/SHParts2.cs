namespace SunamoDevCode;

public class SHParts2
{
    public static string RemoveAfterFirst(string t, char ch)
    {
        int dex = t.IndexOf(ch);
        return dex == -1 || dex == t.Length - 1 ? t : t.Substring(0, dex);
    }

    public static string RemoveAfterFirst(string t, string ch)
    {
        int dex = t.IndexOf(ch);
        if (dex == -1 || dex == t.Length - 1)
        {
            return t;
        }

        string vr = t.Remove(dex);
        return vr;
    }
}
