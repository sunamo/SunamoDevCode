// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo;

internal class AppPaths
{
    internal static string GetStartupPath()
    {
        return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
    internal static string GetFileInStartupPath(string file)
    {
        return Path.Combine(GetStartupPath(), file);
    }
}