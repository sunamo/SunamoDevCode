namespace SunamoDevCode;

/// <summary>
/// Contains filters for files and paths that should not be processed for translation.
/// </summary>
public class FiltersNotTranslateAble
{
    /// <summary>
    /// Singleton instance of the filters.
    /// </summary>
    public static FiltersNotTranslateAble Instance = new();

    /// <summary>
    /// List of substrings - if a path contains any of these, it should not be translated.
    /// </summary>
    public readonly List<string> Contains;


    /// <summary>
    ///     Is good include the most files as is possible due to performamce
    /// </summary>
    public readonly List<string> Ending;

    /// <summary>
    /// AssemblyInfo.cs filename filter.
    /// </summary>
    public string AssemblyInfo = "AssemblyInfo.cs";
    /// <summary>
    /// Attributes folder/file filter.
    /// </summary>
    public string Attributes = "Attributes";
    /// <summary>
    /// Consts folder/file filter.
    /// </summary>
    public string Consts = "Consts";
    /// <summary>
    /// Credentials folder/file filter.
    /// </summary>
    public string Credentials = "Credentials";
    /// <summary>
    /// EnigmaData.cs filename filter.
    /// </summary>
    public string EnigmaData = "EnigmaData.cs";
    /// <summary>
    /// Enums folder/file filter.
    /// </summary>
    public string Enums = "Enums";
    /// <summary>
    /// Interfaces folder/file filter.
    /// </summary>
    public string Interfaces = "Interfaces";
    /// <summary>
    /// Layer.cs filename filter.
    /// </summary>
    public string Layer = "Layer.cs";
    /// <summary>
    /// NotTranslateAble.cs filename ending filter.
    /// </summary>
    public string NotTranslateAbleCs = "NotTranslateAble.cs";
    /// <summary>
    /// NotTranslateAble path part filter.
    /// </summary>
    public string NotTranslateAblePp = "NotTranslateAble";
    /// <summary>
    /// Standard subfolder path filter.
    /// </summary>
    public string Standard = @"\standard\";

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
        Ending = new List<string>([AssemblyInfo, Layer, NotTranslateAbleCs]);
        Contains = new List<string>([SunamoXlf, WithoutDep, Credentials, Interfaces, Enums, NotTranslateAblePp, Consts,
            Standard]);
    }
}