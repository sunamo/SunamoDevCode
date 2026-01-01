namespace SunamoDevCode._public;

// todo nemělo by to dědit z GetFoldersEveryFolderArgs? ve vs2 to tak mám
public class GetFilesArgsDC : GetFilesBaseArgsDC
{
    // todo s touhle třídou jsou jen problémy. udělat pořádek co tu má být a co tu nemám.
    internal bool TrimExt { get; set; } = false;
    internal bool TrimA1AndLeadingBs { get; set; } = false;
    internal List<string> ExcludeFromLocationsContains { get; set; } = new List<string>();
    internal bool DontIncludeNewest { get; set; } = false;
    /// <summary>
    /// Insert SunamoDevCodeHelper.RemoveTemporaryFilesVS etc.
    /// </summary>
    internal Action<List<string>> ExcludeWithMethod { get; set; } = null;
    internal bool ByDateOfLastModifiedAsc { get; set; } = false;
    internal Func<string, DateTime?> LastModifiedFromFn { get; set; }
    /// <summary>
    /// 1-7-2020 changed to false, stil forget to mention and method is bad
    /// </summary>
    internal bool UseMascFromExtension { get; set; } = false;
    internal bool Wildcard { get; set; } = false;
}