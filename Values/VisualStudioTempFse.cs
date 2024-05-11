

namespace SunamoDevCode;


public class VisualStudioTempFse
{
    public static List<string> ignoredForIndexing = null;

    static VisualStudioTempFse()
    {
        ignoredForIndexing = CA.JoinIList<string>(foldersInSolutionToDelete,
foldersInProjectToDelete,
foldersInSolutionDownloaded,
foldersAnywhereToDelete,
foldersInSolutionToKeep,
foldersInProjectToKeep,
foldersInProjectDownloaded);
    }

    //public static List<string> foldersInSolutionToDeleteWithWildcard = new List<string> { "Backup*" };
    /// <summary>
    /// , "packages" cannot be here - after every cleaning, which is most time very often, must open sunamo sln and compile all
    /// bin je např. in _Docker\DockerFromBeginWebForms,Analyzer1\Analyzer1 \Analyzer1 \Analyzer1.Test \Analyzer1.Vsix
    /// </summary>

    //public static List<string> foldersInSolutionToDelete = null;

    //public AllProjectsSearchConsts()
    //{
    //    foldersInSolutionToDelete = foldersInSolutionToDeleteWithWildcard.Concat(foldersInSolutionToDeleteWoWildcard).ToArray();
    //}
    // Is in foldersInSolutionToKeep
    public const string gitFolderName = ".git";
    public const string gitignoreFile = ".gitignore";

    #region 1) To delete
    public static List<string> foldersInSolutionToDelete = new List<string>([".vs", "_UpgradeReport_Files", "obj", "Backup*", "bin", "obj", "TestResults", "MigrationBackup"]);
    public static List<string> foldersInProjectToDelete = new List<string>([".vs", "obj", "bin", "BundleArtifacts"]);
    public static List<string> foldersAnywhereToDelete = new List<string> { };

    public static ExtensionSortedCollection filesInSolutionToDelete = new ExtensionSortedCollection("UpgradeLog*.htm", "UpgradeLog*.htm", "*.suo");
    public static ExtensionSortedCollection filesInProjectToDelete = new ExtensionSortedCollection("*.user");
    public static ExtensionSortedCollection filesAnywhereToDelete = new ExtensionSortedCollection("Thumbs.db", "*.TMP", "*.tmp");
    #endregion

    #region 2) To keep
    public static List<string> foldersInSolutionToKeep = new List<string> { gitFolderName };
    public static List<string> foldersInProjectToKeep = new List<string> { "AppPackages", "Assets", "BundleArtifacts", "Fonts", "MultilingualResources", "Properties", "Service References", "screenshots", "Import Schema Logs", "Stored Procedures", "XMLSchemaCollections", "User Defined Types" };
    public static List<string> foldersAnywhereToKeep = new List<string> { };

    public static List<string> filesInSolutionToKeep = new List<string> { };
    public static List<string> filesInProjectToKeep = new List<string> { "*.suo" };
    public static List<string> filesAnywhereToKeep = new List<string> { "*_TemporaryKey.pfx", "*.png", "*.jpg", "*.bmp", "*.ico", "*.dll", "README.md", "*.resx", "*.mdf", "*.ldf", "project.json", "project.lock.json", "*.nuget.targets" };
    #endregion

    #region 3) Downloaded
    /// <summary>
    /// save meaning as keep
    /// </summary>
    public static List<string> foldersInSolutionDownloaded = new List<string> { "packages", gitFolderName, "node_modules", "x64", ".vs", ".vscode", "lib", ".idea", ".nuget", ".svn", ".*" };
    public static List<string> foldersInProjectDownloaded = new List<string> { };
    public static List<string> foldersAnywhereDownloaded = new List<string> { };

    // next all files starting with dot - ".gitignore", ".browserslistrc"
    // also all .json
    public static ExtensionSortedCollection filesInSolutionDownloaded = new ExtensionSortedCollection(".gitattributes", gitignoreFile, "package-lock.json", "README.md");
    public static List<string> filesInProjectDownloaded = new List<string> { };
    public static List<string> filesAnywhereDownloaded = new List<string> { };
    #endregion

    public static List<string> filesWeb = new List<string>() {  "img",
"ts",
"js",
"css",
"Scripts",
"bower_components"};
}
