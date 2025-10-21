// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps;

public class SellingUCAps
{
    internal static CollectionOnDrive? toSelling = null;
    public async Task Init(ILogger logger, GetFileSettings getFileSettings)
    {
        toSelling = new CollectionOnDrive(logger);
        await toSelling.Load(getFileSettings("ToSelling.txt"), false);
    }
}