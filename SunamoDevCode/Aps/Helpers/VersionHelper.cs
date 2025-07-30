namespace SunamoDevCode.Aps.Helpers;

public class VersionHelper
{
    public static string RemovePartsWhichIsZero(Version v)
    {
        const string dotZero = ".0";

        var s = v.ToString();
        while (s.EndsWith(dotZero))
        {
            s = SHTrim.Trim(s, dotZero);
        }
        return s;
    }
}