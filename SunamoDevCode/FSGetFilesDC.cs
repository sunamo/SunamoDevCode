// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
// Instance variables refactored according to C# conventions
namespace SunamoDevCode;

public class FSGetFilesDC
{
    public static List<string> GetFilesDC(string slnFolder, string fileMask, SearchOption searchOption, GetFilesDCArgs arguments)
    {
        List<string> resultFiles = new List<string>();
        var projectDirectories = Directory.GetDirectories(slnFolder);
        foreach (var projectDirectory in projectDirectories)
        {
            if (arguments.OnlyIn_Sunamo)
            {
                var sunamoFolder = Path.Combine(projectDirectory, "_sunamo");
                if (Directory.Exists(sunamoFolder))
                {
                    resultFiles.AddRange(Directory.GetFiles(sunamoFolder, fileMask, searchOption));
                }
            }
            else
            {
                resultFiles.AddRange(Directory.GetFiles(projectDirectory, fileMask, searchOption));
            }
        }
        return resultFiles;
    }
}