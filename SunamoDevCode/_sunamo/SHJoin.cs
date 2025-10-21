// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo;

internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var ts = list.ConvertAll(d => d.ToString());
        return string.Join("\n", ts);
    }
    internal static string JoinNL(List<string> d)
    {
        return string.Join('\n', d);
    }
}