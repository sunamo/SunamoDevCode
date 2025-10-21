// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Data;

/// <summary>
/// Načte všechny csproj v dir, umí převádět z cesty na název a vice versa
/// </summary>
public static class UniqueCsprojs
{
    static TwoWayDictionary<string, string> dict = new TwoWayDictionary<string, string>();

    public static void AddFromSlnFolder(CsprojsInSolution csp)
    {
        if (dict._d1.Any())
        {
            return;
        }

        foreach (var item in csp.CsprojPaths)
        {
            dict.Add(Path.GetFileNameWithoutExtension(item), item);
        }
    }

    public static string ToName(string path)
    {
#if DEBUG
        if (!dict._d2.ContainsKey(path))
        {
            Debugger.Break();
        }
#endif

        return dict._d2[path];
    }

    /// <summary>
    /// Převede na csproj path, nikoliv na cestu ke složce
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ToPath(string name)
    {
#if DEBUG
        if (!dict._d1.ContainsKey(name))
        {
            Debugger.Break();
        }
#endif

        return dict._d1[name];
    }
}