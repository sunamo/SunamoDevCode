namespace SunamoDevCode._sunamo;

internal class AppPaths
{
    public static string GetStartupPath()
    {
        return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
    public static string GetFileInStartupPath(string file)
    {
        return Path.Combine(GetStartupPath(), file);
    }
}