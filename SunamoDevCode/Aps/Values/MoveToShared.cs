namespace SunamoDevCode.Aps.Values;

public class MoveToShared : IMoveToShared
{
    public string folder { get; set; } = null;
    public string folderSunamo { get; set; } = null;
    public string folderSunamoWeb { get; set; } = null;
    /// <summary>
    /// web, task and so
    /// </summary>
    public string postfix { get; set; } = "web";
    public string slnFrom { get; set; } = "sunamo";

    public MoveToShared(string folder)
    {
        this.folder = folder;

        // Projects\\ tu musí být kvůli sunamo.cz
        folderSunamo = folder + "Projects\\" + slnFrom + "\\";
        folderSunamoWeb = folder + "Projects\\" + "sunamo." + postfix + "\\";
    }
}