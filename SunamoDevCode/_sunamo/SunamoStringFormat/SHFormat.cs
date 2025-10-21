// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoStringFormat;

internal class SHFormat
{

    internal static string Format4(string v, params Object[] o)
    {
        return string.Format(v, o);
    }
}
