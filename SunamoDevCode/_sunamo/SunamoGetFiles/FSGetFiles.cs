namespace SunamoDevCode._sunamo.SunamoGetFiles;

internal class FSGetFiles
{
    internal static List<string> GetFilesEveryFolder(ILogger logger, string fi, string v, SearchOption topDirectoryOnly)
    {
        try
        {
            return Directory.GetFiles(fi, v, topDirectoryOnly).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new List<string>();
        }
    }

    internal static List<string> GetFiles(ILogger logger, string folder2, string mask, bool b, GetFilesArgsDC getFilesArgs = null)
    {
        return GetFiles(logger, folder2, mask, b ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly, getFilesArgs);
    }

#pragma warning disable
    internal static List<string> GetFiles(ILogger logger, string folder2, string mask, SearchOption searchOption, GetFilesArgsDC getFilesArgs = null)
#pragma warning restore
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder2, mask, searchOption).ToList();
    }

#pragma warning disable
    internal static List<string> GetFiles(ILogger logger, string p, bool v)
#pragma warning restore
    {
        return Directory.GetFiles(p, "*", v ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
    }
}