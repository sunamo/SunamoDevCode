namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    /// <summary>
    /// A3 = jestli už jsem vytvořil sln s project distinctions (.Wpf, .Cmd atd.)
    /// 
    /// </summary>
    /// <param name = "sln"></param>
    /// <param name = "projectDistinction"></param>
    /// <param name = "standaloneSlnForProject"></param>
    /// <param name = "addProtectedWhenSelling"></param>
    /// <returns></returns>
    public string ExeToRelease(SolutionFolder sln, string projectDistinction, bool standaloneSlnForProject, bool addProtectedWhenSelling = false, bool publish = false)
    {
#if DEBUG
        if (sln.nameSolution.Contains("GitForDebug") || projectDistinction.Contains("Wpf"))
        {

        }
#endif
        // zde můžu přiřadit jen ty co skutečně existují
        string existingExeReleaseFolder = null;
        // 
        var solutionFolder = sln.fullPathFolder.TrimEnd('\\');
        var exeName = sln.nameSolution;
        string exeNameWithExt = exeName + AllExtensions.exe;
        var projectFolderPath = Path.Combine(solutionFolder, exeName);
        if (!Directory.Exists(projectFolderPath))
        {
            return null;
        }

        var bp = Path.Combine(projectFolderPath, @"bin\");
        string p1 = null;
#if DEBUG
        if (sln.nameSolution.Contains("ConsoleApp1") || projectDistinction.Contains("Wpf"))
        {

        }
#endif
#region MyRegion
        var baseReleaseFolder = Path.Combine(projectFolderPath, @"bin\Release\");

        // EN: Find the highest available .NET version starting from net15.0 and going down
        // CZ: Najdi nejvyšší dostupnou .NET verzi začínající od net15.0 a jdi dolů
        var net7 = FindHighestAvailableNetVersion(baseReleaseFolder, false);
        var net7Windows = FindHighestAvailableNetVersion(baseReleaseFolder, true);

        if (publish)
        {
            if (net7 != null) net7 += "win-x64\\publish\\";
            if (net7Windows != null) net7Windows += "win-x64\\publish\\";
        }

        var b1 = net7 != null && Directory.Exists(net7);
        var b2 = net7Windows != null && Directory.Exists(net7Windows);
        string exePath = null;
        if (b1)
        {
            exePath = Path.Combine(net7, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7, exeNameWithExt);
            }
        }

        if (b2 && existingExeReleaseFolder == null)
        {
            exePath = Path.Combine(net7Windows, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7Windows;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7Windows, exeNameWithExt);
            }
        }

#endregion
        // Kontroluje mi pouze na cestu zda existuje. soubor jako takový nemusí existovat
        //if (File.Exists(net4))
        //{
        //    return null;
        //}
#if DEBUG
        if (sln.nameSolution.Contains("ConsoleApp1") || projectDistinction.Contains("Wpf"))
        {

        }
#endif
        if (existingExeReleaseFolder == null)
        {
            return null;
        }

        var result = Path.Combine(existingExeReleaseFolder, exeNameWithExt);
        return result;
    }

    /// <summary>
    /// EN: Finds the highest available .NET version folder starting from net15.0 and going down
    /// CZ: Najde nejvyšší dostupnou složku .NET verze začínající od net15.0 a jde dolů
    /// </summary>
    /// <param name="baseReleaseFolder">Base release folder path</param>
    /// <param name="isWindows">True for net*-windows, false for net*</param>
    /// <returns>Full path to the found folder or null if none exists</returns>
    private string FindHighestAvailableNetVersion(string baseReleaseFolder, bool isWindows)
    {
        // EN: Start from version 15 and go down to version 5
        // CZ: Začni od verze 15 a jdi dolů k verzi 5
        for (int version = 15; version >= 5; version--)
        {
            var netFolder = isWindows
                ? Path.Combine(baseReleaseFolder, $"net{version}.0-windows\\")
                : Path.Combine(baseReleaseFolder, $"net{version}.0\\");

            if (Directory.Exists(netFolder))
            {
                Console.WriteLine($"Found .NET folder: {netFolder}");
                return netFolder;
            }
        }

        Console.WriteLine($"No .NET folder found in: {baseReleaseFolder} (isWindows: {isWindows})");
        return null;
    }

    private string FindExistingFolderWithRightArchitecture(string net7, string exeNameWithExt)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        var maybe = Path.Combine(net7, "win-x64", exeNameWithExt);
        if (File.Exists(maybe))
        {
            return Path.GetDirectoryName(maybe);
        }

        maybe = Path.Combine(net7, "win-x86", exeNameWithExt);
        if (File.Exists(maybe))
        {
            return Path.GetDirectoryName(maybe);
        }

        return null;
    }

    /// <summary>
    /// Working
    /// </summary>
    public bool HaveGitFolder()
    {
        var f = Path.Combine(fullPathFolder, VisualStudioTempFse.gitFolderName);
        bool vr = Directory.Exists(f);
        return vr;
    }
}