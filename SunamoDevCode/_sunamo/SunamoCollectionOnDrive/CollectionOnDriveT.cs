// variables names: ok
namespace SunamoDevCode._sunamo.SunamoCollectionOnDrive;

internal sealed class CollectionOnDriveT<T>(ILogger logger) : CollectionOnDriveBase<T>(logger) where T : IParserDC
{
    /// <summary>
    /// Loads collection from file
    /// </summary>
    /// <param name="isRemovingDuplicates">Whether to remove duplicates</param>
    internal async override Task Load(bool isRemovingDuplicates)
    {
        if (File.Exists(args.path))
        {
            var lineNumber = 0;
            foreach (var line in SHGetLines.GetLines(await File.ReadAllTextAsync(args.path)))
            {
                var instance = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(instance), instance);
                instance!.Parse(line);
                await AddWithSave(instance);
                lineNumber++;
            }
        }
    }
}