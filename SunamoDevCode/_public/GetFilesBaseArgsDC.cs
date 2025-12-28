namespace SunamoDevCode._public;

public class GetFilesBaseArgsDC /*: GetFoldersEveryFolderArgs - nevracet - číst koment výše*/
{
    internal bool FollowJunctions = false;
    internal Func<string, bool> DIsJunctionPoint = null;
    internal bool TrimA1AndLeadingBs = false;
}