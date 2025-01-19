namespace SunamoDevCode.Aps;

public class SellingUCAps
{
    public static CollectionOnDrive? toSelling = null;
    public async Task Init(ILogger logger, GetFileSettings getFileSettings)
    {
        toSelling = new CollectionOnDrive(logger);
        await toSelling.Load(getFileSettings("ToSelling.txt"), false);
    }
}