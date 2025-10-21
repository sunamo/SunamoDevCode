// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

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