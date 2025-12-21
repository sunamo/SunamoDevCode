namespace SunamoDevCode.Aps.Helpers;

public class VersionHelper
{
    public static string RemovePartsWhichIsZero(Version v)
    {
        const string dotZero = ".0";

        var text = v.ToString();
        while (text.EndsWith(dotZero))
        {
            text = SHTrim.Trim(text, dotZero);
        }
        return text;
    }
}