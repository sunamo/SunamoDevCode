// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;

public class SolutionsIndexerConsts
{
    public const string BitBucket = "BitBucket";
    public const string ProjectsFolderName = "Projects";
    public const string VisualStudio = "Visual Studio";

    public static List<string> SolutionsExcludeWhileWorkingOnSourceCode = new List<string>(["AllProjectsSearch", "sunamo", "CodeBoxControl"]);
}
