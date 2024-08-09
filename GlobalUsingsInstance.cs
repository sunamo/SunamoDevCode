namespace SunamoDevCode;

public class GlobalUsingsInstance
{
    private string path;
    private ParseGlobalUsingsResult r;

    public async Task Init(string path)
    {
        this.path = path;
        if (!File.Exists(path)) await File.WriteAllTextAsync(path, "");
        var l = await File.ReadAllLinesAsync(path);

        r = GlobalUsingsHelper.Parse(l.ToList());
    }

    public void AddNewGlobalUsing(string usng)
    {
        if (!r.GlobalUsings.Contains(usng)) r.GlobalUsings.Add(usng);
    }

    public async Task Save()
    {
        StringBuilder sb = new();

        foreach (var item in r.GlobalSymbols)
            sb.AppendLine(GlobalUsingsHelper.global + item.Key + " = " + item.Value + ";");

        foreach (var item in r.GlobalUsings) sb.AppendLine(GlobalUsingsHelper.globalUsing + item + ";");

        await File.WriteAllTextAsync(path, sb.ToString());
    }
}