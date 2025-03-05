namespace SunamoDevCode._sunamo;

internal class FSGetFolders
{
    internal static IEnumerable<string> GetFolders(string v)
    {
        return Directory.GetFiles(v);
    }
}