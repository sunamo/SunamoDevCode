namespace SunamoDevCode;

public class FiltersNotTranslateAble
{
    public static FiltersNotTranslateAble Instance = new();

    public readonly List<string> contains;


    /// <summary>
    ///     Is good include the most files as is possible due to performamce
    /// </summary>
    public readonly List<string> ending;

    public string AssemblyInfo = "AssemblyInfo.cs";
    public string Attributes = "Attributes";
    public string Consts = "Consts";
    public string Credentials = "Credentials";
    public string EnigmaData = "EnigmaData.cs";
    public string Enums = "Enums";
    public string Interfaces = "Interfaces";
    public string Layer = "Layer.cs";
    public string NotTranslateAbleCs = "NotTranslateAble.cs";
    public string NotTranslateAblePp = "NotTranslateAble";
    public string standard = @"\standard\";

    /// <summary>
    ///     in XLF is not available sess coz is in sunamo
    /// </summary>
    public string SunamoXlf = "sunamo\\Xlf";

    /// <summary>
    ///     All which is WithoutDep cant have Xlf
    ///     If yes, I couldn't have Xlf.web and Xlf
    /// </summary>
    public string WithoutDep = "WithoutDep";

    private FiltersNotTranslateAble()
    {
        ending = new List<string>([AssemblyInfo, Layer, NotTranslateAbleCs]);
        contains = new List<string>([SunamoXlf, WithoutDep, Credentials, Interfaces, Enums, NotTranslateAblePp, Consts,
            standard]);
    }
}