namespace SunamoDevCode;

public class GlobalUsingsInstance
{
    private string path;
    private ParseGlobalUsingsResult r;

    public async Task Init(string path)
    {
        this.path = path;
        if (!File.Exists(path)) await File.WriteAllTextAsync(path, "");
        var list = await File.ReadAllLinesAsync(path);

        r = GlobalUsingsHelper.Parse(list.ToList());
    }

    public void AddNewGlobalUsing(string usng)
    {
        if (!r.GlobalUsings.Contains(usng)) r.GlobalUsings.Add(usng);
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

    public async Task Save()
    {
        StringBuilder stringBuilder = new();

        foreach (var item in r.GlobalSymbols)
            stringBuilder.AppendLine(GlobalUsingsHelper.global + item.Key + " = " + item.Value + ";");

        foreach (var item in r.GlobalUsings) stringBuilder.AppendLine(GlobalUsingsHelper.globalUsing + item + ";");

        await File.WriteAllTextAsync(path, stringBuilder.ToString());
    }
}