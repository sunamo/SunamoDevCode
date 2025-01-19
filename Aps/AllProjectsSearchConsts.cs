namespace SunamoDevCode.Aps;

public partial class AllProjectsSearchConsts
{
    public const string InsertIntoXlfAndConstantCsUCSuMenuItems_Folder_TestFiles = @"E:\vs\AllProjectsSearch.Tests\AllProjectsSearchTestFiles\UC\InsertIntoXlfAndConstantCsUCSuMenuItems";
    static AllProjectsSearchConsts()
    {
    }
    // use InsertIntoXlfAndConstantCsUCSuMenuItems.fileNotTranslate
    //public static readonly List<string> notTranslateAbleFiles = CA.ToListString("AllStrings", "DefaultPaths", "SourceCodePaths", "TemporaryPaths", "SpecialFolders", "AssemblyInfo", "AllChars",
    //                "GeoStates", "UriWebServicesDontTranslate", "PluralConverter", "LoremIpsumGenerator", "RegexHelper",
    //                // Must traslate manually
    //                "XamlGenerator",
    //                "JavaScriptInjection",
    //                "DTFormats",
    //                "GeoCzechRegions",
    //                "GeoStates",
    //                "CSharpGenerator",
    //                // quite c# code
    //                "MSColumnsDB",
    //                // no text, but many quotes etc.
    //                "GeneratorCpp",
    //                // " was not even
    //                "ConstsShared",
    //    // All files with ending NotTranslateAble will be exclude automatically
    //    "*NotTranslateAble",
    //    "AwesomeFontIcons",
    //    "BlogsTemplates"
    //    );
    public const string fnLatestClearTempFiles = "LatestClearTempFiles";
    public static string folderWithTemporaryMovedContentWithoutBackslash = @"E:\code\Temporary moved content\";
    List<string> foldersIgnore = new List<string>(new List<string> { "Addins", "Settings", "Templates", "Visualizers",
    "ArchitectureExplorer", "Backup Files",  "Code Snippets", "StartPages", "Assemblies", "Blend" });
    public static List<string> extCodeElements = new List<string> { ".cs", ".ts", ".dart" };
    public static List<string> extFiles2 = new List<string> { ".ashx", ".aspx", ".xaml", ".cshtml" };
    public const byte forApsPlugins = 9;
}