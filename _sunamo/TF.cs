
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
}