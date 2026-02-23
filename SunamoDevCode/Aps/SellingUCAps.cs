namespace SunamoDevCode.Aps;

/// <summary>
/// Manages the selling use case for APS, loading and maintaining the list of items to sell.
/// </summary>
public class SellingUCAps
{
    internal static CollectionOnDrive? ToSelling = null;
    /// <summary>
    /// Initializes the selling collection by loading it from the ToSelling.txt file.
    /// </summary>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <param name="getFileSettings">Function to resolve file paths by name.</param>
    public async Task Init(ILogger logger, GetFileSettings getFileSettings)
    {
        ToSelling = new CollectionOnDrive(logger);
        await ToSelling.Load(getFileSettings("ToSelling.txt"), false);
    }
}