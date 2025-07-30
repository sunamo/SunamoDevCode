namespace SunamoDevCode.Aps.Args;

public class AskForProjectsOrSlnsArgs
{
    public string hint = string.Empty;
    public string nameSlnFrom = null;
    public object chblTag = null;
    /// <summary>
    /// Whether ask for projects instead of slns
    /// </summary>
    public bool slns = false;
    public bool dialogForMakeSureAboutReplacedWithSessI18n = false;
}