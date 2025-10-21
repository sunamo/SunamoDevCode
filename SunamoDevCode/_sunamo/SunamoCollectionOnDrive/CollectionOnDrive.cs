// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode._sunamo.SunamoCollectionOnDrive;

/// <summary>
///     
/// </summary>
internal sealed class CollectionOnDrive(ILogger logger) : CollectionOnDriveBase<string>(logger)
{
    internal static CollectionOnDrive Dummy = new CollectionOnDrive(NullLogger.Instance);
    internal async Task Load(string path, bool removeDuplicates)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        a.path = path;
        await Load(removeDuplicates);
    }
    internal override async Task Load(bool removeDuplicates)
    {
        if (File.Exists(a.path))
        {
            Clear();
            var rows = SHGetLines.GetLines(await File.ReadAllTextAsync(a.path));
            rows = rows.Where(line => line.Trim() != string.Empty).ToList();
            AddRange(rows);
            if (removeDuplicates)
            {
                var data = this.ToList();
                Clear();
                data = data.Distinct().ToList();
                AddRange(data);
                await Save();
            }
        }
    }
}