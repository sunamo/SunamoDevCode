namespace SunamoDevCode.Aps.Projs.Results;

public class FindProjectsWhichIsSdkStyleResult
{
    public List<string> CsprojSdkStyleList { get; set; } = null!;

    public List<string> NetstandardList { get; set; } = null!;

    public List<string> NonCsprojSdkStyleList { get; set; } = null!;
}
