// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoEnumsHelper;

internal class EnumHelper
{
    internal static T Parse<T>(string web, T _def, bool returnDefIfNull = false)
        where T : struct
    {
        if (returnDefIfNull)
        {
            return _def;
        }
        T result;
        if (Enum.TryParse<T>(web, true, out result))
        {
            return result;
        }

        return _def;
    }
}
