// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo;

internal class TF
{
    internal static async Task<List<string>?> ReadAllLines(string fn)
    {
        return (await File.ReadAllLinesAsync(fn)).ToList();
    }

    internal static async Task<string?> ReadAllText(string f)
    {
        return await File.ReadAllTextAsync(f);
    }

    internal static async Task WriteAllLines(string item2, List<string> l)
    {
        await File.WriteAllLinesAsync(item2, l);
    }

    internal static async Task WriteAllText(string csProj, string c)
    {
        await File.WriteAllTextAsync(csProj, c);
    }
}