
namespace SunamoDevCode._sunamo.SunamoGetFiles;


internal class FSGetFiles
{
    internal static List<string> GetFiles(ILogger logger, string folder2, string mask, SearchOption searchOption, GetFilesArgsDC getFilesArgs = null)
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder2, mask, searchOption).ToList();
    }

    internal static List<string> GetFiles(ILogger logger, string p, bool v)
    {
        return Directory.GetFiles(p, "*", v ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
    }
}
