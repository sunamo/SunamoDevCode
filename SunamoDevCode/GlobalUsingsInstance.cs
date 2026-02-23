namespace SunamoDevCode;

/// <summary>
/// Manages global using declarations in a C# file, supporting adding, removing, and saving.
/// </summary>
public class GlobalUsingsInstance
{
    private string path = null!;
    private ParseGlobalUsingsResult r = null!;

    /// <summary>
    /// Initializes the instance by reading and parsing the global usings file at the given path.
    /// </summary>
    /// <param name="path">Path to the global usings file.</param>
    public async Task Init(string path)
    {
        this.path = path;
        if (!File.Exists(path)) await File.WriteAllTextAsync(path, "");
        var list = await File.ReadAllLinesAsync(path);

        r = GlobalUsingsHelper.Parse(list.ToList());
    }

    /// <summary>
    /// Adds a new global using namespace if it is not already present.
    /// </summary>
    /// <param name="_using">Namespace to add as global using.</param>
    public void AddNewGlobalUsing(string _using)
    {
        if (!r.GlobalUsings.Contains(_using)) r.GlobalUsings.Add(_using);
    }

    /// <summary>
    /// EN: Remove all global usings that start with the specified prefix (case insensitive)
    /// CZ: Odstraň všechny global usings které začínají zadaným prefixem (case insensitive)
    /// </summary>
    /// <param name="prefix">Prefix to match (e.g., "MyNamespace.ExcludedFolder")</param>
    public void RemoveGlobalUsingsStartingWith(string prefix)
    {
        r.GlobalUsings = r.GlobalUsings.Where(ns => !ns.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// Saves the current global usings and symbols back to the file.
    /// </summary>
    public async Task Save()
    {
        StringBuilder stringBuilder = new();

        foreach (var item in r.GlobalSymbols)
            stringBuilder.AppendLine(GlobalUsingsHelper.global + item.Key + " = " + item.Value + ";");

        foreach (var item in r.GlobalUsings) stringBuilder.AppendLine(GlobalUsingsHelper.globalUsing + item + ";");

        await File.WriteAllTextAsync(path, stringBuilder.ToString());
    }
}