// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoStringTrim;

internal class SHTrim
{
    internal static string Trim(string s, string args)
    {
        s = TrimStart(s, args);
        s = TrimEnd(s, args);

        return s;
    }
    // Metoda TrimLeadingNumbersAtStart byla odstraněna - inlined v ConstsManager.cs:110

    internal static string TrimEnd(string name, string ext)
    {
        while (name.EndsWith(ext)) return name.Substring(0, name.Length - ext.Length);

        return name;
    }

    // Metoda TrimStartAndEnd byla odstraněna - inlined v XmlLocalisationInterchangeFileFormat2.cs:691

    internal static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s))
        {
            v = v.Substring(s.Length);
        }

        return v;
    }

}