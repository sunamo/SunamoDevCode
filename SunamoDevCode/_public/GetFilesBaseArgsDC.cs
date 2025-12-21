namespace SunamoDevCode._public;

public class GetFilesBaseArgsDC /*: GetFoldersEveryFolderArgs - nevracet - číst koment výše*/
{
    internal bool followJunctions = false;
    internal Func<string, bool> dIsJunctionPoint = null;
    internal bool _trimA1AndLeadingBs = false;
}