namespace SunamoDevCode._sunamo;

internal class FSGetFolders
{
    public static List<string> GetFoldersEveryFolderWhichContainsFiles(ILogger logger, string d, string masc, SearchOption topDirectoryOnly)
    {
        try
        {
            var f = Directory.GetDirectories(d, "*", topDirectoryOnly/*, new GetFoldersEveryFolderArgs { _trimA1AndLeadingBs = false }*/);
            var result = new List<string>();
            foreach (var item in f)
            {
                var files = Directory.GetFiles(item, masc, topDirectoryOnly).ToList();
                if (files.Count != 0) result.Add(item);
            }
            result = result.ConvertAll(d => d + "\\");
            return result;

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new List<string>();
        }
    }

    internal static IEnumerable<string> GetFolders(string v)
    {
        return Directory.GetFiles(v);
    }
}