namespace SunamoDevCode.Data;

public static class UniqueCsprojs
{
    private static TwoWayDictionary<string, string> dictionary = new();

    public static void AddFromSlnFolder(CsprojsInSolution csprojsInSolution)
    {
        if (dictionary.FirstToSecond.Any())
        {
            return;
        }

        foreach (var csprojPath in csprojsInSolution.CsprojPaths)
        {
            dictionary.Add(Path.GetFileNameWithoutExtension(csprojPath), csprojPath);
        }
    }

    public static string ToName(string csprojPath)
    {

        return dictionary.SecondToFirst[csprojPath];
    }

    // EN: Converts to csproj path, not to folder path
    // CZ: Převede na csproj path, nikoliv na cestu ke složce
    public static string ToPath(string projectName)
    {

        return dictionary.FirstToSecond[projectName];
    }
}
