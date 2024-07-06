namespace SunamoDevCode._sunamo.SunamoGetFiles;


internal class FSGetFiles
{
    internal static List<string> GetFiles(string folder2, string mask, SearchOption searchOption, GetFilesArgsArgs getFilesArgs = null)
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder2, mask, searchOption).ToList();
    }
}
