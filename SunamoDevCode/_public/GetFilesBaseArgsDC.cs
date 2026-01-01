namespace SunamoDevCode._public;

public class GetFilesBaseArgsDC /*: GetFoldersEveryFolderArgs - nevracet - číst koment výše*/
{
    internal bool FollowJunctions { get; set; } = false;
    internal Func<string, bool> IsJunctionPoint { get; set; } = null;
    internal bool TrimA1AndLeadingBs { get; set; } = false;
}