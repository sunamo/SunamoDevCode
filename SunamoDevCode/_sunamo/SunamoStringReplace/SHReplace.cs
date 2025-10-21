// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoStringReplace;

internal class SHReplace
{
    internal static string ReplaceWithIndex(string n, string v, string empty, ref int dx)
    {
        if (dx == -1)
        {
            dx = n.IndexOf(v);
            if (dx != -1)
            {
                n = n.Remove(dx, v.Length);
                n = n.Insert(dx, empty);
            }
        }

        return n;
    }
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

    internal static string ReplaceOnce(string input, string what, string zaco)
    {
        return new Regex(what).Replace(input, zaco, 1);
    }
}