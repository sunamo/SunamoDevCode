namespace SunamoDevCode.Aps;

public class SellingUCAps
{
    internal static CollectionOnDrive? ToSelling = null;
    public async Task Init(ILogger logger, GetFileSettings getFileSettings)
    {
        ToSelling = new CollectionOnDrive(logger);
        await ToSelling.Load(getFileSettings("ToSelling.txt"), false);
    }
}